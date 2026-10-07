using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class VMS_OverallReport_Impl : Interfaces.VMSoverall_ReportInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;

        public VMS_OverallReport_Impl(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }
        public VmsoverallDTO getalldetails(VmsoverallDTO dto)
        {

            try
            {

                dto.MasterQualification = (from emp in _VMSContext.HR_Master_Course
                                           where emp.HRMC_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                           select new VmsoverallDTO
                                           {
                                               HRMC_Id = emp.HRMC_Id,
                                               HRMC_QulaificationName = emp.HRMC_QulaificationName
                                           }).ToArray();


                dto.MasterPositionList = (from emp in _VMSContext.HR_Master_PostionTypeDMO
                                          where emp.HRMPT_ActiveFlg == true && emp.MI_Id == dto.MI_Id
                                          select new VmsoverallDTO
                                          {
                                              HRMPT_Id = emp.HRMPT_Id,
                                              HRMPT_Name = emp.HRMPT_Name
                                          }).ToArray();
                dto.masterjob = (from a in _VMSContext.HR_Master_JobsDMO
                                 where a.HRMJ_ActiveFlg == true
                                 select new VmsoverallDTO
                                 {
                                     HRMJ_Id = a.HRMJ_Id,
                                     HRMJ_JobCode = a.HRMJ_JobCode,
                                     HRMJ_JobTiTle = a.HRMJ_JobTiTle
                                 }).ToArray();
                dto.masterReligion = _VMSContext.MasterReligionDMO.Where(t => t.Is_Active == true).ToArray();
                dto.Gender = _VMSContext.IVRM_Master_Gender.Where(p => p.IVRMMG_ActiveFlag == true && p.MI_Id == dto.MI_Id).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }
        public VmsoverallDTO getReport(VmsoverallDTO dto)
        {
            try
            {

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VmsOvearll";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRMC_Id", SqlDbType.VarChar)
                    {

                        Value = (dto.HRMC_Id.ToString() == "0" ? "" : dto.HRMC_Id.ToString())
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMPT_Id", SqlDbType.VarChar)
                    {
                        Value = (dto.HRMPT_Id.ToString() == "0" ? "" : dto.HRMPT_Id.ToString())
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMJ_Id", SqlDbType.VarChar)
                    {
                        Value = (dto.HRMJ_Id.ToString() == "0" ? "" : dto.HRMJ_Id.ToString())
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMR_Id ", SqlDbType.VarChar)
                    {
                        Value = (dto.HRCD_Religion.ToString() == "0" ? "" : dto.HRCD_Religion.ToString())
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMG_Id ", SqlDbType.VarChar)
                    {
                        Value = (dto.IVRMMG_Id.ToString() == "0" ? "" : dto.IVRMMG_Id.ToString())
                    });

                    string FromDate = "", todate = "";
                    if (dto.FROM_DATE != null && dto.TO_DATE != null)
                    {
                        FromDate = dto.FROM_DATE.Value.ToString("yyyy-MM-dd");
                        todate = dto.TO_DATE.Value.ToString("yyyy-MM-dd");
                    }


                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = todate });

                    cmd.Parameters.Add(new SqlParameter("@BELOW_CTC ", SqlDbType.Decimal)
                    {
                        Value = dto.BELOW_CTC
                    });
                    cmd.Parameters.Add(new SqlParameter("@HIGH_CTC ", SqlDbType.Decimal)
                    {
                        Value = dto.HIGH_CTC
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRCD_ExpFrom ", SqlDbType.Decimal)
                    {
                        Value = dto.HRCD_ExpFrom
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status ", SqlDbType.VarChar)
                    {
                        Value = dto.Recruitmentstatus
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null 
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }

                        dto.Get_DbArray = retObject.Distinct().ToArray();
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
            return dto;
        }
    }
}
