using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.TADA;
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

namespace IssueManager.com.TADA.Services
{
    public class TADA_AdavanceApply_PaymentIMPL : Interface.TADA_AdavanceApply_PaymentInterface
    {


        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;

        public TADA_AdavanceApply_PaymentIMPL(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }
        public TADA_AdavanceApply_PaymentDTO getalldata(TADA_AdavanceApply_PaymentDTO data)
        {
            try
            {


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_AdavancePayments_Account";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                 SqlDbType.BigInt)
                    {
                        Value = "1"
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
                        data.Adavance = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_AdavancePayments_Account";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                 SqlDbType.BigInt)
                    {
                        Value = "2"
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
                        data.GetReport = retObject.ToArray();
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
        //getOnload
        public TADA_AdavanceApply_PaymentDTO getOnload(TADA_AdavanceApply_PaymentDTO data)
        {
            try
            {
                //var Adavance_Detail = _context.TADA_AdavanceApply_PaymentDMO.ToList().Select(p => p.VTADAAA_Id);
                //data.Adavance = (from p in _context.TADA_AdavanceApply_PaymentDMO
                //                 from Q in _context.TADA_AdavanceApplyDMO
                //                 from t in _context.MasterEmployee
                //                 from u in _context.IVRM_Master_CityDMO
                //                 where (Q.HRME_Id == t.HRME_Id && p.VTADAAA_Id == Q.VTADAAA_Id && Q.IVRMMCT_Id == u.IVRMMCT_Id && Q.MI_Id == data.MI_Id
                //                 )
                //                 select new TADA_AdavanceApply_PaymentDTO
                //                 {
                //                     HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                //                     VTADAAAP_AmountPaid = p.VTADAAAP_AmountPaid,
                //                     VTADAAAP_Date = p.VTADAAAP_Date,
                //                     HRME_EmployeeCode = t.HRME_EmployeeCode,
                //                     VTADAAAP_PaymentMode = p.VTADAAAP_PaymentMode,
                //                     VTADAAAP_Id = p.VTADAAAP_Id,
                //                     IVRMMCT_Name = u.IVRMMCT_Name,
                //                     VTADAAA_Id = p.VTADAAA_Id,
                //                     VTADAAAP_ActiveFlg = p.VTADAAAP_ActiveFlg,
                //                     VTADAAAP_Remarks = p.VTADAAAP_Remarks,
                //                     VTADAAAP_CreatedDate = p.VTADAAAP_CreatedDate
                //                 }).Distinct().OrderByDescending(R => R.VTADAAAP_CreatedDate).ToArray();
                //data.GetReport = (from a in _context.TADA_AdavanceApplyDMO
                //                  from c in _context.IVRM_Master_CityDMO
                //                  from t in _context.MasterEmployee
                //                  where (a.VTADAAA_ActiveFlg == true && a.IVRMMCT_Id == c.IVRMMCT_Id && a.HRME_Id == t.HRME_Id && !Adavance_Detail.Contains(a.VTADAAA_Id) && a.MI_Id == data.MI_Id
                //                  && t.HRME_ActiveFlag == true && a.VTADAAA_StatusFlg == "Approved")
                //                  select new TADA_Advance_ApplyDTO
                //                  {
                //                      VTADAAA_ClientId = a.VTADAAA_ClientId,
                //                      VTADAAA_FromDate = a.VTADAAA_FromDate,
                //                      VTADAAA_ToDate = a.VTADAAA_ToDate,
                //                      VTADAAA_ToAddress = a.VTADAAA_ToAddress,
                //                      VTADAAA_Remarks = a.VTADAAA_Remarks,
                //                      VTADAAA_TotalAppliedAmount = a.VTADAAA_TotalAppliedAmount,
                //                      VTADAAA_TotalSactionedAmount = a.VTADAAA_TotalSactionedAmount,
                //                      HRME_EmployeeCode = t.HRME_EmployeeCode,
                //                      VTADAAA_Id = a.VTADAAA_Id,
                //                      IVRMMCT_Id = a.IVRMMCT_Id,
                //                      IVRMMCT_Name = c.IVRMMCT_Name,
                //                      VTADAAA_StatusFlg = a.VTADAAA_StatusFlg,
                //                      HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                //                      VTADAAA_CreatedDate = a.VTADAAA_CreatedDate
                //                  }
                //                  ).Distinct().OrderByDescending(p => p.VTADAAA_CreatedDate).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_RoutePayments_Account";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                 SqlDbType.BigInt)
                    {
                        Value = "1"
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
                        data.Adavance = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_RoutePayments_Account";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                 SqlDbType.BigInt)
                    {
                        Value = "2"
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
                        data.GetReport = retObject.ToArray();
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
        public TADA_AdavanceApply_PaymentDTO edittt(TADA_AdavanceApply_PaymentDTO data)
        {
            try
            {

                var result = _context.TADA_AdavanceApply_PaymentDMO.Where(p => p.VTADAAAP_Id == data.VTADAAAP_Id).FirstOrDefault();
                if (result.VTADAAAP_ActiveFlg == true)
                {
                    data.EditArray = (from a in _context.TADA_AdavanceApplyDMO
                                      from c in _context.IVRM_Master_CityDMO
                                      from t in _context.MasterEmployee
                                      where (a.VTADAAA_ActiveFlg == true && a.IVRMMCT_Id == c.IVRMMCT_Id && a.HRME_Id == t.HRME_Id
                                      && t.HRME_ActiveFlag == true && a.VTADAAA_StatusFlg == "Approved" && a.VTADAAA_Id == data.VTADAAA_Id)
                                      select new TADA_Advance_ApplyDTO
                                      {
                                          VTADAAA_ClientId = a.VTADAAA_ClientId,
                                          VTADAAA_FromDate = a.VTADAAA_FromDate,
                                          VTADAAA_ToDate = a.VTADAAA_ToDate,
                                          VTADAAA_TotalAppliedAmount = a.VTADAAA_TotalAppliedAmount,
                                          VTADAAA_TotalSactionedAmount = a.VTADAAA_TotalSactionedAmount,
                                          IVRMMCT_Name = c.IVRMMCT_Name,
                                          VTADAAA_StatusFlg = a.VTADAAA_StatusFlg,
                                          HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                          VTADAAA_CreatedDate = a.VTADAAA_CreatedDate

                                      }
                                  ).Distinct().OrderByDescending(p => p.VTADAAA_CreatedDate).ToArray();
                    data.Edit_Two = (from p in _context.TADA_AdavanceApply_PaymentDMO
                                     from Q in _context.TADA_AdavanceApplyDMO
                                     where (p.VTADAAA_Id == Q.VTADAAA_Id && p.VTADAAAP_Id == data.VTADAAAP_Id)
                                     select new TADA_AdavanceApply_PaymentDTO
                                     {
                                         VTADAAAP_AmountPaid = p.VTADAAAP_AmountPaid,
                                         VTADAAAP_Date = p.VTADAAAP_Date,
                                         VTADAAAP_PaymentMode = p.VTADAAAP_PaymentMode,
                                         VTADAAAP_Id = p.VTADAAAP_Id,
                                         VTADAAAP_PaymentRefNo = p.VTADAAAP_PaymentRefNo,
                                         VTADAAAP_ActiveFlg = p.VTADAAAP_ActiveFlg,
                                         VTADAAAP_Remarks = p.VTADAAAP_Remarks
                                     }).Distinct().ToArray();
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADA_ADavanceMltiple_SDetailsAmt_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                          SqlDbType.VarChar)
                        {
                            Value = result.VTADAAA_Id
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

                            data.Veiwdetails = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //data.VTADAA_Id = 1;
        public TADA_AdavanceApply_PaymentDTO savedta(TADA_AdavanceApply_PaymentDTO data)
        {
            try
            {
                data.returnvalue = false;
                var result = _context.TADA_AdavanceApply_PaymentDMO.Where(p => p.VTADAAA_Id == data.VTADAAA_Id && p.VTADAAAP_ActiveFlg == true).ToList();
                var resultone = _context.TADA_AdavanceApplyDMO.Where(t => t.VTADAAA_Id == data.VTADAAA_Id && t.VTADAAA_ActiveFlg == true).FirstOrDefault();
                if (data.VTADAAA_Id > 0 && data.VTADAAAP_Id == 0)
                {
                    // if (result.Count == 0)
                    //  {
                    TADA_AdavanceApply_PaymentDMO obj = new TADA_AdavanceApply_PaymentDMO();
                    obj.VTADAAAP_Id = data.VTADAAAP_Id;
                    obj.VTADAAA_Id = data.VTADAAA_Id;
                    obj.VTADAAAP_PaymentMode = data.VTADAAAP_PaymentMode;
                    obj.VTADAAAP_AmountPaid = data.VTADAAAP_AmountPaid;
                    obj.VTADAAAP_PaymentRefNo = data.VTADAAAP_PaymentRefNo;
                    obj.VTADAAAP_Remarks = data.VTADAAAP_Remarks;
                    obj.VTADAAAP_Date = data.VTADAAAP_Date;
                    obj.VTADAAAP_ActiveFlg = true;
                    obj.VTADAAAP_CreatedBy = data.UserId;
                    obj.VTADAAAP_CreatedDate = DateTime.Now;
                    obj.VTADAAAP_UpdatedBy = data.UserId;
                    obj.VTADAAAP_UpdatedDate = DateTime.Now;
                    _context.Add(obj);
                    if (resultone.VTADAAA_Id > 0)
                    {
                        if (resultone.VTADAAA_TotalPaidAmount > 0)
                        {
                            resultone.VTADAAA_TotalPaidAmount = data.VTADAAAP_AmountPaid + resultone.VTADAAA_TotalPaidAmount;
                        }
                        else
                        {
                            resultone.VTADAAA_TotalPaidAmount = data.VTADAAAP_AmountPaid;
                        }
                        if (data.VTADAAAP_PaymentMode == "Rejected")
                        {
                            resultone.VTADAAA_StatusFlg = "Rejected";
                        }
                        _context.Update(resultone);
                    }
                    int a = _context.SaveChanges();
                    if (a > 0)
                    {

                        //HRME_EmployeeCode
                        //usp_i_Route_TADA_Wallet

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "TADA_AccountUpdate";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                              SqlDbType.VarChar)
                            {
                                Value = data.VTADAAA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@VTADASLEAP_Id",
                              SqlDbType.VarChar)
                            {
                                Value = data.VTADASLEAP_Id
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




                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "usp_i_Route_TADA_Wallet";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@RTW_EmpID",
                              SqlDbType.BigInt)
                            {
                                Value = resultone.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@RTW_Date",
                              SqlDbType.DateTime)
                            {
                                Value = DateTime.Now
                            });
                            cmd.Parameters.Add(new SqlParameter("@RTW_IERId",
                            SqlDbType.BigInt)
                            {
                                Value = resultone.IER_ID
                            });
                            cmd.Parameters.Add(new SqlParameter("@RTW_Wallet",
                          SqlDbType.Decimal)
                            {
                                Value = data.VTADAAAP_AmountPaid
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




                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        data.returnvalue = true;
                        data.returnval = "Insert";
                        string mailcc = "";
                        data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                        string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        var chkk = (from P in _context.TADA_AdavanceApply_PaymentDMO
                                    from R in _context.TADA_AdavanceApplyDMO
                                    where (R.VTADAAA_Id == P.VTADAAA_Id && R.VTADAAA_ActiveFlg == true && P.VTADAAA_Id == obj.VTADAAA_Id)
                                    select R
                                  ).ToList();
                        if (chkk.Count > 0)
                        {

                            data.UserId = chkk.FirstOrDefault().HRME_Id;

                            mailcc = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.UserId && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        }
                        sendmail(data.MI_Id, emailid, "TADA_AdvancePayment", obj.VTADAAAP_Id, mailcc);
                    }
                    else
                    {
                        data.returnvalue = true;
                        data.returnval = "Failed";
                    }
                    //   }
                    //else
                    //{
                    //    data.returnvalue = true;
                    //    data.returnval = "Duplicate";
                    //}
                }
                else
                {
                    if (data.VTADAAA_Id > 0 && data.VTADAAAP_Id > 0)
                    {
                        var resultupdate = _context.TADA_AdavanceApply_PaymentDMO.Where(t => t.VTADAAAP_Id == data.VTADAAAP_Id).FirstOrDefault();
                        if (resultupdate.VTADAAAP_ActiveFlg == true)
                        {
                            resultupdate.VTADAAAP_PaymentMode = data.VTADAAAP_PaymentMode;
                            resultupdate.VTADAAAP_AmountPaid = data.VTADAAAP_AmountPaid;
                            resultupdate.VTADAAAP_PaymentRefNo = data.VTADAAAP_PaymentRefNo;
                            resultupdate.VTADAAAP_Remarks = data.VTADAAAP_Remarks;
                            resultupdate.VTADAAAP_Date = data.VTADAAAP_Date;
                            resultupdate.VTADAAAP_UpdatedBy = data.UserId;
                            resultupdate.VTADAAAP_UpdatedDate = DateTime.Now;
                            _context.Update(resultupdate);
                        }
                        if (resultone.VTADAAA_Id > 0)
                        {
                            resultone.VTADAAA_TotalPaidAmount = data.VTADAAAP_AmountPaid;
                            _context.Update(resultone);
                        }
                        int a = _context.SaveChanges();
                        if (a > 0)
                        {
                            data.returnvalue = true;
                            data.returnval = "Update";

                        }
                        else
                        {
                            data.returnvalue = true;
                            data.returnval = "UpdateFailed";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //savePayment
        public TADA_AdavanceApply_PaymentDTO savePayment(TADA_AdavanceApply_PaymentDTO data)
        {
            try
            {
                data.returnvalue = false;
                var result = _context.TADA_AdavanceApply_PaymentDMO.Where(p => p.VTADAAA_Id == data.VTADAAA_Id && p.VTADAAAP_ActiveFlg == true).ToList();
                var resultone = _context.TADA_AdavanceApplyDMO.Where(t => t.VTADAAA_Id == data.VTADAAA_Id && t.VTADAAA_ActiveFlg == true).FirstOrDefault();
                if (data.VTADAAA_Id > 0 && data.VTADAAAP_Id == 0)
                {
                    //if (result.Count == 0)
                    //{
                    TADA_AdavanceApply_PaymentDMO obj = new TADA_AdavanceApply_PaymentDMO();
                    obj.VTADAAAP_Id = data.VTADAAAP_Id;
                    obj.VTADAAA_Id = data.VTADAAA_Id;
                    obj.VTADAAAP_PaymentMode = data.VTADAAAP_PaymentMode;
                    obj.VTADAAAP_AmountPaid = data.VTADAAAP_AmountPaid;
                    obj.VTADAAAP_PaymentRefNo = data.VTADAAAP_PaymentRefNo;
                    obj.VTADAAAP_Remarks = data.VTADAAAP_Remarks;
                    obj.VTADAAAP_Date = data.VTADAAAP_Date;
                    obj.VTADAAAP_ActiveFlg = true;
                    obj.VTADAAAP_CreatedBy = data.UserId;
                    obj.VTADAAAP_CreatedDate = DateTime.Now;
                    obj.VTADAAAP_UpdatedBy = data.UserId;
                    obj.VTADAAAP_UpdatedDate = DateTime.Now;
                    _context.Add(obj);
                    if (resultone.VTADAAA_Id > 0)
                    {
                        decimal? VTADAAAP_AmountPaid = 0;
                        if (resultone.VTADAAA_TotalPaidAmount > 0)
                        {
                            VTADAAAP_AmountPaid = resultone.VTADAAA_TotalPaidAmount;
                        }
                        resultone.VTADAAA_TotalPaidAmount = data.VTADAAAP_AmountPaid + VTADAAAP_AmountPaid.Value;
                        if (data.VTADAAAP_PaymentMode == "Rejected")
                        {
                            resultone.VTADAAA_StatusFlg = "Rejected";
                        }
                        _context.Update(resultone);
                    }
                    int a = _context.SaveChanges();
                    if (a > 0)
                    {
                        data.returnvalue = true;
                        data.returnval = "Insert";
                        string mailcc = "";
                        data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                        string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        var chkk = (from P in _context.TADA_AdavanceApply_PaymentDMO
                                    from R in _context.TADA_AdavanceApplyDMO
                                    where (R.VTADAAA_Id == P.VTADAAA_Id && R.VTADAAA_ActiveFlg == true && P.VTADAAA_Id == obj.VTADAAA_Id)
                                    select R
                                  ).ToList();
                        if (chkk.Count > 0)
                        {

                            data.UserId = chkk.FirstOrDefault().HRME_Id;

                            mailcc = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.UserId && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        }
                        sendmail(data.MI_Id, emailid, "TADA_AdvancePayment", obj.VTADAAAP_Id, mailcc);
                    }
                    else
                    {
                        data.returnvalue = true;
                        data.returnval = "Failed";
                    }
                    //}
                    //else
                    //{
                    //    data.returnvalue = true;
                    //    data.returnval = "Duplicate";
                    //}
                }
                else
                {
                    if (data.VTADAAA_Id > 0 && data.VTADAAAP_Id > 0)
                    {
                        var resultupdate = _context.TADA_AdavanceApply_PaymentDMO.Where(t => t.VTADAAAP_Id == data.VTADAAAP_Id).FirstOrDefault();
                        if (resultupdate.VTADAAAP_ActiveFlg == true)
                        {
                            resultupdate.VTADAAAP_PaymentMode = data.VTADAAAP_PaymentMode;
                            resultupdate.VTADAAAP_AmountPaid = data.VTADAAAP_AmountPaid;
                            resultupdate.VTADAAAP_PaymentRefNo = data.VTADAAAP_PaymentRefNo;
                            resultupdate.VTADAAAP_Remarks = data.VTADAAAP_Remarks;
                            resultupdate.VTADAAAP_Date = data.VTADAAAP_Date;
                            resultupdate.VTADAAAP_UpdatedBy = data.UserId;
                            resultupdate.VTADAAAP_UpdatedDate = DateTime.Now;
                            _context.Update(resultupdate);
                        }
                        if (resultone.VTADAAA_Id > 0)
                        {
                            resultone.VTADAAA_TotalPaidAmount = data.VTADAAAP_AmountPaid;
                            _context.Update(resultone);
                        }
                        int a = _context.SaveChanges();
                        if (a > 0)
                        {
                            data.returnvalue = true;
                            data.returnval = "Update";

                        }
                        else
                        {
                            data.returnvalue = true;
                            data.returnval = "UpdateFailed";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_AdavanceApply_PaymentDTO Viewdetails(TADA_AdavanceApply_PaymentDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TADA_ADavanceMltiple_SDetailsAmt_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.VTADAAA_Id
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

                        data.Veiwdetails = retObject.ToArray();


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

        //deactive
        public TADA_AdavanceApply_PaymentDTO deactive(TADA_AdavanceApply_PaymentDTO data)
        {
            try
            {
                data.returnvalue = false;
                var resultone = _context.TADA_AdavanceApply_PaymentDMO.Where(t => t.VTADAAAP_Id == data.VTADAAAP_Id).FirstOrDefault();

                if (resultone.VTADAAAP_ActiveFlg == true)
                {
                    resultone.VTADAAAP_ActiveFlg = false;
                    resultone.VTADAAAP_UpdatedDate = DateTime.Now;
                    resultone.VTADAAAP_UpdatedBy = data.UserId;
                    _context.Update(resultone);
                }
                else
                {
                    if (resultone.VTADAAAP_ActiveFlg == false)
                    {
                        resultone.VTADAAAP_ActiveFlg = true;
                        resultone.VTADAAAP_UpdatedDate = DateTime.Now;
                        resultone.VTADAAAP_UpdatedBy = data.UserId;
                        _context.Update(resultone);
                    }
                }
                int returnval = _context.SaveChanges();
                if (returnval > 0)
                {
                    data.returnvalue = true;
                    data.returnval = "Delete";
                }
                else
                {
                    data.returnval = "NotDelete";
                    data.returnvalue = true;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        //email
        public string sendmail(long MI_Id, string Email, string Template, long UserID, string mailcc)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
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
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
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
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;
                }

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
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        // mailcc = "";
                        string mailbcc = "";
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
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


                        // mailcc = "";
                        string mailbcc = "";
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
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

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
                            // Value = Mailcontent
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
