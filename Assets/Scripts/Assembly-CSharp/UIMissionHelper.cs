using UnityEngine;

public class UIMissionHelper : MonoBehaviour
{
	[SerializeField]
	private UILabel missionsLabel;

	[SerializeField]
	private UICheckbox MissionCheckBox1;

	[SerializeField]
	private UICheckbox MissionCheckBox2;

	[SerializeField]
	private UICheckbox MissionCheckBox3;

	[SerializeField]
	private UILabel MissionLabel1;

	[SerializeField]
	private UILabel MissionLabel2;

	[SerializeField]
	private UILabel MissionLabel3;

	[SerializeField]
	private UILabel MissionNumber1;

	[SerializeField]
	private UILabel MissionNumber2;

	[SerializeField]
	private UILabel MissionNumber3;

	[SerializeField]
	private UISlider missionProgressSlider;

	[SerializeField]
	private UILabel progressLabel;

	[SerializeField]
	private UILabel missionGoalLabel;

	private int[] _cachedMissionProgressions = new int[3];

	private int _cachedMissionSet;

	private bool hasCached;

	private bool hasDestroyedGameObjects;

	private MissionInfo[] _currentMissions;

	private bool timeDeathMission;

	private void Update()
	{
		if (hasDestroyedGameObjects)
		{
			return;
		}
		if (!Missions.Instance.HasMoreMissions())
		{
			MissionLabel2.text = "Max multiplier reached";
			missionsLabel.pivot = UIWidget.Pivot.Bottom;
			missionsLabel.transform.localPosition = new Vector3(0f, missionsLabel.transform.localPosition.y, 0f);
			missionsLabel.MakePositionPerfect();
			Object.Destroy(missionProgressSlider.gameObject);
			Object.Destroy(MissionCheckBox1.gameObject);
			Object.Destroy(MissionCheckBox3.gameObject);
			foreach (Transform item in MissionCheckBox2.transform)
			{
				if (item.gameObject != MissionLabel2.gameObject)
				{
					Object.Destroy(item.gameObject);
				}
			}
			Object.Destroy(MissionCheckBox2);
			hasDestroyedGameObjects = true;
			return;
		}
		_currentMissions = Missions.Instance.GetMissionInfo();
		if (hasCached)
		{
			bool flag = true;
			for (int i = 0; i < 3; i++)
			{
				if (_cachedMissionProgressions[i] != _currentMissions[i].progress)
				{
					flag = false;
				}
			}
			if (_cachedMissionSet != Missions.Instance.currentMissionSet)
			{
				flag = false;
			}
			if (flag)
			{
				return;
			}
		}
		MissionLabel2.text = string.Format(_currentMissions[1].template.description, _currentMissions[1].mission.goal, _currentMissions[1].mission.goal - _currentMissions[1].progress);
		MissionLabel3.text = string.Format(_currentMissions[2].template.description, _currentMissions[2].mission.goal, _currentMissions[2].mission.goal - _currentMissions[2].progress);
		MissionCheckBox1.isChecked = _currentMissions[0].complete;
		MissionCheckBox2.isChecked = _currentMissions[1].complete;
		MissionCheckBox3.isChecked = _currentMissions[2].complete;
		hasCached = true;
		LabelAndNumberUpdate(0, MissionLabel1, MissionNumber1);
		LabelAndNumberUpdate(1, MissionLabel2, MissionNumber2);
		LabelAndNumberUpdate(2, MissionLabel3, MissionNumber3);
		_cachedMissionProgressions[0] = _currentMissions[0].progress;
		_cachedMissionProgressions[1] = _currentMissions[1].progress;
		_cachedMissionProgressions[2] = _currentMissions[2].progress;
		_cachedMissionSet = Missions.Instance.currentMissionSet;
		UpdateMissionProgress();
	}

	private void LabelAndNumberUpdate(int missionArrayNr, UILabel sendMissionLabel, UILabel sendMissionNumber)
	{
		if (_currentMissions[missionArrayNr].complete)
		{
			string format = ((_currentMissions[missionArrayNr].mission.goal != 1) ? _currentMissions[missionArrayNr].template.ultraShortDescription : _currentMissions[missionArrayNr].template.ultraShortDescriptionSingle);
			sendMissionLabel.text = string.Format(format, _currentMissions[missionArrayNr].mission.goal);
			sendMissionNumber.text = string.Empty;
			return;
		}
		string format2 = ((_currentMissions[missionArrayNr].mission.goal != 1) ? _currentMissions[missionArrayNr].template.description : _currentMissions[missionArrayNr].template.descriptionSingle);
		if (_currentMissions[missionArrayNr].mission.type == Missions.MissionType.TimeDeath)
		{
			if (Game.Instance.isPaused)
			{
				sendMissionLabel.text = string.Format(format2, _currentMissions[missionArrayNr].mission.goal, (int)Game.Instance.GetDuration());
				hasCached = false;
			}
			else
			{
				sendMissionLabel.text = string.Format(format2, _currentMissions[missionArrayNr].mission.goal, 0);
			}
		}
		else
		{
			sendMissionLabel.text = string.Format(format2, _currentMissions[missionArrayNr].mission.goal, _currentMissions[missionArrayNr].mission.goal - _currentMissions[missionArrayNr].progress);
		}
		sendMissionNumber.text = missionArrayNr + 1 + string.Empty;
	}

	private void UpdateMissionProgress()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (_currentMissions[i].complete)
			{
				num++;
			}
		}
		missionProgressSlider.sliderValue = (float)num / 3f;
		progressLabel.text = num + "/" + 3;
		if (PlayerInfo.Instance.GetCurrentRank() == 0)
		{
			if (PlayerInfo.Instance.rawMultiplier == 30)
			{
				missionGoalLabel.text = "I";
			}
			else
			{
				missionGoalLabel.text = "x" + (PlayerInfo.Instance.rawMultiplier + 1);
			}
			return;
		}
		switch (PlayerInfo.Instance.GetCurrentRank() + 1)
		{
		case 1:
			missionGoalLabel.text = "I";
			break;
		case 2:
			missionGoalLabel.text = "II";
			break;
		case 3:
			missionGoalLabel.text = "III";
			break;
		case 4:
			missionGoalLabel.text = "IV";
			break;
		case 5:
			missionGoalLabel.text = "V";
			break;
		case 6:
			missionGoalLabel.text = "VI";
			break;
		case 7:
			missionGoalLabel.text = "VII";
			break;
		case 8:
			missionGoalLabel.text = "VIII";
			break;
		case 9:
			missionGoalLabel.text = "IX";
			break;
		default:
			missionGoalLabel.text = "X";
			break;
		}
	}
}
