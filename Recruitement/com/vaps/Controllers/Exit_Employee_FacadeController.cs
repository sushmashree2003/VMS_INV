using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/Exit_Employee_Facade")]
    public class Exit_Employee_FacadeController : Controller
    {
        public Exit_Employee_Interface _eei;

        public VMSContext _vmsconte;

        public Exit_Employee_FacadeController(Exit_Employee_Interface ee)
        {
            _eei = ee;
        }

        [Route("get_depts")]
        public ISM_Resignation_DTO get_depts([FromBody]ISM_Resignation_DTO dto)
        {
            return _eei.get_depts(dto);
        }
        [Route("get_desig")]
        public ISM_Resignation_DTO get_desig([FromBody]ISM_Resignation_DTO dto)
        {
            return _eei.get_desig(dto);
        }

        //===========================Reason master Start====================================
        [Route("Get_Reason")]
        public ISM_Resignation_Master_Reasons_DTO Get_Reason([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.Get_Reason(dto);
        }

        [Route("Save_Edit_Reason")]
        public ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.Save_Edit_Reason(dto);
        }
        [Route("get_details_reason")]
        public ISM_Resignation_Master_Reasons_DTO get_details_reason([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.get_details_reason(dto);
        }

        [Route("active_deactive_reason")]
        public ISM_Resignation_Master_Reasons_DTO active_deactive_reason([FromBody]ISM_Resignation_Master_Reasons_DTO dto)
        {

            return _eei.active_deactive_reason(dto);
        }
        //==========================================End=============================================
        //===========================check list master Start====================================
        [Route("Get_Checklist")]
        public ISM_Resignation_Master_CheckLists_DTO Get_Checklist([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return _eei.Get_Checklist(dto);
        }
        [Route("company")]
        public ISM_Resignation_Master_CheckLists_DTO company([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return _eei.company(dto);
        }
        [Route("get_company")]
        public ISM_Resignation_Master_CheckLists_DTO get_company([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return _eei.get_company(dto);
        }
        [Route("get_ApproverDepartDesign_checklist")]
        public ISM_Resignation_Master_CheckLists_DTO get_ApproverDepartDesign_checklist([FromBody]ISM_Resignation_Master_CheckLists_DTO dto)
        {

            return _eei.get_ApproverDepartDesign_checklist(dto);
        }
        [Route("Save_Edit_Checklist")]
        public ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return _eei.Save_Edit_Checklist(dto);
        }
        [Route("get_details_checklist")]
        public ISM_Resignation_Master_CheckLists_DTO get_details_checklist([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return _eei.get_details_checklist(dto);
        }

        [Route("active_deactive_checklist")]
        public ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist([FromBody]ISM_Resignation_Master_CheckLists_DTO dto)
        {

            return _eei.active_deactive_checklist(dto);
        }
        //==========================================End=============================================
        //==================================Exit Employee Process Start=================================

        [Route("Load_all_data")]
        public ISM_Resignation_DTO Load_all_data([FromBody] ISM_Resignation_DTO dto)
        {

            return _eei.Load_all_data(dto);
        }


        [Route("GetAllRelData")]
        public ISM_Resignation_DTO GetAllRelData([FromBody] ISM_Resignation_DTO dto)
        {

            return _eei.GetAllRelData(dto);
        }
        [Route("GetAllRelData1")]
        public ISM_Resignation_DTO GetAllRelData1([FromBody] ISM_Resignation_DTO dto)
        {

            return _eei.GetAllRelData1(dto);
        }

        [Route("Exit_empl_SaveEdit")]
        public ISM_Resignation_DTO Exit_empl_SaveEdit([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Exit_empl_SaveEdit(dto);
        }
        [Route("ExitApproveal")]
        public ISM_Resignation_DTO ExitApproveal([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.ExitApproveal(dto);
        }
        [Route("Exit_empl_AccRej")]
        public ISM_Resignation_DTO Exit_empl_AccRej([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Exit_empl_AccRej(dto);
        }
        [Route("HR_RelivingApply")]
        public ISM_Resignation_DTO HR_RelivingApply([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.HR_RelivingApply(dto);
        }
        [Route("HR_EmployeeDetails")]
        public ISM_Resignation_DTO HR_EmployeeDetails([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.HR_EmployeeDetails(dto);
        }
        [Route("Edit_Employee")]
        public ISM_Resignation_DTO Edit_Employee([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Edit_Employee(dto);
        }
        [Route("c_approve_new")]
        public ISM_Resignation_DTO c_approve_new([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.c_approve_new(dto);
        }
        [Route("Savedata_td")]
        public ISM_Resignation_DTO Savedata_td([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Savedata_td(dto);
        }
        [Route("edit_relieving")]
        public ISM_Resignation_DTO edit_relieving([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.edit_relieving(dto);
        }
        [Route("Savedata_print")]
        public Task<ISM_Resignation_DTO.LetterGenerationDTO> Savedata_print([FromBody] ISM_Resignation_DTO.LetterGenerationDTO dto)
        {
            return _eei.Savedata_printAsync(dto);
        }
        [Route("print_exit_employee")]
        public ISM_Resignation_DTO print_exit_employee([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.print_exit_employee(dto);
        }
        [Route("download_doc")]
        public ISM_Resignation_DTO download_doc([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.download_doc(dto);
        }
        //===========================Reports========================
        [Route("get_all_data_R")]
        public ISM_Resignation_DTO get_all_data_R([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.get_all_data_R(dto);
        }
        [Route("showdetails_R")]
        public Task<ISM_Resignation_DTO> showdetails_R([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.showdetails_R(dto);
        }
        [Route("get_all_relieving_data_R")]
        public ISM_Resignation_DTO get_all_relieving_data_R([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.get_all_relieving_data_R(dto);
        }
        [Route("showdetails_relieving_R")]
        public Task<ISM_Resignation_DTO> showdetails_relieving_R([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.showdetails_relieving_R(dto);
        }

        [Route("relieving_exit_employee_view")]
        public ISM_Resignation_DTO relieving_exit_employee_view([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.relieving_exit_employee_view(dto);
        }

        //GAUTAM
        [Route("loadEmployeeData")]
        public ISM_Resignation_DTO loadEmployeeData([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.loadEmployeeData(dto);
        }
        [Route("sendResignationMail")]
        public ISM_Resignation_DTO sendResignationMail([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.sendResignationMail(dto);
        }
        [Route("chcklistdocs")]
        public ISM_Resignation_DTO chcklistdocs([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.chcklistdocs(dto);
        }
        [Route("savechecklists")]
        public ISM_Resignation_DTO savechecklists([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.savechecklists(dto);
        }

        [Route("GetoverallReport")]
        public Task<ISM_Resignation_DTO> GetoverallReport([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.GetoverallReport(dto);
            //GAUTAM
        }
        [Route("approverRemark")]
        public Task<ISM_Resignation_DTO> approverRemark([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.approverRemark(dto);
            //GAUTAM
        }


        //Exit Overall Report ---- Aneeket

        [Route("getalldetails")]
        public ISM_Resignation_Master_Reasons_DTO getalldetails([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.getalldetails(dto);
        }


        [Route("getReport")]
        public ISM_Resignation_Master_Reasons_DTO getReport([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.getReport(dto);
        }


        //---------------------- ISM Resignation Checklist------------------------

        [Route("checklistloaddata")]
        public ISM_Resignation_ChecKLists_DTO checklistloaddata([FromBody] ISM_Resignation_ChecKLists_DTO data)
        {
            return _eei.checklistloaddata(data);
        }

        [Route("viewchecklistdata")]
        public ISM_Resignation_ChecKLists_DTO viewchecklistdata([FromBody] ISM_Resignation_ChecKLists_DTO data)
        {
            return _eei.viewchecklistdata(data);
        }

        [Route("checklistapprovedrecord")]
        public ISM_Resignation_ChecKLists_DTO checklistapprovedrecord([FromBody] ISM_Resignation_ChecKLists_DTO data)
        {
            return _eei.checklistapprovedrecord(data);
        }


        //--------------------------------




        [Route("HRIntemation")]
        public ISM_Resignation_Master_Reasons_DTO HRIntemation([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.HRIntemation(dto);
        }

        [Route("FANDFSettlement")]
        public ISM_Resignation_DTO.fullANDFinalSettlementDTO FANDFSettlement([FromBody] ISM_Resignation_DTO.fullANDFinalSettlementDTO dto)
        {
            return _eei.FANDFSettlement(dto);
        }

        [Route("DamageSave")]
        public ISM_Resignation_DTO.fullANDFinalSettlementDTO DamageSave([FromBody] ISM_Resignation_DTO.fullANDFinalSettlementDTO dto)
        {
            return _eei.DamageSave(dto);
        }



    }
}