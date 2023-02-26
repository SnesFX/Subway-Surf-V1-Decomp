using UnityEngine;

public class MysteryBoxRewardLabelTemplate : MonoBehaviour
{
	public UILabel label;

	public float Alpha
	{
		get
		{
			return label.alpha;
		}
		set
		{
			label.alpha = Mathf.Clamp01(value);
		}
	}

	public void SetupPowerup(PowerupType powerup, int amount)
	{
		Alpha = 0f;
		label.text = _GetPowerupLabel(powerup, amount);
	}

	public void SetupCoins(int amount)
	{
		Alpha = 0f;
		label.text = _GetCoinsLabel(amount);
	}

	public void UpdateCoins(int amount)
	{
		label.text = _GetCoinsLabel(amount);
	}

	public void SetupToken(CharacterModels.TokenType tokenType)
	{
		Alpha = 0f;
		label.text = _GetTokenLabel(tokenType);
	}

	public void SetupTrophy(Trophies.Trophy trophy)
	{
		Alpha = 0f;
		label.text = _GetTrophyLabel(trophy);
	}

	private string _GetPowerupLabel(PowerupType type, int amount)
	{
		string empty = string.Empty;
		empty = empty + amount + "x ";
		return empty + Upgrades.upgrades[type].name;
	}

	private string _GetTokenLabel(CharacterModels.TokenType tokenType)
	{
		string empty = string.Empty;
		return empty + "1x " + CharacterModels.tokenInfo[tokenType].name;
	}

	private string _GetTrophyLabel(Trophies.Trophy trophy)
	{
		return Trophies.trophyData[trophy].name;
	}

	private string _GetCoinsLabel(int amount)
	{
		return amount + " Coins";
	}
}
