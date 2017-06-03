using System;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Globalization;

namespace VisualGPSS
{
    public partial class AnovaForm : Form
    {
        public AnovaForm()
        {
            InitializeComponent();
        }

        private void btnFindFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog(this);
            edtFilename.Text = openFileDialog.FileName;
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            double[,] groups;
            double[,] Y;
            try
            {
                using (StreamReader reader = new StreamReader(edtFilename.Text))
                {
                    string s = reader.ReadLine();
                    int[] ints = s.Split(' ').Select(t => Convert.ToInt32(t)).ToArray();

                    int dataLength = ints[0];
                    int IVNumber = ints[1];
                    int DVNumber = ints[2];

                    groups = new double[dataLength, IVNumber];
                    Y = new double[dataLength, DVNumber];

                    for (int i = 0; i < dataLength; i++)
                    {
                        s = reader.ReadLine();
                        double[] nums = s.Split(' ')
                                         .Select(t => double.Parse(t, CultureInfo.InvariantCulture))
                                         .ToArray();
                        for (int j = 0; j < IVNumber; j++)
                        {
                            groups[i, j] = nums[j];
                        }
                        for (int j = 0; j < DVNumber; j++)
                        {
                            Y[i, j] = nums[IVNumber + j];
                        }
                    }
                }

                Analyzer.Anova(groups, Y);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
