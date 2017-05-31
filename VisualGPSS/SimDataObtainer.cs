using System;
using System.Runtime.InteropServices;

namespace VisualGPSS
{
    static class SimDataObtainer
    {
        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr FindGPSS();

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr FindBlocksWindow(IntPtr gpss);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr FindSimDataLV(IntPtr blocks);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr FindSourceCodeRE(IntPtr gpss, IntPtr blocks);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern int GetSourceCodeLength(IntPtr richedit);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr GetSourceCode(IntPtr richedit, int length);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern void ClearSourceCode(IntPtr str);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern int GetListviewCount(IntPtr listview);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr GetSimulationDataArray(IntPtr listview);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern void ClearData(IntPtr table, int count);

        private static IntPtr gpssHandle;
        private static IntPtr blocksHandle;
        private static IntPtr simInfoHandle;
        private static IntPtr sourceCodeHandle;

        private static int blocksCount;

        private static int sourceCodeLength;
        private static string[] sourceCode;

        public static GpssBlockData[] SimData { get; private set; }

        public static bool Init(ref string error)
        {
            if (!Connect(ref error))
            {
                return false;
            }

            blocksCount = GetListviewCount(simInfoHandle);
            SimData = new GpssBlockData[blocksCount];
            return true;
        }

        public static bool Connect(ref string error)
        {
            gpssHandle = FindGPSS();
            if (gpssHandle.Equals(IntPtr.Zero))
            {
                error = "GPSS World not opened";
                return false;
            }
            blocksHandle = FindBlocksWindow(gpssHandle);
            if (blocksHandle.Equals(IntPtr.Zero))
            {
                error = "Blocks window not opened";
                return false;
            }
            simInfoHandle = FindSimDataLV(blocksHandle);
            if (simInfoHandle.Equals(IntPtr.Zero))
            {
                error = "Unknown error";
                return false;
            }
            sourceCodeHandle = FindSourceCodeRE(gpssHandle, blocksHandle);
            if (sourceCodeHandle.Equals(IntPtr.Zero))
            {
                error = "Unknown error";
                return false;
            }

            sourceCodeLength = GetSourceCodeLength(sourceCodeHandle);
            IntPtr sourceCodeptr = GetSourceCode(sourceCodeHandle, sourceCodeLength);
            string str = Marshal.PtrToStringAuto(sourceCodeptr, sourceCodeLength);
            sourceCode = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            ClearSourceCode(sourceCodeptr);

            return true;
        }

        public static GpssBlockData[] GetSimData()
        {
            IntPtr dataptr = GetSimulationDataArray(simInfoHandle);
            int elementSize = Marshal.SizeOf(typeof(IntPtr));
            for (int i = 0; i < blocksCount; i++)
            {
                IntPtr rowptr = Marshal.ReadIntPtr(dataptr, i * elementSize);
                SimData[i] = new GpssBlockData();
                for (int j = 0; j < 4; j++)
                {
                    IntPtr strptr = Marshal.ReadIntPtr(rowptr, j * elementSize);
                    string str = Marshal.PtrToStringAuto(strptr, 24); //.TrimEnd('\0');
                    str = str.Substring(0, str.IndexOf('\0'));
                    SimData[i]._data[j] = str;
                }
            }
            ClearData(dataptr, blocksCount);

            return SimData;
        }
        // TODO: а может быть, брать только нужную информацию?
    }
}
