using UnityEngine;

public class Achievements
{
	public const int NUMBEROFACHIEVEMENTS = 27;

	private MissionTemplate[] _achievementTemplates;

	private int[] _currentRunProgressForAchievements = new int[27];

	private string[] _achievementIds = new string[27]
	{
		"Banker", "MagneticCenter", "Score_400000_points.", "OverwhelmingPower", "Simon_says_jump", "ABC", "Happy_Birthday", "Boxing_day", "Welcome_back", "One_legged",
		"Batter_Up", "Tonyhawk", "ItsABird", "Attractive", "pussinboots", "DoubleorNothing", "AllMissionsCompleted", "Elite", "FullThrottle", "TrickyTreasure",
		"MeepMeep", "UnlockKing", "GothGirl", "UnlockNinja", "UnlockFrank", "FoxyCleopatra", "BestFriends"
	};

	private Mission[] _achievementArray = new Mission[27]
	{
		new Mission(Missions.MissionType.EarnCoin, 50000),
		new Mission(Missions.MissionType.CoinsWithMagnet, 2000),
		new Mission(Missions.MissionType.ScoreSingleRun, 400000),
		new Mission(Missions.MissionType.Powerups, 100),
		new Mission(Missions.MissionType.GuardJump, 100),
		new Mission(Missions.MissionType.Letters, 26),
		new Mission(Missions.MissionType.MysteryBoxes, 25),
		new Mission(Missions.MissionType.BuyMysterybox, 10),
		new Mission(Missions.MissionType.DailyQuests, 10),
		new Mission(Missions.MissionType.BumpBarrier, 50),
		new Mission(Missions.MissionType.HoverBoard, 40),
		new Mission(Missions.MissionType.HoverBoardExpire, 20),
		new Mission(Missions.MissionType.JetpackSingleRun, 5),
		new Mission(Missions.MissionType.Magnets, 6),
		new Mission(Missions.MissionType.SuperSneakersSingleRun, 6),
		new Mission(Missions.MissionType.DoubleMultiplierSingleRun, 6),
		new Mission(Missions.MissionType.ReachMissionSet, 29),
		new Mission(Missions.MissionType.HaveUpgrades, 20),
		new Mission(Missions.MissionType.HaveHeadStartLarge, 10),
		new Mission(Missions.MissionType.NoCoinsBeforeScore, 30000),
		new Mission(Missions.MissionType.Headstart, 15),
		new Mission(Missions.MissionType.HaveKing, 1),
		new Mission(Missions.MissionType.HavePinkGirl, 1),
		new Mission(Missions.MissionType.HaveNinja, 1),
		new Mission(Missions.MissionType.HaveFrank, 1),
		new Mission(Missions.MissionType.HaveBlackGirl, 1),
		new Mission(Missions.MissionType.PokeFriend, 10)
	};

	private static Achievements _instance;

	private MissionTemplate[] AchievementTemplates
	{
		get
		{
			if (_achievementTemplates == null)
			{
				_achievementTemplates = new MissionTemplate[_achievementArray.Length];
				for (int i = 0; i < _achievementArray.Length; i++)
				{
					_achievementTemplates[i] = Missions.Instance.GetMissionTemplates()[_achievementArray[i].type];
				}
			}
			return _achievementTemplates;
		}
	}

	public static Achievements Instance
	{
		get
		{
			return _instance ?? (_instance = new Achievements());
		}
	}

	public void LogAction(Missions.MissionTarget myTask, int magnitude)
	{
		PlayerInfo instance = PlayerInfo.Instance;
		for (int i = 0; i < _achievementArray.Length; i++)
		{
			if ((AchievementTemplates[i].singleRun && !Missions.Instance.inRun) || _achievementTemplates[i].missionTarget != myTask)
			{
				continue;
			}
			int num = instance.achievementProgress[i];
			if (AchievementTemplates[i].singleRun && Missions.Instance.inRun && num < _achievementArray[i].goal && _currentRunProgressForAchievements != null)
			{
				num = _currentRunProgressForAchievements[i];
			}
			int num2 = num + magnitude;
			if (AchievementTemplates[i].completeIfLess)
			{
				if (num2 > _achievementArray[i].goal)
				{
					if (AchievementTemplates[i].singleRun && Missions.Instance.inRun)
					{
						if (_currentRunProgressForAchievements != null)
						{
							_currentRunProgressForAchievements[i] = num2;
						}
					}
					else
					{
						instance.achievementProgress[i] = num2;
					}
					break;
				}
				if (AchievementTemplates[i].singleRun && _currentRunProgressForAchievements != null)
				{
					_currentRunProgressForAchievements[i] = num2;
				}
				instance.achievementProgress[i] = _achievementArray[i].goal;
				if (num > _achievementArray[i].goal)
				{
					AchievementComplete(i);
				}
			}
			else if (num2 < _achievementArray[i].goal)
			{
				if (AchievementTemplates[i].singleRun)
				{
					if (_currentRunProgressForAchievements != null)
					{
						_currentRunProgressForAchievements[i] = num2;
					}
				}
				else
				{
					instance.achievementProgress[i] = num2;
				}
			}
			else
			{
				instance.achievementProgress[i] = _achievementArray[i].goal;
				if (num < _achievementArray[i].goal)
				{
					AchievementComplete(i);
				}
			}
			break;
		}
	}

