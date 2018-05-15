using System;
using System.IO;
using TSPortion.Constants;
using TSPortion.Entities;

namespace TSPortion
{
    class Program
    {
        static void Main(string[] args)
        {
            int offset = 0;

            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string path = String.Format("{0}\\Videos\\20180510_1957 FTS-191-.TS", home.ToString());

            if (File.Exists(path))
            {
                Console.WriteLine("Yes");
                Console.WriteLine(OtherConstants.SyncByteValue);

                try
                {
                    FileStream stream = new FileStream(path, FileMode.Open);

                    int streamLengthInBytes = (int)stream.Length;

                    Console.WriteLine("Stream length: {0}MB", Calculations.BytesToMegs(stream.Length));

                    StreamReader reader = new StreamReader(stream);


                    while (!reader.EndOfStream)
                    {
                        int b = reader.Read();

                        if (b == OtherConstants.SyncByteValue)
                        {
                            decimal percentageProgessed = ((decimal)offset / (decimal)streamLengthInBytes) * 100;
                            Console.WriteLine("Offset: {0} / {2} ({3}%) SyncByte {1}", offset, b, streamLengthInBytes, percentageProgessed.ToString("0.0"));

                            int byte2 = reader.Read();
                            int byte3 = reader.Read();
                            int byte4 = reader.Read();
                            int byte5 = reader.Read();
                            //watch out for end of file here. Make safe

                            Console.WriteLine($"dec: {byte2} bin:{Converters.IntByteToBinaryString(byte2)} / dec: {byte3} bin: {Converters.IntByteToBinaryString(byte3)}");
                            ushort _16bit = Converters.BytesToPIDType(byte2, byte3);

                            Console.WriteLine("Bytes: {0}", Converters.IntUShortToBinaryString(_16bit));
                            Console.WriteLine("PID Could be: {0} and in Hex: {1:X}", Converters.IntUShortToBinaryString(Converters.ExtractPIDTypeFromTSPacketUShort(_16bit)), Converters.ExtractPIDTypeFromTSPacketUShort(_16bit));

                            byte[] bytesForPacket = new byte[]
                            {
                                (byte)b,
                                (byte)byte2,
                                (byte)byte2,
                                (byte)byte2,
                                (byte)byte2
                            };

                            TransportPacket packet = new TransportPacket(bytesForPacket);
                            Console.WriteLine(packet.GetDebugOutput());

                            Console.ReadKey();
                        }

                        offset++;
                    }


                    stream.Close();
                }
                catch (FileLoadException fex)
                {
                    Console.WriteLine("File load error: {0}", fex.Message);
                }
                catch (IOException ioex)
                {
                    Console.WriteLine("I/O error: {0}", ioex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generic error: {0}", ex.Message);
                }
                finally
                {

                }


            }

            Console.ReadKey();

        }
    }
}
