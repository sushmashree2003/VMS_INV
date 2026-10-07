using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
 
    [Table("HR_Training_Assignment_Qns")]
    public class HR_Training_Assignment_Questions_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTAQNS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMTT_Id { get; set; }
        public string HRTAQNS_QuestionName { get; set; }
        public string HRTAQNS_QuestionTypeFlg { get; set; }
        public long HRTAQNS_QuestionOrder { get; set; }

     
        public bool HRTAQNS_ActiveFlg { get; set; }
        public long? HRTAQNS_CreatedBy { get; set; }
        public long? HRTAQNS_UpdatedBy { get; set; }
        public DateTime? HRTAQNS_CreatedDate { get; set; }
        public DateTime? HRTAQNS_UpdatedDate { get; set; }
    }


}

