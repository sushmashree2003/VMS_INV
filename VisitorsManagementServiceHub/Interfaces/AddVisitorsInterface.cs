using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace VisitorsManagementServiceHub.Interfaces
{
    public interface AddVisitorsInterface
    {
        AddVisitorsDTO getDetails(AddVisitorsDTO data);
        AddVisitorsDTO saveData(AddVisitorsDTO data);
        AddVisitorsDTO EditDetails(AddVisitorsDTO id);
        AddVisitorsDTO deactivate(AddVisitorsDTO data);      
        Task<AddVisitorsDTO> changeofradio(AddVisitorsDTO data);
        AddVisitorsDTO UpdateStatus(AddVisitorsDTO data);
        AddVisitorsDTO GetVisitorDetails(AddVisitorsDTO data);
        AddVisitorsDTO Getmultivisitordetails(AddVisitorsDTO data);
       
        AddVisitorsDTO getAssignDetails(AddVisitorsDTO data);
        AddVisitorsDTO getVisitorAssignDetails(AddVisitorsDTO data);
        AddVisitorsDTO saveAssignedData(AddVisitorsDTO data);
        AddVisitorsDTO SearchAppVisitors(AddVisitorsDTO data);
        AddVisitorsDTO searchfilter(AddVisitorsDTO data);
        AddVisitorsDTO visitornameschange(AddVisitorsDTO id);
        AddVisitorsDTO GetVisitorAssginDetails(AddVisitorsDTO id);
        AddVisitorsDTO addtocart(AddVisitorsDTO id);
        //Tenent Visitor Managment
        AddVisitorsDTO getTenentVisitorDetails(AddVisitorsDTO data);

        Task<AddVisitorsDTO> QRCODECertificates(AddVisitorsDTO data);
        AddVisitorsDTO FetchDetails(AddVisitorsDTO data);
        


    }
}
