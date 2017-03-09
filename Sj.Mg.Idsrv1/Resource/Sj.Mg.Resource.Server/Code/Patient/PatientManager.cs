﻿using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sj.Mg.Resource.Server.Code
{
    public class PatientManager
    {
        public static List<Hl7.Fhir.Model.Patient> Get(string search)
        {
            List<Hl7.Fhir.Model.Patient> ret = new List<Hl7.Fhir.Model.Patient>();
            var client = new FhirClient(Utils.endpoint);
            var query = new string[] { "name=" + (search ?? "") };
            Bundle bundle = null;
            if (string.IsNullOrEmpty(search))
            {
                bundle = client.Search("Patient");
            }
            else
            {
                bundle = client.Search("Patient", query);
            }

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            string str = "";
            foreach (var entry in bundle.Entry)
            {
                Patient p = (Patient)entry.Resource;
                ret.Add(p);
            }

            return ret;
        }
    }
}