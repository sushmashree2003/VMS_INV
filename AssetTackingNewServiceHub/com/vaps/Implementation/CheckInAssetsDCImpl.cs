using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
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
    public class CheckInAssetsDCImpl : Interface.CheckInAssetsDCInterface
    {
        public AssetTrackingContext _ATContext;
        public InventoryContext _INVContext;
        ILogger<CheckInAssetsDCImpl> _logAT;
        public DomainModelMsSqlServerContext _db;
        public CheckInAssetsDCImpl(AssetTrackingContext ATContext, ILogger<CheckInAssetsDCImpl> log, DomainModelMsSqlServerContext db)
        {
            _ATContext = ATContext;
            _logAT = log;
            _db = db;
        }

        public CheckInAssetsDCDTO getloaddata(CheckInAssetsDCDTO data)
        {
            try
            {

                data.instlist = _ATContext.Institution.Where(m => m.MI_Id == data.MI_Id && m.MI_ActiveFlag == 1).OrderBy(m => m.MI_Id).ToArray();
                data.get_checkin = (from a in _ATContext.INV_Asset_CheckInDMO
                                    from b in _ATContext.INV_Master_StoreDMO
                                    from c in _ATContext.INV_Master_ItemDMO
                                    from d in _ATContext.INV_Master_LocationDMO
                                    where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVACI_ActiveFlg==true && (a.INVACI_No==null || a.INVACI_No == "") )
                                    select new CheckInAssetsDCDTO
                                    {
                                        INVACI_Id = a.INVACI_Id,
                                        INVMST_Id = a.INVMST_Id,
                                        INVMS_StoreName = b.INVMS_StoreName,
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = c.INVMI_ItemName,
                                        INVSTO_SalesRate = a.INVSTO_SalesRate,
                                        INVMLO_Id = a.INVMLO_Id,
                                        HRME_Id = a.HRME_Id,
                                        INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                        INVACI_ReceivedBy = a.INVACI_ReceivedBy,
                                        INVACI_CheckInDate = a.INVACI_CheckInDate,
                                        INVACI_CheckInQty = a.INVACI_CheckInQty,
                                        INVACI_CheckInRemarks = a.INVACI_CheckInRemarks,
                                        INVACI_No = a.INVACI_No,
                                        INVACI_ActiveFlg = a.INVACI_ActiveFlg,
                                        UpdatedDate = a.UpdatedDate

                                    }).Distinct().OrderBy(m => m.INVACI_Id).ToArray();

                data.get_checkindc = (from a in _ATContext.INV_Asset_CheckInDMO
                                    from b in _ATContext.INV_Master_StoreDMO
                                    from c in _ATContext.INV_Master_ItemDMO
                                    from d in _ATContext.INV_Master_LocationDMO
                                    where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVACI_ActiveFlg == true && a.INVACI_No != null && a.INVACI_No != "")
                                    select new CheckInAssetsDCDTO
                                    {
                                        INVACI_Id = a.INVACI_Id,
                                        INVMST_Id = a.INVMST_Id,
                                        INVMS_StoreName = b.INVMS_StoreName,
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = c.INVMI_ItemName,
                                        INVSTO_SalesRate = a.INVSTO_SalesRate,
                                        INVMLO_Id = a.INVMLO_Id,
                                        HRME_Id = a.HRME_Id,
                                        INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                        INVACI_ReceivedBy = a.INVACI_ReceivedBy,
                                        INVACI_CheckInDate = a.INVACI_CheckInDate,
                                        INVACI_CheckInQty = a.INVACI_CheckInQty,
                                        INVACI_CheckInRemarks = a.INVACI_CheckInRemarks,
                                        INVACI_No = a.INVACI_No,
                                        INVACI_ActiveFlg = a.INVACI_ActiveFlg,
                                        UpdatedDate = a.UpdatedDate

                                    }).Distinct().OrderBy(m => m.INVACI_Id).ToArray();





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
                _logAT.LogInformation("Assets CheckIn load Page:" + ex.Message);
            }
            return data;
        }

        public CheckInAssetsDCDTO getStore(CheckInAssetsDCDTO data)
        {
            try
            {
                data.get_store = (from a in _ATContext.INV_Asset_CheckOutDMO
                                  from b in _ATContext.INV_Master_LocationDMO
                                  from c in _ATContext.INV_Master_StoreDMO
                                  where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id)
                                  select new CheckInAssetsDCDTO
                                  {
                                      INVMLO_Id = a.INVMLO_Id,
                                      INVMST_Id = a.INVMST_Id,
                                      INVMS_StoreName = c.INVMS_StoreName
                                  }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();

                var contactid = _ATContext.INV_Master_LocationDMO.Single(m => m.MI_Id == data.MI_Id && m.INVMLO_Id == data.INVMLO_Id).HRME_Id;

                if (contactid > 0)
                {
                    data.contactflag = "E";
                    data.get_contactperson = (from a in _ATContext.INV_Master_LocationDMO
                                              from b in _ATContext.INV_Master_SiteDMO
                                              from c in _ATContext.MasterEmployee
                                              where (a.INVMSI_Id == b.INVMSI_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id)
                                              select new CheckOutAssetsDTO
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
                                              select new CheckOutAssetsDTO
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

        public CheckInAssetsDCDTO getitems(CheckInAssetsDCDTO data)
        {
            try
            {
                data.get_items = (from a in _ATContext.INV_Asset_CheckOutDMO
                                  from b in _ATContext.INV_Master_LocationDMO
                                  from c in _ATContext.INV_Master_StoreDMO
                                  from d in _ATContext.INV_Master_ItemDMO
                                  from e in _ATContext.INV_StockDMO
                                  where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && e.INVMST_Id == c.INVMST_Id && e.INVMI_Id == d.INVMI_Id && a.INVSTO_SalesRate == e.INVSTO_SalesRate && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id && a.INVACO_CheckOutQty != 0 && a.INVACO_ActiveFlg == true && d.INVMI_ActiveFlg == true)
                                  select new CheckInAssetsDCDTO
                                  {
                                      // INVACO_Id = a.INVACO_Id,
                                      INVMLO_Id = a.INVMLO_Id,
                                      INVMST_Id = a.INVMST_Id,
                                      INVMI_Id = a.INVMI_Id,
                                      // INVSTO_PurchaseDate = e.INVSTO_PurchaseDate,
                                      INVSTO_SalesRate = e.INVSTO_SalesRate,
                                      // INVSTO_AvaiableStock = e.INVSTO_AvaiableStock,
                                      // INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                      INVMI_ItemName = d.INVMI_ItemName
                                  }).Distinct().OrderByDescending(m => m.INVMI_ItemName).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Check IN get item:" + ex.Message);
            }
            return data;
        }

        public CheckInAssetsDCDTO getdetails(CheckInAssetsDCDTO data)
        {
            try
            {
                data.get_details = (from a in _ATContext.INV_Asset_CheckOutDMO
                                    from b in _ATContext.INV_Master_LocationDMO
                                    from c in _ATContext.INV_Master_StoreDMO
                                    from d in _ATContext.INV_Master_ItemDMO
                                    from e in _ATContext.INV_StockDMO
                                    where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == b.MI_Id
                                    && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVSTO_SalesRate == e.INVSTO_SalesRate && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id && a.INVMI_Id == data.INVMI_Id && a.INVSTO_SalesRate == data.INVSTO_SalesRate && a.INVACO_ActiveFlg == true && d.INVMI_ActiveFlg == true)
                                    select new CheckInAssetsDCDTO
                                    {
                                        INVMST_Id = a.INVMST_Id,
                                        INVMI_Id = a.INVMI_Id,
                                        INVMLO_Id = a.INVMLO_Id,
                                        INVSTO_SalesRate = e.INVSTO_SalesRate,
                                        INVSTO_AvaiableStock = a.INVACO_CheckOutQty
                                    }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Check In Item Details:" + ex.Message);
            }
            return data;
        }

        public CheckInAssetsDCDTO savedetails(CheckInAssetsDCDTO data)
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
                        var result = _ATContext.INV_Asset_CheckInDMO.Single(t => t.INVACI_Id == item.INVACI_Id);
                        result.INVACI_No = data.trans_id;
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


                data.get_checkin = (from a in _ATContext.INV_Asset_CheckInDMO
                                    from b in _ATContext.INV_Master_StoreDMO
                                    from c in _ATContext.INV_Master_ItemDMO
                                    from d in _ATContext.INV_Master_LocationDMO
                                    where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVACI_ActiveFlg == true && (a.INVACI_No == null || a.INVACI_No == ""))
                                    select new CheckInAssetsDCDTO
                                    {
                                        INVACI_Id = a.INVACI_Id,
                                        INVMST_Id = a.INVMST_Id,
                                        INVMS_StoreName = b.INVMS_StoreName,
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = c.INVMI_ItemName,
                                        INVSTO_SalesRate = a.INVSTO_SalesRate,
                                        INVMLO_Id = a.INVMLO_Id,
                                        HRME_Id = a.HRME_Id,
                                        INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                        INVACI_ReceivedBy = a.INVACI_ReceivedBy,
                                        INVACI_CheckInDate = a.INVACI_CheckInDate,
                                        INVACI_CheckInQty = a.INVACI_CheckInQty,
                                        INVACI_CheckInRemarks = a.INVACI_CheckInRemarks,
                                        INVACI_No = a.INVACI_No,
                                        INVACI_ActiveFlg = a.INVACI_ActiveFlg,
                                        UpdatedDate = a.UpdatedDate

                                    }).Distinct().OrderBy(m => m.INVACI_Id).ToArray();

                data.get_checkindc = (from a in _ATContext.INV_Asset_CheckInDMO
                                      from b in _ATContext.INV_Master_StoreDMO
                                      from c in _ATContext.INV_Master_ItemDMO
                                      from d in _ATContext.INV_Master_LocationDMO
                                      where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVACI_ActiveFlg == true && a.INVACI_No != null && a.INVACI_No != "")
                                      select new CheckInAssetsDCDTO
                                      {
                                          INVACI_Id = a.INVACI_Id,
                                          INVMST_Id = a.INVMST_Id,
                                          INVMS_StoreName = b.INVMS_StoreName,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVSTO_SalesRate = a.INVSTO_SalesRate,
                                          INVMLO_Id = a.INVMLO_Id,
                                          HRME_Id = a.HRME_Id,
                                          INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                          INVACI_ReceivedBy = a.INVACI_ReceivedBy,
                                          INVACI_CheckInDate = a.INVACI_CheckInDate,
                                          INVACI_CheckInQty = a.INVACI_CheckInQty,
                                          INVACI_CheckInRemarks = a.INVACI_CheckInRemarks,
                                          INVACI_No = a.INVACI_No,
                                          INVACI_ActiveFlg = a.INVACI_ActiveFlg,
                                          UpdatedDate = a.UpdatedDate

                                      }).Distinct().OrderBy(m => m.INVACI_Id).ToArray();

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Asset CheckIn savedata :" + ex.Message);
            }
            return data;
        }

        public CheckInAssetsDCDTO deactive(CheckInAssetsDCDTO data)
        {
            try
            {
                var result = _ATContext.INV_Asset_CheckInDMO.Single(t => t.INVACI_Id == data.INVACI_Id);

                if (result.INVACI_ActiveFlg == true)
                {
                    result.INVACI_ActiveFlg = false;
                }
                else if (result.INVACI_ActiveFlg == false)
                {
                    result.INVACI_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
                if (returnval > 0)
                {
                    var contactExistsP = _ATContext.Database.ExecuteSqlCommand("INV_StockupdatesCheckIn @p0, @p1,@p2, @p3,@p4,@p5", data.MI_Id, data.INVMST_Id, data.INVMLO_Id, data.INVMI_Id, data.INVSTO_SalesRate, data.INVACI_Id);
                    if (contactExistsP > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                    // data.returnval = true;
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
