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
using System.Net;
using System.IO;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using DomainModel.Model;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class OrderApprovalImpl : Interface.OrderApprovalInterface
    {
        public InventoryContext _INVContext;
        ILogger<OrderApprovalImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public OrderApprovalImpl(InventoryContext InvContext, ILogger<OrderApprovalImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public OrderApprovalDTO getloaddata(OrderApprovalDTO data)
        {
            try
            {
                var prflg = _INVContext.INV_ConfigurationDMO.Where(c => c.MI_Id == data.MI_Id && c.INVC_ProcessApplFlg == true).ToList();
                if (prflg.Count > 0)
                {
                    data.prflag = prflg.FirstOrDefault().INVC_PRApplicableFlg;
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PurchageOrder_Approval";
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
                        data.get_prNo = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_ItemName).ToArray();




            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<OrderApprovalDTO> getpidetails(OrderApprovalDTO data)
        {
            try
            {

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PurchageOrderItemsForApproval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMPO_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.INVMPO_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_pimodel = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.viewcomments = (from a in _INVContext.INV_M_PurchaseOrder_ApprovalDMO
                                     from b in _INVContext.ApplicationUser
                                     where (a.INVMPOAPP_CreatedBy == b.Id && a.INVMPO_Id == data.INVMPO_Id && a.INVMPOAPP_ActiveFlg == true)
                                     select new OrderApprovalDTO
                                     {
                                         UserName = b.UserName,
                                         INVMPOAPP_Remarks = a.INVMPOAPP_Remarks,
                                         INVMPOAPP_PODate = a.INVMPOAPP_PODate,
                                     }).Distinct().OrderBy(a => a.INVMPOAPP_PODate).ToArray();

            }

            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent Item details:" + ex.Message);
            }
            return data;
        }
        public OrderApprovalDTO getprDetail(OrderApprovalDTO data)
        {
            try
            {
                data.get_indentDetail = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                         from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                         from c in _INVContext.INV_Master_ItemDMO
                                         from d in _INVContext.INV_Master_UOMDMO
                                         from e in _INVContext.MasterEmployee
                                         where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && a.HRME_Id == e.HRME_Id && b.INVMUOM_Id == d.INVMUOM_Id && a.INVMPR_ActiveFlg == true && c.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVMPR_Id == data.INVMPR_Id)
                                         select new INV_PurchaseRequisitionDTO
                                         {
                                             INVMPR_Id = a.INVMPR_Id,
                                             INVTPR_Id = b.INVTPR_Id,
                                             INVMI_Id = b.INVMI_Id,
                                             INVMUOM_Id = b.INVMUOM_Id,
                                             HRME_Id = a.HRME_Id,
                                             INVMPR_PRNo = a.INVMPR_PRNo,
                                             INVMI_ItemName = c.INVMI_ItemName,
                                             INVMI_ItemCode = c.INVMI_ItemCode,
                                             INVMUOM_UOMName = d.INVMUOM_UOMName,
                                             employeename = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : " " + e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),
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
                _logInv.LogInformation("Purchase Indent PR Change:" + ex.Message);
            }
            return data;
        }
        public OrderApprovalDTO get_details(OrderApprovalDTO data)
        {
            try
            {
                data.get_PIdetails = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                      from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                      from c in _INVContext.INV_Master_ItemDMO
                                      from d in _INVContext.INV_Master_UOMDMO
                                      where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && a.INVMPR_ActiveFlg == true && c.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVMPR_Id == data.INVMPR_Id)
                                      select new INV_PurchaseRequisitionDTO
                                      {
                                          INVMPR_Id = a.INVMPR_Id,
                                          INVTPR_Id = b.INVTPR_Id,
                                          INVMI_Id = b.INVMI_Id,
                                          INVMUOM_Id = b.INVMUOM_Id,
                                          HRME_Id = a.HRME_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMI_ItemCode = c.INVMI_ItemCode,
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
                _logInv.LogInformation("Purchase Indent PR Change:" + ex.Message);
            }
            return data;
        }
        public OrderApprovalDTO savedetails(OrderApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var indlist = _INVContext.INV_M_PurchaseOrderDMO.Where(w => w.INVMPO_Id == data.INVMPO_Id).ToList();


                if (indlist.Count > 0)
                {
                    INV_M_PurchaseOrder_ApprovalDMO obj = new INV_M_PurchaseOrder_ApprovalDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.INVMPO_Id = data.INVMPO_Id;
                    obj.INVMPOAPP_PONo = indlist.FirstOrDefault().INVMPO_PONo;
                    obj.INVMPOAPP_PODate = indianTime;
                    obj.INVMPOAPP_Remarks = data.INVTPO_Remarks;
                    obj.INVMPOAPP_ReferenceNo = indlist.FirstOrDefault().INVMPO_ReferenceNo;
                    obj.INVMPOAPP_TotAmount = data.INVMPO_TotAmount;
                    obj.INVMPOAPP_TotRate = data.INVMPO_TotRate;
                    obj.INVMPOAPP_TotTax = data.INVMPO_TotTax;
                    obj.INVMPOAPP_POTemplate = indlist.FirstOrDefault().INVMPO_POTemplate;
                    obj.INVMS_Id = indlist.FirstOrDefault().INVMS_Id;
                    obj.INVMSQ_Id = indlist.FirstOrDefault().INVMSQ_Id;
                    obj.INVMPOAPP_POTemplate = indlist.FirstOrDefault().INVMPO_POTemplate;


                    if (data.approvecnt > 0)
                    {
                        obj.INVMPOAPP_RejectFlg = false;
                    }
                    else
                    {
                        obj.INVMPOAPP_RejectFlg = true;
                    }

                    obj.INVMPOAPP_ApprovedBy = data.UserId;
                    obj.INVMPOAPP_ActiveFlg = true;
                    obj.INVMPOAPP_CreatedBy = data.UserId;
                    obj.INVMPOAPP_UpdatedBy = data.UserId;
                    obj.INVMPOAPP_CreatedDate = indianTime;
                    obj.INVMPOAPP_UpdatedDate = indianTime;
                    _INVContext.Add(obj);


                    if (data.arrayPI != null)
                    {
                        foreach (var item in data.arrayPI)
                        {
                            INV_T_PurchaseOrder_ApprovalDMO tobj = new INV_T_PurchaseOrder_ApprovalDMO();

                            tobj.MI_Id = data.MI_Id;
                            tobj.INVMPOAPP_Id = obj.INVMPOAPP_Id;


                            tobj.INVMPI_Id = item.INVMPI_Id;
                            tobj.INVTPO_Id = item.INVTPO_Id;

                            tobj.INVMI_Id = item.INVMI_Id;
                            tobj.INVMUOM_Id = item.INVMUOM_Id;


                            tobj.INVTPOAPP_POQty = item.INVTPO_POQty;
                            tobj.INVTPOAPP_ApprovedQty = item.INVTPO_ApproveQty;
                            tobj.INVTPOAPP_RatePerUnit = item.INVTPO_RatePerUnit;
                            tobj.INVTPOAPP_TaxAmount = item.INVTPO_TaxAmount;
                            tobj.INVTPOAPP_Amount = item.INVTPO_Amount;

                            if (item.flag == "R")
                            {
                                tobj.INVTPOAPP_RejectFlg = true;
                            }
                            else
                            {
                                tobj.INVTPOAPP_RejectFlg = false;
                            }

                            tobj.INVTPOAPP_Remarks = item.INVTPO_Remarks;
                            tobj.INVTPOAPP_ActiveFlg = true;
                            tobj.INVTPOAPP_CreatedBy = data.UserId;
                            tobj.INVTPOAPP_UpdatedBy = data.UserId;
                            tobj.INVTPOAPP_CreatedDate = indianTime; ;
                            tobj.INVTPOAPP_UpdatedDate = indianTime;

                            _INVContext.Add(tobj);
                        }
                    }


                    int ext = _INVContext.SaveChanges();
                    if (ext > 0)
                    {
                        var checkmaxorder = (from a in _INVContext.HR_PROCESSDMO
                                             from b in _INVContext.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PO"
                                             select b.HRPAON_SanctionLevelNo).Max();

                        var checkuserorder = (from a in _INVContext.HR_PROCESSDMO
                                              from b in _INVContext.HR_Process_Auth_OrderNoDMO
                                              where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PO" && b.IVRMUL_Id == data.UserId
                                              select b.HRPAON_SanctionLevelNo).Max();


                        if (checkmaxorder == checkuserorder)
                        {
                            var indentres = _INVContext.INV_M_PurchaseOrderDMO.Single(a => a.INVMPO_Id == data.INVMPO_Id);


                            indentres.INVMPO_UpdatedBy = data.UserId;
                            indentres.UpdatedDate = indianTime;
                            indentres.INVMPO_FinalProcessFlag = true;
                            if (data.approvecnt > 0)
                            {
                                indentres.INVMPO_RejectFlg = false;
                                indentres.INVMPO_TotAmount = data.INVMPO_TotAmount;
                                indentres.INVMPO_TotRate = data.INVMPO_TotRate;
                                indentres.INVMPO_TotTax = data.INVMPO_TotTax;

                            }
                            else
                            {
                                indentres.INVMPO_RejectFlg = true;
                                indentres.INVMPO_TotAmount = 0;
                                indentres.INVMPO_TotRate = data.INVMPO_TotRate;
                                indentres.INVMPO_TotTax = data.INVMPO_TotTax;
                            }
                            _INVContext.Update(indentres);
                            var indentresitem = _INVContext.INV_T_PurchaseOrderDMO.Where(a => a.INVMPO_Id == data.INVMPO_Id).ToList();
                            if (indentresitem.Count > 0)
                            {

                                foreach (var item in indentresitem)
                                {

                                    foreach (var ap in data.arrayPI)
                                    {
                                        if (ap.INVTPO_Id == item.INVTPO_Id)
                                        {

                                            item.INVTPO_RatePerUnit = ap.INVTPO_RatePerUnit;
                                            item.INVTPO_TaxAmount = ap.INVTPO_TaxAmount;

                                            if (ap.flag == "R")
                                            {
                                                item.INVTPO_Amount = 0;
                                                item.INVTPO_ApproveQty = 0;
                                                item.INVTPO_RejectFlg = true;
                                            }
                                            else
                                            {
                                                item.INVTPO_Amount = ap.INVTPO_Amount;
                                                item.INVTPO_ApproveQty = ap.INVTPO_ApproveQty;
                                                item.INVTPO_RejectFlg = false;
                                            }

                                            item.INVTPO_UpdatedBy = data.UserId;
                                            item.UpdatedDate = indianTime;

                                            _INVContext.Update(item);
                                        }

                                    }

                                }

                            }




                            _INVContext.SaveChanges();

                        }

                        data.returnval = true;


                        var primaryid = _INVContext.INV_M_PurchaseOrder_ApprovalDMO.Where(e => e.INVMPOAPP_Id == obj.INVMPOAPP_Id).ToList();
                        if (primaryid.Count > 0)
                        {
                            long hrid = 0;

                            var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                            if (hrmeid1.Count > 0)
                            {
                                hrid = hrmeid1.FirstOrDefault().Emp_Code;
                            }

                            emaildata(primaryid.FirstOrDefault().INVMPOAPP_Id, hrid, data.MI_Id, data.UserId);
                        }
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
                _logInv.LogInformation("Purchase ORDER savedata :" + ex.Message);
            }

            return data;
        }
        public OrderApprovalDTO Rejectorder(OrderApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var indlist = _INVContext.INV_M_PurchaseOrderDMO.Where(w => w.INVMPO_Id == data.INVMPO_Id).ToList();


                if (indlist.Count > 0)
                {
                    INV_M_PurchaseOrder_ApprovalDMO obj = new INV_M_PurchaseOrder_ApprovalDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.INVMPO_Id = data.INVMPO_Id;
                    obj.INVMPOAPP_PONo = indlist.FirstOrDefault().INVMPO_PONo;
                    obj.INVMPOAPP_PODate = indianTime;
                    obj.INVMPOAPP_Remarks = data.INVTPO_Remarks;
                    obj.INVMPOAPP_ReferenceNo = indlist.FirstOrDefault().INVMPO_ReferenceNo;
                    obj.INVMPOAPP_TotAmount = 0;
                    obj.INVMPOAPP_TotRate = 0;
                    obj.INVMPOAPP_TotTax = 0;
                    obj.INVMPOAPP_POTemplate = indlist.FirstOrDefault().INVMPO_POTemplate;
                    obj.INVMS_Id = indlist.FirstOrDefault().INVMS_Id;
                    obj.INVMSQ_Id = indlist.FirstOrDefault().INVMSQ_Id;
                    obj.INVMPOAPP_POTemplate = indlist.FirstOrDefault().INVMPO_POTemplate;

                    obj.INVMPOAPP_RejectFlg = true;


                    obj.INVMPOAPP_ApprovedBy = data.UserId;
                    obj.INVMPOAPP_ActiveFlg = true;
                    obj.INVMPOAPP_CreatedBy = data.UserId;
                    obj.INVMPOAPP_UpdatedBy = data.UserId;
                    obj.INVMPOAPP_CreatedDate = indianTime;
                    obj.INVMPOAPP_UpdatedDate = indianTime;
                    _INVContext.Add(obj);


                    if (data.arrayPI != null)
                    {
                        foreach (var item in data.arrayPI)
                        {
                            INV_T_PurchaseOrder_ApprovalDMO tobj = new INV_T_PurchaseOrder_ApprovalDMO();

                            tobj.MI_Id = data.MI_Id;
                            tobj.INVMPOAPP_Id = obj.INVMPOAPP_Id;


                            tobj.INVMPI_Id = item.INVMPI_Id;
                            tobj.INVTPO_Id = item.INVTPO_Id;

                            tobj.INVMI_Id = item.INVMI_Id;
                            tobj.INVMUOM_Id = item.INVMUOM_Id;


                            tobj.INVTPOAPP_POQty = item.INVTPO_POQty;
                            tobj.INVTPOAPP_ApprovedQty = item.INVTPO_ApproveQty;
                            tobj.INVTPOAPP_RatePerUnit = item.INVTPO_RatePerUnit;
                            tobj.INVTPOAPP_TaxAmount = item.INVTPO_TaxAmount;
                            tobj.INVTPOAPP_Amount = item.INVTPO_Amount;
                            tobj.INVTPOAPP_RejectFlg = true;

                            tobj.INVTPOAPP_Remarks = item.INVTPO_Remarks;
                            tobj.INVTPOAPP_ActiveFlg = true;
                            tobj.INVTPOAPP_CreatedBy = data.UserId;
                            tobj.INVTPOAPP_UpdatedBy = data.UserId;
                            tobj.INVTPOAPP_CreatedDate = indianTime; ;
                            tobj.INVTPOAPP_UpdatedDate = indianTime;

                            _INVContext.Add(tobj);
                        }
                    }


                    int ext = _INVContext.SaveChanges();
                    if (ext > 0)
                    {
                        var checkmaxorder = (from a in _INVContext.HR_PROCESSDMO
                                             from b in _INVContext.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PO"
                                             select b.HRPAON_SanctionLevelNo).Max();

                        var checkuserorder = (from a in _INVContext.HR_PROCESSDMO
                                              from b in _INVContext.HR_Process_Auth_OrderNoDMO
                                              where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PO" && b.IVRMUL_Id == data.UserId
                                              select b.HRPAON_SanctionLevelNo).Max();


                        if (checkmaxorder == checkuserorder)
                        {
                            var indentres = _INVContext.INV_M_PurchaseOrderDMO.Single(a => a.INVMPO_Id == data.INVMPO_Id);


                            indentres.INVMPO_UpdatedBy = data.UserId;
                            indentres.UpdatedDate = indianTime;
                            indentres.INVMPO_FinalProcessFlag = true;
                            //if (data.approvecnt > 0)
                            //{
                            //    indentres.INVMPO_RejectFlg = false;
                            //    indentres.INVMPO_TotAmount = data.INVMPO_TotAmount;
                            //    indentres.INVMPO_TotRate = data.INVMPO_TotRate;
                            //    indentres.INVMPO_TotTax = data.INVMPO_TotTax;

                            //}
                            //else
                            //{
                            indentres.INVMPO_RejectFlg = true;
                            indentres.INVMPO_TotAmount = 0;
                            indentres.INVMPO_TotRate = 0;
                            indentres.INVMPO_TotTax = 0;
                            //}
                            _INVContext.Update(indentres);
                            var indentresitem = _INVContext.INV_T_PurchaseOrderDMO.Where(a => a.INVMPO_Id == data.INVMPO_Id).ToList();
                            if (indentresitem.Count > 0)
                            {

                                foreach (var item in indentresitem)
                                {

                                    foreach (var ap in data.arrayPI)
                                    {
                                        if (ap.INVTPO_Id == item.INVTPO_Id)
                                        {

                                            item.INVTPO_RatePerUnit = ap.INVTPO_RatePerUnit;
                                            item.INVTPO_TaxAmount = ap.INVTPO_TaxAmount;

                                            //if (ap.flag == "R")
                                            //{
                                            item.INVTPO_Amount = 0;
                                            item.INVTPO_ApproveQty = 0;
                                            item.INVTPO_RejectFlg = true;
                                            //}
                                            //else
                                            //{
                                            //    item.INVTPO_Amount = ap.INVTPO_Amount;
                                            //    item.INVTPO_ApproveQty = ap.INVTPO_ApproveQty;
                                            //    item.INVTPO_RejectFlg = false;
                                            //}

                                            item.INVTPO_UpdatedBy = data.UserId;
                                            item.UpdatedDate = indianTime;

                                            _INVContext.Update(item);
                                        }

                                    }

                                }

                            }




                            _INVContext.SaveChanges();

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
                _logInv.LogInformation("Purchase ORDER savedata :" + ex.Message);
            }

            return data;
        }
        public OrderApprovalDTO edit(OrderApprovalDTO data)
        {
            try
            {
                //data.get_editPI = (from a in _INVContext.INV_M_PurchaseIndentDMO
                //                   from b in _INVContext.INV_T_PurchaseIndentDMO
                //                   from c in _INVContext.INV_Master_ItemDMO
                //                   from d in _INVContext.INV_Master_UOMDMO
                //                   where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                //                            && a.INVMPI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVTPI_Id == data.INVTPI_Id)
                //                   select new OrderApprovalDTO
                //                   {
                //                       INVMPI_Id = a.INVMPI_Id,
                //                       INVMPI_PINo = a.INVMPI_PINo,
                //                       INVMI_Id = b.INVMI_Id,
                //                       INVMUOM_Id = b.INVMUOM_Id,
                //                       INVMPR_Id = b.INVMPR_Id,
                //                       INVMI_ItemName = c.INVMI_ItemName,
                //                       INVMUOM_UOMName = d.INVMUOM_UOMName,
                //                       INVMPI_PIDate = a.INVMPI_PIDate,
                //                       INVMPI_ReferenceNo = a.INVMPI_ReferenceNo,
                //                       INVTPI_PIQty = b.INVTPI_PIQty,
                //                       INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                //                       INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                //                       INVMPI_Remarks = a.INVMPI_Remarks,
                //                       INVMPI_ApproxTotAmount = a.INVMPI_ApproxTotAmount,
                //                       INVMPI_POCreatedFlg = a.INVMPI_POCreatedFlg,
                //                       INVMPI_ActiveFlg = a.INVMPI_ActiveFlg,
                //                       INVTPI_Id = b.INVTPI_Id,
                //                       INVTPI_Remarks = b.INVTPI_Remarks,
                //                       INVTPI_ActiveFlg = b.INVTPI_ActiveFlg
                //                   }).Distinct().OrderBy(i => i.INVMPI_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent edit:" + ex.Message);
            }
            return data;
        }
        public OrderApprovalDTO deactiveM(OrderApprovalDTO data)
        {
            //try
            //{
            //    var result = _INVContext.INV_M_PurchaseIndentDMO.Single(t => t.INVMPI_Id == data.INVMPI_Id);

            //    if (result.INVMPI_ActiveFlg == true)
            //    {
            //        result.INVMPI_ActiveFlg = false;
            //    }
            //    else if (result.INVMPI_ActiveFlg == false)
            //    {
            //        result.INVMPI_ActiveFlg = true;
            //    }
            //    result.UpdatedDate = DateTime.Now;
            //    _INVContext.Update(result);

            //    var resultt = _INVContext.INV_T_PurchaseIndentDMO.Where(t => t.INVMPI_Id == data.INVMPI_Id).ToList();
            //    if (resultt.Count > 0)
            //    {
            //        foreach (var rt in resultt)
            //        {
            //            // var rest = _INVContext.INV_T_PurchaseIndentDMO.Single(t => t.INVMPI_Id == rt.INVMPI_Id);
            //            if (result.INVMPI_ActiveFlg == true)
            //            {
            //                rt.INVTPI_ActiveFlg = true;
            //            }
            //            if (result.INVMPI_ActiveFlg == false)
            //            {
            //                rt.INVTPI_ActiveFlg = false;
            //            }
            //            rt.UpdatedDate = DateTime.Now;
            //            _INVContext.Update(rt);
            //        }
            //    }
            //    int returnval = _INVContext.SaveChanges();
            //    if (returnval > 0)
            //    {
            //        data.returnval = true;
            //    }
            //    else
            //    {
            //        data.returnval = false;
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return data;
        }
        public OrderApprovalDTO deactive(OrderApprovalDTO data)
        {
            //try
            //{
            //    var result = _INVContext.INV_T_PurchaseIndentDMO.Single(t => t.INVTPI_Id == data.INVTPI_Id);

            //    if (result.INVTPI_ActiveFlg == true)
            //    {
            //        result.INVTPI_ActiveFlg = false;
            //    }
            //    else if (result.INVTPI_ActiveFlg == false)
            //    {
            //        result.INVTPI_ActiveFlg = true;
            //    }

            //    result.UpdatedDate = DateTime.Now;
            //    _INVContext.Update(result);

            //    int countactiveT = 0;
            //    int countactiveF = 0;
            //    var resultt = _INVContext.INV_T_PurchaseIndentDMO.Where(t => t.INVMPI_Id == data.INVMPI_Id).ToList();
            //    foreach (var rt in resultt)
            //    {
            //        if (rt.INVTPI_ActiveFlg == false)
            //        {
            //            countactiveF += 1;
            //        }
            //        else if (rt.INVTPI_ActiveFlg == true)
            //        {
            //            countactiveT += 1;
            //        }
            //    }
            //    var resultmflg = _INVContext.INV_M_PurchaseIndentDMO.Single(t => t.INVMPI_Id == data.INVMPI_Id);
            //    if (countactiveF > 0 && countactiveT == 0)
            //    {
            //        resultmflg.INVMPI_ActiveFlg = false;
            //        result.UpdatedDate = DateTime.Now;
            //        _INVContext.Update(resultmflg);
            //    }
            //    else if (countactiveT > 0 && countactiveF == 0)
            //    {
            //        resultmflg.INVMPI_ActiveFlg = true;
            //        result.UpdatedDate = DateTime.Now;
            //        _INVContext.Update(resultmflg);
            //    }

            //    int returnval = _INVContext.SaveChanges();
            //    if (returnval > 0)
            //    {
            //        data.returnval = true;
            //    }
            //    else
            //    {
            //        data.returnval = false;
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return data;
        }
        public OrderApprovalDTO rejectallreq(OrderApprovalDTO data)
        {

            try
            {
                var rejlist = _INVContext.INV_M_PurchaseRequisitionDMO.Single(a => a.INVMPR_Id == data.INVMPR_Id);
                rejlist.INVMPR_RejectFlg = true;
                rejlist.UpdatedDate = DateTime.Now;
                rejlist.INVMPR_UpdatedBy = data.UserId;
                _INVContext.Update(rejlist);
                var rejitemlist = _INVContext.INV_T_PurchaseRequisitionDMO.Where(a => a.INVMPR_Id == data.INVMPR_Id).ToList();

                if (rejitemlist.Count > 0)
                {
                    foreach (var item in rejitemlist)
                    {
                        item.INVTPR_RejectFlg = true;
                        item.UpdatedDate = DateTime.Now;
                        item.INVTPR_UpdatedBy = data.UserId;
                        _INVContext.Update(item);
                    }
                }
                int cxt = _INVContext.SaveChanges();
                if (cxt > 0)
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

                throw ex;
            }
            return data;
        }
        public OrderApprovalDTO viewsavedteple(OrderApprovalDTO data)
        {
            try
            {

                List<OrderApprovalDTO> itemlistt = new List<OrderApprovalDTO>();
                decimal total = 0;
                string str = "";
                var checktemplate = _INVContext.SMSEmailSetting.Where(a => a.ISES_Template_Name.Trim() == data.INVMPO_POTemplate.Trim() && a.ISES_MailActiveFlag == true).ToList();
                if (checktemplate.Count == 0)
                {
                    data.message = "Email Template not Mapped to the selected Institution";
                }
                else
                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_PurchageOrderItemsForApproval_Template";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@User_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.UserId
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMPO_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.INVMPO_Id
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
                                    itemlistt.Add(new OrderApprovalDTO
                                    {
                                        INVMI_ItemName = (dataReader["INVMI_ItemName"].ToString()),
                                        INVTPO_ApproveQty = Convert.ToDecimal(dataReader["INVTPO_POQty"].ToString()),
                                        INVTPO_RatePerUnit = Convert.ToDecimal(dataReader["INVTPO_RatePerUnit"].ToString()),
                                        INVTPO_TaxAmount = Convert.ToDecimal(dataReader["INVTPO_TaxAmount"].ToString()),
                                        INVTPO_Amount = Convert.ToDecimal(dataReader["INVTPO_Amount"].ToString()),


                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }





                    if (itemlistt.Count > 0)
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
                        for (int j = 0; j < itemlistt.Count; j++)
                        {
                            rownumber += 1;


                            total += itemlistt[j].INVTPO_Amount;
                            if (data.columnids != null)
                            {

                                str += "<tr>" +
                              "<td" + " >" + rownumber + "</td> ";

                                foreach (var item1 in data.columnids)
                                {

                                    if (item1.coulmn == "ITEMNAME")
                                    {
                                        str += "<td >" + itemlistt[j].INVMI_ItemName + "</td>";
                                    }

                                    if (item1.coulmn == "QUANTITY")
                                    {
                                        str += "<td>" + itemlistt[j].INVTPO_ApproveQty + "</th>";
                                    }

                                    if (item1.coulmn == "PRICE/UNIT")
                                    {
                                        str += "<td>" + itemlistt[j].INVTPO_RatePerUnit + "</th>";
                                    }

                                    if (item1.coulmn == "AMOUNT")
                                    {
                                        str += "<td  >" + itemlistt[j].INVTPO_ApproveQty * itemlistt[j].INVTPO_RatePerUnit + "</th>";
                                    }

                                    if (item1.coulmn == "TAX")
                                    {
                                        str += "<td  >" + itemlistt[j].INVTPO_TaxAmount + "</th>";
                                    }
                                    if (item1.coulmn == "TOTALAMOUNT")
                                    {
                                        str += "<td  >" + itemlistt[j].INVTPO_Amount + "</th>";
                                    }

                                    if (item1.coulmn == "REMARKS")
                                    {
                                        str += "<td  >" + itemlistt[j].INVTPO_Remarks + "</th>";
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
        public OrderApprovalDTO viewtemplate(OrderApprovalDTO data)
        {
            try
            {


                decimal total = 0;
                string str = "";
                var checktemplate = _INVContext.SMSEmailSetting.Where(a => a.ISES_Template_Name.Trim() == data.INVMPO_POTemplate.Trim() && a.ISES_MailActiveFlag == true).ToList();
                if (checktemplate.Count == 0)
                {
                    data.message = "Email Template not Mapped to the selected Institution";
                }
                else
                {






                    if (data.arrayPI.Length > 0)
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
                        for (int j = 0; j < data.arrayPI.Length; j++)
                        {
                            if (data.arrayPI[j].flag == "A")
                            {
                                rownumber += 1;


                                total += data.arrayPI[j].INVTPO_Amount;
                                if (data.columnids != null)
                                {

                                    str += "<tr>" +
                                  "<td" + " >" + rownumber + "</td> ";

                                    foreach (var item1 in data.columnids)
                                    {

                                        if (item1.coulmn == "ITEMNAME")
                                        {
                                            str += "<td >" + data.arrayPI[j].INVMI_ItemName + "</td>";
                                        }

                                        if (item1.coulmn == "QUANTITY")
                                        {
                                            str += "<td>" + data.arrayPI[j].INVTPO_ApproveQty + "</th>";
                                        }

                                        if (item1.coulmn == "PRICE/UNIT")
                                        {
                                            str += "<td>" + data.arrayPI[j].INVTPO_RatePerUnit + "</th>";
                                        }

                                        if (item1.coulmn == "AMOUNT")
                                        {
                                            str += "<td  >" + data.arrayPI[j].INVTPO_ApproveQty * data.arrayPI[j].INVTPO_RatePerUnit + "</th>";
                                        }

                                        if (item1.coulmn == "TAX")
                                        {
                                            str += "<td  >" + data.arrayPI[j].INVTPO_TaxAmount + "</th>";
                                        }
                                        if (item1.coulmn == "TOTALAMOUNT")
                                        {
                                            str += "<td  >" + data.arrayPI[j].INVTPO_Amount + "</th>";
                                        }

                                        if (item1.coulmn == "REMARKS")
                                        {
                                            str += "<td  >" + data.arrayPI[j].INVTPO_Remarks + "</th>";
                                        }
                                    }

                                    str += "</tr>"; ;
                                }
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
        public OrderApprovalDTO OnClickIndividualViewComments(OrderApprovalDTO data)
        {
            try
            {
                data.indiviewcomments = (from a in _INVContext.INV_T_PurchaseOrder_ApprovalDMO
                                         from b in _INVContext.ApplicationUser
                                         where (a.INVTPOAPP_CreatedBy == b.Id && a.INVTPO_Id == data.INVTPO_Id)
                                         select new OrderApprovalDTO
                                         {
                                             UserName = b.UserName,
                                             INVMPOAPP_Remarks = a.INVTPOAPP_Remarks,
                                             INVMPOAPP_PODate = a.INVTPOAPP_CreatedDate,

                                         }).Distinct().OrderBy(a => a.INVMPOAPP_PODate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string emaildata(long INVMPOAPP_Id, long HRME_Id, long MI_Id, long UserID)
        {
            string suss = "";
            try
            {

                decimal total = 0;
                string str = "";
                var itemlist = (from a in _INVContext.INV_M_PurchaseOrder_ApprovalDMO
                                from b in _INVContext.INV_T_PurchaseOrder_ApprovalDMO
                                from c in _INVContext.INV_Master_ItemDMO
                                from d in _INVContext.INV_Master_UOMDMO
                                where a.INVMPOAPP_Id == b.INVMPOAPP_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && b.INVTPOAPP_ActiveFlg == true && a.INVMPOAPP_Id == INVMPOAPP_Id && b.INVTPOAPP_RejectFlg == false
                                select new IndentApprovalDTO
                                {
                                    INVMI_ItemName = c.INVMI_ItemName,
                                    INVMPIAPP_Id = b.INVMPOAPP_Id,
                                    INVTPIAPP_Id = b.INVTPOAPP_Id,
                                    INVTPI_PRQty = b.INVTPOAPP_POQty,
                                    INVTPI_PIQty = b.INVTPOAPP_ApprovedQty,
                                    INVTPI_PIUnitRate = b.INVTPOAPP_RatePerUnit,
                                    INVTPI_ApproxAmount = b.INVTPOAPP_Amount,
                                    INVMPI_Remarks = b.INVTPOAPP_Remarks,
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
                    str += "<th>" + "APPROVED QUANTITY" + "</th>";
                    str += "<th>" + "PRICE/UNIT" + "</th>";
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
                        str += "<td>" + itemlist[j].INVTPI_PRQty + "</th>";
                        str += "<td>" + itemlist[j].INVTPI_PIQty + "</th>";
                        str += "<td>" + itemlist[j].INVTPI_PIUnitRate + "</th>";
                        str += "<td>" + itemlist[j].INVTPI_ApproxAmount + "</th>";
                        str += "<td  >" + itemlist[j].INVMPI_Remarks + "</th>";
                        str += "</tr>";
                    }

                    str += "</table>";


                    var tomailid = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == HRME_Id && a.HRMEM_DeFaultFlag == "default").ToList();
                    String ccmail = "";
                    if (tomailid.Count > 0)
                    {
                        ccmail = tomailid.FirstOrDefault().HRMEM_EmailId;
                    }

                    var approvallist = (from a in _db.HR_PROCESSDMO
                                        from b in _db.HR_Process_Auth_OrderNoDMO
                                        where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PO" && b.IVRMUL_Id == UserID
                                        select new IndentApprovalDTO
                                        {
                                            UserId = b.IVRMUL_Id,
                                            HRPAON_SanctionLevelNo = b.HRPAON_SanctionLevelNo
                                        }).Distinct().ToList();


                    if (approvallist.Count > 0)
                    {

                        var nextleveluser = (from a in _db.HR_PROCESSDMO
                                             from b in _db.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PO" && b.HRPAON_SanctionLevelNo == approvallist.FirstOrDefault().HRPAON_SanctionLevelNo + 1
                                             select new IndentApprovalDTO
                                             {
                                                 UserId = b.IVRMUL_Id,
                                                 HRPAON_SanctionLevelNo = b.HRPAON_SanctionLevelNo
                                             }).Distinct().ToList();


                        if (nextleveluser.Count > 0)
                        {
                            long apprhrmeid = 0;
                            var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == nextleveluser.FirstOrDefault().UserId).ToList();
                            if (hrmeid1.Count > 0)
                            {
                                apprhrmeid = hrmeid1.FirstOrDefault().Emp_Code;
                            }

                            var tomailidapp = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == apprhrmeid && a.HRMEM_DeFaultFlag == "default").ToList();



                            if (tomailidapp.Count > 0)
                            {
                                if (tomailidapp.FirstOrDefault().HRMEM_EmailId != null && tomailidapp.FirstOrDefault().HRMEM_EmailId != "")
                                {
                                    suss = sendmail(MI_Id, UserID, "STARTPOAPPROVAL", tomailidapp.FirstOrDefault().HRMEM_EmailId, str, INVMPOAPP_Id, ccmail);
                                }
                            }
                        }
                        else
                        {
                            suss = sendmail(MI_Id, UserID, "ENDPOAPPROVAL", ccmail, str, INVMPOAPP_Id, "");
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
        public string sendmail(long MI_Id, long UserID, string Template, string emailid, string items, long INVMPI_Id, string ccemailid)
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

                        if (ccemailid != null && ccemailid != "")
                        {
                            message.AddCc(ccemailid);
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
