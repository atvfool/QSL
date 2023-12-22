using Microsoft.AspNetCore.Mvc;
using QSL.Models;
using QSL.WebApp.Models;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using ADIFLib;

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

            List<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>();
            content.Add(new KeyValuePair<string, string>("KEY", ""));
            content.Add(new KeyValuePair<string, string>("ACTION", "FETCH"));
            
            HttpResponseMessage response = client.PostAsync(string.Empty, new FormUrlEncodedContent(content)).Result;
            string result = HttpUtility.HtmlDecode(response.Content.ReadAsStringAsync().Result);
            NameValueCollection nvc = new NameValueCollection();
            nvc = HttpUtility.ParseQueryString(result);
            QRZResult qrz = new QRZResult() { 
                RESULT = nvc["RESULT"],
                COUNT = nvc["COUNT"],
                ADIF = nvc["ADIF"],
                LOGIDS = nvc["LOGIDS"]

            };
            string test = "";
            ADIF adif = new ADIF();
            adif.ReadFromString(qrz.ADIF);
            foreach(ADIFQSO col in adif.TheQSOs)
            {
                test += col.Where(x => x.Name == "call").First().Data + "\r\n";
            }

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