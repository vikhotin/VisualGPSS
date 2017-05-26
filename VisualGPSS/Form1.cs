using System;
using System.Windows.Forms;

namespace VisualGPSS
{
    public partial class Form1 : Form
    {
        private ModelForm modelForm;
        private VisualModel model = new VisualModel();
        private Renderer renderer;

        public Form1()
        {
            InitializeComponent();
            renderer = new Renderer(model);
            modelForm = new ModelForm(renderer);
            //modelForm.Location = new Point(Location.X, Location.Y + Height);
            timer1.Tick += new EventHandler(RunFrame);
            timer1.Enabled = false;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string error = "";
            if (!SimDataObtainer.Init(ref error))
            {
                MessageBox.Show(error);
            }
            else
            {
                GpssBlockData[] gpssBlocks = SimDataObtainer.GetSimData();
                model.blocks = GpssToVisualConverter.Convert(gpssBlocks);
                modelForm.Show(this);
                //modelForm.Invalidate();

                timer1.Enabled = true;
            }
        }

        private void RunFrame(object sender, EventArgs e)
        {
            GpssBlockData[] gpssBlocks = SimDataObtainer.GetSimData();
            model.blocks = GpssToVisualConverter.Convert(gpssBlocks);
            modelForm.Invalidate();
        }
    }
}
