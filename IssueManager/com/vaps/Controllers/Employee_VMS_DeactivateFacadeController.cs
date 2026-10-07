using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Employee_VMS_DeactivateFacadeController : Controller
    {
        public Employee_VMS_DeactivateInterface inter;
        public Employee_VMS_DeactivateFacadeController(Employee_VMS_DeactivateInterface s)
        {
            inter = s;
        }
        
        [Route("Onloaddata")]
        public Employee_VMS_DeactivateDTO Onloaddata([FromBody] Employee_VMS_DeactivateDTO data)
        {
            return inter.Onloaddata(data);
        }
        [Route("SaveData")]
        public Employee_VMS_DeactivateDTO SaveData([FromBody] Employee_VMS_DeactivateDTO data)
        {
            return inter.SaveData(data);
        } 
        //scedular api
        [Route("DeavtivEmployee")]
        public Employee_VMS_DeactivateDTO DeavtivEmployee([FromBody] Employee_VMS_DeactivateDTO data)
        {
            return inter.DeavtivEmployee(data);
        }
    }
}