using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

using Bollekurs.Models;
using Microsoft.Extensions.Options;

namespace Bollekurs.Pages.Mat
{
    public class IndexModel : PageModel
    {
        private const string SessionKey = "_SessionKey";
        private readonly IMemoryCache _cache;
        private readonly ApplicationOptions _options;

        private static readonly Case[] Mat = new Case[]
        {
            new Case("Ferdigmat","Du skal argumentere for at det bare skal være ferdigmat.",new[]{"ferdigmat.png"}),
            new Case("REAL Turmat","Du skal argumentere for at det bare skal være REAL Turmat.",new[]{"realturmat.png"}),
            new Case("Økologisk mat","Du skal argumentere for at all maten skal være økologisk.",new[]{"okologisk.png"}),
            new Case("Vegetarmat","Du skal argumentere for at all maten skal være vegetarisk.",new[]{"vegetar.png"}),
            new Case("Kortreis mat","Du skal argumentere for at maten skal være mest mulig kortreist.",new[]{"kortreist.png"}),
            new Case("Hjemmelaget","Du skal argumentere for at all maten skal lages fra bunnen av.",new[]{"hjemmelaget.png"}),
            new Case("Billig mat","Du skal argumentere for at all maten skal være billigst mulig",new[]{"billig.png"}),
            new Case("Bare brød","NEI, NEI, NEI, gi meg brød med smør på! Og ikke noe mer!",new[]{"brod.png"}),
            new Case("Restaurant","Du skal argumentere for at vi skal spise på restaurant hver dag",new[]{"restaurant.png"}),
        };
        private static readonly int[] Matcounter = new int[Mat.Length];
        private readonly Random _random = new Random();
        public IndexModel(IMemoryCache cache, IOptionsMonitor<ApplicationOptions> options)
        {
            _cache = cache;
            _options = options.CurrentValue;
        }

        public string Title { get; private set; }
        public string Image { get; private set; }
        public string Message { get; private set; }
        public async Task OnGetAsync()
        {
            await HttpContext.Session.LoadAsync();
            var sessionKey = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(sessionKey))
            {
                sessionKey = Guid.NewGuid().ToString();
                HttpContext.Session.SetString(SessionKey, sessionKey);
            }

            var caseIndex = _cache.GetOrCreate(sessionKey, e =>
            {
                e.SetSlidingExpiration(_options.CaseTimeout);
                var index = RandomCase();
                e.RegisterPostEvictionCallback((key, value, reason, state) => Matcounter[(int) state]--, index);
                return index;
            });
            var aCase = Mat[caseIndex];
            
            Title = aCase.Title;
            Message = aCase.Message;
            Image = "mat/" +aCase.Images[_random.Next(aCase.Images.Count)];
        }

        private int RandomCase()
        {
            var min = Matcounter.Min();
            var minIndexes = Matcounter.Select((count, index) => (count, index)).Where(i => i.count == min).Select(i => i.index).ToList();
            var haikIndex = minIndexes[_random.Next(minIndexes.Count)];
            Matcounter[haikIndex]++;
            return haikIndex;
        }
    }
}
