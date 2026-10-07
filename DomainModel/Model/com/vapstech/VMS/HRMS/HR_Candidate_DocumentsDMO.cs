using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_Documents")] 
    public class HR_Candidate_DocumentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRCDD_Id { get; set; } 
        public long HRCD_Id { get; set; } 
        public string HRCDD_DocumentPath { get; set; } 
        public DateTime HRCDD_CreatedDate { get; set; } 
        public DateTime HRCDD_UpdatedDate { get; set; } 
        public long HRCDD_CreatedBy { get; set; } 
        public long HRCDD_UpdatedBy { get; set; } 
        public string HRCDD_DocumentName { get; set; }


    }
}
