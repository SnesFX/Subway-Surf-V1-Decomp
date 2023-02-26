using System;
using UnityEngine;

public class UICharacterSelectButton : MonoBehaviour
{
	[SerializeField]
	private UISlicedSprite fillSprite;

	[SerializeField]
	private UILabel label;

	[SerializeField]
	private UILabel characterName;

	private string fillSelect = "button_fill_select";

	private string fillSelected = "button_fill_selected";

	private string fillNotAvailable = "button_fill_info";

	private string textSelect = "SELECT";

	private string textSelected = "SELECTED";

	private string textBuy = "BUY\n{0} COINS";

	private string textProgress = "{0} / {1}";

	private bool isShowingNotAvailableGraphics = true;

	[SerializeField]
	private GameObject notAvailableGraphics;

	[SerializeField]
	private UISprite tokenSprite;

	[SerializeField]
	private UILabel progressLabel;

	private bool isEnabled;

	private BoxCollider col;

	private void OnEnable()
	{
		isShowingNotAvailableGraphics = true;
		OnChangedCurrentlyShownModel();
	}

	private void Awake()
	{
		col = GetComponent<BoxCollider>();
	}

	private void Start()
	{
		UIModelController instance = UIModelController.Instance;
		instance.OnChangedCurrentlyShown = (Action)Delegate.Combine(instance.OnChangedCurrentlyShown, new Action(OnChangedCurrentlyShownModel));
		OnChangedCurrentlyShownModel();
	}

	private void OnDestroy()
	{
		UIModelController instance = UIModelController.Instance;
		instance.OnChangedCurrentlyShown = (Action)Delegate.Remove(instance.OnChangedCurrentlyShown, new Action(OnChangedCurrentlyShownModel));
	}

	private void OnChangedCurrentlyShownModel()
	{
		CharacterModels.ModelType currentlyShownModel = (CharacterModels.ModelType)UIModelController.Instance.currentlyShownModel;
		CharacterModels.Model model = CharacterModels.modelData[currentlyShownModel];
		characterName.text = model.modelName;
		if (PlayerInfo.Instance.currentCharacter == UIModelController.Instance.currentlyShownModel)
		{
			showAndEnable();
			setNotAvalibleVisibility(false);
			fillSprite.spriteName = fillSelected;
			label.text = textSelected;
			col.enabled = false;
		}
		else if (PlayerInfo.Instance.IsCollectionComplete(currentlyShownModel) ? true : false)
		{
			showAndEnable();
			setNotAvalibleVisibility(false);
			fillSprite.spriteName = fillSelect;
			label.text = textSelect;
			col.enabled = true;
		}
		else if (model.unlockType == CharacterModels.UnlockType.tokens)
		{
			showAndEnable();
			fillSprite.spriteName = fillNotAvailable;
			label.text = string.Empty;
			progressLabel.text = string.Format(textProgress, PlayerInfo.Instance.GetCollectedTokens(currentlyShownModel), model.Price);
			tokenSprite.spriteName = CharacterModels.tokenInfo[CharacterModels.modelData[currentlyShownModel].tokenType[0]].sprite2dName;
			setNotAvalibleVisibility(true);
			col.enabled = false;
		}
		else
		{
			hideAndDisable();
		}
	}

	private void hideAndDisable()
	{
		if (isEnabled)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				child.gameObject.active = false;
			}
			setNotAvalibleVisibility(false);
			col.enabled = false;
			isEnabled = false;
		}
	}

	private void showAndEnable()
	{
		if (!isEnabled)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				child.gameObject.active = true;
			}
			setNotAvalibleVisibility(false);
			label.panel.Refresh();
			col.enabled = true;
			isEnabled = true;
		}
	}

	private void setNotAvalibleVisibility(bool val)
	{
		if (val != isShowingNotAvailableGraphics)
		{
			for (int i = 0; i < notAvailableGraphics.transform.childCount; i++)
			{
				Transform child = notAvailableGraphics.transform.GetChild(i);
				child.gameObject.active = val;
			}
			isShowingNotAvailableGraphics = val;
		}
	}
}
