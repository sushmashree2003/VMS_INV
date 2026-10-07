using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Produces("application/json")]
    [Route("api/Exit_Employee")]
    public class Exit_EmployeeController : Controller
    {
        Exit_Employee_Delegate EED = new Exit_Employee_Delegate();

        public DomainModelMsSqlServerContext _context;

        public Exit_EmployeeController(DomainModelMsSqlServerContext _cont)
        {
            _context = _cont;
        }



        [Route("get_depts")]
        public ISM_Resignation_DTO get_depts([FromBody]ISM_Resignation_DTO dto)
        {
           // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));      
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return EED.get_depts(dto);
        }

        [Route("get_desig")]
        public ISM_Resignation_DTO get_desig([FromBody]ISM_Resignation_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return EED.get_desig(dto);
        }



        //===========================Reason master Start====================================
        [Route("Get_Reason/{id:int}")]
        public ISM_Resignation_Master_Reasons_DTO Get_Reason(int id)
        {
            ISM_Resignation_Master_Reasons_DTO dto = new ISM_Resignation_Master_Reasons_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Get_Reason(dto);
        }
        [Route("Save_Edit_Reason")]
        public ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Save_Edit_Reason(dto);
        }

        [Route("get_details_reason/{id:int}")]
        public ISM_Resignation_Master_Reasons_DTO get_details_reason(int id)
        {
            ISM_Resignation_Master_Reasons_DTO dto = new ISM_Resignation_Master_Reasons_DTO();
            dto.ISMRESGMRE_Id = id;
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.get_details_reason(dto);
        }

        [Route("active_deactive_reason")]
        public ISM_Resignation_Master_Reasons_DTO active_deactive_reason([FromBody]ISM_Resignation_Master_Reasons_DTO dto)
        {
            return EED.active_deactive_reason(dto);
        }

        //==========================================End=============================================

        //===========================Check list master Start====================================
        [Route("Get_Checklist/{id:int}")]
        public ISM_Resignation_Master_CheckLists_DTO Get_Checklist(int id)
        {
            ISM_Resignation_Master_CheckLists_DTO dto = new ISM_Resignation_Master_CheckLists_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.RoleTypeFlag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));

            return EED.Get_Checklist(dto);
        }
        [Route("company")]
        public ISM_Resignation_Master_CheckLists_DTO company([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return EED.company(dto);
        }
        [Route("get_company")]
        public ISM_Resignation_Master_CheckLists_DTO get_company([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return EED.get_company(dto);
        }
        [Route("get_ApproverDepartDesign_checklist")]
        public ISM_Resignation_Master_CheckLists_DTO get_ApproverDepartDesign_checklist([FromBody]ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return EED.get_ApproverDepartDesign_checklist(dto);
        }


        [Route("Save_Edit_Checklist")]
        public ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          //  dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Save_Edit_Checklist(dto);
        }

        [Route("get_details_checklist/{id:int}")]
        public ISM_Resignation_Master_CheckLists_DTO get_details_checklist(int id)
        {
            ISM_Resignation_Master_CheckLists_DTO dto = new ISM_Resignation_Master_CheckLists_DTO();
            dto.ISMRESGMCL_Id = id;
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.get_details_checklist(dto);
        }

        [Route("active_deactive_checklist")]
        public ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist([FromBody]ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return EED.active_deactive_checklist(dto);
        }

        //==========================================End=============================================
        //==================================Exit Employee Process Start=================================

        [Route("Load_all_data/{id:int}")]
        public ISM_Resignation_DTO Load_all_data(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Load_all_data(dto);
        }

        [Route("GetAllRelData/{id:int}")]
        public ISM_Resignation_DTO GetAllRelData(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMRESG_Id = id;
            return EED.GetAllRelData(dto);
        }
        [Route("GetAllRelData1/{id:int}")]
        public ISM_Resignation_DTO GetAllRelData1(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRME_Id = id;
            return EED.GetAllRelData1(dto);
        }


        [Route("Exit_empl_SaveEdit")]
        public ISM_Resignation_DTO Exit_empl_SaveEdit([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Exit_empl_SaveEdit(dto);
        }
        [Route("ExitApproveal")]
        public ISM_Resignation_DTO ExitApproveal([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.ExitApproveal(dto);
        }
        [Route("Exit_empl_AccRej")]
        public ISM_Resignation_DTO Exit_empl_AccRej([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Exit_empl_AccRej(dto);
        }
        [Route("HR_RelivingApply")]
        public ISM_Resignation_DTO HR_RelivingApply([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.HR_RelivingApply(dto);
        }
        [Route("HR_EmployeeDetails")]
        public ISM_Resignation_DTO HR_EmployeeDetails([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.HR_EmployeeDetails(dto);
        }
        [Route("Edit_Employee/{id:int}")]
        public ISM_Resignation_DTO Edit_Employee(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMRESG_Id = id;
            return EED.Edit_Employee(dto);
        }
        [Route("c_approve_new/{id:int}")]
        public ISM_Resignation_DTO c_approve_new(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMRESG_Id = id;
            return EED.c_approve_new(dto);
        }
        [Route("Savedata_td")]
        public ISM_Resignation_DTO Savedata_td([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Savedata_td(dto);
        }

        [Route("edit_relieving/{id:int}")]
        public ISM_Resignation_DTO edit_relieving(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.HRME_Id = id;
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.edit_relieving(dto);
        }
        [Route("Savedata_print")]
        public ISM_Resignation_DTO.LetterGenerationDTO Savedata_print([FromBody] ISM_Resignation_DTO.LetterGenerationDTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Savedata_print(dto);
        }
        [Route("print_exit_employee/{id:int}")]
        public ISM_Resignation_DTO print_exit_employee(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRME_Id = id;

            return EED.print_exit_employee(dto);
        }
        [Route("download_doc/{id:int}")]
        public ISM_Resignation_DTO download_doc(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMRESG_Id = id;

            return EED.download_doc(dto);
        }
        //================================Reports===================================
        [Route("get_all_data_R")]
        public ISM_Resignation_DTO get_all_data_R(ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.get_all_data_R(dto);
        }
        [Route("get_all_relieving_data_R")]
        public ISM_Resignation_DTO get_all_relieving_data_R(ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.get_all_relieving_data_R(dto);
        }

        [Route("showdetails_R")]
        public ISM_Resignation_DTO showdetails_R([FromBody]ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.showdetails_R(dto);
        }

        [Route("showdetails_relieving_R")]
        public ISM_Resignation_DTO showdetails_relieving_R([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.showdetails_relieving_R(dto);
        }

        [Route("relieving_exit_employee_view/{id:int}")]
        public ISM_Resignation_DTO relieving_exit_employee_view(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMRESG_Id = id;

            return EED.relieving_exit_employee_view(dto);
        }

        //GAUTAM
        [Route("loadEmployeeData/{id:int}")]
        public ISM_Resignation_DTO loadEmployeeData(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.loadEmployeeData(dto);
        }

        [Route("sendResignationMail")]
        public ISM_Resignation_DTO sendResignationMail([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.sendResignationMail(dto);
        }

        [Route("chcklistdocs")]
        public ISM_Resignation_DTO chcklistdocs([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.chcklistdocs(dto);
        }
        [Route("savechecklists")]
        public ISM_Resignation_DTO savechecklists([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.savechecklists(dto);
        }
        //GAUTAM
        [Route("GetoverallReport")]
        public ISM_Resignation_DTO GetoverallReport([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.GetoverallReport(dto);
        }
        [Route("approverRemark")]
        public ISM_Resignation_DTO approverRemark([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.approverRemark(dto);
        }

        //Exit Overall Report ----- Aneeket

        [Route("getalldetails/{id:int}")]

        public ISM_Resignation_Master_Reasons_DTO getalldetails(int id)
        {
            ISM_Resignation_Master_Reasons_DTO dto = new ISM_Resignation_Master_Reasons_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.getalldetails(dto);
        }


        [Route("getReport")]
        public ISM_Resignation_Master_Reasons_DTO getReport([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.getReport(dto);
        }



        //-------------ISM_Certificate_Request----------------


        [Route("getloaddata/{id:int}")]
        public ISM_Certificate_RequestDTO getloaddata(int id)
        {
            ISM_Certificate_RequestDTO data = new ISM_Certificate_RequestDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.getloaddata(data);
        }

        [Route("savedetails")]
        public ISM_Certificate_RequestDTO savedetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.savedetails(data);
        }

        [Route("deactive")]
        public ISM_Certificate_RequestDTO deactive([FromBody] ISM_Certificate_RequestDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.deactive(data);
        }

        [Route("Edit")]
        public ISM_Certificate_RequestDTO Edit([FromBody] ISM_Certificate_RequestDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Edit(data);
        }
        [Route("Viewdata")]
        public ISM_Certificate_RequestDTO Viewdata([FromBody] ISM_Certificate_RequestDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Viewdata(data);
        }



        //-----------------------certificate approval--------------
        [Route("loaddata/{id:int}")]
        public ISM_Certificate_RequestDTO loaddata(int id)
        {
            ISM_Certificate_RequestDTO data = new ISM_Certificate_RequestDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.loaddata(data);
        }



        [Route("viewuploadflies")]
        public ISM_Certificate_RequestDTO viewuploadflies([FromBody] ISM_Certificate_RequestDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.viewuploadflies(d);
        }
        [Route("approvedrecord")]
        public ISM_Certificate_RequestDTO approvedrecord([FromBody] ISM_Certificate_RequestDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return EED.approvedrecord(data);
        }

        //---------------------- ISM Resignation Checklist------------------------
        [Route("checklistloaddata/{id:int}")]
        public ISM_Resignation_ChecKLists_DTO checklistloaddata(int id)
        {
            ISM_Resignation_ChecKLists_DTO data = new ISM_Resignation_ChecKLists_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.checklistloaddata(data);
        }

        [Route("viewchecklistdata")]
        public ISM_Resignation_ChecKLists_DTO viewchecklistdata([FromBody] ISM_Resignation_ChecKLists_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.viewchecklistdata(data);
        }

        [Route("checklistapprovedrecord")]
        public ISM_Resignation_ChecKLists_DTO checklistapprovedrecord([FromBody] ISM_Resignation_ChecKLists_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.checklistapprovedrecord(data);
        }


        //--------------------
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public ISM_Certificate_RequestDTO Onload(int n)
        {
            ISM_Certificate_RequestDTO data = new ISM_Certificate_RequestDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Onload(data);
        }

        [HttpPost]
        [Route("getapprovalreport")]
        public ISM_Certificate_RequestDTO getapprovalreport([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.getapprovalreport(data);
        }

        [HttpPost]
        [Route("DocView")]
        public ISM_Certificate_RequestDTO DocView([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.DocView(data);
        }

        [HttpPost]
        [Route("getIEDetails")]
        public ISM_Certificate_RequestDTO getIEDetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.getIEDetails(data);
        }
        [HttpPost]
        [Route("getcertificate")]
        public ISM_Certificate_RequestDTO getcertificate([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.getcertificate(data);
        }
        [Route("getadminloaddata/{id:int}")]
        public ISM_Certificate_RequestDTO getadminloaddata(int id)
        {
            ISM_Certificate_RequestDTO data = new ISM_Certificate_RequestDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.getadminloaddata(data);
        }
        [HttpPost]
        [Route("saveadmindetails")]
        public ISM_Certificate_RequestDTO saveadmindetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.saveadmindetails(data);
        }

        [HttpPost]
        [Route("FANDFSettlement")]
        public ISM_Resignation_DTO.fullANDFinalSettlementDTO FANDFSettlement([FromBody]ISM_Resignation_DTO.fullANDFinalSettlementDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.FANDFSettlement(data);
        }
        [HttpPost]
        [Route("DamageSave")]
        public ISM_Resignation_DTO.fullANDFinalSettlementDTO DamageSave([FromBody]ISM_Resignation_DTO.fullANDFinalSettlementDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.DamageSave(data);
        }




        //OTP Verification For Relieving Letter.
        [Route("getotp")]
        public ISM_Resignation_DTO.relievingletter getotp([FromBody]ISM_Resignation_DTO.relievingletter data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string genotp = "";
            HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
            CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
            genotp = generate.getOTP();
            if (genotp.Length < 6)
            {
                genotp = generate.getOTP();
            }
            else
            {
                HttpContext.Session.SetString("EMAILOTP", genotp);
                CommonLibrary.Email Email = new CommonLibrary.Email(_context);


                string s = Email.sendmail_checkbox(data.MI_Id, data.Emailid, "EMAILOTP", Convert.ToInt64(genotp));

                data.message = "Please Check,OTP Is Sent To Your Email Id";
            }
            return data;
        }

        [Route("getnewotpverification")]
        public ISM_Resignation_DTO.relievingletter getnewotpverification([FromBody]ISM_Resignation_DTO.relievingletter data)
        {
            var EMAILOTP = HttpContext.Session.GetString("EMAILOTP");
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFE"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 30)
            {
                data.message = "OTP Expired";
            }
            else
            {

                if (data.genotp == EMAILOTP)
                {
                    data.message = "Success";

                }
                else
                {
                    data.message = "Wrong Enter OTP !";
                }
            }
            //  HttpContext.Session.SetString("EmailOTPStatus", data.message);


            return data;
        }





    }
}