using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Implementation
{
    public class AssetsTagCheckInDCIMPL : Interface.AssetsTagCheckInDCInterface
    {
        public AssetTrackingContext _ATContext;
        public InventoryContext _INVContext;
        ILogger<AssetsTagCheckInDCIMPL> _logAT;
        public DomainModelMsSqlServerContext _db;
        public AssetsTagCheckInDCIMPL(AssetTrackingContext ATContext, ILogger<AssetsTagCheckInDCIMPL> log, DomainModelMsSqlServerContext db)
        {
            _ATContext = ATContext;
            _logAT = log;
            _db = db;
        }

        public AssetsTagCheckInDTO getloaddata(AssetsTagCheckInDTO data)
        {
            try
            {
                data.instlist = _ATContext.Institution.Where(m => m.MI_Id == data.MI_Id && m.MI_ActiveFlag == 1).OrderBy(m => m.MI_Id).ToArray();
                data.get_checkin = (from a in _ATContext.INV_AssetTag_CheckInDMO
                                    from b in _ATContext.INV_Master_StoreDMO
                                    from c in _ATContext.INV_Master_ItemDMO
                                    from d in _ATContext.INV_Master_LocationDMO
                                    where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVATCI_ActiveFlg == true && (a.INVATCI_No == null || a.INVATCI_No == ""))
                                    select new AssetsTagCheckInDTO
                                    {
                                        INVATCI_Id = a.INVATCI_Id,
                                        INVMST_Id = a.INVMST_Id,
                                        INVMS_StoreName = b.INVMS_StoreName,
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = c.INVMI_ItemName,
                                        //INVSTO_SalesRate = a.INVSTO_SalesRate,
                                        INVMLO_Id = a.INVMLO_Id,
                                        HRME_Id = a.HRME_Id,
                                        INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                        INVATCI_ReceivedBy = a.INVATCI_ReceivedBy,
                                        INVATCI_CheckInDate = a.INVATCI_CheckInDate,
                                        INVATCI_CheckInQty = a.INVATCI_CheckInQty,
                                        INVATCI_CheckInRemarks = a.INVATCI_CheckInRemarks,
                                        INVATCI_No = a.INVATCI_No,
                                        INVATCI_ActiveFlg = a.INVATCI_ActiveFlg,
                                        UpdatedDate = a.UpdatedDate

                                    }).Distinct().OrderBy(m => m.INVATCI_Id).ToArray();

                data.get_checkindc = (from a in _ATContext.INV_AssetTag_CheckInDMO
                                      from b in _ATContext.INV_Master_StoreDMO
                                      from c in _ATContext.INV_Master_ItemDMO
                                      from d in _ATContext.INV_Master_LocationDMO
                                      where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVATCI_ActiveFlg == true && a.INVATCI_No != null && a.INVATCI_No != "")
                                      select new AssetsTagCheckInDTO
                                      {
                                          INVATCI_Id = a.INVATCI_Id,
                                          INVMST_Id = a.INVMST_Id,
                                          INVMS_StoreName = b.INVMS_StoreName,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                         // INVSTO_SalesRate = a.INVSTO_SalesRate,
                                          INVMLO_Id = a.INVMLO_Id,
                                          HRME_Id = a.HRME_Id,
                                          INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                          INVATCI_ReceivedBy = a.INVATCI_ReceivedBy,
                                          INVATCI_CheckInDate = a.INVATCI_CheckInDate,
                                          INVATCI_CheckInQty = a.INVATCI_CheckInQty,
                                          INVATCI_CheckInRemarks = a.INVATCI_CheckInRemarks,
                                          INVATCI_No = a.INVATCI_No,
                                          INVATCI_ActiveFlg = a.INVATCI_ActiveFlg,
                                          UpdatedDate = a.UpdatedDate

                                      }).Distinct().OrderBy(m => m.INVATCI_Id).ToArray();


            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets CheckIn load Page:" + ex.Message);
            }
            return data;
        }

        public AssetsTagCheckInDTO getStore(AssetsTagCheckInDTO data)
        {
            try
            {
              

               


            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Master Location load Page:" + ex.Message);
            }
            return data;
        }

        public AssetsTagCheckInDTO getitems(AssetsTagCheckInDTO data)
        {
            try
            {
               

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Check IN get item:" + ex.Message);
            }
            return data;
        }

        public AssetsTagCheckInDTO getdetails(AssetsTagCheckInDTO data)
        {
            try
            {
                

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Check In Item Details:" + ex.Message);
            }
            return data;
        }

        public AssetsTagCheckInDTO savedetails(AssetsTagCheckInDTO data)
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

                if (data.AssetIditems != null)
                {
                    foreach (var item in data.AssetIditems)
                    {
                        var result = _ATContext.INV_AssetTag_CheckInDMO.Single(t => t.INVATCI_Id == item.AssetId);
                        result.INVATCI_No = data.trans_id;
                        result.UpdatedDate = DateTime.Now;
                        _ATContext.Update(result);

                    }
                }
                var contactExists = _ATContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = "Save";
                }
                else
                {
                    data.returnval = "admin";
                }
                data.get_checkin = (from a in _ATContext.INV_AssetTag_CheckInDMO
                                    from b in _ATContext.INV_Master_StoreDMO
                                    from c in _ATContext.INV_Master_ItemDMO
                                    from d in _ATContext.INV_Master_LocationDMO
                                    where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVATCI_ActiveFlg == true && (a.INVATCI_No == null || a.INVATCI_No == ""))
                                    select new AssetsTagCheckInDTO
                                    {
                                        INVATCI_Id = a.INVATCI_Id,
                                        INVMST_Id = a.INVMST_Id,
                                        INVMS_StoreName = b.INVMS_StoreName,
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = c.INVMI_ItemName,
                                        //INVSTO_SalesRate = a.INVSTO_SalesRate,
                                        INVMLO_Id = a.INVMLO_Id,
                                        HRME_Id = a.HRME_Id,
                                        INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                        INVATCI_ReceivedBy = a.INVATCI_ReceivedBy,
                                        INVATCI_CheckInDate = a.INVATCI_CheckInDate,
                                        INVATCI_CheckInQty = a.INVATCI_CheckInQty,
                                        INVATCI_CheckInRemarks = a.INVATCI_CheckInRemarks,
                                        INVATCI_No = a.INVATCI_No,
                                        INVATCI_ActiveFlg = a.INVATCI_ActiveFlg,
                                        UpdatedDate = a.UpdatedDate

                                    }).Distinct().OrderBy(m => m.INVATCI_Id).ToArray();

                data.get_checkindc = (from a in _ATContext.INV_AssetTag_CheckInDMO
                                      from b in _ATContext.INV_Master_StoreDMO
                                      from c in _ATContext.INV_Master_ItemDMO
                                      from d in _ATContext.INV_Master_LocationDMO
                                      where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVATCI_ActiveFlg == true && a.INVATCI_No != null && a.INVATCI_No != "")
                                      select new AssetsTagCheckInDTO
                                      {
                                          INVATCI_Id = a.INVATCI_Id,
                                          INVMST_Id = a.INVMST_Id,
                                          INVMS_StoreName = b.INVMS_StoreName,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          // INVSTO_SalesRate = a.INVSTO_SalesRate,
                                          INVMLO_Id = a.INVMLO_Id,
                                          HRME_Id = a.HRME_Id,
                                          INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                          INVATCI_ReceivedBy = a.INVATCI_ReceivedBy,
                                          INVATCI_CheckInDate = a.INVATCI_CheckInDate,
                                          INVATCI_CheckInQty = a.INVATCI_CheckInQty,
                                          INVATCI_CheckInRemarks = a.INVATCI_CheckInRemarks,
                                          INVATCI_No = a.INVATCI_No,
                                          INVATCI_ActiveFlg = a.INVATCI_ActiveFlg,
                                          UpdatedDate = a.UpdatedDate

                                      }).Distinct().OrderBy(m => m.INVATCI_Id).ToArray();


            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Asset CheckIn savedata :" + ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public AssetsTagCheckInDTO deactive(AssetsTagCheckInDTO data)
        {
            try
            {
               
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    }
}
