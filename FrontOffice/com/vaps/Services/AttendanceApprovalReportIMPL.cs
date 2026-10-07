using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Services
{
    public class AttendanceApprovalReportIMPL : AttendanceApprovalReportInterface
    {
        public FOContext _FOContext;

        public AttendanceApprovalReportIMPL(FOContext ttcntx)
        {
            _FOContext = ttcntx;

        }

        public AttendanceApprovalReportDTO getdata(AttendanceApprovalReportDTO data)
        {
            try
            {
                data.getmonth = _FOContext.Month.Where(t => t.Is_Active == true).ToArray();

                data.getyear = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.institutionlist = _FOContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AttendanceApprovalReportDTO report(AttendanceApprovalReportDTO data)
        {
            try
            {
                string mi_ids = "0";
                List<long> mid = new List<long>();

                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                    for (int i = 0; i < mid.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_AttendanceApproval_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.VarChar) { Value = data.HRMLY_LeaveYear });
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.VarChar) { Value = data.IVRM_Month_Name });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = data.HREAA_ApprovalFlg });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value= 0 });
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreport = retObject.ToArray();
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
