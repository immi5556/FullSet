using DataBaseConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sj.Mg.Idsrv4.Config
{
    public class Permissions
    {
        public static Dictionary<string, dynamic> _permtckts = new Dictionary<string, dynamic>();

        public static string AddTicket(string tte)
        {
            dynamic rpt = AppConstants.Helper.TokenHelper.DecodeAndWrite(tte);
            string gn = rpt.given_name;
            string rn = rpt.access_name;
            string allwscope = rpt.allowed_scope;
            string key = Guid.NewGuid().ToString().Replace("-", "");
            _permtckts.Add(key, rpt);
            return key;
        }

        public static bool ContainsTicket(string tkt)
        {
            return _permtckts.ContainsKey(tkt);
        }

        public static bool GetAllowedUsers(string tkt)
        {
            dynamic rpt = _permtckts[tkt];
            string rn = rpt.access_name;
            string gn = rpt.given_name;
            string allwscope = rpt.allowed_scope;
            var tt = Config.Users.GetDetails().Find(t => t.UserName == rn);
            var rest = false;
            if (tt.ScopeUsers.Any() && tt.ScopeUsers.Keys.Contains(allwscope))
            {
                rest = tt.ScopeUsers[allwscope].Contains(gn);
            }
            
            if (!rest)
            {
                Config.Users.RegisterRequest(new Constants.Model.ResShare()
                {
                    scope = allwscope,
                    user = gn,
                    touser = rn
                });
            }
            return rest;
        }
    }
}