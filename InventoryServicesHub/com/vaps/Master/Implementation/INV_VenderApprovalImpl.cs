using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using InventoryServicesHub.com.vaps.Master.Interface;
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

namespace InventoryServicesHub.com.vaps.Master.Implementation
{
    public class INV_VenderApprovalImpl : INV_VenderApprovalInterface
    {
        private readonly InventoryContext _INVContext;
        private readonly ILogger<INV_VenderApprovalImpl> _logInv;

        public INV_VenderApprovalImpl(InventoryContext InvContext, ILogger<INV_VenderApprovalImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_MasterVendorEntryDTO> getloaddataOne(INV_MasterVendorEntryDTO data)
        {
            try
            {

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vender_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // ONLY parameter required by SP
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;

                            for (var iField = 0; iField < dataReader.FieldCount; iField++)
                            {
                                if (!dataRow.ContainsKey(dataReader.GetName(iField))) dataRow.Add(
                                    dataReader.GetName(iField),
                                    dataReader.IsDBNull(iField) ? null : dataReader[iField]
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }

                    data.INV_MasterVendorApprovalArray = retObject.ToArray();
                }

            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.saveorupdate = ex.Message;
            }
            return data;
        }


        public async Task<INV_MasterVendorEntryDTO> getloaddataTwo(INV_MasterVendorEntryDTO data)
        {
            try
            {

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "fetch_Vendor_UpdateApproval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // ONLY parameter required by SP
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;

                            for (var iField = 0; iField < dataReader.FieldCount; iField++)
                            {
                                if (!dataRow.ContainsKey(dataReader.GetName(iField))) dataRow.Add(
                                    dataReader.GetName(iField),
                                    dataReader.IsDBNull(iField) ? null : dataReader[iField]
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }

                    data.approved_vendor_list = retObject.ToArray();
                }

            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.saveorupdate = ex.Message;
            }
            return data;
        }


        public async Task<INV_MasterVendorEntryDTO> UpdateSP(INV_MasterVendorEntryDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vendor_UpdateApproval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.VarChar) { Value = data.Action });
                    cmd.Parameters.Add(new SqlParameter("@Userid", SqlDbType.BigInt) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@MVE_Id", SqlDbType.BigInt) { Value = data.MVE_Id });
                    cmd.Parameters.Add(new SqlParameter("@VAS_Id", SqlDbType.BigInt) { Value = data.VAS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;

                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                if (!dataRow.ContainsKey(dataReader.GetName(i)))
                                {
                                    dataRow.Add(
                                        dataReader.GetName(i),
                                        dataReader.IsDBNull(i) ? null : dataReader[i]
                                    );
                                }
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }

                    data.approved_vendor_list = retObject.ToArray();
                    data.returnval = true;
                    data.saveorupdate = "Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.saveorupdate = ex.Message;
            }

            return data;
        }
    }
}