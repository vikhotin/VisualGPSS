using System.Drawing;

namespace VisualGPSS
{
    public class Renderer
    {
        VisualModel model;

        public Renderer(VisualModel model)
        {
            this.model = model;
        }

        public void Render(Graphics g)
        {
            foreach (VisualBlock block in model.blocks)
            {
                block.Draw(g);
            }
        }
    }
}
