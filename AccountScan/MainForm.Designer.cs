namespace AccountScan
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.ExitButton = new System.Windows.Forms.Button();
            this.PictureListBox = new System.Windows.Forms.ListBox();
            this.AccountPictureBox = new System.Windows.Forms.PictureBox();
            this.DetectButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AccountPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Location = new System.Drawing.Point(906, 685);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(90, 32);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "Cancel";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // PictureListBox
            // 
            this.PictureListBox.AllowDrop = true;
            this.PictureListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PictureListBox.FormattingEnabled = true;
            this.PictureListBox.ItemHeight = 21;
            this.PictureListBox.Location = new System.Drawing.Point(12, 12);
            this.PictureListBox.Name = "PictureListBox";
            this.PictureListBox.Size = new System.Drawing.Size(120, 697);
            this.PictureListBox.TabIndex = 1;
            this.PictureListBox.SelectedIndexChanged += new System.EventHandler(this.PictureListBox_SelectedIndexChanged);
            this.PictureListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureListBox_DragDrop);
            this.PictureListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureListBox_DragEnter);
            // 
            // AccountPictureBox
            // 
            this.AccountPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountPictureBox.Location = new System.Drawing.Point(138, 12);
            this.AccountPictureBox.Name = "AccountPictureBox";
            this.AccountPictureBox.Size = new System.Drawing.Size(858, 667);
            this.AccountPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AccountPictureBox.TabIndex = 2;
            this.AccountPictureBox.TabStop = false;
            // 
            // DetectButton
            // 
            this.DetectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DetectButton.BackColor = System.Drawing.Color.PaleGreen;
            this.DetectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DetectButton.Location = new System.Drawing.Point(810, 685);
            this.DetectButton.Name = "DetectButton";
            this.DetectButton.Size = new System.Drawing.Size(90, 32);
            this.DetectButton.TabIndex = 3;
            this.DetectButton.Text = "Detect";
            this.DetectButton.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.DetectButton);
            this.Controls.Add(this.AccountPictureBox);
            this.Controls.Add(this.PictureListBox);
            this.Controls.Add(this.ExitButton);
            this.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.AccountPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.ListBox PictureListBox;
        private System.Windows.Forms.PictureBox AccountPictureBox;
        private System.Windows.Forms.Button DetectButton;
    }
}

