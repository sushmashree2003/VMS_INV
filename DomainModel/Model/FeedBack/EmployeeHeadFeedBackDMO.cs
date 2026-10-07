using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("HR_Employee_GFeedback")]
    public class EmployeeHeadFeedBackDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRGFE_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
    
        public string HRGFE_FeedBack { get; set; }    
        public DateTime HRGFE_FeedbackDate { get; set; }      
        public bool HRGFE_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long HRGFE_CreatedBy { get; set; }
        public long HRGFE_UpdatedBy { get; set; }


    }
}

