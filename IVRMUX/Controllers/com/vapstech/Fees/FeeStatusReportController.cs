using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeStatusReportController
    {
        FeeStatusReportDelegate FSRD = new FeeStatusReportDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTransactionPaymentDTO Get(int id)
        {
            return FSRD.getdetails(id);
        }


        [HttpPost]
        [Route("radiobtndata")]
        public FeeTransactionPaymentDTO radiobtndata([FromBody]FeeTransactionPaymentDTO value)
        {
            return FSRD.radiobtndata(value);
        }
    }
}
