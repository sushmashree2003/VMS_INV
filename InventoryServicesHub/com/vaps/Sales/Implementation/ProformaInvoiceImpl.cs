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
    public class ProformaInvoiceImpl : Interface.ProformaInvoiceInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_T_SalesImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public ProformaInvoiceImpl(InventoryContext InvContext, ILogger<INV_T_SalesImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public async Task<ProformaInvoiceDTO> getloaddata(ProformaInvoiceDTO data)
        {
            try
            {
                try
                {
                    data.instlist = _INVContext.Institution.Where(m => m.MI_Id == data.MI_Id && m.MI_ActiveFlag == 1).OrderBy(m => m.MI_Id).ToArray();
                    var config = _INVContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                    if (config.Count>0)
                    {
                        data.INVC_LIFOFIFOFlg = config.FirstOrDefault().INVC_LIFOFIFOFlg;
                    }
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_SALES_FOR_INVOICE";
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
                            data.get_Sale = retObject.ToArray();
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

        public async Task<ProformaInvoiceDTO> getStudentClsSec(ProformaInvoiceDTO data)
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
        public ProformaInvoiceDTO getsectionlist(ProformaInvoiceDTO data)
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
                                        select new ProformaInvoiceDTO
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

        public async Task<ProformaInvoiceDTO> getStudentlist(ProformaInvoiceDTO data)
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

        public async Task<ProformaInvoiceDTO> getitem(ProformaInvoiceDTO data)
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

        public async Task<ProformaInvoiceDTO> getitemDetail(ProformaInvoiceDTO data)
        {
            try
            {

                data.instlist = _INVContext.Institution.Where(m => m.MI_Id == data.MI_Id && m.MI_ActiveFlag == 1).OrderBy(m => m.MI_Id).ToArray();
                data.get_SaleItemDetails = (from a in _INVContext.INV_T_SalesDMO
                                            from b in _INVContext.INV_M_SalesDMO
                                            from c in _INVContext.INV_Master_ItemDMO
                                            from d in _INVContext.INV_Master_UOMDMO
                                            where (a.INVMSL_Id == b.INVMSL_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && a.INVMSL_Id == data.INVMSL_Id && a.INVTSL_ActiveFlg==true)
                                            select new ProformaInvoiceDTO
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
                                        where (a.INVTSL_Id == b.INVTSL_Id && a.INVMT_Id == c.INVMT_Id && c.MI_Id == data.MI_Id && b.INVMSL_Id == data.INVMSL_Id && a.INVTSLT_ActiveFlg==true)
                                        select new ProformaInvoiceDTO
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

        public async Task<ProformaInvoiceDTO> savedetails(ProformaInvoiceDTO data)
        {
            try
            {
                long sale_id = 0;
                string s = "";
                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }
                var res = _INVContext.INV_M_SalesDMO.Where(t => t.INVMSL_SalesNo == data.trans_id && t.INVMSL_SalesDate == data.INVMSL_SalesDate && t.INVMSL_SalesValue == data.INVMSL_SalesValue && t.INVMST_Id == data.INVMST_Id && t.MI_Id == data.MI_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    INV_M_SalesDMO sale = new INV_M_SalesDMO();
                    sale.MI_Id = data.MI_Id;
                    sale.INVMST_Id = data.INVMST_Id;
                    sale.INVMP_Id = data.INVMP_Id;
                    sale.INVMSL_StuOtherFlg = data.INVMSL_StuOtherFlg;
                    sale.INVMSL_SalesNo = data.trans_id;
                    sale.INVMSL_SalesDate = data.INVMSL_SalesDate;
                    sale.INVMSL_SalesValue = data.INVMSL_SalesValue;
                    sale.INVMSL_TotDiscount = data.INVMSL_TotDiscount;
                    sale.INVMSL_TotTaxAmt = data.INVMSL_TotTaxAmt;
                    sale.INVMSL_TotalAmount = data.INVMSL_TotalAmount;
                    sale.INVMSL_Remarks = data.INVMSL_Remarks;
                    sale.INVMSL_CreditFlg = data.INVMSL_CreditFlg;
                    sale.INVMSL_ActiveFlg = true;
                    sale.CreatedDate = DateTime.Now;
                    sale.UpdatedDate = DateTime.Now;
                    _INVContext.Add(sale);

                    if (data.INVMSL_StuOtherFlg == "Student")
                    {
                        if (data.Student_flag == "I")
                        {
                            var clsSec = (from a in _INVContext.Adm_M_Student
                                          from b in _INVContext.School_Adm_Y_StudentDMO
                                          from c in _INVContext.AcademicYear
                                          from d in _INVContext.School_M_Class
                                          from e in _INVContext.School_M_Section
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)
                                          select new ProformaInvoiceDTO
                                          {
                                              ASMCL_Id = d.ASMCL_Id,
                                              ASMS_Id = e.ASMS_Id
                                          }).Distinct().ToArray();

                            INV_M_Sales_StudentDMO salestudent = new INV_M_Sales_StudentDMO();
                            salestudent.INVMSL_Id = sale.INVMSL_Id;
                            salestudent.AMST_Id = data.AMST_Id;
                            salestudent.ASMCL_Id = clsSec.FirstOrDefault().ASMCL_Id;
                            salestudent.ASMS_Id = clsSec.FirstOrDefault().ASMS_Id;
                            salestudent.ASMAY_Id = data.ASMAY_Id;
                            salestudent.INVMSLS_ActiveFlg = true;

                            salestudent.CreatedDate = DateTime.Now;
                            salestudent.UpdatedDate = DateTime.Now;
                            _INVContext.Add(salestudent);
                        }
                        else if (data.Student_flag == "C" || data.Student_flag == "CS")
                        {
                            foreach (var c in data.studentlist)
                            {
                                INV_M_Sales_StudentDMO Stulist = new INV_M_Sales_StudentDMO();
                                Stulist.INVMSL_Id = sale.INVMSL_Id;
                                Stulist.AMST_Id = c.AMST_Id;
                                Stulist.ASMCL_Id = c.ASMCL_Id;
                                Stulist.ASMS_Id = c.ASMS_Id;
                                Stulist.ASMAY_Id = data.ASMAY_Id;
                                Stulist.INVMSLS_ActiveFlg = true;

                                Stulist.CreatedDate = DateTime.Now;
                                Stulist.UpdatedDate = DateTime.Now;
                                _INVContext.Add(Stulist);
                            }
                        }
                    }
                    else if (data.INVMSL_StuOtherFlg == "Staff")
                    {
                        INV_M_Sales_StaffDMO staff = new INV_M_Sales_StaffDMO();

                        staff.INVMSL_Id = sale.INVMSL_Id;
                        staff.HRME_Id = data.HRME_Id;
                        staff.INVMSLST_ActiveFlg = true;


                        staff.CreatedDate = DateTime.Now;
                        staff.UpdatedDate = DateTime.Now;
                        _INVContext.Add(staff);
                    }
                    else if (data.INVMSL_StuOtherFlg == "Customer")
                    {
                        INV_M_Sales_CustomerDMO customer = new INV_M_Sales_CustomerDMO();
                        customer.INVMSL_Id = sale.INVMSL_Id;
                        customer.INVMC_Id = data.INVMC_Id;
                        customer.INVMSLC_ActiveFlg = true;

                        customer.CreatedDate = DateTime.Now;
                        customer.UpdatedDate = DateTime.Now;
                        _INVContext.Add(customer);
                    }
                    //===================================Sales Transcation Data
                    foreach (var i in data.SaleItem)
                    {
                        INV_T_SalesDMO saleitem = new INV_T_SalesDMO();
                        saleitem.INVMSL_Id = sale.INVMSL_Id;
                        saleitem.INVMI_Id = i.INVMI_Id;
                        saleitem.INVMUOM_Id = i.INVMUOM_Id;
                        //saleitem.INVSTO_Id = i.INVSTO_Id;
                        saleitem.INVTSL_BatchNo = i.INVTSL_BatchNo;
                        saleitem.INVTSL_SalesQty = i.INVTSL_SalesQty;
                        saleitem.INVTSL_TaxAmt = i.INVTSL_TaxAmt;
                        saleitem.INVTSL_SalesPrice = i.INVTSL_SalesPrice;
                        saleitem.INVTSL_DiscountAmt = i.INVTSL_DiscountAmt;
                        saleitem.INVTSL_Amount = i.INVTSL_Amount;
                        saleitem.INVTSL_Naration = i.INVTSL_Naration;
                        saleitem.INVTSL_ActiveFlg = true;
                        saleitem.CreatedDate = DateTime.Now;
                        saleitem.UpdatedDate = DateTime.Now;
                        _INVContext.Add(saleitem);
                        //===================================Sales Transcation Tax
                        foreach (var t in i.saleItemTax)
                        {
                            INV_T_Sales_TaxDMO saletax = new INV_T_Sales_TaxDMO();
                            saletax.INVTSL_Id = saleitem.INVTSL_Id;
                            saletax.INVMT_Id = t.INVMT_Id;
                            saletax.INVTSLT_TaxPer = t.INVTSLT_TaxPer;
                            saletax.INVTSLT_TaxAmt = t.INVTSLT_TaxAmt;
                            saletax.INVTSLT_ActiveFlg = true;

                            saletax.CreatedDate = DateTime.Now;
                            saletax.UpdatedDate = DateTime.Now;
                            _INVContext.Add(saletax);
                        }
                        using (var dbSaleTxn = _INVContext.Database.BeginTransaction())
                        {
                            sale_id = sale.INVMSL_Id;
                            var contexttrans = _INVContext.SaveChanges();
                            dbSaleTxn.Commit();

                            if (contexttrans > 0)
                            {
                                try
                                {
                                    var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_InsertSales @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.INVMST_Id, sale.INVMSL_Id, saleitem.INVMI_Id, saleitem.INVTSL_SalesPrice);
                                    if (contactExistsP > 0)
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
                                dbSaleTxn.Rollback();
                                data.returnval = false;
                            }
                        }
                    }
                    //********************************* SALES SMS ********************************//
                    long amst_mobileno = 0;
                    long SalePrice = 0;
                    string SaleDate = "";
                    List<long> amstids = new List<long>();
                    if (data.INVMSL_StuOtherFlg == "Student")
                    {
                        if (data.Student_flag == "I")
                        {
                            var amstid = _INVContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && a.AMST_Id == data.AMST_Id).Distinct().ToList();
                            if (amstid.Count > 0)
                            {
                                var saledetails = _INVContext.INV_M_SalesDMO.Where(t => t.MI_Id == data.MI_Id && t.INVMSL_Id == sale_id && t.INVMSL_ActiveFlg == true).Distinct().ToList();
                                if (saledetails.Count > 0)
                                {
                                    SalePrice = Convert.ToInt64(saledetails.FirstOrDefault().INVMSL_SalesValue);
                                    SaleDate = saledetails.FirstOrDefault().INVMSL_SalesDate.ToString();

                                }
                                amst_mobileno = amstid.FirstOrDefault().AMST_MobileNo;
                                SMS sms = new SMS(_db);
                                s = await sms.sendSalesSms(data.MI_Id, amst_mobileno, "INVSales", SalePrice, SaleDate);
                            }
                        }
                        else if (data.Student_flag == "C" || data.Student_flag == "CS")
                        {
                            long nofstudent = data.studentlist.Length;
                            long salerate = 0;
                            foreach (var c in data.studentlist)
                            {
                                amstids.Add(c.AMST_Id);
                            }
                            if (amstids.Count > 0)
                            {
                                var amstmobileno = _INVContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && amstids.Contains(a.AMST_Id)).Distinct().ToList();

                                if (amstmobileno.Count > 0)
                                {
                                    var saledetails = _INVContext.INV_M_SalesDMO.Where(t => t.MI_Id == data.MI_Id && t.INVMSL_Id == sale_id && t.INVMSL_ActiveFlg == true).Distinct().ToList();
                                    if (saledetails.Count > 0)
                                    {
                                        salerate = Convert.ToInt64(saledetails.FirstOrDefault().INVMSL_SalesValue);
                                        SaleDate = saledetails.FirstOrDefault().INVMSL_SalesDate.ToString();
                                        SalePrice = salerate / nofstudent;
                                    }
                                    foreach (var m in amstmobileno)
                                    {
                                        amst_mobileno = m.AMST_MobileNo;
                                        SMS sms = new SMS(_db);
                                        s = await sms.sendSalesSms(data.MI_Id, amst_mobileno, "INVSales", SalePrice, SaleDate);
                                    }
                                }
                            }

                        }
                    }



                    //var contactExists = _INVContext.SaveChanges();
                    //if (contactExists > 0)
                    //{
                    //    data.returnval = true;
                    //}
                    //else
                    //{
                    //    data.returnval = false;
                    //}

                    //else
                    //{
                    //    dbSaleTxn.Rollback();
                    //}
                }

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Sales savedata :" + ex.Message);
            }

            return data;
        }

        //====================================Grid Model Data
        public async Task<ProformaInvoiceDTO> getSaletypes(ProformaInvoiceDTO data)
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
        public async Task<ProformaInvoiceDTO> getSaleItemDetails(ProformaInvoiceDTO data)
        {
            try
            {
                data.get_SaleItemDetails = (from a in _INVContext.INV_T_SalesDMO
                                            from b in _INVContext.INV_M_SalesDMO
                                            from c in _INVContext.INV_Master_ItemDMO
                                            from d in _INVContext.INV_Master_UOMDMO
                                            where (a.INVMSL_Id == b.INVMSL_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && a.INVMSL_Id == data.INVMSL_Id)
                                            select new ProformaInvoiceDTO
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
        public ProformaInvoiceDTO getSaleItemTax(ProformaInvoiceDTO data)
        {
            try
            {
                data.get_SaleItemTax = (from a in _INVContext.INV_T_Sales_TaxDMO
                                        from b in _INVContext.INV_T_SalesDMO
                                        from c in _INVContext.INV_Master_TaxDMO
                                        where (a.INVTSL_Id == b.INVTSL_Id && a.INVMT_Id == c.INVMT_Id && c.MI_Id == data.MI_Id && a.INVTSL_Id == data.INVTSL_Id)
                                        select new ProformaInvoiceDTO
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
        public ProformaInvoiceDTO deactive(ProformaInvoiceDTO data)
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
                                 select new ProformaInvoiceDTO
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
                                         select new ProformaInvoiceDTO
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
                                           select new ProformaInvoiceDTO
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
        public ProformaInvoiceDTO deactiveS(ProformaInvoiceDTO data)
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
        public ProformaInvoiceDTO deactivetax(ProformaInvoiceDTO data)
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
