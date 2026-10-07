using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
using System.Net.Mail;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_PlannerApprovalImpl : Interfaces.ISM_PlannerApprovalInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_PlannerApprovalImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
        public async Task<ISM_PlannerCreationDTO> getdetails(ISM_PlannerCreationDTO data)
        {
            //  try
            //  {
            //      var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
            //      long empid = 0;
            //      if (hrmEempid.Length > 0)
            //      {
            //          empid = hrmEempid.FirstOrDefault().Emp_Code;
            //      }
            //      var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
            //      var roletype = "";
            //      if (rolet.Count > 0)
            //      {
            //          roletype = rolet.FirstOrDefault().IVRMRT_Role;
            //      }
            //      else
            //      {
            //          roletype = "";
            //      }
            //      //*********************************** Month List ********************************// 
            //      data.monthlist = _IssueContext.IVRM_Month_DMO.Where(m => m.Is_Active == true).ToArray();
            //      //*********************************** User Employee Mapped List ********************************// 
            //      using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
            //      {
            //          cmd.CommandText = "ISM_User_EmployeeList";
            //          cmd.CommandType = CommandType.StoredProcedure;

            //          cmd.Parameters.Add(new SqlParameter("@user_Id",
            //   SqlDbType.BigInt)
            //          {
            //              Value = data.UserId
            //          });
            //          cmd.Parameters.Add(new SqlParameter("@MI_Id",
            //SqlDbType.BigInt)
            //          {
            //              Value = data.MI_Id
            //          });
            //          if (cmd.Connection.State != ConnectionState.Open)
            //              cmd.Connection.Open();

            //          var retObject = new List<dynamic>();
            //          try
            //          {
            //              using (var dataReader = await cmd.ExecuteReaderAsync())
            //              {
            //                  while (await dataReader.ReadAsync())
            //                  {
            //                      var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                      for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                      {
            //                          dataRow.Add(
            //                              dataReader.GetName(iFiled),
            //                             dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
            //                          );
            //                      }
            //                      retObject.Add((ExpandoObject)dataRow);
            //                  }
            //              }
            //              data.get_userEmplist = retObject.ToArray();
            //          }
            //          catch (Exception ex)
            //          {
            //              Console.WriteLine(ex.Message);
            //          }
            //      }
            //  }
            //  catch (Exception ex)
            //  {
            //      Console.WriteLine(ex.Message);
            //  }

            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                List<ISM_PlannerCreationDTO> dtonewtemp1 = new List<ISM_PlannerCreationDTO>();
                //*********************************** Month List ********************************// 
                data.monthlist = _IssueContext.IVRM_Month_DMO.Where(m => m.Is_Active == true).ToArray();
                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@user_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
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
                                dtonewtemp1.Add(new ISM_PlannerCreationDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"])

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                var hrmEempidnew = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empidnew = 0;
                if (hrmEempidnew.Length > 0)
                {
                    empidnew = hrmEempidnew.FirstOrDefault().Emp_Code;
                }
                var roletn = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletypen = "";
                if (roletn.Count > 0)
                {
                    roletypen = roletn.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletypen = "";
                }
                string hrmeids = "0";
                if (dtonewtemp1.Count() > 0)
                {
                    foreach (var ue in dtonewtemp1)
                    {
                        if (ue.HRME_Id != empid)
                        {
                            hrmeids = hrmeids + "," + ue.HRME_Id;
                        }
                    }
                    hrmeids = hrmeids.Substring(2);
                }
                //else
                //{
                //    if (hrmeids == "" || hrmeids == "0")
                //    {
                //        hrmeids = empid.ToString();
                //    }
                //}

                //**************************************** Pending  Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Peding_Planner_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannerlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //**************************************** Max Date Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_MaxDate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannermax = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //**************************************** Approved Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Approved_Planner_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_updatedplannerlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //****************************************Daily Report Approval Pending *************************//
                DateTime fromdatecon = DateTime.Now;
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
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

                //*********************************** Completed tasks created by me task Count ************************************************//   

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PendingCloseTask_List_DR_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE",
            SqlDbType.VarChar)
                    {
                        Value = DateTime.Now
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.completedcreatedbyme = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //*********************************** Leave Approval Is Pending ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LeaveApproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_leaveDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //*********************************** Certificate Approval Is Pending ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_CertificateRequest_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.UserId)
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
                        data.Cerfificatelist = retObject.ToArray();
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
        public async Task<ISM_PlannerCreationDTO> getplannerlist(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                List<ISM_PlannerCreationDTO> dtonewtemp1 = new List<ISM_PlannerCreationDTO>();
                //*********************************** Month List ********************************// 
                data.monthlist = _IssueContext.IVRM_Month_DMO.Where(m => m.Is_Active == true).ToArray();
                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@user_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
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
                                dtonewtemp1.Add(new ISM_PlannerCreationDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"])

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                var hrmEempidnew = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empidnew = 0;
                if (hrmEempidnew.Length > 0)
                {
                    empidnew = hrmEempidnew.FirstOrDefault().Emp_Code;
                }
                var roletn = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletypen = "";
                if (roletn.Count > 0)
                {
                    roletypen = roletn.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletypen = "";
                }
                string hrmeids = "0";
                if (dtonewtemp1.Count() > 0)
                {
                    foreach (var ue in dtonewtemp1)
                    {
                        hrmeids = hrmeids + "," + ue.HRME_Id;
                    }
                    hrmeids = hrmeids.Substring(2);
                }
                else
                {
                    if (hrmeids == "" || hrmeids == "0")
                    {
                        hrmeids = empid.ToString();
                    }
                }

                //**************************************** Pending  Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Peding_Planner_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannerlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //**************************************** Max Date Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_MaxDate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannermax = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //**************************************** Approved Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Approved_Planner_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_updatedplannerlist = retObject.ToArray();
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
        public async Task<ISM_PlannerCreationDTO> getplannerdetails(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;

                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    if (data.view == 1)
                    {
                        cmd.CommandText = "ISM_Planner_TaskDeatils";
                    }
                    else
                    {
                        cmd.CommandText = "ISM_Planner_TaskDeatils_view2";
                    }

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
          SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
            SqlDbType.VarChar)
                    {
                        Value = data.ISMTPL_Id
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannerdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_TaskDeatils_view2_Tasks";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.ISMTPL_Id
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannerdetails_daily = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_Category";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTPL_Id
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannercategory = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_ApprovalStatus_Deatils";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.BigInt)
                    {
                        Value = empid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTPL_Id
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_plannerApprovaldetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //******************************* Calculate Effort *****************************//

                DateTime datetimennow = DateTime.Now;
                datetimennow = Convert.ToDateTime(datetimennow.Date.ToString("yyyy-MM-dd"));
                long lyear_id = 0;
                var yearid = _IssueContext.HR_Master_LeaveYearDMO.Where(l => l.MI_Id == data.MI_Id && l.HRMLY_ActiveFlag == true && (l.HRMLY_FromDate <= datetimennow && l.HRMLY_ToDate >= datetimennow)).Distinct().ToList();

                var dep_id = _IssueContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;

                if (yearid.Count > 0)
                {
                    lyear_id = yearid.FirstOrDefault().HRMLY_Id;
                }

                var planner = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.ISMTPL_Id == data.ISMTPL_Id).ToList();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_bwDates_Planner";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id",
          SqlDbType.BigInt)
                    {
                        Value = lyear_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@startDate",
             SqlDbType.DateTime)
                    {
                        Value = planner.FirstOrDefault().ISMTPL_StartDate
                    });

                    cmd.Parameters.Add(new SqlParameter("@endDate",
            SqlDbType.DateTime)
                    {
                        Value = planner.FirstOrDefault().ISMTPL_EndDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_effortdetails = retObject.ToArray();
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
        public async Task<ISM_PlannerCreationDTO> getextratask(ISM_PlannerCreationDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }

                var hrmEempid = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.ISMTPL_Id == data.ISMTPL_Id).Distinct().ToArray();

                //var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().HRME_Id;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PlannerApproval_ExtraTask";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = empid
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_extratasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //     using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //     {
                //         cmd.CommandText = "ISM_Pending_AssignedBy_To_Details";
                //         cmd.CommandType = CommandType.StoredProcedure;

                //         cmd.Parameters.Add(new SqlParameter("@RoleFlg",
                //SqlDbType.VarChar)
                //         {
                //             Value = rolet.FirstOrDefault().IVRMRT_Role
                //         });
                //         cmd.Parameters.Add(new SqlParameter("@countflag",
                //SqlDbType.VarChar)
                //         {
                //             Value = "NotAssign"
                //         });

                //         cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                // SqlDbType.VarChar)
                //         {
                //             Value = empid
                //         });
                //         cmd.Parameters.Add(new SqlParameter("@userid",
                // SqlDbType.VarChar)
                //         {
                //             Value = data.UserId
                //         });

                //         if (cmd.Connection.State != ConnectionState.Open)
                //             cmd.Connection.Open();

                //         var retObject = new List<dynamic>();
                //         try
                //         {
                //             using (var dataReader = await cmd.ExecuteReaderAsync())
                //             {
                //                 while (await dataReader.ReadAsync())
                //                 {
                //                     var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                     for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                     {
                //                         dataRow.Add(
                //                             dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                         );
                //                     }
                //                     retObject.Add((ExpandoObject)dataRow);
                //                 }
                //             }
                //             data.get_extratasklist = retObject.ToArray();
                //         }
                //         catch (Exception ex)
                //         {
                //             Console.WriteLine(ex.Message);
                //         }
                //     }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data)
        {
            try
            {

                decimal totalhrs = 0;
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                if (data.addExtraTaskArray != null)
                {
                    foreach (var pa in data.addExtraTaskArray)
                    {
                        pa.ISMTPLTA_ExtraFlg = true;
                        totalhrs += pa.ISMTPLTA_EffortInHrs;
                        var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == pa.ISMTCR_Id
                          && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();
                        if (updateplannertask.Count > 0)
                        {
                            var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == pa.ISMTCR_Id
                            && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true);

                            result.ISMTPLTA_Status = pa.ISMTPLTA_Status;
                            result.ISMTPLTA_Remarks = pa.ISMTPLTA_Remarks;
                            result.UpdatedDate = DateTime.UtcNow;
                            result.ISMTPLTA_UpdatedBy = data.UserId;
                            _IssueContext.Update(result);
                        }
                        else
                        {
                            ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();
                            dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                            dmo_task.ISMTCR_Id = pa.ISMTCR_Id;
                            dmo_task.ISMTPLTA_Remarks = pa.ISMTPLTA_Remarks;
                            dmo_task.ISMTPLTA_StartDate = pa.ISMTPLTA_StartDate;
                            dmo_task.ISMTPLTA_EndDate = pa.ISMTPLTA_EndDate;
                            dmo_task.ISMTPLTA_EffortInHrs = pa.ISMTPLTA_EffortInHrs;
                            dmo_task.ISMTPLTA_Status = "Open";
                            dmo_task.ISMTPLTA_DeviationFlg = false;
                            dmo_task.ISMTPLTA_ExtraFlg = pa.ISMTPLTA_ExtraFlg;
                            dmo_task.ISMTPLTA_TimeTakenInHrs = pa.ISMTPLTA_TimeTakenInHrs;
                            dmo_task.ISMTPLTA_ActiveFlg = true;
                            dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                            dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                            dmo_task.CreatedDate = DateTime.UtcNow;
                            dmo_task.UpdatedDate = DateTime.UtcNow;
                            _IssueContext.Add(dmo_task);
                        }
                    }

                    var plannerresultlist = _IssueContext.ISM_Task_PlannerDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMTPL_Id == data.ISMTPL_Id).ToList();
                    decimal totalhrslist = plannerresultlist.FirstOrDefault().ISMTPL_TotalHrs;
                    totalhrslist = totalhrslist + totalhrs;

                    var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.MI_Id == data.MI_Id && a.ISMTPL_Id == data.ISMTPL_Id);
                    plannerresult.ISMTPL_TotalHrs = totalhrslist;
                    _IssueContext.Update(plannerresult);
                }

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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_PlannerCreationDTO updateplannerStatus(ISM_PlannerCreationDTO data)
        {
            long empid = 0;
            long plannedby_id = 0;
            long counttrue = 0;
            long approvedCount = 0;
            long rejectedCount = 0;
            decimal finaleffort = 0;
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            try
            {
                //Update 
                if (data.ISMTPLAP_Id > 0)
                {
                    var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                    if (hrmEempid.Length > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                    }

                    var planned_id = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.ISMTPL_Id == data.ISMTPL_Id).Distinct().ToArray();
                    if (planned_id.Length > 0)
                    {
                        plannedby_id = planned_id.FirstOrDefault().HRME_Id;
                    }

                    var dmoresult = _IssueContext.ISM_Task_Planner_ApprovedDMO.Single(a => a.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPLAP_ActiveFlg == true);

                    dmoresult.HRME_Id = empid;
                    dmoresult.ISMTPLAP_Remarks = data.ISMTPLAP_Remarks;
                    dmoresult.UpdatedDate = indianTime;
                    dmoresult.ISMTPLAP_UpdatedBy = data.UserId;
                    dmoresult.ISMTPLAP_TotalHrs = data.totaleffort;
                    _IssueContext.Update(dmoresult);
                    decimal totalhrs = 0;

                    foreach (var planner in data.approveplannerArray)
                    {
                        data.ISMTPLAPTA_DeviationFlg = false;
                        if (planner.extraflag == 1)
                        {
                            data.ISMTPLAPTA_ExtraTaskFlg = true;
                            totalhrs += data.ISMTPLAPTA_EffortInHrs;
                        }
                        else
                        {
                            data.ISMTPLAPTA_ExtraTaskFlg = false;
                            totalhrs += 0;
                        }

                        if (planner.plannerStatus == 1)
                        {
                            data.ISMTPLAPTA_ApprovalFlg = true;
                            data.ISMTPLAPTA_RejectedFlg = false;
                        }
                        else
                        {
                            data.ISMTPLAPTA_ApprovalFlg = false;
                            data.ISMTPLAPTA_RejectedFlg = true;
                        }

                        /***************************** Planner Task Approval  **************************/

                        var updateapprovaltask = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                       && a.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPLAPTA_ActiveFlg == true).ToList();

                        if (updateapprovaltask.Count > 0)
                        {
                            //var resulttask = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                            // && a.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPLAPTA_ActiveFlg == true);

                            //resulttask.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            //resulttask.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            //resulttask.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            //resulttask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            //resulttask.ISMTPLAPTA_Status = data.ISMTPLAPTA_Status;
                            //resulttask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                            //resulttask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                            //resulttask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            //resulttask.UpdatedDate = indianTime;
                            //resulttask.ISMTPLAPTA_UpdatedBy = data.UserId;

                            var resulttask = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                            && a.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPLAPTA_ActiveFlg == true).ToArray();

                            foreach (var task in resulttask)
                            {
                                var resulttask1 = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                             && a.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPLAPTA_ActiveFlg == true && a.ISMTPLTA_Id== task.ISMTPLTA_Id);


                                resulttask1.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                resulttask1.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                                resulttask1.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                                resulttask1.ISMTPLAPTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                resulttask1.ISMTPLAPTA_Status = data.ISMTPLAPTA_Status;
                                resulttask1.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                resulttask1.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                                resulttask1.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                resulttask1.UpdatedDate = indianTime;
                                resulttask1.ISMTPLAPTA_UpdatedBy = data.UserId;

                                _IssueContext.Add(resulttask1);
                                _IssueContext.Update(resulttask1);
                            }

                            
                            if (data.ISMTPLAPTA_ApprovalFlg == true)
                            {
                                finaleffort += planner.ISMTPLAPTA_EffortInHrs;
                            }
                        }
                        else
                        {
                            ISM_Task_Planner_Approved_TasksDMO approvedtask = new ISM_Task_Planner_Approved_TasksDMO();
                            approvedtask.ISMTPLAP_Id = Convert.ToInt64(data.ISMTPLAP_Id);
                            approvedtask.ISMTCR_Id = planner.ISMTCR_Id;
                            approvedtask.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            approvedtask.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            approvedtask.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            approvedtask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            approvedtask.ISMTPLAPTA_Status = planner.ISMTPLAPTA_Status;
                            approvedtask.ISMTPLAPTA_DeviationFlg = data.ISMTPLAPTA_DeviationFlg;
                            approvedtask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                            approvedtask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                            approvedtask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            approvedtask.ISMTPLTA_Id = planner.ISMTPLTA_Id;
                            approvedtask.ISMTPLAPTA_ActiveFlg = true;

                            approvedtask.CreatedDate = indianTime;
                            approvedtask.UpdatedDate = indianTime;
                            approvedtask.ISMTPLAPTA_CreatedBy = data.UserId;
                            approvedtask.ISMTPLAPTA_UpdatedBy = data.UserId;
                            _IssueContext.Add(approvedtask);
                            //finaleffort += Convert.ToInt64(planner.ISMTPLAPTA_EffortInHrs);
                            if (data.ISMTPLAPTA_ApprovalFlg == true)
                            {
                                finaleffort += planner.ISMTPLAPTA_EffortInHrs;
                            }
                        }

                        if (data.ISMTPLAPTA_ApprovalFlg == true)
                        {
                            approvedCount = approvedCount + 1;
                        }
                        else if (data.ISMTPLAPTA_RejectedFlg == true)
                        {
                            rejectedCount = rejectedCount + 1;
                        }

                        // ************ Checking Planner Task Is Created Or Not ************ //

                        var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                        && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();

                        if (updateplannertask.Count > 0)
                        {
                            var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                            && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_Id == planner.ISMTPLTA_Id);

                            result.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            result.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            result.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            result.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            result.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            result.UpdatedDate = indianTime;
                            result.ISMTPLTA_UpdatedBy = data.UserId;

                            _IssueContext.Update(result);
                        }
                        else
                        {
                            ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                            dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                            dmo_task.ISMTCR_Id = planner.ISMTCR_Id;
                            dmo_task.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            dmo_task.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            dmo_task.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            dmo_task.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            dmo_task.ISMTPLTA_Status = planner.ISMTPLAPTA_Status;
                            dmo_task.ISMTPLTA_ExtraFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                            dmo_task.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            dmo_task.ISMTPLTA_ActiveFlg = true;
                            dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                            dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                            dmo_task.CreatedDate = indianTime;
                            dmo_task.UpdatedDate = indianTime;
                            _IssueContext.Add(dmo_task);
                        }

                        // ***************** Adding Task To Assigned Task ***************** //
                        var checktaskisassigned = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == plannedby_id
                        && a.ISMTCR_Id == planner.ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                        if (checktaskisassigned.Count() == 0)
                        {
                            ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                            dmo_task_assigned.ISMTCR_Id = data.ISMTCR_Id;
                            dmo_task_assigned.HRME_Id = plannedby_id;
                            dmo_task_assigned.ISMTCRASTO_AssignedDate = DateTime.UtcNow;
                            dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                            dmo_task_assigned.ISMTCRASTO_Remarks = planner.ISMTPLAPTA_Remarks;
                            dmo_task_assigned.ISMTCRASTO_StartDate = planner.ISMTPLAPTA_StartDate;
                            dmo_task_assigned.ISMTCRASTO_EndDate = planner.ISMTPLAPTA_EndDate;
                            dmo_task_assigned.ISMTCRASTO_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                            dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                            dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                            dmo_task_assigned.CreatedDate = indianTime;
                            dmo_task_assigned.UpdatedDate = indianTime;

                            _IssueContext.Add(dmo_task_assigned);
                        }
                    }


                    //***************** Planner Task Overall Status ***************//
                    long totalcount = 0;

                    var plannertasklist = (from a in _IssueContext.ISM_Task_Planner_ApprovedDMO
                                           from b in _IssueContext.ISM_Task_Planner_Approved_TasksDMO
                                           where (a.ISMTPLAP_Id == b.ISMTPLAP_Id && b.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPL_Id == data.ISMTPL_Id)
                                           select new ISM_PlannerCreationDTO
                                           {
                                               ISMTPL_Id = a.ISMTPL_Id,
                                               ISMTPLAPTA_Id = b.ISMTPLAPTA_Id,
                                           }).Distinct().ToList();

                    totalcount = plannertasklist.Count();

                    if (totalcount == approvedCount)
                    {
                        data.ISMTPL_ApprovalFlg = true;
                    }
                    else if (totalcount == rejectedCount)
                    {
                        data.ISMTPL_ApprovalFlg = false;
                    }
                    else
                    {
                        data.ISMTPL_ApprovalFlg = true;
                    }

                    decimal totalhrslist = 0;
                    totalhrslist = finaleffort + totalhrs;

                    var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                    plannerresult.ISMTPL_TotalHrs = totalhrslist;
                    plannerresult.ISMTPL_ApprovalFlg = data.ISMTPL_ApprovalFlg;
                    plannerresult.ISMTPL_ApprovedBy = empid;
                    plannerresult.ISMTPL_ApprovedTo = empid;
                    _IssueContext.Update(plannerresult);
                    var i = _IssueContext.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                        //****************** NOTIFICATION *********************** //   


                        var devicelist = (from a in _IssueContext.ISM_Task_PlannerDMO
                                          from b in _IssueContext.MasterEmployee
                                          where a.HRME_Id == b.HRME_Id && a.ISMTPL_Id == data.ISMTPL_Id
                                          && b.HRME_AppDownloadedDeviceId != null && b.HRME_AppDownloadedDeviceId != ""
                                          select new ISM_ComplaintsTaskListDTO
                                          {
                                              HRME_Id = b.HRME_Id,
                                              HRME_AppDownloadedDeviceId = b.HRME_AppDownloadedDeviceId
                                          }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                        if (devicelist.Count > 0)
                        {
                            string deviceidsnew = "";
                            string devicenew = "";
                            foreach (var deviceid in devicelist)
                            {

                                deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';


                                devicenew = "[" + deviceidsnew + "]";

                                try
                                {
                                    string titletext = "Planner Approved";
                                    if (data.sound == "")
                                    {
                                        data.sound = "default";
                                    }
                                    callnotification(devicenew, deviceid.HRME_Id, data.ISMTPL_Id, titletext, data.MI_Id, data.sound);
                                    Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTPL_Id, titletext, data.UserId, "PLA");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                            }
                        }
                        //add task
                        //DailyReportExtenssion
                        //var ISM_Task_Planner = _IssueContext.ISM_Task_PlannerDMO.Where(R => R.ISMTPL_Id == data.ISMTPL_Id).ToList();
                        //if (ISM_Task_Planner.Count > 0)
                        //{
                        //    if (ISM_Task_Planner.FirstOrDefault().ISMTPL_StartDate != DateTime.Now.Date)
                        //    {
                        //        List<ISM_PlannerCreationDTO> Tasklist = new List<ISM_PlannerCreationDTO>();
                        //        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        //        {
                        //            cmd.CommandText = "ISM_Task_PlannerDaily";
                        //            cmd.CommandType = CommandType.StoredProcedure;
                        //            cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
                        //              SqlDbType.VarChar)
                        //            {
                        //                Value = ISM_Task_Planner.FirstOrDefault().ISMTPL_Id
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
                        //                        Tasklist.Add(new ISM_PlannerCreationDTO
                        //                        {
                        //                            ISMDRPT_Date = Convert.ToDateTime(dataReader["AllDates"]),


                        //                        });
                        //                    }
                        //                }

                        //            }

                        //            catch (Exception ex)
                        //            {
                        //                Console.Write(ex.Message);
                        //            }
                        //        }
                        //        if (Tasklist != null && Tasklist.Count > 0)
                        //        {
                        //            for (int p = 0; p < Tasklist.Count; p++)
                        //            {
                        //                ISM_DailyReportExtensionDMO dailyreport = new ISM_DailyReportExtensionDMO();
                        //                dailyreport.HRME_Id = ISM_Task_Planner.FirstOrDefault().HRME_Id;
                        //                dailyreport.IVRM_Month_Id = Tasklist[p].ISMDRPT_Date.Value.Month;
                        //                dailyreport.ISMODE_Noofdays = 1.ToString();
                        //                dailyreport.ISMODE_Date = Tasklist[p].ISMDRPT_Date.Value;
                        //                dailyreport.ISMODE_Reason = "Auto-Extention-For-Daily Report";
                        //                dailyreport.ISMODE_ActiveFlg = true;
                        //                dailyreport.ISMODE_CreatedBy = data.UserId;
                        //                dailyreport.ISMODE_UpdatedBy = data.UserId;
                        //                dailyreport.CreatedDate = DateTime.Now;
                        //                dailyreport.UpdatedDate = DateTime.Now;
                        //                _IssueContext.Add(dailyreport);
                        //            }
                        //            _IssueContext.SaveChanges();
                        //        }


                        //    }
                        //}

                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                //----FirstTime----
                else
                {
                    var checkduplicate = _IssueContext.ISM_Task_Planner_ApprovedDMO.Where(t => t.ISMTPL_Id == data.ISMTPL_Id).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        if (data.view == 1)
                        {
                            var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                            if (hrmEempid.Length > 0)
                            {
                                empid = hrmEempid.FirstOrDefault().Emp_Code;
                            }
                            var planned_id = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.ISMTPL_Id == data.ISMTPL_Id).Distinct().ToArray();
                            if (planned_id.Length > 0)
                            {
                                plannedby_id = planned_id.FirstOrDefault().HRME_Id;
                            }
                            ISM_Task_Planner_ApprovedDMO dmo = new ISM_Task_Planner_ApprovedDMO();
                            dmo.MI_Id = data.MI_Id;
                            dmo.HRME_Id = empid;
                            dmo.ISMTPL_Id = data.ISMTPL_Id;
                            dmo.ISMTPLAP_Remarks = data.ISMTPLAP_Remarks;
                            dmo.ISMTPLAP_ActiveFlg = true;
                            dmo.CreatedDate = indianTime;
                            dmo.UpdatedDate = indianTime;
                            dmo.ISMTPLAP_TotalHrs = data.totaleffort;
                            dmo.ISMTPLAP_CreatedBy = data.UserId;
                            dmo.ISMTPLAP_UpdatedBy = data.UserId;
                            _IssueContext.Add(dmo);
                            decimal totalhrs = 0;
                            foreach (var planner in data.approveplannerArray)
                            {
                                data.ISMTPLAPTA_DeviationFlg = false;
                                if (planner.extraflag == 1)
                                {
                                    data.ISMTPLAPTA_ExtraTaskFlg = true;
                                    totalhrs += data.ISMTPLAPTA_EffortInHrs;
                                }
                                else
                                {
                                    data.ISMTPLAPTA_ExtraTaskFlg = false;
                                    totalhrs += 0;
                                }

                                if (planner.plannerStatus == 1)
                                {
                                    data.ISMTPLAPTA_ApprovalFlg = true;
                                    data.ISMTPLAPTA_RejectedFlg = false;
                                }
                                else
                                {
                                    data.ISMTPLAPTA_ApprovalFlg = false;
                                    data.ISMTPLAPTA_RejectedFlg = true;
                                }

                                /***************************** Planner Task Approval  **************************/
                                ISM_Task_Planner_Approved_TasksDMO approvedtask = new ISM_Task_Planner_Approved_TasksDMO();
                                approvedtask.ISMTPLAP_Id = dmo.ISMTPLAP_Id;
                                approvedtask.ISMTCR_Id = planner.ISMTCR_Id;
                                approvedtask.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                approvedtask.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                                approvedtask.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                                approvedtask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                approvedtask.ISMTPLAPTA_Status = planner.ISMTPLAPTA_Status;
                                approvedtask.ISMTPLAPTA_DeviationFlg = data.ISMTPLAPTA_DeviationFlg;
                                approvedtask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                approvedtask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                                approvedtask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                approvedtask.ISMTPLTA_Id = planner.ISMTPLTA_Id;
                                approvedtask.ISMTPLAPTA_ActiveFlg = true;

                                approvedtask.CreatedDate = indianTime;
                                approvedtask.UpdatedDate = indianTime;
                                approvedtask.ISMTPLAPTA_CreatedBy = data.UserId;
                                approvedtask.ISMTPLAPTA_UpdatedBy = data.UserId;

                                _IssueContext.Add(approvedtask);
                                if (data.ISMTPLAPTA_ApprovalFlg == true)
                                {
                                    finaleffort += planner.ISMTPLAPTA_EffortInHrs;
                                }
                                if (data.ISMTPLAPTA_ApprovalFlg == true)
                                {
                                    approvedCount = approvedCount + 1;
                                }
                                else if (data.ISMTPLAPTA_RejectedFlg == true)
                                {
                                    rejectedCount = rejectedCount + 1;
                                }
                                // ************ Checking Planner Task Is Created Or Not ************ //

                                var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();

                                if (updateplannertask.Count > 0)
                                {
                                    var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                    && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_Id == planner.ISMTPLTA_Id);

                                    result.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                                    result.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                                    result.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                    result.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                    result.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                    result.UpdatedDate = indianTime;
                                    result.ISMTPLTA_UpdatedBy = data.UserId;

                                    _IssueContext.Update(result);
                                }
                                else
                                {
                                    ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                                    dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                                    dmo_task.ISMTCR_Id = planner.ISMTCR_Id;
                                    dmo_task.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                    dmo_task.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                                    dmo_task.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                                    dmo_task.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                    dmo_task.ISMTPLTA_Status = planner.ISMTPLAPTA_Status;
                                    dmo_task.ISMTPLTA_ExtraFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                    dmo_task.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                    dmo_task.ISMTPLTA_ActiveFlg = true;
                                    dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                                    dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                                    dmo_task.CreatedDate = indianTime;
                                    dmo_task.UpdatedDate = indianTime;
                                    _IssueContext.Add(dmo_task);
                                }

                                // ***************** Adding Task To Assigned Task ***************** //

                                var checktaskisassigned = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == plannedby_id
                                && a.ISMTCR_Id == planner.ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                                if (checktaskisassigned.Count() == 0)
                                {
                                    ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                                    dmo_task_assigned.ISMTCR_Id = planner.ISMTCR_Id;
                                    dmo_task_assigned.HRME_Id = plannedby_id;
                                    dmo_task_assigned.ISMTCRASTO_AssignedDate = indianTime;
                                    dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                                    dmo_task_assigned.ISMTCRASTO_Remarks = planner.ISMTPLAPTA_Remarks;
                                    dmo_task_assigned.ISMTCRASTO_StartDate = planner.ISMTPLAPTA_StartDate;
                                    dmo_task_assigned.ISMTCRASTO_EndDate = planner.ISMTPLAPTA_EndDate;
                                    dmo_task_assigned.ISMTCRASTO_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                    dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                                    dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                                    dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                                    dmo_task_assigned.CreatedDate = indianTime;
                                    dmo_task_assigned.UpdatedDate = indianTime;

                                    _IssueContext.Add(dmo_task_assigned);
                                }
                            }
                            var i = _IssueContext.SaveChanges();
                            //***************** Planner Task Overall Status ***************//
                            long totalcount = 0;
                            var plannertasklist = (from a in _IssueContext.ISM_Task_Planner_ApprovedDMO
                                                   from b in _IssueContext.ISM_Task_Planner_Approved_TasksDMO
                                                   where (a.ISMTPLAP_Id == b.ISMTPLAP_Id && a.ISMTPL_Id == data.ISMTPL_Id)
                                                   select new ISM_PlannerCreationDTO
                                                   {
                                                       ISMTPL_Id = a.ISMTPL_Id,
                                                       ISMTPLAPTA_Id = b.ISMTPLAPTA_Id,
                                                   }).Distinct().ToList();

                            totalcount = plannertasklist.Count();

                            if (totalcount == approvedCount)
                            {
                                data.ISMTPL_ApprovalFlg = true;
                            }
                            else if (totalcount == rejectedCount)
                            {
                                data.ISMTPL_ApprovalFlg = false;
                            }
                            else
                            {
                                data.ISMTPL_ApprovalFlg = true;
                            }
                            decimal totalhrslist = 0;
                            totalhrslist = finaleffort + totalhrs;

                            var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                            plannerresult.ISMTPL_TotalHrs = totalhrslist;
                            plannerresult.ISMTPL_ApprovalFlg = data.ISMTPL_ApprovalFlg;
                            plannerresult.ISMTPL_ApprovedBy = empid;
                            plannerresult.ISMTPL_ApprovedTo = empid;
                            _IssueContext.Update(plannerresult);
                            var j = _IssueContext.SaveChanges();


                            if (data.ISMTPL_ApprovalFlg == false)
                            {
                                try
                                {
                                    int s = _Context.Database.ExecuteSqlCommand("AUTO_PLANNER_EXTENTION_FOR_REJECTION @p0,@p1", plannedby_id, data.ISMTPL_Id);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            };

                            if (i > 0)
                            {
                                data.returnval = true;
                                //var ISM_Task_Planner = _IssueContext.ISM_Task_PlannerDMO.Where(R => R.ISMTPL_Id == data.ISMTPL_Id).ToList();
                                //if (ISM_Task_Planner.Count > 0)
                                //{
                                //    if (ISM_Task_Planner.FirstOrDefault().ISMTPL_StartDate != DateTime.Now.Date)
                                //    {
                                //        List<ISM_PlannerCreationDTO> Tasklist = new List<ISM_PlannerCreationDTO>();
                                //        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                //        {
                                //            cmd.CommandText = "ISM_Task_PlannerDaily";
                                //            cmd.CommandType = CommandType.StoredProcedure;
                                //            cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
                                //              SqlDbType.VarChar)
                                //            {
                                //                Value = ISM_Task_Planner.FirstOrDefault().ISMTPL_Id
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
                                //                        Tasklist.Add(new ISM_PlannerCreationDTO
                                //                        {
                                //                            ISMDRPT_Date = Convert.ToDateTime(dataReader["AllDates"]),


                                //                        });
                                //                    }
                                //                }

                                //            }

                                //            catch (Exception ex)
                                //            {
                                //                Console.Write(ex.Message);
                                //            }
                                //        }
                                //        if (Tasklist != null && Tasklist.Count > 0)
                                //        {
                                //            for (int p = 0; p < Tasklist.Count; p++)
                                //            {
                                //                ISM_DailyReportExtensionDMO dailyreport = new ISM_DailyReportExtensionDMO();
                                //                dailyreport.HRME_Id = ISM_Task_Planner.FirstOrDefault().HRME_Id;
                                //                dailyreport.IVRM_Month_Id = Tasklist[p].ISMDRPT_Date.Value.Month;
                                //                dailyreport.ISMODE_Noofdays = 1.ToString();
                                //                dailyreport.ISMODE_Date = Tasklist[p].ISMDRPT_Date.Value;
                                //                dailyreport.ISMODE_Reason = "Auto-Extention-For-Daily Report";
                                //                dailyreport.ISMODE_ActiveFlg = true;
                                //                dailyreport.ISMODE_CreatedBy = data.UserId;
                                //                dailyreport.ISMODE_UpdatedBy = data.UserId;
                                //                dailyreport.CreatedDate = DateTime.Now;
                                //                dailyreport.UpdatedDate = DateTime.Now;
                                //                _IssueContext.Add(dailyreport);
                                //            }
                                //            _IssueContext.SaveChanges();
                                //        }


                                //    }
                                //}
                                //****************** EMAIL *********************** //

                                /*     string str = "";
                                     DateTime fromdatecon = DateTime.Now;
                                     TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                     DateTime toatecon = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                     List<ISM_PlannerCreationDTO> result = new List<ISM_PlannerCreationDTO>();
                                     string confromdate = "";
                                     string contodate = "";
                                     string datetime = "";
                                     try
                                     {
                                         contodate = toatecon.ToString("dd/MM/yyyy");
                                         datetime = toatecon.ToString("dd/MM/yyyy hh:mm tt");
                                     }
                                     catch (Exception ex)
                                     {
                                         Console.WriteLine(ex.Message);
                                     }

                                     List<ISM_PlannerCreationDTO> dtonewtemp = new List<ISM_PlannerCreationDTO>();

                                     using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                     {
                                         cmd.CommandText = "ISM_PlannerApproval_Search";
                                         cmd.CommandType = CommandType.StoredProcedure;

                                         cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                                         {
                                             Value = data.MI_Id
                                         });

                                         cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                                         {
                                             Value = plannedby_id
                                         });
                                         cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id", SqlDbType.VarChar)
                                         {
                                             Value = data.ISMTPL_Id
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
                                                     dtonewtemp.Add(new ISM_PlannerCreationDTO
                                                     {
                                                         ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                                         ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                                         ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                                         ISMMCLT_ClientName = Convert.ToString(dataReader["ISMMCLT_ClientName"]),
                                                         HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                                         ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                                         ISMTPLAPTA_Statusnew = Convert.ToString(dataReader["ISMTPLAPTA_Status"]),
                                                         approvedby = Convert.ToString(dataReader["approvedby"]),
                                                         ISMTPLAPTA_StartDate = Convert.ToDateTime(dataReader["ISMTPLAPTA_StartDate"]),
                                                         ISMTPLAPTA_EndDate = Convert.ToDateTime(dataReader["ISMTPLAPTA_EndDate"]),
                                                         ISMTPLAPTA_EffortInHrsnew = Convert.ToString(dataReader["ISMTPLAPTA_EffortInHrs"]),
                                                     });
                                                 }
                                             }
                                         }
                                         catch (Exception ex)
                                         {
                                             Console.WriteLine(ex.Message);
                                         }
                                     }


                                     var getemployeedetails = (from a in _IssueContext.MasterEmployee
                                                               from b in _IssueContext.HR_Master_Department
                                                               from c in _IssueContext.HR_Master_Designation
                                                               from d in _IssueContext.Multiple_Email_DMO
                                                               from e in _IssueContext.HR_Master_Grade
                                                               where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_Id == d.HRME_Id && a.HRMG_Id == e.HRMG_Id
                                                               && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && d.HRMEM_DeFaultFlag == "default"
                                                               && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
                                                               select new ISM_PlannerCreationDTO
                                                               {
                                                                   employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                                   + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                                   + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                                   departmnt = b.HRMD_DepartmentName,
                                                                   desgination = c.HRMDES_DesignationName,
                                                                   gradename = e.HRMG_GradeName,
                                                                   emailid = d.HRMEM_EmailId,
                                                                   HRMD_Id = Convert.ToInt64(a.HRMD_Id)
                                                               }).Distinct().ToList();


                                     var emaildetails = _IssueContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == getemployeedetails.FirstOrDefault().HRMD_Id).ToList();

                                     string emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                                     string emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;

                                     var checktemplate = _IssueContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Planner_Status"
                                    && a.ISES_MailActiveFlag == true).ToList();
                                     if (checktemplate.Count == 0)
                                     {
                                         data.message = "Email Template not Mapped to the selected Institution";
                                     }
                                     else
                                     {
                                         var emplname = getemployeedetails.FirstOrDefault().employeename;
                                         var deptname = getemployeedetails.FirstOrDefault().departmnt;
                                         var desginame = getemployeedetails.FirstOrDefault().desgination;
                                         var gradname = getemployeedetails.FirstOrDefault().gradename;
                                         var email = getemployeedetails.FirstOrDefault().emailid;
                                         var date = contodate;
                                         var newdate = datetime;

                                         var completeEffort = data.ISMTPL_TotalHrs;

                                         string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                                         templatedetails = templatedetails.Replace("[DeveloperName]", emplname);
                                         templatedetails = templatedetails.Replace("[DATE]", date);
                                         templatedetails = templatedetails.Replace("[Department]", deptname);
                                         templatedetails = templatedetails.Replace("[Designation]", desginame);
                                         templatedetails = templatedetails.Replace("[CompletedEffort]", completeEffort.ToString());
                                         templatedetails = templatedetails.Replace("[DATENAME]", newdate);

                                         int rownumber = 0;

                                         if (dtonewtemp.Count > 0)
                                         {

                                             for (int j = 0; j < dtonewtemp.Count; j++)
                                             {
                                                 rownumber += 1;
                                                 str += "<tr>" +
                                                     "<td align=" + "center" + " style=" + ">" + rownumber + "</td> " +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_TaskNo + "</td>" +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_Title + "</td>" +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_BugOREnhancementFlg + "</td> " +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].ISMMCLT_ClientName + "</td> " +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].HRMP_Name + "</td> " +
                                                     "<td align=" + "left" + "style=" + "word-break: break-word;" + ">" + dtonewtemp[j].ISMTCR_Desc + "</td> " +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLAPTA_Statusnew + "</td> " +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].approvedby + "</td> " +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLAPTA_StartDate + "</td> " +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLAPTA_EndDate + "</td> " +
                                                     "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLAPTA_EffortInHrsnew + "</td> " +
                                                     "</tr>";
                                             }
                                         }

                                         templatedetails = templatedetails.Replace("[MSG]", str);
                                         try
                                         {
                                             // sendmailexamtt(data.MI_Id, email, templatedetails, emailccdetails, emailbccdetails);
                                         }
                                         catch (Exception ex)
                                         {
                                             Console.WriteLine(ex.Message);
                                         }
                                     }
                                     */
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                            if (hrmEempid.Length > 0)
                            {
                                empid = hrmEempid.FirstOrDefault().Emp_Code;
                            }
                            var planned_id = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.ISMTPL_Id == data.ISMTPL_Id).Distinct().ToArray();
                            if (planned_id.Length > 0)
                            {
                                plannedby_id = planned_id.FirstOrDefault().HRME_Id;
                            }
                            ISM_Task_Planner_ApprovedDMO dmo = new ISM_Task_Planner_ApprovedDMO();
                            dmo.MI_Id = data.MI_Id;
                            dmo.HRME_Id = empid;
                            dmo.ISMTPL_Id = data.ISMTPL_Id;
                            dmo.ISMTPLAP_Remarks = data.ISMTPLAP_Remarks;
                            dmo.ISMTPLAP_ActiveFlg = true;
                            dmo.CreatedDate = indianTime;
                            dmo.UpdatedDate = indianTime;
                            dmo.ISMTPLAP_TotalHrs = data.totaleffort;
                            dmo.ISMTPLAP_CreatedBy = data.UserId;
                            dmo.ISMTPLAP_UpdatedBy = data.UserId;
                            _IssueContext.Add(dmo);
                            decimal totalhrs = 0;
                            foreach (var planner in data.approveplannerArray)
                            {

                                data.ISMTPLAPTA_DeviationFlg = false;
                                if (planner.extraflag == 1)
                                {
                                    data.ISMTPLAPTA_ExtraTaskFlg = true;
                                    totalhrs += data.ISMTPLAPTA_EffortInHrs;
                                }
                                else
                                {
                                    data.ISMTPLAPTA_ExtraTaskFlg = false;
                                    totalhrs += 0;
                                }

                                if (planner.plannerStatus == 1)
                                {
                                    data.ISMTPLAPTA_ApprovalFlg = true;
                                    data.ISMTPLAPTA_RejectedFlg = false;
                                }
                                else
                                {
                                    data.ISMTPLAPTA_ApprovalFlg = false;
                                    data.ISMTPLAPTA_RejectedFlg = true;
                                }


                                var checkmultipletasks = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                                             && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();
                                if (checkmultipletasks.Count > 0 && planner.PTSCount > 1)
                                {
                                    var neweffort = planner.ISMTPLAPTA_EffortInHrs / checkmultipletasks.Count;

                                    foreach (var newplann in checkmultipletasks)
                                    {
                                        /***************************** Planner Task Approval  **************************/
                                        ISM_Task_Planner_Approved_TasksDMO approvedtask = new ISM_Task_Planner_Approved_TasksDMO();
                                        approvedtask.ISMTPLAP_Id = dmo.ISMTPLAP_Id;
                                        approvedtask.ISMTCR_Id = planner.ISMTCR_Id;
                                        approvedtask.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                        approvedtask.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                                        approvedtask.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                                        approvedtask.ISMTPLAPTA_EffortInHrs = neweffort;
                                        approvedtask.ISMTPLAPTA_Status = planner.ISMTPLAPTA_Status;
                                        approvedtask.ISMTPLAPTA_DeviationFlg = data.ISMTPLAPTA_DeviationFlg;
                                        approvedtask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                        approvedtask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                                        approvedtask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                        approvedtask.ISMTPLTA_Id = newplann.ISMTPLTA_Id;
                                        approvedtask.ISMTPLAPTA_ActiveFlg = true;

                                        approvedtask.CreatedDate = indianTime;
                                        approvedtask.UpdatedDate = indianTime;
                                        approvedtask.ISMTPLAPTA_CreatedBy = data.UserId;
                                        approvedtask.ISMTPLAPTA_UpdatedBy = data.UserId;
                                        _IssueContext.Add(approvedtask);
                                    }
                                }
                                else
                                {
                                    /***************************** Planner Task Approval  **************************/
                                    ISM_Task_Planner_Approved_TasksDMO approvedtask = new ISM_Task_Planner_Approved_TasksDMO();
                                    approvedtask.ISMTPLAP_Id = dmo.ISMTPLAP_Id;
                                    approvedtask.ISMTCR_Id = planner.ISMTCR_Id;
                                    approvedtask.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                    approvedtask.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                                    approvedtask.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                                    approvedtask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                    approvedtask.ISMTPLAPTA_Status = planner.ISMTPLAPTA_Status;
                                    approvedtask.ISMTPLAPTA_DeviationFlg = data.ISMTPLAPTA_DeviationFlg;
                                    approvedtask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                    approvedtask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                                    approvedtask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                    approvedtask.ISMTPLTA_Id = planner.ISMTPLTA_Id;
                                    approvedtask.ISMTPLAPTA_ActiveFlg = true;

                                    approvedtask.CreatedDate = indianTime;
                                    approvedtask.UpdatedDate = indianTime;
                                    approvedtask.ISMTPLAPTA_CreatedBy = data.UserId;
                                    approvedtask.ISMTPLAPTA_UpdatedBy = data.UserId;

                                    _IssueContext.Add(approvedtask);
                                }

                                if (data.ISMTPLAPTA_ApprovalFlg == true)
                                {
                                    finaleffort += planner.ISMTPLAPTA_EffortInHrs;
                                }
                                if (data.ISMTPLAPTA_ApprovalFlg == true)
                                {
                                    approvedCount = approvedCount + 1;
                                }
                                else if (data.ISMTPLAPTA_RejectedFlg == true)
                                {
                                    rejectedCount = rejectedCount + 1;
                                }
                                // ************ Checking Planner Task Is Created Or Not ************ //

                                var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();

                                if (updateplannertask.Count > 0 && planner.PTSCount > 1)
                                {
                                    var neweffort1 = planner.ISMTPLAPTA_EffortInHrs / updateplannertask.Count;

                                    foreach (var newplann in checkmultipletasks)
                                    {
                                        var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                    && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_Id == newplann.ISMTPLTA_Id);

                                        result.ISMTPLTA_StartDate = newplann.ISMTPLTA_StartDate;
                                        result.ISMTPLTA_EndDate = newplann.ISMTPLTA_EndDate;
                                        result.ISMTPLTA_EffortInHrs = neweffort1;
                                        result.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                        result.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                        result.UpdatedDate = indianTime;
                                        result.ISMTPLTA_UpdatedBy = data.UserId;

                                        _IssueContext.Update(result);
                                    }
                                }
                                else if (updateplannertask.Count > 0 && planner.PTSCount == 1)
                                {
                                    if (planner.ISMTPLTA_Id == 0)
                                    {
                                        var checkvalue = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                    && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();
                                        if (checkvalue.Count > 0)
                                        {
                                            planner.ISMTPLTA_Id = checkvalue.FirstOrDefault().ISMTPLTA_Id;
                                        }
                                    }
                                    var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                    && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_Id == planner.ISMTPLTA_Id);

                                    result.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                                    result.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                                    result.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                    result.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                    result.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                    result.UpdatedDate = indianTime;
                                    result.ISMTPLTA_UpdatedBy = data.UserId;

                                    _IssueContext.Update(result);
                                }
                                else
                                {
                                    ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                                    dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                                    dmo_task.ISMTCR_Id = planner.ISMTCR_Id;
                                    dmo_task.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                    dmo_task.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                                    dmo_task.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                                    dmo_task.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                    dmo_task.ISMTPLTA_Status = planner.ISMTPLAPTA_Status;
                                    dmo_task.ISMTPLTA_ExtraFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                    dmo_task.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                    dmo_task.ISMTPLTA_ActiveFlg = true;
                                    dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                                    dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                                    dmo_task.CreatedDate = indianTime;
                                    dmo_task.UpdatedDate = indianTime;
                                    _IssueContext.Add(dmo_task);
                                }

                                // ***************** Adding Task To Assigned Task ***************** //

                                var checktaskisassigned = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == plannedby_id
                                && a.ISMTCR_Id == planner.ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                                if (checktaskisassigned.Count() == 0)
                                {
                                    ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                                    dmo_task_assigned.ISMTCR_Id = planner.ISMTCR_Id;
                                    dmo_task_assigned.HRME_Id = plannedby_id;
                                    dmo_task_assigned.ISMTCRASTO_AssignedDate = indianTime;
                                    dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                                    dmo_task_assigned.ISMTCRASTO_Remarks = planner.ISMTPLAPTA_Remarks;
                                    dmo_task_assigned.ISMTCRASTO_StartDate = planner.ISMTPLAPTA_StartDate;
                                    dmo_task_assigned.ISMTCRASTO_EndDate = planner.ISMTPLAPTA_EndDate;
                                    dmo_task_assigned.ISMTCRASTO_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                                    dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                                    dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                                    dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                                    dmo_task_assigned.CreatedDate = indianTime;
                                    dmo_task_assigned.UpdatedDate = indianTime;

                                    _IssueContext.Add(dmo_task_assigned);
                                }
                            }
                            var i = _IssueContext.SaveChanges();
                            //***************** Planner Task Overall Status ***************//
                            long totalcount = 0;
                            var plannertasklist = (from a in _IssueContext.ISM_Task_Planner_ApprovedDMO
                                                   from b in _IssueContext.ISM_Task_Planner_Approved_TasksDMO
                                                   where (a.ISMTPLAP_Id == b.ISMTPLAP_Id && a.ISMTPL_Id == data.ISMTPL_Id)
                                                   select new ISM_PlannerCreationDTO
                                                   {
                                                       ISMTPL_Id = a.ISMTPL_Id,
                                                       ISMTPLAPTA_Id = b.ISMTPLAPTA_Id,
                                                   }).Distinct().ToList();

                            totalcount = plannertasklist.Count();

                            if (totalcount == approvedCount)
                            {
                                data.ISMTPL_ApprovalFlg = true;
                            }
                            else if (totalcount == rejectedCount)
                            {
                                data.ISMTPL_ApprovalFlg = false;
                            }
                            else
                            {
                                data.ISMTPL_ApprovalFlg = true;
                            }
                            decimal totalhrslist = 0;
                            totalhrslist = finaleffort + totalhrs;

                            var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                            plannerresult.ISMTPL_TotalHrs = totalhrslist;
                            plannerresult.ISMTPL_ApprovalFlg = data.ISMTPL_ApprovalFlg;
                            plannerresult.ISMTPL_ApprovedBy = empid;
                            _IssueContext.Update(plannerresult);
                            var j = _IssueContext.SaveChanges();

                            if (data.ISMTPL_ApprovalFlg == false)
                            {
                                try
                                {
                                    int s = _Context.Database.ExecuteSqlCommand("AUTO_PLANNER_EXTENTION_FOR_REJECTION @p0,@p1", plannedby_id, data.ISMTPL_Id);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            };

                            if (i > 0)
                            {
                                data.returnval = true;
                                //var ISM_Task_Planner = _IssueContext.ISM_Task_PlannerDMO.Where(R => R.ISMTPL_Id == data.ISMTPL_Id).ToList();
                                //if (ISM_Task_Planner.Count > 0)
                                //{
                                //    if (ISM_Task_Planner.FirstOrDefault().ISMTPL_StartDate != DateTime.Now.Date)
                                //    {
                                //        List<ISM_PlannerCreationDTO> Tasklist = new List<ISM_PlannerCreationDTO>();
                                //        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                //        {
                                //            cmd.CommandText = "ISM_Task_PlannerDaily";
                                //            cmd.CommandType = CommandType.StoredProcedure;
                                //            cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
                                //              SqlDbType.VarChar)
                                //            {
                                //                Value = ISM_Task_Planner.FirstOrDefault().ISMTPL_Id
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
                                //                        Tasklist.Add(new ISM_PlannerCreationDTO
                                //                        {
                                //                            ISMDRPT_Date = Convert.ToDateTime(dataReader["AllDates"]),


                                //                        });
                                //                    }
                                //                }

                                //            }

                                //            catch (Exception ex)
                                //            {
                                //                Console.Write(ex.Message);
                                //            }
                                //        }
                                //        if (Tasklist != null && Tasklist.Count > 0)
                                //        {
                                //            for (int p = 0; p < Tasklist.Count; p++)
                                //            {
                                //                ISM_DailyReportExtensionDMO dailyreport = new ISM_DailyReportExtensionDMO();
                                //                dailyreport.HRME_Id = ISM_Task_Planner.FirstOrDefault().HRME_Id;
                                //                dailyreport.IVRM_Month_Id = Tasklist[p].ISMDRPT_Date.Value.Month;
                                //                dailyreport.ISMODE_Noofdays = 1.ToString();
                                //                dailyreport.ISMODE_Date = Tasklist[p].ISMDRPT_Date.Value;
                                //                dailyreport.ISMODE_Reason = "Auto-Extention-For-Daily Report";
                                //                dailyreport.ISMODE_ActiveFlg = true;
                                //                dailyreport.ISMODE_CreatedBy = data.UserId;
                                //                dailyreport.ISMODE_UpdatedBy = data.UserId;
                                //                dailyreport.CreatedDate = DateTime.Now;
                                //                dailyreport.UpdatedDate = DateTime.Now;
                                //                _IssueContext.Add(dailyreport);
                                //            }
                                //            _IssueContext.SaveChanges();
                                //        }


                                //    }
                                //}
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        //Daily Report Extenssion  
                        //ISMTPL_Id


                    }
                }
            }
            catch (Exception ex)
            {
                if (counttrue == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_PlannerCreationDTO allupdateplannerStatus(ISM_PlannerCreationDTO data)
        {
            long counttrue = 0;
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            try
            {
                List<long> Emplist = new List<long>();
                if (data.Emplist.Length > 0)
                {
                    foreach (var ue in data.Emplist)
                    {
                        var checkduplicate = _IssueContext.ISM_Task_Planner_ApprovedDMO.Where(t => t.ISMTPL_Id == ue.ISMTPL_Id).ToList();
                        if (checkduplicate.Count == 0)
                        {

                            decimal finaleffort = 0;
                            long empid = 0;
                            long plannedby_id = 0;
                            long approvedCount = 0;
                            long rejectedCount = 0;
                            var empname = "";

                            var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                            if (hrmEempid.Length > 0)
                            {
                                empid = hrmEempid.FirstOrDefault().Emp_Code;
                            }

                            var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                            if (hrmeDepartment_id.Count > 0)
                            {

                                empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                            }

                            plannedby_id = ue.HRME_Id;
                            data.ISMTPLAP_Remarks = "Approved by - " + empname;
                            data.ISMTPL_Id = ue.ISMTPL_Id;
                            data.MI_Id = ue.MI_Id;

                            ISM_Task_Planner_ApprovedDMO dmo = new ISM_Task_Planner_ApprovedDMO();
                            dmo.MI_Id = data.MI_Id;
                            dmo.HRME_Id = empid;
                            dmo.ISMTPL_Id = data.ISMTPL_Id;
                            dmo.ISMTPLAP_Remarks = data.ISMTPLAP_Remarks;
                            dmo.ISMTPLAP_ActiveFlg = true;
                            dmo.CreatedDate = indianTime;
                            dmo.UpdatedDate = indianTime;
                            dmo.ISMTPLAP_TotalHrs = ue.ISMTPL_TotalHrs;
                            dmo.ISMTPLAP_CreatedBy = data.UserId;
                            dmo.ISMTPLAP_UpdatedBy = data.UserId;
                            _IssueContext.Add(dmo);
                            decimal totalhrs = 0;

                            List<ISM_PlannerCreationDTO> dtonewtemp = new List<ISM_PlannerCreationDTO>();
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_Planner_TaskDeatils";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.VarChar)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                                {
                                    Value = ue.HRME_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
                        SqlDbType.VarChar)
                                {
                                    Value = data.ISMTPL_Id
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
                                            dtonewtemp.Add(new ISM_PlannerCreationDTO
                                            {
                                                ISMTPL_Id = Convert.ToInt64(dataReader["ISMTPL_Id"]),
                                                ISMTPLTA_Id = Convert.ToInt64(dataReader["ISMTPLTA_Id"]),
                                                ISMTCR_Id = Convert.ToInt64(dataReader["ISMTCR_Id"]),
                                                HRMPR_Id = Convert.ToInt64(dataReader["HRMPR_Id"]),
                                                HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                                // ISMTPLAP_Id = Convert.ToInt64(dataReader["ISMTPLAP_Id"]),
                                                ISMTCR_BugOREnhancement = Convert.ToString(dataReader["ISMTCR_BugOREnhancement"]),
                                                ISMTCR_CreationDate = Convert.ToDateTime(dataReader["ISMTCR_CreationDate"]),
                                                ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                                ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                                ISMTCR_Status = Convert.ToString(dataReader["ISMTCR_Status"]),
                                                ISMTCR_ReOpenFlg = Convert.ToBoolean(dataReader["ISMTCR_ReOpenFlg"]),
                                                //ISMTCR_ReOpenDate = Convert.ToDateTime(dataReader["ISMTCR_ReOpenDate"]),
                                                ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                                //ISMMCLT_Id = Convert.ToInt64(dataReader["ISMMCLT_Id"]),
                                                // ISMMCLT_ClientName = Convert.ToString(dataReader["ISMMCLT_ClientName"]),
                                                ISMTPL_CreatedBy = Convert.ToInt64(dataReader["ISMTPL_CreatedBy"]),
                                                plannerDate = Convert.ToDateTime(dataReader["plannerDate"]),
                                                ISMTPLTA_Status = Convert.ToString(dataReader["ISMTPLTA_Status"]),
                                                ISMTPL_PlannedBy = Convert.ToInt64(dataReader["ISMTPL_PlannedBy"]),
                                                ISMTPL_PlannerName = Convert.ToString(dataReader["ISMTPL_PlannerName"]),
                                                // ISMTPL_Remarks = Convert.ToString(dataReader["ISMTPL_Remarks"]),
                                                ISMTPL_StartDate = Convert.ToDateTime(dataReader["ISMTPL_StartDate"]),
                                                ISMTPL_EndDate = Convert.ToDateTime(dataReader["ISMTPL_EndDate"]),
                                                ISMTPL_TotalHrs = Convert.ToDecimal(dataReader["ISMTPL_TotalHrs"]),
                                                ISMTPL_ApprovalFlg = Convert.ToBoolean(dataReader["ISMTPL_ApprovalFlg"]),
                                                ISMTPL_ApprovedBy = Convert.ToInt64(dataReader["ISMTPL_ApprovedBy"]),
                                                ISMTPL_ActiveFlg = Convert.ToBoolean(dataReader["ISMTPL_ActiveFlg"]),
                                                ISMTPLTA_ApprovalFlg = Convert.ToBoolean(dataReader["ISMTPLTA_ApprovalFlg"]),
                                                ISMTPLTA_StartDate = Convert.ToDateTime(dataReader["ISMTPLTA_StartDate"]),
                                                ISMTPLTA_EndDate = Convert.ToDateTime(dataReader["ISMTPLTA_EndDate"]),
                                                ISMTPLTA_EffortInHrs = Convert.ToDecimal(dataReader["ISMTPLTA_EffortInHrs"]),
                                                // ISMTPLTA_Remarks = Convert.ToString(dataReader["ISMTPLTA_Remarks"]),
                                                plannedby = Convert.ToString(dataReader["plannedby"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            foreach (var planner in dtonewtemp)
                            {
                                data.ISMTPLAPTA_DeviationFlg = false;

                                data.ISMTPLAPTA_ExtraTaskFlg = false;
                                totalhrs += 0;
                                data.ISMTPLAPTA_ApprovalFlg = true;
                                data.ISMTPLAPTA_RejectedFlg = false;


                                /***************************** Planner Task Approval  **************************/
                                ISM_Task_Planner_Approved_TasksDMO approvedtask = new ISM_Task_Planner_Approved_TasksDMO();
                                approvedtask.ISMTPLAP_Id = dmo.ISMTPLAP_Id;
                                approvedtask.ISMTCR_Id = planner.ISMTCR_Id;
                                approvedtask.ISMTPLAPTA_Remarks = data.ISMTPLAP_Remarks;
                                approvedtask.ISMTPLAPTA_StartDate = planner.ISMTPLTA_StartDate;
                                approvedtask.ISMTPLAPTA_EndDate = planner.ISMTPLTA_EndDate;
                                approvedtask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLTA_EffortInHrs;
                                approvedtask.ISMTPLAPTA_Status = planner.ISMTPLTA_Status;
                                approvedtask.ISMTPLAPTA_DeviationFlg = data.ISMTPLTA_DeviationFlg;
                                approvedtask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                approvedtask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                                approvedtask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                approvedtask.ISMTPLTA_Id = planner.ISMTPLTA_Id;
                                approvedtask.ISMTPLAPTA_ActiveFlg = true;

                                approvedtask.CreatedDate = indianTime;
                                approvedtask.UpdatedDate = indianTime;
                                approvedtask.ISMTPLAPTA_CreatedBy = data.UserId;
                                approvedtask.ISMTPLAPTA_UpdatedBy = data.UserId;

                                _IssueContext.Add(approvedtask);
                                if (data.ISMTPLAPTA_ApprovalFlg == true)
                                {
                                    finaleffort += planner.ISMTPLTA_EffortInHrs;
                                }
                                if (data.ISMTPLAPTA_ApprovalFlg == true)
                                {
                                    approvedCount = approvedCount + 1;
                                }
                                else if (data.ISMTPLAPTA_RejectedFlg == true)
                                {
                                    rejectedCount = rejectedCount + 1;
                                }
                                // ************ Checking Planner Task Is Created Or Not ************ //

                                var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();

                                if (updateplannertask.Count > 0)
                                {
                                    var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                    && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_Id == planner.ISMTPLTA_Id);

                                    result.ISMTPLTA_StartDate = planner.ISMTPLTA_StartDate;
                                    result.ISMTPLTA_EndDate = planner.ISMTPLTA_EndDate;
                                    result.ISMTPLTA_EffortInHrs = planner.ISMTPLTA_EffortInHrs;
                                    result.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                    result.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                    result.UpdatedDate = indianTime;
                                    result.ISMTPLTA_UpdatedBy = data.UserId;

                                    _IssueContext.Update(result);
                                }
                                else
                                {
                                    ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                                    dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                                    dmo_task.ISMTCR_Id = planner.ISMTCR_Id;
                                    dmo_task.ISMTPLTA_Remarks = planner.ISMTPLTA_Remarks;
                                    dmo_task.ISMTPLTA_StartDate = planner.ISMTPLTA_StartDate;
                                    dmo_task.ISMTPLTA_EndDate = planner.ISMTPLTA_EndDate;
                                    dmo_task.ISMTPLTA_EffortInHrs = planner.ISMTPLTA_EffortInHrs;
                                    dmo_task.ISMTPLTA_Status = planner.ISMTPLTA_Status;
                                    dmo_task.ISMTPLTA_ExtraFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                    dmo_task.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                    dmo_task.ISMTPLTA_ActiveFlg = true;
                                    dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                                    dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                                    dmo_task.CreatedDate = indianTime;
                                    dmo_task.UpdatedDate = indianTime;
                                    _IssueContext.Add(dmo_task);
                                }

                                // ***************** Adding Task To Assigned Task ***************** //

                                var checktaskisassigned = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == plannedby_id
                                && a.ISMTCR_Id == planner.ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                                if (checktaskisassigned.Count() == 0)
                                {
                                    ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                                    dmo_task_assigned.ISMTCR_Id = planner.ISMTCR_Id;
                                    dmo_task_assigned.HRME_Id = plannedby_id;
                                    dmo_task_assigned.ISMTCRASTO_AssignedDate = DateTime.UtcNow;
                                    dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                                    dmo_task_assigned.ISMTCRASTO_Remarks = planner.ISMTPLTA_Remarks;
                                    dmo_task_assigned.ISMTCRASTO_StartDate = planner.ISMTPLTA_StartDate;
                                    dmo_task_assigned.ISMTCRASTO_EndDate = planner.ISMTPLTA_EndDate;
                                    dmo_task_assigned.ISMTCRASTO_EffortInHrs = planner.ISMTPLTA_EffortInHrs;
                                    dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                                    dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                                    dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                                    dmo_task_assigned.CreatedDate = indianTime;
                                    dmo_task_assigned.UpdatedDate = indianTime;

                                    _IssueContext.Add(dmo_task_assigned);
                                }
                            }


                            //***************** Planner Task Overall Status ***************//
                            long totalcount = 0;

                            var plannertasklist = (from a in _IssueContext.ISM_Task_Planner_ApprovedDMO
                                                   from b in _IssueContext.ISM_Task_Planner_Approved_TasksDMO
                                                   where (a.ISMTPLAP_Id == b.ISMTPLAP_Id && a.ISMTPL_Id == data.ISMTPL_Id)
                                                   select new ISM_PlannerCreationDTO
                                                   {
                                                       ISMTPL_Id = a.ISMTPL_Id,
                                                       ISMTPLAPTA_Id = b.ISMTPLAPTA_Id,
                                                   }).Distinct().ToList();

                            totalcount = plannertasklist.Count();

                            data.ISMTPL_ApprovalFlg = true;

                            decimal totalhrslist = 0;
                            totalhrslist = finaleffort + totalhrs;

                            var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                            plannerresult.ISMTPL_TotalHrs = totalhrslist;
                            plannerresult.ISMTPL_ApprovalFlg = data.ISMTPL_ApprovalFlg;
                            plannerresult.ISMTPL_ApprovedBy = empid;
                            _IssueContext.Update(plannerresult);
                            var i = _IssueContext.SaveChanges();

                            if (i > 0)
                            {
                                data.returnval = true;

                                var devicelist = (from a in _IssueContext.ISM_Task_PlannerDMO
                                                  from b in _IssueContext.MasterEmployee
                                                  where a.HRME_Id == b.HRME_Id && a.ISMTPL_Id == data.ISMTPL_Id
                                                  && b.HRME_AppDownloadedDeviceId != null && b.HRME_AppDownloadedDeviceId != ""
                                                  select new ISM_ComplaintsTaskListDTO
                                                  {
                                                      HRME_Id = b.HRME_Id,
                                                      HRME_AppDownloadedDeviceId = b.HRME_AppDownloadedDeviceId
                                                  }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                                if (devicelist.Count > 0)
                                {
                                    string deviceidsnew = "";
                                    string devicenew = "";
                                    foreach (var deviceid in devicelist)
                                    {

                                        deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';


                                        devicenew = "[" + deviceidsnew + "]";

                                        try
                                        {
                                            string titletext = "Planner Approved";
                                            if (data.sound == "")
                                            {
                                                data.sound = "default";
                                            }
                                            callnotification(devicenew, deviceid.HRME_Id, data.ISMTPL_Id, titletext, data.MI_Id, data.sound);
                                            Insertnotification(data.MI_Id, deviceid.HRME_Id, data.ISMTPL_Id, titletext, data.UserId, "PLA");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                    }
                                }

                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (counttrue == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Planner_Status" && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = Resultsms;

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

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
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

                        if (Attechement.Equals("1"))
                        {
                            var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                        ////******************* EMAIL BCC DETAILS ****************//

                        //if (emailbccdetails != null && emailbccdetails != "")
                        //{
                        //    string[] bccmaildetails = emailbccdetails.Split(',');

                        //    foreach (var c in bccmaildetails)
                        //    {
                        //        message.AddBcc(c.ToString());
                        //    }
                        //}


                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
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


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }
                                //******************* EMAIL CC DETAILS ****************//

                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }

                                ////******************* EMAIL BCC DETAILS ****************//

                                //if (emailbccdetails != null && emailbccdetails != "")
                                //{
                                //    string[] bccmaildetails = emailbccdetails.Split(',');

                                //    foreach (var c in bccmaildetails)
                                //    {
                                //        emailMessage.Bcc.Add(c.ToString());
                                //    }
                                //}


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Planner_Report_Generation" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _IssueContext.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
        public async Task<ISM_PlannerCreationDTO> getApprovedPlannerDetail(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Approved_Planner_Detail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.BigInt)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMTPLAP_Id",
          SqlDbType.BigInt)
                    {
                        Value = data.ISMTPLAP_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
          SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_ApprovedPlannerDetail = retObject.ToArray();
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
        public ISM_PlannerCreationDTO removetask(ISM_PlannerCreationDTO data)
        {
            try
            {
                var result = _IssueContext.ISM_TaskCreation_AssignedToDMO.Single(t => t.ISMTCRASTO_Id == data.ISMTCRASTO_Id);

                if (result.ISMTCRASTO_ActiveFlg == true)
                {
                    result.ISMTCRASTO_ActiveFlg = false;
                }
                else if (result.ISMTCRASTO_ActiveFlg == false)
                {
                    result.ISMTCRASTO_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);
                int returnval = _IssueContext.SaveChanges();
                if (returnval > 0)
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
            }
            return data;
        }

        public async Task<ISM_PlannerCreationDTO> AutoPlannerApproval(ISM_PlannerCreationDTO data)
        {
            try
            {

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AUTOPLANNERAPPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@user_Id",
          SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_ApprovedPlannerDetail = retObject.ToArray();
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

        public string callnotification(string devicenew, long empid, long task_id, string titletext, long mi_id, string sound)
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

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + indianTime + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

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

        //AutoPlanerApprovalEMpolee wise 
        // ISM_PlannerApproval Empoyyewise
        //loaddata
        public async Task<ISM_PlannerCreationDTO> loaddata(ISM_PlannerCreationDTO data)
        {


            try
            {


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Auto_PlanerEmployee";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.UserId
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
                        data.get_userEmplist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Auto_PlanerEmployee";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);

                            }
                        }
                        data.get_employeelist = retObject.ToArray();
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
        //deactiveemployee
        public async Task<ISM_PlannerCreationDTO> deactiveemployee(ISM_PlannerCreationDTO data)
        {


            try
            {

                if (data.emparray != null && data.emparray.Length > 0)
                {
                    foreach (var d in data.emparray)
                    {
                        var Updatesplaner = _IssueContext.MasterEmployee.Where(R => R.HRME_Id == d.HRME_Id).FirstOrDefault();
                        Updatesplaner.HRME_AutoPlanerApproval = false;
                        Updatesplaner.HRME_UpdatedBy = data.UserId;
                        Updatesplaner.UpdatedDate = DateTime.Now;
                        _IssueContext.Update(Updatesplaner);
                    }
                    int i = _IssueContext.SaveChanges();
                    if(i > 0)
                    {
                        data.sound = "save";
                    }
                    else
                    {
                        data.sound = "notsave";
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.sound = "admin";
            }
            return data;
        }
        //saveemployee
        public async Task<ISM_PlannerCreationDTO> saveemployee(ISM_PlannerCreationDTO data)
        {


            try
            {

                if (data.emparray != null && data.emparray.Length > 0)
                {
                    foreach (var d in data.emparray)
                    {
                        var Updatesplaner = _IssueContext.MasterEmployee.Where(R => R.HRME_Id == d.HRME_Id).FirstOrDefault();
                        Updatesplaner.HRME_AutoPlanerApproval = true;
                        Updatesplaner.HRME_UpdatedBy = data.UserId;
                        Updatesplaner.UpdatedDate = DateTime.Now;
                        _IssueContext.Update(Updatesplaner);

                    }
                  int  i=  _IssueContext.SaveChanges();
                    if (i > 0)
                    {
                        data.sound = "save";
                    }
                    else
                    {
                        data.sound = "notsave";
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.sound = "admin";
            }
            return data;
        }
    }



}
