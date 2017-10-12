using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiaparts.Common.Log
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoggerSupport
    {
        void Debug(String mesasge);

        void Info(String mesasge);

        void Warn(String mesasge);

        void Error(String mesasge);

        void Fatal(String mesasge);

        void Debug(String mesasge, Exception ex);

        void Info(String mesasge, Exception ex);
 
        void Warn(String mesasge, Exception ex);

        void Error(String mesasge, Exception ex);

        void Fatal(String mesasge, Exception ex);
    }
}
