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
            Cursor = Cursors.WaitCursor;

            double[,] groups;
            double[,] Y;
            try
            {
                int dataLength;
                int IVNumber;
                int DVNumber;

                using (StreamReader reader = new StreamReader(edtFilename.Text))
                {
                    string s = reader.ReadLine();
                    int[] ints = s.Split(' ').Select(t => Convert.ToInt32(t)).ToArray();

                    dataLength = ints[0];
                    IVNumber = ints[1];
                    DVNumber = ints[2];

                    groups = new double[dataLength, IVNumber];
                    Y = new double[dataLength, DVNumber];

                    for (int i = 0; i < dataLength; i++)
                    {
                        s = reader.ReadLine();
                        double[] nums = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
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

                double[] T, p;

                Analyzer.Anova(groups, Y, out T, out p);

                rtbResult.Clear();
                for (int i = 0; i < IVNumber; i++)
                {
                    rtbResult.AppendText(string.Format("Factor {0} T={1:0.00000} p={2:0.00000}\n", i + 1, T[i], p[i]));
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                rtbResult.Clear();
                rtbResult.SelectionColor = System.Drawing.Color.Red;
                rtbResult.AppendText(ex.Message);
                rtbResult.SelectionColor = System.Drawing.Color.Black;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
