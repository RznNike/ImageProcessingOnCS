using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ImageProcessingOnSharp
{
    /// <summary>
    /// Main form
    /// </summary>
    public partial class MainForm : Form
    {
        private Stream _originalImage = null;
        private Stream _resultImage = null;
        private string _originalExtension = null;
        private string _resultExtension = null;
        private Hashtable _panels = null;
        private DateTime _time = new DateTime();
        double _compressingTime = 0;
        double _decompressingTime = 0;

        /// <summary>
        /// Main form constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _panels = new Hashtable();
            _panels.Add(panelQuality, new List<string>() { "JPEG" });
            _panels.Add(panelCompression, new List<string>() { "TIFF" });
            _panels.Add(panelWaveletLevels, new List<string>() { "Wavelet+GZIP" });
            _panels.Add(panelCompressionLevel, new List<string>()
                { "GZIP",
                "HInterlacing+GZIP",
                "VInterlacing+GZIP",
                "XInterlacing+GZIP",
                "Wavelet+GZIP" });
            _panels.Add(panelInterimFormat, new List<string>()
                { "HInterlacing+GZIP",
                "VInterlacing+GZIP",
                "XInterlacing+GZIP",
                "Wavelet+GZIP" });
            _panels.Add(panelFinalFormat, new List<string>()
                { "HInterlacing+GZIP",
                "VInterlacing+GZIP",
                "XInterlacing+GZIP",
                "Wavelet+GZIP" });

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
                MessageBox.Show("You should apply algorithm to get result.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                MessageBox.Show("You should select original image first.", "No target image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Algorithm algorithm = (Algorithm)cmbAlgorithm.SelectedItem;
            string option = algorithm.ToString();
            string parameters = "";
            Stream compressedImage = null;
            _resultExtension = algorithm.GetFileExtension();
            object finalFormat = cmbFinalFormat.SelectedItem;
            object interimFormat = cmbInterimFormat.SelectedItem;

            if (option.Equals("JPEG"))
            {
                long qualityLevel = (long)nudQualityLevel.Value;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { qualityLevel });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                parameters = string.Format("quality = {0}%.", qualityLevel);
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (option.Equals("PNG"))
            {
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                parameters = "-";
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (option.Equals("TIFF"))
            {
                int compression = cmbCompression.SelectedIndex;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compression });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                parameters = string.Format("compression = {0}.", cmbCompression.Items[compression].ToString());
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (option.Equals("GZIP"))
            {
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compressionLevel });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                parameters = string.Format("compression level = {0}.", cmbCompressionLevel.Items[compressionLevel].ToString().ToLower());
                _resultExtension = _originalExtension;
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (option.Equals("HInterlacing+GZIP")
                     || option.Equals("VInterlacing+GZIP")
                     || option.Equals("XInterlacing+GZIP"))
            {
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compressionLevel, interimFormat });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                parameters = string.Format("compression level = {0}; interim format = {1}; final format = {2}.",
                    cmbCompressionLevel.Items[compressionLevel].ToString().ToLower(),
                    interimFormat.ToString().ToLower(),
                    finalFormat.ToString().ToLower());
                _resultExtension = cmbFinalFormat.SelectedItem.ToString().ToLower();
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { finalFormat });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (option.Equals("Wavelet+GZIP"))
            {
                int waveletLevels = (int)nudWaveletLevels.Value;
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { waveletLevels, compressionLevel, interimFormat });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                parameters = string.Format("wavelet levels = {0}; compression level = {1}; interim format = {2}; final format = {3}.",
                    waveletLevels,
                    cmbCompressionLevel.Items[compressionLevel].ToString().ToLower(),
                    interimFormat.ToString().ToLower(),
                    finalFormat.ToString().ToLower());
                _resultExtension = cmbFinalFormat.SelectedItem.ToString().ToLower();
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { waveletLevels, finalFormat });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
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
            report.AppendLine(String.Format("Compressing time: {0} ms", _compressingTime));
            report.Append(String.Format("Decompressing time: {0} ms", _decompressingTime));

            return report.ToString();
        }

        private void cmbAlgorithm_SelectedValueChanged(object sender, EventArgs e)
        {
            string option = cmbAlgorithm.SelectedItem.ToString();

            IEnumerator enumerator = _panels.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Panel panel = (Panel)enumerator.Current;
                List<string> list = (List<string>)_panels[panel];
                if (list.Contains(option))
                {
                    panel.Visible = true;
                }
                else
                {
                    panel.Visible = false;
                }
            }
        }

        private void tsAbout_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }
    }
}
