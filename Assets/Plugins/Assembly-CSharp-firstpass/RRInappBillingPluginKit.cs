using System;
using UnityEngine;

public class RRInappBillingPluginKit : IDisposable
{
	void IDisposable.Dispose()
	{
	}

	public static bool InitInAppBillingSupport()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(BillingCONST.PLUGIN_PKG + ".InAppBillingManager");
		return androidJavaClass.CallStatic<bool>("IsBillingSupported", new object[0]);
	}

	public static bool BuyProduct(string productId)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(BillingCONST.PLUGIN_PKG + ".InAppBillingManager");
		bool flag = androidJavaClass.CallStatic<bool>("BuyItem", new object[1] { productId });
		Debug.Log(string.Format("In app billing response: {0}", flag));
		return flag;
	}
}
