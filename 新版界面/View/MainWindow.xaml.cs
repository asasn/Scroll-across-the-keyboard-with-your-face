﻿using RootNS.Helper;
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
            (sender as TabControl).SelectedIndex = Convert.ToInt32(SettingsHelper.Get(Gval.CurrentBook.Name, "NoteSelectedIndex"));
            tabNoteLoadedFlag = true;
        }

        private bool tabNoteLoadedFlag;
        private void TabNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gval.SelectedNoteTab = sender as TabControl;
            Gval.CurrentBook.LoadForNoteTab();
            if (tabNoteLoadedFlag == true)
            {
                SettingsHelper.Set(Gval.CurrentBook.Name, "NoteSelectedIndex", (sender as TabControl).SelectedIndex.ToString());
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
            (sender as TabControl).SelectedIndex = Convert.ToInt32(SettingsHelper.Get(Gval.MaterialBook.Name, "MaterialSelectedIndex"));
            tabMaterialLoadedFlag = true;
        }

        private bool tabMaterialLoadedFlag;
        private void TabMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gval.SelectedMaterialTab = sender as TabControl;
            Gval.MaterialBook.LoadForMaterialTab();
            if (tabMaterialLoadedFlag == true)
            {
                SettingsHelper.Set(Gval.MaterialBook.Name, "MaterialSelectedIndex", (sender as TabControl).SelectedIndex.ToString());
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

        private void BtnCardModel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UcShower_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.View.UcShower = sender as UcShower;
        }
    }
}
