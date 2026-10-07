using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_Master_SciencePark_VendorCostFacadeController : Controller
    {
        public ISM_Master_SciencePark_VendorCostInterface _intface;
        public ISM_Master_SciencePark_VendorCostFacadeController(ISM_Master_SciencePark_VendorCostInterface intface)
        {
            _intface = intface;
        }

        [Route("OnLoadData/")]
        public ISM_Master_SciencePark_VendorCostDto OnLoadData([FromBody] ISM_Master_SciencePark_VendorCostDto dto)
        {
            return _intface.OnLoadData(dto);
        }

        [Route("Save/")]
        public ISM_Master_SciencePark_VendorCostDto Save([FromBody] ISM_Master_SciencePark_VendorCostDto dto)
        {
            return _intface.Save(dto);
        }
        [Route("Deactivate/")]
        public ISM_Master_SciencePark_VendorCostDto Deactivate([FromBody] ISM_Master_SciencePark_VendorCostDto dto)
        {
            return _intface.Deactivate(dto);
        }
    
    }
}
