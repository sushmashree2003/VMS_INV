using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using PreadmissionDTOs.com.vaps.FMS;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace FMS.Com.Services
{
    public class FMS_Correspondance_ReportImpl : Interface.FMS_Correspondance_ReportInterface
    {
        public FMSContext _FMSContext;
        public FMS_Correspondance_ReportImpl(FMSContext FMS)
        {
            _FMSContext = FMS;
        }
        public FMS_Correspondance_ReportDTO onload(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                data.getclient = _FMSContext.ISM_Master_Client_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true).ToArray();
                data.getsupplier = _FMSContext.INV_Master_SupplierDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMS_ActiveFlg == true).ToArray();
                data.getsaleslead = _FMSContext.ISM_Sales_Lead_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Correspondance_ReportDTO getprojectist(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                List<long> clientid = new List<long>();

                foreach (var c in data.FMS_Correspondance_Report_Client_DTO)
                {
                    clientid.Add(c.ISMMCLT_Id);
                }

                data.getproject = (from a in _FMSContext.ISM_Master_Client_ProjectDMO
                                   from b in _FMSContext.ISM_Master_Client_DMO
                                   from c in _FMSContext.MastersProject_DMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && c.ISMMPR_Id == a.ISMMPR_Id
                                   && a.ISMMCLTPR_ActiveFlag == true && clientid.Contains(a.ISMMCLT_Id) && b.MI_Id == data.MI_Id)
                                   select new FMS_CorrespondenceDTO
                                   {
                                       ISMMPR_Id = c.ISMMPR_Id,
                                       ISMMPR_ProjectName = c.ISMMPR_ProjectName
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Correspondance_ReportDTO getreport(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                string clientid = "0";
                string projectid = "0";
                string supplierid = "0";
                string salesleadid = "0";
                string filecategorry = "0";
                string flag = "";

                if (data.flag != null && data.flag != "")
                {
                    flag = data.flag;
                }



                if (data.FMS_Correspondance_Report_Client_DTO != null && data.FMS_Correspondance_Report_Client_DTO.Length > 0)
                {
                    foreach (var c in data.FMS_Correspondance_Report_Client_DTO)
                    {
                        clientid = clientid + "," + c.ISMMCLT_Id;
                    }
                }

                if (data.FMS_Correspondance_Report_Project_DTO != null && data.FMS_Correspondance_Report_Project_DTO.Length > 0)
                {
                    foreach (var c in data.FMS_Correspondance_Report_Project_DTO)
                    {
                        projectid = projectid + "," + c.ISMMPR_Id;
                    }
                }

                if (data.FMS_Correspondance_Report_Supplier_DTO != null && data.FMS_Correspondance_Report_Supplier_DTO.Length > 0)
                {
                    foreach (var c in data.FMS_Correspondance_Report_Supplier_DTO)
                    {
                        supplierid = supplierid + "," + c.INVMS_Id;
                    }
                }

                if (data.FMS_Correspondance_Report_Sales_DTO != null && data.FMS_Correspondance_Report_Sales_DTO.Length > 0)
                {
                    foreach (var c in data.FMS_Correspondance_Report_Sales_DTO)
                    {
                        salesleadid = salesleadid + "," + c.ISMSLE_Id;
                    }
                }

                if (data.File_Category_DTO != null && data.File_Category_DTO.Length > 0)
                {
                    foreach (var c in data.File_Category_DTO)
                    {
                        filecategorry = filecategorry + "," + c.FMSMFC_Id;
                    }
                }

                DateTime fromdate = new DateTime();
                DateTime todate = new DateTime();
                string confromdate = "";
                string contodate = "";

                fromdate = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdate.ToString("yyyy-MM-dd");

                todate = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = todate.ToString("yyyy-MM-dd");

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Correspondance_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@CLIENTSUPPLIERFLGFLAG", SqlDbType.VarChar)
                    {
                        Value = data.ClientSupplierFlgflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });

                    cmd.Parameters.Add(new SqlParameter("@CLIENTIDS", SqlDbType.VarChar)
                    {
                        Value = clientid
                    });
                    cmd.Parameters.Add(new SqlParameter("@PROJECTIDS", SqlDbType.VarChar)
                    {
                        Value = projectid
                    });
                    cmd.Parameters.Add(new SqlParameter("@SUPPLIERIDS", SqlDbType.VarChar)
                    {
                        Value = supplierid
                    });
                    cmd.Parameters.Add(new SqlParameter("@SALESID", SqlDbType.BigInt)
                    {
                        Value = salesleadid
                    });
                    cmd.Parameters.Add(new SqlParameter("@FILECATEGORRY", SqlDbType.BigInt)
                    {
                        Value = filecategorry
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
                        data.getreport = retObject.ToArray();
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
            return data;
        }
        public FMS_Correspondance_ReportDTO viewreferencedetais(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                /*CABINET DETAILS*/
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Correspondance_RefernceNo_Details_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_Id", SqlDbType.VarChar)
                    {
                        Value = data.FMSCOR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
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
                        data.getcabintdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                /*FILES DETAILS*/
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Correspondance_RefernceNo_Details_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_Id", SqlDbType.VarChar)
                    {
                        Value = data.FMSCOR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 2
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
                        data.getfilesdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                /*MODE DETAILS*/
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Correspondance_RefernceNo_Details_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_Id", SqlDbType.VarChar)
                    {
                        Value = data.FMSCOR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 3
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
                        data.getmodedetails = retObject.ToArray();
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
            return data;
        }
        public FMS_Correspondance_ReportDTO onchnageradiobtnsfile(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                if (data.ClientSupplierFlgflag == "Client")
                {
                    data.getclient = _FMSContext.ISM_Master_Client_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true).ToArray();
                }
                else if (data.ClientSupplierFlgflag == "Supplier")
                {
                    data.getsupplier = _FMSContext.INV_Master_SupplierDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMS_ActiveFlg == true).ToArray();
                }
                else if (data.ClientSupplierFlgflag == "Sales Lead")
                {
                    data.getsaleslead = _FMSContext.ISM_Sales_Lead_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_ActiveFlag == true).ToArray();
                }
                else if (data.ClientSupplierFlgflag == "File Category")
                {
                    data.getfilecategory = (from a in _FMSContext.FMS_Master_FileCategoryDMO
                                            from b in _FMSContext.FMS_Department_FileCategoryDMO
                                            where (a.FMSMFC_Id == b.FMSMFC_Id && a.MI_Id == data.MI_Id 
                                            && a.FMSMFC_ActiveFlg == true && b.FMSDFC_ActiveFlg == true)
                                            select a).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public FMS_Correspondance_ReportDTO SearchReport(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Search_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@SearchValue", SqlDbType.VarChar)
                    {
                        Value = data.SearchValue
                    });
                    cmd.Parameters.Add(new SqlParameter("@ClientSupplierFlgflag", SqlDbType.VarChar)
                    {
                        Value = data.ClientSupplierFlgflag
                    });

                    cmd.Parameters.Add(new SqlParameter("@UserFlag", SqlDbType.VarChar)
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreport = retObject.ToArray();
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
            return data;
        }
        public FMS_Correspondance_ReportDTO CountReport(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                DateTime fromdate = new DateTime();
                DateTime todate = new DateTime();
                string confromdate = "";
                string contodate = "";

                var getempcode = _FMSContext.Staff_User_Login.Where(a => a.Id == data.Userid).ToList();

                var getempdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempcode.FirstOrDefault().Emp_Code).ToList();

                fromdate = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdate.ToString("yyyy-MM-dd");

                todate = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = todate.ToString("yyyy-MM-dd");

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_FileCategory_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = getempdetails.FirstOrDefault().HRMD_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreport = retObject.ToArray();
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
            return data;
        }
        public FMS_Correspondance_ReportDTO ViewRefernceNoDetails(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                DateTime fromdate = new DateTime();
                DateTime todate = new DateTime();
                string confromdate = "";
                string contodate = "";

                var getempcode = _FMSContext.Staff_User_Login.Where(a => a.Id == data.Userid).ToList();

                var getempdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempcode.FirstOrDefault().Emp_Code).ToList();

                fromdate = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdate.ToString("yyyy-MM-dd");

                todate = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = todate.ToString("yyyy-MM-dd");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // File Cabinet Reports
        public FMS_Correspondance_ReportDTO OnloadFileCabinetReport(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                data.institutionlist = _FMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Correspondance_ReportDTO FileCabinetReport(FMS_Correspondance_ReportDTO data)
        {
            try
            {
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_PhysicalFile_Cabinet_Details_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreport = retObject.ToArray();
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
            return data;
        }
    }
}