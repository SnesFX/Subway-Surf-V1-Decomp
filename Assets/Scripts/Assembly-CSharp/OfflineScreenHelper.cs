using UnityEngine;

public class OfflineScreenHelper : MonoBehaviour
{
	public GameObject GameCenterBonus;

	public GameObject FacebookBonus;

	public void InitOfflineScreen()
	{
		if (PlayerInfo.Instance.hasPayedOutFacebook)
		{
			NGUITools.SetActive(FacebookBonus, false);
		}
		if (PlayerInfo.Instance.hasPayedOutGameCenter)
		{
			NGUITools.SetActive(GameCenterBonus, false);
		}
	}
}
