using System;
using UnityEngine;

public class FrontHighscoreLabel : MonoBehaviour
{
	private UILabel _highScoreLabel;

	private void OnEnable()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onHighScoreChanged = (Action)Delegate.Combine(instance.onHighScoreChanged, new Action(UpdateHighScore));
		UpdateHighScore();
	}

	private void OnDisable()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onHighScoreChanged = (Action)Delegate.Remove(instance.onHighScoreChanged, new Action(UpdateHighScore));
	}

	private void UpdateHighScore()
	{
		if (_highScoreLabel == null)
		{
			_highScoreLabel = GetComponent<UILabel>();
		}
		_highScoreLabel.text = string.Format("{0:0000}", PlayerInfo.Instance.highestScore);
		_highScoreLabel.panel.Refresh();
	}
}
