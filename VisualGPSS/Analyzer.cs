using MathWorks.MATLAB.NET.Utility;
using MathWorks.MATLAB.NET.Arrays;
using MatlabMancovan;

namespace VisualGPSS
{
    static class Analyzer
    {
        public static void Anova(double[,] groups, double[,] y, out double[] T, out double[] p)
        {
            MWNumericArray Groups = new MWNumericArray(groups);
            MWNumericArray Y = new MWNumericArray(y);

            Mancovan mancova = new Mancovan();
            MWArray[] res = mancova.mancovan(2, Y, Groups);

            T = (double[])((MWNumericArray)res[0]).ToVector(MWArrayComponent.Real);
            p = (double[])((MWNumericArray)res[1]).ToVector(MWArrayComponent.Real);
        }
    }
}
