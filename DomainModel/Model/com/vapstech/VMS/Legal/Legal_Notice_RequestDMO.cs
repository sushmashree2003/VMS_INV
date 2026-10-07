using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{

    [Table("Legal_Notice_Request")]
    public class Legal_Notice_RequestDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LNR_Id { get; set; }
        public long LNC_Id { get; set; }
        public long MI_ID { get; set; }
        public long RequestingFor_id { get; set; }
        public long ASMAY_ID { get; set; }
        public string LNR_Remarks { get; set; }
        public string LNR_Status { get; set; }
        public bool LNR_ActiveFlag { get; set; }
        public DateTime? LNR_CreatedDate { get; set; }
        public DateTime? LNR_UpdatedDate { get; set; }
        public long LNR_CreatedBy { get; set; }
        public long LNR_UpdatedBy { get; set; }     

    }
}
