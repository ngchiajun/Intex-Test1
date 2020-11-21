using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intex_Test1.Models
{
    public class Meme
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int page { get; set; }
        public int requestCount { get; set; }
    }
}