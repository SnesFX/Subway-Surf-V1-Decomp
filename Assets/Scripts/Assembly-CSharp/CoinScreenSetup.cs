using System.Collections;
using UnityEngine;

public class CoinScreenSetup : MonoBehaviour
{
	public GameObject coinPrefab;

	public GameObject coinEarnerPrefab;

	public UIFont headerFont;

	private UITable _table;

	private UIDraggablePanel _parentDragPanel;

	private bool firstRun = true;

	private void Awake()
	{
		_table = GetComponent<UITable>();
		_parentDragPanel = NGUITools.FindInParents<UIDraggablePanel>(base.transform.parent.gameObject);
	}

	private void Start()
	{
		FillTable();
	}

	private void OnEnable()
	{
		_table.repositionNow = true;
	}

	public void RefreshCurrencyEarners()
	{
		FillTable();
	}

	private void FillTable()
	{
		base.transform.parent.GetComponent<UIPanel>().widgetsAreStatic = false;
		foreach (Transform item in base.transform)
		{
			NGUITools.SetActive(item.gameObject, false);
			Object.Destroy(item.gameObject);
		}
		int num = 0;
		UILabel uILabel = NGUITools.AddWidget<UILabel>(base.gameObject);
		uILabel.font = headerFont;
		uILabel.text = "Coin Shop";
		uILabel.color = new Color(0f, 0.2901961f, 0.5019608f, 1f);
		uILabel.name = string.Format("{0:000}", num);
		uILabel.MakePixelPerfect();
		if (DeviceInfo.isHighres)
		{
			uILabel.gameObject.transform.localScale = new Vector3(uILabel.gameObject.transform.localScale.x / 2f, uILabel.gameObject.transform.localScale.y / 2f, uILabel.gameObject.transform.localScale.z);
		}
		num++;
		GameObject gameObject = NGUITools.AddChild(base.gameObject, coinPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<CoinButtonHelper>().Init(InAppData.inAppTier1);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, coinPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<CoinButtonHelper>().Init(InAppData.inAppTier2);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, coinPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<CoinButtonHelper>().Init(InAppData.inAppTier3);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		num++;
		bool flag = false;
		for (int i = 1; i < EarnCurrencyInfo.profiles.Length; i++)
		{
			if (EarnCurrencyInfo.ShouldShowInGUI(i))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			UILabel uILabel2 = NGUITools.AddWidget<UILabel>(base.gameObject);
			uILabel2.font = headerFont;
			uILabel2.text = "Earn Coins";
			uILabel2.color = new Color(0f, 0.2901961f, 0.5019608f, 1f);
			uILabel2.name = string.Format("{0:000}", num);
			uILabel2.MakePixelPerfect();
			if (DeviceInfo.isHighres)
			{
				uILabel2.gameObject.transform.localScale = new Vector3(uILabel2.gameObject.transform.localScale.x / 2f, uILabel2.gameObject.transform.localScale.y / 2f, uILabel2.gameObject.transform.localScale.z);
			}
			num++;
			for (int j = 1; j < EarnCurrencyInfo.profiles.Length; j++)
			{
				if (EarnCurrencyInfo.ShouldShowInGUI(j))
				{
					EarnCurrencyInfo.EarnCurrencyProfile earnCurrencyProfile = EarnCurrencyInfo.profiles[j];
					gameObject = NGUITools.AddChild(base.gameObject, coinEarnerPrefab);
					gameObject.name = string.Format("{0:000}", num);
					string desc = string.Format(earnCurrencyProfile.desc, earnCurrencyProfile.amountOfCoins);
					gameObject.GetComponent<CoinEarnerButtonHelper>().Init(j, earnCurrencyProfile.title, desc, earnCurrencyProfile.iconName);
					gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
					NGUITools.AddWidgetCollider(gameObject);
					num++;
				}
			}
		}
		base.gameObject.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		if (base.gameObject.active)
		{
			_table.Reposition();
			if (!firstRun)
			{
				_parentDragPanel.RestrictWithinBounds(true);
			}
			else
			{
				firstRun = false;
			}
		}
		StartCoroutine(SetStatic());
	}

	private IEnumerator SetStatic()
	{
		yield return null;
		base.transform.parent.GetComponent<UIPanel>().widgetsAreStatic = true;
	}
}
