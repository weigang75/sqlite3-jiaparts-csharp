using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.LocalStorage.Demo
{
    public class UserInfo : AbstractEntity
    {
        [SQLite.Indexed]
        public String UserId { get; set; }

        public String NickName { get; set; }
    }
}
