using System.Collections.Generic;

public class CharacterModels
{
	public enum ModelType
	{
		slick = 0,
		lucy = 1,
		ninja = 2,
		frank = 3,
		king = 4,
		frizzy = 5,
		tricky = 6,
		fresh = 7,
		spike = 8,
		yutani = 9
	}

	public enum UnlockType
	{
		free = 0,
		tokens = 1,
		coins = 2
	}

	public enum TokenType
	{
		jake = 0,
		trickysHat = 1,
		freshsStereo = 2,
		spikesGuitar = 3,
		YutanisUFO = 4,
		frank = 5,
		frizzy = 6,
		king = 7,
		lucy = 8,
		ninja = 9
	}

	public class Token
	{
		public string name = "not_set_yet";

		public string sprite2dName = "not_set_yet";
	}

	public class Model
	{
		public string modelName = "not_set_yet";

		public TokenType[] tokenType = new TokenType[0];

		public int Price = -1;

		public UnlockType unlockType;

		public Token[] getToken()
		{
			Token[] array = new Token[tokenType.Length];
			for (int i = 0; i < tokenType.Length; i++)
			{
				array[i] = tokenInfo[tokenType[i]];
			}
			return array;
		}

		public string getTokenLabel()
		{
			if (tokenType.Length == 1)
			{
				return tokenInfo[tokenType[0]].name;
			}
			return "not_set_yet";
		}
	}

	public static readonly Dictionary<ModelType, Model> modelData = new Dictionary<ModelType, Model>
	{
		{
			ModelType.slick,
			new Model
			{
				modelName = "Jake",
				tokenType = new TokenType[1]
			}
		},
		{
			ModelType.tricky,
			new Model
			{
				modelName = "Tricky",
				unlockType = UnlockType.tokens,
				tokenType = new TokenType[1] { TokenType.trickysHat },
				Price = 3
			}
		},
		{
			ModelType.fresh,
			new Model
			{
				modelName = "Fresh",
				tokenType = new TokenType[1] { TokenType.freshsStereo },
				unlockType = UnlockType.tokens,
				Price = 50
			}
		},
		{
			ModelType.spike,
			new Model
			{
				modelName = "Spike",
				unlockType = UnlockType.tokens,
				tokenType = new TokenType[1] { TokenType.spikesGuitar },
				Price = 200
			}
		},
		{
			ModelType.yutani,
			new Model
			{
				modelName = "Yutani",
				unlockType = UnlockType.tokens,
				tokenType = new TokenType[1] { TokenType.YutanisUFO },
				Price = 500
			}
		},
		{
			ModelType.lucy,
			new Model
			{
				modelName = "Lucy",
				unlockType = UnlockType.coins,
				tokenType = new TokenType[1] { TokenType.lucy },
				Price = 7000
			}
		},
		{
			ModelType.ninja,
			new Model
			{
				modelName = "Ninja",
				unlockType = UnlockType.coins,
				tokenType = new TokenType[1] { TokenType.ninja },
				Price = 20000
			}
		},
		{
			ModelType.frank,
			new Model
			{
				modelName = "Frank",
				unlockType = UnlockType.coins,
				tokenType = new TokenType[1] { TokenType.frank },
				Price = 40000
			}
		},
		{
			ModelType.king,
			new Model
			{
				modelName = "King",
				unlockType = UnlockType.coins,
				tokenType = new TokenType[1] { TokenType.king },
				Price = 80000
			}
		},
		{
			ModelType.frizzy,
			new Model
			{
				modelName = "Frizzy",
				unlockType = UnlockType.coins,
				tokenType = new TokenType[1] { TokenType.frizzy },
				Price = 150000
			}
		}
	};

	public static readonly Dictionary<TokenType, Token> tokenInfo = new Dictionary<TokenType, Token>
	{
		{
			TokenType.jake,
			new Token
			{
				name = string.Empty
			}
		},
		{
			TokenType.trickysHat,
			new Token
			{
				name = "Tricky's Hat",
				sprite2dName = "trophy_tricky_token"
			}
		},
		{
			TokenType.freshsStereo,
			new Token
			{
				name = "Fresh's Stereo",
				sprite2dName = "trophy_fresh_token"
			}
		},
		{
			TokenType.spikesGuitar,
			new Token
			{
				name = "Spike's Guitar",
				sprite2dName = "trophy_spike_token"
			}
		},
		{
			TokenType.YutanisUFO,
			new Token
			{
				name = "Yutani's UFO",
				sprite2dName = "trophy_yutani_token"
			}
		},
		{
			TokenType.frank,
			new Token
			{
				name = "Frank"
			}
		},
		{
			TokenType.frizzy,
			new Token
			{
				name = "Frizzy"
			}
		},
		{
			TokenType.king,
			new Token
			{
				name = "King"
			}
		},
		{
			TokenType.lucy,
			new Token
			{
				name = "Lucy"
			}
		},
		{
			TokenType.ninja,
			new Token
			{
				name = "Ninja"
			}
		}
	};
}
