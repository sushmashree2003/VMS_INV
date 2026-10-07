using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class VMS_Receipt_VoucherIMPL : Interfaces.VMS_Receipt_VoucherInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;
        public VMS_Receipt_VoucherIMPL(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;
        }
        public VMS_Receipt_VoucherDTO loaddata(VMS_Receipt_VoucherDTO data)
        {
            try
            {
                data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
                data.checkbookSource = _IssueManagercontext.VMS_CheckBookSourceDMO.Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //RecieptReport
        public VMS_Receipt_VoucherDTO RecieptReport(VMS_Receipt_VoucherDTO data)
        {
            try
            {
                string startdate = "";
                string enddate = "";
                if (data.startdate != null && data.enddate != null)
                {
                    startdate = data.startdate.Value.ToString();
                    enddate = data.enddate.Value.ToString();
                }
                string hrmdid = "0";
                foreach (var ii in data.Banklistdetails)
                {
                    hrmdid = hrmdid + "," + ii.HRMD_Id;
                }
                string MI_Id = "0";
                MI_Id= MI_Id + "," + data.MI_Id;
                data.logo = _IssueManagercontext.Institution.Where(R => R.MI_Id == data.MI_Id).Select(T => T.MI_Logo).FirstOrDefault();
                //LOAD_PAYMENTRECIPENT_Report
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_PAYMENTRECIPENT_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMBD_Id",
                      SqlDbType.VarChar)
                    {
                        Value = hrmdid
                    });
                    cmd.Parameters.Add(new SqlParameter("@startdate",
                     SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate",
                    SqlDbType.VarChar)
                    {
                        Value = enddate
                    });
                    //enddate
                    cmd.Parameters.Add(new SqlParameter("@optionflag",
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
                        data.allReport = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                //if (data.optionflag=="")
                //{

                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VMS_Receipt_VoucherDTO savedata(VMS_Receipt_VoucherDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);               

                var debit = (from a in _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO
                                    where a.MI_Id == data.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Id
                                    select new VMS_Receipt_VoucherDTO
                                    {
                                        VBABRS_ActualBankBalance = a.VBABAL_ActualBankBalance,
                                        VBABRS_Id = a.VBABAL_Id
                                    }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToList();

                if (debit.Count > 0)
                {
                    data.VBABRS_ActualBankBalance = debit.FirstOrDefault().VBABRS_ActualBankBalance;

                }
                else
                {
                    data.VBABRS_ActualBankBalance = 0;
                }
                if (data.VRECVOU_Id > 0)
                {

                }
                else
                {
                    var VRECVOU_VoucherNo = _IssueManagercontext.VMS_Receipt_VoucherDMO.ToList();
                    var VoucherNo = 0;
                    if(VRECVOU_VoucherNo.Count > 0)
                    {
                        VoucherNo = VRECVOU_VoucherNo.Count + 1;
                    }
                    else
                    {
                        VoucherNo = 1;
                    }

                    VMS_Receipt_VoucherDMO obj = new VMS_Receipt_VoucherDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.VRECVOU_FromClientFlg = false;
                    obj.ISMMCLT_Id = data.ISMMCLT_Id;
                    obj.HRMBD_Id = data.HRMBD_Id;
                    obj.VRECVOU_ReceiptMode = data.VRECVOU_ReceiptMode;
                    obj.VRECVOU_ReceiptReference = data.VRECVOU_ReceiptReference;                  
                    obj.VRECVOU_VoucherNo = data.VRECVOU_VoucherNo;
                    obj.VRECVOU_ReceivedFrom = data.VRECVOU_ReceivedFrom;
                    obj.VRECVOU_ChequeDDNo = data.VRECVOU_ChequeDDNo;
                    obj.VRECVOU_Remarks = data.VRECVOU_Remarks;
                    obj.VRECVOU_Amount = data.VRECVOU_Amount;
                    obj.VRECVOU_ActiveFlg = true;
                    obj.VRECVOU_CreatedBy = data.UserId;
                    obj.VCBSRC_Id = data.VCBSRC_Id;
                    obj.ISMCPPD_Id = data.ISMCPPD_Id;
                    obj.VRECVOU_CreatedDate = DateTime.Now;
                    obj.VRECVOU_UpdatedBy = data.UserId;
                    obj.VRECVOU_UpdatedDate = DateTime.Now;
                    obj.VRECVOU_VoucherNo = VoucherNo.ToString();
                    obj.VRECVOU_Date = data.VRECVOU_Date;
                    _IssueManagercontext.Add(obj);        
                    if(data.VBABRS_ClearedFlg ==true && data.VBABRS_ClearedDate !=null)
                    {
                        VMS_BankAccount_BRSDMO bnkto = new VMS_BankAccount_BRSDMO();
                        bnkto.HRMBD_Id = data.HRMBD_Id;
                        bnkto.VBABRS_RefrenceId = data.VRECVOU_ReceiptReference;
                        bnkto.MI_Id = data.MI_Id;
                        bnkto.VBABRS_Date = data.VRECVOU_Date;
                        bnkto.VBABRS_Amount = data.VRECVOU_Amount;
                        bnkto.VBABRS_CRDRFlg = "CR";
                        bnkto.VBABRS_Remarks = data.VRECVOU_Remarks;
                        bnkto.VBABRS_ActualBankBalance = data.VBABRS_ActualBankBalance + data.VRECVOU_Amount;
                        bnkto.VBABRS_BalanceAsperVoucher = data.VBABRS_ActualBankBalance;
                        bnkto.VBABRS_ClearedDate = data.VBABRS_ClearedDate;
                        bnkto.VBABRS_ClearedFlg = true;
                        bnkto.VBABRS_ActiveFlg = true;
                        bnkto.VBABRS_CreatedDate = indianTime;
                        bnkto.VBABRS_UpdatedBy = data.UserId;
                        bnkto.VBABRS_UpdatedDate = indianTime;
                        bnkto.VBABRS_CreatedBy = data.UserId;
                        bnkto.ISMMCLT_Id = data.ISMMCLT_Id;
                        bnkto.VRECVOU_Id = obj.VRECVOU_Id;
                        _IssueManagercontext.Add(bnkto);
                        VMS_BANK_ACCOUNT_BALANCEDMO bnk = new VMS_BANK_ACCOUNT_BALANCEDMO();
                        bnk.VBABAL_ActualBankBalance = data.VBABRS_ActualBankBalance + data.VRECVOU_Amount;
                       // bnk.VBABAL_BalanceAsperVoucher = data.VBABRS_ActualBankBalance;
                        bnk.VBABAL_BalanceAsperVoucherReceipt= data.VBABRS_ActualBankBalance;
                        bnk.VBABAL_UpdatedBy = data.UserId;
                        bnk.VBABAL_ActiveFlg = true;
                        bnk.VBABAL_CreatedBy = data.UserId;
                        bnk.VBABAL_CreatedDate = DateTime.Now;
                        bnk.VBABAL_UpdatedDate = DateTime.Now;
                        bnk.HRMBD_Id = data.HRMBD_Id;
                        bnk.MI_Id = data.MI_Id;
                        bnk.VBABAL_CRDRFlg = "CR";
                        _IssueManagercontext.Add(bnk);
                    }
                    
                    int r = _IssueManagercontext.SaveChanges();
                    if (r > 0)
                    {
                        data.returnval = "save";
                    }
                    else
                    {
                        data.returnval = "notsave";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //onchange
        public VMS_Receipt_VoucherDTO onchange(VMS_Receipt_VoucherDTO data)
        {
            try
            {
                data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();
                data.clientlist = _IssueManagercontext.clientTable.Where(w => w.MI_Id == data.MI_Id).Distinct().ToArray();
                data.ModeOfPayment = _db.IVRM_ModeOfPaymentDMO.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_PAYMENT_Reciepent";
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CheckBookEntryDTO savedataSource(CheckBookEntryDTO data)        {            try            {                if (data.VCBSRC_Id > 0)                {                    var result = _IssueManagercontext.VMS_CheckBookSourceDMO.Where(R => R.VCBSRC_SourceName == data.VCBSRC_SourceName && R.VCBSRC_Id != data.VCBSRC_Id).ToList();                    if (result.Count > 0)                    {                        data.returnvalTwo = "exit";                        data.returnval = true;                    }                    else                    {                        var resultwo = _IssueManagercontext.VMS_CheckBookSourceDMO.Where(R => R.VCBSRC_Id == data.VCBSRC_Id).FirstOrDefault();                        if (resultwo.VCBSRC_Id > 0)                        {                            resultwo.VCBSRC_SourceName = data.VCBSRC_SourceName;                            resultwo.VCBSRC_SourceMobileNo = data.VCBSRC_SourceMobileNo;                            resultwo.VCBSRC_EmailId = data.VCBSRC_EmailId;                            resultwo.VCBSRC_UpdatedDate = DateTime.Now;                            resultwo.VCBSRC_UpdatedBy = data.UserId;                            resultwo.VCBSRC_Address = data.VCBSRC_Address;                            _IssueManagercontext.Update(resultwo);                            int cxt1 = _IssueManagercontext.SaveChanges();                            if (cxt1 > 0)                            {                                data.returnval = true;                            }                            else                            {                                data.returnval = false;                            }                        }                    }                }                else                {                    var result = _IssueManagercontext.VMS_CheckBookSourceDMO.Where(R => R.VCBSRC_SourceName == data.VCBSRC_SourceName).ToList();                    if (result.Count > 0)                    {                        data.returnvalTwo = "exit";                        data.returnval = true;                    }                    else                    {                        VMS_CheckBookSourceDMO obj = new VMS_CheckBookSourceDMO();                        obj.VCBSRC_SourceName = data.VCBSRC_SourceName;                        obj.VCBSRC_EmailId = data.VCBSRC_EmailId;                        obj.VCBSRC_SourceMobileNo = data.VCBSRC_SourceMobileNo;                        obj.VCBSRC_UpdatedBy = data.UserId;                        obj.VCBSRC_Address = data.VCBSRC_Address;                        obj.VCBSRC_UpdatedDate = DateTime.Now;                        obj.VCBSRC_UpdatedDate = DateTime.Now;                        obj.VCBSRC_CreatedBy = data.UserId;                        obj.VCBSRC_ActiveFlg = true;                        _IssueManagercontext.Add(obj);                        int cxt1 = _IssueManagercontext.SaveChanges();                        if (cxt1 > 0)                        {                            data.returnval = true;                        }                        else                        {                            data.returnval = false;                        }                    }                }



            }            catch (Exception e)            {                Console.WriteLine(e.Message);            }            return data;        }

        public CheckBookEntryDTO clientDecative(CheckBookEntryDTO data)        {            try            {                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);                var u = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(t => t.VPAYVOU_Id == data.VPAYVOU_Id).SingleOrDefault();                if (u.VPAYVOU_ActiveFlg == true)                {                    u.VPAYVOU_ActiveFlg = false;                }                else if (u.VPAYVOU_ActiveFlg == false)                {                    u.VPAYVOU_ActiveFlg = true;                }                u.VPAYVOU_UpdatedBy = data.UserId;                u.VPAYVOU_UpdatedDate = indianTime;                _IssueManagercontext.Update(u);                int o = _IssueManagercontext.SaveChanges();                if (o > 0)                {                    data.returnval = true;                }                else                {                    data.returnval = false;                }                if (data.VCBSRC_Id > 0)                {                    var resultwo = _IssueManagercontext.VMS_CheckBookSourceDMO.Where(P => P.VCBSRC_Id == data.VCBSRC_Id).FirstOrDefault();                    if (resultwo.VCBSRC_ActiveFlg == true)                    {                        resultwo.VCBSRC_ActiveFlg = false;                    }                    else                    {                        resultwo.VCBSRC_ActiveFlg = true;                    }                    resultwo.VCBSRC_UpdatedBy = data.UserId;                    resultwo.VCBSRC_UpdatedDate = DateTime.Now;                    _IssueManagercontext.Update(resultwo);                    int R = _IssueManagercontext.SaveChanges();                    if (R > 0)                    {                        data.returnval = true;                    }                    else                    {                        data.returnval = false;                    }                }            }            catch (Exception f)            {                Console.WriteLine(f.Message);            }            return data;        }
    }
}
