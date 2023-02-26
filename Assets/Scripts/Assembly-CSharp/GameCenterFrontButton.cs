using UnityEngine;

public class GameCenterFrontButton : MonoBehaviour
{
	private void OnClick()
	{
		Debug.Log("Game Center button clicked");
		if (Social.localUser.authenticated)
		{
		}
	}
}
