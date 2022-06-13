using RootNS.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RootNS.Model
{
    public class Pomodoro : NotificationObject
    {
        public Pomodoro()
        {
            Loaded();
        }

        private void Loaded()
        {
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            Timer.Tick += TimeRuner;
            ShowTimeText = String.Format("{0:D2}:{1:D2}", (int)StopWatch.Elapsed.TotalMinutes, StopWatch.Elapsed.Seconds);
        }


        #region 番茄时间
        private string _showTimeText;

        public string ShowTimeText
        {
            get { return _showTimeText; }
            set
            {
                _showTimeText = value;
                RaisePropertyChanged(nameof(ShowTimeText));
            }
        }

        Stopwatch StopWatch = new Stopwatch();
        DispatcherTimer Timer = new DispatcherTimer();
        bool TagChange = false;

        private bool _isSetting;

        public bool IsSetting
        {
            get { return _isSetting; }
            set
            {
                _isSetting = value;
                RaisePropertyChanged(nameof(IsSetting));
            }
        }

        private object _buttonContent = "\ue87e";

        public object ButtonContent
        {
            get { return _buttonContent; }
            set
            {
                _buttonContent = value;
                RaisePropertyChanged(nameof(ButtonContent));
            }
        }

        public MediaElement MeDida { get; set; }
        public MediaElement MeRing { get; set; }

        private double _timeSetValue = 25;

        public double TimeSetValue
        {
            get { return _timeSetValue; }
            set
            {
                _timeSetValue = value;
                RaisePropertyChanged(nameof(TimeSetValue));
            }
        }




        public void Start()
        {
            if (Timer.IsEnabled)
            {
                ButtonContent = "\ue87e";
                Stop();
            }
            else
            {
                ButtonContent = "\ue880";
                if (TagChange == true)
                {
                    string key = this.GetType().Name + nameof(TimeSetValue);
                    SettingsHelper.Set(Gval.MaterialBook.Name, key, TimeSetValue);
                    TagChange = false;
                }

                IsSetting = false;
                MeDida.Stop();
                MeRing.Stop();
                Timer.Start();
                StopWatch.Start();
            }
        }

        /// <summary>
        /// 方法：每次间隔运行的内容
        /// </summary>
        private void TimeRuner(object sender, EventArgs e)
        {
            MeDida.Stop();
            MeDida.Play();

            if ((int)StopWatch.Elapsed.TotalMinutes >= TimeSetValue)
            {
                MeRing.Play();
                Start();
            }
            ShowTimeText = String.Format("{0:D2}:{1:D2}", (int)StopWatch.Elapsed.TotalMinutes, StopWatch.Elapsed.Seconds);
        }

        public void Stop()
        {
            StopWatch = new Stopwatch();
            Timer.Stop();
            ShowTimeText = String.Format("{0:D2}:{1:D2}", (int)StopWatch.Elapsed.TotalMinutes, StopWatch.Elapsed.Seconds);
        }

        public void Pause()
        {
            StopWatch.Stop();
            Timer.Stop();
            ShowTimeText = String.Format("{0:D2}:{1:D2}", (int)StopWatch.Elapsed.TotalMinutes, StopWatch.Elapsed.Seconds);
        }

        /// <summary>
        /// 事件：时间设置值更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CbTime_ValueChanged()
        {
            TagChange = true;
        }

        /// <summary>
        /// 事件：时间设置值载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CbTime_Loaded()
        {
            string key = this.GetType().Name + nameof(TimeSetValue);
            object value = SettingsHelper.Get(Gval.MaterialBook.Name, key);
            if (value != null && value.ToString() != "0")
            {
                TimeSetValue = Convert.ToDouble(value);
            }
        }


        public void MouseRightButtonDown()
        {
            IsSetting = !IsSetting;
        }

        #endregion

    }
}
