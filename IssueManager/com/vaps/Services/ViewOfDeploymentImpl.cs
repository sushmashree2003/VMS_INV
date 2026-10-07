using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.VersionControl;
using IssueManager.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ViewOfDeploymentImpl : ViewOfDeploymentInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ViewOfDeploymentImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public ViewOfDeploymentDTO getloaddata(ViewOfDeploymentDTO data)

        {
            try
            {
                data.get_client = _IssueContext.IVRM_Client_HostingDMO.Where(t => t.IVRMCH_ActiveFlag == true).ToArray();

                //data.get_mi_list = _IssueContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                //data.get_taskdetails = (from a in _IssueContext.ViewOfDeploymentDMO
                //                        from b in _IssueContext.ViewOfDeploymentTasksDMO
                //                        from c in _IssueContext.Institution
                //                        from d in _IssueContext.clientTable
                //                        where (a.VPSD_ActiveFlag == true && a.MI_Id==c.MI_Id && d.ISMMCLT_Id==a.ISMMCLT_Id)

                //                        select new ViewOfDeploymentDTO
                //                        {
                //                            VPSD_Name = a.VPSD_Name,
                //                            VPSD_Environment = a.VPSD_Environment,
                //                            VPSD_CodeType = a.VPSD_CodeType,
                //                            MI_Id = a.MI_Id,
                //                            ISMMCLT_Id = a.ISMMCLT_Id,
                //                            MI_name=c.MI_Name,
                //                            Client_Name=d.ISMMCLT_ClientName,
                //                            VPSD_Id=a.VPSD_Id,
                //                            VPSD_CreatedDate=a.VPSD_CreatedDate
                //                        }).Distinct().OrderByDescending(t => t.VPSD_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public ViewOfDeploymentDTO GetOnchangeData(ViewOfDeploymentDTO data)
        {
            try
            {
                //List<long> mi_id = new List<long>();

                //foreach (var item in data.complistarray)
                //{
                //    mi_id.Add(item.MI_Id);
                //}


                //data.get_client = _IssueContext.clientTable.Where(c=>mi_id.Contains(c.MI_Id) ).Distinct().ToArray();

                data.version_list = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ViewOfDeploymentDTO GetOnchangeClient(ViewOfDeploymentDTO data)
        {
            try
            {
                var list = _IssueContext.ViewOfDeploymentTasksDMO.Where(R => R.VPIWT_ActiveFlag == true).ToList().Select(m => m.ISMTCR_Id);

                List<long> client_id = new List<long>();

                foreach (var item in data.clientlistarray)
                {
                    client_id.Add(item.ISMMCLT_Id);
                }


                data.get_tasks = (from A in _IssueContext.ISM_TaskCreationDMO
                                  from B in _IssueContext.ISM_TaskCreation_ClientDMO
                                  where (A.ISMTCR_Id == B.ISMTCR_Id && A.ISMTCR_ActiveFlg == true && !list.Contains(A.ISMTCR_Id) && client_id.Contains(B.ISMMCLT_Id))
                                  select new ViewOfDeploymentDTO
                                  {
                                      ISMTCR_Id = A.ISMTCR_Id,
                                      ISMTCR_TaskNo = A.ISMTCR_TaskNo,
                                      ISMTCR_Title = A.ISMTCR_Title,
                                      ISMTCR_Status = A.ISMTCR_Status,
                                      ISMTCR_Desc = A.ISMTCR_Desc,
                                      CreatedDate = A.CreatedDate
                                  }).Distinct().OrderBy(t => t.ISMTCR_TaskNo).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ViewOfDeploymentDTO savedata(ViewOfDeploymentDTO data)
        {
            try
            {
                if (data.featurestasklist.Length > 0)
                {
                    try
                    {
                        int totalSaved = 0;
                        foreach (var x in data.featurestasklist)
                        {
                            var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("IVRM_Version_FeaturesDetails_Update @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.flag, x.IVRMVFD_Id, x.IVRMVFD_Features, data.UserId, x.IVRMVFD_Remarks, x.closeflag, x.IVRMVFD_Test_Case, x.IVRMVFD_Test_Case_File_Name);
                            totalSaved += contactExistsP;
                        }
                        if (totalSaved > 0)
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
                        Console.Write(ex);
                    }

                }
                //var saveddata = _IssueContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }


            return data;
            //try
            //{
            //    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            //    if (data.VPSD_Id == 0)
            //    {

            //        foreach (var comp in data.complistarray)
            //        {
            //            foreach (var clnt in data.clientlistarray)
            //            {


            //                ViewOfDeploymentDMO detail = new ViewOfDeploymentDMO();

            //                detail.VPSD_Name = data.VPSD_Name;
            //                detail.VPSD_Date = data.VPSD_Date;
            //                detail.MI_Id = comp.MI_Id;
            //                detail.ISMMCLT_Id = clnt.ISMMCLT_Id;
            //                detail.VPSD_Environment = data.VPSD_Environment;
            //                detail.VPSD_CodeType = data.VPSD_CodeType;
            //                detail.VPSD_Path = data.VPSD_Path;

            //                detail.VPSD_CreatedDate = indianTime;
            //                detail.VPSD_CreatedBy = data.UserId;
            //                detail.VPSD_UpdatedBy = data.UserId;
            //                detail.VPSD_ActiveFlag = true;
            //                detail.VPSD_UpdatedDate = indianTime;
            //                _IssueContext.Add(detail);
            //                foreach (var task in data.tasklistarray)
            //                {
            //                    ViewOfDeploymentTasksDMO taskDetail = new ViewOfDeploymentTasksDMO();

            //                    taskDetail.VPSD_Id = detail.VPSD_Id;

            //                    taskDetail.ISMTCR_Id = task.ISMTCR_Id;
            //                    taskDetail.VPIWT_CreatedDate = indianTime;
            //                    taskDetail.VPIWT_ActiveFlag = true;
            //                    taskDetail.VPIWT_UpdatedDate = indianTime;
            //                    _IssueContext.Add(taskDetail);
            //                }
            //            }

            //        }

            //        int save = _IssueContext.SaveChanges();
            //        if (save > 0)
            //        {
            //            data.returnval = true;
            //        }
            //        else
            //        {
            //            data.returnval = false;
            //        }
            //    }


            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            // return data;
        }

        public ViewOfDeploymentDTO gettaskslist(ViewOfDeploymentDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_VC_FeatureRemark_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)
                    {
                        Value = data.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.BigInt)
                    {
                        Value = data.IVRMCH_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.BigInt)
                    {
                        Value = data.IVRMVT_Id
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.tasks_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.tasks_list = (from a in _IssueContext.ViewOfDeploymentDMO
                //                        from b in _IssueContext.ViewOfDeploymentTasksDMO
                //                        from c in _IssueContext.ISM_TaskCreationDMO
                //                        where (a.VPSD_ActiveFlag == true && b.ISMTCR_Id==c.ISMTCR_Id && a.VPSD_Id==b.VPSD_Id && a.VPSD_Id==data.VPSD_Id)

                //                        select new ViewOfDeploymentDTO
                //                        {
                //                            ISMTCR_TaskNo = c.ISMTCR_TaskNo,


                //                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        //scope report
        public ViewOfDeploymentDTO getReport(ViewOfDeploymentDTO data)

        {

            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                if (hrmEempid.Length > 0)
                {
                    data.HRME_ID = hrmEempid.FirstOrDefault().Emp_Code;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_VC_FeatureRemark_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = data.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMCH_Id", SqlDbType.BigInt)
                    {
                        Value = data.IVRMCH_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMVT_Id", SqlDbType.BigInt)
                    {
                        Value = data.IVRMVT_Id
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.tasks_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //string env = "";
                //string envv = "";

                //if (data.envlistarray != null)
                //{
                //    for (int i = 0; i < data.envlistarray.Length; i++)
                //    {
                //        if (data.envlistarray[i].VPSD_Environment != "")
                //        {

                //            env += "'" + data.envlistarray[i].VPSD_Environment + "',";

                //        }

                //    }

                //    if (env.EndsWith(","))
                //    {
                //        envv = env.Trim(',');

                //    }
                //}

                //using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "ScopeTasksMapping_Report";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                //    {
                //        Value = data.Fromdate
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                //    {
                //        Value = data.Todate
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@Environment", SqlDbType.VarChar)
                //    {
                //        Value = envv
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
                //        data.get_report = retObject.ToArray();
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }


        //--------------------------------------------------------------------------

        public ViewOfDeploymentDTO get_scope_tasklist_for_approval(ViewOfDeploymentDTO scope)
        {
            try
            {

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Version_features_approval_list";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IVRMVT_Id", SqlDbType.BigInt) { Value = scope.IVRMVT_Id });
                    cmd.Parameters.Add(new SqlParameter("@IVRMCH_Id", SqlDbType.BigInt) { Value = scope.IVRMCH_Id });

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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        scope.get_scope_approval_tasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return scope;
        }


        public ViewOfDeploymentDTO approve_reject_scope(ViewOfDeploymentDTO scope)
        {
            try
            {
                long totalSaved = 0;
                foreach (var s in scope.Approved_rejected_socpe_tasks)
                {
                    var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("IVRM_VERSION_TASK_APPROVAL @p0,@p1,@p2,@p3,@p4,@p5,@p6", s.IVRMVFD_Id, s.IVRMVFD_Features, s.IVRMVFD_Remarks, s.IVRMVFD_Test_Case, s.IVRMVFD_Test_Case_File_Name, scope.UserId, s.check);
                    totalSaved += contactExistsP;
                   
                }
                if (totalSaved>0)
                {
                    scope.saved_count = totalSaved;
                    scope.returnval = true;
                }
                else
                {
                    scope.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return scope;
        }

    }
}
