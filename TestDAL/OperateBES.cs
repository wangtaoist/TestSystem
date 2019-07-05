using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestModel;
using System.Windows.Forms;
using System.Threading;
using TestTool;

namespace TestDAL
{
    public class OperateBES
    {
        private SerialOperate Serial;
        private string port;
        public string btAddress = string.Empty;
        private Queue<string> queue;
        public DataBase dataBase;
        public ConfigData config;

        public OperateBES(string port,Queue<string> queue,bool serialStatus)
        {
            this.port = port;
            Serial = new SerialOperate(port, serialStatus,queue);
            this.queue = queue;
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
                byte[] values = Serial.VisaQuery(bytes);
                var version = Encoding.ASCII.GetString(values).Replace("\0", "")
                    .Remove(0, 3);
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
                    string sn = Encoding.ASCII.GetString(values).Replace("\0", "")
                        .Remove(0, 2).ToUpper();
                    data.Result = "Pass";
                    data.Value = sn;
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
                    string sn = Encoding.ASCII.GetString(values).Replace("\0", "")
                        .Remove(0, 2).ToUpper();
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
                        if(sn.StartsWith(data.LowLimit))
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
                        , values[8], values[7], values[6], values[5], values[4], values[3]);
                    data.Result = "Pass";
                    data.Value = btaddress.ToUpper();
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

                string batt = Encoding.ASCII.GetString(values).Replace("\0", "")
                    .Remove(0, 3);
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

                string batt = Encoding.ASCII.GetString(values).Replace("\0", "")
                    .Remove(0, 3);
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
                if (config.AutoSNTest)
                {
                    btAddress = GetProductSN(config.SNHear, config.SNLine);
                    address = Encoding.ASCII.GetBytes(btAddress);
                  
                }
                else
                {
                    address = Encoding.ASCII.GetBytes(btAddress);
                }
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
