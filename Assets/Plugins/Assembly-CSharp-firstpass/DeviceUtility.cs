using UnityEngine;

public class DeviceUtility
{
	public static int GetVersionCode()
	{
		//Discarded unreachable code: IL_005b, IL_006d
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.kiloo.subwaysurf.MainGCM", androidJavaObject))
			{
				int num = androidJavaObject2.CallStatic<int>("getVersionCode", new object[0]);
				Debug.Log(string.Format("Version Code: {0}", num));
				return num;
			}
		}
	}

	public static string GetVersionName()
	{
		//Discarded unreachable code: IL_0056, IL_0068
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.kiloo.subwaysurf.MainGCM", androidJavaObject))
			{
				string text = androidJavaObject2.CallStatic<string>("getVersionName", new object[0]);
				Debug.Log(string.Format("Version Name: {0}", text));
				return text;
			}
		}
	}
}
