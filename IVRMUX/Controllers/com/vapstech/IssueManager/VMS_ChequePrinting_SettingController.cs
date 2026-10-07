using System;
using DataAccessMsSqlServerProvider;
using IVRMUX.Delegates.com.vapstech.Inventory;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class VMS_ChequePrinting_SettingController : Controller
    {
        VMS_ChequePrinting_SettingDelegate del = new VMS_ChequePrinting_SettingDelegate();
        public DomainModelMsSqlServerContext _context;

        public VMS_ChequePrinting_SettingController(DomainModelMsSqlServerContext _cont)
        {
            _context = _cont;
        }

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public VMS_ChequePrinting_SettingDTO getloaddata(int id)
        {
            VMS_ChequePrinting_SettingDTO data = new VMS_ChequePrinting_SettingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getloaddata(data);
        }

        [Route("savedetails")]
        public VMS_ChequePrinting_SettingDTO savedetails([FromBody] VMS_ChequePrinting_SettingDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savedetails(data);
        }

        [Route("deactive")]
        public VMS_ChequePrinting_SettingDTO deactive([FromBody] VMS_ChequePrinting_SettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }

        [Route("companychange")]
        public VMS_ChequePrinting_SettingDTO companychange([FromBody] VMS_ChequePrinting_SettingDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.companychange(data);
        }

    }
}
