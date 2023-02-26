using System;
using UnityEngine;

public class UITokenScreenHandler : MonoBehaviour
{
	public UISlider trickyProgress;

	public UILabel trickyLabel;

	public UISlider spikeProgress;

	public UILabel spikeLabel;

	public UISlider yutaniProgress;

	public UILabel yutaniLabel;

	public UISlider freshProgress;

	public UILabel freshLabel;

	public UILabel trickyNameLabel;

	public UILabel spikeNameLabel;

	public UILabel yutaniNameLabel;

	public UILabel freshNameLabel;

	private void Awake()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.OnTokenCollected = (Action<CharacterModels.TokenType>)Delegate.Combine(instance.OnTokenCollected, new Action<CharacterModels.TokenType>(OnTokenCollected));
		OnTokenCollected(CharacterModels.TokenType.jake);
		trickyNameLabel.text = CharacterModels.modelData[CharacterModels.ModelType.tricky].getTokenLabel();
		spikeNameLabel.text = CharacterModels.modelData[CharacterModels.ModelType.spike].getTokenLabel();
		yutaniNameLabel.text = CharacterModels.modelData[CharacterModels.ModelType.yutani].getTokenLabel();
		freshNameLabel.text = CharacterModels.modelData[CharacterModels.ModelType.fresh].getTokenLabel();
	}

	private void OnTokenCollected(CharacterModels.TokenType type)
	{
	}
}
