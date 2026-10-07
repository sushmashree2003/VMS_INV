using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Master_Travel")]
    public class VMS_TADA_Master_TravelDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADAMT_Id { get; set; }
        public string VTADAMT_Name { get; set; }
        public string VTADAMT_Description { get; set; }
        public bool VTADAMT_ActiveFlag { get; set; }
        public long VTADAMT_CreatedBy { get; set; }
        public long VTADAMT_UpdatedBy { get; set; }
        public DateTime VTADAMT_CreatedDate { get; set; }
        public DateTime VTADAMT_UpdatedDate { get; set; }
    }
}
