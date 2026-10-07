using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("LNC_Master_Case_Type")]
    public class LNC_Master_Case_TypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MCST_Id { get; set; }
        public string MCST_Name { get; set; }
        public bool MCST_ActiveFlag { get; set; }
        public DateTime MCST_CreatedDate { get; set; }
        public DateTime MCST_UpdatedDate { get; set; }
        public long? MCST_CreatedBy { get; set; }
        public long? MCST_UpdatedBy { get; set; }
    }
}
