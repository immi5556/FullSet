﻿using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sj.Mg.Resource.Server.Code
{
    public class AccountManager
    {
        public static List<Hl7.Fhir.Model.Account> Get(string search)
        {
            var client = new FhirClient(Utils.endpoint);
            List<Hl7.Fhir.Model.Account> ret = new List<Account>();
            var query = new string[] { "name=" + (search ?? "") };
            Bundle bundle = null;
            if (string.IsNullOrEmpty(search))
            {
                bundle = client.Search("Account");
            }
            else
            {
                bundle = client.Search("Account", query);
            }

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            //string str = "";
            foreach (var entry in bundle.Entry)
            {
                Account p = (Account)entry.Resource;
                //str = str + p.Id + " " + p.Name + " " + "\r\n";
                ret.Add(p);
            }
            return ret;
        }
        public static void Insert(string name, string status)
        {
            Insert(name, status, null, null);
        }
        public static void Insert(string name, string status,
            string actype, string desc)
        {
            var client = new FhirClient(Utils.endpoint);
            Account act = new Account()
            {
                Name = name ?? "Account1",
                Status = status ?? "Status1",
                Type = new CodeableConcept()
                {
                    Text = actype ?? "ActType"
                },
                Currency = Utils.getCoding(),
                ActivePeriod = Utils.getPeriod(),
                Balance = Utils.getMoney(),
                CoveragePeriod = Utils.getPeriod(),
                VersionId = "1.0",
                Description = desc ?? "Some dec abt account",
                Identifier = new List<Identifier>()
                {
                    new Identifier()
                    {
                        Assigner = new ResourceReference()
                        {
                            Display = "Alt Display Resource",
                            Reference = "Ref url",
                            Url = new Uri(Utils.nsurl)
                        },
                        Value = ""
                    }
                }
            };
            client.Create<Account>(act);
        }
    }
}