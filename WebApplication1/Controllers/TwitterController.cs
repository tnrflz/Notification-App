using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApplication1.Models.TwitterModels;

namespace WebApplication1.Controllers
{
    public class TwitterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        /*
        [HttpPost]
        public async Task  <IActionResult> TwitterAccount(TwitterAccountModel model)
        {
            var httpClient = new HttpClient();
            var jsonBlog = JsonConvert.SerializeObject(p);
            StringContent content = new StringContent(jsonBlog, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:7233/sendmail", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["message"] = "Successfully sent";

                return RedirectToAction("Twitter");
            }

            TempData["message"] = "An error has occured";
            return RedirectToAction("Index");
            
        }
        */
        public IActionResult Twitter()
        {
            return View();
        }




    }
}
