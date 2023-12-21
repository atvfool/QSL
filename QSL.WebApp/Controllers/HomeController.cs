using Microsoft.AspNetCore.Mvc;
using QSL.Models;
using QSL.WebApp.Models;
using System.Diagnostics;

namespace QSL.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string API_URL = "https://logbook.qrz.com/api";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(API_URL);

            string content = "KEY=&ACTION=FETCH";


            HttpResponseMessage response = client.PostAsync("", new StringContent(content)).Result;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Load()
        {

          

            return View();
        }

        public IActionResult Generate(Log log)
        {
            var i = 0;
            
            return View();
        }
    }
}