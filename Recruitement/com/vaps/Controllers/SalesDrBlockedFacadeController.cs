using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SalesDrBlockedFacadeController : Controller
    {
        public SalesDrBlockedInterface _intface;
        public SalesDrBlockedFacadeController(SalesDrBlockedInterface intface)
        {
            _intface = intface;
        }

        [Route("OnLoadData/")]
        public EmployeeDataDto OnLoadData([FromBody] EmployeeDataDto dto)
        {
            return _intface.OnLoadData(dto);
        }

        [Route("Save/")]
        public EmployeeDataDto Save([FromBody] EmployeeDataDto dto)
        {
            return _intface.Save(dto);
        }


        [Route("Deactivate/")]
        public EmployeeDataDto Deactivate([FromBody] EmployeeDataDto dto)
        {
            return _intface.Deactivate(dto);
        }

        [Route("OnLoadIvrmData/")]
        public Ivrm_ModuleDto OnLoadIvrmData([FromBody] Ivrm_ModuleDto dto)
        {
            return _intface.OnLoadIvrmData(dto);
        }


        [Route("ISM_Master_Module_BOMSave/")]
        public Ivrm_ModuleDto Save([FromBody] Ivrm_ModuleDto dto)
        {
            return _intface.ISM_Master_Module_BOMSave(dto);
        }


        [Route("SaveModuleOrder/")]
        public Ivrm_ModuleDto SaveModuleOrder([FromBody] Ivrm_ModuleDto dto)
        {
            return _intface.SaveModuleOrder(dto);
        }
        
        [Route("MasterDataDeactivate/")]
        public Ivrm_ModuleDto MasterDataDeactivate([FromBody] Ivrm_ModuleDto dto)
        {
            return _intface.MasterDataDeactivate(dto);
        }
        [Route("SalesNegaotionSave/")]
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionSave([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return _intface.SalesNegaotionSave(dto);
        }

        [Route("SalesNegaotionDeactivate/")]
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionDeactivate([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return _intface.SalesNegaotionDeactivate(dto);
        }


        [Route("SalesQuatationReport/")]
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesQuatationReport([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return _intface.SalesQuatationReport(dto);
        }

        [Route("OnLoadDataReport/")]
        public ISM_Sales_Lead_Negaotion_DesignationDto OnLoadDataReport([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return _intface.OnLoadDataReport(dto);
        }

        [Route("Employee_Ind_Detail/")]
        public ISM_Sales_Lead_Negaotion_DesignationDto Employee_Ind_Detail([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return _intface.Employee_Ind_Detail(dto);
        }


        [Route("Sales_Negotation_Detail_Report/")]
        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Detail_Report([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return _intface.Sales_Negotation_Detail_Report(dto);
        }

        [Route("Sales_Lead_Demo_Swapping_Report/")]
        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Lead_Demo_Swapping_Report([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return _intface.Sales_Lead_Demo_Swapping_Report(dto);
        }
        [Route("Sales_Negotation_Payment_Report/")]
        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Payment_Report([FromBody] ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return _intface.Sales_Negotation_Payment_Report(dto);
        }

    }
}