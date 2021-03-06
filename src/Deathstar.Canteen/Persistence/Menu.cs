using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Deathstar.Canteen.Persistence
{
	public class Menu
	{
		public string Date { get; set; }

		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		[BsonIgnoreIfDefault]
		public ObjectId Id { get; set; }

		public string[] Meals { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();

			for (var i = 1; i <= Meals.Length; i++)
			{
				sb.AppendLine($"{i}. {Meals[i - 1]}");
			}

			return sb.ToString().Trim();
		}
	}
}
