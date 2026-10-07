using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Sales_Lead_Negaotion_DesignationDto
    {
        public long MI_Id { get; set; }

        public long User_Id { get; set; }
        public long ISLIDN_ID { get; set; }
        public long HRME_Id { get; set; }
        public long ISLIDN_Value { get; set; }
        public string ISLIDN_Remark { get; set; }
        public bool ISLIDN_ActiveFlag { get; set; }
        public DateTime? ISLIDN_CreatedDate { get; set; }
        public DateTime? ISLIDN_UpdatedDate { get; set; }
        public long ISLIDN_CreatedBy { get; set; }
        public long ISLIDN_UpdatedBy { get; set; }
        public string returndata { get; set; }
        public string flag { get; set; }
        public string Year { get; set; }

        public string fromdate { get; set; }
        public string todate { get; set; }
        public Array SalesQuatationReport { get; set; }

        public Array EmployeeData { get; set; }
        public Array Employee_Ind_Details { get; set; }
        public Array Sales_Negotation_Detail_Report { get; set; }
        public Array Sales_Lead_Demo_Swapping_Report { get; set; }
        public Array Sales_Negotation_Payment_Report { get; set; }

    }
}
