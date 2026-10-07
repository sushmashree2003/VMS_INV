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
using System.Net.Mail;
using System.Net;
using System.IO;
using DomainModel.Model;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_PO_ToSupplierImpl : Interface.INV_PO_ToSupplierInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PO_ToSupplierImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        public INV_PO_ToSupplierImpl(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, InventoryContext InvContext, ILogger<INV_PO_ToSupplierImpl> log, DomainModelMsSqlServerContext db)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_PO_ToSupplierDTO getloaddata(INV_PO_ToSupplierDTO data)
        {
            try
            {
                data.get_piNo = _INVContext.INV_M_PurchaseOrderDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMPO_ActiveFlg == true && m.INVMPO_FinalProcessFlag==true &&(m.INVMPO_RejectFlg==null|| m.INVMPO_RejectFlg==false)).OrderByDescending(m => m.INVMPO_Id).ToArray();

                
                data.templatelist = (from a in _INVContext.SMSEmailSetting
                                     from b in _INVContext.Institution_Module_Page
                                     from c in _INVContext.MasterPage
                                     where a.MI_Id == data.MI_Id && a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMP_Id == c.IVRMP_Id && c.IVRMP_PageURL == "app.INV_PurchaseOrder" && a.ISES_MailActiveFlag == true
                                     select new INV_PO_ToSupplierDTO
                                     {
                                         ISES_Id = a.ISES_Id,
                                         ISES_Template_Name = a.ISES_Template_Name,
                                     }).Distinct().ToArray();

                //*********************************** User Department List ********************************app.INV_PO_ToSupplier// 
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_PO_EMAIL_DETAILS";
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
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.gridlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI TO Supplier load Page:" + ex.Message);
            }
            return data;
        }

        public INV_PO_ToSupplierDTO getpiDetail(INV_PO_ToSupplierDTO data)
        {
            try
            {
                data.get_pidetails = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                      from b in _INVContext.INV_T_PurchaseOrderDMO
                                      from c in _INVContext.INV_Master_ItemDMO
                                      from d in _INVContext.INV_Master_UOMDMO
                                      where (a.INVMPO_Id == b.INVMPO_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                               && a.INVMPO_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPO_Id == data.INVMPO_Id && b.INVTPO_ActiveFlg==true && a.INVMPO_FinalProcessFlag == true && (a.INVMPO_RejectFlg == null || a.INVMPO_RejectFlg == false) && (b.INVTPO_RejectFlg==false || b.INVTPO_RejectFlg == null))
                                      select new INV_PurchaseOrderDTO
                                      {
                                          INVMPO_Id = a.INVMPO_Id,
                                          INVMPO_PONo = a.INVMPO_PONo,
                                          INVMI_Id = b.INVMI_Id,
                                          INVMUOM_Id = b.INVMUOM_Id,
                                          INVMPI_Id = b.INVMPI_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                                          INVMPO_PODate = a.INVMPO_PODate,
                                          INVMPO_ReferenceNo = a.INVMPO_ReferenceNo,
                                          INVTPO_POQty = Convert.ToDecimal(b.INVTPO_ApproveQty),
                                          INVTPO_Amount = b.INVTPO_Amount,
                                          INVTPO_RatePerUnit = b.INVTPO_RatePerUnit,
                                          INVTPO_TaxAmount = b.INVTPO_TaxAmount,
                                          INVMPO_Remarks = a.INVMPO_Remarks,
                                          INVMPO_TotAmount = a.INVMPO_TotAmount,
                                          INVMPO_ActiveFlg = a.INVMPO_ActiveFlg,
                                          INVTPO_Remarks = b.INVTPO_Remarks,
                                          INVMPO_TotRate = a.INVMPO_TotRate,
                                          INVMPO_TotTax = a.INVMPO_TotTax,
                                          INVMPO_POTemplate = a.INVMPO_POTemplate,
                                        
                                      }).Distinct().OrderBy(i => i.INVMPO_Id).ToArray();




                data.get_supplier = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                     from b in _INVContext.INV_Master_SupplierDMO
                                     where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMS_Id == b.INVMS_Id && a.INVMPO_Id == data.INVMPO_Id
                                     select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Pi To Supplier Pi change:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_PO_ToSupplierDTO> savedetails(INV_PO_ToSupplierDTO data)
        {
            //try
            //{
            //    string s = "";
            //    string m = "";

            //    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
               
            //    foreach (var sp in data.supplierArray)
            //    {
            //        INV_PurchaseIndent_ToSupplierDMO PITOSP = new INV_PurchaseIndent_ToSupplierDMO();
            //        PITOSP.MI_Id = data.MI_Id;
            //        PITOSP.INVMPO_Id = data.INVMPO_Id;
            //        PITOSP.INVPITS_SupplierName = sp.INVPITS_SupplierName;
            //        PITOSP.INVPITS_ContactNo = sp.INVPITS_ContactNo;
            //        PITOSP.INVPITS_EmailId = sp.INVPITS_EmailId;
            //        PITOSP.INVPITS_SMSSentDate = indianTime;
            //        PITOSP.INVPITS_MailSentDate = indianTime;
            //        PITOSP.INVPITS_ActiveFlg = true;
            //        PITOSP.INVPITS_CreatedBy = data.UserId;
            //        PITOSP.INVPITS_UpdatedBy = data.UserId;
            //        PITOSP.CreatedDate = indianTime;
            //        PITOSP.UpdatedDate = indianTime;
            //        _INVContext.Add(PITOSP);
            //        if (sp.INVPITS_ContactNo != 0)
            //        {
            //            SMS sms = new SMS(_db);
            //            s = await sms.sendPItoSupplierSms(data.MI_Id, sp.INVPITS_ContactNo, "PINotification", data.UserId, sp.INVMS_Id, data.INVMPO_Id);
            //        }
            //        if (sp.INVPITS_EmailId != "")
            //        {
            //            Email Email = new Email(_db);
            //            m = Email.sendPItoSupplierEmail(data.MI_Id, sp.INVPITS_EmailId, "PINotification", data.UserId, sp.INVMS_Id, data.INVMPO_Id);
            //        }
            //    }

            //    var contactExists = _INVContext.SaveChanges();
            //    if (contactExists > 0)
            //    {
            //        if (s == "success" && m == "success")
            //        {
            //            data.returnval = true;
            //        }
            //        else if (s == "success")
            //        {
            //            data.message = "SMS";
            //            data.returnval = true;
            //        }
            //        else if (m == "success")
            //        {
            //            data.message = "Email";
            //            data.returnval = true;
            //        }
            //    }
            //    else
            //    {
            //        data.returnval = false;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    data.message = "Error";
            //    _logInv.LogInformation("PITOSP savedata :" + ex.Message);
            //}

            return data;
        }

        public INV_PO_ToSupplierDTO deactive(INV_PO_ToSupplierDTO data)
        {

            return data;
        }

        public INV_PO_ToSupplierDTO viewtemplatedetails(INV_PO_ToSupplierDTO data)
        {
            try
            {
                decimal total = 0;
                string str = "";
                var checktemplate = _INVContext.SMSEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Id == data.ISES_Id && a.ISES_MailActiveFlag == true).ToList();
                if (checktemplate.Count == 0)
                {
                    data.message = "Email Template not Mapped to the selected Institution";
                }
                else
                {
                   var itemdetails = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                          from b in _INVContext.INV_T_PurchaseOrderDMO
                                          from c in _INVContext.INV_Master_ItemDMO
                                          from d in _INVContext.INV_Master_UOMDMO
                                          where (a.INVMPO_Id == b.INVMPO_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                                   && a.INVMPO_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPO_Id == data.INVMPO_Id && b.INVTPO_ActiveFlg == true && a.INVMPO_FinalProcessFlag == true && (a.INVMPO_RejectFlg == null || a.INVMPO_RejectFlg == false) && (b.INVTPO_RejectFlg == false || b.INVTPO_RejectFlg == null))
                                          select new INV_PurchaseOrderDTO
                                          {
                                              INVMPO_Id = a.INVMPO_Id,
                                              INVMPO_PONo = a.INVMPO_PONo,
                                              INVMI_Id = b.INVMI_Id,
                                              INVMUOM_Id = b.INVMUOM_Id,
                                              INVMPI_Id = b.INVMPI_Id,
                                              INVMI_ItemName = c.INVMI_ItemName,
                                              INVMUOM_UOMName = d.INVMUOM_UOMName,
                                              INVMPO_PODate = a.INVMPO_PODate,
                                              INVMPO_ReferenceNo = a.INVMPO_ReferenceNo,
                                              INVTPO_POQty = Convert.ToDecimal(b.INVTPO_ApproveQty),
                                              INVTPO_Amount = b.INVTPO_Amount,
                                              INVTPO_RatePerUnit = b.INVTPO_RatePerUnit,
                                              INVTPO_TaxAmount = b.INVTPO_TaxAmount,
                                              INVMPO_Remarks = a.INVMPO_Remarks,
                                              INVMPO_TotAmount = a.INVMPO_TotAmount,
                                              INVMPO_ActiveFlg = a.INVMPO_ActiveFlg,
                                              INVTPO_Remarks = b.INVTPO_Remarks,
                                              INVMPO_TotRate = a.INVMPO_TotRate,
                                              INVMPO_TotTax = a.INVMPO_TotTax,
                                          }).Distinct().OrderBy(i => i.INVMI_ItemName).ToList();
                    


                    if (itemdetails.Count>0)
                    {
                        

                        if (data.columnids !=null)
                        {
                            str += "<table width='90%' border=" + "'1'" + "    id=" + "'tablepaging'" + " class=" + "'table table-hover table-responsive table-bordered'" + " style=" + "'border-collapse: collapse; width: 90%;'" + "><tr>"+
                           "<th style=" + "width:20px'" + "> SL.NO. </th>";
                            foreach (var item in data.columnids)
                            {
                                if (item.coulmn== "ITEMNAME")
                                {
                                    str += "<th>" + item.title+ "</th>";
                                }

                                if (item.coulmn == "QUANTITY")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "PRICE/UNIT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "AMOUNT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "TAX")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }
                                if (item.coulmn == "TOTALAMOUNT")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }

                                if (item.coulmn == "REMARKS")
                                {
                                    str += "<th>" + item.title + "</th>";
                                }
                            }
                            
                            str += "</tr>";
                        }



                        int rownumber = 0;
                        for (int j = 0; j < itemdetails.Count; j++)
                        {
                            rownumber += 1;


                            total += itemdetails[j].INVTPO_Amount;
                            if (data.columnids != null)
                            {

                                str += "<tr>" +
                              "<td" + " >"  + rownumber + "</td> ";

                                foreach (var item1 in data.columnids)
                                {
                                    
                                        if (item1.coulmn == "ITEMNAME")
                                        {
                                            str += "<td >" + itemdetails[j].INVMI_ItemName + "</td>";
                                        }

                                        if (item1.coulmn == "QUANTITY")
                                        {
                                            str += "<td>" + itemdetails[j].INVTPO_POQty + "</th>";
                                        }

                                        if (item1.coulmn == "PRICE/UNIT")
                                        {
                                            str += "<td>" + itemdetails[j].INVTPO_RatePerUnit + "</th>";
                                        }

                                        if (item1.coulmn == "AMOUNT")
                                        {
                                            str += "<td  >" + itemdetails[j].INVTPO_POQty * itemdetails[j].INVTPO_RatePerUnit + "</th>";
                                        }

                                        if (item1.coulmn == "TAX")
                                        {
                                            str += "<td  >" + itemdetails[j].INVTPO_TaxAmount + "</th>";
                                        }
                                        if (item1.coulmn == "TOTALAMOUNT")
                                        {
                                            str += "<td  >" + itemdetails[j].INVTPO_Amount + "</th>";
                                        }

                                        if (item1.coulmn == "REMARKS")
                                        {
                                            str += "<td  >" + itemdetails[j].INVTPO_Remarks + "</th>";
                                        }
                                    }

                                str += "</tr>"; ;
                            }

                            }


                        str += "<tr>" +
                               "<td colspan =" + ""+ data.columnids.Length+ ">" + "TOTAL" + "</td> ";

                        str += "<td  >" + total + "</th>";
                        str += "</tr>"; ;
                        str += "</table>";


                        Dictionary<string, string> val = new Dictionary<string, string>();
                       

                        var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == checktemplate[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                        var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                        string Mailcontent = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;
                        string Mailmsg = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                        string Resultsms = Mailcontent;
                        string result = Mailmsg;


                        Mailcontent=Mailcontent.Replace("[SUBJECT]", checktemplate.FirstOrDefault().ISES_MailSubject);
                        
                    

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
                            cmd.Parameters.Add(new SqlParameter("@INVMPO_Id",
                               SqlDbType.VarChar)
                            {
                                Value = data.INVMPO_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Template",
                              SqlDbType.VarChar)
                            {
                                Value = checktemplate.FirstOrDefault().ISES_Template_Name
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

                    }
                    }
                

                }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }


        public async Task<INV_PO_ToSupplierDTO> sendmail(INV_PO_ToSupplierDTO data) {
            try
            {
                 TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                 DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string filename = "";
                var list = _INVContext.INV_M_PurchaseOrderDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMPO_Id == data.INVMPO_Id).ToList();
                if (list.Count>0)
                {
                    filename = list.FirstOrDefault().INVMPO_PONo.Trim() + ".Pdf";
                }
                //string s1 = "<!DOCTYPE html>< html >< body >";
               // string s2 = " </body></ html >";
                data.mailmsg =  data.mailmsg;

                string filepath= await generatepdf(list.FirstOrDefault().INVMPO_PONo.Trim(), data.MI_Id, data.mailmsg);

               
                 string s = sendPOmail(data.MI_Id, data.INVPITS_EmailId, data.mailmsg, data.ISES_Id,data.attachmentflag,filepath, list.FirstOrDefault().INVMPO_PONo.Trim());

                if (s == "success")
                {
                    INV_M_PurchaseOrder_ToSupplierDMO obj = new INV_M_PurchaseOrder_ToSupplierDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.INVMPO_Id = data.INVMPO_Id;
                    obj.INVTPOSUP_SupplierName = data.INVTPOSUP_SupplierName;
                    obj.INVTPOSUP_MailSentDate = indianTime;
                    obj.INVTPOSUP_EmailId = data.INVPITS_EmailId;
                    obj.INVTPOSUP_ContactNo = data.INVTPOSUP_ContactNo;
                    obj.INVTPOSUP_MailAttatchment = filepath;
                    obj.CreatedDate = indianTime;
                    obj.UpdatedDate = indianTime;
                    obj.INVTPOSUP_DateTime = indianTime;
                    obj.INVTPOSUP_CreatedBy = data.UserId;
                    obj.INVTPOSUP_UpdatedBy = data.UserId;
                    obj.INVTPOSUP_ActiveFlg = true;

                    _INVContext.Add(obj);

                    int cx = _INVContext.SaveChanges();
                    if (cx > 0)
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
                    data.returnval = false;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public async Task<string> generatepdf(String str, long mid, string message)
        {

            Regex reg = new Regex("[*'\",_&#^@]");
            str = reg.Replace(str, string.Empty);


        str = str.Replace("/", "-" );
            // string message = str;

            var myfilename = string.Format(@"{0}", Guid.NewGuid());
           // str = "abcd";
            var webRootPath = _hostingEnvironment.WebRootPath;
            string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "POFiles") + $@"\{str}.pdf";


            StringBuilder sb = new StringBuilder(message);
            StringReader sr = new StringReader(sb.ToString());
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
            
            HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

             
                htmlparser.Parse(sr);
                pdfDoc.Close();

                byte[] bytess = memoryStream.ToArray();
                memoryStream.Close();

                var file = Convert.ToBase64String(bytess);
                string emp;
                emp = Convert.ToString(sr);
                string c = "";
                //string v = emp.Replace("System.IO.StringReader", filename);

                using (var imageFile = new FileStream(filepath, FileMode.Create))
                {
                    imageFile.Write(bytess, 0, bytess.Length);
                    imageFile.Flush();
                }


            }


           //var bytess = Convert.FromBase64String(message);
           // using (var imageFile = new FileStream(filepath, FileMode.Create))
           // {
           //     imageFile.Write(bytess, 0, bytess.Length);
           //     imageFile.Flush();
           // }
            string accountname = "";
            string accesskey = "";
            string newImageNamePath = "";
            string newImageName = "";
            var data = _INVContext.IVRM_Storage_path_Details.ToList();
            if (data.Count() > 0)
            {
                accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
            }

            string containername = "files";
            string folder = "POFiles";
            StorageCredentials cre = new StorageCredentials(accountname, accesskey);
            CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
            CloudBlobClient blobClient = acc.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containername);

            var ext = ".pdf";
            var guid = Guid.NewGuid().ToString();
            newImageName = guid + ext;
            string blobName = newImageName;
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
            await blockBlob.UploadFromFileAsync(filepath);
            newImageNamePath = blockBlob.Uri.ToString();

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            return newImageNamePath;
        }



        public string sendPOmail(long MI_Id, string mail, string message1, long ISES_Id, bool attachmentflag,string filepath, string filename)
        {
            try
            {
               
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Id == ISES_Id).ToList();
                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _INVContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                //string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = message1;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }



                Mailcontent = Resultsms;



                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
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
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();





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

                        if (attachmentflag==true)
                        {
                             var webClient = new WebClient();
                                byte[] imageBytes = webClient.DownloadData(filepath);
                                // byte[] bytes = File.ReadAllBytes(a);
                                string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                //message.AddAttachment(item.cfilepath, "", null, null, null);
                                message.AddAttachment(filepath, fileContentsAsBase64, null, null, null);
                          
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


                                if (attachmentflag == true)
                                {
                                    System.Net.Mail.Attachment attachment;
                                    attachment = new System.Net.Mail.Attachment(filepath);
                                    emailMessage.Attachments.Add(attachment);




                                    //StringBuilder sb = new StringBuilder(message1);
                                    //StringReader sr = new StringReader(sb.ToString());
                                    //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);



                                    //HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                                    //using (MemoryStream memoryStream = new MemoryStream())
                                    //{
                                    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                    //    pdfDoc.Open();

                                    //    StyleSheet styles = new StyleSheet();
                                    //    styles.LoadTagStyle(HtmlTags.TABLE, HtmlTags.FONT, "");


                                    //    htmlparser.Parse(sr);
                                    //    pdfDoc.Close();

                                    //    byte[] bytess = memoryStream.ToArray();
                                    //    memoryStream.Close();

                                    //    var file = Convert.ToBase64String(bytess);
                                    //    string emp;
                                    //    emp = Convert.ToString(sr);
                                    //    string c = "";
                                    //    string v = emp.Replace("System.IO.StringReader", filename);


                                    //    Attachment att = new Attachment(new MemoryStream(bytess), filename);

                                    //    // System.Net.Mail.Attachment attachment;
                                    //    // attachment = new System.Net.Mail.Attachment(file,v);
                                    //    // emailMessage.Attachments.Add(attachment);

                                    //    //emailMessage.AddAttachment.a(v, file);
                                    //    //emailMessage.HtmlContent = Mailmsg;

                                    //    emailMessage.Attachments.Add(att);


                                    //}

                                }

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



                                // clientsmtp.Send(emailMessage);
                            }
                        }

                    }

                    //============using API

                    //var message = new SendGridMessage();
                    //message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    //message.Subject = Subject;
                    //message.AddTo(mail);
                    //if (Attechement.Equals("1"))
                    //{
                    //    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                    //    if (img.Count > 0)
                    //    {
                    //        for (int i = 0; i < img.Count; i++)
                    //        {
                    //            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                    //            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                    //            Stream stream = response.GetResponseStream();
                    //            message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                    //        }
                    //    }
                    //}

                    //message.HtmlContent = Mailmsg;
                    //var client = new SendGridClient(sengridkey);
                    //client.SendEmailAsync(message).Wait();

                    //=====================using API complited
                    //==========smtp=
                
                    ////================smtp complited===========

                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Id == ISES_Id).Select(d => d.IVRMIM_Id).ToList();

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
                            return ex.Message;
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}
