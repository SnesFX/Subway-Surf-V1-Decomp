using System.Runtime.InteropServices;
using UnityEngine;

public class InAppPurchaseHandler
{
	private static bool initializedForPurchase;

	private static bool initializedForProductRequest;

	private static string editorPurchaseGameObjectName;

	private static string editorProductRequestGameObjectName;

	private static string editorOnPurchaseSuccessMethodName;

	private static string editorOnPurchaseFailureMethodName;

	private static string editorOnProductRequestSuccessMethodName;

	private static string editorOnProductRequestFailureMethodName;

	public static bool isInitializedForPurchase()
	{
		return initializedForPurchase;
	}

	public static bool isInitializedForProductRequest()
	{
		return initializedForProductRequest;
	}

	public static string parseProductIdFromCallbackString(string transactionAndProductId)
	{
		return transactionAndProductId.Split(',')[1];
	}

	[DllImport("__Internal")]
	private static extern bool purchaseHandlerCanMakePayments();

	public static bool canMakePayments()
	{
		if (Application.isEditor)
		{
			return true;
		}
		return purchaseHandlerCanMakePayments();
	}

	[DllImport("__Internal")]
	private static extern void purchaseHandlerInitPurchase(string gameobjectName, string onSuccessMethodName, string onFailureMethodName);

	public static void initPurchase(string gameobjectName, string onSuccessMethodName, string onFailureMethodName)
	{
		if (initializedForPurchase)
		{
			Debug.LogError("PurchaseHandler already initialized for purchase");
			return;
		}
		if (Application.isEditor)
		{
			editorPurchaseGameObjectName = gameobjectName;
			editorOnPurchaseSuccessMethodName = onSuccessMethodName;
			editorOnPurchaseFailureMethodName = onFailureMethodName;
		}
		else
		{
			purchaseHandlerInitPurchase(gameobjectName, onSuccessMethodName, onFailureMethodName);
		}
		initializedForPurchase = true;
	}

	[DllImport("__Internal")]
	private static extern void purchaseHandlerResetForPurchase();

	public static void resetForPurchase()
	{
		if (Application.isEditor)
		{
			editorPurchaseGameObjectName = null;
			editorOnPurchaseSuccessMethodName = null;
			editorOnPurchaseFailureMethodName = null;
		}
		else
		{
			purchaseHandlerResetForPurchase();
		}
		initializedForPurchase = false;
	}

	[DllImport("__Internal")]
	private static extern void purchaseHandlerInitProductRequest(string gameobjectName, string onSuccessMethodName, string onFailureMethodName);

	public static void initProductRequest(string gameobjectName, string onSuccessMethodName, string onFailureMethodName)
	{
		if (initializedForProductRequest)
		{
			Debug.LogError("PurchaseHandler already initialized for purchase");
			return;
		}
		if (Application.isEditor)
		{
			editorProductRequestGameObjectName = gameobjectName;
			editorOnProductRequestSuccessMethodName = onSuccessMethodName;
			editorOnProductRequestFailureMethodName = onFailureMethodName;
		}
		else
		{
			purchaseHandlerInitProductRequest(gameobjectName, onSuccessMethodName, onFailureMethodName);
		}
		initializedForProductRequest = true;
	}

	[DllImport("__Internal")]
	private static extern void purchaseHandlerResetForProductRequest();

	public static void resetForProductRequest()
	{
		if (Application.isEditor)
		{
			editorProductRequestGameObjectName = null;
			editorOnProductRequestSuccessMethodName = null;
			editorOnProductRequestFailureMethodName = null;
		}
		else
		{
			purchaseHandlerResetForProductRequest();
		}
		initializedForProductRequest = false;
	}

	[DllImport("__Internal")]
	private static extern void purchaseHandlerStartPurchase(string productIdentifier);

	public static void startPurchase(string productIdentifier)
	{
		Debug.Log("PurchaseHandler.startPurchase(" + productIdentifier + ")");
		if (!initializedForPurchase)
		{
			Debug.LogError("PurchaseHandler not initialized for purchase");
		}
		else if (Application.isEditor)
		{
			GameObject.Find(editorPurchaseGameObjectName).SendMessage(editorOnPurchaseSuccessMethodName, "," + productIdentifier);
		}
		else
		{
			purchaseHandlerStartPurchase(productIdentifier);
		}
	}

	[DllImport("__Internal")]
	private static extern void purchaseHandlerCallbackHasBeenHandled(string transactionAndProductId);

	public static void callbackHasBeenHandled(string transactionAndProductIdentifier)
	{
		if (!initializedForPurchase)
		{
			Debug.LogError("PurchaseHandler not initialized for purchase");
		}
		else if (!Application.isEditor)
		{
			purchaseHandlerCallbackHasBeenHandled(transactionAndProductIdentifier);
		}
	}

	[DllImport("__Internal")]
	private static extern void purchaseHandlerQueryProducts(string productIds);

	public static void queryProducts(string productIds)
	{
		if (!initializedForProductRequest)
		{
			Debug.LogError("PurchaseHandler not initialized for product request");
		}
		else if (Application.isEditor)
		{
			string[] array = productIds.Split(',');
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					text += ";";
				}
				text = text + array[i] + ";0,99GBP";
			}
			GameObject.Find(editorProductRequestGameObjectName).SendMessage(editorOnProductRequestSuccessMethodName, text);
		}
		else
		{
			purchaseHandlerQueryProducts(productIds);
		}
	}
}
