using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinForm
{
    public partial class frmSettingLogin : Form
    {
        public string Function;

        public frmSettingLogin()
        {
            InitializeComponent();
        }

        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (Function == "Radio")
            {
                if (txt_password.Text == "123")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.lbl_status.Text = "密码错误";
                    this.txt_password.Text = "";
                }
            }
            else if(Function == "MES")
            {
                if (txt_password.Text == "Engineer123")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.lbl_status.Text = "密码错误";
                    this.txt_password.Text = "";
                }
            }
            else
            {
                if (txt_password.Text == "123.abc")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.lbl_status.Text = "密码错误";
                    this.txt_password.Text = "";
                }
            }
        }
    }
}
