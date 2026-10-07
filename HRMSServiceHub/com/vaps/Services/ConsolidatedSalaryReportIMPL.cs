using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class ConsolidatedSalaryReportIMPL : Interfaces.ConsolidatedSalaryReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public ConsolidatedSalaryReportIMPL(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public ConsolidatedSalaryReportDTO getBasicData(ConsolidatedSalaryReportDTO dto)
        {
            dto.hrmeid = (from a in _HRMSContext.applicationuser
                          from b in _HRMSContext.Staff_User_Login
                          where (a.Id == b.Id && a.Id == dto.userId)
                          select b).Select(t => t.Emp_Code).FirstOrDefault();


            dto.fillyear = (from a in _Context.HR_Master_LeaveYearDMO
                            where (a.MI_Id == dto.MI_Id && a.HRMLY_ActiveFlag == true)
                            select new ConsolidatedSalaryReportDTO
                            {
                                HRMLY_Id = a.HRMLY_Id,
                                HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                            }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

            dto.fillmonth = (from a in _Context.month
                             select new ConsolidatedSalaryReportDTO
                             {
                                 IVRM_Month_Id = a.IVRM_Month_Id,
                                 monthname = a.IVRM_Month_Name
                             }).Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();


           // dto.empnames = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == dto.hrmeid).Distinct().ToArray();

            dto.empnames = (from a in _HRMSContext.MasterEmployee
                            from b in _HRMSContext.HR_Master_Designation
                            from c in _HRMSContext.HR_Master_Department
                            where (a.HRME_Id == dto.hrmeid && a.HRMDES_Id == b.HRMDES_Id &&a.HRMD_Id == c.HRMD_Id)
                            select new ConsolidatedSalaryReportDTO
                            {
                                HRME_Id = a.HRME_Id,
                                EmplYoeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                HRME_EmployeeCode = a.HRME_EmployeeFirstName,
                                HRME_LocStreet = a.HRME_LocStreet,
                                HRME_DOJ = a.HRME_DOJ,
                                HRME_MobileNo = a.HRME_MobileNo,
                                HRME_EmailId = a.HRME_EmailId,
                                HRMDES_DesignationName = b.HRMDES_DesignationName,
                                HRMD_DepartmentName=c.HRMD_DepartmentName,
                                HRME_Photo = a.HRME_Photo
                            }).Distinct().ToArray();

            //dto.empnames = (from a in _HRMSContext.HR_Master_DepartmentCode_Head
            //                    from c in _HRMSContext.HR_Master_Department
            //                    from d in _HRMSContext.MasterEmployee
            //                where (a.HRME_ID == dto.hrmeid && a.HRMDC_ID == c.HRMDC_ID && c.HRMD_Id == d.HRMD_Id && d.HRME_ActiveFlag == true && d.HRME_LeftFlag == false)
            //                    select new ConsolidatedSalaryReportDTO
            //                    {
            //                        HRME_Id = d.HRME_Id,
            //                        EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName)
            //                    }).Distinct().ToArray();
            return dto;
        }




        public ConsolidatedSalaryReportDTO salaryreport(ConsolidatedSalaryReportDTO dto)
        {

            try
            {
                string startdate = dto.yearid + "-" + dto.month + "-01";
                dto.Fromdate1 = Convert.ToDateTime(startdate);
                var year = Convert.ToDateTime(startdate).Year;
                var mon = Convert.ToDateTime(startdate).Month;
                var startOfMonth = new DateTime(year, mon, 1);
                var DaysInMonth = DateTime.DaysInMonth(year, mon);
                dto.Todate1 = new DateTime(year, mon, DaysInMonth);
                if (dto.Fromdate1 != null && dto.Todate1 != null)
                {
                    dto.fromdate = Convert.ToDateTime(dto.Fromdate1).ToString("yyyy-MM-dd");
                    dto.todate = Convert.ToDateTime(dto.Todate1).ToString("yyyy-MM-dd");
                }

                dto.hrmeid = (from a in _HRMSContext.applicationuser
                              from b in _HRMSContext.Staff_User_Login
                              where (a.Id == b.Id && a.Id == dto.userId)
                              select b).Select(t => t.Emp_Code).FirstOrDefault();

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Emps_Monthly_Salary_Report_VMS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",
                     SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
                       SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Multiplehrmeid",
                   SqlDbType.NVarChar)
                    {
                        Value = dto.hrmeid
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                        dto.EmployeesalaryDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeMonthlySalaryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = dto.hrmeid
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_LeaveYear",
                  SqlDbType.BigInt)
                    {
                        Value = dto.yearid
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Name",
                  SqlDbType.VarChar)
                    {
                        Value = dto.month
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeeSalaryslipDetails = retObject.ToArray();


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


    }
}
