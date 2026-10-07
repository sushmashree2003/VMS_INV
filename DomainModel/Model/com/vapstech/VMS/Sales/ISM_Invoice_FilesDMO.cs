using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Invoice_Files")]
  public  class ISM_Invoice_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IIF_ID { get; set; }
        public long ISMINC_ID { get; set; }
        public string IIF_FileName { get; set; }
        public string IIF_FileDesc { get; set; }
        public string IIF_FilePath { get; set; }
        public bool IIF_ActiveFlag { get; set; }
        public string IIF_CreatedBy { get; set; }
        public string IIF_UpdatedBy { get; set; }
        public DateTime IIF_CreatedDate { get; set; }
        public DateTime IIF_UpdatedDate { get; set; }
    }
}
