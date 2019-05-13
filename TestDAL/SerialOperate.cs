using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RJCP.IO.Ports;

namespace TestDAL
{
    public class SerialOperate
    {
        private SerialPortStream Session;
        private string ResourceName;

        public SerialOperate(string ResourceName)
        {
            this.ResourceName = ResourceName;
        }

        public void OpenSerialPort()
        {
            try
            {
                //Session = (SerialSession)ResourceManager.GetLocalManager().Open(ResourceName);
                //Session.BaudRate = 921600;
                //Session.Timeout = 500000;
                //Session.SetBufferSize(BufferTypes.OutBuffer, 1024);
                //Session.SetBufferSize(BufferTypes.InBuffer, 1024);

                Session = new SerialPortStream();
                Session.BaudRate = 921600;
                Session.PortName = ResourceName;
                Session.DataBits = 8;
                Session.StopBits = StopBits.One;
                Session.Parity = Parity.None;
                Session.ReadTimeout = 5000;
                Session.WriteTimeout = 5000;
           
                if(Session.IsOpen)
                {
                    Session.Close();
                }
                Session.Open();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void VisaWrite(byte[] cmd)
        {
            try
            {
                Thread.Sleep(100);
                //Session.Write(cmd);
            }
            catch { }
        }

        public byte[] VisaQuery(byte[] data)
        {
            byte[] values = null;
            try
            {
                Session.Write(data, 0, data.Length);
                Thread.Sleep(200);
                int num = Session.ReadByte();
                values = new byte[num];
                Session.Read(values, 0, values.Length);
                if(values[0] == 0xff)
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
                throw ex;
            }
            return values;
        }

        public void ClosedPort()
        {
            Session.Close();
            //Session.Dispose();
        }
    }
}
