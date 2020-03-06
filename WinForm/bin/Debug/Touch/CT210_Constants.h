#ifndef __CT210_CONSTANTS_H
#define __CT210_CONSTANTS_H

class CT210_Constants
{
    public:

        // ICType Constants
        static const int ICType_I2C             = 4;

        // CommsType Constants
        static const int CommsType_SPI          = 3;
        static const int CommsType_I2C          = 4;
        static const int CommsType_I2C_poll     = 9;

        // CommsSpeed Constants
        static const int CommsSpeed_Fast    = 1;    //400kHz
        static const int CommsSpeed_Medium  = 2;  // 100kHz
        static const int CommsSpeed_Slow    = 3;
        static const int CommsSpeed_Custom  = 4;  // I2C custom baud rate

        // Voltage Constants
        static constexpr float Voltage_3V3 = 3.3f;
        static constexpr float Voltage_5V0 = 5.0f;

        // I2CType Constants
        static const int I2CType_NOReady            = 1;
        static const int I2CType_Ready_Active_High  = 2;
        static const int I2CType_Ready_Active_Low   = 3;
        static const int I2CType_SDA_Sync           = 4;

        // I2CPullUp Constants
        static const int I2CPullUps_Disabled = 1;
        static const int I2CPullUps_Enabled  = 2;

        // I2CRdyPin Constants
        static const int I2CRdyPin_Pin6  = 1;
        static const int I2CRdyPin_Pin8  = 2;
        static const int I2CRdyPin_Pin10 = 3;

        // I2CStop Constants
        static const int I2CStop_NoStop  = 1;
        static const int I2CStop_UseStop = 2;

        // I2CPoll Constants
        static const int I2CPolling_Disabled = 1;
        static const int I2CPolling_Enabled  = 2;

        // I2CFastLevelshifters Constants
        static const int I2CFastLevelshifters_Disabled  = 0;
        static const int I2CFastLevelshifters_Enabled   = 1;

        //I2CPortNum Constants
        static const int I2CPortNum_1   = 1;
        static const int I2CPortNum_2   = 0;

        // I2CAddressSize Constants
        static const int I2CAddrSize_8bit   = 0;
        static const int I2CAddrSize_16bit  = 1;
};

#endif // __CT210_CONSTANTS_H