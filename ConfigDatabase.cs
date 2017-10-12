using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.LocalStorage
{
    public class ConfigDatabase : SQLiteDatabase<user_config>
    {
        public ConfigDatabase()
            : base("db\\config.db")
        {

        }

        public void Save(user_config conf)
        { 
            String key = conf.key;
            IEnumerable<user_config> list = Table<user_config>().Where(x => x.key == key);
            if (list.Count() == 0)
            {
                Insert(conf);
            }
            else
            {
                user_config config = list.FirstOrDefault<user_config>();
                if (!config.value.Equals(conf.value))
                {
                    config.value = conf.value;
                    Update(config);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Save(String key, String value)
        {
            user_config config = new user_config() { key = key, value = value };
            Save(config);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String GetValue(String key)
        {
            String keyLower = key.ToLower();
            IEnumerable<user_config> list = Table<user_config>().Where(x => x.key == keyLower);
            if (list.Count() == 0)
            {
                return null;
            }
            return list.FirstOrDefault<user_config>().value;
        }
    }
}
