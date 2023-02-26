using System;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox
{
	public static readonly CharacterModels.TokenType[] tokenRewardTypes = new CharacterModels.TokenType[4]
	{
		CharacterModels.TokenType.freshsStereo,
		CharacterModels.TokenType.spikesGuitar,
		CharacterModels.TokenType.trickysHat,
		CharacterModels.TokenType.YutanisUFO
	};

	public static MysteryBoxReward Roll()
	{
		Achievements.Instance.SyncAchievements();
		MysteryBoxReward mysteryBoxReward = new MysteryBoxReward();
		PlayerInfo.Instance.amountOfMysteryBoxesOpened++;
		PlayerInfo.Instance.SaveIfDirty();
		if (PlayerInfo.Instance.amountOfMysteryBoxesOpened == 1)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.token;
			mysteryBoxReward.amount = 1;
			mysteryBoxReward.tokenType = tokenRewardTypes[UnityEngine.Random.Range(0, tokenRewardTypes.Length)];
			return mysteryBoxReward;
		}
		if (PlayerInfo.Instance.amountOfMysteryBoxesOpened == 2)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.amount = 1;
			mysteryBoxReward.powerupType = PowerupType.headstart500;
			return mysteryBoxReward;
		}
		if (PlayerInfo.Instance.amountOfMysteryBoxesOpened == 3)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 600;
			return mysteryBoxReward;
		}
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num < 0.4f)
		{
			CharacterModels.TokenType[] array = new CharacterModels.TokenType[tokenRewardTypes.Length];
			int num2 = 0;
			CharacterModels.TokenType[] array2 = tokenRewardTypes;
			foreach (CharacterModels.TokenType tokenType in array2)
			{
				if (PlayerInfo.Instance.IsTokenUseful(tokenType))
				{
					array[num2] = tokenType;
					num2++;
				}
			}
			if (num2 > 0)
			{
				mysteryBoxReward.type = MysteryBoxRewardType.token;
				mysteryBoxReward.amount = 1;
				mysteryBoxReward.tokenType = array[UnityEngine.Random.Range(0, num2)];
			}
			else
			{
				mysteryBoxReward.type = MysteryBoxRewardType.Coins;
				mysteryBoxReward.amount = UnityEngine.Random.Range(100, 100);
			}
		}
		else if (num < 0.45f)
		{
			Trophies.Trophy[] array3 = Enum.GetValues(typeof(Trophies.Trophy)) as Trophies.Trophy[];
			List<Trophies.Trophy> list = new List<Trophies.Trophy>();
			Trophies.Trophy[] array4 = array3;
			foreach (Trophies.Trophy trophy in array4)
			{
				if (!PlayerInfo.Instance.isTrophyUnlocked(trophy))
				{
					list.Add(trophy);
				}
			}
			if (list.Count > 0)
			{
				mysteryBoxReward.type = MysteryBoxRewardType.trophy;
				mysteryBoxReward.amount = 1;
				mysteryBoxReward.trophyType = list[UnityEngine.Random.Range(0, list.Count)];
			}
			else
			{
				mysteryBoxReward.type = MysteryBoxRewardType.Coins;
				mysteryBoxReward.amount = 100;
			}
		}
		else if (num < 0.69f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = UnityEngine.Random.Range(200, 300);
		}
		else if (num < 0.73f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.headstart500;
			mysteryBoxReward.amount = 1;
		}
		else if (num < 0.8f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 500;
		}
		else if (num < 0.87f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.hoverboard;
			mysteryBoxReward.amount = 2;
		}
		else if (num < 0.92f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 1000;
		}
		else if (num < 0.95f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 1500;
		}
		else if (num < 0.96f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.hoverboard;
			mysteryBoxReward.amount = UnityEngine.Random.Range(0, 7) + 3;
		}
		else if (num < 0.97f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.headstart2000;
			mysteryBoxReward.amount = 1;
		}
		else if (num < 0.98f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 2500;
		}
		else if (num < 0.988f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.hoverboard;
			mysteryBoxReward.amount = 10;
		}
		else if (num < 0.993f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 5000;
		}
		else if (num < 0.998f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.headstart2000;
			mysteryBoxReward.amount = 3;
		}
		else if (num <= 1f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 10000;
		}
		return mysteryBoxReward;
	}
}
