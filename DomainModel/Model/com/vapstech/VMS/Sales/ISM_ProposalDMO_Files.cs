using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_PostSales_Signup_Files")]
    public class ISM_ProposalDMO_Files
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMPSSUPF_Id { get; set; }
        public long ISMPSSUP_Id { get; set; }
        public string ISMPSSUPF_FileName { get; set; }
        public string ISMPSSUPF_FilePath { get; set; }
        public long ISMPSSUPF_CreatedBy { get; set; }
        public DateTime? ISMPSSUPF_CreatedDate { get; set; }
        public long ISMPSSUPF_UpdatedBy { get; set; }
        public DateTime? ISMPSSUPF_UpdatedDate { get; set; }

    }
}
