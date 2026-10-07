using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class VMSConsolidateReportTempDTO
    {
        public long VMSCRT_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VMSCRT_Year { get; set; }
        public string VMSCRT_Month { get; set; }
        public string VMSCRT_EMPCode { get; set; }
        public string VMSCRT_EName { get; set; }
        public string VMSCRT_DesigCode { get; set; }
        public double VMSCRT_Earlyout { get; set; }
        public double VMSCRT_DailyReportnotsend { get; set; }
        public double VMSCRT_Latecoming { get; set; }
        public double VMSCRT_unauthorizedleave { get; set; }
        public double VMSCRT_TLOP { get; set; }
        public string VMSCRT_DRDetails { get; set; }
        public string VMSCRT_ULDetails { get; set; }
        public string VMSCRT_EODetails { get; set; }
        public string VMSCRT_LCDetails { get; set; }
        public string VMSCRT_EOU180 { get; set; }
        public string VMSCRT_LCU180 { get; set; }
        public double VMSCRT_HREO { get; set; }
        public double VMSCRT_HRLC { get; set; }
        public double VMSCRT_HRUL { get; set; }
        public double VMSCRT_HRDR { get; set; }
        public double VMSCRT_HRTLOP { get; set; }
        public string VMSCRT_Remarks { get; set; }
        public string VMSCRT_DOJ { get; set; }
        public string ecode { get; set; }
        public string ename { get; set; }
        public string desgname { get; set; }
        public float drcount { get; set; }
        public float leavecount { get; set; }
        public float halfdrcount { get; set; }
        public float totlop { get; set; }
        public string drdates { get; set; }
        public string leavedates { get; set; }
        public string halfdrdates { get; set; }
        public string emailid { get; set; }
        public empdate[] empdate { get; set; }
        public float HR_EO { get; set; }
        public float HR_LC { get; set; }
        public float HR_UL { get; set; }
        public float HR_DR { get; set; }
        public float HR_HDR { get; set; }
        public float HR_LOP { get; set; }
        public string HR_REM { get; set; }
        public long HRMED_Id { get; set; }
        public string HRMED_Name { get; set; }
        public string HRMED_EarnDedFlag { get; set; }
        public decimal? HRESD_Amount { get; set; }
        public long HRELT_Id { get; set; }
        public decimal HRELT_TotDays { get; set; }
        public bool VMSCRT_Approval { get; set; }
        public string VMSCRT_MDComment { get; set; }
        public double VMSCRT_HDailyReport { get; set; }
        public string VMSCRT_HDRDetails { get; set; }
        public double VMSCRT_HRHDR { get; set; }
        public float blkdrcount { get; set; }
        public float HR_BDR { get; set; }
        public string blkdrdates { get; set; }
        public double VMSCRT_BDailyReport { get; set; }
        public string VMSCRT_BDRDetails { get; set; }
        public string html { get; set; }
        public double VMSCRT_HRBDR { get; set; }
        public decimal? nooffcompoffadjested { get; set; }
        public Lopadjestdto[] compoffadjestdate { get; set; }
    }

    public class Lopadjestdto
    {
        public long HRME_Id { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public decimal? LOP { get; set; }

    }

    public class empdate
    {
        public double earlyout { get; set; }
        public double latecoming { get; set; }
        public string latedates { get; set; }
        public string latedateafter { get; set; }
        public string earlydates { get; set; }
        public string earlydateafter { get; set; }
        public double totlop { get; set; }
    }
}
