﻿using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Resource.Server
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}