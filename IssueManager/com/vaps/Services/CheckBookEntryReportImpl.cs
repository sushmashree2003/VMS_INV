

using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class CheckBookEntryReportImpl : Interfaces.CheckBookEntryReportInterface
    {
        public IssueManagerContext _INVContext;
        ILogger<CheckBookEntryReportImpl> _logInv;
        public CheckBookEntryReportImpl(IssueManagerContext InvContext, ILogger<CheckBookEntryReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public CheckBookEntryReportDTO getloaddata(CheckBookEntryReportDTO data)
        {
            try
            {
                //var stfck1 = _INVContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                //var stfcheck1 = _INVContext.applicationRole.Where(a => a.Id == stfck1[0].RoleId).ToList();
                //data.staffcheck = stfcheck1[0].Name;

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_master_institution_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Userid",
            SqlDbType.VarChar)
                    {
                        Value = data.UserId
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
                        data.get_mi_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {

                    string miid = "0";
                    //var stfck = _INVContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                    //var stfcheck = _INVContext.applicationRole.Where(a => a.Id == stfck[0].RoleId).ToList();

                    if (data.get_mi_arraylist != null)
                    {
                        foreach (var ii in data.get_mi_arraylist)
                        {
                            miid = miid + "," + ii.MI_Id.ToString();
                        }
                    }
                    if (data.optionflag == null)
                    {
                        data.optionflag = "ALL";
                    }


                    cmd.CommandText = "GET_CHECKBOOK_SUPPLIER";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = miid
                    });

                    cmd.Parameters.Add(new SqlParameter("@optionflag",
            SqlDbType.VarChar)
                    {
                        Value = data.optionflag
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
                        data.get_PI_details = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Report Page load:" + ex.Message);
            }
            return data;
        }

        //public CheckBookEntryReportDTO onreport(CheckBookEntryReportDTO data)
        //{
        //    try
        //    {
        //        string piids = "0";
        //        string invmids = "0";
        //        string miid = "0";
        //        string startdate = "";
        //        string enddate = "";
        //        if(data.startdate !=null && data.enddate !=null)
        //        {
        //            startdate = data.startdate.Value.ToString();
        //            enddate = data.enddate.Value.ToString();
        //        }
        //        foreach (var ii in data.get_mi_arraylist)
        //        {
        //            miid = miid + "," + ii.MI_Id;
        //        }

        //        if (data.optionflag == "PIno")
        //        {
        //            if (data.piArray != null)
        //            {
        //                foreach (var g in data.piArray)
        //                {
        //                    piids = piids + "," + g.INVMS_Id;
        //                }
        //            }
        //        }
        //        else if (data.optionflag == "Item")
        //        {
        //            if (data.itemArray != null)
        //            {
        //                foreach (var i in data.itemArray)
        //                {
        //                    invmids = invmids + "," + i.VPAYVOU_Id;
        //                }
        //            }
        //        }
        //        else if (data.optionflag == "PV")
        //        {
        //            if (data.itemArray != null)
        //            {
        //                foreach (var i in data.itemArray)
        //                {
        //                    invmids = invmids + "," + i.VPAYVOU_Id;
        //                }
        //            }
        //        }

        //        using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "Checkbook_Report";
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add(new SqlParameter("@FromDate",
        //          SqlDbType.VarChar)
        //            {
        //                Value = startdate
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@ToDate",
        //            SqlDbType.VarChar)
        //            {
        //                Value = enddate
        //            });

        //            cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //     SqlDbType.VarChar)
        //            {
        //                Value = miid
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
        //           SqlDbType.VarChar)
        //            {
        //                Value = piids
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@VPAYVOU_Id",
        //          SqlDbType.VarChar)
        //            {
        //                Value = invmids
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@status",
        //          SqlDbType.VarChar)
        //            {
        //                Value = data.status
        //            });

        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();

        //            var retObject = new List<dynamic>();
        //            try
        //            {
        //                using (var dataReader = cmd.ExecuteReader())
        //                {
        //                    while (dataReader.Read())
        //                    {
        //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                        {
        //                            dataRow.Add(
        //                                dataReader.GetName(iFiled),
        //                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                            );
        //                        }
        //                        retObject.Add((ExpandoObject)dataRow);
        //                    }
        //                }
        //                data.get_PIreport = retObject.ToArray();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logInv.LogInformation("PI Report:" + ex.Message);
        //    }
        //    return data;
        //}

        public CheckBookEntryReportDTO onreport(CheckBookEntryReportDTO data)
        {
            try
            {
                string piids = "0";
                string invmids = "0";
                string miid = "0";
                string startdate = "";
                string enddate = "";
                if (data.startdate != null && data.enddate != null)
                {
                    startdate = data.startdate.Value.ToString("yyyy-MM-dd");
                    enddate = data.enddate.Value.ToString("yyyy-MM-dd");
                    // startdate = data.startdate.Value.ToString();
                    //enddate = data.enddate.Value.ToString();
                }
                foreach (var ii in data.get_mi_arraylist)
                {
                    miid = miid + "," + ii.MI_Id;
                }

                if (data.optionflag == "PIno")
                {
                    if (data.piArray != null)
                    {
                        foreach (var g in data.piArray)
                        {
                            piids = piids + "," + g.INVMS_Id;
                        }
                    }
                }
                else if (data.optionflag == "Item")
                {
                    if (data.itemArray != null)
                    {
                        foreach (var i in data.itemArray)
                        {
                            invmids = invmids + "," + i.VPAYVOU_Id;
                        }
                    }
                }
                else if (data.optionflag == "PV")
                {
                    if (data.itemArray != null)
                    {
                        foreach (var i in data.itemArray)
                        {
                            invmids = invmids + "," + i.VPAYVOU_Id;
                        }
                    }
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Checkbook_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

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

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = miid
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                   SqlDbType.VarChar)
                    {
                        Value = piids
                    });
                    cmd.Parameters.Add(new SqlParameter("@VPAYVOU_Id",
                  SqlDbType.VarChar)
                    {
                        Value = invmids
                    });
                    cmd.Parameters.Add(new SqlParameter("@status",
                  SqlDbType.VarChar)
                    {
                        Value = data.status
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
                        data.get_PIreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Report:" + ex.Message);
            }
            return data;
        }
    }
}
