using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestModel;
using System.Windows.Forms;
using System.Threading;
using TestTool;
using System.IO.Ports;
using WebService;
using System.Reflection.Emit;

namespace TestDAL
{
    public class OperateBES
    {
        public SerialOperate Serial;
        private string port;
        public string btAddress = string.Empty;
        private Queue<string> queue, statusQueue;
        public DataBase dataBase;
        public ConfigData config;
        private WebReference.WebService1 MesWeb;
        public SerialPort FixPort;
        public string PackSN;
        public string BurnINFail;
        public string firstMac, secondMac, oneMac, twoMac, threeMac, fourMac;
        public bool isMainEar;
        public List<TestData> TestItmes;

        public OperateBES(ConfigData config, Queue<string> queue, Queue<string> statusQueue)
        {
            this.config = config;
            Serial = new SerialOperate(config, queue);
            this.queue = queue;
            this.statusQueue = statusQueue;
            //queue.Enqueue(string.Format("串口{0}打开成功", port));
        }

        public TestData OpenSerialPort(TestData data)
        {
            try

            {
                Serial.OpenSerialPort(int.Parse(data.Other));

                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
            }
            return data;
        }

        public TestData OpenOneLineSerialPort(TestData data)
        {
            try

            {
                Serial.OpenOneLineSerialPort();

                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
            }
            return data;
        }

        public TestData OpenOppoSerialPort(TestData data)
        {
            try

            {
                Serial.OpenOppoSerialPort();

                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
            }
            return data;
        }

        public TestData ClosedSerialPort(TestData data)
        {
            try
            {
                Serial.ClosedPort();
                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
            }
            return data;
        }

