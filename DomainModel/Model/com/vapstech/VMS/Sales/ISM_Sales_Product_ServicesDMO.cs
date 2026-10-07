using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("Ism_Sales_Product_Services")]
    public class ISM_Sales_Product_ServicesDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSPS_ID { get; set; }
        public long MI_ID { get; set; }
        public string ISMSPS_Service_Name { get; set; }
        public string ISMSPS_Service_Description { get; set; }
        public bool ISMSPS_ActiveFlag { get; set; }
        public long ISMSPS_CreatedBy { get; set; }
        public long ISMSPS_UpdatedBy { get; set; }
        public DateTime? ISMSPS_CreatedDate { get; set; }
        public DateTime? ISMSPS_UpdatedDate { get; set; }
        public long AbvStrength { get; set; }
    }
}
