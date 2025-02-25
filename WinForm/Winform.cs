using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TestBLL;
using TestModel;
using TestTool;

namespace WinForm
{
    public partial class Winform : UIForm
    {
        //private const string SoftVersion = "V20.05.07.01";
        private const string SoftVersion = "V22.08.18.01";
        private const string SoftNumber = "HW_Headset";
        private double widthX, heightY, columnHeiht;
        private TestLogic testLogic;
        private string initPath, BtAddress, PackSN, filePath;
        private Queue<string> TestQueue, TimeQueue, statusQueue;
        private frmSettingLogin login;
        private List<TestData> TestItmes, FailItems;
        private bool queueFlag, testFlag, focusFlag;
        private Stopwatch stopwatch;
        private ConfigData config;
        private SerialPort FixturePort;
        private const int WM_DEVICE_CHANGE = 0x219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICE_REMOVE_COMPLETE = 0x8004;
        private System.Windows.Forms.Timer time;
        private System.Threading.Timer ShowTime;
        private WebReference.WebService1 web;
        private WebReference1.Service1 verWeb;
        private int color;
        private Thread thread;
        private SerialPortSwitch portSwitch;
        private License license;
        private float DPI;

        
        public Winform()
        {
            InitializeComponent();
            
            widthX = this.Width;
            heightY = this.Height;
            columnHeiht = dgv_Data.ColumnHeadersHeight;
            Others.setTag(this);
            Others.InitOthers(SoftVersion);
            Others.handle = this.Handle;
            Control.CheckForIllegalCrossThreadCalls = false;
            DPI = getDPI();

            #region 调试
            //string da = "真我";
            ////真我Buds T200 Lite
            //string daa = "E79C9FE68891427564732054323030204C697465";
            ////string data = GetChsFromHex(da);
            //string d = GetChsFromHex(daa);
            //var ti = DateTime.ParseExact("20240530 18:51:22"
            //    , "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
            //var time = (DateTime.Now - ti).TotalDays;
            //var t = ti;

            //int b = Int32.MaxValue;
            //byte[] bte = new byte[] { 0x26, 0x8e, 0x12, 0x00 };
            //int a = BitConverter.ToInt32(bte, 0);
            //this.loadWindows = loadWindows;
            //bool STAT = "S".StartsWith("SYN0");
            //string res = string.Empty;
            //web = new WebReference.WebService1();
            //verWeb = new WebReference1.Service1();
            // res = verWeb.ver_cx("CM70", "V20.03.14.02");
            //verWeb.Abort();
            //res = web.SnCx("E09DFAD270ED ", "耳机芯灵敏度、咪曲线测试");
            //res = web.SnCx("28FA1972A0E1", "成品外观检查");
            //res = web.SnCx("2155030949GJ03013157", "包装投入");
            //res = web.SnCx_sn("GJ1253204N400027");
            //res = web.SnCx_SC("E09DFA6A1BB3", "耳机芯灵敏度、咪曲线测试");
            //res = web.SnCx_BZLY("E09DFA4745A6");
            //res = web.SnCx_BZSN("GJ1225201E200445");
            //res = web.SnCx_LY("E09DFA6A1C43");
            //res = web.SnCx_DC("SYN0C1911000300");
            //res = web.SnCx_BDSN("GJ1225203L100664");
            //string pa = Path.Combine(Application.StartupPath, "CMD");
            //Others.CmdExcute(pa + "\\CmdReadWrite.exe QUERY_SW_VER ");
            //web.Abort();
            #endregion
        }

