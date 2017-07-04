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

        public MainForm()
        {
            InitializeComponent();
            cmbAlgorithm.Items.Clear();
            cmbAlgorithm.Items.Add(JPEG.GetInstance());
            cmbAlgorithm.SelectedIndex = 0;
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsmOpenImage_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
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

        private void tsmSaveResult_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = openFileDialog.FileName + ".bmp";
            saveFileDialog.ShowDialog();
            try
            {
                pboxResult.Image.Save(saveFileDialog.FileName, ImageFormat.Bmp);
            }
            catch
            {
                MessageBox.Show("Can't save image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tsmApplyAlgorithm_Click(object sender, EventArgs e)
        {
            Algorithm algorithm = (Algorithm)cmbAlgorithm.SelectedItem;
            if (algorithm.ToString().Equals("JPEG"))
            {
                long qualityLevel = (long)nudQualityLevel.Value;
                Stream compressedImage = algorithm.CompressImage(_originalImage, new List<object>() { qualityLevel });
                _resultImage = algorithm.DecompressImage(compressedImage, new List<object>());
            }
            pboxResult.Image = new Bitmap(_resultImage);
        }
    }
}
