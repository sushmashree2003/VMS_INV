using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class ISM_Checklist_ApprovalDTO
    {
        public long ISMCLA_Id { get; set; }
        public long ISMCLD_Id { get; set; }
        public long HRMF_Id { get; set; }
        public long ISMCLD_Qty { get; set; }
        public DateTime? ISMCLT_Date { get; set; }
        public long? ISMCLT_UserId { get; set; }
        public long ISMCLT_Qty { get; set; }
        public string ISMCLT_Remarks { get; set; }
        public string ISMCL_Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? ISMCLT_CreatedBy { get; set; }
        public long? ISMCLT_UpdatedBy { get; set; }
        public decimal ISMCLD_MorningQty { get; set; }
        public decimal ISMCLD_EveningQty { get; set; }
    }
}
