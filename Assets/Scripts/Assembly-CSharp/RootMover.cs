using UnityEngine;

public class RootMover : MonoBehaviour
{
	private void Awake()
	{
		UIAnchor component = GetComponent<UIAnchor>();
		if (DeviceInfo.formFactor == DeviceInfo.FormFactor.medium)
		{
			component.relativeOffset = new Vector2(0f, 0.05f);
		}
		else if (DeviceInfo.formFactor == DeviceInfo.FormFactor.large)
		{
			component.relativeOffset = new Vector2(0f, 0.05f);
		}
	}
}
