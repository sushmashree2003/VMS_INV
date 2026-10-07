using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Master_Travel_Details")]
    public  class VMS_TADA_Master_Travel_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADAMD_Id { get; set; }
        public long VTADAMT_Id { get; set; }
        public string VTADAMD_Name { get; set; }
        public string VTADAMD_Description { get; set; }
        public bool VTADAMD_ActiveFlag { get; set; }
        public long VTADAMD_CreatedBy { get; set; }
        public long VTADAMD_UpdatedBy { get; set; }
        public DateTime VTADAMD_CreatedDate { get; set; }
        public DateTime VTADAMD_UpdatedDate { get; set; }
    }
}
