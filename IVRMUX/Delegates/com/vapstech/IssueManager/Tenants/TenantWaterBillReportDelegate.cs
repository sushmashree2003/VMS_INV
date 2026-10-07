using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantWaterBillReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantWaterBillReportDTO, TenantWaterBillReportDTO> COMMM = new CommonDelegate<TenantWaterBillReportDTO, TenantWaterBillReportDTO>();

        public TenantWaterBillReportDTO getdetails(TenantWaterBillReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantWaterBillReportFacade/getdetails/");
        }
        public TenantWaterBillReportDTO get_report(TenantWaterBillReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantWaterBillReportFacade/get_report");
        }
        
        public TenantWaterBillReportDTO buildingchange(TenantWaterBillReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantWaterBillReportFacade/buildingchange");
        }
        public TenantWaterBillReportDTO Floorchange(TenantWaterBillReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantWaterBillReportFacade/Floorchange");
        }
        public TenantWaterBillReportDTO sendmail(TenantWaterBillReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantWaterBillReportFacade/sendmail/");
        }
    }
}
