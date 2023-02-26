using System.Runtime.CompilerServices;
using UnityEngine;

public class RRInappBillingCallback : MonoBehaviour
{
	public delegate void ProductPurchasedEventHandler(string productIdentifier);

	public delegate void StoreKitErrorEventHandler(string error);

	[method: MethodImpl(32)]
	public static event StoreKitErrorEventHandler purchaseFailed;

	[method: MethodImpl(32)]
	public static event StoreKitErrorEventHandler purchaseCancelled;

	[method: MethodImpl(32)]
	public static event ProductPurchasedEventHandler purchaseSuccessful;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void productPurchased(string productIdentifier)
	{
		if (RRInappBillingCallback.purchaseSuccessful != null)
		{
			RRInappBillingCallback.purchaseSuccessful(productIdentifier);
		}
	}

	public void productPurchaseFailed(string error)
	{
		if (RRInappBillingCallback.purchaseFailed != null)
		{
			RRInappBillingCallback.purchaseFailed(error);
		}
	}

	public void productPurchaseCancelled(string error)
	{
		if (RRInappBillingCallback.purchaseCancelled != null)
		{
			RRInappBillingCallback.purchaseCancelled(error);
		}
	}
}
