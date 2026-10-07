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
using DomainModel.Model;
using System.Net;
using System.IO;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_PurchaseRequisitionImpl : Interface.INV_PurchaseRequisitionInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PurchaseRequisitionImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_PurchaseRequisitionImpl(InventoryContext InvContext, ILogger<INV_PurchaseRequisitionImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_PurchaseRequisitionDTO getloaddata(INV_PurchaseRequisitionDTO data)
        {
            try
            {

                var hrmeid = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();

                if (hrmeid.Count > 0)
                {
                    data.HRME_Id = hrmeid.FirstOrDefault().Emp_Code;
                }

                data.get_mi_list = _INVContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.get_pidata = _INVContext.INV_T_PurchaseIndentDMO.Where(p => p.MI_Id == data.MI_Id && p.INVTPI_ActiveFlg == true).ToArray();       

                data.get_item = (from a in _INVContext.INV_Master_ItemDMO
                                 from c in _INVContext.INV_Master_UOMDMO
                                 where (c.INVMUOM_Id == a.INVMUOM_Id && a.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                 select new INV_PurchaseRequisitionDTO
                                 {
                                     INVMI_Id = a.INVMI_Id,
                                     INVMUOM_Id = a.INVMUOM_Id,
                                     INVMI_ItemName = a.INVMI_ItemName,
                                     INVMI_ItemCode = a.INVMI_ItemCode,
                                 }).Distinct().OrderBy(i => i.INVMI_ItemName).ToArray();

                data.get_purchaserequisition = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                                from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                                where (a.INVMPR_Id == b.INVMPR_Id  && a.MI_Id == data.MI_Id && a.INVMPR_CreatedBy==data.UserId)
                                                select new INV_PurchaseRequisitionDTO
                                                {
                                                    INVMPR_Id = a.INVMPR_Id,                                                 
                                                    INVMPR_PRNo = a.INVMPR_PRNo,                                                    
                                                    INVMPR_PRDate = a.INVMPR_PRDate,
                                                    INVMPR_Remarks = a.INVMPR_Remarks,
                                                    INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                                    INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                                    INVMPR_ActiveFlg = a.INVMPR_ActiveFlg,
                                                    INVMPR_RejectFlg = a.INVMPR_RejectFlg
                                                }).Distinct().OrderBy(i => i.INVMPR_Id).ToArray();

                data.Client_Master = _INVContext.clientTable.Where(t => t.ISMMCLT_ActiveFlag == true).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition load Page:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseRequisitionDTO get_prdetails(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                data.get_prDetail = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                     from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO                                
                                     where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id  && b.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == data.MI_IdNew && a.INVMPR_Id == data.INVMPR_Id)
                                     select new INV_PurchaseRequisitionDTO
                                     {
                                         INVTPR_Id = b.INVTPR_Id,
                                         INVMPR_Id = a.INVMPR_Id,
                                         INVMI_Id = b.INVMI_Id,
                                         INVMUOM_Id = b.INVMUOM_Id,                                        
                                         INVMPR_PRNo = a.INVMPR_PRNo,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVMUOM_UOMName = d.INVMUOM_UOMName,                                       
                                         INVTPR_PRQty = b.INVTPR_PRQty,
                                         INVTPR_PRUnitRate = b.INVTPR_PRUnitRate,
                                         INVTPR_ApproxAmount = b.INVTPR_ApproxAmount,
                                         INVTPR_ApprovedQty = b.INVTPR_ApprovedQty,
                                         INVMPR_PRDate = a.INVMPR_PRDate,
                                         INVMPR_Remarks = a.INVMPR_Remarks,
                                         INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                         INVTPR_Remarks = b.INVTPR_Remarks,
                                         INVTPR_RejectFlg = b.INVTPR_RejectFlg,
                                         INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                         INVMPR_ActiveFlg = a.INVMPR_ActiveFlg,
                                         INVTPR_ActiveFlg = b.INVTPR_ActiveFlg

                                     }).Distinct().OrderByDescending(i => i.INVTPR_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition Item details:" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseRequisitionDTO getitemDetail(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                data.get_itemDetail = (from a in _INVContext.INV_Master_ItemDMO
                                       from c in _INVContext.INV_Master_UOMDMO
                                       where (c.INVMUOM_Id == a.INVMUOM_Id && a.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMI_Id == data.INVMI_Id)
                                       select new INV_PurchaseRequisitionDTO
                                       {
                                           INVMI_Id = a.INVMI_Id,
                                           INVMI_ItemName = a.INVMI_ItemName,
                                           INVMI_ItemCode = a.INVMI_ItemCode,
                                           INVMUOM_Id = a.INVMUOM_Id,
                                           INVMUOM_UOMName = c.INVMUOM_UOMName,
                                       }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition Item details:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseRequisitionDTO savedetails(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                var hrmeid = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();

                if (hrmeid.Count>0)
                {
                    data.HRME_Id = hrmeid.FirstOrDefault().Emp_Code;
                }
                    
                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_IdNew;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                if (data.INVMPR_Id > 0)
                {
                    var result = _INVContext.INV_M_PurchaseRequisitionDMO.Single(t => t.MI_Id == data.MI_IdNew && t.INVMPR_Id == data.INVMPR_Id);
                    result.MI_Id = data.MI_IdNew;
                    result.INVMPR_PRNo = data.trans_id;
                    result.INVMPR_PRDate = data.INVMPR_PRDate;
                    result.INVMPR_Remarks = data.INVMPR_Remarks;
                    result.INVMPR_ApproxTotAmount = data.INVMPR_ApproxTotAmount;
                    result.INVMPR_PICreatedFlg = false;
                    result.INVMPR_ActiveFlg = true;
                    result.INVMPR_RejectFlg = false;
                    result.INVMPR_UpdatedBy = data.UserId;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);

                    foreach (var r in data.arrayPR)
                    {
                        var res1 = _INVContext.INV_T_PurchaseRequisitionDMO.Where(a => a.INVTPR_Id == r.INVTPR_Id).ToList();
                        if (res1.Count > 0)
                        {
                            var res11 = _INVContext.INV_T_PurchaseRequisitionDMO.Single(a => a.INVTPR_Id == r.INVTPR_Id);
                            res11.INVMPR_Id = result.INVMPR_Id;
                            res11.INVTPR_PRQty = r.INVTPR_PRQty;
                            res11.INVTPR_PRUnitRate = r.INVTPR_PRUnitRate;
                            res11.INVTPR_ApproxAmount = r.INVTPR_ApproxAmount;
                            res11.INVTPR_ApprovedQty = r.INVTPR_ApprovedQty;
                            res11.INVTPR_Remarks = r.INVTPR_Remarks;
                            res11.INVTPR_ActiveFlg = true;
                            res11.INVTPR_RejectFlg = false;
                            res11.INVTPR_UpdatedBy = data.UserId;
                            res11.UpdatedDate = DateTime.Now;
                            _INVContext.Update(res11);
                        }
                    }

                    foreach (var c in data.clientnames)
                    {
                        var resclt = _INVContext.INV_M_PurchaseRequisition_ClientDMO.Where(a => a.INVMPRCLT_Id == c.INVMPRCLT_Id).ToList();
                        if (resclt.Count > 0)
                        {
                            var rescltt = _INVContext.INV_M_PurchaseRequisition_ClientDMO.Single(a => a.INVMPRCLT_Id == c.INVMPRCLT_Id);
                            rescltt.INVMPR_Id = result.INVMPR_Id;
                            rescltt.INVMPRCLT_Remarks = c.INVMPRCLT_Remarks;
                            rescltt.INVMPRCLT_StatusFlg = c.INVMPRCLT_StatusFlg;
                            rescltt.INVMPRCLT_ActiveFlag = true;
                            rescltt.INVMPRCLT_UpdatedBy = data.UserId;
                            rescltt.INVMPRCLT_UpdatedDate = DateTime.Now;
                            _INVContext.Update(rescltt);
                        }
                    }

                    var contactExists = _INVContext.SaveChanges();
                    if (contactExists > 0)
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
                    INV_M_PurchaseRequisitionDMO pr = new INV_M_PurchaseRequisitionDMO();
                    pr.MI_Id = data.MI_IdNew;
                    pr.HRME_Id = data.HRME_Id;
                    pr.INVMPR_PRNo = data.trans_id;
                    pr.INVMPR_PRDate = data.INVMPR_PRDate;
                    pr.INVMPR_Remarks = data.INVMPR_Remarks;
                    pr.INVMPR_ApproxTotAmount = data.INVMPR_ApproxTotAmount;
                    pr.INVMPR_PICreatedFlg = false;
                    pr.INVMPR_ActiveFlg = true;
                    pr.INVMPR_RejectFlg = false;
                    pr.INVMPR_CreatedBy = data.UserId;
                    pr.INVMPR_UpdatedBy = data.UserId;
                    pr.CreatedDate = DateTime.Now;
                    pr.UpdatedDate = DateTime.Now; 
                    _INVContext.Add(pr);

                    foreach (var r in data.arrayPR)
                    {
                        INV_T_PurchaseRequisitionDMO tpr = new INV_T_PurchaseRequisitionDMO();
                        tpr.INVMPR_Id = pr.INVMPR_Id;
                        tpr.INVMI_Id = r.INVMI_Id;
                        tpr.INVMUOM_Id = r.INVMUOM_Id;
                        tpr.INVTPR_PRQty = r.INVTPR_PRQty;
                        tpr.INVTPR_PRUnitRate = r.INVTPR_PRUnitRate;
                        tpr.INVTPR_ApproxAmount = r.INVTPR_ApproxAmount;
                        tpr.INVTPR_ApprovedQty = r.INVTPR_ApprovedQty;
                        tpr.INVTPR_Remarks = r.INVTPR_Remarks;
                        tpr.INVTPR_ActiveFlg = true;
                        tpr.INVTPR_RejectFlg = false;
                        tpr.INVTPR_CreatedBy = data.UserId;
                        tpr.INVTPR_UpdatedBy = data.UserId;
                        tpr.CreatedDate = DateTime.Now; 
                        tpr.UpdatedDate = DateTime.Now;
                        _INVContext.Add(tpr);
                    }

                    foreach (var c in data.clientnames)
                    {
                        INV_M_PurchaseRequisition_ClientDMO clt = new INV_M_PurchaseRequisition_ClientDMO();
                        clt.INVMPR_Id = pr.INVMPR_Id;
                        clt.ISMMCLT_Id = c.ISMMCLT_Id;
                        clt.INVMPRCLT_Remarks = c.INVMPRCLT_Remarks;
                        clt.INVMPRCLT_StatusFlg = c.INVMPRCLT_StatusFlg;
                        clt.INVMPRCLT_CreatedBy = data.UserId;
                        clt.INVMPRCLT_UpdatedBy = data.UserId;
                        clt.INVMPRCLT_ActiveFlag = true;
                        clt.INVMPRCLT_CreatedDate = DateTime.Now;
                        clt.INVMPRCLT_UpdatedDate = DateTime.Now;
                        _INVContext.Add(clt);
                    }

                    var contactExists = _INVContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        var primaryid = _INVContext.INV_M_PurchaseRequisitionDMO.Where(e => e.INVMPR_Id == pr.INVMPR_Id).ToList();
                        if (primaryid.Count>0)
                        {
                            long hrid = 0;
                            if (data.HRME_Id !=null)
                            {
                                hrid = Convert.ToInt64(data.HRME_Id);
                            }

                            emaildata(primaryid.FirstOrDefault().INVMPR_Id, hrid, data.MI_IdNew, data.UserId);
                        }

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
                data.message = "Error";
                _logInv.LogInformation("Purchase Requisition savedata :" + ex.Message);
            }

            return data;
        }
        public INV_PurchaseRequisitionDTO edit(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                data.editPR = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                               from b in _INVContext.INV_T_PurchaseRequisitionDMO
                               from c in _INVContext.INV_Master_ItemDMO
                               from d in _INVContext.INV_Master_UOMDMO
                      
                               where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                           && a.MI_Id == data.MI_IdNew && b.INVMPR_Id == data.INVMPR_Id)
                               select new INV_PurchaseRequisitionDTO
                               {
                                   INVTPR_Id = b.INVTPR_Id,
                                   INVMPR_Id = a.INVMPR_Id,
                                   INVMI_Id = b.INVMI_Id,
                                   INVMUOM_Id = b.INVMUOM_Id,                                  
                                   INVMPR_PRNo = a.INVMPR_PRNo,
                                   INVMI_ItemName = c.INVMI_ItemName,
                                   INVMUOM_UOMName = d.INVMUOM_UOMName,                                  
                                   INVTPR_PRQty = b.INVTPR_PRQty,
                                   INVTPR_PRUnitRate = b.INVTPR_PRUnitRate,
                                   INVTPR_ApproxAmount = b.INVTPR_ApproxAmount,
                                   INVTPR_ApprovedQty = b.INVTPR_ApprovedQty,
                                   INVMPR_PRDate = a.INVMPR_PRDate,
                                   INVMPR_Remarks = a.INVMPR_Remarks,
                                   INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                   INVTPR_Remarks = b.INVTPR_Remarks,
                                   INVTPR_ActiveFlg = b.INVTPR_ActiveFlg,
                                   INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                   INVMPR_ActiveFlg = a.INVMPR_ActiveFlg

                               }).Distinct().OrderBy(i => i.INVTPR_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition edit:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseRequisitionDTO deactiveM(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_PurchaseRequisitionDMO.Single(t => t.INVMPR_Id == data.INVMPR_Id && t.MI_Id== data.MI_IdNew);

                if (result.INVMPR_ActiveFlg == true)
                {
                    result.INVMPR_ActiveFlg = false;
                }
                else if (result.INVMPR_ActiveFlg == false)
                {
                    result.INVMPR_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_PurchaseRequisitionDMO.Where(t => t.INVMPR_Id == data.INVMPR_Id).Distinct().ToArray();
                if (resultt != null)
                {
                    for (int j = 0; j < resultt.Length; j++)
                    {
                        if (result.INVMPR_ActiveFlg == true)
                        {
                            resultt[j].INVTPR_ActiveFlg = true;                          
                        }
                        if (result.INVMPR_ActiveFlg == false)
                        {
                            resultt[j].INVTPR_ActiveFlg = false;
                        }
                        resultt[j].UpdatedDate = DateTime.Now;
                        resultt[j].INVTPR_UpdatedBy = data.UserId;
                        _INVContext.Update(resultt[j]);
                    }
                }
                //foreach (var rt in resultt)
                //{
                //    var resultA = _INVContext.INV_T_PurchaseRequisitionDMO.Single(t => t.INVMPR_Id == rt.INVMPR_Id);
                //    if (result.INVMPR_ActiveFlg == true)
                //    {
                //        resultA.INVTPR_ActiveFlg = true;
                //        resultA.UpdatedDate = DateTime.Now;
                //        _INVContext.Update(resultA);
                //    }
                //    if (result.INVMPR_ActiveFlg == false)
                //    {
                //        resultA.INVTPR_ActiveFlg = false;
                //        resultA.UpdatedDate = DateTime.Now;
                //        _INVContext.Update(resultA);
                //    }
                //}

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

        public INV_PurchaseRequisitionDTO deactive(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_PurchaseRequisitionDMO.Single(t => t.INVTPR_Id == data.INVTPR_Id);

                if (result.INVTPR_ActiveFlg == true)
                {
                    result.INVTPR_ActiveFlg = false;
                }
                else if (result.INVTPR_ActiveFlg == false)
                {
                    result.INVTPR_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                int countactiveT = 0;
                int countactiveF = 0;
                var resultt = _INVContext.INV_T_PurchaseRequisitionDMO.Where(t => t.INVMPR_Id == data.INVMPR_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (rt.INVTPR_ActiveFlg == false)
                    {
                        countactiveF += 1;
                    }
                    else if (rt.INVTPR_ActiveFlg == true)
                    {
                        countactiveT += 1;
                    }
                }
                var resultmflg = _INVContext.INV_M_PurchaseRequisitionDMO.Single(t => t.INVMPR_Id == data.INVMPR_Id);
                if (countactiveF > 0 && countactiveT == 0)
                {
                    resultmflg.INVMPR_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }
                else if (countactiveT > 0 && countactiveF == 0)
                {
                    resultmflg.INVMPR_ActiveFlg = true;
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

        public INV_PurchaseRequisitionDTO GetOnchangeData(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _db.Master_Numbering.Where(t => t.MI_Id == data.MI_IdNew).ToList();
                data.transnumconfig = masnum.ToArray();

                data.get_mi_list = _INVContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.get_pidata = _INVContext.INV_T_PurchaseIndentDMO.Where(p => p.MI_Id == data.MI_IdNew && p.INVTPI_ActiveFlg == true).ToArray();

                data.get_item = (from a in _INVContext.INV_Master_ItemDMO
                                 from c in _INVContext.INV_Master_UOMDMO
                                 where (c.INVMUOM_Id == a.INVMUOM_Id && a.INVMI_ActiveFlg == true && a.MI_Id == data.MI_IdNew)
                                 select new INV_PurchaseRequisitionDTO
                                 {
                                     INVMI_Id = a.INVMI_Id,
                                     INVMUOM_Id = a.INVMUOM_Id,
                                     INVMI_ItemName = a.INVMI_ItemName,
                                     INVMI_ItemCode = a.INVMI_ItemCode,
                                 }).Distinct().OrderBy(i => i.INVMI_ItemName).ToArray();



                data.get_purchaserequisition = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                                from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                                where (a.INVMPR_Id == b.INVMPR_Id && a.MI_Id == data.MI_IdNew)
                                                select new INV_PurchaseRequisitionDTO
                                                {
                                                    INVMPR_Id = a.INVMPR_Id,
                                                    INVMPR_PRNo = a.INVMPR_PRNo,
                                                    INVMPR_PRDate = a.INVMPR_PRDate,
                                                    INVMPR_Remarks = a.INVMPR_Remarks,
                                                    INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                                    INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                                    INVMPR_ActiveFlg = a.INVMPR_ActiveFlg,
                                                    INVMPR_RejectFlg = a.INVMPR_RejectFlg
                                                }).Distinct().OrderBy(i => i.INVMPR_Id).ToArray();


            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition load Page:" + ex.Message);
            }
            return data;
        }
        
        public string emaildata(long INVMPR_Id,long HRME_Id,long MI_Id,long UserID)
        {
            string suss="";
            try
            {
              
                decimal total = 0;
                string str = "";
                var itemlist = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                from c in _INVContext.INV_Master_ItemDMO
                                from d in _INVContext.INV_Master_UOMDMO
                                where a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && b.INVTPR_ActiveFlg == true && a.INVMPR_Id == INVMPR_Id
                                select new INV_PurchaseRequisitionDTO
                                {
                                    INVMI_ItemName = c.INVMI_ItemName,
                                    INVTPR_Id = b.INVTPR_Id,
                                    INVTPR_PRQty = b.INVTPR_PRQty,
                                    INVTPR_PRUnitRate = b.INVTPR_PRUnitRate,
                                    INVTPR_ApproxAmount = b.INVTPR_ApproxAmount,
                                     INVTPR_Remarks = b.INVTPR_Remarks,
                                    INVMUOM_UOMName = d.INVMUOM_UOMName,
                                }
                              ).Distinct().ToList();

                if (itemlist.Count > 0)
                {
                        str += "<table width="+"'600px'"+" style=" + "'text-align: center;line-height:2;border:0;cellspacing:0;cellpadding: 0;background-color:#F5F5F5'" + "><tr>";

                    str += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'> SL.NO. </th>";
                    str += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "ITEM" + "</th>";
                    str += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "UOM" + "</th>";
                    str += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "QUANTITY" + "</th>";
                    str += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "PRICE/UNIT" + "</th>";
                    str += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "AMOUNT" + "</th>";
                    str += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "REMARKS" + "</th>";
                    str += "</tr>";
                    int rownumber = 0;

                    for (int j = 0; j < itemlist.Count; j++)
                    {
                        rownumber += 1;
                        total += itemlist[j].INVTPR_ApproxAmount;
                        str += "<tr>" +
                      "<td style="+"'vertical-align:top'"+"" + " >" + rownumber + "</td> ";
                        str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].INVMI_ItemName + "</td>";
                        str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].INVMUOM_UOMName + "</td>";
                        str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].INVTPR_PRQty + "</td>";
                        str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].INVTPR_PRUnitRate + "</td>";
                        str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].INVTPR_ApproxAmount + "</td>";
                        str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].INVTPR_Remarks + "</td>";
                        str += "</tr>";
                    }
                    
                    str += "</table>";


                    var tomailid = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == HRME_Id && a.HRMEM_DeFaultFlag == "default").ToList();

                    if (tomailid.Count>0)
                    {
                    suss=sendmail(MI_Id, UserID, "PRTEMPLATE", tomailid.FirstOrDefault().HRMEM_EmailId, str, INVMPR_Id);
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
        public string sendmail(long MI_Id, long UserID, string Template, string emailid,string items,long INVMPR_Id)
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
                            Value = INVMPR_Id
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
