using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uspto.Controls;

namespace uspto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = this.ProductName + " v" + this.ProductVersion;
            if (!ucStatusSearch1.LicenseVerify())
            {
                tabControl1.TabPages.Remove(tabPage1);
            }else
            {
                tabControl1.TabPages.Remove(tabPage2);
            }
        }
    }
}
