using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sj.Mg.Resource.Server.Code
{
    public class MedicManager
    {
        public static List<Hl7.Fhir.Model.Medication> Get(string search)
        {
            List<Hl7.Fhir.Model.Medication> ret = new List<Hl7.Fhir.Model.Medication>();
            var client = new FhirClient(CliLib.Utils.Common.fhirendpoint);

            var query = new string[] { "Language=" + (search ?? "") };
            Bundle bundle = null;
            if (string.IsNullOrEmpty(search))
            {
                bundle = client.Search("Medication");
            }
            else
            {
                bundle = client.Search("Medication", query);
            }

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            foreach (var entry in bundle.Entry)
            {
                Medication p = (Medication)entry.Resource;
                ret.Add(p);
            }

            return ret;
        }
    }
}