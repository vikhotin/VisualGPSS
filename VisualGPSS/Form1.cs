using System;
using System.Windows.Forms;

namespace VisualGPSS
{
    public partial class Form1 : Form
    {
        private ModelForm modelForm;

        private AnovaForm anovaForm;
        
        public Form1()
        {
            InitializeComponent();
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
                modelForm = new ModelForm();
                modelForm.Show(this);
                modelForm.StartTimer();
            }
        }

        private void anovaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            anovaForm = new AnovaForm();
            anovaForm.Show(this);
        }
    }
}
