using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace IssueManager.com.vaps.Services
{
    public class CheckBookLogReportImpl : Interfaces.CheckBookLogReportInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;
        public CheckBookLogReportImpl(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;
            
        }

        public CheckBookLogReportDTO loaddata(CheckBookLogReportDTO data)
        {
            try
            {
                data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();                         
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CheckBookLogReportDTO onchange(CheckBookLogReportDTO data)
        {
            try
            {
                data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();
                data.Cheque = _IssueManagercontext.VMS_Payment_VoucherDMO.Where(a => a.MI_Id == data.MI_Id && a.VPAYVOU_ActiveFlg == true && a.HRMBD_Id==data.HRMBD_Id).Distinct().ToArray();           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CheckBookLogReportDTO Report(CheckBookLogReportDTO data)
        {
            try
            {
                string startdate = "";
                string enddate = "";
                if (data.startdate != null && data.enddate != null)
                {
                    startdate = data.startdate.Value.ToString("yyyy-MM-dd");
                    enddate = data.enddate.Value.ToString("yyyy-MM-dd");
                }
                string ChequeNo = "0";
                foreach (var ii in data.chequedetails)
                {
                    ChequeNo = ChequeNo + "," + ii.VPAYVOU_Id;
                }
                data.MI_Logo = _IssueManagercontext.Institution.Where(r => r.MI_Id == data.MI_Id).Select(r => r.MI_Logo).FirstOrDefault();
          


                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chequebook_Log_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@type",
                     SqlDbType.VarChar)
                    {
                        Value = data.type
                    });
                    cmd.Parameters.Add(new SqlParameter("@VPAYVOU_ChequeNo",
                      SqlDbType.VarChar)
                    {
                        Value = ChequeNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMBD_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRMBD_Id
                    });
                    //cmd.Parameters.Add(new SqlParameter("@startdate",
                    //     SqlDbType.VarChar)
                    //{
                    //    Value = startdate
                    //});
                    //cmd.Parameters.Add(new SqlParameter("@enddate",
                    //    SqlDbType.VarChar)
                    //{
                    //    Value = enddate
                    //});
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
                        data.allReport = retObject.ToArray();
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
    }
}