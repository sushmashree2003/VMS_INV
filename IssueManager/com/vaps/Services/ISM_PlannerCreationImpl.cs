using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
    public class ISM_PlannerCreationImpl : Interfaces.ISM_PlannerCreationInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_PlannerCreationImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_PlannerCreationDTO> getdetails(ISM_PlannerCreationDTO data)        {            try            {
                //planercreation
                var hrmEempidppp = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                if (hrmEempidppp !=null && hrmEempidppp.Length > 0)
                {
                  long  empidf = hrmEempidppp.FirstOrDefault().Emp_Code;
                    var hrmDempid = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.HRME_Id == empidf).Distinct().ToList();
                    if (hrmDempid!=null && hrmDempid.Count > 0)
                    {
                        data.HRME_Id = hrmDempid.FirstOrDefault().HRME_Id;
                    }
                }
                else
                {
                    data.HRME_Id = 0;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
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

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RTO_FOObservationcountcheck";
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
                //detailas
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);                List<long?> department_ids = new List<long?>();                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();                long empid = 0;                if (hrmEempid.Length > 0)                {                    empid = hrmEempid.FirstOrDefault().Emp_Code;                }                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == empid).Distinct().ToArray();                string departmentid = "0";                if (hrmeDepartment_id.Length > 0)                {                    foreach (var ei in hrmeDepartment_id)                    {                        departmentid = departmentid + "," + ei.HRMD_Id;                        department_ids.Add(ei.HRMD_Id);                    }                }                data.get_Status = _IssueContext.MasterStatus_DMO.Where(t => t.MI_Id == data.MI_Id && department_ids.Contains(t.HRMD_Id) && t.ISMMISTS_ActiveFlag == true).Distinct().OrderBy(t => t.ISMMISTS_Order).ToArray();

                
                data.holidaylist = _IssueContext.FO_Master_HolidayWorkingDay_DatesDMO.Where(t => t.MI_Id == data.MI_Id && t.FOHWDT_Id == 1 && t.FOMHWD_ActiveFlg == true).Select(t => t.FOMHWDD_FromDate).Distinct().ToArray();
                //======================= Planner List
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "ISM_Planner_List";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",             SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",            SqlDbType.VarChar)                    {                        Value = empid                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = await cmd.ExecuteReaderAsync())                        {                            while (await dataReader.ReadAsync())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.get_plannerlist = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }
                //================ Assigned Task List

                
                string startvalue = "";                string endvalue = "";                               if (data.ISMTPL_StartDate == null && data.ISMTPL_EndDate == null)                {                    startvalue = data.ISMTPL_StartDate.ToString();                    endvalue = data.ISMTPL_EndDate.ToString();                }
                //      using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //      {
                //          cmd.CommandText = "ISM_Assigned_TaskList";
                //          cmd.CommandType = CommandType.StoredProcedure;

                //          cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //   SqlDbType.BigInt)
                //          {
                //              Value = data.MI_Id
                //          });

                //          cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                //  SqlDbType.VarChar)
                //          {
                //              Value = empid
                //          });
                //          cmd.Parameters.Add(new SqlParameter("@startDate",
                //   SqlDbType.VarChar)
                //          {
                //              Value = startvalue
                //          });
                //          cmd.Parameters.Add(new SqlParameter("@endDate",
                //  SqlDbType.VarChar)
                //          {
                //              Value = endvalue
                //          });
                //          cmd.Parameters.Add(new SqlParameter("@user_Id",
                //SqlDbType.VarChar)
                //          {
                //              Value = data.UserId
                //          });

                //          if (cmd.Connection.State != ConnectionState.Open)
                //              cmd.Connection.Open();

                //          var retObject = new List<dynamic>();
                //          try
                //          {
                //              using (var dataReader = await cmd.ExecuteReaderAsync())
                //              {
                //                  while (await dataReader.ReadAsync())
                //                  {
                //                      var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                      for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                      {
                //                          dataRow.Add(
                //                              dataReader.GetName(iFiled),
                //                             dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                          );
                //                      }
                //                      retObject.Add((ExpandoObject)dataRow);
                //                  }
                //              }
                //              data.get_Assignedtasklist = retObject.ToArray();
                //          }
                //          catch (Exception ex)
                //          {
                //              Console.WriteLine(ex.Message);
                //          }
                //      }
                //************ Planner Creation Extension ******************//
                var getplannernamecount = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid && t.ISMTPL_ActiveFlg == true && t.ISMTPL_ApprovalFlg == false && t.ISMTPL_ApprovedBy == 0 && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).ToList();                if (getplannernamecount.Count == 0)                {                    data.plannerextapproval = false;                    data.plannerMaxdate = indianTime.AddDays(7 - (int)indianTime.DayOfWeek);

                    //******************************* Calculate Effort *****************************//
                    DateTime datetimennow = DateTime.Now;                    datetimennow = Convert.ToDateTime(datetimennow.Date.ToString("yyyy-MM-dd"));                    long lyear_id = 0;                    var yearid = _IssueContext.HR_Master_LeaveYearDMO.Where(l => l.MI_Id == data.MI_Id && l.HRMLY_ActiveFlag == true && (l.HRMLY_FromDate <= datetimennow && l.HRMLY_ToDate >= datetimennow)).Distinct().ToList();                    var dep_id = _IssueContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;                    if (yearid.Count > 0)                    {                        lyear_id = yearid.FirstOrDefault().HRMLY_Id;                    }                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())                    {                        cmd.CommandText = "ISM_PLANNER_CREATION_CHECK";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@MI_Id",                SqlDbType.BigInt)                        {                            Value = data.MI_Id                        });                        cmd.Parameters.Add(new SqlParameter("@HRMLY_Id",              SqlDbType.BigInt)                        {                            Value = lyear_id                        });                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",               SqlDbType.BigInt)                        {                            Value = empid                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        var retObject = new List<dynamic>();                        try                        {                            using (var dataReader = await cmd.ExecuteReaderAsync())                            {                                while (await dataReader.ReadAsync())                                {                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                    {                                        dataRow.Add(                                            dataReader.GetName(iFiled),                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]                                        );                                    }                                    retObject.Add((ExpandoObject)dataRow);                                }                            }                            var datelist = retObject.ToList();                            if (datelist.Count > 0)                            {                                if (Convert.ToDateTime(System.DateTime.Today.Date) == datelist[0].PDates)                                {                                    data.plannerextapproval = true;                                    string Day = indianTime.DayOfWeek.ToString();                                    if (Day.Equals("Sunday", StringComparison.OrdinalIgnoreCase))                                    {                                        data.plannerMaxdate = indianTime.Date;                                    }                                    var planner_extension = _IssueContext.ISM_PlannerExtensionDMO.Where(a => a.HRME_Id == empid && a.ISMPLE_ActiveFlg == true).Distinct().ToList();                                    data.plannerextension = planner_extension.ToArray();                                    if (planner_extension.Count > 0)                                    {                                        var acd_Id = _IssueContext.ISM_PlannerExtensionDMO.Where(t => t.HRME_Id == empid && Convert.ToDateTime(t.ISMPLE_FromDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ISMPLE_ToDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).OrderByDescending(t => t.CreatedDate).ToList();                                        if (acd_Id.Count > 0)                                        {                                            data.plannerextapproval = true;                                            var extdate = acd_Id.FirstOrDefault().ISMPLE_FromDate;                                            data.plannerMaxdate = extdate;                                        }                                    }                                }                                else                                {                                    var planner_extension = _IssueContext.ISM_PlannerExtensionDMO.Where(a => a.HRME_Id == empid && a.ISMPLE_ActiveFlg == true).Distinct().ToList();                                    data.plannerextension = planner_extension.ToArray();                                    if (planner_extension.Count > 0)                                    {                                        var acd_Id = _IssueContext.ISM_PlannerExtensionDMO.Where(t => t.HRME_Id == empid && Convert.ToDateTime(t.ISMPLE_FromDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ISMPLE_ToDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).OrderByDescending(t => t.CreatedDate).ToList();                                        if (acd_Id.Count > 0)                                        {                                            data.plannerextapproval = true;                                            var extdate = acd_Id.FirstOrDefault().ISMPLE_FromDate;                                            data.plannerMaxdate = extdate;                                        }                                    }                                    if (data.plannerextapproval == false)                                    {                                        using (var cmd1 = _IssueContext.Database.GetDbConnection().CreateCommand())                                        {                                            cmd1.CommandText = "ISM_Planner_CreationCheck_Next";                                            cmd1.CommandType = CommandType.StoredProcedure;                                            cmd1.Parameters.Add(new SqlParameter("@MI_Id",                                    SqlDbType.BigInt)                                            {                                                Value = data.MI_Id                                            });                                            cmd1.Parameters.Add(new SqlParameter("@HRMLY_Id",                                  SqlDbType.BigInt)                                            {                                                Value = lyear_id                                            });                                            cmd1.Parameters.Add(new SqlParameter("@HRME_Id",                                   SqlDbType.BigInt)                                            {                                                Value = empid                                            });                                            if (cmd1.Connection.State != ConnectionState.Open)                                                cmd1.Connection.Open();                                            var retObject1 = new List<dynamic>();                                            try                                            {                                                using (var dataReader1 = await cmd1.ExecuteReaderAsync())                                                {                                                    while (await dataReader1.ReadAsync())                                                    {                                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;                                                        for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)                                                        {                                                            dataRow.Add(                                                                dataReader1.GetName(iFiled),                                                               dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]                                                            );                                                        }                                                        retObject1.Add((ExpandoObject)dataRow);                                                    }                                                }                                                var countarray = retObject1.ToArray();                                                if (countarray.Length == 0)                                                {
                                                    //ISM_PlannerCreationDTO dates = new ISM_PlannerCreationDTO();
                                                    //dates = startenddays(data);
                                                    string Day = indianTime.DayOfWeek.ToString();                                                    if (Day.Equals("Sunday", StringComparison.OrdinalIgnoreCase))                                                    {                                                        var nextplanner = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid && Convert.ToDateTime(t.ISMTPL_StartDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).ToList();                                                        if (nextplanner.Count() == 0)                                                        {                                                            data.plannerextapproval = true;                                                            data.plannerMaxdate = indianTime.Date;                                                        }                                                    }                                                    else                                                    {                                                        DateTime plannercreation = datelist[0].PDates;                                                        if (Convert.ToDateTime(System.DateTime.Today.Date) > Convert.ToDateTime(plannercreation.Date))                                                        {                                                            var nextplanner = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid && Convert.ToDateTime(t.ISMTPL_StartDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).ToList();                                                            if (nextplanner.Count() == 0)                                                            {                                                                data.plannerextapproval = true;                                                            }                                                        }                                                    }
                                                }                                            }                                            catch (Exception ex)                                            {                                                Console.WriteLine(ex.Message);                                            }                                        }                                    }                                }                            }                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }                else                {                    data.plannerextapproval = false;                    data.plannerMaxdate = indianTime.AddDays(7 - (int)indianTime.DayOfWeek);                }                var plnnerdate = _IssueContext.ISM_Task_PlannerDMO.Where(p => p.MI_Id == data.MI_Id && p.ISMTPL_ActiveFlg == true && p.HRME_Id == empid && ((p.ISMTPL_ApprovalFlg == false && p.ISMTPL_ApprovedBy == 0) || (p.ISMTPL_ApprovalFlg == true && p.ISMTPL_ApprovedBy != 0))                ).OrderByDescending(e => e.ISMTPL_EndDate).Take(1).ToList();                if (plnnerdate.Count > 0)                {                    DateTime a = DateTime.Now;                    a = Convert.ToDateTime(plnnerdate[0].ISMTPL_EndDate);                    if (a > data.plannerMaxdate)                    {                        data.plannerMaxdate = a;                        data.plannerMaxdate = data.plannerMaxdate.AddDays(1).Date;                    }                }
                //*******************************GET EMPLOYEE?DEPARTMENT EFFORTS *****************************//
                var dep_idnew = _IssueContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;                var categorylist = (from a in _IssueContext.ISM_Master_TaskCategory_DMO                                    from b in _IssueContext.ISM_Master_TaskCategory_Employee_DMO                                    where a.MI_Id == data.MI_Id && a.ISMMTCAT_Id == b.ISMMTCAT_Id                                    && b.HRME_Id == empid && a.HRMD_Id == dep_idnew && a.ISMMTCAT_ActiveFlag == true && b.ISMMTCATE_ActiveFlag == true                                    select new ISM_PlannerCreationDTO                                    {                                        ISMMTCAT_Id = a.ISMMTCAT_Id,                                        ISMMTCAT_TaskCategoryName = a.ISMMTCAT_TaskCategoryName,                                        ISMMTCAT_TaskPercentage = a.ISMMTCAT_TaskPercentage,                                        ISMMTCAT_DurationFlg = a.ISMMTCAT_DurationFlg,                                        ISMMTCAT_EachTaskMaxDuration = a.ISMMTCAT_EachTaskMaxDuration,                                        ISMMTCAT_CompulsoryFlg = a.ISMMTCAT_CompulsoryFlg                                    }).Distinct().ToList();                if (categorylist.Count == 0)                {                    categorylist = (from a in _IssueContext.ISM_Master_TaskCategory_DMO                                    where a.MI_Id == data.MI_Id && a.HRMD_Id == dep_idnew && a.ISMMTCAT_ActiveFlag == true                                    select new ISM_PlannerCreationDTO                                    {                                        ISMMTCAT_Id = a.ISMMTCAT_Id,                                        ISMMTCAT_TaskCategoryName = a.ISMMTCAT_TaskCategoryName,                                        ISMMTCAT_TaskPercentage = a.ISMMTCAT_TaskPercentage,                                        ISMMTCAT_DurationFlg = a.ISMMTCAT_DurationFlg,                                        ISMMTCAT_EachTaskMaxDuration = a.ISMMTCAT_EachTaskMaxDuration,                                        ISMMTCAT_CompulsoryFlg = a.ISMMTCAT_CompulsoryFlg                                    }).Distinct().ToList();                }                else                {                    categorylist = (from a in _IssueContext.ISM_Master_TaskCategory_DMO                                    from b in _IssueContext.ISM_Master_TaskCategory_Employee_DMO                                    where a.MI_Id == data.MI_Id && a.ISMMTCAT_Id == b.ISMMTCAT_Id                                    && b.HRME_Id == empid && a.HRMD_Id == dep_idnew && a.ISMMTCAT_ActiveFlag == true && b.ISMMTCATE_ActiveFlag == true                                    select new ISM_PlannerCreationDTO                                    {                                        ISMMTCAT_Id = a.ISMMTCAT_Id,                                        ISMMTCAT_TaskCategoryName = a.ISMMTCAT_TaskCategoryName,                                        ISMMTCAT_TaskPercentage = b.ISMMTCATE_TaskPercentage,                                        ISMMTCAT_DurationFlg = a.ISMMTCAT_DurationFlg,                                        ISMMTCAT_EachTaskMaxDuration = a.ISMMTCAT_EachTaskMaxDuration,                                        ISMMTCAT_CompulsoryFlg = Convert.ToBoolean(b.ISMMTCATE_CompulsoryFlg)                                    }).Distinct().ToList();                }                data.categorylist = categorylist.ToArray();
                if (data.plannerextapproval ==false)
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Planner_RejectList";

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.VarChar)
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
                            data.planner_Rejectlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if(data.planner_Rejectlist.Length > 0)
                    {
                        data.plannerextapproval = true;
                    }
                    else
                    {
                        data.plannerextapproval = false;
                    }
                }


                //get_plannercategory
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_Report_Count_Dailyreport_Block_week";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });


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
                        data.get_plannercategory = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }
            return data;
        }
        public ISM_PlannerCreationDTO startenddays(ISM_PlannerCreationDTO data)
        {
            try
            {
                string day = "";
                List<ISM_PlannerReportsDTO> empdevlist = new List<ISM_PlannerReportsDTO>();
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
                                empdevlist.Add(new ISM_PlannerReportsDTO
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
                        data.startDate = empdevlist[j].startdate;
                        data.endDate = empdevlist[j].enddate;
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
        public async Task<ISM_PlannerCreationDTO> gettaskdetails(ISM_PlannerCreationDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_TaskDeatils";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
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
                        data.get_taskdetails = retObject.ToArray();
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
        public async Task<ISM_PlannerCreationDTO> geteffortdetails(ISM_PlannerCreationDTO data)
        {
            try
                
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                DateTime datetimennow = DateTime.Now;
                datetimennow = Convert.ToDateTime(datetimennow.Date.ToString("yyyy-MM-dd"));
                long lyear_id = 0;
                var yearid = _IssueContext.HR_Master_LeaveYearDMO.Where(l => l.MI_Id == data.MI_Id && l.HRMLY_ActiveFlag == true && (l.HRMLY_FromDate <= datetimennow && l.HRMLY_ToDate >= datetimennow)).Distinct().ToList();

                var dep_id = _IssueContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;

                if (yearid.Count > 0)
                {
                    lyear_id = yearid.FirstOrDefault().HRMLY_Id;
                }

                DateTime today = Convert.ToDateTime(data.ISMTPL_StartDate);
                var yesterday = today.AddDays(-1);
                DateTime WeekStart = today.AddDays(-(int)today.DayOfWeek);
                DateTime WeekEnd = WeekStart.AddDays(7).AddSeconds(-1);

                data.WeekStart = WeekStart;
                data.WeekEnd = WeekEnd;

                //******************************* Calculate Effort *****************************//
                //      using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //      {
                //          cmd.CommandText = "ISM_bwDates";
                //          cmd.CommandType = CommandType.StoredProcedure;

                //          cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //  SqlDbType.BigInt)
                //          {
                //              Value = data.MI_Id
                //          });
                //          cmd.Parameters.Add(new SqlParameter("@HRMLY_Id",
                //SqlDbType.BigInt)
                //          {
                //              Value = lyear_id
                //          });

                //          cmd.Parameters.Add(new SqlParameter("@startDate",
                //   SqlDbType.DateTime)
                //          {
                //              Value = data.ISMTPL_StartDate
                //          });

                //          cmd.Parameters.Add(new SqlParameter("@endDate",
                //  SqlDbType.DateTime)
                //          {
                //              Value = data.ISMTPL_EndDate
                //          });

                //          if (cmd.Connection.State != ConnectionState.Open)
                //              cmd.Connection.Open();

                //          var retObject = new List<dynamic>();
                //          try
                //          {
                //              using (var dataReader = await cmd.ExecuteReaderAsync())
                //              {
                //                  while (await dataReader.ReadAsync())
                //                  {
                //                      var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                      for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                      {
                //                          dataRow.Add(
                //                              dataReader.GetName(iFiled),
                //                             dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                          );
                //                      }
                //                      retObject.Add((ExpandoObject)dataRow);
                //                  }
                //              }
                //              data.get_effortdetails = retObject.ToArray();
                //          }
                //          catch (Exception ex)
                //          {
                //              Console.WriteLine(ex.Message);
                //          }
                //      }



                //******************************* Calculate Effort *****************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_bwDates_Planner";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id",
          SqlDbType.BigInt)
                    {
                        Value = lyear_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@startDate",
             SqlDbType.DateTime)
                    {
                        Value = data.ISMTPL_StartDate
                    });

                    cmd.Parameters.Add(new SqlParameter("@endDate",
            SqlDbType.DateTime)
                    {
                        Value = data.ISMTPL_EndDate
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
                        data.get_effortdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //******************************* Assigned Task List Date-wise *****************************//

                //================  YEAR PLANNER TASKS REJECTED OR NOT ====================
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "ISM_YP_rejected";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",                    SqlDbType.VarChar)                    {
                        Value = empid
                     
                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = await cmd.ExecuteReaderAsync())                        {                            while (await dataReader.ReadAsync())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.get_taskproject = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

                //================ END  YEAR PLANNER TASKS REJECTED OR NOT ====================

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.ISMTPL_StartDate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.ISMTPL_EndDate), INDIAN_ZONE);

                string confromdate = "";
                string contodate = "";


                try
                {
                    // fromdatecon = Convert.ToDateTime(data.ISMTPL_StartDate.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    // todatecon = Convert.ToDateTime(data.ISMTPL_EndDate.Value.Date.ToString("yyyy-MM-dd"));
                    contodate = todatecon.ToString("yyyy-MM-dd");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_ASSIGNED_TASKLIST_WITH_ADVPLANNER";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@startDate",
             SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@endDate",
            SqlDbType.VarChar)
                    {
                        Value = contodate
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
          SqlDbType.VarChar)
                    {
                        Value = data.UserId
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
                        data.get_Assignedtasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //*******************************GET EMPLOYEE?DEPARTMENT EFFORTS *****************************//

                var categorylist = (from a in _IssueContext.ISM_Master_TaskCategory_DMO
                                    from b in _IssueContext.ISM_Master_TaskCategory_Employee_DMO
                                    where a.MI_Id == data.MI_Id && a.ISMMTCAT_Id == b.ISMMTCAT_Id
                                    && b.HRME_Id == empid && a.HRMD_Id == dep_id && a.ISMMTCAT_ActiveFlag == true && b.ISMMTCATE_ActiveFlag == true
                                    select new ISM_PlannerCreationDTO
                                    {
                                        ISMMTCAT_Id = a.ISMMTCAT_Id,
                                        ISMMTCAT_TaskCategoryName = a.ISMMTCAT_TaskCategoryName,
                                        ISMMTCAT_TaskPercentage = a.ISMMTCAT_TaskPercentage,
                                        ISMMTCAT_DurationFlg = a.ISMMTCAT_DurationFlg,
                                        ISMMTCAT_EachTaskMaxDuration = a.ISMMTCAT_EachTaskMaxDuration,

                                    }).Distinct().ToList();

                if (categorylist.Count == 0)
                {
                    categorylist = (from a in _IssueContext.ISM_Master_TaskCategory_DMO
                                    where a.MI_Id == data.MI_Id && a.ISMMTCAT_CompulsoryFlg == true && a.HRMD_Id == dep_id && a.ISMMTCAT_ActiveFlag == true
                                    select new ISM_PlannerCreationDTO
                                    {
                                        ISMMTCAT_Id = a.ISMMTCAT_Id,
                                        ISMMTCAT_TaskCategoryName = a.ISMMTCAT_TaskCategoryName,
                                        ISMMTCAT_TaskPercentage = a.ISMMTCAT_TaskPercentage,
                                        ISMMTCAT_DurationFlg = a.ISMMTCAT_DurationFlg,
                                        ISMMTCAT_EachTaskMaxDuration = a.ISMMTCAT_EachTaskMaxDuration,

                                    }).Distinct().ToList();
                }
                else
                {

                    categorylist = (from a in _IssueContext.ISM_Master_TaskCategory_DMO
                                    from b in _IssueContext.ISM_Master_TaskCategory_Employee_DMO
                                    where a.MI_Id == data.MI_Id && a.ISMMTCAT_Id == b.ISMMTCAT_Id
                                    && b.HRME_Id == empid && a.HRMD_Id == dep_id && a.ISMMTCAT_ActiveFlag == true && b.ISMMTCATE_ActiveFlag == true && b.ISMMTCATE_CompulsoryFlg == true
                                    select new ISM_PlannerCreationDTO
                                    {
                                        ISMMTCAT_Id = a.ISMMTCAT_Id,
                                        ISMMTCAT_TaskCategoryName = a.ISMMTCAT_TaskCategoryName,
                                        ISMMTCAT_TaskPercentage = a.ISMMTCAT_TaskPercentage,
                                        ISMMTCAT_DurationFlg = a.ISMMTCAT_DurationFlg,
                                        ISMMTCAT_EachTaskMaxDuration = a.ISMMTCAT_EachTaskMaxDuration,

                                    }).Distinct().ToList();
                }

                data.categorylist = categorylist.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                if (data.addTaskArray != null)
                {
                    foreach (var pa in data.addTaskArray)
                    {
                        ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                        taskasign.ISMTCR_Id = pa.ISMTCR_Id;
                        taskasign.HRME_Id = empid;
                        taskasign.ISMTCRASTO_AssignedDate = data.ISMTCRASTO_AssignedDate;
                        taskasign.ISMTCRASTO_AssignedBy = empid;
                        taskasign.ISMTCRASTO_Remarks = pa.ISMTCRASTO_Remarks;
                        taskasign.ISMTCRASTO_StartDate = pa.ISMTCRASTO_StartDate;
                        taskasign.ISMTCRASTO_EndDate = pa.ISMTCRASTO_EndDate;
                        taskasign.ISMTCRASTO_EffortInHrs = pa.ISMTCRASTO_EffortInHrs;
                        taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                        taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                        taskasign.ISMTCRASTO_ActiveFlg = true;
                        taskasign.CreatedDate = indianTime;
                        taskasign.UpdatedDate = indianTime;
                        _IssueContext.Add(taskasign);
                    }
                }
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_PlannerCreationDTO savePlanner(ISM_PlannerCreationDTO data)
        {
            try
            {
                PushNotification pb = new PushNotification(_Context);
                bool approvalflg = false;
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                //var config = _IssueContext.GenConfig.Where(c => c.MI_Id == data.MI_Id).Distinct().ToList();
                //if (config.Count > 0)
                //{
                //    approvalflg= config.FirstOrDefault().IVRMGC_PlannerApprovalFlg;
                //}                
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;

                }
                var Approvalpermission = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                                          from b in _IssueContext.HR_Master_Department
                                          from c in _IssueContext.MasterEmployee
                                          where (a.HRMDC_ID == b.HRMDC_ID && b.HRMD_Id == c.HRMD_Id && c.HRME_Id == empid)
                                          select new ISM_DailyReportGenerationDTO
                                          {
                                              Approvalflag = Convert.ToBoolean(a.PlannerApproval)
                                          }).Distinct().ToList();

                if (Approvalpermission.Count > 0)
                {
                    var approvalflgpermission = Approvalpermission.FirstOrDefault().Approvalflag;
                    if (approvalflgpermission == true)
                    {
                        approvalflg = false;
                    }
                }

                ISM_Task_PlannerDMO plnr = new ISM_Task_PlannerDMO();
                plnr.MI_Id = data.MI_Id;
                plnr.HRME_Id = empid;
                plnr.ISMTPL_PlannedBy = empid;
                plnr.ISMTPL_PlannerName = data.ISMTPL_PlannerName;
                plnr.ISMTPL_Remarks = data.ISMTPL_Remarks;
                plnr.ISMTPL_StartDate = data.ISMTPL_StartDate;
                plnr.ISMTPL_EndDate = data.ISMTPL_EndDate;
                plnr.ISMTPL_TotalHrs = data.ISMTPL_TotalHrs;
                plnr.ISMTPL_ApprovalFlg = approvalflg;
                plnr.ISMTPL_ActiveFlg = true;
                plnr.ISMTPL_CreatedBy = data.UserId;
                plnr.ISMTPL_UpdatedBy = data.UserId;
                plnr.CreatedDate = indianTime;
                plnr.UpdatedDate = indianTime;
                plnr.ISMTPL_ApprovedTo = 0;
                _IssueContext.Add(plnr);

                if (data.taskplannerArray != null)
                {
                    foreach (var pl in data.taskplannerArray)
                    {
                        ISM_Task_Planner_TasksDMO plnrtask = new ISM_Task_Planner_TasksDMO();
                        plnrtask.ISMTPL_Id = plnr.ISMTPL_Id;
                        plnrtask.ISMTCR_Id = pl.ISMTCR_Id;
                        plnrtask.ISMTPLTA_Remarks = pl.ISMTPLTA_Remarks;
                        plnrtask.ISMTPLTA_StartDate = pl.ISMTPLTA_StartDate;
                        plnrtask.ISMTPLTA_EndDate = pl.ISMTPLTA_EndDate;
                        plnrtask.ISMTPLTA_EffortInHrs = pl.ISMTPLTA_EffortInHrs;

                        plnrtask.ISMTPLTA_ApprovalFlg = approvalflg;
                        plnrtask.ISMTPLTA_DeviationFlg = false;
                        plnrtask.ISMTPLTA_ExtraFlg = false;
                        plnrtask.ISMTPLTA_ActiveFlg = true;
                        plnrtask.ISMTPLTA_CreatedBy = data.UserId;
                        plnrtask.ISMTPLTA_UpdatedBy = data.UserId;
                        plnrtask.CreatedDate = indianTime;
                        plnrtask.UpdatedDate = indianTime;
                        if (pl.ISMTPLTA_Id > 0)
                        {

                            if (pl.ISMTPLTA_PreviousTask == null || pl.ISMTPLTA_PreviousTask == "false" || pl.ISMTPLTA_PreviousTask == "0" )
                            {
                                if (pl.ISMTPLTA_ApprovalFlg == null || pl.ISMTPLTA_ApprovalFlg == "false" || pl.ISMTPLTA_ApprovalFlg == "0")
                                {
                                    plnrtask.ISMTPLTA_Status = pl.ISMTPLTA_Status;
                                    plnrtask.ISMTPLTA_PreviousTask = false;
                                }
                                else
                                {
                                    plnrtask.ISMTPLTA_Status = pl.ISMTPLTA_Status;
                                    plnrtask.ISMTPLTA_PreviousTask = true;
                                }

                            }
                            else if(pl.ISMTPLTA_PreviousTask=="2")
                            {
                                plnrtask.ISMTPLTA_Status = pl.ISMTPLTA_Status;
                                plnrtask.ISMTPLTA_PreviousTask = false;
                            }

                            else
                            {
                                plnrtask.ISMTPLTA_Status = pl.ISMTPLTA_Status;
                                plnrtask.ISMTPLTA_PreviousTask = true;
                            }

                        }
                        else
                        {
                            plnrtask.ISMTPLTA_Status = "Open";
                            plnrtask.ISMTPLTA_PreviousTask = false;
                        }
                        _IssueContext.Add(plnrtask);
                    }
                }

                if (data.catlistid != null)
                {
                    foreach (var item in data.catlistid)
                    {
                        if (item.ISMMTCAT_Id != null && item.ISMMTCAT_Id >0)
                        {
                            var catemp = _IssueContext.ISM_Master_TaskCategory_Employee_DMO.Where(e => e.ISMMTCAT_Id == item.ISMMTCAT_Id && e.HRME_Id == empid && e.ISMMTCATE_ActiveFlag == true).ToList();

                            var cat = _IssueContext.ISM_Master_TaskCategory_DMO.Single(e => e.ISMMTCAT_Id == item.ISMMTCAT_Id);

                            decimal TaskPercentage = 0.0M;
                            bool? CompulsoryFlg = false;

                            if (catemp.Count > 0)
                            {
                                var catempinsert = _IssueContext.ISM_Master_TaskCategory_Employee_DMO.Single(e => e.ISMMTCAT_Id == item.ISMMTCAT_Id && e.HRME_Id == empid && e.ISMMTCATE_ActiveFlag == true);

                                TaskPercentage = catempinsert.ISMMTCATE_TaskPercentage;
                                CompulsoryFlg = catempinsert.ISMMTCATE_CompulsoryFlg;
                            }
                            else
                            {
                                TaskPercentage = cat.ISMMTCAT_TaskPercentage;
                                CompulsoryFlg = cat.ISMMTCAT_CompulsoryFlg;
                            }

                            ISM_Task_Planner_CategoryDMO res = new ISM_Task_Planner_CategoryDMO();
                            res.ISMTPL_Id = plnr.ISMTPL_Id;
                            res.ISMMTCAT_Id = cat.ISMMTCAT_Id;
                            res.ISMTPLC_TaskPercentage = TaskPercentage;
                            res.ISMTPLC_EachTaskMaxDuration = cat.ISMMTCAT_EachTaskMaxDuration;
                            res.ISMTPLC_DurationFlg = cat.ISMMTCAT_DurationFlg;
                            res.ISMTPLC_CompulsoryFlg = Convert.ToBoolean(CompulsoryFlg);
                            _IssueContext.Add(res);
                        }
                    }
                }
                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                    //AUTO APPROVAL OF PLANNER PROCEDURE RUN
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "AUTOPLANNERAPPROVAL";
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

                    // ============================================== Push Notification
                    var getemployeedetails = (from a in _IssueContext.MasterEmployee
                                              from b in _IssueContext.HR_Master_Department
                                              from c in _IssueContext.HR_Master_Designation

                                              from e in _IssueContext.HR_Master_Grade
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

                    //var notificationdetails = _IssueContext.ISM_Notification_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id
                    //&& a.HRMD_Id == getemployeedetails.FirstOrDefault().HRMD_Id).ToList();
                    //var notificationdetails = (from UEM in _IssueContext.ISM_User_Employees_MappingDMO
                    //                           from HRE in _IssueContext.MasterEmployee
                    //                           where (UEM.HRME_Id == HRE.HRME_Id && HRE.HRME_ActiveFlag == true && HRE.HRME_LeftFlag == false && UEM.HRME_Id == empid)
                    //                           select new ISM_DailyReportGenerationDTO
                    //                           {
                    //                               HRME_Id = UEM.HRME_Id,
                    //                           }).Distinct().ToList();

                    //*********************************** User Employee Mapped List ********************************// 
                    var notificationuserids = (from UEM in _IssueContext.ISM_User_Employees_MappingDMO
                                               from HRE in _IssueContext.MasterEmployee
                                               where (UEM.HRME_Id == HRE.HRME_Id && HRE.HRME_ActiveFlag == true && HRE.HRME_LeftFlag == false && UEM.HRME_Id == empid && UEM.ISMUSEMM_Order == 1)
                                               select new ISM_DailyReportGenerationDTO
                                               {
                                                   UserId = UEM.User_Id,
                                               }).Distinct().ToList();
                    List<long> userids = new List<long>();
                    foreach (var n in notificationuserids)
                    {
                        userids.Add(n.UserId);
                    }

                    var notificationdetails = (from SUL in _IssueContext.Staff_User_Login
                                               where (userids.Contains(SUL.Id))
                                               select new ISM_DailyReportGenerationDTO
                                               {
                                                   HRME_Id = SUL.Emp_Code,
                                               }).Distinct().ToList();
                    var deviceidsnew = "";
                    var devicenew = "";
                    if (notificationdetails.Count > 0)
                    {
                        List<long> employeeids = new List<long>();
                        foreach (var n in notificationdetails)
                        {
                            employeeids.Add(n.HRME_Id);
                        }

                        var webnotificationlist = (from a in _IssueContext.MasterEmployee
                                                   where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id))
                                                   select new ISM_DailyReportGenerationDTO
                                                   {
                                                       HRME_Id = a.HRME_Id
                                                   }).Distinct().ToList();

                        if (webnotificationlist.Count > 0)
                        {
                            var planneridlist = _IssueContext.ISM_Task_PlannerDMO.Where(e => e.HRME_Id == empid && e.ISMTPL_ActiveFlg == true).OrderByDescending(y => y.ISMTPL_Id).Take(1).ToList();
                            if (planneridlist.Count > 0)
                            {
                                foreach (var item in webnotificationlist)
                                {
                                    string dd = pb.savewebnotification(data.MI_Id, "PLANNERWEB", planneridlist.FirstOrDefault().ISMTPL_Id, planneridlist.FirstOrDefault().ISMTPL_Id, item.HRME_Id, data.UserId, "PL");
                                }
                            }



                        }



                        var devicelist = (from a in _IssueContext.MasterEmployee
                                          where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id) && a.HRME_AppDownloadedDeviceId != null && a.HRME_AppDownloadedDeviceId != "")
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
                        if (deviceidsnew != "")
                        {
                            callnotification(devicenew, empid, plnr.ISMTPL_Id, getemployeedetails.FirstOrDefault().employeename, data.MI_Id, data.sound);

                            Insertnotification(data.MI_Id, empid, plnr.ISMTPL_Id, getemployeedetails.FirstOrDefault().employeename, data.UserId);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #region Email                  
                    //****************** EMAIL *********************** //
                    string str = "";
                    DateTime fromdatecon = DateTime.Now;

                    DateTime toatecon = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    List<ISM_PlannerCreationDTO> result = new List<ISM_PlannerCreationDTO>();
                    string confromdate = "";
                    string contodate = "";
                    string datetime = "";
                    try
                    {
                        //    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                        contodate = toatecon.ToString("dd/MM/yyyy");
                        datetime = toatecon.ToString("dd/MM/yyyy hh:mm tt");
                        // fromdatecon = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    List<ISM_PlannerCreationDTO> dtonewtemp = new List<ISM_PlannerCreationDTO>();
                    // Getting The saved Data 
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_PlannerReport_Search";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id", SqlDbType.VarChar)
                        {
                            Value = plnr.ISMTPL_Id
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
                                    dtonewtemp.Add(new ISM_PlannerCreationDTO
                                    {
                                        ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                        ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                        ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                        ISMMCLT_ClientName = Convert.ToString(dataReader["ISMMCLT_ClientName"]),
                                        HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                        ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                        ISMTPLTA_Statusnew = Convert.ToString(dataReader["ISMTPLTA_Status"]),
                                        assignedby = Convert.ToString(dataReader["assignedby"]),
                                        ISMTPLTA_StartDate = Convert.ToDateTime(dataReader["ISMTPLTA_StartDate"]),
                                        ISMTPLTA_EndDate = Convert.ToDateTime(dataReader["ISMTPLTA_EndDate"]),
                                        ISMTPLTA_EffortInHrsnew = Convert.ToString(dataReader["ISMTPLTA_EffortInHrs"]),
                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    var emaildetails = _IssueContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == getemployeedetails.FirstOrDefault().HRMD_Id).ToList();

                    string emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                    string emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;

                    var checktemplate = _IssueContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Planner_Report_Generation"
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

                        var completeEffort = data.ISMTPL_TotalHrs;
                        string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                        templatedetails = templatedetails.Replace("[DeveloperName]", emplname);
                        templatedetails = templatedetails.Replace("[DATE]", date);
                        templatedetails = templatedetails.Replace("[Department]", deptname);
                        templatedetails = templatedetails.Replace("[Designation]", gradname);
                        templatedetails = templatedetails.Replace("[CompletedEffort]", completeEffort.ToString());
                        templatedetails = templatedetails.Replace("[DATENAME]", newdate);

                        int rownumber = 0;

                        if (dtonewtemp.Count > 0)
                        {
                            for (int j = 0; j < dtonewtemp.Count; j++)
                            {
                                rownumber += 1;
                                str += "<tr>" +
                                    "<td align=" + "center" + " style=" + ">" + rownumber + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_TaskNo + "</td>" +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_Title + "</td>" +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_BugOREnhancementFlg + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMMCLT_ClientName + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].HRMP_Name + "</td> " +
                                    "<td align=" + "left" + "style=" + "word-break: break-word;" + ">" + dtonewtemp[j].ISMTCR_Desc + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLTA_Statusnew + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].assignedby + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLTA_StartDate + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLTA_EndDate + "</td> " +
                                    "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLTA_EffortInHrsnew + "</td> " +
                                    "</tr>"
                                    ;
                            }
                        }

                        templatedetails = templatedetails.Replace("[MSG]", str);

                        try
                        {
                            //sendmailexamtt(data.MI_Id, email, templatedetails, emailccdetails, emailbccdetails);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion
                }
                else
                {
                    data.returnval = false;
                }


                

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public string callnotification(string devicenew, long empid, long planner_id, string empname, long mi_id, string sound)
        {
            try
            {
                ISM_Task_PlannerDMO planner = new ISM_Task_PlannerDMO();
                var plannerdetails = _IssueContext.ISM_Task_PlannerDMO.Where(a => a.MI_Id == mi_id && a.HRME_Id == empid
                && a.ISMTPL_Id == planner_id).Distinct().ToList();
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + plannerdetails.FirstOrDefault().ISMTPL_PlannerName + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //    "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"'
                //    + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + plannerdetails.FirstOrDefault().ISMTPL_PlannerName + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";
                string plannertext = "";
                plannertext = empname + " " + "Is Created Planner, Kindly Approve the Planner!!";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + plannerdetails.FirstOrDefault().ISMTPL_PlannerName + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + plannertext + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

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

        public string Insertnotification(long MI_Id, long empid, long planner_id, string empname, long userid)
        {
            try
            {
                //var notificationdetails = (from UEM in _IssueContext.ISM_User_Employees_MappingDMO
                //                           from HRE in _IssueContext.MasterEmployee
                //                           from HMD in _IssueContext.HR_Master_Department
                //                           where (UEM.HRME_Id == HRE.HRME_Id && HRE.HRME_ActiveFlag == true && HRE.HRME_LeftFlag == false && HRE.HRME_ActiveFlag == true && HRE.HRMD_Id == HMD.HRMD_Id && HMD.HRMD_ActiveFlag == true && UEM.User_Id == userid)
                //                           select new ISM_DailyReportGenerationDTO
                //                           {
                //                               HRME_Id = UEM.HRME_Id,
                //                           }).Distinct().ToList();
                var notificationuserids = (from UEM in _IssueContext.ISM_User_Employees_MappingDMO
                                           from HRE in _IssueContext.MasterEmployee
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

                var notificationdetails = (from SUL in _IssueContext.Staff_User_Login
                                           where (userids.Contains(SUL.Id))
                                           select new ISM_DailyReportGenerationDTO
                                           {
                                               HRME_Id = SUL.Emp_Code,
                                           }).Distinct().ToList();

                ISM_Task_PlannerDMO planner = new ISM_Task_PlannerDMO();
                var plannerdetails = _IssueContext.ISM_Task_PlannerDMO.Where(a => a.MI_Id == MI_Id && a.HRME_Id == empid
                && a.ISMTPL_Id == planner_id).Distinct().ToList();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ISM_PlannerCreationDTO data = new ISM_PlannerCreationDTO();

                if (notificationdetails.Count > 0)
                {
                    List<long> employeeids = new List<long>();
                    foreach (var n in notificationdetails)
                    {
                        ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                        push.MI_Id = MI_Id;
                        push.HRME_Id = n.HRME_Id;
                        push.ISMNO_Notification = plannerdetails.FirstOrDefault().ISMTPL_PlannerName;
                        push.ISMNO_NoticationDate = indianTime;
                        push.ISMNO_NotificationType = "PL";
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
        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Planner_Report_Generation" && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }

                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _IssueContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

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
                            var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                        ////******************* EMAIL BCC DETAILS ****************//

                        //if (emailbccdetails != null && emailbccdetails != "")
                        //{
                        //    string[] bccmaildetails = emailbccdetails.Split(',');

                        //    foreach (var c in bccmaildetails)
                        //    {
                        //        message.AddBcc(c.ToString());
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


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                                ////******************* EMAIL BCC DETAILS ****************//

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


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Planner_Report_Generation" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _IssueContext.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
        public ISM_PlannerCreationDTO removetask(ISM_PlannerCreationDTO data)
        {
            try
            {
                var result = _IssueContext.ISM_TaskCreation_AssignedToDMO.Single(t => t.ISMTCRASTO_Id == data.ISMTCRASTO_Id);

                if (result.ISMTCRASTO_ActiveFlg == true)
                {
                    result.ISMTCRASTO_ActiveFlg = false;
                }
                else if (result.ISMTCRASTO_ActiveFlg == false)
                {
                    result.ISMTCRASTO_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);
                int returnval = _IssueContext.SaveChanges();
                if (returnval > 0)
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
        public async Task<ISM_PlannerCreationDTO> getplannerdetails(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;

                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PLANNER_CREATION_TASKDEATILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
            SqlDbType.VarChar)
                    {
                        Value = data.ISMTPL_Id
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


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DeviationReport_Calculation_Chart";
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
                    cmd.Parameters.Add(new SqlParameter("@plannerid",
            SqlDbType.VarChar)
                    {
                        Value = data.ISMTPL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.BigInt)
                    {
                        Value = 1
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
                        data.deviationreport = retObject.ToArray();
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
        public ISM_PlannerCreationDTO TriggerPlannerMail(ISM_PlannerCreationDTO data)
        {
            try
            {
                var Employeelist = _IssueContext.MasterEmployee.Where(R => R.HRME_ActiveFlag == true && R.HRME_LeftFlag == false).ToArray();

                if (Employeelist != null && Employeelist.Length > 0)
                {
                    for (var i = 0; i < Employeelist.Length; i++)
                    {

                        string emailid = "";
                        //string email = "";
                         emailid = _IssueContext.Multiple_Email_DMO.Where(t => t.HRME_Id == Employeelist[i].HRME_Id && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                        if (emailid != "")
                        {
                            sendmail(Employeelist[i].MI_Id, emailid, "PlannerReminder", Employeelist[i].HRME_Id);
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
        public string sendmail(long MI_Id, string Email, string Template, long UserID)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }
                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _IssueContext.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _IssueContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _IssueContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPlannerRemainder";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
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
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();



                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _IssueContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;


                        message.AddTo(Email);




                        var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }




                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

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
                                    var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                                        }
                                    }
                                }

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _IssueContext.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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

            }
            return "success";
        }
    }
}