        private void Winform_Load(object sender, EventArgs e)
        {
            queueFlag = true;
            testFlag = true;
            focusFlag = true;

            TestQueue = new Queue<string>();
            TimeQueue = new Queue<string>();
            statusQueue = new Queue<string>();

            filePath = Properties.Settings.Default.filePath;
            color = Properties.Settings.Default.color;

            initPath = Path.Combine(Application.StartupPath, "DataBase", "TestDB.mdb");
            if (System.IO.File.Exists(filePath))
            {
                initPath = filePath;
            }
            testLogic = new TestLogic(TestQueue, initPath, statusQueue);
            config = testLogic.GetConfigData();

            //license = new License();
            //license.path = initPath;
            //bool lic = license.CheckLicense();

            FillTestItem();
            FillTestRadio();


            this.Text = config.Title;
            label_Test_Version.Text = this.Text;
            stopwatch = new Stopwatch();
            //stopwatch.Start();
            tb_SN.Select();
            dgv_Data.ClearSelection();
            lb_Message.Items.Clear();

            string location = Properties.Settings.Default.location;
            string[] total = null;
            this.Resize += Winform_Resize;
            if (location != "")
            {
                total = location.Split(",");
                if (total[4] == "Normal")
                {
                    this.Width = int.Parse(total[0]);
                    this.Height = int.Parse(total[1]);
                    this.SetDesktopLocation(int.Parse(total[2]), int.Parse(total[3]));
                    //this.Location = new Point(int.Parse(total[2]), int.Parse(total[3]));
                }
                else
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(testLogic.InitTestPort));
            ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestMessage));
            ThreadPool.QueueUserWorkItem(new WaitCallback(ShowStatus));
            //if (config.AudioEnable)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(CheckSound));
            //}
            if (config.AutoFixture)
            {
                
                if (config.AutoHALL)
                {
                    testLogic.FixPort = FixturePort;
                    portSwitch = new SerialPortSwitch(config.FixturePort);
                    portSwitch.SwitchOn += PortSwitch_SwitchOn;
                    portSwitch.Start();
                }
                else
                {
                    FixturePort = new SerialPort();
                    FixturePort.BaudRate = 9600;
                    FixturePort.PortName = config.FixturePort;
                    FixturePort.DataBits = 8;
                    FixturePort.Parity = Parity.None;
                    FixturePort.StopBits = StopBits.One;
                    FixturePort.DataReceived += FixturePort_DataReceived;

                    if (FixturePort.IsOpen)
                    {
                        FixturePort.Close();
                    }
                    FixturePort.Open();
                    //FixturePort.Write("OPEN\r");
                    FixturePort.DiscardInBuffer();
                    FixturePort.DiscardOutBuffer();
                }
            }

          
            #region mes
            //if (config.MesEnable)
            //{
            //    verWeb = new WebReference1.Service1();
            //    string result = verWeb.ver_cx(SoftNumber, SoftVersion);
            //    verWeb.Abort();
            //    verWeb.Dispose();
            //    if (result.Equals("F"))
            //    {
            //        var box = MessageBox.Show("软件版本不是最新版本，" +
            //            "请联系工程人员更新为最新版本", "版本提示"
            //               , MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        if (box == DialogResult.OK)
            //        {
            //            this.WindowState = FormWindowState.Normal;
            //            this.Winform_FormClosed(null, null);
            //            this.Close();
            //        }
            //    }
            //}
            #endregion

            StyleToolStripMenuItem.DropDownItems.Clear();
            var styles = Enum.GetValues(typeof(UIStyle));
            foreach (var item in styles)
            {
                StyleToolStripMenuItem.DropDownItems.Add(item.ToString());
            }

