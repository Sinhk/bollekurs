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

        private static readonly IDictionary<string, Haik> haiker = new Dictionary<string, Haik>(){
            {"sykkel", new Haik("Sykkel haik","Du skal argumenter for at haiken skal være med sykkel",new[]{"sykkeltur.jpg"})},
            {"kano", new Haik("Kano haik","Du skal argumenter for at haiken skal være med kano",new[]{"kanotur.jpg","kanotur2.jpg"})},
            {"ikke", new Haik("Ikke haik","Du skal argumenter for å ikke ha haik",new[]{"ikketur.jpg"})},
            {"by", new Haik("By haik","Du skal argumenter for en by haik",new[]{"byhaik.jpg"})},
        };
        private readonly IMemoryCache cache;
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
