using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.LocalStorage
{
    public interface ISQLiteDatabase
    {
        /// <summary>
        /// 创建数据库
        /// </summary>
        void OnCreate();
        
        /// <summary>
        /// 对数据库表进行升级时候会触发
        /// </summary>
        /// <param name="oldVersion">老版本</param>
        /// <param name="newVersion">当前最新版本</param>
        void OnUpgrade(int oldVersion, int newVersion);
    }
}
