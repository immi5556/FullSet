using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.Model
{
    public class CustomClaim
    {
        public CustomClaim(string type, string value) : this(type, value, null)
        {
        }
        public CustomClaim(string type, string value, string valueType)
        {
            this.type = type;
            this.value = value;
            if (!string.IsNullOrEmpty(valueType))
                this.valueType = valueType;
        }
        public string type { get; set; }
        public string value { get; set; }
        public string valueType { get; set; }
    }
}
