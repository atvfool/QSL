using Microsoft.AspNetCore.Mvc;
using QSL.Models;
using QSL.WebApp.Models;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using ADIFLib;
using QSL.Renderer;
using System.IO.Compression;

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

        public FileResult Index()
        {
            
            return Generate();
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

        public FileResult Generate()
        {
            var i = 0;
            var QSOs = LoadLogs();

            MemoryStream memStream = new MemoryStream();
            using (var archive = new ZipArchive(memStream, ZipArchiveMode.Create, true)) {
                foreach (var item in QSOs)
                {
                    Renderer.Renderer renderer = new Renderer.Renderer(item);
                    var newImg = renderer.Generate();

                    var newEntry = archive.CreateEntry(item.Where(x => x.Name == "call").First().Data + ".png");

                    using (var entryStream = newEntry.Open())
                    using (var sw = new StreamWriter(entryStream))
                    {
                        sw.Write(newImg);
                    }
                }
            }
            return File(memStream.ToArray(), "application/octet-stream");
        }

        private ADIFQSOCollection LoadLogs()
        {
            List<Log> result= new List<Log>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(API_URL);

            List<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>();
            content.Add(new KeyValuePair<string, string>("KEY", ""));
            content.Add(new KeyValuePair<string, string>("ACTION", "FETCH"));

            HttpResponseMessage response = client.PostAsync(string.Empty, new FormUrlEncodedContent(content)).Result;
            string decode = HttpUtility.HtmlDecode(response.Content.ReadAsStringAsync().Result);
            NameValueCollection nvc = new NameValueCollection();
            nvc = HttpUtility.ParseQueryString(decode);
            QRZResult qrz = new QRZResult()
            {
                RESULT = nvc["RESULT"],
                COUNT = nvc["COUNT"],
                ADIF = nvc["ADIF"],
                LOGIDS = nvc["LOGIDS"]

            };
            string test = "";
            ADIF adif = new ADIF();
            adif.ReadFromString(qrz.ADIF);
            foreach (ADIFQSO col in adif.TheQSOs)
            {
                test += col.Where(x => x.Name == "call").First().Data + "\r\n";
            }

            return adif.TheQSOs;
        }
    }
}