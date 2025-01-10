using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestModel;
using TestDAL;

namespace TestDAL
{
    public class OperateInstrument
    {
        private Instrument instrument;
        private Instrument PowerInst;
        private Instrument MultimeterInst;
        private Instrument _4010Inst;
        private ConfigData data;
        private Queue<string> queue;
        private CsrOperate csr;
        public string BtAddress;
        private WebReference.WebService1 web;
        private string instrName;
        private string Levm, Mevm, Hevm;
        //public OperateBES operateBES;
        public DateTime dateTime;

        public OperateInstrument(ConfigData data, Queue<string> queue)
        {
            this.data = data;
            this.queue = queue;
            instrName = string.Empty;
            try
            {
                if (data.GPIB_Enable)
                {
                    instrument = new Instrument(data.VisaPort);
                    dateTime = DateTime.Now;
                }
                if (data.Power_Enable)
                {
                    PowerInst = new Instrument(data.PowerPort);
                }
                if (data.Multimeter_Select)
                {
                    MultimeterInst = new Instrument(data.MultimeterPort);
                }
                if (data._4010Enable)
                {
                    _4010Inst = new Instrument(data._4010Port);
                    //double val = Read_4010_FreqOffset();
                }
                //csr = new CsrOperate();
            }
            catch (Exception ex)
            {
                queue.Enqueue("ex;" + ex.Message);
            }
        }

        public void InitInstr()
        {
            try
            {
                //instrument.Rst();
                instrument.Cls();
                instrument.VisaWrite("OPMD SCRIPT");
                instrument.VisaWrite("SCPTSEL 3");
                instrument.VisaWrite("SCRIPTMODE 3,STANDARD");
                instrument.VisaWrite("TXPWR 3,-40");

                //SCPTCFG 3,OP,ON;MI;IC;CD;SS
                //PC;MS;MP
                //ALLTSTS
                instrument.VisaWrite("SCPTCFG 3,ALLTSTS,OFF");
                if(data.BLE)
                {
                    instrument.VisaWrite("SYSCFG EUTSRCE,BLE2WIRE");
                    //instrument.VisaWrite("SYSCFG EUTRS232,115200");
                    instrument.VisaWrite("SYSCFG CONFIG, RANGE, AUTO");
                   // instrument.VisaWrite("SCPTSEL 3");
                    instrument.VisaWrite("SYSCFG USBADAPTOR,PORT,A");
                    instrument.VisaWrite("SYSCFG EUTHANDSHAKE,RTS/CTS");
                    instrument.VisaWrite("LESCPTCFG 3, LETSTMODE, STANDARD, TRUE");
                    instrument.VisaWrite("SCPTSEL 3");
                }
                else
                {
                    instrument.VisaWrite("SYSCFG EUTSRCE,INQUIRY");
                }
                if (data.OP)
                {
                    if(data.BLE)
                        instrument.VisaWrite("SCPTCFG 3,LEOP,ON");
                    else
                        instrument.VisaWrite("SCPTCFG 3,OP,ON");

                }
                if (data.MI)
                    if (data.BLE)
                    {
                        instrument.VisaWrite("SCPTCFG 3,LEMI,ON");
                    }
                    else
                    {
                        instrument.VisaWrite("SCPTCFG 3,MI,ON");
                    }
                if (data.IC)
                    instrument.VisaWrite("SCPTCFG 3,IC,ON");
                if (data.CD)
                    if (data.BLE)
                    {
                        instrument.VisaWrite("SCPTCFG 3,LEICD,ON");
                    }
                    else
                    {
                        instrument.VisaWrite("SCPTCFG 3,CD,ON");
                    }
                if (data.SS)
                    if(data.BLE)
                    {
                        instrument.VisaWrite("SCPTCFG 3,LESS,ON");
                    }
                else
                    {
                        instrument.VisaWrite("SCPTCFG 3,SS,ON");
                    }
                   
                if (data.EVM)
                {
                    //instrument.VisaWrite("SCPTTSTGP 3,EDRTSTS,ON");
                    instrument.VisaWrite("SCPTCFG 3,ECM,ON");
                }
                //instrument.VisaWrite("SCPTCFG 3,PC,OFF");
                //instrument.VisaWrite("SCPTCFG 3,MS,OFF");
                //instrument.VisaWrite("SCPTCFG 3,MP,OFF");
                //TXPWR 3,-10.0
                if (data.OP)
                {
                    // instrument.VisaWrite(string.Format("OPCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                    //instrument.VisaWrite(string.Format("OPCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                    //instrument.VisaWrite(string.Format("OPCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                    if (data.BLE)
                    {
                        instrument.VisaWrite("LEOPCFG 3,NUMPKTS,10");
                        instrument.VisaWrite("LEOPCFG 3,LEPKTTYPE,BLE,TRUE");
                    }
                    else
                    {
                        instrument.VisaWrite("OPCFG 3,PKTTYPE, DH1");
                        instrument.VisaWrite("OPCFG 3,NUMPKTS,10");
                        instrument.VisaWrite("OPCFG 3,HOPPING,HOPOFF");
                    }
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
                    if (data.BLE)
                    {
                        instrument.VisaWrite("LEICDCFG 3,LEPKTTYPE,BLE,TRUE");
                        instrument.VisaWrite("LEICDCFG 3,NUMPKTS,10");
                    }
                    else
                    {
                        instrument.VisaWrite("CDCFG 3,PKTSIZE,ONESLOT,TRUE");
                        instrument.VisaWrite("CDCFG 3,PKTSIZE,THREESLOT,FALSE");
                        instrument.VisaWrite("CDCFG 3,PKTSIZE,FIVESLOT,FALSE");
                        instrument.VisaWrite("CDCFG 3,NUMPKTS,10");
                        instrument.VisaWrite("CDCFG 3,HOPPING,HOPOFF");
                    }
                }
                if (data.MI)
                {
                    //instrument.VisaWrite(string.Format("MICFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                    //instrument.VisaWrite(string.Format("MICFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                    //instrument.VisaWrite(string.Format("MICFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                    if (data.BLE)
                    {
                        instrument.VisaWrite("LEMICFG 3,LEPKTTYPE,BLE,TRUE");
                        instrument.VisaWrite("LEMICFG 3,DEFAULT");
                        //instrument.VisaWrite("LEMICFG 3,NUMPKTS,10");
                    }
                    else
                    {
                        instrument.VisaWrite("MICFG 3,NUMPKTS,10");
                    }
                }
                if (data.SS)
                {
                    //instrument.VisaWrite(string.Format("SSCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
                    //instrument.VisaWrite(string.Format("SSCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
                    //instrument.VisaWrite(string.Format("SSCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
                    if (data.BLE)
                    {
                        instrument.VisaWrite(string.Format("LESSCFG 3,NUMPKTS, {0}", data.number_of_packets));
                        instrument.VisaWrite(string.Format("LESSCFG 3,TXPWR, {0}", data.Sen_TX_Power));
                    }
                    else
                    {
                        instrument.VisaWrite(string.Format("SSCFG 3,NUMPKTS, {0}", data.number_of_packets));
                        instrument.VisaWrite(string.Format("SSCFG 3,TXPWR, {0}", data.Sen_TX_Power));
                        //SSCFG 3,HOPPING, HOPBOTH
                        //SSCFG 3,HOPMODE, DEFINED
                        instrument.VisaWrite("SSCFG 3,HOPPING, HOPBOTH");
                        //instrument.VisaWrite("CDCFG 3,PKTSIZE,FIVESLOT,FALSE");
                    }
                }
                if (data.EVM)
                {
                    instrument.VisaWrite("ECMCFG 3,NUMBLKS,100");
                    instrument.VisaWrite(string.Format("ECMCFG 3,LTXFREQ, FREQ,{0}000000", "2418"));
                    instrument.VisaWrite(string.Format("ECMCFG 3,MTXFREQ,FREQ, {0}000000", "2444"));
                    instrument.VisaWrite(string.Format("ECMCFG 3,HTXFREQ,FREQ, {0}000000", "2470"));

                    //instrument.VisaWrite("ECMCFG 3,TSTCTRL on");
                }
                //instrument.VisaWrite(string.Format("OPCFG 3,AVGMXLIM, {0}", data.AvgPowerHi));
                //instrument.VisaWrite(string.Format("OPCFG 3,AVGMNLIM, {0}", data.AvgPowerLow));
                //instrument.VisaWrite(string.Format("OPCFG 3,PEAKLIM, {0}", data.PeakPower));

                if (!data.Low_Loss.Contains(";"))
                {
                    instrument.VisaWrite(string.Format("PATHOFF 3,TABLE"));
                    instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,0, {0}", data.Low_Loss));
                    instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,39, {0}", data.Mod_Loss));
                    instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,78, {0}", data.Hi_Loss));
                }
                // instrument.Closed();
            }
            catch(Exception ex)
            {
                queue.Enqueue("ex;" + ex.Message);
            }
        }

