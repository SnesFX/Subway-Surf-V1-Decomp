using System;
using System.Collections.Generic;
using UnityEngine;

public class ChartBoostManager
{
	private const string APPID = "502e041317ba47dc7d000024";

	private const string APPSIGNATURE = "e356ea8420eeb855ff880fba02ce0309364d0613";

	private const string ALLOWNEXT_TICKS_KEY = "cb_alwnxt_ticks";

	private const int FIRSTTIME_DELAY_MINUTES = 1200;

	private const int DEFAULT_DELAY_MINUTES = 2;

	private const string DELAY_MINUTES_ONLINESETTINGSKEY = "chartboost_delay_minutes";

	private static readonly List<string> ALLOWED_SHOW_SCREENS = new List<string> { "FrontUI" };

	private static readonly List<string> ALLOWED_CACHE_SCREENS = new List<string> { "CoinsUI_shop", "CharacterUI", "GameoverUI", "TokensUI", "UpgradesUI_shop" };

	private static ChartBoostManager _instance;

	private bool _isCaching;

	public bool isInstanced
	{
		get
		{
			return _instance != null;
		}
	}

	public static ChartBoostManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ChartBoostManager();
			}
			return _instance;
		}
	}

	private bool interstitialsEnabled
	{
		get
		{
			return PlayerInfo.Instance.inAppPurchaseCount <= 0;
		}
	}

	private ChartBoostManager()
	{
		if (interstitialsEnabled)
		{
			ChartBoostAndroid.init("502e041317ba47dc7d000024", "e356ea8420eeb855ff880fba02ce0309364d0613");
			ChartBoostAndroidManager.didFinishInterstitialEvent += didFinishInterstitialEvent;
			ChartBoostAndroidManager.didFinishMoreAppsEvent += didFinishMoreAppsEvent;
			ChartBoostAndroidManager.didFailToLoadMoreAppsEvent += didFailToLoadMoreAppsEvent;
			ChartBoostAndroidManager.didFailToLoadInterstitialEvent += didFailToLoadInterstitialEvent;
		}
	}

	public void GameScreenChanged(string screenName)
	{
		if (!string.IsNullOrEmpty(screenName))
		{
			ShowOrCacheForScreen(screenName);
		}
	}

	public void CacheNow()
	{
		if (!interstitialsEnabled)
		{
			return;
		}
		if (!_isCaching)
		{
			if (!ChartBoostAndroid.hasCachedInterstitial(null))
			{
				Debug.Log("ChartBoostManager.CacheNow(): Starting to cache");
				_isCaching = true;
				ChartBoostAndroid.cacheInterstitial(null);
			}
			else
			{
				Debug.Log("ChartBoostManager.CacheNow(): Already has a cached interstitial - doing nothing");
			}
		}
		else
		{
			Debug.Log("ChartBoostManager.CacheNow(): Already caching - doing nothing");
		}
	}

	private void ShowOrCacheForScreen(string screenName)
	{
		if (!interstitialsEnabled)
		{
			return;
		}
		if (ChartBoostAndroid.hasCachedInterstitial(null))
		{
			Debug.Log("ChartBoostManager: Has cached interstitial");
			if (_isCaching)
			{
				Debug.Log("ChartBoostManager: _isCaching seems to have gone out of sync - setting it to false");
				_isCaching = false;
			}
			if (!ALLOWED_SHOW_SCREENS.Contains(screenName))
			{
				return;
			}
			Debug.Log("ChartBoostManager: Allowed to show on screen " + screenName);
			DateTime dateTime;
			if (PlayerPrefs.HasKey("cb_alwnxt_ticks"))
			{
				string @string = PlayerPrefs.GetString("cb_alwnxt_ticks");
				long result;
				if (!long.TryParse(@string, out result))
				{
					result = DateTime.Now.Ticks;
				}
				dateTime = new DateTime(result);
				Debug.Log("ChartBoostManager: Read next allow time from PlayerPrefs: " + dateTime);
			}
			else
			{
				dateTime = DateTime.Now + TimeSpan.FromMinutes(1200.0);
				PlayerPrefs.SetString("cb_alwnxt_ticks", dateTime.Ticks.ToString());
				Debug.Log("ChartBoostManager: Initialized next allow time to PlayerPrefs: " + dateTime);
			}
			DateTime now = DateTime.Now;
			bool flag = false;
			if (!(now >= dateTime))
			{
				return;
			}
			Debug.Log("ChartBoostManager: Delay is over - showing interstitial.");
			ChartBoostAndroid.showInterstitial(null);
			int num = 2;
			string valueString;
			if (OnlineSettings.instance.TryGetValue("chartboost_delay_minutes", out valueString))
			{
				int result2;
				if (int.TryParse(valueString, out result2))
				{
					if (result2 >= 0)
					{
						num = result2;
						Debug.Log("ChartBoostManager: Delay read from OnlineSettings to " + num + " minutes");
					}
					else
					{
						Debug.LogError("ChartBoostManager: Delay minutes from OnlineSettings is not a positive number: " + result2);
					}
				}
				else
				{
					Debug.LogError("ChartBoostManager: Failed to parse delay minutes from OnlineSettings: " + valueString);
				}
			}
			dateTime = DateTime.Now + TimeSpan.FromMinutes(num);
			Debug.Log("ChartBoostManager: Setting time for next to " + dateTime);
			PlayerPrefs.SetString("cb_alwnxt_ticks", dateTime.Ticks.ToString());
		}
		else
		{
			Debug.Log("ChartBoostManager: No cached interstitial");
			if (ALLOWED_CACHE_SCREENS.Contains(screenName))
			{
				Debug.Log("ChartBoostManager: Allowed to cache on screen " + screenName);
				CacheNow();
			}
		}
	}

	private void OnDidCacheInterstitial(string location)
	{
		if (_isCaching)
		{
			Debug.Log("ChartBoostManager: Caching succeeded");
		}
		else
		{
			Debug.LogError("ChartBoostManager: OnDidCacheInterstitial called but _isCaching is false");
		}
		_isCaching = false;
	}

	private void OnDidFailToLoadInterstitial(string location)
	{
		if (_isCaching)
		{
			Debug.Log("ChartBoostManager: Caching failed");
		}
		else
		{
			Debug.LogError("ChartBoostManager: OnDidFailToLoadInterstitial called but _isCaching is false");
		}
		_isCaching = false;
	}

	private void didFinishInterstitialEvent(string param)
	{
	}

	private void didFinishMoreAppsEvent(string param)
	{
	}

	private void didFailToLoadMoreAppsEvent()
	{
	}

	private void didFailToLoadInterstitialEvent()
	{
	}
}
