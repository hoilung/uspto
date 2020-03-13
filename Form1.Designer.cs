namespace uspto
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ucStatusSearch1 = new uspto.Controls.UcStatusSearch();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ucVerify1 = new uspto.Controls.UcVerify();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(516, 377);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ucStatusSearch1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(508, 351);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "专利查询";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ucStatusSearch1
            // 
            this.ucStatusSearch1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStatusSearch1.Location = new System.Drawing.Point(3, 3);
            this.ucStatusSearch1.Name = "ucStatusSearch1";
            this.ucStatusSearch1.Size = new System.Drawing.Size(502, 345);
            this.ucStatusSearch1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ucVerify1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(508, 351);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "授权验证";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ucVerify1
            // 
            this.ucVerify1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucVerify1.Location = new System.Drawing.Point(3, 3);
            this.ucVerify1.Name = "ucVerify1";
            this.ucVerify1.Size = new System.Drawing.Size(502, 345);
            this.ucVerify1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 377);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "uspto";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Controls.UcStatusSearch ucStatusSearch1;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.UcVerify ucVerify1;
    }
}

