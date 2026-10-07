using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Tenants;
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
using SendGrid.Helpers.Mail;
using CommonLibrary;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using DomainModel.Model;
using System.Net;
using System.IO;
using SendGrid;

namespace PortalHub.com.vaps.Tenant.Services
{
    public class TenantOfflinePymentImpl : Interface.TenantOfflinePaymentinterface
    {


        private static ConcurrentDictionary<string, TenantDashboardDTO> _login =
       new ConcurrentDictionary<string, TenantDashboardDTO>();
        private PortalContext _portalDashboardContext;
        private readonly DomainModelMsSqlServerContext _db;

        public TenantOfflinePymentImpl(PortalContext portalDashboardContext, DomainModelMsSqlServerContext db)
        {
            _portalDashboardContext = portalDashboardContext;
            _db = db;
        }
        public TenantDashboardDTO getallofflinedetails(TenantDashboardDTO data)
        {
            try
            {
                data.building_array = _db.HR_Master_Building.Where(b => b.MI_Id == data.MI_Id).ToArray();

                data.modeofpayment_array = _db.IVRM_ModeOfPaymentDMO.Where(p => p.MI_Id == data.MI_Id).ToArray();


                data.IVRMMOD_Id = _db.IVRM_ModeOfPaymentDMO.Where(p => p.MI_Id == data.MI_Id && (p.IVRMMOD_Flag == "C" || p.IVRMMOD_Flag == "c")).Select(s => s.IVRMMOD_Id).FirstOrDefault();

                //data.tenant_array = (from a in _portalDashboardContext.TenantAgreement
                //                     from b in _portalDashboardContext.TenantPaymentDetailsDMO
                //                     from c in _portalDashboardContext.IVRM_ModeOfPaymentDMO
                //                     from d in _portalDashboardContext.IVRM_Month_DMO
                //                     where (a.TENANTAGR_Id == b.TENANTAGR_Id && a.MI_Id == data.MI_Id && c.IVRMMOD_Id == b.TENANTAGRPAY_ModeOfPayment && d.IVRM_Month_Id == b.TENANTAGRPAY_PaymentMonth)
                //                     select new TenantDashboardDTO
                //                     {
                //                         TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                //                         TENANTAGR_Id=a.TENANTAGR_Id,
                //                         IVRMMOD_ModeOfPayment = c.IVRMMOD_ModeOfPayment,
                //                         TENANTAGRPAY_PaymentDate = b.TENANTAGRPAY_PaymentDate,
                //                         TENANTAGRPAY_Amount = b.TENANTAGRPAY_Amount,
                //                         TENANTAGR_MonthlyRent=a.TENANTAGR_MonthlyRent,
                //                         TENANTAGRPAY_Remarks = b.TENANTAGRPAY_Remarks,
                //                         IVRM_Month_Name = d.IVRM_Month_Name,
                //                         TENANTAGRPAY_Id = b.TENANTAGRPAY_Id,
                //                         TENANTAGRPAY_Flag=b.TENANTAGRPAY_Flag,
                //                         TENANTAGRPAY_PaymentMonth=b.TENANTAGRPAY_PaymentMonth,
                //                         TENANTAGRPAY_PaymentYear=b.TENANTAGRPAY_PaymentYear



                //                     }).Distinct().OrderByDescending(v=>v.TENANTAGRPAY_Id).ToArray();


                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_array_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.tenant_array = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                data.bank_array = _portalDashboardContext.HR_Master_BankDeatils.Where(b => b.MI_Id == data.MI_Id).Distinct().ToArray();

                //data.TENANTAGR_Id = 0;
                //using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "tenant_overallfinedetails";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@HRMB_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.HRMB_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@HRMR_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.HRMR_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.TENANTAGR_Id
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
                //        data.get_tenant_details = retObject.ToArray();
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public TenantDashboardDTO onchangetype_offline(TenantDashboardDTO data)
        {
            try
            {
                //t.TENANTAGR_UserId == data.User_Id
                // var tenant_user = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true).ToList();
                data.get_tenant_details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true).ToArray();

                //  data.TENANTAGR_Id= _portalDashboardContext.TenantAgreement.Select(s=>s.TENANTAGR_Id).Where(t => t.TENANTAGR_UserId == data.User_Id && t.TENANTAGR_ActiveFlg == true);
                data.institutiondet = _portalDashboardContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                //for (var i = 0; i <= data.get_tenant_details.Length; i++)
                //{

                //    data.TENANTAGR_Id = data.get_tenant_details[i].TENANTAGR_Id;
                //}
                data.TENANTAGR_Id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_UserId == data.User_Id).Select(a => a.TENANTAGR_Id).FirstOrDefault();



                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_online_month_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
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



                data.fillpaymentgateway = (from a in _portalDashboardContext.PAYUDETAILS
                                           from b in _portalDashboardContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && b.FPGD_PGActiveFlag ==
                                           "1" && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_MobileActiveFlag == true)
                                           select new TenantDashboardDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }).ToArray();


