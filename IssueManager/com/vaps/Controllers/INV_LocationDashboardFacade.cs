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
    public class INV_LocationDashboardFacade : Controller
    {

        public INV_LocationDashboardInterface _objinter;

        public INV_LocationDashboardFacade(INV_LocationDashboardInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("onfloordetails")]
        public Task<INV_LocationDashboardDTO> getfloordetail([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.getfloordetail(data);
        }
        [Route("getemployeedetail")]
        public INV_LocationDashboardDTO getemployeedetail([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.getemployeedetail(data);
        }

        [Route("SaveDetails")]
        public INV_LocationDashboardDTO SaveDetails([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.SaveDetails(data);
        }
        //removelocationemployee
        [Route("removelocationemployee")]
        public INV_LocationDashboardDTO removelocationemployee([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.removelocationemployee(data);
        }
        //OnChangeLocation
        [Route("OnChangeLocation")]
        public INV_LocationDashboardDTO OnChangeLocation([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.OnChangeLocation(data);
        }
        //suplier
        [Route("OnChange_Suplier")]
        public INV_LocationDashboardDTO OnChange_Suplier([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.OnChange_Suplier(data);
        }
        [Route("Suplier_Details")]
        public INV_LocationDashboardDTO Suplier_Details([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.Suplier_Details(data);
        }
        [Route("Details_supplier")]
        public INV_LocationDashboardDTO Details_supplier([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.Details_supplier(data);
        }


        //by sushma
        [Route("getemployeedetailCommon")]
        public INV_LocationDashboardDTO getemployeedetailCommon([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.getemployeedetailCommon(data);
        }

        [Route("SaveEmployeeAssets")]
        public INV_LocationDashboardDTO SaveEmployeeAssets([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.SaveEmployeeAssets(data);
        }
        //Sanjai
        [Route("GetPendingAcknowledgementReport")]
        public INV_LocationDashboardDTO GetPendingAcknowledgementReport([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.GetPendingAcknowledgementReport(data);
        }

        [Route("SavePendingAssetLocationTransfer")]
        public INV_LocationDashboardDTO SavePendingAssetLocationTransfer([FromBody] INV_LocationDashboardDTO data)
        {
            return _objinter.SavePendingAssetLocationTransfer(data);
        }
    }
}
