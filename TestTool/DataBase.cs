using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using TestModel;

namespace TestTool
{
    public class DataBase
    {
        public string dbPath;
        private static OleDbConnection oleDbConnection;

        public DataBase(string path)
        {
            dbPath = path;
        }

        private OleDbConnection ConnectDB()
        {
            oleDbConnection = new OleDbConnection();
            try
            {
                //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=
                //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=
                oleDbConnection.ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0" +
                    ";Data Source={0}", dbPath);
                if (oleDbConnection.State == System.Data.ConnectionState.Closed)
                {
                    oleDbConnection.Open();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oleDbConnection;
        }

        private void ClosedConnect()
        {
            oleDbConnection.Close();
            oleDbConnection.Dispose();
        }

        public List<TestData> GetTestItem()
        {
            List<TestData> list = new List<TestData>();
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "select * from Test_Items";
                cmd.CommandTimeout = 20;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool show = bool.Parse(dt.Rows[i].ItemArray[10].ToString()
                       == "" ? "false" : dt.Rows[i].ItemArray[10].ToString());

                    list.Add(new TestData()
                    {
                        ID = show == true ? (j += 1).ToString() : "",
                        TestItem = dt.Rows[i].ItemArray[0].ToString(),
                        TestItemName = dt.Rows[i].ItemArray[1].ToString(),
                        Unit = dt.Rows[i].ItemArray[2].ToString(),
                        UppLimit = dt.Rows[i].ItemArray[3].ToString(),
                        LowLimit = dt.Rows[i].ItemArray[4].ToString(),
                        beferTime = int.Parse(dt.Rows[i].ItemArray[5].ToString()),
                        AfterTime = int.Parse(dt.Rows[i].ItemArray[6].ToString()),
                        Remark = dt.Rows[i].ItemArray[7].ToString(),
                        Other = dt.Rows[i].ItemArray[8].ToString(),
                        Check = bool.Parse(dt.Rows[i].ItemArray[9].ToString() == ""
                        ? "false" : dt.Rows[i].ItemArray[9].ToString()),
                        Show = bool.Parse(dt.Rows[i].ItemArray[10].ToString() == ""
                        ? "false" : dt.Rows[i].ItemArray[10].ToString()),
                        FillValue = dt.Rows[i].ItemArray[11].ToString() == "" ? "0"
                        : dt.Rows[i].ItemArray[11].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ClosedConnect();
            return list;
        }

        public List<TestData> GetFailItem()
        {
            List<TestData> list = new List<TestData>();
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "select * from Fail_Items";
                cmd.CommandTimeout = 20;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new TestData()
                    {
                        ID = (i + 1).ToString(),
                        TestItem = dt.Rows[i].ItemArray[0].ToString(),
                        TestItemName = dt.Rows[i].ItemArray[1].ToString(),
                        Unit = dt.Rows[i].ItemArray[2].ToString(),
                        UppLimit = dt.Rows[i].ItemArray[3].ToString(),
                        LowLimit = dt.Rows[i].ItemArray[4].ToString(),
                        beferTime = int.Parse(dt.Rows[i].ItemArray[5].ToString()),
                        AfterTime = int.Parse(dt.Rows[i].ItemArray[6].ToString()),
                        Remark = dt.Rows[i].ItemArray[7].ToString(),
                        Other = dt.Rows[i].ItemArray[8].ToString(),
                        Check = bool.Parse(dt.Rows[i].ItemArray[9].ToString() == ""
                        ? "false" : dt.Rows[i].ItemArray[9].ToString()),
                        Show = bool.Parse(dt.Rows[i].ItemArray[10].ToString() == ""
                        ? "false" : dt.Rows[i].ItemArray[10].ToString()),
                        FillValue = dt.Rows[i].ItemArray[11].ToString() == "" ? "0"
                        : dt.Rows[i].ItemArray[11].ToString()
                    });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            ClosedConnect();
            return list;
        }

        public ConfigData GetConfigData()
        {
            ConfigData list = new ConfigData();
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "select * from Application_Config";
                cmd.CommandTimeout = 20;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                list.Title = dt.Rows[0].ItemArray[1].ToString();
                list.VisaPort = dt.Rows[1].ItemArray[1].ToString();
                list.SerialPort = dt.Rows[2].ItemArray[1].ToString();
                list.Sen_TX_Power = dt.Rows[3].ItemArray[1].ToString();
                list.number_of_packets = dt.Rows[4].ItemArray[1].ToString();
                list.Low_Freq = dt.Rows[5].ItemArray[1].ToString();
                list.Mod_Freq = dt.Rows[6].ItemArray[1].ToString();
                list.Hi_Freq = dt.Rows[7].ItemArray[1].ToString();
                list.Low_Loss = dt.Rows[8].ItemArray[1].ToString();
                list.Mod_Loss = dt.Rows[9].ItemArray[1].ToString();
                list.Hi_Loss = dt.Rows[10].ItemArray[1].ToString();
                list.Inquiry_TimeOut = dt.Rows[11].ItemArray[1].ToString();
                list.PeakPower = dt.Rows[12].ItemArray[1].ToString();
                list.AvgPowerLow = dt.Rows[13].ItemArray[1].ToString();
                list.AvgPowerHi = dt.Rows[14].ItemArray[1].ToString();

                list.OP = bool.Parse(dt.Rows[15].ItemArray[1].ToString());
                list.IC = bool.Parse(dt.Rows[16].ItemArray[1].ToString());
                list.CD = bool.Parse(dt.Rows[17].ItemArray[1].ToString());
                list.SS = bool.Parse(dt.Rows[18].ItemArray[1].ToString());
                list.MI = bool.Parse(dt.Rows[19].ItemArray[1].ToString());
                list.PowerPort = dt.Rows[20].ItemArray[1].ToString();
                list.Voltage1 = dt.Rows[21].ItemArray[1].ToString();
                list.Voltage2 = dt.Rows[22].ItemArray[1].ToString();
                list.Current = dt.Rows[23].ItemArray[1].ToString();
                list.GPIB_Enable = bool.Parse(dt.Rows[24].ItemArray[1].ToString());
                list.Serial_Enable = bool.Parse(dt.Rows[25].ItemArray[1].ToString());
                list.Power_Enable = bool.Parse(dt.Rows[26].ItemArray[1].ToString());
                list.CompareString = dt.Rows[27].ItemArray[1].ToString();
                list.SNLength = dt.Rows[28].ItemArray[1].ToString() == ""
                    ? 0 : int.Parse(dt.Rows[28].ItemArray[1].ToString());
                list.MultimeterPort = dt.Rows[29].ItemArray[1].ToString();
                list.Multimeter_Select = bool.Parse(dt.Rows[30].ItemArray[1].ToString());

                list.AudioEnable = bool.Parse(dt.Rows[31].ItemArray[1].ToString());
                list.AudioPath = dt.Rows[32].ItemArray[1].ToString();

                list.SerialSelect = bool.Parse(dt.Rows[33].ItemArray[1].ToString());

                list.AutoSNTest = bool.Parse(dt.Rows[34].ItemArray[1].ToString());
                list.SNHear = dt.Rows[35].ItemArray[1].ToString();
                list.SNLine = dt.Rows[36].ItemArray[1].ToString();

                list.AutoFixture = bool.Parse(dt.Rows[37].ItemArray[1].ToString());
                list.FixturePort = dt.Rows[38].ItemArray[1].ToString();

                list.LEDEnable = bool.Parse(dt.Rows[39].ItemArray[1].ToString());
                list.LEDPort = dt.Rows[40].ItemArray[1].ToString();

                list.PlugEnable = bool.Parse(dt.Rows[41].ItemArray[1].ToString());
                list.MaxSet = dt.Rows[42].ItemArray[1].ToString();
                list.PlugNumber = dt.Rows[43].ItemArray[1].ToString();

                list._4010Port = dt.Rows[44].ItemArray[1].ToString();
                list._4010Enable = bool.Parse(dt.Rows[45].ItemArray[1].ToString());

                list.MesEnable = bool.Parse(dt.Rows[46].ItemArray[1].ToString());
                list.MesStation = dt.Rows[47].ItemArray[1].ToString();

                list.AutoHALL = bool.Parse(dt.Rows[48].ItemArray[1].ToString());

                list.RelayPort = dt.Rows[49].ItemArray[1].ToString();
                list.RelayEnable = bool.Parse(dt.Rows[50].ItemArray[1].ToString());

                list.NowStation = dt.Rows[51].ItemArray[1].ToString();

                list.AutoSN = bool.Parse(dt.Rows[52].ItemArray[1].ToString());
                list.EVM = bool.Parse(dt.Rows[53].ItemArray[1].ToString());
            }
            catch (Exception)
            {
                throw;
            }
            ClosedConnect();
            return list;
        }

        public void UpdateConfigData(Dictionary<string, object> dic)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                for (int i = 0; i < dic.Count; i++)
                {
                    string comm = string.Format("update Application_Config set data='{0}' where parameter='{1}'"
                       , dic.Values.ToList()[i], dic.Keys.ToList()[i]);
                    cmd.CommandText = comm;
                    cmd.CommandTimeout = 20;
                    cmd.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            ClosedConnect();
        }

        public TestRatio GetTestRadio()
        {
            TestRatio radio = new TestRatio();
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "select * from Test_Message";
                cmd.CommandTimeout = 20;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    radio.Total = double.Parse(dt.Rows[0].ItemArray[0].ToString());
                    radio.Pass = double.Parse(dt.Rows[0].ItemArray[1].ToString());
                    radio.PassRadio = Math.Round((radio.Pass / radio.Total) * 100, 2);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            ClosedConnect();
            return radio;
        }

        public List<string> GetinitTestItem()
        {
            List<string> list = new List<string>();
            TestRatio radio = new TestRatio();
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "select * from Items";
                cmd.CommandTimeout = 20;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            ClosedConnect();
            return list;
        }

        public void DelTestItem()
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "delete from Test_Items";
                cmd.CommandTimeout = 20;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            ClosedConnect();
        }

