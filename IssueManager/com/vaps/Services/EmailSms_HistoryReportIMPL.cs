using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
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
    public class EmailSms_HistoryReportIMPL : Interfaces.EmailSms_HistoryReportInterface
    {
        public IssueManagerContext _IssueContext;
        ILogger<EmailSms_HistoryDTO> _logger;
        public EmailSms_HistoryReportIMPL(IssueManagerContext _cont, ILogger<EmailSms_HistoryDTO> _log)
        {
            _IssueContext = _cont;
            _logger = _log;
        }
        public EmailSms_HistoryDTO getdetails(EmailSms_HistoryDTO data)
        {
            try
            {

                data.Institution = _IssueContext.Institution.Where(p => p.MI_ActiveFlag == 1).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //getReport
        public EmailSms_HistoryDTO getReport(EmailSms_HistoryDTO data)
        {
            try
            {
                List<string> ModuleName = new List<string>();

                if (data.uploadarray != null)
                {
                    foreach (var i in data.uploadarray)
                    {
                        ModuleName.Add(i.IVRMMM_ModuleName);

                    }
                }
              if(data.optionflag== "EMAIL")
                {
                  
                    data.getReport = _IssueContext.ivrm_email_sentbox.Where(p => p.MI_Id == data.MI_Id && ModuleName.Contains(p.Module_Name) && (p.Datetime >= data.FromDate && p.Datetime <= data.ToDate)).Distinct().ToArray();
                }
                if (data.optionflag == "SMS")
                {
                    data.getReport = _IssueContext.IVRM_sms_sentBoxDMO.Where(R => R.MI_Id == data.MI_Id && ModuleName.Contains(R.Module_Name) && (R.Datetime >= data.FromDate && R.Datetime <= data.ToDate)).Distinct().ToArray();
                }

                //string FromDate = data.FromDate.Value.ToString("yyyy-MM-dd");
                //string ToDate = data.ToDate.Value.ToString("yyyy-MM-dd");
                //using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "ISM_DepartmentList";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
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
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead 
                //                    );
                //                }

                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.getReport = retObject.ToArray();

                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //getmodule
        public EmailSms_HistoryDTO getmodule(EmailSms_HistoryDTO data)
        {
            try
            {
                data.Module = (from a in _IssueContext.Institution_Module
                               from b in _IssueContext.Institution_Module_Page
                               from c in _IssueContext.MasterModules
                               where (a.IVRMIM_Id == a.IVRMIM_Id && c.IVRMM_Id == b.IVRMIM_Id && c.Module_ActiveFlag == 1 && a.MI_Id == data.MI_Id)
                               select new EmailSms_HistoryDTO
                               {
                                   IVRMM_ModuleName = c.IVRMM_ModuleName,
                                   IVRMM_Id = c.IVRMM_Id

                               }
                              ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
