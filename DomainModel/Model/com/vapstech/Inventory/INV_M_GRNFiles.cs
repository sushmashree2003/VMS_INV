using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_GRN_Files", Schema = "INV")]
    public class INV_M_GRNFiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMGRNF_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public string INVMGRNF_FileName { get; set; }
        public string INVMGRNF_Filedesc { get; set; }
        public string INVMGRNF_FilePath { get; set; }
        public string INVMGRNF_StatusFlg { get; set; }
        public bool INVMGRNF_ActiveFlg { get; set; }
        public DateTime? INVMGRNF_CreatedDate { get; set; }
        public DateTime? INVMGRNF_UpdatedDate { get; set; }
    }
}
