namespace uspto.Controls
{
    partial class UcStatusSearch
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
            this.tbx_nums = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.btn_load = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_staus = new System.Windows.Forms.Label();
            this.pb_nums = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_down = new System.Windows.Forms.Label();
            this.pb_down = new System.Windows.Forms.ProgressBar();
            this.btn_down = new System.Windows.Forms.Button();
            this.btn_export = new System.Windows.Forms.Button();
            this.tbx_rst = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbx_nums
            // 
            this.tbx_nums.Location = new System.Drawing.Point(6, 20);
            this.tbx_nums.Multiline = true;
            this.tbx_nums.Name = "tbx_nums";
            this.tbx_nums.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_nums.Size = new System.Drawing.Size(384, 96);
            this.tbx_nums.TabIndex = 1;
            this.tbx_nums.WordWrap = false;
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(395, 22);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 2;
            this.btn_search.Text = "查询";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(395, 51);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(75, 23);
            this.btn_load.TabIndex = 3;
            this.btn_load.Text = "载入";
            this.btn_load.UseVisualStyleBackColor = true;
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(395, 80);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(75, 23);
            this.btn_clear.TabIndex = 4;
            this.btn_clear.Text = "清空";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lb_staus);
            this.groupBox1.Controls.Add(this.pb_nums);
            this.groupBox1.Controls.Add(this.tbx_nums);
            this.groupBox1.Controls.Add(this.btn_clear);
            this.groupBox1.Controls.Add(this.btn_search);
            this.groupBox1.Controls.Add(this.btn_load);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(490, 140);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询";
            // 
            // lb_staus
            // 
            this.lb_staus.AutoSize = true;
            this.lb_staus.Location = new System.Drawing.Point(396, 122);
            this.lb_staus.Name = "lb_staus";
            this.lb_staus.Size = new System.Drawing.Size(41, 12);
            this.lb_staus.TabIndex = 6;
            this.lb_staus.Text = "待查询";
            // 
            // pb_nums
            // 
            this.pb_nums.Location = new System.Drawing.Point(6, 122);
            this.pb_nums.Name = "pb_nums";
            this.pb_nums.Size = new System.Drawing.Size(384, 12);
            this.pb_nums.Step = 1;
            this.pb_nums.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_down);
            this.groupBox2.Controls.Add(this.pb_down);
            this.groupBox2.Controls.Add(this.btn_down);
            this.groupBox2.Controls.Add(this.btn_export);
            this.groupBox2.Controls.Add(this.tbx_rst);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(490, 194);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "结果";
            // 
            // lb_down
            // 
            this.lb_down.AutoSize = true;
            this.lb_down.Location = new System.Drawing.Point(396, 174);
            this.lb_down.Name = "lb_down";
            this.lb_down.Size = new System.Drawing.Size(41, 12);
            this.lb_down.TabIndex = 4;
            this.lb_down.Text = "待下载";
            // 
            // pb_down
            // 
            this.pb_down.Location = new System.Drawing.Point(6, 174);
            this.pb_down.Name = "pb_down";
            this.pb_down.Size = new System.Drawing.Size(384, 12);
            this.pb_down.Step = 0;
            this.pb_down.TabIndex = 3;
            // 
            // btn_down
            // 
            this.btn_down.Location = new System.Drawing.Point(396, 49);
            this.btn_down.Name = "btn_down";
            this.btn_down.Size = new System.Drawing.Size(75, 23);
            this.btn_down.TabIndex = 2;
            this.btn_down.Text = "下载PDF";
            this.btn_down.UseVisualStyleBackColor = true;
            this.btn_down.Click += new System.EventHandler(this.btn_down_Click);
            // 
            // btn_export
            // 
            this.btn_export.Location = new System.Drawing.Point(395, 20);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(75, 23);
            this.btn_export.TabIndex = 1;
            this.btn_export.Text = "导出";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // tbx_rst
            // 
            this.tbx_rst.Location = new System.Drawing.Point(6, 20);
            this.tbx_rst.Multiline = true;
            this.tbx_rst.Name = "tbx_rst";
            this.tbx_rst.ReadOnly = true;
            this.tbx_rst.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_rst.Size = new System.Drawing.Size(384, 148);
            this.tbx_rst.TabIndex = 0;
            this.tbx_rst.WordWrap = false;
            // 
            // UcStatusSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "UcStatusSearch";
            this.Size = new System.Drawing.Size(490, 340);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox tbx_nums;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbx_rst;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.ProgressBar pb_nums;
        private System.Windows.Forms.Label lb_staus;
        private System.Windows.Forms.Button btn_down;
        private System.Windows.Forms.ProgressBar pb_down;
        private System.Windows.Forms.Label lb_down;
    }
}
