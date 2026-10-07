using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Accounts_Team_mapping")]
    public class ISM_Accounts_Team_mapping_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMACC_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        public long MI_Id { get; set; }


        public long ISMACC_CreatedBy { get; set; }
        public long ISMACC_UpdatedBy { get; set; }
        public DateTime? ISMACC_CreatedDate { get; set; }
        public DateTime? ISMACC_UpdatedDate { get; set; }

    }
}
