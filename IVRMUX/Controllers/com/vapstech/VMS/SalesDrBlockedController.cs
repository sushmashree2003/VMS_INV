using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.VMS.Sales;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com;

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class SalesDrBlockedController : Controller
    {

        SalesDrBlockedDelegate sales = new SalesDrBlockedDelegate();


        [Route("OnLoadData/{id:int}")]
        public EmployeeDataDto OnLoadData(int id)
        {
            EmployeeDataDto dto = new EmployeeDataDto();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.OnLoadData(dto);
        }


        [Route("Save")]
        public EmployeeDataDto Save([FromBody] EmployeeDataDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.Save(dto);
        }

        [Route("Deactivate")]
        public EmployeeDataDto Deactivate([FromBody] EmployeeDataDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.Deactivate(dto);
        }

        [Route("Edit/{id:int}")]
        public EmployeeDataDto Edit(int id)
        {
            EmployeeDataDto dto = new EmployeeDataDto();
            // dto.ISMCLTPRBOM_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return sales.Edit(dto);
        }

        [Route("OnLoadIvrmData/{id:int}")]
        public Ivrm_ModuleDto OnLoadIvrmData(int id)
        {
            Ivrm_ModuleDto dto = new Ivrm_ModuleDto();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.OnLoadIvrmData(dto);
        }

        [Route("ISM_Master_Module_BOMSave")]
        public Ivrm_ModuleDto ISM_Master_Module_BOMSave([FromBody] Ivrm_ModuleDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.ISM_Master_Module_BOMSave(dto);
        }

        [Route("SaveModuleOrder")]
        public Ivrm_ModuleDto SaveModuleOrder([FromBody] Ivrm_ModuleDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.SaveModuleOrder(dto);
        }

        [Route("MasterDataDeactivate")]
        public Ivrm_ModuleDto MasterDataDeactivate([FromBody] Ivrm_ModuleDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.MasterDataDeactivate(dto);
        }

        //[Route("OnLoadSalesNegaotionData/{id:int}")]
        //public ISM_Sales_Lead_Negaotion_DesignationDto OnLoadSalesNegaotionData(int id)
        //{
        //    ISM_Sales_Lead_Negaotion_DesignationDto dto = new ISM_Sales_Lead_Negaotion_DesignationDto();
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    return sales.OnLoadSalesNegaotionData(dto);
        //}

        [Route("SalesNegaotionSave")]
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionSave ([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.SalesNegaotionSave(dto);
        }
        [Route("SalesNegaotionDeactivate")]
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionDeactivate([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.SalesNegaotionDeactivate(dto);
        }

        [Route("SalesQuatationReport")]
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesQuatationReport([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.SalesQuatationReport(dto);
        }

        [Route("OnLoadDataReport/{id:int}")]
        public ISM_Sales_Lead_Negaotion_DesignationDto OnLoadDataReport(int id)
        {
            ISM_Sales_Lead_Negaotion_DesignationDto dto = new ISM_Sales_Lead_Negaotion_DesignationDto();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.OnLoadDataReport(dto);
        }


        [Route("Employee_Ind_Detail")]
        public ISM_Sales_Lead_Negaotion_DesignationDto Employee_Ind_Detail([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.Employee_Ind_Detail(dto);
        }

        [Route("Sales_Negotation_Detail_Report")]
        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Detail_Report([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.Sales_Negotation_Detail_Report(dto);
        }


        [Route("Sales_Lead_Demo_Swapping_Report")]
        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Lead_Demo_Swapping_Report([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.Sales_Lead_Demo_Swapping_Report(dto);
        }

        [Route("Sales_Negotation_Payment_Report")]
        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Payment_Report([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sales.Sales_Negotation_Payment_Report(dto);
        }


    }
}


//MasterDataDeactivate