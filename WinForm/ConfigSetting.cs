using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestTool;
using TestModel;
using TestDAL;
using System.IO.Ports;

namespace WinForm
{
    public partial class ConfigSetting : Form
    {
        private ConfigData configData;
        private string initPath;
        private DataBase dataBase;

        public ConfigSetting(string path)
        {
            InitializeComponent();
            this.initPath = path;
            dataBase = new DataBase(initPath);
        }

        private void ConfigSetting_Load(object sender, EventArgs e)
        {
            GetInstPort();
            configData = dataBase.GetConfigData();
            tb_Title.Text = configData.Title;
            cb_GPIB.SelectedItem = configData.VisaPort;
            cb_Serial.SelectedItem = configData.SerialPort;
            cb_Power.SelectedItem = configData.PowerPort;
            tb_RXPower.Text = configData.Sen_TX_Power;
            tb_Packets.Text = configData.number_of_packets;
            tb_InqTimeOut.Text = configData.Inquiry_TimeOut;
            //tb_PeakLimit.Text = configData.PeakPower;
            //tb_AvgHI.Text = configData.AvgPowerHi;
            //tb_AvgLow.Text = configData.AvgPowerLow;
            
            tb_LowFreq.Text = configData.Low_Freq;
            tb_ModFreq.Text = configData.Mod_Freq;
            tb_HiFreq.Text = configData.Hi_Freq;

            tb_LowLoss.Text = configData.Low_Loss;
            tb_ModLoss.Text = configData.Mod_Loss;
            tb_HiLoss.Text = configData.Hi_Loss;

            cb_cd.Checked = configData.CD;
            cb_ic.Checked = configData.IC;
            cb_mi.Checked = configData.MI;
            cb_op.Checked = configData.OP;
            cb_ss.Checked = configData.SS;
            tb_Voltage1.Text = configData.Voltage1;
            tb_Voltage2.Text = configData.Voltage2;
            tb_Current.Text = configData.Current;

            GPIB_Select.Checked = configData.GPIB_Enable;
            Serial_Select.Checked = configData.Serial_Enable;
            Power_Select.Checked = configData.Power_Enable;

            tb_CompareString.Text = configData.CompareString;
            tb_SNLength.Text = configData.SNLength.ToString();

            Multimeter_Select.Checked = configData.Multimeter_Select;
            cb_Multimeter.SelectedItem = configData.MultimeterPort;

            cb_AudioEnable.Checked = configData.AudioEnable;
            tb_Path.Text = configData.AudioPath;

            cb_AudioEnable.Checked = configData.AudioEnable;
            tb_Path.Text = configData.AudioPath;

            cb_SerialSelect.Checked = configData.SerialSelect;

            cb_SNAuto.Checked = configData.AutoSNTest;
            tb_SNHear.Text = configData.SNHear;
            tb_Line.Text = configData.SNLine;

            cb_FixAuto.Checked = configData.AutoFixture;
            cb_FixPort.SelectedItem = configData.FixturePort;
        }

        public void GetInstPort()
        {
            List<string> list = Instrument.GetLocalName();

            //cb_GPIB.DataSource = list;
            cb_Serial.DataSource = SerialPort.GetPortNames();
            for (int i = 0; i < list.Count; i++)
            {
                cb_GPIB.Items.Add(list[i]);
                cb_Power.Items.Add(list[i]);
                cb_Multimeter.Items.Add(list[i]);
            }
            //cb_Power.DataSource = list;
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Title", tb_Title.Text.Trim());
            dic.Add("VisaPort", cb_GPIB.SelectedItem);
            dic.Add("SerialPort", cb_Serial.SelectedItem);
            dic.Add("PowerPort", cb_Power.SelectedItem);
            dic.Add("Sen_TX_Power", tb_RXPower.Text.Trim());
            dic.Add("number_of_packets", tb_Packets.Text.Trim());
            dic.Add("Low_Freq", tb_LowFreq.Text.Trim());
            dic.Add("Mod_Freq", tb_ModFreq.Text.Trim());
            dic.Add("Hi_Freq", tb_HiFreq.Text.Trim());
            dic.Add("Low_Loss", tb_LowLoss.Text.Trim());
            dic.Add("Mod_Loss", tb_ModLoss.Text.Trim());
            dic.Add("Hi_Loss", tb_HiLoss.Text.Trim());
            dic.Add("Inquiry_TimeOut", tb_InqTimeOut.Text.Trim());
            //dic.Add("PeakPower", tb_PeakLimit.Text.Trim());
            //dic.Add("AvgPowerLow", tb_AvgLow.Text.Trim());
            //dic.Add("AvgPowerHi", tb_AvgHI.Text.Trim());
            dic.Add("OP", cb_op.Checked);
            dic.Add("IC", cb_ic.Checked);
            dic.Add("CD", cb_cd.Checked);
            dic.Add("SS", cb_ss.Checked);
            dic.Add("MI", cb_mi.Checked);
            dic.Add("Voltage1", tb_Voltage1.Text.Trim());
            dic.Add("Voltage2", tb_Voltage2.Text.Trim());
            dic.Add("Current",tb_Current.Text.Trim());
            dic.Add("GPIB_Enable", GPIB_Select.Checked);
            dic.Add("Serial_Enable",Serial_Select.Checked);
            dic.Add("Power_Enable",Power_Select.Checked);
            dic.Add("CompareSN", tb_CompareString.Text.Trim());
            dic.Add("SNLength", tb_SNLength.Text.Trim());
            dic.Add("MultimeterPort", cb_Multimeter.SelectedItem);
            dic.Add("Multimeter_Enable", Multimeter_Select.Checked);
            dic.Add("AudioEnable", cb_AudioEnable.Checked);
            dic.Add("AudioPath", tb_Path.Text.Trim());
            dic.Add("SerialSelect", cb_SerialSelect.Checked);
            dic.Add("SNAuto", cb_SNAuto.Checked);
            dic.Add("SNHear", tb_SNHear.Text.Trim());
            dic.Add("SNLine", tb_Line.Text.Trim());
            dic.Add("FixAuto", cb_FixAuto.Checked);
            dic.Add("FixPort", cb_FixPort.SelectedItem);
            dataBase.UpdateConfigData(dic);
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void ConfigSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!(this.DialogResult == System.Windows.Forms.DialogResult.Yes))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void bt_Select_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "A2项目文件|*.abprojx|All files(*.*)|*.*";
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.ShowDialog();
            if(openFileDialog1.FileName != "")
            {
                tb_Path.Text = openFileDialog1.FileName.Trim();
            }

        }
    }
}
