using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CoeServiceHub.com.vaps.Services
{
    public class COEReportImpl:Interfaces.COEReportInterface
    {
        public DomainModelMsSqlServerContext _db;
        public COEContext _coe;
        public FOContext _FOContext;
        public COEReportImpl(DomainModelMsSqlServerContext db, COEContext coe, FOContext FOContext)
        {
            _db = db;
            _coe = coe;
            _FOContext = FOContext;

        }
        public  async Task<COEReportDTO> getinitialData(int id)
        {
            COEReportDTO dto = new COEReportDTO();
            try
            {
                //var q = (from a in _FOContext.holidaydate
                //         where (a.MI_Id == id && a.FOMHWD_ActiveFlg == true)
                //         select new
                //         {
                //             monthid = a.FOMHWDD_FromDate.Value.Month,
                //             monthname = Convert.ToDateTime(a.FOMHWDD_FromDate).ToString("MMMMM").ToString()
                //         }).Distinct().ToArray();
                //var query = q.Distinct().ToArray();
                //dto.fillmonth = (from a in query
                //                 select new EmployeeLogReportDTO
                //                 {
                //                     monthid = a.monthid,
                //                     monthname = a.monthname
                //                 }).Distinct().OrderBy(t => t.monthid).ToArray();

                dto.modulenames = _db.masterModule.Distinct().ToArray();

                var q = (from a in _db.month
                         where (a.Is_Active == true)
                         select new
                         {
                             monthid = a.IVRM_Month_Id,
                             monthname = a.IVRM_Month_Name,
                         }).Distinct().ToArray();

                var query = q.Distinct().ToArray();
                dto.fillmonth = (from a in query
                                  select new EmployeeLogReportDTO
                                  {
                                      monthid = Convert.ToInt32(a.monthid),
                                      monthname = a.monthname
                                  }).Distinct().OrderBy(t => t.monthid).ToArray();


                dto.fillyear = (from a in _db.AcademicYear
                                where (a.MI_Id == id && a.ASMAY_ActiveFlag == 1)
                                select new HR_Master_LeaveYearDTO
                                {
                                    HRMLY_Id = Convert.ToInt32(a.ASMAY_Id),
                                    HRMLY_LeaveYear = a.ASMAY_Year

                                }
                    ).Distinct().ToArray();

                dto.ClassList = _db.School_M_Class.Where(d => d.MI_Id == id && d.ASMCL_ActiveFlag == true).
               Select(d => new AdmissionClass
               {
                   ASMCL_Id = d.ASMCL_Id,
                   ASMCL_ClassName = d.ASMCL_ClassName
               }).ToArray();

                return dto;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public COEReportDTO getReport(COEReportDTO data)
        {
            try
            {
                if (data.type.Equals("1"))
                {
                      
                    data.coereport = (from m in _coe.COE_Master_EventsDMO
                                      from n in _coe.COE_EventsDMO
                                      where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == data.month
                                      select new COEReportDTO{
                                          eventName=m.COEME_EventName,
                                          eventDesc=m.COEME_EventDesc,
                                          COEE_EStartDate=n.COEE_EStartDate,
                                          COEE_EEndDate=n.COEE_EEndDate
                                      }).ToArray();

                    if (data.coereport.Length > 0)
                    {
                        data.count = data.coereport.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }

                }
                else if(data.type.Equals("2"))
                {
                    data.coereport = (from m in _coe.COE_Master_EventsDMO
                                      from n in _coe.COE_EventsDMO
                                      from o in _coe.COE_Events_ClassesDMO
                                      where m.COEME_Id==n.COEME_Id && n.COEE_Id == o.COEE_Id && n.MI_Id==data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && o.ASMCL_Id==data.ASMCL_Id
                                      select new COEReportDTO
                                      {
                                          eventName = m.COEME_EventName,
                                          eventDesc = m.COEME_EventDesc,
                                          COEE_EStartDate = n.COEE_EStartDate,
                                          COEE_EEndDate = n.COEE_EEndDate
                                      }).ToArray();
                    if (data.coereport.Length > 0)
                    {
                        data.count = data.coereport.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                }

                data.institute = _db.Institute.Where(t => t.MI_Id == data.MI_Id && t.MI_ActiveFlag == 1).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public COEReportDTO mothreport(COEReportDTO data)
        {
            try
            {

               
                    data.coereport = (from m in _coe.COE_Master_EventsDMO
                                      from n in _coe.COE_EventsDMO
                                      where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.year && n.COEE_EStartDate.Value.Month == data.month
                                      select new COEReportDTO
                                      {
                                          eventName = m.COEME_EventName,
                                          eventDesc = m.COEME_EventDesc,
                                          COEE_EStartDate = n.COEE_EStartDate,
                                          COEE_EEndDate = n.COEE_EEndDate
                                      }).ToArray();

                    if (data.coereport.Length > 0)
                    {
                        data.count = data.coereport.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }

                data.eventDesc = _db.IVRM_Email_sentBox.Where(q => q.MI_Id == data.MI_Id && q.Datetime.Value.Month == data.month && q.Module_Name== "Calendar of Event").Count().ToString();

                data.eventName = _db.IVRM_sms_sentBoxDMO.Where(w => w.Datetime.Month == data.month && w.MI_Id == data.MI_Id && w.Module_Name == "Calendar of Event").Count().ToString();




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public COEReportDTO getcoesmsreport(COEReportDTO data)
        {
            try
            {
                var from_date = data.COEE_EStartDate.Value.Date.ToString("yyyy-MM-dd");

                var to_date = data.COEE_EEndDate.Value.Date.ToString("yyyy-MM-dd");

                string Modulename = "0";
                if (data.modulearray.Length > 0 && data.modulearray != null)
                {
                    foreach (var ue in data.modulearray)
                    {
                        Modulename = Modulename + "," + ue.IVRMM_Id;
                    }
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())                {

                    cmd.CommandText = "CoeModuleWise_SmsSentReport";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",                                SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMM_ModuleName",                                SqlDbType.VarChar)                    {                        Value = Modulename                    });
                    cmd.Parameters.Add(new SqlParameter("@From_date",                                SqlDbType.VarChar)                    {                        Value = from_date                    });
                    cmd.Parameters.Add(new SqlParameter("@To_date",                                SqlDbType.VarChar)                    {                        Value = to_date                    });
                    cmd.Parameters.Add(new SqlParameter("@type",                                SqlDbType.VarChar)                    {                        Value = data.type                    });

                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.coereport = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
