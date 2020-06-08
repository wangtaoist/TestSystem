using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATCAPI;
using TestModel;
using System.Threading;
using System.IO;

namespace TestDAL
{
    public class AudioOperate
    {
        private ATC ATc;
        private ConfigData config;
        private double[] SpkLevels, SpkTHD, SpkSNR, SpkCrossralk, LowNoise;
        private OperateBES Operate;
        public string btAddress;
        private bool btStatus;

        public AudioOperate(ConfigData config, OperateBES operate)
        {
            try
            {
                this.config = config;
                btAddress = string.Empty;
                ATc = new ATC();
                ATc.Visible = false;
                //Thread thread = new Thread(() => 
                //{
                if (File.Exists(config.AudioPath))
                {
                    string name = ATc.ProjectFileName;
                    if ((!config.AudioPath.Contains(name)) || name == "")
                    {
                        ATc.OpenProject(config.AudioPath);
                    }
                }
                //ATc.BtsimSettings.Reset();
                //});
                // thread.Start();
                this.Operate = operate;
                btStatus = true;
            }
            catch (Exception)
            {

            }
            
        }

        public TestData OpenA2(TestData data)
        {
            try
            {
                //ATc.BtsimSettings.CallCancel();
                OutputConnectorType type = ATc.SignalPathSetup.OutputConnector.Type;
                
                InputConnectorType ty = ATc.SignalPathSetup.InputConnector.Type;
                if (type == OutputConnectorType.Btsim || ty == InputConnectorType.Btsim)
                {
                    ATc.BtsimSettings.Reset();
                    data.Result = "Pass";
                    data.Value = "Pass";
                    btStatus = true;
                }
                else
                {
                    btStatus = false;

                    //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.Bluetooth;
                    //ATc.BluetoothSettings.ProfileSet = BluetoothProfileSet.A2dpSourceHfpGatewayAvrcp;

                    ATc.BluetoothSettings.InquiryTimeout = 5;
                    ATc.BluetoothSettings.ClearDeviceList();
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.Btsim;
                //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.AnalogUnbalanced;
                //ATc.BtsimSettings.Reset();
                //string name = ATc.BtsimSettings.BtsimName;

                //ATc.BluetoothSettings.ClearDeviceList();
                //ATc.BluetoothSettings.ProfileSet = BluetoothProfileSet.A2dpSourceHfpGatewayAvrcp;
                //ATc.BluetoothSettings.InquiryTimeout = 5;
               
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData SwitchToA2dp(TestData data)
        {
            try
            {
                if (btStatus)
                {
                    //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.Btsim;
                    //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.AnalogBalanced;
                    ATc.BtsimSettings.SwitchToA2dp();

                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    ATc.BluetoothSettings.ConnectA2dp(btAddress);
                    if (ATc.BluetoothSettings.AcceptA2dpConnections)
                    {
                        data.Value = "Pass";
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = "Fail";
                        data.Result = "Fail";
                    }
                }
                //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.Bluetooth;
                //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.AnalogBalanced;

                //ATc.BluetoothSettings.ConnectA2dp(btAddress);

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
                if (btStatus)
                {
                    //ATc.BtsimSettings.SwitchToHfp();
                    //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.AnalogUnbalanced;
                    //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.Btsim;
                    ATc.BtsimSettings.SwitchToHfp();
                }
                else
                {
                    //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.Bluetooth;
                    //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.AnalogBalanced;
                    ATc.BluetoothSettings.ConnectHfp(btAddress);
                    ATc.BluetoothSettings.HfpAudioGatewayCommand(BluetoothHfpAgCommand.OpenSco);
                }
                //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.AnalogUnbalanced;
                //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.Bluetooth;
                //ATc.BluetoothSettings.ConnectHfp(btAddress);
                //ATc.BluetoothSettings.HfpAudioGatewayCommand(BluetoothHfpAgCommand.OpenSco);
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
            btAddress = Operate.BES_ReadBTAddress();
            for (int i = 0; i < 3; i++)
            {
                //ATc.BtsimSettings.Reset();

                if (btStatus)
                {
                    try
                    {
                        ATc.BtsimSettings.Reset();
                        ATc.BtsimSettings.ConnectToDevice(btAddress, "1", 10);
                        if (ATc.BtsimSettings.AppState == BtsimAppState.Connected)
                        //if(conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    try
                    {
                        bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                        if (conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }

                    }
                    //bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            return data;
        }
          
        public TestData CSR_EarPair(TestData data)
        {

            for (int i = 0; i < 3; i++)
            {
                //ATc.BtsimSettings.Reset();
                //btAddress = Operate.BES_ReadBTAddress();
                if (btStatus)
                {
                    //IBtsimDeviceCollection lists = ATc.BtsimSettings.ScanForDevices(5);

                    //Dictionary<string, double> dic = new Dictionary<string, double>();
                    //for (int i = 0; i < lists.Count; i++)
                    //{
                    //    dic.Add(lists[i].Address, double.Parse(lists[i].Rssi));
                    //}
                    //dic
                    try
                    {
                        ATc.BtsimSettings.Reset();
                        ATc.BtsimSettings.ConnectToDevice(btAddress, "1", 10);
                        if (ATc.BtsimSettings.AppState == BtsimAppState.Connected)
                        //if(conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    //IBluetoothDeviceCollection lists = ATc.BluetoothSettings.ScanForDevices(5);
                    //Dictionary<string, double> dic = new Dictionary<string, double>();
                    //for (int i = 0; i < lists.Count; i++)
                    //{
                    //    dic.Add(lists[i].Address, double.Parse(lists[i].Rssi));
                    //}
                    try
                    {
                        bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                        if (conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }

                    }
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                    //bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                }
            }

            return data;
        }

        public TestData SpeakerLevel_Left(TestData data)
        {
            try
            {
                //Operate.BES_SetVolume(data);
                for (int i = 0; i < 3; i++)
                {
                    //Operate.SetVolume();
                    ATc.Sequence["Speaker"]["Level and Gain"].Run();
                   
                    if (ATc.Sequence["Speaker"]["Level and Gain"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]["Level and Gain"]
                            .SequenceResults;

                        SpkLevels = results[0].GetMeterValues();

                        if (SpkLevels[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                            && SpkLevels[0] + double.Parse(data.FillValue)
                            >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(SpkLevels[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(SpkLevels[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                        }
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
                if (SpkLevels[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && SpkLevels[1] + double.Parse(data.FillValue) 
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkLevels[1] 
                        + double.Parse(data.FillValue), 3).ToString();
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
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["THD+N"].Run();
                    if (ATc.Sequence["Speaker"]["THD+N"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]["THD+N"]
                            .SequenceResults;
                        SpkTHD = results[2].GetMeterValues();
                        if (SpkTHD[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && SpkTHD[0] + double.Parse(data.FillValue) 
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(SpkTHD[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(SpkTHD[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                        }
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
                if (SpkTHD[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && SpkTHD[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkTHD[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkTHD[1]
                         + double.Parse(data.FillValue), 3).ToString();
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
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["Signal to Noise Ratio"].Run();

                    if (ATc.Sequence["Speaker"]["Signal to Noise Ratio"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]["Signal to Noise Ratio"]
                            .SequenceResults;
                        SpkSNR = results[0].GetMeterValues();
                        if (SpkSNR[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && SpkSNR[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(SpkSNR[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(SpkSNR[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
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
                if (SpkSNR[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && SpkSNR[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkSNR[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkSNR[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerNoise_Left(TestData data)
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["THD+N"].Run();

                    if (ATc.Sequence["Speaker"]["THD+N"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]["THD+N"]
                            .SequenceResults;
                        LowNoise = results[7].GetMeterValues();
                        if (LowNoise[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && LowNoise[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(LowNoise[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(LowNoise[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
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

        public TestData SpeakerNoise_Right(TestData data)
        {
            try
            {
                if (LowNoise[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && LowNoise[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(LowNoise[1] 
                        + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(LowNoise[1]
                         + double.Parse(data.FillValue), 3).ToString();
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
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["Crosstalk, One Channel Undriven"].Run();

                    if (ATc.Sequence["Speaker"]["Crosstalk, One Channel Undriven"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]
                            ["Crosstalk, One Channel Undriven"].SequenceResults;
                        SpkCrossralk = results[0].GetMeterValues();
                        if (SpkCrossralk[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && SpkCrossralk[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(SpkCrossralk[0]
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(SpkCrossralk[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
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
                if (SpkCrossralk[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && SpkCrossralk[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkCrossralk[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkCrossralk[1]
                         + double.Parse(data.FillValue), 3).ToString();
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
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Micphone"]["Level and Gain"].Run();
                    if (ATc.Sequence["Micphone"]["Level and Gain"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Micphone"]
                            ["Level and Gain"].SequenceResults;

                        double[] MicLevels = results[0].GetMeterValues();

                        if (MicLevels[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                            && MicLevels[0] + double.Parse(data.FillValue)
                            >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(MicLevels[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(MicLevels[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
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
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Micphone"]["THD+N"].Run();

                    if (ATc.Sequence["Micphone"]["THD+N"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Micphone"]["THD+N"]
                            .SequenceResults;
                        double[] MicTHD = results[2].GetMeterValues();
                        if (MicTHD[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && MicTHD[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(MicTHD[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(MicTHD[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
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
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Micphone"]["Signal to Noise Ratio"].Run();

                    if (ATc.Sequence["Micphone"]["Signal to Noise Ratio"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Micphone"]["Signal to Noise Ratio"]
                            .SequenceResults;
                        double[] MicSNR = results[0].GetMeterValues();
                        if (MicSNR[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && MicSNR[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(MicSNR[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(MicSNR[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
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
                if(btStatus)
                {
                    ATc.BtsimSettings.Disconnect();
                }
                else
                {
                    ATc.BluetoothSettings.Disconnect();
                }
                
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
