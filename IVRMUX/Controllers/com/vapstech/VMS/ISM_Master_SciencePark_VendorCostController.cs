using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class ISM_Master_SciencePark_VendorCostController : Controller
    {
        ISM_Master_SciencePark_VendorCostDelegate vendorcost = new ISM_Master_SciencePark_VendorCostDelegate();


        [Route("OnLoadData/{id:int}")]
        public ISM_Master_SciencePark_VendorCostDto OnLoadData(int id)
        {
            ISM_Master_SciencePark_VendorCostDto dto = new ISM_Master_SciencePark_VendorCostDto();
            dto.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return vendorcost.OnLoadData(dto);

        }


        [Route("Save")]
        public ISM_Master_SciencePark_VendorCostDto Save([FromBody] ISM_Master_SciencePark_VendorCostDto dto)
        {
            dto.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return vendorcost.Save(dto);
        }

        [Route("Deactivate")]
        public ISM_Master_SciencePark_VendorCostDto Deactivate([FromBody] ISM_Master_SciencePark_VendorCostDto dto)
        {

            dto.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return vendorcost.Deactivate(dto);
        }

        //[Route("Edit/{id:int}")]
        //public EmployeeDataDto Edit(int id)
        //{
        //    EmployeeDataDto dto = new EmployeeDataDto();
        //    // dto.ISMCLTPRBOM_Id = id;
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return sales.Edit(dto);
        //}
    }
}
