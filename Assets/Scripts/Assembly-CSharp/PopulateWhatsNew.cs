using UnityEngine;

public class PopulateWhatsNew : MonoBehaviour
{
	public UILabel[] labelList;

	private void Start()
	{
		string[] newsForCurrentVersion = WhatsNew.getNewsForCurrentVersion();
		if (newsForCurrentVersion == null || newsForCurrentVersion.Length == 0)
		{
			Debug.LogWarning("Unable to get WhatsNew data. Closing popup", this);
			UIScreenController.Instance.ClosePopup();
			return;
		}
		if (labelList.Length < newsForCurrentVersion.Length)
		{
			Debug.LogWarning("Whats news list for current update contains more than 5 updates. Cannot display all", this);
		}
		for (int i = 0; i < labelList.Length; i++)
		{
			if (i < newsForCurrentVersion.Length)
			{
				labelList[i].text = newsForCurrentVersion[i];
			}
			else
			{
				labelList[i].transform.parent.gameObject.SetActiveRecursively(false);
			}
		}
	}
}