	private void AchievementComplete(int achievementIndex, bool ForceComplete = false)
	{
		Debug.Log(string.Concat("Complete achievement ", _achievementArray[achievementIndex].type, " ", _achievementArray[achievementIndex].goal));
		if (ForceComplete)
		{
			PlayerInfo.Instance.achievementProgress[achievementIndex] = _achievementArray[achievementIndex].goal;
		}
		if (_achievementIds.Length > achievementIndex && achievementIndex > -1)
		{
		}
		else
		{
			Debug.LogWarning("no achievement id match for achievement index " + achievementIndex + ". could not set achievement");
		}
	}

	public void EmthyCurrentProgress()
	{
		_currentRunProgressForAchievements = new int[27];
	}

	public void CheckAllAchievementsCompleteAndIncrement()
	{
		if (_currentRunProgressForAchievements == null)
		{
			return;
		}
		PlayerInfo instance = PlayerInfo.Instance;
		for (int i = 0; i < _achievementArray.Length; i++)
		{
			if (_achievementTemplates[i].singleRun && _achievementTemplates[i].completeIfLess && _currentRunProgressForAchievements[i] < _achievementArray[i].goal)
			{
				instance.achievementProgress[i] = _achievementArray[i].goal;
				AchievementComplete(i);
			}
		}
		_currentRunProgressForAchievements = null;
	}

	public void OverWriteAchievementsProgress(Missions.MissionTarget myTask, int overWrite)
	{
		PlayerInfo instance = PlayerInfo.Instance;
		for (int i = 0; i < _achievementArray.Length; i++)
		{
			if (AchievementTemplates[i].missionTarget == myTask)
			{
				instance.achievementProgress[i] = overWrite;
				if (overWrite >= _achievementArray[i].goal)
				{
					AchievementComplete(i);
				}
			}
		}
	}

	public void SyncAchievements()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		OverWriteAchievementsProgress(Missions.MissionTarget.ReachMissionSet, Missions.Instance.currentMissionSet);
		OverWriteAchievementsProgress(Missions.MissionTarget.HaveUpgrades, instance.GetUpgradeTierSum());
		OverWriteAchievementsProgress(Missions.MissionTarget.HaveHeadStartLarge, instance.GetUpgradeAmount(PowerupType.headstart2000));
		OverWriteAchievementsProgress(Missions.MissionTarget.HavePinkGirl, instance.GetCollectedTokens(CharacterModels.ModelType.lucy) / CharacterModels.modelData[CharacterModels.ModelType.lucy].Price);
		OverWriteAchievementsProgress(Missions.MissionTarget.HaveNinja, instance.GetCollectedTokens(CharacterModels.ModelType.ninja) / CharacterModels.modelData[CharacterModels.ModelType.ninja].Price);
		OverWriteAchievementsProgress(Missions.MissionTarget.HaveFrank, instance.GetCollectedTokens(CharacterModels.ModelType.frank) / CharacterModels.modelData[CharacterModels.ModelType.frank].Price);
		OverWriteAchievementsProgress(Missions.MissionTarget.HaveKing, instance.GetCollectedTokens(CharacterModels.ModelType.king) / CharacterModels.modelData[CharacterModels.ModelType.king].Price);
		OverWriteAchievementsProgress(Missions.MissionTarget.HaveBlackGirl, instance.GetCollectedTokens(CharacterModels.ModelType.frizzy) / CharacterModels.modelData[CharacterModels.ModelType.frizzy].Price);
	}
}
