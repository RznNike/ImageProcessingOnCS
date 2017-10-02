using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
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
        private double _compression = 0;
        private string _originalExtension = null;
        private string _resultExtension = null;
        private Hashtable _panels = null;
        private DateTime _time = new DateTime();
        private double _compressingTime = 0;
        private double _decompressingTime = 0;
        private bool _groupReporting = true;
        private string _algorithm = null;
        private string _algorithmParameters = null;


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
            openFileDialog.Multiselect = false;
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
            _algorithm = algorithm.ToString();
            _algorithmParameters = "";
            Stream compressedImage = null;
            _resultExtension = algorithm.GetFileExtension();
            object finalFormat = cmbFinalFormat.SelectedItem;
            object interimFormat = cmbInterimFormat.SelectedItem;

            if (_algorithm.Equals("JPEG"))
            {
                long qualityLevel = (long)nudQualityLevel.Value;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { qualityLevel });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                _algorithmParameters = string.Format("quality = {0}%.", qualityLevel);
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (_algorithm.Equals("PNG"))
            {
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                _algorithmParameters = "-";
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (_algorithm.Equals("TIFF"))
            {
                int compression = cmbCompression.SelectedIndex;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compression });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                _algorithmParameters = string.Format("compression = {0}.", cmbCompression.Items[compression].ToString());
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (_algorithm.Equals("GZIP"))
            {
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compressionLevel });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                _algorithmParameters = string.Format("compression level = {0}.", cmbCompressionLevel.Items[compressionLevel].ToString().ToLower());
                _resultExtension = _originalExtension;
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (_algorithm.Equals("HInterlacing+GZIP")
                     || _algorithm.Equals("VInterlacing+GZIP")
                     || _algorithm.Equals("XInterlacing+GZIP"))
            {
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { compressionLevel, interimFormat });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                _algorithmParameters = string.Format("compression level = {0}; interim format = {1}; final format = {2}.",
                    cmbCompressionLevel.Items[compressionLevel].ToString().ToLower(),
                    interimFormat.ToString().ToLower(),
                    finalFormat.ToString().ToLower());
                _resultExtension = cmbFinalFormat.SelectedItem.ToString().ToLower();
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { finalFormat });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }
            else if (_algorithm.Equals("Wavelet+GZIP"))
            {
                int waveletLevels = (int)nudWaveletLevels.Value;
                int compressionLevel = cmbCompressionLevel.SelectedIndex;
                _time = DateTime.UtcNow;
                compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { waveletLevels, compressionLevel, interimFormat });
                _compressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
                _algorithmParameters = string.Format("wavelet levels = {0}; compression level = {1}; interim format = {2}; final format = {3}.",
                    waveletLevels,
                    cmbCompressionLevel.Items[compressionLevel].ToString().ToLower(),
                    interimFormat.ToString().ToLower(),
                    finalFormat.ToString().ToLower());
                _resultExtension = cmbFinalFormat.SelectedItem.ToString().ToLower();
                _time = DateTime.UtcNow;
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>() { waveletLevels, finalFormat });
                _decompressingTime = (DateTime.UtcNow - _time).TotalMilliseconds;
            }

            _compression = (_originalImage.Length - compressedImage.Length) * 100.0 / _originalImage.Length;
            if (!_groupReporting)
            {
                rtbStatistic.Text = this.MakeReport(compressedImage);
                pboxResult.Image = new Bitmap(_resultImage);
            }
        }

        private string MakeReport(Stream parCompressedImage)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine(String.Format("Original image size: {0} bytes", _originalImage.Length));
            report.AppendLine(String.Format("Compressed image size: {0} bytes", parCompressedImage.Length));
            report.AppendLine(String.Format("Decompressed image size: {0} bytes", _resultImage.Length));
            report.AppendLine(String.Format("Total compression: {0}%", _compression));
            report.AppendLine(String.Format("Algorithm: {0}", _algorithm));
            report.AppendLine(String.Format("Parameters: {0}", _algorithmParameters));
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

        private void tsmGroupAnalysis_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = true;
            DialogResult choice = openFileDialog.ShowDialog();
            if (choice == DialogResult.OK)
            {
                Thread processingThread = new Thread(ApplyAlgorithmToGroup);
                processingThread.Start();
            }
        }

        private delegate void Del(object sender, EventArgs e);

        private void ApplyAlgorithmToGroup()
        {
            _groupReporting = true;

            string[ ] names = openFileDialog.FileNames;
            int processedCount = 0;
            double averageCompression = 0;
            double averageAccuracy = 0;
            double averageCompressingTime = 0;
            double averageDecompressingTime = 0;

            for (int i = 0; i < names.Length; i++)
            {
                try
                {
                    _originalImage = new MemoryStream(File.ReadAllBytes(names[i]));
                    Del del = tsmApplyAlgorithm_Click;
                    this.Invoke(del, null, null);

                    averageCompression += _compression;
                    averageAccuracy += ImageComparator.CalculateImagesEquality(_originalImage, _resultImage);
                    averageCompressingTime += _compressingTime;
                    averageDecompressingTime += _decompressingTime;
                    processedCount++;
                }
                catch
                {
                }

                this.rtbStatistic.BeginInvoke((MethodInvoker)
                    (() => this.rtbStatistic.Text = $"Processed images: {processedCount}"));
                this.progressBarGroupProcessing.BeginInvoke((MethodInvoker)
                    (() => this.progressBarGroupProcessing.Value = (int)(i * 1000.0 / names.Length)));
            }
            averageCompression /= processedCount;
            averageAccuracy /= processedCount;
            averageCompressingTime /= processedCount;
            averageDecompressingTime /= processedCount;

            string report = MakeGroupReport(processedCount, averageCompression, averageAccuracy,
                averageCompressingTime, averageDecompressingTime);
            this.rtbStatistic.BeginInvoke((MethodInvoker)
                (() => this.rtbStatistic.Text = report));

            this.progressBarGroupProcessing.BeginInvoke((MethodInvoker)
                    (() => this.progressBarGroupProcessing.Value = 0));

            Thread.Sleep(300);
            _groupReporting = false;

            GC.Collect();
        }

        private string MakeGroupReport(int parProcessedCount, double parAverageCompression, double parAverageAccuracy,
            double parAverageCompressingTime, double parAverageDecompressingTime)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine(String.Format("Algorithm: {0}", _algorithm));
            report.AppendLine(String.Format("Parameters: {0}", _algorithmParameters));
            report.AppendLine(String.Format("Processed images: {0}", parProcessedCount));
            report.AppendLine(String.Format("Average compression: {0}%", parAverageCompression));
            report.AppendLine(String.Format("Average accuracy: {0}%", parAverageAccuracy * 100));
            report.AppendLine(String.Format("Average compressing time: {0} ms", parAverageCompressingTime));
            report.Append(String.Format("Average decompressing time: {0} ms", parAverageDecompressingTime));

            return report.ToString();
        }
    }
}
