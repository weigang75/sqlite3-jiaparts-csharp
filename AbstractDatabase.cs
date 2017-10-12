using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jiaparts.LocalStorage;

namespace Jiaparts.LocalStorage
{
    public class AbstractDatabase<T> : SQLiteDatabase<T> where T : AbstractEntityTable ,new( )
    {
        public AbstractDatabase(string databaseFile, string password)
            : base(databaseFile, password)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Save(T obj)
        {
            if (obj._id > 0)
            {
                return Update(obj);
            }
            else
            {
                return Insert(obj);
            }
        }

        public T GetById(object id)
        {
            return base.GetByPk(id);
        }      
    }
}
