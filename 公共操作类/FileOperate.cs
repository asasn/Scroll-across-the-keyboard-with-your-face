using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace 脸滚键盘
{
    static class FileOperate
    {

        public static object GetImgObject(string imgPath)
        {
            if (false == FileOperate.IsFileExists(imgPath))
            {
                return "未找到图片";
            }
            BitmapImage img = new BitmapImage();

            img.BeginInit();
            img.UriSource = new Uri(imgPath);
            img.EndInit();

            Image imgShow = new Image() { Source = img };
            return imgShow;
        }

        /// <summary>
        /// 创建新文件夹
        /// </summary>
        /// <param name="srcFolderPath"></param>
        public static void CreateFolder(string srcFolderPath)
        {
            if (false == IsFolderExists(srcFolderPath))
            {
                System.IO.Directory.CreateDirectory(srcFolderPath);
            }
        }

        /// <summary>
        /// 创建新文档
        /// </summary>
        /// <param name="newDocFullName"></param>
        public static void CreateNewDoc(string newDocFullName)
        {
            WriteToTxt(newDocFullName, "　　");
        }

        /// <summary>
        /// 判断文件夹路径是否存在
        /// </summary>
        /// <param name="srcFolderPath"></param>
        /// <returns></returns>
        public static bool IsFolderExists(string srcFolderPath)
        {
            return System.IO.Directory.Exists(srcFolderPath);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static bool IsFileExists(string fullFileName)
        {
            return File.Exists(fullFileName);
        }

        /// <summary>
        /// 文件夹非法字符
        /// </summary>
        public static char[] invalidCharsInFolderName = Path.GetInvalidPathChars();

        /// <summary>
        /// 文件名非法字符
        /// </summary>
        public static char[] invalidCharsInFileName = Path.GetInvalidFileNameChars();

        /// <summary>
        /// 方法：替换非法字符
        /// </summary>
        /// <param name="name">填入的名称</param>
        /// <returns>合法字符</returns>
        public static string ReplaceFileName(string name)
        {
            name = name.Replace("/", "／");
            name = name.Replace("\\", "＼");
            name = name.Replace(":", "：");
            name = name.Replace("*", "※");
            name = name.Replace("?", "？");
            name = name.Replace("\"", "“");
            name = name.Replace("<", "＜");
            name = name.Replace(">", "＞");
            name = name.Replace("|", "│");
            name = name.Trim(new char[] { '.' }); //过滤首尾字符'.'
            return name;
        }

        /// <summary>
        /// 方法：删除文档
        /// </summary>
        /// <param name="fullFileName">完整的文件名</param>
        public static void deleteDoc(string fullFileName)
        {
            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }
        }

        /// <summary>
        /// 方法：删除文件夹
        /// </summary>
        /// <param name="fullFolderName">完整的文件夹名</param>
        public static void deleteDir(string fullFolderName)
        {
            if (System.IO.Directory.Exists(fullFolderName))
            {
                DirectoryInfo di = new DirectoryInfo(fullFolderName);
                di.Delete(true);
            }
        }

        /// <summary>
        /// 方法：文件夹重命名
        /// </summary>
        /// <param name="srcFolderPath">原文件夹</param>
        /// <param name="destFolderPath">新文件夹</param>
        public static void renameDir(string srcFolderPath, string destFolderPath)
        {
            //来源文件夹存在，目标文件夹不存在，且来源和目标文件夹不相同
            if (System.IO.Directory.Exists(srcFolderPath) && !System.IO.Directory.Exists(destFolderPath) && srcFolderPath != destFolderPath)
            {
                System.IO.Directory.Move(srcFolderPath, destFolderPath);
            }
        }

        /// <summary>
        /// 方法：文件重命名
        /// </summary>
        /// <param name="fOld"></param>
        /// <param name="fNew"></param>
        public static void renameDoc(string fOld, string fNew)
        {
            //原文件存在，且改名后的新文件不存在
            if (File.Exists(fOld) && false == File.Exists(fNew))
            {
                File.Move(fOld, fNew);
            }
        }

        /// <summary>
        /// 方法：写入TXT文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="content"></param>
        public static void WriteToTxt(string fullFileName, string content)
        {
            if (string.IsNullOrEmpty(fullFileName))
                return;
            FileStream fs = new FileStream((fullFileName), FileMode.OpenOrCreate, FileAccess.Write);

            if (File.Exists(fullFileName))
            {
                fs.SetLength(0); //先清空文件
            }
            StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(true));
            sw.Write(content);   //写入字符串
            sw.Close();
            Console.WriteLine("保存至：" + fullFileName);
        }

        /// <summary>
        /// 方法：读取TXT文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static string ReadFromTxt(string fullFileName)
        {
            if (false == File.Exists(fullFileName))
                return string.Empty;
            StreamReader sr = new StreamReader(fullFileName, Encoding.UTF8);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
    }
}
