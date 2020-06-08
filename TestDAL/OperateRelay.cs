using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using TestModel;
using System.Threading;

namespace TestDAL
{
    public class OperateRelay
    {
        private SerialPort relayPort;
        private ConfigData config;

        public OperateRelay(ConfigData config)
        {
            this.config = config;
        }

        public TestData OpenRelay(TestData data)
        {
            try
            {
                relayPort = new SerialPort(config.RelayPort, 9600);
                if (relayPort.IsOpen)
                {
                    relayPort.Close();
                }
                relayPort.Open();
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

        public TestData OpenChannel(TestData data)
        {
            try
            {
                //33 01 12 00 00 00 01 47 
                byte[] buff = { 0x33, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00 };
                buff[6] = byte.Parse(data.LowLimit);
                int add = 0;
                for (int i = 0; i < buff.Length; i++)
                {
                    add += buff[i];
                }
                buff[7] = byte.Parse(add.ToString());
                relayPort.Write(buff, 0, buff.Length);
                Thread.Sleep(200);
                int length = relayPort.ReadByte();
                byte[] retBuff = new byte[length];
                relayPort.Read(retBuff, 0, length);
                if (retBuff[0] == 0x01)
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
            catch (Exception)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData ClosedChannel(TestData data)
        {
            try
            {
                //33 01 12 00 00 00 01 47 
                byte[] buff = { 0x33, 0x01, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00 };
                buff[6] = byte.Parse(data.LowLimit);
                int add = 0;
                for (int i = 0; i < buff.Length; i++)
                {
                    add += buff[i];
                }
                buff[7] = byte.Parse(add.ToString());
                relayPort.Write(buff, 0, buff.Length);
                Thread.Sleep(200);
                int length = relayPort.ReadByte();
                byte[] retBuff = new byte[length];
                relayPort.Read(retBuff, 0, length);
                if (retBuff[0] == 0x01)
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
            catch (Exception)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData OpenAllChannel(TestData data)
        {
            try
            {
                //33 01 14 00 00 00 00 48
                byte[] buff = { 0x33, 0x01, 0x13, 0x00, 0x00, 0x00, 0x00, 0x47 };

                relayPort.Write(buff, 0, buff.Length);
                Thread.Sleep(200);
                int length = relayPort.ReadByte();
                byte[] retBuff = new byte[length];
                relayPort.Read(retBuff, 0, length);
                if (retBuff[0] == 0x01)
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
            }
            return data;
        }

        public TestData ClosedAllChannel(TestData data)
        {
            try
            {
                //33 01 14 00 00 00 00 48
                byte[] buff = { 0x33, 0x01, 0x14, 0x00, 0x00, 0x00, 0x00, 0x48 };

                relayPort.Write(buff, 0, buff.Length);
                Thread.Sleep(200);
                int length = relayPort.ReadByte();
                byte[] retBuff = new byte[length];
                relayPort.Read(retBuff, 0, length);
                if (retBuff[0] == 0x01)
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
            catch (Exception)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData ClosedRelay(TestData data)
        {
            try
            {

                if (relayPort.IsOpen)
                {
                    relayPort.Close();
                }

                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }
    }
}
