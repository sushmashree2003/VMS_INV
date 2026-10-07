using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Emp_Loan_Documents")]
    public class HR_Emp_Loan_Documents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRELD_Id { get; set; }
      public long HREL_ID { get; set; }
      public long HRMDOC_Id { get; set; }
      public string HRELD_Filepath { get; set; }
      public string HRELD_Filetype { get; set; }
      public bool HRELD_ActiveFlag { get; set; }
      public DateTime? Createddate { get; set; }
      public DateTime? Updateddate { get; set; }
      public long? Createdby { get; set; }
      public long? Updatedby { get; set; }









    }
}
