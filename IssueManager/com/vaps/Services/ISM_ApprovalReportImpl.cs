using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Services
{
    public class ISM_ApprovalReportImpl : Interfaces.ISM_ApprovalReportInterface
    {
        public IssueManagerContext _IssueContext;
        ILogger<ISM_PlannerReportsDTO> _logger;

        public ISM_ApprovalReportImpl(IssueManagerContext _cont, ILogger<ISM_PlannerReportsDTO> _log)
        {
            _IssueContext = _cont;
            _logger = _log;
        }
        public ISM_PlannerReportsDTO getdetails(ISM_PlannerReportsDTO data)
        {
            try
            {


                var rolelist = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).ToList();

                data.IVRMRT_Role = rolelist.FirstOrDefault().IVRMRT_Role.ToLower();

                if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    //*********************************** User Department List ********************************// 
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ADMIN_ISM_DepartmentList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt)
                        {
                            Value = 0
                        });
                        cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                        {
                            Value = data.IVRMRT_Role
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.DepartmentList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //*********************************** User Designation List ********************************// 
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ADMIN_DesignationList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                        {
                            Value = data.UserId
                        });
                        cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                        {
                            Value = data.IVRMRT_Role
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.Designation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
                    long empid = 0;
                    if (hrmEempid.Count > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                    }
                    long? dept_id = 0;
                    var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                    if (hrmeDepartment_id.Count > 0)
                    {
                        dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;

                        data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                    }

                    var hrmeDepartment = _IssueContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == dept_id && t.HRMD_ActiveFlag == true).Distinct().ToList();
                    if (hrmeDepartment.Count > 0)
                    {
                        data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                    }
                    //*********************************** User Employee Mapped List ********************************// 
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_User_EmployeeList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                        {
                            Value = data.UserId
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.get_userEmplist = retObject.ToArray();
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
            return data;
        }
        public ISM_PlannerReportsDTO getPlannerlist(ISM_PlannerReportsDTO data)
        {
            try
            {
                var rolelist = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).ToList();

                data.IVRMRT_Role = rolelist.FirstOrDefault().IVRMRT_Role.ToLower();
                List<long> Emplist = new List<long>();

                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        Emplist.Add(ue.HRME_Id);
                    }
                }


                if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    if (data.dateflag == 1)
                    {

                        data.get_Plannerlist = (from a in _IssueContext.MasterEmployee
                                                from b in _IssueContext.ISM_Task_PlannerDMO
                                                where a.HRME_Id == b.ISMTPL_PlannedBy && Emplist.Contains(b.HRME_Id) && (b.ISMTPL_StartDate >= Convert.ToDateTime(data.startdate) && b.ISMTPL_EndDate <= Convert.ToDateTime(data.enddate)) && b.ISMTPL_ActiveFlg == true
                                                select new ISM_PlannerReportsDTO
                                                {
                                                    ISMTPL_Id = b.ISMTPL_Id,
                                                    ISMTPL_PlannerName = b.ISMTPL_PlannerName,
                                                    empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) +
                                                    " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) +
                                                    " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                                    ISMTPL_StartDate = b.ISMTPL_StartDate,
                                                    ISMTPL_EndDate = b.ISMTPL_EndDate,
                                                    ISMTPL_TotalHrs = b.ISMTPL_TotalHrs

                                                }).ToArray();
                    }
                    else
                    {

                        data.get_Plannerlist = (from a in _IssueContext.MasterEmployee
                                                from b in _IssueContext.ISM_Task_PlannerDMO
                                                where a.HRME_Id == b.ISMTPL_PlannedBy && Emplist.Contains(b.HRME_Id) && b.ISMTPL_ActiveFlg == true
                                                select new ISM_PlannerReportsDTO
                                                {
                                                    ISMTPL_Id = b.ISMTPL_Id,
                                                    ISMTPL_PlannerName = b.ISMTPL_PlannerName,
                                                    empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) +
                                                    " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) +
                                                    " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                                    ISMTPL_StartDate = b.ISMTPL_StartDate,
                                                    ISMTPL_EndDate = b.ISMTPL_EndDate,
                                                    ISMTPL_TotalHrs = b.ISMTPL_TotalHrs
                                                }).ToArray();
                    }
                }
                else
                {
                    if (data.dateflag == 1)
                    {
                        data.get_Plannerlist = (from a in _IssueContext.MasterEmployee
                                                from b in _IssueContext.ISM_Task_PlannerDMO
                                                where a.HRME_Id == b.ISMTPL_PlannedBy && Emplist.Contains(b.HRME_Id) && (b.ISMTPL_StartDate >= Convert.ToDateTime(data.startdate) && b.ISMTPL_EndDate <= Convert.ToDateTime(data.enddate)) && b.ISMTPL_ActiveFlg == true
                                                select new ISM_PlannerReportsDTO
                                                {
                                                    ISMTPL_Id = b.ISMTPL_Id,
                                                    ISMTPL_PlannerName = b.ISMTPL_PlannerName,
                                                    empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) +
                                                    " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) +
                                                    " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                                    ISMTPL_StartDate = b.ISMTPL_StartDate,
                                                    ISMTPL_EndDate = b.ISMTPL_EndDate,
                                                    ISMTPL_TotalHrs = b.ISMTPL_TotalHrs
                                                }).ToArray();

                    }
                    else
                    {

                        data.get_Plannerlist = (from a in _IssueContext.MasterEmployee
                                                from b in _IssueContext.ISM_Task_PlannerDMO
                                                where a.HRME_Id == b.ISMTPL_PlannedBy && Emplist.Contains(b.HRME_Id) && b.ISMTPL_ActiveFlg == true
                                                select new ISM_PlannerReportsDTO
                                                {
                                                    ISMTPL_Id = b.ISMTPL_Id,
                                                    ISMTPL_PlannerName = b.ISMTPL_PlannerName,
                                                    empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) +
                                                    " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) +
                                                    " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                                    ISMTPL_StartDate = b.ISMTPL_StartDate,
                                                    ISMTPL_EndDate = b.ISMTPL_EndDate,
                                                    ISMTPL_TotalHrs = b.ISMTPL_TotalHrs
                                                }).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Failed";
                _logger.LogInformation("Approval report getPlannerlist : " + ex.Message);
            }
            return data;
        }
        public async Task<ISM_PlannerReportsDTO> getApprovalReport(ISM_PlannerReportsDTO data)
        {
            try
            {
                string ISMTPL_Ids = "0";
                if (data.temp_Planner.Length > 0)
                {
                    foreach (var tp in data.temp_Planner)
                    {
                        ISMTPL_Ids = ISMTPL_Ids + "," + tp.ISMTPL_Id;
                    }
                }


                string hrme_Ids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var tp in data.arrayuserEmp)
                    {
                        hrme_Ids = hrme_Ids + "," + tp.HRME_Id;
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ADMIN_ISM_Approval_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = hrme_Ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id", SqlDbType.VarChar)
                    {
                        Value = ISMTPL_Ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@APPROVAL", SqlDbType.VarChar)
                    {
                        Value = data.approvalstatus
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_approvalreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Approval Report : " + ex.Message);
            }
            return data;
        }
        public async Task<ISM_PlannerReportsDTO> Deptselectiondetails(ISM_PlannerReportsDTO data)
        {
            try
            {
                string departments = "10000";
                if (data.departmentlist.Length > 0)
                {
                    foreach (var ue in data.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar)
                    {
                        Value = departments
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Designation = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_PlannerReportsDTO> Desgselectiondetails(ISM_PlannerReportsDTO data)
        {
            try
            {
                string departments = "0";
                if (data.designationlist.Length > 0)
                {
                    foreach (var ue in data.designationlist)
                    {
                        departments = departments + "," + ue.HRMDES_Id;
                    }

                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DesignationChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar)
                    {
                        Value = departments
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_PlannerReportsDTO> getempdetailsReport(ISM_PlannerReportsDTO data)
        {
            try
            {
                var hrmEempid1 = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();

                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Fromdate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Todate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");

                string hrme_Ids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var tp in data.arrayuserEmp)
                    {
                        hrme_Ids = hrme_Ids + "," + tp.HRME_Id;
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Emp_DailyReport_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = hrme_Ids });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = hrmEempid1.FirstOrDefault().Emp_Code });

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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_emp_dailyreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_PlannerReportsDTO send_feedback(ISM_PlannerReportsDTO dto)
        {
            try
            {
                var hrmEempid1 = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == dto.MI_Id && t.Id == dto.UserId).Distinct().ToList();
                foreach (var item in dto.feedbackdtl)
                {
                    var ss = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Where(a => a.ISMDRF_Send_HRME_Id == hrmEempid1[0].Emp_Code
                    && a.ISMTCR_Id == item.ISMTCR_Id && a.ISMDRF_Feedback_DR_Date == item.ISMDRF_Feedback_DR_Date).ToList();
                    if (ss.Count == 0)
                    {
                        ISM_DailyReport_FeedBack_DMO fdm = new ISM_DailyReport_FeedBack_DMO();
                        fdm.ISMTCR_Id = item.ISMTCR_Id;
                        fdm.ISMDRF_Send_HRME_Id = hrmEempid1[0].Emp_Code;
                        fdm.ISMDRF_RCV_HRME_Id = item.ISMDRF_RCV_HRME_Id;
                        fdm.ISMDRF_FeedBack = item.ISMDRF_FeedBack;
                        fdm.ISMDRF_Feedback_DR_Date = item.ISMDRF_Feedback_DR_Date;
                        fdm.ISMDRF_Flg = false;
                        fdm.ISMDRF_OpenFlg = false;
                        fdm.ISMDRF_OpenFeedback = true;
                        fdm.CreatedDate = DateTime.Now;
                        fdm.UpdatedDate = DateTime.Now;
                        _IssueContext.Add(fdm);

                        ISM_DailyReport_Feedback_Send_DMO fs = new ISM_DailyReport_Feedback_Send_DMO();
                        fs.MI_Id = dto.MI_Id;
                        fs.ISMDRF_Id = fdm.ISMDRF_Id;
                        fs.ISMTCR_Id = item.ISMTCR_Id;
                        fs.ISMDRFS_Send_HRME_Id = hrmEempid1[0].Emp_Code;
                        fs.ISMDRFS_FeedBack = item.ISMDRF_FeedBack;
                        fs.ISMDRFS_Status = "1";
                        fs.createdate = DateTime.Now;
                        fs.updatetime = DateTime.Now;
                        _IssueContext.Add(fs);

                        ISM_DailyReport_Feedback_Receive_DMO fr = new ISM_DailyReport_Feedback_Receive_DMO();
                        fr.MI_Id = item.MI_Id_r;
                        fr.ISMDRF_Id = fdm.ISMDRF_Id;
                        fr.ISMTCR_Id = item.ISMTCR_Id;
                        fr.ISMDRFR_Receive_HRME_Id = item.ISMDRF_RCV_HRME_Id;
                        fr.ISMDRFR_FeedBack = item.ISMDRF_FeedBack;
                        fr.ISMDRFR_Status = "2";
                        fr.createdate = DateTime.Now;
                        fr.updatetime = DateTime.Now;
                        _IssueContext.Add(fr);

                        dto.send_flg = "Success";
                    }

                    else
                    {
                        ISM_DailyReport_Feedback_Send_DMO fs = new ISM_DailyReport_Feedback_Send_DMO();
                        fs.MI_Id = dto.MI_Id;
                        fs.ISMDRF_Id = ss[0].ISMDRF_Id;
                        fs.ISMTCR_Id = item.ISMTCR_Id;
                        fs.ISMDRFS_Send_HRME_Id = hrmEempid1[0].Emp_Code;
                        fs.ISMDRFS_FeedBack = item.ISMDRF_FeedBack;
                        fs.ISMDRFS_Status = "1";
                        fs.createdate = DateTime.Now;
                        fs.updatetime = DateTime.Now;
                        _IssueContext.Add(fs);

                        ISM_DailyReport_Feedback_Receive_DMO fr = new ISM_DailyReport_Feedback_Receive_DMO();
                        fr.MI_Id = item.MI_Id_r;
                        fr.ISMDRF_Id = ss[0].ISMDRF_Id;
                        fr.ISMTCR_Id = item.ISMTCR_Id;
                        fr.ISMDRFR_Receive_HRME_Id = item.ISMDRF_RCV_HRME_Id;
                        fr.ISMDRFR_FeedBack = item.ISMDRF_FeedBack;
                        fr.ISMDRFR_Status = "2";
                        fr.createdate = DateTime.Now;
                        fr.updatetime = DateTime.Now;
                        _IssueContext.Add(fr);

                        var ss12 = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Where(a => a.ISMDRF_RCV_HRME_Id == item.ISMDRF_RCV_HRME_Id
                        && a.ISMTCR_Id == item.ISMTCR_Id && a.ISMDRF_Feedback_DR_Date == item.ISMDRF_Feedback_DR_Date).ToList();

                        if (ss12.Count == 1)
                        {
                            var ss1 = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Single(a => a.ISMDRF_RCV_HRME_Id == item.ISMDRF_RCV_HRME_Id
                            && a.ISMTCR_Id == item.ISMTCR_Id && a.ISMDRF_Feedback_DR_Date == item.ISMDRF_Feedback_DR_Date);
                            ss1.ISMDRF_OpenFlg = false;
                            ss1.ISMDRF_FeedBack = item.ISMDRF_FeedBack;
                            ss1.CreatedDate = DateTime.Now;
                            ss1.UpdatedDate = DateTime.Now;
                            _IssueContext.Update(ss1);
                        }
                        else
                        {
                            var ss4 = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Where(a => a.ISMDRF_RCV_HRME_Id == item.ISMDRF_RCV_HRME_Id
                            && a.ISMTCR_Id == item.ISMTCR_Id && a.ISMDRF_Feedback_DR_Date == item.ISMDRF_Feedback_DR_Date).LastOrDefault();
                            var ss5 = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Single(a => a.ISMDRF_RCV_HRME_Id == ss4.ISMDRF_RCV_HRME_Id
                            && a.ISMTCR_Id == ss4.ISMTCR_Id && a.ISMDRF_Feedback_DR_Date == ss4.ISMDRF_Feedback_DR_Date && a.ISMDRF_Id == ss4.ISMDRF_Id);

                            ss5.ISMDRF_OpenFlg = false;
                            ss5.ISMDRF_FeedBack = item.ISMDRF_FeedBack;
                            ss5.CreatedDate = DateTime.Now;
                            ss5.UpdatedDate = DateTime.Now;
                            _IssueContext.Update(ss5);
                        }
                        dto.send_flg = "Success";
                    }
                    var i = _IssueContext.SaveChanges();
                    if (i > 0)
                    {
                        dto.send_flg = "Success";
                    }
                    else
                    {
                        dto.send_flg = "Failed";
                    }

                    var deviceidsnew = "";
                    var devicenew = "";
                    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                    var getemployeedetails = (from a in _IssueContext.MasterEmployee
                                              from b in _IssueContext.HR_Master_Department
                                              from c in _IssueContext.HR_Master_Designation

                                              from e in _IssueContext.HR_Master_Grade
                                              where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                                              && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == dto.MI_Id && a.HRME_Id == item.ISMDRF_RCV_HRME_Id)
                                              select new ISM_DailyReportGenerationDTO
                                              {
                                                  employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                  + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                  + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                  department = b.HRMD_DepartmentName,
                                                  desgination = c.HRMDES_DesignationName,
                                                  gradename = e.HRMG_GradeName,
                                                  HRMD_Id = a.HRMD_Id
                                              }).Distinct().ToList();

                    var devicelist = (from a in _IssueContext.MasterEmployee
                                      where (a.HRME_ActiveFlag == true && a.HRME_Id == item.ISMDRF_RCV_HRME_Id)
                                      select new ISM_PlannerReportsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                          HRME_MobileNo = a.HRME_MobileNo
                                      }).Distinct().ToList();
                    dto.deviceArray = devicelist.ToArray();

                    if (devicelist.Count > 0)
                    {
                        int k = 0;
                        foreach (var deviceid in devicelist)
                        {
                            if (k == 0)
                            {
                                deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                k = 1;
                            }
                            else
                            {
                                deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                            }
                        }
                        devicenew = "[" + deviceidsnew + "]";
                    }
                    try
                    {
                        if (dto.sound == "")
                        {
                            dto.sound = "default";
                        }
                        callnotification(devicenew, item.ISMDRF_RCV_HRME_Id, indiantime0, getemployeedetails.FirstOrDefault().employeename, dto.MI_Id, dto.sound, item.ISMTCR_Id);
                        Insertnotification(dto.MI_Id, item.ISMDRF_RCV_HRME_Id, indiantime0, getemployeedetails.FirstOrDefault().employeename, dto.UserId);

                        Insert_PushNotification_interaction(dto.MI_Id, item.ISMDRF_RCV_HRME_Id, item.ISMDRF_FeedBack, item.ISMTCR_Id, devicenew, dto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.send_flg = "Failed";
            }
            return dto;
        }
        public async Task<ISM_PlannerReportsDTO> getPlannerDReport(ISM_PlannerReportsDTO data)
        {
            try
            {
                string ISMTPL_Ids = "0";
                if (data.temp_Planner.Length > 0)
                {
                    foreach (var tp in data.temp_Planner)
                    {
                        ISMTPL_Ids = ISMTPL_Ids + "," + tp.ISMTPL_Id;
                    }
                }

                string hrme_Ids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var tp in data.arrayuserEmp)
                    {
                        hrme_Ids = hrme_Ids + "," + tp.HRME_Id;
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PLANNER_CREATION_TASKDEATILS_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = hrme_Ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id", SqlDbType.VarChar)
                    {
                        Value = ISMTPL_Ids
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannerdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Approval Report : " + ex.Message);
            }
            return data;
        }
        public ISM_PlannerReportsDTO viewresponse(ISM_PlannerReportsDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_TaskDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMTCR_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_taskdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_TaskResponse";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMTCR_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_taskresponse = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_TaskAttachements";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMTCR_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_taskattachments = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_ResponseAttachements";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMTCR_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_responseattachments = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Approval Report : " + ex.Message);
            }
            return data;
        }

        public ISM_PlannerReportsDTO SummaryReport(ISM_PlannerReportsDTO data)
        {
            try
            {
            
              


                string hrme_Ids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var tp in data.arrayuserEmp)
                    {
                        hrme_Ids = hrme_Ids + "," + tp.HRME_Id;
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DR_notsend_reject_LOP_Summary";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = hrme_Ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = data.Fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = data.Todate
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Rejected_SummaryReport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Approval Report : " + ex.Message);
            }
            return data;
        }
        public string callnotification(string devicenew, long empid, DateTime? ISMDRPT_Date, string empname, long mi_id, string sound, long ISMTCR_Id)
        {
            try
            {
                ISM_DailyReportGenerationDTO data = new ISM_DailyReportGenerationDTO();
                var dailyreport = _IssueContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == mi_id && a.HRME_Id == empid
                && a.ISMDRPT_Date == ISMDRPT_Date).Distinct().ToList();
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Daily Report " + ISMDRPT_Date + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";


                notificationparams.Add(daata.ToString());
                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";
                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string Insertnotification(long MI_Id, long empid, DateTime? ISMDRPT_Date, string empname, long userid)
        {
            try
            {
                var notificationdetails = (from UEM in _IssueContext.ISM_User_Employees_MappingDMO
                                           from HRE in _IssueContext.MasterEmployee
                                           from HMD in _IssueContext.HR_Master_Department
                                           where (UEM.HRME_Id == HRE.HRME_Id && HRE.HRME_ActiveFlag == true && HRE.HRME_LeftFlag == false && HRE.HRME_ActiveFlag == true && HRE.HRMD_Id == HMD.HRMD_Id && HMD.HRMD_ActiveFlag == true && UEM.User_Id == userid)
                                           select new ISM_DailyReportGenerationDTO
                                           {
                                               HRME_Id = UEM.HRME_Id,
                                           }).Distinct().ToList();


                var notification = "Daily Report of " + empname + "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ISM_DailyReportGenerationDTO data = new ISM_DailyReportGenerationDTO();

                if (notificationdetails.Count > 0)
                {
                    List<long> employeeids = new List<long>();
                    foreach (var n in notificationdetails)
                    {
                        ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                        push.MI_Id = MI_Id;
                        push.HRME_Id = empid;
                        push.ISMNO_Notification = notification;
                        push.ISMNO_NoticationDate = indianTime;
                        push.ISMNO_NotificationType = "FEEDBACK";
                        push.ISMNO_ReadFlg = false;
                        push.ISMNO_MakeUnReadFlg = false;
                        push.ISMNO_ActiveFlag = true;
                        push.ISMNO_CreatedBy = userid;
                        push.ISMNO_UpdatedBy = userid;
                        push.CreatedDate = indianTime;
                        push.UpdatedDate = indianTime;
                        _IssueContext.Add(push);

                        var contactExists = _IssueContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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

        public string Insert_PushNotification_interaction(long MI_Id, long HRME_Id, string ISMDRF_FeedBack, long ISMTCR_Id, string device, ISM_PlannerReportsDTO dto)
        {
            try
            {
                string title = "";
                title = "Staff";
                var headername = _IssueContext.ISM_TaskCreationDMO.Where(a => a.ISMTCR_Id == ISMTCR_Id && a.MI_Id == MI_Id).ToList();


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = MI_Id;
                sdd.PNSD_HeaderName = headername[0].ISMTCR_Title;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = title;
                sdd.PNSD_Header_Flg = title;
                sdd.PNSD_SMSMessage = ISMDRF_FeedBack;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _IssueContext.Add(sdd);


                PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                ss.PNSD_Id = sdd.PNSD_Id;
                ss.PNSDDE_MobileNo = dto.HRME_MobileNo;
                ss.PNSDDE_DeviceId = dto.HRME_AppDownloadedDeviceId;
                ss.PNSDDE_ReadStatus = "1";
                ss.PNSDDE_DeliveryDate = indianTime;
                ss.PNSDDE_DeliveryTime = time;
                ss.PNSDDE_MakeUnreadFlg = false;
                ss.PNSDDE_ApprovalLevel = "0";
                _IssueContext.Add(ss);


                PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                sds.PNSD_Id = sdd.PNSD_Id;
                sds.HRME_Id = dto.HRME_Id;
                _IssueContext.Add(sds);

                _IssueContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }

        public ISM_PlannerReportsDTO getcatempdetails(ISM_PlannerReportsDTO data)
        {
            try
            {

            
                    //To get con call category Employee List
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Employee_Concall_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.get_userEmplist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Concall_LOPGridDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
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
                            data.griddata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }


        public async Task<ISM_PlannerReportsDTO> getempcatedetails(ISM_PlannerReportsDTO data)
        {
            try
            {
                var hrmEempid1 = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();

                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Fromdate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Todate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");

                string hrme_Ids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var tp in data.arrayuserEmp)
                    {
                        hrme_Ids = hrme_Ids + "," + tp.HRME_Id;
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Concall_Employee_Task_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = hrme_Ids });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar) { Value = contodate });
                    //cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = hrmEempid1.FirstOrDefault().Emp_Code });

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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_emp_dailyreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_PlannerReportsDTO saveAddLop(ISM_PlannerReportsDTO dto)
        {
            try
            {
                if (dto.AdditionLOPDetails.Length > 0)
                {
                    foreach (var x in dto.AdditionLOPDetails)
                    {
                       
                        Lop_DetailsDMO CaData = new Lop_DetailsDMO();
                        CaData.MI_Id = dto.MI_Id;
                        CaData.HRME_Id = x.HRME_Id;
                        CaData.HRALD_Type = "ConCall Lop";
                        CaData.HRALD_Remarks ="Con call not attended";
                        CaData.HRALD_Date = DateTime.Now;
                        CaData.Createddate = DateTime.Now;
                        CaData.Updateddate = DateTime.Now;
                        CaData.Createdby = dto.UserId;
                        CaData.Updatedby = dto.UserId;
                        CaData.ISMTCR_Id = x.ISMTCR_Id;
                        CaData.HRALD_Activeflag = true;

                        _IssueContext.Add(CaData);
                    }
                    var res = _IssueContext.SaveChanges();

                    if (res >= 1)
                    {
                        dto.message = "saved";
                    }
                    else
                    {
                        dto.message = "failed";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_PlannerReportsDTO deactivate(ISM_PlannerReportsDTO dto)
        {
            try
            {
                var Concalltype = "";
                Concalltype = "CON CALL";
                //var CurrentDate = DateTime.Now;
                var Drdate = dto.ISMDRPT_Date;

                var hrme_id = _IssueContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();
                dto.HRME_Id = hrme_id.FirstOrDefault().Emp_Code;
                //&& t.Updatedby == dto.UserId && t.Createdby == dto.UserId
                if (dto.reporttype=="No")
                {
                    var result = _IssueContext.Lop_DetailsDMO.Where(t => t.HRME_Id == dto.HRME_Id && t.HRALD_Type == Concalltype && t.HRALD_Date.Date == dto.ISMDRPT_Date.Value.Date && t.Updatedby == dto.UserId && t.Createdby == dto.UserId).FirstOrDefault();
                    if (result != null)
                    {
                          result.Updatedby = dto.UserId;
                          result.Updateddate = Drdate.Value;
                          result.ISMTCR_Id = 0;
                        _IssueContext.Update(result);   
                           
                        var res = _IssueContext.SaveChanges();

                        if (res >= 1)
                        {
                            dto.message = "saved";
                        }
                        else
                        {
                            dto.message = "failed";
                        }

                    }
                    else
                    {
                        Lop_DetailsDMO CaData = new Lop_DetailsDMO();
                        CaData.MI_Id = dto.MI_Id;
                        CaData.HRME_Id = dto.HRME_Id;
                        CaData.HRALD_Type = Concalltype;
                        CaData.HRALD_Remarks = "Con Call LOP";
                        CaData.HRALD_Date = Drdate.Value;
                        CaData.Createddate = Drdate.Value;
                        CaData.Updateddate = Drdate.Value;
                        CaData.Createdby = dto.UserId;
                        CaData.Updatedby = dto.UserId;
                        CaData.ISMTCR_Id = 1;
                        CaData.HRALD_Activeflag = true;

                        _IssueContext.Add(CaData);
                        var res = _IssueContext.SaveChanges();

                        if (res >= 1)
                        {
                            dto.message = "saved";
                        }
                        else
                        {
                            dto.message = "failed";
                        }
                    }

                }
                else
                {
                    dto.message = "failed";
                }

                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }
        public ISM_PlannerReportsDTO getonlodatdata(ISM_PlannerReportsDTO data)
        {
            try
            {



                data.Masteryear = _IssueContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true)
                                    .GroupBy(t => t.HRMLY_LeaveYear)
                                    .Select(g => g.First())
                                    .ToArray();

                data.Monthnames = _IssueContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ISM_PlannerReportsDTO getconcalllopdetails(ISM_PlannerReportsDTO dto)
        {
            try
            {
                DateTime fromdate1 = Convert.ToDateTime(dto.Fromdate);
                  var fromdate11 = fromdate1.ToString("yyyy-MM-dd");
                DateTime todate1 = Convert.ToDateTime(dto.Todate);
                var todate = todate1.ToString("yyyy-MM-dd");

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_CONCALL_LOP_EMPLOYEE_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = dto.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = fromdate11 });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = dto.conflag });
                    cmd.Parameters.Add(new SqlParameter("@MONTHNAME", SqlDbType.VarChar) { Value = dto.monthname });
                    cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.VarChar) { Value = dto.year });

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
                        dto.griddata = retObject.ToArray();
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