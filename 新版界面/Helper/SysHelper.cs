using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Helper
{
    public class SysHelper
    {
        /// <summary>
        /// 创建结构体用于返回捕获时间
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            /// <summary>
            /// 设置结构体块容量
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            /// <summary>
            /// 抓获的时间
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        /// <summary>
        /// 获取键盘和鼠标没有操作的时间
        /// </summary>
        /// <returns>用户上次使用系统到现在的时间间隔，单位为毫秒</returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo))
            {
                return 0;
            }
            else
            {
                return Environment.TickCount - (long)vLastInputInfo.dwTime;
            }
        }
    }
}
