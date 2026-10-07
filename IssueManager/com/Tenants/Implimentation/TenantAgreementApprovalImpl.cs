using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.Rating;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IssueManager.com.Tenants.Implimentation
{
    public class TenantAgreementApprovalImpl : Interface.TenantAgreementApprovalInterface
    {
        public IssueManagerContext _context;
        public TenantAgreementApprovalImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }
        public TenantAgreementApprovalDTO loaddata(TenantAgreementApprovalDTO data)
        {
            try
            {

                data.getinstitution = _context.Institution.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TenantAgreementApprovalDTO getdatas(TenantAgreementApprovalDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantAgreement_ReportProc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }
                        data.viewlist = retObject.ToArray();

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


        public TenantAgreementApprovalDTO savedetails(TenantAgreementApprovalDTO data)
        {
            try
            {
                if (data.grid_arrays != null && data.grid_arrays.Length > 0)
                {
                    data.TENANTAGRAPR_FApprovedBy = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                    foreach (var i in data.grid_arrays)

                    {
                        data.TENANTAGRAPR_ApprovedFlg = false;
                        if (i.TENANTAGRAPR_ApprovedFlg == 1)
                        {
                            data.TENANTAGRAPR_ApprovedFlg = true;
                        }
                        else
                        {
                            data.TENANTAGRAPR_ApprovedFlg = false;
                        }

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Tenant_Approval_Insert";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id",
                            SqlDbType.BigInt)
                            {
                                Value = i.TENANTAGR_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.TENANTAGRAPR_FApprovedBy
                            });
                            cmd.Parameters.Add(new SqlParameter("@ApprovedFlag",
                            SqlDbType.Bit
                            )
                            {
                                Value = data.TENANTAGRAPR_ApprovedFlg
                            });
                            
                            cmd.Parameters.Add(new SqlParameter("@Remark",
                            SqlDbType.VarChar
                            )
                            {
                                Value = i.TENANTAGRAPR_Remarks
                            });
                            cmd.Parameters.Add(new SqlParameter("@User_Id",
                           SqlDbType.BigInt)
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
                                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                                                       );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.viewlist = retObject.ToArray();
                                data.returnval = "Add";

                            }


                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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


    }
}
