using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.HRMS;
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

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class INV_DepreciationIMPL : Interface.INV_DepreciationInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_DepreciationIMPL> _logInv;
        public INV_DepreciationIMPL(InventoryContext InvContext, ILogger<INV_DepreciationIMPL> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }
        public INV_DepreciationDTO getloaddata(INV_DepreciationDTO data)
        {
            try
            {
                data.Institution = _INVContext.Institution.Where(P => P.MI_ActiveFlag == 1).Distinct().ToArray();
                data.IMFY_YEAR = _INVContext.IVRM_Master_FinancialYearDMO.Distinct().ToArray();
                data.GetReport = (from a in _INVContext.INV_DepreciationDMO
                                  from b in _INVContext.INV_Master_ItemDMO
                                  from c in _INVContext.INV_Master_StoreDMO
                                  from d in _INVContext.Institution
                                  from e in _INVContext.INV_OpeningBalanceDMO
                                  from f in _INVContext.IVRM_Master_FinancialYearDMO
                                  where (a.MI_Id == d.MI_Id && a.INVMST_Id == c.INVMST_Id &&
                                  a.INVMI_Id == b.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.IMFY_Id==f.IMFY_Id)
                                  select new INV_DepreciationDTO
                                  {
                                      MI_Name = d.MI_Name,
                                      INVMS_StoreName = c.INVMS_StoreName,
                                      INVMI_ItemName = b.INVMI_ItemName,
                                      INVOB_PurchaseRate = e.INVOB_PurchaseRate,
                                      INVDEP_DepreciationPer = a.INVDEP_DepreciationPer,
                                      INVDEP_OpeningValue = a.INVDEP_OpeningValue,
                                      INVDEP_OpeningValueAsOnDate = a.INVDEP_OpeningValueAsOnDate,
                                      INVDEP_ClosingValue = a.INVDEP_ClosingValue,
                                      INVDEP_ClosingValueAsOnDate = a.INVDEP_ClosingValueAsOnDate,
                                      INVDEP_ReValuationValue = a.INVDEP_ReValuationValue,
                                      INVDEP_Id = a.INVDEP_Id,
                                      INVDEP_ActiveFlg = a.INVDEP_ActiveFlg,
                                      MI_Id = a.MI_Id,
                                      INVMST_Id = a.INVMST_Id,
                                      IMFY_Id = a.IMFY_Id,
                                      INVMI_Id = a.INVMI_Id,
                                      INVMI_ItemMoreThan180 = a.INVMI_ItemMoreThan180,
                                      INVMI_ItemLessThan180 = a.INVMI_ItemLessThan180,
                                      INVMI_DeletedItemAmount = a.INVMI_DeletedItemAmount


                                  }
                                ).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //storechanges
        public INV_DepreciationDTO storechanges(INV_DepreciationDTO data)
        {
            try
            {
                List<long> sec = new List<long>();
                if (data.companyNames != null)
                {
                    foreach (var i in data.companyNames)
                    {
                        sec.Add(i.MI_Id);
                    }
                }
                data.get_Store = (from a in _INVContext.INV_Master_StoreDMO
                                  from b in _INVContext.Institution
                                  where (a.MI_Id == b.MI_Id && a.INVMS_ActiveFlg == true && sec.Contains(a.MI_Id))
                                  select new INV_DepreciationDTO
                                  {
                                      MI_Name = b.MI_Name,
                                      INVMST_Id = a.INVMST_Id,
                                      INVMS_StoreName = a.INVMS_StoreName
                                  }
                             ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public INV_DepreciationDTO savedetails(INV_DepreciationDTO data)
        {
            try
            {
                if (data.INVDEP_Id > 0)
                {
                    var resultone = _INVContext.INV_DepreciationDMO.Single(t => t.INVDEP_Id == data.INVDEP_Id);
                    if (resultone.INVDEP_Id > 0)
                    {
                        for (int c = 0; c < data.INV_Depreciation.Length; c++)
                        {
                            resultone.INVDEP_OpeningValue = data.INV_Depreciation[c].INVDEP_OpeningValue;
                            resultone.INVDEP_OpeningValueAsOnDate = data.INV_Depreciation[c].INVDEP_OpeningValueAsOnDate;
                            resultone.INVDEP_DepreciationPer = data.INV_Depreciation[c].INVDEP_DepreciationPer;
                            resultone.INVDEP_ClosingValue = data.INV_Depreciation[c].INVDEP_ClosingValue;
                            resultone.INVDEP_ClosingValueAsOnDate = data.INV_Depreciation[c].INVDEP_ClosingValueAsOnDate;
                            resultone.INVDEP_ReValuationValue = data.INV_Depreciation[c].INVDEP_ReValuationValue;
                            resultone.INVDEP_UpdatedDate = DateTime.Now;
                            resultone.INVDEP_UpdatedBy = data.User_Id;
                            resultone.INVMI_ItemMoreThan180 = data.INV_Depreciation[c].INVMI_ItemMoreThan180;
                            resultone.INVMI_ItemLessThan180 = data.INV_Depreciation[c].INVMI_ItemLessThan180;
                            resultone.INVMI_DeletedItemAmount = data.INV_Depreciation[c].INVMI_DeletedItemAmount;
                            _INVContext.Update(resultone);
                        }
                        int a = _INVContext.SaveChanges();
                        if (a > 0)
                        {
                            data.Returnstatus = "Update";

                        }
                        else
                        {
                            data.Returnstatus = "Notupdate";
                        }
                    }
                }
                else
                {
                    for (int c = 0; c < data.INV_Depreciation.Length; c++)
                    {
                        var list = _INVContext.INV_DepreciationDMO.Where(R => R.MI_Id == data.INV_Depreciation[c].MI_Id && R.INVMI_Id == data.INV_Depreciation[c].INVMI_Id && R.IMFY_Id == data.IMFY_Id && R.INVMST_Id == data.INV_Depreciation[c].INVMST_Id).ToList();
                        if (list.Count > 0)
                        {
                            //
                            if (data.message != "")
                            {
                                data.message = data.INV_Depreciation[c].INVMI_ItemName;
                            }
                            else
                            {
                                data.message = data.message + ", " + data.INV_Depreciation[c].INVMI_ItemName;
                            }
                        }
                        else
                        {
                            INV_DepreciationDMO obj = new INV_DepreciationDMO();
                            obj.MI_Id = data.INV_Depreciation[c].MI_Id;
                            obj.INVMST_Id = data.INV_Depreciation[c].INVMST_Id;
                            obj.INVMI_Id = data.INV_Depreciation[c].INVMI_Id;
                            obj.IMFY_Id = data.IMFY_Id;
                            obj.INVDEP_OpeningValue = data.INV_Depreciation[c].INVDEP_OpeningValue;
                            obj.INVDEP_OpeningValueAsOnDate = data.INV_Depreciation[c].INVDEP_OpeningValueAsOnDate;
                            obj.INVDEP_DepreciationPer = data.INV_Depreciation[c].INVDEP_DepreciationPer;
                            obj.INVDEP_ClosingValue = data.INV_Depreciation[c].INVDEP_ClosingValue;
                            obj.INVDEP_ClosingValueAsOnDate = data.INV_Depreciation[c].INVDEP_ClosingValueAsOnDate;
                            obj.INVDEP_ReValuationValue = data.INV_Depreciation[c].INVDEP_ReValuationValue;
                            obj.INVMI_ItemMoreThan180 = data.INV_Depreciation[c].INVMI_ItemMoreThan180;
                            obj.INVMI_ItemLessThan180 = data.INV_Depreciation[c].INVMI_ItemLessThan180;
                            obj.INVMI_DeletedItemAmount = data.INV_Depreciation[c].INVMI_DeletedItemAmount;
                            obj.INVDEP_ActiveFlg = true;
                            obj.INVDEP_CreatedDate = DateTime.Now;
                            obj.INVDEP_UpdatedDate = DateTime.Now;
                            obj.INVDEP_CreatedBy = data.User_Id;
                            obj.INVDEP_UpdatedBy = data.User_Id;
                            _INVContext.Add(obj);
                        }
                    }
                    int a = _INVContext.SaveChanges();
                    if (a > 0)
                    {
                        data.Returnstatus = "Insert";

                    }
                    else
                    {
                        data.Returnstatus = "Failed";
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //Itemchanges
        public INV_DepreciationDTO Itemchanges(INV_DepreciationDTO data)
        {
            try
            {
                string sec_Id = "0";
                foreach (var c in data.companyNames)
                {
                    sec_Id = sec_Id + "," + c.MI_Id.ToString();
                }
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Depreciation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.VarChar)
                    {
                        Value = sec_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
            SqlDbType.VarChar)
                    {
                        Value = data.INVMSTSS_Id
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
                        data.getdata = retObject.ToArray();
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
        //deactive
        public INV_DepreciationDTO deactive(INV_DepreciationDTO data)
        {
            try
            {
                var resultone = _INVContext.INV_DepreciationDMO.Single(t => t.INVDEP_Id == data.INVDEP_Id);

                if (resultone.INVDEP_ActiveFlg == true)
                {
                    resultone.INVDEP_ActiveFlg = false;
                }
                else
                {
                    resultone.INVDEP_ActiveFlg = true;
                }
                resultone.INVDEP_UpdatedDate = DateTime.Now;
                resultone.INVDEP_UpdatedBy = data.User_Id;
                _INVContext.Update(resultone);
                int a = _INVContext.SaveChanges();
                if (a > 0)
                {
                    data.Returnstatus = "Delete";

                }
                else
                {
                    data.Returnstatus = "NotDelete";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // public INV_DepreciationDTO savedetails(INV_DepreciationDTO data)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
    }
}
