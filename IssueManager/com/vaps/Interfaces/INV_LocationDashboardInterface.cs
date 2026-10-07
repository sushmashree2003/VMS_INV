using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface INV_LocationDashboardInterface
    {
        Task<INV_LocationDashboardDTO> getfloordetail(INV_LocationDashboardDTO data);
        INV_LocationDashboardDTO getemployeedetail(INV_LocationDashboardDTO data);
        INV_LocationDashboardDTO SaveDetails(INV_LocationDashboardDTO data);
        INV_LocationDashboardDTO OnChangeLocation(INV_LocationDashboardDTO data);
        //suplier
        INV_LocationDashboardDTO OnChange_Suplier(INV_LocationDashboardDTO data);
        INV_LocationDashboardDTO Suplier_Details(INV_LocationDashboardDTO data);
        INV_LocationDashboardDTO Details_supplier(INV_LocationDashboardDTO data);
        //removelocationemployee
        INV_LocationDashboardDTO removelocationemployee(INV_LocationDashboardDTO data);

        //sushma
        INV_LocationDashboardDTO getemployeedetailCommon(INV_LocationDashboardDTO data);
        INV_LocationDashboardDTO SaveEmployeeAssets(INV_LocationDashboardDTO data);
        //Sanjai
        INV_LocationDashboardDTO GetPendingAcknowledgementReport(INV_LocationDashboardDTO data);
        INV_LocationDashboardDTO SavePendingAssetLocationTransfer(INV_LocationDashboardDTO data);
    }
}
