using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Correspondence_Client")]
    public class FMS_Correspondence_ClientDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMSCORCL_Id {get;set;}
        public long FMSCOR_Id { get;set;}
        public long ISMMCLT_Id { get;set;}
        public long? ISMMPR_Id { get;set;}
        public bool FMSCORCL_ActiveFlg { get;set;}
        public DateTime? FMSCORCL_CreatedDate { get;set;}
        public DateTime? FMSCORCL_UpdatedDate { get;set;}
        public long? FMSCORCL_CreatedBy { get;set;}
        public long? FMSCORCL_UpdatedBy { get;set;}
    }
}