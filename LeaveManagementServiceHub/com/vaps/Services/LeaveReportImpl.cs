using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;


namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveReportImpl : LeaveReportInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveReportImpl> _log;
        public LMContext _lmContext;
        public LeaveReportImpl(LMContext ttcategory)
        {
            _lmContext = ttcategory;
        }


        public LeaveCreditDTO getleavereport(LeaveCreditDTO data)
        {
            List<Institution> Institution_List = new List<Institution>();
            Institution_List = _lmContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToList();
            data.Institution_List = Institution_List.Distinct().ToArray();

            List<HR_Master_GroupType_DMO> staf_types = new List<HR_Master_GroupType_DMO>();
            staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.HRMGT_ActiveFlag == true).ToList();
            data.stf_types = staf_types.Distinct().ToArray();

            List<HR_Master_Department_DMO> Department_types = new List<HR_Master_Department_DMO>();
            Department_types = _lmContext.HR_Master_Department_DMO.Where(t => t.HRMD_ActiveFlag == true).ToList();
            data.Department_types = Department_types.Distinct().ToArray();


            List<HR_Master_Designation_DMO> Designation_types = new List<HR_Master_Designation_DMO>();
            Designation_types = _lmContext.HR_Master_Designation_DMO.Where(t => t.HRMDES_ActiveFlag == true).ToList();
            data.Designation_types = Designation_types.Distinct().ToArray();

            List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
            leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
            data.leave_name = leave_name.Distinct().ToArray();

            List<IVRM_Month_DMO> credit_month = new List<IVRM_Month_DMO>();
            credit_month = _lmContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToList();
            data.credit_month = credit_month.Distinct().ToArray();

            List<HR_Master_LeaveYear_DMO> get_year = new List<HR_Master_LeaveYear_DMO>();
            get_year = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).ToList();
            data.get_year = get_year.Distinct().OrderByDescending(a => a.HRMLY_LeaveYearOrder).ToArray();

            return data;
        }

        //Implementation for Searchable Institution List Dropdown
        public LeaveCreditDTO get_stafftypes(LeaveCreditDTO data)
        {
            try
            {
                data.Staff_List = (from a in _lmContext.HR_Master_GroupType_DMO
                                   where (a.MI_Id == data.Master_Id)
                                   select new LeaveCreditDTO
                                   {
                                       HRMGT_Id = a.HRMGT_Id,
                                       HRMGT_EmployeeGroupType = a.HRMGT_EmployeeGroupType,
                                   }

                             ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        public LeaveCreditDTO get_departments(LeaveCreditDTO data)
        {
            List<long> selected_emp_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_emp_types.Add(itm.HRMGT_Id);
            }

            data.Department_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                     from b in _lmContext.HR_Master_Department_DMO
                                     where (a.HRMD_Id == b.HRMD_Id && b.HRMD_ActiveFlag == true && selected_emp_types.Contains(a.HRMGT_Id))
                                     select new LeaveCreditDTO
                                     {
                                         HRMD_Id = b.HRMD_Id,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName,
                                     }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO get_designation(LeaveCreditDTO data)
        {
            List<long?> selected_desg_types = new List<long?>();

            foreach (var itm in data.emptypes)
            {
                selected_desg_types.Add(itm.HRMD_Id);
            }

            data.Designation_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                      from b in _lmContext.HR_Master_Designation_DMO
                                      where (a.HRMDES_Id == b.HRMDES_Id && b.HRMDES_ActiveFlag == true && a.MI_Id == b.MI_Id && selected_desg_types.Contains(a.HRMD_Id))
                                      select new LeaveCreditDTO
                                      {
                                          HRMDES_Id = b.HRMDES_Id,
                                          HRMDES_DesignationName = b.HRMDES_DesignationName,
                                      }).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO get_Employees(LeaveCreditDTO data)
        {

            if (data.emptypes.Length > 0)
            {
                List<long> selected_emp = new List<long>();

                foreach (var itm in data.emptypes)
                {
                    selected_emp.Add(itm.HRMDES_Id);
                }

                data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
                                from b in _lmContext.HR_Master_Designation_DMO
                                from c in _lmContext.HR_Master_Department_DMO
                                where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                 && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && selected_emp.Contains(b.HRMDES_Id))
                                select new LeaveTransactionManualDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRMDES_Id = a.HRMDES_Id,
                                    HRMD_Id = a.HRMD_Id,
                                    HRMGT_Id = a.HRMGT_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                                }).Distinct().ToArray();
            }
            else
            {
                data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
                                where (a.HRME_ActiveFlag == true)
                                select new LeaveTransactionManualDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                                }).Distinct().ToArray();
            }


            return data;
        }
        public async Task<LeaveCreditDTO> get_report(LeaveCreditDTO data)
        {
            List<long> MI_Id_list = new List<long>();
            List<long> leave_list = new List<long>();
            List<long> emp_list = new List<long>();
            List<Institution> get_institution = new List<Institution>();
            string employeelist = "0";

            data.HRML_LeaveCode = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_Id == data.HRML_Id).Select(t => t.HRML_LeaveCode).FirstOrDefault();
            MI_Id_list = _lmContext.Institution.Where(t => t.MI_ActiveFlag == 1).Select(t => t.MI_Id).ToList();
            data.MI_Id_liststring = string.Join(",", MI_Id_list);
            employeelist = string.Join(",", data.selectedreport);


            if (data.HRML_Id == 999)
            {
                leave_list = (from a in _lmContext.HR_Master_Leave_DMO
                              from b in _lmContext.Institution
                              where (a.MI_Id == b.MI_Id && b.MI_ActiveFlag == 1)
                              select a.HRML_Id).ToList();
                data.selectedLeave = string.Join(",", leave_list);
            }
            else
            {
                leave_list = (from a in _lmContext.HR_Master_Leave_DMO
                              from b in _lmContext.Institution
                              where (a.MI_Id == b.MI_Id && b.MI_ActiveFlag == 1 && a.HRML_LeaveCode == data.HRML_LeaveCode)
                              select a.HRML_Id).ToList();
                data.selectedLeave = string.Join(",", leave_list);
            }

            try
            {
                if (data.selectedreport != null)
                {
                    if (data.checkbox_value == false)
                    {
                        if (data.HRML_LeaveCode == "OD")
                        {
                            data.activityIds = (from a in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                from b in _lmContext.HR_Master_Employee_DMO
                                                where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.HRELAP_ApplicationStatus == "Approved" && data.selectedreport.Contains(b.HRME_Id) && a.HRELAP_FromDate >= data.HRELTD_FromDate && a.HRELAP_ToDate <= data.HRELT_ToDate)
                                                select new LeaveCreditDTO
                                                {
                                                    HRME_Id = a.HRME_Id,
                                                    HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                                    HRELAP_FromDate = a.HRELAP_FromDate,
                                                    HRELAP_ToDate = a.HRELAP_ToDate,
                                                    HRELAP_ApplicationStatus = a.HRELAP_ApplicationStatus,
                                                    HRME_EmployeeCode = b.HRME_EmployeeCode
                                                }).ToArray();
                        }
                        else
                        {
                            List<LeaveCreditDTO> result = new List<LeaveCreditDTO>();
                            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Employees_Bal_Leaves";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                                {
                                    Value = data.MI_Id_liststring
                                });
                                cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.Date)
                                {
                                    Value = data.HRELTD_FromDate
                                });
                                cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date)
                                {
                                    Value = data.HRELT_ToDate
                                });
                                cmd.Parameters.Add(new SqlParameter("@Leaveid", SqlDbType.VarChar)
                                {
                                    Value = data.selectedLeave.ToString()
                                });
                                cmd.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.VarChar)
                                {
                                    Value = employeelist
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = await cmd.ExecuteReaderAsync())
                                    {
                                        while (await dataReader.ReadAsync())
                                        {
                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                            {
                                                dataRow.Add(
                                                    dataReader.GetName(iFiled),
                                                    dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    data.activityIds = retObject.ToArray();
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
                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "FO_AllEmpsLeaveTransaction_Details";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = data.MI_Id_liststring
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Ids", SqlDbType.VarChar)
                            {
                                Value = employeelist
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.report_details = retObject.ToArray();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("error");
                _log.LogDebug(ex.Message);
            }

            return data;
        }

        // Implementation added to fetch data for leave authorization details of employees

        public async Task<LeaveCreditDTO> get_authorization_report(LeaveCreditDTO data)
        {
            //string employeelist = string.Join(",", data.selectedreport);

            string HRMGT_Id = "0";
            string HRMD_Id = "0";
            string HRMDES_Id = "0";
            List<long> mid = new List<long>();

            if (data.authorizationreport.Length > 0)
            {
                //foreach (var item in data.authorizationreport)
                //{
                //    mid.Add(item.MI_Id);
                //}

                for (int i = 0; i < data.authorizationreport.Length; i++)
                {
                    HRMGT_Id = HRMGT_Id + "," + data.authorizationreport[i].HRMGT_Id.ToString();
                    HRMD_Id = HRMD_Id + "," + data.authorizationreport[i].HRMD_Id.ToString();
                    HRMDES_Id = HRMDES_Id + "," + data.authorizationreport[i].HRMDES_Id.ToString();
                }
            }


            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_Dept_wise_LeaveApprover";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HRMGT_Id", SqlDbType.VarChar)
                {
                    Value = HRMGT_Id
                });
                cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                {
                    Value = HRMD_Id
                });
                cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                {
                    Value = HRMDES_Id
                });
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.authorizationreport_details = retObject.ToArray();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            //{
            //    cmd.CommandText = "HR_EmployeeLeaveAuthorizationLevels";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
            //    {
            //        Value = employeelist
            //    });
            //    if (cmd.Connection.State != ConnectionState.Open)
            //        cmd.Connection.Open();

            //    var retObject = new List<dynamic>();
            //    try
            //    {
            //        using (var dataReader = await cmd.ExecuteReaderAsync())
            //        {
            //            while (await dataReader.ReadAsync())
            //            {
            //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                {
            //                    dataRow.Add(
            //                        dataReader.GetName(iFiled),
            //                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
            //                    );
            //                }
            //                retObject.Add((ExpandoObject)dataRow);
            //            }
            //        }
            //        data.authorizationreport_details = retObject.ToArray();

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
            return data;
        }
    }
}