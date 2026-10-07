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
using System.Text.RegularExpressions;
using SendGrid;
using System.Net;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using DomainModel.Model;
using System.IO;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_VendorPaymentImpl : Interface.INV_VendorPaymentInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_VendorPaymentImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_VendorPaymentImpl(InventoryContext InvContext, ILogger<INV_VendorPaymentImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_VendorPaymentDTO getloaddata(INV_VendorPaymentDTO data)
        {
            try
            {
                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).Distinct().OrderBy(m => m.INVMS_Id).ToArray();
                data.get_paymentMode = _INVContext.IVRM_ModeOfPaymentDMO.Where(m => m.MI_Id == data.MI_Id && m.IVRMMOD_ActiveFlag == true).Distinct().OrderBy(m => m.IVRMMOD_Id).ToArray();
                data.get_vendorpayment = (from a in _INVContext.INV_Supplier_PaymentDMO
                                          from b in _INVContext.INV_Master_SupplierDMO
                                          where (a.INVMS_Id == b.INVMS_Id && a.MI_Id == data.MI_Id)
                                          select new INV_VendorPaymentDTO
                                          {
                                              INVSPT_Id = a.INVSPT_Id,
                                              INVMS_Id = a.INVMS_Id,
                                              INVMS_SupplierName = b.INVMS_SupplierName,
                                              INVSPT_PaymentDate = a.INVSPT_PaymentDate,
                                              INVSPT_ModeOfPayment = a.INVSPT_ModeOfPayment,
                                              INVSPT_PaymentReference = a.INVSPT_PaymentReference,
                                              INVSPT_ChequeDDNo = a.INVSPT_ChequeDDNo,
                                              INVSPT_BankName = a.INVSPT_BankName,
                                              INVSPT_ChequeDDDate = a.INVSPT_ChequeDDDate,
                                              INVSPT_Amount = a.INVSPT_Amount,
                                              INVSPT_Remarks = a.INVSPT_Remarks,
                                              INVSPT_ActiveFlg = a.INVSPT_ActiveFlg
                                          }).Distinct().OrderBy(s => s.INVSPT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Vendor Payment load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_VendorPaymentDTO> getGRNdetail(INV_VendorPaymentDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_VendorGRN";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.INVMS_Id
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

                        data.get_SuplierGRNno = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.get_GRNpayment = (from a in _INVContext.INV_Supplier_PaymentDMO
                                       from b in _INVContext.INV_Supplier_Payment_GRNDMO
                                       from c in _INVContext.INV_Master_SupplierDMO
                                       from d in _INVContext.INV_M_GRNDMO
                                       where (a.INVSPT_Id == b.INVSPT_Id && b.INVSPTGRN_ActiveFlg == true && a.INVMS_Id == c.INVMS_Id && a.MI_Id == c.MI_Id && a.INVMS_Id == d.INVMS_Id && b.INVMGRN_Id == d.INVMGRN_Id && a.INVSPT_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMS_Id == data.INVMS_Id && b.INVMGRN_Id == data.INVMGRN_Id)
                                       select new INV_VendorPaymentDTO
                                       {
                                           INVSPT_Id = a.INVSPT_Id,
                                           INVSPTGRN_Id = b.INVSPTGRN_Id,
                                           INVMGRN_Id = b.INVMGRN_Id,
                                           INVSPT_ModeOfPayment = a.INVSPT_ModeOfPayment,
                                           INVSPT_PaymentDate = a.INVSPT_PaymentDate,
                                           INVMGRN_GRNNo = d.INVMGRN_GRNNo,
                                           INVMGRN_PurchaseValue = d.INVMGRN_PurchaseValue,
                                           INVSPTGRN_Amount = b.INVSPTGRN_Amount,
                                           INVMGRN_TotalPaid = d.INVMGRN_TotalPaid,
                                           INVMGRN_TotalBalance = d.INVMGRN_TotalBalance
                                       }).Distinct().OrderBy(p => p.INVSPT_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Ventor Payment Get Grn no.:" + ex.Message);
            }
            return data;
        }

        public INV_VendorPaymentDTO savedetails(INV_VendorPaymentDTO data)
        {
            try
            {
                if (data.INVSPT_Id != 0)
                {

                    var result = _INVContext.INV_Supplier_PaymentDMO.Single(t => t.INVSPT_Id == data.INVSPT_Id);
                    result.MI_Id = data.MI_Id;
                    result.INVMS_Id = data.INVMS_Id;
                    result.INVSPT_PaymentDate = data.INVSPT_PaymentDate;
                    result.INVSPT_ModeOfPayment = data.INVSPT_ModeOfPayment;
                    result.INVSPT_PaymentReference = data.INVSPT_PaymentReference;
                    result.INVSPT_ChequeDDNo = data.INVSPT_ChequeDDNo;
                    result.INVSPT_BankName = data.INVSPT_BankName;
                    result.INVSPT_ChequeDDDate = data.INVSPT_ChequeDDDate;
                    result.INVSPT_Amount = data.INVSPT_Amount;
                    result.INVSPT_Remarks = data.INVSPT_Remarks;
                    result.INVSPT_CreatedBy = data.UserId;
                    result.INVSPT_UpdatedBy = data.UserId;
                    result.INVSPT_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);

                    foreach (var pu in data.paymentArray)
                    {
                        var res1 = _INVContext.INV_Supplier_Payment_GRNDMO.Where(a => a.INVSPTGRN_Id == pu.INVSPTGRN_Id).ToList();
                        if (res1.Count > 0)
                        {
                            var res11 = _INVContext.INV_Supplier_Payment_GRNDMO.Single(a => a.INVSPTGRN_Id == pu.INVSPTGRN_Id);
                            res11.INVMGRN_Id = pu.INVMGRN_Id;
                            res11.INVSPTGRN_Amount = pu.INVSPTGRN_Amount;
                            res11.INVSPTGRN_Remarks = pu.INVSPTGRN_Remarks;
                            res11.INVSPTGRN_UpdatedBy = data.UserId;
                            res11.UpdatedDate = DateTime.Now;
                            _INVContext.Update(res11);
                        }
                        else
                        {
                            INV_Supplier_Payment_GRNDMO pymu = new INV_Supplier_Payment_GRNDMO();
                            pymu.INVSPT_Id = result.INVSPT_Id;
                            pymu.INVMGRN_Id = pu.INVMGRN_Id;
                            pymu.INVSPTGRN_Amount = pu.INVSPTGRN_Amount;
                            pymu.INVSPTGRN_Remarks = pu.INVSPTGRN_Remarks;
                            pymu.INVSPTGRN_CreatedBy = data.UserId;
                            pymu.INVSPTGRN_UpdatedBy = data.UserId;
                            pymu.CreatedDate = DateTime.Now;
                            pymu.UpdatedDate = DateTime.Now;
                            _INVContext.Add(pymu);
                        }
                        var grnupdate = _INVContext.INV_M_GRNDMO.Single(g => g.MI_Id == data.MI_Id && g.INVMGRN_Id == pu.INVMGRN_Id);
                        grnupdate.INVMGRN_TotalPaid = pu.INVMGRN_TotalPaid;
                        grnupdate.INVMGRN_TotalBalance = pu.INVMGRN_TotalBalance;
                        grnupdate.UpdatedDate = DateTime.Now;
                        _INVContext.Update(grnupdate);
                    }
                    var contactExists = _INVContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    INV_Supplier_PaymentDMO pay = new INV_Supplier_PaymentDMO();
                    pay.MI_Id = data.MI_Id;
                    pay.INVMS_Id = data.INVMS_Id;
                    pay.INVSPT_PaymentDate = data.INVSPT_PaymentDate;
                    pay.INVSPT_ModeOfPayment = data.INVSPT_ModeOfPayment;
                    pay.INVSPT_PaymentReference = data.INVSPT_PaymentReference;

                    pay.INVSPT_ChequeDDNo = data.INVSPT_ChequeDDNo;
                    pay.INVSPT_BankName = data.INVSPT_BankName;
                    pay.INVSPT_ChequeDDDate = data.INVSPT_ChequeDDDate;

                    pay.INVSPT_Amount = data.INVSPT_Amount;
                    pay.INVSPT_Remarks = data.INVSPT_Remarks;
                    pay.INVSPT_CreatedBy = data.UserId;
                    pay.INVSPT_UpdatedBy = data.UserId;
                    pay.INVSPT_ActiveFlg = true;
                    pay.CreatedDate = DateTime.Now;
                    pay.UpdatedDate = DateTime.Now;
                    _INVContext.Add(pay);

                    foreach (var p in data.paymentArray)
                    {
                        INV_Supplier_Payment_GRNDMO paygrn = new INV_Supplier_Payment_GRNDMO();
                        paygrn.INVSPT_Id = pay.INVSPT_Id;
                        paygrn.INVMGRN_Id = p.INVMGRN_Id;
                        paygrn.INVSPTGRN_Amount = p.INVSPTGRN_Amount;
                        paygrn.INVSPTGRN_Remarks = p.INVSPTGRN_Remarks;
                        paygrn.INVSPTGRN_ActiveFlg = true;
                        paygrn.INVSPTGRN_CreatedBy = data.UserId;
                        paygrn.INVSPTGRN_UpdatedBy = data.UserId;
                        paygrn.CreatedDate = DateTime.Now;
                        paygrn.UpdatedDate = DateTime.Now;
                        _INVContext.Add(paygrn);

                        var grnupdate = _INVContext.INV_M_GRNDMO.Single(g => g.MI_Id == data.MI_Id && g.INVMGRN_Id == p.INVMGRN_Id);
                        grnupdate.INVMGRN_TotalPaid = p.INVMGRN_TotalPaid;
                        grnupdate.INVMGRN_TotalBalance = p.INVMGRN_TotalBalance;
                        grnupdate.UpdatedDate = DateTime.Now;
                        _INVContext.Update(grnupdate);
                    }


                    var contactExists = _INVContext.SaveChanges();
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
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Vendor Payment savedata :" + ex.Message);
            }
            return data;
        }

        public INV_VendorPaymentDTO deactive(INV_VendorPaymentDTO data)
        {
            try
            {
                //var result = _INVContext.INV_Supplier_PaymentDMO.Single(t => t.INVSPT_Id == data.INVSPT_Id && t.MI_Id == data.MI_Id);

                //if (result.INVSPT_ActiveFlg == true)
                //{
                //    result.INVSPT_ActiveFlg = false;
                //}
                //else if (result.INVSPT_ActiveFlg == false)
                //{
                //    result.INVSPT_ActiveFlg = true;
                //}
                //var resultt = _INVContext.INV_Supplier_Payment_GRNDMO.Where(t => t.INVSPT_Id == data.INVSPT_Id).ToList();
                //foreach (var r in resultt)
                //{
                //    if (result.INVSPT_ActiveFlg == true)
                //    {
                //        r.INVSPTGRN_ActiveFlg = true;
                //    }
                //    else if (result.INVSPT_ActiveFlg == false)
                //    {
                //        r.INVSPTGRN_ActiveFlg = false;
                //    }
                //    r.UpdatedDate = DateTime.Now;
                //    _INVContext.Update(r);
                //}
                //result.UpdatedDate = DateTime.Now;
                //_INVContext.Update(result);
                //int returnval = _INVContext.SaveChanges();
                //if (returnval > 0)
                //{
                //    try
                //    {
                //        if (contextupdate > 0)
                //        {
                //            data.returnduplicatestatus = "Updated";
                //        }
                //        else
                //        {
                //            data.returnduplicatestatus = "notUpdated";
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public INV_VendorPaymentDTO deactiveGRN(INV_VendorPaymentDTO data)
        {
            try
            {
                //var result = _INVContext.INV_T_SalesDMO.Single(t => t.INVTSL_Id == data.INVTSL_Id);

                //if (result.INVTSL_ActiveFlg == true)
                //{
                //    result.INVTSL_ActiveFlg = false;
                //}
                //else if (result.INVTSL_ActiveFlg == false)
                //{
                //    result.INVTSL_ActiveFlg = true;
                //}

                //var resulttx = _INVContext.INV_T_Sales_TaxDMO.Where(t => t.INVTSL_Id == data.INVTSL_Id).ToList();
                //foreach (var tx in resulttx)
                //{
                //    if (result.INVTSL_ActiveFlg == true)
                //    {
                //        tx.INVTSLT_ActiveFlg = true;
                //    }
                //    else if (result.INVTSL_ActiveFlg == false)
                //    {
                //        tx.INVTSLT_ActiveFlg = false;
                //    }

                //    tx.UpdatedDate = DateTime.Now;
                //    _INVContext.Update(tx);
                //}

                //result.UpdatedDate = DateTime.Now;
                //_INVContext.Update(result);
                //int returnval = _INVContext.SaveChanges();
                //if (returnval > 0)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public INV_VendorPaymentDTO getmodeldetail(INV_VendorPaymentDTO data)
        {
            data.get_modeldetails = (from a in _INVContext.INV_Supplier_PaymentDMO
                                     from b in _INVContext.INV_Supplier_Payment_GRNDMO
                                     from c in _INVContext.INV_M_GRNDMO
                                     from d in _INVContext.INV_Master_SupplierDMO
                                     where (a.INVSPT_Id == b.INVSPT_Id && a.INVMS_Id == c.INVMS_Id && b.INVMGRN_Id == c.INVMGRN_Id && a.INVMS_Id == d.INVMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.INVSPT_Id == data.INVSPT_Id)
                                     select new INV_VendorPaymentDTO
                                     {
                                         INVSPT_Id = a.INVSPT_Id,
                                         INVSPTGRN_Id = b.INVSPTGRN_Id,
                                         INVMGRN_Id = b.INVMGRN_Id,
                                         INVMGRN_GRNNo = c.INVMGRN_GRNNo,
                                         INVMGRN_PurchaseValue = c.INVMGRN_PurchaseValue,
                                         INVMGRN_TotalPaid = c.INVMGRN_TotalPaid,
                                         INVMGRN_TotalBalance = c.INVMGRN_TotalBalance,
                                         INVSPTGRN_Amount = b.INVSPTGRN_Amount,
                                         INVSPTGRN_Remarks = b.INVSPTGRN_Remarks,
                                         INVSPTGRN_ActiveFlg = b.INVSPTGRN_ActiveFlg
                                     }).Distinct().OrderBy(s => s.INVMGRN_Id).ToArray();
            return data;
        }


        public INV_VendorPaymentDTO supplerpaymentreminder(INV_VendorPaymentDTO data)
        {
            try
            {
                decimal Atotal = 0;
                decimal Ptotal = 0;
                decimal Btotal = 0;
                string str = "";
                string mail = "";

                List<INV_VendorPaymentDTO> emaillists = new List<INV_VendorPaymentDTO>();
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SUPPLIERSUBSCRIPTIONEMAILIDS";
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
                                emaillists.Add(new INV_VendorPaymentDTO
                                {
                                    INVMS_EmailId = (dataReader["INVMS_EmailId"].ToString()),

                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (emaillists.Count>0)
                {
                    if (emaillists.FirstOrDefault().INVMS_EmailId!=null && emaillists.FirstOrDefault().INVMS_EmailId!="")
                    {

                   
                    string[] sndmailids = emaillists.FirstOrDefault().INVMS_EmailId.Split(',');

                    foreach (var eml in sndmailids)
                    {

                            mail = eml;

                          var template = _INVContext.SMSEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "SUPPAYMENTREMINDER" && a.ISES_MailActiveFlag == true && a.ISES_AlertBeforeDays > 0).ToList();
                if (template.Count == 0)
                {
                    data.message = "Email Template not Mapped to the selected Institution";
                }
                else
                {

                    data.ISES_Id = template.FirstOrDefault().ISES_Id;
                    List<INV_VendorPaymentDTO> itemlistt = new List<INV_VendorPaymentDTO>();
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_PENDING_SUPPLIER_NOTIFY";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Days",
                        SqlDbType.VarChar)
                        {
                            Value = template.FirstOrDefault().ISES_AlertBeforeDays
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
                                    itemlistt.Add(new INV_VendorPaymentDTO
                                    {
                                        INVSPT_PaymentDate = Convert.ToDateTime(dataReader["INVMGRN_PurchaseDate"].ToString()),
                                        INVMS_SupplierName = dataReader["INVMS_SupplierName"].ToString(),
                                        INVMS_SupplierCode = dataReader["INVMS_SupplierCode"].ToString(),
                                        INVMS_SupplierConatctPerson = dataReader["INVMS_SupplierConatctPerson"].ToString(),
                                        INVMS_SupplierConatctNo = dataReader["INVMS_SupplierConatctNo"].ToString(),
                                        INVMS_EmailId = dataReader["INVMS_EmailId"].ToString(),
                                        INVMGRN_GRNNo = dataReader["INVMGRN_GRNNo"].ToString(),
                                        INVMGRN_PurchaseValue = Convert.ToDecimal(dataReader["INVMGRN_PurchaseValue"].ToString()),
                                        INVMGRN_TotalPaid = Convert.ToDecimal(dataReader["INVMGRN_TotalPaid"].ToString()),
                                        INVMGRN_TotalBalance = Convert.ToDecimal(dataReader["INVMGRN_TotalBalance"].ToString()),

                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    if (itemlistt.Count > 0)
                    {



                        str += "<table width='100%' border=" + "'1'" + "    id=" + "'tablepaging'" + " class=" + "'table table-hover table-responsive table-bordered'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                       "<th style=" + "width:20px'" + "> SL.NO. </th>";

                        str += "<th>" + "PURCHASE DATE" + "</th>";
                        str += "<th>" + "SUPPLIER NAME" + "</th>";
                        str += "<th>" + "CONTACT PERSON" + "</th>";
                        str += "<th>" + "MOBILE NO" + "</th>";
                        str += "<th>" + "EMAIL" + "</th>";
                        str += "<th>" + "GRN NO." + "</th>";
                        str += "<th>" + "TOTAL AMOUNT" + "</th>";
                        str += "<th>" + "PAID AMOUNT" + "</th>";
                        str += "<th>" + "BALANCE" + "</th>";


                        str += "</tr>";




                        int rownumber = 0;
                        for (int j = 0; j < itemlistt.Count; j++)
                        {
                            rownumber += 1;


                            Atotal += itemlistt[j].INVMGRN_PurchaseValue;
                            Ptotal += itemlistt[j].INVMGRN_TotalPaid;
                            Btotal += itemlistt[j].INVMGRN_TotalBalance;


                            str += "<tr>" +
                          "<td" + " >" + rownumber + "</td> ";

                            str += "<td >" + itemlistt[j].INVSPT_PaymentDate + "</td>";
                            str += "<td>" + itemlistt[j].INVMS_SupplierName + "</th>";
                            str += "<td>" + itemlistt[j].INVMS_SupplierConatctPerson + "</th>";
                            str += "<td  >" + itemlistt[j].INVMS_SupplierConatctNo + "</th>";
                            str += "<td  >" + itemlistt[j].INVMS_EmailId + "</th>";
                            str += "<td  >" + itemlistt[j].INVMGRN_GRNNo + "</th>";
                            str += "<td  >" + itemlistt[j].INVMGRN_PurchaseValue + "</th>";
                            str += "<td  >" + itemlistt[j].INVMGRN_TotalPaid + "</th>";
                            str += "<td  >" + itemlistt[j].INVMGRN_TotalBalance + "</th>";



                            str += "</tr>"; ;


                        }


                        str += "<tr>" +
                               "<td colspan =" + "" + 7 + ">" + "TOTAL" + "</td> ";

                        str += "<td  >" + Atotal + "</th>";
                        str += "<td  >" + Ptotal + "</th>";
                        str += "<td  >" + Btotal + "</th>";
                        str += "</tr>"; ;
                        str += "</table>";


                        Dictionary<string, string> val = new Dictionary<string, string>();


                        var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                        var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                        string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                        string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                        string Resultsms = Mailcontent;
                        string result = Mailmsg;



                        List<Match> variables = new List<Match>();

                        foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                        {
                            variables.Add(match);
                        }
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {

                            cmd.CommandText = "INVPOSMSMAILPARAMETER";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@Template",
                              SqlDbType.VarChar)
                            {
                                Value = template.FirstOrDefault().ISES_Template_Name
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISES_Id",
                              SqlDbType.VarChar)
                            {
                                Value = data.ISES_Id
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

                            }

                        }
                        for (int j = 0; j < ParamaetersName.Count; j++)
                        {
                            for (int p = 0; p < val.Count; p++)
                            {
                                if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                                {
                                    result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                    Mailmsg = result;
                                }
                            }
                        }
                        Mailmsg = result;
                        for (int j = 0; j < ParamaetersName.Count; j++)
                        {
                            for (int p = 0; p < val.Count; p++)
                            {
                                if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                                {
                                    Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                    Mailcontent = Resultsms;
                                }
                            }
                        }
                        Mailcontent = Resultsms;

                        Mailcontent = Mailcontent = Mailcontent.Replace("[ITEMS]", str);

                        data.mailmsg = Mailcontent;


                        var institutionName = _INVContext.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();


                        Mailmsg = Mailcontent;




                        //  Mailcontent = Resultsms;



                        List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                        alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                        string Attechement = "";
                        if (alldetails.Count > 0)
                        {
                            string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                            string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                            string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                            Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                            string Subject = template[0].ISES_MailSubject.ToString();
                            string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                            List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                            smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();





                            if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                            {

                                var message = new SendGridMessage();
                                if (template.FirstOrDefault().ISES_EnableMailBCCFlg == true)
                                {

                                    if (template.FirstOrDefault().ISES_MailBCCId != null && template.FirstOrDefault().ISES_MailBCCId != "")
                                    {
                                        string[] bccmail = template.FirstOrDefault().ISES_MailBCCId.Split(',');

                                        foreach (var item in bccmail)
                                        {
                                            message.AddBcc(item);
                                        }

                                    }


                                }

                                if (template.FirstOrDefault().ISES_EnableMailCCFlg == true)
                                {

                                    if (template.FirstOrDefault().ISES_MailCCId != null && template.FirstOrDefault().ISES_MailCCId != "")
                                    {
                                        string[] ccmail = template.FirstOrDefault().ISES_MailCCId.Split(',');

                                        foreach (var item in ccmail)
                                        {
                                            message.AddCc(item);
                                        }

                                    }


                                }
                                if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                                {
                                    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                                }


                                message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                                message.Subject = Subject;
                                message.AddTo(mail);

                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {
                                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                            Stream stream = response.GetResponseStream();
                                            message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                        }
                                    }
                                }




                                message.HtmlContent = Mailmsg;
                                var client = new SendGridClient(sengridkey);

                                client.SendEmailAsync(message).Wait();
                            }
                            else
                            {

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
                                        emailMessage.To.Add(new MailAddress(mail));
                                        emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                        emailMessage.Subject = Subject;
                                        emailMessage.Body = Mailmsg;
                                        emailMessage.IsBodyHtml = true;
                                        if (template.FirstOrDefault().ISES_EnableMailBCCFlg == true)
                                        {

                                            if (template.FirstOrDefault().ISES_MailBCCId != null && template.FirstOrDefault().ISES_MailBCCId != "")
                                            {
                                                string[] bccmail = template.FirstOrDefault().ISES_MailBCCId.Split(',');

                                                foreach (var item in bccmail)
                                                {
                                                    emailMessage.Bcc.Add(item);
                                                }

                                            }


                                        }

                                        if (template.FirstOrDefault().ISES_EnableMailCCFlg == true)
                                        {

                                            if (template.FirstOrDefault().ISES_MailCCId != null && template.FirstOrDefault().ISES_MailCCId != "")
                                            {
                                                string[] ccmail = template.FirstOrDefault().ISES_MailCCId.Split(',');

                                                foreach (var item in ccmail)
                                                {
                                                    emailMessage.CC.Add(item);
                                                }

                                            }


                                        }


                                        clientsmtp.Send(emailMessage);
                                    }
                                }

                            }



                            using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                            {
                                var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Id == data.ISES_Id).Select(d => d.IVRMIM_Id).ToList();

                                var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                                var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                //if (modulename.Count == 0)
                                //{
                                //    modulename[0] = "Issue Manager";
                                //}

                                cmd.CommandText = "IVRM_Email_Outgoing";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@EmailId",
                                    SqlDbType.NVarChar)
                                {
                                    Value = mail
                                });
                                cmd.Parameters.Add(new SqlParameter("@Message",
                                   SqlDbType.NVarChar)
                                {
                                    Value = Mailcontent
                                });
                                cmd.Parameters.Add(new SqlParameter("@module",
                                SqlDbType.VarChar)
                                {
                                    Value = modulename[0]
                                });
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                               SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
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

                                }
                            }
                        }


                    }
                }

            }
            }
            }
            }
            catch (Exception ex )
            {

                throw ex ;
            }


            return data;

        }

    }
}
