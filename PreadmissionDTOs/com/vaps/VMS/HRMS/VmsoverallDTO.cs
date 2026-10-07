using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
   public class VmsoverallDTO
    {
        public long MI_Id { get; set; }
        public string HRMJ_JobCode { get; set; }
        public long HRMPT_Id { get; set; }
        public long HRMJ_Id { get;set;}
        public decimal HRCD_ExpFrom { get; set; }
        public string HRMJ_JobTiTle { get; set; }
        public string HRMC_QulaificationName { get; set; }
        public string RDO_TyPETwo { get; set; }

        public Array MasterQualification { get; set; }
        public string Recruitmentstatus { get; set; }
        public string candiatename { get; set; }
        public string IVRMMR_Name { get; set; }
        public string Rdotype { get; set; }
        public long? HRMC_Id { get; set; }
        public DateTime HRCD_DOB { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime ? TO_DATE { get; set; }
        public long HRCD_MobileNo { get; set; }
        public string HRCD_EmailId { get; set; }
        public string HRCD_Resume { get; set; }
        public string HRCD_Photo { get; set; }
        public Array getReport { get; set; }
        public Array MasterPositionList { get; set; }
        public Array getjobTitle { get; set; }
        public Array getPosition { get; set; }
        //public Array Date_Wise { get; set; }
       public long IVRMMG_Id { get; set; }
        public long HRCD_Religion { get; set; }
        public Array masterjob { get; set; }
        public Array getreligion { get; set; }
       public Array masterReligion { get; set; }
        public Array getdatewise { get; set; }
        public string HRMPT_Name { get; set; }
        public Array getctc { get; set; }

        public Array Get_DbArray { get; set; }
        public Array getexperiance { get; set; }

        public decimal HRCD_ExpectedCTC { get; set; }

        public decimal BELOW_CTC { get; set; }


        public Array Gender { get; set; }
        public decimal HIGH_CTC { get; set; }
    }
}
