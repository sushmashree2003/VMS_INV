using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FinancialYearWisePFVPFInterestFacadeController : Controller
    {
        public FinancialYearWisePFVPFInterestInterface _Inv;
        public FinancialYearWisePFVPFInterestFacadeController(FinancialYearWisePFVPFInterestInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getdetails")]
        public FinancialYearWisePFVPFInterestDTO getalldetails([FromBody] FinancialYearWisePFVPFInterestDTO data)
        {
            return _Inv.getalldetails(data);
        }
        [HttpPost]
        [Route("Savedetails")]
        public FinancialYearWisePFVPFInterestDTO Savedetails([FromBody] FinancialYearWisePFVPFInterestDTO data)
        {
            return _Inv.Savedetails(data);
        }
        [Route("edit")]
        public FinancialYearWisePFVPFInterestDTO edit([FromBody] FinancialYearWisePFVPFInterestDTO data)
        {
            return _Inv.edit(data);
        }
        [Route("deactive")]
        public FinancialYearWisePFVPFInterestDTO deactive([FromBody] FinancialYearWisePFVPFInterestDTO data)
        {
            return _Inv.deactive(data);
        }
    }
}