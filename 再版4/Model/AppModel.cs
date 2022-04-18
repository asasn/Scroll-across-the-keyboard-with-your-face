using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version4.Model
{
    public class AppModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        public AppModel()
        {
            this.PropertyChanged += AppModel_PropertyChanged;
            StaticPropertyChanged += AppModel_StaticPropertyChanged;
        }

        private void AppModel_StaticPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }


        private void AppModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public string AppName { get; set; } = "脸滚键盘——优秀的小说创作解决方案";
        public string PathRoot { get { return Environment.CurrentDirectory; } }
        public string PathBooks { get { return Environment.CurrentDirectory + "/books"; } }
        public string PathResourses { get { return Environment.CurrentDirectory + "/Resourses"; } }
        public string PathAssets { get { return Environment.CurrentDirectory + "/Assets"; } }


    }


}
