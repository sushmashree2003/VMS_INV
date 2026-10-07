using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveOveallReportIMP : LeaveOveallReportInerface

    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveOveallReportIMP> _log;
        public LMContext _lmContext;
        public LeaveOveallReportIMP(LMContext ttcategory)
        {
            _lmContext = ttcategory;
        }
        public LeaveOverallReportDTO getleavereport(LeaveOverallReportDTO data)
        {
            try
            {
                List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
                leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.leave_name = leave_name.Distinct().ToArray();
                data.institutionlist = _lmContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
                //data.get_emp = _lmContext.HR_Master_Employee_DMO.Where(p => p.HRME_ActiveFlag == true).Select(d =>d.HRME_Id).Distinct().ToArray();

                data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO

                                where (a.HRME_ActiveFlag == true
                                 )

                                select new LeaveTransactionManualDTO
                                {


                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                                }
                       ).Distinct().ToArray();

             

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                Console.WriteLine(ex.Message);
            }
           

            return data;

        }


        public LeaveOverallReportDTO getleavetype(LeaveOverallReportDTO data)
        {
            try
            {
                List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
                leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.leave_name = leave_name.Distinct().ToArray();
                data.getyear = (from a in _lmContext.HR_Master_LeaveYear_DMO
                                where (a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                select new LeaveOverallReportDTO
                                {
                                    HRMLY_Id = Convert.ToInt32(a.HRMLY_Id),
                                    HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                    HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                }).Distinct().OrderBy(tt => tt.HRMLY_LeaveYearOrder).ToArray();
                data.getmonth = _lmContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                Console.WriteLine(ex.Message);
            }


            return data;

        }


        public LeaveOverallReportDTO get_report(LeaveOverallReportDTO data)
        {
            try
            {
              
                List<long> leave_list = new List<long>();                
               

                if (data.HRML_Id == 999)                    
                {
                    Array remove_details = _lmContext.HR_Master_Leave_DMO.Select(a =>a.HRML_Id).ToArray();
                    foreach (long d in remove_details)
                    {
                        leave_list.Add(d);
                    }
                }
                else
                {
                    leave_list.Add(data.HRML_Id);
                }

                if (data.Radioflag == "Employeewise") {
                    if (data.HRME_Id == 0)
                    {

                        data.getReport = (from a in _lmContext.HR_Master_Employee_DMO
                                          from b in _lmContext.HR_Emp_Leave_Trans_DMO
                                          from c in _lmContext.HR_Master_Leave_DMO
                                          from d in _lmContext.HR_Master_Department_DMO
                                          where (a.HRME_Id == b.HRME_Id && b.HRELT_LeaveId == c.HRML_Id && a.HRMD_Id == d.HRMD_Id && leave_list.Contains(c.HRML_Id) && b.HRELT_ActiveFlag == true && b.HRELT_Status == data.Rdotype &&
                                          (b.HRELT_FromDate >= data.FROM_DATE && b.HRELT_ToDate <= data.TO_DATE))
                                          select new LeaveOverallReportDTO
                                          {
                                              HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                              HRELAP_FromDate = b.HRELT_FromDate,
                                              HRELAP_ToDate = b.HRELT_ToDate,
                                              HRELAP_ApplicationStatus = b.HRELT_Status,
                                              HRML_LeaveName = c.HRML_LeaveName,
                                              HRMD_DepartmentName = d.HRMD_DepartmentName,
                                              HRELT_TotDays=b.HRELT_TotDays
                                          }).Distinct().ToArray();
                    }
                    else
                    {
                        if (data.HRME_Id > 0)
                        {
                            data.getReport = (from a in _lmContext.HR_Master_Employee_DMO
                                              from b in _lmContext.HR_Emp_Leave_Trans_DMO
                                              from c in _lmContext.HR_Master_Leave_DMO
                                              from d in _lmContext.HR_Master_Department_DMO
                                              where (a.HRME_Id == b.HRME_Id && b.HRELT_LeaveId == c.HRML_Id && b.HRME_Id == data.HRME_Id && a.HRMD_Id == d.HRMD_Id && b.HRELT_ActiveFlag == true
                                              && b.HRELT_Status == data.Rdotype && (b.HRELT_FromDate >= data.FROM_DATE && b.HRELT_ToDate <= data.TO_DATE))
                                              select new LeaveOverallReportDTO
                                              {

                                                  HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                                  HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                  HRELAP_FromDate = b.HRELT_FromDate,
                                                  HRELAP_ToDate = b.HRELT_ToDate,
                                                  HRELAP_ApplicationStatus = b.HRELT_Status,
                                                  HRML_LeaveName = c.HRML_LeaveName,
                                                  HRMD_DepartmentName = d.HRMD_DepartmentName


                                              }).Distinct().ToArray();

                        }
                    }
                }
                else
                {
                    data.getReport = (from a in _lmContext.HR_Master_Employee_DMO
                                      from b in _lmContext.HR_Emp_Leave_Trans_DMO
                                      from c in _lmContext.HR_Master_Leave_DMO
                                      from d in _lmContext.HR_Master_Department_DMO
                                      where (a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id  && a.HRME_Id == b.HRME_Id && b.HRELT_LeaveId == c.HRML_Id && a.HRMD_Id == d.HRMD_Id && leave_list.Contains(c.HRML_Id) && b.HRELT_ActiveFlag == true && b.HRELT_Status == data.Rdotype && b.MI_Id == data.Institute_Id &&
                                      (b.HRELT_FromDate >= data.FROM_DATE && b.HRELT_ToDate <= data.TO_DATE))
                                      select new LeaveOverallReportDTO
                                      {
                                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          HRELAP_FromDate = b.HRELT_FromDate,
                                          HRELAP_ToDate = b.HRELT_ToDate,
                                          HRELAP_ApplicationStatus = b.HRELT_Status,
                                          HRML_LeaveName = c.HRML_LeaveName,
                                          HRMD_DepartmentName = d.HRMD_DepartmentName
                                      }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        ////////MONTHENDREPORT////////////////////////////////////////////////////////////////////////////////

        public LeaveOverallReportDTO getmonthendreport(LeaveOverallReportDTO data)
        {
            try
            {               
                List<long> leave_list = new List<long>();


                if (data.HRML_Id == 999)
                {
                    Array remove_details = _lmContext.HR_Master_Leave_DMO.Select(a => a.HRML_Id).ToArray();
                    foreach (long d in remove_details)
                    {
                        leave_list.Add(d);
                    }
                }
                else
                {
                    leave_list.Add(data.HRML_Id);
                }

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Empleaves_Month_End_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@LeaveId", SqlDbType.BigInt)
                    {
                        Value = data.HRML_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar)
                    {
                        Value = data.Rdotype
                    });
                    cmd.Parameters.Add(new SqlParameter("@month_Id", SqlDbType.VarChar)
                    {
                        Value = data.IVRM_Month_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@year_id", SqlDbType.VarChar)
                    {
                        Value = data.year
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
                        data.getmonthReport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                //data.getmonthReport = (from b in _lmContext.HR_Emp_Leave_Trans_DMO
                //                           from c in _lmContext.HR_Master_Leave_DMO
                //                           from d in _lmContext.HR_Master_Department_DMO
                //                           from e in _lmContext.IVRM_Month_DMO
                //                           from f in _lmContext.HR_Master_LeaveYear_DMO
                //                           from g in _lmContext.Institution
                //                           where (g.MI_Id== data.Institute_Id && b.MI_Id == data.Institute_Id && b.MI_Id == f.MI_Id && b.MI_Id == c.MI_Id && b.HRELT_LeaveId == c.HRML_Id && d.MI_Id == b.MI_Id && leave_list.Contains(c.HRML_Id) && b.HRELT_ActiveFlag == true && b.HRELT_Status == data.Rdotype
                //                   && f.HRMLY_Id == data.HRMLY_Id && b.HRMLY_Id == f.HRMLY_Id && e.IVRM_Month_Id == data.IVRM_Month_Id)
                //                           select new LeaveOverallReportDTO
                //                           {
                //                               HRMLY_Id = b.HRMLY_Id,
                //                               IVRM_Month_Id = e.IVRM_Month_Id,
                //                               IVRM_Month_Name = e.IVRM_Month_Name,
                //                               MI_Name=g.MI_Name,
                //                               HRMLY_LeaveYear = f.HRMLY_LeaveYear,
                //                               HRELAP_ApplicationStatus = b.HRELT_Status,
                //                               HRML_LeaveName = c.HRML_LeaveName,
                //                               HRELT_TotDays = b.HRELT_TotDays,
                //                               HRMD_DepartmentName = d.HRMD_DepartmentName
                //                           }).Distinct().ToArray();
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
