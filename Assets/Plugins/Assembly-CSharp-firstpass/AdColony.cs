using System;
using UnityEngine;

public class AdColony : MonoBehaviour
{
	public delegate void VideoStartedDelegate();

	public delegate void VideoFinishedDelegate();

	public delegate void V4VCResultDelegate(bool success, string name, int amount);

	public static VideoStartedDelegate OnVideoStarted;

	public static VideoFinishedDelegate OnVideoFinished;

	public static V4VCResultDelegate OnV4VCResult;

	private static bool configured;

	private bool was_paused;

	private bool saved_timescale;

	private float previous_timescale;

	private static AndroidJavaClass class_UnityPlayer;

	private static IntPtr class_UnityADC = IntPtr.Zero;

	private static IntPtr method_resume = IntPtr.Zero;

	private static IntPtr method_isVideoAvailable = IntPtr.Zero;

	private static IntPtr method_isV4VCAvailable = IntPtr.Zero;

	private static IntPtr method_getDeviceID = IntPtr.Zero;

	private static IntPtr method_getV4VCAmount = IntPtr.Zero;

	private static IntPtr method_getV4VCName = IntPtr.Zero;

	private static IntPtr method_showVideo = IntPtr.Zero;

	private static IntPtr method_showV4VC = IntPtr.Zero;

	private static IntPtr method_offerV4VC = IntPtr.Zero;

	public static void Configure(string app_version, string app_id, params string[] zone_ids)
	{
		if (!configured)
		{
			GameObject gameObject = GameObject.Find("Main Camera");
			if (gameObject != null)
			{
				gameObject.AddComponent<AdColony>();
				AndroidConfigure(app_version, app_id, zone_ids);
			}
			else
			{
				Debug.LogError("AdColony requires a GameObject named \"Main Camera\" to attach to.");
			}
		}
	}

	public static bool IsVideoAvailable()
	{
		if (!configured)
		{
			return false;
		}
		return AndroidIsVideoAvailable(null);
	}

	public static bool IsVideoAvailable(string zone_id)
	{
		if (!configured)
		{
			return false;
		}
		return AndroidIsVideoAvailable(zone_id);
	}

	public static bool IsV4VCAvailable()
	{
		if (!configured)
		{
			Debug.LogError("V4VC not configured");
			return false;
		}
		return AndroidIsV4VCAvailable(null);
	}

	public static bool IsV4VCAvailable(string zone_id)
	{
		if (!configured)
		{
			Debug.LogError("V4VC not configured for the zone");
			return false;
		}
		return AndroidIsV4VCAvailable(zone_id);
	}

	public static string GetDeviceID()
	{
		if (!configured)
		{
			return "undefined";
		}
		return AndroidGetDeviceID();
	}

	public static string GetOpenUDID()
	{
		if (!configured)
		{
			return "undefined";
		}
		return AndroidGetOpenUDID();
	}

	public static int GetV4VCAmount()
	{
		if (!configured)
		{
			return 0;
		}
		return AndroidGetV4VCAmount(null);
	}

	public static int GetV4VCAmount(string zone_id)
	{
		if (!configured)
		{
			return 0;
		}
		return AndroidGetV4VCAmount(zone_id);
	}

	public static string GetV4VCName()
	{
		if (!configured)
		{
			return "undefined";
		}
		return AndroidGetV4VCName(null);
	}

	public static string GetV4VCName(string zone_id)
	{
		if (!configured)
		{
			return "undefined";
		}
		return AndroidGetV4VCName(zone_id);
	}

	public static bool ShowVideoAd()
	{
		if (!configured)
		{
			return false;
		}
		return AndroidShowVideoAd(null);
	}

	public static bool ShowVideoAd(string zone_id)
	{
		if (!configured)
		{
			return false;
		}
		return AndroidShowVideoAd(zone_id);
	}

