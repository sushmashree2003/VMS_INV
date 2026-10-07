using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_Client_ComplaintsTaskListImpl : Interfaces.ISM_Client_ComplaintsTaskListInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_Client_ComplaintsTaskListImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
        public async Task<ISM_ComplaintsTaskListDTO> getdetails(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                string role = "";
                if (userrole.Count > 0)
                {
                    role = userrole.FirstOrDefault().IVRMRT_Role;
                    data.roletype = role;
                }
                string statuslist = "Open,Inprogress,Reopen";
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                var clientlist = (from A in _IssueContext.clientTable
                                  from B in _IssueContext.ISM_Master_Client_UserMappingDMO
                                  where (A.ISMMCLT_Id == B.ISMMCLT_Id && A.ISMMCLT_ActiveFlag == true && B.ISMCIM_IEList == data.UserId)
                                  select new ISM_ComplaintsTaskListDTO
                                  {
                                      ISMMCLT_Id = A.ISMMCLT_Id,
                                      ISMMCLT_ClientName = A.ISMMCLT_ClientName
                                  }).Distinct().OrderBy(t => t.ISMMCLT_Id).ToList();

                data.get_client = clientlist.ToArray();

                var totaltask = (from A in _IssueContext.ISM_TaskCreationDMO
                                 from B in _IssueContext.ISM_TaskCreation_ClientDMO
                                 from C in _IssueContext.ApplicationUser
                                 where (A.ISMTCR_Id == B.ISMTCR_Id && A.ISMTCR_ActiveFlg == true && A.ISMTCR_CreatedBy == C.Id && A.ISMTCR_CreatedBy == data.UserId)
                                 select new ISM_ComplaintsTaskListDTO
                                 {
                                     ISMTCR_Id = A.ISMTCR_Id,
                                 }).Distinct().OrderBy(t => t.ISMMCLT_Id).ToList();                
                data.taskCount = totaltask.Count;

                //  List<long?> ismmclt_Ids = new List<long?>();
                string ismmclt_Ids = "0";
                if (clientlist.Count > 0)
                {
                    foreach (var cl in clientlist)
                    {
                        //ismmclt_Ids.Add(cl.ISMMCLT_Id);
                        ismmclt_Ids = ismmclt_Ids + "," + cl.ISMMCLT_Id;
                    }
                }
                //*********************************** Task List ************************************************//           
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Client_Complaint_TaskList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@RoleFlg",
           SqlDbType.VarChar)
                    {
                        Value = data.roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@TypeFlg",
           SqlDbType.VarChar)
                    {
                        Value = data.typeFlg
                    });
                    cmd.Parameters.Add(new SqlParameter("@status",
           SqlDbType.VarChar)
                    {
                        Value = statuslist
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id",
                               SqlDbType.VarChar)
                    {
                        Value = ismmclt_Ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMM_Id",
            SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.VarChar)
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
                        data.get_clientcomptasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //*********************************** IE List ********************************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_EmployeeList";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        data.get_employeelist = retObject.ToArray();
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
        public async Task<ISM_ComplaintsTaskListDTO> getsearchtask(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                string ismmclt_Ids = "0";
                if (data.arrayClient.Length > 0)
                {
                    foreach (var ue in data.arrayClient)
                    {
                        ismmclt_Ids = ismmclt_Ids + "," + ue.ISMMCLT_Id;
                    }
                }
                string statuslist = "";
                if (data.arrayStatus.Length > 0)
                {
                    foreach (var sts in data.arrayStatus)
                    {
                        statuslist = statuslist + "," + sts.ISMMISTS_StatusName;
                    }
                }
                else
                {
                    if (statuslist == "")
                    {
                        statuslist = "Open";
                    }
                }
                //*********************************** Task List ************************************************//           
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Client_Complaint_TaskList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@RoleFlg",
           SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
                    });
                    cmd.Parameters.Add(new SqlParameter("@TypeFlg",
           SqlDbType.VarChar)
                    {
                        Value = data.typeFlg
                    });
                    cmd.Parameters.Add(new SqlParameter("@status",
           SqlDbType.VarChar)
                    {
                        Value = statuslist
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id",
            SqlDbType.VarChar)
                    {
                        Value = ismmclt_Ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMM_Id",
            SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.VarChar)
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
                        data.get_clientcomptasklist = retObject.ToArray();
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
        public async Task<ISM_ComplaintsTaskListDTO> gettaskdetails(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Client_TaskDeatils";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
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
                        data.get_taskdetails = retObject.ToArray();
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
