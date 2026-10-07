using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.SpaceBooking
{
    [Table("HR_Master_Account_Details")]
    public class HR_Master_Account_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMA_Id { get; set; }
        public long HRMF_Id  { get; set; }
        public string HRMA_Merchant_Id { get; set; }
        public string HRMA_Salt_Id { get; set; }
        public string HRMA_Authorisation_Id { get; set; }
        public string HRMF_URL { get; set; }
        public string HRMF_Submerchant_Id { get; set; }
        public long IMPG_Id { get; set; }
        public long User_Id { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
