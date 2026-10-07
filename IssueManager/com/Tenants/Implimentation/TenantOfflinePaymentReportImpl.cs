using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{

    public class TenantOfflinePaymentReportImpl : Interfaces.TenantOfflinePaymentReportInterface
    {
        public IssueManagerContext _context;
        public TenantOfflinePaymentReportImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }

        //public PortalContext _context1;
        //    public TenantOfflinePaymentReportImpl(PortalContext _cont1)
        //    {
        //        _context1 = _cont1;
        //    }
        public TenantOfflinePaymentReportDTO onloaddata(TenantOfflinePaymentReportDTO data)
        {
            try
            {
                data.yeardata = _context.HR_Master_LeaveYearDMO.Where(a => a.HRMLY_ActiveFlag == true && a.MI_Id == data.MI_Id).Distinct().ToArray();
                data.monthdata = _context.IVRM_Month_DMO.Where(R => R.Is_Active == true).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TenantOfflinePaymentReportDTO report(TenantOfflinePaymentReportDTO data)
        {
            try
            {

                List<long> IVRM_Month_Id = new List<long>();
                foreach (var c in data.monthlist)
                {
                    IVRM_Month_Id.Add(c.IVRM_Month_Id);
                }

                string joined = string.Join(",", IVRM_Month_Id);

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantOfflinePaymentReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Id", SqlDbType.VarChar) { Value = joined });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGRPAY_Flag", SqlDbType.VarChar) { Value = data.type });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGRPAY_PaymentYear", SqlDbType.VarChar) { Value = data.yrrrrs });
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
                        data.displayTableOneDetails = retObject.ToArray();
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

        public TenantOfflinePaymentReportDTO viewuploadflies(TenantOfflinePaymentReportDTO data)
        {
            try
            {


                data.editfiles = (from a in _context.TenantAgreement
                                  from b in _context.TenantPaymentDetailsDMO
                                  from c in _context.IVRM_Month_DMO
                                  where (b.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id && a.TENANTAGR_Id == b.TENANTAGR_Id && c.IVRM_Month_Id == data.IVRM_Month_Id)
                                  select new TenantOfflinePaymentReportDTO
                                  {

                                      name = a.TENANTAGR_TenantName,
                                      IVRM_Month_Name = c.IVRM_Month_Name + '-' + b.TENANTAGRPAY_PaymentYear,
                                      TENANTAGRPAY_Amount = b.TENANTAGRPAY_Amount,
                                      TENANTAGRPAY_PaymentDate = b.TENANTAGRPAY_PaymentDate,
                                      email = a.TENANTAGR_EmailId,
                                      phonenumber = a.TENANTAGR_ContactNo,
                                      agreementfromdate = a.TENANTAGR_AgrFromDate,
                                      agreementtodate = a.TENANTAGR_AgrToDate,
                                      TENANTAGRPAY_ReceiptNo = b.TENANTAGRPAY_ReceiptNo,
                                      TENANTAGRPAY_Flag  = b.TENANTAGRPAY_Flag,



                                  }).Distinct().OrderBy(a => a.name).ToArray();

                data.MI_Logo = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Select(R => R.MI_Logo).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public TenantOfflinePaymentReportDTO getpaymentreport(TenantOfflinePaymentReportDTO data)
        {
            try
            {
                var tenant_user = _context.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.TENANTAGR_Id).Select(s => s.TENANTAGR_Id).FirstOrDefault();

                List<long> IVRM_Month_Id = new List<long>();
                foreach (var c in data.monthlist)
                {
                    IVRM_Month_Id.Add(c.IVRM_Month_Id);
                }

                string joined = string.Join(",", IVRM_Month_Id);

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantIndividualOfflinePaymentReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Id", SqlDbType.VarChar) { Value = joined });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGRPAY_Flag", SqlDbType.VarChar) { Value = data.type });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGRPAY_PaymentYear", SqlDbType.VarChar) { Value = data.yrrrrs });
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
                        data.displayTableOneDetails = retObject.ToArray();
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