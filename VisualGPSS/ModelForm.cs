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
    public partial class ModelForm : Form
    {
        public Renderer Renderer { get; set; }

        public ModelForm(Renderer r)
        {
            InitializeComponent();
            Renderer = r;
        }

        private void ModelForm_Paint(object sender, PaintEventArgs e)
        {
            Renderer.Render(e.Graphics);
        }
    }
}
