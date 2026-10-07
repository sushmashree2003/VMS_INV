using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
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
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeRegistrationService : Interfaces.EmployeeRegistrationInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeRegistrationService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public MasterEmployeeDTO getBasicData(MasterEmployeeDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public MasterEmployeeDTO SaveUpdate(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.TabName.Equals("FirstTab"))
                {
                    AddUpdateEmployeePersonalDetails(dto);
                }
                else if (dto.TabName.Equals("AddressTab"))
                {
                    AddUpdateEmployeeAddressDetails(dto);
                }
                else if (dto.TabName.Equals("QualificationTab"))
                {
                    AddUpdateEmployeeQualificationDetails(dto);
                }
                else if (dto.TabName.Equals("ExperienceTab"))
                {
                    AddUpdateEmployeeExperienceDetails(dto);
                }
                else if (dto.TabName.Equals("DocumentTab"))
                {
                    AddUpdateEmployeeDocumentDetails(dto);
                }
                else if (dto.TabName.Equals("OtherTab"))
                {
                    AddUpdateEmployeeOtherDetails(dto);
                }
                else if (dto.TabName.Equals("SalaryTab"))
                {
                    //AddUpdateEmployeeEarningDetails(dto);
                    //AddUpdateEmployeeDeductionDetails(dto);
                    //AddUpdateEmployeeArrearDetails(dto);
                    dto.retrunMsg = "Update";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }

            return dto;
        }

        //Personal Details
        public MasterEmployeeDTO AddUpdateEmployeePersonalDetails(MasterEmployeeDTO dto)
        {

            dto.retrunMsg = "";
            try
            {
                MasterEmployee dmoObj = Mapper.Map<MasterEmployee>(dto.Employeedto);

                if (dmoObj.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);

                    dto.Employeedto.UpdatedDate = DateTime.Now;
                    dto.Employeedto.HRME_UpdatedBy = dto.LogInUserId;
                    Mapper.Map(dto.Employeedto, result);

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        // dto.HRME_Id = dmoObj.HRME_Id;
                        dto.retrunMsg = "Update";
                        for (int i = 0; i < dto.email_list_dto.Length; i++)
                        {
                            var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == dto.email_list_dto[i].HRMEM_EmailId && d.HRMEM_DeFaultFlag == "default").Count();
                            if (tcnoduplicate > 0)
                            {
                            }
                            else
                            {
                                student_mobile_no(dto);
                                student_email_id(dto);
                            }
                        }

                        for (int t = 0; t < dto.mobile_list_dto.Length; t++)
                        {
                            var tcnoduplicate = _HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == dto.mobile_list_dto[t].HRMEMNO_MobileNo && d.HRMEMNO_DeFaultFlag == "default").Count();
                            if (tcnoduplicate > 0)
                            {
                            }
                            else
                            {
                                student_mobile_no(dto);
                            }
                        }



                        AddUpdateEmployeeBankDetails(dto);
                        Feeconcession(dto);

                        var currentEmp = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                        Mapper.Map(currentEmp, dto.Employeedto);
                        string emailid = "";
                        var list = (from a in _HRMSContext.HR_Master_DepartmentCodeDMO
                                    from b in _HRMSContext.HR_Master_DepartmentCode_Head
                                    from c in _HRMSContext.Emp_Email_Id
                                    where (a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.HRME_Id && a.HRMDC_Code == "HR")
                                    select new MasterEmployeeDTO
                                    {
                                        HRMEM_EmailId = c.HRMEM_EmailId
                                    }

                                         ).ToList();
                        if (list.Count > 0)
                        {
                            emailid = list.FirstOrDefault().HRMEM_EmailId;
                        }
                        sendmail(dto.MI_Id, emailid, "UpdatedFormate", dmoObj.HRME_Id);

                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }
                else
                {
                    var duplicatecountresult = 0;
                    if (dmoObj.HRME_EmployeeCode != null)
                    {
                        duplicatecountresult = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();
                    }
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRME_ActiveFlag = true;
                        dmoObj.HRME_LeftFlag = false;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.HRME_UpdatedBy = dto.LogInUserId;
                        dmoObj.HRME_CreatedBy = dto.LogInUserId;

                        var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();

                        if (resultCout > 0)
                        {
                            dto.retrunMsg = "Duplicate";
                            return dto;
                        }

                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            MasterEmployeeDTO DTO = Mapper.Map<MasterEmployeeDTO>(dmoObj);
                            DTO.HRME_EmployeeOrder = Convert.ToInt32(DTO.HRME_Id);
                            var resultd = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == DTO.HRME_Id);
                            Mapper.Map(DTO, resultd);
                            _HRMSContext.Update(resultd);
                            _HRMSContext.SaveChanges();
                            dto.Employeedto.HRME_Id = dmoObj.HRME_Id;
                            dto.Employeedto.MI_Id = dto.MI_Id;
                            dto.retrunMsg = "Add";

                            for (int i = 0; i < dto.email_list_dto.Length; i++)
                            {
                                var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == dto.email_list_dto[i].HRMEM_EmailId && d.HRMEM_DeFaultFlag == "default").Count();


                                student_email_id(dto);

                            }

                            for (int t = 0; t < dto.email_list_dto.Length; t++)
                            {
                                var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == dto.email_list_dto[t].HRMEM_EmailId && d.HRMEM_DeFaultFlag == "default").Count();

                                student_mobile_no(dto);


                            }

                            AddUpdateEmployeeBankDetails(dto);


                            var currentEmp = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                            Mapper.Map(currentEmp, dto.Employeedto);
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
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

        //Contact Details
        public MasterEmployeeDTO AddUpdateEmployeeAddressDetails(MasterEmployeeDTO dto)
        {

            dto.retrunMsg = "";
            try
            {

                MasterEmployee dmoObj = Mapper.Map<MasterEmployee>(dto.Employeedto);

                if (dmoObj.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);

                    dto.Employeedto.UpdatedDate = DateTime.Now;
                    dto.Employeedto.HRME_UpdatedBy = dto.LogInUserId;
                    Mapper.Map(dto.Employeedto, result);
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";
                        var currentEmp = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                        Mapper.Map(currentEmp, dto.Employeedto);
                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }
                else
                {
                    var duplicatecountresult = 0;
                    if (dmoObj.HRME_EmployeeCode != null)
                    {
                        duplicatecountresult = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();
                    }
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRME_ActiveFlag = true;
                        dmoObj.HRME_LeftFlag = false;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.HRME_CreatedBy = dto.LogInUserId;
                        dmoObj.HRME_UpdatedBy = dto.LogInUserId;

                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            MasterEmployeeDTO DTO = Mapper.Map<MasterEmployeeDTO>(dmoObj);
                            DTO.HRME_EmployeeOrder = Convert.ToInt32(DTO.HRME_Id);
                            var resultd = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == DTO.HRME_Id);
                            Mapper.Map(DTO, resultd);
                            _HRMSContext.Update(resultd);
                            _HRMSContext.SaveChanges();
                            dto.Employeedto.HRME_Id = dmoObj.HRME_Id;
                            dto.Employeedto.MI_Id = dto.MI_Id;
                            dto.retrunMsg = "Add";
                            var currentEmp = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                            Mapper.Map(currentEmp, dto.Employeedto);
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
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

        //Qualification details
        public MasterEmployeeDTO AddUpdateEmployeeQualificationDetails(MasterEmployeeDTO dto)
        {
            //add/update Qulaification details
            try
            {
                if (dto.EmployeeQulaificationDTO.Count() > 0)
                {
                    foreach (Master_Employee_QulaificationDTO QulaificationDTO in dto.EmployeeQulaificationDTO)
                    {
                        QulaificationDTO.MI_Id = dto.Employeedto.MI_Id;
                        QulaificationDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        Master_Employee_Qulaification Qulaification = Mapper.Map<Master_Employee_Qulaification>(QulaificationDTO);

                        if (Qulaification.HRMEQ_Id > 0)
                        {
                            var Qulaificationresult = _HRMSContext.Master_Employee_Qulaification.Single(t => t.HRMEQ_Id == QulaificationDTO.HRMEQ_Id);
                            //added by 20/05/2017   
                            Qulaificationresult.UpdatedDate = DateTime.Now;
                            Qulaificationresult.HRMEQ_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(QulaificationDTO, Qulaificationresult);
                            _HRMSContext.Update(Qulaificationresult);
                            var flag = _HRMSContext.SaveChanges();
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
                            Qulaification.CreatedDate = DateTime.Now;
                            Qulaification.UpdatedDate = DateTime.Now;
                            Qulaification.HRMEQ_CreatedBy = dto.LogInUserId;
                            _HRMSContext.Add(Qulaification);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        //Experiance details
        public MasterEmployeeDTO AddUpdateEmployeeExperienceDetails(MasterEmployeeDTO dto)
        {
            //add/update Experience details
            try
            {
                if (dto.EmployeeExperienceDTO.Count() > 0)
                {
                    foreach (Master_Employee_ExperienceDTO ExperienceDTO in dto.EmployeeExperienceDTO)
                    {
                        ExperienceDTO.MI_Id = dto.Employeedto.MI_Id;
                        ExperienceDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        Master_Employee_Experience Experience = Mapper.Map<Master_Employee_Experience>(ExperienceDTO);

                        if (Experience.HRMEE_Id > 0)
                        {
                            var Experienceresult = _HRMSContext.Master_Employee_Experience.Single(t => t.HRMEE_Id == ExperienceDTO.HRMEE_Id);
                            //added by 20/05/2017   
                            Experienceresult.UpdatedDate = DateTime.Now;
                            Experienceresult.HRMEE_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(ExperienceDTO, Experienceresult);
                            _HRMSContext.Update(Experienceresult);
                            var flag = _HRMSContext.SaveChanges();
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
                            Experience.CreatedDate = DateTime.Now;
                            Experience.UpdatedDate = DateTime.Now;
                            Experience.HRMEE_CreatedBy = dto.LogInUserId;

                            _HRMSContext.Add(Experience);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        //Document details
        public MasterEmployeeDTO AddUpdateEmployeeDocumentDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EmployeeDocumentDTO.Count() > 0)
                {
                    foreach (Master_Employee_DocumentsDTO DocumentsDTO in dto.EmployeeDocumentDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        Master_Employee_Documents Documents = Mapper.Map<Master_Employee_Documents>(DocumentsDTO);

                        if (Documents.HRMEDS_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.Master_Employee_Documents.Single(t => t.HRMEDS_Id == DocumentsDTO.HRMEDS_Id);
                            //added by 20/05/2017   
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documentsresult.HRMEDS_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
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
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HRMEDS_CreatedBy = dto.LogInUserId;
                            Documents.HRMEDS_UpdatedBy = dto.LogInUserId;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public MasterEmployeeDTO AddUpdateEmployeeOtherDetails(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                MasterEmployee dmoObj = Mapper.Map<MasterEmployee>(dto.Employeedto);

                if (dmoObj.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                    Feeconcession(dto);
                    UpdateLeftEmployee(dto);
                    dto.Employeedto.UpdatedDate = DateTime.Now;
                    dto.Employeedto.HRME_UpdatedBy = dto.LogInUserId;
                    dto.HRME_DOC = dto.Employeedto.HRME_DOC;
                    Mapper.Map(dto.Employeedto, result);
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();

                    //var empShiftData = _HRMSContext.EmployeeShiftMappingDMO.Where(t => t.HRME_Id == dmoObj.HRME_Id).ToList();
                    //foreach (var item in empShiftData)
                    //{
                    //    var res = _HRMSContext.EmployeeShiftMappingDMO.Single(a => a.FOEST_Id == item.FOEST_Id);
                    //    _HRMSContext.Remove(res);
                    //   
                    //}

                    if (dto.Employeedto.HRME_LeftFlag == true)
                    {
                        var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_Alumini_User_Creation @p0", dmoObj.HRME_Id);
                        if (contactExistsP > 0)
                        {
                            string tomail = _HRMSContext.Emp_Email_Id.Where(t => t.HRME_Id == dmoObj.HRME_Id && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                            string hrccmail = (from a in _HRMSContext.HR_Master_DepartmentCodeDMO
                                               from b in _HRMSContext.HR_Master_DepartmentCode_Head
                                               from c in _HRMSContext.Emp_Email_Id
                                               where a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.HRME_Id && c.HRMEM_DeFaultFlag == "default" && a.HRMDC_Code == "HR"
                                               select c).Select(t => t.HRMEM_EmailId).FirstOrDefault();
                            string ccheadmail = (from a in _HRMSContext.HR_Master_DepartmentCodeDMO
                                                 from b in _HRMSContext.HR_Master_DepartmentCode_Head
                                                 from c in _HRMSContext.Emp_Email_Id
                                                 from d in _HRMSContext.MasterEmployee
                                                 from e in _HRMSContext.HR_Master_Department
                                                 where a.HRMDC_ID == e.HRMDC_ID && d.HRMD_Id == e.HRMD_Id && a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.HRME_Id && c.HRMEM_DeFaultFlag == "default" && a.HRMDC_Code == "HR"
                                                 select c).Select(t => t.HRMEM_EmailId).FirstOrDefault();
                            string ccmail = hrccmail + "," + ccheadmail;

                            if (tomail.Length > 0)
                            {

                                //SendAlumniCredential(dmoObj.MI_Id, "SendAlumniCredential", dmoObj.HRME_Id, tomail, ccmail);
                            }
                        }
                        else
                        {
                            //dto.returnval = "notUpdated";
                        }
                    }





                    //if (dto.FrontOfficedto.Count() > 0)
                    //{
                    //    EmployeeShiftMappingDMO objpge = new EmployeeShiftMappingDMO();
                    //    foreach (var item in dto.FrontOfficedto)
                    //    {

                    //        objpge.HRME_Id = dmoObj.HRME_Id;
                    //        objpge.MI_Id = dmoObj.MI_Id;
                    //        objpge.FOHWDT_Id = item.FOHWDT_Id;
                    //        objpge.FOMS_Id = item.FOMS_Id;
                    //        objpge.FOEST_IHalfLoginTime = item.FOMST_IHalfLoginTime;
                    //        objpge.FOEST_IIHalfLogoutTime = item.FOMST_IIHalfLogoutTime;
                    //        objpge.FOEST_IHalfLogoutTime = item.FOMST_IHalfLogoutTime;
                    //        objpge.FOEST_IIHalfLoginTime = item.FOMST_IIHalfLoginTime;
                    //        objpge.FOEST_FDWHrMin = item.FOMST_FDWHrMin;
                    //        objpge.FOEST_HDWHrMin = item.FOMST_HDWHrMin;
                    //        objpge.FOEST_DelayPerShiftHrMin = item.FOMST_DelayPerShiftHrMin;
                    //        objpge.FOEST_EarlyPerShiftHrMin = item.FOMST_EarlyPerShiftHrMin;
                    //        objpge.FOEST_LunchHoursDuration = item.FOMST_LunchHoursDuration;
                    //        //objpge.FOEST_FixTimings = item.FOMST_FixTimings;
                    //        objpge.CreatedDate = DateTime.Now;
                    //        objpge.UpdatedDate = DateTime.Now;
                    //        _HRMSContext.Add(objpge);
                    //        _HRMSContext.SaveChanges();


                    //    }

                    //}



                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";

                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                    var resultss = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);

                    var Paramaeters = _HRMSContext.Staff_User_Login.Where(i => i.Emp_Code == dmoObj.HRME_Id).Select(d => d.Id).ToList();
                    var result1 = _HRMSContext.UserRoleWithInstituteDMO.Where(j => Paramaeters.Contains(j.Id)).ToList();
                    if (result1.Count > 0)
                    {
                        if (dto.Employeedto.HRME_LeftFlag == true)
                        {
                            result1[0].Activeflag = 0;
                            result1[0].UpdatedDate = DateTime.Now;
                        }
                        else if (dto.Employeedto.HRME_LeftFlag == false)
                        {
                            result1[0].Activeflag = 1; result1[0].UpdatedDate = DateTime.Now;
                        }
                        _HRMSContext.Update(result1[0]);

                        var flags = _HRMSContext.SaveChanges();
                        if (flags > 0)
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
                    var duplicatecountresult = 0;
                    if (dmoObj.HRME_EmployeeCode != null)
                    {
                        duplicatecountresult = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();
                    }
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRME_ActiveFlag = true;

                        dmoObj.HRME_LeftFlag = false;
                        dmoObj.HRME_DOC = dto.Employeedto.HRME_DOC;
                        //dmoObj.HRME_DOC = dto.HRME_DOC;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.HRME_CreatedBy = dto.LogInUserId;
                        dmoObj.HRME_UpdatedBy = dto.LogInUserId;

                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            MasterEmployeeDTO DTO = Mapper.Map<MasterEmployeeDTO>(dmoObj);
                            DTO.HRME_EmployeeOrder = Convert.ToInt32(DTO.HRME_Id);
                            var resultd = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == DTO.HRME_Id);
                            dmoObj.HRME_DOC = dto.HRME_DOC;
                            Mapper.Map(DTO, resultd);
                            _HRMSContext.Update(resultd);
                            _HRMSContext.SaveChanges();
                            dto.Employeedto.HRME_Id = dmoObj.HRME_Id;
                            dto.Employeedto.MI_Id = dto.MI_Id;
                            dto.retrunMsg = "Add";
                            _HRMSContext.SaveChanges();


                            //var empShiftData = _HRMSContext.EmployeeShiftMappingDMO.Where(t => t.HRME_Id == dmoObj.HRME_Id).ToList();
                            //foreach (var item in empShiftData)
                            //{
                            //    var res = _HRMSContext.EmployeeShiftMappingDMO.Single(a => a.FOEST_Id == item.FOEST_Id);
                            //    _HRMSContext.Remove(res);
                            //}



                            //if (dto.FrontOfficedto.Count() > 0)
                            //{
                            //    EmployeeShiftMappingDMO objpge = new EmployeeShiftMappingDMO();
                            //    foreach (var item in dto.FrontOfficedto)
                            //    {

                            //        objpge.HRME_Id = dmoObj.HRME_Id;
                            //        objpge.MI_Id = dmoObj.MI_Id;
                            //        objpge.FOHWDT_Id = item.FOHWDT_Id;
                            //        objpge.FOMS_Id = item.FOMS_Id;
                            //        objpge.FOEST_IHalfLoginTime = item.FOMST_IHalfLoginTime;
                            //        objpge.FOEST_IIHalfLogoutTime = item.FOMST_IIHalfLogoutTime;
                            //        objpge.FOEST_IHalfLogoutTime = item.FOMST_IHalfLogoutTime;
                            //        objpge.FOEST_IIHalfLoginTime = item.FOMST_IIHalfLoginTime;
                            //        objpge.FOEST_FDWHrMin = item.FOMST_FDWHrMin;
                            //        objpge.FOEST_HDWHrMin = item.FOMST_HDWHrMin;
                            //        objpge.FOEST_DelayPerShiftHrMin = item.FOMST_DelayPerShiftHrMin;
                            //        objpge.FOEST_EarlyPerShiftHrMin = item.FOMST_EarlyPerShiftHrMin;
                            //        objpge.FOEST_LunchHoursDuration = item.FOMST_LunchHoursDuration;
                            //        //objpge.FOEST_FixTimings = item.FOMST_FixTimings;
                            //        objpge.CreatedDate = DateTime.Now;
                            //        objpge.UpdatedDate = DateTime.Now;
                            //        _HRMSContext.Add(objpge);


                            //    }

                            //}

                            //var contactExists = _HRMSContext.SaveChanges();




                            var resultss = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);

                            var Paramaeters = _HRMSContext.Staff_User_Login.Where(i => i.Emp_Code == dmoObj.HRME_Id).Select(d => d.Id).ToList();
                            var result1 = _HRMSContext.UserRoleWithInstituteDMO.Where(j => Paramaeters.Contains(j.Id)).ToList();
                            if (result1.Count > 0)
                            {
                                if (dto.Employeedto.HRME_LeftFlag == true)
                                {
                                    result1[0].Activeflag = 0;
                                    result1[0].UpdatedDate = DateTime.Now;

                                }
                                else if (dto.Employeedto.HRME_LeftFlag == false)
                                {
                                    result1[0].Activeflag = 1; result1[0].UpdatedDate = DateTime.Now;
                                }
                                _HRMSContext.Update(result1[0]);
                            }
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
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

        //Increament details
        public MasterEmployeeDTO AddUpdateEmployeeIncreamentDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.IncrementDetailsDTO != null)
                {

                    dto.IncrementDetailsDTO.MI_Id = dto.Employeedto.MI_Id;
                    dto.IncrementDetailsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                    HR_Master_Employee_IncrementDetails IncrementDetails = Mapper.Map<HR_Master_Employee_IncrementDetails>(dto.IncrementDetailsDTO);

                    if (dto.IncrementDetailsDTO.HRMEID_Id > 0)
                    {
                        var Incrementresult = _HRMSContext.HR_Master_Employee_IncrementDetails.Single(t => t.HRMEID_Id == dto.IncrementDetailsDTO.HRMEID_Id);
                        //added by 20/05/2017   
                        Mapper.Map(dto.IncrementDetailsDTO, Incrementresult);
                        _HRMSContext.Update(Incrementresult);
                        _HRMSContext.SaveChanges();

                    }
                    else
                    {
                        _HRMSContext.Add(IncrementDetails);
                        _HRMSContext.SaveChanges();

                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
            }

            return dto;
        }

        //Employee Earning details
        public MasterEmployeeDTO AddUpdateEmployeeEarningDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EarningDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.EarningDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        //HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);                    

                        if (DocumentsDTO.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);

                            Documentsresult.HREED_Amount = DocumentsDTO.HREED_Amount;
                            Documentsresult.HREED_Percentage = DocumentsDTO.HREED_Percentage;
                            Documentsresult.HRMED_Id = DocumentsDTO.HRMED_Id;
                            Documentsresult.HREED_UpdatedBy = dto.LogInUserId;
                            Documentsresult.UpdatedDate = DateTime.Now;

                            // Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
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
                            HR_Employee_EarningsDeductions Documents = new HR_Employee_EarningsDeductions();
                            Documents.MI_Id = DocumentsDTO.MI_Id;
                            Documents.HRME_Id = DocumentsDTO.HRME_Id;
                            Documents.HRMED_Id = DocumentsDTO.HRMED_Id;
                            Documents.HREED_Amount = DocumentsDTO.HREED_Amount;
                            Documents.HREED_Percentage = DocumentsDTO.HREED_Percentage;

                            Documents.HREED_CreatedBy = dto.LogInUserId;
                            Documents.HREED_UpdatedBy = dto.LogInUserId;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.CreatedDate = DateTime.Now;

                            Documents.HREED_ActiveFlag = true;
                            Documents.CreatedDate = DateTime.Now;
                            Documents.HREED_CreatedBy = dto.LogInUserId;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREED_UpdatedBy = dto.LogInUserId;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        //Employee Arrear details
        public MasterEmployeeDTO AddUpdateEmployeeArrearDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EarningDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.ArrearDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);

                        if (Documents.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documentsresult.HREED_UpdatedBy = dto.LogInUserId;
                            //added by 20/05/2017  
                            //  DocumentsDTO.HREED_ActiveFlag = true;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
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
                            // Documents.HREED_ActiveFlag = true;
                            Documents.CreatedDate = DateTime.Now;
                            Documents.HREED_CreatedBy = dto.LogInUserId;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREED_UpdatedBy = dto.LogInUserId;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        //Employee Deduction details
        public MasterEmployeeDTO AddUpdateEmployeeDeductionDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.DeductionDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.DeductionDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);

                        if (Documents.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);
                            //added by 20/05/2017   
                            // DocumentsDTO.HREED_ActiveFlag = true;
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Documentsresult.HREED_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
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
                            Documents.CreatedDate = DateTime.Now;
                            Documents.HREED_CreatedBy = dto.LogInUserId;
                            Documents.UpdatedDate = DateTime.Now;
                            Documents.HREED_UpdatedBy = dto.LogInUserId;
                            //Documents.HREED_ActiveFlag = true;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public MasterEmployeeDTO editData(int id)
        {

            MasterEmployeeDTO dto = new MasterEmployeeDTO();
            dto.retrunMsg = "";
            try
            {
                List<MasterEmployee> lorg = new List<MasterEmployee>();
                lorg = _HRMSContext.MasterEmployee.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.employeedetailList = lorg.ToArray();

                //Qualification details
                List<Master_Employee_Qulaification> Qulaificationlorg = new List<Master_Employee_Qulaification>();
                Qulaificationlorg = _HRMSContext.Master_Employee_Qulaification.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.qualificationDetails = Qulaificationlorg.ToArray();


                //Experiance details
                List<Master_Employee_Experience> Experiencelorg = new List<Master_Employee_Experience>();
                Experiencelorg = _HRMSContext.Master_Employee_Experience.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.experienceDetails = Experiencelorg.ToArray();

                //Documents details
                List<Master_Employee_Documents> Documentslorg = new List<Master_Employee_Documents>();
                Documentslorg = _HRMSContext.Master_Employee_Documents.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.documentList = Documentslorg.ToArray();


                //increament and Basic Amount details
                List<HR_Master_Employee_IncrementDetails> IncrementDetails = new List<HR_Master_Employee_IncrementDetails>();
                IncrementDetails = _HRMSContext.HR_Master_Employee_IncrementDetails.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.incrementDetails = IncrementDetails.ToArray();


                //Employee Earning & deduction details

                List<HR_Employee_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Employee_EarningsDeductionsDTO>();
                //EarningsDeductionsDetails = _HRMSContext.HR_Employee_EarningsDeductions.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();


                EarningsDeductionsDetails = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from med in _HRMSContext.HR_Master_EarningsDeductions
                                             from hrme in _HRMSContext.MasterEmployee
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true && emp.HRME_Id == hrme.HRME_Id
                                             && emp.HRME_Id == id
                                             select new HR_Employee_EarningsDeductionsDTO
                                             {
                                                 HREED_Id = emp.HREED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRME_Id = emp.HRME_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HREED_Amount = emp.HREED_Amount,
                                                 HREED_Percentage = emp.HREED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HREED_ActiveFlag = emp.HREED_ActiveFlag,
                                                 HRME_CTC = hrme.HRME_CTC
                                             }).ToList();

                dto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.Distinct().ToArray();

                var mobile_list = (from a in _HRMSContext.Emp_MobileNo
                                   from b in _HRMSContext.MasterEmployee
                                   where (a.HRME_Id == b.HRME_Id && a.HRME_Id == id)
                                   select a).ToList();

                dto.selectedmobile_list_dto = mobile_list.ToArray();


                var email_list = (from a in _HRMSContext.MasterEmployee
                                  from b in _HRMSContext.Emp_Email_Id
                                  where (a.HRME_Id == b.HRME_Id && a.HRME_Id == id)
                                  select b).ToList();

                dto.selectedemail_list_dto = email_list.ToArray();

                var empdata = (from a in _HRMSContext.MasterEmployee
                               from b in _HRMSContext.HR_Master_Employee_Bank
                               where (a.HRME_Id == b.HRME_Id && a.HRME_Id == id)
                               select b).ToList();

                dto.employeebankDetails = empdata.ToArray();


                dto.userdetails = (from a in _HRMSContext.Staff_User_Login
                                   from b in _HRMSContext.applicationuser
                                   from c in _HRMSContext.ApplicationUserRoleDMO
                                   where (a.Emp_Code == id && a.Id == b.Id && a.Id == c.UserId)
                                   select new MasterEmployeeDTO
                                   {
                                       username = b.UserName,
                                       IVRMRT_Id = c.RoleTypeId
                                   }).ToArray();


                //var HRME_CTC = _HRMSContext.MasterEmployee.Where(t=>t.HRME_Id=)
                //EmployeeList.HRME_CTC

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeShiftDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                    {
                        Value = id
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
                        dto.employeeshifts = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }








                ////
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public MasterEmployeeDTO deactivate(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dto.HRME_Id);
                    var Paramaeters = _HRMSContext.Staff_User_Login.Where(i => i.Emp_Code == dto.HRME_Id).Select(d => d.Id).ToList();
                    var result1 = _HRMSContext.UserRoleWithInstituteDMO.Where(j => Paramaeters.Contains(j.Id)).ToList();
                    if (result1.Count > 0)
                    {
                        if (result.HRME_ActiveFlag == true)
                        {
                            result1[0].Activeflag = 0;
                            result1[0].UpdatedDate = DateTime.Now;

                        }
                        else if (result.HRME_ActiveFlag == false)
                        {
                            result1[0].Activeflag = 1;
                            result1[0].UpdatedDate = DateTime.Now;
                        }
                        _HRMSContext.Update(result1[0]);
                    }

                    if (result.HRME_ActiveFlag == true)
                    {
                        result.HRME_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        result.HRME_UpdatedBy = dto.LogInUserId;
                    }

                    else if (result.HRME_ActiveFlag == false)
                    {
                        result.HRME_ActiveFlag = true;
                        result.HRME_UpdatedBy = dto.LogInUserId;
                        result.UpdatedDate = DateTime.Now;
                    }

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRME_ActiveFlag == true)
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
        public MasterEmployeeDTO GetAllDropdownAndDatatableDetails(MasterEmployeeDTO dto)
        {
            List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<IVRM_Master_Gender> Genderlist = new List<IVRM_Master_Gender>();
            List<IVRM_Master_Marital_Status> MaritalStatuslist = new List<IVRM_Master_Marital_Status>();
            List<HR_Master_EarningsDeductions> earningdeductiondatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistArrear = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductions> arreardatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> grossdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistGross = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_BankDeatils> banklist = new List<HR_Master_BankDeatils>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Page_Privilege_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                    {
                        Value = dto.LogInUserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@pageurl", SqlDbType.VarChar)
                    {
                        Value = "app.employeeRegistration"
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
                        dto.pagePrivilege = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                //Only it will work once page will mapp to that perticular employee
                if (dto.pagePrivilege.Length > 0)
                {



                    PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                   from pa in _HRMSContext.HR_PROCESSDMO
                                   from cc in _HRMSContext.Staff_User_Login
                                   where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)


                                   select pa
                          ).ToList();

                    if (PROCESSList.Count() > 0)
                    {

                        List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                        List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                        List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                        EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                        dto.employeedetailList = EmployeeList.ToArray();

                        //GroupTypelist
                        GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                        dto.groupTypedropdownlist = GroupTypelist.ToArray();

                        //Departmentlist
                        Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                        dto.departmentdropdownlist = Departmentlist.ToArray();

                        //Designationlist
                        Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                        dto.designationdropdownlist = Designationlist.ToArray();

                    }
                    else
                    {
                        EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                        dto.employeedetailList = EmployeeList.ToArray();

                        //GroupTypelist
                        GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                        dto.groupTypedropdownlist = GroupTypelist.ToArray();

                        //Departmentlist
                        Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                        dto.departmentdropdownlist = Departmentlist.ToArray();

                        //Designationlist
                        Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                        dto.designationdropdownlist = Designationlist.ToArray();
                    }






                    banklist = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMBD_ActiveFlag == true).ToList();
                    dto.bankdropdownlist = banklist.ToArray();

                    //emptype
                    EmployeeTypelist = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToList();
                    dto.employeeTypedropdownlist = EmployeeTypelist.ToArray();

                    //Gradelist
                    Gradelist = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMG_ActiveFlag == true).OrderBy(t => t.HRMG_Order).ToList();
                    dto.gradedropdownlist = Gradelist.ToArray();

                    //Genderlist
                    Genderlist = _HRMSContext.IVRM_Master_Gender.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.IVRMMG_ActiveFlag == true).ToList();
                    dto.genderdropdownlist = Genderlist.ToArray();

                    //MaritalStatuslist
                    MaritalStatuslist = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.IVRMMMS_ActiveFlag == true).ToList();
                    dto.maritalStatusdropdownlist = MaritalStatuslist.ToArray();

                    //Courselist
                    Courselist = _HRMSContext.HR_Master_Course.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMC_ActiveFlag == true).OrderBy(t => t.HRMC_Order).ToList();
                    dto.coursedropdownlist = Courselist.ToArray();


                    dto.empshifts = _HRMSContext.masterShifts.Where(t => t.MI_Id == dto.MI_Id && t.FOMS_ActiveFlg == true).ToArray();

                    dto.holiday_types = _HRMSContext.holidayWorkingDayType.Where(t => t.MI_Id == dto.MI_Id && t.FOHWDT_ActiveFlg == true).ToArray();


                    //
                    dto.fillroletype = (from a in _HRMSContext.institutionRolePrivileges
                                        from b in _HRMSContext.masterRoleType
                                        from c in _HRMSContext.institution_Module_Page
                                        from d in _HRMSContext.Institution_Module
                                        where (a.IVRMRT_Id == b.IVRMRT_Id && c.IVRMIMP_Id == a.IVRMIMP_Id && d.IVRMIM_Id == c.IVRMIM_Id && d.MI_Id == dto.MI_Id)
                                        select b).Distinct().ToArray();

                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                    dto.configurationDetails = dmoObj;

                    var AllCastess = (from a in _HRMSContext.Caste
                                      from b in _HRMSContext.CasteCategory
                                      where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == dto.MI_Id)
                                      select a).ToArray();

                    dto.castedropdownlist = AllCastess.Distinct().ToArray();

                    dto.documentList = _HRMSContext.MasterDocumentDMO.Where(t => t.MI_Id == dto.MI_Id).ToArray();


                    //Earning list
                    earningdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning")
                    && t.HRMED_ActiveFlag == true).ToList();

                    if (earningdatalist.Count() > 0)
                    {
                        foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                        {
                            HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                            if (phdto.HRMED_AmountPercentFlag == "Percentage")
                            {
                                var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                                if (EarningsDeductionsPerlist.Count() > 0)
                                {
                                    List<string> percentOff = new List<string>();

                                    foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                    {
                                        var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

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

                    //Deduction List
                    deductiondatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Deduction") && t.HRMED_ActiveFlag == true).ToList();


                    if (deductiondatalist.Count() > 0)
                    {
                        foreach (HR_Master_EarningsDeductions ph in deductiondatalist)
                        {
                            HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                            if (phdto.HRMED_AmountPercentFlag == "Percentage")
                            {
                                var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                                if (EarningsDeductionsPerlist.Count() > 0)
                                {
                                    List<string> percentOff = new List<string>();

                                    foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                    {
                                        var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

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

                    //Arrear list
                    arreardatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Arrear") && t.HRMED_ActiveFlag == true).ToList();

                    if (arreardatalist.Count() > 0)
                    {
                        foreach (HR_Master_EarningsDeductions ph in arreardatalist)
                        {
                            HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                            if (phdto.HRMED_AmountPercentFlag == "Percentage")
                            {
                                var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                                if (EarningsDeductionsPerlist.Count() > 0)
                                {
                                    List<string> percentOff = new List<string>();

                                    foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                    {
                                        var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

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

                            DTOdatalistArrear.Add(phdto);

                        }

                    }

                    dto.arrearList = DTOdatalistArrear.ToArray();

                    //Gross list
                    grossdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Gross") && t.HRMED_ActiveFlag == true).ToList();

                    if (grossdatalist.Count() > 0)
                    {
                        foreach (HR_Master_EarningsDeductions ph in grossdatalist)
                        {
                            HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                            if (phdto.HRMED_AmountPercentFlag == "Percentage")
                            {
                                var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                                if (EarningsDeductionsPerlist.Count() > 0)
                                {
                                    List<string> percentOff = new List<string>();

                                    foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                    {
                                        var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

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

                            DTOdatalistGross.Add(phdto);
                        }
                    }

                    dto.grossList = DTOdatalistGross.ToArray();

                    List<Country> Allcountry = new List<Country>();
                    Allcountry = _Context.country.Where(c => c.IVRMMC_CountryName != null).ToList();
                    dto.countrydropdownlist = Allcountry.ToArray();

                    //caste category
                    List<CasteCategory> AllcasteCategory = new List<CasteCategory>();
                    AllcasteCategory = _HRMSContext.CasteCategory.Where(c => c.IMCC_CategoryName != null).ToList();
                    dto.casteCategorydropdownlist = AllcasteCategory.ToArray();

                    List<MasterReligionDMO> AllReligion = new List<MasterReligionDMO>();
                    AllReligion = _HRMSContext.Religion.Where(r => r.Is_Active == true && r.IVRMMR_Name != null).ToList();
                    dto.religiondropdownlist = AllReligion.ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //get statelistByCountryId
        public MasterEmployeeDTO getstateDropdown(int id)//int IVRMM_Id
        {
            MasterEmployeeDTO dto = new MasterEmployeeDTO();
            List<State> AllState = new List<State>();
            if (id > 0)
            {
                AllState = _Context.State.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
                dto.AllState = AllState.ToArray();
            }
            return dto;
        }

        //validate data
        public MasterEmployeeDTO validateData(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                // if (dto.HRME_Id == 0)
                //  {
                if (dto.Type.Equals("EmployeeOrder"))
                {
                    return EmployeeOrderDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("mobileNo"))
                {
                    return MobileNoDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("EmailID"))
                {
                    return EmailIdDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("AadharCardNo"))
                {
                    return AdharcardNumberDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("PANNumber"))
                {
                    return PANCardNumberDuplicateCheck(dto);
                }

                else if (dto.Type.Equals("SpouseEmailId"))
                {
                    return SpouseEmailIdDuplicateCheck(dto);
                }

                else if (dto.Type.Equals("SpouseMobileNo"))
                {
                    return SpouseMobileNoDuplicateCheck(dto);
                }

                else if (dto.Type.Equals("BiometricCode"))
                {
                    return BiometricCodeDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("EmployeeCode"))
                {
                    return EmployeeCodeDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("RFCardId"))
                {
                    return RFCardIdDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("PFAccNo"))
                {
                    return PFAccNoDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("UINumber"))
                {
                    return UINumberDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("ESIAccNo"))
                {
                    return ESIAccNoDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("GratuityAccNo"))
                {
                    return GratuityAccNoDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("NationalSSN"))
                {
                    return NationalSSNDuplicateCheck(dto);
                }

                //  }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public MasterEmployeeDTO pindingtasks(MasterEmployeeDTO dto)
        {
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_task_creation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hrme_id ", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
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
                        dto.emppindingtasks = retObject.Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public MasterEmployeeDTO sentintemation(MasterEmployeeDTO dto)
        {
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Empheaddetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
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
                        var managmentempdetails = retObject.ToList();

                        if (managmentempdetails.Count() > 0)
                        {

                            var empemailId = _HRMSContext.Emp_Email_Id.Where(t => t.HRME_Id == dto.HRME_Id && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();


                            sendmailDeptHead(dto.MI_Id, managmentempdetails[0].HRME_EmployeeFirstName, empemailId, "DeptHeadTaskItmn", managmentempdetails[0].Deptheadname, managmentempdetails[0].Deptheadmailid, dto.emptaskcount);
                            dto.retrunMsg = "Sent";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }








            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        private MasterEmployeeDTO NationalSSNDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_NationalSSN.Equals(dto.HRME_NationalSSN)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_NationalSSN.Equals(dto.HRME_NationalSSN) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }
        private MasterEmployeeDTO GratuityAccNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {

                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_GratuityAccNo.Equals(dto.HRME_GratuityAccNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }


                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_GratuityAccNo.Equals(dto.HRME_GratuityAccNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }
        private MasterEmployeeDTO ESIAccNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ESIAccNo.Equals(dto.HRME_ESIAccNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }

                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ESIAccNo.Equals(dto.HRME_ESIAccNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }
        private MasterEmployeeDTO PFAccNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {

                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_PFAccNo.Equals(dto.HRME_PFAccNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_PFAccNo.Equals(dto.HRME_PFAccNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }
        private MasterEmployeeDTO UINumberDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {

                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_UINumber.Equals(dto.HRME_UINumber)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_UINumber.Equals(dto.HRME_UINumber) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }
        private MasterEmployeeDTO RFCardIdDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_RFCardId.Equals(dto.HRME_RFCardId)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_RFCardId.Equals(dto.HRME_RFCardId) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }
        private MasterEmployeeDTO EmployeeCodeDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeCode.Equals(dto.HRME_EmployeeCode)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeCode.Equals(dto.HRME_EmployeeCode)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }
        private MasterEmployeeDTO BiometricCodeDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_BiometricCode.Equals(dto.HRME_BiometricCode)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_BiometricCode.Equals(dto.HRME_BiometricCode) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        //Employee Order validation 
        public MasterEmployeeDTO EmployeeOrderDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeOrder.Equals(dto.HRME_EmployeeOrder)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeOrder.Equals(dto.HRME_EmployeeOrder) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        //mobile Number validation
        public MasterEmployeeDTO MobileNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_MobileNo.Equals(dto.HRME_MobileNo)).Count();

                if (resultCout > 0)
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        //Email Id validation
        public MasterEmployeeDTO EmailIdDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                //var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) 
                //&& t.HRME_LeftFlag==false 
                //&& t.HRME_EmailId.Equals(dto.HRME_EmailId)).Count();

                //    if (resultCout > 0)
                //    {
                //        dto.retrunMsg = "Duplicate";
                //        return dto;
                //    }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        //Adharcard Number validation
        public MasterEmployeeDTO AdharcardNumberDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_AadharCardNo.Equals(dto.HRME_AadharCardNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_AadharCardNo.Equals(dto.HRME_AadharCardNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        //PAN Card Number validation
        public MasterEmployeeDTO PANCardNumberDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_PANCardNo.Equals(dto.HRME_PANCardNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_PANCardNo.Equals(dto.HRME_PANCardNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        //Spouse Email Id validation
        public MasterEmployeeDTO SpouseEmailIdDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_SpouseEmailId.Equals(dto.HRME_SpouseEmailId)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_SpouseEmailId.Equals(dto.HRME_SpouseEmailId) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        //
        //Spouse mobile Number validation
        public MasterEmployeeDTO SpouseMobileNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_SpouseMobileNo.Equals(dto.HRME_SpouseMobileNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_SpouseMobileNo.Equals(dto.HRME_SpouseMobileNo) && t.HRME_Id != dto.HRME_Id).Count();
                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        //salary calculation

        //validate data  
        public MasterEmployeeDTO SalaryCalculation(HR_Master_Employee_IncrementDetailsDTO dto)
        {
            MasterEmployeeDTO EmpDTO = new MasterEmployeeDTO();
            List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();
            try
            {
                //Earning list
                earningdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning")).ToList();

                if (earningdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = new HR_Master_EarningsDeductionsDTO();

                        phdto.HREED_Amount = 0;

                        if (ph.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(ph.MI_Id) && t.HRMED_Id == ph.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault().HRMED_Name);

                                    if (percentOffHRMED_Name.FirstOrDefault().HRMED_Name == "Basic Pay")
                                    {
                                        phdto.HREED_Amount = phdto.HREED_Amount + dto.HRMEID_Amount;
                                    }
                                    else
                                    {
                                        phdto.HREED_Amount = phdto.HREED_Amount + Convert.ToInt64(percentOffHRMED_Name.FirstOrDefault().HRMED_AmountPercent);
                                    }
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                                phdto.HREED_Amount = (Convert.ToInt64(phdto.HRMED_AmountPercent) / 100) * phdto.HREED_Amount;
                            }
                            else
                            {
                                phdto.percentOff = "";
                                phdto.HREED_Amount = Convert.ToDecimal(phdto.HRMED_AmountPercent);
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                            phdto.HREED_Amount = Convert.ToDecimal(phdto.HRMED_AmountPercent);
                        }

                        DTOdatalistEarning.Add(phdto);
                    }
                }
                EmpDTO.earningList = DTOdatalistEarning.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return EmpDTO;
        }

        //onchange into set order fuction 
        public MasterEmployeeDTO employeesetorder(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //Order updated
                if (dto.EmporderDTO.Count() > 0)
                {
                    foreach (MasterEmployeeDTO mob in dto.EmporderDTO)
                    {
                        if (mob.HRME_Id > 0)
                        {
                            var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == mob.HRME_Id);
                            result.HRME_UpdatedBy = dto.LogInUserId;
                            result.UpdatedDate = DateTime.Now;
                            Mapper.Map(mob, result);
                            _HRMSContext.Update(result);
                            _HRMSContext.SaveChanges();
                        }
                    }
                    dto.retrunMsg = "Order updated successfully";
                }
                else
                {
                    dto.retrunMsg = "No records found to set Order !!!";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public Master_Employee_QulaificationDTO DeleteQualificationRecord(Master_Employee_QulaificationDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //Delete Existing Record 
                List<Master_Employee_Qulaification> lorg = new List<Master_Employee_Qulaification>();
                lorg = _HRMSContext.Master_Employee_Qulaification.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRMC_Id == dto.HRMC_Id).ToList();

                foreach (Master_Employee_Qulaification ph1 in lorg)
                {
                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public Master_Employee_ExperienceDTO DeleteExperienceRecord(Master_Employee_ExperienceDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //Delete Existing Record 
                List<Master_Employee_Experience> lorg = new List<Master_Employee_Experience>();
                lorg = _HRMSContext.Master_Employee_Experience.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRMEE_Id == dto.HRMEE_Id).ToList();

                foreach (Master_Employee_Experience ph1 in lorg)
                {
                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;

        }
        public Master_Employee_DocumentsDTO DeleteDocumentRecord(Master_Employee_DocumentsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //Delete Existing Record 
                List<Master_Employee_Documents> lorg = new List<Master_Employee_Documents>();
                lorg = _HRMSContext.Master_Employee_Documents.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRMEDS_Id == dto.HRMEDS_Id).ToList();

                foreach (Master_Employee_Documents ph1 in lorg)
                {
                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;

        }
        public MasterEmployeeDTO student_mobile_no(MasterEmployeeDTO datastdmobile)
        {
            try
            {
                List<long> temparr = new List<long>();
                List<long> temparr1 = new List<long>();
                //getting all mobilenumbers
                foreach (Mobile_Number_DTO ph in datastdmobile.mobile_list_dto)
                {
                    temparr.Add(ph.HRMEMNO_Id);
                }
                if (temparr.Count() > 0)
                {
                    //removing mobile number 
                    Array Phone_Noresultremove = _HRMSContext.Emp_MobileNo.Where(t => !temparr.Contains(t.HRMEMNO_Id) && t.HRME_Id == datastdmobile.Employeedto.HRME_Id).ToArray();
                    foreach (Multiple_Mobile_DMO ph1 in Phone_Noresultremove)
                    {
                        _HRMSContext.Remove(ph1);
                    }

                }



                //updating and saving 

                foreach (Mobile_Number_DTO ph in datastdmobile.mobile_list_dto)
                {
                    ph.HRME_Id = datastdmobile.Employeedto.HRME_Id;
                    Multiple_Mobile_DMO phone = Mapper.Map<Multiple_Mobile_DMO>(ph);
                    if (phone.HRMEMNO_Id > 0)
                    {

                        var tcnoduplicate23 = _HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == ph.HRMEMNO_MobileNo).Count();
                        if (tcnoduplicate23 > 0)
                        {
                            datastdmobile.tcflagexists = "Mobile Number Already Exists";
                            datastdmobile.tcflagexists = "Duplicate";
                        }
                        else
                        {

                            var Phone_Noresult = _HRMSContext.Emp_MobileNo.Single(t => t.HRMEMNO_Id == ph.HRMEMNO_Id);
                            ph.UpdatedDate = DateTime.Now;
                            ph.CreatedDate = Phone_Noresult.CreatedDate;
                            Phone_Noresult.HRMEMNO_MobileNo = ph.HRMEMNO_MobileNo;
                            Mapper.Map(ph, Phone_Noresult);
                            _HRMSContext.Update(Phone_Noresult);

                            foreach (var mob in datastdmobile.mobile_list_dto)
                            {
                                if (mob.HRMEMNO_DeFaultFlag == "default")
                                {
                                    datastdmobile.HRMEMNO_MobileNo = mob.HRMEMNO_MobileNo;
                                }
                            }
                        }

                        foreach (var mail in datastdmobile.email_list_dto)
                        {
                            if (mail.HRMEM_DeFaultFlag == "default")
                            {
                                datastdmobile.HRMEM_EmailId = mail.HRMEM_EmailId;
                            }
                        }

                        var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == datastdmobile.HRMEM_EmailId).Count();
                        if (tcnoduplicate > 0)
                        {
                            datastdmobile.tcflagexists = "Email_Id Already Exists";

                        }

                        else if (datastdmobile.HRMEM_EmailId != null || datastdmobile.HRMEM_EmailId != "")
                        {
                            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ApplicationUser_UPDATE";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)
                                {
                                    Value = Convert.ToString(datastdmobile.HRMEM_EmailId)
                                });

                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(ph.HRME_Id)
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
                                    // retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }

                        // var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(datastdmobile.MI_Id) && t.HRME_MobileNo.Equals(datastdmobile.HRMEMNO_MobileNo)).Count();
                        var tcnoduplicate2 = _HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == datastdmobile.HRMEMNO_MobileNo).Count();
                        if (tcnoduplicate2 > 0)
                        {
                            datastdmobile.tcflagexists = "Mobile Number Already Exists";
                            datastdmobile.tcflagexists = "Duplicate";
                        }


                        else if (datastdmobile.HRMEMNO_MobileNo != 0)
                        {
                            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ApplicationUser_UPDATE_mobile";
                                cmd.CommandType = CommandType.StoredProcedure;


                                cmd.Parameters.Add(new SqlParameter("@PhoneNo", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(datastdmobile.HRMEMNO_MobileNo)
                                });

                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(ph.HRME_Id)
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
                                    // retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }


                        if (datastdmobile.HRME_Photo != null || datastdmobile.HRME_Photo != "")
                        {
                            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ApplicationUser_UPDATE_photo";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@Photo", SqlDbType.NVarChar)
                                {
                                    Value = Convert.ToString(datastdmobile.Employeedto.HRME_Photo)
                                });

                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(ph.HRME_Id)
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
                                    // retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }



                    }
                    else
                    {
                        ph.CreatedDate = DateTime.Now;
                        ph.UpdatedDate = DateTime.Now;
                        phone.HRMEMNO_MobileNo = ph.HRMEMNO_MobileNo;
                        Mapper.Map(ph, phone);
                        _HRMSContext.Add(phone);
                    }
                    var count = _HRMSContext.SaveChanges();
                    if (count > 0)
                    {
                        ApplUser objuser = new ApplUser();
                        objuser = (from a in _HRMSContext.Staff_User_Login
                                   from b in _HRMSContext.applicationuser
                                   where (a.Emp_Code == ph.HRME_Id && a.MI_Id == datastdmobile.MI_Id && a.Id == b.Id)
                                   select b).FirstOrDefault();
                        if (objuser != null)
                        {
                            objuser.PhoneNumber = ph.HRMEMNO_MobileNo.ToString();
                            _HRMSContext.Update(objuser);
                            _HRMSContext.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                // _log.LogInformation("Student_Mobile error");
                // _log.LogDebug(e.Message);
                Console.WriteLine(e.Message);
                _HRMSContext.Database.RollbackTransaction();
            }
            return datastdmobile;
        }

        // Employee Bank Details
        public MasterEmployeeDTO AddUpdateEmployeeBankDetails(MasterEmployeeDTO datastdmobile)
        {
            try
            {
                List<long> temparr = new List<long>();
                List<long> temparr1 = new List<long>();
                //getting all mobilenumbers
                foreach (HR_Master_Employee_BankDTO ph in datastdmobile.employeebankDTO)
                {
                    temparr.Add(ph.HRMEB_Id);
                }

                if (temparr.Count() > 0)
                {
                    //removing mobile number 
                    Array EmployeeBankDetailsresultremove = _HRMSContext.HR_Master_Employee_Bank.Where(t => !temparr.Contains(t.HRMEB_Id) && t.HRME_Id == datastdmobile.Employeedto.HRME_Id).ToArray();
                    foreach (HR_Master_Employee_Bank ph1 in EmployeeBankDetailsresultremove)
                    {
                        _HRMSContext.Remove(ph1);
                    }
                }

                //updating and saving 

                foreach (HR_Master_Employee_BankDTO ph in datastdmobile.employeebankDTO)
                {
                    ph.HRME_Id = datastdmobile.Employeedto.HRME_Id;
                    HR_Master_Employee_Bank EmployeeBankDetails = Mapper.Map<HR_Master_Employee_Bank>(ph);
                    if (EmployeeBankDetails.HRMEB_Id > 0)
                    {
                        var EmployeeBankDetailsresult = _HRMSContext.HR_Master_Employee_Bank.Single(t => t.HRMEB_Id == ph.HRMEB_Id);
                        ph.UpdatedDate = DateTime.Now;
                        Mapper.Map(ph, EmployeeBankDetailsresult);
                        _HRMSContext.Update(EmployeeBankDetailsresult);
                    }
                    else
                    {
                        ph.CreatedDate = DateTime.Now;
                        ph.UpdatedDate = DateTime.Now;
                        Mapper.Map(ph, EmployeeBankDetails);
                        _HRMSContext.Add(EmployeeBankDetails);
                    }
                    _HRMSContext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _HRMSContext.Database.RollbackTransaction();
            }
            return datastdmobile;
        }
        public MasterEmployeeDTO student_email_id(MasterEmployeeDTO datastdemail)
        {
            try
            {
                if (datastdemail.email_list_dto != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all emails
                    foreach (Email_Id_DTO ph in datastdemail.email_list_dto)
                    {
                        temparr.Add(ph.HRMEEM_Id);
                    }

                    if (temparr.Count() > 0)
                    {
                        //removing email number 
                        Array Phone_Noresultremove = _HRMSContext.Emp_Email_Id.Where(t => !temparr.Contains(t.HRMEEM_Id) && t.HRME_Id == datastdemail.Employeedto.HRME_Id).ToArray();
                        foreach (Email_Id_DTO ph1 in Phone_Noresultremove)
                        {
                            _HRMSContext.Remove(ph1);
                        }
                    }

                    //updating and saving
                    foreach (Email_Id_DTO ph in datastdemail.email_list_dto)
                    {
                        ph.HRME_Id = datastdemail.Employeedto.HRME_Id;

                        Multiple_Email_DMO phone = Mapper.Map<Multiple_Email_DMO>(ph);
                        if (phone.HRMEEM_Id > 0)
                        {
                            var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == ph.HRMEM_EmailId).Count();
                            if (tcnoduplicate > 0)
                            {
                                datastdemail.tcflagexists = "Email_Id Already Exists";
                            }
                            else
                            {
                                var Phone_Noresult = _HRMSContext.Emp_Email_Id.Single(t => t.HRMEEM_Id == ph.HRMEEM_Id);
                                ph.UpdatedDate = DateTime.Now;
                                ph.CreatedDate = Phone_Noresult.CreatedDate;
                                Phone_Noresult.HRMEM_EmailId = ph.HRMEM_EmailId;
                                Mapper.Map(ph, Phone_Noresult);
                                _HRMSContext.Update(Phone_Noresult);
                            }
                        }
                        else
                        {
                            ph.CreatedDate = DateTime.Now;
                            ph.UpdatedDate = DateTime.Now;
                            //phone.CreatedDate = DateTime.Now;
                            //phone.UpdatedDate = DateTime.Now;
                            phone.HRMEM_EmailId = ph.HRMEM_EmailId;
                            Mapper.Map(ph, phone);
                            _HRMSContext.Add(phone);
                        }

                        var count = _HRMSContext.SaveChanges();
                        if (count > 0)
                        {
                            ApplUser objuser = new ApplUser();
                            objuser = (from a in _HRMSContext.Staff_User_Login
                                       from b in _HRMSContext.applicationuser
                                       where (a.Emp_Code == ph.HRME_Id && a.MI_Id == datastdemail.MI_Id && a.Id == b.Id)
                                       select b).FirstOrDefault();
                            if (objuser != null)
                            {
                                objuser.Email = ph.HRMEM_EmailId.ToString();
                                objuser.NormalizedEmail = ph.HRMEM_EmailId.ToString();
                                _HRMSContext.Update(objuser);
                                _HRMSContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _HRMSContext.Database.RollbackTransaction();
            }

            return datastdemail;
        }

        //checking duplicate mobile number
        public MasterEmployeeDTO Dupl_mob(Mobile_Number_DTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();

            try
            {
                if (dto.HRMEMNO_Id == 0)
                {
                    var tcnoduplicate = _HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == dto.HRMEMNO_MobileNo).Count();
                    if (tcnoduplicate > 0)
                    {
                        dd.tcflagexists = "Mobile Number Already Exists";

                    }
                }
                else
                {
                    var tcnoduplicate = _HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == dto.HRMEMNO_MobileNo && d.HRMEMNO_Id != dto.HRMEMNO_Id).Count();
                    if (tcnoduplicate > 0)
                    {
                        dd.tcflagexists = "Mobile Number Already Exists";

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }

        //Dupl_email
        public MasterEmployeeDTO Dupl_email(Email_Id_DTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();
            try
            {
                if (dto.HRMEEM_Id == 0)
                {
                    var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == dto.HRMEM_EmailId).Count();
                    if (tcnoduplicate > 0)
                    {
                        dd.tcflagexists = "Email_Id Already Exists";
                    }
                }
                else
                {
                    var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == dto.HRMEM_EmailId && d.HRMEEM_Id != dto.HRMEEM_Id).Count();
                    if (tcnoduplicate > 0)
                    {
                        dd.tcflagexists = "Email_Id Already Exists";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }

        //duplicate_bankAccountNo
        public MasterEmployeeDTO duplicate_bankAccountNo(HR_Master_Employee_BankDTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();
            try
            {
                if (dto.HRMEB_Id == 0)
                {
                    var tcnoduplicate = _HRMSContext.HR_Master_Employee_Bank.Where(d => d.HRMEB_AccountNo == dto.HRMEB_AccountNo).Count();
                    if (tcnoduplicate > 0)
                    {
                        dd.tcflagexists = "Account Number Already Exists";
                    }
                }
                else
                {
                    var tcnoduplicate = _HRMSContext.HR_Master_Employee_Bank.Where(d => d.HRMEB_AccountNo == dto.HRMEB_AccountNo && d.HRMEB_Id != dto.HRMEB_Id).Count();
                    if (tcnoduplicate > 0)
                    {
                        dd.tcflagexists = "Account Number Already Exists";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }

        //del_mob
        public MasterEmployeeDTO del_mob(Mobile_Number_DTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();
            dd.retrunMsg = "";
            try
            {
                List<Multiple_Mobile_DMO> lorg = new List<Multiple_Mobile_DMO>();
                lorg = _HRMSContext.Emp_MobileNo.Where(t => t.HRMEMNO_Id.Equals(dto.HRMEMNO_Id)).ToList();
                foreach (Multiple_Mobile_DMO ph1 in lorg)
                {
                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dd.retrunMsg = "Deleted";
                }
            }
            //Delete Existing Record 
            // List<Master_Employee_Qulaification> lorg = new List<Master_Employee_Qulaification>();
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }

        //del_email
        public MasterEmployeeDTO del_email(Email_Id_DTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();
            dd.retrunMsg = "";
            try
            {

                List<Multiple_Email_DMO> lorg = new List<Multiple_Email_DMO>();
                lorg = _HRMSContext.Emp_Email_Id.Where(t => t.HRMEEM_Id.Equals(dto.HRMEEM_Id)).ToList();

                foreach (Multiple_Email_DMO ph1 in lorg)
                {
                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dd.retrunMsg = "Deleted";
                }
            }
            //Delete Existing Record 
            // List<Master_Employee_Qulaification> lorg = new List<Master_Employee_Qulaification>();

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }
        public MasterEmployeeDTO delete_bankAccountNo(HR_Master_Employee_BankDTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();
            dd.retrunMsg = "";
            try
            {
                List<HR_Master_Employee_Bank> lorg = new List<HR_Master_Employee_Bank>();
                lorg = _HRMSContext.HR_Master_Employee_Bank.Where(t => t.HRMEB_Id.Equals(dto.HRMEB_Id)).ToList();
                foreach (HR_Master_Employee_Bank ph1 in lorg)
                {
                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dd.retrunMsg = "Deleted";
                }
            }
            //Delete Existing Record 
            // List<Master_Employee_Qulaification> lorg = new List<Master_Employee_Qulaification>();

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }
        //OnLoadSaveEmployeeSalaryHeads
        public MasterEmployeeDTO OnLoadSaveEmployeeSalaryHeads(MasterEmployeeDTO dto)
        {
            //Earning list
            List<HR_Master_EarningsDeductions> masterheadlist = new List<HR_Master_EarningsDeductions>();
            try
            {
                var EmployeeHRMED_Idlist = _HRMSContext.HR_Employee_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(dto.HRME_Id)).Select(t => t.HRMED_Id);
                if (EmployeeHRMED_Idlist.Count() > 0)
                {
                    masterheadlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && !EmployeeHRMED_Idlist.Contains(t.HRMED_Id) && t.HRMED_ActiveFlag == true).ToList();
                }
                else
                {
                    masterheadlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToList();
                }

                if (masterheadlist.Count > 0)
                {

                    foreach (HR_Master_EarningsDeductions ph in masterheadlist)
                    {
                        HR_Employee_EarningsDeductionsDTO EmployeeDetails = new HR_Employee_EarningsDeductionsDTO();
                        EmployeeDetails.HRMED_Id = ph.HRMED_Id;
                        EmployeeDetails.HRME_Id = dto.HRME_Id;
                        EmployeeDetails.MI_Id = dto.MI_Id;
                        EmployeeDetails.HREIC_CreatedBy = dto.LogInUserId;
                        EmployeeDetails.CreatedDate = DateTime.Now;
                        EmployeeDetails.HREIC_UpdatedBy = dto.LogInUserId;
                        EmployeeDetails.UpdatedDate = DateTime.Now;

                        if (ph.HRMED_AmountPercentFlag == "Amount")
                        {
                            EmployeeDetails.HREED_Amount = Convert.ToDecimal(ph.HRMED_AmountPercent);
                            EmployeeDetails.HREED_Percentage = "0";
                        }
                        else
                        {
                            EmployeeDetails.HREED_Percentage = ph.HRMED_AmountPercent;
                            EmployeeDetails.HREED_Amount = 0;
                        }
                        EmployeeDetails.HREED_ActiveFlag = true;
                        //EmployeeDetails. = dto.MI_Id;
                        //EmployeeDetails.MI_Id = dto.MI_Id;
                        HR_Employee_EarningsDeductions phdto = Mapper.Map<HR_Employee_EarningsDeductions>(EmployeeDetails);
                        _HRMSContext.Add(phdto);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            // dto.retrunMsg = "Add";
                        }
                        else
                        {
                            // dto.retrunMsg = "false";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public MasterEmployeeDTO getEmployeeSalaryDetails(MasterEmployeeDTO dto)
        {
            List<HR_Master_EarningsDeductions> mstheads = new List<HR_Master_EarningsDeductions>();
            try
            {
                OnLoadSaveEmployeeSalaryHeads(dto);

                HR_Employee_EarningsDeductionsDTO phdto = new HR_Employee_EarningsDeductionsDTO();
                phdto.MI_Id = dto.MI_Id;
                phdto.HRME_Id = dto.HRME_Id;
                //Calculating the standard salarycalculation
                CalculateEmployeeEarnDeductionDetailsByHead(phdto);

                //Employee Earning & deduction details
                List<HR_Employee_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Employee_EarningsDeductionsDTO>();
                //EarningsDeductionsDetails = _HRMSContext.HR_Employee_EarningsDeductions.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();

                EarningsDeductionsDetails = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from med in _HRMSContext.HR_Master_EarningsDeductions
                                             from hrme in _HRMSContext.MasterEmployee
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true && emp.HRME_Id == hrme.HRME_Id
                                             && emp.HRME_Id == dto.HRME_Id
                                             select new HR_Employee_EarningsDeductionsDTO
                                             {
                                                 HREED_Id = emp.HREED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRME_Id = emp.HRME_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HREED_Amount = emp.HREED_Amount,
                                                 HREED_Percentage = emp.HREED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HREED_ActiveFlag = emp.HREED_ActiveFlag,
                                                 HRME_CTC = hrme.HRME_CTC
                                             }).ToList();

                dto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public MasterEmployeeDTO getEmployeeSalaryDetailsByHead(HR_Employee_EarningsDeductionsDTO dto)
        {
            MasterEmployeeDTO empdto = new MasterEmployeeDTO();
            try
            {
                if (dto.HREED_Id > 0)
                {

                    var employeedata = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dto.HRME_Id);
                    employeedata.HRME_CTC = dto.HRME_CTC;
                    _HRMSContext.Update(employeedata);

                    var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == dto.HREED_Id);
                    //added by 20/05/2017   
                    // DocumentsDTO.HREED_ActiveFlag = true;
                    Mapper.Map(dto, Documentsresult);
                    _HRMSContext.Update(Documentsresult);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        CalculateEmployeeEarnDeductionDetailsByHead(dto);
                    }
                    else
                    {
                        // dto.retrunMsg = "false";
                    }

                }


                List<HR_Employee_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Employee_EarningsDeductionsDTO>();

                EarningsDeductionsDetails = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from med in _HRMSContext.HR_Master_EarningsDeductions
                                             from hrme in _HRMSContext.MasterEmployee
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true && emp.HRME_Id == hrme.HRME_Id
                                             && emp.HRME_Id == dto.HRME_Id
                                             select new HR_Employee_EarningsDeductionsDTO
                                             {
                                                 HREED_Id = emp.HREED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRME_Id = emp.HRME_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HREED_Amount = emp.HREED_Amount,
                                                 HREED_Percentage = emp.HREED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HREED_ActiveFlag = emp.HREED_ActiveFlag,
                                                 HRME_CTC = hrme.HRME_CTC,
                                             }
                                ).ToList();


                empdto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return empdto;
        }
        public HR_Employee_EarningsDeductionsDTO CalculateEmployeeEarnDeductionDetailsByHead(HR_Employee_EarningsDeductionsDTO dto)
        {
            try
            {
                //var empheadDetail = _HRMSContext.HR_Master_EarningsDeductions.SingleOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id == dto.HRMED_Id);

                //var AmountPercentage = "";
                //if (empheadDetail.HRMED_AmountPercentFlag.Equals("Amount"))
                //{
                //    AmountPercentage = dto.HREED_Amount.ToString();
                //}
                //else
                //{
                //    AmountPercentage = dto.HREED_Percentage;
                //}

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeSalaryDetailDynamicCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt) { Value = Convert.ToInt64(dto.HRME_Id) });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt) { Value = Convert.ToInt64(dto.MI_Id) });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        // var data = cmd.ExecuteNonQuery();
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public MasterEmployeeDTO getcaste(MasterEmployeeDTO data)
        {
            var AllCastess = (from a in _HRMSContext.Caste
                              from b in _HRMSContext.CasteCategory
                              where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == data.MI_Id && a.IMCC_Id == data.CasteCategoryId)
                              select a).ToArray();

            data.castedropdownlist = AllCastess.Distinct().ToArray();
            return data;
        }

        public MasterEmployeeDTO useridcreation(MasterEmployeeDTO data)
        {
            var useriddata = (from a in _HRMSContext.MasterEmployee
                              from b in _HRMSContext.HR_Master_Designation
                              from c in _HRMSContext.Emp_Email_Id
                              where (a.MI_Id == b.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.HRME_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == data.HRME_Id)
                              select new MasterEmployeeDTO
                              {
                                  HRME_Id = a.HRME_Id,
                                  MI_Id = a.MI_Id,
                                  HRME_Photo = a.HRME_Photo,
                                  HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                  HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                  HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                  HRME_MobileNo = a.HRME_MobileNo,
                                  HRME_BloodGroup = a.HRME_BloodGroup,
                                  HRME_EmployeeCode = a.HRME_EmployeeCode,
                                  HRME_DOJ = a.HRME_DOJ,
                                  HRMEM_EmailId = c.HRMEM_EmailId,
                                  HRMDES_DesignationName = b.HRMDES_DesignationName,
                                  HRME_PerStreet = a.HRME_PerStreet,
                                  HRME_PerArea = a.HRME_PerArea,
                                  HRME_PerCity = a.HRME_PerCity,
                                  HRME_PerPincode = a.HRME_PerPincode

                              }).ToArray();

            data.idcarddata = useriddata.Distinct().ToArray();
            return data;
        }


        public async Task<MasterEmployeeDTO> Feeconcession(MasterEmployeeDTO dto)
        {
            try
            {
                dto = await Feeconcessionprocedure(dto, "JSH_Employee_left_fee_Concession");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;

        }
        public async Task<MasterEmployeeDTO> Feeconcessionprocedure(MasterEmployeeDTO dto, string ProcedureName)
        {
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = ProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt) { Value = Convert.ToInt64(dto.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value = Convert.ToInt64(dto.HRME_Id) });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public string sendmailDeptHead(long MI_Id, string EmpName, string EmpEmail, string Template, string Deptheadname, string Deptheadmail, string taskcount)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }


                //  string Empname = _vmsconte.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).Select(p => p.HRME_EmployeeFirstName).FirstOrDefault();
                Mailmsg = Mailmsg.Replace("[NAME]", EmpName);
                Mailmsg = Mailmsg.Replace("[TaskCount]", taskcount);
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

                    message.AddCc(EmpEmail);
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
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Deptheadmail);

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
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Deptheadmail
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

        public string sendmail(long MI_Id, string Email, string Template, long UserID)
        {
            try
            {
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

                if (Template == "EMAILOTP" || Template == "EMAILOTP2F")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
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
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
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
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
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
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

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
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

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


                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

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
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public void UpdateLeftEmployee(MasterEmployeeDTO dto)
        {
            if (dto.Employeedto.HRME_LeftFlag == true)
            {
                var feecheck = _HRMSContext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id == dto.MI_Id && t.FMCC_ConcessionFlag == "E").ToList();
                if (feecheck.Count > 0)
                {
                    try
                    {
                        var academic_year = _Context.AcademicYear.Where(e => e.MI_Id == dto.MI_Id && DateTime.Now.Date >= e.ASMAY_From_Date && DateTime.Now.Date <= e.ASMAY_To_Date).Select(t => t.ASMAY_Id).FirstOrDefault();

                        using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "DELETEEMPCONCESSION";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt) { Value = Convert.ToInt64(dto.MI_Id) });
                            cmd.Parameters.Add(new SqlParameter("@EMPCDE", SqlDbType.VarChar) { Value = Convert.ToInt64(dto.Employeedto.HRME_Id) });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = Convert.ToInt64(academic_year) });
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
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
        }

        public string SendAlumniCredential(long MI_Id, string Template, long HRME_ID, string email, string ccemail)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Email_Prametars_Replace";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",
                  SqlDbType.VarChar)
                    {
                        Value = HRME_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                  SqlDbType.VarChar)
                    {
                        Value = "SendAlumniCredential"
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

                                    else if (datatype.Name == "Decimal")
                                    {
                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();
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
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
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
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;

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
                    // smstpdetails = _Context.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    string mailcc = "";
                    string mailbcc = "";

                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

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
                    message.AddTo(email);
                    // message.AddTo("kavitajnandoor21@gmail.com");
                    //message.AddTo("sreepriyagangadharan15@gmail.com");

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
                    //******************* EMAIL CC DETAILS ****************//

                    if (ccemail != null && ccemail != "")
                    {
                        string[] ccmaildetails = ccemail.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }


                    if (mailcc != null && mailcc != "")
                    {
                        string[] ccmaildetails = mailcc.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }

                    //if (ccemail != null && ccemail != "")
                    //{
                    //    message.AddBcc(ccemail);
                    //}

                    //******************* EMAIL BCC DETAILS ****************//




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
                    try
                    {
                        client.SendEmailAsync(message).Wait();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = email
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";

        }

    }
}