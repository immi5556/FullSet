using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConstants.Model
{
    public class Permission
    {
        public string ticket { get; set; }
        public DateTime expires { get; set; }
    }

    public class PermissionRequest
    {
        public PermissionRequest()
        {
            scopes = new List<string>();
        }
        public string resource_set_id { get; set; }
        public List<string> scopes { get; set; }
    }
}