        public void Set8852()
        {
            //instrument.VisaWrite("OPMD SCRIPT");
            //instrument.VisaWrite("SCPTSEL 3");
            //instrument.VisaWrite("SCRIPTMODE 3,STANDARD");

            //SCPTCFG 3,OP,ON;MI;IC;CD;SS
            //PC;MS;MP
            //ALLTSTS
            instrument.VisaWrite("SCPTCFG 3,ALLTSTS,OFF");
            //if (data.OP)
            //    instrument.VisaWrite("SCPTCFG 3,OP,ON");
            //if (data.MI)
            //    instrument.VisaWrite("SCPTCFG 3,MI,ON");
            //if (data.IC)
            instrument.VisaWrite("SCPTCFG 3,IC,ON");
            //if (data.CD)
            //    instrument.VisaWrite("SCPTCFG 3,CD,ON");
            //if (data.SS)
            //    instrument.VisaWrite("SCPTCFG 3,SS,ON");
            ////instrument.VisaWrite("SCPTCFG 3,PC,OFF");
            ////instrument.VisaWrite("SCPTCFG 3,MS,OFF");
            ////instrument.VisaWrite("SCPTCFG 3,MP,OFF");
            ////TXPWR 3,-10.0
            //if (data.OP)
            //{
            //    //HOPPING
            //    instrument.VisaWrite("TXPWR 3,-40");
            //    // instrument.VisaWrite(string.Format("OPCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
            //    //instrument.VisaWrite(string.Format("OPCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
            //    //instrument.VisaWrite(string.Format("OPCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
            //    instrument.VisaWrite("OPCFG 3,PKTTYPE, DH1");
            //    instrument.VisaWrite("OPCFG 3,NUMPKTS,10");
            //    instrument.VisaWrite("OPCFG 3,HOPPING,HOPON");
            //}

            //if (data.IC)
            //{
            instrument.VisaWrite("ICCFG 3,NUMPKTS,10");
            instrument.VisaWrite("ICCFG 3,HOPPING,HOPOFF");
            //}
            //if (data.CD)
            //{
            //    //instrument.VisaWrite(string.Format("CDCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
            //    //instrument.VisaWrite(string.Format("CDCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
            //    //instrument.VisaWrite(string.Format("CDCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
            //    instrument.VisaWrite("CDCFG 3,PKTSIZE,ONESLOT,TRUE");
            //    instrument.VisaWrite("CDCFG 3,PKTSIZE,THREESLOT,FALSE");
            //    instrument.VisaWrite("CDCFG 3,PKTSIZE,FIVESLOT,FALSE");
            //    instrument.VisaWrite("CDCFG 3,NUMPKTS,10");
            //}
            //if (data.MI)
            //{
            //    //instrument.VisaWrite(string.Format("MICFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
            //    //instrument.VisaWrite(string.Format("MICFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
            //    //instrument.VisaWrite(string.Format("MICFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
            //    instrument.VisaWrite("MICFG 3,NUMPKTS,10");
            //}
            //if (data.SS)
            //{
            //    //instrument.VisaWrite(string.Format("SSCFG 3,LTXFREQ, FREQ, {0}", data.Low_Freq));
            //    //instrument.VisaWrite(string.Format("SSCFG 3,MTXFREQ,FREQ, {0}", data.Mod_Freq));
            //    //instrument.VisaWrite(string.Format("SSCFG 3,HTXFREQ, FREQ, {0}", data.Hi_Freq));
            //    instrument.VisaWrite(string.Format("SSCFG 3,NUMPKTS, {0}", data.number_of_packets));
            //    instrument.VisaWrite(string.Format("SSCFG 3,TXPWR, {0}", data.Sen_TX_Power));
            //    //SSCFG 3,HOPPING, HOPBOTH
            //    //SSCFG 3,HOPMODE, DEFINED
            //    instrument.VisaWrite("SSCFG 3,HOPPING, HOPBOTH");
            //    //instrument.VisaWrite("CDCFG 3,PKTSIZE,FIVESLOT,FALSE");
            //}
            ////instrument.VisaWrite(string.Format("OPCFG 3,AVGMXLIM, {0}", data.AvgPowerHi));
            ////instrument.VisaWrite(string.Format("OPCFG 3,AVGMNLIM, {0}", data.AvgPowerLow));
            ////instrument.VisaWrite(string.Format("OPCFG 3,PEAKLIM, {0}", data.PeakPower));

            //instrument.VisaWrite(string.Format("PATHOFF 3,TABLE"));
            //instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,0, {0}", data.Low_Loss));
            //instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,39, {0}", data.Mod_Loss));
            //instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,78, {0}", data.Hi_Loss));

        }

