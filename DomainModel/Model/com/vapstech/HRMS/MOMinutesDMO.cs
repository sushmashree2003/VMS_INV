using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("Minutes_Of_Meeting")]
    public class MOMinutesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MOM_Id { get; set; }
        public long MOM_ParentId { get; set; }
        public long MI_Id { get; set; }
        public string MOM_Agenda { get; set; }
        public string MOM_Name { get; set; }
        public DateTime MOM_Date { get; set; }
        public string MOM_StartTime { get; set; }
        public string MOM_EndTime { get; set; }
        public string MOM_TentativeStartTime { get; set; }
        public string MOM_TentativeEndTime { get; set; }
        public string MOM_Venue { get; set; }
        public string MOM_Minutes { get; set; }
        public bool MOM_ActiveFlag { get; set; }
        public string MOM_EXTINTFlag { get; set; }
        public string MOM_Flag { get; set; }
        public DateTime MOM_CreatedDate { get; set; }
        public DateTime MOM_UpdatedDate { get; set; }
        public long MOM_CreatedBy { get; set; }
        public long MOM_UpdatedBy { get; set; }
        public string MOM_Status { get; set; }
    }
}
