using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("VMS_DriverOt_Details")]
    public class DriverOtDetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSDT_ID { get; set; }
        public long HRME_Id { get; set; }
        public DateTime VMSDT_Punchdate { get; set; }
        public string VMSDT_PunchIn { get; set; }
        public string VMSDT_PunchOut { get; set; }
        public string VMSDT_LateIn { get; set; }
        public string VMSDT_EarlyOut { get; set; }
        public string VMSDT_EarlyComingOt { get; set; }
        public string VMSDT_LategoingOt { get; set; }
        public string VMSDT_TotalOt { get; set; }
        public string VMSDT_FinalOt { get; set; }
        public string VMSDT_Remarks { get; set; }
        public long VMSDT_MonthId { get; set; }
        public string VMSDT_FinalLatein { get; set; }
        public long VMSDT_CreatedBy { get; set; }
        public long VMSDT_UpdatedBy { get; set; }
        public long VMSDT_ShiftId { get; set; }
        public bool withpreviousdayflag { get; set; }
    }
}     
     
















       
