using System;
using UnityEngine;

public class Mirror : MonoBehaviour
{
	private Transform[] children;

	private TrackObject trackObject;

	private static int[] counts = new int[2];

	private void Awake()
	{
		trackObject = GetComponent<TrackObject>() ?? base.gameObject.AddComponent<TrackObject>();
		TrackObject obj = trackObject;
		obj.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(obj.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
		children = new Transform[base.transform.GetChildCount()];
		for (int i = 0; i < base.transform.GetChildCount(); i++)
		{
			children[i] = base.transform.GetChild(i);
		}
	}

	public void OnActivate()
	{
		int num = UnityEngine.Random.Range(0, 2) * 2 - 1;
		for (int i = 0; i < children.Length; i++)
		{
			Vector3 localPosition = children[i].localPosition;
			localPosition.x *= num;
			children[i].localPosition = localPosition;
		}
	}
}
