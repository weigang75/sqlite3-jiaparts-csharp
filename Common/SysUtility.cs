using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Security.Principal;

namespace Jiaparts.Common.Utilities
{
    public class SysUtility
    {
        /// <summary>
        /// 根据网卡类型来获取mac地址
        /// </summary>
        /// <param name="networkType">网卡类型</param>
        /// <param name="macAddressFormatHanlder">格式化获取到的mac地址</param>
        /// <returns>获取到的mac地址</returns>
        public static string GetMacAddress(NetworkInterfaceType networkType, Func<string, string> macAddressFormatHanlder)
        {
            string _mac = string.Empty;
            NetworkInterface[] _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in _networkInterfaces)
            {
                if (adapter.NetworkInterfaceType == networkType)
                {
                    _mac = adapter.GetPhysicalAddress().ToString();
                    if (!String.IsNullOrEmpty(_mac))
                        break;
                }
            }
            if (macAddressFormatHanlder != null)
                _mac = macAddressFormatHanlder(_mac);
            return _mac;
        }
        /// <summary>
        /// 根据网卡类型以及网卡状态获取mac地址
        /// </summary>
        /// <param name="networkType">网卡类型</param>
        /// <param name="status">网卡状态</param>
        ///Up 网络接口已运行，可以传输数据包。 
        ///Down 网络接口无法传输数据包。 
        ///Testing 网络接口正在运行测试。 
        ///Unknown 网络接口的状态未知。 
        ///Dormant 网络接口不处于传输数据包的状态；它正等待外部事件。 
        ///NotPresent 由于缺少组件（通常为硬件组件），网络接口无法传输数据包。 
        ///LowerLayerDown 网络接口无法传输数据包，因为它运行在一个或多个其他接口之上，而这些“低层”接口中至少有一个已关闭。 
        /// <param name="macAddressFormatHanlder">格式化获取到的mac地址</param>
        /// <returns>获取到的mac地址</returns>
        public static string GetMacAddress(NetworkInterfaceType networkType, OperationalStatus status, Func<string, string> macAddressFormatHanlder)
        {
            string _mac = string.Empty;
            NetworkInterface[] _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in _networkInterfaces)
            {
                if (adapter.NetworkInterfaceType == networkType)
                {
                    if (adapter.OperationalStatus != status) continue;
                    _mac = adapter.GetPhysicalAddress().ToString();
                    if (!String.IsNullOrEmpty(_mac)) break;
                }
            }
            if (macAddressFormatHanlder != null)
                _mac = macAddressFormatHanlder(_mac);
            return _mac;
        }
        /// <summary>
        /// 获取读到的第一个mac地址
        /// </summary>
        /// <returns>获取到的mac地址</returns>
        public static string GetMacAddress(Func<string, string> macAddressFormatHanlder)
        {
            string _mac = string.Empty;
            NetworkInterface[] _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in _networkInterfaces)
            {
                _mac = adapter.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(_mac))
                    break;
            }
            if (macAddressFormatHanlder != null)
                _mac = macAddressFormatHanlder(_mac);
            return _mac;
        }
        /// <summary>
        /// 获取本机IP
        /// 仅限ipv4
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIps()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in localIPs)
            {
                //根据AddressFamily判断是否为ipv4,如果是InterNetWork则为ipv6 
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "";
        }

        /// <summary>
        /// 解析文件（文件夹）路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ResolvePath(string path)
        {
            return Path.Combine(Environment.CurrentDirectory, path.TrimStart('\\'));
        }

        /// <summary>
        /// 获取临时目录的位置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string TemporaryPath(string path)
        {
            string tempPath = Path.Combine(Environment.CurrentDirectory, "temp");
            if (String.IsNullOrEmpty(path))
                return tempPath;
            else
                return Path.Combine(tempPath, path);
        }

        /// <summary>
        /// 获取接收文件的位置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string RecvFilePath(string path)
        {
            return TemporaryPath("recvfiles\\" + path.Trim('\\'));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="recvTicks"></param>
        /// <returns></returns>
        public static string GetFilePathByOssKey(string fileKey, long recvTicks)
        {
            if (String.IsNullOrEmpty(fileKey))
                return null;

            DateTime dt = new DateTime(recvTicks);
            string key = fileKey.Replace("/", "_").Replace("\\", "_");

            return TemporaryPath(String.Format("{0}\\{1}", dt.ToString("yyyyMM"), key));
        }

        /// <summary>
        /// 获取接收文件的位置(加入时间戳)
        /// </summary>
        /// <param name="format">时间格式:yyyyMMdd</param>
        /// <returns></returns>
        private static string RecvFileTimestampPath(string format)
        {
            if (String.IsNullOrEmpty(format))
                format = "yyyyMMdd";

            return RecvFilePath(DateTimeUtil.CurrentTime.ToString(format));
        }

        /// <summary>
        /// 获取接收文件的位置(加入时间戳)，时间格式默认为：时间格式:yyyyMMdd
        /// </summary>
        /// <returns></returns>
        private static string RecvFileTimestampPath()
        {
            return RecvFileTimestampPath(null);
        }

        /// <summary>
        /// 获取发送文件的位置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string SentFilePath(string path)
        {
            return TemporaryPath("sentfiles\\" + path.Trim('\\'));
        }

        /// <summary>
        /// 获取发送文件的位置(加入时间戳)
        /// </summary>
        /// <param name="format">时间格式:yyyyMMdd</param>
        /// <returns></returns>
        public static string SentFileTimestampPath(string format)
        {
            if (String.IsNullOrEmpty(format))
                format = "yyyyMMdd";

            return SentFilePath(DateTimeUtil.CurrentTime.ToString(format));
        }
        
        /// <summary>
        /// 获取发送文件的位置(加入时间戳)，时间格式默认为：时间格式:yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string SentFileTimestampPath()
        {
            return SentFileTimestampPath(null);
        }

        /// <summary>
        /// 安全创建路径
        /// </summary>
        /// <param name="dirPath"></param>
        public static void SafeCreateDirectory(string dirPath)
        {
            if (!System.IO.Directory.Exists(dirPath))
                System.IO.Directory.CreateDirectory(dirPath);
        }

        /// <summary>
        /// 安全创建文件的父路径
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        public static void SafeCreateFileDirectory(string filePath)
        {
            string dir = System.IO.Path.GetDirectoryName(filePath);

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// 操作系统类型，PC为3
        /// </summary>
        public const string OS_TYPE = "3";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSysHeaderInfo()
        {

            //获取系统信息
            System.OperatingSystem osInfo = System.Environment.OSVersion;
            string mac = SysUtility.GetMacAddress(null);
            ////获取操作系统ID  此处（int）强行转换
            //System.PlatformID platformID = osInfo.Platform;

            ////获取主版本号
            //int versionMajor = osInfo.Version.Major;

            ////获取副版本号
            //int versionMinor = osInfo.Version.Minor;
            //                   品牌 | 型号    |1| 唯一吗       | 操作系统| 版本|1|586|
            // request.header = "Honor|Che1-CL10|1|A00000555F1CA3|Android4.4.4|2.1.1|1|586|";
            // 手机品牌|手机型号|操作系统类型|设备唯一标示|系统版本号| app版本号| app类型|用户ID|
            string header = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|",
                "Microsoft", // 品牌
                osInfo.VersionString, // 型号
                OS_TYPE, // 操作系统类型，PC为3
                mac, // 唯一码
                osInfo.Platform.ToString(), // 操作系统
                osInfo.Version.ToString(), // 版本
                "1",
                "0"
                );

            //header = "Honor|Che1-CL10|1|A00000555F1CA3|Android4.4.4|2.1.1|1|586|";

            return header;
        }

        public static bool SafeCopyFile(string sourceFileName, string destFileName)
        {
            try
            {
                SafeCreateFileDirectory(destFileName);

                File.Copy(sourceFileName, destFileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        private static Dictionary<string, FileStream> lockFiles = new Dictionary<string, FileStream>();

        /// <summary>
        /// 登录成功后加锁，防止重复登录
        /// </summary>
        /// <param name="key"></param>
        public static bool LockKey(string key)
        {
            return true; // 默认不加锁
            //String lockFile = Path.Combine(TemporaryPath("lock"), key);

            //SafeCreateFileDirectory(lockFile);

            //try
            //{
            //    FileStream fs = File.Open(lockFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //    if (lockFiles.ContainsKey(key))
            //        lockFiles[key] = fs;
            //    else
            //        lockFiles.Add(key, fs);

            //    return true;
            //}
            //catch //(Exception ex)
            //{
            //    return false;
            //}            
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key"></param>
        public static void UnlockKey(string key)
        {
            if (lockFiles.ContainsKey(key))
            {
                try
                {
                    lockFiles[key].Close();

                }
                catch //(Exception ex)
                {

                }
                finally 
                {
                    lockFiles.Remove(key);
                }
            }
        }

        ///// <summary>
        ///// 单例运行模式
        ///// </summary>
        ///// <returns></returns>
        //public static Process SingletonRun()
        //{
        //    Process OtherProcess = WinApis.RunningInstance();

        //    if (OtherProcess != null)
        //    {
        //        MessageBox.Show("该应用程序已经正在运行。");
        //        WinApis.ShowWindowAsync(OtherProcess.MainWindowHandle, WinApis.WS_SHOWNORMAL);
        //        WinApis.SetForegroundWindow(OtherProcess.MainWindowHandle); //把打开的程序放到最前面
        //    }

        //    return OtherProcess;
        //}


        /// <summary>
        /// 判断程序是否是管理员权限运行
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 缩略图目录名称
        /// </summary>
        public const string ThumbnailName = "_thumbnail";
        //private static String Detect32or64Result = null;
        // 64位 32位
        //public static string Detect32or64()
        //{
        //    try
        //    {
        //        if (String.IsNullOrEmpty(Detect32or64Result))
        //        {
        //            string addressWidth = String.Empty;
        //            ConnectionOptions mConnOption = new ConnectionOptions();
        //            ManagementScope mMs = new ManagementScope("\\\\localhost", mConnOption);
        //            ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
        //            ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
        //            ManagementObjectCollection mObjectCollection = mSearcher.Get();
        //            foreach (ManagementObject mObject in mObjectCollection)
        //            {
        //                addressWidth = mObject["AddressWidth"].ToString();
        //            }
        //            Detect32or64Result = addressWidth;
        //        }

        //        return Detect32or64Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return String.Empty;
        //    }
        //}
    }
}
