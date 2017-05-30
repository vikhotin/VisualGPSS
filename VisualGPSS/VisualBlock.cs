using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisualGPSS
{
    public class VisualBlock : Control
    {
        public int Id { get; set; }

        public string Label { get; set; }
        public int TaskCount { get; set; }

        // public Point Location { get; set; }

        public List<VisualBlock> Links { get; private set; }

        private Bitmap TaskImage;

        public VisualBlock()
        {
            Links = new List<VisualBlock>();
            TaskImage = new Bitmap(10, 10);
            using (Graphics graphics = Graphics.FromImage(TaskImage))
            {
                graphics.DrawImage(Properties.Resources.task, 0, 0, 10, 10);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // base.OnPaint(e);
            Draw(e.Graphics);
        }

        public virtual void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawEllipse(pb, Location.X - 3, Location.Y - 3, 6, 6);
            DrawLinks(g);
            DrawTasks(g);
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

        protected void DrawTasks(Graphics g)
        {
            if (TaskCount > 0)
            {
                g.DrawImageUnscaled(TaskImage, new Point(Location.X + 20, Location.Y - 5));
                g.DrawString(TaskCount.ToString(), new Font(FontFamily.GenericSansSerif, 10),
                             Brushes.Black, Location.X + 30, Location.Y - 10);
            }
        }
    }

    class GeneratorBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawPolygon(pb, new Point[] {
                new Point(Location.X - 5, Location.Y - 10),
                new Point(Location.X - 5, Location.Y - 5),
                new Point(Location.X - 2, Location.Y - 5),
                new Point(Location.X - 2, Location.Y),
                new Point(Location.X + 2, Location.Y),
                new Point(Location.X + 2, Location.Y - 5),
                new Point(Location.X + 5, Location.Y - 5),
                new Point(Location.X + 5, Location.Y - 10),
            });
            DrawLinks(g);
            DrawTasks(g);
        }
    }

    class QueueBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawLine(pb, new Point(Location.X - 5, Location.Y - 10), 
                           new Point(Location.X - 5, Location.Y - 5));
            g.DrawLine(pb, new Point(Location.X + 5, Location.Y - 10),
                           new Point(Location.X + 5, Location.Y - 5));
            g.DrawRectangles(pb, new Rectangle[] {
                new Rectangle(Location.X - 5, Location.Y - 5, 10, 5),
                new Rectangle(Location.X - 5, Location.Y, 10, 5),
                new Rectangle(Location.X - 5, Location.Y + 5, 10, 5),
            });
            DrawLinks(g);
            DrawTasks(g);
        }
    }

    class FacilityBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawRectangles(pb, new Rectangle[] {
                new Rectangle(Location.X - 6, Location.Y - 6, 10, 10),
                new Rectangle(Location.X - 5, Location.Y - 5, 10, 10),
                new Rectangle(Location.X - 4, Location.Y - 4, 10, 10),
            });
            DrawLinks(g);
            DrawTasks(g);
        }
    }

    class StorageBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawRectangles(pb, new Rectangle[] {
                new Rectangle(Location.X - 10, Location.Y - 5, 5, 10),
                new Rectangle(Location.X - 5, Location.Y - 5, 5, 10),
                new Rectangle(Location.X    , Location.Y - 5, 5, 10),
                new Rectangle(Location.X + 5, Location.Y - 5, 5, 10),
            });
            DrawLinks(g);
            DrawTasks(g);
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
