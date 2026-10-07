using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class GRN_Return_Approval_Impl : Interface.GRN_Return_Approval_Interface
    {
        public InventoryContext _INVContext;
        public GRN_Return_Approval_Impl(InventoryContext INVContext)
        {
            _INVContext = INVContext;
        }
        public GRN_Return_Apply_DTO getloaddata(GRN_Return_Apply_DTO dto)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GRNReturn_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.UserId
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
                        dto.grn_m_return = retObject.ToArray();
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
            return dto;
        }

        public async Task<GRN_Return_Apply_DTO> getpidetails(GRN_Return_Apply_DTO dto)
        {
            try
            {

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GRNReturnItem_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMGRNRET_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.INVMGRNRET_Id
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
                        dto.grn_item_return = retObject.ToArray();
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
            return dto;
        }

        public GRN_Return_Apply_DTO savedetails(GRN_Return_Apply_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                INV_M_GRN_Return_Apply_DMO re = new INV_M_GRN_Return_Apply_DMO();
                re.MI_Id = data.MI_Id;
                re.INVMGRN_Id = data.INVMGRN_Id;
                re.INVMS_Id = data.INVMS_Id;
                re.INVMGRNRETAPP_GRNRETNo = data.INVMGRNRETAPP_GRNRETNo;
                re.INVMGRNRETAPP_ReturnDate = indianTime;
                re.INVMGRNRETAPP_TotalAmount = data.INVMGRNRETAPP_TotalAmount;
                re.INVMGRNRETAPP_Remarks = data.INVMGRNRETAPP_Remarks;
                re.INVMGRNRETAPP_StatusFlg = "APPROVED";
                re.INVMGRNRETAPP_ActiveFlg = true;
                re.INVMGRNRETAPP_CreatedBy = data.UserId;
                re.INVMGRNRETAPP_UpdatedBy = data.UserId;
                re.INVMGRNRETAPP_CreatedDate = DateTime.Today;
                re.INVMGRNRETAPP_UpdatedDate = DateTime.Today;
                _INVContext.Add(re);

                foreach (var item in data.arrayretgrn)
                {
                    INV_T_GRN_Return_Apply_DMO ra = new INV_T_GRN_Return_Apply_DMO();
                    ra.INVMGRNRETAPP_Id = re.INVMGRNRETAPP_Id;
                    ra.INVMI_Id = item.INVMI_Id;
                    ra.INVMUOM_Id = item.INVMUOM_Id;
                    ra.INVTGRNRETAPP_ReturnQty = item.INVTPIAPP_ApprovedQty;
                    ra.INVTGRNRETAPP_ReturnAmount = item.invtpI_ApproxAmount;
                    ra.INVTGRNRETAPP_ReturnNaration = item.INVTGRNRETAPP_ReturnNaration;
                    ra.INVTGRNRETAPP_ActiveFlg = true;
                    ra.INVTGRNRETAPP_CreatedBy = data.UserId;
                    ra.INVTGRNRETAPP_UpdatedBy = data.UserId;
                    ra.INVTGRNRETAPP_ApprovalFlg = item.flag;
                    ra.INVTGRNRETAPP_CreatedDate = DateTime.Today;
                    ra.INVTGRNRETAPP_UpdatedDate = DateTime.Today;
                    _INVContext.Add(ra);

                }
                var result = _INVContext.INV_M_GRN_Return_DMO_con.Single(a => a.INVMGRNRET_Id == data.INVMGRNRET_Id);
                result.INVMGRNRETAPP_Id = re.INVMGRNRETAPP_Id;
                _INVContext.Update(result);

                var cnt = _INVContext.SaveChanges();
                if (cnt > 0)
                {
                    var id = re.INVMGRNRETAPP_Id;
                    var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_GRNReturn_UpdationInStock @p0, @p1", id, data.MI_Id);
                    if (contactExistsP > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
    }
}
