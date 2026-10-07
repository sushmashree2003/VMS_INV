using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.Tenants.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Tenants.Facade
{
    [Route("api/[controller]")]
    public class NocCreationFacade : ControllerBase
    {
        // GET: api/values
        NocCreationInterface _Inv;

        public NocCreationFacade(NocCreationInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public NocCreationDTO getloaddata([FromBody] NocCreationDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getfloors")]
        public NocCreationDTO getfloors([FromBody] NocCreationDTO data)
        {
            return _Inv.getfloors(data);
        }
        [Route("gettenantname")]
        public NocCreationDTO gettenantname([FromBody] NocCreationDTO data)
        {
            return _Inv.gettenantname(data);
        }
        

       [Route("getAreaName")]
        public NocCreationDTO getAreaName([FromBody] NocCreationDTO data)
        {
            return _Inv.getAreaName(data);
        }

        [Route("savedetailsStaffMapping")]
        public NocCreationDTO savedetailsStaffMapping([FromBody] NocCreationDTO data)
        {
            return _Inv.savedetailsStaffMapping(data);
        }

        [Route("deactive")]
        public NocCreationDTO deactive([FromBody] NocCreationDTO data)
        {
            return _Inv.deactive(data);
        }

        //[Route("edit")]
        //public NocCreationDTO edit([FromBody] NocCreationDTO data)
        //{
        //    return _Inv.edit(data);
        //}


    }
}
