using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Exit
{
    [Table("ISM_Resignation_ApprovedBy")]
    public class ISM_Resignation_ApprovedBy_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRESGAB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMRESG_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMRESGAB_Date { get; set; }
        public string ISMRESGAB_Remarks { get; set; }
        public bool ISMRESGAB_ActiveFlag { get; set; }
        public DateTime ISMRESGAB_CreatedDate { get; set; }
        public DateTime ISMRESGAB_UpdatedDate { get; set; }
        public DateTime? ISMRESGAB_RelievingDate { get; set; }
        public long ISMRESGAB_CreatedBy { get; set; }
        public long ISMRESGAB_UpdatedBy { get; set; }
        public string ISMRESGAB_AppRejFlg { get; set; }
        public decimal? ISMRESGAB_DamageAmount { get; set; }
        public string ISMRESGAB_DamageRemarks { get; set; }

    }
}
