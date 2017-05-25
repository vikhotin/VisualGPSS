using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            }
        }
    }
}
