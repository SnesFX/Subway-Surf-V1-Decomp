using System;
using UnityEngine;

public class PurchaseHandler
{
	private Action _onUpgradePurchase;

	private static PurchaseHandler _instance;

	public static PurchaseHandler Instance
	{
		get
		{
			return _instance ?? (_instance = new PurchaseHandler());
		}
	}

	public void AddOnUpgradePurchase(Action handler)
	{
		_onUpgradePurchase = (Action)Delegate.Combine(_onUpgradePurchase, handler);
	}

	public void RemoveOnUpgradePurchase(Action handler)
	{
		if (_onUpgradePurchase != null)
		{
			_onUpgradePurchase = (Action)Delegate.Remove(_onUpgradePurchase, handler);
		}
	}

	public void PurchaseCharacter(CharacterModels.ModelType modelType, UICharacterBuyButton sender)
	{
		CharacterModels.Model model = CharacterModels.modelData[modelType];
		if (model.unlockType != CharacterModels.UnlockType.coins)
		{
			Debug.LogWarning("Cannot buy character with unlocktype: " + model.unlockType);
			sender.PurchaseFailure();
			return;
		}
		if (model.tokenType.Length != 1)
		{
			Debug.LogWarning("Tokenlist for character model unlockable with coins must be of length 1. Character with wrong tokenlist is: " + modelType);
			sender.PurchaseFailure();
			return;
		}
		int price = model.Price;
		if (PlayerInfo.Instance.amountOfCoins >= price)
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, price);
			PlayerInfo.Instance.CollectToken(model.tokenType[0], price);
			PlayerInfo.Instance.amountOfCoins -= price;
			sender.PurchaseSuccessful();
			switch (modelType)
			{
			case CharacterModels.ModelType.ninja:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.HaveNinja);
				Debug.Log("Achievements " + modelType);
				break;
			case CharacterModels.ModelType.frizzy:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.HaveBlackGirl);
				Debug.Log("Achievements " + modelType);
				break;
			case CharacterModels.ModelType.frank:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.HaveFrank);
				Debug.Log("Achievements " + modelType);
				break;
			case CharacterModels.ModelType.king:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.HaveKing);
				Debug.Log("Achievements " + modelType);
				break;
			case CharacterModels.ModelType.lucy:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.HavePinkGirl);
				Debug.Log("Achievements " + modelType);
				break;
			}
			PlayerInfo.Instance.SaveIfDirty();
		}
		else
		{
			InAppHelper.Instance.SetupNativePopup(price);
			sender.PurchaseFailure();
		}
	}

	public void PurchaseHoverboard(BuyHoverboardButton sender)
	{
		int price = Upgrades.upgrades[PowerupType.hoverboard].getPrice(0);
		if (PlayerInfo.Instance.amountOfCoins >= price)
		{
			PlayerInfo.Instance.IncreaseUpgradeAmount(PowerupType.hoverboard);
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, price);
			PlayerInfo.Instance.amountOfCoins -= price;
			sender.PurchaseSuccessful();
			Action onUpgradePurchase = _onUpgradePurchase;
			if (onUpgradePurchase != null)
			{
				onUpgradePurchase();
			}
			PlayerInfo.Instance.SaveIfDirty();
		}
		else
		{
			InAppHelper.Instance.SetupNativePopup(price);
			sender.PurchaseFailure();
		}
	}

	public void PurchaseUpgrade(PowerupType type, BuyButtonIngame sender)
	{
		int num = ((Upgrades.upgrades[type].numberOfTiers != 0) ? Upgrades.upgrades[type].getPrice(PlayerInfo.Instance.GetCurrentTier(type) + 1) : Upgrades.upgrades[type].getPrice(0));
		if (PlayerInfo.Instance.amountOfCoins >= num)
		{
			switch (type)
			{
			case PowerupType.hoverboard:
			case PowerupType.headstart500:
			case PowerupType.headstart2000:
				PlayerInfo.Instance.IncreaseUpgradeAmount(type);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				if (type == PowerupType.headstart2000)
				{
					Missions.Instance.PlayerDidThis(Missions.MissionTarget.HaveHeadStartLarge);
				}
				break;
			case PowerupType.jetpack:
			case PowerupType.supersneakers:
			case PowerupType.coinmagnet:
			case PowerupType.letters:
			case PowerupType.doubleMultiplier:
				PlayerInfo.Instance.IncreasePowerupTier(type);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.HaveUpgrades);
				break;
			case PowerupType.mysterybox:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BuyMysterybox);
				break;
			case PowerupType.skipmission1:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				break;
			case PowerupType.skipmission2:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				break;
			case PowerupType.skipmission3:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				break;
			}
			PlayerInfo.Instance.amountOfCoins -= num;
			sender.PurchaseSuccessful();
			Action onUpgradePurchase = _onUpgradePurchase;
			if (onUpgradePurchase != null)
			{
				onUpgradePurchase();
			}
			PlayerInfo.Instance.SaveIfDirty();
		}
		else
		{
			InAppHelper.Instance.SetupNativePopup(num);
			sender.PurchaseFailure();
		}
	}
}
