using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory.Purchase;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_RequisitionNotificationIMPL : Interface.INV_RequisitionNotificationInterface
    {

        public InventoryContext _INVContext;
        ILogger<INV_RequisitionNotificationIMPL> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_RequisitionNotificationIMPL(InventoryContext InvContext, ILogger<INV_RequisitionNotificationIMPL> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_RequisitionNotificationDTO getloaddata(INV_RequisitionNotificationDTO data)
        {
            try
            {
                var hrmeid = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                data.get_mi_list = _db.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
                var get_mi_list = _db.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                if (hrmeid.Count > 0)
                {
                    data.HRME_Id = hrmeid.FirstOrDefault().Emp_Code;
                    //INV_PurchaseNotification
                    data.get_purchaserequisition = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                                    from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                                    where (a.INVMPR_Id == b.INVMPR_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id)
                                                    select new INV_PurchaseRequisitionDTO
                                                    {
                                                        INVMPR_Id = a.INVMPR_Id,
                                                        INVMPR_PRNo = a.INVMPR_PRNo,
                                                        INVMPR_PRDate = a.INVMPR_PRDate,
                                                        INVMPR_Remarks = a.INVMPR_Remarks,
                                                        INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                                        INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                                        INVMPR_ActiveFlg = a.INVMPR_ActiveFlg,
                                                        INVMPR_RejectFlg = a.INVMPR_RejectFlg
                                                    }).Distinct().OrderBy(i => i.INVMPR_Id).ToArray();
                    data.rolflag = true;
                }
                else
                {
                    data.get_purchaserequisition = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                                    from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                                    where (a.INVMPR_Id == b.INVMPR_Id && a.MI_Id == data.MI_Id)
                                                    select new INV_PurchaseRequisitionDTO
                                                    {
                                                        INVMPR_Id = a.INVMPR_Id,
                                                        INVMPR_PRNo = a.INVMPR_PRNo,
                                                        INVMPR_PRDate = a.INVMPR_PRDate,
                                                        INVMPR_Remarks = a.INVMPR_Remarks,
                                                        INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                                        INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                                        INVMPR_ActiveFlg = a.INVMPR_ActiveFlg,
                                                        INVMPR_RejectFlg = a.INVMPR_RejectFlg
                                                    }).Distinct().OrderBy(i => i.INVMPR_Id).ToArray();
                    data.rolflag = false;
                }





            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition load Page:" + ex.Message);
            }
            return data;
        }

        public INV_RequisitionNotificationDTO get_prdetails(INV_RequisitionNotificationDTO data)
        {
            try
            {


            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition Item details:" + ex.Message);
            }
            return data;
        }
        public INV_RequisitionNotificationDTO getitemDetail(INV_RequisitionNotificationDTO data)
        {
            try
            {


            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition Item details:" + ex.Message);
            }
            return data;
        }

        public INV_RequisitionNotificationDTO savedetails(INV_RequisitionNotificationDTO data)
        {
            try
            {
                //var hrmeid = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                //data.get_mi_list = _db.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
                //var get_mi_list = _db.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                //if (hrmeid.Count > 0)
                //{
                //    data.HRME_Id = hrmeid.FirstOrDefault().Emp_Code;
                //}
                string INVMPR_Id = "0";
                string INVMPO_Id = "0";
                if (data.arrayPR != null)
                {
                    foreach (var f in data.arrayPR)
                    {
                        INVMPR_Id = INVMPR_Id + "," + f.INVMPR_Id;
                    }
                }
                else
                {
                    INVMPR_Id = INVMPR_Id + "," + data.INVMPR_Id;
                }
                if (data.Flag == "Order_Remark" || data.Flag == "OrderAp_Remark")
                {
                    var orderlist = _INVContext.INV_T_PurchaseIndentDMO.Where(R => R.INVMPR_Id == data.INVMPR_Id).ToList();
                    if (orderlist.Count > 0)
                    {
                        var orerapproval = _INVContext.INV_T_PurchaseOrderDMO.Where(R => R.INVMPI_Id == orderlist.FirstOrDefault().INVMPI_Id).ToList();
                        if(orerapproval.Count > 0)
                        {
                            
                            INVMPO_Id = INVMPO_Id + "," + orerapproval.FirstOrDefault().INVMPO_Id;
                        }
                    }
                    else
                    {

                    }
                   
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_Purchaseitem_levelwise";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMPR_Id", SqlDbType.VarChar)
                        {
                            Value = INVMPO_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = data.Flag
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
                            data.get_Details = retObject.ToArray();

                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_Purchaseitem_levelwise";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMPR_Id", SqlDbType.VarChar)
                        {
                            Value = INVMPR_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = data.Flag
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
                            data.get_Details = retObject.ToArray();

                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
               
                data.logo = _db.Institute.Where(R => R.MI_Id == data.MI_Id).Select(P => P.MI_Logo).FirstOrDefault();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return data;
        }
        public INV_RequisitionNotificationDTO edit(INV_RequisitionNotificationDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition edit:" + ex.Message);
            }
            return data;
        }

        public INV_RequisitionNotificationDTO deactiveM(INV_RequisitionNotificationDTO data)
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

        public INV_RequisitionNotificationDTO deactive(INV_RequisitionNotificationDTO data)
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

        public INV_RequisitionNotificationDTO GetOnchangeData(INV_RequisitionNotificationDTO data)
        {
            try
            {
                //

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition load Page:" + ex.Message);
            }
            return data;
        }
    }
}
