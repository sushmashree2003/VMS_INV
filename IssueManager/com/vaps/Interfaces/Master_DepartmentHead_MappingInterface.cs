using IssueManager.com.vaps.Services;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface Master_DepartmentHead_MappingInterface
    {
        Master_DepartmentHead_MappingDTO loaddata(Master_DepartmentHead_MappingDTO data);
        Master_DepartmentHead_MappingDTO savedata(Master_DepartmentHead_MappingDTO data);
        Master_DepartmentHead_MappingDTO depchange(Master_DepartmentHead_MappingDTO data);
        Master_DepartmentHead_MappingDTO Decative(Master_DepartmentHead_MappingDTO data);
        Master_DepartmentHead_MappingDTO Editdata(Master_DepartmentHead_MappingDTO data);
       
    }
}
