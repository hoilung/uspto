using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uspto.Models
{
    public class Patent
    {
        public Patent()
        {
            this.TEASPlusNewApplication.Add("INTERNATIONAL CLASS", "");
            this.TEASPlusNewApplication.Add("IDENTIFICATION", "");

            this.TEASPlusNewApplication.Add("NAME", "");
            this.TEASPlusNewApplication.Add("STREET", "");
            this.TEASPlusNewApplication.Add("CITY", "");
            this.TEASPlusNewApplication.Add("STATE", "");
            this.TEASPlusNewApplication.Add("COUNTRY", "");
            this.TEASPlusNewApplication.Add("ZIP", "");
            this.TEASPlusNewApplication.Add("PHONE", "");            
            this.TEASPlusNewApplication.Add("EMAIL ADDRESS", "");
            this.TEASPlusNewApplication.Add("AUTHORIZED TO COMMUNICATE VIA EMAIL", "");
            this.TEASPlusNewApplication.Add("APPLICATION FILING OPTION", "");
            this.TEASPlusNewApplication.Add("NUMBER OF CLASSES", "");
            this.TEASPlusNewApplication.Add("FEE PER CLASS", "");
            this.TEASPlusNewApplication.Add("TOTAL FEE PAID", "");
            this.TEASPlusNewApplication.Add("SIGNATURE", "");
            this.TEASPlusNewApplication.Add("SIGNATORY'S NAME", "");
            this.TEASPlusNewApplication.Add("SIGNATORY'S POSITION", "");
            this.TEASPlusNewApplication.Add("DATE SIGNED", "");

        }
        public string Name { get; set; } = string.Empty;

        public string Num { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string StatusDate { get; set; } = string.Empty;

        public string PublicationDate { get; set; } = string.Empty;

        public string PdfFile { get; set; } = string.Empty;

        public string[] DocDates { get; set; }

        public Dictionary<string, string> TEASPlusNewApplication { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 复生文件html地址
        /// </summary>
        public string OffcActionFile { get; internal set; }
    }
}
