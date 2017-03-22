using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Model
{
    public class resource
    {
        public resource()
        {
            scopes = new List<string>();
        }
        public string resource_set_id { get; set; }
        public List<string> scopes { get; set; }
        public long exp { get; set; }
    }
}
