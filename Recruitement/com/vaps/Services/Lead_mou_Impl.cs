using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.Sales;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
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
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Lead_mou_Impl : Interfaces.Lead_mou_Interface
    {

        public VMSContext _vmsconte;
        public HRMSContext _hrmscon;
        private DomainModelMsSqlServerContext _db;
        public Lead_mou_Impl(VMSContext vmsContext, HRMSContext hrms, DomainModelMsSqlServerContext db)
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
                    cmd.CommandText = "ISM_Sales_MOU_Details";
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
                        dto.mousavedeatils = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //added by praveenkumar //@Flag

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Client_Lead_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
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
                        dto.leadloaddata = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Client_Lead_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
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
                        dto.MouApproval = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //added by praveenkumar approval below grid 

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_SALES_MD_Approval_NEW";
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
                        dto.leadloaddata = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //approval page grid here 

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_SALES_MD_Approvall_NEW";
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
                        dto.leadtargetMapping_lts = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //orderacceptance after approval 
                using ( var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Client_Project_DocsApprovalList";
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
                        dto.Orderacceptanceapproval = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



                //signed mou procedure after uploading 
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Client_Project_Docs";
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
                        dto.Signedmoudetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_MOU";
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
                        dto.Mompendinglist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                List<MasterAcademic> list1 = new List<MasterAcademic>();
                list1 = _vmsconte.AcademicYear.Where(t => t.MI_Id == dto.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                dto.yearlist = list1.ToArray();


                dto.getinstallment = (from a in _vmsconte.FeeInstallmentDMO
                                       from b in _vmsconte.FeeInstallmentsyearlyDMO
                                       where (a.FMI_Id == b.FMI_Id && a.FMI_ActiceFlag == true && a.MI_Id == dto.MI_Id && b.MI_ID == dto.MI_Id)
                                       select b).Distinct().OrderBy(a => a.FTI_Name).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


        public Lead_Target_MappingDTO lead_product(Lead_Target_MappingDTO dto)
        {

            try
            {
                using ( var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_MOU_product";
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
                        dto.Momproductpendinglist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_MOU_product_Order_Acceptance";
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
                        dto.Momproductpendinglist1 = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                // IVRM PRODUCT LIST ADDED BY PRAVEENKUMAR
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_LeadModuleList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                      SqlDbType.VarChar)
                    {
                        Value = "M"
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



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_LeadModuleList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                      SqlDbType.VarChar)
                    {
                        Value = "E"
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
                        dto.ModuleName1 = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_MOU_product";
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
                        dto.Mompendinglist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_leadwise_Project_Payment";
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
                        dto.leadprojectpayment = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                //added on 21012025
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "USP_s_Post_OrderAccepteance_Detailed";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLET_ID", SqlDbType.BigInt)
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
                        dto.approvedBy = retObject.ToArray();
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


        public Lead_Target_MappingDTO savedata(Lead_Target_MappingDTO dto)
        {

            try
            {
                if (dto.ISMSMOUA_Id !=0)
                {



                }
                else
                {
                    
                    ISM_Sales_Lead_MOU obj = new ISM_Sales_Lead_MOU();
                    obj.ISMSLE_Id = dto.ISMSLE_Id;
                    obj.ISMSMOU_Studentstrength = dto.ISMSMOU_Studentstrength;
                    obj.ISMSMOU_Amount = dto.ISMSMOU_Amount;
                    obj.ISMSMOU_Packagename = dto.ISMSMOU_Packagename;
                    obj.ISMSMOUA_ProductName = dto.ISMSMOUA_ProductName;
                    obj.ISMSMOU_CreatedDate = DateTime.Now;
                    obj.ISMSMOU_ActiveFlag = true;
                    _vmsconte.Add(obj);

                        if (dto.ISM_Sales_Lead_MOU_Attachments != null && dto.ISM_Sales_Lead_MOU_Attachments.Length > 0)
                        {
                            foreach (var itm in dto.ISM_Sales_Lead_MOU_Attachments)
                            {
                            ISM_Sales_Lead_MOU_Attachments had = new ISM_Sales_Lead_MOU_Attachments();
                                had.ISMSMOU_Id = obj.ISMSMOU_Id;
                                had.ISMSMOUA_AttachmentName = itm.ISMSMOUA_AttachmentName;
                                had.ISMSMOUA_AttachmentPath = itm.ISMSMOUA_AttachmentPath;
                                had.ISMSMOUA_ActiveFlag = true;
                                had.ISMSMOUA_CreatedDate = DateTime.Now;
                                _vmsconte.Add(had);
                            }
                        }
                        var contactExists = _vmsconte.SaveChanges();
                        if (contactExists > 0)
                        {
                        dto.msg = "save";

                        }
                        else
                        {
                        dto.msg = "Notsave";
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
        
        public Lead_Target_MappingDTO MOUViewdata(Lead_Target_MappingDTO dto)
        {

            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_MOU_Details_Attachments";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSMOU_Id", SqlDbType.VarChar)
                    {
                        Value = dto.ISMSMOU_Id
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
                        dto.mouattachmentsdeatils = retObject.ToArray();
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


        public Lead_Target_MappingDTO sendmail(Lead_Target_MappingDTO data)
        {
            try
            {
                foreach (var i in data.ISMSMOU_Ids)
                {




                    var tenantnotice = (from a in _vmsconte.ISM_Sales_Lead_MOU
                                        from b in _vmsconte.ISM_Sales_Lead_DMO_con
                                        where (a.ISMSLE_Id == b.ISMSLE_Id  && a.ISMSMOU_ActiveFlag == true)
                                        select b).Distinct().ToArray();

                    //foreach (var j in tenantnotice)
                    //{

                    //    var tenantdetails = (from b in _context.TenantNoticeBoardTenantDMO
                    //                         from g in _context.TenantNoticeBoardDMO
                    //                         from d in _context.TenantNoticeBoardFilesDMO
                    //                         from a in _context.TenantAgreement
                    //                         from c in _context.HR_Master_Building
                    //                         from e in _context.HR_Master_Room
                    //                         where (a.MI_Id == data.MI_Id && b.TENENTNTB_Id == j.TENENTNTB_Id && d.TENENTNTB_Id == g.TENENTNTB_Id && b.TENANTAGR_Id == a.TENANTAGR_Id && a.TENANTAGR_Id == j.TENANTAGR_Id && a.TENANTAGR_ActiveFlg == true && b.TENENTNTB_Id == g.TENENTNTB_Id &&
                    //                         c.HRMB_Id == a.HRMB_Id && g.HRMB_Id == c.HRMB_Id && e.HRMR_Id == a.HRMR_Id)
                    //                         select new TenantAgreementDTO
                    //                         {
                    //                             TENANTAGR_Id = a.TENANTAGR_Id,
                    //                             TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                    //                             TENENTNTB_Id = b.TENENTNTB_Id,
                    //                             TENANTAGR_Remarks = g.TENENTNTB_Title,
                    //                             TENANTAGRREN_Remarks = g.TENENTNTB_Description,
                    //                             returnval = b.TENANTNTBTNT_ActiveFlag,
                    //                             TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                    //                             HRMB_BuildingName = c.HRMB_BuildingName,
                    //                             HRMR_RoomName = e.HRMR_RoomName,
                    //                             TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                    //                             TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                    //                             TENANTAGR_Address = a.TENANTAGR_Address,
                    //                             TENENTNTBFL_FilePath = d.TENENTNTBFL_FilePath


                    //                         }).ToArray();



                    //    string emailccdetails = "";
                    //    string emailbccdetails = "";


                    //    var checktemplate = _vmsconte.SMSEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "TENANTNOTICEBOARD"
                    //    && a.ISES_MailActiveFlag == true).ToList();

                    //    DateTime now = DateTime.Now;
                    //    string strDate = now.ToString("dd-MM-yyyy");

                    //    if (checktemplate.Count > 0)
                    //    {
                    //        emailccdetails = checktemplate.FirstOrDefault().ISES_MailCCId;
                    //        emailbccdetails = checktemplate.FirstOrDefault().ISES_MailBCCId;
                    //    }
                    //    else
                    //    {
                    //        emailccdetails = "";
                    //        emailbccdetails = "";
                    //    }

                    //    if (checktemplate.Count == 0)
                    //    {
                    //        data.retrunMsg = "Email Template not Mapped to the selected Institution";
                    //    }
                    //    else
                    //    {

                    //        string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                    //        if (templatedetails.Contains("[TENANTNAME]"))
                    //        {
                    //            templatedetails = templatedetails.Replace("[TENANTNAME]", tenantdetails[0].TENANTAGR_TenantName.ToUpper());
                    //        }
                    //        if (templatedetails.Contains(" [TITLE]"))
                    //        {
                    //            templatedetails = templatedetails.Replace(" [TITLE]", tenantdetails[0].TENANTAGR_Remarks.ToUpper());
                    //        }
                    //        if (templatedetails.Contains("[DESCRIPTION]"))
                    //        {
                    //            templatedetails = templatedetails.Replace("[DESCRIPTION]", tenantdetails[0].TENANTAGRREN_Remarks);
                    //        }
                    //        if (templatedetails.Contains("[FLAT]"))
                    //        {
                    //            templatedetails = templatedetails.Replace("[FLAT]", tenantdetails[0].HRMR_RoomName.ToString());
                    //        }
                    //        if (templatedetails.Contains("[BUILDING]"))
                    //        {
                    //            templatedetails = templatedetails.Replace("[BUILDING]", tenantdetails[0].HRMB_BuildingName.ToString());
                    //        }
                    //        if (templatedetails.Contains("[ADDRESS]"))
                    //        {
                    //            templatedetails = templatedetails.Replace("[ADDRESS]", tenantdetails[0].TENANTAGR_Address.ToString());
                    //        }
                    //        if (templatedetails.Contains("[STARTDATE]"))
                    //        {
                    //            templatedetails = templatedetails.Replace("[STARTDATE]", tenantdetails[0].TENANTAGR_AgrFromDate.ToString("dd-MM-yyyy"));
                    //        }
                    //        if (templatedetails.Contains("[DATE1]"))
                    //        {

                    //            templatedetails = templatedetails.Replace("[DATE1]", strDate);
                    //        }

                    //        try
                    //        {
                    //            data.retrunMsg = sendmailexamtt(data.MI_Id, tenantdetails[0].TENANTAGR_EmailId.ToString(), templatedetails, emailccdetails, emailbccdetails, tenantdetails[0].TENENTNTBFL_FilePath, "TENANTNOTICEBOARD");
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Console.WriteLine(ex.Message);
                    //        }

                    //    }


                    //}
                }


            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                data.retrunMsg = ex.Message;
            }



            return data;

        }

        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails, string files, string temp)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _vmsconte.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == temp
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = url;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _vmsconte.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _vmsconte.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    string mailcc = "";
                    string mailbcc = "";

                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }




                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(mail_id);
                    //  message.AddTo("kavitajnandoor21@gmail.com");

                    if (files != null && files != "")
                    {
                        var webClient = new WebClient();
                        byte[] imageBytes = webClient.DownloadData(files);

                        string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);

                        message.AddAttachment(files, fileContentsAsBase64, null, null, null);
                    }

                    if (Attechement.Equals("1"))
                    {
                        var img = _vmsconte.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                    //******************* EMAIL CC DETAILS ****************//

                    if (emailccdetails != null && emailccdetails != "")
                    {
                        string[] ccmaildetails = emailccdetails.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }

                    //******************** Attachment ******************* //





                    //******************* EMAIL BCC DETAILS ****************//




                    //if (mailcc != null && mailcc != "")
                    //{
                    //    message.AddCc(mailcc);
                    //}
                    //if (mailbcc != null && mailbcc != "")
                    //{
                    //    message.AddBcc(mailbcc);
                    //}

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    try
                    {
                        client.SendEmailAsync(message).Wait();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "TENANTNOTICEBOARD" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";

        }
    }
}
