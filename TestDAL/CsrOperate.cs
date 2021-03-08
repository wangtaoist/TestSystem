using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestEngineAPI;
using TestModel;
using csrApi = CsrTestEngineAPI;

namespace TestDAL
{
    public class CsrOperate
    {
        private uint csrHandle = 0;
        public int portNumber;
        //private string CFG_DB_PARAM = "hyd.sdb:CSRA68100_CONFIG";
        //hydracore_config.sdb:QCC512X_CONFIG
        private string CFG_DB_PARAM = "hydracore_config.sdb:QCC512X_CONFIG";
        private UInt32 KEY_READ_BUFFER_LEN = 128;
        private ushort sid, sinkid, tid, operatorId;

        public TestData OpenPort(TestData item)
        {
            try
            {
                Int32 count = 0;
                Thread.Sleep(500);
                string mode = item.Other.Split(':')[0];
                string port = item.Other.Split(':')[1];
                string str = null;
                switch (mode)
                {
                    case "USBDBG":
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                csrHandle = TestEngine.openTestEngine
                                    (256, port, 0, 5000, 0);
                                if (csrHandle != 0)
                                {
                                    int success = 0;
                                    success = TestEngine.teConfigCacheInit(csrHandle, CFG_DB_PARAM);
                                    success = TestEngine.teConfigCacheRead(csrHandle, str, 1);
                                    if (success == 1)
                                    {
                                        item.Result = "Pass";
                                        item.Value = "Pass";
                                    }
                                    else
                                    {
                                        item.Result = "Fail";
                                        item.Value = "Fail";
                                    }
                                    //uint offset = 0;
                                    //ReadOffset(out offset);
                                    break;
                                }
                                if (i == 2 && csrHandle == 0)
                                {
                                    item.Result = "Fail";
                                    item.Value = "Fail";
                                }
                            }
                            break;
                        }
                    case "SPI":
                        {
                            do
                            {
                                count++;
                                csrHandle = TestEngine.openTestEngineSpiTrans
                                    ("SPITRANS=USB SPIPORT=" + port, 0);
                                if (0 != csrHandle)
                                {
                                    int success = 0;
                                    success = TestEngine.teConfigCacheInit(csrHandle, CFG_DB_PARAM);
                                    success = TestEngine.teConfigCacheRead(csrHandle, str, 0);
                                    if (success == 1)
                                    {
                                        item.Result = "Pass";
                                        item.Value = "Pass";
                                    }
                                    else
                                    {
                                        item.Result = "Fail";
                                        item.Value = "Fail";
                                    }
                                    break;
                                }
                                else
                                {
                                    item.Result = "Fail";
                                    item.Value = "Fail";
                                }

                              Thread.Sleep(500);
                            } while (count < 2);

                            break;
                        }
                    case "TRB":
                        {
                            do
                            {
                                count++;
                                csrHandle = TestEngine.openTestEngine
                                   (128, port, 0, 5000, 0);
                                if (0 != csrHandle)
                                {
                                    int success = 0;
                                    success = TestEngine.teConfigCacheInit(csrHandle, CFG_DB_PARAM);
                                    success = TestEngine.teConfigCacheRead(csrHandle, str, 0);
                                    if (success == 1)
                                    {
                                        item.Result = "Pass";
                                        item.Value = "Pass";
                                    }
                                    else
                                    {
                                        item.Result = "Fail";
                                        item.Value = "Fail";
                                    }
                                    break;
                                }
                                else
                                {
                                    item.Result = "Fail";
                                    item.Value = "Fail";
                                }
                                Thread.Sleep(500);
                            } while (count < 2);
                            break;
                        }
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item;
        }

