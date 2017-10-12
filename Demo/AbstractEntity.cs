using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jiaparts.LocalStorage;

namespace Jiaparts.LocalStorage.Demo
{
    public class AbstractEntity
    {
        [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int PK { get; set; }


        public DateTime CreateTime { get; set; }
    }
}
