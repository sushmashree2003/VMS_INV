using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Master_ChequeBook_SLNo")]
    public class VMSMaster_ChequeBook_SLNoDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMCHEBKSLN_Id { get; set; }
        public long VMCHEBK_Id { get; set; }
        public string VMCHEBK_LeafNo { get; set; }
        public bool VMCHEBKSLN_CancelledFlg { get; set; }
        public string VMCHEBKSLN_Remarks { get; set; }
        public bool VMCHEBKSLN_ActiveFlg{get;set;}
        public long VMCHEBKSLN_CreatedBy { get; set; }
        public DateTime? VMCHEBKSLN_CreatedDate { get; set; }
        public long VMCHEBKSLN_UpdatedBy { get; set; }
        public DateTime? VMCHEBKSLN_UpdatedDate { get; set; }
    }
}
