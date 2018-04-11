using System.Collections.Generic;

namespace Foods.Service.Api
{
    public class RateLimitRule
    {
        public string EndPoint { get; set; }
        public int PeriodInSeconds { get; set; }
        public int Limit { get; set; }
    }

    public class RateLimiting
    {
        public IEnumerable<RateLimitRule> Rules { get; set; }
        public IEnumerable<string> IpWhiteList { get; set; }
    }
}