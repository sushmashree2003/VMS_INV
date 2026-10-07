using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_TaskCreationImpl : Interfaces.ISM_TaskCreationInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_TaskCreationImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
        public async Task<ISM_TaskCreationDTO> getdetails(ISM_TaskCreationDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Page_Privilege_TaskCreation";
                    // cmd.CommandText = "IVRM_Page_Privilege_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@pageurl", SqlDbType.VarChar)
                    {
                        Value = data.PageFlag
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
                        data.pagePrivilege = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                if (data.pagePrivilege != null && data.pagePrivilege.Length > 0)
                {
                    data.TypeFlg = "";
                    var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                    long empid = 0;
                    if (hrmEempid.Length > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                        data.HRME_Id = empid;
                    }
                    var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                    string role = "";
                    if (userrole.Count > 0)
                    {
                        role = userrole.FirstOrDefault().IVRMRT_Role;
                        data.roletype = role;
                    }
                    data.get_priority = _IssueContext.MastersPriorities_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMP_ActiveFlag == true).Distinct().OrderBy(t => t.HRMP_Order).ToArray();
                    data.get_days = _IssueContext.masterday.Where(t => t.MI_Id == data.MI_Id && t.FOMD_ActiveFlag == true).Distinct().OrderBy(t => t.FOMD_Id).ToArray();
                    if (role.Equals("ClientUser", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {

                            data.get_department = (from a in _IssueContext.HR_Master_Department
                                                   where (a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMD_DepartmentName == "Client")
                                                   select new ISM_TaskCreationDTO
                                                   {
                                                       HRMD_Id = a.HRMD_Id,
                                                       HRMDC_ID = a.HRMDC_ID,
                                                       HRMD_DepartmentName = a.HRMD_DepartmentName
                                                   }).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                            data.get_client = (from A in _IssueContext.clientTable
                                               from B in _IssueContext.ISM_Master_Client_UserMappingDMO
                                               where (A.ISMMCLT_Id == B.ISMMCLT_Id && A.ISMMCLT_ActiveFlag == true && B.ISMCIM_IEList == data.UserId)
                                               select new ISM_ComplaintsTaskListDTO
                                               {
                                                   ISMMCLT_Id = A.ISMMCLT_Id,
                                                   ISMMCLT_ClientName = A.ISMMCLT_ClientName
                                               }).Distinct().OrderBy(t => t.ISMMCLT_ClientName).ToArray();

                            data.get_project = (from a in _IssueContext.clientTable
                                                from b in _IssueContext.MastersProject_DMO
                                                from c in _IssueContext.ISM_Master_Client_ProjectDMO
                                                where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.ISMMCLT_ActiveFlag == true && b.ISMMPR_ActiveFlg == true && c.ISMMCLTPR_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                                select new ISM_TaskCreationDTO
                                                {
                                                    ISMMPR_Id = c.ISMMPR_Id,
                                                    ISMMPR_ProjectName = b.ISMMPR_ProjectName
                                                }).Distinct().OrderBy(a => a.ISMMPR_ProjectName).ToArray();

                            //*********************************** Client Task List ******************************************//           
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_Client_Task_List";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@User_Id",
                        SqlDbType.BigInt)
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
                    }
                    else
                    {
                        try
                        {
                            //data.get_department = (from a in _IssueContext.HR_Master_Department
                            //                       from b in _IssueContext.MasterEmployee
                            //                       where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true && b.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id )
                            //                       select new ISM_TaskCreationDTO
                            //                       {
                            //                           HRMD_Id = a.HRMD_Id,
                            //                           HRMD_DepartmentName = a.HRMD_DepartmentName
                            //                       }).Distinct().ToArray();
                            data.get_department = (from a in _IssueContext.HR_Master_Department
                                                   where (a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                                   select new ISM_TaskCreationDTO
                                                   {
                                                       HRMD_Id = a.HRMD_Id,
                                                       HRMDC_ID = a.HRMDC_ID,
                                                       HRMD_DepartmentName = a.HRMD_DepartmentName
                                                   }).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                            //data.get_project = _IssueContext.MastersProject_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMPR_ActiveFlg == true).Distinct().OrderBy(t => t.ISMMPR_Id).ToArray();

                            //data.get_client = (from a in _IssueContext.clientTable
                            //                   from b in _IssueContext.clientMappingTable
                            //                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMCIM_IEList == empid && a.ISMMCLT_ActiveFlag == true && a.MI_Id == data.MI_Id)
                            //                   select new ISM_ComplaintsTaskListDTO
                            //                   {
                            //                       ISMMCLT_Id = b.ISMMCLT_Id,
                            //                       ISMMCLT_ClientName = a.ISMMCLT_ClientName
                            //                   }).Distinct().ToArray();

                            //*********************************** Task List ******************************************//           
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_Task_List";
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
                                    data.get_taskdetails = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            //*********************************** Employee List ********************************************// 
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_EmployeeList_New_Taks";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Userid",
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
                                    data.get_employeelist = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            //***********************************YEAR PLAN TASK LIST ******************************************//           
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_Task_List_Year_Planner";
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
                                    data.yeartaskarray = retObject.ToArray();
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
                    }
                    //************ Planner Creation Extension ******************//
                    var getplannernamecount = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid && t.ISMTPL_ActiveFlg == true && t.ISMTPL_ApprovalFlg == false && t.ISMTPL_ApprovedBy == 0
                    && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(System.DateTime.Today.Date)
                    && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).ToList();
                    if (getplannernamecount.Count == 0)
                    {

                        data.plannerextapproval = false;
                        System.Globalization.CultureInfo ci =
                        System.Threading.Thread.CurrentThread.CurrentCulture;
                        DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
                        DayOfWeek today = DateTime.Now.DayOfWeek;
                        data.plannerMaxdate = DateTime.Now.AddDays(-(today - fdow)).Date;
                        data.plannerMaxdate = data.plannerMaxdate.AddDays(7).Date;


                        //******************************* Calculate Effort *****************************//
                        DateTime datetimennow = DateTime.Now;
                        datetimennow = Convert.ToDateTime(datetimennow.Date.ToString("yyyy-MM-dd"));
                        long lyear_id = 0;
                        var yearid = _IssueContext.HR_Master_LeaveYearDMO.Where(l => l.MI_Id == data.MI_Id && l.HRMLY_ActiveFlag == true && (l.HRMLY_FromDate <= datetimennow && l.HRMLY_ToDate >= datetimennow)).Distinct().ToList();

                        var dep_id = _IssueContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;

                        if (yearid.Count > 0)
                        {
                            lyear_id = yearid.FirstOrDefault().HRMLY_Id;
                        }
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_PLANNER_CREATION_CHECK";
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
                                var datelist = retObject.ToList();


                                if (datelist.Count > 0)
                                {
                                    if (Convert.ToDateTime(System.DateTime.Today.Date) == datelist[0].PDates)
                                    {
                                        data.plannerextapproval = true;

                                        var planner_extension = _IssueContext.ISM_PlannerExtensionDMO.Where(a => a.HRME_Id == empid && a.ISMPLE_ActiveFlg == true).Distinct().ToList();
                                        data.plannerextension = planner_extension.ToArray();
                                        if (planner_extension.Count > 0)
                                        {
                                            var acd_Id = _IssueContext.ISM_PlannerExtensionDMO.Where(t => t.HRME_Id == empid && Convert.ToDateTime(t.ISMPLE_FromDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ISMPLE_ToDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).OrderByDescending(t => t.CreatedDate).ToList();
                                            if (acd_Id.Count > 0)
                                            {
                                                data.plannerextapproval = true;
                                                var extdate = acd_Id.FirstOrDefault().ISMPLE_FromDate;
                                                data.plannerMaxdate = extdate;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var planner_extension = _IssueContext.ISM_PlannerExtensionDMO.Where(a => a.HRME_Id == empid && a.ISMPLE_ActiveFlg == true).Distinct().ToList();
                                        data.plannerextension = planner_extension.ToArray();
                                        if (planner_extension.Count > 0)
                                        {
                                            var acd_Id = _IssueContext.ISM_PlannerExtensionDMO.Where(t => t.HRME_Id == empid && Convert.ToDateTime(t.ISMPLE_FromDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ISMPLE_ToDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).OrderByDescending(t => t.CreatedDate).ToList();
                                            if (acd_Id.Count > 0)
                                            {
                                                data.plannerextapproval = true;
                                                var extdate = acd_Id.FirstOrDefault().ISMPLE_FromDate;
                                                data.plannerMaxdate = extdate;
                                            }
                                        }
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        data.plannerextapproval = false;
                        System.Globalization.CultureInfo ci =
                        System.Threading.Thread.CurrentThread.CurrentCulture;
                        DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
                        DayOfWeek today = DateTime.Now.DayOfWeek;
                        data.plannerMaxdate = DateTime.Now.AddDays(-(today - fdow)).Date;
                        data.plannerMaxdate = data.plannerMaxdate.AddDays(7).Date;
                    }
                    var plnnerdate = _IssueContext.ISM_Task_PlannerDMO.Where(p => p.MI_Id == data.MI_Id && p.ISMTPL_ActiveFlg == true && p.HRME_Id == empid && ((p.ISMTPL_ApprovalFlg == false && p.ISMTPL_ApprovedBy == 0) || (p.ISMTPL_ApprovalFlg == true && p.ISMTPL_ApprovedBy != 0))
                    ).OrderByDescending(e => e.ISMTPL_EndDate).Take(1).ToList();

                    if (plnnerdate.Count > 0)
                    {
                        DateTime a = DateTime.Now;
                        a = Convert.ToDateTime(plnnerdate[0].ISMTPL_EndDate);
                        if (a > data.plannerMaxdate)
                        {
                            data.plannerMaxdate = a;

                            data.plannerMaxdate = data.plannerMaxdate.AddDays(1).Date;
                        }

                    }
                    if (data.plannerMaxdate > DateTime.Now)
                    {
                        data.plannerextapproval = false;
                        data.plannerMaxdate = DateTime.Now;
                    }
                    if (data.HRME_Id > 0)
                    {
                        data.EmplyeeEnhancements = (from a in _IssueContext.MasterEmployee
                                                    where (
                                                    a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                                    select new ISM_TaskCreationDTO
                                                    {
                                                        employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                        HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                        HRME_Id = a.HRME_Id,
                                                        HRMD_Id = a.HRMD_Id.Value

                                                    }).Distinct().ToArray();

                        //data.Deparmetnhead = (from a in _IssueContext.MasterEmployee from b in _IssueContext.HR_Master_DepartmentCodeDMO from c in _IssueContext.HR_Master_DepartmentCode_Head from d in _IssueContext.HR_Master_Department where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && d.HRMDC_ID == c.HRMDC_ID && c.HRMDC_ID == b.HRMDC_ID && c.HRME_ID == a.HRME_Id && d.HRMD_ActiveFlag == true) select new ISM_TaskCreationDTO { employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(), HRME_EmployeeCode = a.HRME_EmployeeCode, HRME_Id = a.HRME_Id, HRMD_Id = a.HRMD_Id.Value }).Distinct().ToArray();
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "HR_Department_Wise_Employee";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                    SqlDbType.BigInt)
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
                                data.Deparmetnhead = retObject.ToArray();
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
                    data.TypeFlg = "PageNotMapped";

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_TaskCreationDTO> getIEuser(ISM_TaskCreationDTO data)
        {
            try
            {
                //*********************************** IE AND User list ******************************************//           
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Client_IEuserList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMMCLT_Id
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
                        data.get_IEuser = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.get_module = (from a in _IssueContext.MasterModules
                                   from b in _IssueContext.MastersModule_DMO
                                   from c in _IssueContext.ISM_Master_Client_ProjectDMO
                                   from d in _IssueContext.MastersProject_DMO
                                   where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMMD_Id == c.ISMMMD_Id && c.ISMMPR_Id == d.ISMMPR_Id
                                   && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMCLTPR_ActiveFlag == true && d.ISMMPR_ActiveFlg == true
                                   && b.MI_Id == data.MI_Id && b.ISMMPR_Id == data.ISMMPR_Id && b.HRMD_Id == data.HRMD_Id && c.ISMMCLT_Id == data.ISMMCLT_Id)
                                   select new ISM_TaskCreationDTO
                                   {
                                       ISMMPR_Id = b.ISMMPR_Id,
                                       IVRMM_Id = b.IVRMM_Id,
                                       IVRMM_ModuleName = a.IVRMM_ModuleName,
                                       ISMMMD_ModuleHeadId = b.ISMMMD_ModuleHeadId
                                   }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_TaskCreationDTO getmodule(ISM_TaskCreationDTO data)
        {
            try
            {
                if (data.roletype.Equals("ClientUser", StringComparison.OrdinalIgnoreCase))
                {
                    data.get_module = (from a in _IssueContext.MasterModules
                                       from b in _IssueContext.MastersModule_DMO
                                       from c in _IssueContext.ISM_Master_Client_ProjectDMO
                                       from d in _IssueContext.MastersProject_DMO
                                       where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMMD_Id == c.ISMMMD_Id && c.ISMMPR_Id == d.ISMMPR_Id
                                       && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMCLTPR_ActiveFlag == true && d.ISMMPR_ActiveFlg == true
                                       && b.MI_Id == data.MI_Id && b.ISMMPR_Id == data.ISMMPR_Id)
                                       select new ISM_TaskCreationDTO
                                       {
                                           ISMMPR_Id = b.ISMMPR_Id,
                                           IVRMM_Id = b.IVRMM_Id,
                                           IVRMM_ModuleName = a.IVRMM_ModuleName,
                                           ISMMMD_ModuleHeadId = b.ISMMMD_ModuleHeadId
                                       }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();

                    data.get_client = (from A in _IssueContext.clientTable
                                       from B in _IssueContext.ISM_Master_Client_UserMappingDMO
                                       where (A.ISMMCLT_Id == B.ISMMCLT_Id && A.ISMMCLT_ActiveFlag == true && B.ISMCIM_IEList == data.UserId)
                                       select new ISM_ComplaintsTaskListDTO
                                       {
                                           ISMMCLT_Id = A.ISMMCLT_Id,
                                           ISMMCLT_ClientName = A.ISMMCLT_ClientName
                                       }).Distinct().OrderBy(t => t.ISMMCLT_ClientName).ToArray();
                }
                else
                {

                    data.get_module = (from a in _IssueContext.MasterModules
                                       from b in _IssueContext.MastersModule_DMO
                                       from c in _IssueContext.ISM_Master_Client_ProjectDMO
                                       from d in _IssueContext.MastersProject_DMO
                                       where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMMD_Id == c.ISMMMD_Id && c.ISMMPR_Id == d.ISMMPR_Id
                                       && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMCLTPR_ActiveFlag == true && d.ISMMPR_ActiveFlg == true
                                       && b.MI_Id == data.MI_Id && b.ISMMPR_Id == data.ISMMPR_Id && b.HRMD_Id == data.HRMD_Id)
                                       select new ISM_TaskCreationDTO
                                       {
                                           ISMMPR_Id = b.ISMMPR_Id,
                                           IVRMM_Id = b.IVRMM_Id,
                                           IVRMM_ModuleName = a.IVRMM_ModuleName,
                                           ISMMMD_ModuleHeadId = b.ISMMMD_ModuleHeadId
                                       }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();

                    //data.get_client = (from A in _IssueContext.clientTable
                    //                   from B in _IssueContext.ISM_Master_Client_ProjectDMO
                    //                   from C in _IssueContext.MastersModule_DMO
                    //                   where (B.ISMMPR_Id == data.ISMMPR_Id && A.ISMMCLT_ActiveFlag == true && A.MI_Id == data.MI_Id && B.ISMMPR_Id == C.ISMMPR_Id && C.HRMD_Id == data.HRMD_Id && B.ISMMCLTPR_ActiveFlag == true && C.MI_Id == data.MI_Id)
                    //                   select new ISM_ComplaintsTaskListDTO
                    //                   {
                    //                       ISMMCLT_Id = A.ISMMCLT_Id,
                    //                       ISMMCLT_ClientName = A.ISMMCLT_ClientName
                    //                   }).Distinct().OrderBy(t => t.ISMMCLT_ClientName).ToArray();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Client_List";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt)
                        {
                            Value = data.HRMD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMMPR_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMPR_Id
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
                            data.get_client = retObject.ToArray();
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
        public ISM_TaskCreationDTO savedata(ISM_TaskCreationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                PushNotification pb = new PushNotification(_Context);
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    data.HRME_Id = empid;
                }

                var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                string role = "";
                if (userrole.Count > 0)
                {
                    role = userrole.FirstOrDefault().IVRMRT_Role;
                    data.roletype = role;

                }
                long client_id = 0;
                if (role == "ClientUser")
                {
                    client_id = 0;
                }
                else
                {
                    client_id = data.ISMMCLT_Id;
                }

                if (data.ISMTCR_Id != 0)
                {
                    var res = _IssueContext.ISM_TaskCreationDMO.Where(t => t.HRMD_Id == data.HRMD_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.IVRMM_Id == data.IVRMM_Id && t.HRMPR_Id == data.HRMPR_Id && t.MI_Id == data.MI_Id && t.ISMTCR_Id != data.ISMTCR_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_Id == data.ISMTCR_Id);
                        result.MI_Id = data.MI_Id;
                        result.HRMD_Id = data.HRMD_Id;
                        result.ISMMPR_Id = data.ISMMPR_Id;
                        result.ISMMTCAT_Id = data.ISMMTCAT_Id;
                        result.ISMMCLT_Id = client_id;
                        result.IVRMM_Id = data.IVRMM_Id;
                        result.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                        result.ISMTCR_CreationDate = data.ISMTCR_CreationDate;
                        result.ISMTCR_Title = data.ISMTCR_Title;
                        result.HRMPR_Id = data.HRMPR_Id;
                        result.ISMTCR_Desc = data.ISMTCR_Desc;
                        result.ISMTCR_Status = data.ISMTCR_Status;
                        result.HRME_Id = empid;
                        result.ISMTCR_ReOpenFlg = false;
                        result.ISMTCR_UpdatedBy = data.UserId;
                        result.ISMTCR_ActiveFlg = true;
                        result.UpdatedDate = indianTime;
                        _IssueContext.Update(result);

                        if (data.ISMMCLT_Id > 0)
                        {
                            var res0 = _IssueContext.ISM_TaskCreation_ClientDMO.Where(a => a.ISMTCRCL_Id == data.ISMTCRCL_Id).ToList();
                            if (res0.Count > 0)
                            {
                                var res01 = _IssueContext.ISM_TaskCreation_ClientDMO.Single(a => a.ISMTCRCL_Id == data.ISMTCRCL_Id);
                                res01.ISMTCR_Id = result.ISMTCR_Id;
                                res01.ISMMCLT_Id = data.ISMMCLT_Id;
                                res01.ISMTCRCL_ActiveFlg = true;
                                res01.ISMTCRCL_CreatedBy = data.UserId;
                                res01.ISMTCRCL_UpdatedBy = data.UserId;
                                res01.UpdatedDate = indianTime;
                                _IssueContext.Update(res01);
                            }
                            else
                            {
                                ISM_TaskCreation_ClientDMO uptaskclt = new ISM_TaskCreation_ClientDMO();
                                uptaskclt.ISMTCR_Id = result.ISMTCR_Id;
                                uptaskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                                uptaskclt.ISMTCRCL_ActiveFlg = true;
                                uptaskclt.ISMTCRCL_CreatedBy = data.UserId;
                                uptaskclt.ISMTCRCL_UpdatedBy = data.UserId;
                                uptaskclt.CreatedDate = indianTime;
                                uptaskclt.UpdatedDate = indianTime;
                                _IssueContext.Add(uptaskclt);
                            }
                        }
                        if (data.attachmentArray != null)
                        {
                            foreach (var file in data.attachmentArray)
                            {
                                if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                                {
                                    var res1 = _IssueContext.ISM_TaskCreation_AttachmentDMO.Where(a => a.ISMTCRAT_Id == data.ISMTCRAT_Id).ToList();
                                    if (res1.Count > 0)
                                    {
                                        var res11 = _IssueContext.ISM_TaskCreation_AttachmentDMO.Single(a => a.ISMTCRAT_Id == data.ISMTCRAT_Id);
                                        res11.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                        res11.ISMTCRAT_File = file.ISMTCRAT_File;
                                        res11.ISMTCRAT_UpdatedBy = data.UserId;
                                        res11.UpdatedDate = indianTime;
                                        _IssueContext.Update(res11);
                                    }
                                    else
                                    {
                                        ISM_TaskCreation_AttachmentDMO att = new ISM_TaskCreation_AttachmentDMO();
                                        att.ISMTCR_Id = result.ISMTCR_Id;
                                        att.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                        att.ISMTCRAT_File = file.ISMTCRAT_File;
                                        att.ISMTCRAT_ActiveFlg = true;
                                        att.ISMTCRAT_CreatedBy = data.UserId;
                                        att.ISMTCRAT_UpdatedBy = data.UserId;
                                        att.CreatedDate = indianTime;
                                        att.UpdatedDate = indianTime;
                                        _IssueContext.Add(att);
                                    }
                                }

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
                }

                else
                {
                    string Auto_taskNumber = AutoGenerateTaskNumber(data);
                    data.Auto_id = Auto_taskNumber;

                    if (role.Equals("ClientUser", StringComparison.OrdinalIgnoreCase) || (data.assignto == "N" && data.ISMTCR_BugOREnhancementFlg != "T"))
                    {
                        SaveTaskDetails(data);

                        var contactExists = _IssueContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            // ============================================== Push Notification Client task creation     
                            if (role.Equals("ClientUser", StringComparison.OrdinalIgnoreCase))
                            {
                                var deviceidsnew = "";
                                var devicenew = "";
                                var devicelist = (from a in _IssueContext.ISM_TaskCreationDMO
                                                  from b in _IssueContext.ISM_TaskCreation_ClientDMO
                                                  from c in _IssueContext.clientTable
                                                  from d in _IssueContext.clientMappingTable
                                                  from e in _IssueContext.MasterEmployee
                                                  where (a.ISMTCR_Id == b.ISMTCR_Id && b.ISMMCLT_Id == c.ISMMCLT_Id && c.ISMMCLT_Id == d.ISMMCLT_Id
                                                  && d.ISMCIM_IEList == e.HRME_Id && a.ISMTCR_ActiveFlg == true && c.ISMMCLT_ActiveFlag == true
                                                  && e.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && b.ISMMCLT_Id == data.ISMMCLT_Id)
                                                  select new ISM_ComplaintsTaskListDTO
                                                  {
                                                      HRME_Id = d.ISMCIM_IEList,
                                                      HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                  }).Distinct().OrderBy(t => t.HRME_Id).ToList();

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
                                var taskiddd = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_TaskNo == data.Auto_id);
                                try
                                {
                                    string subject = "Client: New Task Created";
                                    string titletext = taskiddd.ISMTCR_TaskNo + " - " + taskiddd.ISMTCR_Title;
                                    if (data.sound == "")
                                    {
                                        data.sound = "default";
                                    }
                                    callnotification(devicenew, empid, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                    Insertnotification(data.MI_Id, empid, data.ISMTCR_Id, titletext, data.UserId, "Client");

                                    var depheaderlist = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                                                         from b in _IssueContext.HR_Master_Department
                                                         from c in _IssueContext.MasterEmployee
                                                         from d in _IssueContext.HR_Master_DepartmentCodeDMO
                                                         where a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == c.HRME_Id
                                                         && b.HRMD_Id == c.HRMD_Id && a.HRMDC_ID == d.HRMDC_ID && b.HRMDC_ID == d.HRMDC_ID && (d.HRMDC_Code == "SW" || d.HRMDC_Code == "SQT" || d.HRMDC_Code == "STI")
                                                         select new ISM_ComplaintsTaskListDTO
                                                         {
                                                             HRME_Id = c.HRME_Id,
                                                             HRME_AppDownloadedDeviceId = c.HRME_AppDownloadedDeviceId
                                                         }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                    if (depheaderlist.Count > 0)
                                    {



                                        foreach (var item in depheaderlist)
                                        {

                                            if (item.HRME_Id != empid)
                                            {
                                                string dd = pb.savewebnotification(data.MI_Id, "NOTASSIGNTASKWEB", taskiddd.ISMTCR_Id, taskiddd.ISMTCR_Id, item.HRME_Id, data.UserId, "NotAssigned");

                                                var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                                                     from a in _IssueContext.Staff_User_Login
                                                                     where (e.ISMPNU_Userid == a.Id && a.Emp_Code == item.HRME_Id)
                                                                     select new ISM_ComplaintsTaskListDTO
                                                                     {
                                                                         UserId = e.ISMPNU_Userid
                                                                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                                if (webpushtocken.Count > 0)
                                                {
                                                    foreach (var webtockenid in webpushtocken)
                                                    {
                                                        PushNotification ObjPushNotification = new PushNotification(_Context);
                                                        ObjPushNotification.callpushnotification(data.ISMTCR_Title, "Client-New Task Creation!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskiddd.ISMTCR_Id, webtockenid.UserId);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            else
                            {
                                var depheaderlist = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                                                     from b in _IssueContext.HR_Master_Department
                                                     from c in _IssueContext.MasterEmployee
                                                     where a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == c.HRME_Id && b.HRMD_Id == data.HRMD_Id
                                                     && b.HRMD_Id == c.HRMD_Id
                                                     select new ISM_ComplaintsTaskListDTO
                                                     {
                                                         HRME_Id = c.HRME_Id,
                                                         HRME_AppDownloadedDeviceId = c.HRME_AppDownloadedDeviceId
                                                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                var taskiddd = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_TaskNo == data.Auto_id);

                                if (depheaderlist.Count > 0)
                                {


                                    foreach (var item in depheaderlist)
                                    {

                                        if (item.HRME_Id != empid)
                                        {
                                            string dd = pb.savewebnotification(data.MI_Id, "NOTASSIGNTASKWEB", taskiddd.ISMTCR_Id, taskiddd.ISMTCR_Id, item.HRME_Id, data.UserId, "NotAssigned");

                                            var createemp = (from a in _IssueContext.MasterEmployee
                                                             from b in _IssueContext.HR_Master_Department
                                                             where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == item.HRME_Id && a.HRME_ActiveFlag == true)
                                                             select a.HRMD_Id);

                                            var assignemp = (from a in _IssueContext.MasterEmployee
                                                             from b in _IssueContext.HR_Master_Department
                                                             where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == empid && a.HRME_ActiveFlag == true)
                                                             select a.HRMD_Id);

                                            if (createemp != assignemp)
                                            {
                                                var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                                                     from a in _IssueContext.Staff_User_Login
                                                                     where (e.ISMPNU_Userid == a.Id && a.Emp_Code == item.HRME_Id)
                                                                     select new ISM_ComplaintsTaskListDTO
                                                                     {
                                                                         UserId = e.ISMPNU_Userid
                                                                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                                if (webpushtocken.Count > 0)
                                                {
                                                    foreach (var webtockenid in webpushtocken)
                                                    {
                                                        PushNotification ObjPushNotification = new PushNotification(_Context);
                                                        ObjPushNotification.callpushnotification(data.ISMTCR_Title, "New Task Created!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskiddd.ISMTCR_Id, webtockenid.UserId);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (depheaderlist.Count > 0)
                                {
                                    string deviceidsnew = "";
                                    string devicenew = "";
                                    foreach (var deviceid in depheaderlist)
                                    {
                                        if (deviceid.HRME_AppDownloadedDeviceId != null && deviceid.HRME_AppDownloadedDeviceId != "")
                                        {


                                            deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                            devicenew = "[" + deviceidsnew + "]";
                                            try
                                            {
                                                string subject = "New Task Created!";
                                                string titletext = taskiddd.ISMTCR_TaskNo + " - " + taskiddd.ISMTCR_Title;
                                                if (data.sound == "")
                                                {
                                                    data.sound = "default";
                                                }
                                                callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Create");
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                    else if (data.assignto == "Y" || data.ISMTCR_BugOREnhancementFlg == "T")
                    {
                        if (data.taskEmpArray.Length > 0)
                        {
                            foreach (var te in data.taskEmpArray)
                            {
                                SaveTaskDetails(data);

                                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.startdate), INDIAN_ZONE);
                                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.enddate), INDIAN_ZONE);

                                string confromdate = "";
                                string contodate = "";
                                try
                                {
                                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                    contodate = todatecon.ToString("yyyy-MM-dd");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                var empHRMEid = _Context.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
                                long employeeid = 0;
                                var depart_id = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empHRMEid[0].Emp_Code).ToList();

                                var departmenthead = (from a in _IssueContext.HR_Master_Department
                                                      from b in _IssueContext.HR_Master_DepartmentCode_Head
                                                      from c in _IssueContext.Staff_User_Login
                                                      where (a.HRMD_Id == depart_id[0].HRMD_Id && a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.Emp_Code && a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                                      select new ISM_TaskCreationDTO
                                                      {
                                                          HRME_Id = b.HRME_ID,
                                                          UserId = c.Id
                                                      }).ToList();


                                ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                                taskasign.ISMTCR_Id = data.ISMTCR_Id;
                                taskasign.HRME_Id = te.HRME_Id;
                                taskasign.ISMTCRASTO_AssignedDate = indianTime;
                                taskasign.ISMTCRASTO_AssignedBy = employeeid = empid;
                                taskasign.ISMTCRASTO_Remarks = data.remarks;
                                taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(confromdate);
                                taskasign.ISMTCRASTO_EndDate = Convert.ToDateTime(contodate);
                                taskasign.ISMTCRASTO_EffortInHrs = data.effortinhrs;
                                taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                                taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                                taskasign.ISMTCRASTO_TimeRequiredFlg = data.TimeRequiredFlg;
                                taskasign.ISMTCRASTO_ActiveFlg = true;
                                taskasign.CreatedDate = indianTime;
                                taskasign.UpdatedDate = indianTime;
                                _IssueContext.Add(taskasign);

                                if (data.ISMTCR_BugOREnhancementFlg == "T")
                                {
                                    ISM_Task_Advance_PlannerDMO adv = new ISM_Task_Advance_PlannerDMO();
                                    adv.ISMTCR_Id = data.ISMTCR_Id;
                                    adv.ISMTAPL_Remarks = data.remarks;
                                    adv.MI_Id = data.MI_Id;
                                    adv.HRME_Id = te.HRME_Id;
                                    adv.ISMTAPL_Year = data.ASMAY_Id;
                                    adv.ISMTAPL_TimeRequired = data.effortinhrs;
                                    adv.ISMTAPL_FromDate = Convert.ToDateTime(confromdate);
                                    adv.ISMTAPL_ToDate = Convert.ToDateTime(contodate);
                                    adv.ISMTAPL_Periodicity = data.periodicity.Trim();
                                    adv.ISMTAPL_TimeRequiredFlg = data.TimeRequiredFlg.Trim();
                                    adv.ISMTAPL_CreatedBy = data.UserId;
                                    adv.ISMTAPL_UpdatedBy = data.UserId;
                                    adv.ISMTAPL_ActiveFlg = true;
                                    adv.CreatedDate = DateTime.Now;
                                    adv.UpdatedDate = DateTime.Now;
                                    adv.ISMTAPL_OFFDate = data.Yearlydate;
                                    adv.ISMTAPL_ApprovalFlag = 1;
                                    adv.ISMTAPL_Day = Convert.ToInt64(data.TaskDay);
                                    _IssueContext.Add(adv);

                                    ISM_Task_Advance_Planner_DatesDMO datedmo = new ISM_Task_Advance_Planner_DatesDMO();
                                    datedmo.ISMTAPL_Id = adv.ISMTAPL_Id;
                                    datedmo.ISMTAPLD_StartDate = Convert.ToDateTime(confromdate);
                                    datedmo.ISMTAPLD_EndDate = Convert.ToDateTime(contodate);
                                    datedmo.ISMTAPLD_CreatedBy = data.UserId;
                                    datedmo.ISMTAPLD_UpdatedBy = data.UserId;
                                    datedmo.ISMTAPLD_ActiveFlg = true;
                                    datedmo.ISMTAPLD_CreatedDate = DateTime.Now;
                                    datedmo.ISMTAPLD_UpdatedDate = DateTime.Now;
                                    _IssueContext.Add(datedmo);
                                }

                                var i = _IssueContext.SaveChanges();

                                if (i > 0)
                                {
                                    data.returnval = true;

                                    var taskiddd = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_TaskNo == data.Auto_id).OrderByDescending(t => t.ISMTCR_Id).ToList();
                                    var assignedTolist = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                          where b.ISMTCR_Id == taskiddd.FirstOrDefault().ISMTCR_Id
                                                          select new ISM_ComplaintsTaskListDTO
                                                          {
                                                              HRME_Id = b.HRME_Id
                                                          }
                                                        ).Distinct().ToList(); ;

                                    if (assignedTolist.Count > 0)
                                    {
                                        foreach (var item in assignedTolist)
                                        {
                                            string dd = pb.savewebnotification(data.MI_Id, "TASKASSIGNEDWEB", item.ISMTCRASTO_Id, taskiddd.FirstOrDefault().ISMTCR_Id, item.HRME_Id, data.UserId, "Assigned");
                                        }
                                    }




                                    var devicelistto = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                        from e in _IssueContext.MasterEmployee
                                                        where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.HRME_Id == e.HRME_Id && e.HRME_AppDownloadedDeviceId != null
                                                        && e.HRME_AppDownloadedDeviceId != "")
                                                        select new ISM_ComplaintsTaskListDTO
                                                        {
                                                            HRME_Id = b.HRME_Id,
                                                            HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                        }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                    var devicelistfrom = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                          from e in _IssueContext.MasterEmployee
                                                          where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.ISMTCRASTO_AssignedBy == e.HRME_Id
                                                          && e.HRME_AppDownloadedDeviceId != null && e.HRME_AppDownloadedDeviceId != ""
                                                        )
                                                          select new ISM_ComplaintsTaskListDTO
                                                          {
                                                              HRME_Id = b.HRME_Id,
                                                              HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                          }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                    if (devicelistto.Count > 0 && devicelistfrom.Count > 0)
                                    {
                                        var tasklist = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_TaskNo == data.Auto_id).OrderByDescending(t => t.ISMTCR_Id).ToList();
                                        if (devicelistto.FirstOrDefault().HRME_Id != devicelistfrom.FirstOrDefault().HRME_Id)
                                        {
                                            var deviceidsnew = "";
                                            var devicenew = "";
                                            if (devicelistto.Count > 0)
                                            {
                                                foreach (var deviceid in devicelistto)
                                                {
                                                    deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';

                                                    devicenew = "[" + deviceidsnew + "]";

                                                    try
                                                    {
                                                        string titletext = tasklist.FirstOrDefault().ISMTCR_TaskNo + " - " + tasklist.FirstOrDefault().ISMTCR_Title;
                                                        string subject = "New Task Assigned";
                                                        if (data.sound == "")
                                                        {
                                                            data.sound = "default";
                                                        }
                                                        callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                        Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Assigned");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }

                                            if (devicelistfrom.Count > 0)
                                            {
                                                foreach (var deviceid in devicelistfrom)
                                                {
                                                    deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                                    devicenew = "[" + deviceidsnew + "]";
                                                    try
                                                    {
                                                        string titletext = tasklist.FirstOrDefault().ISMTCR_TaskNo + " - " + tasklist.FirstOrDefault().ISMTCR_Title;
                                                        string subject = "New Task Created";
                                                        if (data.sound == "")
                                                        {
                                                            data.sound = "default";
                                                        }
                                                        callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                        Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Assigned");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (te.HRME_Id != empid)
                                    {
                                        //var webpushtocken = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                        //                     from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                        //                     from a in _IssueContext.Staff_User_Login
                                        //                     where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.HRME_Id == a.Emp_Code && b.HRME_Id == te.HRME_Id && a.Emp_Code== te.HRME_Id)
                                        //                     select new ISM_ComplaintsTaskListDTO
                                        //                     {
                                        //                         UserId = e.ISMPNU_Userid
                                        //                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                        List<ISM_TaskCreationDTO> webpushtocken = new List<ISM_TaskCreationDTO>();
                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "ISM_GET_PUSHNOTIFICATION_USERS";
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add(new SqlParameter("@hrmeid",
                                          SqlDbType.BigInt)
                                            {
                                                Value = te.HRME_Id
                                            });
                                            cmd.Parameters.Add(new SqlParameter("@taskid",
                                            SqlDbType.BigInt)
                                            {
                                                Value = taskasign.ISMTCR_Id
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
                                                        webpushtocken.Add(new ISM_TaskCreationDTO
                                                        {
                                                            UserId = Convert.ToInt64(dataReader["UserId"])

                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        if (webpushtocken.Count > 0)
                                        {
                                            foreach (var webtockenid in webpushtocken)
                                            {
                                                PushNotification ObjPushNotification = new PushNotification(_Context);
                                                ObjPushNotification.callpushnotification(data.ISMTCR_Title, "New Task Assigned!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskasign.ISMTCR_Id, webtockenid.UserId);
                                            }
                                        }
                                    }
                                }
                            }
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
        public string callnotification(string devicenew, long empid, long task_id, string titletext, long mi_id, string sound, string subject)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + indianTime + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
          "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + subject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + titletext + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

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
        public string Insertnotification(long MI_Id, long empid, long task_id, string titletext, long userid, string type)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ISM_PlannerCreationDTO data = new ISM_PlannerCreationDTO();
                ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                push.MI_Id = MI_Id;
                push.HRME_Id = empid;
                push.ISMNO_Notification = titletext;
                push.ISMNO_NoticationDate = indianTime;
                push.ISMNO_NotificationType = type;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public ISM_TaskCreationDTO deactive(ISM_TaskCreationDTO data)
        {
            try
            {
                var result = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_Id == data.ISMTCR_Id);

                if (result.ISMTCR_ActiveFlg == true)
                {
                    result.ISMTCR_ActiveFlg = false;
                }
                else if (result.ISMTCR_ActiveFlg == false)
                {
                    result.ISMTCR_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);
                var resultatt = _IssueContext.ISM_TaskCreation_AttachmentDMO.Where(a => a.ISMTCR_Id == data.ISMTCR_Id).Distinct().ToList();
                foreach (var at in resultatt)
                {
                    var resultA = _IssueContext.ISM_TaskCreation_AttachmentDMO.Single(t => t.ISMTCRAT_Id == at.ISMTCRAT_Id);

                    if (resultA.ISMTCRAT_ActiveFlg == true)
                    {
                        resultA.ISMTCRAT_ActiveFlg = false;
                    }
                    else if (resultA.ISMTCRAT_ActiveFlg == false)
                    {
                        resultA.ISMTCRAT_ActiveFlg = true;
                    }
                    resultA.UpdatedDate = DateTime.Now;
                    _IssueContext.Update(resultA);
                }

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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_TaskCreationDTO gettaskAttachment(ISM_TaskCreationDTO data)
        {
            try
            {
                data.get_Attdetails = (from a in _IssueContext.ISM_TaskCreationDMO
                                       from b in _IssueContext.ISM_TaskCreation_AttachmentDMO
                                       where (a.ISMTCR_Id == b.ISMTCR_Id && a.MI_Id == data.MI_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCR_Id == data.ISMTCR_Id)
                                       select new ISM_TaskCreationDTO
                                       {
                                           ISMTCRAT_Attatchment = b.ISMTCRAT_Attatchment
                                       }).Distinct().ToArray();

                data.get_taskproject = (from a in _IssueContext.ISM_TaskCreationDMO
                                        from b in _IssueContext.MastersProject_DMO
                                        where (a.ISMMPR_Id == b.ISMMPR_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ISMTCR_Id == data.ISMTCR_Id)
                                        select new ISM_TaskCreationDTO
                                        {
                                            ISMMPR_Id = a.ISMMPR_Id,
                                            ISMMPR_ProjectName = b.ISMMPR_ProjectName
                                        }).Distinct().OrderBy(a => a.ISMMPR_ProjectName).ToArray();

                data.get_taskmodule = (from a in _IssueContext.ISM_TaskCreationDMO
                                       from b in _IssueContext.MasterModules
                                       where (a.IVRMM_Id == b.IVRMM_Id && a.MI_Id == data.MI_Id && a.ISMTCR_Id == data.ISMTCR_Id)
                                       select new ISM_TaskCreationDTO
                                       {
                                           IVRMM_Id = a.IVRMM_Id,
                                           IVRMM_ModuleName = b.IVRMM_ModuleName
                                       }).Distinct().ToArray();

                data.get_taskclient = (from a in _IssueContext.ISM_TaskCreationDMO
                                       from b in _IssueContext.ISM_TaskCreation_ClientDMO
                                       from c in _IssueContext.clientTable
                                       where (a.ISMTCR_Id == b.ISMTCR_Id && b.ISMMCLT_Id == c.ISMMCLT_Id && a.ISMTCR_ActiveFlg == true && a.MI_Id == data.MI_Id && a.ISMTCR_Id == data.ISMTCR_Id)
                                       select new ISM_ComplaintsTaskListDTO
                                       {
                                           ISMTCR_Id = a.ISMTCR_Id,
                                           ISMMCLT_Id = b.ISMMCLT_Id,
                                           ISMMCLT_ClientName = c.ISMMCLT_ClientName
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_TaskCreationDTO get_category(ISM_TaskCreationDTO dto)
        {
            try
            {
                dto.get_category = _IssueContext.ISM_Master_TaskCategory_DMO_con.Where(a => a.HRMD_Id == dto.HRMD_Id && a.MI_Id == dto.MI_Id && a.ISMMTCAT_ActiveFlag == true).OrderBy(t => t.ISMMTCAT_TaskCategoryName).ToArray();

                dto.get_project = (from a in _IssueContext.clientTable
                                   from b in _IssueContext.MastersProject_DMO
                                   from c in _IssueContext.MastersModule_DMO
                                   where (a.MI_Id == dto.MI_Id && b.MI_Id == dto.MI_Id && a.MI_Id == b.MI_Id && a.ISMMCLT_ActiveFlag == true && b.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == c.ISMMPR_Id && c.HRMD_Id == dto.HRMD_Id)
                                   select new ISM_ClientProject_MappingDTO
                                   {
                                       ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                       ISMMPR_Id = b.ISMMPR_Id
                                   }).Distinct().OrderBy(t => t.ISMMPR_ProjectName).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public string AutoGenerateTaskNumber(ISM_TaskCreationDTO data)
        {
            string Auto_id = "";
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _Context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "ISMTASK").ToList();
                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Auto_id;
        }
        public ISM_TaskCreationDTO SaveTaskDetails(ISM_TaskCreationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var empHRMEid = _Context.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
                long empId = 0;
                if (empHRMEid.Count > 0)
                {
                    var depart_id = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empHRMEid[0].Emp_Code).ToList();
                    var departmenthead = (from a in _IssueContext.HR_Master_Department
                                          from b in _IssueContext.HR_Master_DepartmentCode_Head
                                          from c in _IssueContext.Staff_User_Login
                                          where (a.HRMD_Id == depart_id[0].HRMD_Id && a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.Emp_Code && a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                          select new ISM_TaskCreationDTO
                                          {
                                              HRME_Id = b.HRME_ID,
                                              UserId = c.Id
                                          }).ToList();
                    ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                    task.ISMTCR_TaskNo = data.Auto_id;
                    task.MI_Id = data.MI_Id;
                    task.ISMMTGRP_Id = 0;
                    task.ISMTCR_TGOrder = 0;
                    task.HRMD_Id = data.HRMD_Id;
                    task.ISMMPR_Id = data.ISMMPR_Id;
                    task.IVRMM_Id = data.IVRMM_Id;
                    task.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                    task.ISMTCR_CreationDate = indianTime;
                    task.ISMTCR_Title = data.ISMTCR_Title;
                    task.HRMPR_Id = data.HRMPR_Id;
                    task.HRME_Id = empId = data.HRME_Id;
                    task.ISMTCR_Desc = data.ISMTCR_Desc;
                    task.ISMTCR_Status = data.ISMTCR_Status;
                    task.ISMMTCAT_Id = data.ISMMTCAT_Id;
                    task.ISMMCLT_Id = data.ISMMCLT_Id;
                    task.ISMTCR_Days = data.ISMTCR_Days;
                    task.ISMTCR_Hours = data.ISMTCR_Hours;
                    task.ISMTCR_ReOpenFlg = false;
                    task.ISMTCR_ActiveFlg = true;
                    task.ISMTCR_CreatedBy = data.UserId;
                    task.ISMTCR_UpdatedBy = data.UserId;
                    task.CreatedDate = indianTime;
                    task.UpdatedDate = indianTime;
                    task.ISMTCR_MainGroupTaskFlg = data.ISMTCR_MainGroupTaskFlg;
                    _IssueContext.Add(task);

                    data.ISMTCR_Id = task.ISMTCR_Id;

                    if (data.ISMMCLT_Id > 0)
                    {
                        ISM_TaskCreation_ClientDMO taskclt = new ISM_TaskCreation_ClientDMO();
                        taskclt.ISMTCR_Id = data.ISMTCR_Id;
                        taskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                        taskclt.ISMTCRCL_ActiveFlg = true;
                        taskclt.ISMTCRCL_CreatedBy = data.UserId;
                        taskclt.ISMTCRCL_UpdatedBy = data.UserId;
                        taskclt.CreatedDate = indianTime;
                        taskclt.UpdatedDate = indianTime;
                        _IssueContext.Add(taskclt);
                    }

                    if (data.attachmentArray != null)
                    {
                        foreach (var file in data.attachmentArray)
                        {
                            if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                            {
                                ISM_TaskCreation_AttachmentDMO taskatt = new ISM_TaskCreation_AttachmentDMO();
                                taskatt.ISMTCR_Id = data.ISMTCR_Id;
                                taskatt.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                taskatt.ISMTCRAT_File = file.ISMTCRAT_File;
                                taskatt.ISMTCRAT_ActiveFlg = true;
                                taskatt.ISMTCRAT_CreatedBy = data.UserId;
                                taskatt.ISMTCRAT_UpdatedBy = data.UserId;
                                taskatt.CreatedDate = indianTime;
                                taskatt.UpdatedDate = indianTime;
                                _IssueContext.Add(taskatt);
                            }
                        }
                    }

                    if (data.MulttaskDepartments != null && data.MulttaskDepartments.Length > 0)
                    {
                        foreach (var d in data.MulttaskDepartments)
                        {
                            ISM_TaskEnhancement_DeptwiseDMO dep = new ISM_TaskEnhancement_DeptwiseDMO();
                            dep.MI_ID = data.MI_Id;
                            dep.HRMD_Id = d.TTEDDP_HRMDCID;
                            dep.TTEDDP_Percentage = d.TTEDDP_Percentage;
                            dep.ISMTCR_Id = data.ISMTCR_Id;
                            dep.ISMTCR_TaskNo = data.Auto_id;
                            dep.TTEDDP_Remarks = d.TTEDDP_Remarks;
                            dep.TTEDDP_CreatedBy = data.UserId;
                            dep.TTEDDP_UpdatedBy = data.UserId;
                            dep.CreatedDate = indianTime;
                            dep.UpdatedDate = indianTime;
                            dep.TTEDDP_Days = d.TTEDDP_Days;
                            dep.TTEDDP_level = d.TTEDDP_level;
                            dep.HRME_Id = d.HRME_Id;
                            dep.TTEDDP_ToDate = d.TTEDDP_ToDate;
                            dep.TTEDDP_FromDate = d.TTEDDP_FromDate;
                            dep.TTEDDP_Hours = d.TTEDDP_Hours;
                            _IssueContext.Add(dep);
                        }
                    }
                }
                else
                {
                    ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                    task.ISMTCR_TaskNo = data.Auto_id;
                    task.MI_Id = data.MI_Id;
                    task.ISMMTGRP_Id = 0;
                    task.ISMTCR_TGOrder = 0;
                    task.HRMD_Id = data.HRMD_Id;
                    task.ISMMPR_Id = data.ISMMPR_Id;
                    task.IVRMM_Id = data.IVRMM_Id;
                    task.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                    task.ISMTCR_CreationDate = indianTime;
                    task.ISMTCR_Title = data.ISMTCR_Title;
                    task.HRMPR_Id = data.HRMPR_Id;
                    task.HRME_Id = empId;
                    task.ISMTCR_Desc = data.ISMTCR_Desc;
                    task.ISMTCR_Status = data.ISMTCR_Status;
                    task.ISMMTCAT_Id = data.ISMMTCAT_Id;
                    task.ISMMCLT_Id = data.ISMMCLT_Id;
                    task.ISMTCR_ReOpenFlg = false;
                    task.ISMTCR_ActiveFlg = true;
                    task.ISMTCR_CreatedBy = data.UserId;
                    task.ISMTCR_UpdatedBy = data.UserId;
                    task.CreatedDate = indianTime;
                    task.UpdatedDate = indianTime;
                    task.ISMTCR_Days = data.ISMTCR_Days;
                    task.ISMTCR_Hours = data.ISMTCR_Hours;
                    task.ISMTCR_MainGroupTaskFlg = data.ISMTCR_MainGroupTaskFlg;
                    _IssueContext.Add(task);

                    data.ISMTCR_Id = task.ISMTCR_Id;

                    if (data.ISMMCLT_Id > 0)
                    {
                        ISM_TaskCreation_ClientDMO taskclt = new ISM_TaskCreation_ClientDMO();
                        taskclt.ISMTCR_Id = data.ISMTCR_Id;
                        taskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                        taskclt.ISMTCRCL_ActiveFlg = true;
                        taskclt.ISMTCRCL_CreatedBy = data.UserId;
                        taskclt.ISMTCRCL_UpdatedBy = data.UserId;
                        taskclt.CreatedDate = indianTime;
                        taskclt.UpdatedDate = indianTime;
                        _IssueContext.Add(taskclt);
                    }

                    if (data.attachmentArray != null)
                    {
                        foreach (var file in data.attachmentArray)
                        {
                            if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                            {
                                ISM_TaskCreation_AttachmentDMO taskatt = new ISM_TaskCreation_AttachmentDMO();
                                taskatt.ISMTCR_Id = data.ISMTCR_Id;
                                taskatt.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                taskatt.ISMTCRAT_File = file.ISMTCRAT_File;
                                taskatt.ISMTCRAT_ActiveFlg = true;
                                taskatt.ISMTCRAT_CreatedBy = data.UserId;
                                taskatt.ISMTCRAT_UpdatedBy = data.UserId;
                                taskatt.CreatedDate = indianTime;
                                taskatt.UpdatedDate = indianTime;
                                _IssueContext.Add(taskatt);
                            }
                        }
                    }
                    if (data.MulttaskDepartments != null && data.MulttaskDepartments.Length > 0)
                    {
                        foreach (var d in data.MulttaskDepartments)
                        {
                            ISM_TaskEnhancement_DeptwiseDMO dep = new ISM_TaskEnhancement_DeptwiseDMO();
                            dep.MI_ID = data.MI_Id;
                            dep.HRMD_Id = d.TTEDDP_HRMDCID;
                            dep.TTEDDP_Percentage = d.TTEDDP_Percentage;
                            dep.ISMTCR_Id = data.ISMTCR_Id;
                            dep.ISMTCR_TaskNo = data.Auto_id;
                            dep.TTEDDP_Remarks = "";
                            dep.TTEDDP_CreatedBy = data.UserId;
                            dep.TTEDDP_UpdatedBy = data.UserId;
                            dep.CreatedDate = indianTime;
                            dep.UpdatedDate = indianTime;
                            dep.TTEDDP_Days = d.TTEDDP_Days;
                            dep.TTEDDP_level = d.TTEDDP_level;
                            dep.HRME_Id = d.HRME_Id;
                            dep.TTEDDP_ToDate = d.TTEDDP_ToDate;
                            dep.TTEDDP_FromDate = d.TTEDDP_FromDate;
                            dep.TTEDDP_Hours = d.TTEDDP_Hours;
                            _IssueContext.Add(dep);
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

        //Task Creation For Client IN IVRM
        public async Task<ISM_TaskCreationDTO> getdetailsClient(ISM_TaskCreationDTO data)
        {
            try
            {


                data.get_priority = _IssueContext.MastersPriorities_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMP_ActiveFlag == true).Distinct().OrderBy(t => t.HRMP_Order).ToArray();
                data.get_days = _IssueContext.masterday.Where(t => t.MI_Id == data.MI_Id && t.FOMD_ActiveFlag == true).Distinct().OrderBy(t => t.FOMD_Id).ToArray();


                try
                {

                    data.get_department = (from a in _IssueContext.HR_Master_Department
                                           where (a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMD_DepartmentName == "Client")
                                           select new ISM_TaskCreationDTO
                                           {
                                               HRMD_Id = a.HRMD_Id,
                                               HRMDC_ID = a.HRMDC_ID,
                                               HRMD_DepartmentName = a.HRMD_DepartmentName
                                           }).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                    data.get_client = (from A in _IssueContext.clientTable
                                       from B in _IssueContext.ISM_Master_Client_UserMappingDMO
                                       where (A.ISMMCLT_Id == B.ISMMCLT_Id && A.ISMMCLT_ActiveFlag == true && B.ISMCIM_IEList == data.UserId)
                                       select new ISM_ComplaintsTaskListDTO
                                       {
                                           ISMMCLT_Id = A.ISMMCLT_Id,
                                           ISMMCLT_ClientName = A.ISMMCLT_ClientName
                                       }).Distinct().OrderBy(t => t.ISMMCLT_ClientName).ToArray();

                    data.get_project = (from a in _IssueContext.clientTable
                                        from b in _IssueContext.MastersProject_DMO
                                        from c in _IssueContext.ISM_Master_Client_ProjectDMO
                                        where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.ISMMCLT_ActiveFlag == true && b.ISMMPR_ActiveFlg == true && c.ISMMCLTPR_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                        select new ISM_TaskCreationDTO
                                        {
                                            ISMMPR_Id = c.ISMMPR_Id,
                                            ISMMPR_ProjectName = b.ISMMPR_ProjectName
                                        }).Distinct().OrderBy(a => a.ISMMPR_ProjectName).ToArray();

                    //*********************************** Client Task List ******************************************//           
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Client_Task_List";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@User_Id",
                SqlDbType.BigInt)
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



                //************ Planner Creation Extension ******************//
                //  var getplannernamecount = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id == empid && t.ISMTPL_ActiveFlg == true && t.ISMTPL_ApprovalFlg == false && t.ISMTPL_ApprovedBy == 0
                //  && Convert.ToDateTime(t.ISMTPL_StartDate) <= Convert.ToDateTime(System.DateTime.Today.Date)
                //  && Convert.ToDateTime(t.ISMTPL_EndDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).ToList();
                //  if (getplannernamecount.Count == 0)
                //  {

                //      data.plannerextapproval = false;
                //      System.Globalization.CultureInfo ci =
                //      System.Threading.Thread.CurrentThread.CurrentCulture;
                //      DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
                //      DayOfWeek today = DateTime.Now.DayOfWeek;
                //      data.plannerMaxdate = DateTime.Now.AddDays(-(today - fdow)).Date;
                //      data.plannerMaxdate = data.plannerMaxdate.AddDays(7).Date;


                //      //******************************* Calculate Effort *****************************//
                //      DateTime datetimennow = DateTime.Now;
                //      datetimennow = Convert.ToDateTime(datetimennow.Date.ToString("yyyy-MM-dd"));
                //      long lyear_id = 0;
                //      var yearid = _IssueContext.HR_Master_LeaveYearDMO.Where(l => l.MI_Id == data.MI_Id && l.HRMLY_ActiveFlag == true && (l.HRMLY_FromDate <= datetimennow && l.HRMLY_ToDate >= datetimennow)).Distinct().ToList();

                //      var dep_id = _IssueContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;

                //      if (yearid.Count > 0)
                //      {
                //          lyear_id = yearid.FirstOrDefault().HRMLY_Id;
                //      }
                //      using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //      {
                //          cmd.CommandText = "ISM_PLANNER_CREATION_CHECK";
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
                //          cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                // SqlDbType.BigInt)
                //          {
                //              Value = empid
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
                //              var datelist = retObject.ToList();


                //              if (datelist.Count > 0)
                //              {
                //                  if (Convert.ToDateTime(System.DateTime.Today.Date) == datelist[0].PDates)
                //                  {
                //                      data.plannerextapproval = true;

                //                      var planner_extension = _IssueContext.ISM_PlannerExtensionDMO.Where(a => a.HRME_Id == empid && a.ISMPLE_ActiveFlg == true).Distinct().ToList();
                //                      data.plannerextension = planner_extension.ToArray();
                //                      if (planner_extension.Count > 0)
                //                      {
                //                          var acd_Id = _IssueContext.ISM_PlannerExtensionDMO.Where(t => t.HRME_Id == empid && Convert.ToDateTime(t.ISMPLE_FromDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ISMPLE_ToDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).OrderByDescending(t => t.CreatedDate).ToList();
                //                          if (acd_Id.Count > 0)
                //                          {
                //                              data.plannerextapproval = true;
                //                              var extdate = acd_Id.FirstOrDefault().ISMPLE_FromDate;
                //                              data.plannerMaxdate = extdate;
                //                          }
                //                      }
                //                  }
                //                  else
                //                  {
                //                      var planner_extension = _IssueContext.ISM_PlannerExtensionDMO.Where(a => a.HRME_Id == empid && a.ISMPLE_ActiveFlg == true).Distinct().ToList();
                //                      data.plannerextension = planner_extension.ToArray();
                //                      if (planner_extension.Count > 0)
                //                      {
                //                          var acd_Id = _IssueContext.ISM_PlannerExtensionDMO.Where(t => t.HRME_Id == empid && Convert.ToDateTime(t.ISMPLE_FromDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ISMPLE_ToDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).OrderByDescending(t => t.CreatedDate).ToList();
                //                          if (acd_Id.Count > 0)
                //                          {
                //                              data.plannerextapproval = true;
                //                              var extdate = acd_Id.FirstOrDefault().ISMPLE_FromDate;
                //                              data.plannerMaxdate = extdate;
                //                          }
                //                      }
                //                  }
                //              }


                //          }
                //          catch (Exception ex)
                //          {
                //              Console.WriteLine(ex.Message);
                //          }
                //      }
                //  }
                //  else
                //  {
                //      data.plannerextapproval = false;
                //      System.Globalization.CultureInfo ci =
                //      System.Threading.Thread.CurrentThread.CurrentCulture;
                //      DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
                //      DayOfWeek today = DateTime.Now.DayOfWeek;
                //      data.plannerMaxdate = DateTime.Now.AddDays(-(today - fdow)).Date;
                //      data.plannerMaxdate = data.plannerMaxdate.AddDays(7).Date;
                //  }


                //var plnnerdate = _IssueContext.ISM_Task_PlannerDMO.Where(p => p.MI_Id == data.MI_Id && p.ISMTPL_ActiveFlg == true && p.HRME_Id == empid && ((p.ISMTPL_ApprovalFlg == false && p.ISMTPL_ApprovedBy == 0) || (p.ISMTPL_ApprovalFlg == true && p.ISMTPL_ApprovedBy != 0))
                //).OrderByDescending(e => e.ISMTPL_EndDate).Take(1).ToList();

                //if (plnnerdate.Count > 0)
                //{
                //    DateTime a = DateTime.Now;
                //    a = Convert.ToDateTime(plnnerdate[0].ISMTPL_EndDate);
                //    if (a > data.plannerMaxdate)
                //    {
                //        data.plannerMaxdate = a;

                //        data.plannerMaxdate = data.plannerMaxdate.AddDays(1).Date;
                //    }

                //}
                //if (data.plannerMaxdate > DateTime.Now)
                //{
                //    data.plannerextapproval = false;
                //    data.plannerMaxdate = DateTime.Now;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public ISM_TaskCreationDTO savedataClient(ISM_TaskCreationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                PushNotification pb = new PushNotification(_Context);
                long empid = 0;

                string role = "ClientUser";



                string Auto_taskNumber = AutoGenerateTaskNumber(data);
                data.Auto_id = Auto_taskNumber;

                if (role.Equals("ClientUser", StringComparison.OrdinalIgnoreCase) || (data.assignto == "N" && data.ISMTCR_BugOREnhancementFlg != "T"))
                {
                    SaveTaskDetails(data);

                    var contactExists = _IssueContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        // ============================================== Push Notification Client task creation     
                        if (role.Equals("ClientUser", StringComparison.OrdinalIgnoreCase))
                        {
                            var deviceidsnew = "";
                            var devicenew = "";
                            var devicelist = (from a in _IssueContext.ISM_TaskCreationDMO
                                              from b in _IssueContext.ISM_TaskCreation_ClientDMO
                                              from c in _IssueContext.clientTable
                                              from d in _IssueContext.clientMappingTable
                                              from e in _IssueContext.MasterEmployee
                                              where (a.ISMTCR_Id == b.ISMTCR_Id && b.ISMMCLT_Id == c.ISMMCLT_Id && c.ISMMCLT_Id == d.ISMMCLT_Id
                                              && d.ISMCIM_IEList == e.HRME_Id && a.ISMTCR_ActiveFlg == true && c.ISMMCLT_ActiveFlag == true
                                              && e.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && b.ISMMCLT_Id == data.ISMMCLT_Id)
                                              select new ISM_ComplaintsTaskListDTO
                                              {
                                                  HRME_Id = d.ISMCIM_IEList,
                                                  HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                              }).Distinct().OrderBy(t => t.HRME_Id).ToList();

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
                            var taskiddd = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_TaskNo == data.Auto_id);
                            try
                            {
                                string subject = "Client: New Task Created";
                                string titletext = taskiddd.ISMTCR_TaskNo + " - " + taskiddd.ISMTCR_Title;
                                if (data.sound == "")
                                {
                                    data.sound = "default";
                                }
                                callnotification(devicenew, empid, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                Insertnotification(data.MI_Id, empid, data.ISMTCR_Id, titletext, data.UserId, "Client");

                                var depheaderlist = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                                                     from b in _IssueContext.HR_Master_Department
                                                     from c in _IssueContext.MasterEmployee
                                                     from d in _IssueContext.HR_Master_DepartmentCodeDMO
                                                     where a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == c.HRME_Id
                                                     && b.HRMD_Id == c.HRMD_Id && a.HRMDC_ID == d.HRMDC_ID && b.HRMDC_ID == d.HRMDC_ID && (d.HRMDC_Code == "SW" || d.HRMDC_Code == "SQT" || d.HRMDC_Code == "STI")
                                                     select new ISM_ComplaintsTaskListDTO
                                                     {
                                                         HRME_Id = c.HRME_Id,
                                                         HRME_AppDownloadedDeviceId = c.HRME_AppDownloadedDeviceId
                                                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                if (depheaderlist.Count > 0)
                                {



                                    foreach (var item in depheaderlist)
                                    {

                                        if (item.HRME_Id != empid)
                                        {
                                            string dd = pb.savewebnotification(data.MI_Id, "NOTASSIGNTASKWEB", taskiddd.ISMTCR_Id, taskiddd.ISMTCR_Id, item.HRME_Id, data.UserId, "NotAssigned");

                                            var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                                                 from a in _IssueContext.Staff_User_Login
                                                                 where (e.ISMPNU_Userid == a.Id && a.Emp_Code == item.HRME_Id)
                                                                 select new ISM_ComplaintsTaskListDTO
                                                                 {
                                                                     UserId = e.ISMPNU_Userid
                                                                 }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                            if (webpushtocken.Count > 0)
                                            {
                                                foreach (var webtockenid in webpushtocken)
                                                {
                                                    PushNotification ObjPushNotification = new PushNotification(_Context);
                                                    ObjPushNotification.callpushnotification(data.ISMTCR_Title, "Client-New Task Creation!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskiddd.ISMTCR_Id, webtockenid.UserId);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            var depheaderlist = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.MasterEmployee
                                                 where a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == c.HRME_Id && b.HRMD_Id == data.HRMD_Id
                                                 && b.HRMD_Id == c.HRMD_Id
                                                 select new ISM_ComplaintsTaskListDTO
                                                 {
                                                     HRME_Id = c.HRME_Id,
                                                     HRME_AppDownloadedDeviceId = c.HRME_AppDownloadedDeviceId
                                                 }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                            var taskiddd = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_TaskNo == data.Auto_id);

                            if (depheaderlist.Count > 0)
                            {


                                foreach (var item in depheaderlist)
                                {

                                    if (item.HRME_Id != empid)
                                    {
                                        string dd = pb.savewebnotification(data.MI_Id, "NOTASSIGNTASKWEB", taskiddd.ISMTCR_Id, taskiddd.ISMTCR_Id, item.HRME_Id, data.UserId, "NotAssigned");

                                        var createemp = (from a in _IssueContext.MasterEmployee
                                                         from b in _IssueContext.HR_Master_Department
                                                         where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == item.HRME_Id && a.HRME_ActiveFlag == true)
                                                         select a.HRMD_Id);

                                        var assignemp = (from a in _IssueContext.MasterEmployee
                                                         from b in _IssueContext.HR_Master_Department
                                                         where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == empid && a.HRME_ActiveFlag == true)
                                                         select a.HRMD_Id);

                                        if (createemp != assignemp)
                                        {
                                            var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                                                 from a in _IssueContext.Staff_User_Login
                                                                 where (e.ISMPNU_Userid == a.Id && a.Emp_Code == item.HRME_Id)
                                                                 select new ISM_ComplaintsTaskListDTO
                                                                 {
                                                                     UserId = e.ISMPNU_Userid
                                                                 }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                            if (webpushtocken.Count > 0)
                                            {
                                                foreach (var webtockenid in webpushtocken)
                                                {
                                                    PushNotification ObjPushNotification = new PushNotification(_Context);
                                                    ObjPushNotification.callpushnotification(data.ISMTCR_Title, "New Task Created!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskiddd.ISMTCR_Id, webtockenid.UserId);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (depheaderlist.Count > 0)
                            {
                                string deviceidsnew = "";
                                string devicenew = "";
                                foreach (var deviceid in depheaderlist)
                                {
                                    if (deviceid.HRME_AppDownloadedDeviceId != null && deviceid.HRME_AppDownloadedDeviceId != "")
                                    {


                                        deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                        devicenew = "[" + deviceidsnew + "]";
                                        try
                                        {
                                            string subject = "New Task Created!";
                                            string titletext = taskiddd.ISMTCR_TaskNo + " - " + taskiddd.ISMTCR_Title;
                                            if (data.sound == "")
                                            {
                                                data.sound = "default";
                                            }
                                            callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                            Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Create");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                else if (data.assignto == "Y" || data.ISMTCR_BugOREnhancementFlg == "T")
                {
                    if (data.taskEmpArray.Length > 0)
                    {
                        foreach (var te in data.taskEmpArray)
                        {
                            SaveTaskDetails(data);

                            DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.startdate), INDIAN_ZONE);
                            DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.enddate), INDIAN_ZONE);

                            string confromdate = "";
                            string contodate = "";
                            try
                            {
                                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                contodate = todatecon.ToString("yyyy-MM-dd");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            var empHRMEid = _Context.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
                            long employeeid = 0;
                            var depart_id = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empHRMEid[0].Emp_Code).ToList();

                            var departmenthead = (from a in _IssueContext.HR_Master_Department
                                                  from b in _IssueContext.HR_Master_DepartmentCode_Head
                                                  from c in _IssueContext.Staff_User_Login
                                                  where (a.HRMD_Id == depart_id[0].HRMD_Id && a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.Emp_Code && a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                                  select new ISM_TaskCreationDTO
                                                  {
                                                      HRME_Id = b.HRME_ID,
                                                      UserId = c.Id
                                                  }).ToList();


                            ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                            taskasign.ISMTCR_Id = data.ISMTCR_Id;
                            taskasign.HRME_Id = te.HRME_Id;
                            taskasign.ISMTCRASTO_AssignedDate = indianTime;
                            taskasign.ISMTCRASTO_AssignedBy = employeeid = empid;
                            taskasign.ISMTCRASTO_Remarks = data.remarks;
                            taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(confromdate);
                            taskasign.ISMTCRASTO_EndDate = Convert.ToDateTime(contodate);
                            taskasign.ISMTCRASTO_EffortInHrs = data.effortinhrs;
                            taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                            taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                            taskasign.ISMTCRASTO_TimeRequiredFlg = data.TimeRequiredFlg;
                            taskasign.ISMTCRASTO_ActiveFlg = true;
                            taskasign.CreatedDate = indianTime;
                            taskasign.UpdatedDate = indianTime;
                            _IssueContext.Add(taskasign);

                            if (data.ISMTCR_BugOREnhancementFlg == "T")
                            {
                                ISM_Task_Advance_PlannerDMO adv = new ISM_Task_Advance_PlannerDMO();
                                adv.ISMTCR_Id = data.ISMTCR_Id;
                                adv.ISMTAPL_Remarks = data.remarks;
                                adv.MI_Id = data.MI_Id;
                                adv.HRME_Id = te.HRME_Id;
                                adv.ISMTAPL_Year = data.ASMAY_Id;
                                adv.ISMTAPL_TimeRequired = data.effortinhrs;
                                adv.ISMTAPL_FromDate = Convert.ToDateTime(confromdate);
                                adv.ISMTAPL_ToDate = Convert.ToDateTime(contodate);
                                adv.ISMTAPL_Periodicity = data.periodicity.Trim();
                                adv.ISMTAPL_TimeRequiredFlg = data.TimeRequiredFlg.Trim();
                                adv.ISMTAPL_CreatedBy = data.UserId;
                                adv.ISMTAPL_UpdatedBy = data.UserId;
                                adv.ISMTAPL_ActiveFlg = true;
                                adv.CreatedDate = DateTime.Now;
                                adv.UpdatedDate = DateTime.Now;
                                adv.ISMTAPL_OFFDate = data.Yearlydate;
                                adv.ISMTAPL_Day = Convert.ToInt64(data.TaskDay);
                                adv.ISMTAPL_ApprovalFlag = 1;
                                _IssueContext.Add(adv);

                                ISM_Task_Advance_Planner_DatesDMO datedmo = new ISM_Task_Advance_Planner_DatesDMO();
                                datedmo.ISMTAPL_Id = adv.ISMTAPL_Id;
                                datedmo.ISMTAPLD_StartDate = Convert.ToDateTime(confromdate);
                                datedmo.ISMTAPLD_EndDate = Convert.ToDateTime(contodate);
                                datedmo.ISMTAPLD_CreatedBy = data.UserId;
                                datedmo.ISMTAPLD_UpdatedBy = data.UserId;
                                datedmo.ISMTAPLD_ActiveFlg = true;
                                datedmo.ISMTAPLD_CreatedDate = DateTime.Now;
                                datedmo.ISMTAPLD_UpdatedDate = DateTime.Now;
                                _IssueContext.Add(datedmo);
                            }

                            var i = _IssueContext.SaveChanges();

                            if (i > 0)
                            {
                                data.returnval = true;

                                var taskiddd = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_TaskNo == data.Auto_id).OrderByDescending(t => t.ISMTCR_Id).ToList();
                                var assignedTolist = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                      where b.ISMTCR_Id == taskiddd.FirstOrDefault().ISMTCR_Id
                                                      select new ISM_ComplaintsTaskListDTO
                                                      {
                                                          HRME_Id = b.HRME_Id
                                                      }
                                                    ).Distinct().ToList(); ;

                                if (assignedTolist.Count > 0)
                                {
                                    foreach (var item in assignedTolist)
                                    {
                                        string dd = pb.savewebnotification(data.MI_Id, "TASKASSIGNEDWEB", item.ISMTCRASTO_Id, taskiddd.FirstOrDefault().ISMTCR_Id, item.HRME_Id, data.UserId, "Assigned");
                                    }
                                }




                                var devicelistto = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                    from e in _IssueContext.MasterEmployee
                                                    where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.HRME_Id == e.HRME_Id && e.HRME_AppDownloadedDeviceId != null
                                                    && e.HRME_AppDownloadedDeviceId != "")
                                                    select new ISM_ComplaintsTaskListDTO
                                                    {
                                                        HRME_Id = b.HRME_Id,
                                                        HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                    }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                var devicelistfrom = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                      from e in _IssueContext.MasterEmployee
                                                      where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.ISMTCRASTO_AssignedBy == e.HRME_Id
                                                      && e.HRME_AppDownloadedDeviceId != null && e.HRME_AppDownloadedDeviceId != ""
                                                    )
                                                      select new ISM_ComplaintsTaskListDTO
                                                      {
                                                          HRME_Id = b.HRME_Id,
                                                          HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                      }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                if (devicelistto.Count > 0 && devicelistfrom.Count > 0)
                                {
                                    var tasklist = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_TaskNo == data.Auto_id).OrderByDescending(t => t.ISMTCR_Id).ToList();
                                    if (devicelistto.FirstOrDefault().HRME_Id != devicelistfrom.FirstOrDefault().HRME_Id)
                                    {
                                        var deviceidsnew = "";
                                        var devicenew = "";
                                        if (devicelistto.Count > 0)
                                        {
                                            foreach (var deviceid in devicelistto)
                                            {
                                                deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';

                                                devicenew = "[" + deviceidsnew + "]";

                                                try
                                                {
                                                    string titletext = tasklist.FirstOrDefault().ISMTCR_TaskNo + " - " + tasklist.FirstOrDefault().ISMTCR_Title;
                                                    string subject = "New Task Assigned";
                                                    if (data.sound == "")
                                                    {
                                                        data.sound = "default";
                                                    }
                                                    callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                    Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Assigned");
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }
                                        }

                                        if (devicelistfrom.Count > 0)
                                        {
                                            foreach (var deviceid in devicelistfrom)
                                            {
                                                deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                                devicenew = "[" + deviceidsnew + "]";
                                                try
                                                {
                                                    string titletext = tasklist.FirstOrDefault().ISMTCR_TaskNo + " - " + tasklist.FirstOrDefault().ISMTCR_Title;
                                                    string subject = "New Task Created";
                                                    if (data.sound == "")
                                                    {
                                                        data.sound = "default";
                                                    }
                                                    callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                    Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Assigned");
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (te.HRME_Id != empid)
                                {
                                    //var webpushtocken = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                    //                     from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                    //                     from a in _IssueContext.Staff_User_Login
                                    //                     where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.HRME_Id == a.Emp_Code && b.HRME_Id == te.HRME_Id && a.Emp_Code== te.HRME_Id)
                                    //                     select new ISM_ComplaintsTaskListDTO
                                    //                     {
                                    //                         UserId = e.ISMPNU_Userid
                                    //                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                    List<ISM_TaskCreationDTO> webpushtocken = new List<ISM_TaskCreationDTO>();
                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "ISM_GET_PUSHNOTIFICATION_USERS";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add(new SqlParameter("@hrmeid",
                                      SqlDbType.BigInt)
                                        {
                                            Value = te.HRME_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@taskid",
                                        SqlDbType.BigInt)
                                        {
                                            Value = taskasign.ISMTCR_Id
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
                                                    webpushtocken.Add(new ISM_TaskCreationDTO
                                                    {
                                                        UserId = Convert.ToInt64(dataReader["UserId"])

                                                    });
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    if (webpushtocken.Count > 0)
                                    {
                                        foreach (var webtockenid in webpushtocken)
                                        {
                                            PushNotification ObjPushNotification = new PushNotification(_Context);
                                            ObjPushNotification.callpushnotification(data.ISMTCR_Title, "New Task Assigned!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskasign.ISMTCR_Id, webtockenid.UserId);
                                        }
                                    }
                                }
                            }
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


        // Dont Use 
        public ISM_TaskCreationDTO savedata_working(ISM_TaskCreationDTO data)
        {
            try
            {

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                string role = "";
                if (userrole.Count > 0)
                {
                    role = userrole.FirstOrDefault().IVRMRT_Role;
                    data.roletype = role;

                }
                long client_id = 0;
                if (role == "ClientUser")
                {
                    client_id = 0;
                }
                else
                {
                    client_id = data.ISMMCLT_Id;
                }

                if (data.ISMTCR_Id != 0)
                {
                    var res = _IssueContext.ISM_TaskCreationDMO.Where(t => t.HRMD_Id == data.HRMD_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.IVRMM_Id == data.IVRMM_Id && t.HRMPR_Id == data.HRMPR_Id && t.MI_Id == data.MI_Id && t.ISMTCR_Id != data.ISMTCR_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_Id == data.ISMTCR_Id);
                        result.MI_Id = data.MI_Id;
                        result.HRMD_Id = data.HRMD_Id;
                        result.ISMMPR_Id = data.ISMMPR_Id;
                        result.ISMMTCAT_Id = data.ISMMTCAT_Id;
                        result.ISMMCLT_Id = client_id;
                        result.IVRMM_Id = data.IVRMM_Id;
                        result.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                        result.ISMTCR_CreationDate = data.ISMTCR_CreationDate;
                        result.ISMTCR_Title = data.ISMTCR_Title;
                        result.HRMPR_Id = data.HRMPR_Id;
                        result.ISMTCR_Desc = data.ISMTCR_Desc;
                        result.ISMTCR_Status = data.ISMTCR_Status;
                        result.HRME_Id = empid;
                        result.ISMTCR_ReOpenFlg = false;
                        result.ISMTCR_UpdatedBy = data.UserId;
                        result.ISMTCR_ActiveFlg = true;
                        result.UpdatedDate = indianTime;
                        _IssueContext.Update(result);

                        if (data.ISMMCLT_Id > 0)
                        {
                            var res0 = _IssueContext.ISM_TaskCreation_ClientDMO.Where(a => a.ISMTCRCL_Id == data.ISMTCRCL_Id).ToList();
                            if (res0.Count > 0)
                            {
                                var res01 = _IssueContext.ISM_TaskCreation_ClientDMO.Single(a => a.ISMTCRCL_Id == data.ISMTCRCL_Id);
                                res01.ISMTCR_Id = result.ISMTCR_Id;
                                res01.ISMMCLT_Id = data.ISMMCLT_Id;
                                res01.ISMTCRCL_ActiveFlg = true;
                                res01.ISMTCRCL_CreatedBy = data.UserId;
                                res01.ISMTCRCL_UpdatedBy = data.UserId;
                                res01.UpdatedDate = indianTime;
                                _IssueContext.Update(res01);
                            }
                            else
                            {
                                ISM_TaskCreation_ClientDMO uptaskclt = new ISM_TaskCreation_ClientDMO();
                                uptaskclt.ISMTCR_Id = result.ISMTCR_Id;
                                uptaskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                                uptaskclt.ISMTCRCL_ActiveFlg = true;
                                uptaskclt.ISMTCRCL_CreatedBy = data.UserId;
                                uptaskclt.ISMTCRCL_UpdatedBy = data.UserId;
                                uptaskclt.CreatedDate = indianTime;
                                uptaskclt.UpdatedDate = indianTime;
                                _IssueContext.Add(uptaskclt);
                            }
                        }
                        if (data.attachmentArray != null)
                        {
                            foreach (var file in data.attachmentArray)
                            {
                                if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                                {
                                    var res1 = _IssueContext.ISM_TaskCreation_AttachmentDMO.Where(a => a.ISMTCRAT_Id == data.ISMTCRAT_Id).ToList();
                                    if (res1.Count > 0)
                                    {
                                        var res11 = _IssueContext.ISM_TaskCreation_AttachmentDMO.Single(a => a.ISMTCRAT_Id == data.ISMTCRAT_Id);
                                        res11.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                        res11.ISMTCRAT_File = file.ISMTCRAT_File;
                                        res11.ISMTCRAT_UpdatedBy = data.UserId;
                                        res11.UpdatedDate = indianTime;
                                        _IssueContext.Update(res11);
                                    }
                                    else
                                    {
                                        ISM_TaskCreation_AttachmentDMO att = new ISM_TaskCreation_AttachmentDMO();
                                        att.ISMTCR_Id = result.ISMTCR_Id;
                                        att.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                        att.ISMTCRAT_File = file.ISMTCRAT_File;
                                        att.ISMTCRAT_ActiveFlg = true;
                                        att.ISMTCRAT_CreatedBy = data.UserId;
                                        att.ISMTCRAT_UpdatedBy = data.UserId;
                                        att.CreatedDate = indianTime;
                                        att.UpdatedDate = indianTime;
                                        _IssueContext.Add(att);
                                    }
                                }

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
                }

                else
                {
                    Master_NumberingDTO check = new Master_NumberingDTO();
                    data.transnumbconfigurationsettingsss = check;
                    List<Master_Numbering> MM = new List<Master_Numbering>();
                    MM = _Context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "ISMTASK").ToList();
                    if (MM.Count() > 0)
                    {
                        data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                        data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                        data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                        data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                        data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                        data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                        data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                        data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                        data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                        data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                        data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                        data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                        data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                        data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                        data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                        data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                        data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                    }

                    if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Context);
                        data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                        data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                        data.Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                    }

                    ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                    task.ISMTCR_TaskNo = data.Auto_id;
                    task.MI_Id = data.MI_Id;
                    task.ISMMTGRP_Id = 0;
                    task.ISMTCR_TGOrder = 0;
                    task.HRMD_Id = data.HRMD_Id;
                    task.ISMMPR_Id = data.ISMMPR_Id;
                    task.IVRMM_Id = data.IVRMM_Id;
                    task.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                    task.ISMTCR_CreationDate = indianTime;
                    task.ISMTCR_Title = data.ISMTCR_Title;
                    task.HRMPR_Id = data.HRMPR_Id;
                    task.HRME_Id = empid;
                    task.ISMTCR_Desc = data.ISMTCR_Desc;
                    task.ISMTCR_Status = data.ISMTCR_Status;
                    task.ISMMTCAT_Id = data.ISMMTCAT_Id;
                    task.ISMMCLT_Id = client_id;
                    task.ISMTCR_ReOpenFlg = false;
                    task.ISMTCR_ActiveFlg = true;
                    task.ISMTCR_CreatedBy = data.UserId;
                    task.ISMTCR_UpdatedBy = data.UserId;
                    task.CreatedDate = indianTime;
                    task.UpdatedDate = indianTime;
                    _IssueContext.Add(task);

                    if (data.ISMMCLT_Id > 0)
                    {
                        ISM_TaskCreation_ClientDMO taskclt = new ISM_TaskCreation_ClientDMO();
                        taskclt.ISMTCR_Id = task.ISMTCR_Id;
                        taskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                        taskclt.ISMTCRCL_ActiveFlg = true;
                        taskclt.ISMTCRCL_CreatedBy = data.UserId;
                        taskclt.ISMTCRCL_UpdatedBy = data.UserId;
                        taskclt.CreatedDate = indianTime;
                        taskclt.UpdatedDate = indianTime;
                        _IssueContext.Add(taskclt);
                    }

                    if (data.attachmentArray != null)
                    {
                        foreach (var file in data.attachmentArray)
                        {
                            if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                            {
                                ISM_TaskCreation_AttachmentDMO taskatt = new ISM_TaskCreation_AttachmentDMO();
                                taskatt.ISMTCR_Id = task.ISMTCR_Id;
                                taskatt.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                taskatt.ISMTCRAT_File = file.ISMTCRAT_File;
                                taskatt.ISMTCRAT_ActiveFlg = true;
                                taskatt.ISMTCRAT_CreatedBy = data.UserId;
                                taskatt.ISMTCRAT_UpdatedBy = data.UserId;
                                taskatt.CreatedDate = indianTime;
                                taskatt.UpdatedDate = indianTime;
                                _IssueContext.Add(taskatt);
                            }
                        }
                    }

                    var contactExists = _IssueContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        // ============================================== Push Notification Client task creation     
                        if (role.Equals("ClientUser", StringComparison.OrdinalIgnoreCase))
                        {
                            var deviceidsnew = "";
                            var devicenew = "";
                            var devicelist = (from a in _IssueContext.ISM_TaskCreationDMO
                                              from b in _IssueContext.ISM_TaskCreation_ClientDMO
                                              from c in _IssueContext.clientTable
                                              from d in _IssueContext.clientMappingTable
                                              from e in _IssueContext.MasterEmployee
                                              where (a.ISMTCR_Id == b.ISMTCR_Id && b.ISMMCLT_Id == c.ISMMCLT_Id && c.ISMMCLT_Id == d.ISMMCLT_Id && d.ISMCIM_IEList == e.HRME_Id
                                              && a.ISMTCR_ActiveFlg == true && c.ISMMCLT_ActiveFlag == true && e.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && b.ISMMCLT_Id == data.ISMMCLT_Id)
                                              select new ISM_ComplaintsTaskListDTO
                                              {
                                                  HRME_Id = d.ISMCIM_IEList,
                                                  HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                              }).Distinct().OrderBy(t => t.HRME_Id).ToList();

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
                                string titletext = "Client: New Task Created";
                                string subject = "Client: New Task Created";
                                if (data.sound == "")
                                {
                                    data.sound = "default";
                                }
                                callnotification(devicenew, empid, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                Insertnotification(data.MI_Id, empid, data.ISMTCR_Id, titletext, data.UserId, "Client");

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    if (data.assignto == "Y" || data.ISMTCR_BugOREnhancementFlg == "T")
                    {
                        if (data.taskEmpArray.Count() > 0)
                        {
                            foreach (var te in data.taskEmpArray)
                            {
                                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.startdate), INDIAN_ZONE);
                                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.enddate), INDIAN_ZONE);

                                string confromdate = "";
                                string contodate = "";
                                try
                                {
                                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                    contodate = todatecon.ToString("yyyy-MM-dd");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                                taskasign.ISMTCR_Id = task.ISMTCR_Id;
                                taskasign.HRME_Id = te.HRME_Id;
                                taskasign.ISMTCRASTO_AssignedDate = indianTime;
                                taskasign.ISMTCRASTO_AssignedBy = empid;
                                taskasign.ISMTCRASTO_Remarks = data.remarks;
                                taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(confromdate);
                                taskasign.ISMTCRASTO_EndDate = Convert.ToDateTime(contodate);
                                taskasign.ISMTCRASTO_EffortInHrs = data.effortinhrs;
                                taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                                taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                                taskasign.ISMTCRASTO_TimeRequiredFlg = data.TimeRequiredFlg;
                                taskasign.ISMTCRASTO_ActiveFlg = true;
                                taskasign.CreatedDate = indianTime;
                                taskasign.UpdatedDate = indianTime;
                                _IssueContext.Add(taskasign);
                                var contactExistsc = _IssueContext.SaveChanges();

                                if (data.ISMTCR_BugOREnhancementFlg == "T")
                                {
                                    ISM_Task_Advance_PlannerDMO adv = new ISM_Task_Advance_PlannerDMO();
                                    adv.ISMTCR_Id = task.ISMTCR_Id;
                                    adv.ISMTAPL_Remarks = data.remarks;
                                    adv.MI_Id = data.MI_Id;
                                    adv.HRME_Id = te.HRME_Id;
                                    adv.ISMTAPL_Year = data.ASMAY_Id;
                                    adv.ISMTAPL_TimeRequired = data.effortinhrs;
                                    adv.ISMTAPL_FromDate = Convert.ToDateTime(confromdate);
                                    adv.ISMTAPL_ToDate = Convert.ToDateTime(contodate);
                                    adv.ISMTAPL_Periodicity = data.periodicity.Trim();
                                    adv.ISMTAPL_TimeRequiredFlg = data.TimeRequiredFlg.Trim();
                                    adv.ISMTAPL_CreatedBy = data.UserId;
                                    adv.ISMTAPL_UpdatedBy = data.UserId;
                                    adv.ISMTAPL_ActiveFlg = true;
                                    adv.CreatedDate = DateTime.Now;
                                    adv.UpdatedDate = DateTime.Now;
                                    adv.ISMTAPL_OFFDate = data.Yearlydate;
                                    adv.ISMTAPL_Day = Convert.ToInt64(data.TaskDay);
                                    adv.ISMTAPL_ApprovalFlag = 1;
                                    _IssueContext.Add(adv);
                                    var contactExistAdv = _IssueContext.SaveChanges();

                                    ISM_Task_Advance_Planner_DatesDMO datedmo = new ISM_Task_Advance_Planner_DatesDMO();
                                    datedmo.ISMTAPL_Id = adv.ISMTAPL_Id;
                                    datedmo.ISMTAPLD_StartDate = Convert.ToDateTime(confromdate);
                                    datedmo.ISMTAPLD_EndDate = Convert.ToDateTime(contodate);
                                    datedmo.ISMTAPLD_CreatedBy = data.UserId;
                                    datedmo.ISMTAPLD_UpdatedBy = data.UserId;
                                    datedmo.ISMTAPLD_ActiveFlg = true;
                                    datedmo.ISMTAPLD_CreatedDate = DateTime.Now;
                                    datedmo.ISMTAPLD_UpdatedDate = DateTime.Now;
                                    _IssueContext.Add(datedmo);
                                    var Existdatedmoc = _IssueContext.SaveChanges();
                                }



                                var devicelistto = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                    from e in _IssueContext.MasterEmployee
                                                    where (b.ISMTCR_Id == task.ISMTCR_Id && b.HRME_Id == e.HRME_Id && e.HRME_AppDownloadedDeviceId != null
                                                    && e.HRME_AppDownloadedDeviceId != "")
                                                    select new ISM_ComplaintsTaskListDTO
                                                    {
                                                        HRME_Id = b.HRME_Id,
                                                        HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                    }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                var devicelistfrom = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                      from e in _IssueContext.MasterEmployee
                                                      where (b.ISMTCR_Id == task.ISMTCR_Id && b.ISMTCRASTO_AssignedBy == e.HRME_Id
                                                      && e.HRME_AppDownloadedDeviceId != null && e.HRME_AppDownloadedDeviceId != ""
                                                    )
                                                      select new ISM_ComplaintsTaskListDTO
                                                      {
                                                          HRME_Id = b.HRME_Id,
                                                          HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                      }).Distinct().OrderBy(t => t.HRME_Id).ToList();


                                if (devicelistto.Count > 0 && devicelistfrom.Count > 0)
                                {
                                    if (devicelistto.FirstOrDefault().HRME_Id != devicelistfrom.FirstOrDefault().HRME_Id)
                                    {
                                        var deviceidsnew = "";
                                        var devicenew = "";
                                        if (devicelistto.Count > 0)
                                        {

                                            foreach (var deviceid in devicelistto)
                                            {
                                                deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';

                                                devicenew = "[" + deviceidsnew + "]";

                                                try
                                                {
                                                    string titletext = "New Task Assigned";
                                                    string subject = "New Task Assigned";
                                                    if (data.sound == "")
                                                    {
                                                        data.sound = "default";
                                                    }
                                                    callnotification(devicenew, deviceid.HRME_Id, task.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                    Insertnotification(data.MI_Id, deviceid.HRME_Id, task.ISMTCR_Id, titletext, data.UserId, "Assigned");

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }
                                        }

                                        if (devicelistfrom.Count > 0)
                                        {
                                            foreach (var deviceid in devicelistfrom)
                                            {
                                                deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                                devicenew = "[" + deviceidsnew + "]";
                                                try
                                                {
                                                    string titletext = "New Task Created";
                                                    string subject = "New Task Created";
                                                    if (data.sound == "")
                                                    {
                                                        data.sound = "default";
                                                    }
                                                    callnotification(devicenew, deviceid.HRME_Id, task.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                    Insertnotification(data.MI_Id, deviceid.HRME_Id, task.ISMTCR_Id, titletext, data.UserId, "Assigned");
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }


                            }
                        }
                    }
                    else
                    {
                        var depheaderlist = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                                             from b in _IssueContext.HR_Master_Department
                                             from c in _IssueContext.MasterEmployee
                                             where a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == c.HRME_Id && b.HRMD_Id == data.HRMD_Id
                                             && b.HRMD_Id == c.HRMD_Id && c.HRME_AppDownloadedDeviceId != null && c.HRME_AppDownloadedDeviceId != ""
                                             select new ISM_ComplaintsTaskListDTO
                                             {
                                                 HRME_Id = c.HRME_Id,
                                                 HRME_AppDownloadedDeviceId = c.HRME_AppDownloadedDeviceId
                                             }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                        if (depheaderlist.Count > 0)
                        {
                            string deviceidsnew = "";
                            string devicenew = "";
                            foreach (var deviceid in depheaderlist)
                            {
                                deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                devicenew = "[" + deviceidsnew + "]";
                                try
                                {
                                    string titletext = "New Task Created";
                                    string subject = "New Task Created";
                                    if (data.sound == "")
                                    {
                                        data.sound = "default";
                                    }
                                    callnotification(devicenew, deviceid.HRME_Id, task.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                    Insertnotification(data.MI_Id, deviceid.HRME_Id, task.ISMTCR_Id, titletext, data.UserId, "Create");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
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

        //Department Wise Task Creation
        public ISM_TaskCreationDTO SaveTaskDeptWise(ISM_TaskCreationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                PushNotification pb = new PushNotification(_Context);
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    data.HRME_Id = empid;
                }

                var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                string role = "";
                if (userrole.Count > 0)
                {
                    role = userrole.FirstOrDefault().IVRMRT_Role;
                    data.roletype = role;

                }
                long client_id = 0;
                if (role == "ClientUser")
                {
                    client_id = 0;
                }
                else
                {
                    client_id = data.ISMMCLT_Id;
                }

                if (data.ISMTCR_Id != 0)
                {
                    var res = _IssueContext.ISM_TaskCreationDMO.Where(t => t.HRMD_Id == data.HRMD_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.IVRMM_Id == data.IVRMM_Id && t.HRMPR_Id == data.HRMPR_Id && t.MI_Id == data.MI_Id && t.ISMTCR_Id != data.ISMTCR_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_Id == data.ISMTCR_Id);
                        result.MI_Id = data.MI_Id;
                        result.HRMD_Id = data.HRMD_Id;
                        result.ISMMPR_Id = data.ISMMPR_Id;
                        result.ISMMTCAT_Id = data.ISMMTCAT_Id;
                        result.ISMMCLT_Id = client_id;
                        result.IVRMM_Id = data.IVRMM_Id;
                        result.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                        result.ISMTCR_CreationDate = data.ISMTCR_CreationDate;
                        result.ISMTCR_Title = data.ISMTCR_Title;
                        result.HRMPR_Id = data.HRMPR_Id;
                        result.ISMTCR_Desc = data.ISMTCR_Desc;
                        result.ISMTCR_Status = data.ISMTCR_Status;
                        result.HRME_Id = empid;
                        result.ISMTCR_ReOpenFlg = false;
                        result.ISMTCR_UpdatedBy = data.UserId;
                        result.ISMTCR_ActiveFlg = true;
                        result.UpdatedDate = indianTime;
                        _IssueContext.Update(result);

                        if (data.ISMMCLT_Id > 0)
                        {
                            var res0 = _IssueContext.ISM_TaskCreation_ClientDMO.Where(a => a.ISMTCRCL_Id == data.ISMTCRCL_Id).ToList();
                            if (res0.Count > 0)
                            {
                                var res01 = _IssueContext.ISM_TaskCreation_ClientDMO.Single(a => a.ISMTCRCL_Id == data.ISMTCRCL_Id);
                                res01.ISMTCR_Id = result.ISMTCR_Id;
                                res01.ISMMCLT_Id = data.ISMMCLT_Id;
                                res01.ISMTCRCL_ActiveFlg = true;
                                res01.ISMTCRCL_CreatedBy = data.UserId;
                                res01.ISMTCRCL_UpdatedBy = data.UserId;
                                res01.UpdatedDate = indianTime;
                                _IssueContext.Update(res01);
                            }
                            else
                            {
                                ISM_TaskCreation_ClientDMO uptaskclt = new ISM_TaskCreation_ClientDMO();
                                uptaskclt.ISMTCR_Id = result.ISMTCR_Id;
                                uptaskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                                uptaskclt.ISMTCRCL_ActiveFlg = true;
                                uptaskclt.ISMTCRCL_CreatedBy = data.UserId;
                                uptaskclt.ISMTCRCL_UpdatedBy = data.UserId;
                                uptaskclt.CreatedDate = indianTime;
                                uptaskclt.UpdatedDate = indianTime;
                                _IssueContext.Add(uptaskclt);
                            }
                        }
                        if (data.attachmentArray != null)
                        {
                            foreach (var file in data.attachmentArray)
                            {
                                if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                                {
                                    var res1 = _IssueContext.ISM_TaskCreation_AttachmentDMO.Where(a => a.ISMTCRAT_Id == data.ISMTCRAT_Id).ToList();
                                    if (res1.Count > 0)
                                    {
                                        var res11 = _IssueContext.ISM_TaskCreation_AttachmentDMO.Single(a => a.ISMTCRAT_Id == data.ISMTCRAT_Id);
                                        res11.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                        res11.ISMTCRAT_File = file.ISMTCRAT_File;
                                        res11.ISMTCRAT_UpdatedBy = data.UserId;
                                        res11.UpdatedDate = indianTime;
                                        _IssueContext.Update(res11);
                                    }
                                    else
                                    {
                                        ISM_TaskCreation_AttachmentDMO att = new ISM_TaskCreation_AttachmentDMO();
                                        att.ISMTCR_Id = result.ISMTCR_Id;
                                        att.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                        att.ISMTCRAT_File = file.ISMTCRAT_File;
                                        att.ISMTCRAT_ActiveFlg = true;
                                        att.ISMTCRAT_CreatedBy = data.UserId;
                                        att.ISMTCRAT_UpdatedBy = data.UserId;
                                        att.CreatedDate = indianTime;
                                        att.UpdatedDate = indianTime;
                                        _IssueContext.Add(att);
                                    }
                                }

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
                }

                else
                {
                    string Auto_taskNumber = AutoGenerateTaskNumber(data);
                    data.Auto_id = Auto_taskNumber;

                    if (role.Equals("ClientUser", StringComparison.OrdinalIgnoreCase) || (data.assignto == "N" && data.ISMTCR_BugOREnhancementFlg != "T"))
                    {
                        SaveTaskDetailsDeptWise(data);

                        var contactExists = _IssueContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            // ============================================== Push Notification Client task creation     
                            if (role.Equals("ClientUser", StringComparison.OrdinalIgnoreCase))
                            {
                                var deviceidsnew = "";
                                var devicenew = "";
                                var devicelist = (from a in _IssueContext.ISM_TaskCreationDMO
                                                  from b in _IssueContext.ISM_TaskCreation_ClientDMO
                                                  from c in _IssueContext.clientTable
                                                  from d in _IssueContext.clientMappingTable
                                                  from e in _IssueContext.MasterEmployee
                                                  where (a.ISMTCR_Id == b.ISMTCR_Id && b.ISMMCLT_Id == c.ISMMCLT_Id && c.ISMMCLT_Id == d.ISMMCLT_Id
                                                  && d.ISMCIM_IEList == e.HRME_Id && a.ISMTCR_ActiveFlg == true && c.ISMMCLT_ActiveFlag == true
                                                  && e.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && b.ISMMCLT_Id == data.ISMMCLT_Id)
                                                  select new ISM_ComplaintsTaskListDTO
                                                  {
                                                      HRME_Id = d.ISMCIM_IEList,
                                                      HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                  }).Distinct().OrderBy(t => t.HRME_Id).ToList();

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
                                var taskiddd = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_TaskNo == data.Auto_id);
                                try
                                {
                                    string subject = "Client: New Task Created";
                                    string titletext = taskiddd.ISMTCR_TaskNo + " - " + taskiddd.ISMTCR_Title;
                                    if (data.sound == "")
                                    {
                                        data.sound = "default";
                                    }
                                    callnotification(devicenew, empid, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                    Insertnotification(data.MI_Id, empid, data.ISMTCR_Id, titletext, data.UserId, "Client");

                                    var depheaderlist = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                                                         from b in _IssueContext.HR_Master_Department
                                                         from c in _IssueContext.MasterEmployee
                                                         from d in _IssueContext.HR_Master_DepartmentCodeDMO
                                                         where a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == c.HRME_Id
                                                         && b.HRMD_Id == c.HRMD_Id && a.HRMDC_ID == d.HRMDC_ID && b.HRMDC_ID == d.HRMDC_ID && (d.HRMDC_Code == "SW" || d.HRMDC_Code == "SQT" || d.HRMDC_Code == "STI")
                                                         select new ISM_ComplaintsTaskListDTO
                                                         {
                                                             HRME_Id = c.HRME_Id,
                                                             HRME_AppDownloadedDeviceId = c.HRME_AppDownloadedDeviceId
                                                         }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                    if (depheaderlist.Count > 0)
                                    {



                                        foreach (var item in depheaderlist)
                                        {

                                            if (item.HRME_Id != empid)
                                            {
                                                string dd = pb.savewebnotification(data.MI_Id, "NOTASSIGNTASKWEB", taskiddd.ISMTCR_Id, taskiddd.ISMTCR_Id, item.HRME_Id, data.UserId, "NotAssigned");

                                                var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                                                     from a in _IssueContext.Staff_User_Login
                                                                     where (e.ISMPNU_Userid == a.Id && a.Emp_Code == item.HRME_Id)
                                                                     select new ISM_ComplaintsTaskListDTO
                                                                     {
                                                                         UserId = e.ISMPNU_Userid
                                                                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                                if (webpushtocken.Count > 0)
                                                {
                                                    foreach (var webtockenid in webpushtocken)
                                                    {
                                                        PushNotification ObjPushNotification = new PushNotification(_Context);
                                                        ObjPushNotification.callpushnotification(data.ISMTCR_Title, "Client-New Task Creation!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskiddd.ISMTCR_Id, webtockenid.UserId);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            else
                            {
                                var depheaderlist = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                                                     from b in _IssueContext.HR_Master_Department
                                                     from c in _IssueContext.MasterEmployee
                                                     where a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == c.HRME_Id && b.HRMD_Id == data.HRMD_Id
                                                     && b.HRMD_Id == c.HRMD_Id
                                                     select new ISM_ComplaintsTaskListDTO
                                                     {
                                                         HRME_Id = c.HRME_Id,
                                                         HRME_AppDownloadedDeviceId = c.HRME_AppDownloadedDeviceId
                                                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                var taskiddd = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_TaskNo == data.Auto_id);

                                if (depheaderlist.Count > 0)
                                {


                                    foreach (var item in depheaderlist)
                                    {

                                        if (item.HRME_Id != empid)
                                        {
                                            string dd = pb.savewebnotification(data.MI_Id, "NOTASSIGNTASKWEB", taskiddd.ISMTCR_Id, taskiddd.ISMTCR_Id, item.HRME_Id, data.UserId, "NotAssigned");

                                            var createemp = (from a in _IssueContext.MasterEmployee
                                                             from b in _IssueContext.HR_Master_Department
                                                             where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == item.HRME_Id && a.HRME_ActiveFlag == true)
                                                             select a.HRMD_Id);

                                            var assignemp = (from a in _IssueContext.MasterEmployee
                                                             from b in _IssueContext.HR_Master_Department
                                                             where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == empid && a.HRME_ActiveFlag == true)
                                                             select a.HRMD_Id);

                                            if (createemp != assignemp)
                                            {
                                                var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                                                     from a in _IssueContext.Staff_User_Login
                                                                     where (e.ISMPNU_Userid == a.Id && a.Emp_Code == item.HRME_Id)
                                                                     select new ISM_ComplaintsTaskListDTO
                                                                     {
                                                                         UserId = e.ISMPNU_Userid
                                                                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                                if (webpushtocken.Count > 0)
                                                {
                                                    foreach (var webtockenid in webpushtocken)
                                                    {
                                                        PushNotification ObjPushNotification = new PushNotification(_Context);
                                                        ObjPushNotification.callpushnotification(data.ISMTCR_Title, "New Task Created!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskiddd.ISMTCR_Id, webtockenid.UserId);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (depheaderlist.Count > 0)
                                {
                                    string deviceidsnew = "";
                                    string devicenew = "";
                                    foreach (var deviceid in depheaderlist)
                                    {
                                        if (deviceid.HRME_AppDownloadedDeviceId != null && deviceid.HRME_AppDownloadedDeviceId != "")
                                        {


                                            deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                            devicenew = "[" + deviceidsnew + "]";
                                            try
                                            {
                                                string subject = "New Task Created!";
                                                string titletext = taskiddd.ISMTCR_TaskNo + " - " + taskiddd.ISMTCR_Title;
                                                if (data.sound == "")
                                                {
                                                    data.sound = "default";
                                                }
                                                callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Create");
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                    else if (data.assignto == "Y" || data.ISMTCR_BugOREnhancementFlg == "T")
                    {
                        if (data.taskEmpArray.Length > 0)
                        {
                            foreach (var te in data.taskEmpArray)
                            {
                                SaveTaskDetailsDeptWise(data);

                                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.startdate), INDIAN_ZONE);
                                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.enddate), INDIAN_ZONE);

                                string confromdate = "";
                                string contodate = "";
                                try
                                {
                                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                    contodate = todatecon.ToString("yyyy-MM-dd");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                var empHRMEid = _Context.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
                                long employeeid = 0;
                                var depart_id = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empHRMEid[0].Emp_Code).ToList();

                                var departmenthead = (from a in _IssueContext.HR_Master_Department
                                                      from b in _IssueContext.HR_Master_DepartmentCode_Head
                                                      from c in _IssueContext.Staff_User_Login
                                                      where (a.HRMD_Id == depart_id[0].HRMD_Id && a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.Emp_Code && a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                                      select new ISM_TaskCreationDTO
                                                      {
                                                          HRME_Id = b.HRME_ID,
                                                          UserId = c.Id
                                                      }).ToList();


                                ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                                taskasign.ISMTCR_Id = data.ISMTCR_Id;
                                taskasign.HRME_Id = te.HRME_Id;
                                taskasign.ISMTCRASTO_AssignedDate = indianTime;
                                taskasign.ISMTCRASTO_AssignedBy = employeeid = empid;
                                taskasign.ISMTCRASTO_Remarks = data.remarks;
                                taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(confromdate);
                                taskasign.ISMTCRASTO_EndDate = Convert.ToDateTime(contodate);
                                taskasign.ISMTCRASTO_EffortInHrs = data.effortinhrs;
                                taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                                taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                                taskasign.ISMTCRASTO_TimeRequiredFlg = data.TimeRequiredFlg;
                                taskasign.ISMTCRASTO_ActiveFlg = true;
                                taskasign.CreatedDate = indianTime;
                                taskasign.UpdatedDate = indianTime;
                                _IssueContext.Add(taskasign);

                                if (data.ISMTCR_BugOREnhancementFlg == "T")
                                {
                                    ISM_Task_Advance_PlannerDMO adv = new ISM_Task_Advance_PlannerDMO();
                                    adv.ISMTCR_Id = data.ISMTCR_Id;
                                    adv.ISMTAPL_Remarks = data.remarks;
                                    adv.MI_Id = data.MI_Id;
                                    adv.HRME_Id = te.HRME_Id;
                                    adv.ISMTAPL_Year = data.ASMAY_Id;
                                    adv.ISMTAPL_TimeRequired = data.effortinhrs;
                                    adv.ISMTAPL_FromDate = Convert.ToDateTime(confromdate);
                                    adv.ISMTAPL_ToDate = Convert.ToDateTime(contodate);
                                    adv.ISMTAPL_Periodicity = data.periodicity.Trim();
                                    adv.ISMTAPL_TimeRequiredFlg = data.TimeRequiredFlg.Trim();
                                    adv.ISMTAPL_CreatedBy = data.UserId;
                                    adv.ISMTAPL_UpdatedBy = data.UserId;
                                    adv.ISMTAPL_ActiveFlg = true;
                                    adv.CreatedDate = DateTime.Now;
                                    adv.UpdatedDate = DateTime.Now;
                                    adv.ISMTAPL_OFFDate = data.Yearlydate;
                                    adv.ISMTAPL_ApprovalFlag = 1;
                                    adv.ISMTAPL_Day = Convert.ToInt64(data.TaskDay);
                                    _IssueContext.Add(adv);

                                    ISM_Task_Advance_Planner_DatesDMO datedmo = new ISM_Task_Advance_Planner_DatesDMO();
                                    datedmo.ISMTAPL_Id = adv.ISMTAPL_Id;
                                    datedmo.ISMTAPLD_StartDate = Convert.ToDateTime(confromdate);
                                    datedmo.ISMTAPLD_EndDate = Convert.ToDateTime(contodate);
                                    datedmo.ISMTAPLD_CreatedBy = data.UserId;
                                    datedmo.ISMTAPLD_UpdatedBy = data.UserId;
                                    datedmo.ISMTAPLD_ActiveFlg = true;
                                    datedmo.ISMTAPLD_CreatedDate = DateTime.Now;
                                    datedmo.ISMTAPLD_UpdatedDate = DateTime.Now;
                                    _IssueContext.Add(datedmo);
                                }

                                var i = _IssueContext.SaveChanges();

                                if (i > 0)
                                {
                                    data.returnval = true;

                                    var taskiddd = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_TaskNo == data.Auto_id).OrderByDescending(t => t.ISMTCR_Id).ToList();
                                    var assignedTolist = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                          where b.ISMTCR_Id == taskiddd.FirstOrDefault().ISMTCR_Id
                                                          select new ISM_ComplaintsTaskListDTO
                                                          {
                                                              HRME_Id = b.HRME_Id
                                                          }
                                                        ).Distinct().ToList(); ;

                                    if (assignedTolist.Count > 0)
                                    {
                                        foreach (var item in assignedTolist)
                                        {
                                            string dd = pb.savewebnotification(data.MI_Id, "TASKASSIGNEDWEB", item.ISMTCRASTO_Id, taskiddd.FirstOrDefault().ISMTCR_Id, item.HRME_Id, data.UserId, "Assigned");
                                        }
                                    }




                                    var devicelistto = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                        from e in _IssueContext.MasterEmployee
                                                        where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.HRME_Id == e.HRME_Id && e.HRME_AppDownloadedDeviceId != null
                                                        && e.HRME_AppDownloadedDeviceId != "")
                                                        select new ISM_ComplaintsTaskListDTO
                                                        {
                                                            HRME_Id = b.HRME_Id,
                                                            HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                        }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                    var devicelistfrom = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                          from e in _IssueContext.MasterEmployee
                                                          where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.ISMTCRASTO_AssignedBy == e.HRME_Id
                                                          && e.HRME_AppDownloadedDeviceId != null && e.HRME_AppDownloadedDeviceId != ""
                                                        )
                                                          select new ISM_ComplaintsTaskListDTO
                                                          {
                                                              HRME_Id = b.HRME_Id,
                                                              HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                                          }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                    if (devicelistto.Count > 0 && devicelistfrom.Count > 0)
                                    {
                                        var tasklist = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_TaskNo == data.Auto_id).OrderByDescending(t => t.ISMTCR_Id).ToList();
                                        if (devicelistto.FirstOrDefault().HRME_Id != devicelistfrom.FirstOrDefault().HRME_Id)
                                        {
                                            var deviceidsnew = "";
                                            var devicenew = "";
                                            if (devicelistto.Count > 0)
                                            {
                                                foreach (var deviceid in devicelistto)
                                                {
                                                    deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';

                                                    devicenew = "[" + deviceidsnew + "]";

                                                    try
                                                    {
                                                        string titletext = tasklist.FirstOrDefault().ISMTCR_TaskNo + " - " + tasklist.FirstOrDefault().ISMTCR_Title;
                                                        string subject = "New Task Assigned";
                                                        if (data.sound == "")
                                                        {
                                                            data.sound = "default";
                                                        }
                                                        callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                        Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Assigned");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }

                                            if (devicelistfrom.Count > 0)
                                            {
                                                foreach (var deviceid in devicelistfrom)
                                                {
                                                    deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                                    devicenew = "[" + deviceidsnew + "]";
                                                    try
                                                    {
                                                        string titletext = tasklist.FirstOrDefault().ISMTCR_TaskNo + " - " + tasklist.FirstOrDefault().ISMTCR_Title;
                                                        string subject = "New Task Created";
                                                        if (data.sound == "")
                                                        {
                                                            data.sound = "default";
                                                        }
                                                        callnotification(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                                        Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.UserId, "Assigned");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (te.HRME_Id != empid)
                                    {
                                        //var webpushtocken = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                        //                     from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                        //                     from a in _IssueContext.Staff_User_Login
                                        //                     where (b.ISMTCR_Id == taskasign.ISMTCR_Id && b.HRME_Id == a.Emp_Code && b.HRME_Id == te.HRME_Id && a.Emp_Code== te.HRME_Id)
                                        //                     select new ISM_ComplaintsTaskListDTO
                                        //                     {
                                        //                         UserId = e.ISMPNU_Userid
                                        //                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                        List<ISM_TaskCreationDTO> webpushtocken = new List<ISM_TaskCreationDTO>();
                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "ISM_GET_PUSHNOTIFICATION_USERS";
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add(new SqlParameter("@hrmeid",
                                          SqlDbType.BigInt)
                                            {
                                                Value = te.HRME_Id
                                            });
                                            cmd.Parameters.Add(new SqlParameter("@taskid",
                                            SqlDbType.BigInt)
                                            {
                                                Value = taskasign.ISMTCR_Id
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
                                                        webpushtocken.Add(new ISM_TaskCreationDTO
                                                        {
                                                            UserId = Convert.ToInt64(dataReader["UserId"])

                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        if (webpushtocken.Count > 0)
                                        {
                                            foreach (var webtockenid in webpushtocken)
                                            {
                                                PushNotification ObjPushNotification = new PushNotification(_Context);
                                                ObjPushNotification.callpushnotification(data.ISMTCR_Title, "New Task Assigned!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + data.Auto_id + "&taskid=" + taskasign.ISMTCR_Id, webtockenid.UserId);
                                            }
                                        }
                                    }
                                }
                            }
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

        public ISM_TaskCreationDTO SaveTaskDetailsDeptWise(ISM_TaskCreationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var empHRMEid = _Context.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
                long empId = 0;
                if (empHRMEid.Count > 0)
                {
                    var depart_id = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empHRMEid[0].Emp_Code).ToList();
                    var departmenthead = (from a in _IssueContext.HR_Master_Department
                                          from b in _IssueContext.HR_Master_DepartmentCode_Head
                                          from c in _IssueContext.Staff_User_Login
                                          where (a.HRMD_Id == depart_id[0].HRMD_Id && a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.Emp_Code && a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                          select new ISM_TaskCreationDTO
                                          {
                                              HRME_Id = b.HRME_ID,
                                              UserId = c.Id
                                          }).ToList();

                    if (data.Multideptrow.Length > 0)
                    {
                        for (int i = 0; i < data.Multideptrow.Length; i++)
                        {


                            ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                            task.ISMTCR_TaskNo = data.Auto_id;
                            task.MI_Id = data.MI_Id;
                            task.ISMMTGRP_Id = 0;
                            task.ISMTCR_TGOrder = 0;
                            task.HRMD_Id = data.Multideptrow[i].HRMD_Id;
                            task.ISMMPR_Id = data.Multideptrow[i].ISMMPR_Id;
                            task.IVRMM_Id = data.Multideptrow[i].IVRMM_Id;
                            task.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                            task.ISMTCR_CreationDate = indianTime;
                            task.ISMTCR_Title = data.ISMTCR_Title;
                            task.HRMPR_Id = data.Multideptrow[i].HRMPR_Id;
                            task.HRME_Id = empId = data.HRME_Id;
                            task.ISMTCR_Desc = data.ISMTCR_Desc;
                            task.ISMTCR_Status = data.ISMTCR_Status;
                            task.ISMMTCAT_Id = data.Multideptrow[i].ISMMTCAT_Id;
                            task.ISMMCLT_Id = data.Multideptrow[i].ISMMCLT_Id;
                            task.ISMTCR_ReOpenFlg = false;
                            task.ISMTCR_ActiveFlg = true;
                            task.ISMTCR_CreatedBy = data.UserId;
                            task.ISMTCR_UpdatedBy = data.UserId;
                            task.CreatedDate = indianTime;
                            task.UpdatedDate = indianTime;
                            _IssueContext.Add(task);

                            data.ISMTCR_Id = task.ISMTCR_Id;

                            if (data.ISMMCLT_Id > 0)
                            {
                                ISM_TaskCreation_ClientDMO taskclt = new ISM_TaskCreation_ClientDMO();
                                taskclt.ISMTCR_Id = data.ISMTCR_Id;
                                taskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                                taskclt.ISMTCRCL_ActiveFlg = true;
                                taskclt.ISMTCRCL_CreatedBy = data.UserId;
                                taskclt.ISMTCRCL_UpdatedBy = data.UserId;
                                taskclt.CreatedDate = indianTime;
                                taskclt.UpdatedDate = indianTime;
                                _IssueContext.Add(taskclt);
                            }
                        }
                    }
                    if (data.attachmentArray != null)
                    {
                        foreach (var file in data.attachmentArray)
                        {
                            if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                            {
                                ISM_TaskCreation_AttachmentDMO taskatt = new ISM_TaskCreation_AttachmentDMO();
                                taskatt.ISMTCR_Id = data.ISMTCR_Id;
                                taskatt.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                taskatt.ISMTCRAT_File = file.ISMTCRAT_File;
                                taskatt.ISMTCRAT_ActiveFlg = true;
                                taskatt.ISMTCRAT_CreatedBy = data.UserId;
                                taskatt.ISMTCRAT_UpdatedBy = data.UserId;
                                taskatt.CreatedDate = indianTime;
                                taskatt.UpdatedDate = indianTime;
                                _IssueContext.Add(taskatt);
                            }
                        }
                    }

                }
                else
                {
                    ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                    task.ISMTCR_TaskNo = data.Auto_id;
                    task.MI_Id = data.MI_Id;
                    task.ISMMTGRP_Id = 0;
                    task.ISMTCR_TGOrder = 0;
                    task.HRMD_Id = data.HRMD_Id;
                    task.ISMMPR_Id = data.ISMMPR_Id;
                    task.IVRMM_Id = data.IVRMM_Id;
                    task.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                    task.ISMTCR_CreationDate = indianTime;
                    task.ISMTCR_Title = data.ISMTCR_Title;
                    task.HRMPR_Id = data.HRMPR_Id;
                    task.HRME_Id = empId;
                    task.ISMTCR_Desc = data.ISMTCR_Desc;
                    task.ISMTCR_Status = data.ISMTCR_Status;
                    task.ISMMTCAT_Id = data.ISMMTCAT_Id;
                    task.ISMMCLT_Id = data.ISMMCLT_Id;
                    task.ISMTCR_ReOpenFlg = false;
                    task.ISMTCR_ActiveFlg = true;
                    task.ISMTCR_CreatedBy = data.UserId;
                    task.ISMTCR_UpdatedBy = data.UserId;
                    task.CreatedDate = indianTime;
                    task.UpdatedDate = indianTime;
                    _IssueContext.Add(task);

                    data.ISMTCR_Id = task.ISMTCR_Id;

                    if (data.ISMMCLT_Id > 0)
                    {
                        ISM_TaskCreation_ClientDMO taskclt = new ISM_TaskCreation_ClientDMO();
                        taskclt.ISMTCR_Id = data.ISMTCR_Id;
                        taskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                        taskclt.ISMTCRCL_ActiveFlg = true;
                        taskclt.ISMTCRCL_CreatedBy = data.UserId;
                        taskclt.ISMTCRCL_UpdatedBy = data.UserId;
                        taskclt.CreatedDate = indianTime;
                        taskclt.UpdatedDate = indianTime;
                        _IssueContext.Add(taskclt);
                    }

                    if (data.attachmentArray != null)
                    {
                        foreach (var file in data.attachmentArray)
                        {
                            if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                            {
                                ISM_TaskCreation_AttachmentDMO taskatt = new ISM_TaskCreation_AttachmentDMO();
                                taskatt.ISMTCR_Id = data.ISMTCR_Id;
                                taskatt.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                taskatt.ISMTCRAT_File = file.ISMTCRAT_File;
                                taskatt.ISMTCRAT_ActiveFlg = true;
                                taskatt.ISMTCRAT_CreatedBy = data.UserId;
                                taskatt.ISMTCRAT_UpdatedBy = data.UserId;
                                taskatt.CreatedDate = indianTime;
                                taskatt.UpdatedDate = indianTime;
                                _IssueContext.Add(taskatt);
                            }
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

    }
}