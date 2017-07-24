using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ImageProcessingOnSharp
{
    public partial class MainForm : Form
    {
        Stream _originalImage;
        Stream _resultImage;
        string _originalExtension;
        string _resultExtension;

        public MainForm()
        {
            InitializeComponent();
            cmbAlgorithm.Items.Clear();
            cmbAlgorithm.Items.Add(JPEG.GetInstance());
            cmbAlgorithm.Items.Add(PNG.GetInstance());
            cmbAlgorithm.Items.Add(TIFF.GetInstance());
            cmbAlgorithm.Items.Add(GZIP.GetInstance());
            cmbAlgorithm.Items.Add(HInterlacingWithGZIP.GetInstance());
            cmbAlgorithm.Items.Add(VInterlacingWithGZIP.GetInstance());
            cmbAlgorithm.Items.Add(XInterlacingWithGZIP.GetInstance());
            cmbAlgorithm.Items.Add(WaveletWithGZIP.GetInstance());
            cmbAlgorithm.SelectedIndex = 0;

            cmbCompression.Items.Clear();
            cmbCompression.Items.Add("Default");
            cmbCompression.Items.Add("None");
            cmbCompression.Items.Add("CCITT3");
            cmbCompression.Items.Add("CCITT4");
            cmbCompression.Items.Add("LZW");
            cmbCompression.Items.Add("RLE");
            cmbCompression.Items.Add("ZIP");
            cmbCompression.Items.Add(".NET default");
            cmbCompression.SelectedIndex = 7;

            cmbCompressionLevel.Items.Clear();
            cmbCompressionLevel.Items.Add("Optimal");
            cmbCompressionLevel.Items.Add("Fast");
            cmbCompressionLevel.Items.Add("None");
            cmbCompressionLevel.SelectedIndex = 0;
            
            cmbInterimFormat.Items.Clear();
            cmbInterimFormat.Items.Add(ImageFormat.Bmp);
            cmbInterimFormat.Items.Add(ImageFormat.Png);
            cmbInterimFormat.Items.Add(ImageFormat.Tiff);
            cmbInterimFormat.SelectedIndex = 0;

            cmbFinalFormat.Items.Clear();
            cmbFinalFormat.Items.Add(ImageFormat.Bmp);
            cmbFinalFormat.Items.Add(ImageFormat.Png);
            cmbFinalFormat.Items.Add(ImageFormat.Tiff);
            cmbFinalFormat.SelectedIndex = 0;

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
                    _originalExtension = Path.GetExtension(openFileDialog.FileName).TrimStart('.');
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
            saveFileDialog.Filter = string.Format("Image|*.{0}", _resultExtension);
            saveFileDialog.FileName = string.Format("{0}.{1}", openFileDialog.FileName, _resultExtension);
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
            string option = algorithm.ToString();
            string parameters = "";
            Stream compressedImage = null;
            _resultExtension = algorithm.GetFileExtension();
            object finalFormat = cmbFinalFormat.SelectedItem;
            object interimFormat = cmbInterimFormat.SelectedItem;
            int waveletLevels = (int)nudWaveletLevels.Value;

            if (option.Equals("JPEG"))
            {
                long qualityLevel = (long)nudQualityLevel.Value;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { qualityLevel });
                parameters = string.Format("quality = {0}%.", qualityLevel);
            }
            else if (option.Equals("PNG"))
            {
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { });
                parameters = "-";
            }
            else if (option.Equals("TIFF"))
            {
                int compression = cmbCompression.SelectedIndex;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compression });
                parameters = string.Format("compression = {0}.", cmbCompression.Items[compression].ToString());
            }
            else if (option.Equals("GZIP"))
            {
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compressionLevel });
                parameters = string.Format("compression level = {0}.", cmbCompressionLevel.Items[compressionLevel].ToString());
                _resultExtension = _originalExtension;
            }
            else if (option.Equals("HInterlacing+GZIP")
                     || option.Equals("VInterlacing+GZIP")
                     || option.Equals("XInterlacing+GZIP"))
            {
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compressionLevel, interimFormat });
                parameters = string.Format("compression level = {0}.", cmbCompressionLevel.Items[compressionLevel].ToString());
                _resultExtension = cmbFinalFormat.SelectedItem.ToString().ToLower();
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { finalFormat });
            }
            else if (option.Equals("Wavelet+GZIP"))
            {
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { waveletLevels, compressionLevel, interimFormat });
                parameters = "-";
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { waveletLevels, finalFormat });
            }

            if (!option.Equals("HInterlacing+GZIP")
                && !option.Equals("VInterlacing+GZIP")
                && !option.Equals("XInterlacing+GZIP")
                && !option.Equals("Wavelet+GZIP"))
            {
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
            }
            rtbStatistic.Text = this.MakeReport(compressedImage, parameters);
            pboxResult.Image = new Bitmap(_resultImage);
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
                panelCompression.Visible = false;
                panelWaveletLevels.Visible = false;
                panelCompressionLevel.Visible = false;
                panelInterimFormat.Visible = false;
                panelFinalFormat.Visible = false;
            }
            else if (option.Equals("PNG"))
            {
                panelQuality.Visible = false;
                panelCompression.Visible = false;
                panelWaveletLevels.Visible = false;
                panelCompressionLevel.Visible = false;
                panelInterimFormat.Visible = false;
                panelFinalFormat.Visible = false;
            }
            else if (option.Equals("TIFF"))
            {
                panelQuality.Visible = false;
                panelCompression.Visible = true;
                panelWaveletLevels.Visible = false;
                panelCompressionLevel.Visible = false;
                panelInterimFormat.Visible = false;
                panelFinalFormat.Visible = false;
            }
            else if (option.Equals("GZIP"))
            {
                panelQuality.Visible = false;
                panelCompression.Visible = false;
                panelWaveletLevels.Visible = false;
                panelCompressionLevel.Visible = true;
                panelInterimFormat.Visible = false;
                panelFinalFormat.Visible = false;
            }
            else if (option.Equals("HInterlacing+GZIP")
                     || option.Equals("VInterlacing+GZIP")
                     || option.Equals("XInterlacing+GZIP"))
            {
                panelQuality.Visible = false;
                panelCompression.Visible = false;
                panelWaveletLevels.Visible = false;
                panelCompressionLevel.Visible = true;
                panelInterimFormat.Visible = true;
                panelFinalFormat.Visible = true;
            }
            else if (option.Equals("Wavelet+GZIP"))
            {
                panelQuality.Visible = false;
                panelCompression.Visible = false;
                panelWaveletLevels.Visible = true;
                panelCompressionLevel.Visible = true;
                panelInterimFormat.Visible = true;
                panelFinalFormat.Visible = true;
            }
        }

        private void tsAbout_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }
    }
}
