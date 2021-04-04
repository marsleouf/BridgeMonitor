using BridgeMonitor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BridgeMonitor.Controllers
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

        public IActionResult Privacy()
        {
            var boats = GetBoatFromApi();
                       boats.Sort((x, y) => DateTime.Compare(x.ClosingDate, y.ClosingDate));
            DateTime now = DateTime.Now;
            foreach(var boat in boats)
            {
                if (boat.ClosingDate > now)
                {
                    return View(boat);
                } else
                {
                    continue;
                }
            }
            return View();
        }


        public IActionResult Fermetures()
        {
            var boats = GetBoatFromApi();
            var boats_before = new List<Boat>();
            var boats_after = new List<Boat>();
            var boats_result = new BoatModel()
            {
                BoatsBefore = boats_before,
                BoatsAfter = boats_after
            };
            boats.Sort((x, y) => DateTime.Compare(x.ClosingDate, y.ClosingDate));
            DateTime now = DateTime.Now;
            foreach (var boat in boats)
            {
                if (boat.ClosingDate > now)
                {
                    boats_after.Add(boat);
                }
                else
                {
                    boats_before.Add(boat);
                }
            }

            return View(boats_result);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private static List<Boat> GetBoatFromApi()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("https://api.alexandredubois.com/pont-chaban/api.php");
                var stringResult = response.Result.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Boat>>(stringResult.Result);
                return result;
            }
        }
    }
}
