using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uspto.Models
{
    public class Patent
    {
        public string Name { get; set; } = string.Empty;

        public string Num { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string StatusDate { get; set; } = string.Empty;

        public string PublicationDate { get; set; } = string.Empty;

        public string PdfFile { get; set; } = string.Empty;
    }
}
