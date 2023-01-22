using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Bollekurs.Models;
using Microsoft.Extensions.Options;

namespace Bollekurs.Pages.Del2
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationOptions _options;
        private readonly CaseManager _manager;

        public IndexModel(CaseManager manager, IOptionsMonitor<ApplicationOptions> options)
        {
            _options = options.CurrentValue;
            _manager = manager;
        }

        public string? Title { get; private set; }
        public string? Image { get; private set; }
        public string? Message { get; private set; }

        public async Task OnGetAsync(int? id, bool? ny)
        {
            Case @case;
            if (id != null)
            {
                @case = _manager.GetCase(id.Value);
            }
            else
            {
                @case = await _manager.GetRandomCase(HttpContext.Session, ny.GetValueOrDefault());
            }
            Title = @case.Title;
            Message = @case.Message;
            Image = @case.Images[Random.Shared.Next(@case.Images.Count)];
        }
    }
}