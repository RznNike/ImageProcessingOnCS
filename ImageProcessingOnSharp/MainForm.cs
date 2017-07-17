using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingOnSharp
{
    public partial class MainForm : Form
    {
        Stream _originalImage;
        Stream _resultImage;
        String _resultExtension;

        public MainForm()
        {
            InitializeComponent();
            cmbAlgorithm.Items.Clear();
            cmbAlgorithm.Items.Add(JPEG.GetInstance());
            cmbAlgorithm.Items.Add(PNG.GetInstance());
            cmbAlgorithm.SelectedIndex = 0;

            cmbCompression.Items.Clear();
            cmbCompression.Items.Add("LZW");
            cmbCompression.Items.Add("CCITT3");
            cmbCompression.Items.Add("CCITT4");
            cmbCompression.Items.Add("RLE");
            cmbCompression.Items.Add("None");
            cmbCompression.SelectedIndex = 0;

            _resultExtension = null;
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsmOpenImage_Click(object sender, EventArgs e)
        {
            DialogResult choice = openFileDialog.ShowDialog();
            if (choice == DialogResult.OK)
            {
                try
                {
                    _originalImage = new MemoryStream(File.ReadAllBytes(openFileDialog.FileName));

                    pboxOriginal.Image = new Bitmap(_originalImage);
                }
                catch
                {
                    MessageBox.Show("Can't open image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void tsmSaveResult_Click(object sender, EventArgs e)
        {
            if (_resultImage == null)
            {
                return;
            }

            saveFileDialog.DefaultExt = _resultExtension;
            saveFileDialog.Filter = String.Format("Изображение|*.{0}", _resultExtension);
            saveFileDialog.FileName = String.Format("{0}.{1}", openFileDialog.FileName, _resultExtension);
            DialogResult choice = saveFileDialog.ShowDialog();
            if (choice == DialogResult.OK)
            {
                try
                {
                    Image image = Image.FromStream(_resultImage);
                    image.Save(saveFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Can't save image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void tsmApplyAlgorithm_Click(object sender, EventArgs e)
        {
            if (_originalImage == null)
            {
                return;
            }

            Algorithm algorithm = (Algorithm)cmbAlgorithm.SelectedItem;
            if (algorithm.ToString().Equals("JPEG"))
            {
                long qualityLevel = (long)nudQualityLevel.Value;
                Stream compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { qualityLevel });
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>());
                rtbStatistic.Text = this.MakeReport(compressedImage, "");
            }
            else if (algorithm.ToString().Equals("PNG"))
            {
                long qualityLevel = (long)nudQualityLevel.Value;
                Stream compressedImage = algorithm.CompressImage(_originalImage, new List<object>());
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>());
                rtbStatistic.Text = this.MakeReport(compressedImage, "");
            }
            pboxResult.Image = new Bitmap(_resultImage);
            _resultExtension = algorithm.GetFileExtension();
        }

        private string MakeReport(Stream parCompressedImage, string parAlgorithmParameters)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine(String.Format("Original image size: {0} bytes", _originalImage.Length));
            report.AppendLine(String.Format("Compressed image size: {0} bytes", parCompressedImage.Length));
            report.AppendLine(String.Format("Decompressed image size: {0} bytes", _resultImage.Length));
            double compression = (_originalImage.Length - parCompressedImage.Length) * 100.0 / _originalImage.Length;
            report.AppendLine(String.Format("Total compression: {0}%", compression));
            report.AppendLine(String.Format("Algorithm: {0}", cmbAlgorithm.SelectedItem));
            report.AppendLine(String.Format("Parameters: {0}", parAlgorithmParameters));
            double accuracy = ImageComparator.CalculateImagesEquality(_originalImage, _resultImage);
            report.AppendLine(String.Format("Accuracy: {0}%", accuracy * 100));

            return report.ToString();
        }

        private void cmbAlgorithm_SelectedValueChanged(object sender, EventArgs e)
        {
            string option = cmbAlgorithm.SelectedItem.ToString();
            if (option.Equals("JPEG"))
            {
                panelQuality.Visible = true;
            }
            else if (option.Equals("PNG"))
            {
                panelQuality.Visible = false;
            }
        }

        private void tsAbout_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }
    }
}
