using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using DomainModel.Model.com.vapstech.FMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FMS;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Services
{
    public class Employee_Wise_VMS_FeedBackImpl :Interface.Employee_Wise_VMS_FeedBackInterface
    {
        public FMSContext _FMSContext;
        public Employee_Wise_VMS_FeedBackImpl(FMSContext FMS)
        {
            _FMSContext = FMS;
        }

        public Employee_Wise_VMS_FeedBackDTO getfeedback(Employee_Wise_VMS_FeedBackDTO data)
        {
            string currentMonth = DateTime.Now.ToString("MMMM");
            int currentMonthInt = DateTime.Now.Month;
             data.Feedbackdata = _FMSContext.Employee_Wise_VMS_Feedback.Where(F =>F.VEF_CreatedDate.Value.Month==currentMonthInt).ToArray();
            try
            {
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_EMP_WISE_FEEDBACK_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.VarChar) { Value = currentMonth });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.Feedbackdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Employee_FeedBack_employees";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.VarChar) { Value = currentMonth });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.employee_list = retObject.ToArray();
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
        public Employee_Wise_VMS_FeedBackDTO Savedata(Employee_Wise_VMS_FeedBackDTO dto)
        {
            try
            {
                if (dto.Feedback_list.Length > 0)
                {
                    foreach (var x in dto.Feedback_list)
                    {
                        Employee_Wise_VMS_Feedback EVF = new Employee_Wise_VMS_Feedback();
                        EVF.HRME_Id = x.HRME_Id;
                        EVF.VEF_Remarks = x.VEF_Remarks;
                        EVF.VEF_NegativeFlag = x.VEF_NegativeFlag;
                        EVF.VEF_FeedBackDate = DateTime.Now;
                        EVF.VEF_UpdatedDate = DateTime.Now;
                        EVF.VEF_CreatedBy = dto.userid.Value;
                        EVF.VEF_UpdatedBy = dto.userid.Value;
                        EVF.VEF_CreatedDate = DateTime.Now;
                        EVF.VEF_ActiveFlag = true;
                        _FMSContext.Add(EVF);
                    }
                    var res = _FMSContext.SaveChanges();

                    if (res >= 1)
                    {
                        dto.saved = "saved";
                    }
                    else
                    {
                        dto.saved = "failed";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        //Call center Data
        public HW_HeathCheckUpDTO GetMonthYear(HW_HeathCheckUpDTO data)
        {
            try
            {
                //data.Masteryear = _FMSContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true).Distinct().ToArray();

                data.Masteryear = _FMSContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true)
                                    .GroupBy(t => t.HRMLY_LeaveYear) 
                                    .Select(g => g.First()) 
                                    .ToArray();

                data.Monthnames = _FMSContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();

            }
           

             catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public HW_HeathCheckUpDTO employeefeedback(HW_HeathCheckUpDTO data)
        {

            var currentMonth = data.monthname;
            var year = data.year;
            var monthid = _FMSContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Name.Equals(currentMonth) && t.Is_Active).Select(t => t.IVRM_Month_Id).FirstOrDefault();


            try
            {
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HW_HealthCheckup_Feedback_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.VarChar) { Value = currentMonth });
                    cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.BigInt) { Value = year });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.getloaddata = retObject.ToArray();
                    
                    
                }

                data.feedbacklist = (from a in _FMSContext.ISM_Master_Client_DMO
                                   from c in _FMSContext.HW_HealthCheckUp_FeedbackDMO
                                   from d in _FMSContext.HW_HealthCheckUpDMO
                                     where (a.ISMMCLT_Id == c.ISMMCLT_Id && c.HWHCHUP_Id == d.HWHCHUP_Id && d.HWHCHUP_CreatedDate.Value.Month == monthid && d.HWHCHUP_CreatedDate.Value.Year == data.year && c.HWHCFB_ActiveFlag==true)
                                   select new CallcenterclientDto
                                   {
                                       VEF_Remarks = c.HWHCFB_Feedback,
                                       VEF_NegativeFlag = c.HWHCFB_SatisfiedFlg,
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       HWHCFB_CreatedDate =c.HWHCFB_CreatedDate
                                   }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HW_HeathCheckUpDTO savecallcenter(HW_HeathCheckUpDTO dto)
        {
            try
            {

                var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == dto.UserId).ToList();
                 if (getempid.Count > 0)
                {
                    var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();
                    dto.HRME_Id = empdetails[0].HRME_Id;
                }

                if (dto.CallFeedback_List.Length > 0)
                {

                    foreach (var x in dto.CallFeedback_List)
                   {
                        
                        HW_HealthCheckUp_FeedbackDMO CaData = new HW_HealthCheckUp_FeedbackDMO();
                        CaData.HRME_Id = dto.HRME_Id;
                        CaData.HWHCHUP_Id = x.HWHCHUP_Id;
                        CaData.HWHCFB_Feedback = x.VEF_Remarks;
                        CaData.HWHCFB_SatisfiedFlg = x.VEF_NegativeFlag;
                        CaData.HWHCFB_CreatedDate = DateTime.Now;
                        CaData.HWHCFB_UpdatedDate = DateTime.Now;
                        CaData.HWHCFB_CreatedBy = dto.UserId;
                        CaData.HWHCFB_UpdatedBy = dto.UserId;
                        CaData.HWHCFB_ActiveFlag = true;
                        CaData.ISMMCLT_Id = x.ISMMCLT_Id;
                      _FMSContext.Add(CaData);
                    }
                    var res = _FMSContext.SaveChanges();

                  if (res >= 1)
                    {
                        dto.returnmsg = "saved";
                    }
                    else
                    {
                        dto.returnmsg = "failed";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HW_HeathCheckUpDTO ViewHealthCheckUpFiles(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.getviewfilesdetails = (from a in _FMSContext.HW_HealthCheckUp_FilesDMO
                                            where (a.HWHCHUP_Id == data.HWHCHUP_Id)
                                            select new HW_HeathCheckUpDTO
                                            {
                                                HWHCHUP_Id = a.HWHCHUP_Id,
                                                HWHCHUPF_Id = a.HWHCHUPF_Id,
                                                filename = a.HWHCHUPF_FileName,
                                                filepath = a.HWHCHUPF_FilePath
                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }

}
