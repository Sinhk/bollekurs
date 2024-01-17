using System.Collections.Generic;
using System.Linq;

namespace Bollekurs.Models
{
    internal sealed class Case
    {
        public string Title {get;}
        public string Message {get;}
        public List<string> Images {get;}

        public Case(string title, string message, IEnumerable<string> images)
        {
            Title = title;
            Message = message;
            Images = images.ToList();
        }
    }
}