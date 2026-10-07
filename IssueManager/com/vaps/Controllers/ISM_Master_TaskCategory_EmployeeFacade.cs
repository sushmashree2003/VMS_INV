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
    public class ISM_Master_TaskCategory_EmployeeFacade : Controller
    {
        public ISM_Master_TaskCategory_EmployeeInterface inter;
        public ISM_Master_TaskCategory_EmployeeFacade(ISM_Master_TaskCategory_EmployeeInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public ISM_Master_TaskCategory_Employee_DTO loaddata([FromBody] ISM_Master_TaskCategory_Employee_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public ISM_Master_TaskCategory_Employee_DTO save([FromBody] ISM_Master_TaskCategory_Employee_DTO data)
        {
            return inter.save(data);
        }
        [Route("deactive")]
        public ISM_Master_TaskCategory_Employee_DTO deactive([FromBody] ISM_Master_TaskCategory_Employee_DTO data)
        {
            return inter.deactive(data);
        }
        [Route("EditData")]
        public ISM_Master_TaskCategory_Employee_DTO EditData([FromBody] ISM_Master_TaskCategory_Employee_DTO data)
        {
            return inter.EditData(data);
        }
        [Route("getcategory")]
        public ISM_Master_TaskCategory_Employee_DTO getcategory([FromBody] ISM_Master_TaskCategory_Employee_DTO data)
        {
            return inter.getcategory(data);
        }
    }
}
