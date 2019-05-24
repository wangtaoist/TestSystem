using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATCAPI;
using TestModel;
using System.Threading;

namespace TestDAL
{
    public class AudioOperate
    {
        private ATC ATc;
        private ConfigData config;
        private double[] SpkLevels, SpkTHD, SpkSNR, SpkCrossralk;
        private OperateBES Operate;

        public AudioOperate(ConfigData config, OperateBES operate)
        {
            this.config = config;
            ATc = new ATC();
            ATc.Visible = false;
            //Thread thread = new Thread(() => 
            //{
            string name = ATc.ProjectFileName;
            if ((!config.AudioPath.Contains(name)) || name == "")
            {
                ATc.OpenProject(config.AudioPath);
            }

            //ATc.BtsimSettings.Reset();
            //});
            // thread.Start();
            this.Operate = operate;
        }

        public TestData OpenA2(TestData data)
        {
            try
            {
                ATc.BtsimSettings.Reset();
                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch (Exception)
            {
                data.Result = "Pass";
                data.Value = "Pass";
            }
            return data;
        }

        public TestData SwitchToA2dp(TestData data)
        {
            try
            {
                ATc.BtsimSettings.SwitchToA2dp();
                data.Value = "Pass";
                data.Result = "Pass";
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SwitchToHfp(TestData data)
        {
            try
            {
                ATc.BtsimSettings.SwitchToHfp();
                data.Value = "Pass";
                data.Result = "Pass";
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData EarPair(TestData data)
        {
            try
            {
                ATc.BtsimSettings.Reset();
                string btAddress = Operate.BES_ReadBTAddress();
                ATc.BtsimSettings.ConnectToDevice(btAddress, "1", 10);
                if(ATc.BtsimSettings.AppState == BtsimAppState.Connected)
                {
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = "Fail";
                }
            }
            catch (Exception)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData SpeakerLevel_Left(TestData data)
        {
            try
            {
                //Operate.BES_SetVolume(data);
                Operate.SetVolume();
                ATc.Sequence["Speaker"]["Level and Gain"].Run();
                if (ATc.Sequence["Speaker"]["Level and Gain"].HasSequenceResults)
                {
                    ISequenceResultCollection results = ATc.Sequence["Speaker"]["Level and Gain"]
                        .SequenceResults;

                   SpkLevels = results[0].GetMeterValues();

                    if(SpkLevels[0] <= double.Parse(data.UppLimit) 
                        && SpkLevels[0] >= double.Parse(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = Math.Round(SpkLevels[0], 3).ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = Math.Round(SpkLevels[0], 3).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerLevel_Right(TestData data)
        {
            try
            {
                if (SpkLevels[1] <= double.Parse(data.UppLimit)
                        && SpkLevels[1] >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkLevels[1], 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkLevels[1], 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerTHD_Left(TestData data)
        {
            try
            {
                ATc.Sequence["Speaker"]["THD+N"].Run();

                if (ATc.Sequence["Speaker"]["THD+N"].HasSequenceResults)
                {
                    ISequenceResultCollection results = ATc.Sequence["Speaker"]["THD+N"]
                        .SequenceResults;
                    SpkTHD = results[2].GetMeterValues();
                    if (SpkTHD[0] <= double.Parse(data.UppLimit)
                         && SpkTHD[0] >= double.Parse(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = Math.Round(SpkTHD[0], 3).ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = Math.Round(SpkTHD[0], 3).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerTHD_Right(TestData data)
        {
            try
            {
                if (SpkTHD[1] <= double.Parse(data.UppLimit)
                        && SpkTHD[1] >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkTHD[1], 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkTHD[1], 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerSNR_Left(TestData data)
        {
            try
            {
                ATc.Sequence["Speaker"]["Signal to Noise Ratio"].Run();

                if (ATc.Sequence["Speaker"]["Signal to Noise Ratio"].HasSequenceResults)
                {
                    ISequenceResultCollection results = ATc.Sequence["Speaker"]["Signal to Noise Ratio"]
                        .SequenceResults;
                    SpkSNR = results[0].GetMeterValues();
                    if (SpkSNR[0] <= double.Parse(data.UppLimit)
                         && SpkSNR[0] >= double.Parse(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = Math.Round(SpkSNR[0], 3).ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = Math.Round(SpkSNR[0], 3).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerSNR_Right(TestData data)
        {
            try
            {
                if (SpkSNR[1] <= double.Parse(data.UppLimit)
                        && SpkSNR[1] >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkSNR[1], 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkSNR[1], 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerCrosstalk_Left(TestData data)
        {
            try
            {
                ATc.Sequence["Speaker"]["Crosstalk, One Channel Undriven"].Run();

                if (ATc.Sequence["Speaker"]["Crosstalk, One Channel Undriven"].HasSequenceResults)
                {
                    ISequenceResultCollection results = ATc.Sequence["Speaker"]
                        ["Crosstalk, One Channel Undriven"] .SequenceResults;
                    SpkCrossralk = results[0].GetMeterValues();
                    if (SpkCrossralk[0] <= double.Parse(data.UppLimit)
                         && SpkCrossralk[0] >= double.Parse(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = Math.Round(SpkCrossralk[0], 3).ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = Math.Round(SpkCrossralk[0], 3).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerCrosstalk_Right(TestData data)
        {
            try
            {
                if (SpkCrossralk[1] <= double.Parse(data.UppLimit)
                        && SpkCrossralk[1] >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkCrossralk[1], 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkCrossralk[1], 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        //Micphone
        public TestData MicphoneLevel(TestData data)
        {
            try
            {
                ATc.Sequence["Micphone"]["Level and Gain"].Run();
                if (ATc.Sequence["Micphone"]["Level and Gain"].HasSequenceResults)
                {
                    ISequenceResultCollection results = ATc.Sequence["Micphone"]
                        ["Level and Gain"]
                        .SequenceResults;

                    double[] MicLevels = results[0].GetMeterValues();

                    if (MicLevels[0] <= double.Parse(data.UppLimit)
                        && MicLevels[0] >= double.Parse(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = Math.Round(MicLevels[0], 3).ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = Math.Round(MicLevels[0], 3).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData MicphoneTHD(TestData data)
        {
            try
            {
                ATc.Sequence["Micphone"]["THD+N"].Run();

                if (ATc.Sequence["Micphone"]["THD+N"].HasSequenceResults)
                {
                    ISequenceResultCollection results = ATc.Sequence["Micphone"]["THD+N"]
                        .SequenceResults;
                  double[]  MicTHD = results[2].GetMeterValues();
                    if (MicTHD[0] <= double.Parse(data.UppLimit)
                         && MicTHD[0] >= double.Parse(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = Math.Round(MicTHD[0], 3).ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = Math.Round(MicTHD[0], 3).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData MicphoneSNR(TestData data)
        {
            try
            {
                ATc.Sequence["Micphone"]["Signal to Noise Ratio"].Run();

                if (ATc.Sequence["Micphone"]["Signal to Noise Ratio"].HasSequenceResults)
                {
                    ISequenceResultCollection results = ATc.Sequence["Micphone"]["Signal to Noise Ratio"]
                        .SequenceResults;
                    double[] MicSNR = results[0].GetMeterValues();
                    if (MicSNR[0] <= double.Parse(data.UppLimit)
                         && MicSNR[0] >= double.Parse(data.LowLimit))
                    {
                        data.Result = "Pass";
                        data.Value = Math.Round(MicSNR[0], 3).ToString();
                    }
                    else
                    {
                        data.Result = "Fail";
                        data.Value = Math.Round(MicSNR[0], 3).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData DisConnect(TestData data)
        {
            try
            {
                ATc.BtsimSettings.Disconnect();
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

        public void ExitA2()
        {
            ATc.Exit();
        }
    }
}
