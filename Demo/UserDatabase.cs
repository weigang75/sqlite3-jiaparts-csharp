using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.LocalStorage.Demo
{
    public class UserDatabase : AbstractDatabase<UserInfo>
    {
        public UserDatabase():base("db\\user.db","123")
        {
            // 注册Mapping
            CreateView<UserView>();
        }

        public List<UserView> QueryUsers(String nickname)
        {
            String query = @"select * from UserInfo u where u.NickName like '%'+?+'%'";

            List<UserView> retList = Query<UserView>(query, new object[] { nickname });

            return retList;
        }

        public IEnumerable<UserInfo> QureyAllUsers()
        {
            return Table<UserInfo>();
        }

        public IEnumerable<UserInfo> QureyUsersBy(String nickname)
        {
            return Table<UserInfo>().Where(x => x.NickName == nickname);
        }
    }
}
