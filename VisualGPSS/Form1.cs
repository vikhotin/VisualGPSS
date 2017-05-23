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
        public Form1()
        {
            InitializeComponent();
            string error = "";
            if (!SimDataObtainer.Init(ref error))
            {
                MessageBox.Show(error);
                Text = "False";
            }
            else
                Text = "True";
            SimDataObtainer.GetSimData();
        }
    }
}
