using System.Collections;
using System.Collections.Generic;

namespace Prime31
{
	public static class MiniJsonExtensions
	{
		public static string toJson(this Dictionary<string, string> obj)
		{
			return MiniJSON.jsonEncode(obj);
		}

		public static ArrayList arrayListFromJson(this string json)
		{
			return MiniJSON.jsonDecode(json) as ArrayList;
		}
	}
}
