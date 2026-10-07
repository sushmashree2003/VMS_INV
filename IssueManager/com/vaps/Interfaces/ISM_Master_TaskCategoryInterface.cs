using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ISM_Master_TaskCategoryInterface
    {
        ISM_Master_TaskCategory_DTO loaddata(ISM_Master_TaskCategory_DTO data);
        ISM_Master_TaskCategory_DTO save(ISM_Master_TaskCategory_DTO data);
        ISM_Master_TaskCategory_DTO deactive(ISM_Master_TaskCategory_DTO data);
        ISM_Master_TaskCategory_DTO EditData(ISM_Master_TaskCategory_DTO data);

        //============== Master Task Categary Check List===========
        ISM_Master_TaskCategory_DTO loaddata_checklist(ISM_Master_TaskCategory_DTO data);
        ISM_Master_TaskCategory_DTO getcategory(ISM_Master_TaskCategory_DTO data);
        ISM_Master_TaskCategory_DTO savechecklist(ISM_Master_TaskCategory_DTO data);
        ISM_Master_TaskCategory_DTO EditData_checklist(ISM_Master_TaskCategory_DTO data);
        ISM_Master_TaskCategory_DTO deactive_checklist(ISM_Master_TaskCategory_DTO data);
    }
}
