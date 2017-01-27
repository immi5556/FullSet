using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants.Model
{
    public class ResShare
    {
        public string user { get; set; }
        public string scope { get; set; }
        public string touser { get; set; }
        public List<string> access { get; set; }
    }
}
