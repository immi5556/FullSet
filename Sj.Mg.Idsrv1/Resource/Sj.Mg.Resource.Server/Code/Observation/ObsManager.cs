﻿using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sj.Mg.Resource.Server.Code
{
    public class ObsManager
    {
        public static List<Hl7.Fhir.Model.Observation> Get(string search)
        {
            List<Hl7.Fhir.Model.Observation> ret = new List<Hl7.Fhir.Model.Observation>();
            var client = new FhirClient(CliLib.Utils.Common.fhirendpoint);
            var query = new string[] { "identifier=" + (search ?? "") };
            Bundle bundle = null;
            if (string.IsNullOrEmpty(search))
            {
                bundle = client.Search("Observation");
            }
            else
            {
                bundle = client.Search("Observation", query);
            }

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            foreach (var entry in bundle.Entry)
            {
                Observation p = (Observation)entry.Resource;
                ret.Add(p);
            }

            return ret;
        }
    }
}