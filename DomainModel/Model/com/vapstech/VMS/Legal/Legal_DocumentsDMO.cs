using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_Documents")]
    public class Legal_DocumentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LD_Id { get; set; }
        public long LCD_Id { get; set; }
   
        public string LD_DocumentName { get; set; }
        public string Ld_DocumentPath { get; set; }
        public string LD_Remarks { get; set; }
        public bool LD_ActiveFlag { get; set; }
        public DateTime LD_CreatedDate { get; set; }
        public DateTime LD_UpdatedDate { get; set; }
        public long LD_CreatedBy { get; set; }
        public long LD_UpdatedBy { get; set; }
    }
}