	public static bool ShowV4VC(bool popup_result)
	{
		if (!configured)
		{
			return false;
		}
		return AndroidShowV4VC(popup_result, null);
	}

	public static bool ShowV4VC(bool popup_result, string zone_id)
	{
		if (!configured)
		{
			return false;
		}
		return AndroidShowV4VC(popup_result, zone_id);
	}

	public static void OfferV4VC(bool popup_result)
	{
		if (configured)
		{
			AndroidOfferV4VC(popup_result, null);
		}
	}

	public static void OfferV4VC(bool popup_result, string zone_id)
	{
		if (configured)
		{
			AndroidOfferV4VC(popup_result, zone_id);
		}
	}

	private void Awake()
	{
		base.name = "AdColony";
	}

	private void OnApplicationPause()
	{
		was_paused = true;
	}

	private void Update()
	{
		if (was_paused)
		{
			was_paused = false;
			AndroidResume();
		}
	}

	public void OnAdColonyVideoStarted(string args)
	{
		if (OnVideoStarted != null)
		{
			OnVideoStarted();
		}
		previous_timescale = Time.timeScale;
		Time.timeScale = 0f;
		saved_timescale = true;
	}

	public void OnAdColonyVideoFinished(string args)
	{
		if (saved_timescale)
		{
			saved_timescale = false;
			Time.timeScale = previous_timescale;
		}
		if (OnVideoFinished != null)
		{
			OnVideoFinished();
		}
	}

	public void OnAdColonyV4VCResult(string args)
	{
		if (OnV4VCResult != null)
		{
			int num = args.IndexOf("|");
			int num2 = args.IndexOf("|", num + 1);
			string text = args.Substring(0, num);
			string s = args.Substring(num + 1, num2 - num - 1);
			string text2 = args.Substring(num2 + 1);
			OnV4VCResult(text.Equals("true"), text2, int.Parse(s));
		}
	}

