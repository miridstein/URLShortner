using System;
using System.Collections.Generic;
using System.Text;

namespace URLShortner.Data
{
    public class UrlLink
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string HashedUrl { get; set; }
        public int? UserId { get; set; }
        public int Views { get; set; }
    }
}
