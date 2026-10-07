using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Implementation
{
    public class AssetTagCheckOutDCIMPL : Interface.AssetTagCheckOutDCInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagCheckOutDCIMPL> _logAT;
        public DomainModelMsSqlServerContext _db;
        public AssetTagCheckOutDCIMPL(AssetTrackingContext ATContext, ILogger<AssetTagCheckOutDCIMPL> log, DomainModelMsSqlServerContext db)
        {
            _ATContext = ATContext;
            _logAT = log;
            _db = db;
        }

        public AssetTagCheckOutDTO getloaddata(AssetTagCheckOutDTO data)
        {
            try
            {
                data.instlist = _ATContext.Institution.Where(m => m.MI_Id == data.MI_Id && m.MI_ActiveFlag == 1).OrderBy(m => m.MI_Id).ToArray();
                data.get_locations = _ATContext.INV_Master_LocationDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMLO_ActiveFlg == true).OrderBy(m => m.INVMLO_Id).ToArray();

                data.get_checkout = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                     from b in _ATContext.INV_Master_StoreDMO
                                     from c in _ATContext.INV_Master_ItemDMO
                                     from d in _ATContext.INV_Master_LocationDMO
                                     from e in _ATContext.INV_StockDMO
                                     from f in _ATContext.INV_Asset_AssetTagDMO
                                     where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && (a.INVMLO_No == null || a.INVMLO_No == "") && a.INVATCO_ActiveFlg == true
                                     && f.INVAAT_Id == a.INVAAT_Id  && a.INVATCO_CheckInFlg==true && a.INVATCO_ActiveFlg==true)
                                     select new AssetTagCheckOutDTO
                                     {
                                         INVATCO_Id = a.INVATCO_Id,
                                         INVMST_Id = a.INVMST_Id,
                                         INVMS_StoreName = b.INVMS_StoreName,
                                         INVMI_Id = a.INVMI_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         // INVSTO_SalesRate = a.INVSTO_SalesRate,
                                         INVMLO_Id = a.INVMLO_Id,
                                         HRME_Id = a.HRME_Id,
                                         INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                         INVATCO_ReceivedBy = a.INVATCO_ReceivedBy,
                                         INVATCO_CheckoutDate = a.INVATCO_CheckoutDate,
                                         INVATCO_CheckOutQty = a.INVATCO_CheckOutQty,
                                         INVATCO_CheckOutRemarks = a.INVATCO_CheckOutRemarks,
                                         INVATCO_ActiveFlg = a.INVATCO_ActiveFlg,
                                         INVMLO_No = a.INVMLO_No,
                                         UpdatedDate = a.UpdatedDate,
                                         INVAAT_AssetId = f.INVAAT_AssetId,
                                         INVAAT_ModelNo = f.INVAAT_ModelNo,
                                         INVAAT_SerialNo = f.INVAAT_SerialNo,
                                         INVAAT_PurchaseDate = f.INVAAT_PurchaseDate

                                     }).Distinct().OrderBy(m => m.INVATCO_Id).ToArray();



                //two
                data.get_checkoutdclist = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                           from b in _ATContext.INV_Master_StoreDMO
                                           from c in _ATContext.INV_Master_ItemDMO
                                           from d in _ATContext.INV_Master_LocationDMO
                                           from e in _ATContext.INV_StockDMO
                                           from f in _ATContext.INV_Asset_AssetTagDMO
                                           where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVMLO_No != null && a.INVMLO_No != "" && a.INVATCO_ActiveFlg == true && f.INVAAT_Id == a.INVAAT_Id)
                                           select new AssetTagCheckOutDTO
                                           {
                                               INVATCO_Id = a.INVATCO_Id,
                                               INVMST_Id = a.INVMST_Id,
                                               INVMS_StoreName = b.INVMS_StoreName,
                                               INVMI_Id = a.INVMI_Id,
                                               INVMI_ItemName = c.INVMI_ItemName,
                                               // INVSTO_SalesRate = a.INVSTO_SalesRate,
                                               INVMLO_Id = a.INVMLO_Id,
                                               HRME_Id = a.HRME_Id,
                                               INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                               INVATCO_ReceivedBy = a.INVATCO_ReceivedBy,
                                               INVATCO_CheckoutDate = a.INVATCO_CheckoutDate,
                                               INVATCO_CheckOutQty = a.INVATCO_CheckOutQty,
                                               INVATCO_CheckOutRemarks = a.INVATCO_CheckOutRemarks,
                                               INVATCO_ActiveFlg = a.INVATCO_ActiveFlg,
                                               UpdatedDate = a.UpdatedDate,
                                               INVMLO_No = a.INVMLO_No,
                                               INVAAT_AssetId = f.INVAAT_AssetId,
                                               INVAAT_ModelNo = f.INVAAT_ModelNo,
                                               INVAAT_SerialNo = f.INVAAT_SerialNo,
                                               INVAAT_PurchaseDate = f.INVAAT_PurchaseDate

                                           }).Distinct().OrderBy(m => m.INVATCO_Id).ToArray();


            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Check Out load Page:" + ex.Message);
            }
            return data;
        }
        public AssetTagCheckOutDTO getitems(AssetTagCheckOutDTO data)
        {
            try
            {


            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Checkout get items:" + ex.Message);
            }
            return data;
        }

        public async Task<AssetTagCheckOutDTO> getitemtagdata(AssetTagCheckOutDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag get Data:" + ex.Message);
            }
            return data;
        }

        public AssetTagCheckOutDTO savedata(AssetTagCheckOutDTO data)
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
                        var result = _ATContext.INV_AssetTag_CheckOutDMO.Single(t => t.INVATCO_Id == item.INVATCO_Id);
                        result.INVMLO_No = data.trans_id;
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
                //assetdc

                data.get_checkout = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                     from b in _ATContext.INV_Master_StoreDMO
                                     from c in _ATContext.INV_Master_ItemDMO
                                     from d in _ATContext.INV_Master_LocationDMO
                                     from e in _ATContext.INV_StockDMO
                                     where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && (a.INVMLO_No == null || a.INVMLO_No == "") && a.INVATCO_ActiveFlg == true)
                                     select new AssetTagCheckOutDTO
                                     {
                                         INVATCO_Id = a.INVATCO_Id,
                                         INVMST_Id = a.INVMST_Id,
                                         INVMS_StoreName = b.INVMS_StoreName,
                                         INVMI_Id = a.INVMI_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         // INVSTO_SalesRate = a.INVSTO_SalesRate,
                                         INVMLO_Id = a.INVMLO_Id,
                                         HRME_Id = a.HRME_Id,
                                         INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                         INVATCO_ReceivedBy = a.INVATCO_ReceivedBy,
                                         INVATCO_CheckoutDate = a.INVATCO_CheckoutDate,
                                         INVATCO_CheckOutQty = a.INVATCO_CheckOutQty,
                                         INVATCO_CheckOutRemarks = a.INVATCO_CheckOutRemarks,
                                         INVATCO_ActiveFlg = a.INVATCO_ActiveFlg,
                                         UpdatedDate = a.UpdatedDate

                                     }).Distinct().OrderBy(m => m.INVATCO_Id).ToArray();



                //two
                data.get_checkoutdclist = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                           from b in _ATContext.INV_Master_StoreDMO
                                           from c in _ATContext.INV_Master_ItemDMO
                                           from d in _ATContext.INV_Master_LocationDMO
                                           from e in _ATContext.INV_StockDMO
                                           from f in _ATContext.INV_Asset_AssetTagDMO
                                           where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id && a.INVMLO_No != null && a.INVMLO_No != "" && a.INVATCO_ActiveFlg == true && f.INVAAT_Id == a.INVAAT_Id)
                                           select new AssetTagCheckOutDTO
                                           {
                                               INVATCO_Id = a.INVATCO_Id,
                                               INVMST_Id = a.INVMST_Id,
                                               INVMS_StoreName = b.INVMS_StoreName,
                                               INVMI_Id = a.INVMI_Id,
                                               INVMI_ItemName = c.INVMI_ItemName,
                                               // INVSTO_SalesRate = a.INVSTO_SalesRate,
                                               INVMLO_Id = a.INVMLO_Id,
                                               HRME_Id = a.HRME_Id,
                                               INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                               INVATCO_ReceivedBy = a.INVATCO_ReceivedBy,
                                               INVATCO_CheckoutDate = a.INVATCO_CheckoutDate,
                                               INVATCO_CheckOutQty = a.INVATCO_CheckOutQty,
                                               INVATCO_CheckOutRemarks = a.INVATCO_CheckOutRemarks,
                                               INVATCO_ActiveFlg = a.INVATCO_ActiveFlg,
                                               UpdatedDate = a.UpdatedDate,
                                               INVMLO_No = a.INVMLO_No,
                                               INVAAT_AssetId = f.INVAAT_AssetId,
                                               INVAAT_ModelNo = f.INVAAT_ModelNo,
                                               INVAAT_SerialNo = f.INVAAT_SerialNo,
                                               INVAAT_PurchaseDate = f.INVAAT_PurchaseDate
                                           }).Distinct().OrderBy(m => m.INVATCO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Asset tag check out savedata :" + ex.Message);
            }
            return data;
        }


        public AssetTagCheckOutDTO deactive(AssetTagCheckOutDTO data)
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
