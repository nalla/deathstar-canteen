using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Deathstar.Canteen.Persistence;

namespace Deathstar.Canteen.Services
{
	public class MenuParser : IMenuParser
	{
		public IEnumerable<Menu> ParseText(string text)
		{
			string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
			var dictionary = new Dictionary<DateTime, Menu>();
			DateTime? currentDate = null;

			foreach (string line in lines)
			{
				string trimmed = line.Trim();
				if (DateTime.TryParseExact(trimmed, "dddd, d. MMMM yyyy", CultureInfo.GetCultureInfo("de-de"), DateTimeStyles.AssumeLocal, out DateTime date))
				{
					currentDate = date;

					continue;
				}

				if (trimmed.Length > 0 && currentDate.HasValue)
				{
					if (dictionary.TryGetValue(currentDate.Value, out Menu entry))
					{
						entry.Meals = entry.Meals.Append(trimmed).ToArray();
					}
					else
					{
						entry = new Menu { Date = currentDate.Value.ToString("yyyyMMdd"), Meals = new[] { trimmed } };
						dictionary[currentDate.Value] = entry;
					}
				}
			}

			return dictionary.Values;
		}
	}
}
