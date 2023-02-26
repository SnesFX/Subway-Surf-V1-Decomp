using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Friend
{
	public enum Medal
	{
		none = 0,
		bronze = 1,
		silver = 2,
		gold = 3
	}

	public class Status
	{
		public DateTime lastPokeTime = DateTime.MinValue;

		public int gamesCashedIn;
	}

	public static readonly string[] romanRanks = new string[21]
	{
		string.Empty,
		"I",
		"II",
		"III",
		"IV",
		"V",
		"VI",
		"VII",
		"VIII",
		"IX",
		"X",
		"XI",
		"XII",
		"XIII",
		"XIV",
		"XV",
		"XVI",
		"XVII",
		"XVIII",
		"XIX",
		"XX"
	};

	public static readonly Dictionary<Medal, string> medalSpriteString = new Dictionary<Medal, string>
	{
		{
			Medal.none,
			string.Empty
		},
		{
			Medal.bronze,
			"icon_medal_bronze"
		},
		{
			Medal.silver,
			"icon_medal_silver"
		},
		{
			Medal.gold,
			"icon_medal_gold"
		}
	};

	public int userid;

	public int score;

	public int meters;

	public int games;

	public int rank;

	public IUserProfile gcProfile;

	public FacebookProfile fbProfile;

	public Status status;

	public string name
	{
		get
		{
			if (fbProfile != null)
			{
				return fbProfile.name;
			}
			if (gcProfile != null)
			{
				return gcProfile.userName;
			}
			return null;
		}
	}

	public Texture2D image
	{
		get
		{
			if (fbProfile != null)
			{
				return fbProfile.image;
			}
			if (gcProfile != null)
			{
				return gcProfile.image;
			}
			return null;
		}
	}

	public int relation
	{
		get
		{
			int num = 0;
			if (fbProfile != null)
			{
				num |= 1;
			}
			if (gcProfile != null)
			{
				num |= 2;
			}
			return num;
		}
	}

	public string id
	{
		get
		{
			if (fbProfile != null)
			{
				return fbProfile.id;
			}
			if (gcProfile != null)
			{
				return gcProfile.id;
			}
			return null;
		}
	}

	public int gamesToCashIn
	{
		get
		{
			return games - status.gamesCashedIn;
		}
	}
}
