using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeInstallmentFacade : Controller
    {
        public FeeInstallmentInterface _feegrouppage;

        public FeeInstallmentFacade(FeeInstallmentInterface maspag)
        {
            _feegrouppage = maspag;
        }

        [Route("getdetails")]
        public FeeInstallmentDTO getorgdet([FromBody] FeeInstallmentDTO mas)
        {
            return _feegrouppage.getdetails(mas);
        }

        [Route("SaveGroupData")]
        public FeeInstallmentDTO SaveGroupData([FromBody] FeeInstallmentDTO org)
        {
            return _feegrouppage.SaveGroupData(org);
        }

        [Route("getpagedetails/{id:int}")]        
        public FeeInstallmentDTO getpagedetails(int id)
        {           
            return _feegrouppage.getpageedit(id);
        }

        [Route("deactivate")]
        public FeeInstallmentDTO deactivateAcdmYear([FromBody] FeeInstallmentDTO id)
        {            
            return _feegrouppage.deactivate(id);
        }
    }
}