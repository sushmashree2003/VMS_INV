using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Exit
{
    public class ISM_Resignation_ChecKLists_DTO
    {
        public long ISMRESGCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public long ISMRESG_Id { get; set; }
        public long ISMRESGMCL_Id { get; set; }
        public string ISMRESGCL_FileName { get; set; }
        public string ISMRESGCL_FilePath { get; set; }
        public bool ISMRESGCL_ActiveFlag { get; set; }
        public DateTime ISMRESGCL_CreatedDate { get; set; }
        public DateTime ISMRESGCL_UpdatedDate { get; set; }
        public long ISMRESGCL_CreatedBy { get; set; }
        public long ISMRESGCL_UpdatedBy { get; set; }

        public string returnval { get; set; }
        // ISM_Resignation Checklist Approved By
        public long ISMRESGCLAPP_Id { get; set; }
        public string ISMRESGCLAPP_Remarks { get; set; }
        public string ISMRESGCLAPP_FileName { get; set; }
        public string ISMRESGCLAPP_FilePath { get; set; }
        public string ISMRESGCLAPP_Status { get; set; }
        public Array approvallist { get; set; }
        public Array viewchecklistdatas { get; set; }
        public ISM_Resignation_ChecKLists_DTO[] ChecklistArray { get; set; }


    }
}
