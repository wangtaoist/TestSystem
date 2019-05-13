using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestTool;
using TestDAL;
using TestModel;
using TestBLL;
using System.Threading;
using System.Diagnostics;
using TestEngineAPI;

namespace WinForm
{
    public partial class Winform : Form
    {
        private double widthX, heightY, columnHeiht;
        private TestLogic testLogic;
        private string initPath, BtAddress;
        private Queue<string> TestQueue, TimeQueue;
        private frmSettingLogin login;
        private List<TestData> TestItmes;
        private bool queueFlag, testFlag;
        private Stopwatch stopwatch;

        public Winform()
        {
            InitializeComponent();
            widthX = this.Width;
            heightY = this.Height;
            columnHeiht = dgv_Data.ColumnHeadersHeight;
            Others.setTag(this);
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Winform_Load(object sender, EventArgs e)
        {
            queueFlag = true;
            testFlag = true;
            initPath = Application.StartupPath;
            TestQueue = new Queue<string>();
            TimeQueue = new Queue<string>();
            testLogic = new TestLogic(TestQueue, initPath);
            FillTestItem();
            FillTestRadio();
            this.Text = testLogic.GetConfigData().Title;
            label_Test_Version.Text = this.Text;
            stopwatch = new Stopwatch();
            //stopwatch.Start();
            tb_SN.Select();
            dgv_Data.ClearSelection();

            ThreadPool.QueueUserWorkItem(new WaitCallback(testLogic.InitTestPort));
            ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestMessage));
        }    

        private void Winform_Resize(object sender, EventArgs e)
        {
            double xRate = this.Width / widthX;
            double yRate = this.Height / heightY;
            Others.setResolution(xRate, yRate, this);
            if (xRate > 1)
            {
                dgv_Data.ColumnHeadersHeight = Convert.ToInt16(columnHeiht * xRate) - 8;
            }
            else
            {
                dgv_Data.ColumnHeadersHeight =(int) columnHeiht;
            }
           
        }

        public void FillTestItem()
        {
             TestItmes = testLogic.GetTestItem();
             for (int i = 0; i < TestItmes.Count; i++)
            {
                dgv_Data.Rows.Add(new object[]
                {
                    TestItmes[i].ID,TestItmes[i].TestItemName,TestItmes[i].LowLimit
                    ,TestItmes[i].UppLimit,TestItmes[i].Unit,"",""
                });
            }
        }

