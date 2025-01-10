using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RJCP.IO.Ports;
using System.IO.Ports;
using TestModel;
using System.Linq;

namespace TestDAL
{
    public class SerialOperate
    {
      
        private SerialPortStream Session;
        //private SerialSession Session;
        private SerialPort Session1;
        private string ResourceName;
        private bool serialStatus;
        private Queue<string> queue;
        private ConfigData config;

        public SerialOperate(ConfigData config,Queue<string> queue)
        {
            this.config = config;
            this.serialStatus = config.SerialSelect;
            this.ResourceName = config.SerialPort;
            this.queue = queue;
        }

        public void OpenSerialPort(int BaudRate)
        {
            try
            {
                //string[] list = SerialPort.GetPortNames();
                //for (int i = 0; i < list.Length; i++)
                //{
                //    if ((list[i] != config.RelayPort && list[i] != "com1") && list[i] != "com2")
                //    {
                //        ResourceName = list[i];
                //    }
                //}

                if (serialStatus)
                {
                    Session1 = new SerialPort();
                    Session1.BaudRate = BaudRate;
                    Session1.PortName = ResourceName;
                    Session1.DataBits = 8;
                    Session1.StopBits = System.IO.Ports.StopBits.One;
                    Session1.Parity = System.IO.Ports.Parity.None;
                    Session1.ReadTimeout = 5000;
                    Session1.WriteTimeout = 5000;
                    if (Session1.IsOpen)
                    {
                        Session1.Close();
                    }
                    Session1.Open();
                }
                else
                {
                    Session = new SerialPortStream();
                    Session.BaudRate = BaudRate;
                    Session.PortName = ResourceName;
                    Session.DataBits = 8;
                    Session.StopBits = RJCP.IO.Ports.StopBits.One;
                    Session.Parity = RJCP.IO.Ports.Parity.None;
                    Session.ReadTimeout = 5000;
                    Session.WriteTimeout = 5000;
                    if (Session.IsOpen)
                    {
                        Session.Close();
                    }
                    Session.Open();
                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void OpenOneLineSerialPort()
        {
            try
            {
                //Session = (SerialSession)ResourceManager.GetLocalManager().Open(ResourceName);
                //Session.BaudRate = 921600;
                ////Session.BaudRate = 921600;
                //Session.Timeout = 500000;
                //Session.SetBufferSize(BufferTypes.OutBuffer, 1024);
                //Session.SetBufferSize(BufferTypes.InBuffer, 1024);

                if (serialStatus)
                {
                    Session1 = new SerialPort();
                    Session1.BaudRate = 9600;
                    Session1.PortName = ResourceName;
                    Session1.DataBits = 8;
                    Session1.StopBits = System.IO.Ports.StopBits.One;
                    Session1.Parity = System.IO.Ports.Parity.None;
                    Session1.ReadTimeout = 2000;
                    Session1.WriteTimeout = 2000;
                    if (Session1.IsOpen)
                    {
                        Session1.Close();
                    }
                    Session1.Open();
                }
                else
                {
                    Session = new SerialPortStream();
                    Session.BaudRate = 9600;
                    Session.PortName = ResourceName;
                    Session.DataBits = 8;
                    Session.StopBits = RJCP.IO.Ports.StopBits.One;
                    Session.Parity = RJCP.IO.Ports.Parity.None;
                    Session.ReadTimeout = 2000;
                    Session.WriteTimeout = 2000;
                    if (Session.IsOpen)
                    {
                        Session.Close();
                    }
                    Session.Open();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void OpenOppoSerialPort()
        {
            try
            {
                //Session = (SerialSession)ResourceManager.GetLocalManager().Open(ResourceName);
                //Session.BaudRate = 921600;
                ////Session.BaudRate = 921600;
                //Session.Timeout = 500000;
                //Session.SetBufferSize(BufferTypes.OutBuffer, 1024);
                //Session.SetBufferSize(BufferTypes.InBuffer, 1024);

                if (serialStatus)
                {
                    Session1 = new SerialPort();
                    Session1.BaudRate = 115200;
                    Session1.PortName = ResourceName;
                    Session1.DataBits = 8;
                    Session1.StopBits = System.IO.Ports.StopBits.One;
                    Session1.Parity = System.IO.Ports.Parity.None;
                    Session1.ReadTimeout = 5000;
                    Session1.WriteTimeout = 5000;
                    if (Session1.IsOpen)
                    {
                        Session1.Close();
                    }
                    Session1.Open();
                }
                else
                {
                    Session = new SerialPortStream();
                    Session.BaudRate = 115200;
                    Session.PortName = ResourceName;
                    Session.DataBits = 8;
                    Session.StopBits = RJCP.IO.Ports.StopBits.One;
                    Session.Parity = RJCP.IO.Ports.Parity.None;
                    Session.ReadTimeout = 5000;
                    Session.WriteTimeout = 5000;
                    if (Session.IsOpen)
                    {
                        Session.Close();
                    }
                    Session.Open();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void VisaWrite(byte[] cmd)
        {
            try
            {
                Thread.Sleep(100);
                Session.Write(cmd, 0, cmd.Length);
            }
            catch { }
        }

        public byte[] VisaQuery(byte[] data)
        {
            byte[] values = null;
            int num = 0;
            try
            {
                if (serialStatus)
                {
                    Session1.DiscardInBuffer();
                    Session1.DiscardOutBuffer();
                    //Thread.Sleep(100);
                    Session1.Write(data, 0, data.Length);
                    TestTool.Others.WriteInformationLog("Send--->" + ToHexStrFromByte(data));
                    Thread.Sleep(200);
                    //num = Session1.ReadByte();
                    values = new byte[124];
                    Session1.Read(values, 0, values.Length);
                }
                else
                {
                    Session.DiscardInBuffer();
                    Session.DiscardOutBuffer();

                    //Session.Write(data, 0, data.Length);
                    //Thread.Sleep(200);
                    Session.Write(data, 0, data.Length);
                    TestTool.Others.WriteInformationLog("Send--->" + ToHexStrFromByte(data));
                    Thread.Sleep(200);

                    //num = Session.ReadByte();
                    //if(num == 0)
                    //{
                    //    Session.Write(data, 0, data.Length);
                    //    TestTool.Others.WriteInformationLog("Send Second--->" + ToHexStrFromByte(data));
                    //    Thread.Sleep(500);
                    //}

                    values = new byte[num == 0 ? 124 : num];
                    Session.Read(values, 0, values.Length);
                    TestTool.Others.WriteInformationLog("Reply Original--->" + ToHexStrFromByte(values));
                }
                //Session.Write(data);
                //values = Session.ReadByteArray();
                //values = Session.Query(data);
                //Session.Write(data);
                //values = Session.ReadByteArray();
                if (values[0] == 0xff)
                {
                    byte[] errvalue = values;
                    values = new byte[num];
                    errvalue.CopyTo(values, 1);
                }
                //byte[] copy = new byte[255];
                //Array.Copy(values, 3, copy, 0, values.Length - 3);
            }
            catch(Exception ex)
            {
                queue.Enqueue("ex" + ex.Message);
            }
            finally
            {
                Session.DiscardInBuffer();
                Session.DiscardOutBuffer();
            }
            values = Decode(values);
            TestTool.Others.WriteInformationLog("Reply--->" + ToHexStrFromByte(values));
            return values;
        }

        public string VisaQuery(string data,int delay)
        {
            //byte[] values = null;
            string values = string.Empty;
            int num = 0;
            try
            {
                if (serialStatus)
                {
                    Session1.DiscardInBuffer();
                    Session1.DiscardOutBuffer();
                    //Thread.Sleep(100);
                    Session1.WriteLine(data);
                    Thread.Sleep(200);
                    //num = Session1.ReadByte();
                    //values = new byte[num];
                    //Session1.Read(values, 0, values.Length);
                    values = Session1.ReadExisting();
                }
                else
                {
                    Session.DiscardInBuffer();
                    Session.DiscardOutBuffer();

                    Session.WriteLine(data);
                    Thread.Sleep(delay);
                    //num = Session.ReadByte();
                    //values = new byte[num];
                    //Session.Read(values, 0, values.Length);

                    values = Session.ReadExisting();
                }
                //Session.Write(data);
                //values = Session.ReadByteArray();
                //values = Session.Query(data);
                //Session.Write(data);
                //values = Session.ReadByteArray();
                //if (values[0] == 0xff)
                //{
                //    byte[] errvalue = values;
                //    values = new byte[num];
                //    errvalue.CopyTo(values, 1);
                //}
                //byte[] copy = new byte[255];
                //Array.Copy(values, 3, copy, 0, values.Length - 3);
            }
            catch (Exception ex)
            {
                queue.Enqueue("ex;" + ex.Message);
            }
            return values;
        }

        public string VisaQuery1(string data, int delay)
        {
            //byte[] values = null;
            string values = string.Empty;
            int num = 0;
            try
            {
                if (serialStatus)
                {
                    Session1.DiscardInBuffer();
                    Session1.DiscardOutBuffer();
                    //Thread.Sleep(100);
                    Session1.WriteLine(data);
                    Thread.Sleep(200);
                    //num = Session1.ReadByte();
                    //values = new byte[num];
                    //Session1.Read(values, 0, values.Length);
                    values = Session1.ReadExisting();
                }
                else
                {
                    Session.DiscardInBuffer();
                    Session.DiscardOutBuffer();

                    Session.WriteLine(data);
                    Thread.Sleep(delay);
                    //num = Session.ReadByte();
                    //values = new byte[num];
                    //Session.Read(values, 0, values.Length);

                    values = Session.ReadExisting();
                }
                //Session.Write(data);
                //values = Session.ReadByteArray();
                //values = Session.Query(data);
                //Session.Write(data);
                //values = Session.ReadByteArray();
                //if (values[0] == 0xff)
                //{
                //    byte[] errvalue = values;
                //    values = new byte[num];
                //    errvalue.CopyTo(values, 1);
                //}
                //byte[] copy = new byte[255];
                //Array.Copy(values, 3, copy, 0, values.Length - 3);
            }
            catch (Exception ex)
            {
                queue.Enqueue("ex;" + ex.Message);
            }
            return values;
        }

        public void setVolume(byte[] data)
        {
            //Thread.Sleep(500);
            if (serialStatus)
            {
                //Session1.Write(data, 0, data.Length);
                Thread.Sleep(500);
                Session1.Write(data, 0, data.Length);
            }
            else
            {
                //Session.Write(data, 0, data.Length);
                Thread.Sleep(500);
                Session.Write(data, 0, data.Length);
            }
            //Thread.Sleep(1500);
            //byte[] ret = new byte[255];
            //Session.Read(ret, 0, ret.Length);
            //return ret;
        }

        public static string ToHexStrFromByte( byte[] byteDatas)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteDatas.Length; i++)
            {
                builder.Append(string.Format("{0:X2} ", byteDatas[i]));
            }
            return builder.ToString().Trim();
        }

        public static byte[] Decode(byte[] packet)
        {
            long num = BitConverter.ToInt64(packet, 0);
            if(num == 0)
            {
                return packet;
            }
            var i = packet.Length - 1;
            while (packet[i] == 0)
            {
                --i;
            }
            var temp = new byte[i + 1];
            Array.Copy(packet, temp, i + 1);
            return temp;
        }

        public void ClosedPort()
        {
            if (serialStatus)
            {
                Session1.Close();
                Session1.Dispose();
            }
            else
            {
                Session.Close();
                Session.Dispose();
            }
        }
    }
}
