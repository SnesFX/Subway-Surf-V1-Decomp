using UnityEngine;

public class BackBtnBehaviourAndroid : MonoBehaviour
{
	public enum ScreenChangeType
	{
		PushScreen = 0,
		SwitchScreen = 1,
		BackToPrevious = 2,
		QueuePopup = 3,
		ClosePopup = 4
	}

	private GameObject target;

	private string functionName = string.Empty;

	public ScreenChangeType screenChangeType;

	public string ScreenNameToOpen;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Send();
		}
	}

	private void Awake()
	{
		if (screenChangeType == ScreenChangeType.PushScreen)
		{
			functionName = "PushScreen";
		}
		else if (screenChangeType == ScreenChangeType.SwitchScreen)
		{
			functionName = "SwitchScreen";
		}
		else if (screenChangeType == ScreenChangeType.BackToPrevious)
		{
			functionName = "BackToPrevious";
		}
		else if (screenChangeType == ScreenChangeType.QueuePopup)
		{
			functionName = "QueuePopup";
		}
		else if (screenChangeType == ScreenChangeType.ClosePopup)
		{
			functionName = "ClosePopup";
		}
	}

	protected void Send()
	{
		if (base.enabled && base.gameObject.active)
		{
			if (string.IsNullOrEmpty(ScreenNameToOpen) && (screenChangeType == ScreenChangeType.PushScreen || screenChangeType == ScreenChangeType.SwitchScreen || screenChangeType == ScreenChangeType.QueuePopup))
			{
				Debug.LogError(base.name + " tried to send an empty Change Screen message");
			}
			if (target == null)
			{
				target = MessageCenter.Instance.gameObject;
			}
			Transform[] componentsInChildren = target.GetComponentsInChildren<Transform>();
			Transform[] array = componentsInChildren;
			foreach (Transform transform in array)
			{
				transform.gameObject.SendMessage(functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
