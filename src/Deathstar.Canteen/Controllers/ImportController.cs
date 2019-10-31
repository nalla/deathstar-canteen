using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Services;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Deathstar.Canteen.Controllers
{
	[ApiController]
	[Route("imports")]
	public class ImportController : Controller
	{
		private readonly IConfiguration configuration;
		private readonly ILogger logger;
		private readonly IMenuParser menuParser;
		private readonly IMenuRepository menuRepository;

		public ImportController(
			ILogger<ImportController> logger,
			IMenuParser menuParser,
			IMenuRepository menuRepository,
			IConfiguration configuration)
		{
			this.logger = logger;
			this.menuParser = menuParser;
			this.menuRepository = menuRepository;
			this.configuration = configuration;
		}

		[HttpGet]
		public IActionResult Get() => Ok(
			"Please POST the pdf file with content type \"multipart/form-data\" to this endpoint. Use \"pdf\" as the key for the file and \"apiKey\" for the api key inside the form data.");

		[HttpPost]
		[Consumes("multipart/form-data")]
		[Produces("application/json")]
		public async Task<IActionResult> PostAsync([FromForm] FormData formData, CancellationToken cancellationToken)
		{
			var importResult = new ImportResult();

			if (formData == null)
			{
				importResult.Error = "Form data is missing.";

				return BadRequest(importResult);
			}

			if (formData.ApiKey?.Equals(configuration["Import:ApiKey"]) != true)
			{
				importResult.Error = "Api Key is missing or invalid.";

				return BadRequest(importResult);
			}

			try
			{
				using (Stream stream = formData.Pdf.OpenReadStream())
				{
					var document = new PdfDocument(new PdfReader(stream));
					var text = new StringBuilder();

					for (var i = 1; i <= document.GetNumberOfPages(); i++)
					{
						var strategy = new LocationTextExtractionStrategy();
						var parser = new PdfCanvasProcessor(strategy);
						parser.ProcessPageContent(document.GetPage(i));
						text.Append(strategy.GetResultantText());
					}

					importResult.Pdf2Text = text.ToString();
				}
			}
			catch (Exception e)
			{
				logger.LogError(e, e.Message);
				importResult.Error = e.Message;

				return BadRequest(importResult);
			}

			try
			{
				IEnumerable<Menu> menus = menuParser.ParseText(importResult.Pdf2Text).ToArray();

				foreach (Menu menu in menus)
				{
					await menuRepository.ReplaceOrInsertAsync(menu, cancellationToken);
				}

				importResult.ImportedMenus = menus.Count();

				return Ok(importResult);
			}
			catch (Exception e)
			{
				logger.LogError(e, e.Message);
				importResult.Error = e.Message;

				return StatusCode(500, importResult);
			}
		}
	}
}
