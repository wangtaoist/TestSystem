using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestModel;

namespace TestDAL
{
    public class OperateInstrument
    {
        private Instrument instrument;
        private Instrument PowerInst;
        private Instrument MultimeterInst;
        private ConfigData data;
        private Queue<string> queue;
        private CsrOperate csr;

        public OperateInstrument(ConfigData data, Queue<string> queue)
        {
            this.data = data;
            this.queue = queue;
            try
            {
                if (data.GPIB_Enable)
                {
                    instrument = new Instrument(data.VisaPort);
                }
                if (data.Power_Enable)
                {
                    PowerInst = new Instrument(data.PowerPort);
                }
                if (data.Multimeter_Select)
                {
                    MultimeterInst = new Instrument(data.MultimeterPort);
                }
                //csr = new CsrOperate();
            }
            catch (Exception ex)
            {
                queue.Enqueue(ex.Message);
            }
        }

        public void InitInstr()
        {
            instrument.Rst();
            instrument.VisaWrite("OPMD SCRIPT");
            instrument.VisaWrite("SCPTSEL 3");
            instrument.VisaWrite("SCRIPTMODE 3,STANDARD");

            //SCPTCFG 3,OP,ON;MI;IC;CD;SS
            //PC;MS;MP
            //ALLTSTS
            instrument.VisaWrite("SCPTCFG 3,ALLTSTS,OFF");
            if (data.OP)
                instrument.VisaWrite("SCPTCFG 3,OP,ON");
            if (data.MI)
                instrument.VisaWrite("SCPTCFG 3,MI,ON");
            if (data.IC)
                instrument.VisaWrite("SCPTCFG 3,IC,ON");
            if (data.CD)
                instrument.VisaWrite("SCPTCFG 3,CD,ON");
            if (data.SS)
                instrument.VisaWrite("SCPTCFG 3,SS,ON");
            //instrument.VisaWrite("SCPTCFG 3,PC,OFF");
            //instrument.VisaWrite("SCPTCFG 3,MS,OFF");
            //instrument.VisaWrite("SCPTCFG 3,MP,OFF");
            //TXPWR 3,-10.0
            if (data.OP)
            {
                instrument.VisaWrite("TXPWR 3,-40");
                // instrument.VisaWrite(string.Format("OPCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("OPCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("OPCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                instrument.VisaWrite("OPCFG 3,PKTTYPE, DH1");
                instrument.VisaWrite("OPCFG 3,NUMPKTS,10");
                instrument.VisaWrite("OPCFG 3,HOPPING,HOPOFF");
            }

            if (data.IC)
            {
                //instrument.VisaWrite(string.Format("ICCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("ICCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("ICCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                //instrument.VisaWrite("ICCFG 3,HOPMODE, ANY");
                instrument.VisaWrite("ICCFG 3,NUMPKTS,10");
                instrument.VisaWrite("ICCFG 3,HOPPING,HOPOFF");
            }
            if (data.CD)
            {
                //instrument.VisaWrite(string.Format("CDCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("CDCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("CDCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                instrument.VisaWrite("CDCFG 3,PKTSIZE,ONESLOT,TRUE");
                instrument.VisaWrite("CDCFG 3,PKTSIZE,THREESLOT,FALSE");
                instrument.VisaWrite("CDCFG 3,PKTSIZE,FIVESLOT,FALSE");
                instrument.VisaWrite("CDCFG 3,NUMPKTS,10");
                instrument.VisaWrite("CDCFG 3,HOPPING,HOPOFF");
            }
            if (data.MI)
            {
                //instrument.VisaWrite(string.Format("MICFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("MICFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("MICFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                instrument.VisaWrite("MICFG 3,NUMPKTS,10");
            }
            if (data.SS)
            {
                //instrument.VisaWrite(string.Format("SSCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("SSCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("SSCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                instrument.VisaWrite(string.Format("SSCFG 3,NUMPKTS, {0}", data.number_of_packets));
                instrument.VisaWrite(string.Format("SSCFG 3,TXPWR, {0}", data.Sen_TX_Power));
                //SSCFG 3,HOPPING, HOPBOTH
                //SSCFG 3,HOPMODE, DEFINED
                instrument.VisaWrite("SSCFG 3,HOPPING, HOPBOTH");
                //instrument.VisaWrite("CDCFG 3,PKTSIZE,FIVESLOT,FALSE");
            }
            //instrument.VisaWrite(string.Format("OPCFG 3,AVGMXLIM, {0}", data.AvgPowerHi));
            //instrument.VisaWrite(string.Format("OPCFG 3,AVGMNLIM, {0}", data.AvgPowerLow));
            //instrument.VisaWrite(string.Format("OPCFG 3,PEAKLIM, {0}", data.PeakPower));

            instrument.VisaWrite(string.Format("PATHOFF 3,TABLE"));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,0, {0}", data.Low_Loss));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,39, {0}", data.Mod_Loss));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,78, {0}", data.Hi_Loss));

