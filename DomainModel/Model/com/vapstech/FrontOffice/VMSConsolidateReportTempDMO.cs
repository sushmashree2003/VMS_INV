using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("VMS_Consolidate_Report_Temp")]
    public class VMSConsolidateReportTempDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public bool VMSCRT_Approval { get; set; }
        public string VMSCRT_MDComment { get; set; }
        public double VMSCRT_HDailyReport { get; set; }
        public string VMSCRT_HDRDetails { get; set; }
        public double VMSCRT_HRHDR { get; set; }
        public double VMSCRT_BDailyReport { get; set; }
        public string VMSCRT_BDRDetails { get; set; }
        public double VMSCRT_HRBDR { get; set; }
        public decimal? VMSCRT_CompoffAdjustCount { get; set; }
    }
}
