using UnityEngine;

[RequireComponent(typeof(UIRoot))]
public class RootScaler : MonoBehaviour
{
	private const float factor = 1.16f;

	private UIRoot myUIRoot;

	private void Awake()
	{
		myUIRoot = base.gameObject.GetComponent<UIRoot>();
		Debug.Log("Screen formfactor: " + DeviceInfo.formFactor);
		if (DeviceInfo.formFactor == DeviceInfo.FormFactor.iPad)
		{
			Debug.Log("iPad screen");
			int num = Mathf.RoundToInt(myUIRoot.manualHeight * 16 / 15);
			Debug.Log("New height: " + num);
			myUIRoot.manualHeight = num;
		}
		else if (DeviceInfo.formFactor == DeviceInfo.FormFactor.small)
		{
			myUIRoot.manualHeight = myUIRoot.manualHeight;
		}
		else if (DeviceInfo.formFactor == DeviceInfo.FormFactor.medium)
		{
			myUIRoot.manualHeight = (int)((float)myUIRoot.manualHeight * 1.16f);
		}
		else if (DeviceInfo.formFactor == DeviceInfo.FormFactor.large)
		{
			myUIRoot.manualHeight = (int)((float)myUIRoot.manualHeight * 1.16f);
		}
		Debug.Log("Screen real size: " + Screen.width + "*" + Screen.height + ", " + 1.16f);
		Debug.Log("Screen Manual height: " + myUIRoot.manualHeight);
	}
}
