using UnityEngine;

public class TestStarter : MonoBehaviour
{
	public string screenToShowAtStart;

	private void Update()
	{
		if (!string.IsNullOrEmpty(screenToShowAtStart))
		{
			UIScreenController.Instance.PushScreen(base.gameObject, screenToShowAtStart);
		}
		base.enabled = false;
	}
}
