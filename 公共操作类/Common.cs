using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 脸滚键盘.公共操作类
{
    class Common
    {
        public  static Thread CreateSplashWindow()
        {
            Thread t = new Thread(() =>
            {
                Gval.Uc.SpWin = new SplashWindow();
                Gval.Uc.SpWin.ShowDialog();//不能用Show
            });
            t.SetApartmentState(ApartmentState.STA);//设置单线程
            t.Start();
            return t;
        }
    }
}
