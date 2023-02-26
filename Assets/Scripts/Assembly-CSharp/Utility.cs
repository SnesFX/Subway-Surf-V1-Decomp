using UnityEngine;

public static class Utility
{
	public static void SetLayerRecursively(Transform t, int layer)
	{
		t.gameObject.layer = layer;
		foreach (Transform item in t)
		{
			SetLayerRecursively(item, layer);
		}
	}

	public static int NumberOfDigits(int number)
	{
		int num = 0;
		if (number == 0)
		{
			return 1;
		}
		while (number != 0)
		{
			number /= 10;
			num++;
		}
		return num;
	}

	public static int CompareVersions(string leftVersion, string rightVersion)
	{
		string[] array = leftVersion.Split('.');
		string[] array2 = rightVersion.Split('.');
		for (int i = 0; i < array.Length || i < array2.Length; i++)
		{
			int num = ((i < array.Length) ? int.Parse(array[i]) : 0);
			int num2 = ((i < array2.Length) ? int.Parse(array2[i]) : 0);
			if (num != num2)
			{
				return num - num2;
			}
		}
		return 0;
	}
}
