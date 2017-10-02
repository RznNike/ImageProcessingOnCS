namespace ImageToBmpConverter
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDial = new System.Windows.Forms.OpenFileDialog();
            this.buttonChooseInput = new System.Windows.Forms.Button();
            this.buttonChooseOutput = new System.Windows.Forms.Button();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.progressBarGeneration = new System.Windows.Forms.ProgressBar();
            this.folderBrowserDial = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // openFileDial
            // 
            this.openFileDial.Multiselect = true;
            // 
            // buttonChooseInput
            // 
            this.buttonChooseInput.Location = new System.Drawing.Point(12, 12);
            this.buttonChooseInput.Name = "buttonChooseInput";
            this.buttonChooseInput.Size = new System.Drawing.Size(310, 23);
            this.buttonChooseInput.TabIndex = 0;
            this.buttonChooseInput.Text = "Choose input";
            this.buttonChooseInput.UseVisualStyleBackColor = true;
            this.buttonChooseInput.Click += new System.EventHandler(this.buttonChooseInput_Click);
            // 
            // buttonChooseOutput
            // 
            this.buttonChooseOutput.Location = new System.Drawing.Point(12, 51);
            this.buttonChooseOutput.Name = "buttonChooseOutput";
            this.buttonChooseOutput.Size = new System.Drawing.Size(310, 23);
            this.buttonChooseOutput.TabIndex = 1;
            this.buttonChooseOutput.Text = "Choose output";
            this.buttonChooseOutput.UseVisualStyleBackColor = true;
            this.buttonChooseOutput.Click += new System.EventHandler(this.buttonChooseOutput_Click);
            // 
            // buttonConvert
            // 
            this.buttonConvert.Location = new System.Drawing.Point(12, 89);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(310, 23);
            this.buttonConvert.TabIndex = 2;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(12, 129);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(310, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // progressBarGeneration
            // 
            this.progressBarGeneration.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarGeneration.Location = new System.Drawing.Point(0, 160);
            this.progressBarGeneration.Maximum = 1000;
            this.progressBarGeneration.Name = "progressBarGeneration";
            this.progressBarGeneration.Size = new System.Drawing.Size(334, 23);
            this.progressBarGeneration.Step = 1;
            this.progressBarGeneration.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarGeneration.TabIndex = 4;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 183);
            this.Controls.Add(this.progressBarGeneration);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.buttonChooseOutput);
            this.Controls.Add(this.buttonChooseInput);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 222);
            this.MinimumSize = new System.Drawing.Size(350, 222);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image to BMP converter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDial;
        private System.Windows.Forms.Button buttonChooseInput;
        private System.Windows.Forms.Button buttonChooseOutput;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ProgressBar progressBarGeneration;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDial;
    }
}

