using UnityEngine;

public static class DeviceInfo
{
	public enum FormFactor
	{
		iPad = 0,
		small = 1,
		medium = 2,
		large = 3
	}

	public enum PerformanceLevel
	{
		Low = 0,
		Medium = 1,
		High = 2
	}

	public static string deviceModel;

	public static readonly FormFactor formFactor;

	public static readonly bool isHighres;

	static DeviceInfo()
	{
		deviceModel = SystemInfo.deviceModel;
		if (Screen.height < 500)
		{
			formFactor = FormFactor.small;
			Debug.Log("Form factor small");
		}
		else if (Screen.height < 900)
		{
			formFactor = FormFactor.medium;
			Debug.Log("Form factor medium");
		}
		else
		{
			formFactor = FormFactor.large;
			Debug.Log("Form factor large");
		}
		if (isTablet())
		{
			formFactor = FormFactor.iPad;
			Debug.Log("Form factor tablet");
		}
		if (Screen.height >= 960 && Screen.width >= 640)
		{
			isHighres = true;
		}
		else
		{
			isHighres = false;
		}
		Debug.Log("High res set to: " + isHighres);
	}

	private static bool isTablet()
	{
		float f = (float)Screen.width / Screen.dpi;
		float f2 = (float)Screen.height / Screen.dpi;
		double num = Mathf.Sqrt(Mathf.Pow(f, 2f) + Mathf.Pow(f2, 2f));
		Debug.Log("size of inches: " + num);
		return num >= 6.0;
	}
}
