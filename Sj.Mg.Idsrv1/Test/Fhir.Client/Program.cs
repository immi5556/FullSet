using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhir.Client
{
    class Program
    {
        //static Uri endpoint = new Uri("http://spark.furore.com/fhir");
        //static Uri endpoint = new Uri("https://oidc.medgrotto.com:9003/fhir");
        static Uri endpoint = new Uri("http://localhost:49922/fhir");
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
    ((sender, certificate, chain, sslPolicyErrors) => true);
            //Observtion
            //InsertObservetion();
            //GetObservetion();

            //Medication
            //InsetMedication();
            //GetMeds();

            ///Accounts
            //InsertAcct();
            //GetAccts();
            //InsertAcctAddl("bob@smith.co");
            //InsertAcctAddl("john@john.co");
            //InsertAcctAddl("sam@sam.co");
            //InsertAcctAddl("saphire@saphire.co");
            //InsertAcctAddl("alice@bob.co");
            //InsertAcctAddl("bob@bob.co");
            //InsertAcctAddl("admin@medgrotto.com");

            ///Patients
            //InsertPats();
            //GetPats();
            InsertDiagn();
            Console.ReadKey();
        }

        static void InsertDiagn()
        {
            var tt = Newtonsoft.Json.JsonConvert.DeserializeObject<Patient>(System.IO.File.ReadAllText(@"D:\Immi\Projects\HeartWG\Openid\openid_dotnet\git_src\FullSet\Sj.Mg.Idsrv1\Test\Fhir.Client\Data\Patient_Sample.json"));
        }

        static void GetObservetion()
        {
            var client = new FhirClient(endpoint);

            //var query = new string[] { "name=Rob" };
            var query = new string[] { "Language=Obs" };
            var bundle = client.Search("Observation", query);

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            string str = "";
            foreach (var entry in bundle.Entry)
            {
                Observation p = (Observation)entry.Resource;
                str = str + p.Id + " " + p.Subject.Display + " " + "\r\n";
            }

            Console.WriteLine(str);
        }
        static void InsertObservetion()
        {
            Observation obs = new Observation()
            {
                BodySite = Common.GetCodeableConcept("BosySite", "DispBosySite", "BodySiteText"),
                Category = Common.GetCodeableConcept("Categ", "DispCateg", "TextCateg"),
                Code = Common.GetCodeableConcept(),
                Comments = "Obs comments",
                Component = new List<Observation.ComponentComponent>()
                {
                    new Observation.ComponentComponent()
                    {
                        Code = Common.GetCodeableConcept("ObsComp", "DispObsComp", "TextObsCode"),
                        DataAbsentReason = Common.GetCodeableConcept("DataAbs", "DispDataAbs", "ObsTextAbs"),
                        ReferenceRange = new List<Observation.ReferenceRangeComponent>()
                        {
                            new Observation.ReferenceRangeComponent()
                            {
                                Age = Common.GetRange()
                            }
                        }
                    }
                },
                Specimen = Common.GetResourceReference("ObsSepcimen", "Obsreference"),
                Device = Common.GetResourceReference("ObsDevice", "ObsDevReference"),
                DataAbsentReason = Common.GetCodeableConcept("ObsDataAbsentRason", "ObsDispDataAbsentRason", "TexttObs"),
                Encounter = Common.GetResourceReference("ObsEncounter", "EncounterRefer"),
                ImplicitRules = "ObsImplicitRukes",
                Interpretation = Common.GetCodeableConcept("ObsInterpretation", "ObsDispInterpretation", "TextObsInter"),
                Language = "ObsLang",
                Meta = Common.GetMeta(),
                Method = Common.GetCodeableConcept("ObsMeth", "ObsDisp", "ObsMethText"),
                Performer = new List<ResourceReference>()
                {
                    Common.GetResourceReference("ObsResPerf", "ObsResPerfref")
                },
                ReferenceRange = new List<Observation.ReferenceRangeComponent>()
                {
                    new Observation.ReferenceRangeComponent()
                    {
                        Age = Common.GetRange(),
                        High = Common.GetSimpleQuantity("HighCode", 123m, "HighUnit"),
                        Low = Common.GetSimpleQuantity("LowCode", 123m, "LowhUnit"),
                        Meaning = Common.GetCodeableConcept("MEanCOde", "MeanDisp", "MeanText"),
                        Text = "Text"
                    }
                },
                Subject = Common.GetResourceReference("SubDisp", "SubRef"),
                Text = Common.GetNarrative()
            };
            var client = new FhirClient(endpoint);
            client.Create<Observation>(obs);
        }
        static void GetMeds()
        {
            var client = new FhirClient(endpoint);

            //var query = new string[] { "name=Rob" };
            var query = new string[] { "Language=Med" };
            var bundle = client.Search("Medication", query);

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            string str = "";
            foreach (var entry in bundle.Entry)
            {
                Medication p = (Medication)entry.Resource;
                str = str + p.Id + " " + ((p.Product != null && p.Product.Form != null) ? p.Product.Form.Text : "") + " " + "\r\n";
            }

            Console.WriteLine(str);
        }
        static void InsetMedication()
        {
            Medication md = new Medication()
            {
                //Id = "qwqwq",
                Code = new CodeableConcept()
                {
                    Coding = new List<Coding>()
                    {
                        new Coding()
                        {
                            Code = "MedCode1",
                            Display = "MedoceDisplay",
                            Version = "V1.0",
                            System = "MedSys"
                        }
                    },
                    Text = "CodeText"
                },
                ImplicitRules = "ImpRules",
                Text = new Narrative()
                {
                    Div =  "<div class=\"atg:role: property - name\" id=\"ID\" />",
                    Status = Narrative.NarrativeStatus.Empty
                },
                Manufacturer = new ResourceReference()
                {
                    Display = "MedManf",
                    Url = new Uri(Common.nsurl)
                },
                Product = new Medication.ProductComponent()
                {
                    Batch = new List<Medication.BatchComponent>()
                    {
                        new Medication.BatchComponent()
                        {
                            ExpirationDate = "2012-05-30T09:00:00",
                            LotNumber = "Lot-1"
                        }
                    },
                    Form = new CodeableConcept()
                    {
                        Text = "Tablet|Carton|Powder",
                        Coding = new List<Coding>()
                        {
                           new Coding()
                           {
                               Code = "Tablet-1",
                               Display = "Dispaly-1",
                               Version = "V1",
                           }
                        }
                    },
                },
                Language = "MedLang",
                Meta = Common.GetMeta(),
                Package = new Medication.PackageComponent()
                {
                    Content = new List<Medication.ContentComponent>()
                    {
                        new Medication.ContentComponent()
                        {
                            Amount = new SimpleQuantity()
                            {
                                Value = 12121212.3434m,
                                Code = "Ccode",
                                Unit = "CM",
                                System = "MedSys"
                            }
                        }
                    }
                },
                IsBrand = true
            };
            var client = new FhirClient(endpoint);
            client.Create<Medication>(md);
        }
        static void GetAccts()
        {
            var client = new FhirClient(endpoint);

            //var query = new string[] { "name=Rob" };
            //var query = new string[] { "name=Acc" };
            //var bundle = client.Search("Account", query);

            var bundle = client.Search("Account");

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            string str = "";
            foreach (var entry in bundle.Entry)
            {
                Account p = (Account)entry.Resource;
                str = str + p.Id + " " + p.Name + " " + "\r\n";
            }

            Console.WriteLine(str);
        }
        static void InsertAcctAddl(string email)
        {
            var client = new FhirClient(endpoint);
            Account act = new Account()
            {
                Name = email,
                Status = "Status1",
                Type = new CodeableConcept()
                {
                    Text = "ActType"
                },
                Currency = Common.getCoding(),
                ActivePeriod = Common.getPeriod(),
                Balance = Common.getMoney(),
                CoveragePeriod = Common.getPeriod(),
                VersionId = "1.0",
                Description = "Some dec abt account",
                Identifier = new List<Identifier>()
                {
                    new Identifier()
                    {
                        Assigner = new ResourceReference()
                        {
                            Display = "Alt Display Resource",
                            Reference = "Ref url",
                            Url = new Uri(Common.nsurl)
                        },
                        Value = ""
                    }
                }
            };
            client.Create<Account>(act);
        }
        static void InsertAcct()
        {
            var client = new FhirClient(endpoint);
            Account act = new Account()
            {
                Name = "Account1",
                Status = "Status1",
                Type = new CodeableConcept()
                {
                    Text = "ActType"
                },
                Currency = Common.getCoding(),
                ActivePeriod = Common.getPeriod(),
                Balance = Common.getMoney(),
                CoveragePeriod = Common.getPeriod(),
                VersionId = "1.0",
                Description = "Some dec abt account",
                Identifier = new List<Identifier>()
                {
                    new Identifier()
                    {
                        Assigner = new ResourceReference()
                        {
                            Display = "Alt Display Resource",
                            Reference = "Ref url",
                            Url = new Uri(Common.nsurl)
                        },
                        Value = ""
                    }
                }
            };
            client.Create<Account>(act);
        }
        static void InsertPats()
        {
            var client = new FhirClient(endpoint);

            Patient p = client.Create<Patient>(new Patient()
            {
                //Id = "SomeID--" + new Random().Next(1, 121212).ToString(),
                Name = new List<HumanName>()
                {
                    new HumanName()
                    {
                        Family = new string[] { "FamilyName" },
                        Given = new string[] { "GivenName1" }
                    },
                    new HumanName()
                    {
                        Given = new string[] { "GivenName" }

                    }
                },
                Address = Common.GetAddr()
                
            });
            Console.WriteLine(p.Id);
        }

        static void GetPats()
        {
            //var endpoint = new Uri("http://spark.furore.com/fhir");
            //var endpoint = new Uri("http://localhost:49922/fhir");
            var client = new FhirClient(endpoint);

            //var query = new string[] { "name=Rob" };
            var query = new string[] { "name=FamilyName" };
            var bundle = client.Search("Patient", query);

            Console.WriteLine("Got " + bundle.Entry.Count() + " records!");
            string str = "";
            foreach (var entry in bundle.Entry)
            {
                Patient p = (Patient)entry.Resource;
                //str = str + p.Id + " " + p.Name.First().Text + " " + "\r\n";
                str = str + Print.PrintAddr(p);
            }

            Console.WriteLine(str);
        }        
    }
}
