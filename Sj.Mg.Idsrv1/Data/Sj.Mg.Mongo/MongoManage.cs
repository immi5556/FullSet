using IdentityServer3.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.Mongo
{
    public class MongoManage
    {
        public static void Insert<T>(T data, string tbl)
        {
            var db = Data.BaseMongo.GetDatabase();
            var toins = db.GetCollection<T>(tbl);
            toins.InsertOne(data);
        }
        public static void Update<T>(T filter, T change, string tbl)
        {
            var db = Data.BaseMongo.GetDatabase();
            db.GetCollection<T>(tbl).FindOneAndReplace(Data.FilterManage.GetCustomerFilter<T>(filter), change);
        }
        public static List<T> Select<T>(T filter, string tbl)
        {
            var db = Data.BaseMongo.GetDatabase();
            return db.GetCollection<T>(tbl).Find(Data.FilterManage.GetCustomerFilter<T>(filter)).ToList<T>();
        }
        public static List<T> Select<T>(Dictionary<string, object> filter, string tbl)
        {
            var db = Data.BaseMongo.GetDatabase();
            var tt = filter == null ? new BsonDocument() : new BsonDocument(filter);
            return db.GetCollection<T>(tbl).Find(tt).ToList<T>();
        }

        public static List<Sj.Mg.CliLib.Model.permission> Select(string _id, string tbl)
        {
            var db = Data.BaseMongo.GetDatabase();
            return db.GetCollection<Sj.Mg.CliLib.Model.permission>(tbl).Find(_ => _.Id == new ObjectId(_id)).ToList();
        }

        public static List<IdentityServer3.Core.Models.Client> SelectClients(string tbl)
        {
            var db = Data.BaseMongo.GetDatabase();
            return db.GetCollection<IdentityServer3.Core.Models.Client>(tbl).Find(ClientId => true).ToList();
        }

        public static List<Sj.Mg.CliLib.Model.CustomUser> SearchUser(string srch)
        {
            var db = Data.BaseMongo.GetDatabase();
            return db.GetCollection<Sj.Mg.CliLib.Model.CustomUser>("Users").Find(_ => _.Subject.ToLower().Contains((srch ?? "").ToLower())).ToList();
        }
        public static List<Sj.Mg.CliLib.Model.UserClientsList> SearchUserClients(string srch)
        {
            var db = Data.BaseMongo.GetDatabase();
            return db.GetCollection<Sj.Mg.CliLib.Model.UserClientsList>("UsersClientsData").Find(_ => _.email.ToLower().Contains((srch ?? "").ToLower())).ToList();
        }
        public static void UpdateUserClients(Sj.Mg.CliLib.Model.UserClientsList data)
        {
            var db = Data.BaseMongo.GetDatabase();
            var toins = db.GetCollection<Sj.Mg.CliLib.Model.UserClientsList>("UsersClientsData");
            toins.FindOneAndReplace<Sj.Mg.CliLib.Model.UserClientsList>(f => f.email == data.email, data);
        }
        public static void ReplaceUser(Sj.Mg.CliLib.Model.CustomUser data)
        {
            var db = Data.BaseMongo.GetDatabase();
            var toins = db.GetCollection<Sj.Mg.CliLib.Model.CustomUser>("Users");
            toins.FindOneAndReplace<Sj.Mg.CliLib.Model.CustomUser>(f => f.Subject == data.Subject, data);
        }
        public static List<Sj.Mg.CliLib.Model.RequestPerm> GetUserPerms()
        {
            var db = Data.BaseMongo.GetDatabase();
            return db.GetCollection<Sj.Mg.CliLib.Model.RequestPerm>("ReqPerms").Find(_ => true).ToList();
        }

        public static List<Sj.Mg.CliLib.Model.RequestPerm> GetReqUserPerms(string userId)
        {
            var db = Data.BaseMongo.GetDatabase();
            var toins = db.GetCollection<Sj.Mg.CliLib.Model.RequestPerm>("ReqPerms");
            return toins.Find<Sj.Mg.CliLib.Model.RequestPerm>(f => f.MyEmail == userId).ToList();
        }

        public static void ReplaceReqPerm(Sj.Mg.CliLib.Model.RequestPerm data)
        {
            var db = Data.BaseMongo.GetDatabase();
            var toins = db.GetCollection<Sj.Mg.CliLib.Model.RequestPerm>("ReqPerms");
            toins.FindOneAndReplace<Sj.Mg.CliLib.Model.RequestPerm>(f => f.MyEmail == data.MyEmail, data);
        }

        public static void ReplaceClient(Client data)
        {
            var db = Data.BaseMongo.GetDatabase();
            var toins = db.GetCollection<Client>("Clients");
            toins.FindOneAndReplace<Client>(f => f.ClientId == data.ClientId, data);
        }

        public static void ReplaceUserByUserName(Sj.Mg.CliLib.Model.CustomUser data)
        {
            var db = Data.BaseMongo.GetDatabase();
            var toins = db.GetCollection<Sj.Mg.CliLib.Model.CustomUser>("Users");
            toins.FindOneAndReplace<Sj.Mg.CliLib.Model.CustomUser>(f => f.Username == data.Username, data);
        }
    }
}
