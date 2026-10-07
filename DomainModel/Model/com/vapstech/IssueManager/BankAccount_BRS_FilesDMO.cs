using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Payment_Voucher_Files")]
    public class BankAccount_BRS_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VPAYVOUF_Id { get; set; }
        public string VPAYVOUF_FileName { get; set; }
        public string VPAYVOUF_Filedesc { get; set; }
        public string VPAYVOUF_FilePath { get; set; }
        public long VPAYVOU_Id { get; set; }
        public string VPAYVOUF_StatusFlg { get; set; }
        public bool VPAYVOUF_ActiveFlg { get; set; }

    }
}
