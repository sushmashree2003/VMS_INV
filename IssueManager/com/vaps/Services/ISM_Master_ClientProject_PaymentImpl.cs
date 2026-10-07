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
    public class ISM_Master_ClientProject_PaymentImpl : Interfaces.ISM_Master_ClientProject_PaymentInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public ISM_Master_ClientProject_PaymentImpl(IssueManagerContext hh)
        {
            _IssueManagercontext = hh;

        }
        public ISM_Master_ClientProject_PaymentDTO loaddata(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                data.clientlist = (from a in _IssueManagercontext.clientTable
                                   from b in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new ISM_Master_ClientProject_PaymentDTO
                                   {
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       ISMMCLT_Id = b.ISMMCLT_Id
                                   }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();


                List<AcademicYear> list = new List<AcademicYear>();
                list = _IssueManagercontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.master_bankdetails = _IssueManagercontext.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).ToArray();

                data.getinstallment = (from a in _IssueManagercontext.FeeInstallmentDMO
                                       from b in _IssueManagercontext.FeeInstallmentsyearlyDMO
                                       where (a.FMI_Id == b.FMI_Id && a.FMI_ActiceFlag == true && a.MI_Id == data.MI_Id && b.MI_ID == data.MI_Id)
                                       select b).Distinct().OrderBy(a => a.FTI_Name).ToArray();


                data.alldata = (from a in _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO
                                from b in _IssueManagercontext.MastersProject_DMO
                                from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                from d in _IssueManagercontext.AcademicYear
                                from e in _IssueManagercontext.clientTable
                                from f in _IssueManagercontext.FeeInstallmentsyearlyDMO
                                from g in _IssueManagercontext.FeeInstallmentDMO
                                where (a.ISMCLTPRP_Year == d.ASMAY_Id && a.ISMMCLTPR_Id == c.ISMMCLTPR_Id && e.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id
                                && f.FTI_Id == a.FTI_Id && f.FMI_Id == g.FMI_Id && b.MI_Id == data.MI_Id && e.MI_Id == data.MI_Id && g.MI_Id == data.MI_Id)
                                select new ISM_Master_ClientProject_PaymentDTO
                                {
                                    ISMCLTPRP_Id = a.ISMCLTPRP_Id,
                                    ISMCLTPRP_Year = a.ISMCLTPRP_Year,
                                    ISMCLTPRP_InstallmentName = f.FTI_Name,
                                    ISMCLTPRP_InstallmentAmt = a.ISMCLTPRP_InstallmentAmt,
                                    ISMCLTPRP_BalanceAmt = a.ISMCLTPRP_BalanceAmt,
                                    ISMCLTPRP_PaymentDate = a.ISMCLTPRP_PaymentDate,
                                    ISMMPR_Id = b.ISMMPR_Id,
                                    ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                    ASMAY_Year = d.ASMAY_Year,
                                    ISMMCLT_Id = e.ISMMCLT_Id,
                                    ISMMCLT_ClientName = e.ISMMCLT_ClientName,
                                    ISMCLTPRP_ActiveFlag = a.ISMCLTPRP_ActiveFlag,
                                    paymentstatusflag = a.ISMCLTPRP_PaymentStatus,
                                    ISMCLTPRP_ReceivedAmt = (a.ISMCLTPRP_InstallmentAmt - a.ISMCLTPRP_BalanceAmt),
                                    CreatedDate = a.CreatedDate
                                }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Master_ClientProject_PaymentDTO getProject(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                data.projectlist = (from a in _IssueManagercontext.clientTable
                                    from b in _IssueManagercontext.MastersProject_DMO
                                    from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id)
                                    select new ISM_Master_ClientProject_PaymentDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                    }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ISM_Master_ClientProject_PaymentDTO savedata(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                var cltid = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMPR_Id == data.ISMMPR_Id && t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMMCLTPR_ActiveFlag == true).ToList();

                data.cltprid = cltid[0].ISMMCLTPR_Id;

                if (data.ISMCLTPRP_Id == 0)
                {
                    var duplicate = _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO.Where(t => t.ISMMCLTPR_Id == data.cltprid
                    && t.FTI_Id == data.FTI_Id && t.ISMCLTPRP_Year == data.asmaY_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        ISM_Master_ClientProject_PaymentDMO dmo = new ISM_Master_ClientProject_PaymentDMO();

                        var cltidd = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMPR_Id == data.ISMMPR_Id
                        && t.ISMMCLT_Id == data.ISMMCLT_Id).ToList();

                        data.cltprid = cltidd[0].ISMMCLTPR_Id;

                        dmo.ISMMCLTPR_Id = data.cltprid;

                        dmo.ISMCLTPRP_InstallmentName = data.ISMCLTPRP_InstallmentName;
                        dmo.ISMCLTPRP_Year = data.asmaY_Id;
                        dmo.FTI_Id = data.FTI_Id;

                        dmo.ISMCLTPRP_InstallmentAmt = data.ISMCLTPRP_InstallmentAmt;
                        dmo.ISMCLTPRP_BalanceAmt = data.ISMCLTPRP_InstallmentAmt;
                        dmo.ISMCLTPRP_PaymentStatus = "In Progress";
                        dmo.ISMCLTPRP_PaymentDate = data.ISMCLTPRP_PaymentDate;
                        dmo.ISMCLTPRP_ActiveFlag = true;
                        dmo.ISMCLTPRP_CreatedBy = data.UserId;
                        dmo.ISMCLTPRP_UpdatedBy = data.UserId;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;

                        _IssueManagercontext.Add(dmo);

                        int y = _IssueManagercontext.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }

                else if (data.ISMCLTPRP_Id > 0)
                {
                    var duplicate1 = _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO.Where(t => t.ISMCLTPRP_InstallmentName == data.ISMCLTPRP_InstallmentName && t.ISMCLTPRP_InstallmentAmt == data.ISMCLTPRP_InstallmentAmt && t.ISMCLTPRP_PaymentDate == data.ISMCLTPRP_PaymentDate && t.ISMCLTPRP_Id != data.ISMCLTPRP_Id).ToArray();

                    var duplicate = _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO.Where(t => t.ISMMCLTPR_Id == data.cltprid
                    && t.FTI_Id == data.FTI_Id && t.ISMCLTPRP_Id != data.ISMCLTPRP_Id && t.ISMCLTPRP_Year == data.asmaY_Id && t.ISMCLTPRP_PaymentDate == data.ISMCLTPRP_PaymentDate && t.ISMCLTPRP_ActiveFlag==false ).ToArray();

                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var checkreceivedamount = _IssueManagercontext.ISM_Client_Project_Payment_DetailsDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id).ToList();
                        decimal? receivedamnt = 0.00m;
                        if (checkreceivedamount.Count > 0)
                        {
                            foreach (var c in checkreceivedamount)
                            {
                                receivedamnt += c.ISMCPPD_ReceivedAmount;
                            }
                        }

                        decimal? balanceamnt = 0.00m;
                        if (receivedamnt > 0)
                        {
                            balanceamnt = data.ISMCLTPRP_InstallmentAmt - receivedamnt;
                        }
                        else
                        {
                            balanceamnt = data.ISMCLTPRP_InstallmentAmt;
                        }

                        string paymentstatus = "";
                        decimal? balanceamt = 0;
                        decimal? excessamt = 0;

                        if (data.ISMCLTPRP_InstallmentAmt == receivedamnt)
                        {
                            paymentstatus = "Full Payment";
                            balanceamt = 0;
                            excessamt = 0;
                        }

                        else if (receivedamnt < data.ISMCLTPRP_InstallmentAmt)
                        {
                            paymentstatus = "Partial Payment";
                            balanceamt = data.ISMCLTPRP_InstallmentAmt - receivedamnt;
                            excessamt = 0;
                        }
                        else if (receivedamnt > data.ISMCLTPRP_InstallmentAmt)
                        {
                            paymentstatus = "Full Payment";
                            excessamt = receivedamnt - data.ISMCLTPRP_InstallmentAmt;
                            balanceamt = 0;
                        }

                        var dmo = _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO.Single(t => t.ISMCLTPRP_Id == data.ISMCLTPRP_Id);

                        var cltids = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMPR_Id == data.ISMMPR_Id
                        && t.ISMMCLT_Id == data.ISMMCLT_Id).ToList();

                        data.cltprid = cltids[0].ISMMCLTPR_Id;

                        dmo.ISMCLTPRP_InstallmentName = data.ISMCLTPRP_InstallmentName;
                        dmo.ISMMCLTPR_Id = data.cltprid;
                        dmo.ISMCLTPRP_Year = data.asmaY_Id;
                        dmo.ISMCLTPRP_InstallmentAmt = data.ISMCLTPRP_InstallmentAmt;
                        dmo.ISMCLTPRP_BalanceAmt = balanceamnt;
                        dmo.ISMCLTPRP_PaymentDate = data.ISMCLTPRP_PaymentDate;
                        dmo.ISMCLTPRP_UpdatedBy = data.UserId;
                        dmo.FTI_Id = data.FTI_Id;
                        dmo.UpdatedDate = DateTime.Now;

                        _IssueManagercontext.Update(dmo);

                        int r = _IssueManagercontext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
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
        public ISM_Master_ClientProject_PaymentDTO Editdata(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                var geteditclient = _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id).ToList();

                var getclient_project_id = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(a => a.ISMMCLTPR_Id == geteditclient.FirstOrDefault().ISMMCLTPR_Id).ToList();

                data.geteditclient = (from a in _IssueManagercontext.clientTable
                                      from b in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                      where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true
                                      && a.ISMMCLT_Id == getclient_project_id.FirstOrDefault().ISMMCLT_Id)
                                      select new ISM_Master_ClientProject_PaymentDTO
                                      {
                                          ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                          ISMMCLT_Id = b.ISMMCLT_Id
                                      }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();

                data.projectlist = (from a in _IssueManagercontext.clientTable
                                    from b in _IssueManagercontext.MastersProject_DMO
                                    from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == getclient_project_id.FirstOrDefault().ISMMCLT_Id)
                                    select new ISM_Master_ClientProject_PaymentDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                    }).Distinct().ToArray();

                data.geteditclientprojectid = getclient_project_id.ToArray();


                var editlist = (from a in _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO
                                where (a.ISMCLTPRP_Id == data.ISMCLTPRP_Id)
                                select new ISM_Master_ClientProject_PaymentDTO
                                {
                                    ISMCLTPRP_Id = a.ISMCLTPRP_Id,
                                    ISMMCLTPR_Id = a.ISMMCLTPR_Id,
                                    ISMCLTPRP_InstallmentName = a.ISMCLTPRP_InstallmentName,
                                    ISMCLTPRP_InstallmentAmt = a.ISMCLTPRP_InstallmentAmt,
                                    ISMCLTPRP_PaymentDate = Convert.ToDateTime(a.ISMCLTPRP_PaymentDate),
                                    ISMCLTPRP_Year = a.ISMCLTPRP_Year,
                                    FTI_Id = a.FTI_Id
                                }).Distinct().ToArray();

                data.editlist = editlist.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ISM_Master_ClientProject_PaymentDTO clientDecative(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                var u = _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO.Where(t => t.ISMCLTPRP_Id == data.ISMCLTPRP_Id).SingleOrDefault();
                if (u.ISMCLTPRP_ActiveFlag == true)
                {
                    u.ISMCLTPRP_ActiveFlag = false;
                }
                else if (u.ISMCLTPRP_ActiveFlag == false)
                {
                    u.ISMCLTPRP_ActiveFlag = true;
                }
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
        public ISM_Master_ClientProject_PaymentDTO viewdetails(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                data.paymentmodedetails = _IssueManagercontext.IVRM_ModeOfPayment.Where(a => a.MI_Id == data.MI_Id
                && a.IVRMMOD_ActiveFlag == true).Distinct().ToArray();

                data.getpaymentdetails = _IssueManagercontext.ISM_Client_Project_Payment_DetailsDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Master_ClientProject_PaymentDTO savepaymentdetailsrecord(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                string paymentstatus = "";

                var getinstallmentamt = _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id).ToList();
                decimal? installmentamt = 0;
                decimal? balanceamt = 0;
                decimal? excessamt = 0;
                //installmentamt = getinstallmentamt.FirstOrDefault().ISMCLTPRP_InstallmentAmt;
                installmentamt = data.ISMCLTPRP_InstallmentAmt;

                decimal? installmentpaidamt = 0;

                if (data.ISM_Client_Project_Payment_Details != null && data.ISM_Client_Project_Payment_Details.Length > 0)
                {
                    List<long> paymentdetailsid = new List<long>();

                    foreach (var d in data.ISM_Client_Project_Payment_Details)
                    {
                        paymentdetailsid.Add(d.ISMCPPD_Id);
                    }

                    Array getnonpaymentdetailsid = _IssueManagercontext.ISM_Client_Project_Payment_DetailsDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id
                    && !paymentdetailsid.Contains(a.ISMCPPD_Id)).ToArray();

                    foreach (var e in getnonpaymentdetailsid)
                    {
                        _IssueManagercontext.Remove(e);
                    }

                    Dictionary<long?, decimal?> amount = new Dictionary<long?, decimal?>();

                    foreach (var c in data.ISM_Client_Project_Payment_Details)
                    {
                        decimal? VBABRS_ActualBankBalance = 0.00m;

                        installmentpaidamt = installmentpaidamt + c.ISMCPPD_ReceivedAmount;

                        if (c.ISMCPPD_Id > 0)
                        {
                            var checkresult = _IssueManagercontext.ISM_Client_Project_Payment_DetailsDMO.Single(a => a.ISMCPPD_Id == c.ISMCPPD_Id);
                            checkresult.ISMCPPD_ReceivedAmount = c.ISMCPPD_ReceivedAmount;
                            checkresult.ISMCPPD_ReceivedDate = c.ISMCPPD_ReceivedDate;
                            checkresult.IVRMMOD_Id = c.IVRMMOD_Id;
                            checkresult.ISMCPPD_PaymentRefNo = c.ISMCPPD_PaymentRefNo;
                            checkresult.ISMCPPD_TDSAmount = c.ISMCPPD_TDSAmount;

                            checkresult.ISMCPPD_Remarks = c.ISMCPPD_Remarks;
                            if (c.flag == 1)
                            {
                                checkresult.ISMCPPD_ChequeDate = c.ISMCPPD_ChequeDate;
                                checkresult.HRMBD_Id = c.HRMBD_Id;
                            }
                            else
                            {
                                checkresult.ISMCPPD_ChequeDate = null;
                            }
                            checkresult.ISMCPPD_UpdatedBy = data.UserId;
                            checkresult.Updateddate = indiantime0;
                            _IssueManagercontext.Update(checkresult);
                        }

                        else
                        {
                            ISM_Client_Project_Payment_DetailsDMO iSM_Client_Project_Payment_DetailsDMO = new ISM_Client_Project_Payment_DetailsDMO();
                            iSM_Client_Project_Payment_DetailsDMO.ISMCLTPRP_Id = data.ISMCLTPRP_Id;
                            iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_ReceivedAmount = c.ISMCPPD_ReceivedAmount;
                            iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_ReceivedDate = c.ISMCPPD_ReceivedDate;
                            iSM_Client_Project_Payment_DetailsDMO.IVRMMOD_Id = c.IVRMMOD_Id;
                            iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_PaymentRefNo = c.ISMCPPD_PaymentRefNo;
                            iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_TDSAmount = c.ISMCPPD_TDSAmount;
                            iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_Remarks = c.ISMCPPD_Remarks;
                            if (c.flag == 1)
                            {
                                iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_ChequeDate = c.ISMCPPD_ChequeDate;
                                iSM_Client_Project_Payment_DetailsDMO.HRMBD_Id = c.HRMBD_Id;
                            }
                            else
                            {
                                iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_ChequeDate = null;
                            }
                            iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_CreatedBy = data.UserId;
                            iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_UpdatedBy = data.UserId;
                            iSM_Client_Project_Payment_DetailsDMO.Createddate = indiantime0;
                            iSM_Client_Project_Payment_DetailsDMO.Updateddate = indiantime0;
                            _IssueManagercontext.Add(iSM_Client_Project_Payment_DetailsDMO);

                            if (c.flag == 1)
                            {
                                var previous_amount_brs = amount.Where(a => a.Key.Value == c.HRMBD_Id).ToList();

                                if (previous_amount_brs.Count > 0)
                                {
                                    var aa = amount.Single(s => s.Key == c.HRMBD_Id).Value;
                                    VBABRS_ActualBankBalance = aa + c.ISMCPPD_ReceivedAmount;
                                    amount[c.HRMBD_Id] = VBABRS_ActualBankBalance;
                                }
                                else
                                {
                                    var projectlist = (from a in _IssueManagercontext.VMS_BankAccount_BRSDMO
                                                       where a.MI_Id == data.MI_Id && a.VBABRS_ActiveFlg == true && a.HRMBD_Id == c.HRMBD_Id
                                                       select a).OrderByDescending(d => d.VBABRS_Id).Take(1).ToList();

                                    if (projectlist.Count > 0)
                                    {
                                        VBABRS_ActualBankBalance = projectlist.FirstOrDefault().VBABRS_ActualBankBalance;
                                    }

                                    VBABRS_ActualBankBalance = c.ISMCPPD_ReceivedAmount + VBABRS_ActualBankBalance;
                                    amount.Add(c.HRMBD_Id, VBABRS_ActualBankBalance);
                                }

                                var paymentmodename = _IssueManagercontext.IVRM_ModeOfPayment.Where(a => a.IVRMMOD_Id == c.IVRMMOD_Id).ToList();

                                VMS_Receipt_VoucherDMO vMS_Receipt_VoucherDMO = new VMS_Receipt_VoucherDMO
                                {
                                    MI_Id = data.MI_Id,
                                    VRECVOU_FromClientFlg = true,
                                    ISMMCLT_Id = data.ISMMCLT_Id,
                                    HRMBD_Id = c.HRMBD_Id,
                                    VRECVOU_ReceiptMode = paymentmodename.Count > 0 ? paymentmodename.FirstOrDefault().IVRMMOD_ModeOfPayment.ToUpper() : "CASH",
                                    VRECVOU_ReceiptReference = c.ISMCPPD_PaymentRefNo,
                                    VRECVOU_VoucherNo = c.ISMCPPD_PaymentRefNo,
                                    VRECVOU_ReceivedFrom = data.ISMMCLT_ClientName,
                                    VRECVOU_ChequeDDNo = c.flag == 1 ? c.ISMCPPD_PaymentRefNo : null,
                                    VRECVOU_Remarks = c.ISMCPPD_Remarks,
                                    VRECVOU_TDSAmount = c.ISMCPPD_TDSAmount,
                                    VRECVOU_Amount = c.ISMCPPD_ReceivedAmount,
                                    VRECVOU_ActiveFlg = true,
                                    VRECVOU_CreatedBy = data.UserId,
                                    VRECVOU_CreatedDate = indiantime0,
                                    VRECVOU_UpdatedBy = data.UserId,
                                    VRECVOU_UpdatedDate = indiantime0,
                                    ISMCPPD_Id = iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_Id
                                };

                                _IssueManagercontext.Add(vMS_Receipt_VoucherDMO);

                                VMS_BankAccount_BRSDMO vMS_BankAccount_BRSDMO = new VMS_BankAccount_BRSDMO
                                {
                                    MI_Id = data.MI_Id,
                                    HRMBD_Id = c.HRMBD_Id,
                                    VBABRS_RefrenceId = c.ISMCPPD_PaymentRefNo,
                                    VBABRS_Date = c.ISMCPPD_ReceivedDate,
                                    VBABRS_Amount = c.ISMCPPD_ReceivedAmount,
                                    VBABRS_CRDRFlg = "CR",
                                    VBABRS_Remarks = c.ISMCPPD_Remarks,
                                    VBABRS_TDSAmount = c.ISMCPPD_TDSAmount,
                                    VBABRS_ClearedFlg = true,
                                    VBABRS_ActualBankBalance = VBABRS_ActualBankBalance,
                                    VBABRS_ClearedDate = c.ISMCPPD_ReceivedDate,
                                    VBABRS_ActiveFlg = true,
                                    VBABRS_CreatedBy = data.UserId,
                                    VBABRS_CreatedDate = indiantime0,
                                    VBABRS_UpdatedBy = data.UserId,
                                    VBABRS_UpdatedDate = indiantime0,
                                    ISMMCLT_Id = data.ISMMCLT_Id,
                                    VRECVOU_Id = vMS_Receipt_VoucherDMO.VRECVOU_Id
                                };
                                _IssueManagercontext.Add(vMS_BankAccount_BRSDMO);
                                VMS_BANK_ACCOUNT_BALANCEDMO bnk = new VMS_BANK_ACCOUNT_BALANCEDMO
                                {
                                    VBABAL_ActualBankBalance = VBABRS_ActualBankBalance,
                                    VBABAL_BalanceAsperVoucherReceipt = VBABRS_ActualBankBalance,
                                    VBABAL_UpdatedBy = data.UserId,
                                    VBABAL_ActiveFlg = true,
                                    VBABAL_CreatedBy = data.UserId,
                                    VBABAL_CreatedDate = DateTime.Now,
                                    VBABAL_UpdatedDate = DateTime.Now,
                                    HRMBD_Id = c.HRMBD_Id.Value,
                                    MI_Id = data.MI_Id,
                                    VBABAL_CRDRFlg = "CR"
                                };

                                _IssueManagercontext.Add(bnk);

                            }
                        }
                    }

                    if (installmentamt == installmentpaidamt)
                    {
                        paymentstatus = "Full Payment";
                        balanceamt = 0;
                        excessamt = 0;
                    }

                    else if (installmentpaidamt < installmentamt)
                    {
                        paymentstatus = "Partial Payment";
                        balanceamt = installmentamt - installmentpaidamt;
                        excessamt = 0;
                    }
                    else if (installmentpaidamt > installmentamt)
                    {
                        paymentstatus = "Full Payment";
                        excessamt = installmentpaidamt - installmentamt;
                        balanceamt = 0;
                    }

                    var checkresultpayment = _IssueManagercontext.ISM_Master_ClientProject_PaymentDMO.Single(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id);
                    checkresultpayment.ISMCLTPRP_PaymentStatus = paymentstatus;
                    checkresultpayment.ISMCLTPRP_BalanceAmt = balanceamt;
                    checkresultpayment.ISMCLTPRP_ExcessAmt = excessamt;
                    checkresultpayment.ISMCLTPRP_InstallmentAmt = data.ISMCLTPRP_InstallmentAmt;
                    checkresultpayment.UpdatedDate = indiantime0;
                    checkresultpayment.ISMCLTPRP_UpdatedBy = data.UserId;
                    _IssueManagercontext.Update(checkresultpayment);

                    var i = _IssueManagercontext.SaveChanges();

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
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment Configuration
        public ISM_Master_ClientProject_PaymentDTO loaddataconfig(ISM_Master_ClientProject_PaymentDTO data)
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
        public ISM_Master_ClientProject_PaymentDTO savedataconfig(ISM_Master_ClientProject_PaymentDTO data)
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
        public ISM_Master_ClientProject_PaymentDTO loaddatareport(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                data.clientlist = (from a in _IssueManagercontext.clientTable
                                   from b in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new ISM_Master_ClientProject_PaymentDTO
                                   {
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       ISMMCLT_Id = b.ISMMCLT_Id
                                   }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Master_ClientProject_PaymentDTO getreport(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                var geturl = _IssueManagercontext.clientTable.Where(a => a.ISMMCLT_Id == data.ISMMCLT_Id).ToList();

                if (geturl.Count > 0)
                {
                    string url = geturl.FirstOrDefault().ISMMCLT_IVRM_URL;

                    ISM_Master_ClientProject_PaymentDTO dtonew = new ISM_Master_ClientProject_PaymentDTO();

                    dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                    dtonew.fromdate = data.fromdate;
                    dtonew.todate = data.todate;
                    // url = "http://localhost:51263/";
                    if (url != null && url != "")
                    {
                        HttpClient client1 = new HttpClient();
                        client1.BaseAddress = new Uri(url);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client1.PostAsJsonAsync("api/InstitutionUserMappingFacade/getvmspaymentsubsctiptionreport", dtonew).Result;

                        string description = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            description = result;

                            dtonew = JsonConvert.DeserializeObject<ISM_Master_ClientProject_PaymentDTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                            data.getreportdetails = dtonew.getreportdetails;
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

        //payment notification
        public ISM_Master_ClientProject_PaymentDTO paymentnotification(ISM_Master_ClientProject_PaymentDTO data)
        {
            try
            {
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_GET_PAYMENT_NOTIFICATION_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRM_MI_Id", SqlDbType.VarChar) { Value = data.IVRM_MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_ClientCode", SqlDbType.VarChar) { Value = data.ISMMCLT_ClientCode });

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}