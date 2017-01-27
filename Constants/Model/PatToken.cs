using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants.Model
{
    public class PatToken
    {
        public bool active { get; set; }
        public long exp { get; set; }
        public long iat { get; set; }
        public List<string> scopes { get; set; }
    }
}
