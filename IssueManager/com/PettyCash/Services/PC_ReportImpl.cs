using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

namespace IssueManager.com.PettyCash.Services
{
    public class PC_ReportImpl : Interface.PC_ReportInterface
    {
        public PettyCashContext _pettyCashContext;
        public PC_ReportImpl(PettyCashContext _pettyCash)
        {
            _pettyCashContext = _pettyCash;
        }
        public PC_ReportDTO onloaddata(PC_ReportDTO data)
        {
            try
            {
                var getuserinstitution = _pettyCashContext.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                data.getuserinstitution = _pettyCashContext.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_ReportDTO getrequisitionreport(PC_ReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                fromdatecon = Convert.ToDateTime(data.Fromdate.Value.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.Todate.Value.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                using (var cmd = _pettyCashContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PettyCash_Requistion_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = contodate
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
                        data.getrequisitionreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.institutiondetails = _pettyCashContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_ReportDTO getindentreport(PC_ReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                fromdatecon = Convert.ToDateTime(data.Fromdate.Value.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.Todate.Value.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                using (var cmd = _pettyCashContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PettyCash_Indent_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = contodate
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
                        data.getindentreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.institutiondetails = _pettyCashContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_ReportDTO getindentapprovedreport(PC_ReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                fromdatecon = Convert.ToDateTime(data.Fromdate.Value.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.Todate.Value.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                using (var cmd = _pettyCashContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PettyCash_Indent_Approved_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)

                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = contodate
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
                        data.getindenapprovedreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.institutiondetails = _pettyCashContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_ReportDTO getexpenditurereport(PC_ReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                fromdatecon = Convert.ToDateTime(data.Fromdate.Value.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.Todate.Value.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                using (var cmd = _pettyCashContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PettyCash_Expenditure_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)

                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = contodate
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
                        data.getindenapprovedreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.institutiondetails = _pettyCashContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_ReportDTO getexpenditurereports(PC_ReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                fromdatecon = Convert.ToDateTime(data.Fromdate.Value.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.Todate.Value.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                using (var cmd = _pettyCashContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PettyCash_Expenditure_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)

                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = contodate
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
                        data.getindenapprovedreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.institutiondetails = _pettyCashContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_ReportDTO onloaddatabalancesheet(PC_ReportDTO data)
        {
            try
            {
                var getuserinstitution = _pettyCashContext.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                data.getuserinstitution = _pettyCashContext.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();

                List<long> ids = new List<long>();


                var getexcependitureids = _pettyCashContext.PC_ExpenditureDMO.Where(a => a.MI_Id == data.MI_Id && a.PCEXPTR_ActiveFlg == true).Select(a => a.PCINDENTAP_Id).Distinct().ToArray();


                var getapprovedids = _pettyCashContext.PC_Indent_ApprovedDMO.Where(a => a.MI_Id == data.MI_Id 
                && a.PCINDENTAPT_ActiveFlg == true && getexcependitureids.Contains(a.PCINDENTAP_Id)).Select(a => a.PCINDENT_Id).Distinct().ToList();

                data.indentdetails = _pettyCashContext.PC_IndentDMO.Where(a => a.MI_Id == data.MI_Id && a.PCINDENT_ActiveFlg == true
                && getapprovedids.Contains(a.PCINDENT_Id)).OrderByDescending(a => a.PCINDENT_Date).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_ReportDTO OnChangeBalanceSheetInstitution(PC_ReportDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                var getexcependitureids = _pettyCashContext.PC_ExpenditureDMO.Where(a => a.MI_Id == data.MI_Id && a.PCEXPTR_ActiveFlg == true).Select(a => a.PCINDENTAP_Id).Distinct().ToArray();

                var getapprovedids = _pettyCashContext.PC_Indent_ApprovedDMO.Where(a => a.MI_Id == data.MI_Id 
                && a.PCINDENTAPT_ActiveFlg == true && getexcependitureids.Contains(a.PCINDENTAP_Id)).Select(a => a.PCINDENT_Id).Distinct().ToList();

                data.indentdetails = _pettyCashContext.PC_IndentDMO.Where(a => a.MI_Id == data.MI_Id && a.PCINDENT_ActiveFlg == true
                && getapprovedids.Contains(a.PCINDENT_Id)).OrderByDescending(a => a.PCINDENT_Date).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_ReportDTO getbalancesheetreports(PC_ReportDTO data)
        {
            try
            {
                using (var cmd = _pettyCashContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PC_IndentExpenditure_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)

                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@PCINDENT_Id", SqlDbType.VarChar)
                    {
                        Value = data.PCINDENT_Id
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
                        data.getindenapprovedreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.institutiondetails = _pettyCashContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getindentdetails = _pettyCashContext.PC_Indent_ApprovedDMO.Where(a => a.MI_Id == data.MI_Id && a.PCINDENT_Id==data.PCINDENT_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //=====Expendature Balance Report======//
        public PC_ReportDTO getexpenditurebalancereports(PC_ReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                fromdatecon = Convert.ToDateTime(data.Fromdate.Value.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.Todate.Value.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                using (var cmd = _pettyCashContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PC_IndentExpenditure_Details_Betweendates";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                    {
                        Value = contodate
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
                        data.getexpendaturebalancereport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.institutiondetails = _pettyCashContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
