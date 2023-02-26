using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenController : MonoBehaviour
{
	public enum SlideInType
	{
		Mission = 0,
		MissionSet = 1,
		Letters = 2,
		Character = 3,
		LettersComplete = 4
	}

	private class SlideIn
	{
		public SlideInType type;

		public string payload = string.Empty;
	}

	private const string INGAMEUI_SCREENNAME = "IngameUI";

	private static UIScreenController _instance;

	public GameObject backgroundAnchor;

	public GameObject overlayAnchor;

	public GameObject popupAnchor;

	public GameObject superPopupAnchor;

	public Camera Camera3D;

	public GameObject MenuElements3D;

	public bool LoadMenuOnStart;

	public UIFont FloatingTextFont;

	private Camera mainCamera;

	public Action<bool> OnChangedScreen;

	private static readonly List<string> PAYOUT_DISALLOWED_SCREENS = new List<string> { "IngameUI" };

	private bool _facebookPayoutPopupQueued;

	private bool _gameCenterPayoutPopupQueued;

	private Dictionary<string, GameObject> _cachedScreens = new Dictionary<string, GameObject>();

	private Stack<string> _screenStack = new Stack<string>(20);

	private Queue<string> _popupQueue = new Queue<string>(15);

	private bool _popupActive;

	private bool _upgradesWasShown;

	private float _timescaleBeforeUpgradeUI = 1f;

	private List<string> _screenNamesWithoutBackground = new List<string> { "FrontUI", "IngameUI" };

	private List<string> _screenNamesWithOnlineVersion = new List<string> { "LeaderboardUI", "FriendsUI" };

	public UISlideInMissionHelper missionSlideIn;

	public UISlideInMissionSetHelper missionSetSlideIn;

	public UISlideInLettersHelper lettersSlideIn;

	public UISlideInCharacterUnlock characterSlideIn;

	public UISlideIn lettersCompleteSlideIn;

	public UISlideInRankHelper rankSlideIn;

	private Queue<SlideIn> _slideInQueue = new Queue<SlideIn>(15);

	private bool slideInActive;

	public AudioClipInfo slideInSound;

	public AudioClipInfo slideInFanfare;

	public UIMessageHelper messageHelper;

	private Queue<string> _messageQueue = new Queue<string>();

	private bool messageShowing;

	public GameObject inAppPurchaseOverlay;

	public static UIScreenController Instance
	{
		get
		{
			return _instance ?? (_instance = UnityEngine.Object.FindObjectOfType(typeof(UIScreenController)) as UIScreenController);
		}
	}

	private void Awake()
	{
		Missions instance = Missions.Instance;
		instance.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Combine(instance.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionCompleted));
		Missions instance2 = Missions.Instance;
		instance2.onMissionSetComplete = (Missions.MissionSetCompleteHandler)Delegate.Combine(instance2.onMissionSetComplete, new Missions.MissionSetCompleteHandler(OnMissionSetCompleted));
		PlayerInfo instance3 = PlayerInfo.Instance;
		instance3.OnPickedUpLetter = (Action)Delegate.Combine(instance3.OnPickedUpLetter, new Action(OnLetterPickedUp));
		PlayerInfo instance4 = PlayerInfo.Instance;
		instance4.OnTokenCollected = (Action<CharacterModels.TokenType>)Delegate.Combine(instance4.OnTokenCollected, new Action<CharacterModels.TokenType>(OnTokenPickUp));
		Achievements.Instance.SyncAchievements();
		bool flag = true;
		bool[] unlockedTrophies = PlayerInfo.Instance.GetUnlockedTrophies();
		for (int i = 0; i < unlockedTrophies.Length; i++)
		{
			if (!unlockedTrophies[i])
			{
				flag = false;
			}
		}
		if (flag)
		{
		}
	}

	private void onDestroy()
	{
		Missions instance = Missions.Instance;
		instance.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Remove(instance.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionCompleted));
		Missions instance2 = Missions.Instance;
		instance2.onMissionSetComplete = (Missions.MissionSetCompleteHandler)Delegate.Remove(instance2.onMissionSetComplete, new Missions.MissionSetCompleteHandler(OnMissionSetCompleted));
		PlayerInfo instance3 = PlayerInfo.Instance;
		instance3.OnPickedUpLetter = (Action)Delegate.Remove(instance3.OnPickedUpLetter, new Action(OnLetterPickedUp));
		PlayerInfo instance4 = PlayerInfo.Instance;
		instance4.OnTokenCollected = (Action<CharacterModels.TokenType>)Delegate.Remove(instance4.OnTokenCollected, new Action<CharacterModels.TokenType>(OnTokenPickUp));
	}

	private void Start()
	{
		HideInAppPurchaseOverlay();
		if (LoadMenuOnStart)
		{
			ShowMainMenu();
		}
		PlayerInfo.Instance.BragCompleted();
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			if (_screenStack.Count > 0 && _screenStack.Peek() == "IngameUI")
			{
				PushScreen(null, "PauseUI");
			}
			PlayerInfo.Instance.SaveIfDirty();
		}
	}

	public void FacebookLogIn(bool loggedIn)
	{
		EtceteraAndroid.hideProgressDialog();
		if (_screenStack.Count <= 0)
		{
			return;
		}
		if (_screenStack.Peek() == "FriendsUI_offline")
		{
			if (loggedIn)
			{
				_SwitchScreen("FriendsUI_online");
			}
		}
		else if (_screenStack.Peek() == "FriendsUI_online")
		{
			if (loggedIn)
			{
				_SwitchScreen("FriendsUI_online");
			}
		}
		else if (_screenStack.Peek() == "LeaderboardUI_online")
		{
			if (loggedIn)
			{
				_SwitchScreen("LeaderboardUI_online");
			}
		}
		else if (_screenStack.Peek() == "LeaderboardUI_offline")
		{
			if (loggedIn)
			{
				_SwitchScreen("LeaderboardUI_online");
			}
		}
		else if (_screenStack.Peek() == "GameoverUI" && loggedIn)
		{
			_cachedScreens["GameoverUI"].GetComponent<UIGameOverHelper>().FacebookLoggedIn();
		}
	}

	public void ShowMainMenu()
	{
		string screenName = "FrontUI";
		_ActivateScreen(screenName);
	}

	public void GameOverTriggered()
	{
		Missions.Instance.inRun = false;
		string screenName = "GameoverUI";
		_ActivateScreen(screenName);
	}

	public void QueueMessage(string message)
	{
		Debug.Log("Showing message: " + message);
		_QueueMessage(message);
	}

	public void GoToMainMenuFromGame(GameObject sender)
	{
		if (Game.Instance != null)
		{
			Missions.Instance.inRun = false;
			Game.Instance.StartTopMenu();
			Game.Instance.TriggerPause(false);
		}
		_ActivateScreen("FrontUI");
	}

	public void PushScreen(GameObject sender)
	{
		PushScreen(sender, string.Empty);
	}

	public void PushScreen(GameObject sender, string screenOverride)
	{
		string empty = string.Empty;
		empty = ((!(screenOverride != string.Empty)) ? sender.GetComponent<UIButtonChangeScreen>().ScreenNameToOpen : screenOverride);
		if (_screenNamesWithOnlineVersion.Contains(empty))
		{
		}
		_ActivateScreen(empty);
		if (empty == "IngameUI")
		{
			_cachedScreens[empty].GetComponent<UIIngameUpdater>().TriggerInGameUI();
			if (!UIIngameUpdater.isCountingDown())
			{
				Missions.Instance.inRun = true;
			}
		}
		else if (empty == "PauseUI" && Game.Instance != null)
		{
			Game.Instance.TriggerPause(true);
		}
	}

	public void SwitchScreen(GameObject sender)
	{
		SwitchScreen(sender, string.Empty);
	}

	public void SwitchScreen(GameObject sender, string screenOverride = "")
	{
		string empty = string.Empty;
		empty = ((!(screenOverride != string.Empty)) ? sender.GetComponent<UIButtonChangeScreen>().ScreenNameToOpen : screenOverride);
		if (_screenNamesWithOnlineVersion.Contains(empty))
		{
		}
		_SwitchScreen(empty);
	}

	public void CheckForLoginBonus()
	{
	}

	public void BackToPrevious()
	{
		_BackToPreviousScreen();
	}

	private void QueueFacebookPayoutPopup()
	{
		_facebookPayoutPopupQueued = true;
		TryQueuePayoutPopups();
	}

	private void QueueGameCenterPayoutPopup()
	{
		_gameCenterPayoutPopupQueued = true;
		TryQueuePayoutPopups();
	}

	private void TryQueuePayoutPopups()
	{
		if ((!_facebookPayoutPopupQueued && !_gameCenterPayoutPopupQueued) || _screenStack == null || _screenStack.Count <= 0)
		{
			return;
		}
		string item = _screenStack.Peek();
		if (!PAYOUT_DISALLOWED_SCREENS.Contains(item))
		{
			if (_facebookPayoutPopupQueued)
			{
				QueuePopup("FacebookPayoutPopup");
				_facebookPayoutPopupQueued = false;
			}
			if (_gameCenterPayoutPopupQueued)
			{
				_gameCenterPayoutPopupQueued = false;
			}
		}
		else
		{
			Debug.Log("Cannot show payout popup on this screen");
		}
	}

	public void QueuePopup(string popupName)
	{
		_QueuePopup(popupName);
	}

	public void QueuePopup(GameObject sender)
	{
		string screenNameToOpen = sender.GetComponent<UIButtonChangeScreen>().ScreenNameToOpen;
		_QueuePopup(screenNameToOpen);
	}

	public void QueueMysteryBox()
	{
		string text = string.Empty;
		if (_popupQueue.Count > 0)
		{
			Debug.Log("Peeking at popup queue: " + _popupQueue.Peek());
			text = _popupQueue.Peek();
			_RemovePopup();
		}
		_QueuePopup("MysteryBoxPopup");
		if (text != string.Empty)
		{
			_QueuePopup(text);
			Debug.Log("Queueing " + text);
		}
	}

	public void ClosePopup()
	{
		_RemovePopup();
	}

	public void SpawnCollectText(Vector3 startPosition, string text)
	{
		UILabel uILabel = NGUITools.AddWidget<UILabel>(superPopupAnchor);
		uILabel.text = text;
		uILabel.transform.position = new Vector3(startPosition.x, startPosition.y, uILabel.cachedTransform.position.z);
		uILabel.font = FloatingTextFont;
		uILabel.color = new Color(50f / 51f, 66f / 85f, 0.23529412f, 0f);
		uILabel.cachedTransform.localScale = new Vector3(17f, 17f, 1f);
		StartCoroutine(AnimateCollectText(uILabel));
	}

	private IEnumerator AnimateCollectText(UILabel collectText)
	{
		Vector3 fromLocalPosition = collectText.transform.localPosition;
		Vector3 toLocalPosition = new Vector3(fromLocalPosition.x, fromLocalPosition.y + 50f, fromLocalPosition.z);
		yield return StartCoroutine(AnimateAlpha(collectText, 0.1f, 1f));
		StartCoroutine(MoveTransform(collectText.cachedTransform, 1f, toLocalPosition));
		yield return new WaitForSeconds(0.8f);
		StartCoroutine(AnimateAlpha(collectText, 0.2f, 0f));
		yield return new WaitForSeconds(0.25f);
		UnityEngine.Object.Destroy(collectText.gameObject);
	}

	private IEnumerator AnimateAlpha(UILabel label, float duration, float toAlpha)
	{
		float fromAlpha = label.alpha;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			label.alpha = Mathf.Lerp(fromAlpha, toAlpha, factor2);
			yield return null;
		}
	}

	private IEnumerator MoveTransform(Transform trans, float duration, Vector3 toPos)
	{
		Vector3 fromPos = trans.localPosition;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			trans.localPosition = Vector3.Lerp(fromPos, toPos, factor2);
			yield return null;
		}
	}

	private void _ActivateScreen(string screenName)
	{
		if (_cachedScreens.ContainsKey(screenName))
		{
			GameObject gameObject = _cachedScreens[screenName];
			gameObject.SetActiveRecursively(true);
			gameObject.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
			if (_screenStack.Contains(screenName))
			{
				while (_screenStack.Contains(screenName) && _screenStack.Peek() != screenName)
				{
					string key = _screenStack.Pop();
					_cachedScreens[key].SetActiveRecursively(false);
				}
			}
			else
			{
				_cachedScreens[_screenStack.Peek()].SetActiveRecursively(false);
				_screenStack.Push(screenName);
			}
		}
		else
		{
			GameObject prefab = Resources.Load("Prefabs/Screens/" + screenName, typeof(GameObject)) as GameObject;
			GameObject gameObject2 = NGUITools.AddChild(overlayAnchor, prefab);
			_cachedScreens.Add(screenName, gameObject2);
			if (_screenStack.Count > 0)
			{
				_cachedScreens[_screenStack.Peek()].SetActiveRecursively(false);
			}
			_screenStack.Push(screenName);
			gameObject2.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		}
		UIModelController.Instance.ClearModels();
		switch (screenName)
		{
		case "GameoverUI":
			UIModelController.Instance.ActivateGameOverModel();
			_cachedScreens[screenName].GetComponent<UIGameOverHelper>().SetupBeforeMysteryBox();
			if (PlayerInfo.Instance.mysteryBoxesToUnlock != 0)
			{
				_QueuePopup("MysteryBoxPopup");
			}
			else
			{
				_cachedScreens[screenName].GetComponent<UIGameOverHelper>().SetupAfterMysteryBox();
			}
			break;
		case "CharacterUI":
			UIModelController.Instance.ActivateCharacterModel();
			break;
		case "FriendsUI_online":
		case "LeaderboardUI_online":
			Debug.Log("Getting an online screen!");
			break;
		case "FriendsUI_offline":
		case "LeaderboardUI_offline":
			_cachedScreens[screenName].SendMessage("InitOfflineScreen", SendMessageOptions.DontRequireReceiver);
			break;
		}
		if (screenName == "UpgradesUI_shop")
		{
			_upgradesWasShown = true;
			_timescaleBeforeUpgradeUI = Time.timeScale;
			Time.timeScale = 0f;
		}
		else if (_upgradesWasShown)
		{
			_upgradesWasShown = false;
			Time.timeScale = _timescaleBeforeUpgradeUI;
		}
		SetBackground(!_screenNamesWithoutBackground.Contains(screenName));
		Action<bool> onChangedScreen = OnChangedScreen;
		if (onChangedScreen != null)
		{
			onChangedScreen(screenName == "FrontUI");
		}
		ScreenDidChange(screenName);
	}

	private void _SwitchScreen(string screenName)
	{
		string key = _screenStack.Pop();
		_cachedScreens[key].SetActiveRecursively(false);
		_ActivateScreen(screenName);
	}

	private void _BackToPreviousScreen()
	{
		if (_screenStack.Count > 1)
		{
			string key = _screenStack.Pop();
			_cachedScreens[key].SetActiveRecursively(false);
			key = _screenStack.Peek();
			_cachedScreens[key].SetActiveRecursively(true);
			SetBackground(!_screenNamesWithoutBackground.Contains(key));
			ScreenDidChange(key);
		}
		else
		{
			Debug.LogError("Tried to remove the only screen in the stack. You dun goofed.", this);
		}
	}

	private void ScreenDidChange(string newScreenName)
	{
		messageHelper.SetTemporaryHidden(newScreenName != "IngameUI");
		ChartBoostManager.instance.GameScreenChanged(newScreenName);
		TryQueuePayoutPopups();
		if (newScreenName == "FrontUI")
		{
			HouseKeeper.RefreshOnlineSettingsIfNeeded();
		}
	}

	private void _QueuePopup(string name)
	{
		_popupQueue.Enqueue(name);
		if (!_popupActive)
		{
			_ActivateNextPopup();
		}
	}

	private void _ActivateNextPopup()
	{
		if (_popupQueue.Count > 0)
		{
			_PauseAnimations(true, MenuElements3D.transform);
			NGUITools.SetActive(MenuElements3D, false);
			string text = _popupQueue.Peek();
			if (!_cachedScreens.ContainsKey(text))
			{
				GameObject prefab = Resources.Load("Prefabs/Popups/" + text, typeof(GameObject)) as GameObject;
				GameObject value = NGUITools.AddChild(popupAnchor, prefab);
				_cachedScreens.Add(text, value);
			}
			_cachedScreens[text].SetActiveRecursively(true);
			if (text == "MysteryBoxPopup")
			{
				if (mainCamera == null)
				{
					mainCamera = Camera.main;
				}
				mainCamera.enabled = false;
				_cachedScreens[text].GetComponent<MysteryBoxHandler>().SetupMysteryBoxScreen();
			}
			_cachedScreens[text].BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
			_popupActive = true;
			if (text == "UpgradesUI_quick")
			{
				_upgradesWasShown = true;
				_timescaleBeforeUpgradeUI = Time.timeScale;
				Time.timeScale = 0f;
			}
			else if (_upgradesWasShown)
			{
				_upgradesWasShown = false;
				Time.timeScale = _timescaleBeforeUpgradeUI;
			}
			Action<bool> onChangedScreen = OnChangedScreen;
			if (onChangedScreen != null)
			{
				onChangedScreen(false);
			}
		}
		else
		{
			NGUITools.SetActive(MenuElements3D, true);
			_PauseAnimations(false, MenuElements3D.transform);
			if (_upgradesWasShown)
			{
				_upgradesWasShown = false;
				Time.timeScale = _timescaleBeforeUpgradeUI;
			}
		}
	}

	private void _PauseAnimations(bool pause, Transform trans)
	{
		foreach (Transform tran in trans)
		{
			_PauseAnimations(pause, tran);
		}
		if (trans.GetComponent<CharacterModel>() != null)
		{
			if (pause)
			{
				trans.GetComponent<CharacterModel>().StopIdleAnimations();
			}
			else
			{
				trans.GetComponent<CharacterModel>().StartIdleAnimations();
			}
		}
	}

	private void _RemovePopup()
	{
		if (_popupQueue.Count < 1)
		{
			return;
		}
		string text = _popupQueue.Dequeue();
		_cachedScreens[text].SetActiveRecursively(false);
		_popupActive = false;
		Action<bool> onChangedScreen = OnChangedScreen;
		if (onChangedScreen != null)
		{
			onChangedScreen(true);
		}
		_ActivateNextPopup();
		if (text == "MysteryBoxPopup")
		{
			if (mainCamera != null)
			{
				mainCamera.enabled = true;
			}
			if (_screenStack.Peek() == "GameoverUI")
			{
				_cachedScreens[_screenStack.Peek()].GetComponent<UIGameOverHelper>().SetupAfterMysteryBox();
			}
		}
	}

	private void SetBackground(bool state)
	{
		string text = "NotebookPanel";
		if (state)
		{
			if (!_cachedScreens.ContainsKey(text))
			{
				GameObject prefab = Resources.Load("Prefabs/Screens/" + text, typeof(GameObject)) as GameObject;
				GameObject value = NGUITools.AddChild(backgroundAnchor, prefab);
				_cachedScreens.Add(text, value);
			}
			_cachedScreens[text].SetActiveRecursively(true);
			_cachedScreens[text].BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		}
		else if (_cachedScreens.ContainsKey(text))
		{
			_cachedScreens[text].SetActiveRecursively(false);
		}
	}

	private void OnMissionCompleted(string message)
	{
		Debug.Log(message);
		QueueSlideIn(SlideInType.Mission, message);
	}

	private void OnMissionSetCompleted()
	{
		Debug.Log("Mission Set Completed, increase multiplier");
		Missions.Instance.PlayerDidThis(Missions.MissionTarget.ReachMissionSet);
		QueueSlideIn(SlideInType.MissionSet, string.Empty);
	}

	private void OnLetterPickedUp()
	{
		QueueSlideIn(SlideInType.Letters, string.Empty);
	}

	private void OnTokenPickUp(CharacterModels.TokenType type)
	{
		foreach (int value in Enum.GetValues(typeof(CharacterModels.ModelType)))
		{
			CharacterModels.Model model = CharacterModels.modelData[(CharacterModels.ModelType)value];
			bool flag = false;
			CharacterModels.TokenType[] tokenType = model.tokenType;
			foreach (CharacterModels.TokenType tokenType2 in tokenType)
			{
				if (tokenType2 == type)
				{
					flag = true;
					break;
				}
			}
			if (flag && PlayerInfo.Instance.GetCollectedTokens((CharacterModels.ModelType)value) == model.Price)
			{
				QueueSlideIn(SlideInType.Character, model.modelName);
				Debug.Log("Queue character slidein");
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Id", type.ToString());
			}
		}
	}

	public void QueueSlideIn(SlideInType type, string payload = "")
	{
		SlideIn slideIn = new SlideIn();
		slideIn.type = type;
		slideIn.payload = payload;
		_slideInQueue.Enqueue(slideIn);
		if (!slideInActive)
		{
			_ShowSlideIn();
		}
	}

	public void ReadyForNextSlide()
	{
		slideInActive = false;
		if (!slideInActive)
		{
			_ShowSlideIn();
		}
	}

	private void _ShowSlideIn()
	{
		if (_slideInQueue.Count > 0)
		{
			SlideIn slideIn = _slideInQueue.Dequeue();
			if (slideIn.type == SlideInType.Mission)
			{
				So.Instance.playSound(slideInFanfare);
				missionSlideIn.SetupSlideInMission(slideIn.payload);
			}
			else if (slideIn.type == SlideInType.MissionSet)
			{
				So.Instance.playSound(slideInFanfare);
				missionSetSlideIn.SetupSlideInMissionSet(PlayerInfo.Instance.rawMultiplier);
			}
			else if (slideIn.type == SlideInType.Letters)
			{
				So.Instance.playSound(slideInSound);
				lettersSlideIn.SetupLetters();
			}
			else if (slideIn.type == SlideInType.Character)
			{
				So.Instance.playSound(slideInSound);
				characterSlideIn.SetupSlideInCharacter(slideIn.payload);
			}
			else if (slideIn.type == SlideInType.LettersComplete)
			{
				So.Instance.playSound(slideInFanfare);
				lettersCompleteSlideIn.SetupSlideIn();
			}
			slideInActive = true;
		}
	}

	public void ReadyForNextMessage()
	{
		messageShowing = false;
		_ShowNextMessage();
	}

	private void _QueueMessage(string message)
	{
		_messageQueue.Enqueue(message);
		if (!messageShowing)
		{
			_ShowNextMessage();
		}
		if (!slideInActive)
		{
			_ShowSlideIn();
		}
	}

	private void _ShowNextMessage()
	{
		if (_messageQueue.Count > 0)
		{
			string message = _messageQueue.Dequeue();
			messageHelper.ShowMessage(message);
			messageShowing = true;
		}
	}

	public void ShowInAppPurchaseOverlay()
	{
		inAppPurchaseOverlay.SetActiveRecursively(true);
		Camera3D.enabled = false;
	}

	public void HideInAppPurchaseOverlay()
	{
		inAppPurchaseOverlay.SetActiveRecursively(false);
		Camera3D.enabled = true;
	}
}
