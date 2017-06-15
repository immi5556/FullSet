using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Model
{
    public class UpdateUsersClientData
    {
        public UserClientsList userClientsList { get; set; }
        public bool delClient { get; set; }
        public bool delScope { get; set; }
        public string delItem { get; set; }
    }

    public class ReqParamObj
    {
        public string scope { get; set; }
        public string user { get; set; }
        public string provider { get; set; }
        public string scopeType { get; set; }
    }
}
