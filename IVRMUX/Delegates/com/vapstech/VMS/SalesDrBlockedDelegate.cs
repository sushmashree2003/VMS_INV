using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class SalesDrBlockedDelegate
    {
        CommonVMSDelegate<EmployeeDataDto, EmployeeDataDto> CMMICP = new CommonVMSDelegate<EmployeeDataDto, EmployeeDataDto>();

        CommonVMSDelegate<Ivrm_ModuleDto, Ivrm_ModuleDto> CMM = new CommonVMSDelegate<Ivrm_ModuleDto, Ivrm_ModuleDto>();
        CommonVMSDelegate<ISM_Sales_Lead_Negaotion_DesignationDto, ISM_Sales_Lead_Negaotion_DesignationDto> CM = new CommonVMSDelegate<ISM_Sales_Lead_Negaotion_DesignationDto, ISM_Sales_Lead_Negaotion_DesignationDto>();
        public EmployeeDataDto OnLoadData(EmployeeDataDto dto)
        {
            return CMMICP.POSTData(dto, "SalesDrBlockedFacade/OnLoadData/");
        }
        public EmployeeDataDto Save(EmployeeDataDto dto)
        {
            return CMMICP.POSTData(dto, "SalesDrBlockedFacade/Save/");
        }
        public EmployeeDataDto Deactivate(EmployeeDataDto dto)
        {
            return CMMICP.POSTData(dto, "SalesDrBlockedFacade/Deactivate/");
        }
        public EmployeeDataDto Edit(EmployeeDataDto dto)
        {
            return CMMICP.POSTData(dto, "SalesDrBlockedFacade/Edit/");
        }
        public Ivrm_ModuleDto OnLoadIvrmData(Ivrm_ModuleDto dto)
        {
            return CMM.POSTData(dto, "SalesDrBlockedFacade/OnLoadIvrmData/");
        }

        public Ivrm_ModuleDto ISM_Master_Module_BOMSave(Ivrm_ModuleDto dto)
        {
            return CMM.POSTData(dto, "SalesDrBlockedFacade/ISM_Master_Module_BOMSave/");
        }

        public Ivrm_ModuleDto SaveModuleOrder(Ivrm_ModuleDto dto)
        {
            return CMM.POSTData(dto, "SalesDrBlockedFacade/SaveModuleOrder/");
        }
        
        public Ivrm_ModuleDto MasterDataDeactivate(Ivrm_ModuleDto dto)
        {
            return CMM.POSTData(dto, "SalesDrBlockedFacade/MasterDataDeactivate/");
        }
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionSave(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return CM.POSTData(dto, "SalesDrBlockedFacade/SalesNegaotionSave/");
        }
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionDeactivate(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return CM.POSTData(dto, "SalesDrBlockedFacade/SalesNegaotionDeactivate/");
        }
        public ISM_Sales_Lead_Negaotion_DesignationDto SalesQuatationReport(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return CM.POSTData(dto, "SalesDrBlockedFacade/SalesQuatationReport/");
        }


        public ISM_Sales_Lead_Negaotion_DesignationDto OnLoadDataReport(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return CM.POSTData(dto, "SalesDrBlockedFacade/OnLoadDataReport/");
        }
        public ISM_Sales_Lead_Negaotion_DesignationDto Employee_Ind_Detail(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return CM.POSTData(dto, "SalesDrBlockedFacade/Employee_Ind_Detail/");
        }
        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Detail_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return CM.POSTData(dto, "SalesDrBlockedFacade/Sales_Negotation_Detail_Report/");
        }
        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Lead_Demo_Swapping_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return CM.POSTData(dto, "SalesDrBlockedFacade/Sales_Lead_Demo_Swapping_Report/");
        }

        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Payment_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            return CM.POSTData(dto, "SalesDrBlockedFacade/Sales_Negotation_Payment_Report/");
        }

    }
}


//ISM_Master_Module_BOM