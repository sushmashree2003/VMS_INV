using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class AddVisitorsDelegate
    {
        CommonDelegate<AddVisitorsDTO, AddVisitorsDTO> COMVISITOR = new CommonDelegate<AddVisitorsDTO, AddVisitorsDTO>();

        public AddVisitorsDTO saveData(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/saveData/");
        }
        public AddVisitorsDTO getDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/getDetails/");
        }
        public AddVisitorsDTO EditDetails(AddVisitorsDTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "AddVisitorsFacade/EditDetails/");
        }
        public AddVisitorsDTO deactivate(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/deactivate/");
        }
        public AddVisitorsDTO changeofradio(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/changeofradio/");
        }
        public AddVisitorsDTO GetVisitorDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/GetVisitorDetails/");
        }
        public AddVisitorsDTO Getmultivisitordetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/Getmultivisitordetails/");
        }       
        public AddVisitorsDTO UpdateStatus(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/UpdateStatus/");
        }
        public AddVisitorsDTO getAssignDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/getAssignDetails/");
        }
        public AddVisitorsDTO getVisitorAssignDetails(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/getVisitorAssignDetails/");
        }
        public AddVisitorsDTO saveAssignedData(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/saveAssignedData/");
        }
        public AddVisitorsDTO SearchAppVisitors(AddVisitorsDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AddVisitorsFacade/SearchAppVisitors/");
        }
        public AddVisitorsDTO searchfilter(AddVisitorsDTO sel)
        {
            return COMVISITOR.POSTDataVisitors(sel, "AddVisitorsFacade/searchfilter");
        }
        public AddVisitorsDTO visitornameschange(AddVisitorsDTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "AddVisitorsFacade/visitornameschange");
        }
        public AddVisitorsDTO GetVisitorAssginDetails(AddVisitorsDTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "AddVisitorsFacade/GetVisitorAssginDetails");
        }
        public AddVisitorsDTO addtocart(AddVisitorsDTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "AddVisitorsFacade/addtocart");
        }
        //Tenent Visitor Managment
        public AddVisitorsDTO getTenentVisitorDetails(AddVisitorsDTO sel)
        {
            return COMVISITOR.POSTDataVisitors(sel, "AddVisitorsFacade/getTenentVisitorDetails");
        }
        public AddVisitorsDTO GenerateQRCode(AddVisitorsDTO sel)
        {
            return COMVISITOR.POSTDataVisitors(sel, "AddVisitorsFacade/GenerateQRCode");
        }

        public AddVisitorsDTO QRCODECertificates(AddVisitorsDTO dto)
        {
            return COMVISITOR.POSTDataVisitors(dto, "AddVisitorsFacade/QRCODECertificates");
        }
    }
}
 