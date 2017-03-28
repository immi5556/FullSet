using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sj.Mg.Resource.Server.Code
{
    public class Utils
    {
        public static string nsurl = "http://oidc.medgrotto.com/";
        public static ResourceReference GetResourceReference(string display, string reference)
        {
            return new ResourceReference()
            {
                Display = display,
                Reference = reference,
                Url = new Uri("https://oidc.medgrotto.com/")
            };
        }
        public static List<Address> GetAddr()
        {
            return GetAddr("City1", "COuntry1", "Diostrict1");
        }
        public static List<Address> GetAddr(string city, string country, string district)
        {
            return GetAddr(city, country, district, getPeriod());
        }
        public static List<Address> GetAddr(string city, string country, string district, Period period)
        {
            return new List<Address>()
                {
                    new Address()
                    {
                        City = city,
                        Country = country,
                        District = district,
                        Period = period
                    }
                };
        }
        public static Period getPeriod()
        {
            return getPeriod("2012-05-30T09:00:00", "2016-05-30T09:00:00");
        }
        public static Period getPeriod(string start, string end)
        {
            return new Period()
            {
                Start = start,
                End = end
            };
        }

        public static CodeableConcept GetCodeableConcept()
        {
            return GetCodeableConcept("code", "Display", "Text");
        }
        public static CodeableConcept GetCodeableConcept(string code, string display, string text)
        {
            return new CodeableConcept()
            {
                Coding = new List<Coding>()
                {
                    getCoding(code, display)
                },
                Text = text
            };
        }
        public static Coding getCoding()
        {
            return getCoding("$", "SomeDisp");
        }
        public static Coding getCoding(string code, string display)
        {
            return new Coding()
            {
                Code = code,
                Display = code + display,
                Extension = new List<Extension>()
                        {
                            new Extension()
                            {
                                Url = "Extension url"
                            }
                        },
                System = code + "System",
                Version = code + "-V1"
            };
        }
        public static Range GetRange()
        {
            return GetRange("Age", 123m, "No");
        }
        public static Range GetRange(string code, decimal val, string unit)
        {
            return new Range()
            {
                High = new SimpleQuantity()
                {
                    Code = code,
                    Value = val,
                    Unit = unit
                },
                Low = new SimpleQuantity()
                {
                    Code = code,
                    Value = val,
                    Unit = unit
                }
            };
        }
        public static SimpleQuantity GetSimpleQuantity(string code, decimal val, string unit)
        {
            return new SimpleQuantity()
            {
                Code = code,
                Value = val,
                Unit = unit
            };
        }
        public static Meta GetMeta()
        {
            return new Meta()
            {
                Profile = new List<string>()
                    {
                        "MetaProf-1",
                        "MetaProf-2",
                        "MetaProf-3"
                    },
                VersionId = "V2",
                Security = new List<Coding>() { getCoding("SecCode", "SecDisp") },
                Tag = new List<Coding>() { getCoding("TagCode", "TagDisp") }
            };
        }
        public static Money getMoney()
        {
            return new Money()
            {
                Code = "$",
                Value = 123m
            };
        }

        public static Narrative GetNarrative()
        {
            return new Narrative()
            {
                Div = "<div class=\"atg:role: property - name\" id=\"ID\" />",
                Status = Narrative.NarrativeStatus.Empty
            };
        }
    }
}