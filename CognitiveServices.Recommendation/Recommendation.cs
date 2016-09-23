using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveServices.Recommendation
{
    public class Recommendation
    {
        public IEnumerable<CatalogItem> Items { get; set; }

        public decimal Rating { get; set; }

        public IEnumerable<string> Reasoning { get; set; }
    }
}
