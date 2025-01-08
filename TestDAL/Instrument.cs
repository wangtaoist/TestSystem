using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NationalInstruments.VisaNS;
using System.Threading;
using TestTool;

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
                Others.WriteErrorLog(ex.Message);
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
                Thread.Sleep(60);
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
                if (Session != null)
                {
                    Thread.Sleep(50);
                    Session.Write("*RST\n");
                    Others.WriteInformationLog("*RST");
                }
            }
            catch
            {

            }
        }

        public int Opc()
        {
            int ret = 0;
            try
            {
                if (Session != null)
                {
                    Thread.Sleep(500);
                    ret = int.Parse(Session.Query("*OPC?\n"));
                    Others.WriteInformationLog("*OPC?");
                }
            }
            catch { }
            return ret;
        }

        public void Cls()
        {
            try
            {
                if (Session != null)
                {
                    Thread.Sleep(50);
                    Session.Write("*CLS\n");
                    Others.WriteInformationLog("*CLS");
                }
            }
            catch { }
        }

        public void VisaWrite(string cmd)
        {
            try
            {
                if (Session != null )
                {
                    Thread.Sleep(50);
                    //Cls();
                    Session.Write(string.Format("*OPC;{0}\n", cmd));
                    Others.WriteInformationLog(string.Format("*OPC;{0}\n", cmd));
                }
            }
         
            catch (Exception ex)
            {
                Others.WriteErrorLog(ex.Message);
                throw;
            }
        }

        public string VisaQuery(string cmd)
        {
            string data = string.Empty;
            try
            {
                if (Session != null)
                {
                    Thread.Sleep(50);
                    string send = string.Format("*OPC;{0}\n", cmd);
                    Others.WriteInformationLog(send);
                    data = Session.Query(send).Trim();
                    Others.WriteInformationLog(data);
                }
            }
            catch (Exception ex)
            {
                Others.WriteErrorLog(ex.Message);
                throw;
            }
            return data;
        }

        public void Closed()
        {
            if (Session != null)
            {
                Session.Dispose();
                Session = null;
            }
        }
    }
}
