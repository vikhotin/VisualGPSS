using MathWorks.MATLAB.NET.Utility;
using MathWorks.MATLAB.NET.Arrays;
using MatlabMancovan;

namespace VisualGPSS
{
    static class Analyzer
    {
        public static void Anova(double[,] groups, double[,] y)
        {
            MWNumericArray Groups = new MWNumericArray(groups);
            MWNumericArray Y = new MWNumericArray(y);

            Mancovan mancova = new Mancovan();
            MWArray[] res = mancova.mancovan(2, Y, Groups);
        }
    }
}
