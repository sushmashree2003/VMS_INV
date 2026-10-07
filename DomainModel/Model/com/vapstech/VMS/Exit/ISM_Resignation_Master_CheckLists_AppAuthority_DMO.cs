using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Exit
{
    [Table("ISM_Resignation_Master_CheckLists_AppAuthority")]
    public class ISM_Resignation_Master_CheckLists_AppAuthority_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRESGMCLAPAUT_Id { get; set; }
        public long ISMRESGMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public bool ISMRESGMCLAPAUT_ActiveFlag { get; set; }
        public DateTime? ISMRESGMCLAPAUT_CreatedDate { get; set; }
        public DateTime? ISMRESGMCLAPAUT_UpdatedDate { get; set; }
        public long ISMRESGMCLAPAUT_CreatedBy { get; set; }
        public long ISMRESGMCLAPAUT_UpdatedBy { get; set; }

    }
}
