using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper.Models
{
    [Serializable]
    public class SeriesModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
    }
}
