using System.Drawing;

namespace VisualGPSS
{
    public class VisualModel
    {
        public VisualBlock[] blocks { get; set; }

        public void Paint(Graphics g)
        {
            foreach (VisualBlock block in blocks)
            {
                block.Draw(g);
            }
        }
    }
}
