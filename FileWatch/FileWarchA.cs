using System;
using System.IO;

namespace FileWatch
{
    public class FileWarchA:FileWatchBase
    {
         

        #region 拷贝文件  
        /// <summary>
        /// 拷贝文件 
        /// </summary>
        public override void CopyFile(object filePath)
        {
            string filePathStr = filePath.ToString();
            if (File.Exists(filePathStr))
            {
                System.Threading.Thread.Sleep(SleepSecond * 1000);//休眠5000秒
                try
                {

                    string newFilePath = filePathStr.Replace(UpdatePath, AppPath);
                    string[] fileArr = newFilePath.Split('\\');
                    string fileName = fileArr[fileArr.Length - 1];
                    if (IgnoreFiles.Contains(fileArr[fileArr.Length - 1]))
                    {
                        return; //如果是忽略文件,则忽略
                    } 
                    
                    fileArr[fileArr.Length - 1] = DateTime.Now.ToString("yyyy-MM-dd");
                    string newDic = string.Join("\\", fileArr);
                    if (!Directory.Exists(newDic))
                    {
                        Directory.CreateDirectory(newDic);
                    }
                    newFilePath = newDic + "\\" + fileName;
                    string dirPath = Path.GetDirectoryName(filePathStr);
                    if (File.Exists(newFilePath))
                    {
                        File.Move(newFilePath,newFilePath+DateTime.Now.ToString("yyyy_mm_dd_HH_mm_ss")+Guid.NewGuid());
                    }
                    File.Move(filePathStr, newFilePath);
                    WriteMsg("移动成功:" + DateTime.Now.ToString("HH:mm:ss") + "(" + newFilePath + ")", ConsoleColor.Green);
                    DirectoryInfo di=new DirectoryInfo(dirPath);
                    if(di.GetDirectories().Length==0&&di.GetFiles().Length==0)
                    {
                        Directory.Delete(dirPath);
                    }
                }
                catch (Exception e)
                {
                    WriteMsg("移动失败:" + DateTime.Now.ToString("HH:mm:ss") + "(" + filePathStr + ")" + e.ToString(), ConsoleColor.Red);
                }
                finally
                {
                    this.Files.Remove(filePathStr);
                }
            }
            
            
        }

        #endregion 
    }
}
