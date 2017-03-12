using MongoDB.Bson;
using MongoDB.Driver;
using Sj.Mg.Model;

namespace Sj.Mg.Mongo.Data
{
    public class FilterManage
    {
        public static FilterDefinition<T> GetCustomerFilter<T>(T t)
        {
            FilterDefinition<T> filt = GetCustFilter(t);
            return filt;
        }

        static string GetCustFilter<T>(T t)
        {
            if (t is Sj.Mg.Model.CustomUser)
            {
                Sj.Mg.Model.CustomUser t1 = t as Sj.Mg.Model.CustomUser;
                return Newtonsoft.Json.JsonConvert.SerializeObject(new Sj.Mg.Model.CustomUser()
                {
                    Username = t1.Username,
                    Password = t1.Password,
                    Subject = t1.Subject
                });
            }

            return null;
        }

    }
}
