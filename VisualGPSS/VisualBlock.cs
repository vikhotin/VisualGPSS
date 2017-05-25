using System.Drawing;
using System.Collections.Generic;

namespace VisualGPSS
{
    public class VisualBlock
    {
        public int Id { get; set; }

        public string Label { get; set; }
        public int TaskCount { get; set; }

        public Point Location { get; set; }

        public List<VisualBlock> Links { get; private set; }

        public VisualBlock()
        {
            Links = new List<VisualBlock>();
        }

        public virtual void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawEllipse(pb, Location.X - 5, Location.Y - 5, 10, 10);
            DrawLinks(g);
        }

        protected void DrawLinks(Graphics g)
        {
            foreach (VisualBlock blockTo in Links)
            {
                if (blockTo.Id - Id == 1)
                {
                    Pen p = new Pen(Color.Black);
                    g.DrawLine(p, Location, blockTo.Location);
                }
                else
                {
                    // TODO:
                }
            }
        }
    }

    class GeneratorBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            // TODO
            base.Draw(g);
        }
    }

    class QueueBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            // TODO
            base.Draw(g);
        }
    }

    class FacilityBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            // TODO
            base.Draw(g);
        }
    }

    class StorageBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            // TODO
            base.Draw(g);
        }
    }

    class TerminateBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawLine(pb, Location.X - 5, Location.Y - 5, Location.X + 5, Location.Y + 5);
            g.DrawLine(pb, Location.X - 5, Location.Y + 5, Location.X + 5, Location.Y - 5);
        }
    }
}
