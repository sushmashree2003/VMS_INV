using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;


namespace HRMSServicesHub.com.vaps.Services
{
    public class HREmpSalaryAdvanceService : Interfaces.HREmpSalaryAdvanceInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HREmpSalaryAdvanceService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
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
            dto.retrunMsg = "";
            try
            {
                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO conobj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);

                if (conobj.HRC_SalAdvApprovalFlg == true)
                {
                    dto.HRESA_AdvStatus = "Applied";
                }
                else
                {
                    dto.HRESA_AdvStatus = "Sanctioned";
                    dto.HRESA_SanctinedAmount = dto.HRESA_AppliedAmount;
                }


                HR_Emp_SalaryAdvance dmoObj = Mapper.Map<HR_Emp_SalaryAdvance>(dto);

                if (dmoObj.HRESA_Id > 0)
                {
                    //   var duplicatAdd = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HRESA_AdvYear == dto.HRESA_AdvYear && t.HRESA_AdvMonth == dto.HRESA_AdvMonth && t.HRESA_Id != dmoObj.HRESA_Id).Count();
                    //  if (duplicatAdd == 0)
                    //     {

                    var result = _HRMSContext.HR_Emp_SalaryAdvance.Single(t => t.HRESA_Id == dmoObj.HRESA_Id);


                    dto.UpdatedDate = DateTime.Now;
                    result.HRESA_UpdatedBy = dto.LogInUserId;
                    Mapper.Map(dto, result);
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
                    //      }
                    //   else
                    //       {
                    //       dto.retrunMsg = "Duplicate";
                    //      return dto;
                    //     }



                }
                else
                {

                    //   var duplicatAdd = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HRESA_AdvYear == dto.HRESA_AdvYear && t.HRESA_AdvMonth == dto.HRESA_AdvMonth).Count();
                    //   if (duplicatAdd == 0)
                    //     {
                    // dmoObj.HRESA_EntryDate = DateTime.Now;
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
                    //    }
                    //  else
                    //     {
                    //     dto.retrunMsg = "Duplicate";
                    //     return dto;
                    //     }

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

        public HR_Emp_SalaryAdvanceDTO editData(int id)
        {
            HR_Emp_SalaryAdvanceDTO dto = new HR_Emp_SalaryAdvanceDTO();
            dto.retrunMsg = "";
            try
            {
                //  List<HR_Emp_SalaryAdvance> lorg = new List<HR_Emp_SalaryAdvance>();
                var lorg = _HRMSContext.HR_Emp_SalaryAdvance.AsNoTracking().Where(t => t.HRESA_Id.Equals(id)).ToList();

                dto = Mapper.Map<HR_Emp_SalaryAdvanceDTO>(lorg.FirstOrDefault());
                dto.empadvaList = lorg.ToArray();


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

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
                searchfilter(dto);
                GetTotalAppliedAmount(dto);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
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

        public HR_Emp_SalaryAdvanceDTO GetAllDropdownAndDatatableDetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            List<HR_Emp_SalaryAdvance> datalist = new List<HR_Emp_SalaryAdvance>();

            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();
            List<Month> Monthlist = new List<Month>();

            try
            {
                //employee
                var employees = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                 from mm in _HRMSContext.MasterEmployee
                                 from med in _HRMSContext.HR_Master_EarningsDeductions
                                 where mm.MI_Id.Equals(dto.MI_Id)
                                 && emp.HRMED_Id == med.HRMED_Id
                                 && med.HRMED_EDTypeFlag == "Advance" && emp.HREED_ActiveFlag == true
                                 && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id
                                 orderby mm.HRME_EmployeeOrder
                                 select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();
                dto.employeedropdown = employees.ToArray();

                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.ToArray();

                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true && t.MI_Id == dto.MI_Id).ToList();
                dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;


                if (employees.Count() > 0)
                {

                    var empIds = employees.Select(t => t.HRME_Id);

                    datalist = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.MI_Id.Equals(dto.MI_Id) && empIds.Contains(t.HRME_Id)).ToList();
                }

                dto.empadvaList = datalist.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public HR_Emp_SalaryAdvanceDTO empdetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
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
                        dto.griddisplay = retObject.ToArray();
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
        public HR_Emp_SalaryAdvanceDTO getDetailsByEmployee(HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.empGrossSal = 0;
            dto.totalAppliedAmount = 0;


            try
            {
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

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
                GetTotalAppliedAmount(dto);




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return dto;
        }

        public void GetTotalAppliedAmount(HR_Emp_SalaryAdvanceDTO dto)
        {
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
                            // var days = getNumberOfDays(Convert.ToInt32(dto.HRES_Year), IVRM_Month_Id);
                            var days = DateTime.DaysInMonth(Convert.ToInt32(year), IVRM_Month_Id);
                            config.HRC_SalaryToDay = days;
                        }

                        //employee list
                        DateTime selectedFromdate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryFromDay, 0, 0, 0, 0);

                        // string selectedTodate = "" + config.HRC_SalaryToDay + "-" + IVRM_Month_Id + "-" + Convert.ToInt32(dto.HRES_Year) + "";
                        DateTime selectedTodate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryToDay, 0, 0, 0, 0);

                        if (dto.HRESA_Id == 0)
                        {
                            totalApplied = _HRMSContext.HR_Emp_SalaryAdvance.Where(t => t.HRME_Id == dto.HRME_Id && t.MI_Id == dto.MI_Id && t.HRESA_EntryDate >= selectedFromdate && t.HRESA_EntryDate <= selectedTodate && t.HRESA_ActiveFlag == true).ToList();
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
        }


        public HR_Emp_SalaryAdvanceDTO searchfilter(HR_Emp_SalaryAdvanceDTO dto)
        {

            List<MasterEmployee> employe = new List<MasterEmployee>();

            var employees = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                             from mm in _HRMSContext.MasterEmployee
                             from med in _HRMSContext.HR_Master_EarningsDeductions
                             where mm.MI_Id.Equals(dto.MI_Id)
                             && emp.HRMED_Id == med.HRMED_Id
                             && med.HRMED_EDTypeFlag == "Advance" && emp.HREED_ActiveFlag == true
                             && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id && (mm.HRME_EmployeeFirstName.StartsWith(dto.searchfilter))
                             orderby mm.HRME_EmployeeOrder
                             select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();



            dto.employeedropdown = employees.ToArray();
            return dto;
        }
    }
}
