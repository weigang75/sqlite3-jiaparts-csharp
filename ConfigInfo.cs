using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.LocalStorage
{
    /// <summary>
    /// 用户配置
    /// </summary>
    public class user_config
    {
        private String m_Key = null;
        [SQLite.PrimaryKey, SQLite.MaxLength(30),SQLite.Indexed]
        public String key 
        {
            get
            {
                return m_Key;
            }

            set
            {
                m_Key = value.ToLower();
            }
        }

        [SQLite.MaxLength(200)]
        public String value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String user { get; set; }
    }
}
