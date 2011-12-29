using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace PostBin
{
    public static class Database
    {
        private static DynamicModel _db = new DynamicModel("MySQL");

        public static IEnumerable<dynamic> GetBins()
        {
            return _db.Query("SELECT * FROM bins");
        }

        public static dynamic GetBin(string name)
        {
            return _db.Query("SELECT * FROM bins WHERE NAME = @0", name).FirstOrDefault();
        }

        public static void CreateBin(string name)
        {
            _db.Execute("INSERT INTO bins(NAME) VALUES(@0)", name);
        }

        public static void DeleteBin(string name)
        {
            _db.Execute("DELETE FROM bins WHERE NAME = @0", name);
        }

        public static IEnumerable<dynamic> GetPosts(int binId)
        {
            return _db.Query("SELECT * FROM posts WHERE binId = @0", binId);
        }

        public  static void SavePost(int binId, string data)
        {
            _db.Execute("INSERT INTO posts(DATA, BINID) VALUES(@0, @1)", data, binId);
        }
    }
}