using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Tenants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Tenant.Services
{
    public class TenantDashboardimpl:Interface.TenantDashboardInterface
    {

        private static ConcurrentDictionary<string, TenantDashboardDTO> _login =
           new ConcurrentDictionary<string, TenantDashboardDTO>();
        private PortalContext _portalDashboardContext;
        private readonly DomainModelMsSqlServerContext _db;

        public TenantDashboardimpl(PortalContext portalDashboardContext , DomainModelMsSqlServerContext db)
        {
            _portalDashboardContext = portalDashboardContext;
            _db = db;
        }
        public TenantDashboardDTO ViewTenantProfile(TenantDashboardDTO data)
        {
            try
            {

                var tenant_user = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.MI_Id == data.MI_Id).Select(s=>s.TENANTAGR_Id).FirstOrDefault();

                if(tenant_user!=null)
                {
                    data.tenants_details=_portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_Id== tenant_user && t.MI_Id == data.MI_Id).ToArray();
                }

                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TENANTPHOTDETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TENANTID", SqlDbType.VarChar) { Value = tenant_user });

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
                        data.get_tenant_details = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TENANT_PAID_DETAILS_TEST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar) { Value = tenant_user });

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
                        data.recenocol = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.viewstudentjoineddetails = (from a in _studentDashboardContext.Adm_M_Student
                //                                 from b in _studentDashboardContext.AcademicYearDMO
                //                                 from c in _studentDashboardContext.School_M_Class
                //                                 where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.AMST_Id == data.AMST_Id
                //                                 && a.MI_Id == data.MI_Id)
                //                                 select new StudentDashboardDTO
                //                                 {
                //                                     studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                //                                     (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                //                                     (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                //                                     AMST_AdmNo = a.AMST_AdmNo,
                //                                     AMST_RegistrationNo = a.AMST_RegistrationNo,
                //                                     ASMAY_Year = b.ASMAY_Year,
                //                                     ASMCL_ClassName = c.ASMCL_ClassName,
                //                                     AMST_Photoname = a.AMST_Photoname,
                //                                     AMST_Sex = a.AMST_Sex,
                //                                     AMST_SOL = a.AMST_SOL,
                //                                     AMST_Date = a.AMST_Date,
                //                                     AMST_DOB = a.AMST_DOB,
                //                                 }).Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public TenantDashboardDTO getMonthlyBalance_details(TenantDashboardDTO data)
        {
            try
            {

                var TENANTAGR_Id = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.MI_Id == data.MI_Id).Select(s => s.TENANTAGR_Id).FirstOrDefault();

                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "tenant_month_details_bal";
                    cmd.CommandText = "tenant_month_details";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = TENANTAGR_Id
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
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_month_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
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
