using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class CheckBankBalanceImpl : Interfaces.CheckBankBalanceInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;
        public CheckBankBalanceImpl(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;
            //
        }

        public CheckBankBalanceDTO loaddata(CheckBankBalanceDTO data)
        {
            try
            {
                data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
                //data.sourcelist = _IssueManagercontext.VMS_CheckBookSourceDMO.Where(L => L.VCBSRC_ActiveFlg == true).Distinct().ToArray();
                data.sourcelist = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Distinct().ToArray();
                data.state_list = _IssueManagercontext.IVRM_Master_StateDMO.Where(R => R.IVRMMC_Id == 101).Distinct().ToArray();
                //stateList

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CheckBankBalanceDTO companychange(CheckBankBalanceDTO data)
        {
            try
            {

                //data.MasterChecks = _IssueManagercontext.VMSMaster_ChequeBookDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();

                data.get_supplier = _IssueManagercontext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_Id).Distinct().ToArray();
                List<AcademicYear> list = new List<AcademicYear>();

                data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_BANKBALANCE_ENTRY";
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
                        data.alldata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_CHECKBOOK_CONTRENTRY";
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
                        data.alldatatwo = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public VMSMaster_ChequeBookdto getProject(VMSMaster_ChequeBookdto data)
        {
            try
            {
                List<long> MI_IdS = new List<long>();
                if (data.emplist != null && data.emplist.Length > 0)
                {
                    foreach (var c in data.emplist)
                    {
                        MI_IdS.Add(c.MI_Id);
                    }
                }
                data.banklist = (from b in _IssueManagercontext.HR_Master_BankDeatils
                                 from c in _IssueManagercontext.Institution
                                 where (MI_IdS.Contains(b.MI_Id) && b.MI_Id == c.MI_Id)
                                 select new VMSMaster_ChequeBookdto
                                 {
                                     HRMBD_Id = b.HRMBD_Id,
                                     MI_Name = c.MI_Name,
                                     HRMBD_BankName = b.HRMBD_BankName,
                                     HRMBD_BankAccountNo = b.HRMBD_BankAccountNo,
                                 }
                                 ).Distinct().ToArray();
                data.get_supplier = (from b in _IssueManagercontext.INV_Master_SupplierDMO
                                     from c in _IssueManagercontext.Institution
                                     where (MI_IdS.Contains(b.MI_Id) && b.MI_Id == c.MI_Id && b.INVMS_ActiveFlg == true)
                                     select new VMSMaster_ChequeBookdto
                                     {
                                         INVMS_Id = b.INVMS_Id,
                                         INVMS_SupplierName = b.INVMS_SupplierName,
                                         MI_Name = c.MI_Name,

                                     }
                               ).Distinct().OrderBy(m => m.INVMS_Id).ToArray();
                data.checkarray = (from a in _IssueManagercontext.VMS_Payment_VoucherDMO
                                   from b in _IssueManagercontext.HR_Master_BankDeatils
                                   from c in _IssueManagercontext.Institution
                                   where (MI_IdS.Contains(a.MI_Id) && a.MI_Id == c.MI_Id && a.HRMBD_Id == b.HRMBD_Id)
                                   select new VMSMaster_ChequeBookdto
                                   {

                                       MI_Name = c.MI_Name,
                                       HRMBD_BankName = b.HRMBD_BankName,
                                       HRMBD_BankAccountNo = b.HRMBD_BankAccountNo,
                                       VPAYVOU_ChequeNo = a.VPAYVOU_ChequeNo,
                                       VPAYVOU_Id = a.VPAYVOU_Id
                                   }
                                  ).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBankBalanceDTO getbom(CheckBankBalanceDTO data)
        {
            try
            {
                //data.projectlist = (from a in _IssueManagercontext.VMS_BankAccount_BRSDMO
                //                    where a.MI_Id == data.MI_Id && a.VBABRS_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Id
                //                    select new CheckBankBalanceDTO
                //                    {
                //                        VBABRS_BankBalalnce = a.VBABRS_ActualBankBalance,
                //                        VBABRS_Id = a.VBABRS_Id
                //                    }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();

                data.projectlist = (from a in _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO
                                    where a.MI_Id == data.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Id
                                    select new CheckBankBalanceDTO
                                    {
                                        VBABRS_BankBalalnce = a.VBABAL_ActualBankBalance,
                                        VBABRS_Id = a.VBABAL_Id
                                    }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        //GetReport
        public CheckBankBalanceDTO getReporttwo(CheckBankBalanceDTO data)
        {
            try
            {
                string fromDate = "";
                string Todate = "";

                if (data.fromdate != null && data.todate != null)
                {
                    fromDate = data.fromdate.Value.ToString("yyyy-MM-dd");
                    Todate = data.todate.Value.ToString("yyyy-MM-dd");
                }


                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BANKBALANCETransactrion_ENTRY";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMBD_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.HRMBD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = fromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = Todate
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
                        data.TrasactionReport = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                //data.TrasactionReport=
                data.FMI_Name = _IssueManagercontext.Institution.Where(R => R.MI_Id == data.MI_Id).Select(p => p.MI_Logo).FirstOrDefault();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        //ClearenceDate
        public CheckBankBalanceDTO ClearenceDate(CheckBankBalanceDTO data)
        {
            try
            {
                string fromDate = "";
                string Todate = "";

                if (data.fromdate != null && data.todate != null)
                {
                    fromDate = data.fromdate.Value.ToString("yyyy-MM-dd");
                    Todate = data.todate.Value.ToString("yyyy-MM-dd");
                }


                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMSPayment_ENTRY";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMBD_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.HRMBD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = fromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = Todate
                    });
                    //@OptionFlag
                    cmd.Parameters.Add(new SqlParameter("@OptionFlag",
                   SqlDbType.VarChar)
                    {
                        Value = data.optionflag
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
                        data.TrasactionReport = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                //data.TrasactionReport=
                data.FMI_Name = _IssueManagercontext.Institution.Where(R => R.MI_Id == data.MI_Id).Select(p => p.MI_Logo).FirstOrDefault();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBankBalanceDTO savedata(CheckBankBalanceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.optionflag == "payment")
                {
                    if (data.VBABRS_Id == 0)
                    {
                        VMS_BankAccount_BRSDMO obj = new VMS_BankAccount_BRSDMO();
                        obj.HRMBD_Id = data.HRMBD_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.VBABRS_Date = data.VBABRS_Date;
                        obj.VBABRS_Amount = data.VBABRS_Amount;
                        obj.VBABRS_CRDRFlg = data.VBABRS_CRDRFlg;
                        obj.VBABRS_Remarks = data.VBABRS_Remarks;
                        obj.VBABRS_ClearedFlg = data.VBABRS_ClearedFlg;
                        obj.VBABRS_ActualBankBalance = data.VBABRS_BankBalalnce;
                        obj.VBABRS_ClearedDate = data.VBABRS_Date;
                        obj.VBABRS_ClearedFlg = true;
                        obj.VBABRS_ActiveFlg = true;
                        obj.VBABRS_CreatedDate = indianTime;
                        obj.VBABRS_UpdatedBy = data.UserId;
                        obj.VBABRS_UpdatedDate = indianTime;
                        obj.VBABRS_CreatedBy = data.UserId;
                        _IssueManagercontext.Add(obj);
                        VMS_BANK_ACCOUNT_BALANCEDMO bnk = new VMS_BANK_ACCOUNT_BALANCEDMO();
                        bnk.VBABAL_ActualBankBalance = data.VBABRS_BankBalalnce;
                        bnk.VBABAL_BalanceAsperVoucher = data.VBABRS_BankBalalnce;
                        bnk.VBABAL_UpdatedBy = data.UserId;
                        bnk.VBABAL_ActiveFlg = true;
                        bnk.VBABAL_CreatedBy = data.UserId;
                        bnk.VBABAL_CreatedDate = DateTime.Now;
                        bnk.VBABAL_UpdatedDate = DateTime.Now;
                        bnk.HRMBD_Id = data.HRMBD_Id;
                        bnk.MI_Id = data.MI_Id;
                        bnk.VBABAL_CRDRFlg = data.VBABRS_CRDRFlg;
                        _IssueManagercontext.Add(bnk);
                        int cxt = _IssueManagercontext.SaveChanges();
                        if (cxt > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                        //
                    }
                    else
                    {
                        var update = _IssueManagercontext.VMS_BankAccount_BRSDMO.Single(e => e.VBABRS_Id == data.VBABRS_Id);

                        update.VBABRS_Date = data.VBABRS_Date;
                        update.VBABRS_Amount = data.VBABRS_Amount;
                        update.VBABRS_CRDRFlg = data.VBABRS_CRDRFlg;
                        update.VBABRS_Remarks = data.VBABRS_Remarks;
                        update.VBABRS_ClearedFlg = data.VBABRS_ClearedFlg;
                        update.VBABRS_ActualBankBalance = data.VBABRS_BankBalalnce;
                        if (data.VBABRS_ClearedFlg == true)
                        {
                            update.VBABRS_ClearedDate = data.VBABRS_ClearedDate;
                        }
                        else
                        {
                            data.VBABRS_ClearedDate = null;
                        }
                        update.VBABRS_UpdatedDate = indianTime;
                        update.VBABRS_UpdatedBy = data.UserId;
                        _IssueManagercontext.Update(update);
                        int cxt = _IssueManagercontext.SaveChanges();
                        if (cxt > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = true;
                        }
                    }
                }
                else if (data.optionflag == "bnktobnk")
                {
                    decimal? creditbankBlance = 0;

                    var debit = (from a in _IssueManagercontext.VMS_BankAccount_BRSDMO
                                 where a.MI_Id == data.MI_Id && a.VBABRS_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Id
                                 select new CheckBankBalanceDTO
                                 {
                                     VBABRS_BankBalalnce = a.VBABRS_ActualBankBalance,
                                     VBABRS_Id = a.VBABRS_Id
                                 }).OrderByDescending(d => d.VBABRS_Id).Take(1).FirstOrDefault();
                    var credit = (from a in _IssueManagercontext.VMS_BankAccount_BRSDMO
                                  where a.MI_Id == data.MI_Id && a.VBABRS_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Idtwo
                                  select new CheckBankBalanceDTO
                                  {
                                      VBABRS_BankBalalnce = a.VBABRS_ActualBankBalance,
                                      VBABRS_Id = a.VBABRS_Id

                                  }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToList();

                    if (credit.Count > 0)
                    {
                        creditbankBlance = credit.FirstOrDefault().VBABRS_BankBalalnce;

                    }
                    else
                    {
                        data.VBABRS_BankBalalnce = 0;
                    }
                    if (debit.VBABRS_BankBalalnce > 0)
                    {
                        VMS_BankAccount_BRSDMO obj = new VMS_BankAccount_BRSDMO();
                        obj.HRMBD_Id = data.HRMBD_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.VBABRS_Date = indianTime;
                        obj.VBABRS_Amount = data.VBABRS_Amounttwo;
                        obj.VBABRS_CRDRFlg = "DR";
                        obj.VBABRS_Remarks = data.VBABRS_Remarks;
                        obj.VBABRS_ClearedFlg = data.VBABRS_ClearedFlg;
                        obj.VBABRS_ActualBankBalance = debit.VBABRS_BankBalalnce - data.VBABRS_Amounttwo;
                        //obj.VBABRS_BankBalalnce = data.VBABRS_BankBalalnce;

                        if (data.VBABRS_ClearedFlg == true)
                        {
                            obj.VBABRS_ClearedDate = data.VBABRS_ClearedDate;
                        }
                        else
                        {
                            obj.VBABRS_ClearedDate = null;
                        }

                        obj.VBABRS_ActiveFlg = true;
                        obj.VBABRS_CreatedDate = indianTime;
                        obj.VBABRS_UpdatedBy = data.UserId;
                        obj.VBABRS_UpdatedDate = indianTime;
                        obj.VBABRS_CreatedBy = data.UserId;
                        _IssueManagercontext.Add(obj);
                        VMS_BankAccount_BRSDMO objtwo = new VMS_BankAccount_BRSDMO();
                        objtwo.HRMBD_Id = data.HRMBD_Idtwo;
                        objtwo.MI_Id = data.MI_Id;
                        objtwo.VBABRS_Date = indianTime;
                        objtwo.VBABRS_Amount = data.VBABRS_Amounttwo;
                        objtwo.VBABRS_CRDRFlg = "CR";
                        objtwo.VBABRS_Remarks = data.VBABRS_Remarks;
                        objtwo.VBABRS_ClearedFlg = data.VBABRS_ClearedFlg;
                        objtwo.VBABRS_ActualBankBalance = creditbankBlance + data.VBABRS_Amounttwo;
                        if (data.VBABRS_ClearedFlg == true)
                        {
                            objtwo.VBABRS_ClearedDate = data.VBABRS_ClearedDate;
                        }
                        else
                        {
                            objtwo.VBABRS_ClearedDate = null;
                        }

                        objtwo.VBABRS_ActiveFlg = true;
                        objtwo.VBABRS_CreatedDate = indianTime;
                        objtwo.VBABRS_UpdatedBy = data.UserId;
                        objtwo.VBABRS_UpdatedDate = indianTime;
                        objtwo.VBABRS_CreatedBy = data.UserId;
                        _IssueManagercontext.Add(objtwo);

                    }


                    int cxt = _IssueManagercontext.SaveChanges();
                    if (cxt > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }


                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public CheckBankBalanceDTO getinvoiceno(CheckBankBalanceDTO data)
        {
            try
            {
                //obj.VPAYVOU_Id
                List<string> VPAYVOU_ChequeNo = new List<string>();
                if(data.VPAYVOU_Id > 0)
                {
                    var ChequeNo = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.HRMBD_Id == data.HRMBD_Id && R.MI_Id == data.MI_Id && R.VPAYVOU_Id != data.VPAYVOU_Id && (R.VPAYVOU_StatusFlg == "Pending" || R.VPAYVOU_StatusFlg == "Approved")).ToList();
                    foreach (var c in ChequeNo)
                    {
                        VPAYVOU_ChequeNo.Add(c.VPAYVOU_ChequeNo);
                    }
                }
                else
                {
                    var ChequeNo = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.HRMBD_Id == data.HRMBD_Id && R.MI_Id == data.MI_Id && (R.VPAYVOU_StatusFlg == "Pending" || R.VPAYVOU_StatusFlg == "Approved")).ToList();
                    foreach (var c in ChequeNo)
                    {
                        VPAYVOU_ChequeNo.Add(c.VPAYVOU_ChequeNo);
                    }
                }
                
                data.CheckbookSource = (from a in _IssueManagercontext.VMSMaster_ChequeBookDMO
                                        from b in _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO
                                        where a.MI_Id == data.MI_Id && a.VMCHEBK_Id == b.VMCHEBK_Id && a.HRMBD_Id == data.HRMBD_Id && !VPAYVOU_ChequeNo.Contains(b.VMCHEBK_LeafNo) 
                                        select new CheckBookEntryDTO
                                        {
                                            VPAYVOU_ChequeNo = b.VMCHEBK_LeafNo,
                                            VMCHEBKSLN_Id = b.VMCHEBKSLN_Id

                                        }).OrderBy(R => R.VPAYVOU_ChequeNo).ToArray();
                var Apply_Detail = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true && a.HRMBD_Id == data.HRMBD_Id).ToList().Select(p => p.HRMBD_Id);

                data.banklistTwo = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true && !Apply_Detail.Contains(a.HRMBD_Id)).Distinct().ToArray();
                //data.projectlistTWO = (from a in _IssueManagercontext.VMS_BankAccount_BRSDMO
                //                       where a.MI_Id == data.MI_Id && a.VBABRS_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Id
                //                       select new CheckBankBalanceDTO
                //                       {
                //                           VBABRS_BankBalalnce = a.VBABRS_ActualBankBalance,
                //                           VBABRS_Id = a.VBABRS_Id
                //                       }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
                data.projectlistTWO = (from a in _IssueManagercontext.VMS_BankAccount_BRSDMO
                                       where a.MI_Id == data.MI_Id && a.VBABRS_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Id
                                    select new CheckBankBalanceDTO
                                    {
                                        VBABRS_BankBalalnce = a.VBABRS_ActualBankBalance,
                                        VBABRS_Id = a.VBABRS_Id
                                    }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
                var OpeningBalnce = _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO.Where(R => R.HRMBD_Id == data.HRMBD_Id).ToList();
                if (OpeningBalnce.Count > 0)
                {
                    data.msg = "OB";
                }
                else
                {
                    data.msg = "NotOB";
                }
                data.ModeOfPayment = _db.IVRM_ModeOfPaymentDMO.Where(R => R.MI_Id == data.MI_Id && R.IVRMMOD_ModeOfPayment != "Cash").Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public CheckBankBalanceDTO Editdata(CheckBankBalanceDTO data)
        {
            try
            {

                if (data.optionflag == "report")
                {
                    data.checkBooklist = (from a in _IssueManagercontext.VMSMaster_ChequeBookDMO
                                          from b in _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO
                                          where a.MI_Id == data.MI_Id && a.VMCHEBK_Id == b.VMCHEBK_Id && a.HRMBD_Id == data.HRMBD_Id
                                          select new CheckBookEntryDTO
                                          {
                                              VPAYVOU_ChequeNo = b.VMCHEBK_LeafNo,
                                              VMCHEBKSLN_Id = b.VMCHEBKSLN_Id

                                          }).Distinct().OrderBy(R => R.VMCHEBKSLN_Id).ToArray();

                }
                else if (data.optionflag == "All")
                {
                    data.MI_Id = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).Select(T => T.MI_Id).FirstOrDefault();
                    data.VPAYVOU_Id = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).Select(L => L.VPAYVOU_Id).FirstOrDefault();
                    data.FMI_Name = _IssueManagercontext.Institution.Where(R => R.MI_Id == data.MI_Id).Select(Y => Y.MI_Logo).FirstOrDefault();

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LOAD_CHECKBOOK_ENTRY_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@VPAYVOU_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.VPAYVOU_Id
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
                            data.checkDetails = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CheckBookGRNDEtalsApproval";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@VPAYVOU_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.VPAYVOU_Id
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
                            data.Checkbookgrn = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //VMS_Payment_ApprovalRemark
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Payment_ApprovalRemarkl";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@VPAYVOU_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.VPAYVOU_Id
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
                            data.projectlistTWO = retObject.ToArray();
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
            return data;
        }
        public CheckBankBalanceDTO clientDecative(CheckBankBalanceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var u = _IssueManagercontext.VMS_BankAccount_BRSDMO.Where(t => t.VBABRS_Id == data.VBABRS_Id).SingleOrDefault();
                if (u.VBABRS_ActiveFlg == true)
                {
                    u.VBABRS_ActiveFlg = false;
                }
                else if (u.VBABRS_ActiveFlg == false)
                {
                    u.VBABRS_ActiveFlg = true;
                }

                u.VBABRS_UpdatedBy = data.UserId;
                u.VBABRS_UpdatedDate = indianTime;
                _IssueManagercontext.Update(u);
                int o = _IssueManagercontext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public VMSMaster_ChequeBookdto viewdetails(VMSMaster_ChequeBookdto data)
        {
            try
            {
                data.edtilistM = _IssueManagercontext.VMSMaster_ChequeBookDMO.Where(R => R.VMCHEBK_Id == data.VMCHEBK_Id).Distinct().ToArray();
                data.edtilistP = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Where(R => R.VMCHEBK_Id == data.VMCHEBK_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VMSMaster_ChequeBookdto savepaymentdetailsrecord(VMSMaster_ChequeBookdto data)
        {
            try
            {

                if (data.slnocheck != null)
                {

                    if (data.VMCHEBK_Id > 0)
                    {
                        foreach (var slno in data.slnocheck)
                        {
                            var duplicate = (from a in _IssueManagercontext.VMSMaster_ChequeBookDMO
                                             from b in _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO
                                             where a.MI_Id == data.MI_Id && a.VMCHEBK_Id == b.VMCHEBK_Id && a.HRMBD_Id == data.HRMBD_Id && b.VMCHEBK_LeafNo == slno.VMCHEBK_LeafNo && a.VMCHEBK_Id !=data.VMCHEBK_Id
                                             select new CheckBookEntryDTO
                                             {
                                                 VPAYVOU_ChequeNo = b.VMCHEBK_LeafNo,
                                                 VMCHEBKSLN_Id = b.VMCHEBKSLN_Id

                                             }).ToList();
                            // var duplicate = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Where(R => R.VMCHEBK_LeafNo == slno.VMCHEBK_LeafNo && R.VMCHEBK_Id != data.VMCHEBK_Id).ToList();
                            if (duplicate.Count > 0)
                            {
                                data.VMCHEBK_Remarks = "duplicate";
                                return data;
                            }
                        }
                        var updateone = _IssueManagercontext.VMSMaster_ChequeBookDMO.Where(R => R.VMCHEBK_Id == data.VMCHEBK_Id).FirstOrDefault();
                        updateone.VMCHEBK_ReceivedDate = data.VMCHEBK_ReceivedDate;
                        updateone.VMCHEBK_NoOfChequeLeafs = data.VMCHEBK_NoOfChequeLeafs;
                        updateone.VMCHEBK_SLNoFrom = data.VMCHEBK_SLNoFrom;
                        updateone.VMCHEBK_SLNoTo = data.VMCHEBK_SLNoTo;
                        updateone.VMCHEBK_Remarks = data.VMCHEBK_Remarks;
                        updateone.VMCHEBK_UpdatedBy = data.UserId;
                        updateone.VMCHEBK_UpdatedDate = DateTime.Now;
                        _IssueManagercontext.Update(updateone);
                        var leafRemove = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Where(R => R.VMCHEBK_Id == data.VMCHEBK_Id).ToList();
                        if (leafRemove.Count > 0)
                        {
                            foreach (var r in leafRemove)
                            {
                                _IssueManagercontext.Remove(r);
                            }

                        }
                        foreach (var slno in data.slnocheck)
                        {
                            VMSMaster_ChequeBook_SLNoDMO dobj = new VMSMaster_ChequeBook_SLNoDMO();
                            dobj.VMCHEBK_Id = data.VMCHEBK_Id;
                            dobj.VMCHEBK_LeafNo = slno.VMCHEBK_LeafNo;
                            dobj.VMCHEBKSLN_CancelledFlg = false;
                           // dobj.VMCHEBKSLN_Remarks = data.VMCHEBK_Remarks;
                            dobj.VMCHEBKSLN_ActiveFlg = true;
                            dobj.VMCHEBKSLN_CreatedBy = data.UserId;
                            dobj.VMCHEBKSLN_CreatedDate = DateTime.Now;
                            dobj.VMCHEBKSLN_UpdatedBy = data.UserId;
                            dobj.VMCHEBKSLN_UpdatedDate = DateTime.Now;
                            _IssueManagercontext.Add(dobj);
                        }
                        int cxt = _IssueManagercontext.SaveChanges();
                        if (cxt > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        foreach (var slno in data.slnocheck)
                        {
                            var duplicate = (from a in _IssueManagercontext.VMSMaster_ChequeBookDMO
                                                  from b in _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO
                                                  where a.MI_Id == data.MI_Id && a.VMCHEBK_Id == b.VMCHEBK_Id && a.HRMBD_Id == data.HRMBD_Id && b.VMCHEBK_LeafNo== slno.VMCHEBK_LeafNo
                                             select new CheckBookEntryDTO
                                                  {
                                                      VPAYVOU_ChequeNo = b.VMCHEBK_LeafNo,
                                                      VMCHEBKSLN_Id = b.VMCHEBKSLN_Id

                                                  }).ToList();
                            
                            //var duplicate = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Where(R => R.VMCHEBK_LeafNo == slno.VMCHEBK_LeafNo ).ToList();
                            if (duplicate.Count > 0)
                            {
                                data.VMCHEBK_Remarks = "duplicate";
                                return data;
                            }
                        }
                        VMSMaster_ChequeBookDMO obj = new VMSMaster_ChequeBookDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HRMBD_Id = data.HRMBD_Id;
                        obj.VMCHEBK_ReceivedDate = data.VMCHEBK_ReceivedDate;
                        obj.VMCHEBK_NoOfChequeLeafs = data.VMCHEBK_NoOfChequeLeafs;
                        obj.VMCHEBK_SLNoFrom = data.VMCHEBK_SLNoFrom;
                        obj.VMCHEBK_SLNoTo = data.VMCHEBK_SLNoTo;
                        obj.VMCHEBK_Remarks = data.VMCHEBK_Remarks;
                        obj.VMCHEBK_ActiveFlg = true;
                        obj.VMCHEBK_CreatedBy = data.UserId;
                        obj.VMCHEBK_CreatedDate = DateTime.Now;
                        obj.VMCHEBK_UpdatedBy = data.UserId;
                        obj.VMCHEBK_UpdatedDate = DateTime.Now;
                        _IssueManagercontext.Add(obj);
                        if (data.slnocheck != null)
                        {
                            foreach (var slno in data.slnocheck)
                            {
                                VMSMaster_ChequeBook_SLNoDMO dobj = new VMSMaster_ChequeBook_SLNoDMO();
                                dobj.VMCHEBK_Id = obj.VMCHEBK_Id;
                                dobj.VMCHEBK_LeafNo = slno.VMCHEBK_LeafNo;
                                dobj.VMCHEBKSLN_CancelledFlg = false;
                               // dobj.VMCHEBKSLN_Remarks = data.VMCHEBK_Remarks;
                                dobj.VMCHEBKSLN_ActiveFlg = true;
                                dobj.VMCHEBKSLN_CreatedBy = data.UserId;
                                dobj.VMCHEBKSLN_CreatedDate = DateTime.Now;
                                dobj.VMCHEBKSLN_UpdatedBy = data.UserId;
                                dobj.VMCHEBKSLN_UpdatedDate = DateTime.Now;
                                _IssueManagercontext.Add(dobj);
                            }
                        }

                        int cxt = _IssueManagercontext.SaveChanges();
                        if (cxt > 0)
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

        // Payment Configuration
        public VMSMaster_ChequeBookdto loaddataconfig(VMSMaster_ChequeBookdto data)
        {
            try
            {
                List<long> MI_IdS = new List<long>();
                if (data.emplist != null && data.emplist.Length > 0)
                {
                    foreach (var c in data.emplist)
                    {
                        MI_IdS.Add(c.MI_Id);
                    }
                }
                string Inmvs_list = "0";
                if (data.Inmvs_list != null && data.Inmvs_list.Length > 0)
                {
                    foreach (var ue in data.Inmvs_list)
                    {
                        Inmvs_list = Inmvs_list + "," + ue.INVMS_Id;
                    }

                }
                string milist = "0";
                if (data.emplist != null && data.emplist.Length > 0)
                {
                    foreach (var ue in data.emplist)
                    {
                        milist = milist + "," + ue.MI_Id;
                    }

                }
                string hrmdlist = "0";
                if (data.hrmdlist != null && data.hrmdlist.Length > 0)
                {
                    foreach (var ue in data.hrmdlist)
                    {
                        hrmdlist = hrmdlist + "," + ue.HRMD_Id;
                    }

                }
                string FromDate = "";
                string ToDate = "";
                data.VMCHEBK_SLNoFrom = "";
                if (data.CreatedDate != null && data.UpdatedDate != null)
                {
                    FromDate = data.CreatedDate.Value.ToString("yyyy-MM-dd");
                    ToDate = data.UpdatedDate.Value.ToString("yyyy-MM-dd");

                }

                if (data.optionflag == "vmscheckbook")
                {

                    data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();
                    data.MasterChecks = (from a in _IssueManagercontext.VMSMaster_ChequeBookDMO
                                         from b in _IssueManagercontext.HR_Master_BankDeatils
                                         from c in _IssueManagercontext.Institution
                                         where (a.HRMBD_Id == b.HRMBD_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id)
                                         select new VMSMaster_ChequeBookdto
                                         {
                                             HRMBD_Id = a.HRMBD_Id,
                                             VMCHEBK_ReceivedDate = a.VMCHEBK_ReceivedDate,
                                             VMCHEBK_NoOfChequeLeafs = a.VMCHEBK_NoOfChequeLeafs,
                                             VMCHEBK_SLNoFrom = a.VMCHEBK_SLNoFrom,
                                             VMCHEBK_SLNoTo = a.VMCHEBK_SLNoTo,
                                             VMCHEBK_ActiveFlg = a.VMCHEBK_ActiveFlg,
                                             VMCHEBK_Id = a.VMCHEBK_Id,
                                             MI_Id = a.MI_Id,
                                             HRMBD_BankName = b.HRMBD_BankName,
                                             HRMBD_BankAccountNo = b.HRMBD_BankAccountNo,
                                             MI_Name = c.MI_Name,
                                             VMCHEBK_Remarks = a.VMCHEBK_Remarks,


                                         }
                                        ).Distinct().ToArray();
                }
                else
                {
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ChequeLeafCancel";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar)
                        {
                            Value = milist
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMBD_Id",
                         SqlDbType.VarChar)
                        {
                            Value = hrmdlist
                        });
                        cmd.Parameters.Add(new SqlParameter("@optionflag",
                       SqlDbType.VarChar)
                        {
                            Value = data.optionflag
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                       SqlDbType.VarChar)
                        {
                            Value = FromDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                       SqlDbType.VarChar)
                        {
                            Value = ToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                      SqlDbType.VarChar)
                        {
                            Value = Inmvs_list
                        });
                        cmd.Parameters.Add(new SqlParameter("@VPAYVOU_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.VPAYVOU_Idtwo
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
                            data.checckleafTotal = retObject.ToArray();


                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
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
        public CheckBankBalanceDTO savedataconfig(CheckBankBalanceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ISMCPC_Id > 0)
                {
                    var checkresult = _IssueManagercontext.ISM_Client_Payment_ConfigurationDMO.Single(a => a.ISMCPC_Id == data.ISMCPC_Id);
                    checkresult.ISMCPC_RemainderDays = data.ISMCPC_RemainderDays;
                    checkresult.ISMCPC_FullORPartialPayment = data.ISMCPC_FullORPartialPayment;
                    checkresult.UpdatedDate = indiantime0;
                    _IssueManagercontext.Update(checkresult);
                    var i = _IssueManagercontext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
                else
                {
                    ISM_Client_Payment_ConfigurationDMO iSM_Client_Payment_ConfigurationDMO = new ISM_Client_Payment_ConfigurationDMO();
                    iSM_Client_Payment_ConfigurationDMO.ISMCPC_RemainderDays = data.ISMCPC_RemainderDays;
                    iSM_Client_Payment_ConfigurationDMO.ISMCPC_FullORPartialPayment = data.ISMCPC_FullORPartialPayment;
                    iSM_Client_Payment_ConfigurationDMO.UpdatedDate = indiantime0;
                    iSM_Client_Payment_ConfigurationDMO.CreatedDate = indiantime0;
                    _IssueManagercontext.Add(iSM_Client_Payment_ConfigurationDMO);

                    var i = _IssueManagercontext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                data.msg = "Failed";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment subscription audit report
        public CheckBankBalanceDTO loaddatareport(CheckBankBalanceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var u = _IssueManagercontext.VMSMaster_ChequeBookDMO.Where(t => t.VMCHEBK_Id == data.VMCHEBK_Id).SingleOrDefault();
                if (u.VMCHEBK_ActiveFlg == true)
                {
                    u.VMCHEBK_ActiveFlg = false;
                }
                else if (u.VMCHEBK_ActiveFlg == false)
                {
                    u.VMCHEBK_ActiveFlg = true;
                }

                u.VMCHEBK_UpdatedBy = data.UserId;
                u.VMCHEBK_UpdatedDate = indianTime;
                _IssueManagercontext.Update(u);
                int o = _IssueManagercontext.SaveChanges();
                if (o > 0)
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
        public CheckBankBalanceDTO getreport(CheckBankBalanceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.cleararrflag != null)
                {
                    if(data.optionflag== "PAYMENT" || data.optionflag== "CONTRA")
                    {
                        foreach (var flag in data.cleararrflag)
                        {

                            var indentres = _IssueManagercontext.VMS_Payment_VoucherDMO.Single(a => a.VPAYVOU_Id == flag.VPAYVOU_Id);
                            var VBABRS_RefrenceId = "";
                            if (indentres.VPAYVOU_ChequeNo != "")
                            {
                                VBABRS_RefrenceId = indentres.VPAYVOU_ChequeNo;
                            }
                            else if (indentres.VPAYVOU_PaymentReference != "")
                            {
                                VBABRS_RefrenceId = indentres.VPAYVOU_PaymentReference;
                            }

                            decimal? penatlties = 0;
                            if (indentres.VPAYVOU_TDS > 0)
                            {
                                penatlties += indentres.VPAYVOU_TDS;
                            }
                            if (indentres.VPAYVOU_Penalty > 0)
                            {
                                penatlties += indentres.VPAYVOU_Penalty;
                            }
                            if (indentres.VPAYVOU_ESICAmount > 0)
                            {
                                penatlties += indentres.VPAYVOU_ESICAmount;
                            }
                            if (indentres.VPAYVOU_OtherAmount > 0)
                            {
                                penatlties += indentres.VPAYVOU_OtherAmount;
                            }
                            if (indentres.HRMBD_InterbankTransFlg == true && indentres.VPAYVOU_HRMBD_Id > 0)
                            {

                                decimal? creditbankBlance = 0;
                                var debit = (from a in _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO
                                             where a.MI_Id == indentres.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == indentres.HRMBD_Id
                                             select new CheckBankBalanceDTO
                                             {
                                                 VBABRS_BankBalalnce = a.VBABAL_ActualBankBalance,
                                                 VBABRS_Id = a.VBABAL_Id
                                             }).OrderByDescending(d => d.VBABRS_Id).Take(1).FirstOrDefault();

                                var credit = (from a in _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO
                                              where a.MI_Id == indentres.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == indentres.VPAYVOU_HRMBD_Id
                                              select new CheckBankBalanceDTO
                                              {
                                                  VBABRS_BankBalalnce = a.VBABAL_ActualBankBalance,
                                                  VBABRS_Id = a.VBABAL_Id
                                              }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToList();

                                if (credit.Count > 0)
                                {
                                    creditbankBlance = credit.FirstOrDefault().VBABRS_BankBalalnce;

                                }
                                else
                                {
                                    creditbankBlance = 0;
                                }

                                if (debit.VBABRS_BankBalalnce > 0)
                                {
                                    VMS_BankAccount_BRSDMO bnktobnkFrom = new VMS_BankAccount_BRSDMO();
                                    bnktobnkFrom.VBABRS_RefrenceId = VBABRS_RefrenceId;
                                    bnktobnkFrom.VPAYVOU_Id = indentres.VPAYVOU_Id;
                                    bnktobnkFrom.HRMBD_Id = indentres.HRMBD_Id;
                                    bnktobnkFrom.MI_Id = indentres.MI_Id;
                                    bnktobnkFrom.VBABRS_Date = indentres.VPAYVOU_PaymentDate;
                                    bnktobnkFrom.VBABRS_Amount = indentres.VPAYVOU_ApprovedAmount;
                                    bnktobnkFrom.VBABRS_CRDRFlg = "DR";
                                    bnktobnkFrom.VBABRS_Remarks = indentres.VPAYVOU_Remarks;
                                    bnktobnkFrom.VBABRS_ClearedDate = flag.VBABRS_ClearedDateone;
                                    bnktobnkFrom.VBABRS_ActualBankBalance = debit.VBABRS_BankBalalnce -        indentres.VPAYVOU_ApprovedAmount - penatlties;
                                    bnktobnkFrom.VBABRS_ClearedDate = flag.VBABRS_ClearedDateone;
                                    bnktobnkFrom.VBABRS_ClearedFlg = true;
                                    bnktobnkFrom.VBABRS_ActiveFlg = true;
                                    bnktobnkFrom.VBABRS_CreatedDate = indianTime;
                                    bnktobnkFrom.VBABRS_UpdatedBy = data.UserId;
                                    bnktobnkFrom.VBABRS_UpdatedDate = indianTime;
                                    bnktobnkFrom.VBABRS_CreatedBy = data.UserId;
                                    bnktobnkFrom.VBABRS_BalanceAsperVoucher = indentres.VPAYVOU_BankBalalnce;
                                    _IssueManagercontext.Add(bnktobnkFrom);

                                    VMS_BANK_ACCOUNT_BALANCEDMO bnk = new VMS_BANK_ACCOUNT_BALANCEDMO();
                                    bnk.VBABAL_ActualBankBalance = debit.VBABRS_BankBalalnce - indentres.VPAYVOU_ApprovedAmount - penatlties;
                                    bnk.VBABAL_BalanceAsperVoucher = indentres.VPAYVOU_BankBalalnce;
                                    bnk.VBABAL_UpdatedBy = data.UserId;
                                    bnk.VBABAL_ActiveFlg = true;
                                    bnk.VBABAL_CreatedBy = data.UserId;
                                    bnk.VBABAL_CreatedDate = DateTime.Now;
                                    bnk.VBABAL_UpdatedDate = DateTime.Now;
                                    bnk.HRMBD_Id = indentres.HRMBD_Id;
                                    bnk.MI_Id = indentres.MI_Id;
                                    bnk.VBABAL_CRDRFlg = "DR"; ;
                                    _IssueManagercontext.Add(bnk);
                                    //BankAccount
                                    VMS_BankAccount_BRSDMO bnkto = new VMS_BankAccount_BRSDMO();
                                    bnkto.VBABRS_RefrenceId = VBABRS_RefrenceId;
                                    bnkto.HRMBD_Id = indentres.VPAYVOU_HRMBD_Id;
                                    bnkto.VPAYVOU_Id = indentres.VPAYVOU_Id;
                                    bnkto.MI_Id = indentres.MI_Id;
                                    bnkto.VBABRS_Date = indentres.VPAYVOU_PaymentDate;
                                    bnkto.VBABRS_Amount = indentres.VPAYVOU_ApprovedAmount;
                                    bnkto.VBABRS_CRDRFlg = "CR";
                                    bnkto.VBABRS_Remarks = indentres.VPAYVOU_Remarks;
                                    bnkto.VBABRS_ClearedFlg = true;
                                    bnkto.VBABRS_ClearedDate = flag.VBABRS_ClearedDateone;
                                    bnkto.VBABRS_ActualBankBalance = creditbankBlance + indentres.VPAYVOU_ApprovedAmount;
                                    bnkto.VBABRS_ActiveFlg = true;
                                    bnkto.VBABRS_CreatedDate = indianTime;
                                    bnkto.VBABRS_UpdatedBy = data.UserId;
                                    bnkto.VBABRS_UpdatedDate = indianTime;
                                    bnkto.VBABRS_CreatedBy = data.UserId;
                                    bnkto.VBABRS_BalanceAsperVoucher = indentres.VPAYVOU_BankBalalnce;
                                    _IssueManagercontext.Add(bnkto);

                                    VMS_BANK_ACCOUNT_BALANCEDMO bnkp = new VMS_BANK_ACCOUNT_BALANCEDMO();

                                    bnkp.VBABAL_ActualBankBalance = creditbankBlance + indentres.VPAYVOU_ApprovedAmount;
                                    // bnkp.VBABAL_BalanceAsperVoucher = indentres.VPAYVOU_BankBalalnce;
                                    bnkp.VBABAL_BalanceAsperVoucherReceipt = indentres.VPAYVOU_BankBalalnce;
                                    bnkp.VBABAL_UpdatedBy = data.UserId;
                                    bnkp.VBABAL_ActiveFlg = true;
                                    bnkp.VBABAL_CreatedBy = data.UserId;
                                    bnkp.VBABAL_CreatedDate = DateTime.Now;
                                    bnkp.VBABAL_UpdatedDate = DateTime.Now;
                                    bnkp.HRMBD_Id = indentres.VPAYVOU_HRMBD_Id.Value;
                                    bnkp.MI_Id = indentres.MI_Id;
                                    bnkp.VBABAL_CRDRFlg = "CR";
                                    _IssueManagercontext.Add(bnkp);
                                }
                            }
                            else
                            {
                                var bnkbalance = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(w => w.VPAYVOU_Id == flag.VPAYVOU_Id).FirstOrDefault();


                                var listprj = (from a in _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO
                                               where a.MI_Id == indentres.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == indentres.HRMBD_Id
                                               select new CheckBankBalanceDTO
                                               {
                                                   VBABRS_BankBalalnce = a.VBABAL_ActualBankBalance,
                                                   VBABRS_Id = a.VBABAL_Id
                                               }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
                                VMS_BankAccount_BRSDMO obj1 = new VMS_BankAccount_BRSDMO();
                                obj1.VBABRS_RefrenceId = VBABRS_RefrenceId;
                                obj1.MI_Id = indentres.MI_Id;
                                obj1.HRMBD_Id = indentres.HRMBD_Id;
                                obj1.VBABRS_Date = indentres.VPAYVOU_PaymentDate;
                                obj1.VBABRS_Amount = indentres.VPAYVOU_ApprovedAmount;
                                obj1.VBABRS_CRDRFlg = "DR";
                                obj1.VBABRS_Remarks = indentres.VPAYVOU_Remarks;
                                obj1.VBABRS_ActualBankBalance = listprj.FirstOrDefault().VBABRS_BankBalalnce - indentres.VPAYVOU_ApprovedAmount - penatlties;
                                obj1.VBABRS_ActiveFlg = true;
                                obj1.VBABRS_CreatedBy = data.UserId;
                                obj1.VBABRS_CreatedDate = DateTime.Now;
                                obj1.VBABRS_UpdatedBy = data.UserId;
                                obj1.VBABRS_UpdatedDate = DateTime.Now;
                                obj1.VPAYVOU_Id = indentres.VPAYVOU_Id;
                                obj1.VBABRS_BalanceAsperVoucher = indentres.VPAYVOU_BankBalalnce;
                                obj1.VBABRS_ClearedFlg = true;
                                obj1.VBABRS_ClearedDate = flag.VBABRS_ClearedDateone;
                                _IssueManagercontext.Add(obj1);
                                //added another bank 
                                VMS_BANK_ACCOUNT_BALANCEDMO bnk = new VMS_BANK_ACCOUNT_BALANCEDMO();
                                bnk.VBABAL_ActualBankBalance = listprj.FirstOrDefault().VBABRS_BankBalalnce - indentres.VPAYVOU_ApprovedAmount - penatlties;
                                bnk.VBABAL_BalanceAsperVoucher = indentres.VPAYVOU_BankBalalnce;
                                bnk.VBABAL_UpdatedBy = data.UserId;
                                bnk.VBABAL_ActiveFlg = true;
                                bnk.VBABAL_CreatedBy = data.UserId;
                                bnk.VBABAL_CreatedDate = DateTime.Now;
                                bnk.VBABAL_UpdatedDate = DateTime.Now;
                                bnk.HRMBD_Id = indentres.HRMBD_Id;
                                bnk.MI_Id = indentres.MI_Id;
                                bnk.VBABAL_CRDRFlg = "DR";
                                _IssueManagercontext.Add(bnk);
                            }


                        }
                    }
                    else
                    {
                        if(data.optionflag== "RECEIPT")
                        {
                            foreach (var flag in data.cleararrflag)
                            {
                                var indentres = _IssueManagercontext.VMS_Receipt_VoucherDMO.Single(a => a.VRECVOU_Id == flag.VPAYVOU_Id);
                                var debit = (from a in _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO
                                             where a.MI_Id == data.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == indentres.HRMBD_Id
                                             select new VMS_Receipt_VoucherDTO
                                             {
                                                 VBABRS_ActualBankBalance = a.VBABAL_ActualBankBalance,
                                                 VBABRS_Id = a.VBABAL_Id
                                             }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToList();
                                decimal? VBABRS_ActualBankBalance = 0;
                                if (debit.Count > 0)
                                {
                                    VBABRS_ActualBankBalance = debit.FirstOrDefault().VBABRS_ActualBankBalance;

                                }
                                else
                                {
                                    VBABRS_ActualBankBalance = 0;
                                }

                                VMS_BankAccount_BRSDMO bnkto = new VMS_BankAccount_BRSDMO();
                                bnkto.HRMBD_Id = indentres.HRMBD_Id;
                                bnkto.VBABRS_RefrenceId = indentres.VRECVOU_ReceiptReference;
                                bnkto.MI_Id = indentres.MI_Id;
                                bnkto.VBABRS_Date = indentres.VRECVOU_Date;
                                bnkto.VBABRS_Amount = indentres.VRECVOU_Amount;
                                bnkto.VBABRS_CRDRFlg = "CR";
                                bnkto.VBABRS_Remarks = indentres.VRECVOU_Remarks;
                                bnkto.VBABRS_ActualBankBalance = VBABRS_ActualBankBalance + indentres.VRECVOU_Amount;
                                bnkto.VBABRS_BalanceAsperVoucher = VBABRS_ActualBankBalance;
                                bnkto.VBABRS_ClearedDate = flag.VBABRS_ClearedDateone;
                                bnkto.VBABRS_ClearedFlg = true;
                                bnkto.VBABRS_ActiveFlg = true;
                                bnkto.VBABRS_CreatedDate = indianTime;
                                bnkto.VBABRS_UpdatedBy = data.UserId;
                                bnkto.VBABRS_UpdatedDate = indianTime;
                                bnkto.VBABRS_CreatedBy = data.UserId;
                                bnkto.ISMMCLT_Id = indentres.ISMMCLT_Id;
                                bnkto.VRECVOU_Id = indentres.VRECVOU_Id;
                                _IssueManagercontext.Add(bnkto);
                                VMS_BANK_ACCOUNT_BALANCEDMO bnk = new VMS_BANK_ACCOUNT_BALANCEDMO();
                                bnk.VBABAL_ActualBankBalance = VBABRS_ActualBankBalance + indentres.VRECVOU_Amount;
                               
                                bnk.VBABAL_BalanceAsperVoucherReceipt = VBABRS_ActualBankBalance;
                                bnk.VBABAL_UpdatedBy = data.UserId;
                                bnk.VBABAL_ActiveFlg = true;
                                bnk.VBABAL_CreatedBy = data.UserId;
                                bnk.VBABAL_CreatedDate = DateTime.Now;
                                bnk.VBABAL_UpdatedDate = DateTime.Now;
                                bnk.HRMBD_Id = indentres.HRMBD_Id.Value;
                                bnk.MI_Id = indentres.MI_Id;
                                bnk.VBABAL_CRDRFlg = "CR";
                                _IssueManagercontext.Add(bnk);
                            }
                            

                        }
                    }
                  
                    var i = _IssueManagercontext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //payment notification
        public CheckBankBalanceDTO paymentnotification(CheckBankBalanceDTO data)
        {
            try
            {
                data.getpaymentnotificationdetails = _IssueManagercontext.VMSMaster_ChequeBookDMO.Where(R => R.VMCHEBK_Id == data.VMCHEBK_Id).Distinct().ToArray();
                data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ChecKbOOkNoSLNO";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMCHEBK_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.VMCHEBK_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMBD_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRMBD_Id
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
                        data.CheckbookSource = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
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