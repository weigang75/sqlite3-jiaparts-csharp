using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Runtime.InteropServices;

namespace Jiaparts.LocalStorage
{
    /// <summary>
    /// 
    /// </summary>
    public static class SQLiteExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(this SQLiteCommand cmd)
        {
            return (T)cmd.ExecuteScalar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        //public static List<T> ExecuteQuery<T>(this SQLiteCommand cmd)
        //{
        //    return ExecuteQuery<T>(_conn.GetMapping(typeof(T)));

        //}

        public static int ExecuteNonQuery(this SQLiteCommand cmd, object[] vals)
        {
            cmd.Parameters.Clear();
            foreach (var item in vals)
            {
                var p = cmd.CreateParameter();
                p.Value = item;
                cmd.Parameters.Add(p);
            }

            return cmd.ExecuteNonQuery();
        }

        public static List<T> ExecuteQuery<T>(this SQLiteCommand cmd, SQLite.TableMapping map)
        {
            var reader = cmd.ExecuteReader();
            List<T> list = new List<T>();
            var cols = new SQLite.TableMapping.Column[reader.FieldCount];
           

            int propIdx = 0;
            foreach (var prop in map.MappedType.GetProperties())
            {
                cols[propIdx++] = map.FindColumn(prop.Name);
            }

            //for (int i = 0; i < cols.Length; i++)
            //{
            //    var name = Marshal.PtrToStringUni(SQLite3.ColumnName16(stmt, i));
            //    cols[i] = map.FindColumn(name);
            //}

            Dictionary<string, int> nameMaps = new Dictionary<string, int>();
            //
            
            while (reader.Read())
            {
                var obj = Activator.CreateInstance(map.MappedType);
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i] == null)
                        continue;

                    string colName = cols[i].Name;

                    if (!nameMaps.ContainsKey(colName))
                    {
                        int idx = reader.FindNameIdx(colName);
                        nameMaps.Add(colName, idx);
                    }

                    //var colType = SQLite3.ColumnType(stmt, i);
                    //var val = ReadCol(stmt, i, colType, cols[i].ColumnType);
                    cols[i].SetValue(obj, reader[nameMaps[colName]]);
                }
                list.Add((T)obj);
            }

            return list;// (T)
        }

        private static int FindNameIdx(this SQLiteDataReader reader, string name)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if(name.Equals(reader.GetName(i)))
                {
                    return i;
                }
            }
            return -1;
        }


        // public List<T> ExecuteQuery<T>(SQLite.SQLiteConn _conn, SQLite.TableMapping map)
        //{
        //    if (_conn.Trace)
        //    {
        //        Console.WriteLine("Executing Query: " + this);
        //    }

        //    var r = new List<T>();

        //    var stmt = Prepare();

        //    var cols = new TableMapping.Column[SQLite3.ColumnCount(stmt)];

        //    for (int i = 0; i < cols.Length; i++)
        //    {
        //        var name = Marshal.PtrToStringUni(SQLite3.ColumnName16(stmt, i));
        //        cols[i] = map.FindColumn(name);
        //    }

        //    while (SQLite3.Step(stmt) == SQLite3.Result.Row)
        //    {
        //        var obj = Activator.CreateInstance(map.MappedType);
        //        for (int i = 0; i < cols.Length; i++)
        //        {
        //            if (cols[i] == null)
        //                continue;
        //            var colType = SQLite3.ColumnType(stmt, i);
        //            var val = ReadCol(stmt, i, colType, cols[i].ColumnType);
        //            cols[i].SetValue(obj, val);
        //        }
        //        r.Add((T)obj);
        //    }

        //    Finalize(stmt);
        //    return r;
        //}

        // public T ExecuteScalar<T>()
        // {
        //     if (_conn.Trace)
        //     {
        //         Console.WriteLine("Executing Query: " + this);
        //     }

        //     T val = default(T);

        //     var stmt = Prepare();
        //     if (SQLite3.Step(stmt) == SQLite3.Result.Row)
        //     {
        //         var colType = SQLite3.ColumnType(stmt, 0);
        //         val = (T)ReadCol(stmt, 0, colType, typeof(T));
        //     }
        //     Finalize(stmt);

        //     return val;
        // }
    }
}
