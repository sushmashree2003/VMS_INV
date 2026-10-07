using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_ACKN_Status")]
    public class ISM_ACKN_Status_DMO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ISM_AS_Id { get; set; }
        public long User_Id { get; set; }
        public string ISM_AS_Remarks { get; set; }
        public DateTime? ISM_Date { get; set; }
        public DateTime? ISM_Created_Date { get; set; }
        public DateTime? ISM_Updated_Date { get; set; }
        public long? MI_Id { get; set; }
    }
}
