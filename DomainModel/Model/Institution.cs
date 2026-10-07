using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Master_Institution")]
    public class Institution : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MI_Id { get; set; }
        public long MO_Id { get; set; }
        public string MI_Name { get; set; }
        public string MI_Code { get; set; }
        public string MI_Type { get; set; }
        public string MI_Address1 { get; set; }
        public string MI_Address2 { get; set; }
        public string MI_Address3 { get; set; }
        public string MI_AddressArea { get; set; }
        public string IVRMMCT_Name { get; set; }
        public long IVRMMS_Id { get; set; }
        public long IVRMMC_Id { get; set; }
        public int MI_Pincode { get; set; }
        public int? MI_FaxNo { get; set; }
        
        public string MI_BackgroundImage { get; set; }
        public string MI_FormColor { get; set; }
        public string MI_FontColor { get; set; }
        public int MI_FontSize { get; set; }
        public string MI_WeekStartDay { get; set; }
        public string MI_DateFormat { get; set; }
        public string MI_DateSeparator { get; set; }
        public string MI_Logo { get; set; }
        public string MI_Precision { get; set; }
        public string MI_GradingSystem { get; set; }
        public string MI_PasswordFlag { get; set; }
        public int MI_ActiveFlag { get; set; }
        public int? MI_FranchiseFlag { get; set; }
        public string MI_Affiliation { get; set; }
        //  public virtual ICollection<Preadmission_SeatBlocked_Student> seatblocked { get; set; }

        [ForeignKey("MO_Id")]
        public virtual Organisation Organisation { get; set; }

        public string MI_Subdomain { get; set; }

        public string MI_AboutInstitute { get; set; }

        public string MI_ContactDetails  { get; set; }

        public string MI_HelpFile  { get; set; }

        public string  MI_SchoolCollegeFlag { get; set; }

        public string MI_PAN { get; set; }
        public string MI_TAN { get; set; }

        public string MI_GPSUserName { get; set; }

        public string MI_IVRSVirtualNo  { get; set; }

        public string MI_IVRSOutboundNo  { get; set; }
        public string MI_GSTNO { get; set; }
        public string MI_CINNo { get; set; }
        public string MI_WhatsAppto { get; set; }
        public string MI_WhatsAppName { get; set; }
        public string MI_WhatsApptype { get; set; }
        public string MI_WhatsAppToken { get; set; }
    }
}
