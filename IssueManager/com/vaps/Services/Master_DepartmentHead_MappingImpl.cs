using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
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
    public class Master_DepartmentHead_MappingIMPL : Interfaces.Master_DepartmentHead_MappingInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public Master_DepartmentHead_MappingIMPL(IssueManagerContext hh)
        {
            _IssueManagercontext = hh;
        }
        //====================================loaddata ===========================================
        public Master_DepartmentHead_MappingDTO loaddata(Master_DepartmentHead_MappingDTO data)
        {
            try
            {
                data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();


                data.alldepartmentcode = (from a in _IssueManagercontext.HR_Master_DepartmentCodeDMO
                                          from b in _IssueManagercontext.HR_Master_Department
                                          where (a.HRMDC_ID == b.HRMDC_ID && b.HRMD_ActiveFlag == true)
                                          select new Master_DepartmentHead_MappingDTO
                                          {
                                              HRMDC_ID = a.HRMDC_ID,
                                              HRMDC_Name = a.HRMDC_Name,
                                              HRMDC_Code = a.HRMDC_Code,
                                              MI_Id=b.MI_Id
                                          }).Distinct().ToArray();

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DepartmentDisplaydata";
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.displaydata = retObject.ToArray();
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

        //OnChangeEmployee
        public Master_DepartmentHead_MappingDTO depchange(Master_DepartmentHead_MappingDTO data)
        {
            try
            {

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "load_empname_ondep";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.BigInt) { Value = data.HRMDC_ID });
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
                        data.alldepartmentcode = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DepartmentDisplaydata";
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.displaydata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        //==========================================save data=======================================
        public Master_DepartmentHead_MappingDTO savedata(Master_DepartmentHead_MappingDTO data)
        {
            try
            {
                if (data.HRMDCH_ID != 0)
                {
                    var res = _IssueManagercontext.HR_Master_DepartmentCode_Head.Where(t => t.HRME_ID == data.HRME_ID && t.HRMDC_ID == data.HRMDC_ID && t.HRMDCH_ID != data.HRMDCH_ID).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _IssueManagercontext.HR_Master_DepartmentCode_Head.Single(t => t.HRMDCH_ID == data.HRMDCH_ID);
                        result.HRMDC_ID = data.HRMDC_ID;
                        result.HRME_ID = data.HRME_ID;
                        result.PlannerApproval = true;
                        result.HRMDCH_UpdatedBy = data.UserId;
                        _IssueManagercontext.Update(result);
                        var contactExists = _IssueManagercontext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    var res = _IssueManagercontext.HR_Master_DepartmentCode_Head.Where(t => t.HRME_ID == data.HRME_ID && t.HRMDC_ID == data.HRMDC_ID).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        HR_Master_DepartmentCode_HeadDMO dep = new HR_Master_DepartmentCode_HeadDMO();
                        dep.HRME_ID = data.HRME_ID;
                        dep.HRMDC_ID = data.HRMDC_ID;
                        dep.PlannerApproval = true;
                        dep.HRMDCH_CreatedBy = data.UserId;
                        _IssueManagercontext.Add(dep);
                        var contactExists = _IssueManagercontext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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
        public Master_DepartmentHead_MappingDTO Decative(Master_DepartmentHead_MappingDTO data)
        {
            try
            {
                var u = _IssueManagercontext.HR_Master_DepartmentCode_Head.Where(t => t.HRMDCH_ID == data.HRMDCH_ID).SingleOrDefault();
                if (u.PlannerApproval == true)
                {
                    u.PlannerApproval = false;
                }
                else if (u.PlannerApproval == false)
                {
                    u.PlannerApproval = true;
                }

                u.HRMDCH_UpdatedBy = data.UserId;
                _IssueManagercontext.Update(u);
                int o = _IssueManagercontext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public Master_DepartmentHead_MappingDTO Editdata(Master_DepartmentHead_MappingDTO data)
        {
            try
            {

                data.getdetails = _IssueManagercontext.HR_Master_DepartmentCode_Head.Where(R => R.HRMDCH_ID == data.HRMDCH_ID).Distinct().ToArray();
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "load_empname_ondep";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.BigInt) { Value = data.HRMDC_ID });
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
                        data.alldepartmentcode = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}


