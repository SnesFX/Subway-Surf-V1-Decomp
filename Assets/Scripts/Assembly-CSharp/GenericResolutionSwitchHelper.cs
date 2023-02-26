using UnityEngine;

public class GenericResolutionSwitchHelper : MonoBehaviour
{
	public string highResName;

	private void Start()
	{
		if (DeviceInfo.isHighres)
		{
			UISprite component = GetComponent<UISprite>();
			component.spriteName = highResName;
		}
	}
}
