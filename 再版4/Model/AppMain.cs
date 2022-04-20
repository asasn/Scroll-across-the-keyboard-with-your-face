using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version4.Model
{
    public class AppMain
    {
        public AppMain()
        {

        }
        public static string WindowTitle { get; set; } = "脸滚键盘——优秀的小说创作解决方案";
        public static string PathRoot { get { return Environment.CurrentDirectory; } }
        public static string PathBooks { get { return Environment.CurrentDirectory + "/books"; } }
        public static string PathAssets { get { return Environment.CurrentDirectory + "/Assets"; } }

    }


}
