using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class ISM_Checklist_DetailsDTO
    {
        public long ISMCLD_Id { get; set; }
        public long ISMCLT_Id { get; set; }
        public string ISMCL_Name { get; set; }
        public long ISMCL_UserId { get; set; }
        public bool ISMCL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long HRMF_Id { get; set; }
        public long ISMCLD_Qty { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long ISMCLT_Qty { get; set; }
        public decimal? ISMCLD_MorningQty { get; set; }
        public decimal? ISMCLD_EveningQty { get; set; }
        public string ISMCLT_FilePath { get; set; }
        public Array checklisttypeList { get; set; }
        public Array floorList { get; set; }
        public string retrunMsg { get; set; }
        public Array checklistmappedList { get; set; }
        public string ISMCLT_Name { get; set; }
        public string HRMF_FloorName { get; set; }
        public string ISMCLT_Remarks { get; set; }
        public ISM_Checklist_ApprovalDTO[] ApprovalTempDTO { get; set; }
        public long ISMCLA_Id { get; set; }
        public Array columnnames { get; set; }
        public Array filldata { get; set; }
        public DateTime myDate_from { get; set; }
        public DateTime myDate_to { get; set; }
        public DateTime? ISMCLT_Date { get; set; }
        public string userrole { get; set; }
        public long ISMCLT_UserId { get; set; }
        public Array userrolearray { get; set; }
       
        public Array masterdit { get; set; }
    }
}
