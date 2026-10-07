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

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_T_GRNImpl : Interface.INV_T_GRNInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_T_GRNImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_T_GRNImpl(InventoryContext InvContext, ILogger<INV_T_GRNImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_T_GRNDTO getloaddata(INV_T_GRNDTO data)
        {
            try
            {
                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_ItemName).ToArray();
                data.get_Store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();
                data.get_tax = _INVContext.INV_Master_TaxDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMT_ActiveFlg == true).OrderBy(m => m.INVMT_Id).ToArray();
                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_Id).ToArray();

                data.Group = _INVContext.INV_Master_GroupDMO.Where(R => R.INVMG_ActiveFlg == true && R.MI_Id == data.MI_Id).Distinct().ToArray();

                data.get_GRN = (from a in _INVContext.INV_M_GRNDMO
                                from b in _INVContext.INV_Master_StoreDMO
                                from c in _INVContext.INV_M_GRN_StoreDMO
                                where a.INVMGRN_Id == c.INVMGRN_Id && b.INVMST_Id == c.INVMST_Id && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id
                                select new INV_T_GRNDTO
                                {
                                    INVMGRN_Id = a.INVMGRN_Id,
                                    INVMS_Id = a.INVMS_Id,
                                    INVMGRN_GRNNo = a.INVMGRN_GRNNo,
                                    INVMGRN_InvoiceNo = a.INVMGRN_InvoiceNo,
                                    INVMGRN_PurchaseDate = a.INVMGRN_PurchaseDate,
                                    INVMGRN_PurchaseValue = a.INVMGRN_PurchaseValue,
                                    INVMGRN_TotDiscount = a.INVMGRN_TotDiscount,
                                    INVMGRN_TotTaxAmt = a.INVMGRN_TotTaxAmt,
                                    INVMGRN_TotalAmount = a.INVMGRN_TotalAmount,
                                    INVMGRN_ActiveFlg = a.INVMGRN_ActiveFlg,
                                    INVMGRN_POReference = a.INVMGRN_POReference,
                                    INVMGRN_EWayBillRef = a.INVMGRN_EWayBillRef,
                                    INVMGRN_TowardsProjects = a.INVMGRN_TowardsProjects,
                                    INVMS_StoreName = b.INVMS_StoreName,
                                    INVMS_SupplierName = _INVContext.INV_Master_SupplierDMO.Where(w => w.INVMS_Id == a.INVMS_Id && w.MI_Id == data.MI_Id && a.INVMS_Id != null).FirstOrDefault().INVMS_SupplierName
                                }).OrderByDescending(m => m.INVMGRN_Id).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                _logInv.LogInformation("GRN load Page:" + ex.Message);
            }
            return data;
        }

        public INV_T_GRNDTO getitemDetail(INV_T_GRNDTO data)
        {
            try
            {
                if(data.INVMI_Id > 0)
                {
                    data.get_itemDetail = (from a in _INVContext.INV_Master_ItemDMO
                                           from b in _INVContext.INV_Master_UOMDMO
                                           where (a.INVMUOM_Id == b.INVMUOM_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMI_Id == data.INVMI_Id && a.INVMI_ActiveFlg == true)
                                           select new INV_T_GRNDTO
                                           {
                                               INVMI_Id = a.INVMI_Id,
                                               INVMI_ItemName = a.INVMI_ItemName,
                                               INVMUOM_Id = b.INVMUOM_Id,
                                               INVMUOM_UOMName = b.INVMUOM_UOMName,
                                               INVMUOM_UOMAliasName = b.INVMUOM_UOMAliasName

                                           }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();

                    data.get_itemTax = (from a in _INVContext.INV_Master_ItemDMO
                                        from b in _INVContext.INV_Master_Item_TaxDMO
                                        from c in _INVContext.INV_Master_TaxDMO
                                        where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.MI_Id == c.MI_Id && a.INVMI_Id == data.INVMI_Id && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true)
                                        select new INV_T_GRNDTO
                                        {
                                            INVMI_Id = a.INVMI_Id,
                                            INVMI_ItemName = a.INVMI_ItemName,
                                            INVMT_Id = b.INVMT_Id,
                                            INVMT_TaxName = c.INVMT_TaxName,
                                            INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                            INVMIT_TaxValue = b.INVMIT_TaxValue

                                        }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();
                }
                else if(data.INVTGRN_Id  > 0)
                {
                    data.get_itemTax = _INVContext.INV_T_GRN_TaxDMO.Where(R => R.INVTGRN_Id == data.INVTGRN_Id).Distinct().ToArray();
                }
        
                else  if(data.INVMS_Id > 0)
                {
                    data.GrnFiles = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                              from b in _INVContext.INV_T_PurchaseOrderDMO
                                              from c in _INVContext.INV_T_PurchaseIndentDMO
                                              from d in _INVContext.INV_M_PurchaseRequisitionDMO
                                              from e in _INVContext.MasterEmployee
                                              from g in _INVContext.HR_Master_Department
                                              where a.MI_Id == data.MI_Id && a.INVMPO_Id == b.INVMPO_Id && c.INVMPI_Id == b.INVMPI_Id && a.INVMPO_ActiveFlg==true && a.INVMS_Id== data.INVMS_Id && d.HRME_Id == e.HRME_Id && g.HRMD_Id == e.HRMD_Id && d.INVMPR_Id == c.INVMPR_Id
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
                                                  INVMPO_POTemplate = a.INVMPO_POTemplate,
                                                  INVMPO_ActiveFlg = a.INVMPO_ActiveFlg,
                                                  INVMPO_Id = a.INVMPO_Id,
                                                  INVMS_Id = a.INVMS_Id

                                              }).Distinct().OrderByDescending(o => o.INVMPO_Id).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("GRN load Page:" + ex.Message);
            }
            return data;
        }


        public INV_T_GRNDTO Edit_GRN_details(INV_T_GRNDTO data)
        {
            try
            {
                data.edit_GRN_Details_List = (from a in _INVContext.INV_T_GRNDMO
                                              from b in _INVContext.INV_M_GRNDMO
                                              from c in _INVContext.INV_Master_ItemDMO
                                              from d in _INVContext.INV_Master_UOMDMO
                                              where (a.INVMGRN_Id == b.INVMGRN_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && a.INVMGRN_Id == data.INVMGRN_Id)
                                              select new INV_T_GRNDTO
                                              {

                                                  INVMGRN_Id = a.INVMGRN_Id,
                                                  INVTGRN_Id = a.INVTGRN_Id,
                                                  INVMG_Id = c.INVMG_Id,
                                                  INVMGRN_GRNNo = b.INVMGRN_GRNNo,
                                                  INVMI_Id = a.INVMI_Id,
                                                  INVMUOM_Id = a.INVMUOM_Id,
                                                  INVMI_ItemName = c.INVMI_ItemName,
                                                  INVMUOM_UOMName = d.INVMUOM_UOMName,
                                                  INVTGRN_BatchNo = a.INVTGRN_BatchNo,
                                                  INVTGRN_PurchaseRate = a.INVTGRN_PurchaseRate,
                                                  INVTGRN_MRP = a.INVTGRN_MRP,
                                                  INVTGRN_SalesPrice = a.INVTGRN_SalesPrice,
                                                  INVTGRN_DiscountAmt = a.INVTGRN_DiscountAmt,
                                                  INVTGRN_TaxAmt = a.INVTGRN_TaxAmt,
                                                  INVTGRN_Amount = a.INVTGRN_Amount,
                                                  INVTGRN_Qty = a.INVTGRN_Qty,
                                                  INVTGRN_Naration = a.INVTGRN_Naration,
                                                  INVTGRN_MfgDate = a.INVTGRN_MfgDate,
                                                  INVTGRN_ExpDate = a.INVTGRN_ExpDate,
                                                  INVMGRN_EWayBillRef = b.INVMGRN_EWayBillRef,
                                                  INVMGRN_POReference = b.INVMGRN_POReference,
                                                  INVMGRN_TowardsProjects = b.INVMGRN_TowardsProjects,
                                                  INVTGRN_ActiveFlg = a.INVTGRN_ActiveFlg,
                                                  INVTGRN_AssetTypeFlag = a.INVTGRN_AssetTypeFlag,
                                                  INVTGRN_WithTaxFlg = a.INVTGRN_WithTaxFlg,
                                              }).Distinct().OrderBy(m => m.INVTGRN_Id).ToArray();

                data.edit_GRN_Master_Details = (from a in _INVContext.INV_M_GRNDMO
                                                from b in _INVContext.INV_M_GRN_StoreDMO
                                                from c in _INVContext.INV_Master_StoreDMO
                                                from d in _INVContext.INV_Master_SupplierDMO
                                                where a.INVMGRN_Id == b.INVMGRN_Id && b.INVMST_Id == c.INVMST_Id && a.INVMS_Id == d.INVMS_Id && d.MI_Id == a.MI_Id && a.INVMGRN_Id == data.INVMGRN_Id
                                                select new INV_T_GRNDTO
                                                {
                                                    INVMGRN_Id = a.INVMGRN_Id,
                                                    INVMGRN_PurchaseDate = a.INVMGRN_PurchaseDate,
                                                    INVMGRN_InvoiceNo = a.INVMGRN_InvoiceNo,
                                                    INVMS_Id = a.INVMS_Id,
                                                    INVMS_SupplierName = d.INVMS_SupplierName,
                                                    INVMST_Id = b.INVMST_Id,
                                                    INVMS_StoreName = c.INVMS_StoreName,
                                                    INVMGRN_TotalAmount = a.INVMGRN_TotalAmount,
                                                    INVMGRN_TotDiscount = a.INVMGRN_TotDiscount,
                                                    INVMGRN_TowardsProjects = a.INVMGRN_TowardsProjects,
                                                    INVMGRN_POReference = a.INVMGRN_POReference,
                                                    INVMGRN_EWayBillRef = a.INVMGRN_EWayBillRef,
                                                    INVTGRNT_TaxAmt = Convert.ToDecimal(a.INVMGRN_TotTaxAmt),
                                                    INVMGRN_PurchaseValue = Convert.ToDecimal(a.INVMGRN_PurchaseValue),
                                                    INVMGRN_Remarks = a.INVMGRN_Remarks
                                                }).ToArray();



                data.savedget_itemTax = (from a in _INVContext.INV_T_GRN_TaxDMO
                                         from b in _INVContext.INV_T_GRNDMO
                                         from d in _INVContext.INV_Master_ItemDMO
                                         from e in _INVContext.INV_Master_Item_TaxDMO
                                         from f in _INVContext.INV_Master_TaxDMO
                                         where a.INVTGRN_Id == b.INVTGRN_Id && b.INVMGRN_Id == data.INVMGRN_Id && a.INVTGRNT_ActiveFlg == true && a.INVMT_Id == e.INVMT_Id && a.INVMT_Id == f.INVMT_Id && d.INVMI_Id == e.INVMI_Id && d.INVMI_Id == b.INVMI_Id && f.MI_Id == d.MI_Id && f.MI_Id == data.MI_Id
                                         select new INV_T_GRNDTO
                                         {
                                             INVMI_Id = d.INVMI_Id,
                                             INVTGRN_Id = b.INVTGRN_Id,
                                             INVMI_ItemName = d.INVMI_ItemName,
                                             INVMT_Id = f.INVMT_Id,
                                             INVMT_TaxName = f.INVMT_TaxName,
                                             INVMT_TaxAliasName = f.INVMT_TaxAliasName,
                                             INVMIT_TaxValue = a.INVTGRNT_TaxPer,
                                             INVTGRN_TaxAmt = a.INVTGRNT_TaxAmt

                                         }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();







                //data.savedget_itemTax = (from a in _INVContext.INV_Master_ItemDMO
                //                    from b in _INVContext.INV_Master_Item_TaxDMO
                //                    from c in _INVContext.INV_Master_TaxDMO
                //                    from d in _INVContext.INV_T_GRN_TaxDMO
                //                         from e in _INVContext.INV_M_GRNDMO
                //                         from f in _INVContext.INV_T_GRNDMO
                //                    where (a.MI_Id==c.MI_Id && a.MI_Id==data.MI_Id && e.MI_Id==a.MI_Id && e.INVMGRN_Id==f.INVMGRN_Id && a.INVMI_Id ==b.INVMI_Id && a.INVMI_Id==f.INVMI_Id && b.INVMT_Id==d.INVMT_Id && c.INVMT_Id== b.INVMT_Id &&  d.INVTGRNT_Id == d.INVTGRNT_Id && d.INVTGRNT_ActiveFlg==true)
                //                    select new INV_T_GRNDTO
                //                    {
                //                        INVMI_Id = a.INVMI_Id,
                //                        INVTGRN_Id = d.INVTGRN_Id,
                //                        INVMI_ItemName = a.INVMI_ItemName,
                //                        INVMT_Id = b.INVMT_Id,
                //                        INVMT_TaxName = c.INVMT_TaxName,
                //                        INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                //                        INVMIT_TaxValue = b.INVMIT_TaxValue

                //                    }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();



                data.get_itemTax = (from a in _INVContext.INV_Master_ItemDMO
                                    from b in _INVContext.INV_Master_Item_TaxDMO
                                    from c in _INVContext.INV_Master_TaxDMO
                                    from e in _INVContext.INV_T_GRNDMO
                                    where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.MI_Id == c.MI_Id && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true && e.INVMI_Id == b.INVMI_Id && a.MI_Id == data.MI_Id && e.INVMGRN_Id == data.INVMGRN_Id)
                                    select new INV_T_GRNDTO
                                    {
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = a.INVMI_ItemName,
                                        INVMT_Id = b.INVMT_Id,
                                        INVMT_TaxName = c.INVMT_TaxName,
                                        INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                        INVMIT_TaxValue = b.INVMIT_TaxValue
                                    }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();

                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_ItemName).ToArray();
                data.get_Store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();
                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_Id).ToArray();
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_TAXEDIT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INVMGRN_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.INVMGRN_Id
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
                        data.get_itemTaxtemp = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                data.GrnFiles = _INVContext.INV_M_GRNFiles.Where(R => R.INVMGRN_Id == data.INVMGRN_Id).Distinct().ToArray();
                //libararyDetails 
                data.Group = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                     from b in _INVContext.INV_T_PurchaseOrderDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO
                                     where (a.INVMPO_Id == b.INVMPO_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == data.MI_Id && a.INVMPO_PONo == data.INVMGRN_POReference)
                                     select new INV_PurchaseOrderDTO
                                     {
                                         INVMPO_Id = a.INVMPO_Id,
                                         INVMPO_PONo = a.INVMPO_PONo,
                                         INVMS_Id = a.INVMS_Id,
                                         INVTPO_Id = b.INVTPO_Id,
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
                                         INVMPO_POTemplate = a.INVMPO_POTemplate,
                                         optionflag = c.INVMI_ItemType
                                     }).Distinct().OrderBy(i => i.INVTPO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("GRN Model:" + ex.Message);
            }
            return data;
        }
        public INV_T_GRNDTO savedetails(INV_T_GRNDTO data)
        {
           
            using (var dbCtxTxn = _INVContext.Database.BeginTransaction())
            {

                if (data.INVMGRN_Id > 0)
                {
                    var contactExistsP1 = _db.Database.ExecuteSqlCommand("INV_UpdateGrn @p0, @p1, @p2, @p3, @p4", data.MI_Id, data.INVMGRN_Id, data.INVMST_Id, data.INVMI_Id, data.INVTGRN_Qty);

                    var itmsdd = _INVContext.INV_T_GRNDMO.Where(e => e.INVMGRN_Id == data.INVMGRN_Id).ToList();

                    if (itmsdd.Count > 0)
                    {
                        foreach (var item in itmsdd)
                        {

                            var removetax = _INVContext.INV_T_GRN_TaxDMO.Where(r => r.INVTGRN_Id == item.INVTGRN_Id).ToList();
                            if (removetax.Count > 0)
                            {
                                foreach (var item2 in removetax)
                                {
                                    _INVContext.Remove(item2);
                                }
                            }

                        }

                        foreach (var item in itmsdd)
                        {

                            _INVContext.Remove(item);


                        }
                    }




                    var update = _INVContext.INV_M_GRNDMO.Single(e => e.INVMGRN_Id == data.INVMGRN_Id);
                    update.INVMS_Id = data.INVMS_Id;
                    update.INVMGRN_InvoiceNo = data.INVMGRN_InvoiceNo;
                    update.INVMGRN_PurchaseDate = data.INVMGRN_PurchaseDate;
                    update.INVMGRN_PurchaseValue = data.INVMGRN_PurchaseValue;
                    update.INVMGRN_TotDiscount = data.INVMGRN_TotDiscount;
                    update.INVMGRN_TotTaxAmt = data.INVMGRN_TotTaxAmt;
                    update.INVMGRN_TotalAmount = data.INVMGRN_TotalAmount;
                    update.INVMGRN_Remarks = data.INVMGRN_Remarks;
                    update.INVMGRN_POReference = data.INVMGRN_POReference;
                    update.INVMGRN_EWayBillRef = data.INVMGRN_EWayBillRef;
                    update.INVMGRN_TowardsProjects = data.INVMGRN_TowardsProjects;
                    update.UpdatedDate = DateTime.Now;
                    _INVContext.Update(update);





                    foreach (var i in data.GRNItem)
                    {
                        INV_T_GRNDMO grnitem = new INV_T_GRNDMO();

                        grnitem.INVMI_Id = i.INVMI_Id;
                        grnitem.INVMUOM_Id = i.INVMUOM_Id;
                        grnitem.INVTGRN_BatchNo = i.INVTGRN_BatchNo;
                        grnitem.INVTGRN_PurchaseRate = i.INVTGRN_PurchaseRate;
                        grnitem.INVTGRN_MRP = i.INVTGRN_MRP;
                        grnitem.INVTGRN_TaxAmt = i.INVTGRN_TaxAmt;
                        grnitem.INVTGRN_SalesPrice = i.INVTGRN_SalesPrice;
                        grnitem.INVTGRN_DiscountAmt = i.INVTGRN_DiscountAmt;
                        grnitem.INVTGRN_TaxAmt = i.INVTGRN_TaxAmt;
                        grnitem.INVTGRN_Amount = i.INVTGRN_Amount;
                        grnitem.INVTGRN_Qty = i.INVTGRN_Qty;
                        grnitem.INVTGRN_Naration = i.INVTGRN_Naration;
                        grnitem.INVTGRN_MfgDate = i.INVTGRN_MfgDate;
                        grnitem.INVTGRN_ExpDate = i.INVTGRN_ExpDate;
                        grnitem.INVTGRN_ActiveFlg = true;
                        grnitem.INVTGRN_WithTaxFlg = i.INVTGRN_WithTaxFlg;

                        grnitem.CreatedDate = DateTime.Now;
                        grnitem.UpdatedDate = DateTime.Now;
                        grnitem.INVTGRN_AssetTypeFlag = i.INVTGRN_AssetTypeFlag;
                        grnitem.INVMGRN_Id = data.INVMGRN_Id;

                        _INVContext.Add(grnitem);
                       

                        //===================================GRN Transcation Tax

                        if (i.GRNItemTax != null)
                        {
                            foreach (var t in i.GRNItemTax)
                            {
                                INV_T_GRN_TaxDMO grntax = new INV_T_GRN_TaxDMO();

                                grntax.INVMT_Id = t.INVMT_Id;
                                grntax.INVTGRNT_TaxPer = t.INVTGRNT_TaxPer;
                                grntax.INVTGRNT_TaxAmt = t.INVTGRNT_TaxAmt;
                                grntax.INVTGRNT_ActiveFlg = true;
                                grntax.CreatedDate = DateTime.Now;
                                grntax.UpdatedDate = DateTime.Now;

                                grntax.INVTGRN_Id = grnitem.INVTGRN_Id;

                                _INVContext.Add(grntax);

                            }
                        }
                        if (data.filelist != null && data.filelist.Length > 0)
                        {
                            var Removefiles = _INVContext.INV_M_GRNFiles.Where(R => R.INVMGRN_Id == data.INVMGRN_Id).ToList();
                            if(Removefiles.Count > 0)
                            {
                                foreach(var d  in Removefiles)
                                {
                                    _INVContext.Remove(d);
                                }
                            }
                            foreach (var d in data.filelist)
                            {
                                INV_M_GRNFiles obj = new INV_M_GRNFiles();
                                obj.INVMGRN_Id = data.INVMGRN_Id;
                                obj.INVMGRNF_FileName = d.VBABRSF_FileName;
                                obj.INVMGRNF_Filedesc = d.VBABRSF_Filedesc;
                                obj.INVMGRNF_FilePath = d.VBABRSF_FilePath;
                                obj.INVMGRNF_ActiveFlg = true;
                                obj.INVMGRNF_CreatedDate = DateTime.Now;
                                obj.INVMGRNF_UpdatedDate = DateTime.Now;
                                _INVContext.Add(obj);
                            }

                        }

                    }
                    var contactExists = _INVContext.SaveChanges();
                    dbCtxTxn.Commit();
                    if (contactExists > 0)
                    {
                        try
                        {
                            var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_UpdateGrn @p0, @p1, @p2, @p3, @p4", data.MI_Id, data.INVMGRN_Id, data.INVMST_Id, data.INVMI_Id, data.INVTGRN_Qty);
                            if (contactExistsP > 0)
                            {
                                data.returnduplicatestatus = "Updated";
                            }
                            else
                            {
                                data.returnduplicatestatus = "not Updated";
                            }
                        }
                        catch (Exception ex)
                        {
                            dbCtxTxn.Rollback();
                            data.message = "Error";
                            _logInv.LogInformation("GRN savedata :" + ex.Message);
                        }

                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                else
                {
                    try
                    {
                        if (data.optionflag == "GOODS")
                        {
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }
                        }
                        else if (data.optionflag == "SERVICE")
                        {
                            //if (data.serivicenumbconfigurationsettings.IMN_AutoManualFlag == "Auto")
                            //{
                            //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            //    data.serivicenumbconfigurationsettings.MI_Id = data.MI_Id;
                            //    data.serivicenumbconfigurationsettings.ASMAY_Id = data.ASMAY_Id;
                            //    data.trans_id = a.GenerateNumber(data.serivicenumbconfigurationsettings);
                            //}
                        }


                        var res = _INVContext.INV_M_GRNDMO.Where(t => t.INVMGRN_GRNNo == data.trans_id && t.INVMGRN_InvoiceNo == data.INVMGRN_InvoiceNo && t.INVMS_Id == data.INVMS_Id && t.MI_Id == data.MI_Id).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            INV_M_GRNDMO grn = new INV_M_GRNDMO();
                            grn.MI_Id = data.MI_Id;
                            grn.INVMS_Id = data.INVMS_Id;
                            grn.INVMGRN_GRNNo = data.trans_id;
                            grn.INVMGRN_InvoiceNo = data.INVMGRN_InvoiceNo;
                            grn.INVMGRN_PurchaseDate = data.INVMGRN_PurchaseDate;
                            grn.INVMGRN_PurchaseValue = data.INVMGRN_PurchaseValue;
                            grn.INVMGRN_TotDiscount = data.INVMGRN_TotDiscount;
                            grn.INVMGRN_TotTaxAmt = data.INVMGRN_TotTaxAmt;
                            grn.INVMGRN_TotalAmount = data.INVMGRN_TotalAmount;
                            grn.INVMGRN_Remarks = data.INVMGRN_Remarks;
                            grn.INVMGRN_TowardsProjects = data.INVMGRN_TowardsProjects;
                            grn.INVMGRN_EWayBillRef = data.INVMGRN_EWayBillRef;
                            grn.INVMGRN_POReference = data.INVMGRN_POReference;
                            grn.INVMGRN_ReturnFlg = data.INVMGRN_ReturnFlg;
                            grn.INVMGRN_PaidFlg = data.INVMGRN_PaidFlg;
                            grn.INVMGRN_CreditFlg = data.INVMGRN_CreditFlg;
                            grn.INVMGRN_ActiveFlg = true;
                            grn.CreatedDate = DateTime.Now;
                            grn.UpdatedDate = DateTime.Now;
                            _INVContext.Add(grn);

                            //===================================GRN Store
                            if (data.INVMST_Id > 0)
                            {
                                INV_M_GRN_StoreDMO grnstore = new INV_M_GRN_StoreDMO();
                                grnstore.INVMGRN_Id = grn.INVMGRN_Id;
                                grnstore.INVMST_Id = data.INVMST_Id;
                                grnstore.CreatedDate = DateTime.Now;
                                grnstore.UpdatedDate = DateTime.Now;
                                _INVContext.Add(grnstore);

                            }

                            //===================================GRN Transcation Data


                            foreach (var i in data.GRNItem)
                            {
                                INV_T_GRNDMO grnitem = new INV_T_GRNDMO();

                                grnitem.INVMI_Id = i.INVMI_Id;
                                grnitem.INVMUOM_Id = i.INVMUOM_Id;
                                grnitem.INVTGRN_BatchNo = i.INVTGRN_BatchNo;
                                grnitem.INVTGRN_PurchaseRate = i.INVTGRN_PurchaseRate;
                                grnitem.INVTGRN_MRP = i.INVTGRN_MRP;
                                grnitem.INVTGRN_TaxAmt = i.INVTGRN_TaxAmt;
                                grnitem.INVTGRN_SalesPrice = i.INVTGRN_SalesPrice;
                                grnitem.INVTGRN_DiscountAmt = i.INVTGRN_DiscountAmt;
                                grnitem.INVTGRN_TaxAmt = i.INVTGRN_TaxAmt;
                                grnitem.INVTGRN_Amount = i.INVTGRN_Amount;
                                grnitem.INVTGRN_Qty = i.INVTGRN_Qty;
                                grnitem.INVTGRN_Naration = i.INVTGRN_Naration;
                                grnitem.INVTGRN_MfgDate = i.INVTGRN_MfgDate;
                                grnitem.INVTGRN_ExpDate = i.INVTGRN_ExpDate;
                                grnitem.INVTGRN_ActiveFlg = true;
                                grnitem.CreatedDate = DateTime.Now;
                                grnitem.UpdatedDate = DateTime.Now;
                                grnitem.INVTGRN_AssetTypeFlag = i.INVTGRN_AssetTypeFlag;
                                grnitem.INVMGRN_Id = grn.INVMGRN_Id;
                                grnitem.INVTGRN_WithTaxFlg = i.INVTGRN_WithTaxFlg;
                                _INVContext.Add(grnitem);
                              
                              

                                //===================================GRN Transcation Tax

                                if (i.GRNItemTax != null)
                                {
                                    foreach (var t in i.GRNItemTax)
                                    {
                                        INV_T_GRN_TaxDMO grntax = new INV_T_GRN_TaxDMO();

                                        grntax.INVMT_Id = t.INVMT_Id;
                                        grntax.INVTGRNT_TaxPer = t.INVTGRNT_TaxPer;
                                        grntax.INVTGRNT_TaxAmt = t.INVTGRNT_TaxAmt;
                                        grntax.INVTGRNT_ActiveFlg = true;
                                        grntax.CreatedDate = DateTime.Now;
                                        grntax.UpdatedDate = DateTime.Now;

                                        grntax.INVTGRN_Id = grnitem.INVTGRN_Id;

                                        _INVContext.Add(grntax);

                                    }
                                }


                            }
                            if (data.filelist != null && data.filelist.Length > 0)
                            {
                                foreach (var d in data.filelist)
                                {
                                    INV_M_GRNFiles obj = new INV_M_GRNFiles();
                                    obj.INVMGRN_Id = grn.INVMGRN_Id;
                                    obj.INVMGRNF_FileName = d.VBABRSF_FileName;
                                    obj.INVMGRNF_Filedesc = d.VBABRSF_Filedesc;
                                    obj.INVMGRNF_FilePath = d.VBABRSF_FilePath;
                                    obj.INVMGRNF_ActiveFlg = true;
                                    obj.INVMGRNF_CreatedDate = DateTime.Now;
                                    obj.INVMGRNF_UpdatedDate = DateTime.Now;
                                    _INVContext.Add(obj);
                                }

                            }
                            var contactExists = _INVContext.SaveChanges();
                            dbCtxTxn.Commit();
                            if (contactExists > 0)
                            {
                                try
                                {
                                    var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_InsertGrn @p0, @p1", grn.INVMGRN_Id, data.MI_Id);
                                    if (contactExistsP > 0)
                                    {
                                        data.returnduplicatestatus = "Updated";
                                    }
                                    else
                                    {
                                        data.returnduplicatestatus = "not Updated";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    dbCtxTxn.Rollback();
                                    data.message = "Error";
                                    _logInv.LogInformation("GRN savedata :" + ex.Message);
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
                        dbCtxTxn.Rollback();
                        data.message = "Error";
                        _logInv.LogInformation("GRN savedata :" + ex.Message);
                    }
                }


            }
            return data;
        }

        public INV_T_GRNDTO get_GRNitemDetails(INV_T_GRNDTO data)
        {
            try
            {
                if(data.MI_IdNew > 0)
                {
                    data.MI_Id = data.MI_IdNew;
                }
                data.get_GRNItemDetails = (from a in _INVContext.INV_T_GRNDMO
                                           from b in _INVContext.INV_M_GRNDMO
                                           from c in _INVContext.INV_Master_ItemDMO
                                           from d in _INVContext.INV_Master_UOMDMO
                                           from e in _INVContext.INV_Master_SupplierDMO
                                           where (a.INVMGRN_Id == b.INVMGRN_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && b.MI_Id == c.MI_Id && e.INVMS_Id == b.INVMS_Id && b.MI_Id == data.MI_Id && a.INVMGRN_Id == data.INVMGRN_Id && a.INVTGRN_ActiveFlg == true)
                                           select new INV_T_GRNDTO
                                           {
                                               INVTGRN_Id = a.INVTGRN_Id,
                                               INVMGRN_Id = a.INVMGRN_Id,
                                               INVMGRN_GRNNo = b.INVMGRN_GRNNo,
                                               INVMI_Id = a.INVMI_Id,
                                               INVMUOM_Id = a.INVMUOM_Id,
                                               INVMI_ItemName = c.INVMI_ItemName,
                                               INVMUOM_UOMName = d.INVMUOM_UOMName,
                                               INVTGRN_BatchNo = a.INVTGRN_BatchNo,
                                               INVTGRN_PurchaseRate = a.INVTGRN_PurchaseRate,
                                               INVTGRN_MRP = a.INVTGRN_MRP,
                                               INVTGRN_SalesPrice = a.INVTGRN_SalesPrice,
                                               INVTGRN_DiscountAmt = a.INVTGRN_DiscountAmt,
                                               INVTGRN_TaxAmt = a.INVTGRN_TaxAmt,
                                               INVTGRN_Amount = a.INVTGRN_Amount,
                                               INVTGRN_Qty = a.INVTGRN_Qty,
                                               INVTGRN_Naration = a.INVTGRN_Naration,
                                               INVTGRN_MfgDate = a.INVTGRN_MfgDate,
                                               INVTGRN_ExpDate = a.INVTGRN_ExpDate,
                                               INVTGRN_ActiveFlg = a.INVTGRN_ActiveFlg,
                                               INVMS_SupplierName = e.INVMS_SupplierName

                                           }).Distinct().OrderBy(m => m.INVTGRN_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("GRN Model:" + ex.Message);
            }
            return data;
        }
        public INV_T_GRNDTO get_itemtax(INV_T_GRNDTO data)
        {
            try
            {
                data.get_GRNItemTax = (from a in _INVContext.INV_T_GRN_TaxDMO
                                       from b in _INVContext.INV_T_GRNDMO
                                       from c in _INVContext.INV_Master_TaxDMO
                                       where (a.INVTGRN_Id == b.INVTGRN_Id && a.INVMT_Id == c.INVMT_Id && c.MI_Id == data.MI_Id && a.INVTGRN_Id == data.INVTGRN_Id)
                                       select new INV_T_GRNDTO
                                       {
                                           INVTGRNT_Id = a.INVTGRNT_Id,
                                           INVTGRN_Id = a.INVTGRN_Id,
                                           INVMT_Id = a.INVMT_Id,
                                           INVMT_TaxName = c.INVMT_TaxName,
                                           INVTGRNT_TaxPer = a.INVTGRNT_TaxPer,
                                           INVTGRNT_TaxAmt = a.INVTGRNT_TaxAmt,
                                           INVTGRNT_ActiveFlg = a.INVTGRNT_ActiveFlg

                                       }).Distinct().OrderBy(m => m.INVTGRNT_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("GRN Model:" + ex.Message);
            }
            return data;
        }
        public INV_T_GRNDTO deactive(INV_T_GRNDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_GRNDMO.Single(t => t.INVMGRN_Id == data.INVMGRN_Id);

                if (result.INVMGRN_ActiveFlg == true)
                {
                    result.INVMGRN_ActiveFlg = false;
                }
                else if (result.INVMGRN_ActiveFlg == false)
                {
                    result.INVMGRN_ActiveFlg = true;
                }
                var resultt = _INVContext.INV_T_GRNDMO.Where(t => t.INVMGRN_Id == data.INVMGRN_Id).ToList();
                foreach (var r in resultt)
                {
                    if (r.INVTGRN_ActiveFlg == true)
                    {
                        r.INVTGRN_ActiveFlg = false;
                    }
                    else if (r.INVTGRN_ActiveFlg == false)
                    {
                        r.INVTGRN_ActiveFlg = true;
                    }
                    r.UpdatedDate = DateTime.Now;
                    _INVContext.Update(r);
                }
                var grntrans = (from a in _INVContext.INV_M_GRNDMO
                                from b in _INVContext.INV_T_GRNDMO
                                where (a.INVMGRN_Id == b.INVMGRN_Id && a.INVMGRN_Id == data.INVMGRN_Id)
                                select new INV_T_GRNDTO
                                {
                                    INVMGRN_Id = a.INVMGRN_Id,
                                    INVTGRN_Id = b.INVTGRN_Id,

                                }).Distinct().OrderBy(m => m.INVMGRN_Id).ToList();
                foreach (var tid in grntrans)
                {
                    var resulttax = _INVContext.INV_T_GRN_TaxDMO.Where(t => t.INVTGRN_Id == tid.INVTGRN_Id).ToList();
                    foreach (var rtax in resulttax)
                    {
                        if (rtax.INVTGRNT_ActiveFlg == true)
                        {
                            rtax.INVTGRNT_ActiveFlg = false;
                        }
                        else if (rtax.INVTGRNT_ActiveFlg == false)
                        {
                            rtax.INVTGRNT_ActiveFlg = true;
                        }

                        rtax.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rtax);
                    }
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
        public INV_T_GRNDTO deactiveg(INV_T_GRNDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_GRNDMO.Single(t => t.INVTGRN_Id == data.INVTGRN_Id);

                if (result.INVTGRN_ActiveFlg == true)
                {
                    result.INVTGRN_ActiveFlg = false;
                }
                else if (result.INVTGRN_ActiveFlg == false)
                {
                    result.INVTGRN_ActiveFlg = true;
                }
                var resulttax = _INVContext.INV_T_GRN_TaxDMO.Where(t => t.INVTGRN_Id == data.INVTGRN_Id).ToList();
                foreach (var rtax in resulttax)
                {
                    if (rtax.INVTGRNT_ActiveFlg == true)
                    {
                        rtax.INVTGRNT_ActiveFlg = false;
                    }
                    else if (rtax.INVTGRNT_ActiveFlg == false)
                    {
                        rtax.INVTGRNT_ActiveFlg = true;
                    }
                    rtax.UpdatedDate = DateTime.Now;
                    _INVContext.Update(rtax);
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
        public INV_T_GRNDTO deactivet(INV_T_GRNDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_GRN_TaxDMO.Single(t => t.INVTGRNT_Id == data.INVTGRNT_Id);

                if (result.INVTGRNT_ActiveFlg == true)
                {
                    result.INVTGRNT_ActiveFlg = false;
                }
                else if (result.INVTGRNT_ActiveFlg == false)
                {
                    result.INVTGRNT_ActiveFlg = true;
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

        //savedetailstwo
        public INV_T_GRNDTO savedetailstwo(INV_T_GRNDTO data)
        {
            using (var dbCtxTxn = _INVContext.Database.BeginTransaction())
            {

                if (data.INVMGRN_Id > 0)
                {
                    var contactExistsP1 = _db.Database.ExecuteSqlCommand("INV_UpdateGrn @p0, @p1, @p2, @p3, @p4", data.MI_Id, data.INVMGRN_Id, data.INVMST_Id, data.INVMI_Id, data.INVTGRN_Qty);

                    var itmsdd = _INVContext.INV_T_GRNDMO.Where(e => e.INVMGRN_Id == data.INVMGRN_Id).ToList();

                    if (itmsdd.Count > 0)
                    {
                        foreach (var item in itmsdd)
                        {

                            var removetax = _INVContext.INV_T_GRN_TaxDMO.Where(r => r.INVTGRN_Id == item.INVTGRN_Id).ToList();
                            if (removetax.Count > 0)
                            {
                                foreach (var item2 in removetax)
                                {
                                    _INVContext.Remove(item2);
                                }
                            }

                        }

                        foreach (var item in itmsdd)
                        {

                            _INVContext.Remove(item);


                        }
                    }




                    var update = _INVContext.INV_M_GRNDMO.Single(e => e.INVMGRN_Id == data.INVMGRN_Id);
                    update.INVMS_Id = data.INVMS_Id;
                    update.INVMGRN_InvoiceNo = data.INVMGRN_InvoiceNo;
                    update.INVMGRN_PurchaseDate = data.INVMGRN_PurchaseDate;
                    update.INVMGRN_PurchaseValue = data.INVMGRN_PurchaseValue;
                    update.INVMGRN_TotDiscount = data.INVMGRN_TotDiscount;
                    update.INVMGRN_TotTaxAmt = data.INVMGRN_TotTaxAmt;
                    update.INVMGRN_TotalAmount = data.INVMGRN_TotalAmount;
                    update.INVMGRN_Remarks = data.INVMGRN_Remarks;
                    update.INVMGRN_POReference = data.INVMGRN_POReference;
                    update.INVMGRN_EWayBillRef = data.INVMGRN_EWayBillRef;
                    update.INVMGRN_TowardsProjects = data.INVMGRN_TowardsProjects;
                    update.UpdatedDate = DateTime.Now;
                    _INVContext.Update(update);





                    foreach (var i in data.GRNItem)
                    {
                        INV_T_GRNDMO grnitem = new INV_T_GRNDMO();

                        grnitem.INVMI_Id = i.INVMI_Id;
                        grnitem.INVMUOM_Id = i.INVMUOM_Id;
                        grnitem.INVTGRN_BatchNo = i.INVTGRN_BatchNo;
                        grnitem.INVTGRN_PurchaseRate = i.INVTGRN_PurchaseRate;
                        grnitem.INVTGRN_MRP = i.INVTGRN_MRP;
                        grnitem.INVTGRN_TaxAmt = i.INVTGRN_TaxAmt;
                        grnitem.INVTGRN_SalesPrice = i.INVTGRN_SalesPrice;
                        grnitem.INVTGRN_DiscountAmt = i.INVTGRN_DiscountAmt;
                        grnitem.INVTGRN_TaxAmt = i.INVTGRN_TaxAmt;
                        grnitem.INVTGRN_Amount = i.INVTGRN_Amount;
                        grnitem.INVTGRN_Qty = i.INVTGRN_Qty;
                        grnitem.INVTGRN_Naration = i.INVTGRN_Naration;
                        grnitem.INVTGRN_MfgDate = i.INVTGRN_MfgDate;
                        grnitem.INVTGRN_ExpDate = i.INVTGRN_ExpDate;
                        grnitem.INVTGRN_ActiveFlg = true;

                        grnitem.CreatedDate = DateTime.Now;
                        grnitem.UpdatedDate = DateTime.Now;
                        grnitem.INVTGRN_AssetTypeFlag = i.INVTGRN_AssetTypeFlag;
                        grnitem.INVMGRN_Id = data.INVMGRN_Id;
                        grnitem.INVTGRN_WithTaxFlg = i.INVTGRN_WithTaxFlg;
                        _INVContext.Add(grnitem);
                        var group = _INVContext.INV_Master_ItemDMO.Where(R => R.INVMI_Id == i.INVMI_Id).ToList();
                        if (group.Count > 0 && i.INVMG_Id > 0)
                        {
                            var updates = _INVContext.INV_Master_ItemDMO.Where(R => R.INVMI_Id == i.INVMI_Id).FirstOrDefault();

                            updates.INVMG_Id = i.INVMG_Id;
                            _INVContext.Update(updates);
                        }
                        //===================================GRN Transcation Tax

                        if (i.GRNItemTax != null)
                        {
                            foreach (var t in i.GRNItemTax)
                            {
                                INV_T_GRN_TaxDMO grntax = new INV_T_GRN_TaxDMO();
                                //
                                grntax.INVMT_Id = t.INVMT_Id;
                                grntax.INVTGRNT_TaxPer = t.INVTGRNT_TaxPer;
                                //  grntax.INVTGRNT_TaxAmt = t.INVTGRNT_TaxAmt;
                                //grntax.INVTGRNT_TaxAmt = t.INVTGRNT_TaxPer * t.INVTGRNT_TaxAmt / 100;
                                grntax.INVTGRNT_TaxAmt = t.INVTGRNT_TaxPer * i.INVTGRN_Amount / 100;
                                grntax.INVTGRNT_ActiveFlg = true;
                                grntax.CreatedDate = DateTime.Now;
                                grntax.UpdatedDate = DateTime.Now;

                                grntax.INVTGRN_Id = grnitem.INVTGRN_Id;

                                _INVContext.Add(grntax);

                            }
                        }


                    }
                    var contactExists = _INVContext.SaveChanges();
                    dbCtxTxn.Commit();
                    if (contactExists > 0)
                    {
                        try
                        {
                            var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_UpdateGrn @p0, @p1, @p2, @p3, @p4", data.MI_Id, data.INVMGRN_Id, data.INVMST_Id, data.INVMI_Id, data.INVTGRN_Qty);
                            if (contactExistsP > 0)
                            {
                                data.returnduplicatestatus = "Updated";
                            }
                            else
                            {
                                data.returnduplicatestatus = "not Updated";
                            }
                        }
                        catch (Exception ex)
                        {
                            dbCtxTxn.Rollback();
                            data.message = "Error";
                            _logInv.LogInformation("GRN savedata :" + ex.Message);
                        }

                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                else
                {
                    try
                    {
                        if (data.optionflag == "GOODS")
                        {
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }
                        }
                        else if (data.optionflag == "SERVICE")
                        {
                            //if (data.serivicenumbconfigurationsettings.IMN_AutoManualFlag == "Auto")
                            //{
                            //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            //    data.serivicenumbconfigurationsettings.MI_Id = data.MI_Id;
                            //    data.serivicenumbconfigurationsettings.ASMAY_Id = data.ASMAY_Id;
                            //    data.trans_id = a.GenerateNumber(data.serivicenumbconfigurationsettings);
                            //}
                        }


                        var res = _INVContext.INV_M_GRNDMO.Where(t => t.INVMGRN_GRNNo == data.trans_id && t.INVMGRN_InvoiceNo == data.INVMGRN_InvoiceNo && t.INVMS_Id == data.INVMS_Id && t.MI_Id == data.MI_Id).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            INV_M_GRNDMO grn = new INV_M_GRNDMO();
                            grn.MI_Id = data.MI_Id;
                            grn.INVMS_Id = data.INVMS_Id;
                            grn.INVMGRN_GRNNo = data.trans_id;
                            grn.INVMGRN_InvoiceNo = data.INVMGRN_InvoiceNo;
                            grn.INVMGRN_PurchaseDate = data.INVMGRN_PurchaseDate;
                            grn.INVMGRN_PurchaseValue = data.INVMGRN_PurchaseValue;
                            grn.INVMGRN_TotDiscount = data.INVMGRN_TotDiscount;
                            grn.INVMGRN_TotTaxAmt = data.INVMGRN_TotTaxAmt;
                            grn.INVMGRN_TotalAmount = data.INVMGRN_TotalAmount;
                            grn.INVMGRN_Remarks = data.INVMGRN_Remarks;
                            grn.INVMGRN_TowardsProjects = data.INVMGRN_TowardsProjects;
                            grn.INVMGRN_EWayBillRef = data.INVMGRN_EWayBillRef;
                            grn.INVMGRN_POReference = data.INVMGRN_POReference;
                            grn.INVMGRN_ReturnFlg = data.INVMGRN_ReturnFlg;
                            grn.INVMGRN_PaidFlg = data.INVMGRN_PaidFlg;
                            grn.INVMGRN_CreditFlg = data.INVMGRN_CreditFlg;
                            grn.INVMGRN_ActiveFlg = true;
                            grn.CreatedDate = DateTime.Now;
                            grn.UpdatedDate = DateTime.Now;
                            _INVContext.Add(grn);
                            //===================================GRN Store
                            if (data.INVMST_Id > 0)
                            {
                                INV_M_GRN_StoreDMO grnstore = new INV_M_GRN_StoreDMO();
                                grnstore.INVMGRN_Id = grn.INVMGRN_Id;
                                grnstore.INVMST_Id = data.INVMST_Id;
                                grnstore.CreatedDate = DateTime.Now;
                                grnstore.UpdatedDate = DateTime.Now;
                                _INVContext.Add(grnstore);
                            }
                            //===================================GRN Transcation Data
                            foreach (var i in data.GRNItem)
                            {
                                INV_T_GRNDMO grnitem = new INV_T_GRNDMO();
                                grnitem.INVMI_Id = i.INVMI_Id;
                                grnitem.INVMUOM_Id = i.INVMUOM_Id;
                                grnitem.INVTGRN_BatchNo = i.INVTGRN_BatchNo;
                                grnitem.INVTGRN_PurchaseRate = i.INVTGRN_PurchaseRate;
                                grnitem.INVTGRN_MRP = i.INVTGRN_MRP;
                                grnitem.INVTGRN_TaxAmt = i.INVTGRN_TaxAmt;
                                grnitem.INVTGRN_SalesPrice = i.INVTGRN_SalesPrice;
                                grnitem.INVTGRN_DiscountAmt = i.INVTGRN_DiscountAmt;
                                grnitem.INVTGRN_TaxAmt = i.INVTGRN_TaxAmt;
                                grnitem.INVTGRN_Amount = i.INVTGRN_Amount;
                                grnitem.INVTGRN_Qty = i.INVTGRN_Qty;
                                grnitem.INVTGRN_Naration = i.INVTGRN_Naration;
                                grnitem.INVTGRN_MfgDate = i.INVTGRN_MfgDate;
                                grnitem.INVTGRN_ExpDate = i.INVTGRN_ExpDate;
                                grnitem.INVTGRN_ActiveFlg = true;
                                grnitem.CreatedDate = DateTime.Now;
                                grnitem.UpdatedDate = DateTime.Now;
                                grnitem.INVTGRN_AssetTypeFlag = i.INVTGRN_AssetTypeFlag;
                                grnitem.INVMGRN_Id = grn.INVMGRN_Id;
                                grnitem.INVTGRN_WithTaxFlg = i.INVTGRN_WithTaxFlg;
                                _INVContext.Add(grnitem);
                                var group = _INVContext.INV_Master_ItemDMO.Where(R => R.INVMI_Id == i.INVMI_Id).ToList();
                                if (group.Count > 0 && i.INVMG_Id > 0)
                                {
                                    var updates = _INVContext.INV_Master_ItemDMO.Where(R => R.INVMI_Id == i.INVMI_Id).FirstOrDefault();
                                    updates.INVMG_Id = i.INVMG_Id;
                                    _INVContext.Update(updates);
                                }
                                //===================================GRN Transcation Tax
                                if (i.GRNItemTax != null)
                                {
                                    foreach (var t in i.GRNItemTax)
                                    {
                                        INV_T_GRN_TaxDMO grntax = new INV_T_GRN_TaxDMO();
                                        grntax.INVMT_Id = t.INVMT_Id;
                                        grntax.INVTGRNT_TaxPer = t.INVTGRNT_TaxPer;                                       
                                        grntax.INVTGRNT_TaxAmt = t.INVTGRNT_TaxPer * i.INVTGRN_Amount / 100;
                                        grntax.INVTGRNT_ActiveFlg = true;
                                        grntax.CreatedDate = DateTime.Now;
                                        grntax.UpdatedDate = DateTime.Now;
                                        grntax.INVTGRN_Id = grnitem.INVTGRN_Id;
                                        _INVContext.Add(grntax);
                                    }
                                }
                            }
                            var contactExists = _INVContext.SaveChanges();
                            dbCtxTxn.Commit();
                            if (contactExists > 0)
                            {
                                try
                                {
                                    var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_InsertGrn @p0, @p1", grn.INVMGRN_Id, data.MI_Id);
                                    if (contactExistsP > 0)
                                    {
                                        data.returnduplicatestatus = "Updated";
                                    }
                                    else
                                    {
                                        data.returnduplicatestatus = "not Updated";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    dbCtxTxn.Rollback();
                                    data.message = "Error";
                                    _logInv.LogInformation("GRN savedata :" + ex.Message);
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
                        dbCtxTxn.Rollback();
                        data.message = "Error";
                        _logInv.LogInformation("GRN savedata :" + ex.Message);
                    }
                }


            }
            return data;
        }
    }
}
