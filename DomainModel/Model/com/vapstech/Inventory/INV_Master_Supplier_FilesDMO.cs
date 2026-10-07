using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{

    [Table("INV_Master_Supplier_Files", Schema = "INV")]
    public  class INV_Master_Supplier_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSF_Id_Id { get; set; }
        public long INVMS_Id { get; set; }
        public string INVMSF_FileName { get; set; }
        public string INVMSF_Filedesc { get; set; }
        public string INVMSF_FilePath { get; set; }
        public string INVMSF_StatusFlg { get; set; }
        public bool INVMSF_ActiveFlg { get; set; }
        public DateTime? INVMSF_CreatedDate { get; set; }
        public DateTime? INVMSF_UpdatedDate { get; set; }
        public long INVMSF_CreatedBy { get; set; }
        public long INVMSF_UpdatedBy { get; set; }
    }
}
