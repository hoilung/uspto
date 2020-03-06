using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uspto.Models
{
    public class DocsList
    {
        public string caseId { get; set; }
        public CaseDoc[] caseDocs { get; set; }
    }

    public class CaseDoc
    {
        public string description { get; set; }
        public string displayDate { get; set; }
        public string[] urlPathList { get; set; }
        public string[] mediaTypeList { get; set; }
    }
}
