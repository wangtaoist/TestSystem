using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestEngineAPI;
using TestModel;

namespace TestDAL
{
    public class CsrOperate
    {
        private uint csrHandle = 0;
        public int portNumber;

        public TestData OpenPort(TestData item)
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    //int port = int.Parse(item.Other.Split(':')[1]);
                    //csrHandle = TestEngine.openTestEngineSpiTrans("SPITRANS=USB SPIPORT=0", 0);
                    //csrHandle = TestEngine.openTestEngineSpi(port, 0, 2);
                    csrHandle = TestEngine.openTestEngineSpi(1, 0, 1);
                    if (csrHandle != 0)
                    {
                        item.Result = "Pass";
                        item.Value = "Pass";
                        break;
                    }
                    if(i == 2 && csrHandle ==0)
                    {
                        item.Result = "Fail";
                        item.Value = "Fail";
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
                OpenPort();
                uint lap = 0;
                byte uap = 0;
                ushort nap = 0;
                int ret = TestEngine.psReadBdAddr(csrHandle, out lap, out uap, out nap);
                if (ret == 1)
                {
                    strBdAddress = String.Format("{0:X4}{1:X2}{2:X6}", nap, uap, lap);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return strBdAddress;
        }

        public TestData EnableTestMode(TestData item)
        {
            Thread.Sleep(500);
            int flag = 0;
            if (TestEngine.bccmdEnableDeviceConnect(csrHandle) == 1)
            {
                Thread.Sleep(500);
                flag = TestEngine.bccmdEnableDeviceUnderTestMode(csrHandle);
                if(flag == 0)
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

        public int ColdReset()
        {
            Thread.Sleep(500);
            return TestEngine.bccmdSetColdReset(csrHandle, 500);
        }

        public int WarmReset()
        {
            return TestEngine.bccmdSetWarmReset(csrHandle, 1000);
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

        public int ReadOffset(out short offset)
        {
            short offsetValue = 0;
            int ret = 0;
            ret = TestEngine.psReadXtalOffset(csrHandle, out offsetValue);
            offset = offsetValue;
            return ret;
        }

        public int EnterTxStart(ushort Freq)
        {
            Thread.Sleep(100);
            return TestEngine.radiotestTxstart(csrHandle, Freq, 50, 255, 0);
        }

        public int CalFreq(double Nfreq, double ActFreq, out short offset)
        {
            Thread.Sleep(100);
            short offsetValue = 0;
            int ret = 0;
            ret = TestEngine.radiotestCalcXtalOffset(Nfreq, ActFreq, out offsetValue);
            offset = offsetValue;
            return ret;
        }

        public int WriteCalFreq(short offset)
        {
            Thread.Sleep(100);
            return TestEngine.psWriteXtalOffset(csrHandle, offset);
        }

        public int ReadPsKey(string pskeyno, string values, out string ps)
        {
            Thread.Sleep(100);
            string psk = string.Empty;
            ushort[] val = new ushort[0];
            ushort len = 0;
            int Ref = 0;
            ushort pakey = UInt16.Parse(pskeyno, System.Globalization.NumberStyles.HexNumber);
            TestEngine.psRead(csrHandle, pakey, 0, 0, val, out len);
            if (len == 0)
            {
                psk = "0000";
                Ref = 1;
            }
            else
            {
                ushort[] value = new ushort[len];
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
            uRef = TestEngine.psWrite(csrHandle, ps_key, 1, j, UsrData);
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

        public void ClosedPort()
        {
            try
            {
                TestEngine.closeTestEngine(csrHandle);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
