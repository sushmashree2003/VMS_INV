using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using PreadmissionDTOs.com.vaps.Legal;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISMDashboardImpl : Interfaces.ISMDashboardInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISMDashboardImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }


        public async Task<ISMDashboardDTO> getdetailsAsync(ISMDashboardDTO data)
        {
            try
            {
                string[] leavecode = new string[2];
                leavecode[0] = "PH";
                leavecode[1] = "OH";

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_TotalLoanAmount";
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Dashboardloandetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.Role_flag = rolet[0].IVRMRT_Role;

                var departmentdatacode = "";

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    var emphrme = _IssueContext.Staff_User_Login.Where(h => h.Id == data.UserId && h.MI_Id == data.MI_Id).ToList();

                    var employeedata = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == emphrme[0].Emp_Code && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();

                    var departmentdata = _IssueContext.HR_Master_Department.Where(d => d.HRMD_Id == employeedata[0].HRMD_Id && d.HRMD_ActiveFlag == true && d.MI_Id == data.MI_Id).ToList();

                    departmentdatacode = _IssueContext.HR_Master_DepartmentCodeDMO.Where(d => d.HRMDC_ID == departmentdata[0].HRMDC_ID).Select(c => c.HRMDC_Code).FirstOrDefault();
                    data.dept_code = departmentdatacode;

                }

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    var emphrmeid = _IssueContext.Staff_User_Login.Where(h => h.Id == data.UserId && h.MI_Id == data.MI_Id).ToList();

                    var empdepartment = _IssueContext.MasterEmployee.Where(a => a.HRME_Id == emphrmeid[0].Emp_Code).ToList();

                    var questionauthority = (from a in _IssueContext.Ratings_Master_QuestionsDMO
                                             from b in _IssueContext.Ratings_Master_Authority_QuestionsDMO
                                             from c in _IssueContext.Rating_Master_AuthorityDMO
                                             where (a.RATMQE_Id == b.RATMQE_Id && b.RATMAUT_Id == c.RATMAUT_Id && a.MI_Id == data.MI_Id && c.HRMD_Id == empdepartment[0].HRMD_Id && c.HRMDES_Id == empdepartment[0].HRMDES_Id && a.RATMQE_ActiveFlag == true && (a.RATMQE_QuestionFlg == "Proxy Role" || a.RATMQE_QuestionFlg == "Enhancement"))
                                             select b).ToList();

                    string RATMQE_Ids = "0";
                    if (questionauthority.Count > 0 && questionauthority != null)
                    {
                        foreach (var ue in questionauthority)
                        {
                            RATMQE_Ids = RATMQE_Ids + "," + ue.RATMQE_Id;
                        }
                    }

                    if (questionauthority.Count > 0)
                    {

                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Rating_ProxyRoleAndEnhancement_FetchEmployee";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@RATMQE_Id", SqlDbType.VarChar) { Value = RATMQE_Ids });
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
                                data.Ratingfetchemployee = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Sales Head", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISm_Sales_Lead_DemoUpdateListdash";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
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
                            data.demo_list = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISm_Sales_Lead_DemoUpdateListdashtellicaller";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
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
                            data.demo_list_tellicaller = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    data.HRME_Id = _IssueContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "DASHBOARDRATING";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                            data.RatingDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CallCenter_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                        //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
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
                            data.callCentreDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //Added by Aman for Vefiry Documents
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_InterviewCandidate_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = data.UserId });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var candidateDetails = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        dataRow.Add(dataReader.GetName(i), dataReader.IsDBNull(i) ? null : dataReader[i]);
                                    }
                                    candidateDetails.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.totalemployees = candidateDetails.Distinct().ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }




                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TADA_PendingBalance_HRMS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
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
                            data.totpending = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMSLOANDASHBOARDDETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                            data.Dashboardloandetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_ProxyLeave_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                            data.proxyleavedetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //PraveenProcedure
                    string HRMD_Id = "0";
                    List<ISMDashboardDTO> Tasklist = new List<ISMDashboardDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_AssetTag_CheckOut_ActiveDeactive_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRMDC_Id", SqlDbType.VarChar) { Value = 0 });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Active_Onload" });
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
                                    Tasklist.Add(new ISMDashboardDTO
                                    {
                                        HRMDC_Id = Convert.ToInt64(dataReader["HRMDC_Id"]),


                                    });
                                }
                            }
                            //  data.employeedetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (Tasklist != null && Tasklist.Count > 0)
                    {

                        foreach (var d in Tasklist)
                        {
                            HRMD_Id = HRMD_Id + ',' + d.HRMDC_Id;
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Department_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "1" });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = HRMD_Id });
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
                            data.departmentname = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Department_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "2" });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = 0 });
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
                            data.departmentcount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //PraveenProcedure


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_PendingIssues";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMPR_Id", SqlDbType.BigInt) { Value = 0 });
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
                            data.proxytaskslist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }






                }
                else
                {
                    data.HRME_Id = 0;
                }
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Accountant", StringComparison.OrdinalIgnoreCase))
                {
                    var Sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Sunday);
                    var sundysecod = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);


                    string FromDate = Sunday.ToString("yyyy-MM-dd");
                    string ToDate = sundysecod.ToString("yyyy-MM-dd");
                    // string FromDate = data.fromdate.ToString("yyyy-MM-dd");
                    // string ToDate = data.todate.ToString("yyyy-MM-dd");

                    for (int b = 0; b < 2; b++)
                    {
                        if (b == 0)
                        {
                            data.TypeFlg = "planer";
                        }
                        if (b == 1)
                        {
                            data.TypeFlg = "petycash";
                        }
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "IsmDashboard";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            //@TypeFlg
                            cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
                            cmd.Parameters.Add(new SqlParameter("@TypeFlg", SqlDbType.VarChar) { Value = data.TypeFlg });
                            cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
                            cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
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
                                if (data.TypeFlg == "petycash")
                                {
                                    data.IsmDashBoard = retObject.ToArray();
                                }
                                if (data.TypeFlg == "planer")
                                {
                                    data.planerapproval = retObject.ToArray();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    //planer approval
                }



                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Sales Head", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LeadDashboardData";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });

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
                            data.DashbaordSalesDetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LeadDashboardData_CurrentDate";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });

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
                            data.DashbaordSalesDetailsCountCurrentDate = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Sales Head", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StatewiseLeadList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });

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
                            data.StatewiseLeadListDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }


                //|| departmentdatacode[0].HRMDC_Code.Equals("ACC", StringComparison.OrdinalIgnoreCase)
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) || departmentdatacode.Equals("ACC", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Accountant", StringComparison.OrdinalIgnoreCase))
                {

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_TotalLoanAmount";
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.Dashboardloandetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TenantsDashboardData";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });

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
                            data.DashbaordTenantDetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PendingIssues";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMPR_Id", SqlDbType.BigInt) { Value = 0 });
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
                        data.prioritylist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GratuityCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                        data.Graduitydetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

              
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetEmployeePLAmount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                        data.EmployeePLAmount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                var leaveyear = _IssueContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_FromDate <= DateTime.Today && t.HRMLY_ToDate >= DateTime.Today && t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).ToArray().FirstOrDefault();

                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                var projectcount = (from p in _IssueContext.MastersProject_DMO
                                    from t in _IssueContext.ISM_TaskCreationDMO
                                    where (p.ISMMPR_Id == t.ISMMPR_Id && t.ISMTCR_ActiveFlg == true && p.ISMMPR_ActiveFlg == true && p.MI_Id == t.MI_Id && t.MI_Id == data.MI_Id && t.HRME_Id == empid)
                                    select new ISMDashboardDTO
                                    {
                                        ISMMPR_Id = p.ISMMPR_Id,
                                        ISMMPR_ProjectName = p.ISMMPR_ProjectName
                                    }).Distinct().ToList();

                var opencount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid && t.ISMTCR_Status == "Open").ToList();
                var closedcount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid && t.ISMTCR_Status == "Closed").ToList();
                var completedcount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid && t.ISMTCR_Status == "Completed").ToList();
                var inprogresscount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid && t.ISMTCR_Status == "Inprogress").ToList();
                var taskcount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid).ToList();

                data.projectCount = projectcount.Count;
                data.taskCount = taskcount.Count;
                data.openCount = opencount.Count;
                data.closedCount = closedcount.Count;
                data.completedCount = completedcount.Count;
                data.inprogressCount = inprogresscount.Count;

                data.employeedetails = (from a in _IssueContext.MasterEmployee
                                        from b in _IssueContext.HR_Master_Department
                                        from c in _IssueContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_Id == empid && b.HRMD_Id == a.HRMD_Id && c.HRMDES_Id == a.HRMDES_Id)
                                        select new ISMDashboardDTO
                                        {
                                            employeename = (a.HRME_EmployeeFirstName + " " + a.HRME_EmployeeMiddleName + " " + a.HRME_EmployeeLastName).Trim(),
                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                            HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                            HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                            HRMD_Id = a.HRMD_Id,
                                            HRMD_DepartmentName = b.HRMD_DepartmentName,
                                            HRMDES_Id = a.HRMDES_Id,
                                            HRMDES_DesignationName = c.HRMDES_DesignationName,
                                            HRME_Photo = a.HRME_Photo,
                                            HRME_DOB = a.HRME_DOB,
                                            DeviceID = a.HRME_AppDownloadedDeviceId,
                                            HRME_Id = a.HRME_Id
                                        }).ToArray();

                //data.birthdaylist = _IssueContext.MasterEmployee.Where(t => t.HRME_DOB.Value.Day == DateTime.Today.Day && t.HRME_DOB.Value.Month == DateTime.Today.Month && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();

                data.birthdaylist = (from t in _IssueContext.MasterEmployee
                                     from b in _IssueContext.HR_Master_Designation
                                     where (b.HRMDES_Id == t.HRMDES_Id && t.HRME_DOB.Value.Day == DateTime.Today.Day && t.HRME_DOB.Value.Month == DateTime.Today.Month && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && b.MI_Id == t.MI_Id)
                                     select new ISMDashboardDTO
                                     {
                                         HRME_EmployeeFirstName = t.HRME_EmployeeFirstName,
                                         HRME_EmployeeMiddleName = t.HRME_EmployeeMiddleName,
                                         HRME_EmployeeLastName = t.HRME_EmployeeLastName,
                                         HRMDES_DesignationName = b.HRMDES_DesignationName,
                                         HRME_Photo = t.HRME_Photo,
                                     }).ToArray();
                if (leaveyear != null)
                {
                    data.leavedetails = (from a in _IssueContext.HR_Emp_Leave_StatusDMO
                                         from b in _IssueContext.HR_Master_Leave_DMO
                                         where (a.HRME_Id == empid && a.MI_Id == data.MI_Id && a.HRMLY_Id == leaveyear.HRMLY_Id && a.HRML_Id == b.HRML_Id && b.MI_Id == data.MI_Id && b.HRML_LeaveCode == "CL")
                                         select new ISMDashboardDTO
                                         {
                                             HRELS_TotalLeaves = a.HRELS_TotalLeaves,
                                             HRELS_CBLeaves = a.HRELS_CBLeaves
                                         }).ToArray();

                    data.upcomingholiday = (from a in _IssueContext.HolidayWorkingDayTypeDMO
                                            from b in _IssueContext.FO_Master_HolidayWorkingDay_DatesDMO
                                            where (a.MI_Id == data.MI_Id && leavecode.Contains(a.FOHTWD_HolidayWDTypeFlag) && a.FOHWDT_ActiveFlg == true && b.MI_Id == a.MI_Id && b.HRMLY_Id == leaveyear.HRMLY_Id && b.FOHWDT_Id == a.FOHWDT_Id && b.FOMHWDD_FromDate > DateTime.Today && b.FOMHWDD_Name != "Sunday" && b.FOMHWD_ActiveFlg == true)
                                            select new ISMDashboardDTO
                                            {
                                                FOMHWDD_Name = b.FOMHWDD_Name,
                                                FOHTWD_HolidayWDType = a.FOHTWD_HolidayWDTypeFlag,
                                                FOMHWDD_FromDate = b.FOMHWDD_FromDate
                                            }).OrderBy(b => b.FOMHWDD_FromDate).ToArray();
                }

                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = DateTime.Today;

                // DateTime previousstartdate = DateTime.Now;
                //DateTime previousenddate = DateTime.Now;
                string previousstart = "";
                //var getmonth = now.Month-1 ;
                var getmonth = DateTime.Now.AddMonths(-1).Month;
                var today = DateTime.Today;
                var month = new DateTime(today.Year, today.Month, 1);
                var previousstartdate = month.AddMonths(-1);
                var previousenddate = month.AddDays(-1);
                if (getmonth >= 1)
                {
                    getmonth = Convert.ToInt32(getmonth);
                    var getyear = now.Year;
                    previousstart = getyear + "-" + getmonth + "-01";
                    // previousstartdate = Convert.ToDateTime(previousstart);
                    var mon = Convert.ToDateTime(previousstartdate).Month;
                    var startOfMonth = new DateTime(getyear, getmonth, 1);
                    var DaysInMonth = DateTime.DaysInMonth(getyear, mon);
                    //previousenddate = new DateTime(getyear, mon, DaysInMonth);
                }
                else
                {
                    var getyear = Convert.ToInt32((now.Year)) - 1;
                    previousstart = getyear + "-" + 12 + "-01";
                    //previousstartdate = Convert.ToDateTime(previousstart);
                    var mon = Convert.ToDateTime(previousstartdate).Month;
                    var startOfMonth = new DateTime(getyear, getmonth, 1);
                    var DaysInMonth = DateTime.DaysInMonth(getyear, mon);
                    //previousenddate = new DateTime(getyear, mon, DaysInMonth);
                }

                try
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fo_Emp_Log_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = startDate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar) { Value = endDate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar) { Value = "LIEO" });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.lateindata = retObject1.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fo_Emp_Log_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = previousstartdate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar) { Value = previousenddate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar) { Value = "LIEO" });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.lateindataprevious = retObject1.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fo_Emp_Log_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = startDate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar) { Value = endDate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar) { Value = "punch" });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.punchdetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fo_Emp_Log_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = previousstartdate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar) { Value = previousenddate.ToString("yyyy-MM-dd") });
                        cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar) { Value = "punch" });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.punchdetailsprevious = retObject1.ToArray();
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

                data.totdayspresent = (from a in _IssueContext.FO_Emp_PunchDMO
                                       where (a.HRME_Id == empid && a.MI_Id == data.MI_Id && a.FOEP_PunchDate >= startDate)
                                       select new ISMDashboardDTO
                                       {
                                           FOEP_Id = a.FOEP_Id,
                                           FOEP_PunchDate = a.FOEP_PunchDate
                                       }).ToArray().Length;

                data.lastpunchdate = _IssueContext.FO_Emp_PunchDMO.Where(t => t.HRME_Id == empid && t.MI_Id == data.MI_Id && t.FOEP_PunchDate >= startDate).OrderByDescending(t => t.FOEP_PunchDate).Select(t => t.FOEP_PunchDate).FirstOrDefault();

                if (data.lastpunchdate == null)
                {
                    data.lastpunchdate = startDate;
                }

                if (leaveyear != null)
                {
                    data.totholiday = (from a in _IssueContext.HolidayWorkingDayTypeDMO
                                       from b in _IssueContext.FO_Master_HolidayWorkingDay_DatesDMO
                                       where (a.MI_Id == data.MI_Id && a.FOHTWD_HolidayWDTypeFlag == "PH" && a.FOHTWD_HolidayFlag == true && a.FOHWDT_ActiveFlg == true && b.MI_Id == a.MI_Id && b.HRMLY_Id == leaveyear.HRMLY_Id && b.FOHWDT_Id == a.FOHWDT_Id && b.FOMHWDD_FromDate >= startDate && b.FOMHWDD_FromDate <= data.lastpunchdate && b.FOMHWD_ActiveFlg == true)
                                       select new ISMDashboardDTO
                                       {
                                           FOMHWDD_Name = b.FOMHWDD_Name,
                                           FOMHWDD_FromDate = b.FOMHWDD_FromDate
                                       }).ToArray().Length;
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generation_Deviation_Calculation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = DateTime.Today.ToString("yyyy-MM-dd") });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = 1 });

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
                        data.getdeviationreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var checkdeptheadcode = _IssueContext.HR_Master_DepartmentCode_Head.Where(a => a.HRME_ID == empid).ToList();
                data.checkdeptheadcode = 0;

                if (checkdeptheadcode.Count > 0)
                {
                    data.checkdeptheadcode = 1;

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Dashboard_User_Employee_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

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
                            data.mappedemployeedetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    var employeedata = _Context.HR_Master_Employee_DMO.Where(t => t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToList();
                    var departmentdata = _Context.HR_Master_Department.Where(d => d.HRMD_Id == employeedata[0].HRMD_Id && d.HRMD_ActiveFlag == true).ToList();
                    var departmentcode = _Context.HR_Master_DepartmentCodeDMO.Where(d => d.HRMDC_ID == departmentdata[0].HRMDC_ID).ToList();

                    if (departmentcode[0].HRMDC_Code == "FCT")
                    {
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Dashboard_Tenant_TaskList";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@HRMDC_Id", SqlDbType.VarChar) { Value = departmentdata[0].HRMDC_ID });

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
                                data.tenanttasklist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Dashboard_InterDepartment_TaskList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMDC_Id", SqlDbType.VarChar) { Value = checkdeptheadcode.FirstOrDefault().HRMDC_ID });
                         cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
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
                            data.interdeptmenttasklist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //interdeptment_list
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Development_CompletedTasks_Popup";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
 ;
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
                        data.interdeptment_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                /* CHECKING WHETHER TL GIVEN APPROVAL FOR THERE TEAM IF NOT TL WILL NOT GENERATE DR  */

                if (checkdeptheadcode.Count > 0 && data.DR_NotApprovedTL == 0)
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Checking_DailyReport_Approval_GivenOrNot_TL_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = confromdate });

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
                            data.drnotapprovedmessage = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

               
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Checking_DailyReport_REApproval";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = confromdate });

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
                            data.Reapprovedmessage = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
               

                // LapTop Not Approved List By Team Lead Or Hardware Team
                if (checkdeptheadcode.Count > 0)
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Laptop_Applied_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

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
                            data.Laptopnotapprovedlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                /* CHECKING THE EMPLOYEE DAILY REPORT BLOCKED OR NOT */
                //var getdailyreportblocked = _IssueContext.ISM_Block_EmployeeDMO.Where(t => t.HRME_Id == empid
                //&& t.ISMBE_BlockFlg == true && t.ISMBE_ActiveFlg == true).ToList();
                //data.blockedcount = getdailyreportblocked.Count();


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_NonEarnedDays";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
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
                                //retObject.Add((ExpandoObject)dataRow);

                                data.blockedcount = Convert.ToInt64(dataReader["BlockedDays"].ToString());
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Blocked_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
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
                        data.blockeddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("HR", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    string ratingmonth = DateTime.Now.ToString("MMMM");
                    //Attendance
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Dashboard_Attendance_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

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
                            data.DashbaordAttendanceDetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //Planner
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Dashboard_Planner_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

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
                            data.DashbaordPlannerDetailsCount = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //Legal added by Rohan
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SP_Legal_Notice_CategoryDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.BigInt) { Value = data.flag });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt) { Value = data.ASMAY_Id });

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
                            data.DashbaordLegalDetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //Planner Approval
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Dashboard_Planner_Approved_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

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
                            data.DashbaordPlannerApprovedDetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //VMS MODULE FEEDBACK RATING added by adarsh
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Module_AvergeRating";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.VarChar) { Value = ratingmonth });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = DateTime.Now.Year.ToString() });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Overall" });
                       // cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

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
                            data.dashboardmoduleratingreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                string ratingmonth1 = DateTime.Now.ToString("MMMM");

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Training_Monitoring_IssueList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = DateTime.Now.Year.ToString() });
                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.VarChar) { Value = ratingmonth1 });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "All" });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.griddata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Bond_Completing_Employees";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.bondCompletingEmpList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Accountant", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    //Petty Cash
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Dashboard_PettyCash_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

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
                            data.DashbaordPettyCashDetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //dashbaordHRMS Salary count added by ada.ms
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ACC_Dashboard_salary_Report";
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
                            data.DashbaordHrmssalaryDetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_GRN_Report";
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
                            data.DashbaordInventoryDetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //Driver Indent Details
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "DriverIndentApproval";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                        {
                            Value = rolet.FirstOrDefault().IVRMRT_Role.ToUpper()
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject11 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {

                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
                                    }
                                    retObject11.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.DashboardDriverIndentDetailsCount = retObject11.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                //Leave
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LeaveApproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.UserId });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.DashboardLeaveDetailsCount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Purchase Indent Details
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PurchageIndent_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.DashboardPurchaseIndentaseDetailsCount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Purchase Order Details
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PurchageOrder_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.DashboardPurchaseOrderDetailsCount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Salary Advance approval
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_SalaryAdvance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                        dtoReader.GetName(iFiled),
                                        dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.getadvancesalary = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //Tada Advance approval
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_ADVANCE_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });

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
                        data.getadvancetada = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Tada Apply
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_APPLY_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });

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
                        data.DashboardTADAAppliedDetailsCount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //CHEQUEBOOK  Details
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_PAYMENT_VOCHER_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });


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
                        data.DashboardChequeBookDetailsCount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //VISITOR APPOINTMENT APPROVAL
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_VISITORS_FOR_APPROVAL_NEW";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@ROLE", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role.ToUpper() });

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
                        data.DashboardVisitorAppointmentDetailsCount = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                var checkapproval = (from a in _IssueContext.HR_PROCESSDMO
                                     from b in _IssueContext.HR_Process_Auth_OrderNoDMO
                                     where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == data.UserId && a.HRPA_TypeFlag.Equals("HW_HealthCheckUp"))
                                     select b).Distinct().ToArray();

                if (checkapproval.Length > 0)
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HW_HealthCheckup_Approval_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.VarChar) { Value = data.UserId });
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
                            data.DashboardHWHealthcheckupdetailsCount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                // Checking User Wise Completed Task Count Details
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_TADA_Details";
                    //  cmd.CommandText = "ISM_PendingCloseTask_List_DR_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });

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
                        data.CloseTaskCoutnDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //add by raj emp_details
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

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
                        data.emp_deatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_ActivityDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@HRMAR_Id ", SqlDbType.VarChar) { Value = data.HRMAR_Id });

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
                        data.QRcodeActivityDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //QR Code Generation
                var Qrcode = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
                if (Qrcode.Count > 0)
                {
                    data.HRME_Id = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                    var result = _IssueContext.MasterEmployee.Where(R => R.MI_Id == data.MI_Id && R.HRME_Id == data.HRME_Id).FirstOrDefault();
                    if (result.HRME_QRCode != "" && result.HRME_QRCode != null)
                    {
                        data.Qrflag = false;
                        data.HRME_QRCode = result.HRME_QRCode;
                    }
                    else
                    {
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {

                            cmd.CommandText = "QRCODE_DisplaynameNewupdated";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id }); cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
                            cmd.Parameters.Add(new SqlParameter("@QRCODE", SqlDbType.VarChar, Int32.MaxValue) { Direction = ParameterDirection.Output });


                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            data.HRME_QRCode = cmd.Parameters["@QRCODE"].Value.ToString();
                        }
                        if (data.HRME_QRCode != null && data.HRME_QRCode != "")
                        {
                            data.Qrflag = true;
                        }
                        else
                        {
                            data.Qrflag = false;
                        }

                    }
                }


                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Front Office Executive", StringComparison.OrdinalIgnoreCase))
                {
                    //data.visitorcheckoutdetails = _IssueContext.Visitor_Management_MasterVisitor_DMO.Where(t => t.VMMV_ActiveFlag == true && t.VMMV_CkeckedInOutStatus != "Checked Out").ToArray();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CheckOutDetails";
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
                            data.visitorcheckoutdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                //Added by Rohan

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase) && departmentdatacode.Equals("MANG", StringComparison.OrdinalIgnoreCase))
                {

                    data.pendingcount = _IssueContext.Legal_Notice_RequestDMO.Where(S => S.MI_ID == data.MI_Id && S.LNR_Status == "P" && S.LNR_ActiveFlag == true).ToArray();
                    data.generatedcount = _IssueContext.Legal_Notice_RequestDMO.Where(S => S.MI_ID == data.MI_Id && S.LNR_Status == "G" && S.LNR_ActiveFlag == true).ToArray();
                }
                //
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RoutePlan_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Hrme_id", SqlDbType.BigInt) { Value = empid });
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
                        data.fileupload = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //admindata
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_TOUR_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.admindata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //GetReport

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_FINAL_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
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
                        data.GetReport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MOM_DR_BlockIN3days";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.momdrblockreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //vendor onboarding sushma

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Inv_vendoronboardpopup";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt)
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.invvendoronboardreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




                //added by praveenkumar
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Lead_Follow_PopUp";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                        data.Leadfollowpopup = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Notpunchtoday";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.QRcode_emp_deatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Daily_UnotherisedLeave";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.unotherisedLeavedeatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_lateinearlyout";
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.lateinearlyout = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_Dashboard";
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "demolead"
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Sataewiselead = retObject.ToArray(); // existing DTO used 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_Dashboard";
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "rawdata"
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.DashbaordTenantLeft = retObject.ToArray(); // existing DTO used 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Development_CompletedTasks_Popup";
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Dev_Completedtask = retObject.ToArray(); // existing DTO used 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //Sales_Completedtask
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_negotiation_Approval_deails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
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
                        data.Sales_Completedtask = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //added By bdm
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_UpComingDays_TourDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
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
                        data.getbda_Report = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //added by ganesh

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TADA_Advance_Accounts_Popup";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt) { Value = data.MI_Id });
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
                        data.TADA_Paymentpopup = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_AdavancePayments_Account_popup";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
               SqlDbType.VarChar)
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.TADA_AdavancePayments_popup = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_RoutePayments_Account";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                 SqlDbType.BigInt)
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead 
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.TADA_AdvsalesPaymentpopup = retObject.ToArray();
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

        //public async Task<ISMDashboardDTO> getdetailsAsync(ISMDashboardDTO data)
        //{
        //    try
        //    {
        //        string[] leavecode = new string[2];
        //        leavecode[0] = "PH";
        //        leavecode[1] = "OH";

        //        var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
        //        data.Role_flag = rolet[0].IVRMRT_Role;
        //        if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
        //        {

        //            data.HRME_Id = _IssueContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
        //        }
        //        else
        //        {
        //            data.HRME_Id = 0;
        //        }
        //        if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Accountant", StringComparison.OrdinalIgnoreCase))
        //        {
        //            var Sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Sunday);
        //            var sundysecod = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);


        //            string FromDate = Sunday.ToString("yyyy-MM-dd");
        //            string ToDate = sundysecod.ToString("yyyy-MM-dd");
        //            // string FromDate = data.fromdate.ToString("yyyy-MM-dd");
        //            // string ToDate = data.todate.ToString("yyyy-MM-dd");

        //            for (int b = 0; b < 2; b++)
        //            {
        //                if (b == 0)
        //                {
        //                    data.TypeFlg = "planer";
        //                }
        //                if (b == 1)
        //                {
        //                    data.TypeFlg = "petycash";
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "IsmDashboard";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();
        //                    //@TypeFlg
        //                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
        //                    cmd.Parameters.Add(new SqlParameter("@TypeFlg", SqlDbType.VarChar) { Value = data.TypeFlg });
        //                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
        //                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        if (data.TypeFlg == "petycash")
        //                        {
        //                            data.IsmDashBoard = retObject.ToArray();
        //                        }
        //                        if (data.TypeFlg == "planer")
        //                        {
        //                            data.planerapproval = retObject.ToArray();
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }

        //            //planer approval
        //        }
        //        //Tenant data

        //        if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        //        {
        //            //Petty Cash
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "TenantsDashboardData";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.DashbaordTenantDetailsCount = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }




        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "ISM_PendingIssues";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
        //                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = data.UserId });
        //                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
        //                cmd.Parameters.Add(new SqlParameter("@HRMPR_Id", SqlDbType.BigInt) { Value = 0 });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.prioritylist = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }


        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "GratuityCalculation";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
        //                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Graduitydetails = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

        //            var leaveyear = _IssueContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_FromDate <= DateTime.Today && t.HRMLY_ToDate >= DateTime.Today && t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).ToArray().FirstOrDefault();

        //            long empid = 0;
        //            if (hrmEempid.Length > 0)
        //            {
        //                empid = hrmEempid.FirstOrDefault().Emp_Code;
        //            }

        //            var projectcount = (from p in _IssueContext.MastersProject_DMO
        //                                from t in _IssueContext.ISM_TaskCreationDMO
        //                                where (p.ISMMPR_Id == t.ISMMPR_Id && t.ISMTCR_ActiveFlg == true && p.ISMMPR_ActiveFlg == true && p.MI_Id == t.MI_Id && t.MI_Id == data.MI_Id && t.HRME_Id == empid)
        //                                select new ISMDashboardDTO
        //                                {
        //                                    ISMMPR_Id = p.ISMMPR_Id,
        //                                    ISMMPR_ProjectName = p.ISMMPR_ProjectName
        //                                }).Distinct().ToList();

        //            var opencount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid && t.ISMTCR_Status == "Open").ToList();
        //            var closedcount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid && t.ISMTCR_Status == "Closed").ToList();
        //            var completedcount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid && t.ISMTCR_Status == "Completed").ToList();
        //            var inprogresscount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid && t.ISMTCR_Status == "Inprogress").ToList();
        //            var taskcount = _IssueContext.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_ActiveFlg == true && t.HRME_Id == empid).ToList();

        //            data.projectCount = projectcount.Count;
        //            data.taskCount = taskcount.Count;
        //            data.openCount = opencount.Count;
        //            data.closedCount = closedcount.Count;
        //            data.completedCount = completedcount.Count;
        //            data.inprogressCount = inprogresscount.Count;

        //            data.employeedetails = (from a in _IssueContext.MasterEmployee
        //                                    from b in _IssueContext.HR_Master_Department
        //                                    from c in _IssueContext.HR_Master_Designation
        //                                    where (a.MI_Id == data.MI_Id && a.HRME_Id == empid && b.HRMD_Id == a.HRMD_Id && c.HRMDES_Id == a.HRMDES_Id)
        //                                    select new ISMDashboardDTO
        //                                    {
        //                                        employeename = (a.HRME_EmployeeFirstName + " " + a.HRME_EmployeeMiddleName + " " + a.HRME_EmployeeLastName).Trim(),
        //                                        HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
        //                                        HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
        //                                        HRME_EmployeeLastName = a.HRME_EmployeeLastName,
        //                                        HRMD_Id = a.HRMD_Id,
        //                                        HRMD_DepartmentName = b.HRMD_DepartmentName,
        //                                        HRMDES_Id = a.HRMDES_Id,
        //                                        HRMDES_DesignationName = c.HRMDES_DesignationName,
        //                                        HRME_Photo = a.HRME_Photo,
        //                                        HRME_DOB = a.HRME_DOB,
        //                                        DeviceID = a.HRME_AppDownloadedDeviceId,
        //                                        HRME_Id = a.HRME_Id
        //                                    }).ToArray();

        //            //data.birthdaylist = _IssueContext.MasterEmployee.Where(t => t.HRME_DOB.Value.Day == DateTime.Today.Day && t.HRME_DOB.Value.Month == DateTime.Today.Month && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();

        //            data.birthdaylist = (from t in _IssueContext.MasterEmployee
        //                                 from b in _IssueContext.HR_Master_Designation
        //                                 where (b.HRMDES_Id == t.HRMDES_Id && t.HRME_DOB.Value.Day == DateTime.Today.Day && t.HRME_DOB.Value.Month == DateTime.Today.Month && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && b.MI_Id == t.MI_Id)
        //                                 select new ISMDashboardDTO
        //                                 {
        //                                     HRME_EmployeeFirstName = t.HRME_EmployeeFirstName,
        //                                     HRME_EmployeeMiddleName = t.HRME_EmployeeMiddleName,
        //                                     HRME_EmployeeLastName = t.HRME_EmployeeLastName,
        //                                     HRMDES_DesignationName = b.HRMDES_DesignationName,
        //                                     HRME_Photo = t.HRME_Photo,
        //                                 }).ToArray();
        //            if (leaveyear != null)
        //            {
        //                data.leavedetails = (from a in _IssueContext.HR_Emp_Leave_StatusDMO
        //                                     from b in _IssueContext.HR_Master_Leave_DMO
        //                                     where (a.HRME_Id == empid && a.MI_Id == data.MI_Id && a.HRMLY_Id == leaveyear.HRMLY_Id && a.HRML_Id == b.HRML_Id && b.MI_Id == data.MI_Id && b.HRML_LeaveCode == "CL")
        //                                     select new ISMDashboardDTO
        //                                     {
        //                                         HRELS_TotalLeaves = a.HRELS_TotalLeaves,
        //                                         HRELS_CBLeaves = a.HRELS_CBLeaves
        //                                     }).ToArray();

        //                data.upcomingholiday = (from a in _IssueContext.HolidayWorkingDayTypeDMO
        //                                        from b in _IssueContext.FO_Master_HolidayWorkingDay_DatesDMO
        //                                        where (a.MI_Id == data.MI_Id && leavecode.Contains(a.FOHTWD_HolidayWDTypeFlag) && a.FOHWDT_ActiveFlg == true && b.MI_Id == a.MI_Id && b.HRMLY_Id == leaveyear.HRMLY_Id && b.FOHWDT_Id == a.FOHWDT_Id && b.FOMHWDD_FromDate > DateTime.Today && b.FOMHWDD_Name != "Sunday" && b.FOMHWD_ActiveFlg == true)
        //                                        select new ISMDashboardDTO
        //                                        {
        //                                            FOMHWDD_Name = b.FOMHWDD_Name,
        //                                            FOHTWD_HolidayWDType = a.FOHTWD_HolidayWDTypeFlag,
        //                                            FOMHWDD_FromDate = b.FOMHWDD_FromDate
        //                                        }).OrderBy(b => b.FOMHWDD_FromDate).ToArray();
        //            }

        //            DateTime now = DateTime.Now;
        //            var startDate = new DateTime(now.Year, now.Month, 1);
        //            var endDate = DateTime.Today;

        //            // DateTime previousstartdate = DateTime.Now;
        //            //DateTime previousenddate = DateTime.Now;
        //            string previousstart = "";
        //            //var getmonth = now.Month-1 ;
        //            var getmonth = DateTime.Now.AddMonths(-1).Month;
        //            var today = DateTime.Today;
        //            var month = new DateTime(today.Year, today.Month, 1);
        //            var previousstartdate = month.AddMonths(-1);
        //            var previousenddate = month.AddDays(-1);
        //            if (getmonth >= 1)
        //            {
        //                getmonth = Convert.ToInt32(getmonth);
        //                var getyear = now.Year;
        //                previousstart = getyear + "-" + getmonth + "-01";
        //                // previousstartdate = Convert.ToDateTime(previousstart);
        //                var mon = Convert.ToDateTime(previousstartdate).Month;
        //                var startOfMonth = new DateTime(getyear, getmonth, 1);
        //                var DaysInMonth = DateTime.DaysInMonth(getyear, mon);
        //                //previousenddate = new DateTime(getyear, mon, DaysInMonth);
        //            }
        //            else
        //            {
        //                var getyear = Convert.ToInt32((now.Year)) - 1;
        //                previousstart = getyear + "-" + 12 + "-01";
        //                //previousstartdate = Convert.ToDateTime(previousstart);
        //                var mon = Convert.ToDateTime(previousstartdate).Month;
        //                var startOfMonth = new DateTime(getyear, getmonth, 1);
        //                var DaysInMonth = DateTime.DaysInMonth(getyear, mon);
        //                //previousenddate = new DateTime(getyear, mon, DaysInMonth);
        //            }

        //            try
        //            {
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "Fo_Emp_Log_Report";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = startDate.ToString("yyyy-MM-dd") });
        //                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar) { Value = endDate.ToString("yyyy-MM-dd") });
        //                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar) { Value = empid });
        //                    cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar) { Value = "LIEO" });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();
        //                    try
        //                    {
        //                        var retObject1 = new List<dynamic>();
        //                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
        //                        {
        //                            while (await dataReader1.ReadAsync())
        //                            {
        //                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
        //                                {
        //                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
        //                                }
        //                                retObject1.Add((ExpandoObject)dataRow1);
        //                            }
        //                        }
        //                        data.lateindata = retObject1.ToArray();

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }

        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "Fo_Emp_Log_Report";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = previousstartdate.ToString("yyyy-MM-dd") });
        //                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar) { Value = previousenddate.ToString("yyyy-MM-dd") });
        //                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar) { Value = empid });
        //                    cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar) { Value = "LIEO" });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();
        //                    try
        //                    {
        //                        var retObject1 = new List<dynamic>();
        //                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
        //                        {
        //                            while (await dataReader1.ReadAsync())
        //                            {
        //                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
        //                                {
        //                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
        //                                }
        //                                retObject1.Add((ExpandoObject)dataRow1);
        //                            }
        //                        }
        //                        data.lateindataprevious = retObject1.ToArray();

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }

        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "Fo_Emp_Log_Report";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = startDate.ToString("yyyy-MM-dd") });
        //                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar) { Value = endDate.ToString("yyyy-MM-dd") });
        //                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar) { Value = empid });
        //                    cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar) { Value = "punch" });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();
        //                    try
        //                    {
        //                        var retObject1 = new List<dynamic>();
        //                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
        //                        {
        //                            while (await dataReader1.ReadAsync())
        //                            {
        //                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
        //                                {
        //                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
        //                                }
        //                                retObject1.Add((ExpandoObject)dataRow1);
        //                            }
        //                        }
        //                        data.punchdetails = retObject1.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }

        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "Fo_Emp_Log_Report";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = "" });
        //                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = previousstartdate.ToString("yyyy-MM-dd") });
        //                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar) { Value = previousenddate.ToString("yyyy-MM-dd") });
        //                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar) { Value = empid });
        //                    cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar) { Value = "punch" });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();
        //                    try
        //                    {
        //                        var retObject1 = new List<dynamic>();
        //                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
        //                        {
        //                            while (await dataReader1.ReadAsync())
        //                            {
        //                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
        //                                {
        //                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
        //                                }
        //                                retObject1.Add((ExpandoObject)dataRow1);
        //                            }
        //                        }
        //                        data.punchdetailsprevious = retObject1.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }
        //            catch (Exception ee)
        //            {
        //                Console.WriteLine(ee.Message);
        //            }

        //            data.totdayspresent = (from a in _IssueContext.FO_Emp_PunchDMO
        //                                   where (a.HRME_Id == empid && a.MI_Id == data.MI_Id && a.FOEP_PunchDate >= startDate)
        //                                   select new ISMDashboardDTO
        //                                   {
        //                                       FOEP_Id = a.FOEP_Id,
        //                                       FOEP_PunchDate = a.FOEP_PunchDate
        //                                   }).ToArray().Length;

        //            data.lastpunchdate = _IssueContext.FO_Emp_PunchDMO.Where(t => t.HRME_Id == empid && t.MI_Id == data.MI_Id && t.FOEP_PunchDate >= startDate).OrderByDescending(t => t.FOEP_PunchDate).Select(t => t.FOEP_PunchDate).FirstOrDefault();

        //            if (data.lastpunchdate == null)
        //            {
        //                data.lastpunchdate = startDate;
        //            }

        //            if (leaveyear != null)
        //            {
        //                data.totholiday = (from a in _IssueContext.HolidayWorkingDayTypeDMO
        //                                   from b in _IssueContext.FO_Master_HolidayWorkingDay_DatesDMO
        //                                   where (a.MI_Id == data.MI_Id && a.FOHTWD_HolidayWDTypeFlag == "PH" && a.FOHTWD_HolidayFlag == true && a.FOHWDT_ActiveFlg == true && b.MI_Id == a.MI_Id && b.HRMLY_Id == leaveyear.HRMLY_Id && b.FOHWDT_Id == a.FOHWDT_Id && b.FOMHWDD_FromDate >= startDate && b.FOMHWDD_FromDate <= data.lastpunchdate && b.FOMHWD_ActiveFlg == true)
        //                                   select new ISMDashboardDTO
        //                                   {
        //                                       FOMHWDD_Name = b.FOMHWDD_Name,
        //                                       FOMHWDD_FromDate = b.FOMHWDD_FromDate
        //                                   }).ToArray().Length;
        //            }

        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "ISM_DailyReport_Generation_Deviation_Calculation";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
        //                cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = DateTime.Today.ToString("yyyy-MM-dd") });
        //                cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = 1 });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.getdeviationreport = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            var checkdeptheadcode = _IssueContext.HR_Master_DepartmentCode_Head.Where(a => a.HRME_ID == empid).ToList();
        //            data.checkdeptheadcode = 0;

        //            if (checkdeptheadcode.Count > 0)
        //            {
        //                data.checkdeptheadcode = 1;

        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "ISM_Dashboard_User_Employee_Details";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
        //                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.mappedemployeedetails = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }

        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "ISM_Dashboard_InterDepartment_TaskList";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@HRMDC_Id", SqlDbType.VarChar) { Value = checkdeptheadcode.FirstOrDefault().HRMDC_ID });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.interdeptmenttasklist = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }

        //            DateTime fromdatecon = DateTime.Now;
        //            DateTime toatecon = DateTime.Now;
        //            string confromdate = "";
        //            try
        //            {
        //                fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
        //                confromdate = fromdatecon.ToString("yyyy-MM-dd");
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }

        //            /* CHECKING WHETHER TL GIVEN APPROVAL FOR THERE TEAM IF NOT TL WILL NOT GENERATE DR  */

        //            if (checkdeptheadcode.Count > 0 && data.DR_NotApprovedTL == 0)
        //            {
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "ISM_Checking_DailyReport_Approval_GivenOrNot_TL_Modify";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
        //                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
        //                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar) { Value = confromdate });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.drnotapprovedmessage = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }

        //            // LapTop Not Approved List By Team Lead Or Hardware Team
        //            if (checkdeptheadcode.Count > 0)
        //            {
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "ISM_Laptop_Applied_Details";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.Laptopnotapprovedlist = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }

        //            /* CHECKING THE EMPLOYEE DAILY REPORT BLOCKED OR NOT */
        //            var getdailyreportblocked = _IssueContext.ISM_Block_EmployeeDMO.Where(t => t.HRME_Id == empid
        //            && t.ISMBE_BlockFlg == true && t.ISMBE_ActiveFlg == true).ToList();
        //            data.blockedcount = getdailyreportblocked.Count();
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "ISM_DailyReport_Blocked_Details";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
        //                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empid });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.blockeddetails = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }


        //            if (rolet.FirstOrDefault().IVRMRT_Role.Equals("HR", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        //            {
        //                //Attendance
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "ISM_Dashboard_Attendance_Details";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
        //                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.DashbaordAttendanceDetailsCount = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }

        //                //Planner
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "ISM_Dashboard_Planner_Details";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
        //                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.DashbaordPlannerDetailsCount = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }

        //                //Planner Approval
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "ISM_Dashboard_Planner_Approved_Details";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
        //                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.DashbaordPlannerApprovedDetailsCount = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }

        //            if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Accountant", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        //            {
        //                //Petty Cash
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "ISM_Dashboard_PettyCash_Details";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
        //                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.DashbaordPettyCashDetailsCount = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }

        //                //Driver Indent Details
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "DriverIndentApproval";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
        //                    {
        //                        Value = rolet.FirstOrDefault().IVRMRT_Role.ToUpper()
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();
        //                    try
        //                    {
        //                        var retObject11 = new List<dynamic>();
        //                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
        //                        {
        //                            while (await dataReader1.ReadAsync())
        //                            {

        //                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
        //                                {
        //                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]);
        //                                }
        //                                retObject11.Add((ExpandoObject)dataRow1);
        //                            }
        //                        }
        //                        data.DashboardDriverIndentDetailsCount = retObject11.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }

        //            //Leave
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "VMS_LeaveApproval";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.UserId });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = await cmd.ExecuteReaderAsync())
        //                    {
        //                        while (await dataReader.ReadAsync())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.DashboardLeaveDetailsCount = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            //Purchase Indent Details
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "INV_PurchageIndent_Approval";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.DashboardPurchaseIndentaseDetailsCount = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            //Purchase Order Details
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "INV_PurchageOrder_Approval";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.DashboardPurchaseOrderDetailsCount = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            //Tada Advance approval
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "TADA_ADVANCE_APPROVAL";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.getadvancetada = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            //Tada Apply
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "TADA_APPLY_APPROVAL";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.DashboardTADAAppliedDetailsCount = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            //CHEQUEBOOK  Details
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "VMS_PAYMENT_VOCHER_APPROVAL";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });


        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.DashboardChequeBookDetailsCount = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            //VISITOR APPOINTMENT APPROVAL
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "GET_VISITORS_FOR_APPROVAL_NEW";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = data.UserId });
        //                cmd.Parameters.Add(new SqlParameter("@ROLE", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role.ToUpper() });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.DashboardVisitorAppointmentDetailsCount = retObject.ToArray();
        //                }

        //                catch (Exception ex)
        //                {
        //                    Console.Write(ex.Message);
        //                }
        //            }

        //            var checkapproval = (from a in _IssueContext.HR_PROCESSDMO
        //                                 from b in _IssueContext.HR_Process_Auth_OrderNoDMO
        //                                 where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == data.UserId && a.HRPA_TypeFlag.Equals("HW_HealthCheckUp"))
        //                                 select b).Distinct().ToArray();

        //            if (checkapproval.Length > 0)
        //            {
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "HW_HealthCheckup_Approval_Details";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.VarChar) { Value = data.UserId });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.DashboardHWHealthcheckupdetailsCount = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }

        //            // Checking User Wise Completed Task Count Details
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "ISM_PendingCloseTask_List_DR_Dashboard_Details";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
        //                cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.CloseTaskCoutnDetails = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            //add by raj emp_details
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "Employee_details";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
        //                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.emp_deatils = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "QRcode_ActivityDetails";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.UserId });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.QRcodeActivityDetails = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            //QR Code Generation
        //            var Qrcode = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
        //            if (Qrcode.Count > 0)
        //            {
        //                data.HRME_Id = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
        //                var result = _IssueContext.MasterEmployee.Where(R => R.MI_Id == data.MI_Id && R.HRME_Id == data.HRME_Id).FirstOrDefault();
        //                if (result.HRME_QRCode != "" && result.HRME_QRCode != null)
        //                {
        //                    data.Qrflag = false;
        //                    data.HRME_QRCode = result.HRME_QRCode;
        //                }
        //                else
        //                {
        //                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                    {

        //                        cmd.CommandText = "QRCODE_DisplaynameNewupdated";
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id }); cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
        //                        cmd.Parameters.Add(new SqlParameter("@QRCODE", SqlDbType.VarChar, Int32.MaxValue) { Direction = ParameterDirection.Output });


        //                        if (cmd.Connection.State != ConnectionState.Open)
        //                            cmd.Connection.Open();

        //                        var data1 = cmd.ExecuteNonQuery();

        //                        data.HRME_QRCode = cmd.Parameters["@QRCODE"].Value.ToString();
        //                    }
        //                    if (data.HRME_QRCode != null && data.HRME_QRCode != "")
        //                    {
        //                        data.Qrflag = true;
        //                    }
        //                    else
        //                    {
        //                        data.Qrflag = false;
        //                    }

        //                }
        //            }


        //            if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Front Office Executive", StringComparison.OrdinalIgnoreCase))
        //            {
        //                //data.visitorcheckoutdetails = _IssueContext.Visitor_Management_MasterVisitor_DMO.Where(t => t.VMMV_ActiveFlag == true && t.VMMV_CkeckedInOutStatus != "Checked Out").ToArray();
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "CheckOutDetails";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.visitorcheckoutdetails = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        public async Task<ISMDashboardDTO> getlateearlyoutreport(ISMDashboardDTO data)
        {
            try
            {
                DateTime now = data.fromdate;
                var startDate1 = new DateTime(now.Year, now.Month, 1);
                var endDate = data.todate;

                if (data.flag == "Month")
                {
                    string startdate = data.yearid + "-" + data.month + "-01";
                    data.fromdate = Convert.ToDateTime(startdate);
                    var year = Convert.ToDateTime(startdate).Year;
                    var mon = Convert.ToDateTime(startdate).Month;
                    var startOfMonth = new DateTime(year, mon, 1);
                    var DaysInMonth = DateTime.DaysInMonth(year, mon);
                    data.todate = new DateTime(year, mon, DaysInMonth);

                    startDate1 = data.fromdate;
                    endDate = data.todate;
                }

                try
                {


                    if (data.TypeFlg == "RCLeave")
                    {

                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "HR_ReconsiderLeave_Details";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt)
                            {
                                Value = data.yearid
                            });
                            cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt)
                            {
                                Value = data.month
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                            {
                                Value = data.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                            {
                                Value = startDate1.ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                            {
                                Value = endDate.ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                            {
                                Value = data.flag
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
                                data.griddata = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fo_Emp_Log_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                        {
                            Value = startDate1.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar)
                        {
                            Value = endDate.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar)
                        {
                            Value = "LIEO"
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.lateindata = retObject1.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fo_Emp_Log_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                        {
                            Value = startDate1.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                        {
                            Value = endDate.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.NVarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar)
                        {
                            Value = "punch"
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.punchdetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "FO_GPS_PunchDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                        {
                            Value = startDate1.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                        {
                            Value = endDate.ToString("yyyy-MM-dd")
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.admindata = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Emp_PunchMissing_Approval";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                        {
                            Value = data.UserId
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.punchdetailsprevious = retObject1.ToArray();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISMDashboardDTO ViewTenantDetails(ISMDashboardDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();


                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Staff", StringComparison.OrdinalIgnoreCase) || rolet.FirstOrDefault().IVRMRT_Role.Equals("Accountant", StringComparison.OrdinalIgnoreCase))
                {
                    //Petty Cash
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Tenant_Viewdetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.BigInt) { Value = 1 });

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
                            data.DashbaordTenantTotalAmountPaid = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Tenant_Viewdetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.BigInt) { Value = 2 });

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
                            data.DashbaordTenantBalanceAmount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Tenant_Viewdetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.BigInt) { Value = 3 });

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
                            data.DashbaordTenantJoined = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Tenant_Viewdetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.BigInt) { Value = 4 });

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
                            data.DashbaordTenantLeft = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Tenant_Viewdetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolet.FirstOrDefault().IVRMRT_Role });
                        cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.BigInt) { Value = 5 });

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
                            data.DashbaordTenantagreementend = retObject.ToArray();
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
        public ISMDashboardDTO getmodule(ISMDashboardDTO data)
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
        public ISMDashboardDTO loaddataMDDASHBOARD(ISMDashboardDTO data)
        {
            try
            {
                List<MastersProject_DMO> staf_types = new List<MastersProject_DMO>();
                staf_types = _IssueContext.MastersProject_DMO.Where(t => t.ISMMPR_ActiveFlg == true).ToList();
                data.projectcount = staf_types.Distinct().ToArray();

                List<ISM_TaskCreationDMO> opentask = new List<ISM_TaskCreationDMO>();
                opentask = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_Status == "Open").ToList();
                data.totalopentask = opentask.Distinct().ToArray();

                List<ISM_TaskCreationDMO> inprogress = new List<ISM_TaskCreationDMO>();
                inprogress = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_Status == "ReOpen").ToList();
                data.totalinprogresstask = inprogress.Distinct().ToArray();

                List<ISM_TaskCreationDMO> completed = new List<ISM_TaskCreationDMO>();
                completed = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_Status == "Completed").ToList();
                data.totalpendingtask = completed.Distinct().ToArray();

                data.birthdaylist = (from a in _IssueContext.MasterEmployee
                                     from b in _IssueContext.HR_Master_Department
                                     from c in _IssueContext.Multiple_Mobile_DMO
                                     where (b.HRMD_Id == a.HRMD_Id && a.HRME_Id == c.HRME_Id && a.HRME_DOB.Value.Day == DateTime.Today.Day && a.HRME_DOB.Value.Month == DateTime.Today.Month && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new ISMDashboardDTO
                                     {
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName,
                                         HRME_Photo = a.HRME_Photo,
                                         HRMEMNO_MobileNo = c.HRMEMNO_MobileNo,
                                         emailid = a.HRME_EmailId,
                                         HRME_Id = a.HRME_Id
                                     }).Distinct().ToArray();

                //data.birthdaylist = _IssueContext.MasterEmployee.Where(t => t.HRME_DOB.Value.Day == DateTime.Today.Day && t.HRME_DOB.Value.Month == DateTime.Today.Month && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();

                DateTime today = DateTime.Now;
                DateTime enddate = today.AddDays(5);

                data.eventlist = (from a in _IssueContext.COE_Master_EventsDMO
                                  from b in _IssueContext.COE_EventsDMO
                                  where (b.COEE_EStartDate >= today.Date && b.COEE_EStartDate <= enddate && a.COEME_Id == b.COEME_Id && b.COEE_ActiveFlag == true)
                                  select a.COEME_EventName).Distinct().ToArray();

                data.blocklist = (from a in _IssueContext.ISM_Block_EmployeeDMO
                                  from b in _IssueContext.MasterEmployee
                                  where (a.HRME_Id == b.HRME_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && a.ISMBE_BlockFlg == true)
                                  select new ISMDashboardDTO
                                  {
                                      HRME_EmployeeFirstName = b.HRME_EmployeeFirstName,

                                  }).Distinct().ToArray();

                data.totapplied = (from a in _IssueContext.HR_Master_Leave_DMO
                                   from b in _IssueContext.HR_Emp_Leave_Trans_DMO
                                   where (b.HRELT_FromDate.Month == DateTime.Today.Month && b.HRELT_ToDate.Month == DateTime.Today.Month && b.HRELT_FromDate.Year == DateTime.Today.Year && b.HRELT_ActiveFlag == true && a.HRML_Id == b.HRELT_LeaveId && a.HRML_LeaveType != "LWP")
                                   select b.HRELT_Id).ToArray();

                data.totapproved = (from a in _IssueContext.HR_Master_Leave_DMO
                                    from b in _IssueContext.HR_Emp_Leave_Trans_DMO
                                    where (b.HRELT_FromDate.Month == DateTime.Today.Month && b.HRELT_ToDate.Month == DateTime.Today.Month && b.HRELT_FromDate.Year == DateTime.Today.Year && b.HRELT_ActiveFlag == true && a.HRML_Id == b.HRELT_LeaveId && a.HRML_LeaveType != "LWP" && b.HRELT_Status == "Approved")
                                    select b.HRELT_Id).ToArray();

                data.totrejected = (from a in _IssueContext.HR_Master_Leave_DMO
                                    from b in _IssueContext.HR_Emp_Leave_Trans_DMO
                                    where (b.HRELT_FromDate.Month == DateTime.Today.Month && b.HRELT_ToDate.Month == DateTime.Today.Month && b.HRELT_FromDate.Year == DateTime.Today.Year && b.HRELT_ActiveFlag == true && a.HRML_Id == b.HRELT_LeaveId && a.HRML_LeaveType != "LWP" && b.HRELT_Status == "Rejected")
                                    select b.HRELT_Id).ToArray();

                data.totpending = (from a in _IssueContext.HR_Master_Leave_DMO
                                   from b in _IssueContext.HR_Emp_Leave_Trans_DMO
                                   where (b.HRELT_FromDate.Month == DateTime.Today.Month && b.HRELT_ToDate.Month == DateTime.Today.Month && b.HRELT_FromDate.Year == DateTime.Today.Year && b.HRELT_ActiveFlag == true && a.HRML_Id == b.HRELT_LeaveId && a.HRML_LeaveType != "LWP" && b.HRELT_Status != "Rejected" && b.HRELT_Status != "Approved")
                                   select b.HRELT_Id).ToArray();

                data.totalemployees = _IssueContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Emp_Monthly_yearly_Report_VMS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@fromdate",
             SqlDbType.VarChar)
                    {
                        Value = today
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
             SqlDbType.VarChar)
                    {
                        Value = today
                    });

                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
             SqlDbType.VarChar)
                    {
                        Value = 0
                    });

                    cmd.Parameters.Add(new SqlParameter("@miid",
            SqlDbType.VarChar)
                    {
                        Value = 0
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
           SqlDbType.VarChar)
                    {
                        Value = "absent"
                    });

                    cmd.Parameters.Add(new SqlParameter("@cols",
          SqlDbType.VarChar)
                    {
                        Value = ""
                    });

                    cmd.Parameters.Add(new SqlParameter("@totalpresent",
          SqlDbType.VarChar)
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getabsentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "departmentWise_Dev_Report_Dashboard";
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getdeviationdepartmentwise = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PlannerReport_Dashboard";
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.plannernotgeneratedreport = retObject.ToArray();
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
        public ISMDashboardDTO onclickdata(ISMDashboardDTO data)
        {
            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_Dashboard_Details";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@currentdate", SqlDbType.VarChar)
                {
                    Value = data.date
                });
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                {
                    Value = data.UserId
                });
                cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                {
                    Value = data.type
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
                    data.admindata = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
        public ISMDashboardDTO pendingissues(ISMDashboardDTO dto)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _IssueContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }

                if (dto.HRMPR_Id == 12345)
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "prioritynamepivot";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@UserId",
                 SqlDbType.BigInt)
                        {
                            Value = dto.UserId
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.HRME_Id
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
                            dto.prioritypivot = retObject.ToArray();
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
                        cmd.CommandText = "ISM_PendingIssues";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@UserId",
                 SqlDbType.BigInt)
                        {
                            Value = dto.UserId
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMPR_Id",
                 SqlDbType.BigInt)
                        {
                            Value = dto.HRMPR_Id
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
                            dto.issueslist = retObject.ToArray();
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
            return dto;
        }
        public ISMDashboardDTO pendingissuesnotification(ISMDashboardDTO dto)
        {
            try
            {
                DateTime sDate = DateTime.Now.Date;
                var year = sDate.Year;
                var yearid = _IssueContext.HR_Master_LeaveYearDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRMLY_LeaveYear == year.ToString()).ToList();
                var ldate = (from a in _IssueContext.HolidayWorkingDayTypeDMO
                             from b in _IssueContext.FO_Master_HolidayWorkingDay_DatesDMO
                             where a.MI_Id == dto.MI_Id && b.MI_Id == dto.MI_Id && a.FOHTWD_HolidayFlag == false
                             && b.HRMLY_Id == yearid[0].HRMLY_Id && b.FOMHWDD_FromDate == sDate && a.FOHTWD_HolidayFlag == false
                             && a.FOHWDT_ActiveFlg == true
                             select new ISMDashboardDTO
                             {
                                 FOMHWDD_FromDate = b.FOMHWDD_FromDate
                             }).Distinct().ToList();


                if (ldate.Count > 0)
                {
                    var empid = _IssueContext.MasterEmployee.Where(a => a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false).ToList();
                    foreach (var item in empid)
                    {
                        var empid1 = (from a in _IssueContext.MasterEmployee
                                      from b in _IssueContext.Multiple_Email_DMO
                                      where a.HRME_Id == b.HRME_Id && a.HRME_Id == item.HRME_Id && a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                      select new ISMDashboardDTO
                                      {
                                          emailid = b.HRMEM_EmailId
                                      }).FirstOrDefault();

                        int rownumber = 0;
                        string str = "";
                        string str1 = "";
                        string str2 = "";
                        dto.message = "";
                        List<ISMDashboardDTO> dtonewtemp = new List<ISMDashboardDTO>();
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_PendingIssues";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                            {
                                Value = dto.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@UserId",
                     SqlDbType.BigInt)
                            {
                                Value = 0
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                            {
                                Value = item.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRMPR_Id",
                     SqlDbType.BigInt)
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
                                        dtonewtemp.Add(new ISMDashboardDTO
                                        {
                                            MI_Name = (dataReader["MI_Name"].ToString()),
                                            HRMP_Name = (dataReader["HRMP_Name"].ToString()),
                                            totalcount = Convert.ToInt64(dataReader["totalcount"]),


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

                            // var stdate = Convert.ToDateTime(data.ISMSLECOM_Date).ToString("dd/MM/yyyy");

                            str += "<p> <span style=" + "'font-weight:bold'" + "><span style=" + "'float: center;padding-left:10%;'" + ">" + "</span> </span></p> <table width='130%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Issues From</th>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Priority</th>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;''" + " >  Total Task </th>" +

                                     "</tr>";
                            for (int j = 0; j < dtonewtemp.Count; j++)
                            {
                                rownumber += 1;
                                str1 += "<tr> " +
                                    "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + dtonewtemp[j].MI_Name + "</td>" +
                                    "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + dtonewtemp[j].HRMP_Name + "</td>" +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].totalcount + "</td> " +




                                     "</tr>";
                            }
                            str2 = "</table>";

                            dto.message = str + str1 + str2;
                            var Template = "PriorityPending";

                            sendmail(item.HRME_Id, empid1.emailid, item.MI_Id, Template, dto.message);
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
        public ISMDashboardDTO pendingissuesnotification_admin(ISMDashboardDTO dto)
        {
            try
            {


                int rownumber = 0;
                string str = "";
                string str1 = "";
                string str2 = "";
                dto.message = "";
                List<ISMDashboardDTO> dtonewtemp = new List<ISMDashboardDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "prioritynamepivot";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
             SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.BigInt)
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
                                dtonewtemp.Add(new ISMDashboardDTO
                                {
                                    DepartmentName = (dataReader["DepartmentName"].ToString()),
                                    Blocker = (dataReader["Blocker"].ToString()),
                                    Critical = (dataReader["Critical"].ToString()),
                                    Major = (dataReader["Major"].ToString()),
                                    Minor = (dataReader["Minor"].ToString()),


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

                    // var stdate = Convert.ToDateTime(data.ISMSLECOM_Date).ToString("dd/MM/yyyy");

                    str += "<p> <span style=" + "'font-weight:bold'" + "><span style=" + "'float: center;padding-left:10%;'" + ">" + "</span> </span></p> <table width='130%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +

                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Department Name</th>" +
                          "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Blocker</th>" +
                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;''" + " > Critical </th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;''" + " > Major </th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;''" + " > Minor </th>" +

                             "</tr>";
                    for (int j = 0; j < dtonewtemp.Count; j++)
                    {
                        rownumber += 1;
                        str1 += "<tr> " +
                            "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + dtonewtemp[j].DepartmentName + "</td>" +
                            "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + dtonewtemp[j].Blocker + "</td>" +
                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].Critical + "</td> " +
                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].Major + "</td> " +
                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].Minor + "</td> " +




                             "</tr>";
                    }
                    str2 = "</table>";

                    dto.message = str + str1 + str2;
                    var Template = "PriorityPending_admin";
                    long hrme = 0;
                    string email = "pstomd@vapstech.com";
                    sendmail(hrme, email, dto.MI_Id, Template, dto.message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public ISMDashboardDTO ViewEmployeeIssueList(ISMDashboardDTO dto)
        {
            try
            {
                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == dto.HRME_Id).Distinct().ToArray();

                string departmentid = "0";
                List<long?> department_ids = new List<long?>();

                if (hrmeDepartment_id.Length > 0)
                {
                    foreach (var ei in hrmeDepartment_id)
                    {
                        departmentid = departmentid + "," + ei.HRMD_Id;
                        department_ids.Add(ei.HRMD_Id);
                    }
                }

                string statuscode = "0";

                var statuslist = _IssueContext.MasterStatus_DMO.Where(t => t.MI_Id == dto.MI_Id && department_ids.Contains(t.HRMD_Id)
                && t.ISMMISTS_ActiveFlag == true).Distinct().OrderBy(t => t.ISMMISTS_Order).ToArray();

                dto.get_statuslist = statuslist;

                foreach (var c in statuslist)
                {
                    statuscode = statuscode + "," + c.ISMMISTS_StatusName;
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Dashboard_Employee_Issue_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = dto.UserId });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = dto.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@statuscode", SqlDbType.VarChar) { Value = statuscode });

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
                        dto.mappedemployeeissuedetails = retObject.ToArray();
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
        public ISMDashboardDTO OnClickViewHrPlannerDetails(ISMDashboardDTO data)
        {
            try
            {
                var empid_details = _IssueContext.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();

                if (empid_details.Count > 0)
                {
                    var empid = empid_details.FirstOrDefault().Emp_Code;


                    var empdetails = (from a in _IssueContext.MasterEmployee
                                      where (a.HRME_Id == empid)
                                      select new ISMDashboardDTO
                                      {
                                          employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) +
                                          (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                          (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName))

                                      }).Distinct().ToList();

                    data.employeename = empdetails.FirstOrDefault().employeename;

                    DateTime fromdatecon = DateTime.Now;
                    string confromdate = "";
                    try
                    {
                        fromdatecon = Convert.ToDateTime(data.ISMDRPT_Date.Value.Date.ToString("yyyy-MM-dd"));
                        confromdate = fromdatecon.ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_HR_PlannerManagment_Details_DR";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = data.UserId });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });
                        cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });

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
                            data.hrremindersplannerdetails = retObject.ToArray();
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
        public ISMDashboardDTO QrCodeAPIUpdate(ISMDashboardDTO data)
        {
            try
            {
                data.HRME_Id = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.HRME_Id > 0)
                {
                    var updates = _IssueContext.MasterEmployee.Where(R => R.HRME_Id == data.HRME_Id).FirstOrDefault();
                    string st = data.HRME_QRCode;
                    st = st.Insert(0, "<");
                    int stlength;
                    stlength = st.Length;
                    st += st.Insert(stlength, " width=" + "100" + " height=" + "100" + " />");

                    updates.HRME_QRCode = st;
                    _IssueContext.Update(updates);
                    int i = _IssueContext.SaveChanges();
                    if (i > 0)
                    {
                        var MasterEmployee = _IssueContext.MasterEmployee.Where(R => R.HRME_Id == data.HRME_Id).FirstOrDefault();
                        if (MasterEmployee.HRME_QRCode != "")
                        {
                            data.HRME_QRCode = MasterEmployee.HRME_QRCode;
                        }
                    }
                    else
                    {

                    }

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISMDashboardDTO QRcode_Emp_profile(ISMDashboardDTO data)
        {
            try
            {

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_QRcodeEmployeedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });

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
                        data.QRcode_emp_deatils = retObject.ToArray();
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
        public ISMDashboardDTO updatevisitorscheckoutlist(ISMDashboardDTO data)
        {
            try
            {

                foreach (var vm in data.visitorcheckoutlist)
                {
                    var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("updateCheckout @p0,@p1", vm.VMMV_Id, vm.VMMV_Remarks);
                    if (contactExistsP > 0)
                    {
                        data.message = "updated";
                    }
                    else
                    {
                        data.message = "notUpdated";
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISMDashboardDTO savetrainingRemarklist(ISMDashboardDTO data)
        {
            try
            {

                foreach (var vm in data.trainigProcessIssueR)
                {
                    var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("VMS_TrainingMonitorHR_Update @p0,@p1", vm.TMO_Id, vm.TMO_Remark);
                    if (contactExistsP > 0)
                    {
                        data.message = "Saved";
                    }
                    else
                    {
                        data.message = "NotSaved";
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public string sendmail(long HRME_Id, string EmailId, long MI_Id, string Template, string message1)
        {
            try
            {

                string mailcc = "";
                string mailbcc = "";


                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _IssueContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _IssueContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;


                string result = "";
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_Detailse_Fetching";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = HRME_Id
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



                result = Mailmsg.Replace("[MSG]", message1);
                Mailmsg = result;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
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
                    smstpdetails = _IssueContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
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
                        message.AddTo(EmailId);



                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                if (c != EmailId)
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
                                if (bc != EmailId)
                                {
                                    message.AddBcc(bc);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();

                        ivrm_email_sentbox es = new ivrm_email_sentbox();
                        es.MI_Id = MI_Id;
                        es.Email_Id = EmailId;
                        es.Message = Mailmsg;
                        es.Datetime = DateTime.Today;
                        es.Module_Name = "Issue Manager";
                        es.CreatedDate = DateTime.Today;
                        es.UpdatedDate = DateTime.Today;
                        es.To_FLag = "Staff";
                        _IssueContext.Add(es);
                        _IssueContext.SaveChanges();

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
                                emailMessage.To.Add(new MailAddress(EmailId));
                                emailMessage.From = new MailAddress(SendingEmail, institutionName[0].MI_Name);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;




                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != EmailId)
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
                                        if (bc != EmailId)
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

        public ISMDashboardDTO getdatedetails(ISMDashboardDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())

                {
                    cmd.CommandText = "HR_DeptHeadWise_AbsentPresent_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User",
                      SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@PunchDate",
                     SqlDbType.DateTime)
                    {
                        Value = data.FOEP_PunchDate
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
                        data.GetReport = retObject.ToArray();
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


        public ISMDashboardDTO absentPresent(ISMDashboardDTO data)
        {
            try
            {
                var deptheadlist = _IssueContext.HR_Master_DepartmentCode_Head.ToArray();
                for (var i = 0; i < deptheadlist.Length; i++)
                {
                    var UserID = _IssueContext.Staff_User_Login.Where(t => t.Emp_Code == deptheadlist[i].HRME_ID).Select(t => t.Id).FirstOrDefault();


                    string email = "";
                    email = _IssueContext.Multiple_Email_DMO.Where(t => t.HRME_Id == deptheadlist[i].HRME_ID && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    List<ISMDashboardDTO> presentabsentgridvalue = new List<ISMDashboardDTO>();
                    //  Array[] presentabsentgridvalue = new Array[10000];


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_DeptHeadWise_AbsentPresent_List";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@User",
                          SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@PunchDate",
                         SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
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
                                    presentabsentgridvalue.Add(new ISMDashboardDTO
                                    {


                                        HRME_EmployeeCode = Convert.ToInt64(dataReader["HRME_EmployeeCode"]),
                                        HRME_EmployeeFirstName = Convert.ToString(dataReader["HRME_EmployeeFirstName"]),
                                        Status = Convert.ToString(dataReader["Status"]),

                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                    }


                    string PresentAbsentList = "";
                    int rownumber1 = 0;
                    if (presentabsentgridvalue.Count() > 0)
                    {
                        PresentAbsentList += "<table width=" + "'600px'" + " style=" + "'text-align: center;line-height:2;border:0;cellspacing:0;cellpadding: 0;background-color:#F5F5F5'" + ">" +
                    "<tr>";
                        PresentAbsentList += "<tr>";
                        PresentAbsentList += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'> " + "SL.NO." + "</th>";
                        PresentAbsentList += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Employee Code" + "</th>";
                        PresentAbsentList += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Employee Name" + "</th>";
                        PresentAbsentList += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Status" + "</th>";
                        PresentAbsentList += "</tr>";
                        for (int j = 0; j < presentabsentgridvalue.Count(); j++)
                        {
                            rownumber1 += 1;
                            PresentAbsentList += "<tr>" +
                            "<td align=" + "center" + " style=" + ">" + rownumber1 + "</td> " +
                            "<td align=" + "left" + ">" + presentabsentgridvalue[j].HRME_EmployeeCode + "</td>" +
                            "<td align=" + "left" + ">" + presentabsentgridvalue[j].HRME_EmployeeFirstName + "</td>" +
                            "<td align=" + "left" + ">" + presentabsentgridvalue[j].Status + "</td> " +
                            "</tr>";
                        }
                        PresentAbsentList += "</table>";
                    }

                    if (email.Count() > 0)
                    {
                        sendmail(data.MI_Id, email, PresentAbsentList, "PresentAbsentList");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string sendmail(long MI_Id, string Email, string PresentAbsentList, string Template)
        {
            try
            {
                //Dictionary<string, string> val = new Dictionary<string, string>();
                //var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                //if (template.Count == 0)
                //{
                //    return "Email Template not Mapped to the selected Institution";
                //}
                //var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                //var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                //var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                //string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                //List<Match> variables = new List<Match>();

                //foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                //{
                //    variables.Add(match);
                //}

                //Mailmsg = Mailmsg.Replace("[LOGO]", institutionName[0].MI_Logo);
                //Mailmsg = Mailmsg.Replace("[Level]", sactionlevel.ToString());
                //Mailmsg = Mailmsg.Replace("[CompanyName]", institutionName[0].MI_Name);
                //Mailmsg = Mailmsg.Replace("[YEAR]", Year);
                //Mailmsg = Mailmsg.Replace("[MONTH]", month);
                //Mailmsg = Mailmsg.Replace("[NAME]", nextapprovar);
                //Mailmsg = Mailmsg.Replace("[SALARYHOLD]", salaryhold);
                //Mailmsg = Mailmsg.Replace("[SALARYAPPROVED]", salaryapproved);


                //DateTime date = DateTime.Now;
                //DateTime date1 = DateTime.Parse(date.ToString());
                //Mailmsg = Mailmsg.Replace("[DATE]", date1.ToString("dd/MM/yyyy"));



                //List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                //alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                //string Attechement = "";
                //if (alldetails.Count > 0)
                //{
                //    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                //    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                //    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                //    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                //    string Subject = template[0].ISES_MailSubject.ToString();
                //    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                //    string mailcc = "";
                //    string mailbcc = "";
                //    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                //    {
                //        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                //        mailcc = ccmail[0].ToString();

                //        if (ccmail.Length > 1)
                //        {
                //            if (ccmail[1] != null || ccmail[1] != "")
                //            {
                //                mailbcc = "";
                //            }
                //        }

                //    }
                //    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                //    {
                //        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                //    }

                //    var message = new SendGridMessage();
                //    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                //    message.Subject = Subject;
                //    message.AddTo(Email);

                //    if (Attechement.Equals("1"))
                //    {
                //        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                //        if (img.Count > 0)
                //        {
                //            for (int i = 0; i < img.Count; i++)
                //            {
                //                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                //                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                //                Stream stream = response.GetResponseStream();
                //                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                //            }
                //        }
                //    }
                //    if (Emailcc != null && Emailcc != "")
                //    {
                //        message.AddCc(Emailcc);
                //    }

                //    //if (mailcc != null && mailcc != "")
                //    //{
                //    //    message.AddCc(mailcc);
                //    //}
                //    if (mailbcc != null && mailbcc != "")
                //    {
                //        message.AddBcc(mailbcc);
                //    }
                //    message.HtmlContent = Mailmsg;
                //    var client = new SendGridClient(sengridkey);
                //    client.SendEmailAsync(message).Wait();
                //    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                //    {
                //        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                //        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                //        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                //        cmd.CommandText = "IVRM_Email_Outgoing";
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add(new SqlParameter("@EmailId",
                //            SqlDbType.NVarChar)
                //        {
                //            Value = Email
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@Message",
                //           SqlDbType.NVarChar)
                //        {
                //            Value = Mailmsg
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@module",
                //        SqlDbType.VarChar)
                //        {
                //            Value = modulename[0]
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //       SqlDbType.BigInt)
                //        {
                //            Value = MI_Id
                //        });

                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        try
                //        {
                //            using (var dataReader = cmd.ExecuteReader())
                //            {
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            return ex.Message;
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        //Praveen
        public ISMDashboardDTO getemployeedetail(ISMDashboardDTO data)
        {
            try
            {
                long HRMDC_Id_New = 0;
                string Department_Code = "0";
                if (data.TypeFlg == "Onload" && data.Flags == "DeActive")
                {
                    string Dep_Code = "0";
                    List<ISMDashboardDTO> Tasklist = new List<ISMDashboardDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_AssetTag_CheckOut_ActiveDeactive_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRMDC_Id", SqlDbType.VarChar) { Value = 0 });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Deactive_Onload" });
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
                                    Tasklist.Add(new ISMDashboardDTO
                                    {
                                        Department_Code = Convert.ToString(dataReader["HRMDC_Code"]),


                                    });
                                }
                            }
                            //  data.employeedetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (Tasklist != null && Tasklist.Count > 0)
                    {
                        Department_Code = Tasklist[0].Department_Code;
                        foreach (var d in Tasklist)
                        {
                            Dep_Code = Dep_Code + ',' + d.Department_Code;
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Department_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "3" });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = Dep_Code });
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
                            data.departmentname = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                }
                else
                {

                    Department_Code = data.Department_Code;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_AssetTag_CheckOut_ActiveDeactive_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRMDC_Id", SqlDbType.VarChar) { Value = Department_Code });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.Flags });
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
                        data.employeedetails = retObject.ToArray();
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


        public ISMDashboardDTO StatewiseleadDetails(ISMDashboardDTO data)
        {
            try
            {

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StatewiseLeadList_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id", SqlDbType.BigInt) { Value = data.IVRMMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.flag });
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
                        data.Sataewiselead = retObject.ToArray();
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

        public ISMDashboardDTO employeetasklist(ISMDashboardDTO data)
        {
            try
            {
                var emphrme = _IssueContext.Staff_User_Login.Where(h => h.Id == data.UserId && h.MI_Id == data.MI_Id).Select(t => t.Emp_Code).FirstOrDefault(); ;

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Employee_PendingIssues";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = emphrme });
                    cmd.Parameters.Add(new SqlParameter("@ISMTCRASTO_AssignedBy", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMPR_Id", SqlDbType.BigInt) { Value = data.HRMPR_Id });
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
                        data.prioritylist = retObject.ToArray();
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

        //dinner

        public ISMDashboardDTO orderedinner(ISMDashboardDTO data)
        {
            try
            {
                var datetime = DateTime.Now;

                TimeSpan time2 = TimeSpan.Parse("14:00:00");
                if (datetime.TimeOfDay < time2)
                {
                    var emphrme = _IssueContext.Staff_User_Login.Where(h => h.Id == data.UserId && h.MI_Id == data.MI_Id).FirstOrDefault();

                    var employeedata = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == emphrme.Emp_Code && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).FirstOrDefault();




                    var outputval = _Context.Database.ExecuteSqlCommand("VMS_AfterSeven_Employee_Work_Save @p0,@p1,@p2", employeedata.HRME_Id, datetime, data.UserId);

                    if (outputval > 0)
                    {

                        data.message = "Success";

                    }
                    else
                    {
                        data.message = "Exist";

                    }

                }
                else
                {
                    data.message = "Fail";


                }



            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return data;
        }


        public ISMDashboardDTO dinnerorderlist(ISMDashboardDTO data)
        {

            try
            {

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_AfterSeven_Employee_Details_Display";
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
                        data.dinnerorderlist = retObject.ToArray();

                        if (data.dinnerorderlist.Length > 0)
                        {
                            data.message = "list";
                        }
                        else
                        {
                            data.message = "nodata";
                        }

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

    }
}

