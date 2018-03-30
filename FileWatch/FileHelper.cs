using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatch
{
    public class FileHelper
    {
        public string FromPath;
        public string ToPath;
        List<string> Files = new List<string>();

        #region 开始监控 
        /// <summary>
        /// 开始监控
        /// </summary> 
        public  void WatcherStrat()
        {
            FromPath = ConfigurationSettings.AppSettings["frompath"];
            ToPath= ConfigurationSettings.AppSettings["toPath"];
            string filter = "*.*";
            WriteMsg("开始监听:" + FromPath + "(" + filter + ")");
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = FromPath;
            watcher.Filter = filter;
            watcher.Changed += Watcher_Changed; ;
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.IncludeSubdirectories = true;
        }

        #endregion

        #region 监控事件

        private  void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (Files.Contains(e.FullPath))
            {
                System.Threading.Thread.Sleep(5*1000);//休眠5000秒
                CopyFile(e.FullPath);
            }
            else
            {
                Files.Add(e.FullPath);
            }

        }

        #endregion

        #region 拷贝文件  
        /// <summary>
        /// 拷贝文件 
        /// </summary>
        public void CopyFile(string filePath)
        {
            if(Directory.Exists(filePath))
            {
                DirectoryInfo di=new DirectoryInfo(filePath);
                var dirs = di.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    CopyFile(dir.FullName);
                }
                var files = di.GetFiles();
                foreach (var file in files)
                {
                    CopyFile(file.FullName);
                }
            }
            else if (File.Exists(filePath))
            {
                try
                {

                    string newFilePath = filePath.Replace(FromPath, ToPath);
                    string[] fileArr = newFilePath.Split('\\');
                    string fileName = fileArr[fileArr.Length - 1];
                    fileArr[fileArr.Length - 1] = DateTime.Now.ToString("yyyy-MM-dd");
                    string newDic = string.Join("\\", fileArr);
                    if (!Directory.Exists(newDic))
                    {
                        Directory.CreateDirectory(newDic);
                    }
                    newFilePath = newDic + "\\" + fileName;
                    File.Move(filePath, newFilePath);
                    WriteMsg("移动成功:" + DateTime.Now.ToString("HH:mm:ss") + "(" + newFilePath + ")", ConsoleColor.Green);

                }
                catch (Exception e)
                {
                    WriteMsg("移动失败:" + DateTime.Now.ToString("HH:mm:ss") + "(" + filePath + ")" + e.Message, ConsoleColor.Red);
                }
            }
            
            
        }

        #endregion

        #region 打印消息

        public static void WriteMsg(string msg,ConsoleColor color= ConsoleColor.White)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }

        #endregion
    }
}
