using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using WebApplication1.Models;

using Newtonsoft.Json;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(NotModel p)
        {
            var httpClient = new HttpClient();
            var jsonBlog = JsonConvert.SerializeObject(p);
            StringContent content = new StringContent(jsonBlog, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:7233/notification/send", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["message"] = "Successfully sent";

                return RedirectToAction("Index");
            }

            TempData["message"] = "An error has occured";
            return RedirectToAction("Index"); 

        }

        public IActionResult Mail()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Mail(MailModel p)
		{
			var httpClient = new HttpClient();
			var jsonBlog = JsonConvert.SerializeObject(p);
			StringContent content = new StringContent(jsonBlog, Encoding.UTF8, "application/json");
			var responseMessage = await httpClient.PostAsync("https://localhost:7233/sendmail", content);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["message"] = "Successfully sent";

				return RedirectToAction("Mail");
			}

			TempData["message"] = "An error has occured";
			return RedirectToAction("Mail");

		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}