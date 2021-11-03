using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 脸滚键盘.公共操作类
{
    class SettingsOperate
    {
        public static string Get(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            return value;
        }

        public static void Set(string key, string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }
    }
}
