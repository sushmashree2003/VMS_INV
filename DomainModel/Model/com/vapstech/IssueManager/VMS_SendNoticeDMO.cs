using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_NoticeBoard")]
    public class VMS_SendNoticeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VNB_ID { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VNB_Title { get; set; }
        public string VNB_Description { get; set; }
        public string VNB_Type { get; set; }
        public bool VNB_ActiveFlag { get; set; }
        public string VNB_FileName { get; set; }
        public string VNB_FilePath { get; set; }
        public DateTime? VNB_CreatedDate { get; set; }
        public DateTime? VNB_UpdatedDate { get; set; }
        public long VNB_CreatedBy { get; set; }
        public long VNB_UpdatedBy { get; set; }


    }
}
