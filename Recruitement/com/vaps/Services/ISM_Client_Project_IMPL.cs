using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class ISM_Client_Project_IMPL : Interfaces.ISM_Client_ProjectInterface
    {
        public VMSContext _vmsconte;
       private DomainModelMsSqlServerContext _db;

        public ISM_Client_Project_IMPL(VMSContext vmscon, DomainModelMsSqlServerContext db)
        {
            _vmsconte = vmscon;
            _db = db;
        }
        //========================= master components
        public ISM_Client_Project_DTO getdate_cmc(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.components_list = _vmsconte.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO details_cmc(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.components_details = _vmsconte.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMCLTC_Id == dto.ISMCLTC_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO SaveEdit_cmc(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTC_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Master_Components_DMO_con.Single(a => a.ISMCLTC_Id == dto.ISMCLTC_Id && a.MI_Id == dto.MI_Id);


                    result.ISMCLTC_Name = dto.ISMCLTC_Name;
                    result.ISMCLTC_Description = dto.ISMCLTC_Description;
                    result.ISMCLTC_UpdatedBy = dto.User_Id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Client_Master_Components_DMO cmc = new ISM_Client_Master_Components_DMO();

                    cmc.MI_Id = dto.MI_Id;
                    cmc.ISMCLTC_Name = dto.ISMCLTC_Name;
                    cmc.ISMCLTC_Description = dto.ISMCLTC_Description;
                    cmc.ISMCLTC_ActiveFlag = true;
                    cmc.ISMCLTC_CreatedBy = dto.User_Id;
                    cmc.CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO deactivate_cmc(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Master_Components_DMO_con.Single(a => a.ISMCLTC_Id == dto.ISMCLTC_Id && a.MI_Id == dto.MI_Id);
                if (dto.ISMCLTC_ActiveFlag == true)
                {
                    result.ISMCLTC_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTC_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        //======================================BOM

        public ISM_Client_Project_DTO getdata_BOM(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DD_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.clientproject_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_BOM_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.bom_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                dto.components_dd = _vmsconte.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMCLTC_ActiveFlag == true).ToArray();
                dto.AcademicYearlist= _vmsconte.AcademicYear.Where(b=>b.MI_Id==dto.MI_Id && b.Is_Active == true).ToArray();
                dto.clientprojectdata = _vmsconte.MastersProject_DMO.Where(c =>c.ISMMPR_ActiveFlg==true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO details_BOM(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_BOM_Details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMCLTPRBOM_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMCLTPRBOM_Id
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
                        dto.bom_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DD_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.clientproject_dd = retObject.ToArray();
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


        public ISM_Client_Project_DTO SaveEdit_BOM(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTPRBOM_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Project_BOM_DMO_con.Single(a => a.ISMCLTPRBOM_Id == dto.ISMCLTPRBOM_Id);


                    result.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    result.ISMCLTC_Id = dto.ISMCLTC_Id;
                    result.ISMCLTPRBOM_Qty = dto.ISMCLTPRBOM_Qty;
                    result.ISMCLTPRBOM_Remarks = dto.ISMCLTPRBOM_Remarks;
                    result.ISMCLTPRBOM_UpdatedBy = dto.User_Id;
                    result.UpdatedDate = DateTime.Now;
                    result.AYear = dto.AYear;
                    result.BOMFlag = dto.BOMFlag;
                    result.DCNo = dto.DCNo;
                    result.BOMDate = dto.BOMDate;
                    result.LearningID = dto.LearningID;


                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Client_Project_BOM_DMO cmc = new ISM_Client_Project_BOM_DMO();
                    cmc.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    cmc.ISMCLTC_Id = dto.ISMCLTC_Id;
                    cmc.ISMCLTPRBOM_Qty = dto.ISMCLTPRBOM_Qty;
                    cmc.ISMCLTPRBOM_Remarks = dto.ISMCLTPRBOM_Remarks;
                    cmc.ISMCLTPRBOM_ActiveFlag = true;
                    cmc.ISMCLTPRBOM_CreatedBy = dto.User_Id;
                    cmc.CreatedDate = DateTime.Now;
                    cmc.AYear = dto.AYear;
                    cmc.BOMFlag = dto.BOMFlag;
                    cmc.DCNo = dto.DCNo;
                    cmc.BOMDate = dto.BOMDate;
                    cmc.LearningID = dto.LearningID;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO deactivate_BOM(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Project_BOM_DMO_con.Single(a => a.ISMCLTPRBOM_Id == dto.ISMCLTPRBOM_Id);
                if (dto.ISMCLTPRBOM_ActiveFlag == true)
                {
                    result.ISMCLTPRBOM_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTPRBOM_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        //======================================Man power

        public ISM_Client_Project_DTO getdata_MP(ISM_Client_Project_DTO dto)
        {
            try
            {
                //employee list
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Master_Employee_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.Master_Employee_List = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DD_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.clientproject_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_MPlist_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.mp_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //getdata_MP
                //components_list
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    
                    cmd.CommandText = "ISM_Sales_Lead_Verifiy";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandTimeout = 360;


                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
                    });
                    //@Flag
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "Raw_Lead_Emp"
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
                        dto.components_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.yearlist = _vmsconte.AcademicYear.Where(t => t.MI_Id == dto.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO details_MP(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_MP_Details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMCLTPRMP_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMCLTPRMP_Id
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
                        dto.mp_details = retObject.ToArray();
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


        public ISM_Client_Project_DTO SaveEdit_MP(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTPRMP_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Project_ManPower_DMO_con.Single(a => a.ISMCLTPRMP_Id == dto.ISMCLTPRMP_Id);


                    result.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    result.ISMCLTPRMP_ResourceName = dto.ISMCLTPRMP_ResourceName;
                    result.ISMCLTPRMP_Qty = dto.ISMCLTPRMP_Qty;
                    result.ISMCLTPRMP_Remarks = dto.ISMCLTPRMP_Remarks;
                    result.DateofDeployment = dto.DateofDeployment;
                    result.EndDate = dto.EndDate;

                    result.ISMCLTPRMP_AcademicYear = dto.ISMCLTPRMP_AcademicYear;

                    result.ISMCLTPRMP_UpdatedBy = dto.User_Id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    ISM_Client_Project_ManPower_DMO cmc = new ISM_Client_Project_ManPower_DMO();

                    cmc.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    cmc.ISMCLTPRMP_ResourceName = dto.ISMCLTPRMP_ResourceName;
                    cmc.ISMCLTPRMP_Qty = dto.ISMCLTPRMP_Qty;
                    cmc.ISMCLTPRMP_Remarks = dto.ISMCLTPRMP_Remarks;
                    cmc.ISMCLTPRMP_AcademicYear = dto.ISMCLTPRMP_AcademicYear;
                    cmc.DateofDeployment = dto.DateofDeployment;
                    cmc.EndDate = dto.EndDate;
                    cmc.ISMCLTPRMP_ActiveFlag = true;
                    cmc.ISMCLTPRMP_CreatedBy = dto.User_Id;
                    cmc.CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO deactivate_MP(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Project_ManPower_DMO_con.Single(a => a.ISMCLTPRMP_Id == dto.ISMCLTPRMP_Id);
                if (dto.ISMCLTPRMP_ActiveFlag == true)
                {
                    result.ISMCLTPRMP_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTPRMP_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        //======================================MASTER DOC

        public ISM_Client_Project_DTO getdata_MDOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.docmaster_list = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO details_MDOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.docmaster_details = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMCLTPRMDOC_Id == dto.ISMCLTPRMDOC_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO SaveEdit_MDOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTPRMDOC_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Single(a => a.ISMCLTPRMDOC_Id == dto.ISMCLTPRMDOC_Id && a.MI_Id == dto.MI_Id);


                    result.ISMCLTPRMDOC_Name = dto.ISMCLTPRMDOC_Name;
                    result.ISMCLTPRMDOC_Description = dto.ISMCLTPRMDOC_Description;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Client_Project_Master_Docs_DMO cmc = new ISM_Client_Project_Master_Docs_DMO();

                    cmc.ISMCLTPRMDOC_Name = dto.ISMCLTPRMDOC_Name;
                    cmc.MI_Id = dto.MI_Id;
                    cmc.ISMCLTPRMDOC_Description = dto.ISMCLTPRMDOC_Description;
                    cmc.ISMCLTPRMDOC_ActiveFlag = true;
                    cmc.CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO deactivate_MDOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Single(a => a.ISMCLTPRMDOC_Id == dto.ISMCLTPRMDOC_Id && a.MI_Id == dto.MI_Id);
                if (dto.ISMCLTPRMDOC_ActiveFlag == true)
                {
                    result.ISMCLTPRMDOC_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTPRMDOC_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        //======================================  DOC

        public ISM_Client_Project_DTO getdata_DOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DD_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.clientproject_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DOClist_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.doc_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                dto.document_dd = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMCLTPRMDOC_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO details_DOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DOC_Details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMCLTPRDOC_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMCLTPRDOC_Id
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
                        dto.doc_details = retObject.ToArray();
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
        public ISM_Client_Project_DTO SaveEdit_DOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.Database.ExecuteSqlCommand
                    ("VMS_InsertClientProjectDoc" +
                    " @p0," +
                    "@p1," +
                    "@p2," +
                    "@p3," +
                    "@p4," +
                    "@p5", +
                    dto.ISMMCLTPR_Id,
                    dto.ISMCLTPRMDOC_Id,
                    dto.ISMCLTPRDOC_FileName,
                    dto.ISMCLTPRDOC_FilePath,
                    dto.ISMCLTPRDOC_Date,
                    dto.User_Id
                    );
                if (result > 0)
                {
                    dto.returndata = "saved";
                }
                else
                {
                    dto.returndata = "Failed";
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return dto;

            //try
            //{
            //    if (dto.ISMCLTPRDOC_Id > 0)
            //    {
            //        var result = _vmsconte.ISM_Client_Project_Docs_DMO_con.Single(a => a.ISMCLTPRDOC_Id == dto.ISMCLTPRDOC_Id);


            //        result.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
            //        result.ISMCLTPRMDOC_Id = dto.ISMCLTPRMDOC_Id;
            //        result.ISMCLTPRDOC_FileName = dto.ISMCLTPRDOC_FileName;
            //        result.ISMCLTPRDOC_FilePath = dto.ISMCLTPRDOC_FilePath;
            //        result.ISMCLTPRDOC_Date = dto.ISMCLTPRDOC_Date;
            //        result.ISMCLTPRDOC_UpdatedBy = dto.User_Id;
            //        result.UpdatedDate = DateTime.Now;
            //        _vmsconte.Update(result);
            //        var vv = _vmsconte.SaveChanges();
            //        if (vv > 0)
            //        {
            //            dto.returndata = "Update";
            //        }
            //        else
            //        {
            //            dto.returndata = "Error";
            //        }



            //    }
            //    else
            //    {
            //        ISM_Client_Project_Docs_DMO cmc = new ISM_Client_Project_Docs_DMO();
            //        cmc.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
            //        cmc.ISMCLTPRMDOC_Id = dto.ISMCLTPRMDOC_Id;
            //        cmc.ISMCLTPRDOC_FileName = dto.ISMCLTPRDOC_FileName;
            //        cmc.ISMCLTPRDOC_FilePath = dto.ISMCLTPRDOC_FilePath;
            //        cmc.ISMCLTPRDOC_Date = dto.ISMCLTPRDOC_Date;
            //        cmc.ISMCLTPRDOC_ActiveFlag = true;
            //        cmc.ISMCLTPRDOC_CreatedBy = dto.User_Id;
            //        cmc.CreatedDate = DateTime.Now;
            //        _vmsconte.Add(cmc);
            //        var vv = _vmsconte.SaveChanges();
            //        if (vv > 0)
            //        {
            //            dto.returndata = "Add";
            //        }
            //        else
            //        {
            //            dto.returndata = "Error";
            //        }

            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);

            //}

            //return dto;
        }
        public ISM_Client_Project_DTO deactivate_DOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Project_Docs_DMO_con.Single(a => a.ISMCLTPRDOC_Id == dto.ISMCLTPRDOC_Id);
                if (dto.ISMCLTPRDOC_ActiveFlag == true)
                {
                    result.ISMCLTPRDOC_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTPRDOC_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        //=============Lead Comments

        public ISM_Client_Project_DTO getdata_CMT(ISM_Client_Project_DTO dto)
        {
            try
            {
                //dto.commentlist = (from a in _vmsconte.ISM_Sales_Lead_Comments_DMO_con
                //                   from b in _vmsconte.ISM_Sales_Lead_DMO_con
                //                   where a.ISMSLE_Id == b.ISMSLE_Id && a.MI_Id == dto.MI_Id
                //                   select new ISM_Client_Project_DTO
                //                   {
                //                       ISMSLECOM_Id = a.ISMSLECOM_Id,
                //                       ISMSLECOM_Comments = a.ISMSLECOM_Comments,
                //                       ISMSLE_LeadName = b.ISMSLE_LeadName,
                //                       ISMSLECOM_ActiveFlag = a.ISMSLECOM_ActiveFlag,
                //                       ISMSLECOM_SendQuotationFlg = a.ISMSLECOM_SendQuotationFlg
                //                   }).OrderByDescending(t => t.ISMSLECOM_Id).ToArray();

                //nagaraj
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Lead_Comments";
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
                        dto.commentlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                //dto.leadlist_dd = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true).Distinct().ToArray();
                //  dto.project_list = _vmsconte.MastersProject_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMPR_ActiveFlg == true).Distinct().ToArray();


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
                        dto.leadlist_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "followupleadlist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSMST_StatusName", SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt)
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
                        dto.followuplead = retObject.ToArray();
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
        public ISM_Client_Project_DTO details_CMT(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.edit_comment = _vmsconte.ISM_Sales_Lead_Comments_DMO_con.Where(a => a.ISMSLECOM_Id == dto.ISMSLECOM_Id && a.MI_Id == dto.MI_Id).ToArray();

                dto.leadlist_dd = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true).ToArray();
                dto.editArryThree = _vmsconte.ISM_Sales_Lead_CommentsFilesDMO.Where(a => a.ISMSLECOM_Id == dto.ISMSLECOM_Id).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        //public ISM_Client_Project_DTO SaveEdit_CMT(ISM_Client_Project_DTO dto)
        //{
        //    try
        //    {
        //        string taskdetails = "";
        //        if (dto.ISMSLECOM_Id > 0)
        //        {
        //            var result = _vmsconte.ISM_Sales_Lead_Comments_DMO_con.Single(a => a.ISMSLECOM_Id == dto.ISMSLECOM_Id && a.MI_Id == dto.MI_Id);
        //            result.ISMSLE_Id = dto.ISMSLE_Id;
        //            result.ISMSLECOM_Comments = dto.ISMSLECOM_Comments;
        //            result.ISMSLECOM_UpdatedDate = DateTime.Now;
        //            result.ISMSLECOM_UpdatedBy = dto.User_Id;
        //            result.ISMSLE_FilePath = dto.ISMSLE_FilePath;
        //            _vmsconte.Update(result);
        //            var vv = _vmsconte.SaveChanges();
        //            if (vv > 0)
        //            {
        //                dto.returndata = "Update";
        //            }
        //            else
        //            {
        //                dto.returndata = "Error";
        //            }



        //        }
        //        else
        //        {
        //            ISM_Sales_Lead_Comments_DMO cmc = new ISM_Sales_Lead_Comments_DMO();
        //            cmc.ISMSLE_Id = dto.ISMSLE_Id;
        //            cmc.ISMSLECOM_Comments = dto.ISMSLECOM_Comments;
        //            cmc.MI_Id = dto.MI_Id;
        //            cmc.ISMSLECOM_ActiveFlag = true;
        //            cmc.ISMSLECOM_CreatedDate = DateTime.Now;
        //            cmc.ISMSLECOM_CreatedBy = dto.User_Id;
        //            cmc.ISMSLE_FilePath = dto.ISMSLE_FilePath;
        //            _vmsconte.Add(cmc);
        //            var vv = _vmsconte.SaveChanges();
        //            var lead = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == dto.ISMSLE_Id && a.MI_Id == dto.MI_Id).ToList();
        //            dto.leadname = lead[0].ISMSLE_LeadName;
        //            taskdetails = getleadReport(dto);
        //            var Template = "LeadComments";
        //            dto.message = sendmail(dto.MI_Id, lead[0].ISMSLE_EmailId,   taskdetails, Template, dto.User_Id, dto.ISMSLE_Id);

        //            if (vv > 0)
        //            {
        //                dto.returndata = "Add";
        //            }
        //            else
        //            {
        //                dto.returndata = "Error";
        //            }

        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);

        //    }

        //    return dto;
        //}


        public ISM_Client_Project_DTO SaveEdit_CMT(ISM_Client_Project_DTO dto)
        {
            try
            {
                string taskdetails = "";
                if (dto.ISMSLECOM_Id > 0)
                {

                    var result = _vmsconte.ISM_Sales_Lead_Comments_DMO_con.Single(a => a.ISMSLECOM_Id == dto.ISMSLECOM_Id && a.MI_Id == dto.MI_Id);
                    result.ISMSLE_Id = dto.ISMSLE_Id;
                    result.ISMSLECOM_Comments = dto.ISMSLECOM_Comments;
                    result.ISMSLECOM_UpdatedDate = DateTime.Now;
                    result.ISMSLECOM_UpdatedBy = dto.User_Id;
                    result.ISMSLE_FilePath = dto.ISMSLE_FilePath;
                    //added by roopa//
                    result.ISMSLECOM_RemainderDate = dto.ISMSLECOM_RemainderDate;
                    result.ISMSLECOM_RemainderTime = dto.ISMSLECOM_RemainderTime;
                    result.ISMSLECOM_RemainderHrs = dto.ISMSLECOM_RemainderHrs;

                    result.ISMSLECOM_SendProposalFlg = dto.ISMSLECOM_SendProposalFlg;
                    result.ISMSLECOM_SendQuotationFlg = dto.ISMSLECOM_SendQuotationFlg.Value;
                    result.ISMSLECOM_NextReminderDate = dto.ISMSLECOM_NextReminderDate;
                    //
                    _vmsconte.Update(result);
                    var removefile = _vmsconte.ISM_Sales_Lead_CommentsFilesDMO.Where(a => a.ISMSLECOM_Id == dto.ISMSLECOM_Id).ToList();
                    if (removefile.Count > 0)
                    {
                        foreach (var d in removefile)
                        {
                            _vmsconte.Remove(d);
                        }
                    }
                    if (dto.filelist != null)
                    {

                        for (int i = 0; i < dto.filelist.Length; i++)
                        {
                            ISM_Sales_Lead_CommentsFilesDMO dobj = new ISM_Sales_Lead_CommentsFilesDMO();
                            dobj.ISMSLECOM_Id = dto.ISMSLECOM_Id;
                            dobj.ISMSLECF_FileName = dto.filelist[i].ISMSLECF_FileName;
                            dobj.ISMSLECF_FileDesc = dto.filelist[i].ISMSLECF_FileDesc;
                            dobj.ISMSLECF_FilePath = dto.filelist[i].ISMSLECF_FilePath;
                            dobj.ISMSLECF_CreatedDate = DateTime.Now;
                            dobj.ISMSLECF_UpdatedDate = DateTime.Now;
                            dobj.ISMSLECF_CreatedBy = dto.User_Id;
                            dobj.ISMSLECF_UpdatedBy = dto.User_Id;
                            _vmsconte.Add(dobj);
                        }
                    }
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                        var lead = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == dto.ISMSLE_Id && a.MI_Id == dto.MI_Id).ToList();
                        dto.leadname = lead[0].ISMSLE_LeadName;
                        taskdetails = getleadReport(dto);
                        var Template = "LeadComments";
                        dto.message = sendmail(dto.MI_Id, lead[0].ISMSLE_EmailId, taskdetails, Template, dto.User_Id, dto.ISMSLE_Id);

                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Sales_Lead_Comments_DMO cmc = new ISM_Sales_Lead_Comments_DMO();
                    cmc.ISMSLE_Id = dto.ISMSLE_Id;
                    cmc.ISMSLECOM_Comments = dto.ISMSLECOM_Comments;
                    cmc.MI_Id = dto.MI_Id;
                    cmc.ISMSLECOM_ActiveFlag = true;
                    cmc.ISMSLECOM_CreatedDate = DateTime.Now;
                    cmc.ISMSLECOM_CreatedBy = dto.User_Id;
                    cmc.ISMSLE_FilePath = dto.ISMSLE_FilePath;
                    //added by roopa//
                    cmc.ISMSLECOM_RemainderDate = dto.ISMSLECOM_RemainderDate;
                    cmc.ISMSLECOM_RemainderTime = dto.ISMSLECOM_RemainderTime;
                    cmc.ISMSLECOM_RemainderHrs = dto.ISMSLECOM_RemainderHrs;
                    cmc.ISMSLECOM_SendProposalFlg = dto.ISMSLECOM_SendProposalFlg;
                    cmc.ISMSLECOM_SendQuotationFlg = dto.ISMSLECOM_SendQuotationFlg.Value;
                    cmc.ISMSLECOM_NextReminderDate = dto.ISMSLECOM_NextReminderDate;
                    //
                    _vmsconte.Add(cmc);
                    if (dto.filelist != null)
                    {
                        for (int i = 0; i < dto.filelist.Length; i++)
                        {
                            ISM_Sales_Lead_CommentsFilesDMO dobj = new ISM_Sales_Lead_CommentsFilesDMO();
                            dobj.ISMSLECOM_Id = cmc.ISMSLECOM_Id;
                            dobj.ISMSLECF_FileName = dto.filelist[i].ISMSLECF_FileName;
                            dobj.ISMSLECF_FileDesc = dto.filelist[i].ISMSLECF_FileDesc;
                            dobj.ISMSLECF_FilePath = dto.filelist[i].ISMSLECF_FilePath;
                            dobj.ISMSLECF_CreatedDate = DateTime.Now;
                            dobj.ISMSLECF_UpdatedDate = DateTime.Now;
                            dobj.ISMSLECF_CreatedBy = dto.User_Id;
                            dobj.ISMSLECF_UpdatedBy = dto.User_Id;
                            _vmsconte.Add(dobj);
                        }
                    }
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                        var lead = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == dto.ISMSLE_Id && a.MI_Id == dto.MI_Id).ToList();
                        dto.leadname = lead[0].ISMSLE_LeadName;
                        taskdetails = getleadReport(dto);
                        var Template = "LeadComments";
                        dto.message = sendmail(dto.MI_Id, lead[0].ISMSLE_EmailId, taskdetails, Template, dto.User_Id, dto.ISMSLE_Id);


                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO deactivate_CMT(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Lead_Comments_DMO_con.Single(a => a.ISMSLECOM_Id == dto.ISMSLECOM_Id && a.MI_Id == dto.MI_Id);
                if (dto.ISMSLECOM_ActiveFlag == true)
                {
                    result.ISMSLECOM_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMSLECOM_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO getcmtrpt_data(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.StatusListOnload = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.ISMSMST_ActiveFlag == true).ToArray();

                List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
                List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_DD";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = "0"
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


                //using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "ISm_Sales_Lead_DD";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //    {
                //        Value = dto.MI_Id
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
                //                    dataReader.GetName(iFiled),
                //                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        dto.leadlist = retObject.ToArray();
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}

                dto.logo = _vmsconte.Institution.Where(p => p.MI_Id == dto.MI_Id && p.MI_ActiveFlag == 1).Select(R => R.MI_Logo).Single();

                //Departmentlist
                Departmentlist = _vmsconte.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                dto.departmentdropdown = Departmentlist.ToArray();

                dto.companies = _vmsconte.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();

                //Designationlist
                Designationlist = _vmsconte.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                dto.designationdropdown = Designationlist.ToArray();

                dto.sourcelist = (from a in _vmsconte.ISM_Sales_Master_Source_DMO
                                  from b in _vmsconte.Institution
                                  where (a.MI_Id == b.MI_Id)
                                  select new ISM_Client_Project_DTO
                                  {
                                      ISMSMSO_SourceName = a.ISMSMSO_SourceName,
                                      MI_Name = b.MI_Name,
                                      ISMSMSO_Id = a.ISMSMSO_Id
                                  }).Distinct().ToArray();

                dto.statelist = _vmsconte.IVRM_Master_State.Where(t => t.IVRMMC_Id == 101).ToArray();


                //sales portfolio

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_Sales_Portfolio_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "CLOSURESTATUS"
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
                        dto.Closurestatus = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_Sales_Portfolio_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "BDE"
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
                        dto.BDElist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_Sales_Portfolio_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "CA"
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
                        dto.ClientAcquisitionlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_Sales_Portfolio_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "TELECALLER"
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
                        dto.Telecallerlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_Sales_Portfolio_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "REGION"
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
                        dto.Regionlist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_Sales_Portfolio_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "REVENUE"
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
                        dto.Revenue = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //sales portfolio


                //channel partner report onload

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_ChannelPartner_Report_Onload";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                        Value = "REGION"
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
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
                        dto.ChannelParter_lead_Reportonloadregion = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_ChannelPartner_Report_Onload";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                        Value = "CP"
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
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
                        dto.ChannelParter_lead_ReportonloadCP = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_ChannelPartner_Report_Onload";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                        Value = "status"
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
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
                        dto.ChannelParter_lead_Reportonloadstatus = retObject.ToArray();
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

        public ISM_Client_Project_DTO searchleadvalue(ISM_Client_Project_DTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_DD";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@searchleadvalue", SqlDbType.VarChar)
                    {
                        Value = dto.searchleadvalue
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
                        dto.leadlist = retObject.ToArray();
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

        public async Task<ISM_Client_Project_DTO> report_cmtrpt(ISM_Client_Project_DTO dto)
        {
            try
            {
                //string FromDate = dto.fromdate.Value.ToString("yyyy-MM-dd");
                //string ToDate = dto.todate.Value.ToString("yyyy-MM-dd");

                //startdate = dto.INTB_StartDate.ToString("yyyy-MM-dd");
                //enddate = dto.INTB_EndDate.ToString("yyyy-MM-dd");

                var mi_ids = "0";
                if (dto.MI_Id_IdList.Length > 0)
                {
                    foreach (var item in dto.MI_Id_IdList)
                    {
                        mi_ids = mi_ids + "," + item;
                    }

                }

                string depart = "0";
                if (dto.hrmD_IdList.Length > 0)
                {
                    foreach (var item in dto.hrmD_IdList)
                    {
                        depart = depart + "," + item;
                    }

                }

                string desg = "0";
                if (dto.hrmdes_IdList.Length > 0)
                {
                    foreach (var item in dto.hrmdes_IdList)
                    {
                        desg = desg + "," + item;
                    }

                }


                var hrme_ids = "0";
                if (dto.hrmE_IdList.Length > 0)
                {
                    foreach (var item in dto.hrmE_IdList)
                    {
                        hrme_ids = hrme_ids + "," + item;
                    }

                }


                if (dto.Status == "LeftEmployees")
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                       
                        cmd.CommandText = "ISm_Sales_Lead_Report_Count_New_LeftEmployees";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            // Value = dto.MI_Id
                            Value = mi_ids
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                        {
                            Value = depart
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                        {
                            Value = desg
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = hrme_ids
                        });

                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                        {
                            Value = dto.fromdate.ToString()
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                        {
                            Value = dto.todate.ToString()

                        });
                        cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
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
                            dto.leadcommentsreport = retObject.ToArray();
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
                        // cmd.CommandText = "ISm_Sales_Lead_Comments_ReportsDeptDesgEmployee";
                        cmd.CommandText = "ISm_Sales_Lead_Report_Count_New";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.CommandTimeout = 90000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            // Value = dto.MI_Id
                            Value = mi_ids
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                        {
                            Value = depart
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                        {
                            Value = desg
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = hrme_ids
                        });

                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                        {
                            Value = dto.fromdate.ToString()
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                        {
                            Value = dto.todate.ToString()

                        });
                        cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
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
                            dto.leadcommentsreport = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }



               
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    // cmd.CommandText = "ISm_Sales_Lead_Comments_ReportsDeptDesgEmployee";
                    cmd.CommandText = "ISm_Sales_Lead_Report_Count_Total";
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
                        dto.components_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "ISm_Sales_Lead_Comments_Reports";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //    {
                //        Value = dto.MI_Id
                //    });

                //     cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                //    {
                //        Value = leadid
                //     });

                //     cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                //    {
                //        Value = FromDate
                //     });
                //    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                //    {
                //        Value = ToDate
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
                //                    dataReader.GetName(iFiled),
                //                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        dto.leadcommentsreport = retObject.ToArray();
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        public ISM_Client_Project_DTO Sales_Lead_Remark(ISM_Client_Project_DTO dto)
        {
            try
            {
                //string FromDate = dto.fromdate.Value.ToString("yyyy-MM-dd");
                //string ToDate = dto.todate.Value.ToString("yyyy-MM-dd");

                //startdate = dto.INTB_StartDate.ToString("yyyy-MM-dd");
                //enddate = dto.INTB_EndDate.ToString("yyyy-MM-dd");

                //var mi_ids = "0";
                //if (dto.MI_Id_IdList.Length > 0)
                //{
                //    foreach (var item in dto.MI_Id_IdList)
                //    {
                //        mi_ids = mi_ids + "," + item;
                //    }

                //}

                //string depart = "0";
                //if (dto.hrmD_IdList.Length > 0)
                //{
                //    foreach (var item in dto.hrmD_IdList)
                //    {
                //        depart = depart + "," + item;
                //    }

                //}

                //string desg = "0";
                //if (dto.hrmdes_IdList.Length > 0)
                //{
                //    foreach (var item in dto.hrmdes_IdList)
                //    {
                //        desg = desg + "," + item;
                //    }

                //}


                var hrme_ids = "0";
                if (dto.hrmE_IdList.Length > 0 && dto.hrmE_IdList != null)
                {
                    foreach (var item in dto.hrmE_IdList)
                    {
                        hrme_ids = hrme_ids + "," + item;
                    }

                }




                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                 
                    cmd.CommandText = "Sales_Extract_Report";
                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add(new SqlParameter("@ISMSMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = hrme_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

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
                        dto.Sales_Lead_RemarkReport = retObject.ToArray();
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





        public ISM_Client_Project_DTO report_cmtrptDetails(ISM_Client_Project_DTO dto)
        {
            try
            {
               

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    // cmd.CommandText = "ISm_Sales_Lead_Comments_ReportsDeptDesgEmployee";
                    cmd.CommandText = "ISm_Sales_Lead_Report_Count_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.MI_Id
                    //});
                   
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = dto.FlagType
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = dto.Status
                    });
                    cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.VarChar)
                    {
                        Value = dto.ISMSMPR_ProductName
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
                        dto.leadcommentsreportDeatils = retObject.ToArray();
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

        public ISM_Client_Project_DTO Total_commentreport(ISM_Client_Project_DTO dto)
        {
            try
            {


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    // cmd.CommandText = "ISm_Sales_Lead_Comments_ReportsDeptDesgEmployee";
                    cmd.CommandText = "ISm_Sales_Lead_Report_Count_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.MI_Id
                    //});

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
                    {
                        Value = "GivenBY"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = dto.Status
                    });
                    cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.VarChar)
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
                        dto.Totalcommentreport = retObject.ToArray();
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



        public ISM_Client_Project_DTO report_database(ISM_Client_Project_DTO dto)
        {
            try
            {

                string source = "";
                if (dto.source_IdList.Length > 0)
                {
                    foreach (var item in dto.source_IdList)
                    {
                        source = source + "," + item;
                    }

                }

                string state = "";
                if (dto.state_IdList.Length > 0)
                {
                    foreach (var item in dto.state_IdList)
                    {
                        state = state + "," + item;
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISm_Sales_Lead_Database_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.MI_Id
                    //});

                    cmd.Parameters.Add(new SqlParameter("@ISMSMSO_Id", SqlDbType.VarChar)
                    {
                        Value = source
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id", SqlDbType.VarChar)
                    {
                        Value = state
                    });


                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
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
                        dto.leaddatabasereport = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }






                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {

                   cmd.CommandText = "ISm_Sales_RawLeads_Report_NewData";

                  //  cmd.CommandText = "ISm_Sales_RawLeads_New";

                    //cmd.CommandText = "ISm_Sales_RawLeads_New_20250627";
                    

                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.MI_Id
                    //});

                    cmd.Parameters.Add(new SqlParameter("@ISMSMSO_Id", SqlDbType.VarChar)
                    {
                        Value = source
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id", SqlDbType.VarChar)
                    {
                        Value = state
                    });


                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    //cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
                    //{
                    //    Value = dto.flag
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
                        dto.leaddatabasereportdetails = retObject.ToArray();
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


        public ISM_Client_Project_DTO report_Quotationrequest(ISM_Client_Project_DTO dto)
        {
            try
            {

                string source = "";
                if (dto.source_IdList.Length > 0)
                {
                    foreach (var item in dto.source_IdList)
                    {
                        source = source + "," + item;
                    }

                }

                string state = "";
                if (dto.state_IdList.Length > 0)
                {
                    foreach (var item in dto.state_IdList)
                    {
                        state = state + "," + item;
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISm_Sales_Lead_Quotationrequest_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSMSO_Id", SqlDbType.VarChar)
                    {
                        Value = source
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id", SqlDbType.VarChar)
                    {
                        Value = state
                    });


                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
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
                        dto.leadquotationreport = retObject.ToArray();
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

        public ISM_Client_Project_DTO report_database_details(ISM_Client_Project_DTO dto)
        {
            try
            {

                //string source = "";
                //if (dto.source_IdList.Length > 0)
                //{
                //    foreach (var item in dto.source_IdList)
                //    {
                //        source = source + "," + item;
                //    }

                //}

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISm_Sales_Lead_Database_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = dto.Status
                    });

                    //cmd.Parameters.Add(new SqlParameter("@ISMSMSO_Id", SqlDbType.VarChar)
                    //{
                    //    Value = source
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
                        dto.leaddatabasereportdetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                string source = "";
                if (dto.source_IdList != null && dto.source_IdList.Length > 0)
                {
                    foreach (var item in dto.source_IdList)
                    {
                        source = source + "," + item;
                    }

                }

                string state = "";
                if (dto.source_IdList != null && dto.state_IdList.Length > 0)
                {
                    foreach (var item in dto.state_IdList)
                    {
                        state = state + "," + item;
                    }

                }


                string startdate = "";
                string enddate = "";
                if (dto.fromdate != null && dto.todate != null)
                {
                    startdate = dto.fromdate.Value.ToString("yyyy-MM-dd");
                    enddate = dto.todate.Value.ToString("yyyy-MM-dd");
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "ISm_Sales_RawLeads_Report_Details_New";


                    cmd.CommandText = "ISm_Sales_RawLeads_New_20250627_Details";





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
                        Value = dto.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("Flag2",
                   SqlDbType.VarChar)
                    {
                        Value = dto.searchleadvalue
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMSMSO_Id",
                   SqlDbType.VarChar)
                    {
                        Value = source
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id",
                   SqlDbType.VarChar)
                    {
                        Value = state
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
                        dto.leadQuotationrequestreportdetails = ret.ToArray();
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

        public ISM_Client_Project_DTO report_Quotationrequest_details(ISM_Client_Project_DTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISm_Sales_Lead_Quotationrequest_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = dto.Status
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
                        dto.leadQuotationrequestreportdetails = retObject.ToArray();
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

        public ISM_Client_Project_DTO report_cmtrptDetailsverify(ISM_Client_Project_DTO dto)
        {
            try
            {
                string depart = "0";
                if (dto.hrmD_IdList.Length > 0)
                {
                    foreach (var item in dto.hrmD_IdList)
                    {
                        depart = depart + "," + item;
                    }

                }

                string desg = "0";
                if (dto.hrmdes_IdList.Length > 0)
                {
                    foreach (var item in dto.hrmdes_IdList)
                    {
                        desg = desg + "," + item;
                    }

                }


                var hrme_ids = "0";
                if (dto.hrmE_IdList.Length > 0)
                {
                    foreach (var item in dto.hrmE_IdList)
                    {
                        hrme_ids = hrme_ids + "," + item;
                    }

                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    // cmd.CommandText = "ISm_Sales_Lead_Comments_ReportsDeptDesgEmployee";
                    cmd.CommandText = "ISm_Sales_Lead_Report_Verify_Count";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                    {
                        Value = depart
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                    {
                        Value = desg
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = hrme_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    //cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
                    //{
                    //    Value = dto.flag
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
                        dto.leadcommentsreport = retObject.ToArray();
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



        public ISM_Client_Project_DTO report_cmtrptDetailsverifyDetails(ISM_Client_Project_DTO dto)
        {
            try
            {


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    // cmd.CommandText = "ISm_Sales_Lead_Comments_ReportsDeptDesgEmployee";
                    cmd.CommandText = "ISm_Sales_Lead_Report_Verify_Count_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = dto.fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = dto.todate.ToString()

                    });
                    //cmd.Parameters.Add(new SqlParameter("@emp_flg", SqlDbType.VarChar)
                    //{
                    //    Value = dto.flag
                    //});
                    //cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    //{
                    //    Value = dto.FlagType
                    //});
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = dto.Status
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
                        dto.leadcommentsreportDeatils = retObject.ToArray();
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

        public string getleadReport(ISM_Client_Project_DTO data)
        {
            try
            {
                int rownumber = 0;
                string str = "";
                string str1 = "";
                string str2 = "";


                List<ISM_Client_Project_DTO> dtonewtemp = new List<ISM_Client_Project_DTO>();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SalesLead_Comments_Report_Data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                     SqlDbType.VarChar)
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
                                dtonewtemp.Add(new ISM_Client_Project_DTO
                                {
                                    employeename = Convert.ToString(dataReader["employeename"]),
                                    ISMSLECOM_Comments = Convert.ToString(dataReader["ISMSLECOM_Comments"]),
                                    ISMSLECOM_Date = Convert.ToString(dataReader["ISMSLECOM_CreatedDate"]),
                                    ISMSLECOM_Id = Convert.ToInt64(dataReader["ISMSLECOM_Id"])
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

                    var stdate = Convert.ToDateTime(data.ISMSLECOM_Date).ToString("dd/MM/yyyy");

                    str += "<p> <span style=" + "'font-weight:bold'" + "><span style=" + "'float: center;padding-left:10%;'" + ">" + "</span> </span></p> <table width='130%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Comments By.</th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Comments</th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;' width='20%'" + " >  Date </th>" +

                             "</tr>";
                    for (int j = 0; j < dtonewtemp.Count; j++)
                    {
                        rownumber += 1;
                        str1 += "<tr> " +
                            "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + dtonewtemp[j].employeename + "</td>" +
                            "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + dtonewtemp[j].ISMSLECOM_Comments + "</td>" +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].ISMSLECOM_Date + "</td> " +



                             "</tr>";
                    }
                    str2 = "</table>";

                }
                data.message = str + str1 + str2;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.message;
        }

        public ISM_Client_Project_DTO get_depart(ISM_Client_Project_DTO data)
        {
            try
            {
                var mi_ids = "0";
                if (data.MI_Id_IdList.Length > 0)
                {
                    foreach (var item in data.MI_Id_IdList)
                    {
                        mi_ids = mi_ids + "," + item;
                    }

                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Department";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = mi_ids
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
                        data.departmentdropdown = retObject.ToArray();
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

        public ISM_Client_Project_DTO get_desig(ISM_Client_Project_DTO data)
        {
            try
            {

                var hrmD_ids = "0";
                if (data.hrmD_IdList.Length > 0)
                {
                    foreach (var item in data.hrmD_IdList)
                    {
                        hrmD_ids = hrmD_ids + "," + item;
                    }

                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Department_Designation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                    {
                        Value = hrmD_ids
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
                        data.designationdropdown = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                


                //data.designationdropdown = (from a in _vmsconte.HRGroupDeptDessgDMO
                //                            from b in _vmsconte.HR_Master_Designation
                //                            where (a.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                //                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ISM_Client_Project_DTO get_employee(ISM_Client_Project_DTO data)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_DD";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.hrmE_IdList
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


                var mi_ids = "0";
                if (data.MI_Id_IdList.Length > 0)
                {
                    foreach (var item in data.MI_Id_IdList)
                    {
                        mi_ids = mi_ids + "," + item;
                    }

                }
             

                var hrmdes_ids = "0";
                if (data.hrmdes_IdList.Length > 0)
                {
                    foreach (var item in data.hrmdes_IdList)
                    {
                        hrmdes_ids = hrmdes_ids + "," + item;
                    }

                }
                var hrmD_ids = "0";
                if (data.hrmD_IdList.Length > 0)
                {
                    foreach (var item in data.hrmD_IdList)
                    {
                        hrmD_ids = hrmD_ids + "," + item;
                    }

                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Dept_Desgwise_EmpDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@hrmdes_IdList", SqlDbType.VarChar)
                    {
                        Value = hrmdes_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@hrmD_IdList", SqlDbType.VarChar)
                    {
                        Value = hrmD_ids
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
                        data.employeedropdown = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                

                //data.employeedropdown = (from a in _vmsconte.HRGroupDeptDessgDMO
                //                         from b in _vmsconte.HR_Master_Designation
                //                         from c in _vmsconte.Hr_Master_Employee_con
                //                         where (c.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRMDES_Id == b.HRMDES_Id && c.HRMD_Id == a.HRMD_Id
                //                         && data.hrmdes_IdList.Contains(a.HRMDES_Id)
                //                         && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                //                         select c).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ISM_Client_Project_DTO getleads(ISM_Client_Project_DTO data)
        {
            try
            {
                var hrme_ids = "0";
                if (data.hrmE_IdList.Length > 0)
                {
                    foreach (var item in data.hrmE_IdList)
                    {
                        hrme_ids = hrme_ids + "," + item;
                    }

                }



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_DD";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = hrme_ids
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



        public string sendmail(long MI_Id, string Email, string taskdetails, string Template, long userid, long ISMSLE_Id)
        {
            try
            {
                long HRMEID = 0;
                string mailcc = "";
                string mailbcc = "";


                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _vmsconte.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _vmsconte.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _vmsconte.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LEAD_COMMENTS_PARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
                        SqlDbType.BigInt)
                    {
                        Value = userid

                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                       SqlDbType.BigInt)
                    {
                        Value = ISMSLE_Id
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

                val.Add("[MSG]", taskdetails);

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

                Mailcontent = Mailmsg;
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
                    string emailccdetails = "";
                    string emailbccdetails = "";

                    if (template[0].ISES_MailCCId != null || template[0].ISES_MailCCId != "")
                    {
                        emailccdetails = template[0].ISES_MailCCId;
                        //emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                    }
                    else
                    {
                        emailccdetails = "";
                        emailbccdetails = "";
                    }

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
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


                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddCc(c);
                                }

                            }
                        }

                        if (emailbccdetails != null && emailbccdetails != "")
                        {
                            string[] bccmaildetails = emailbccdetails.Split(',');

                            foreach (var bc in bccmaildetails)
                            {
                                if (bc != Email)
                                {
                                    message.AddBcc(bc);
                                }
                            }
                        }


                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();




                        //using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                        //{
                        //    var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                        //    var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        //    var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        //    if (modulename.Count == 0)
                        //    {
                        //        modulename[0] = "Issue Manager";
                        //    }

                        //    cmd.CommandText = "IVRM_Email_Outgoing";
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Add(new SqlParameter("@EmailId",
                        //        SqlDbType.NVarChar)
                        //    {
                        //        Value = Email
                        //    });
                        //    cmd.Parameters.Add(new SqlParameter("@Message",
                        //       SqlDbType.NVarChar)
                        //    {
                        //        Value = template
                        //    });
                        //    cmd.Parameters.Add(new SqlParameter("@module",
                        //    SqlDbType.VarChar)
                        //    {
                        //        Value = modulename[0]
                        //    });
                        //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        //   SqlDbType.BigInt)
                        //    {
                        //        Value = MI_Id
                        //    });

                        //    if (cmd.Connection.State != ConnectionState.Open)
                        //        cmd.Connection.Open();

                        //    try
                        //    {
                        //        using (var dataReader = cmd.ExecuteReader())
                        //        {
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        return ex.Message;
                        //    }
                        //}
                    }
                    else
                    {

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }
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
                                emailMessage.From = new MailAddress(SendingEmail, institutionName[0].MI_Name);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                if (Attechement.Equals("1"))
                                {
                                    var img = _vmsconte.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
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



                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            emailMessage.CC.Add(c);
                                        }
                                    }
                                }

                                if (emailbccdetails != null && emailbccdetails != "")
                                {
                                    string[] bccmaildetails = emailbccdetails.Split(',');

                                    foreach (var bc in bccmaildetails)
                                    {
                                        if (bc != Email)
                                        {
                                            emailMessage.Bcc.Add(bc);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);



                            }
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

        public ISM_Client_Project_DTO Transferload(ISM_Client_Project_DTO dto)
        {
            try
            {


                dto.yearlist = _vmsconte.IVRM_Master_FinancialYearDMO.OrderByDescending(s => s.IMFY_FinancialYear).ToArray();
                dto.logo = _vmsconte.Institution.Where(p => p.MI_Id == dto.MI_Id && p.MI_ActiveFlag == 1).Select(R => R.MI_Logo).Single();
                dto.leadlist = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSLE_ActiveFlag == true).ToArray();



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Incharge_Lead";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        dto.leadIncharge = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Employees";
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
                        dto.leadInchargetList = retObject.ToArray();
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
        public ISM_Client_Project_DTO gettransferLeads(ISM_Client_Project_DTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Financial_Leadlist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
                    {
                        Value = dto.IMFY_Id
                    });

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
                        dto.leadlist = retObject.ToArray();
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
        public ISM_Client_Project_DTO getTransferDetails(ISM_Client_Project_DTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Incharge_Lead";
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
                        dto.leadIncharge = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Employees";
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
                        dto.leadInchargetList = retObject.ToArray();
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

        public ISM_Client_Project_DTO LeadTransferSave(ISM_Client_Project_DTO dto)
        {
            try
            {
                if(dto.leaddatalist !=null  && dto.leaddatalist.Length >0)
                {
                    foreach(var d  in dto.leaddatalist)
                    {
                        //var duplicate = (from a in _vmsconte.ISM_Sales_Lead_InchargesDMO
                        //                 from b in _vmsconte.HR_Master_Employee_DMO
                        //                 where (a.HRME_Id == b.HRME_Id && b.HRME_Id== dto.HRME_Id && a.ISMSLE_Id==d.ISMSLE_Id && b.HRME_ActiveFlag==true && b.HRME_LeftFlag==false)
                        //                select new ISM_Client_Project_DTO
                        //                {
                        //                    HRME_Id = Convert.ToInt64(b.HRME_Id)
                        //                }

                        //        ).ToList();
                        //if(duplicate.Count >0)
                        //{
                        //    dto.message = "Transfered Successfully ! Duplicate";

                        //}
                        //else
                        //{


                        //}
                        var duplicate = _vmsconte.ISM_Sales_Lead_InchargesDMO.Where(R => R.ISMSLE_Id == d.ISMSLE_Id && R.ISMSLEPRI_ActiveFlag == true).ToList();
                        if(duplicate.Count > 0)
                        {
                            foreach(var H  in duplicate)
                            {
                                H.ISMSLEPRI_ActiveFlag = false;
                                H.ISMSLEPRI_UpdatedBy= dto.User_Id;
                                H.ISMSLEPRI_UpdatedDate = DateTime.Now;
                                _vmsconte.Update(H);
                            }
                        }
                       
                        ISM_Sales_Lead_InchargesDMO ld = new ISM_Sales_Lead_InchargesDMO();
                        ld.ISMSLE_Id = d.ISMSLE_Id;
                        ld.HRME_Id = dto.HRME_Id;
                        ld.ISMSLEPRI_ActiveFlag = true;
                        ld.ISMSLEPRI_CreatedDate = DateTime.Now;
                        ld.ISMSLEPRI_CreatedBy = dto.User_Id;
                        ld.ISMSLEPRI_UpdatedDate = DateTime.Now;
                        ld.ISMSLEPRI_UpdatedBy = dto.User_Id;
                        _vmsconte.Add(ld);
                    }

                    var cnt = _vmsconte.SaveChanges();
                    if (cnt > 0)
                    {

                        dto.message = "Transfered Successfully!!";
                    }
                   
                }

                //if (dto.ISMSLE_Id > 0)
                //{
                //    if (dto.HRME_Id > 0)
                //    {
                //        var inchargearray = _vmsconte.ISM_Sales_Lead_InchargesDMO.Where(s => s.ISMSLE_Id == dto.ISMSLE_Id).ToArray();
                //        if (inchargearray.Length > 0)
                //        {
                //            foreach (var item in inchargearray)
                //            {
                //                var result1 = _vmsconte.ISM_Sales_Lead_InchargesDMO.Single(s => s.ISMSLE_Id == dto.ISMSLE_Id && s.ISMSLEPRI_Id == item.ISMSLEPRI_Id);
                //                result1.ISMSLEPRI_ActiveFlag = false;
                //                result1.ISMSLEPRI_UpdatedDate = DateTime.Now;
                //                result1.ISMSLEPRI_UpdatedBy = dto.User_Id;
                //                _vmsconte.Update(result1);
                //            }
                //            var cntt = _vmsconte.SaveChanges();
                //        }



                //        ISM_Sales_Lead_InchargesDMO ld = new ISM_Sales_Lead_InchargesDMO();
                //        ld.ISMSLE_Id = dto.ISMSLE_Id;
                //        ld.HRME_Id = dto.HRME_Id;
                //        ld.ISMSLEPRI_ActiveFlag = true;
                //        ld.ISMSLEPRI_CreatedDate = DateTime.Now;
                //        ld.ISMSLEPRI_CreatedBy = dto.User_Id;
                //        ld.ISMSLEPRI_UpdatedDate = DateTime.Now;
                //        ld.ISMSLEPRI_UpdatedBy = dto.User_Id;
                //        _vmsconte.Add(ld);


                //        var cnt = _vmsconte.SaveChanges();
                //        if (cnt > 0)
                //        {

                //            dto.message = "Transfered Successfully!!";
                //        }

                //    }

                //}
                //else
                //{
                //    dto.message = "Something Went Wrong!!";
                //}


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        //Nagaraj
        public ISM_Client_Project_DTO StatusUpdateOnLoad (ISM_Client_Project_DTO dto)
        {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Lead_Comments_StatusUpdate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value =dto.ISMSLE_Id
                    });
                  cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                    {
                     Value = dto.flag
                   });

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
                        dto.StatusUpdateOnLoad = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            return dto;
        }
        public ISM_Client_Project_DTO StatusDropDownData(ISM_Client_Project_DTO dto)
        {
            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Sales_Lead_Comments_StatusUpdate";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                {
                    Value = dto.User_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                {
                    Value = dto.ISMSLE_Id
                });
                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                {
                    Value = dto.flag
                });

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
                    dto.StatusDropDownData = retObject.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return dto;
        }

        public ISM_Client_Project_DTO StatusUpdate(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.selectedDataArray.Length > 0)
                {
                    foreach (var aa in dto.selectedDataArray)
                    {
                        var result = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Single(a => a.ISMSLEDMPR_Id == aa.ISMSLEDMPR_Id);
                        result.ISMSMST_Id = aa.ISMSMST_Id;
                        result.UpdatedDate = DateTime.Now;
                        result.ISMSLEDMPR_UpdatedBy = dto.User_Id;
                        _vmsconte.Update(result);
                        var vv = _vmsconte.SaveChanges();
                        if (vv > 0)
                        {
                            dto.returndata = "update";
                            if (aa.ISMSLEDMPR_SendQuotationFlg == true)
                            {
                                result.ISMSLEDMPR_SendQuotationFlg = true;
                                result.UpdatedDate = DateTime.Now;
                                result.ISMSLEDMPR_UpdatedBy = dto.User_Id;
                                _vmsconte.Update(result);
                                _vmsconte.SaveChanges();

                               
                                var emailid = "mktgcoord@vapstech.com";

                                if (emailid != null && emailid != "")
                                {
                                    sendmails(dto.MI_Id, emailid, "ConfirmedClient", aa.ISMSLEDM_Id, dto.User_Id);
                                }
                            }
                        }
                        else
                        {
                            dto.returndata = "Not Update";
                        }

                    }
                }
                else
                {
                    dto.returndata = "Error";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returndata = "Error";
            }
            return dto;
        }


        public ISM_Client_Project_DTO getClientData(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.ClientDataOnload = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_ActiveFlag == true && a.ISMSLE_VerifiedFlg == true).ToArray();
                dto.ProductListOnload= _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.ISMSMPR_ActiveFlag == true).ToArray();
                dto.StatusListOnload= _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.ISMSMST_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returndata = "Error";
            }
            return dto;
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


        //getClientReport
        public ISM_Client_Project_DTO getClientReport(ISM_Client_Project_DTO dto)
        {
            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Lead_Sales_Details";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                {
                    Value = dto.ISMSLE_IdNew
                });
                cmd.Parameters.Add(new SqlParameter("@ISMSMST_Id", SqlDbType.VarChar)
                {
                    Value = dto.ISMSMST_IdNew
                });
                cmd.Parameters.Add(new SqlParameter("@ISMSMPR_Id", SqlDbType.VarChar)
                {
                    Value = dto.ISMSMPR_IdNew
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
                    dto.ClientReport = retObject.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return dto;
        }



        public ISM_Client_Project_DTO SalesPortfolioReport(ISM_Client_Project_DTO dto)
        {

            string bdes = "0";
            if (dto.BDElists.Length > 0 && dto.BDElists != null)
            {
                foreach (var item in dto.BDElists)
                {
                    bdes = bdes + "," + item;
                }

            }

            string Revenues = "0";
            if ( dto.Revenues != null)
            {
                foreach (var item in dto.Revenues)
                {
                    Revenues = Revenues + "," + item;
                }

            }

            string Region = "0";
            if (dto.RegionlistLists.Length > 0 && dto.RegionlistLists != null)
            {
                foreach (var item in dto.RegionlistLists)
                {
                    Region = Region + "," + item;
                }

            }

            string cas = "0";
            if (dto.ClientAcquisitionlists.Length > 0 && dto.ClientAcquisitionlists != null)
            {
                foreach (var item in dto.ClientAcquisitionlists)
                {
                    cas = cas + "," + item;
                }

            }

            string Telcals = "0";
            if (dto.Telecallerlists.Length > 0 && dto.Telecallerlists != null )
            {
                foreach (var item in dto.Telecallerlists)
                {
                    Telcals = Telcals + "," + item;
                }

            }

            string closure = "0";
            if (dto.Closurestatuss != null)
            {
                foreach (var item in dto.Closurestatuss)
                {
                    closure = closure + "," + item;
                }

            }


            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Sales_Portfolio_Report";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                {
                    Value = dto.fromdateNew
                });
                cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                {
                    Value = dto.todateNew
                });
                cmd.Parameters.Add(new SqlParameter("@CAHRME_ID", SqlDbType.VarChar)
                {
                    Value = cas
                });
                cmd.Parameters.Add(new SqlParameter("@TCHRME_ID", SqlDbType.VarChar)
                {
                    Value = Telcals
                });
                cmd.Parameters.Add(new SqlParameter("@BDEHRME_ID", SqlDbType.VarChar)
                {
                    Value = bdes
                });
                cmd.Parameters.Add(new SqlParameter("@Revenue", SqlDbType.VarChar)
                {
                    Value = Revenues
                });
                cmd.Parameters.Add(new SqlParameter("@Region", SqlDbType.VarChar)
                {
                    Value = Region
                });
                cmd.Parameters.Add(new SqlParameter("@Closure", SqlDbType.VarChar)
                {
                    Value = closure
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
                    dto.SalesPortfolioReport = retObject.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return dto;
        }


        public ISM_Client_Project_DTO SalesRefferalLeadOnload(ISM_Client_Project_DTO dto)
        {

            


            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Referral_Lead_Client";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                {
                    Value = "Client"
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
                    dto.ReferralClientData = retObject.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Referral_Lead_Client";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                {
                    Value = "CallStatus"
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
                    dto.CallStatusData = retObject.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ISM_Referral_Lead_Management_Details";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt)
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
                    dto.referralDatasLoad = retObject.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }


            //report onload
            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Ism_Sales_Portfolio_Details";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                {
                    Value = "CA"
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
                    dto.ReffralLeadReportonload = retObject.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }


            return dto;
        }

        public ISM_Client_Project_DTO SaveSalesRefferalLead(ISM_Client_Project_DTO dto)
        {
            try
            {

                if(dto.referralDatasNew[0].ISMRLM_Id > 0)
                {
                    int countnum = 0;
                    foreach (var d in dto.referralDatasNew)
                    {
                        var updatedata = _vmsconte.Database.ExecuteSqlCommand("ISM_Referral_Lead_Management_Update  @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12",
                            d.ISMRLM_Id,
                                          d.ISMMCLT_Id,
                                          d.callstatus,
                                          d.reffredpersonname,
                                          d.reffredpersonContactNumber,
                                          d.reffredpersonEmail,
                                          d.relationwithclient,
                                          d.Remarks,
                                          d.RefferalPersonName,
                                          d.Refferalconatctnumber,
                                          d.Refferalemail,
                                          d.Remarks,
                                          dto.User_Id);
                        if (updatedata > 0)
                        {
                            countnum = countnum + updatedata;
                        }

                    }
                    if (countnum > 0)
                    {
                        dto.returndata = "update";
                    }
                    else
                    {
                        dto.returndata = "Notupdate";
                    }
                }

                else
                {
                    int countnum = 0;
                    foreach (var d in dto.referralDatasNew)
                    {
                        var savedata = _vmsconte.Database.ExecuteSqlCommand("ISM_Referral_Lead_Management_save @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11",
                                          d.ISMMCLT_Id,
                                          d.callstatus,
                                          d.reffredpersonname,
                                          d.reffredpersonContactNumber,
                                          d.reffredpersonEmail,
                                          d.relationwithclient,
                                          d.Remarks,
                                          d.RefferalPersonName,
                                          d.Refferalconatctnumber,
                                          d.Refferalemail,
                                          d.Remarks,
                                          dto.User_Id);
                        if (savedata > 0)
                        {
                            countnum = countnum + savedata;
                        }

                    }
                    if (countnum > 0)
                    {
                        dto.returndata = "Save";
                    }
                    else
                    {
                        dto.returndata = "NotSave";
                    }
                }
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return dto;
        }


        public ISM_Client_Project_DTO SalesReferralLeadReport(ISM_Client_Project_DTO dto)
        {
            try
            {
                if(dto.fromdateNew == null)
                {
                    dto.fromdateNew = "";
                }
                if (dto.todateNew == null)
                {
                    dto.todateNew = "";
                }

                string hrmeids = "0";

                if(dto.ClientAcquisitionlists.Length > 0 && dto.ClientAcquisitionlists != null)
                {
                    foreach (var d in dto.ClientAcquisitionlists)
                    {
                        hrmeids = hrmeids + "," + d;
                    }
                }

                string status = "";

                if (dto.Closurestatuss.Length > 0 && dto.Closurestatuss != null)
                {
                    foreach (var d in dto.Closurestatuss)
                    {
                        status = status + "," + d;
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Referal_Sales_Lead_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.BigInt)
                    {
                        Value = dto.month
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                    {
                        Value = dto.fromdateNew
                    });

                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = dto.todateNew
                    });

                    cmd.Parameters.Add(new SqlParameter("@Hrme_Id", SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = status
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
                        dto.ReffralLeadReport = retObject.ToArray();
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



        public ISM_Client_Project_DTO Ism_sales_ChannelParterReport(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.fromdateNew == null)
                {
                    dto.fromdateNew = "";
                }
                if (dto.todateNew == null)
                {
                    dto.todateNew = "";
                }

                //string hrmeids = "0";

                //if (dto.ClientAcquisitionlists.Length > 0 && dto.ClientAcquisitionlists != null)
                //{
                //    foreach (var d in dto.ClientAcquisitionlists)
                //    {
                //        hrmeids = hrmeids + "," + d;
                //    }
                //}

                //string status = "";

                //if (dto.Closurestatuss.Length > 0 && dto.Closurestatuss != null)
                //{
                //    foreach (var d in dto.Closurestatuss)
                //    {
                //        status = status + "," + d;
                //    }
                //}


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_ChannelPartner_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                    {
                        Value = dto.fromdateNew
                    });

                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = dto.todateNew
                    });

                    //cmd.Parameters.Add(new SqlParameter("@Hrme_Id", SqlDbType.VarChar)
                    //{
                    //    Value = hrmeids
                    //});

                    //cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    //{
                    //    Value = status
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
                        dto.Ism_Sales_ChannelParterReport = retObject.ToArray();
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


        public ISM_Client_Project_DTO Ism_Sales_ChannelParter_lead_Report(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.fromdateNew == null)
                {
                    dto.fromdateNew = "";
                }
                if (dto.todateNew == null)
                {
                    dto.todateNew = "";
                }

             
                string cpdata ="0";
                if (dto.cplists.Length > 0 && dto.cplists != null)
                {
                    foreach (var d in dto.cplists)
                    {
                        cpdata = cpdata + "," + d;
                    }
                }

                string statuslist = "";
                if (dto.Closurestatuss.Length > 0 && dto.Closurestatuss != null)
                {
                    foreach (var d in dto.Closurestatuss)
                    {
                        statuslist = statuslist + "," + d;
                    }
                }


                string regionlist = "0";
                if (dto.RegionlistLists.Length > 0 && dto.RegionlistLists != null)
                {
                    foreach (var d in dto.RegionlistLists)
                    {
                        regionlist = regionlist + "," + d;
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_ChannelPartner_lead_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@channelPartner", SqlDbType.VarChar)
                    {
                        Value = cpdata
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = statuslist
                    });
                    cmd.Parameters.Add(new SqlParameter("@Region", SqlDbType.VarChar)
                    {
                        Value = regionlist
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = dto.fromdateNew
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = dto.todateNew
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
                        dto.Ism_Sales_ChannelParter_lead_Report = retObject.ToArray();
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





        public ISM_Client_Project_DTO Getmodules(ISM_Client_Project_DTO dto)
        {
            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ISM_ImplementationPending_Modules";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                {
                    Value = dto.ISMMCLT_Id
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
                    dto.welcomeNonImplementation = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return dto;
        }



        //mano

        public async Task<General_SendSMSDTO> savedetail(General_SendSMSDTO data)
        {
            //if (data.radiotype == "General")
            //{
            //    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
            //    {
            //        if (data.mobilenolist.Length > 0)
            //        {
            //            foreach (var item in data.mobilenolist)
            //            {
            //                if (data.sms_flag == true)
            //                {
            //                    string s = await sms.sendsmsfromPortal(data.MI_Id, Convert.ToInt64(item.hrmemnO_MobileNo), data.mes);
            //                }
            //                //if (data.whatsapp_flag == true)
            //                //{
            //                //    string s = await sms.SendwhatsappfromPortal(data.MI_Id, Convert.ToInt64(item.hrmemnO_MobileNo), data.mes, data.fileattachementforwhatsapp, data.whatsapp_filetype);
            //                //}
            //            }
            //        }

            //        data.smsStatus = "sent";

            //        //if (s.Equals("Success"))
            //        //{
            //        //    data.smsStatus = "sent";
            //        //}
            //        //else
            //        //{
            //        //    data.smsStatus = "failed";
            //        //}
            //    }
            //}

            if (data.radiotype == "Gemail")

            {
                EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                {

                    foreach (var item in data.emaillist)
                    {
                        using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Mai_triiger_Client_general";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                            {
                                Value = item.ISMMCLT_Id
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
                                        //retObject.Add((ExpandoObject)dataRow);
                                        data.AMST_AdmNo = Convert.ToString(dataReader["AAA"]);
                                        data.HRME_EmployeeFirstName = Convert.ToString(dataReader["BBB"]);
                                        data.HRME_EmployeeMiddleName = Convert.ToString(dataReader["CCC"]);
                                        data.AMST_emailId = Convert.ToString(dataReader["InviceNo"]);
                                        data.ASMC_SectionName = Convert.ToString(dataReader["DDD"]);
                                    }
                                }
                                // data.employeedropdown = retObject.ToArray();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }


                        string currentDate = DateTime.Now.ToString("dd-MM-yyyy"); // Formats the current date

                        //   string messageheader = "Date:27-06-2025<br>"+data.AMST_emailId+"<br>To<br>" +
                        //data.AMST_AdmNo + "<br>" +
                        //data.HRME_EmployeeFirstName + "<br>" +

                        //"<strong>" + data.HRME_EmployeeMiddleName +

                        //".</strong><br />";
                        string messageheader = "<strong>Date:</strong> " + currentDate + "<br><br>" +
                          "<strong>Ref no:</strong>" + data.AMST_emailId + "<br><br>To<br>" +
                          data.AMST_AdmNo + "<br>" +
                          data.HRME_EmployeeFirstName + "<br>" +
                          "<strong>" + data.HRME_EmployeeMiddleName + ".</strong><br />" + "<strong>" + data.ASMC_SectionName + ".</strong><br />";

                        string s = email.sendmailWT(data.MI_Id, item.ISMMCLT_EmailId, item.ISMMCLT_Id, data.AMST_emailId, data.mes, 7, data.esubject, data.footer, data.filelist, messageheader, "", data.atchflag, data.fhead);

                    }
                }

                data.smsStatus = "sent";


            }


            return data;
        }


        public ISM_Client_Project_DTO Save_HealthCheckupAssign(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTPRMP_Id > 0)
                {
                    var result = _vmsconte.ISM_HealthCheckup_Assign_DMO.Single(a => a.ISMHCA_Id == dto.ISMHCA_Id);


                    result.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    result.ISMHCA_Quarter = dto.ISMHCA_Quarter;
                    result.ISMHCA_HealthCheckupDate = dto.ISMHCA_HealthCheckupDate;
                    result.ISMHCA_AcademicYear = dto.ISMHCA_AcademicYear;
                    result.ISMHCA_UpdatedBy = dto.User_Id;
                    result.ISMHCA_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    ISM_HealthCheckup_Assign_DMO cmc = new ISM_HealthCheckup_Assign_DMO();

                    cmc.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    cmc.ISMHCA_Quarter = dto.ISMHCA_Quarter;
                    cmc.ISMHCA_AcademicYear = dto.ISMHCA_AcademicYear;
                    cmc.ISMHCA_HealthCheckupDate = dto.ISMHCA_HealthCheckupDate;
                    cmc.ISMHCA_ActiveFlag = true;
                    cmc.ISMHCA_CreatedBy = dto.User_Id;
                    cmc.ISMHCA_CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO Save_Elearning_Enhancement(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMPEE_Id > 0)
                {
                    var result = _vmsconte.ISM_Postsales_enhancement_Elearning_DMO.Single(a => a.ISMPEE_Id == dto.ISMPEE_Id);


                    result.ISMMCLT_Id = dto.ISMMCLT_Id;
                    result.ISMPEE_Enhancement = dto.ISMPEE_Enhancement;
                    result.ISMPEE_Studentstrength = dto.ISMPEE_Studentstrength;
                    result.ISMPEE_AcademicYear = dto.ISMPEE_AcademicYear;
                    result.ISMPEE_UpdatedBy = dto.User_Id;
                    result.ISMPEE_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    ISM_Postsales_enhancement_Elearning_DMO cmc = new ISM_Postsales_enhancement_Elearning_DMO();

                    cmc.ISMMCLT_Id = dto.ISMMCLT_Id;
                    cmc.ISMPEE_Enhancement = dto.ISMPEE_Enhancement;
                    cmc.ISMPEE_AcademicYear = dto.ISMPEE_AcademicYear;
                    cmc.ISMPEE_Studentstrength = dto.ISMPEE_Studentstrength;


                    cmc.ISMPEE_ActiveFlag = true;
                    cmc.ISMPEE_CreatedBy = dto.User_Id;
                    cmc.ISMPEE_CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        public ISM_Client_Project_DTO Save_accounts(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMACC_Id > 0)
                {
                    var result = _vmsconte.ISM_Accounts_Team_mapping_DMO.Single(a => a.ISMACC_Id == dto.ISMACC_Id);


                    result.ISMMCLT_Id = dto.ISMMCLT_Id;
                    result.HRME_Id = dto.HRME_Id;
                    result.ISMSMPR_Id = dto.ISMSMPR_Id;
                    result.MI_Id = dto.MI_Id;
                    result.ISMACC_UpdatedBy = dto.User_Id;
                    result.ISMACC_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    ISM_Accounts_Team_mapping_DMO cmc = new ISM_Accounts_Team_mapping_DMO();

                    cmc.ISMMCLT_Id = dto.ISMMCLT_Id;
                    cmc.HRME_Id = dto.HRME_Id;
                    cmc.ISMSMPR_Id = dto.ISMSMPR_Id;
                    cmc.MI_Id = dto.MI_Id;
                    cmc.ISMACC_CreatedBy = dto.User_Id;
                    cmc.ISMACC_CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        public ISM_Client_Project_DTO Impending_module(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMMP_Id > 0)
                {
                    var result = _vmsconte.ISM_Module_Pending_DMO.Single(a => a.ISMMP_Id == dto.ISMMP_Id);


                    result.ISMMCLT_Id = dto.ISMMCLT_Id;
                    result.IVRMM_Id = dto.IVRMM_Id;
                    result.ISMMP_Date = dto.ISMMP_Date;
                    result.ISMMP_AcademicYear = dto.ISMMP_AcademicYear;
                    result.ISMMP_UpdatedBy = dto.User_Id;
                    result.ISMMP_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    ISM_Module_Pending_DMO cmc = new ISM_Module_Pending_DMO();

                    cmc.ISMMCLT_Id = dto.ISMMCLT_Id;
                    cmc.IVRMM_Id = dto.IVRMM_Id;
                    cmc.ISMMP_AcademicYear = dto.ISMMP_AcademicYear;
                    cmc.ISMMP_Date = dto.ISMMP_Date;
                    cmc.ISMMP_ActiveFlag = true;
                    cmc.ISMMP_CreatedBy = dto.User_Id;
                    cmc.ISMMP_CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        public ISM_Client_Project_DTO deactivate_Healthcheckup(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_HealthCheckup_Assign_DMO.Single(a => a.ISMHCA_Id == dto.ISMHCA_Id);
                if (dto.ISMHCA_ActiveFlag == true)
                {
                    result.ISMHCA_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMHCA_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO editImplementationDetails(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Edit_ModulePendingById";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMMP_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMMP_Id
                    });
                    //cmd.Parameters.Add(new SqlParameter("@ISMCLTPRMP_Id", SqlDbType.BigInt)
                    //{
                    //    Value = dto.ISMCLTPRMP_Id
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
                        dto.mp_details = retObject.ToArray();
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
    }
}