        public TestData PublicByteSerialPortCommand(TestData data)
        {
            try
            {
                //指令;跳过几个byte;第几位byte是数据长度;ASCII/16/10;true
                string[] comm = data.Other.Split(';');

                string[] bytes = comm[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] val = new byte[bytes.Length];
                byte[] needData;
                for (int i = 0; i < bytes.Length; i++)
                {
                    val[i] = Convert.ToByte(bytes[i], 16);
                }
                int skip = int.Parse(comm[1]);
                int length, offset = 0;
                if (comm[2].Contains(","))
                {
                    length = int.Parse(comm[2].Split(',')[0]);
                    offset = int.Parse(comm[2].Split(',')[1]);
                }
                else
                {
                    length = int.Parse(comm[2]);
                }
                bool reture = bool.Parse(comm[4]);

                if (reture)
                {
                    byte[] values = null;
                    if (comm.Length == 6)
                    {
                        values = Serial.VisaQuery(val, int.Parse(comm[5]));
                    }
                    else
                    {
                        values = Serial.VisaQuery(val);
                    }
                    if (values[0] != 0)
                    {
                        needData = values.Skip(skip).Take(length - offset).ToArray();
                        if (needData.Length != length)
                        {
                            int empty = length - needData.Length;
                            List<byte> list = needData.ToList();
                            for (int i = 0; i < empty; i++)
                            {
                                list.Add(0);
                            }
                            needData = list.ToArray();
                        }
                        Others.WriteInformationLog("Cut Out--->" + SerialOperate.ToHexStrFromByte(needData));

                        string da = string.Empty;
                        if (comm[3] == "ASCII")
                        {

                            da = Encoding.ASCII.GetString(needData).Trim().ToUpper();
                            if (da.Contains("\r\n"))
                            {
                                da = da.Split('\r')[0];
                            }
                            else if (da.Contains("\n"))
                            {
                                da = da.Split('\n')[0];
                            }

                        }
                        else if (comm[3] == "16")
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < needData.Length; i++)
                            {
                                string bt = needData[i].ToString("x");
                                sb.Append(bt.Length == 1 ? "0" + bt : bt);
                            }
                            da = sb.ToString().ToUpper().Trim();
                        }
                        else if (comm[3] == "10")
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < needData.Length; i++)
                            {
                                sb.Append(needData[i].ToString());
                            }
                            da = sb.ToString().ToUpper();
                        }
                        else if (comm[3] == "voltage")
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < needData.Length; i++)
                            {
                                string bt = needData[i].ToString("x");
                                sb.Append(bt.Length == 1 ? "0" + bt : bt);
                            }
                            da = int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                        }
                        else if (comm[3] == "bt")
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = needData.Length - 1; i >= 0; i--)
                            {
                                string bt = needData[i].ToString("x");
                                sb.Append(bt.Length == 1 ? "0" + bt : bt);
                            }
                            da = sb.ToString().ToUpper();

                        }
                        else if (comm[3] == "bt1")
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < needData.Length; i++)
                            {
                                string bt = needData[i].ToString("x");
                                sb.Append(bt.Length == 1 ? "0" + bt : bt);
                            }
                            da = sb.ToString().ToUpper().Trim();

                            //if (Convert.ToInt64(da, 16) == 0)
                            //{
                            //    data.Value = da;
                            //    data.Result = "Fail";
                            //}
                        }
                        else if (comm[3] == "average")
                        {
                            List<int> list = new List<int>();

                            list.Add(BitConverter.ToInt32(new byte[]
                            {
                            needData[0],
                            needData[1],
                            needData[2],
                            needData[3]
                            }, 0));
                            list.Add(BitConverter.ToInt32(new byte[] { needData[4], needData[5], needData[6], needData[7] }, 0));
                            list.Add(BitConverter.ToInt32(new byte[] { needData[8], needData[9], needData[10], needData[11] }, 0));


                            //StringBuilder sb = new StringBuilder();
                            //for (int i = 0; i < needData.Length; i++)
                            //{
                            //    string bt = needData[i].ToString("x");
                            //    sb.Append(bt.Length == 1 ? "0" + bt : bt);
                            //}
                            //da = int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();

                            ////list.Add();
                            //double total = needData.Average(num => Convert.ToInt16(num));
                            da = Math.Round(list.Average(), 0).ToString();
                            ////var total = needData.ToList().Average()<int>;
                            data.Value = da;
                            data.Result = "Pass";
                            //if (total >= double.Parse(data.LowLimit)
                            //   && num <= double.Parse(data.UppLimit))
                            //{
                            //    data.Value = da;
                            //    data.Result = "Pass";
                            //}
                            //else
                            //{
                            //    data.Value = da;
                            //    data.Result = "Fail";
                            //}
                        }
                        else if(comm[3] == "utf".ToUpper())
                        {
                            da = Encoding.GetEncoding("utf-8").GetString(needData);
                        }
                        if (data.UppLimit == "NA")
                        {
                            if (data.LowLimit.Equals(da))
                            {
                                data.Result = "Pass";
                                data.Value = da;
                            }
                            else
                            {
                                data.Result = "Fail";
                                data.Value = da;
                            }
                        }
                        else
                        {
                            double num = double.Parse(da) + double.Parse(data.FillValue);
                            if (num >= double.Parse(data.LowLimit)
                                && num <= double.Parse(data.UppLimit))
                            {
                                data.Value = num.ToString();
                                data.Result = "Pass";
                            }
                            else
                            {
                                data.Value = num.ToString();
                                data.Result = "Fail";
                            }

                            if (needData.Length == 2 && comm[3] != "voltage")
                            {
                                int Lbatt = int.Parse(needData[0].ToString());
                                int Rbatt = int.Parse(needData[1].ToString());
                                if ((Lbatt >= double.Parse(data.LowLimit)
                                 && Lbatt <= double.Parse(data.UppLimit))
                                 && (Rbatt >= double.Parse(data.LowLimit)
                                 && Rbatt <= double.Parse(data.UppLimit)))
                                {
                                    data.Value = string.Format("{0},{1}", Lbatt, Rbatt);
                                    data.Result = "Pass";
                                }
                                else
                                {
                                    data.Value = string.Format("{0},{1}", Lbatt, Rbatt);
                                    data.Result = "Fail";
                                }
                            }
                        }
                        if (data.LowLimit == "NA" && data.UppLimit == "NA")
                        {
                            if (comm[3] == "bt1" || comm[3] == "bt")
                            {
                                if (Convert.ToInt64(da, 16) == 0)
                                {
                                    data.Value = da;
                                    data.Result = "Fail";
                                }
                                else
                                {
                                    data.Value = da;
                                    data.Result = "Pass";
                                }
                            }
                            else
                            {
                                data.Value = da;
                                data.Result = "Pass";
                            }
                        }

                        if (config.MesEnable && data.TestItemName.Contains("蓝牙地址"))
                        {
                            WebService.WebService1 web = new WebService1();
                            string ret = web.P_JK_BCP(da);
                            if (ret.Contains("OK"))
                            {
                                data.Value = da;
                                data.Result = "Pass";
                            }
                            else
                            {
                                data.Value = ret;
                                data.Result = "Fail";
                            }
                            web.Dispose();
                        }
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    Serial.VisaWrite(val);
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData PublicAsciiSerialPortCommand(TestData data)
        {
            try
            {
                //指令;匹配字符;分割符;是否返回;读取延迟时间;是否写入;写入位置;写入数据;是否比对扫描SN
                string[] comm = data.Other.Split(';');

                string cmd = comm[0];
                string match = comm[1];
                char split = char.Parse(comm[2].Contains("\\n") ? "\n" : comm[2]);
                //string split = comm[2] == "\\r\\n" ? Environment.NewLine : comm[2];
                bool reture = bool.Parse(comm[3]);
                int delay = int.Parse(comm[4]);
                bool isWrite = bool.Parse(comm[5]);
                string local = comm[6];
                string writeData = comm[7];
                bool isScan = bool.Parse(comm[8]);
                if (reture)
                {
                    if (isWrite && writeData == "")
                    {
                        cmd += PackSN;
                    }

                    else
                    {
                        cmd += writeData;
                    }
                    string values = string.Empty;
                    for (int i = 0; i < 3; i++)
                    {
                        values = string.Empty;
                        Others.WriteInformationLog(string.Format(
                            "Send--->{0}，第{1}次", cmd, i + 1));
                        values = Serial.VisaQuery(cmd, delay);
                        if (values.Contains("OK") || values.Contains("PASS"))
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    string value = values.StartsWith(cmd)
                           ? values.Remove(0, cmd.Length) : values;
                    //string needData = values.Substring(skip, length - offset);
                    Others.WriteInformationLog("Reply--->" + values);
                    List<string> list = value.Split(split).ToList();
                    string need = string.Empty;
                    need = list.Where(s => s.Contains(match)).ToList()[0];
                    if (need.Contains("OK") || need.Contains("PASS"))
                    {
                        need = need.Trim();
                    }
                    else
                    {
                        need = need.Replace(match, "").Trim();
                    }
                    Others.WriteInformationLog("Need--->" + need);
                    if (isWrite)
                    {
                        if (need == match)
                        {
                            data.Value = need;
                            data.Result = "Pass";
                        }
                        else
                        {
                            data.Value = need;
                            data.Result = "Fail";
                        }
                    }
                    else
                    {
                        if (data.UppLimit == "NA" && data.LowLimit != "NA")
                        {
                            if (data.LowLimit == need)
                            {
                                data.Value = need;
                                data.Result = "Pass";
                            }
                            else
                            {
                                data.Result = "Fail";
                                data.Value = need;
                            }
                        }
                        else if (data.LowLimit != "NA" && data.UppLimit != "NA")
                        {
                            double num = double.Parse(need);
                            if (num >= double.Parse(data.LowLimit)
                                && num <= double.Parse(data.UppLimit))
                            {
                                data.Value = need;
                                data.Result = "Pass";
                            }
                            else
                            {
                                data.Value = need;
                                data.Result = "Fail";
                            }

                        }
                        else if (data.LowLimit == "NA" && data.UppLimit == "NA")
                        {
                            if (!isScan)
                            {
                                data.Value = need;
                                data.Result = "Pass";
                            }
                            else
                            {
                                if (need.Equals(PackSN))
                                {
                                    data.Value = need;
                                    data.Result = "Pass";
                                }

                                else
                                {
                                    data.Value = need;
                                    data.Result = "Fail";
                                }
                            }
                        }
                        else
                        {
                            data.Value = need;
                            data.Result = "Pass";
                        }
                    }
                }
                else
                {
                    Serial.VisaQuery(cmd, delay);
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData PublicAsciiSerialPortNormalCommand(TestData data)
        {
            try
            {
                //指令;跳过几个byte;第几位byte是数据长度;ASCII/16/10;true
                string[] comm = data.Other.Split(';');

                string[] bytes = comm[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] val = new byte[bytes.Length];
                byte[] needData;
                for (int i = 0; i < bytes.Length; i++)
                {
                    val[i] = Convert.ToByte(bytes[i], 16);
                }
                int skip = int.Parse(comm[1]);
                int length, offset = 0;
                if (comm[2].Contains(","))
                {
                    length = int.Parse(comm[2].Split(',')[0]);
                    offset = int.Parse(comm[2].Split(',')[1]);
                }
                else
                {
                    length = int.Parse(comm[2]);
                }
                bool reture = bool.Parse(comm[4]);

                if (reture)
                {
                    byte[] values = null;
                    if (comm.Length == 6)
                    {
                        values = Serial.VisaQuery(val, int.Parse(comm[5]));
                    }
                    else
                    {
                        values = Serial.VisaQuery(val);
                    }
                    needData = values.Skip(skip).Take(length - offset).ToArray();
                    if (needData.Length != length)
                    {
                        int empty = length - needData.Length;
                        List<byte> list = needData.ToList();
                        for (int i = 0; i < empty; i++)
                        {
                            list.Add(0);
                        }
                        needData = list.ToArray();
                    }
                    Others.WriteInformationLog("Cut Out--->" + SerialOperate.ToHexStrFromByte(needData));

                    string da = string.Empty;
                    if (comm[3] == "ASCII")
                    {

                        da = Encoding.ASCII.GetString(needData).Trim().ToUpper();
                        if (da.Contains("\r\n"))
                        {
                            da = da.Split('\r')[0];
                        }
                        else if (da.Contains("\n"))
                        {
                            da = da.Split('\n')[0];
                        }

                    }
                    else if (comm[3] == "16")
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < needData.Length; i++)
                        {
                            string bt = needData[i].ToString("x");
                            sb.Append(bt.Length == 1 ? "0" + bt : bt);
                        }
                        da = sb.ToString().ToUpper().Trim();
                    }
                    else if (comm[3] == "10")
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < needData.Length; i++)
                        {
                            sb.Append(needData[i].ToString());
                        }
                        da = sb.ToString().ToUpper();
                    }
                    else if (comm[3] == "voltage")
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < needData.Length; i++)
                        {
                            string bt = needData[i].ToString("x");
                            sb.Append(bt.Length == 1 ? "0" + bt : bt);
                        }
                        da = int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                    }
                    else if (comm[3] == "bt")
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = needData.Length - 1; i >= 0; i--)
                        {
                            string bt = needData[i].ToString("x");
                            sb.Append(bt.Length == 1 ? "0" + bt : bt);
                        }
                        da = sb.ToString().ToUpper();

                    }
                    else if (comm[3] == "bt1")
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < needData.Length; i++)
                        {
                            string bt = needData[i].ToString("x");
                            sb.Append(bt.Length == 1 ? "0" + bt : bt);
                        }
                        da = sb.ToString().ToUpper().Trim();

                        //if (Convert.ToInt64(da, 16) == 0)
                        //{
                        //    data.Value = da;
                        //    data.Result = "Fail";
                        //}
                    }
                    else if (comm[3] == "average")
                    {
                        List<int> list = new List<int>();

                        list.Add(BitConverter.ToInt32(new byte[]
                        {
                            needData[0],
                            needData[1],
                            needData[2],
                            needData[3]
                        }, 0));
                        list.Add(BitConverter.ToInt32(new byte[] { needData[4], needData[5], needData[6], needData[7] }, 0));
                        list.Add(BitConverter.ToInt32(new byte[] { needData[8], needData[9], needData[10], needData[11] }, 0));


                        //StringBuilder sb = new StringBuilder();
                        //for (int i = 0; i < needData.Length; i++)
                        //{
                        //    string bt = needData[i].ToString("x");
                        //    sb.Append(bt.Length == 1 ? "0" + bt : bt);
                        //}
                        //da = int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();

                        ////list.Add();
                        //double total = needData.Average(num => Convert.ToInt16(num));
                        da = Math.Round(list.Average(), 0).ToString();
                        ////var total = needData.ToList().Average()<int>;
                        data.Value = da;
                        data.Result = "Pass";
                        //if (total >= double.Parse(data.LowLimit)
                        //   && num <= double.Parse(data.UppLimit))
                        //{
                        //    data.Value = da;
                        //    data.Result = "Pass";
                        //}
                        //else
                        //{
                        //    data.Value = da;
                        //    data.Result = "Fail";
                        //}
                    }

                    if (data.UppLimit == "NA")
                    {
                        if (data.LowLimit.Equals(da))
                        {
                            data.Result = "Pass";
                            data.Value = da;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = da;
                        }
                    }
                    else
                    {
                        double num = double.Parse(da) + double.Parse(data.FillValue);
                        if (num >= double.Parse(data.LowLimit)
                            && num <= double.Parse(data.UppLimit))
                        {
                            data.Value = num.ToString();
                            data.Result = "Pass";
                        }
                        else
                        {
                            data.Value = num.ToString();
                            data.Result = "Fail";
                        }

                        if (needData.Length == 2 && comm[3] != "voltage")
                        {
                            int Lbatt = int.Parse(needData[0].ToString());
                            int Rbatt = int.Parse(needData[1].ToString());
                            if ((Lbatt >= double.Parse(data.LowLimit)
                             && Lbatt <= double.Parse(data.UppLimit))
                             && (Rbatt >= double.Parse(data.LowLimit)
                             && Rbatt <= double.Parse(data.UppLimit)))
                            {
                                data.Value = string.Format("{0},{1}", Lbatt, Rbatt);
                                data.Result = "Pass";
                            }
                            else
                            {
                                data.Value = string.Format("{0},{1}", Lbatt, Rbatt);
                                data.Result = "Fail";
                            }
                        }
                    }
                    if (data.LowLimit == "NA" && data.UppLimit == "NA")
                    {
                        if (comm[3] == "bt1" || comm[3] == "bt")
                        {
                            if (Convert.ToInt64(da, 16) == 0)
                            {
                                data.Value = da;
                                data.Result = "Fail";
                            }
                            else
                            {
                                data.Value = da;
                                data.Result = "Pass";
                            }
                        }
                        else
                        {
                            data.Value = da;
                            data.Result = "Pass";
                        }
                    }

                    if (config.MesEnable && data.TestItemName.Contains("蓝牙地址"))
                    {
                        WebService.WebService1 web = new WebService1();
                        string ret = web.P_JK_BCP(da);
                        if (ret.Contains("OK"))
                        {
                            data.Value = da;
                            data.Result = "Pass";
                        }
                        else
                        {
                            data.Value = ret;
                            data.Result = "Fail";
                        }
                        web.Dispose();
                    }
                }
                else
                {
                    Serial.VisaWrite(val);
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData PublicWriteSerialNumber(TestData data)
        {
            try
            {
                //4C 43 14 00 20;true;AA;3;1;16
                List<byte> total = new List<byte>();
                string[] comm = data.Other.Split(';');

                string[] bytes = comm[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < bytes.Length; i++)
                {
                    total.Add(Convert.ToByte(bytes[i], 16));
                }
                PackSN = PackSN.StartsWith("SN:") ? PackSN.Substring(3) : PackSN;
                total.AddRange(Encoding.ASCII.GetBytes(PackSN).ToList());

                if (bool.Parse(comm[1]))
                {
                    byte end = Convert.ToByte(comm[2], 16);
                    total.Add(end);
                }


                int skip, length, offset = 0;

                skip = int.Parse(comm[3]);
                length = int.Parse(comm[4]);

                byte[] values = Serial.VisaQuery(total.ToArray());
                byte[] needData = values.Skip(skip).Take(length - offset).ToArray();
                Others.WriteInformationLog("Cut Out--->" + SerialOperate.ToHexStrFromByte(needData));
                string da = string.Empty;

                if (comm[5] == "ASCII")
                {

                    da = Encoding.ASCII.GetString(needData).Trim().ToUpper();
                    if (da.Contains("\r\n"))
                    {
                        da = da.Split('\r')[0];
                    }
                    else if (da.Contains("\n"))
                    {
                        da = da.Split('\n')[0];
                    }

                }
                else if (comm[5] == "16")
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        string bt = needData[i].ToString("x");
                        sb.Append(bt.Length == 1 ? "0" + bt : bt);
                    }
                    da = sb.ToString().ToUpper().Trim();
                }
                else if (comm[5] == "10")
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        sb.Append(needData[i].ToString());
                    }
                    da = sb.ToString().ToUpper();
                }

                if (data.UppLimit == "NA")
                {
                    if (data.LowLimit.Equals(da))
                    {
                        data.Result = "Pass";
                        data.Value = da;
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = da;
                    }
                }
            }
            catch (Exception EX)
            {

                throw EX;
            }

            return data;
        }

        public TestData PublicReadAndCompareSerialNumber(TestData data)
        {
            try
            {
                //指令;跳过几个byte;第几位byte是数据长度;ASCII/16/10;true
                string[] comm = data.Other.Split(';');

                string[] bytes = comm[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] val = new byte[bytes.Length];
                byte[] needData;
                for (int i = 0; i < bytes.Length; i++)
                {
                    val[i] = Convert.ToByte(bytes[i], 16);
                }
                int skip = int.Parse(comm[1]);
                int length, offset = 0;
                if (comm[2].Contains(","))
                {
                    length = int.Parse(comm[2].Split(',')[0]);
                    offset = int.Parse(comm[2].Split(',')[1]);
                }
                else
                {
                    length = int.Parse(comm[2]);
                }

                byte[] values = Serial.VisaQuery(val);
                needData = values.Skip(skip).Take(length - offset).ToArray();
                Others.WriteInformationLog("Cut Out--->" + SerialOperate.ToHexStrFromByte(needData));

                string da = string.Empty;
                if (comm[3] == "ASCII")
                {

                    da = Encoding.ASCII.GetString(needData).Trim().ToUpper();
                    if (da.Contains("\r\n"))
                    {
                        da = da.Split('\r')[0];
                    }
                    else if (da.Contains("\n"))
                    {
                        da = da.Split('\n')[0];
                    }

                }
                else if (comm[3] == "16")
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        string bt = needData[i].ToString("x");
                        sb.Append(bt.Length == 1 ? "0" + bt : bt);
                    }
                    da = sb.ToString().ToUpper().Trim();
                }
                else if (comm[3] == "10")
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        sb.Append(needData[i].ToString());
                    }
                    da = sb.ToString().ToUpper();
                }
                PackSN = PackSN.StartsWith("SN:") ? PackSN.Substring(3) : PackSN;
                if (PackSN.Equals(da))
                {
                    data.Result = "Pass";
                    data.Value = da;
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = da;
                }

            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData CompareMacAddress(TestData data)
        {
            try
            {
                //var numbers = Array.ConvertAll(data.Other.Split(';'), int.Parse);
                var numbers = data.Other.Split(';');
                string mac1 = string.Empty;
                string mac2 = string.Empty;
                string mac3 = string.Empty;
                string mac4 = string.Empty;
                if (numbers.Length == 2)
                {
                    mac1 = TestItmes[int.Parse(numbers[0]) - 1].Value;
                    mac2 = TestItmes[int.Parse(numbers[1]) - 1].Value;
                    queue.Enqueue("Mac1 is " + mac1);
                    queue.Enqueue("Mac2 is " + mac2);

                    if (data.LowLimit == "NA")
                    {
                        if (mac1 == "00" || mac2 == "00")
                        {
                            data.Value = mac1;
                            data.Result = "Fail";
                        }
                        else
                        {
                            if (mac1.Equals(mac2))
                            {
                                data.Value = "Pass";
                                data.Result = "Pass";
                            }
                            else
                            {
                                data.Value = mac1;
                                data.Result = "Fail";
                            }
                        }
                    }
                    else
                    {
                        if (mac1.StartsWith(data.LowLimit) && mac1.StartsWith(data.LowLimit))
                        {
                            data.Value = "Pass";
                            data.Result = "Pass";
                        }
                    }
                }

                else if(numbers.Length == 3)
                {
                    mac1 = TestItmes[int.Parse(numbers[0]) - 1].Value;
                    mac2 = TestItmes[int.Parse(numbers[1]) - 1].Value;
                    queue.Enqueue("value1 is " + mac1);
                    queue.Enqueue("value2 is " + mac2);
                    double one = double.Parse(mac1 == "" ? "0" : mac1);
                    double two = double.Parse(mac2 == "" ? "0" : mac2);
                    double total = 0;
                    switch (numbers[2].ToString())
                    {
                        case "+":
                            {
                               total = one + two;
                                break;
                            }
                        case "-":
                            {
                                total = one - two;
                                break;
                            }
                        case "*":
                            {
                                total = one * two;
                                break;
                            }
                        case "/":
                            {
                                total = one / two;
                                break;
                            }
                    }

                    if (total >= double.Parse(data.LowLimit)
                            && total <= double.Parse(data.UppLimit))
                    {
                        data.Value = total.ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = total.ToString();
                        data.Result = "Fail";
                    }
                }

                else
                {
                    mac1 = TestItmes[int.Parse(numbers[0]) - 1].Value;
                    mac2 = TestItmes[int.Parse(numbers[1]) - 1].Value;
                    mac3 = TestItmes[int.Parse(numbers[2]) - 1].Value;
                    mac4 = TestItmes[int.Parse(numbers[3]) - 1].Value;
                    //mac1 = "B49A95B5FE80";
                    //mac2 = "B49A95B5FE80";
                    //mac3 = "B49A95B60A32";
                    //mac4 = "B49A95B5FE80";
                    queue.Enqueue("Mac1 is " + mac1);
                    queue.Enqueue("Mac2 is " + mac2);
                    queue.Enqueue("Mac3 is " + mac3);
                    queue.Enqueue("Mac4 is " + mac4);

                    if ((!mac1.StartsWith("0000") && !mac2.StartsWith("0000")) &&
                       (!mac3.StartsWith("0000") && !mac4.StartsWith("0000")))
                    {
                        bool tws = mac1.Equals(mac2);
                        bool local = !mac3.Equals(mac4);
                        //bool one = mac1.Equals(mac3);
                        //bool two = mac1.Equals(mac4);
                        //bool three = mac2.Equals(mac3);
                        //bool four = mac2.Equals(mac4);
                        if (tws && local)
                        {
                            //if ((one || two) || (three || four))
                            //{
                            //    data.Value = mac1;
                            //    data.Result = "Pass";
                            //}
                            if (mac1.Equals(mac3) || mac1.Equals(mac4))
                            {
                                data.Value = mac1;
                                data.Result = "Pass";
                            }
                            else
                            {
                                data.Value = mac1;
                                data.Result = "Fail";
                            }
                        }
                        else
                        {
                            data.Value = mac1;
                            data.Result = "Fail";
                        }
                    }
                    else
                    {
                        data.Value = mac1;
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public TestData EnterProductLineTestMode(TestData data)
        {
            try
            {
                //54 41 AA 5A 9D 18
                //54 41 AA 5A 9D 18
                //51 41 54 5A 16 94 
                byte[] bytes = { 0x54, 0x41, 0xAA, 0x5A, 0x9D, 0x18 };
                Thread.Sleep(200);
                for (int i = 0; i < 15; i++)
                {
                    byte[] values = Serial.VisaQuery(bytes);
                    //if(ByteArrayCompare(values,ret))
                    if (values[0] == 0x51 && values[1] == 0x41)
                    {
                        data.Value = "Pass";
                        data.Result = "Pass";
                        break;
                    }
                    else
                    {
                        data.Value = "Fail";
                        data.Result = "Fail";
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData EnterProductLineTestModeRealMe(TestData data)
        {
            try
            {
                string[] times = data.Other.Split(';');
                //04 FB B0 00 01 01
                byte[] bytes = { 0x04, 0xFB, 0xB0, 0x00, 0x01, 0x01 };
                Thread.Sleep(200);
                for (int i = 0; i < 15; i++)
                {
                    byte[] values = Serial.VisaQuery(bytes, int.Parse(times[0]));
                    //if(ByteArrayCompare(values,ret))
                    //TestTool.Others.WriteInformationLog("Send--->" +SerialOperate.ToHexStrFromByte(values));
                    if (values[0] == 0xFB && values[1] == 0x04)
                    {
                        data.Value = "Pass";
                        data.Result = "Pass";
                        break;
                    }
                    else
                    {
                        data.Value = "Fail";
                        data.Result = "Fail";
                    }
                    Thread.Sleep(times[1] == "" ? 200 : int.Parse(times[1]));
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData Readx5sBattaryLevel(TestData data)
        {
            try
            {
                //54 C7 01 AA 5A 8B 51
                byte[] bytes = { 0x54, 0xC7, 0x01, 0xAA, 0x5A, 0x8B,0x51 };
                Thread.Sleep(200);
                for (int i = 0; i < 15; i++)
                {
                    byte[] values = Serial.VisaQuery(bytes);
                    //if(ByteArrayCompare(values,ret))
                    if ((values[0] == 0x51 && values[1] == 0xC7) && values.Length >=4)
                    {

                        int num = values[3];
                        if (num >= double.Parse(data.LowLimit)
                            && num <= double.Parse(data.UppLimit))
                        {
                            data.Value = num.ToString();
                            data.Result = "Pass";
                        }
                        else
                        {
                            data.Value = num.ToString(); 
                            data.Result = "Fail";
                        }
                        break;
                    }
                    else
                    {
                        data.Value = "Fail";
                        data.Result = "Fail";
                    }
                    Thread.Sleep(500);
                }


            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData ReadSoftVersion(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x02, 0x01, 0x00 };
                Thread.Sleep(500);
                byte[] values = Serial.VisaQuery(bytes);
                byte[] needData = values.Skip(3).Take(values[2]).ToArray();

                var version = Encoding.ASCII.GetString(needData).Trim();
                if (data.LowLimit.Equals(version))
                {
                    data.Result = "Pass";
                    data.Value = version;
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = version;
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData ReadHWVersion(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x03, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                // int length = Convert.ToInt16(values[2]);
                // var version = Encoding.ASCII.GetString(values).Replace("\0", "")
                //  .Remove(0, 3);

                StringBuilder version = new StringBuilder();
                for (int i = 3; i < 11; i++)
                {
                    version.Append(Convert.ToInt16(values[i]));
                }
                if (data.LowLimit.Equals(version.ToString()))
                {
                    data.Result = "Pass";
                    data.Value = version.ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = version.ToString();
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }

            return data;
        }

        public TestData ReadHWVersion_ASCII(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x03, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                // int length = Convert.ToInt16(values[2]);
                byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                var version = Encoding.ASCII.GetString(needData);

                //StringBuilder version = new StringBuilder();
                //for (int i = 3; i < 11; i++)
                //{
                //    version.Append(Convert.ToInt16(values[i]));
                //}
                if (data.LowLimit.Equals(version.ToString()))
                {
                    data.Result = "Pass";
                    data.Value = version.ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = version.ToString();
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }

            return data;
        }

        public TestData BES_WhiteLight(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x0e, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x00 && values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_RedLight(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x0f, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x00 && values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadSN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x00, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                //\u0004\u001020225719ag000001
                if (values[0] == 0x04)
                {
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    string sn = Encoding.ASCII.GetString(needData);

                    //string reslut = string.Empty;

                    //if (config.MesEnable)
                    //{
                    //    MesWeb = new WebReference.WebService1();
                    //    if (PackSN.Length == 20)
                    //    {
                    //        reslut = MesWeb.SnCx_BZSN(sn);
                    //        if (reslut == "P")
                    //        {
                    //            queue.Enqueue("产品SN未重复");
                    //        }
                    //    }
                    //    //else
                    //    //{
                    //    //    reslut = MesWeb.SnCx_sn(sn);
                    //    //    if (reslut == "P")
                    //    //    {
                    //    //        queue.Enqueue("产品SN未重复");
                    //    //    }
                    //    //}
                    //    MesWeb.Abort();
                    //}
                    //else
                    //{
                    //    //检查组段SN是否重复
                    //    reslut = "P";
                    //    //MesWeb.Abort();
                    //}
                    //if (reslut.Contains("P"))
                    //{
                    data.Result = "Pass";
                    data.Value = sn;
                    //}
                    //else
                    //{
                    //    queue.Enqueue("产品SN重复，请检查");
                    //    data.Result = "Fail";
                    //    data.Value = sn;
                    //}
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_CompareSN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x00, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                //\u0004\u001020225719ag000001
                if (values[0] == 0x04)
                {
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    string sn = Encoding.ASCII.GetString(needData);
                    string reslut = string.Empty;

                    #region 取消
                    //if (config.MesEnable )
                    //{
                    //    MesWeb = new WebReference.WebService1();
                    //    if (PackSN.Length == 20)
                    //    {
                    //        //sn = "GJ1225202K100773";
                    //        //检查包装段SN是否重复
                    //        reslut = MesWeb.SnCx_BZSN(sn);
                    //        if (reslut == "P")
                    //        {
                    //            queue.Enqueue("产品SN未重复");
                    //        }
                    //    }
                    //    //else
                    //    //{
                    //    //    //检查组段SN是否重复
                    //    //    reslut = MesWeb.SnCx_sn(sn);
                    //    //    if (reslut == "P")
                    //    //    {
                    //    //        queue.Enqueue("产品SN未重复");
                    //    //    }
                    //    //}
                    //    MesWeb.Abort();
                    //}
                    //else
                    //{
                    //    reslut = "P";
                    //    //MesWeb.Abort();
                    //}
                    //if (reslut.Contains("P"))
                    //{
                    #endregion

                    if (btAddress != "")
                    {
                        if (btAddress == sn && btAddress.StartsWith(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = sn;
                        }
                        else
                        {
                            queue.Enqueue("产品SN重复和电池SN比对失败，" +
                                "请勿在写入电池SN的时候比对产品SN");
                            data.Result = "Fail";
                            data.Value = sn;
                            if (data.Check)
                            {
                                Serial.ClosedPort();
                            }
                        }
                    }
                    else
                    {
                        if (sn.StartsWith(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = sn;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = sn;
                            if (data.Check)
                            {
                                Serial.ClosedPort();
                            }
                        }
                    }
                }
                //    else
                //    {
                //        queue.Enqueue("产品SN重复，请检查");
                //        data.Result = "Fail";
                //        data.Value = sn;
                //    }
                //}
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadBTName(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x04, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x00 && values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadBTAddress(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x05, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x06)
                {
                    string btaddress = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                        , values[8], values[7], values[6], values[5], values[4], values[3]).ToUpper();
                    data.Value = btaddress;
                    data.Result = "Pass";
                    //////string btaddress = "FFFFFFFFFFFF";
                    //if (btaddress.StartsWith(data.LowLimit))
                    //{
                    //    if (config.MesEnable)
                    //    {
                    //        //btaddress = "E09DFA6A1BA1";
                    //        queue.Enqueue("检查蓝牙地址是否过站/重复/测试三次:" + btaddress);
                    //        //ServiceReference1.WebService1SoapClient MesWeb = null;
                    //        //MesWeb = new ServiceReference1.WebService1SoapClient("WebService1Soap");
                    //        MesWeb = new WebReference.WebService1();
                    //        //MesWeb.Url = "http://218.65.34.28:82/WebService1.asmx";
                    //        //btaddress = "E09DFA6A1BB3";
                    //        string failResult = string.Empty;
                    //        string reslut = string.Empty;
                    //        //string packResult = string.Empty;
                    //        string btResult = string.Empty;
                    //        if (PackSN == null)
                    //            PackSN = string.Empty;

                    //        if (PackSN.Length != config.SNLength)
                    //        {
                    //            //btaddress = "E09DFA513DF6";
                    //            //半成品工站拦截蓝牙地址重复
                    //            //btResult = MesWeb.SnCx_LY(btaddress);
                    //            //检查上一工站是否Pass
                    //            reslut = MesWeb.SnCx(btaddress, config.MesStation);
                    //            //检查是否测试三次
                    //            failResult = MesWeb.SnCx_SC(btaddress, config.NowStation);
                    //        }
                    //        else
                    //        {
                    //            //reslut = MesWeb.SnCx(btaddress, config.MesStation);
                    //            //已在输入20位SN时检测
                    //            reslut = "P";
                    //            //检查是否测试三次
                    //            failResult = MesWeb.SnCx_SC(btaddress, config.NowStation);
                    //            //检查包装段蓝牙地址是否重复
                    //            //btResult = MesWeb.SnCx_BZLY(btaddress);
                    //        }

                    //        //MesWeb.Close();
                    //        MesWeb.Abort();

                    //        if (reslut.Contains("F"))
                    //        {
                    //            data.Value = btaddress;
                    //            data.Result = "Fail";
                    //            queue.Enqueue(string.Format("上一个工位:{0}:连续测试NG品,请检查 "
                    //        , config.MesStation));
                    //        }
                    //        else
                    //        {
                    //            queue.Enqueue("检查蓝牙地址: " + btaddress + ",过站Pass");
                    //            data.Result = "Pass";
                    //            data.Value = btaddress;
                    //            if (PackSN.Length != config.SNLength)
                    //            {
                    //                if (failResult.Contains("P"))
                    //                {
                    //                    queue.Enqueue("该蓝牙地址: " + btaddress + ",无重复测试");
                    //                    data.Result = "Pass";
                    //                    data.Value = btaddress;
                    //                }
                    //                else
                    //                {
                    //                    data.Value = btaddress;
                    //                    data.Result = "Fail";
                    //                    queue.Enqueue("该蓝牙地址: " + btaddress + ",重复测试三次");
                    //                }
                    //            }
                    //        }
                    //        //if (btResult != string.Empty)
                    //        //{
                    //        //    if (btResult.Contains("P"))
                    //        //    {
                    //        //        queue.Enqueue("该蓝牙地址: " + btaddress + ",无重复");
                    //        //        data.Result = "Pass";
                    //        //        data.Value = btaddress;
                    //        //    }
                    //        //    else
                    //        //    {
                    //        //        data.Value = btaddress;
                    //        //        data.Result = "Fail";
                    //        //        queue.Enqueue("该蓝牙地址: " + btaddress + ",重复,请检查");
                    //        //    }
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        data.Result = "Pass";
                    //        data.Value = btaddress;
                    //    }
                    //}
                    //else
                    //{
                    //    data.Result = "Fail";
                    //    data.Value = btaddress;
                    //}
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadBattarySN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x06, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                string batt = Encoding.ASCII.GetString(needData);
                //if (config.MesEnable)
                //{
                //    string res = MesWeb.SnCx_DC(batt);
                //    if (res != "P")
                //    {
                //        data.Result = "Fail";
                //        data.Value = batt;
                //        queue.Enqueue("电池SN:" + btAddress + "重复，请检查");
                //    }
                //    else
                //    {
                //        data.Result = "Pass";
                //        data.Value = batt;
                //        queue.Enqueue("电池SN无重复");
                //    }

                //}
                //else
                //{
                data.Result = "Pass";
                data.Value = batt;
                //}
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_CompareBattarySN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x06, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                string batt = Encoding.ASCII.GetString(needData);

                #region 取消
                //if (config.MesEnable)
                //{
                //    //batt = "SYN0C1909049061";
                //    string res = MesWeb.SnCx_DC(batt);
                //    if (res != "P")
                //    {
                //        data.Result = "Fail";
                //        data.Value = batt;
                //        queue.Enqueue("电池SN:" + btAddress + "重复，请检查");
                //    }
                //    else
                //    {
                //        if (btAddress != "")
                //        {
                //            if (btAddress == batt && btAddress.StartsWith(data.LowLimit))
                //            {
                //                data.Result = "Pass";
                //                data.Value = batt;
                //            }
                //            else
                //            {
                //                data.Result = "Fail";
                //                data.Value = batt;
                //                if (data.Check)
                //                {
                //                    Serial.ClosedPort();
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (batt.StartsWith(data.LowLimit))
                //            {
                //                data.Result = "Pass";
                //                data.Value = batt;
                //            }
                //            else
                //            {
                //                data.Result = "Fail";
                //                data.Value = batt;
                //                if (data.Check)
                //                {
                //                    Serial.ClosedPort();
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                #endregion

                if (btAddress != "")
                {
                    if (btAddress == batt && btAddress.StartsWith(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = batt;
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = batt;
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }
                else
                {
                    if (batt.StartsWith(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = batt;
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = batt;
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadProductColor(TestData data)
        {

            try
            {
                byte[] bytes = { 0x04, 0xff, 0x07, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                //2:1;3:1
                if (values[1] == 0x07)
                {
                    byte product = values[3];
                    //int english = int.Parse(values[3].ToString());
                    string prod = string.Empty;
                    string eng = string.Empty;

                    if (product == 0x00)
                        prod = "华为橙色";
                    else if (product == 0x01)
                        prod = "华为黑色";
                    //else if (product == 0x02)
                    //    prod = "华为绿色";
                    else if (product == 0x02)
                        prod = "荣耀黑色";
                    else if (product == 0x03)
                        prod = "华为银色";
                    //else if (product == 0x04)
                    //    prod = "华为紫色";
                    else if (product == 0x04)
                        prod = "荣耀绿色";
                    else if (product == 0x05)
                        prod = "华为橘红色";
                    else if (product == 0x10)
                        prod = "荣耀灰色";
                    else if (product == 0x11)
                        prod = "荣耀蓝色";
                    else if (product == 0x12)
                        prod = "荣耀红色";
                    else if (product == 0x13)
                        prod = "荣耀营销色";
                    else if (product == 0x09)
                        prod = "荣耀橙色";
                    if (data.LowLimit == prod)
                    {
                        data.Result = "Pass";
                        data.Value = prod;
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = prod;
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadWarningTone(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x15, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                string name = data.LowLimit;
                string retName = "";
                if (values[3] == 0x00 && values[4] == 0x00)
                {
                    retName = "华为中文";
                }
                else if (values[3] == 0x00 && values[4] == 0x01)
                {
                    retName = "华为英文";
                }
                else if (values[3] == 0x01 && values[4] == 0x00)
                {
                    retName = "荣耀中文";
                }
                else if (values[3] == 0x01 && values[4] == 0x01)
                {
                    retName = "荣耀英文";
                }

                if (retName == name)
                {
                    data.Result = "Pass";
                    data.Value = retName;
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = retName;
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_Semi_Product_CheckMacAddress(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x05, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x06)
                {
                    string btaddress = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                        , values[8], values[7], values[6], values[5], values[4], values[3]).ToUpper();
                    if (config.MesEnable)
                    {
                        //btaddress = "E09DFA47038D";
                        MesWeb = new WebReference.WebService1();
                        string result = MesWeb.SnCx_LY(btaddress);
                        if (result.Equals("P"))
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            queue.Enqueue("该蓝牙地址：" + btaddress + "无重复");
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                            queue.Enqueue("该蓝牙地址：" + btaddress + "重复，请检查");
                        }
                        MesWeb.Abort();
                        MesWeb.Dispose();

                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Semi_Product_CheckProductSN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x00, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                //\u0004\u001020225719ag000001
                if (values[0] == 0x04)
                {
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    string sn = Encoding.ASCII.GetString(needData);
                    if (config.MesEnable)
                    {
                        //sn = "GJ1225203W100575";
                        MesWeb = new WebReference.WebService1();
                        string reslut = MesWeb.SnCx_sn(sn);
                        if (reslut.Equals("P"))
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            queue.Enqueue("该产品SN：" + sn + "无重复");
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                            queue.Enqueue("该产品SN：" + sn + "重复，请检查");
                        }
                        MesWeb.Abort();
                        MesWeb.Dispose();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Semi_Product_CheckBatterySN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x06, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                string batt = Encoding.ASCII.GetString(needData);
                if (config.MesEnable)
                {
                    //batt = "SYN0C1909040980";
                    MesWeb = new WebReference.WebService1();
                    string res = MesWeb.SnCx_DC(batt);
                    if (res != "P")
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        queue.Enqueue("电池SN：" + btAddress + "重复，请检查");
                    }
                    else
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                        queue.Enqueue("电池SN：" + btAddress + "无重复");
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_Assy_CRCCheckProductSN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x00, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                //\u0004\u001020225719ag000001
                if (values[0] == 0x04)
                {
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    string sn = Encoding.ASCII.GetString(needData);
                    if (config.MesEnable)
                    {
                        //sn = "GJ1225203W100575";
                        MesWeb = new WebReference.WebService1();
                        string reslut = MesWeb.SnCx_BDSN(sn);
                        if (reslut.Equals("P"))
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            queue.Enqueue("该产品SN：" + sn + "无重复");
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                            queue.Enqueue("该产品SN：" + sn + "重复，请检查");
                        }
                        MesWeb.Abort();
                        MesWeb.Dispose();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Assy_CRCBatterySN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x06, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                string batt = Encoding.ASCII.GetString(needData);
                if (config.MesEnable)
                {
                    //batt = "SYN0C1909040980";
                    MesWeb = new WebReference.WebService1();
                    string res = MesWeb.SnCx_DC2(batt);
                    if (res != "P")
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        queue.Enqueue("电池SN：" + btAddress + "重复，请检查");
                    }
                    else
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                        queue.Enqueue("电池SN：" + btAddress + "无重复");
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_Assy_CRCCheckMacAddress(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x05, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x06)
                {
                    string btaddress = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                        , values[8], values[7], values[6], values[5], values[4], values[3]).ToUpper();
                    if (config.MesEnable)
                    {
                        //btaddress = "E09DFA47038D";
                        MesWeb = new WebReference.WebService1();
                        string result = MesWeb.SnCx_LY2(btaddress);
                        if (result.Equals("P"))
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            queue.Enqueue("该蓝牙地址：" + btaddress + "无重复");
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                            queue.Enqueue("该蓝牙地址：" + btaddress + "重复，请检查");
                        }
                        MesWeb.Abort();
                        MesWeb.Dispose();

                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Pack_CRCBatterySN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x06, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                string batt = Encoding.ASCII.GetString(needData);
                if (config.MesEnable)
                {
                    //batt = "SYN0C1909040980";
                    MesWeb = new WebReference.WebService1();
                    string res = MesWeb.SnCx_DC3(batt);
                    if (res != "P")
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        queue.Enqueue("电池SN：" + batt + "重复，请检查");
                    }
                    else
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                        queue.Enqueue("电池SN：" + batt + "无重复");
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_Pack_CheckMacAddress(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x05, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x06)
                {
                    string btaddress = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                        , values[8], values[7], values[6], values[5], values[4], values[3]).ToUpper();
                    if (config.MesEnable)
                    {
                        //btaddress = "E09DFA47038D";
                        MesWeb = new WebReference.WebService1();
                        string result = MesWeb.SnCx_BZLY(btaddress);
                        if (result.Equals("P"))
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            queue.Enqueue("该蓝牙地址：" + btaddress + "无重复");
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                            queue.Enqueue("该蓝牙地址：" + btaddress + "重复，请检查");
                        }
                        MesWeb.Abort();
                        MesWeb.Dispose();

                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Pack_CheckProductSN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x00, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                //\u0004\u001020225719ag000001
                if (values[0] == 0x04)
                {
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    string sn = Encoding.ASCII.GetString(needData);
                    if (config.MesEnable)
                    {
                        //sn = "GJ1225203W100575";
                        MesWeb = new WebReference.WebService1();
                        string reslut = MesWeb.SnCx_BZSN(sn);
                        if (reslut.Equals("P"))
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            queue.Enqueue("该产品SN：" + sn + "无重复");
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                            queue.Enqueue("该产品SN：" + sn + "重复，请检查");
                        }
                        MesWeb.Abort();
                        MesWeb.Dispose();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Pack_CheckPackSN(TestData data)
        {
            try
            {
                if (config.MesEnable)
                {
                    //sn = "GJ1225203W100575";
                    MesWeb = new WebReference.WebService1();
                    string reslut = MesWeb.SnCx_BZSN2(PackSN);
                    if (reslut.Equals("P"))
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                        queue.Enqueue("该包装SN：" + PackSN + "无重复");
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        queue.Enqueue("该包装SN：" + PackSN + "重复，请检查");
                    }
                    MesWeb.Abort();
                    MesWeb.Dispose();
                }

            }

            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Pair(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x0a, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x00 && values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_SetVolume(TestData data)
        {
            try
            {
                // 04 ff 14 00 01 11
                byte[] bytes = { 0x04, 0xff, 0x14, 0x00, 0x01, 0x11 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[3] == 0x01 && values[4] == 0x11)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_Enter_DUTMode(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x0d, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x00 && values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_Shutdown(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x0b, 0x01 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x00 && values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_Reset(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x0c, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x00 && values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ClearPair(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x11, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x00 && values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ShippingMode(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x1a, 0x01 };
                //Thread.Sleep(5000);
                byte[] values = Serial.VisaQuery(bytes);


                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_BurnIN(TestData data)
        {
            try
            {
                //5a 01 07 ff ff ff ff ff ff ff a3
                byte[] bytes = { 0x5a, 0x01, 0x07, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xa3 };
                //Thread.Sleep(5000);
                byte[] values = Serial.VisaQuery(bytes);


                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_ReadBurnIN(TestData data)
        {
            try
            {
                //5a 02 07 ff ff ff ff ff ff ff a0
                byte[] bytes = { 0x5a, 0x02, 0x07, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xa0 };
                //Thread.Sleep(5000);
                //5A 03 07 01 FF FF FF FF FF FF 5F
                byte[] values = Serial.VisaQuery(bytes);
                //BurnINByte = values;
                //values = new byte[] { 0x03, 0x07, 0x01, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xff, 0x5f };
                if (values[0] == 0x03)
                {
                    //data.Result = "Pass";
                    //data.Value = "Pass";
                    #region
                    for (int i = 3; i <= 8; i++)
                    {
                        if (values[i] != 0xff)
                        {
                            if (i % 2 == 0)
                            {
                                switch (values[i])
                                {
                                    case 0xfe:
                                        {
                                            data.Value = "休眠唤醒后,充电异常";
                                            break;
                                        }
                                    case 0xfd:
                                        {
                                            data.Value = "写入和读取4KB异常";
                                            break;
                                        }
                                    case 0xfb:
                                        {
                                            data.Value = "低电压关机";
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                switch (values[i])
                                {
                                    case 0xfe:
                                        {
                                            data.Value = "正常状态下,HALL异常";
                                            break;
                                        }
                                    case 0xfd:
                                        {
                                            data.Value = "正常状态下,电量计异常";
                                            break;
                                        }
                                    case 0xfb:
                                        {
                                            data.Value = "正常状态下,充电芯片异常";
                                            break;
                                        }
                                    case 0xf7:
                                        {
                                            data.Value = "最大功率条件下,HALL异常";
                                            break;
                                        }
                                    case 0xef:
                                        {
                                            data.Value = "最大功率条件下,电量计异常";
                                            break;
                                        }
                                    case 0xdf:
                                        {
                                            data.Value = "最大功率条件下,充电芯片异常";
                                            break;
                                        }
                                    case 0xbf:
                                        {
                                            data.Value = "休眠唤醒后,HALL异常";
                                            break;
                                        }
                                    case 0x7f:
                                        {
                                            data.Value = "休眠唤醒后,电量计异常";
                                            break;
                                        }
                                }
                            }
                            data.Result = "Fail";
                            break;
                        }

                    }
                    if (string.IsNullOrWhiteSpace(data.Result))
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                    }
                    #endregion
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            BurnINFail = data.Value;
            data.Result = "Pass";
            data.Value = "Pass";
            return data;
        }

        public TestData BES_Read_NormalBurnIN_HALL(TestData data)
        {
            try
            {
                if (BurnINFail == "正常状态下,HALL异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_NormalBurnIN_Power(TestData data)
        {
            try
            {
                if (BurnINFail == "正常状态下,电量计异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_NormalBurnIN_Charge(TestData data)
        {
            try
            {
                if (BurnINFail == "正常状态下,充电芯片异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_MaxPowerBurnIN_HALL(TestData data)
        {
            try
            {
                if (BurnINFail == "最大功率条件下,HALL异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_MaxPowerBurnIN_Power(TestData data)
        {
            try
            {
                if (BurnINFail == "最大功率条件下,电量计异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_MaxPowerBurnIN_Charge(TestData data)
        {
            try
            {
                if (BurnINFail == "最大功率条件下,充电芯片异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_SleepBurnIN_HALL(TestData data)
        {
            try
            {
                if (BurnINFail == "休眠唤醒后,HALL异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_SleepBurnIN_Power(TestData data)
        {
            try
            {
                if (BurnINFail == "休眠唤醒后,电量计异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_SleepBurnIN_Charge(TestData data)
        {
            try
            {
                if (BurnINFail == "休眠唤醒后,充电异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_BurnIN_LowBattery(TestData data)
        {
            try
            {
                if (BurnINFail == "低电压关机")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_Read_BurnIN_Memory(TestData data)
        {
            try
            {
                if (BurnINFail == "写入和读取4KB异常")
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
                else
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData BES_ReadBattaryVoltage(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x12, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                //3:4
                if (values[3] == 0x04)
                {
                    double voltage = double.Parse(string.Format("{0}{1}{2}{3}"
                        , values[4], values[5], values[6], values[7])) / 1000;
                    //电池电压大于等于4.4时在重新测试一次
                    if (voltage >= 4.4)
                    {
                        values = Serial.VisaQuery(bytes);
                        if (values[3] == 0x04)
                        {
                            voltage = double.Parse(string.Format("{0}{1}{2}{3}"
                        , values[4], values[5], values[6], values[7])) / 1000;
                        }
                    }
                    if (voltage >= double.Parse(data.LowLimit)
                        && voltage <= double.Parse(data.UppLimit))
                    {
                        data.Result = "Pass";
                        data.Value = voltage.ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = voltage.ToString();
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }

                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadNTC(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x16, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[3] == 0x04)
                {
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    double voltage = double.Parse(string.Format("{0}{1}{2}{3}"
                       , values[4], values[5], values[6], values[7]));
                    if (voltage >= double.Parse(data.LowLimit)
                        && voltage <= double.Parse(data.UppLimit))
                    {
                        data.Result = "Pass";
                        data.Value = voltage.ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = voltage.ToString();
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }

                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadTrim(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x08, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[1] == 0x08)
                {
                    data.Result = "Pass";
                    data.Value = values[3].ToString("X2");
                }

                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadWaterProof(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x17, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[3] == 0x04)
                {
                    double voltage = double.Parse(string.Format("{0}{1}{2}{3}"
                       , values[4], values[5], values[6], values[7]));
                    if (voltage >= double.Parse(data.LowLimit)
                        && voltage <= double.Parse(data.UppLimit))
                    {
                        data.Result = "Pass";
                        data.Value = voltage.ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }

                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadChargeMode(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x18, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                string mode = "";

                if (values[3] == 0x01)
                {
                    mode = "工厂模式";
                }
                else if (values[3] == 0x00)
                {
                    mode = "用户模式";
                }
                if (data.LowLimit == mode)
                {
                    data.Result = "Pass";
                    data.Value = mode;
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = mode;
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadTestStation(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x01, 0x01, 0x02, 0x01, 0x00 };
                //byte station = byte.Parse(data.LowLimit
                //    , System.Globalization.NumberStyles.HexNumber);

                byte[] values = Serial.VisaQuery(bytes);
                string flag = Encoding.ASCII.GetString(values, 3, 2);
                int result = string.Compare(flag, data.LowLimit);
                if (result >= 0)
                {
                    data.Result = "Pass";
                    data.Value = flag.ToString();
                }

                else
                {
                    data.Result = "Fail";
                    data.Value = Convert.ToString(values[3], 16);
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_HALLTest(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x10, 0x01, 0x00, 0x03 };
                byte[] values = new byte[0];
                List<byte> status = new List<byte>();
                //for (int i = 0; i < 3; i++)
                //{
                values = Serial.VisaQuery(bytes);
                //Thread.Sleep(100);
                //status.Add(values[5]);
                //}                        
                if (values[3] == 0x02)
                {
                    //HALLTest hALL = new HALLTest();
                    //hALL.ShowDialog();
                    if (config.AutoHALL)
                    {
                        FixPort.WriteLine("HALL" + "\r");
                    }
                    if (values[5] == 0x01)
                    {
                        //    Thread thread = new Thread(() =>
                        //{
                        try
                        {
                            statusQueue.Enqueue("移开霍尔");
                            for (int i = 0; i < 10; i++)
                            {
                                values = Serial.VisaQuery(bytes);
                                if (values[5] == 0x00)
                                {
                                    data.Value = "Pass";
                                    data.Result = "Pass";
                                    //hALL.Close();
                                    statusQueue.Enqueue("End");
                                    break;
                                }
                                if (values[5] != 0x00 && i == 9)
                                {
                                    data.Value = "Fail";
                                    data.Result = "Fail";
                                    if (data.Check)
                                    {
                                        Serial.ClosedPort();
                                    }
                                    //hALL.Close();
                                    statusQueue.Enqueue("Fail");
                                }
                                Thread.Sleep(500);
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Value = "Fail";
                            data.Result = "Fail";
                            if (data.Check)
                            {
                                Serial.ClosedPort();
                            }
                            //hALL.Close();
                            statusQueue.Enqueue("Fail");
                        }
                        //});
                        //    thread.Start();
                        //    hALL.ShowDialog();

                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_HALLClosedTest(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x10, 0x01, 0x00, 0x03 };
                byte[] values = new byte[0];
                List<byte> status = new List<byte>();
                //for (int i = 0; i < 3; i++)
                //{
                values = Serial.VisaQuery(bytes);
                //Thread.Sleep(100);
                //status.Add(values[5]);
                //}                        
                if (values[3] == 0x02)
                {
                    //HALLTest hALL = new HALLTest();
                    //hALL.ShowDialog();
                    //if (config.AutoHALL)
                    //{
                    //    FixPort.WriteLine("HALL" + "\r");
                    //}
                    if (values[5] == 0x01)
                    {
                        //    Thread thread = new Thread(() =>
                        //{
                        //try
                        //{
                        //statusQueue.Enqueue("移开霍尔");
                        //for (int i = 0; i < 10; i++)
                        //{
                        //    values = Serial.VisaQuery(bytes);
                        //    if (values[5] == 0x00)
                        //    {
                        data.Value = "Pass";
                        data.Result = "Pass";
                        //        //hALL.Close();
                        //        statusQueue.Enqueue("End");
                        //        break;
                        //    }
                        //    if (values[5] != 0x00 && i == 9)
                        //    {
                        //        data.Value = "Fail";
                        //        data.Result = "Fail";
                        //        if (data.Check)
                        //        {
                        //            Serial.ClosedPort();
                        //        }
                        //        //hALL.Close();
                        //        statusQueue.Enqueue("Fail");
                        //    }
                        //    Thread.Sleep(500);
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    data.Value = "Fail";
                        //    data.Result = "Fail";
                        //    if (data.Check)
                        //    {
                        //        Serial.ClosedPort();
                        //    }
                        //    //hALL.Close();
                        //    statusQueue.Enqueue("Fail");
                        //}
                        //});
                        //    thread.Start();
                        //    hALL.ShowDialog();

                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        //public TestData BES_PowerKeyTest(TestData data)
        //{
        //    try
        //    {
        //        byte[] bytes = { 0x04, 0xff, 0x10, 0x01, 0x00, 0x03 };
        //        byte[] values = new byte[0];
        //        List<byte> status = new List<byte>();
        //        //for (int i = 0; i < 3; i++)
        //        //{
        //        values = Serial.VisaQuery(bytes);
        //        //Thread.Sleep(100);
        //        //status.Add(values[5]);
        //        //}                        
        //        if (values[3] == 0x02)
        //        {
        //            HALLTest hALL = new HALLTest();
        //            //hALL.ShowDialog();
        //            if (values[5] == 0x01)
        //            {
        //                Thread thread = new Thread(() =>
        //                {
        //                    try
        //                    {
        //                        for (int i = 0; i < 10; i++)
        //                        {
        //                            values = Serial.VisaQuery(bytes);
        //                            if (values[5] == 0x00)
        //                            {
        //                                data.Value = "Pass";
        //                                data.Result = "Pass";
        //                                hALL.Close();
        //                                break;
        //                            }
        //                            if (values[5] != 0x00 && i == 9)
        //                            {
        //                                data.Value = "Fail";
        //                                data.Result = "Fail";
        //                                if (data.Check)
        //                                {
        //                                    Serial.ClosedPort();
        //                                }
        //                                hALL.Close();
        //                            }
        //                            Thread.Sleep(500);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        data.Value = "Fail";
        //                        data.Result = "Fail";
        //                        if (data.Check)
        //                        {
        //                            Serial.ClosedPort();
        //                        }
        //                        hALL.Close();
        //                    }
        //                });
        //                thread.Start();
        //                hALL.ShowDialog();
        //            }
        //            else
        //            {
        //                data.Result = "Fail";
        //                data.Value = "Fail";
        //                if (data.Check)
        //                {
        //                    Serial.ClosedPort();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            data.Result = "Fail";
        //            data.Value = "Fail";
        //            if (data.Check)
        //            {
        //                Serial.ClosedPort();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Result = "Fail";
        //        data.Value = "Fail";
        //        queue.Enqueue("ex;" + ex.Message);
        //        if (data.Check)
        //        {
        //            Serial.ClosedPort();
        //        }
        //    }
        //    return data;
        //}

        public TestData BES_ReadPackSN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x1B, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[0] == 0x04)
                {
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    string sn = Encoding.ASCII.GetString(needData, 0, needData.Length);
                    if (sn.Equals(PackSN))
                    {
                        data.Result = "Pass";
                        data.Value = sn;
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = sn;
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }

                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WritePackSN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x1b, 0x00, 0x00 };
                //byte[] values = Serial.VisaQuery(bytes);
                byte[] address = Encoding.ASCII.GetBytes(PackSN);
                byte length = byte.Parse(PackSN.Length.ToString());
                byte[] value = new byte[bytes.Length + PackSN.Length];
                bytes.CopyTo(value, 0);
                value[4] = length;
                address.CopyTo(value, 5);

                byte[] ret = Serial.VisaQuery(value);
                if (ret[2] == length)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }

            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteSN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x00, 0x00, 0x00 };
                //byte[] values = Serial.VisaQuery(bytes);
                byte[] address = new byte[0];
                if (config.AutoSN)
                {
                    btAddress = GetProductSN(config.SNHear, config.SNLine);
                    address = Encoding.ASCII.GetBytes(btAddress);
                }
                else
                {
                    address = Encoding.ASCII.GetBytes(btAddress);
                }
                string result = string.Empty;
                if (config.MesEnable)
                {
                    MesWeb = new WebReference.WebService1();
                    result = MesWeb.SnCx_sn(btAddress);
                    MesWeb.Abort();
                }
                else
                {
                    result = "P";
                }
                if (result.Contains("P"))
                {
                    byte length = byte.Parse(address.Length.ToString());
                    byte[] value = new byte[bytes.Length + address.Length];
                    bytes.CopyTo(value, 0);
                    value[4] = length;
                    address.CopyTo(value, 5);

                    byte[] ret = Serial.VisaQuery(value);
                    if (ret[2] == length)
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }
                else
                {
                    queue.Enqueue("产品SN重复，请检查");
                    data.Result = "Fail";
                    data.Value = btAddress;
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteBattarySN(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x06, 0x00, 0x00 };
                //byte[] values = Serial.VisaQuery(bytes);
                byte[] address = Encoding.ASCII.GetBytes(btAddress);
                byte length = byte.Parse(address.Length.ToString());
                byte[] value = new byte[bytes.Length + address.Length];
                bytes.CopyTo(value, 0);
                value[4] = length;
                address.CopyTo(value, 5);

                if (config.MesEnable)
                {
                    MesWeb = new WebReference.WebService1();
                    string res = MesWeb.SnCx_DC(btAddress);
                    if (res != "P")
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        queue.Enqueue("电池SN:" + btAddress + "重复，请检查");
                    }
                    else
                    {
                        queue.Enqueue("电池SN" + "无重复");
                        byte[] ret = Serial.VisaQuery(value);
                        if (ret[2] == length)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                            if (data.Check)
                            {
                                Serial.ClosedPort();
                            }
                        }
                        MesWeb.Abort();
                        MesWeb.Dispose();
                    }
                }
                else
                {
                    byte[] ret = Serial.VisaQuery(value);
                    if (ret[2] == length)
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteBTAddress(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x05, 0x00, 0x00 };
                //byte[] values = Serial.VisaQuery(bytes);

                byte[] address = new byte[6];
                address[0] = Convert.ToByte(btAddress.Substring(10, 2), 16);
                address[1] = Convert.ToByte(btAddress.Substring(8, 2), 16);
                address[2] = Convert.ToByte(btAddress.Substring(6, 2), 16);
                address[3] = Convert.ToByte(btAddress.Substring(4, 2), 16);
                address[4] = Convert.ToByte(btAddress.Substring(2, 2), 16);
                address[5] = Convert.ToByte(btAddress.Substring(0, 2), 16);
                byte length = byte.Parse(address.Length.ToString());
                byte[] value = new byte[bytes.Length + address.Length];
                bytes.CopyTo(value, 0);
                value[4] = length;
                address.CopyTo(value, 5);

                byte[] ret = Serial.VisaQuery(value);

                if (ret.Length > 0)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteProductColor(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x07, 0x00, 0x01, 0x00 };

                string name = data.LowLimit;
                if (name == "华为橙色")
                    bytes[5] = 0x00;
                else if (name == "华为黑色")
                    bytes[5] = 0x01;
                //else if (name == "华为绿色")
                //    bytes[5] = 0x02;
                else if (name == "荣耀黑色")
                    bytes[5] = 0x02;
                else if (name == "华为银色")
                    bytes[5] = 0x03;
                //else if (name == "华为紫色")
                //    bytes[5] = 0x04;
                else if (name == "荣耀绿色")
                    bytes[5] = 0x04;
                else if (name == "华为橘红色")
                    bytes[5] = 0x05;
                else if (name == "荣耀灰色")
                    bytes[5] = 0x10;
                else if (name == "荣耀蓝色")
                    bytes[5] = 0x11;
                else if (name == "荣耀红色")
                    bytes[5] = 0x12;
                else if (name == "荣耀营销色")
                    bytes[5] = 0x13;
                else if (name == "荣耀橙色")
                    bytes[5] = 0x09;
                byte[] values = Serial.VisaQuery(bytes);
                if (values[3] == bytes[5])
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteWarningTone(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x15, 0x00, 0x02, 0x00, 0x00 };

                string name = data.LowLimit;
                if (name == "华为中文")
                {
                    bytes[5] = 0x00;
                    bytes[6] = 0x00;
                }
                else if (name == "华为英文")
                {
                    bytes[5] = 0x00;
                    bytes[6] = 0x01;
                }
                else if (name == "荣耀中文")
                {
                    bytes[5] = 0x01;
                    bytes[6] = 0x00;
                }
                else if (name == "荣耀英文")
                {
                    bytes[5] = 0x01;
                    bytes[6] = 0x01;
                }

                byte[] values = Serial.VisaQuery(bytes);

                if (values[3] == bytes[5] && values[4] == bytes[6])
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteChargeMode(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x18, 0x00, 0x01, 0x00 };
                if (data.LowLimit == "工厂模式")
                {
                    bytes[5] = 0x01;
                }
                else if (data.LowLimit == "用户模式")
                {
                    bytes[5] = 0x00;
                }
                byte[] values = Serial.VisaQuery(bytes);
                if (values[3] == 0x01)
                {
                    data.Result = "Pass";
                    data.Value = "工厂模式";
                }
                else if (values[3] == 0x00)
                {
                    data.Result = "Pass";
                    data.Value = "用户模式";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteTestStation(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x01, 0x00, 0x02, 0x00, 0x00 };
                //byte station = byte.Parse(data.LowLimit
                //    , System.Globalization.NumberStyles.HexNumber);
                byte[] station = Encoding.ASCII.GetBytes(data.LowLimit.Trim());
                bytes[5] = station[0];
                bytes[6] = station[1];
                byte[] values = Serial.VisaQuery(bytes);
                string flag = Encoding.ASCII.GetString(values, 3, 2);
                data.Result = "Pass";
                data.Value = "Pass";
                //if (flag == data.LowLimit)
                //{
                //    data.Result = "Pass";
                //    data.Value = flag;
                //}

                //else
                //{
                //    data.Result = "Fail";
                //    data.Value = flag;
                //    if (data.Check)
                //    {
                //        Serial.ClosedPort();
                //    }
                //}
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteHWVersion(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x03, 0x00, 0x00 };
                //byte[] values = Serial.VisaQuery(bytes);

                //if (config.AutoSNTest)
                //{
                //    btAddress = GetProductSN(config.SNHear, config.SNLine);
                //    address = Encoding.ASCII.GetBytes(btAddress);
                //}
                //else
                //{
                //address = Encoding.ASCII.GetBytes(data.LowLimit.Trim());
                //}
                // address = Others.HexStringToByteArray(data.LowLimit);
                byte length = byte.Parse(data.LowLimit.Trim().Length.ToString());
                byte[] address = new byte[data.LowLimit.Trim().Length];
                byte[] value = new byte[bytes.Length + length];
                bytes.CopyTo(value, 0);
                value[4] = length;

                for (int i = 0; i < length; i++)
                {
                    address[i] = byte.Parse(data.LowLimit.Trim().Substring(i, 1));
                }
                address.CopyTo(value, 5);
                byte[] ret = Serial.VisaQuery(value);
                if (ret[2] == 0x08)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_WriteHWVersion_ASCII(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x03, 0x00, 0x00 };
                //byte[] values = Serial.VisaQuery(bytes);

                //if (config.AutoSNTest)
                //{
                //    btAddress = GetProductSN(config.SNHear, config.SNLine);
                //    address = Encoding.ASCII.GetBytes(btAddress);
                //}
                //else
                //{
                byte[] address = Encoding.ASCII.GetBytes(data.LowLimit.Trim());
                //}
                // address = Others.HexStringToByteArray(data.LowLimit);
                byte length = byte.Parse(data.LowLimit.Trim().Length.ToString());
                //byte[] address = new byte[data.LowLimit.Trim().Length];
                byte[] value = new byte[bytes.Length + length];
                bytes.CopyTo(value, 0);
                value[4] = length;

                //for (int i = 0; i < length; i++)
                //{
                //    address[i] = byte.Parse(data.LowLimit.Trim().Substring(i, 1));
                //}
                address.CopyTo(value, 5);
                byte[] ret = Serial.VisaQuery(value);
                if (ret[2] == length)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ControlMic1(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x20, 0x01, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[0] == 0x04)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ControlMic2(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x20, 0x02, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[0] == 0x04)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ControlMic3(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x22, 0x00, 0x01, 0x02 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[0] == 0x04)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ControlMicAllOpen(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x22, 0x00, 0x01, 0x03 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[1] == 0x04)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ExitCDC(TestData data)
        {
            try
            {
                //04 FF 23 00 04 75 73 65 72 
                byte[] bytes = { 0x04, 0xff, 0x23, 0x00, 0x04, 0x75, 0x73, 0x65, 0x72 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[0] == 0x04)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadTouchData(TestData data)
        {
            try
            {
                //FF 04 26 70 02 E7 02 FE 03 09 02 E4 02 FE 03 1C 03 15 03 0D 02 FA 03 
                //19 03 13 03 23 03 08 02 EC 03 01 03 1C 03 0E 03 15 03 25 03 01 03 38 03
                //3A 03 28 04 0C 04 80 03 70 03 27 03 09 03 03 04 46 04 90 03 49 03 1F 03 
                //19 02 FD 03 50 03 23 02 F9 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
                //00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 

                byte[] bytes = { 0x04, 0xff, 0x26, 0x01, 0x00 };
                var msg = MessageBox.Show("请滑动触摸板", "Message", MessageBoxButtons.OK
                    , MessageBoxIcon.Asterisk);
                if (msg == DialogResult.OK)
                {
                    byte[] values = Serial.VisaQuery(bytes);
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    if (values[0] == 0x04)
                    {
                        data.Result = "Pass";
                        List<int> list = new List<int>();
                        for (int i = 0; i < needData.Length; i++)
                        {
                            list.Add(needData[i]);
                        }
                        data.Value = string.Join(";", list);
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_ReadWearData(TestData data)
        {
            try
            {
                //FF 04 26 70 02 E7 02 FE 03 09 02 E4 02 FE 03 1C 03 15 03 0D 02 FA 03 
                //19 03 13 03 23 03 08 02 EC 03 01 03 1C 03 0E 03 15 03 25 03 01 03 38 03
                //3A 03 28 04 0C 04 80 03 70 03 27 03 09 03 03 04 46 04 90 03 49 03 1F 03 
                //19 02 FD 03 50 03 23 02 F9 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
                //00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 

                byte[] bytes = { 0x04, 0xff, 0x27, 0x01, 0x00 };
                var msg = MessageBox.Show("请移动佩戴板", "Message", MessageBoxButtons.OK
                    , MessageBoxIcon.Asterisk);
                if (msg == DialogResult.OK)
                {
                    byte[] values = Serial.VisaQuery(bytes);
                    byte[] needData = values.Skip(3).Take(values[2]).ToArray();
                    Array.Reverse(needData);
                    int val = Convert.ToInt32(string.Format("{0:X}{1:X}"
                        , needData[0], needData[1]), 16);

                    if (values[0] == 0x04)
                    {
                        if (val <= int.Parse(data.UppLimit) && val >= int.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = val.ToString();
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = val.ToString();
                        }
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_Enter_UsbAudio(TestData data)
        {
            try
            {
                //04 FF 23 00 04 75 73 65 72 
                byte[] bytes = { 0x04, 0xff, 0x25, 0x00, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
                if (values[0] == 0x04)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_BD02CompareFirstPairRecord(TestData data)
        {
            try
            {
                //55 AA FF 0D 00 AE
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0X0D, 0X00, 0XAE };
                byte[] values = Serial.VisaQuery(bytes);
                firstMac = "";
                if (values[0] == 0xAA)
                {
                    byte[] needData = values.Skip(4).Take(values[3] - 1).ToArray();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        sb.Append(needData[i].ToString("x2"));
                    }

                    data.Result = "Pass";
                    data.Value = sb.ToString().ToUpper();
                    firstMac = sb.ToString().ToUpper();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_BD02CompareSecondPairRecord(TestData data)
        {
            try
            {
                //55 AA FF 0D 00 AE
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0X0D, 0X00, 0XAE };
                byte[] values = Serial.VisaQuery(bytes);
                secondMac = "";
                if (values[0] == 0xAA)
                {
                    byte[] needData = values.Skip(4).Take(values[3] - 1).ToArray();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        sb.Append(needData[i].ToString("x2"));
                    }

                    data.Result = "Pass";
                    data.Value = sb.ToString().ToUpper();
                    secondMac = sb.ToString().ToUpper();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_BD02ComparePairRecord(TestData data)
        {
            try
            {
                if (firstMac != "" && secondMac != "")
                {
                    if (firstMac == secondMac)
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;

        }

        public TestData BES_BD02LeftCompareSoftVersion(TestData data)
        {
            try
            {
                // 55 AA FF 17 00 A5

                byte[] bytes = { 0x55, 0xAA, 0xFF, 0X17, 0X00, 0XA5 };
                byte[] values = Serial.VisaQuery(bytes);
                secondMac = "";
                if (values[0] == 0xAA)
                {
                    byte[] needData = values.Skip(4).Take(values[3] - 1).ToArray();
                    string version = string.Format("{0:x}", needData[1]);
                    if (data.LowLimit == version && needData[0] == 0)
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = version;
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_BD02RightCompareSoftVersion(TestData data)
        {
            try
            {
                // 55 AA FF 17 00 A5

                byte[] bytes = { 0x55, 0xAA, 0xFF, 0X17, 0X00, 0XA5 };
                byte[] values = Serial.VisaQuery(bytes);
                secondMac = "";
                if (values[0] == 0xAA)
                {
                    byte[] needData = values.Skip(4).Take(values[3] - 1).ToArray();
                    string version = string.Format("{0:x}", needData[1]);
                    if (data.LowLimit == version && needData[0] == 1)
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = version;
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_BD02ReadLeftPairMessage(TestData data)
        {
            try
            {
                //0x55 0xAA 0xFF 0x18 0x00 0xbd
                isMainEar = false;
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0x18, 0x00, 0xbd };
                byte[] values = Serial.VisaQuery(bytes);
                oneMac = "";
                twoMac = "";
                if (values[0] == 0xAA && values[4] == 0x00)
                {
                    oneMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                            , values[5], values[6], values[7], values[8], values[9], values[10]).ToUpper();
                    twoMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                          , values[11], values[12], values[13], values[14], values[15], values[16]).ToUpper();
                    data.Result = "Pass";
                    data.Value = oneMac + "_" + twoMac;
                    queue.Enqueue(string.Format("左耳tws地址{0}，本地地址{1}"
                        , oneMac, twoMac));
                }
                else
                {
                    queue.Enqueue("返回值有错");
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_BD02ReadRightPairMessage(TestData data)
        {
            try
            {
                //0x55 0xAA 0xFF 0x18 0x00 0xbd
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0x18, 0x00, 0xbd };
                byte[] values = Serial.VisaQuery(bytes);
                threeMac = "";
                fourMac = "";
                if (values[0] == 0xAA && values[4] == 0x01)
                {
                    threeMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                             , values[5], values[6], values[7], values[8], values[9], values[10]).ToUpper();
                    fourMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                      , values[11], values[12], values[13], values[14], values[15], values[16]).ToUpper();
                    data.Result = "Pass";
                    data.Value = threeMac + "_" + fourMac;
                    queue.Enqueue(string.Format("右耳tws地址{0}，本地地址{1}"
                       , threeMac, fourMac));
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_BD02LeftRightCompare(TestData data)
        {
            try
            {
                if ((oneMac == twoMac) && (twoMac == threeMac) && (threeMac == fourMac))
                {
                    data.Value = "Fail";
                    data.Result = "Fail";

                    return data;
                }

                if ((oneMac == twoMac) && (threeMac == oneMac))
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else if ((threeMac == fourMac) && (threeMac == oneMac))
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else if (twoMac == fourMac)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_T1207ReadMac(TestData data)
        {
            try
            {
                //55 AA FF 0D 00 AE
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0X0D, 0X00, 0XAE };
                byte[] values = Serial.VisaQuery(bytes);
                firstMac = "";
                if (values[0] == 0xAA)
                {
                    byte[] needData = values.Skip(5).Take(values[3] - 1).ToArray();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        sb.Append(needData[i].ToString("x2"));
                    }

                    data.Result = "Pass";
                    data.Value = sb.ToString().ToUpper();
                    firstMac = sb.ToString().ToUpper();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207SoftVersion(TestData data)
        {
            try
            {
                //55 AA FF E2 00 C3
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0XE2, 0X00, 0XC3 };
                byte[] values = Serial.VisaQuery(bytes);
                firstMac = "";
                if (values[0] == 0xAA)
                {
                    byte[] needData = values.Skip(5).Take(values[3] - 2).ToArray();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        sb.Append(needData[i].ToString("x"));
                    }
                    string ver = sb.ToString();
                    if (ver == data.LowLimit)
                    {
                        data.Result = "Pass";
                        data.Value = ver;
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = ver;
                    }

                    //firstMac = sb.ToString().ToUpper();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207EnterDUT(TestData data)
        {
            try
            {
                //55 AA FF 10 00 CB
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0X10, 0X00, 0XCB };
                byte[] values = Serial.VisaQuery(bytes);
                firstMac = "";
                if (values[0] == 0xAA)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207Reset(TestData data)
        {
            try
            {
                //55 AA FF 0b 00 04
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0X0b, 0X00, 0X04 };
                byte[] values = Serial.VisaQuery(bytes);
                firstMac = "";
                if (values[0] == 0xAA)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207WriteBtAddress(TestData data)
        {
            try
            {
                byte[] buf = { 0x55, 0xAA, 0xFF, 0x0C, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                buf[5] = byte.Parse(btAddress.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                buf[6] = byte.Parse(btAddress.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                buf[7] = byte.Parse(btAddress.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                buf[8] = byte.Parse(btAddress.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                buf[9] = byte.Parse(btAddress.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
                buf[10] = byte.Parse(btAddress.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);

                byte[] total = new byte[12];
                buf.CopyTo(total, 0);
                total[11] = crc8_maxim(buf, buf.Length);
                byte[] retvalue = Serial.VisaQuery(total);

                if (retvalue[0] == 0xaa)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_T1207ClsPair(TestData data)
        {
            try
            {
                //55 AA FF 1D 00 42
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0X1D, 0X00, 0X42 };
                byte[] values = Serial.VisaQuery(bytes);
                firstMac = "";
                if (values[0] == 0xAA)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207ReadLeftPairMessage(TestData data)
        {
            try
            {
                //55 AA FF E1 00 96
                isMainEar = false;
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0xE1, 0x00, 0x96 };
                byte[] values = Serial.VisaQuery(bytes);
                oneMac = "";
                twoMac = "";
                if (values[0] == 0xAA && values[4] == 0x00)
                {
                    oneMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                            , values[5], values[6], values[7], values[8], values[9], values[10]).ToUpper();
                    twoMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                          , values[11], values[12], values[13], values[14], values[15], values[16]).ToUpper();
                    data.Result = "Pass";
                    data.Value = oneMac + "_" + twoMac;
                    queue.Enqueue(string.Format("左耳tws地址{0}，本地地址{1}"
                        , oneMac, twoMac));
                }
                else
                {
                    queue.Enqueue("返回值有错");
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207ReadLeftElectricity(TestData data)
        {
            try
            {
                //55 AA FF E4 00 69
                isMainEar = false;
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0xE4, 0x00, 0x69 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0xAA && values[5] == 0x00)
                {
                    int elec = values[6] * 10;
                    if (elec >= int.Parse(data.LowLimit) && elec <= int.Parse(data.UppLimit))
                    {
                        data.Value = elec.ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = elec.ToString();
                        data.Result = "Fail";
                    }
                }
                else
                {
                    queue.Enqueue("返回值有错");
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207ReadRightElectricity(TestData data)
        {
            try
            {
                //55 AA FF E4 00 69
                isMainEar = false;
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0xE4, 0x00, 0x69 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0xAA && values[5] == 0x01)
                {
                    int elec = values[6] * 10;
                    if (elec >= int.Parse(data.LowLimit) && elec <= int.Parse(data.UppLimit))
                    {
                        data.Value = elec.ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = elec.ToString();
                        data.Result = "Fail";
                    }
                }
                else
                {
                    queue.Enqueue("返回值有错");
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207ReadRightPairMessage(TestData data)
        {
            try
            {
                //55 AA FF E1 00 96
                byte[] bytes = { 0x55, 0xAA, 0xFF, 0xE1, 0x00, 0x96 };
                byte[] values = Serial.VisaQuery(bytes);
                threeMac = "";
                fourMac = "";
                if (values[0] == 0xAA && values[4] == 0x01)
                {
                    threeMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                             , values[5], values[6], values[7], values[8], values[9], values[10]).ToUpper();
                    fourMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                      , values[11], values[12], values[13], values[14], values[15], values[16]).ToUpper();
                    data.Result = "Pass";
                    data.Value = threeMac + "_" + fourMac;
                    queue.Enqueue(string.Format("右耳tws地址{0}，本地地址{1}"
                       , threeMac, fourMac));
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_T1207LeftRightCompare(TestData data)
        {
            try
            {
                if ((oneMac == twoMac) && (twoMac == threeMac) && (threeMac == fourMac))
                {
                    data.Value = "Fail";
                    data.Result = "Fail";

                    return data;
                }

                if ((oneMac == twoMac) && (fourMac == oneMac))
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else if ((threeMac == fourMac) && (fourMac == oneMac))
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else if (twoMac == fourMac)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            finally
            {
                oneMac = "";
                twoMac = "";
                threeMac = "";
                fourMac = "";
            }
            return data;
        }

        public TestData BES_TW18LeftRightCompare(TestData data)
        {
            try
            {
                byte[] bytes = { 0x04, 0xFF, 0x24 };
                byte[] values = Serial.VisaQuery(bytes);
                StringBuilder sb = new StringBuilder();
                //var one = CalcCheck(values);
                //var two = CalcCheck(new byte[] { 0x04,0x24,0x01,0xff,0x04,0x24,0x00});
                for (int i = 0; i < 7; i++)
                {
                    sb.Append(values[i].ToString("x").Length == 1
                        ? string.Format("0{0}", values[i].ToString("x").ToUpper())
                        : values[i].ToString("x").ToUpper());
                }
                if (byte.Parse(data.LowLimit, System.Globalization.NumberStyles.HexNumber) == CalcCheck(values))
                {
                    data.Value = sb.ToString();
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = sb.ToString();
                    data.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_JT51ReadLeftPairMessage(TestData data)
        {
            try
            {
                //AA 55 FF 01 A1
                isMainEar = false;
                byte[] bytes = { 0xAA, 0x55, 0xFF, 0x01, 0xA1 };
                byte[] values = Serial.VisaQuery(bytes);
                oneMac = "";
                twoMac = "";
                if (values[0] == 85)
                {
                    oneMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                            , values[9], values[8], values[7], values[6], values[5], values[4]).ToUpper();
                    twoMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                          , values[15], values[14], values[13], values[12], values[11], values[10]).ToUpper();
                    data.Result = "Pass";
                    data.Value = oneMac + "_" + twoMac;
                    queue.Enqueue(string.Format("左耳本地地址{0}，本地tws地址{1}"
                       , oneMac, twoMac));
                }
                else
                {
                    queue.Enqueue("返回值有错");
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_JT51ReadRightPairMessage(TestData data)
        {
            try
            {
                //AA 55 FF 01 A1
                isMainEar = false;
                byte[] bytes = { 0xAA, 0x55, 0xFF, 0x01, 0xA1 };
                byte[] values = Serial.VisaQuery(bytes);
                threeMac = "";
                fourMac = "";
                if (values[0] == 85)
                {
                    threeMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                            , values[9], values[8], values[7], values[6], values[5], values[4]).ToUpper();
                    fourMac = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                          , values[15], values[14], values[13], values[12], values[11], values[10]).ToUpper();
                    data.Result = "Pass";
                    data.Value = threeMac + "_" + fourMac;
                    queue.Enqueue(string.Format("左耳本地地址{0}，本地tws地址{1}"
                        , threeMac, fourMac));
                }
                else
                {
                    queue.Enqueue("返回值有错");
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_TWS_TXMode(TestData data)
        {
            try
            {
                byte cmd = 0x15;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0x95)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_TWS_DUTMode(TestData data)
        {
            try
            {
                byte cmd = 0x07;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0x87)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_TWS_ReadVersion(TestData data)
        {
            try
            {
                byte cmd = 0x11;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0x91)
                {
                    string ver = values[2].ToString("X2") + values[3].ToString("X2");
                    if (data.LowLimit.Equals(ver))
                    {
                        data.Value = ver;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = ver;
                        data.Result = "Fail";
                    }
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_TWS_ReadMACAddress(TestData data)
        {
            try
            {
                byte cmd = 0x17;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0x97)
                {
                    string address = values[7].ToString("X2") + values[6].ToString("X2") + values[5].ToString("X2")
                        + values[4].ToString("X2") + values[3].ToString("X2") + values[2].ToString("X2");

                    data.Value = address;
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_TWS_MainMic(TestData data)
        {
            try
            {
                byte cmd = 0x31;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0xb1)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_TWS_FFMic(TestData data)
        {
            try
            {
                byte cmd = 0x32;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0xb2)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_TWS_ClearAll(TestData data)
        {
            try
            {
                byte cmd = 0x0D;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0x8d)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_TWS_AodioLoop(TestData data)
        {
            try
            {
                byte cmd = 0x08;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0x88)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_TWS_ComparePairName(TestData data)
        {
            try
            {
                byte cmd = 0x10;
                byte[] comm = GetTwsCommand(cmd);
                byte[] values = Serial.VisaQuery(comm);
                if (values[1] == 0x90)
                {
                    byte[] needData = values.Skip(2).Take(values[1]).ToArray();
                    string batt = Encoding.ASCII.GetString(needData).Split('?')[0];
                    if (data.LowLimit.Equals(batt))
                    {
                        data.Value = "Pass";
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = "Fail";
                        data.Result = "Fail";
                    }

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_EnterDUT(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x10, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_ReadSoftVersion(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x01, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    string version = Encoding.ASCII.GetString(values.Skip(4)
                        .Take(values[3]).ToArray());
                    if (data.LowLimit.Equals(version))
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = version;
                        data.Result = "Fail";
                    }

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_ReadHWVersion(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x02, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    string version = Encoding.ASCII.GetString(values.Skip(4)
                        .Take(values[3]).ToArray());
                    if (data.LowLimit.Equals(version))
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = version;
                        data.Result = "Fail";
                    }

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_ReadVoltage(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x07, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    string voltage = Encoding.ASCII.GetString(values.Skip(4)
                        .Take(values[3] - 1).ToArray());
                    if (double.Parse(voltage) >= double.Parse(data.LowLimit)
                        && double.Parse(voltage) <= double.Parse(data.UppLimit))
                    {
                        data.Value = voltage;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = voltage;
                        data.Result = "Fail";
                    }

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_ReadElectricity(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x03, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    string voltage = values[4].ToString();
                    if (double.Parse(voltage) >= double.Parse(data.LowLimit)
                        && double.Parse(voltage) <= double.Parse(data.UppLimit))
                    {
                        data.Value = voltage;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = voltage;
                        data.Result = "Fail";
                    }

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_ReadBTAddress(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x21, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    string btaddress = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}"
                        , values[9], values[8], values[7], values[6], values[5], values[4]).ToUpper();

                    data.Value = btaddress;
                    data.Result = "Pass";

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_WriteBTAddress(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                //48 53 21 01 06 E0 9D FA 12 34 56 AA

                byte[] cmd = { 0x4c, 0x43, 0x21, 0x00, 0x06 };
                byte[] address = new byte[7];
                address[0] = (byte)Convert.ToInt32(btAddress.Substring(10, 2), 16);
                address[1] = (byte)Convert.ToInt32(btAddress.Substring(8, 2), 16);
                address[2] = (byte)Convert.ToInt32(btAddress.Substring(6, 2), 16);
                address[3] = (byte)Convert.ToInt32(btAddress.Substring(4, 2), 16);
                address[4] = (byte)Convert.ToInt32(btAddress.Substring(2, 2), 16);
                address[5] = (byte)Convert.ToInt32(btAddress.Substring(0, 2), 16);
                address[6] = 0xaa;
                byte[] total = new byte[12];
                cmd.CopyTo(total, 0);
                address.CopyTo(total, 5);
                byte[] values = Serial.VisaQuery(total);
                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_WriteBTName(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                //48 53 21 01 06 E0 9D FA 12 34 56 AA
                string btName = data.LowLimit;
                byte[] cmd = { 0x4c, 0x43, 0x20, 0x00, (byte)btName.Length };
                byte[] name = Encoding.ASCII.GetBytes(btName);
                byte[] total = new byte[cmd.Length + name.Length + 1];

                cmd.CopyTo(total, 0);
                name.CopyTo(total, 5);
                total[total.Length - 1] = 0xaa;
                byte[] values = Serial.VisaQuery(total);
                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_ReadBTName(TestData data)
        {
            try
            {
                //4C 43 10 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x20, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    string btaddress = Encoding.ASCII.GetString(values.Skip(4)
                        .Take(values[3]).ToArray());
                    if (btaddress.Equals(data.LowLimit))
                    {
                        data.Value = btaddress;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = btaddress;
                        data.Result = "Fail";
                    }

                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_Reset(TestData data)
        {
            try
            {
                //4C 43 15  01 AA
                byte[] cmd = { 0x4c, 0x43, 0x14, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_ShutDown(TestData data)
        {
            try
            {
                //4C 43 15  01 AA
                byte[] cmd = { 0x4c, 0x43, 0x15, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_CloseLog(TestData data)
        {
            try
            {
                //4C 43 15  01 AA
                byte[] cmd = { 0x4c, 0x43, 0x19, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                //if (values[0] == 0x53)
                //{
                data.Value = "Pass";
                data.Result = "Pass";
                //}
                //else
                //{
                //    data.Value = "Fail";
                //    data.Result = "Fail";
                //}
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_04cCharge(TestData data)
        {
            try
            {
                //4C 43 50 00 AA
                byte[] cmd = { 0x4c, 0x43, 0x50, 0x00, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                //if (values[0] == 0x53)
                //{
                data.Value = "Pass";
                data.Result = "Pass";
                //}
                //else
                //{
                //    data.Value = "Fail";
                //    data.Result = "Fail";
                //}
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_1cCharge(TestData data)
        {
            try
            {
                //4C 43 50 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x50, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                //if (values[0] == 0x53)
                //{
                data.Value = "Pass";
                data.Result = "Pass";
                //}
                //else
                //{
                //    data.Value = "Fail";
                //    data.Result = "Fail";
                //}
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_TalkMic(TestData data)
        {
            try
            {
                //4C 43 50 01 AA
                //4C 43 11 01 AA
                byte[] cmd = { 0x4c, 0x43, 0x11, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_FFMIC(TestData data)
        {
            try
            {
                //4C 43 11 03 AA
                byte[] cmd = { 0x4c, 0x43, 0x11, 0x02, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_Lchse_TWS_FBMIC(TestData data)
        {
            try
            {
                //4C 43 11 03 AA
                byte[] cmd = { 0x4c, 0x43, 0x11, 0x03, 0xaa };
                byte[] values = Serial.VisaQuery(cmd);
                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_XE25_TWS_EnterDUT(TestData data)
        {
            try
            {
                //AT+FTMMODE
                string comm = "AT+FTMMODE";
                //byte[] vs = Encoding.ASCII.GetBytes(comm);
                //byte[] ret = Serial.VisaQuery(vs);
                string ret = Serial.VisaQuery(comm, 1000);

                if (ret.Length > 0)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }

            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_XE25_TWS_CloseLog(TestData data)
        {
            try
            {
                //AT+TEST
                string comm = "AT+TEST";
                byte[] vs = Encoding.ASCII.GetBytes(comm);
                byte[] ret = Serial.VisaQuery(vs);
                if (ret.Length >= 0)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }

            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_XE25_TWS_ReadVersion(TestData data)
        {
            try
            {
                //AT+READVERSION
                string comm = "AT+READVERSION";
                byte[] vs = Encoding.ASCII.GetBytes(comm);
                byte[] ret = Serial.VisaQuery(vs);
                string ver = Encoding.ASCII.GetString(ret, 0, ret.Length).Split(':')[1];


                if (ver.Remove(ver.IndexOf("\0")) == data.LowLimit)
                {
                    data.Value = ver;
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = ver;
                    data.Result = "Fail";
                }

            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_XE25_TWS_ReadMac(TestData data)
        {
            try
            {
                //AT+READMAC
                string comm = "AT+READMAC";
                byte[] vs = Encoding.ASCII.GetBytes(comm);
                byte[] ret = Serial.VisaQuery(vs);
                string ver = Encoding.ASCII.GetString(ret, 0, ret.Length).Split(':')[1];

                data.Value = ver.Remove(ver.IndexOf("\0"));
                data.Result = "Pass";

            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_XE25_TWS_ReadName(TestData data)
        {
            try
            {
                //AT+READBTNAME
                string comm = "AT+READBTNAME";
                byte[] vs = Encoding.ASCII.GetBytes(comm);
                byte[] ret = Serial.VisaQuery(vs);
                string ver = Encoding.ASCII.GetString(ret, 0, ret.Length).Split(':')[1];
                if (ver.Remove(ver.IndexOf("\0")) == data.LowLimit)
                {
                    data.Value = ver;
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = ver;
                    data.Result = "Fail";
                }

            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_XE25_TWS_ClsRepair(TestData data)
        {
            try
            {
                //AT+FATRST
                string comm = "AT+FATRST";
                byte[] vs = Encoding.ASCII.GetBytes(comm);
                byte[] ret = Serial.VisaQuery(vs);
                string ver = Encoding.ASCII.GetString(ret, 0, ret.Length);
                if ("OK".Contains(ver.Remove(ver.IndexOf("\0"))))
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }

            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_XE25_TWS_LoopBackOn(TestData data)
        {
            try
            {
                //AT+AUDIOLOOP=1
                string comm = "AT+AUDIOLOOP=1";
                byte[] vs = Encoding.ASCII.GetBytes(comm);
                byte[] ret = Serial.VisaQuery(vs);
                string ver = Encoding.ASCII.GetString(ret, 0, ret.Length);
                if ("OK".Contains(ver.Remove(ver.IndexOf("\0"))))
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }

            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_XE25_TWS_LoopBackOff(TestData data)
        {
            try
            {
                //AT+AUDIOLOOP=0
                string comm = "AT+AUDIOLOOP=0";
                byte[] vs = Encoding.ASCII.GetBytes(comm);
                byte[] ret = Serial.VisaQuery(vs);
                string ver = Encoding.ASCII.GetString(ret, 0, ret.Length);
                if ("OK".Contains(ver.Remove(ver.IndexOf("\0"))))
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }

            }
            catch (Exception)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadSoftVersion(TestData data)
        {
            try
            {
                //4C 43 01 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x01, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    string version = Encoding.ASCII.GetString(needData);
                    if (version.Equals(data.LowLimit))
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = version;
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadHardVersion(TestData data)
        {
            try
            {
                //4C 43 02 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x02, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    string version = Encoding.ASCII.GetString(needData);
                    if (version.Equals(data.LowLimit))
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = version;
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadElectricity(TestData data)
        {
            try
            {
                //4C 43 03 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x03, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    double elect = (double)needData[0];
                    if (elect <= double.Parse(data.UppLimit)
                        && elect >= double.Parse(data.LowLimit))
                    {
                        data.Value = elect.ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = elect.ToString();
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadButton(TestData data)
        {
            try
            {
                //4C 43 05 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x05, 0x01, 0xaa };


                if (data.LowLimit == "01")
                {
                    statusQueue.Enqueue("按下MBT按键");
                }
                else if (data.LowLimit == "02")
                {
                    statusQueue.Enqueue("按下+按键");
                }
                else if (data.LowLimit == "04")
                {
                    statusQueue.Enqueue("按下-按键");
                }
                for (int i = 0; i < 10; i++)
                {
                    byte[] values = Serial.VisaQuery(bytes);
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();

                    if (needData[0] == int.Parse(data.LowLimit))
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Pass";
                        statusQueue.Enqueue("Test");
                        break;
                    }
                    else
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Fail";
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoHALLTest(TestData data)
        {
            try
            {
                //4C 43 04 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x04, 0x01, 0xaa };
                byte[] values = new byte[0];
                List<byte> status = new List<byte>();
                //for (int i = 0; i < 3; i++)
                //{
                values = Serial.VisaQuery(bytes);
                //Thread.Sleep(100);
                //status.Add(values[5]);
                //}                        
                if (values[4] == 0x00)
                {
                    //HALLTest hALL = new HALLTest();
                    //hALL.ShowDialog();
                    if (config.AutoHALL)
                    {
                        FixPort.WriteLine("HALL" + "\r");
                    }
                    if (values[4] == 0x00)
                    {
                        //    Thread thread = new Thread(() =>
                        //{
                        try
                        {
                            statusQueue.Enqueue("吸合霍尔");
                            for (int i = 0; i < 10; i++)
                            {
                                values = Serial.VisaQuery(bytes);
                                if (values[4] == 0x01)
                                {
                                    data.Value = "Pass";
                                    data.Result = "Pass";
                                    //hALL.Close();
                                    statusQueue.Enqueue("End");
                                    break;
                                }
                                if (values[5] != 0x00 && i == 9)
                                {
                                    data.Value = "Fail";
                                    data.Result = "Fail";
                                    if (data.Check)
                                    {
                                        Serial.ClosedPort();
                                    }
                                    //hALL.Close();
                                    statusQueue.Enqueue("Fail");
                                }
                                Thread.Sleep(500);
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Value = "Fail";
                            data.Result = "Fail";
                            if (data.Check)
                            {
                                Serial.ClosedPort();
                            }
                            //hALL.Close();
                            statusQueue.Enqueue("Fail");
                        }
                        //});
                        //    thread.Start();
                        //    hALL.ShowDialog();

                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        if (data.Check)
                        {
                            Serial.ClosedPort();
                        }
                    }
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                    if (data.Check)
                    {
                        Serial.ClosedPort();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue("ex;" + ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_OppoReadNTC(TestData data)
        {
            try
            {
                //4C 43 06 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x06, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    double elect = (double)needData[1];
                    if (elect <= double.Parse(data.UppLimit)
                        && elect >= double.Parse(data.LowLimit))
                    {
                        data.Value = elect.ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = elect.ToString();
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadVoltage(TestData data)
        {
            try
            {
                //4C 43 07 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x07, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    double elect = double.Parse(Encoding.ASCII.GetString(needData));
                    if (elect <= double.Parse(data.UppLimit)
                        && elect >= double.Parse(data.LowLimit))
                    {
                        data.Value = elect.ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = elect.ToString();
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoEnterDUT(TestData data)
        {
            try
            {
                //4C 43 10 01 01 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x10, 0x01, 0x01, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                //if (values[0] == 0x53)
                //{
                data.Value = "Pass";
                data.Result = "Pass";
                //}
                //else
                //{
                //    data.Value = "Fail";
                //    data.Result = "Fail";
                //}
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReset(TestData data)
        {
            try
            {
                //4C 43 14 00 AA
                byte[] bytes = { 0x4c, 0x43, 0x14, 0x00, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }


            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoPowerOff(TestData data)
        {
            try
            {
                //4C 43 15 00 AA
                byte[] bytes = { 0x4c, 0x43, 0x15, 0x00, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }


            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoFactorySettings(TestData data)
        {
            try
            {
                //4C 43 16 00 AA
                byte[] bytes = { 0x4c, 0x43, 0x16, 0x00, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }


            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoChargeManage(TestData data)
        {
            try
            {
                //4C 43 1A 01 01 00 AA
                //4C 43 1A 01 01 01 AA
                //4C 43 1A 01 01 02 AA
                byte[] bytes = { 0x4c, 0x43, 0x1a, 0x01, 0x01, 0x00, 0xaa };
                byte val = byte.Parse(data.LowLimit);
                bytes[5] = val;
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    if (needData[0] == val)
                    {
                        data.Value = val.ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = val.ToString();
                        data.Result = "Fail";
                    }
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }


            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadChargeManage(TestData data)
        {
            try
            {
                //4C 43 1A 01 01 00 AA
                //4C 43 1A 01 01 01 AA
                //4C 43 1A 01 01 02 AA
                byte[] bytes = { 0x4c, 0x43, 0x1a, 0x01, 0x01, 0x02, 0xaa };
                byte val = byte.Parse(data.LowLimit);
                //bytes[5] = val;
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    if (needData[0] == val)
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Fail";
                    }
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }


            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoShipping(TestData data)
        {
            try
            {
                //4C 43 1B 00 AA
                byte[] bytes = { 0x4c, 0x43, 0x1b, 0x00, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }


            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadBtName(TestData data)
        {
            try
            {
                //4C 43 20 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x20, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    string version = Encoding.ASCII.GetString(needData);
                    if (version.Equals(data.LowLimit))
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = version;
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadBtAddress(TestData data)
        {
            try
            {
                //4C 43 21 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x21, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    Array.Reverse(needData);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        sb.Append(needData[i].ToString("x2"));
                    }
                    string btaddress = sb.ToString().ToUpper();
                    if (config.MesEnable)
                    {
                        //btaddress = "28FA1972BDFD";
                        queue.Enqueue("检查蓝牙地址是否过站/重复/测试三次:" + btaddress);
                        //ServiceReference1.WebService1SoapClient MesWeb = null;
                        //MesWeb = new ServiceReference1.WebService1SoapClient("WebService1Soap");
                        MesWeb = new WebReference.WebService1();
                        //MesWeb.Url = "http://218.65.34.28:82/WebService1.asmx";
                        //btaddress = "E09DFA6A1BB3";
                        string failResult = string.Empty;
                        string reslut = string.Empty;
                        //string packResult = string.Empty;
                        string btResult = string.Empty;
                        if (PackSN == null)
                            PackSN = string.Empty;

                        if (PackSN.Length != config.SNLength)
                        {
                            //btaddress = "E09DFA513DF6";
                            //半成品工站拦截蓝牙地址重复
                            //btResult = MesWeb.SnCx_LY(btaddress);
                            //检查上一工站是否Pass
                            reslut = MesWeb.SnCx(btaddress, config.MesStation);
                            //检查是否测试三次
                            failResult = MesWeb.SnCx_SC(btaddress, config.NowStation);
                        }
                        else
                        {
                            //reslut = MesWeb.SnCx(btaddress, config.MesStation);
                            //已在输入20位SN时检测
                            reslut = "P";
                            //检查是否测试三次
                            failResult = MesWeb.SnCx_SC(btaddress, config.NowStation);
                            //检查包装段蓝牙地址是否重复
                            //btResult = MesWeb.SnCx_BZLY(btaddress);
                        }

                        //MesWeb.Close();
                        MesWeb.Abort();

                        if (reslut.Contains("F"))
                        {
                            data.Value = btaddress;
                            data.Result = "Fail";
                            queue.Enqueue(string.Format("上一个工位:{0}:连续测试NG品,请检查 "
                        , config.MesStation));
                        }
                        else
                        {
                            queue.Enqueue("检查蓝牙地址: " + btaddress + ",过站Pass");
                            data.Result = "Pass";
                            data.Value = btaddress;
                            if (PackSN.Length != config.SNLength)
                            {
                                if (failResult.Contains("P"))
                                {
                                    queue.Enqueue("该蓝牙地址: " + btaddress + ",无重复测试");
                                    data.Result = "Pass";
                                    data.Value = btaddress;
                                }
                                else
                                {
                                    data.Value = btaddress;
                                    data.Result = "Fail";
                                    queue.Enqueue("该蓝牙地址: " + btaddress + ",重复测试三次");
                                }
                            }
                        }

                    }
                    else
                    {
                        data.Result = "Pass";
                        data.Value = btaddress;
                    }
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadBattarySN(TestData data)
        {
            try
            {
                //4C 43 23 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x23, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    string version = Encoding.ASCII.GetString(needData);

                    data.Value = version;
                    data.Result = "Pass";
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoCompareBattarySN(TestData data)
        {
            try
            {
                //4C 43 23 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x23, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    string version = Encoding.ASCII.GetString(needData);

                    if (version.Equals(btAddress))
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = version;
                        data.Result = "Fail";
                    }

                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoWriteBattarySN(TestData data)
        {
            try
            {
                //4C 43 23 00 len data AA(len = 0x10 数据大小为16字节)
                byte[] bytes = { 0x4c, 0x43, 0x23, 0x00, 0x00 };
                //byte[] values = Serial.VisaQuery(bytes);
                byte[] address = Encoding.ASCII.GetBytes(btAddress);
                byte length = byte.Parse(btAddress.Length.ToString());
                byte[] value = new byte[bytes.Length + btAddress.Length + 1];
                bytes.CopyTo(value, 0);
                value[4] = length;
                address.CopyTo(value, 5);
                value[value.Length - 1] = 0xaa;

                byte[] ret = Serial.VisaQuery(value);
                if (ret[0] == 0x53)
                {
                    data.Result = "Pass";
                    data.Value = btAddress;
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = btAddress;
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadTelecomSN(TestData data)
        {
            try
            {
                //4C 43 24 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x24, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    string version = Encoding.ASCII.GetString(needData);

                    data.Value = version;
                    data.Result = "Pass";
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoCompareTelecomSN(TestData data)
        {
            try
            {
                //4C 43 24 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x24, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    string version = Encoding.ASCII.GetString(needData);

                    if (btAddress.Equals(version))
                    {
                        data.Value = version;
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = version;
                        data.Result = "Fail";
                    }
                }
                else
                {
                    data.Value = "Fail";
                    data.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoWriteTelecomSN(TestData data)
        {
            try
            {
                //4C 43 24 00 len data AA(len = 0x10 数据大小为16字节)
                byte[] bytes = { 0x4c, 0x43, 0x24, 0x00, 0x00 };
                //byte[] values = Serial.VisaQuery(bytes);
                if (config.AutoSN)
                {
                    byte[] address = Encoding.ASCII.GetBytes
                        (GetOppoProductSN(config.SNHear, config.SNLine));
                    byte length = byte.Parse(address.Length.ToString());
                    byte[] value = new byte[bytes.Length + address.Length + 1];
                    bytes.CopyTo(value, 0);
                    value[4] = length;
                    address.CopyTo(value, 5);
                    value[value.Length - 1] = 0xaa;

                    byte[] ret = Serial.VisaQuery(value);
                    if (ret[0] == 0x53)
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    byte[] address = Encoding.ASCII.GetBytes(btAddress);
                    byte length = byte.Parse(address.Length.ToString());
                    byte[] value = new byte[bytes.Length + address.Length + 1];
                    bytes.CopyTo(value, 0);
                    value[4] = length;
                    address.CopyTo(value, 5);
                    value[value.Length - 1] = 0xaa;

                    byte[] ret = Serial.VisaQuery(value);
                    if (ret[0] == 0x53)
                    {
                        data.Result = "Pass";
                        data.Value = "Pass";
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }


            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadEQ(TestData data)
        {
            try
            {
                //4C 43 29 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x29, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();

                    if (needData[0] == byte.Parse(data.LowLimit))
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoWriteEQ(TestData data)
        {
            try
            {
                //4C 43 29 00 len data AA(len = 0x01 数据大小为1字节)

                Byte[] bytes = { 0x4c, 0x43, 0x29, 0x00, 0x01, 0x00, 0xaa };
                byte eq = byte.Parse(data.LowLimit);
                bytes[5] = eq;
                byte[] ret = Serial.VisaQuery(bytes);
                if (ret[0] == 0x53)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoClsElectricity(TestData data)
        {
            try
            {
                //4C 43 19 00 AA

                Byte[] bytes = { 0x4c, 0x43, 0x19, 0x00, 0xaa };

                byte[] ret = Serial.VisaQuery(bytes);
                if (ret[0] == 0x53)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Pass";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadENC(TestData data)
        {
            try
            {
                //4C 43 2A 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x2A, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    if (needData[0] == byte.Parse(data.LowLimit))
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoWriteENC(TestData data)
        {
            try
            {
                //4C 43 2A 00 len data AA(len = 0x01 数据大小为1字节)
                Byte[] bytes = { 0x4c, 0x43, 0x2a, 0x00, 0x01, 0x00, 0xaa };
                byte eq = byte.Parse(data.LowLimit);
                bytes[5] = eq;
                byte[] ret = Serial.VisaQuery(bytes);
                if (ret[0] == 0x53)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadColor(TestData data)
        {
            try
            {
                //4C 43 2B 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x2b, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();

                    if (needData[0] == byte.Parse(data.LowLimit))
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoWriteColor(TestData data)
        {
            try
            {
                //4C 43 2B 00 len data AA(len = 0x01 数据大小为1字节)

                Byte[] bytes = { 0x4c, 0x43, 0x2b, 0x00, 0x01, 0x00, 0xaa };
                byte eq = byte.Parse(data.LowLimit);
                bytes[5] = eq;
                byte[] ret = Serial.VisaQuery(bytes);
                if (ret[0] == 0x53)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData BES_OppoReadEncLicense(TestData data)
        {
            try
            {
                //4C 43 08 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x08, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();

                    if (needData[0] == byte.Parse(data.LowLimit))
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = needData[0].ToString();
                        data.Result = "Fail";
                    }
                }

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public string BES_XE25_TWS_ReadMac()
        {
            string add = string.Empty;
            try
            {
                //AT+READMAC
                string comm = "AT+READMAC";
                byte[] vs = Encoding.ASCII.GetBytes(comm);
                byte[] ret = Serial.VisaQuery(vs);
                string ver = Encoding.ASCII.GetString(ret, 0, ret.Length).Split(':')[1];

                add = ver.Remove(ver.IndexOf("\0"));

            }
            catch (Exception)
            {

            }
            return add;
        }

        public string BES_OppoReadBtAddress()
        {
            string mac = string.Empty;
            try
            {
                //4C 43 21 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x21, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    byte[] needData = values.Skip(4).Take(values[3]).ToArray();
                    Array.Reverse(needData);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < needData.Length; i++)
                    {
                        sb.Append(needData[i].ToString("x2"));
                    }

                    mac = sb.ToString().ToUpper();
                }


            }
            catch (Exception ex)
            {

            }
            return mac;
        }

        public string BES_ReadBTAddress()
        {
            string address = string.Empty;
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x05, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[2] == 0x06)
                {
                    address = string.Format("{0:x2}:{1:x2}:{2:x2}:{3:x2}:{4:x2}:{5:x2}"
                       , values[8], values[7], values[6], values[5], values[4], values[3]);
                }

            }
            catch (Exception ex)
            {
                Serial.ClosedPort();
            }
            return address;
        }

        public bool BES_WriteTrim(byte trim)
        {
            bool status = false; ;
            try
            {
                byte[] bytes = { 0x04, 0xff, 0x08, 0x00, 0x01, trim };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[3] == trim)
                {
                    status = true;
                }

            }
            catch (Exception ex)
            {
                Serial.ClosedPort();
            }
            return status;
        }

        public bool BES_Lchse_WriteTrim(byte trim)
        {
            bool status = false; ;
            try
            {
                byte[] bytes = { 0x4c, 0x43, 0x22, 0x00, 0x01, trim, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    status = true;
                }

            }
            catch (Exception ex)
            {
                Serial.ClosedPort();
            }
            return status;
        }

        public int BES_Lchse_ReadTrim()
        {
            int trim = 0;
            try
            {
                //4C 43 22 01 AA
                byte[] bytes = { 0x4c, 0x43, 0x22, 0x01, 0xaa };
                byte[] values = Serial.VisaQuery(bytes);

                if (values[0] == 0x53)
                {
                    trim = values[4];
                }

            }
            catch (Exception ex)
            {
                Serial.ClosedPort();
            }
            return trim;
        }

        public void SetVolume()
        {
            //04 ff 14 00 01 11
            Serial.setVolume(new byte[] { 0x04, 0xff, 0x14, 0x00, 0x01, 0x11 });
        }

        private string GetProductSN(string Head, string line)
        {
            string sn = string.Empty;
            string year = DateTime.Now.Year.ToString().Substring(2, 2);
            string[] months = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C" };
            string[] days = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F",
          "G", "H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y"};

            string month = months[DateTime.Now.Month - 1];
            string day = days[DateTime.Now.Day - 1];
            var table = dataBase.Getsn();
            string num = table.Rows[0][1].ToString();
            string compDay = table.Rows[0][0].ToString();
            if (!day.Equals(compDay))
            {
                num = "1";
            }
            switch (num.Length)
            {
                case 1:
                    {
                        num = string.Format("0000{0}", num);
                        break;
                    }
                case 2:
                    {
                        num = string.Format("000{0}", num);
                        break;
                    }
                case 3:
                    {
                        num = string.Format("00{0}", num);
                        break;
                    }
                case 4:
                    {
                        num = string.Format("0{0}", num);
                        break;
                    }
                case 5:
                    {
                        num = string.Format("{0}", num);
                        break;
                    }
            }
            sn = string.Format("{0}{1}{2}{3}{4}{5}", Head, year, month, day, line, num);
            dataBase.UpdateSN(day, int.Parse(num) + 1);
            return sn;
        }

        private string GetOppoProductSN(string Head, string line)
        {
            string sn = string.Empty;
            string[] years = { "A", "B", "C", "D", "E" };
            string[] months = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C" };
            string[] days = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F",
          "G", "H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y"};

            string year = DateTime.Now.Year.ToString().Substring(3, 1);
            string month = months[DateTime.Now.Month - 1];
            string day = days[DateTime.Now.Day - 1];
            var table = dataBase.Getsn();
            string num = table.Rows[0][1].ToString();
            string compDay = table.Rows[0][0].ToString();
            if (!day.Equals(compDay))
            {
                num = "1";
            }
            switch (num.Length)
            {
                case 1:
                    {
                        num = string.Format("0000{0}", num);
                        break;
                    }
                case 2:
                    {
                        num = string.Format("000{0}", num);
                        break;
                    }
                case 3:
                    {
                        num = string.Format("00{0}", num);
                        break;
                    }
                case 4:
                    {
                        num = string.Format("0{0}", num);
                        break;
                    }
                case 5:
                    {
                        num = string.Format("{0}", num);
                        break;
                    }
            }
            sn = string.Format("{0}{1}{2}{3}{4}{5}", Head, year, month, day, line, num);
            dataBase.UpdateSN(day, int.Parse(num) + 1);
            return sn;
        }

        private string GetTCLProductSN(string Head)
        {
            string sn = string.Empty;
            string year = DateTime.Now.Year.ToString().Substring(2, 2);
            string[] years = { "E", "F", "G", "H", "I" };
            string[] months = { "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
            string[] days = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F",
          "G", "H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V"};


            string month = months[DateTime.Now.Month - 1];
            string day = days[DateTime.Now.Day - 1];
            var table = dataBase.Getsn();
            string num = table.Rows[0][1].ToString();
            string compDay = table.Rows[0][0].ToString();
            if (!day.Equals(compDay))
            {
                num = "1";
            }
            switch (num.Length)
            {
                case 1:
                    {
                        num = string.Format("000{0}", num);
                        break;
                    }
                case 2:
                    {
                        num = string.Format("00{0}", num);
                        break;
                    }
                case 3:
                    {
                        num = string.Format("0{0}", num);
                        break;
                    }
                case 4:
                    {
                        num = string.Format("{0}", num);
                        break;
                    }

            }
            sn = string.Format("{0}{1}{2}{3}{4}", Head, day, month, year, num);
            dataBase.UpdateSN(day, int.Parse(num) + 1);
            return sn;
        }

        private byte[] GetTwsCommand(byte cmd)
        {
            int num;
            byte[] buffer = new byte[8];
            buffer[0] = 0xbc;
            buffer[1] = 8;
            buffer[2] = cmd;
            for (num = 3; num < 7; num++)
            {
                buffer[num] = 0;
            }
            byte num2 = 0;
            for (num = 0; num < 7; num++)
            {
                num2 = (byte)(num2 + buffer[num]);
            }
            buffer[7] = num2;
            return buffer;
        }

        public bool BES_T1207CalcFreq(int value)
        {
            try
            {
                //55 AA FF 15 01 1E 99
                byte[] buf = { 0x55, 0xAA, 0xFF, 0x15, 0x01, 0x00 };
                byte[] total = new byte[7];
                buf[5] = (byte)value;
                buf.CopyTo(total, 0);

                total[6] = crc8_maxim(buf, buf.Length);
                byte[] retvalue = Serial.VisaQuery(total);

                if (retvalue[0] == 0xaa)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static byte CalcCheck(byte[] data)
        {
            //byte[] data = { 0xbb, 0xdd, 0xe4, 0x07, 0x03, 0x57, 0x04, 0xc6, 0x10
            //,0x00,0x01};
            int i, result;
            for (result = data[0], i = 1; i < data.Length; i++)
            {
                result ^= data[i];
            }
            return (byte)result;
        }

        public static byte crc8(byte[] buffer, int len)
        {
            byte crc, i, j;
            crc = 0;

            for (j = 0; j < len; j++)
            {
                for (i = 0x01; i != 0; i <<= 1)
                {
                    if (((crc & 0x01) ^ (buffer[j] & i)) == 1)
                    {
                        crc ^= 0x18;
                        crc >>= 1;
                        crc |= 0x80;
                        //crc = (byte)((crc >> 1) ^ 0x8c);
                    }
                    else
                        crc = (byte)(crc >> 1);
                }
            }
            return crc;
        }

        public byte crc8_maxim(byte[] buf, int length)
        {
            int i;
            byte crc = 0;
            for (i = 0; i < length; i++)
            {
                crc = vusb_crc8_tbl[(crc ^ buf[i])];
            }
            return crc;
        }

        static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }

        byte[] vusb_crc8_tbl = {
    0x00, 0x5e, 0xbc, 0xe2, 0x61, 0x3f, 0xdd, 0x83, 0xc2, 0x9c, 0x7e, 0x20, 0xa3, 0xfd, 0x1f, 0x41,
    0x9d, 0xc3, 0x21, 0x7f, 0xfc, 0xa2, 0x40, 0x1e, 0x5f, 0x01, 0xe3, 0xbd, 0x3e, 0x60, 0x82, 0xdc,
    0x23, 0x7d, 0x9f, 0xc1, 0x42, 0x1c, 0xfe, 0xa0, 0xe1, 0xbf, 0x5d, 0x03, 0x80, 0xde, 0x3c, 0x62,
    0xbe, 0xe0, 0x02, 0x5c, 0xdf, 0x81, 0x63, 0x3d, 0x7c, 0x22, 0xc0, 0x9e, 0x1d, 0x43, 0xa1, 0xff,
    0x46, 0x18, 0xfa, 0xa4, 0x27, 0x79, 0x9b, 0xc5, 0x84, 0xda, 0x38, 0x66, 0xe5, 0xbb, 0x59, 0x07,
    0xdb, 0x85, 0x67, 0x39, 0xba, 0xe4, 0x06, 0x58, 0x19, 0x47, 0xa5, 0xfb, 0x78, 0x26, 0xc4, 0x9a,
    0x65, 0x3b, 0xd9, 0x87, 0x04, 0x5a, 0xb8, 0xe6, 0xa7, 0xf9, 0x1b, 0x45, 0xc6, 0x98, 0x7a, 0x24,
    0xf8, 0xa6, 0x44, 0x1a, 0x99, 0xc7, 0x25, 0x7b, 0x3a, 0x64, 0x86, 0xd8, 0x5b, 0x05, 0xe7, 0xb9,
    0x8c, 0xd2, 0x30, 0x6e, 0xed, 0xb3, 0x51, 0x0f, 0x4e, 0x10, 0xf2, 0xac, 0x2f, 0x71, 0x93, 0xcd,
    0x11, 0x4f, 0xad, 0xf3, 0x70, 0x2e, 0xcc, 0x92, 0xd3, 0x8d, 0x6f, 0x31, 0xb2, 0xec, 0x0e, 0x50,
    0xaf, 0xf1, 0x13, 0x4d, 0xce, 0x90, 0x72, 0x2c, 0x6d, 0x33, 0xd1, 0x8f, 0x0c, 0x52, 0xb0, 0xee,
    0x32, 0x6c, 0x8e, 0xd0, 0x53, 0x0d, 0xef, 0xb1, 0xf0, 0xae, 0x4c, 0x12, 0x91, 0xcf, 0x2d, 0x73,
    0xca, 0x94, 0x76, 0x28, 0xab, 0xf5, 0x17, 0x49, 0x08, 0x56, 0xb4, 0xea, 0x69, 0x37, 0xd5, 0x8b,
    0x57, 0x09, 0xeb, 0xb5, 0x36, 0x68, 0x8a, 0xd4, 0x95, 0xcb, 0x29, 0x77, 0xf4, 0xaa, 0x48, 0x16,
    0xe9, 0xb7, 0x55, 0x0b, 0x88, 0xd6, 0x34, 0x6a, 0x2b, 0x75, 0x97, 0xc9, 0x4a, 0x14, 0xf6, 0xa8,
    0x74, 0x2a, 0xc8, 0x96, 0x15, 0x4b, 0xa9, 0xf7, 0xb6, 0xe8, 0x0a, 0x54, 0xd7, 0x89, 0x6b, 0x35,
};
    }
}
