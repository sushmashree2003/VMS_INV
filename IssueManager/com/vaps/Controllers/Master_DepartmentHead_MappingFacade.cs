using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using IssueManager.com.vaps.Services;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Master_DepartmentHead_MappingFacade : Controller
    {
        public Master_DepartmentHead_MappingInterface inter;
        public Master_DepartmentHead_MappingFacade(Master_DepartmentHead_MappingInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public Master_DepartmentHead_MappingDTO loaddata([FromBody] Master_DepartmentHead_MappingDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public Master_DepartmentHead_MappingDTO savedata([FromBody] Master_DepartmentHead_MappingDTO data)
        {
            return inter.savedata(data);
        }
        [Route("depchange")]
        public Master_DepartmentHead_MappingDTO depchange([FromBody] Master_DepartmentHead_MappingDTO data)
        {
            return inter.depchange(data);
        }
        [Route("Decative")]
        public Master_DepartmentHead_MappingDTO Decative([FromBody] Master_DepartmentHead_MappingDTO data)
        {
            return inter.Decative(data);
        }
        [Route("Editdata")]
        public Master_DepartmentHead_MappingDTO Editdata([FromBody] Master_DepartmentHead_MappingDTO data)
        {
            return inter.Editdata(data);
        }
        

    }
}
