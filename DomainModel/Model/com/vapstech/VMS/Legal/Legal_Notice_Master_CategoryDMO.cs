using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_Notice_Master_Category")]
    public class Legal_Notice_Master_CategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LNC_Id { get; set; }
        public string LNC_Category_Name { get; set; }
        public bool LNC_ActiveFlag { get; set; }
        public DateTime? LNC_CreatedDate { get; set; }
        public DateTime? LNC_UpdatedDate { get; set; }
        public long? LNC_CreatedBy { get; set; }
        public long? LNC_UpdatedBy { get; set; }
    }
}
