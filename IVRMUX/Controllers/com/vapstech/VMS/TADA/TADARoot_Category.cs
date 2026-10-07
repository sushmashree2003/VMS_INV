using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.TADA
{
    [Route("api/[controller]")]
    public class TADARoot_Category : Controller
    {
        TADARoot_CategoryDelgate _delegate = new TADARoot_CategoryDelgate();

        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADARoot_CategoryDTO getalldata(int id)
        {
            TADARoot_CategoryDTO dto = new TADARoot_CategoryDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }

        [Route("SaveCategory")]
        public TADARoot_CategoryDTO SaveCategory([FromBody]TADARoot_CategoryDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveCategory(data);
        }
        [Route("SaveEmprouteCategory")]
        public TADARoot_CategoryDTO SaveRouteEmpCategory([FromBody]TADARoot_CategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveRouteEmpCategory(data);
        }

        [Route("saveRegionMapping")]
        public TADARoot_CategoryDTO saveRegionMapping([FromBody]TADARoot_CategoryDTO data)
        {
             data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.saveRegionMapping(data);
        }


    
        [Route("deactiveY")]
        public TADARoot_CategoryDTO deactiveY([FromBody]TADARoot_CategoryDTO user)
        {
            user.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            user.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactiveY(user);

        }

        [Route("deactiveCat")]
        public TADARoot_CategoryDTO deactiveCat([FromBody]TADARoot_CategoryDTO user)
        {
            user.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            user.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactiveCat(user);

        }
        [Route("deactiveRegion")]
        public TADARoot_CategoryDTO deactiveRegion([FromBody]TADARoot_CategoryDTO user)
        {
            user.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            user.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactiveRegion(user);

        }

        [Route("viewrecordspopup")]
        public TADARoot_CategoryDTO viewrecordspopup([FromBody] TADARoot_CategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.viewrecordspopup(data);
        }
        [Route("Saveprimary")]
        public TADARoot_CategoryDTO Saveprimary([FromBody] TADARoot_CategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Saveprimary(data);
        }
        [Route("ActivatePrimary")]
        public TADARoot_CategoryDTO ActivatePrimary([FromBody] TADARoot_CategoryDTO primary)
        {
            primary.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            primary.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.ActivatePrimary(primary);
        }
        [Route("validateordernumber")]
        public TADARoot_CategoryDTO validateordernumber([FromBody] TADARoot_CategoryDTO primary)
        {
            primary.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            primary.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.validateordernumber(primary);
        }
        [Route("SetOrder")]
        public TADARoot_CategoryDTO SetOrder([FromBody] TADARoot_CategoryDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.SetOrder(data);
        }
    }
}
