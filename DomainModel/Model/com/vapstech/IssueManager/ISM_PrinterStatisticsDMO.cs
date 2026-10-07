using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_PrintOut_Details")]
    public class ISM_PrinterStatisticsDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long VMSPOD_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long MI_Id { get; set; }

        public DateTime VMSPOD_PODate { get; set; }
        public long VMSPOD_NoOfPages { get; set; }

        public string VMSPOD_Remarks { get; set; }
        public string VMSPOD_Type { get; set; }
        public DateTime VMSPOD_CreatedDate { get; set; }

        public  DateTime VMSPOD_UpdatedDate { get; set; }

        public long VMSPOD_CreatedBy { get; set; }
        public long VMSPOD_UpdatedBy { get; set; }

        public bool VMSPOD_ActiveFlag { get; set; }

        public long VMSMPR_Id { get; set; }

        public DateTime? VMSPOD_FromDate { get; set; }

        public DateTime?  VMSPOD_ToDate { get; set; }

    //public long User_Id { get; set; }

}
}
