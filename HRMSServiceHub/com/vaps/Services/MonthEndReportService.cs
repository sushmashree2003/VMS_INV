using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class MonthEndReportService : Interfaces.MonthEndReportInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MonthEndReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;
        }

        public MonthEndReportDTO getBasicData(MonthEndReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public MonthEndReportDTO GetAllDropdownAndDatatableDetails(MonthEndReportDTO dto)
        {
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            try
            {
                //leave year
                dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).ToArray();
                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();

                GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                dto.groupTypedropdown = GroupTypelist.ToArray();

                //Departmentlist
                Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                dto.departmentdropdown = Departmentlist.ToArray();

                //Designationlist
                Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                dto.designationdropdown = Designationlist.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public async Task<MonthEndReportDTO> getEmployeedetailsBySelection(MonthEndReportDTO dto)
        {
            List<HR_Employee_Salary> employeeSalaryDetails = new List<HR_Employee_Salary>();
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            try
            {
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                dto.HRES_Month = _Context.month.Where(t => t.IVRM_Month_Id == dto.ivrM_Month_Id).FirstOrDefault().IVRM_Month_Name;

                dto.empid = _HRMSContext.MasterEmployee.Where(t => dto.groupTypeIdList.Contains(t.HRMGT_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && t.MI_Id == dto.MI_Id && t.HRME_ActiveFlag == true).Select(t => t.HRME_Id).ToArray();

                dto.empIds = String.Join(",", dto.empid);

                dto.employeeDetails = (from a in _HRMSContext.MasterEmployee
                                       from b in _HRMSContext.HR_Master_Department
                                       from c in _HRMSContext.HR_Master_Designation
                                       from d in _HRMSContext.IVRM_Master_Gender
                                       where (a.MI_Id == dto.MI_Id && dto.empid.Contains(a.HRME_Id) && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.IVRMMG_Id == d.IVRMMG_Id)
                                       select new MonthEndReportDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "":a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "":a.HRME_EmployeeLastName),
                                           HRME_FatherName = a.HRME_FatherName,
                                           IVRMMG_Id = a.IVRMMG_Id,
                                           IVRMMG_GenderName = d.IVRMMG_GenderName,
                                           HRMD_Id = a.HRMD_Id,
                                           HRMD_DepartmentName = b.HRMD_DepartmentName,
                                           HRMDES_Id = a.HRMDES_Id,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                                           HRME_DOJ = a.HRME_DOJ,
                                           HRME_ESIAccNo = a.HRME_ESIAccNo,
                                           HRME_PFAccNo = a.HRME_PFAccNo
                                       }).ToArray();

                try
                {
                    string startdate = dto.hreS_Year + "-" + dto.HRES_Month + "-01";
                    int monthend = _Context.month.Where(t => t.IVRM_Month_Id == dto.ivrM_Month_Id).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault();
                    string enddate = dto.hreS_Year + "-" + dto.HRES_Month + "-" + monthend;
                    dto.Fromdate = Convert.ToDateTime(startdate);
                    dto.Todate = Convert.ToDateTime(enddate);

                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_Emp_Monthly_yearly_Report_New";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@fromdate",SqlDbType.VarChar)
                        {
                            Value = dto.Fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",SqlDbType.VarChar)
                        {
                            Value = dto.Todate
                        });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",SqlDbType.NVarChar)
                        {
                            Value = dto.empIds
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid",SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "monthly"
                        });
                        cmd.Parameters.Add(new SqlParameter("@cols",SqlDbType.NVarChar, 2000)
                        {
                            Direction = ParameterDirection.Output
                        });
                        cmd.Parameters.Add(new SqlParameter("@totalpresent",SqlDbType.VarChar, 10)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            var retObject11 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {
                                        );
                                    }
                                    retObject11.Add((ExpandoObject)dataRow1);
                                }
                            }
                            dto.employeeAttendance = retObject11.ToArray();
                            dto.columnnames = cmd.Parameters["@cols"].Value.ToString();
                            dto.totalworkingdays = cmd.Parameters["@totalpresent"].Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    dto.earningheadlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_EarnDedFlag == "Earning" && t.HRMED_ActiveFlag == true).ToArray();

                    dto.earningheadvalues = (from a in _HRMSContext.HR_Master_EarningsDeductions
                                             from b in _HRMSContext.HR_Employee_EarningsDeductions
                                             where (a.HRMED_Id == b.HRMED_Id && a.MI_Id == dto.MI_Id && a.HRMED_ActiveFlag == true && dto.empid.Contains(b.HRME_Id))
                                             select new MonthEndReportDTO
                                             {
                                                 HRMED_Id = a.HRMED_Id,
                                                 HRME_Id = b.HRME_Id,
                                                 HREED_Amount = b.HREED_Amount
                                             }).ToArray();

                    dto.deductionheadlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_EarnDedFlag == "Deduction" && t.HRMED_ActiveFlag == true).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }
        public MonthEndReportDTO getalldetailsQForm(MonthEndReportDTO data)
        {
            data.employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();

            return data;
        }
        public MonthEndReportDTO getEmployeedetailsQForm(MonthEndReportDTO data)
        {
            data.employeeDetailssingle = (from a in _HRMSContext.MasterEmployee
                                          from b in _HRMSContext.HR_Master_Designation
                                          from d in _HRMSContext.Institution
                                          where (a.HRME_Id == data.HRME_Id && a.HRMDES_Id == b.HRMDES_Id && a.MI_Id == d.MI_Id)
                                          select new MasterEmployeeDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRMDES_Id = a.HRMDES_Id,
                                              HRMDES_DesignationName = b.HRMDES_DesignationName,
                                              HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                              HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                              HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                              HRME_PerStreet = a.HRME_PerStreet,
                                              HRME_PerArea = a.HRME_PerArea,
                                              HRME_PerCity = a.HRME_PerCity,
                                              HRME_PerAdd4 = a.HRME_PerAdd4,
                                              HRME_PerStateId = a.HRME_PerStateId,
                                              HRME_PerPincode = a.HRME_PerPincode,
                                              HRME_LocStreet = a.HRME_LocStreet,
                                              HRME_LocArea = a.HRME_LocArea,
                                              HRME_LocCity = a.HRME_LocCity,
                                              HRME_LocAdd4 = a.HRME_LocAdd4,
                                              HRME_LocPincode = a.HRME_PerPincode,
                                              HRME_FatherName = a.HRME_FatherName,
                                              HRME_DOB = a.HRME_DOB,
                                              HRME_DOJ = a.HRME_DOJ
                                          }).ToArray();

            data.basicamount = (from a in _HRMSContext.MasterEmployee
                                from b in _HRMSContext.HR_Master_EarningsDeductions
                                from c in _HRMSContext.HR_Employee_EarningsDeductions
                                where (a.HRME_Id == c.HRME_Id && a.HRME_Id == data.HRME_Id && b.HRMED_EDTypeFlag == "Basic Pay" && b.HRMED_Id == c.HRMED_Id && a.MI_Id == b.MI_Id && b.HRMED_ActiveFlag == true)
                                select c.HREED_Amount).FirstOrDefault();

            data.allowanceamount = (from a in _HRMSContext.MasterEmployee
                                from b in _HRMSContext.HR_Master_EarningsDeductions
                                from c in _HRMSContext.HR_Employee_EarningsDeductions
                                where (a.HRME_Id == c.HRME_Id && a.HRME_Id == data.HRME_Id && b.HRMED_EDTypeFlag == "Local Conveyance" && b.HRMED_Id == c.HRMED_Id && a.MI_Id == b.MI_Id && b.HRMED_ActiveFlag == true)
                                select c.HREED_Amount).FirstOrDefault();

            data.hraamount = (from a in _HRMSContext.MasterEmployee
                                from b in _HRMSContext.HR_Master_EarningsDeductions
                                from c in _HRMSContext.HR_Employee_EarningsDeductions
                                where (a.HRME_Id == c.HRME_Id && a.HRME_Id == data.HRME_Id && b.HRMED_EDTypeFlag == "HRA" && b.HRMED_Id == c.HRMED_Id && a.MI_Id == b.MI_Id && b.HRMED_ActiveFlag == true)
                                select c.HREED_Amount).FirstOrDefault();

            var employeedetails = _HRMSContext.MasterEmployee.Where(a => a.HRME_Id == data.HRME_Id).ToList();


            data.GetEmpinstitutiondetails = _HRMSContext.Institution.Where(a => a.MI_Id == employeedetails.FirstOrDefault().MI_Id).ToArray();


            return data;
        }
        public MonthEndReportDTO SearchUdetails (MonthEndReportDTO data)
        {
            try
            {
                data.companydetails = _HRMSContext.Institution.Where(t => t.MI_Id == data.MI_Id && t.MI_ActiveFlag == 1).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MonthEndReportDTO SearchFReport(MonthEndReportDTO data)
        {
            try
            {
                data.employeeDetailssingle = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == data.HRME_Id).ToArray();

                data.leavedetails = (from a in _HRMSContext.HR_Master_Leave
                                     from b in _HRMSContext.HR_MasterLeaveYear
                                     from c in _HRMSContext.HR_Emp_Leave_StatusDMO
                                     from d in _HRMSContext.MasterEmployee
                                     where (a.MI_Id == d.MI_Id && d.HRME_Id == data.HRME_Id && b.MI_Id == a.MI_Id && b.HRMLY_ActiveFlag == true && c.HRME_Id == d.HRME_Id && c.HRMLY_Id == b.HRMLY_Id)
                                     select new MonthEndReportDTO
                                     {
                                         HRML_LeaveName = a.HRML_LeaveName,
                                         HRMLY_LeaveYear = b.HRMLY_LeaveYear,
                                         HRELS_OBLeaves = c.HRELS_OBLeaves,
                                         HRELS_CreditedLeaves = c.HRELS_CreditedLeaves
                                     }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}