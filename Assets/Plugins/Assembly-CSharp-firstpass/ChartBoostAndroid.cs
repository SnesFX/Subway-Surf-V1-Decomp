using UnityEngine;

public class ChartBoostAndroid
{
	private static AndroidJavaObject _plugin;

	static ChartBoostAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.ChartBoostPlugin"))
		{
			_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public static void init(string appId, string appSignature)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("init", appId, appSignature);
		}
	}

	public static void cacheInterstitial(string location)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (location == null)
			{
				location = string.Empty;
			}
			_plugin.Call("cacheInterstitial", location);
		}
	}

	public static bool hasCachedInterstitial(string location)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		if (location == null)
		{
			location = string.Empty;
		}
		return _plugin.Call<bool>("hasCachedInterstitial", new object[1] { location });
	}

	public static void showInterstitial(string location)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (location == null)
			{
				location = string.Empty;
			}
			_plugin.Call("showInterstitial", location);
		}
	}

	public static void cacheMoreApps()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("cacheMoreApps");
		}
	}

	public static bool hasCachedMoreApps()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _plugin.Call<bool>("hasCachedMoreApps", new object[0]);
	}

	public static void showMoreApps()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showMoreApps");
		}
	}
}
