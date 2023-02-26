using UnityEngine;

public class ChartBoostUIManager : MonoBehaviour
{
	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float width = ((Screen.width < 800 && Screen.height < 800) ? 160 : 320);
		float num2 = ((Screen.width < 800 && Screen.height < 800) ? 40 : 80);
		float num3 = num2 + 10f;
		if (GUI.Button(new Rect(left, num, width, num2), "Init"))
		{
			ChartBoostAndroid.init("YOUR_APP_ID", "YOUR_APP_SIGNATURE");
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "Cache Interstitial"))
		{
			ChartBoostAndroid.cacheInterstitial(null);
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "Check for Cached Interstitial"))
		{
			Debug.Log("has cached interstitial: " + ChartBoostAndroid.hasCachedInterstitial(null));
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "Show Interstitial"))
		{
			ChartBoostAndroid.showInterstitial(null);
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "Cache More Apps"))
		{
			ChartBoostAndroid.cacheMoreApps();
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "Has Cached More Apps"))
		{
			Debug.Log("has cached more apps: " + ChartBoostAndroid.hasCachedMoreApps());
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "Show More Apps"))
		{
			ChartBoostAndroid.showMoreApps();
		}
	}
}
