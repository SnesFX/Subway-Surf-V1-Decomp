using System;
using UnityEngine;

public class TrophyLoader : MonoBehaviour
{
	public GameObject trophyPrefab;

	private UIGrid grid;

	private void Awake()
	{
		grid = GetComponent<UIGrid>();
		loadTrophies();
	}

	private void loadTrophies()
	{
		foreach (Transform item in base.transform)
		{
			NGUITools.SetActive(item.gameObject, false);
			UnityEngine.Object.Destroy(item.gameObject);
		}
		Trophies.Trophy[] array = Enum.GetValues(typeof(Trophies.Trophy)) as Trophies.Trophy[];
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(base.gameObject, trophyPrefab);
			gameObject.name = string.Format("{0:000}trophy", i);
			TrophyHelper component = gameObject.GetComponent<TrophyHelper>();
			component.setTrophy(array[i]);
		}
	}
}
