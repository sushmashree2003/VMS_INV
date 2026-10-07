using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_BOM")]
    public class ISM_Sales_BOMDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSBOM_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        public  string ISMSBOM_BOMFlg { get; set; }
        public string ISMSBOM_BOMName { get; set; }
        public string ISMSBOM_BOMDescription { get; set; }
        public bool ISMSBOM_ActiveFlag { get; set; }
        public long ISMSBOM_CreatedBy { get; set; }
        public long ISMSBOM_UpdatedBy { get; set; }
        public DateTime? ISMSBOM_CreatedDate { get; set; }
        public DateTime? ISMSBOM_UpdatedDate { get; set; }
    }
}
