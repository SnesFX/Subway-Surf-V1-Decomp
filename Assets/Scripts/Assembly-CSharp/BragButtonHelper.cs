using UnityEngine;

public class BragButtonHelper : MonoBehaviour
{
	public UISlicedSprite fill;

	private Color activeColor = new Color(26f / 51f, 40f / 51f, 9f / 85f, 1f);

	private Color inactiveColor = new Color(0.8f, 0.8f, 0.8f, 1f);

	[HideInInspector]
	public bool buttonEnabled;

	public void EnableButton()
	{
		NGUITools.AddWidgetCollider(base.gameObject);
		fill.color = activeColor;
		buttonEnabled = true;
	}

	public void DisableButton()
	{
		if (base.gameObject.GetComponent<Collider>() != null)
		{
			Object.Destroy(base.gameObject.GetComponent<Collider>());
		}
		fill.color = inactiveColor;
		buttonEnabled = false;
	}
}
