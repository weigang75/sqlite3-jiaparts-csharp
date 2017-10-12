using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jiaparts.LocalStorage;

namespace Jiaparts.LocalStorage.Demo
{
    public abstract class AbstractDatabase<T> : SQLiteDatabase<T> where T : AbstractEntity, new()
    {

        public AbstractDatabase(string databaseFile, string password) : base(databaseFile, password)
        {
            CreateTable<T>();
            //SQLite.SQLiteConnection.DatabaseFile = "sqlite.db";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public T GetByPk(int pk)
        {
            return base.Get<T>(pk);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<T> GetByDate(DateTime begin, DateTime end)
        {
            return from s in Table<T>()
                   where (s.CreateTime >= begin) && (s.CreateTime <= end)
                   orderby s.CreateTime descending
                   orderby s.PK descending
                   select s;
        }
    }
}
