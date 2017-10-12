using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jiaparts.LocalStorage;

namespace Jiaparts.LocalStorage
{
    public class AbstractEntityTable
    {
        // [SQLite.MaxLength(30),SQLite.Indexed]

        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public virtual int _id { get; set; }

    }
}
