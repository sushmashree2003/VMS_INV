using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class ISM_SwapIssuesImpl:Interfaces.ISM_SwapIssuesInterface
    {
        public IssueManagerContext _issueManagerContext;
        public DomainModelMsSqlServerContext _db;
        ILogger<ISM_SwapIssuesDTO> _logger;
        public ISM_SwapIssuesImpl(IssueManagerContext _Context, ILogger<ISM_SwapIssuesDTO> _log, DomainModelMsSqlServerContext db)
        {
            _issueManagerContext = _Context;
            _logger = _log;
            _db = db;
        }

        /* lOAD DATA */
        public ISM_SwapIssuesDTO loaddata(ISM_SwapIssuesDTO data)
        {
            try { 
            var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
            long empid = 0;
            if (hrmEempid.Count > 0)
            {
                empid = hrmEempid.FirstOrDefault().Emp_Code;
            }
            long? dept_id = 0;
            var hrmeDepartment_id = _issueManagerContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
            if (hrmeDepartment_id.Count > 0)
            {
                dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;

                data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
            }

            var hrmeDepartment = _issueManagerContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == dept_id && t.HRMD_ActiveFlag == true).Distinct().ToList();
            if (hrmeDepartment.Count > 0)
            {
                data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
            }
                //  ***********************************User Employee Mapped List * *******************************// 
                //using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "ISM_User_EmployeeList";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                //        {
                //            Value = data.UserId
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //        {
                //            Value = data.MI_Id
                //        });
                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        var retObject = new List<dynamic>();
                //        try
                //        {
                //            using (var dataReader = cmd.ExecuteReader())
                //            {
                //                while (dataReader.Read())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                        );
                //                    }
                //                    retObject.Add((ExpandoObject)dataRow);
                //                }
                //            }
                //            data.get_userEmplist = retObject.ToArray();
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }

                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_EmployeeList_ByUser";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ISM_User_StatusList";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
                {
                    Value = data.UserId
                });
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
                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.get_Status = retObject.ToArray();
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

        /* GET OTHERS DATE DETAILS*/
        public ISM_SwapIssuesDTO Report(ISM_SwapIssuesDTO data)
        {

            try
            {
                var rolet = _issueManagerContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                //string hrmeids = "0";
                //if (data.arrayuserEmp.Length > 0)
                //{
                //    foreach (var ue in data.arrayuserEmp)
                //    {
                //        hrmeids = hrmeids + "," + ue.HRME_Id;
                //    }
                //}
                //else
                //{
                //    if (hrmeids == "" || hrmeids == "0")
                //    {
                //        hrmeids = empid.ToString();
                //    }
                //}

                

                       using (var cmd = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_ExtratasklistSwapping";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 3000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.getextratasks = retObject.ToArray();
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



        public ISM_SwapIssuesDTO Swaplist(ISM_SwapIssuesDTO data)
        {

            try
            {
                var rolet = _issueManagerContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                var hrmEempid = _issueManagerContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                //string hrmeids = "0";
                //if (data.arrayuserEmp.Length > 0)
                //{
                //    foreach (var ue in data.arrayuserEmp)
                //    {
                //        hrmeids = hrmeids + "," + ue.HRME_Id;
                //    }
                //}
                //else
                //{
                //    if (hrmeids == "" || hrmeids == "0")
                //    {
                //        hrmeids = empid.ToString();
                //    }
                //}
                string category = "";
                if (data.swaplst.Length > 0)
                {
                    foreach (var ue in data.swaplst)
                    {
                        category =category + "'" + ue.category+"'," ;
                    }
                }
                string client = "";
                if (data.swaplst.Length > 0)
                {
                    foreach (var ue in data.swaplst)
                    {
                        client =client +  "'" + ue.client +"',";
                    }
                }
                if (category.EndsWith(','))
                {
                    category = category.Trim(',');  
                }
                if (client.EndsWith(','))
                {
                    client = client.Trim(',');
                }

                using (var cmd1 = _issueManagerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "ISM_Swappinglist";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 3000000;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    }); 
                    cmd1.Parameters.Add(new SqlParameter("@categaroy", SqlDbType.VarChar)
                    {
                        Value = category
                    });
                    cmd1.Parameters.Add(new SqlParameter("@Client", SqlDbType.VarChar)
                    {
                        Value = client
                    });


                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd1.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader1.GetName(iFiled),
                                       dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.swappinglst = retObject1.ToArray();
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


        public ISM_SwapIssuesDTO Swaptask(ISM_SwapIssuesDTO data)
        {

            try
            {

                var outputval = 0;
                var outputval1 = 0;
                if (data.swappingtask != null)
                {
                    for (int i = 0; i < data.swappingtask.Length; i++)
                    {
                        
                  
                       
                        outputval = _issueManagerContext.Database.ExecuteSqlCommand("ISM_SwappingTaskUpdation @p0,@p1", data.swappingtask[i].ISMTCR_Id,data.swappingtask[i].ISMTPL_Id);
                      
                       
                    }
                }

                if (outputval > 0)
                {

           

                if (data.swaplst != null)
                {
                        for (int i = 0; i < data.swaplst.Length; i++)
                        {
                            //foreach (var ue in data.arrayuserEmp)
                            //{
                               
                                outputval1 = _issueManagerContext.Database.ExecuteSqlCommand("ISM_SwappingTaskInsertion @p0,@p1,@p2", data.swaplst[i].ISMTCR_Id, data.HRME_Id, data.UserId);

                           // }
                        }
                }

                }



                if(outputval!=0 && outputval1!=0)
                {

                    data.message = "Saved";
                }
                else
                {
                    data.message = "Failed";

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
