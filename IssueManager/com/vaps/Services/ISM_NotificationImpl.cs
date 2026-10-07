using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
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

namespace IssueManager.com.vaps.Services
{
    public class ISM_NotificationImpl : Interfaces.ISM_NotificationInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_NotificationImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_NotificationDTO> getdetails(ISM_NotificationDTO data)
        {
            try
            {
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Notification_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.BigInt)
                    {
                        Value = empid
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
                        data.get_notifications = retObject.ToArray();
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
        public async Task<ISM_NotificationDTO> getplannernotgenerated(ISM_NotificationDTO data)
        {
            try
            {
                ISM_NotificationDTO dates = new ISM_NotificationDTO();
                dates = startenddays(data);

                //System.Globalization.CultureInfo ci =
                //  System.Threading.Thread.CurrentThread.CurrentCulture;
                //DayOfWeek firstdayofweek = ci.DateTimeFormat.FirstDayOfWeek;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                DayOfWeek today = indiantime0.DayOfWeek;

                //DateTime weekstartdate = DateTime.Now.AddDays(-(today - firstdayofweek)).Date;
                //DateTime weekenddate = weekstartdate.AddDays(6).Date;
                data.startdate = dates.startdate;
                data.enddate = dates.enddate;

                if (dates.message == today.ToString())
                {
                    //var Departmentheadlist = _Context.HR_Master_DepartmentCodeDMO.ToList();
                    List<ISM_NotificationDTO> totalapprovalemp = new List<ISM_NotificationDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_GetPlannerApproval_Employees";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                      SqlDbType.BigInt)
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
                                    totalapprovalemp.Add(new ISM_NotificationDTO
                                    {
                                        employeename = Convert.ToString(dataReader["employeename"]),
                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                        UserId = Convert.ToInt64(dataReader["Id"]),
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (totalapprovalemp.Count() > 0)
                    {
                        foreach (var hrmdc in totalapprovalemp)
                        {
                            int rownumber = 0;
                            string str = "";
                            string str1 = "";
                            string str2 = "";

                            List<ISM_NotificationDTO> empdevlist = new List<ISM_NotificationDTO>();
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_PlannerNotgenerated_New";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@StartDate",
                              SqlDbType.DateTime)
                                {
                                    Value = data.startdate
                                });
                                cmd.Parameters.Add(new SqlParameter("@EndDate",
                                SqlDbType.DateTime)
                                {
                                    Value = data.enddate
                                });
                                cmd.Parameters.Add(new SqlParameter("@userid",
                        SqlDbType.VarChar)
                                {
                                    Value = hrmdc.UserId
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
                                            empdevlist.Add(new ISM_NotificationDTO
                                            {
                                                employeename = Convert.ToString(dataReader["employeename"]),
                                                empcode = Convert.ToString(dataReader["empcode"]),
                                                department = Convert.ToString(dataReader["designation"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            if (empdevlist.Count > 0)
                            {
                                str += "<p> <span style=" + "'font-weight:bold'" + ">Employees who not generated planner:</span></p> <table width='100%' style=" + "'border-collapse: collapse; width: 100%;'" + " class=" + "'table1'" + "><tr>" +
                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                         "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMPLOYEE NAME </th>" +
                                          "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMP CODE </th>" +
                                          "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DESIGNATION </th>" +
                                         "</tr>";
                                for (int j = 0; j < empdevlist.Count; j++)
                                {
                                    rownumber += 1;
                                    str1 += "<tr> " +
                                   "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                   "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].employeename + "</td>" +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].empcode + "</td>" +
                                     "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].department + "</td>" +
                                   "</tr>";
                                }
                                str2 = "</table>";
                            }
                            data.message = str + str1 + str2;

                            if (empdevlist.Count > 0)
                            {
                                var Employee = (from a in _IssueContext.MasterEmployee
                                                from b in _Context.Multiple_Email_DMO
                                                where a.HRME_Id == b.HRME_Id && a.HRME_Id == hrmdc.HRME_Id
                                                select new ISM_PlannerReportsDTO
                                                {
                                                    HRMEM_EmailId = b.HRMEM_EmailId,
                                                    HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                    MI_Id = a.MI_Id,
                                                    employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                                    gender = a.IVRMMG_Id
                                                }).ToList();
                                if (Employee.Count() > 0)
                                {
                                    var Empgender = _Context.IVRM_Master_Gender.Where(t => t.IVRMMG_Id == Employee.FirstOrDefault().gender).ToList();

                                    sendmail(17, Employee.FirstOrDefault().employeename, Employee.FirstOrDefault().HRMEM_EmailId, "PlannerNotGenerated", data.message, data, Empgender.FirstOrDefault().IVRMMG_GenderName);
                                }
                            }
                            //}
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
        public async Task<ISM_NotificationDTO> getplannernotapproved(ISM_NotificationDTO data)
        {
            try
            {
                ISM_NotificationDTO dates = new ISM_NotificationDTO();
                dates = startenddays(data);

                //System.Globalization.CultureInfo ci =
                //  System.Threading.Thread.CurrentThread.CurrentCulture;
                //DayOfWeek firstdayofweek = ci.DateTimeFormat.FirstDayOfWeek;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                DayOfWeek today = indiantime0.DayOfWeek;

                //DateTime weekstartdate = DateTime.Now.AddDays(-(today - firstdayofweek)).Date;
                //DateTime weekenddate = weekstartdate.AddDays(6).Date;
                data.startdate = dates.startdate;
                data.enddate = dates.enddate;

                if (!today.ToString().Equals("Sunday", StringComparison.OrdinalIgnoreCase))
                {
                    List<ISM_NotificationDTO> totalapprovalemp = new List<ISM_NotificationDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_GetPlannerApproval_Employees";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                      SqlDbType.BigInt)
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
                                    totalapprovalemp.Add(new ISM_NotificationDTO
                                    {
                                        employeename = Convert.ToString(dataReader["employeename"]),
                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                        UserId = Convert.ToInt64(dataReader["Id"]),
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (totalapprovalemp.Count() > 0)
                    {
                        foreach (var hrmdc in totalapprovalemp)
                        {
                            int rownumber = 0;
                            string str = "";
                            string str1 = "";
                            string str2 = "";

                            List<ISM_NotificationDTO> empdevlist = new List<ISM_NotificationDTO>();
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_PlannerNotApproved_New";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@StartDate",
                              SqlDbType.DateTime)
                                {
                                    Value = data.startdate
                                });
                                cmd.Parameters.Add(new SqlParameter("@EndDate",
                                SqlDbType.DateTime)
                                {
                                    Value = data.enddate
                                });
                                cmd.Parameters.Add(new SqlParameter("@userid",
                        SqlDbType.VarChar)
                                {
                                    Value = hrmdc.UserId
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
                                            empdevlist.Add(new ISM_NotificationDTO
                                            {
                                                employeename = Convert.ToString(dataReader["employeename"]),
                                                empcode = Convert.ToString(dataReader["empcode"]),
                                                department = Convert.ToString(dataReader["designation"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            if (empdevlist.Count > 0)
                            {
                                str += "<p> <span style=" + "'font-weight:bold'" + ">Employees whose planner not approved yet :</span></p> <table width='100%' style=" + "'border-collapse: collapse; width: 100%;'" + " class=" + "'table1'" + "><tr>" +
                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                         "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMPLOYEE NAME </th>" +
                                          "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMP CODE </th>" +
                                          "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DESIGNATION </th>" +
                                         "</tr>";
                                for (int j = 0; j < empdevlist.Count; j++)
                                {
                                    rownumber += 1;
                                    str1 += "<tr> " +
                                   "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                   "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].employeename + "</td>" +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].empcode + "</td>" +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].department + "</td>" +
                                   "</tr>";

                                }
                                str2 = "</table>";
                            }
                            data.message = str + str1 + str2;

                            if (empdevlist.Count > 0)
                            {
                                var Employee = (from a in _IssueContext.MasterEmployee
                                                from b in _Context.Multiple_Email_DMO
                                                where a.HRME_Id == b.HRME_Id && a.HRME_Id == hrmdc.HRME_Id
                                                select new ISM_PlannerReportsDTO
                                                {
                                                    HRMEM_EmailId = b.HRMEM_EmailId,
                                                    HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                    MI_Id = a.MI_Id,
                                                    employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                                    gender = a.IVRMMG_Id
                                                }).ToList();
                                if (Employee.Count() > 0)
                                {
                                    var Empgender = _Context.IVRM_Master_Gender.Where(t => t.IVRMMG_Id == Employee.FirstOrDefault().gender).ToList();

                                    sendmail(17, Employee.FirstOrDefault().employeename, Employee.FirstOrDefault().HRMEM_EmailId, "PlannerNotApproved", data.message, data, Empgender.FirstOrDefault().IVRMMG_GenderName);
                                }
                            }
                            //}
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
        public async Task<ISM_NotificationDTO> getplannernotapprovedmd(ISM_NotificationDTO data)
        {
            try
            {
                ISM_NotificationDTO dates = new ISM_NotificationDTO();
                dates = startenddays(data);

                //System.Globalization.CultureInfo ci =
                //  System.Threading.Thread.CurrentThread.CurrentCulture;
                //DayOfWeek firstdayofweek = ci.DateTimeFormat.FirstDayOfWeek;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                DayOfWeek today = indiantime0.DayOfWeek;

                //DateTime weekstartdate = DateTime.Now.AddDays(-(today - firstdayofweek)).Date;
                //DateTime weekenddate = weekstartdate.AddDays(6).Date;
                data.startdate = dates.startdate;
                data.enddate = dates.enddate;

                //if (dates.message == today.ToString())
                if (!today.ToString().Equals("Sunday", StringComparison.OrdinalIgnoreCase))
                {
                    //var Departmentheadlist = _Context.HR_Master_DepartmentCodeDMO.ToList();

                    //if (Departmentheadlist.Count() > 0)
                    //{
                    //    foreach (var hrmdc in Departmentheadlist)
                    //    {
                    int rownumber = 0;
                    string str = "";
                    string str1 = "";
                    string str2 = "";

                    List<ISM_NotificationDTO> empdevlist = new List<ISM_NotificationDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_PlannerNotApprovedMD";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@StartDate",
                      SqlDbType.DateTime)
                        {
                            Value = dates.startdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                        SqlDbType.DateTime)
                        {
                            Value = data.enddate
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
                                    empdevlist.Add(new ISM_NotificationDTO
                                    {
                                        employeename = Convert.ToString(dataReader["employeename"]),
                                        empcode = Convert.ToString(dataReader["empcode"]),
                                        department = Convert.ToString(dataReader["department"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (empdevlist.Count > 0)
                    {
                        str += "<p> <span style=" + "'font-weight:bold'" + ">Employees whose planner not approved yet :</span></p> <table width='100%' style=" + "'border-collapse: collapse; width: 100%;'" + " class=" + "'table1'" + "><tr>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMPLOYEE NAME </th>" +
                                  "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMP CODE </th>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DEPARTMENT </th>" +
                                 "</tr>";
                        for (int j = 0; j < empdevlist.Count; j++)
                        {
                            rownumber += 1;
                            str1 += "<tr> " +
                           "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                           "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].employeename + "</td>" +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].empcode + "</td>" +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].department + "</td>" +
                           "</tr>";

                        }
                        str2 = "</table>";
                    }

                    data.message = str + str1 + str2;

                    if (empdevlist.Count > 0)
                    {
                        sendmailmdteam(17, "PlannerNotApprovedMD", data.message, data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_NotificationDTO> getdailyreportnotapproved(ISM_NotificationDTO data)
        {
            try
            {
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Notification_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.BigInt)
                    {
                        Value = empid
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
                        data.get_notifications = retObject.ToArray();
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
        public async Task<ISM_NotificationDTO> getnotificationdetails(ISM_NotificationDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_NotificationDTO> pushnotificationdetails(ISM_NotificationDTO data)
        {
            try
            {
                var inserttocken = _IssueContext.Database.ExecuteSqlCommand("Insert_Pushnotification_Tocken @p0,@p1,@p2", data.pushtocken, data.UserId, data.MI_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_NotificationDTO> emialnotificationfordailyreportblock(ISM_NotificationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                string taskdetails = "";
                //var Departmentheadlist = _Context.HR_Master_DepartmentCodeDMO.ToList();
                List<ISM_NotificationDTO> blockeddailyreports = new List<ISM_NotificationDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_GetTodayBlocked_Dailyreport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.BigInt)
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
                                blockeddailyreports.Add(new ISM_NotificationDTO
                                {
                                    ISMBE_Id = Convert.ToInt64(dataReader["ISMBE_Id"]),
                                    ISMEMN_ID = Convert.ToInt64(dataReader["ISMEMN_ID"]),
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                    ISMBE_Reason = Convert.ToString(dataReader["ISMBE_Reason"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (blockeddailyreports.Count() > 0)
                {
                    foreach (var block in blockeddailyreports)
                    {
                        data.ISMEMN_ID = block.ISMEMN_ID;
                        data.ISMBE_Id = block.ISMBE_Id;
                        data.HRME_Id = block.HRME_Id;
                        data.ISMBE_Reason = block.ISMBE_Reason;

                        if (block.ISMEMN_ID != null && block.ISMEMN_ID != 0)
                        {
                            var memonotice = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.ISMEMN_ID == block.ISMEMN_ID).ToList();
                            if (memonotice.Count > 0)
                            {
                                data.ISMBE_Reason = memonotice.FirstOrDefault().ISMEMN_No;
                            }
                        }
                        taskdetails = gettaskregisterReportnotice(data);

                        var Employee = (from a in _IssueContext.MasterEmployee
                                        from b in _Context.Multiple_Email_DMO
                                        where a.HRME_Id == b.HRME_Id && a.HRME_Id == block.HRME_Id
                                        select new ISM_PlannerReportsDTO
                                        {
                                            HRMEM_EmailId = b.HRMEM_EmailId,
                                            HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                            MI_Id = a.MI_Id,
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                            gender = a.IVRMMG_Id
                                        }).ToList();
                        if (Employee.Count() > 0)
                        {
                            var Empgender = _Context.IVRM_Master_Gender.Where(t => t.IVRMMG_Id == Employee.FirstOrDefault().gender).ToList();

                            sendmail(17, Employee.FirstOrDefault().employeename, Employee.FirstOrDefault().HRMEM_EmailId, "DAILYREPORTBLOCK", data.message, data, Empgender.FirstOrDefault().IVRMMG_GenderName);
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
        public string sendmail(long MI_Id, string empname, string Email, string Template, string taskdetails, ISM_NotificationDTO data, string gender)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";

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

                if (gender.Equals("MALE", StringComparison.OrdinalIgnoreCase))
                {
                    val.Add("[NAME]", "Mr. " + empname);
                }
                else if ((gender.Equals("FEMALE", StringComparison.OrdinalIgnoreCase)))
                {
                    val.Add("[NAME]", "Ms. " + empname);
                }

                val.Add("[MSG]", taskdetails);
                val.Add("[SUBJECT]", data.ISMBE_Reason);
                //val.Add("[DESIGNATION]", department.ToUpper());
                val.Add("[STARTDATE]", Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy"));
                val.Add("[ENDDATE]", Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy"));

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
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = "";
                    if (Template == "DAILYREPORTBLOCK")
                    {
                        Subject = template[0].ISES_MailSubject.ToString();
                    }
                    else
                    {
                        Subject = template[0].ISES_MailSubject.ToString() + " - " + Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy");
                    }

                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

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

                    try
                    {
                        List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                        using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                        {
                            if (Template == "DAILYREPORTBLOCK")
                            {
                                cmd.CommandText = "Fetch_DailyreportBlockccmails";
                            }
                            else
                            {
                                cmd.CommandText = "Fetch_Plannerccmails";
                            }

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.HRME_Id
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
                                        dtonewtemp.Add(new ISM_PlannerReportsDTO
                                        {
                                            MailCc = Convert.ToString(dataReader["cc"]),
                                            MailBCc = Convert.ToString(dataReader["bcc"])
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        if (dtonewtemp.Count > 0)
                        {
                            for (int j = 0; j < dtonewtemp.Count; j++)
                            {
                                mailcc = dtonewtemp[j].MailCc;
                                mailbcc = dtonewtemp[j].MailBCc;
                            }
                            if (mailcc != null && mailcc != "")
                            {
                                string[] ccmaildetails = mailcc.Split(',');

                                foreach (var c in ccmaildetails)
                                {
                                    if (c != Email)
                                    {
                                        message.AddCc(c);
                                    }
                                }
                            }
                            if (mailbcc != null && mailbcc != "")
                            {
                                string[] bccmaildetails = mailbcc.Split(',');

                                foreach (var c in bccmaildetails)
                                {
                                    if (c != Email)
                                    {
                                        message.AddBcc(c);
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception error)
                    {
                        //
                    }

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        if (modulename.Count == 0)
                        {
                            modulename[0] = "Issue Manager";
                        }

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
                            Value = template + " - " + data.employeename
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
        public string sendmailmdteam(long MI_Id, string Template, string taskdetails, ISM_NotificationDTO data)
        {
            try
            {
                string tomail = "";
                string mailcc = "";
                string mailbcc = "";

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

                val.Add("[MSG]", taskdetails);

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
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString() + " - " + Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy");
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_PSTOMDMails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.BigInt)
                        {
                            Value = 1
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
                                    dtonewtemp.Add(new ISM_PlannerReportsDTO
                                    {
                                        Tomail = Convert.ToString(dataReader["to"]),
                                        MailCc = Convert.ToString(dataReader["cc"]),
                                        MailBCc = Convert.ToString(dataReader["bcc"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (dtonewtemp.Count > 0)
                    {
                        //message.AddTo(Email);

                        for (int j = 0; j < dtonewtemp.Count; j++)
                        {
                            tomail = dtonewtemp[j].Tomail;
                            mailcc = dtonewtemp[j].MailCc;
                            mailbcc = dtonewtemp[j].MailBCc;
                        }
                        if (tomail != null && tomail != "")
                        {
                            string[] tomaildetails = tomail.Split(',');

                            foreach (var c in tomaildetails)
                            {
                                message.AddTo(c);
                            }
                        }
                        if (mailcc != null && mailcc != "" && tomail != "")
                        {
                            string[] ccmaildetails = mailcc.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }
                        if (mailbcc != null && mailbcc != "" && tomail != "")
                        {
                            string[] bccmaildetails = mailbcc.Split(',');

                            foreach (var c in bccmaildetails)
                            {
                                message.AddBcc(c);
                            }
                        }
                    }

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        if (modulename.Count == 0)
                        {
                            modulename[0] = "Issue Manager";
                        }

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = "pstomd@vapstech.com"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = "Planner Not Approved - " + data.employeename
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
        public ISM_NotificationDTO startenddays(ISM_NotificationDTO data)
        {
            try
            {
                string day = "";
                List<ISM_NotificationDTO> empdevlist = new List<ISM_NotificationDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "START_END_DAY_OF_WEEK";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
            SqlDbType.VarChar)
                    {
                        Value = 1
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
                                empdevlist.Add(new ISM_NotificationDTO
                                {
                                    startdate = Convert.ToDateTime(dataReader["START_OF_WEEK"]),
                                    enddate = Convert.ToDateTime(dataReader["END_OF_WEEK"]),
                                    message = Convert.ToString(dataReader["START_DAY_OF_WEEK"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (empdevlist.Count() > 0)
                {
                    for (int j = 0; j < empdevlist.Count; j++)
                    {
                        data.startdate = empdevlist[j].startdate;
                        data.enddate = empdevlist[j].enddate;
                        data.message = empdevlist[j].message;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string gettaskregisterReportnotice(ISM_NotificationDTO data)
        {
            try
            {
                int rownumber = 0;
                string str = "";
                string str1 = "";
                string str2 = "";
                List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_BLOCK_DAILYREPORT_NOTIFICATION";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_ID",
                    SqlDbType.BigInt)
                    {
                        Value = data.ISMEMN_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMBE_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ISMBE_Id
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
                                dtonewtemp.Add(new ISM_PlannerReportsDTO
                                {
                                    ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                    ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                    ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                    ISMTCR_Status = Convert.ToString(dataReader["ISMTCR_Status"]),
                                    AssignedBy = Convert.ToString(dataReader["AssignedBy"]),
                                    taskstartdate = Convert.ToString(dataReader["StartDate"]),
                                    taskatenddate = Convert.ToString(dataReader["EndDate"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (dtonewtemp.Count > 0)
                {
                    //Deviation: " + data.deviation + " %</ span > < span style = " + "'float: center;padding-left:5%;'" + " > Total Open Tasks Tilldate: " + dtonewtemp.Count + " </ span ></ span ></ p >
                    var stdate = Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy");
                    var eddate = Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy");
                    str += "<p> <span style=" + "'font-weight:bold'" + ">Pending Issue Lists :<span style=" + "'float: center;padding-left:10%;'" + "></span> </span></p> <table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK NO.</th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK NAME</th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK TYPE </th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STATUS </th>" +
                            "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ASSIGNEDBY </th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STARTDATE </th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ENDDATE </th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > View </th>" +
                             "</tr>";
                    for (int j = 0; j < dtonewtemp.Count; j++)
                    {
                        rownumber += 1;
                        str1 += "<tr> " +
                            "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + dtonewtemp[j].ISMTCR_TaskNo + "</td>" +
                            "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + dtonewtemp[j].ISMTCR_Title + "</td>" +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].ISMTCR_BugOREnhancementFlg + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].ISMTCR_Status + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].AssignedBy + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].taskstartdate + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].taskatenddate + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + "https://vapsmanagementsoftware.azurewebsites.net/#/app/ISM_ComplaintsTaskList/729" + "</td> " +
                             "</tr>";
                    }
                    str2 = "</table>";

                }
                data.message = str + str1 + str2;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.message;
        }
        public async Task<ISM_NotificationDTO> noticeProbition(ISM_NotificationDTO data)
        {   //added
            try
            {

                long hrmEempid = _Context.Staff_User_Login.Where(t =>t.Id == data.UserId).Select(p => p.Emp_Code).FirstOrDefault();

                string Template = "";
                long HRME_ID = 0;
                string email = "";

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_probationperiod_15days_left";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_notifications = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    string emailid = _IssueContext.Multiple_Email_DMO.Where(p => p.HRME_Id == hrmEempid && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                    if (data.get_notifications.Length > 0)
                    {
                        sendEmailprobi(16, "Probition", hrmEempid, emailid);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_NotificationDTO> unauthorizednotice(ISM_NotificationDTO data)
        {
            try
            {
                string taskdetail = "";
                List<ISM_NotificationDTO> totalapprovalemp = new List<ISM_NotificationDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Daily_UnotherisedLeave";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                                totalapprovalemp.Add(new ISM_NotificationDTO
                                {
                                    EmployeeName = Convert.ToString(dataReader["EmployeeName"]),
                                    HRME_EmployeeCode = Convert.ToString(dataReader["HRME_EmployeeCode"]),
                                    MI_Name = Convert.ToString(dataReader["MI_Name"]),
                                    HRMDES_DesignationName = Convert.ToString(dataReader["HRMDES_DesignationName"]),
                                    FOEP_PunchDate = Convert.ToDateTime(dataReader["FOEP_PunchDate"]),
                                    MI_Logo = Convert.ToString(dataReader["MI_Logo"]),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                if (totalapprovalemp.Count() > 0)
                {
                    foreach (var block in totalapprovalemp)
                    {
                        data.EmployeeName = block.EmployeeName;
                        data.HRME_EmployeeCode = block.HRME_EmployeeCode;
                        data.MI_Name = block.MI_Name;
                        data.HRMDES_DesignationName = block.HRMDES_DesignationName;
                        data.FOEP_PunchDate = block.FOEP_PunchDate;
                        data.MI_Logo = block.MI_Logo;
                        //if (block.EmployeeName != null && block.EmployeeName != "")
                        //{
                        //    var memonotice = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.ISMEMN_ID == block.ISMEMN_ID).ToList();
                        //    if (memonotice.Count > 0)
                        //    {
                        //        data.ISMBE_Reason = memonotice.FirstOrDefault().ISMEMN_No;
                        //    }
                        //}
                        //taskdetail = gettaskregisterReportnotice(data);

                        var Employee = (from a in _IssueContext.MasterEmployee
                                        from b in _Context.Multiple_Email_DMO
                                        where a.HRME_Id == b.HRME_Id && a.HRME_EmployeeCode == block.HRME_EmployeeCode && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                        select new ISM_PlannerReportsDTO
                                        {
                                            HRMEM_EmailId = b.HRMEM_EmailId,
                                            MI_Id = a.MI_Id,
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                            gender = a.IVRMMG_Id
                                        }).ToList();
                        if (Employee.Count() > 0)
                        {
                            var Empgender = _Context.IVRM_Master_Gender.Where(t => t.IVRMMG_Id == Employee.FirstOrDefault().gender).ToList();

                            sendmailun(16, Employee.FirstOrDefault().employeename, Employee.FirstOrDefault().HRMEM_EmailId, "Unauthorizedleave", data.message, data, Empgender.FirstOrDefault().IVRMMG_GenderName);
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

        public async Task<ISM_NotificationDTO> lateinearlyoutnotice(ISM_NotificationDTO data)
        {
            try
            {
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
     
                List<ISM_NotificationDTO> totallateemp = new List<ISM_NotificationDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_penaltyRelaxtime";
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
                                totallateemp.Add(new ISM_NotificationDTO
                                {
                                    EmployeeName = Convert.ToString(dataReader["EmployeeName"]),
                                    HRME_EmployeeCode = Convert.ToString(dataReader["HRME_EmployeeCode"]),
                                    MI_Name = Convert.ToString(dataReader["MI_Name"]),
                                    HRMDES_DesignationName = Convert.ToString(dataReader["HRMDES_DesignationName"]),
                                    FOEP_PunchDate = Convert.ToDateTime(dataReader["FOEP_PunchDate"]),
                                    HRMERT_Enddate = Convert.ToDateTime(dataReader["HRMERT_Enddate"]),
                                    MI_Logo = Convert.ToString(dataReader["MI_Logo"]),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                if (totallateemp.Count() > 0)
                {
                    foreach (var block in totallateemp)
                    {
                        data.EmployeeName = block.EmployeeName;
                        data.HRME_EmployeeCode = block.HRME_EmployeeCode;
                        data.MI_Name = block.MI_Name;
                        data.HRMDES_DesignationName = block.HRMDES_DesignationName;
                        data.FOEP_PunchDate = block.FOEP_PunchDate;
                        data.HRMERT_Enddate = block.HRMERT_Enddate;
                        data.MI_Logo = block.MI_Logo;                 

                        var Employee = (from a in _IssueContext.MasterEmployee
                                        from b in _Context.Multiple_Email_DMO
                                        where a.HRME_Id == b.HRME_Id && a.HRME_EmployeeCode == block.HRME_EmployeeCode && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                        select new ISM_PlannerReportsDTO
                                        {
                                            HRMEM_EmailId = b.HRMEM_EmailId,
                                            MI_Id = a.MI_Id,
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                            gender = a.IVRMMG_Id
                                        }).ToList();
                        if (Employee.Count() > 0)
                        {
                            var Empgender = _Context.IVRM_Master_Gender.Where(t => t.IVRMMG_Id == Employee.FirstOrDefault().gender).ToList();

                            sendmaillateinearly(16, Employee.FirstOrDefault().employeename, Employee.FirstOrDefault().HRMEM_EmailId, "lateinearlyout", data.message, data, Empgender.FirstOrDefault().IVRMMG_GenderName);
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

        public string sendmailun(long MI_Id, string empname, string Email, string Template, string taskdetail, ISM_NotificationDTO data, string gender)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";

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

                if (gender.Equals("MALE", StringComparison.OrdinalIgnoreCase))
                {
                    val.Add("[NAME]", "Mr. " + empname);
                }
                else if ((gender.Equals("FEMALE", StringComparison.OrdinalIgnoreCase)))
                {
                    val.Add("[NAME]", "Ms. " + empname);
                }

                //val.Add("[MSG]", taskdetail);
                val.Add("[EMPLOYEENAME]", data.EmployeeName);
                val.Add("[EMPDESIGNATION]", data.HRMDES_DesignationName.ToUpper());
                val.Add("[DATE]", Convert.ToDateTime(data.FOEP_PunchDate).ToString("dd/MM/yyyy"));
                val.Add("[DATE1]", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"));
                val.Add("[Company]", data.MI_Name);
                // val.Add("[LOGO]", data.MI_Logo);
                val.Add("[LOGO]", "<img src=\"" + data.MI_Logo + "\" alt=\"Logo\">");

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
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = "";
                    if (Template == "Unauthorizedleave")
                    {
                        Subject = template[0].ISES_MailSubject.ToString();
                    }
                    else
                    {
                        Subject = template[0].ISES_MailSubject.ToString() + " - " + Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy");
                    }

                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

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

                    //try
                    //{
                    //    List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                    //    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    //    {
                    //        if (Template == "Unauthorizedleave")
                    //        {
                    //            cmd.CommandText = "Unauthorizedleave";
                    //        }
                    //        else
                    //        {
                    //            cmd.CommandText = "Fetch_Plannerccmails";
                    //        }

                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    //        SqlDbType.BigInt)
                    //        {
                    //            Value = data.HRME_Id
                    //        });

                    //        if (cmd.Connection.State != ConnectionState.Open)
                    //            cmd.Connection.Open();

                    //        var retObject = new List<dynamic>();
                    //        try
                    //        {
                    //            using (var dataReader = cmd.ExecuteReader())
                    //            {
                    //                while (dataReader.Read())
                    //                {
                    //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                    dtonewtemp.Add(new ISM_PlannerReportsDTO
                    //                    {
                    //                        MailCc = Convert.ToString(dataReader["cc"]),
                    //                        MailBCc = Convert.ToString(dataReader["bcc"])
                    //                    });
                    //                }
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Console.WriteLine(ex.Message);
                    //        }
                    //    }
                    //    if (dtonewtemp.Count > 0)
                    //    {
                    //        for (int j = 0; j < dtonewtemp.Count; j++)
                    //        {
                    //            mailcc = dtonewtemp[j].MailCc;
                    //            mailbcc = dtonewtemp[j].MailBCc;
                    //        }
                    //        if (mailcc != null && mailcc != "")
                    //        {
                    //            string[] ccmaildetails = mailcc.Split(',');

                    //            foreach (var c in ccmaildetails)
                    //            {
                    //                if (c != Email)
                    //                {
                    //                    message.AddCc(c);
                    //                }
                    //            }
                    //        }
                    //        if (mailbcc != null && mailbcc != "")
                    //        {
                    //            string[] bccmaildetails = mailbcc.Split(',');

                    //            foreach (var c in bccmaildetails)
                    //            {
                    //                if (c != Email)
                    //                {
                    //                    message.AddBcc(c);
                    //                }
                    //            }
                    //        }
                    //    }

                    //}
                    //catch (Exception error)
                    //{
                    //    //
                    //}

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        if (modulename.Count == 0)
                        {
                            modulename[0] = "Issue Manager";
                        }

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

        public string sendmaillateinearly(long MI_Id, string empname, string Email, string Template, string taskdetail, ISM_NotificationDTO data, string gender)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";

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

                if (gender.Equals("MALE", StringComparison.OrdinalIgnoreCase))
                {
                    val.Add("[NAME]", "Mr. " + empname);
                }
                else if ((gender.Equals("FEMALE", StringComparison.OrdinalIgnoreCase)))
                {
                    val.Add("[NAME]", "Ms. " + empname);
                }

                //val.Add("[MSG]", taskdetail);
                val.Add("[EMPLOYEENAME]", data.EmployeeName);
                val.Add("[EMPDESIGNATION]", data.HRMDES_DesignationName.ToUpper());
                val.Add("[DATE]", Convert.ToDateTime(data.FOEP_PunchDate).ToString("dd/MM/yyyy"));
                val.Add("[DATE1]", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"));
                val.Add("[Company]", data.MI_Name);
                val.Add("[EMPCODE]", data.HRME_EmployeeCode);
                val.Add("[LOGO]", "<img src=\"" + data.MI_Logo + "\" alt=\"Logo\">");

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
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = "";
                    if (Template == "Probition")
                    {
                        Subject = template[0].ISES_MailSubject.ToString();
                    }
                    else
                    {
                        Subject = template[0].ISES_MailSubject.ToString() + " - " + Convert.ToDateTime(data.FOEP_PunchDate).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(data.HRMERT_Enddate).ToString("dd/MM/yyyy");
                    }

                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

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

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        if (modulename.Count == 0)
                        {
                            modulename[0] = "Issue Manager";
                        }

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

        public string sendEmailprobi(long MI_Id, string Template, long HRME_ID, string email)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";

                }
                string str = "";
                int rownumber = 0;
                if (template.Count > 0)
                {            
                    str += "<h4> Dear Sir/ Madam</h4> <br>";
                    str += "<h5>Here to inform you about the employees who are scheduled to complete their probationary period.Please find the list below for your reference:</h5> <br>";
                    str += "<table class='table' style='border-collapse: collapse; width: 100%;' border='1'>" +
                        "<thead>" +
                        "<tr style='background-color: #f2f2f2;'>" +
                        "<th style='text-align: center;'>SL No</th>" +
                        "<th style='text-align: left;'>EMP CODE</th>" +
                        "<th style='text-align: left;'>Name</th>" +
                        "<th style='text-align: left;'>Date of Joining</th>" +
                        "<th style='text-align: left;'>Confirmation Date</th>" +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>";


                    List<ISM_DailyReportGenerationDTO> Tasklist = new List<ISM_DailyReportGenerationDTO>();
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "HR_Email_Prametars_Replace";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_IDE",
                            SqlDbType.BigInt)
                        {
                            Value = HRME_ID
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
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
                                    Tasklist.Add(new ISM_DailyReportGenerationDTO
                                    {

                                        employeename = Convert.ToString(dataReader["EMPNAME"]),
                                        HRME_DOJ = Convert.ToString(dataReader["DATE1"]),
                                        HRME_DOC = Convert.ToString(dataReader["DATE2"]),
                                        HRME_Employeecode = Convert.ToString(dataReader["EMPCODE"]),


                                    });
                                }
                            }
                        }
                        //SupplierName
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    if (Tasklist != null)
                    {

                        foreach (var t in Tasklist)
                        {

                            //FTI_Name
                            rownumber += 1;
                            str += "<tr>" +
                                                 "<td align=" + "left" + ">" + rownumber + "</td> " +
                                                   "<td align=" + "left" + ">" + t.HRME_Employeecode + "</td> " +
                                                "<td align=" + "left" + ">" + t.employeename + "</td>" +
                                                           "<td align=" + "left" + ">" + t.HRME_DOJ + "</td>" +
                                                             "<td align=" + "left" + ">" + t.HRME_DOC + "</td>" +

                                                "</tr>";
                        }
                    }
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;

                string Resultsms = Mailcontent;

                string Mailmsg = str;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
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
                        message.AddTo(email);

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
                            Value = email
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
