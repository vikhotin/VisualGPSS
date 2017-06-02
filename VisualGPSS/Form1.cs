using System;
using System.Windows.Forms;

namespace VisualGPSS
{
    public partial class Form1 : Form
    {
        private ModelForm modelForm;
        
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
    }
}
