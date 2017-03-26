using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Model
{
    public class RequestPerm
    {

        public string MyEmail { get; set;  }
        public Dictionary<string, Dictionary<string, List<string>>> AllowedUsers { get; set; }
    }
}
