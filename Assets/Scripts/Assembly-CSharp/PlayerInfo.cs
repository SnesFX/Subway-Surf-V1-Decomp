using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInfo
{
	private enum Key
	{
		AmountOfCoins = 0,
		OldHighestScore = 1,
		HighestScore = 2,
		DailyWord = 3,
		DailyWordUnlockMask = 4,
		DailyWordExpireTime = 5,
		DailyWordPayedOutTime = 6,
		CurrentCharacter = 7,
		CurrentMissionSet = 8,
		CurrentMissionSetProgress = 9,
		CollectedCharacterTokens = 10,
		AmountOfMysteryBoxesOpened = 11,
		TutorialCompleted = 12,
		InAppPurchaseCount = 13,
		EarnCurrencyData = 14,
		PayBonusFacebook = 15,
		PayBonusGameCenter = 16,
		Count = 17,
		UnlockedTrophies = 18,
		AchievementProgress = 19
	}

	private const string SECRET = "we12rtyuiklhgfdjerKJGHfvghyuhnjiokLJHl145rtyfghjvbn";

	private const int VERSION = 1;

	private bool _dirty;

	public Action onCoinsChanged;

	private int _amountOfCoins;

	private int _highestScore;

	private int _oldHighestScore;

	public Action onHighScoreChanged;

	private int _highestMeters;

	private int _amountOfMysteryBoxesOpened;

	private int _mysteryBoxesToUnlock;

	private int _lastMissionCompleted = -1;

	private int _currentMissionSet = -1;

	private int[] _currentMissionProgress;

	public Action onScoreMultiplierChanged;

	private int _currentCharacter;

	public Action<CharacterModels.TokenType> OnTokenCollected;

	private int[] _collectedCharacterTokens = new int[CharacterModels.tokenInfo.Count];

	public Action<Trophies.Trophy> OnTrophyUnlocked;

	private bool[] _unlockedTrophies = new bool[Enum.GetValues(typeof(Trophies.Trophy)).Length];

	private int[] _achievementProgress = new int[27];

	private bool _hasPayedOutFacebook;

	private bool _hasPayedOutGameCenter;

	private string _dailyWord = string.Empty;

	private IntMask _dailyWordUnlockedMask;

	private DateTime _dailyWordExpireTime;

	private DateTime _dailyWordPayedOutTime;

	public Action OnPickedUpLetter;

	private bool _tutorialCompleted;

	private int _inAppPurchaseCount;

	private string _earnCurrenyData = string.Empty;

	public Action onPowerupAmountChanged;

	private Dictionary<PowerupType, int> _upgradeAmounts = new Dictionary<PowerupType, int>
	{
		{
			PowerupType.hoverboard,
			0
		},
		{
			PowerupType.headstart500,
			0
		},
		{
			PowerupType.headstart2000,
			0
		},
		{
			PowerupType.mysterybox,
			0
		}
	};

	private Dictionary<PowerupType, int> _upgradeTiers = new Dictionary<PowerupType, int>
	{
		{
			PowerupType.jetpack,
			0
		},
		{
			PowerupType.supersneakers,
			0
		},
		{
			PowerupType.coinmagnet,
			0
		},
		{
			PowerupType.letters,
			0
		},
		{
			PowerupType.doubleMultiplier,
			4
		}
	};

	private bool _doubleScore;

	private static PlayerInfo _instance;

	public bool dirty
	{
		get
		{
			return _dirty;
		}
	}

	public int amountOfCoins
	{
		get
		{
			return _amountOfCoins;
		}
		set
		{
			if (_amountOfCoins != value)
			{
				_amountOfCoins = value;
				_dirty = true;
				Action action = onCoinsChanged;
				if (action != null)
				{
					action();
				}
			}
		}
	}

	public int highestScore
	{
		get
		{
			return _highestScore;
		}
		set
		{
			if (value > _highestScore)
			{
				_oldHighestScore = _highestScore;
				_highestScore = value;
				_dirty = true;
				Action action = onHighScoreChanged;
				if (action != null)
				{
					action();
				}
			}
		}
	}

	public int oldHighestScore
	{
		get
		{
			return _oldHighestScore;
		}
	}

	public int highestMeters
	{
		get
		{
			return _highestMeters;
		}
		set
		{
			_highestMeters = value;
			_dirty = true;
		}
	}

	public int amountOfMysteryBoxesOpened
	{
		get
		{
			return _amountOfMysteryBoxesOpened;
		}
		set
		{
			_amountOfMysteryBoxesOpened = value;
		}
	}

	public int mysteryBoxesToUnlock
	{
		get
		{
			return _mysteryBoxesToUnlock;
		}
		set
		{
			_mysteryBoxesToUnlock = value;
		}
	}

	public int currentMissionSet
	{
		get
		{
			return _currentMissionSet;
		}
	}

	public int lastMissionCompleted
	{
		get
		{
			return _lastMissionCompleted;
		}
	}

	public int scoreMultiplier
	{
		get
		{
			int num = Mathf.Clamp(_currentMissionSet + 1, 0, 30);
			if (doubleScore)
			{
				num *= 2;
			}
			return num;
		}
	}

	public int rawMultiplier
	{
		get
		{
			return Mathf.Clamp(_currentMissionSet + 1, 0, 30);
		}
	}

	public int currentCharacter
	{
		get
		{
			return _currentCharacter;
		}
		set
		{
			if (value != _currentCharacter)
			{
				_currentCharacter = value;
				_dirty = true;
				SaveIfDirty();
			}
		}
	}

	public int[] achievementProgress
	{
		get
		{
			return _achievementProgress;
		}
		set
		{
			_achievementProgress = value;
		}
	}

	public bool hasPayedOutFacebook
	{
		get
		{
			return _hasPayedOutFacebook;
		}
		set
		{
			_hasPayedOutFacebook = value;
			_dirty = true;
		}
	}

	public bool hasPayedOutGameCenter
	{
		get
		{
			return _hasPayedOutGameCenter;
		}
		set
		{
			_hasPayedOutGameCenter = value;
			_dirty = true;
		}
	}

	public string dailyWord
	{
		get
		{
			return _dailyWord;
		}
	}

	public IntMask dailyWordUnlockedMask
	{
		get
		{
			return _dailyWordUnlockedMask;
		}
	}

	public DateTime dailyWordExpireTime
	{
		get
		{
			return _dailyWordExpireTime;
		}
	}

	public DateTime dailyWordPayedOutTime
	{
		get
		{
			return _dailyWordPayedOutTime;
		}
	}

	public bool tutorialCompleted
	{
		get
		{
			return _tutorialCompleted;
		}
		set
		{
			_tutorialCompleted = value;
			_dirty = true;
			SaveIfDirty();
		}
	}

	public int inAppPurchaseCount
	{
		get
		{
			return _inAppPurchaseCount;
		}
		set
		{
			_inAppPurchaseCount = value;
			_dirty = true;
		}
	}

	public string earnCurrenyData
	{
		get
		{
			return _earnCurrenyData;
		}
		set
		{
			_earnCurrenyData = value;
			_dirty = true;
		}
	}

	public float doubleScoreMultiplierDuration
	{
		get
		{
			return GetPowerupDuration(PowerupType.doubleMultiplier);
		}
	}

	public bool doubleScore
	{
		get
		{
			return _doubleScore;
		}
		set
		{
			if (value != _doubleScore)
			{
				_doubleScore = value;
				Action action = onScoreMultiplierChanged;
				if (action != null)
				{
					action();
				}
			}
		}
	}

	public static PlayerInfo Instance
	{
		get
		{
			return _instance ?? (_instance = new PlayerInfo());
		}
	}

	private PlayerInfo()
	{
		Load();
	}

	public void SetOldestHighestScore()
	{
		if (_oldHighestScore < _highestScore)
		{
			_oldHighestScore = _highestScore;
			_dirty = true;
		}
	}

	public void BragCompleted()
	{
		if (_oldHighestScore < _highestScore)
		{
			_oldHighestScore = _highestScore;
			_dirty = true;
		}
	}

	public bool IsCurrentMissionSetInited()
	{
		return false;
	}

	public void InitCurrentMissionSet(int missionSet, int missionCount)
	{
		if (missionSet != _currentMissionSet)
		{
			_currentMissionSet = missionSet;
			_currentMissionProgress = new int[missionCount];
			for (int i = 0; i < missionCount; i++)
			{
				_currentMissionProgress[i] = 0;
			}
			_dirty = true;
			Action action = onScoreMultiplierChanged;
			if (action != null)
			{
				action();
			}
		}
	}

	public void ReInitCurrentMissionSet(int missionSet, int missionCount)
	{
		_currentMissionSet = missionSet;
		_currentMissionProgress = new int[missionCount];
		for (int i = 0; i < missionCount; i++)
		{
			_currentMissionProgress[i] = 0;
		}
		_dirty = true;
		Action action = onScoreMultiplierChanged;
		if (action != null)
		{
			action();
		}
	}

	public int GetCurrentMissionProgress(int mission)
	{
		if (_currentMissionProgress == null)
		{
			return 0;
		}
		if (mission < _currentMissionProgress.Length)
		{
			return _currentMissionProgress[mission];
		}
		return 0;
	}

	public void SetCurrentMissionProgress(int mission, int progress)
	{
		if (_currentMissionProgress[mission] != progress)
		{
			_currentMissionProgress[mission] = progress;
			_dirty = true;
		}
	}

	public bool IncrementCurrentMissionProgress(int mission, int target)
	{
		if (_currentMissionProgress[mission] < target)
		{
			_currentMissionProgress[mission]++;
			_dirty = true;
			return _currentMissionProgress[mission] == target;
		}
		return false;
	}

	public int GetCurrentRank()
	{
		return currentMissionSet / Missions.Instance.GetNumberOfBasicMission();
	}

	public void CollectToken(CharacterModels.TokenType tokenType, int amount = 1)
	{
		_collectedCharacterTokens[(int)tokenType] += amount;
		_dirty = true;
		Action<CharacterModels.TokenType> onTokenCollected = OnTokenCollected;
		if (onTokenCollected != null)
		{
			onTokenCollected(tokenType);
		}
		SaveIfDirty();
	}

	public bool[] GetUnlockedTrophies()
	{
		return _unlockedTrophies;
	}

	public void unlockTrophy(Trophies.Trophy trophy)
	{
		if (_unlockedTrophies[(int)trophy])
		{
			return;
		}
		_unlockedTrophies[(int)trophy] = true;
		_dirty = true;
		Action<Trophies.Trophy> onTrophyUnlocked = OnTrophyUnlocked;
		if (onTrophyUnlocked != null)
		{
			onTrophyUnlocked(trophy);
		}
		bool flag = true;
		bool[] unlockedTrophies = _unlockedTrophies;
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
		SaveIfDirty();
	}

	public bool isTrophyUnlocked(Trophies.Trophy trophy)
	{
		return _unlockedTrophies[(int)trophy];
	}

	public bool IsCollectionComplete(CharacterModels.ModelType modelType)
	{
		CharacterModels.Model model = CharacterModels.modelData[modelType];
		if (model.unlockType == CharacterModels.UnlockType.free)
		{
			return true;
		}
		return GetCollectedTokens(modelType) >= model.Price;
	}

	public int GetCollectedTokens(CharacterModels.ModelType modelType)
	{
		int num = 0;
		CharacterModels.TokenType[] tokenType = CharacterModels.modelData[modelType].tokenType;
		foreach (CharacterModels.TokenType tokenType2 in tokenType)
		{
			num += _collectedCharacterTokens[(int)tokenType2];
		}
		return num;
	}

	public bool IsTokenUseful(CharacterModels.TokenType tokenType)
	{
		foreach (int value in Enum.GetValues(typeof(CharacterModels.ModelType)))
		{
			CharacterModels.Model model = CharacterModels.modelData[(CharacterModels.ModelType)value];
			bool flag = false;
			CharacterModels.TokenType[] tokenType2 = model.tokenType;
			foreach (CharacterModels.TokenType tokenType3 in tokenType2)
			{
				if (tokenType3 == tokenType)
				{
					flag = true;
					break;
				}
			}
			if (!flag || Instance.GetCollectedTokens((CharacterModels.ModelType)value) >= model.Price)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public void InitDailyWord(string word, DateTime expires)
	{
		if (!_dailyWord.Equals(word) || !_dailyWordExpireTime.Equals(expires))
		{
			_dailyWord = word;
			_dailyWordExpireTime = expires;
			_dailyWordPayedOutTime = DateTime.UtcNow;
			_dailyWordUnlockedMask = 0;
			_dirty = true;
			SaveIfDirty();
		}
		DailyLetterPickupManager.Instance.UpdateLetter();
	}

	public void PickedupLetter(char letter)
	{
		for (int i = 0; i < _dailyWord.Length; i++)
		{
			if (_dailyWord[i] == letter && !_dailyWordUnlockedMask[i])
			{
				_dailyWordUnlockedMask[i] = true;
				Action onPickedUpLetter = OnPickedUpLetter;
				if (onPickedUpLetter != null)
				{
					onPickedUpLetter();
				}
				_dirty = true;
				SaveIfDirty();
				break;
			}
		}
		if (isDailyWordComplete() && _dailyWordPayedOutTime != _dailyWordExpireTime)
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.DailyQuests);
			_mysteryBoxesToUnlock++;
			_dailyWordPayedOutTime = _dailyWordExpireTime;
			_dirty = true;
			SaveIfDirty();
			UIScreenController.Instance.QueueSlideIn(UIScreenController.SlideInType.LettersComplete, string.Empty);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Id", _dailyWord);
		}
	}

	public char GetNewDailyLetter()
	{
		for (int i = 0; i < _dailyWord.Length; i++)
		{
			if (!_dailyWordUnlockedMask[i])
			{
				return _dailyWord[i];
			}
		}
		return '\0';
	}

	public bool isDailyWordComplete()
	{
		return (1 << _dailyWord.Length) - 1 == (int)_dailyWordUnlockedMask;
	}

	public int GetUpgradeTierSum()
	{
		return GetCurrentTier(PowerupType.jetpack) + GetCurrentTier(PowerupType.doubleMultiplier) + GetCurrentTier(PowerupType.coinmagnet) + GetCurrentTier(PowerupType.supersneakers);
	}

	public int GetUpgradeAmount(PowerupType type)
	{
		return _upgradeAmounts[type];
	}

	public int GetCurrentTier(PowerupType type)
	{
		if (!_upgradeTiers.ContainsKey(type))
		{
			return 0;
		}
		return _upgradeTiers[type];
	}

	public float GetPowerupDuration(PowerupType type)
	{
		if (!Upgrades.upgrades.ContainsKey(type))
		{
			Debug.Log("Couldn't find any upgrades of the type: " + type.ToString() + ". Returning 0");
			return 0f;
		}
		return Upgrades.upgrades[type].durations[GetCurrentTier(type)];
	}

	public void IncreasePowerupTier(PowerupType type)
	{
		if (_upgradeTiers.ContainsKey(type))
		{
			_upgradeTiers[type] += 1;
			_dirty = true;
			SaveIfDirty();
		}
		else
		{
			Debug.LogError("Trying to increase tier for a non-tiered upgrade");
		}
	}

	public void UseUpgrade(PowerupType type)
	{
		Debug.Log("Used powerup: " + type);
		if (_upgradeAmounts.ContainsKey(type))
		{
			Dictionary<PowerupType, int> upgradeAmounts;
			Dictionary<PowerupType, int> dictionary = (upgradeAmounts = _upgradeAmounts);
			PowerupType key;
			PowerupType key2 = (key = type);
			int num = upgradeAmounts[key];
			dictionary[key2] = num - 1;
			_dirty = true;
			Action action = onPowerupAmountChanged;
			if (action != null)
			{
				action();
			}
			SaveIfDirty();
		}
	}

	public void IncreaseUpgradeAmount(PowerupType type, int amount = 1)
	{
		if (_upgradeAmounts.ContainsKey(type))
		{
			Dictionary<PowerupType, int> upgradeAmounts;
			Dictionary<PowerupType, int> dictionary = (upgradeAmounts = _upgradeAmounts);
			PowerupType key;
			PowerupType key2 = (key = type);
			int num = upgradeAmounts[key];
			dictionary[key2] = num + amount;
			_dirty = true;
			Action action = onPowerupAmountChanged;
			if (action != null)
			{
				action();
			}
			SaveIfDirty();
		}
		else
		{
			Debug.LogError("Trying to increase upgrade amount for a non-consumable");
		}
	}

	public int GetNumberOfAffordableUpgrades()
	{
		int num = 0;
		bool flag = Missions.Instance.HasMoreMissions();
		foreach (KeyValuePair<PowerupType, Upgrade> upgrade in Upgrades.upgrades)
		{
			Upgrade value = upgrade.Value;
			if (value.numberOfTiers > 0)
			{
				int num2 = GetCurrentTier(upgrade.Key) + 1;
				if (num2 < value.pricesRaw.Length)
				{
					int price = value.getPrice(num2);
					if (price <= amountOfCoins)
					{
						num++;
					}
				}
			}
			else if (value.pricesRaw != null && value.pricesRaw.Length > 0 && (upgrade.Key != PowerupType.skipmission1 || (flag && !Missions.Instance.GetMissionInfo(0).complete)) && (upgrade.Key != PowerupType.skipmission2 || (flag && !Missions.Instance.GetMissionInfo(1).complete)) && (upgrade.Key != PowerupType.skipmission3 || (flag && !Missions.Instance.GetMissionInfo(2).complete)))
			{
				int price2 = value.getPrice(0);
				if (price2 <= amountOfCoins)
				{
					num++;
				}
			}
		}
		return num;
	}

	private static string GetSaveDataPath()
	{
		string text = Application.persistentDataPath + "/playerdata";
		Debug.Log("playerdata save data path: \"" + text + "\"");
		return text;
	}

	public void SaveIfDirty()
	{
		if (_dirty)
		{
			Save();
		}
	}

	public void Save()
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream(8192);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(1);
			Dictionary<Key, string> dictionary = new Dictionary<Key, string>(17);
			dictionary[Key.AmountOfCoins] = _amountOfCoins.ToString();
			dictionary[Key.HighestScore] = _highestScore.ToString();
			dictionary[Key.OldHighestScore] = _oldHighestScore.ToString();
			dictionary[Key.DailyWord] = _dailyWord;
			dictionary[Key.DailyWordUnlockMask] = ((int)_dailyWordUnlockedMask).ToString();
			dictionary[Key.DailyWordExpireTime] = _dailyWordExpireTime.ToString();
			dictionary[Key.DailyWordPayedOutTime] = _dailyWordPayedOutTime.ToString();
			dictionary[Key.CurrentCharacter] = _currentCharacter.ToString();
			dictionary[Key.CurrentMissionSet] = _currentMissionSet.ToString();
			dictionary[Key.AmountOfMysteryBoxesOpened] = _amountOfMysteryBoxesOpened.ToString();
			dictionary[Key.TutorialCompleted] = _tutorialCompleted.ToString();
			dictionary[Key.InAppPurchaseCount] = _inAppPurchaseCount.ToString();
			dictionary[Key.EarnCurrencyData] = _earnCurrenyData;
			dictionary[Key.PayBonusFacebook] = _hasPayedOutFacebook.ToString();
			dictionary[Key.PayBonusGameCenter] = _hasPayedOutGameCenter.ToString();
			if (_currentMissionSet >= 0)
			{
				dictionary[Key.CurrentMissionSetProgress] = string.Join(",", Array.ConvertAll(_currentMissionProgress, (int input) => input.ToString()));
			}
			dictionary[Key.CollectedCharacterTokens] = string.Join(",", Array.ConvertAll(_collectedCharacterTokens, (int input) => input.ToString()));
			dictionary[Key.UnlockedTrophies] = string.Join(",", Array.ConvertAll(_unlockedTrophies, (bool input) => input.ToString()));
			dictionary[Key.AchievementProgress] = string.Join(",", Array.ConvertAll(_achievementProgress, (int input) => input.ToString()));
			FileUtil.WriteEnumStringDictionary(binaryWriter, dictionary);
			FileUtil.WriteEnumIntDictionary(binaryWriter, _upgradeAmounts);
			FileUtil.WriteEnumIntDictionary(binaryWriter, _upgradeTiers);
			FileUtil.Save(GetSaveDataPath(), "we12rtyuiklhgfdjerKJGHfvghyuhnjiokLJHl145rtyfghjvbn", memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			memoryStream.Close();
			_dirty = false;
		}
		catch (Exception ex)
		{
			Debug.LogError("Error saving player info: " + ex.ToString());
		}
	}

	public void Load()
	{
		try
		{
			byte[] buffer = FileUtil.Load(GetSaveDataPath(), "we12rtyuiklhgfdjerKJGHfvghyuhnjiokLJHl145rtyfghjvbn");
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			int num = binaryReader.ReadInt32();
			Dictionary<Key, string> dictionary = FileUtil.ReadEnumStringDictionary<Key>(binaryReader);
			_amountOfCoins = (dictionary.ContainsKey(Key.AmountOfCoins) ? int.Parse(dictionary[Key.AmountOfCoins]) : 0);
			_highestScore = (dictionary.ContainsKey(Key.HighestScore) ? int.Parse(dictionary[Key.HighestScore]) : 0);
			_oldHighestScore = (dictionary.ContainsKey(Key.OldHighestScore) ? int.Parse(dictionary[Key.HighestScore]) : 0);
			_dailyWord = ((!dictionary.ContainsKey(Key.DailyWord)) ? string.Empty : dictionary[Key.DailyWord]);
			_dailyWordUnlockedMask = (dictionary.ContainsKey(Key.DailyWordUnlockMask) ? int.Parse(dictionary[Key.DailyWordUnlockMask]) : 0);
			_dailyWordExpireTime = ((!dictionary.ContainsKey(Key.DailyWordExpireTime)) ? DateTime.UtcNow : DateTime.Parse(dictionary[Key.DailyWordExpireTime]));
			_dailyWordPayedOutTime = ((!dictionary.ContainsKey(Key.DailyWordPayedOutTime)) ? DateTime.UtcNow : DateTime.Parse(dictionary[Key.DailyWordPayedOutTime]));
			_currentCharacter = (dictionary.ContainsKey(Key.CurrentCharacter) ? int.Parse(dictionary[Key.CurrentCharacter]) : 0);
			_currentMissionSet = ((!dictionary.ContainsKey(Key.CurrentMissionSet)) ? (-1) : int.Parse(dictionary[Key.CurrentMissionSet]));
			_amountOfMysteryBoxesOpened = (dictionary.ContainsKey(Key.AmountOfMysteryBoxesOpened) ? int.Parse(dictionary[Key.AmountOfMysteryBoxesOpened]) : 0);
			_tutorialCompleted = dictionary.ContainsKey(Key.TutorialCompleted) && bool.Parse(dictionary[Key.TutorialCompleted]);
			_inAppPurchaseCount = (dictionary.ContainsKey(Key.InAppPurchaseCount) ? int.Parse(dictionary[Key.InAppPurchaseCount]) : 0);
			_earnCurrenyData = ((!dictionary.ContainsKey(Key.EarnCurrencyData)) ? string.Empty : dictionary[Key.EarnCurrencyData]);
			_hasPayedOutFacebook = dictionary.ContainsKey(Key.PayBonusFacebook) && bool.Parse(dictionary[Key.PayBonusFacebook]);
			_hasPayedOutGameCenter = dictionary.ContainsKey(Key.PayBonusGameCenter) && bool.Parse(dictionary[Key.PayBonusGameCenter]);
			_currentMissionProgress = null;
			if (dictionary.ContainsKey(Key.CurrentMissionSetProgress))
			{
				string text = dictionary[Key.CurrentMissionSetProgress];
				if (!string.IsNullOrEmpty(text))
				{
					_currentMissionProgress = Array.ConvertAll(text.Split(','), (string input) => int.Parse(input));
				}
			}
			if (dictionary.ContainsKey(Key.CollectedCharacterTokens))
			{
				string text2 = dictionary[Key.CollectedCharacterTokens];
				if (!string.IsNullOrEmpty(text2))
				{
					int[] array = Array.ConvertAll(text2.Split(','), (string input) => int.Parse(input));
					int i = Mathf.Min(array.Length, _collectedCharacterTokens.Length);
					Array.Copy(array, _collectedCharacterTokens, i);
					for (; i < _collectedCharacterTokens.Length; i++)
					{
						_collectedCharacterTokens[i] = 0;
					}
				}
			}
			if (dictionary.ContainsKey(Key.UnlockedTrophies))
			{
				string text3 = dictionary[Key.UnlockedTrophies];
				if (!string.IsNullOrEmpty(text3))
				{
					bool[] array2 = Array.ConvertAll(text3.Split(','), (string input) => bool.Parse(input));
					int j = Mathf.Min(array2.Length, _unlockedTrophies.Length);
					Array.Copy(array2, _unlockedTrophies, j);
					for (; j < _unlockedTrophies.Length; j++)
					{
						_unlockedTrophies[j] = false;
					}
				}
			}
			if (dictionary.ContainsKey(Key.AchievementProgress))
			{
				string text4 = dictionary[Key.AchievementProgress];
				if (!string.IsNullOrEmpty(text4))
				{
					int[] array3 = Array.ConvertAll(text4.Split(','), (string input) => int.Parse(input));
					int k = Mathf.Min(array3.Length, _achievementProgress.Length);
					Array.Copy(array3, _achievementProgress, k);
					for (; k < _achievementProgress.Length; k++)
					{
						_achievementProgress[k] = 0;
					}
				}
			}
			Dictionary<PowerupType, int> dictionary2 = FileUtil.ReadEnumIntDictionary<PowerupType>(binaryReader);
			foreach (KeyValuePair<PowerupType, int> item in dictionary2)
			{
				_upgradeAmounts[item.Key] = item.Value;
			}
			Dictionary<PowerupType, int> dictionary3 = FileUtil.ReadEnumIntDictionary<PowerupType>(binaryReader);
			foreach (KeyValuePair<PowerupType, int> item2 in dictionary3)
			{
				_upgradeTiers[item2.Key] = item2.Value;
			}
			memoryStream.Close();
			_dirty = false;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error loading player info: " + ex.ToString());
			InitNew();
		}
	}

	public void InitNew()
	{
		_amountOfCoins = 0;
		_highestScore = 0;
		_dailyWord = string.Empty;
		_dailyWordUnlockedMask = 0;
		_dailyWordExpireTime = DateTime.UtcNow;
		_dailyWordPayedOutTime = DateTime.UtcNow;
		_amountOfMysteryBoxesOpened = 0;
		_currentCharacter = 0;
		_currentMissionSet = -1;
		_currentMissionProgress = null;
		_tutorialCompleted = false;
		_inAppPurchaseCount = 0;
		_earnCurrenyData = string.Empty;
		_hasPayedOutFacebook = false;
		_hasPayedOutGameCenter = false;
		for (int i = 0; i < _collectedCharacterTokens.Length; i++)
		{
			_collectedCharacterTokens[i] = 0;
		}
		for (int j = 0; j < _achievementProgress.Length; j++)
		{
			_achievementProgress[j] = 0;
		}
		Dictionary<PowerupType, int> dictionary = new Dictionary<PowerupType, int>(_upgradeAmounts.Count);
		foreach (PowerupType key in _upgradeAmounts.Keys)
		{
			if (key == PowerupType.hoverboard)
			{
				dictionary[key] = 3;
			}
			else
			{
				dictionary[key] = 0;
			}
		}
		_upgradeAmounts = dictionary;
		dictionary = new Dictionary<PowerupType, int>(_upgradeTiers.Count);
		foreach (PowerupType key2 in _upgradeTiers.Keys)
		{
			dictionary[key2] = 0;
		}
		_upgradeTiers = dictionary;
	}

	public float GetHoverBoardCoolDown()
	{
		return 5f;
	}
}
