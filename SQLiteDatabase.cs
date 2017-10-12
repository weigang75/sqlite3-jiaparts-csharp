using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.LocalStorage
{
    public class SQLiteDatabase<T> : SQLite.SQLiteConn, ISQLiteDatabase where T : new()//where T : DbEntity ,new()
    {
        public const String NullString = "";
        
        /// <summary>
        /// 数据库表对象基类（数据库加入加密）
        /// </summary>
        /// <param name="databaseFile">数据库文件地址</param>
        /// <param name="password">加入密码</param>
        public SQLiteDatabase(string databaseFile, string password)
            : base(databaseFile, password)
        {
            CreateTable<T>();
        }

        /// <summary>
        /// 数据库表对象基类（数据库无加密）
        /// </summary>
        /// <param name="databaseFile">数据库文件地址</param>
        public SQLiteDatabase(string databaseFile)
            : this(databaseFile, "")
        {
            
        }

        public T GetByPk(object pk)
        {
            return base.Get<T>(pk);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ConvertToUtf8(string str)
        {
            byte[] utf8bytes = System.Text.Encoding.Default.GetBytes(str);
            string utf8 = System.Text.Encoding.UTF8.GetString(utf8bytes);
            return utf8;
        }

        /// <summary>
        /// 当前的表名
        /// </summary>
        public virtual string TableName
        {
            get
            {
                return this.Table<T>().Table.TableName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(T obj)
        {
            return base.Insert(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(T obj)
        {
            return base.Update(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Delete(T obj)
        {
            return base.Delete<T>(obj);           
        }

        /// <summary>
        /// 对数据库表进行升级时候会触发
        /// </summary>
        /// <param name="oldVersion">老版本</param>
        /// <param name="newVersion">当前最新版本</param>
        public virtual void OnUpgrade(int oldVersion, int newVersion)
        {

        }

        /// <summary>
        /// 创建数据库表
        /// </summary>
        public void OnCreate()
        {
            
        }
    }
}