            // instrument.Closed();
        }

        public void Set8852()
        {
            instrument.VisaWrite("OPMD SCRIPT");
            instrument.VisaWrite("SCPTSEL 3");
            instrument.VisaWrite("SCRIPTMODE 3,STANDARD");

            //SCPTCFG 3,OP,ON;MI;IC;CD;SS
            //PC;MS;MP
            //ALLTSTS
            instrument.VisaWrite("SCPTCFG 3,ALLTSTS,OFF");
            if (data.OP)
                instrument.VisaWrite("SCPTCFG 3,OP,ON");
            if (data.MI)
                instrument.VisaWrite("SCPTCFG 3,MI,ON");
            if (data.IC)
                instrument.VisaWrite("SCPTCFG 3,IC,ON");
            if (data.CD)
                instrument.VisaWrite("SCPTCFG 3,CD,ON");
            if (data.SS)
                instrument.VisaWrite("SCPTCFG 3,SS,ON");
            //instrument.VisaWrite("SCPTCFG 3,PC,OFF");
            //instrument.VisaWrite("SCPTCFG 3,MS,OFF");
            //instrument.VisaWrite("SCPTCFG 3,MP,OFF");
            //TXPWR 3,-10.0
            if (data.OP)
            {
                //HOPPING
                instrument.VisaWrite("TXPWR 3,-40");
                // instrument.VisaWrite(string.Format("OPCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("OPCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("OPCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                instrument.VisaWrite("OPCFG 3,PKTTYPE, DH1");
                instrument.VisaWrite("OPCFG 3,NUMPKTS,10");
                instrument.VisaWrite("OPCFG 3,HOPPING,HOPON");
            }

            if (data.IC)
            {
                //instrument.VisaWrite(string.Format("ICCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("ICCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("ICCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                //instrument.VisaWrite("ICCFG 3,HOPMODE, ANY");
                instrument.VisaWrite("ICCFG 3,NUMPKTS,10");
                //instrument.VisaWrite("ICCFG 3,HOPPING,HOPOFF");
            }
            if (data.CD)
            {
                //instrument.VisaWrite(string.Format("CDCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("CDCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("CDCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                instrument.VisaWrite("CDCFG 3,PKTSIZE,ONESLOT,TRUE");
                instrument.VisaWrite("CDCFG 3,PKTSIZE,THREESLOT,FALSE");
                instrument.VisaWrite("CDCFG 3,PKTSIZE,FIVESLOT,FALSE");
                instrument.VisaWrite("CDCFG 3,NUMPKTS,10");
            }
            if (data.MI)
            {
                //instrument.VisaWrite(string.Format("MICFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("MICFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("MICFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                instrument.VisaWrite("MICFG 3,NUMPKTS,10");
            }
            if (data.SS)
            {
                //instrument.VisaWrite(string.Format("SSCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                //instrument.VisaWrite(string.Format("SSCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                //instrument.VisaWrite(string.Format("SSCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                instrument.VisaWrite(string.Format("SSCFG 3,NUMPKTS, {0}", data.number_of_packets));
                instrument.VisaWrite(string.Format("SSCFG 3,TXPWR, {0}", data.Sen_TX_Power));
                //SSCFG 3,HOPPING, HOPBOTH
                //SSCFG 3,HOPMODE, DEFINED
                instrument.VisaWrite("SSCFG 3,HOPPING, HOPBOTH");
                //instrument.VisaWrite("CDCFG 3,PKTSIZE,FIVESLOT,FALSE");
            }
            //instrument.VisaWrite(string.Format("OPCFG 3,AVGMXLIM, {0}", data.AvgPowerHi));
            //instrument.VisaWrite(string.Format("OPCFG 3,AVGMNLIM, {0}", data.AvgPowerLow));
            //instrument.VisaWrite(string.Format("OPCFG 3,PEAKLIM, {0}", data.PeakPower));

            instrument.VisaWrite(string.Format("PATHOFF 3,TABLE"));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,0, {0}", data.Low_Loss));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,39, {0}", data.Mod_Loss));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,78, {0}", data.Hi_Loss));

        }

        public void InitPower()
        {
            PowerInst.Rst();
            PowerInst.VisaWrite(string.Format("INST:COUP:OUTP:STAT NONE"));
            //PowerInst.VisaWrite(string.Format(":SOUR1:VOLT:LEVel {0}"
            //    , data.Voltage1));
            //PowerInst.VisaWrite(string.Format(":SOUR2:VOLT:LEVel {0}"
            //   , data.Voltage2));
            //PowerInst.VisaWrite(string.Format(":SOUR1:CURRent:LIMit:VALue  {0}"
            //  , data.Current));
            //PowerInst.VisaWrite(string.Format(":SOUR2:CURRent:LIMit:VALue  {0}"
            //  , data.Current));
            //PowerInst.VisaWrite(string.Format(":SENSe1:CURRent:DC:RANGe:AUTO ON"));
            //PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe:AUTO ON"));
        }

        public void initMultimeter()
        {
            MultimeterInst.Rst();
        }

        private void SetCWMode()
        {
            instrument.VisaWrite("OPMD CWMEAS");
            instrument.VisaWrite(string.Format("CWMEAS FREQ,{0}.00MHZ,1.00MS"
                , data.Mod_Freq));
        }

        private double ReadFreqOffect()
        {
            double freq = 0;
            string val = instrument.VisaQuery("CWRESULT FREQOFF");
            if (val.Contains("CWRESULT"))
            {
                freq = double.Parse(val.Split(',')[2]);
            }
            return freq / 1000;
        }

        public TestData Open_MT8852(TestData item)
        {
            try
            {
                instrument.OpenVisa(data.VisaPort);
                //Set8852();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("打开MT8852 Pass");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Closed_MT8852(TestData item)
        {
            try
            {
                instrument.VisaWrite("DISCONNECT");
                instrument.Cls();
                instrument.Closed();
                //InitInstr();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("关闭MT8852 Pass");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Run_Script(TestData item)
        {          
            instrument.VisaWrite("SYSCFG EUTSRCE,INQUIR");
            //instrument.VisaWrite("SYSCFG INQSET,RNUM,12");
            instrument.VisaWrite(string.Format("SYSCFG INQSET,TIMEOUT,{0}", data.Inquiry_TimeOut));
            //INQUIRY   INQRSP?
            instrument.VisaWrite("INQUIRY");
            //instrument.VisaWrite("CONNECT");
            string btAddress = string.Empty;
            bool page = false;
            queue.Enqueue("呼叫开始");
            instrument.Cls();
            for (int i = 0; i < 50; i++)
            {
                string ret = instrument.VisaQuery("INQRSP?");
                //instrument.Cls();

                //1,E09DFA349168,7,NO NAME.,
                if (ret.StartsWith("1"))
                {
                    btAddress = ret.Split(',')[1];
                    queue.Enqueue("呼叫" + btAddress + "成功");
                    page = true;
                    break;
                }
                //else if(ret.StartsWith("0"))
                //{
                //    queue.Enqueue("呼叫中......");
                //    //instrument.VisaWrite("INQUIRY");
                //}
                if (i == 49 && !(ret.StartsWith("1")))
                {
                    item.Result = "Fail";
                    item.Value = "Fail";
                    queue.Enqueue("呼叫超时，请检查耳机");
                }
                Thread.Sleep(200);
            }
            //SYSCFG EUTSRCE, MANUAL
            //SYSCFG EUTADDR, E09DFA349168,
            //instrument.VisaWrite("SYSCFG EUTSRCE, MANUAL");
            //instrument.VisaWrite(string.Format("SYSCFG EUTADDR,{0}", btAddress));
            //0003OP1A0100002
            if (page)
            {
                instrument.VisaWrite("CONNECT");
                queue.Enqueue("连接中");
                instrument.Cls();
                bool connStatus = false;
                for (int j = 0; j < 50; j++)
                {
                    string conn = instrument.VisaQuery("STATUS").Substring(6, 1);
                    if (conn == "1")
                    {
                        connStatus = true;
                        queue.Enqueue("连接成功");
                        break;
                    }
                    //else if(conn == "0")
                    //{
                    //    queue.Enqueue("连接中......");
                    //    //instrument.VisaWrite("CONNECT");
                    //}

                    if (j == 49 && connStatus != true)
                    {
                        item.Result = "Fail";
                        item.Value = "Fail";
                        queue.Enqueue("连接超时，请检查耳机");
                        connStatus = false;
                    }
                    Thread.Sleep(200);
                }
                if (connStatus)
                {
                    instrument.Cls();
                    Thread.Sleep(100);
                    instrument.VisaWrite("RUN");
                    queue.Enqueue("Script 运行中");
                    instrument.Cls();
                    int index = 0;
                    string val = "";
                    for (int i = 0; i < 100; i++)
                    {
                        #region
                        //0003OP1A0200002
                        //string error = instrument.VisaQuery("STATUS");
                        //instrument.Cls();
                        //queue.Enqueue(error);
                        //bool connect = error.Substring(6, 1) != "0";
                        //string model = error.Substring(4, 2);
                        //queue.Enqueue(error);
                        //switch (model)
                        //{
                        //    case "OP":
                        //        {
                        //            queue.Enqueue("输出功率测试中");
                        //            break;
                        //        }
                        //    case "SS":
                        //        {
                        //            queue.Enqueue("BER测试中");
                        //            break;
                        //        }
                        //    case "MI":
                        //        {
                        //            queue.Enqueue("调制指数测试中");
                        //            break;
                        //        }
                        //    case "IC":
                        //        {
                        //            queue.Enqueue("初始载波测试中");
                        //            break;
                        //        }
                        //    case "CD":
                        //        {
                        //            queue.Enqueue("载波偏移测试中");
                        //            break;
                        //        }
                        //}

                        //if(connect)
                        //{
                        //    queue.Enqueue( "连接耳机成功，测试中" );
                        //}
                        //else
                        //{
                        //    item.Result = "Fail";
                        //    item.Value = "Fail";
                        //    queue.Enqueue("测试中耳机退出TestMode，请重新进入");
                        //    queue.Enqueue("连接耳机失败");
                        //    //break;
                        //}
                        #endregion
                        val = instrument.VisaQuery("*INS?");
                        //instrument.Cls();
                        //queue.Enqueue(val);
                        if (val == "45")
                        {

                            item.Result = "Pass";
                            item.Value = "Pass";
                            queue.Enqueue("Script 运行完成");
                            break;
                        }
                        else if (val == "41")
                        {
                            queue.Enqueue("连接耳机成功，测试中");
                        }
                        else if (val == "46")
                        {
                            item.Result = "Fail";
                            item.Value = "Fail";
                            queue.Enqueue("连接超时，请检查耳机");
                            break;
                        }
                        else
                        {
                            item.Result = "Fail";
                            item.Value = "Fail";
                            queue.Enqueue("测试异常，请检查MT8852");
                            break;
                        }
                        if (index == 99 && val != "45")
                        {
                            item.Result = "Fail";
                            item.Value = "Fail";
                            queue.Enqueue("Script 运行超时");
                            break;
                        }
                        Thread.Sleep(500);

                    }
                    queue.Enqueue("断开耳机连接");

                }
            }
            return item;
        }
         
        public TestData GetTXPower(TestData item)
        {
            //XOP,HOPONL,TRUE,-15.73,-15.76,-15.22,-15.75,0,10,PASS
            string retureVal = string.Empty;
            double avgPower = 0;
            instrument.Cls();
            if (item.Other.Split(':')[1] == data.Low_Freq)
            {
                retureVal = instrument.VisaQuery("XRESULT OP,HOPOFFL");            
            }
            else if(item.Other.Split(':')[1] == data.Mod_Freq)
            {
                retureVal = instrument.VisaQuery("XRESULT OP,HOPOFFM");
            }
            else if(item.Other.Split(':')[1] == data.Hi_Freq)
            {
                retureVal = instrument.VisaQuery("XRESULT OP,HOPOFFH");
            }
            if (retureVal.Contains("XOP"))
            {
                avgPower = double.Parse(retureVal.Split(',')[6]);
            }
            if (avgPower >= double.Parse(item.LowLimit)
                && avgPower <= double.Parse(item.UppLimit))
            {
                item.Value = avgPower.ToString();
                item.Result = "Pass";
            }
            else
            {
                item.Value = avgPower.ToString();
                item.Result = "Fail";
            }
            return item;
        }

        public TestData GetSingleSensitivity(TestData item)
        {
            //XSS,HOPOFFL,FALSE,0.016,3.361,FAIL,246,4,3,7405,252,249,7408

            string retureVal = string.Empty;
            double avgPower = 0;
            instrument.Cls();
            if (item.Other.Split(':')[1] == data.Low_Freq)
            {
                retureVal = instrument.VisaQuery("XRESULT SS,HOPOFFL");
            }
            else if (item.Other.Split(':')[1] == data.Mod_Freq)
            {
                retureVal = instrument.VisaQuery("XRESULT SS,HOPOFFM");
            }
            else if (item.Other.Split(':')[1] == data.Hi_Freq)
            {
                retureVal = instrument.VisaQuery("XRESULT SS,HOPOFFH");
            }
            if (retureVal.Contains("XSS"))
            {
                avgPower = double.Parse(retureVal.Split(',')[3]);
            }
            if (avgPower >= double.Parse(item.LowLimit)
                && avgPower <= double.Parse(item.UppLimit))
            {
                item.Value = avgPower.ToString();
                item.Result = "Pass";
            }
            else
            {
                item.Value = avgPower.ToString();
                item.Result = "Fail";
            }
            return item;
        }

        public TestData GetModulationindex(TestData item)
        {

            string retureVal = string.Empty;
            double avgPower = 0;
            instrument.Cls();
            //if (item.Other.Split(':')[1] == data.Low_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT SS,HOPONL");
            //}
            //else if (item.Other.Split(':')[1] == data.Mod_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT SS,HOPONM");
            //}
            //else if (item.Other.Split(':')[1] == data.Hi_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT SS,HOPONH");
            //}
            //MI0,TRUE,1.830e+005,1.750e+005,1.224e+005,1.398e+005,0.790,FAIL
            retureVal = instrument.VisaQuery("ORESULT TEST,0,MI");
            if (retureVal.Contains("MI0"))
            {
                avgPower = double.Parse(retureVal.Split(',')[3]) / 1000;
            }
            if (avgPower >= double.Parse(item.LowLimit)
                && avgPower <= double.Parse(item.UppLimit))
            {
                item.Value = avgPower.ToString();
                item.Result = "Pass";
            }
            else
            {
                item.Value = avgPower.ToString();
                item.Result = "Fail";
            }
            return item;
        }

        public TestData GetInitialcarrier(TestData item)
        {
            //IC0,TRUE,-7300.0,-8500.0,-5900.0,-12400.0,PASS
            string retureVal = string.Empty;
            double avgPower = 0;
            instrument.Cls();
            //if (item.Other.Split(':')[1] == data.Low_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT IC,HOPONL");
            //}
            //else if (item.Other.Split(':')[1] == data.Mod_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT IC,HOPONM");
            //}
            //else if (item.Other.Split(':')[1] == data.Hi_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT IC,HOPONH");
            //}
            //IC0,TRUE,-9400.0,-8300.0,-4900.0,-10600.0,PASS
            retureVal = instrument.VisaQuery("ORESULT TEST,0,IC");
            if (retureVal.Contains("IC0"))
            {
                avgPower = double.Parse(retureVal.Split(',')[3]) / 1000;
            }
            if (avgPower >= double.Parse(item.LowLimit)
                && avgPower <= double.Parse(item.UppLimit))
            {
                item.Value = avgPower.ToString();
                item.Result = "Pass";
            }
            else
            {
                item.Value = avgPower.ToString();
                item.Result = "Fail";
            }
            return item;
        }

        public TestData GetCarrierdrift(TestData item)
        {
            string retureVal = string.Empty;
            double avgPower = 0;
            instrument.Cls();
            //if (item.Other.Split(':')[1] == data.Low_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT IC,HOPONL");
            //}
            //else if (item.Other.Split(':')[1] == data.Mod_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT IC,HOPONM");
            //}
            //else if (item.Other.Split(':')[1] == data.Hi_Freq)
            //{
            //    retureVal = instrument.VisaQuery("XRESULT IC,HOPONH");
            //}
            //CD0,TRUE,3931,TRUE,8.0e+003,FALSE,0.0e+000,FALSE,0.0e+000,PASS
            retureVal = instrument.VisaQuery("ORESULT TEST,0,CD");
            if (retureVal.Contains("CD0"))
            {
                avgPower = double.Parse(retureVal.Split(',')[2]) / 1000;
            }
            if (avgPower >= double.Parse(item.LowLimit)
                && avgPower <= double.Parse(item.UppLimit))
            {
                item.Value = avgPower.ToString();
                item.Result = "Pass";
            }
            else
            {
                item.Value = avgPower.ToString();
                item.Result = "Fail";
            }
            return item;
        }

        public TestData GetBTAddress(TestData item)
        {
            instrument.Cls();
            string address = instrument.VisaQuery("SYSCFG? EUTADDR");
            item.Value = address;
            item.Result = "Pass";
            return item;
        }

        public TestData CalFreq(TestData item)
        {
            short TrimValue = 0;
            short offset = 0;
            short writeOffset = 0;
            for (int i = 0; i < 5; i++)
            {
                csr.ColdReset();
                SetCWMode();
                csr.ReadOffset(out TrimValue);
                csr.EnterTxStart(ushort.Parse(data.Mod_Freq));
                double freqErr = ReadFreqOffect();
                csr.ColdReset();
                double Nfreq = double.Parse(data.Mod_Freq);
                double ActFrwq = Nfreq + freqErr / 1000;
                csr.CalFreq(Nfreq, ActFrwq, out offset);
                writeOffset = (short)(TrimValue + offset);

                csr.ColdReset();
                csr.EnterTxStart(ushort.Parse(data.Mod_Freq));
                freqErr = ReadFreqOffect();
                if (freqErr <= double.Parse(item.UppLimit) &&
                    freqErr >= double.Parse(item.LowLimit))
                {
                    item.Value = freqErr.ToString();
                    item.Result = "Pass";
                    break;
                }
                if(i == 4 && (freqErr <= double.Parse(item.UppLimit) &&
                    freqErr >= double.Parse(item.LowLimit)))
                {
                    item.Value = freqErr.ToString();
                    item.Result = "Fail";
                }
            }
            

            return item;
        }

        public TestData K2300Series_Open(TestData item)
        {
            try
            {
                PowerInst.OpenVisa(data.PowerPort);
                InitPower();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("打开电源供应器成功");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData K2300Series_ChannelOne_OutPut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(string.Format(":SENSe1:CURRent:DC:RANGe:AUTO ON"));
                PowerInst.VisaWrite(string.Format(":SOUR1:VOLT:LEVel {0}"
               , data.Voltage1));
                PowerInst.VisaWrite(string.Format(":SOUR1:CURRent:LIMit:VALue  {0}"
             , data.Current));
                PowerInst.VisaWrite(":OUTPut1:STAT ON");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道1输出电压");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                PowerInst.Rst();
            }
            return item;
        }

        public TestData K2300Series_ChannelTwo_OutPut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe:AUTO ON"));
                PowerInst.VisaWrite(string.Format(":SOUR2:VOLT:LEVel {0}"
             , data.Voltage2));
                PowerInst.VisaWrite(string.Format(":SOUR2:CURRent:LIMit:VALue  {0}"
             , data.Current));
                PowerInst.VisaWrite(":OUTPut2:STAT ON");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道2输出电压");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                PowerInst.Rst();
            }
            return item;
        }

        public TestData K2300Series_ChannelOne_ReadVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                double voltage = double.Parse(PowerInst.VisaQuery(":MEASure1:VOLTage:DC?"));
                if (item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                }
                if (voltage <= double.Parse(item.UppLimit)
                    && voltage >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = voltage.ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = voltage.ToString();
                    if (item.Check)
                    {
                        PowerInst.Rst();
                        PowerInst.Closed();
                    }
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if(item.Check)
                {
                    PowerInst.Rst();
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData K2300Series_ChannelOne_ReadCurrent(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                List<double> lists = new List<double>();
                double multiple = 1;
                if(item.Unit.ToUpper() == "UA")
                {
                    multiple = 1000000;
                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    multiple = 1000;
                }
                else
                {
                    multiple = 1;
                }

                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(200);
                    lists.Add(double.Parse(PowerInst.VisaQuery(":MEASure1:CURRent:DC?")));
                }
                double current = lists.Average() * multiple;
               
                if (current <= double.Parse(item.UppLimit)
                   && current >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = Math.Round(current, 3).ToString();       
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = Math.Round(current, 3).ToString();
                    if (item.Check)
                    {
                        PowerInst.Rst();
                        PowerInst.Closed();
                    }
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Rst();
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData K2300Series_ChannelTwo_ReadVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                double voltage = double.Parse(PowerInst.VisaQuery(":MEASure2:VOLTage:DC?"));
                if (item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                }
                if (voltage <= double.Parse(item.UppLimit)
                    && voltage >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = voltage.ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = voltage.ToString();
                    if (item.Check)
                    {
                        PowerInst.Rst();
                        PowerInst.Closed();
                    }
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData K2300Series_ChannelTwo_ReadCurrent(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                List<double> lists = new List<double>();
                double multiple = 1;
                if (item.Unit.ToUpper() == "UA")
                {
                    multiple = 1000000;
                    PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe {0}"
                        , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));

                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    multiple = 1000;
                    PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe {0}"
                       , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));

                }
                else
                {
                    multiple = 1;
                    PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe {0}"
                       , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));
                }

                for (int i = 0; i < 5; i++)
                {
                    lists.Add(double.Parse(PowerInst.VisaQuery(":MEASure2:CURRent:DC?")));
                }
                double current = lists.Average() * multiple;

                if (current <= double.Parse(item.UppLimit)
                   && current >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = Math.Round(current, 3).ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = Math.Round(current, 3).ToString();
                    if (item.Check)
                    {
                        PowerInst.Rst();
                        PowerInst.Closed();
                    }
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Rst();
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData K2300Series_ChannelOne_StopOut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(":OUTPut1:STAT OFF");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道1关闭输出");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Rst();
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData K2300Series_ChannelTwo_StopOut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(":OUTPut2:STAT OFF");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道2关闭输出");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Rst();
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData K2300Series_Closed(TestData item)
        {
            try
            {
                PowerInst.Closed();
                //InitPower();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("关闭电源供应器成功");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData HP66319D_Open(TestData item)
        {
            try
            {
                PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("打开电源供应器成功");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData HP66319D_ChannelOne_OutPut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                //INST: COUP: OUTP: STAT NONE
                //PowerInst.VisaWrite(string.Format("INST: COUP: OUTP: STAT NONE"));

               PowerInst.VisaWrite(string.Format(":SOUR:VOLT1:LEVel {0}"
               , data.Voltage1));
                PowerInst.VisaWrite(string.Format(":SOUR:CURR1  {0} "
             , data.Current));
                PowerInst.VisaWrite(":OUTPut1:STAT ON");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道1输出电压");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData HP66319D_ChannelTwo_OutPut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
               // PowerInst.VisaWrite(string.Format("INST: COUP: OUTP: STAT NONE"));
                PowerInst.VisaWrite(string.Format(":SOUR:VOLT2:LEVel {0}"
             , data.Voltage2));
                PowerInst.VisaWrite(string.Format(":SOUR:CURR2  {0}"
             , data.Current));
                PowerInst.VisaWrite(":OUTPut2:STAT ON");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道2输出电压");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData HP66319D_ChannelOne_ReadVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                double voltage = double.Parse(PowerInst.VisaQuery("MEAS:VOLTage1?"));
                if (item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                }
                if (voltage <= double.Parse(item.UppLimit)
                    && voltage >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = voltage.ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = voltage.ToString();
                    if (item.Check)
                    {
                        PowerInst.Closed();
                    }
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData HP66319D_ChannelOne_ReadCurrent(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                double current = double.Parse(PowerInst.VisaQuery("MEAS:CURRent1?"));
                if (item.Unit.ToUpper() == "UA")
                {
                    current *= 1000000;
                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    current *= 1000;
                }
                if (current <= double.Parse(item.UppLimit)
                   && current >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = current.ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = current.ToString();
                    if (item.Check)
                    {
                        PowerInst.Closed();
                    }
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData HP66319D_ChannelTwo_ReadVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                double voltage = double.Parse(PowerInst.VisaQuery("MEAS:VOLTage2?"));
                if (item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                }
                if (voltage <= double.Parse(item.UppLimit)
                    && voltage >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = voltage.ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = voltage.ToString();
                    if (item.Check)
                    {
                        PowerInst.Closed();
                    }
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData HP66319D_ChannelTwo_ReadCurrent(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                double current = double.Parse(PowerInst.VisaQuery("MEAS:CURRent2?"));
                if (item.Unit.ToUpper() == "UA")
                {
                    current *= 1000000;
                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    current *= 1000;
                }
                if (current <= double.Parse(item.UppLimit)
                   && current >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = current.ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = current.ToString();
                    if (item.Check)
                    {
                        PowerInst.Closed();
                    }
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData HP66319D_ChannelOne_StopOut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(":OUTPut1:STAT OFF");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道1关闭输出");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData HP66319D_ChannelTwo_StopOut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(":OUTPut2:STAT OFF");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道2关闭输出");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                if (item.Check)
                {
                    PowerInst.Closed();
                }
            }
            return item;
        }

        public TestData HP66319D_Closed(TestData item)
        {
            try
            {
                PowerInst.Closed();
                //InitPower();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("关闭电源供应器成功");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Key34461_Open(TestData item)
        {
            try
            {
                MultimeterInst.OpenVisa(data.MultimeterPort);
                MultimeterInst.Cls();
                //InitPower();
                if (data.MultimeterPort.StartsWith("ASRL"))
                {
                    MultimeterInst.VisaWrite("SYST:REM");
                }
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("打开万用表成功");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Key34461_ReadVoltage(TestData item)
        {
            try
            {
                Thread.Sleep(200);
                MultimeterInst.VisaWrite(":FUNC  \"VOLT:DC\";:VOLT:DC:RANG:AUTO ON");
                //*OPC;:SAMP:COUN 10;:TRIG:SOUR IMM;:READ?;
                Thread.Sleep(200);
                string[] values = MultimeterInst.VisaQuery(":SAMP:COUN 5;:TRIG:SOUR IMM;:READ?").Split(',');
                List<double> voltages = new List<double>();
                for (int i = 0; i < values.Length; i++)
                {
                    voltages.Add(double.Parse(values[i].Trim()));
                }
                double voltage = voltages.Average();
                if(item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                }
                if (voltage <= double.Parse(item.UppLimit)
                    && voltage >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = voltage.ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = voltage.ToString();
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Key34461_ReadCurrent(TestData item)
        {
            try
            {
                //:FUNC "CURR:DC";:CURR:DC:RANG:AUTO ON;
                //:FUNC "CURR:DC";:CURR:DC:RANG:AUTO ON;
                Thread.Sleep(200);
                MultimeterInst.VisaWrite(":FUNC  \"CURR:DC\";:CURR:DC:RANG:AUTO ON;");
                Thread.Sleep(200);
                string[] values = MultimeterInst.VisaQuery(":SAMP:COUN 5;:TRIG:SOUR IMM;:READ?").Split(',');
                List<double> currents = new List<double>();
                for (int i = 0; i < values.Length; i++)
                {
                    currents.Add(double.Parse(values[i].Trim()));
                }
                double current = currents.Average();
                if (item.Unit.ToUpper() == "MA")
                {
                    current *= 1000;
                }
                else if(item.Unit.ToUpper() == "UA")
                {
                    current *= 1000000;
                }
                if (current <= double.Parse(item.UppLimit)
                    && current >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = current.ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = current.ToString();
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Key34461_Closed(TestData item)
        {
            try
            {
                MultimeterInst.Closed();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("关闭万用表成功");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public void CloesdInstr()
        {
            if (instrument != null)
            {
                instrument.Closed();
            }
            if(PowerInst != null)
            {
                PowerInst.Closed();
            }
            if(MultimeterInst != null)
            {
                MultimeterInst.Closed();
            }
        }
    }
}
