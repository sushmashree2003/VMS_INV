using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_MemoNoticeReportImpl : Interfaces.ISM_MemoNoticeReportInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_MemoNoticeReportImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_EMPLOYEE_MEMONOTICEDTO> getdetails(ISM_EMPLOYEE_MEMONOTICEDTO data)
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

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_ID && t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                long? dept_id = 0;
                long? design_id = 0;
                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_ID && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

                    data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                }

                var hrmeDepartment = _IssueContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_ID && t.HRMD_Id == dept_id && t.HRMD_ActiveFlag == true).Distinct().ToList();
                if (hrmeDepartment.Count > 0)
                {
                    data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                }
                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_admin";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",SqlDbType.BigInt)
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
                //using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "ISM_DepartmentList";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //    {
                //        Value = data.MI_ID
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                //    {
                //        Value = roletype
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt)
                //    {
                //        Value = dept_id
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
                //                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.DepartmentList = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Rating_Departments";
                    cmd.CommandType = CommandType.StoredProcedure;
                   
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
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",SqlDbType.BigInt)
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

                //viewdetails //arrayuserEmp

                data.viewdetails = _Context.IVRM_Master_FinancialYear.Distinct().ToArray();
                data.arrayuserEmp = _Context.month.Distinct().ToArray();

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "ISm_Sales_ProductList";
                    cmd.CommandText = "ISm_Sales_ProductListRating";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
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
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.memonoticereport = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //empdRawReports

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "ISm_Sales_ProductList_New";
                    cmd.CommandText = "ISM_CA_DemoEmp_Rating_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = data.MI_ID
                    //});
                    //cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    //{
                    //    Value = data.UserId
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.empdRawReports = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_EMPLOYEE_MEMONOTICEDTO getMemoNoticeReport(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            try
            {
                if(data.Type=="Task_Notice")
                {
                    string HRME_Id = "0";
                    if (data.employeelist !=null  && data.employeelist.Length >0)
                    {
                        foreach(var d in data.employeelist)
                        {
                            HRME_Id = HRME_Id + "," + d;
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_EmpOpenTask_Status";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fropmdate", SqlDbType.Date)
                        {
                            Value = data.startdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.Date)
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
                            data.memonoticereport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
               else if(data.Type == "Both")
                {
                    data.memonoticereport = (from a in _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE                                             
                                             from c in _IssueContext.MasterEmployee
                                             where (a.HRME_ID == c.HRME_Id && data.employeelist.Contains(c.HRME_Id) && a.ISMEMN_Date >= data.startdate && a.ISMEMN_Date <= data.enddate)
                                             //&& data.departmentlist.Contains(d.HRMDC_ID) && data.designationlist.Contains(c.HRMDES_Id)
                                             select new ISM_EMPLOYEE_MEMONOTICEDTO
                                             {
                                                 ISMEMN_ID = a.ISMEMN_ID,
                                                 HRME_ID = a.HRME_ID,
                                                 empname = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                                 ISMEMN_No = a.ISMEMN_No,
                                                 ISMEMN_Date = a.ISMEMN_Date,
                                                 ISMEMN_Type = a.ISMEMN_Type,
                                                 ISMEMN_Description = a.ISMEMN_Description,
                                                 ISMEMN_CompleByDate = a.ISMEMN_CompleByDate
                                             }).OrderByDescending(t=>t.ISMEMN_CompleByDate).ToArray();
                }
                else
                {
                    data.memonoticereport = (from a in _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE                                             
                                             from c in _IssueContext.MasterEmployee
                                             where (a.HRME_ID == c.HRME_Id && data.employeelist.Contains(c.HRME_Id) && a.ISMEMN_Type == data.Type && a.ISMEMN_Date >= data.startdate && a.ISMEMN_Date <= data.enddate)
                                             //&& data.departmentlist.Contains(d.HRMDC_ID) && data.designationlist.Contains(c.HRMDES_Id)
                                             select new ISM_EMPLOYEE_MEMONOTICEDTO
                                             {
                                                 ISMEMN_ID = a.ISMEMN_ID,
                                                 HRME_ID = a.HRME_ID,
                                                 empname = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                                 ISMEMN_No = a.ISMEMN_No,
                                                 ISMEMN_Date = a.ISMEMN_Date,
                                                 ISMEMN_Type = a.ISMEMN_Type,
                                                 ISMEMN_Description = a.ISMEMN_Description,
                                                 ISMEMN_CompleByDate = a.ISMEMN_CompleByDate
                                             }).OrderByDescending(t => t.ISMEMN_CompleByDate).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_EMPLOYEE_MEMONOTICEDTO gettaskdetails(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            try
            {
                data.viewdetails = (from b in _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE_TASKS
                                    from c in _IssueContext.ISM_Task_Planner_TasksDMO
                                    from d in _IssueContext.ISM_TaskCreationDMO
                                    where (b.ISMTPLTA_Id == data.ISMTPLTA_Id && b.ISMTPLTA_Id == c.ISMTPLTA_Id && c.ISMTCR_Id == d.ISMTCR_Id)
                                         select new ISM_EMPLOYEE_MEMONOTICEDTO
                                         {
                                             ISMTPLTA_Id = b.ISMTPLTA_Id,
                                             ISMTCR_Id = c.ISMTCR_Id,
                                             ISMTCR_Title = d.ISMTCR_Title,
                                             ISMTCR_Desc = d.ISMTCR_Desc,
                                             ISMTCR_Status = d.ISMTCR_Status,
                                             ISMTCR_TaskNo = d.ISMTCR_TaskNo
                                         }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<ISM_EMPLOYEE_MEMONOTICEDTO> getdetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data)
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

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_ID && t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                long? dept_id = 0;
                long? design_id = 0;
                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_ID && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

                    data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                }

                var hrmeDepartment = _IssueContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_ID && t.HRMD_Id == dept_id && t.HRMD_ActiveFlag == true).Distinct().ToList();
                if (hrmeDepartment.Count > 0)
                {
                    data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                }
                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_admin_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
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
                    cmd.CommandText = "ISM_DepartmentList_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
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
                    cmd.CommandText = "DesignationList_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
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

                //viewdetails //arrayuserEmp

                data.viewdetails = _Context.IVRM_Master_FinancialYear.Distinct().ToArray();
                data.arrayuserEmp = _Context.month.Distinct().ToArray();
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ISM_EMPLOYEE_MEMONOTICEDTO> DeptselectiondetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            try
            {

                var hrmdc_ids = "0";
                if (data.departmentlist.Length > 0 && data.departmentlist != null)
                {
                    foreach (var item in data.departmentlist)
                    {
                        hrmdc_ids = hrmdc_ids + "," + item;
                    }

                }
                //var departmentlistIds = data.departmentlistNew;
                ////var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                ////var roletype = "";
                ////data.roleflag = true;
                ////if (rolet.Count > 0)
                ////{
                ////    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                ////    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                ////    {
                ////        data.roleflag = false;
                ////    }
                ////}
                ////else
                ////{
                ////    roletype = "";
                ////    data.roleflag = true;
                ////}

                ////var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_ID && t.Id == data.UserId).Distinct().ToList();
                ////long empid = 0;
                ////if (hrmEempid.Count > 0)
                ////{
                ////    empid = hrmEempid.FirstOrDefault().Emp_Code;
                ////}
                ////long? dept_id = 0;
                ////long? design_id = 0;
                ////var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_ID && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                ////if (hrmeDepartment_id.Count > 0)
                ////{
                ////    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                ////    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

                ////    data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                ////}

                ////var hrmeDepartment = _IssueContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_ID && t.HRMD_Id == dept_id && t.HRMD_ActiveFlag == true).Distinct().ToList();
                ////if (hrmeDepartment.Count > 0)
                ////{
                ////    data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                ////}


                //*********************************** User Designation List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DesignationList_DemoRating";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                    {
                        Value = hrmdc_ids
                    });
                    //cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    //{
                    //    Value = roletype
                    //});
                    //cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                    //{
                    //    Value = data.UserId
                    //});
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
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "ISm_Sales_ProductList_New";
                    cmd.CommandText = "ISM_CA_DemoEmp_Rating_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = data.MI_ID
                    //});
                    //cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    //{
                    //    Value = data.UserId
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.empdRawReports = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                ////viewdetails //arrayuserEmp

                ////data.viewdetails = _Context.IVRM_Master_FinancialYear.Distinct().ToArray();
                ////data.arrayuserEmp = _Context.month.Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ISM_EMPLOYEE_MEMONOTICEDTO> DesgselectiondetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            try
            {

                var hrmd_ids = "0";
                if (data.designationlist.Length > 0 && data.designationlist != null)
                {
                    foreach (var item in data.designationlist)
                    {
                        hrmd_ids = hrmd_ids + "," + item;
                    }

                }
                //*********************************** User Designation List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_DemoRating";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                    {
                        Value = hrmd_ids
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
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISM_CA_DemoEmp_Rating_Report";
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.empdRawReports = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
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
