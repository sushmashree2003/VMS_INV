using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_Master_TaskCategoryDelegate
    {
        CommonDelegate<ISM_Master_TaskCategory_DTO, ISM_Master_TaskCategory_DTO> comm = new CommonDelegate<ISM_Master_TaskCategory_DTO, ISM_Master_TaskCategory_DTO>();
        public ISM_Master_TaskCategory_DTO loaddata(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/loaddata");
        }
        public ISM_Master_TaskCategory_DTO save(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/save");
        }
        public ISM_Master_TaskCategory_DTO deactive(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/deactive");
        }
        public ISM_Master_TaskCategory_DTO EditData(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/EditData");
        }

        //============== Master Task Categary Check List===========
        public ISM_Master_TaskCategory_DTO loaddata_checklist(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/loaddata_checklist");
        }
        public ISM_Master_TaskCategory_DTO getcategory(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/getcategory");
        }
         public ISM_Master_TaskCategory_DTO savechecklist(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/savechecklist");
        }
         public ISM_Master_TaskCategory_DTO EditData_checklist(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/EditData_checklist");
        }
         public ISM_Master_TaskCategory_DTO deactive_checklist(ISM_Master_TaskCategory_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategoryFacade/deactive_checklist");
        }


    }
}
