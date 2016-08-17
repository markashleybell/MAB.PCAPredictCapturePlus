using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAB.PCAPredict
{
    public class PCAPredictFindResult
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Highlight { get; set; }
        public int Cursor { get; set; }
        public string Description { get; set; }
        public string Next { get; set; }
    }
}