        public void OpenPort()
        {
            try
            {
                if (csrHandle != 0) return;

                for (int i = 0; i < 3; i++)
                {
                    csrHandle = TestEngine.openTestEngineSpi(1, 0, 1);
                    if(csrHandle != 0)
                    {
                        break;
                    }
                }
                
                #region
                //switch (portMode)
                //{
                //    case "Single":
                //        {
                //            for (int i = 0; i < 3; i++)
                //            {
                //                csrHandle = TestEngine.openTestEngineSpiTrans(@"SPITRANS=TRB SPIPORT=1", 0);
                //                if (csrHandle != 0)
                //                {
                //                    break;
                //                }
                //            }
                //            break;

                //        }
                //    case "Multiple":
                //        {
                //            csrHandle = TestEngine.openTestEngineSpi(portNumber, 0, 3);
                //            if (csrHandle != 0)
                //            {
                //                break;
                //            }
                //            break;
                //        }
                //}
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ReadBdaddr()
        {
            string strBdAddress = string.Empty;
            try
            {
                //OpenPort();
                //uint lap = 0;
                //byte uap = 0;
                //ushort nap = 0;
                StringBuilder sb = new StringBuilder(0);
                uint maxLen = 128;
                //int ret = TestEngine.psReadBdAddr(csrHandle, out lap, out uap, out nap);
                int ret = TestEngine.teConfigCacheReadItem(csrHandle, "bt:pskey_bdaddr", sb, out maxLen);
                //{ 0x345678, 0x12, 0xea11}
                string[] bt = sb.ToString().Replace("{", "").Replace("}", "").Trim().Split(','); 

                if (ret == 1)
                {
                    strBdAddress = String.Format("{0:4}{1:2}{2:6}"
                        , bt[2].Trim().Remove(0,2)
                        , bt[1].Trim().Remove(0, 2)
                        , bt[0].Trim().Remove(0, 2));
                }

            }
            catch (Exception)
            {

                throw;
            }
            return strBdAddress.ToUpper();
        }

        public TestData EnableTestMode(TestData item)
        {
            Thread.Sleep(500);
            int flag = 0;
            if (TestEngine.bccmdEnableDeviceConnect(csrHandle) == 1)
            {
                Thread.Sleep(500);
                flag = TestEngine.bccmdEnableDeviceUnderTestMode(csrHandle);
                if(flag == 1)
                {
                    item.Result = "Pass";
                    item.Value = "Pass";
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = "Fail";
                }
            }
            return item;
        }

        public TestData ReadBTaddress(TestData item)
        {
            try
            {
                item.Value = ReadBdaddr();
                item.Result = "Pass";
            }
            catch (Exception)
            {
                item.Value = "Fail";
                item.Result = "Fail";
            }
            return item;
        }

        public TestData StartAudioLoop(TestData item)
        {
            try
            {
                sid = 0;
                sinkid = 0;
                operatorId = 0;
                tid = 0;
                if (TestEngine.teAudioGetSource(csrHandle, 3, 0, 0, out sid) == 1)
                {
                    if (TestEngine.teAudioGetSink(csrHandle, 3, 0, 0, out sinkid) == 1)
                    {
                        if (TestEngine.teAudioMicBiasConfigure(csrHandle
                            , 0, 0, 1) == 1)
                        {
                            if (TestEngine.teAudioConnect(csrHandle, sid, sinkid, out tid) == 1)
                            {
                                item.Value = "Pass";
                                item.Result = "Pass";
                            }
                            else
                            {
                                item.Value = "Fail";
                                item.Result = "Fail";
                            }
                        }
                        else
                        {
                            item.Value = "Fail";
                            item.Result = "Fail";
                        }
                    }
                    else
                    {
                        item.Value = "Fail";
                        item.Result = "Fail";
                    }
                }
                else
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }
            }
            catch (Exception ex)
            {
                item.Value = "Fail";
                item.Result = "Fail";
            }
            return item;
        }

        public TestData StopAudioLoop(TestData item)
        {
            try
            {
                sid = 0;
                sinkid = 0;
                operatorId = 0;
                tid = 0;
                if (TestEngine.teAudioDisconnect(csrHandle, tid) == 1)
                {
                    if (TestEngine.teAudioCloseSource(csrHandle, sid) == 1)
                    {
                        if (TestEngine.teAudioCloseSink(csrHandle, sinkid) == 1)
                        {
                            item.Value = "Pass";
                            item.Result = "Pass";
                        }
                        else
                        {
                            item.Value = "Fail";
                            item.Result = "Fail";
                        }
                    }
                }
                else
                {
                    item.Value = "Fail";
                    item.Result = "Fail";
                }

            }
            catch (Exception ex)
            {
                item.Value = "Fail";
                item.Result = "Fail";
            }
            return item;
        }

        public int ColdReset()
        {
            Thread.Sleep(500);
            return csrApi.TestEngine.bccmdSetColdReset(csrHandle, 500);
        }

        public int WarmReset()
        {
            return TestEngine.bccmdSetWarmReset(csrHandle, 1000);
        }

        public int ChipReset()
        {
            return TestEngine.teChipReset(csrHandle, 0);
        }

        public int EnableConnect()
        {
            Thread.Sleep(500);
            return TestEngine.bccmdEnableDeviceConnect(csrHandle);
        }

        public TestData ReadOffset(TestData item)
        {
            short offsetValue = 0;
            int ret = 0;
            ret = TestEngine.psReadXtalOffset(csrHandle, out offsetValue);
           if(ret <= short.Parse(item.UppLimit) && ret >= short.Parse(item.LowLimit))
            {
                item.Value = ret.ToString();
                item.Result = "Pass";
            }
           else
            {
                item.Value = ret.ToString();
                item.Result = "Fail";
            }
            return item;
        }

        public uint ReadOffset(out uint offset)
        {
            int success = 0;
            string str = null;
            //Thread.Sleep(1000);
            // success = TestEngine.teConfigCacheInit(csrHandle, "hydracore_config.sdb:QCC512X_CONFIG");
            //success = TestEngine.teConfigCacheRead(csrHandle, str, 0);
           
            //short offsetValue = 0;
            uint ret = 0;
            //ret = TestEngine.psReadXtalOffset(csrHandle, out offsetValue);
            ////ret = TestEngine.psReadXtalFtrim(csrHandle, out offsetValue);
            StringBuilder valueString = new StringBuilder();
            UInt32 maxLen = 128;
            success = TestEngine.teConfigCacheReadItem(csrHandle, "curator:XtalFreqTrim", valueString, out maxLen);
            offset = Convert.ToUInt16(valueString.ToString());
            success = TestEngine.teConfigCacheReadItem(csrHandle, "curator:XtalLoadCapacitance", valueString, out maxLen);
            ret = Convert.ToUInt16(valueString.ToString(),16);
            //success = TestEngine.teConfigCacheWriteItem(csrHandle, "curator15:XtalFreqTrim", "0");
            //success = TestEngine.teConfigCacheWriteItem(csrHandle, "curator15: XtalLoadCapacitance", "17");
            //success = TestEngine.teConfigCacheWrite(csrHandle, str, 0);
            //TestEngine.bccmdSetWarmReset(csrHandle, 100);

            // success = TestEngine.teConfigCacheReadItem(csrHandle, "curator:XtalFreqTrim", valueString, out maxLen);
            //success = TestEngine.teConfigCacheReadItem(csrHandle, "curator:XtalLoadCapacitance", valueString, out maxLen);
            return ret;
        }

        public int AdjustFreq(uint xtal,uint Cap)
        {
            int success = 0;
            string str = null;
            success = TestEngine.teConfigCacheWriteItem(csrHandle
                , "curator15:XtalFreqTrim", xtal.ToString());
            success = TestEngine.teConfigCacheWriteItem(csrHandle
                , "curator15: XtalLoadCapacitance", Cap.ToString());
            success = TestEngine.teConfigCacheWrite(csrHandle, str, 0);
            return success;
        }

        public int WriteBtAddress(string bt)
        {
            int success = 0;
            string str = null;
            string strInput = "{0x" + bt.Substring(6, 6) + ",0x"
                + bt.Substring(4, 2) + ",0x" + bt.Substring(0, 4) + "}";
            //OpenPort(new TestData());
            success = TestEngine.teConfigCacheWriteItem(csrHandle
                , "bt15:PSKEY_BDADDR", strInput);
            success = TestEngine.teConfigCacheWrite(csrHandle, str, 0);
            return success;
        }

        public int EnterTxStart(ushort Freq)
        {
            Thread.Sleep(100);
            int ret = TestEngine.radiotestTxstart(csrHandle, Freq, 50, 255, 0);
            return ret;
        }

        public int CalFreq(double Nfreq, double ActFreq, out short offset)
        {
            Thread.Sleep(100);
            short offsetValue = 0;
            int ret = 0;
            ret = csrApi.TestEngine.radiotestCalcXtalOffset(Nfreq, ActFreq, out offsetValue);
            offset = offsetValue;
            return ret;
        }

        public int WriteCalFreq(short offset)
        {
            Thread.Sleep(100);
            int ret = csrApi.TestEngine.psWriteXtalOffset(csrHandle, offset);
            return ret;
        }

        public int ReadPsKey(string pskeyno, string values, out string ps)
        {
            //0000ff09005b0002
            Thread.Sleep(100);
            string psk = string.Empty;
            ushort[] val = new ushort[4];
            ushort len = 0;
            int Ref = 0;
            ushort pakey = UInt16.Parse(pskeyno, System.Globalization.NumberStyles.HexNumber);
            TestEngine.psSize(csrHandle, 1, 0, out len);
            Ref = TestEngine.psRead(csrHandle, pakey, 0, 4, val, out len);
            ushort[] value = new ushort[len];
            Ref = TestEngine.psRead(csrHandle, pakey, 0, len, value, out len);
            //int ret = TestEngine.psRead(csrHandle, pakey, 0, len, PSKeyArray, out len);
            //int ret = TestEngine.tePsRead(csrHandle, pakey, 64, val, out len);
            if (len == 0)
            {
                psk = "0000";
                Ref = 1;
            }
            else
            {
                //ushort[] value = new ushort[len];
                Ref = TestEngine.psRead(csrHandle, pakey, 0, len, value, out len);
                if (Ref == 1)
                {
                    for (int i = 0; i < len; i++)
                    {
                        psk += System.Convert.ToString(value[i], 16).PadLeft(4, '0'); ;
                    }

                }
            }
            ps = psk;
            return Ref;
        }

        public int WritePsKey(string pskeyno, string values)
        {
            Thread.Sleep(100);
            Int32 uRef = 0;
            UInt16 ps_key = 0;
            UInt16[] UsrData = new ushort[values.Length / 4];
            UInt16 i = 0;
            UInt16 j = 0;
            ps_key = UInt16.Parse(pskeyno, System.Globalization.NumberStyles.HexNumber);
            for (i = 0, j = 0; i < values.Length; i += 4, j++)
            {
                UsrData[j] = UInt16.Parse(values.Substring(i * 1, 4), System.Globalization.NumberStyles.HexNumber);
            }
            //uRef = TestEngine.psWrite(csrHandle, ps_key, 1, j, UsrData);
            uRef = TestEngine.psWrite(csrHandle, ps_key, 1,3
                , new ushort[] { 123,234,345});
            if (uRef == 1)
            {
                String strTmp = "";
                UInt16[] ReadStrArray = new UInt16[256];
                UInt16[] len = new UInt16[1];
                Int32 iRtn = TestEngine.psRead(csrHandle, ps_key, 1, 256, ReadStrArray, out (len[0]));
                if (iRtn == 1)
                {
                    for (i = 0; i < len[0]; i++)
                    {
                        strTmp += (System.Convert.ToString(ReadStrArray[i], 16)).PadLeft(4, '0');
                    }

                }
            }
            return uRef;
        }

        public void QCCClosedDev()
        {
            Thread.Sleep(500);
            if (csrHandle != 0)
            {
                TestEngine.closeTestEngine(csrHandle);
            }
            csrHandle = 0;
        }

        public TestData OpenCsrDev(TestData item)
        {
            Int32 count = 0;
            Thread.Sleep(500);
            string port = item.Other.Split(':')[1];
            string openduttype = item.Other.Split(':')[0];
            switch (openduttype)
            {
                case "USB":
                    do
                    {
                        count++;
                        csrHandle = csrApi.TestEngine.openTestEngine(2, @"\\.\CSR0", 0, 0, 250);
                        if (0 != csrHandle)
                        {
                            item.Result = "Pass";
                            item.Value = "Pass";
                            break;
                        }
                        Thread.Sleep(500);
                    } while (count < 2);
                    break;
                case "SPI":
                    do
                    {
                        count++;
                        csrHandle = csrApi.TestEngine.openTestEngineSpi(1, 0, 1);
                        if (0 != csrHandle)
                        {
                            item.Result = "Pass";
                            item.Value = "Pass";
                            break;
                        }
                        Thread.Sleep(500);
                    } while (count < 2);
                    break;
                case "UART":
                    do
                    {
                        count++;
                        csrHandle = csrApi.TestEngine.openTestEngine(1, "\\\\.\\" + "", 38400, 5000, 0);
                        if (0 != csrHandle)
                        {
                            item.Result = "Pass";
                            item.Value = "Pass";
                            break;
                        }
                        Thread.Sleep(500);
                    } while (count < 2);
                    break;
                case "USB-SPI":
                    do
                    {
                        count++;
                        csrHandle = csrApi.TestEngine.openTestEngineSpiTrans("SPITRANS=USB SPIPORT=0", 0);
                        if (0 != csrHandle)
                        {
                            item.Result = "Pass";
                            item.Value = "Pass";
                            break;
                        }
                        Thread.Sleep(500);
                    } while (count < 2);
                    break;
                case "USB-LPT":
                    do
                    {
                        count++;
                        csrHandle = TestEngine.openTestEngineSpiTrans("SPITRANS=LPT SPIPORT=1", 0);
                        if (0 != csrHandle)
                        {
                            item.Result = "Pass";
                            item.Value = "Pass";
                            break;
                        }
                        Thread.Sleep(500);
                    } while (count < 2);
                    break; 
            }
            if (item.Result == null || item.Result == "")
            {
                item.Result = "Fail";
                item.Value = "Fail";
            }
            return item;
        }

        private void OpenCsrDev()
        {
            string item = "USB-SPI:0";
            Int32 count = 0;
            Thread.Sleep(500);
            string port = item.Split(':')[1];
            string openduttype = item.Split(':')[0];
            switch (openduttype)
            {
                case "USB-SPI":
                    do
                    {
                        count++;
                        if(csrHandle != 0)
                        {
                            csrApi.TestEngine.closeTestEngine(csrHandle);
                            csrHandle = 0;
                        }
                        csrHandle = csrApi.TestEngine.openTestEngineSpiTrans("SPITRANS=USB SPIPORT=0", 0);
                        if (0 != csrHandle)
                        {
                           
                            break;
                        }
                        Thread.Sleep(500);
                    } while (count < 2);
                    break;
               
            }
        }

        public TestData ReadCsrBDAddress(TestData item)
        {
            Boolean Flag = true;
            string strBdAddress = "";
            if (0 != csrHandle)
            {
                UInt32 ReadBdLap = 0;
                Byte ReadBdUap = 0;
                UInt16 ReadBdNap = 0;

                Int32 uRef = 0;

                uRef = csrApi.TestEngine.psReadBdAddr(csrHandle, out (ReadBdLap), out (ReadBdUap), out (ReadBdNap));
                if (uRef == 1)
                {
                    strBdAddress = String.Format("{0:X4}{1:X2}{2:X6}", ReadBdNap, ReadBdUap, ReadBdLap);
                    item.Result = "Pass";
                    item.Value = strBdAddress;
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = "Fail";
                    Flag = false;
                }
            }
            else
            {
                Flag = false;
            }
            return item;
        }

        public TestData CsrEnableTestMode(TestData item)
        {
            Thread.Sleep(500);
            int flag = 0;
            if (csrApi.TestEngine.bccmdEnableDeviceConnect(csrHandle) == 1)
            {
                Thread.Sleep(500);
                flag = csrApi.TestEngine.bccmdEnableDeviceUnderTestMode(csrHandle);
                if(flag == 1)
                {
                    item.Result = "Pass";
                    item.Value = "Pass";
                }
                else
                {
                    item.Result = "Fail";
                    item.Value = "Fail";
                }
            }
            return item;
        }

        public int CsrEnterTxStart(ushort Freq)
        {
            Thread.Sleep(100);
            int ret = csrApi.TestEngine.radiotestTxstart(csrHandle, Freq, 50, 255, 0);
            return ret;
        }

        public int CsrReadOffset(out short offset)
        {
            short offsetValue = 0;
            int ret = 0;
            ret = csrApi.TestEngine.psReadXtalOffset(csrHandle, out offsetValue);
            offset = offsetValue;
            return ret;
        }

        public int CsrColdReset()
        {
            Thread.Sleep(500);
            return csrApi.TestEngine.bccmdSetColdReset(csrHandle, 500);
        }

        public int WriteCsrBtAddress(string bt)
        {
            int success = 0;
            string str = null;
            uint lap = uint.Parse(bt.Substring(6, 6)
                ,System.Globalization.NumberStyles.AllowHexSpecifier);
           uint uap = uint.Parse(bt.Substring(4, 2)
               , System.Globalization.NumberStyles.AllowHexSpecifier);
            uint nap = uint.Parse(bt.Substring(0, 4)
                , System.Globalization.NumberStyles.AllowHexSpecifier);
            //OpenPort(new TestData());
            success = csrApi.TestEngine.psWriteBdAddr(csrHandle, lap, uap, nap);
            //csrApi.TestEngine.closeTestEngine(csrHandle);
            ResetConnCsr();
            //OpenCsrDev();
            return success;
        }

        public int CsrWarmReset()
        {
            return csrApi.TestEngine.bccmdSetWarmReset(csrHandle, 1000);
        }

        public void ResetConnCsr()
        {
            csrApi.TestEngine.closeTestEngine(csrHandle);
            OpenCsrDev();
        }

        public TestData CsrClosedPort(TestData item)
        {
            try
            {
                int val = csrApi.TestEngine.closeTestEngine(csrHandle);
                if (val != 1)
                {
                    item.Result = "Fail";
                    item.Value = "Fail";
                }
                else
                {
                    item.Result = "Pass";
                    item.Value = "Pass";
                }
            }
            catch (Exception)
            {
                item.Result = "Fail";
                item.Value = "Fail";
                throw;
            }
            return item;
        }
    }
}
