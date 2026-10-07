using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_T_GRN_Return_Impl : Interface.INV_T_GRN_ReturnInterface
    {
        public InventoryContext _INVContext;

        public DomainModelMsSqlServerContext _db;
        public INV_T_GRN_Return_Impl(InventoryContext InvContext, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _db = db;
        }


        public GRN_ReturnDTO getloaddata(GRN_ReturnDTO data)
        {
            try
            {
                data.get_GRN = _INVContext.INV_M_GRNDMO.Where(m => m.MI_Id == data.MI_Id).ToArray();
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GRNReturnList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMGRNRET_Id",
            SqlDbType.BigInt)
                    {
                        Value = 0
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "GRNList"
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
                        data.get_GRNReturnList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //data.get_GRN = (from a in _INVContext.INV_M_GRNDMO
                //                from b in _INVContext.INV_Master_SupplierDMO
                //                where a.MI_Id == b.MI_Id && a.INVMS_Id == b.INVMS_Id && a.MI_Id == data.MI_Id
                //                select new GRN_ReturnDTO
                //                {
                //                    INVMGRN_Id = a.INVMGRN_Id,
                //                    INVMGRN_GRNNo = a.INVMGRN_GRNNo,
                //                    INVMGRN_InvoiceNo = a.INVMGRN_InvoiceNo,
                //                    INVMGRN_PurchaseDate = a.INVMGRN_PurchaseDate,
                //                    INVMGRN_PurchaseValue = a.INVMGRN_PurchaseValue,
                //                    INVMGRN_TotDiscount = a.INVMGRN_TotDiscount,
                //                    INVMGRN_TotTaxAmt = a.INVMGRN_TotTaxAmt,
                //                    INVMGRN_TotalAmount = a.INVMGRN_TotalAmount,
                //                    INVMGRN_Remarks = a.INVMGRN_Remarks,
                //                    INVMGRN_ActiveFlg = a.INVMGRN_ActiveFlg,
                //                    INVMS_SupplierName = b.INVMS_SupplierName


                //                }).OrderBy(m => m.INVMGRN_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public GRN_ReturnDTO getitem(GRN_ReturnDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GetItem";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMGRN_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.INVMGRN_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "Item"
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
                        data.get_item = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.get_item = (from a in _INVContext.INV_Master_ItemDMO
                //                 from b in _INVContext.INV_T_GRNDMO
                //                 from c in _INVContext.INV_M_GRNDMO
                //                 from d in _INVContext.INV_Master_UOMDMO
                //                 where (a.INVMI_Id == b.INVMI_Id && b.INVMGRN_Id == c.INVMGRN_Id && a.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == data.MI_Id && b.INVMGRN_Id==data.INVMGRN_Id)
                //                 select new GRN_ReturnDTO
                //                 {
                //                     INVMI_Id = a.INVMI_Id,
                //                     INVMI_ItemName = a.INVMI_ItemName,
                //                     INVMUOM_Id = d.INVMUOM_Id,
                //                 }).Distinct().OrderBy(m => m.INVMI_ItemName).ToArray();

                data.getsupplier = (from a in _INVContext.INV_M_GRNDMO
                                    from b in _INVContext.INV_Master_SupplierDMO

                                    where (a.INVMS_Id == b.INVMS_Id && a.MI_Id == data.MI_Id && a.INVMGRN_Id == data.INVMGRN_Id)
                                    select new GRN_ReturnDTO
                                    {
                                        INVMS_Id = Convert.ToInt32(a.INVMS_Id),
                                        INVMS_SupplierName = b.INVMS_SupplierName,
                                        INVMGRN_PurchaseDate = a.INVMGRN_PurchaseDate,
                                    }).Distinct().OrderBy(m => m.INVMS_SupplierName).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public GRN_ReturnDTO getitemDetail(GRN_ReturnDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GetItemDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMGRN_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.INVMGRN_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.INVMI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "ItemDetail"
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
                        data.get_itemDetail = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //data.get_itemDetail = (from a in _INVContext.INV_Master_ItemDMO
                //                       from b in _INVContext.INV_Master_UOMDMO
                //                       from c in _INVContext.INV_T_GRNDMO
                //                       where (c.INVMI_Id == a.INVMI_Id && a.INVMUOM_Id == b.INVMUOM_Id && c.INVMGRN_Id == data.INVMGRN_Id && c.INVMI_Id == data.INVMI_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                //                       select new GRN_ReturnDTO
                //                       {
                //                           INVMI_Id = a.INVMI_Id,
                //                           INVMI_ItemName = a.INVMI_ItemName,
                //                           INVMUOM_Id = b.INVMUOM_Id,
                //                           INVMUOM_UOMName = b.INVMUOM_UOMName,
                //                           INVMUOM_UOMAliasName = b.INVMUOM_UOMAliasName,
                //                           INVTGRN_PurchaseRate = c.INVTGRN_PurchaseRate,

                //                       }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public GRN_ReturnDTO savedetails(GRN_ReturnDTO data)
        {
            using (var dbCtxTxn = _INVContext.Database.BeginTransaction())
            {
                try
                {
                    if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                        data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                        data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                    }

                    var res = _INVContext.INV_M_GRN_Return_DMO_con.Where(t => t.INVMGRNRET_GRNRETNo == data.trans_id && t.INVMS_Id == data.INVMS_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_M_GRN_Return_DMO grn = new INV_M_GRN_Return_DMO();
                        grn.INVMGRNRETAPP_Id = 0;
                        grn.MI_Id = data.MI_Id;
                        grn.INVMGRN_Id = data.INVMGRN_Id;
                        grn.INVMS_Id = data.INVMS_Id;
                        grn.INVMGRNRET_GRNRETNo = data.trans_id;
                        grn.INVMGRNRET_ReturnDate = data.INVMGRNRET_ReturnDate;
                        grn.INVMGRNRET_DCDate = data.INVMGRNRET_DCDate;
                        grn.INVMGRNRET_DCNo = data.INVMGRNRET_DCNo;
                        grn.INVMGRNRET_DebitNoteDate = data.INVMGRNRET_DebitNoteDate;
                        grn.INVMGRNRET_DebitNoteNo = data.INVMGRNRET_DebitNoteNo;
                        grn.INVMGRNRET_EWayBillNo = data.INVMGRNRET_EWayBillNo;
                        grn.INVMGRNRET_TotalAmount = data.INVMGRN_PurchaseValue;
                        grn.INVMGRNRET_Remarks = data.INVMGRNRET_Remarks;
                        grn.INVMGRNRET_ActiveFlg = true;
                        grn.INVMGRNRET_CreatedBy = data.UserId;
                        grn.INVMGRNRET_UpdatedBy = data.UserId;
                        grn.INVMGRNRET_CreatedDate = DateTime.Today;
                        grn.INVMGRNRET_UpdatedDate = DateTime.Today;
                        _INVContext.Add(grn);



                        //===================================GRN Transcation Data


                        foreach (var i in data.GRNItem)
                        {
                            INV_T_GRN_Return_DMO grnitem = new INV_T_GRN_Return_DMO();

                            grnitem.INVMGRNRET_Id = grn.INVMGRNRET_Id;
                            grnitem.INVMI_Id = i.INVMI_Id;
                            grnitem.INVMUOM_Id = i.INVMUOM_Id;
                            grnitem.INVTGRNRET_ReturnQty = i.INVTGRNRET_ReturnQty;
                            grnitem.INVTGRNRET_ReturnAmount = i.INVTGRNRET_ReturnAmount;
                            grnitem.INVTGRNRET_ReturnNaration = i.INVTGRN_Naration;
                            grnitem.INVTGRNRET_ActiveFlg = true;
                            grnitem.INVTGRNRET_CreatedDate = DateTime.Today;
                            grnitem.INVTGRNRET_UpdatedDate = DateTime.Today;
                            grnitem.INVTGRNRET_CreatedBy = data.UserId;
                            grnitem.INVTGRNRET_UpdatedBy = data.UserId;


                            _INVContext.Add(grnitem);

                            //===================================GRN Transcation Tax
                            foreach (var t in i.GRNItemTax)
                            {
                                if (i.INVMI_Id == t.INVMI_Id)
                                {
                                    INV_T_GRN_Tax_Return_DMO grntax = new INV_T_GRN_Tax_Return_DMO();

                                    grntax.INVTGRNRET_Id = grnitem.INVTGRNRET_Id;
                                    grntax.INVMT_Id = t.INVMT_Id;
                                    grntax.INVTGRNTRET_TaxPer = t.INVTGRNT_TaxPer;
                                    grntax.INVTGRNTRET_TaxAmt = t.INVTGRNT_TaxAmt;
                                    grntax.INVTGRNTRET_ActiveFlg = true;
                                    grntax.INVTGRNTRET_CreatedDate = DateTime.Today;
                                    grntax.INVTGRNTRET_UpdatedDate = DateTime.Today;
                                    grntax.INVTGRNTRET_CreatedBy = data.UserId;
                                    grntax.INVTGRNTRET_UpdatedBy = data.UserId;
                                    _INVContext.Add(grntax);
                                }

                            }

                        }
                        var contactExists = _INVContext.SaveChanges();
                        dbCtxTxn.Commit();
                        if (contactExists > 0)
                        {
                            //try
                            //{
                            //    var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_InsertGrn @p0, @p1", grn.INVMGRN_Id, data.MI_Id);
                            //    if (contactExistsP > 0)
                            //    {
                            //        data.returnduplicatestatus = "Updated";
                            //    }
                            //    else
                            //    {
                            //        data.returnduplicatestatus = "not Updated";
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    dbCtxTxn.Rollback();
                            //    data.message = "Error";
                            //    _logInv.LogInformation("GRN savedata :" + ex.Message);
                            //}

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
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
        public GRN_ReturnDTO get_GRNitemDetails(GRN_ReturnDTO data)
        {
            try
            {

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GRNReturnList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMGRNRET_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.INVMGRNRET_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "GRNItemDetails"
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
                        data.get_GRNitemdetails = retObject.ToArray();
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
        public GRN_ReturnDTO get_itemtax(GRN_ReturnDTO data)
        {
            try
            {
                //data.get_GRNItemTax = (from a in _INVContext.INV_T_GRN_TaxDMO
                //                       from b in _INVContext.INV_T_GRNDMO
                //                       from c in _INVContext.INV_Master_TaxDMO
                //                       where (a.INVTGRN_Id == b.INVTGRN_Id && a.INVMT_Id == c.INVMT_Id && c.MI_Id == data.MI_Id && a.INVTGRN_Id == data.INVTGRN_Id)
                //                       select new GRN_ReturnDTO
                //                       {
                //                           INVTGRNT_Id = a.INVTGRNT_Id,
                //                           INVTGRN_Id = a.INVTGRN_Id,
                //                           INVMT_Id = a.INVMT_Id,
                //                           INVMT_TaxName = c.INVMT_TaxName,
                //                           INVTGRNT_TaxPer = a.INVTGRNT_TaxPer,
                //                           INVTGRNT_TaxAmt = a.INVTGRNT_TaxAmt,
                //                           INVTGRNT_ActiveFlg = a.INVTGRNT_ActiveFlg

                //                       }).Distinct().OrderBy(m => m.INVTGRNT_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public GRN_ReturnDTO deactive(GRN_ReturnDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_GRN_Return_DMO_con.Single(t => t.INVMGRNRET_Id == data.INVMGRNRET_Id && t.MI_Id == data.MI_Id);

                if (result.INVMGRNRET_ActiveFlg == true)
                {
                    result.INVMGRNRET_ActiveFlg = false;
                }
                else if (result.INVMGRNRET_ActiveFlg == false)
                {
                    result.INVMGRNRET_ActiveFlg = true;
                }
                result.INVMGRNRET_UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_GRN_Return_DMO_con.Where(t => t.INVMGRNRET_Id == data.INVMGRNRET_Id).ToList();
                foreach (var r in resultt)
                {
                    var slt = _INVContext.INV_T_GRN_Return_DMO_con.Single(t => t.INVMGRNRET_Id == data.INVMGRNRET_Id && t.INVTGRNRET_Id == r.INVTGRNRET_Id);

                    if (slt.INVTGRNRET_ActiveFlg == true)
                    {
                        slt.INVTGRNRET_ActiveFlg = false;
                    }
                    else if (slt.INVTGRNRET_ActiveFlg == false)
                    {
                        slt.INVTGRNRET_ActiveFlg = true;
                    }
                    slt.INVTGRNRET_UpdatedDate = DateTime.Now;
                    _INVContext.Update(slt);

                    var tax = _INVContext.INV_T_GRN_Tax_Return_DMO_con.Where(t => t.INVTGRNRET_Id == slt.INVTGRNRET_Id).ToList();
                    foreach (var tt in tax)
                    {
                        var tax1 = _INVContext.INV_T_GRN_Tax_Return_DMO_con.Single(t => t.INVTGRNRET_Id == slt.INVTGRNRET_Id && t.INVTGRNTRET_Id == tt.INVTGRNTRET_Id);
                        if (tax1.INVTGRNTRET_ActiveFlg == true)
                        {
                            tax1.INVTGRNTRET_ActiveFlg = false;
                        }
                        else if (tax1.INVTGRNTRET_ActiveFlg == false)
                        {
                            tax1.INVTGRNTRET_ActiveFlg = true;
                        }
                        tax1.INVTGRNTRET_UpdatedDate = DateTime.Now;
                        _INVContext.Update(tax1);
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

    }
}
