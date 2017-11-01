using System;
using MongoDB.Bson;

namespace Deathstar.Canteen.Persistence
{
	public class Menu
	{
		public string Date { get; set; }

		public ObjectId Id { get; set; }

		public string[] Meals { get; set; }

		public override string ToString()
		{
			string result = string.Empty;

			for( int i = 1; i <= Meals.Length; i++ )
				result += $"{i}. {Meals[i - 1]}{Environment.NewLine}";

			return result.Trim();
		}
	}
}
