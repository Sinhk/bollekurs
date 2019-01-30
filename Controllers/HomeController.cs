using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            Console.WriteLine(HttpContext.Connection.RemoteIpAddress);
            //Console.WriteLine(HttpContext.Connection.Remote);
            return View(text[counter++%4]);
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
