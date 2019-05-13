using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinForm
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Winform());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }
    }
}
