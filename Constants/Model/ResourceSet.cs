using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConstants.Model
{
    public class ResourceSet
    {
        public ResourceSet()
        {
            scopes = new List<string>();
        }
        public string name { get; set; }
        public string icon_uri { get; set; }
        public List<string> scopes { get; set; }
        public string type { get; set; }
        public string _id { get; set; }
    }
}
