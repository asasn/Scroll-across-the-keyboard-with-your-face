using RootNS.Brick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RootNS.Model
{
    public static class Gval
    {
        /// <summary>
        /// 程序路径
        /// </summary>
        public struct Path
        {
            public static string App { get { return Environment.CurrentDirectory; } }

            public static string Books { get { return Environment.CurrentDirectory + "/books"; } }

            public static string Resourses { get { return Environment.CurrentDirectory + "/Resourses"; } }
        }

        public static Book CurrentBook { get; set; } = new Book();
        public static Material Material { get; set; } = new Material();

        public static Dictionary<string, CSqlitePlus> SqlitePool { get; set; } = new Dictionary<string, CSqlitePlus>();

        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public struct PoolOperate
        {
            /// <summary>
            /// 检校字典中是否存在数据库对象，如果不存在则添加
            /// </summary>
            /// <param name="dbPath"></param>
            /// <param name="dbName"></param>
            public static void Add(string dbPath, string dbName)
            {
                if (SqlitePool.ContainsKey(dbName) == true)
                {
                    return;
                }
                else
                {
                    SqlitePool.Add(dbName, new CSqlitePlus(Gval.Path.Books, dbName + ".db"));
                }
            }

            /// <summary>
            /// 关闭数据库连接并且从字典中删除
            /// </summary>
            /// <param name="keyName"></param>
            public static void Remove(string keyName)
            {
                //关闭数据库连接并从字典中删除
                if (SqlitePool.ContainsKey(keyName) == true)
                {
                    SqlitePool[keyName].Close();
                    SqlitePool.Remove(keyName);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 打开文档的集合
        /// </summary>
        public static ObservableCollection<Node> OpenedDocList { set; get; } = new ObservableCollection<Node>();

        public static HandyControl.Controls.TabControl EditorTabControl;
    }
}
