using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using DomainModel.Model.com.vapstech.Purchase.Inventory;
using System.Text.RegularExpressions;
using DomainModel.Model;
using SendGrid.Helpers.Mail;
using System.Net;
using SendGrid;
using System.Net.Mail;
using System.IO;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_PurchaseOrderImpl : Interface.INV_PurchaseOrderInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PurchaseOrderImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_PurchaseOrderImpl(InventoryContext InvContext, ILogger<INV_PurchaseOrderImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_PurchaseOrderDTO getloaddata(INV_PurchaseOrderDTO data)
        {
            try
            {
                data.get_quotationno = _INVContext.INV_M_SupplierQuotationDMO.Where(s => s.MI_Id == data.MI_Id && s.INVMSQ_ActiveFlg == true).Distinct().OrderBy(o => o.INVMSQ_Id).ToArray();

                data.get_comparequotationno = _INVContext.INV_M_SupplierQuotationDMO.Where(s => s.MI_Id == data.MI_Id && s.INVMSQ_ActiveFlg == true && s.INVMSQ_FinaliseFlg == true).Distinct().OrderBy(o => o.INVMSQ_Id).ToArray();

                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_Id).Distinct().ToArray();
                data.get_PI = _INVContext.INV_M_PurchaseIndentDMO.Where(p => p.MI_Id == data.MI_Id && p.INVMPI_ActiveFlg == true && p.INVMPI_FinalProcessFlag==true && (p.INVMPI_RejectFlg==null || p.INVMPI_RejectFlg == false)  && p.INVMPI_POCreatedFlg==false).OrderBy(o => o.INVMPI_Id).Distinct().OrderByDescending(r=>r.INVMPI_Id).ToArray();

                //data.get_purchaseorder = _INVContext.INV_M_PurchaseOrderDMO.Where(s => s.MI_Id == data.MI_Id).Distinct().OrderByDescending(o => o.INVMPO_Id).ToArray();

                //data.get_purchaseorder = _INVContext.INV_M_PurchaseOrderDMO.Where(s => s.MI_Id == data.MI_Id).Distinct().OrderByDescending(o => o.INVMPO_Id).ToArray();


                data.get_purchaseorder = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                          from b in _INVContext.INV_T_PurchaseOrderDMO
                                          from c in _INVContext.INV_T_PurchaseIndentDMO
                                          from d in _INVContext.INV_M_PurchaseRequisitionDMO
                                          from e in _INVContext.MasterEmployee
                                          from g in _INVContext.HR_Master_Department
                                          where a.MI_Id == data.MI_Id && a.INVMPO_Id == b.INVMPO_Id && c.INVMPI_Id == b.INVMPI_Id && d.HRME_Id == e.HRME_Id && g.HRMD_Id == e.HRMD_Id && d.INVMPR_Id == c.INVMPR_Id
                                          select new INV_PurchaseOrderDTO
                                          {
                                              INVMPO_PONo = a.INVMPO_PONo,
                                              INVMPO_PODate = a.INVMPO_PODate,
                                              INVMPO_ReferenceNo = a.INVMPO_ReferenceNo,
                                              employeename = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : " " + e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),                                              
                                              departmentname = g.HRMD_DepartmentName,
                                              INVMPO_TotRate = a.INVMPO_TotRate,
                                              INVMPO_TotTax = a.INVMPO_TotTax,
                                              INVMPO_TotAmount = a.INVMPO_TotAmount,
                                              INVMPO_Remarks = a.INVMPO_Remarks,
                                              INVMPO_POTemplate= a.INVMPO_POTemplate,
                                              INVMPO_ActiveFlg=a.INVMPO_ActiveFlg,
                                              INVMPO_Id= a.INVMPO_Id,
                                              INVMS_Id=a.INVMS_Id,
                                              INVMPO_Status=a.INVMPO_Status

                                          }).Distinct().OrderByDescending(o => o.INVMPO_Id).ToArray();


                data.templatelist = (from a in _INVContext.SMSEmailSetting
                                     from b in _INVContext.Institution_Module_Page
                                     from c in _INVContext.MasterPage
                                     where a.MI_Id == data.MI_Id && a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMP_Id == c.IVRMP_Id && c.IVRMP_PageURL == "app.INV_PurchaseOrder" && a.ISES_MailActiveFlag == true
                                     select new INV_PurchaseOrderDTO
                                     {
                                         ISES_Id = a.ISES_Id,
                                         ISES_Template_Name = a.ISES_Template_Name,
                                     }).Distinct().ToArray();
                data.get_mi_list = _INVContext.Institution.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Order load Page:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseOrderDTO getqtDetail(INV_PurchaseOrderDTO data)
        {
            try
            {
                List<INV_PurchaseOrderDTO> qtdetails = new List<INV_PurchaseOrderDTO>();

                if (data.Selectionflag == "P")
                {
                    data.pidetails = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                      from b in _INVContext.INV_T_PurchaseIndentDMO
                                      from c in _INVContext.INV_Master_ItemDMO
                                      from d in _INVContext.INV_Master_UOMDMO
                                      where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                               && a.INVMPI_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPI_Id == data.INVMPI_Id && (b.INVTPI_RejectFlg==false || b.INVTPI_RejectFlg == null))
                                      select new INV_PurchaseOrderDTO
                                      {
                                          INVMPI_Id = a.INVMPI_Id,
                                          INVTPI_Id = b.INVTPI_Id,
                                          INVMI_Id = b.INVMI_Id,
                                          INVMUOM_Id = b.INVMUOM_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                                          INVTPI_PIQty = b.INVTPI_PIQty,
                                          INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                          INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                          INVTPI_ApproveQty = b.INVTPI_ApproveQty,
                                          INVTPO_POQty = Convert.ToDecimal(b.INVTPI_ApproveQty)
                                      }).Distinct().ToArray();
                }
                else
                {
                    if (data.quotationflag == "0")
                    {
                        qtdetails = (from a in _INVContext.INV_M_SupplierQuotationDMO
                                     from b in _INVContext.INV_T_SupplierQuotationDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO
                                     where (a.INVMSQ_Id == b.INVMSQ_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                              && a.INVMSQ_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMSQ_Id == data.INVMSQ_Id)
                                     select new INV_PurchaseOrderDTO
                                     {
                                         INVMPI_Id = a.INVMPI_Id,
                                         INVTSQ_Id = b.INVTSQ_Id,
                                         INVMI_Id = b.INVMI_Id,
                                         INVMUOM_Id = b.INVMUOM_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVMUOM_UOMName = d.INVMUOM_UOMName,
                                         INVTSQ_QuotedRate = b.INVTSQ_QuotedRate,
                                         INVTSQ_NegotiatedRate = b.INVTSQ_NegotiatedRate,
                                     }).Distinct().ToList();
                    }
                    else
                    {
                        qtdetails = (from a in _INVContext.INV_M_SupplierQuotationDMO
                                     from b in _INVContext.INV_T_SupplierQuotationDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO
                                     where (a.INVMSQ_Id == b.INVMSQ_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                              && a.INVMSQ_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMSQ_Id == data.INVMSQ_Id && a.INVMSQ_FinaliseFlg == true && b.INVTSQ_FinaliseFlg == true)
                                     select new INV_PurchaseOrderDTO
                                     {
                                         INVMPI_Id = a.INVMPI_Id,
                                         INVTSQ_Id = b.INVTSQ_Id,
                                         INVMI_Id = b.INVMI_Id,
                                         INVMUOM_Id = b.INVMUOM_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVMUOM_UOMName = d.INVMUOM_UOMName,
                                         INVTSQ_QuotedRate = b.INVTSQ_QuotedRate,
                                         INVTSQ_NegotiatedRate = b.INVTSQ_NegotiatedRate,
                                     }).Distinct().ToList();
                    }
                }
                data.get_qtdetails = qtdetails.ToArray();
                if (qtdetails.Count > 0)
                {
                    foreach (var pi in qtdetails)
                    {
                        var itemtax = (from a in _INVContext.INV_Master_ItemDMO
                                       from b in _INVContext.INV_Master_Item_TaxDMO
                                       from c in _INVContext.INV_Master_TaxDMO
                                       from d in _INVContext.INV_T_SupplierQuotationDMO
                                       where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == c.MI_Id && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true && a.INVMI_Id == pi.INVMI_Id)
                                       select new INV_PurchaseOrderDTO
                                       {
                                           INVMI_Id = a.INVMI_Id,
                                           INVMI_ItemName = a.INVMI_ItemName,
                                           INVMT_Id = b.INVMT_Id,
                                           INVMT_TaxName = c.INVMT_TaxName,
                                           INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                           INVMIT_TaxValue = b.INVMIT_TaxValue,
                                       }).Distinct().OrderBy(m => m.INVMT_Id).ToList();

                        data.get_itemTax = itemtax.ToArray();
                    }
                }

                //if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                //{
                //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                //    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                //}
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PurchaseOrderNO_Generation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@StudentAdmnoOP", SqlDbType.VarChar, Int32.MaxValue)
                    { Direction = ParameterDirection.Output });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var data1 = cmd.ExecuteNonQuery();
                    data.trans_id = cmd.Parameters["@StudentAdmnoOP"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Details :" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseOrderDTO getitemtax(INV_PurchaseOrderDTO data)
        {
            try
            {
                List<INV_PurchaseOrderDTO> itemtax = new List<INV_PurchaseOrderDTO>();
                if (data.Selectionflag == "P")
                {
                    itemtax = (from a in _INVContext.INV_Master_ItemDMO
                               from b in _INVContext.INV_Master_Item_TaxDMO
                               from c in _INVContext.INV_Master_TaxDMO
                               from d in _INVContext.INV_T_PurchaseIndentDMO
                               where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == c.MI_Id && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true && a.INVMI_Id == data.INVMI_Id && d.INVTPI_PIUnitRate == data.INVTPI_PIUnitRate)
                               select new INV_PurchaseOrderDTO
                               {
                                   INVMI_Id = a.INVMI_Id,
                                   INVMIT_Id = b.INVMIT_Id,
                                   INVMI_ItemName = a.INVMI_ItemName,
                                   INVMT_Id = b.INVMT_Id,
                                   INVMT_TaxName = c.INVMT_TaxName,
                                   INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                   INVMIT_TaxValue = b.INVMIT_TaxValue,
                                   INVTPI_PIUnitRate = d.INVTPI_PIUnitRate
                               }).Distinct().OrderBy(m => m.INVMT_Id).ToList();
                }
                else
                {
                    itemtax = (from a in _INVContext.INV_Master_ItemDMO
                               from b in _INVContext.INV_Master_Item_TaxDMO
                               from c in _INVContext.INV_Master_TaxDMO
                               from d in _INVContext.INV_T_SupplierQuotationDMO
                               where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == c.MI_Id && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true && a.INVMI_Id == data.INVMI_Id && d.INVTSQ_NegotiatedRate == data.INVTSQ_NegotiatedRate)
                               select new INV_PurchaseOrderDTO
                               {
                                   INVMI_Id = a.INVMI_Id,
                                   INVMIT_Id = b.INVMIT_Id,
                                   INVMI_ItemName = a.INVMI_ItemName,
                                   INVMT_Id = b.INVMT_Id,
                                   INVMT_TaxName = c.INVMT_TaxName,
                                   INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                   INVMIT_TaxValue = b.INVMIT_TaxValue,
                                   INVTSQ_NegotiatedRate = d.INVTSQ_NegotiatedRate
                               }).Distinct().OrderBy(m => m.INVMT_Id).ToList();
                }
                data.get_itemTax = itemtax.ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Details :" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseOrderDTO savedetails(INV_PurchaseOrderDTO data)
        {
            try
            {
                //if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                //{
                //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                //    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                //}

                List<long> indids = new List<long>();
                if (data.arrayPO !=null)
                {
                    foreach (var item in data.arrayPO)
                    {
                        indids.Add(item.INVMPI_Id);
                    }
                }
                indids = indids.Distinct().ToList(); 

                if (data.INVMPO_Id != 0)
                {
                   
                    var RemoveOlditems = _INVContext.INV_T_PurchaseOrderDMO.Where(R => R.INVMPO_Id == data.INVMPO_Id).ToList();
                    if(RemoveOlditems.Count > 0)
                    {
                        var RemoveOldtax = _INVContext.INV_T_PurchaseOrder_TaxDMO.Where(R => R.INVTPO_Id == RemoveOlditems.FirstOrDefault().INVTPO_Id).ToList();

                        if(RemoveOldtax.Count > 0)
                        {
                            foreach(var  d  in RemoveOldtax)
                            {
                                _INVContext.Remove(d);
                                _INVContext.SaveChanges();
                            }
                            
                        }
                        foreach (var f in RemoveOlditems)
                        {
                            _INVContext.Remove(f);
                            _INVContext.SaveChanges();
                        }
                    }
                    var M_PurchaseOrder = _INVContext.INV_M_PurchaseOrderDMO.Where(R => R.INVMPO_Id == data.INVMPO_Id).ToList();
                    if(M_PurchaseOrder.Count > 0)
                    {
                        foreach (var m in M_PurchaseOrder)
                        {
                            _INVContext.Remove(m);
                            _INVContext.SaveChanges();
                        }
                    }


                    INV_M_PurchaseOrderDMO po = new INV_M_PurchaseOrderDMO();
                    po.MI_Id = data.MI_Id;
                    po.INVMS_Id = data.INVMS_Id;
                    po.INVMPO_PONo = data.trans_id;
                    po.INVMSQ_Id = data.INVMSQ_Id;
                    po.INVMPO_PODate = data.INVMPO_PODate;
                    po.INVMPO_POTemplate = data.INVMPO_POTemplate;
                    po.INVMPO_Remarks = data.INVMPO_Remarks;
                    po.INVMPO_TotRate = data.INVMPO_TotRate;
                    po.INVMPO_ReferenceNo = data.INVMPO_ReferenceNo;
                    po.INVMPO_TotTax = data.INVMPO_TotTax;
                    po.INVMPO_TotAmount = data.INVMPO_TotAmount;
                    po.INVMPO_CreatedBy = data.UserId;
                    po.INVMPO_UpdatedBy = data.UserId;
                    po.INVMPO_Status = data.INVMPO_Status;                   
                    po.INVMPO_ActiveFlg = true;
                    po.INVMPO_RejectFlg = false;
                    po.INVMPO_FinalProcessFlag = false;
                    po.CreatedDate = DateTime.Now;
                    po.UpdatedDate = DateTime.Now;
                    _INVContext.Add(po);
                    //===================================PO Transcation Data
                    foreach (var i in data.arrayPO)
                    {
                        INV_T_PurchaseOrderDMO potrans = new INV_T_PurchaseOrderDMO();
                        potrans.MI_Id = data.MI_Id;
                        potrans.INVMPO_Id = po.INVMPO_Id;
                        potrans.INVMPI_Id = i.INVMPI_Id;
                        potrans.INVMI_Id = i.INVMI_Id;
                        potrans.INVMUOM_Id = i.INVMUOM_Id;
                        potrans.INVTPO_POQty = i.INVTPO_POQty;
                        potrans.INVTPO_RatePerUnit = i.INVTPO_RatePerUnit;
                        potrans.INVTPO_TaxAmount = i.INVTPO_TaxAmount;
                        potrans.INVTPO_Amount = i.INVTPO_Amount;
                        potrans.INVTPO_Remarks = i.INVTPO_Remarks;
                        potrans.INVTPO_ActiveFlg = true;
                        potrans.INVTPO_RejectFlg = false;
                        potrans.INVTPO_ApproveQty = 0;
                        potrans.INVTPO_CreatedBy = data.UserId;
                        potrans.INVTPO_UpdatedBy = data.UserId;
                        potrans.CreatedDate = DateTime.Now;
                        potrans.UpdatedDate = DateTime.Now;
                        _INVContext.Add(potrans);
                        //===================================PO Transcation Tax
                        if (i.arrayPOtax.Length > 0)
                        {
                            foreach (var t in i.arrayPOtax)
                            {
                                INV_T_PurchaseOrder_TaxDMO potax = new INV_T_PurchaseOrder_TaxDMO();
                                potax.MI_Id = data.MI_Id;
                                potax.INVTPO_Id = potrans.INVTPO_Id;
                                potax.INVMIT_Id = t.INVMIT_Id;
                                potax.INVTPOT_TaxPercent = t.INVTPOT_TaxPercent;
                                potax.INVTPOT_TaxAmount = t.INVTPOT_TaxAmount;
                                potax.INVTPOT_ActiveFlg = true;
                                potax.INVTPOT_CreatedBy = data.UserId;
                                potax.INVTPOT_UpdatedBy = data.UserId;
                                potax.CreatedDate = DateTime.Now;
                                potax.UpdatedDate = DateTime.Now;
                                _INVContext.Add(potax);
                            }
                        }
                    }
                    using (var dbPOTxn = _INVContext.Database.BeginTransaction())
                    {
                        var contexttrans = _INVContext.SaveChanges();
                        dbPOTxn.Commit();
                        if (contexttrans > 0)
                        {
                            var indentlist = _INVContext.INV_M_PurchaseIndentDMO.Where(a => a.INVMPI_ActiveFlg == true && indids.Contains(a.INVMPI_Id)).Distinct().ToList();

                            if (indentlist.Count > 0)
                            {
                                foreach (var item in indentlist)
                                {
                                    item.INVMPI_POCreatedFlg = true;
                                    item.INVMPI_UpdatedBy = data.UserId;
                                    _INVContext.Update(item);
                                }
                            }
                            _INVContext.SaveChanges();
                            data.returnval = true;

                            var primaryid = _INVContext.INV_M_PurchaseOrderDMO.Where(e => e.INVMPO_Id == po.INVMPO_Id).ToList();
                            if (primaryid.Count > 0)
                            {
                                long hrid = 0;

                                var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                                if (hrmeid1.Count > 0)
                                {
                                    hrid = hrmeid1.FirstOrDefault().Emp_Code;
                                }

                                emaildata(primaryid.FirstOrDefault().INVMPO_Id, hrid, data.MI_Id, data.UserId);
                            }

                        }
                        else
                        {
                            dbPOTxn.Rollback();
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    INV_M_PurchaseOrderDMO po = new INV_M_PurchaseOrderDMO();
                    po.MI_Id = data.MI_Id;
                    po.INVMS_Id = data.INVMS_Id;
                    po.INVMPO_PONo = data.trans_id;
                    po.INVMSQ_Id = data.INVMSQ_Id;
                    po.INVMPO_PODate = data.INVMPO_PODate;
                    po.INVMPO_POTemplate = data.INVMPO_POTemplate;
                    po.INVMPO_Remarks = data.INVMPO_Remarks;
                    po.INVMPO_TotRate = data.INVMPO_TotRate;
                    po.INVMPO_ReferenceNo = data.INVMPO_ReferenceNo;
                    po.INVMPO_TotTax = data.INVMPO_TotTax;
                    po.INVMPO_TotAmount = data.INVMPO_TotAmount;
                    po.INVMPO_CreatedBy = data.UserId;
                    po.INVMPO_UpdatedBy = data.UserId;
                    po.INVMPO_ActiveFlg = true;
                    po.INVMPO_RejectFlg = false;
                    po.INVMPO_FinalProcessFlag = false;
                    po.CreatedDate = DateTime.Now;
                    po.UpdatedDate = DateTime.Now;
                    po.INVMPO_Status = data.INVMPO_Status;
                    _INVContext.Add(po);
                    //===================================PO Transcation Data
                    foreach (var i in data.arrayPO)
                    {
                        INV_T_PurchaseOrderDMO potrans = new INV_T_PurchaseOrderDMO();
                        potrans.MI_Id = data.MI_Id;
                        potrans.INVMPO_Id = po.INVMPO_Id;
                        potrans.INVMPI_Id = i.INVMPI_Id;
                        potrans.INVMI_Id = i.INVMI_Id;
                        potrans.INVMUOM_Id = i.INVMUOM_Id;
                        potrans.INVTPO_POQty = i.INVTPO_POQty;
                        potrans.INVTPO_RatePerUnit = i.INVTPO_RatePerUnit;
                        potrans.INVTPO_TaxAmount = i.INVTPO_TaxAmount;
                        potrans.INVTPO_Amount = i.INVTPO_Amount;
                        potrans.INVTPO_Remarks = i.INVTPO_Remarks;
                        potrans.INVTPO_ActiveFlg = true;
                        potrans.INVTPO_RejectFlg = false;
                        potrans.INVTPO_ApproveQty = 0;
                        potrans.INVTPO_CreatedBy = data.UserId;
                        potrans.INVTPO_UpdatedBy = data.UserId;
                        potrans.CreatedDate = DateTime.Now;
                        potrans.UpdatedDate = DateTime.Now;
                        _INVContext.Add(potrans);
                        //===================================PO Transcation Tax
                        if (i.arrayPOtax.Length > 0)
                        {
                            foreach (var t in i.arrayPOtax)
                            {
                                INV_T_PurchaseOrder_TaxDMO potax = new INV_T_PurchaseOrder_TaxDMO();
                                potax.MI_Id = data.MI_Id;
                                potax.INVTPO_Id = potrans.INVTPO_Id;
                                potax.INVMIT_Id = t.INVMIT_Id;
                                potax.INVTPOT_TaxPercent = t.INVTPOT_TaxPercent;
                                potax.INVTPOT_TaxAmount = t.INVTPOT_TaxAmount;
                                potax.INVTPOT_ActiveFlg = true;
                                potax.INVTPOT_CreatedBy = data.UserId;
                                potax.INVTPOT_UpdatedBy = data.UserId;
                                potax.CreatedDate = DateTime.Now;
                                potax.UpdatedDate = DateTime.Now;
                                _INVContext.Add(potax);
                            }
                        }
                    }
                    using (var dbPOTxn = _INVContext.Database.BeginTransaction())
                    {
                        var contexttrans = _INVContext.SaveChanges();
                        dbPOTxn.Commit();
                        if (contexttrans > 0)
                        {
                            var indentlist = _INVContext.INV_M_PurchaseIndentDMO.Where(a => a.INVMPI_ActiveFlg == true && indids.Contains(a.INVMPI_Id)).Distinct().ToList();

                            if (indentlist.Count > 0)
                            {
                                foreach (var item in indentlist)
                                {
                                    item.INVMPI_POCreatedFlg = true;
                                    item.INVMPI_UpdatedBy = data.UserId;
                                    _INVContext.Update(item);
                                }
                            }
                            _INVContext.SaveChanges();
                            data.returnval = true;

                            var primaryid = _INVContext.INV_M_PurchaseOrderDMO.Where(e => e.INVMPO_Id == po.INVMPO_Id).ToList();
                            if (primaryid.Count > 0)
                            {
                                long hrid = 0;

                                var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                                if (hrmeid1.Count > 0)
                                {
                                    hrid = hrmeid1.FirstOrDefault().Emp_Code;
                                }

                                emaildata(primaryid.FirstOrDefault().INVMPO_Id, hrid, data.MI_Id, data.UserId);
                            }

                        }
                        else
                        {
                            dbPOTxn.Rollback();
                            data.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("PO savedata :" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseOrderDTO deactiveM(INV_PurchaseOrderDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_PurchaseOrderDMO.Single(t => t.INVMPO_Id == data.INVMPO_Id);

                if (result.INVMPO_ActiveFlg == true)
                {
                    result.INVMPO_ActiveFlg = false;
                }
                else if (result.INVMPO_ActiveFlg == false)
                {
                    result.INVMPO_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_PurchaseOrderDMO.Where(t => t.INVMPO_Id == data.INVMPO_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (result.INVMPO_ActiveFlg == true)
                    {
                        rt.INVTPO_ActiveFlg = true;

                        var resulttax = _INVContext.INV_T_PurchaseOrder_TaxDMO.Where(t => t.INVTPO_Id == rt.INVTPO_Id).ToList();
                        foreach (var rtx in resulttax)
                        {
                            rtx.INVTPOT_ActiveFlg = true;
                            rtx.UpdatedDate = DateTime.Now;
                            _INVContext.Update(rtx);
                        }

                        rt.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rt);
                    }
                    if (result.INVMPO_ActiveFlg == false)
                    {
                        rt.INVTPO_ActiveFlg = false;
                        var resulttax = _INVContext.INV_T_PurchaseOrder_TaxDMO.Where(t => t.INVTPO_Id == rt.INVTPO_Id).ToList();
                        foreach (var rtx in resulttax)
                        {
                            rtx.INVTPOT_ActiveFlg = false;
                            rtx.UpdatedDate = DateTime.Now;
                            _INVContext.Update(rtx);
                        }
                        rt.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rt);
                    }

                }

                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public INV_PurchaseOrderDTO deactive(INV_PurchaseOrderDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_PurchaseOrderDMO.Single(t => t.INVTPO_Id == data.INVTPO_Id);

                if (result.INVTPO_ActiveFlg == true)
                {
                    result.INVTPO_ActiveFlg = false;
                }
                else if (result.INVTPO_ActiveFlg == false)
                {
                    result.INVTPO_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                int countactiveT = 0;
                int countactiveF = 0;
                var resultt = _INVContext.INV_T_PurchaseOrderDMO.Where(t => t.INVMPO_Id == data.INVMPO_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (rt.INVTPO_ActiveFlg == false)
                    {
                        countactiveF += 1;
                    }
                    else if (rt.INVTPO_ActiveFlg == true)
                    {
                        countactiveT += 1;
                    }
                }
                var resultmflg = _INVContext.INV_M_PurchaseOrderDMO.Single(t => t.INVMPO_Id == data.INVMPO_Id);
                if (countactiveF > 0 && countactiveT == 0)
                {
                    resultmflg.INVMPO_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }
                else if (countactiveT > 0 && countactiveF == 0)
                {
                    resultmflg.INVMPO_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }

                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public INV_PurchaseOrderDTO deactiveTx(INV_PurchaseOrderDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_PurchaseOrder_TaxDMO.Single(t => t.INVTPOT_Id == data.INVTPOT_Id);

                if (result.INVTPOT_ActiveFlg == true)
                {
                    result.INVTPOT_ActiveFlg = false;
                }
                else if (result.INVTPOT_ActiveFlg == false)
                {
                    result.INVTPOT_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public INV_PurchaseOrderDTO get_modeldetails(INV_PurchaseOrderDTO data)
        {
            try
            {
                data.get_poDetail = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                     from b in _INVContext.INV_T_PurchaseOrderDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO                                    
                                     where (a.INVMPO_Id == b.INVMPO_Id &&  b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == data.MI_Id && b.INVMPO_Id == data.INVMPO_Id)
                                     select new INV_PurchaseOrderDTO
                                     {
                                         INVMPO_Id = a.INVMPO_Id,
                                         INVMPO_PONo = a.INVMPO_PONo,
                                         INVMS_Id = a.INVMS_Id,
                                         INVTPO_Id = b.INVTPO_Id,
                                         INVMPI_Id = b.INVMPI_Id,
                                         INVMI_Id = b.INVMI_Id,
                                         INVMUOM_Id = b.INVMUOM_Id,
                                         INVMSQ_Id = a.INVMSQ_Id,
                                         INVMPO_ReferenceNo = a.INVMPO_ReferenceNo,
                                         INVMPO_PODate = a.INVMPO_PODate,
                                         INVMPO_TotRate = a.INVMPO_TotAmount,
                                         INVMPO_TotTax = a.INVMPO_TotTax,
                                         INVMPO_TotAmount = a.INVMPO_TotAmount,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVMI_ItemCode = c.INVMI_ItemCode,
                                         INVMUOM_UOMName = d.INVMUOM_UOMName,
                                         INVTPO_POQty = b.INVTPO_POQty,
                                         INVTPO_RatePerUnit = b.INVTPO_RatePerUnit,
                                         INVTSQ_NegotiatedRate = b.INVTPO_RatePerUnit,
                                         INVTPI_PIUnitRate = b.INVTPO_RatePerUnit,
                                         INVTPO_TaxAmount = b.INVTPO_TaxAmount,
                                         INVTPO_Amount = b.INVTPO_Amount,
                                         INVTPO_Remarks = b.INVTPO_Remarks,
                                         INVTPO_ActiveFlg = b.INVTPO_ActiveFlg,
                                         INVMPO_POTemplate=a.INVMPO_POTemplate,
                                         optionflag = c.INVMI_ItemType
                                     }).Distinct().OrderBy(i => i.INVTPO_Id).ToArray();


                data.get_supdata = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true && m.INVMS_Id == data.INVMS_Id).OrderBy(m => m.INVMS_Id).Distinct().ToArray();

                data.get_potax = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                  from b in _INVContext.INV_T_PurchaseOrderDMO
                                  from c in _INVContext.INV_T_PurchaseOrder_TaxDMO
                                  from d in _INVContext.INV_Master_Item_TaxDMO
                                  from e in _INVContext.INV_Master_TaxDMO
                                  from f in _INVContext.INV_Master_ItemDMO

                                  where (a.INVMPO_Id == b.INVMPO_Id && b.INVTPO_Id == c.INVTPO_Id && c.INVMIT_Id == d.INVMIT_Id && d.INVMT_Id == e.INVMT_Id && b.INVMI_Id == f.INVMI_Id
                                  && a.MI_Id == data.MI_Id && b.INVTPO_Id == data.INVTPO_Id)
                                  select new INV_PurchaseOrderDTO
                                  {
                                      INVTPOT_Id = c.INVTPOT_Id,
                                      INVMPO_Id = a.INVMPO_Id,
                                      INVMPO_PONo = a.INVMPO_PONo,
                                      INVTPO_Id = b.INVTPO_Id,
                                      INVMI_Id = b.INVMI_Id,
                                      INVMI_ItemName = f.INVMI_ItemName,
                                      INVMI_ItemCode = f.INVMI_ItemCode,
                                      INVMT_TaxName = e.INVMT_TaxName,
                                      INVTPOT_TaxPercent = c.INVTPOT_TaxPercent,
                                      INVTPOT_TaxAmount = c.INVTPOT_TaxAmount,
                                      INVTPOT_ActiveFlg = c.INVTPOT_ActiveFlg,

                                  }).Distinct().OrderBy(i => i.INVTPO_Id).ToArray();

                //data.get_editDetail = (from a in _INVContext.INV_M_PurchaseOrderDMO
                //                       from b in _INVContext.INV_T_PurchaseOrderDMO
                //                       from c in _INVContext.INV_Master_ItemDMO
                //                       from d in _INVContext.INV_Master_UOMDMO
                //                       from e in _INVContext.INV_T_PurchaseOrder_TaxDMO
                //                       from f in _INVContext.INV_M_SupplierQuotationDMO
                //                       where (a.INVMPO_Id == b.INVMPO_Id && b.INVTPO_Id == e.INVTPO_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                //                       && a.MI_Id == data.MI_Id && b.INVMPO_Id == data.INVMPO_Id)
                //                       select new INV_PurchaseOrderDTO
                //                       {
                //                           INVMPO_Id = a.INVMPO_Id,
                //                           INVMPO_PONo = a.INVMPO_PONo,
                //                           INVTPO_Id = b.INVTPO_Id,
                //                           INVMI_Id = b.INVMI_Id,
                //                           INVMUOM_Id = b.INVMUOM_Id,
                //                           INVMPO_ReferenceNo = a.INVMPO_ReferenceNo,
                //                           INVMPO_PODate = a.INVMPO_PODate,
                //                           INVMI_ItemName = c.INVMI_ItemName,
                //                           INVMI_ItemCode = c.INVMI_ItemCode,
                //                           INVMUOM_UOMName = d.INVMUOM_UOMName,
                //                           INVTPO_POQty = b.INVTPO_POQty,
                //                           INVTPO_RatePerUnit = b.INVTPO_RatePerUnit,
                //                           INVTPO_TaxAmount = b.INVTPO_TaxAmount,
                //                           INVTPO_Amount = b.INVTPO_Amount,
                //                           INVTPO_Remarks = b.INVTPO_Remarks,
                //                           INVTPO_ActiveFlg = b.INVTPO_ActiveFlg,

                //                       }).Distinct().OrderBy(i => i.INVTPO_Id).ToArray();
                data.get_PI = _INVContext.INV_M_PurchaseIndentDMO.Where(p => p.MI_Id == data.MI_Id && p.INVMPI_ActiveFlg == true).OrderBy(o => o.INVMPI_Id).Distinct().OrderByDescending(r => r.INVMPI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public INV_PurchaseOrderDTO viewtemplatedetails(INV_PurchaseOrderDTO data)
        {
            try
            {
                if(data.message== "FetchTemp")
                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TEMPLATES";
                        cmd.CommandType = CommandType.StoredProcedure;
                      cmd.Parameters.Add(new SqlParameter("@TEMPLATENAME", SqlDbType.VarChar)
                        {
                            Value = data.pending
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
                            data.templatelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else {
                    decimal total = 0;
                    string str = "";
                    var checktemplate = _INVContext.SMSEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Id == data.ISES_Id && a.ISES_MailActiveFlag == true).ToList();
                    if (checktemplate.Count == 0)
                    {
                        data.message = "Email Template not Mapped to the selected Institution";
                    }
                    else
                    {


                        if (data.columnids != null)
                        {
                            str += "<table width='95%' border=" + "'1'" + "    id=" + "'tablepaging'" + " class=" + "'table table-hover table-responsive table-bordered'" + " style=" + "'border-collapse: collapse; width: 95%;'" + "><tr>" +
                           "<th style=" + "width:20px'" + "> SL.NO. </th>";
                            foreach (var item in data.columnids)
                            {
                                if (item.coulmn == "ITEMNAME")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "QUANTITY")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "PRICE/UNIT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "AMOUNT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "TAX")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }
                                if (item.coulmn == "TOTALAMOUNT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "REMARKS")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }
                            }

                            str += "</tr>";
                        }



                        int rownumber = 0;
                        for (int j = 0; j < data.arrayPO.Length; j++)
                        {
                            rownumber += 1;


                            total += data.arrayPO[j].INVTPO_Amount;
                            if (data.columnids != null)
                            {

                                str += "<tr>" +
                              "<td" + " >" + rownumber + "</td> ";

                                foreach (var item1 in data.columnids)
                                {

                                    if (item1.coulmn == "ITEMNAME")
                                    {
                                        str += "<td >" + data.arrayPO[j].INVMI_ItemName + "</td>";
                                    }

                                    if (item1.coulmn == "QUANTITY")
                                    {
                                        str += "<td>" + data.arrayPO[j].INVTPO_POQty + "</th>";
                                    }

                                    if (item1.coulmn == "PRICE/UNIT")
                                    {
                                        str += "<td>" + data.arrayPO[j].INVTPO_RatePerUnit + "</th>";
                                    }

                                    if (item1.coulmn == "AMOUNT")
                                    {
                                        str += "<td  >" + data.arrayPO[j].INVTPO_POQty * data.arrayPO[j].INVTPO_RatePerUnit + "</th>";
                                    }

                                    if (item1.coulmn == "TAX")
                                    {
                                        str += "<td  >" + data.arrayPO[j].INVTPO_TaxAmount + "</th>";
                                    }
                                    if (item1.coulmn == "TOTALAMOUNT")
                                    {
                                        str += "<td  >" + data.arrayPO[j].INVTPO_Amount + "</th>";
                                    }

                                    if (item1.coulmn == "REMARKS")
                                    {
                                        str += "<td  >" + data.arrayPO[j].INVTPO_Remarks + "</th>";
                                    }
                                }

                                str += "</tr>"; ;
                            }

                        }


                        str += "<tr>" +
                               "<td colspan =" + "" + data.columnids.Length + ">" + "TOTAL" + "</td> ";

                        str += "<td  >" + total + "</th>";
                        str += "</tr>"; ;
                        str += "</table>";


                        Dictionary<string, string> val = new Dictionary<string, string>();


                        var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == checktemplate[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                        var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                        string Mailcontent = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;
                        string Mailmsg = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                        string Resultsms = Mailcontent;
                        string result = Mailmsg;


                        Mailcontent = Mailcontent.Replace("[SUBJECT]", checktemplate.FirstOrDefault().ISES_MailSubject);



                        List<Match> variables = new List<Match>();

                        foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                        {
                            variables.Add(match);
                        }
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "INVPOTEMPLATEDETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@INVMPO_Id",
                               SqlDbType.VarChar)
                            {
                                Value = data.INVMPO_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Template",
                              SqlDbType.VarChar)
                            {
                                Value = checktemplate.FirstOrDefault().ISES_Template_Name
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISES_Id",
                              SqlDbType.VarChar)
                            {
                                Value = data.ISES_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                                  SqlDbType.VarChar)
                            {
                                Value = data.INVMS_Id
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

                            }

                        }
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

                        Mailcontent = Mailcontent = Mailcontent.Replace("[ITEMS]", str);
                        Mailcontent = Mailcontent = Mailcontent.Replace("[REFNO]", data.INVMPO_ReferenceNo);
                        Mailcontent = Mailcontent = Mailcontent.Replace("[DATE]", data.INVMPO_PODate.ToString("dd/MM/yyyy"));

                        data.mailmsg = Mailcontent;


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public INV_PurchaseOrderDTO viewsavedteple(INV_PurchaseOrderDTO data)
        {
            try
            {
                decimal total = 0;
                string str = "";
                var checktemplate = _INVContext.SMSEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name.Trim() == data.INVMPO_POTemplate.Trim() && a.ISES_MailActiveFlag == true).ToList();
                if (checktemplate.Count == 0)
                {
                    data.message = "Email Template not Mapped to the selected Institution";
                }
                else
                {
                    var itemdetails = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                       from b in _INVContext.INV_T_PurchaseOrderDMO
                                       from c in _INVContext.INV_Master_ItemDMO
                                       from d in _INVContext.INV_Master_UOMDMO
                                       where (a.INVMPO_Id == b.INVMPO_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                                && a.INVMPO_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPO_Id == data.INVMPO_Id && b.INVTPO_ActiveFlg == true)
                                       select new INV_PurchaseOrderDTO
                                       {
                                           INVMPO_Id = a.INVMPO_Id,
                                           INVMPO_PONo = a.INVMPO_PONo,
                                           INVMI_Id = b.INVMI_Id,
                                           INVMUOM_Id = b.INVMUOM_Id,
                                           INVMPI_Id = b.INVMPI_Id,
                                           INVMI_ItemName = c.INVMI_ItemName,
                                           INVMUOM_UOMName = d.INVMUOM_UOMName,
                                           INVMPO_PODate = a.INVMPO_PODate,
                                           INVMPO_ReferenceNo = a.INVMPO_ReferenceNo,
                                           INVTPO_POQty = b.INVTPO_POQty,
                                           INVTPO_Amount = b.INVTPO_Amount,
                                           INVTPO_RatePerUnit = b.INVTPO_RatePerUnit,
                                           INVTPO_TaxAmount = b.INVTPO_TaxAmount,
                                           INVMPO_Remarks = a.INVMPO_Remarks,
                                           INVMPO_TotAmount = a.INVMPO_TotAmount,
                                           INVMPO_ActiveFlg = a.INVMPO_ActiveFlg,
                                           INVTPO_Remarks = b.INVTPO_Remarks,
                                           INVMPO_TotRate = a.INVMPO_TotRate,
                                           INVMPO_TotTax = a.INVMPO_TotTax,
                                       }).Distinct().OrderBy(i => i.INVMI_ItemName).ToList();



                    if (itemdetails.Count > 0)
                    {


                        if (data.columnids != null)
                        {
                            str += "<table width='95%' border=" + "'1'" + "    id=" + "'tablepaging'" + " class=" + "'table table-hover table-responsive table-bordered'" + " style=" + "'border-collapse: collapse; width: 95%;'" + "><tr>" +
                           "<th style=" + "width:20px'" + "> SL.NO. </th>";
                            foreach (var item in data.columnids)
                            {
                                if (item.coulmn == "ITEMNAME")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "QUANTITY")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "PRICE/UNIT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "AMOUNT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "TAX")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }
                                if (item.coulmn == "TOTALAMOUNT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "REMARKS")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }
                            }

                            str += "</tr>";
                        }



                        int rownumber = 0;
                        for (int j = 0; j < itemdetails.Count; j++)
                        {
                            rownumber += 1;


                            total += itemdetails[j].INVTPO_Amount;
                            if (data.columnids != null)
                            {

                                str += "<tr>" +
                              "<td" + " >" + rownumber + "</td> ";

                                foreach (var item1 in data.columnids)
                                {

                                    if (item1.coulmn == "ITEMNAME")
                                    {
                                        str += "<td >" + itemdetails[j].INVMI_ItemName + "</td>";
                                    }

                                    if (item1.coulmn == "QUANTITY")
                                    {
                                        str += "<td>" + itemdetails[j].INVTPO_POQty + "</th>";
                                    }

                                    if (item1.coulmn == "PRICE/UNIT")
                                    {
                                        str += "<td>" + itemdetails[j].INVTPO_RatePerUnit + "</th>";
                                    }

                                    if (item1.coulmn == "AMOUNT")
                                    {
                                        str += "<td  >" + itemdetails[j].INVTPO_POQty * itemdetails[j].INVTPO_RatePerUnit + "</th>";
                                    }

                                    if (item1.coulmn == "TAX")
                                    {
                                        str += "<td  >" + itemdetails[j].INVTPO_TaxAmount + "</th>";
                                    }
                                    if (item1.coulmn == "TOTALAMOUNT")
                                    {
                                        str += "<td  >" + itemdetails[j].INVTPO_Amount + "</th>";
                                    }

                                    if (item1.coulmn == "REMARKS")
                                    {
                                        str += "<td  >" + itemdetails[j].INVTPO_Remarks + "</th>";
                                    }
                                }

                                str += "</tr>"; ;
                            }

                        }


                        str += "<tr>" +
                               "<td colspan =" + "" + data.columnids.Length + ">" + "TOTAL" + "</td> ";

                        str += "<td  >" + total + "</th>";
                        str += "</tr>"; ;
                        str += "</table>";


                        Dictionary<string, string> val = new Dictionary<string, string>();


                        var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == checktemplate[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                        var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                        string Mailcontent = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;
                        string Mailmsg = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                        string Resultsms = Mailcontent;
                        string result = Mailmsg;


                        Mailcontent = Mailcontent.Replace("[SUBJECT]", checktemplate.FirstOrDefault().ISES_MailSubject);



                        List<Match> variables = new List<Match>();

                        foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                        {
                            variables.Add(match);
                        }
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {

                            cmd.CommandText = "INVPOSAVEDTEMPLATEDETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@INVMPO_Id",
                               SqlDbType.VarChar)
                            {
                                Value = data.INVMPO_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Template",
                              SqlDbType.VarChar)
                            {
                                Value = checktemplate.FirstOrDefault().ISES_Template_Name
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISES_Id",
                              SqlDbType.VarChar)
                            {
                                Value = data.ISES_Id
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

                            }

                        }
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

                        Mailcontent = Mailcontent = Mailcontent.Replace("[ITEMS]", str);

                        data.mailmsg = Mailcontent;

                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }


        public string emaildata(long INVMPO_Id, long HRME_Id, long MI_Id, long UserID)
        {
            string suss = "";
            try
            {

                decimal total = 0;
                string str = "";
                var itemlist = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                from b in _INVContext.INV_T_PurchaseOrderDMO
                                from c in _INVContext.INV_Master_ItemDMO
                                from d in _INVContext.INV_Master_UOMDMO
                                where a.INVMPO_Id == b.INVMPO_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && b.INVTPO_ActiveFlg == true && a.INVMPO_Id == INVMPO_Id
                                select new INV_PurchaseOrderDTO
                                {
                                    INVMI_ItemName = c.INVMI_ItemName,
                                    INVMPO_Id = b.INVMPO_Id,
                                    INVTPO_Id = b.INVTPO_Id,
                                    INVTPO_POQty = b.INVTPO_POQty,
                                    INVTPO_RatePerUnit = b.INVTPO_RatePerUnit,
                                    INVTPO_TaxAmount = b.INVTPO_TaxAmount,
                                    INVTPO_Amount = b.INVTPO_Amount,
                                    INVTPO_Remarks = b.INVTPO_Remarks,
                                    INVMUOM_UOMName = d.INVMUOM_UOMName,
                                }
                              ).Distinct().ToList();

                if (itemlist.Count > 0)
                {
                    str += "<table width='100%' border=" + "'1'" + "    id=" + "'tablepaging'" + " class=" + "'table table-hover table-responsive table-bordered'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                   "<th style=" + "width:20px'" + "> SL.NO. </th>";
                    str += "<th>" + "ITEM NAME" + "</th>";
                    str += "<th>" + "UOM" + "</th>";
                    str += "<th>" + "PO QUANTITY" + "</th>";

                    str += "<th>" + "PRICE/UNIT" + "</th>";
                    str += "<th>" + "TAX" + "</th>";
                    str += "<th>" + "AMOUNT" + "</th>";

                    str += "<th>" + "REMARKS" + "</th>";
                    str += "</tr>";
                    int rownumber = 0;
                    for (int j = 0; j < itemlist.Count; j++)
                    {
                        rownumber += 1;
                        total += itemlist[j].INVTPI_ApproxAmount;
                        str += "<tr>" +
                      "<td" + " >" + rownumber + "</td> ";
                        str += "<td >" + itemlist[j].INVMI_ItemName + "</td>";
                        str += "<td >" + itemlist[j].INVMUOM_UOMName + "</td>";
                        str += "<td>" + itemlist[j].INVTPO_POQty + "</th>";
                        str += "<td>" + itemlist[j].INVTPO_RatePerUnit + "</th>";
                        str += "<td>" + itemlist[j].INVTPO_TaxAmount + "</th>";
                        str += "<td  >" + itemlist[j].INVTPO_Amount + "</th>";
                        str += "<td  >" + itemlist[j].INVTPO_Remarks + "</th>";
                        str += "</tr>";
                    }

                    str += "</table>";


                    var tomailid = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == HRME_Id && a.HRMEM_DeFaultFlag == "default").ToList();

                    if (tomailid.Count > 0)
                    {
                        suss = sendmail(MI_Id, UserID, "POTEMPLATE", tomailid.FirstOrDefault().HRMEM_EmailId, str, INVMPO_Id);
                    }

                    var approvallist = (from a in _db.HR_PROCESSDMO
                                        from b in _db.HR_Process_Auth_OrderNoDMO
                                        where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PO" && b.HRPAON_SanctionLevelNo == 1
                                        select new INV_PurchaseIndentDTO
                                        {
                                            UserId = b.IVRMUL_Id
                                        }).Distinct().ToList();


                    if (approvallist.Count > 0)
                    {
                        long apprhrmeid = 0;
                        var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == approvallist.FirstOrDefault().UserId).ToList();
                        if (hrmeid1.Count > 0)
                        {
                            apprhrmeid = hrmeid1.FirstOrDefault().Emp_Code;
                        }

                        var tomailidapp = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == apprhrmeid && a.HRMEM_DeFaultFlag == "default").ToList();
                        if (tomailidapp.Count > 0)
                        {
                            if (tomailidapp.FirstOrDefault().HRMEM_EmailId != null && tomailidapp.FirstOrDefault().HRMEM_EmailId != "")
                            {
                                suss = sendmail(MI_Id, UserID, "POTEMPLATEAPPROVAL", tomailidapp.FirstOrDefault().HRMEM_EmailId, str, INVMPO_Id);
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return suss;
        }



        //EMAIL

        #region   email
        public string sendmail(long MI_Id, long UserID, string Template, string emailid, string items, long INVMPI_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INVENTORY_ALL_MAIL_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMPR_Id",
                           SqlDbType.VarChar)
                        {
                            Value = INVMPI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Template",
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
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

                Mailmsg = Mailmsg.Replace("[ITEMS]", items);

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;


                        message.AddTo(emailid);




                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }




                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

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

                                emailMessage.To.Add(new MailAddress(emailid));



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

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "INVALL_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailid
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion


        //END
    }
}