        public void DelFailItem()
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "delete from Fail_Items";
                cmd.CommandTimeout = 20;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ClosedConnect();
        }

        public void DelTestRadio()
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "delete from Test_Message";
                cmd.CommandTimeout = 20;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ClosedConnect();
        }

        public void InsterTestItem(List<TestData> lists)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                foreach (var item in lists)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("insert into Test_Items values('{0}'", item.TestItem);
                    sb.AppendFormat(",'{0}','{1}'", item.TestItemName, item.Unit);
                    sb.AppendFormat(",'{0}','{1}'", item.UppLimit, item.LowLimit);
                    sb.AppendFormat(",{0},{1}", item.beferTime, item.AfterTime);
                    sb.AppendFormat(",'{0}','{1}','{2}','{3}','{4}')"
                        , item.Remark, item.Other, item.Check
                        , item.Show, item.FillValue);

                    cmd.CommandText = sb.ToString();
                    cmd.CommandTimeout = 20;
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            ClosedConnect();
        }

        public void InsterFailItem(List<TestData> lists)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                foreach (var item in lists)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("insert into Fail_Items values('{0}'", item.TestItem);
                    sb.AppendFormat(",'{0}','{1}'", item.TestItemName, item.Unit);
                    sb.AppendFormat(",'{0}','{1}'", item.UppLimit, item.LowLimit);
                    sb.AppendFormat(",{0},{1}", item.beferTime, item.AfterTime);
                    sb.AppendFormat(",'{0}','{1}','{2}','{3}','{4}')"
                        , item.Remark, item.Other, item.Check
                        , item.Show, item.FillValue);

                    cmd.CommandText = sb.ToString();
                    cmd.CommandTimeout = 20;
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            ClosedConnect();
        }

        public bool InsertData(string comm)
        {
            bool status = true;
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = ConnectDB();
                cmd.CommandText = comm;
                cmd.CommandTimeout = 20;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public void UpdataTestCount(bool status)
        {
            TestRatio radio = GetTestRadio();
            if (status)
            {
                radio.Pass += 1;
                radio.Total += 1;
                radio.PassRadio = Math.Round((radio.Pass / radio.Total) * 100, 2);
                //DelTestRadio();
            }
            else
            {
                radio.Total += 1;
                radio.PassRadio = Math.Round((radio.Pass / radio.Total) * 100, 2);
                //DelTestRadio();
            }
            UpdataRadio(radio);
            ClosedConnect();
        }

        private void UpdataRadio(TestRatio radio)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                //cmd.CommandText = string.Format("insert into Test_Message values('{0}','{1}','{2}')"
                //    , radio.Total, radio.Pass, radio.PassRadio);
                string command = string.Format("update Test_Message  set Total='{0}',Pass='{1}',Pass_Rate='{2}'"
                    , radio.Total, radio.Pass, radio.PassRadio);
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Getsn()
        {
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "select * from SN";
                cmd.CommandTimeout = 20;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public void UpdateSN(string day, int num)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                //cmd.CommandText = string.Format("insert into Test_Message values('{0}','{1}','{2}')"
                //    , radio.Total, radio.Pass, radio.PassRadio);
                string command = string.Format("update SN set SN.day='{0}',SN.SN='{1}'"
                    , day, num);
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTCLSN(string day, int num)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                //cmd.CommandText = string.Format("insert into Test_Message values('{0}','{1}','{2}')"
                //    , radio.Total, radio.Pass, radio.PassRadio);
                string command = string.Format("update SN set SN.day='{0}',SN.SN='{1}'"
                    , day, num);
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClsRadio()
        {
            TestRatio radio = new TestRatio();
            radio.Pass = 0;
            radio.Total = 0;
            radio.PassRadio = 0;
            UpdataRadio(radio);
        }

        public void ClsPlug()
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                string comm = string.Format("update Application_Config set data='{0}' " +
                    "where parameter='{1}'", 0, "PlugNumber");
                cmd.CommandText = comm;
                cmd.CommandTimeout = 20;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ClosedConnect();
        }

        public int GetPlugNumber()
        {
            int number = 0;
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "select data from Application_Config where " +
                    "parameter='PlugNumber'";
                cmd.CommandTimeout = 20;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                number = int.Parse(dt.Rows[0].ItemArray[0].ToString());

            }
            catch (Exception ex)
            {

                throw ex;
            }
            ClosedConnect();
            return number;
        }

        public void UpdataPlugNumber(int number)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                //cmd.CommandText = string.Format("insert into Test_Message values('{0}','{1}','{2}')"
                //    , radio.Total, radio.Pass, radio.PassRadio);
                string command = string.Format("update Application_Config set data='{0}' " +
                    "where parameter='PlugNumber'", number);
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveConfig(string path, ConfigData config)
        {
            dbPath = path;
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                //cmd.CommandText = "CREATE TABLE Application_Config(parameter varchar(255),data varchar(255))";
                //cmd.ExecuteNonQuery();

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Title", config.Title);
                dic.Add("VisaPort", config.VisaPort);
                dic.Add("SerialPort", config.SerialPort);
                dic.Add("PowerPort", config.PowerPort);
                dic.Add("Sen_TX_Power", config.Sen_TX_Power);
                dic.Add("number_of_packets", config.number_of_packets);
                dic.Add("Low_Freq", config.Low_Freq);
                dic.Add("Mod_Freq", config.Mod_Freq);
                dic.Add("Hi_Freq", config.Hi_Freq);
                dic.Add("Low_Loss", config.Low_Loss);
                dic.Add("Mod_Loss", config.Mod_Loss);
                dic.Add("Hi_Loss", config.Hi_Loss);
                dic.Add("Inquiry_TimeOut", config.Inquiry_TimeOut);

                dic.Add("OP", config.OP);
                dic.Add("IC", config.IC);
                dic.Add("CD", config.CD);
                dic.Add("SS", config.SS);
                dic.Add("MI", config.MI);
                dic.Add("Voltage1", config.Voltage1);
                dic.Add("Voltage2", config.Voltage2);
                dic.Add("Current", config.Current);
                dic.Add("GPIB_Enable", config.GPIB_Enable);
                dic.Add("Serial_Enable", config.Serial_Enable);
                dic.Add("Power_Enable", config.Power_Enable);
                dic.Add("CompareSN", config.CompareString);
                dic.Add("SNLength", config.SNLength);
                dic.Add("MultimeterPort", config.MultimeterPort);
                dic.Add("Multimeter_Enable", config.Multimeter_Select);
                dic.Add("AudioEnable", config.AudioEnable);
                dic.Add("AudioPath", config.AudioPath);
                dic.Add("SerialSelect", config.SerialSelect);
                dic.Add("SNAuto", config.AutoSN);
                dic.Add("SNHear", config.SNHear);
                dic.Add("SNLine", config.SNLine);
                dic.Add("FixAuto", config.AutoFixture);
                dic.Add("FixPort", config.AutoFixture);
                dic.Add("LEDEnable", config.LEDEnable);
                dic.Add("LEDPort", config.LEDPort);
                dic.Add("PlugEnable", config.PlugEnable);
                dic.Add("MaxSet", config.MaxSet);
                dic.Add("PlugNumber", config.PlugNumber);
                dic.Add("_4010Port", config._4010Port);
                dic.Add("_4010Enable", config._4010Enable);
                dic.Add("AutoHALL", config.AutoHALL);
                dic.Add("RelayPort", config.RelayPort);
                dic.Add("RelayEnable", config.RelayEnable);
                //dic.Add("NowStation", tb_NowStation.Text.Trim());
                dic.Add("AutoSN", config.AutoSN);
                dic.Add("EVM", config.EVM);
                UpdateConfigData(dic);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //CREATE TABLE Application_Config(parameter varchar(255),data varchar(255));
            //insert into Application_Config (parameter,data) values ("Title","wangatao")
        }

        public string GetLicenseData(string data)
        {
            string lic = string.Empty;
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = ConnectDB();
                cmd.CommandText = "SELECT data from application_config where parameter = 'License'";
                cmd.CommandTimeout = 20;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                int num = dt.Rows.Count;
                if (num > 0)
                {
                    lic = dt.Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    cmd.CommandText = string.Format("insert into application_config values('License','{0}')", data);
                    cmd.CommandTimeout = 20;
                    cmd.ExecuteNonQuery();
                }  
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lic;
        }
    }
}
