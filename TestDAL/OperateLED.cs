using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestModel;
using System.IO.Ports;
using System.Threading;

namespace TestDAL
{
   public class OperateLED
    {
        private ConfigData config;
        private static SerialPort serialPort;

        public OperateLED(ConfigData data)
        {
            config = data;
            serialPort = new SerialPort();
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.PortName = config.LEDPort;
            serialPort.BaudRate = 115200;
            serialPort.ReadTimeout = 5000;
            serialPort.WriteTimeout = 5000;
        }

        public TestData OpenLedPort(TestData data)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                serialPort.Open();
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

        public TestData LED1Test(TestData data)
        {
            try
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
                serialPort.WriteLine(":001r_rgbi01-01\n");
                Thread.Sleep(200);
                string value = serialPort.ReadExisting().Split('=')[1];
                string[] values = value.Split(',');
                double R = double.Parse(values[0]);
                double G = double.Parse(values[1]);
                double B = double.Parse(values[2]);
                double brightness = double.Parse(values[3]);

                string[] low = data.LowLimit.Split(',');
                string[] high = data.UppLimit.Split(',');

                double LR = double.Parse(low[0]);
                double LG = double.Parse(low[1]);
                double LB = double.Parse(low[2]);
                double Lbrightness = double.Parse(low[3]);

                double HR = double.Parse(high[0]);
                double HG = double.Parse(high[1]);
                double HB = double.Parse(high[2]);
                double Hbrightness = double.Parse(high[3]);

               if(((R <= HR && R >= LR) && (G <= HG && G >= LG)) 
                    && ((B <= HB && B >= LB) && (brightness >= Lbrightness)))
                {
                    data.Result = "Pass";
                    data.Value = value;
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = value;
                }
            }
            catch (Exception)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData ClosedLEDPort(TestData data)
        {
            try
            {
                serialPort.Close();
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

        public static byte[] ReadSerialData(byte[] buff)
        {
            byte[] retBuff = new byte[512];
            try
            {
                //04 FF 03 01 00 00

                //01 00 
                //01 03 00 00 01 01 00 
                //02 03 05 6B 00 01 00 
                //03 03 04 86 00 01 00 
                //04 03 00 00 00 01 00 
                //05 03 00 00 00 01 00 
                //07 03 08 1C 00 01 00 
                //08 03 07 D8 00 01 00 
                //09 03 08 32 00 01 00 
                //0A 03 00 00 00
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
                //buff = { 0x04, 0xff, 0x03, 0x01, 0x00, 0x00 };
                serialPort.Write(buff, 0, buff.Length);
                Thread.Sleep(200);
                serialPort.Read(retBuff, 0, retBuff.Length);
                
            }
            catch (Exception)
            {
                retBuff = null;
            }
            return retBuff;
        }


    }
}
