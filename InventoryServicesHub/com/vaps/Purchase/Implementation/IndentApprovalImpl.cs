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
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using DomainModel.Model;
using System.Text.RegularExpressions;
using System.IO;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class IndentApprovalImpl : Interface.IndentApprovalInterface
    {
        public InventoryContext _INVContext;
        ILogger<IndentApprovalImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public IndentApprovalImpl(InventoryContext InvContext, ILogger<IndentApprovalImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }
        public IndentApprovalDTO getloaddata(IndentApprovalDTO data)
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
                    cmd.CommandText = "INV_PurchageIndent_Approval";
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
        public async Task<IndentApprovalDTO> getpidetails(IndentApprovalDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PurchageIndentItemsForApproval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMPI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.INVMPI_Id
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

                data.viewcomments = (from a in _INVContext.INV_M_PurchaseIndent_ApprovalDMO
                                     from b in _INVContext.ApplicationUser
                                     where (a.INVMPIAPP_CreatedBy == b.Id && a.INVMPI_Id == data.INVMPI_Id && a.INVMPIAPP_ActiveFlg == true)
                                     select new IndentApprovalDTO
                                     {
                                         UserName = b.UserName,
                                         INVMPIAPP_Remarks = a.INVMPIAPP_Remarks,
                                         INVMPIAPP_PIDate = a.INVMPIAPP_PIDate,
                                     }).Distinct().OrderBy(a => a.INVMPIAPP_PIDate).ToArray();


                var purchaseindentcomments = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                              from b in _INVContext.ApplicationUser
                                              from c in _INVContext.Staff_User_Login
                                              from e in _INVContext.MasterEmployee
                                              where (a.INVMPI_CreatedBy == b.Id && b.Id == c.Id && c.Emp_Code == e.HRME_Id && a.INVMPI_Id == data.INVMPI_Id)
                                              select new IndentApprovalDTO
                                              {
                                                  UserName = b.UserName,
                                                  employeename = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : e.HRME_EmployeeFirstName) +
                                             (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " +
                                             e.HRME_EmployeeMiddleName) +
                                             (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " +
                                             e.HRME_EmployeeLastName)).Trim(),
                                                  INVMPI_Remarks = a.INVMPI_Remarks,
                                                  INVMPI_PIDate = a.INVMPI_PIDate,
                                                  INVMPI_PINo = a.INVMPI_PINo,
                                              }).Distinct().OrderBy(a => a.INVMPI_PIDate).ToArray();

                data.purchaseindentcomments = purchaseindentcomments;


                var purchaserequisitioncomments = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                                   from b in _INVContext.MasterEmployee
                                                   from c in _INVContext.INV_T_PurchaseIndentDMO
                                                   from d in _INVContext.INV_M_PurchaseIndentDMO
                                                   where (a.HRME_Id == b.HRME_Id && a.INVMPR_Id == c.INVMPR_Id && c.INVMPI_Id == d.INVMPI_Id && d.INVMPI_Id == data.INVMPI_Id)
                                                   select new IndentApprovalDTO
                                                   {
                                                       employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName) +
                                             (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " +
                                             b.HRME_EmployeeMiddleName) +
                                             (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " +
                                             b.HRME_EmployeeLastName)).Trim(),

                                                       INVMPR_PRNo = a.INVMPR_PRNo,
                                                       INVMPR_Remarks = a.INVMPR_Remarks,
                                                       INVMPR_PRDate = a.INVMPR_PRDate
                                                   }).Distinct().ToArray();
                data.purchaserequisitioncomments = purchaserequisitioncomments;

                try
                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Purchase_ClientName";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@INVMPI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.INVMPI_Id
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
                            data.get_clientname = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent Item details:" + ex.Message);
            }
            return data;
        }
        public IndentApprovalDTO getprDetail(IndentApprovalDTO data)
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
        public IndentApprovalDTO get_details(IndentApprovalDTO data)
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
        public IndentApprovalDTO savedetails(IndentApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var indlist = _INVContext.INV_M_PurchaseIndentDMO.Where(w => w.INVMPI_Id == data.INVMPI_Id).ToList();

                if (indlist.Count > 0)
                {
                    INV_M_PurchaseIndent_ApprovalDMO obj = new INV_M_PurchaseIndent_ApprovalDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.INVMPI_Id = data.INVMPI_Id;
                    obj.INVMPIAPP_PINo = indlist.FirstOrDefault().INVMPI_PINo;
                    obj.INVMPIAPP_PIDate = indianTime;
                    obj.INVMPIAPP_Remarks = data.INVMPI_Remarks;
                    obj.INVMPIAPP_ReferenceNo = indlist.FirstOrDefault().INVMPI_ReferenceNo;
                    obj.INVMPIAPP_ApproxTotAmount = data.INVMPI_ApproxTotAmount;

                    if (data.approvecnt > 0)
                    {
                        obj.INVMPIAPP_RejectFlg = false;
                    }
                    else
                    {
                        obj.INVMPIAPP_RejectFlg = true;
                    }

                    obj.INVMPIAPP_ApprovedBy = data.UserId;
                    obj.INVMPIAPP_ActiveFlg = true;
                    obj.INVMPIAPP_CreatedBy = data.UserId;
                    obj.INVMPIAPP_UpdatedBy = data.UserId;
                    obj.INVMPIAPP_CreatedDate = indianTime;
                    obj.INVMPIAPP_UpdatedDate = indianTime;
                    _INVContext.Add(obj);


                    if (data.arrayPI != null)
                    {
                        foreach (var item in data.arrayPI)
                        {
                            INV_T_PurchaseIndent_ApprovalDMO tobj = new INV_T_PurchaseIndent_ApprovalDMO();
                            tobj.MI_Id = data.MI_Id;
                            tobj.INVMPIAPP_Id = obj.INVMPIAPP_Id;
                            tobj.INVMPR_Id = item.INVMPR_Id;
                            tobj.INVMI_Id = item.INVMI_Id;
                            tobj.INVTPI_Id = item.INVTPI_Id;
                            tobj.INVMUOM_Id = item.INVMUOM_Id;
                            tobj.INVTPIAPP_PRQty = item.INVTPI_PRQty;
                            tobj.INVTPIAPP_PIQty = item.INVTPI_PIQty;
                            tobj.INVTPIAPP_PIUnitRate = item.INVTPI_PIUnitRate;
                            tobj.INVTPIAPP_ApproxAmount = item.INVTPI_ApproxAmount;

                            if (item.flag == "R")
                            {
                                tobj.INVTPIAPP_RejectFlg = true;
                            }
                            else
                            {
                                tobj.INVTPIAPP_RejectFlg = false;
                            }

                            tobj.INVTPIAPP_ApprovedQty = item.INVTPIAPP_ApprovedQty;
                            tobj.INVTPIAPP_Remarks = item.INVTPI_Remarks;
                            tobj.INVTPIAPP_ActiveFlg = true;
                            tobj.INVTPIAPP_CreatedDate = indianTime;
                            tobj.INVTPIAPP_UpdatedDate = indianTime;
                            tobj.INVTPIAPP_UpdatedBy = data.UserId;
                            tobj.INVTPIAPP_CreatedBy = data.UserId;
                            _INVContext.Add(tobj);
                        }
                    }


                    int ext = _INVContext.SaveChanges();
                    if (ext > 0)
                    {
                        var checkmaxorder = (from a in _INVContext.HR_PROCESSDMO
                                             from b in _INVContext.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PI"
                                             select b.HRPAON_SanctionLevelNo).Max();

                        var checkuserorder = (from a in _INVContext.HR_PROCESSDMO
                                              from b in _INVContext.HR_Process_Auth_OrderNoDMO
                                              where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PI" && b.IVRMUL_Id == data.UserId
                                              select b.HRPAON_SanctionLevelNo).Max();


                        if (checkmaxorder == checkuserorder)
                        {
                            var indentres = _INVContext.INV_M_PurchaseIndentDMO.Single(a => a.INVMPI_Id == data.INVMPI_Id);


                            indentres.INVMPI_UpdatedBy = data.UserId;
                            indentres.UpdatedDate = indianTime;
                            indentres.INVMPI_FinalProcessFlag = true;
                            if (data.approvecnt > 0)
                            {
                                indentres.INVMPI_RejectFlg = false;
                                indentres.INVMPI_ApproxTotAmount = data.INVMPI_ApproxTotAmount;
                            }
                            else
                            {
                                indentres.INVMPI_RejectFlg = true;
                                indentres.INVMPI_ApproxTotAmount = 0;
                            }
                            _INVContext.Update(indentres);
                            var indentresitem = _INVContext.INV_T_PurchaseIndentDMO.Where(a => a.INVMPI_Id == data.INVMPI_Id).ToList();
                            if (indentresitem.Count > 0)
                            {

                                foreach (var item in indentresitem)
                                {

                                    foreach (var ap in data.arrayPI)
                                    {
                                        if (ap.INVTPI_Id == item.INVTPI_Id)
                                        {

                                            item.INVTPI_PIUnitRate = ap.INVTPI_PIUnitRate;
                                            if (ap.flag == "R")
                                            {
                                                item.INVTPI_ApproxAmount = 0;
                                                item.INVTPI_ApproveQty = 0;
                                                item.INVTPI_RejectFlg = true;
                                            }
                                            else
                                            {
                                                item.INVTPI_ApproxAmount = ap.INVTPI_ApproxAmount;
                                                item.INVTPI_ApproveQty = ap.INVTPIAPP_ApprovedQty;
                                                item.INVTPI_RejectFlg = false;
                                            }

                                            item.INVTPI_UpdatedBy = data.UserId;
                                            item.UpdatedDate = indianTime;

                                            _INVContext.Update(item);
                                        }
                                    }
                                }
                            }
                            _INVContext.SaveChanges();
                        }

                        data.returnval = true;

                        var primaryid = _INVContext.INV_M_PurchaseIndent_ApprovalDMO.Where(e => e.INVMPIAPP_Id == obj.INVMPIAPP_Id).ToList();
                        if (primaryid.Count > 0)
                        {
                            long hrid = 0;

                            var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                            if (hrmeid1.Count > 0)
                            {
                                hrid = hrmeid1.FirstOrDefault().Emp_Code;
                            }

                            emaildata(primaryid.FirstOrDefault().INVMPIAPP_Id, hrid, data.MI_Id, data.UserId);
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
        public IndentApprovalDTO edit(IndentApprovalDTO data)
        {
            try
            {
                data.get_editPI = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                   from b in _INVContext.INV_T_PurchaseIndentDMO
                                   from c in _INVContext.INV_Master_ItemDMO
                                   from d in _INVContext.INV_Master_UOMDMO
                                   where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                            && a.INVMPI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVTPI_Id == data.INVTPI_Id)
                                   select new IndentApprovalDTO
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
        public IndentApprovalDTO deactiveM(IndentApprovalDTO data)
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
        public IndentApprovalDTO deactive(IndentApprovalDTO data)
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
        public IndentApprovalDTO rejectallreq(IndentApprovalDTO data)
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
        public IndentApprovalDTO OnClickIndividualViewComments(IndentApprovalDTO data)
        {
            try
            {
                data.indiviewcomments = (from a in _INVContext.INV_T_PurchaseIndent_ApprovalDMO
                                         from b in _INVContext.ApplicationUser
                                         where (a.INVTPIAPP_CreatedBy == b.Id && a.INVTPI_Id == data.INVTPI_Id)
                                         select new IndentApprovalDTO
                                         {
                                             INVMPIAPP_Remarks = a.INVTPIAPP_Remarks,
                                             INVMPIAPP_PIDate = a.INVTPIAPP_CreatedDate,
                                             UserName = b.UserName
                                         }).Distinct().OrderBy(a => a.INVMPIAPP_PIDate).ToArray();


                var purchaseindentindicomments = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                                  from b in _INVContext.ApplicationUser
                                                  from c in _INVContext.Staff_User_Login
                                                  from e in _INVContext.MasterEmployee
                                                  from d in _INVContext.INV_T_PurchaseIndentDMO
                                                  where (a.INVMPI_CreatedBy == b.Id && b.Id == c.Id && c.Emp_Code == e.HRME_Id && a.INVMPI_Id == d.INVMPI_Id
                                                  && a.INVMPI_Id == data.INVMPI_Id && d.INVTPI_Id == data.INVTPI_Id)
                                                  select new IndentApprovalDTO
                                                  {
                                                      UserName = b.UserName,
                                                      employeename = ((e.HRME_EmployeeFirstName == null
                                                      || e.HRME_EmployeeFirstName == "" ? "" : e.HRME_EmployeeFirstName) +
                                                 (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == ""
                                                 || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) +
                                                 (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == ""
                                                 || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),
                                                      INVMPI_Remarks = d.INVTPI_Remarks,
                                                      INVMPI_PIDate = a.INVMPI_PIDate,
                                                      INVMPI_PINo = a.INVMPI_PINo,
                                                  }).Distinct().OrderBy(a => a.INVMPI_PIDate).ToArray();

                data.purchaseindentindicomments = purchaseindentindicomments;


                var purchaserequisitionindicomments = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                                       from b in _INVContext.MasterEmployee
                                                       from c in _INVContext.INV_T_PurchaseIndentDMO
                                                       from d in _INVContext.INV_M_PurchaseIndentDMO
                                                       from e in _INVContext.INV_T_PurchaseRequisitionDMO
                                                       where (a.HRME_Id == b.HRME_Id && a.INVMPR_Id == c.INVMPR_Id && c.INVMPI_Id == d.INVMPI_Id
                                                       && a.INVMPR_Id == e.INVMPR_Id && e.INVMI_Id==data.INVMI_Id
                                                       && d.INVMPI_Id == data.INVMPI_Id && c.INVTPI_Id == data.INVTPI_Id)
                                                       select new IndentApprovalDTO
                                                       {
                                                           employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" :
                                                           b.HRME_EmployeeFirstName) +
                                                 (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " "
                                                 + b.HRME_EmployeeMiddleName) +
                                                 (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " +
                                                 b.HRME_EmployeeLastName)).Trim(),

                                                           INVMPR_PRNo = a.INVMPR_PRNo,
                                                           INVMPR_Remarks = e.INVTPR_Remarks,
                                                           INVMPR_PRDate = a.INVMPR_PRDate
                                                       }).Distinct().ToArray();

                data.purchaserequisitionindicomments = purchaserequisitionindicomments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public string emaildata(long INVMPIAPP_Id, long HRME_Id, long MI_Id, long UserID)
        {
            string suss = "";
            try
            {

                decimal total = 0;
                string str = "";
                var itemlist = (from a in _INVContext.INV_M_PurchaseIndent_ApprovalDMO
                                from b in _INVContext.INV_T_PurchaseIndent_ApprovalDMO
                                from c in _INVContext.INV_Master_ItemDMO
                                from d in _INVContext.INV_Master_UOMDMO
                                where a.INVMPIAPP_Id == b.INVMPIAPP_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && b.INVTPIAPP_ActiveFlg == true && a.INVMPIAPP_Id == INVMPIAPP_Id && b.INVTPIAPP_RejectFlg == false
                                select new IndentApprovalDTO
                                {
                                    INVMI_ItemName = c.INVMI_ItemName,
                                    INVMPIAPP_Id = b.INVMPIAPP_Id,
                                    INVTPIAPP_Id = b.INVTPIAPP_Id,
                                    INVTPI_PRQty = b.INVTPIAPP_PIQty,
                                    INVTPI_PIQty = b.INVTPIAPP_PIQty,
                                    INVTPI_PIUnitRate = b.INVTPIAPP_PIUnitRate,
                                    INVTPI_ApproxAmount = b.INVTPIAPP_ApproxAmount,
                                    INVMPI_Remarks = b.INVTPIAPP_Remarks,
                                    INVMUOM_UOMName = d.INVMUOM_UOMName,
                                }
                              ).Distinct().ToList();

                if (itemlist.Count > 0)
                {
                    str += "<table width='100%' border=" + "'1'" + "    id=" + "'tablepaging'" + " class=" + "'table table-hover table-responsive table-bordered'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                   "<th style=" + "width:20px'" + "> SL.NO. </th>";
                    str += "<th>" + "ITEM NAME" + "</th>";
                    str += "<th>" + "UOM" + "</th>";
                    str += "<th>" + "PI QUANTITY" + "</th>";
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
                                        where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PI" && b.IVRMUL_Id == UserID
                                        select new IndentApprovalDTO
                                        {
                                            UserId = b.IVRMUL_Id,
                                            HRPAON_SanctionLevelNo = b.HRPAON_SanctionLevelNo
                                        }).Distinct().ToList();


                    if (approvallist.Count > 0)
                    {

                        var nextleveluser = (from a in _db.HR_PROCESSDMO
                                             from b in _db.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "PI" && b.HRPAON_SanctionLevelNo == approvallist.FirstOrDefault().HRPAON_SanctionLevelNo + 1
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
                                    suss = sendmail(MI_Id, UserID, "STARTPIAPPROVAL", tomailidapp.FirstOrDefault().HRMEM_EmailId, str, INVMPIAPP_Id, ccmail);
                                }
                            }
                        }
                        else
                        {
                            suss = sendmail(MI_Id, UserID, "ENDPIAPPROVAL", ccmail, str, INVMPIAPP_Id, "");
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