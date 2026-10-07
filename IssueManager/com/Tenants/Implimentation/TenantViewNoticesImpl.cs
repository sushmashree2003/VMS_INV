using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Implimentation
{
    public class TenantViewNoticesImpl:Interface.TenantViewNoticesInterface
    {
        public IssueManagerContext _context;

        public TenantViewNoticesImpl(IssueManagerContext isc)
        {





            _context = isc;

        }

        public async Task<TenantAgreementDTO> getdetails(TenantAgreementDTO data)
        {
            try
            {
                var TENANTAGR_Id = _context.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.UserId).ToList();
                data.TENANTAGR_Id = TENANTAGR_Id[0].TENANTAGR_Id;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantNotices";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar) { Value = TENANTAGR_Id[0].TENANTAGR_Id });
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
                            data.getloaddata = retObject.Distinct().ToArray();
                        }
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


        public async Task<TenantAgreementDTO> get_report(TenantAgreementDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantNoticesMonthly";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar) { Value = data.TENANTAGR_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = data.TENANTAGR_AgrFromDate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = data.TENANTAGR_AgrToDate });
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
                            data.getloaddata = retObject.Distinct().ToArray();
                        }
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

        public async Task<TenantAgreementDTO> get_NoticeBoardreport(TenantAgreementDTO data)
        {
            try
            {
                var tenant_user = _context.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.UserId ).Select(s => s.TENANTAGR_Id).FirstOrDefault();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantNoticesBoardReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar) { Value = tenant_user });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = data.TENANTAGR_AgrFromDate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = data.TENANTAGR_AgrToDate });
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
                            data.getloaddata = retObject.Distinct().ToArray();
                        }
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

        public async Task<TenantAgreementDTO> ViewFiles(TenantAgreementDTO data)
        {
            try
            {
    
              
                    var resultduplicate = _context.TenantNoticeBoardTenantViewedDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENENTNTB_Id == data.TENENTNTB_Id) .ToList();



                    if (resultduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                       TenantNoticeBoardTenantViewedDMO result = new TenantNoticeBoardTenantViewedDMO();
           
                        result.TENENTNTB_Id = data.TENENTNTB_Id;
                        result.TENANTAGR_Id = data.TENANTAGR_Id;
                        result.TENANTNTBTNTV_ViewedDateTime = DateTime.Now;
                        result.TENANTNTBTNTV_ActiveFlag = true;

                        result.TENANTNTBTNTV_CreatedBy = data.UserId;
                        result.TENANTNTBTNTV_CreatedDate = DateTime.Now;

                        result.TENANTNTBTNTV_UpdatedBy = data.UserId;


                        result.TENANTNTBTNTV_UpdatedDate = DateTime.Now;
                        result.TENANTAGR_Id = data.TENANTAGR_Id;

                        _context.Add(result);


                    




                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Failed";
                        }
                    }
            


                





                data.getviewfilesdetails = (from a in _context.TenantNoticeBoardFilesDMO
                                            where (a.TENENTNTB_Id == data.TENENTNTB_Id)
                                            select new TenantAgreementDTO
                                            {
                                                TENENTNTB_Id = a.TENENTNTB_Id,
                                                TENENTNTBFL_Id = a.TENENTNTBFL_Id,
                                                TENENTNTBFL_FileName = a.TENENTNTBFL_FileName,
                                                TENENTNTBFL_FilePath = a.TENENTNTBFL_FilePath,
                                            }).Distinct().ToArray();



            


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}
