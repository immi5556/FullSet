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
        static Uri endpoint = new Uri("https://oidc.medgrotto.com:9003/fhir");
        //static Uri endpoint = new Uri("http://localhost:49922/fhir");

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

        public static bool Update(MedicationStatement data)
        {
            var client = new FhirClient(endpoint);
            var query = new string[] { "identifier=" + data.Id };
            var bundle = client.Search("MedicationStatement", query);
            MedicationStatement p = new MedicationStatement();
            foreach (var entry in bundle.Entry)
            {
                p = (MedicationStatement)entry.Resource;
                p.Language = data.Language;
                p.DateAsserted = data.DateAsserted;

                client.Update<Hl7.Fhir.Model.MedicationStatement>(p);
            }
            return true;
        }
    }
}