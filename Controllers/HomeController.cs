using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Tur.Models;

namespace Tur.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string[] text = new []{"kanotur","sykkeltur","boring","byhaik"};
        private readonly Random random = new Random();
        private static int counter = 0;

        public IActionResult Index()
        {
            var haik = HttpContext.Session.GetString("haik");
            if(string.IsNullOrEmpty(haik)){
                haik = text[counter++%4];
                HttpContext.Session.SetString("haik",haik);
            }
            return View(haik);
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
    }
}
