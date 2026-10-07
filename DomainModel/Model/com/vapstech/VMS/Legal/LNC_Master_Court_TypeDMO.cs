using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("LNC_Master_Court_Type")]
    public class LNC_Master_Court_TypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MCT_Id { get; set; }
        public string MCT_Type { get; set; }
        public bool MCT_ActiveFlag { get; set; }
        public DateTime MCT_CreatedDate { get; set; }
        public DateTime MCT_UpdatedDate { get; set; }
        public long? MCT_CreatedBy { get; set; }
        public long? MCT_UpdatedBy { get; set; }
    }

}

