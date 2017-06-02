using System;
using System.Windows.Forms;

namespace VisualGPSS
{
    public partial class ModelForm : Form
    {
        public Renderer Renderer { get; set; }

        private VisualModel model = new VisualModel();

        public ModelForm()
        {
            InitializeComponent();
            Renderer = new Renderer(model);
            timer1.Tick += new EventHandler(RunFrame);
            StopTimer();
        }
        
        public void StartTimer()
        {
            timer1.Enabled = true;
        }

        public void StopTimer()
        {
            timer1.Enabled = false;
        }

        private void ModelForm_Paint(object sender, PaintEventArgs e)
        {
            Renderer.Render(e.Graphics);
        }

        private void RunFrame(object sender, EventArgs e)
        {
            GpssBlockData[] gpssBlocks = SimDataObtainer.GetSimData();
            if (model.blocks != null)
                GpssToVisualConverter.UpdateStats(model.blocks, gpssBlocks);
            else
                model.blocks = GpssToVisualConverter.Convert(gpssBlocks);
            foreach (VisualBlock block in model.blocks)
            {
                Controls.Add(block);
            }
            Invalidate();
        }

        private void ModelForm_ResizeBegin(object sender, EventArgs e)
        {
            StopTimer();
        }

        private void ModelForm_ResizeEnd(object sender, EventArgs e)
        {
            StartTimer();
        }
    }
}
