using System;

namespace Bollekurs
{
    public class ApplicationOptions
    {
        public TimeSpan CaseTimeout { get; set; } = TimeSpan.FromMinutes(10);
    }
}