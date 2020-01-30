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

namespace Bollekurs.Pages.Haik
{
    public class IndexModel : PageModel
    {
        private readonly string SessionKey = "_SessionKey";
        private readonly IMemoryCache cache;
        private readonly ApplicationOptions _options;

        private static readonly IDictionary<string, Case> haiker = new Dictionary<string, Case>(){
            {"sykkel", new Case("Sykkel haik","Du skal argumentere for at haiken skal være med sykkel",new[]{"sykkeltur.jpg"})},
            {"kano", new Case("Kano haik","Du skal argumentere for at haiken skal være med kano",new[]{"kanotur.jpg","kanotur2.jpg"})},
            {"ikke", new Case("Ikke haik","Du skal argumentere for å ikke ha haik",new[]{"ikketur.jpg"})},
            {"by", new Case("By haik","Du skal argumentere for en by haik",new[]{"byhaik.jpg"})},
            {"lang", new Case("Lang haik","Du skal argumentere for en lang haik",new []{"Langtur haik.JPG"})},
            {"kort", new Case("Kort haik","Du skal argumentere for en kort haik",new []{"kort haik.JPG"})},
            {"mat", new Case("Mat haik","Du skal argumentere for en mat haik. \n En haik med god mat",new []{"Mathaik.JPG"})},
            {"noysomhet", new Case("Nøysomhets haik","Du skal argumentere for en nøysomhets haik. \n Matbudsjett på 26kr per person.",new []{"noysomhet.jpg"})},
            {"jakke", new Case("Jakke haik","Du skal argumentere for en jakke haik. \n Du får kun ha med det du får plass til i ei jakke",new []{"jakke.jpg"})},
        };
        private static readonly int[] haikcounter = new int[haiker.Count];
        private readonly Random random = new Random();
        public IndexModel(IMemoryCache cache, IOptionsMonitor<ApplicationOptions> options)
        {
            this.cache = cache;
            _options = options.CurrentValue;
        }

        public string Title { get; private set; }
        public string Image { get; private set; }
        public string Message { get; private set; }
        public async Task OnGetAsync()
        {
            var sessionKey = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(sessionKey))
            {
                sessionKey = Guid.NewGuid().ToString();
                HttpContext.Session.SetString(SessionKey, sessionKey);
            }

            var haikKey = await cache.GetOrCreateAsync(sessionKey, e =>
            {
                e.SetSlidingExpiration(_options.CaseTimeout);

                var min = haikcounter.Min();
                var minIndexes = haikcounter.Select((count, index) => (count, index)).Where(i => i.count == min).Select(i => i.index).ToList();
                var haikIndex = minIndexes[random.Next(minIndexes.Count)];
                var haikId = haiker.Keys.ElementAt(haikIndex);
                haikcounter[haikIndex]++;
                e.RegisterPostEvictionCallback((key, value, reason, state) => haikcounter[(int)state]--, haikIndex);
                return Task.FromResult(haikId);
            });
            var haik = haiker[haikKey];

            Title = haik.Title;
            Message = haik.Message;
            Image = haik.Images[random.Next(haik.Images.Count)];
        }
    }
}
