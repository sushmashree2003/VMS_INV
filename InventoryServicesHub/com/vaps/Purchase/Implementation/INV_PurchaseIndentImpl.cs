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
using System.Net;
using System.IO;
using System.Net.Mail;
using SendGrid;
using DomainModel.Model;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_PurchaseIndentImpl : Interface.INV_PurchaseIndentInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PurchaseIndentImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_PurchaseIndentImpl(InventoryContext InvContext, ILogger<INV_PurchaseIndentImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_PurchaseIndentDTO getloaddata(INV_PurchaseIndentDTO data)
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
                    cmd.CommandText = "INV_PR_LIST_NEW";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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

                //data.get_prNo = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                //                 from b in _INVContext.MasterEmployee

                //                 where ( a.MI_Id == b.MI_Id && a.INVMPR_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPR_PICreatedFlg == false)
                //                 select new INV_PurchaseIndentDTO
                //                 {
                //                     INVMPR_Id = a.INVMPR_Id,
                //                     HRME_Id = a.HRME_Id,
                //                     employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                //                     INVMPR_PRNo = a.INVMPR_PRNo,
                //                     INVMPR_PRDate = a.INVMPR_PRDate,
                //                     INVMPR_Remarks = a.INVMPR_Remarks,
                //                     INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                //                     INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                //                 }).Distinct().OrderByDescending(i => i.INVMPR_Id).ToArray();

                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_ItemName).ToArray();

                //data.get_item = (from a in _INVContext.INV_Master_ItemDMO
                //                 from b in _INVContext.INV_StockDMO
                //                 from c in _INVContext.INV_Master_UOMDMO
                //                 where (a.INVMI_Id == b.INVMI_Id && c.INVMUOM_Id == a.INVMUOM_Id && a.MI_Id == b.MI_Id && b.INVSTO_AvaiableStock != 0 && a.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id)
                //                 select new INV_PurchaseIndentDTO
                //                 {
                //                     INVMI_Id = a.INVMI_Id,
                //                     INVMUOM_Id = a.INVMUOM_Id,
                //                     INVMI_ItemName = a.INVMI_ItemName,
                //                     INVMI_ItemCode = a.INVMI_ItemCode,
                //                 }).Distinct().OrderBy(i => i.INVMI_ItemName).ToArray();

                //data.get_purchaseindent = _INVContext.INV_M_PurchaseIndentDMO.Where(i => i.MI_Id == data.MI_Id).ToArray();

                data.get_purchaseindent = (from o in _INVContext.INV_M_PurchaseIndentDMO
                                           from m in _INVContext.INV_T_PurchaseIndentDMO
                                           from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                           from b in _INVContext.INV_T_PurchaseRequisitionDMO

                                           from e in _INVContext.MasterEmployee
                                           from g in _INVContext.HR_Master_Department
                                           where (o.INVMPI_Id == m.INVMPI_Id && m.INVMPR_Id == a.INVMPR_Id && a.INVMPR_Id == b.INVMPR_Id && a.HRME_Id == e.HRME_Id && a.INVMPR_ActiveFlg == true && g.HRMD_Id == e.HRMD_Id && o.MI_Id == data.MI_Id)
                                           select new INV_PurchaseIndentDTO
                                           {
                                               INVMPR_Id = a.INVMPR_Id,
                                               INVMPI_Id = o.INVMPI_Id,
                                               HRME_Id = a.HRME_Id,
                                               INVMPI_PINo = o.INVMPI_PINo,
                                               INVMPI_PIDate = o.INVMPI_PIDate,
                                               departmentname = g.HRMD_DepartmentName,
                                               INVMPI_ReferenceNo=o.INVMPI_ReferenceNo,
                                               employeename = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : " " + e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),

                                               INVMPI_ApproxTotAmount = o.INVMPI_ApproxTotAmount,
                                               INVMPI_POCreatedFlg=o.INVMPI_POCreatedFlg,
                                               INVMPI_ActiveFlg=o.INVMPI_ActiveFlg,
                                             
                                           }).Distinct().OrderBy(i => i.INVTPI_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_PurchaseIndentDTO> getpidetails(INV_PurchaseIndentDTO data)
        {
            try
            {
                data.get_pimodel = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                    from b in _INVContext.INV_T_PurchaseIndentDMO
                                    from c in _INVContext.INV_Master_ItemDMO
                                    from d in _INVContext.INV_Master_UOMDMO
                                    where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                             && a.MI_Id == data.MI_Id && a.INVMPI_Id == data.INVMPI_Id)
                                    select new INV_PurchaseIndentDTO
                                    {
                                        INVMPI_Id = a.INVMPI_Id,
                                        INVTPI_Id = b.INVTPI_Id,
                                        INVMPI_PINo = a.INVMPI_PINo,
                                        INVMI_Id = b.INVMI_Id,
                                        INVMUOM_Id = b.INVMUOM_Id,
                                        INVMPR_Id = b.INVMPR_Id,
                                        INVMI_ItemName = c.INVMI_ItemName,
                                        INVMUOM_UOMName = d.INVMUOM_UOMName,
                                        INVTPI_PRQty = b.INVTPI_PRQty,
                                        INVMPI_PIDate = a.INVMPI_PIDate,
                                        INVMPI_ReferenceNo = a.INVMPI_ReferenceNo,
                                        INVTPI_PIQty = b.INVTPI_PIQty,
                                        INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                        INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                        INVMPI_Remarks = a.INVMPI_Remarks,
                                        INVMPI_ApproxTotAmount = a.INVMPI_ApproxTotAmount
                                    }).Distinct().OrderByDescending(i => i.INVMPI_Id).ToArray();


                if (data.checkArray.Length > 0)
                {
                    string ckdids = "0";
                    if (data.checkArray != null)
                    {
                        foreach (var c in data.checkArray)
                        {
                            ckdids = ckdids + "," + c.INVMPI_Id;
                        }
                    }
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_PI_ReceiptPrint";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ckd_Ids",
                        SqlDbType.VarChar)
                        {
                            Value = ckdids
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
                            data.get_PIReceipt = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent Item details:" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseIndentDTO getprDetail(INV_PurchaseIndentDTO data)
        {
            try
            {
                data.get_indentDetail = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                         from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                         from c in _INVContext.INV_Master_ItemDMO
                                         from d in _INVContext.INV_Master_UOMDMO
                                         from e in _INVContext.MasterEmployee
                                             //from g in _INVContext.HR_Master_Department && g.HRMD_Id==e.HRMD_Id 
                                         where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && a.HRME_Id == e.HRME_Id 
                                         && b.INVMUOM_Id == d.INVMUOM_Id && a.INVMPR_ActiveFlg == true 
                                         && c.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVMPR_Id == data.INVMPR_Id)
                                          select new INV_PurchaseRequisitionDTO
                                         {
                                             INVMPR_Id = a.INVMPR_Id,
                                             INVTPR_Id = b.INVTPR_Id,
                                             INVMI_Id = b.INVMI_Id,
                                             INVMUOM_Id = b.INVMUOM_Id,
                                             HRME_Id = a.HRME_Id,
                                             INVMPR_PRNo = a.INVMPR_PRNo,
                                             INVMI_ItemName = c.INVMI_ItemName,
                                             //departmentname = g.HRMD_DepartmentName,
                                             INVMI_ItemCode = c.INVMI_ItemCode,
                                             INVMUOM_UOMName = d.INVMUOM_UOMName,
                                             employeename = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : e.HRME_EmployeeFirstName) +
                                             (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " +
                                             e.HRME_EmployeeMiddleName) +
                                             (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " +
                                             e.HRME_EmployeeLastName)).Trim(),
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

                data.requisition_remarks = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                            from e in _INVContext.MasterEmployee
                                            where (a.HRME_Id == e.HRME_Id && a.INVMPR_Id == data.INVMPR_Id)
                                            select new INV_PurchaseIndentDTO
                                            {
                                                employeename = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : e.HRME_EmployeeFirstName) +
                                             (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " +
                                             e.HRME_EmployeeMiddleName) +
                                             (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " +
                                             e.HRME_EmployeeLastName)).Trim(),
                                                INVMPR_PRNo = a.INVMPR_PRNo,
                                                INVMPR_PRDate = a.INVMPR_PRDate,
                                                INVMPR_Remarks = a.INVMPR_Remarks,
                                                INVMPR_Id = a.INVMPR_Id
                                            }).Distinct().ToArray(); ;

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent PR Change:" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseIndentDTO get_details(INV_PurchaseIndentDTO data)
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

        public INV_PurchaseIndentDTO savedetails(INV_PurchaseIndentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                if (data.INVMPI_Id != 0)
                {
                    var result = _INVContext.INV_M_PurchaseIndentDMO.Single(t => t.MI_Id == data.MI_Id && t.INVMPI_Id == data.INVMPI_Id);
                    result.MI_Id = data.MI_Id;
                    result.INVMPI_PIDate = data.INVMPI_PIDate;
                    result.INVMPI_Remarks = data.INVMPI_Remarks;
                    result.INVMPI_ReferenceNo = data.INVMPI_ReferenceNo;
                    result.INVMPI_ApproxTotAmount = data.INVMPI_ApproxTotAmount;
                    result.INVMPI_POCreatedFlg = false;
                    result.INVMPI_ActiveFlg = true;
                    result.INVMPI_RejectFlg = false;

                    result.INVMPI_UpdatedBy = data.UserId;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);

                    foreach (var r in data.arrayPI)
                    {
                        var res1 = _INVContext.INV_T_PurchaseIndentDMO.Where(a => a.INVTPI_Id == r.INVTPI_Id).ToList();
                        if (res1.Count > 0)
                        {
                            var res11 = _INVContext.INV_T_PurchaseIndentDMO.Single(a => a.INVTPI_Id == r.INVTPI_Id);
                            res11.INVTPI_PRQty = r.INVTPI_PRQty;
                            res11.INVTPI_PIQty = r.INVTPI_PIQty;
                            res11.INVTPI_PIUnitRate = r.INVTPI_PIUnitRate;
                            res11.INVTPI_ApproxAmount = r.INVTPI_ApproxAmount;
                            res11.INVTPI_Remarks = r.INVTPI_Remarks;
                            res11.INVTPI_ActiveFlg = true;
                            res11.INVTPI_UpdatedBy = data.UserId;
                            res11.UpdatedDate = indianTime;
                            _INVContext.Update(res11);
                        }
                        else
                        {
                            INV_T_PurchaseIndentDMO piu = new INV_T_PurchaseIndentDMO();
                            piu.INVTPI_PRQty = r.INVTPI_PRQty;
                            piu.INVTPI_PIQty = r.INVTPI_PIQty;
                            piu.INVTPI_PIUnitRate = r.INVTPI_PIUnitRate;
                            piu.INVTPI_ApproxAmount = r.INVTPI_ApproxAmount;
                            piu.INVTPI_Remarks = r.INVTPI_Remarks;
                            piu.INVTPI_ActiveFlg = true;
                            piu.INVTPI_RejectFlg = false;
                            piu.INVTPI_ApproveQty = 0;
                            piu.INVTPI_UpdatedBy = data.UserId;
                            piu.UpdatedDate = indianTime;
                            _INVContext.Add(piu);
                        }

                    }
                    var contactExists = _INVContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        var hrmeid = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                        if (hrmeid.Count > 0)
                        {
                            data.HRME_Id = hrmeid.FirstOrDefault().Emp_Code;
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


                    INV_M_PurchaseIndentDMO pi = new INV_M_PurchaseIndentDMO();
                    pi.MI_Id = data.MI_Id;

                    pi.INVMPI_PINo = data.trans_id;
                    pi.INVMPI_PIDate = data.INVMPI_PIDate;
                    pi.INVMPI_Remarks = data.INVMPI_Remarks;
                    pi.INVMPI_ReferenceNo = data.INVMPI_ReferenceNo;
                    pi.INVMPI_ApproxTotAmount = data.INVMPI_ApproxTotAmount;
                    pi.INVMPI_POCreatedFlg = false;
                    pi.INVMPI_ActiveFlg = true;
                    pi.INVMPI_RejectFlg = false;
                    pi.INVMPI_FinalProcessFlag = false;
                    pi.INVMPI_CreatedBy = data.UserId;
                    pi.INVMPI_UpdatedBy = data.UserId;
                    pi.CreatedDate = indianTime;
                    pi.UpdatedDate = indianTime;
                    _INVContext.Add(pi);

                    foreach (var r in data.arrayPI)
                    {
                        INV_T_PurchaseIndentDMO tpi = new INV_T_PurchaseIndentDMO();
                        tpi.MI_Id = data.MI_Id;
                        tpi.INVMPI_Id = pi.INVMPI_Id;
                        tpi.INVMI_Id = r.INVMI_Id;
                        tpi.INVMUOM_Id = r.INVMUOM_Id;
                        tpi.INVMPR_Id = r.INVMPR_Id;
                        tpi.INVTPI_PRQty = r.INVTPI_PRQty;
                        tpi.INVTPI_PIUnitRate = r.INVTPI_PIUnitRate;
                        tpi.INVTPI_ApproxAmount = r.INVTPI_ApproxAmount;
                        tpi.INVTPI_PIQty = r.INVTPI_PIQty;
                        tpi.INVTPI_ApproveQty = 0;
                        tpi.INVTPI_Remarks = r.INVTPI_Remarks;
                        tpi.INVTPI_ActiveFlg = true;
                        tpi.INVTPI_RejectFlg = false;

                        tpi.INVTPI_CreatedBy = data.UserId;
                        tpi.INVTPI_UpdatedBy = data.UserId;

                        tpi.CreatedDate = indianTime;
                        tpi.UpdatedDate = indianTime;
                        _INVContext.Add(tpi);


                        if (data.prflag == true)
                        {

                            var resUpdate = _INVContext.INV_M_PurchaseRequisitionDMO.Single(a => a.MI_Id == data.MI_Id && a.INVMPR_Id == r.INVMPR_Id);
                            if (resUpdate.INVMPR_PICreatedFlg == false)
                            {
                                resUpdate.INVMPR_PICreatedFlg = true;
                            }
                            _INVContext.Update(resUpdate);

                            var resUpdateqty = _INVContext.INV_T_PurchaseRequisitionDMO.Single(a => a.INVMPR_Id == r.INVMPR_Id && a.INVMI_Id == r.INVMI_Id && a.INVTPR_PRQty == r.INVTPI_PRQty && a.INVTPR_Id == r.INVTPR_Id);
                            resUpdateqty.INVTPR_ApprovedQty = r.INVTPI_PIQty;
                            _INVContext.Update(resUpdateqty);
                        }
                    }
                    var contactExists = _INVContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        foreach (var item in data.arrayPIr)
                        {
                            var rejlist = _INVContext.INV_T_PurchaseRequisitionDMO.Single(a => a.INVMPR_Id == item.INVMPR_Id && a.INVMI_Id == item.INVMI_Id && a.INVTPR_PRQty == item.INVTPI_PRQty && a.INVTPR_Id == item.INVTPR_Id);
                            rejlist.INVTPR_RejectFlg = true;
                            _INVContext.Update(rejlist);

                        }
                        var ext = _INVContext.SaveChanges();
                        if (ext > 0)
                        {
                            data.returnval = true;
                        }
                        data.returnval = true;

                        var primaryid = _INVContext.INV_M_PurchaseIndentDMO.Where(e => e.INVMPI_Id == pi.INVMPI_Id).ToList();
                        if (primaryid.Count > 0)
                        {
                            long hrid = 0;

                            var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                            if (hrmeid1.Count > 0)
                            {
                                hrid = hrmeid1.FirstOrDefault().Emp_Code;
                            }

                            emaildata(primaryid.FirstOrDefault().INVMPI_Id, hrid, data.MI_Id, data.UserId);
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
                _logInv.LogInformation("Purchase Indent savedata :" + ex.Message);
            }

            return data;
        }

        public INV_PurchaseIndentDTO edit(INV_PurchaseIndentDTO data)
        {
            try
            {
                data.get_editPI = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                   from b in _INVContext.INV_T_PurchaseIndentDMO
                                   from c in _INVContext.INV_Master_ItemDMO
                                   from d in _INVContext.INV_Master_UOMDMO
                                   where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                            && a.INVMPI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVTPI_Id == data.INVTPI_Id)
                                   select new INV_PurchaseIndentDTO
                                   {
                                       INVMPI_Id = a.INVMPI_Id,
                                       INVMPI_PINo = a.INVMPI_PINo,
                                       INVMI_Id = b.INVMI_Id,
                                       INVMUOM_Id = b.INVMUOM_Id,
                                       INVMPR_Id = b.INVMPR_Id,
                                       INVMI_ItemName = c.INVMI_ItemName,
                                       INVMUOM_UOMName = d.INVMUOM_UOMName,
                                       INVMPI_PIDate = a.INVMPI_PIDate,
                                       INVMPI_ReferenceNo = a.INVMPI_ReferenceNo,
                                       INVTPI_PIQty = b.INVTPI_PIQty,
                                       INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                       INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                       INVMPI_Remarks = a.INVMPI_Remarks,
                                       INVMPI_ApproxTotAmount = a.INVMPI_ApproxTotAmount,
                                       INVMPI_POCreatedFlg = a.INVMPI_POCreatedFlg,
                                       INVMPI_ActiveFlg = a.INVMPI_ActiveFlg,
                                       INVTPI_Id = b.INVTPI_Id,
                                       INVTPI_Remarks = b.INVTPI_Remarks,
                                       INVTPI_ActiveFlg = b.INVTPI_ActiveFlg
                                   }).Distinct().OrderBy(i => i.INVMPI_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent edit:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseIndentDTO deactiveM(INV_PurchaseIndentDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_PurchaseIndentDMO.Single(t => t.INVMPI_Id == data.INVMPI_Id);

                if (result.INVMPI_ActiveFlg == true)
                {
                    result.INVMPI_ActiveFlg = false;
                }
                else if (result.INVMPI_ActiveFlg == false)
                {
                    result.INVMPI_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_PurchaseIndentDMO.Where(t => t.INVMPI_Id == data.INVMPI_Id).ToList();
                if (resultt.Count > 0)
                {
                    foreach (var rt in resultt)
                    {
                        // var rest = _INVContext.INV_T_PurchaseIndentDMO.Single(t => t.INVMPI_Id == rt.INVMPI_Id);
                        if (result.INVMPI_ActiveFlg == true)
                        {
                            rt.INVTPI_ActiveFlg = true;
                        }
                        if (result.INVMPI_ActiveFlg == false)
                        {
                            rt.INVTPI_ActiveFlg = false;
                        }
                        rt.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rt);
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

        public INV_PurchaseIndentDTO deactive(INV_PurchaseIndentDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_PurchaseIndentDMO.Single(t => t.INVTPI_Id == data.INVTPI_Id);

                if (result.INVTPI_ActiveFlg == true)
                {
                    result.INVTPI_ActiveFlg = false;
                }
                else if (result.INVTPI_ActiveFlg == false)
                {
                    result.INVTPI_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                int countactiveT = 0;
                int countactiveF = 0;
                var resultt = _INVContext.INV_T_PurchaseIndentDMO.Where(t => t.INVMPI_Id == data.INVMPI_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (rt.INVTPI_ActiveFlg == false)
                    {
                        countactiveF += 1;
                    }
                    else if (rt.INVTPI_ActiveFlg == true)
                    {
                        countactiveT += 1;
                    }
                }
                var resultmflg = _INVContext.INV_M_PurchaseIndentDMO.Single(t => t.INVMPI_Id == data.INVMPI_Id);
                if (countactiveF > 0 && countactiveT == 0)
                {
                    resultmflg.INVMPI_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }
                else if (countactiveT > 0 && countactiveF == 0)
                {
                    resultmflg.INVMPI_ActiveFlg = true;
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


        public INV_PurchaseIndentDTO rejectallreq(INV_PurchaseIndentDTO data)
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


        public string emaildata(long INVMPI_Id, long HRME_Id, long MI_Id, long UserID)
        {
            string suss = "";
            try
            {

                decimal total = 0;
                string str = "";
                var itemlist = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                from b in _INVContext.INV_T_PurchaseIndentDMO
                                from c in _INVContext.INV_Master_ItemDMO
                                from d in _INVContext.INV_Master_UOMDMO
                                where a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && b.INVTPI_ActiveFlg == true && a.INVMPI_Id == INVMPI_Id
                                select new INV_PurchaseIndentDTO
                                {
                                    INVMI_ItemName = c.INVMI_ItemName,
                                    INVMPI_Id = b.INVMPI_Id,
                                    INVTPI_Id = b.INVTPI_Id,
                                    INVTPI_PRQty = b.INVTPI_PRQty,
                                    INVTPI_PIQty = b.INVTPI_PIQty,
                                    INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                    INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                    INVTPI_Remarks = b.INVTPI_Remarks,
                                    INVMUOM_UOMName = d.INVMUOM_UOMName,
                                }
                              ).Distinct().ToList();

                if (itemlist.Count > 0)
                {
                    str += "<table width='100%' border=" + "'1'" + "    id=" + "'tablepaging'" + " class=" + "'table table-hover table-responsive table-bordered'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                   "<th style=" + "width:20px'" + "> SL.NO. </th>";
                    str += "<th>" + "ITEMNAME" + "</th>";
                    str += "<th>" + "UOM" + "</th>";
                    str += "<th>" + "PR QUANTITY" + "</th>";
                    str += "<th>" + "PI QUANTITY" + "</th>";

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
                        str += "<td  >" + itemlist[j].INVTPI_ApproxAmount + "</th>";
                        str += "<td  >" + itemlist[j].INVTPI_Remarks + "</th>";
                        str += "</tr>";
                    }

                    str += "</table>";


                    var tomailid = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == HRME_Id && a.HRMEM_DeFaultFlag == "default").ToList();

                    if (tomailid.Count > 0)
                    {
                        suss = sendmail(MI_Id, UserID, "PITEMPLATE", tomailid.FirstOrDefault().HRMEM_EmailId, str, INVMPI_Id);
                    }

                    var approvallist = (from a in _db.HR_PROCESSDMO
                                        from b in _db.HR_Process_Auth_OrderNoDMO
                                        where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PI" && b.HRPAON_SanctionLevelNo == 1
                                        select new INV_PurchaseIndentDTO
                                        {
                                            UserId = b.IVRMUL_Id
                                        }).Distinct().ToList();


                    if (approvallist.Count > 0)
                    {
                        long apprhrmeid = 0;
                        var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == approvallist.FirstOrDefault().UserId).ToList();
                        if (hrmeid1.Count > 0)
                        {
                            apprhrmeid = hrmeid1.FirstOrDefault().Emp_Code;
                        }

                        var tomailidapp = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == apprhrmeid && a.HRMEM_DeFaultFlag == "default").ToList();
                        if (tomailidapp.Count > 0)
                        {
                            if (tomailidapp.FirstOrDefault().HRMEM_EmailId != null && tomailidapp.FirstOrDefault().HRMEM_EmailId != "")
                            {
                                suss = sendmail(MI_Id, UserID, "PITEMPLATEAPPROVAL", tomailidapp.FirstOrDefault().HRMEM_EmailId, str, INVMPI_Id);
                            }
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
        public string sendmail(long MI_Id, long UserID, string Template, string emailid, string items, long INVMPI_Id)
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
