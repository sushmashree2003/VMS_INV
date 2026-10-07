using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using DomainModel.Model;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using SendGrid;
using PreadmissionDTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.IO;
using System.Dynamic;
using System.Net.Mail;
//using Microsoft.AspNetCore.Identity;

namespace Recruitment.com.vaps.Services
{
    public class AddCandidateInterviewVMSService : Interfaces.AddCandidateInterviewVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
       private readonly UserManager<ApplicationUser> _UserManager;
        public AddCandidateInterviewVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext
            , UserManager<ApplicationUser> UserManager
            )
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
            _UserManager = UserManager;
        }

        public HR_CandidateInterviewScheduleDTO getBasicData(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public async Task<HR_CandidateInterviewScheduleDTO> SaveUpdate(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {

                if (dto.HRCISC_InterviewRounds == "Online Test")
                {
                    var cdetails = _VMSContext.HR_Candidate_DetailsDMO.Where(w => w.HRCD_Id == dto.HRCD_Id).ToList();

                    var checkquestion = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                                         from b in _VMSContext.OT_Master_OE_QuestionsDMO
                                         where a.OTQPTYP_Id == b.OTQPTYP_Id && a.MI_Id == cdetails.FirstOrDefault().MI_Id && a.MI_Id == b.MI_Id && a.HRMP_Id == b.HRMP_Id && a.HRMP_Id == cdetails.FirstOrDefault().HRMP_Id && a.OTQPTYP_ActiveFlg == true && b.OTMOEQ_ActiveFlg == true
                                         select a).ToList();

                    if (checkquestion.Count>0)
                    {

                        HR_CandidateInterviewScheduleDMO dmoObj = Mapper.Map<HR_CandidateInterviewScheduleDMO>(dto);

                        var duplicatecountresult = _VMSContext.HR_CandidateInterviewScheduleDMO.Where(t => t.HRCD_Id == dto.HRCD_Id && t.HRCISC_InterviewRounds == dto.HRCISC_InterviewRounds && t.HRCISC_InterviewDateTime == dto.HRCISC_InterviewDateTime && t.HRCISC_InterviewVenue == dto.HRCISC_InterviewVenue && t.HRCISC_Interviewer == dto.HRCISC_Interviewer).Count();
                        if (duplicatecountresult == 0)
                        {
                            if (dmoObj.HRCISC_Id > 0)
                            {
                                var result = _VMSContext.HR_CandidateInterviewScheduleDMO.Single(t => t.HRCISC_Id == dmoObj.HRCISC_Id);
                                dto.UpdatedDate = DateTime.Now;
                                dto.HRCISC_ActiveFlg = true;
                                Mapper.Map(dto, result);
                                _VMSContext.Update(result);
                                var flag = _VMSContext.SaveChanges();
                                if (flag > 0)
                                {
                                    dto.retrunMsg = "Update";
                                }
                                else
                                {
                                    dto.retrunMsg = "false";
                                }
                            }
                            else
                            {
                                if (dto.HRCISC_InterviewRounds == "Online Test")
                                {
                                    dmoObj.HRCISC_OnlineTestUserName = dto.HRCISC_OnlineTestUserName;
                                    dmoObj.HRCISC_OnlineTestPassword = dto.HRCISC_OnlineTestPassword;
                                }

                                dmoObj.HRCISC_ActiveFlg = true;
                                dmoObj.HRCISC_CreatedBy = dto.HRCISC_UpdatedBy;
                                dmoObj.UpdatedDate = DateTime.Now;
                                dmoObj.CreatedDate = DateTime.Now;
                                _VMSContext.Add(dmoObj);
                                var flag = _VMSContext.SaveChanges();
                                //var flag = 1;
                                if (flag == 1)
                                {


                                    if (dto.HRCISC_InterviewRounds == "Online Test" && dmoObj.HRCISC_NotifyEmail == true)
                                    {
                                        var schedulelist = _VMSContext.HR_CandidateInterviewScheduleDMO.Where(a => a.HRCISC_Id == dmoObj.HRCISC_Id).ToList();

                                        var candidatemail = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).ToList();


                                        if (candidatemail.FirstOrDefault().HRCD_EmailId != "" && candidatemail.FirstOrDefault().HRCD_EmailId != null)
                                        {
                                            SendEmailOnlineTest("ONLINTERVIEW", dto.MI_Id, candidatemail.FirstOrDefault().HRCD_EmailId, candidatemail.FirstOrDefault().HRCD_UpdatedBy, schedulelist.FirstOrDefault().HRCISC_Interviewer, schedulelist.FirstOrDefault().HRCISC_Id);
                                        }


                                    }
                                    else

                                    if (dmoObj.HRCISC_NotifyEmail == true)
                                    {
                                        string candidatemail = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).Select(t => t.HRCD_EmailId).FirstOrDefault();
                                        if (candidatemail != null && candidatemail != "")
                                        {
                                            SendEmail(dto, "Interview Call Letter", "Interview", dto.MI_Id);
                                        }
                                    }
                                    dto.retrunMsg = "Add";
                                }
                                else
                                {
                                    dto.retrunMsg = "false";
                                }
                            }
                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                            return dto;
                        }
                        dto = GetAllDropdownAndDatatableDetails(dto);
                    }
                    else
                    {
                        dto.retrunMsg = "NQ";
                    }
                }
                else
                {
                    HR_CandidateInterviewScheduleDMO dmoObj = Mapper.Map<HR_CandidateInterviewScheduleDMO>(dto);

                    var duplicatecountresult = _VMSContext.HR_CandidateInterviewScheduleDMO.Where(t => t.HRCD_Id == dto.HRCD_Id && t.HRCISC_InterviewRounds == dto.HRCISC_InterviewRounds && t.HRCISC_InterviewDateTime == dto.HRCISC_InterviewDateTime && t.HRCISC_InterviewVenue == dto.HRCISC_InterviewVenue && t.HRCISC_Interviewer == dto.HRCISC_Interviewer).Count();
                    if (duplicatecountresult == 0)
                    {
                        if (dmoObj.HRCISC_Id > 0)
                        {
                            var result = _VMSContext.HR_CandidateInterviewScheduleDMO.Single(t => t.HRCISC_Id == dmoObj.HRCISC_Id);
                            dto.UpdatedDate = DateTime.Now;
                            dto.HRCISC_ActiveFlg = true;
                            Mapper.Map(dto, result);
                            _VMSContext.Update(result);
                            var flag = _VMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            if (dto.HRCISC_InterviewRounds == "Online Test")
                            {
                                dmoObj.HRCISC_OnlineTestUserName = dto.HRCISC_OnlineTestUserName;
                                dmoObj.HRCISC_OnlineTestPassword = dto.HRCISC_OnlineTestPassword;
                            }

                            dmoObj.HRCISC_ActiveFlg = true;
                            dmoObj.HRCISC_CreatedBy = dto.HRCISC_UpdatedBy;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            //var flag = 1;
                            if (flag == 1)
                            {

                                //var candidatedetails = _VMSContext.HR_Candidate_DetailsDMO.Where(w => w.HRCD_Id == dto.HRCD_Id).ToList();
                                //dto.Name = candidatedetails.FirstOrDefault().HRCD_FirstName;
                                //dto.Email_id = candidatedetails.FirstOrDefault().HRCD_EmailId;
                                //dto.username = candidatedetails.FirstOrDefault().HRCD_EmailId;
                                //dto.Mobileno = candidatedetails.FirstOrDefault().HRCD_MobileNo.ToString();
                                //dto.password = "Password@123";
                                //dto.profile_pic_path = candidatedetails.FirstOrDefault().HRCD_Photo;



                                if (dto.HRCISC_InterviewRounds == "Online Test" && dmoObj.HRCISC_NotifyEmail == true)
                                {
                                    var schedulelist = _VMSContext.HR_CandidateInterviewScheduleDMO.Where(a => a.HRCISC_Id == dmoObj.HRCISC_Id).ToList();

                                    var candidatemail = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).ToList();


                                    if (candidatemail.FirstOrDefault().HRCD_EmailId != "" && candidatemail.FirstOrDefault().HRCD_EmailId != null)
                                    {
                                        SendEmailOnlineTest("ONLINTERVIEW", dto.MI_Id, candidatemail.FirstOrDefault().HRCD_EmailId, candidatemail.FirstOrDefault().HRCD_UpdatedBy, schedulelist.FirstOrDefault().HRCISC_Interviewer, schedulelist.FirstOrDefault().HRCISC_Id);
                                    }


                                }
                                else

                                if (dmoObj.HRCISC_NotifyEmail == true)
                                {
                                    string candidatemail = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).Select(t => t.HRCD_EmailId).FirstOrDefault();
                                    if (candidatemail != null && candidatemail != "")
                                    {
                                        SendEmail(dto, "Interview Call Letter", "Interview", dto.MI_Id);
                                    }
                                }
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                    dto = GetAllDropdownAndDatatableDetails(dto);
                }


               
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO editData(int id)
        {
            HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.retrunMsg = "";
            try
            {
                dto.VMSEditValue = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                    from b in _VMSContext.HR_Candidate_DetailsDMO
                                    where (a.HRCD_Id == b.HRCD_Id)
                                    select new HR_CandidateInterviewScheduleDTO
                                    {
                                        HRCISC_Id = a.HRCISC_Id,
                                        HRCD_Id = a.HRCD_Id,
                                        HRCD_FullName = b.HRCD_FirstName + " " + b.HRCD_MiddleName + " " + b.HRCD_LastName,
                                        HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                        HRCISC_InterviewDateTime = a.HRCISC_InterviewDateTime,
                                        HRCISC_InterviewVenue = a.HRCISC_InterviewVenue
                                    }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO deactivate(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRCISC_Id > 0)
                {
                    var result = _VMSContext.HR_CandidateInterviewScheduleDMO.Single(t => t.HRCISC_Id == dto.HRCISC_Id);

                    if (result.HRCISC_ActiveFlg == true)
                    {
                        result.HRCISC_ActiveFlg = false;
                    }
                    else if (result.HRCISC_ActiveFlg == false)
                    {
                        result.HRCISC_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRCISC_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO GetAllDropdownAndDatatableDetails(HR_CandidateInterviewScheduleDTO dto)
        {
            List<HR_CandidateInterviewScheduleDMO> datalist = new List<HR_CandidateInterviewScheduleDMO>();
            try
            {
                var interviewlist = _VMSContext.HR_CandidateInterviewScheduleDMO.Where(p => (p.HRCISC_Status == "Upcomming" || p.HRCISC_Status == "InProgress") && p.HRCISC_ActiveFlg == true).Select(p => p.HRCD_Id).Distinct().ToList();

                dto.CandidateDetailsList = (from emp in _VMSContext.HR_Candidate_DetailsDMO
                                            where emp.HRCD_ActiveFlg == true && emp.MI_Id == dto.MI_Id && !interviewlist.Contains(emp.HRCD_Id)
                                            select new HR_CandidateInterviewScheduleDTO
                                            {
                                                HRCD_Id = emp.HRCD_Id,
                                                HRCD_FirstName = emp.HRCD_FirstName,
                                                HRCD_MiddleName = (emp.HRCD_MiddleName == null? "":emp.HRCD_MiddleName),
                                                HRCD_LastName = (emp.HRCD_LastName == null ? "":emp.HRCD_LastName)
                                                //HRCD_FullName = emp.HRCD_FirstName.ToString() + " " + emp.HRCD_MiddleName.ToString() + " " + emp.HRCD_LastName.ToString(),
                                            }).Distinct().ToArray();

                dto.InterviewerList = (from emp in _VMSContext.IVRM_Staff_User_Login
                                       from empl in _VMSContext.HR_Master_Employee_DMO
                                       from empi in _VMSContext.Institution
                                       where (emp.Emp_Code == empl.HRME_Id && empl.HRME_ActiveFlag == true && empl.HRME_LeftFlag == false && empl.MI_Id == empi.MI_Id && empi.MI_ActiveFlag == 1 && emp.IVRMSTAUL_ActiveFlag == 1)
                                       select new HR_CandidateInterviewScheduleDTO
                                       {
                                           Id = emp.Id,
                                           HRME_Id = empl.HRME_Id,
                                           HRME_EmployeeFirstName = empl.HRME_EmployeeFirstName,
                                           HRME_EmployeeMiddleName = (empl.HRME_EmployeeMiddleName == null ? "":empl.HRME_EmployeeMiddleName),
                                           HRME_EmployeeLastName = (empl.HRME_EmployeeLastName == null ? "":empl.HRME_EmployeeLastName)
                                       }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO getrpt(HR_CandidateInterviewScheduleDTO dto)
        {
            if (dto.rdotype == "UpComming")
            {
                dto.upcomingintvw = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                     from b in _VMSContext.HR_Candidate_DetailsDMO
                                     from c in _VMSContext.IVRM_Staff_User_Login
                                     from d in _VMSContext.HR_Master_Employee_DMO
                                     where (a.HRCD_Id == b.HRCD_Id && b.HRCD_ActiveFlg == true && a.HRCISC_ActiveFlg == true && a.HRCISC_Interviewer == c.Id && c.Emp_Code == d.HRME_Id && a.HRCISC_Status == "Upcomming" && a.HRCISC_InterviewDateTime >= dto.fromdate && a.HRCISC_InterviewDateTime <= dto.todate)
                                     select new HR_CandidateInterviewScheduleDTO
                                     {
                                         HRCISC_Id = a.HRCISC_Id,
                                         HRCD_Id = a.HRCD_Id,
                                         HRCD_FullName = b.HRCD_FirstName + " " + b.HRCD_MiddleName + " " + b.HRCD_LastName,
                                         HRCD_FirstName = b.HRCD_FirstName,
                                         HRCD_MiddleName = b.HRCD_MiddleName,
                                         HRCD_LastName = b.HRCD_LastName,
                                         HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                         HRCISC_InterviewDateTime = a.HRCISC_InterviewDateTime,
                                         HRCISC_InterviewVenue = a.HRCISC_InterviewVenue,
                                         HRCISC_Interviewer = a.HRCISC_Interviewer,
                                         HRME_EmployeeFirstName = d.HRME_EmployeeFirstName,
                                         HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                         HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                         HRCISC_NotifyEmail = a.HRCISC_NotifyEmail,
                                         HRCISC_NotifySMS = a.HRCISC_NotifySMS
                                     }).OrderBy(a => a.HRCISC_Id).ToArray();
            }
            else if (dto.rdotype == "InProgress")
            {
                dto.inprogressintvw = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                       from b in _VMSContext.HR_Candidate_DetailsDMO
                                       from c in _VMSContext.IVRM_Staff_User_Login
                                       from d in _VMSContext.HR_Master_Employee_DMO
                                       where (a.HRCD_Id == b.HRCD_Id && b.HRCD_ActiveFlg == true && a.HRCISC_ActiveFlg == true && a.HRCISC_Interviewer == c.Id && c.Emp_Code == d.HRME_Id && a.HRCISC_Status == "InProgress" && a.HRCISC_InterviewDateTime >= dto.fromdate && a.HRCISC_InterviewDateTime <= dto.todate)
                                       select new HR_CandidateInterviewScheduleDTO
                                       {
                                           HRCISC_Id = a.HRCISC_Id,
                                           HRCD_Id = a.HRCD_Id,
                                           HRCD_FullName = b.HRCD_FirstName + " " + b.HRCD_MiddleName + " " + b.HRCD_LastName,
                                           HRCD_FirstName = b.HRCD_FirstName,
                                           HRCD_MiddleName = b.HRCD_MiddleName,
                                           HRCD_LastName = b.HRCD_LastName,
                                           HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                           HRCISC_InterviewDateTime = a.HRCISC_InterviewDateTime,
                                           HRCISC_InterviewVenue = a.HRCISC_InterviewVenue,
                                           HRCISC_Interviewer = a.HRCISC_Interviewer,
                                           HRME_EmployeeFirstName = d.HRME_EmployeeFirstName,
                                           HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                           HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                           HRCISC_NotifyEmail = a.HRCISC_NotifyEmail,
                                           HRCISC_NotifySMS = a.HRCISC_NotifySMS
                                       }).OrderBy(a => a.HRCISC_Id).ToArray();
            }
            else if (dto.rdotype == "Completed")
            {
                dto.completedintvw = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                      from b in _VMSContext.HR_Candidate_DetailsDMO
                                      from c in _VMSContext.IVRM_Staff_User_Login
                                      from d in _VMSContext.HR_Master_Employee_DMO
                                      from e in _VMSContext.HR_Candidate_InterviewStatusDMO
                                      where (a.HRCD_Id == b.HRCD_Id && b.HRCD_ActiveFlg == true && a.HRCISC_ActiveFlg == true && a.HRCISC_Interviewer == c.Id && c.Emp_Code == d.HRME_Id && a.HRCISC_Status == "Complete" && b.HRCD_Id == e.HRCD_Id && a.HRCISC_Interviewer == e.IVRMUL_Id && a.HRCISC_InterviewDateTime.Date >= dto.fromdate.Date && a.HRCISC_InterviewDateTime.Date <= dto.todate.Date)
                                      select new HR_CandidateInterviewScheduleDTO
                                      {
                                          HRCISC_Id = a.HRCISC_Id,
                                          HRCD_Id = a.HRCD_Id,
                                          HRCD_FullName = b.HRCD_FirstName + " " + b.HRCD_MiddleName + " " + b.HRCD_LastName,
                                          HRCD_FirstName = b.HRCD_FirstName,
                                          HRCD_MiddleName = b.HRCD_MiddleName,
                                          HRCD_LastName = b.HRCD_LastName,
                                          HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                          HRCISC_InterviewDateTime = a.HRCISC_InterviewDateTime,
                                          HRCISC_InterviewVenue = a.HRCISC_InterviewVenue,
                                          HRCISC_Interviewer = a.HRCISC_Interviewer,
                                          HRME_EmployeeFirstName = d.HRME_EmployeeFirstName,
                                          HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                          HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                          HRCISC_NotifyEmail = a.HRCISC_NotifyEmail,
                                          HRCISC_NotifySMS = a.HRCISC_NotifySMS,
                                          HRCIS_InterviewFeedBack = e.HRCIS_InterviewFeedBack,
                                          HRCIS_CandidateStatus = e.HRCIS_CandidateStatus
                                      }).OrderBy(a => a.HRCISC_Id).ToArray();
            }
            return dto;
        }

        public HR_CandidateInterviewScheduleDTO savefeedback(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            dto.HRCIS_Datetime = DateTime.Now;
            try
            {
                HR_Candidate_InterviewStatusDMO dmoObj = Mapper.Map<HR_Candidate_InterviewStatusDMO>(dto);
                HR_CandidateInterviewScheduleDMO OBJDMO = Mapper.Map<HR_CandidateInterviewScheduleDMO>(dto);

                var duplicatecountresult = _VMSContext.HR_Candidate_InterviewStatusDMO.Where(t => t.HRCD_Id == dto.HRCD_Id && t.IVRMUL_Id == dto.IVRMUL_Id && t.HRCIS_Datetime == dto.HRCIS_Datetime).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRCIS_Id > 0)
                    {
                        var result = _VMSContext.HR_Candidate_InterviewStatusDMO.Single(t => t.HRCIS_Id == dmoObj.HRCIS_Id);
                        dto.UpdatedDate = DateTime.Now;

                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRCIS_ActiveFlg = true;
                        dmoObj.HRCD_Id = dto.HRCD_Id;
                        dmoObj.IVRMUL_Id = dto.HRCISC_UpdatedBy;
                        dmoObj.HRCIS_InterviewFeedBack = dto.HRCIS_InterviewFeedBack;
                        dmoObj.HRCIS_Datetime = dto.HRCIS_Datetime;
                        dmoObj.HRCIS_Status = dto.HRCISC_Status;
                        dmoObj.HRCISC_CreatedBy = dto.HRCISC_UpdatedBy;
                        dmoObj.HRCISC_UpdatedBy = dto.HRCISC_UpdatedBy;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                            SendEmailFeedback(dto, "Interview Feedback", "Interview Feedback", dto.MI_Id);
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                        if(dto.HRCD_Id > 0)
                        {
                            var contactExistsP = _VMSContext.Database.ExecuteSqlCommand("HR_Sp_RiskFactor @p0,@p1,@p2,@P3",
                              dto.HRCD_Id, dto.Special,dto.HRCISC_UpdatedBy,"Save");
                        }


                    }

                    if (OBJDMO.HRCISC_Id > 0)
                    {
                        var result = _VMSContext.HR_CandidateInterviewScheduleDMO.Single(t => t.HRCISC_Id == OBJDMO.HRCISC_Id);
                        dto.UpdatedDate = DateTime.Now;
                        dto.HRCISC_ActiveFlg = true;
                        dto.HRCISC_Status = dto.HRCISC_Status;
                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public void SendEmail(HR_CandidateInterviewScheduleDTO obj, string subject, string body, long id)
        {
            try
            {
                var candidatedetails = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == obj.HRCD_Id).FirstOrDefault();
                string mailid = candidatedetails.HRCD_EmailId;
                //mailid = "goutamkumar@vapstech.com";
                string mailcc = "", mailcc1 = "", mailcc2 = "";

                mailcc1 = (from a in _VMSContext.IVRM_Staff_User_Login
                                 from c in _VMSContext.Multiple_Email_DMO
                                 where (a.Id == obj.HRCISC_UpdatedBy && a.Emp_Code == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                                 select c.HRMEM_EmailId).FirstOrDefault();

                mailcc2 = (from a in _VMSContext.IVRM_Staff_User_Login
                           from c in _VMSContext.Multiple_Email_DMO
                           where (a.Id == obj.HRCISC_Interviewer && a.Emp_Code == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                           select c.HRMEM_EmailId).FirstOrDefault();

                mailcc = mailcc1 + "," + mailcc2;
                //if(mailcc != "") { mailcc = mailcc + ",hr@vapstech.com"; } else { mailcc = "hr@vapstech.com"; }

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Interview_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    //string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                            if(mailcc1 != null) { message.AddBcc(mailcc1); }
                            if (mailcc2 != null) { message.AddBcc(mailcc2); }
                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", candidatedetails.HRCD_FirstName + " " + (candidatedetails.HRCD_MiddleName == null ? "" : candidatedetails.HRCD_MiddleName) + " " + (candidatedetails.HRCD_LastName == null ? "" : candidatedetails.HRCD_LastName));
                        message.HtmlContent = message.HtmlContent.Replace("[VENUE]", obj.HRCISC_InterviewVenue);
                        message.HtmlContent = message.HtmlContent.Replace("[DATE]", obj.HRCISC_InterviewDateTime.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Interview_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",SqlDbType.VarChar)
                        {
                            Value = "Staff"
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public string SendEmailOnlineTest(string Template,long MI_Id, string Email,long HRCISC_UpdatedBy,long HRCISC_Interviewer,long HRCISC_Id)
        {
            try
            {
                //Email = "praveend114@gmail.com";

                string mailcc = "", mailcc1 = "", mailcc2 = "";

                mailcc1 = (from a in _VMSContext.IVRM_Staff_User_Login
                           from c in _VMSContext.Multiple_Email_DMO
                           where (a.Id == HRCISC_UpdatedBy && a.Emp_Code == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                           select c.HRMEM_EmailId).FirstOrDefault();

                mailcc2 = (from a in _VMSContext.IVRM_Staff_User_Login
                           from c in _VMSContext.Multiple_Email_DMO
                           where (a.Id == HRCISC_Interviewer && a.Emp_Code == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                           select c.HRMEM_EmailId).FirstOrDefault();

                mailcc = mailcc1 + "," + mailcc2;

               // mailcc = "";
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, MI_Id.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER_ONLINETEST";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = HRCISC_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }


                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;



                    Mailcontent = Resultsms;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        //string mailcc = "";
                        //string mailbcc = "";
                        //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        //{
                        //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        //    mailcc = ccmail[0].ToString();

                        //    if (ccmail.Length > 1)
                        //    {
                        //        if (ccmail[1] != null || ccmail[1] != "")
                        //        {
                        //            mailbcc = ccmail[1].ToString();
                        //        }
                        //    }

                        //}
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);

                        if (Attechement.Equals("1"))
                        {
                            var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        //if (mailbcc != null && mailbcc != "")
                        //{
                        //    message.AddBcc(mailbcc);
                        //}

                        message.HtmlContent = Mailmsg;
                        // var client = new Web("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");

                        //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                        //{
                        //    message.AddAttachment(template.FirstOrDefault().ISES_MailHTMLTemplate);

                        //}
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {


                        //string mailcc = "";
                        //string mailbcc = "";
                        //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        //{
                        //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        //    mailcc = ccmail[0].ToString();

                        //    if (ccmail.Length > 1)
                        //    {
                        //        if (ccmail[1] != null || ccmail[1] != "")
                        //        {
                        //            mailbcc = ccmail[1].ToString();
                        //        }
                        //    }

                        //}
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }


                                            //var attachment = new MimePart("image", "gif")
                                            //{
                                            //    ContentObject = new ContentObject(File.OpenRead(img[i].IVRM_Att_Path), ContentEncoding.Default),
                                            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                            //    ContentTransferEncoding = ContentEncoding.Base64,
                                            //    FileName = Path.GetFileName(img[i].IVRM_Att_Path)
                                            //};
                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                //if (mailbcc != null && mailbcc != "")
                                //{
                                //    emailMessage.Bcc.Add(mailbcc);
                                //}
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }
                return "S";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public void SendEmailFeedback(HR_CandidateInterviewScheduleDTO obj, string subject, string body, long id)
        {
            try
            {
                var candidatedetails = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == obj.HRCD_Id).FirstOrDefault();

                var interviewername = (from a in _VMSContext.IVRM_Staff_User_Login
                           from c in _VMSContext.HR_Master_Employee_DMO
                           where (a.Id == obj.HRCISC_UpdatedBy && a.Emp_Code == c.HRME_Id)
                           select new { c.HRME_EmployeeFirstName,c.HRME_EmployeeMiddleName,c.HRME_EmployeeLastName }).FirstOrDefault();

                string mailid = "hr@vapstech.com";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("InterviewFeedback_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = "hr@vapsknowledge.com";
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", candidatedetails.HRCD_FirstName + " " + (candidatedetails.HRCD_MiddleName == null ? "" : candidatedetails.HRCD_MiddleName) + " " + (candidatedetails.HRCD_LastName == null ? "" : candidatedetails.HRCD_LastName));
                        message.HtmlContent = message.HtmlContent.Replace("[ROUND]", obj.HRCISC_InterviewRounds);
                        message.HtmlContent = message.HtmlContent.Replace("[STATUS]", obj.HRCISC_Status);
                        message.HtmlContent = message.HtmlContent.Replace("[FEEDBACK]", obj.HRCIS_InterviewFeedBack);
                        message.HtmlContent = message.HtmlContent.Replace("[INTERVIEWER]", interviewername.HRME_EmployeeFirstName + " " + interviewername.HRME_EmployeeMiddleName + " " + interviewername.HRME_EmployeeLastName);
                        message.HtmlContent = message.HtmlContent.Replace("[CSTATUS]", obj.HRCIS_CandidateStatus);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("InterviewFeedback_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        //public async Task<HR_CandidateInterviewScheduleDTO> CreateCndUsername(HR_CandidateInterviewScheduleDTO reg) //
        //{
        //    // string returnMsg = "";
        //    try
        //    {

        //        ApplicationUser user = new ApplicationUser();
        //        ApplicationUser user1 = new ApplicationUser();
        //        ApplicationUser user3 = new ApplicationUser();
        //        if (reg.Special != "Special")
        //        {
        //            user = await _UserManager.FindByEmailAsync(reg.Email_id);
        //        }
        //        reg.username = "axcuyyy";
        //        user1 = await _UserManager.FindByNameAsync(reg.username.Trim());

        //        if (user == null || !(await _UserManager.IsEmailConfirmedAsync(user)))
        //        {
        //            if (user1 == null)
        //            {
                       
        //                user = new ApplicationUser { UserName = reg.username.Trim(), Email = reg.Email_id };
        //                // Added on 11-11-2016 to store Institute, entry date and ip address
        //                //user.MI_Id = reg.MI_Id;
                       
        //                user.Machine_Ip_Address = reg.Machine_Ip_Address;
        //                // Added on 11-11-2016 to store Institute, entry date and ip address
        //                user.UserImagePath = reg.profile_pic_path;
        //                //15/11/2016
        //                user.PhoneNumber = reg.Mobileno;

        //                user.RoleTypeFlag = "Online Exam Candidate";



        //                user.EmailConfirmed = true;
        //                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
        //                user.CreatedDate = indianTime;
        //                user.UpdatedDate = indianTime;
        //                user.Entry_Date = indianTime;

        //                user.Name = reg.Name;
        //                // user.UserImagePath = "D:\\IVRM 2.0 23.12\\corewebapi18072016\\src\\corewebapi18072016\\wwwroot\\images\\uploads\\Profile_Pics\\a_1c6e77cd - 1ba4 - 45eb - 959a - 19b80a5c0dfc.jpg";



        //                //  var fetchval = _UserManager.CreateAsync(user, reg.password).Result.Errors.ToArray();
        //                var result = await _UserManager.CreateAsync(user, reg.password);

        //                //  string abc = fetchval[0].Code;
        //                if (result.Succeeded)
        //                {
        //                    reg.returnMsg = "created";
        //                    //Date:15-12-2016 Send Confirmation email.
        //                    // reg.User_Id = user.Id;

        //                    //string confirmationToken = _UserManager.
        //                    //                       GenerateEmailConfirmationTokenAsync(user).Result;
        //                    //reg.token = confirmationToken;

        //                    //try
        //                    //{
        //                    //    //SendEmail(user.UserName, user.Email, reg.MI_Id, "RegistrationEmailTemplate");
        //                    //    string s = await sendSms("Af7660f900c9df85ac5dfae4b055d75bd", "HHSJCP", Convert.ToInt64(reg.Mobileno), reg.MI_Id, user.UserName, "RegistrationEmailTemplate");
        //                    //}
        //                    //catch (Exception e)
        //                    //{
        //                    //    Console.WriteLine(e.Message);
        //                    //}

        //                    // Student Roles
        //                    string studentRole = "";

        //                    studentRole = "Online Exam Candidate";

        //                    var id = _Context.applicationRole.Single(d => d.Name == studentRole);
        //                    //

        //                    // Student Role Type
        //                    string studentRoleType = "";
        //                    studentRoleType = "Online Exam Candidate";



        //                    var id2 = _Context.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
        //                    //

        //                    // Save role
        //                    var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
        //                    _Context.appUserRole.Add(role);
        //                    _Context.SaveChanges();

        //                    UserRoleWithInstituteDTO master = new UserRoleWithInstituteDTO();
        //                    master.Id = user.Id;
        //                    master.MI_Id = reg.MI_Id;
        //                    master.Activeflag = 1;



        //                    UserRoleWithInstituteDMO mas = Mapper.Map<UserRoleWithInstituteDMO>(master);
        //                    _Context.Add(mas);
        //                    _Context.SaveChanges();

        //                    //if (reg.Special == "Special")
        //                    //{
        //                    //    Preadmission_Special_Registration special = new Preadmission_Special_Registration();
        //                    //    special.ID = user.Id;
        //                    //    _Context.Add(special);
        //                    //    _Context.SaveChanges();
        //                    //}





        //                }
        //                //else if (fetchval.FirstOrDefault().Code.Equals("DuplicateUserName"))
        //                //{
        //                //    reg.returnMsg = fetchval.FirstOrDefault().Description.ToString();
        //                //}

        //                else
        //                {
        //                    reg.returnMsg = result.Errors.FirstOrDefault().Description.ToString();
        //                    reg.returnMsg = "Passwords must have at least 6 characters,one Special,one lowercase and one uppercase character.";
        //                }
        //            }
        //            else
        //            {
        //                reg.returnMsg = "Username Already exist .. !!";
        //            }
        //        }
        //        else
        //        {
        //            reg.returnMsg = "Email id Already exist .. !!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //    }
        //    return reg;

        //}



    }
}
