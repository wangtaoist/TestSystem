﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;

namespace TestTool
{
  public  class Others
    {
        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public static void setResolution(double newx, double newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                double a = Convert.ToSingle(mytag[0]) * newx;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newy;
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;
                con.Top = (int)(a);
                double currentSize = Convert.ToSingle(mytag[4]) * Math.Min(newx, newy);
                con.Font = new Font(con.Font.Name, (float)currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setResolution(newx, newy, con);
                }
            }
        }

        public static void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }

        public static bool isWin10()
        {
            var info = System.Environment.OSVersion;
            if(info.Version.Major == 6 && info.Version.Minor == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}