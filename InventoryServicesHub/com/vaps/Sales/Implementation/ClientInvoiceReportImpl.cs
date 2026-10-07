using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
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
    public class ClientInvoiceReportImpl : Interface.ClientInvoiceReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_T_SalesImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public ClientInvoiceReportImpl(InventoryContext InvContext, ILogger<INV_T_SalesImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public async Task<ClientInvoiceReportDTO> getloaddata(ClientInvoiceReportDTO data)
        {
            try
            {
                try
                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_master_institution_proc_DC";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
               SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Userid",
                SqlDbType.VarChar)
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
                            data.get_mi_list = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logInv.LogInformation("Sales Item :" + ex.Message);
                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<ClientInvoiceReportDTO> getStudentClsSec(ClientInvoiceReportDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_AMSTID_CLASS_SECTION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = data.type
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
                        data.get_Student_Cls_Sec = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("get_StudentClsSec :" + ex.Message);
            }
            return data;
        }
        public ClientInvoiceReportDTO getsectionlist(ClientInvoiceReportDTO data)
        {
            try
            {
                data.get_sectionlist = (from a in _INVContext.Adm_M_Student
                                        from b in _INVContext.School_Adm_Y_StudentDMO
                                        from c in _INVContext.AcademicYear
                                        from d in _INVContext.School_M_Class
                                        from e in _INVContext.School_M_Section
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMS_Id == e.ASMS_Id
                                        && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id)
                                        select new ClientInvoiceReportDTO
                                        {
                                            ASMS_Id = e.ASMS_Id,
                                            ASMC_SectionName = e.ASMC_SectionName
                                        }).Distinct().OrderBy(m => m.ASMS_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Section List:" + ex.Message);
            }
            return data;
        }

        public async Task<ClientInvoiceReportDTO> getStudentlist(ClientInvoiceReportDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_STUDENTLIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = data.type
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
                        data.get_Studentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("get_StudentClsSec :" + ex.Message);
            }
            return data;
        }

        public async Task<ClientInvoiceReportDTO> getitem(ClientInvoiceReportDTO data)
        {
            try
            {
                var config = _INVContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                data.INVC_LIFOFIFOFlg = config.FirstOrDefault().INVC_LIFOFIFOFlg;

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_LIFO_FIFO_ITEM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVC_LIFOFIFOFlg",
                  SqlDbType.VarChar)
                    {
                        Value = data.INVC_LIFOFIFOFlg
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                 SqlDbType.BigInt)
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
                        data.get_item = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Inventory Sales get items:" + ex.Message);
            }
            return data;
        }

        public async Task<ClientInvoiceReportDTO> getitemDetail(ClientInvoiceReportDTO data)
        {
            try
            {

                data.instlist = _INVContext.Institution.Where(m => m.MI_Id == data.MI_Id && m.MI_ActiveFlag == 1).OrderBy(m => m.MI_Id).ToArray();
                data.get_SaleItemDetails = (from a in _INVContext.INV_T_SalesDMO
                                            from b in _INVContext.INV_M_SalesDMO
                                            from c in _INVContext.INV_Master_ItemDMO
                                            from d in _INVContext.INV_Master_UOMDMO
                                            where (a.INVMSL_Id == b.INVMSL_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && a.INVMSL_Id == data.INVMSL_Id && a.INVTSL_ActiveFlg == true)
                                            select new SalesInvoiceDTO
                                            {
                                                INVTSL_Id = a.INVTSL_Id,
                                                INVMSL_Id = a.INVMSL_Id,
                                                INVMI_Id = a.INVMI_Id,
                                                INVMUOM_Id = a.INVMUOM_Id,
                                                INVMSL_SalesNo = b.INVMSL_SalesNo,
                                                INVMI_ItemName = c.INVMI_ItemName,
                                                INVMUOM_UOMName = d.INVMUOM_UOMName,
                                                INVTSL_BatchNo = a.INVTSL_BatchNo,
                                                INVTSL_SalesQty = a.INVTSL_SalesQty,
                                                INVTSL_SalesPrice = a.INVTSL_SalesPrice,
                                                INVTSL_DiscountAmt = a.INVTSL_DiscountAmt,
                                                INVTSL_TaxAmt = a.INVTSL_TaxAmt,
                                                INVTSL_Amount = a.INVTSL_Amount,
                                                INVTSL_Naration = a.INVTSL_Naration,
                                                INVTSL_ActiveFlg = a.INVTSL_ActiveFlg,
                                            }).Distinct().OrderBy(m => m.INVTSL_Id).ToArray();

                data.get_SaleItemTax = (from a in _INVContext.INV_T_Sales_TaxDMO
                                        from b in _INVContext.INV_T_SalesDMO
                                        from c in _INVContext.INV_Master_TaxDMO
                                        where (a.INVTSL_Id == b.INVTSL_Id && a.INVMT_Id == c.INVMT_Id && c.MI_Id == data.MI_Id && b.INVMSL_Id == data.INVMSL_Id && a.INVTSLT_ActiveFlg == true)
                                        select new SalesInvoiceDTO
                                        {
                                            INVTSLT_Id = a.INVTSLT_Id,
                                            INVTSL_Id = a.INVTSL_Id,
                                            INVMT_Id = a.INVMT_Id,
                                            INVMT_TaxName = c.INVMT_TaxName,
                                            INVTSLT_TaxPer = a.INVTSLT_TaxPer,
                                            INVTSLT_TaxAmt = a.INVTSLT_TaxAmt,
                                            INVTSLT_ActiveFlg = a.INVTSLT_ActiveFlg

                                        }).Distinct().OrderBy(m => m.INVTSLT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales Item Details:" + ex.Message);
            }
            return data;
        }

        public async Task<ClientInvoiceReportDTO> savedetails(ClientInvoiceReportDTO data)
        {
            try
            {
                string miid = "0";
                List<long> Mi_idlist = new List<long>();
                foreach (var ii in data.get_mi_arraylist)
                {
                    miid = miid + "," + ii.MI_Id;
                    Mi_idlist.Add(ii.MI_Id);
                }
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLIENT_INVOICEREPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.VarChar)
                    {
                        Value = miid
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                  SqlDbType.VarChar)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.enddate
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
                        data.get_salesdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.banklist = _db.HR_Master_BankDeatils.Where(R => Mi_idlist.Contains(R.MI_Id)).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Sales savedata :" + ex.Message);
            }

            return data;
        }
        public ClientInvoiceReportDTO Editdata(ClientInvoiceReportDTO data)
        {
            try
            {
                data.instlist = _INVContext.Institution.Where(m => m.MI_Id == data.MI_Id && m.MI_ActiveFlag == 1).OrderBy(m => m.MI_Id).ToArray();
                var editlist = _INVContext.ISM_InvoiceDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).Distinct().ToList();

                data.editlist = editlist.ToArray();


                data.clientlist = _INVContext.ISM_Master_Client_DMO.Where(d => d.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id).ToArray();

                data.get_itemTax = _INVContext.INV_Master_TaxDMO.Where(d => d.MI_Id == data.MI_Id).Distinct().ToArray();

              


                data.editlistdetails = _INVContext.ISM_Invoice_DetailsDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).ToArray();
                data.editlisttax = _INVContext.ISM_Invoice_TaxDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        //====================================Grid Model Data
        public async Task<ClientInvoiceReportDTO> getSaletypes(ClientInvoiceReportDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_SALE_TYPES_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMSL_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.INVMSL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@saletype",
                    SqlDbType.VarChar)
                    {
                        Value = data.saletype
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
                        data.get_Saletypes = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sale Types :" + ex.Message);
            }
            return data;
        }
        public async Task<ClientInvoiceReportDTO> getSaleItemDetails(ClientInvoiceReportDTO data)
        {
            try
            {
                data.get_SaleItemDetails = (from a in _INVContext.INV_T_SalesDMO
                                            from b in _INVContext.INV_M_SalesDMO
                                            from c in _INVContext.INV_Master_ItemDMO
                                            from d in _INVContext.INV_Master_UOMDMO
                                            where (a.INVMSL_Id == b.INVMSL_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && a.INVMSL_Id == data.INVMSL_Id)
                                            select new ClientInvoiceReportDTO
                                            {
                                                INVTSL_Id = a.INVTSL_Id,
                                                INVMSL_Id = a.INVMSL_Id,
                                                INVMI_Id = a.INVMI_Id,
                                                INVMUOM_Id = a.INVMUOM_Id,
                                                INVMSL_SalesNo = b.INVMSL_SalesNo,
                                                INVMI_ItemName = c.INVMI_ItemName,
                                                INVMUOM_UOMName = d.INVMUOM_UOMName,
                                                INVTSL_BatchNo = a.INVTSL_BatchNo,
                                                INVTSL_SalesQty = a.INVTSL_SalesQty,
                                                INVTSL_SalesPrice = a.INVTSL_SalesPrice,
                                                INVTSL_DiscountAmt = a.INVTSL_DiscountAmt,
                                                INVTSL_TaxAmt = a.INVTSL_TaxAmt,
                                                INVTSL_Amount = a.INVTSL_Amount,
                                                INVTSL_Naration = a.INVTSL_Naration,
                                                INVTSL_ActiveFlg = a.INVTSL_ActiveFlg,


                                            }).Distinct().OrderBy(m => m.INVTSL_Id).ToArray();

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_SALE_TYPES_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMSL_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.INVMSL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@saletype",
                    SqlDbType.VarChar)
                    {
                        Value = data.saletype
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
                        data.get_Saletypes = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("sale Model:" + ex.Message);
            }
            return data;
        }
        public ClientInvoiceReportDTO getSaleItemTax(ClientInvoiceReportDTO data)
        {
            try
            {
                data.get_SaleItemTax = (from a in _INVContext.INV_T_Sales_TaxDMO
                                        from b in _INVContext.INV_T_SalesDMO
                                        from c in _INVContext.INV_Master_TaxDMO
                                        where (a.INVTSL_Id == b.INVTSL_Id && a.INVMT_Id == c.INVMT_Id && c.MI_Id == data.MI_Id && a.INVTSL_Id == data.INVTSL_Id)
                                        select new ClientInvoiceReportDTO
                                        {
                                            INVTSLT_Id = a.INVTSLT_Id,
                                            INVTSL_Id = a.INVTSL_Id,
                                            INVMT_Id = a.INVMT_Id,
                                            INVMT_TaxName = c.INVMT_TaxName,
                                            INVTSLT_TaxPer = a.INVTSLT_TaxPer,
                                            INVTSLT_TaxAmt = a.INVTSLT_TaxAmt,
                                            INVTSLT_ActiveFlg = a.INVTSLT_ActiveFlg

                                        }).Distinct().OrderBy(m => m.INVTSLT_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("sale Tax Model:" + ex.Message);
            }
            return data;
        }
        //====================================Grid Activate and Deactivate
        public ClientInvoiceReportDTO deactive(ClientInvoiceReportDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_SalesDMO.Single(t => t.INVMSL_Id == data.INVMSL_Id && t.MI_Id == data.MI_Id);

                if (result.INVMSL_ActiveFlg == true)
                {
                    result.INVMSL_ActiveFlg = false;
                }
                else if (result.INVMSL_ActiveFlg == false)
                {
                    result.INVMSL_ActiveFlg = true;
                }
                var resultt = _INVContext.INV_T_SalesDMO.Where(t => t.INVMSL_Id == data.INVMSL_Id).ToList();
                foreach (var r in resultt)
                {
                    if (result.INVMSL_ActiveFlg == true)
                    {
                        r.INVTSL_ActiveFlg = true;
                    }
                    else if (result.INVMSL_ActiveFlg == false)
                    {
                        r.INVTSL_ActiveFlg = false;
                    }

                    // if (r.INVTSL_ActiveFlg == true)
                    //{
                    // r.INVTSL_ActiveFlg = false;
                    //}
                    //else if (r.INVTSL_ActiveFlg == false)
                    //{
                    //    r.INVTSL_ActiveFlg = true;
                    //}
                    r.UpdatedDate = DateTime.Now;
                    _INVContext.Update(r);
                }
                var saletrans = (from a in _INVContext.INV_M_SalesDMO
                                 from b in _INVContext.INV_T_SalesDMO
                                 where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                 select new ClientInvoiceReportDTO
                                 {
                                     INVMSL_Id = a.INVMSL_Id,
                                     INVTSL_Id = b.INVTSL_Id,
                                     INVMSL_StuOtherFlg = a.INVMSL_StuOtherFlg,
                                 }).Distinct().OrderBy(m => m.INVMSL_Id).ToList();
                foreach (var sid in saletrans)
                {
                    var resulttax = _INVContext.INV_T_Sales_TaxDMO.Where(t => t.INVTSL_Id == sid.INVTSL_Id).ToList();
                    foreach (var rtax in resulttax)
                    {
                        if (result.INVMSL_ActiveFlg == true)
                        {
                            rtax.INVTSLT_ActiveFlg = true;
                        }
                        else if (result.INVMSL_ActiveFlg == false)
                        {
                            rtax.INVTSLT_ActiveFlg = false;
                        }

                        //if (rtax.INVTSLT_ActiveFlg == true)
                        //{
                        //    rtax.INVTSLT_ActiveFlg = false;
                        //}
                        //else if (rtax.INVTSLT_ActiveFlg == false)
                        //{
                        //    rtax.INVTSLT_ActiveFlg = true;
                        //}
                        rtax.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rtax);
                    }
                }
                var type = _INVContext.INV_M_SalesDMO.Where(t => t.INVMSL_Id == data.INVMSL_Id).ToList();
                foreach (var typ in type)
                {
                    if (typ.INVMSL_StuOtherFlg == "Staff")
                    {
                        var salestaff = (from a in _INVContext.INV_M_SalesDMO
                                         from b in _INVContext.INV_M_Sales_StaffDMO
                                         where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                         select new ClientInvoiceReportDTO
                                         {
                                             INVMSL_Id = a.INVMSL_Id,
                                             INVMSLST_Id = b.INVMSLST_Id,
                                         }).Distinct().OrderBy(m => m.INVMSLST_Id).ToList();

                        foreach (var salestf in salestaff)
                        {
                            var resultstaff = _INVContext.INV_M_Sales_StaffDMO.Where(t => t.INVMSLST_Id == salestf.INVMSLST_Id).ToList();
                            foreach (var rstaff in resultstaff)
                            {
                                if (result.INVMSL_ActiveFlg == true)
                                {
                                    rstaff.INVMSLST_ActiveFlg = true;
                                }
                                else if (result.INVMSL_ActiveFlg == false)
                                {
                                    rstaff.INVMSLST_ActiveFlg = false;
                                }
                                //if (rstaff.INVMSLST_ActiveFlg == true)
                                //{
                                //    rstaff.INVMSLST_ActiveFlg = false;
                                //}
                                //else if (rstaff.INVMSLST_ActiveFlg == false)
                                //{
                                //    rstaff.INVMSLST_ActiveFlg = true;
                                //}
                                rstaff.UpdatedDate = DateTime.Now;
                                _INVContext.Update(rstaff);
                            }
                        }
                    }

                    else if (typ.INVMSL_StuOtherFlg == "Student")
                    {
                        var salestudent = (from a in _INVContext.INV_M_SalesDMO
                                           from b in _INVContext.INV_M_Sales_StudentDMO
                                           where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                           select new ClientInvoiceReportDTO
                                           {
                                               INVMSL_Id = a.INVMSL_Id,
                                               INVMSLS_Id = b.INVMSLS_Id,
                                           }).Distinct().OrderBy(m => m.INVMSLS_Id).ToList();

                        foreach (var salestu in salestudent)
                        {
                            var resultstu = _INVContext.INV_M_Sales_StudentDMO.Where(t => t.INVMSLS_Id == salestu.INVMSLS_Id).ToList();
                            foreach (var rstu in resultstu)
                            {
                                if (result.INVMSL_ActiveFlg == true)
                                {
                                    rstu.INVMSLS_ActiveFlg = true;
                                }
                                else if (result.INVMSL_ActiveFlg == false)
                                {
                                    rstu.INVMSLS_ActiveFlg = false;
                                }
                                //if (rstu.INVMSLS_ActiveFlg == true)
                                //{
                                //    rstu.INVMSLS_ActiveFlg = false;
                                //}
                                //else if (rstu.INVMSLS_ActiveFlg == false)
                                //{
                                //    rstu.INVMSLS_ActiveFlg = true;
                                //}
                                rstu.UpdatedDate = DateTime.Now;
                                _INVContext.Update(rstu);
                            }
                        }
                    }
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    try
                    {
                        var contextupdate = _INVContext.Database.ExecuteSqlCommand("INV_UpdateStockforDeactiveSales @p0,@p1,@p2,@p3", data.MI_Id, data.IMFY_Id, data.INVMSL_Id, data.INVMST_Id);
                        if (contextupdate > 0)
                        {
                            data.returnduplicatestatus = "Updated";
                        }
                        else
                        {
                            data.returnduplicatestatus = "notUpdated";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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
        public ClientInvoiceReportDTO deactiveS(ClientInvoiceReportDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_SalesDMO.Single(t => t.INVTSL_Id == data.INVTSL_Id);

                if (result.INVTSL_ActiveFlg == true)
                {
                    result.INVTSL_ActiveFlg = false;
                }
                else if (result.INVTSL_ActiveFlg == false)
                {
                    result.INVTSL_ActiveFlg = true;
                }

                var resulttx = _INVContext.INV_T_Sales_TaxDMO.Where(t => t.INVTSL_Id == data.INVTSL_Id).ToList();
                foreach (var tx in resulttx)
                {
                    if (result.INVTSL_ActiveFlg == true)
                    {
                        tx.INVTSLT_ActiveFlg = true;
                    }
                    else if (result.INVTSL_ActiveFlg == false)
                    {
                        tx.INVTSLT_ActiveFlg = false;
                    }

                    tx.UpdatedDate = DateTime.Now;
                    _INVContext.Update(tx);
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
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
        public ClientInvoiceReportDTO deactivetax(ClientInvoiceReportDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_Sales_TaxDMO.Single(t => t.INVTSLT_Id == data.INVTSLT_Id);

                if (result.INVTSLT_ActiveFlg == true)
                {
                    result.INVTSLT_ActiveFlg = false;
                }
                else if (result.INVTSLT_ActiveFlg == false)
                {
                    result.INVTSLT_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
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



    }
}
