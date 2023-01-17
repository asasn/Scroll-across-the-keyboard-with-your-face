using RootNS.Helper;
using RootNS.View;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using RootNS.Workfolw;
using System.Windows.Automation.Peers;

namespace RootNS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataIn.ReadyForBaseInfo();
        }

        private void WinMain_Loaded(object sender, RoutedEventArgs e)
        {
            if (FunctionPack.IsInDesignMode(this))
            {
                return;
            }
            Gval.View.MainWindow = this;
        }

        private void WinMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Gval.EditorTabControl != null)
            {
                while (Gval.EditorTabControl.Items.Count > 0)//集合可能改变，故而不需要i++之类的条件
                {
                    HandyControl.Controls.TabItem tabItem = Gval.EditorTabControl.Items[Gval.EditorTabControl.Items.Count - 1] as HandyControl.Controls.TabItem;
                    tabItem.Focus();
                    CommandHelper.FindByName(tabItem.CommandBindings, "Close").Execute(tabItem);
                }
            }
            foreach (SqliteHelper cSqlite in SqliteHelper.PoolDict.Values)
            {
                cSqlite.Close();
            }
            if (FindReplaceDialog.theDialog != null)
            {
                FindReplaceDialog.theDialog.Close();
            }
            Application.Current.Shutdown(0);
        }


        private void TabBook_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TabBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gval.SelectedChapterTab = sender as TabControl;
            Gval.CurrentBook.LoadForChapterTab();
        }


        private void TabNote_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TabControl).SelectedIndex = Convert.ToInt32(SettingsHelper.Get(Gval.CurrentBook.Name, Book.SettingKeyName.NoteSelectedIndex.ToString()));
            tabNoteLoadedFlag = true;
            Gval.View.TabNote = sender as TabControl;
        }

        private bool tabNoteLoadedFlag;
        private void TabNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gval.SelectedNoteTab = sender as TabControl;
            Gval.CurrentBook.LoadForNoteTab();
            if (tabNoteLoadedFlag == true)
            {
                SettingsHelper.Set(Gval.CurrentBook.Name, Book.SettingKeyName.NoteSelectedIndex.ToString(), (sender as TabControl).SelectedIndex.ToString());
            }
        }


        private void TabCard_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TabCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gval.SelectedCardTab = sender as TabControl;
            Gval.CurrentBook.LoadForCardTab();
        }

        private void TabMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TabControl).SelectedIndex = Convert.ToInt32(SettingsHelper.Get(Gval.MaterialBook.Name, Material.SettingKeyName.MaterialSelectedIndex.ToString()));
            tabMaterialLoadedFlag = true;
        }

        private bool tabMaterialLoadedFlag;
        private void TabMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gval.SelectedMaterialTab = sender as TabControl;
            Gval.MaterialBook.LoadForMaterialTab();
            if (tabMaterialLoadedFlag == true)
            {
                SettingsHelper.Set(Gval.MaterialBook.Name, Material.SettingKeyName.MaterialSelectedIndex.ToString(), (sender as TabControl).SelectedIndex.ToString());
            }
        }



        private void TabPublicCard_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TabPublicCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gval.SelectedPublicCardTab = sender as TabControl;
            Gval.MaterialBook.LoadForPublicCardTab();
        }

        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Choose();
            win.ShowDialog();
        }

        /// <summary>
        /// 内容渲染完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinMain_ContentRendered(object sender, EventArgs e)
        {
            Gval.FlagLoadingCompleted = true;
        }
        private void UcShower_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.View.UcShower = sender as UcShower;
            Gval.View.UcShower.DataContext = new Shower();
        }

        private void BtnNameer_Click(object sender, RoutedEventArgs e)
        {
            NameerWindow win = new NameerWindow();
            win.ShowDialog();
        }
        private void BtnMap_Click(object sender, RoutedEventArgs e)
        {
            MapWindow win = new MapWindow();
            win.ShowDialog();
        }

        private void BtnHans_Click(object sender, RoutedEventArgs e)
        {
            HansWindow win = new HansWindow();
            win.ShowDialog();
        }

        private void BtnDBManger_Click(object sender, RoutedEventArgs e)
        {
            DBMangerWindow win = new DBMangerWindow();
            Workfolw.ViewSet.ForViewPointX(win, sender as Button, -(win.Width / 2));
            Workfolw.ViewSet.ForViewPointY(win, sender as Button, 50);
            win.ShowDialog();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            VersionWindow win = new VersionWindow();
            Workfolw.ViewSet.ForViewPointX(win, sender as Button, -(win.Width / 2));
            Workfolw.ViewSet.ForViewPointY(win, sender as Button, 50);
            win.ShowDialog();
        }

        private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = Gval.Path.App;
            process.Start();
        }

        private void BtnOpenHomepage_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Gval.HomePage.ToString());
        }

        private void UcSearch_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.View.UcSearch = sender as UcSearch;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            string bookContent = String.Empty;
            Gval.CurrentBook.LoadForAllChapterTabs();
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            folder.Description = "选择文件所在文件夹目录";  //提示的文字
            folder.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bookContent += "# 【" + "已发布" + "】" + "\n\n\n";
                foreach (Node dirNodes in Gval.CurrentBook.BoxPublished.ChildNodes)
                {
                    bookContent += "## 【" + dirNodes.Title + "】" + "\n\n\n";
                    foreach (Node node in dirNodes.ChildNodes)
                    {
                        bookContent += node.Title + "\n\n" + node.Text + "\n\n\n";                        
                    }
                }
                bookContent += "# 【" + "草稿箱" + "】" + "\n\n\n";
                foreach (Node node in Gval.CurrentBook.BoxDraft.ChildNodes)
                {
                    bookContent += node.Title + "\n\n" + node.Text + "\n\n\n";
                }
                string fullFileName = String.Format("{0}/{1}.txt", folder.SelectedPath, Gval.CurrentBook.Name);
                int n = 1;
                while (IOHelper.IsFileExists(fullFileName) == true)
                {
                    fullFileName = String.Format("{0}/{1} - {2}.txt", folder.SelectedPath, Gval.CurrentBook.Name, n);
                    n++;
                }
                IOHelper.WriteToTxt(fullFileName, bookContent);
            }
        }


    }
}
