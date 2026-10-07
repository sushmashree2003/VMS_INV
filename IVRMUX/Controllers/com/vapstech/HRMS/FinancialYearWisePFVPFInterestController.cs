using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class FinancialYearWisePFVPFInterestController : Controller
    {
        FinancialYearWisePFVPFInterestDelegate _delegate = new FinancialYearWisePFVPFInterestDelegate();

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public FinancialYearWisePFVPFInterestDTO getalldetails(int id)
        {
            FinancialYearWisePFVPFInterestDTO dv = new FinancialYearWisePFVPFInterestDTO();
            dv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getalldetails(dv);
        }
        [HttpPost]
        [Route("Savedetails")]
        public FinancialYearWisePFVPFInterestDTO Savedetails([FromBody] FinancialYearWisePFVPFInterestDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delegate.Savedetails(dto);                 
        }
        [Route("edit")]
        public FinancialYearWisePFVPFInterestDTO edit([FromBody] FinancialYearWisePFVPFInterestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("MI_Id"));
            return _delegate.edit(data);
        }
        [Route("deactive")]
        public FinancialYearWisePFVPFInterestDTO deactive([FromBody] FinancialYearWisePFVPFInterestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("MI_Id"));
            return _delegate.deactive(data);
        }

    }
}