                data.Payment_details = (from a in _portalDashboardContext.TenantPaymentDetailsDMO
                                        from b in _portalDashboardContext.TenantAgreement
                                        where (a.TENANTAGR_Id == b.TENANTAGR_Id && b.MI_Id == data.MI_Id && b.TENANTAGR_Id == data.TENANTAGR_Id && b.TENANTAGR_ActiveFlg == true)
                                        select new TenantDashboardDTO
                                        {
                                            TENANTAGR_AdvanceAmount = b.TENANTAGR_AdvanceAmount,
                                            TENANTAGR_MonthlyRent = b.TENANTAGR_MonthlyRent,
                                            TENANTAGRPAY_Amount = a.TENANTAGRPAY_Amount,
                                            TENANTAGRPAY_PaymentDate = a.TENANTAGRPAY_PaymentDate
                                        }).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public TenantDashboardDTO getfloor(TenantDashboardDTO data)
        {
            try
            {
                data.floor_array = (from a in _db.HR_Master_Building
                                    from b in _db.HR_Master_Floor
                                    where (a.MI_Id == data.MI_Id && a.HRMB_Id == b.HRMB_Id && b.HRMB_Id == data.HRMB_Id)
                                    select new TenantDashboardDTO
                                    {
                                        HRMF_FloorName = b.HRMF_FloorName,
                                        HRMF_Id = b.HRMF_Id

                                    }).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public TenantDashboardDTO getroom(TenantDashboardDTO data)
        {
            try
            {
                data.room_array = (from a in _db.HR_Master_Building
                                   from b in _db.HR_Master_Floor
                                   from c in _db.HR_Master_Room
                                   where (a.MI_Id == data.MI_Id && a.HRMB_Id == b.HRMB_Id && c.HRMF_Id == b.HRMF_Id && c.HRMF_Id == data.HRMF_Id && b.HRMB_Id == data.HRMB_Id)
                                   select new TenantDashboardDTO
                                   {
                                       HRMR_RoomName = c.HRMR_RoomName,
                                       HRMR_Id = c.HRMR_Id

                                   }).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public TenantDashboardDTO searchfilter(TenantDashboardDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();



                data.fillstudlist = (from a in _portalDashboardContext.TenantAgreement

                                     where (a.MI_Id == data.MI_Id && ((a.TENANTAGR_TenantName.Trim().ToUpper().Trim()).Contains(data.searchfilter)))
                                     select new TenantDashboardDTO
                                     {
                                         TENANTAGR_Id = a.TENANTAGR_Id,
                                         TENANTAGR_TenantName = a.TENANTAGR_TenantName

                                     }
         ).ToArray();


            }
            catch (Exception ex)
            {
                // _log.LogInformation("searcherror in studycertificate: '" + ex.Message + "'");
            }
            return data;
        }


        public TenantDashboardDTO getmonthdetails(TenantDashboardDTO data)
        {
            try
            {
                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_month_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
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
            catch (Exception ex)
            {
                // _log.LogInformation("searcherror in studycertificate: '" + ex.Message + "'");
            }
            return data;
        }


        public TenantDashboardDTO gettenantdetails(TenantDashboardDTO data)
        {
            try
            {

                //data.TENANTAGR_Id = 0;
                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMB_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMR_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRMR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
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
                        data.get_tenant_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                 //   cmd.CommandText = "tenant_paid_details";
                    cmd.CommandText = "TENANT_PAID_DETAILS_TEST";
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
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
                        data.get_tenant_paid_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                if (data.get_tenant_details.Length > 0)
                {
                    if(data.type=="Fine")
                    {
                        using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                        {
                            //cmd.CommandText = "tenant_month_details_bal";
                            cmd.CommandText = "tenantfine_month_details";

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                            {
                                Value = data.TENANTAGR_Id
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
                    else
                    {
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
                                Value = data.TENANTAGR_Id
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
                   
                }

            }
            catch (Exception ex)
            {
                // _log.LogInformation("searcherror in studycertificate: '" + ex.Message + "'");
            }
            return data;
        }
        public async Task<TenantDashboardDTO> savedetails(TenantDashboardDTO data)
        {
            string recno = "";
            int returnval = 0;
            long TENANTMWBILL_Id = 0;

            var std_monthly_amt= _portalDashboardContext.TenantAgreement.Where(s => s.TENANTAGR_Id == data.TENANTAGR_Id).Select(d=>d.TENANTAGR_MonthlyRent).Distinct().FirstOrDefault();
            // long TENANTEBWB_Id = 0;
            long hrmr_id = 0;
            try
            {
                if (data.TENANTAGRPAY_Id > 0)
                {
                    //TenantPaymentDetailsDMO pay = new TenantPaymentDetailsDMO();
                    //var duplicate = _portalDashboardContext.TenantPaymentDetailsDMO.Where(a => a.TENANTAGR_Id == data.TENANTAGR_Id && a.TENANTAGRPAY_PaymentMonth == data.TENANTAGRPAY_PaymentMonth).ToList();
                    //if (duplicate.Count > 0)
                    //{

                    var result = _portalDashboardContext.TenantPaymentDetailsDMO.Where(s => s.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id).Distinct().FirstOrDefault();
                    result.TENANTAGR_Id = data.TENANTAGR_Id;
                    result.TENANTAGRPAY_Amount = data.TENANTAGRPAY_Amount;
                    result.TENANTAGRPAY_Remarks = data.TENANTAGRPAY_Remarks;
                    result.TENANTAGRPAY_PaymentDate = data.TENANTAGRPAY_PaymentDate;
                    result.TENANTAGRPAY_UpdatedDate = DateTime.Now;
                    result.TENANTAGRPAY_UpdatedBy = data.User_Id;
                    _portalDashboardContext.Update(result);
                    //}
                }
                else
                {
                    if(data.type == "Advance")
                    {
                        TenantPaymentDetailsDMO pay = new TenantPaymentDetailsDMO();

                        if (data.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                        {
                            recno = data.TENANTAGRPAY_ReceiptNo;
                        }
                        else
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_portalDashboardContext);
                            data.transnumconfigsettings.MI_Id = data.MI_Id;
                            data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                            recno = a.GenerateNumberTenant(data.transnumconfigsettings);
                        }
                      
                        pay.TENANTAGR_Id = data.TENANTAGR_Id;
                        pay.TENANTAGRPAY_Amount = data.TENANTAGRPAY_Amount;
                        pay.TENANTAGRPAY_PaymentDate = data.TENANTAGRPAY_PaymentDate;
                        pay.TENANTAGRPAY_ModeOfPayment = data.TENANTAGRPAY_ModeOfPayment;
                        pay.TENANTAGRPAY_Remarks = data.TENANTAGRPAY_Remarks;
                        pay.TENANTAGRPAY_PaymentMonth = 0;
                        pay.TENANTAGRPAY_PaymentYear = 0;
                        pay.TENANTAGRPAY_Chequedate = data.TENANTAGRPAY_Chequedate;
                        pay.TENANTAGRPAY_Bankname = data.TENANTAGRPAY_Bankname;
                        pay.TENANTAGRPAY_Chequeno = data.TENANTAGRPAY_Chequeno;
                        //pay.TENANTAGRPAY_Bankname = data.TENANTAGRPAY_Bankname;
                        pay.TENANTAGRPAY_Balance = data.TENANTAGRPAY_Balance;
                        pay.TENANTAGRPAY_ActualFineAmount = 0;
                        pay.TENANTAGRPAY_ReceiptNo = recno;
                        pay.TENANTAGRPAY_Flag = data.type;
                        pay.TENANTAGRPAY_FinePaidAmount = 0;
                        pay.TENANTAGRPAY_CreatedDate = DateTime.Now;
                        pay.TENANTAGRPAY_UpdatedDate = DateTime.Now;
                        pay.TENANTAGRPAY_CreatedBy = data.User_Id;
                        pay.TENANTAGRPAY_UpdatedBy = data.User_Id;
                        _portalDashboardContext.Add(pay);
                    }

                    //if (data.type == "Fine")
                    //{
                    //    TenantPaymentDetailsDMO pay = new TenantPaymentDetailsDMO();

                    //    if (data.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                    //    {
                    //        recno = data.TENANTAGRPAY_ReceiptNo;
                    //    }
                    //    else
                    //    {
                    //        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_portalDashboardContext);
                    //        data.transnumconfigsettings.MI_Id = data.MI_Id;
                    //        data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                    //        recno = a.GenerateNumberTenant(data.transnumconfigsettings);
                    //    }
                    //    //var month_std_amt = _portalDashboardContext.TenantAgreement.Where(s => s.TENANTAGR_Id == data.TENANTAGR_Id).Select(m => m.TENANTAGR_MonthlyRent).FirstOrDefault();
                    //    //var prev_balamt = _portalDashboardContext.TenantPaymentDetailsDMO.Where(s => s.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id).Select(m => m.TENANTAGRPAY_Balance).Distinct().FirstOrDefault();
                    //    //if (prev_balamt > 0)
                    //    //{
                    //    //    data.TENANTAGRPAY_Amount = data.TENANTAGRPAY_Amount - Convert.ToDecimal(prev_balamt);
                    //    //}
                    //    //if (data.TENANTAGRPAY_Amount > month_std_amt)
                    //    //{
                    //    //    pay.TENANTAGRPAY_Excessamount = month_std_amt - data.TENANTAGRPAY_Amount;
                    //    //    pay.TENANTAGRPAY_Balance = 0;
                    //    //}
                    //    //else if (data.TENANTAGRPAY_Amount < month_std_amt)
                    //    //{
                    //    //    pay.TENANTAGRPAY_Balance = data.TENANTAGRPAY_Amount - month_std_amt;
                    //    //    pay.TENANTAGRPAY_Excessamount = 0;
                    //    //}
                    //    //else if (data.TENANTAGRPAY_Amount == month_std_amt)
                    //    //{
                    //    //    pay.TENANTAGRPAY_Amount = data.TENANTAGRPAY_Amount;
                    //    //    pay.TENANTAGRPAY_Excessamount = 0;
                    //    //    pay.TENANTAGRPAY_Balance = 0;

                    //    //}
                    //    pay.TENANTAGR_Id = data.TENANTAGR_Id;
                    //    pay.TENANTAGRPAY_Amount = 0;
                    //    pay.TENANTAGRPAY_PaymentDate = data.TENANTAGRPAY_PaymentDate;
                    //    pay.TENANTAGRPAY_ModeOfPayment = data.TENANTAGRPAY_ModeOfPayment;
                    //    pay.TENANTAGRPAY_Remarks = data.TENANTAGRPAY_Remarks;
                    //    pay.TENANTAGRPAY_PaymentMonth = 0;
                    //    pay.TENANTAGRPAY_PaymentYear = 0;
                    //    pay.TENANTAGRPAY_Chequedate = data.TENANTAGRPAY_Chequedate;
                    //    pay.TENANTAGRPAY_Bankname = data.TENANTAGRPAY_Bankname;
                    //    pay.TENANTAGRPAY_Chequeno = data.TENANTAGRPAY_Chequeno;
                    //    //pay.TENANTAGRPAY_Bankname = data.TENANTAGRPAY_Bankname;
                    //    pay.TENANTAGRPAY_ReceiptNo = recno;
                    //    pay.TENANTAGRPAY_Flag = data.type;
                    //    pay.TENANTAGRPAY_Balance = data.TENANTAGRPAY_Balance;
                    //    pay.TENANTAGRPAY_ActualFineAmount = data.TENANTAGRPAY_ActualFineAmount;
                    //    pay.TENANTAGRPAY_FinePaidAmount = data.TENANTAGRPAY_FinePaidAmount;
                    //    pay.TENANTAGRPAY_CreatedDate = DateTime.Now;
                    //    pay.TENANTAGRPAY_UpdatedDate = DateTime.Now;
                    //    pay.TENANTAGRPAY_CreatedBy = data.User_Id;
                    //    pay.TENANTAGRPAY_UpdatedBy = data.User_Id;
                    //    _portalDashboardContext.Add(pay);
                    //}
                    else
                    {
                        foreach (var month in data.Month_detailsarray)
                        {

                            hrmr_id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_Id == data.TENANTAGR_Id).Select(g => g.HRMR_Id).FirstOrDefault();

                            try
                            {
                                //if (data.type == "WaterBill")
                                //{
                                //    TENANTMWBILL_Id = (from a in _portalDashboardContext.Tenant_Monthly_WaterBillsDMO
                                //                       from b in _portalDashboardContext.Tenant_Monthly_WaterBills_RoomWiseDMO
                                //                       where (a.TENANTMWBILL_Id == b.TENANTMWBILL_Id && a.TENANTMWBILL_Month == month.MonthId && a.TENANTMWBILL_Year == month.TENANTAGRPAY_PaymentYear && b.HRMR_ID == hrmr_id)
                                //                       select new TenantDashboardDTO
                                //                       {
                                //                           TENANTMWBILL_Id = a.TENANTMWBILL_Id
                                //                       }).Select(a => a.TENANTMWBILL_Id).FirstOrDefault();
                                //    var TENANTMWBILLRW_Id = _portalDashboardContext.Tenant_Monthly_WaterBills_RoomWiseDMO.Where(s => s.TENANTMWBILL_Id == TENANTMWBILL_Id).FirstOrDefault();
                                //    //Tenant_Monthly_WaterBills_RoomWiseDMO waterdto = new Tenant_Monthly_WaterBills_RoomWiseDMO();
                                //    TENANTMWBILLRW_Id.TENANTMWBILLRW_AmountPaid = month.TENANTAGR_MonthlyRent;
                                //    TENANTMWBILLRW_Id.TENANTMWBILL_Id = TENANTMWBILL_Id;
                                //    TENANTMWBILLRW_Id.TENANTMWBILLRW_Remarks = data.TENANTAGRPAY_Remarks;
                                //    TENANTMWBILLRW_Id.TENANTMWBILLRW_PaidFlg = true;
                                //    TENANTMWBILLRW_Id.TENANTMWBILLRW_UpdatedBy = data.User_Id;
                                //    TENANTMWBILLRW_Id.TENANTMWBILLRW_UpdatedDate = DateTime.Now;
                                //    _portalDashboardContext.Tenant_Monthly_WaterBills_RoomWiseDMO.Update(TENANTMWBILLRW_Id);
                                //}

                               

                                TenantPaymentDetailsDMO pay = new TenantPaymentDetailsDMO();

                                if (data.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                                {
                                    recno = data.TENANTAGRPAY_ReceiptNo;
                                }
                                else
                                {
                                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_portalDashboardContext);
                                    data.transnumconfigsettings.MI_Id = data.MI_Id;
                                    data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                                    recno = a.GenerateNumberTenant(data.transnumconfigsettings);
                                }
                                //var month_std_amt = _portalDashboardContext.TenantAgreement.Where(s => s.TENANTAGR_Id == data.TENANTAGR_Id).Select(m => m.TENANTAGR_MonthlyRent).FirstOrDefault();
                                //var prev_balamt = _portalDashboardContext.TenantPaymentDetailsDMO.Where(s => s.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id).Select(m => m.TENANTAGRPAY_Balance).Distinct().FirstOrDefault();
                                //var excessamt = _portalDashboardContext.TenantPaymentDetailsDMO.Where(s => s.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id).Select(m => m.TENANTAGRPAY_Excessamount).Sum();
                                //if (prev_balamt > 0)
                                //{
                                //   if(excessamt>0)
                                //    {

                                //    }
                                //    data.TENANTAGRPAY_Amount = data.TENANTAGRPAY_Amount - Convert.ToDecimal(prev_balamt);
                                //   // data.TENANTAGRPAY_Amount

                                //}
                                //if (data.TENANTAGRPAY_Amount > month_std_amt)
                                //{
                                //    pay.TENANTAGRPAY_Excessamount = data.TENANTAGRPAY_Amount-month_std_amt;
                                //    pay.TENANTAGRPAY_Balance = 0;
                                // //   data.TENANTAGRPAY_Amount = data.TENANTAGRPAY_Amount- month_std_amt;
                                //}
                                //else if (data.TENANTAGRPAY_Amount < month_std_amt)
                                //{
                                //    pay.TENANTAGRPAY_Balance = month_std_amt- data.TENANTAGRPAY_Amount ;
                                //    pay.TENANTAGRPAY_Excessamount = 0;
                                //  //  data.TENANTAGRPAY_Amount = month_std_amt-data.TENANTAGRPAY_Amount;

                                //}
                                //else if (data.TENANTAGRPAY_Amount == month_std_amt)
                                //{
                                //    pay.TENANTAGRPAY_Amount = data.TENANTAGRPAY_Amount;
                                //    pay.TENANTAGRPAY_Excessamount = 0;
                                //    pay.TENANTAGRPAY_Balance = 0;

                                //}

                                pay.TENANTAGR_Id = data.TENANTAGR_Id;
                                pay.TENANTAGRPAY_Amount = data.TENANTAGRPAY_Amount;
                                pay.TENANTAGRPAY_PaymentDate = data.TENANTAGRPAY_PaymentDate;
                                pay.TENANTAGRPAY_ModeOfPayment = data.TENANTAGRPAY_ModeOfPayment;
                                pay.TENANTAGRPAY_Remarks = data.TENANTAGRPAY_Remarks;
                                pay.TENANTAGRPAY_PaymentMonth = month.MonthId;
                                pay.TENANTAGRPAY_PaymentYear = month.TENANTAGRPAY_PaymentYear;
                                pay.TENANTAGRPAY_Chequedate = data.TENANTAGRPAY_Chequedate;
                                pay.TENANTAGRPAY_Bankname = data.TENANTAGRPAY_Bankname;
                                pay.TENANTAGRPAY_Chequeno = data.TENANTAGRPAY_Chequeno;
                                pay.TENANTAGRPAY_ReceiptNo = recno;
                                pay.TENANTAGRPAY_Flag = data.type;
                                pay.TENANTAGRPAY_FinePaidAmount = data.TENANTAGRPAY_FinePaidAmount;
                                pay.TENANTAGRPAY_Balance = (std_monthly_amt + data.TENANTAGRPAY_ActualFineAmount) - data.TENANTAGRPAY_Amount;
                                pay.TENANTAGRPAY_ActualFineAmount = data.TENANTAGRPAY_ActualFineAmount;
                                pay.TENANTAGRPAY_Balance = data.TENANTAGRPAY_FinePaidAmount;
                                pay.TENANTAGRPAY_CreatedDate = DateTime.Now;
                                pay.TENANTAGRPAY_UpdatedDate = DateTime.Now;
                                pay.TENANTAGRPAY_CreatedBy = data.User_Id;
                                pay.TENANTAGRPAY_UpdatedBy = data.User_Id;
                                _portalDashboardContext.Add(pay);
                            }
                            //returnval = _portalDashboardContext.SaveChanges();
                            catch (Exception ee)
                            {

                         
                                data.returnval = "false";
                                Console.WriteLine(ee.Message);

                            }



                        }

                    }

                }
                returnval = _portalDashboardContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = "add";
                    //var tenantrecord = _portalDashboardContext.TenantAgreement.Where(i => i.TENANTAGR_Id == data.TENANTAGR_Id && i.MI_Id == data.MI_Id).ToList();
                    var tenantrecord = (from a in _portalDashboardContext.TenantAgreement
                                        from b in _portalDashboardContext.HR_Master_Building
                                        from c in _portalDashboardContext.HR_Master_Room
                                        where (a.HRMB_Id == b.HRMB_Id && a.HRMR_Id == c.HRMR_Id && b.HRMB_Id == c.HRMB_Id && a.TENANTAGR_Id == data.TENANTAGR_Id && a.MI_Id == data.MI_Id)
                                        select new TenantDashboardDTO {
                                            TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                            TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                            TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                            TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                            HRMB_BuildingName = b.HRMB_BuildingName,
                                            HRMR_RoomName = c.HRMR_RoomName,
                                            TENANTAGR_Address = a.TENANTAGR_Address
                                        }).ToList();

                    //data.returnval = "true";
                    //Email Email = new Email(_db);

                    //string m = Email.sendmailtenant(data.MI_Id, mail_id, "TenantMonthlyPayment", data.TENANTAGR_Id);
                    var MI_Id = data.MI_Id;
                    var agrfromdate = tenantrecord[0].TENANTAGR_AgrFromDate.ToString("dd-MM-yyyy");
                    foreach (var month in data.Month_detailsarray)
                    {
                        var month_name = _portalDashboardContext.IVRM_Month_DMO.Where(m => m.IVRM_Month_Id == month.MonthId).Select(m => m.IVRM_Month_Name).FirstOrDefault();

                        sendmailtransction("TENANTPAYMENTRECEIPT", tenantrecord[0].TENANTAGR_TenantName, agrfromdate, tenantrecord[0].HRMB_BuildingName, tenantrecord[0].HRMR_RoomName, data.TENANTAGRPAY_Amount, tenantrecord[0].TENANTAGR_Address, data.type, month_name, month.TENANTAGRPAY_PaymentYear,tenantrecord[0].TENANTAGR_EmailId, MI_Id);
                    }
                }
            }


            catch (Exception ee)
            {
                data.returnval = "false";
                Console.WriteLine(ee.Message);
                
            }

            return data;
        }


        public TenantDashboardDTO savefinedata(TenantDashboardDTO data)
        {

            int returnval = 0;

            try
            {
                if (data.TENANTAGRPAY_Id > 0)
                {
                    foreach (var month in data.Month_detailsarray)
                    {

                        var result = _portalDashboardContext.TenantPaymentDetailsDMO.Where(s => s.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id && s.TENANTAGR_Id == data.TENANTAGR_Id &&
                        s.TENANTAGRPAY_PaymentMonth == month.MonthId && s.TENANTAGRPAY_PaymentYear == month.TENANTAGRPAY_PaymentYear ).Distinct().FirstOrDefault();

                        result.TENANTAGR_Id = data.TENANTAGR_Id;
                        result.TENANTAGRPAY_ActualFineAmount = data.TENANTAGRPAY_ActualFineAmount - data.TENANTAGRPAY_FinePaidAmount;
                        result.TENANTAGRPAY_FinePaidAmount = data.TENANTAGRPAY_FinePaidAmount;
                        result.TENANTAGRPAY_Remarks = data.TENANTAGRPAY_Remarks;
                        result.TENANTAGRPAY_UpdatedDate = DateTime.Now;
                        result.TENANTAGRPAY_UpdatedBy = data.User_Id;
                        _portalDashboardContext.Update(result);
                    }
                    returnval = _portalDashboardContext.SaveChanges();

                    if(returnval > 0)
                    {
                        data.returnval = "add";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
                
               
            }
            catch (Exception ee)
            {
                data.returnval = "false";
                Console.WriteLine(ee.Message);

            }
            return data;
        }

        public string sendmailtransction(string Template, string tenantname, string agrfromdate,string building,string flat,decimal amount,string address,string type,string month,long year,string email, long mi_id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == mi_id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                Mailmsg = Mailmsg.Replace("[TENANTNAME]", tenantname);
                Mailmsg = Mailmsg.Replace("[STARTDATE]", agrfromdate);
                Mailmsg = Mailmsg.Replace("[BUILDING]", building);
                Mailmsg = Mailmsg.Replace("[FLAT]", flat);
                Mailmsg = Mailmsg.Replace("[ADDRESS]", address);
                Mailmsg = Mailmsg.Replace("[DESC]", type);
                Mailmsg = Mailmsg.Replace("[MONTHNAME]", month);
                Mailmsg = Mailmsg.Replace("[YEARNAME]", Convert.ToString(year));
                Mailmsg = Mailmsg.Replace("[AMOUNT]", Convert.ToString(amount));
               
                Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(mi_id)).ToList();

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
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(mi_id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
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
                        message.AddTo(email)
;


                        if (Attechement.Equals("1"))
                        {
                            var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }




                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)
;
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }




                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = email
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
                            Value = mi_id
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";


        }

        public TenantDashboardDTO GetSelectedRowDetails(TenantDashboardDTO data)
        {
            try
            {
                var details = (from a in _portalDashboardContext.TenantAgreement
                               from b in _portalDashboardContext.TenantPaymentDetailsDMO
                               from c in _portalDashboardContext.IVRM_Month_DMO
                               from d in _portalDashboardContext.IVRM_ModeOfPaymentDMO
                               from e in _portalDashboardContext.HR_Master_Building
                               from f in _portalDashboardContext.HR_Master_Room
                               where (a.MI_Id == data.MI_Id && b.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id && a.TENANTAGR_Id == b.TENANTAGR_Id && c.IVRM_Month_Id == b.TENANTAGRPAY_PaymentMonth && d.IVRMMOD_Id == b.TENANTAGRPAY_ModeOfPayment && b.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id && a.HRMB_Id == e.HRMB_Id && a.HRMR_Id == f.HRMR_Id)
                               select new TenantDashboardDTO
                               {
                                   TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                   TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                   TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                   TENANTAGRPAY_Amount = b.TENANTAGRPAY_Amount,
                                   TENANTAGRPAY_PaymentDate = b.TENANTAGRPAY_PaymentDate,
                                   TENANTAGRPAY_PaymentMonth = b.TENANTAGRPAY_PaymentMonth,
                                   TENANTAGRPAY_PaymentYear = b.TENANTAGRPAY_PaymentYear,
                                   IVRMMOD_Id = d.IVRMMOD_Id,
                                   TENANTAGRPAY_Id = b.TENANTAGRPAY_Id,
                                   TENANTAGRPAY_Remarks = b.TENANTAGRPAY_Remarks,
                                   HRMB_Id = a.HRMB_Id,
                                   HRMR_Id = a.HRMR_Id,
                                   HRMB_BuildingName = e.HRMB_BuildingName,
                                   HRMR_RoomName = f.HRMR_RoomName


                               }).Distinct().ToList();
                data.editdata = details.ToArray();


                var TENANTAGR_Id = _portalDashboardContext.TenantPaymentDetailsDMO.Where(a => a.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id).Select(a => a.TENANTAGR_Id).FirstOrDefault();
                // var list = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id).ToList();

                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_month_details_edit";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = TENANTAGR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGRPAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGRPAY_Id
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
                        data.get_month_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMB_Id", SqlDbType.VarChar)
                    {
                        Value = details.FirstOrDefault().HRMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMR_Id", SqlDbType.VarChar)
                    {
                        Value = details.FirstOrDefault().HRMR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = TENANTAGR_Id
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
                        data.get_tenant_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "tenant_paid_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
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
                        data.get_tenant_paid_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                //data.floor_array = (from a in _db.HR_Master_Building
                //                    from b in _db.HR_Master_Floor
                //                    where (a.MI_Id == data.MI_Id && a.HRMB_Id == b.HRMB_Id && b.HRMB_Id == details.FirstOrDefault().HRMB_Id)
                //                    select new TenantDashboardDTO
                //                    {
                //                        HRMF_FloorName = b.HRMF_FloorName,
                //                        HRMF_Id = b.HRMF_Id

                //                    }).ToArray();

                //data.room_array = (from a in _db.HR_Master_Building
                //                   from b in _db.HR_Master_Floor
                //                   from c in _db.HR_Master_Room
                //                   where (a.MI_Id == data.MI_Id && a.HRMB_Id == b.HRMB_Id && c.HRMF_Id == b.HRMF_Id && c.HRMF_Id == data.HRMF_Id && b.HRMB_Id == data.HRMB_Id)
                //                   select new TenantDashboardDTO
                //                   {
                //                       HRMR_RoomName = c.HRMR_RoomName,
                //                       HRMR_Id = c.HRMR_Id

                //                   }).ToArray();


            }
            catch (Exception ee)
            {

            }
            return data;
        }


        public TenantDashboardDTO viewuploadflies(TenantDashboardDTO data)
        {
            try
            {


                data.viewarray = (from a in _portalDashboardContext.TenantAgreement
                                  from b in _portalDashboardContext.TenantPaymentDetailsDMO
                                  from c in _portalDashboardContext.IVRM_Month_DMO
                                  where (b.TENANTAGRPAY_Id == data.TENANTAGRPAY_Id && a.TENANTAGR_Id == b.TENANTAGR_Id && c.IVRM_Month_Id == data.TENANTAGRPAY_PaymentMonth && b.TENANTAGRPAY_PaymentYear==data.TENANTAGRPAY_PaymentYear)
                                  select new TenantDashboardDTO
                                  {

                                      TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                      IVRM_Month_Name = c.IVRM_Month_Name,
                                      TENANTAGRPAY_Amount = b.TENANTAGRPAY_Amount,
                                      TENANTAGRPAY_PaymentDate = b.TENANTAGRPAY_PaymentDate,
                                      TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                      TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                      TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                      TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                      TENANTAGRPAY_ReceiptNo=b.TENANTAGRPAY_ReceiptNo,
                                      TENANTAGRPAY_Flag = b.TENANTAGRPAY_Flag
                                  }).Distinct().OrderBy(a => a.name).ToArray();

                data.MI_Logo = _portalDashboardContext.Institute.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Select(R => R.MI_Logo).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }


        public TenantDashboardDTO getfinedetails(TenantDashboardDTO data)
        {
            try
            {
                var month = "";
                var year = "";
                data.TENANTAGR_Id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_Id == data.TENANTAGR_Id).Select(a => a.TENANTAGR_Id).FirstOrDefault();
                if (data.MonthdetailsarrayBal != null)
                {
                    month = data.MonthdetailsarrayBal[0].MonthId.ToString();
                    year = data.MonthdetailsarrayBal[0].yearid.ToString();
                }
                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "Tenant_Monthly_Finecalculation";
                    cmd.CommandText = "Tenant_Monthly_Fine_calculation";
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@PAYMENTMONTH", SqlDbType.VarChar)
                    {
                        Value = month
                    });
                    cmd.Parameters.Add(new SqlParameter("@PAYMENTYEAR", SqlDbType.VarChar)
                    {
                        Value = year
                    });

                    cmd.Parameters.Add(new SqlParameter("@PAYMENTDATE", SqlDbType.VarChar)
                    {
                        Value = data.tenantagrpay_PaymentDate
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
                        data.get_fine_details = retObject.ToArray();
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


        public TenantDashboardDTO getbalncedetails(TenantDashboardDTO data)
        {
            try
            {
                //data.TENANTAGR_Id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_UserId == data.User_Id).Select(a => a.TENANTAGR_Id).FirstOrDefault();
                var month = "";
                var year = "";
                if (data.MonthdetailsarrayBal!=null )
                {
                    month = data.MonthdetailsarrayBal[0].MonthId.ToString();
                    year = data.MonthdetailsarrayBal[0].yearid.ToString();
                }

                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                  //  cmd.CommandText = "Tenant_Monthly_Balcalculation";
                    cmd.CommandText = "TENANT_MONTHLY_BALCALCULATION";
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.VarChar)
                    {
                        Value = month
                    });

                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.VarChar)
                    {
                        Value =year
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
                        data.get_balnce_details = retObject.ToArray();
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


        public TenantDashboardDTO getbreakup(TenantDashboardDTO data)
        {
            try
            {
                //data.TENANTAGR_Id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_UserId == data.User_Id).Select(a => a.TENANTAGR_Id).FirstOrDefault();
                var month = "";
                var year = "";
                if (data.MonthdetailsarrayBal != null)
                {
                    month = data.MonthdetailsarrayBal[0].MonthId.ToString();
                    year = data.MonthdetailsarrayBal[0].yearid.ToString();
                }

                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "Tenant_Monthly_breakup";
                   // cmd.CommandText = "Tenant_Monthly_breakup_test";
                    cmd.CommandText = "Tenant_Monthly_breakup";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.VarChar)
                    {
                        Value = month
                    });

                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.VarChar)
                    {
                        Value = year
                    });
                    cmd.Parameters.Add(new SqlParameter("@TotalPayble", SqlDbType.VarChar)
                    {
                        Value = data.total_payable
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
                        data.get_balnce_details = retObject.ToArray();
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


        public TenantDashboardDTO getMonthlyPaid_details(TenantDashboardDTO data)
        {
            try
            {
                //data.TENANTAGR_Id = _portalDashboardContext.TenantAgreement.Where(a => a.TENANTAGR_UserId == data.User_Id).Select(a => a.TENANTAGR_Id).FirstOrDefault();
                

                using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "Tenant_Monthly_breakup";
                    //cmd.CommandText = "tenant_monthly_paid_details";
                    cmd.CommandText = "TENANT_MONTHLY_PAID_DETAILS";
                    

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.TENANTAGR_Id
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
                        data.tenant_monthly_paid_details = retObject.ToArray();
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
