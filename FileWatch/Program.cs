 

 
using System;
using System.Configuration;

namespace FileWatch
{
    class Program
    { 
         
        static void Main(string[] args)
        {
            string workType = ConfigurationSettings.AppSettings["WorkType"];
            FileWatchBase file=null;
            switch (workType.ToUpper())
            {
                case "A":/*文件移动到指定目录当前日期文件夹下*/
                    file=new FileWarchA();
                    break;

                case "B":/*文件移动到指定目录,并把指定目录文件备份到当前日期文件*/
                    file = new FileWarchB();
                    break;

                default:
                    FileWatchBase.WriteMsg("未定义的工作类型:"+workType, ConsoleColor.Red);
                    break;
                    
            }
            FileWatchBase.WriteMsg("工作模式:" + workType, ConsoleColor.Blue);
            if (file!=null)
            { 
                file.WatcherStrat();
            }
            Console.ReadKey();
        } 

        
         
    }
}