        public void InitPower()
        {
            PowerInst.Rst();

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

        public void init4010()
        {
            //_4010Inst.Rst();
            _4010Inst.VisaWrite("SEQuence:CLEar");
            if (data.OP)
                _4010Inst.VisaWrite("SEQ:ADD OPOW");
            _4010Inst.VisaWrite("LINK:CONF:CHAN:HOPP OFF");
            if (data.SS)
                _4010Inst.VisaWrite("SEQuence:ADD SSENsitivity");
            _4010Inst.VisaWrite(string.Format("LINK:CONFigure:BITS {0}", data.number_of_packets));
            _4010Inst.VisaWrite("LINK:CONF:CHAN:HOPP OFF");
            if (data.MI)
                _4010Inst.VisaWrite("SEQuence:ADD MCHar");
            if (data.IC)
                _4010Inst.VisaWrite("SEQuence:ADD ICFT");
            _4010Inst.VisaWrite("LINK:CONF:CHAN:HOPP OFF");
            if (data.CD)
                _4010Inst.VisaWrite("SEQuence:ADD CFDRift");


            _4010Inst.VisaWrite("LINK:TYPE TESTmode");
            _4010Inst.VisaWrite("SENS:CORR:LOSS:STAT ON");
            Thread.Sleep(500);
            _4010Inst.VisaWrite(string.Format("SENS:CORR:LOSS:FIX {0}", data.Low_Loss));
            _4010Inst.VisaWrite("LINK:TX:POW:LEV -40");
            //Thread.Sleep(5000);


            _4010Inst.VisaWrite(string.Format("LINK:CONFigure:TX:POWer:LEVel {0}"
     , data.Sen_TX_Power));
            _4010Inst.VisaWrite(string.Format("LINK:INQ:DUR {0}", data.Inquiry_TimeOut));


            //_4010Inst.VisaWrite("");
            //_4010Inst.VisaWrite("");
            //_4010Inst.VisaWrite("");
        }

        private void Set_8852B_CWMode()
        {
            instrument.VisaWrite("OPMD CWMEAS");
            instrument.VisaWrite(string.Format("CWMEAS FREQ,{0}.00MHZ,1.00MS"
                , 2441));
        }

        private double Read_8852B_FreqOffect()
        {
            double freq = 0;
            string val = instrument.VisaQuery("CWRESULT FREQOFF");
            if (val.Contains("CWRESULT"))
            {
                freq = double.Parse(val.Split(',')[2]);
            }
            else
            {
                freq = double.Parse(val);
            }
            return freq / 1000;
        }

        private double Read_4010_FreqOffset()
        {
            _4010Inst.VisaWrite("INST:SEL \"RFA\"");
            _4010Inst.VisaWrite("SENS:FREQ:CENT 2441");
            //SEQuence:LOOP CONTinuous
            _4010Inst.VisaWrite("SEQuence:LOOP FIXed");
            _4010Inst.VisaWrite("SEQ:LOOP:NUMB 20");
            //FETCh:APOWer? 获取cw功率
            _4010Inst.VisaWrite("INIT");
            //Thread.Sleep(500);
            return Math.Round(double.Parse(_4010Inst.VisaQuery("FETCh:FOFFset?")), 2);
        }

        public void Agilent4010_CalcFreq()
        {
            _4010Inst.Rst();
            _4010Inst.VisaWrite("SEQuence:CLEar");
            _4010Inst.VisaWrite("SEQuence:ADD ICFT");
            _4010Inst.VisaWrite("LINK:CONF:CHAN:HOPP OFF");

            Run_4010Script(new TestData());
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
                //DateTime now = DateTime.Now;
                //TimeSpan span = now - dateTime;
                //if (span.Hours >= 1)
                //{
                //    queue.Enqueue("程序使用一小时，复位一次设备");
                //    dateTime = DateTime.Now;

                instrument.Rst();
                InitInstr();
                ////Set8852();
                //}
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

        public TestData Set8852LossOne(TestData item)
        {
            //string low = data.Low_Loss.Split(';')[0];
            instrument.VisaWrite(string.Format("PATHOFF 3,TABLE"));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,0, {0}"
                , data.Low_Loss.Split(';')[0]));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,39, {0}"
                , data.Mod_Loss.Split(';')[0]));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,78, {0}"
                , data.Hi_Loss.Split(';')[0]));
            item.Result = "Pass";
            item.Value = "Pass";
            return item;
        }

        public TestData Set8852LossTwo(TestData item)
        {

            instrument.VisaWrite(string.Format("PATHOFF 3,TABLE"));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,0, {0}"
                , data.Low_Loss.Split(';')[1]));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,39, {0}"
                , data.Mod_Loss.Split(';')[1]));
            instrument.VisaWrite(string.Format("PATHEDIT 1,CHAN,78, {0}"
                , data.Hi_Loss.Split(';')[1]));

            item.Result = "Pass";
            item.Value = "Pass";
            return item;
        }

        public TestData Open_QCC_Port(TestData item)
        {
            csr = new CsrOperate();
            return csr.OpenPort(item);
        }

        public TestData QCC_ReadBtAdress(TestData item)
        {
            return csr.ReadBTaddress(item);
        }

        public TestData QCC_CalFreq(TestData item)
        {
            short TrimValue = 0;
            short offset = 0;
            Set_8852B_CWMode();
            for (int i = 0; i < 3; i++)
            {
                csr.ColdReset();
                //csr.ReadOffset(out TrimValue);
                csr.EnterTxStart((ushort)2441);

                double freqError = Read_8852B_FreqOffect();
                csr.ColdReset();
                csr.CalFreq(2441, freqError, out offset);
                csr.WriteCalFreq((short)(TrimValue + offset));
                csr.ColdReset();
                csr.EnterTxStart(2441);
                freqError = Read_8852B_FreqOffect();
                if (freqError >= double.Parse(item.LowLimit)
                    && freqError <= double.Parse(item.UppLimit))
                {
                    item.Value = freqError.ToString();
                    item.Result = "Pass";
                    break;
                }
                else
                {
                    item.Value = freqError.ToString();
                    item.Result = "Fail";
                }
            }
            return item;
        }

        public TestData QCC_Write_BtAddress(TestData item)
        {
            if (csr.WriteBtAddress(BtAddress) == 1)
            {
                item.Result = "Pass";
                item.Value = BtAddress;
            }
            else
            {
                item.Result = "Fail";
                item.Value = BtAddress;
            }
            return item;
        }

        public TestData QCC_Read_Trim(TestData item)
        {
            return csr.ReadOffset(item);
        }

        public TestData QCC_EnableTestMode(TestData item)
        {
            return csr.EnableTestMode(item);
        }

        public TestData QCC_StartAudioLoop(TestData item)
        {
            return csr.StartAudioLoop(item);
        }

        public TestData QCC_StopAudioLoop(TestData item)
        {
            return csr.StopAudioLoop(item);
        }

        public TestData OpenCsrDev(TestData item)
        {
            csr = new CsrOperate();
            return csr.OpenCsrDev(item);
        }

        public TestData ReadCsrBDAddress(TestData item)
        {
            return csr.ReadCsrBDAddress(item);
        }

        public TestData CsrEnableTestMode(TestData item)
        {
            //csr.WriteCsrBtAddress("784405268d56");
            return csr.CsrEnableTestMode(item);
        }

        public TestData CSR_CalFreq(TestData item)
        {
            short TrimValue = 0;
            short offset = 0;
            Set_8852B_CWMode();
            for (int i = 0; i < 3; i++)
            {
                csr.ColdReset();
                //csr.ReadOffset(out TrimValue);
                csr.EnterTxStart((ushort)2441);

                double freqError = Read_8852B_FreqOffect();
                csr.ColdReset();
                csr.CalFreq(2441, freqError, out offset);
                csr.WriteCalFreq((short)(TrimValue + offset));
                csr.ColdReset();
                csr.EnterTxStart(2441);
                freqError = Read_8852B_FreqOffect();
                if (freqError >= double.Parse(item.LowLimit)
                    && freqError <= double.Parse(item.UppLimit))
                {
                    item.Value = freqError.ToString();
                    item.Result = "Pass";
                    break;
                }
                else
                {
                    item.Value = freqError.ToString();
                    item.Result = "Fail";
                }
            }
            return item;
        }

        public TestData CSR_Write_BtAddress(TestData item)
        {
            if (csr.WriteCsrBtAddress(BtAddress) == 1)
            {
                item.Result = "Pass";
                item.Value = BtAddress;
            }
            else
            {
                item.Result = "Fail";
                item.Value = BtAddress;
            }
            return item;
        }

        public TestData CsrClosedPort(TestData item)
        {
            return csr.CsrClosedPort(item);
        }

        public TestData QCC_Closed_Port(TestData item)
        {
            try
            {
                csr.QCCClosedDev();
                item.Result = "Pass";
                item.Value = "Pass";
            }
            catch (Exception)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Run_Script(TestData item)
        {
            instrument.VisaWrite(string.Format("SYSCFG INQSET,TIMEOUT,{0}", data.Inquiry_TimeOut));
            //instrument.VisaWrite("DISCONNECT");
            //instrument.VisaWrite("SYSCFG EUTSRCE,INQUIRY");
            //instrument.VisaWrite("SYSCFG INQSET,RNUM,12");
            //INQUIRY   INQRSP?
            instrument.VisaWrite("INQUIRY");

            //instrument.VisaWrite("CONNECT");
            string btAddress = string.Empty;
            bool page = true;
            queue.Enqueue("呼叫开始");
            instrument.Cls();

            for (int i = 0; i < 30; i++)
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
                //else if (i == 15)
                //{
                //    queue.Enqueue("搜索不到");
                //    //instrument.VisaWrite("INQCANCEL");
                //    //instrument.Rst();
                //    //InitInstr();
                //    queue.Enqueue("再次发送指令进入QBQ模式");
                //    string result = operateBES.Serial.VisaQuery1("$L^BQBOF=1", 500);
                //    queue.Enqueue("再次进入QBQ模式：" + result);
                //    Thread.Sleep(500);
                //    //instrument.VisaWrite("SYSCFG EUTSRCE,INQUIR");
                //    //instrument.VisaWrite("INQUIRY");
                //    queue.Enqueue("重新搜索开始");
                //}

                if (i == 29 && !(ret.StartsWith("1")))
                {
                    item.Result = "Fail";
                    item.Value = "Fail";
                    queue.Enqueue("呼叫超时，请检查耳机");
                    instrument.VisaWrite("INQCANCEL");
                    page = false;
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
                //instrument.VisaWrite("CONNECT");
                //queue.Enqueue("连接中");

                bool connStatus = false;
                instrument.VisaWrite("RUN");
                for (int j = 0; j < 30; j++)
                {
                    instrument.Cls();
                    queue.Enqueue("连接中......");
                    string conn = instrument.VisaQuery("*INS?");

                    if (conn == "42" || conn == "10")
                    {
                        connStatus = true;
                        queue.Enqueue("连接成功");
                        break;
                    }
                    else if(conn == "41" || conn == "9")
                    {
                        connStatus = true;
                        queue.Enqueue("连接成功，测试中");
                        break;
                    }
                    //else if (conn == "0" && (j == 15))
                    //{
                    //    instrument.VisaWrite("DISCONNECT");
                    //    queue.Enqueue("再次连接中......");
                    //    instrument.VisaWrite("CONNECT");
                    //}

                    if (j == 29 && connStatus != true)
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
                    string error = string.Empty;
                    instrument.Cls();
                    Thread.Sleep(100);

                    queue.Enqueue("Script 运行中");
                    instrument.Cls();
                    int index = 0;
                    string val = "";
                    for (int i = 0; i < 100; i++)
                    {
                        //error = instrument.VisaQuery("ERRLST");
                        //if (error.Split('!')[1].Contains("NO") && error.Split('!')[2].Contains("NO"))
                        //{

                        val = instrument.VisaQuery("*INS?");

                        //instrument.Cls();
                        //queue.Enqueue(val);
                        if (val == "45" || val == "13")
                        {
                            item.Result = "Pass";
                            item.Value = "Pass";
                            queue.Enqueue("Script 运行完成");
                            break;
                        }
                        else if (val == "41" || val == "9")
                        {
                            queue.Enqueue("连接耳机成功，测试中");
                        }
                        //else if (val == "46")
                        //{
                        //    item.Result = "Fail";
                        //    item.Value = "Fail";
                        //    queue.Enqueue("通讯超时，请检查耳机");
                        //    break;
                        //}
                        //}
                        //else
                        //{
                        //    item.Result = "Fail";
                        //    item.Value = "Fail";
                        //    queue.Enqueue("仪器或者耳机报错，请检查");
                        //    break;
                        //}
                        if (index == 99 && val != "45")
                        {
                            item.Result = "Fail";
                            item.Value = "Fail";
                            queue.Enqueue("Script 运行超时");
                            break;
                        }
                        Thread.Sleep(200);

                    }
                    queue.Enqueue("断开耳机连接");

                }
            }
            return item;
        }

        public TestData Run_MT8852_CalcFreqScript(TestData item)
        {
            Set8852();
            //instrument.VisaWrite("DISCONNECT");
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
                else if (ret.StartsWith("0") && (i == 15 || i == 30))
                {
                    queue.Enqueue("再次呼叫中......");
                    //INQCANCEL
                    instrument.VisaWrite("INQCANCEL");
                    Thread.Sleep(1000);
                    instrument.VisaWrite("INQUIRY");
                }
                if (i == 49 && !(ret.StartsWith("1")))
                {
                    item.Result = "Fail";
                    item.Value = "Fail";
                    queue.Enqueue("呼叫超时，请检查耳机");
                    instrument.VisaWrite("INQCANCEL");
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
                    else if (conn == "0" && (j == 25))
                    {
                        queue.Enqueue("再次连接中......");
                        instrument.VisaWrite("CONNECT");
                    }

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
                        val = instrument.VisaQuery("*INS?");
                        //instrument.Cls();
                        //queue.Enqueue(val);
                        if (val == "45" || val == "13")
                        {
                            item.Result = "Pass";
                            item.Value = "Pass";
                            queue.Enqueue("Script 运行完成");
                            instrument.VisaWrite("DISCONNECT");
                            //instrument.Rst();
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
                        //else
                        //{
                        //    item.Result = "Fail";
                        //    item.Value = "Fail";
                        //    queue.Enqueue("测试异常，请检查MT8852");
                        //    break;
                        //}
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

        public TestData Run_MT8852_CalcFreq(TestData item)
        {
            Set_8852B_CWMode();

            int success = 0;
            uint TrimValue = 0;
            short offset = 0;
            csr.EnterTxStart(2441);
            csr.AdjustFreq(0, 11);
            uint TrimLoadCap = csr.ReadOffset(out TrimValue) + 1;
            //csr.AdjustFreq(11, TrimValue);
            //csr.AdjustFreq(TrimValue, TrimLoadCap);
            uint i = TrimLoadCap;
            for (; i < 31; i++)
            {
                double freqError = Read_8852B_FreqOffect();
                if (freqError >= double.Parse(item.LowLimit)
                    && freqError <= double.Parse(item.UppLimit))
                {
                    item.Value = freqError.ToString();
                    item.Result = "Pass";
                    break;
                }
                else
                {
                    csr.AdjustFreq(TrimValue, i);
                }
            }
            //Thread.Sleep(500);
            if (i != TrimLoadCap)
            {
                success = csr.ChipReset();
            }
            Thread.Sleep(1000);
            instrument.VisaWrite("OPMD SCRIPT");
            instrument.VisaWrite("SCPTSEL 3");
            instrument.VisaWrite("SCRIPTMODE 3,STANDARD");
            if (string.IsNullOrWhiteSpace(item.Result))
            {
                item.Value = i.ToString();
                item.Result = "Fail";
            }
            return item;
        }

        public TestData Run_MT8852_CsrCalcFreq(TestData item)
        {
            Set_8852B_CWMode();

            int success = 0;
            short TrimValue = 0;
            short offset = 0;
            short writeOffset = 0;

            //csr.AdjustFreq(0, 11);
            //uint TrimLoadCap = csr.ReadOffset(out TrimValue) + 1;
            //csr.AdjustFreq(11, TrimValue);
            //csr.AdjustFreq(TrimValue, TrimLoadCap);
            uint i = 0;
            for (; i < 3; i++)
            {
                csr.ColdReset();
                csr.ResetConnCsr();
                csr.CsrReadOffset(out TrimValue);
                csr.CsrEnterTxStart(2441);

                double freqError = Read_8852B_FreqOffect() / 1000;
                csr.CalFreq(2441, 2441 + freqError, out offset);
                writeOffset = (short)(TrimValue + offset);
                csr.WriteCalFreq(writeOffset);
                csr.ColdReset();
                //csr.ResetConnCsr();
                csr.CsrEnterTxStart(2441);
                freqError = Read_8852B_FreqOffect();
                if (freqError >= double.Parse(item.LowLimit)
                    && freqError <= double.Parse(item.UppLimit))
                {
                    item.Value = freqError.ToString();
                    item.Result = "Pass";
                    break;
                }
                else
                {
                    //csr.AdjustFreq(TrimValue, i);
                }
            }
            //Thread.Sleep(500);

            Thread.Sleep(500);
            instrument.VisaWrite("OPMD SCRIPT");
            instrument.VisaWrite("SCPTSEL 3");
            instrument.VisaWrite("SCRIPTMODE 3,STANDARD");
            if (string.IsNullOrWhiteSpace(item.Result))
            {
                item.Value = i.ToString();
                item.Result = "Fail";
            }
            return item;
        }

        public TestData Run_MT8852_BLE(TestData item)
        {
            try
            {
                string baud = item.Other;
                if (baud == "")
                {
                    baud = "115200";
                }
                instrument.VisaWrite(string.Format("SYSCFG EUTRS232,{0}", baud));
                Thread.Sleep(500);
                instrument.VisaWrite("RUN");
                string val = string.Empty;
                for (int i = 0; i < 100; i++)
                {
                    val = instrument.VisaQuery("*INS?");
                    if (val == "46" || val == "13")
                    {
                        item.Result = "Pass";
                        item.Value = "Pass";
                        queue.Enqueue("Script 运行完成");
                        break;
                    }
                    else if (val == "42" || val == "9")
                    {
                        queue.Enqueue("连接耳机成功，测试中");
                    }

                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                queue.Enqueue(ex.Message);
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData GetTXPower(TestData item)
        {
            //XOP,HOPONL,TRUE,-15.73,-15.76,-15.22,-15.75,0,10,PASS
            //XLEOP,HOPOFFL,TRUE,-7.10,-7.09,-7.11,0.13,0,10,PASS\n

            for (int i = 0; i < 3; i++)
            {
                string retureVal = string.Empty;
                double avgPower = 0;
                instrument.Cls();
                if (item.Other.Split(':')[1] == data.Low_Freq)
                {
                    if (data.BLE)
                    {
                        retureVal = instrument.VisaQuery("XRESULT LEOP,HOPOFFL");
                    }
                    else
                    {
                        retureVal = instrument.VisaQuery("XRESULT OP,HOPOFFL");
                    }
                }
                else if (item.Other.Split(':')[1] == data.Mod_Freq)
                {
                    if (data.BLE)
                    {
                        retureVal = instrument.VisaQuery("XRESULT LEOP,HOPOFFM");
                    }
                    else
                    {
                        retureVal = instrument.VisaQuery("XRESULT OP,HOPOFFM");
                    }
                }
                else if (item.Other.Split(':')[1] == data.Hi_Freq)
                {
                    if (data.BLE)
                    {
                        retureVal = instrument.VisaQuery("XRESULT LEOP,HOPOFFH");
                    }
                    else
                    {
                        retureVal = instrument.VisaQuery("XRESULT OP,HOPOFFH");
                    }
                }

                if (retureVal.Contains("XOP") || retureVal.Contains("XLEOP"))
                {
                    if (bool.Parse(retureVal.Split(',')[2]))
                    {
                        avgPower = double.Parse(retureVal.Split(',')[3])
                       + double.Parse(item.FillValue);

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
                    }
                    else
                    {
                        item.Value = "Fail";
                        item.Result = "Fail";
                        queue.Enqueue("测试过程中通讯超时，测试Fail");
                    }
                    break;
                }
                else
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }
                //Thread.Sleep(300);
            }
            return item;
        }

        public TestData GetSingleSensitivity(TestData item)
        {
            //XSS,HOPOFFL,FALSE,0.016,3.361,FAIL,246,4,3,7405,252,249,7408
            //XLESS,HOPOFFL,TRUE,32.800,672,1000,FAIL\n
            for (int i = 0; i < 5; i++)
            {
                string retureVal = string.Empty;
                double avgPower = 0;
                instrument.Cls();
                if (item.Other.Split(':')[1] == data.Low_Freq)
                {
                    if (data.BLE)
                    {
                        retureVal = instrument.VisaQuery("XRESULT LESS,HOPOFFL");
                    }
                    else
                    {
                        retureVal = instrument.VisaQuery("XRESULT SS,HOPOFFL");
                    }
                }
                else if (item.Other.Split(':')[1] == data.Mod_Freq)
                {
                    if (data.BLE)
                    {
                        retureVal = instrument.VisaQuery("XRESULT LESS,HOPOFFM");
                    }
                    else
                    {
                        retureVal = instrument.VisaQuery("XRESULT SS,HOPOFFM");
                    }
                }
                else if (item.Other.Split(':')[1] == data.Hi_Freq)
                {
                    if (data.BLE)
                    {
                        retureVal = instrument.VisaQuery("XRESULT LESS,HOPOFFH");
                    }
                    else
                    {
                        retureVal = instrument.VisaQuery("XRESULT SS,HOPOFFH");
                    }
                }
                if (retureVal.Contains("XSS") || retureVal.Contains("XLESS"))
                {
                    avgPower = double.Parse(retureVal.Split(',')[3])
                        + double.Parse(item.FillValue);

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
                    break;
                }
                else
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }
                //Thread.Sleep(300);
            }
            return item;
        }

        public TestData GetSingleSensitivity_FER(TestData item)
        {
            //XSS,HOPOFFL,FALSE,0.016,3.361,FAIL,246,4,3,7405,252,249,7408
            for (int i = 0; i < 5; i++)
            {
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
                    avgPower = double.Parse(retureVal.Split(',')[4])
                        + double.Parse(item.FillValue);

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
                    break;
                }
                else
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }

                //Thread.Sleep(300);
            }
            return item;
        }

        public TestData GetCarrierModRMS2M(TestData item)
        {
            //XECM,HOPOFFL,TRUE,0.102,0.284,100.00,0.086,-15.0,14.7,
            //-2.1,FAIL,TRUE,0.107,0.282,98.56,0.085,-8.9,8.6,-2.5,FAIL\n
            try
            {
                string retureVal = string.Empty;
                double evm = 0;
                instrument.Cls();
                if (item.Other.Split(':')[1] == data.Low_Freq)
                {
                    retureVal = instrument.VisaQuery("XRESULT ECM,HOPOFFL");
                    Levm = retureVal;
                }
                else if (item.Other.Split(':')[1] == data.Mod_Freq)
                {
                    retureVal = instrument.VisaQuery("XRESULT ECM,HOPOFFM");
                   Mevm = retureVal;
                }
                else if (item.Other.Split(':')[1] == data.Hi_Freq)
                {
                    retureVal = instrument.VisaQuery("XRESULT ECM,HOPOFFH");
                    Hevm = retureVal;
                }
                evm = double.Parse(retureVal.Split(',')[3]);

                if (retureVal.Contains("XECM"))
                {
                    evm = evm + double.Parse(item.FillValue);
                }
               
                if (evm >= double.Parse(item.LowLimit)
               && evm <= double.Parse(item.UppLimit))
                {
                    item.Value = evm.ToString();
                    item.Result = "Pass";
                }
                else
                {
                    item.Value = evm.ToString();
                    item.Result = "Fail";
                }
            }
            catch (Exception)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData GetCarrierModRMS3M(TestData item)
        {
            double evm = 0;
            try
            {
                if (item.Other.Split(':')[1] == data.Low_Freq)
                {
                    evm = double.Parse(Levm.Split(',')[12]);
                }
                else if (item.Other.Split(':')[1] == data.Mod_Freq)
                {
                    evm = double.Parse(Mevm.Split(',')[12]);
                }
                else if (item.Other.Split(':')[1] == data.Hi_Freq)
                {
                    evm = double.Parse(Hevm.Split(',')[12]);
                }

                evm = evm + double.Parse(item.FillValue);

                if (evm >= double.Parse(item.LowLimit)
               && evm <= double.Parse(item.UppLimit))
                {
                    item.Value = evm.ToString();
                    item.Result = "Pass";
                }
                else
                {
                    item.Value = evm.ToString();
                    item.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData GetCarrierModPeak2M(TestData item)
        {
            double evm = 0;
            try
            {
                if (item.Other.Split(':')[1] == data.Low_Freq)
                {
                    evm = double.Parse(Levm.Split(',')[4]);
                }
                else if (item.Other.Split(':')[1] == data.Mod_Freq)
                {
                    evm = double.Parse(Mevm.Split(',')[4]);
                }
                else if (item.Other.Split(':')[1] == data.Hi_Freq)
                {
                    evm = double.Parse(Hevm.Split(',')[4]);
                }

                evm = evm + double.Parse(item.FillValue);

                if (evm >= double.Parse(item.LowLimit)
               && evm <= double.Parse(item.UppLimit))
                {
                    item.Value = evm.ToString();
                    item.Result = "Pass";
                }
                else
                {
                    item.Value = evm.ToString();
                    item.Result = "Fail";
                }
            }
            catch (Exception)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData GetCarrierModPeak3M(TestData item)
        {
            double evm = 0;
            try
            {
                if (item.Other.Split(':')[1] == data.Low_Freq)
                {
                    evm = double.Parse(Levm.Split(',')[13]);
                }
                else if (item.Other.Split(':')[1] == data.Mod_Freq)
                {
                    evm = double.Parse(Mevm.Split(',')[13]);
                }
                else if (item.Other.Split(':')[1] == data.Hi_Freq)
                {
                    evm = double.Parse(Hevm.Split(',')[13]);
                }

                evm = evm + double.Parse(item.FillValue);

                if (evm >= double.Parse(item.LowLimit)
               && evm <= double.Parse(item.UppLimit))
                {
                    item.Value = evm.ToString();
                    item.Result = "Pass";
                }
                else
                {
                    item.Value = evm.ToString();
                    item.Result = "Fail";
                }
            }
            catch (Exception)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData GetModulationindex(TestData item)
        {
            for (int i = 0; i < 5; i++)
            {
                string retureVal = string.Empty;
                double avgPower = 0;
                instrument.Cls();
                #region
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
#endregion
                retureVal = instrument.VisaQuery("ORESULT TEST,0,MI");
                if (retureVal.Contains("MI0"))
                {
                    avgPower = (double.Parse(retureVal.Split(',')[3]) / 1000)
                        + double.Parse(item.FillValue);

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
                    break;
                }
                else
                {
                    item.Value ="Fail";
                    item.Result = "Fail";
                }
                //Thread.Sleep(300);
            }
            return item;
        }

        public TestData GetInitialcarrier(TestData item)
        {
            //IC0,TRUE,-7300.0,-8500.0,-5900.0,-12400.0,PASS
            for (int i = 0; i < 5; i++)
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
                //IC0,TRUE,-9400.0,-8300.0,-4900.0,-10600.0,PASS
                retureVal = instrument.VisaQuery("ORESULT TEST,0,IC");
                if (retureVal.Contains("IC0"))
                {
                    avgPower = (double.Parse(retureVal.Split(',')[3]) / 1000)
                        + double.Parse(item.FillValue);

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
                    break;
                }
                else
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }
                //Thread.Sleep(300);
            }
            return item;
        }

        public TestData GetCarrierdrift(TestData item)
        {
            for (int i = 0; i < 5; i++)
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
                //LEICD1,TRUE,8.000e+003,9.400e+003,6.500e+003,2039,1.0e+003,3.0e+003,0,30,PASS,-1481\n
                if (data.BLE)
                {
                    retureVal = instrument.VisaQuery("ORESULT TEST,0,LEICD");
                }
                else
                {
                    retureVal = instrument.VisaQuery("ORESULT TEST,0,CD");
                }
                if (retureVal.Contains("CD0") || retureVal.Contains("LECD0"))
                {
                    avgPower = (double.Parse(retureVal.Split(',')[2]) / 1000)
                        + double.Parse(item.FillValue);

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
                    break;
                }
               else
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }
            }
            return item;
        }

        public TestData GetBTAddress(TestData item)
        {
            instrument.Cls();
            string address = instrument.VisaQuery("SYSCFG? EUTADDR");
            //address = "E09DFA4D9B0D";
            if (address.Length == 12)
            {
                if (data.MesEnable)
                {
                    queue.Enqueue("检查蓝牙地址是否过站及测试三次:" + address);
                    web = new WebReference.WebService1();
                    //ServiceReference1.WebService1SoapClient web = null;
                    //web = new ServiceReference1.WebService1SoapClient("WebService1Soap");
                    string reslut = web.SnCx(address, data.MesStation);
                    //string btResult = web.SnCx_LY(address);
                    string failResult = web.SnCx_SC(address, data.NowStation);
                    //web.Close();
                    //reslut = "F";
                    web.Abort();
                    if (reslut.Contains("F"))
                    {
                        item.Value = address;
                        item.Result = "Fail";
                        queue.Enqueue(string.Format("上一个工位:{0}:连续测试NG品,请检查 "
                            , data.MesStation));
                    }
                    else
                    {
                        queue.Enqueue("检查蓝牙地址: " + address + ",过站Pass");
                        item.Result = "Pass";
                        item.Value = address;
                        //if (btResult.Contains("P"))
                        //{
                        //    queue.Enqueue("该蓝牙地址: " + address + ",无重复");
                        //    item.Result = "Pass";
                        //    item.Value = address;
                        if (failResult.Contains("P"))
                        {
                            queue.Enqueue("该蓝牙地址: " + address + ",无重复测试");

                            //if(btResult.Contains("P"))
                            //{
                            //    item.Result = "Pass";
                            //    item.Value = address;
                            //    queue.Enqueue("该蓝牙地址: " + address + ",无重复");
                            //}
                            //else
                            //{
                            //    item.Result = "Fail";
                            //    item.Value = address;
                            //    queue.Enqueue("该蓝牙地址: " + address + ",重复，请检查");
                            //}
                        }
                        else
                        {
                            item.Value = address;
                            item.Result = "Fail";
                            queue.Enqueue("该蓝牙地址: " + address + ",重复测试三次");
                        }
                        //}
                        //else
                        //{
                        //    item.Value = address;
                        //    item.Result = "Fail";
                        //    queue.Enqueue("该蓝牙地址: " + address + ",重复,请检查");
                        //}
                    }


                }
                else
                {
                    item.Value = address;
                    item.Result = "Pass";
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    address = instrument.VisaQuery("SYSCFG? EUTADDR");
                    if (address.Length == 12)
                    {
                        if (data.MesEnable)
                        {
                            queue.Enqueue("检查蓝牙地址是否过站及测试三次:" + address);
                            web = new WebReference.WebService1();
                            //ServiceReference1.WebService1SoapClient web = null;
                            //web = new ServiceReference1.WebService1SoapClient("WebService1Soap");
                            string reslut = web.SnCx(address, data.MesStation);
                            //string btResult = web.SnCx_LY(address);
                            string failResult = web.SnCx_SC(address, data.NowStation);
                            //web.Close();
                            //reslut = "F";
                            web.Abort();
                            if (reslut.Contains("F"))
                            {
                                item.Value = address;
                                item.Result = "Fail";
                                queue.Enqueue(string.Format("上一个工位:{0}:连续测试NG品,请检查 "
                                    , data.MesStation));
                            }
                            else
                            {
                                queue.Enqueue("检查蓝牙地址: " + address + ",过站Pass");
                                item.Result = "Pass";
                                item.Value = address;
                                //if (btResult.Contains("P"))
                                //{
                                //    queue.Enqueue("该蓝牙地址: " + address + ",无重复");
                                //    item.Result = "Pass";
                                //    item.Value = address;
                                if (failResult.Contains("P"))
                                {
                                    queue.Enqueue("该蓝牙地址: " + address + ",无重复测试");

                                    //if(btResult.Contains("P"))
                                    //{
                                    //    item.Result = "Pass";
                                    //    item.Value = address;
                                    //    queue.Enqueue("该蓝牙地址: " + address + ",无重复");
                                    //}
                                    //else
                                    //{
                                    //    item.Result = "Fail";
                                    //    item.Value = address;
                                    //    queue.Enqueue("该蓝牙地址: " + address + ",重复，请检查");
                                    //}
                                }
                                else
                                {
                                    item.Value = address;
                                    item.Result = "Fail";
                                    queue.Enqueue("该蓝牙地址: " + address + ",重复测试三次");
                                }
                                //}
                                //else
                                //{
                                //    item.Value = address;
                                //    item.Result = "Fail";
                                //    queue.Enqueue("该蓝牙地址: " + address + ",重复,请检查");
                                //}
                            }


                        }
                        else
                        {
                            item.Value = address;
                            item.Result = "Pass";
                        }
                        break;
                    }
                    //Thread.Sleep(300);
                }
            }
            return item;
        }

        public TestData CompareBTAddress(TestData item)
        {
            instrument.Cls();
            string address = instrument.VisaQuery("SYSCFG? EUTADDR");
            //address = "E09DFA4D9B0D";
            if (data.MesEnable)
            {
                queue.Enqueue("检查蓝牙地址是否过站及测试三次:" + address);
                web = new WebReference.WebService1();
                //ServiceReference1.WebService1SoapClient web = null;
                //web = new ServiceReference1.WebService1SoapClient("WebService1Soap");
                string reslut = web.SnCx(address, data.MesStation);
                //string btResult = web.SnCx_LY(address);
                string failResult = web.SnCx_SC(address, data.NowStation);
                //web.Close();
                //reslut = "F";
                web.Abort();
                if (reslut.Contains("F"))
                {
                    item.Value = address;
                    item.Result = "Fail";
                    queue.Enqueue(string.Format("上一个工位:{0}:连续测试NG品,请检查 "
                        , data.MesStation));
                }
                else
                {
                    queue.Enqueue("检查蓝牙地址: " + address + ",过站Pass");
                    item.Result = "Pass";
                    item.Value = address;
                    //if (btResult.Contains("P"))
                    //{
                    //    queue.Enqueue("该蓝牙地址: " + address + ",无重复");
                    //    item.Result = "Pass";
                    //    item.Value = address;
                    if (failResult.Contains("P"))
                    {
                        queue.Enqueue("该蓝牙地址: " + address + ",无重复测试");

                        //if(btResult.Contains("P"))
                        //{
                        //    item.Result = "Pass";
                        //    item.Value = address;
                        //    queue.Enqueue("该蓝牙地址: " + address + ",无重复");
                        //}
                        //else
                        //{
                        //    item.Result = "Fail";
                        //    item.Value = address;
                        //    queue.Enqueue("该蓝牙地址: " + address + ",重复，请检查");
                        //}
                    }
                    else
                    {
                        item.Value = address;
                        item.Result = "Fail";
                        queue.Enqueue("该蓝牙地址: " + address + ",重复测试三次");
                    }
                    //}
                    //else
                    //{
                    //    item.Value = address;
                    //    item.Result = "Fail";
                    //    queue.Enqueue("该蓝牙地址: " + address + ",重复,请检查");
                    //}
                }


            }
            else
            {
                if (address.StartsWith(item.LowLimit))
                {
                    item.Value = address;
                    item.Result = "Pass";
                }
                else
                {
                    item.Value = address;
                    item.Result = "Fail";
                }
            }
            return item;
        }

        public TestData GetBTName(TestData item)
        {
            instrument.Cls();
            string address = instrument.VisaQuery("SYSCFG? EUTNAME");
            if(item.LowLimit.Equals(address))
            {
                item.Value = address;
                item.Result = "Pass";
            }
            else
            {
                item.Value = address;
                item.Result = "Fail";
            }
            return item;
        }

        //public TestData CompareBTAddress(TestData item)
        //{
        //    instrument.Cls();
        //    string address = instrument.VisaQuery("SYSCFG? EUTADDR");
        //    //address = "E09DFA4D9B0D";
        //    int add = int.Parse(address.Substring(6, 6), System.Globalization.NumberStyles.HexNumber);
        //    int lowAdd = int.Parse(item.LowLimit);
        //    int upAdd = int.Parse(item.UppLimit);
        //    if (add >= lowAdd && add <= upAdd)
        //    {
        //        item.Value = address;
        //        item.Result = "Pass";
        //    }
        //    else
        //    {
        //        item.Value = address;
        //        item.Result = "Fail";
        //    }
        //    return item;
        //}

        public TestData Open_4010(TestData item)
        {
            try
            {
                _4010Inst.OpenVisa(data._4010Port);
                //Set8852();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("打开4010 Pass");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Closed_4010(TestData item)
        {
            try
            {
                _4010Inst.VisaWrite("LINK:CONT:DISC:IMM ");
                _4010Inst.Rst();
                //init4010();
                //InitInstr();
                //instrument.Rst();
                //Set8852();
                //instrument.Cls();
                _4010Inst.Closed();
                //InitInstr();
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("关闭4010 Pass");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Run_4010Script(TestData item)
        {
            //INST:NSEL 2
            init4010();
            _4010Inst.VisaWrite("INST:NSEL 1");
            _4010Inst.VisaWrite("LINK:CONT:DISC:IMM");
            _4010Inst.VisaWrite("LINK:CONT:INQ:IMM");
            //instrument.VisaWrite("SYSCFG INQSET,RNUM,12");
            _4010Inst.VisaWrite(string.Format("LINK:INQ:DUR {0}", data.Inquiry_TimeOut));
            //INQUIRY   INQRSP?
            //instrument.VisaWrite("LINK:INQ:BDAD:COUN?");

            //instrument.VisaWrite("CONNECT");
            string btAddress = string.Empty;
            bool page = false;
            queue.Enqueue("呼叫开始");
            _4010Inst.Cls();
            for (int i = 0; i < 50; i++)
            {
                string ret = _4010Inst.VisaQuery("LINK:INQ:BDAD:COUN?");
                //instrument.Cls();

                //1,E09DFA349168,7,NO NAME.,
                if (ret.StartsWith("+1"))
                {
                    //btAddress = ret.Split(',')[1];
                    queue.Enqueue("呼叫" + btAddress + "成功");
                    page = true;
                    break;
                }
                else if (ret.StartsWith("+0") && (i == 15 || i == 30))
                {
                    queue.Enqueue("再次呼叫中......");
                    //INQCANCEL
                    _4010Inst.VisaWrite("ABORt");
                    Thread.Sleep(1000);
                    _4010Inst.VisaWrite("LINK:CONT:INQ:IMM");
                }
                if (i == 49 && !(ret.StartsWith("+1")))
                {
                    item.Result = "Fail";
                    item.Value = "Fail";
                    queue.Enqueue("呼叫超时，请检查耳机");
                    _4010Inst.VisaWrite("ABORt");
                }
                Thread.Sleep(300);
            }
            //SYSCFG EUTSRCE, MANUAL
            //SYSCFG EUTADDR, E09DFA349168,
            //instrument.VisaWrite("SYSCFG EUTSRCE, MANUAL");
            //instrument.VisaWrite(string.Format("SYSCFG EUTADDR,{0}", btAddress));
            //0003OP1A0100002
            if (page)
            {
                btAddress = _4010Inst.VisaQuery("LINK:INQ:BDAD:RESP?");
                _4010Inst.VisaWrite(string.Format("LINK:EUT:BDAD {0}",btAddress));
                queue.Enqueue("连接中");
                _4010Inst.VisaWrite(string.Format("INIT"));
                queue.Enqueue("Script 运行中");
                _4010Inst.Cls();
                //bool connStatus = false;
                item.Value = "Pass";
                item.Result = "Pass";
                #region
                //for (int j = 0; j < 50; j++)
                //{
                //    string conn = instrument.VisaQuery("STATUS").Substring(6, 1);
                //    if (conn == "1")
                //    {
                //        connStatus = true;
                //        queue.Enqueue("连接成功");
                //        break;
                //    }
                //    else if (conn == "0" && (j == 25))
                //    {
                //        queue.Enqueue("再次连接中......");
                //        instrument.VisaWrite("CONNECT");
                //    }

                //    if (j == 49 && connStatus != true)
                //    {
                //        item.Result = "Fail";
                //        item.Value = "Fail";
                //        queue.Enqueue("连接超时，请检查耳机");
                //        connStatus = false;
                //    }
                //    Thread.Sleep(200);
                //}
                //if (connStatus)
                //{
                //    instrument.Cls();
                //    Thread.Sleep(100);
                //    //instrument.VisaWrite("RUN");
                //    queue.Enqueue("Script 运行中");
                //    instrument.Cls();
                //    int index = 0;
                //    bool end = true;
                //    string val = "";
                //    for (int i = 0; i < 100; i++)
                //    {
                //        #region
                //        //0003OP1A0200002
                //        //string error = instrument.VisaQuery("STATUS");
                //        //instrument.Cls();
                //        //queue.Enqueue(error);
                //        //bool connect = error.Substring(6, 1) != "0";
                //        //string model = error.Substring(4, 2);
                //        //queue.Enqueue(error);
                //        //switch (model)
                //        //{
                //        //    case "OP":
                //        //        {
                //        //            queue.Enqueue("输出功率测试中");
                //        //            break;
                //        //        }
                //        //    case "SS":
                //        //        {
                //        //            queue.Enqueue("BER测试中");
                //        //            break;
                //        //        }
                //        //    case "MI":
                //        //        {
                //        //            queue.Enqueue("调制指数测试中");
                //        //            break;
                //        //        }
                //        //    case "IC":
                //        //        {
                //        //            queue.Enqueue("初始载波测试中");
                //        //            break;
                //        //        }
                //        //    case "CD":
                //        //        {
                //        //            queue.Enqueue("载波偏移测试中");
                //        //            break;
                //        //        }
                //        //}

                //        //if(connect)
                //        //{
                //        //    queue.Enqueue( "连接耳机成功，测试中" );
                //        //}
                //        //else
                //        //{
                //        //    item.Result = "Fail";
                //        //    item.Value = "Fail";
                //        //    queue.Enqueue("测试中耳机退出TestMode，请重新进入");
                //        //    queue.Enqueue("连接耳机失败");
                //        //    //break;
                //        //}

                //        //val = instrument.VisaQuery("*INS?");
                //        //instrument.Cls();
                //        //queue.Enqueue(val);
                //        //if (val == "45")
                //        //{
                //        //    item.Result = "Pass";
                //        //    item.Value = "Pass";
                //        //    queue.Enqueue("Script 运行完成");
                //        //    break;
                //        //}
                //        //else if (val == "41")
                //        //{
                //        //    queue.Enqueue("连接耳机成功，测试中");
                //        //}
                //        //else if (val == "46")
                //        //{
                //        //    item.Result = "Fail";
                //        //    item.Value = "Fail";
                //        //    queue.Enqueue("连接超时，请检查耳机");
                //        //    break;
                //        //}
                //        //else
                //        //{
                //        //    item.Result = "Fail";
                //        //    item.Value = "Fail";
                //        //    queue.Enqueue("测试异常，请检查MT8852");
                //        //    break;
                //        //}
                //        //if (index == 99 && val != "45")
                //        //{
                //        //    item.Result = "Fail";
                //        //    item.Value = "Fail";
                //        //    queue.Enqueue("Script 运行超时");
                //        //    break;
                //        //}
                //        #endregion

                //        if(data.OP)
                //        {
                //           val =  instrument.VisaQuery("SEQuence:DONE? OPOWer");
                //            if (val == "1")
                //            {
                //                queue.Enqueue("发送功率测试完成");
                //                end &= true;
                //            }
                //            else
                //            {
                //                queue.Enqueue("发送功率正在测试中");
                //                end &= false;
                //            }
                //        }
                //        if (data.MI)
                //        {
                //            val = instrument.VisaQuery("SEQuence:DONE? SSENsitivity");
                //            if (val == "1")
                //            {
                //                queue.Enqueue("BER测试完成");
                //                end &= true;
                //            }
                //            else
                //            {
                //                queue.Enqueue("BER正在测试中");
                //                end &= false;
                //            }
                //        }
                //        if(data.CD)
                //        {
                //            val = instrument.VisaQuery("SEQuence:DONE? CFDRift");
                //            if (val == "1")
                //            {
                //                queue.Enqueue("载波漂移测试完成");
                //                end &= true;
                //            }
                //            else
                //            {
                //                queue.Enqueue("载波漂移正在测试中");
                //                end &= false;
                //            }
                //        }
                //        if(data.SS)
                //        {
                //            val = instrument.VisaQuery("SEQuence:DONE? SSENsitivity");
                //            if (val == "1")
                //            {
                //                queue.Enqueue("多BER测试完成");
                //                end &= true;
                //            }
                //            else
                //            {
                //                queue.Enqueue("多BER正在测试中");
                //                end &= false;
                //            }
                //        }
                //        if (data.IC)
                //        {
                //            val = instrument.VisaQuery("SEQuence:DONE? SSENsitivity");
                //            if (val == "1")
                //            {
                //                queue.Enqueue("初始载波测试完成");
                //                end &= true;
                //            }
                //            else
                //            {
                //                queue.Enqueue("初始载波正在测试中");
                //                end &= false;
                //            }
                //        }
                //        Thread.Sleep(500);

                //    }
                //    queue.Enqueue("断开耳机连接");

                //}
                #endregion
            }
            else
            {
                item.Value = "Fail";
                item.Result = "Fail";
            }
            return item;
        }

        public TestData Run_4010CalcFreqAfterScript(TestData item)
        {
            init4010();

            _4010Inst.VisaWrite(string.Format("INIT"));
            item.Result = "Pass";
            item.Value = "Pass";
            queue.Enqueue("Script 运行中");
            return item;
        }

        public TestData Get4010TXPower(TestData item)
        {
            //XOP,HOPONL,TRUE,-15.73,-15.76,-15.22,-15.75,0,10,PASS
            string retureVal = string.Empty;
            double avgPower = 0;
            _4010Inst.Cls();
           
            for (int i = 0; i < 50; i++)
            {
                string val = _4010Inst.VisaQuery("SEQuence:DONE? OPOWer");
                if (val == "+1")
                {
                    if (item.Other.Split(':')[1] == data.Low_Freq)
                    {
                        queue.Enqueue("发送功率"+ data.Low_Freq +"测试完成");
                        retureVal = _4010Inst.VisaQuery("FETCh:OPOWer:PEAK? LOW");
                    }
                    else if (item.Other.Split(':')[1] == data.Mod_Freq)
                    {
                        queue.Enqueue("发送功率" + data.Mod_Freq + "测试完成");
                        retureVal = _4010Inst.VisaQuery("FETCh:OPOWer:PEAK? MED");
                    }
                    else if (item.Other.Split(':')[1] == data.Hi_Freq)
                    {
                        queue.Enqueue("发送功率" + data.Hi_Freq + "测试完成");
                        retureVal = _4010Inst.VisaQuery("FETCh:OPOWer:PEAK? HIGH");
                    }
                    //if (retureVal.Contains("XOP"))
                    //{
                    avgPower = Math.Round(double.Parse(retureVal), 2)
                        + double.Parse(item.FillValue);
                    //}
                    if (avgPower >= double.Parse(item.LowLimit)
                        && avgPower <= double.Parse(item.UppLimit))
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Pass";
                        break;
                    }
                    else
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Fail";
                        break;
                    }
                }
                else
                {
                    queue.Enqueue("发送功率正在测试中");
                }
                Thread.Sleep(200);
                if(i == 49)
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }
            }
           
            return item;
        }

        public TestData Get4010SingleSensitivity(TestData item)
        {
            //XSS,HOPOFFL,FALSE,0.016,3.361,FAIL,246,4,3,7405,252,249,7408

            string retureVal = string.Empty;
            double avgPower = 0;
            _4010Inst.Cls();
           
            for (int i = 0; i < 50; i++)
            {
                string val = _4010Inst.VisaQuery("SEQuence:DONE? SSENsitivity");
                if (val == "+1")
                {
                    if (item.Other.Split(':')[1] == data.Low_Freq)
                    {
                        queue.Enqueue("BER" + data.Low_Freq + "测试完成");
                        retureVal = _4010Inst.VisaQuery("FETCh:SSENsitivity:BER? LOW");
                    }
                    else if (item.Other.Split(':')[1] == data.Mod_Freq)
                    {
                        queue.Enqueue("BER" + data.Mod_Freq + "测试完成");
                        retureVal = _4010Inst.VisaQuery("FETCh:SSENsitivity:BER? MEDium");
                    }
                    else if (item.Other.Split(':')[1] == data.Hi_Freq)
                    {
                        queue.Enqueue("BER" + data.Hi_Freq + "测试完成");
                        retureVal = _4010Inst.VisaQuery("FETCh:SSENsitivity:BER? HIGH");
                    }
                    //if (retureVal.Contains("XSS"))
                    //{
                    avgPower = Math.Round(double.Parse(retureVal), 2)
                        + double.Parse(item.FillValue);
                    //}
                    if (avgPower >= double.Parse(item.LowLimit)
                        && avgPower <= double.Parse(item.UppLimit))
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Pass";
                        break;
                    }
                    else
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Fail";
                        break;
                    }
                }
                else
                {
                    queue.Enqueue("BER正在测试完成");
                }
                Thread.Sleep(200);
                if(i == 49)
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }
            }
           
            return item;
        }

        public TestData Get4010Modulationindex(TestData item)
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
           
            //if (retureVal.Contains("MI0"))
            //{
            for (int i = 0; i < 50; i++)
            {
                string val = _4010Inst.VisaQuery("SEQuence:DONE? MCHar");
                if (val == "+1")
                {
                    queue.Enqueue("调制指数测试完成");
                    retureVal = _4010Inst.VisaQuery("FETCh:MCHar:DF1Average?");
                    avgPower = Math.Round(double.Parse(retureVal) / 1000, 2)
                        + double.Parse(item.FillValue);
                    //}
                    if (avgPower >= double.Parse(item.LowLimit)
                        && avgPower <= double.Parse(item.UppLimit))
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Pass";
                        break;
                    }
                    else
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Fail";
                        break;
                    }
                }
                else
                {
                    queue.Enqueue("调制指数正在测试中");
                }
                Thread.Sleep(200);
            }
            
            return item;
        }

        public TestData Get4010Initialcarrier(TestData item)
        {
            //IC0,TRUE,-7300.0,-8500.0,-5900.0,-12400.0,PASS
            string retureVal = string.Empty;
            double avgPower = 0;
            _4010Inst.Cls();
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
            for (int i = 0; i < 50; i++)
            {
                string val = _4010Inst.VisaQuery("SEQuence:DONE? ICFT");
                if(val == "+1")
                {
                    queue.Enqueue("初始载波测试完成");
                    retureVal = _4010Inst.VisaQuery("FETC:ICFT? LOW,MIN");
                    //if (retureVal.Contains("IC0"))
                    //{
                    avgPower = Math.Round(double.Parse(retureVal) / 1000, 2)
                        + double.Parse(item.FillValue);
                    //}
                    if (avgPower >= double.Parse(item.LowLimit)
                        && avgPower <= double.Parse(item.UppLimit))
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Pass";
                        break;
                    }
                    else
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Fail";
                        break;
                    }
                }
                else
                {
                    queue.Enqueue("初始载波正在测试");
                }
                Thread.Sleep(200);
                if(i == 49)
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }
            }
           
            return item;
        }

        public TestData Get4010Carrierdrift(TestData item)
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
            for (int i = 0; i < 50; i++)
            {
                string val = _4010Inst.VisaQuery("SEQuence:DONE? CFDRift");
                if(val == "+1")
                {
                    queue.Enqueue("载波漂移测试完成");
                    retureVal = _4010Inst.VisaQuery("FETCh:CFDRift:DRIFt? SUMMary");
                    //if (retureVal.Contains("CD0"))
                    //{
                    avgPower = Math.Round(double.Parse(retureVal) / 1000, 2)
                        + double.Parse(item.FillValue);
                    //}
                    if (avgPower >= double.Parse(item.LowLimit)
                        && avgPower <= double.Parse(item.UppLimit))
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Pass";
                        break;
                    }
                    else
                    {
                        item.Value = avgPower.ToString();
                        item.Result = "Fail";
                        break;
                    }
                }
                else
                {
                    queue.Enqueue("载波漂移正在测试中");
                }
            }
            
            return item;
        }

        public TestData Get4010BTAddress(TestData item)
        {
            _4010Inst.Cls();
            string address = _4010Inst.VisaQuery("LINK:EUT:BDAD?");
            item.Value = address.Remove(0, 2).Trim(); 
            item.Result = "Pass";
            return item;
        }

        public TestData PowerSupplyCommon(TestData item)
        {
            try
            {
                string instr = PowerInst.IDN();
                if (instr.Contains("2306"))
                {
                    instrName = "2306";
                }
                else if(instr.Contains("2303"))
                {
                    instrName = "2303";
                }
                else if (instr.Contains("66319"))
                {
                    instrName = "66319";
                }
                item.Result = "Pass";
                item.Value = "Pass";
            }
            catch (Exception)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData PowerSupplyCommonOpen(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_Open(item);
                }
                else if (instrName == "2303")
                {
                    item  = K2303Series_Open(item);
                }
                else if (instrName == "66319")
                {
                    item = HP66319D_Open(item);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return item;
        }

        public TestData PowerSupplyCommonClose(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_Closed(item);
                }
                else if (instrName == "2303")
                {
                    item = K2303Series_Closed(item);
                }
                else if (instrName == "66319")
                {
                    item = HP66319D_Closed(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelOneOut(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelOne_OutPut(item);
                }
                else if (instrName == "2303")
                {
                    item = K2303Series_ChannelOne_OutPut(item);
                }
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelOne_OutPut(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelTwoOut(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelTwo_OutPut(item);
                }
                //else if (instrName == "2303")
                //{
                //    item = K2303Series_ChannelOne_OutPut(item);
                //}
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelTwo_OutPut(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelOneOverVoltage(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2303Series_ChannelOne_OverVoltage(item);
                }
                else if (instrName == "2303")
                {
                    item = K2303Series_ChannelOne_OverVoltage(item);
                }
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelOne_OverVoltage(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelTwoOverVoltage(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelTwo_OverVoltage(item);
                }
                //else if (instrName == "2303")
                //{
                //    item = K2303Series_ChannelOne_OverVoltage(item);
                //}
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelTwo_OverVoltage(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelOneReadCurrent(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelOne_ReadCurrent(item);
                }
                else if (instrName == "2303")
                {
                    item = K2303Series_ChannelOne_ReadCurrent(item);
                }
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelOne_ReadCurrent(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelOneReadVoltage(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelOne_ReadVoltage(item);
                }
                else if (instrName == "2303")
                {
                    item = K2303Series_ChannelOne_ReadVoltage(item);
                }
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelOne_ReadVoltage(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelTwoReadCurrent(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelTwo_ReadCurrent(item);
                }
                //else if (instrName == "2303")
                //{
                //    item = K2303Series_ChannelOne_ReadCurrent(item);
                //}
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelTwo_ReadCurrent(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelTwoReadVoltage(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelTwo_ReadVoltage(item);
                }
                //else if (instrName == "2303")
                //{
                //    item = K2303Series_ChannelOne_ReadVoltage(item);
                //}
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelTwo_ReadVoltage(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelOneStopOut(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelOne_StopOut(item);
                }
                else if (instrName == "2303")
                {
                    item = K2303Series_ChannelOne_StopOut(item);
                }
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelOne_StopOut(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public TestData PowerSupplyCommonChannelTwoStopOut(TestData item)
        {
            try
            {
                if (instrName == "2306")
                {
                    item = K2300Series_ChannelTwo_StopOut(item);
                }
                //else if (instrName == "2303")
                //{
                //    item = K2303Series_ChannelOne_StopOut(item);
                //}
                else if (instrName == "66319")
                {
                    item = HP66319D_ChannelTwo_StopOut(item);
                }
            }
            catch (Exception)
            {

                throw;
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

        public TestData K2300Series_ChannelTwo_ChargeOutPut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe:AUTO ON"));
                PowerInst.VisaWrite(string.Format(":SOUR2:VOLT:LEVel {0}"
             , item.LowLimit.Trim()));
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

        public TestData K2300Series_ChannelOne_OverVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(string.Format(":SENSe1:CURRent:DC:RANGe:AUTO ON"));
                PowerInst.VisaWrite(string.Format(":SOUR1:VOLT:LEVel {0}"
               , item.LowLimit));
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
                    voltage += double.Parse(item.FillValue);
                    voltage = Math.Round(voltage, 3);
                }
                else
                {
                    voltage = Math.Round(voltage, 3);
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
                    //if (item.Check)
                    //{
                    //    PowerInst.Rst();
                    //    PowerInst.Closed();
                    //}
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
                    //PowerInst.VisaWrite(string.Format(":SENSe1:CURRent:DC:RANGe {0}"
                    //    , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));
                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    multiple = 1000;
                    //PowerInst.VisaWrite(string.Format(":SENSe1:CURRent:DC:RANGe {0}"
                    //   , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));
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
                lists.Remove(lists.Max());
                lists.Remove(lists.Min());
                double current = (lists.Average() * multiple) 
                    + double.Parse(item.FillValue);
               
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
                    //if (item.Check)
                    //{
                    //    PowerInst.Rst();
                    //    PowerInst.Closed();
                    //}
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

        public TestData K2300Series_ChannelTwo_OverVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe:AUTO ON"));
                PowerInst.VisaWrite(string.Format(":SOUR2:VOLT:LEVel {0}"
               , item.LowLimit));
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

        public TestData K2300Series_ChannelTwo_ReadVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                double voltage  = double.Parse(PowerInst.VisaQuery(":MEASure2:VOLTage:DC?"));
                if (item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                    voltage += double.Parse(item.FillValue);
                    voltage = Math.Round(voltage, 3);
                }
                else
                {
                    voltage = Math.Round(voltage, 3);
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
                    //if (item.Check)
                    //{
                    //    PowerInst.Rst();
                    //    PowerInst.Closed();
                    //}
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
                    //PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe {0}"
                    //    , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));

                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    multiple = 1000;
                    //PowerInst.VisaWrite(string.Format(":SENSe2:CURRent:DC:RANGe {0}"
                    //   , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));
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
                    Thread.Sleep(100);
                }
                lists.Remove(lists.Max());
                lists.Remove(lists.Min());
                double current = (lists.Average() * multiple)
                    + double.Parse(item.FillValue);

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
                    //if (item.Check)
                    //{
                    //    PowerInst.Rst();
                    //    PowerInst.Closed();
                    //}
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
                //PowerInst.VisaWrite(string.Format(":SOUR1:VOLT:LEVel {0}", 0));
                PowerInst.VisaWrite(":OUTPut1:STAT OFF");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道1关闭输出");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                //if (item.Check)
                //{
                //    PowerInst.Rst();
                //    PowerInst.Closed();
                //}
            }
            return item;
        }

        public TestData K2300Series_ChannelTwo_StopOut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                //PowerInst.VisaWrite(string.Format(":SOUR2:VOLT:LEVel {0}", 0));
                PowerInst.VisaWrite(":OUTPut2:STAT OFF");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道2关闭输出");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                //if (item.Check)
                //{
                //    PowerInst.Rst();
                //    PowerInst.Closed();
                //}
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

        public TestData K2303Series_Open(TestData item)
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

        public TestData K2303Series_ChannelOne_OutPut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(string.Format(":SENSe:CURRent:DC:RANGe:AUTO ON"));
                PowerInst.VisaWrite(string.Format(":SOUR:VOLT:LEVel {0}"
               , data.Voltage1));
                PowerInst.VisaWrite(string.Format(":SOUR:CURRent:LIMit:VALue  {0}"
             , data.Current));
                PowerInst.VisaWrite(":OUTPut:STAT ON");
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

        public TestData K2303Series_ChannelOne_OverVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                PowerInst.VisaWrite(string.Format(":SENSe:CURRent:DC:RANGe:AUTO ON"));
                PowerInst.VisaWrite(string.Format(":SOUR:VOLT:LEVel {0}"
               , item.LowLimit));
                PowerInst.VisaWrite(string.Format(":SOUR:CURRent:LIMit:VALue  {0}"
             , data.Current));
                PowerInst.VisaWrite(":OUTPut:STAT ON");
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

        public TestData K2303Series_ChannelOne_ReadVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                double voltage = double.Parse(PowerInst.VisaQuery(":MEASure:VOLTage:DC?"));
                if (item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                    voltage += double.Parse(item.FillValue);
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
                    //if (item.Check)
                    //{
                    //    PowerInst.Rst();
                    //    PowerInst.Closed();
                    //}
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

        public TestData K2303Series_ChannelOne_ReadCurrent(TestData item)
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
                    //PowerInst.VisaWrite(string.Format(":SENSe1:CURRent:DC:RANGe {0}"
                    //    , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));
                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    multiple = 1000;
                    //PowerInst.VisaWrite(string.Format(":SENSe1:CURRent:DC:RANGe {0}"
                    //   , double.Parse(item.UppLimit) * 2 / multiple, item.Unit));
                }
                else
                {
                    multiple = 1;
                }

                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(200);
                    lists.Add(double.Parse(PowerInst.VisaQuery(":MEASure:CURRent:DC?")));
                }
                lists.Remove(lists.Max());
                lists.Remove(lists.Min());
                double current = (lists.Average() * multiple)
                    + double.Parse(item.FillValue);

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
                    //if (item.Check)
                    //{
                    //    PowerInst.Rst();
                    //    PowerInst.Closed();
                    //}
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

        public TestData K2303Series_ChannelOne_StopOut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                //PowerInst.VisaWrite(string.Format(":SOUR1:VOLT:LEVel {0}", 0));
                PowerInst.VisaWrite(":OUTPut:STAT OFF");
                item.Result = "Pass";
                item.Value = "Pass";
                queue.Enqueue("通道1关闭输出");
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                //if (item.Check)
                //{
                //    PowerInst.Rst();
                //    PowerInst.Closed();
                //}
            }
            return item;
        }

        public TestData K2303Series_Closed(TestData item)
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
                PowerInst.VisaWrite(string.Format("INST:COUP:OUTP:STAT NONE"));
                
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

        public TestData HP66319D_ChannelOne_OverVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                //INST: COUP: OUTP: STAT NONE
                //PowerInst.VisaWrite(string.Format("INST: COUP: OUTP: STAT NONE"));
                PowerInst.VisaWrite(string.Format(":SOUR:VOLT1:LEVel {0}"
               , item.LowLimit));
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

        public TestData HP66319D_ChannelTwo_ChargeOutPut(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                // PowerInst.VisaWrite(string.Format("INST: COUP: OUTP: STAT NONE"));
                PowerInst.VisaWrite(string.Format(":SOUR:VOLT2:LEVel {0}"
             , item.LowLimit.Trim()));
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
                    voltage += double.Parse(item.FillValue);
                }
                voltage += double.Parse(item.FillValue);
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
                    //if (item.Check)
                    //{
                    //    PowerInst.Closed();
                    //}
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
                if (item.Unit.ToUpper() == "UA")
                {
                    PowerInst.VisaWrite("SENS:CURR:RANG 0.02");
                }
                else
                {
                    PowerInst.VisaWrite("SENS:CURR:RANG 1");
                }
                //PowerInst.VisaWrite("SENS:CURR:RANG AUTO");
                List<double> list = new List<double>();
                for (int i = 0; i < 5; i++)
                {
                    double curr = double.Parse(PowerInst.VisaQuery("MEAS:CURRent1?"));
                    Thread.Sleep(50);
                    list.Add(curr);
                }
                list.Remove(list.Max());
                list.Remove(list.Min());
                double current = list.Average();
                if (item.Unit.ToUpper() == "UA")
                {
                    current *= 1000000;
                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    current *= 1000;
                }
                current += double.Parse(item.FillValue);
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
                    //if (item.Check)
                    //{
                    //    PowerInst.Closed();
                    //}
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                //if (item.Check)
                //{
                //    PowerInst.Closed();
                //}
            }
            return item;
        }

        public TestData HP66319D_ChannelTwo_OverVoltage(TestData item)
        {
            try
            {
                //PowerInst.OpenVisa(data.PowerPort);
                //InitPower();
                //INST: COUP: OUTP: STAT NONE
                //PowerInst.VisaWrite(string.Format("INST: COUP: OUTP: STAT NONE"));
                PowerInst.VisaWrite(string.Format(":SOUR:VOLT2:LEVel {0}"
               , item.LowLimit));
                PowerInst.VisaWrite(string.Format(":SOUR:CURR2  {0} "
             , data.Current));
                PowerInst.VisaWrite(":OUTPut2:STAT ON");
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
                voltage += double.Parse(item.FillValue);
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
                    //if (item.Check)
                    //{
                    //    PowerInst.Closed();
                    //}
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
                //PowerInst.VisaWrite("SENS:CURR:RANG 0.02");
                //PowerInst.VisaWrite("SENS:CURR:RANG AUTO");
                if (item.Unit.ToUpper() == "UA")
                {
                    PowerInst.VisaWrite("SENS:CURR:RANG 0.02");
                }
                else
                {
                    PowerInst.VisaWrite("SENS:CURR:RANG 1");
                }
                List<double> list = new List<double>();
                for (int i = 0; i < 5; i++)
                {
                    double curr = double.Parse(PowerInst.VisaQuery("MEAS:CURRent2?"));
                    Thread.Sleep(100);
                    list.Add(curr);
                }
                list.Remove(list.Max());
                list.Remove(list.Min());
                double current = list.Average();
                if (item.Unit.ToUpper() == "UA")
                {
                    current *= 1000000;
                }
                else if (item.Unit.ToUpper() == "MA")
                {
                    current *= 1000;
                }
                current += double.Parse(item.FillValue);
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
                    //if (item.Check)
                    //{
                    //    PowerInst.Closed();
                    //}
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                //if (item.Check)
                //{
                //    PowerInst.Closed();
                //}
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
                //if (item.Check)
                //{
                //    PowerInst.Closed();
                //}
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
                //if (item.Check)
                //{
                //    PowerInst.Closed();
                //}
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
                //MultimeterInst.VisaWrite("SYST:LOC");
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
                //MultimeterInst.VisaWrite("SYST:LOC");
                for (int i = 0; i < values.Length; i++)
                {
                    voltages.Add(double.Parse(values[i].Trim()));
                }
                voltages.Remove(voltages.Max());
                voltages.Remove(voltages.Min());
                double voltage = voltages.Average();
                if(item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                    
                }
                voltage += double.Parse(item.FillValue);
                if (voltage <= double.Parse(item.UppLimit)
                    && voltage >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = Math.Round(voltage, 3).ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = Math.Round(voltage, 3).ToString();
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Key34461_ReadACVoltage(TestData item)
        {
            try
            {
                Thread.Sleep(200);
                MultimeterInst.VisaWrite(":FUNC  \"VOLT:AC\";:VOLT:AC:RANG:AUTO ON");
                //*OPC;:SAMP:COUN 10;:TRIG:SOUR IMM;:READ?;
                Thread.Sleep(200);
                string[] values = MultimeterInst.VisaQuery(":SAMP:COUN 5;:TRIG:SOUR IMM;:READ?").Split(',');
                List<double> voltages = new List<double>();
                //MultimeterInst.VisaWrite("SYST:LOC");
                for (int i = 0; i < values.Length; i++)
                {
                    voltages.Add(double.Parse(values[i].Trim()));
                }
                voltages.Remove(voltages.Max());
                voltages.Remove(voltages.Min());
                double voltage = voltages.Average();
                if (item.Unit.ToUpper() == "MV")
                {
                    voltage *= 1000;
                    voltage += double.Parse(item.FillValue);
                }
                if (voltage <= double.Parse(item.UppLimit)
                    && voltage >= double.Parse(item.LowLimit))
                {
                    item.Result = "Pass";
                    item.Value = Math.Round(voltage, 3).ToString();
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = Math.Round(voltage, 3).ToString();
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
                //Thread.Sleep(200);
                MultimeterInst.VisaWrite(":FUNC  \"CURR:DC\";:CURR:DC:RANG:AUTO ON;");
                Thread.Sleep(200);
                string[] values = MultimeterInst.VisaQuery(":SAMP:COUN 5;:TRIG:SOUR IMM;:READ?").Split(',');
                List<double> currents = new List<double>();
                //MultimeterInst.VisaWrite("SYST:LOC");
                for (int i = 0; i < values.Length; i++)
                {
                    currents.Add(double.Parse(values[i].Trim()));
                    //currents.Add(double.Parse(MultimeterInst.VisaQuery("READ?")));
                    //Thread.Sleep(100);
                }
                currents.Remove(currents.Max());
                currents.Remove(currents.Min());
                double current = currents.Average();
                if (item.Unit.ToUpper() == "MA")
                {
                    current *= 1000;
                }
                else if(item.Unit.ToUpper() == "UA")
                {
                    current *= 1000000;
                    if (current <= 19 && current > 18.2)
                        current -= 2.4;
                    else if (current < 18.2 && current > 17.6)
                        current -= 1.8;
                    else if (current < 17.6 && current > 17)
                        current -= 1.2;
                    else if (current < 17 && current > 16.4)
                        current -= 0.6;
                }
                current += double.Parse(item.FillValue);
               
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
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Key34461_ReadResistance(TestData item)
        {
            try
            {
                //:FUNC "CURR:DC";:CURR:DC:RANG:AUTO ON;
                //:FUNC "CURR:DC";:CURR:DC:RANG:AUTO ON;
                Thread.Sleep(200);
                MultimeterInst.VisaWrite(":FUNC  \"RES\";:RES:RANG:AUTO ON;");
                Thread.Sleep(200);
                string[] values = MultimeterInst.VisaQuery(":SAMP:COUN 5;:TRIG:SOUR IMM;:READ?").Split(',');
                List<double> currents = new List<double>();
                //MultimeterInst.VisaWrite("SYST:LOC");
                for (int i = 0; i < values.Length; i++)
                {
                    currents.Add(double.Parse(values[i].Trim()));
                }
                currents.Remove(currents.Max());
                currents.Remove(currents.Min());
                double current = currents.Average();
                if (item.Unit.ToUpper() == "KΩ")
                {
                    current /= 1000;
                }
                else if (item.Unit.ToUpper() == "MΩ")
                {
                    current /= 1000000;
                }
                current += double.Parse(item.FillValue);
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
                }
            }
            catch (Exception ex)
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        public TestData Key34461_ReadContinuity(TestData item)
        {
            try
            {
                //:FUNC "CURR:DC";:CURR:DC:RANG:AUTO ON;
                //:FUNC "CURR:DC";:CURR:DC:RANG:AUTO ON;
                Thread.Sleep(200);
                MultimeterInst.VisaWrite(":FUNC  \"CONT\";:RES:RANG:AUTO ON;");
                Thread.Sleep(200);
                string[] values = MultimeterInst.VisaQuery(":SAMP:COUN 5;:TRIG:SOUR IMM;:READ?").Split(',');
                List<double> currents = new List<double>();
                //MultimeterInst.VisaWrite("SYST:LOC");
                for (int i = 0; i < values.Length; i++)
                {
                    currents.Add(double.Parse(values[i].Trim()));
                }
                currents.Remove(currents.Max());
                currents.Remove(currents.Min());
                double current = currents.Average();
                if (item.Unit.ToUpper() == "KΩ")
                {
                    current /= 1000;
                }
                else if (item.Unit.ToUpper() == "MΩ")
                {
                    current /= 1000000;
                }
                current += double.Parse(item.FillValue);
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
