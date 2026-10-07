using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ModuleWiseFeedbackReportImpl : Interfaces.ModuleWiseFeedbackReportInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ModuleWiseFeedbackReportImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public ModuleWiseFeedbackReport_DTO OnLoadClient(ModuleWiseFeedbackReport_DTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getclientdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
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
                        data.masterclientlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.hr_masteryear = _IssueContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRMLY_ActiveFlag == true).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;

        }
        public ModuleWiseFeedbackReport_DTO getfeedbackreport(ModuleWiseFeedbackReport_DTO data)
        {
            try
            {

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ModuleWiseFeedbackReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.clientlistarr[0].IVRM_MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Institute_code", SqlDbType.VarChar) { Value = data.clientlistarr[0].Institute_code });
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
                        data.getfeedbackdata = retObject.ToArray();
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
        public ModuleWiseFeedbackReport_DTO getvmsmodulefeedbackreport(ModuleWiseFeedbackReport_DTO data)
        {
            try
            {
                if (data.optionflag == "Average")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Module_AvergeRating";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.VarChar) { Value = data.r_month });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = data.r_year });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Modulewise" });
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
                            data.vmsmoduleratingreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Module_AvergeRating";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.VarChar) { Value = data.r_month });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = data.r_year });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Individual" });
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
                            data.vmsmoduleratingreport = retObject.ToArray();
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
        public ModuleWiseFeedbackReport_DTO saveModuleWisefeedback(ModuleWiseFeedbackReport_DTO data)
        {
            try
            {


                if (data.savemodulefeedback.Count() > 0)
                {

                    for (int j = 0; j < data.savemodulefeedback.Count(); j++)
                    {
                        var outputval = 0;

                        outputval = _IssueContext.Database.ExecuteSqlCommand("Feedback_Insert_Modulewise @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13", data.IVRM_MI_Id, data.Institute_Name, data.savemodulefeedback[j].FMTY_Id, data.savemodulefeedback[j].FMQE_Id, Convert.ToInt64(data.savemodulefeedback[j].name), data.savemodulefeedback[j].FMOP_OptionsValue, data.savemodulefeedback[j].FMOP_FeedbackOptions, data.savemodulefeedback[j].FSTTR_FeedBack, data.UserId, data.staffname, data.savemodulefeedback[j].FMTY_FeedbackTypeName, data.savemodulefeedback[j].FMQE_FeedbackQuestions, data.savemodulefeedback[j].FMQE_FeedbackQRemarks, data.Institute_code);

                        data.returnval = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }
        public ModuleWiseFeedbackReport_DTO getcanteenandhousekeepingfeedback(ModuleWiseFeedbackReport_DTO data)
        {
            try
            {
                if (data.optionflag == "Average")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Canteen_HouseKeeping_Feedback_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.VarChar) { Value = data.type });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Avg" });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar) { Value = data.fromdate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = data.todate.ToString("yyyy-MM-dd") });
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
                            data.canteenfeedbackreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Canteen_HouseKeeping_Feedback_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.VarChar) { Value = data.type });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Overall" });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar) { Value = data.fromdate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = data.todate.ToString("yyyy-MM-dd") });
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
                            data.canteenfeedbackreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception x)
            {
                Console.Write(x);
            }

            return data;
        }
    }
}
