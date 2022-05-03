using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Site:Node
    {
        private double _pointX;

        public double PointX
        {
            get { return _pointX; }
            set
            {
                _pointX = value;
                this.RaisePropertyChanged("PointX");
            }
        }

        private double _pointY;

        public double PointY
        {
            get { return _pointY; }
            set
            {
                _pointY = value;
                this.RaisePropertyChanged("PointY");
            }
        }
    }
}
