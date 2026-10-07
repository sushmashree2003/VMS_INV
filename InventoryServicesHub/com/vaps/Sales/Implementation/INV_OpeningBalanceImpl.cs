using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class INV_OpeningBalanceImpl : Interface.INV_OpeningBalanceInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_OpeningBalanceImpl> _logInv;
        public INV_OpeningBalanceImpl(InventoryContext InvContext, ILogger<INV_OpeningBalanceImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_OpeningBalanceDTO getloaddata(INV_OpeningBalanceDTO data)
        {
            try
            {
                data.get_Store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();
                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_Id).ToArray();
                data.get_openingbalance = (from a in _INVContext.INV_OpeningBalanceDMO
                                           from b in _INVContext.INV_Master_StoreDMO
                                           from c in _INVContext.INV_Master_ItemDMO
                                           from d in _INVContext.INV_Master_UOMDMO
                                           from e in _INVContext.INV_Master_GroupDMO
                                           where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && c.INVMG_Id==e.INVMG_Id)
                                           select new INV_OpeningBalanceDTO
                                           {
                                               INVMS_StoreName = b.INVMS_StoreName,
                                               INVMI_ItemName = c.INVMI_ItemName,
                                               INVMUOM_UOMName = d.INVMUOM_UOMName,
                                               INVMUOM_UOMAliasName = d.INVMUOM_UOMAliasName,
                                               INVOB_Id = a.INVOB_Id,
                                               INVMST_Id = a.INVMST_Id,
                                               INVMI_Id = a.INVMI_Id,
                                               INVMUOM_Id = a.INVMUOM_Id,
                                               INVOB_BatchNo = a.INVOB_BatchNo,
                                               INVOB_PurchaseDate = a.INVOB_PurchaseDate,
                                               INVOB_PurchaseRate = a.INVOB_PurchaseRate,
                                               INVOB_SaleRate = a.INVOB_SaleRate,
                                               INVOB_Qty = a.INVOB_Qty,
                                               INVOB_Naration = a.INVOB_Naration,
                                               INVOB_MfgDate = a.INVOB_MfgDate,
                                               INVOB_ExpDate = a.INVOB_ExpDate,
                                               INVOB_ActiveFlg = a.INVOB_ActiveFlg,
                                               INVOB_DepreciatedValue = a.INVOB_DepreciatedValue,
                                               INVOB_DepreciationAsOnDate = a.INVOB_DepreciationAsOnDate,
                                               INVMG_GroupName = e.INVMG_GroupName
                                          

                                           }).Distinct().OrderByDescending(m => m.INVOB_Id).ToArray();
                data.get_obdetails = _INVContext.IVRM_Master_FinancialYearDMO.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("OB load Page:" + ex.Message);
            }
            return data;
        }
        //savedeparture
        public INV_OpeningBalanceDTO savedeparture(INV_OpeningBalanceDTO data)
        {
            try
            {
                if(data.INVOB_Id > 0)
                {
                    var OBC = _INVContext.INV_OpeningBalanceDMO.Single(b => b.MI_Id == data.MI_Id && b.INVOB_Id == data.INVOB_Id);
                    OBC.INVOB_DepreciatedValue = data.INVOB_DepreciatedValue;
                    OBC.INVOB_DepreciationAsOnDate = data.INVOB_DepreciationAsOnDate;
                    OBC.UpdatedDate = DateTime.Now;
                    _INVContext.Update(OBC);
                    var contactExists = _INVContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnduplicatestatus = "Updated";
                    }
                    else
                    {
                        data.returnduplicatestatus = "notUpdated";
                    }
                }
                else
                {
                    data.returnduplicatestatus = "adminstartor";
                }
                
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("OB savedata :" + ex.Message);
            }
            return data;
        }
        public INV_OpeningBalanceDTO savedetails(INV_OpeningBalanceDTO data)
        {
            try
            {
                if (data.INVOB_Id != 0)
                {
                    foreach (var o in data.OBItem)
                    {
                        var OBU = _INVContext.INV_OpeningBalanceDMO.Single(b => b.MI_Id == data.MI_Id && b.INVOB_Id == data.INVOB_Id);
                        OBU.MI_Id = data.MI_Id;
                        OBU.INVMST_Id = data.INVMST_Id;
                        OBU.INVMUOM_Id = o.INVMUOM_Id;
                        OBU.IMFY_Id = data.IMFY_Id;
                        OBU.INVOB_BatchNo = o.INVOB_BatchNo;
                        OBU.INVOB_PurchaseDate = o.INVOB_PurchaseDate;
                        OBU.INVOB_PurchaseRate = o.INVOB_PurchaseRate;
                        OBU.INVOB_SaleRate = o.INVOB_SaleRate;
                        OBU.INVOB_Qty = o.INVOB_Qty;
                        OBU.INVOB_Naration = o.INVOB_Naration;
                        OBU.INVOB_DepreciatedValue = o.INVOB_DepreciatedValue;
                        OBU.INVOB_DepreciationAsOnDate = o.INVOB_DepreciationAsOnDate;
                        OBU.INVOB_ActiveFlg = true;
                        OBU.UpdatedDate = DateTime.Now;
                        _INVContext.Update(OBU);

                        var contactExists = _INVContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            try
                            {
                                var contactExistsO = _INVContext.Database.ExecuteSqlCommand("INV_InsertOPB @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, OBU.INVOB_Id, data.INVMST_Id, OBU.INVMI_Id, OBU.INVOB_PurchaseRate, OBU.INVOB_SaleRate);
                                if (contactExistsO > 0)
                                {
                                    data.returnduplicatestatus = "Updated";
                                }
                                else
                                {
                                    data.returnduplicatestatus = "notUpdated";
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {

                    foreach (var o in data.OBItem)
                    {
                        INV_OpeningBalanceDMO OB = new INV_OpeningBalanceDMO();
                        OB.MI_Id = data.MI_Id;
                        OB.INVMST_Id = data.INVMST_Id;
                        OB.INVMI_Id = o.INVMI_Id;
                        OB.INVMUOM_Id = o.INVMUOM_Id;
                        OB.IMFY_Id = data.IMFY_Id;
                        OB.INVOB_BatchNo = o.INVOB_BatchNo;
                        OB.INVOB_PurchaseDate = o.INVOB_PurchaseDate;
                        OB.INVOB_PurchaseRate = o.INVOB_PurchaseRate;
                        OB.INVOB_SaleRate = o.INVOB_SaleRate;
                        OB.INVOB_Qty = o.INVOB_Qty;
                        OB.INVOB_Naration = o.INVOB_Naration;
                        OB.INVOB_MfgDate = o.INVOB_MfgDate;
                        OB.INVOB_ExpDate = o.INVOB_ExpDate;
                        OB.INVOB_DepreciatedValue = o.INVOB_DepreciatedValue;
                        OB.INVOB_DepreciationAsOnDate = o.INVOB_DepreciationAsOnDate;
                        OB.INVOB_ActiveFlg = true;
                        OB.UpdatedDate = DateTime.Now;
                        OB.CreatedDate = DateTime.Now;
                      //  OB.INVOB_OBDate= DateTime.Now;
                        _INVContext.Add(OB);
                        var contextOB = _INVContext.SaveChanges();
                        if (contextOB > 0)
                        {
                            try
                            {
                                var contactExistsO = _INVContext.Database.ExecuteSqlCommand("INV_InsertOPB @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, OB.INVOB_Id, data.INVMST_Id, OB.INVMI_Id, OB.INVOB_PurchaseRate, OB.INVOB_SaleRate);
                                if (contactExistsO > 0)
                                {
                                    data.returnduplicatestatus = "Updated";
                                }
                                else
                                {
                                    data.returnduplicatestatus = "notUpdated";
                                }
                                var asset = _INVContext.INV_Master_ItemDMO.Where(R => R.INVMI_Id == OB.INVMI_Id).FirstOrDefault();
                               
                                if (asset.INVMI_ItemAssetType== "fixedasset")
                                {
                                    var contacdep = _INVContext.Database.ExecuteSqlCommand("INV_DepricaitionOB_Insert @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.INVMST_Id, OB.INVMI_Id, OB.IMFY_Id, OB.INVOB_PurchaseRate, OB.INVOB_DepreciatedValue);
                                    if (contacdep > 0)
                                    {
                                        data.returnduplicatestatus = "Updated";
                                    }
                                    else
                                    {
                                        data.returnduplicatestatus = "notUpdated";
                                    }
                                }
                                
                                //INV_DepricaitionOB_Insert
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
                data.message = "Error";
                _logInv.LogInformation("OB savedata :" + ex.Message);
            }
            return data;
        }

        public INV_OpeningBalanceDTO deactive(INV_OpeningBalanceDTO data)
        {
            try
            {
                var result = _INVContext.INV_OpeningBalanceDMO.Single(t => t.INVOB_Id == data.INVOB_Id);
                if (result.INVOB_ActiveFlg == true)
                {
                    result.INVOB_ActiveFlg = false;
                }
                else if (result.INVOB_ActiveFlg == false)
                {
                    result.INVOB_ActiveFlg = true;
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

        public INV_OpeningBalanceDTO getobdetails(INV_OpeningBalanceDTO data)
        {
            try
            {

                data.get_obdetails = (from a in _INVContext.INV_OpeningBalanceDMO
                                      from b in _INVContext.INV_Master_StoreDMO
                                      from c in _INVContext.INV_Master_ItemDMO
                                      from d in _INVContext.INV_Master_UOMDMO
                                      where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.INVOB_Id == data.INVOB_Id)
                                      select new INV_OpeningBalanceDTO
                                      {
                                          INVMS_StoreName = b.INVMS_StoreName,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                                          INVMUOM_UOMAliasName = d.INVMUOM_UOMAliasName,
                                          INVOB_Id = a.INVOB_Id,
                                          INVMST_Id = a.INVMST_Id,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMUOM_Id = a.INVMUOM_Id,
                                          INVOB_BatchNo = a.INVOB_BatchNo,
                                          INVOB_PurchaseDate = a.INVOB_PurchaseDate,
                                          INVOB_PurchaseRate = a.INVOB_PurchaseRate,
                                          INVOB_SaleRate = a.INVOB_SaleRate,
                                          INVOB_Qty = a.INVOB_Qty,
                                          INVOB_Naration = a.INVOB_Naration,
                                          INVOB_MfgDate = a.INVOB_MfgDate,
                                          INVOB_ExpDate = a.INVOB_ExpDate,
                                          INVOB_ActiveFlg = a.INVOB_ActiveFlg,

                                      }).Distinct().OrderBy(m => m.INVOB_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("OB load Page:" + ex.Message);
            }
            return data;
        }


    }
}
