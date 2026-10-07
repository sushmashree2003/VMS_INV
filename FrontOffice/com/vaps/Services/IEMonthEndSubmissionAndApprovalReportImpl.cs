using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Services
{
    public class IEMonthEndSubmissionAndApprovalReportImpl : Interfaces.IEMonthEndSubmissionAndApprovalReportInterface
    {
        //public HRMSContext _context;
        public FOContext _cont;
        public IEMonthEndSubmissionAndApprovalReportImpl(FOContext _co)
        {

            _cont = _co;
        }

        public IEMonthEndSubmissionAndApprovalReportDTO Onload(IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            try
            {
         

                data.fillyear = (from a in _cont.HR_Master_LeaveYearDMO
                                where (a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                select new HR_Master_LeaveYearDTO
                                {
                                    HRMLY_Id = a.HRMLY_Id,
                                    HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                    HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

               
                data.fillmonth = _cont.Month.Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();

                data.intistutename = _cont.ISM_Master_Client_DMO.Where(t => t.ISMMCLT_ActiveFlag == true).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IEMonthEndSubmissionAndApprovalReportDTO getmonthreport(IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            try
            {
                List<long> IVRM_Month_Id = new List<long>();

                for (var i = 0; i < data.MONTHID.Length; i++)
                {
                    IVRM_Month_Id.Add(data.MONTHID[i].IVRM_Month_Id);
                }
                string monthid = string.Join(",", IVRM_Month_Id);

                List<long> ISMMCLT_Id = new List<long>();

                for (var i = 0; i < data.CLIENTID.Length; i++)
                {
                    ISMMCLT_Id.Add(data.CLIENTID[i].ISMMCLT_Id);
                }
                string clientid = string.Join(",", ISMMCLT_Id);

                using (var cmd = _cont.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IEMONTHEND_SUBMISSION_AND_APPROVAL_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar) { Value = clientid });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = data.yearid });
                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Id", SqlDbType.VarChar) { Value = monthid });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.rdbbutton });

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
                        data.iemonthendReport = retObject.ToArray();
                        
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
        public IEMonthEndSubmissionAndApprovalReportDTO DocView(IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            try
            {
                using (var cmd = _cont.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IEMONTHEND_SUBMISSION_AND_APPROVED_DOCUMENT_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMCLMER_Id", SqlDbType.VarChar) { Value = data.ISMCLMER_Id });
                    
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
                        data.MultidocView = retObject.ToArray();

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

        public IEMonthEndSubmissionAndApprovalReportDTO getIEDetails(IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            try
            {
                data.ielist = (from a in _cont.HR_Master_Employee_DMO
                               from b in _cont.ISM_Master_Client_IEMapping_DMO
                               from c in _cont.ISM_Master_Client_DMO
                               where (a.HRME_Id == b.ISMCIM_IEList && b.ISMMCLT_Id == c.ISMMCLT_Id && c.ISMMCLT_Id == data.ISMMCLT_Id)
                               select new MasterEmployeeDTO
                               {
                                  HRME_Id = a.HRME_Id,
                                  HRME_EmployeeFirstName = (a.HRME_EmployeeFirstName+' '+a.HRME_EmployeeMiddleName+' '+a.HRME_EmployeeLastName)
                               }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}

