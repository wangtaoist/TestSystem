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
        private bool queueFlag, testFlag,focusFlag;
        private Stopwatch stopwatch;
        private ConfigData config;

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
            focusFlag = true;
            initPath = Application.StartupPath;
            TestQueue = new Queue<string>();
            TimeQueue = new Queue<string>();
            testLogic = new TestLogic(TestQueue, initPath);
            FillTestItem();
            FillTestRadio();
            config = testLogic.GetConfigData();
            this.Text = config.Title;
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
            cht_PassRadio.Series[0].Points[1].Color = Color.SpringGreen;
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            //try
            //{
                ClsTestValue();
                Thread.Sleep(500);
                label_TestResult.Text = "Test";
                label_TestResult.BackColor = Color.LightSteelBlue;
                btTest.Enabled = false;
                tb_SN.Enabled = false;
                lb_Message.Items.Clear();
                stopwatch.Restart();
                testFlag = true;
                focusFlag = false;
                BtAddress = tb_SN.Text.Trim();
                testLogic.BTAddress = BtAddress;

                dgv_Data.ClearSelection();
                Thread.Sleep(500);
                ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestTime));
                ThreadPool.QueueUserWorkItem(new WaitCallback(TestProcess));
            //}
            //catch(Exception ex)
            //{
            //    lb_Message.Items.Add(ex.Message);
            //}
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
            try
            {
                foreach (var item in TestItmes)
                {
                    TestQueue.Enqueue(item.TestItemName + "开始测试");
                    Thread.Sleep(item.beferTime);
                    TestData testData = testLogic.TestProcess(item);

                    ShowTestItem(testData);

                    if (testData.Result == "Fail")
                    {
                        if (item.Check)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(item.AfterTime);
                    TestQueue.Enqueue(item.TestItemName + "测试完成");
                }
            }
            catch (Exception ex)
            {
                TestQueue.Enqueue(ex.Message);
                label_TestResult.Text = "Fail";
                label_TestResult.BackColor = Color.Red;
                btTest.Enabled = true;
                tb_SN.Enabled = true;
                testFlag = false;
                stopwatch.Stop();
                tb_SN.Text = "";
                tb_SN.Invoke(new Action(() =>
                {
                    tb_SN.Select();
                    tb_SN.Focus();
                    this.ActiveControl = tb_SN;
                }));
                if (Others.isWin10())
                {
                    focusFlag = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(FocusTextBox));
                }
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
                    tb_SN.Enabled = true;
                    btTest.Enabled = true;
                    dgv_Data.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                    label_TestResult.Text = "Fail";
                    label_TestResult.BackColor = Color.Red;
                    testLogic.RstPower();
                  
                    testFlag = false;
                    stopwatch.Stop();
                    testLogic.UpdataTestCount(false);
                    tb_SN.Text = "";
                    tb_SN.Invoke(new Action(() => 
                    {
                        tb_SN.Select();
                        tb_SN.Focus();
                        this.ActiveControl = tb_SN;
                    }));
                    ShowTestRadio();
                    var bt = TestItmes.Where(s => s.TestItem.Contains("Address")).ToList();
                    testLogic.SaveTestLog(TestItmes
                        , new LogColume()
                        {
                            SN = BtAddress,
                            TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                            MAC =  bt.Count > 0 ? bt[0].Value: "" ,
                            //MAC = BtAddress,
                            TotalStatus = "Fail"
                        });
                    if (Others.isWin10())
                    {
                        focusFlag = true;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(FocusTextBox));
                    }
                }
                else
                {
                    dgv_Data.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                }

            }

            if (TestItmes.Where(s => s.Result == "Pass").Count() == TestItmes.Count)
            {
                tb_SN.Enabled = true;
                btTest.Enabled = true;
                testFlag = false;
                stopwatch.Stop();
                label_TestResult.Text = "Pass";
                label_TestResult.BackColor = Color.SpringGreen;
                testLogic.UpdataTestCount(true);
               
                //Thread.Sleep(1000);
                ShowTestRadio();
                tb_SN.Text = "";
                tb_SN.Invoke(new Action(() => 
                {
                    tb_SN.Select();
                    tb_SN.Focus();
                    this.ActiveControl = tb_SN;
                }));
                var bt = TestItmes.Where(s => s.TestItem.Contains("Address")).ToList();
                testLogic.SaveTestLog(TestItmes
                    , new LogColume()
                    {
                        SN = BtAddress,
                        TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                        MAC = bt.Count > 0 ? bt[0].Value : "",
                        //MAC = BtAddress,
                        TotalStatus = "Pass"
                    });
                if (Others.isWin10())
                {
                    focusFlag = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(FocusTextBox));
                }
            }
            else if (TestItmes.Where(s => s.Result != "").Count() == TestItmes.Count)
            {              
                btTest.Enabled = true;
                tb_SN.Enabled = true;
                tb_SN.Text = "";
                testFlag = false;
                stopwatch.Stop();
                label_TestResult.Text = "Fail";
                label_TestResult.BackColor = Color.Red;
                testLogic.UpdataTestCount(false);
                tb_SN.Text = "";
                tb_SN.Invoke(new Action(() => 
                {
                    tb_SN.Select();
                    tb_SN.Focus();
                    this.ActiveControl = tb_SN;
                }));
                ShowTestRadio();
                var bt = TestItmes.Where(s => s.TestItem.Contains("Address")).ToList();
                testLogic.SaveTestLog(TestItmes
                    , new LogColume()
                    {
                        SN = BtAddress,
                        TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                        MAC = bt.Count > 0 ? bt[0].Value : "",
                        //MAC = BtAddress,
                        TotalStatus = "Fail"
                    });
                if (Others.isWin10())
                {
                    focusFlag = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(FocusTextBox));
                }
            }

            dgv_Data.FirstDisplayedScrollingRowIndex = index;
        }

        public void ShowTestStatus(string status)
        {
            this.BeginInvoke(new Action(delegate ()
                {
                    label_TestResult.Text = status;
                    if (status == "Pass")
                    {
                        label_TestResult.BackColor = Color.SpringGreen;
                    }
                    else
                    {
                        label_TestResult.BackColor = Color.Red;
                    }
                }));
        }

        public void ShowTestMessage(object obj)
        {
            while (queueFlag)
            {
                //lock (this)
                //{
                //this.BeginInvoke(new Action(delegate ()
                //    {
                if (TestQueue.Count != 0 && TestQueue != null)
                {
                    try
                    {
                        lb_Message.Items.Add(TestQueue.Dequeue());
                        Others.SendMessage(lb_Message.Handle, 0x0115, 1, 0);
                    }
                    catch (Exception ex)
                    {
                        //lb_Message.Items.Add(ex.Message);
                    }
                }
                //}));
                Thread.Sleep(50);
                //}
            }
        }

        public void ShowTestTime(object obj)
        {
            while (testFlag)
            {
                lock (this)
                {
                    string time = String.Format("{0:00}:{1:00}"
                            , stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
                    this.BeginInvoke(new Action(delegate ()
                    {
                        label_Time.Text = time;
                    }));
                    Thread.Sleep(200);
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
                cht_PassRadio.Series[0].Points[1].Color = Color.SpringGreen;

            }));
        }

        private void Tb_SN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                focusFlag = false;
                if (e.KeyCode == Keys.Enter)
                {
                    if (tb_SN.Text.Trim().StartsWith(config.CompareString)
                        && tb_SN.Text.Trim().Length == config.SNLength)
                    {
                        tb_SN.Enabled = false;
                        //btTest.PerformClick();
                        btTest_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("SN格式或者长度不够");
                        tb_SN.Text = "";
                        tb_SN.Select();
                    }
                }
            }
            catch(Exception ex)
            {
                lb_Message.Items.Add(ex.Message);
            }
        }

        private void cht_PassRadio_Click(object sender, EventArgs e)
        {
            frmSettingLogin login = new frmSettingLogin();
            if(login.ShowDialog(this) == DialogResult.OK)
            {
                testLogic.ClsRadio();
                Thread.Sleep(1000);
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
                    label_TestResult.Text = "Ready";
                    label_TestResult.BackColor = Color.LightSteelBlue;
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
                    label_TestResult.Text = "Ready";
                    label_TestResult.BackColor = Color.LightSteelBlue;
                    Winform_Load(null, null);
                }
            }
        }

        private void Winform_FormClosed(object sender, FormClosedEventArgs e)
        {
            focusFlag = false;
            queueFlag = false;
            testLogic.ClosedInstrument();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            focusFlag = false;
            queueFlag = false;
            testLogic.ClosedInstrument();
            this.Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }   

        public void FocusTextBox(object obj)
        {
            while (focusFlag)
            {
                this.BeginInvoke(new Action(() =>
                {
                    tb_SN.Select();
                    tb_SN.Focus();
                    this.ActiveControl = tb_SN;
                }));
                Thread.Sleep(200);
            }
           
        }
    }
}
