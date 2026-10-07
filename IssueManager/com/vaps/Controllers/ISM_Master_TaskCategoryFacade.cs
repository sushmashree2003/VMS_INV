using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_Master_TaskCategoryFacade : Controller
    {
        public ISM_Master_TaskCategoryInterface inter;
        public ISM_Master_TaskCategoryFacade(ISM_Master_TaskCategoryInterface q)
        {
            inter = q;
        }
        [Route("loaddata")]
        public ISM_Master_TaskCategory_DTO loaddata([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public ISM_Master_TaskCategory_DTO save([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.save(data);
        }
        [Route("deactive")]
        public ISM_Master_TaskCategory_DTO deactive([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.deactive(data);
        }
        [Route("EditData")]
        public ISM_Master_TaskCategory_DTO EditData([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.EditData(data);
        }

        //============== Master Task Categary Check List===========
        [Route("loaddata_checklist")]
        public ISM_Master_TaskCategory_DTO loaddata_checklist([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.loaddata_checklist(data);
        }
        [Route("getcategory")]
        public ISM_Master_TaskCategory_DTO getcategory([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.getcategory(data);
        }
         [Route("savechecklist")]
        public ISM_Master_TaskCategory_DTO savechecklist([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.savechecklist(data);
        }
        [Route("EditData_checklist")]
        public ISM_Master_TaskCategory_DTO EditData_checklist([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.EditData_checklist(data);
        }
         [Route("deactive_checklist")]
        public ISM_Master_TaskCategory_DTO deactive_checklist([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            return inter.deactive_checklist(data);
        }


    }
}
