using System;
using System.Collections.Generic;
using System.Text;

namespace TSPortion.Constants
{

    public static class OtherConstants
    {
        public const byte SyncByteValue = 0x47;
    }

    public static class Calculations
    {
        public static int BytesToMegs(decimal numberOfBytes)
        {
            return (int)numberOfBytes / 1024 / 1024;
        }
    }

    public static class Converters {
        public static string IntByteToBinaryString(int byt)
        {
            return Convert.ToString(byt, 2).PadLeft(8, '0');
        }

        public static string IntShortToBinaryString(short bytes)
        {
            return Convert.ToString(bytes, 2).PadLeft(16, '0');
        }

        public static short ExtractPIDTypeFromTSPacketShort(short bytes)
        {
            byte[] bytes16 = BitConverter.GetBytes(bytes);
            int shift = (int)bytes16[0] << 3;
            shift = shift >> 3;
            bytes16[0] = (byte)shift;

            return BitConverter.ToInt16(bytes16, 0);

        }

        public static short BytesToPIDType(int byte1, int byte2)
        {
            byte[] bytes = new byte[]
            {
                (byte)byte1,
                (byte)byte2
            };

            short _16BitShort = BitConverter.ToInt16(bytes, 0);
            return _16BitShort;
        }

    }


}
