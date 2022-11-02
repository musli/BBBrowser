using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 本地日志纪录类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="text">日志内容</param>
        public static void WriteLog(string text)
        {
            try
            {

                string FolderName1 = DateTime.Now.ToString("yyyy年");
                string FolderName2 = DateTime.Now.ToString("yyyy年MM月");
                string FileName = DateTime.Now.ToString("yyyy年MM月dd日");
                //文件夹路径
                string strPath = AppDomain.CurrentDomain.BaseDirectory + @"Temp\Log";
                //文件路径
                string strFilePath = strPath + @"\" + FileName + ".ini";

                if (!File.Exists(strPath))//判断文件夹是否存在
                {
                    Directory.CreateDirectory(strPath);
                    if (!File.Exists(strFilePath))//判断文件是否存在
                    {
                        File.Create(strFilePath).Close();
                    }
                }

                List<string> listConfig = new List<string>();
                listConfig.Add($"{ DateTime.Now.ToString("HH:mm:ss")}：{text}");
                File.AppendAllLines(strFilePath, listConfig.ToArray(), Encoding.UTF8);//写入日志

                CheckDeleteLog();
            }
            catch (Exception)
            {
                // throw ErrorLogEx;
            }
        }
        /// <summary>
        /// 检查删除过于久远的日志文件
        /// </summary>
        public static void CheckDeleteLog()
        {
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"Temp\Log");
            var files = dir.GetFiles();
            //保留后30个文件
            if (files.Length > 30)
                for (int i = 0; i < files.Length - 30; i++)
                    files[i].Delete();

        }
    }
}
