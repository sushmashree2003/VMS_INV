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
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_ConsolidateDailyReportImpl : Interfaces.ISM_ConsolidateDailyReportInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_ConsolidateDailyReportImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
        public async Task<ISM_DailyReportGenerationDTO> getdetails(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                long? dept_id = 0;
                long? design_id = 0;
                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

                    data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                }

                var hrmeDepartment = _IssueContext.HR_Master_Department.Where(t =>  t.HRMD_Id == dept_id && t.HRMD_ActiveFlag == true).Distinct().ToList();
                if (hrmeDepartment.Count > 0)
                {
                    data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                }

                var checkdepthead = _IssueContext.HR_Master_DepartmentCode_Head.Where(a => a.HRME_ID == empid).ToList();

                if (checkdepthead.Count > 0)
                {
                    roletype = "Admin";
                    data.roleflag = false;
                }


                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_admin";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
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
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Department List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt)
                    {
                        Value = dept_id
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
                    cmd.CommandText = "DesignationList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
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
                        data.Designation = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Module List ********************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DEPARTMENTWISE_MODULE_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
                    });

                    cmd.Parameters.Add(new SqlParameter("@DEPT_Id", SqlDbType.VarChar)
                    {
                        Value = ""
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
                        data.moduledropdown = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Status List ********************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DEPARTMENTWISE_STATUS_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
                    });

                    cmd.Parameters.Add(new SqlParameter("@DEPT_Id", SqlDbType.VarChar)
                    {
                        Value = ""
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
                        data.get_Status = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.clientdropdown = _IssueContext.clientTable.Where(a => a.ISMMCLT_ActiveFlag == true).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_DailyReportGenerationDTO> getReport(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                string HRME_Id = "0";
                if(data.emplist !=null && data.emplist.Length >0)
                {
                    foreach (var c in data.emplist)
                    {
                        HRME_Id = HRME_Id + "," + c.HRME_Id;
                    }
                }
                

                string HRMDC_ID = "0";
                if(data.deptlist !=null  && data.deptlist.Length >0)
                {
                    foreach (var c in data.deptlist)
                    {
                        HRMDC_ID = HRMDC_ID + "," + c.HRMDC_ID;
                    }

                }

                string HRMDES_Id = "0";
                if (data.desglist !=null && data.desglist.Length >0)
                {
                    foreach (var c in data.desglist)
                    {
                        HRMDES_Id = HRMDES_Id + "," + c.HRMDES_Id;
                    }
                }
                

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                if (data.reporttype != "consolidatereport")
                {
                    try
                    {
                        fromdatecon = Convert.ToDateTime(data.start_date.Value.ToString("yyyy-MM-dd"));
                        confromdate = fromdatecon.ToString("yyyy-MM-dd");

                        toatecon = Convert.ToDateTime(data.end_date.Value.ToString("yyyy-MM-dd"));
                        contodate = toatecon.ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (data.reporttype == "consolidatereport")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_ConsolidateDailyReport_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)
                        {
                            Value = data.start_date
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)
                        {
                            Value = data.end_date
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
                            data.get_report = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if(data.reporttype== "overallcommentsreport")
                {                   
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Detailed_DailyReport_Comments_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar){Value = confromdate});
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar){Value = contodate});
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar){Value = HRME_Id});
                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar){Value = HRMDES_Id});
                        cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.VarChar){Value = HRMDC_ID});
                        cmd.Parameters.Add(new SqlParameter("@StatusFlag", SqlDbType.VarChar){Value = data.StatusFlg});
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
                            data.get_report = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if(data.reporttype == "MultiLevelDRApproval")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_DailyReport_MultiLevel_Approval_Details_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar) { Value = contodate });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar) { Value = HRMDES_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.VarChar) { Value = HRMDC_ID });
                        cmd.Parameters.Add(new SqlParameter("@StatusFlag", SqlDbType.VarChar) { Value = "All" });
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
                            data.get_report = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if(data.reporttype == "DailyReportBlockedCountDetails")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_DailyReport_Blocked_Count_Details_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar) { Value = contodate });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar) { Value = HRMDES_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.VarChar) { Value = HRMDC_ID });
                        cmd.Parameters.Add(new SqlParameter("@StatusFlag", SqlDbType.VarChar) { Value = "All" });
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
                            data.get_report = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if(data.reporttype == "DailyReportApproval")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Detailed_DailyReport_Head";                            
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar) { Value = contodate });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });                      
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

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        //cmd.CommandText = "ISMEmpwiseDrDaywiseAppEfforts_Report_Modify";
                        cmd.CommandText = "ISM_Detailed_DailyReport_Head_EMP";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar) { Value = contodate });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id }); ;
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
                            data.get_report_effort = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Detailed_Consolidated_DailyReport_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar){Value = confromdate});
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar){Value = contodate});
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar){Value = HRME_Id});
                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar){Value = HRMDES_Id});
                        cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.VarChar){Value = HRMDC_ID});
                        cmd.Parameters.Add(new SqlParameter("@StatusFlag", SqlDbType.VarChar){Value = data.StatusFlg});
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
                                        dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
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

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISMEmpwiseDrDaywiseAppEfforts_Report_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar){Value = confromdate});
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar){Value = contodate});
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar){Value = HRME_Id});
                        cmd.Parameters.Add(new SqlParameter("@StatusFlag", SqlDbType.VarChar) { Value = data.StatusFlg });
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
                            data.get_report_effort = retObject.ToArray();
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
        public ISM_DailyReportGenerationDTO ViewCheckListSavedDetails(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                data.getchecklistarray = (from a in _IssueContext.ISM_DailyReport_FilesDMO
                                          from b in _IssueContext.ISM_DailyReportGenerationDMO
                                          from c in _IssueContext.ISM_Master_TaskCategory_ChecklistDMO_con
                                          where (a.ISMDRPT_Id == b.ISMDRPT_Id && a.ISMMTCATCL_Id == c.ISMMTCATCL_Id && b.MI_Id == data.MI_Id
                                          && a.ISMDRPT_Id == data.ISMDRPT_Id)
                                          select new ISM_DailyReportGenerationDTO
                                          {
                                              ISMDRPTFL_FileName = a.ISMDRPTFL_FileName,
                                              ISMDRPTFL_FilePath = a.ISMDRPTFL_FilePath,
                                              ISMDRPTFL_RefNo = a.ISMDRPTFL_RefNo,
                                              ISMDRPTFL_Remarks = a.ISMDRPTFL_Remarks,
                                              ISMMTCATCL_Id = a.ISMMTCATCL_Id,
                                              ISMDRPT_Id = a.ISMDRPT_Id,
                                              ISMMTCATCL_CheckListName = c.ISMMTCATCL_CheckListName
                                          }).Distinct().ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_DailyReportGenerationDTO viewresponse(ISM_DailyReportGenerationDTO data)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_DailyReportGenerationDTO> getfeedbackReport(ISM_DailyReportGenerationDTO data)
        {
            try
            {

                string HRME_Id = "0";
                foreach (var c in data.emplist)
                {
                    HRME_Id = HRME_Id + "," + c.HRME_Id;
                }

                string HRMDC_ID = "0";
                foreach (var c in data.deptlist)
                {
                    HRMDC_ID = HRMDC_ID + "," + c.HRMDC_ID;
                }

                string HRMDES_Id = "0";
                foreach (var c in data.desglist)
                {
                    HRMDES_Id = HRMDES_Id + "," + c.HRMDES_Id;
                }

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";
                //fromdatecon = Convert.ToDateTime("2020-02-17");
                try
                {
                    fromdatecon = Convert.ToDateTime(data.start_date.Value.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    toatecon = Convert.ToDateTime(data.end_date.Value.ToString("yyyy-MM-dd"));
                    contodate = toatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_FEEDBACK_DR_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@START_DATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@END_DATE", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                    {
                        Value = HRMDES_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.VarChar)
                    {
                        Value = HRMDC_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = "CONSOLIDATE"
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
                        data.get_report = retObject.ToArray();
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
        public async Task<ISM_DailyReportGenerationDTO> viewcomments(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                var hrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == data.UserId && a.MI_Id == data.MI_Id).ToList();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyFeedback_View_Comments";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id", SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Dept_Head_Id", SqlDbType.BigInt)
                    {
                        Value = data.Dept_Head_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@DR_DATE", SqlDbType.VarChar)
                    {
                        Value = data.DR_DATE
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
                        data.get_feedback = retObject.ToArray();
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
            return data;
        }
        public ISM_DailyReportGenerationDTO getmodulelist(ISM_DailyReportGenerationDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }

                string departments = "0";
                if (data.Department_tempDTO.Length > 0)
                {
                    foreach (var ue in data.Department_tempDTO)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DEPARTMENTWISE_MODULE_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "2"
                    });

                    cmd.Parameters.Add(new SqlParameter("@DEPT_Id", SqlDbType.VarChar)
                    {
                        Value = departments
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
                        data.moduledropdown = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Status List ********************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DEPARTMENTWISE_STATUS_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "2"
                    });

                    cmd.Parameters.Add(new SqlParameter("@DEPT_Id", SqlDbType.VarChar)
                    {
                        Value = departments
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
                        data.get_Status = retObject.ToArray();
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
        public ISM_DailyReportGenerationDTO getcomplinetstaskreport(ISM_DailyReportGenerationDTO data)
        {
            try
            {

                string HRME_Id = "0";
                foreach (var c in data.emplist)
                {
                    HRME_Id = HRME_Id + "," + c.HRME_Id;
                }

                string HRMDC_ID = "0";
                foreach (var c in data.deptlist)
                {
                    HRMDC_ID = HRMDC_ID + "," + c.HRMDC_ID;
                }

                string HRMDES_Id = "0";
                foreach (var c in data.desglist)
                {
                    HRMDES_Id = HRMDES_Id + "," + c.HRMDES_Id;
                }

                string Moduleid = "0";
                foreach (var c in data.moduletemp_dto)
                {
                    Moduleid = Moduleid + "," + c.IVRMM_Id;
                }

                string statusid = "0";
                foreach (var c in data.statustemp_dto)
                {
                    statusid = statusid + "," + c.ISMMISTS_StatusName;
                }

                string clientid = "0";
                foreach (var c in data.clienttemp_dto)
                {
                    clientid = clientid + "," + c.ISMMCLT_Id;
                }

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";
                //fromdatecon = Convert.ToDateTime("2020-02-17");
                try
                {
                    fromdatecon = Convert.ToDateTime(data.start_date.Value.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    toatecon = Convert.ToDateTime(data.end_date.Value.ToString("yyyy-MM-dd"));
                    contodate = toatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Detailed_Compliant_Task_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                    {
                        Value = HRMDES_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.VarChar)
                    {
                        Value = HRMDC_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Module_Id", SqlDbType.VarChar)
                    {
                        Value = Moduleid
                    });

                    cmd.Parameters.Add(new SqlParameter("@Client_Id", SqlDbType.VarChar)
                    {
                        Value = clientid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status_Id", SqlDbType.VarChar)
                    {
                        Value = statusid
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
                        data.get_report = retObject.ToArray();
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
        public ISM_DailyReportGenerationDTO getyerlyplannerreport(ISM_DailyReportGenerationDTO data)
        {
            try
            {

                string HRME_Id = "0";
                foreach (var c in data.emplist)
                {
                    HRME_Id = HRME_Id + "," + c.HRME_Id;
                }

                string HRMDC_ID = "0";
                foreach (var c in data.deptlist)
                {
                    HRMDC_ID = HRMDC_ID + "," + c.HRMDC_ID;
                }

                string HRMDES_Id = "0";
                foreach (var c in data.desglist)
                {
                    HRMDES_Id = HRMDES_Id + "," + c.HRMDES_Id;
                }

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";                 
                try
                {
                    fromdatecon = Convert.ToDateTime(data.start_date.Value.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    toatecon = Convert.ToDateTime(data.end_date.Value.ToString("yyyy-MM-dd"));
                    contodate = toatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Detailed_YearlyPlanner_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                    {
                        Value = HRMDES_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.VarChar)
                    {
                        Value = HRMDC_ID
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader =  cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
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
                        data.get_report = retObject.ToArray();
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
    }
}
