using System.Collections.Generic;

public class Trophies
{
	public enum Trophy
	{
		diamond = 0,
		goldbar = 1,
		goldChainClock = 2,
		goldChainDollar = 3,
		goldSkull = 4,
		headphones = 5,
		lpBlack = 6,
		tapeBlack = 7
	}

	public static readonly string trophyAchievementID = "AllTrophiesCollected";

	public static readonly Dictionary<Trophy, TrophyData> trophyData = new Dictionary<Trophy, TrophyData>
	{
		{
			Trophy.diamond,
			new TrophyData
			{
				name = "Gemstone",
				description = "It belongs in a museum!",
				spriteUnlocked = "trophy_diamond"
			}
		},
		{
			Trophy.goldbar,
			new TrophyData
			{
				name = "Gold Bar",
				description = "Directly from Fort Knox.",
				spriteUnlocked = "trophy_gold"
			}
		},
		{
			Trophy.goldChainClock,
			new TrophyData
			{
				name = "Classy Clock",
				description = "Show your style like Flavor Flav!",
				spriteUnlocked = "trophy_goldChainClock"
			}
		},
		{
			Trophy.goldChainDollar,
			new TrophyData
			{
				name = "Dollar Chain",
				description = "Bring some bling to the party.",
				spriteUnlocked = "trophy_goldChainDollar"
			}
		},
		{
			Trophy.goldSkull,
			new TrophyData
			{
				name = "The Paperweight",
				description = "A little Gold Sapiens.",
				spriteUnlocked = "trophy_goldSkull"
			}
		},
		{
			Trophy.headphones,
			new TrophyData
			{
				name = "Headphones",
				description = "Groove is in the heart.",
				spriteUnlocked = "trophy_headphones"
			}
		},
		{
			Trophy.lpBlack,
			new TrophyData
			{
				name = "Platinum Record",
				description = "Bring the beat back!",
				spriteUnlocked = "trophy_lpBlack"
			}
		},
		{
			Trophy.tapeBlack,
			new TrophyData
			{
				name = "Cassette Tape",
				description = "Retro musicology.",
				spriteUnlocked = "trophy_tapeBlack"
			}
		}
	};
}
