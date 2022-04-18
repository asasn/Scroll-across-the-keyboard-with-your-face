using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version4.Model;

namespace Version4.Workflow
{
    public class VmMainWindow :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        public VmMainWindow()
        {
            WindowTitle = MyApp.AppName;
        }

        public AppModel MyApp { get; set; } = new AppModel();

        private string? _windowTitle;

        public string? WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowTitle)));
            }
        }

        private static string? myVar;

        public static string? MyStaticProperty
        {
            get { return myVar; }
            set
            {
                myVar = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(MyStaticProperty)));
            }
        }

    }
}
