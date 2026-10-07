using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.TADA.Controllers
{
    [Route("api/[controller]")]
    public class TADARoot_CategoryFacade : Controller
    {
        public TADARoot_CategoryInterface _objinter;
        public TADARoot_CategoryFacade(TADARoot_CategoryInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADARoot_CategoryDTO getalldata([FromBody]TADARoot_CategoryDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("SaveCategory")]
        public TADARoot_CategoryDTO SaveCategory([FromBody]TADARoot_CategoryDTO data)
        {
            return _objinter.SaveCategory(data);
        }
        [Route("SaveRouteEmpCategory")]
        public TADARoot_CategoryDTO SaveRouteEmpCategory([FromBody]TADARoot_CategoryDTO data)
        {
            return _objinter.SaveRouteEmpCategory(data);
        }
        [Route("saveRegionMapping")]
        public TADARoot_CategoryDTO saveRegionMapping([FromBody]TADARoot_CategoryDTO data)
        {
            return _objinter.saveRegionMapping(data);
        }
        [Route("deactiveY")]
        public TADARoot_CategoryDTO deactiveY([FromBody]TADARoot_CategoryDTO user)
        {
            return _objinter.deactiveY(user);
        }

        [Route("viewrecordspopup")]
        public TADARoot_CategoryDTO viewrecordspopup([FromBody]TADARoot_CategoryDTO data)
        {
            return _objinter.viewrecordspopup(data);
        }


        [Route("deactiveCat")]
        public TADARoot_CategoryDTO deactiveCat([FromBody]TADARoot_CategoryDTO user)
        {
            return _objinter.deactiveCat(user);
        }
        [Route("deactiveRegion")]
        public TADARoot_CategoryDTO deactiveRegion([FromBody]TADARoot_CategoryDTO user)
        {
            return _objinter.deactiveRegion(user);
        }
        [Route("Saveprimary")]
        public TADARoot_CategoryDTO Saveprimary([FromBody]TADARoot_CategoryDTO user)
        {
            return _objinter.Saveprimary(user);
        }
        [Route("ActivatePrimary")]
        public TADARoot_CategoryDTO ActivatePrimary([FromBody]TADARoot_CategoryDTO primary)
        {
            return _objinter.ActivatePrimary(primary);
        }
        [Route("validateordernumber")]
        public TADARoot_CategoryDTO validateordernumber([FromBody]TADARoot_CategoryDTO primary)
        {
            return _objinter.validateordernumber(primary);
        }
        [Route("SetOrder")]
        public TADARoot_CategoryDTO SetOrder([FromBody]TADARoot_CategoryDTO primary)
        {
            return _objinter.SetOrder(primary);
        }
    }
}
