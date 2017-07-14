using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterOhana.Models
{
    public class Trend
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string Query { get; set; }
        public string PromotedContent { get; set; }
        public int? TweetVolume { get; set; }
        public List<Models.Tweet> Tweet { get; set; }
    }
}
