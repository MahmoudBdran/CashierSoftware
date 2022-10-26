using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Windows.Input;
using CashierSoftware.SellScreenPackage;
using System.Windows;

namespace CashierSoftware
{
    public class Click
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy,
                              int dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

      
        public void leftClick(int x, int y)
        {
         
            mouse_event((int)(MouseEventFlags.LEFTDOWN), x, y, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), x, y, 0, 0);
        }

        public void rightClick(int x , int y)
        {
           
            mouse_event((int)(MouseEventFlags.RIGHTDOWN), x, y, 0, 0);
            mouse_event((int)(MouseEventFlags.RIGHTUP), x, y, 0, 0);
        }

       
    }

}
