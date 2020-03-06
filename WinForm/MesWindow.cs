using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestModel;
using TestTool;

namespace WinForm
{
    public partial class MesWindow : Form
    {
        private string path;
        public MesWindow(ConfigData configData,string path)
        {
            InitializeComponent();
            cb_MesEnable.Checked = configData.MesEnable;
            tb_Station.Text = configData.MesStation;
            tb_NowStation.Text = configData.NowStation;
            this.path = path;
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            DataBase dataBase = new DataBase(path);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MesEnable", cb_MesEnable.Checked);
            dic.Add("MesStation", tb_Station.Text.Trim());
            dic.Add("NowStation", tb_NowStation.Text.Trim());
            dataBase.UpdateConfigData(dic);
            this.DialogResult = DialogResult.Yes;
        }
    }
}
