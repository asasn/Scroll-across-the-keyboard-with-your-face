using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMAPP.Commands
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunc == null)
            {
                return true;
            }
            return this.CanExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            if (ExecutAction == null)
            {
                return;
            }
            this.ExecutAction(parameter);
        }

        public Action<object> ExecutAction { get; set; }
        public Func<object, bool> CanExecuteFunc { get; set; }
    }
}
