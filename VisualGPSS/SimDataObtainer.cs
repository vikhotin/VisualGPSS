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
        }
    }
}
