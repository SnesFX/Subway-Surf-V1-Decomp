using UnityEngine;

public class FacebookPayoutPopup : MonoBehaviour
{
	private const string TITLE_TEXT = "Congratulations";

	private const string DESC_TEXT = "You have connected to Facebook and have been awarded {0} coins!";

	public UILabel titleLabel;

	public UILabel descLabel;

	private void Start()
	{
		titleLabel.text = "Congratulations";
		descLabel.text = string.Format("You have connected to Facebook and have been awarded {0} coins!", 5000);
	}
}
