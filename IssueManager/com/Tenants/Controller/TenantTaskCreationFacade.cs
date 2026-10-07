using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.Tenants.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Tenants.Controller
{
    [Route("api/[controller]")]
    public class TenantTaskCreationFacade : ControllerBase
    {

        public TenantTaskCreationInterface _objinter;

        public TenantTaskCreationFacade(TenantTaskCreationInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<TenantTaskCreationDTO> getdetails([FromBody] TenantTaskCreationDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getIEuser")]
        public Task<TenantTaskCreationDTO> getIEuser([FromBody]TenantTaskCreationDTO data)
        {
            return _objinter.getIEuser(data);
        }
        [Route("getmodule")]
        public TenantTaskCreationDTO getmodule([FromBody]TenantTaskCreationDTO data)
        {
            return _objinter.getmodule(data);
        }
        [Route("savedata")]
        public TenantTaskCreationDTO savedata([FromBody]TenantTaskCreationDTO data)
        {
            return _objinter.savedata(data);
        }

        [Route("deactive")]
        public TenantTaskCreationDTO deactive([FromBody]TenantTaskCreationDTO data)
        {
            return _objinter.deactive(data);
        }
        [Route("gettaskAttachment")]
        public TenantTaskCreationDTO gettaskAttachment([FromBody]TenantTaskCreationDTO data)
        {
            return _objinter.gettaskAttachment(data);
        }
         [Route("get_category")]
        public TenantTaskCreationDTO get_category([FromBody]TenantTaskCreationDTO data)
        {
            return _objinter.get_category(data);
        }


        //Task Creation For Client IN IVRM
        [Route("getdetailsClient")]
        public Task<TenantTaskCreationDTO> getdetailsClient([FromBody] TenantTaskCreationDTO data)
        {
            return _objinter.getdetailsClient(data);
        }
        [Route("savedataClient")]
        public TenantTaskCreationDTO savedataClient([FromBody]TenantTaskCreationDTO data)
        {
            return _objinter.savedataClient(data);
        }

        [Route("saveResponse")]
        public TenantTaskCreationDTO saveResponse([FromBody]TenantTaskCreationDTO data)
        {
            return _objinter.saveResponse(data);
        }

    }
}
