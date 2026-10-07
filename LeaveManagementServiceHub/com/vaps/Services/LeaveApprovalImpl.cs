using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveApprovalImpl : LeaveApprovalInterface
    {
        public DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveReportImpl> _log;
        public LMContext _lmContext;
        public LeaveApprovalImpl(LMContext ttcategory, DomainModelMsSqlServerContext abc, ILogger<LeaveReportImpl> _logger)
        {
            _lmContext = ttcategory;
            _db = abc;
            _log = _logger;
        }

        public async Task<LeaveCreditDTO> getApprovalStatus(LeaveCreditDTO data)
        {
            List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
            leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveCreditFlg == true).ToList();
            data.leave_name = leave_name.Distinct().ToArray();

            data.get_emp = (from x in _lmContext.HR_Master_Employee_DMO
                            where (x.MI_Id == data.MI_Id && x.HRME_ActiveFlag == true)
                            select new HR_Master_Employee_DMO
                            {
                                HRME_Id = x.HRME_Id,
                                HRME_EmployeeFirstName = x.HRME_EmployeeFirstName
                            }).Distinct().ToArray();


            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "VMS_LeaveApproval";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                {
                    Value = data.LoginId
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
                    data.get_leavestatus = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "employeeLeavesDetails";
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
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.employeeleavedetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }





            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_Leave_Approval_Grid";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                {
                    Value = data.LoginId
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




            return data;
        }

        public async Task<LeaveCreditDTO> leaveApprovalStatusAsync(LeaveCreditDTO data)
        {
            LeaveCreditDTO load = new LeaveCreditDTO();
            try
            {
                data.get_leavestatus = (from a in _lmContext.HR_Emp_Leave_ApplicationDMO
                                        from b in _lmContext.HR_Emp_Leave_Trans_DMO
                                        from c in _lmContext.HR_Master_Employee_DMO
                                        from d in _lmContext.HR_Master_Leave_DMO
                                        from e in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                        from f in _lmContext.Staff_User_Login
                                        from g in _lmContext.HR_Leave_Authorisation_DMO
                                        where (f.Id == data.LoginId && f.Emp_Code == e.IVRMUL_Id && b.HRME_Id == e.HRME_Id && a.HRELAP_FromDate == b.HRELT_FromDate && a.HRELAP_ToDate == b.HRELT_ToDate && a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && a.HRME_Id == c.HRME_Id && c.HRME_ActiveFlag == true && c.MI_Id == data.MI_Id && b.HRELT_LeaveId == d.HRML_Id && d.MI_Id == data.MI_Id && e.HRLA_Id == g.HRLA_Id && g.HRMDES_Id == c.HRMDES_Id && g.HRMD_Id == c.HRMD_Id && g.HRMGT_Id == c.HRMGT_Id && g.HRMG_Id == c.HRMG_Id && g.HRML_Id == b.HRELT_LeaveId && f.MI_Id == data.MI_Id && a.HRELAP_ApplicationStatus != "Approved" && a.HRELAP_ApplicationStatus != "Rejected" && a.HRELAP_ActiveFlag == true && b.HRELT_ActiveFlag == true)
                                        select new LeaveCreditDTO
                                        {
                                            HRELAP_Id = a.HRELAP_Id,
                                            HRME_EmployeeFirstName = c.HRME_EmployeeFirstName,
                                            HRML_LeaveType = d.HRML_LeaveName,
                                            HRELAP_ApplicationID = a.HRELAP_ApplicationID,
                                            HRELAP_FromDate = a.HRELAP_FromDate,
                                            HRELAP_ToDate = a.HRELAP_ToDate,
                                            HRELAP_TotalDays = a.HRELAP_TotalDays,
                                            HRELAP_ReportingDate = a.HRELAP_ReportingDate,
                                            HRELAP_LeaveReason = a.HRELAP_LeaveReason,
                                            HRME_Id = c.HRME_Id,
                                            HRML_Id = d.HRML_Id
                                        }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Leave Approval  error");
                _log.LogDebug(ex.Message);
            }
            return data;
        }

        public async Task<LeaveCreditDTO.LeaveApprovalDTO> get_status(LeaveCreditDTO.LeaveApprovalDTO data)
         {
            string status = "Approved";
            string sectionlevel = "1";
            long leave_year = 0;

            try
            {
                if (data.get_leave_status != null)
                {
                    for (var i = 0; i < data.get_leave_status.Count(); i++)
                    {
                        var temp_id = data.get_leave_status[i].HRELAP_ApplicationID;
                        var temp_leave_res = data.get_leave_status[i].HRELAP_LeaveReason;
                        var HRELAPA_Remarks = data.get_leave_status[i].HRELAPA_Remarks;

                        data.MI_Id = _lmContext.HR_Master_Employee_DMO.Where(e => e.HRME_Id == data.get_leave_status[i].HRME_Id).Select(t => t.MI_Id).FirstOrDefault();

                        leave_year = _lmContext.HR_Master_LeaveYear_DMO.Where(e => e.MI_Id == data.MI_Id && data.get_leave_status[i].HRELAP_FromDateapprv.Date >= e.HRMLY_FromDate.Date && data.get_leave_status[i].HRELAP_ToDateapprv.Date <= e.HRMLY_ToDate.Date && e.HRMLY_ActiveFlag == true).Select(t => t.HRMLY_Id).FirstOrDefault();

                        var checkbalanceleave = _lmContext.HR_Emp_Leave_StatusDMO.Where(d => d.MI_Id == data.MI_Id && d.HRME_Id == data.get_leave_status[i].HRME_Id && d.HRML_Id == data.get_leave_status[i].HRML_Id && d.HRMLY_Id == leave_year).ToList();

                        if (checkbalanceleave.Count > 0 )
                        {
                            var bal_leave = checkbalanceleave.FirstOrDefault().HRELS_CBLeaves; //- data.get_leave_status[i].HRELAP_TotalDaysapprv;

                            if (bal_leave <= 0 && data.status=="Approved")
                            {
                                data.message = "Balance Crossed";
                                return data;
                            }
                        }
                        else if ((checkbalanceleave.Count == 0 && data.status == "Approved"))
                        {
                            data.message = "Leave Is Not Credited";
                            return data;
                        }

                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "leave_approval_status_VMS_UserWise";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRELAP_ApplicationID", SqlDbType.VarChar) { Value = temp_id });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = data.status });                           
                            cmd.Parameters.Add(new SqlParameter("@LoginID", SqlDbType.Int) { Value = data.LoginId });
                            cmd.Parameters.Add(new SqlParameter("@Remarks", SqlDbType.VarChar) { Value = HRELAPA_Remarks });
                            cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.VarChar) { Value = temp_leave_res });
                            cmd.Parameters.Add(new SqlParameter("@EmpHRME_Id", SqlDbType.BigInt) { Value = data.get_leave_status[i].HRME_Id });
                            cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                            {
                                Value = data.get_leave_status[i].HRELAP_FromDateapprv.ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                            {
                                Value = data.get_leave_status[i].HRELAP_ToDateapprv.ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@ApprovedDays", SqlDbType.VarChar)
                            {
                                Value = data.get_leave_status[i].HRELAP_TotalDaysapprv
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

                        var leaveDetails = (from x in _lmContext.HR_Master_Leave_DMO
                                            from y in _lmContext.HR_Emp_Leave_StatusDMO
                                            from z in _lmContext.HR_Emp_Leave_Trans_Details_DMO
                                            from a in _lmContext.HR_Emp_Leave_Trans_DMO
                                            where (x.HRML_Id == y.HRML_Id && x.MI_Id == data.MI_Id && x.HRML_LeaveCreditFlg == true
                                            && y.HRME_Id == data.get_leave_status[i].HRME_Id && z.MI_Id == data.MI_Id
                                            && z.HRME_Id == data.get_leave_status[i].HRME_Id && z.HRELTD_FromDate == data.get_leave_status[i].HRELAP_FromDate
                                            && z.HRELTD_ToDate == data.get_leave_status[i].HRELAP_ToDate
                                            && z.HRELT_Id == a.HRELT_Id && a.HRELT_ActiveFlag == true
                                            && z.HRELTD_TotDays == data.get_leave_status[i].HRELAP_TotalDays && y.HRML_Id == z.HRML_Id)
                                            select new LeaveCreditDTO
                                            {
                                                HRMLY_Id = y.HRMLY_Id,
                                                HRML_Id = z.HRML_Id,
                                                HRELT_Id = z.HRELT_Id,
                                                HRELTD_Id = z.HRELTD_Id,
                                                HRML_LeaveName = x.HRML_LeaveName
                                            }).Distinct().ToArray();

                        long HRELAPId = Convert.ToInt64(temp_id);

                        var chkfinalflg = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_Id == HRELAPId && t.HRME_Id == data.get_leave_status[i].HRME_Id).FirstOrDefault();


                        bool approvallevel = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                              from b in _lmContext.Staff_User_Login
                                              where ( a.IVRMUL_Id == b.Emp_Code && b.Id == data.LoginId)
                                              select a.HRLAON_FinalFlg).FirstOrDefault();


                        string approvarname = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                               from b in _lmContext.Staff_User_Login
                                               where ( a.IVRMUL_Id == b.Emp_Code && b.Id == data.LoginId)
                                               select b.IVRMSTAUL_UserName).FirstOrDefault();



                        //var leavename = _lmContext.HR_Master_Leave_DMO.Where(d => d.MI_Id == data.MI_Id && d.HRML_Id == leaveDetails[0].HRML_Id).Select(t => t.HRML_LeaveName).FirstOrDefault();


                        string emailid = "";
                        string Template = "";
                        var ccemailid = "";


                        if (chkfinalflg.HRELAP_FinalFlag == true)
                        {



                            emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == data.get_leave_status[i].HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                            long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == data.get_leave_status[i].HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();
                            Template = "Leave_Approved";
                        }
                        else
                        {
                            long findlevel = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                              from b in _lmContext.Staff_User_Login
                                              where (a.HRME_Id == data.get_leave_status[i].HRME_Id && a.IVRMUL_Id == b.Emp_Code && b.Id == data.LoginId)
                                              select a.HRLAON_SanctionLevelNo).FirstOrDefault();




                            var deptlist = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.get_leave_status[i].HRME_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList().FirstOrDefault();



                            emailid = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                       from b in _lmContext.HR_Leave_Authorisation_DMO
                                       from d in _lmContext.Emp_Email_Id
                                       where (d.HRME_Id == a.IVRMUL_Id && a.HRLA_Id == b.HRLA_Id
                                       && b.HRMD_Id == deptlist.HRMD_Id && b.HRMDES_Id == deptlist.HRMDES_Id && b.HRMGT_Id == deptlist.HRMGT_Id && b.HRMG_Id == deptlist.HRMG_Id && d.HRMEM_DeFaultFlag == "default"
                                       && b.HRML_Id == leaveDetails[0].HRML_Id && a.HRLAON_SanctionLevelNo == (findlevel + 1))
                                       select d.HRMEM_EmailId).FirstOrDefault();

                            ccemailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == data.get_leave_status[i].HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                            Template = "Leave_Approved";
                        }

                        approvarname = _lmContext.applicationUser.Where(t => t.Id == data.LoginId).Select(t => t.UserName).FirstOrDefault();
                        //emailid = "praveenkumar@globalqtytrainig.com";
                        if (emailid != null && emailid.Length > 0)
                        {

                            // sendmail(data.MI_Id, emailid, ccemailid, approvarname, leavename, data.get_leave_status[i].HRELAP_LeaveReason, Template, data.get_leave_status[i].HRME_Id, data.get_leave_status[i].HRELAP_FromDate);
                            sendEmail(data.MI_Id, Template, chkfinalflg.HRELAP_Id, emailid, ccemailid);
                            //sendEmail(data.MI_Id, Template, 7078, emailid, "");
                        }


                        var devicenew = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.get_leave_status[i].HRME_Id).Select(t => t.HRME_AppDownloadedDeviceId).FirstOrDefault();
                        data.AMST_AppDownloadedDeviceId = devicenew;



                    }
                }
                data.message = "Success";
            }

            catch (Exception ex)
            {
                _log.LogInformation("Leave Approval  error");
                _log.LogDebug(ex.Message);
                data.message = "False";
            }
            return data;
        }

        public async Task<LeaveCreditDTO.LeaveApprovalDTO> reject_status(LeaveCreditDTO.LeaveApprovalDTO data)
        {
            string status = "Rejected";
            string sectionlevel = "1";
            long hrmeId = 0;
            try
            {
                if (data.get_leave_status != null)
                {
                    for (var i = 0; i < data.get_leave_status.Count(); i++)
                    {
                        var temp_id = data.get_leave_status[i].HRELAP_ApplicationID;
                        var temp_leave_res = data.get_leave_status[i].HRELAP_LeaveReason;
                        var HRELAPA_Remarks = data.get_leave_status[i].HRELAPA_Remarks;


                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "leave_approval_status_VMS_UserWise";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRELAP_ApplicationID", SqlDbType.VarChar) { Value = temp_id });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = status });
                            //cmd.Parameters.Add(new SqlParameter("@HRELAP_SanctioningLevel",SqlDbType.Int)
                            //{
                            //    Value = sectionlevel
                            //});
                            cmd.Parameters.Add(new SqlParameter("@LoginID", SqlDbType.Int) { Value = data.LoginId });
                            cmd.Parameters.Add(new SqlParameter("@Remarks", SqlDbType.VarChar) { Value = HRELAPA_Remarks });
                            cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.VarChar) { Value = temp_leave_res });
                            cmd.Parameters.Add(new SqlParameter("@EmpHRME_Id", SqlDbType.BigInt) { Value = data.get_leave_status[i].HRME_Id });
                            cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                            {
                                Value = data.get_leave_status[i].HRELAP_FromDateapprv.Date.ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                            {
                                Value = data.get_leave_status[i].HRELAP_ToDateapprv.Date.ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@ApprovedDays", SqlDbType.VarChar)
                            {
                                Value = data.get_leave_status[i].HRELAP_TotalDaysapprv
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
                                    data.activityIds = retObject.ToArray();

                                }
                                if (retObject.Count > 0)
                                {
                                    hrmeId = 0;
                                    long hrmL_Id = 0;
                                    decimal hrelaP_TotalDays = 0;
                                    foreach (ApprovalStatus item in data.get_leave_status)
                                    {
                                        hrmeId = item.HRME_Id;
                                        hrmL_Id = item.HRML_Id;
                                        hrelaP_TotalDays = item.HRELAP_TotalDays;
                                    }
                                    var updatleave = _lmContext.HR_Emp_Leave_StatusDMO.Single(d => d.MI_Id == data.MI_Id && d.HRME_Id == hrmeId && d.HRML_Id == hrmL_Id);
                                    //updatleave.HRELS_CBLeaves = updatleave.HRELS_CBLeaves + hrelaP_TotalDays;
                                    _lmContext.Update(updatleave);
                                    _lmContext.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            // leaveApprovalStatus(data);
                        }
                        string Emailcc = "";
                        string approvarname = "";
                        string leavename = "";


                        approvarname = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                        from b in _lmContext.Staff_User_Login
                                        where (a.HRME_Id == data.get_leave_status[i].HRME_Id && a.IVRMUL_Id == b.Emp_Code && b.Id == data.LoginId)
                                        select b.IVRMSTAUL_UserName).FirstOrDefault();

                        leavename = _lmContext.HR_Master_Leave_DMO.Where(d => d.HRML_Id == data.get_leave_status[0].HRML_Id).Select(t => t.HRML_LeaveName).FirstOrDefault();


                        string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == hrmeId && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == hrmeId && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();

                        sendmail(data.MI_Id, emailid, Emailcc, approvarname, leavename, HRELAPA_Remarks, "Leave_Rejected", hrmeId, data.get_leave_status[i].HRELAP_FromDate);
                        //sendSms(data.MI_Id, mobileno, "Leave_Rejected", hrmeId, data.get_leave_status[0].HRELAP_FromDate, data.get_leave_status[i].HRML_Id);

                        var devicenew = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.get_leave_status[i].HRME_Id).Select(t => t.HRME_AppDownloadedDeviceId).FirstOrDefault();
                        data.AMST_AppDownloadedDeviceId = devicenew;
                        //callnotification(devicenew, temp_id, data.MI_Id, data.get_leave_status[i], "LEAVE MANAGEMENT", "Leave Rejected");
                    }
                }
                data.message = "Success";
            }

            catch (Exception ex)
            {
                _log.LogInformation("Leave Approval  error");
                _log.LogDebug(ex.Message);
                data.message = "False";
            }
            return data;
        }

        public string sendmail(long MI_Id, string Email, string Emailcc, string approvarname, string leavename, string leavereson, string Template, long UserID, DateTime? LeaveDate)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                var employeedetails = _lmContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).ToList().FirstOrDefault();
                Mailmsg = Mailmsg.Replace("[LOGO]", institutionName[0].MI_Logo);
                Mailmsg = Mailmsg.Replace("[NAME]", employeedetails.HRME_EmployeeFirstName);
                Mailmsg = Mailmsg.Replace("[EmpCode]", employeedetails.HRME_EmployeeCode);
                Mailmsg = Mailmsg.Replace("[EMPNAME]", approvarname);
                Mailmsg = Mailmsg.Replace("[LEAVENAME]", leavename);
                Mailmsg = Mailmsg.Replace("[LEAVEREASON]", leavereson);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));
                Mailmsg = Mailmsg.Replace("[DATE1]", date.ToString("dd/MM/yyyy"));
                Mailmsg = Mailmsg.Replace("[DATE2]", date.ToString("dd/MM/yyyy"));


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
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
                    message.AddTo(Email);

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
                    if (Emailcc != null && Emailcc != "")
                    {
                        message.AddCc(Emailcc);
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
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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

        public string sendEmail(long MI_Id, string Template, long HRELAP_Id, string email, string ccemail)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == Template //&& e.MI_Id== MI_Id
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Email_Prametars_Replace";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",
                  SqlDbType.VarChar)
                    {
                        Value = HRELAP_Id
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

                                    else if (datatype.Name == "Decimal")
                                    {
                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();
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

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {


                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();

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
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    string mailcc = "";
                    string mailbcc = "";

                    //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    //{
                    //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                    //    mailcc = ccmail[0].ToString();

                    //    if (ccmail.Length > 1)
                    //    {
                    //        if (ccmail[1] != null || ccmail[1] != "")
                    //        {
                    //            mailbcc = ccmail[1].ToString();
                    //        }
                    //    }

                    //}
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }



                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(email);

                    // mailto:message.addto("kavitajnandoor21@gmail.com");
                    //message.AddTo("sreepriyagangadharan15@gmail.com");

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

                    if (ccemail != null && ccemail != "")
                    {
                        string[] ccmaildetails = ccemail.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }


                    //if (mailcc != null && mailcc != "")
                    //{
                    //    string[] ccmaildetails = mailcc.Split(',');

                    //    foreach (var c in ccmaildetails)
                    //    {
                    //        message.AddCc(c);
                    //    }
                    //}

                    //if (ccemail != null && ccemail != "")
                    //{
                    //    message.AddBcc(ccemail);
                    //}

                    //****************** EMAIL BCC DETAILS ***************//




                    if (mailcc != null && mailcc != "")
                    {
                        //message.AddCc(mailcc);
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



                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();

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
            }
            return "success";

        }

        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID, DateTime? LeaveDate, long leaveid)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string Empname = _lmContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).Select(p => p.HRME_EmployeeFirstName).FirstOrDefault();
                sms = sms.Replace("[NAME]", Empname);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                sms = sms.Replace("[DATE]", date.ToString("dd/MM/yyyy"));
                string leavename = _lmContext.HR_Master_Leave_DMO.Where(p => p.MI_Id == MI_Id && p.HRML_Id == leaveid).Select(p => p.HRML_LeaveName).FirstOrDefault();
                sms = sms.Replace("[LEAVENAME]", leavename);

                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    string PHNO = mobileNo.ToString();
                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    #region
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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
                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string callnotification(string devicenew, string temp_id, long mi_id, LeaveCreditDTO dto, string header_flg, string headerName)
        {
            try
            {
                var key = _lmContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;

                LeaveCreditDTO data = new LeaveCreditDTO();
                var leavedetails = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(h => h.MI_Id == mi_id && h.HRELAP_ActiveFlag == true && h.HRELAP_ApplicationID == temp_id).Distinct().ToList();

                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                string sound = "default";
                string notId = "4";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
               "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Classwork" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + leavedetails.FirstOrDefault().HRELAP_LeaveReason + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
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
                }

                PushNotification push_noti = new PushNotification(_db);
                push_noti.Insert_PushNotificationLeave(temp_id, mi_id, devicenew, dto, header_flg, headerName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public LeaveCreditDTO viewcomment(LeaveCreditDTO dto)
        {
            try
            {
                dto.commentlist = (from a in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                                   from b in _lmContext.HR_Master_Employee_DMO
                                   where (a.HRELAP_Id == dto.HRELAP_Id && a.HRME_Id == b.HRME_Id)
                                   select new LeaveCreditDTO
                                   {
                                       HRELAP_Id = a.HRELAP_Id,
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                       HRELAPA_SanctioningLevel = a.HRELAPA_SanctioningLevel,
                                       HRELAPA_Remarks = a.HRELAPA_Remarks,
                                       CreatedDate = a.CreatedDate,
                                       HRELAPA_TotalDays = a.HRELAPA_TotalDays,
                                       HRELAPA_FromDate = a.HRELAPA_FromDate,
                                       HRELAPA_ToDate = a.HRELAPA_ToDate,
                                       HRELAPA_LeaveStatus = a.HRELAPA_LeaveStatus,
                                   }).OrderBy(a => a.HRELAPA_SanctioningLevel).ToArray();

                List<deviationdetailDTO> alldata = new List<deviationdetailDTO>();

                //string month1, month2, month3;
                //string year1, year2, year3;
                //string devi1, devi2, devi3;

                var get_applieddate = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(a => a.HRME_Id == dto.HRME_Id && a.HRELAP_Id == dto.HRELAP_Id).ToList();

                LeaveCreditDTO obj = new LeaveCreditDTO();
                int currmonth = DateTime.Now.Month;
                if (get_applieddate.Count > 0)
                {
                    currmonth = get_applieddate.FirstOrDefault().HRELAP_ApplicationDate.Month;
                }

                deviationdetailDTO objdv = new deviationdetailDTO();
                List<DEVDTO> dsadf = new List<DEVDTO>();

                //DateTime date = Convert.ToDateTime(get_applieddate.FirstOrDefault().HRELAP_ApplicationDate.Month);
                //DateTime firstDayLastMonth = date.AddDays(1 - date.Day).AddMonths(0);
                //DateTime lastDayLastMonth = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month, DateTime.DaysInMonth(firstDayLastMonth.Year, firstDayLastMonth.Month));

                //Console.WriteLine(firstDayLastMonth);
                //Console.WriteLine(lastDayLastMonth);

                //Console.WriteLine("Hello World1");
                //DateTime firstDayLastMonth1 = date.AddDays(1 - date.Day).AddMonths(-1);
                //DateTime lastDayLastMonth1 = new DateTime(firstDayLastMonth1.Year, firstDayLastMonth1.Month, DateTime.DaysInMonth(firstDayLastMonth1.Year,
                //                                                                                                                 firstDayLastMonth1.Month));
                //Console.WriteLine(firstDayLastMonth1);
                //Console.WriteLine(lastDayLastMonth1);


                //Console.WriteLine("Hello World2");
                //DateTime firstDayLastMonth2 = date.AddDays(1 - date.Day).AddMonths(-2);
                //DateTime lastDayLastMonth2 = new DateTime(firstDayLastMonth2.Year, firstDayLastMonth2.Month, DateTime.DaysInMonth(firstDayLastMonth2.Year,
                //                                                                                                                 firstDayLastMonth2.Month));
                //Console.WriteLine(firstDayLastMonth2);
                //Console.WriteLine(lastDayLastMonth2);

                List<int> ids = new List<int>();

                ids.Add(0);
                ids.Add(-1);
                ids.Add(-2);

                DateTime date = Convert.ToDateTime(get_applieddate.FirstOrDefault().HRELAP_ApplicationDate);
                for (var i = 0; i < ids.Count; i++)
                {
                    DateTime firstDayLastMonth = date.AddDays(1 - date.Day).AddMonths(ids[i]);
                    DateTime lastDayLastMonth = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month,
                        DateTime.DaysInMonth(firstDayLastMonth.Year, firstDayLastMonth.Month));

                    string monthfirstdate = firstDayLastMonth.ToString("yyyy-MM-dd");
                    string monthlastdate = lastDayLastMonth.ToString("yyyy-MM-dd");

                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_MonthlyDeviation_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = monthfirstdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                        {
                            Value = monthlastdate
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    dsadf.Add(new DEVDTO
                                    {
                                        HRME_Id = Convert.ToString(dataReader1["HRME_Id"]),
                                        HRMDC_Name = Convert.ToString(dataReader1["HRMDC_Name"]),
                                        HRMDC_ID = Convert.ToString(dataReader1["HRMDC_ID"]),
                                        DevPer = Convert.ToString(dataReader1["DevPer"]),
                                        CompPer = Convert.ToString(dataReader1["Comp_Per"]),
                                        MonthYear = Convert.ToString(dataReader1["MonthYear"]),
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    dto.deviationdetails = dsadf.ToArray();
                }

                #region Previous Code For Deviation New
                //for (int i = 0; i < 3; i++)
                //{
                //    string firstdate = "";
                //    string lday = "";
                //    string lastdate = "";
                //    string monthid = "";
                //    string yearid = "";

                //    var Get_MonthId = currmonth - i;
                //    monthid = Get_MonthId.ToString();
                //    if (Get_MonthId <= currmonth && Get_MonthId > 0)
                //    {
                //        yearid = DateTime.Now.Year.ToString();
                //        if (Get_MonthId > 0 && Get_MonthId < 10)
                //        {
                //            monthid = "0" + Get_MonthId.ToString();
                //        }
                //    }
                //    else
                //    {
                //        if (Get_MonthId == -1)
                //        {
                //            monthid = "12";
                //        }
                //        if (Get_MonthId == -2)
                //        {
                //            monthid = "11";
                //        }
                //        yearid = (DateTime.Now.Year - 1).ToString();
                //    }

                //    firstdate = yearid + "-" + monthid + "-01";
                //    lday = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == Get_MonthId).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault().ToString();

                //    //lastdate = yearid + "-" + monthid + "-" + lday;

                //    var lastDayOfMonth = DateTime.DaysInMonth(Convert.ToInt32(yearid), Convert.ToInt32(monthid));
                //    lastdate = yearid + "-" + monthid + "-" + lastDayOfMonth;

                //    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "VMS_MonthlyDeviation_Modify";
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                //        {
                //            Value = dto.HRME_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                //        {
                //            Value = firstdate
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                //        {
                //            Value = lastdate
                //        });
                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();
                //        try
                //        {
                //            var retObject1 = new List<dynamic>();
                //            using (var dataReader1 = cmd.ExecuteReader())
                //            {
                //                while (dataReader1.Read())
                //                {
                //                    dsadf.Add(new DEVDTO
                //                    {
                //                        HRME_Id = Convert.ToString(dataReader1["HRME_Id"]),
                //                        HRMDC_Name = Convert.ToString(dataReader1["HRMDC_Name"]),
                //                        HRMDC_ID = Convert.ToString(dataReader1["HRMDC_ID"]),
                //                        DevPer = Convert.ToString(dataReader1["DevPer"]),
                //                        CompPer = Convert.ToString(dataReader1["Comp_Per"]),
                //                        MonthYear = Convert.ToString(dataReader1["MonthYear"]),
                //                    });
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }
                //}

                //dto.deviationdetails = dsadf.ToArray();
                #endregion Previous Code For Deviation New

                #region Previous Code For Deviation
                //if (currmonth >= 4)
                //{
                //    for (int cnt = 3; cnt > 0; cnt--)
                //    {
                //        int yearid = DateTime.Now.Year;
                //        string firstdate = DateTime.Now.Year + "-" + (currmonth - cnt) + "-01";
                //        string lday = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == (currmonth - cnt)).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault().ToString();
                //        string lastdate = DateTime.Now.Year + "-" + (currmonth - cnt) + "-" + lday;
                //        List<DEVDTO> dsadf = new List<DEVDTO>();

                //        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                //        {
                //            cmd.CommandText = "VMS_MonthlyDeviation";
                //            cmd.CommandType = CommandType.StoredProcedure;
                //            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                //            {
                //                Value = dto.HRME_Id
                //            });
                //            cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                //            {
                //                Value = firstdate
                //            });
                //            cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                //            {
                //                Value = lastdate
                //            });
                //            if (cmd.Connection.State != ConnectionState.Open)
                //                cmd.Connection.Open();
                //            try
                //            {
                //                var retObject1 = new List<dynamic>();
                //                using (var dataReader1 = cmd.ExecuteReader())
                //                {
                //                    while (dataReader1.Read())
                //                    {
                //                        dsadf.Add(new DEVDTO
                //                        {
                //                            HRME_Id = Convert.ToString(dataReader1["HRME_Id"]),
                //                            HRMDC_Name = Convert.ToString(dataReader1["HRMDC_Name"]),
                //                            HRMDC_ID = Convert.ToString(dataReader1["HRMDC_ID"]),
                //                            DevPer = Convert.ToString(dataReader1["DevPer"]),
                //                        });
                //                    }
                //                    if (dataReader1.HasRows == false)
                //                    {
                //                        dsadf.Add(new DEVDTO
                //                        {
                //                            HRME_Id = "N/A",
                //                            HRMDC_Name = "N/A",
                //                            HRMDC_ID = "N/A",
                //                            DevPer = "0",
                //                        });
                //                    }
                //                }
                //            }
                //            catch (Exception ex)
                //            {
                //                Console.WriteLine(ex.Message);
                //            }
                //        }

                //        deviationdetailDTO objdv = new deviationdetailDTO();

                //        string month = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == (currmonth - cnt)).Select(t => t.IVRM_Month_Name).FirstOrDefault().ToString();
                //        string dept = dsadf[0].HRMDC_Name;
                //        objdv.DEPARTMENT = dept.ToString();
                //        if (cnt == 3)
                //        {
                //            month1 = month.ToString(); year1 = Convert.ToString(yearid); devi1 = dsadf[0].DevPer;
                //            objdv.MON_YR1 = (month1 + "-" + year1).ToString();
                //            objdv.DEVI1 = (devi1).ToString();
                //            objdv.COMP1 = (100 - Convert.ToDecimal(devi1)).ToString();
                //        }
                //        else if (cnt == 2)
                //        {
                //            month2 = month.ToString(); year2 = Convert.ToString(yearid); devi2 = dsadf[0].DevPer;
                //            objdv.MON_YR1 = (month2 + "-" + year2).ToString();
                //            objdv.DEVI1 = (devi2).ToString();
                //            objdv.COMP1 = (100 - Convert.ToDecimal(devi2)).ToString();
                //        }
                //        else if (cnt == 1)
                //        {
                //            month3 = month.ToString(); year3 = Convert.ToString(yearid); devi3 = dsadf[0].DevPer;
                //            objdv.MON_YR1 = (month3 + "-" + year3).ToString();
                //            objdv.DEVI1 = (devi3).ToString();
                //            objdv.COMP1 = (100 - Convert.ToDecimal(devi3)).ToString();
                //        }
                //        alldata.Add(objdv);
                //    }
                //}

                //else
                //{
                //    for (int cnt = 3; cnt > 0; cnt--)
                //    {
                //        deviationdetailDTO objdv = new deviationdetailDTO();
                //        int monthid = 0;
                //        int yearid = 0;
                //        if ((currmonth - cnt) == 0) { monthid = 12; yearid = DateTime.Now.Year - 1; }
                //        else if ((currmonth - cnt) == -1) { monthid = 11; yearid = DateTime.Now.Year - 1; }
                //        else if ((currmonth - cnt) == -2) { monthid = 10; yearid = DateTime.Now.Year - 1; }
                //        else { monthid = currmonth; yearid = DateTime.Now.Year; }

                //        string firstdate = yearid + "-" + monthid + "-01";
                //        string lday = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == monthid).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault().ToString();
                //        string lastdate = yearid + "-" + monthid + "-" + lday;

                //        List<DEVDTO> dsadf = new List<DEVDTO>();

                //        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                //        {
                //            cmd.CommandText = "VMS_MonthlyDeviation";
                //            cmd.CommandType = CommandType.StoredProcedure;
                //            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                //            {
                //                Value = dto.HRME_Id
                //            });
                //            cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                //            {
                //                Value = firstdate
                //            });
                //            cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                //            {
                //                Value = lastdate
                //            });
                //            if (cmd.Connection.State != ConnectionState.Open)
                //                cmd.Connection.Open();
                //            try
                //            {
                //                var retObject1 = new List<dynamic>();
                //                using (var dataReader1 = cmd.ExecuteReader())
                //                {
                //                    while (dataReader1.Read())
                //                    {
                //                        dsadf.Add(new DEVDTO
                //                        {
                //                            HRME_Id = Convert.ToString(dataReader1["HRME_Id"]),
                //                            HRMDC_Name = Convert.ToString(dataReader1["HRMDC_Name"]),
                //                            HRMDC_ID = Convert.ToString(dataReader1["HRMDC_ID"]),
                //                            DevPer = Convert.ToString(dataReader1["DevPer"]),
                //                        });
                //                    }
                //                }
                //            }
                //            catch (Exception ex)
                //            {
                //                Console.WriteLine(ex.Message);
                //            }
                //        }

                //        string month = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == monthid).Select(t => t.IVRM_Month_Name).FirstOrDefault().ToString();
                //        string dept = dsadf[0].HRMDC_Name;
                //        objdv.DEPARTMENT = dept.ToString();
                //        if (cnt == 3)
                //        {
                //            month1 = month.ToString(); year1 = Convert.ToString(yearid); devi1 = dsadf[0].DevPer;
                //            objdv.MON_YR1 = (month1 + "-" + year1).ToString();
                //            objdv.DEVI1 = (devi1).ToString();
                //            objdv.COMP1 = (100 - Convert.ToDecimal(devi1)).ToString();
                //        }
                //        else if (cnt == 2)
                //        {
                //            month2 = month.ToString(); year2 = Convert.ToString(yearid); devi2 = dsadf[0].DevPer;
                //            objdv.MON_YR1 = (month2 + "-" + year2).ToString();
                //            objdv.DEVI1 = (devi2).ToString();
                //            objdv.COMP1 = (100 - Convert.ToDecimal(devi2)).ToString();
                //        }
                //        else if (cnt == 1)
                //        {
                //            month3 = month.ToString(); year3 = Convert.ToString(yearid); devi3 = dsadf[0].DevPer;
                //            objdv.MON_YR1 = (month3 + "-" + year3).ToString();
                //            objdv.DEVI1 = (devi3).ToString();
                //            objdv.COMP1 = (100 - Convert.ToDecimal(devi3)).ToString();
                //        }
                //        alldata.Add(objdv);
                //    }
                //}

                //dto.deviationdetails = alldata.ToArray();
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public LeaveCreditDTO getRequestStatus(LeaveCreditDTO dto)
        {
            try
            {


              


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "HR_Compoffondutyleave_APPROVALEMPLIST_04012024";
                    cmd.CommandText = "HR_Compoffondutyleave_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar) { Value = dto.LoginId });
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
                        dto.get_leavestatus = retObject.ToArray();
                        //cumDTO.get_leavestatus = get_leavestatus1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "HR_Compoffondutyleave_APPROVALEMPLIST_04012024";
                    cmd.CommandText = "HR_Compoffondutyleave_APPROVALEMPGrid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar) { Value = dto.LoginId });

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
                        dto.activityIds = retObject.ToArray();                   
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //dto.activityIds = (from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                //                   from d in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                //                   from e in _lmContext.HR_Master_Employee_DMO
                //                   from f in _lmContext.HR_Master_Leave_DMO
                //                   from g in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                //                   where (c.HRELAP_Id == d.HRELAP_Id && c.HRELAP_Id == g.HRELAP_Id && f.HRML_Id == g.HRML_Id && leavecodes.Contains(f.HRML_LeaveCode) && f.MI_Id == dto.MI_Id && c.HRME_Id == e.HRME_Id && c.HRELAP_ActiveFlag == true && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.MI_Id == dto.MI_Id)
                //                   select new LeaveCreditDTO
                //                   {
                //                       HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                //                       HRML_LeaveName = f.HRML_LeaveName,
                //                       HRELAP_FromDate = c.HRELAP_FromDate,
                //                       HRELAP_ToDate = c.HRELAP_ToDate,
                //                       HRELAP_TotalDays = c.HRELAP_TotalDays,
                //                       HRME_EmployeeCode = e.HRME_EmployeeCode,
                //                       HRELAP_ApplicationStatus = c.HRELAP_ApplicationStatus,
                //                       HRELAPA_Remarks = d.HRELAPA_Remarks,
                //                       HRELAPA_InTime = d.HRELAPA_InTime,
                //                       HRELAPA_OutTime = d.HRELAPA_OutTime
                //                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dto;
        }


        public async Task<LeaveCreditDTO> get_approvestatusAsync(LeaveCreditDTO data)
        {


            try
            {

                var HRME_ID = _lmContext.Staff_User_Login.Where(t => t.Id == data.LoginId).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.doc_list2.Length > 0)
                {
                    foreach (var p in data.doc_list2)
                    {
                        var contactExistsP = _lmContext.Database.ExecuteSqlCommand("CompoffLeaveApproval @p0,@p1,@p2,@p3,@p4,@p5", p.HRELAP_Id, HRME_ID, p.HRELAPA_Remarks, p.HRELAPD_InTime, p.HRELAPD_OutTime, p.Status);
                        if (contactExistsP > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "notUpdated";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("CompOff/OD Approval error");
                _log.LogDebug(ex.Message);
                data.message = "False";
            }
            return data;
        }



        public async Task<LeaveCreditDTO> get_rejectedstatusAsync(LeaveCreditDTO data)
        {
            string status = "Rejected";
            string sectionlevel = "1";

            string[] leavecodes = new string[2];
            leavecodes[0] = "OD";
            leavecodes[1] = "COMPOFF";

            try
            {
                HR_Emp_Leave_ApplicationDMO obj = new HR_Emp_Leave_ApplicationDMO();
                HR_Emp_Leave_Appl_AuthorisationDMO objauth = new HR_Emp_Leave_Appl_AuthorisationDMO();
                HR_Emp_Leave_StatusDMO objpge1 = new HR_Emp_Leave_StatusDMO();

                if (data.get_leave_status != null)
                {
                    for (var i = 0; i < data.get_leave_status.Count(); i++)
                    {
                        var instid = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.get_leave_status[i].HRME_Id).Select(t => t.MI_Id).FirstOrDefault();
                        var temp_id = data.get_leave_status[i].HRELAP_ApplicationID;
                        var temp_leave_res = data.get_leave_status[i].HRELAP_LeaveReason;
                        var leaveremarks = data.HRELAPA_Remarks;

                        long leave_yearid = _lmContext.HR_Master_LeaveYear_DMO.Where(e => e.MI_Id == instid && DateTime.Now.Date >= e.HRMLY_FromDate.Date && DateTime.Now.Date <= e.HRMLY_ToDate.Date).Select(t => t.HRMLY_Id).FirstOrDefault();

                        obj = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_ApplicationID == temp_id && t.MI_Id == instid).FirstOrDefault();
                        obj.HRELAP_ApplicationStatus = status;
                        obj.HRELAP_FinalFlag = true;
                        obj.UpdatedDate = DateTime.Today;
                        obj.HRELAP_CompOffCreditApplFlg = true;
                        _lmContext.Update(obj);
                        _lmContext.SaveChanges();

                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "AddLeaveApplAuthorisation";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRELAP_Id", SqlDbType.BigInt)
                            {
                                Value = data.get_leave_status[i].HRELAP_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                            {
                                Value = data.get_leave_status[i].HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRELAPA_Remarks", SqlDbType.VarChar)
                            {
                                Value = leaveremarks
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRELAPA_InTime", SqlDbType.NVarChar)
                            {
                                Value = data.HRELAPA_InTime
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRELAPA_OutTime", SqlDbType.NVarChar)
                            {
                                Value = data.HRELAPA_OutTime
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            try
                            {
                                var retObject1 = new List<dynamic>();
                                using (var dataReader1 = await cmd.ExecuteReaderAsync())
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
                data.message = "Success";
            }

            catch (Exception ex)
            {
                _log.LogInformation("CompOff/OD Approval error");
                _log.LogDebug(ex.Message);
                data.message = "False";
            }
            return data;
        }

        public LeaveCreditDTO ViewRemarksLevelWise(LeaveCreditDTO data)
        {
            try
            {
                data.Getviewremarksdetails = (from a in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                                              from b in _lmContext.HR_Master_Employee_DMO
                                              from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                                              where(a.HRME_Id == b.HRME_Id && a.HRELAP_Id == data.HRELAP_Id && a.HRELAP_Id==c.HRELAP_Id)
                                              select new LeaveCreditDTO
                                              {
                                                  HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName) +
                                                  (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                                  (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)),
                                                  HRELAPA_SanctioningLevel = a.HRELAPA_SanctioningLevel,
                                                  HRELAPA_Remarks = a.HRELAPA_Remarks,
                                                  HRELAPA_FinalFlag = a.HRELAPA_FinalFlag,
                                                  HRELAPA_FromDate = a.HRELAPA_FromDate,
                                                  HRELAPA_ToDate = a.HRELAPA_ToDate,
                                                  HRELAP_LeaveReason = c.HRELAP_LeaveReason,
                                                  HRELAPA_TotalDays = a.HRELAPA_TotalDays,
                                                  HRELAPA_LeaveStatus = a.HRELAPA_LeaveStatus,
                                                  CreatedDate = a.CreatedDate,
                                                  HRML_LeaveName = a.HRML_Id > 0 ? _lmContext.HR_Master_Leave_DMO.Where(c => c.HRML_Id == a.HRML_Id).FirstOrDefault().HRML_LeaveName : ""
                                              }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message);
                data.message = "False";
            }
            return data;
        }

        public LeaveCreditDTO Viewleavebalancehistory(LeaveCreditDTO data)
        {
            try
            {
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmpWiseLeavesDetails";
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getemployeeleavedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmpWiseCYAllLeavesDetails";
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getemployeehistroyleavedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message);
                data.message = "False";
            }
            return data;
        }
        
        public LeaveCreditDTO LeaveCredition(LeaveCreditDTO data)
        {
            DateTime Today = DateTime.Now.Date;
            //var CurrentDayLeaveDetails=
            return data;
        }
        public LeaveCreditDTO ViewOtherDetails(LeaveCreditDTO data)
        {
            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
                //cmd.CommandText = "HR_Compoffondutyleave_APPROVALEMPLIST_04012024";
                cmd.CommandText = "HR_CompoffEmployeeDRAndApprovarDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HRELAP_Id", SqlDbType.BigInt) { Value = data.HRELAP_Id });
                cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime) { Value = data.HRMED_EntryDate });
                cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = data.HRML_LeaveType });
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
                    data.report_details = retObject.ToArray();
                    //cumDTO.get_leavestatus = get_leavestatus1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
               
                cmd.CommandText = "HR_Compoffondutyleavemanualdr";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime) { Value = Convert.ToDateTime(data.HRMED_EntryDate).ToString() });
                //cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = data.HRML_LeaveType });
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
                    data.get_leavestatus = retObject.ToArray();
                    //cumDTO.get_leavestatus = get_leavestatus1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            //_lmContext.Database.ExecuteSqlCommand("HR_Compoff_ReApproval @p0,@p1,@p2", data.MI_Id, data.Id, data.Fromdate);
            return data;
        }
    }
}

