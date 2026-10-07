using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class CheckOutAssetsDCImpl : Interface.CheckOutAssetsDCInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<CheckOutAssetsDCImpl> _logAT;
        public DomainModelMsSqlServerContext _db;
        public CheckOutAssetsDCImpl(AssetTrackingContext ATContext, ILogger<CheckOutAssetsDCImpl> log, DomainModelMsSqlServerContext db)
        {
            _ATContext = ATContext;
            _logAT = log;
            _db = db;
        }

        public CheckOutAssetsDCDTO getloaddata(CheckOutAssetsDCDTO data)
        {
            try
            {
                data.get_store = _ATContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();
                data.instlist = _ATContext.Institution.Where(m => m.MI_Id == data.MI_Id && m.MI_ActiveFlag == 1).OrderBy(m => m.MI_Id).ToArray();
                data.get_locations = _ATContext.INV_Master_LocationDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMLO_ActiveFlg == true).OrderBy(m => m.INVMLO_Id).ToArray();



                data.get_checkout = (from a in _ATContext.INV_Asset_CheckOutDMO
                                     from b in _ATContext.INV_Master_StoreDMO
                                     from c in _ATContext.INV_Master_ItemDMO
                                     from d in _ATContext.INV_Master_LocationDMO
                                     from e in _ATContext.INV_StockDMO
                                     where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && (a.INVACO_No == null || a.INVACO_No == "") && a.INVACO_ActiveFlg == true)
                                     select new CheckOutAssetsDCDTO
                                     {
                                         INVACO_Id = a.INVACO_Id,
                                         INVMST_Id = a.INVMST_Id,
                                         INVMS_StoreName = b.INVMS_StoreName,
                                         INVMI_Id = a.INVMI_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVSTO_SalesRate = a.INVSTO_SalesRate,
                                         INVMLO_Id = a.INVMLO_Id,
                                         HRME_Id = a.HRME_Id,
                                         INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                         INVACO_ReceivedBy = a.INVACO_ReceivedBy,
                                         INVACO_CheckoutDate = a.INVACO_CheckoutDate,
                                         INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                         INVACO_CheckOutRemarks = a.INVACO_CheckOutRemarks,
                                         INVACO_ActiveFlg = a.INVACO_ActiveFlg,
                                         UpdatedDate = a.UpdatedDate

                                     }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();



                data.get_checkoutdclist = (from a in _ATContext.INV_Asset_CheckOutDMO
                                           from b in _ATContext.INV_Master_StoreDMO
                                           from c in _ATContext.INV_Master_ItemDMO
                                           from d in _ATContext.INV_Master_LocationDMO
                                           from e in _ATContext.INV_StockDMO
                                           where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVACO_No != null && a.INVACO_No != "" && a.INVACO_ActiveFlg == true)
                                           select new CheckOutAssetsDCDTO
                                           {
                                               INVACO_Id = a.INVACO_Id,
                                               INVMST_Id = a.INVMST_Id,
                                               INVMS_StoreName = b.INVMS_StoreName,
                                               INVMI_Id = a.INVMI_Id,
                                               INVMI_ItemName = c.INVMI_ItemName,
                                               INVSTO_SalesRate = a.INVSTO_SalesRate,
                                               INVMLO_Id = a.INVMLO_Id,
                                               HRME_Id = a.HRME_Id,
                                               INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                               INVACO_ReceivedBy = a.INVACO_ReceivedBy,
                                               INVACO_CheckoutDate = a.INVACO_CheckoutDate,
                                               INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                               INVACO_CheckOutRemarks = a.INVACO_CheckOutRemarks,
                                               INVACO_ActiveFlg = a.INVACO_ActiveFlg,
                                               INVACO_No = a.INVACO_No,
                                               UpdatedDate = a.UpdatedDate

                                           }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();


                data.get_employee = (from a in _ATContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new AT_MasterLocationDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Checkout load Page:" + ex.Message);
            }
            return data;
        }

        public CheckOutAssetsDCDTO getitems(CheckOutAssetsDCDTO data)
        {
            try
            {
                var config = _ATContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                data.INVC_LIFOFIFOFlg = config.FirstOrDefault().INVC_LIFOFIFOFlg;

                if (data.INVC_LIFOFIFOFlg == "LIFO")
                {
                    data.get_items = (from a in _ATContext.INV_Master_ItemDMO
                                      from b in _ATContext.INV_StockDMO
                                      from c in _ATContext.INV_Master_StoreDMO
                                      where (a.INVMI_Id == b.INVMI_Id && b.INVMST_Id == c.INVMST_Id && a.MI_Id == data.MI_Id && b.INVSTO_AvaiableStock != 0 && b.INVSTO_PurchaseDate != null && b.INVMST_Id == data.INVMST_Id && a.INVMI_ActiveFlg == true)
                                      select new CheckOutAssetsDCDTO
                                      {
                                          INVMI_Id = a.INVMI_Id,
                                          INVSTO_PurchaseDate = b.INVSTO_PurchaseDate,
                                          INVMI_ItemName = a.INVMI_ItemName,
                                          INVSTO_SalesRate = b.INVSTO_SalesRate,
                                          INVSTO_AvaiableStock = b.INVSTO_AvaiableStock,
                                      }).Distinct().OrderByDescending(m => m.INVSTO_PurchaseDate).ToArray();
                }
                else
                {
                    data.get_items = (from a in _ATContext.INV_Master_ItemDMO
                                      from b in _ATContext.INV_StockDMO
                                      from c in _ATContext.INV_Master_StoreDMO
                                      where (a.INVMI_Id == b.INVMI_Id && b.INVMST_Id == c.INVMST_Id && a.MI_Id == data.MI_Id && b.INVSTO_AvaiableStock != 0 && b.INVSTO_PurchaseDate != null && b.INVMST_Id == data.INVMST_Id && a.INVMI_ActiveFlg == true)
                                      select new CheckOutAssetsDCDTO
                                      {
                                          INVMI_Id = a.INVMI_Id,
                                          INVSTO_PurchaseDate = b.INVSTO_PurchaseDate,
                                          INVMI_ItemName = a.INVMI_ItemName,
                                          INVSTO_SalesRate = b.INVSTO_SalesRate,
                                          INVSTO_AvaiableStock = b.INVSTO_AvaiableStock,
                                      }).Distinct().OrderBy(m => m.INVSTO_PurchaseDate).ToArray();
                }

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Checkout get items:" + ex.Message);
            }
            return data;
        }

        public CheckOutAssetsDCDTO savedetails(CheckOutAssetsDCDTO data)
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
                data.UpdatedDate = DateTime.Now;

                if (data.selecteditems != null)
                {
                    foreach (var item in data.selecteditems)
                    {
                        var result = _ATContext.INV_Asset_CheckOutDMO.Single(t => t.INVACO_Id == item.INVACO_Id);
                        result.INVACO_No = data.trans_id;
                        result.UpdatedDate = DateTime.Now;
                        _ATContext.Update(result);

                    }
                }
                var contactExists = _ATContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }



                data.get_checkout = (from a in _ATContext.INV_Asset_CheckOutDMO
                                     from b in _ATContext.INV_Master_StoreDMO
                                     from c in _ATContext.INV_Master_ItemDMO
                                     from d in _ATContext.INV_Master_LocationDMO
                                     from e in _ATContext.INV_StockDMO
                                     where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && (a.INVACO_No == null || a.INVACO_No == "") && a.INVACO_ActiveFlg == true)
                                     select new CheckOutAssetsDCDTO
                                     {
                                         INVACO_Id = a.INVACO_Id,
                                         INVMST_Id = a.INVMST_Id,
                                         INVMS_StoreName = b.INVMS_StoreName,
                                         INVMI_Id = a.INVMI_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVSTO_SalesRate = a.INVSTO_SalesRate,
                                         INVMLO_Id = a.INVMLO_Id,
                                         HRME_Id = a.HRME_Id,
                                         INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                         INVACO_ReceivedBy = a.INVACO_ReceivedBy,
                                         INVACO_CheckoutDate = a.INVACO_CheckoutDate,
                                         INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                         INVACO_CheckOutRemarks = a.INVACO_CheckOutRemarks,
                                         INVACO_ActiveFlg = a.INVACO_ActiveFlg,
                                         UpdatedDate = a.UpdatedDate

                                     }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();



                data.get_checkoutdclist = (from a in _ATContext.INV_Asset_CheckOutDMO
                                           from b in _ATContext.INV_Master_StoreDMO
                                           from c in _ATContext.INV_Master_ItemDMO
                                           from d in _ATContext.INV_Master_LocationDMO
                                           from e in _ATContext.INV_StockDMO
                                           where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVACO_No != null && a.INVACO_No != "" && a.INVACO_ActiveFlg == true)
                                           select new CheckOutAssetsDCDTO
                                           {
                                               INVACO_Id = a.INVACO_Id,
                                               INVMST_Id = a.INVMST_Id,
                                               INVMS_StoreName = b.INVMS_StoreName,
                                               INVMI_Id = a.INVMI_Id,
                                               INVMI_ItemName = c.INVMI_ItemName,
                                               INVSTO_SalesRate = a.INVSTO_SalesRate,
                                               INVMLO_Id = a.INVMLO_Id,
                                               HRME_Id = a.HRME_Id,
                                               INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                               INVACO_ReceivedBy = a.INVACO_ReceivedBy,
                                               INVACO_CheckoutDate = a.INVACO_CheckoutDate,
                                               INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                               INVACO_CheckOutRemarks = a.INVACO_CheckOutRemarks,
                                               INVACO_ActiveFlg = a.INVACO_ActiveFlg,
                                               INVACO_No = a.INVACO_No,
                                               UpdatedDate = a.UpdatedDate

                                           }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Asset Check out savedata :" + ex.Message);
            }
            return data;
        }

        public CheckOutAssetsDCDTO deactive(CheckOutAssetsDCDTO data)
        {
            try
            {
                var result = _ATContext.INV_Asset_CheckOutDMO.Single(t => t.INVACO_Id == data.INVACO_Id);

                if (result.INVACO_ActiveFlg == true)
                {
                    result.INVACO_ActiveFlg = false;
                }
                else if (result.INVACO_ActiveFlg == false)
                {
                    result.INVACO_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
                if (returnval > 0)
                {
                    try
                    {
                        var contactExistsP = _ATContext.Database.ExecuteSqlCommand("INV_StockupdatesCheckOut @p0, @p1,@p2, @p3,@p4,@p5", data.MI_Id, data.INVMST_Id, data.INVMLO_Id, data.INVMI_Id, data.INVSTO_SalesRate, data.INVACO_Id);
                        if (contactExistsP > 0)
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
                        data.message = "Error";
                        _logAT.LogInformation("Asset Checkout Procedure :" + ex.Message);
                    }
                    //  data.returnval = true;
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

      

        public CheckOutAssetsDCDTO getcontactperson(CheckOutAssetsDCDTO data)
        {
            try
            {
                var contactid = _ATContext.INV_Master_LocationDMO.Single(m => m.MI_Id == data.MI_Id && m.INVMLO_Id == data.INVMLO_Id).HRME_Id;

                if (contactid > 0)
                {
                    data.contactflag = "E";
                    data.get_contactperson = (from a in _ATContext.INV_Master_LocationDMO
                                              from b in _ATContext.INV_Master_SiteDMO
                                              from c in _ATContext.MasterEmployee
                                              where (a.INVMSI_Id == b.INVMSI_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id)
                                              select new CheckOutAssetsDCDTO
                                              {
                                                  INVMLO_Id = a.INVMLO_Id,
                                                  INVMSI_Id = b.INVMSI_Id,
                                                  INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                                  INVMLO_LocationRemarks = a.INVMLO_LocationRemarks,
                                                  INVMLO_InchargeName = a.INVMLO_InchargeName,
                                                  HRME_Id = a.HRME_Id,
                                                  employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " " + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)).Trim(),

                                              }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();
                }
                else
                {
                    data.contactflag = "O";
                    data.get_contactperson = (from a in _ATContext.INV_Master_LocationDMO
                                              from b in _ATContext.INV_Master_SiteDMO

                                              where (a.INVMSI_Id == b.INVMSI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id)
                                              select new CheckOutAssetsDCDTO
                                              {
                                                  INVMLO_Id = a.INVMLO_Id,
                                                  INVMSI_Id = b.INVMSI_Id,
                                                  INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                                  INVMLO_LocationRemarks = a.INVMLO_LocationRemarks,
                                                  INVMLO_InchargeName = a.INVMLO_InchargeName,
                                              }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();
                }

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Master Location load Page:" + ex.Message);
            }
            return data;
        }
        public CheckOutAssetsDCDTO get_avaiablestock(CheckOutAssetsDCDTO data)
        {
            try
            {


                data.availablestock = (from a in _ATContext.INV_Master_ItemDMO
                                       from b in _ATContext.INV_StockDMO
                                       from c in _ATContext.INV_Master_StoreDMO
                                       where (a.INVMI_Id == b.INVMI_Id && b.INVMST_Id == c.INVMST_Id && a.MI_Id == data.MI_Id && b.INVSTO_AvaiableStock != 0 && b.INVSTO_PurchaseDate != null && b.INVMI_Id == data.INVMI_Id && b.INVMST_Id == data.INVMST_Id && b.INVSTO_SalesRate == data.INVSTO_SalesRate)
                                       select new CheckOutAssetsDCDTO
                                       {
                                           INVMI_Id = a.INVMI_Id,
                                           INVSTO_PurchaseDate = b.INVSTO_PurchaseDate,
                                           INVSTO_SalesRate = b.INVSTO_SalesRate,
                                           INVMI_ItemName = a.INVMI_ItemName,
                                           INVSTO_AvaiableStock = b.INVSTO_AvaiableStock,

                                       }).Distinct().OrderByDescending(m => m.INVSTO_PurchaseDate).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Check-out avaiablestock:" + ex.Message);
            }
            return data;
        }
        //
        //public CheckOutAssetsDCDTO editdcNew(CheckOutAssetsDCDTO data)
        //{
        //    try
        //    {


               

        //    }
        //    catch (Exception ex)
        //    {
        //        _logAT.LogInformation("Assets Check-out avaiablestock:" + ex.Message);
        //    }
        //    return data;
        //}
    }
}
