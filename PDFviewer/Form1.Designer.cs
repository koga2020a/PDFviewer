namespace PDFviewer
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2_debug = new System.Windows.Forms.Label();
            this.label1_Message = new System.Windows.Forms.Label();
            this.line_4 = new System.Windows.Forms.Panel();
            this.line_3 = new System.Windows.Forms.Panel();
            this.line_2 = new System.Windows.Forms.Panel();
            this.line_1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.line_4);
            this.panel1.Controls.Add(this.line_3);
            this.panel1.Controls.Add(this.line_2);
            this.panel1.Controls.Add(this.line_1);
            this.panel1.Controls.Add(this.label2_debug);
            this.panel1.Controls.Add(this.label1_Message);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1464, 1183);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.pictureBox1.Location = new System.Drawing.Point(101, 604);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1360, 576);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label2_debug
            // 
            this.label2_debug.AutoSize = true;
            this.label2_debug.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2_debug.Location = new System.Drawing.Point(623, 816);
            this.label2_debug.Name = "label2_debug";
            this.label2_debug.Size = new System.Drawing.Size(222, 40);
            this.label2_debug.TabIndex = 4;
            this.label2_debug.Text = "label2_debug";
            // 
            // label1_Message
            // 
            this.label1_Message.AutoSize = true;
            this.label1_Message.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1_Message.Location = new System.Drawing.Point(606, 670);
            this.label1_Message.Name = "label1_Message";
            this.label1_Message.Size = new System.Drawing.Size(267, 40);
            this.label1_Message.TabIndex = 3;
            this.label1_Message.Text = "label1_Message";
            // 
            // line_4
            // 
            this.line_4.BackColor = System.Drawing.Color.DarkRed;
            this.line_4.Location = new System.Drawing.Point(931, 831);
            this.line_4.Margin = new System.Windows.Forms.Padding(4);
            this.line_4.Name = "line_4";
            this.line_4.Size = new System.Drawing.Size(352, 16);
            this.line_4.TabIndex = 10;
            this.line_4.Visible = false;
            // 
            // line_3
            // 
            this.line_3.BackColor = System.Drawing.Color.DarkRed;
            this.line_3.Location = new System.Drawing.Point(931, 795);
            this.line_3.Margin = new System.Windows.Forms.Padding(4);
            this.line_3.Name = "line_3";
            this.line_3.Size = new System.Drawing.Size(352, 16);
            this.line_3.TabIndex = 11;
            this.line_3.Visible = false;
            // 
            // line_2
            // 
            this.line_2.BackColor = System.Drawing.Color.DarkRed;
            this.line_2.Location = new System.Drawing.Point(931, 760);
            this.line_2.Margin = new System.Windows.Forms.Padding(4);
            this.line_2.Name = "line_2";
            this.line_2.Size = new System.Drawing.Size(352, 16);
            this.line_2.TabIndex = 12;
            this.line_2.Visible = false;
            // 
            // line_1
            // 
            this.line_1.BackColor = System.Drawing.Color.DarkRed;
            this.line_1.Location = new System.Drawing.Point(931, 725);
            this.line_1.Margin = new System.Windows.Forms.Padding(4);
            this.line_1.Name = "line_1";
            this.line_1.Size = new System.Drawing.Size(352, 16);
            this.line_1.TabIndex = 9;
            this.line_1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(2715, 1219);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Info;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDFviewer";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2_debug;
        private System.Windows.Forms.Label label1_Message;
        private System.Windows.Forms.Panel line_4;
        private System.Windows.Forms.Panel line_3;
        private System.Windows.Forms.Panel line_2;
        private System.Windows.Forms.Panel line_1;
    }
}

