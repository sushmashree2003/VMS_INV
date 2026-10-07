using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.Sales;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;
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
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.WebSockets.Internal.Constants;

namespace Recruitment.com.vaps.Services
{
    public class Lead_Target_MappingImpl : Interfaces.Lead_Target_MappingInterface
    {

        public VMSContext _vmsconte;
        public HRMSContext _hrmscon;
        private DomainModelMsSqlServerContext _db;
        public Lead_Target_MappingImpl(VMSContext vmsContext, HRMSContext hrms, DomainModelMsSqlServerContext db)
        {
            _vmsconte = vmsContext;
            _hrmscon = hrms;
            _db = db;
        }


        public Lead_Target_MappingDTO getBasicData(Lead_Target_MappingDTO dto)
        {

            try
            {



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_confirmList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.MI_Id
                    //});


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
                        dto.leadlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_lead_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                        Value = ""
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
                        dto.leadTempDetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Template";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
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
                        dto.leadApproval = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                //customization proposal data

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Custom_Template";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
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
                        dto.confirmList = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //dto.bomfinallist = _vmsconte.ISM_Sales_Lead_BOMDMO.ToArray();


                //custom proposal details
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Custom_Template_Request";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
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
                        dto.bomfinallist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public Lead_Target_MappingDTO OnloadGetDetailsnego(Lead_Target_MappingDTO dto)
        {

            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_negotiation";
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
                        dto.leadnegotiation = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_negotiation_deails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                        dto.leadnegotiation_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_negotiation_Approval_deails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
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
                        dto.leadnegotiation_Approvaldetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //adeedd by N

                dto.yearlist = _vmsconte.AcademicYear.Where(t => t.MI_Id == dto.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                dto.getinstallment = (from a in _vmsconte.FeeInstallmentDMO
                                      from b in _vmsconte.FeeInstallmentsyearlyDMO
                                      where (a.FMI_Id == b.FMI_Id && a.FMI_ActiceFlag == true && a.MI_Id == dto.MI_Id && b.MI_ID == dto.MI_Id)
                                      select b).Distinct().OrderBy(a => a.FTI_Name).ToArray();



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Modules_Neagaotation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.MI_Id
                    //});
                    //cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.User_Id
                    //});


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
                        dto.getLeadtarget_Mapping = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


        public Lead_Target_MappingDTO savedetails(Lead_Target_MappingDTO dto)
        {
            try
            {
                if (dto.leadlistarray != null && dto.leadlistarray.Length > 0)
                {
                    foreach (var d in dto.leadlistarray)
                    {
                        var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISm_Sales_Lead_BOMInsert @p0,@p1,@p2,@p3,@p4",
                            d.ISMSLET_Id, dto.ISMS_NegoMDApprovedFlag, d.ISMS_MOMFlag, d.Negotiation_Amount, d.ISMSLE_Id);
                        if (contactExistsP > 0)
                        {

                        }
                        if (d.Project_Payment_Lead != null && d.Project_Payment_Lead.Length > 0)
                        {
                            var duplicate = _vmsconte.ISM_Client_Project_Payment_LeadDMO.Where(R => R.ISMSLE_Id == d.ISMSLE_Id && R.ISMSLET_Id == d.ISMSLET_Id).ToList();
                            if (duplicate.Count > 0)
                            {
                                foreach (var rem in duplicate)
                                {
                                    _vmsconte.Remove(rem);
                                }
                            }

                            foreach (var payment in d.Project_Payment_Lead)
                            {
                                ISM_Client_Project_Payment_LeadDMO obdfj = new ISM_Client_Project_Payment_LeadDMO();
                                obdfj.ISMSLE_Id = d.ISMSLE_Id;
                                obdfj.ISMCLTPRP_Year = payment.ISMCLTPRP_Year;
                                obdfj.ISMCLTPRPL_Per = payment.ISMCLTPRPL_Per;
                                obdfj.ISMCLTPRP_InstallmentAmt = payment.ISMCLTPRP_InstallmentAmt;
                                obdfj.ISMCLTPRP_PaymentDate = payment.ISMCLTPRP_PaymentDate;
                                obdfj.ISMCLTPRP_ActiveFlag = true;
                                obdfj.ISMCLTPRP_CreatedBy = dto.User_Id;
                                obdfj.ISMCLTPRP_UpdatedBy = dto.User_Id;
                                obdfj.CreatedDate = DateTime.Now;
                                obdfj.UpdatedDate = DateTime.Now;
                                obdfj.FTI_Id = payment.FTI_Id;
                                obdfj.ISMCLTPRP_PaymentStatus = "In Progress";
                                obdfj.ISMCLTPRP_BalanceAmt = payment.ISMCLTPRP_BalanceAmt;
                                obdfj.ISMCLTPRP_ExcessAmt = payment.ISMCLTPRP_ExcessAmt;
                                obdfj.FMI_Id = payment.FMI_Id;
                                obdfj.ISMSLET_Id = d.ISMSLET_Id;
                                _vmsconte.Add(obdfj);
                            }
                            int s = _vmsconte.SaveChanges();
                            if (s > 0)
                            {
                                dto.msg = "Update";
                            }
                            else
                            {
                                dto.msg = "NotUpdate";
                            }
                        }
                        //added New things
                        var ISMSALENA_StatusFlg = "Pending";
                        if (d.ISMS_MOMFlag == true)
                        {
                            ISMSALENA_StatusFlg = "Approved";
                        }
                        ISM_Sales_Lead_Negotiation_AuthorisationDMO obj = new ISM_Sales_Lead_Negotiation_AuthorisationDMO();
                        obj.ISMSLE_Id = d.ISMSLE_Id;
                        obj.ISMSALENA_Sanctioned = 0;
                        obj.ISMSALENA_SanctionedAmount = Convert.ToDecimal(d.Negotiation_Client);
                        obj.ISMSALENA_Remarks = d.ISMSALENA_Remarks;
                        obj.IVRMUL_Id = dto.User_Id;
                        obj.ISMSALENA_ActiveFlg = true;
                        obj.ISMSALENA_CreatedBy = dto.User_Id;
                        obj.ISMSALENA_UpdatedBy = dto.User_Id;
                        obj.ISMSALENA_CreatedDate = DateTime.Now;
                        obj.ISMSALENA_UpdatedDate = DateTime.Now;
                        obj.ISMSALENA_Date = DateTime.Now;
                        obj.ISMSALENA_StatusFlg = ISMSALENA_StatusFlg;
                        obj.ISMSLET_Id = d.ISMSLET_Id;
                        _vmsconte.Add(obj);
                        int s2 = _vmsconte.SaveChanges();

                        if (s2 > 0)
                        {
                            long Id = _db.Staff_User_Login.Where(t => t.Id == dto.User_Id).Select(t => t.Emp_Code).FirstOrDefault();
                            //string emailid = "sanjeev@globalqtytrainig.com";
                             //string emailidcc = "nagaraja.mv@vapstech.com";
                            string emailidcc = _db.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                           // sendmails(dto.MI_Id, emailid, "MOUConfirmed", d.ISMSLET_Id, dto.User_Id, ISMSALENA_StatusFlg, d.Negotiation_Client, d.ISMSALENA_Remarks);
                            sendmails(dto.MI_Id, emailidcc, "MOUConfirmed", d.ISMSLET_Id, dto.User_Id, ISMSALENA_StatusFlg, d.Negotiation_Client, d.ISMSALENA_Remarks);


                        }
                        else
                        {

                        }
                    }
                    //

                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        public Lead_Target_MappingDTO saveNegotiationdetails(Lead_Target_MappingDTO dto)
        {
            try
            {
                //leadlistarray
                if (dto.leadlistarray != null && dto.leadlistarray.Length > 0)
                {
                    if (dto.msg == "MOU")
                    {
                        foreach (var d in dto.leadlistarray)
                        {
                            var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISm_Sales_Lead_BOMInsert @p0,@p1,@p2,@p3,@p4",
                            d.ISMSLET_Id, dto.ISMS_NegoMDApprovedFlag, d.ISMS_MOMFlag, d.Negotiation_Amount, d.ISMSLE_Id);
                            if (contactExistsP > 0)
                            {

                            }

                            if (d.Project_Payment_Lead != null && d.Project_Payment_Lead.Length > 0)
                            {
                                var duplicate = _vmsconte.ISM_Client_Project_Payment_LeadDMO.Where(R => R.ISMSLE_Id == d.ISMSLE_Id && R.ISMSLET_Id == d.ISMSLET_Id).ToList();
                                if (duplicate.Count > 0)
                                {
                                    foreach (var rem in duplicate)
                                    {
                                        _vmsconte.Remove(rem);
                                    }
                                }

                                foreach (var payment in d.Project_Payment_Lead)
                                {
                                    ISM_Client_Project_Payment_LeadDMO obkkj = new ISM_Client_Project_Payment_LeadDMO();
                                    obkkj.ISMSLE_Id = d.ISMSLE_Id;
                                    obkkj.ISMCLTPRP_Year = payment.ISMCLTPRP_Year;
                                    obkkj.ISMCLTPRP_InstallmentAmt = payment.ISMCLTPRP_InstallmentAmt;
                                    obkkj.ISMCLTPRP_PaymentDate = payment.ISMCLTPRP_PaymentDate;
                                    obkkj.ISMCLTPRP_ActiveFlag = true;
                                    obkkj.ISMCLTPRP_CreatedBy = dto.User_Id;
                                    obkkj.ISMCLTPRP_UpdatedBy = dto.User_Id;
                                    obkkj.CreatedDate = DateTime.Now;
                                    obkkj.UpdatedDate = DateTime.Now;
                                    obkkj.FTI_Id = payment.FTI_Id;
                                    obkkj.ISMCLTPRP_PaymentStatus = "In Progress";
                                    obkkj.ISMCLTPRP_BalanceAmt = payment.ISMCLTPRP_BalanceAmt;
                                    obkkj.ISMCLTPRP_ExcessAmt = payment.ISMCLTPRP_ExcessAmt;
                                    obkkj.FMI_Id = payment.FMI_Id;
                                    obkkj.ISMSLET_Id = d.ISMSLET_Id;
                                    _vmsconte.Add(obkkj);
                                }
                                int s = _vmsconte.SaveChanges();
                                if (s > 0)
                                {
                                    // dto.msg = "Update";
                                }
                                else
                                {
                                    // dto.msg = "UpdateFailed";
                                }
                            }
                            //
                            ISM_Sales_Lead_Negotiation_AuthorisationDMO obj = new ISM_Sales_Lead_Negotiation_AuthorisationDMO();
                            obj.ISMSLE_Id = d.ISMSLE_Id;
                            obj.ISMSALENA_Sanctioned = d.SanctionLevelNo;
                            obj.ISMSALENA_SanctionedAmount = d.Negotiation_Amount;
                            obj.ISMSALENA_Remarks = d.ISMSALENA_Remarks;
                            obj.IVRMUL_Id = dto.User_Id;
                            obj.ISMSALENA_ActiveFlg = true;
                            obj.ISMSALENA_CreatedBy = dto.User_Id;
                            obj.ISMSALENA_UpdatedBy = dto.User_Id;
                            obj.ISMSALENA_CreatedDate = DateTime.Now;
                            obj.ISMSALENA_UpdatedDate = DateTime.Now;
                            obj.ISMSALENA_Date = DateTime.Now;
                            obj.ISMSLET_Id = d.ISMSLET_Id;
                            obj.ISMSALENA_StatusFlg = "Approved";
                            int s2 = _vmsconte.SaveChanges();
                            _vmsconte.Add(obj);
                            if (s2 > 0)
                            {
                                dto.msg = "Update";
                            }
                            else
                            {
                                dto.msg = "UpdateFailed";
                            }
                        }
                    }
                    else
                    {
                        foreach (var d in dto.leadlistarray)
                        {
                            ISM_Sales_Lead_Negotiation_AuthorisationDMO obj = new ISM_Sales_Lead_Negotiation_AuthorisationDMO();
                            obj.ISMSLE_Id = d.ISMSLE_Id;
                            obj.ISMSALENA_Sanctioned = d.SanctionLevelNo;
                            obj.ISMSALENA_SanctionedAmount = d.Negotiation_Amount;
                            obj.ISMSALENA_Remarks = d.ISMSALENA_Remarks;
                            obj.IVRMUL_Id = dto.User_Id;
                            obj.ISMSALENA_ActiveFlg = true;
                            obj.ISMSALENA_CreatedBy = dto.User_Id;
                            obj.ISMSALENA_UpdatedBy = dto.User_Id;
                            obj.ISMSALENA_CreatedDate = DateTime.Now;
                            obj.ISMSALENA_UpdatedDate = DateTime.Now;
                            obj.ISMSALENA_Date = DateTime.Now;
                            obj.ISMSLET_Id = d.ISMSLET_Id;
                            if (d.HRPAON_FinalFlg == true)
                            {
                                obj.ISMSALENA_StatusFlg = "Approved";
                                var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISm_Sales_Lead_BOMInsert @p0,@p1,@p2,@p3,@p4",
                             d.ISMSLET_Id, dto.ISMS_NegoMDApprovedFlag, d.ISMS_MOMFlag, d.Negotiation_Amount, d.ISMSLE_Id);
                                if (contactExistsP > 0)
                                {

                                }
                                var TemplateP = _vmsconte.Database.ExecuteSqlCommand("Sales_Lead_negotiation_Qutation_Insert @p0,@p1,@p2,@p3",
                            d.ISMSLET_Id, d.Negotiation_Amount, dto.User_Id, d.ISMSALENA_Remarks);
                                if (TemplateP > 0)
                                {

                                }


                            }
                            else
                            {
                                obj.ISMSALENA_StatusFlg = "Pending";

                            }
                            _vmsconte.Add(obj);
                        }
                        int s = _vmsconte.SaveChanges();
                        if (s > 0)
                        {
                            dto.msg = "Update";
                        }
                        else
                        {
                            dto.msg = "UpdateFailed";
                        }
                    }

                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Payment_Installments";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@installment", SqlDbType.BigInt)
                    {
                        Value = dto.installment
                    });


                    cmd.Parameters.Add(new SqlParameter("@Duration", SqlDbType.BigInt)
                    {
                        Value = dto.Duration
                    });


                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_ID
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
                        dto.Paymentterms = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        public Lead_Target_MappingDTO OnChangeOnChangeleadlist(Lead_Target_MappingDTO dto)
        {


            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_ISMSLECOM_SendQuotation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.MI_Id
                    //});

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
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
                        dto.SendQuotationleadlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_lead_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                        Value = dto.flag
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
                        dto.selectedleadlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_lead_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                        Value = "ProductDetails"
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
                        dto.productlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //added 
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_BOM_template_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                        dto.BOMTemplateList = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public Lead_Target_MappingDTO OnChangeOnChangeproductlist(Lead_Target_MappingDTO dto)
        {

            try
            {
                var product_ids = "0";
                if (dto.product.Length > 0)
                {
                    foreach (var item in dto.product)
                    {
                        product_ids = product_ids + "," + item;
                    }

                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_BOM_template";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSMPR_Id", SqlDbType.VarChar)
                    {
                        Value = product_ids
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
                        dto.BOMList = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //added By 



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public Lead_Target_MappingDTO get_quotation(Lead_Target_MappingDTO dto)
        {

            try
            {
                int count = 0;
                if(dto.bomListdetails !=null  && dto.bomListdetails.Length >0)
                {
                    foreach (var d in dto.bomListdetails)
                    {
                        var leadbomupdate = _vmsconte.Database.ExecuteSqlCommand("ISm_Sales_Negotion_ModuleInsert @p0,@p1,@p2,@p3", dto.ISMSLE_Id, dto.ISMSLBOM_Id, dto.User_Id,d.ISMSPS_ID);
                        if (leadbomupdate > 0)
                        {
                            count = count + 1;
                        }
                    }
                    if(count > 0)
                    {
                        dto.msg = "Add";
                    }
                }
                else
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Sales_Product_package_detail_perstudent_QutationReq";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
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
                            dto.perstudent_QutationReq = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                
                //added new thisg for getting lead module list
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_BOM_ModuleList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id

                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLBOM_Id

                    });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                        Value = "1"
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
                        dto.leadloaddata1 = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_BOM_ModuleList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id

                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.BigInt)
                    {

                        Value = dto.ISMSLBOM_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                        Value = "2"
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
                        dto.leadnegotiation_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }



            return dto;
        }
        public Lead_Target_MappingDTO OnChangeOnChangeBOMList(Lead_Target_MappingDTO dto)
        {

            try
            {
                var BOM_ids = "0";
                if (dto.BOMListt != null && dto.BOMListt.Length > 0)
                {
                    foreach (var item in dto.BOMListt)
                    {
                        BOM_ids = BOM_ids + "," + item;
                    }
                }

                //var bomdetails = _vmsconte.ISM_Sales_BOMDMO.Where(a => a.ISMSBOM_Id == dto.BOMListt[0] && a.ISMSBOM_ActiveFlag == true).Select(a => a.ISMSBOM_BOMName).FirstOrDefault();


                if (dto.ISMSLBOM_BOMFlg == "Proposal Customization")
                {
                    if(dto.BOMListt != null && dto.BOMListt.Length > 0)
                    {
                        int FmsInsertBomId = Convert.ToInt32(dto.BOMListt[0]);

                        var Fms = _vmsconte.Database.ExecuteSqlCommand("FMS_Correspondence_Lead_Insert @p0,@p1,@p2,@p3",
                                      dto.ISMSLE_Id, "Proposal Customization", dto.User_Id, FmsInsertBomId);
                        if (Fms > 0)
                        {

                        }
                    }
                    


                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Sales_Product_Templates";
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
                            dto.bomfinallist = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }



                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Sales_Product_package_detail_custom_prop";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                        {
                            Value = BOM_ids
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
                            dto.BOMTemplateList = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }


                    //mou number
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_cust_ProposalDet";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "H"
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
                            dto.perstudent_QutationReq = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    //Client specific changes
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_cust_ProposalDet";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "C"
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
                            dto.selectedleadlist = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }


                    //Product specific changes
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_cust_ProposalDet";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "P"
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
                            dto.Paymentterms = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }


                }
                else
                {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Sales_Product_package_detail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
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
                        dto.BOMTemplateList = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Sales_Product_package_detail_perstudent";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
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
                        dto.Sales_CostPerMonth = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Sales_Product_Templates";
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
                        dto.bomfinallist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //perstudent_QutationReq
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_BillOfMatrial";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
                    });
                    //@Flag
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
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
                        dto.perstudent_QutationReq = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //getLeadtarget_Mapping
                //selectedleadlist
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_ScicneceParkPerCost";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
                    });
                    //@Flag
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
                    //@CostPerStudent
                    cmd.Parameters.Add(new SqlParameter("@CostPerStudent", SqlDbType.VarChar)
                    {
                        Value = 0
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
                        dto.selectedleadlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //editleadtemp
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_ScincePark_BOOT_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLET_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
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
                        dto.editleadtemp = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //leadloaddata

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_ScincePark_MatrialName";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLET_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
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
                        dto.leadloaddata = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //leadTempDetails
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_RoboticPerCost";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
                    });
                    //@Flag
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
                    //@CostPerStudent
                    cmd.Parameters.Add(new SqlParameter("@CostPerStudent", SqlDbType.VarChar)
                    {
                        Value = 0
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
                        dto.leadTempDetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_ModuleInsert";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_ID", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
                    });
                    //User_Id
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
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
                        dto.ModuleName = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //ModuleName
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Qutation_Project_Payment";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_ID", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLET_Id
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
                        dto.Paymentterms = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

              }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public Lead_Target_MappingDTO Saveboomlistpackage(Lead_Target_MappingDTO dto)
        {
            try
            {

                if (dto.bomListdetails != null && dto.bomListdetails.Length > 0)
                {
                    foreach (var item in dto.bomListdetails)
                    {
                        var data = _vmsconte.ISM_Sales_Lead_BOMDMO.Single(t => t.ISPSIF_Id == item.ISPSIF_Id);
                        data.ISPSIF_ItemsName = item.ISPSIF_ItemsName;
                        data.ISPSIF_Quantity = item.ISPSIF_Quantity;
                        data.ISPSIF_Cost = item.ISPSIF_Cost;
                        _vmsconte.Update(data);
                    }
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "Error";
                    }

                }
                //
                else if (dto.SCINCE_Parks != null && dto.SCINCE_Parks.Length > 0)
                {
                    foreach (var c in dto.SCINCE_Parks)
                    {
                        //Scincepark_Data_Insert
                        var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("Scincepark_Data_Insert @p0,@p1,@p2,@p3",
                          c.ISPSIF_Id, c.ISPSIF_Quantity, c.ISPSIF_Cost, dto.User_Id);
                        if (contactExistsP > 0)
                        {

                        }
                    }
                    var BOM_ids = "0";
                    if (dto.BOMListt != null && dto.BOMListt.Length > 0)
                    {
                        foreach (var item in dto.BOMListt)
                        {
                            BOM_ids = BOM_ids + "," + item;
                        }

                    }
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_ScicneceParkPerCost";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                        {
                            Value = BOM_ids
                        });
                        //@Flag
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "1"
                        });
                        cmd.Parameters.Add(new SqlParameter("@CostPerStudent", SqlDbType.VarChar)
                        {
                            Value = 0
                        });
                        //@CostPerStudent

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
                            dto.selectedleadlist = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                //Robotic_Labs
                else if (dto.Robotic_Labs != null && dto.Robotic_Labs.Length > 0)
                {
                    foreach (var c in dto.Robotic_Labs)
                    {
                        //Scincepark_Data_Insert
                        var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("Robotic_Data_Insert @p0,@p1,@p2,@p3",
                          c.ISPSIF_Id, c.ISPSIF_Quantity, c.ISPSIF_Cost, dto.User_Id);
                        if (contactExistsP > 0)
                        {

                        }
                    }
                    var BOM_ids = "0";
                    if (dto.BOMListt != null && dto.BOMListt.Length > 0)
                    {
                        foreach (var item in dto.BOMListt)
                        {
                            BOM_ids = BOM_ids + "," + item;
                        }

                    }
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_RoboticPerCost";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                        {
                            Value = BOM_ids
                        });
                        //@Flag
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "1"
                        });
                        cmd.Parameters.Add(new SqlParameter("@CostPerStudent", SqlDbType.VarChar)
                        {
                            Value = 0
                        });
                        //@CostPerStudent

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
                            dto.selectedleadlist = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sales_Lead_Demo_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                        {
                            Value = dto.ISMSLE_Id
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
                            dto.productlist = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }


        public async Task<Lead_Target_MappingDTO> sendmailDetails(Lead_Target_MappingDTO data)
        {
            try
            {
                if (data.ISMSMOU_Packagename== "ProposalCustomization")
                {
                    try
                    {
                        whatsapp whtsappnew1 = new whatsapp(_db);

                        //
                        string fileid1 = "";
                        string filename1 = "Proposal";
                        string mailcc1 = "";
                        string mailbcc1 = "";
                        //string Template = "Client Quotation";
                        //string Template1 = "Proposal for  " + data.FirstName;
                        string Template1 = data.FirstName;

                        Dictionary<string, string> val1 = new Dictionary<string, string>();
                        var template1 = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "Client Quotation" && e.ISES_MailActiveFlag == true).ToList();
                        if (template1.Count == 0)
                        {
                            return data;
                        }

                        var institutionName1 = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                        var Paramaeters1 = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template1[0].ISES_Id
                        && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                        var ParamaetersName1 = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters1.Contains(i.ISMP_ID)).ToList();
                        string Mailcontent1 = template1.FirstOrDefault().ISES_SMSMessage;
                        string Mailmsg1 = template1.FirstOrDefault().ISES_MailHTMLTemplate;
                        string Resultsms1 = Mailcontent1;
                        string result1 = Mailmsg1;
                        List<Match> variables1 = new List<Match>();
                        foreach (Match match in Regex.Matches(Mailmsg1, @"\[.*?\]"))
                        {
                            variables1.Add(match);
                        }

                        Mailmsg1 = Mailmsg1.Replace("[MSG]", data.Templates);
                        Mailcontent1 = Resultsms1;
                        List<EMAIL_DETAILS_DMO> alldetails1 = new List<EMAIL_DETAILS_DMO>();
                        alldetails1 = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                        string Attechement1 = "";
                        try
                        {
                            List<ISM_Sales_Lead_Demo_DTO> dtonewtemp = new List<ISM_Sales_Lead_Demo_DTO>();
                            string CommandPrcodure = "";
                            if (data.ManagerMailid == "mktgcoord@vapstech.com")
                            {
                                 CommandPrcodure = "Fetch_ClientQuotation_DefaultCust";
                            }
                            else
                            {
                                //CommandPrcodure = "Fetch_ClientQuotation_Default";
                                CommandPrcodure = "Fetch_ClientQuotationccmailssCust";
                            }

                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = CommandPrcodure;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = 1
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
                                            dtonewtemp.Add(new ISM_Sales_Lead_Demo_DTO
                                            {
                                                return_status = Convert.ToString(dataReader["cc"]),
                                                ISMSLEDM_Remarks = Convert.ToString(dataReader["bcc"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (dtonewtemp.Count > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    mailcc1 = dtonewtemp[j].return_status;
                                    mailbcc1 = dtonewtemp[j].ISMSLEDM_Remarks;
                                }
                            }

                        }
                        catch (Exception error)
                        {
                            //
                        }
                        if (alldetails1.Count > 0)
                        {
                            string SendingEmail = "noreply@vapstech.com";
                            string SendingEmailPassword = alldetails1[0].IVRMMD_Mail_PASSWORD;
                            string SendingEmailHostName = alldetails1[0].IVRMMD_HOSTNAME;
                            Int32 PortNumber = Convert.ToInt32(alldetails1[0].IVRMMD_PORTNO);
                            //   string Subject = template[0].ISES_MailSubject.ToString();
                            //string Subject = "Proposal for VAPS " + data.FirstName;
                            string Subject =data.FirstName;
                            string sengridkey = alldetails1[0].IVRM_sendgridkey.ToString();
                            List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                            smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();
                            if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                            {

                                var message = new SendGridMessage();
                                message.From = new EmailAddress(SendingEmail, institutionName1[0].MI_Name);
                                message.Subject = Subject;
                                message.AddTo(data.ManagerMailid);
                                StringBuilder sb = new StringBuilder(data.Template);
                                StringReader sr = new StringReader(sb.ToString());
                                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                                HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                                if (mailcc1 != null && mailcc1 != "")
                                {
                                    string[] ccmaildetails = mailcc1.Split(',');
                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != data.ManagerMailid)
                                        {
                                            message.AddCc(c);
                                        }
                                    }
                                }

                                using (MemoryStream memoryStream = new MemoryStream())
                                {
                                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                    pdfDoc.Open();
                                    htmlparser.Parse(sr);
                                    pdfDoc.Close();
                                    byte[] bytess = memoryStream.ToArray();
                                    var file = Convert.ToBase64String(bytess);
                                    byte[] fileBytes = Convert.FromBase64String(file);
                                    string emp;
                                    emp = Convert.ToString(sr);
                                    string c = "";
                                    //string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");
                                    //message.AddAttachment(v, file);
                                    message.HtmlContent = Mailmsg1;
                                    var client = new SendGridClient(sengridkey);
                                    memoryStream.Close();
                                    client.SendEmailAsync(message).Wait();
                                    var datavalues = file.Substring(file.IndexOf(',') + 1);
                                    var byteArray = Convert.FromBase64String(datavalues);
                                    using (var stream = new MemoryStream(byteArray))
                                    {
                                        IFormFile formFile = new FormFile(stream, 0, byteArray.Length, "file", "Proposal.pdf")
                                        {
                                            Headers = new HeaderDictionary(),
                                            ContentType = "application/pdf"
                                        };
                                        string accountname = "";
                                        string accesskey = "";
                                        var azure = _db.IVRM_Storage_path_Details.ToList();
                                        if (azure.Count() > 0)
                                        {
                                            accountname = azure.FirstOrDefault().IVRM_SD_Access_Name;
                                            accesskey = azure.FirstOrDefault().IVRM_SD_Access_Key;
                                        }
                                        var names = new List<string>();
                                        var contentTypes = new List<string>();
                                        fileid1 = await whtsappnew1.uploadtoazure(formFile, "presales", "salesQutations", data.MI_Id, accountname, accesskey);
                                        data.ISMSLET_HtmlPdf = fileid1;
                                    }
                                }
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
                                        emailMessage.To.Add(new MailAddress(data.ManagerMailid));
                                        emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                        emailMessage.Subject = Subject;
                                        emailMessage.Body = Mailmsg1;
                                        emailMessage.IsBodyHtml = true;
                                        if (Attechement1.Equals("1"))
                                        {
                                            var img1 = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template1[0].ISES_Id).ToList();
                                            if (img1.Count > 0)
                                            {
                                                for (int i = 0; i < img1.Count; i++)
                                                {
                                                    foreach (var attache in img1.ToList())
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

                                        if (mailcc1 != null && mailcc1 != "")
                                        {
                                            string[] ccmaildetails = mailcc1.Split(',');

                                            foreach (var c in ccmaildetails)
                                            {
                                                emailMessage.CC.Add(c);
                                            }
                                        }
                                        if (mailbcc1 != null && mailbcc1 != "")
                                        {
                                            string[] bccmaildetails = mailbcc1.Split(',');

                                            foreach (var c in bccmaildetails)
                                            {
                                                emailMessage.Bcc.Add(c);
                                            }
                                        }
                                        //adedd

                                        //addd
                                        clientsmtp.Send(emailMessage);





                                    }
                                }
                            }

                            if (data.ManagerMailid == "mktgcoord@vapstech.com")
                            {
                                //var datainsert = _vmsconte.Database.ExecuteSqlCommand("Pre_Enhance_Proposal_QuatationUpdate @p0,@p1",
                                //data.ISMSLET_Id, true);
                                //if (datainsert > 0)
                                //{

                                //}
                            }
                            else
                            {
                                var datainsert = _vmsconte.Database.ExecuteSqlCommand("Pre_Enhance_Proposal_QuatationUpdate @p0,@p1",
                                 data.ISMSLET_Id, true);
                                if (datainsert > 0)
                                {

                                }
                            }




                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    whatsapp whtsappnew = new whatsapp(_db);

                    //
                    string fileid = "";
                    string filename = "Proposal";
                    string mailcc = "";
                    string mailbcc = "";
                    //string Template = "Client Quotation";
                    string Template = "Proposal for  " + data.FirstName;

                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "Client Quotation" && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count == 0)
                    {
                        return data;
                    }

                    var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                    var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id
                    && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                    var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                    string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                    string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                    string Resultsms = Mailcontent;
                    string result = Mailmsg;
                    List<Match> variables = new List<Match>();
                    foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                    {
                        variables.Add(match);
                    }

                    Mailmsg = Mailmsg.Replace("[MSG]", data.Templates);
                    Mailcontent = Resultsms;
                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                    string Attechement = "";
                    try
                    {
                        List<ISM_Sales_Lead_Demo_DTO> dtonewtemp = new List<ISM_Sales_Lead_Demo_DTO>();
                        string CommandPrcodure = "";
                        if (data.ManagerMailid == "mktgcoord@vapstech.com")
                        {
                            CommandPrcodure = "Fetch_ClientQuotation_Default";
                        }
                        else
                        {
                            //CommandPrcodure = "Fetch_ClientQuotation_Default";
                             CommandPrcodure = "Fetch_ClientQuotationccmailss";
                        }

                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = CommandPrcodure;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                            SqlDbType.BigInt)
                            {
                                Value = 1
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
                                        dtonewtemp.Add(new ISM_Sales_Lead_Demo_DTO
                                        {
                                            return_status = Convert.ToString(dataReader["cc"]),
                                            ISMSLEDM_Remarks = Convert.ToString(dataReader["bcc"])
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        if (dtonewtemp.Count > 0)
                        {
                            for (int j = 0; j < dtonewtemp.Count; j++)
                            {
                                mailcc = dtonewtemp[j].return_status;
                                mailbcc = dtonewtemp[j].ISMSLEDM_Remarks;
                            }
                        }

                    }
                    catch (Exception error)
                    {
                        //
                    }
                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = "noreply@vapstech.com";
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        //   string Subject = template[0].ISES_MailSubject.ToString();
                        string Subject = "Proposal for VAPS " + data.FirstName;
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                        smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();
                        if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                        {

                            var message = new SendGridMessage();
                            message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                            message.Subject = Subject;
                            message.AddTo(data.ManagerMailid);
                            StringBuilder sb = new StringBuilder(data.Template);
                            StringReader sr = new StringReader(sb.ToString());
                            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                            if (mailcc != null && mailcc != "")
                            {
                                string[] ccmaildetails = mailcc.Split(',');
                                foreach (var c in ccmaildetails)
                                {
                                    if (c != data.ManagerMailid)
                                    {
                                        message.AddCc(c);
                                    }
                                }
                            }

                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                pdfDoc.Open();
                                htmlparser.Parse(sr);
                                pdfDoc.Close();
                                byte[] bytess = memoryStream.ToArray();
                                var file = Convert.ToBase64String(bytess);
                                byte[] fileBytes = Convert.FromBase64String(file);
                                string emp;
                                emp = Convert.ToString(sr);
                                string c = "";
                                string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");
                                message.AddAttachment(v, file);
                                message.HtmlContent = Mailmsg;
                                var client = new SendGridClient(sengridkey);
                                memoryStream.Close();
                                client.SendEmailAsync(message).Wait();
                                var datavalues = file.Substring(file.IndexOf(',') + 1);
                                var byteArray = Convert.FromBase64String(datavalues);
                                using (var stream = new MemoryStream(byteArray))
                                {
                                    IFormFile formFile = new FormFile(stream, 0, byteArray.Length, "file", "Proposal.pdf")
                                    {
                                        Headers = new HeaderDictionary(),
                                        ContentType = "application/pdf"
                                    };
                                    string accountname = "";
                                    string accesskey = "";
                                    var azure = _db.IVRM_Storage_path_Details.ToList();
                                    if (azure.Count() > 0)
                                    {
                                        accountname = azure.FirstOrDefault().IVRM_SD_Access_Name;
                                        accesskey = azure.FirstOrDefault().IVRM_SD_Access_Key;
                                    }
                                    var names = new List<string>();
                                    var contentTypes = new List<string>();
                                    fileid = await whtsappnew.uploadtoazure(formFile, "presales", "salesQutations", data.MI_Id, accountname, accesskey);
                                    data.ISMSLET_HtmlPdf = fileid;
                                }
                            }
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
                                    emailMessage.To.Add(new MailAddress(data.ManagerMailid));
                                    emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                    emailMessage.Subject = Subject;
                                    emailMessage.Body = Mailmsg;
                                    emailMessage.IsBodyHtml = true;
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

                                    if (mailcc != null && mailcc != "")
                                    {
                                        string[] ccmaildetails = mailcc.Split(',');

                                        foreach (var c in ccmaildetails)
                                        {
                                            emailMessage.CC.Add(c);
                                        }
                                    }
                                    if (mailbcc != null && mailbcc != "")
                                    {
                                        string[] bccmaildetails = mailbcc.Split(',');

                                        foreach (var c in bccmaildetails)
                                        {
                                            emailMessage.Bcc.Add(c);
                                        }
                                    }
                                    //adedd

                                    //addd
                                    clientsmtp.Send(emailMessage);





                                }
                            }
                        }

                        if (data.bomListdetails != null && data.bomListdetails.Length > 0)
                        {
                            var count = 0;
                            foreach (var item in data.bomListdetails)
                            {
                                count = count + 1;
                                var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("MultipleLead_Quatation_Insert @p0,@p1,@p2,@p3,@p4",
                                  item.ISMSPS_ID, data.ISMSLET_Id, item.ISPSIF_Quantity, count, data.User_Id);
                                if (contactExistsP > 0)
                                {

                                }
                            }

                        }
                        if (data.ManagerMailid == "mktgcoord@vapstech.com")
                        {

                        }
                        else
                        {
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                if (data.ISMSLE_Id > 0)
                                {
                                    var res = _vmsconte.ISM_Sales_Lead_DMO_con.Single(a => a.ISMSLE_Id == data.ISMSLE_Id);
                                    res.ISMSLE_QuotationSent = true;

                                    _vmsconte.Update(res);
                                    var vv = _vmsconte.SaveChanges();
                                    if (vv > 0)
                                    {
                                        data.retrunMsg = "Update";
                                    }
                                    else
                                    {
                                        data.retrunMsg = "Error";
                                    }
                                    //added New Things
                                    var update = _vmsconte.ISM_Sales_Lead_TemplateDMO_con.Single(a => a.ISMSLET_Id == data.ISMSLET_Id);
                                    if (update != null)
                                    {
                                        update.ISMSLET_HtmlTemplate = data.ISMSLET_HtmlTemplate;
                                        update.ISMSLET_HtmlPdf = data.ISMSLET_HtmlPdf;
                                        update.ISMSLET_QuotationSent = true;
                                        update.ISMSLET_UpdatedDate = DateTime.Now;
                                        _vmsconte.Update(update);
                                        var Slt = _vmsconte.SaveChanges();

                                    }


                                }
                                if (data.retrunwhatsup == "whatsup")
                                {
                                    //data.ISMSLED_MobileNo = "7619656037";

                                    string MI_Name = _db.Institute.Where(R => R.MI_Id == data.MI_Id).FirstOrDefault().MI_Name;
                                    string ss = await whtsappnew.Qutation(data.MI_Id, data.ISMSLED_MobileNo, "Client Quotation", filename, fileid, data.FirstName, MI_Name);
                                }
                                //"Client Quotation"
                                var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                                var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                                var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
                                cmd.CommandText = "IVRM_Email_Outgoing";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@EmailId",
                                    SqlDbType.NVarChar)
                                {
                                    Value = data.ManagerMailid
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
                                    Console.WriteLine(ex);
                                }
                            }
                        }

                        //added By New table 

                    }
                }


                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Lead_Target_MappingDTO Edit_LeadTargetMapping(Lead_Target_MappingDTO dto)
        {
            try
            {
                dto.editLeadTarget_Mapping = _vmsconte.ISM_Sales_Lead_TargetMappingDMO.Where(b => b.MI_Id == dto.MI_Id && b.ISMSLETARM_Id == dto.ISMSLETARM_Id && b.ISMSLETARM_ApprovedBy == dto.ISMSLETARM_ApprovedBy).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public Lead_Target_MappingDTO OnChangeLTM_Institution(Lead_Target_MappingDTO dto)
        {
            try
            {
                dto.leadtargetMapping_lts = _vmsconte.ISM_Sales_Lead_TargetMappingDMO.Where(b => b.MI_Id == dto.MI_Id).ToList().ToArray();


                if (dto.bomListdetails != null && dto.bomListdetails.Length > 0)
                {
                    var BOM_ids = "0";
                    foreach (var item in dto.bomListdetails)
                    {
                        var data = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Single(t => t.ISMSLEDMPR_Id == item.ISMSMPR_Id);
                        data.ISMSBOM_Id = item.ISMSBOM_Id;
                        data.ISMSLEDMPR_UpdatedBy = dto.User_Id;
                        data.UpdatedDate = DateTime.Now;
                        _vmsconte.Update(data);
                        BOM_ids = BOM_ids + "#" + item.ISMSBOM_Id;

                        var Fms = _vmsconte.Database.ExecuteSqlCommand("FMS_Correspondence_Lead_Insert @p0,@p1,@p2,@p3",
                                  item.ISMSLE_Id, item.ISMSMPR_ProductName, dto.User_Id, item.ISMSBOM_Id);
                        if (Fms > 0)
                        {

                        }
                    }
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.retrunMsg = "Add";
                        if (dto.ISMSLET_Id > 0)
                        {
                            var update = _vmsconte.ISM_Sales_Lead_TemplateDMO_con.Single(a => a.ISMSLET_Id == dto.ISMSLET_Id);
                            update.ISMSBOM_Id = BOM_ids;
                            update.ISMSLET_UpdatedBy = dto.User_Id;
                            update.ISMSLET_UpdatedDate = DateTime.Now;
                            _vmsconte.Update(update);
                            var Slt = _vmsconte.SaveChanges();
                        }


                    }
                    else
                    {
                        dto.retrunMsg = "Error";
                    }

                }
                else
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Sales_Payment_detail";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
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
                            dto.leadlist = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                dto.retrunMsg = "admin";
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public async Task<Lead_Target_MappingDTO> sendProDetails(Lead_Target_MappingDTO data)
        {
            try
            {
                

                if (data.flag == "ProposalCustomization")
                {

                    int productid = Convert.ToInt32(data.product[0]);
                    int bomid = Convert.ToInt32(data.BOMListt[0]);
                    //foreach(var dda in data.bomListdetails)
                    //{

                    var datainsert = _vmsconte.Database.ExecuteSqlCommand("Pre_Enhance_Proposal_Insert @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9",
                                  data.ISMSLE_Id, data.ISMSLBOM_BOMDescription, productid,data.User_Id, bomid,"I","Pending",0,"",0);
                        if (datainsert > 0)
                        {
                            data.retrunMsg = "Add";
                        }

                    //}


                }
                else
                {
                    whatsapp whtsappnew = new whatsapp(_db);
                    StringBuilder sb = new StringBuilder(data.Template);
                    StringReader sr = new StringReader(sb.ToString());
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);

                    string fileid = "";
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        byte[] bytess = memoryStream.ToArray();
                        var file = Convert.ToBase64String(bytess);
                        byte[] fileBytes = Convert.FromBase64String(file);
                        string emp;
                        emp = Convert.ToString(sr);
                        string c = "";
                        string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");

                        var datavalues = file.Substring(file.IndexOf(',') + 1);
                        var byteArray = Convert.FromBase64String(datavalues);
                        using (var stream = new MemoryStream(byteArray))
                        {
                            IFormFile formFile = new FormFile(stream, 0, byteArray.Length, "file", "Proposal.pdf")
                            {
                                Headers = new HeaderDictionary(),
                                ContentType = "application/pdf"
                            };
                            string accountname = "";
                            string accesskey = "";
                            var azure = _db.IVRM_Storage_path_Details.ToList();
                            if (azure.Count() > 0)
                            {
                                accountname = azure.FirstOrDefault().IVRM_SD_Access_Name;
                                accesskey = azure.FirstOrDefault().IVRM_SD_Access_Key;
                            }
                            var names = new List<string>();
                            var contentTypes = new List<string>();
                            fileid = await whtsappnew.uploadtoazure(formFile, "files", "Trnsportdocuments", data.MI_Id, accountname, accesskey);
                            data.ISMSLET_HtmlPdf = fileid;
                        }
                    }
                    var product_ids = "0";
                    string ISMSMPR_Id = "0";
                    if (data.product.Length > 0)
                    {
                        foreach (var item in data.product)
                        {
                            product_ids = product_ids + "#" + item;
                            ISMSMPR_Id = ISMSMPR_Id + ',' + item;
                        }

                    }
                    var BOM_ids = "0";
                    string ISMSBOM_Id = "0";
                    if (data.BOMListt.Length > 0)
                    {
                        foreach (var item in data.BOMListt)
                        {
                            BOM_ids = BOM_ids + "#" + item;
                            ISMSBOM_Id = ISMSBOM_Id + "," + item;
                        }

                    }


                    if (data.ISMSLET_Id > 0)
                    {

                        var update = _vmsconte.ISM_Sales_Lead_TemplateDMO_con.Single(a => a.ISMSLET_Id == data.ISMSLET_Id);
                        if (update != null)
                        {
                            update.ISMSLET_Amount_Old = update.ISMSLET_Amount_I;
                            update.ISMSLET_Amount_Final_old = update.ISMSLET_Amount_Final;
                            update.ISMSLET_HtmlTemplate = data.ISMSLET_HtmlTemplate;
                            update.ISMSLET_HtmlPdf = data.ISMSLET_HtmlPdf;
                            update.ISMSLET_HtmlName = data.ISMSLET_HtmlName;
                            update.ISMSLET_Amount_I = data.ISMSLET_Amount_I;
                            update.ISMSLET_Amount_Final = data.ISMSLET_Amount_Final;
                            update.ISMSLET_ActiveFlag = true;
                            update.ISMSLET_UpdatedDate = DateTime.Now;
                            update.ISMSLET_UpdatedBy = data.User_Id;
                            update.ISMSMPR_Id = product_ids;
                            update.ISMSBOM_Id = BOM_ids;
                            // update.ISMSLET_Cost = data.ISMSLET_Cost;
                            var Slt = _vmsconte.SaveChanges();

                            if (Slt > 0)
                            {
                                data.retrunMsg = "Update";
                                if (data.ISMSLET_HtmlName == "Medical Delite Plus Package" || data.ISMSLET_HtmlName == "Engineering Delite Plus Package" || data.ISMSLET_HtmlName == "Degree Delite Plus Package" || data.ISMSLET_HtmlName == "Degree Delite Plus Package" || data.ISMSLET_HtmlName == "Elite Package" || data.ISMSLET_HtmlName == "Delite  Package" || data.ISMSLET_HtmlName == "Delite Plus Package" || data.ISMSLET_HtmlName == "Delite Plus Package")
                                {
                                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "VMS_Sales_Product_package_Update_AMt";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                                        {
                                            Value = update.ISMSLE_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                                        {
                                            Value = BOM_ids
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FirstYrAmtPerChild", SqlDbType.Decimal)
                                        {
                                            Value = decimal.Parse(update.ISMSLET_Amount_Old)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@AftFirstYrAmtPerChild", SqlDbType.Decimal)
                                        {
                                            Value = decimal.Parse(update.ISMSLET_Amount_Final_old)
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FirstYrChangeAmtPerChild", SqlDbType.Decimal)
                                        {
                                            Value = decimal.Parse(data.ISMSLET_Amount_I)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@AftFirstYrChangeAmtPerChild", SqlDbType.Decimal)
                                        {
                                            Value = decimal.Parse(data.ISMSLET_Amount_Final)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@StudentStregth", SqlDbType.Int)
                                        {
                                            Value = (int)data.ISMSLE_StudentStrength
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar)
                                        {
                                            Value = data.User_Id.ToString()
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@MI_id", SqlDbType.BigInt)
                                        {
                                            Value = data.MI_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id", SqlDbType.BigInt)
                                        {
                                            Value = data.ISMSLET_Id
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
                                            data.confirmList = retObject.ToArray();
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }

                                    }

                                }
                                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "ISM_Sales_CostUpdate";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id", SqlDbType.BigInt)
                                    {
                                        Value = data.ISMSLET_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                                    {
                                        Value = ISMSBOM_Id
                                    });
                                    //ISMSMPR_Id
                                    cmd.Parameters.Add(new SqlParameter("@ISMSMPR_Id", SqlDbType.VarChar)
                                    {
                                        Value = ISMSMPR_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                                    {
                                        Value = data.ISMSLE_Id
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

                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }

                                }
                            }
                            else
                            {
                                data.retrunMsg = "Error";
                            }

                        }

                    }

                    else
                    {
                        ISM_Sales_Lead_TemplateDMO obj = new ISM_Sales_Lead_TemplateDMO();
                        obj.ISMSLE_Id = data.ISMSLE_Id;
                        obj.USER_Id = data.User_Id;
                        obj.ISMSLET_HtmlTemplate = data.ISMSLET_HtmlTemplate;
                        obj.ISMSLET_HtmlPdf = data.ISMSLET_HtmlPdf;
                        obj.ISMSLET_HtmlName = data.ISMSLET_HtmlName;
                        obj.ISMSMPR_Id = product_ids;
                        obj.ISMSBOM_Id = BOM_ids;
                        obj.ISMSLET_Status = "Pending";
                        obj.ISMSLET_CreatedDate = DateTime.Now;
                        obj.ISMSLET_UpdatedDate = DateTime.Now;
                        obj.ISMSLET_CreatedBy = data.User_Id;
                        obj.ISMSLET_UpdatedBy = data.User_Id;
                        obj.ISMSLET_Amount_I = data.ISMSLET_Amount_I;
                        obj.ISMSLET_Amount_Final = data.ISMSLET_Amount_Final;
                        obj.ISMSLET_Amount_Old = data.preAmount_I;
                        obj.ISMSLET_Amount_Final_old = data.PreAmount_Final;
                        obj.ISMSLET_ActiveFlag = true;
                        obj.ISMSLET_flag = "Q";
                        //  obj.ISMSLET_Cost = data.ISMSLET_Cost;
                        _vmsconte.Add(obj);
                        var Slt = _vmsconte.SaveChanges();
                        data.ISMSLET_Id = obj.ISMSLET_Id;

                        if (Slt > 0)
                        {
                            data.retrunMsg = "Add";
                            if (data.ISMSLET_HtmlName == "Medical Delite Plus Package" || data.ISMSLET_HtmlName == "Engineering Delite Plus Package" || data.ISMSLET_HtmlName == "Degree Delite Plus Package" || data.ISMSLET_HtmlName == "Degree Delite Plus Package" || data.ISMSLET_HtmlName == "Elite Package" || data.ISMSLET_HtmlName == "Delite  Package" || data.ISMSLET_HtmlName == "Delite Plus Package" || data.ISMSLET_HtmlName == "Delite Plus Package")
                            {
                                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "VMS_Sales_Product_package_Update_AMt";
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                                    {
                                        Value = data.ISMSLE_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                                    {
                                        Value = BOM_ids
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FirstYrAmtPerChild", SqlDbType.Decimal)
                                    {
                                        Value = decimal.Parse(data.preAmount_I)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@AftFirstYrAmtPerChild", SqlDbType.Decimal)
                                    {
                                        Value = decimal.Parse(data.PreAmount_Final)
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FirstYrChangeAmtPerChild", SqlDbType.Decimal)
                                    {
                                        Value = decimal.Parse(data.ISMSLET_Amount_I)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@AftFirstYrChangeAmtPerChild", SqlDbType.Decimal)
                                    {
                                        Value = decimal.Parse(data.ISMSLET_Amount_Final)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@StudentStregth", SqlDbType.Int)
                                    {
                                        Value = (int)data.ISMSLE_StudentStrength
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar)
                                    {
                                        Value = data.User_Id.ToString()
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_id", SqlDbType.BigInt)
                                    {
                                        Value = data.MI_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id", SqlDbType.BigInt)
                                    {
                                        Value = data.ISMSLET_Id
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
                                        data.confirmList = retObject.ToArray();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }

                                }


                            }
                            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_Sales_CostUpdate";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id", SqlDbType.BigInt)
                                {
                                    Value = data.ISMSLET_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                                {
                                    Value = ISMSBOM_Id
                                });
                                //ISMSMPR_Id
                                cmd.Parameters.Add(new SqlParameter("@ISMSMPR_Id", SqlDbType.VarChar)
                                {
                                    Value = ISMSMPR_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                                {
                                    Value = data.ISMSLE_Id
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

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }

                            }
                        }
                        else
                        {
                            data.retrunMsg = "Error";
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

        //SendMdApproval
        public async Task<Lead_Target_MappingDTO> SendMdApproval(Lead_Target_MappingDTO data)
        {
            try
            {
                if (data.ISMSLET_Id > 0)
                {
                    whatsapp whtsappnew = new whatsapp(_db);
                    StringBuilder sb = new StringBuilder(data.Template);
                    StringReader sr = new StringReader(sb.ToString());
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                    string fileid = "";
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        byte[] bytess = memoryStream.ToArray();
                        var file = Convert.ToBase64String(bytess);
                        byte[] fileBytes = Convert.FromBase64String(file);
                        string emp;
                        emp = Convert.ToString(sr);
                        string c = "";
                        string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");

                        var datavalues = file.Substring(file.IndexOf(',') + 1);
                        var byteArray = Convert.FromBase64String(datavalues);
                        using (var stream = new MemoryStream(byteArray))
                        {
                            IFormFile formFile = new FormFile(stream, 0, byteArray.Length, "file", "Proposal.pdf")
                            {
                                Headers = new HeaderDictionary(),
                                ContentType = "application/pdf"
                            };
                            string accountname = "";
                            string accesskey = "";
                            var azure = _db.IVRM_Storage_path_Details.ToList();
                            if (azure.Count() > 0)
                            {
                                accountname = azure.FirstOrDefault().IVRM_SD_Access_Name;
                                accesskey = azure.FirstOrDefault().IVRM_SD_Access_Key;
                            }
                            var names = new List<string>();
                            var contentTypes = new List<string>();
                            fileid = await whtsappnew.uploadtoazure(formFile, "files", "Trnsportdocuments", data.MI_Id, accountname, accesskey);
                            data.ISMSLET_HtmlPdf = fileid;
                        }
                    }

                    var obj = _vmsconte.ISM_Sales_Lead_TemplateDMO_con.Where(R => R.ISMSLET_Id == data.ISMSLET_Id).FirstOrDefault();
                    obj.ISMSLET_HtmlTemplate = data.ISMSLET_HtmlTemplate;
                    obj.ISMSLET_HtmlPdf = data.ISMSLET_HtmlPdf;
                    obj.ISMSLET_HtmlName = data.ISMSLET_HtmlName;
                    obj.ISMSBOM_ApprovedBy = data.User_Id;
                    obj.ISMSLET_ApprovedRemark = data.ISMSLET_ApprovedRemark;
                    obj.ISMSLET_Status = "Approved";
                    _vmsconte.Update(obj);
                    var Slt = _vmsconte.SaveChanges();
                    if (Slt > 0)
                    {
                        data.retrunMsg = "Add";
                    }
                    else
                    {
                        data.retrunMsg = "Error";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<Lead_Target_MappingDTO> MdApproval(Lead_Target_MappingDTO data)
        {
            try
            {

                if (data.flag == "ProposalCustomization")
                {
                    

                    foreach (var item in data.approvTempList)
                    {

                        var dataupdate = _vmsconte.Database.ExecuteSqlCommand("Pre_Enhance_Proposal_Insert @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9",
                           data.ISMSLE_Id, "", 0, data.User_Id, 0, "U", item.ISMSLBOM_BOMName, item.ISMSLET_Id,item.ISMSLET_ApprovedRemark, Convert.ToDecimal(item.ISMSLET_CostApproved));
                        if (dataupdate > 0)
                        {
                            data.retrunMsg = "Add";
                        }

                    }


                        

                }
                else
                {
                    if (data.ISMSLET_Id > 0)
                    {
                        whatsapp whtsappnew = new whatsapp(_db);
                        StringBuilder sb = new StringBuilder(data.Template);
                        StringReader sr = new StringReader(sb.ToString());
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                        string fileid = "";
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            byte[] bytess = memoryStream.ToArray();
                            var file = Convert.ToBase64String(bytess);
                            byte[] fileBytes = Convert.FromBase64String(file);
                            string emp;
                            emp = Convert.ToString(sr);
                            string c = "";
                            string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");

                            var datavalues = file.Substring(file.IndexOf(',') + 1);
                            var byteArray = Convert.FromBase64String(datavalues);
                            using (var stream = new MemoryStream(byteArray))
                            {
                                IFormFile formFile = new FormFile(stream, 0, byteArray.Length, "file", "Proposal.pdf")
                                {
                                    Headers = new HeaderDictionary(),
                                    ContentType = "application/pdf"
                                };
                                string accountname = "";
                                string accesskey = "";
                                var azure = _db.IVRM_Storage_path_Details.ToList();
                                if (azure.Count() > 0)
                                {
                                    accountname = azure.FirstOrDefault().IVRM_SD_Access_Name;
                                    accesskey = azure.FirstOrDefault().IVRM_SD_Access_Key;
                                }
                                var names = new List<string>();
                                var contentTypes = new List<string>();
                                fileid = await whtsappnew.uploadtoazure(formFile, "files", "Trnsportdocuments", data.MI_Id, accountname, accesskey);
                                data.ISMSLET_HtmlPdf = fileid;
                            }
                        }

                    }

                    if (data.approvTempList != null && data.approvTempList.Length > 0)
                    {
                        foreach (var item in data.approvTempList)
                        {
                            var obj = _vmsconte.ISM_Sales_Lead_TemplateDMO_con.Where(R => R.ISMSLET_Id == item.ISMSLET_Id).FirstOrDefault();
                            obj.ISMSBOM_ApprovedBy = data.User_Id;
                            obj.ISMSLET_ApprovedRemark = item.ISMSLET_ApprovedRemark;
                            obj.ISMSLET_Amount_IMD = item.ISMSLET_Amount_IMD;
                            obj.ISMSLET_Amount_FinalMD = item.ISMSLET_Amount_FinalMD;
                            obj.ISMSLET_Status = item.flag;
                            obj.ISMSLET_CostApproved = item.ISMSLET_CostApproved;
                            obj.ISMSLET_UpdatedDate = DateTime.Now;
                            _vmsconte.Update(obj);
                            var Slt = _vmsconte.SaveChanges();
                            if (Slt > 0)
                            {
                                var leadlist = _vmsconte.ISM_Sales_Lead_DMO_con.Where(R => R.ISMSLE_Id == obj.ISMSLE_Id).ToList();
                                data.retrunMsg = "Add";
                                if (obj.ISMSLET_HtmlName == "Medical Delite Plus Package" || obj.ISMSLET_HtmlName == "Engineering Delite Plus Package" || obj.ISMSLET_HtmlName == "Degree Delite Plus Package" || obj.ISMSLET_HtmlName == "Degree Delite Plus Package" || obj.ISMSLET_HtmlName == "Elite Package" || obj.ISMSLET_HtmlName == "Delite  Package" || obj.ISMSLET_HtmlName == "Delite Plus Package" || obj.ISMSLET_HtmlName == "Delite Plus Package")
                                {
                                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "VMS_Sales_Product_package_Update_AMt";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                                        {
                                            Value = obj.ISMSLE_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                                        {
                                            Value = obj.ISMSBOM_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FirstYrAmtPerChild", SqlDbType.Decimal)
                                        {
                                            Value = decimal.Parse(obj.ISMSLET_Amount_I)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@AftFirstYrAmtPerChild", SqlDbType.Decimal)
                                        {
                                            Value = decimal.Parse(obj.ISMSLET_Amount_Final)
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FirstYrChangeAmtPerChild", SqlDbType.Decimal)
                                        {
                                            Value = decimal.Parse(obj.ISMSLET_Amount_IMD)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@AftFirstYrChangeAmtPerChild", SqlDbType.Decimal)
                                        {
                                            Value = decimal.Parse(obj.ISMSLET_Amount_FinalMD)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@StudentStregth", SqlDbType.Int)
                                        {
                                            Value = (int)leadlist.FirstOrDefault().ISMSLE_StudentStrength
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar)
                                        {
                                            Value = data.User_Id.ToString()
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@MI_id", SqlDbType.BigInt)
                                        {
                                            Value = leadlist.FirstOrDefault().MI_Id

                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id", SqlDbType.BigInt)
                                        {
                                            Value = obj.ISMSLET_Id
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

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }

                                    }

                                }
                                if (item.SCINCE_Parks != null && item.SCINCE_Parks.Length > 0)
                                {
                                    foreach (var d in item.SCINCE_Parks)
                                    {
                                        var Payment_Update = _vmsconte.Database.ExecuteSqlCommand("VMS_ScincePark_Payment_Update @p0,@p1,@p2,@p3", d.ISPSIF_Id, d.ISPSIF_Cost, d.ISPSIF_Quantity, d.ISPSIF_ItemsName);
                                        if (Payment_Update > 0)
                                        {

                                        }
                                    }
                                }

                                //SciencePark_Boot
                                if (item.SciencePark_Boot != null && item.SciencePark_Boot.Length > 0)
                                {
                                    foreach (var d in item.SciencePark_Boot)
                                    {
                                        var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("VMS_ScincePark_BOOT_Insert @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9",
                                item.ISMSLET_Id, item.ISMSLBOM_BOMName, d.ISMLESPB_First_Year, d.ISMLESPB_Second_Year, d.ISMLESPB_Third_Year,
                                d.ISMLESPB_Fourth_Year, d.ISMLESPB_Fifth_Year, d.ISMLESPB_Flag, data.User_Id, d.ISMLESPB_FlagName);
                                        if (contactExistsP > 0)
                                        {

                                        }
                                    }
                                }
                                if (item.ISMSLBOM_BOMName == "BOTH" || item.ISMSLBOM_BOMName == "BOOTMODEL")
                                {
                                    var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("VMS_ScincePark_BOOT_Update @p0,@p1,@p2",
                                 item.ISMSLET_Id, item.ISMSLBOM_BOMName, data.User_Id);
                                    if (contactExistsP > 0)
                                    {

                                    }
                                }
                                if (item.ISMSLBOM_BOMName == "ROBOTICBOTH" || item.ISMSLBOM_BOMName == "ROBOTICBOOT")
                                {
                                    var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("VMS_ROBOTIC_BOOT_Update @p0,@p1,@p2",
                                 item.ISMSLET_Id, item.ISMSLBOM_BOMName, data.User_Id);
                                    if (contactExistsP > 0)
                                    {

                                    }
                                }
                            }
                            else
                            {
                                data.retrunMsg = "Error";
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
        public Lead_Target_MappingDTO deactivate(Lead_Target_MappingDTO data)
        {
            try
            {



                if (data.ISMSLBOM_BOMName == "ProposalCustomization")
                {
                    var datainsert = _vmsconte.Database.ExecuteSqlCommand("Pre_Enhance_Proposal_ActiveDeactive @p0",
                                 data.ISMSLET_Id);
                    if (datainsert > 0)
                    {
                        data.retrunMsg = "true";
                    }
                    else
                    {
                        data.retrunMsg = "false";
                    }
                }

                else
                {
                    if (data.ISMSLET_Id > 0)
                    {
                        var query = _vmsconte.ISM_Sales_Lead_TemplateDMO_con.Single(s => s.ISMSLET_Id == data.ISMSLET_Id);

                        if (query.ISMSLET_ActiveFlag == true)
                        {
                            query.ISMSLET_ActiveFlag = false;
                        }
                        else
                        {
                            query.ISMSLET_ActiveFlag = true;
                        }

                        _vmsconte.Update(query);
                        var contactExists = _vmsconte.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "true";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }

                    else
                    {
                        var BOM_ids = "0";
                        if (data.BOMListt != null && data.BOMListt.Length > 0)
                        {
                            foreach (var item in data.BOMListt)
                            {
                                BOM_ids = BOM_ids + "," + item;
                            }

                        }
                        if (data.SCINCE_Parks != null && data.SCINCE_Parks.Length > 0)
                        {
                            foreach (var c in data.SCINCE_Parks)
                            {
                                //Scincepark_Data_Insert
                                var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("Scincepark_Data_Insert @p0,@p1,@p2,@p3",
                                  c.ISPSIF_Id, c.ISPSIF_Quantity, c.ISPSIF_Cost, data.User_Id);
                                if (contactExistsP > 0)
                                {

                                }
                            }
                        }

                        //perstudent_QutationReq
                        using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Sales_BillOfMatrial";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                            {
                                Value = data.ISMSLE_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                            {
                                Value = BOM_ids
                            });
                            //@Flag
                            cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                            {
                                Value = "1"
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
                                data.perstudent_QutationReq = retObject.ToArray();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "usp_s_ScicneceParkPerCost";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                            {
                                Value = data.ISMSLE_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                            {
                                Value = BOM_ids
                            });
                            //@Flag
                            cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                            {
                                Value = "2"
                            });
                            //@CostPerStudent
                            cmd.Parameters.Add(new SqlParameter("@CostPerStudent", SqlDbType.VarChar)
                            {
                                Value = data.Negotiation_Amount
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
                                data.selectedleadlist = retObject.ToArray();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
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

        public Lead_Target_MappingDTO getdetails(int id)
        {
            Lead_Target_MappingDTO cate = new Lead_Target_MappingDTO();
            try
            {
                List<ISM_Sales_Lead_TemplateDMO> lorg = new List<ISM_Sales_Lead_TemplateDMO>();
                lorg = _vmsconte.ISM_Sales_Lead_TemplateDMO_con.Where(t => t.ISMSLET_Id == id).ToList();
                cate.editleadtemp = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return cate;
        }

        //adde Mou-Negtition Mail
        public string sendmails(long MI_Id, string Email, string Template, long UserID, long user_id, string ISMSALENA_StatusFlg, string ISMSALENA_SanctionedAmount, string ISMSALENA_Remarks)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id
                && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
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

                    cmd.CommandText = "SALES_MOU_NegationParameter";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLET_Id",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSALENA_StatusFlg",
                       SqlDbType.VarChar)
                    {
                        Value = ISMSALENA_StatusFlg
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSALENA_SanctionedAmount",
                      SqlDbType.VarChar)
                    {
                        Value = ISMSALENA_SanctionedAmount
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSALENA_Remarks",
                    SqlDbType.VarChar)
                    {
                        Value = ISMSALENA_Remarks
                    });


                    //obj.ISMSALENA_SanctionedAmount
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
                        return ex.Message;
                    }

                }
                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
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
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                try
                {

                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                       
                        // mailcc = alldetails[0].IVRM_mailcc;//cc mail id
                        mailcc = Email;//cc mail id
                    }
                    else
                    {
                      
                        mailcc = Email;//cc mail id
                    }
                }
                catch (Exception error)
                {
                    //
                }
                string subjects = "";
                if (ISMSALENA_StatusFlg == "Pending")
                {
                    subjects = "Negotiation";
                }
                else
                {
                    subjects = "MOU";
                }
                if (alldetails.Count > 0)
                {
                    // string SendingEmail = "noreply@vapstech.com";
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString() + " " + subjects;
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        // message.AddTo(Email);//to address mail id
                        // message.AddTo("manohara.nm@vapstech.com");
                        //message.AddTo("rameshbabu@vapstech.com");
                        //"nagaraja.mv@vapstech.com", 
                        message.AddTo("pandith.sharma@vapstech.com");
                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }
                        if (mailcc != null && mailcc != "")
                        {
                            string[] ccmaildetails = mailcc.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                //if (c != Email)
                                {
                                    message.AddCc(c);
                                }
                            }
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            string[] bccmaildetails = mailbcc.Split(',');

                            foreach (var c in bccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddBcc(c);
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
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
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

                                if (mailcc != null && mailcc != "")
                                {
                                    string[] ccmaildetails = mailcc.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    string[] bccmaildetails = mailbcc.Split(',');

                                    foreach (var c in bccmaildetails)
                                    {
                                        emailMessage.Bcc.Add(c);
                                    }
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
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


        public async Task<Lead_Target_MappingDTO> SaveScienceParkTransaction(Lead_Target_MappingDTO data)
        {
            try
            {
                if (data.goldandsilverData != null && data.goldandsilverData.Length > 0)
                {
                    foreach (var item in data.goldandsilverData)
                    {
                        try
                        {

                            ISM_SciencePark_GoldAndSilver_Package_TransactionDMO TransactionDmo = new ISM_SciencePark_GoldAndSilver_Package_TransactionDMO();
                            TransactionDmo.ISGSISMSBOM_Id = item.ISMSBOM_Id;
                            TransactionDmo.ISGSISMSLE_Id = item.ISMSLE_Id;
                            TransactionDmo.ISGSISMSPSI_ItemFlag = item.ISMSPSI_ItemFlag;
                            TransactionDmo.ISGSISMSPSI_ItemsDescription = item.ISMSPSI_ItemsDescription;
                            TransactionDmo.ISGSIMSV_TotalCostPerQty = item.IMSV_TotalCostPerQty;
                            TransactionDmo.ISGSISPSIF_AmountScincePark = item.ISPSIF_AmountScincePark;
                            TransactionDmo.ISGSISPSIF_AmtScincePark = item.ISPSIF_AmtScincePark;
                            TransactionDmo.ISGSISPSIF_Cost = item.ISPSIF_Cost;
                            TransactionDmo.ISGSISPSIF_CostNew = item.ISPSIF_CostNew;
                            TransactionDmo.ISGSISPSIF_ItemsName = item.ISPSIF_ItemsName;
                            TransactionDmo.ISGSISPSIF_Quantity = item.ISPSIF_Quantity;
                            TransactionDmo.ISGSISPSIF_Quantitys = item.ISPSIF_Quantitys;
                            TransactionDmo.ISGSScienceParkFlag = item.ScienceParkFlag;
                            TransactionDmo.CreatedBy = data.User_Id;
                            TransactionDmo.UpdatedBy = data.User_Id;
                            TransactionDmo.CreatedDate = DateTime.Now;
                            TransactionDmo.UpdatedDate = DateTime.Now;
                            _vmsconte.Add(TransactionDmo);
                            var i = _vmsconte.SaveChanges();
                            if (i > 0)
                            {
                                data.retrunMsg = "Add";
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                    }
                }
                if (data.goldandsilverYrwiseData != null && data.goldandsilverYrwiseData.Length > 0)
                {
                    foreach (var item in data.goldandsilverYrwiseData)
                    {
                        try
                        {
                            ISM_SciencePark_TransactionDMO dmo = new ISM_SciencePark_TransactionDMO();
                            dmo.ItemName = item.ItemName;
                            dmo.Cost = item.Cost;
                            dmo.FirstYear = item.FirstYear;
                            dmo.FirstYearNew = item.FirstYearNew;
                            dmo.SecondYear = item.SecondYear;
                            dmo.SecondYearNew = item.SecondYearNew;
                            dmo.ThirdYear = item.ThirdYear;
                            dmo.ThirdYearNew = item.ThirdYearNew;
                            dmo.FourYear = item.FourYear;
                            dmo.FourYearNew = item.FourYearNew;
                            dmo.FifthYear = item.FifthYear;
                            dmo.FifthYearNew = item.FifthYearNew;
                            dmo.ISMSBOM_Id = item.ISMSBOM_Id;
                            dmo.ISMSLE_Id = item.ISMSLE_Id;
                            dmo.PakageType = item.PakageType;
                            dmo.ScienceParkFlag = item.ScienceParkFlag;
                            dmo.CreatedBy = data.User_Id;
                            dmo.UpdatedBy = data.User_Id;
                            dmo.CreatedDate = DateTime.Now;
                            dmo.UpdatedDate = DateTime.Now;
                            _vmsconte.Add(dmo);
                            var i = _vmsconte.SaveChanges();
                            if (i > 0)
                            {
                                data.retrunMsg = "Add";
                            }
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<Lead_Target_MappingDTO> SaveISMSalesMultipleTransaction(Lead_Target_MappingDTO data)
        {
            try
            {
                if (data.ISMSalesMultipleTransaction != null && data.ISMSalesMultipleTransaction.Length > 0)
                {
                    foreach (var item in data.ISMSalesMultipleTransaction)
                    {
                        try
                        {
                            ISM_Sales_Multiple_TransactionDMO TransactionDmo = new ISM_Sales_Multiple_TransactionDMO();
                            TransactionDmo.ISMSBOM_Id = item.ISMSBOM_Id;
                            TransactionDmo.ISMMT_BOMName = item.ISMSBOM_Name;
                            TransactionDmo.ISMSLE_Id = item.ISMSLE_Id;
                            TransactionDmo.ISMSPSI_ID = item.ISMSPSI_ID;
                            TransactionDmo.ISPSIF_Id = item.ISPSIF_Id;
                            TransactionDmo.ISMSLET_Id = item.ISMSLET_Id;
                            TransactionDmo.ISMMT_ItemsName = item.ISPSIF_ItemsName;
                            TransactionDmo.ISMMT_ItemFlag = item.ISMSPSI_ItemFlag;
                            TransactionDmo.ISMMT_ItemsDescription = item.ISMSPSI_ItemsDescription;
                            TransactionDmo.ISMMT_CostAprdMD = item.ISPSIF_CostAppMd;
                            TransactionDmo.ISMMT_CostOriginal = item.ISPSIF_CostOriginal;
                            TransactionDmo.ISMMT_Quantitys = item.ISPSIF_Quantitys;
                            TransactionDmo.ISMMT_Quantity = item.ISPSIF_Quantity;
                            TransactionDmo.ISMMT_TotalPRCost = item.TotalCost;
                            TransactionDmo.ISMMT_ActiveFlag = true;
                            TransactionDmo.ISMMT_CreatedBy = data.User_Id;
                            TransactionDmo.ISMMT_UpdatedBy = data.User_Id;
                            TransactionDmo.ISMMT_CreatedDate = DateTime.Now;
                            TransactionDmo.ISMMT_UpdatedDate = DateTime.Now;
                            _vmsconte.Add(TransactionDmo);
                            var i = _vmsconte.SaveChanges();
                            if (i > 0)
                            {
                                data.retrunMsg = "Add";
                            }
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
//




//

