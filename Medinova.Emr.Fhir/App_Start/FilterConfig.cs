using System.Web;
using System.Web.Mvc;

namespace Medinova.Emr.Fhir
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
