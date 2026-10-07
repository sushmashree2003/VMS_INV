using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Mastert_Consultancy")]
    public class Mastert_ConsultancyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMCON_Id { get; set; }
        public string HRMCON_ConsultancyName { get; set; }
        public string HRMCON_Address { get; set; }
        public string HRMCON_City { get; set; }
        public long HRMCON_State { get; set; }
        public string HRMCON_ContactNo { get; set; }
        public string HRMCON_EmailId { get; set; }
        public long HRMCON_PINCode { get; set; }
        public string HRMCON_ContactPersonName { get; set; }
        public bool HRMCON_ActiveFlg { get; set; }

        public DateTime HRMCON_CreatedDate { get; set; }
        public long HRMCON_CreatedBy { get; set; }
        public long HRMCON_UpdatedBy { get; set; }
        public DateTime HRMCON_UpdatedDate { get; set; }
    }
}
