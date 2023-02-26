using UnityEngine;

public class BuyHoverboardButton : MonoBehaviour
{
	private bool _purchaseInProgress;

	[SerializeField]
	private UILabel amountLabel;

	[SerializeField]
	private UILabel priceLabel;

	private void Start()
	{
		priceLabel.text = Upgrades.upgrades[PowerupType.hoverboard].getPrice(0).ToString();
	}

	private void OnEnable()
	{
		PurchaseHandler.Instance.AddOnUpgradePurchase(UpdateLabels);
		UpdateLabels();
	}

	private void OnDisable()
	{
		PurchaseHandler.Instance.RemoveOnUpgradePurchase(UpdateLabels);
	}

	private void OnClick()
	{
		if (!_purchaseInProgress)
		{
			PurchaseHandler.Instance.PurchaseHoverboard(this);
		}
	}

	private void UpdateLabels()
	{
		amountLabel.text = PlayerInfo.Instance.GetUpgradeAmount(PowerupType.hoverboard).ToString();
	}

	public void PurchaseSuccessful()
	{
		_purchaseInProgress = false;
		UpdateLabels();
	}

	public void PurchaseFailure()
	{
		_purchaseInProgress = false;
	}
}
