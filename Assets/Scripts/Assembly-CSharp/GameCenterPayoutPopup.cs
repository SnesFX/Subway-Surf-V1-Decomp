using UnityEngine;

public class GameCenterPayoutPopup : MonoBehaviour
{
	private const string TITLE_TEXT = "Congratulations";

	private const string DESC_TEXT = "You have logged into Game Center and have been awarded {0} coins!";

	public UILabel titleLabel;

	public UILabel descLabel;

	private void Start()
	{
		titleLabel.text = "Congratulations";
		descLabel.text = string.Format("You have logged into Game Center and have been awarded {0} coins!", 250);
	}
}
