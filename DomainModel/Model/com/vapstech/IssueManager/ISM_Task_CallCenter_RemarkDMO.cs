using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_CallCenter_Remark")]
    public class ISM_Task_CallCenter_RemarkDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCCR_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCCR_Remarks { get; set; }
        public DateTime? ISMTCCR_CreatedDate { get; set; }
        public DateTime? ISMTCCR_UpdatedDate { get; set; }
        public long ISMTCCR_CreatedBy { get; set; }
        public long ISMTCCR_UpdateBy { get; set; }
    }
}
