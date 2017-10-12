using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.LocalStorage
{
    /// <summary>
    /// SQLite工厂类
    /// </summary>
    public class SQLiteFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<Type, ISQLiteDatabase> databases = new Dictionary<Type, ISQLiteDatabase>();

        /// <summary>
        /// 获取 SQLiteDatabase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetDatabase<T>() where T:ISQLiteDatabase,new()
        {
            Type type = typeof(T);

            if (databases.ContainsKey(type))
                return (T)databases[type];

            lock (databases)
            { 
                if(!databases.ContainsKey(type))
                    databases.Add(type, new T());
                
                return (T)databases[type];
            }
        }
    }
}
