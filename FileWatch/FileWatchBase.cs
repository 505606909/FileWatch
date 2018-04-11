using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

namespace FileWatch
{
    public abstract class FileWatchBase
    {
        public static object obj=1;
        public string UpdatePath;
        public string AppPath;
        public string BackUpPath;
        public List<string> Files = new List<string>();
        public List<string> IgnoreFiles=new List<string>();
        public int SleepSecond = 0;
        #region 开始监控 
        /// <summary>
        /// 开始监控
        /// </summary> 
        public  void WatcherStrat()
        {
            UpdatePath = ConfigurationSettings.AppSettings["updatePath"];
            AppPath= ConfigurationSettings.AppSettings["AppPath"];
            BackUpPath= ConfigurationSettings.AppSettings["backUpPath"];
            string ignoreStr= ConfigurationSettings.AppSettings["ignoreFile"];
            string internalBufferSize = ConfigurationSettings.AppSettings["InternalBufferSize"]; 
            SleepSecond = int.Parse(ConfigurationSettings.AppSettings["sleepSecond"]);
            if (!string.IsNullOrEmpty(ignoreStr))
            {
                string[] ignoreArr = ignoreStr.Split(',');
                foreach (string s in ignoreArr)
                {
                    IgnoreFiles.Add(s);
                }
            }
            string filter = "*.*";
            WriteMsg("开始监听:" + UpdatePath + "(" + filter + ")");
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = UpdatePath;
            watcher.Filter = filter;
            watcher.Changed += Watcher_Changed;
            watcher.Created += Watcher_Changed;
            watcher.InternalBufferSize = 1024*int.Parse(internalBufferSize);//1kb*
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite|NotifyFilters.Size|NotifyFilters.CreationTime|NotifyFilters.LastAccess|NotifyFilters.DirectoryName|NotifyFilters.LastAccess;
            watcher.IncludeSubdirectories = true;
        }

        #endregion

        #region 监控事件

        private  void Watcher_Changed(object sender, FileSystemEventArgs e)
        { 
            try
            {
                
                if(!Files.Contains(e.FullPath))
                {
                    Files.Add(e.FullPath);
                }
                CopyFile(e.FullPath);
            }
            catch (Exception ex)
            {
                WriteMsg(ex.Message, ConsoleColor.Red);
            }

        }

        #endregion

        #region 拷贝文件  
        /// <summary>
        /// 拷贝文件 
        /// </summary>
        public abstract void CopyFile(object filePathStr);

        #endregion

        #region 打印消息

        public static void WriteMsg(string msg,ConsoleColor color= ConsoleColor.White)
        {
            lock (obj)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ForegroundColor = oldColor;
                obj = 1;

            }
         
        }

        #endregion
    }
}
