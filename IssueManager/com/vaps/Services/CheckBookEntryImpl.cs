using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class CheckBookEntryImpl : Interfaces.CheckBookEntryInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;

        public CheckBookEntryImpl(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;
            //
        }

        public CheckBookEntryDTO loaddata(CheckBookEntryDTO data)
        {
            try
            {
                data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                //  data.CheckbookSource = _IssueManagercontext.VMS_CheckBookSourceDMO.Where(R => R.VCBSRC_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CheckBookEntryDTO companychange(CheckBookEntryDTO data)
        {
            try
            {
                //IVRM_ModeOfPaymentDMO
                data.ModeOfPayment = _db.IVRM_ModeOfPaymentDMO.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();

                data.Client_Master = _IssueManagercontext.clientTable.Where(t => t.ISMMCLT_ActiveFlag == true && t.MI_Id == data.MI_Id).Distinct().ToArray();
                data.MI_logo = _IssueManagercontext.Institution.Where(R => R.MI_Id == data.MI_Id && R.MI_ActiveFlag == 1).Select(P => P.MI_Logo).Single();
                data.get_supplier = _IssueManagercontext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_Id).Distinct().ToArray();

                data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_CHECKBOOK_ENTRY";
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

                //  data.CheckBookBudget = _IssueManagercontext.VMS_Master_BudgetHeadsDMO.Where(R => R.MI_Id == data.MI_Id && R.VMBDGHD_ActiveFlg == true).Distinct().ToArray();
                data.CreatedDate = DateTime.Now;
                string paymentdate = data.CreatedDate.Value.ToString("yyyy-MM-dd");


                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chequebook_Budgetlist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@paymentdate",
                    SqlDbType.VarChar)
                    {
                        Value = paymentdate
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
                        data.CheckBookBudget = retObject.ToArray();
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
        public CheckBookEntryDTO getProject(CheckBookEntryDTO data)
        {
            try
            {
                data.projectlist = (from a in _IssueManagercontext.clientTable
                                    from b in _IssueManagercontext.MastersProject_DMO
                                    from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id)
                                    select new CheckBookEntryDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                    }).Distinct().ToArray();


                data.clientlist = _IssueManagercontext.clientTable.Where(w => w.ISMMCLT_Id == data.ISMMCLT_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBookEntryDTO getbom(CheckBookEntryDTO data)
        {
            try
            {
                List<string> VPAYVOU_ChequeNo = new List<string>();
                var ChequeNo = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.HRMBD_Id == data.HRMBD_Id && R.MI_Id == data.MI_Id && (R.VPAYVOU_StatusFlg == "Pending" || R.VPAYVOU_StatusFlg == "Approved")).ToList();
                foreach (var c in ChequeNo)
                {
                    VPAYVOU_ChequeNo.Add(c.VPAYVOU_ChequeNo);
                }
                var OpeningBalnce = _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO.Where(R => R.HRMBD_Id == data.HRMBD_Id).ToList();
                if (OpeningBalnce.Count > 0)
                {
                    data.msg = "OB";
                }
                else
                {
                    data.msg = "NotOB";
                }
                data.projectlist = (from a in _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO
                                    where a.MI_Id == data.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Id
                                    select new CheckBankBalanceDTO
                                    {
                                        VBABRS_BankBalalnce = a.VBABAL_ActualBankBalance,
                                        VBABRS_Id = a.VBABAL_Id
                                    }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
                //&& a.VMCHEBK_ActiveFlg == true && b.VMCHEBKSLN_CancelledFlg == false
                data.CheckbookSource = (from a in _IssueManagercontext.VMSMaster_ChequeBookDMO
                                        from b in _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO
                                        where a.MI_Id == data.MI_Id && a.VMCHEBK_Id == b.VMCHEBK_Id && a.HRMBD_Id == data.HRMBD_Id && !VPAYVOU_ChequeNo.Contains(b.VMCHEBK_LeafNo)
                                        select new CheckBookEntryDTO
                                        {
                                            VPAYVOU_ChequeNo = b.VMCHEBK_LeafNo,
                                            VMCHEBKSLN_Id = b.VMCHEBKSLN_Id

                                        }).OrderBy(R => R.VPAYVOU_ChequeNo).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBookEntryDTO savedata(CheckBookEntryDTO data)
        {
            try
            {

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.VPAYVOU_SupplierFlg == false)
                {
                    data.INVMS_Id = null;
                }
                if (data.VPAYVOU_Id > 0)
                {
                    var update = _IssueManagercontext.VMS_Payment_VoucherDMO.Single(e => e.VPAYVOU_Id == data.VPAYVOU_Id);
                    update.VPAYVOU_PaymentDate = data.VPAYVOU_PaymentDate;
                    update.VPAYVOU_AppliedAmount = data.VPAYVOU_AppliedAmount;
                    update.VPAYVOU_Remarks = data.VPAYVOU_Remarks;
                    update.VPAYVOU_BankBalalnce = data.VPAYVOU_BankBalalnce;
                    update.VPAYVOU_UpdatedBy = data.UserId;
                    update.VPAYVOU_UpdatedDate = indianTime;
                    update.VPAYVOU_ChequeNo = data.VPAYVOU_ChequeNo;
                    update.VPAYVOU_TDS = data.VPAYVOU_TDS;
                    update.VPAYVOU_Penalty = data.VPAYVOU_Penalty;
                    update.VPAYVOU_ToSupplierFlg = data.VPAYVOU_ToSupplierFlg;
                    update.VPAYVOU_SupplierFlg = data.VPAYVOU_SupplierFlg;
                    update.VPAYVOU_SelfPayFlg = data.VPAYVOU_SelfPayFlg;
                    update.VPAYVOU_PaymentTo = data.VPAYVOU_PaymentTo;
                    update.VPAYVOU_ESICAmount = data.VPAYVOU_ESICAmount;
                    update.VPAYVOU_OtherAmount = data.VPAYVOU_OtherAmount;
                    update.VPAYVOU_PaymentMode = data.VPAYVOU_PaymentMode;
                    update.VPAYVOU_PaymentReference = data.VPAYVOU_PaymentReference;                
                    _IssueManagercontext.Update(update);
                    if (data.VoucherGRN != null && data.VoucherGRN.Length > 0)
                    {
                        var grntwo = _IssueManagercontext.VMS_Payment_Voucher_Supplier_GRNDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                        if (grntwo.Count > 0)
                        {
                            foreach (var item in grntwo)
                            {
                                _IssueManagercontext.Remove(item);
                            }
                        }
                        for (int i = 0; i < data.VoucherGRN.Length; i++)
                        {
                            decimal? VPAYVOUSGRN_ActualTotalAmount = 0;
                            decimal? VPAYVOUSGRN_BalanceAmount = 0;
                            var  updategrn= _IssueManagercontext.VMS_Payment_Voucher_Supplier_GRNDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id
                            &&  R.INVMGRN_Id== data.VoucherGRN[i].INVMGRN_Id && R.INVMS_Id == data.VoucherGRN[i].INVMS_Id).ToList();
                            if(updategrn.Count > 0)
                            {
                                VPAYVOUSGRN_ActualTotalAmount = updategrn.FirstOrDefault().VPAYVOUSGRN_ActualTotalAmount;
                                VPAYVOUSGRN_BalanceAmount = updategrn.FirstOrDefault().VPAYVOUSGRN_ActualTotalAmount - data.VoucherGRN[i].VPAYVOUSGRN_PaidAmount;
                            }
                            else
                            {
                                VPAYVOUSGRN_ActualTotalAmount = data.VoucherGRN[i].VPAYVOUSGRN_ActualTotalAmount;
                                VPAYVOUSGRN_BalanceAmount = data.VoucherGRN[i].VPAYVOUSGRN_BalanceAmount;
                            }
                            VMS_Payment_Voucher_Supplier_GRNDMO grn = new VMS_Payment_Voucher_Supplier_GRNDMO();
                            grn.VPAYVOU_Id = data.VPAYVOU_Id;
                            grn.INVMS_Id = data.VoucherGRN[i].INVMS_Id;
                            grn.INVMGRN_Id = data.VoucherGRN[i].INVMGRN_Id;
                            grn.VPAYVOUSGRN_ActualTotalAmount = VPAYVOUSGRN_ActualTotalAmount;
                            grn.VPAYVOUSGRN_BalanceAmount = VPAYVOUSGRN_BalanceAmount;
                            grn.VPAYVOUSGRN_PaidAmount = data.VoucherGRN[i].VPAYVOUSGRN_PaidAmount;
                            grn.VPAYVOUSGRN_Remarks = data.VoucherGRN[i].VPAYVOUSGRN_Remarks;
                            grn.VPAYVOUSGRN_ActiveFlg = true;
                            grn.VPAYVOUSGRN_CreatedBy = data.UserId;
                            grn.VPAYVOUSGRN_CreatedDate = DateTime.Now;
                            grn.VPAYVOUSGRN_UpdatedBy = data.UserId;
                            grn.VPAYVOUSGRN_UpdatedDate = DateTime.Now;
                            _IssueManagercontext.Add(grn);
                        }
                    }
                    if (data.filelist != null && data.filelist.Length > 0)
                    {
                        var filelist = _IssueManagercontext.BankAccount_BRS_FilesDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                        if (filelist.Count > 0)
                        {
                            foreach (var file in filelist)
                            {
                                _IssueManagercontext.Remove(file);
                            }
                        }
                        for (int b = 0; b < data.filelist.Length; b++)
                        {
                            BankAccount_BRS_FilesDMO objfile = new BankAccount_BRS_FilesDMO();
                            objfile.VPAYVOUF_FileName = data.filelist[b].VBABRSF_FileName;
                            objfile.VPAYVOUF_FilePath = data.filelist[b].VBABRSF_FilePath;
                            objfile.VPAYVOUF_Filedesc = data.filelist[b].VBABRSF_Filedesc;
                            objfile.VPAYVOU_Id = data.VPAYVOU_Id;
                            objfile.VPAYVOUF_StatusFlg = "approved";
                            objfile.VPAYVOUF_ActiveFlg = true;
                            _IssueManagercontext.Add(objfile);
                        }
                    }
                    if (data.BudgetMapings != null && data.BudgetMapings.Length > 0)
                    {
                        var BudgetMapings = _IssueManagercontext.VMS_Payment_Voucher_BudgetDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                        if(BudgetMapings.Count > 0)
                        {
                            foreach (var Budget in BudgetMapings)
                            {
                                _IssueManagercontext.Remove(Budget);
                            }
                        }

                        foreach (var d in data.BudgetMapings)
                        {
                            VMS_Payment_Voucher_BudgetDMO objbudget = new VMS_Payment_Voucher_BudgetDMO();
                            objbudget.VPAYVOU_Id = data.VPAYVOU_Id;
                            objbudget.VMBDGHD_Id = d.VMBDGHD_Id;
                            objbudget.VMBDGHD_AmountSpent = d.VMBDGHD_AmountSpent;
                            objbudget.VPAYVOUBUD_Remark = d.VPAYVOUBUD_Remark;
                            objbudget.VPAYVOUBUD_ActiveFlg = true;
                            objbudget.VPAYVOUBUD_CreatedBy = data.UserId;
                            objbudget.VPAYVOUBUD_UpdatedBy = data.UserId;
                            objbudget.VPAYVOUBUD_CreatedDate = DateTime.Now;
                            objbudget.VPAYVOUBUD_UpdatedDate = DateTime.Now;
                            _IssueManagercontext.Add(objbudget);

                        }
                    }
                    int cxt = _IssueManagercontext.SaveChanges();
                    if (cxt > 0)
                    {
                        data.msgval = "save";
                        if (data.leafcancelD != null)
                        {
                            for (int i = 0; i < data.leafcancelD.Length; i++)
                            {
                                var upadate = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Where(R => R.VMCHEBK_LeafNo == data.leafcancelD[i].vpayvoU_ChequeNoleaf).FirstOrDefault();
                                upadate.VMCHEBKSLN_CancelledFlg = true;
                                upadate.VMCHEBKSLN_UpdatedBy = data.UserId;
                                upadate.VMCHEBKSLN_UpdatedDate = indianTime;
                                _IssueManagercontext.Update(upadate);
                                int r = _IssueManagercontext.SaveChanges();

                            }

                        }
                    }
                    else
                    {
                        data.msgval = "admin";
                    }
                }
                else
                {

                    var TransIdcount = _IssueManagercontext.VMS_Payment_VoucherDMO.Distinct().ToList();
                    if (TransIdcount.Count > 0)
                    {
                        data.trans_id = TransIdcount.Count.ToString();
                    }
                    else
                    {
                        data.trans_id = "0";
                    }
                    string bankaccont = "";
                    var banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_Id == data.HRMBD_Id).Distinct().ToList();
                    if (data.optionflag == "bnktobnk")
                    {
                        data.HRMBD_InterbankTransFlg = true;

                    }
                    else
                    {
                        data.HRMBD_InterbankTransFlg = false;
                        data.VPAYVOU_HRMBD_Id = 0;
                    }
                    if (banklist.Count > 0)
                    {
                        bankaccont = banklist.FirstOrDefault().HRMBD_BankAccountNo;
                    }
                    // data.Client_Master = null;
                    if (data.VPAYVOU_ChequeNo != null && data.VPAYVOU_ChequeNo != "")
                    {

                        data.Client_Master = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.HRMBD_Id == data.HRMBD_Id && R.VPAYVOU_ActiveFlg == true && R.VPAYVOU_ChequeNo == data.VPAYVOU_ChequeNo && R.MI_Id == data.MI_Id && (R.VPAYVOU_StatusFlg == "Pending" || R.VPAYVOU_StatusFlg == "Approved")).Distinct().ToArray();

                    }

                    if (data.Client_Master != null && data.Client_Master.Length > 0)
                    {
                        data.msgval = "duplicate";
                    }
                    else
                    {
                        VMS_Payment_VoucherDMO obj = new VMS_Payment_VoucherDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.INVMS_Id = data.INVMS_Id;
                        obj.VPAYVOU_PaymentDate = data.VPAYVOU_PaymentDate;
                        obj.VPAYVOU_AppliedAmount = data.VPAYVOU_AppliedAmount;
                        obj.VPAYVOU_ApprovedAmount = 0;
                        obj.VPAYVOU_Remarks = data.VPAYVOU_Remarks;
                        obj.VPAYVOU_BankBalalnce = data.VPAYVOU_BankBalalnce;
                        obj.VPAYVOU_StatusFlg = "Pending";
                        obj.VPAYVOU_ChequePrintedFlg = false;
                        obj.VPAYVOU_ActiveFlg = true;
                        obj.VPAYVOU_CreatedBy = data.UserId;
                        obj.VPAYVOU_CreatedDate = indianTime;
                        obj.VPAYVOU_UpdatedBy = data.UserId;
                        obj.VPAYVOU_UpdatedDate = indianTime;
                        obj.VPAYVOU_BankAccountNo = bankaccont;
                        obj.VPAYVOU_VoucherNo = data.trans_id;
                        obj.HRMBD_Id = data.HRMBD_Id;
                        obj.VPAYVOU_ChequeNo = data.VPAYVOU_ChequeNo;
                        obj.VPAYVOU_TDS = data.VPAYVOU_TDS;
                        obj.VPAYVOU_Penalty = data.VPAYVOU_Penalty;
                        obj.VPAYVOU_ToSupplierFlg = data.VPAYVOU_ToSupplierFlg;
                        obj.VPAYVOU_SupplierFlg = data.VPAYVOU_SupplierFlg;
                        obj.VPAYVOU_SelfPayFlg = data.VPAYVOU_SelfPayFlg;
                        obj.VPAYVOU_PaymentTo = data.VPAYVOU_PaymentTo;
                        obj.VPAYVOU_ESICAmount = data.VPAYVOU_ESICAmount;
                        obj.VPAYVOU_OtherAmount = data.VPAYVOU_OtherAmount;
                        obj.VPAYVOU_PaymentMode = data.VPAYVOU_PaymentMode;
                        obj.VPAYVOU_PaymentReference = data.VPAYVOU_PaymentReference;
                        obj.HRMBD_InterbankTransFlg = data.HRMBD_InterbankTransFlg;
                        obj.VPAYVOU_HRMBD_Id = data.VPAYVOU_HRMBD_Id;
                       
                        _IssueManagercontext.Add(obj);
                        if (data.VoucherGRN != null && data.VoucherGRN.Length > 0)
                        {
                            for (int i = 0; i < data.VoucherGRN.Length; i++)
                            {
                                VMS_Payment_Voucher_Supplier_GRNDMO grn = new VMS_Payment_Voucher_Supplier_GRNDMO();
                                grn.VPAYVOU_Id = obj.VPAYVOU_Id;
                                grn.INVMS_Id = data.VoucherGRN[i].INVMS_Id;
                                grn.INVMGRN_Id = data.VoucherGRN[i].INVMGRN_Id;
                                grn.VPAYVOUSGRN_ActualTotalAmount = data.VoucherGRN[i].VPAYVOUSGRN_ActualTotalAmount;
                                grn.VPAYVOUSGRN_BalanceAmount = data.VoucherGRN[i].VPAYVOUSGRN_BalanceAmount;
                                grn.VPAYVOUSGRN_PaidAmount = data.VoucherGRN[i].VPAYVOUSGRN_PaidAmount;
                                grn.VPAYVOUSGRN_Remarks = data.VoucherGRN[i].VPAYVOUSGRN_Remarks;
                                grn.VPAYVOUSGRN_ActiveFlg = true;
                                grn.VPAYVOUSGRN_CreatedBy = data.UserId;
                                grn.VPAYVOUSGRN_CreatedDate = DateTime.Now;
                                grn.VPAYVOUSGRN_UpdatedBy = data.UserId;
                                grn.VPAYVOUSGRN_UpdatedDate = DateTime.Now;
                                _IssueManagercontext.Add(grn);
                            }
                        }
                        if (data.filelist != null && data.filelist.Length >0)
                        {
                            for (int b = 0; b < data.filelist.Length; b++)
                            {
                                BankAccount_BRS_FilesDMO objfile = new BankAccount_BRS_FilesDMO();
                                objfile.VPAYVOUF_FileName = data.filelist[b].VBABRSF_FileName;
                                objfile.VPAYVOUF_FilePath = data.filelist[b].VBABRSF_FilePath;
                                objfile.VPAYVOUF_Filedesc = data.filelist[b].VBABRSF_Filedesc;
                                objfile.VPAYVOU_Id = obj.VPAYVOU_Id;
                                objfile.VPAYVOUF_StatusFlg = "approved";
                                objfile.VPAYVOUF_ActiveFlg = true;
                                _IssueManagercontext.Add(objfile);
                            }
                        }
                        if(data.BudgetMapings !=null && data.BudgetMapings.Length >0)
                        {
                            foreach(var d in data.BudgetMapings)
                            {
                                VMS_Payment_Voucher_BudgetDMO objbudget = new VMS_Payment_Voucher_BudgetDMO();
                                objbudget.VPAYVOU_Id = obj.VPAYVOU_Id;
                                objbudget.VMBDGHD_Id = d.VMBDGHD_Id;
                                objbudget.VMBDGHD_AmountSpent = d.VMBDGHD_AmountSpent;
                                objbudget.VPAYVOUBUD_Remark = d.VPAYVOUBUD_Remark;
                                objbudget.VPAYVOUBUD_ActiveFlg = true;
                                objbudget.VPAYVOUBUD_CreatedBy = data.UserId;
                                objbudget.VPAYVOUBUD_UpdatedBy = data.UserId;
                                objbudget.VPAYVOUBUD_CreatedDate = DateTime.Now;
                                objbudget.VPAYVOUBUD_UpdatedDate = DateTime.Now;
                                _IssueManagercontext.Add(objbudget);


                            }
                        }
                        int cxt = _IssueManagercontext.SaveChanges();
                        if (cxt > 0)
                        {
                            if (data.leafcancelD != null && data.leafcancelD.Length> 0)
                            {
                                for (int i = 0; i < data.leafcancelD.Length; i++)
                                {
                                    var upadate = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Where(R => R.VMCHEBK_LeafNo == data.leafcancelD[i].vpayvoU_ChequeNoleaf).FirstOrDefault();
                                    upadate.VMCHEBKSLN_CancelledFlg = true;
                                    upadate.VMCHEBKSLN_UpdatedBy = data.UserId;
                                    upadate.VMCHEBKSLN_UpdatedDate = indianTime;
                                    _IssueManagercontext.Update(upadate);
                                    int r = _IssueManagercontext.SaveChanges();
                                    if (r > 0)
                                    {
                                        CheckBookCancelMail(data, "Check Book Cancel", "Check Book Cancel", data.MI_Id, data.leafcancelD[i].vpayvoU_ChequeNoleaf);


                                    }

                                }
                            }
                            data.msgval = "save";
                            string mailcc = "";
                            string emailid = "";
                            var checkmaxorder = (from P in _IssueManagercontext.HR_PROCESSDMO
                                                 from R in _IssueManagercontext.HR_Process_Auth_OrderNoDMO
                                                 where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "CHECKBOOK"
                                                 select new CheckBookEntryDTO
                                                 {
                                                     HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                                     IVRMUL_Id = R.IVRMUL_Id
                                                 }
                                                 ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                            if (checkmaxorder.Count > 0)
                            {
                                long UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;
                                long Id = _IssueManagercontext.Staff_User_Login.Where(t => t.Id == UserId).Select(t => t.Emp_Code).FirstOrDefault();
                                mailcc = _IssueManagercontext.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            }
                            long Idtwo = _IssueManagercontext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                            emailid = _IssueManagercontext.Multiple_Email_DMO.Where(p => p.HRME_Id == Idtwo && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                            sendmail(data.MI_Id, emailid, "checkBookMail", obj.VPAYVOU_Id, mailcc);
                        }
                        else
                        {
                            data.msgval = "admin";
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                data.msgval = "admin";
            }
            return data;
        }
        public CheckBookEntryDTO savedataSource(CheckBookEntryDTO data)
        {
            try
            {

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_CHECKBOOK_GRN";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.INVMS_Id
                    });
                    //   cmd.Parameters.Add(new SqlParameter("@VoucherAmount",
                    //SqlDbType.Decimal)
                    //   {
                    //       Value = amount
                    //   });
                    //@
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
                        data.getpaymentnotificationdetails = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBookEntryDTO getinvoiceno(CheckBookEntryDTO data)
        {
            try
            {
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_CHECKBOOK_GR_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.INVMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@VoucherAmount",
                 SqlDbType.Decimal)
                    {
                        Value = data.ISMINC_AdvPer
                    });
                    //@
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
                        data.grnDetails = retObject.ToArray();
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
        public CheckBookEntryDTO Editdata(CheckBookEntryDTO data)
        {
            try
            {
                if (data.VPAYVOU_Id > 0)
                {
                    var list = _IssueManagercontext.VMS_Payment_Voucher_AuthorisationDMO.Where(p => p.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                    if (list.Count > 0)
                    {
                        data.ISMMCLT_ClientName = "auturization";
                    }
                    else
                    {
                        var editlist = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(w => w.VPAYVOU_Id == data.VPAYVOU_Id).Distinct().ToList();

                        data.editlist = editlist.ToArray();


                        data.clientlist = _IssueManagercontext.INV_Master_SupplierDMO.Where(d => d.INVMS_Id == editlist.FirstOrDefault().INVMS_Id).ToArray();
                        data.getFilesArray = _IssueManagercontext.BankAccount_BRS_FilesDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).Distinct().ToArray();

                        data.geteditclient = (from a in _IssueManagercontext.INV_Master_SupplierDMO
                                              from b in _IssueManagercontext.VMS_Payment_VoucherDMO
                                              where (a.INVMS_Id == b.INVMS_Id && a.MI_Id == data.MI_Id
                                              && a.INVMS_Id == editlist.FirstOrDefault().INVMS_Id && b.VPAYVOU_Id == data.VPAYVOU_Id)
                                              select new CheckBookEntryDTO
                                              {
                                                  INVMS_SupplierName = a.INVMS_SupplierName,
                                                  INVMS_Id = b.INVMS_Id
                                              }).Distinct().OrderBy(a => a.INVMS_SupplierName).ToArray();
                        List<string> VPAYVOU_ChequeNo = new List<string>();
                        var ChequeNo = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.HRMBD_Id == editlist.FirstOrDefault().HRMBD_Id && R.MI_Id == editlist.FirstOrDefault().MI_Id).ToList();
                        if (ChequeNo.Count > 0)
                        {
                            foreach (var c in ChequeNo)
                            {
                                if (c.VPAYVOU_ChequeNo != editlist.FirstOrDefault().VPAYVOU_ChequeNo)
                                {
                                    VPAYVOU_ChequeNo.Add(c.VPAYVOU_ChequeNo);
                                }

                            }
                        }

                        data.CheckbookSource = (from a in _IssueManagercontext.VMSMaster_ChequeBookDMO
                                                from b in _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO
                                                where a.MI_Id == data.MI_Id && a.VMCHEBK_Id == b.VMCHEBK_Id && a.HRMBD_Id == editlist.FirstOrDefault().HRMBD_Id && !VPAYVOU_ChequeNo.Contains(b.VMCHEBK_LeafNo) && a.VMCHEBK_ActiveFlg == true && b.VMCHEBKSLN_CancelledFlg == false
                                                select new CheckBookEntryDTO
                                                {
                                                    VPAYVOU_ChequeNo = b.VMCHEBK_LeafNo,
                                                    VMCHEBKSLN_Id = b.VMCHEBKSLN_Id

                                                }).OrderBy(R => R.VMCHEBKSLN_Id).ToArray();

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
                                data.grnDetails = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        data.editbanklist = (from a in _IssueManagercontext.VMS_Payment_VoucherDMO
                                             from b in _IssueManagercontext.HR_Master_BankDeatils
                                             where a.HRMBD_Id == b.HRMBD_Id && a.VPAYVOU_Id == data.VPAYVOU_Id
                                             select new CheckBookEntryDTO
                                             {
                                                 HRMBD_Id = a.HRMBD_Id,
                                                 HRMBD_BankName = b.HRMBD_BankName,
                                                 HRMBD_BankAccountNo = b.HRMBD_BankAccountNo
                                                 //hrmbD_BankAccountNo
                                                 // VPAYVOU_BankAccountNo = b.HRMBD_BankAccountNo

                                             }

                     ).Distinct().OrderByDescending(d => d.HRMBD_Id).ToArray();

                        data.ModeOfPayment = _db.IVRM_ModeOfPaymentDMO.Where(R => R.MI_Id == editlist.FirstOrDefault().MI_Id && R.IVRMMOD_ModeOfPayment != "Cash").Distinct().ToArray();

                        data.CheckBookBudget = _IssueManagercontext.VMS_Payment_Voucher_BudgetDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).Distinct().ToArray();
                        //editdata
                        if(editlist.FirstOrDefault().VPAYVOU_SupplierFlg==true)
                        {
                            using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "VOUCHER_GRN_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                                  SqlDbType.VarChar)
                                {
                                    Value = editlist.FirstOrDefault().INVMS_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@Flag",
                                  SqlDbType.VarChar)
                                {
                                    Value = "1"
                                });
                                //IMFY_Id
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
                        

                    }
                }


                // editbanklist









            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CheckBookEntryDTO clientDecative(CheckBookEntryDTO data)
        {
            try
            {
                if (data.VPAYVOU_Id > 0)
                {
                    var list = _IssueManagercontext.VMS_Payment_Voucher_AuthorisationDMO.Where(p => p.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                    if (list.Count > 0)
                    {
                        data.ISMMCLT_ClientName = "auturization";
                    }
                    else
                    {
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        var u = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(t => t.VPAYVOU_Id == data.VPAYVOU_Id).SingleOrDefault();
                        if (u.VPAYVOU_ActiveFlg == true)
                        {
                            u.VPAYVOU_ActiveFlg = false;
                        }
                        else if (u.VPAYVOU_ActiveFlg == false)
                        {
                            u.VPAYVOU_ActiveFlg = true;
                        }

                        u.VPAYVOU_UpdatedBy = data.UserId;
                        u.VPAYVOU_UpdatedDate = indianTime;
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
                }



            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public CheckBookEntryDTO viewdetails(CheckBookEntryDTO data)
        {
            try
            {
                data.getFilesArray = _IssueManagercontext.BankAccount_BRS_FilesDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CheckBookEntryDTO sms(CheckBookEntryDTO data)
        {
            try
            {
                if (data.filelist != null)
                {
                    var filelist = _IssueManagercontext.BankAccount_BRS_FilesDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                    if (filelist.Count > 0)
                    {
                        foreach (var file in filelist)
                        {
                            _IssueManagercontext.Remove(file);
                        }
                    }
                    for (int b = 0; b < data.filelist.Length; b++)
                    {
                        BankAccount_BRS_FilesDMO objfile = new BankAccount_BRS_FilesDMO();
                        objfile.VPAYVOUF_FileName = data.filelist[b].VBABRSF_FileName;
                        objfile.VPAYVOUF_FilePath = data.filelist[b].VBABRSF_FilePath;
                        objfile.VPAYVOUF_Filedesc = data.filelist[b].VBABRSF_Filedesc;
                        objfile.VPAYVOU_Id = data.VPAYVOU_Id;
                        objfile.VPAYVOUF_StatusFlg = "approved";
                        objfile.VPAYVOUF_ActiveFlg = true;
                        _IssueManagercontext.Add(objfile);
                    }
                }
                int cxt = _IssueManagercontext.SaveChanges();
                if (cxt > 0)
                {
                    data.msgval = "save";

                }
                else
                {
                    data.msgval = "notsave";
                }
            }
            catch (Exception ex)
            {
                data.msgval = "admin";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment Configuration
        public CheckBookEntryDTO loaddataconfig(CheckBookEntryDTO data)
        {
            try
            {
                data.getconfigloaddata = _IssueManagercontext.ISM_Client_Payment_ConfigurationDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CheckBookEntryDTO savedataconfig(CheckBookEntryDTO data)
        {
            try
            {
                if(data.VPAYVOU_Id > 0 && data.BudgetMapings != null)
                {
                    var BudgetMapings = _IssueManagercontext.VMS_Payment_Voucher_BudgetDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                    if (BudgetMapings.Count > 0)
                    {
                        foreach (var Budget in BudgetMapings)
                        {
                            _IssueManagercontext.Remove(Budget);
                        }
                    }

                    foreach (var d in data.BudgetMapings)
                    {
                        VMS_Payment_Voucher_BudgetDMO objbudget = new VMS_Payment_Voucher_BudgetDMO();
                        objbudget.VPAYVOU_Id = data.VPAYVOU_Id;
                        objbudget.VMBDGHD_Id = d.VMBDGHD_Id;
                        objbudget.VMBDGHD_AmountSpent = d.VMBDGHD_AmountSpent;
                        objbudget.VPAYVOUBUD_Remark = d.VPAYVOUBUD_Remark;
                        objbudget.VPAYVOUBUD_ActiveFlg = true;
                        objbudget.VPAYVOUBUD_CreatedBy = data.UserId;
                        objbudget.VPAYVOUBUD_UpdatedBy = data.UserId;
                        objbudget.VPAYVOUBUD_CreatedDate = DateTime.Now;
                        objbudget.VPAYVOUBUD_UpdatedDate = DateTime.Now;
                        _IssueManagercontext.Add(objbudget);

                    }


                    int i = _IssueManagercontext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "save";
                    }
                    else
                    {
                        data.msg = "notsave";
                    }

                }
                else
                {
                    data.msg = "admin";
                }
            }
            catch (Exception ex)
            {
                data.msg = "admin";
               
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment subscription audit report
        public CheckBookEntryDTO loaddatareport(CheckBookEntryDTO data)
        {
            try
            {
               //loaddatareport
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VOUCHER_GRN_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.INVMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                    //IMFY_Id
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
        public CheckBookEntryDTO getreport(CheckBookEntryDTO data)
        {
            try
            {
                if (data.message == "ViewApproval")
                {
                    string VMBDGHD_Id = "0";
                    data.IMFY_Id =0;
                    var list = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                    if(list.Count > 0)
                    {
                        var BudgetDetalis = _IssueManagercontext.VMS_Payment_Voucher_BudgetDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).ToList();
                        if(BudgetDetalis !=null && BudgetDetalis.Count > 0)
                        {
                            foreach(var i in BudgetDetalis)
                            {
                                VMBDGHD_Id = VMBDGHD_Id + ',' + i.VMBDGHD_Id;
                            }
                            
                          data.IMFY_Id = _db.IVRM_Master_FinancialYear.Single(a => a.IMFY_FromDate <= list.FirstOrDefault().VPAYVOU_PaymentDate && a.IMFY_ToDate >= list.FirstOrDefault().VPAYVOU_PaymentDate).IMFY_Id;                                                        
                        }
                        data.MI_Id = list.FirstOrDefault().MI_Id;
                        using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "CHECKBOOK_Budget_DetailsApproval";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                              SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@VMBDGHD_Id",
                           SqlDbType.VarChar)
                            {
                                Value = VMBDGHD_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@IMFY_Id",
                             SqlDbType.VarChar)
                            {
                                Value = data.IMFY_Id
                            });
                            //IMFY_Id
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
                    
                }
                else
                {
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CHECKBOOK_Budget_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@VMBDGHD_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.VMBDGHD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@IMFY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.IMFY_Id
                        });
                        //IMFY_Id
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
              

                data.MI_logo = _IssueManagercontext.Institution.Where(R => R.MI_Id == data.MI_Id).Select(R => R.MI_Logo).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //payment notification
        public CheckBookEntryDTO paymentnotification(CheckBookEntryDTO data)
        {
            try
            {
                if (data.VPAYVOU_Id > 0)
                {
                    //data.MI_Id=_IssueManagercontext.VMS_Payment_VoucherDMO.Where(R=>R.)
                    data.MI_Id = _IssueManagercontext.VMS_Payment_VoucherDMO.Single(a => a.VPAYVOU_Id == data.VPAYVOU_Id).MI_Id;
                    data.VPAYVOU_PaymentDate= _IssueManagercontext.VMS_Payment_VoucherDMO.Single(a => a.VPAYVOU_Id == data.VPAYVOU_Id).VPAYVOU_PaymentDate;
                }
                string paymentdate = data.VPAYVOU_PaymentDate.ToString("yyyy-MM-dd");
                if (data.msg == "Fiancial")
                {

                   
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Chequebook_Budgetlist";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@paymentdate",
                        SqlDbType.VarChar)
                        {
                            Value = paymentdate
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
                            data.CheckBookBudget = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                    data.getconfigloaddata = _IssueManagercontext.VMS_Payment_Voucher_BudgetDMO.Where(R => R.VPAYVOU_Id == data.VPAYVOU_Id).Distinct().ToArray();
                }
                else
                {
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Chequebook_BudgetAmount_Paymentamount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@VMBDGHD_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.VMBDGHD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@paymentdate",
                        SqlDbType.VarChar)
                        {
                            Value = paymentdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                        SqlDbType.BigInt)
                        {
                            Value = data.VPAYVOU_AppliedAmount
                        });
                        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.VarChar, Int32.MaxValue)
                        { Direction = ParameterDirection.Output });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        data.message = cmd.Parameters["@Result"].Value.ToString();
                    }
                }
                //paymentnotification

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = ex.Message;
            }
            return data;
        }
        //savedataEMI
        public CheckBookEntryDTO savedataEMI(CheckBookEntryDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.VPAYVOU_SupplierFlg == false)
                {
                    data.INVMS_Id = null;
                }
                var TransIdcount = _IssueManagercontext.VMS_Payment_VoucherDMO.Distinct().ToList();
                if (TransIdcount.Count > 0)
                {
                    data.trans_id = TransIdcount.Count.ToString();
                }
                else
                {
                    data.trans_id = "0";
                }
                string bankaccont = "";
                var banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_Id == data.HRMBD_Id).Distinct().ToList();

                if (banklist.Count > 0)
                {
                    bankaccont = banklist.FirstOrDefault().HRMBD_BankAccountNo;
                }



                if (data.Client_Master != null && data.Client_Master.Length > 0)
                {

                }
                else
                {
                    VMS_Payment_VoucherDMO obj = new VMS_Payment_VoucherDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.VPAYVOU_PaymentDate = data.VPAYVOU_PaymentDate;
                    obj.VPAYVOU_AppliedAmount = data.VPAYVOU_AppliedAmount;
                    obj.VPAYVOU_ApprovedAmount = data.VPAYVOU_AppliedAmount;
                    obj.VPAYVOU_Remarks = data.VPAYVOU_Remarks;
                    obj.VPAYVOU_BankBalalnce = data.VPAYVOU_BankBalalnce;
                    obj.VPAYVOU_StatusFlg = "Approved";
                    obj.VPAYVOU_ChequePrintedFlg = false;
                    obj.VPAYVOU_ActiveFlg = true;
                    obj.VPAYVOU_CreatedBy = data.UserId;
                    obj.VPAYVOU_CreatedDate = indianTime;
                    obj.VPAYVOU_UpdatedBy = data.UserId;
                    obj.VPAYVOU_UpdatedDate = indianTime;
                    obj.VPAYVOU_BankAccountNo = bankaccont;
                    obj.VPAYVOU_VoucherNo = data.trans_id;
                    obj.HRMBD_Id = data.HRMBD_Id;
                    obj.VPAYVOU_ToSupplierFlg = data.VPAYVOU_ToSupplierFlg;
                    obj.VPAYVOU_SupplierFlg = false;
                    obj.VPAYVOU_SelfPayFlg = false;
                    obj.VPAYVOU_PaymentTo = data.VPAYVOU_PaymentTo;
                    obj.VPAYVOU_PaymentMode = data.VPAYVOU_PaymentMode;
                    obj.VPAYVOU_PaymentReference = data.VPAYVOU_PaymentReference;
                    obj.VPAYVOU_ESICAmount = data.VPAYVOU_ESICAmount;
                    obj.VPAYVOU_OtherAmount = data.VPAYVOU_OtherAmount;
                    obj.HRMBD_InterbankTransFlg = false;
                    obj.VPAYVOU_TDS = data.VPAYVOU_TDS;
                    obj.VPAYVOU_Penalty = data.VPAYVOU_Penalty;
                    _IssueManagercontext.Add(obj);

                    if (data.filelist != null)
                    {
                        for (int b = 0; b < data.filelist.Length; b++)
                        {
                            BankAccount_BRS_FilesDMO objfile = new BankAccount_BRS_FilesDMO();
                            objfile.VPAYVOUF_FileName = data.filelist[b].VBABRSF_FileName;
                            objfile.VPAYVOUF_FilePath = data.filelist[b].VBABRSF_FilePath;
                            objfile.VPAYVOUF_Filedesc = data.filelist[b].VBABRSF_Filedesc;
                            objfile.VPAYVOU_Id = obj.VPAYVOU_Id;
                            objfile.VPAYVOUF_StatusFlg = "approved";
                            objfile.VPAYVOUF_ActiveFlg = true;
                            _IssueManagercontext.Add(objfile);
                        }
                    }

                    int cxt = _IssueManagercontext.SaveChanges();
                    if (cxt > 0)
                    {

                        var indentres = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(w => w.MI_Id == data.MI_Id && w.HRMBD_Id == data.HRMBD_Id).OrderByDescending(d => d.VPAYVOU_Id).Take(1).FirstOrDefault();
                        var listprj = (from a in _IssueManagercontext.VMS_BANK_ACCOUNT_BALANCEDMO
                                       where a.MI_Id == data.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == data.HRMBD_Id
                                       select new CheckBankBalanceDTO
                                       {
                                           VBABRS_BankBalalnce = a.VBABAL_ActualBankBalance,
                                           VBABRS_Id = a.VBABAL_Id
                                       }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
                        var VBABRS_RefrenceId = "";
                        if (indentres.VPAYVOU_ChequeNo != "")
                        {
                            VBABRS_RefrenceId = indentres.VPAYVOU_ChequeNo;
                        }
                        else if (indentres.VPAYVOU_PaymentReference != "")
                        {
                            VBABRS_RefrenceId = indentres.VPAYVOU_PaymentReference;
                        }
                        VMS_BankAccount_BRSDMO obj1 = new VMS_BankAccount_BRSDMO();
                        obj1.VBABRS_RefrenceId = VBABRS_RefrenceId;
                        obj1.MI_Id = data.MI_Id;
                        obj1.HRMBD_Id = data.HRMBD_Id;
                        obj1.VBABRS_Date = data.VPAYVOU_PaymentDate;
                        obj1.VBABRS_Amount = data.VPAYVOU_AppliedAmount;
                        obj1.VBABRS_CRDRFlg = "DR";
                        obj1.VBABRS_Remarks = data.VPAYVOU_Remarks;
                        obj1.VBABRS_ActualBankBalance = listprj.FirstOrDefault().VBABRS_BankBalalnce - data.VPAYVOU_AppliedAmount;
                        obj1.VBABRS_ActiveFlg = true;
                        obj1.VBABRS_CreatedBy = data.UserId;
                        obj1.VBABRS_CreatedDate = DateTime.Now;
                        obj1.VBABRS_UpdatedBy = data.UserId;
                        obj1.VBABRS_UpdatedDate = DateTime.Now;
                        obj1.VPAYVOU_Id = indentres.VPAYVOU_Id;
                        obj1.VBABRS_BalanceAsperVoucher = data.VPAYVOU_BankBalalnce;
                        obj1.VBABRS_ClearedFlg = true;
                        obj1.VBABRS_ClearedDate = data.VPAYVOU_PaymentDate;
                        _IssueManagercontext.Add(obj1);
                        //added another bank 
                        VMS_BANK_ACCOUNT_BALANCEDMO bnk = new VMS_BANK_ACCOUNT_BALANCEDMO();
                        bnk.VBABAL_ActualBankBalance = listprj.FirstOrDefault().VBABRS_BankBalalnce - data.VPAYVOU_AppliedAmount;
                        bnk.VBABAL_BalanceAsperVoucher = data.VPAYVOU_BankBalalnce;
                        bnk.VBABAL_UpdatedBy = data.UserId;
                        bnk.VBABAL_ActiveFlg = true;
                        bnk.VBABAL_CreatedBy = data.UserId;
                        bnk.VBABAL_CreatedDate = DateTime.Now;
                        bnk.VBABAL_UpdatedDate = DateTime.Now;
                        bnk.HRMBD_Id = data.HRMBD_Id;
                        bnk.MI_Id = data.MI_Id;
                        bnk.VBABAL_CRDRFlg = "DR";
                        _IssueManagercontext.Add(bnk);
                        int cxtpp = _IssueManagercontext.SaveChanges();
                        data.msgval = "save";
                        string mailcc = "";
                        string emailid = "";
                        var checkmaxorder = (from P in _IssueManagercontext.HR_PROCESSDMO
                                             from R in _IssueManagercontext.HR_Process_Auth_OrderNoDMO
                                             where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "CHECKBOOK"
                                             select new CheckBookEntryDTO
                                             {
                                                 HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                                 IVRMUL_Id = R.IVRMUL_Id
                                             }
                                             ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                        if (checkmaxorder.Count > 0)
                        {
                            long UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;
                            long Id = _IssueManagercontext.Staff_User_Login.Where(t => t.Id == UserId).Select(t => t.Emp_Code).FirstOrDefault();
                            mailcc = _IssueManagercontext.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        }
                        long Idtwo = _IssueManagercontext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                        emailid = _IssueManagercontext.Multiple_Email_DMO.Where(p => p.HRME_Id == Idtwo && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                        sendmail(data.MI_Id, emailid, "checkBookMail", obj.VPAYVOU_Id, mailcc);
                    }
                    else
                    {
                        data.msgval = "admin";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //mail
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

                        cmd.CommandText = "SMSMAILCHECKBOOK";
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
                    string mailccc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    var message = new SendGridMessage();
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        string mailbcc = "";

                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);

                        if (mailccc != null && mailccc != "")
                        {
                            string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                            if (mail_id.Length > 0)
                            {
                                for (int i = 0; i < mail_id.Length; i++)
                                {
                                    message.AddBcc(mail_id[i]);
                                }
                            }
                        }
                        //if (mailcc != null && mailcc != "")
                        //{
                        //    message.AddCc(mailcc);
                        //}
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
                                //if (mailccc != null && mailccc != "")
                                //{
                                //    string[] mail_id = template[0].ISES_MailCCId.Split(',');

                                //    if (mail_id.Length > 0)
                                //    {
                                //        for (int i = 0; i < mail_id.Length; i++)
                                //        {
                                //            message.AddBcc(mail_id[i]);
                                //        }
                                //    }
                                //    emailMessage.CC.Add(mailccc);
                                //}


                                if (mailccc != null && mailccc != "")
                                {
                                    string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                                    if (mail_id.Length > 0)
                                    {
                                        for (int i = 0; i < mail_id.Length; i++)
                                        {
                                            message.AddBcc(mail_id[i]);
                                        }
                                    }
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
        //CheckBookCancelMail
        public void CheckBookCancelMail(CheckBookEntryDTO obj, string subject, string body, long id, string VMCHEBK_LeafNo)
        {
            try
            {
                long Id = _IssueManagercontext.Staff_User_Login.Where(t => t.Id == obj.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                string mailid = _IssueManagercontext.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                var cretename = (from a in _IssueManagercontext.Staff_User_Login
                                 from c in _IssueManagercontext.MasterEmployee
                                 where (a.Id == obj.UserId && a.Emp_Code == c.HRME_Id)
                                 select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();


                var VMCHEBK_LeafNoone = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Where(t => t.VMCHEBK_LeafNo == VMCHEBK_LeafNo).FirstOrDefault();
                // string mailid = "kanavisanjeev@gmail.com";

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _IssueManagercontext.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("checkbookcancel", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _IssueManagercontext.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueManagercontext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = template[0].ISES_MailCCId;

                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] mail_id = template[0].ISES_MailCCId.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddTo(mail_id[i]);
                            }
                        }
                    }
                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                    {
                        string[] mail_id = template[0].ISES_MailBCCId.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }
                    if (mailid != null)
                    {
                        message.AddCc(mailid);
                    }
                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", cretename.HRME_EmployeeFirstName + " " + (cretename.HRME_EmployeeMiddleName == null ? "" : cretename.HRME_EmployeeMiddleName) + " " + (cretename.HRME_EmployeeLastName == null ? "" : cretename.HRME_EmployeeLastName));
                        message.HtmlContent = message.HtmlContent.Replace("[USR]", VMCHEBK_LeafNoone.VMCHEBK_LeafNo);
                        message.HtmlContent = message.HtmlContent.Replace("[STATUS]", VMCHEBK_LeafNoone.VMCHEBKSLN_Remarks);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueManagercontext.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("checkbookcancel", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueManagercontext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public CheckBookEntryDTO viewuploadflies(CheckBookEntryDTO data)
        {
            try
            {
                if(data.ISMMCLT_ClientName=="Cheque")
                {
                    data.allcompany = (from a in _IssueManagercontext.VMS_ChequePrinting_SettingDMO
                                       from b in _IssueManagercontext.HR_Master_BankDeatils
                                       where (a.HRMBD_Id == b.HRMBD_Id && b.HRMBD_Id == data.HRMBD_Id && b.MI_Id == data.MI_Id)
                                       select new VMS_ChequePrinting_SettingDTO
                                       {
                                           VCPSET_Id = a.VCPSET_Id,
                                           HRMBD_Id = b.HRMBD_Id,
                                           VCPSET_PrintingTemplateId = a.VCPSET_PrintingTemplateId,
                                           VCPSET_ChequeImageFileName = a.VCPSET_ChequeImageFileName,
                                           VCPSET_ChequeImageFilePath = a.VCPSET_ChequeImageFilePath,
                                           VCPSET_Remarks = a.VCPSET_Remarks,
                                           VPAYVOU_Id = data.VPAYVOU_Id,
                                           MI_Id=b.MI_Id
                                       }
                                  ).Distinct().ToArray();
                }
                else
                {
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CheckBook_RTGS_Format";
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
                            data.editfiles = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
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
        public CheckBookEntryDTO geterifyEmail(CheckBookEntryDTO data)        {            try            {                var getemailid = _db.ApplicationUser.Where(a => a.Id == data.UserId).ToList();                Email Email = new Email(_db);                data.EMAIL_Id = getemailid.FirstOrDefault().Email;                if (data.EMAIL_Id != null && data.EMAIL_Id != "")                {                    string s = Email.sendmail_checkbox(data.MI_Id, getemailid.FirstOrDefault().Email, "EMAILOTP", Convert.ToInt64(data.EMAILOTP));                }                data.message = "Please Check,OTP Is Sent To Your Email Id";                return data;            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }
    }
}