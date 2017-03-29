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
        public static List<Hl7.Fhir.Model.MedicationStatement> GetStatement(string search)
        {
            List<Hl7.Fhir.Model.MedicationStatement> ret = new List<Hl7.Fhir.Model.MedicationStatement>();
            var client = new FhirClient(CliLib.Utils.Common.fhirendpoint);

            var query = new string[] { "identifier=" + (search ?? "") };
            Bundle bundle = null;
            if (string.IsNullOrEmpty(search))
            {
                bundle = client.Search("MedicationStatement");
            }
            else
            {
                bundle = client.Search("MedicationStatement", query);
            }

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            foreach (var entry in bundle.Entry)
            {
                MedicationStatement p = (MedicationStatement)entry.Resource;
                ret.Add(p);
            }

            return ret;
        }
    }
}