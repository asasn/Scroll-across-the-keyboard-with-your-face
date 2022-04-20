using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Version4.Model
{
    public class Book : BaseBook
    {
        public Book()
        {
            this.PropertyChanged += Book_PropertyChanged;
            InitRootNodes();
        }


        /// <summary>
        /// 根节点初始化
        /// </summary>
        /// <param name="bookName"></param>
        private void InitRootNodes()
        {
            Chapter[] rootChapters = { this.BoxDraft, this.BoxTemp, this.BoxPublished};
            foreach (Chapter node in rootChapters)
            {
                node.Owner = this;
            }
            Note[] rootNotes = { this.NoteMemorabilia, this.NoteStory, this.NoteScenes, this.NoteClues, this.NoteTemplate };
            foreach (Note note in rootNotes)
            {
                note.Owner = this;
            }
        }

        private void Book_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                string imgPath = AppMain.PathBooks + "/" + Name + ".jpg";
                if (System.IO.File.Exists(imgPath) == true)
                {
                    this.CoverPath = imgPath;
                }
            }
        }

        private double _price;
        /// <summary>
        /// 书籍单价
        /// </summary>
        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                RaisePropertyChanged(nameof(Price));
            }
        }

        private long _currentYear;
        /// <summary>
        /// 书中的当前年份
        /// </summary>
        public long CurrentYear
        {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                RaisePropertyChanged(nameof(CurrentYear));
            }
        }


        private string _coverPath = "../Assets/nullbookface.jpg";
        /// <summary>
        /// 封面路径
        /// </summary>
        public string CoverPath
        {
            get { return _coverPath; }
            set
            {
                _coverPath = value;
                RaisePropertyChanged(nameof(CoverPath));
            }
        }

        private SolidColorBrush? _selectedBrush = Brushes.WhiteSmoke;
        /// <summary>
        /// 选中时的书籍卡片边框颜色
        /// </summary>
        public SolidColorBrush? SelectedBrush
        {
            get { return _selectedBrush; }
            set
            {
                _selectedBrush = value;
                RaisePropertyChanged(nameof(SelectedBrush));
            }
        }


        /// <summary>
        /// 目录树TabItem标志
        /// </summary>
        public enum ChapterTabName
        {
            草稿,
            暂存,
            已发布,
        }

        /// <summary>
        /// 记事板TabItem标志标志
        /// </summary>
        public enum NoteTabName
        {
            大事记,
            故事,
            场景,
            线索,
            模板,
        }

        #region 目录树
        public Chapter BoxDraft { set; get; } = new Chapter() { TabName = ChapterTabName.草稿.ToString() };
        public Chapter BoxTemp { set; get; } = new Chapter() { TabName = ChapterTabName.暂存.ToString() };
        public Chapter BoxPublished { set; get; } = new Chapter() { TabName = ChapterTabName.已发布.ToString() };
        #endregion

        #region 记事板
        public Note NoteMemorabilia { set; get; } = new Note() { TabName = NoteTabName.大事记.ToString() };
        public Note NoteStory { set; get; } = new Note() { TabName = NoteTabName.故事.ToString() };
        public Note NoteScenes { get; set; } = new Note() { TabName = NoteTabName.场景.ToString() };
        public Note NoteClues { set; get; } = new Note() { TabName = NoteTabName.线索.ToString() };
        public Note NoteTemplate { set; get; } = new Note() { TabName = NoteTabName.模板.ToString() };
        #endregion
    }
}
