using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Services
{
    public class ISM_DailyReport_ApprovalImpl : Interfaces.ISM_DailyReport_ApprovalInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_DailyReport_ApprovalImpl(IssueManagerContext _issueManager, DomainModelMsSqlServerContext _db)
        {
            _IssueContext = _issueManager;
            _Context = _db;
        }
        public ISM_DailyReport_ApprovalDTO GetDetails(ISM_DailyReport_ApprovalDTO data)
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

                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true
                && t.HRME_LeftFlag == false).Distinct().ToList();

                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

                    data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                }

                var hrmeDepartment = _IssueContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == dept_id
                && t.HRMD_ActiveFlag == true).Distinct().ToList();

                if (hrmeDepartment.Count > 0)
                {
                    data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                }

                if (data.DRApprovalTypeFlag.Equals("DRMultiApproval"))
                {
                    roletype = "Admin";
                    data.roleflag = false;
                }

                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_admin_DR_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
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
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt) { Value = dept_id });
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
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
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
                        data.Designation = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** Getting Previous Date ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_GET_Previous_Date_DR_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.Date, Int32.MaxValue) { Direction = ParameterDirection.Output });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data1 = cmd.ExecuteNonQuery();

                    data.previousdate = Convert.ToDateTime(cmd.Parameters["@FROMDATE"].Value);
                }

                //*********************************** Leave Approval Is Pending ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LeaveApproval";
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
                        data.get_leaveDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** Certificate Approval Is Pending ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_CertificateRequest_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.UserId)
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
                        data.Cerfificatelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                //*********************************** Comp Off Approval Is Pending ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Compoffondutyleave_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.UserId)
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
                        data.compoffdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                //*********************************** Noc Approval Is Pending ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_NocRequest_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.UserId)
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
                        data.NOCdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


                //*********************************** Rating Approval Is Pending ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Rating_User_EmployeeList_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = data.Role_flag });
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


                //***********************************  ********************************// 

                data.monthlist = _IssueContext.IVRM_Month_DMO.ToArray();

                data.yearlist = _IssueContext.HR_Master_LeaveYearDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true).OrderByDescending(a => a.HRMLY_LeaveYear).ToArray();
                // added by sanjeev 
              //  data.UserId = 60152;
                var adavnace = (from a in _IssueContext.HR_PROCESSDMO
                                from b in _IssueContext.HR_Process_Auth_OrderNoDMO
                                where (a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAD" && b.IVRMUL_Id == data.UserId)
                                select a
                               ).ToList();
                var tadaapply = (from a in _IssueContext.HR_PROCESSDMO
                                 from b in _IssueContext.HR_Process_Auth_OrderNoDMO
                                 where (a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAPL" && b.IVRMUL_Id == data.UserId)
                                 select a
                               ).ToList();
                if (adavnace.Count > 0 || tadaapply.Count > 0)
                {

                    DateTime dates = DateTime.Now.AddDays(-2);

                    var VTADAA_IdLi = _IssueContext.TADA_Advance_Apply_AuthorisationDMO.Where(R => R.IVRMUL_Id == data.UserId).ToList();
                    List<long> VTADAA_IdCC = new List<long>();
                    if (VTADAA_IdLi.Count > 0)
                    {
                        foreach (var d in VTADAA_IdLi)
                        {
                            VTADAA_IdCC.Add(d.VTADAAA_Id);
                        }
                    }


                    var TadaAdavancecount = _IssueContext.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_StatusFlg == "Pending" && (R.VTADAAA_AppliedDate < dates) && !VTADAA_IdCC.Contains(R.VTADAAA_Id)).ToList();
                    if (TadaAdavancecount.Count > 0)
                    {
                        //GetDaywisedetails
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "TADA_ADVANCE_APPROVAL";
                            cmd.CommandType = CommandType.StoredProcedure;

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
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead 
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.GetDaywisedetails = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        if (data.GetDaywisedetails != null && data.GetDaywisedetails.Length > 0)
                        {
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
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

                    }


                    var VTADAA_IdList = _IssueContext.TADA_Apply_AuthorisationDMO.Where(R => R.IVRMUL_Id == data.UserId).ToList();
                    List<long> VTADAA_IdC = new List<long>();
                    if (VTADAA_IdList.Count > 0)
                    {
                        foreach (var d in VTADAA_IdList)
                        {
                            VTADAA_IdC.Add(d.VTADAA_Id);
                        }
                    }
                    var applycount = _IssueContext.TADA_ApplyDMO.Where(R => R.VTADAA_StatusFlg == "Pending" &&
                             (R.VTADAA_AppliedDate <= dates) && !VTADAA_IdC.Contains(R.VTADAA_Id)).ToList();
                    if (applycount.Count > 0)
                    {
                        data.GetDaywisedetails = null;
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "TADA_APPLY_APPROVAL";
                            cmd.CommandType = CommandType.StoredProcedure;

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
                                data.GetDaywisedetails = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        if (data.GetDaywisedetails != null && data.GetDaywisedetails.Length > 0)
                        {
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
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

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_DailyReport_ApprovalDTO DeptSelectionDetails(ISM_DailyReport_ApprovalDTO data)
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
                if (data.DRApprovalTypeFlag.Equals("DRMultiApproval"))
                {
                    roletype = "Admin";
                    data.roleflag = false;
                }


                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DesignationList_DR_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.VarChar) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
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
        public ISM_DailyReport_ApprovalDTO DesgSelectionDetails(ISM_DailyReport_ApprovalDTO data)
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

                if (data.DRApprovalTypeFlag.Equals("DRMultiApproval"))
                {
                    roletype = "Admin";
                    data.roleflag = false;
                }


                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DesignationChange_DR_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.VarChar) { Value = data.UserId });
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_DailyReport_ApprovalDTO GetSavedDetails(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                string departments = "0";
                if (data.deptlist.Length > 0)
                {
                    foreach (var ue in data.deptlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }

                string desgination = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.desglist)
                    {
                        desgination = desgination + "," + ue.HRMDES_Id;
                    }
                }

                string emplist = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.emplist)
                    {
                        emplist = emplist + "," + ue.HRME_Id;
                    }
                }

                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Fromdate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Todate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");

                var GET_EMPID = _IssueContext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                long empid = GET_EMPID.Count > 0 ? GET_EMPID.FirstOrDefault().Emp_Code : 0;


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Approval_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@emplist", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

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
                        data.getsaveddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "FO_Emps_WorkingHours";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar) { Value = contodate });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1]);
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.filldata = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "ISM_GET_EMPLOYEE_DEPT_WISE_STATUS_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@emplist", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1]);
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.empwise_statuslist = retObject1.ToArray();
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
        public ISM_DailyReport_ApprovalDTO SaveApprovalDetails(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var gethrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == data.UserId).Distinct().ToList();

                if (gethrmeid.Count() > 0)
                {
                    if (data.selectedemployeedetails.Length > 0)
                    {
                        foreach (var d in data.selectedemployeedetails)
                        {
                            var datacheckdr = _IssueContext.ISM_DailyReport_DaywiseDMO.Where(a => a.HRME_Id == d.HRME_Id
                            && a.ISMDRPTDW_Date == d.fromdate).ToList();

                            if (datacheckdr.Count > 0)
                            {
                                var datacheckresultdr = _IssueContext.ISM_DailyReport_DaywiseDMO.Where(a => a.HRME_Id == d.HRME_Id && a.ISMDRPTDW_Date == d.fromdate).FirstOrDefault();
                                //var datacheckresultdr = _IssueContext.ISM_DailyReport_DaywiseDMO.Single(a => a.HRME_Id == d.HRME_Id && a.ISMDRPTDW_Date == d.fromdate);
                                datacheckresultdr.ISMDRPTDW_OverallComments = d.remarks;
                                datacheckresultdr.ISMDRPTDW_ApprovedEffort = d.approvedefforts;
                                datacheckresultdr.ISMDRPTDW_UpdatedBy = data.UserId;
                                datacheckresultdr.ISMDRPTDW_UpdatedDate = indiantime0;
                                _IssueContext.Update(datacheckresultdr);

                                ISM_DailyReport_Daywise_Multiple_ApprovalDMO obj = new ISM_DailyReport_Daywise_Multiple_ApprovalDMO
                                {
                                    ISMDRPTDW_Id = datacheckdr.FirstOrDefault().ISMDRPTDW_Id,
                                    ISMDRPTDWMAPP_ApprovedEffort = d.approvedefforts,
                                    ISMDRPTDWMAPP_Remarks = d.remarks,
                                    ISMDRPTDWMAPP_ApprovedHRME_Id = gethrmeid.FirstOrDefault().Emp_Code,
                                    ISMDRPTDWMAPP_ActiveFlg = true,
                                    ISMDRPTDWMAPP_CreatedDate = indiantime0,
                                    ISMDRPTDWMAPP_UpdatedDate = indiantime0,
                                    ISMDRPTDWMAPP_CreatedBy = data.UserId,
                                    ISMDRPTDWMAPP_UpdatedBy = data.UserId
                                };
                                _IssueContext.Add(obj);
                            }

                            foreach (var c in d.selectedapprovaldetails)
                            {
                              if(c.ISMMTCAT_TaskCategoryName.ToUpper()=="CON CALL")
                              {
                                   
                                    try
                                    {
                                        var fromdate11 = data.Fromdate.ToString("yyyy-MM-dd");

                                        var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("VMS_Concal_Insert @p0,@p1,@p2,@p3",
                                            c.HRME_Id,c.ConCallApprovedflag, fromdate11, "CON CALL");
                                        if (contactExistsP > 0)
                                        {

                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.InnerException);
                                    }

                              }


                                var result = _IssueContext.ISM_DailyReportGenerationDMO.Where(a => a.ISMDRPT_Id == c.ISMDRPT_Id).FirstOrDefault();
                                // var result = _IssueContext.ISM_DailyReportGenerationDMO.Single(a => a.ISMDRPT_Id == c.ISMDRPT_Id);
                                result.ISMDRPT_ApprovedFlg = c.Approvedflag;
                                result.ISMDRPT_ApprovedTime = c.Efforts;
                                result.ISMDRPT_ApprovedTimeFlg = c.ISMDRPT_TimeTakenInHrsmin;
                                result.UpdatedDate = indiantime0;
                                result.ISMDRPT_UpdatedBy = data.UserId;
                                result.ISMDRPT_Status = c.Approvedflag == false ? "Open" : result.ISMDRPT_Status;

                                _IssueContext.Update(result);

                                ISM_DailyReport_ApprovalDMO iSM_DailyReport_ApprovalDMO = new ISM_DailyReport_ApprovalDMO
                                {
                                    HRME_Id = gethrmeid.FirstOrDefault().Emp_Code,
                                    ISMDRPT_Id = c.ISMDRPT_Id,
                                    ISMDRPTAPP_Remarks = c.Remarks,
                                    ISMDRPTAPP_ActiveFlg = true,
                                    ISMDRPTAPP_CreatedBy = data.UserId,
                                    ISMDRPTAPP_UpdatedBy = data.UserId,
                                    ISMDRPTAPP_CreatedDate = indiantime0,
                                    ISMDRPTAPP_UpdatedDate = indiantime0
                                };

                                _IssueContext.Add(iSM_DailyReport_ApprovalDMO);

                                ISM_DailyReport_Multiple_ApprovalDMO iSM_DailyReport_Multiple_ApprovalDMO = new ISM_DailyReport_Multiple_ApprovalDMO
                                {
                                    HRME_Id = gethrmeid.FirstOrDefault().Emp_Code,
                                    ISMDRPT_Id = c.ISMDRPT_Id,
                                    ISMDRPTMAPP_Remarks = c.Remarks,
                                    ISMDRPTMAPP_ApprovedFlag = c.Approvedflag,
                                    ISMDRPTMAPP_ApprovedEfforts = c.Efforts,
                                    ISMDRPTMAPP_ActiveFlg = true,
                                    ISMDRPTMAPP_CreatedBy = data.UserId,
                                    ISMDRPTMAPP_UpdatedBy = data.UserId,
                                    ISMDRPTMAPP_CreatedDate = indiantime0,
                                    ISMDRPTMAPP_UpdatedDate = indiantime0
                                };
                                _IssueContext.Add(iSM_DailyReport_Multiple_ApprovalDMO);

                                // Updating Task Response Status If Task Is Rejected Or Task_Response is not null or empty
                                if (c.Task_Response != null && c.Task_Response != "")
                                {
                                    //Task Creation Details
                                    var gettaskcreationdetails = _IssueContext.ISM_TaskCreationDMO.Where(a => a.ISMTCR_Id == c.ISMTCR_Id).ToList();
                                    if (gettaskcreationdetails.Count > 0)
                                    {
                                        var resulttaskcreationdetails = _IssueContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == c.ISMTCR_Id);
                                        resulttaskcreationdetails.ISMTCR_Status = c.Task_Response;
                                        resulttaskcreationdetails.UpdatedDate = indiantime0;
                                        resulttaskcreationdetails.ISMTCR_UpdatedBy = data.UserId;
                                        _IssueContext.Update(resulttaskcreationdetails);
                                    }

                                    // Task Creation Response Details
                                    ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO
                                    {
                                        ISMTCR_Id = c.ISMTCR_Id,
                                        HRME_Id = gethrmeid.FirstOrDefault().Emp_Code,
                                        ISMTCRRES_ResponseDate = indiantime0,
                                        ISMTCRRES_Response = c.Remarks,
                                        ISMTCRRES_Status = c.Task_Response,
                                        ISMTCRRES_ResponsePageName = "Daily Report Approval",
                                        ISMTCRRES_ResponseAttachment = "",
                                        ISMTCRRES_ActiveFlg = true,
                                        CreatedDate = indiantime0,
                                        UpdatedDate = indiantime0,
                                        ISMTCRRES_CreatedBy = data.UserId,
                                        ISMTCRRES_UpdatedBy = data.UserId
                                    };
                                    _IssueContext.Add(taskresponse);
                                }

                                /* UPDATING PLANNER TASK DETAILS IF TASK IS REJECTED */
                                if (c.Approvedflag == false)
                                {
                                    var plannerid = result.ISMTPL_Id;
                                    var plannerapprovedid = result.ISMTPLAP_Id;

                                    // Planner Task Details
                                    var getplannertaskdetails = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == c.ISMTCR_Id
                                    && a.ISMTPL_Id == plannerid && a.ISMTPLTA_Id == c.ISMTPLTA_Id).ToList();
                                    if (getplannertaskdetails.Count > 0)
                                    {
                                        var resultplannertaskdetails = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == c.ISMTCR_Id
                                        && a.ISMTPL_Id == plannerid && a.ISMTPLTA_Id == c.ISMTPLTA_Id);
                                        resultplannertaskdetails.ISMTPLTA_Status = "Open";
                                        resultplannertaskdetails.UpdatedDate = indiantime0;
                                        resultplannertaskdetails.ISMTPLTA_UpdatedBy = data.UserId;
                                        _IssueContext.Update(resultplannertaskdetails);
                                    }

                                    // Planner Task Approved Details
                                    var getplannerapprovedid = _IssueContext.ISM_Task_Planner_ApprovedDMO.Where(a => a.ISMTPL_Id == plannerid).ToList();
                                    if (getplannerapprovedid.Count > 0)
                                    {
                                        var getplannerapprovedtaskdetails = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Where(a => a.ISMTCR_Id == c.ISMTCR_Id
                                        && a.ISMTPLAP_Id == getplannerapprovedid.FirstOrDefault().ISMTPLAP_Id && a.ISMTPLTA_Id == c.ISMTPLTA_Id).ToList();

                                        if (getplannerapprovedtaskdetails.Count > 0)
                                        {
                                            var resultplannerapprovedtaskdetails = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Single(a =>
                                            a.ISMTCR_Id == c.ISMTCR_Id && a.ISMTPLAP_Id == getplannerapprovedid.FirstOrDefault().ISMTPLAP_Id
                                            && a.ISMTPLTA_Id == c.ISMTPLTA_Id && a.ISMTPLAPTA_Id == getplannerapprovedtaskdetails.FirstOrDefault().ISMTPLAPTA_Id);

                                            resultplannerapprovedtaskdetails.ISMTPLAPTA_Status = "Open";
                                            resultplannerapprovedtaskdetails.UpdatedDate = indiantime0;
                                            resultplannerapprovedtaskdetails.ISMTPLAPTA_UpdatedBy = data.UserId;
                                            _IssueContext.Update(resultplannerapprovedtaskdetails);
                                        }
                                    }
                                }
                            }
                        }

                        var i = _IssueContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            PushNotification pb = new PushNotification(_Context);
                            foreach (var item in data.selectedemployeedetails)
                            {
                                string dd = pb.savewebnotification(data.MI_Id, "DRAPPROVAL", data.ISMDRPT_Id, data.ISMDRPT_Id, item.HRME_Id, data.UserId, "DRAPP");
                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }

                        foreach (var v in data.selectedemployeedetails)
                        {
                            var getemployeedetails = (from a in _IssueContext.MasterEmployee
                                                      from b in _IssueContext.HR_Master_Department
                                                      from c in _IssueContext.HR_Master_Designation
                                                      from e in _IssueContext.HR_Master_Grade
                                                      where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                                                      && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == v.HRME_Id)
                                                      select new ISM_DailyReport_ApprovalDTO
                                                      {
                                                          employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ?
                                                          "" : a.HRME_EmployeeFirstName)
                                                          + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " "
                                                          + a.HRME_EmployeeMiddleName)
                                                          + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " "
                                                          + a.HRME_EmployeeLastName)).Trim(),
                                                          department = b.HRMD_DepartmentName,
                                                          desgination = c.HRMDES_DesignationName,
                                                          gradename = e.HRMG_GradeName,
                                                          HRMD_Id = a.HRMD_Id,
                                                          MI_Id = a.MI_Id
                                                      }).Distinct().ToList();

                            //*********************************** User Employee Mapped List ********************************// 
                            var notificationuserids = (from UEM in _IssueContext.ISM_User_Employees_MappingDMO
                                                       from HRE in _IssueContext.MasterEmployee
                                                       where (UEM.HRME_Id == HRE.HRME_Id && HRE.HRME_ActiveFlag == true && HRE.HRME_LeftFlag == false
                                                       && UEM.HRME_Id == v.HRME_Id)
                                                       select new ISM_DailyReport_ApprovalDTO
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
                                                       select new ISM_DailyReport_ApprovalDTO
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
                                var devicelist = (from a in _IssueContext.MasterEmployee
                                                  where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id))
                                                  select new ISM_DailyReport_ApprovalDTO
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
                                        if (deviceid.HRME_AppDownloadedDeviceId != null && deviceid.HRME_AppDownloadedDeviceId != "")
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
                                    }
                                    devicenew = "[" + deviceidsnew + "]";
                                }
                            }
                            try
                            {
                                if (data.sound == null || data.sound == "")
                                {
                                    data.sound = "default";
                                }

                                TimeZoneInfo INDIAN_ZONE1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                DateTime indiantime1 = TimeZoneInfo.ConvertTime(data.Fromdate, INDIAN_ZONE1);

                                callnotification(devicenew, v.HRME_Id, indiantime1, getemployeedetails.FirstOrDefault().employeename,
                                    getemployeedetails.FirstOrDefault().MI_Id, data.sound);
                                Insertnotification(getemployeedetails.FirstOrDefault().MI_Id, v.HRME_Id, indiantime1,
                                    getemployeedetails.FirstOrDefault().employeename, data.UserId);
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
        public ISM_DailyReport_ApprovalDTO ViewCheckListSavedDetails(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {      
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_DailyReport_ApprovalDTO viewresponse(ISM_DailyReport_ApprovalDTO data)
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

        /* DR Not Sent Remrks Report Details */
        public ISM_DailyReport_ApprovalDTO GetDRRemarksReport(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                string departments = "0";
                if (data.deptlist.Length > 0)
                {
                    foreach (var ue in data.deptlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }

                string desgination = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.desglist)
                    {
                        desgination = desgination + "," + ue.HRMDES_Id;
                    }
                }

                string emplist = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.emplist)
                    {
                        emplist = emplist + "," + ue.HRME_Id;
                    }
                }

                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Fromdate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Todate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DR_HRPlanner_NotSent_Remarks_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@DEPARTMENTS_Ids", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@DESGINATION_Ids", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@TemplateName", SqlDbType.VarChar) { Value = data.TemplateName });

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
                        data.getdrnotsetreport = retObject.ToArray();
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

        /* DR Not Approved Details */
        public ISM_DailyReport_ApprovalDTO GetDRNotApprovedList(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                long empid = 0;

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().ToArray();

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = rolet.FirstOrDefault().IVRMRT_Role;

                roletype = roletype == null || roletype == "" ? "" : roletype.ToUpper();

                var checkdeptheadcode = _IssueContext.HR_Master_DepartmentCode_Head.Where(a => a.HRME_ID == empid).ToList();

                if (checkdeptheadcode.Count > 0)
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Checking_DailyReport_Approval_GivenOrNot_MonthYearWise";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@MonthId", SqlDbType.VarChar) { Value = data.MonthId });
                        cmd.Parameters.Add(new SqlParameter("@YearId", SqlDbType.VarChar) { Value = data.YearId });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = roletype });

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
                            data.drnotapprovedmessage = retObject.ToArray();
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

        /* DR Multi Approval Details */
        public ISM_DailyReport_ApprovalDTO GetMultiDRSavedDetails(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                long empid = 0;

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().ToArray();

                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = rolet.FirstOrDefault().IVRMRT_Role;

                roletype = roletype == null || roletype == "" ? "" : roletype.ToUpper();


                string departments = "0";
                if (data.deptlist.Length > 0)
                {
                    foreach (var ue in data.deptlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }

                string desgination = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.desglist)
                    {
                        desgination = desgination + "," + ue.HRMDES_Id;
                    }
                }

                string emplist = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.emplist)
                    {
                        emplist = emplist + "," + ue.HRME_Id;
                    }
                }

                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Fromdate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Todate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Multi_Approval_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@emplist", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@ISMDRPT_Id", SqlDbType.VarChar) { Value = data.ISMDRPT_Id });

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
                        data.getsaveddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "FO_Emps_WorkingHours";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar) { Value = contodate });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1]);
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.filldata = retObject1.ToArray();
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
        public ISM_DailyReport_ApprovalDTO ViewApprovalTaskDetails(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                string departments = "0";
                string desgination = "0";
                string emplist = data.HRME_Id.ToString();

                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Fromdate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Todate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Multi_Approval_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@emplist", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@ISMDRPT_Id", SqlDbType.VarChar) { Value = data.ISMDRPT_Id });

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
                        data.gettaskwisecommentsdetails = retObject.ToArray();
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
        public ISM_DailyReport_ApprovalDTO SaveDRMultiApprovalDetails(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                var gethrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == data.UserId).Distinct().ToList();

                if (gethrmeid.Count() > 0)
                {
                    if (data.selectedemployeedetails.Length > 0)
                    {
                        foreach (var d in data.selectedemployeedetails)
                        {
                            var datacheckdr = _IssueContext.ISM_DailyReport_DaywiseDMO.Where(a => a.HRME_Id == d.HRME_Id
                            && a.ISMDRPTDW_Date == d.fromdate).ToList();

                            ISM_DailyReport_Daywise_Multiple_ApprovalDMO obj = new ISM_DailyReport_Daywise_Multiple_ApprovalDMO();

                            obj.ISMDRPTDW_Id = datacheckdr.FirstOrDefault().ISMDRPTDW_Id;
                            obj.ISMDRPTDWMAPP_ApprovedEffort = d.approvedefforts;
                            obj.ISMDRPTDWMAPP_Remarks = d.remarks;
                            obj.ISMDRPTDWMAPP_ApprovedHRME_Id = gethrmeid.FirstOrDefault().Emp_Code;
                            obj.ISMDRPTDWMAPP_ActiveFlg = true;
                            obj.ISMDRPTDWMAPP_CreatedDate = indiantime0;
                            obj.ISMDRPTDWMAPP_UpdatedDate = indiantime0;
                            obj.ISMDRPTDWMAPP_CreatedBy = data.UserId;
                            obj.ISMDRPTDWMAPP_UpdatedBy = data.UserId;
                            _IssueContext.Add(obj);

                            foreach (var c in d.selectedapprovaldetails)
                            {
                                ISM_DailyReport_Multiple_ApprovalDMO iSM_DailyReport_Multiple_ApprovalDMO = new ISM_DailyReport_Multiple_ApprovalDMO();
                                iSM_DailyReport_Multiple_ApprovalDMO.HRME_Id = gethrmeid.FirstOrDefault().Emp_Code;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPT_Id = c.ISMDRPT_Id;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPTMAPP_Remarks = c.Remarks;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPTMAPP_ApprovedFlag = c.Approvedflag;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPTMAPP_ApprovedEfforts = c.Efforts;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPTMAPP_ActiveFlg = true;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPTMAPP_CreatedBy = data.UserId;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPTMAPP_UpdatedBy = data.UserId;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPTMAPP_CreatedDate = indiantime0;
                                iSM_DailyReport_Multiple_ApprovalDMO.ISMDRPTMAPP_UpdatedDate = indiantime0;
                                _IssueContext.Add(iSM_DailyReport_Multiple_ApprovalDMO);
                            }
                        }

                        var i = _IssueContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
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
        public ISM_DailyReport_ApprovalDTO ViewOverAllComments(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                var GetDaywisedetails = (from a in _IssueContext.ISM_DailyReport_Daywise_Multiple_ApprovalDMO
                                         from b in _IssueContext.ISM_DailyReport_DaywiseDMO
                                         from c in _IssueContext.MasterEmployee
                                         where (a.ISMDRPTDW_Id == b.ISMDRPTDW_Id && a.ISMDRPTDWMAPP_ApprovedHRME_Id == c.HRME_Id
                                         && b.HRME_Id == data.HRME_Id && b.ISMDRPTDW_Date.Value.Date == data.Fromdate.Date)
                                         select new ISM_DailyReport_ApprovalDTO
                                         {
                                             ISMDRPTDWMAPP_Remarks = a.ISMDRPTDWMAPP_Remarks,
                                             ISMDRPTDWMAPP_ApprovedEffort = a.ISMDRPTDWMAPP_ApprovedEffort,
                                             ISMDRPTDWMAPP_ApprovedHRME_Id = a.ISMDRPTDWMAPP_ApprovedHRME_Id,
                                             employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : c.HRME_EmployeeFirstName) +
                                             (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" ? "" : " " + c.HRME_EmployeeMiddleName) +
                                             (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" ? "" : " " + c.HRME_EmployeeLastName)),
                                             ISMDRPTDWMAPP_CreatedDate = a.ISMDRPTDWMAPP_CreatedDate
                                         }).Distinct().ToArray();

                data.GetDaywisedetails = GetDaywisedetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* DR Regenerate approval */

        public ISM_DailyReport_ApprovalDTO SaveRegenerateDRApproval(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var gethrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == data.UserId).Distinct().ToList();

                if (gethrmeid.Count() > 0)
                {
                    if (data.selectedemployeedetails.Length > 0)
                    {
                        foreach (var d in data.selectedemployeedetails)
                        {
                            var datacheckdr = _IssueContext.ISM_DailyReport_DaywiseDMO.Where(a => a.HRME_Id == d.HRME_Id
                            && a.ISMDRPTDW_Date == d.fromdate).ToList();

                            if (datacheckdr.Count > 0)
                            {
                                var datacheckresultdr = _IssueContext.ISM_DailyReport_DaywiseDMO.Where(a => a.HRME_Id == d.HRME_Id && a.ISMDRPTDW_Date == d.fromdate).FirstOrDefault();
                                //var datacheckresultdr = _IssueContext.ISM_DailyReport_DaywiseDMO.Single(a => a.HRME_Id == d.HRME_Id && a.ISMDRPTDW_Date == d.fromdate);
                                datacheckresultdr.ISMDRPTDW_OverallComments = d.remarks;
                                datacheckresultdr.ISMDRPTDW_ApprovedEffort = d.approvedefforts;
                                datacheckresultdr.ISMDRPTDW_UpdatedBy = data.UserId;
                                datacheckresultdr.ISMDRPTDW_UpdatedDate = indiantime0;
                                _IssueContext.Update(datacheckresultdr);

                                ISM_DailyReport_Daywise_Multiple_ApprovalDMO obj = new ISM_DailyReport_Daywise_Multiple_ApprovalDMO
                                {
                                    ISMDRPTDW_Id = datacheckdr.FirstOrDefault().ISMDRPTDW_Id,
                                    ISMDRPTDWMAPP_ApprovedEffort = d.approvedefforts,
                                    ISMDRPTDWMAPP_Remarks = d.remarks,
                                    ISMDRPTDWMAPP_ApprovedHRME_Id = gethrmeid.FirstOrDefault().Emp_Code,
                                    ISMDRPTDWMAPP_ActiveFlg = true,
                                    ISMDRPTDWMAPP_CreatedDate = indiantime0,
                                    ISMDRPTDWMAPP_UpdatedDate = indiantime0,
                                    ISMDRPTDWMAPP_CreatedBy = data.UserId,
                                    ISMDRPTDWMAPP_UpdatedBy = data.UserId
                                };
                                _IssueContext.Add(obj);
                            }

                            foreach (var c in d.selectedapprovaldetails)
                            {
                                var result = _IssueContext.ISM_DailyReportGenerationDMO.Where(a => a.ISMDRPT_Id == c.ISMDRPT_Id).FirstOrDefault();

                                // var result = _IssueContext.ISM_DailyReportGenerationDMO.Single(a => a.ISMDRPT_Id == c.ISMDRPT_Id);
                                result.ISMDRPT_ApprovedFlg = c.Approvedflag;
                                //if (c.ISMDRPT_ApprovedTime == null)
                                //{
                                //    result.ISMDRPT_ApprovedTime = c.Efforts + Convert.ToDecimal(0.0);
                                //}
                                //else
                                //{
                                //    result.ISMDRPT_ApprovedTime = c.Efforts ;
                                //}
                                result.ISMDRPT_ApprovedTime = c.ISMDRPT_ApprovedTime + c.Efforts;
                                result.ISMDRPT_ApprovedTimeFlg = c.ISMDRPT_TimeTakenInHrsmin;
                                result.UpdatedDate = indiantime0;
                                result.ISMDRPT_UpdatedBy = data.UserId;
                                result.ISMDRPT_Status = c.Approvedflag == false ? "Open" : result.ISMDRPT_Status;

                                _IssueContext.Update(result);

                                var checkDRReport = _IssueContext.ISM_DailyReport_ApprovalDMO.Where(a => a.HRME_Id == gethrmeid.FirstOrDefault().Emp_Code
                       && a.ISMDRPT_Id == c.ISMDRPT_Id).ToList();

                                if (checkDRReport.Count > 0)
                                {
                                    var resultcheckDRReport = _IssueContext.ISM_DailyReport_ApprovalDMO.Single(a => a.HRME_Id == gethrmeid.FirstOrDefault().Emp_Code && a.ISMDRPT_Id == c.ISMDRPT_Id);
                                    resultcheckDRReport.ISMDRPTAPP_Remarks = c.Remarks;
                                    resultcheckDRReport.ISMDRPTAPP_UpdatedBy = data.UserId;
                                    _IssueContext.Update(resultcheckDRReport);
                                }
                                else
                                {
                                    ISM_DailyReport_ApprovalDMO iSM_DailyReport_ApprovalDMO = new ISM_DailyReport_ApprovalDMO();
                                    iSM_DailyReport_ApprovalDMO.HRME_Id = gethrmeid.FirstOrDefault().Emp_Code;
                                    iSM_DailyReport_ApprovalDMO.ISMDRPT_Id = c.ISMDRPT_Id;
                                    iSM_DailyReport_ApprovalDMO.ISMDRPTAPP_Remarks = c.Remarks;
                                    iSM_DailyReport_ApprovalDMO.ISMDRPTAPP_ActiveFlg = true;
                                    iSM_DailyReport_ApprovalDMO.ISMDRPTAPP_CreatedBy = data.UserId;
                                    iSM_DailyReport_ApprovalDMO.ISMDRPTAPP_UpdatedBy = data.UserId;
                                    iSM_DailyReport_ApprovalDMO.ISMDRPTAPP_CreatedDate = indiantime0;
                                    iSM_DailyReport_ApprovalDMO.ISMDRPTAPP_UpdatedDate = indiantime0;

                                    _IssueContext.Add(iSM_DailyReport_ApprovalDMO);
                                }

                                if (result.ISMDRPT_Id > 0)
                                {
                                    var result123 = _IssueContext.ISM_DailyReportReapprovalDMO.Where(a => a.ISMDRPT_Id == c.ISMDRPT_Id).ToList();
                                    if (result123.Count > 0)
                                    {
                                        var finalvalue = _IssueContext.ISM_DailyReportReapprovalDMO.Single(a =>  a.ISMDRPT_Id == c.ISMDRPT_Id);
                                        finalvalue.HRME_Id = gethrmeid.FirstOrDefault().Emp_Code;
                                        finalvalue.ISMDRPTR_Date = result.ISMDRPT_Date;
                                        finalvalue.ISMDRPTR_Remarks = result.ISMDRPT_Remarks;
                                        finalvalue.ISMDRPTR_ActiveFlg = result.ISMDRPT_ActiveFlg;
                                        finalvalue.ISMDRPTR_TimeTakenFlg = result.ISMDRPT_TimeTakenFlg;
                                        finalvalue.ISMDRPTR_ApprovedFlg = result.ISMDRPT_ApprovedFlg;
                                        finalvalue.ISMDRPTR_ApprovedTime =  c.Efforts;
                                        finalvalue.ISMDRPTR_ApprovedTimeFlg = result.ISMDRPT_ApprovedTimeFlg;
                                        finalvalue.ISMDRPTR_UpdatedBy = data.UserId;
                                        finalvalue.UpdatedDate = indiantime0;
                                        _IssueContext.Update(finalvalue);
                                    }


                                    else
                                    {
                                        ISM_DailyReportReapprovalDMO ISM_DailyReportReapprovalDMO = new ISM_DailyReportReapprovalDMO();

                                        ISM_DailyReportReapprovalDMO.HRME_Id = gethrmeid.FirstOrDefault().Emp_Code;
                                        ISM_DailyReportReapprovalDMO.ISMDRPT_Id = c.ISMDRPT_Id;
                                        ISM_DailyReportReapprovalDMO.ISMDRPTR_Remarks = c.Remarks;
                                        ISM_DailyReportReapprovalDMO.ISMDRPTR_ApprovedFlg = c.Approvedflag;
                                        ISM_DailyReportReapprovalDMO.ISMDRPTR_ApprovedTime = c.Efforts;
                                        ISM_DailyReportReapprovalDMO.ISMDRPTR_TimeTakenInHrs = result.ISMDRPT_TimeTakenInHrs;
                                        ISM_DailyReportReapprovalDMO.ISMDRPTR_ActiveFlg = true;
                                        ISM_DailyReportReapprovalDMO.ISMDRPTR_CreatedBy = data.UserId;
                                        ISM_DailyReportReapprovalDMO.ISMDRPTR_UpdatedBy = data.UserId;
                                        ISM_DailyReportReapprovalDMO.CreatedDate = indiantime0;
                                        ISM_DailyReportReapprovalDMO.UpdatedDate = indiantime0;

                                        _IssueContext.Add(ISM_DailyReportReapprovalDMO);
                                    }
                                }

                                //Regenearte approval 

                                //var regenerate = _IssueContext.ISM_DailyReportReapprovalDMO.Where(a => a.ISMDRPT_Id == c.ISMDRPT_Id).ToList();
                                //if (regenerate.Count > 0)
                                //{
                                //    var regeneratevalue = _IssueContext.ISM_DailyReportGenerationDMO.Single(a =>  a.ISMDRPT_Id == c.ISMDRPT_Id);
                                //    regeneratevalue.ISMDRPT_ApprovedTime = result.ISMDRPT_ApprovedTime;
                                //    regeneratevalue.ISMDRPT_TimeTakenInHrs = result.ISMDRPT_TimeTakenInHrs;
                                //    _IssueContext.Update(regeneratevalue);
                                //}


                                ISM_DailyReport_Multiple_ApprovalDMO iSM_DailyReport_Multiple_ApprovalDMO = new ISM_DailyReport_Multiple_ApprovalDMO
                                {
                                    HRME_Id = gethrmeid.FirstOrDefault().Emp_Code,
                                    ISMDRPT_Id = c.ISMDRPT_Id,
                                    ISMDRPTMAPP_Remarks = c.Remarks,
                                    ISMDRPTMAPP_ApprovedFlag = c.Approvedflag,
                                    ISMDRPTMAPP_ApprovedEfforts = c.Efforts,
                                    ISMDRPTMAPP_ActiveFlg = true,
                                    ISMDRPTMAPP_CreatedBy = data.UserId,
                                    ISMDRPTMAPP_UpdatedBy = data.UserId,
                                    ISMDRPTMAPP_CreatedDate = indiantime0,
                                    ISMDRPTMAPP_UpdatedDate = indiantime0
                                };
                                _IssueContext.Add(iSM_DailyReport_Multiple_ApprovalDMO);

                                // Updating Task Response Status If Task Is Rejected Or Task_Response is not null or empty
                                if (c.Task_Response != null && c.Task_Response != "")
                                {
                                    //Task Creation Details
                                    var gettaskcreationdetails = _IssueContext.ISM_TaskCreationDMO.Where(a => a.ISMTCR_Id == c.ISMTCR_Id).ToList();
                                    if (gettaskcreationdetails.Count > 0)
                                    {
                                        var resulttaskcreationdetails = _IssueContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == c.ISMTCR_Id);
                                        resulttaskcreationdetails.ISMTCR_Status = c.Task_Response;
                                        resulttaskcreationdetails.UpdatedDate = indiantime0;
                                        resulttaskcreationdetails.ISMTCR_UpdatedBy = data.UserId;
                                        _IssueContext.Update(resulttaskcreationdetails);
                                    }

                                    // Task Creation Response Details
                                    ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO
                                    {
                                        ISMTCR_Id = c.ISMTCR_Id,
                                        HRME_Id = gethrmeid.FirstOrDefault().Emp_Code,
                                        ISMTCRRES_ResponseDate = indiantime0,
                                        ISMTCRRES_Response = c.Remarks,
                                        ISMTCRRES_Status = c.Task_Response,
                                        ISMTCRRES_ResponsePageName = "Daily Report Approval",
                                        ISMTCRRES_ResponseAttachment = "",
                                        ISMTCRRES_ActiveFlg = true,
                                        CreatedDate = indiantime0,
                                        UpdatedDate = indiantime0,
                                        ISMTCRRES_CreatedBy = data.UserId,
                                        ISMTCRRES_UpdatedBy = data.UserId
                                    };
                                    _IssueContext.Add(taskresponse);
                                }

                                /* UPDATING PLANNER TASK DETAILS IF TASK IS REJECTED */
                                if (c.Approvedflag == false)
                                {
                                    var plannerid = result.ISMTPL_Id;
                                    var plannerapprovedid = result.ISMTPLAP_Id;

                                    // Planner Task Details
                                    var getplannertaskdetails = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == c.ISMTCR_Id
                                    && a.ISMTPL_Id == plannerid && a.ISMTPLTA_Id == c.ISMTPLTA_Id).ToList();
                                    if (getplannertaskdetails.Count > 0)
                                    {
                                        var resultplannertaskdetails = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == c.ISMTCR_Id
                                        && a.ISMTPL_Id == plannerid && a.ISMTPLTA_Id == c.ISMTPLTA_Id);
                                        resultplannertaskdetails.ISMTPLTA_Status = "Open";
                                        resultplannertaskdetails.UpdatedDate = indiantime0;
                                        resultplannertaskdetails.ISMTPLTA_UpdatedBy = data.UserId;
                                        _IssueContext.Update(resultplannertaskdetails);
                                    }

                                    // Planner Task Approved Details
                                    var getplannerapprovedid = _IssueContext.ISM_Task_Planner_ApprovedDMO.Where(a => a.ISMTPL_Id == plannerid).ToList();
                                    if (getplannerapprovedid.Count > 0)
                                    {
                                        var getplannerapprovedtaskdetails = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Where(a => a.ISMTCR_Id == c.ISMTCR_Id
                                        && a.ISMTPLAP_Id == getplannerapprovedid.FirstOrDefault().ISMTPLAP_Id && a.ISMTPLTA_Id == c.ISMTPLTA_Id).ToList();

                                        if (getplannerapprovedtaskdetails.Count > 0)
                                        {
                                            var resultplannerapprovedtaskdetails = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Single(a =>
                                            a.ISMTCR_Id == c.ISMTCR_Id && a.ISMTPLAP_Id == getplannerapprovedid.FirstOrDefault().ISMTPLAP_Id
                                            && a.ISMTPLTA_Id == c.ISMTPLTA_Id && a.ISMTPLAPTA_Id == getplannerapprovedtaskdetails.FirstOrDefault().ISMTPLAPTA_Id);

                                            resultplannerapprovedtaskdetails.ISMTPLAPTA_Status = "Open";
                                            resultplannerapprovedtaskdetails.UpdatedDate = indiantime0;
                                            resultplannerapprovedtaskdetails.ISMTPLAPTA_UpdatedBy = data.UserId;
                                            _IssueContext.Update(resultplannerapprovedtaskdetails);
                                        }
                                    }
                                }

                            }


                        }

                        var i = _IssueContext.SaveChanges();

                        //var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("DR_Reapproval_TimeUpddate @p0", 476387);
                        //if (contactExistsP > 0)
                        //{
                        //    data.returnval = true;
                        //}
                        //else
                        //{
                        //    data.returnval = false;

                        //}
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_DailyReport_ApprovalDTO GetSavedDetails1(ISM_DailyReport_ApprovalDTO data)
        {
            try
            {
                string departments = "0";
                if (data.deptlist.Length > 0)
                {
                    foreach (var ue in data.deptlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }

                string desgination = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.desglist)
                    {
                        desgination = desgination + "," + ue.HRMDES_Id;
                    }
                }

                string emplist = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.emplist)
                    {
                        emplist = emplist + "," + ue.HRME_Id;
                    }
                }

                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Fromdate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Todate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");

                var GET_EMPID = _IssueContext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                long empid = GET_EMPID.Count > 0 ? GET_EMPID.FirstOrDefault().Emp_Code : 0;


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Approval_Details_Modify_RE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@emplist", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

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
                        data.getsaveddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "FO_Emps_WorkingHours";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar) { Value = contodate });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1]);
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.filldata = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "ISM_GET_EMPLOYEE_DEPT_WISE_STATUS_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@emplist", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1]);
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.empwise_statuslist = retObject1.ToArray();
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


        // MOBILE NOTIFICATION
        public string callnotification(string devicenew, long empid, DateTime? ISMDRPT_Date, string empname, long mi_id, string sound)
        {
            try
            {
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                string confromdate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(ISMDRPT_Date), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Your Daily Report Date " + confromdate + "  " + "Approved By Your Team Lead " + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";




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
                                           where (UEM.HRME_Id == HRE.HRME_Id && HRE.HRME_ActiveFlag == true && HRE.HRME_LeftFlag == false && HRE.HRME_ActiveFlag == true && HRE.HRMD_Id == HMD.HRMD_Id && HMD.HRMD_ActiveFlag == true && UEM.HRME_Id == empid)
                                           select new ISM_DailyReportGenerationDTO
                                           {
                                               HRME_Id = UEM.HRME_Id,
                                           }).Distinct().ToList();

                string confromdate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(ISMDRPT_Date), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");


                var notification = "Your Daily Report Date " + confromdate + " Approved By Team Lead ";
                TimeZoneInfo INDIAN_ZONE1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE1);

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
                        push.ISMNO_NotificationType = "DRApproval";
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
    }
}