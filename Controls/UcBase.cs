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
    public partial class UcBase : UserControl
    {

        protected readonly string UserID;        
        public UcBase()
        {
            InitializeComponent();

            try
            {
                var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/uspto";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (!File.Exists(Path.Combine(dir, Properties.Resources.userid)))
                {
                    File.AppendAllText(Path.Combine(dir, Properties.Resources.userid), Guid.NewGuid().ToString("N").ToUpper(), Encoding.UTF8);
                }
                UserID = File.ReadAllText(Path.Combine(dir, Properties.Resources.userid));

            }
            catch (Exception)
            {
                MessageBox.Show("启动错误，软件将关闭", "错误");
                this.FindForm().Close();
            }
        }

        public bool LicenseVerify()
        {
            var result = false;
            var licenseFile = Path.Combine(Directory.GetCurrentDirectory(), Properties.Resources.license);
            if (File.Exists(licenseFile))
            {
                var pub = new Encrypt.RSA.PublicRSA(Convert.FromBase64String(Properties.Resources.pub));
                var signdata = File.ReadAllBytes(licenseFile);
                var data = Convert.FromBase64String(UserID);
                result = pub.Verify(data, signdata);
            }            
            return result;
        }

    }
}
