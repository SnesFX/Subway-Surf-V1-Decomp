using System;
using System.Collections.Generic;
using UnityEngine;

public class NewVersionPopup : MonoBehaviour
{
	private const string ONLINESETTINGS_LATESTVERSION_KEY = "latestversion";

	private const string ONLINESETTINGS_LATESTVERSION_CHANGELIST_KEY = "latestversion_changelist";

	private const string NEWVERSIONPOPUP_LAST_SHOWN_VERSION_KEY = "newverlastshown";

	private const string APPSTORE_URL = "http://itunes.apple.com/app/subway-surfers/id512939461";

	private const string CALLOUT_TEXT = "Update Available!";

	private const string DOWNLOADBUTTON_TEXT = "Download";

	public UILabel calloutLabel;

	public UILabel changeListLabel;

	public UILabel downloadButtonLabel;

	private void OnEnable()
	{
		calloutLabel.text = "Update Available!";
		downloadButtonLabel.text = "Download";
		string valueString;
		if (OnlineSettings.instance.TryGetValue("latestversion_changelist", out valueString))
		{
			changeListLabel.text = valueString;
			return;
		}
		Debug.LogError("Showing NewVersion popup, but no changeList found in OnlineSettings", this);
		changeListLabel.text = string.Empty;
	}

	private void CloseButtonClicked()
	{
		UIScreenController.Instance.ClosePopup();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Result", "Cancel");
	}

	private void DownloadButtonClicked()
	{
		Application.OpenURL("http://itunes.apple.com/app/subway-surfers/id512939461");
		UIScreenController.Instance.ClosePopup();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Result", "Ok");
	}

	public static void ShowIfNeeded()
	{
		string valueString;
		if (!OnlineSettings.instance.TryGetValue("latestversion", out valueString))
		{
			return;
		}
		string versionName = DeviceUtility.GetVersionName();
		bool flag = false;
		try
		{
			if (Utility.CompareVersions(versionName, valueString) < 0)
			{
				flag = true;
			}
		}
		catch (FormatException ex)
		{
			Debug.LogError("Failed to parse versions for comparison: " + versionName + " and " + valueString + "  : " + ex);
		}
		if (flag && PlayerPrefs.GetString("newverlastshown", string.Empty) != valueString)
		{
			PlayerPrefs.SetString("newverlastshown", valueString);
			UIScreenController.Instance.QueuePopup("NewVersionPopup");
		}
	}
}
