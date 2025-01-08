using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Serilog;

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

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
           IntPtr wParam, IntPtr lParam);
        public static IntPtr handle;

        public static void InitOthers(string version)
        {
            Log.Logger = new LoggerConfiguration()
           .WriteTo.File(string.Format("CommLog\\log_{0}.txt", DateTime.Now.ToString("yyyyMMdd"))
           , outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
           .CreateLogger();
            WriteInformationLog(string.Format("软件版本：{0}，程序启动", version));
        }

        public static void WriteInformationLog(string data)
        {
            //Thread.Sleep(500);
            Log.Information(data);
        }

        public static void WriteErrorLog(string data)
        {
            //Thread.Sleep(50);
            Log.Error(data);
        }

        public static void WriteWarningLog(string data)
        {
            //Thread.Sleep(50);
            Log.Warning(data);
        }

        private static object obj = new object();

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
            //var info = System.Environment.OSVersion;
            //if(info.Version.Major == 6 && info.Version.Minor == 2)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return false;
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

        public static void WriteTestLog(string log)
        {
            string logPath = Path.Combine(Application.StartupPath, "CommLog");
            string logName = Path.Combine(logPath,
                DateTime.Now.ToString("yyyyMMdd") + ".txt");
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
           
            lock (obj)
            {
                using (FileStream fs = new FileStream(logName, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        string data = string.Format("{0}--->{1}",
                            DateTime.Now.ToString("HH:mm:ss:fff"), log.Trim());
                        sw.WriteLine(data);
                    }
                }
            }
        }

        public static void MaxVolume()
        {
            for (int i = 0; i < 50; i++)
            {
                SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_UP);
            }
        }

        public static byte[] DecodeZero(byte[] packet)
        {
            var i = packet.Length - 1;
            while (packet[i] == 0)
            {
                --i;
            }
            var temp = new byte[i + 1];
            Array.Copy(packet, temp, i + 1);
            return temp;
        }

        public static string[] DecodeZero(string[] packet)
        {
            var i = packet.Length - 1;
            while (packet[i] == "00" || packet[i] == "\0")
            {
                --i;
            }
            var temp = new string[i + 1];
            Array.Copy(packet, temp, i + 1);
            return temp;
        }



    }

    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolume
    {
        int _0(); int _1(); int _2(); int _3();
        int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);
        int _5();
        int GetMasterVolumeLevelScalar(out float pfLevel);
        int _7(); int _8(); int _9(); int _10(); int _11(); int _12();
    }
    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDevice
    {
        int Activate(ref System.Guid id, int clsCtx, int activationParams, out IAudioEndpointVolume aev);
    }
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceEnumerator
    {
        int _0();
        int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice endpoint);
    }
    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")] class MMDeviceEnumeratorComObject { }
    public class Audio
    {
        private static readonly IAudioEndpointVolume _MMVolume;
        static Audio()
        {
            var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
            enumerator.GetDefaultAudioEndpoint(0, 1, out IMMDevice dev);
            var aevGuid = typeof(IAudioEndpointVolume).GUID;
            dev.Activate(ref aevGuid, 1, 0, out _MMVolume);
        }
        public static int Volume
        {
            get
            {
                _MMVolume.GetMasterVolumeLevelScalar(out float level);
                return (int)(level * 100);
            }
            set
            {
                _MMVolume.SetMasterVolumeLevelScalar((float)value / 100, new Guid());
            }
        }
    }
}
