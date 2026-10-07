using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class ISM_Master_SciencePark_VendorCostImp: ISM_Master_SciencePark_VendorCostInterface
    {
        private readonly VMSContext _context;
        private readonly DomainModelMsSqlServerContext _db;

        public ISM_Master_SciencePark_VendorCostImp(VMSContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }

        public ISM_Master_SciencePark_VendorCostDto OnLoadData(ISM_Master_SciencePark_VendorCostDto dto)
        {

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ISM_Master_SciencePark_VendorCost_Details";

                cmd.CommandType = CommandType.StoredProcedure;

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
                    dto.OnLoadVendorCostList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "INV_Supplier_Details";
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = dto.MI_ID
                });
                cmd.CommandType = CommandType.StoredProcedure;

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
                    dto.OnLoadSupplierList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }


            return dto;
        }

        public ISM_Master_SciencePark_VendorCostDto Save(ISM_Master_SciencePark_VendorCostDto dto)
        {
            try
            {

                if (dto.INVMS_ID != null && dto.INVMS_ID != 0)
                {
                    foreach (var d in dto.VendorCostSave)
                    {
                        var duplicate = _context.ISM_Master_SciencePark_VendorCostDMO.Where(R => R.IMSV_PackageName == d.IMSV_PackageName && R.IMSV_Quantity == d.IMSV_Quantity && R.IMSV_Price == d.IMSV_Price && R.IMSV_Percentage == d.IMSV_Percentage && R.IMSV_TotalCostPerQty==d.IMSV_TotalCostPerQty && R.INVMS_ID==dto.INVMS_ID && R.IMSV_PackageFlag==d.IMSV_PackageFlag).ToList();
                        if (duplicate.Count > 0)
                        {
                            dto.returndata = "Duplicate";
                        }
                        else
                        {
                            ISM_Master_SciencePark_VendorCostDMO obj = new ISM_Master_SciencePark_VendorCostDMO();
                            obj.IMSV_PackageName = d.IMSV_PackageName;
                            obj.IMSV_Quantity = d.IMSV_Quantity;
                            obj.IMSV_Price = d.IMSV_Price;
                            obj.IMSV_Percentage = d.IMSV_Percentage;
                            obj.IMSV_TotalCost = d.IMSV_TotalCost;
                            obj.IMSV_TotalCostPerQty = d.IMSV_TotalCostPerQty;
                            obj.IMSV_PackageFlag = d.IMSV_PackageFlag;
                            obj.INVMS_ID = dto.INVMS_ID;
                            obj.IMSV_ActiveFlag = true;
                            obj.IMSV_CreatedBy = dto.User_Id;
                            obj.IMSV_UpdatedBy = dto.User_Id;
                            obj.IMSV_CreatedDate = DateTime.Now;
                            obj.IMSV_UpdatedDate = DateTime.Now;
                            _context.Add(obj);
                        }

                    }
                    var vv = _context.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Notsave";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returndata = "admin";
            }
            return dto;
        }

        public ISM_Master_SciencePark_VendorCostDto Deactivate (ISM_Master_SciencePark_VendorCostDto dto)
        {
            try
            {

                if (dto.IMSV_ID != null && dto.IMSV_ID != 0)
                {

                    var data = _context.ISM_Master_SciencePark_VendorCostDMO.Single(a => a.IMSV_ID == dto.IMSV_ID);
                    data.IMSV_UpdatedDate = DateTime.Now;
                    data.IMSV_UpdatedBy = dto.User_Id;
                    if (data.IMSV_ActiveFlag == true)
                    {
                        data.IMSV_ActiveFlag = false;
                    }
                    else
                    {
                        data.IMSV_ActiveFlag = true;
                    }
                    _context.Update(data);
                    var vv = _context.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                    
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returndata = "admin";
            }
            return dto;
        }

    }
}
