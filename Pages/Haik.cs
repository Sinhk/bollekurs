using System.Collections.Generic;
using System.Linq;

namespace Tur.Pages
{
    internal class Haik
    {
        public string Title {get;}
        public string Message {get;}
        public List<string> Images {get;}

        public Haik(string title, string message, IEnumerable<string> images)
        {
            Title = title;
            Message = message;
            Images = images.ToList();
        }
    }
}