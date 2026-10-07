using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServiceHub.com.vaps.Services
{
    public class employeeadvanceapplyImpl : Interfaces.employeeadvanceapplyInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public employeeadvanceapplyImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Emp_SalaryAdvanceDTO getBasicData(HR_Emp_SalaryAdvanceDTO dto)
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

        public HR_Emp_SalaryAdvanceDTO SaveUpdate(HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.HRME_Id = _HRMSContext.Staff_User_Login.Single(c => c.Id == dto.LogInUserId && c.MI_Id == dto.MI_Id).Emp_Code;

            dto.retrunMsg = "";
            try
            {
                var mapped = (
                    from a in _HRMSContext.MasterEmployee
                    from b in _HRMSContext.HR_PROCESSDMO
                    from c in _HRMSContext.HR_Process_Auth_OrderNoDMO
                    from d in _HRMSContext.Staff_User_Login
                    where (b.HRMDES_Id == a.HRMDES_Id && b.HRPA_Id == c.HRPA_Id && d.Id == c.IVRMUL_Id && b.HRPA_TypeFlag == "SalaryAdvance")
                    select b).Count();
                if (mapped > 0)
                {
                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    HR_ConfigurationDTO conobj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);

                    if (conobj.HRC_SalAdvApprovalFlg == true)
                    {
                        dto.HRESA_AdvStatus = "Applied";
                    }
                    else
                    {
                        dto.retrunMsg = "Not Mapped";
                        return dto;
                        //dto.HRESA_AdvStatus = "Sanctioned";
                        //dto.HRESA_SanctinedAmount = dto.HRESA_AppliedAmount;
                    }
                    //cheching previous salary calucation
                
                    var currentDate = dto.HRESA_EntryDate;
                    var year = currentDate.Year;
                    var month = Convert.ToInt32(currentDate.Month);
                    var IVRM_Month_Id = month;
                    if (conobj.HRC_SalaryFromDay <= 1 && Convert.ToInt32(IVRM_Month_Id) <= 12)
                    {

                        IVRM_Month_Id = Convert.ToInt32(month) - 1;
                    }

                    var presalryadvance = _HRMSContext.Month.Where (R=>R.IVRM_Month_Id == IVRM_Month_Id).ToArray();

                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PreviousAdvanceSalaryDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRESA_AdvYear", SqlDbType.VarChar)
                        {
                            Value = dto.HRESA_AdvYear
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRESA_AdvMonth", SqlDbType.VarChar)
                        {
                            Value = presalryadvance[0].IVRM_Month_Name
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
                            dto.preadvance = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    if (dto.preadvance.Length > 0)
                    {
                        if (dto.HRESA_AdvStatus == "Applied" | dto.HRESA_AdvStatus == "Partial Approved")
                        {
                            dto.retrunMsg = "AdvancePending";
                        }
                        else if (dto.HRESA_AdvStatus == "Sanctioned" | dto.HRESA_AdvStatus == "Rejected")
                        {

                            if (dto.HRESA_Id > 0)
                            {
                                var duplicatAdd = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HRESA_AdvYear == dto.HRESA_AdvYear && t.HRESA_AdvMonth == dto.HRESA_AdvMonth && t.HRESA_AppliedAmount == dto.HRESA_AppliedAmount && t.HRESA_Id != dto.HRESA_Id).Count();
                                if (duplicatAdd == 0)
                                {

                                    var result = _HRMSContext.HR_Emp_SalaryAdvance.Single(t => t.HRESA_Id == dto.HRESA_Id);


                                    result.HRESA_EntryDate = dto.HRESA_EntryDate;
                                    result.HRESA_AdvMonth = dto.HRESA_AdvMonth;
                                    result.HRESA_AdvYear = dto.HRESA_AdvYear;
                                    result.HRESA_AppliedAmount = dto.HRESA_AppliedAmount;
                                    result.HRESA_ModeOfPayment = dto.HRESA_ModeOfPayment;
                                    result.HRESA_ReferenceNo = dto.HRESA_ReferenceNo;
                                    result.HRESA_AdvStatus = dto.HRESA_AdvStatus;
                                    result.HRESA_Remarks = dto.HRESA_Remarks;
                                    result.HRESA_ActiveFlag = true;
                                    result.UpdatedDate = DateTime.Now;
                                    result.CreatedDate = DateTime.Now;
                                    result.HRESA_UpdatedBy = dto.LogInUserId;
                                    result.HRESA_CreatedBy = dto.LogInUserId;
                                    _HRMSContext.Update(result);
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
                                    dto.retrunMsg = "Duplicate";
                                    return dto;
                                }
                            }
                            else
                            {
                                HR_Emp_SalaryAdvance dmoObj = new HR_Emp_SalaryAdvance();
                                var duplicatAdd = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HRESA_AdvYear == dto.HRESA_AdvYear && t.HRESA_AdvMonth == dto.HRESA_AdvMonth && t.HRESA_AdvStatus != "Rejected" && t.HRESA_ActiveFlag == true).Count();
                                if (duplicatAdd == 0)
                                {
                                    dmoObj.HRESA_EntryDate = DateTime.Now;
                                    dmoObj.HRESA_AdvMonth = dto.HRESA_AdvMonth;
                                    dmoObj.HRESA_AdvYear = dto.HRESA_AdvYear;
                                    dmoObj.HRESA_AppliedAmount = dto.HRESA_AppliedAmount;
                                    dmoObj.HRESA_ModeOfPayment = dto.HRESA_ModeOfPayment;
                                    dmoObj.HRESA_ReferenceNo = dto.HRESA_ReferenceNo;
                                    dmoObj.HRESA_AdvStatus = dto.HRESA_AdvStatus;
                                    dmoObj.HRESA_Remarks = dto.HRESA_Remarks;
                                    dmoObj.MI_Id = dto.MI_Id;
                                    dmoObj.HRME_Id = dto.HRME_Id;
                                    dmoObj.HRESA_ActiveFlag = true;
                                    dmoObj.UpdatedDate = DateTime.Now;
                                    dmoObj.CreatedDate = DateTime.Now;
                                    dmoObj.HRESA_UpdatedBy = dto.LogInUserId;
                                    dmoObj.HRESA_CreatedBy = dto.LogInUserId;

                                    _HRMSContext.Add(dmoObj);
                                    var flag = _HRMSContext.SaveChanges();
                                    if (flag == 1)
                                    {
                                        dto.retrunMsg = "Add";
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
                    }
                    
                    else if(dto.preadvance.Length == 0)
                    {
                        if (dto.HRESA_Id > 0)
                        {
                            var duplicatAdd = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HRESA_AdvYear == dto.HRESA_AdvYear && t.HRESA_AdvMonth == dto.HRESA_AdvMonth && t.HRESA_AppliedAmount == dto.HRESA_AppliedAmount && t.HRESA_Id != dto.HRESA_Id).Count();
                            if (duplicatAdd == 0)
                            {

                                var result = _HRMSContext.HR_Emp_SalaryAdvance.Single(t => t.HRESA_Id == dto.HRESA_Id);


                                result.HRESA_EntryDate = dto.HRESA_EntryDate;
                                result.HRESA_AdvMonth = dto.HRESA_AdvMonth;
                                result.HRESA_AdvYear = dto.HRESA_AdvYear;
                                result.HRESA_AppliedAmount = dto.HRESA_AppliedAmount;
                                result.HRESA_ModeOfPayment = dto.HRESA_ModeOfPayment;
                                result.HRESA_ReferenceNo = dto.HRESA_ReferenceNo;
                                result.HRESA_AdvStatus = dto.HRESA_AdvStatus;
                                result.HRESA_Remarks = dto.HRESA_Remarks;
                                result.HRESA_ActiveFlag = true;
                                result.UpdatedDate = DateTime.Now;
                                result.CreatedDate = DateTime.Now;
                                result.HRESA_UpdatedBy = dto.LogInUserId;
                                result.HRESA_CreatedBy = dto.LogInUserId;
                                _HRMSContext.Update(result);
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
                                dto.retrunMsg = "Duplicate";
                                return dto;
                            }
                        }
                        else
                        {
                            HR_Emp_SalaryAdvance dmoObj = new HR_Emp_SalaryAdvance();
                            var duplicatAdd = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HRESA_AdvYear == dto.HRESA_AdvYear && t.HRESA_AdvMonth == dto.HRESA_AdvMonth && t.HRESA_AdvStatus != "Rejected" && t.HRESA_ActiveFlag == true).Count();
                            if (duplicatAdd == 0)
                            {
                                dmoObj.HRESA_EntryDate = DateTime.Now;
                                dmoObj.HRESA_AdvMonth = dto.HRESA_AdvMonth;
                                dmoObj.HRESA_AdvYear = dto.HRESA_AdvYear;
                                dmoObj.HRESA_AppliedAmount = dto.HRESA_AppliedAmount;
                                dmoObj.HRESA_ModeOfPayment = dto.HRESA_ModeOfPayment;
                                dmoObj.HRESA_ReferenceNo = dto.HRESA_ReferenceNo;
                                dmoObj.HRESA_AdvStatus = dto.HRESA_AdvStatus;
                                dmoObj.HRESA_Remarks = dto.HRESA_Remarks;
                                dmoObj.MI_Id = dto.MI_Id;
                                dmoObj.HRME_Id = dto.HRME_Id;
                                dmoObj.HRESA_ActiveFlag = true;
                                dmoObj.UpdatedDate = DateTime.Now;
                                dmoObj.CreatedDate = DateTime.Now;
                                dmoObj.HRESA_UpdatedBy = dto.LogInUserId;
                                dmoObj.HRESA_CreatedBy = dto.LogInUserId;

                                _HRMSContext.Add(dmoObj);
                                var flag = _HRMSContext.SaveChanges();
                                if (flag == 1)
                                {
                                    dto.retrunMsg = "Add";
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
                    
                }
                else
                {
                    dto.retrunMsg = "Not Mapped";
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Emp_SalaryAdvanceDTO Edit(HR_Emp_SalaryAdvanceDTO dto)
        {
            try
            {
                dto.empadvaList = _HRMSContext.HR_Emp_SalaryAdvance.Where(a => a.HRESA_Id == dto.HRESA_Id).ToArray();
                dto.advanceapproval =( from a in _HRMSContext.HR_Emp_SalaryAdvanceApproval
                                      from b in _HRMSContext.Staff_User_Login
                                      from c in _HRMSContext.MasterEmployee
                                      where (a.HRESA_Id == dto.HRESA_Id && a.IVRMUL_Id==b.Id && c.HRME_Id==b.Emp_Code)
                select new HR_Emp_SalaryAdvanceDTO
                {
                    HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + (string.IsNullOrEmpty(c.HRME_EmployeeMiddleName) ? "" : ' ' + c.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(c.HRME_EmployeeLastName) ? "" : ' ' + c.HRME_EmployeeLastName),
                    HRESAA_SanctionLevel=a.HRESAA_SanctionLevel,
                    HRESAA_Remarks=a.HRESAA_Remarks,
                    HRESAA_Amount = a.HRESAA_Amount,
                    HRESAA_Status =a.HRESAA_Status,
                    HRESAA_Date=a.HRESAA_Date

                }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public HR_Emp_SalaryAdvanceDTO deactivate(HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRESA_Id > 0)
                {
                    var result = _HRMSContext.HR_Emp_SalaryAdvance.Single(t => t.HRESA_Id == dto.HRESA_Id);

                    if (result.HRESA_ActiveFlag == true)
                    {
                        result.HRESA_ActiveFlag = false;
                    }
                    else if (result.HRESA_ActiveFlag == false)
                    {
                        result.HRESA_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    result.HRESA_UpdatedBy = dto.LogInUserId;
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRESA_ActiveFlag == true)
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Emp_SalaryAdvanceDTO GetAllDropdownAndDatatableDetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            List<HR_Emp_SalaryAdvance> datalist = new List<HR_Emp_SalaryAdvance>();

            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();
            List<Month> Monthlist = new List<Month>();

            try
            {
                dto.HRME_Id = _HRMSContext.Staff_User_Login.Where(t => t.Id == dto.LogInUserId).Select(t => t.Emp_Code).FirstOrDefault();

                var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning")
                                  select new HR_Employee_Salary_DetailsDTO
                                  {
                                      HRMED_Id = emp.HRMED_Id,
                                      HRMED_Name = mas.HRMED_Name,
                                      HRESD_Amount = emp.HREED_Amount,
                                      HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                                  }).ToList();
                dto.empEarningGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));

                var empDed = (from ded in _HRMSContext.HR_Employee_EarningsDeductions
                                  from demas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (ded.HRMED_Id == demas.HRMED_Id && ded.HREED_ActiveFlag == true && ded.HRME_Id == dto.HRME_Id && demas.HRMED_EarnDedFlag == "Deduction")
                                  select new HR_Employee_Salary_DetailsDTO
                                  {
                                      HRMED_Id = ded.HRMED_Id,
                                      HRMED_Name = demas.HRMED_Name,
                                      HRESD_Amount = ded.HREED_Amount,
                                      HRMED_EarnDedFlag = demas.HRMED_EarnDedFlag
                                  }).ToList();
                dto.empDeductionGrossSal = Convert.ToDecimal(empDed.Sum(t => t.HRESD_Amount));
                dto.empGrossSal = ((dto.empEarningGrossSal - dto.empDeductionGrossSal)/2);

                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.ToArray();

                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true && t.MI_Id == dto.MI_Id).ToList();
                dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;
                var datalistt = (from sal in _HRMSContext.HR_Emp_SalaryAdvance
                                 from emp in _HRMSContext.MasterEmployee
                                 where sal.HRME_Id == emp.HRME_Id && sal.MI_Id == dto.MI_Id && sal.HRME_Id == dto.HRME_Id
                                 select new HR_Emp_SalaryAdvanceDTO
                                 {
                                     HRESA_Id = sal.HRESA_Id,
                                     MI_Id = sal.MI_Id,
                                     HRME_Id = sal.HRME_Id,
                                     HRME_EmployeeFirstName = emp.HRME_EmployeeFirstName + (string.IsNullOrEmpty(emp.HRME_EmployeeMiddleName) ? "" : ' ' + emp.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(emp.HRME_EmployeeLastName) ? "" : ' ' + emp.HRME_EmployeeLastName),
                                     HRESA_EntryDate = sal.HRESA_EntryDate,
                                     HRESA_AdvMonth = sal.HRESA_AdvMonth,
                                     HRESA_AdvYear = sal.HRESA_AdvYear,
                                     HRESA_AppliedAmount = sal.HRESA_AppliedAmount,
                                     HRESA_ModeOfPayment = sal.HRESA_ModeOfPayment,
                                     HRESA_Remarks = sal.HRESA_Remarks,
                                     HRESA_AdvStatus = sal.HRESA_AdvStatus,
                                     HRESA_ActiveFlag = sal.HRESA_ActiveFlag,
                                     CreatedDate = sal.CreatedDate,
                                     UpdatedDate = sal.UpdatedDate
                                 }).ToList();
                dto.empadvaList = datalistt.ToArray();     
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public HR_Emp_SalaryAdvanceDTO getDetailsByEmployee(HR_Emp_SalaryAdvanceDTO dto)
        {    
            dto.totalAppliedAmount = 0;  
            dto.HRME_Id = _HRMSContext.Staff_User_Login.Where(t => t.Id == dto.LogInUserId).Select(t => t.Emp_Code).FirstOrDefault();
            List<HR_Emp_SalaryAdvance> totalApplied = new List<HR_Emp_SalaryAdvance>();
            decimal totalAppliedAmount = 0;
            try
            {
                var currentDate = dto.HRESA_EntryDate;
                var year = currentDate.Year;
                var month = Convert.ToInt32(currentDate.Month);


                if (year > 0 && month > 0)
                {

                    var config = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();
                    int IVRM_Month_Id = month;
                    if (month > 0)
                    {
                        if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(IVRM_Month_Id) < 12)
                        {

                            IVRM_Month_Id = Convert.ToInt32(month) + 1;
                        }
                        else if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(IVRM_Month_Id) == 12)
                        {
                            IVRM_Month_Id = 01;
                            year = (year + 1);
                        }
                        else
                        {
                            IVRM_Month_Id = Convert.ToInt32(IVRM_Month_Id);

                            var days = DateTime.DaysInMonth(Convert.ToInt32(year), IVRM_Month_Id);
                            config.HRC_SalaryToDay = days;
                        }

                        //employee list
                        DateTime selectedFromdate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryFromDay, 0, 0, 0, 0);


                        DateTime selectedTodate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryToDay, 0, 0, 0, 0);

                        if (dto.HRESA_Id == 0)
                        {
                            totalApplied = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.HRME_Id == dto.HRME_Id && t.MI_Id == dto.MI_Id && t.HRESA_EntryDate >= selectedFromdate && t.HRESA_EntryDate <= selectedTodate && t.HRESA_ActiveFlag == true && t.HRESA_AdvMonth == dto.HRESA_AdvMonth).ToList();
                        }
                        else
                        {
                            totalApplied = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.HRME_Id == dto.HRME_Id && t.MI_Id == dto.MI_Id && t.HRESA_EntryDate >= selectedFromdate && t.HRESA_EntryDate <= selectedTodate && t.HRESA_ActiveFlag == true && t.HRESA_Id != dto.HRESA_Id).ToList();
                        }

                        if (totalApplied.Count() > 0)
                        {
                            totalAppliedAmount = Convert.ToDecimal(totalApplied.Sum(t => t.HRESA_AppliedAmount));
                        }
                    }

                }
                dto.totalAppliedAmount = totalAppliedAmount;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return dto;
        }

    }
}