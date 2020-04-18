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
        public string selectedDocId { get; set; }
        public CaseDoc[] caseDocs { get; set; }

        public string GetPdfDownloadUrl(CaseDoc caseDoc)
        {
            if (caseDoc != null && caseDoc.urlPathList != null && caseId.Length > 0)
            {
                return $"https://tsdrsec.uspto.gov/ts/cd/casedoc/{caseId}/{caseDoc.docId}/download.pdf";
            }

            return string.Empty;
        }
    }

    public class CaseDoc
    {
        public string docId { get; set; }
        public string docIdShort { get; set; }
        public string pageCount { get; set; }
        public string description { get; set; }
        public string displayDate { get; set; }
        public string[] urlPathList { get; set; }
        public string[] mediaTypeList { get; set; }
    }
}
