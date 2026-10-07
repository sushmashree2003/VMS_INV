using CommonLibrary;
//using IVRMUX.Controllers.com.vapstech.IssueManager.com.vaps.Services;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class Master_DepartmentHead_MappingDelegate
    {
        CommonDelegate<Master_DepartmentHead_MappingDTO, Master_DepartmentHead_MappingDTO> comm = new CommonDelegate<Master_DepartmentHead_MappingDTO, Master_DepartmentHead_MappingDTO>();
        public Master_DepartmentHead_MappingDTO loaddata(Master_DepartmentHead_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "Master_DepartmentHead_MappingFacade/loaddata");
        }
        public Master_DepartmentHead_MappingDTO savedata(Master_DepartmentHead_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "Master_DepartmentHead_MappingFacade/savedata");
        }
        public Master_DepartmentHead_MappingDTO depchange(Master_DepartmentHead_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "Master_DepartmentHead_MappingFacade/depchange");
        }
        public Master_DepartmentHead_MappingDTO Decative(Master_DepartmentHead_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "Master_DepartmentHead_MappingFacade/Decative");
        }
        public Master_DepartmentHead_MappingDTO Editdata(Master_DepartmentHead_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "Master_DepartmentHead_MappingFacade/Editdata");
        }
        

    }
}
