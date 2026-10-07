using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Assignment_Questions_Options")]
    public class HR_Training_Assignment_Question_Option_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTAQOPT_Id { get; set; } 
        public long MI_Id { get; set; }
        public long HRTAQNS_Id { get; set; }
        public long HRTAOPT_Id { get; set; }
       
        public bool HRTAQOPT_ActiveFlg { get; set; }
        public long HRTAQOPT_CreatedBy { get; set; }
        public long HRTAQOPT_UpdatedBy { get; set; }
        public DateTime? HRTAQOPT_CreatedDate { get; set; }
        public DateTime? HRTAQOPT_UpdatedDate { get; set; }

    }
}
