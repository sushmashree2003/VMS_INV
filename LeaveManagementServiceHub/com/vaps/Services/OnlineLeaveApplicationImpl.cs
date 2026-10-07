using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs;
using CommonLibrary;
using DomainModel.Model;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using SendGrid;
using Newtonsoft.Json.Linq;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class OnlineLeaveApplicationImpl : OnlineLeaveApplicationInterface
    {
        public LMContext _lmContext;
        public DomainModelMsSqlServerContext _db;
        public OnlineLeaveApplicationImpl(LMContext ttcategory, DomainModelMsSqlServerContext abc)
        {
            _lmContext = ttcategory;
            _db = abc;
        }
        public LeaveCreditDTO getonlineLeave(LeaveCreditDTO data)
        {
            long yearid = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.HRMLY_FromDate <= DateTime.Today.Date
            && t.HRMLY_ToDate >= DateTime.Today.Date && t.HRMLY_ActiveFlag == true && t.MI_Id == data.MI_Id).Select(t => t.HRMLY_Id).FirstOrDefault();


            var qyery1 = (from q in _db.Staff_User_Login
                          from r in _db.HR_Master_Employee_DMO
                          where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                          select new LeaveCreditDTO
                          {
                              HRME_Id = r.HRME_Id,
                          }).Distinct().ToArray();

            //data.leaverestriction = (from a in _lmContext.HR_Leave_Restriction_DatesDMO
            //                         from b in _lmContext.HR_Master_Department_DMO
            //                         from c in _lmContext.HR_Master_Employee_DMO
            //                         where (c.HRME_Id== qyery1.FirstOrDefault().HRME_Id && a.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRLRD_FromDate <= DateTime.Today.Date && a.HRLRD_ToDate >= DateTime.Today.Date && a.HRLRD_ActiveFlg == true)
            //                         select a).ToArray();
            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandText = "HR_Compoffondutyleave_rejectedEMPGridtoapprove";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = qyery1[0].HRME_Id });
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
            data.leaveName = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveType != "LWP").ToArray();

            if (qyery1.Length > 0)
            {

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Emp_leave_Restration";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = qyery1.FirstOrDefault().HRME_Id
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
                        data.leaverestriction = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_employee_CurrentLeaves";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = qyery1.FirstOrDefault().HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id",
                      SqlDbType.VarChar)
                    {
                        Value = yearid
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
                        data.leave_name = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.online_leave = (from a in _lmContext.HR_Master_Employee_DMO
                                     from b in _lmContext.HR_Master_Designation_DMO
                                     from c in _lmContext.HR_Emp_Leave_StatusDMO
                                     where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == qyery1.FirstOrDefault().HRME_Id)
                                     select new LeaveCreditDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                         HRME_DOJ = a.HRME_DOJ,
                                         HRME_MobileNo = _lmContext.Emp_MobileNo.Where(m => m.HRME_Id == a.HRME_Id && m.HRMEMNO_DeFaultFlag == "default").Select(a => a.HRMEMNO_MobileNo).FirstOrDefault(),

                                         HRME_EmailId = _lmContext.Emp_Email_Id.Where(m => m.HRME_Id == a.HRME_Id && m.HRMEM_DeFaultFlag == "default").Select(a => a.HRMEM_EmailId).FirstOrDefault(),

                                         HRMDES_DesignationName = b.HRMDES_DesignationName,
                                         HRME_EmployeeCode = a.HRME_EmployeeCode
                                     }).Distinct().ToArray();

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Reconsider_Leave_Applied_Count";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = qyery1.FirstOrDefault().HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id",
                      SqlDbType.VarChar)
                    {
                        Value = yearid
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
                        data.countrecon = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Leave_Applied_DaysCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = qyery1.FirstOrDefault().HRME_Id
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
                        data.lappcount = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }

            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_DeviationPERCENTAGE_CLleaves";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                {
                    Value = data.MI_Id
                });

                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                  SqlDbType.VarChar)
                {
                    Value = qyery1.FirstOrDefault().HRME_Id
                });
                cmd.Parameters.Add(new SqlParameter("@FLAG",
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
                    data.deviationdetails = retObject.ToArray();
                }

                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

            }

            return data;
        }
        public LeaveCreditDTO saveonlineLeave(LeaveCreditDTO _category)
        {

            if (_category.returnmsg == "RCLeave")
            {
                try
                {


                    foreach (var item in _category.multi_leave_name) 
                    {
                        var contactExistsP = _lmContext.Database.ExecuteSqlCommand("HR_ReconsiderLeave_Apply @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10",
                              item.HRELAP_Id, _category.MI_Id, item.HRME_Id, item.HRERLAP_FromDate.ToString("yyyy-MM-dd"), item.HRERLAP_ToDate.ToString("yyyy-MM-dd"), item.HRELAP_TotalDays, item.HRELAP_LeaveReason, "Pending", _category.UserId, item.HRML_Id, item.HRMLY_Id);
                        if (contactExistsP > 0)
                        {
                            _category.retrunMsg = "Success";
                        }
                        else
                        {
                            _category.retrunMsg = "NotSuccess";
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else
            {
                int count = 0;
                long HRELAPD_Id = 0;
                HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(_category);

                HR_Emp_Leave_Trans_DMO objLvTrans = Mapper.Map<HR_Emp_Leave_Trans_DMO>(_category);
                HR_Emp_Leave_Trans_Details_DMO objLvTrDetail = Mapper.Map<HR_Emp_Leave_Trans_Details_DMO>(_category);

                var leave_year = _lmContext.HR_Master_LeaveYear_DMO.Where(e => e.MI_Id == _category.MI_Id && DateTime.Now.Date >= e.HRMLY_FromDate.Date && DateTime.Now.Date <= e.HRMLY_ToDate.Date).ToList();

                var leave_id = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_LeaveName == _category.temp_table_data.FirstOrDefault().HRML_LeaveName && p.MI_Id == _category.MI_Id).Select(p => p.HRML_Id).FirstOrDefault();

                try
                {
                    var hrme = _lmContext.Staff_User_Login.Where(p => p.Id == _category.UserId && p.MI_Id == _category.MI_Id).Select(p => p.Emp_Code).FirstOrDefault();


                    List<LeaveCreditDTO> duplicate = new List<LeaveCreditDTO>();

                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        // cmd.CommandTimeout = 500000;
                        cmd.CommandText = "HR_Leave_Dublicate_Check";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                        {
                            Value = hrme
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRML_Id", SqlDbType.BigInt)
                        {
                            Value = _category.temp_table_data.FirstOrDefault().HRML_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                        {
                            Value = _category.frmToDates[0].HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy")
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                        {
                            Value = _category.frmToDates[0].HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy")
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();


                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {

                                while (dataReader.Read())
                                {
                                    try
                                    {
                                        duplicate.Add(new LeaveCreditDTO
                                        {
                                            HRELAP_Id = Convert.ToInt64(dataReader["HRELAP_Id"].ToString()),


                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }




                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }


                    //var duplicate = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELAP_FromDate == _category.frmToDates[0].HRELAP_FromDate && z.HRELAP_ToDate == _category.frmToDates[0].HRELAP_ToDate && z.HRELAP_ActiveFlag == true && z.HRME_Id == hrme && (z.HRELAP_ApplicationStatus == "Approved" || z.HRELAP_ApplicationStatus == "Applied")).ToArray();

                    var employeedetails = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == hrme).ToList().FirstOrDefault();
                    var employeename = employeedetails.HRME_EmployeeFirstName + " " + (employeedetails.HRME_EmployeeMiddleName == null ? "" : employeedetails.HRME_EmployeeMiddleName) + " " + (employeedetails.HRME_EmployeeLastName == null ? "" : employeedetails.HRME_EmployeeLastName);
                    var employeecode = employeedetails.HRME_EmployeeCode;

                    if (duplicate.Count() == 0)
                    {
                        if (objpge.HRELAP_Id > 0)
                        {
                            var resultCount = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_FromDate <= objpge.HRELAP_FromDate && t.HRELAP_ToDate >= objpge.HRELAP_ToDate).Count();

                            if (resultCount == 0)
                            {
                                var result = _lmContext.HR_Emp_Leave_ApplicationDMO.Single(t => t.MI_Id == objpge.MI_Id);

                                result.MI_Id = objpge.MI_Id;
                                result.HRME_Id = objpge.HRME_Id;
                                result.HRELAP_ApplicationDate = objpge.HRELAP_ApplicationDate;
                                result.HRELAP_FromDate = objpge.HRELAP_FromDate;
                                result.HRELAP_ToDate = objpge.HRELAP_ToDate;
                                result.HRELAP_TotalDays = objpge.HRELAP_TotalDays;
                                result.HRELAP_LeaveReason = objpge.HRELAP_LeaveReason;
                                result.HRELAP_ContactNoOnLeave = objpge.HRELAP_ContactNoOnLeave;
                                result.HRELAP_ReportingDate = objpge.HRELAP_ReportingDate;
                                result.UpdatedDate = DateTime.Now;
                                result.HRELAP_UpdatedBy = _category.UserId;
                                result.HRELAP_CompOffAdjestFlg = _category.frmToDates[0].LOPAdjest;
                                // result.HRELAP_ApplicationID = objpge.HRELAP_ApplicationID;

                                result.UpdatedDate = DateTime.Now;
                                _lmContext.Update(result);
                                var contactExists = _lmContext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                            else
                            {
                                _category.returnduplicatestatus = "Duplicate";
                                return _category;
                            }
                        }
                        else
                        {
                            var qyery1 = (from q in _db.Staff_User_Login
                                          from r in _db.HR_Master_Employee_DMO
                                          where (q.Emp_Code == r.HRME_Id && q.Id == _category.UserId)
                                          select new LeaveCreditDTO
                                          {
                                              HRME_Id = r.HRME_Id,
                                          }).Distinct().ToArray();

                            if (qyery1.Length > 0)
                            {
                                var checkbalanceleave = _lmContext.HR_Emp_Leave_StatusDMO.Where(d => d.MI_Id == _category.MI_Id && d.HRME_Id == qyery1.FirstOrDefault().HRME_Id && d.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id && d.HRMLY_Id == leave_year.FirstOrDefault().HRMLY_Id).ToList();
                                var bal_leave = checkbalanceleave.FirstOrDefault().HRELS_CBLeaves - _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                var fromdate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate).ToString("d");
                                var todate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate).ToString("d");

                                string leavetype = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_Id == _category.temp_table_data[0].HRML_Id && p.MI_Id == _category.MI_Id).Select(p => p.HRML_LeaveName).FirstOrDefault();

                                if (leavetype == "ON DUTY")
                                {
                                    var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                          from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                          where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id
                                                          && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                          && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                          && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                                          select new LeaveCreditDTO
                                                          {
                                                              HRELAP_Id = m.HRELAP_Id
                                                          }).ToList();

                                    if (checkduplicate.Count == 0)
                                    {
                                        var id = _lmContext.HR_Emp_Leave_Trans_Details_DMO.Where(t => t.HRELTD_FromDate.Date >= Convert.ToDateTime(fromdate).Date && t.HRELTD_ToDate.Date <= Convert.ToDateTime(todate).Date && t.MI_Id == _category.MI_Id && t.HRELTD_LWPFlag == true && t.HRME_Id == qyery1.FirstOrDefault().HRME_Id).ToList();
                                        if (id.Count > 0)
                                        {
                                            for (int i = 0; i < id.Count(); i++)
                                            {
                                                id[i].HRELTD_LWPFlag = false;
                                                id[i].UpdatedDate = DateTime.Now;
                                                _lmContext.Update(id[i]);
                                                //  _lmcontext.savechanges();
                                                var hreltdid = id[i].HRELTD_Id;
                                                var hreltid = id[i].HRELT_Id;

                                                var idtrans11 = _lmContext.HR_Emp_Leave_Trans_DMO.Single(t => t.HRELT_Id == hreltid && t.MI_Id == _category.MI_Id && t.HRME_Id == qyery1.FirstOrDefault().HRME_Id);
                                                idtrans11.HRELT_LeaveReason = "OD";
                                                idtrans11.HRELT_Status = "Applied";
                                                idtrans11.UpdatedDate = DateTime.Now;
                                                _lmContext.Update(idtrans11);
                                                _lmContext.SaveChanges();
                                            }

                                            //Leave Id Creation from transaction numbering.
                                            var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                            if (transnumconfigsettings.Count > 0)
                                            {
                                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                                Master_NumberingDTO num = new Master_NumberingDTO();
                                                num.MI_Id = _category.MI_Id;
                                                num.ASMAY_Id = _category.asmay_id;
                                                num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                                num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                                num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                                num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                                num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                                num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                                num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                                num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                                num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                                num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                                num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                                num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                                num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                                num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                                num.IMN_Flag = "LeaveNo";
                                                _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                            }

                                            //LEAVE APPLICATION
                                            objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                            objpge.CreatedDate = DateTime.Now;
                                            objpge.UpdatedDate = DateTime.Now;
                                            objpge.HRELAP_CreatedBy = _category.UserId;
                                            objpge.HRELAP_UpdatedBy = _category.UserId;
                                            objpge.HRELAP_ActiveFlag = true;
                                            objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                            objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                            objpge.HRELAP_ApplicationStatus = "Applied";
                                            objpge.HRELAP_SanctioningLevel = "1";
                                            objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                            objpge.HRELAP_SupportingDocument = _category.HRELT_SupportingDocument;
                                            objpge.HRELAP_CompOffAdjestFlg = _category.frmToDates[0].LOPAdjest;
                                            _lmContext.Add(objpge);
                                            count = _lmContext.SaveChanges();

                                            //LEAVE APPLICATION DETAILS
                                            if (objpge.HRELAP_Id > 0)
                                            {
                                                HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                                updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                                updatedet.HRELAPD_LeaveStatus = "Applied";
                                                updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                                updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                                updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                                updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                                updatedet.UpdatedDate = DateTime.Now;
                                                updatedet.CreatedDate = DateTime.Now;
                                                updatedet.HRELAPD_CreatedBy = _category.UserId;
                                                updatedet.HRELAPD_UpdatedBy = _category.UserId;
                                                updatedet.HRELAPD_ActiveFlag = true;
                                                //Mapper.Map(objpge1, resultempltransDetails);
                                                _lmContext.Add(updatedet);
                                                _lmContext.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (checkbalanceleave.FirstOrDefault().HRELS_CBLeaves > 0 && bal_leave >= 0)
                                    {
                                        //var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                        //                      from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                        //                      where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id
                                        //                      && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id
                                        //                      && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                        //                      && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                        //                      && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                        //                      select new LeaveCreditDTO
                                        //                      {
                                        //                          HRELAP_Id = m.HRELAP_Id
                                        //                      }).ToList();

                                        List<LeaveCreditDTO> checkduplicate = new List<LeaveCreditDTO>();
                                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "HR_Leave_Dublicate_Check_New";
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                            //{
                                            //    Value = _category.MI_Id
                                            //});

                                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                                            {
                                                Value = qyery1.FirstOrDefault().HRME_Id
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@HRML_Id", SqlDbType.BigInt)
                                            {
                                                Value = _category.temp_table_data.FirstOrDefault().HRML_Id
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date)
                                            {
                                                Value = fromdate
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.Date)
                                            {
                                                Value = todate
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
                                                        checkduplicate.Add(new LeaveCreditDTO
                                                        {
                                                            HRELAP_Id = Convert.ToInt64(dataReader1["HRELAP_Id"]),
                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }

                                        //var Rejectdublicatecheck = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                        //                            from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                        //                            where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id
                                        //                            && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id
                                        //                            && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                        //                            && m.HRELAP_ApplicationStatus == "Rejected"
                                        //                            && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                        //                            select new LeaveCreditDTO
                                        //                            {
                                        //                                HRELAP_Id = m.HRELAP_Id
                                        //                            }).ToList();


                                        List<LeaveCreditDTO> Rejectdublicatecheck = new List<LeaveCreditDTO>();
                                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "HR_Leave_Reject_Check_New";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                            //{
                                            //    Value = _category.MI_Id
                                            //});

                                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                                            {
                                                Value = qyery1.FirstOrDefault().HRME_Id
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@HRML_Id", SqlDbType.BigInt)
                                            {
                                                Value = _category.temp_table_data.FirstOrDefault().HRML_Id
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date)
                                            {
                                                Value = fromdate
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.Date)
                                            {
                                                Value = todate
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
                                                        Rejectdublicatecheck.Add(new LeaveCreditDTO
                                                        {
                                                            HRELAP_Id = Convert.ToInt64(dataReader1["HRELAP_Id"]),
                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }



                                        if (Rejectdublicatecheck.Count > 0)
                                        {

                                            var updateleaveapplication = _lmContext.HR_Emp_Leave_ApplicationDMO.Single(d => d.MI_Id == _category.MI_Id && d.HRME_Id == qyery1.FirstOrDefault().HRME_Id && d.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate && d.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate && d.HRELAP_Id == Rejectdublicatecheck.FirstOrDefault().HRELAP_Id && d.HRELAP_ApplicationStatus == "Rejected");
                                            var updateleaveapplicationdetails = _lmContext.HR_Emp_Leave_Appl_DetailsDMO.Single(d => d.HRELAP_Id == Rejectdublicatecheck.FirstOrDefault().HRELAP_Id && d.HRELAPD_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate && d.HRELAPD_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate && d.HRELAP_Id == Rejectdublicatecheck.FirstOrDefault().HRELAP_Id && d.HRELAPD_LeaveStatus == "Rejected");
                                            var updateleaveTrans = _lmContext.HR_Emp_Leave_Trans_DMO.Single(d => d.MI_Id == _category.MI_Id && d.HRME_Id == qyery1.FirstOrDefault().HRME_Id && d.HRELT_FromDate.Date.ToString("dd/MM/yyyy") == fromdate && d.HRELT_ToDate.Date.ToString("dd/MM/yyyy") == todate && d.HRELT_Status == "Rejected");
                                            // var updateleaveTransdetails = _lmContext.HR_Emp_Leave_Trans_Details_DMO.Single(d => d.MI_Id == _category.MI_Id && d.HRME_Id == qyery1.FirstOrDefault().HRME_Id && d.HRELTD_FromDate.Date.ToString("dd/MM/yyyy") == fromdate  && d.HRELTD_ToDate.Date.ToString("dd/MM/yyyy") == todate && d.HRELT_Id == updateleaveTrans.HRELT_Id);
                                            updateleaveapplication.HRELAP_ActiveFlag = false;
                                            updateleaveapplicationdetails.HRELAPD_ActiveFlag = false;
                                            updateleaveTrans.HRELT_ActiveFlag = false;
                                            _lmContext.Update(updateleaveapplication);
                                            _lmContext.Update(updateleaveapplicationdetails);
                                            _lmContext.Update(updateleaveTrans);
                                            _lmContext.SaveChanges();


                                            objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                            objpge.CreatedDate = DateTime.Now;
                                            objpge.UpdatedDate = DateTime.Now;
                                            objpge.HRELAP_ActiveFlag = true;
                                            //HR_Emp_Leave_Trans_DMO
                                            objLvTrans.MI_Id = _category.MI_Id;
                                            objLvTrans.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                            objLvTrans.HRMLY_Id = leave_year.FirstOrDefault().HRMLY_Id;
                                            objLvTrans.HRELT_LeaveId = leave_id;
                                            //objLvTrans.HRELT_Reportingdate = _category.HRELT_Reportingdate;
                                            objLvTrans.HRELT_Reportingdate = Convert.ToDateTime(_category.HRELAP_ReportingDate);
                                            objLvTrans.HRELT_LeaveReason = _category.HRELT_LeaveReason;
                                            objLvTrans.HRELT_Status = "Applied";
                                            objLvTrans.CreatedDate = DateTime.Now;
                                            objLvTrans.UpdatedDate = DateTime.Now;
                                            objLvTrans.HRELT_ActiveFlag = true;
                                            objLvTrans.HRELT_SupportingDocument = _category.HRELT_SupportingDocument;
                                            objLvTrans.HRELT_lopAdjestFlg = _category.frmToDates[0].LOPAdjest;


                                            //HR_Emp_Leave_Trans_Details_DMO
                                            objLvTrDetail.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                            objLvTrDetail.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                            objLvTrDetail.MI_Id = _category.MI_Id;
                                            objLvTrDetail.HRELTD_LWPFlag = false;
                                            objLvTrDetail.CreatedDate = DateTime.Now;
                                            objLvTrDetail.UpdatedDate = DateTime.Now;

                                            //Leave Id Creation from transaction numbering.
                                            var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id
                                            && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                            if (transnumconfigsettings.Count > 0)
                                            {
                                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                                Master_NumberingDTO num = new Master_NumberingDTO();
                                                num.MI_Id = _category.MI_Id;
                                                num.ASMAY_Id = _category.asmay_id;
                                                num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                                num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                                num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                                num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                                num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                                num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                                num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                                num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                                num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                                num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                                num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                                num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                                num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                                num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                                num.IMN_Flag = "LeaveNo";
                                                _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                            }
                                            else
                                            {
                                                _category.returnmsg = "Nomapping";
                                            }
                                            if (_category.HRELAP_ApplicationID != null)
                                            {
                                                //HR_Emp_Leave_Trans_DMO
                                                objLvTrans.HRELT_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                                objLvTrans.HRELT_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                                objLvTrans.HRELT_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                                //HR_Emp_Leave_Trans_Details_DMO
                                                objLvTrDetail.HRELTD_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                                objLvTrDetail.HRELTD_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                                objLvTrDetail.HRELTD_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                                objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                                objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                                objpge.HRELAP_ApplicationStatus = "Applied";
                                                objpge.HRELAP_SanctioningLevel = "1";
                                                objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                                objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                                objpge.HRELAP_SupportingDocument = _category.HRELT_SupportingDocument;
                                                objpge.HRELAP_CompOffAdjestFlg = _category.frmToDates[0].LOPAdjest;
                                                _lmContext.Add(objpge);
                                                count = _lmContext.SaveChanges();

                                                _lmContext.Add(objLvTrans);
                                                _lmContext.SaveChanges();

                                                var hrelt_id = _lmContext.HR_Emp_Leave_Trans_DMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELT_ActiveFlag == true
                                                && z.HRELT_FromDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate)
                                                && z.HRELT_ToDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate)
                                                && z.HRELT_TotDays == (_category.frmToDates.FirstOrDefault().HRELAP_TotalDays)
                                                && z.HRME_Id == qyery1.FirstOrDefault().HRME_Id && z.HRELT_LeaveId == leave_id
                                                && z.HRELT_LeaveReason == _category.HRELT_LeaveReason).Select(s => s.HRELT_Id).FirstOrDefault();
                                                objLvTrDetail.HRELT_Id = hrelt_id;
                                                _lmContext.Add(objLvTrDetail);
                                                _lmContext.SaveChanges();
                                            }
                                            var contactExists = count;
                                            HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);


                                            if (objpge.HRELAP_Id > 0)
                                            {
                                                HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                                updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                                updatedet.HRELAPD_LeaveStatus = "Applied";
                                                updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                                updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                                updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                                updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                                updatedet.UpdatedDate = DateTime.Now;
                                                updatedet.CreatedDate = DateTime.Now;
                                                updatedet.HRELAPD_UpdatedBy = _category.UserId;
                                                updatedet.HRELAPD_CreatedBy = _category.UserId;
                                                updatedet.HRELAPD_ActiveFlag = true;
                                                //Mapper.Map(objpge1, resultempltransDetails);
                                                _lmContext.Add(updatedet);
                                                _lmContext.SaveChanges();

                                                string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id
                                                && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                                                long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id
                                                && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();

                                                _category.HRME_MobileNo = mobileno;
                                                _category.HRME_Id = qyery1.FirstOrDefault().HRME_Id;

                                                string ccmail = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                                                 from b in _lmContext.HR_Leave_Authorisation_DMO
                                                                 from c in _lmContext.HR_Master_Employee_DMO
                                                                 from d in _lmContext.Emp_Email_Id
                                                                 where (c.HRME_Id == a.IVRMUL_Id && c.HRME_Id == d.HRME_Id && a.HRLA_Id == b.HRLA_Id
                                                                 && a.HRME_Id == checkbalanceleave[0].HRME_Id && d.HRMEM_DeFaultFlag == "default"
                                                                 && b.HRML_Id == leave_id && a.HRLAON_SanctionLevelNo == 1)
                                                                 select d.HRMEM_EmailId).FirstOrDefault();
                                                var leavename = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id).Select(t => t.HRML_LeaveName).FirstOrDefault();

                                                sendmail(checkbalanceleave[0].MI_Id, emailid, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate, _category.frmToDates.FirstOrDefault().HRELAP_ToDate, _category.HRELAP_LeaveReason, ccmail, leavename, employeecode);

                                                var devicenew = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == checkbalanceleave[0].HRME_Id).Select(t => t.HRME_AppDownloadedDeviceId).FirstOrDefault();
                                                _category.AMST_AppDownloadedDeviceId = devicenew;

                                                callnotification(devicenew, objpge.HRELAP_Id, _category.MI_Id, _category, "LEAVE MANAGEMENT");
                                            }
                                            else
                                            {

                                            }

                                            if (contactExists > 0)
                                            {
                                                _category.returnval = true;
                                            }
                                            else
                                            {
                                                _category.returnval = false;
                                            }
                                        }
                                        else
                                        {
                                            if (checkduplicate.Count == 0)
                                            {
                                                objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                                objpge.CreatedDate = DateTime.Now;
                                                objpge.UpdatedDate = DateTime.Now;
                                                objpge.HRELAP_ActiveFlag = true;
                                                //HR_Emp_Leave_Trans_DMO
                                                objLvTrans.MI_Id = _category.MI_Id;
                                                objLvTrans.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                                objLvTrans.HRMLY_Id = leave_year.FirstOrDefault().HRMLY_Id;
                                                objLvTrans.HRELT_LeaveId = leave_id;
                                                //objLvTrans.HRELT_Reportingdate = _category.HRELT_Reportingdate;
                                                objLvTrans.HRELT_Reportingdate = Convert.ToDateTime(_category.HRELAP_ReportingDate);
                                                objLvTrans.HRELT_LeaveReason = _category.HRELT_LeaveReason;
                                                objLvTrans.HRELT_Status = "Applied";
                                                objLvTrans.CreatedDate = DateTime.Now;
                                                objLvTrans.UpdatedDate = DateTime.Now;
                                                objLvTrans.HRELT_ActiveFlag = true;
                                                objLvTrans.HRELT_SupportingDocument = _category.HRELT_SupportingDocument;
                                                objLvTrans.HRELT_lopAdjestFlg = _category.frmToDates[0].LOPAdjest;

                                                //HR_Emp_Leave_Trans_Details_DMO
                                                objLvTrDetail.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                                objLvTrDetail.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                                objLvTrDetail.MI_Id = _category.MI_Id;
                                                objLvTrDetail.HRELTD_LWPFlag = false;
                                                objLvTrDetail.CreatedDate = DateTime.Now;
                                                objLvTrDetail.UpdatedDate = DateTime.Now;

                                                //Leave Id Creation from transaction numbering.
                                                var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id
                                                && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                                if (transnumconfigsettings.Count > 0)
                                                {
                                                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                                    Master_NumberingDTO num = new Master_NumberingDTO();
                                                    num.MI_Id = _category.MI_Id;
                                                    num.ASMAY_Id = _category.asmay_id;
                                                    num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                                    num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                                    num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                                    num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                                    num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                                    num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                                    num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                                    num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                                    num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                                    num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                                    num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                                    num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                                    num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                                    num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                                    num.IMN_Flag = "LeaveNo";
                                                    _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                                }
                                                else
                                                {
                                                    _category.returnmsg = "Nomapping";
                                                }
                                                if (_category.HRELAP_ApplicationID != null)
                                                {
                                                    //HR_Emp_Leave_Trans_DMO
                                                    objLvTrans.HRELT_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                                    objLvTrans.HRELT_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                                    objLvTrans.HRELT_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                                    //HR_Emp_Leave_Trans_Details_DMO
                                                    objLvTrDetail.HRELTD_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                                    objLvTrDetail.HRELTD_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                                    objLvTrDetail.HRELTD_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                                    objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                                    objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                                    objpge.HRELAP_ApplicationStatus = "Applied";
                                                    objpge.HRELAP_SanctioningLevel = "1";
                                                    objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                                    objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                                    objpge.HRELAP_SupportingDocument = _category.HRELT_SupportingDocument;
                                                    objpge.HRELAP_CompOffAdjestFlg = _category.frmToDates[0].LOPAdjest;
                                                    _lmContext.Add(objpge);
                                                    count = _lmContext.SaveChanges();

                                                    _lmContext.Add(objLvTrans);
                                                    _lmContext.SaveChanges();

                                                    var hrelt_id = _lmContext.HR_Emp_Leave_Trans_DMO.Where(z => z.MI_Id == _category.MI_Id
                                                    && z.HRELT_FromDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate)
                                                    && z.HRELT_ToDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate)
                                                    && z.HRELT_TotDays == (_category.frmToDates.FirstOrDefault().HRELAP_TotalDays)
                                                    && z.HRME_Id == qyery1.FirstOrDefault().HRME_Id && z.HRELT_LeaveId == leave_id
                                                    && z.HRELT_LeaveReason == _category.HRELT_LeaveReason).Select(s => s.HRELT_Id).FirstOrDefault();
                                                    objLvTrDetail.HRELT_Id = hrelt_id;
                                                    _lmContext.Add(objLvTrDetail);
                                                    _lmContext.SaveChanges();
                                                }
                                                var contactExists = count;
                                                HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);


                                                if (objpge.HRELAP_Id > 0)
                                                {
                                                    HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                                    updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                                    updatedet.HRELAPD_LeaveStatus = "Applied";
                                                    updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                                    updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                                    updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                                    updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                                    updatedet.UpdatedDate = DateTime.Now;
                                                    updatedet.CreatedDate = DateTime.Now;
                                                    updatedet.HRELAPD_UpdatedBy = _category.UserId;
                                                    updatedet.HRELAPD_CreatedBy = _category.UserId;
                                                    updatedet.HRELAPD_ActiveFlag = true;
                                                    //Mapper.Map(objpge1, resultempltransDetails);
                                                    _lmContext.Add(updatedet);
                                                    _lmContext.SaveChanges();
                                                    HRELAPD_Id = updatedet.HRELAPD_Id;
                                                    string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id
                                                    && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                                                    long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id
                                                    && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();

                                                    _category.HRME_MobileNo = mobileno;
                                                    //_category.hrme_id = qyery1.FirstOrDefault().HRME_Id;

                                                    string ccmail = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                                                     from b in _lmContext.HR_Leave_Authorisation_DMO
                                                                     from d in _lmContext.Emp_Email_Id
                                                                     where (a.IVRMUL_Id == d.HRME_Id && a.HRLA_Id == b.HRLA_Id && a.HRME_Id == checkbalanceleave[0].HRME_Id && d.HRMEM_DeFaultFlag == "default"
                                                                     && b.HRML_Id == leave_id && a.HRLAON_SanctionLevelNo == 1)
                                                                     select d.HRMEM_EmailId).FirstOrDefault();



                                                    var leavename = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id).Select(t => t.HRML_LeaveName).FirstOrDefault();

                                                    sendmail(checkbalanceleave[0].MI_Id, ccmail, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate, _category.frmToDates.FirstOrDefault().HRELAP_ToDate, _category.HRELAP_LeaveReason, emailid, leavename, employeecode);

                                                    var devicenew = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == checkbalanceleave[0].HRME_Id).Select(t => t.HRME_AppDownloadedDeviceId).FirstOrDefault();
                                                    _category.AMST_AppDownloadedDeviceId = devicenew;

                                                    callnotification(devicenew, objpge.HRELAP_Id, _category.MI_Id, _category, "LEAVE MANAGEMENT");
                                                }
                                                else
                                                {

                                                }

                                                if (contactExists > 0)
                                                {
                                                    _category.returnval = true;
                                                }
                                                else
                                                {
                                                    _category.returnval = false;
                                                }
                                            }
                                            else
                                            {
                                                _category.returnduplicatestatus = "Duplicate";
                                            }
                                        }


                                        if (_category.HRME_Id > 0)
                                        {

                                            var approvedresult = _lmContext.Database.ExecuteSqlCommand("HR_Leave_proxy_insert @p0,@p1,@p2,@p3,@p4", HRELAPD_Id, _category.HRME_Id, DateTime.Now, "", _category.UserId);

                                        }




                                    }
                                    else
                                    {
                                        _category.message = "You Crossed Your Leave Limits";
                                        List<HR_Emp_Leave_ApplicationDMO> mm_events = new List<HR_Emp_Leave_ApplicationDMO>();
                                        mm_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                                        _category.master_eventlist = mm_events.ToArray();
                                        return _category;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _category.message = "Leave Already applied on these dates";
                        List<HR_Emp_Leave_ApplicationDMO> m_events = new List<HR_Emp_Leave_ApplicationDMO>();
                        m_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                        _category.master_eventlist = m_events.ToArray();
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }

            return _category;
        }
        public LeaveCreditDTO saveadminLeave(LeaveCreditDTO _category)
        {
            int count = 0;
            HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(_category);

            HR_Emp_Leave_Trans_DMO objLvTrans = Mapper.Map<HR_Emp_Leave_Trans_DMO>(_category);
            HR_Emp_Leave_Trans_Details_DMO objLvTrDetail = Mapper.Map<HR_Emp_Leave_Trans_Details_DMO>(_category);

            var leave_year = _lmContext.HR_Master_LeaveYear_DMO.Where(e => e.MI_Id == _category.MI_Id && DateTime.Now.Date >= e.HRMLY_FromDate.Date && DateTime.Now.Date <= e.HRMLY_ToDate.Date).ToList();

            var leave_id = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_LeaveName == _category.temp_table_data.FirstOrDefault().HRML_LeaveName && p.MI_Id == _category.MI_Id).Select(p => p.HRML_Id).FirstOrDefault();

            var employeedetails = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == objpge.HRME_Id).ToList().FirstOrDefault();
            var employeename = employeedetails.HRME_EmployeeFirstName + " " + (employeedetails.HRME_EmployeeMiddleName == null ? "" : employeedetails.HRME_EmployeeMiddleName) + " " + (employeedetails.HRME_EmployeeLastName == null ? "" : employeedetails.HRME_EmployeeLastName);
            var employeecode = employeedetails.HRME_EmployeeCode;

            try
            {
                var duplicate = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELAP_FromDate >= _category.frmToDates[0].HRELAP_FromDate && z.HRELAP_ToDate <= _category.frmToDates[0].HRELAP_ToDate && z.HRELAP_ActiveFlag == true && z.HRME_Id == _category.HRME_Id).ToArray();
                if (duplicate.Length == 0)
                {
                    if (objpge.HRELAP_Id > 0)
                    {
                        var resultCount = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_FromDate <= objpge.HRELAP_FromDate && t.HRELAP_ToDate >= objpge.HRELAP_ToDate).Count();

                        if (resultCount == 0)
                        {
                            var result = _lmContext.HR_Emp_Leave_ApplicationDMO.Single(t => t.MI_Id == objpge.MI_Id);

                            result.MI_Id = objpge.MI_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.HRELAP_ApplicationDate = objpge.HRELAP_ApplicationDate;
                            result.HRELAP_FromDate = objpge.HRELAP_FromDate;
                            result.HRELAP_ToDate = objpge.HRELAP_ToDate;
                            result.HRELAP_TotalDays = objpge.HRELAP_TotalDays;
                            result.HRELAP_LeaveReason = objpge.HRELAP_LeaveReason;
                            result.HRELAP_ContactNoOnLeave = objpge.HRELAP_ContactNoOnLeave;
                            result.HRELAP_ReportingDate = objpge.HRELAP_ReportingDate;
                            result.UpdatedDate = DateTime.Now;
                            result.HRELAP_UpdatedBy = _category.UserId;
                            // result.HRELAP_ApplicationID = objpge.HRELAP_ApplicationID;


                            _lmContext.Update(result);
                            var contactExists = _lmContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                _category.returnval = true;
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        }
                        else
                        {
                            _category.returnduplicatestatus = "Duplicate";
                            return _category;
                        }
                    }
                    else
                    {
                        var checkbalanceleave = _lmContext.HR_Emp_Leave_StatusDMO.Where(d => d.MI_Id == _category.MI_Id && d.HRME_Id == _category.HRME_Id && d.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id && d.HRMLY_Id == leave_year.FirstOrDefault().HRMLY_Id).ToList();
                        var bal_leave = checkbalanceleave.FirstOrDefault().HRELS_CBLeaves - _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                        var fromdate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate).ToString("d");
                        var todate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate).ToString("d");

                        string leavetype = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_Id == _category.temp_table_data[0].HRML_Id && p.MI_Id == _category.MI_Id).Select(p => p.HRML_LeaveName).FirstOrDefault();

                        if (leavetype == "ON DUTY")
                        {
                            var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                  from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                  where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == _category.HRME_Id
                                                  && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                  && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                  && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                                  select new LeaveCreditDTO
                                                  {
                                                      HRELAP_Id = m.HRELAP_Id
                                                  }).ToList();

                            if (checkduplicate.Count == 0)
                            {
                                objpge.HRME_Id = _category.HRME_Id;
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.HRELAP_UpdatedBy = _category.UserId;
                                objpge.HRELAP_UpdatedBy = _category.UserId;
                                objpge.HRELAP_ActiveFlag = true;
                                //HR_Emp_Leave_Trans_DMO
                                objLvTrans.MI_Id = _category.MI_Id;
                                objLvTrans.HRME_Id = _category.HRME_Id;
                                objLvTrans.HRMLY_Id = leave_year.FirstOrDefault().HRMLY_Id;
                                objLvTrans.HRELT_LeaveId = leave_id;
                                objLvTrans.HRELT_Reportingdate = _category.HRELT_Reportingdate;
                                objLvTrans.HRELT_LeaveReason = _category.HRELT_LeaveReason;
                                objLvTrans.HRELT_Status = "Applied";
                                objLvTrans.CreatedDate = DateTime.Now;
                                objLvTrans.UpdatedDate = DateTime.Now;
                                objLvTrans.HRELT_CreatedBy = _category.UserId;
                                objLvTrans.HRELT_UpdatedBy = _category.UserId;
                                objLvTrans.HRELT_ActiveFlag = true;

                                objLvTrDetail.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                objLvTrDetail.HRME_Id = _category.HRME_Id;
                                objLvTrDetail.MI_Id = _category.MI_Id;
                                objLvTrDetail.HRELTD_LWPFlag = false;
                                objLvTrDetail.CreatedDate = DateTime.Now;
                                objLvTrDetail.UpdatedDate = DateTime.Now;
                                objLvTrDetail.HRELTD_UpdatedBy = _category.UserId;
                                objLvTrDetail.HRELTD_CreatedBy = _category.UserId;

                                //Leave Id Creation from transaction numbering.
                                var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                if (transnumconfigsettings.Count > 0)
                                {
                                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                    Master_NumberingDTO num = new Master_NumberingDTO();
                                    num.MI_Id = _category.MI_Id;
                                    num.ASMAY_Id = _category.asmay_id;
                                    num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                    num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                    num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                    num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                    num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                    num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                    num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                    num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                    num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                    num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                    num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                    num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                    num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                    num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                    num.IMN_Flag = "LeaveNo";
                                    _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                }
                                else
                                {
                                    _category.returnmsg = "Nomapping";
                                }

                                if (_category.HRELAP_ApplicationID != null)
                                {
                                    //HR_Emp_Leave_Trans_DMO
                                    objLvTrans.HRELT_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                    objLvTrans.HRELT_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                    objLvTrans.HRELT_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                    objLvTrDetail.HRELTD_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                    objLvTrDetail.HRELTD_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                    objLvTrDetail.HRELTD_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                    objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                    objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                    objpge.HRELAP_ApplicationStatus = "Applied";
                                    objpge.HRELAP_SanctioningLevel = "1";
                                    objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                    objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                    _lmContext.Add(objpge);
                                    count = _lmContext.SaveChanges();

                                    _lmContext.Add(objLvTrans);
                                    _lmContext.SaveChanges();

                                    var hrelt_id = _lmContext.HR_Emp_Leave_Trans_DMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELT_FromDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate) && z.HRELT_ToDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate) && z.HRELT_TotDays == (_category.frmToDates.FirstOrDefault().HRELAP_TotalDays) && z.HRME_Id == _category.HRME_Id && z.HRELT_LeaveId == leave_id && z.HRELT_LeaveReason == _category.HRELT_LeaveReason).Select(s => s.HRELT_Id).FirstOrDefault();

                                    objLvTrDetail.HRELT_Id = hrelt_id;
                                    _lmContext.Add(objLvTrDetail);
                                    _lmContext.SaveChanges();
                                }
                                var contactExists = count;
                                HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);

                                if (objpge.HRELAP_Id > 0)
                                {
                                    HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                    updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                    updatedet.HRELAPD_LeaveStatus = "Applied";
                                    updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                    updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                    updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                    updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                    updatedet.UpdatedDate = DateTime.Now;
                                    updatedet.CreatedDate = DateTime.Now;
                                    updatedet.HRELAPD_CreatedBy = _category.UserId;
                                    updatedet.HRELAPD_UpdatedBy = _category.UserId;
                                    updatedet.HRELAPD_ActiveFlag = true;
                                    //Mapper.Map(objpge1, resultempltransDetails);
                                    _lmContext.Add(updatedet);
                                    _lmContext.SaveChanges();

                                    string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                    long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();

                                    //string ccmail = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                    //                 from b in _lmContext.HR_Leave_Authorisation_DMO
                                    //                 where (a.HRLA_Id == b.HRLA_Id && a.HRME_Id == checkbalanceleave[0].HRME_Id && b.HRML_Id == leave_id)
                                    //                 select b.HRLA_EmailTo).FirstOrDefault();

                                    string ccmail = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                                     from b in _lmContext.HR_Leave_Authorisation_DMO
                                                     from c in _lmContext.HR_Master_Employee_DMO
                                                     from d in _lmContext.Emp_Email_Id
                                                     where (c.HRME_Id == a.IVRMUL_Id && c.HRME_Id == d.HRME_Id && a.HRLA_Id == b.HRLA_Id
                                                     && a.HRME_Id == checkbalanceleave[0].HRME_Id && d.HRMEM_DeFaultFlag == "default"
                                                     && b.HRML_Id == leave_id && a.HRLAON_SanctionLevelNo == 1)
                                                     select d.HRMEM_EmailId).FirstOrDefault();
                                    var leavename = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id).Select(t => t.HRML_LeaveName).FirstOrDefault();

                                    sendmail(checkbalanceleave[0].MI_Id, emailid, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate, _category.frmToDates.FirstOrDefault().HRELAP_ToDate, _category.HRELAP_LeaveReason, ccmail, leavename, employeecode);

                                }
                            }
                        }
                        else
                        {
                            if (checkbalanceleave.FirstOrDefault().HRELS_CBLeaves > 0 && bal_leave >= 0)
                            {
                                var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                      from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                      where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == _category.HRME_Id
                                                      && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                      && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                      && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                                      select new LeaveCreditDTO
                                                      {
                                                          HRELAP_Id = m.HRELAP_Id
                                                      }).ToList();

                                if (checkduplicate.Count == 0)
                                {
                                    objpge.HRME_Id = _category.HRME_Id;
                                    objpge.CreatedDate = DateTime.Now;
                                    objpge.UpdatedDate = DateTime.Now;
                                    objpge.HRELAP_UpdatedBy = _category.UserId;
                                    objpge.HRELAP_UpdatedBy = _category.UserId;
                                    objpge.HRELAP_ActiveFlag = true;

                                    //HR_Emp_Leave_Trans_DMO
                                    objLvTrans.MI_Id = _category.MI_Id;
                                    objLvTrans.HRME_Id = _category.HRME_Id;
                                    objLvTrans.HRMLY_Id = leave_year.FirstOrDefault().HRMLY_Id;
                                    objLvTrans.HRELT_LeaveId = leave_id;
                                    objLvTrans.HRELT_Reportingdate = _category.HRELT_Reportingdate;
                                    objLvTrans.HRELT_LeaveReason = _category.HRELT_LeaveReason;
                                    objLvTrans.HRELT_Status = "Applied";
                                    objLvTrans.CreatedDate = DateTime.Now;
                                    objLvTrans.UpdatedDate = DateTime.Now;
                                    objLvTrans.HRELT_CreatedBy = _category.UserId;
                                    objLvTrans.HRELT_UpdatedBy = _category.UserId;
                                    objLvTrans.HRELT_ActiveFlag = true;

                                    objLvTrDetail.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                    objLvTrDetail.HRME_Id = _category.HRME_Id;
                                    objLvTrDetail.MI_Id = _category.MI_Id;
                                    objLvTrDetail.HRELTD_LWPFlag = false;
                                    objLvTrDetail.CreatedDate = DateTime.Now;
                                    objLvTrDetail.UpdatedDate = DateTime.Now;
                                    objLvTrDetail.UpdatedDate = DateTime.Now;
                                    objLvTrDetail.HRELTD_UpdatedBy = _category.UserId;
                                    objLvTrDetail.HRELTD_CreatedBy = _category.UserId;

                                    //Leave Id Creation from transaction numbering.
                                    var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                    if (transnumconfigsettings.Count > 0)
                                    {
                                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                        Master_NumberingDTO num = new Master_NumberingDTO();
                                        num.MI_Id = _category.MI_Id;
                                        num.ASMAY_Id = _category.asmay_id;
                                        num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                        num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                        num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                        num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                        num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                        num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                        num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                        num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                        num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                        num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                        num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                        num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                        num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                        num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                        num.IMN_Flag = "LeaveNo";
                                        _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                    }
                                    else
                                    {
                                        _category.returnmsg = "Nomapping";
                                    }
                                    if (_category.HRELAP_ApplicationID != null)
                                    {
                                        //HR_Emp_Leave_Trans_DMO
                                        objLvTrans.HRELT_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        objLvTrans.HRELT_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                        objLvTrans.HRELT_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                        objLvTrDetail.HRELTD_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        objLvTrDetail.HRELTD_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                        objLvTrDetail.HRELTD_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                        objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                        objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                        objpge.HRELAP_ApplicationStatus = "Applied";
                                        objpge.HRELAP_SanctioningLevel = "1";
                                        objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                        _lmContext.Add(objpge);
                                        count = _lmContext.SaveChanges();

                                        _lmContext.Add(objLvTrans);
                                        _lmContext.SaveChanges();

                                        var hrelt_id = _lmContext.HR_Emp_Leave_Trans_DMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELT_FromDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate) && z.HRELT_ToDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate) && z.HRELT_TotDays == (_category.frmToDates.FirstOrDefault().HRELAP_TotalDays) && z.HRME_Id == _category.HRME_Id && z.HRELT_LeaveId == leave_id && z.HRELT_LeaveReason == _category.HRELT_LeaveReason).Select(s => s.HRELT_Id).FirstOrDefault();

                                        objLvTrDetail.HRELT_Id = hrelt_id;
                                        _lmContext.Add(objLvTrDetail);
                                        _lmContext.SaveChanges();
                                    }
                                    var contactExists = count;
                                    HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);


                                    if (objpge.HRELAP_Id > 0)
                                    {
                                        HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                        updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                        updatedet.HRELAPD_LeaveStatus = "Applied";
                                        updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                        updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                        updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                        updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                        updatedet.UpdatedDate = DateTime.Now;
                                        updatedet.CreatedDate = DateTime.Now;

                                        updatedet.HRELAPD_ActiveFlag = true;
                                        //Mapper.Map(objpge1, resultempltransDetails);
                                        _lmContext.Add(updatedet);
                                        _lmContext.SaveChanges();

                                        string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                        long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();

                                        string ccmail = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                                         from b in _lmContext.HR_Leave_Authorisation_DMO
                                                         from c in _lmContext.HR_Master_Employee_DMO
                                                         from d in _lmContext.Emp_Email_Id
                                                         where (c.HRME_Id == a.IVRMUL_Id && c.HRME_Id == d.HRME_Id && a.HRLA_Id == b.HRLA_Id
                                                         && a.HRME_Id == checkbalanceleave[0].HRME_Id && d.HRMEM_DeFaultFlag == "default"
                                                         && b.HRML_Id == leave_id && a.HRLAON_SanctionLevelNo == 1)
                                                         select d.HRMEM_EmailId).FirstOrDefault();
                                        var leavename = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id).Select(t => t.HRML_LeaveName).FirstOrDefault();

                                        sendmail(checkbalanceleave[0].MI_Id, emailid, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate, _category.frmToDates.FirstOrDefault().HRELAP_ToDate, _category.HRELAP_LeaveReason, ccmail, leavename, employeecode);

                                    }
                                    else
                                    {

                                    }

                                    if (contactExists > 0)
                                    {
                                        _category.returnval = true;

                                    }
                                    else
                                    {
                                        _category.returnval = false;
                                    }
                                }
                                else
                                {
                                    _category.returnduplicatestatus = "Duplicate";
                                }

                            }
                            else
                            {
                                _category.message = "You Crossed Your Leave Limits";
                                List<HR_Emp_Leave_ApplicationDMO> mm_events = new List<HR_Emp_Leave_ApplicationDMO>();
                                mm_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                                _category.master_eventlist = mm_events.ToArray();
                                return _category;
                            }
                        }
                    }
                }
                else
                {
                    _category.message = "Leave Already applied on these dates";
                    List<HR_Emp_Leave_ApplicationDMO> m_events = new List<HR_Emp_Leave_ApplicationDMO>();
                    m_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                    _category.master_eventlist = m_events.ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }
        public LeaveCreditDTO getonlineLeavestatus(LeaveCreditDTO data)
        {
            try
            {
                var qyery1 = (from q in _db.Staff_User_Login
                              from r in _db.HR_Master_Employee_DMO
                              where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                              select new LeaveCreditDTO
                              {
                                  HRME_Id = r.HRME_Id,
                              }).Distinct().ToArray();
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "OptionalLeaves";
                    cmd.CommandType = CommandType.StoredProcedure;
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.optionalleavedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                if (qyery1.Length > 0)
                {
                    var hrmeid = qyery1.FirstOrDefault().HRME_Id;



                    //data.leave_name = (from a in _lmContext.HR_Master_Leave_DMO
                    //                   from b in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                    //                   from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                    //                   where (a.HRML_Id == b.HRML_Id && c.HRELAP_Id == b.HRELAP_Id && a.MI_Id == data.MI_Id && c.HRME_Id == hrmeid)
                    //                   select new LeaveCreditDTO
                    //                   {
                    //                       HRELAP_Id = c.HRELAP_Id,
                    //                       HRML_LeaveName = a.HRML_LeaveName,
                    //                       HRML_LeaveCode = a.HRML_LeaveCode,
                    //                       HRELAP_FromDate = c.HRELAP_FromDate,
                    //                       HRELAP_ToDate = c.HRELAP_ToDate,
                    //                       HRELTD_TotDays = b.HRELAPD_TotalDays,
                    //                       HRELAP_ApplicationStatus = b.HRELAPD_LeaveStatus,
                    //                       HRML_Id = b.HRML_Id,
                    //                       HRME_IdMy = c.HRME_Id,
                    //                       HRELAP_LeaveReason = c.HRELAP_LeaveReason,
                    //                       HRELAPD_OutTime=b.HRELAPD_OutTime,
                    //                       HRELAPD_InTime=b.HRELAPD_InTime
                    //                   }).Distinct().OrderByDescending(d => d.HRELAP_Id).ToArray();

                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LEAVE_NAME_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = hrmeid
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
                            data.leave_name = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LEAVE_NAME_DETAILSCompoff";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = hrmeid
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
                            data.leave_namelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Rejected_Leaves_Reconsider";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                        {
                            Value = hrmeid
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
                            data.lrlist = retObject.ToArray();
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
        public LeaveCreditDTO getSingleEmpLeavestatus(LeaveCreditDTO data)
        {
            try
            {
                long yearid = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.HRMLY_FromDate <= DateTime.Today && t.HRMLY_ToDate >= DateTime.Today && t.HRMLY_ActiveFlag == true && t.MI_Id == data.MI_Id).Select(t => t.HRMLY_Id).FirstOrDefault();

                data.multi_leave_name = (from x in _lmContext.HR_Master_Leave_DMO
                                         from y in _lmContext.HR_Emp_Leave_StatusDMO
                                         from z in _lmContext.Emp_MobileNo
                                         where (x.HRML_Id == y.HRML_Id && x.MI_Id == data.MI_Id && y.HRMLY_Id == yearid && x.HRML_LeaveCreditFlg == true && y.HRME_Id == data.HRME_Id && z.HRME_Id == data.HRME_Id && z.HRMEMNO_DeFaultFlag == "default")
                                         select new LeaveCreditDTO
                                         {
                                             HRELS_Id = y.HRELS_Id,
                                             HRML_LeaveName = x.HRML_LeaveName,
                                             HRELS_TotalLeaves = y.HRELS_TotalLeaves,
                                             HRELS_CreditedLeaves = y.HRELS_CreditedLeaves,
                                             HRELS_TransLeaves = y.HRELS_TransLeaves,
                                             HRELS_CBLeaves = y.HRELS_CBLeaves,
                                             HRML_Id = y.HRML_Id,
                                             HRMEMNO_MobileNo = z.HRMEMNO_MobileNo
                                         }).Distinct().ToArray();

                data.leave_name = (from a in _lmContext.HR_Master_Leave_DMO
                                   from b in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                   from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                                   join d in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO on c.HRELAP_Id equals d.HRELAP_Id into FULLTABLE
                                   from rw in FULLTABLE.DefaultIfEmpty()
                                   where (a.HRML_Id == b.HRML_Id && c.HRELAP_Id == b.HRELAP_Id && a.MI_Id == data.MI_Id && c.HRME_Id == data.HRME_Id)
                                   select new LeaveCreditDTO
                                   {
                                       HRELAP_Id = c.HRELAP_Id,
                                       HRML_LeaveName = a.HRML_LeaveName,
                                       HRELAP_FromDate = c.HRELAP_FromDate,
                                       HRELAP_ToDate = c.HRELAP_ToDate,
                                       HRELTD_TotDays = b.HRELAPD_TotalDays,
                                       HRELAP_ApplicationStatus = b.HRELAPD_LeaveStatus,
                                       HRML_Id = b.HRML_Id,
                                       HRELAP_LeaveReason = c.HRELAP_LeaveReason,
                                       HRELAPA_Remarks = rw.HRELAPA_Remarks
                                   }).OrderByDescending(d => d.HRELAP_Id).Distinct().ToArray();

                //data.online_leave = (from a in _lmContext.HR_Master_Employee_DMO
                //                     from b in _lmContext.HR_Master_Designation_DMO
                //                     where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == qyery1.FirstOrDefault().HRME_Id)
                //                     select new LeaveCreditDTO
                //                     {
                //                         HRME_Id = a.HRME_Id,
                //                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                //                         HRME_DOJ = a.HRME_DOJ,
                //                         HRME_MobileNo = a.HRME_MobileNo,
                //                         HRME_EmailId = a.HRME_EmailId,
                //                         HRMDES_DesignationName = b.HRMDES_DesignationName
                //                     }
                //         ).Distinct().ToArray();
                //}

                return data;
            }
            catch (Exception ex)
            {

            }
            return data;
        }
        public LeaveCreditDTO getemployeeadmin(LeaveCreditDTO data)
        {
            try
            {
                data.online_leave = (from a in _lmContext.HR_Master_Employee_DMO
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new LeaveCreditDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                         HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName,
                                         HRME_EmployeeLastName = a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName,
                                         HRME_EmployeeCode = a.HRME_EmployeeCode
                                         //((a.HRME_EmployeeFirstName + " " + a.HRME_EmployeeMiddleName).Trim() + " " + a.HRME_EmployeeLastName).Trim()
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {

            }
            return data;
        }

        public LeaveCreditDTO CheckAppliedDate(LeaveCreditDTO data)
        {
            try
            {




                var HRMEID = _db.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();


                var LearId = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.MI_Id == data.MI_Id && data.HRELAP_FromDate >= t.HRMLY_FromDate && data.HRELAP_FromDate <= t.HRMLY_ToDate && t.HRMLY_ActiveFlag == true).Select(t => t.HRMLY_Id).FirstOrDefault();
                data.getemployeeleavedetails = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRME_Id == HRMEID && t.HRML_Id == data.HRML_Id && t.HRMLY_Id == LearId).ToArray();

                var leavename = _lmContext.HR_Master_Leave_DMO.Where(e => e.MI_Id == data.MI_Id && e.HRML_Id == data.HRML_Id && e.HRML_LeaveCode == "EL" || e.HRML_LeaveCode == "COMPOFF" || e.HRML_LeaveCode == "OD").ToList();


                //if (leavename.Count > 0)
                //{
                //    var getdetails = 0;
                //}
                //else
                //{

                data.HRELTD_TotDays = (from a in _lmContext.HR_Emp_Leave_Trans_DMO
                                       from b in _lmContext.HR_Emp_Leave_Trans_Details_DMO
                                       where (a.HRELT_Id == b.HRELT_Id && a.HRME_Id == HRMEID
                                       && data.HRELAP_FromDate.Value.Month == a.HRELT_FromDate.Month && data.HRELAP_FromDate.Value.Year == a.HRELT_FromDate.Year
                                       && a.HRELT_ActiveFlag == true && a.HRELT_LeaveId == data.HRML_Id
                                       && b.HRELTD_LWPFlag == false)
                                       select b).Sum(t => t.HRELTD_TotDays);

                var getdetails = (from a in _lmContext.HR_Emp_Leave_Trans_DMO
                                  from b in _lmContext.HR_Emp_Leave_Trans_Details_DMO
                                  where (a.HRELT_Id == b.HRELT_Id && a.HRME_Id == HRMEID
                                  && data.HRELAP_FromDate.Value.Date >= a.HRELT_FromDate.Date && data.HRELAP_FromDate.Value.Date <= a.HRELT_ToDate.Date && a.HRELT_ActiveFlag == true
                                  && b.HRELTD_LWPFlag == false)
                                  select b).Distinct().ToList();

                if (getdetails[0].HRELTD_TotDays >= 1)
                {
                    data.message = "Applied";
                }

                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LeaveCreditDTO CheckApprovalAuthorizationMapped(LeaveCreditDTO data)
        {
            try
            {

                var HRME_Id = _db.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Leave_Authorization_Mapping_Check";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRML_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRML_Id
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
                        data.get_leaveDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                if (data.get_leaveDetails.Length > 0)
                {
                    data.authorizationmessage = "Mapped";
                }
                else
                {
                    data.authorizationmessage = "NotMapped";
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Department_Employee";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = HRME_Id
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
                        data.Staff_List = retObject.ToArray();
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

        public string sendmail(long MI_Id, string Email, string Template, long UserID, DateTime? LeaveDate, DateTime? LeaveDate2, string LeaveReason, string ccmails, string leavename, string employeecode)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
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

                //TEMPLATE EDITING
                Mailmsg = Templatemaking(UserID, Mailmsg, LeaveDate, LeaveDate2, LeaveReason, MI_Id, leavename, employeecode);
                //TEMPLATE EDITING

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
                    string mailcc = ccmails;
                    string mailbcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 0)
                        {
                            if (ccmail[0] != null || ccmail[0] != "")
                            {
                                mailbcc = ccmails; //ccmail[1].ToString();
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

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailcc != ccmails)
                    {
                        message.AddCc(ccmails);
                    }

                    //if (mailbcc != null && mailbcc != "")
                    //{
                    //    message.AddBcc(mailbcc);
                    //}
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
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

        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID, DateTime? LeaveDate)
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

        public String Templatemaking(long UserID, string Mailmsg, DateTime? LeaveDate, DateTime? LeaveDate2, string LeaveReason, long MI_Id, string leavename, string employeecode)
        {
            try
            {
                var Empnames = _lmContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).Select(p => new { p.HRME_EmployeeFirstName, p.HRME_EmployeeMiddleName, p.HRME_EmployeeLastName, p.HRME_DOJ }).FirstOrDefault();
                string Empname = Empnames.HRME_EmployeeFirstName + " " + (Empnames.HRME_EmployeeMiddleName + " " + Empnames.HRME_EmployeeLastName).Trim();


                var companydetails = _lmContext.Institution.Where(t => t.MI_Id == MI_Id).ToList().FirstOrDefault();


                TimeSpan tp1 = DateTime.Today.Subtract(Convert.ToDateTime(Empnames.HRME_DOJ));
                if (tp1.TotalDays > 90) { }
                else { Mailmsg = "<p> <span style='font-weight:bold'>New Leave Application by [NAME] from [DATE1] to [DATE2].</span></p><p><span style='font-weight:bold'> Probation period not completed for this person. DOJ : [DOJ]</span></p>"; }

                Mailmsg = Mailmsg.Replace("[LOGO]", companydetails.MI_Logo);
                Mailmsg = Mailmsg.Replace("[COMPANYNAME]", companydetails.MI_Name);
                Mailmsg = Mailmsg.Replace("[ADDRESS]", companydetails.MI_Address1);
                Mailmsg = Mailmsg.Replace("[NAME]", Empname);
                Mailmsg = Mailmsg.Replace("[EmpCode]", employeecode);
                Mailmsg = Mailmsg.Replace("[LEAVENAME]", leavename);
                DateTime datenow = DateTime.Now;
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                DateTime date2 = DateTime.Parse(LeaveDate2.ToString());
                DateTime date3 = DateTime.Parse(Empnames.HRME_DOJ.ToString());
                Mailmsg = Mailmsg.Replace("[DATE]", datenow.ToString("dd/MM/yyyy"));
                Mailmsg = Mailmsg.Replace("[DATE1]", date.ToString("dd/MM/yyyy"));
                Mailmsg = Mailmsg.Replace("[DATE2]", date2.ToString("dd/MM/yyyy"));
                Mailmsg = Mailmsg.Replace("[LEAVEREASON]", LeaveReason);
                Mailmsg = Mailmsg.Replace("[DOJ]", date3.ToString("dd/MM/yyyy"));
                string month1, month2, month3;
                string year1, year2, year3;
                string devi1, devi2, devi3;

                LeaveCreditDTO obj = new LeaveCreditDTO();
                int currmonth = DateTime.Now.Month;
                if (currmonth >= 4)
                {
                    for (int cnt = 3; cnt > 0; cnt--)
                    {
                        int yearid = DateTime.Now.Year;
                        string firstdate = DateTime.Now.Year + "-" + (currmonth - cnt) + "-01";
                        string lday = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == (currmonth - cnt)).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault().ToString();
                        string lastdate = DateTime.Now.Year + "-" + (currmonth - cnt) + "-" + lday;
                        List<DEVDTO> dsadf = new List<DEVDTO>();
                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "VMS_MonthlyDeviation";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                            {
                                Value = UserID
                            });
                            cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                            {
                                Value = firstdate
                            });
                            cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                            {
                                Value = lastdate
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
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        string month = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == (currmonth - cnt)).Select(t => t.IVRM_Month_Name).FirstOrDefault().ToString();
                        string dept = dsadf[0].HRMDC_Name;
                        Mailmsg = Mailmsg.Replace("[DEP]", dept.ToString());
                        if (cnt == 3)
                        {
                            month1 = month.ToString(); year1 = Convert.ToString(yearid); devi1 = dsadf[0].DevPer;
                            Mailmsg = Mailmsg.Replace("[MON-YR1]", month1 + "-" + year1);
                            Mailmsg = Mailmsg.Replace("[DEVI1]", devi1);
                            Mailmsg = Mailmsg.Replace("[COMP1]", (100 - Convert.ToDecimal(devi1)).ToString());
                        }
                        else if (cnt == 2)
                        {
                            month2 = month.ToString(); year2 = Convert.ToString(yearid); devi2 = dsadf[0].DevPer;
                            Mailmsg = Mailmsg.Replace("[MON-YR2]", month2 + "-" + year2);
                            Mailmsg = Mailmsg.Replace("[DEVI2]", devi2);
                            Mailmsg = Mailmsg.Replace("[COMP2]", (100 - Convert.ToDecimal(devi2)).ToString());
                        }
                        else if (cnt == 1)
                        {
                            month3 = month.ToString(); year3 = Convert.ToString(yearid); devi3 = dsadf[0].DevPer;
                            Mailmsg = Mailmsg.Replace("[MON-YR3]", month3 + "-" + year3);
                            Mailmsg = Mailmsg.Replace("[DEVI3]", devi3);
                            Mailmsg = Mailmsg.Replace("[COMP3]", (100 - Convert.ToDecimal(devi3)).ToString());
                        }
                    }
                }
                else
                {
                    for (int cnt = 3; cnt > 0; cnt--)
                    {
                        int monthid = 0;
                        int yearid = 0;
                        if ((currmonth - cnt) == 0) { monthid = 12; yearid = DateTime.Now.Year - 1; }
                        else if ((currmonth - cnt) == -1) { monthid = 11; yearid = DateTime.Now.Year - 1; }
                        else if ((currmonth - cnt) == -2) { monthid = 10; yearid = DateTime.Now.Year - 1; }
                        else { monthid = currmonth; yearid = DateTime.Now.Year; }

                        string firstdate = yearid + "-" + monthid + "-01";
                        string lday = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == monthid).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault().ToString();
                        string lastdate = yearid + "-" + monthid + "-" + lday;

                        List<DEVDTO> dsadf = new List<DEVDTO>();

                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "VMS_MonthlyDeviation";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                            {
                                Value = UserID
                            });
                            cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                            {
                                Value = firstdate
                            });
                            cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                            {
                                Value = lastdate
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
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        string month = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == monthid).Select(t => t.IVRM_Month_Name).FirstOrDefault().ToString();
                        string dept = dsadf[0].HRMDC_Name;
                        Mailmsg = Mailmsg.Replace("[DEP]", dept.ToString());
                        if (cnt == 3)
                        {
                            month1 = month.ToString(); year1 = Convert.ToString(yearid); devi1 = dsadf[0].DevPer;
                            Mailmsg = Mailmsg.Replace("[MON-YR1]", month1 + "-" + year1);
                            Mailmsg = Mailmsg.Replace("[DEVI1]", devi1);
                            Mailmsg = Mailmsg.Replace("[COMP1]", (100 - Convert.ToDecimal(devi1)).ToString());
                        }
                        else if (cnt == 2)
                        {
                            month2 = month.ToString(); year2 = Convert.ToString(yearid); devi2 = dsadf[0].DevPer;
                            Mailmsg = Mailmsg.Replace("[MON-YR2]", month2 + "-" + year2);
                            Mailmsg = Mailmsg.Replace("[DEVI2]", devi2);
                            Mailmsg = Mailmsg.Replace("[COMP2]", (100 - Convert.ToDecimal(devi2)).ToString());
                        }
                        else if (cnt == 1)
                        {
                            month3 = month.ToString(); year3 = Convert.ToString(yearid); devi3 = dsadf[0].DevPer;
                            Mailmsg = Mailmsg.Replace("[MON-YR3]", month3 + "-" + year3);
                            Mailmsg = Mailmsg.Replace("[DEVI3]", devi3);
                            Mailmsg = Mailmsg.Replace("[COMP3]", (100 - Convert.ToDecimal(devi3)).ToString());
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Mailmsg;
        }

        public string callnotification(string devicenew, long HRELAP_Id, long mi_id, LeaveCreditDTO dto, string header_flg)
        {
            try
            {
                var key = _lmContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;

                LeaveCreditDTO data = new LeaveCreditDTO();
                var leavedetails = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(h => h.MI_Id == mi_id && h.HRELAP_ActiveFlag == true && h.HRELAP_Id == HRELAP_Id).Distinct().ToList();

                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                string sound = "default";
                long notId = 4;
                string empname = "Leave Details";

                string devicenew1 = '"' + devicenew + '"';
                devicenew1 = "[" + devicenew1 + "]";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew1 + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Leave Reason" + '"' + " } }";

                notificationparams.Add(daata.ToString());
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.Method = "post";
                connection.Headers.Add(string.Format("Authorization: key={0}", "AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS"));
                connection.Headers.Add(string.Format("Sender: id={0}", "fDOUfTEYDfU:APA91bHLkVeYnCRvxh4oXYLoEf-VmJOx0u3TEg6mr8V7oDDlCbCcXGoNA02xr3TMji3VaYRlTPObS7uOh4487KkWi7ldtYu1_Uxr1xvwkVKDyZzQw1wv4g4KOdADD392ook426aUttTj"));
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                var payload = new
                {
                    to = "fDOUfTEYDfU:APA91bHLkVeYnCRvxh4oXYLoEf-VmJOx0u3TEg6mr8V7oDDlCbCcXGoNA02xr3TMji3VaYRlTPObS7uOh4487KkWi7ldtYu1_Uxr1xvwkVKDyZzQw1wv4g4KOdADD392ook426aUttTj",
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = "Test",
                        title = "Test",
                        badge = 1
                    },
                    data = new
                    {
                        key1 = "value1",
                        key2 = "value2"
                    }
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                connection.ContentLength = byteArray.Length;
                using (Stream dataStream = connection.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = connection.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
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

        public async Task<LeaveCreditDTO> getempleavehistory(LeaveCreditDTO data)
        {
            try
            {
                long leaveid = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true && t.HRMLY_FromDate >= DateTime.Today && t.HRMLY_ToDate <= DateTime.Today).Select(t => t.HRMLY_Id).Single();

                data.leavehistory = (from a in _lmContext.HR_Emp_Leave_ApplicationDMO
                                     from b in _lmContext.HR_Emp_Leave_Trans_DMO
                                     from c in _lmContext.HR_Master_Leave_DMO
                                     where (a.MI_Id == data.MI_Id && a.HRELAP_ActiveFlag == true && a.HRME_Id == b.HRME_Id && a.HRELAP_FromDate == b.HRELT_FromDate && a.HRELAP_ToDate == b.HRELT_ToDate && a.MI_Id == c.MI_Id && a.HRME_Id == data.HRME_Id && b.HRELT_LeaveId == c.HRML_Id && b.HRMLY_Id == leaveid)
                                     select new LeaveCreditDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRELAP_ApplicationID = a.HRELAP_ApplicationID,
                                         HRELAP_FromDate = a.HRELAP_FromDate,
                                         HRELAP_ToDate = a.HRELAP_ToDate,
                                         HRELAP_TotalDays = a.HRELAP_TotalDays,
                                         HRML_LeaveName = c.HRML_LeaveName,
                                         HRELAP_SupportingDocument = a.HRELAP_SupportingDocument
                                     }).ToArray();

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Weekly_DeviationReport_Calculation_Report_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)
                    {
                        Value = data.HRELAP_FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)
                    {
                        Value = data.HRELAP_ToDate
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
            catch (Exception ex)
            {

            }
            return data;
        }

        public LeaveCreditDTO requestleave(LeaveCreditDTO data)
        {
            try
            {
                data.returnmsg = "";
                data.returnval = false;
                HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(data);
                if (data.HRML_LeaveType == "COMPOFF")
                {
                    data.HRML_Id = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveType == "Comp_Off" && t.MI_Id == data.MI_Id).Select(t => t.HRML_Id).FirstOrDefault();
                    for (var i = 0; i < data.selectedcompoff.Length; i++)
                    {
                        _lmContext.Database.ExecuteSqlCommand("HR_Compoff_ReApproval @p0,@p1", data.selectedcompoff[i].HRME_Id, data.selectedcompoff[i].HRELAP_Id);
                    }
                    for (var j = 0; j < data.selecteddr.Length; j++)
                    {
                        _lmContext.Database.ExecuteSqlCommand("HR_Compoff_ReApproval_DR_FlagUpdate @p0,@p1", data.selecteddr[j].ISMDRPT_Id, data.UserId);//, data.selectedcompoff[i].HRELAP_Remarks ,@p3
                    }
                    data.returnval = true;
                    return data;
                }
                else if (data.HRML_LeaveType == "OD")
                {
                    data.HRML_Id = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveCode == "OD" && t.MI_Id == data.MI_Id).Select(t => t.HRML_Id).FirstOrDefault();
                }

                if (data.HRML_Id > 0)
                {
                    var qyery1 = (from q in _db.Staff_User_Login
                                  from r in _db.HR_Master_Employee_DMO
                                  where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                                  select new LeaveCreditDTO
                                  {
                                      HRME_Id = r.HRME_Id,
                                  }).Distinct().ToArray();

                    var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                          from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                          where (m.HRELAP_Id == n.HRELAP_Id && n.HRML_Id == data.HRML_Id && m.MI_Id == data.MI_Id && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id && m.HRELAP_FromDate == data.HRELAP_FromDate && m.HRELAP_ToDate == data.HRELAP_ToDate && m.HRELAP_ApplicationStatus != "Rejected")
                                          select new LeaveCreditDTO
                                          {
                                              HRELAP_Id = m.HRELAP_Id
                                          }).ToList();

                    if (checkduplicate.Count == 0)
                    {
                        //Leave Id Creation from transaction numbering.
                        var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == data.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                        if (transnumconfigsettings.Count > 0)
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            Master_NumberingDTO num = new Master_NumberingDTO();
                            num.MI_Id = data.MI_Id;
                            num.ASMAY_Id = data.asmay_id;
                            num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                            num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                            num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                            num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                            num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                            num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                            num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                            num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                            num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                            num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                            num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                            num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                            num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                            num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                            num.IMN_Flag = "LeaveNo";
                            data.HRELAP_ApplicationID = a.GenerateNumber(num);
                        }


                        //LEAVE APPLICATION
                        objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.HRELAP_CreatedBy = data.UserId;
                        objpge.HRELAP_UpdatedBy = data.UserId;
                        objpge.HRELAP_ActiveFlag = true;
                        objpge.HRELAP_ApplicationID = data.HRELAP_ApplicationID != null ? data.HRELAP_ApplicationID : "";
                        objpge.HRELAP_ApplicationStatus = "Requested";
                        objpge.HRELAP_SanctioningLevel = "1";
                        objpge.HRELAP_CompOffCreditApplFlg = false;
                        objpge.HRELAP_FromDate = Convert.ToDateTime(data.HRELAP_FromDate);
                        objpge.HRELAP_ToDate = Convert.ToDateTime(data.HRELAP_ToDate);
                        objpge.HRELAP_SupportingDocument = data.HRELT_SupportingDocument;
                        _lmContext.Add(objpge);
                        _lmContext.SaveChanges();

                        //LEAVE APPLICATION DETAILS
                        if (objpge.HRELAP_Id > 0)
                        {
                            HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                            updatedet.HRELAPD_FromDate = data.HRELAP_FromDate;
                            updatedet.HRELAPD_LeaveStatus = "Applied";
                            updatedet.HRELAPD_ToDate = data.HRELAP_ToDate;
                            updatedet.HRELAPD_TotalDays = data.HRELAP_TotalDays;
                            updatedet.HRELAP_Id = objpge.HRELAP_Id;
                            updatedet.HRML_Id = data.HRML_Id;
                            updatedet.UpdatedDate = DateTime.Now;
                            updatedet.CreatedDate = DateTime.Now;
                            updatedet.HRELAPD_UpdatedBy = data.UserId;
                            updatedet.HRELAPD_CreatedBy = data.UserId;
                            updatedet.HRELAPD_ActiveFlag = true;
                            updatedet.HRELAPD_InTime = data.HRELAPD_InTime;
                            updatedet.HRELAPD_OutTime = data.HRELAPD_OutTime;
                            _lmContext.Add(updatedet);
                            _lmContext.SaveChanges();
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnmsg = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LeaveCreditDTO viewcomment(LeaveCreditDTO data)
        {
            try
            {
                //data.commentlist = (from a in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                //                    from b in _lmContext.HR_Master_Employee_DMO
                //                    from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                //                    where (a.HRME_Id == b.HRME_Id && a.HRELAP_Id == data.HRELAP_Id && a.HRELAP_Id == c.HRELAP_Id)
                //                    select new LeaveCreditDTO
                //                    {
                //                        HRME_Id = a.HRME_Id,
                //                        HRELAPA_Remarks = a.HRELAPA_Remarks,
                //                        HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                //                        HRELAPA_TotalDays = a.HRELAPA_TotalDays,
                //                        HRELAPA_FromDate = a.HRELAPA_FromDate,
                //                        HRELAPA_ToDate = a.HRELAPA_ToDate,
                //                        HRELAPA_LeaveStatus = a.HRELAPA_LeaveStatus,
                //                        HRELAPA_SanctioningLevel = a.HRELAPA_SanctioningLevel,
                //                        HRELAP_FromDate = c.HRELAP_FromDate,
                //                        HRELAP_ToDate = c.HRELAP_ToDate,
                //                        CreatedDate = a.CreatedDate,
                //                        HRELAP_SupportingDocument = c.HRELAP_SupportingDocument
                //                    }).OrderBy(a => a.HRELAPA_SanctioningLevel).ToArray();


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Levelwise_Leave_report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRELAP_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRELAP_Id
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
                        data.commentlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public bool LeaveCredition()
        {
            var currentdate = DateTime.Now.Date;
            var sample = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_FromDate.Value.Date == currentdate && t.HRELAP_ApplicationStatus == "Approved").Select(t => t.HRME_Id).Distinct().ToList();
            var PunchDetails = (from a in _lmContext.FO_Emp_Punch where (sample.Contains(a.HRME_Id) && a.CreatedDate.Value.Date == currentdate) select a).ToList();
            foreach (var emp in PunchDetails)
            {
                var FirstPunchIn = _lmContext.FO_Emp_Punch_Details.Where(t => t.FOEP_Id == emp.FOEP_Id && t.FOEPD_InOutFlg == "I").Select(t => t.FOEPD_PunchTime).FirstOrDefault().ToString();

                var LastPunchOut = _lmContext.FO_Emp_Punch_Details.Where(t => t.FOEP_Id == emp.FOEP_Id && t.FOEPD_InOutFlg == "O").Select(t => t.FOEPD_PunchTime).LastOrDefault().ToString();

                var FirstPunchInTime = TimeSpan.Parse(FirstPunchIn);
                var LastPunchOutTime = TimeSpan.Parse(LastPunchOut);
                var Diffrence = LastPunchOutTime - FirstPunchInTime;
                int TotalWorkingHours = int.Parse(Diffrence.ToString());

                if (TotalWorkingHours >= 8)
                {

                }
                else if (TotalWorkingHours >= 4 && TotalWorkingHours <= 8)
                {

                }
                else
                {

                }


            }

            return true;

        }


        public LeaveCreditDTO Leaveapprover(LeaveCreditDTO data)
        {
            try
            {

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EmployeeWiseLeaveApprover";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
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
                        data.approverList = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public LeaveCreditDTO proxytaskdetails(LeaveCreditDTO dto)
        {
            try
            {
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssignedTask_DetailsForProxy";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TransferredBy",
                      SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TransferredTo",
              SqlDbType.BigInt)
                    {
                        Value = dto.HRMEIdproxy
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                                  SqlDbType.VarChar)
                    {
                        Value = dto.HRELAP_FromDate.ToString()
                    });

                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                      SqlDbType.VarChar)
                    {
                        Value = dto.HRELAP_ToDate.ToString()
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
                        dto.proxytaskList = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssignedTaskDetailsForProxyView";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRELAP_Id",
              SqlDbType.BigInt)
                    {
                        Value = dto.HRELAP_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                                  SqlDbType.VarChar)
                    {
                        Value = dto.HRELAP_FromDate.ToString()
                    });

                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                      SqlDbType.VarChar)
                    {
                        Value = dto.HRELAP_ToDate.ToString()
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
                        dto.proxytaskListview = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dto;
        }



        ///****************** Leave Restriction**********************////
        public LeaveCreditDTO LRdetails(LeaveCreditDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto.institutionarray = _lmContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();

                var lrdatalist = (from sal in _lmContext.HR_Leave_Restriction_DatesDMO
                                  from a in _lmContext.HR_Master_Department_DMO
                                  where sal.HRMD_Id == a.HRMD_Id
                                  select new LeaveCreditDTO
                                  {
                                      HRLRD_Id = sal.HRLRD_Id,
                                      HRMD_Id = sal.HRMD_Id,
                                      HRLRD_FromDate = sal.HRLRD_FromDate,
                                      HRLRD_ToDate = sal.HRLRD_ToDate,
                                      HRLRD_Remarks = sal.HRLRD_Remarks,
                                      HRLRD_ActiveFlg = sal.HRLRD_ActiveFlg,
                                      HRLRD_UpdatedDate = sal.HRLRD_UpdatedDate,
                                      HRLRD_CreatedDate = sal.HRLRD_CreatedDate,
                                      HRLRD_UpdatedBy = sal.HRLRD_UpdatedBy,
                                      HRLRD_CreatedBy = sal.HRLRD_CreatedBy,
                                      HRMD_DepartmentName = a.HRMD_DepartmentName
                                  }).ToList();
                dto.leaverestriclist = lrdatalist.ToArray();



                dto.employeelist = (from a in _lmContext.HR_Master_Employee_DMO
                                    where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                    select new LeaveCreditDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                    }).OrderBy(a => a.HRELAPA_SanctioningLevel).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public LeaveCreditDTO getdepartment(LeaveCreditDTO dto)
        {
            try
            {
                dto.departmentdropdown = (from a in _lmContext.HR_Master_Department_DMO
                                          from b in _lmContext.Institution
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id)
                                          select new LeaveCreditDTO
                                          {
                                              HRMD_Id = a.HRMD_Id,
                                              MI_Id = a.MI_Id,
                                              HRMD_DepartmentName = a.HRMD_DepartmentName
                                          }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public LeaveCreditDTO LRSaveUpdate(LeaveCreditDTO dto)
        {

            try
            {
                int count = 0;


                if (dto.HRLRD_Id > 0)
                {
                    var duplicatAdd = _lmContext.HR_Leave_Restriction_DatesDMO.Where(t => t.HRLRD_ToDate < dto.HRLRD_FromDate && t.HRLRD_Id != dto.HRLRD_Id).Count();
                    if (duplicatAdd > 0)
                    {
                        count = count + 1;

                    }
                    if (duplicatAdd == 0)
                    {
                        var result = _lmContext.HR_Leave_Restriction_DatesDMO.Single(t => t.HRLRD_Id == dto.HRLRD_Id);
                        foreach (var d in dto.selected_Dept)
                        {
                            result.HRLRD_FromDate = dto.HRLRD_FromDate;
                            result.HRMD_Id = d.HRMD_Id != null && d.HRMD_Id != 0 ? d.HRMD_Id : 0;
                            result.HRME_Id = d.HRME_Id != null && d.HRME_Id != 0 ? d.HRME_Id : 0;
                            result.HRLRD_ToDate = dto.HRLRD_ToDate;
                            result.HRLRD_Remarks = dto.HRLRD_Remarks;
                            result.HRLRD_ActiveFlg = true;
                            result.HRLRD_UpdatedDate = DateTime.Now;
                            result.HRLRD_CreatedDate = DateTime.Now;
                            result.HRLRD_UpdatedBy = dto.UserId;
                            result.HRLRD_CreatedBy = dto.UserId;
                            _lmContext.Update(result);
                        }
                        var flag = _lmContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var duplicatAdd = _lmContext.HR_Leave_Restriction_DatesDMO.Where(t => t.HRLRD_Id != dto.HRLRD_Id).Count();
                    if (duplicatAdd > 0)
                    {
                        count = count + 1;
                        dto.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        foreach (var d in dto.selected_Dept)
                        {
                            HR_Leave_Restriction_DatesDMO dmoObj = new HR_Leave_Restriction_DatesDMO();
                            dmoObj.HRLRD_Id = dto.HRLRD_Id;
                            dmoObj.HRMD_Id = d.HRMD_Id != null && d.HRMD_Id != 0 ? d.HRMD_Id : 0;
                            dmoObj.HRME_Id = d.HRME_Id != null && d.HRME_Id != 0 ? d.HRME_Id : 0;
                            dmoObj.HRLRD_FromDate = dto.HRLRD_FromDate;
                            dmoObj.HRLRD_ToDate = dto.HRLRD_ToDate;
                            dmoObj.HRLRD_Remarks = dto.HRLRD_Remarks;
                            dmoObj.HRLRD_ActiveFlg = true;
                            dmoObj.HRLRD_UpdatedDate = DateTime.Now;
                            dmoObj.HRLRD_CreatedDate = DateTime.Now;
                            dmoObj.HRLRD_UpdatedBy = dto.UserId;
                            dmoObj.HRLRD_CreatedBy = dto.UserId;
                            _lmContext.Add(dmoObj);
                        }
                        var flag = _lmContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                }

                dto.duplicateCount = count;
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public LeaveCreditDTO LREdit(LeaveCreditDTO dto)
        {
            try
            {
                dto.lrlist = (from a in _lmContext.HR_Leave_Restriction_DatesDMO
                              from b in _lmContext.Institution
                              from c in _lmContext.HR_Master_Department_DMO
                              where (b.MI_Id == c.MI_Id && a.HRLRD_Id == dto.HRLRD_Id && a.HRMD_Id == c.HRMD_Id)
                              select new LeaveCreditDTO
                              {
                                  HRLRD_Id = a.HRLRD_Id,
                                  HRMD_Id = a.HRMD_Id,
                                  MI_Id = c.MI_Id,
                                  HRLRD_FromDate = a.HRLRD_FromDate,
                                  HRLRD_ToDate = a.HRLRD_ToDate,
                                  HRLRD_Remarks = a.HRLRD_Remarks,
                                  HRLRD_ActiveFlg = a.HRLRD_ActiveFlg,
                                  HRMD_DepartmentName = c.HRMD_DepartmentName
                              }).Distinct().ToArray();
                dto.departmentdropdown = (from a in _lmContext.HR_Master_Department_DMO
                                          from b in _lmContext.Institution
                                          where (a.MI_Id == b.MI_Id && a.HRMD_Id == dto.HRMD_Id)
                                          select new LeaveCreditDTO
                                          {
                                              HRMD_Id = a.HRMD_Id,
                                              MI_Id = a.MI_Id,
                                              HRMD_DepartmentName = a.HRMD_DepartmentName
                                          }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public LeaveCreditDTO ActiveDeactiveRecord(LeaveCreditDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRLRD_Id > 0)
                {
                    var result = _lmContext.HR_Leave_Restriction_DatesDMO.Single(t => t.HRLRD_Id == dto.HRLRD_Id);

                    if (result.HRLRD_ActiveFlg == true)
                    {
                        result.HRLRD_ActiveFlg = false;
                    }
                    else if (result.HRLRD_ActiveFlg == false)
                    {
                        result.HRLRD_ActiveFlg = true;
                    }
                    result.HRLRD_UpdatedDate = DateTime.Now;
                    result.HRLRD_UpdatedBy = dto.UserId;
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRLRD_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public LeaveCreditDTO leveConditions(LeaveCreditDTO data)
        {
            try
            {
                long hrme_id = _lmContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Select(t => t.Emp_Code).Single();

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employees_Leave_Appl_Conditions";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = hrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                     SqlDbType.Date)
                    {
                        Value = data.HRELAP_FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.Date)
                    {
                        Value = data.HRELAP_ToDate
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
                        data.commentlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}

