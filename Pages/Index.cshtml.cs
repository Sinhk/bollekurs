using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace Tur.Pages
{
    public class IndexModel : PageModel
    {
        private readonly string SessionKey = "_SessionKey";
        private readonly IMemoryCache cache;

        private static readonly IDictionary<string, Haik> haiker = new Dictionary<string, Haik>(){
            {"sykkel", new Haik("Sykkel haik","Du skal argumentere for at haiken skal være med sykkel",new[]{"sykkeltur.jpg"})},
            {"kano", new Haik("Kano haik","Du skal argumentere for at haiken skal være med kano",new[]{"kanotur.jpg","kanotur2.jpg"})},
            {"ikke", new Haik("Ikke haik","Du skal argumentere for å ikke ha haik",new[]{"ikketur.jpg"})},
            {"by", new Haik("By haik","Du skal argumentere for en by haik",new[]{"byhaik.jpg"})},
            {"lang", new Haik("Lang haik","Du skal argumentere for en lang haik",new []{"Langtur haik.JPG"})},
            {"kort", new Haik("Kort haik","Du skal argumentere for en kort haik",new []{"kort haik.JPG"})},
            {"mat", new Haik("Mat haik","Du skal argumentere for en mat haik. <\\br>En haik med god mat",new []{"Mathaik.jpg"})},
            {"noysomhet", new Haik("Nøysomhets haik","Du skal argumentere for en nøysomhets haik. \nMatbudsjett på 26kr per person.",new []{"noysomhet.jpg"})},
            {"jakke", new Haik("Jakke haik","Du skal argumentere for en jakke haik. \nDu får kun ha med det du får plass til i ei jakke",new []{"jakke.jpg"})},
        };
        private static readonly int[] haikcounter = new int[haiker.Count];
        private readonly Random random = new Random();
        public IndexModel(IMemoryCache cache)
        {
            this.cache = cache;
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
                e.SetSlidingExpiration(Startup.Timeout);

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
