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
        public Point Center { get { return new Point(Location.X + Width / 2 + 1, Location.Y + Height / 2 + 1); } }

        public List<VisualBlock> Links { get; private set; }

        private Bitmap TaskImage;

        public VisualBlock()
        {
            Links = new List<VisualBlock>();
            TaskImage = new Bitmap(15, 15);
            using (Graphics graphics = Graphics.FromImage(TaskImage))
            {
                graphics.DrawImage(Properties.Resources.task, 0, 0, 15, 15);
            }
            //Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // base.OnPaint(e);
            Draw(e.Graphics);
        }

        public virtual void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawEllipse(pb, Center.X - 6, Center.Y - 6, 12, 12);
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
                    g.DrawLine(p, Center, blockTo.Center);
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
                g.DrawImageUnscaled(TaskImage, new Point(Center.X + 25, Center.Y - 5));
                g.DrawString(TaskCount.ToString(), new Font(FontFamily.GenericSansSerif, 10),
                             Brushes.Black, Center.X + 40, Center.Y - 5);
            }
        }
    }

    class GeneratorBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawPolygon(pb, new Point[] {
                new Point(Center.X - 10, Center.Y - 20),
                new Point(Center.X - 10, Center.Y - 10),
                new Point(Center.X - 5, Center.Y - 10),
                new Point(Center.X - 5, Center.Y),
                new Point(Center.X + 5, Center.Y),
                new Point(Center.X + 5, Center.Y - 10),
                new Point(Center.X + 10, Center.Y - 10),
                new Point(Center.X + 10, Center.Y - 20),
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
            g.DrawLine(pb, new Point(Center.X - 10, Center.Y - 20), 
                           new Point(Center.X - 10, Center.Y - 10));
            g.DrawLine(pb, new Point(Center.X + 10, Center.Y - 20),
                           new Point(Center.X + 10, Center.Y - 10));
            g.DrawRectangles(pb, new Rectangle[] {
                new Rectangle(Center.X - 10, Center.Y - 10, 20, 10),
                new Rectangle(Center.X - 10, Center.Y, 20, 10),
                new Rectangle(Center.X - 10, Center.Y + 10, 20, 10),
            });
            g.DrawString(Label,
                         new Font(FontFamily.GenericSansSerif, 14, FontStyle.Regular),
                         Brushes.Black,
                         Center.X + 20, Center.Y - 25);
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
                new Rectangle(Center.X - 16, Center.Y - 16, 30, 30),
                new Rectangle(Center.X - 15, Center.Y - 15, 30, 30),
                new Rectangle(Center.X - 14, Center.Y - 14, 30, 30),
            });
            g.DrawString(Label, 
                         new Font(FontFamily.GenericSansSerif, 14, FontStyle.Regular),
                         Brushes.Black,
                         Center.X + 20, Center.Y - 25);
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
                new Rectangle(Center.X - 20, Center.Y - 10, 10, 20),
                new Rectangle(Center.X - 10, Center.Y - 10, 10, 20),
                new Rectangle(Center.X     , Center.Y - 10, 10, 20),
                new Rectangle(Center.X + 10, Center.Y - 10, 10, 20),
            });
            g.DrawString(Label, 
                         new Font(FontFamily.GenericSansSerif, 14, FontStyle.Regular),
                         Brushes.Black,
                         Center.X + 20, Center.Y - 25);
            DrawLinks(g);
            DrawTasks(g);
        }
    }

    class TerminateBlock : VisualBlock
    {
        public override void Draw(Graphics g)
        {
            Pen pb = new Pen(Color.Blue, 2);
            g.DrawLine(pb, Center.X - 10, Center.Y - 10, Center.X + 10, Center.Y + 10);
            g.DrawLine(pb, Center.X - 10, Center.Y + 10, Center.X + 10, Center.Y - 10);
        }
    }
}
