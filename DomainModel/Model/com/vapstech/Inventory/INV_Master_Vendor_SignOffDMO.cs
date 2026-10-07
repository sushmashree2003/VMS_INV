using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Vendor_SignOff")]
    public class INV_Master_Vendor_SignOffDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVMSO_Id { get; set; }

        public long MI_ID { get; set; }

        public long MVE_Id { get; set; }

        public DateTime? INVMSO_SignOffDate { get; set; }

        public bool INVMSO_ActiveFlg { get; set; }

        public long INVMSO_CreatedBy { get; set; }

        public long INVMSO_UpdatedBy { get; set; }

        public DateTime? INVMSO_CreatedDate { get; set; }

        public DateTime? INVMSO_UpdatedDate { get; set; }

        public string INVMSO_MOU_FilePath { get; set; }

        public string INVMSO_WorkOrder_FilePath { get; set; }
    }
}
