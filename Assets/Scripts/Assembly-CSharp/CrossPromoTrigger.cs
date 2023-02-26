using System;
using System.Collections.Generic;
using UnityEngine;

public class CrossPromoTrigger : MonoBehaviour
{
	private const string PREFAB_NAME = "CrossPromoPopup";

	private const string URL_TO_OPEN = "http://itunes.apple.com/app/frisbee-forever-2/id528241232";

	private const string ALLOWAFTER_DATATIME_TICKS_KEY = "crprm_date_ticks";

	private const string HAS_SHOWN_KEY = "crprm_has_shown";

	private const int ALLOWAFTER_DELAY_HOURS = 6;

	private const int ALLOWAFTER_DELAY_MINUTES = 0;

	private void OnEnable()
	{
		if (ShouldDisplayCrossPromo())
		{
			UIScreenController.Instance.QueuePopup("CrossPromoPopup");
		}
	}

	private bool ShouldDisplayCrossPromo()
	{
		if (!string.IsNullOrEmpty("http://itunes.apple.com/app/frisbee-forever-2/id528241232"))
		{
			if (PlayerPrefs.GetInt("crprm_has_shown", 0) == 0)
			{
				DateTime dateTime;
				if (PlayerPrefs.HasKey("crprm_date_ticks"))
				{
					string @string = PlayerPrefs.GetString("crprm_date_ticks");
					long result;
					if (!long.TryParse(@string, out result))
					{
						result = DateTime.Now.Ticks;
					}
					dateTime = new DateTime(result);
				}
				else
				{
					dateTime = DateTime.Now + new TimeSpan(6, 0, 0);
					PlayerPrefs.SetString("crprm_date_ticks", dateTime.Ticks.ToString());
				}
				if (DateTime.Now >= dateTime)
				{
					PlayerPrefs.SetInt("crprm_has_shown", 1);
					return true;
				}
				Debug.Log("Cannot show cross promo yet. Allow-after = " + dateTime, this);
			}
			else
			{
				Debug.Log("Cross promo already shown", this);
			}
		}
		return false;
	}

	public static void OkButtonClicked()
	{
		UIScreenController.Instance.ClosePopup();
		Application.OpenURL("http://itunes.apple.com/app/frisbee-forever-2/id528241232");
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Result", "Ok");
	}

	public static void CancelButtonClicked()
	{
		UIScreenController.Instance.ClosePopup();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Result", "Cancel");
	}
}
