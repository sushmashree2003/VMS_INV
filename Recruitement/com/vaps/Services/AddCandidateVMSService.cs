using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class AddCandidateVMSService : Interfaces.AddCandidateVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public AddCandidateVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public HR_Candidate_DetailsDTO getBasicData(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                var institutionlist = (from a in _VMSContext.Institution                                       
                                       where  a.MI_ActiveFlag == 1
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                dto.institutionlist = institutionlist.ToArray();
                if (dto.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        dto.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HR_Candidate_DetailsDTO SaveUpdate(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Candidate_DetailsDMO dmoObj = Mapper.Map<HR_Candidate_DetailsDMO>(dto);
                HR_MRF_ListDMO mrfListObj = new HR_MRF_ListDMO();

                var duplicatecountresult = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMPT_Id == dto.HRMPT_Id && t.HRMC_Id == dto.HRMC_Id && t.HRCD_MRFNO == dto.HRCD_MRFNO && t.HRCD_FirstName == dto.HRCD_FirstName && t.HRCD_MiddleName == dto.HRCD_MiddleName && t.HRCD_LastName == dto.HRCD_LastName && t.HRMJ_Id == dto.HRMJ_Id && t.HRCD_Skills == dto.HRCD_Skills && t.HRCD_DOB == dto.HRCD_DOB && t.IVRMMG_Id == dto.IVRMMG_Id && t.HRCD_MobileNo == dto.HRCD_MobileNo && t.HRCD_EmailId == dto.HRCD_EmailId && t.HRCD_AadharNo == dto.HRCD_AadharNo).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRCD_Id > 0)
                    {
                        var result = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dmoObj.HRCD_Id);
                        dto.UpdatedDate = DateTime.Now;
                        dto.HRCD_ActiveFlg = true;
                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        dto.HRCD_Id = result.HRCD_Id;
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                            if(dto.welcomenotice == true)
                            {
                                Welcomemail(dto, "Welcome Notification", "Welcome Notification", dto.MI_Id);
                            }
                            else if (dto.thanksnotice == true)
                            {
                                Thanksgivingmail(dto, "Thanks Giving Notification", "Thanks Giving Notification", dto.MI_Id);
                            }

                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRCD_ActiveFlg = true;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRMPT_Id = dto.HRMPT_Id;
                        dmoObj.HRMC_Id = dto.HRMC_Id;
                        dmoObj.HRCD_MRFNO = dto.HRCD_MRFNO;
                        dmoObj.HRCD_FirstName = dto.HRCD_FirstName;
                        dmoObj.HRCD_MiddleName = dto.HRCD_MiddleName;
                        dmoObj.HRCD_LastName = dto.HRCD_LastName;
                        dmoObj.HRMJ_Id = dto.HRMJ_Id;
                        dmoObj.HRCD_Skills = dto.HRCD_Skills;
                        dmoObj.HRCD_DOB = dto.HRCD_DOB;
                        dmoObj.IVRMMG_Id = dto.IVRMMG_Id;
                        dmoObj.HRCD_MobileNo = dto.HRCD_MobileNo;
                        dmoObj.HRCD_EmailId = dto.HRCD_EmailId;
                        dmoObj.HRCD_ExpFrom = dto.HRCD_ExpFrom;
                        dmoObj.HRCD_ExpTo = dto.HRCD_ExpTo;
                        dmoObj.HRCD_CurrentCompany = dto.HRCD_CurrentCompany;
                        dmoObj.HRCD_ResumeSource = dto.HRCD_ResumeSource;
                        dmoObj.HRCD_JobPortalName = dto.HRCD_JobPortalName;
                        dmoObj.HRCD_RefCode = dto.HRCD_RefCode;
                        dmoObj.HRCD_LastCTC = dto.HRCD_LastCTC;
                        dmoObj.HRCD_ExpectedCTC = dto.HRCD_ExpectedCTC;
                        dmoObj.HRCD_AppDate = dto.HRCD_AppDate;
                        dmoObj.HRCD_InterviewDate = dto.HRCD_InterviewDate;
                        dmoObj.HRCD_NoticePeriod = dto.HRCD_NoticePeriod;
                        dmoObj.HRCD_Remarks = dto.HRCD_Remarks;
                        dmoObj.HRCD_Resume = dto.HRCD_Resume;
                        dmoObj.HRCD_RecruitmentStatus = dto.HRCD_RecruitmentStatus;
                        dmoObj.HRCD_CreatedBy = dto.HRCD_CreatedBy;
                        dmoObj.HRCD_UpdatedBy = dto.HRCD_UpdatedBy;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.HRCD_Photo = dto.HRCD_Photo;
                        dmoObj.HRCD_AadharNo = dto.HRCD_AadharNo;
                        dmoObj.HRCD_PAN = dto.HRCD_PAN;
                        dmoObj.HRCD_AddressLocal = dto.HRCD_AddressLocal;
                        dmoObj.HRCD_AddressLocal2 = dto.HRCD_AddressLocal2;
                        dmoObj.HRCD_AddLocalPlace = dto.HRCD_AddLocalPlace;
                        dmoObj.HRCD_AddLocalPIN = dto.HRCD_AddLocalPIN;
                        dmoObj.HRCD_AddressPermanent = dto.HRCD_AddressPermanent;
                        dmoObj.HRCD_AddressPermanent2 = dto.HRCD_AddressPermanent2;
                        dmoObj.HRCD_AddPermanentPlace = dto.HRCD_AddPermanentPlace;
                        dmoObj.HRCD_AddPermanentPIN = dto.HRCD_AddPermanentPIN;
                        dmoObj.HRMP_Id = dto.HRMP_Id;
                      //  dmoObj.HRMCON_Id = dto.HRMCON_Id;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        dto.HRCD_Id = dmoObj.HRCD_Id;
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                            SendNotification(dto);
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
                //dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HR_Candidate_DetailsDTO editData(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.retrunMsg = "";
            try
            {
                dto.VMSCandidateList = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                        from b in _VMSContext.HR_Master_PostionTypeDMO
                                        from c in _VMSContext.HR_Master_Course
                                        from d  in _VMSContext.IVRM_Master_Gender
                                  where (a.HRCD_Id.Equals(id) && a.HRMPT_Id == b.HRMPT_Id && a.HRMC_Id == c.HRMC_Id && a.IVRMMG_Id == d.IVRMMG_Id)
                                  select new HR_Candidate_DetailsDTO
                                  {
                                      HRCD_Id = a.HRCD_Id,
                                      HRMPT_Id = a.HRMPT_Id,
                                      HRMPT_Name = b.HRMPT_Name,
                                      HRMC_Id = a.HRMC_Id,
                                      HRMC_QulaificationName = c.HRMC_QulaificationName,
                                      HRCD_MRFNO = a.HRCD_MRFNO,
                                      HRCD_FirstName = a.HRCD_FirstName,
                                      HRCD_MiddleName = a.HRCD_MiddleName,
                                      HRCD_LastName = a.HRCD_LastName,
                                      HRMJ_Id = a.HRMJ_Id,
                                      HRCD_Skills = a.HRCD_Skills,
                                      HRCD_DOB = a.HRCD_DOB,
                                      IVRMMG_Id = a.IVRMMG_Id,
                                      IVRMMG_GenderName = d.IVRMMG_GenderName,
                                      HRCD_MobileNo = a.HRCD_MobileNo,
                                      HRCD_EmailId = a.HRCD_EmailId,
                                      HRCD_ExpFrom = a.HRCD_ExpFrom,
                                      HRCD_ExpTo = a.HRCD_ExpTo,
                                      HRCD_CurrentCompany = a.HRCD_CurrentCompany,
                                      HRCD_ResumeSource = a.HRCD_ResumeSource,
                                      HRCD_JobPortalName = a.HRCD_JobPortalName,
                                      HRCD_RefCode = a.HRCD_RefCode,
                                      HRCD_LastCTC = a.HRCD_LastCTC,
                                      HRCD_ExpectedCTC = a.HRCD_ExpectedCTC,
                                      HRCD_AppDate = a.HRCD_AppDate,
                                      HRCD_InterviewDate = a.HRCD_InterviewDate,
                                      HRCD_NoticePeriod = a.HRCD_NoticePeriod,
                                      HRCD_Remarks = a.HRCD_Remarks,
                                      HRCD_Resume = a.HRCD_Resume,
                                      HRCD_RecruitmentStatus = a.HRCD_RecruitmentStatus,
                                      CreatedDate = a.CreatedDate,
                                      HRCD_CreatedBy = a.HRCD_CreatedBy,
                                      HRMP_Id = a.HRMP_Id,
                                   //   HRMCON_Id = a.HRMCON_Id
                                  }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Candidate_DetailsDTO deactivate(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRCD_Id > 0)
                {
                    var result = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dto.HRCD_Id);

                    if (result.HRCD_ActiveFlg == true)
                    {
                        result.HRCD_ActiveFlg = false;
                    }
                    else if (result.HRCD_ActiveFlg == false)
                    {
                        result.HRCD_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRCD_ActiveFlg == true)
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
        public HR_Candidate_DetailsDTO GetAllDropdownAndDatatableDetails(HR_Candidate_DetailsDTO dto)
        {
            List<HR_Candidate_DetailsDMO> datalist = new List<HR_Candidate_DetailsDMO>();
            List<HR_Candidate_DetailsDTO> DTOdatalistEarning = new List<HR_Candidate_DetailsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();
            try
            {
                var usertypelist = _VMSContext.ApplicationUserDMO.Where(t => t.Id == dto.UserId && t.RoleTypeFlag == "New Candidate").Count();
                if (usertypelist>0)
                {
                    dto.candidateflage = true;
                }
               

                dto.MasterPosTypeList = (from emp in _VMSContext.HR_Master_PostionTypeDMO
                                         where emp.HRMPT_ActiveFlg == true && emp.MI_Id == dto.MI_Id
                                         select new HR_Candidate_DetailsDTO
                                         {
                                             HRMPT_Id = emp.HRMPT_Id,
                                             HRMPT_Name = emp.HRMPT_Name
                                         }).ToArray();

                dto.MasterPositionList = (from emp in _VMSContext.HR_Master_PositionDMO
                                          where emp.HRMP_ActiveFlg == true && emp.MI_Id == dto.MI_Id
                                          select new HR_Candidate_DetailsDTO
                                         {
                                             HRMP_Id = emp.HRMP_Id,
                                             HRMP_Position = emp.HRMP_Position
                                         }).ToArray();

                dto.MasterQualification = (from emp in _VMSContext.HR_Master_Course
                                           where emp.HRMC_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                           select new HR_Candidate_DetailsDTO
                                           {
                                               HRMC_Id = emp.HRMC_Id,
                                               HRMC_QulaificationName = emp.HRMC_QulaificationName
                                           }).ToArray();

                dto.MasterGender = (from emp in _VMSContext.IVRM_Master_Gender
                                    where emp.IVRMMG_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                    select new HR_Candidate_DetailsDTO
                                    {
                                        IVRMMG_Id = emp.IVRMMG_Id,
                                        IVRMMG_GenderName = emp.IVRMMG_GenderName
                                    }).ToArray();

                dto.masterjob = (from a in _VMSContext.HR_Master_JobsDMO
                                 where a.HRMJ_ActiveFlg == true
                                 select new HR_Candidate_DetailsDTO
                                 {
                                     HRMJ_Id = a.HRMJ_Id,
                                     HRMJ_JobCode = a.HRMJ_JobCode,
                                     HRMJ_JobTiTle = a.HRMJ_JobTiTle
                                 }).ToArray();

                dto.mastercountry = _VMSContext.IVRM_Master_Country.ToArray();
                dto.masterReligion = _VMSContext.MasterReligionDMO.Where(t=>t.Is_Active == true).ToArray();
                dto.masterCaste = _VMSContext.mastercasteDMO.Where(t=>t.MI_Id == dto.MI_Id).ToArray();
                dto.mastermaritalstatus = _VMSContext.IVRM_Master_Marital_Status.Where(t=>t.MI_Id == dto.MI_Id && t.IVRMMMS_ActiveFlag == true).ToArray();
                dto.cousultencydetail = _VMSContext.Mastert_ConsultancyDMO.Where(t => t.HRMCON_ActiveFlg == true).ToArray();

                dto.departmenlist = (from a in _Context.HR_Master_Department
                                     where a.MI_Id == dto.MI_Id && a.HRMD_ActiveFlag == true
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRMD_Id =a.HRMD_Id,
                                         HRMD_DepartmentName = a.HRMD_DepartmentName,
                                     }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();

                dto.candidatelist = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                     where (a.MI_Id == dto.MI_Id && a.HRCD_ActiveFlg == true)
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRCD_Id=a.HRCD_Id,
                                         HRCD_FirstName = ((a.HRCD_FirstName == null ? " " : a.HRCD_FirstName) + " " + (a.HRCD_MiddleName == null ? " " : a.HRCD_MiddleName) + " " + (a.HRCD_LastName == null ? " " : a.HRCD_LastName)).Trim(),
                                     }).Distinct().OrderBy(t => t.HRCD_Id).ToArray();

                dto.earingdeductionlist = (from a in _VMSContext.HR_Master_EarningsDeductions
                                           where (a.MI_Id == dto.MI_Id && a.HRMED_ActiveFlag == true)
                                           select new HR_Candidate_DetailsDTO
                                           {
                                               HRMED_Name=a.HRMED_Name,
                                               HRMED_AmountPercentFlag = a.HRMED_AmountPercentFlag,
                                           }).Distinct().ToArray();


                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Candidatealldetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt) { Value = dto.UserId });                 
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.candidatedetailsgrid = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

               

                List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
                //Earning list
                earningdatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning") && t.HRMED_ActiveFlag == true).ToList();

                if (earningdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                    {
                        HR_Candidate_DetailsDTO phdto = Mapper.Map<HR_Candidate_DetailsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistEarning.Add(phdto);

                    }

                }

                dto.earningList = DTOdatalistEarning.ToArray();

                List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();
                //Deduction List
                deductiondatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Deduction") && t.HRMED_ActiveFlag == true).ToList();


                if (deductiondatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in deductiondatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistDeduction.Add(phdto);

                    }

                }

                dto.detectionList = DTOdatalistDeduction.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO data)
        {
            try
            {
                data.desgnationlist = (from a in _Context.HR_Master_Employee_DMO
                                       from b in _Context.HR_Master_Department
                                       from c in _Context.HR_Master_Designation
                                       where (a.HRMD_Id == data.HRMD_Id && a.MI_Id==data.MI_Id && a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                       select new HR_Candidate_DetailsDTO
                                       {
                                           HRMDES_Id = c.HRMDES_Id,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                                       }).Distinct().OrderBy(t => t.HRMDES_Order).ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Candidate_DetailsDTO saveAppointmentdata(HR_Candidate_DetailsDTO data)
        {
            try
            {

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Candidate_DetailsDTO addQualification(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int icount = 0; icount < dto.QualificationsDTO.Length; icount++)
                {
                    dto.HRCD_Id = dto.Employeedto.HRCD_Id;
                    HR_Candidate_QualificationsDMO dmoObj = Mapper.Map<HR_Candidate_QualificationsDMO>(dto.QualificationsDTO[icount]);
                    var duplicatecountresult = _VMSContext.HR_Candidate_QualificationsDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.Employeedto.HRCD_Id && t.HRCQUAL_Course == dmoObj.HRCQUAL_Course && t.HRCQUAL_Board == dmoObj.HRCQUAL_Board && t.HRCQUAL_PassingYear == dmoObj.HRCQUAL_PassingYear).Count();
                    //if (duplicatecountresult == 0)
                    //{
                        if (dmoObj.HRCQUAL_Id > 0)
                        {
                            var result = _VMSContext.HR_Candidate_QualificationsDMO.Where(t => t.HRCQUAL_Id == dmoObj.HRCQUAL_Id).FirstOrDefault();
                            result.HRCQUAL_UpdatedBy = dto.UserId;
                            result.UpdatedDate = DateTime.Now;
                            result.HRCQUAL_ActiveFlag = true;
                            Mapper.Map(dmoObj, result);
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
                            dmoObj.HRCD_Id = dto.HRCD_Id;
                            dmoObj.MI_Id = dto.Employeedto.MI_Id;
                            dmoObj.HRCQUAL_ActiveFlag = true;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRCQUAL_CreatedBy = dto.UserId;
                            dmoObj.HRCQUAL_UpdatedBy = dto.UserId;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    //}
                    //else
                    //{
                    //    dto.retrunMsg = "Duplicate";
                    //    return dto;
                    //}
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HR_Candidate_DetailsDTO addexperience(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for(int icount=0;icount< dto.ExperienceDTO.Length; icount++)
                {
                    dto.HRCD_Id = dto.Employeedto.HRCD_Id;
                    HR_Candidate_ExperienceDMO dmoObj = Mapper.Map<HR_Candidate_ExperienceDMO>(dto.ExperienceDTO[icount]);
                    var duplicatecountresult = _VMSContext.HR_Candidate_ExperienceDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.HRCD_Id && t.HRCEXP_CompanyName == dmoObj.HRCEXP_CompanyName && t.HRCEXP_Designation == dmoObj.HRCEXP_Designation && t.HRCEXP_From == dmoObj.HRCEXP_From && t.HRCEXP_To == dmoObj.HRCEXP_To && t.HRCEXP_Salary == dmoObj.HRCEXP_Salary).Count();
                    //if (duplicatecountresult == 0)
                    //{
                        if (dmoObj.HRCEXP_Id > 0)
                        {
                            var result = _VMSContext.HR_Candidate_ExperienceDMO.Where(t => t.HRCEXP_Id == dmoObj.HRCEXP_Id).FirstOrDefault();
                            result.HRCEXP_UpdatedBy = dto.UserId;
                            result.UpdatedDate = DateTime.Now;
                            result.HRCEXP_ActiveFlag = true;
                            Mapper.Map(dmoObj, result);
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
                            dmoObj.HRCD_Id = dto.HRCD_Id;
                            dmoObj.MI_Id = dto.Employeedto.MI_Id;
                            dmoObj.HRCEXP_ActiveFlag = true;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRCEXP_CreatedBy = dto.UserId;
                            dmoObj.HRCEXP_UpdatedBy = dto.UserId;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    //}
                    //else
                    //{
                    //    dto.retrunMsg = "Duplicate";
                    //    return dto;
                    //}
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HR_Candidate_DetailsDTO addfamily(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int icount = 0; icount < dto.FamilyDTO.Length; icount++)
                {
                    dto.HRCD_Id = dto.Employeedto.HRCD_Id;
                    HR_Candidate_FamilyDMO dmoObj = Mapper.Map<HR_Candidate_FamilyDMO>(dto.FamilyDTO[icount]);
                    var duplicatecountresult = _VMSContext.HR_Candidate_FamilyDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.Employeedto.HRCD_Id && t.HRCFAM_Name == dmoObj.HRCFAM_Name && t.HRCFAM_Relationship == dmoObj.HRCFAM_Relationship && t.HRCFAM_Occupation == dmoObj.HRCFAM_Occupation && t.HRCFAM_CompanyName == dmoObj.HRCFAM_CompanyName && t.HRCFAM_Age == dmoObj.HRCFAM_Age).Count();
                    //if (duplicatecountresult == 0)
                    //{
                        if (dmoObj.HRCFAM_Id > 0)
                        {
                            var result = _VMSContext.HR_Candidate_FamilyDMO.Where(t => t.HRCFAM_Id == dmoObj.HRCFAM_Id).FirstOrDefault();
                            result.HRCFAM_UpdatedBy = dto.UserId;
                            result.UpdatedDate = DateTime.Now;
                            result.HRCFAM_ActiveFlag = true;
                            Mapper.Map(dmoObj, result);
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
                            dmoObj.HRCD_Id = dto.HRCD_Id;
                            dmoObj.MI_Id = dto.Employeedto.MI_Id;
                            dmoObj.HRCFAM_ActiveFlag = true;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRCFAM_CreatedBy = dto.UserId;
                            dmoObj.HRCFAM_UpdatedBy = dto.UserId;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    //}
                    //else
                    //{
                    //    dto.retrunMsg = "Duplicate";
                    //    return dto;
                    //}
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HR_Candidate_DetailsDTO addlanguage(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int icount = 0; icount < dto.LanguagesDTO.Length; icount++)
                {
                    dto.HRCD_Id = dto.Employeedto.HRCD_Id;
                    HR_Candidate_LanguagesDMO dmoObj = Mapper.Map<HR_Candidate_LanguagesDMO>(dto.LanguagesDTO[icount]);
                    var duplicatecountresult = _VMSContext.HR_Candidate_LanguagesDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.HRCD_Id && t.HRCLAN_ToRead == dmoObj.HRCLAN_ToRead && t.HRCLAN_ToWrite == dmoObj.HRCLAN_ToWrite && t.HRCLAN_ToSpeak == dmoObj.HRCLAN_ToSpeak).Count();
                    //if (duplicatecountresult == 0)
                    //{
                        if (dmoObj.HRCLAN_Id > 0)
                        {
                            var result = _VMSContext.HR_Candidate_LanguagesDMO.Where(t => t.HRCLAN_Id == dmoObj.HRCLAN_Id).FirstOrDefault();
                            result.HRCLAN_UpdatedBy = dto.UserId;
                            result.UpdatedDate = DateTime.Now;
                            result.HRCLAN_ActiveFlag = true;
                            Mapper.Map(dmoObj, result);
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
                            dmoObj.HRCD_Id = dto.HRCD_Id;
                            dmoObj.MI_Id = dto.Employeedto.MI_Id;
                            dmoObj.HRCLAN_ActiveFlag = true;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRCLAN_CreatedBy = dto.UserId;
                            dmoObj.HRCLAN_UpdatedBy = dto.UserId;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.UpdatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    //}
                    //else
                    //{
                    //    dto.retrunMsg = "Duplicate";
                    //    return dto;
                    //}
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public void SendNotification(HR_Candidate_DetailsDTO data)
        {
            try
            {
                string companyname = _VMSContext.Institution.Where(t => t.MI_Id == data.MI_Id).Select(t => t.MI_Name).FirstOrDefault();

                string str = "";
                str += "<table width='100%' border= '1'><tr align='center' width='100%'>Dear " + data.HRCD_FirstName + " Welcome to " + companyname + " </tr></table>";

                if (data.HRCD_EmailId != null && data.HRCD_EmailId != "")
                {
                    SendEmail(data.HRCD_EmailId, "Welcome Notification", str, 17);
                }
            }
            
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }  
        }







        public HR_Candidate_DetailsDTO AddCandidateDocuments(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = string.Empty;
            try
            {
                //dto.HRCD_Id = dto.Employeedto.HRCD_Id;
                for (int i = 0; i < dto.HR_Candidate_DocumentsDTO.Length; i++)
                {
                    var documentDto = dto.HR_Candidate_DocumentsDTO[i];
                    var dmoObj = new HR_Candidate_DocumentsDMO
                    
                    {
                        HRCD_Id = dto.HRCD_Id,
                        
                        HRCDD_DocumentPath = documentDto.ismclT_FilePath, 
                        HRCDD_CreatedDate = DateTime.Now,
                        HRCDD_UpdatedDate = DateTime.Now,
                        HRCDD_CreatedBy = dto.UserId,
                        HRCDD_UpdatedBy = dto.UserId,
                        HRCDD_DocumentName= documentDto.HRCDD_DocumentName
                    };

                    _VMSContext.Add(dmoObj);
                }

                var addResult = _VMSContext.SaveChanges();
                dto.retrunMsg = addResult > 0 ? "Add successful" : "Add failed";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.retrunMsg = "Error occurred: " + ex.Message;
            }

            return dto;
        }







        public HR_Candidate_DetailsDTO getalldetailsDocVerfy(HR_Candidate_DetailsDTO data)
        {
            //if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            //{
            //    var getuserinstitution = _VMSContext.HR_Candidate_DetailsDMO.Where(a => a.HRCD_Id == data.UserId).ToList();

            //}

            //using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
            //{
            //    cmd.CommandText = "HR_InterviewCandidate_Details";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add(new SqlParameter("@USER_ID", SqlDbType.VarChar)
            //    {
            //        Value = data.UserId
            //    });
     
            //    if (cmd.Connection.State != ConnectionState.Open)
            //        cmd.Connection.Open();
            //    var retObject = new List<dynamic>();
            //    try
            //    {
            //        using (var dataReader = cmd.ExecuteReader())
            //        {
            //            while (dataReader.Read())
            //            {
            //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                {
            //                    dataRow.Add(
            //                    dataReader.GetName(iFiled),
            //                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                    );
            //                }
            //                retObject.Add((ExpandoObject)dataRow);
            //            }
            //        }
            //        data.CandidateDetailsverfy = retObject.ToArray();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }

            //}


            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_InterviewCandidate_Details";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = data.UserId });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var candidateDetails = new List<dynamic>();
                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var i = 0; i < dataReader.FieldCount; i++)
                            {
                                dataRow.Add(dataReader.GetName(i), dataReader.IsDBNull(i) ? null : dataReader[i]);
                            }
                            candidateDetails.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.CandidateDetailsverfy = candidateDetails.Distinct().ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }




        public HR_Candidate_DetailsDTO saveVerify(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = string.Empty;

            try
            {
                var empcodes = _Context.IVRM_Staff_User_Login.Where(R => R.Id == dto.UserId).ToList();
                if (empcodes.Count > 0)
                {
                    for (int i = 0; i < dto.HR_Candidate_Documents_VerifiedByDTO.Length; i++)
                    {
                        var documentDto = dto.HR_Candidate_Documents_VerifiedByDTO[i];

                        var dmoObj = new HR_Candidate_Documents_VerifiedByDMO
                        {
                            HRCD_Id = documentDto.HRMEDS_Id,
                            HRME_Id = empcodes.FirstOrDefault().Emp_Code,
                            HRCDDV_Remarks = documentDto.HRCDDV_Remarks,
                            HRCDDV_RejectedFlg = documentDto.HRCDDV_RejectedFlg,
                            HRCDDV_DateTime = DateTime.Now,
                            HRCDDV_CreatedDate = DateTime.Now,
                            HRCDDV_UpdatedDate = DateTime.Now,
                            HRCDDV_CreatedBy = dto.UserId,
                            HRCDDV_UpdatedBy = dto.UserId
                        };

                        _VMSContext.Add(dmoObj);
                    }

                    var addResult = _VMSContext.SaveChanges();
                    if(addResult > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "notsave";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.retrunMsg = "admin";
                
            }

            return dto;
        }











        ////Added by Aman for Documnets verify reports
        public HR_Candidate_DetailsDTO VerifyReports(HR_Candidate_DetailsDTO data)
        {
            try
            {
                var empcodes = _Context.IVRM_Staff_User_Login.Where(R => R.Id == data.UserId).ToList();

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";
                fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");


                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Candidate_DC_Aprroval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Hrme_id", SqlDbType.BigInt)
                    {
                        Value = empcodes[0].Emp_Code

                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRCDDV_RejectedFlg", SqlDbType.VarChar)
                    {
                        Value = data.HRCDDV_RejectedFlg
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
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
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.CandidateDetailsverfy = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }









        public void SendEmail(string mailid, string subject, string body, long id)
        {
            //mailid = "goutamkumar@vapstech.com";
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_Template_Name.Equals("FO", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
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

                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
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
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

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
                // return ex.Message;
            }          
        }
        public HR_Candidate_DetailsDTO sendCallLettermail(HR_Candidate_DetailsDTO dto)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == "CallLetterTemplate" && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    dto.retrunMsg = "Email Template not Mapped to the selected Institution";
                    return dto;
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == dto.MI_Id).ToList();
                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == dto.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                //TEMPLATE EDITING
                Mailmsg = dto.Template;
                //TEMPLATE EDITING

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";
                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] ccmail = template[0].ISES_MailCCId.Split(',');
                        mailcc = ccmail[0].ToString();
                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }
                    }
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(dto.HRCD_EmailId);

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
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == "CallLetterTemplate" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",SqlDbType.NVarChar)
                        {
                            Value = dto.HRCD_EmailId
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
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
                            dto.retrunMsg = ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.retrunMsg = ex.Message;
            }

            dto.retrunMsg = "success";
            VMS_CallLetter_sentDMO obj = new VMS_CallLetter_sentDMO();
            obj.VMSCS_EmailId = dto.HRCD_EmailId;
            obj.VMSCS_SentDate = DateTime.Now;
            obj.VMSCS_CreatedBy = dto.UserId;
            obj.VMSCS_updatedBy = dto.UserId;
            _VMSContext.Add(obj);
            _VMSContext.SaveChanges();
            return dto;
        }
        public void Thanksgivingmail(HR_Candidate_DetailsDTO obj, string subject, string body, long id)
        {
            try
            {
                string jobtitle = _VMSContext.HR_Master_JobsDMO.Where(t => t.HRMJ_Id == obj.HRMJ_Id).Select(t => t.HRMJ_JobTiTle).FirstOrDefault();
                string companyname = _VMSContext.Institution.Where(t => t.MI_Id == obj.MI_Id).Select(t => t.MI_Name).FirstOrDefault();

                string mailid = obj.HRCD_EmailId;
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Thanks_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
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
                        message.HtmlContent = message.HtmlContent.Replace("[DATE]", DateTime.Today.ToString("dd/MM/yyyy"));
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", obj.HRCD_FirstName + " " + (obj.HRCD_MiddleName == null ? "" : obj.HRCD_MiddleName) + " " + (obj.HRCD_LastName == null ? "" : obj.HRCD_LastName));
                        message.HtmlContent = message.HtmlContent.Replace("[JOBPOST]", jobtitle);
                        message.HtmlContent = message.HtmlContent.Replace("[COMPANY]", companyname);
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
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Thanks_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

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
        public void Welcomemail(HR_Candidate_DetailsDTO obj, string subject, string body, long id)
        {
            try
            {
                string mailid = "hr@vapstech.com";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Selected_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
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
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", obj.HRCD_FirstName + " " + (obj.HRCD_MiddleName == null ? "" : obj.HRCD_MiddleName) + " " + (obj.HRCD_LastName == null ? "" : obj.HRCD_LastName));
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
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Selected_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

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
        public HR_Candidate_DetailsDTO saveAppointmenttab(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto.candidatetype = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                     from b in _VMSContext.HR_Master_PostionTypeDMO
                                     where (a.HRCD_Id == dto.Employeedto.HRCD_Id && a.HRMPT_Id == b.HRMPT_Id)
                                     select b.HRMPT_Name).FirstOrDefault();

                var candidatedetailsfirst = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dto.Employeedto.HRCD_Id);

                HR_Candidate_AppointmentDMO dmoObj = Mapper.Map<HR_Candidate_AppointmentDMO>(dto);
                var duplicatecountresult = _VMSContext.HR_Candidate_AppointmentDMO.Where(t => t.HRCD_Id == dto.Employeedto.HRCD_Id && t.HRCA_FirstDocName == dto.HRCA_FirstDocName && t.HRCA_FirstDocDate == dto.HRCA_FirstDocDate && t.HRCA_SecDocName == dto.HRCA_SecDocName && t.HRCA_SecDocDate == dto.HRCA_SecDocDate && t.HRCA_AnnualCTC == dto.HRCA_AnnualCTC && t.HRCA_MonthlyCTC == dto.HRCA_MonthlyCTC && t.HRCA_AppointmentRefNo == dto.HRCA_AppointmentRefNo && t.HRCA_AcknowledgementRefNo == dto.HRCA_AcknowledgementRefNo).Count();
                if (duplicatecountresult == 0)
                {
                    if (dto.HRCA_Id > 0)
                    {
                        var result = _VMSContext.HR_Candidate_AppointmentDMO.Where(t => t.HRCA_Id == dto.HRCA_Id).FirstOrDefault();
                        result.UpdatedDate = DateTime.Now;
                        result.HRCA_UpdatedBy = dto.UserId;
                        result.HRCA_FirstDocName = dto.HRCA_FirstDocName;
                        result.HRCA_FirstDocDate = dto.HRCA_FirstDocDate;
                        result.HRCA_SecDocName = dto.HRCA_SecDocName;
                        result.HRCA_SecDocDate = dto.HRCA_SecDocDate;
                        result.HRCA_AnnualCTC = dto.HRCA_AnnualCTC;
                        result.HRCA_MonthlyCTC = dto.HRCA_MonthlyCTC;
                        result.HRCA_AppointmentRefNo = dto.HRCA_AppointmentRefNo;
                        result.HRCA_AcknowledgementRefNo = dto.HRCA_AcknowledgementRefNo;
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                            if (dto.welcomenotice == true)
                            {
                                sendmail(dto.MI_Id, candidatedetailsfirst.HRCD_EmailId, candidatedetailsfirst.HRCD_FirstName, "CanidateWelcomeMail");
                            }
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRCD_Id = dto.Employeedto.HRCD_Id;
                        dmoObj.HRCA_FirstDocName = dto.HRCA_FirstDocName;
                        dmoObj.HRCA_FirstDocDate = dto.HRCA_FirstDocDate;
                        dmoObj.HRCA_SecDocName = dto.HRCA_SecDocName;
                        dmoObj.HRCA_SecDocDate = dto.HRCA_SecDocDate;
                        dmoObj.HRCA_AnnualCTC = dto.HRCA_AnnualCTC;
                        dmoObj.HRCA_MonthlyCTC = dto.HRCA_MonthlyCTC;
                        dmoObj.HRCA_AppointmentRefNo = dto.HRCA_AppointmentRefNo;
                        dmoObj.HRCA_AcknowledgementRefNo = dto.HRCA_AcknowledgementRefNo;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.HRCA_CreatedBy = dto.UserId;
                        dmoObj.HRCA_UpdatedBy = dto.UserId;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                            if (dto.welcomenotice==true)
                            {
                                 sendmail(dto.MI_Id, candidatedetailsfirst.HRCD_EmailId, candidatedetailsfirst.HRCD_FirstName, "CanidateWelcomeMail");
                            }



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
                    if (dto.welcomenotice == true)
                    {
                        sendmail(dto.MI_Id, candidatedetailsfirst.HRCD_EmailId, candidatedetailsfirst.HRCD_FirstName, "CanidateWelcomeMail");
                    }
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
        public HR_Candidate_DetailsDTO EditRecord(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto.qualificationlist = _VMSContext.HR_Candidate_QualificationsDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).ToArray();
                dto.experiencelist = _VMSContext.HR_Candidate_ExperienceDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).ToArray();
                dto.languagelist = _VMSContext.HR_Candidate_LanguagesDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).ToArray();
                dto.familylist = _VMSContext.HR_Candidate_FamilyDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).ToArray();

                dto.HRCD_Id = dto.HRCD_Id;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HR_Candidate_DetailsDTO activedeactive(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if(dto.userMessage== "Concert")
                {
                    var candidatedetails = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dto.HRCD_Id);
                    candidatedetails.HRCD_UpdatedBy = dto.UserId;
                    candidatedetails.HRCD_ConsentFlag = "A";
                    _VMSContext.Update(candidatedetails);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";
                    }
                    else
                    {
                        dto.retrunMsg = "Not Update";
                    }

                }
                else
                {
                    var candidatedetails = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dto.HRCD_Id);

                    if (candidatedetails.HRCD_ActiveFlg == true)
                    {
                        candidatedetails.HRCD_ActiveFlg = false;
                        _VMSContext.Update(candidatedetails);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                        else
                        {
                            dto.retrunMsg = "Not Deactivated";
                        }
                    }
                    else
                    {
                        candidatedetails.HRCD_ActiveFlg = true;
                        _VMSContext.Update(candidatedetails);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Not Activated";
                        }
                    }
                }
               
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


        public string sendmail(long MI_Id, string Email,string candidatename, string Template )
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _VMSContext.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _VMSContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _VMSContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _VMSContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                var date = DateTime.Now;                

               
                Mailmsg = Mailmsg.Replace("[LOGO]", institutionName[0].MI_Logo);               
                Mailmsg = Mailmsg.Replace("[INSTUITENAME]", institutionName[0].MI_Name);               
                Mailmsg = Mailmsg.Replace("[NAME]", candidatename);               
                Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _VMSContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";

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
                    //if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    //{
                    //    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    //}

                    var message = new SendGridMessage();
                    //if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    //{
                    //    string[] mail_id = template[0].ISES_MailCCId.Split(',');
                    //    if (mail_id.Length > 0)
                    //    {
                    //        for (int i = 0; i < mail_id.Length; i++)
                    //        {
                    //            message.AddTo(mail_id[i]);

                    //        }
                    //    }
                    //}
                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] mail_id = template[0].ISES_MailCCId.Split(',');
                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddCc(mail_id[i]);

                            }
                        }
                    }

                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                    {
                        string[] mail_id = template[0].ISES_MailBCCId.Split(',');
                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddCc(mail_id[i]);

                            }
                        }
                    }
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _VMSContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                    //if (mailcc != null && mailcc != "")
                    //{
                    //    message.AddCc(mailcc);
                    //}
                    //if (mailbcc != null && mailbcc != "")
                    //{
                    //    message.AddBcc(mailbcc);
                    //}
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _VMSContext.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _VMSContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _VMSContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Value = Mailmsg
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}