using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace ImageToBmpConverter
{
    public partial class FormMain : Form
    {
        string[ ] _names = null;
        string _outputPath = null;
        Thread _conversionThread = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonChooseInput_Click(object sender, EventArgs e)
        {
            DialogResult choice = openFileDial.ShowDialog();
            if (choice == DialogResult.OK)
            {
                _names = openFileDial.FileNames;
            }
        }

        private void buttonChooseOutput_Click(object sender, EventArgs e)
        {
            DialogResult choice = folderBrowserDial.ShowDialog();
            if (choice == DialogResult.OK)
            {
                _outputPath = folderBrowserDial.SelectedPath;
            }
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            while (_names == null)
            {
                DialogResult choice = openFileDial.ShowDialog();
                if (choice == DialogResult.OK)
                {
                    _names = openFileDial.FileNames;
                }
                else
                {
                    return;
                }
            }
            while (_outputPath == null)
            {
                DialogResult choice = folderBrowserDial.ShowDialog();
                if (choice == DialogResult.OK)
                {
                    _outputPath = folderBrowserDial.SelectedPath;
                }
                else
                {
                    return;
                }
            }

            _conversionThread = new Thread(Convert);
            _conversionThread.Start();
        }

        private void Convert()
        {
            for (int i = 0; i < _names.Length; i++)
            {
                try
                {
                    Image original = Image.FromFile(_names[i]);
                    string newName = $"{_outputPath}\\{i + 1}.bmp";
                    original.Save(newName, ImageFormat.Bmp);
                }
                catch
                {
                }
                
                this.progressBarGeneration.BeginInvoke((MethodInvoker)
                    (() => this.progressBarGeneration.Value = (int)(i * 1000.0 / _names.Length)));
            }
            this.progressBarGeneration.BeginInvoke((MethodInvoker)
                    (() => this.progressBarGeneration.Value = 0));
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (_conversionThread != null)
            {
                _conversionThread.Abort();
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
