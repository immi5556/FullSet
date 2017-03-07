using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhir.Client
{
    class Print
    {
        static public string PrintAddr(Patient p)
        {
            string str = p.Id;
            foreach (var v in p.Name)
            {
                foreach (var v1 in v.Given)
                {
                    str = str + " " + v1;
                }
                foreach (var v1 in v.Family)
                {
                    str = str + " " + v1;
                }
            }
            str = str + "\r\n";
            foreach (var v in p.Address)
            {
                foreach (var v1 in v.Line)
                {
                    str = str + " " + v1;
                }
                str = str + " " + (v.City ?? "");
                str = str + " " + (v.District ?? "");
                str = str + " " + (v.Country ?? "");
            }
            str = str + "\r\n";
            return str;
        }
    }
}
