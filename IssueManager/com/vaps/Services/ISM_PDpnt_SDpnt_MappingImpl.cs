using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.HRMS;
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
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_PDpnt_SDpnt_MappingImpl : Interfaces.ISM_PDpnt_SDpnt_MappingInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_PDpnt_SDpnt_MappingImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_PDpnt_SDpnt_MappingDTO> loaddataAsync(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            try
            {
                data.Pmi_name = _IssueContext.Institution.Where(t => t.MI_Id == data.MI_Id).Select(t => t.MI_Name).FirstOrDefault();

                data.departmentlist = _IssueContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.institutionlist = _IssueContext.Institution.Where(t => t.MI_ActiveFlag == 1).OrderBy(a=>a.MI_Name).ToArray();


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Primary_Secondary_DeptMapping";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.gridviewdetails = retObject1.ToArray();
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

        public ISM_PDpnt_SDpnt_MappingDTO save(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            try
            {
                if (data.ISMPDSDMAP_Id == 0)
                {
                    var duplicate = _IssueContext.ISM_PDpnt_SDpnt_Mapping_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == data.HRMD_Id && t.ISMPDSDMAP_SMI_Id == data.ISMPDSDMAP_SMI_Id && t.ISMPDSDMAP_SHRMD_Id == data.ISMPDSDMAP_SHRMD_Id).Distinct().ToList();

                    if (duplicate.Count() > 0)
                    {
                        data.msg = "Duplicate";
                    }
                    else
                    {
                        ISM_PDpnt_SDpnt_MappingDMO a = new ISM_PDpnt_SDpnt_MappingDMO();
                        a.MI_Id = data.MI_Id;
                        a.HRMD_Id = data.HRMD_Id;
                        a.ISMPDSDMAP_SMI_Id = data.ISMPDSDMAP_SMI_Id;
                        a.ISMPDSDMAP_SHRMD_Id = data.ISMPDSDMAP_SHRMD_Id;
                        a.ISMPDSDMAP_ActiveFlag = true;
                        a.ISMPDSDMAP_CreatedBy = data.User_Id;
                        a.ISMPDSDMAP_UpdatedBy = data.User_Id;
                        a.ISMPDSDMAP_CreatedDate = DateTime.Now;
                        a.ISMPDSDMAP_UpdatedDate = DateTime.Now;
                        _IssueContext.Add(a);
                        var w = _IssueContext.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "Saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.ISMPDSDMAP_Id > 0)
                {
                    var duplicate = _IssueContext.ISM_PDpnt_SDpnt_Mapping_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == data.HRMD_Id && t.ISMPDSDMAP_SMI_Id == data.ISMPDSDMAP_SMI_Id && t.ISMPDSDMAP_SHRMD_Id == data.ISMPDSDMAP_SHRMD_Id).Distinct().ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.msg = "Duplicate";
                    }
                    else
                    {
                        var j = _IssueContext.ISM_PDpnt_SDpnt_Mapping_DMO.Where(t => t.MI_Id == data.MI_Id & t.ISMPDSDMAP_Id == data.ISMPDSDMAP_Id).SingleOrDefault();
                        j.HRMD_Id = data.HRMD_Id;
                        j.MI_Id = data.MI_Id;
                        j.ISMPDSDMAP_SMI_Id = data.ISMPDSDMAP_SMI_Id;
                        j.ISMPDSDMAP_SHRMD_Id = data.ISMPDSDMAP_SHRMD_Id;
                        j.ISMPDSDMAP_ActiveFlag = true;
                        j.ISMPDSDMAP_UpdatedBy = data.User_Id;
                        j.ISMPDSDMAP_UpdatedDate = DateTime.Now;
                        _IssueContext.Update(j);
                        var r = _IssueContext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
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

        public ISM_PDpnt_SDpnt_MappingDTO editdata(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            try
            {
                data.editlist = _IssueContext.ISM_PDpnt_SDpnt_Mapping_DMO.Where(t => t.ISMPDSDMAP_Id == data.ISMPDSDMAP_Id).ToArray();

                data.departmentlist = (from a in _IssueContext.ISM_PDpnt_SDpnt_Mapping_DMO
                                       from b in _IssueContext.HR_Master_Department
                                       where (a.ISMPDSDMAP_SMI_Id == b.MI_Id && b.HRMD_ActiveFlag == true && a.ISMPDSDMAP_Id == data.ISMPDSDMAP_Id)
                                       select new HR_Master_DepartmentDTO
                                       {
                                           HRMD_Id = b.HRMD_Id,
                                           MI_Id = b.MI_Id,
                                           HRMD_DepartmentName = b.HRMD_DepartmentName
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_PDpnt_SDpnt_MappingDTO deletedata(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            try
            {
                var selectdata = _IssueContext.ISM_PDpnt_SDpnt_Mapping_DMO.Where(t => t.ISMPDSDMAP_Id == data.ISMPDSDMAP_Id).FirstOrDefault();
                _IssueContext.Remove(selectdata);
                _IssueContext.SaveChanges();

                data.msg = "Success";
            }
            catch (Exception ex)
            {
                data.msg = "Failed";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_PDpnt_SDpnt_MappingDTO loaddepartment(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            try
            {
                data.departmentlist = _IssueContext.HR_Master_Department.Where(t => t.MI_Id == data.ISMPDSDMAP_SMI_Id && t.HRMD_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}