        public void FillTestRadio()
        {
            TestRatio radio = testLogic.GetTestRadio();
            label_TotalNumber.Text = radio.Total.ToString();
            label_PassNumber.Text = radio.Pass.ToString();
            label_Defect_rate.Text = radio.PassRadio.ToString() + "%";

            string[] xValue = { "Fail", "Pass" };//设置标签
            double[] yValue = { 100 - radio.PassRadio,radio.PassRadio };    //获取要显示的值

            cht_PassRadio.Series[0].Points.DataBindXY(xValue, yValue);

            cht_PassRadio.Series[0].Points[0].Color = Color.Red;
            cht_PassRadio.Series[0].Points[1].Color = Color.Green;
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            label_TestResult.Text = "Test...";
            label_TestResult.BackColor = Color.LightSteelBlue;
            btTest.Enabled = false;
            lb_Message.Items.Clear();
            //stopwatch.Reset();
            stopwatch.Restart();
            ClsTestValue();
            testFlag = true;
            BtAddress = tb_SN.Text.Trim();
            tb_SN.Select();
            dgv_Data.ClearSelection();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestTime));
            ThreadPool.QueueUserWorkItem(new WaitCallback(TestProcess));

        }

        public void ClsTestValue()
        {
            for (int i = 0; i < dgv_Data.Rows.Count; i++)
            {
                dgv_Data.Rows[i].Cells[5].Value = "";
                dgv_Data.Rows[i].Cells[6].Value = "";
                dgv_Data.Rows[i].DefaultCellStyle.BackColor = SystemColors.Window;
                TestItmes[i].Result = "";
                TestItmes[i].Value = "";              
            }
        }

        public void TestProcess(object obj)
        {
            foreach (var item in TestItmes)
            {
                Thread.Sleep(item.beferTime);
                TestData testData = testLogic.TestProcess(item);
                ShowTestItem(testData);
                if(testData.Result == "Fail")
                {
                    if (item.Check)
                    {
                        break;
                    }
                }
                Thread.Sleep(item.AfterTime);
            }
        }

        public void ShowTestItem(TestData data)
        {
            int index = int.Parse(data.ID) - 1;
            dgv_Data.Rows[index].Cells[5].Value = data.Value;
            dgv_Data.Rows[index].Cells[6].Value = data.Result;

            if (data.Result == "Pass")
            {
                dgv_Data.Rows[index].DefaultCellStyle.BackColor = Color.SpringGreen;
            }
            else
            {
                if (data.Check)
                {
                    dgv_Data.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                    label_TestResult.Text = "Fail";
                    label_TestResult.BackColor = Color.Red;
                    btTest.Enabled = true;
                    testFlag = false;
                    stopwatch.Stop();
                    testLogic.UpdataTestCount(false);
                    //Thread.Sleep(500);
                    ShowTestRadio();
                    tb_SN.Select();
                   // var bt = TestItmes.Where(s => s.TestItem == "Read_BD_Address").ToList();
                    testLogic.SaveTestLog(TestItmes
                        , new LogColume()
                        {
                            SN = BtAddress,
                            TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                            //MAC = BtAddress == "" ? bt.Count > 0 ? bt[0].ToString() : "" : "",
                            MAC = BtAddress,
                            TotalStatus = "Fail"
                        });
                }
                else
                {
                    dgv_Data.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                }
                
            }

            if (TestItmes.Where(s => s.Result == "Pass").Count() == TestItmes.Count)
            {

                btTest.Enabled = true;
                testFlag = false;
                stopwatch.Stop();
                label_TestResult.Text = "Pass";
                label_TestResult.BackColor = Color.SpringGreen;
                testLogic.UpdataTestCount(true);
                //Thread.Sleep(1000);
                ShowTestRadio();
                tb_SN.Select();
                //var bt = TestItmes.Where(s => s.TestItem == "Read_BD_Address").ToList();
                testLogic.SaveTestLog(TestItmes
                    , new LogColume()
                    {
                        SN = BtAddress,
                        TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                        //MAC = BtAddress == "" ? bt.Count > 0 ? bt[0].Value : "" : "",
                        MAC = BtAddress,
                        TotalStatus = "Pass"
                    });
            }
            else if(TestItmes.Where(s=>s.Result != "").Count() == TestItmes.Count)
            {
                btTest.Enabled = true;
                testFlag = false;
                stopwatch.Stop();
                label_TestResult.Text = "Fail";
                label_TestResult.BackColor = Color.Red;
                testLogic.UpdataTestCount(false);
                //Thread.Sleep(1000);op

                ShowTestRadio();
                tb_SN.Select();
                //var bt = TestItmes.Where(s => s.TestItem == "Read_BD_Address").ToList();
                testLogic.SaveTestLog(TestItmes
                    , new LogColume()
                    {
                        SN = BtAddress,
                        TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                        //MAC = BtAddress == "" ? bt.Count > 0 ? bt[0].Value : "" : "",
                        MAC = BtAddress,
                        TotalStatus = "Fail"
                    });
            }
        }

        public void ShowTestStatus(string status)
        {
            //this.BeginInvoke(new Action(delegate()
            //    {
                    label_TestResult.Text = status;
                    if (status == "Pass")
                    {
                        label_TestResult.BackColor = Color.SpringGreen;
                    }
                    else
                    {
                        label_TestResult.BackColor = Color.Red;
                    }
                //}));
        }

        public void ShowTestMessage(object obj)
        {
            while (queueFlag)
            {
                lock (this)
                {
                    //this.BeginInvoke(new Action(delegate ()
                    //    {
                            if (TestQueue.Count != 0)
                            {
                                try
                                {
                                    lb_Message.Items.Add(TestQueue.Dequeue());
                                    Others.SendMessage(lb_Message.Handle, 0x0115, 1, 0);
                                }
                                catch (Exception) { }
                            }
                        //}));
                    Thread.Sleep(20);
                }
            }
        }

        public void ShowTestTime(object obj)
        {
            while (true)
            {
                if (testFlag)
                {
                    lock (this)
                    {
                        string time = String.Format("{0:00}:{1:00}"
                                , stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
                        this.BeginInvoke(new Action(delegate() 
                        {
                            label_Time.Text = time;
                        }));
                        Thread.Sleep(200);
                    }
                }
            }
          
        }

        public void ShowTestRadio()
        {
            this.Invoke(new Action(delegate () 
            {
                TestRatio radio = testLogic.GetTestRadio();
                label_TotalNumber.Text = radio.Total.ToString();
                label_PassNumber.Text = radio.Pass.ToString();
                label_Defect_rate.Text = radio.PassRadio.ToString();

                string[] xValue = { "Fail", "Pass" };//设置标签
                double[] yValue = { 100 - radio.PassRadio, radio.PassRadio };    //获取要显示的值

                cht_PassRadio.Series[0].Points.DataBindXY(xValue, yValue);

                cht_PassRadio.Series[0].Points[0].Color = Color.Red;
                cht_PassRadio.Series[0].Points[1].Color = Color.Green;

            }));
        }

        private void Tb_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btTest_Click(null, null);
            }
        }

        private void cht_PassRadio_Click(object sender, EventArgs e)
        {
            frmSettingLogin login = new frmSettingLogin();
            if(login.ShowDialog(this) == DialogResult.OK)
            {
                testLogic.ClsRadio();
                Thread.Sleep(500);
                ShowTestRadio();
            }
        }

        private void ConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login = new frmSettingLogin();
            var dialong = login.ShowDialog(this);
            if (dialong == System.Windows.Forms.DialogResult.OK)
            {
                ConfigSetting setting = new ConfigSetting(initPath);
                if (setting.ShowDialog(this) == System.Windows.Forms.DialogResult.Yes)
                {
                    dgv_Data.Rows.Clear();
                    queueFlag = false;
                    Winform_Load(null, null);
                }
            }
        }

        private void TestItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login = new frmSettingLogin();
            var dialong = login.ShowDialog(this);
            if (dialong == System.Windows.Forms.DialogResult.OK)
            {
                ItemConfig comfig = new ItemConfig(initPath);
                if (comfig.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    dgv_Data.Rows.Clear();
                    queueFlag = false;
                    Winform_Load(null, null);
                }
            }
        }

        private void Winform_FormClosed(object sender, FormClosedEventArgs e)
        {
            queueFlag = false;
            testLogic.ClosedInstrument();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }   
    }
}
