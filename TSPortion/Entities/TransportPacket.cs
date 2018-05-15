using System;
using System.Collections.Generic;
using System.Text;
using TSPortion.Constants;

namespace TSPortion.Entities
{
    public class TransportPacket
    {
        public byte SyncByte { get; set; }
        public bool TransportErrorIndicator { get; set; }
        public bool PayloadUnitStartIndicator { get; set; }
        public bool TransportPriority { get; set; }
        public ushort PIDType { get; set; }
        public byte TransportScramblingControl { get; set; }
        public byte AdaptationFieldControl { get; set; }
        public int ContinuityCounter { get; set; }

        public TransportPacket(byte[] bytes)
        {
            if (bytes.Length < 5)
            {
                throw new Exception($"There were only {bytes.Length} bytes passed in. 5 are required");
            }

            SyncByte = bytes[0];

            byte indicators = bytes[1]; //01001001 : bits to get 010

            TransportErrorIndicator = ((indicators >> 7) & 1) == 1;
            PayloadUnitStartIndicator = ((indicators >> 6) & 1) == 1;
            TransportPriority = ((indicators >> 5) & 1) == 1;

            ushort bytes2and3 = Converters.BytesToPIDType(bytes[1], bytes[2]);
            ushort maskIndicators = 0b0001111111111111;
            PIDType = (ushort)(bytes2and3 & maskIndicators); //2377 h:949 b:‭100101001001‬

            byte controlsAndCounter = bytes[4];

            TransportScramblingControl = (byte)(controlsAndCounter >> 6);
            AdaptationFieldControl = (byte)((controlsAndCounter >> 4) & 0b00000011);
            ContinuityCounter = (controlsAndCounter & 0b00001111);

        }

        public string GetDebugOutput()
        {
            string output = $"0x{SyncByte:X} PID: 0x{PIDType} Count: {ContinuityCounter}";
            return output;
        }
    }
}
