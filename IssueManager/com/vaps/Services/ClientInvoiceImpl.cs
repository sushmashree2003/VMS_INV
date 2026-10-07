using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.IssueManager;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ClientInvoiceImpl : Interfaces.ClientInvoiceInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;
        public ClientInvoiceImpl(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;
            //
        }
        public ClientInvoiceDTO loaddata(ClientInvoiceDTO data)
        {
            try
            {
                data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "USP_Get_feedbackDetails";
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
                        data.getfeedbackdata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClientInvoiceDTO ClientData(ClientInvoiceDTO data)
        {
            try
            {
                data.MonthList = _IssueManagercontext.IVRM_Month_DMO.Distinct().ToArray();

                data.clientlist = _IssueManagercontext.clientTable.Where(a => a.ISMMCLT_ActiveFlag == true).Distinct().ToArray();
                data.AcademicList = _IssueManagercontext.AcademicYear.Where(a => a.ASMAY_ActiveFlag == 1 && a.MI_Id==data.MI_Id).Distinct().ToArray();
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "USP_IVRM_MODULE_LIST";
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
                        data.Modulelist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "USP_Get_feedbackDetailsFlag";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                    {
                        Value = "receiving"
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
                        data.getfeedbackdata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "USP_Get_feedbackDetailsFlag";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                    {
                        Value = "byus"
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
                        data.getfeedbackdatabyus = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClientInvoiceDTO GetIEName(ClientInvoiceDTO data)
        {
            try
            {
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "USP_GET_IE_NAMES";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ISMMCLT_Id
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
                        data.GetallIEnames = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClientInvoiceDTO Editclientfeedback(ClientInvoiceDTO data)
        {
            try
            {
                data.clientlist = _IssueManagercontext.clientTable.Where(a => a.ISMMCLT_ActiveFlag == true).Distinct().ToArray();
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "USP_Edit_feedbackDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ICCF_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.ICCF_ID
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
                        data.Editfeedbackdata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "USP_GET_IE_NAMES";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ISMMCLT_Id
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
                        data.GetallIEnames = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClientInvoiceDTO savefeedbackdetails(ClientInvoiceDTO data)
        {
            try
            {
                if (data.ICCF_ID > 0)
                {
                    var result = _IssueManagercontext.ISM_CallCenter_CLientFeedbackDMO.Single(a => a.ICCF_ID == data.ICCF_ID );


                    result.ISMMCLT_Id = data.ISMMCLT_Id;
                    result.ICCF_IE_HRMEID = data.ICCF_IE_HRMEID;
                    result.EndUser_No = data.EndUser_No;
                    result.Mangement_No = data.Mangement_No;
                    result.NodalOfficerRemark = data.NodalOfficerRemark;
                    result.MangmentRemark = data.MangmentRemark;
                    result.ConfirmationRemark = data.ConfirmationRemark;
                    result.ICCF_Date = data.ICCF_Date;
                    result.ICCF_Createby = data.UserId;
                    result.ICCF_updatedby = data.UserId;
                    result.ICCF_createddate = DateTime.Now;
                    result.ICCF_updateddate = DateTime.Now;
                    result.ICCF_Activeflag = true;
                    result.ICCF_Flag = data.ICCF_Flag;
                    result.ASMAY_Id = data.asmaY_Id;
                    result.IMM_Id = data.IMM_Id;
                    result.IVRMM_Id = data.IVRMM_Id;
                    result.ParentsRemarks = data.ParentsRemarks;
                    result.ParentsNumber = data.ParentsNumber;
                    result.PCstatus = data.PCstatus;

                    _IssueManagercontext.Update(result);
                    var vv = _IssueManagercontext.SaveChanges();
                    if (vv > 0)
                    {
                        data.returnvalue = "Update";
                    }
                    else
                    {
                        data.returnvalue = "Error";
                    }



                }
                else
                {
                    ISM_CallCenter_CLientFeedbackDMO feedback = new ISM_CallCenter_CLientFeedbackDMO();

                    feedback.ISMMCLT_Id = data.ISMMCLT_Id;
                    feedback.ICCF_IE_HRMEID = data.ICCF_IE_HRMEID;
                    feedback.EndUser_No = data.EndUser_No;
                    feedback.Mangement_No = data.Mangement_No;
                    feedback.NodalOfficerRemark = data.NodalOfficerRemark;
                    feedback.MangmentRemark = data.MangmentRemark;
                    feedback.ConfirmationRemark = data.ConfirmationRemark;
                    feedback.ICCF_Date = Convert.ToDateTime(data.ICCF_Date);
                    feedback.ICCF_Createby = data.UserId;
                    feedback.ICCF_updatedby = data.UserId;
                    feedback.ICCF_createddate = DateTime.Now;
                    feedback.ICCF_updateddate = DateTime.Now;
                    feedback.ICCF_Activeflag = true;
                    feedback.ICCF_Flag = data.ICCF_Flag;
                    feedback.ASMAY_Id = data.asmaY_Id;
                    feedback.IMM_Id = data.IMM_Id;
                    feedback.IVRMM_Id = data.IVRMM_Id;
                    feedback.ParentsRemarks = data.ParentsRemarks;
                    feedback.ParentsNumber = data.ParentsNumber;
                    feedback.PCstatus = data.PCstatus;
                    _IssueManagercontext.Add(feedback);
                    var vv = _IssueManagercontext.SaveChanges();
                    if (vv > 0)
                    {
                        data.returnvalue = "Add";
                    }
                    else
                    {
                        data.returnvalue = "Error";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClientInvoiceDTO companychange(ClientInvoiceDTO data)
        {
            try
            {
                data.clientlist = (from a in _IssueManagercontext.clientTable
                                   from b in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new ClientInvoiceDTO
                                   {
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       ISMMCLT_Id = b.ISMMCLT_Id,



                                   }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();


                List<AcademicYear> list = new List<AcademicYear>();
                list = _IssueManagercontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.taxlist = _IssueManagercontext.INV_Master_TaxDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMT_ActiveFlg == true).Distinct().ToArray();

                data.instlist = _IssueManagercontext.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).Distinct().ToArray();

                data.instlistmobile = _IssueManagercontext.Institution_Phone_No.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();

                data.instlistemail = _IssueManagercontext.Institution_EmailId.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();
                data.modeofpaymentlist = _db.IVRM_ModeOfPaymentDMO.Where(a => a.MI_Id == data.MI_Id && a.IVRMMOD_ActiveFlag == true).Distinct().ToArray();

                data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_CLIENT_INVIOCE";
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.alldata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }





            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public ClientInvoiceDTO getProject(ClientInvoiceDTO data)
        {
            try
            {

                data.projectlist = (from a in _IssueManagercontext.clientTable
                                    from b in _IssueManagercontext.MastersProject_DMO
                                    from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id)
                                    select new ClientInvoiceDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                        ISMINC_MOURefNo = c.ISMMCLTPR_MOURefNo,
                                        ISMINC_MOUDate = c.ISMMCLTPR_MOUDate,
                                        ISMINC_WorkOrder = c.ISMMCLTPR_WorkOrder,
                                    }).Distinct().ToArray();


                data.clientlist = _IssueManagercontext.clientTable.Where(w => w.ISMMCLT_Id == data.ISMMCLT_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientInvoiceDTO getbom(ClientInvoiceDTO data)
        {
            try
            {
                data.projectlist = (from a in _IssueManagercontext.clientTable
                                    from b in _IssueManagercontext.MastersProject_DMO
                                    from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                    from d in _IssueManagercontext.ISM_Client_Master_Components_DMO
                                    from e in _IssueManagercontext.ISM_Client_Project_BOM_DMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id && c.ISMMCLTPR_Id == e.ISMMCLTPR_Id && d.ISMCLTC_Id == e.ISMCLTC_Id && e.ISMCLTPRBOM_ActiveFlag == true && d.ISMCLTC_ActiveFlag == true && a.MI_Id == d.MI_Id)
                                    select new ClientInvoiceDTO
                                    {
                                        ISMCLTC_Id = d.ISMCLTC_Id,
                                        ISMCLTC_Name = d.ISMCLTC_Name,
                                        ISMCLTPRBOM_Qty = e.ISMCLTPRBOM_Qty,
                                        ISMINC_MOURefNo = c.ISMMCLTPR_MOURefNo,
                                        ISMINC_MOUDate = c.ISMMCLTPR_MOUDate,
                                        ISMINC_WorkOrder = c.ISMMCLTPR_WorkOrder

                                    }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientInvoiceDTO savedata(ClientInvoiceDTO data)
        {
            try
            {
                if (data.ISMINC_Id == 0)
                {
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_INVOICE_UPDATE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                        { Value = data.asmaY_Id });
                        cmd.Parameters.Add(new SqlParameter("@StudentAdmnoOP", SqlDbType.VarChar, Int32.MaxValue)
                        { Direction = ParameterDirection.Output });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        data.trans_id = cmd.Parameters["@StudentAdmnoOP"].Value.ToString();
                    }

                    ISM_InvoiceDMO obj = new ISM_InvoiceDMO();
                    obj.ISMMCLT_Id = data.ISMMCLT_Id;
                    obj.MI_Id = data.MI_Id;
                    obj.ISMMPR_Id = data.ISMMPR_Id;
                    obj.ISMINC_WorkOrder = data.ISMINC_WorkOrder;
                    obj.ISMINC_PrInviceNo = data.trans_id;
                    obj.ISMINC_Date = Convert.ToDateTime(data.ISMCLTPRP_PaymentDate);
                    obj.ISMINC_TotalTaxAmount = data.ISMINC_TotalTaxAmount;
                    obj.ISMINC_TotalAmount = data.ISMINC_TotalAmount;
                    obj.ISMINC_Remarks = data.ISMINC_Remarks;
                    obj.ISMINC_MOURefNo = data.ISMINC_MOURefNo;
                    obj.ISMINC_AcademicYear = data.ISMINC_AcademicYear;
                    obj.ISMINC_InstallmentName = data.ISMINC_InstallmentName;
                    obj.ISMINC_TotalBasicAmount = data.ISMINC_TotalBasicAmount;
                    obj.ISMINC_TotalPercentage = data.ISMINC_TotalPercentage;

                    if (data.ISMINC_MOURefNo == "" || data.ISMINC_MOURefNo == null)
                    {
                        obj.ISMINC_MOUDate = null;
                    }
                    else
                    {
                        obj.ISMINC_MOUDate = data.ISMINC_MOUDate;
                    }

                    obj.HRMBD_Id = data.HRMBD_Id;
                    obj.ISMINC_ModeOfPayment = data.ISMINC_ModeOfPayment;
                    obj.ISMINC_ActiveFlag = true;
                    obj.ISMINC_CreatedDate = DateTime.Now;
                    obj.ISMINC_CreatedBy = data.UserId;
                    obj.ISMINC_UpdatedDate = DateTime.Now;
                    obj.ISMINC_UpdatedBy = data.UserId;
                    _IssueManagercontext.Add(obj);
                    if (data.itemsdto != null)
                    {
                        foreach (var item in data.itemsdto)
                        {
                            ISM_Invoice_DetailsDMO obj1 = new ISM_Invoice_DetailsDMO();
                            obj1.ISMINC_Id = obj.ISMINC_Id;
                            obj1.ISMCLTC_Id = item.ISMCLTC_Id;
                            obj1.ISMCLTPRMP_Id = item.ISMCLTPRMP_Id;
                            obj1.ISMINCD_Qty = item.ISMINCD_Qty;
                            obj1.ISMINCD_UnitRate = item.ISMINCD_UnitRate;
                            obj1.ISMINCD_Amount = item.ISMINCD_Amount;
                            obj1.ISMINCD_ItemDesc = item.ISMINCD_ItemDesc;
                            obj1.ISMINCD_Remarks = item.ISMINCD_Remarks;
                            obj1.ISMINCD_ActiveFlag = true;
                            obj1.ISMINCD_CreatedDate = DateTime.Now;
                            obj1.ISMINCD_CreatedBy = data.UserId;
                            obj1.ISMINCD_UpdatedDate = DateTime.Now;
                            obj1.ISMINCD_UpdatedBy = data.UserId;
                            obj1.ISMINCD_HSNCode = item.ISMINCD_HSNCode;
                            obj1.ISMINCD_SACCode = item.ISMINCD_SACCode;
                            _IssueManagercontext.Add(obj1);
                        }
                    }
                    if (data.taxdto != null)
                    {
                        foreach (var item in data.taxdto)
                        {
                            ISM_Invoice_TaxDMO obj2 = new ISM_Invoice_TaxDMO();
                            obj2.ISMINC_Id = obj.ISMINC_Id;
                            obj2.INVMT_Id = item.INVMT_Id;
                            obj2.ISMINTX_TaxPercent = item.INVMIT_TaxValue;
                            obj2.ISMINTX_TaxAmount = item.taxamount;
                            obj2.ISMINTX_ActiveFlag = true;
                            obj2.ISMINTX_CreatedDate = DateTime.Now;
                            obj2.ISMINTX_CreatedBy = data.UserId;
                            obj2.ISMINTX_UpdatedDate = DateTime.Now;
                            obj2.ISMINTX_UpdatedBy = data.UserId;
                            _IssueManagercontext.Add(obj2);
                        }
                    }
                    int cxt = _IssueManagercontext.SaveChanges();
                    if (cxt > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }

                }
                else
                {
                    var update = _IssueManagercontext.ISM_InvoiceDMO.Single(e => e.ISMINC_Id == data.ISMINC_Id);
                    update.ISMINC_WorkOrder = data.ISMINC_WorkOrder;
                    update.ISMINC_Date = Convert.ToDateTime(data.ISMCLTPRP_PaymentDate);
                    update.ISMINC_TotalTaxAmount = data.ISMINC_TotalTaxAmount;
                    update.ISMINC_TotalAmount = data.ISMINC_TotalAmount;
                    update.ISMINC_Remarks = data.ISMINC_Remarks;
                    update.ISMINC_MOURefNo = data.ISMINC_MOURefNo;
                    update.ISMINC_InstallmentName = data.ISMINC_InstallmentName;
                    if (data.ISMINC_MOURefNo == "" || data.ISMINC_MOURefNo == null)
                    {
                        update.ISMINC_MOUDate = null;
                    }
                    else
                    {
                        update.ISMINC_MOUDate = data.ISMINC_MOUDate;
                    }
                    update.HRMBD_Id = data.HRMBD_Id;
                    update.ISMINC_ModeOfPayment = data.ISMINC_ModeOfPayment;
                    update.ISMINC_ActiveFlag = true;
                    update.ISMINC_UpdatedDate = DateTime.Now;
                    update.ISMINC_UpdatedBy = data.UserId;
                    update.ISMINC_TotalBasicAmount = data.ISMINC_TotalBasicAmount;
                    update.ISMINC_TotalPercentage = data.ISMINC_TotalPercentage;
                    update.ISMINC_AcademicYear = data.ISMINC_AcademicYear;
                    _IssueManagercontext.Update(update);

                    var detailslist = _IssueManagercontext.ISM_Invoice_DetailsDMO.Where(e => e.ISMINC_Id == data.ISMINC_Id).ToList();
                    if (detailslist.Count > 0)
                    {
                        foreach (var item in detailslist)
                        {
                            _IssueManagercontext.Remove(item);
                        }
                    }

                    var taxlist = _IssueManagercontext.ISM_Invoice_TaxDMO.Where(e => e.ISMINC_Id == data.ISMINC_Id).ToList();
                    if (taxlist.Count > 0)
                    {
                        foreach (var item in taxlist)
                        {
                            _IssueManagercontext.Remove(item);
                        }
                    }

                    if (data.itemsdto != null)
                    {
                        foreach (var item in data.itemsdto)
                        {
                            ISM_Invoice_DetailsDMO obj1 = new ISM_Invoice_DetailsDMO();
                            obj1.ISMINC_Id = data.ISMINC_Id;
                            obj1.ISMCLTC_Id = item.ISMCLTC_Id;
                            obj1.ISMCLTPRMP_Id = item.ISMCLTPRMP_Id;
                            obj1.ISMINCD_Qty = item.ISMINCD_Qty;
                            obj1.ISMINCD_UnitRate = item.ISMINCD_UnitRate;
                            obj1.ISMINCD_Amount = item.ISMINCD_Amount;
                            obj1.ISMINCD_ItemDesc = item.ISMINCD_ItemDesc;
                            obj1.ISMINCD_Remarks = item.ISMINCD_Remarks;
                            obj1.ISMINCD_ActiveFlag = true;
                            obj1.ISMINCD_CreatedDate = DateTime.Now;
                            obj1.ISMINCD_CreatedBy = data.UserId;
                            obj1.ISMINCD_UpdatedDate = DateTime.Now;
                            obj1.ISMINCD_UpdatedBy = data.UserId;
                            obj1.ISMINCD_HSNCode = item.ISMINCD_HSNCode;
                            obj1.ISMINCD_SACCode = item.ISMINCD_SACCode;

                            _IssueManagercontext.Add(obj1);

                        }
                    }

                    if (data.taxdto != null)
                    {
                        foreach (var item in data.taxdto)
                        {
                            ISM_Invoice_TaxDMO obj2 = new ISM_Invoice_TaxDMO();
                            obj2.ISMINC_Id = data.ISMINC_Id;
                            obj2.INVMT_Id = item.INVMT_Id;
                            obj2.ISMINTX_TaxPercent = item.INVMIT_TaxValue;
                            obj2.ISMINTX_TaxAmount = item.taxamount;
                            obj2.ISMINTX_ActiveFlag = true;
                            obj2.ISMINTX_CreatedDate = DateTime.Now;
                            obj2.ISMINTX_CreatedBy = data.UserId;
                            obj2.ISMINTX_UpdatedDate = DateTime.Now;
                            obj2.ISMINTX_UpdatedBy = data.UserId;
                            _IssueManagercontext.Add(obj2);
                        }
                    }

                    int cxt = _IssueManagercontext.SaveChanges();
                    if (cxt > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public ClientInvoiceDTO getinvoiceno(ClientInvoiceDTO data)
        {
            try
            {

                //getinvoiceno
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public ClientInvoiceDTO Editdata(ClientInvoiceDTO data)
        {
            try
            {

                var editlist = _IssueManagercontext.ISM_InvoiceDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).Distinct().ToList();

                data.editlist = editlist.ToArray();


                data.clientlist = _IssueManagercontext.clientTable.Where(d => d.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id).ToArray();

                data.geteditclient = (from a in _IssueManagercontext.clientTable
                                      from b in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                      where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id
                                      && a.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id)
                                      select new ClientInvoiceDTO
                                      {
                                          ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                          ISMMCLT_Id = b.ISMMCLT_Id
                                      }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();


                data.projectlist = (from a in _IssueManagercontext.clientTable
                                    from b in _IssueManagercontext.MastersProject_DMO
                                    from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id)
                                    select new ISM_Master_ClientProject_PaymentDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                    }).Distinct().ToArray();

                data.editlistdetails = _IssueManagercontext.ISM_Invoice_DetailsDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).ToArray();
                data.editlisttax = _IssueManagercontext.ISM_Invoice_TaxDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientInvoiceDTO clientDecative(ClientInvoiceDTO data)
        {
            try
            {
                var u = _IssueManagercontext.ISM_InvoiceDMO.Where(t => t.ISMINC_Id == data.ISMINC_Id).SingleOrDefault();
                if (u.ISMINC_ActiveFlag == true)
                {
                    u.ISMINC_ActiveFlag = false;
                }
                else if (u.ISMINC_ActiveFlag == false)
                {
                    u.ISMINC_ActiveFlag = true;
                }

                u.ISMINC_UpdatedBy = data.UserId;
                u.ISMINC_UpdatedDate = DateTime.Now;
                _IssueManagercontext.Update(u);
                int o = _IssueManagercontext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public ClientInvoiceDTO viewdetails(ClientInvoiceDTO data)
        {
            try
            {
                data.paymentmodedetails = _IssueManagercontext.IVRM_ModeOfPayment.Where(a => a.MI_Id == data.MI_Id
                && a.IVRMMOD_ActiveFlag == true).Distinct().ToArray();

                data.getpaymentdetails = _IssueManagercontext.ISM_Client_Project_Payment_DetailsDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClientInvoiceDTO savepaymentdetailsrecord(ClientInvoiceDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment Configuration
        public ClientInvoiceDTO loaddataconfig(ClientInvoiceDTO data)
        {
            try
            {
                data.getconfigloaddata = _IssueManagercontext.ISM_Client_Payment_ConfigurationDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClientInvoiceDTO savedataconfig(ClientInvoiceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ISMCPC_Id > 0)
                {
                    var checkresult = _IssueManagercontext.ISM_Client_Payment_ConfigurationDMO.Single(a => a.ISMCPC_Id == data.ISMCPC_Id);
                    checkresult.ISMCPC_RemainderDays = data.ISMCPC_RemainderDays;
                    checkresult.ISMCPC_FullORPartialPayment = data.ISMCPC_FullORPartialPayment;
                    checkresult.UpdatedDate = indiantime0;
                    _IssueManagercontext.Update(checkresult);
                    var i = _IssueManagercontext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
                else
                {
                    ISM_Client_Payment_ConfigurationDMO iSM_Client_Payment_ConfigurationDMO = new ISM_Client_Payment_ConfigurationDMO();
                    iSM_Client_Payment_ConfigurationDMO.ISMCPC_RemainderDays = data.ISMCPC_RemainderDays;
                    iSM_Client_Payment_ConfigurationDMO.ISMCPC_FullORPartialPayment = data.ISMCPC_FullORPartialPayment;
                    iSM_Client_Payment_ConfigurationDMO.UpdatedDate = indiantime0;
                    iSM_Client_Payment_ConfigurationDMO.CreatedDate = indiantime0;
                    _IssueManagercontext.Add(iSM_Client_Payment_ConfigurationDMO);

                    var i = _IssueManagercontext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                data.msg = "Failed";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment subscription audit report
        public ClientInvoiceDTO loaddatareport(ClientInvoiceDTO data)
        {
            try
            {
                data.clientlist = (from a in _IssueManagercontext.clientTable
                                   from b in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new ClientInvoiceDTO
                                   {
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       ISMMCLT_Id = b.ISMMCLT_Id
                                   }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClientInvoiceDTO getreport(ClientInvoiceDTO data)
        {
            try
            {
                //ISMMPR_Id
                var update = _db.INV_M_PurchaseOrderDMO.Where(R => R.INVMPO_Id == data.ISMMPR_Id).ToList();
                if (update.Count > 0)
                {
                    foreach (var d in update)
                    {
                        d.INVMPO_POTemplate = data.msg;
                        _db.Update(d);
                    }
                    int order = _db.SaveChanges();
                    if (order > 0)
                    {
                        if (data.ISMINC_WorkOrder == "SAVE")
                        {
                            return data;
                        }
                    }

                }
                Dictionary<string, string> val = new Dictionary<string, string>();//CleintInvoiceMail
                var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "Potemplate" && e.ISES_MailActiveFlag == true).ToList();
                // var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "CleintInvoiceMail" && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return data;
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = data.msg;
                string Mailmsg = data.msg;
                Mailcontent = "<br />";
                Mailmsg = "<br />";
                Mailcontent = Mailcontent + data.msg;
                Mailmsg = Mailmsg + data.msg;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    var message = new SendGridMessage();
                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                    {
                        string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                        foreach (var c in ccmail)
                        {
                            if (c != "")
                            {
                                message.AddBcc(c);

                            }
                        }

                    }

                    //if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    //{
                    //    string[] ccmail = template[0].ISES_MailCCId.Split(',');
                    //    foreach (var c in ccmail)
                    //    {
                    //        if (c != "")
                    //        {
                    //            message.AddCc(c);

                    //        }
                    //    }

                    //}
                    //FHEAD
                    if (data.FHEAD != null && data.FHEAD != "")
                    {
                        string[] ccmail = data.FHEAD.Split(',');
                        foreach (var c in ccmail)
                        {
                            if (c != "")
                            {
                                message.AddCc(c);

                            }
                        }

                    }
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    if (data.esubject != null && data.esubject != "")
                    {
                        message.Subject = data.esubject;
                    }
                    else
                    {
                        message.Subject = Subject;
                    }
                    // string emailid = "sanjeev@globalqtytrainig.com";
                    string emailid = data.ClientMail;
                    message.AddTo(emailid);


                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "Potemplate" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = emailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        //payment notification
        public async Task<ClientInvoiceDTO> paymentnotification(ClientInvoiceDTO data)
        {
            try
            {
                if (data.ISMMCLT_ClientCode == "WhatsApp")
                {
                    whatsapp whatsapp = new whatsapp(_db);
                    string filename = "Invoice"; string fileid = "";
                    string fileContentsAsBase64 = "";
                    if (data.taxdto != null && data.taxdto.Length > 0)
                    {
                        foreach (var item in data.taxdto)
                        {
                            if (item.cfilepath != null && item.cfilepath != "")
                            {
                                var webClient = new WebClient();
                                byte[] imageBytes = webClient.DownloadData(item.cfilepath);
                                fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                byte[] fileBytes = Convert.FromBase64String(fileContentsAsBase64);
                                fileid = item.cfilepath;                                                          
                            }
                        }
                    }                           
                    string s = await whatsapp.Whatsapp(data.MI_Id, data.ISMMCLT_ContactNo, "CleintInvoiceMail", filename, fileid, data.esubject, data.fromdate, data.trans_id, data.roundoffttl, data.words, data.MI_Name, data.parameter);
                }
                else
                {
                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "CleintInvoiceMail" && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count == 0)
                    {
                        return data;
                    }
                    var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                    var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                    var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                    string Mailcontent = data.msg;
                    string Mailmsg = data.msg;
                    Mailcontent = "<br />";
                    Mailmsg = "<br />";
                    Mailcontent = Mailcontent + data.msg;
                    Mailmsg = Mailmsg + data.msg;
                    if (data.FHEAD != null && data.FHEAD != "")
                    {
                        Mailcontent = Mailcontent + "<br />" + data.FHEAD + "<br />";
                        Mailmsg = Mailmsg + "<br />" + data.FHEAD + "<br />";
                    }
                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string Subject = template[0].ISES_MailSubject.ToString();
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        var message = new SendGridMessage();
                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                            foreach (var c in ccmail)
                            {
                                if (c != "")
                                {
                                    message.AddBcc(c);

                                }
                            }

                        }

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmail = template[0].ISES_MailCCId.Split(',');
                            foreach (var c in ccmail)
                            {
                                if (c != "")
                                {
                                    message.AddCc(c);

                                }
                            }

                        }
                        //ISES_MailBCCId

                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        if (data.esubject != null && data.esubject != "")
                        {
                            message.Subject = data.esubject;
                        }
                        else
                        {
                            message.Subject = Subject;
                        }

                        string emailid = data.ClientMail;
                        //  string emailid = "sanjeev@globalqtytrainig.com";
                        message.AddTo(emailid);


                        //StringBuilder sb = new StringBuilder(data.ISMMCLT_ClientName);
                        //StringReader sr = new StringReader(sb.ToString());

                        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                        //HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                        //using (MemoryStream memoryStream = new MemoryStream())
                        //{
                        //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        //    pdfDoc.Open();
                        //    htmlparser.Parse(sr);
                        //    pdfDoc.Close();
                        //    byte[] bytess = memoryStream.ToArray();
                        //    memoryStream.Close();
                        //    var file = Convert.ToBase64String(bytess);
                        //    string emp;
                        //    emp = Convert.ToString(sr);
                        //    string c = "";
                        //    string v = emp.Replace("System.IO.StringReader", "Invoice.Pdf");
                        //    message.AddAttachment(v, file);


                        //}
                        //
                        if (data.taxdto != null && data.taxdto.Length > 0)
                        {
                            foreach (var item in data.taxdto)
                            {
                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(item.cfilepath);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(item.cfilepath, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }


                        //
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "CleintInvoiceMail" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                            var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                            var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_Email_Outgoing";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                            {
                                Value = emailid
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                            {
                                Value = Mailmsg
                            });
                            cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                            {
                                Value = modulename[0]
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //added By sanjeev

        public ClientInvoiceDTO savedatadoc(ClientInvoiceDTO data)
        {
            try
            {
                if (data.ISMINC_Id > 0)
                {
                    Array GetFiles = _IssueManagercontext.ISM_Invoice_FilesDMO.Where(a => a.ISMINC_ID == data.ISMINC_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _IssueManagercontext.Remove(c);
                    }
                    _IssueManagercontext.SaveChanges();
                }
                if (data.Docupload != null && data.Docupload.Length > 0)
                {
                    foreach (var d in data.Docupload)
                    {
                        var result = _IssueManagercontext.Database.ExecuteSqlCommand("usp_i_Invoice_Files @p0,@p1,@p2,@p3,@p4,@p5",
                        data.ISMINC_Id,d.IIF_FileName,d.IIF_FileDesc,d.IIF_FilePath,1,data.UserId);

                        if (result > 0)
                        {
                            data.msg = "Saved";
                        }
                        else
                        {
                            data.msg = "NotSaved";
                        }
                    }
                   
                }
                else
                {
                    data.returnval = false;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public ClientInvoiceDTO Duplicatedoc(ClientInvoiceDTO data)
        {
            try
            {
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Invoice_Files";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMINC_ID", SqlDbType.BigInt)
                    {
                        Value = data.ISMINC_Id
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
                        data.Invoice_Files = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientInvoiceDTO ViewFiles(ClientInvoiceDTO data)
        {
            try
            {
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Invoice_Files";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMINC_ID", SqlDbType.BigInt)
                    {
                        Value = data.ISMINC_Id
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
                        data.getviewfilesdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientInvoiceDTO uploaddeactivate(ClientInvoiceDTO data)
        {
            try
            {
                var u = _IssueManagercontext.ISM_Invoice_FilesDMO.Where(a => a.IIF_ID == data.IIF_ID).SingleOrDefault();
                if (u.IIF_ActiveFlag == true)
                {
                    u.IIF_ActiveFlag = false;
                }
                else if (u.IIF_ActiveFlag == false)
                {
                    u.IIF_ActiveFlag = true;
                }

                u.IIF_UpdatedBy = data.UserId.ToString();
                u.IIF_UpdatedDate = DateTime.Now;
                _IssueManagercontext.Update(u);
                int o = _IssueManagercontext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}

