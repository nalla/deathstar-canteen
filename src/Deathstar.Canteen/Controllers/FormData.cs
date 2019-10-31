using Microsoft.AspNetCore.Http;

namespace Deathstar.Canteen.Controllers
{
	public class FormData
	{
		public string ApiKey { get; set; }

		public IFormFile Pdf { get; set; }
	}
}
