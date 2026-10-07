using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Comments")]
    public class ISM_Sales_Lead_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLECOM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public string ISMSLECOM_Comments { get; set; }
        public bool ISMSLECOM_ActiveFlag { get; set; }
        public DateTime? ISMSLECOM_CreatedDate { get; set; }
        public DateTime? ISMSLECOM_UpdatedDate { get; set; }
        public long ISMSLECOM_CreatedBy { get; set; }
        public long ISMSLECOM_UpdatedBy { get; set; }
        public string ISMSLE_FilePath { get; set; }        
        public DateTime? ISMSLECOM_RemainderDate { get; set; }
        public long? ISMSLECOM_RemainderHrs { get; set; }
        public string ISMSLECOM_RemainderTime { get; set; }
        public bool ISMSLECOM_SendProposalFlg { get; set; }
        public bool ISMSLECOM_SendQuotationFlg { get; set; }
        public DateTime? ISMSLECOM_NextReminderDate { get; set; }
        public List<ISM_Sales_Lead_CommentsFilesDMO> ISM_Sales_Lead_CommentsFilesDMO { get; set; }
    }
}
