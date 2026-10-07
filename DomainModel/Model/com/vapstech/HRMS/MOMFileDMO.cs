using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("Minutes_Of_Meeting_Files")]
   public class MOMFileDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long  MOMFL_Id { get; set; }
        public long  MOM_Id { get; set; }
      public string  MOMFL_FileName { get; set; }
       public string MOMFL_FilePath { get; set; }
      public bool  MOMFL_ActiveFlag { get; set; }
     public DateTime   MOMFL_CreatedDate { get; set; }
     public DateTime   MOMFL_UpdatedDate { get; set; }
      public long?  MOMFL_CreatedBy { get; set; }
      public long? MOMFL_UpdatedBy { get; set; }
    }
}
