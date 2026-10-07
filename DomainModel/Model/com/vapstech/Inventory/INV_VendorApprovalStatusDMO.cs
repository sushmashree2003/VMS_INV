using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_VendorApprovalStatus")]
    public class INV_VendorApprovalStatusDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VAS_Id { get; set; }

        public long MI_Id { get; set; }

        public string ApprovalStatus { get; set; }

        public DateTime? ActionDate { get; set; }

        public long MVE_Id { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
