using System;
using UnityEngine;

public class TrophyHelper : MonoBehaviour
{
	private Trophies.Trophy _trophy;

	public UILabel title;

	public UILabel description;

	public UISprite image;

	private void Awake()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.OnTrophyUnlocked = (Action<Trophies.Trophy>)Delegate.Combine(instance.OnTrophyUnlocked, new Action<Trophies.Trophy>(onTrophyUnlocked));
	}

	private void onDestroy()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.OnTrophyUnlocked = (Action<Trophies.Trophy>)Delegate.Remove(instance.OnTrophyUnlocked, new Action<Trophies.Trophy>(onTrophyUnlocked));
	}

	private void Start()
	{
		updateDisplay();
	}

	private void onTrophyUnlocked(Trophies.Trophy trophy)
	{
		updateDisplay();
	}

	public void setTrophy(Trophies.Trophy trophy)
	{
		_trophy = trophy;
		updateDisplay();
	}

	public void updateDisplay()
	{
		TrophyData trophyData = Trophies.trophyData[_trophy];
		bool flag = PlayerInfo.Instance.isTrophyUnlocked(_trophy);
		title.text = trophyData.name;
		description.text = trophyData.description;
		if ((bool)image)
		{
			if (PlayerInfo.Instance.isTrophyUnlocked(_trophy))
			{
				image.spriteName = trophyData.spriteUnlocked;
				image.color = Color.white;
			}
			else
			{
				image.spriteName = trophyData.spriteLocked;
				image.color = new Color(0.95686275f, 0.38039216f, 0f);
			}
			image.MakePixelPerfect();
		}
	}
}
