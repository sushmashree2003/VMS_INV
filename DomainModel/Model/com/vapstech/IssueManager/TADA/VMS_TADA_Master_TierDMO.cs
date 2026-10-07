using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Master_Tier")]
    public class VMS_TADA_Master_TierDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VRMMDT_Id { get; set; }
        public string VRMMDT_Name { get; set; }
        public bool VRMMDT_ActiveFlag { get; set; }
        public long VRMMDT_CreatedBy { get; set; }
        public long VRMMDT_UpdatedBy { get; set; }
        public DateTime VRMMDT_CreatedDate { get; set; }
        public DateTime VRMMDT_UpdatedDate { get; set; }
      public List<VMSTADAMasterCityDMO> VMSTADAMasterCityDMO { get; set; }
    }
}
