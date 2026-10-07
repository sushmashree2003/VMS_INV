using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_VendorCategoryMaster")]
    public class INV_VendorCategoryMasterDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VCM_Id { get; set; }

        public long MI_Id { get; set; }

        public string VCM_Vendorname { get; set; } //category name

        public bool VCM_Activeflag { get; set; }

        public long VCM_Createdby { get; set; }

        public long VCM_Updatedby { get; set; }

        public DateTime VCM_Createddate { get; set; }

        public DateTime VCM_Updateddate { get; set; }


    }
}
