using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RJCP.IO.Ports;
using NationalInstruments.VisaNS;
using System.IO.Ports;

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

        public SerialOperate(string ResourceName,bool status,Queue<string> queue)
        {
            this.ResourceName = ResourceName;
            this.serialStatus = status;
            this.queue = queue;
        }

        public void OpenSerialPort()
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
                    Session1.BaudRate = 921600;
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
                    Session.BaudRate = 921600;
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
                    Session1.Write(data, 0, data.Length);
                    Thread.Sleep(200);
                    num = Session1.ReadByte();
                    values = new byte[num];
                    Session1.Read(values, 0, values.Length);
                }
                else
                {
                    Session.DiscardInBuffer();
                    Session.DiscardOutBuffer();
                    Session.Write(data, 0, data.Length);
                    Thread.Sleep(200);
                    num = Session.ReadByte();
                    values = new byte[num];
                    Session.Read(values, 0, values.Length);
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
                queue.Enqueue(ex.Message);
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
