using UnityEngine;

public class WhatsNew : MonoBehaviour
{
	private const string PATH = "WhatsNew/";

	private const string LAST_SEEN_BUNDLE_VERSION_KEY = "lastSeenBundleVersionKey";

	private void Start()
	{
		if (ShouldDisplayWhatsNew())
		{
			UIScreenController.Instance.QueuePopup("WhatsNewPopup");
		}
	}

	private bool ShouldDisplayWhatsNew()
	{
		if (Application.isEditor)
		{
			return true;
		}
		string versionName = DeviceUtility.GetVersionName();
		string @string = PlayerPrefs.GetString("lastSeenBundleVersionKey", string.Empty);
		if (@string.Equals(versionName))
		{
			return false;
		}
		PlayerPrefs.SetString("lastSeenBundleVersionKey", DeviceUtility.GetVersionName());
		return true;
	}

	public static string[] getNewsForCurrentVersion()
	{
		string versionName = DeviceUtility.GetVersionName();
		int versionCode = DeviceUtility.GetVersionCode();
		string path = "WhatsNew/" + versionName;
		TextAsset textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogWarning("NO UPDATE INFO AVALIBLE FOR VERSION: " + versionName);
			return null;
		}
		if (textAsset.text.Contains("\r"))
		{
			textAsset.text.Replace("\r", "\n");
		}
		string[] array = textAsset.text.Split('\n');
		if (array.Length == 0)
		{
			return null;
		}
		return array;
	}
}
