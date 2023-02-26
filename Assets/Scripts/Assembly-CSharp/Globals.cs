public class Globals
{
	public const bool DEBUG = false;

	public const bool DEBUG_SOCIAL_MANAGER_SERVER = false;

	public const bool DEBUG_FREE_PURCHASES = false;

	public const bool DEBUG_ALL_CHARS = false;

	public const bool DEBUG_ALWAYS_ONLINE = false;

	public const bool DEBUG_FREE_INAPP_PURCHASE = false;

	public const int MAX_MULTIPLIER = 30;

	public const int MAX_RANK = 1;

	public const int BONUS_FACEBOOK = 5000;

	public const int BONUS_GAMECENTER = 250;

	public const int MIN_FRIEND_SCORE_REQUEST_INTERVAL = 15;

	public static Friend[] GetDebugFriends(int numberOfFriends = 10)
	{
		return new Friend[numberOfFriends];
	}
}
