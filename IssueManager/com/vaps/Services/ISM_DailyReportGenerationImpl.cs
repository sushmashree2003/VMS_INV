using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace IssueManager.com.vaps.Services
{
    public class ISM_DailyReportGenerationImpl : Interfaces.ISM_DailyReportGenerationInterface
    {
        public IssueManagerContext _issueManagerContext;
        public DomainModelMsSqlServerContext _db;
        ILogger<ISM_DailyReportGenerationDTO> _logger;
        public ISM_DailyReportGenerationImpl(IssueManagerContext _Context, ILogger<ISM_DailyReportGenerationDTO> _log, DomainModelMsSqlServerContext db)
        {
            _issueManagerContext = _Context;
            _logger = _log;
            _db = db;
        }

        /* lOAD DATA */
        public ISM_DailyReportGenerationDTO loaddata(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;
            long? deptid = 0;

            try
            {
                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    var hrmDempid = _issueManagerContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false
                    && t.HRME_Id == empid).Distinct().ToList();
                    if (hrmDempid.Count > 0)
                    {
                        deptid = hrmDempid.FirstOrDefault().HRMD_Id;
                    }
                }

                data.getdepartwisedeviationremrks = _issueManagerContext.ISM_Departmentwise_RemarksDMO.Where(a => a.MI_Id == data.MI_Id
                && a.HRMD_Id == deptid && a.ISMDR_ActiveFlag == true).ToArray();

                data.getdepartwisecategory = _issueManagerContext.ISM_Master_TaskCategory_DMO.Where(a => a.MI_Id == data.MI_Id
                && a.HRMD_Id == deptid && a.ISMMTCAT_ActiveFlag == true).ToArray();

                data.get_Status = _issueManagerContext.MasterStatus_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == deptid
                && t.ISMMISTS_ActiveFlag == true).Distinct().OrderBy(t => t.ISMMISTS_Order).ToArray();

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                //fromdatecon = Convert.ToDateTime("2020-02-17");
                try
                {
                    fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                data.getprioritylist = _issueManagerContext.MastersPriorities_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRMP_ActiveFlag == true).OrderBy(a => a.HRMP_Order).ToArray();

                data.getstatuslist = _issueManagerContext.MasterStatus_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMISTS_ActiveFlag == true).OrderBy(a => a.ISMMISTS_Order).ToArray();

                var getplannername = _issueManagerContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid && t.ISMTPL_ActiveFlg == true
                && t.ISMTPL_ApprovalFlg == true && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(fromdatecon)
                && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(fromdatecon)).ToList();

                var getplannernamecount = _issueManagerContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid
                && t.ISMTPL_ActiveFlg == true
                && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(fromdatecon)
                && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(fromdatecon)).ToList().OrderByDescending(t => t.CreatedDate);

                data.plannerlength = getplannernamecount.Count();

                data.getplannerdetails = getplannernamecount.ToArray();

                if (getplannername.Count > 0)
                {
                    data.plannername = getplannername.FirstOrDefault().ISMTPL_PlannerName;
                    data.plannerdetails = getplannername.ToArray();

                    var totalplanner = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTPL_Id == getplannername.FirstOrDefault().ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).Count();

                    data.plannerlength = totalplanner;

                    data.Approvalflag = true;
                }
                else
                {
                    data.Approvalflag = false;
                }


                // ****************** Daily Report Generated For Running Month Year Count ******************//

                //var dailyreportgenratedcount = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                //&& a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_HalfDayFlag != true && Convert.ToDateTime(a.ISMDRPT_Date).Month == System.DateTime.Today.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == System.DateTime.Today.Year).Select(a => a.ISMDRPT_Date).Distinct().Count();

                //var dailyreportgenratedcounthlafday = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                //&& a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_HalfDayFlag == true && Convert.ToDateTime(a.ISMDRPT_Date).Month == System.DateTime.Today.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == System.DateTime.Today.Year).Select(a => a.ISMDRPT_Date).Distinct();

                //var count = 0.00;

                //foreach (var c in dailyreportgenratedcounthlafday)
                //{
                //    count += 0.5;
                //}
                //data.dailyreportcount = Convert.ToDecimal(dailyreportgenratedcount) + Convert.ToDecimal(count);


                long DR_Generated_Count = 0;
                data.dailyreportcount = DR_Generated_Count;

                long DR_Generated_OthersDate_Count = 0;
                data.dailyreportothersdatecount = DR_Generated_OthersDate_Count;

                long DR_Generated_BlockDate_Count = 0;
                data.dailyreportblockedcount = DR_Generated_BlockDate_Count;

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generated_Count";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@DATE", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@OutputCount", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@OutputOthersDateCount", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@OutputBlockDateCount", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data1 = cmd.ExecuteNonQuery();

                    DR_Generated_Count = Convert.ToInt64(cmd.Parameters["@OutputCount"].Value.ToString());
                    DR_Generated_OthersDate_Count = Convert.ToInt64(cmd.Parameters["@OutputOthersDateCount"].Value.ToString());
                    DR_Generated_BlockDate_Count = Convert.ToInt64(cmd.Parameters["@OutputBlockDateCount"].Value.ToString());

                    if (DR_Generated_Count > 0)
                    {
                        data.dailyreportcount = DR_Generated_Count;
                    }

                    if (DR_Generated_OthersDate_Count > 0)
                    {
                        data.dailyreportothersdatecount = DR_Generated_OthersDate_Count;
                    }

                    if (DR_Generated_BlockDate_Count > 0)
                    {
                        data.dailyreportblockedcount = DR_Generated_BlockDate_Count;
                    }
                }


                //************ Other's Date Generated Daily Report COunt For Running Month Year Count ******************//

                //var dailyreportgenratedotherdatecountcount = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.HRME_Id == empid && a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_OrdersDateFlg == true && a.ISMDRPT_HalfDayFlag != true
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Month == System.DateTime.Today.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == System.DateTime.Today.Year).Select(a => a.ISMDRPT_Date).Distinct().Count();

                //var dailyreportgenratedotherdatecountcounthlafday = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.HRME_Id == empid && a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_OrdersDateFlg == true && a.ISMDRPT_HalfDayFlag == true
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Month == System.DateTime.Today.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == System.DateTime.Today.Year).Select(a => a.ISMDRPT_Date).Distinct();


                //var countothers = 0.00;

                //foreach (var c in dailyreportgenratedotherdatecountcounthlafday)
                //{
                //    countothers += 0.5;
                //}

                //data.dailyreportothersdatecount = Convert.ToDecimal(dailyreportgenratedotherdatecountcount) + Convert.ToDecimal(countothers);


                //************ Blocked's Date Generated Daily Report COunt For Running Month Year Count ******************//

                //var dailyreportgenrateblockedcountcount = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.HRME_Id == empid && a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_BlockedFlag == true && a.ISMDRPT_HalfDayFlag != true
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Month == System.DateTime.Today.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == System.DateTime.Today.Year).Select(a => a.ISMDRPT_Date).Distinct().Count();

                //var dailyreportgenratedblockeddatecountcounthlafday = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.HRME_Id == empid && a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_BlockedFlag == true && a.ISMDRPT_HalfDayFlag == true
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Month == System.DateTime.Today.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == System.DateTime.Today.Year).Select(a => a.ISMDRPT_Date).Distinct();


                //var counblockedtothers = 0.00;

                //foreach (var c in dailyreportgenratedblockeddatecountcounthlafday)
                //{
                //    counblockedtothers += 0.5;
                //}

                //data.dailyreportblockedcount = Convert.ToDecimal(dailyreportgenrateblockedcountcount) + Convert.ToDecimal(counblockedtothers);


                //************ Other's Date Extension For Generated Daily Report Count ******************//
                var dailyreportextensiondotherdatecount = _issueManagerContext.ISM_DailyReportExtensionDMO.Where(a => a.HRME_Id == empid && a.ISMODE_ActiveFlg == true
                && a.IVRM_Month_Id == DateTime.Now.Month
                && Convert.ToDateTime(a.ISMODE_Date).Year == DateTime.Now.Year).Distinct().ToList();

                data.dailyreportextensiondotherdate = dailyreportextensiondotherdatecount.ToArray();
                if (dailyreportextensiondotherdatecount.Count > 0)
                {
                    var extdays = 0;
                    for (var iFiled = 0; iFiled < dailyreportextensiondotherdatecount.Count; iFiled++)
                    {
                        extdays = extdays + Convert.ToInt32(dailyreportextensiondotherdatecount[iFiled].ISMODE_Noofdays);
                    }
                    data.extensiondays = extdays;
                }
                else
                {
                    data.extensiondays = 0;
                }

                if (data.dailyreportothersdatecount < (3 + data.extensiondays))
                {
                    data.othersdayenable = true;
                }
                else
                {
                    data.othersdayenable = false;
                }

                //added by manohar to check weather the con call category is mapped or note
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Concall_Employee_Mapping_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });

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
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //added by nagaraj  to check weather the employee has taken demo rating from td sir
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_desg_list_DemoRatings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date) { Value = confromdate });

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
                        data.get_userEmplistDemoRating = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });
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
                        data.gettasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //added by praveenkumar order acceptance dr block
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "OrderAceptanceBlock_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
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
                        data.deviation_create = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //added by praveenkumar performal invoice dr block 
           

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Proforma_Invoice_DR_Block";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
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
                        data.ProformaInvoice = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.getemployeedetails = _issueManagerContext.MasterEmployee.Where(a => a.HRME_Id == empid).ToArray();

                /* CHECKING THE EMPLOYEE DAILY REPORT BLOCKED OR NOT */
                var getdailyreportblocked = _issueManagerContext.ISM_Block_EmployeeDMO.Where(t => t.HRME_Id == empid
                && t.ISMBE_BlockFlg == true && t.ISMBE_ActiveFlg == true).ToList();

                data.blockedcount = getdailyreportblocked.Count();

                // data.blockeddetails = getdailyreportblocked.ToArray();

                /* Daily Report Blocked Details */
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Blocked_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
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
                        data.blockeddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                /*GET THE EFFORT WORKING DAYS */
                DateTime datetimennow = DateTime.Now;
                datetimennow = Convert.ToDateTime(datetimennow.Date.ToString("yyyy-MM-dd"));
                long lyear_id = 0;
                var yearid = _issueManagerContext.HR_Master_LeaveYearDMO.Where(l => l.MI_Id == data.MI_Id && l.HRMLY_ActiveFlag == true
                && (l.HRMLY_FromDate <= datetimennow && l.HRMLY_ToDate >= datetimennow)).Distinct().ToList();

                var dep_id = _issueManagerContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;

                if (yearid.Count > 0)
                {
                    lyear_id = yearid.FirstOrDefault().HRMLY_Id;
                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_bwDates_Planner";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt) { Value = lyear_id });
                    cmd.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
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
                        data.get_effortdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.getgenerateddailyreportdetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                && a.HRME_Id == empid && a.ISMDRPT_Date == data.ISMDRPT_Date).ToArray();
                if (data.getgenerateddailyreportdetails.Length > 0)
                {
                    data.message = "Generated";
                }

                if (data.DR_Notification == 0)
                {
                    var checkresult = _issueManagerContext.ISM_DailyReportNotSent_DateWiseRemarksDMO.Where(a => a.HRME_Id == empid
                    && a.ISMDRNSDR_RemarksDate.Value.Date == DateTime.Now.Date && a.ISMDRNSDR_TemplateName == "DRNotSentRemarks").ToList();
                    if (checkresult.Count == 0)
                    {
                        using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "EmpDailyReportNotSentDatesList";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
                            cmd.Parameters.Add(new SqlParameter("@DrDate", SqlDbType.VarChar) { Value = confromdate });

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
                                data.getdrnotsentdetails = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }

                /* CHECKING WHETHER TL GIVEN APPROVAL FOR THERE TEAM IF NOT TL WILL NOT GENERATE DR  */

                var checkdeptheadcode = _issueManagerContext.HR_Master_DepartmentCode_Head.Where(a => a.HRME_ID == empid).ToList();

                if (checkdeptheadcode.Count > 0)
                {
                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Checking_DailyReport_Approval_GivenOrNot_TL_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = confromdate });

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
                            data.drnotapprovedmessage = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                // Task Category Check List

                //if (getplannername.Count > 0)
                //{
                //    var getplanner_taskdetails = (from a in _issueManagerContext.ISM_Task_Planner_TasksDMO
                //                                  from b in _issueManagerContext.ISM_TaskCreationDMO
                //                                  from c in _issueManagerContext.ISM_Task_PlannerDMO
                //                                  where (a.ISMTPL_Id == c.ISMTPL_Id && a.ISMTCR_Id == b.ISMTCR_Id
                //                                  && a.ISMTPL_Id == getplannername.FirstOrDefault().ISMTPL_Id && c.ISMTPL_Id == getplannername.FirstOrDefault().ISMTPL_Id
                //                                  && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_ApprovalFlg == true)
                //                                  select b).Distinct().Select(a => a.ISMMTCAT_Id).ToList();

                //    data.gettaskcategorychecklist = (from a in _issueManagerContext.ISM_Master_TaskCategory_ChecklistDMO_con
                //                                     from b in _issueManagerContext.ISM_Master_TaskCategory_DMO
                //                                     where (a.ISMMTCAT_Id == b.ISMMTCAT_Id && a.ISMMTCATCL_ActiveFlag == true
                //                                     && getplanner_taskdetails.Contains(a.ISMMTCAT_Id))
                //                                     select a).Distinct().ToArray();
                //}

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var gettoday_drdetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                && a.ISMDRPT_Date.Value.Date == indiantime0.Date).Select(a => a.ISMDRPT_Id).ToList();

                var getfiledetails = (from a in _issueManagerContext.ISM_Master_TaskCategory_ChecklistDMO_con
                                      from b in _issueManagerContext.ISM_Master_TaskCategory_DMO
                                      from c in _issueManagerContext.ISM_DailyReport_FilesDMO
                                      from d in _issueManagerContext.ISM_DailyReportGenerationDMO
                                      where (a.ISMMTCAT_Id == b.ISMMTCAT_Id && c.ISMMTCATCL_Id == a.ISMMTCATCL_Id && c.ISMDRPT_Id == d.ISMDRPT_Id
                                      && gettoday_drdetails.Contains(c.ISMDRPT_Id))
                                      select new ISM_DailyReportGenerationDTO
                                      {
                                          ISMDRPTFL_Id = c.ISMDRPTFL_Id,
                                          ISMDRPT_Id = c.ISMDRPT_Id,
                                          ISMDRPTFL_FileName = c.ISMDRPTFL_FileName,
                                          ISMDRPTFL_RefNo = c.ISMDRPTFL_RefNo,
                                          ISMDRPTFL_Remarks = c.ISMDRPTFL_Remarks,
                                          ISMDRPTFL_FilePath = c.ISMDRPTFL_FilePath,
                                          ISMMTCATCL_Id = c.ISMMTCATCL_Id,
                                          ISMTPLTA_Id = d.ISMTPLTA_Id,
                                          ISMTCR_Id = d.ISMTCR_Id,
                                          ISMMTCATCL_CheckListName = a.ISMMTCATCL_CheckListName
                                      }).Distinct().ToArray();

                data.getfiledetails = getfiledetails;

                data.daywise_drreport = _issueManagerContext.ISM_DailyReport_DaywiseDMO.Where(a => a.HRME_Id == empid
                && a.ISMDRPTDW_Date.Value.Date == indiantime0.Date).ToArray();

                data.HRME_Id = empid;

                //HR Planner Details For Mapped Users

                List<ISM_DailyReportGenerationDTO> hrplannerdetails = new List<ISM_DailyReportGenerationDTO>();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var checkplannerresult = _issueManagerContext.ISM_DailyReportNotSent_DateWiseRemarksDMO.Where(a => a.HRME_Id == empid
                    && a.ISMDRNSDR_RemarksDate.Value.Date == DateTime.Now.Date && a.ISMDRNSDR_TemplateName == "HRPlannerRemarks").ToList();

                if (checkplannerresult.Count == 0)
                {
                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_HR_PlannerManagment_Details_DR";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });

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
                            data.hrplannerdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "telecallerblockDR";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.VarChar) { Value = data.UserId });
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
                        data.telecallerblock = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                // Checking User Wise Completed Task Count Details
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PendingCloseTask_List_DR_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });

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
                        data.CloseTaskCoutnDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //Added By sanjeev RTO
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RTO_FOemplist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
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
                        data.RTO_FOemplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (data.RTO_FOemplist != null && data.RTO_FOemplist.Length > 0)
                {
                    DateTime? fromdates = DateTime.Now;
                    string fromdate = fromdates.Value.ToString("yyyy-MM-dd");

                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RTO_FOVehiclecountcheck";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DATE", SqlDbType.VarChar) { Value = fromdate });
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
                            data.RTO_VehcalList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //RTO_FOVehiclecountcheck  
                }

                var adavnace = (from a in _issueManagerContext.HR_PROCESSDMO
                                from b in _issueManagerContext.HR_Process_Auth_OrderNoDMO
                                where (a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAD" && b.IVRMUL_Id == data.UserId)
                                select a
                                ).ToList();
                var tadaapply = (from a in _issueManagerContext.HR_PROCESSDMO
                                 from b in _issueManagerContext.HR_Process_Auth_OrderNoDMO
                                 where (a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAPL" && b.IVRMUL_Id == data.UserId)
                                 select a
                               ).ToList();
                if (adavnace.Count > 0 || tadaapply.Count > 0)
                {

                    DateTime dates = DateTime.Now.AddDays(-2);

                    var VTADAA_IdLi = _issueManagerContext.TADA_Advance_Apply_AuthorisationDMO.Where(R => R.IVRMUL_Id == data.UserId).ToList();
                    List<long> VTADAA_IdCC = new List<long>();
                    if (VTADAA_IdLi.Count > 0)
                    {
                        foreach (var d in VTADAA_IdLi)
                        {
                            VTADAA_IdCC.Add(d.VTADAAA_Id);
                        }
                    }


                    var TadaAdavancecount = _issueManagerContext.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_StatusFlg == "Pending" && (R.VTADAAA_AppliedDate < dates) && !VTADAA_IdCC.Contains(R.VTADAAA_Id)).ToList();
                    if (TadaAdavancecount.Count > 0)
                    {
                        using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "TADA_LIST";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
                            cmd.Parameters.Add(new SqlParameter("@TypeFlag", SqlDbType.VarChar) { Value = "Advance" });
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
                                data.AdavanceList = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }


                    var VTADAA_IdList = _issueManagerContext.TADA_Apply_AuthorisationDMO.Where(R => R.IVRMUL_Id == data.UserId).ToList();
                    List<long> VTADAA_IdC = new List<long>();
                    if (VTADAA_IdList.Count > 0)
                    {
                        foreach (var d in VTADAA_IdList)
                        {
                            VTADAA_IdC.Add(d.VTADAA_Id);
                        }
                    }
                    var applycount = _issueManagerContext.TADA_ApplyDMO.Where(R => R.VTADAA_StatusFlg == "Pending" &&
                             (R.VTADAA_AppliedDate <= dates) && !VTADAA_IdC.Contains(R.VTADAA_Id)).ToList();
                    if (applycount.Count > 0)
                    {
                        using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "TADA_LIST";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
                            cmd.Parameters.Add(new SqlParameter("@TypeFlag", SqlDbType.VarChar) { Value = "Apply" });
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
                                data.ApplyList = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                }

                //*********************************** Rating Approval Is Pending ********************************// 
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {

                    var role = _issueManagerContext.IVRM_Role_Type.Where(a => a.IVRMRT_Id == data.roleId).Select(a => a.IVRMRT_Role).FirstOrDefault();

                    cmd.CommandText = "ISM_Rating_User_EmployeeList_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = role });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.get_unratedemployeelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //*********************************** Appoint generation DR Bloack ***************************************//

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {

                    var role = _issueManagerContext.IVRM_Role_Type.Where(a => a.IVRMRT_Id == data.roleId).Select(a => a.IVRMRT_Role).FirstOrDefault();

                    cmd.CommandText = "HR_Candidate_Approval_DRblock";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    //cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = role });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.dailyreportextensiondotherdate = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //*********************************** Asset Mapping DR Bloack by Aman ***************************************//

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    var role = _issueManagerContext.IVRM_Role_Type.Where(a => a.IVRMRT_Id == data.roleId).Select(a => a.IVRMRT_Role).FirstOrDefault();

                    cmd.CommandText = "HR_Candidate_Asset_DR_Block";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt) { Value = data.UserId });

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
                                    var columnName = dataReader.GetName(iFiled);
                                    if (!string.IsNullOrEmpty(columnName) && !dataRow.ContainsKey(columnName))
                                    {
                                        dataRow.Add(columnName, dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    else
                                    {
                                        // Optionally log or handle duplicates
                                        Console.WriteLine($"Duplicate or empty column: {columnName}");
                                    }
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.assetsmappingdrblock = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }





                //**************************************** Pending  feedback ********************** addded by kaveti***//
                var emptype = "";
                var emptype1 = "";
                var traineehrmeid = _issueManagerContext.HR_Training_Create_Participants_DMO_con.Where(t => t.HRME_Id == empid).ToArray();
                var trainerhrmeid = _issueManagerContext.HR_Training_Create_IntTrainer_DMO_con.Where(t => t.HRME_Id == empid).ToArray();
                if (trainerhrmeid.Length > 0)
                {
                    emptype = "Trainer";
                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_Feedback_pendingList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                 SqlDbType.BigInt)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
          SqlDbType.VarChar)
                        {
                            Value = emptype
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
                            data.get_feedbackTrainer = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                if (traineehrmeid.Length > 0)
                {
                    emptype1 = "Trainee";
                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_Feedback_pendingList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                 SqlDbType.BigInt)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
          SqlDbType.VarChar)
                        {
                            Value = emptype1
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
                            data.get_feedbackTrainee = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }


                //added By sanjeev set Reminder Account
                var rolet = _issueManagerContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;

                    if (roletype.Equals(roletype.Equals("Accountant", StringComparison.OrdinalIgnoreCase)))
                    {
                        using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "HR_PlanerManagment_Reminder";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
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
                                data.ReminderList = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        //get_report
                        using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "TADA_Payments_AccountReminder";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
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
                                data.get_report = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    //IVRMRT_Role


                }

                //clientdropdown
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Pending_AssignedBy_Approved_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@RoleFlg",
           SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@countflag",
           SqlDbType.VarChar)
                    {
                        Value = "Notapproved"
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.VarChar)
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.clientdropdown = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RoutePlan_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Hrme_id", SqlDbType.BigInt) { Value = empid });
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
                        data.fileupload = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //adedd   
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TADA_Advance_ApplyCount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Hrme_id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt) { Value = "1" });
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
                        data.droveralldetailsdatewise = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (data.droveralldetailsdatewise != null && data.droveralldetailsdatewise.Length > 0)
                {
                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADA_Advance_ApplyCount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Hrme_id", SqlDbType.BigInt) { Value = data.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt) { Value = "2" });
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
                            data.drnotapprovedmessageindividual = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //get_report_effort
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RoutePlanBlock_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
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
                        data.notificationUser = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //getdetails
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_DemoUpdateListdashDR";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.VarChar) { Value = data.UserId });
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
                        data.demo_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //*************************************** ******************** addded by kaveti***//
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
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
                        data.get_feedback = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_probationperiod_threeMonth_left";
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
                        data.getchecklistarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MOM_DR_Block ";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
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
                        data.get_report_effort = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Regenarated_Task_details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
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
                        data.ISM_RegeneratedDR = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //getbda_Report //@HRME_ID

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_UpComingDays_TourDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
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
                        data.getbda_Report = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_DR_Block_Proxy";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.proxyNotMapped = retObject.ToArray();
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
                _logger.LogInformation("Daily Report Generation Load Data For Employee Id : " + empid + "   : " + ex.Message);
            }
            return data;
        }

        /* GET OTHERS DATE DETAILS*/
        public ISM_DailyReportGenerationDTO Otherdaycount(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;
            long? deptid = 0;
            try
            {
                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    var hrmDempid = _issueManagerContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.HRME_Id == empid).Distinct().ToList();
                    if (hrmDempid.Count > 0)
                    {
                        deptid = hrmDempid.FirstOrDefault().HRMD_Id;
                    }
                }

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                //fromdatecon = Convert.ToDateTime("2020-02-17");
                try
                {
                    fromdatecon = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                long DR_Generated_Count = 0;
                data.dailyreportcount = DR_Generated_Count;

                long DR_Generated_OthersDate_Count = 0;
                data.dailyreportothersdatecount = DR_Generated_OthersDate_Count;

                long DR_Generated_BlockDate_Count = 0;
                data.dailyreportblockedcount = DR_Generated_BlockDate_Count;

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generated_Count";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@DATE", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@OutputCount", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@OutputOthersDateCount", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@OutputBlockDateCount", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data1 = cmd.ExecuteNonQuery();

                    DR_Generated_Count = Convert.ToInt64(cmd.Parameters["@OutputCount"].Value.ToString());
                    DR_Generated_OthersDate_Count = Convert.ToInt64(cmd.Parameters["@OutputOthersDateCount"].Value.ToString());
                    DR_Generated_BlockDate_Count = Convert.ToInt64(cmd.Parameters["@OutputBlockDateCount"].Value.ToString());

                    if (DR_Generated_Count > 0)
                    {
                        data.dailyreportcount = DR_Generated_Count;
                    }

                    if (DR_Generated_OthersDate_Count > 0)
                    {
                        data.dailyreportothersdatecount = DR_Generated_OthersDate_Count;
                    }

                    if (DR_Generated_BlockDate_Count > 0)
                    {
                        data.dailyreportblockedcount = DR_Generated_BlockDate_Count;
                    }
                }


                //added by manohar to check weather the con call category is mapped or note
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Concall_Employee_Mapping_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });

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
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //added by nagaraja to check weather the employee taken demo rating  
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_desg_list_DemoRatings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date) { Value = confromdate });


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
                        data.get_userEmplistDemoRating = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                ////************ Other's Date Generated Daily Report Count Month-Year wise Count ******************//
                //var dailyreportgenratedotherdatecountcount = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.HRME_Id == empid && a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_OrdersDateFlg == true && a.ISMDRPT_HalfDayFlag != true
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Month == data.ISMDRPT_Date.Value.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == data.ISMDRPT_Date.Value.Year).Select(a => a.ISMDRPT_Date).Distinct().Count();

                //var dailyreportgenratedotherdatecountcounthlafcount = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.HRME_Id == empid && a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_OrdersDateFlg == true && a.ISMDRPT_HalfDayFlag == true
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Month == data.ISMDRPT_Date.Value.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == data.ISMDRPT_Date.Value.Year).Select(a => a.ISMDRPT_Date).Distinct();

                //var counthalf = 0.00;

                //foreach (var c in dailyreportgenratedotherdatecountcounthlafcount)
                //{
                //    counthalf += 0.5;
                //}

                //data.dailyreportothersdatecount = Convert.ToDecimal(dailyreportgenratedotherdatecountcount) + Convert.ToDecimal(counthalf);


                ////************ Blocked's Date Generated Daily Report Count For Running Month Year Count ******************//
                //var dailyreportgenrateblockedcountcount = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.HRME_Id == empid && a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_BlockedFlag == true && a.ISMDRPT_HalfDayFlag != true
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Month == data.ISMDRPT_Date.Value.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == data.ISMDRPT_Date.Value.Year).Select(a => a.ISMDRPT_Date).Distinct().Count();

                //var dailyreportgenratedblockeddatecountcounthlafday = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.HRME_Id == empid && a.ISMDRPT_ActiveFlg == true && a.ISMDRPT_BlockedFlag == true && a.ISMDRPT_HalfDayFlag == true
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Month == data.ISMDRPT_Date.Value.Month
                //&& Convert.ToDateTime(a.ISMDRPT_Date).Year == data.ISMDRPT_Date.Value.Year).Select(a => a.ISMDRPT_Date).Distinct();

                //var counblockedtothers = 0.00;
                //foreach (var c in dailyreportgenratedblockeddatecountcounthlafday)
                //{
                //    counblockedtothers += 0.5;
                //}

                //data.dailyreportblockedcount = Convert.ToDecimal(dailyreportgenrateblockedcountcount) + Convert.ToDecimal(counblockedtothers);

                //************ Other's Date Extension For Generated Daily Report Count ******************//
                var dailyreportextensiondotherdatecount = _issueManagerContext.ISM_DailyReportExtensionDMO.Where(a => a.HRME_Id == empid
                && a.ISMODE_ActiveFlg == true && a.IVRM_Month_Id == data.ISMDRPT_Date.Value.Month
                && Convert.ToDateTime(a.ISMODE_Date).Year == data.ISMDRPT_Date.Value.Year).Distinct().ToList();

                data.dailyreportextensiondotherdate = dailyreportextensiondotherdatecount.ToArray();

                if (dailyreportextensiondotherdatecount.Count > 0)
                {
                    var extdays = 0;
                    for (var iFiled = 0; iFiled < dailyreportextensiondotherdatecount.Count; iFiled++)
                    {
                        extdays = extdays + Convert.ToInt32(dailyreportextensiondotherdatecount[iFiled].ISMODE_Noofdays);
                    }
                    data.extensiondays = extdays;
                }
                else
                {
                    data.extensiondays = 0;
                }

                if (data.dailyreportothersdatecount < (3 + data.extensiondays))
                {
                    data.othersdayenable = true;
                }
                else
                {
                    data.othersdayenable = false;
                }

                /*GET THE EFFORT WORKING DAYS */
                DateTime datetimennow = DateTime.Now;
                datetimennow = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                long lyear_id = 0;
                var yearid = _issueManagerContext.HR_Master_LeaveYearDMO.Where(l => l.MI_Id == data.MI_Id && l.HRMLY_ActiveFlag == true
                && (l.HRMLY_FromDate <= datetimennow && l.HRMLY_ToDate >= datetimennow)).Distinct().ToList();

                var dep_id = _issueManagerContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;

                if (yearid.Count > 0)
                {
                    lyear_id = yearid.FirstOrDefault().HRMLY_Id;
                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_bwDates_Planner";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt) { Value = lyear_id });
                    cmd.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
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
                        data.get_effortdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });
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
                        data.gettasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var getplannername = _issueManagerContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid && t.ISMTPL_ActiveFlg == true
                && t.ISMTPL_ApprovalFlg == true && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(fromdatecon)
                && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(fromdatecon)).ToList();

                var getplannernamecount = _issueManagerContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid
                && t.ISMTPL_ActiveFlg == true
                && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(fromdatecon)
                && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(fromdatecon)).ToList().OrderByDescending(t => t.CreatedDate);

                data.plannerlength = getplannernamecount.Count();

                data.getplannerdetails = getplannernamecount.ToArray();

                if (getplannername.Count > 0)
                {
                    data.plannername = getplannername.FirstOrDefault().ISMTPL_PlannerName;
                    data.plannerdetails = getplannername.ToArray();

                    var totalplanner = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTPL_Id == getplannername.FirstOrDefault().ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).Count();

                    data.plannerlength = totalplanner;

                    data.Approvalflag = true;
                }
                else
                {
                    data.Approvalflag = false;
                }

                data.getgenerateddailyreportdetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                && a.ISMDRPT_Date == data.ISMDRPT_Date).ToArray();
                if (data.getgenerateddailyreportdetails.Length > 0)
                {
                    data.message = "Generated";
                }

                var checkdeptheadcode = _issueManagerContext.HR_Master_DepartmentCode_Head.Where(a => a.HRME_ID == empid).ToList();

                if (checkdeptheadcode.Count > 0)
                {
                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Checking_DailyReport_Approval_GivenOrNot_TL_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = confromdate });

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
                            data.drnotapprovedmessage = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                // Checking User Wise Completed Task Count Details
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PendingCloseTask_List_DR_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });

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
                        data.CloseTaskCoutnDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // Task Category Check List
                //data.gettaskcategorychecklist = (from a in _issueManagerContext.ISM_Master_TaskCategory_ChecklistDMO_con
                //                                 from b in _issueManagerContext.ISM_Master_TaskCategory_DMO
                //                                 where (a.ISMMTCAT_Id == b.ISMMTCAT_Id && a.ISMMTCATCL_ActiveFlag == true && b.MI_Id == data.MI_Id
                //                                 && b.HRMD_Id == deptid)
                //                                 select a).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Daily Report Other Day Count : " + empid + "   : " + ex.Message);
            }
            return data;
        }

        /* SAVE DR DETAILS */
        public ISM_DailyReportGenerationDTO generatedailyreport(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;
            long counttrue = 0;
            try
            {
                data.ISMDRPT_BlockedFlag = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                //var checkdailyreportgenerated = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                //&& a.ISMDRPT_Date == data.ISMDRPT_Date).ToList();
                //if (checkdailyreportgenerated.Count() > 0)
                //{
                //    data.message = "Generated";
                //    return data;
                //}

                if (data.todayorothers == "today")
                {
                    data.ISMDRPT_OrdersDateFlg = false;
                    data.ISMDRPT_OthersDateReason = "";
                }
                else
                {
                    data.ISMDRPT_OrdersDateFlg = true;
                    data.ISMDRPT_OthersDateReason = data.reason;
                }

                DateTime fromdatecon1 = DateTime.Now;
                string confromdate1 = "";
                try
                {
                    fromdatecon1 = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate1 = fromdatecon1.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (data.ISMTPL_Id > 0)
                {
                    var plannerid = data.ISMTPL_Id;
                }
                else
                {
                    var getplannername = _issueManagerContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid
                    && t.ISMTPL_ActiveFlg == true && t.ISMTPL_ApprovalFlg == true
                    && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(fromdatecon1)
                    && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(fromdatecon1)).ToList();

                    data.ISMTPL_Id = getplannername.FirstOrDefault().ISMTPL_Id;

                }

                /* GET THE APPROVED PLANNER ID FOR SELECTD PLANNER  */
                var getapprovedplanner = _issueManagerContext.ISM_Task_Planner_ApprovedDMO.Where(a => a.ISMTPL_Id == data.ISMTPL_Id).ToList();
                decimal? totalhrs = 0;

                // ******** CHECKING DAILY REPORT IS BLOCKED ******* //
                if (data.todayorothers == "today")
                {
                    var getdailyreportblocked = _issueManagerContext.ISM_Block_EmployeeDMO.Where(t => t.HRME_Id == empid
                    && t.ISMBE_BlockFlg == true && t.ISMBE_ActiveFlg == true).ToList();

                    data.blockedcount = getdailyreportblocked.Count();
                    if (data.blockedcount > 0)
                    {
                        data.ISMDRPT_BlockedFlag = true;
                    }
                }
                else
                {
                    //var getdailyreportblocked = _issueManagerContext.ISM_Block_EmployeeDMO.Where(t => t.HRME_Id == empid
                    //&& t.ISMBE_BlockDate.Value.Date >= data.ISMDRPT_Date.Value.Date
                    //&& t.ISEBE_UnblockDate.Value.Date <= data.ISMDRPT_Date.Value.Date && t.ISMBE_ActiveFlg == true).ToList();

                    //data.blockedcount = getdailyreportblocked.Count();
                    //if (data.blockedcount > 0)
                    //{
                    //    data.ISMDRPT_BlockedFlag = true;
                    //}
                    data.ISMDRPT_BlockedFlag = false;
                    DateTime blockeddate = DateTime.Now;
                    string blockeddate_confromdate1 = "";
                    long Blocked_Count = 0;
                    try
                    {
                        blockeddate = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                        blockeddate_confromdate1 = blockeddate.ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_DailyReport_Others_Block_Day_Generation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@hrme_id", SqlDbType.BigInt) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = blockeddate_confromdate1 });
                        cmd.Parameters.Add(new SqlParameter("@count", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        Blocked_Count = Convert.ToInt64(cmd.Parameters["@count"].Value.ToString());

                        if (Blocked_Count > 0)
                        {
                            data.ISMDRPT_BlockedFlag = true;
                        }
                    }
                }

                #region Insert Or Update Daywise Daily Report Table
                var checkdaywisedr_details = _issueManagerContext.ISM_DailyReport_DaywiseDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                     && a.ISMDRPTDW_Date == data.ISMDRPT_Date).ToList();

                if (checkdaywisedr_details.Count > 0)
                {
                    var result_checkdaywisedr_details = _issueManagerContext.ISM_DailyReport_DaywiseDMO.Single(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                     && a.ISMDRPTDW_Date == data.ISMDRPT_Date);
                    result_checkdaywisedr_details.ISMDRPTDW_BlockedDayDRFlg = data.ISMDRPT_BlockedFlag;
                    result_checkdaywisedr_details.ISMDRPTDW_HalfDayDRFlg = data.ISMDRPT_HalfDayFlag;
                    result_checkdaywisedr_details.ISMDRPTDW_OrthersDateFlg = data.ISMDRPT_OrdersDateFlg;
                    result_checkdaywisedr_details.ISMDRPTDW_OthersDateReason = data.ISMDRPT_OthersDateReason;
                    result_checkdaywisedr_details.ISMDRPTDW_UpdatedBy = data.UserId;
                    result_checkdaywisedr_details.ISMDRPTDW_UpdatedDate = indiantime0;
                    _issueManagerContext.Update(result_checkdaywisedr_details);
                }
                else
                {
                    ISM_DailyReport_DaywiseDMO iSM_DailyReport_DaywiseDMO = new ISM_DailyReport_DaywiseDMO();
                    iSM_DailyReport_DaywiseDMO.MI_Id = data.MI_Id;
                    iSM_DailyReport_DaywiseDMO.HRME_Id = empid;
                    iSM_DailyReport_DaywiseDMO.ISMTPL_Id = data.ISMTPL_Id;
                    iSM_DailyReport_DaywiseDMO.ISMTPLAP_Id = getapprovedplanner.FirstOrDefault().ISMTPLAP_Id;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_OverallComments = "";
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_BlockedDayDRFlg = data.ISMDRPT_BlockedFlag;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_HalfDayDRFlg = data.ISMDRPT_HalfDayFlag;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_OrthersDateFlg = data.ISMDRPT_OrdersDateFlg;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_OthersDateReason = data.ISMDRPT_OthersDateReason;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_ActiveFlg = true;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_CreatedDate = indiantime0;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_UpdatedDate = indiantime0;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_CreatedBy = data.UserId;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_UpdatedBy = data.UserId;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_Date = data.ISMDRPT_Date;
                    _issueManagerContext.Add(iSM_DailyReport_DaywiseDMO);
                }
                #endregion 
                int ExtraCountTask = 0;
                for (int k = 0; k < data.Temp_ISM_DailyReportGenerationDTO.Count(); k++)
                {
                    var getextraflag = data.Temp_ISM_DailyReportGenerationDTO[k].extraflag;
                    var gettaskid = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                    var remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                    var startdate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_StartDate;
                    var enddate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EndDate;
                    var plannerid = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;
                    var plannertaskid = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id;

                    decimal? previous_timetaken = 0.00m;

                    #region Get_DateWise_Dr_TaskDetails 
                    List<ISM_DailyReportGenerationDMO> get_previous_dr_taskdetails = new List<ISM_DailyReportGenerationDMO>();

                    if (plannertaskid > 0)
                    {
                        get_previous_dr_taskdetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                                           && a.ISMDRPT_Date == data.ISMDRPT_Date && a.ISMTCR_Id == gettaskid && a.ISMTPL_Id == plannerid && a.ISMTPLTA_Id == plannertaskid).ToList();

                        if (get_previous_dr_taskdetails.Count > 0)
                        {
                            previous_timetaken = get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_TimeTakenInHrs;
                        }
                    }

                    #endregion                   

                    ISM_DailyReportGenerationDMO dmo = new ISM_DailyReportGenerationDMO();

                    data.ISMDRPT_DeviationFlg = false;
                    data.ISMDRPT_ExtraFlg = false;
                    totalhrs += 0;


                    // ************ Checking Planner Task Is Created Or Not ************ //
                    #region Insert Or Update Planner Task Details

                    ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                    var updateplannertask = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id
                    && a.ISMTPL_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true
                    && a.ISMTPLTA_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id).ToList();

                    if (updateplannertask.Count > 0)
                    {
                        decimal? count = 0;

                        count = updateplannertask.FirstOrDefault().ISMTPLTA_TimeTakenInHrs - previous_timetaken;

                        count += data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;

                        var result = _issueManagerContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id
                        && a.ISMTPL_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true
                        && a.ISMTPLTA_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id);

                        result.ISMTPLTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        result.ISMTPLTA_DeviationFlg = data.ISMDRPT_DeviationFlg;
                        if (data.ISMDRPT_DeviationFlg == true)
                        {
                            result.ISMTPLTA_FinishedDate = indiantime0;
                        }
                        result.ISMTPLTA_TimeTakenInHrs = Convert.ToDecimal(count);
                        result.UpdatedDate = indiantime0;
                        result.ISMTPLTA_UpdatedBy = data.UserId;
                        _issueManagerContext.Update(result);
                    }
                    else
                    {
                        dmo_task.ISMTPL_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;
                        dmo_task.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        dmo_task.ISMTPLTA_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Remarks;
                        dmo_task.ISMTPLTA_StartDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_StartDate;
                        dmo_task.ISMTPLTA_EndDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EndDate;
                        dmo_task.ISMTPLTA_EffortInHrs = Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs);
                        dmo_task.ISMTPLTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        dmo_task.ISMTPLTA_DeviationFlg = data.ISMDRPT_DeviationFlg;
                        if (data.ISMDRPT_DeviationFlg == true)
                        {
                            dmo_task.ISMTPLTA_FinishedDate = indiantime0;
                        }
                        if (data.Temp_ISM_DailyReportGenerationDTO[k].extraflag == 1)
                        {
                            data.ISMDRPT_ExtraFlg = true;
                        }

                        dmo_task.ISMTPLTA_ExtraFlg = data.ISMDRPT_ExtraFlg;
                        dmo_task.ISMTPLTA_PreviousTask = false;
                        dmo_task.ISMTPLTA_TimeTakenInHrs = Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs);
                        dmo_task.ISMTPLTA_ActiveFlg = true;
                        dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                        dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                        dmo_task.CreatedDate = indiantime0;
                        dmo_task.UpdatedDate = indiantime0;
                        dmo_task.ISMTPLTA_ApprovalFlg = true;

                        _issueManagerContext.Add(dmo_task);
                    }
                    //if (updateplannertask.Count > 0)
                    //{
                    //    decimal? count = 0;

                    //    count = updateplannertask.FirstOrDefault().ISMTPLTA_TimeTakenInHrs - previous_timetaken;

                    //    count += data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;

                    //    if (data.ISMDRPT_OrdersDateFlg == true) //adarsh
                    //    {

                    //        var result = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id &&
                    //                         a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_ApprovalFlg == true);

                    //        foreach (var X in result)
                    //        {
                    //            X.ISMTPLTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;


                    //            X.ISMTPLTA_DeviationFlg = data.ISMDRPT_DeviationFlg;
                    //            if (data.ISMDRPT_DeviationFlg == true)
                    //            {
                    //                X.ISMTPLTA_FinishedDate = indiantime0;
                    //            }
                    //            X.ISMTPLTA_TimeTakenInHrs = Convert.ToDecimal(count);
                    //            X.UpdatedDate = indiantime0;
                    //            X.ISMTPLTA_UpdatedBy = data.UserId;
                    //            _issueManagerContext.Update(X);
                    //        }

                    //    }
                    //    else
                    //    {

                    //        var result = _issueManagerContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id
                    //        && a.ISMTPL_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true
                    //        && a.ISMTPLTA_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id);

                    //        result.ISMTPLTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                    //        result.ISMTPLTA_DeviationFlg = data.ISMDRPT_DeviationFlg;
                    //        if (data.ISMDRPT_DeviationFlg == true)
                    //        {
                    //            result.ISMTPLTA_FinishedDate = indiantime0;
                    //        }
                    //        result.ISMTPLTA_TimeTakenInHrs = Convert.ToDecimal(count);
                    //        result.UpdatedDate = indiantime0;
                    //        result.ISMTPLTA_UpdatedBy = data.UserId;
                    //        _issueManagerContext.Update(result);
                    //    }
                    //}
                    //else
                    //{
                    //    dmo_task.ISMTPL_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;
                    //    dmo_task.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                    //    dmo_task.ISMTPLTA_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Remarks;
                    //    dmo_task.ISMTPLTA_StartDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_StartDate;
                    //    dmo_task.ISMTPLTA_EndDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EndDate;
                    //    dmo_task.ISMTPLTA_EffortInHrs = Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs);
                    //    dmo_task.ISMTPLTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                    //    dmo_task.ISMTPLTA_DeviationFlg = data.ISMDRPT_DeviationFlg;
                    //    if (data.ISMDRPT_DeviationFlg == true)
                    //    {
                    //        dmo_task.ISMTPLTA_FinishedDate = indiantime0;
                    //    }
                    //    if (data.Temp_ISM_DailyReportGenerationDTO[k].extraflag == 1)
                    //    {
                    //        data.ISMDRPT_ExtraFlg = true;
                    //    }

                    //    dmo_task.ISMTPLTA_ExtraFlg = data.ISMDRPT_ExtraFlg;
                    //    dmo_task.ISMTPLTA_PreviousTask = false;
                    //    dmo_task.ISMTPLTA_TimeTakenInHrs = Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs);
                    //    dmo_task.ISMTPLTA_ActiveFlg = true;
                    //    dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                    //    dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                    //    dmo_task.CreatedDate = indiantime0;
                    //    dmo_task.UpdatedDate = indiantime0;
                    //    dmo_task.ISMTPLTA_ApprovalFlg = true;

                    //    _issueManagerContext.Add(dmo_task);
                    //}

                    #endregion


                    #region Insert Into Daily Report Table Or Update

                    if (previous_timetaken > 0.00m)
                    {
                        var Update_DR_TaskDetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Single(a => a.HRME_Id == empid
                        && a.ISMDRPT_Date == data.ISMDRPT_Date && a.ISMTPL_Id == plannerid && a.ISMTPLTA_Id == plannertaskid && a.ISMTCR_Id == gettaskid
                        && a.ISMDRPT_Id == get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id);

                        Update_DR_TaskDetails.ISMDRPT_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                        Update_DR_TaskDetails.ISMDRPT_TimeTakenInHrs = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;
                        Update_DR_TaskDetails.ISMDRPT_TimeTakenFlg = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrsmin;
                        Update_DR_TaskDetails.ISMDRPT_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        Update_DR_TaskDetails.ISMDRPT_OthersDateReason = data.ISMDRPT_OthersDateReason;
                        Update_DR_TaskDetails.ISMDRPT_BlockedFlag = data.ISMDRPT_BlockedFlag;
                        Update_DR_TaskDetails.ISMDRPT_OrdersDateFlg = data.ISMDRPT_OrdersDateFlg;
                        Update_DR_TaskDetails.ISMDRPT_CompoffApplicableflag = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_CompoffApplicableflag;
                        Update_DR_TaskDetails.ISMDRPT_TimeTakenFlg = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrsmin;

                        if (data.Temp_ISM_DailyReportGenerationDTO[k].deviationflag == true)
                        {
                            Update_DR_TaskDetails.ISMDR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDR_Id;
                        }
                        Update_DR_TaskDetails.UpdatedDate = indiantime0;
                        Update_DR_TaskDetails.ISMDRPT_UpdatedBy = data.UserId;
                        _issueManagerContext.Update(Update_DR_TaskDetails);
                    }
                    else
                    {
                        if (data.Temp_ISM_DailyReportGenerationDTO[k].extraflag == 1)
                        {
                            ExtraCountTask += 1;
                            data.ISMDRPT_ExtraFlg = true;

                            if (previous_timetaken == 0.00m)
                            {
                                totalhrs += data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs;
                            }
                        }

                        if (data.Temp_ISM_DailyReportGenerationDTO[k].deviationflag == true)
                        {
                            data.ISMDRPT_DeviationFlg = true;
                            dmo.ISMDR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDR_Id;
                        }

                        dmo.MI_Id = data.MI_Id;
                        dmo.ISMTPLAP_Id = getapprovedplanner.FirstOrDefault().ISMTPLAP_Id;
                        dmo.HRME_Id = empid;
                        dmo.ISMTPL_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;

                        if (data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id > 0)
                        {
                            dmo.ISMTPLTA_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id;
                        }
                        else
                        {
                            dmo.ISMTPLTA_Id = dmo_task.ISMTPLTA_Id;
                        }

                        dmo.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        dmo.ISMDRPT_Date = data.ISMDRPT_Date;
                        dmo.ISMDRPT_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                        dmo.ISMDRPT_TimeTakenInHrs = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;
                        dmo.ISMDRPT_TimeTakenFlg = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrsmin;
                        dmo.ISMDRPT_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        dmo.ISMDRPT_CompoffApplicableflag = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_CompoffApplicableflag;
                        dmo.ISMDRPT_DeviationFlg = data.ISMDRPT_DeviationFlg;
                        dmo.ISMDRPT_ExtraFlg = data.ISMDRPT_ExtraFlg;
                        dmo.ISMDRPT_OrdersDateFlg = data.ISMDRPT_OrdersDateFlg;
                        dmo.ISMDRPT_OthersDateReason = data.ISMDRPT_OthersDateReason;
                        dmo.ISMDRPT_ActiveFlg = true;
                        dmo.ISMDRPT_BlockedFlag = data.ISMDRPT_BlockedFlag;
                        dmo.ISMDRPT_JeotaggedPhoto = data.ISMDRPT_JeotaggedPhoto;
                        dmo.ISMDRPT_JeotaggedPhotoFath = data.ISMDRPT_JeotaggedPhotoFath;
                        dmo.ISMDRPT_LocationLatLogValue = data.ISMDRPT_LocationLatLogValue;
                        dmo.ISMDRPT_LocationName = data.ISMDRPT_LocationName;
                        dmo.CreatedDate = indiantime0;
                        dmo.UpdatedDate = indiantime0;
                        dmo.ISMDRPT_CreatedBy = data.UserId;
                        dmo.ISMDRPT_UpdatedBy = data.UserId;
                        dmo.ISMDRPT_HalfDayFlag = data.ISMDRPT_HalfDayFlag;
                        _issueManagerContext.Add(dmo);
                    }

                    #endregion


                    // Adding Check List Files Or Comments
                    #region Task Wise Check List (Maker Checker Details)

                    if (data.Temp_ISM_DailyReportGenerationDTO[k].categorychecklist != null && data.Temp_ISM_DailyReportGenerationDTO[k].categorychecklist.Length > 0)
                    {
                        if (get_previous_dr_taskdetails.Count > 0)
                        {
                            Array Remove_files = _issueManagerContext.ISM_DailyReport_FilesDMO.Where(a => a.ISMDRPT_Id == get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id).ToArray();

                            foreach (var c in Remove_files)
                            {
                                _issueManagerContext.Remove(c);
                            }
                        }



                        foreach (var c in data.Temp_ISM_DailyReportGenerationDTO[k].categorychecklist)
                        {
                            ISM_DailyReport_FilesDMO iSM_DailyReport_FilesDMO = new ISM_DailyReport_FilesDMO();
                            if (previous_timetaken == 0.00m)
                            {
                                iSM_DailyReport_FilesDMO.ISMDRPT_Id = dmo.ISMDRPT_Id;
                            }
                            else
                            {
                                iSM_DailyReport_FilesDMO.ISMDRPT_Id = get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id;
                            }

                            iSM_DailyReport_FilesDMO.ISMMTCATCL_Id = c.ISMMTCATCL_Id;
                            iSM_DailyReport_FilesDMO.ISMDRPTFL_FileName = c.filename;
                            iSM_DailyReport_FilesDMO.ISMDRPTFL_RefNo = c.refno;
                            iSM_DailyReport_FilesDMO.ISMDRPTFL_Remarks = c.comments;
                            iSM_DailyReport_FilesDMO.ISMDRPT_ActiveFlg = true;
                            iSM_DailyReport_FilesDMO.ISMDRPT_CreatedDate = indiantime0;
                            iSM_DailyReport_FilesDMO.ISMDRPT_UpdatedDate = indiantime0;
                            iSM_DailyReport_FilesDMO.ISMDRPT_CreatedBy = data.UserId;
                            iSM_DailyReport_FilesDMO.ISMDRPT_UpdatedBy = data.UserId;
                            iSM_DailyReport_FilesDMO.ISMDRPTFL_FilePath = c.filepath;
                            _issueManagerContext.Add(iSM_DailyReport_FilesDMO);

                        }
                    }
                    #endregion


                    //*********** Approved Task Details Inserting ********//
                    #region Insert Into Planner Approved Task Details For Extra Task

                    if (getextraflag == 1 && previous_timetaken == 0.00m)
                    {
                        ISM_Task_Planner_Approved_TasksDMO approvedtasklist = new ISM_Task_Planner_Approved_TasksDMO();

                        approvedtasklist.ISMTPLAP_Id = getapprovedplanner.FirstOrDefault().ISMTPLAP_Id;
                        approvedtasklist.ISMTCR_Id = gettaskid;
                        approvedtasklist.ISMTPLAPTA_Remarks = remarks;
                        approvedtasklist.ISMTPLAPTA_StartDate = startdate;
                        approvedtasklist.ISMTPLAPTA_EndDate = enddate;
                        approvedtasklist.ISMTPLAPTA_EffortInHrs = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs;
                        approvedtasklist.ISMTPLAPTA_FinishedDate = data.ISMDRPT_Date;
                        approvedtasklist.ISMTPLAPTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        approvedtasklist.ISMTPLAPTA_DeviationFlg = data.Temp_ISM_DailyReportGenerationDTO[k].deviationflag;
                        approvedtasklist.ISMTPLAPTA_ExtraTaskFlg = true;
                        approvedtasklist.ISMTPLAPTA_TimeTakenInHrs = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs;
                        approvedtasklist.ISMTPLAPTA_RejectedFlg = false;
                        approvedtasklist.ISMTPLAPTA_ActiveFlg = true;
                        approvedtasklist.CreatedDate = indiantime0;
                        approvedtasklist.UpdatedDate = indiantime0;
                        approvedtasklist.ISMTPLAPTA_CreatedBy = data.UserId;
                        approvedtasklist.ISMTPLAPTA_UpdatedBy = data.UserId;
                        approvedtasklist.ISMTPLAPTA_ApprovalFlg = true;
                        approvedtasklist.ISMTPLTA_Id = dmo_task.ISMTPLTA_Id;
                        _issueManagerContext.Add(approvedtasklist);
                    }

                    #endregion


                    // ***************** Adding Task To Assigned Task ***************** //
                    #region Adding Assigned Table

                    var checktaskisassigned = _issueManagerContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == empid
                    && a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                    if (checktaskisassigned.Count() == 0)
                    {
                        ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                        dmo_task_assigned.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        dmo_task_assigned.HRME_Id = empid;
                        dmo_task_assigned.ISMTCRASTO_AssignedDate = indiantime0;
                        dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                        dmo_task_assigned.ISMTCRASTO_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Remarks;
                        dmo_task_assigned.ISMTCRASTO_StartDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_StartDate;
                        dmo_task_assigned.ISMTCRASTO_EndDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EndDate;
                        dmo_task_assigned.ISMTCRASTO_EffortInHrs = Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs);
                        dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                        dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                        dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                        dmo_task_assigned.CreatedDate = indiantime0;
                        dmo_task_assigned.UpdatedDate = indiantime0;

                        _issueManagerContext.Add(dmo_task_assigned);
                    }
                    #endregion


                    // ******************** Status Updation In Task Creation ******************** //
                    #region Updating Task Creation Table

                    var checktaskcreation = _issueManagerContext.ISM_TaskCreationDMO.Where(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id).ToList();

                    if (checktaskcreation.Count() > 0)
                    {
                        var checktaskcreationresult = _issueManagerContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id);

                        checktaskcreationresult.ISMTCR_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        checktaskcreationresult.ISMTCR_UpdatedBy = data.UserId;
                        checktaskcreationresult.UpdatedDate = indiantime0;
                        _issueManagerContext.Update(checktaskcreationresult);
                    }
                    #endregion


                    // ******************** Status Updation In Task Creation Response ******************** //
                    #region Task Creation Response Table

                    if (data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList != null)
                    {
                        foreach (var taskatt in data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList)
                        {
                            ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();
                            taskresponse.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                            taskresponse.HRME_Id = empid;
                            taskresponse.ISMTCRRES_ResponseDate = indiantime0;
                            taskresponse.ISMTCRRES_Response = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                            taskresponse.ISMTCRRES_ResponseAttachment = taskatt.filepath;
                            taskresponse.ISMTCRRES_ResponsePageName = "Daily Report";
                            taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                            taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                            taskresponse.ISMTCRRES_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                            taskresponse.ISMTCRRES_ActiveFlg = true;
                            taskresponse.CreatedDate = indiantime0;
                            taskresponse.UpdatedDate = indiantime0;
                            _issueManagerContext.Add(taskresponse);
                        }
                    }
                    else
                    {
                        ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();

                        taskresponse.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        taskresponse.HRME_Id = empid;
                        taskresponse.ISMTCRRES_ResponseDate = indiantime0;
                        taskresponse.ISMTCRRES_Response = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                        taskresponse.ISMTCRRES_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        taskresponse.ISMTCRRES_ResponseAttachment = "";
                        taskresponse.ISMTCRRES_ResponsePageName = "Daily Report";
                        taskresponse.ISMTCRRES_ActiveFlg = true;
                        taskresponse.CreatedDate = indiantime0;
                        taskresponse.UpdatedDate = indiantime0;
                        taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                        taskresponse.ISMTCRRES_UpdatedBy = data.UserId;

                        _issueManagerContext.Add(taskresponse);

                    }
                    #endregion


                    // ******************** INSERTING INTO DEVAITION TABLE ******************** //
                    #region Insert Into Deveiation Table

                    if (data.ISMDRPT_DeviationFlg == true)
                    {
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime toatecon = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                        ISM_Task_Planner_DeviationDMO deviationdmo = new ISM_Task_Planner_DeviationDMO();

                        deviationdmo.ISMTPL_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;
                        deviationdmo.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        deviationdmo.HRME_Id = empid;
                        deviationdmo.ISMTPLDE_StartDate = Convert.ToDateTime(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_StartDate);
                        deviationdmo.ISMTPLDE_EndDate = Convert.ToDateTime(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EndDate);
                        deviationdmo.ISMTPLDE_FinishedDate = toatecon;
                        deviationdmo.ISMTPLDE_DeviationReason = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                        deviationdmo.ISMTPLDE_ActiveFlg = true;
                        deviationdmo.CreatedDate = toatecon;
                        deviationdmo.UpdatedDate = toatecon;
                        deviationdmo.ISMTPLDE_CreatedBy = data.UserId;
                        deviationdmo.ISMTPLDE_UpdatedBy = data.UserId;

                        deviationdmo.ISMDR_Id = Convert.ToInt64(data.Temp_ISM_DailyReportGenerationDTO[k].ISMDR_Id);

                        //deviationdmo.ISMDR_Id =Convert.ToInt64(data.Temp_ISM_DailyReportGenerationDTO[k].ISMDR_Id);

                        deviationdmo.ISMTPLDE_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        _issueManagerContext.Add(deviationdmo);
                    }
                    #endregion
                }

                //***************** Planner Total Hours Updation If They Add Extra Work ***************//
                var plannerresultlist = _issueManagerContext.ISM_Task_PlannerDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMTPL_Id == data.ISMTPL_Id).ToList();
                decimal totalhrslist = plannerresultlist.FirstOrDefault().ISMTPL_TotalHrs;
                totalhrslist = totalhrslist + Convert.ToDecimal(totalhrs);

                var plannerresult = _issueManagerContext.ISM_Task_PlannerDMO.Single(a => a.MI_Id == data.MI_Id && a.ISMTPL_Id == data.ISMTPL_Id);
                plannerresult.ISMTPL_TotalHrs = totalhrslist;
                _issueManagerContext.Update(plannerresult);

                //***************** Planner Approved Total Hours Updation If They Add Extra Work ***************//
                var plannerlistapproved = _issueManagerContext.ISM_Task_Planner_ApprovedDMO.Where(a => a.ISMTPL_Id == data.ISMTPL_Id).ToList();
                decimal? totalhrslistapproved = plannerlistapproved.FirstOrDefault().ISMTPLAP_TotalHrs;
                totalhrslistapproved = totalhrslistapproved + Convert.ToDecimal(totalhrs);

                var plannerlistapprovedresult = _issueManagerContext.ISM_Task_Planner_ApprovedDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                plannerlistapprovedresult.ISMTPLAP_TotalHrs = totalhrslistapproved;
                _issueManagerContext.Update(plannerlistapprovedresult);

                var i = _issueManagerContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                    counttrue = 1;

                    var count = 0;
                    for (int k2 = 0; k2 < data.Temp_ISM_DailyReportGenerationDTO.Length; k2++)
                    {
                        var getextraflag = data.Temp_ISM_DailyReportGenerationDTO[k2].extraflag;
                        var gettaskid = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMTCR_Id;
                        var gettaskstatus = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMDRPT_Status;
                        var remarks = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMDRPT_Remarks;
                        var startdate = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMTPLTA_StartDate;
                        var enddate = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMTPLTA_EndDate;
                        var periodicityflag = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMTAPL_Periodicity;

                        periodicityflag = periodicityflag == null || periodicityflag == "" ? "" : periodicityflag.ToUpper();

                        if ((periodicityflag == "SPECIFIC DAY" || periodicityflag == "DAILY" || periodicityflag == "MONTHLY"
                            || periodicityflag == "YEARLY ONCE" || periodicityflag == "WEEKLY" || periodicityflag == "ONCE IN FORTNIGHT"))
                        {
                            DateTime fromdatecon12 = DateTime.Now;
                            string confromdate12 = "";

                            fromdatecon12 = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                            confromdate12 = fromdatecon12.ToString("yyyy-MM-dd");

                            count += 1;

                            var Get_Task_CountIds = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == gettaskid
                            && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ApprovalFlg == true).Count();

                            var Get_Task_Completed_CountIds = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == gettaskid
                             && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ApprovalFlg == true && (a.ISMTPLTA_Status != "Open" && a.ISMTPLTA_Status != "ReOpen")).Count();

                            var UpdatePeriodicityStatus = _issueManagerContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == gettaskid);

                            if (Get_Task_CountIds == Get_Task_Completed_CountIds)
                            {
                                //UpdatePeriodicityStatus.ISMTCR_Status = "Completed";

                                var outputval = _issueManagerContext.Database.ExecuteSqlCommand("ISM_TaskCreationStatus_Update  @p0,@p1,@p2", empid,
                                    data.ISMTPL_Id, confromdate12);
                                if (outputval > 0)
                                {
                                    Console.WriteLine(" Task Creation Status Update DR " + outputval);
                                }
                                else
                                {
                                    Console.WriteLine(" Task Creation Status Update DR " + outputval);
                                }
                            }
                            else
                            {
                                UpdatePeriodicityStatus.ISMTCR_Status = "Open";
                                UpdatePeriodicityStatus.ISMTCR_UpdatedBy = data.UserId;
                                UpdatePeriodicityStatus.UpdatedDate = indiantime0;
                                _issueManagerContext.Update(UpdatePeriodicityStatus);
                            }
                        }
                    }

                    if (count > 0)
                    {
                        try
                        {
                            var j2 = _issueManagerContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //if (plannerresultlist.FirstOrDefault().ISMTPL_EndDate.Value.Date == indiantime0.Date && data.totalworkinghrsflag == 1)
                    //{
                    DateTime fromdatecon123 = DateTime.Now;
                    string confromdate123 = "";

                    fromdatecon123 = Convert.ToDateTime(indiantime0.Date.ToString("yyyy-MM-dd"));
                    confromdate123 = fromdatecon123.ToString("yyyy-MM-dd");

                    try
                    {
                        var outputval = _issueManagerContext.Database.ExecuteSqlCommand("PlannerAddedTasksDetetion  @p0,@p1", empid, confromdate123);
                        if (outputval > 0)
                        {
                            Console.WriteLine(" Planner Updated outputval DR " + outputval);
                        }
                        else
                        {
                            Console.WriteLine(" Planner Not Updated outputval DR " + outputval);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //}

                    if (ExtraCountTask > 0)
                    {
                        DateTime fromdatecon2 = DateTime.Now;
                        string confromdate2 = "";

                        fromdatecon2 = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                        confromdate2 = fromdatecon2.ToString("yyyy-MM-dd");
                        try
                        {
                            var outputval1 = _issueManagerContext.Database.ExecuteSqlCommand("ISM_DRCategoryRecord_Insert  @p0,@p1,@p2", empid,
                            fromdatecon2, plannerresultlist.FirstOrDefault().ISMTPL_Id);

                            if (outputval1 > 0)
                            {
                                Console.WriteLine(" Task Category Updated outputval1 DR " + outputval1);
                            }
                            else
                            {
                                Console.WriteLine(" Task Category Not Updated outputval1 DR " + outputval1);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    // ============================================== Push Notification
                    var getemployeedetails = (from a in _issueManagerContext.MasterEmployee
                                              from b in _issueManagerContext.HR_Master_Department
                                              from c in _issueManagerContext.HR_Master_Designation

                                              from e in _issueManagerContext.HR_Master_Grade
                                              where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                                              && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
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

                    //*********************************** User Employee Mapped List ********************************// 
                    var notificationuserids = (from UEM in _issueManagerContext.ISM_User_Employees_MappingDMO
                                               from HRE in _issueManagerContext.MasterEmployee
                                               where (UEM.HRME_Id == HRE.HRME_Id && HRE.HRME_ActiveFlag == true && HRE.HRME_LeftFlag == false && UEM.HRME_Id == empid)
                                               select new ISM_DailyReportGenerationDTO
                                               {
                                                   UserId = UEM.User_Id,
                                               }).Distinct().ToList();

                    List<long> userids = new List<long>();
                    foreach (var n in notificationuserids)
                    {
                        userids.Add(n.UserId);
                    }

                    var notificationdetails = (from SUL in _issueManagerContext.Staff_User_Login
                                               where (userids.Contains(SUL.Id))
                                               select new ISM_DailyReportGenerationDTO
                                               {
                                                   HRME_Id = SUL.Emp_Code,
                                               }).Distinct().ToList();

                    var deviceidsnew = "";
                    var devicenew = "";
                    if (notificationdetails.Count > 0)
                    {
                        data.notificationUser = notificationdetails.ToArray();
                        List<long> employeeids = new List<long>();
                        foreach (var n in notificationdetails)
                        {
                            employeeids.Add(n.HRME_Id);
                        }

                        var webnotification = (from a in _issueManagerContext.MasterEmployee
                                               where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id))
                                               select new ISM_DailyReportGenerationDTO
                                               {
                                                   HRME_Id = a.HRME_Id
                                               }).Distinct().ToList();

                        if (webnotification.Count > 0)
                        {
                            PushNotification pb = new PushNotification(_db);
                            string dd = pb.savewebnotification(data.MI_Id, "DRWEB", data.ISMDRPT_Id, data.ISMDRPT_Id, data.HRME_Id, data.UserId, "DR");
                        }


                        var devicelist = (from a in _issueManagerContext.MasterEmployee
                                          where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id))
                                          select new ISM_DailyReportGenerationDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                          }).Distinct().ToList();

                        data.deviceArray = devicelist.ToArray();
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
                    }
                    try
                    {
                        if (data.sound == "")
                        {
                            data.sound = "default";
                        }
                        callnotification(devicenew, empid, indiantime0, getemployeedetails.FirstOrDefault().employeename, data.MI_Id, data.sound);
                        Insertnotification(data.MI_Id, empid, indiantime0, getemployeedetails.FirstOrDefault().employeename, data.UserId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    #region Email                
                    //string str = "";

                    //DateTime fromdatecon = DateTime.Now;

                    //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    //DateTime toatecon = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    //string confromdate = "";
                    //string contodate = "";
                    //string datetime = "";
                    //try
                    //{
                    //    fromdatecon = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                    //    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    //    contodate = toatecon.ToString("dd/MM/yyyy");

                    //    datetime = toatecon.ToString("dd/MM/yyyy hh:mm tt");
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                    //List<ISM_DailyReportGenerationDTO> dtonewtemp = new List<ISM_DailyReportGenerationDTO>();


                    //// Getting The saved Data
                    //using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "ISM_DailyReport_Generation_Search";
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = data.MI_Id
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = empid
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                    //    {
                    //        Value = confromdate
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    //    {
                    //        Value = 1
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
                    //                dtonewtemp.Add(new ISM_DailyReportGenerationDTO
                    //                {
                    //                    ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                    //                    HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                    //                    ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                    //                    ISMDRPT_Statusnew = Convert.ToString(dataReader["ISMDRPT_Status"]),
                    //                    ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                    //                    ISMDRPT_TimeTakenInHrsnew = Convert.ToString(dataReader["ISMDRPT_TimeTakenInHrs"]),
                    //                    ISMMCLT_ClientName = Convert.ToString(dataReader["ISMMCLT_ClientName"]),
                    //                    assignedby = Convert.ToString(dataReader["assignedby"]),
                    //                    ISMDRPT_Datenew = Convert.ToDateTime(dataReader["ISMDRPT_Date"]),
                    //                });
                    //            }
                    //        }

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //}

                    //// Calculate Percentage
                    //List<ISM_DailyReportGenerationDTO> result = new List<ISM_DailyReportGenerationDTO>();

                    //using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "ISM_DailyReport_Generation_Deviation_Calculation";
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = data.MI_Id
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = empid
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                    //    {
                    //        Value = confromdate
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    //    {
                    //        Value = 1
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
                    //                result.Add(new ISM_DailyReportGenerationDTO
                    //                {
                    //                    completedper = Convert.ToDecimal(dataReader["per"]),
                    //                    deviationper = Convert.ToDecimal(dataReader["deviation"]),
                    //                });
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //}

                    //string emailccdetails = "";
                    //string emailbccdetails = "";

                    //var emaildetails = _issueManagerContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id
                    //&& a.HRMD_Id == getemployeedetails.FirstOrDefault().HRMD_Id).ToList();

                    //if (emaildetails.Count > 0)
                    //{
                    //    emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                    //    emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                    //}
                    //else
                    //{
                    //    emailccdetails = "";
                    //    emailbccdetails = "";
                    //}

                    //var checktemplate = _issueManagerContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Daily_Report_Generation"
                    //&& a.ISES_MailActiveFlag == true).ToList();

                    //if (checktemplate.Count == 0)
                    //{
                    //    data.message = "Email Template not Mapped to the selected Institution";
                    //}
                    //else
                    //{
                    //    var emplname = getemployeedetails.FirstOrDefault().employeename;
                    //    var deptname = getemployeedetails.FirstOrDefault().department;
                    //    var desginame = getemployeedetails.FirstOrDefault().desgination;
                    //    var gradname = getemployeedetails.FirstOrDefault().gradename;
                    //    var email = getemployeedetails.FirstOrDefault().emailid;
                    //    var date = contodate;
                    //    var newdate = datetime;

                    //    decimal? completedperc = 0;
                    //    decimal? deviationperc = 0;

                    //    if (result.Count() > 0)
                    //    {
                    //        completedperc = result.FirstOrDefault().completedper;
                    //        deviationperc = result.FirstOrDefault().deviationper;
                    //    }
                    //    else
                    //    {
                    //        completedperc = 0;
                    //        deviationperc = 100;
                    //    }

                    //    string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                    //    templatedetails = templatedetails.Replace("[DeveloperName]", emplname);
                    //    templatedetails = templatedetails.Replace("[DATE]", date);
                    //    templatedetails = templatedetails.Replace("[Department]", deptname);
                    //    templatedetails = templatedetails.Replace("[Designation]", gradname);
                    //    templatedetails = templatedetails.Replace("[CompletedPer]", completedperc.ToString());
                    //    templatedetails = templatedetails.Replace("[Pendingwork]", deviationperc.ToString());
                    //    templatedetails = templatedetails.Replace("[DATENAME]", newdate);

                    //    int rownumber = 0;

                    //    if (dtonewtemp.Count > 0)
                    //    {
                    //        for (int j = 0; j < dtonewtemp.Count; j++)
                    //        {
                    //            rownumber += 1;
                    //            str += "<tr>" +
                    //                "<td align=" + "center" + " style=" + ">" + rownumber + "</td> " +
                    //                "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_BugOREnhancementFlg + "</td>" +
                    //                "<td align=" + "left" + ">" + dtonewtemp[j].ISMMCLT_ClientName + "</td>" +
                    //                "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_TaskNo + "</td> " +
                    //                "<td align=" + "left" + ">" + dtonewtemp[j].HRMP_Name + "</td> " +
                    //                "<td align=" + "left" + "style=" + "word-break: break-word;" + ">" + dtonewtemp[j].ISMTCR_Desc + "</td> " +
                    //                "<td align=" + "left" + ">" + dtonewtemp[j].ISMDRPT_Statusnew + "</td> " +
                    //                "<td align=" + "left" + ">" + dtonewtemp[j].assignedby + "</td> " +
                    //                "<td align=" + "left" + ">" + contodate + "</td> " +
                    //                "<td align=" + "left" + ">" + dtonewtemp[j].ISMDRPT_TimeTakenInHrsnew + "</td> " +
                    //                "</tr>"
                    //                ;
                    //        }
                    //        // str += "</table>";
                    //    }

                    //    templatedetails = templatedetails.Replace("[MSG]", str);

                    //    try
                    //    {
                    //        sendmailexamtt(data.MI_Id, email, templatedetails, emailccdetails, emailbccdetails);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //        _logger.LogInformation("Daily Report Generation generatedailyreport EMAIL For Employee Id : " + empid + "   : " + ex.Message);
                    //    }
                    //}
                    //data.returnval = true;
                    #endregion
                }
                else
                {
                    data.returnval = false;
                }

                #region Deviation
                //if (data.deviation_id == 1)
                //{

                //    var hrme = _issueManagerContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);

                //    string hrmeids = "0";

                //    hrmeids = hrmeids + "," + hrme.HRME_Id;

                //    List<ISM_Weekly_Deviation_DTO> wd = new List<ISM_Weekly_Deviation_DTO>();
                //    ISM_Weekly_Deviation_DTO wd1 = new ISM_Weekly_Deviation_DTO();
                //    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "ISM_Weekly_DeviationReport_Calculation";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //        {
                //            Value = data.MI_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                //        SqlDbType.VarChar)
                //        {
                //            Value = hrmeids
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@StartDate",
                //       SqlDbType.DateTime)
                //        {
                //            Value = data.startweek
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@EndDate",
                //        SqlDbType.DateTime)
                //        {
                //            Value = data.endweek
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@Flag",
                //        SqlDbType.VarChar)
                //        {
                //            Value = 1
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
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                        );
                //                    }
                //                    retObject.Add((ExpandoObject)dataRow);
                //                }
                //            }
                //            wd1.deviation_ary = retObject.ToArray();

                //            var deviation = _issueManagerContext.ISM_Weekly_Deviation_DMO_con.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                //            if (deviation.ISMWD_Deviation >= 20 && deviation.ISMWD_Deviation <= 40)
                //            {
                //                var checktemplate = _issueManagerContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Memo_Generation"
                //       && a.ISES_MailActiveFlag == true).ToList();

                //                if (checktemplate.Count == 0)
                //                {
                //                    data.message = "Email Template not Mapped to the selected Institution";
                //                }
                //                else
                //                {
                //                    var getemployeedetails1 = (from a in _issueManagerContext.MasterEmployee
                //                                               from b in _issueManagerContext.HR_Master_Department
                //                                               from c in _issueManagerContext.HR_Master_Designation

                //                                               from e in _issueManagerContext.HR_Master_Grade
                //                                               where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                //                                               && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
                //                                               select new ISM_DailyReportGenerationDTO
                //                                               {
                //                                                   employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                //                                                   + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                //                                                   + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                //                                                   department = b.HRMD_DepartmentName,
                //                                                   desgination = c.HRMDES_DesignationName,
                //                                                   gradename = e.HRMG_GradeName,
                //                                                   HRMD_Id = a.HRMD_Id,
                //                                                   emailid = a.HRME_EmailId
                //                                               }).Distinct().ToList();
                //                    var emplname = getemployeedetails1.FirstOrDefault().employeename;
                //                    var deptname = getemployeedetails1.FirstOrDefault().department;
                //                    var desginame = getemployeedetails1.FirstOrDefault().desgination;
                //                    var gradname = getemployeedetails1.FirstOrDefault().gradename;
                //                    var email = getemployeedetails1.FirstOrDefault().emailid;

                //                    string templatedetails1 = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                //                    string date = "";
                //                    DateTime fromdatecon = DateTime.Now;
                //                    fromdatecon = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd"));
                //                    date = fromdatecon.ToString("yyyy-MM-dd");

                //                    //===================bcc mail===========
                //                    string emailccdetails = "";
                //                    string emailbccdetails = "";
                //                    var emaildetails = _issueManagerContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == getemployeedetails1.FirstOrDefault().HRMD_Id).ToList();
                //                    if (emaildetails.Count > 0)
                //                    {
                //                        emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                //                        emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                //                    }
                //                    else
                //                    {
                //                        emailccdetails = "";
                //                        emailbccdetails = "";
                //                    }
                //                    //==============================bccmailend=======
                //                    //=================getdate to mail==========

                //                    templatedetails1 = templatedetails1.Replace("[DeveloperName]", emplname);
                //                    templatedetails1 = templatedetails1.Replace("[Department]", deptname);
                //                    templatedetails1 = templatedetails1.Replace("[Designation]", gradname);
                //                    templatedetails1 = templatedetails1.Replace("[Deviation]", Convert.ToDecimal(deviation.ISMWD_Deviation).ToString());
                //                    templatedetails1 = templatedetails1.Replace("[DATENAME]", date);


                //                    try
                //                    {
                //                        send_memo_deviation(data.MI_Id, email, templatedetails1, emailccdetails);
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        Console.WriteLine(ex.Message);
                //                        _logger.LogInformation("Daily Report Generation generatedailyreport EMAIL For Employee Id : " + empid + "   : " + ex.Message);
                //                    }
                //                    data.returnval = true;
                //                }
                //            }
                //            else if (deviation.ISMWD_Deviation >= 40)
                //            {
                //                var checktemplate = _issueManagerContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Notice_Generation"
                //                                  && a.ISES_MailActiveFlag == true).ToList();

                //                if (checktemplate.Count == 0)
                //                {
                //                    data.message = "Email Template not Mapped to the selected Institution";
                //                }
                //                else
                //                {
                //                    var getemployeedetails1 = (from a in _issueManagerContext.MasterEmployee
                //                                               from b in _issueManagerContext.HR_Master_Department
                //                                               from c in _issueManagerContext.HR_Master_Designation

                //                                               from e in _issueManagerContext.HR_Master_Grade
                //                                               where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                //                                               && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
                //                                               select new ISM_DailyReportGenerationDTO
                //                                               {
                //                                                   employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                //                                                   + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                //                                                   + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                //                                                   department = b.HRMD_DepartmentName,
                //                                                   desgination = c.HRMDES_DesignationName,
                //                                                   gradename = e.HRMG_GradeName,
                //                                                   HRMD_Id = a.HRMD_Id,
                //                                                   emailid = a.HRME_EmailId
                //                                               }).Distinct().ToList();
                //                    var emplname = getemployeedetails1.FirstOrDefault().employeename;
                //                    var deptname = getemployeedetails1.FirstOrDefault().department;
                //                    var desginame = getemployeedetails1.FirstOrDefault().desgination;
                //                    var gradname = getemployeedetails1.FirstOrDefault().gradename;
                //                    var email = getemployeedetails1.FirstOrDefault().emailid;

                //                    string templatedetails1 = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                //                    string date = "";
                //                    DateTime fromdatecon = DateTime.Now;
                //                    fromdatecon = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd"));
                //                    date = fromdatecon.ToString("yyyy-MM-dd");

                //                    //===================bcc mail===========
                //                    string emailccdetails = "";
                //                    string emailbccdetails = "";
                //                    string name_cc = "";
                //                    string name_bcc = "";

                //                    var emaildetails = _issueManagerContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == getemployeedetails1.FirstOrDefault().HRMD_Id).ToList();
                //                    if (emaildetails.Count > 0)
                //                    {
                //                        emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                //                        emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                //                    }
                //                    else
                //                    {
                //                        emailccdetails = "";
                //                        emailbccdetails = "";
                //                    }
                //                    //==============================bccmailend=======
                //                    //=================getdate to mail==========

                //                    templatedetails1 = templatedetails1.Replace("[DeveloperName]", emplname);
                //                    templatedetails1 = templatedetails1.Replace("[Department]", deptname);
                //                    templatedetails1 = templatedetails1.Replace("[Designation]", gradname);
                //                    templatedetails1 = templatedetails1.Replace("[Deviation]", Convert.ToDecimal(deviation.ISMWD_Deviation).ToString());
                //                    templatedetails1 = templatedetails1.Replace("[DATENAME]", date);
                //                    templatedetails1 = templatedetails1.Replace("[CCTo]", emailccdetails);


                //                    try
                //                    {
                //                        send_notice_deviation(data.MI_Id, email, templatedetails1, emailccdetails);
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        Console.WriteLine(ex.Message);
                //                        _logger.LogInformation("Daily Report Generation generatedailyreport EMAIL For Employee Id : " + empid + "   : " + ex.Message);
                //                    }
                //                    data.returnval = true;
                //                }
                //            }
                //        }

                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }

                //    data.returnval = true;
                //}

                #endregion Deviation

            }
            catch (Exception ex)
            {
                if (counttrue == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Daily Report Generation generatedailyreport For Employee Id : " + empid + "   : " + ex.Message);
            }
            return data;
        }

        /* SEARCH PREVIOUS DR REPORT*/
        public ISM_DailyReportGenerationDTO searchpreviousdailyreport(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;
            try
            {

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generation_Search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
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
                        data.getsearchdailyreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.droveralldetailsdatewise = _issueManagerContext.ISM_DailyReport_DaywiseDMO.Where(a => a.HRME_Id == empid
                && a.ISMDRPTDW_Date.Value.Date == data.ISMDRPT_Date.Value.Date).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Daily Report Generation searchpreviousdailyreport For Employee Id : " + empid + "   : " + ex.Message);
            }
            return data;
        }

        /* ADD EXTRA TASK TO DR*/
        public ISM_DailyReportGenerationDTO addextratask(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;

            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generation_Search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = 2
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
                        data.getextratasks = retObject.ToArray();
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
                _logger.LogInformation("Daily Report Generation addextratask For Employee Id : " + empid + "   : " + ex.Message);
            }
            return data;
        }

        /* VIEW TASK WISE RESPONSE */
        public ISM_DailyReportGenerationDTO viewresponse(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
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

                    //cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    //{
                    //    Value = "1"
                    //});

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

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
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

                    //cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    //{
                    //    Value = "2"
                    //});

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

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
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

                    //cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    //{
                    //    Value = "3"
                    //});

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

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
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

                    //cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    //{
                    //    Value = "4"
                    //});

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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* SAVE DR NOT SENT REMARKS */
        public ISM_DailyReportGenerationDTO SaveDRNotSentRemarks(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.SaveDRNotSentRemarksDetails != null && data.SaveDRNotSentRemarksDetails.Length > 0)
                {
                    foreach (var c in data.SaveDRNotSentRemarksDetails)
                    {
                        ISM_DailyReportNotSent_DateWiseRemarksDMO iSM_DailyReportNotSent_DateWiseRemarksDMO = new ISM_DailyReportNotSent_DateWiseRemarksDMO();
                        iSM_DailyReportNotSent_DateWiseRemarksDMO.MI_Id = data.MI_Id;
                        iSM_DailyReportNotSent_DateWiseRemarksDMO.HRME_Id = hrmEempid.FirstOrDefault().Emp_Code;
                        iSM_DailyReportNotSent_DateWiseRemarksDMO.ISMDRNSDR_DRNotSentDate = c.drnotsentdate;
                        iSM_DailyReportNotSent_DateWiseRemarksDMO.ISMDRNSDR_RemarksDate = data.ISMDRNSDR_RemarksDate;
                        iSM_DailyReportNotSent_DateWiseRemarksDMO.ISMDRNSDR_Remarks = c.remarks;
                        iSM_DailyReportNotSent_DateWiseRemarksDMO.ISMDRNSDR_TemplateName = c.Template;
                        iSM_DailyReportNotSent_DateWiseRemarksDMO.CreatedDate = indiantime0;
                        iSM_DailyReportNotSent_DateWiseRemarksDMO.UpdatedDate = indiantime0;
                        _issueManagerContext.Add(iSM_DailyReportNotSent_DateWiseRemarksDMO);
                    }

                    var i = _issueManagerContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* VIEW CHECKLIST LIST SAVE DR */
        public ISM_DailyReportGenerationDTO ViewCheckListSavedDetails(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DailyReport_TaskCategory_Checklist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMDRPT_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMDRPT_Id
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
                        data.getchecklistarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //data.getchecklistarray = (from a in _issueManagerContext.ISM_DailyReport_FilesDMO
                //                          from b in _issueManagerContext.ISM_DailyReportGenerationDMO
                //                          from c in _issueManagerContext.ISM_Master_TaskCategory_ChecklistDMO_con
                //                          where (a.ISMDRPT_Id == b.ISMDRPT_Id && a.ISMMTCATCL_Id == c.ISMMTCATCL_Id && b.MI_Id == data.MI_Id
                //                          && a.ISMDRPT_Id == data.ISMDRPT_Id)
                //                          select new ISM_DailyReportGenerationDTO
                //                          {
                //                              ISMDRPTFL_FileName = a.ISMDRPTFL_FileName,
                //                              ISMDRPTFL_FilePath = a.ISMDRPTFL_FilePath,
                //                              ISMDRPTFL_RefNo = a.ISMDRPTFL_RefNo,
                //                              ISMDRPTFL_Remarks = a.ISMDRPTFL_Remarks,
                //                              ISMMTCATCL_Id = a.ISMMTCATCL_Id,
                //                              ISMDRPT_Id = a.ISMDRPT_Id,
                //                              ISMMTCATCL_CheckListName = c.ISMMTCATCL_CheckListName
                //                          }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* VIEW BLOCKED DETAILS LIST*/
        public ISM_DailyReportGenerationDTO ViewBlockedDetails(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                long empid = 0;

                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;

                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_BLOCK_DAILYREPORT_NOTIFICATION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = empid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_ID", SqlDbType.BigInt)
                    {
                        Value = data.ISMEMN_ID == null ? 0 : data.ISMEMN_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMBE_Id", SqlDbType.BigInt)
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
                        data.drblockeddetailslist = retObject.ToArray();
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

        /* Get Category Checklist Details  */
        public ISM_DailyReportGenerationDTO GetCategoryChecklistDetails(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                data.gettaskcategorychecklist = (from a in _issueManagerContext.ISM_Master_TaskCategory_ChecklistDMO_con
                                                 from b in _issueManagerContext.ISM_Master_TaskCategory_DMO
                                                 from c in _issueManagerContext.ISM_TaskCreationDMO
                                                 where (a.ISMMTCAT_Id == b.ISMMTCAT_Id && c.ISMMTCAT_Id == b.ISMMTCAT_Id && a.ISMMTCAT_Id == c.ISMMTCAT_Id
                                                 && a.ISMMTCATCL_ActiveFlag == true && a.ISMMTCAT_Id == data.ISMMTCAT_Id && c.ISMTCR_Id == data.ISMTCR_Id)
                                                 select a).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* Get Category DR Not Approved Details  */
        public ISM_DailyReportGenerationDTO ViewDRNotApprovedList(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                long empid = 0;
                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Checking_DailyReport_Approval_GivenOrNot_User_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = confromdate });

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
                        data.drnotapprovedmessageindividual = retObject.ToArray();
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

        //******** Email Sending *********//
        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _issueManagerContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Daily_Report_Generation"
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _issueManagerContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _issueManagerContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _issueManagerContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

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

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mail_id);


                        if (Attechement.Equals("1"))
                        {
                            var img = _issueManagerContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //******************* EMAIL BCC DETAILS ****************//

                        //if (emailbccdetails != null && emailbccdetails != "")
                        //{
                        //    string[] bccmaildetails = emailbccdetails.Split(',');

                        //    foreach (var bc in bccmaildetails)
                        //    {
                        //        message.AddBcc(bc);
                        //    }
                        //}


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
                            clientsmtp.UseDefaultCredentials = false;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _issueManagerContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }

                                //******************* EMAIL CC DETAILS ****************//

                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }

                                //******************* EMAIL BCC DETAILS ****************//

                                //if (emailbccdetails != null && emailbccdetails != "")
                                //{
                                //    string[] bccmaildetails = emailbccdetails.Split(',');

                                //    foreach (var c in bccmaildetails)
                                //    {
                                //        emailMessage.Bcc.Add(c.ToString());
                                //    }
                                //}

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


                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _issueManagerContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Daily_Report_Generation" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _issueManagerContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _issueManagerContext.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
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
                //return ex.Message;
            }
            return "success";

        }
        public string send_memo_deviation(long MI_Id, string mail_id, string url, string emailccdetails)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _issueManagerContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Memo_Generation"
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _issueManagerContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _issueManagerContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _issueManagerContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

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

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mail_id);


                        if (Attechement.Equals("1"))
                        {
                            var img = _issueManagerContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //******************* EMAIL BCC DETAILS ****************//

                        //if (emailbccdetails != null && emailbccdetails != "")
                        //{
                        //    string[] bccmaildetails = emailbccdetails.Split(',');

                        //    foreach (var bc in bccmaildetails)
                        //    {
                        //        message.AddBcc(bc);
                        //    }
                        //}


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
                            clientsmtp.UseDefaultCredentials = false;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _issueManagerContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }

                                //******************* EMAIL CC DETAILS ****************//

                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }

                                //******************* EMAIL BCC DETAILS ****************//

                                //if (emailbccdetails != null && emailbccdetails != "")
                                //{
                                //    string[] bccmaildetails = emailbccdetails.Split(',');

                                //    foreach (var c in bccmaildetails)
                                //    {
                                //        emailMessage.Bcc.Add(c.ToString());
                                //    }
                                //}

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


                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _issueManagerContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Daily_Report_Generation" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _issueManagerContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _issueManagerContext.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
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
                //return ex.Message;
            }
            return "success";

        }
        public string send_notice_deviation(long MI_Id, string mail_id, string url, string emailccdetails)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _issueManagerContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Notice_Generation"
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _issueManagerContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _issueManagerContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _issueManagerContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

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

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mail_id);


                        if (Attechement.Equals("1"))
                        {
                            var img = _issueManagerContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //******************* EMAIL BCC DETAILS ****************//

                        //if (emailbccdetails != null && emailbccdetails != "")
                        //{
                        //    string[] bccmaildetails = emailbccdetails.Split(',');

                        //    foreach (var bc in bccmaildetails)
                        //    {
                        //        message.AddBcc(bc);
                        //    }
                        //}


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
                            clientsmtp.UseDefaultCredentials = false;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _issueManagerContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }

                                //******************* EMAIL CC DETAILS ****************//

                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }

                                //******************* EMAIL BCC DETAILS ****************//

                                //if (emailbccdetails != null && emailbccdetails != "")
                                //{
                                //    string[] bccmaildetails = emailbccdetails.Split(',');

                                //    foreach (var c in bccmaildetails)
                                //    {
                                //        emailMessage.Bcc.Add(c.ToString());
                                //    }
                                //}

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


                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _issueManagerContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Daily_Report_Generation" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _issueManagerContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _issueManagerContext.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
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
                //return ex.Message;
            }
            return "success";

        }

        // MOBILE NOTIFICATION
        public string callnotification(string devicenew, long empid, DateTime? ISMDRPT_Date, string empname, long mi_id, string sound)
        {
            try
            {
                ISM_DailyReportGenerationDTO data = new ISM_DailyReportGenerationDTO();
                var dailyreport = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                && a.ISMDRPT_Date == ISMDRPT_Date).Distinct().ToList();
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Daily Report " + ISMDRPT_Date + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //  "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"'
                //  + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Daily Report " + ISMDRPT_Date + '"' + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' +
                //" , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " ,  " + '"' + "summaryText" + '"' + ":" + '"' + "here are % n % notifications" + '"' + " ,  " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Daily Report " + ISMDRPT_Date + '"' + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "style" + '"' + ":" + '"' + "inbox" + '"' + " , " + '"' + "summaryText" + '"' + ":" + '"' + "You have %n% notifications" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Daily Report " + ISMDRPT_Date + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";


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
                var notificationdetails = (from UEM in _issueManagerContext.ISM_User_Employees_MappingDMO
                                           from HRE in _issueManagerContext.MasterEmployee
                                           from HMD in _issueManagerContext.HR_Master_Department
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
                        push.HRME_Id = n.HRME_Id;
                        push.ISMNO_Notification = notification;
                        push.ISMNO_NoticationDate = indianTime;
                        push.ISMNO_NotificationType = "DR";
                        push.ISMNO_ReadFlg = false;
                        push.ISMNO_MakeUnReadFlg = false;
                        push.ISMNO_ActiveFlag = true;
                        push.ISMNO_CreatedBy = userid;
                        push.ISMNO_UpdatedBy = userid;
                        push.CreatedDate = indianTime;
                        push.UpdatedDate = indianTime;
                        _issueManagerContext.Add(push);

                        var contactExists = _issueManagerContext.SaveChanges();
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

        /* NO USE FN */
        public ISM_DailyReportGenerationDTO SaveRemarks(ISM_DailyReportGenerationDTO data)
        {
            try
            {

                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                ISM_DailyReportNotSent_DateWiseRemarksDMO iSM_DailyReportNotSent_DateWiseRemarksDMO = new ISM_DailyReportNotSent_DateWiseRemarksDMO();
                iSM_DailyReportNotSent_DateWiseRemarksDMO.MI_Id = data.MI_Id;
                iSM_DailyReportNotSent_DateWiseRemarksDMO.HRME_Id = hrmEempid.FirstOrDefault().Emp_Code;
                iSM_DailyReportNotSent_DateWiseRemarksDMO.ISMDRNSDR_DRNotSentDate = data.ISMDRNSDR_DRNotSentDate;
                iSM_DailyReportNotSent_DateWiseRemarksDMO.ISMDRNSDR_RemarksDate = data.ISMDRNSDR_RemarksDate;
                iSM_DailyReportNotSent_DateWiseRemarksDMO.ISMDRNSDR_Remarks = data.DRNotSentRemarks;
                iSM_DailyReportNotSent_DateWiseRemarksDMO.CreatedDate = indiantime0;
                iSM_DailyReportNotSent_DateWiseRemarksDMO.UpdatedDate = indiantime0;
                _issueManagerContext.Add(iSM_DailyReportNotSent_DateWiseRemarksDMO);
                var i = _issueManagerContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_DailyReportGenerationDTO CheckDeviationRemarks(ISM_DailyReportGenerationDTO data)
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

        //Save Dr Details Backup
        public ISM_DailyReportGenerationDTO generatedailyreport_backup(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;
            long counttrue = 0;
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                var checkdailyreportgenerated = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                && a.ISMDRPT_Date == data.ISMDRPT_Date).ToList();
                if (checkdailyreportgenerated.Count() > 0)
                {
                    //data.message = "Generated";
                    //return data;
                }

                if (data.todayorothers == "today")
                {
                    data.ISMDRPT_OrdersDateFlg = false;
                    data.ISMDRPT_OthersDateReason = "";
                }
                else
                {
                    data.ISMDRPT_OrdersDateFlg = true;
                    data.ISMDRPT_OthersDateReason = data.reason;
                }

                DateTime fromdatecon1 = DateTime.Now;
                string confromdate1 = "";
                try
                {
                    fromdatecon1 = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate1 = fromdatecon1.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (data.ISMTPL_Id > 0)
                {
                    var plannerid = data.ISMTPL_Id;
                }
                else
                {
                    var getplannername = _issueManagerContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid
                    && t.ISMTPL_ActiveFlg == true && t.ISMTPL_ApprovalFlg == true
                    && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(fromdatecon1)
                    && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(fromdatecon1)).ToList();

                    data.ISMTPL_Id = getplannername.FirstOrDefault().ISMTPL_Id;

                }

                /* GET THE APPROVED PLANNER ID FOR SELECTD PLANNER  */
                var getapprovedplanner = _issueManagerContext.ISM_Task_Planner_ApprovedDMO.Where(a => a.ISMTPL_Id == data.ISMTPL_Id).ToList();


                decimal? totalhrs = 0;

                // ******** CHECKING DAILY REPORT IS BLOCKED ******* //

                if (data.todayorothers == "today")
                {
                    var getdailyreportblocked = _issueManagerContext.ISM_Block_EmployeeDMO.Where(t => t.HRME_Id == empid
                    && t.ISMBE_BlockFlg == true && t.ISMBE_ActiveFlg == true).ToList();

                    data.blockedcount = getdailyreportblocked.Count();
                    if (data.blockedcount > 0)
                    {
                        data.ISMDRPT_BlockedFlag = true;
                    }
                    else
                    {
                        data.ISMDRPT_BlockedFlag = false;
                    }
                }
                else
                {
                    var getdailyreportblocked = _issueManagerContext.ISM_Block_EmployeeDMO.Where(t => t.HRME_Id == empid
                    && t.ISMBE_BlockDate.Value.Date >= data.ISMDRPT_Date.Value.Date
                    && t.ISEBE_UnblockDate.Value.Date <= data.ISMDRPT_Date.Value.Date && t.ISMBE_ActiveFlg == true).ToList();

                    data.blockedcount = getdailyreportblocked.Count();
                    if (data.blockedcount > 0)
                    {
                        data.ISMDRPT_BlockedFlag = true;
                    }
                    else
                    {
                        data.ISMDRPT_BlockedFlag = false;
                    }
                }

                var checkdaywisedr_details = _issueManagerContext.ISM_DailyReport_DaywiseDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                     && a.ISMDRPTDW_Date == data.ISMDRPT_Date).ToList();

                if (checkdaywisedr_details.Count > 0)
                {
                    var result_checkdaywisedr_details = _issueManagerContext.ISM_DailyReport_DaywiseDMO.Single(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                     && a.ISMDRPTDW_Date == data.ISMDRPT_Date);
                    result_checkdaywisedr_details.ISMDRPTDW_BlockedDayDRFlg = data.ISMDRPT_BlockedFlag;
                    result_checkdaywisedr_details.ISMDRPTDW_HalfDayDRFlg = data.ISMDRPT_HalfDayFlag;
                    result_checkdaywisedr_details.ISMDRPTDW_OrthersDateFlg = data.ISMDRPT_OrdersDateFlg;
                    result_checkdaywisedr_details.ISMDRPTDW_OthersDateReason = data.ISMDRPT_OthersDateReason;
                    result_checkdaywisedr_details.ISMDRPTDW_UpdatedBy = data.UserId;
                    result_checkdaywisedr_details.ISMDRPTDW_UpdatedDate = indiantime0;
                    _issueManagerContext.Add(result_checkdaywisedr_details);
                }
                else
                {
                    ISM_DailyReport_DaywiseDMO iSM_DailyReport_DaywiseDMO = new ISM_DailyReport_DaywiseDMO();
                    iSM_DailyReport_DaywiseDMO.MI_Id = data.MI_Id;
                    iSM_DailyReport_DaywiseDMO.HRME_Id = empid;
                    iSM_DailyReport_DaywiseDMO.ISMTPL_Id = data.ISMTPL_Id;
                    iSM_DailyReport_DaywiseDMO.ISMTPLAP_Id = getapprovedplanner.FirstOrDefault().ISMTPLAP_Id;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_OverallComments = "";
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_BlockedDayDRFlg = data.ISMDRPT_BlockedFlag;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_HalfDayDRFlg = data.ISMDRPT_HalfDayFlag;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_OrthersDateFlg = data.ISMDRPT_OrdersDateFlg;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_OthersDateReason = data.ISMDRPT_OthersDateReason;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_ActiveFlg = true;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_CreatedDate = indiantime0;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_UpdatedDate = indiantime0;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_CreatedBy = data.UserId;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_UpdatedBy = data.UserId;
                    iSM_DailyReport_DaywiseDMO.ISMDRPTDW_Date = data.ISMDRPT_Date;
                    _issueManagerContext.Add(iSM_DailyReport_DaywiseDMO);
                }

                for (int k = 0; k < data.Temp_ISM_DailyReportGenerationDTO.Count(); k++)
                {
                    data.ISMDRPT_DeviationFlg = false;
                    ISM_DailyReportGenerationDMO dmo = new ISM_DailyReportGenerationDMO();
                    if (data.Temp_ISM_DailyReportGenerationDTO[k].extraflag == 1)
                    {
                        data.ISMDRPT_ExtraFlg = true;
                        totalhrs += data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs;
                    }
                    else
                    {
                        data.ISMDRPT_ExtraFlg = false;
                        totalhrs += 0;
                    }
                    if (data.Temp_ISM_DailyReportGenerationDTO[k].deviationflag == true)
                    {
                        data.ISMDRPT_DeviationFlg = true;
                    }
                    else
                    {
                        data.ISMDRPT_DeviationFlg = false;
                    }

                    dmo.MI_Id = data.MI_Id;
                    dmo.ISMTPLAP_Id = getapprovedplanner.FirstOrDefault().ISMTPLAP_Id;
                    dmo.HRME_Id = empid;
                    dmo.ISMTPL_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;
                    if (data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id > 0)
                    {
                        dmo.ISMTPLTA_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id;
                    }

                    dmo.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                    dmo.ISMDRPT_Date = data.ISMDRPT_Date;
                    dmo.ISMDRPT_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                    dmo.ISMDRPT_TimeTakenInHrs = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;
                    dmo.ISMDRPT_TimeTakenFlg = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrsmin;
                    dmo.ISMDRPT_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                    dmo.ISMDRPT_DeviationFlg = data.ISMDRPT_DeviationFlg;
                    dmo.ISMDRPT_ExtraFlg = data.ISMDRPT_ExtraFlg;
                    dmo.ISMDRPT_OrdersDateFlg = data.ISMDRPT_OrdersDateFlg;
                    dmo.ISMDRPT_OthersDateReason = data.ISMDRPT_OthersDateReason;
                    dmo.ISMDRPT_ActiveFlg = true;
                    dmo.ISMDRPT_BlockedFlag = data.ISMDRPT_BlockedFlag;
                    dmo.ISMDRPT_JeotaggedPhoto = data.ISMDRPT_JeotaggedPhoto;
                    dmo.ISMDRPT_JeotaggedPhotoFath = data.ISMDRPT_JeotaggedPhotoFath;
                    dmo.ISMDRPT_LocationLatLogValue = data.ISMDRPT_LocationLatLogValue;
                    dmo.ISMDRPT_LocationName = data.ISMDRPT_LocationName;
                    dmo.CreatedDate = indiantime0;
                    dmo.UpdatedDate = indiantime0;
                    dmo.ISMDRPT_CreatedBy = data.UserId;
                    dmo.ISMDRPT_UpdatedBy = data.UserId;
                    dmo.ISMDRPT_HalfDayFlag = data.ISMDRPT_HalfDayFlag;

                    _issueManagerContext.Add(dmo);

                    // Adding Check List Files Or Comments

                    if (data.Temp_ISM_DailyReportGenerationDTO[k].categorychecklist != null && data.Temp_ISM_DailyReportGenerationDTO[k].categorychecklist.Length > 0)
                    {
                        foreach (var c in data.Temp_ISM_DailyReportGenerationDTO[k].categorychecklist)
                        {
                            ISM_DailyReport_FilesDMO iSM_DailyReport_FilesDMO = new ISM_DailyReport_FilesDMO();

                            iSM_DailyReport_FilesDMO.ISMDRPT_Id = dmo.ISMDRPT_Id;
                            iSM_DailyReport_FilesDMO.ISMMTCATCL_Id = c.ISMMTCATCL_Id;
                            iSM_DailyReport_FilesDMO.ISMDRPTFL_FileName = c.filename;
                            iSM_DailyReport_FilesDMO.ISMDRPTFL_RefNo = c.refno;
                            iSM_DailyReport_FilesDMO.ISMDRPTFL_Remarks = c.comments;
                            iSM_DailyReport_FilesDMO.ISMDRPT_ActiveFlg = true;
                            iSM_DailyReport_FilesDMO.ISMDRPT_CreatedDate = indiantime0;
                            iSM_DailyReport_FilesDMO.ISMDRPT_UpdatedDate = indiantime0;
                            iSM_DailyReport_FilesDMO.ISMDRPT_CreatedBy = data.UserId;
                            iSM_DailyReport_FilesDMO.ISMDRPT_UpdatedBy = data.UserId;
                            iSM_DailyReport_FilesDMO.ISMDRPTFL_FilePath = c.filepath;
                            _issueManagerContext.Add(iSM_DailyReport_FilesDMO);
                        }
                    }


                    // ************ Checking Planner Task Is Created Or Not ************ //
                    var updateplannertask = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id
                    && a.ISMTPL_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true
                    && a.ISMTPLTA_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id).ToList();

                    if (updateplannertask.Count > 0)
                    {
                        decimal? count = 0;

                        count = updateplannertask.FirstOrDefault().ISMTPLTA_TimeTakenInHrs;

                        count += data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;

                        var result = _issueManagerContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id
                        && a.ISMTPL_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true
                        && a.ISMTPLTA_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id);

                        result.ISMTPLTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        result.ISMTPLTA_DeviationFlg = data.ISMDRPT_DeviationFlg;
                        if (data.ISMDRPT_DeviationFlg == true)
                        {
                            result.ISMTPLTA_FinishedDate = indiantime0;
                        }
                        result.ISMTPLTA_TimeTakenInHrs = Convert.ToDecimal(count);
                        result.UpdatedDate = indiantime0;
                        result.ISMTPLTA_UpdatedBy = data.UserId;

                        _issueManagerContext.Update(result);
                    }
                    else
                    {
                        ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                        dmo_task.ISMTPL_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;
                        dmo_task.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        dmo_task.ISMTPLTA_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Remarks;
                        dmo_task.ISMTPLTA_StartDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_StartDate;
                        dmo_task.ISMTPLTA_EndDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EndDate;
                        dmo_task.ISMTPLTA_EffortInHrs = Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs);
                        dmo_task.ISMTPLTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        dmo_task.ISMTPLTA_DeviationFlg = data.ISMDRPT_DeviationFlg;
                        if (data.ISMDRPT_DeviationFlg == true)
                        {
                            dmo_task.ISMTPLTA_FinishedDate = indiantime0;
                        }
                        dmo_task.ISMTPLTA_ExtraFlg = data.ISMDRPT_ExtraFlg;
                        dmo_task.ISMTPLTA_PreviousTask = false;
                        dmo_task.ISMTPLTA_TimeTakenInHrs = Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs);
                        dmo_task.ISMTPLTA_ActiveFlg = true;
                        dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                        dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                        dmo_task.CreatedDate = indiantime0;
                        dmo_task.UpdatedDate = indiantime0;
                        dmo_task.ISMTPLTA_ApprovalFlg = true;

                        _issueManagerContext.Add(dmo_task);
                    }


                    //*********** Checking Planner Approved Task List ********//
                    if (getapprovedplanner.Count() > 0)
                    {
                        var updateplannerapprovedtask = _issueManagerContext.ISM_Task_Planner_Approved_TasksDMO.Where(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id && a.ISMTPLAP_Id == getapprovedplanner.FirstOrDefault().ISMTPLAP_Id).ToList();
                    }

                    // ***************** Adding Task To Assigned Task ***************** //
                    var checktaskisassigned = _issueManagerContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == empid
                    && a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                    if (checktaskisassigned.Count() == 0)
                    {
                        ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                        dmo_task_assigned.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        dmo_task_assigned.HRME_Id = empid;
                        dmo_task_assigned.ISMTCRASTO_AssignedDate = indiantime0;
                        dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                        dmo_task_assigned.ISMTCRASTO_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Remarks;
                        dmo_task_assigned.ISMTCRASTO_StartDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_StartDate;
                        dmo_task_assigned.ISMTCRASTO_EndDate = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EndDate;
                        dmo_task_assigned.ISMTCRASTO_EffortInHrs = Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EffortInHrs);
                        dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                        dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                        dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                        dmo_task_assigned.CreatedDate = indiantime0;
                        dmo_task_assigned.UpdatedDate = indiantime0;

                        _issueManagerContext.Add(dmo_task_assigned);
                    }

                    // ******************** Status Updation In Task Creation ******************** //

                    var checktaskcreation = _issueManagerContext.ISM_TaskCreationDMO.Where(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id).ToList();

                    if (checktaskcreation.Count() > 0)
                    {
                        var checktaskcreationresult = _issueManagerContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id);

                        checktaskcreationresult.ISMTCR_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        checktaskcreationresult.ISMTCR_UpdatedBy = data.UserId;
                        checktaskcreationresult.UpdatedDate = indiantime0;
                        _issueManagerContext.Update(checktaskcreationresult);
                    }


                    // ******************** Status Updation In Task Creation Response ******************** //

                    if (data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList != null)
                    {
                        foreach (var taskatt in data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList)
                        {
                            ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();
                            taskresponse.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                            taskresponse.HRME_Id = empid;
                            taskresponse.ISMTCRRES_ResponseDate = indiantime0;
                            taskresponse.ISMTCRRES_Response = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                            taskresponse.ISMTCRRES_ResponseAttachment = taskatt.filepath;
                            taskresponse.ISMTCRRES_ResponsePageName = "Daily Report";
                            taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                            taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                            taskresponse.ISMTCRRES_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                            taskresponse.ISMTCRRES_ActiveFlg = true;
                            taskresponse.CreatedDate = indiantime0;
                            taskresponse.UpdatedDate = indiantime0;
                            _issueManagerContext.Add(taskresponse);
                        }
                    }
                    else
                    {
                        ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();

                        taskresponse.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        taskresponse.HRME_Id = empid;
                        taskresponse.ISMTCRRES_ResponseDate = indiantime0;
                        taskresponse.ISMTCRRES_Response = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                        taskresponse.ISMTCRRES_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        taskresponse.ISMTCRRES_ResponseAttachment = "";
                        taskresponse.ISMTCRRES_ResponsePageName = "Daily Report";
                        taskresponse.ISMTCRRES_ActiveFlg = true;
                        taskresponse.CreatedDate = indiantime0;
                        taskresponse.UpdatedDate = indiantime0;
                        taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                        taskresponse.ISMTCRRES_UpdatedBy = data.UserId;

                        _issueManagerContext.Add(taskresponse);

                    }


                    // ******************** INSERTING INTO DEVAITION TABLE ******************** //
                    if (data.ISMDRPT_DeviationFlg == true)
                    {
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime toatecon = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                        ISM_Task_Planner_DeviationDMO deviationdmo = new ISM_Task_Planner_DeviationDMO();

                        deviationdmo.ISMTPL_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;
                        deviationdmo.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                        deviationdmo.HRME_Id = empid;
                        deviationdmo.ISMTPLDE_StartDate = Convert.ToDateTime(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_StartDate);
                        deviationdmo.ISMTPLDE_EndDate = Convert.ToDateTime(data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_EndDate);
                        deviationdmo.ISMTPLDE_FinishedDate = toatecon;
                        deviationdmo.ISMTPLDE_DeviationReason = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                        deviationdmo.ISMTPLDE_ActiveFlg = true;
                        deviationdmo.CreatedDate = toatecon;
                        deviationdmo.UpdatedDate = toatecon;
                        deviationdmo.ISMTPLDE_CreatedBy = data.UserId;
                        deviationdmo.ISMTPLDE_UpdatedBy = data.UserId;
                        deviationdmo.ISMDR_Id = Convert.ToInt64(data.Temp_ISM_DailyReportGenerationDTO[k].ISMDR_Id);
                        deviationdmo.ISMTPLDE_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        _issueManagerContext.Add(deviationdmo);
                    }
                }

                //***************** Planner Total Hours Updation If They Add Extra Work ***************//
                var plannerresultlist = _issueManagerContext.ISM_Task_PlannerDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMTPL_Id == data.ISMTPL_Id).ToList();
                decimal totalhrslist = plannerresultlist.FirstOrDefault().ISMTPL_TotalHrs;
                totalhrslist = totalhrslist + Convert.ToDecimal(totalhrs);

                var plannerresult = _issueManagerContext.ISM_Task_PlannerDMO.Single(a => a.MI_Id == data.MI_Id && a.ISMTPL_Id == data.ISMTPL_Id);
                plannerresult.ISMTPL_TotalHrs = totalhrslist;
                _issueManagerContext.Update(plannerresult);

                //***************** Planner Approved Total Hours Updation If They Add Extra Work ***************//
                var plannerlistapproved = _issueManagerContext.ISM_Task_Planner_ApprovedDMO.Where(a => a.ISMTPL_Id == data.ISMTPL_Id).ToList();
                decimal? totalhrslistapproved = plannerlistapproved.FirstOrDefault().ISMTPLAP_TotalHrs;
                totalhrslistapproved = totalhrslistapproved + Convert.ToDecimal(totalhrs);

                var plannerlistapprovedresult = _issueManagerContext.ISM_Task_Planner_ApprovedDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                plannerlistapprovedresult.ISMTPLAP_TotalHrs = totalhrslistapproved;
                _issueManagerContext.Update(plannerlistapprovedresult);

                var i = _issueManagerContext.SaveChanges();

                if (i > 0)
                {
                    data.returnval = true;

                    for (int k1 = 0; k1 < data.Temp_ISM_DailyReportGenerationDTO.Length; k1++)
                    {
                        var getextraflag = data.Temp_ISM_DailyReportGenerationDTO[k1].extraflag;
                        var gettaskid = data.Temp_ISM_DailyReportGenerationDTO[k1].ISMTCR_Id;
                        var remarks = data.Temp_ISM_DailyReportGenerationDTO[k1].ISMDRPT_Remarks;
                        var startdate = data.Temp_ISM_DailyReportGenerationDTO[k1].ISMTPLTA_StartDate;
                        var enddate = data.Temp_ISM_DailyReportGenerationDTO[k1].ISMTPLTA_EndDate;

                        if (getextraflag == 1)
                        {
                            var getplannertaskid = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == gettaskid
                            && a.ISMTPL_Id == data.ISMTPL_Id).ToList();

                            if (getplannertaskid.Count() > 0)
                            {
                                var getplannertaskidresult = _issueManagerContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == gettaskid
                                && a.ISMTPL_Id == data.ISMTPL_Id);

                                ISM_Task_Planner_Approved_TasksDMO approvedtasklist = new ISM_Task_Planner_Approved_TasksDMO();

                                approvedtasklist.ISMTPLAP_Id = plannerlistapprovedresult.ISMTPLAP_Id;
                                approvedtasklist.ISMTCR_Id = gettaskid;
                                approvedtasklist.ISMTPLAPTA_Remarks = remarks;
                                approvedtasklist.ISMTPLAPTA_StartDate = startdate;
                                approvedtasklist.ISMTPLAPTA_EndDate = enddate;
                                approvedtasklist.ISMTPLAPTA_EffortInHrs = data.Temp_ISM_DailyReportGenerationDTO[k1].ISMTPLTA_EffortInHrs;
                                approvedtasklist.ISMTPLAPTA_FinishedDate = data.ISMDRPT_Date;
                                approvedtasklist.ISMTPLAPTA_Status = data.Temp_ISM_DailyReportGenerationDTO[k1].ISMDRPT_Status;
                                approvedtasklist.ISMTPLAPTA_DeviationFlg = data.Temp_ISM_DailyReportGenerationDTO[k1].deviationflag;
                                approvedtasklist.ISMTPLAPTA_ExtraTaskFlg = true;
                                approvedtasklist.ISMTPLAPTA_TimeTakenInHrs = data.Temp_ISM_DailyReportGenerationDTO[k1].ISMTPLTA_EffortInHrs;
                                approvedtasklist.ISMTPLAPTA_RejectedFlg = false;
                                approvedtasklist.ISMTPLAPTA_ActiveFlg = true;
                                approvedtasklist.CreatedDate = indiantime0;
                                approvedtasklist.UpdatedDate = indiantime0;
                                approvedtasklist.ISMTPLAPTA_CreatedBy = data.UserId;
                                approvedtasklist.ISMTPLAPTA_UpdatedBy = data.UserId;
                                approvedtasklist.ISMTPLAPTA_ApprovalFlg = true;
                                approvedtasklist.ISMTPLTA_Id = getplannertaskid.FirstOrDefault().ISMTPLTA_Id;

                                _issueManagerContext.Add(approvedtasklist);


                                var getdrtaskdetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.HRME_Id == empid && a.ISMTCR_Id == gettaskid
                                && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMDRPT_Date == data.ISMDRPT_Date).ToList();

                                if (getdrtaskdetails.Count > 0)
                                {
                                    var resultdrtaskdetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Single(a => a.HRME_Id == empid
                                    && a.ISMTCR_Id == gettaskid && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMDRPT_Date == data.ISMDRPT_Date
                                    && a.ISMDRPT_Id == getdrtaskdetails.FirstOrDefault().ISMDRPT_Id);
                                    resultdrtaskdetails.ISMTPLTA_Id = getplannertaskid.FirstOrDefault().ISMTPLTA_Id;
                                    _issueManagerContext.Update(resultdrtaskdetails);
                                }
                            }
                        }
                    }

                    var j1 = _issueManagerContext.SaveChanges();

                    var count = 0;
                    for (int k2 = 0; k2 < data.Temp_ISM_DailyReportGenerationDTO.Length; k2++)
                    {
                        var getextraflag = data.Temp_ISM_DailyReportGenerationDTO[k2].extraflag;
                        var gettaskid = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMTCR_Id;
                        var gettaskstatus = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMDRPT_Status;
                        var remarks = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMDRPT_Remarks;
                        var startdate = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMTPLTA_StartDate;
                        var enddate = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMTPLTA_EndDate;
                        var periodicityflag = data.Temp_ISM_DailyReportGenerationDTO[k2].ISMTAPL_Periodicity;

                        if ((periodicityflag == "Daily" || periodicityflag == "Weekly"))
                        {
                            count += 1;

                            var Get_Task_CountIds = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == gettaskid
                            && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ApprovalFlg == true).Count();

                            var Get_Task_Completed_CountIds = _issueManagerContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == gettaskid
                             && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ApprovalFlg == true && a.ISMTPLTA_Status == "Completed").Count();

                            var UpdatePeriodicityStatus = _issueManagerContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == gettaskid);

                            //if (gettaskstatus == "Completed")
                            //{
                            if (Get_Task_CountIds == Get_Task_Completed_CountIds)
                            {
                                UpdatePeriodicityStatus.ISMTCR_Status = "Completed";
                            }
                            else
                            {
                                UpdatePeriodicityStatus.ISMTCR_Status = "Open";
                            }

                            UpdatePeriodicityStatus.ISMTCR_UpdatedBy = data.UserId;
                            UpdatePeriodicityStatus.UpdatedDate = indiantime0;
                            _issueManagerContext.Update(UpdatePeriodicityStatus);
                            //}
                        }
                    }

                    if (count > 0)
                    {
                        var j2 = _issueManagerContext.SaveChanges();
                    }

                    if (plannerresultlist.FirstOrDefault().ISMTPL_EndDate.Value.Date == indiantime0.Date)
                    {
                        DateTime fromdatecon12 = DateTime.Now;
                        string confromdate12 = "";

                        fromdatecon12 = Convert.ToDateTime(indiantime0.Date.ToString("yyyy-MM-dd"));
                        confromdate12 = fromdatecon12.ToString("yyyy-MM-dd");

                        var outputval = _issueManagerContext.Database.ExecuteSqlCommand("PlannerAddedTasksDetetion  @p0,@p1", empid, confromdate12);
                        if (outputval > 0)
                        {
                            Console.WriteLine(" Planner Updated outputval " + outputval);
                        }
                        else
                        {
                            Console.WriteLine(" Planner Not Updated outputval " + outputval);
                        }
                    }

                    data.returnval = true;
                    // ============================================== Push Notification
                    var getemployeedetails = (from a in _issueManagerContext.MasterEmployee
                                              from b in _issueManagerContext.HR_Master_Department
                                              from c in _issueManagerContext.HR_Master_Designation

                                              from e in _issueManagerContext.HR_Master_Grade
                                              where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                                              && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
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

                    //var notificationdetails = _issueManagerContext.ISM_Notification_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id
                    //&& a.HRMD_Id == getemployeedetails.FirstOrDefault().HRMD_Id).ToList();

                    //*********************************** User Employee Mapped List ********************************// 
                    var notificationuserids = (from UEM in _issueManagerContext.ISM_User_Employees_MappingDMO
                                               from HRE in _issueManagerContext.MasterEmployee
                                               where (UEM.HRME_Id == HRE.HRME_Id && HRE.HRME_ActiveFlag == true && HRE.HRME_LeftFlag == false && UEM.HRME_Id == empid)
                                               select new ISM_DailyReportGenerationDTO
                                               {
                                                   UserId = UEM.User_Id,
                                               }).Distinct().ToList();
                    List<long> userids = new List<long>();
                    foreach (var n in notificationuserids)
                    {
                        userids.Add(n.UserId);
                    }

                    var notificationdetails = (from SUL in _issueManagerContext.Staff_User_Login
                                               where (userids.Contains(SUL.Id))
                                               select new ISM_DailyReportGenerationDTO
                                               {
                                                   HRME_Id = SUL.Emp_Code,
                                               }).Distinct().ToList();

                    var deviceidsnew = "";
                    var devicenew = "";
                    if (notificationdetails.Count > 0)
                    {
                        data.notificationUser = notificationdetails.ToArray();
                        List<long> employeeids = new List<long>();
                        foreach (var n in notificationdetails)
                        {
                            employeeids.Add(n.HRME_Id);
                        }
                        var devicelist = (from a in _issueManagerContext.MasterEmployee
                                          where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id))
                                          select new ISM_DailyReportGenerationDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                          }).Distinct().ToList();
                        data.deviceArray = devicelist.ToArray();
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
                    }
                    try
                    {
                        if (data.sound == "")
                        {
                            data.sound = "default";
                        }
                        callnotification(devicenew, empid, indiantime0, getemployeedetails.FirstOrDefault().employeename, data.MI_Id, data.sound);
                        Insertnotification(data.MI_Id, empid, indiantime0, getemployeedetails.FirstOrDefault().employeename, data.UserId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    #region Email                
                    string str = "";
                    counttrue = 1;
                    DateTime fromdatecon = DateTime.Now;

                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime toatecon = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    string confromdate = "";
                    string contodate = "";
                    string datetime = "";
                    try
                    {
                        fromdatecon = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                        confromdate = fromdatecon.ToString("yyyy-MM-dd");

                        contodate = toatecon.ToString("dd/MM/yyyy");

                        datetime = toatecon.ToString("dd/MM/yyyy hh:mm tt");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    List<ISM_DailyReportGenerationDTO> dtonewtemp = new List<ISM_DailyReportGenerationDTO>();

                    // Getting The saved Data 

                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_DailyReport_Generation_Search";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
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
                                    dtonewtemp.Add(new ISM_DailyReportGenerationDTO
                                    {
                                        ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                        HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                        ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                        ISMDRPT_Statusnew = Convert.ToString(dataReader["ISMDRPT_Status"]),
                                        ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                        ISMDRPT_TimeTakenInHrsnew = Convert.ToString(dataReader["ISMDRPT_TimeTakenInHrs"]),
                                        ISMMCLT_ClientName = Convert.ToString(dataReader["ISMMCLT_ClientName"]),
                                        assignedby = Convert.ToString(dataReader["assignedby"]),
                                        ISMDRPT_Datenew = Convert.ToDateTime(dataReader["ISMDRPT_Date"]),
                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    // Calculate Percentage     

                    List<ISM_DailyReportGenerationDTO> result = new List<ISM_DailyReportGenerationDTO>();


                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_DailyReport_Generation_Deviation_Calculation";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                        {
                            Value = confromdate
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
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
                                    result.Add(new ISM_DailyReportGenerationDTO
                                    {
                                        completedper = Convert.ToDecimal(dataReader["per"]),
                                        deviationper = Convert.ToDecimal(dataReader["deviation"]),
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    string emailccdetails = "";
                    string emailbccdetails = "";

                    var emaildetails = _issueManagerContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == getemployeedetails.FirstOrDefault().HRMD_Id).ToList();
                    if (emaildetails.Count > 0)
                    {
                        emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                        emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                    }
                    else
                    {
                        emailccdetails = "";
                        emailbccdetails = "";
                    }

                    var checktemplate = _issueManagerContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Daily_Report_Generation"
                    && a.ISES_MailActiveFlag == true).ToList();

                    if (checktemplate.Count == 0)
                    {
                        data.message = "Email Template not Mapped to the selected Institution";
                    }
                    else
                    {
                        var emplname = getemployeedetails.FirstOrDefault().employeename;
                        var deptname = getemployeedetails.FirstOrDefault().department;
                        var desginame = getemployeedetails.FirstOrDefault().desgination;
                        var gradname = getemployeedetails.FirstOrDefault().gradename;
                        var email = getemployeedetails.FirstOrDefault().emailid;
                        var date = contodate;
                        var newdate = datetime;

                        decimal? completedperc = 0;
                        decimal? deviationperc = 0;

                        if (result.Count() > 0)
                        {
                            completedperc = result.FirstOrDefault().completedper;
                            deviationperc = result.FirstOrDefault().deviationper;
                        }
                        else
                        {
                            completedperc = 0;
                            deviationperc = 100;
                        }

                        string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                        templatedetails = templatedetails.Replace("[DeveloperName]", emplname);
                        templatedetails = templatedetails.Replace("[DATE]", date);
                        templatedetails = templatedetails.Replace("[Department]", deptname);
                        templatedetails = templatedetails.Replace("[Designation]", gradname);
                        templatedetails = templatedetails.Replace("[CompletedPer]", completedperc.ToString());
                        templatedetails = templatedetails.Replace("[Pendingwork]", deviationperc.ToString());
                        templatedetails = templatedetails.Replace("[DATENAME]", newdate);

                        int rownumber = 0;

                        if (dtonewtemp.Count > 0)
                        {
                            for (int j = 0; j < dtonewtemp.Count; j++)
                            {
                                rownumber += 1;
                                str += "<tr>" +
                                    "<td align=" + "center" + " style=" + ">" + rownumber + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_BugOREnhancementFlg + "</td>" +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMMCLT_ClientName + "</td>" +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_TaskNo + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].HRMP_Name + "</td> " +
                                    "<td align=" + "left" + "style=" + "word-break: break-word;" + ">" + dtonewtemp[j].ISMTCR_Desc + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMDRPT_Statusnew + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].assignedby + "</td> " +
                                    "<td align=" + "left" + ">" + contodate + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMDRPT_TimeTakenInHrsnew + "</td> " +
                                    "</tr>"
                                    ;
                            }
                            // str += "</table>";
                        }

                        templatedetails = templatedetails.Replace("[MSG]", str);

                        try
                        {
                            sendmailexamtt(data.MI_Id, email, templatedetails, emailccdetails, emailbccdetails);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            _logger.LogInformation("Daily Report Generation generatedailyreport EMAIL For Employee Id : " + empid + "   : " + ex.Message);
                        }
                    }
                    data.returnval = true;
                    #endregion
                }
                else
                {
                    data.returnval = false;
                }
                if (data.deviation_id == 1)
                {

                    var hrme = _issueManagerContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);

                    string hrmeids = "0";

                    hrmeids = hrmeids + "," + hrme.HRME_Id;

                    List<ISM_Weekly_Deviation_DTO> wd = new List<ISM_Weekly_Deviation_DTO>();
                    ISM_Weekly_Deviation_DTO wd1 = new ISM_Weekly_Deviation_DTO();
                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Weekly_DeviationReport_Calculation";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.VarChar)
                        {
                            Value = hrmeids
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate",
                       SqlDbType.DateTime)
                        {
                            Value = data.startweek
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                        SqlDbType.DateTime)
                        {
                            Value = data.endweek
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
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
                            wd1.deviation_ary = retObject.ToArray();

                            var deviation = _issueManagerContext.ISM_Weekly_Deviation_DMO_con.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                            if (deviation.ISMWD_Deviation >= 20 && deviation.ISMWD_Deviation <= 40)
                            {
                                var checktemplate = _issueManagerContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Memo_Generation"
                       && a.ISES_MailActiveFlag == true).ToList();

                                if (checktemplate.Count == 0)
                                {
                                    data.message = "Email Template not Mapped to the selected Institution";
                                }
                                else
                                {
                                    var getemployeedetails1 = (from a in _issueManagerContext.MasterEmployee
                                                               from b in _issueManagerContext.HR_Master_Department
                                                               from c in _issueManagerContext.HR_Master_Designation

                                                               from e in _issueManagerContext.HR_Master_Grade
                                                               where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                                                               && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
                                                               select new ISM_DailyReportGenerationDTO
                                                               {
                                                                   employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                                   + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                                   + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                                   department = b.HRMD_DepartmentName,
                                                                   desgination = c.HRMDES_DesignationName,
                                                                   gradename = e.HRMG_GradeName,
                                                                   HRMD_Id = a.HRMD_Id,
                                                                   emailid = a.HRME_EmailId
                                                               }).Distinct().ToList();
                                    var emplname = getemployeedetails1.FirstOrDefault().employeename;
                                    var deptname = getemployeedetails1.FirstOrDefault().department;
                                    var desginame = getemployeedetails1.FirstOrDefault().desgination;
                                    var gradname = getemployeedetails1.FirstOrDefault().gradename;
                                    var email = getemployeedetails1.FirstOrDefault().emailid;

                                    string templatedetails1 = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                                    string date = "";
                                    DateTime fromdatecon = DateTime.Now;
                                    fromdatecon = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd"));
                                    date = fromdatecon.ToString("yyyy-MM-dd");

                                    //===================bcc mail===========
                                    string emailccdetails = "";
                                    string emailbccdetails = "";
                                    var emaildetails = _issueManagerContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == getemployeedetails1.FirstOrDefault().HRMD_Id).ToList();
                                    if (emaildetails.Count > 0)
                                    {
                                        emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                                        emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                                    }
                                    else
                                    {
                                        emailccdetails = "";
                                        emailbccdetails = "";
                                    }
                                    //==============================bccmailend=======
                                    //=================getdate to mail==========

                                    templatedetails1 = templatedetails1.Replace("[DeveloperName]", emplname);
                                    templatedetails1 = templatedetails1.Replace("[Department]", deptname);
                                    templatedetails1 = templatedetails1.Replace("[Designation]", gradname);
                                    templatedetails1 = templatedetails1.Replace("[Deviation]", Convert.ToDecimal(deviation.ISMWD_Deviation).ToString());
                                    templatedetails1 = templatedetails1.Replace("[DATENAME]", date);


                                    try
                                    {
                                        send_memo_deviation(data.MI_Id, email, templatedetails1, emailccdetails);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        _logger.LogInformation("Daily Report Generation generatedailyreport EMAIL For Employee Id : " + empid + "   : " + ex.Message);
                                    }
                                    data.returnval = true;
                                }
                            }
                            else if (deviation.ISMWD_Deviation >= 40)
                            {
                                var checktemplate = _issueManagerContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Notice_Generation"
                                                  && a.ISES_MailActiveFlag == true).ToList();

                                if (checktemplate.Count == 0)
                                {
                                    data.message = "Email Template not Mapped to the selected Institution";
                                }
                                else
                                {
                                    var getemployeedetails1 = (from a in _issueManagerContext.MasterEmployee
                                                               from b in _issueManagerContext.HR_Master_Department
                                                               from c in _issueManagerContext.HR_Master_Designation

                                                               from e in _issueManagerContext.HR_Master_Grade
                                                               where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                                                               && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
                                                               select new ISM_DailyReportGenerationDTO
                                                               {
                                                                   employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                                   + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                                   + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                                   department = b.HRMD_DepartmentName,
                                                                   desgination = c.HRMDES_DesignationName,
                                                                   gradename = e.HRMG_GradeName,
                                                                   HRMD_Id = a.HRMD_Id,
                                                                   emailid = a.HRME_EmailId
                                                               }).Distinct().ToList();
                                    var emplname = getemployeedetails1.FirstOrDefault().employeename;
                                    var deptname = getemployeedetails1.FirstOrDefault().department;
                                    var desginame = getemployeedetails1.FirstOrDefault().desgination;
                                    var gradname = getemployeedetails1.FirstOrDefault().gradename;
                                    var email = getemployeedetails1.FirstOrDefault().emailid;

                                    string templatedetails1 = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                                    string date = "";
                                    DateTime fromdatecon = DateTime.Now;
                                    fromdatecon = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd"));
                                    date = fromdatecon.ToString("yyyy-MM-dd");

                                    //===================bcc mail===========
                                    string emailccdetails = "";
                                    string emailbccdetails = "";
                                    string name_cc = "";
                                    string name_bcc = "";

                                    var emaildetails = _issueManagerContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == getemployeedetails1.FirstOrDefault().HRMD_Id).ToList();
                                    if (emaildetails.Count > 0)
                                    {
                                        emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                                        emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;



                                    }
                                    else
                                    {
                                        emailccdetails = "";
                                        emailbccdetails = "";
                                    }
                                    //==============================bccmailend=======
                                    //=================getdate to mail==========

                                    templatedetails1 = templatedetails1.Replace("[DeveloperName]", emplname);
                                    templatedetails1 = templatedetails1.Replace("[Department]", deptname);
                                    templatedetails1 = templatedetails1.Replace("[Designation]", gradname);
                                    templatedetails1 = templatedetails1.Replace("[Deviation]", Convert.ToDecimal(deviation.ISMWD_Deviation).ToString());
                                    templatedetails1 = templatedetails1.Replace("[DATENAME]", date);
                                    templatedetails1 = templatedetails1.Replace("[CCTo]", emailccdetails);


                                    try
                                    {
                                        send_notice_deviation(data.MI_Id, email, templatedetails1, emailccdetails);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        _logger.LogInformation("Daily Report Generation generatedailyreport EMAIL For Employee Id : " + empid + "   : " + ex.Message);
                                    }
                                    data.returnval = true;
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.returnval = true;
                }

            }
            catch (Exception ex)
            {
                if (counttrue == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Daily Report Generation generatedailyreport For Employee Id : " + empid + "   : " + ex.Message);
            }
            return data;
        }

        public ISM_DailyReportGenerationDTO TriggerPlannerMail(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                List<ISM_DailyReportGenerationDTO> Employeelist = new List<ISM_DailyReportGenerationDTO>();


                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DRNOTSENT_EMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
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

                                Employeelist.Add(new ISM_DailyReportGenerationDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_ID"]),
                                    DRNotSentDate = Convert.ToString(dataReader["DRnotsentdate"]),
                                    employeename = Convert.ToString(dataReader["Employeename"]),



                                });

                            }
                        }

                        //  Employeelist = retObject.ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (Employeelist != null && Employeelist.Count > 0)
                {
                    for (var i = 0; i < Employeelist.Count; i++)
                    {

                        string emailid = _db.Multiple_Email_DMO.Where(t => t.HRME_Id == Employeelist[i].HRME_Id && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                        if (emailid != "")
                        {
                            sendmail(data.MI_Id, emailid, "DRNOTSENDMAIL", Employeelist[i].HRME_Id, Employeelist[i].DRNotSentDate, Employeelist[i].employeename);

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

        public string sendmail(long mi_id, string email, string Template, long UserID, string DRNotSentDate, string employeename)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == mi_id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = Mailmsg.Replace("[EMPNAME]", employeename);
                Mailmsg = Mailmsg.Replace("[DATE]", DRNotSentDate);
                Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(mi_id)).ToList();

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
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(mi_id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
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

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)
    ;


                        if (Attechement.Equals("1"))
                        {
                            var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
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

                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)
    ;
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
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

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }




                    }


                    using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Value = (mi_id.ToString())

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
        public ISM_DailyReportGenerationDTO DRGenerationreject(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;
            long? deptid = 0;

            try
            {

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var hrmeid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                if (hrmeid.Length > 0)
                {
                    empid = hrmeid.FirstOrDefault().Emp_Code;
                    var hrmDempid = _issueManagerContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false
                    && t.HRME_Id == empid).Distinct().ToList();
                    if (hrmDempid.Count > 0)
                    {
                        deptid = hrmDempid.FirstOrDefault().HRMD_Id;
                    }
                }
                data.get_Status = _issueManagerContext.MasterStatus_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == deptid
              && t.ISMMISTS_ActiveFlag == true).Distinct().OrderBy(t => t.ISMMISTS_Order).ToArray();

                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generation_reject";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    //cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    //{
                    //    Value = 1
                    //});

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
                        data.DR_Generationreject = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.droveralldetailsdatewise = _issueManagerContext.ISM_DailyReport_DaywiseDMO.Where(a => a.HRME_Id == empid
                && a.ISMDRPTDW_Date.Value.Date == data.ISMDRPT_Date.Value.Date).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Daily Report Generation searchpreviousdailyreport For Employee Id : " + empid + "   : " + ex.Message);
            }
            return data;
        }

        public ISM_DailyReportGenerationDTO generateRejectedtask(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;
            long counttrue = 0;
            try
            {
                //int s = _Context.Database.ExecuteSqlCommand("AUTO_PLANNER_EXTENTION_FOR_REJECTION @p0,@p1", plannedby_id, data.ISMTPL_Id);

                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                for (int k = 0; k < data.Temp_ISM_DailyReportGenerationDTO.Count(); k++)

                {
                    var plannerid = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPL_Id;
                    var plannertaskid = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTPLTA_Id;

                    decimal? previous_timetaken = 0.00m;
                    var gettaskid = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                    List<ISM_DailyReportGenerationDMO> get_previous_dr_taskdetails = new List<ISM_DailyReportGenerationDMO>();

                    if (plannertaskid > 0)
                    {
                        get_previous_dr_taskdetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == empid
                                           && a.ISMDRPT_Date == data.ISMDRPT_Date && a.ISMTCR_Id == gettaskid && a.ISMTPL_Id == plannerid && a.ISMTPLTA_Id == plannertaskid).ToList();

                        if (get_previous_dr_taskdetails.Count > 0)
                        {
                            previous_timetaken = get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_ApprovedTime;
                        }
                    }

                    var result = _issueManagerContext.ISM_DailyReportGenerationDMO.Where(a => a.ISMDRPT_Id == get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id).FirstOrDefault();

                    decimal timeCount = Convert.ToDecimal(previous_timetaken);
                    decimal timeCountString = timeCount;

                    timeCountString += Convert.ToDecimal(data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs);



                    if (timeCountString > 0.00m)
                    {
                        var DR_TaskDetails = _issueManagerContext.ISM_DailyReportGenerationDMO.Single(a => a.HRME_Id == empid
                        && a.ISMDRPT_Date == data.ISMDRPT_Date && a.ISMTPL_Id == plannerid && a.ISMTPLTA_Id == plannertaskid && a.ISMTCR_Id == gettaskid
                        && a.ISMDRPT_Id == get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id);

                        DR_TaskDetails.ISMDRPT_Remarks = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                        //  DR_TaskDetails.ISMDRPT_TimeTakenInHrs = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;
                        DR_TaskDetails.ISMDRPT_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                        DR_TaskDetails.ISMDRPT_ApprovedFlg = null;
                        DR_TaskDetails.ISMDRPT_Date = data.ISMDRPT_Date;
                        DR_TaskDetails.UpdatedDate = DateTime.Now;

                        DR_TaskDetails.UpdatedDate = DateTime.Now;
                        DR_TaskDetails.ISMDRPT_UpdatedBy = data.UserId;
                        _issueManagerContext.Update(DR_TaskDetails);

                        if (get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id > 0)
                        {
                            var result123 = _issueManagerContext.ISM_DailyReportReapprovalDMO.Where(a => a.ISMDRPT_Id == get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id).ToList();
                            if (result123.Count > 0)
                            {
                                var finalvalue = _issueManagerContext.ISM_DailyReportReapprovalDMO.Single(a => a.ISMDRPT_Id == get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id);
                                finalvalue.ISMDRPTR_Date = result.ISMDRPT_Date;
                                finalvalue.ISMDRPTR_Remarks = result.ISMDRPT_Remarks;
                                finalvalue.ISMDRPTR_ActiveFlg = result.ISMDRPT_ActiveFlg;
                                finalvalue.ISMDRPTR_TimeTakenFlg = result.ISMDRPT_TimeTakenFlg;
                                finalvalue.ISMDRPTR_ApprovedFlg = result.ISMDRPT_ApprovedFlg;
                                finalvalue.ISMDRPTR_ApprovedTime = 0;
                                finalvalue.ISMDRPTR_TimeTakenInHrs = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;
                                finalvalue.ISMDRPTR_ApprovedTimeFlg = result.ISMDRPT_ApprovedTimeFlg;
                                finalvalue.ISMDRPTR_UpdatedBy = data.UserId;
                                finalvalue.ISMDRPTR_Status = data.ISMDRPT_Status;

                                finalvalue.UpdatedDate = DateTime.Now;
                                _issueManagerContext.Update(finalvalue);
                            }
                            else
                            {
                                ISM_DailyReportReapprovalDMO ISM_DailyReportReapprovalDMO = new ISM_DailyReportReapprovalDMO();

                                ISM_DailyReportReapprovalDMO.HRME_Id = result.HRME_Id;
                                ISM_DailyReportReapprovalDMO.ISMDRPT_Id = result.ISMDRPT_Id;
                                ISM_DailyReportReapprovalDMO.ISMDRPTR_Remarks = result.ISMDRPT_Remarks;
                                ISM_DailyReportReapprovalDMO.ISMDRPTR_ApprovedFlg = result.ISMDRPT_ApprovedFlg;
                                ISM_DailyReportReapprovalDMO.ISMDRPTR_ApprovedTime = 0;
                                ISM_DailyReportReapprovalDMO.ISMDRPTR_Status = data.ISMDRPT_Status;
                                ISM_DailyReportReapprovalDMO.ISMDRPTR_TimeTakenInHrs = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_TimeTakenInHrs;
                                ISM_DailyReportReapprovalDMO.ISMDRPTR_ActiveFlg = true;
                                ISM_DailyReportReapprovalDMO.ISMDRPTR_CreatedBy = data.UserId;
                                ISM_DailyReportReapprovalDMO.ISMDRPTR_UpdatedBy = data.UserId;
                                ISM_DailyReportReapprovalDMO.CreatedDate = DateTime.Now;
                                ISM_DailyReportReapprovalDMO.UpdatedDate = DateTime.Now;

                                _issueManagerContext.Add(ISM_DailyReportReapprovalDMO);
                            }
                        }


                        var checktaskcreation = _issueManagerContext.ISM_TaskCreationDMO.Where(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id).ToList();

                        if (checktaskcreation.Count() > 0)
                        {
                            var checktaskcreationresult = _issueManagerContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id);

                            checktaskcreationresult.ISMTCR_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                            checktaskcreationresult.ISMTCR_UpdatedBy = data.UserId;
                            checktaskcreationresult.UpdatedDate = DateTime.Now;
                            _issueManagerContext.Update(checktaskcreationresult);
                        }



                        if (data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList != null)
                        {
                            foreach (var taskatt in data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList)
                            {
                                DateTime ISMDRPT_Date = DateTime.Parse(result.ISMDRPT_Date.ToString());
                                ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();
                                taskresponse.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                                taskresponse.HRME_Id = empid;
                                taskresponse.ISMTCRRES_ResponseDate = ISMDRPT_Date;
                                taskresponse.ISMTCRRES_Response = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                                taskresponse.ISMTCRRES_ResponseAttachment = taskatt.filepath;
                                taskresponse.ISMTCRRES_ResponsePageName = "DR Reject";
                                taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                                taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                                taskresponse.ISMTCRRES_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                                taskresponse.ISMTCRRES_ActiveFlg = true;
                                taskresponse.CreatedDate = DateTime.Now;
                                taskresponse.UpdatedDate = DateTime.Now;
                                _issueManagerContext.Add(taskresponse);
                            }
                        }
                        else
                        {
                            DateTime ISMDRPT_Date = DateTime.Parse(result.ISMDRPT_Date.ToString());
                            ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();

                            taskresponse.ISMTCR_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMTCR_Id;
                            taskresponse.HRME_Id = empid;
                            taskresponse.ISMTCRRES_ResponseDate = ISMDRPT_Date;
                            taskresponse.ISMTCRRES_Response = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Remarks;
                            taskresponse.ISMTCRRES_Status = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Status;
                            taskresponse.ISMTCRRES_ResponseAttachment = "";
                            taskresponse.ISMTCRRES_ResponsePageName = "DR Reject";
                            taskresponse.ISMTCRRES_ActiveFlg = true;
                            taskresponse.CreatedDate = DateTime.Now;
                            taskresponse.UpdatedDate = DateTime.Now;
                            taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                            taskresponse.ISMTCRRES_UpdatedBy = data.UserId;

                            _issueManagerContext.Add(taskresponse);

                        }











                        if (data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList != null && data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList.Length > 0)
                        {
                            if (get_previous_dr_taskdetails.Count > 0)
                            {
                                Array Remove_files = _issueManagerContext.ISM_DailyReport_FilesDMO.Where(a => a.ISMDRPT_Id == get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id).ToArray();

                                foreach (var c in Remove_files)
                                {
                                    _issueManagerContext.Remove(c);
                                }
                            }



                            foreach (var c in data.Temp_ISM_DailyReportGenerationDTO[k].Temp_TaskReponseList)
                            {
                                ISM_DailyReport_FilesDMO iSM_DailyReport_FilesDMO = new ISM_DailyReport_FilesDMO();
                                if (previous_timetaken == 0.00m)
                                {
                                    iSM_DailyReport_FilesDMO.ISMDRPT_Id = data.Temp_ISM_DailyReportGenerationDTO[k].ISMDRPT_Id;
                                }
                                else
                                {
                                    iSM_DailyReport_FilesDMO.ISMDRPT_Id = get_previous_dr_taskdetails.FirstOrDefault().ISMDRPT_Id;
                                }

                                //iSM_DailyReport_FilesDMO.ISMMTCATCL_Id = c.ISMMTCATCL_Id;
                                iSM_DailyReport_FilesDMO.ISMDRPTFL_FileName = c.filename;
                                //iSM_DailyReport_FilesDMO.ISMDRPTFL_RefNo = c.refno;
                                //iSM_DailyReport_FilesDMO.ISMDRPTFL_Remarks = c.comments;
                                iSM_DailyReport_FilesDMO.ISMDRPT_ActiveFlg = true;
                                iSM_DailyReport_FilesDMO.ISMDRPT_CreatedDate = DateTime.Now;
                                iSM_DailyReport_FilesDMO.ISMDRPT_UpdatedDate = DateTime.Now;
                                iSM_DailyReport_FilesDMO.ISMDRPT_CreatedBy = data.UserId;
                                iSM_DailyReport_FilesDMO.ISMDRPT_UpdatedBy = data.UserId;
                                iSM_DailyReport_FilesDMO.ISMDRPTFL_FilePath = c.filepath;
                                _issueManagerContext.Add(iSM_DailyReport_FilesDMO);

                            }
                        }

                    }



                }
                var r = _issueManagerContext.SaveChanges();
                if (r > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {


            }
            return data;
        }
        public ISM_DailyReportGenerationDTO saveDRremarks(ISM_DailyReportGenerationDTO data)
        {
            long empid = 0;

            try
            {
                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                ISM_Task_Regenarate_RemarksDMO ISM_Task_Regenarate_RemarksDMO = new ISM_Task_Regenarate_RemarksDMO();
                ISM_Task_Regenarate_RemarksDMO.MI_Id = data.MI_Id;
                ISM_Task_Regenarate_RemarksDMO.HRME_Id = hrmEempid.FirstOrDefault().Emp_Code;
                ISM_Task_Regenarate_RemarksDMO.ISMTRR_Remarks = data.ISMTRR_Remarks;
                ISM_Task_Regenarate_RemarksDMO.ISMTRR_RemarksDate = indiantime0;
                ISM_Task_Regenarate_RemarksDMO.ISMTRR_CreatedDate = indiantime0;
                ISM_Task_Regenarate_RemarksDMO.ISMTRR_UpdateDate = indiantime0;
                ISM_Task_Regenarate_RemarksDMO.ISMTRR_CreatedBy = data.UserId;
                ISM_Task_Regenarate_RemarksDMO.ISMTRR_UpdatedBy = data.UserId;
                _issueManagerContext.Add(ISM_Task_Regenarate_RemarksDMO);
                var i = _issueManagerContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {


            }
            return data;
        }
    }
}
