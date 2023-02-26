using UnityEngine;

public class UIHoverboardUpdateLabel : MonoBehaviour
{
	[SerializeField]
	private UILabel amountLabel;

	private void OnEnable()
	{
		PurchaseHandler.Instance.AddOnUpgradePurchase(UpdateLabels);
		UpdateLabels();
	}

	private void OnDisable()
	{
		PurchaseHandler.Instance.RemoveOnUpgradePurchase(UpdateLabels);
	}

	private void UpdateLabels()
	{
		amountLabel.text = PlayerInfo.Instance.GetUpgradeAmount(PowerupType.hoverboard).ToString();
	}
}
