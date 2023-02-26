using UnityEngine;

public class ForceCoinScreenButton : MonoBehaviour
{
	private void OnClick()
	{
		UIScreenController.Instance.ClosePopup();
		UIScreenController.Instance.QueuePopup("CoinsUI_quick");
	}
}
