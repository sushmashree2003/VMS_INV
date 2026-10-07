using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_Acts")]
    public class Legal_ActsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LACT_Id { get; set; }
        public long LCD_Id { get; set; }
        public string LACT_UnderActs { get; set; }
        public string LACT_UnderSection { get; set; }
        public bool LACT_ActiveFlag { get; set; }
        public DateTime? LACT_CreatedDate { get; set; }
        public DateTime? LACT_UpdatedDate { get; set; }
        public long LACT_CreatedBy { get; set; }
        public long LACT_UpdatedBy { get; set; }
    }
}