            uiStyleManager1.Style = (UIStyle)color;

           
        }

        private void Winform_Resize(object sender, EventArgs e)
        {
            double xRate = ((double)base.Width) / this.widthX;
            double yRate = ((double)base.Height) / this.heightY;
            Others.setResolution(xRate, yRate, this, DPI);
            if (xRate > 1.1)
            {
                this.dgv_Data.ColumnHeadersHeight = Convert.ToInt16((double)(this.columnHeiht * xRate)) - 8;
            }
            else
            {
                this.dgv_Data.ColumnHeadersHeight = (int)this.columnHeiht;
            }
        }

        public void FillTestItem()
        {
            TestItmes = testLogic.GetTestItem();
            FailItems = testLogic.GetFailItem();
            int j = 0;
            for (int i = 0; i < TestItmes.Count; i++)
            {

                if (TestItmes[i].Show)
                {
                    j += 1;
                    dgv_Data.Rows.Add(new object[]
                    {
                    j,TestItmes[i].TestItemName,TestItmes[i].LowLimit
                    ,TestItmes[i].UppLimit,TestItmes[i].Unit,"",""
                    });

                    int count = dgv_Data.Rows.Count;
                    dgv_Data.Rows[count -1].Height = int.Parse((dgv_Data.Rows[count -1].Height * DPI).ToString());

                }
            }
        }

        public void FillTestRadio()
        {
            TestRatio radio = testLogic.GetTestRadio();
            label_TotalNumber.Text = radio.Total.ToString();
            label_PassNumber.Text = radio.Pass.ToString();
            label_Defect_rate.Text = radio.PassRadio.ToString() + "%";

            string[] xValue = { "Fail", "Pass" };//设置标签
            double[] yValue = { 100 - radio.PassRadio, radio.PassRadio };    //获取要显示的值

            cht_PassRadio.Series[0].Points.DataBindXY(xValue, yValue);

            cht_PassRadio.Series[0].Points[0].Color = Color.Red;
            cht_PassRadio.Series[0].Points[1].Color = Color.SpringGreen;
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            try
            {
                testLogic.PackSN = tb_SN.Text;
                ClsTestValue();
                Thread.Sleep(200);
                label_TestResult.Text = "Test";
                label_TestResult.BackColor = Color.LightSteelBlue;
                btTest.Enabled = false;
                tb_SN.Enabled = false;
                lb_Message.Items.Clear();
                CloseFixture();
                dgv_Data.FirstDisplayedScrollingRowIndex = 1;
            }
            catch (Exception ex)
            {
                TestQueue.Enqueue("ex;"  + ex.Message);
            }
            stopwatch.Restart();
            testFlag = true;
            focusFlag = false;
            queueFlag = true;
            BtAddress = tb_SN.Text.Trim();
            testLogic.BTAddress = BtAddress;

            dgv_Data.ClearSelection();
            Thread.Sleep(200);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestTime));

            //ThreadPool.QueueUserWorkItem(new WaitCallback(TestProcess));
            thread = new Thread(new ThreadStart(TestProcess));
            thread.IsBackground = true;
            thread.Start();
        }

        public void ClsTestValue()
        {
            for (int i = 0; i < dgv_Data.Rows.Count; i++)
            {
                dgv_Data.Rows[i].Cells[5].Value = "";
                dgv_Data.Rows[i].Cells[6].Value = "";
                dgv_Data.Rows[i].DefaultCellStyle.BackColor = SystemColors.Window;

            }
            for (int i = 0; i < TestItmes.Count; i++)
            {
                TestItmes[i].Result = "";
                TestItmes[i].Value = "";
            }
        }

        public void TestProcess()
        {
            try
            {
                foreach (var item in TestItmes)
                {
                    //string status = obj == null ? "" : obj.ToString();
                  
                    TestQueue.Enqueue(item.TestItemName + "开始测试");
                    Thread.Sleep(item.beferTime);
                    TestData testData = null;
                    if(item.TestItem == "FixtureControl")
                    {
                        testData = item;
                        FixturePort.Write(item.Other + "\r");
                        testData.Result = "Pass";
                        testData.Value = "Pass";
                    }
                    else
                    {
                        testData = testLogic.TestProcess(item);
                    }
                    TestQueue.Enqueue(item.TestItemName + "测试完成");
                    testLogic.TestItmes = TestItmes;
                    ShowTestItem(testData);

                    if (testData.Result == "Fail")
                    {
                        if (item.Check)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(item.AfterTime);

                }
            }
            catch (Exception ex)
            {
                TestQueue.Enqueue("ex;" + ex.Message + ",测试项目报错，打开屏蔽箱");
                //OpenFixture();
                //TestQueue.Enqueue(ex.Message);
                label_TestResult.Text = "Fail";
                label_TestResult.BackColor = Color.Red;
                btTest.Enabled = true;
                tb_SN.Enabled = true;
                testFlag = false;
                stopwatch.Stop();
                tb_SN.Text = "";

                ThreadPool.QueueUserWorkItem(new WaitCallback(TestFailProcess));

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

        public void TestFailProcess(object obj)
        {
            foreach (var item in FailItems)
            {
                //TestQueue.Enqueue("Fail后：" + item.TestItemName + "开始测试");
                Thread.Sleep(item.beferTime);
                testLogic.TestProcess(item);
                Thread.Sleep(item.AfterTime);
                //TestQueue.Enqueue("Fail后：" + item.TestItemName + "测试完成");
            }
        }

        public void ShowTestItem(TestData data)
        {
            int index = 0;
            if (data.Result == "" || data.Value == "")
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            if (data.Show)
            {
                index = int.Parse(data.ID) - 1;
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
                        TestQueue.Enqueue("测试Fail，打开屏蔽箱");
                        OpenFixture();
                        PlugManagement();
                        tb_SN.Enabled = true;
                        btTest.Enabled = true;
                        dgv_Data.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                        label_TestResult.Text = "Fail";
                        label_TestResult.BackColor = Color.Red;
                        //testLogic.RstPower();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(TestFailProcess));

                        testFlag = false;
                        stopwatch.Stop();
                       
                        tb_SN.Text = "";
                        tb_SN.Invoke(new Action(() =>
                        {
                            tb_SN.Select();
                            tb_SN.Focus();
                            this.ActiveControl = tb_SN;
                        }));
                       
                        var bt = TestItmes.Where(s => s.TestItem.Contains("Address")).ToList()
                .Count == 0
                ? TestItmes.Where(s => s.TestItem.Contains("PairMessage")).ToList()
                .Count == 0 ? TestItmes.Where(s => s.TestItemName.Contains("蓝牙地址")).ToList()
                : TestItmes.Where(s => s.TestItem.Contains("PairMessage")).ToList()
                : TestItmes.Where(s => s.TestItem.Contains("Address")).ToList();
                        testLogic.SaveTestLog(TestItmes
                            , new LogColume()
                            {
                                SN = BtAddress == "" ? bt.Count == 0 ? "" : bt[0].Value : BtAddress,
                                TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                                MAC =  bt.Count > 0 ? bt[0].Value : BtAddress,
                                //MAC = BtAddress,
                                TotalStatus = "Fail"
                            });

                        if (TestItmes.Where(s => s.TestItem.Contains("MT8852")).ToList().Count > 0)
                        {
                            if (bt.Count != 0 && bt[0].Value != "")
                            {
                                testLogic.UpdataTestCount(false);
                                ShowTestRadio();
                            }
                        }
                        else
                        {
                            testLogic.UpdataTestCount(false);
                            ShowTestRadio();
                        }

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
            }
            else
            {
                if (data.Result == "Fail")
                {
                    int row = dgv_Data.Rows.Count;
                    int i = 0;
                    for (; i < row; i++)
                    {
                        if (dgv_Data.Rows[i].Cells[6].Value.ToString() == "")
                        {
                            break;
                        }
                    }
                    dgv_Data.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    //ShowFailResult(index);
                }
            }
            if (TestItmes.Where(s => s.Result == "Pass").Count() == TestItmes.Count)
            {
                TestQueue.Enqueue("测试Pass，打开屏蔽箱");
                OpenFixture();
                PlugManagement();
                tb_SN.Enabled = true;
                btTest.Enabled = true;

                //if (TestItmes.Where(s => s.TestItem.Contains("BES_ClearPair")).Count() == 0 
                //    && TestItmes.Where(s => s.TestItem.Contains("BES_Shutdown")).Count() == 0)
                //{
                testFlag = false;
                stopwatch.Stop();
                label_TestResult.Text = "Pass";
                label_TestResult.BackColor = Color.SpringGreen;
                //}
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
                var bt = TestItmes.Where(s => s.TestItem.Contains("Address")).ToList()
                .Count == 0
                ? TestItmes.Where(s => s.TestItem.Contains("PairMessage")).ToList()
                .Count == 0 ? TestItmes.Where(s => s.TestItemName.Contains("蓝牙地址")).ToList()
                : TestItmes.Where(s => s.TestItem.Contains("PairMessage")).ToList()
                : TestItmes.Where(s => s.TestItem.Contains("Address")).ToList();

                testLogic.SaveTestLog(TestItmes
                    , new LogColume()
                    {
                        SN = BtAddress == "" ?bt.Count == 0 ? "" : bt[0].Value : BtAddress,
                        TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                        MAC = bt.Count > 0 ? bt[0].Value : BtAddress,
                        //MAC = BtAddress,
                        TotalStatus = "Pass"
                    });
                if (Others.isWin10())
                {
                    focusFlag = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(FocusTextBox));
                }

                //if (TestItmes.Where(s => s.TestItem.Contains("ClearPair")).Count() == 0
                //    && TestItmes.Where(s => s.TestItem.Contains("Shutdown")).Count() == 0)
                //{
                ShowTime = new System.Threading.Timer(new TimerCallback(ShowTime_Tick));
                ShowTime.Change(3000, 5000);
                //}

            }
            else if (TestItmes.Where(s => s.Result != "").Count() == TestItmes.Count)
            {
                ShowFailResult(index);
            }

            if (dgv_Data.Rows.Count > 0)
            {
                if (index != 0 )
                {
                    dgv_Data.FirstDisplayedScrollingRowIndex = index;
                }
            }
        }

        public void ShowFailResult(int index)
        {
            TestQueue.Enqueue("测试Fail2，打开屏蔽箱");
            OpenFixture();
            PlugManagement();
            btTest.Enabled = true;
            tb_SN.Enabled = true;
            tb_SN.Text = "";
            testFlag = false;
            stopwatch.Stop();
            label_TestResult.Text = "Fail";
            label_TestResult.BackColor = Color.Red;
           
            tb_SN.Text = "";
            tb_SN.Invoke(new Action(() =>
            {
                tb_SN.Select();
                tb_SN.Focus();
                this.ActiveControl = tb_SN;
            }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(TestFailProcess));

            var bt = TestItmes.Where(s => s.TestItem.Contains("Address")).ToList()
                .Count == 0
                ? TestItmes.Where(s => s.TestItem.Contains("PairMessage")).ToList()
                .Count == 0
                ? TestItmes.Where(s => s.TestItemName.Contains("蓝牙地址")).ToList()
                : TestItmes.Where(s => s.TestItem.Contains("PairMessage")).ToList()
                : TestItmes.Where(s => s.TestItem.Contains("Address")).ToList();

            //var bt = TestItmes.Where(s => s.TestItem.Contains("Address")).ToList()
            //    .Count == 0
            //    ? TestItmes.Where(s => s.TestItemName.Contains("蓝牙地址")).ToList()
            //    : TestItmes.Where(s => s.TestItem.Contains("Address")).ToList();

            testLogic.SaveTestLog(TestItmes
                , new LogColume()
                {
                    SN = BtAddress == "" ? bt.Count == 0 ? "" : bt[0].Value : BtAddress,
                    TestTime = DateTime.Now.ToString("yyyyMMddHHddss"),
                    MAC = bt.Count > 0 ? bt[0].Value : BtAddress,
                    //MAC = BtAddress,
                    TotalStatus = "Fail"
                });

            if (TestItmes.Where(s => s.TestItem.Contains("MT8852")).ToList().Count > 0)
            {
                if (bt.Count != 0 && bt[0].Value != "")
                {
                    testLogic.UpdataTestCount(false);
                    ShowTestRadio();
                }
            }
            else
            {
                testLogic.UpdataTestCount(false);
                ShowTestRadio();
            }

            if (Others.isWin10())
            {
                focusFlag = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(FocusTextBox));
            }

            if (dgv_Data.Rows.Count > 0)
            {
                if (index != 0)
                {
                    dgv_Data.FirstDisplayedScrollingRowIndex = index;
                }
            }
        }

        public void ShowStatus(object obj)
        {
            while (queueFlag)
            {
                if (statusQueue.Count != 0 && statusQueue != null)
                {
                    try
                    {
                        ShowTestStatus(statusQueue.Dequeue());
                    }
                    catch (Exception ex)
                    {
                        lb_Message.Items.Add(ex.Message);
                    }
                }
                Thread.Sleep(50);
            }
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
                    else if (status == "Fail")
                    {
                        label_TestResult.BackColor = Color.Red;
                    }
                    else if (status == "移开霍尔")
                    {
                        label_TestResult.BackColor = Color.Yellow;
                    }
                    else if (status == "End")
                    {
                        label_TestResult.BackColor = Color.LightSteelBlue;
                        label_TestResult.Text = "Test";
                    }

                }));
        }

        public void ShowTestMessage(object obj)
        {
            while (queueFlag)
            {
                //lock (this)
                //{
                this.BeginInvoke(new Action(() =>
                    {
                        if (TestQueue.Count != 0 && TestQueue != null)
                        {
                            try
                            {
                                string msg = TestQueue.Dequeue();
                                if (msg.StartsWith("ex"))
                                {
                                    //Thread.Sleep(10);
                                    Others.WriteErrorLog(msg.Substring(3, msg.Length - 3));
                                    lb_Message.Items.Add(msg.Substring(3, msg.Length - 3));
                                }
                                else
                                {
                                    //Thread.Sleep(10);
                                    //Others.WriteInformationLog(msg);
                                    lb_Message.Items.Add(msg);
                                }

                                Others.SendMessage(lb_Message.Handle, 0x0115, 1, 0);
                            }
                            catch (Exception ex)
                            {
                                lb_Message.Items.Add(ex.Message);
                                Others.WriteErrorLog("ex;" + ex.Message);
                            }
                        }
                    }));
                Thread.Sleep(50);
                //}
            }
        }

        public void ShowTestTime(object obj)
        {
            while (testFlag)
            {
                //lock (this)
                //{
                string time = String.Format("{0:00}:{1:00}"
                        , stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
                this.BeginInvoke(new Action(delegate ()
                {
                    label_Time.Text = time;
                }));
                Thread.Sleep(200);
                //}

            }
        }

        public void CheckSound(object obj)
        {
            while (queueFlag)
            {
                if (btTest.Enabled)
                {
                    ManagementObjectSearcher VoiceDeviceSearcher =
                        new ManagementObjectSearcher("select * from Win32_SoundDevice");//声明一个用于检索设备管理信息的对象
                    foreach (ManagementObject VoiceDeviceObject in VoiceDeviceSearcher.Get())//循环遍历WMI实例中的每一个对象
                    {
                        string name = VoiceDeviceObject["ProductName"].ToString(); //在当前文本框中显示声音设备的名称
                        if (name.Contains("USB"))
                        {
                            TestQueue.Enqueue("插入音频设备");
                            focusFlag = false;

                            btTest_Click(null, null);
                        }
                    }

                }
                Thread.Sleep(200);
            }
        }

        public void ShowTestRadio()
        {
            this.BeginInvoke(new Action(delegate ()
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
                        //CloseFixture();
                        //if (config.MesEnable)
                        //{
                        //    if (config.SNLength != 0)
                        //    {
                        //        if (tb_SN.Text.Length == 20)
                        //        {
                        //            web = new WebReference.WebService1();
                        //            PackSN = tb_SN.Text.Trim();
                        //            string reslut = web.SnCx(PackSN, config.MesStation);
                        //            testLogic.PackSN = PackSN;
                        //            if (reslut == "P")
                        //            {
                        //                btTest_Click(null, null);
                        //            }
                        //            else
                        //            {
                        //                MessageBox.Show(string.Format("上一个工位:{0}:连续测试NG品,请检查 ", config.MesStation)
                        //                    , "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //                tb_SN.Enabled = true;
                        //                tb_SN.Clear();
                        //                tb_SN.Select();
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        btTest_Click(null, null);
                        //    }
                        //}
                        //else
                        //{
                        btTest_Click(null, null);
                        //}
                        //btTest.PerformClick();

                    }
                    else
                    {
                        MessageBox.Show("SN格式或者长度不够");
                        tb_SN.Text = "";
                        tb_SN.Select();
                    }
                }
            }
            catch (Exception ex)
            {
                TestQueue.Enqueue("ex;" + ex.Message);
            }
        }

        private void ReLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgv_Data.Rows.Clear();
            Winform_Load(null, null);
        }

        private void cht_PassRadio_Click(object sender, EventArgs e)
        {
            focusFlag = false;
            frmSettingLogin login = new frmSettingLogin();
            login.Function = "Radio";
            if (login.ShowDialog(this) == DialogResult.OK)
            {
                testLogic.ClsRadio();
                Thread.Sleep(1000);
                ShowTestRadio();
            }
            else
            {
                focusFlag = true;
            }
        }

        private void ConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login = new frmSettingLogin();
            focusFlag = false;
            queueFlag = false;
            if (config.AutoFixture == true)
            {
                if (config.AutoHALL)
                {
                    portSwitch.Stop();
                   
                }
                else
                {
                    FixturePort.Close();
                }
            }
            var dialong = login.ShowDialog(this);
            if (dialong == System.Windows.Forms.DialogResult.OK)
            {
                ConfigSetting setting = new ConfigSetting(initPath);
                if (setting.ShowDialog(this) == System.Windows.Forms.DialogResult.Yes)
                {
                    dgv_Data.Rows.Clear();
                    //queueFlag = false;
                    label_TestResult.Text = "Ready";
                    label_TestResult.BackColor = Color.LightSteelBlue;
                    Winform_Load(null, null);
                }
                else
                {
                    focusFlag = true;
                    queueFlag = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestMessage));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ShowStatus));
                }
            }
            else
            {
                focusFlag = true;
                queueFlag = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestMessage));
                ThreadPool.QueueUserWorkItem(new WaitCallback(ShowStatus));
            }
        }

        private void TestItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login = new frmSettingLogin();
            focusFlag = false;
            queueFlag = false;
            if (config.AutoFixture == true)
            {
                if (config.AutoHALL)
                {
                    portSwitch.Stop();
                }
                else
                {
                    FixturePort.Close();
                }
            }
            var dialong = login.ShowDialog(this);
            if (dialong == System.Windows.Forms.DialogResult.OK)
            {
                ItemConfig comfig = new ItemConfig(initPath);
                if (comfig.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    dgv_Data.Rows.Clear();
                    //queueFlag = false;
                    label_TestResult.Text = "Ready";
                    label_TestResult.BackColor = Color.LightSteelBlue;
                    Winform_Load(null, null);
                }
                else
                {
                    focusFlag = true;
                    queueFlag = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestMessage));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ShowStatus));
                }
            }
            else
            {
                focusFlag = true;
                queueFlag = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(ShowTestMessage));
                ThreadPool.QueueUserWorkItem(new WaitCallback(ShowStatus));
            }
        }

        private void Winform_FormClosed(object sender, FormClosedEventArgs e)
        {
            focusFlag = false;
            queueFlag = false;
            testLogic.ClosedInstrument();
            saveLocation();
            Others.WriteInformationLog("程序关闭");
            if(portSwitch != null)
            {
                portSwitch.Stop();
            }
            Application.Exit();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            focusFlag = false;
            queueFlag = false;
            testLogic.ClosedInstrument();
            saveLocation();
            Others.WriteInformationLog("程序关闭");
            //this.Close();
            if (portSwitch != null)
            {
                portSwitch.Stop();
            }
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.lb_Verison.Text = SoftVersion;
            about.ShowDialog();
        }

        private void mESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login = new frmSettingLogin();
            login.Function = "MES";
            focusFlag = false;
            //queueFlag = false;
            if (config.AutoFixture == true)
            {
                if (config.AutoHALL)
                {
                    portSwitch.Stop();
                }
                else
                {
                    FixturePort.Close();
                }
            }
            var dialong = login.ShowDialog(this);
            if (dialong == System.Windows.Forms.DialogResult.OK)
            
            {
                MesWindow setting = new MesWindow(config, initPath);
                if (setting.ShowDialog(this) == System.Windows.Forms.DialogResult.Yes)
                {
                    dgv_Data.Rows.Clear();
                    //queueFlag = false;
                    label_TestResult.Text = "Ready";
                    label_TestResult.BackColor = Color.LightSteelBlue;
                    Winform_Load(null, null);
                }
            }
            else
            {
                focusFlag = true;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectData = dgv_Data.CurrentCell.Value.ToString(); 
            if(selectData != "")
            {
                Clipboard.SetDataObject(selectData);
            }
            else
            {
                MessageBox.Show("数据为空，请重新选择");
            }
        }

        private void StyleToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int style = StyleToolStripMenuItem.DropDownItems.IndexOf(e.ClickedItem);
            switch (style)
            {
                case 10: style = 101; break;
                case 11: style = 102; break;
                case 12: style = 103; break;
                case 13: style = 201; break;
                case 14: style = 202; break;
                case 15: style = 203; break;
                case 16: style = 204; break;
                case 17: style = 205; break;
                case 18: style = 209; break;
                case 19: style = 999; break;
            }
            uiStyleManager1.Style = (UIStyle)style;
            Properties.Settings.Default.color = style;
            Properties.Settings.Default.Save();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login = new frmSettingLogin();
            focusFlag = false;
            queueFlag = false;
            if (config.AutoFixture == true)
            {
                if (config.AutoHALL)
                {
                    portSwitch.Stop();
                }
                else
                {
                    FixturePort.Close();
                }
            }
            var dialong = login.ShowDialog(this);
            if (dialong == System.Windows.Forms.DialogResult.OK)
            {
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "Access File (*.mdb)|*.mdb|All files (*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.filePath = openFileDialog1.FileName;
                    Properties.Settings.Default.Save();
                    dgv_Data.Rows.Clear();
                    Winform_Load(null, null);
                }
            }
            else
            {
                focusFlag = true;
                queueFlag = true;
            }
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login = new frmSettingLogin();
            focusFlag = false;
            queueFlag = false;
            if (config.AutoFixture == true)
            {
                if (config.AutoHALL)
                {
                    portSwitch.Stop();
                }
                else
                {
                    FixturePort.Close();
                }
            }
            var dialong = login.ShowDialog(this);
            if (dialong == System.Windows.Forms.DialogResult.OK)
            {
                saveFileDialog1.Filter = "Access files (*.mdb)|*.mdb|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //MessageBox.Show(saveFileDialog1.FileName);
                    filePath = saveFileDialog1.FileName;
                    System.IO.File.Copy(testLogic.GetDbPath(), filePath);

                    Properties.Settings.Default.filePath = filePath;
                    Properties.Settings.Default.Save();

                    testLogic.SaveConfig(filePath, config);
                    testLogic.SaveTestItem(TestItmes, FailItems);
                }
            }
            else
            {
                focusFlag = true;
                queueFlag = true;
            }
        }

        public void FocusTextBox(object obj)
        {
            while (focusFlag)
            {
                //int x = this.Location.X;
                //int y = this.Location.Y;
                this.BeginInvoke(new Action(() =>
                {
                    //Others.mouse_event((int)0x0002, x, y, 0, IntPtr.Zero);
                    if (this.Handle != Others.GetF())
                    {
                        Others.SetF(this.Handle);
                    }
                    tb_SN.Select();
                    tb_SN.Focus();

                }));
                Thread.Sleep(1000);
            }

        }

        protected override void WndProc(ref Message m)
        {
            if (m.WParam.ToInt32() == DBT_DEVICEARRIVAL)
            {
                TestQueue.Enqueue("插入设备");
                if (config.AutoSNTest)
                {
                    if (SerialPort.GetPortNames().Where(s =>
                    s.Contains(config.SerialPort)).Count() > 0)
                    {
                        focusFlag = false;
                        if (btTest.Enabled)
                        {
                            btTest_Click(null, null);
                        }
                    }
                }
            }
            else if (m.WParam.ToInt32() == DBT_DEVICE_REMOVE_COMPLETE)
            {
                TestQueue.Enqueue("拔出设备");
            }

            base.WndProc(ref m);
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            label_TestResult.Text = "Pass";
            label_TestResult.BackColor = Color.SpringGreen;
            time.Stop();
        }

        private void ShowTime_Tick(object sender)
        {
            label_TestResult.Text = "Ready";
            label_TestResult.BackColor = Color.LightSteelBlue;
            ShowTime.Dispose();
        }

        private void FixturePort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (FixturePort.ReadByte() > 0)
            {
                Thread.Sleep(200);
                string cmd = FixturePort.ReadExisting().Trim();
                TestQueue.Enqueue("屏蔽箱返回值：" + cmd);
                if ("READY".Contains(cmd))
                {
                    if (btTest.Enabled)
                    {
                        focusFlag = false;
                        btTest_Click(null, null);
                    }
                }
                else if (cmd.Contains("N"))
                {
                    TestQueue.Enqueue("屏蔽箱打开成功");
                }
                FixturePort.DiscardInBuffer();
                FixturePort.DiscardOutBuffer();
            }
        }

        private void PortSwitch_SwitchOn(Pin pin)
        {
            if (pin == Pin.CTS)
            {
                TestQueue.Enqueue("脚踏按下");
                if (tb_SN.Enabled == true)
                {
                    focusFlag = false;
                    btTest_Click(null, null);
                }
            }
        }

        private void OpenFixture()
        {
            if (config.AutoFixture)
            {
                FixturePort.WriteLine("OPEN" + "\r");
            }
        }

        private void CloseFixture()
        {
            if (config.AutoFixture)
            {
                FixturePort.WriteLine("CLOSE\r");
            }
        }

        private void PlugManagement()
        {
            if (config.PlugEnable)
            {
                int MaxNum = int.Parse(config.MaxSet);
                int PlugNumber = testLogic.GetPlugNumber() + 1;
                if (PlugNumber >= MaxNum)
                {
                    MessageBox.Show("插座已达到最大使用次数，请进行更换", "Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    testLogic.SetPlugNumber(PlugNumber);
                }
            }
        }

        public void saveLocation()
        {
            //Normal
            double w = this.Width;
            double h = this.Height;
            double x = this.Location.X;
            double y = this.Location.Y;
            string state = this.WindowState.ToString(); 
            string location = string.Format("{0},{1},{2},{3},{4}", w, h, x, y,state);
            Properties.Settings.Default.location = location;
            Properties.Settings.Default.Save();
        }

        public float getDPI()
        {
            Graphics graphics = this.CreateGraphics();
            float dpiX = graphics.DpiX;
            float dpi = 1;
            switch (dpiX)
            {
                case 96:
                case 120:
                    {
                        dpi = (float)(Others.isWin11() == true ? 1.5 : 1);
                        break;
                    }
                case 144:
                    {
                        dpi = (float)1.5;
                        break;
                    }
                case 168:
                    {
                        dpi = (float)1.75;
                        break;
                    }
                case 192:
                    {
                        dpi = (float)2;
                        break;
                    }
            }
            return dpi;

        }

        
    }
}
