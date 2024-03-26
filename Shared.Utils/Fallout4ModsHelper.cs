using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Shared.Utils
{

    public static class Fallout4ModsHelper
    {
        public const int MaxStringLength = 511;
        private const int ESLFlag = 0x00000200;

        #region ESP file struct
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct ESPHeaderIntro
        {
            [FieldOffset(0)]
            private int Signature;
            [FieldOffset(4)]
            private uint Length;
            [FieldOffset(8)]
            private int Flags;
            [FieldOffset(12)]
            private int Unk1;
            [FieldOffset(16)]
            private int Unk2;

            [FieldOffset(20)]
            private int FormVersion;

            [FieldOffset(24)]
            private int HEDRSignature;
            [FieldOffset(28)]
            private ushort HEDRLength; // 000C
            [FieldOffset(30)] 
            private float FileVersion;
            [FieldOffset(34)]
            private int NumberOfRecords; // 0
            [FieldOffset(38)]
            private int NextObjectId; // 0x800

            // header records:
            // CNAME of creator
            // SNAME with summary
            // INTV for idk


            private static readonly uint ExtraLength = (uint)Marshal.SizeOf<ESPHeaderIntro>() - (uint)Marshal.OffsetOf<ESPHeaderIntro>(nameof(HEDRSignature));

            public static ESPHeaderIntro New(int dataSize) => new ESPHeaderIntro
            {
                Signature = 0x34534554, // TES4
                Length = (uint)dataSize + ExtraLength, 
                Flags = ESLFlag,
                FormVersion = 0x83, // 131,
                HEDRSignature = 0x52444548, // HEDR
                HEDRLength = 0x000C,
                FileVersion = 1.0f,
                NumberOfRecords = 0,
                NextObjectId = 0x800,
            };
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct STRING
        {
            private int Signature; // CNAM/SNAM
            private ushort Length;
            private byte[] Value; // including tailing \0

            public static STRING CNAM(string value)
            {
                STRING retVal = new STRING{Signature = 0x4D414E43};
                SetValue(ref retVal, value);
                return retVal;
            }

            private static void SetValue(ref STRING retVal, string value)
            {
                var data = Encoding.UTF8.GetBytes(value);
                retVal.Value = new byte[data.Length + 1]; // last byte is initialized by 0
                Array.Copy(data,retVal.Value,data.Length);
                retVal.Length = (ushort)(retVal.Value.Length);
            }

            public static STRING SNAM(string value)
            {
                STRING retVal = new STRING{Signature = 0x4D414E53};
                SetValue(ref retVal, value);
                return retVal;
            }

            public byte[] ToArray()
            {
                byte[] retVal = new byte [4 + 2 + Value.Length]; // sizeof(Signature) + sizeof(Length)
                var sig = BitConverter.GetBytes(Signature);
                Array.Copy(sig, retVal, sig.Length);
                var len = BitConverter.GetBytes(Length);
                Array.Copy(len, 0, retVal, sig.Length, len.Length);
                Array.Copy(Value,0, retVal, sig.Length + len.Length, Value.Length);
                return retVal;
            }
        }

        [StructLayout(LayoutKind.Sequential,Pack = 1)]
        struct INTV
        {
            private int Signature; // INTV
            private ushort Length; // 4
            private int Value;

            public static INTV FromVal(int value)
            {
                return new INTV { Signature = 0x56544E49, Length = 4, Value = value };
            }
        }
        #endregion

        private static byte[] ToBytes<T>(this T data) where T : struct
        {
            var rawData = new byte[Marshal.SizeOf(data)];
            var handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);

            try
            {
                Marshal.StructureToPtr(data, handle.AddrOfPinnedObject(), false);
            }
            finally
            {
                handle.Free();
            }

            return rawData;
        }

        private static void Write(this MemoryStream ms, byte[] data)
        {
            ms.Write(data, 0, data.Length);
        }

        public static byte[] GenerateEmptyESL(string author, string description)
        {
            var creator = STRING.CNAM(author).ToArray();
            var summary = STRING.SNAM(description).ToArray();
            var unkInt = INTV.FromVal(1).ToBytes();
            using (MemoryStream ms = new MemoryStream())
            {
                var header = ESPHeaderIntro.New(creator.Length + summary.Length + unkInt.Length);
                ms.Write(header.ToBytes());
                ms.Write(creator);
                ms.Write(summary);
                ms.Write(unkInt);
                var retVal = ms.ToArray();
                return retVal;
            }
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        private struct ShortHeader
        {
            [FieldOffset(0)]
            private int Signature;
            [FieldOffset(4)]
            private uint Length;
            [FieldOffset(8)]
            public int Flags;

            public static readonly int HeaderLength = Marshal.SizeOf<ShortHeader>();
        }

        private static T ToStruct<T>(this byte[] bytes)
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                T retVal = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
                return retVal;
            }
            finally
            {
                handle.Free();
            }
        }

        public static bool IsESLFile(this string path, out bool found)
        {
            found = false;
            if (!File.Exists(path))
                return false;

            FileStream fs;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            byte[] buffer = new byte[ShortHeader.HeaderLength];
            int readBytes = fs.Read(buffer, 0, buffer.Length);
            if (readBytes != buffer.Length)
            {
                fs.Close();
                return false;
            }

            try
            {
                ShortHeader header = buffer.ToStruct<ShortHeader>();
                found = true;
                return (header.Flags & ESLFlag) != 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}