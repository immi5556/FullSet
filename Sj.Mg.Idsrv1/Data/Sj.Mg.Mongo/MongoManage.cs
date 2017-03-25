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
    }
}
