using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Stretch")]
public class UIStretch : MonoBehaviour
{
	public enum Style
	{
		None = 0,
		Horizontal = 1,
		Vertical = 2,
		Both = 3,
		BasedOnHeight = 4
	}

	public bool modifyPosition = true;

	private bool modifyHeight;

	public Camera uiCamera;

	public Style style;

	public Vector2 relativeSize = Vector2.one;

	private Transform mTrans;

	private UIRoot mRoot;

	private void Awake()
	{
		Debug.Log("modifyPosition " + modifyPosition, base.gameObject);
		if (modifyPosition)
		{
			Debug.Log("modifyPosition Doing it anyway!", base.gameObject);
			if (DeviceInfo.formFactor == DeviceInfo.FormFactor.medium)
			{
				base.transform.position = new Vector3(0f, -28f, 0f);
				modifyHeight = true;
			}
			else if (DeviceInfo.formFactor == DeviceInfo.FormFactor.large)
			{
				base.transform.position = new Vector3(0f, -28f, 0f);
				modifyHeight = true;
			}
		}
	}

	private void OnEnable()
	{
		if (uiCamera == null)
		{
			uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
	}

	private void Update()
	{
		if (!(uiCamera != null) || style == Style.None)
		{
			return;
		}
		if (mTrans == null)
		{
			mTrans = base.transform;
		}
		Rect pixelRect = uiCamera.pixelRect;
		float num = pixelRect.width;
		float num2 = pixelRect.height;
		if (modifyHeight)
		{
			num2 = pixelRect.height - 25f;
		}
		if (mRoot != null && !mRoot.automatic && num2 > 1f)
		{
			float num3 = (float)mRoot.manualHeight / num2;
			num *= num3;
			num2 *= num3;
		}
		Vector3 localScale = mTrans.localScale;
		if (style == Style.BasedOnHeight)
		{
			localScale.x = relativeSize.x * num2;
			localScale.y = relativeSize.y * num2;
		}
		else
		{
			if (style == Style.Both || style == Style.Horizontal)
			{
				localScale.x = relativeSize.x * num;
			}
			if (style == Style.Both || style == Style.Vertical)
			{
				localScale.y = relativeSize.y * num2;
			}
		}
		if (mTrans.localScale != localScale)
		{
			mTrans.localScale = localScale;
		}
	}
}
