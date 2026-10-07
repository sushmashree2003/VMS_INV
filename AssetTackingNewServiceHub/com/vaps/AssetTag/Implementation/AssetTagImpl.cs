using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking.AssetTag;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Implementation
{
    public class AssetTagImpl : Interface.AssetTagInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagImpl> _logAT;
        public AssetTagImpl(AssetTrackingContext ATContext, ILogger<AssetTagImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }
        #region QRCODE
        public AssetTagDTO create_inv_qrcode(AssetTagDTO data)
        {
            try
            {

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Save_Inv_QRCode";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IQ_Created_By", SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@IQ_Updated_By", SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMSI_Id", SqlDbType.BigInt)
                    {
                        Value = data.INVMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMA_QRCODEPath", SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    try
                    {
                        object result = cmd.ExecuteScalar();
                        long pk = Convert.ToInt64(result);
                        string valueqr = "";
                        valueqr = "INV" + "#" + Convert.ToInt32(data.INVMST_Id).ToString();
                        if (pk > 0)
                        {
                            QRCodeGenerator QrGenerator = new QRCodeGenerator();
                            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                            QRCode QrCode = new QRCode(QrCodeInfo);
                            Bitmap QrBitmap = QrCode.GetGraphic(60);

                            byte[] byteArray = BitmapToByteArray(QrBitmap);
                            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));

                            using (var cmdd = _ATContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmdd.CommandText = "Update_Inv_QRCode";
                                cmdd.CommandType = CommandType.StoredProcedure;
                                cmdd.Parameters.Add(new SqlParameter("@IQ_Id", SqlDbType.BigInt)
                                {
                                    Value = pk
                                });
                                cmdd.Parameters.Add(new SqlParameter("@HRMA_QRCODEPath", SqlDbType.VarChar)
                                {
                                    Value = QrUri
                                });
                                if (cmdd.Connection.State != ConnectionState.Open)
                                    cmdd.Connection.Open();
                                try
                                {
                                    int rowsAffected = cmdd.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                        data.returnval = false;

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "exist";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        data.returnval = false;
                    }
                }

            }
            catch (Exception ex)
            {

                _logAT.LogInformation("Asset tag savedata :" + ex.Message);
            }
            return data;
        }
        public AssetTagDTO Save_INV_qrremarks(AssetTagDTO data)
        {
            try
            {
                foreach (var a in data.tagckdArray)
                {
                    using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Save_INV_QRCode_FeedBack";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Created_By", SqlDbType.BigInt)
                        {
                            Value = data.userId
                        }); cmd.Parameters.Add(new SqlParameter("@Updated_By", SqlDbType.BigInt)
                        {
                            Value = data.userId
                        }); cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        }); cmd.Parameters.Add(new SqlParameter("@INVMG_Id", SqlDbType.BigInt)
                        {
                            Value = a.INVMI_Id
                        }); cmd.Parameters.Add(new SqlParameter("@IQ_Id", SqlDbType.BigInt)
                        {
                            Value = a.INVAAT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMSI_Id", SqlDbType.BigInt)
                        {
                            Value = a.INVMST_Id
                        }); cmd.Parameters.Add(new SqlParameter("@Active_Flag", SqlDbType.Bit)
                        {
                            Value = true
                        });
                        cmd.Parameters.Add(new SqlParameter("@QR_Feedback", SqlDbType.VarChar)
                        {
                            Value = a.INVAAT_AssetDescription
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();



                        try
                        {
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                data.message = "Success";
                            }
                            else
                                data.message = "fail";

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

                _logAT.LogInformation("Asset tag savedata :" + ex.Message);
            }
            return data;
        }
        public AssetTagDTO LoadSite(AssetTagDTO data)
        {
            try
            {

                data.get_store = (from a in _ATContext.INV_Master_SiteDMO where a.MI_Id == data.MI_Id select a).ToArray();
                data.get_edit_data = (from a in _ATContext.INV_Master_SiteDMO where a.MI_Id == data.MI_Id select a).ToArray();
                using (var cmdd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdd.CommandText = "Inv_QRCode_List";
                    cmdd.CommandType = CommandType.StoredProcedure;
                    cmdd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmdd.Connection.State != ConnectionState.Open)
                        cmdd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmdd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_items = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                _logAT.LogInformation("Sile List :" + ex.Message);
            }
            return data;
        }
        public AssetTagDTO LoadQRdata(AssetTagDTO data)
        {
            try
            {


                using (var cmdd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdd.CommandText = "Get_Inv_QRCode_Data";
                    cmdd.CommandType = CommandType.StoredProcedure;
                    cmdd.Parameters.Add(new SqlParameter("@INVMSI_Id", SqlDbType.BigInt)
                    {
                        Value = data.INVMST_Id
                    });

                    if (cmdd.Connection.State != ConnectionState.Open)
                        cmdd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmdd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_tagdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                _logAT.LogInformation("Sile List :" + ex.Message);
            }
            return data;
        }

        public AssetTagDTO INV_QR_Report(AssetTagDTO data)
        {
            try
            {


                using (var cmdd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdd.CommandText = "SP_GetQRCodeFeedbackReport";
                    cmdd.CommandType = CommandType.StoredProcedure;
                    cmdd.Parameters.Add(new SqlParameter("@From_Date", SqlDbType.Date)
                    {
                        Value = data.startdate
                    });
                    cmdd.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.Date)
                    {
                        Value = data.enddate
                    });
                    if (cmdd.Connection.State != ConnectionState.Open)
                        cmdd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmdd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_tagdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                _logAT.LogInformation("Sile List :" + ex.Message);
            }
            return data;
        }
        public AssetTagDTO INV_QR_Report_User(AssetTagDTO data)
        {
            try
            {


                using (var cmdd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdd.CommandText = "SP_GetQRCodeFeedback_User";
                    cmdd.CommandType = CommandType.StoredProcedure;
                    cmdd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });

                    if (cmdd.Connection.State != ConnectionState.Open)
                        cmdd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmdd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_tagdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                _logAT.LogInformation("Sile List :" + ex.Message);
            }
            return data;
        }
        public AssetTagDTO INV_QR_View_Item(AssetTagDTO data)
        {
            try
            {


                using (var cmdd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdd.CommandText = "SP_GetItemCountByGroup";
                    cmdd.CommandType = CommandType.StoredProcedure;
                    cmdd.Parameters.Add(new SqlParameter("@INVMSI_Id", SqlDbType.BigInt)
                    {
                        Value = data.INVMST_Id
                    });
                    cmdd.Parameters.Add(new SqlParameter("@INVMG_Id", SqlDbType.BigInt)
                    {
                        Value = data.INVMG_Id
                    });

                    if (cmdd.Connection.State != ConnectionState.Open)
                        cmdd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmdd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_items = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                _logAT.LogInformation("Sile List :" + ex.Message);
            }
            return data;
        }
        public static byte[] BitmapToByteArray(Bitmap QrBitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                QrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        #endregion
        public AssetTagDTO getloaddata(AssetTagDTO data)
        {
            try
            {
                data.get_store = (from a in _ATContext.INV_StockDMO
                                  from b in _ATContext.INV_Master_StoreDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                  select new AssetTagDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMS_StoreName = b.INVMS_StoreName,
                                  }).Distinct().OrderBy(m => m.INVMST_Id).ToArray();


                data.get_Assetstag = (from a in _ATContext.INV_Asset_AssetTagDMO
                                      from b in _ATContext.INV_Master_StoreDMO
                                      from c in _ATContext.INV_Master_ItemDMO
                                      where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                      select new AssetTagDTO
                                      {
                                          INVAAT_Id = a.INVAAT_Id,
                                          INVMST_Id = a.INVMST_Id,
                                          INVMS_StoreName = b.INVMS_StoreName,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVAAT_AssetId = a.INVAAT_AssetId,
                                          INVAAT_AssetDescription = a.INVAAT_AssetDescription,
                                          INVAAT_ManufacturedDate = a.INVAAT_ManufacturedDate,
                                          INVAAT_SKU = a.INVAAT_SKU,
                                          INVAAT_ModelNo = a.INVAAT_ModelNo,
                                          INVAAT_SerialNo = a.INVAAT_SerialNo,
                                          INVAAT_PurchaseDate = a.INVAAT_PurchaseDate,
                                          INVAAT_WarantyPeriod = a.INVAAT_WarantyPeriod,
                                          INVAAT_WarantyExpiryDate = a.INVAAT_WarantyExpiryDate,
                                          INVAAT_UnderAMCFlg = a.INVAAT_UnderAMCFlg,
                                          INVAAT_AMCExpiryDate = a.INVAAT_AMCExpiryDate,
                                          INVAAT_CheckOutFlg = a.INVAAT_CheckOutFlg,
                                          INVAAT_DisposedFlg = a.INVAAT_DisposedFlg,
                                          INVAAT_ActiveFlg = a.INVAAT_ActiveFlg,
                                          //CommonFlag = a.CommonFlag


                                      }).Distinct().OrderBy(m => m.INVAAT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<AssetTagDTO> getdata(AssetTagDTO data)
        {
            try
            {
                //======================================== Items with quantity 
                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTag_Item_Qty";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.INVMST_Id
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
                        data.get_items = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagData";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.INVMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.INVMI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@PurchaseRate",
               SqlDbType.Decimal)
                    {
                        Value = data.PurchaseRate
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVSTO_Id",
               SqlDbType.BigInt)
                    {
                        Value = data.INVSTO_Id
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
                        data.get_tagdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag get Data:" + ex.Message);
            }
            return data;
        }

        public AssetTagDTO savedata(AssetTagDTO data)
        {
            try
            {
                if (data.INVAAT_Id != 0)
                {
                    foreach (var t in data.tagckdArray)
                    {
                        DateTime? mafdate = null;
                        DateTime? purchasedate = null;
                        DateTime? wartydate = null;
                        DateTime? amcdate = null;
                        if (t.INVAAT_ManufacturedDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            mafdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_ManufacturedDate), INDIAN_ZONE);
                        }

                        if (t.INVAAT_PurchaseDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            purchasedate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_PurchaseDate), INDIAN_ZONE1);
                        }

                        if (t.INVAAT_WarantyExpiryDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE2 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            wartydate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_WarantyExpiryDate), INDIAN_ZONE2);
                        }

                        if (t.INVAAT_UnderAMCFlg == true)
                        {
                            if (t.INVAAT_AMCExpiryDate != null)
                            {
                                TimeZoneInfo INDIAN_ZONE3 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                amcdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_AMCExpiryDate), INDIAN_ZONE3);
                            }
                        }

                        var res = _ATContext.INV_Asset_AssetTagDMO.Where(i => i.INVMST_Id == t.INVMST_Id && i.INVMI_Id == t.INVMI_Id && i.MI_Id == data.MI_Id && i.INVAAT_AssetId == t.INVAAT_AssetId && i.INVAAT_Id != data.INVAAT_Id).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _ATContext.INV_Asset_AssetTagDMO.Single(a => a.MI_Id == data.MI_Id && a.INVAAT_Id == data.INVAAT_Id);
                            result.MI_Id = data.MI_Id;
                            result.INVAAT_AssetId = t.INVAAT_AssetId;
                            result.INVAAT_AssetDescription = t.INVAAT_AssetDescription;
                            result.INVAAT_ManufacturerName = t.INVAAT_ManufacturerName;
                            result.INVAAT_ManufacturedDate = mafdate;
                            result.INVAAT_SKU = t.INVAAT_SKU;
                            result.INVAAT_ModelNo = t.INVAAT_ModelNo;
                            result.INVAAT_SerialNo = t.INVAAT_SerialNo;
                            result.INVAAT_PurchaseDate = purchasedate;
                            result.INVAAT_WarantyPeriod = t.INVAAT_WarantyPeriod;
                            result.INVAAT_WarantyExpiryDate = wartydate;
                            result.INVAAT_UnderAMCFlg = t.INVAAT_UnderAMCFlg;
                            result.INVAAT_AMCExpiryDate = amcdate;
                            result.INVAAT_ActiveFlg = true;
                            result.UpdatedDate = DateTime.Now;
                            result.INVAAT_FilePath = t.staffcheck;
                            //result.CommonFlag = t.CommonFlag;
                            _ATContext.Update(result);

                            var contactExists = _ATContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }

                    }
                }
                else
                {
                    foreach (var t in data.tagckdArray)
                    {
                        DateTime? mafdate = null;
                        DateTime? purchasedate = null;
                        DateTime? wartydate = null;
                        DateTime? amcdate = null;
                        if (t.INVAAT_ManufacturedDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            mafdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_ManufacturedDate), INDIAN_ZONE);
                        }


                        if (t.INVAAT_PurchaseDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            purchasedate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_PurchaseDate), INDIAN_ZONE1);
                        }


                        if (t.INVAAT_WarantyExpiryDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE2 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            wartydate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_WarantyExpiryDate), INDIAN_ZONE2);
                        }


                        if (t.INVAAT_AMCExpiryDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE3 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            amcdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_AMCExpiryDate), INDIAN_ZONE3);
                        }

                        var res = _ATContext.INV_Asset_AssetTagDMO.Where(i => i.INVMST_Id == t.INVMST_Id && i.INVMI_Id == t.INVMI_Id && i.MI_Id == data.MI_Id && i.INVAAT_AssetId == t.INVAAT_AssetId).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            INV_Asset_AssetTagDMO tag = new INV_Asset_AssetTagDMO();
                            tag.MI_Id = data.MI_Id;
                            tag.INVMST_Id = t.INVMST_Id;
                            tag.INVMI_Id = t.INVMI_Id;
                            tag.INVAAT_AssetId = t.INVAAT_AssetId;
                            tag.INVAAT_AssetDescription = t.INVAAT_AssetDescription;
                            tag.INVAAT_ManufacturerName = t.INVAAT_ManufacturerName;
                            tag.INVAAT_ManufacturedDate = mafdate;
                            tag.INVAAT_SKU = t.INVAAT_SKU;
                            tag.INVAAT_ModelNo = t.INVAAT_ModelNo;
                            tag.INVAAT_SerialNo = t.INVAAT_SerialNo;
                            tag.INVAAT_PurchaseDate = purchasedate;
                            tag.INVAAT_WarantyPeriod = t.INVAAT_WarantyPeriod;
                            tag.INVAAT_WarantyExpiryDate = wartydate;
                            tag.INVAAT_UnderAMCFlg = t.INVAAT_UnderAMCFlg;
                            tag.INVAAT_AMCExpiryDate = amcdate;
                            tag.INVAAT_CheckOutFlg = false;
                            tag.INVAAT_DisposedFlg = false;
                            tag.INVAAT_ActiveFlg = true;
                            tag.UpdatedDate = DateTime.Now;
                            tag.CreatedDate = DateTime.Now;
                            tag.INVAAT_FilePath = t.staffcheck;
                            //tag.CommonFlag = t.CommonFlag;
                            _ATContext.Add(tag);
                        }
                    }
                    var contextOB = _ATContext.SaveChanges();
                    if (contextOB > 0)
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

                _logAT.LogInformation("Asset tag savedata :" + ex.Message);
            }
            return data;
        }


        public AssetTagDTO deactive(AssetTagDTO data)
        {
            try
            {
                var result = _ATContext.INV_Asset_AssetTagDMO.Single(t => t.INVAAT_Id == data.INVAAT_Id);

                if (result.INVAAT_ActiveFlg == true)
                {
                    result.INVAAT_ActiveFlg = false;
                }
                else if (result.INVAAT_ActiveFlg == false)
                {
                    result.INVAAT_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
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

        public AssetTagDTO get_edit_data(AssetTagDTO dto)
        {
            try
            {
                dto.get_edit_data = (from a in _ATContext.INV_Asset_AssetTagDMO
                                     from b in _ATContext.INV_Master_StoreDMO
                                     from c in _ATContext.INV_Master_ItemDMO
                                     where a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVAAT_Id == dto.INVAAT_Id && a.INVMST_Id == dto.INVMST_Id && a.MI_Id == dto.MI_Id
                                     select new AssetTagDTO
                                     {
                                         INVMS_StoreName = b.INVMS_StoreName,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVMST_Id = b.INVMST_Id,
                                         INVMI_Id = c.INVMI_Id,
                                         INVAAT_Id = a.INVAAT_Id,
                                         staffcheck = a.INVAAT_FilePath,
                                         //CommonFlag = a.CommonFlag

                                     }).ToList().ToArray();


                // _ATContext.INV_Asset_AssetTagDMO.Where(t => t.INVAAT_Id == dto.INVAAT_Id && t.INVMST_Id==dto.INVMST_Id && t.MI_Id==dto.MI_Id).ToArray();

                dto.get_edit_store = (from a in _ATContext.INV_Asset_AssetTagDMO
                                      from b in _ATContext.INV_Master_StoreDMO
                                      where a.INVMST_Id == b.INVMST_Id && a.INVAAT_Id == dto.INVAAT_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id
                                      select new AssetTagDTO
                                      {
                                          INVMST_Id = b.INVMST_Id,
                                          INVMS_StoreName = b.INVMS_StoreName
                                      }).ToArray();


                dto.get_edit_item = (from a in _ATContext.INV_Asset_AssetTagDMO
                                     from b in _ATContext.INV_Master_ItemDMO
                                     where a.INVMI_Id == b.INVMI_Id && a.INVAAT_Id == dto.INVAAT_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id
                                     select new AssetTagDTO
                                     {
                                         INVMI_Id = b.INVMI_Id,
                                         INVMI_ItemName = b.INVMI_ItemName
                                     }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
