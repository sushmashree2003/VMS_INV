using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class PurchaseReturn_ReportIMPL : Interface.PurchaseReturn_ReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<PurchaseReturn_ReportIMPL> _logInv;
        public PurchaseReturn_ReportIMPL(InventoryContext InvContext, ILogger<PurchaseReturn_ReportIMPL> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }
        public PurchaseReturn_ReportDTO getdataReport(PurchaseReturn_ReportDTO data)
        {
            try
            {
                data.GRNETLIst = _INVContext.INV_M_GRN_Return_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.INVMGRNRET_ActiveFlg == true).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public PurchaseReturn_ReportDTO onreport(PurchaseReturn_ReportDTO data)
        {
            try
            {

                List<long> sec_Id = new List<long>();
                foreach (var c in data.INVMGRNRET_Id_Filter)
                {
                    sec_Id.Add(c.INVMGRNRET_Id);
                }


                if (data.CheckValues == "ALL")
                {
                    data.getReport = (from a in _INVContext.INV_M_GRN_Return_DMO_con
                                      from b in _INVContext.INV_T_GRN_Return_DMO_con
                                      from c in _INVContext.INV_Master_ItemDMO
                                      where (a.INVMGRNRET_Id == b.INVMGRNRET_Id && b.INVMI_Id == c.INVMI_Id &&
                                      sec_Id.Contains(a.INVMGRNRET_Id)
                                      && (a.INVMGRNRET_ReturnDate.Date >= Convert.ToDateTime(data.From_Date).Date && a.INVMGRNRET_ReturnDate.Date <= Convert.ToDateTime(data.To_Date).Date))
                                      select new PurchaseReturn_ReportDTO
                                      {
                                          INVMGRNRET_GRNRETNo = a.INVMGRNRET_GRNRETNo,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVTGRNRET_ReturnQty = b.INVTGRNRET_ReturnQty,
                                          INVMGRNRET_TotalAmount = a.INVMGRNRET_TotalAmount,
                                          INVTGRNRET_ReturnAmount = b.INVTGRNRET_ReturnAmount,
                                          INVMGRNRET_Remarks = a.INVMGRNRET_Remarks,
                                          INVMGRNRETAPP_Id = a.INVMGRNRETAPP_Id,
                                          INVMGRNET_Id =a.INVMGRNRET_Id
                                      }
          ).Distinct().OrderBy(t => t.INVMGRNRET_GRNRETNo).ToArray();
                }
                else if (data.CheckValues == "Approved")
                {
                    data.getReport = (from a in _INVContext.INV_M_GRN_Return_DMO_con
                                      from b in _INVContext.INV_T_GRN_Return_DMO_con
                                      from c in _INVContext.INV_Master_ItemDMO
                                      where (a.INVMGRNRET_Id == b.INVMGRNRET_Id && a.INVMGRNRETAPP_Id > 0 && b.INVMI_Id == c.INVMI_Id &&
                                      sec_Id.Contains(a.INVMGRNRET_Id)
                                      && (a.INVMGRNRET_ReturnDate.Date >= Convert.ToDateTime(data.From_Date).Date && a.INVMGRNRET_ReturnDate.Date <= Convert.ToDateTime(data.To_Date).Date))
                                      select new PurchaseReturn_ReportDTO
                                      {
                                          INVMGRNRET_GRNRETNo = a.INVMGRNRET_GRNRETNo,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVTGRNRET_ReturnQty = b.INVTGRNRET_ReturnQty,
                                          INVMGRNRET_TotalAmount = a.INVMGRNRET_TotalAmount,
                                          INVTGRNRET_ReturnAmount = b.INVTGRNRET_ReturnAmount,
                                          INVMGRNRET_Remarks = a.INVMGRNRET_Remarks,
                                          INVMGRNRETAPP_Id = a.INVMGRNRETAPP_Id
                                      }


                                  ).Distinct().ToArray();
                }
                else if (data.CheckValues == "NotApproved")
                {
                    data.getReport = (from a in _INVContext.INV_M_GRN_Return_DMO_con
                                      from b in _INVContext.INV_T_GRN_Return_DMO_con
                                      from c in _INVContext.INV_Master_ItemDMO
                                      where (a.INVMGRNRET_Id == b.INVMGRNRET_Id && a.INVMGRNRETAPP_Id == 0 && b.INVMI_Id == c.INVMI_Id &&
                                      sec_Id.Contains(a.INVMGRNRET_Id)
                                      && (a.INVMGRNRET_ReturnDate.Date >= Convert.ToDateTime(data.From_Date).Date && a.INVMGRNRET_ReturnDate.Date <= Convert.ToDateTime(data.To_Date).Date))
                                      select new PurchaseReturn_ReportDTO
                                      {
                                          INVMGRNRET_GRNRETNo = a.INVMGRNRET_GRNRETNo,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVTGRNRET_ReturnQty = b.INVTGRNRET_ReturnQty,
                                          INVMGRNRET_TotalAmount = a.INVMGRNRET_TotalAmount,
                                          INVTGRNRET_ReturnAmount = b.INVTGRNRET_ReturnAmount,
                                          INVMGRNRET_Remarks = a.INVMGRNRET_Remarks,
                                          INVMGRNRETAPP_Id = a.INVMGRNRETAPP_Id
                                      }


                                  ).Distinct().ToArray();
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
