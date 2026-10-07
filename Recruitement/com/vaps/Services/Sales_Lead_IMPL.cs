using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonLibrary;
using System.Text;
using System.Reflection.Metadata;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Recruitment.com.vaps.Services
{
    public class Sales_Lead_Master_IMPL : Interfaces.Sales_Lead_Master_Interface
    {
        public VMSContext _vmsconte;
        public HRMSContext _hrmscon;
        private DomainModelMsSqlServerContext _db;
        public IssueManagerContext _IssueContext;
        public Sales_Lead_Master_IMPL(VMSContext vmsContext, HRMSContext hrms, DomainModelMsSqlServerContext db, IssueManagerContext issueManager)
        {
            _vmsconte = vmsContext;
            _hrmscon = hrms;
            _db = db;
            _IssueContext = issueManager;
        }
        //===============================Category==============================================
        public ISM_Sales_Master_Category_DTO get_load_Cat(int id)
        {
            ISM_Sales_Master_Category_DTO CD = new ISM_Sales_Master_Category_DTO();
            try
            {
                CD.category_list = _vmsconte.ISM_Sales_Lead_TargetMapping_DMO.Where(a => a.MI_Id == id).ToList().ToArray();

                CD.institutionlist = _vmsconte.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                CD.MI_Id = id;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return CD;
        }
        public ISM_Sales_Master_Category_DTO Edit_details_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            try
            {
                dto.edit_category_list = _vmsconte.ISM_Sales_Lead_TargetMapping_DMO.Where(t => t.ISMSMCA_Id == dto.ISMSMCA_Id
                && t.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Category_DTO SaveEdit_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            try
            {
                ISM_Sales_Master_Category_DMO CD = new ISM_Sales_Master_Category_DMO();

                if (dto.ISMSMCA_Id > 0)
                {
                    var result = _vmsconte.ISM_Sales_Lead_TargetMapping_DMO.Single(t => t.ISMSMCA_Id == dto.ISMSMCA_Id);
                    result.ISMSMCA_CategoryName = dto.ISMSMCA_CategoryName;
                    result.ISMSMCA_CategoryType = dto.ISMSMCA_CategoryType;
                    result.ISMSMCA_Remarks = dto.ISMSMCA_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.UpdatedBy = dto.userId;
                    result.ISMSMCA_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else
                {
                    var result = _vmsconte.ISM_Sales_Lead_TargetMapping_DMO.Where(a=>a.ISMSMCA_CategoryName== dto.ISMSMCA_CategoryName && a.ISMSMCA_CategoryType==dto.ISMSMCA_CategoryType && a.ISMSMCA_Remarks== dto.ISMSMCA_Remarks).ToList();
                    if (result.Count > 0)
                    {
                        dto.returnvalue = "Duplicate";
                    }
                    else
                    {
                        CD.ISMSMCA_CategoryName = dto.ISMSMCA_CategoryName;
                        CD.ISMSMCA_CategoryType = dto.ISMSMCA_CategoryType;
                        CD.ISMSMCA_Remarks = dto.ISMSMCA_Remarks;
                        CD.MI_Id = dto.MI_Id;
                        CD.ISMSMCA_ActiveFlag = true;
                        CD.CreatedBy = dto.userId;
                        CD.ISMSMCA_CreatedDate = DateTime.Now;
                        _vmsconte.Add(CD);
                        _vmsconte.SaveChanges();
                        dto.returnvalue = "Add";
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Category_DTO deactivate_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Lead_TargetMapping_DMO.Single(t => t.ISMSMCA_Id == dto.ISMSMCA_Id);

                if (dto.ISMSMCA_ActiveFlag == true)
                {
                    result.ISMSMCA_ActiveFlag = false;
                    result.UpdatedBy = dto.userId;
                    result.ISMSMCA_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {
                    result.ISMSMCA_ActiveFlag = true;
                    result.UpdatedBy = dto.userId;
                    result.ISMSMCA_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }
        public ISM_Sales_Master_Category_DTO OnChangeCategoryInstitution(ISM_Sales_Master_Category_DTO dto)
        {
            try
            {
                dto.category_list = _vmsconte.ISM_Sales_Lead_TargetMapping_DMO.Where(a => a.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        //===============================Product==============================================
        public ISM_Sales_Master_Product_DTO get_load_pro(int id)
        {
            ISM_Sales_Master_Product_DTO CD = new ISM_Sales_Master_Product_DTO();
            try
            {
                CD.product_list = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == id).ToList().ToArray();
                CD.MI_Id = id;
                CD.institutionlist = _vmsconte.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return CD;
        }
        public ISM_Sales_Master_Product_DTO Edit_details_pro(ISM_Sales_Master_Product_DTO dto)
        {
            try
            {
                dto.edit_product_list = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(t => t.ISMSMPR_Id == dto.ISMSMPR_Id
                && t.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Product_DTO SaveEdit_pro(ISM_Sales_Master_Product_DTO dto)
        {
            try
            {
                ISM_Sales_Master_Product_DMO CD = new ISM_Sales_Master_Product_DMO();

                if (dto.ISMSMPR_Id > 0)
                {
                    var result = _vmsconte.ISM_Sales_Master_Product_DMO_con.Single(t => t.ISMSMPR_Id == dto.ISMSMPR_Id);
                    result.ISMSMPR_ProductName = dto.ISMSMPR_ProductName;
                    result.ISMSMPR_Remarks = dto.ISMSMPR_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.ISMSMPR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    CD.ISMSMPR_ProductName = dto.ISMSMPR_ProductName;
                    CD.ISMSMPR_Remarks = dto.ISMSMPR_Remarks;
                    CD.MI_Id = dto.MI_Id;
                    CD.ISMSMPR_ActiveFlag = true;
                    CD.ISMSMPR_CreatedBy = dto.userId;
                    CD.CreatedDate = DateTime.Now;
                    _vmsconte.Add(CD);
                    _vmsconte.SaveChanges();
                    dto.returnvales = "Add";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Product_DTO deactivate_pro(ISM_Sales_Master_Product_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Master_Product_DMO_con.Single(t => t.ISMSMPR_Id == dto.ISMSMPR_Id);

                if (dto.ISMSMPR_ActiveFlag == true)
                {
                    result.ISMSMPR_ActiveFlag = false;
                    result.ISMSMPR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {
                    result.ISMSMPR_ActiveFlag = true;
                    result.ISMSMPR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }
        public ISM_Sales_Master_Product_DTO OnChangeProductInstitution(ISM_Sales_Master_Product_DTO dto)
        {
            try
            {
                dto.product_list = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        //======================Source==================
        public ISM_Sales_Master_Source_DTO get_load_src(int id)
        {
            ISM_Sales_Master_Source_DTO CD = new ISM_Sales_Master_Source_DTO();
            try
            {
                CD.source_list = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == id).ToList().ToArray();
                CD.MI_Id = id;
                CD.institutionlist = _vmsconte.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return CD;
        }
        public ISM_Sales_Master_Source_DTO Edit_details_src(ISM_Sales_Master_Source_DTO dto)
        {
            try
            {
                dto.edit_source_list = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(t => t.ISMSMSO_Id == dto.ISMSMSO_Id && t.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Source_DTO SaveEdit_src(ISM_Sales_Master_Source_DTO dto)
        {
            try
            {
                ISM_Sales_Master_Source_DMO CD = new ISM_Sales_Master_Source_DMO();

                var duplicate = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(d => d.MI_Id == dto.MI_Id && d.ISMSMSO_SourceName == dto.ISMSMSO_SourceName && d.ISMSMSO_Remarks == dto.ISMSMSO_Remarks).ToList();
                if(duplicate.Count > 0 && duplicate != null)
                {
                    dto.returnvales = "Duplicate";
                }
                else
                {
                    if (dto.ISMSMSO_Id > 0)
                    {
                        var result = _vmsconte.ISM_Sales_Master_Source_DMO_con.Single(t => t.ISMSMSO_Id == dto.ISMSMSO_Id);
                        result.ISMSMSO_SourceName = dto.ISMSMSO_SourceName;
                        result.ISMSMSO_Remarks = dto.ISMSMSO_Remarks;
                        result.MI_Id = dto.MI_Id;
                        result.ISMSMSO_UpdatedBy = dto.userId;
                        result.UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Update";
                    }
                    else
                    {
                        CD.ISMSMSO_SourceName = dto.ISMSMSO_SourceName;
                        CD.ISMSMSO_Remarks = dto.ISMSMSO_Remarks;
                        CD.MI_Id = dto.MI_Id;
                        CD.ISMSMSO_ActiveFlag = true;
                        CD.ISMSMSO_CreatedBy = dto.userId;
                        CD.CreatedDate = DateTime.Now;
                        CD.ISMSMSO_UpdatedBy = dto.userId;
                        CD.UpdatedDate = DateTime.Now;
                        _vmsconte.Add(CD);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Add";
                    }
                }
               
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Source_DTO deactivate_src(ISM_Sales_Master_Source_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Master_Source_DMO_con.Single(t => t.ISMSMSO_Id == dto.ISMSMSO_Id);

                if (dto.ISMSMSO_ActiveFlag == true)
                {
                    result.ISMSMSO_ActiveFlag = false;
                    result.ISMSMSO_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {
                    result.ISMSMSO_ActiveFlag = true;
                    result.ISMSMSO_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }
        public ISM_Sales_Master_Source_DTO OnChangeSourceInstitution(ISM_Sales_Master_Source_DTO dto)
        {
            try
            {
                dto.source_list = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        //======================Status==================
        public ISM_Sales_Master_Status_DTO get_load_sts(int id)
        {
            ISM_Sales_Master_Status_DTO CD = new ISM_Sales_Master_Status_DTO();
            try
            {
                CD.status_list = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == id).ToList().ToArray();
                CD.institutionlist = _vmsconte.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                CD.MI_Id = id;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return CD;
        }
        public ISM_Sales_Master_Status_DTO Edit_details_sts(ISM_Sales_Master_Status_DTO dto)
        {
            try
            {
                dto.edit_status_list = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(t => t.ISMSMST_Id == dto.ISMSMST_Id && t.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Status_DTO SaveEdit_sts(ISM_Sales_Master_Status_DTO dto)
        {
            try
            {
                ISM_Sales_Master_Status_DMO CD = new ISM_Sales_Master_Status_DMO();

                if (dto.ISMSMST_Id > 0)
                {
                    var result = _vmsconte.ISM_Sales_Master_Status_DMO_con.Single(t => t.ISMSMST_Id == dto.ISMSMST_Id);
                    result.ISMSMST_StatusName = dto.ISMSMST_StatusName;
                    result.ISMSMST_Remarks = dto.ISMSMST_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.ISMSMST_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else
                {
                    var result = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a=>a.ISMSMST_StatusName== dto.ISMSMST_StatusName && a.ISMSMST_Remarks==dto.ISMSMST_Remarks).ToList();
                    if(result.Count > 0)
                    {
                        dto.returnvalue = "Duplicate";
                    }
                    else
                    {
                        CD.ISMSMST_StatusName = dto.ISMSMST_StatusName;
                        CD.ISMSMST_Remarks = dto.ISMSMST_Remarks;
                        CD.MI_Id = dto.MI_Id;
                        CD.ISMSMST_ActiveFlag = true;
                        CD.ISMSMST_CreatedBy = dto.userId;
                        CD.CreatedDate = DateTime.Now;
                        _vmsconte.Add(CD);
                        _vmsconte.SaveChanges();
                        dto.returnvalue = "Add";
                    }
                   
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Status_DTO deactivate_sts(ISM_Sales_Master_Status_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Master_Status_DMO_con.Single(t => t.ISMSMST_Id == dto.ISMSMST_Id);

                if (dto.ISMSMST_ActiveFlag == true)
                {
                    result.ISMSMST_ActiveFlag = false;
                    result.ISMSMST_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {
                    result.ISMSMST_ActiveFlag = true;
                    result.ISMSMST_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }
        public ISM_Sales_Master_Status_DTO OnChangeStatusInstitution(ISM_Sales_Master_Status_DTO dto)
        {
            try
            {
                dto.status_list = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToList().ToArray();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        //======================Lead==================
        public ISM_Sales_Lead_DTO load_all_ChannelPartner(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_Tellemp";
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
                    });
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
                        dto.personal_list_dd = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.ChannalPatnerList = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMCP_ActiveFlag == true).ToArray();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_Chnlptnr";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@USER_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.leadlists = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //LeadVerfiey //ISM_Sales_Lead_Verifi

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                  //  cmd.CommandText = "ISM_Sales_Lead_Tellemp";
                    cmd.CommandText = "ISM_Sales_Lead_Verifiy";
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
                    });
                    //@Flag
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "LEAD"
                    });
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
                        dto.LeadVerfiey = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //get_lead_detailsfinal
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    
                    cmd.CommandText = "ISM_Sales_Lead_Verifiy";
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
                    });
                    //@Flag
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "Raw_LEAD"
                    });
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
                        dto.get_lead_detailsfinal = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //personal_list_dd
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISM_Sales_Lead_Verifiy";
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
                    });
                    //@Flag
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "Raw_LEAD_Tel"
                    });
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
                        dto.personal_list_dd = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO getdetails(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                var ismsle_ids = "0";
                if (dto.Leadlsttwo.Length > 0)
                {
                    foreach (var ue in dto.Leadlsttwo)
                    {
                        ismsle_ids = ismsle_ids + "," + ue.ISMSLE_Id;

                    }

                }

                var ismsmcp_ids = "0";
                if (dto.Channellsttwo.Length > 0)
                {
                    foreach (var ue in dto.Channellsttwo)
                    {
                        ismsmcp_ids = ismsmcp_ids + "," + ue.ISMSMCP_Id;

                    }

                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_Chnlptnrwise";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSMCP_Id",
                     SqlDbType.VarChar)
                    {
                        Value = ismsmcp_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                    SqlDbType.VarChar)
                    {
                        Value = ismsle_ids
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
                        dto.lead_list2 = retObject.ToArray();
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
        public ISM_Sales_Lead_DTO get_lead(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                var ismsmcp_ids = "0";
                if (dto.Channellsttwo.Length > 0)
                {
                    foreach (var ue in dto.Channellsttwo)
                    {
                        ismsmcp_ids = ismsmcp_ids + "," + ue.ISMSMCP_Id;

                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_Chnlptnr";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSMCP_Id",
                    SqlDbType.VarChar)
                    {
                        Value = ismsmcp_ids
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
                        dto.leadlists = retObject.ToArray();
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
        public ISM_Sales_Lead_DTO viewdetails(ISM_Sales_Lead_DTO data)
        {
            try
            {
                //data.Leaddetails = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_Id == data.ISMSLE_Id).Distinct().ToArray();

                data.Leaddetails = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a =>a.ISMSLE_Id == data.ISMSLE_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Sales_Lead_DTO load_all_lead(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                dto.companylist = _vmsconte.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                dto.product_dd = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == 17 && a.ISMSMPR_ActiveFlag == true).ToArray();

                dto.category_dd = _vmsconte.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMCA_ActiveFlag == true).ToArray();

                dto.source_dd = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMSO_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

                dto.status_dd = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMST_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

                dto.country_dd = _hrmscon.IVRM_Master_Country.Where(a => a.IVRMMC_Id == 101).ToArray();


                var HRMEID = _hrmscon.Staff_User_Login.Where(t => t.Id == dto.user_id).Select(t => t.Emp_Code).FirstOrDefault();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_SalesLead_Details";
                    cmd.CommandType = CommandType.StoredProcedure;                 
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = HRMEID
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
                        dto.lead_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



                dto.ChannalPatner_List = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Where(a => a.MI_Id == dto.MI_Id).ToArray();
                dto.ChannalPatnerList = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMCP_ActiveFlag == true).ToArray();
               

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Employees";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.Telecaller = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_leadDetails";
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
                        dto.viewdetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                dto.leadlists = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true).ToArray();
                //finalleadlist

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_RawLeads";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
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
                        dto.finalleadlist = retObject.ToArray();
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
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO select_state(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                dto.state_dd = _hrmscon.IVRM_Master_State.Where(a => a.IVRMMC_Id == dto.IVRMMC_Id).ToArray();
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        
        public ISM_Sales_Lead_DTO select_District(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                dto.district_dd = _hrmscon.IVRM_Master_CityDMO.Where(a => a.IVRMMS_Id == dto.IVRMMS_Id).ToArray();
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO checkemailtemplet(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                var result1 = _vmsconte.ISM_Sales_Master_Source_DMO.Where(a => a.ISMSMSO_Id == dto.ISMSMSO_Id).ToList();
                if (result1.Count > 0)
                {
                    var template = _db.smsEmailSetting.Where(e => e.MI_Id == dto.MI_Id && e.ISES_Template_Name == result1.FirstOrDefault().ISMSMSO_Templet
                    && e.ISES_MailActiveFlag == true).ToList();

                    if (template.Count > 0)
                    {
                        dto.returnvalue = false;
                    }
                    else
                    {
                        dto.returnvalue = true;
                    }
                }
                else
                {
                    dto.returnvalue = true;
                }
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO getproduct(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                dto.product_dd = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMPR_ActiveFlag == true).ToArray();

                dto.category_dd = _vmsconte.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMCA_ActiveFlag == true).ToArray();

                dto.source_dd = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMSO_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

                dto.status_dd = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMST_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();
                var HRMEID = _hrmscon.Staff_User_Login.Where(t => t.Id == dto.user_id).Select(t => t.Emp_Code).FirstOrDefault();
                //dto.lead_list = (from b in _vmsconte.ISM_Sales_Master_Category_DMO_con
                //                 from c in _vmsconte.ISM_Sales_Master_Source_DMO_con
                //                 from d in _vmsconte.ISM_Sales_Lead_DMO_con
                //                 from e in _vmsconte.ISM_Sales_Master_Status_DMO_con
                //                 where d.ISMSMCA_Id == b.ISMSMCA_Id && d.ISMSMSO_Id == c.ISMSMSO_Id && d.MI_Id == dto.MI_Id && d.ISMSMST_Id == e.ISMSMST_Id && d.ISMSLE_ActiveFlag == true
                //                 select new ISM_Sales_Lead_DTO
                //                 {
                //                     ISMSLE_LeadName = d.ISMSLE_LeadName,
                //                     ISMSLE_LeadCode = d.ISMSLE_LeadCode,
                //                     ISMSMCA_CategoryName = b.ISMSMCA_CategoryName,
                //                     ISMSMSO_SourceName = c.ISMSMSO_SourceName,
                //                     ISMSMST_StatusName = e.ISMSMST_StatusName,
                //                     ISMSLE_ActiveFlag = d.ISMSLE_ActiveFlag,
                //                     ISMSLE_Remarks = d.ISMSLE_Remarks,
                //                     ISMSLE_Id = d.ISMSLE_Id,
                //                     MI_Id = d.MI_Id,
                //                     ISMSLE_VisitedDate = d.ISMSLE_VisitedDate
                //                 }).OrderByDescending(t => t.ISMSLE_VisitedDate).ToList().ToArray();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_SalesLead_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = HRMEID
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
                        dto.lead_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO Save_Edit_SaleLead(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                if (dto.ISMSLE_Id > 0)
                {

                    var duplicate = _vmsconte.ISM_Sales_Lead_DMO_con.Where(s => s.ISMSLE_Pincode == dto.ISMSLE_Pincode && s.ISMSLE_EmailId == dto.ISMSLE_EmailId && s.ISMSLE_ContactNo == dto.ISMSLE_ContactNo && s.ISMSLE_Id != dto.ISMSLE_Id).ToArray();
                    if (duplicate.Length == 0)
                    {


                        var result = _vmsconte.ISM_Sales_Lead_DMO_con.Single(a => a.ISMSLE_Id == dto.ISMSLE_Id);
                        var result2 = _vmsconte.ISM_Sales_Lead_Products_DMO_con.Where(a => a.ISMSLE_Id == dto.ISMSLE_Id).ToList();
                        foreach (var item in result2)
                        {
                            var result1 = _vmsconte.ISM_Sales_Lead_Products_DMO_con.Single(a => a.ISMSMPR_Id == item.ISMSMPR_Id && a.ISMSLE_Id == dto.ISMSLE_Id);
                            _vmsconte.Remove(result1);

                        }
                        result.ISMSMCP_Id = dto.ISMSMCP_Id;
                        result.HRME_Id = dto.HRME_Id;
                        result.ISMSMCP_ChannelPartnerFlg = dto.ISMSMCP_ChannelPartnerFlg;
                        result.ISMSLE_LeadName = dto.ISMSLE_LeadName;
                        result.ISMSLE_LeadCode = dto.ISMSLE_LeadCode;
                        result.ISMSLE_ContactPerson = dto.ISMSLE_ContactPerson;
                        result.ISMSLE_LeadAddress1 = dto.ISMSLE_LeadAddress1;
                        result.ISMSLE_LeadAddress2 = dto.ISMSLE_LeadAddress2;
                        result.ISMSLE_LeadAddress3 = dto.ISMSLE_LeadAddress3;
                        result.ISMSLE_VisitedDate = dto.ISMSLE_VisitedDate;
                        result.ISMSLE_Pincode = dto.ISMSLE_Pincode;
                        result.ISMSLE_ContactNo = dto.ISMSLE_ContactNo;
                        result.ISMSMST_Id = dto.ISMSMST_Id;
                        result.IVRMMC_Id = dto.IVRMMC_Id.Value;
                        result.ISMSLE_EmailId = dto.ISMSLE_EmailId;
                        result.ISMSMCA_Id = dto.ISMSMCA_Id;
                        result.ISMSMSO_Id = dto.ISMSMSO_Id;
                        result.ISMSLE_Reference = dto.ISMSLE_Reference;
                        result.ISMSLE_ContactDesignation = dto.ISMSLE_ContactDesignation;
                        result.IVRMMS_Id = dto.IVRMMS_Id;
                        result.IVRMMCT_Id = dto.IVRMMCT_Id;
                        result.ISMSLE_StudentStrength = dto.ISMSLE_StudentStrength;
                        result.ISMSLE_StaffStrength = dto.ISMSLE_StaffStrength;
                        result.ISMSLE_NoOfInstitutions = dto.ISMSLE_NoOfInstitutions;
                        result.ISMSLE_Remarks = dto.ISMSLE_Remarks;
                        result.UpdatedDate = DateTime.Now;
                        result.ISMSLE_UpdatedBy = dto.user_id;
                        result.ISMSLE_QuotationValue = dto.ISMSLE_QuotationValue;
                        result.ISMSLE_DedicatedHostingFlg = dto.ISMSLE_DedicatedHostingFlg;
                        result.ISMSLE_DedicatedMobileAppFlg = dto.ISMSLE_DedicatedMobileAppFlg;
                        result.ISMSLE_HostingServer = dto.ISMSLE_HostingServer;
                        result.ISMSLE_Board = dto.ISMSLE_Board;
                        _vmsconte.Update(result);

                        foreach (var item in dto.product_list_save)
                        {
                            ISM_Sales_Lead_Products_DMO ld = new ISM_Sales_Lead_Products_DMO();
                            ld.ISMSMPR_Id = item.ISMSMPR_Id;
                            ld.ISMSLE_Id = result.ISMSLE_Id;
                            ld.MI_Id = item.MI_Id;
                            ld.ISMSLEPR_ActiveFlag = true;
                            ld.UpdatedDate = DateTime.Now;
                            ld.ISMSLEPR_UpdatedBy = dto.user_id;
                            _vmsconte.Add(ld);
                        }

                       

                        _vmsconte.SaveChanges();
                        dto.return_value = "Update";

                        var result3 = _vmsconte.ISM_Sales_Master_Source_DMO.Where(a => a.ISMSMSO_Id == dto.ISMSMSO_Id).ToList();
                        if (result3.Count > 0)
                        {
                            var template = _db.smsEmailSetting.Where(e => e.MI_Id == dto.MI_Id && e.ISES_Template_Name == result3.FirstOrDefault().ISMSMSO_Templet && e.ISES_MailActiveFlag == true).ToList();
                            if (template.Count > 0)
                            {
                                if (dto.sendemail == true)
                                {
                                    if (dto.ISMSLE_EmailId != null && dto.ISMSLE_EmailId != "")
                                    {
                                        sendmail(dto.MI_Id, dto.ISMSLE_EmailId, result3.FirstOrDefault().ISMSMSO_Templet, dto.ISMSLE_Id);
                                    }
                                }
                            }
                        }
                        //sendmail(dto.MI_Id, dto.ISMSLE_EmailId, "LEAD_CREATE", dto.ISMSLE_Id);
                    }
                    else
                    {
                        dto.return_value = "Duplicate";
                    }
                }
                else
                {

                    dto.ISMSLERaw_flag = false;
                    var duplicate = _vmsconte.ISM_Sales_Lead_DMO_con.Where(s => s.ISMSLE_Pincode == dto.ISMSLE_Pincode && s.ISMSLE_EmailId == dto.ISMSLE_EmailId && s.ISMSLE_ContactNo == dto.ISMSLE_ContactNo && s.ISMSLE_ActiveFlag == true).ToList();

                    var duplicateMobile = _vmsconte.ISM_Sales_Lead_DMO_con.Where(R => R.ISMSLE_ContactNo == dto.ISMSLE_ContactNo && R.ISMSLE_ActiveFlag==true).ToList();
                    var duplicatelead = _vmsconte.ISM_Sales_Lead_DMO_con.Where(R => R.ISMSLE_LeadName == dto.ISMSLE_LeadName && R.IVRMMCT_Id== dto.IVRMMCT_Id && R.ISMSLE_ActiveFlag == true).ToList();

                    var duplicateEmail = _vmsconte.ISM_Sales_Lead_DMO_con.Where(R => R.ISMSLE_EmailId == dto.ISMSLE_EmailId && R.ISMSLE_ActiveFlag == true).ToList();
                    if(dto.ISMSLERaw_Id !="")
                    {
                        dto.ISMSLERaw_flag = true;
                    }
                    if (duplicate.Count > 0 && dto.ISMSLERaw_flag==false)
                    {
                        dto.return_value = "duplicate";
                        dto.ISMSLE_TypeFlg = "lead";
                    }
                   else if (duplicateMobile.Count > 0 && dto.ISMSLERaw_flag == false)
                    {
                        dto.return_value = "duplicate";
                        dto.ISMSLE_TypeFlg = "mobile";
                    }
                    else if (duplicatelead.Count > 0 && dto.ISMSLERaw_flag == false)
                    {
                        dto.return_value = "duplicate";
                        dto.ISMSLE_TypeFlg = "lead";
                    }
                    else if (duplicateEmail.Count > 0 && dto.ISMSLERaw_flag == false)
                    {
                        dto.return_value = "duplicate";
                        dto.ISMSLE_TypeFlg = "email";
                    }
                    else
                    {
                        ISM_Sales_Lead_DMO sl = new ISM_Sales_Lead_DMO();
                        sl.ISMSMCP_Id = dto.ISMSMCP_Id;
                        sl.HRME_Id = dto.HRME_Id;
                        sl.ISMSMCP_ChannelPartnerFlg = dto.ISMSMCP_ChannelPartnerFlg;
                        sl.ISMSLE_LeadName = dto.ISMSLE_LeadName;
                        sl.ISMSLE_LeadCode = dto.ISMSLE_LeadCode;
                        sl.ISMSLE_ContactPerson = dto.ISMSLE_ContactPerson;
                        sl.ISMSLE_LeadAddress1 = dto.ISMSLE_LeadAddress1;
                        sl.ISMSLE_LeadAddress2 = dto.ISMSLE_LeadAddress2;
                        sl.ISMSLE_LeadAddress3 = dto.ISMSLE_LeadAddress3;
                        sl.ISMSLE_VisitedDate = dto.ISMSLE_VisitedDate;
                        sl.ISMSLE_Pincode = dto.ISMSLE_Pincode;
                        sl.ISMSLE_ContactNo = dto.ISMSLE_ContactNo;
                        sl.ISMSMST_Id = dto.ISMSMST_Id;
                        sl.IVRMMC_Id = dto.IVRMMC_Id.Value;
                        sl.ISMSLE_EmailId = dto.ISMSLE_EmailId;
                        sl.ISMSMCA_Id = dto.ISMSMCA_Id;
                        sl.ISMSMSO_Id = dto.ISMSMSO_Id;
                        sl.ISMSLE_Reference = dto.ISMSLE_Reference;
                        sl.ISMSLE_ContactDesignation = dto.ISMSLE_ContactDesignation;
                        sl.IVRMMS_Id = dto.IVRMMS_Id;
                        sl.IVRMMCT_Id = dto.IVRMMCT_Id;
                        sl.ISMSLE_StudentStrength = dto.ISMSLE_StudentStrength;
                        sl.ISMSLE_StaffStrength = dto.ISMSLE_StaffStrength;
                        sl.ISMSLE_NoOfInstitutions = dto.ISMSLE_NoOfInstitutions;
                        sl.ISMSLE_Remarks = dto.ISMSLE_Remarks;
                        sl.MI_Id = dto.MI_Id;
                        sl.ISMSLE_ActiveFlag = true;
                        sl.CreatedDate = DateTime.Now;
                        sl.ISMSLE_CreatedBy = dto.user_id;
                        sl.ISMSLE_QuotationValue = dto.ISMSLE_QuotationValue;
                        sl.ISMSLE_DedicatedHostingFlg = dto.ISMSLE_DedicatedHostingFlg;
                        sl.ISMSLE_DedicatedMobileAppFlg = dto.ISMSLE_DedicatedMobileAppFlg;
                        sl.ISMSLE_HostingServer = dto.ISMSLE_HostingServer;
                        sl.ISMSLE_Board = dto.ISMSLE_Board;
                        sl.ISMSLERaw_Id = dto.ISMSLERaw_Id;
                        _vmsconte.Add(sl);

                        ISM_Sales_Lead_EmailId_mobileNoDMO email = new ISM_Sales_Lead_EmailId_mobileNoDMO();
                        email.ISMSLEEM_EmailId = dto.ISMSLE_EmailId;
                        email.ISMSLEEM_MobileNo = dto.ISMSLE_ContactNo.ToString();
                        email.ISMSLEEM_DefaultFlag = "default";
                        email.ISMSLE_Id = sl.ISMSLE_Id;

                        _vmsconte.Add(email);
                        foreach (var item in dto.product_list_save)
                        {
                            ISM_Sales_Lead_Products_DMO ld = new ISM_Sales_Lead_Products_DMO();
                            ld.ISMSMPR_Id = item.ISMSMPR_Id;
                            ld.ISMSLE_Id = sl.ISMSLE_Id;
                            ld.MI_Id = item.MI_Id;
                            ld.ISMSLEPR_ActiveFlag = true;
                            ld.CreatedDate = DateTime.Now;
                            ld.ISMSLEPR_CreatedBy = dto.user_id;
                            ld.UpdatedDate = DateTime.Now;
                            ld.ISMSLEPR_UpdatedBy = dto.user_id;
                            _vmsconte.Add(ld);
                        }

          
                        var check = _vmsconte.SaveChanges();
                        if (check > 0)
                        {
                            dto.return_value = "Add";
                            var result1 = _vmsconte.ISM_Sales_Master_Source_DMO.Where(a => a.ISMSMSO_Id == dto.ISMSMSO_Id).ToList();
                            if (result1.Count > 0)
                            {
                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == dto.MI_Id && e.ISES_Template_Name == result1.FirstOrDefault().ISMSMSO_Templet && e.ISES_MailActiveFlag == true).ToList();
                                if (template.Count > 0)
                                {
                                    if (dto.sendemail == true)
                                    {
                                        if (dto.ISMSLE_EmailId != null && dto.ISMSLE_EmailId != "")
                                        {
                                           sendmail(dto.MI_Id, dto.ISMSLE_EmailId, result1.FirstOrDefault().ISMSMSO_Templet, sl.ISMSLE_Id);
                                           
                                        }
                                    }
                                }
                            }
                            //long MI_Id, string Email, string Template, long UserID,string FHEAD,string esubject,string msg
                            //sendmailNew(dto.MI_Id, dto.ISMSLE_EmailId, result1.FirstOrDefault().ISMSMSO_Templet, sl.ISMSLE_Id);
                        }
                    }






                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO Sales_lead_edit(ISM_Sales_Lead_DTO dTO)
        {
            try
            {
                dTO.leadTempList = _vmsconte.ISM_Sales_Lead_TemplateDMO_con.Where(a => a.ISMSLE_Id == dTO.ISMSLE_Id).ToArray();
                dTO.source_dd = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMSO_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

                dTO.status_dd = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMST_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

                dTO.channel_patner_edit = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMCP_Id == dTO.ISMSMCP_Id).ToArray();


                var imgdata = (from a in _vmsconte.ISM_Sales_Master_ChannelPartner_DocumentsDMO_con
                               from b in _vmsconte.ISM_Sales_Master_ChannelPartnerDMO
                               where a.ISMSMCP_Id == dTO.ISMSMCP_Id && a.ISMSMCP_Id == b.ISMSMCP_Id
                               select new ISM_Sales_Lead_DTO
                               {
                                   ISMSMCP_Id = a.ISMSMCP_Id,
                                   ISMSMCPD_DocumentName = a.ISMSMCPD_DocumentName,
                                   ISMSMCPD_DocumentPath = a.ISMSMCPD_DocumentPath,
                               }).ToArray();
                if (imgdata.Length > 0)
                {
                    dTO.attachementlist = imgdata;
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SalesLead_Details_LeadWise";

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dTO.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dTO.ISMSLE_Id
                    });


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
                        dTO.sales_lead_edit = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }







                //dTO.sales_lead_edit = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                       from b in _vmsconte.ISM_Sales_Master_Category_DMO_con
                //                       from c in _vmsconte.IVRM_Master_State
                //                       from d in _vmsconte.ISM_Sales_Master_Source_DMO_con
                //                       from e in _vmsconte.ISM_Sales_Master_Status_DMO_con
                //                       from f in _vmsconte.IVRM_Master_Country
                //                       from g in _vmsconte.IVRM_Master_CityDMO
                //                       where a.ISMSMCA_Id == b.ISMSMCA_Id && a.IVRMMS_Id == c.IVRMMS_Id && a.IVRMMCT_Id==g.IVRMMCT_Id && a.ISMSMSO_Id == d.ISMSMSO_Id && a.MI_Id == dTO.MI_Id && a.ISMSMST_Id == e.ISMSMST_Id && a.IVRMMC_Id == f.IVRMMC_Id && a.ISMSLE_Id == dTO.ISMSLE_Id
                //                       select new ISM_Sales_Lead_DTO
                //                       {
                //                           ISMSMCP_Id = a.ISMSMCP_Id,
                //                           HRME_Id = a.HRME_Id,
                //                           ISMSMCP_ChannelPartnerFlg = a.ISMSMCP_ChannelPartnerFlg,
                //                           ISMSLE_Id = a.ISMSLE_Id,
                //                           MI_Id = a.MI_Id,
                //                           ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                           ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                //                           ISMSLE_ContactPerson = a.ISMSLE_ContactPerson,
                //                           ISMSLE_LeadAddress1 = a.ISMSLE_LeadAddress1,
                //                           ISMSLE_LeadAddress2 = a.ISMSLE_LeadAddress2,
                //                           ISMSLE_LeadAddress3 = a.ISMSLE_LeadAddress3,
                //                           ISMSLE_VisitedDate = Convert.ToDateTime(a.ISMSLE_VisitedDate),
                //                           ISMSLE_Pincode = Convert.ToInt64(a.ISMSLE_Pincode),
                //                           ISMSLE_ContactNo = a.ISMSLE_ContactNo,
                //                           ISMSMST_Id = a.ISMSMST_Id,
                //                           IVRMMS_Name = c.IVRMMS_Name,
                //                           ISMSLE_EmailId = a.ISMSLE_EmailId,
                //                           ISMSMCA_Id = a.ISMSMCA_Id,
                //                           ISMSMCA_CategoryName = b.ISMSMCA_CategoryName,
                //                           ISMSMSO_Id = a.ISMSMSO_Id,
                //                           ISMSMSO_SourceName = d.ISMSMSO_SourceName,
                //                           ISMSLE_Reference = a.ISMSLE_Reference,
                //                           ISMSLE_ContactDesignation = a.ISMSLE_ContactDesignation,
                //                           IVRMMS_Id = a.IVRMMS_Id,
                //                           IVRMMCT_Id = a.IVRMMCT_Id.Value,
                //                           ISMSLE_StudentStrength = a.ISMSLE_StudentStrength,
                //                           ISMSLE_StaffStrength = a.ISMSLE_StaffStrength,
                //                           ISMSLE_NoOfInstitutions = a.ISMSLE_NoOfInstitutions,
                //                           ISMSLE_Remarks = a.ISMSLE_Remarks,
                //                           ISMSMST_StatusName = e.ISMSMST_StatusName,
                //                           IVRMMC_Id = a.IVRMMC_Id,
                //                           IVRMMC_CountryName = f.IVRMMC_CountryName,
                //                           IVRMMCT_Name = g.IVRMMCT_Name,
                //                           ISMSLE_QuotationValue = a.ISMSLE_QuotationValue,
                //                           ISMSLE_DedicatedHostingFlg = a.ISMSLE_DedicatedHostingFlg,
                //                           ISMSLE_DedicatedMobileAppFlg = a.ISMSLE_DedicatedMobileAppFlg,
                //                           ISMSLE_Board = a.ISMSLE_Board,
                //                           ISMSLE_HostingServer = a.ISMSLE_HostingServer
                //                       }).ToList().ToArray();









                dTO.companylist = _vmsconte.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                dTO.sales_lead_edit_product_dd_product = (from a in _vmsconte.ISM_Sales_Lead_Products_DMO_con
                                                          from b in _vmsconte.ISM_Sales_Master_Product_DMO_con


                                                          where a.ISMSMPR_Id == b.ISMSMPR_Id && a.MI_Id == 17 && a.ISMSLE_Id == dTO.ISMSLE_Id
                                                          select new ISM_Sales_Lead_DTO
                                                          {
                                                              ISMSMPR_ProductName = b.ISMSMPR_ProductName,
                                                              ISMSMPR_Id = a.ISMSMPR_Id,
                                                              ISMSLEPR_ActiveFlag = a.ISMSLEPR_ActiveFlag,
                                                              ISMSLE_Id = a.ISMSLE_Id

                                                          }).ToArray();

                dTO.sales_lead_edit_product_dd = (from a in _vmsconte.ISM_Sales_Lead_Products_DMO_con
                                                  from b in _vmsconte.ISM_Sales_Master_Product_DMO_con


                                                  where a.ISMSMPR_Id == b.ISMSMPR_Id && a.MI_Id == 17 && a.ISMSLE_Id == dTO.ISMSLE_Id
                                                  && a.ISMSLEPR_ActiveFlag == true
                                                  select new ISM_Sales_Lead_DTO
                                                  {
                                                      ISMSMPR_ProductName = b.ISMSMPR_ProductName,
                                                      ISMSMPR_Id = a.ISMSMPR_Id,
                                                  }).ToArray();

                dTO.product_dd = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == 17 && a.ISMSMPR_ActiveFlag == true).Distinct().ToArray();
                dTO.category_dd = _vmsconte.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMCA_ActiveFlag == true).ToArray();
                dTO.source_dd = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMSO_ActiveFlag == true).ToArray();
                dTO.status_dd = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMST_ActiveFlag == true).ToArray();

                var LeadCountry = _vmsconte.ISM_Sales_Lead_DMO_con.Single(t => t.ISMSLE_Id == dTO.ISMSLE_Id).IVRMMC_Id;
                dTO.state_dd = _hrmscon.IVRM_Master_State.Where(t => t.IVRMMC_Id == LeadCountry).ToArray();
                dTO.country_dd = _hrmscon.IVRM_Master_Country.ToArray();
                dTO.IVRMMC_Id = LeadCountry;

                var leadcity = _vmsconte.ISM_Sales_Lead_DMO_con.Single(t => t.ISMSLE_Id == dTO.ISMSLE_Id).IVRMMCT_Id;
                dTO.district_dd = _hrmscon.IVRM_Master_CityDMO.Where(t => t.IVRMMC_Id == LeadCountry).ToArray();
                if (leadcity > 0)
                {
                    dTO.IVRMMCT_Id = leadcity.Value;
                }
             



                dTO.IVRMMS_Id = _vmsconte.ISM_Sales_Lead_DMO_con.Single(t => t.ISMSLE_Id == dTO.ISMSLE_Id).IVRMMS_Id;

                var sourceid = _vmsconte.ISM_Sales_Lead_DMO_con.Single(t => t.ISMSLE_Id == dTO.ISMSLE_Id).ISMSMSO_Id;
                var result1 = _vmsconte.ISM_Sales_Master_Source_DMO.Where(a => a.ISMSMSO_Id == sourceid).ToList();


                if (result1.Count > 0)
                {
                    var template = _db.smsEmailSetting.Where(e => e.MI_Id == dTO.MI_Id && e.ISES_Template_Name == result1.FirstOrDefault().ISMSMSO_Templet && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count > 0)
                    {
                        dTO.returnvalue = false;
                    }
                    else
                    {
                        dTO.returnvalue = true;
                    }
                }
                else
                {
                    dTO.returnvalue = true;
                }

                dTO.ChannalPatnerList = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMCP_ActiveFlag == true).ToArray();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_RawLeads";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "2"
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                    {
                        Value = dTO.ISMSLE_Id
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
                        dTO.finalleadlist = retObject.ToArray();
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
            }
            return dTO;
        }
        public ISM_Sales_Lead_DTO SaveChannel(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                DateTime? nullDateTime = new DateTime();

                if (dto.ISMSMCP_Id == 0)
                {
                    var checkduplicate1 = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Where(d => d.MI_Id == dto.MI_Id && (d.ISMSMCP_CPContactNo == dto.ISMSMCP_CPContactNo || d.ISMSMCP_CPEmailId == dto.ISMSMCP_CPEmailId)).ToList();
                    if (checkduplicate1.Count() > 0)
                    {
                        dto.msg = "duplicate";
                    }
                    else
                    {
                        
                       ISM_Sales_Master_ChannelPartnerDMO mapp = new ISM_Sales_Master_ChannelPartnerDMO();
                        mapp.MI_Id = dto.MI_Id;
                        mapp.ISMSMCP_ChannelPartnerName = dto.ISMSMCP_ChannelPartnerName;
                        mapp.ISMSMCP_CPAddress = dto.ISMSMCP_CPAddress;
                        mapp.ISMSMCP_CPContactNo = dto.ISMSMCP_CPContactNo;
                        mapp.ISMSMCP_CPEmailId = dto.ISMSMCP_CPEmailId;
                        mapp.ISMSMCP_ActiveFlag = true;
                        mapp.ISMSMCP_UpdatedDate = DateTime.Now;
                        mapp.ISMSMCP_CreatedDate = DateTime.Now;
                        mapp.ISMSMCP_ActiveFromDate = dto.ISMSMCP_ActiveFromDate;
                        mapp.ISMSMCP_AgreementStartDate = dto.ISMSMCP_AgreementStartDate;
                        mapp.ISMSMCP_ActiveToDate = dto.ISMSMCP_ActiveToDate;

                        mapp.ISMSMCP_CreatedBy = dto.user_id;
                        mapp.ISMSMCP_UpdatedBy = dto.user_id;
                        mapp.ISMSMCP_AgreementDuration = dto.ISMSMCP_AgreementDuration;
                        if (dto.ISMSMCP_AgreementStartDate != null)
                        {
                            mapp.ISMSMCP_AgreementStartDate = dto.ISMSMCP_AgreementStartDate;
                        }
                        else
                        {
                            mapp.ISMSMCP_AgreementStartDate = null;
                        }
                        if (dto.ISMSMCP_ActiveFromDate != null)
                        {
                            mapp.ISMSMCP_ActiveFromDate = dto.ISMSMCP_ActiveFromDate;
                        }
                        else
                        {
                            mapp.ISMSMCP_ActiveFromDate = null;
                        }
                        if (dto.ISMSMCP_ActiveToDate != null)
                        {
                            mapp.ISMSMCP_ActiveToDate = dto.ISMSMCP_ActiveToDate;
                        }
                        else
                        {
                            mapp.ISMSMCP_ActiveToDate = null;
                        }
                        //mapp.ISMSMCP_AgreementStartDate = dto.ISMSMCP_AgreementStartDate != null ? DateTime.Parse(dto.ISMSMCP_AgreementStartDate.ToString("yyyy-MM-dd")) : null;
                        //mapp.ISMSMCP_ActiveFromDate =  dto.ISMSMCP_ActiveFromDate != null ? DateTime.Parse(dto.ISMSMCP_ActiveFromDate.ToString("yyyy-MM-dd")) : null;
                        //mapp.ISMSMCP_ActiveToDate =  dto.ISMSMCP_ActiveToDate != null ? DateTime.Parse(dto.ISMSMCP_ActiveToDate.ToString("yyyy-MM-dd")) : null;
                        mapp.ISMSMCP_RenewedFlg = dto.ISMSMCP_RenewedFlg;
                        _vmsconte.Add(mapp);
                        if (dto.ISMSMCPD_DocumentPath_Array != null && dto.ISMSMCPD_DocumentPath_Array.Length > 0)
                        {
                            foreach (var itm in dto.ISMSMCPD_DocumentPath_Array)
                            {
                                ISM_Sales_Master_ChannelPartner_DocumentsDMO had = new ISM_Sales_Master_ChannelPartner_DocumentsDMO();

                                had.ISMSMCP_Id = mapp.ISMSMCP_Id;
                                had.ISMSMCPD_DocumentName = itm.FileName;
                                had.ISMSMCPD_DocumentPath = itm.ISMSMCPD_DocumentPath;
                                had.ISMSMCPD_ActiveFlag = true;
                                had.ISMSMCPD_CreatedDate = DateTime.Now;
                                had.ISMSMCPD_UpdatedDate = DateTime.Now;
                                _vmsconte.Add(had);
                            }
                        }

                        int s = _vmsconte.SaveChanges();
                        if (s > 0)
                        {
                            dto.msg = "Add";
                        }
                        else
                        {
                            dto.msg = "savingFailed";
                        }
                    }

                }
                else if (dto.ISMSMCP_Id > 0)
                {
                    var checkduplicate2 = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Where(d => d.MI_Id == dto.MI_Id && d.ISMSMCP_Id != dto.ISMSMCP_Id && (d.ISMSMCP_CPContactNo == dto.ISMSMCP_CPContactNo || d.ISMSMCP_CPEmailId == dto.ISMSMCP_CPEmailId)).ToList();
                    if (checkduplicate2.Count() > 0)
                    {
                        dto.msg = "duplicate";
                    }
                    else
                    {

                        var update = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Single(d => d.ISMSMCP_Id == dto.ISMSMCP_Id && d.MI_Id == dto.MI_Id);
                        update.ISMSMCP_ChannelPartnerName = dto.ISMSMCP_ChannelPartnerName;
                        update.ISMSMCP_CPAddress = dto.ISMSMCP_CPAddress;
                        update.ISMSMCP_CPContactNo = dto.ISMSMCP_CPContactNo;
                        update.ISMSMCP_CPEmailId = dto.ISMSMCP_CPEmailId;
                        update.ISMSMCP_ActiveFlag = true;
                        update.ISMSMCP_UpdatedDate = DateTime.Now;
                        update.ISMSMCP_CreatedBy = dto.user_id;
                        update.ISMSMCP_UpdatedBy = dto.user_id;
                        update.ISMSMCP_AgreementDuration = dto.ISMSMCP_AgreementDuration;
                        update.ISMSMCP_AgreementStartDate = dto.ISMSMCP_AgreementStartDate;
                        update.ISMSMCP_ActiveFromDate = dto.ISMSMCP_ActiveFromDate;
                        update.ISMSMCP_ActiveToDate = dto.ISMSMCP_ActiveToDate;
                        update.ISMSMCP_RenewedFlg = dto.ISMSMCP_RenewedFlg;
                        _vmsconte.Update(update);

                        if (dto.ISMSMCPD_DocumentPath_Array != null && dto.ISMSMCPD_DocumentPath_Array.Length > 0)
                        {
                            var result = _vmsconte.ISM_Sales_Master_ChannelPartner_DocumentsDMO_con.Where(a => a.ISMSMCP_Id == dto.ISMSMCP_Id).ToList();
                            if (result.Count > 0)
                            {

                                foreach (var e in result)
                                {
                                    var result1 = _vmsconte.ISM_Sales_Master_ChannelPartner_DocumentsDMO_con.Single(a => a.ISMSMCP_Id == dto.ISMSMCP_Id && a.ISMSMCPD_Id == e.ISMSMCPD_Id);
                                    _vmsconte.Remove(result1);
                                }
                            }

                            foreach (var itm in dto.ISMSMCPD_DocumentPath_Array)
                            {
                                ISM_Sales_Master_ChannelPartner_DocumentsDMO had = new ISM_Sales_Master_ChannelPartner_DocumentsDMO();

                                had.ISMSMCP_Id = dto.ISMSMCP_Id;
                                had.ISMSMCPD_DocumentName = itm.FileName;
                                had.ISMSMCPD_DocumentPath = itm.ISMSMCPD_DocumentPath;
                                had.ISMSMCPD_ActiveFlag = true;
                                had.ISMSMCPD_UpdatedDate = DateTime.Now; ;
                                _vmsconte.Add(had);
                            }
                        }

                    }


                    int s = _vmsconte.SaveChanges();
                    if (s > 0)
                    {
                        dto.msg = "Updated";
                    }
                    else
                    {
                        dto.msg = "updateFailed";
                    }

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO SaveVerifyflag(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                //ISM_Sales_Lead_RawdataDMO
                if (dto.retrunMsg== "Raw_data")
                {
                    foreach (var d in dto.Leadlsttwo)
                    {
                        var mapp = _vmsconte.ISM_Sales_Lead_RawdataDMO.Where(R => R.ISMSLERaw_Id == d.ISMSLE_Id).FirstOrDefault();
                        if (d.flag == "A")
                        {
                            mapp.ISMSLERaw_status = "Approved";
                        }
                        else
                        {
                            mapp.ISMSLERaw_status = "Reject";
                        }
                        mapp.ISMSLERaw_ApprovedRemark = d.ISMSLE_VerifedRemark;
                        mapp.ISMSLERaw_ApprovedBy = Convert.ToString(dto.user_id);
                        mapp.ISMSLERaw_UpdatedDate = DateTime.Now;                      
                        _vmsconte.Update(mapp);
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
               else if (dto.retrunMsg == "Raw_data_tel")
                {
                    foreach (var d in dto.Leadlsttwo)
                    {
                        var mapp = _vmsconte.ISM_Sales_Lead_RawdataDMO.Where(R => R.ISMSLERaw_Id == d.ISMSLE_Id).FirstOrDefault();
                        if (d.flag == "A")
                        {
                            mapp.ISMSLERaw_statusTel = "Approved";
                        }
                        else
                        {
                            mapp.ISMSLERaw_statusTel = "Reject";
                        }
                        mapp.ISMSLERaw_ApprovedRemarkTel = d.ISMSLE_VerifedRemark;
                        mapp.ISMSLERaw_ApprovedByTel = Convert.ToString(dto.user_id);
                        mapp.ISMSLERaw_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(mapp);
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
                else
                {
                    foreach (var d in dto.Leadlsttwo)
                    {
                        var mapp = _vmsconte.ISM_Sales_Lead_DMO_con.Where(R => R.ISMSLE_Id == d.ISMSLE_Id).FirstOrDefault();
                        if (d.flag == "A")
                        {
                            mapp.ISMSLE_VerifiedFlg = true;
                        }
                        else
                        {
                            mapp.ISMSLE_VerifiedFlg = false;
                        }
                        mapp.ISMSLE_VerifedRemark = d.ISMSLE_VerifedRemark;
                        mapp.ISMSLE_ConfirmedBy = d.ISMSLE_ConfirmedBy;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.ISMSLE_UpdatedBy = dto.user_id;
                        _vmsconte.Update(mapp);
                        using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "usp_s_AutoDemoSchedule";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                            {
                                Value = d.ISMSLE_Id
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
                                //dto.leadlist = retObject.ToArray();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                        }
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

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO deactivate(ISM_Sales_Lead_DTO dto)
        {
            try
            {

                var result = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Single(t => t.MI_Id == dto.MI_Id && t.ISMSMCP_Id == dto.ISMSMCP_Id);
                if (result.ISMSMCP_ActiveFlag == true)
                {
                    result.ISMSMCP_ActiveFlag = false;
                }
                else
                {
                    result.ISMSMCP_ActiveFlag = true;
                }
                result.ISMSMCP_CreatedDate = result.ISMSMCP_CreatedDate;
                result.ISMSMCP_UpdatedDate = DateTime.Now;
                _vmsconte.Update(result);
                var flag = _vmsconte.SaveChanges();
                if (flag == 1)
                {
                    dto.returnVal = true;

                    if (result.ISMSMCP_ActiveFlag == true)
                    {
                        dto.msg = "Activated Successfully.";
                    }
                    else if (result.ISMSMCP_ActiveFlag == false)
                    {
                        dto.msg = "Deactivated Successfully.";
                    }
                }
                else
                {
                    dto.returnVal = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO viewData(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                dto.attachementlist = (from a in _vmsconte.ISM_Sales_Master_ChannelPartnerDMO
                                       from b in _vmsconte.ISM_Sales_Master_ChannelPartner_DocumentsDMO_con
                                       where b.ISMSMCP_Id == dto.ISMSMCP_Id && a.ISMSMCP_Id == b.ISMSMCP_Id
                                       select new ISM_Sales_Lead_DTO
                                       {
                                           ISMSMCPD_DocumentName = b.ISMSMCPD_DocumentName,
                                           ISMSMCPD_DocumentPath = b.ISMSMCPD_DocumentPath,
                                           ISMSMCP_Id = a.ISMSMCP_Id
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO deactiv_prde(ISM_Sales_Lead_DTO dto)
        {
            try
            {

                var result = _vmsconte.ISM_Sales_Lead_Products_DMO_con.Single(t => t.ISMSMPR_Id == dto.ISMSMPR_Id && t.ISMSLE_Id == dto.ISMSLE_Id);

                if (dto.ISMSLEPR_ActiveFlag == true)
                {
                    result.ISMSLEPR_ActiveFlag = false;
                    result.ISMSLEPR_UpdatedBy = dto.user_id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {

                    result.ISMSLEPR_ActiveFlag = true;
                    result.ISMSLEPR_UpdatedBy = dto.user_id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }


            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        public ISM_Sales_Lead_DTO FilterEmployeeData(ISM_Sales_Lead_DTO dto)
        {
            List<ISM_Sales_Lead_DMO> leadDetails = new List<ISM_Sales_Lead_DMO>();
            try
            {
                if (dto.channelprtnrselected.Count() > 0 && dto.leadselected.Count() > 0)
                {
                    leadDetails = _vmsconte.ISM_Sales_Lead_DMO_con.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.channelprtnrselected.Contains(t.ISMSMCP_Id) && dto.leadselected.Contains(t.ISMSLE_Id) && t.ISMSLE_ActiveFlag == true).ToList();
                }


                dto.leaddropdown = leadDetails.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Sales_Lead_DTO getLead(ISM_Sales_Lead_DTO dto)
        {
            try
            {


                dto.leadlists = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true && a.ISMSMCP_Id == dto.ISMSMCP_Id).ToArray();

            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }


        public ISM_Sales_Lead_DTO Sales_Lead_EmailMobNumSave (ISM_Sales_Lead_DTO dto)
        {
            try
            {
                foreach (var d in dto.SaveEmailMobNum)
                {
                    var duplicate = _vmsconte.ISM_Sales_Lead_EmailId_mobileNoDMO.Where(R => R.ISMSLE_Id ==dto.ISMSLE_Id && R.ISMSLEEM_MobileNo == d.mobnum && R.ISMSLEEM_EmailId == d.email).ToList();
                    if (duplicate.Count > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        ISM_Sales_Lead_EmailId_mobileNoDMO mob = new ISM_Sales_Lead_EmailId_mobileNoDMO();
                        mob.ISMSLE_Id = dto.ISMSLE_Id;
                        mob.ISMSLEEM_MobileNo = d.mobnum;
                        mob.ISMSLEEM_EmailId = d.email;
                        mob.ISMSLEEM_DefaultFlag = "default";
                        _vmsconte.Add(mob);
                    }
                }
                var vv = _vmsconte.SaveChanges();
                if (vv > 0)
                {
                    dto.retrunMsg = "Add";
                }
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }



        public ISM_Sales_Lead_DTO LeadSearchByColumn(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_SalesLead_Details_ColumnBySearch";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SearchValue", SqlDbType.VarChar)
                    {
                        Value = dto.EnteredData
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = dto.SearchColumnFlag
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
                        dto.lead_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }

        //=====================Lead Demo===========================
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_SalesLead_Detailslist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
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
                        dto.lead_list_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //dto.lead_list_dd = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                    from b in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                    where a.MI_Id == dto.MI_Id && a.ISMSLE_Id == b.ISMSLE_Id && a.ISMSLE_ActiveFlag == true && b.ISMSLEDM_Status_Flg ==null
                //                    select new ISM_Sales_Lead_Demo_DTO
                //                    {
                //                        ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                        ISMSLE_Id = a.ISMSLE_Id,
                //                        ISMSLE_ContactPerson =a.ISMSLE_ContactPerson

                //                    }).ToArray();
                //dto.lead_list_dd = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_ActiveFlag == true
                //&& a.MI_Id == dto.MI_Id).OrderBy(a => a.ISMSLE_LeadName).ToArray();

                dto.hrme_list_dd = (from a in _vmsconte.Hr_Master_Employee_con
                                    from b in _vmsconte.Institution
                                    from c in _vmsconte.HR_Master_Department
                                    from d in _vmsconte.HR_Master_Designation
                                    where (a.HRME_ActiveFlag == true && a.HRMDES_Id == d.HRMDES_Id && a.HRME_LeftFlag == false && a.MI_Id == b.MI_Id && b.MI_ActiveFlag == 1 && a.HRMD_Id==c.HRMD_Id)
                                    select new ISM_Sales_Lead_Demo_DTO
                                    {
                                        employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                        + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                        + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                        HRME_Id = a.HRME_Id,
                                        HRMD_DepartmentName=c.HRMD_DepartmentName,
                                        HRMDES_DesignationName = d.HRMDES_DesignationName

                                    }).Distinct().OrderBy(a => a.employeename).ToList().ToArray();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Employees";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.hrme_list_ddd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_ProductList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.product_list_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



                //     dto.product_list_dd = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.ISMSMPR_ActiveFlag == true && a.MI_Id == 17).OrderBy(a => a.ISMSMPR_ProductName).ToArray();

                dto.hrmr_list_dd = _vmsconte.HR_Master_Room_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMR_ActiveFlag == true).OrderBy(a => a.HRMR_RoomName).ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_DemoUpdateList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.demo_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_DemoUpdateList_new";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.demo_listnew = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                //dto.demo_list = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                 from b in _vmsconte.ISM_Sales_Master_Status_DMO_con
                //                 from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                 from d in _vmsconte.HR_Master_Employee_DMO
                //                 where a.ISMSMST_Id == b.ISMSMST_Id && a.ISMSLE_Id == c.ISMSLE_Id && a.MI_Id == dto.MI_Id && c.HRME_Id==d.HRME_Id 


                //                 select new ISM_Sales_Lead_Demo_DTO
                //                 {
                //                     ISMSLEDM_Id = c.ISMSLEDM_Id,
                //                     ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                     ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                //                     ISMSLE_Id = a.ISMSLE_Id,
                //                     ISMSLEDM_DemoType = c.ISMSLEDM_DemoType,
                //                     ISMSMST_StatusName = b.ISMSMST_StatusName,
                //                     ISMSMST_Id = b.ISMSMST_Id,
                //                     ISMSLEDM_ContactPerson = c.ISMSLEDM_ContactPerson,
                //                     ISMSLEDM_DemoDate = c.ISMSLEDM_DemoDate,
                //                     ISMSLEDM_ActiveFlag = c.ISMSLEDM_ActiveFlag,
                //                     ISMSLEDM_Status_Flg = c.ISMSLEDM_Status_Flg,
                //                     employeename =d.HRME_EmployeeFirstName


                //                 }).OrderByDescending(t => t.ISMSLEDM_Id).ThenBy(t=>t.ISMSLEDM_Status_Flg==0).ToArray();
                //}).OrderByDescending(t => t.ISMSLEDM_Status_Flg==1).ToArray();

                //nagaraj added for getting emp data for swapping dropdown

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Swapping_HR_Employee_Names";
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
                        dto.Sales_swappingEmpData = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                dto.incentiveleads = _vmsconte.ISM_CA_Demo_RatingsDMO.Where(r => r.ICDR_Active == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {

                //dto.Edit_details_lead_demo = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                              from b in _vmsconte.ISM_Sales_Master_Status_DMO_con
                //                              from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                              from d in _vmsconte.Hr_Master_Employee_con
                //                              from e in _vmsconte.HR_Master_Room_con
                //                              where a.ISMSMST_Id == b.ISMSMST_Id && a.ISMSLE_Id == c.ISMSLE_Id && a.MI_Id == dto.MI_Id
                //                              && c.ISMSLEDM_Id == dto.ISMSLEDM_Id && a.ISMSLE_Id == dto.ISMSLE_Id && c.HRME_Id == d.HRME_Id && c.HRMR_Id == e.HRMR_Id

                //                              select new ISM_Sales_Lead_Demo_DTO
                //                              {
                //                                  employeename = ((d.HRME_EmployeeFirstName == null || d.HRME_EmployeeFirstName == "" ? "" : d.HRME_EmployeeFirstName)
                //                                  + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == "" ? "" : " " + d.HRME_EmployeeMiddleName)
                //                                  + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == "" ? "" : " " + d.HRME_EmployeeLastName)).Trim(),
                //                                  HRME_Id = d.HRME_Id,
                //                                  ISMSLEDM_Id = c.ISMSLEDM_Id,
                //                                  HRMR_Id = e.HRMR_Id,
                //                                  ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                                  ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                //                                  ISMSLE_Id = a.ISMSLE_Id,
                //                                  ISMSLEDM_DemoType = c.ISMSLEDM_DemoType,
                //                                  ISMSMST_StatusName = b.ISMSMST_StatusName,
                //                                  ISMSMST_Id = b.ISMSMST_Id,
                //                                  ISMSLEDM_DemoAddress = c.ISMSLEDM_DemoAddress,
                //                                  ISMSLEDM_ContactPerson = c.ISMSLEDM_ContactPerson,
                //                                  ISMSLEDM_DemoDate = Convert.ToDateTime(c.ISMSLEDM_DemoDate),
                //                                  ISMSLEDM_ActiveFlag = c.ISMSLEDM_ActiveFlag,
                //                                  ISMSLEDM_Remarks = c.ISMSLEDM_Remarks
                //                              }).ToArray();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Demo_Creation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLEDM_Id",
                   SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLEDM_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                   SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
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
                        dto.Edit_details_lead_demo = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.Lead_Demo_Products_list = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                                               from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                               from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                               where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id && c.ISMSLE_Id == dto.ISMSLE_Id
                                               && b.MI_Id == dto.MI_Id && b.ISMSMPR_Id == d.ISMSMPR_Id

                                               select new ISM_Sales_Lead_Demo_DTO
                                               {
                                                   ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                                                   ISMSMPR_Id = b.ISMSMPR_Id,
                                                   ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints
                                               }).ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_ProductList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
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
                        dto.product_list_dd = retObject.ToArray();
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
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                if (dto.ISMSLEDM_Id > 0)
                {
                    var result1 = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Where(a => a.ISMSLEDM_Id == dto.ISMSLEDM_Id).ToList();
                    
                    foreach (var item in result1)
                    {
                        var res = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Single(a => a.ISMSLEDMPR_Id == item.ISMSLEDMPR_Id);
                        _vmsconte.Remove(res);
                    }

                    var result = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Single(a => a.ISMSLEDM_Id == dto.ISMSLEDM_Id);
                    result.HRMR_Id = dto.HRMR_Id;
                    result.MI_Id = dto.MI_Id;
                    result.ISMSLE_Id = dto.ISMSLE_Id;
                    result.HRME_Id = dto.HRME_Id;
                    result.ISMSLEDM_DemoType = dto.ISMSLEDM_DemoType;
                    result.ISMSLEDM_DemoDate = dto.ISMSLEDM_DemoDate;
                    result.ISMSLEDM_ContactPerson = dto.ISMSLEDM_ContactPerson;
                    result.ISMSLEDM_DemoScheduledBy = dto.ISMSLEDM_DemoScheduledBy;
                    result.ISMSLEDM_DemoAddress = dto.ISMSLEDM_DemoAddress;
                    result.ISMSLEDM_Remarks = dto.ISMSLEDM_Remarks;
                    result.ISMSLEDM_RescheduleFlag = false;
                    result.ISMSLEDM_DemoExecuted = false;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMSLEDM_UpdatedBy = dto.user_id;
                    _vmsconte.Update(result);

                    foreach (var pdm in dto.product_demo_master_temp1)
                    {
                        ISM_Sales_Lead_Demo_Products_DMO dm = new ISM_Sales_Lead_Demo_Products_DMO();
                        dm.ISMSMPR_Id = pdm.ISMSMPR_Id;
                        dm.ISMSLEDMPR_DiscussionPoints = pdm.ISMSLEDMPR_DiscussionPoints;
                        dm.ISMSLEDM_Id = result.ISMSLEDM_Id;
                        dm.UpdatedDate = DateTime.Now;
                        dm.ISMSLEDMPR_UpdatedBy = dto.user_id;
                        dm.MI_Id = dto.MI_Id;
                        dm.ISMSBOM_Id = dto.ISMSBOM_Id;
                        _vmsconte.Add(dm);
                    }
                    _vmsconte.SaveChanges();
                    dto.return_status = "Update";
                }
                else
                {
                    var empcompany = _vmsconte.Hr_Master_Employee_con.Where(t => t.HRME_Id == dto.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();

                    var existingData = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Where(d => d.ISMSLE_Id == dto.ISMSLE_Id && d.ISMSLEDM_DemoType == dto.ISMSLEDM_DemoType && d.ISMSLEDM_DemoScheduledBy == dto.ISMSLEDM_DemoScheduledBy && d.HRME_Id == dto.HRME_Id ).ToList();  //&& d.HRMR_Id == dto.HRMR_Id
                    if (existingData.Count > 0)
                    {
                        dto.return_status = "Duplicate";
                    }
                    else
                    {
                        ISM_Sales_Lead_Demo_DMO res = new ISM_Sales_Lead_Demo_DMO();
                        res.MI_Id = empcompany;
                        res.ISMSLE_Id = dto.ISMSLE_Id;
                        res.HRME_Id = dto.HRME_Id;
                        res.HRMR_Id = dto.HRMR_Id;
                        res.ISMSLEDM_DemoType = dto.ISMSLEDM_DemoType;
                        res.ISMSLEDM_DemoDate = dto.ISMSLEDM_DemoDate;
                        res.ISMSLEDM_ContactPerson = dto.ISMSLEDM_ContactPerson;
                        res.ISMSLEDM_DemoScheduledBy = dto.ISMSLEDM_DemoScheduledBy;
                        res.ISMSLEDM_DemoAddress = dto.ISMSLEDM_DemoAddress;
                        res.ISMSLEDM_Remarks = dto.ISMSLEDM_Remarks;
                        res.CreatedDate = DateTime.Now;
                        res.ISMSLEDM_DemoExecuted = false;
                        res.ISMSLEDM_ActiveFlag = true;
                        res.ISMSBOM_Id = dto.ISMSBOM_Id;
                        res.ISMSLEDM_CreatedBy = dto.user_id;
                        _vmsconte.Add(res);
                        foreach (var pdm in dto.product_demo_master_temp1)
                        {
                            ISM_Sales_Lead_Demo_Products_DMO dm = new ISM_Sales_Lead_Demo_Products_DMO();
                            dm.ISMSMPR_Id = pdm.ISMSMPR_Id;
                            dm.ISMSLEDMPR_DiscussionPoints = pdm.ISMSLEDMPR_DiscussionPoints;
                            dm.ISMSLEDM_Id = res.ISMSLEDM_Id;
                            dm.CreatedDate = DateTime.Now;
                            dm.ISMSLEDMPR_CreatedBy = dto.user_id;
                            dm.MI_Id = empcompany;
                            dm.ISMSBOM_Id = pdm.ISMSBOM_Id;
                            _vmsconte.Add(dm);
                        }
                        var check = _vmsconte.SaveChanges();
                        if (check > 0)
                        {
                            dto.return_status = "Add";
                            var emailid = _vmsconte.ISM_Sales_Lead_DMO_con.Where(R => R.ISMSLE_Id == dto.ISMSLE_Id).FirstOrDefault().ISMSLE_EmailId;
                            string emailids = _vmsconte.Multiple_Email_DMO.Where(t => t.HRME_Id == dto.HRME_Id && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                            string DemoScheduledBy = _vmsconte.Multiple_Email_DMO.Where(t => t.HRME_Id == dto.ISMSLEDM_DemoScheduledBy && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                            string FHEAD = "";
                            if (emailids != null && emailids != "")
                            {
                                FHEAD = emailids;
                            }
                            if (DemoScheduledBy != null && DemoScheduledBy != "")
                            {
                                if (FHEAD != "")
                                {
                                    FHEAD = FHEAD + "," + DemoScheduledBy;
                                }
                                else
                                {
                                    FHEAD = DemoScheduledBy;
                                }
                            }

                            if (emailid != null && emailid != "")
                            {
                                //  sendmail(dto.MI_Id, emailid, "Demoschedule", res.ISMSLEDM_Id);
                                //   sendmailNew(long MI_Id, string Email, string Template, long UserID, string FHEAD, string esubject, string msg)
                                sendmailNew(dto.MI_Id, emailid, "Demoschedule", res.ISMSLEDM_Id, FHEAD, "Demo Schedule", dto.ISMSLE_LEADADDRESS1);
                            }
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO Save_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                foreach (var p in dto.product_demo_master_temp2)
                {
                    var studentStrengthUpdate= _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == p.ISMSLE_Id).FirstOrDefault();
                    try
                    {
                        var leaddatahistory = _vmsconte.Database.ExecuteSqlCommand("ism_sales_records_history @p0", p.ISMSLE_Id);
                        if (leaddatahistory > 0)
                        {

                        }

                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                    if (studentStrengthUpdate != null)
                    {
                        studentStrengthUpdate.ISMSLE_StudentStrength = dto.ISMSLE_STUDENTSTRENGTH;
                       _vmsconte.Update(studentStrengthUpdate);
                        var updated = _vmsconte.SaveChanges();
                        if (updated > 0)
                        {

                        }
                    }
                    var result = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Where(a => a.ISMSLEDM_Id == p.ISMSLEDM_Id && a.ISMSMPR_Id == p.ISMSMPR_Id).OrderByDescending(a=>a.ISMSLEDMPR_Id).FirstOrDefault();
                    //if(result.ISMSBOM_Id > 0 && result.ISMSBOM_Id != null && result.ISMSBOM_Id != 0)
                    //{

                    //}
                    var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_demo_response_update_proc @p0,@p1,@p2,@p3,@p4,@p5,@p6",
                        p.ISMSLEDM_Id, result.ISMSLEDMPR_Id, dto.MI_Id, p.ISMSMST_Id, p.ISMSLEDMPR_Remarks,p.ISMSLECOM_SendQuotationFlg,p.ISMSBOM_Id);
                    if (contactExistsP > 0)
                    {
                        dto.return_status = "Updated";

                        if (p.ISMSLECOM_SendQuotationFlg == true)
                        {
                            try
                            {
                                var Fms = _vmsconte.Database.ExecuteSqlCommand("FMS_Correspondence_Lead_Insert @p0,@p1,@p2,@p3",
                                   p.ISMSLE_Id, p.ISMSMPR_ProductName, dto.user_id, p.ISMSBOM_Id);
                                if (Fms > 0)
                                {

                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            //using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                            //{
                            //    cmd.CommandText = "FMS_Correspondence_Lead_Insert";
                            //    cmd.CommandType = CommandType.StoredProcedure;

                            //    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                            //    SqlDbType.BigInt)
                            //    {
                            //        Value = p.ISMSLE_Id
                            //    });
                            //    cmd.Parameters.Add(new SqlParameter("@Subject",
                            //   SqlDbType.BigInt)
                            //    {
                            //        Value = p.ISMSMPR_ProductName
                            //    });
                            //    cmd.Parameters.Add(new SqlParameter("@UserId",
                            //   SqlDbType.BigInt)
                            //    {
                            //        Value = dto.user_id
                            //    });
                            //    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id",
                            //      SqlDbType.BigInt)
                            //    {
                            //        Value = p.ISMSBOM_Id
                            //    });
                            //    if (cmd.Connection.State != ConnectionState.Open)
                            //        cmd.Connection.Open();

                            //    var retObject = new List<dynamic>();
                            //    try
                            //    {
                            //        using (var dataReader = cmd.ExecuteReader())
                            //        {
                            //            while (dataReader.Read())
                            //            {
                            //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            //                {
                            //                    dataRow.Add(
                            //                        dataReader.GetName(iFiled),
                            //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                            //                    );
                            //                }
                            //                retObject.Add((ExpandoObject)dataRow);
                            //            }
                            //        }

                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine(ex.Message);
                            //    }
                            //}

                            //var emailid = "mktgcoord@vapstech.com";
                            //if (emailid != null && emailid != "")
                            //{
                            //    sendmails(dto.MI_Id, emailid, "ConfirmedClient", p.ISMSLEDM_Id, dto.user_id);
                            //}

                        }
                    }
                    else
                    {
                        dto.return_status = "notUpdated";
                    }

                    //SqlConnection con = new SqlConnection("Data Source=demovaps.database.windows.net,1433;Initial Catalog=DevelopmentDataBase;Persist Security Info=False;User ID=demovaps;Password=vaps@123;Connection Timeout=30;");

                    //SqlCommand cmd = new SqlCommand("ISM_demo_response_update_proc", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@ISMSLEDM_Id", p.ISMSLEDM_Id);
                    //cmd.Parameters.AddWithValue("@ISMSLEDMPR_Id", result.ISMSLEDMPR_Id);
                    //cmd.Parameters.AddWithValue("@MI_Id", dto.MI_Id);
                    //cmd.Parameters.AddWithValue("@ISMSMST_Id", p.ISMSMST_Id);
                    //cmd.Parameters.AddWithValue("@ISMSLEDMPR_Remarks", p.ISMSLEDMPR_Remarks);
                    //con.Open();
                    //cmd.ExecuteNonQuery();
                    //con.Close();
                }
                _vmsconte.SaveChanges();
               
                dto.return_status = "Update";

                  



            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO view_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                //dto.view_lead_demo = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                      from b in _vmsconte.ISM_Sales_Master_Status_DMO_con
                //                      from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                      from d in _vmsconte.Hr_Master_Employee_con
                //                      from e in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                //                      where a.ISMSMST_Id == b.ISMSMST_Id && a.ISMSLE_Id == c.ISMSLE_Id && c.ISMSLEDM_Id == dto.ISMSLEDM_Id
                //                      && a.ISMSLE_Id == dto.ISMSLE_Id && c.HRME_Id == d.HRME_Id && c.ISMSLEDM_Id==e.ISMSLEDM_Id
                //                      select new ISM_Sales_Lead_Demo_DTO
                //                      {
                //                          employeename = ((d.HRME_EmployeeFirstName == null || d.HRME_EmployeeFirstName == "" ? "" : d.HRME_EmployeeFirstName)
                //                          + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == "" ? "" : " " + d.HRME_EmployeeMiddleName)
                //                          + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == "" ? "" : " " + d.HRME_EmployeeLastName)).Trim(),
                //                          HRME_Id = d.HRME_Id,
                //                          ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                          ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                //                          ISMSLEDM_ContactPerson = c.ISMSLEDM_ContactPerson,
                //                          ISMSLE_ContactNo = a.ISMSLE_ContactNo,
                //                          ISMSLEDM_DemoAddress = c.ISMSLEDM_DemoAddress,
                //                          ISMSLEDMPR_SendQuotationFlg = e.ISMSLEDMPR_SendQuotationFlg,
                //                          ISMSLE_LEADADDRESS1= a.ISMSLE_LeadAddress1,
                //                          //ISMSMST_STATUSNAME=a.ISMSMST_STATUSNAME,
                //                          ISMSLE_EmailId=a.ISMSLE_EmailId,
                //                          //ISMSMCA_CATEGORYNAME=a.ISMSMCA_CATEGORYNAME,
                //                          //ISMSMSO_SOURCENAME=a.ISMSMSO_SOURCENAME,
                //                          ISMSLE_REFERENCE=a.ISMSLE_Reference,
                //                          //IVRMMS_Name=a.IVRMMS_Name,
                //                          ISMSLE_STUDENTSTRENGTH=a.ISMSLE_StudentStrength,
                //                          ISMSLE_STAFFSTRENGTH=a.ISMSLE_StaffStrength,
                //                          ISMSLE_NOOFINSTITUTIONS=a.ISMSLE_NoOfInstitutions,
                //                          //IVRMMC_CountryName=a.IVRMMC_CountryName,
                //                          ISMSLE_REMARKS=a.ISMSLE_Remarks
                //                      }).ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "View_Lead_Demo_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLEDM_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLEDM_Id
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
                        dto.view_lead_demo = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_ProuctDemoList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLEDM_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLEDM_Id
                    });
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
                        dto.product_dd_s = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //dto.product_dd_s = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                //                    from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                    from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                //                    where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id && c.ISMSLE_Id == dto.ISMSLE_Id 
                //                    && b.ISMSMPR_Id == d.ISMSMPR_Id
                //                    //&& b.MI_Id == dto.MI_Id
                //                    select new ISM_Sales_Lead_Demo_DTO
                //                    {
                //                        ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                //                        ISMSMPR_Id = b.ISMSMPR_Id,
                //                        ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints,
                //                        ISMSLEDMPR_ActiveFlag = b.ISMSLEDMPR_ActiveFlag,
                //                        ISMSLEDMPR_SendQuotationFlg=b.ISMSLEDMPR_SendQuotationFlg
                //                    }).ToArray();

                if (dto.viewall == 1)
                {
                    //dto.demo_response_details = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                    //                             from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                    //                             from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                    //                             from e in _vmsconte.ISM_Sales_Master_Status_DMO_con
                    //                             from f in _vmsconte.ISM_Sales_BOMDMO
                    //                             where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id //&& b.MI_Id == dto.MI_Id
                    //                             && b.ISMSMPR_Id == d.ISMSMPR_Id && e.ISMSMST_Id == b.ISMSMST_Id 
                    //                             && d.ISMSMPR_Id ==f.ISMSMPR_Id
                    //                             select new ISM_Sales_Lead_Demo_DTO
                    //                             {
                    //                                 ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                    //                                 ISMSMPR_Id = b.ISMSMPR_Id,
                    //                                 ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints,
                    //                                 ISMSLEDM_Id = c.ISMSLEDM_Id,
                    //                                 ISMSLEDMPR_Remarks = b.ISMSLEDMPR_Remarks,
                    //                                 ISMSMST_Id = b.ISMSMST_Id,
                    //                                 ISMSMST_StatusName = e.ISMSMST_StatusName,
                    //                                 ISMSLEDMPR_SendQuotationFlg = b.ISMSLEDMPR_SendQuotationFlg
                    //                                 ,ISMSBOM_BOMName = f.ISMSBOM_BOMName
                    //                             }).Distinct().ToArray();

                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sales_lead_DemoDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMSLEDM_Id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLEDM_Id
                        });
                        //cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        //{
                        //    Value = dto.ISMSLE_Id
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
                            dto.demo_response_details = retObject.ToArray();
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
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                var demo_response_list = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                                          from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                          from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                          from a in _vmsconte.ISM_Sales_Lead_DMO_con 
                                          where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id && c.ISMSLE_Id == dto.ISMSLE_Id
                                           && b.ISMSMPR_Id == d.ISMSMPR_Id && a.ISMSLE_Id==c.ISMSLE_Id

                                          select new ISM_Sales_Lead_Demo_DTO
                                          {
                                              ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                                              ISMSMPR_Id = b.ISMSMPR_Id,
                                              ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints,
                                              ISMSLEDM_Id = c.ISMSLEDM_Id,
                                              ISMSLE_Id = c.ISMSLE_Id,
                                              ISMSBOM_Id=b.ISMSBOM_Id,
                                              ISMSLE_STUDENTSTRENGTH=a.ISMSLE_StudentStrength
                                          }).ToList();

                dto.demo_response_list = demo_response_list.ToArray();

                dto.status_demo_master = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.ISMSMST_ActiveFlag == true && a.MI_Id==dto.MI_Id).ToArray();
                string product_ids = "0";
                if(demo_response_list !=null  && demo_response_list.Count >0)
                {
                    foreach(var d in demo_response_list)
                    {
                        product_ids = product_ids + "," + d.ISMSMPR_Id;
                    }
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    //
                    cmd.CommandText = "ISM_Sales_Lead_BOM_template";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                        dto.demo_listnew = retObject.ToArray();
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
            }
            return dto;
        }
        //client  mail
        public string sendmailsclient(long MI_Id, string Email, string Template, long UserID,string tempatename)
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

                    cmd.CommandText = "SMSMAILPARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
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
                    List<ISM_Sales_Lead_Demo_DTO> dtonewtemp = new List<ISM_Sales_Lead_Demo_DTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_Leadccmailss";
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
                    //  string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
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
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
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
                                if (c != Email)
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
                                //adedd

                                //addd
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


        //mail
        public string sendmails(long MI_Id, string Email, string Template, long UserID, long user_id)
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

                    cmd.CommandText = "SMSMAILPARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
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
                    List<ISM_Sales_Lead_Demo_DTO> dtonewtemp = new List<ISM_Sales_Lead_Demo_DTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_Leadccmailss";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.BigInt)
                        {
                            Value = user_id
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
                   // string SendingEmail = "noreply@vapstech.com";
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
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
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
                                if (c != Email)
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

        //Lead Email-------------
        public string sendmail(long MI_Id, string Email, string Template, long UserID)
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

                    cmd.CommandText = "SMSMAILPARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
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
                    List<ISM_Sales_Lead_Demo_DTO> dtonewtemp = new List<ISM_Sales_Lead_Demo_DTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_Leadccmails";
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
                     //string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
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
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
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
                                if (c != Email)
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
        //Send Mail  
        //sendmailNew
        public string sendmailNew(long MI_Id, string Email, string Template, long UserID,string FHEAD,string esubject,string msg)
        {
              MI_Id = 17;
            Dictionary<string, string> val = new Dictionary<string, string>();//CleintInvoiceMail
            var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
          
            if (template.Count == 0)
            {
                return "success";
            }
            var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
          
            string Mailmsg = msg;
          
            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
            alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
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

               
                if (FHEAD != null && FHEAD != "")
                {
                    string[] ccmail = FHEAD.Split(',');
                    foreach (var c in ccmail)
                    {
                        if (c != "")
                        {
                            message.AddCc(c);

                        }
                    }
                    message.AddCc("mktgcoord@vapstech.com");
                    message.AddCc("pstomd@vapsknowledge.com");                    
                }
                else
                {
                    message.AddCc("mktgcoord@vapstech.com");
                    message.AddCc("pstomd@vapsknowledge.com");
                }
                message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                if (esubject != null && esubject != "")
                {
                    message.Subject =esubject;
                }
                else
                {
                    message.Subject = Subject;
                }
                string emailid = "sanjeev@globalqtytrainig.com";
               // string emailid = Email;
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

                    }
                }
            }
            return "success";
        }
        public ISM_Sales_Lead_Demo_DTO getalllead(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                //var lead = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true).ToList();
                //if(lead.Count>0)
                //{
                //    dto.leadlist = lead.ToArray();
                //}
                dto.roomlist = _vmsconte.HR_Master_Room_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMR_ActiveFlag == true).OrderBy(a => a.HRMR_RoomName).ToArray();
                dto.leadlist = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                                from b in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                where a.MI_Id == dto.MI_Id && a.ISMSLE_Id ==b.ISMSLE_Id &&  a.ISMSLE_ActiveFlag == true && b.ISMSLEDM_Status_Flg ==1
                                select new ISM_Sales_Lead_Demo_DTO
                                {
                                    ISMSLE_LeadName = a.ISMSLE_LeadName,
                                    ISMSLE_Id = a.ISMSLE_Id
                                }).ToArray();

                dto.incentiveleads=_vmsconte.ISM_Sales_Lead_DMO_con.Where(a =>  a.ISMSLE_OrderConfirmedFlg == true).OrderBy(a => a.ISMSLE_LeadName).ToArray();
                dto.incentiveemployee = _vmsconte.Hr_Master_Employee_con.Where(a => a.HRME_ActiveFlag == true && a.HRME_LeftFlag==false).OrderBy(a => a.HRME_Id).ToArray();

               

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Sales_Lead_Demo_DTO demo_report(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                string leadid = "0";

                //if (dto.leaddatalist.Length > 0)
                //{
                //    foreach (var item in dto.leaddatalist)
                //    {
                //        leadid = leadid + "," + item.ISMSLE_Id;
                //    }

                //}
                //string roomid = "0";

                //if (dto.roomdatalist.Length > 0)
                //{
                //    foreach (var item in dto.roomdatalist)
                //    {
                //        roomid = roomid + "," + item.HRMR_Id;
                //    }

                //}



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_Demo_Reports";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                    {
                        Value = leadid
                    });

                    //cmd.Parameters.Add(new SqlParameter("@HRMR_Id", SqlDbType.VarChar)
                    //{
                    //    Value = roomid
                    //});

                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });

                    cmd.Parameters.Add(new SqlParameter("@Online", SqlDbType.VarChar)
                    {
                        Value = dto.Online
                    });

                    cmd.Parameters.Add(new SqlParameter("@OffLine", SqlDbType.VarChar)
                    {
                        Value = dto.OffLine
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
                        dto.leaddemoreport = retObject.ToArray();
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

            }

            return dto;
        }


        //democancelreport
        public ISM_Sales_Lead_Demo_DTO DemoCancelReportNew(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {

                    string startdate = "";
                    string enddate = "";
                    if (dto.FROM_DATE != null && dto.TO_DATE != null)
                    {
                        startdate = dto.FROM_DATE.Value.ToString("yyyy-MM-dd");
                        enddate = dto.TO_DATE.Value.ToString("yyyy-MM-dd");
                    }
                    cmd.CommandText = "Sales_lead_ConfirmCancel_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = dto.Report_Type
                    });
                    cmd.Parameters.Add(new SqlParameter("@Designation", SqlDbType.VarChar)
                    {
                        Value = dto.EmpType
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = enddate
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
                        dto.DemoCancelReportNew = retObject.ToArray();
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

            }

            return dto;
        }
        //demo status
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo_status(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                //dto.lead_list_dd = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                    from b in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                    where a.ISMSLE_Id==b.ISMSLE_Id && a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true && a.ISMSLE_VerifiedFlg==true 
                //                    select new ISM_Sales_Lead_Demo_DTO
                //                    {
                //                        ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                        ISMSLE_Id = a.ISMSLE_Id,
                //                        ISMSLEDM_DemoDate=b.ISMSLEDM_DemoDate,
                //                        ISMSLEDM_DemoScheduledBy=b.ISMSLEDM_DemoScheduledBy,
                //                        ISMSLEDM_DemoType=b.ISMSLEDM_DemoType
                //                    }).ToArray();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_Demodata";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.lead_list_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_DemoList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.demo_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //dto.demo_list = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                 from b in _vmsconte.ISM_Sales_Master_Status_DMO_con
                //                 from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                 where a.ISMSMST_Id == b.ISMSMST_Id && a.ISMSLE_Id == c.ISMSLE_Id && a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true && a.ISMSLE_VerifiedFlg == true && c.ISMSLEDM_DemoExecuted == true

                //                 select new ISM_Sales_Lead_Demo_DTO
                //                 {
                //                     ISMSLEDM_Id = c.ISMSLEDM_Id,
                //                     ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                     ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                //                     ISMSLE_Id = a.ISMSLE_Id,
                //                     ISMSLEDM_DemoType = c.ISMSLEDM_DemoType,
                //                     ISMSMST_StatusName = b.ISMSMST_StatusName,
                //                     ISMSMST_Id = b.ISMSMST_Id,
                //                     ISMSLEDM_ContactPerson = c.ISMSLEDM_ContactPerson,
                //                     ISMSLEDM_DemoDate = c.ISMSLEDM_DemoDate,
                //                     ISMSLEDM_ActiveFlag = c.ISMSLEDM_ActiveFlag,
                //                     ISMSLEDM_Status_Flg = c.ISMSLEDM_Status_Flg
                //                 }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo_response(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                if (dto.LeadlstStatus != null && dto.LeadlstStatus.Length > 0)
                {
                    foreach (var d in dto.LeadlstStatus)
                    {
                        //var mapp = _vmsconte.ISM_Sales_Lead_DMO_con.Where(R => R.ISMSLE_Id == d.ISMSLE_Id ).FirstOrDefault();
                        //mapp.ISMSMST_Id = d.ISMSMST_Id;
                        //_vmsconte.Update(mapp);


                        var mapp_lead = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Where(R => R.ISMSLE_Id == d.ISMSLE_Id && R.ISMSLEDM_Id == d.ISMSLEDM_Id).FirstOrDefault();
                        mapp_lead.ISMSLEDM_DemoExecuted = true;
                        _vmsconte.Update(mapp_lead);

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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Sales_Lead_Demo_DTO getLeadResponseDetails(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {


                //dto.lead_list_dd = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                    from b in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                    where a.ISMSLE_Id == b.ISMSLE_Id && a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true && a.ISMSLE_VerifiedFlg == true
                //                    select new ISM_Sales_Lead_Demo_DTO
                //                    {
                //                        ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                        ISMSLE_Id = a.ISMSLE_Id,
                //                        ISMSLEDM_DemoDate = b.ISMSLEDM_DemoDate,
                //                        ISMSLEDM_DemoScheduledBy = b.ISMSLEDM_DemoScheduledBy,
                //                        ISMSLEDM_DemoType = b.ISMSLEDM_DemoType
                //                    }).ToArray();
                dto.status_demo_master = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.ISMSMST_ActiveFlag == true).ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_Demo_response_data";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.user_id
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
                        dto.leaddeResponseData = retObject.ToArray();
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

            }

            return dto;
        }


        //mapping channel patner

        public ISM_Sales_Lead_DTO load_all_lead_mapping(ISM_Sales_Lead_DTO dto)
        {
            try
            {

                dto.yearlist = _vmsconte.IVRM_Master_FinancialYearDMO.OrderByDescending(s => s.IMFY_FinancialYear).ToArray();
                //dto.ChannalPatnerList = _vmsconte.ISM_Sales_Master_ChannelPartnerDMO.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMCP_ActiveFlag == true).ToArray();




                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ChannelPartnerAndLeadList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
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
                        dto.lead_list = retObject.ToArray();
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
            }
            return dto;
        }

        public ISM_Sales_Lead_DTO getChannelPatner(ISM_Sales_Lead_DTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Financial_ChannelPartnerList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
                    {
                        Value = dto.IMFY_Id
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
                        dto.ChannalPatnerList = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                dto.leadlists = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public ISM_Sales_Lead_DTO save_ChannelPatnerMapping(ISM_Sales_Lead_DTO dto)
        {
            try
            {


                if (dto.ISMSLE_Id == 0)
                {
                    
                    if (dto.Leadlsttwo.Length > 0)
                    {
                        foreach (var ue in dto.Leadlsttwo)
                        {
                            var checkduplicate1 = _vmsconte.ISM_Sales_Lead_DMO_con.Where(d => d.MI_Id == dto.MI_Id && (d.ISMSMCP_Id == dto.ISMSMCP_Id && d.ISMSLE_Id == ue.ISMSLE_Id)).ToList();
                            if (checkduplicate1.Count == 0)
                            {
                                var result1 = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == ue.ISMSLE_Id && a.ISMSMCP_Id == dto.ISMSMCP_Id).ToArray();
                                if (result1.Length > 0)
                                {

                                }
                                else
                                {
                                    var result = _vmsconte.ISM_Sales_Lead_DMO_con.Single(a => a.ISMSLE_Id == ue.ISMSLE_Id);
                                    result.ISMSMCP_Id = dto.ISMSMCP_Id;
                                    result.IMFY_Id = dto.IMFY_Id;
                                    result.UpdatedDate = DateTime.Now;
                                    result.ISMSLE_UpdatedBy = dto.user_id;
                                    _vmsconte.Update(result);
                                }

                            }

                        }

                        int s = _vmsconte.SaveChanges();
                        if (s > 0)
                        {
                            dto.msg = "Add";
                        }
                        else
                        {
                            dto.msg = "savingFailed";
                        }

                    }

                }
                else
                {
                    var duplicate = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == dto.ISMSLE_Id && a.ISMSMCP_Id == dto.ISMSMCP_Id).ToArray();
                    if (duplicate.Length == 0)
                    {
                        var result = _vmsconte.ISM_Sales_Lead_DMO_con.Single(a => a.ISMSLE_Id == dto.ISMSLE_Id);
                        result.ISMSMCP_Id = dto.ISMSMCP_Id;
                       // result.IMFY_Id = dto.IMFY_Id;
                        result.UpdatedDate = DateTime.Now;
                        result.ISMSLE_UpdatedBy = dto.user_id;
                        _vmsconte.Update(result);

                    }

                    int s = _vmsconte.SaveChanges();
                    if (s > 0)
                    {
                        dto.msg = "Add";
                    }
                    else
                    {
                        dto.msg = "savingFailed";
                    }

                }


            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Sales_Lead_DTO leadChannelmapping_edit(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Financial_ChannelPartnerList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
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
                        dto.ChannalPatnerList = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                dto.leadlists = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true && a.ISMSLE_VerifiedFlg == true).ToArray();



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ChannelPartnerAndLeadList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });


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
                        dto.lead_list = retObject.ToArray();
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
            }
            return dto;
        }



        public ISM_Sales_Lead_DTO getverifyleaddetails(ISM_Sales_Lead_DTO data)
        {
            try
            {
                data.fillyear = _vmsconte.IVRM_Master_FinancialYearDMO.Select(a => a).ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_confirmListFinal";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
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
                        data.finalleadlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_confirmList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
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
                        data.leadlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                
          
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Sales_Lead_DTO getleaddetails(ISM_Sales_Lead_DTO data)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_confirmList_send";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = data.@ISMSLE_Id
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
                        data.get_lead_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // data.get_lead_details = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_Id == data.ISMSLE_Id).Select(a => a).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ISM_Sales_Lead_DTO getleaddetailsfinal(ISM_Sales_Lead_DTO data)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_confirmList_sendfinal";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = data.@ISMSLE_Id
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
                        data.get_lead_detailsfinal = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // data.get_lead_details = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_Id == data.ISMSLE_Id).Select(a => a).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ISM_Sales_Lead_DTO sendmailDetails(ISM_Sales_Lead_DTO data)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";
                string Template = "Client Quotation";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
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

                Mailmsg = Mailmsg.Replace("[MSG]", data.Template);
                Mailcontent = Resultsms;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                string Attechement = "";
                try
                {
                    List<ISM_Sales_Lead_Demo_DTO> dtonewtemp = new List<ISM_Sales_Lead_Demo_DTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_ClientQuotationccmailss";
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
                    //  string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
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
                            memoryStream.Close();

                            var file = Convert.ToBase64String(bytess);
                            string emp;
                            emp = Convert.ToString(sr);
                            string c = "";
                            string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");
                            message.AddAttachment(v, file);
                            message.HtmlContent = Mailmsg;
                            var client = new SendGridClient(sengridkey);
                            client.SendEmailAsync(message).Wait();
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
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);            
            }
            return data;
        }


        public ISM_Sales_Lead_DTO updatedetails(ISM_Sales_Lead_DTO data)
        {
            try
            {
                if (data.ISMSLE_Id > 0)
                {
                    var update = _vmsconte.ISM_Sales_Lead_DMO_con.Single(d => d.ISMSLE_Id == data.ISMSLE_Id && d.MI_Id == data.MI_Id);

                    update.ISMSLE_DedicatedHostingFlg = data.ISMSLE_DedicatedHostingFlg;
                    update.ISMSLE_DedicatedMobileAppFlg = data.ISMSLE_DedicatedMobileAppFlg;
                    update.ISMSLE_HostingServer = data.ISMSLE_HostingServer;
                    update.ISMSLE_QuotationValue = data.ISMSLE_QuotationValue;
                    _vmsconte.Update(update);

                    int s = _vmsconte.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "Update";

                    }
                    else
                    {
                        data.msg = "updateFailed";
                    }

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public ISM_Sales_Lead_DTO get_leads(ISM_Sales_Lead_DTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FinacialYrwiseLead";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.IMFY_Id
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //dataesFormat
                string startdate = "";
                string enddate = "";
                if (dto.FROMDATE != null && dto.TODATE != null)
                {
                    startdate = dto.FROMDATE.Value.ToString("yyyy-MM-dd");
                    enddate = dto.TODATE.Value.ToString("yyyy-MM-dd");                  
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_RawLeads_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = enddate
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
                        dto.leaddropdown = retObject.ToArray();
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


        public ISM_Sales_Lead_DTO Reschedule(ISM_Sales_Lead_DTO dto)
        {
            try
            {

                foreach (var e in dto.ISMSLEDM_Id)
                {
                    var update = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Single(d => d.ISMSLEDM_Id == e.ISMSLEDM_Id);

                    update.ISMSLEDM_RescheduleFlag = true;
                    update.UpdatedDate = DateTime.Now;
                    update.ISMSLEDM_UpdatedBy = dto.user_id;
                    _vmsconte.Update(update);

                }
                int s = _vmsconte.SaveChanges();
                if (s > 0)
                {
                    dto.msg = "Reschedule";
                }
                else
                {
                    dto.msg = "Not-Reschedule";
                }
 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        //nagaraj added 

        public ISM_Sales_Lead_DTO CancelNew(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                    var update = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Single(d => d.ISMSLEDM_Id == dto.ISMSLEDM_Id_Cancel);
                    update.ISMSLEDM_DemoCancelledFlg = true;
                    update.ISMSLEDM_DemoCancel_Remark = dto.CancelRemarks;
                    update.ISMSLEDM_Status_Flg = 1;
                    update.ISMSLEDM_DemoCancelledFlg = true;
                    update.UpdatedDate = DateTime.Now;
                    update.ISMSLEDM_UpdatedBy = dto.user_id;
                    _vmsconte.Update(update);

           
                int s = _vmsconte.SaveChanges();
                if (s > 0)
                {
                    dto.msg = "CancelNew";
                }
                else
                {
                    dto.msg = "Not-Cancel";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public ISM_Sales_Lead_Demo_DTO onreport(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                if(dto.reporttype=="Product")
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISm_Sales_ProductList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
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
                            dto.product_dd_s = retObject.ToArray();
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
                        cmd.CommandText = "Sales_Incentive_share";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_id", SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date)
                        {
                            Value = dto.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.Date)
                        {
                            Value = dto.todate
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = dto.reporttype
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
                            if (dto.reporttype == "Employee")
                            {
                                dto.get_employeereport = retObject.ToArray();
                            }
                            else if (dto.reporttype == "Lead")
                            {
                                dto.get_leadreport = retObject.ToArray();
                            }
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

            }

            return dto;
        }

      
        public ISM_Sales_Lead_Demo_DTO getleaddetail(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Incentive_share_Dept";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_id", SqlDbType.BigInt)
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

                        dto.getleaddetails = retObject.ToArray();                       
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
            }
            return dto;
        }


        public ISM_Sales_Lead_DTO LeadUpdatemail(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                if(dto.Leadlsttwo !=null  && dto.Leadlsttwo.Length >0)
                {
                    foreach(var d  in dto.Leadlsttwo)
                    {
                        ISM_Sales_Lead_EmailId_mobileNoDMO email = new ISM_Sales_Lead_EmailId_mobileNoDMO();
                        email.ISMSLEEM_EmailId = d.ISMSLEEM_EmailId;
                        email.ISMSLEEM_MobileNo = d.ISMSLEEM_MobileNo;
                        if(d.ISMSLEEM_DefaultFlag== "default")
                        {
                            var updates = _vmsconte.ISM_Sales_Lead_DMO_con.Single(R => R.ISMSLE_Id == d.ISMSLE_Id);
                            updates.ISMSLE_ContactNo =Convert.ToInt64(d.ISMSLEEM_MobileNo);


                        }
                        email.ISMSLEEM_DefaultFlag = d.ISMSLEEM_DefaultFlag;
                        email.ISMSLE_Id = dto.ISMSLE_Id;
                        _vmsconte.Add(email);
                    }
                   
                }
              else if(dto.ISM_DemoRatingsNew !=null  && dto.ISM_DemoRatingsNew.Length >0)
                {
                    var stafflist = _db.Staff_User_Login.Where(R => R.Id == dto.user_id).ToList();
                    if(stafflist.Count == 0 || stafflist == null)
                    {
                        dto.retrunMsg = "NotAccess";
                        return dto;
                    }
                    foreach (var d in dto.ISM_DemoRatingsNew)
                    {
                        var duplicate = _vmsconte.ISM_CA_Demo_RatingsDMO.Where(R => R.ICDR_GivenBy_EmpID == d.ICDR_GivenBy_EmpID && R.ICDR_ProductID == d.ICDR_ProductID && R.ICDR_GivenTo_EmpId == stafflist.FirstOrDefault().Emp_Code && R.IVRM_Month_Id == dto.IVRM_Month_Id && R.IMFY_FinancialYear == dto.IMFY_FinancialYear).ToList();
                        if(duplicate.Count >0)
                        {
                            foreach(var dd  in duplicate)
                            {
                                var obj = _vmsconte.ISM_CA_Demo_RatingsDMO.Where(R => R.ICDR_ID == dd.ICDR_ID).FirstOrDefault();
                                obj.MI_id = dto.MI_Id;
                                obj.ICDR_GivenBy_EmpID = stafflist.FirstOrDefault().Emp_Code;
                                // obj.ICDR_GivenBy_EmpID = d.ICDR_GivenBy_EmpID;
                                obj.ICDR_ProductID = d.ICDR_ProductID;
                                //obj.ICDR_GivenTo_EmpId = stafflist.FirstOrDefault().Emp_Code;
                                obj.ICDR_GivenTo_EmpId = d.ICDR_GivenBy_EmpID;
                                obj.ICDR_DemoDate = d.ICDR_DemoDate;
                                obj.ICDR_Remarks = d.ICDR_Remarks;
                                obj.ICDR_Ratings = d.ICDR_Ratings;
                                obj.ICDR_Active = true;                                                         
                                obj.ICDR_UpdateDate = DateTime.Now;
                                obj.ICDR_Updateby = dto.user_id;
                                obj.IVRM_Month_Id = dto.IVRM_Month_Id;
                                obj.IMFY_FinancialYear = dto.IMFY_FinancialYear;
                                _vmsconte.Update(obj);
                            }
                        }
                        else
                        {
                            ISM_CA_Demo_RatingsDMO obj = new ISM_CA_Demo_RatingsDMO();
                            obj.MI_id = dto.MI_Id;
                            obj.ICDR_GivenBy_EmpID = stafflist.FirstOrDefault().Emp_Code;
                            //obj.ICDR_GivenBy_EmpID = d.ICDR_GivenBy_EmpID;
                            obj.ICDR_ProductID = d.ICDR_ProductID;
                            // obj.ICDR_GivenTo_EmpId = stafflist.FirstOrDefault().Emp_Code;
                            obj.ICDR_GivenTo_EmpId = d.ICDR_GivenBy_EmpID;
                            obj.ICDR_DemoDate = d.ICDR_DemoDate;
                            obj.ICDR_Remarks = d.ICDR_Remarks;
                            obj.ICDR_Ratings = d.ICDR_Ratings;
                            obj.ICDR_Active = true;
                            obj.ICDR_CreatedBy = dto.user_id;
                            obj.ICDR_CreatedDate = DateTime.Now;
                            obj.ICDR_UpdateDate = DateTime.Now;
                            obj.ICDR_Updateby = dto.user_id;
                            obj.IVRM_Month_Id = dto.IVRM_Month_Id;
                            obj.IMFY_FinancialYear = dto.IMFY_FinancialYear;
                            _vmsconte.Add(obj);

                        }

                    }
                    int i = _vmsconte.SaveChanges();
                    if(i > 0)
                    {
                        dto.retrunMsg = "save";
                    }
                    else
                    {
                        dto.retrunMsg = "notsave";
                    }
                }

                //StudentStrength_Mapping
              else  if(dto.StudentStrength_Mapping !=null && dto.StudentStrength_Mapping.Length >0)
                {
                    foreach(var d  in dto.StudentStrength_Mapping)
                    {
                        var duplicate = _vmsconte.ISM_CA_StudentStrength_MappingDMO.Where(R => R.ICSSM_Strenth == d.ICSSM_Strenth && R.ICSSM_PrecentageFrom == d.ICSSM_PrecentageFrom && R.ICSSM_PrecentageTo == d.ICSSM_PrecentageTo).ToList();
                        if(duplicate.Count >0)
                        {
                            foreach (var dd in duplicate)
                            {
                                var obj = _vmsconte.ISM_CA_StudentStrength_MappingDMO.Where(R => R.ICSSM_ID == dd.ICSSM_ID).FirstOrDefault();
                                obj.ICSSM_Strenth = d.ICSSM_Strenth;
                                obj.ICSSM_PrecentageFrom = d.ICSSM_PrecentageFrom;
                                obj.ICSSM_PrecentageTo = d.ICSSM_PrecentageTo;                             
                                obj.ICSSM_Updatedby = dto.user_id;
                                obj.ICSSM_UpdateOn = DateTime.Now;
                                _vmsconte.Update(obj);
                            }
                        }
                        else
                        {
                            ISM_CA_StudentStrength_MappingDMO obj = new ISM_CA_StudentStrength_MappingDMO();
                            obj.ICSSM_Strenth = d.ICSSM_Strenth;
                            obj.ICSSM_PrecentageFrom = d.ICSSM_PrecentageFrom;
                            obj.ICSSM_PrecentageTo = d.ICSSM_PrecentageTo;
                            obj.ICSSM_Active =true;
                            obj.MI_ID = dto.MI_Id;
                            obj.ICSSM_Createdby = dto.user_id;
                            obj.ICSSM_CreatedOn = DateTime.Now;
                            obj.ICSSM_Updatedby = dto.user_id;
                            obj.ICSSM_UpdateOn = DateTime.Now;
                            _vmsconte.Add(obj);
                        }
                    }
                    int i = _vmsconte.SaveChanges();
                    if (i > 0)
                    {
                        dto.retrunMsg = "save";
                    }
                    else
                    {
                        dto.retrunMsg = "notsave";
                    }
                }
                else
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_CA_StudentStrength_Mapping_New";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                        {
                            Value = dto.user_id
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

                            dto.viewdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


        //nagaraj

        public ISM_Sales_Lead_DTO Sales_Raw_Report_OnclickGridData(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                string startdate = "";
                string enddate = "";
                if (dto.FROMDATE != null && dto.TODATE != null)
                {
                    startdate = dto.FROMDATE.Value.ToString("yyyy-MM-dd");
                    enddate = dto.TODATE.Value.ToString("yyyy-MM-dd");
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_RawLeads_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                   SqlDbType.VarChar)
                    {
                        Value = enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag1",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag1
                    });
                    cmd.Parameters.Add(new SqlParameter("Flag2",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag2
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var ret = new List<dynamic>();
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

                                ret.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.Sales_Raw_Report_OnclickGridData = ret.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_RawLeads_Report_Details_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                   SqlDbType.VarChar)
                    {
                        Value = enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag1",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag1
                    });
                    cmd.Parameters.Add(new SqlParameter("Flag2",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag2
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSMSO_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag2
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag2
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var ret = new List<dynamic>();
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

                                ret.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.Sales_Raw_Report_OnclickGridData = ret.ToArray();
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

        //nagaraj

        public ISM_Sales_Lead_DTO Swap(ISM_Sales_Lead_DTO dto)
        {
            var duplicate = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Where(a => a.ISMSLEDM_Id == dto.ISMSLEDM_IdNew && a.HRME_Id == dto.HRME_Id).ToList();
            if (duplicate.Count > 0 && duplicate != null)
            {
                dto.msg = "Duplicate";
            }
            else
            {



                try
                {
                    var result = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Single(a => a.ISMSLEDM_Id == dto.ISMSLEDM_IdNew);
                    // result.ISMSLEDM_DemoScheduledBy = dto.HRME_Id;
                    var BeforeDemoScheduledTo = result.HRME_Id;

                    result.HRME_Id = dto.HRME_Id;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMSLEDM_UpdatedBy = dto.user_id;
                    result.ISMSLEDM_SwappingFlg = "1";
                    _vmsconte.Update(result);
                    try
                    {
                        ISM_Sales_Lead_Demo_SwappingDMO swapping = new ISM_Sales_Lead_Demo_SwappingDMO();

                        swapping.ISMSLEDM_Id = result.ISMSLEDM_Id;
                        swapping.MI_Id = result.MI_Id;
                        swapping.ISMSLE_Id = result.ISMSLE_Id;
                        swapping.HRME_BeforeDemoScheduledTo = BeforeDemoScheduledTo;
                        swapping.HRME_AfterDemoScheduledTo = dto.HRME_Id;
                        swapping.ISMSLEDMS_Activeflag = true;
                        swapping.ISMSLEDMS_DemoType = result.ISMSLEDM_DemoType;
                        swapping.ISMSLEDMS_DemoScheduledBy = result.ISMSLEDM_DemoScheduledBy;
                        swapping.ISMSLEDMS_CreatedBy = dto.user_id;
                        swapping.ISMSLEDMS_UpdatedBy = dto.user_id;
                        swapping.ISMSLEDMS_CreatedDate = DateTime.Now;
                        swapping.ISMSLEDMS_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(swapping);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.msg = "swap";
                    }
                    else
                    {
                        dto.msg = "Not swap";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    dto.msg = "Error";
                }

                //web push notifications
                try
                {
                    var leadid = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Single(a => a.ISMSLEDM_Id == dto.ISMSLEDM_IdNew);

                    var leadName = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == leadid.ISMSLE_Id).FirstOrDefault();


                    // dto.ISMMCLT_Id = dto.HRME_Id;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    PushNotification pb = new PushNotification(_db);

                    var hrmEempid = _vmsconte.Staff_User_Login.Where(t => t.MI_Id == dto.MI_Id && t.Id == dto.user_id).Distinct().ToArray();
                    long empid = 0;
                    if (hrmEempid.Length > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                    }

                    var userrole = _vmsconte.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();
                    string role = "";
                    if (userrole.Count > 0)
                    {
                        role = userrole.FirstOrDefault().IVRMRT_Role;
                        dto.roletype = role;

                    }


                    if (role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                    {
                        var deviceidsnew = "";
                        var devicenew = "";

                        var devicelist1 = _vmsconte.HR_Master_Employee_DMO
                                          .Where(e => e.HRME_Id == dto.HRME_Id && e.HRME_ActiveFlag == true && e.MI_Id == dto.MI_Id)
                                          .Distinct().ToList();

                        if (devicelist1.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist1)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";
                        }
                        try
                        {
                            string titletext = "Demo Swapping :- Lead Name : " + leadName.ISMSLE_LeadName + ", Contact Number : " + leadName.ISMSLE_ContactNo + ", Contact Person : " + leadName.ISMSLE_ContactPerson + ", Demo Type : " + leadid.ISMSLEDM_DemoType + "," + "Demo Date : " + leadid.ISMSLEDM_DemoDate.ToString("dd/MM/yyyy");
                            string subject = "Demo Swapping";
                            if (dto.sound == "")
                            {
                                dto.sound = "default";
                            }

                            //string subject = "Employee Punched";
                            //string sound = "";
                            // callnotificationasy(devicenew, dto.HRME_Id, empid, titletext, dto.MI_Id, dto.sound, subject);




                            callnotification(devicenew, 0, 0, titletext, dto.MI_Id, dto.sound, subject);
                            Insertnotification(dto.MI_Id, dto.HRME_Id, 0, titletext, dto.user_id, "Demo swapping");

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
                //web push notifications
            }

            return dto;
        }


        public ISM_Sales_Lead_DTO SwappingEmpList(ISM_Sales_Lead_DTO dto)
        {
            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Swapping_Emp_List";
                cmd.Parameters.Add(new SqlParameter("@Ratings", SqlDbType.Decimal)
                {
                    Value = dto.Rating
                });
                cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                {
                    Value = dto.HRME_Id
                });

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
                    dto.SwappingEmpList = retObject.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return dto;
        }




        //push notifications

        public string callnotification(string devicenew, long empid, long task_id, string titletext, long mi_id, string sound, string subject)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + indianTime + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
          "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + subject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + titletext + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());
                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";
                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string Insertnotification(long MI_Id, long empid, long task_id, string titletext, long userid, string type)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ISM_PlannerCreationDTO data = new ISM_PlannerCreationDTO();
                ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                push.MI_Id = MI_Id;
                push.HRME_Id = empid;
                push.ISMNO_Notification = titletext;
                push.ISMNO_NoticationDate = indianTime;
                push.ISMNO_NotificationType = type;
                push.ISMNO_ReadFlg = false;
                push.ISMNO_MakeUnReadFlg = false;
                push.ISMNO_ActiveFlag = true;
                push.ISMNO_ModeFlg = "WEB";
                push.ISMNO_CreatedBy = userid;
                push.ISMNO_UpdatedBy = userid;
                push.CreatedDate = indianTime;
                push.UpdatedDate = indianTime;
                _IssueContext.Add(push);
                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
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

