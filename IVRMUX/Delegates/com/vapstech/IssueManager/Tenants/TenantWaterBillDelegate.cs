using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantWaterBillDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TenantWaterBillDTO, TenantWaterBillDTO> COMMB = new CommonDelegate<TenantWaterBillDTO, TenantWaterBillDTO>();

        public TenantWaterBillDTO loaddata(TenantWaterBillDTO id)
        {
            return COMMB.POSTIssueManager(id, "TenantWaterBillFacade/loaddata/");
        }

       
        public TenantWaterBillDTO savetrainerfeedback(TenantWaterBillDTO dto)
        {
            return COMMB.POSTIssueManager(dto, "TenantWaterBillFacade/savetrainerfeedback/");
        }
        public TenantWaterBillDTO deactive(TenantWaterBillDTO data)
        {
            return COMMB.POSTIssueManager(data, "TenantWaterBillFacade/deactive");
        }

        public TenantWaterBillDTO viewroomwisewaterbill(TenantWaterBillDTO data)
        {
            return COMMB.POSTIssueManager(data, "TenantWaterBillFacade/viewroomwisewaterbill");
        }

    }
}
