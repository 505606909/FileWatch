using System;
using System.IO;

namespace FileWatch
{
    public class FileWarchB:FileWatchBase
    { 
        #region 拷贝文件  
        /// <summary>
        /// 文件部署 
        /// </summary>
        public override void CopyFile(object filePath)
        {
            string filePathStr= filePath.ToString();
            if (File.Exists(filePathStr))
            {
                System.Threading.Thread.Sleep(SleepSecond * 10);//休眠5000秒
                try
                {

                    string newFilePath = filePathStr.Replace(UpdatePath, AppPath);
                    string[] fileArr = newFilePath.Split('\\');
                    string fileName = fileArr[fileArr.Length - 1];
                    if (IgnoreFiles.Contains(fileArr[fileArr.Length - 1]))
                    {
                        return; //如果是忽略文件,则忽略
                    }
                    fileArr[fileArr.Length - 1] = "";
                    string newDic = string.Join("\\", fileArr);
                    if (!Directory.Exists(newDic))
                    {
                        Directory.CreateDirectory(newDic);
                    }
                    newFilePath = newDic + "\\" + fileName;


                    if (File.Exists(newFilePath))
                    {
                        /*备份到指定目录,并删除此文件*/
                        BackUpFile(newFilePath);
                        File.Delete(newFilePath);
                    }

                    try
                    { 
                        File.Move(filePathStr, newFilePath);
                    }
                    catch (Exception e)
                    { 
                        throw new Exception(string.Format("source:{0},target:{1},{2}",filePathStr,newFilePath,e.Message));
                    }
                    WriteMsg("移动成功:" + DateTime.Now.ToString("HH:mm:ss") + "(" + newFilePath + ")", ConsoleColor.Green);
                   
                }
                catch (Exception e)
                {
                    WriteMsg("移动失败:" + DateTime.Now.ToString("HH:mm:ss") + "(" + filePathStr + ")" + e.ToString(), ConsoleColor.Red);
                }
                finally
                {
                    lock (Files)
                    {
                        DeleteEmptyDic(Path.GetDirectoryName(filePathStr));
                        //this.Files.Remove(filePathStr);
                    }
                    
                }
            }
            
            
        }

        #endregion

        #region 清除空的目录
        /// <summary>
        /// 删除空目录
        /// </summary>
        /// <param name="path"></param>
        private void DeleteEmptyDic(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            string dirPath = path;
            if(path!=this.UpdatePath)
            {
                try
                {
                    if (di.GetDirectories().Length == 0 || di.GetFiles().Length == 0)
                    {
                        Directory.Delete(dirPath);
                        if (di.Parent != null)
                        {
                            DeleteEmptyDic(di.Parent.FullName);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            } 
        }

        #endregion

        #region 备份文件 

        /// <summary>
        /// 备份文件 
        /// </summary>
        public void BackUpFile(string filePath)
        { 
            string newFilePath = BackUpPath+"\\"+ DateTime.Now.ToString("yyyyMMdd")+"\\" +filePath.Replace(AppPath, "") ;
            string dirName = Path.GetDirectoryName(newFilePath);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            if(File.Exists(newFilePath))
            {
                throw new Exception("备份文件已存在,请移走当前的备份文件:"+newFilePath);
            }
            File.Move(filePath, newFilePath);
        }

        #endregion
    }
}
