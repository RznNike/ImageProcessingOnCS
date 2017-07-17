namespace ImageProcessingOnSharp {
    partial class MainForm {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent( ) {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpenImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSaveResult = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsActions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmApplyAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtbStatistic = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pboxOriginal = new System.Windows.Forms.PictureBox();
            this.pboxResult = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnApplyAlgorithm = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelAlgorithm = new System.Windows.Forms.Panel();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panelQuality = new System.Windows.Forms.Panel();
            this.nudQualityLevel = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panelCompression = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCompression = new System.Windows.Forms.ComboBox();
            this.menuStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxResult)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelAlgorithm.SuspendLayout();
            this.panelQuality.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQualityLevel)).BeginInit();
            this.panelCompression.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsFile,
            this.tsActions});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(689, 24);
            this.menuStrip.TabIndex = 13;
            this.menuStrip.Text = "menuStrip1";
            // 
            // tsFile
            // 
            this.tsFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOpenImage,
            this.tsmSaveResult,
            this.toolStripSeparator1,
            this.tsmExit});
            this.tsFile.Name = "tsFile";
            this.tsFile.Size = new System.Drawing.Size(37, 20);
            this.tsFile.Text = "File";
            // 
            // tsmOpenImage
            // 
            this.tsmOpenImage.Name = "tsmOpenImage";
            this.tsmOpenImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmOpenImage.Size = new System.Drawing.Size(182, 22);
            this.tsmOpenImage.Text = "Open image";
            this.tsmOpenImage.Click += new System.EventHandler(this.tsmOpenImage_Click);
            // 
            // tsmSaveResult
            // 
            this.tsmSaveResult.Name = "tsmSaveResult";
            this.tsmSaveResult.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmSaveResult.Size = new System.Drawing.Size(182, 22);
            this.tsmSaveResult.Text = "Save result";
            this.tsmSaveResult.Click += new System.EventHandler(this.tsmSaveResult_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // tsmExit
            // 
            this.tsmExit.Name = "tsmExit";
            this.tsmExit.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.X)));
            this.tsmExit.Size = new System.Drawing.Size(182, 22);
            this.tsmExit.Text = "Exit";
            this.tsmExit.Click += new System.EventHandler(this.tsmExit_Click);
            // 
            // tsActions
            // 
            this.tsActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmApplyAlgorithm});
            this.tsActions.Name = "tsActions";
            this.tsActions.Size = new System.Drawing.Size(59, 20);
            this.tsActions.Text = "Actions";
            // 
            // tsmApplyAlgorithm
            // 
            this.tsmApplyAlgorithm.Name = "tsmApplyAlgorithm";
            this.tsmApplyAlgorithm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.tsmApplyAlgorithm.Size = new System.Drawing.Size(202, 22);
            this.tsmApplyAlgorithm.Text = "Apply algorithm";
            this.tsmApplyAlgorithm.Click += new System.EventHandler(this.tsmApplyAlgorithm_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.pboxOriginal, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pboxResult, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnLoad, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnApplyAlgorithm, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.19048F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(689, 512);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 5);
            this.groupBox1.Controls.Add(this.rtbStatistic);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 434);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(683, 75);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statistic:";
            // 
            // rtbStatistic
            // 
            this.rtbStatistic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbStatistic.Location = new System.Drawing.Point(3, 16);
            this.rtbStatistic.Name = "rtbStatistic";
            this.rtbStatistic.ReadOnly = true;
            this.rtbStatistic.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtbStatistic.Size = new System.Drawing.Size(677, 56);
            this.rtbStatistic.TabIndex = 1;
            this.rtbStatistic.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(317, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 298);
            this.label1.TabIndex = 15;
            this.label1.Text = "→";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pboxOriginal
            // 
            this.pboxOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pboxOriginal, 2);
            this.pboxOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pboxOriginal.Location = new System.Drawing.Point(3, 3);
            this.pboxOriginal.Name = "pboxOriginal";
            this.pboxOriginal.Size = new System.Drawing.Size(308, 292);
            this.pboxOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pboxOriginal.TabIndex = 16;
            this.pboxOriginal.TabStop = false;
            // 
            // pboxResult
            // 
            this.pboxResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pboxResult, 2);
            this.pboxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pboxResult.Location = new System.Drawing.Point(377, 3);
            this.pboxResult.Name = "pboxResult";
            this.pboxResult.Size = new System.Drawing.Size(309, 292);
            this.pboxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pboxResult.TabIndex = 17;
            this.pboxResult.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 298);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 40);
            this.label2.TabIndex = 18;
            this.label2.Text = "Original:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLoad
            // 
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Location = new System.Drawing.Point(95, 301);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(216, 34);
            this.btnLoad.TabIndex = 19;
            this.btnLoad.Text = "choose image file";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.tsmOpenImage_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(377, 298);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 40);
            this.label3.TabIndex = 20;
            this.label3.Text = "Result:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApplyAlgorithm
            // 
            this.btnApplyAlgorithm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApplyAlgorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnApplyAlgorithm.Location = new System.Drawing.Point(469, 301);
            this.btnApplyAlgorithm.Name = "btnApplyAlgorithm";
            this.btnApplyAlgorithm.Size = new System.Drawing.Size(217, 34);
            this.btnApplyAlgorithm.TabIndex = 21;
            this.btnApplyAlgorithm.Text = "apply algorithm & back";
            this.btnApplyAlgorithm.UseMnemonic = false;
            this.btnApplyAlgorithm.UseVisualStyleBackColor = true;
            this.btnApplyAlgorithm.Click += new System.EventHandler(this.tsmApplyAlgorithm_Click);
            // 
            // groupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 5);
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 341);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 87);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panelAlgorithm);
            this.flowLayoutPanel1.Controls.Add(this.panelQuality);
            this.flowLayoutPanel1.Controls.Add(this.panelCompression);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(677, 68);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panelAlgorithm
            // 
            this.panelAlgorithm.AutoSize = true;
            this.panelAlgorithm.Controls.Add(this.cmbAlgorithm);
            this.panelAlgorithm.Controls.Add(this.label4);
            this.panelAlgorithm.Location = new System.Drawing.Point(3, 3);
            this.panelAlgorithm.Name = "panelAlgorithm";
            this.panelAlgorithm.Size = new System.Drawing.Size(186, 28);
            this.panelAlgorithm.TabIndex = 0;
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Location = new System.Drawing.Point(62, 4);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(121, 21);
            this.cmbAlgorithm.TabIndex = 1;
            this.cmbAlgorithm.SelectedValueChanged += new System.EventHandler(this.cmbAlgorithm_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Algorithm:";
            // 
            // panelQuality
            // 
            this.panelQuality.AutoSize = true;
            this.panelQuality.Controls.Add(this.nudQualityLevel);
            this.panelQuality.Controls.Add(this.label5);
            this.panelQuality.Location = new System.Drawing.Point(195, 3);
            this.panelQuality.Name = "panelQuality";
            this.panelQuality.Size = new System.Drawing.Size(90, 28);
            this.panelQuality.TabIndex = 1;
            // 
            // nudQualityLevel
            // 
            this.nudQualityLevel.Location = new System.Drawing.Point(46, 5);
            this.nudQualityLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQualityLevel.Name = "nudQualityLevel";
            this.nudQualityLevel.Size = new System.Drawing.Size(41, 20);
            this.nudQualityLevel.TabIndex = 1;
            this.nudQualityLevel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Quality:";
            // 
            // panelCompression
            // 
            this.panelCompression.AutoSize = true;
            this.panelCompression.Controls.Add(this.cmbCompression);
            this.panelCompression.Controls.Add(this.label6);
            this.panelCompression.Location = new System.Drawing.Point(291, 3);
            this.panelCompression.Name = "panelCompression";
            this.panelCompression.Size = new System.Drawing.Size(146, 28);
            this.panelCompression.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Compression:";
            // 
            // cmbCompression
            // 
            this.cmbCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompression.FormattingEnabled = true;
            this.cmbCompression.Location = new System.Drawing.Point(71, 4);
            this.cmbCompression.Name = "cmbCompression";
            this.cmbCompression.Size = new System.Drawing.Size(72, 21);
            this.cmbCompression.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 536);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(705, 575);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageProcessing";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pboxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxResult)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panelAlgorithm.ResumeLayout(false);
            this.panelAlgorithm.PerformLayout();
            this.panelQuality.ResumeLayout(false);
            this.panelQuality.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQualityLevel)).EndInit();
            this.panelCompression.ResumeLayout(false);
            this.panelCompression.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsFile;
        private System.Windows.Forms.ToolStripMenuItem tsmOpenImage;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveResult;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pboxOriginal;
        private System.Windows.Forms.PictureBox pboxResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnApplyAlgorithm;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem tsActions;
        private System.Windows.Forms.ToolStripMenuItem tsmApplyAlgorithm;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelAlgorithm;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Panel panelQuality;
        private System.Windows.Forms.NumericUpDown nudQualityLevel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rtbStatistic;
        private System.Windows.Forms.Panel panelCompression;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCompression;
    }
}

