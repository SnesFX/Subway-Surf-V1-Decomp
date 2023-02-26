using System;
using System.Collections;
using UnityEngine;

public class UIGameOverHelper : MonoBehaviour
{
	public UILabel scoreLabel;

	public UILabel collectedCoinLabel;

	public UILabel coinboxLabel;

	public UILabel gettingReadyLabel;

	public CoinBoxSizer coinBoxSizer;

	private Friend[] _friends;

	private int scoreFrom;

	private int scoreTo;

	private int coinboxFrom;

	private int coinboxTo;

	private int collectedCoinsFrom;

	private int collectedCoinsTo;

	private bool countingUpCoins;

	private ScoreCounterSoundPlayer scoreCounterSoundPlayer;

	public GameObject newUpgradesIcon;

	public UILabel newUpgradesText;

	[HideInInspector]
	public bool HasBeenSetupAfterAGame;

	private void Awake()
	{
		scoreCounterSoundPlayer = GetComponent<ScoreCounterSoundPlayer>();
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause && !countingUpCoins)
		{
			ReloadFriends();
		}
	}

	private void OnEnable()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onCoinsChanged = (Action)Delegate.Combine(instance.onCoinsChanged, new Action(OnCoinsChanged));
		Missions instance2 = Missions.Instance;
		instance2.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Combine(instance2.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionComplete));
		UpdateNewUpgradesLabel();
	}

	private void OnDisable()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onCoinsChanged = (Action)Delegate.Remove(instance.onCoinsChanged, new Action(OnCoinsChanged));
		Missions instance2 = Missions.Instance;
		instance2.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Remove(instance2.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionComplete));
	}

	public void SetupBeforeMysteryBox()
	{
		HasBeenSetupAfterAGame = true;
		scoreLabel.text = string.Empty + GameStats.Instance.score;
		collectedCoinLabel.text = string.Empty + GameStats.Instance.coins;
		scoreFrom = GameStats.Instance.score;
	}

	public void SetupAfterMysteryBox()
	{
		if (HasBeenSetupAfterAGame)
		{
			coinBoxSizer.updateAutomatically = false;
			collectedCoinsFrom = GameStats.Instance.coins;
			collectedCoinsTo = 0;
			scoreTo = scoreFrom + GameStats.CoinToScoreConversion(collectedCoinsFrom);
			coinboxFrom = PlayerInfo.Instance.amountOfCoins;
			coinboxTo = coinboxFrom + GameStats.Instance.coins;
			PlayerInfo.Instance.amountOfCoins = coinboxTo;
			if (PlayerInfo.Instance.highestScore != PlayerInfo.Instance.oldHighestScore)
			{
				PlayerInfo.Instance.SetOldestHighestScore();
			}
			PlayerInfo.Instance.highestScore = scoreTo;
			StartCoroutine(CountUpCoins());
			HasBeenSetupAfterAGame = false;
		}
	}

	private void UpdateNewUpgradesLabel()
	{
		int numberOfAffordableUpgrades = PlayerInfo.Instance.GetNumberOfAffordableUpgrades();
		if (numberOfAffordableUpgrades > 1)
		{
			newUpgradesIcon.active = true;
			newUpgradesText.gameObject.active = true;
			newUpgradesText.text = numberOfAffordableUpgrades.ToString();
		}
		else
		{
			newUpgradesIcon.active = false;
			newUpgradesText.gameObject.active = false;
		}
	}

	private void OnCoinsChanged()
	{
		UpdateNewUpgradesLabel();
	}

	private void OnMissionComplete(string message)
	{
		UpdateNewUpgradesLabel();
	}

	private void ReloadFriends()
	{
	}

	private void CountUpCompleted()
	{
		ReloadFriends();
	}

	public void FacebookLoggedIn()
	{
		ReloadFriends();
	}

	private IEnumerator CountUpCoins()
	{
		float countFactor = 0f;
		float countTime = Mathf.Lerp(0.3f, 2f, (float)collectedCoinsFrom / 100f);
		countingUpCoins = true;
		yield return new WaitForSeconds(0.5f);
		while (countFactor < 1f)
		{
			scoreCounterSoundPlayer.PlayCoinSound(countFactor);
			countFactor += Time.deltaTime / countTime;
			scoreLabel.text = string.Empty + Mathf.Round(Mathf.SmoothStep(scoreFrom, scoreTo, countFactor));
			coinboxLabel.text = string.Empty + Mathf.Round(Mathf.SmoothStep(coinboxFrom, coinboxTo, countFactor));
			collectedCoinLabel.text = string.Empty + Mathf.Round(Mathf.SmoothStep(collectedCoinsFrom, collectedCoinsTo, countFactor));
			yield return null;
		}
		scoreCounterSoundPlayer.StopScoreSound();
		scoreLabel.text = string.Empty + scoreTo;
		coinboxLabel.text = string.Empty + coinboxTo;
		collectedCoinLabel.text = string.Empty + collectedCoinsTo;
		coinBoxSizer.updateAutomatically = true;
		countingUpCoins = false;
		CountUpCompleted();
	}
}