	private static void AndroidConfigure(string app_version, string app_id, string[] zone_ids)
	{
		bool flag = true;
		IntPtr intPtr = AndroidJNI.FindClass("com/jirbo/unityadc/UnityADC");
		if (intPtr != IntPtr.Zero)
		{
			class_UnityADC = AndroidJNI.NewGlobalRef(intPtr);
			AndroidJNI.DeleteLocalRef(intPtr);
			IntPtr intPtr2 = AndroidJNI.FindClass("com/jirbo/adcolony/AdColony");
			if (intPtr2 != IntPtr.Zero)
			{
				AndroidJNI.DeleteLocalRef(intPtr2);
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = false;
		}
		if (flag)
		{
			string[] array = new string[zone_ids.Length + 1];
			array[0] = app_id;
			for (int i = 0; i < zone_ids.Length; i++)
			{
				array[i + 1] = zone_ids[i];
			}
			class_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = class_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			IntPtr l = AndroidJNI.NewStringUTF(app_version);
			IntPtr l2 = AndroidJNIHelper.ConvertToJNIArray(array);
			IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(class_UnityADC, "configure", "(Landroid/app/Activity;Ljava/lang/String;[Ljava/lang/String;)V");
			jvalue[] array2 = new jvalue[3];
			array2[0].l = @static.GetRawObject();
			array2[1].l = l;
			array2[2].l = l2;
			AndroidJNI.CallStaticVoidMethod(class_UnityADC, staticMethodID, array2);
			method_resume = AndroidJNI.GetStaticMethodID(class_UnityADC, "resume", "(Landroid/app/Activity;)V");
			method_isVideoAvailable = AndroidJNI.GetStaticMethodID(class_UnityADC, "isVideoAvailable", "(Ljava/lang/String;)Z");
			method_isV4VCAvailable = AndroidJNI.GetStaticMethodID(class_UnityADC, "isV4VCAvailable", "(Ljava/lang/String;)Z");
			method_getDeviceID = AndroidJNI.GetStaticMethodID(class_UnityADC, "getDeviceID", "()Ljava/lang/String;");
			method_getV4VCAmount = AndroidJNI.GetStaticMethodID(class_UnityADC, "getV4VCAmount", "(Ljava/lang/String;)I");
			method_getV4VCName = AndroidJNI.GetStaticMethodID(class_UnityADC, "getV4VCName", "(Ljava/lang/String;)Ljava/lang/String;");
			method_showVideo = AndroidJNI.GetStaticMethodID(class_UnityADC, "showVideo", "(Ljava/lang/String;)Z");
			method_showV4VC = AndroidJNI.GetStaticMethodID(class_UnityADC, "showV4VC", "(ZLjava/lang/String;)Z");
			method_offerV4VC = AndroidJNI.GetStaticMethodID(class_UnityADC, "offerV4VC", "(ZLjava/lang/String;)V");
			configured = true;
		}
		else
		{
			Debug.LogError("AdColony configuration error - make sure adcolony.jar and unityadc.jar libraries are in your Unity project's Assets/Plugins/Android folder.");
		}
	}

	private static void AndroidResume()
	{
		AndroidJavaObject @static = class_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		jvalue[] array = new jvalue[1];
		array[0].l = @static.GetRawObject();
		AndroidJNI.CallStaticVoidMethod(class_UnityADC, method_resume, array);
	}

	private static bool AndroidIsVideoAvailable(string zone_id)
	{
		jvalue[] array = new jvalue[1];
		array[0].l = AndroidJNI.NewStringUTF(zone_id);
		return AndroidJNI.CallStaticBooleanMethod(class_UnityADC, method_isVideoAvailable, array);
	}

	private static bool AndroidIsV4VCAvailable(string zone_id)
	{
		jvalue[] array = new jvalue[1];
		array[0].l = AndroidJNI.NewStringUTF(zone_id);
		return AndroidJNI.CallStaticBooleanMethod(class_UnityADC, method_isV4VCAvailable, array);
	}

	private static string AndroidGetDeviceID()
	{
		jvalue[] args = new jvalue[0];
		return AndroidJNI.CallStaticStringMethod(class_UnityADC, method_getDeviceID, args);
	}

	private static string AndroidGetOpenUDID()
	{
		return "undefined";
	}

	private static int AndroidGetV4VCAmount(string zone_id)
	{
		jvalue[] array = new jvalue[1];
		array[0].l = AndroidJNI.NewStringUTF(zone_id);
		return AndroidJNI.CallStaticIntMethod(class_UnityADC, method_getV4VCAmount, array);
	}

	private static string AndroidGetV4VCName(string zone_id)
	{
		jvalue[] array = new jvalue[1];
		array[0].l = AndroidJNI.NewStringUTF(zone_id);
		return AndroidJNI.CallStaticStringMethod(class_UnityADC, method_getV4VCName, array);
	}

	private static bool AndroidShowVideoAd(string zone_id)
	{
		jvalue[] array = new jvalue[1];
		array[0].l = AndroidJNI.NewStringUTF(zone_id);
		AndroidJNI.CallStaticBooleanMethod(class_UnityADC, method_showVideo, array);
		return true;
	}

	private static bool AndroidShowV4VC(bool popup_result, string zone_id)
	{
		jvalue[] array = new jvalue[2];
		array[0].z = popup_result;
		array[1].l = AndroidJNI.NewStringUTF(zone_id);
		AndroidJNI.CallStaticBooleanMethod(class_UnityADC, method_showV4VC, array);
		return true;
	}

	private static void AndroidOfferV4VC(bool popup_result, string zone_id)
	{
		jvalue[] array = new jvalue[2];
		array[0].z = popup_result;
		array[1].l = AndroidJNI.NewStringUTF(zone_id);
		AndroidJNI.CallStaticVoidMethod(class_UnityADC, method_offerV4VC, array);
	}
}
