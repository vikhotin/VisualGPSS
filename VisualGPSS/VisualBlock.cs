using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

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
            Location = new Point(0, 0);
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

        private static int Shift = 25;
        private RandColor rand;

        protected void DrawLinks(Graphics g)
        {
            RandColor rand = new RandColor(Id);
            foreach (VisualBlock blockTo in Links)
            {
                if (blockTo.Id - Id == 1)
                {
                    Pen p = new Pen(Color.Black);
                    g.DrawLine(p, Center, blockTo.Center);
                }
                else
                {
                    int shift = Shift + Id * 3;
                    if (shift > 50)
                        shift %= 50;
                    Pen p = new Pen(rand.GetColor());

                    Point interPoint1;

                    if (blockTo.Left == Left)
                        interPoint1 = new Point(Center.X - shift, Center.Y);
                    else
                        interPoint1 = new Point(Center.X + shift, Center.Y);
                    Point interPoint2 = new Point(blockTo.Center.X - shift, blockTo.Center.Y - 5);
                    Point interPoint3 = new Point(blockTo.Center.X, blockTo.Center.Y - 5);

                    g.DrawLine(p, Center, interPoint1);
                    g.DrawLine(p, interPoint1, interPoint2);
                    p.CustomEndCap = new AdjustableArrowCap(5, 7);
                    g.DrawLine(p, interPoint2, interPoint3);
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
                new Point(Center.X - 15, Center.Y - 25),
                new Point(Center.X - 15, Center.Y - 15),
                new Point(Center.X - 10, Center.Y - 15),
                new Point(Center.X - 10, Center.Y),
                new Point(Center.X + 10, Center.Y),
                new Point(Center.X + 10, Center.Y - 15),
                new Point(Center.X + 15, Center.Y - 15),
                new Point(Center.X + 15, Center.Y - 25),
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
            Brush b;
            Rectangle r1 = new Rectangle(Center.X - 10, Center.Y - 10, 20, 10);
            g.DrawRectangle(pb, r1);
            if (TaskCount > 10)
            {
                b = Brushes.Red;
                g.FillRectangle(b, r1);
            }

            Rectangle r2 = new Rectangle(Center.X - 10, Center.Y, 20, 10);
            g.DrawRectangle(pb, r2);
            if (TaskCount > 10)
            {
                b = Brushes.Red;
                g.FillRectangle(b, r2);
            }
            else if (TaskCount > 5)
            {
                b = Brushes.Yellow;
                g.FillRectangle(b, r2);
            }

            Rectangle r3 = new Rectangle(Center.X - 10, Center.Y + 10, 20, 10);
            g.DrawRectangle(pb, r3);
            if (TaskCount > 10)
            {
                b = Brushes.Red;
                g.FillRectangle(b, r3);
            }
            else if (TaskCount > 5)
            {
                b = Brushes.Yellow;
                g.FillRectangle(b, r3);
            }
            else if (TaskCount > 0)
            {
                b = Brushes.Green;
                g.FillRectangle(b, r3);
            }

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

namespace VisualGPSS
{
    class RandColor
    {
        private System.Random rand;
        private static Color[] colors = new Color[]
        {
            Color.SandyBrown,
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.DarkBlue,
            Color.DarkCyan,
            Color.DarkGoldenrod,
            Color.DarkGray,
            Color.DarkGreen,
            Color.DarkKhaki,
            Color.DarkMagenta,
            Color.DarkOliveGreen,
            Color.DarkOrange,
            Color.DarkOrchid,
            Color.DarkRed,
            Color.DarkSalmon,
            Color.DarkSeaGreen,
            Color.DarkSlateBlue,
            Color.DarkSlateGray,
            Color.DarkTurquoise,
            Color.DarkViolet,
            Color.DeepPink,
            Color.DeepSkyBlue,
        };
        public Color GetColor()
        {
            return colors[rand.Next(colors.Length)];
        }
        public RandColor(int seed)
        {
            rand = new System.Random(seed);
        }
    }
}