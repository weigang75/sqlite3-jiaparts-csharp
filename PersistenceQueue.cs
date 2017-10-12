using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Jiaparts.Common.Utilities;
using System.Data.SQLite;
using Jiaparts.Common.Log;

namespace Jiaparts.LocalStorage
{
    /// <summary>
    /// 持久化队列(通过在同一个事务中批量处理持久化操作能提供性能)；
    /// 注意：该操作是随机执行事务的，所以不能保证2个连在一起操作会同一个事务。
    /// </summary>
    public class PersistenceQueue
    {
        private Queue messageQueue = null;

        private WaitCall waitCall = null;

        private SQLite.SQLiteConn conn;

        /// <summary>
        /// dueTime、timeout可以理解为持久化的时间在dueTime - timeout毫秒之间运行
        /// </summary>
        /// <param name="database"></param>
        /// <param name="dueTime"></param>
        /// <param name="timeout"></param>
        public PersistenceQueue(SQLite.SQLiteConn database, int dueTime, int timeout)
        {
            if (database == null)
                throw new ArgumentNullException("database");

            messageQueue = Queue.Synchronized(new Queue());
            waitCall = new WaitCall(BeginPersistence, dueTime, timeout);
            conn = database;
        }

        /// <summary>
        /// 添加持久化工作单元到队列中。
        /// </summary>
        /// <param name="workItem"></param>
        public void Enqueue(PersistenceWorkItem workItem)
        {
            messageQueue.Enqueue(workItem);
            waitCall.Run();
        }

        /// <summary>
        /// 开始执行持久化
        /// </summary>
        private void BeginPersistence()
        {
            lock (messageQueue.SyncRoot)
            {
                if (messageQueue.Count == 0)
                    return;
                // 返回位于 Queue 开始处的对象，不移除（试探性获取是否存在）。
                var obj = messageQueue.Peek();
                if (obj != null)
                {
                    bool success = true;
                    conn.BeginTransaction();
                    while (success)
                    {
                        success = DequeueAndExecute();
                    }
                    conn.Commit();
                }
            }
        }

        /// <summary>
        /// 移除并返回位于 Queue 开始处的对象。
        /// 并执行。
        /// </summary>
        /// <returns></returns>
        private bool DequeueAndExecute()
        {
            if (messageQueue.Count == 0)
                return false;

            PersistenceWorkItem workItem = messageQueue.Dequeue() as PersistenceWorkItem;
            try
            {
                workItem.Action(workItem.State);
            }
            catch (Exception ex)
            {
                Logger.Error("DequeueAndExecute -> workItem.Action(workItem.State)", ex);
            }

            if (workItem.Callback != null)
            {
                try
                {
                    workItem.Callback(workItem.State);
                }
                catch (Exception ex)
                {
                    Logger.Error("DequeueAndExecute -> workItem.Callback(workItem.State)", ex);
                }                
            }
            return true;
        }
    }

    /// <summary>
    /// 持久化工作单元
    /// </summary>
    public class PersistenceWorkItem
    {
        public PersistenceWorkItem(Action<object> action, object state)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            Action = action;
            State = state;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        /// <param name="callback"></param>
        public PersistenceWorkItem(Action<object> action, object state, Action<object> callback)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            
            if (callback == null)
                throw new ArgumentNullException("callback");

            Action = action;
            State = state;
            Callback = callback;
        }
        /// <summary>
        /// 执行完后回调
        /// </summary>
        public Action<object> Callback { get; set; }
        /// <summary>
        /// 执行活动
        /// </summary>
        public Action<object> Action { get; set; }
        /// <summary>
        /// 传入状态
        /// </summary>
        public object State { get; set; }
    }
}
