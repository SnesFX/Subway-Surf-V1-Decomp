using UnityEngine;

public class ChartBoostAndroidEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		ChartBoostAndroidManager.didFinishInterstitialEvent += didFinishInterstitialEvent;
		ChartBoostAndroidManager.didFinishMoreAppsEvent += didFinishMoreAppsEvent;
		ChartBoostAndroidManager.didFailToLoadMoreAppsEvent += didFailToLoadMoreAppsEvent;
		ChartBoostAndroidManager.didFailToLoadInterstitialEvent += didFailToLoadInterstitialEvent;
	}

	private void OnDisable()
	{
		ChartBoostAndroidManager.didFinishInterstitialEvent -= didFinishInterstitialEvent;
		ChartBoostAndroidManager.didFinishMoreAppsEvent -= didFinishMoreAppsEvent;
		ChartBoostAndroidManager.didFailToLoadMoreAppsEvent -= didFailToLoadMoreAppsEvent;
		ChartBoostAndroidManager.didFailToLoadInterstitialEvent -= didFailToLoadInterstitialEvent;
	}

	private void didFinishInterstitialEvent(string param)
	{
		Debug.Log("didFinishInterstitialEvent: " + param);
	}

	private void didFinishMoreAppsEvent(string param)
	{
		Debug.Log("didFinishMoreAppsEvent: " + param);
	}

	private void didFailToLoadMoreAppsEvent()
	{
		Debug.Log("didFailToLoadMoreAppsEvent");
	}

	private void didFailToLoadInterstitialEvent()
	{
		Debug.Log("didFailToLoadInterstitialEvent");
	}
}
