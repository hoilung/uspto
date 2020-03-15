using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace uspto.Controls
{
    public partial class UcVerify : UcBase
    {
        public UcVerify()
        {
            InitializeComponent();
            this.tbx_userid.Text = UserID;
        }

        private void btn_verify_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "授权文件|*.cert|全部文件|*.*";
            openFile.DefaultExt = "cert";
            openFile.CheckFileExists = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                tbx_license.Text = openFile.FileName;

                if (MessageBox.Show("受否验证授权文件？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        File.Copy(openFile.FileName, Path.Combine(Directory.GetCurrentDirectory(), Properties.Resources.license), true);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("文件可能被其他程序占用，请关闭占用的程序或请复制授权文件到软件目录下", "提示", MessageBoxButtons.OK);
                        return;
                    }
                    if (LicenseVerify())
                    {
                        lb_msg.Text = "验证授权成功,请重新打开软件使用";
                    }
                    else
                    {
                        MessageBox.Show("错误的授权文件或无效文件,请重新选择", "提示", MessageBoxButtons.OK);
                    }
                }
            }
        }

    }
}
