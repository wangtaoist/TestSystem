using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestModel;
using System.Windows.Forms;
using System.Threading;
using TestTool;
using System.IO.Ports;

namespace TestDAL
{
    public class OperateBES
    {
        private SerialOperate Serial;
        private string port;
        public string btAddress = string.Empty;
        private Queue<string> queue, statusQueue;
        public DataBase dataBase;
        public ConfigData config;
        private WebReference.WebService1 MesWeb;
        public SerialPort FixPort;
        public string PackSN;
        public string BurnINFail;
        
        public OperateBES(string port,Queue<string> queue,bool serialStatus
            ,Queue<string> statusQueue)
        {
            this.port = port;
            Serial = new SerialOperate(port, serialStatus,queue);
            this.queue = queue;
            this.statusQueue = statusQueue;
        }

        public TestData OpenSerialPort(TestData data)
        {
            try

            {
                Serial.OpenSerialPort();
                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
            }
            return data;
        }

        public TestData  ReadSoftVersion(TestData data)
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
            catch(Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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

                    string reslut = string.Empty;
                    if (config.MesEnable && PackSN.Length == 20)
                    {
                        MesWeb = new WebReference.WebService1();
                        reslut = MesWeb.SnCx_BZSN(sn);
                        if (reslut == "P")
                        {
                            queue.Enqueue("产品SN未重复");
                        }
                        MesWeb.Abort();
                    }
                    else
                    {
                        reslut = "P";
                    }
                    if (reslut.Contains("P"))
                    {
                        data.Result = "Pass";
                        data.Value = sn;
                    }
                    else
                    {
                        queue.Enqueue("产品SN重复，请检查");
                        data.Result = "Fail";
                        data.Value = sn;
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
                queue.Enqueue(ex.Message);
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
                    if (config.MesEnable && PackSN.Length == 20)
                    {
                        //sn = "GJ1225202K100773";
                        MesWeb = new WebReference.WebService1();
                        reslut = MesWeb.SnCx_BZSN(sn);
                        if(reslut == "P")
                        {
                            queue.Enqueue("产品SN未重复");
                        }
                        MesWeb.Abort();
                    }
                    else
                    {
                        reslut = "P";
                    }
                    if (reslut.Contains("P"))
                    {
                       
                        if (btAddress != "")
                        {
                            if (btAddress == sn && btAddress.StartsWith(data.LowLimit))
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
                    else
                    {
                        queue.Enqueue("产品SN重复，请检查");
                        data.Result = "Fail";
                        data.Value = sn;
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                    if (config.MesEnable)
                    {
                        queue.Enqueue("检查蓝牙地址是否过站/重复/测试三次:" + btaddress);
                        //ServiceReference1.WebService1SoapClient MesWeb = null;
                        //MesWeb = new ServiceReference1.WebService1SoapClient("WebService1Soap");
                        MesWeb = new WebReference.WebService1();
                        //MesWeb.Url = "http://218.65.34.28:82/WebService1.asmx";
                        //btaddress = "E09DFA52CD40";
                        string failResult = string.Empty;
                        string reslut = string.Empty;
                        //string packResult = string.Empty;
                        string btResult = string.Empty;
                        if (PackSN == null)
                            PackSN = string.Empty;
                        
                        if (PackSN.Length != 20)
                        {
                            //btaddress = "E09DFA513DF6";
                            reslut = MesWeb.SnCx(btaddress, config.MesStation);
                            failResult = MesWeb.SnCx_SC(btaddress, config.NowStation);
                        }
                        else
                        {
                            //reslut = MesWeb.SnCx(btaddress, config.MesStation);
                            reslut = "P";
                            failResult = MesWeb.SnCx_SC(btaddress, config.NowStation);
                            btResult = MesWeb.SnCx_BZLY(btaddress);
                        }

                        //MesWeb.Close();
                        MesWeb.Abort();
                       
                        if (reslut.Contains( "F"))
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
                            if (PackSN.Length != 20)
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
                        if (btResult != string.Empty)
                        {
                            if (btResult.Contains("P"))
                            {
                                queue.Enqueue("该蓝牙地址: " + btaddress + ",无重复");
                                data.Result = "Pass";
                                data.Value = btaddress;
                            }
                            else
                            {
                                data.Value = btaddress;
                                data.Result = "Fail";
                                queue.Enqueue("该蓝牙地址: " + btaddress + ",重复,请检查");
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
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue(ex.Message);
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
                data.Result = "Pass";
                data.Value = batt;
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue(ex.Message);
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
                    if(batt.StartsWith(data.LowLimit))
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
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
                queue.Enqueue(ex.Message);
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
                    else if (product == 0x02)
                        prod = "华为绿色";
                    else if (product == 0x03)
                        prod = "华为银色";
                    else if (product == 0x04)
                        prod = "华为紫色";
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
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
                queue.Enqueue(ex.Message);
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
                byte[] bytes = { 0x04, 0xff, 0x14, 0x00, 0x01,0x11 };
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                byte[] bytes = { 0x04, 0xff, 0x0b, 0x01};
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
            catch(Exception ex)
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
               if(BurnINFail == "正常状态下,HALL异常")
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
                    if(voltage >=4.4)
                    {
                        values = Serial.VisaQuery(bytes);
                        if(values[3] == 0x04)
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                if ( result >= 0 )
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
        }

        public TestData BES_PowerKeyTest(TestData data)
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
                    HALLTest hALL = new HALLTest();
                    //hALL.ShowDialog();
                    if (values[5] == 0x01)
                    {
                        Thread thread = new Thread(() =>
                        {
                            try
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    values = Serial.VisaQuery(bytes);
                                    if (values[5] == 0x00)
                                    {
                                        data.Value = "Pass";
                                        data.Result = "Pass";
                                        hALL.Close();
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
                                        hALL.Close();
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
                                hALL.Close();
                            }
                        });
                        thread.Start();
                        hALL.ShowDialog();
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                else if (name == "华为绿色")
                    bytes[5] = 0x02;
                else if (name == "华为银色")
                    bytes[5] = 0x03;
                else if (name == "华为紫色")
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                byte[] bytes = { 0x04, 0xff, 0x22, 0x00, 0x01, 0x00 };
                byte[] values = Serial.VisaQuery(bytes);
               if(values[0] == 0x04)
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
                queue.Enqueue(ex.Message);
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
                byte[] bytes = { 0x04, 0xff, 0x22, 0x00, 0x01, 0x01 };
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
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
                queue.Enqueue(ex.Message);
                if (data.Check)
                {
                    Serial.ClosedPort();
                }
            }
            return data;
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
            if(!day.Equals(compDay))
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
    }
}
