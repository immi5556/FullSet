using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sj.Mg.Idsrv4.Config
{
    public class Scopes
    {
        static List<Scope> _lstscopes = new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.Address,
                new Scope()
                {
                    Name = "user.Observation",
                    DisplayName = "observation for users",
                    Description = "patient's observation",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", false)
                    }
                },
                new Scope()
                {
                    Name = "patient.MedicationOrder",
                    DisplayName = "orders for medications",
                    Description = "patient's orders for medications",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", false)
                    }
                },
                new Scope()
                {
                    Name = "roles",
                    DisplayName = "Role(s)",
                    Description = "Allow apps to see your role(s)",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", true)
                    }
                },
                new Scope()
                {
                    Name = "uma_authorization",
                    DisplayName = "Uma Authz",
                    Description = "Uma authorization to enable AAT token to client",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", true)
                    }
                },
                new Scope()
                {
                    Name = "uma_protection",
                    DisplayName = "Uma protect",
                    Description = "Uma Protection to enable PAT token to resource server",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", true)
                    }
                },
                new Scope()
                {
                    Name = "re-api",
                Type = ScopeType.Identity,
                Emphasize = true,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("name", true),
                        new ScopeClaim("given_name", true),
                        new ScopeClaim("family_name", true),
                        new ScopeClaim("email", true),
                        new ScopeClaim("role", true)
                    }
                }
            };
        static Scopes()
        {
            AddFhirScopes();
        }
        public static IEnumerable<Scope> Get()
        {
            return _lstscopes;
        }
        public static AppConstants.Model.ResourceSet Insert(AppConstants.Model.ResourceSet set)
        {
            if (_lstscopes.Find(t => t.Name == set._id) == null)
            {
                set._id = Guid.NewGuid().ToString();
                var t1 = new Scope()
                {
                    Name = set._id,
                    Type = ScopeType.Resource,
                    DisplayName = set.name,
                    Claims = new List<ScopeClaim>()
                };
                t1.Claims = new List<ScopeClaim>();
                (set.scopes ?? new List<string>()).ForEach(t2 =>
                {
                    t1.Claims.Add(new ScopeClaim()
                    {
                        Name = t2,
                        Description = t2
                    });
                });
                _lstscopes.Add(t1);
            }
            return set;
        }

        public static AppConstants.Model.ResourceSet GetResource(string id)
        {
            var tt = _lstscopes.Find(t => t.Name == id) ?? new Scope();
            return new AppConstants.Model.ResourceSet()
            {
                name = tt.DisplayName,
                _id = id,
                scopes = tt.Claims.Select(t => t.Name).ToList()
            };
        }

        public static AppConstants.Model.ResourceSet UpdateResource(string id, AppConstants.Model.ResourceSet set)
        {
            set._id = id;
            var tt = _lstscopes.Find(t => t.Name == id) ?? new Scope();
            var t1 = new Scope()
            {
                Name = set._id,
                Type = ScopeType.Resource,
                DisplayName = set.name,
                Claims = new List<ScopeClaim>()
            };
            t1.Claims = new List<ScopeClaim>();
            (set.scopes ?? new List<string>()).ForEach(t2 =>
            {
                t1.Claims.Add(new ScopeClaim()
                {
                    Name = t2,
                    Description = t2
                });
            });
            tt = t1;
            return set;
        }

        public static void DeleteResource(string id)
        {
            if (_lstscopes.Find(t => t.Name == id) != null)
                _lstscopes.Remove(_lstscopes.Find(t => t.Name == id));
        }

        public static void AddFhirScopes()
        {
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/Patient.read",
                    DisplayName = "patient/Patient.read",
                    Description = "Read access to a single patient's demographic information.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/Patient.write",
                    DisplayName = "patient/Patient.write",
                    Description = "Read and write access to a single patient's demographic information.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/Patient.*",
                    DisplayName = "patient/Patient.*",
                    Description = "Full access to a single patient's demographic information.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationOrder.read",
                    DisplayName = "patient/MedicationOrder.read",
                    Description = "Read access to a single patient's orders for medications.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationOrder.write",
                    DisplayName = "patient/MedicationOrder.write",
                    Description = "Read and write access to a single patient's orders for medications.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationOrder.*",
                    DisplayName = "patient/MedicationOrder.*",
                    Description = "Full access to a single patient's orders for medications.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationDispense.read",
                    DisplayName = "patient/MedicationDispense.read",
                    Description = "Read access to supply of medications to a single patient.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationDispense.write",
                    DisplayName = "patient/MedicationDispense.write",
                    Description = "Read and write access to supply of medications to a single patient.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationDispense.*",
                    DisplayName = "patient/MedicationDispense.*",
                    Description = "Full access to supply of medications to a single patient.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationAdministration.read",
                    DisplayName = "patient/MedicationAdministration.read",
                    Description = "Read access to a single patient's medications consumption or other administration.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationAdministration.write",
                    DisplayName = "patient/MedicationAdministration.write",
                    Description = "Read and write access to a single patient's medications consumption or other administration.",
                    Type = ScopeType.Resource
                });
        }
    }
}