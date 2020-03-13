namespace uspto.Controls
{
    partial class UcVerify
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tbx_userid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_msg = new System.Windows.Forms.Label();
            this.btn_verify = new System.Windows.Forms.Button();
            this.tbx_license = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbx_userid
            // 
            this.tbx_userid.Location = new System.Drawing.Point(76, 46);
            this.tbx_userid.Name = "tbx_userid";
            this.tbx_userid.ReadOnly = true;
            this.tbx_userid.Size = new System.Drawing.Size(375, 21);
            this.tbx_userid.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "机器码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "授权文件";
            // 
            // lb_msg
            // 
            this.lb_msg.AutoSize = true;
            this.lb_msg.Location = new System.Drawing.Point(74, 141);
            this.lb_msg.Name = "lb_msg";
            this.lb_msg.Size = new System.Drawing.Size(53, 12);
            this.lb_msg.TabIndex = 5;
            this.lb_msg.Text = "验证信息";
            // 
            // btn_verify
            // 
            this.btn_verify.Location = new System.Drawing.Point(376, 88);
            this.btn_verify.Name = "btn_verify";
            this.btn_verify.Size = new System.Drawing.Size(75, 23);
            this.btn_verify.TabIndex = 6;
            this.btn_verify.Text = "导入授权";
            this.btn_verify.UseVisualStyleBackColor = true;
            this.btn_verify.Click += new System.EventHandler(this.btn_verify_Click);
            // 
            // tbx_license
            // 
            this.tbx_license.Location = new System.Drawing.Point(76, 90);
            this.tbx_license.Name = "tbx_license";
            this.tbx_license.ReadOnly = true;
            this.tbx_license.Size = new System.Drawing.Size(294, 21);
            this.tbx_license.TabIndex = 1;
            // 
            // UcVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_verify);
            this.Controls.Add(this.lb_msg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbx_license);
            this.Controls.Add(this.tbx_userid);
            this.Name = "UcVerify";
            this.Size = new System.Drawing.Size(500, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbx_userid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_msg;
        private System.Windows.Forms.Button btn_verify;
        private System.Windows.Forms.TextBox tbx_license;
    }
}
