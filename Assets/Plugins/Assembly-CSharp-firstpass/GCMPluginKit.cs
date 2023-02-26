using System;
using UnityEngine;

public class GCMPluginKit : IDisposable
{
	void IDisposable.Dispose()
	{
	}

	public static void getGCMToken(string objName)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.kiloo.subwaysurf.MainGCM", androidJavaObject))
			{
				androidJavaObject2.CallStatic("registerDevice", objName);
			}
		}
	}
}
