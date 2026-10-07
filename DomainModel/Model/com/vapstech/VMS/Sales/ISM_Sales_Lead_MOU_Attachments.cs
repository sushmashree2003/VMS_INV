using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sale_MOU_Attachments")]
    public class ISM_Sales_Lead_MOU_Attachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSMOUA_Id  { get; set; }
        public long ISMSMOU_Id { get; set; }
        public string ISMSMOUA_AttachmentName { get; set; }
        public string ISMSMOUA_AttachmentPath { get; set; }
        public bool ISMSMOUA_ActiveFlag { get; set; }
        public DateTime? ISMSMOUA_CreatedDate { get; set; }
        public DateTime? ISMSMOUA_UpdatedDate { get; set; }
        public long ISMSMOUA_CreatedBy { get; set; }
        public long ISMSMOUA_UpdatedBy { get; set; }
    }
}
