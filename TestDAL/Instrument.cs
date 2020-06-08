using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NationalInstruments.VisaNS;
using System.Threading;

namespace TestDAL
{
  public  class Instrument
    {
        private MessageBasedSession Session;

        public MessageBasedSession _Session
        {
            get { return Session; }
        }

        public Instrument(string ResourceName)
        {
            try
            {
                Session = (MessageBasedSession)ResourceManager.GetLocalManager()
                    .Open(ResourceName, AccessModes.NoLock, 1000);
                Session.Timeout = 50000;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void OpenVisa(string resourcs)
        {
            try
            {
                Session = (MessageBasedSession)ResourceManager.GetLocalManager()
                    .Open(resourcs, AccessModes.NoLock, 1000);
                Session.Timeout = 50000;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> GetLocalName()
        {
            List<string> list = new List<string>();
            try
            {
                list = ResourceManager.GetLocalManager().FindResources("?*").ToList();
            }
            catch { }
            return list;
        }

        public string IDN()
        {
            string ret = string.Empty;
            try
            {
                Thread.Sleep(100);
                ret = Session.Query("*IDN?\n");
            }
            catch(Exception ex) 
            {
                ret = "GPIB 端口错误," + ex.Message;
            }
            return ret;
        }

        public void Rst()
        {
            try
            {
                Thread.Sleep(100);
                Session.Write("*RST\n");
            }
            catch { }
        }

        public int Opc()
        {
            int ret = 0;
            try
            {
                Thread.Sleep(500);
                ret = int.Parse(Session.Query("*OPC?\n"));


            }
            catch { }
            return ret;
        }

        public void Cls()
        {
            try
            {
                Thread.Sleep(100);
                Session.Write("*CLS\n");
            }
            catch { }
        }

        public void VisaWrite(string cmd)
        {
            try
            {
                Thread.Sleep(50);
                //Cls();
                Session.Write(string.Format("*OPC;{0}\n", cmd));
            }
            catch { }
        }

        public string VisaQuery(string cmd)
        {
            Thread.Sleep(50);
            return Session.Query(string.Format("*OPC;{0}\n", cmd)).Trim();
        }

        public void Closed()
        {
            if (Session != null)
            {
                Session.Dispose();
            }
        }
    }
}
