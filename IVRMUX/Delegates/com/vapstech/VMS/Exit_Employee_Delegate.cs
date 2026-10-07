using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class Exit_Employee_Delegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonVMSDelegate<ISM_Resignation_Master_Reasons_DTO, ISM_Resignation_Master_Reasons_DTO> COMMI = new CommonVMSDelegate<ISM_Resignation_Master_Reasons_DTO, ISM_Resignation_Master_Reasons_DTO>();
        CommonVMSDelegate<ISM_Resignation_Master_CheckLists_DTO, ISM_Resignation_Master_CheckLists_DTO> COMMRC = new CommonVMSDelegate<ISM_Resignation_Master_CheckLists_DTO, ISM_Resignation_Master_CheckLists_DTO>();
        CommonVMSDelegate<ISM_Resignation_DTO, ISM_Resignation_DTO> COMMR = new CommonVMSDelegate<ISM_Resignation_DTO, ISM_Resignation_DTO>();
        CommonVMSDelegate<ISM_Resignation_DTO.fullANDFinalSettlementDTO, ISM_Resignation_DTO.fullANDFinalSettlementDTO> COMMRF = new CommonVMSDelegate<ISM_Resignation_DTO.fullANDFinalSettlementDTO, ISM_Resignation_DTO.fullANDFinalSettlementDTO>();

        CommonVMSDelegate<ISM_Certificate_RequestDTO, ISM_Certificate_RequestDTO> COMM = new CommonVMSDelegate<ISM_Certificate_RequestDTO, ISM_Certificate_RequestDTO>();
        CommonVMSDelegate<ISM_Resignation_ChecKLists_DTO, ISM_Resignation_ChecKLists_DTO> COMRC = new CommonVMSDelegate<ISM_Resignation_ChecKLists_DTO, ISM_Resignation_ChecKLists_DTO>();
        CommonVMSDelegate<ISM_Resignation_DTO.LetterGenerationDTO, ISM_Resignation_DTO.LetterGenerationDTO> COMRCL = new CommonVMSDelegate<ISM_Resignation_DTO.LetterGenerationDTO, ISM_Resignation_DTO.LetterGenerationDTO>();

        public ISM_Resignation_DTO get_depts(ISM_Resignation_DTO maspage)
        {
            return COMMR.POSTData(maspage, "Exit_Employee_Facade/get_depts/");
        }
        public ISM_Resignation_DTO get_desig(ISM_Resignation_DTO maspage)
        {
            return COMMR.POSTData(maspage, "Exit_Employee_Facade/get_desig/");
        }

        //===========================Reason master Start====================================
        public ISM_Resignation_Master_Reasons_DTO Get_Reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            return COMMI.POSTData(dto, "Exit_Employee_Facade/Get_Reason/");
        }
        public ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            return COMMI.POSTData(dto, "Exit_Employee_Facade/Save_Edit_Reason/");
        }
        public ISM_Resignation_Master_Reasons_DTO get_details_reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            return COMMI.POSTData(dto, "Exit_Employee_Facade/get_details_reason/");
        }


        public ISM_Resignation_Master_Reasons_DTO active_deactive_reason(ISM_Resignation_Master_Reasons_DTO dto)
        {

            return COMMI.POSTData(dto, "Exit_Employee_Facade/active_deactive_reason/");
        }
        //==========================================End=============================================
        //===========================Check mlist master Start====================================
        public ISM_Resignation_Master_CheckLists_DTO Get_Checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/Get_Checklist/");
        }
        public ISM_Resignation_Master_CheckLists_DTO company(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/company/");
        }
        public ISM_Resignation_Master_CheckLists_DTO get_company(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/get_company/");
        }
        public ISM_Resignation_Master_CheckLists_DTO get_ApproverDepartDesign_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/get_ApproverDepartDesign_checklist/");
        }
        public ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/Save_Edit_Checklist/");
        }
        public ISM_Resignation_Master_CheckLists_DTO get_details_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/get_details_checklist/");
        }
        public ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/active_deactive_checklist/");
        }


        //==========================================End=============================================

        //==================================Exit Employee Process Start=================================

        public ISM_Resignation_DTO Load_all_data(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Load_all_data/");
        }
        
        public ISM_Resignation_DTO GetAllRelData(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/GetAllRelData/");
        }
        public ISM_Resignation_DTO GetAllRelData1(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/GetAllRelData1/");
        }
        public ISM_Resignation_DTO Exit_empl_SaveEdit(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Exit_empl_SaveEdit/");
        }
        public ISM_Resignation_DTO ExitApproveal(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/ExitApproveal/");
        }
        public ISM_Resignation_DTO Exit_empl_AccRej(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Exit_empl_AccRej/");
        }
        public ISM_Resignation_DTO HR_RelivingApply(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/HR_RelivingApply/");
        }
        public ISM_Resignation_DTO HR_EmployeeDetails(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/HR_EmployeeDetails/");
        }
        public ISM_Resignation_DTO Edit_Employee(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Edit_Employee/");
        }
        public ISM_Resignation_DTO c_approve_new(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/c_approve_new/");
        }
        public ISM_Resignation_DTO Savedata_td(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Savedata_td/");
        }
        public ISM_Resignation_DTO edit_relieving(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/edit_relieving/");
        }
        public ISM_Resignation_DTO.LetterGenerationDTO Savedata_print(ISM_Resignation_DTO.LetterGenerationDTO dto)
        {

            return COMRCL.POSTData(dto, "Exit_Employee_Facade/Savedata_print/");
        }
        public ISM_Resignation_DTO print_exit_employee(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/print_exit_employee/");
        }
        public ISM_Resignation_DTO download_doc(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/download_doc/");
        }
        //=========================Report===========================
        public ISM_Resignation_DTO get_all_data_R(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/get_all_data_R/");
        }
        public ISM_Resignation_DTO showdetails_R(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/showdetails_R/");
        }
        public ISM_Resignation_DTO get_all_relieving_data_R(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/get_all_relieving_data_R/");
        }
        public ISM_Resignation_DTO showdetails_relieving_R(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/showdetails_relieving_R/");
        }
       
        public ISM_Resignation_DTO relieving_exit_employee_view(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/relieving_exit_employee_view/");
        }

        //GAUTAM
        public ISM_Resignation_DTO loadEmployeeData(ISM_Resignation_DTO dto)
        {
            return COMMR.POSTData(dto, "Exit_Employee_Facade/loadEmployeeData/");
        }
        public ISM_Resignation_DTO sendResignationMail(ISM_Resignation_DTO dto)
        {
            return COMMR.POSTData(dto, "Exit_Employee_Facade/sendResignationMail/");
        }
        public ISM_Resignation_DTO chcklistdocs(ISM_Resignation_DTO dto)
        {
            return COMMR.POSTData(dto, "Exit_Employee_Facade/chcklistdocs/");
        }
        public ISM_Resignation_DTO savechecklists(ISM_Resignation_DTO dto)
        {
            return COMMR.POSTData(dto, "Exit_Employee_Facade/savechecklists/");
        }
        public ISM_Resignation_DTO GetoverallReport(ISM_Resignation_DTO dto)
        {
            return COMMR.POSTData(dto, "Exit_Employee_Facade/GetoverallReport/");
        }
        public ISM_Resignation_DTO approverRemark(ISM_Resignation_DTO dto)
        {
            return COMMR.POSTData(dto, "Exit_Employee_Facade/approverRemark/");
        }
        //GAUTAM

        // Exit Overall Report ---- Aneeket

        public ISM_Resignation_Master_Reasons_DTO getalldetails(ISM_Resignation_Master_Reasons_DTO dto)
        {
            return COMMI.POSTData(dto, "Exit_Employee_Facade/getalldetails/");
        }


        public ISM_Resignation_Master_Reasons_DTO getReport(ISM_Resignation_Master_Reasons_DTO dto)
        {
            return COMMI.POSTData(dto, "Exit_Employee_Facade/getReport/");
        }

        //---------------------------ISM_Certificate_Request---------------------
        public ISM_Certificate_RequestDTO getloaddata(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/getloaddata");
        }

        public ISM_Certificate_RequestDTO savedetails(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/savedata/");
        }
        public ISM_Certificate_RequestDTO deactive(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/deactive/");
        }
        public ISM_Certificate_RequestDTO Edit(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/Edit/");
        }
        public ISM_Certificate_RequestDTO Viewdata(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/Viewdata");
        }

        //--------certificate approval----------------------


        public ISM_Certificate_RequestDTO loaddata(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/loaddata");
        }




        public ISM_Certificate_RequestDTO viewuploadflies(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/viewuploadflies");
        }

        public ISM_Certificate_RequestDTO approvedrecord(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/approvedrecord");
        }

        public ISM_Resignation_ChecKLists_DTO checklistloaddata(ISM_Resignation_ChecKLists_DTO data)
        {
            return COMRC.POSTData(data, "Exit_Employee_Facade/checklistloaddata");
        }
        public ISM_Resignation_ChecKLists_DTO viewchecklistdata(ISM_Resignation_ChecKLists_DTO data)
        {
            return COMRC.POSTData(data, "Exit_Employee_Facade/viewchecklistdata");
        }
        public ISM_Resignation_ChecKLists_DTO checklistapprovedrecord(ISM_Resignation_ChecKLists_DTO data)
        {
            return COMRC.POSTData(data, "Exit_Employee_Facade/checklistapprovedrecord");
        }


        //------------------------
        public ISM_Certificate_RequestDTO Onload(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/Onload");
        }

        public ISM_Certificate_RequestDTO getapprovalreport(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/getapprovalreport");
        }
        public ISM_Certificate_RequestDTO DocView(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/DocView");
        }
        public ISM_Certificate_RequestDTO getIEDetails(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/getIEDetails");
        }
        public ISM_Certificate_RequestDTO getcertificate(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/getcertificate");
        }
        public ISM_Certificate_RequestDTO getadminloaddata(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/getadminloaddata");
        }
        public ISM_Certificate_RequestDTO saveadmindetails(ISM_Certificate_RequestDTO data)
        {
            return COMM.POSTData(data, "Exit_Employee_Facade/saveadmindetails");
        }
        public ISM_Resignation_DTO.fullANDFinalSettlementDTO FANDFSettlement(ISM_Resignation_DTO.fullANDFinalSettlementDTO data)
        {
            return COMMRF.POSTData(data, "Exit_Employee_Facade/FANDFSettlement");
        }
        public ISM_Resignation_DTO.fullANDFinalSettlementDTO DamageSave(ISM_Resignation_DTO.fullANDFinalSettlementDTO data)
        {
            return COMMRF.POSTData(data, "Exit_Employee_Facade/DamageSave");
        }
        


    }
}
