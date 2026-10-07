using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Assignment_Options")]
    public class HR_Training_Assignment_Option_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTAOPT_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRTAOPT_OptionName { get; set; }
        public long HRTAOPT_OptionOrder { get; set; }
        public bool HRTAOPT_ActiveFlg { get; set; }
        public long? HRTAOPT_CreatedBy { get; set; }
        public long? HRTAOPT_UpdatedBy { get; set; }
        public DateTime? HRTAOPT_CreatedDate { get; set; }
        public DateTime? HRTAOPT_UpdatedDate { get; set; }
        
       
    }

}

