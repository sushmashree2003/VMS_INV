using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_CaseDetails")]
    public class Legal_CaseDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LCD_Id { get; set; }
        public string LCD_CaseType { get; set; }
        public string LCD_FillingNumber { get; set; }
        public DateTime? LCD_FillingDate { get; set; }
        public string LCD_RegistrationNumber { get; set; }
        public DateTime? LCD_RegistrationDate { get; set; }
        public string LCD_CNRNumber { get; set; }
        public bool LCD_ActiveFlag { get; set; }
        public DateTime? LCD_CreatedDate { get; set; }
        public DateTime? LCD_UpdatedDate { get; set; }
        public long LCD_CreatedBy { get; set; }
        public long LCD_UpdatedBy { get; set; }
        public string LCD_PartyName { get; set; }
        public DateTime? LCD_NextHearingDate { get; set; }
    }
}
