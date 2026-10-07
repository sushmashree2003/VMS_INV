using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Master_ChequeBook")]
    public class VMSMaster_ChequeBookDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMCHEBK_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMBD_Id { get; set; }
        public DateTime? VMCHEBK_ReceivedDate { get; set; }
        public long VMCHEBK_NoOfChequeLeafs { get; set; }
        public string VMCHEBK_SLNoFrom { get; set; }
        public string VMCHEBK_SLNoTo { get; set; }
        public string VMCHEBK_Remarks { get; set; }
        public bool VMCHEBK_ActiveFlg { get; set; }
        public long VMCHEBK_CreatedBy { get; set; }
        public DateTime? VMCHEBK_CreatedDate { get; set; }
        public long VMCHEBK_UpdatedBy { get; set; }
        public DateTime? VMCHEBK_UpdatedDate { get; set; }
        public List<VMSMaster_ChequeBook_SLNoDMO> VMSMaster_ChequeBook_SLNoDMO { get; set; }

    }
}
