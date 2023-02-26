using System;
using System.Collections.Generic;
using UnityEngine;

public class AdColonyCallbackHandler : MonoBehaviour
{
	public const string ADCOLONY_APPVERSION = "1.0";

	public const string ADCOLONY_APPID = "appc0d018638a3a47a3b725ab";

	public const string ADCOLONY_V4VC_SECRET = "v4vc12440e42dcca4e52bca55e";

	public const string ADCOLONY_ZONEID = "vzc54d2d8389a24681852d05";

	private const string ADCOLONY_NOVIDEOS_NATIVE_POPUP_TITLE = "No videos available";

	private const string ADCOLONY_NOVIDEOS_NATIVE_POPUP_MESSAGE = "We are currently out of videos to show you. Please try again later.";

	private const string ADCOLONY_NOVIDEOS_NATIVE_POPUP_OKBUTTONTEXT = "OK";

	private bool adColonySoundOnBeforeMute;

	private void Awake()
	{
		AdColony.OnVideoStarted = (AdColony.VideoStartedDelegate)Delegate.Combine(AdColony.OnVideoStarted, new AdColony.VideoStartedDelegate(OnVideoStarted));
		AdColony.OnVideoFinished = (AdColony.VideoFinishedDelegate)Delegate.Combine(AdColony.OnVideoFinished, new AdColony.VideoFinishedDelegate(OnVideoFinished));
		AdColony.OnV4VCResult = (AdColony.V4VCResultDelegate)Delegate.Combine(AdColony.OnV4VCResult, new AdColony.V4VCResultDelegate(OnV4VCResult));
	}

	private void OnDestroy()
	{
		AdColony.OnVideoStarted = (AdColony.VideoStartedDelegate)Delegate.Remove(AdColony.OnVideoStarted, new AdColony.VideoStartedDelegate(OnVideoStarted));
		AdColony.OnVideoFinished = (AdColony.VideoFinishedDelegate)Delegate.Remove(AdColony.OnVideoFinished, new AdColony.VideoFinishedDelegate(OnVideoFinished));
		AdColony.OnV4VCResult = (AdColony.V4VCResultDelegate)Delegate.Remove(AdColony.OnV4VCResult, new AdColony.V4VCResultDelegate(OnV4VCResult));
	}

	private void OnVideoStarted()
	{
		adColonySoundOnBeforeMute = Settings.optionSound;
		Settings.optionSound = false;
	}

	private void OnVideoFinished()
	{
		Settings.optionSound = adColonySoundOnBeforeMute;
	}

	private void OnV4VCResult(bool success, string name, int amount)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (success)
		{
			PlayerInfo.Instance.amountOfCoins += amount;
			PlayerInfo.Instance.SaveIfDirty();
			dictionary.Add("video is received", "Ok");
		}
		else
		{
			dictionary.Add("video is received", "No video");
		}
	}

	private void VideoAdNotServed()
	{
		EtceteraAndroid.showAlert("No videos available", "We are currently out of videos to show you. Please try again later.", "OK");
	}
}
