using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.LocalStorage
{
    public class DbEntity
    { 
        /// <summary>
        /// 
        /// </summary>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public virtual int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SQLite.Ignore]
        public virtual int Version { get; set; }
    }
}
