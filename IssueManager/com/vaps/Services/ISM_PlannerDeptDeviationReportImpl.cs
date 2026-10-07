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
    public class ISM_PlannerDeptDeviationReportImpl : Interfaces.ISM_PlannerDeptDeviationReportInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_PlannerDeptDeviationReportImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_PlannerReportsDTO> getdetails(ISM_PlannerReportsDTO data)
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

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                long? dept_id = 0;
                long? design_id = 0;
                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

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
                    cmd.CommandText = "ISM_User_EmployeeList_admin";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
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
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
SqlDbType.BigInt)
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
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
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
                        data.Designation = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** DEVIATION % ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DEVIATION_PERCENTAGE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.Deviationpercentage = retObject.ToArray();
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
        public async Task<ISM_PlannerReportsDTO> getdetails_weekly(ISM_PlannerReportsDTO data)
        {
            try
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

                    cmd.Parameters.Add(new SqlParameter("@user_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
        public async Task<ISM_PlannerReportsDTO> getdeviationReport(ISM_PlannerReportsDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                string hrmeids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrmeids = hrmeids + "," + ue.HRME_Id;
                    }
                }
                else
                {
                    if (hrmeids == "" || hrmeids == "0")
                    {
                        hrmeids = empid.ToString();
                    }
                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//
                if (data.deviationtype == "Remarks")
                {
                    string statuslist = "";
                    if (data.arraydeviationStatus.Length > 0)
                    {
                        foreach (var sts in data.arraydeviationStatus)
                        {
                            statuslist = statuslist + "," + sts.deviationStatus;
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Deviation_Remarkswise_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.VarChar)
                        {
                            Value = hrmeids
                        });
                        cmd.Parameters.Add(new SqlParameter("@status",
                        SqlDbType.VarChar)
                        {
                            Value = statuslist
                        });
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
                        cmd.Parameters.Add(new SqlParameter("@NoOfDays",
                        SqlDbType.VarChar)
                        {
                            Value = data.NoOfDays
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
                            data.get_deviationRemarksReport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    if (data.Deviationreporttype == "Dept")
                    {
                        var HRMDIDS = new List<HR_Master_Department>();
                        string departments = "0";
                        //if (data.departmentlist.Length > 0)
                        //{
                        //    foreach (var ue in data.departmentlist)
                        //    {
                        //        departments = departments + ",'" + ue.HRMD_DepartmentCode + "'";
                        //    }

                        //    departments = "'" + departments;
                        //}
                        if (data.departmentlist.Length > 0)
                        {
                            foreach (var ue in data.departmentlist)
                            {
                                var HRMDIDSS = _IssueContext.HR_Master_Department.Where(t => t.HRMDC_ID == ue.HRMDC_ID).ToList();
                                if (HRMDIDSS.Count() > 0)
                                {
                                    foreach (var uee in HRMDIDSS)
                                    {
                                        HRMDIDS.Add(uee);
                                    }
                                }
                                //departments = departments + ",'" + ue.HRMD_DepartmentCode + "'";
                            }
                            if (HRMDIDS.Count() > 0)
                            {
                                foreach (var ue in HRMDIDS)
                                {
                                    departments = departments + "," + ue.HRMD_Id;
                                }
                            }
                        }



                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "departmentWise_Dev_Report";
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
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                          SqlDbType.VarChar)
                            {
                                Value = hrmeids
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                          SqlDbType.VarChar)
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
                                data.Dept_deviationRemarksReport = retObject.ToArray();
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
                            cmd.CommandText = "ISM_DeviationReport_Calculation_Dept";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
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
                                Value = data.startdate
                            });
                            cmd.Parameters.Add(new SqlParameter("@EndDate",
                            SqlDbType.DateTime)
                            {
                                Value = data.enddate
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
                                data.get_deviationreport = retObject.ToArray();
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
                Console.WriteLine(ex.Message);
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
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
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
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
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

        public async Task<ISM_PlannerReportsDTO> DeptselectionHeaddetails(ISM_PlannerReportsDTO data)
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
                    cmd.CommandText = "ISM_DepartmentHeadList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
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
                        data.departmnthead = retObject.ToArray();
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
        public async Task<ISM_PlannerReportsDTO> empcommunication(ISM_PlannerReportsDTO data)
        {
            try
            {
                string TransNo = "";
                string taskdetails = "";
                if (data.EmplistforMNNE.Length > 0)
                {
                    foreach (var ue in data.EmplistforMNNE)
                    {
                        var Employee = (from a in _IssueContext.MasterEmployee
                                        from b in _Context.Multiple_Email_DMO
                                        where a.HRME_Id == b.HRME_Id && a.HRME_Id == ue.HRME_Id && b.HRME_Id == ue.HRME_Id
                                        select new ISM_PlannerReportsDTO
                                        {
                                            HRMEM_EmailId = b.HRMEM_EmailId,
                                            HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                            HRMD_Id = Convert.ToInt64(a.HRMD_Id),
                                            MI_Id = a.MI_Id,
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim()

                                        }).ToList();
                        if (Employee.Count > 0)
                        {
                            if (data.EmplistforMNNE.Length > 0)
                            {
                                decimal deviationper = 0;
                                foreach (var plan in data.EmplistforMNNE)
                                {
                                    if (plan.plannerdetails != null && plan.plannerdetails.Length > 1)
                                    {
                                        foreach (var planD in plan.plannerdetails)
                                        {
                                            deviationper = deviationper + Convert.ToDecimal(planD.Deviation_Percentage);
                                        }
                                        deviationper = (deviationper / plan.plannerdetails.Length);
                                    }
                                    else if (plan.plannerdetails != null && plan.plannerdetails.Length == 0)
                                    {
                                        deviationper = Convert.ToDecimal(plan.plannerdetails.FirstOrDefault().Deviation_Percentage);
                                    }
                                    data.deviation = deviationper.ToString();
                                }
                            }
                            data.MI_Id = Employee.FirstOrDefault().MI_Id;
                            if (ue.comp_Date == null)
                            {
                                data.comp_Date = DateTime.Now.AddDays(3);
                            }
                            else
                            {
                                data.comp_Date = ue.comp_Date;
                            }
                            data.ASMAY_Id = _IssueContext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                            data.HRME_Id = ue.HRME_Id;
                            data.employeename = Employee.FirstOrDefault().employeename;
                            data.HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                            if (Employee.FirstOrDefault().HRMEM_EmailId != null && Employee.FirstOrDefault().HRMEM_EmailId != "")
                            {
                                var EmployeeEmail = Employee.FirstOrDefault().HRMEM_EmailId;
                                data.EmployeeEmail = EmployeeEmail;
                                //data.EmployeeEmail = "rakesh.reddy@vapstech.com";
                                //EmployeeEmail = data.EmployeeEmail;

                                if (data.communicationfalg == "Memo")
                                {
                                    TransNo = gettransactionno(data);
                                    taskdetails = gettaskregisterReport(data);
                                    data.message = sendmail(data.MI_Id, EmployeeEmail, "MEMO", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, data.communicationfalg, TransNo, data.communicationmsg, taskdetails, data, data.comp_Date);
                                }
                                else if (data.communicationfalg == "Notice")
                                {
                                    data.startdate = ue.start_Date;
                                    data.enddate = ue.end_Date;
                                    data.ISMEMN_No = ue.ISMEMN_No;
                                    data.ISMEMN_Date = ue.ISMEMN_Date;
                                    data.ISMEMN_CompleByDate = ue.ISMEMN_CompleByDate;

                                    TransNo = gettransactionno(data);
                                    taskdetails = gettaskregisterReportnotice(data);
                                    data.message = sendmail(data.MI_Id, EmployeeEmail, "NOTICE", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, data.communicationfalg, TransNo, data.communicationmsg, taskdetails, data, data.comp_Date);
                                }
                                else if (data.communicationfalg == "Email")
                                {
                                    TransNo = gettransactionno(data);
                                    data.message = sendmailcmt(data.MI_Id, EmployeeEmail, "DEVIATIONCMNT", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, data.communicationfalg, TransNo, data.communicationmsg);
                                }
                            }
                            if (Employee.FirstOrDefault().HRME_AppDownloadedDeviceId != null && Employee.FirstOrDefault().HRME_AppDownloadedDeviceId != "")
                            {
                                var EmployeeDeviceid = Employee.FirstOrDefault().HRME_AppDownloadedDeviceId;
                                if (data.communicationfalg == "Notif")
                                {
                                    int e = callnotification(data.HRME_Id, EmployeeDeviceid, data.MI_Id, "Deviation-Comments", data.communicationmsg, data.communicationfalg, data.UserId);
                                    if (e == 1)
                                    {
                                        data.message = "success";
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
        public async Task<ISM_PlannerReportsDTO> apiempcommunication(ISM_PlannerReportsDTO data)
        {
            try
            {
                ISM_PlannerReportsDTO dates = new ISM_PlannerReportsDTO();
                dates = startenddays(data);
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                DayOfWeek today = indiantime0.DayOfWeek;

                data.startdate =Convert.ToDateTime(dates.startdate).AddDays(-8);
                data.enddate = Convert.ToDateTime(data.startdate).AddDays(7);

                data.communicationfalg = "Memo";
                data.Deviationreporttype = "Emp";

                data.type = true;
                if (dates.message == today.ToString())
                {
                    var acd_Id = _IssueContext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                    data.ASMAY_Id = acd_Id;

                    var hrmEempid = (from t in _IssueContext.MasterEmployee
                                     where t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false
                                     select new ISM_PlannerReportsDTO
                                     {
                                         HRME_Id = t.HRME_Id
                                     }).ToList();

                    string hrmeids = "0";
                    if (hrmEempid.Count() > 0)
                    {

                        foreach (var ue in hrmEempid)
                        {
                            hrmeids = hrmeids + "," + ue.HRME_Id;
                        }
                        List<EmplistforMNNEDTO> empdevlist = new List<EmplistforMNNEDTO>();
                        List<plannerdetails> empplanners = new List<plannerdetails>();
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_DeviationReport_Calculation_Dept";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
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
                                Value = data.startdate
                            });
                            cmd.Parameters.Add(new SqlParameter("@EndDate",
                            SqlDbType.DateTime)
                            {
                                Value = data.enddate
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

                                        empdevlist.Add(new EmplistforMNNEDTO
                                        {
                                            HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                            Deviation_Percentage = Convert.ToDecimal(dataReader["Deviation_Percentage"])
                                        });

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        string TransNo = "";
                        string taskdetails = "";
                        if (empdevlist.Count() > 0)
                        {
                            foreach (var ue in empdevlist)
                            {
                                if (ue.Deviation_Percentage >= 20)
                                {
                                    var findhead = (from t in _IssueContext.MasterEmployee
                                                    where t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.HRME_Id == ue.HRME_Id
                                                    select new ISM_PlannerReportsDTO
                                                    {
                                                        HRMD_Id = Convert.ToInt64(t.HRMD_Id)
                                                    }).ToList();

                                    var EmployeeHead = (from a in _IssueContext.MasterEmployee
                                                        from b in _Context.HR_Master_Department
                                                        from c in _Context.HR_Master_DepartmentCodeDMO
                                                        from d in _Context.HR_Master_DepartmentCode_HeadDMO
                                                        from f in _Context.Staff_User_Login
                                                        where a.HRME_Id == d.HRME_ID && a.HRMD_Id == b.HRMD_Id && b.HRMDC_ID == c.HRMDC_ID && c.HRMDC_ID == d.HRMDC_ID && a.HRME_Id == f.Emp_Code && a.HRMD_Id == findhead.FirstOrDefault().HRMD_Id
                                                        select new ISM_PlannerReportsDTO
                                                        {
                                                            hrmehead = f.Id
                                                        }).ToList();
                                    if (EmployeeHead.Count() > 0)
                                    {
                                        data.UserId = Convert.ToInt64(EmployeeHead.FirstOrDefault().hrmehead);
                                    }

                                    var Employee = (from a in _IssueContext.MasterEmployee
                                                    from b in _Context.Multiple_Email_DMO
                                                    where a.HRME_Id == b.HRME_Id && a.HRME_Id == ue.HRME_Id && b.HRME_Id == ue.HRME_Id
                                                    select new ISM_PlannerReportsDTO
                                                    {
                                                        HRMEM_EmailId = b.HRMEM_EmailId,
                                                        HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                        HRMD_Id = Convert.ToInt64(a.HRMD_Id),
                                                        MI_Id = a.MI_Id,
                                                        employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim()

                                                    }).ToList();
                                    if (Employee.Count > 0)
                                    {
                                        if (empdevlist.Count() > 0)
                                        {
                                            decimal deviationper = 0;
                                            foreach (var plan in empdevlist)
                                            {
                                                if (plan.Deviation_Percentage != null && plan.Deviation_Percentage > 1)
                                                {                                                  
                                                    deviationper = deviationper + Convert.ToDecimal(plan.Deviation_Percentage);
                                                }
                                                data.deviation = deviationper.ToString();
                                            }
                                        }
                                        data.MI_Id = Employee.FirstOrDefault().MI_Id;

                                        var compdays = completebydays(1, "Memo");
                                        data.comp_Date = DateTime.Now.AddDays(compdays);
                                       
                                        data.ASMAY_Id = _IssueContext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                                        data.HRME_Id = ue.HRME_Id;
                                        data.employeename = Employee.FirstOrDefault().employeename;
                                        data.HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                                        if (Employee.FirstOrDefault().HRMEM_EmailId != null && Employee.FirstOrDefault().HRMEM_EmailId != "")
                                        {
                                            var EmployeeEmail = Employee.FirstOrDefault().HRMEM_EmailId;
                                            data.EmployeeEmail = EmployeeEmail;
                                            //data.EmployeeEmail = "rakesh.reddy@vapstech.com";
                                            //EmployeeEmail = data.EmployeeEmail;

                                            if (data.communicationfalg == "Memo")
                                            {
                                                TransNo = gettransactionno(data);
                                                taskdetails = gettaskregisterReport(data);
                                                data.message = sendmail(data.MI_Id, EmployeeEmail, "MEMO", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, data.communicationfalg, TransNo, data.communicationmsg, taskdetails, data, data.comp_Date);
                                            }
                                            else if (data.communicationfalg == "Notice")
                                            {
                                                data.startdate = ue.start_Date;
                                                data.enddate = ue.end_Date;
                                                data.ISMEMN_No = ue.ISMEMN_No;
                                                data.ISMEMN_Date = ue.ISMEMN_Date;
                                                data.ISMEMN_CompleByDate = ue.ISMEMN_CompleByDate;

                                                TransNo = gettransactionno(data);
                                                taskdetails = gettaskregisterReportnotice(data);
                                                data.message = sendmail(data.MI_Id, EmployeeEmail, "NOTICE", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, data.communicationfalg, TransNo, data.communicationmsg, taskdetails, data, data.comp_Date);
                                            }
                                            else if (data.communicationfalg == "Email")
                                            {
                                                TransNo = gettransactionno(data);
                                                data.message = sendmailcmt(data.MI_Id, EmployeeEmail, "DEVIATIONCMNT", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, data.communicationfalg, TransNo, data.communicationmsg);
                                            }
                                        }
                                        if (Employee.FirstOrDefault().HRME_AppDownloadedDeviceId != null && Employee.FirstOrDefault().HRME_AppDownloadedDeviceId != "")
                                        {
                                            var EmployeeDeviceid = Employee.FirstOrDefault().HRME_AppDownloadedDeviceId;
                                            if (data.communicationfalg == "Notif")
                                            {
                                                int e = callnotification(data.HRME_Id, EmployeeDeviceid, data.MI_Id, "Deviation-Comments", data.communicationmsg, data.communicationfalg, data.UserId);
                                                if (e == 1)
                                                {
                                                    data.message = "success";
                                                }
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

        public string gettransactionno(ISM_PlannerReportsDTO data)
        {
            string TransNo = "";
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == data.MI_Id && t.ISMEMN_Type == data.communicationfalg && t.ASMAY_ID == data.ASMAY_Id).OrderByDescending(t => t.ISMEMN_ID).ToList();
            if (checktransno.Count > 0)
            {
                string[] Newtransno = checktransno.FirstOrDefault().ISMEMN_No.Split('/');
                var Clinet = Newtransno[0].ToString();
                var Type = Newtransno[1].ToString();
                var Year = Newtransno[2].ToString();
                var NewTransNo = Newtransno[3].ToString();
                TransNo = (Convert.ToInt32(NewTransNo) + 1).ToString();
                TransNo = (Clinet + "/" + Type + "/" + Year + "/" + TransNo).ToString();
            }
            else
            {
                var transano = _Context.Master_Numbering.Where(imp => imp.MI_Id == data.MI_Id && imp.IMN_Flag == data.communicationfalg).ToList();
                if (transano.Count > 0)
                {
                    var acyr = _Context.AcademicYear.Where(t => t.ASMAY_Id.Equals(data.ASMAY_Id)).FirstOrDefault();
                    string AcadYear = acyr.ASMAY_Year;

                    var Clinet = transano.FirstOrDefault().IMN_PrefixParticular;
                    var Type = data.communicationfalg.ToUpper();
                    var Year = AcadYear;
                    var NewTransNo = transano.FirstOrDefault().IMN_StartingNo;
                    //TransNo = (Convert.ToInt32(NewTransNo) + 1).ToString();
                    TransNo = (Clinet + "/" + Type + "/" + Year + "/" + NewTransNo).ToString();
                }
            }
            if (TransNo != "")
            {
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    DateTime cmpdate = DateTime.Now;
                    if (data.comp_Date == null)
                    {
                        cmpdate = DateTime.Now.AddDays(3);
                    }
                    else
                    {
                        cmpdate = Convert.ToDateTime(data.comp_Date);
                    }
                    cmd.CommandText = "IVRM_MEMO_NOTICE_Outgoing";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_No",
                    SqlDbType.NVarChar)
                    {
                        Value = TransNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_Type",
                       SqlDbType.NVarChar)
                    {
                        Value = data.communicationfalg
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_Description",
                    SqlDbType.NVarChar)
                    {
                        Value = data.communicationfalg
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_CreatedBy",
                       SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@INDDate",
                      SqlDbType.DateTime)
                    {
                        Value = indiantime0
                    });
                    cmd.Parameters.Add(new SqlParameter("@COMPDate",
                     SqlDbType.DateTime)
                    {
                        Value = cmpdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@startdate",
                      SqlDbType.DateTime)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Enddate",
                     SqlDbType.DateTime)
                    {
                        Value = data.enddate
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

            return TransNo;
        }
        public async Task<ISM_PlannerReportsDTO> getdeviationReport_weekly(ISM_PlannerReportsDTO data)
        {
            try
            {

                string hrmeids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrmeids = hrmeids + "," + ue.HRME_Id;
                    }
                }

                //******************************Deviation Remarks & Deviation Calculation Report ************************************//

                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Weekly_DeviationReport_Calculation_Report_proc";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.VarChar)
                        {
                            Value = hrmeids
                        });
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
                            data.get_deviationreport = retObject.ToArray();
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

        public string sendmail(long MI_Id, string Email, string Template, long UserID, long HRMD_Id, long id, string flag, string transno, string msg, string taskdetails, ISM_PlannerReportsDTO data, DateTime? compdate)
        {
            try
            {
                long HRMEID = 0;
                string mailcc = "";
                string mailbcc = "";
                var Employee = (from a in _IssueContext.MasterEmployee
                                from b in _Context.Staff_User_Login
                                where a.HRME_Id == b.Emp_Code && b.Id == id
                                select new ISM_PlannerReportsDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    MI_Id = a.MI_Id
                                }).ToList();
                if (Employee.Count() > 0)
                {
                    HRMEID = Convert.ToInt64(Employee.FirstOrDefault().HRME_Id);
                }

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
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_MEMO_NOTICE_TEMPLET_PARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@EMP_HRMEID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                        //Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                       SqlDbType.BigInt)
                    {
                        Value = HRMEID
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                        SqlDbType.VarChar)
                    {
                        Value = flag
                        //Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.VarChar)
                    {
                        Value = MI_Id
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

                val.Add("[MSG]", taskdetails);
                if (data.communicationfalg == "Notice")
                {
                    val.Add("[ISMEMN_No]", data.ISMEMN_No);
                    val.Add("[ISMEMN_Date]", data.ISMEMN_Date);
                    val.Add("[ISMEMN_CompleByDate]", data.ISMEMN_CompleByDate);
                }
                val.Add("[STARTDATE]", Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy"));
                val.Add("[ENDDATE]", Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy"));
                val.Add("[COMPDATE]", Convert.ToDateTime(compdate).ToString("dd/MM/yyyy"));

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
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string emailccdetails = "";
                    string emailbccdetails = "";
                    var emaildetails = _Context.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == MI_Id && a.HRMD_Id == HRMD_Id).ToList();
                    if (emaildetails.Count > 0)
                    {
                        emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                        emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                    }
                    else
                    {
                        string heademail = "";
                        if (HRMD_Id != 0)
                        {
                            List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Fetch_DepartmentHead";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = HRMD_Id
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
                                                Tomail = Convert.ToString(dataReader["Head"])                                               
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if(dtonewtemp.Count()>0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    heademail = dtonewtemp[j].Tomail;                                   
                                }
                            }
                        }

                        if(heademail!="")
                        {
                            emailccdetails = heademail+",hr@vapstech.com,pstomd@vapstech.com";
                        }
                        else
                        {
                            emailccdetails = "hr@vapstech.com,pstomd@vapstech.com";
                        }                       
                        //emailbccdetails = "rakesh.reddy@vapstech.com";
                    }

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
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

                        if (data.Deviationreporttype == "Dept")
                        {
                            //message.AddCc("siddesh@vapstech.com");
                            //message.AddCc("hr@vapstech.com");
                            //message.AddCc("pstomd@vapstech.com");
                            try
                            {
                                List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Fetch_MemoNoticeccmails";
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
                        }
                        else
                        {
                            if (emailccdetails != null && emailccdetails != "")
                            {
                                string[] ccmaildetails = emailccdetails.Split(',');

                                foreach (var c in ccmaildetails)
                                {
                                    if (c != Email)
                                    {
                                        message.AddCc(c);
                                    }

                                }
                            }

                            if (emailbccdetails != null && emailbccdetails != "")
                            {
                                string[] bccmaildetails = emailbccdetails.Split(',');

                                foreach (var bc in bccmaildetails)
                                {
                                    if (bc != Email)
                                    {
                                        message.AddBcc(bc);
                                    }
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();

                        var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == MI_Id && t.ISMEMN_No == transno).ToList();
                        if (checktransno.Count() > 0)
                        {
                            var Resultupdate = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Single(a => a.ISMEMN_ID == checktransno.FirstOrDefault().ISMEMN_ID);
                            Resultupdate.ISMEMN_EmailSentFlag = true;
                            _IssueContext.Update(Resultupdate);
                            _IssueContext.SaveChanges();
                        }

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
                                Value = data.communicationfalg + " - " + transno + " - " + data.employeename
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
                    else
                    {

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
                                emailMessage.From = new MailAddress(SendingEmail, institutionName[0].MI_Name);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                if (Attechement.Equals("1"))
                                {
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
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


                                if (data.Deviationreporttype == "Dept")
                                {
                                    emailMessage.CC.Add("siddesh@vapstech.com");
                                    emailMessage.CC.Add("pstomd@vapstech.com");
                                    emailMessage.CC.Add("hr@vapstech.com");
                                }
                                else
                                {
                                    if (emailccdetails != null && emailccdetails != "")
                                    {
                                        string[] ccmaildetails = emailccdetails.Split(',');

                                        foreach (var c in ccmaildetails)
                                        {
                                            if (c != Email)
                                            {
                                                emailMessage.CC.Add(c);
                                            }
                                        }
                                    }

                                    if (emailbccdetails != null && emailbccdetails != "")
                                    {
                                        string[] bccmaildetails = emailbccdetails.Split(',');

                                        foreach (var bc in bccmaildetails)
                                        {
                                            if (bc != Email)
                                            {
                                                emailMessage.Bcc.Add(bc);
                                            }
                                        }
                                    }
                                }

                                clientsmtp.Send(emailMessage);

                                var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == MI_Id && t.ISMEMN_No == transno).ToList();
                                if (checktransno.Count() > 0)
                                {
                                    var Resultupdate = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Single(a => a.ISMEMN_ID == checktransno.FirstOrDefault().ISMEMN_ID);
                                    Resultupdate.ISMEMN_EmailSentFlag = true;
                                    _IssueContext.Update(Resultupdate);
                                    _IssueContext.SaveChanges();
                                }
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
                                        Value = data.communicationfalg + " - " + transno + " - " + data.employeename
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

        public string sendmailcmt(long MI_Id, string Email, string Template, long UserID, long HRMD_Id, long id, string flag, string transno, string msg)
        {
            try
            {
                string HRMEM_EmailId = "";
                string SendingEmail = "";
                var Employee = (from a in _IssueContext.MasterEmployee
                                from b in _Context.Staff_User_Login
                                from c in _Context.Multiple_Email_DMO
                                where a.HRME_Id == b.Emp_Code && a.HRME_Id == c.HRME_Id && b.Id == id
                                select new ISM_PlannerReportsDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    MI_Id = a.MI_Id,
                                    HRMEM_EmailId = c.HRMEM_EmailId
                                }).ToList();
                if (Employee.Count() > 0)
                {
                    HRMEM_EmailId = Employee.FirstOrDefault().HRMEM_EmailId;
                }
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                if (alldetails.Count > 0)
                {
                    if (HRMEM_EmailId != "" && HRMEM_EmailId != null)
                    {
                        SendingEmail = HRMEM_EmailId;
                    }
                    else
                    {
                        SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    }
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = "Deviation-Comments";
                    var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        message.HtmlContent = msg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
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

        public int callnotification(long hrme_id, string devicenew, long MI_Id, string headername, string text, string flag, long id)
        {
            string transid = "";
            string result = "";
            string key = "";
            string emp = "";
            var head = "";
            var empimage = "";
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            int cnt = 0;
            var keylist = _Context.MobileApplAuthenticationDMO.Where(t => t.MI_Id == MI_Id).Distinct().ToList();
            if (keylist.Count > 0)
            {
                key = keylist.FirstOrDefault().MAAN_AuthenticationKey;
            }
            var Employee = (from a in _IssueContext.MasterEmployee
                            from b in _Context.Staff_User_Login
                            from c in _Context.Multiple_Email_DMO
                            where a.HRME_Id == b.Emp_Code && a.HRME_Id == c.HRME_Id && b.Id == id
                            select new ISM_PlannerReportsDTO
                            {
                                HRME_Id = a.HRME_Id,
                                MI_Id = a.MI_Id,
                                HRMEM_EmailId = c.HRMEM_EmailId,
                                employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                HRME_Photo = a.HRME_Photo
                            }).ToList();
            if (Employee.Count > 0)
            {
                emp = Employee.FirstOrDefault().employeename;
                head = headername + " by " + emp;
                empimage = Employee.FirstOrDefault().HRME_Photo;
            }
            else
            {
                head = headername;
            }

            try
            {
                if (key != "" && key != null)
                {
                    string devicenew1 = '"' + devicenew + '"';
                    devicenew1 = "[" + devicenew1 + "]";
                    string url = "";
                    url = "https://fcm.googleapis.com/fcm/send";
                    List<string> notificationparams = new List<string>();
                    string daata = "";
                    string sound = "default";
                    daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew1 + "," +
                    "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + head + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "body" + '"' + ":" + '"' + text + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + "," + '"' + "image" + '"' + ":" + '"' + empimage + '"' + "," + '"' + "image-type" + '"' + ":" + '"' + "circle" + '"' + "}}";
                    notificationparams.Add(daata.ToString());
                    var mycontent = notificationparams[0];
                    string postdata = mycontent.ToString();
                    HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                    connection.ContentType = "application/json";
                    connection.MediaType = "application/json";
                    connection.Accept = "application/json";
                    connection.Method = "post";
                    connection.Headers["authorization"] = "key=" + key;
                    using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                    {
                        requestwriter.Write(postdata);
                    }
                    string responsedata = string.Empty;
                    using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                    {
                        responsedata = responsereader.ReadToEnd();
                        JObject joresponse1 = JObject.Parse(responsedata);
                        transid = (string)joresponse1["multicast_id"];
                        result = (string)joresponse1["success"];
                    }

                    if (result == "1")
                    {
                        Insertnotification(MI_Id, hrme_id, indiantime0, head, id, text);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (result == "0")
            {
                cnt = 0;
            }
            else if (result == "1")
            {
                cnt = 1;
            }
            else
            {
                cnt = 0;
            }
            return cnt;
        }

        public string Insertnotification(long MI_Id, long empid, DateTime indianTime, string empname, long userid, string text)
        {
            try
            {
                var notification = empname;
                ISM_DailyReportGenerationDTO data = new ISM_DailyReportGenerationDTO();
                ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                push.MI_Id = MI_Id;
                push.HRME_Id = empid;
                push.ISMNO_Notification = notification;
                push.ISMNO_NoticationDate = indianTime;
                push.ISMNO_NotificationType = "DEVIATION";
                push.ISMNO_ReadFlg = false;
                push.ISMNO_MakeUnReadFlg = false;
                push.ISMNO_ActiveFlag = true;
                push.ISMNO_CreatedBy = userid;
                push.ISMNO_UpdatedBy = userid;
                push.CreatedDate = indianTime;
                push.UpdatedDate = indianTime;
                push.ISMNO_Response = text;
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

        public string gettaskregisterReport(ISM_PlannerReportsDTO data)
        {
            try
            {
                int rownumber = 0;
                string str = "";
                string str1 = "";
                string str2 = "";
                if (data.Deviationreporttype == "Dept")
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
                    long empuserid = 0;
                    var useridofemp = _IssueContext.Staff_User_Login.Where(t => t.Emp_Code == data.HRME_Id).ToList();
                    if (useridofemp.Count > 0)
                    {
                        empuserid = Convert.ToInt64(useridofemp.FirstOrDefault().Id);
                    }
                    List<ISM_PlannerReportsDTO> emplist = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_User_EmployeeList_admin";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@role",
          SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@user_Id",
                 SqlDbType.BigInt)
                        {
                            Value = empuserid
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
                                    emplist.Add(new ISM_PlannerReportsDTO
                                    {
                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    string hrmeids = "0";
                    if (emplist.Count > 0)
                    {
                        for (int j = 0; j < emplist.Count; j++)
                        {
                            if (emplist.Count > 0)
                            {
                                foreach (var ue in emplist)
                                {
                                    hrmeids = hrmeids + "," + ue.HRME_Id;
                                }
                            }
                            else
                            {
                                if (hrmeids == "" || hrmeids == "0")
                                {
                                    hrmeids = data.HRME_Id.ToString();
                                }
                            }
                        }
                    }

                    List<ISM_PlannerReportsDTO> empdevlist = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "departmentWise_Dev_Report_Employee";
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
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                        {
                            Value = hrmeids
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
                                        employeename = Convert.ToString(dataReader["employeename"]),
                                        deviation = Convert.ToString(dataReader["DevPer"])
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
                        str += "<p> <span style=" + "'font-weight:bold'" + ">Developer Wise Deviation Lists :</span></p> <table width='100%' style=" + "'border-collapse: collapse; width: 100%;'" + " class=" + "'table1'" + "><tr>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMPLOYEE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STARTDATE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ENDDATE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DEVIATION </th>" +
                                 "</tr>";
                        for (int j = 0; j < empdevlist.Count; j++)
                        {

                            if (Convert.ToDecimal(empdevlist[j].deviation) >= Convert.ToDecimal(data.NoOfDays))
                            {
                                rownumber += 1;
                                str1 += "<tr> " +
                               "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].employeename + "</td>" +
                               "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-family: sans-serif;'" + ">" + data.startdate.Value.ToShortDateString() + "</td>" +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + data.enddate.Value.ToShortDateString() + "</td> " +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].deviation + "%</td> " +
                               "</tr>";
                            }
                        }
                        str2 = "</table>";
                    }
                    data.message = str + str1 + str2;
                }
                else
                {
                    List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Deviation_Remarkswise_Report_Department";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@startDate_N",
                       SqlDbType.Date)
                        {
                            Value = data.startdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate_N",
                       SqlDbType.Date)
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
                                    dtonewtemp.Add(new ISM_PlannerReportsDTO
                                    {
                                        ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                        ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                        ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                        ISMTCR_Status = Convert.ToString(dataReader["ISMTCR_Status"]),
                                        AssignedBy = Convert.ToString(dataReader["AssignedBy"]),
                                        taskstartdate = Convert.ToString(dataReader["StartDate"]),
                                        taskatenddate = Convert.ToString(dataReader["EndDate"]),
                                        ISMTPLTA_Id = Convert.ToInt64(dataReader["ISMTPLTA_Id"])
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
                        if (data.deviation.Length > 5)
                        {
                            data.deviation = data.deviation.Substring(0, 5);
                        }
                        //Deviation: " + data.deviation + " %</ span > < span style = " + "'float: center;padding-left:5%;'" + " > Total Open Tasks Tilldate: " + dtonewtemp.Count + " </ span ></ span ></ p >
                        var stdate = Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy");
                        var eddate = Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy");
                        str += "<p> <span style=" + "'font-weight:bold'" + ">Pending Issue Lists :<span style=" + "'float: center;padding-left:10%;'" + ">" + stdate + " TO " + eddate + "</span> </span></p> <table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK NO.</th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK NAME</th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK TYPE </th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STATUS </th>" +
                                "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ASSIGNEDBY </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STARTDATE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ENDDATE </th>" +
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
                                 "</tr>";
                        }
                        str2 = "</table>";

                    }
                    data.message = str + str1 + str2;

                    var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == data.MI_Id && t.ISMEMN_Type == data.communicationfalg && t.ASMAY_ID == data.ASMAY_Id).OrderByDescending(t => t.ISMEMN_ID).ToList();
                    long memonoticeid = 0;
                    if (checktransno.Count() > 0)
                    {
                        memonoticeid = checktransno.FirstOrDefault().ISMEMN_ID;
                    }

                    if (dtonewtemp.Count() > 0)
                    {
                        if (data.type == true)
                        {
                            foreach (var item in dtonewtemp)
                            {
                                ISM_EMPLOYEE_MEMO_NOTICE_TASKS res = new ISM_EMPLOYEE_MEMO_NOTICE_TASKS();

                                res.ISMTPLTA_Id = item.ISMTPLTA_Id;
                                res.ISMEMN_ID = memonoticeid;
                                res.Createddate = DateTime.Now;
                                res.Updateddate = DateTime.Now;
                                _IssueContext.Add(res);
                                _IssueContext.SaveChanges();
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.message;
        }
        public string gettaskregisterReportnotice(ISM_PlannerReportsDTO data)
        {
            try
            {
                int rownumber = 0;
                string str = "";
                string str1 = "";
                string str2 = "";
                if (data.Deviationreporttype == "Dept")
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
                    long empuserid = 0;
                    var useridofemp = _IssueContext.Staff_User_Login.Where(t => t.Emp_Code == data.HRME_Id).ToList();
                    if (useridofemp.Count > 0)
                    {
                        empuserid = Convert.ToInt64(useridofemp.FirstOrDefault().Id);
                    }
                    List<ISM_PlannerReportsDTO> emplist = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_User_EmployeeList_admin";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@role",
          SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@user_Id",
                 SqlDbType.BigInt)
                        {
                            Value = empuserid
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
                                    emplist.Add(new ISM_PlannerReportsDTO
                                    {
                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    string hrmeids = "0";
                    if (emplist.Count > 0)
                    {
                        for (int j = 0; j < emplist.Count; j++)
                        {
                            if (emplist.Count > 0)
                            {
                                foreach (var ue in emplist)
                                {
                                    hrmeids = hrmeids + "," + ue.HRME_Id;
                                }
                            }
                            else
                            {
                                if (hrmeids == "" || hrmeids == "0")
                                {
                                    hrmeids = data.HRME_Id.ToString();
                                }
                            }
                        }
                    }

                    List<ISM_PlannerReportsDTO> empdevlist = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "departmentWise_Dev_Report_Employee";
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
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                        {
                            Value = hrmeids
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
                                        employeename = Convert.ToString(dataReader["employeename"]),
                                        deviation = Convert.ToString(dataReader["DevPer"])
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
                        str += "<p> <span style=" + "'font-weight:bold'" + ">Developer Wise Deviation Lists :</span></p> <table width='100%' style=" + "'border-collapse: collapse; width: 100%;'" + " class=" + "'table1'" + "><tr>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMPLOYEE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STARTDATE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ENDDATE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DEVIATION </th>" +
                                 "</tr>";
                        for (int j = 0; j < empdevlist.Count; j++)
                        {

                            if (Convert.ToDecimal(empdevlist[j].deviation) >= Convert.ToDecimal(data.NoOfDays))
                            {
                                rownumber += 1;
                                str1 += "<tr> " +
                               "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].employeename + "</td>" +
                               "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-family: sans-serif;'" + ">" + data.startdate.Value.ToShortDateString() + "</td>" +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + data.enddate.Value.ToShortDateString() + "</td> " +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].deviation + "%</td> " +
                               "</tr>";
                            }
                        }
                        str2 = "</table>";
                    }
                    data.message = str + str1 + str2;
                }
                else
                {
                    List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Peding_Memo_Task_List";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Memo",
                        SqlDbType.VarChar)
                        {
                            Value = data.ISMEMN_No
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
                                        taskatenddate = Convert.ToString(dataReader["EndDate"]),
                                        ISMTPLTA_Id = Convert.ToInt64(dataReader["ISMTPLTA_Id"])
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
                                 "</tr>";
                        }
                        str2 = "</table>";

                    }
                    data.message = str + str1 + str2;

                    var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == data.MI_Id && t.ISMEMN_Type == data.communicationfalg && t.ASMAY_ID == data.ASMAY_Id).OrderByDescending(t => t.ISMEMN_ID).ToList();
                    long memonoticeid = 0;
                    if (checktransno.Count() > 0)
                    {
                        memonoticeid = checktransno.FirstOrDefault().ISMEMN_ID;
                    }
                    if (data.type == true)
                    {
                        if (dtonewtemp.Count() > 0)
                        {
                            foreach (var item in dtonewtemp)
                            {
                                ISM_EMPLOYEE_MEMO_NOTICE_TASKS res = new ISM_EMPLOYEE_MEMO_NOTICE_TASKS();

                                res.ISMTPLTA_Id = item.ISMTPLTA_Id;
                                res.ISMEMN_ID = memonoticeid;
                                res.Createddate = DateTime.Now;
                                res.Updateddate = DateTime.Now;
                                _IssueContext.Add(res);
                                _IssueContext.SaveChanges();
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.message;
        }

        public async Task<ISM_PlannerReportsDTO> gettaskdetails(ISM_PlannerReportsDTO data)
        {
            if (data.type == true)
            {
                try
                {
                    if (data.communicationfalg == "Notice")
                    {
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Peding_Memo_Task_List";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                            SqlDbType.VarChar)
                            {
                                Value = data.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Memo",
                       SqlDbType.VarChar)
                            {
                                Value = data.ISMEMN_No
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
                    else
                    {
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Deviation_Remarkswise_Report_Department";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                            SqlDbType.VarChar)
                            {
                                Value = data.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@startDate_N",
                          SqlDbType.Date)
                            {
                                Value = data.startdate
                            });
                            cmd.Parameters.Add(new SqlParameter("@EndDate_N",
                           SqlDbType.Date)
                            {
                                Value = data.enddate
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                if (data.communicationfalg == "Notice")
                {
                    data.html = gethtmltempletnotice(data.communicationfalg, data);
                }
                else
                {
                    data.html = gethtmltemplet(data.communicationfalg, data);
                }

            }
            return data;
        }
        public async Task<ISM_PlannerReportsDTO> gettaskdetailsreport(ISM_PlannerReportsDTO data)
        {
                try
                {
                 using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Peding_Memo_Task_ListReport";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                            SqlDbType.VarChar)
                            {
                                Value = data.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Memo",
                       SqlDbType.VarChar)
                            {
                                Value = data.ISMEMN_No
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

        public string gethtmltemplet(string Template, ISM_PlannerReportsDTO data)
        {
            string Mailmsg = "";
            string taskdetails = "";
            try
            {
                foreach (var ue in data.EmplistforMNNE)
                {
                    var Employee = (from a in _IssueContext.MasterEmployee
                                    from b in _Context.Multiple_Email_DMO
                                    where a.HRME_Id == b.HRME_Id && a.HRME_Id == ue.HRME_Id && b.HRME_Id == ue.HRME_Id
                                    select new ISM_PlannerReportsDTO
                                    {
                                        HRMEM_EmailId = b.HRMEM_EmailId,
                                        HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                        HRMD_Id = Convert.ToInt64(a.HRMD_Id),
                                        MI_Id = a.MI_Id,
                                        employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim()

                                    }).ToList();
                    data.HRME_Id = ue.HRME_Id;


                    if (data.EmplistforMNNE.Length > 0)
                    {
                        decimal deviationper = 0;
                        foreach (var plan in data.EmplistforMNNE)
                        {
                            if (plan.plannerdetails.Length > 1)
                            {
                                foreach (var planD in plan.plannerdetails)
                                {
                                    deviationper = deviationper + Convert.ToDecimal(planD.Deviation_Percentage);
                                }
                                deviationper = (deviationper / plan.plannerdetails.Length);
                            }
                            else
                            {
                                deviationper = Convert.ToDecimal(plan.plannerdetails.FirstOrDefault().Deviation_Percentage);
                            }
                            data.deviation = deviationper.ToString();
                        }
                    }
                    long id = data.UserId;
                    string flag = "M";
                    data.MI_Id = Employee.FirstOrDefault().MI_Id;
                    long MI_Id = Employee.FirstOrDefault().MI_Id;
                    data.ASMAY_Id = _IssueContext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                    data.HRME_Id = ue.HRME_Id;
                    data.employeename = Employee.FirstOrDefault().employeename;
                    data.HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                    long HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                    var EmployeeEmail = Employee.FirstOrDefault().HRMEM_EmailId;
                    data.EmployeeEmail = EmployeeEmail;
                    taskdetails = gettaskregisterReport(data);
                    long HRMEID = 0;
                    var Employee1 = (from a in _IssueContext.MasterEmployee
                                     from b in _Context.Staff_User_Login
                                     where a.HRME_Id == b.Emp_Code && b.Id == id
                                     select new ISM_PlannerReportsDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         MI_Id = a.MI_Id
                                     }).ToList();
                    if (Employee1.Count() > 0)
                    {
                        HRMEID = Convert.ToInt64(Employee1.FirstOrDefault().HRME_Id);
                    }

                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count == 0)
                    {
                        return "";
                    }
                    var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                    var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                    var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                    Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                    string result = Mailmsg;
                    List<Match> variables = new List<Match>();
                    foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                    {
                        variables.Add(match);
                    }
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VIEWISM_MEMO_NOTICE_TEMPLET_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EMP_HRMEID",
                            SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                            //Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                           SqlDbType.BigInt)
                        {
                            Value = HRMEID
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                            SqlDbType.VarChar)
                        {
                            Value = data.communicationfalg
                            //Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
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
                    string taskex = "";
                    if (data.communicationfalg == "Memo")
                    {
                        taskex = ".../MEMO/2019-2020/..";
                    }
                    else
                    {
                        taskex = ".../NOTICE/2019-2020/..";
                    }

                    val.Add("[MSG]", taskdetails);
                    val.Add("[REFNO]", taskdetails);
                    val.Add("[STARTDATE]", Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy"));
                    val.Add("[ENDDATE]", Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy"));
                    val.Add("[COMPDATE]", Convert.ToDateTime(ue.comp_Date).ToString("dd/MM/yyyy"));

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                if (ParamaetersName[j].ISMP_NAME == "[REFNO]")
                                {
                                    val[ParamaetersName[j].ISMP_NAME] = taskex;
                                }
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[j].ISMP_NAME]);
                                Mailmsg = result;
                            }
                        }
                    }

                    Mailmsg = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return Mailmsg;
        }

        public string gethtmltempletnotice(string Template, ISM_PlannerReportsDTO data)
        {
            string Mailmsg = "";
            string taskdetails = "";
            try
            {
                foreach (var ue in data.EmplistforMNNE)
                {
                    var Employee = (from a in _IssueContext.MasterEmployee
                                    from b in _Context.Multiple_Email_DMO
                                    where a.HRME_Id == b.HRME_Id && a.HRME_Id == ue.HRME_Id && b.HRME_Id == ue.HRME_Id
                                    select new ISM_PlannerReportsDTO
                                    {
                                        HRMEM_EmailId = b.HRMEM_EmailId,
                                        HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                        HRMD_Id = Convert.ToInt64(a.HRMD_Id),
                                        MI_Id = a.MI_Id,
                                        employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim()

                                    }).ToList();
                    data.HRME_Id = ue.HRME_Id;
                    long id = data.UserId;
                    string flag = "M";
                    data.MI_Id = Employee.FirstOrDefault().MI_Id;
                    long MI_Id = Employee.FirstOrDefault().MI_Id;
                    data.ASMAY_Id = _IssueContext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                    data.HRME_Id = ue.HRME_Id;
                    data.employeename = Employee.FirstOrDefault().employeename;
                    data.HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                    long HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                    var EmployeeEmail = Employee.FirstOrDefault().HRMEM_EmailId;
                    data.EmployeeEmail = EmployeeEmail;
                    data.ISMEMN_No = ue.ISMEMN_No;
                    taskdetails = gettaskregisterReportnotice(data);
                    long HRMEID = 0;
                    var Employee1 = (from a in _IssueContext.MasterEmployee
                                     from b in _Context.Staff_User_Login
                                     where a.HRME_Id == b.Emp_Code && b.Id == id
                                     select new ISM_PlannerReportsDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         MI_Id = a.MI_Id
                                     }).ToList();
                    if (Employee1.Count() > 0)
                    {
                        HRMEID = Convert.ToInt64(Employee1.FirstOrDefault().HRME_Id);
                    }

                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count == 0)
                    {
                        return "";
                    }
                    var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                    var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                    var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                    Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                    string result = Mailmsg;
                    List<Match> variables = new List<Match>();
                    foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                    {
                        variables.Add(match);
                    }
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VIEWISM_MEMO_NOTICE_TEMPLET_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EMP_HRMEID",
                            SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                            //Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                           SqlDbType.BigInt)
                        {
                            Value = HRMEID
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                            SqlDbType.VarChar)
                        {
                            Value = data.communicationfalg
                            //Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
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
                    string taskex = "";
                    if (data.communicationfalg == "Memo")
                    {
                        taskex = ".../MEMO/2019-2020/..";
                    }
                    else
                    {
                        taskex = ".../NOTICE/2019-2020/..";
                    }

                    val.Add("[MSG]", taskdetails);
                    val.Add("[REFNO]", taskdetails);
                    val.Add("[ISMEMN_No]", ue.ISMEMN_No);
                    val.Add("[ISMEMN_Date]", ue.ISMEMN_Date);
                    val.Add("[ISMEMN_CompleByDate]", ue.ISMEMN_CompleByDate);
                    val.Add("[STARTDATE]", Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy"));
                    val.Add("[ENDDATE]", Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy"));
                    val.Add("[COMPDATE]", Convert.ToDateTime(ue.comp_Date).ToString("dd/MM/yyyy"));

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                if (ParamaetersName[j].ISMP_NAME == "[REFNO]")
                                {
                                    val[ParamaetersName[j].ISMP_NAME] = taskex;
                                }
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[j].ISMP_NAME]);
                                Mailmsg = result;
                            }
                        }
                    }

                    Mailmsg = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return Mailmsg;
        }

        public ISM_PlannerReportsDTO getdaily_deviation(ISM_PlannerReportsDTO dto)
        {
            try
            {
                int rownumber = 0;
                string str = "";
                string str1 = "";
                string str2 = "";
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                var hrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();
                List<ISM_PlannerReportsDTO> dtonewtemp1 = new List<ISM_PlannerReportsDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Daily_Deviation_Employee_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = hrmeid[0].Emp_Code
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
                                dtonewtemp1.Add(new ISM_PlannerReportsDTO
                                {
                                    ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                    ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                    ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                    ISMTCR_Status = Convert.ToString(dataReader["ISMTCR_Status"]),
                                    AssignedBy = Convert.ToString(dataReader["AssignedBy"]),
                                    taskstartdate = Convert.ToString(dataReader["StartDate"]),
                                    taskatenddate = Convert.ToString(dataReader["EndDate"]),
                                    day_count = Convert.ToInt32(dataReader["TotalDays"]),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (dtonewtemp1.Count > 0)
                {
                    str += "<p> <span style=" + "'font-weight:bold'" + ">Pending Issue Lists :<span style=" + "'float: center;padding-left:30%;'" + "> Total Tasks:" + dtonewtemp1.Count + "</span></span></p> <table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK NO.</th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK NAME</th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK TYPE </th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STATUS </th>" +
                            "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ASSIGNEDBY </th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STARTDATE </th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ENDDATE </th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Total Days </th>" +
                             "</tr>";
                    for (int j = 0; j < dtonewtemp1.Count; j++)
                    {
                        rownumber += 1;
                        str1 += "<tr> " +
                            "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + dtonewtemp1[j].ISMTCR_TaskNo + "</td>" +
                            "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + dtonewtemp1[j].ISMTCR_Title + "</td>" +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp1[j].ISMTCR_BugOREnhancementFlg + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp1[j].ISMTCR_Status + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp1[j].AssignedBy + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp1[j].taskstartdate + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp1[j].taskatenddate + "</td> " +
                             "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp1[j].day_count + "</td> " +
                             "</tr>";
                    }
                    str2 = "</table>";

                }
                dto.message = str + str1 + str2;
                var checktemplate = _IssueContext.smsEmailSetting.Where(a => a.MI_Id == dto.MI_Id && a.ISES_Template_Name == "Daily_deviation" && a.ISES_MailActiveFlag == true).ToList();
                var emailid = _Context.Multiple_Email_DMO.Where(a => a.HRME_Id == hrmeid[0].Emp_Code).ToList();

                string templatedetails1 = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                var Employee12 = (from a in _IssueContext.MasterEmployee
                                  from b in _Context.HR_Master_Department
                                  from c in _Context.HR_Master_Designation
                                  where a.HRMDES_Id == c.HRMDES_Id && a.HRMD_Id == b.HRMD_Id && a.HRME_Id == hrmeid[0].Emp_Code && a.MI_Id == dto.MI_Id
                                  select new ISM_PlannerReportsDTO
                                  {

                                      employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                      HRMD_DepartmentName = b.HRMD_DepartmentName,
                                      HRMDES_DesignationName = c.HRMDES_DesignationName


                                  }).ToList();
                DateTime dt = DateTime.Now;
                DateTime dt1 = dt.Date;

                templatedetails1 = templatedetails1.Replace("[STARTDATE]", Convert.ToDateTime(dto.taskstartdate).ToString("dd/MM/yyyy"));
                templatedetails1 = templatedetails1.Replace("[ENDDATE]", Convert.ToDateTime(dto.taskatenddate).ToString("dd/MM/yyyy"));
                templatedetails1 = templatedetails1.Replace("[MSG]", dto.message);
                templatedetails1 = templatedetails1.Replace("[EMPNAME]", Employee12[0].employeename);
                templatedetails1 = templatedetails1.Replace("[EMPDESIGNATION]", Employee12[0].HRMDES_DesignationName);
                templatedetails1 = templatedetails1.Replace("[DATE]", Convert.ToDateTime(dt).ToString());
                //templatedetails1 = templatedetails1.Replace("[COMPDATE]", Convert.ToDateTime(dt).ToString());

                sendmail_daily_deviation1(dto.MI_Id, emailid[0].HRMEM_EmailId, templatedetails1, dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }



            return dto;




        }
        public string sendmail_daily_deviation1(long MI_Id, string mail, string message1, ISM_PlannerReportsDTO data)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Daily_deviation" && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                //string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = message1;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
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
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    //============using API

                    //var message = new SendGridMessage();
                    //message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    //message.Subject = Subject;
                    //message.AddTo(mail);
                    //if (Attechement.Equals("1"))
                    //{
                    //    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                    //    if (img.Count > 0)
                    //    {
                    //        for (int i = 0; i < img.Count; i++)
                    //        {
                    //            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                    //            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                    //            Stream stream = response.GetResponseStream();
                    //            message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                    //        }
                    //    }
                    //}

                    //message.HtmlContent = Mailmsg;
                    //var client = new SendGridClient(sengridkey);
                    //client.SendEmailAsync(message).Wait();

                    //=====================using API complited
                    //==========smtp=
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


                            emailMessage.To.Add(new MailAddress(mail));
                            emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                            emailMessage.Subject = Subject;
                            emailMessage.Body = Mailmsg;
                            emailMessage.IsBodyHtml = true;


                            if (Attechement.Equals("1"))
                            {
                                var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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



                            clientsmtp.Send(emailMessage);
                        }
                    }
                    ////================smtp complited===========

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Daily_deviation").Select(d => d.IVRMIM_Id).ToList();

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
                            Value = mail
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = data.communicationfalg + " -  " + data.employeename
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

        //=====================rating report==========
        public ISM_PlannerReportsDTO getEmp_ratingReport(ISM_PlannerReportsDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                string hrmeids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrmeids = hrmeids + "," + ue.HRME_Id;
                    }
                }
                else
                {
                    if (hrmeids == "" || hrmeids == "0")
                    {
                        hrmeids = empid.ToString();
                    }
                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DeviationReport_Calculation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
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
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate",
                    SqlDbType.DateTime)
                    {
                        Value = data.enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
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
                        data.get_emp_rating_report = retObject.ToArray();
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

        public ISM_PlannerReportsDTO startenddays(ISM_PlannerReportsDTO data)
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

        public ISM_PlannerReportsDTO taskRejectReport(ISM_PlannerReportsDTO data)
        {
            try
            {
               
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                string hrmeids = "0";
               

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DR_notsend_reject_LOP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                    SqlDbType.BigInt)
                    {
                        Value = empid
                    });
                   
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                   SqlDbType.DateTime)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
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
                        data.RejectReport = retObject.ToArray();
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

        public long completebydays(long hrmd_id, string flag)
        {
            long days = 0;
            try
            {
                List<ISM_PlannerReportsDTO> empdevlist = new List<ISM_PlannerReportsDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MEMO_NOTICE_COMPLETEBY_DATE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRMD_ID",
            SqlDbType.BigInt)
                    {
                        Value = hrmd_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
            SqlDbType.VarChar)
                    {
                        Value = flag
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
                                    day_count = Convert.ToInt64(dataReader["Days"])
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
                    days = empdevlist[0].day_count;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return days;
        }
    }
}

