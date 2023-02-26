using System;
using UnityEngine;

public class Distort : MonoBehaviour
{
	[Serializable]
	public class EnviromentSettings
	{
		public Color backgroundColor;

		public Color materialColor = new Color(1f, 1f, 1f, 1f);
	}

	public float partLength = 700f;

	public float smoothTime = 10f;

	public Material[] materials;

	public Vector3 distortion = Vector3.zero;

	private float nextPartZ;

	public EnviromentSettings day;

	public EnviromentSettings night;

	private void Awake()
	{
		Vector3 vector = distortion / 100f;
		Material[] array = materials;
		foreach (Material material in array)
		{
			material.SetVector("_Distort", vector);
		}
	}

	public void Reset()
	{
	}
}
