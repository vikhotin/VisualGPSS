using System;
using System.Runtime.InteropServices;

namespace VisualGPSS
{
    class SimDataObtainer
    {
        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr FindGPSS();

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr FindBlocksWindow(IntPtr gpss);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr FindSimDataLV(IntPtr blocks);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr GetSimulationDataArray(IntPtr listview);

        [DllImport("DataObtainerLib.dll", SetLastError = true)]
        public static extern IntPtr ClearData(IntPtr listview, IntPtr table);

        private static IntPtr gpssHandle;
        private static IntPtr blocksHandle;
        private static IntPtr simInfoHandle;

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
            return true;
        }

        public static /**/void/**/ GetSimData()
        {
            IntPtr dataptr = GetSimulationDataArray(simInfoHandle);
            int elementSize = Marshal.SizeOf(typeof(IntPtr));
            for (int i = 0; i < 14; i++) // TODO: добавить число блоков
            {
                IntPtr rowptr = Marshal.ReadIntPtr(dataptr, i * elementSize);
                for (int j = 0; j < 4; j++)
                {
                    IntPtr strptr = Marshal.ReadIntPtr(rowptr, j * elementSize);
                    string str = Marshal.PtrToStringAuto(strptr, 24).TrimEnd('\0');
                    str = str.Substring(0, str.IndexOf('\0'));
                    // TODO: сохранять строки в структуру
                }
            }
            // TODO: а может быть, брать только нужную информацию?
            // TODO: не забыть освободить память
        }
    }
}
