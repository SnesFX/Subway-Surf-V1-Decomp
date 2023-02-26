using UnityEngine;

public class HouseKeeper : MonoBehaviour
{
	private const float ONLINESETTINGS_DEFAULT_MIN_REFRESH_INTERVAL = 3600f;

	private const string ONLINESETTINGS_REFRESH_INTERVAL_KEY = "refreshinterval";

	private static float _onlineSettingsLastDownloadTime = float.NegativeInfinity;

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		RefreshOnlineSettingsIfNeeded();
		ChartBoostManager.instance.CacheNow();
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
		}
	}

	public static void RefreshOnlineSettingsIfNeeded()
	{
		float num = 3600f;
		string valueString;
		float result;
		if (OnlineSettings.instance.TryGetValue("refreshinterval", out valueString) && float.TryParse(valueString, out result) && result >= 10f)
		{
			num = result;
		}
		if (Time.realtimeSinceStartup > _onlineSettingsLastDownloadTime + num)
		{
			_onlineSettingsLastDownloadTime = Time.realtimeSinceStartup;
			if (!OnlineSettings.instance.isDownloading)
			{
				OnlineSettings.instance.DownloadNow();
			}
		}
	}
}
