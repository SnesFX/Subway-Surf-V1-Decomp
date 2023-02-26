using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppManager : MonoBehaviour
{
	private enum InAppPurchaseState
	{
		NotStarted = 0,
		Started = 1,
		Failed = 2,
		Complete = 3
	}

	private InAppData inAppData;

	private static InAppManager _instance;

	private InAppPurchaseState _inAppPurchaseState;

	[HideInInspector]
	public bool productRequestSucceeded;

	public Action onProductRequestSuccess;

	public Action onPurchaseSuccess;

	public static InAppManager Instance
	{
		get
		{
			return _instance ?? (_instance = UnityEngine.Object.FindObjectOfType(typeof(InAppManager)) as InAppManager);
		}
	}

	public static bool IsInstanced()
	{
		return _instance != null;
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		productRequestSucceeded = RRInappBillingPluginKit.InitInAppBillingSupport();
		inAppData = new InAppData();
	}

	public void BuyInAppNow(GameObject sender)
	{
		string key = sender.GetComponent<CoinButtonHelper>().Key;
		StartPurchase(key);
	}

	public void BuyFromPopup(string purchaseId)
	{
		StartPurchase(purchaseId);
	}

	private void StartPurchase(string inAppPurchaseId)
	{
		if (RRInappBillingPluginKit.InitInAppBillingSupport())
		{
			UIScreenController.Instance.ShowInAppPurchaseOverlay();
			if (!RRInappBillingPluginKit.BuyProduct(inAppPurchaseId))
			{
				UIScreenController.Instance.HideInAppPurchaseOverlay();
				EtceteraAndroid.showAlert("Alert", "There was an error while trying to connect to the server. Please try again later!", "Ok");
			}
		}
	}

	public void PurchaseSuccess(string productId)
	{
		PlayerInfo.Instance.inAppPurchaseCount++;
		PlayerInfo.Instance.amountOfCoins += InAppData.inAppData[productId].amountOfCoins;
		PlayerInfo.Instance.SaveIfDirty();
		UIScreenController.Instance.HideInAppPurchaseOverlay();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Id", productId);
		dictionary.Clear();
		if (productId == InAppData.inAppTier1)
		{
			dictionary.Add("Mission Set", PlayerInfo.Instance.currentMissionSet.ToString());
		}
		else if (productId == InAppData.inAppTier2)
		{
			dictionary.Add("Mission Set", PlayerInfo.Instance.currentMissionSet.ToString());
		}
		else if (productId == InAppData.inAppTier3)
		{
			dictionary.Add("Mission Set", PlayerInfo.Instance.currentMissionSet.ToString());
		}
	}

	public void PurchaseFailure(string error)
	{
		UIScreenController.Instance.HideInAppPurchaseOverlay();
	}

	public void PurchaseCancel(string error)
	{
		UIScreenController.Instance.HideInAppPurchaseOverlay();
	}

	private void OnEnable()
	{
		RRInappBillingCallback.purchaseSuccessful += PurchaseSuccess;
		RRInappBillingCallback.purchaseFailed += PurchaseFailure;
		RRInappBillingCallback.purchaseCancelled += PurchaseCancel;
	}

	private void OnDisable()
	{
		RRInappBillingCallback.purchaseSuccessful -= PurchaseSuccess;
		RRInappBillingCallback.purchaseFailed -= PurchaseFailure;
		RRInappBillingCallback.purchaseCancelled -= PurchaseCancel;
	}

	private void OnDestroy()
	{
	}

	public void RetryProductRequest()
	{
		if (!productRequestSucceeded)
		{
			StartCoroutine(QueryInAppPurchases());
		}
	}

	private IEnumerator QueryInAppPurchases()
	{
		if (WillQueryInAppPurchases())
		{
			if (!InAppPurchaseHandler.isInitializedForProductRequest())
			{
				InAppPurchaseHandler.initProductRequest(base.gameObject.name, "ProductRequestSuccess", "ProductRequestFailure");
			}
			InAppPurchaseHandler.queryProducts(inAppData.CommaSeparatedProductIds);
		}
		yield break;
	}

	public bool WillQueryInAppPurchases()
	{
		if (_inAppPurchaseState == InAppPurchaseState.NotStarted || _inAppPurchaseState == InAppPurchaseState.Failed)
		{
			_inAppPurchaseState = InAppPurchaseState.Started;
			return true;
		}
		return false;
	}

	public bool HasQueriedInAppPurchases()
	{
		return _inAppPurchaseState == InAppPurchaseState.Complete;
	}

	public bool IsQueryingInAppPurchases()
	{
		return _inAppPurchaseState == InAppPurchaseState.Started;
	}

	public void ProductRequestSuccess(string validProductIdsAndPrices)
	{
		string[] array = validProductIdsAndPrices.Split(";"[0]);
		int num = array.Length / 2;
		string[] array2 = new string[num];
		string[] array3 = new string[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = array[i * 2];
			array3[i] = array[i * 2 + 1];
		}
		for (int j = 0; j < num; j++)
		{
			InAppData.inAppData[array2[j]].price = array3[j];
			InAppData.inAppData[array2[j]].validInApp = true;
		}
		Action action = onProductRequestSuccess;
		if (action != null)
		{
			action();
		}
		_inAppPurchaseState = InAppPurchaseState.Complete;
		productRequestSucceeded = true;
	}

	public void ProductRequestFailure()
	{
		_inAppPurchaseState = InAppPurchaseState.Failed;
		productRequestSucceeded = false;
	}
}
