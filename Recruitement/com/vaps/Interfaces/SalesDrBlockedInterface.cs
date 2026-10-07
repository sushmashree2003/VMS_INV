using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
   public interface SalesDrBlockedInterface
   {
        EmployeeDataDto OnLoadData(EmployeeDataDto dto);
        EmployeeDataDto Save(EmployeeDataDto dto);
        EmployeeDataDto Deactivate(EmployeeDataDto dto);

        Ivrm_ModuleDto OnLoadIvrmData(Ivrm_ModuleDto dto);
        Ivrm_ModuleDto ISM_Master_Module_BOMSave(Ivrm_ModuleDto dto);

        Ivrm_ModuleDto SaveModuleOrder(Ivrm_ModuleDto dto);
        Ivrm_ModuleDto MasterDataDeactivate(Ivrm_ModuleDto dto);
        ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionSave(ISM_Sales_Lead_Negaotion_DesignationDto dto);
        ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionDeactivate(ISM_Sales_Lead_Negaotion_DesignationDto dto);
        ISM_Sales_Lead_Negaotion_DesignationDto SalesQuatationReport(ISM_Sales_Lead_Negaotion_DesignationDto dto);
        ISM_Sales_Lead_Negaotion_DesignationDto OnLoadDataReport(ISM_Sales_Lead_Negaotion_DesignationDto dto);


        ISM_Sales_Lead_Negaotion_DesignationDto Employee_Ind_Detail(ISM_Sales_Lead_Negaotion_DesignationDto dto);
        ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Detail_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto);
        ISM_Sales_Lead_Negaotion_DesignationDto Sales_Lead_Demo_Swapping_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto);

        ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Payment_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto);

    }
}
