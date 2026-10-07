using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class TrackerReportDTO
    {
        public long institution { get; set; }
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public string ModuleName { get; set; }
        public string companyname { get; set; }

        //HRMS
        public long totalactiveemployee { get; set; }
        public long totalleftemployee { get; set; }
        public Array ActEmpsYearly { get; set; }
        public Array ActEmpsMonthly { get; set; }
        public Array LeftEmpsYearly { get; set; }
        public Array LeftEmpsMonthly { get; set; }
        public Array LoanAdvEmpsYearly { get; set; }
        public Array LoanAdvEmpsMonthly { get; set; }
        public Array SalEmpsyearly { get; set; }
        public Array SalEmpsMonthly { get; set; }
        public Array ModeofPaymentyearly { get; set; }
        public Array ModeofPaymentMonthly { get; set; }
        public Array PFESIEmpsYearly { get; set; }
        public Array PFESIEmpsMonthly { get; set; }
        public Array CumulativeSalaryYearly { get; set; }
        public Array CumulativeSalaryMonthly { get; set; }
        public Array EmpsMobEmailYearly { get; set; }
        public Array EmpsMobEmailMonthly { get; set; }
        //HRMS
    }
}
