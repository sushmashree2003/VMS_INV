using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{

    [Table("VMS_TADA_CityMapping")]
    public class VMS_TADA_CityMappingBDM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADACM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long IVRMMCT_Id { get; set; }
        public decimal VTADACM_FoodAmt { get; set; }
        public decimal VTADACM_AccommodationAmt { get; set; }
        public decimal VTADACM_TransportAmt { get; set; }
        public bool VTADACM_ActiveFlag { get; set; }
        public DateTime? VTADACM_CreatedDate { get; set; }
        public DateTime? VTADACM_UpdatedDate { get; set; }
        public long VTADACM_CreatedBy { get; set; }
        public long VTADACM_UpdatedBy { get; set; }
        public decimal? VTADACM_TravelHour { get; set; }

    }
}
