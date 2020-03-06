using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Threading;

namespace TestTool
{
  public  class Others
    {
        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32")]
        public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);
        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetF(); //获得本窗体的句柄
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetF(IntPtr hWnd); //设置此窗体为活动窗体

        [DllImport("transferdll.dll",EntryPoint = "dldstart", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern bool dldstart(IntPtr hWnd); //设置此窗体为活动窗体

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

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

        public static string CmdExcute(string cmdStr)
        {
            Process process = new Process();
            StreamReader errorRead = null;
            string error = string.Empty;
            string output = "";

            IntPtr ptr = new IntPtr();
            bool bOS_X64 = System.Environment.Is64BitOperatingSystem;
            if (bOS_X64)
            {
                Wow64DisableWow64FsRedirection(ref ptr);        // 关闭system32文件重定向
            }

            try
            {
                process.StartInfo.FileName = @"cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                if (process.Start())//开始进程
                {
                    process.StandardInput.AutoFlush = true;
                    process.StandardInput.WriteLine(cmdStr);
                    process.StandardInput.WriteLine("exit");
                    Thread.Sleep(100);
                    StreamReader reader = process.StandardOutput;
                    errorRead = process.StandardError;
                    error = errorRead.ReadToEnd();
                    string[] data = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    output = data[3];
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (process != null)
                {
                    process.WaitForExit();
                    process.Close();
                }
            }
            if (bOS_X64)
            {
                Wow64RevertWow64FsRedirection(ptr);               //恢复文件重定向
            }
            if (error == "")
            {
                return output;
            }
            else
            {
                return "Fail";
            }
        }
    }
}
