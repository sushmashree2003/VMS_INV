using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.MobileApp;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.Portals.Student;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeePtalImpl : Interfaces.EmployeePtalInterface
    {
        public HRMSContext _hrms;
        public TTContext _tt;
        public ExamContext _exm;
        public FOContext _FOContext;
        public COEContext _COEContext;
        public PortalContext _PortalContext;
        public EmployeePtalImpl(HRMSContext hrms, TTContext tt, FOContext fOContext, ExamContext exm, COEContext COEContext, PortalContext portalContext)
        {
            _hrms = hrms;
            _tt = tt;
            _COEContext = COEContext;
            _FOContext = fOContext;
            _exm = exm;
            _PortalContext = portalContext;
        }
        public EmployeeDashboardDTO getdata(EmployeeDashboardDTO dto)
        {
            try
            {

                var emp_Id = _exm.Staff_User_Login.Where(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Distinct().ToList();
                dto.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;

                var currentyear = DateTime.Now.Year;
                //currentyear = 2017;
                dto.salarylist = (from m in _hrms.HR_Employee_Salary
                                  from n in _hrms.HR_Employee_Salary_Details
                                  from o in _hrms.HR_Master_EarningsDeductions
                                  where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && m.HRES_Year == Convert.ToString(currentyear) && o.HRMED_EarnDedFlag.Equals("Earning")
                                  group new { m, n, o }
                                    by new { m.HRES_Month } into g
                                  select new EmployeeDashboardDTO
                                  {
                                      salary = g.Sum(d => d.n.HRESD_Amount),
                                      monthName = g.FirstOrDefault().m.HRES_Month,

                                  }
                               ).ToArray();

                //dto.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                //                      from b in _FOContext.HR_Master_Department_DMO
                //                      from c in _FOContext.HR_Master_Designation_DMO
                //                      from d in  _FOContext.Multiple_Mobile_DMO
                //                      from e in _FOContext.Emp_Email_Id
                //                      where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && d.HRME_Id==a.HRME_Id && a.HRME_Id==e.HRME_Id && e.HRMEM_DeFaultFlag=="default" && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && d.HRMEMNO_DeFaultFlag== "default")
                //                      select new EmployeeDashboardDTO
                //                      {
                //                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                          HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                //                          HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                //                          HRME_DOJ = a.HRME_DOJ,
                //                          HRMD_DepartmentName = b.HRMD_DepartmentName,
                //                          HRMDES_DesignationName = c.HRMDES_DesignationName,
                //                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                //                          HRME_MobileNo = d.HRMEMNO_MobileNo,
                //                          HRME_EmailId = e.HRMEM_EmailId,
                //                          HRME_DOB = a.HRME_DOB,
                //                          HRME_PhotoNo = a.HRME_Photo,
                //                      }).Distinct().ToArray();


                dto.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                      from b in _FOContext.HR_Master_Department_DMO
                                      from c in _FOContext.HR_Master_Designation_DMO

                                      where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id)
                                      select new EmployeeDashboardDTO
                                      {
                                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                          HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                          HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                          HRME_DOJ = a.HRME_DOJ,
                                          HRMD_DepartmentName = b.HRMD_DepartmentName,
                                          HRMDES_DesignationName = c.HRMDES_DesignationName,
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          HRME_DOB = a.HRME_DOB,
                                          HRME_PhotoNo = a.HRME_Photo,
                                      }).Distinct().ToArray();

                dto.mobile = (from a in _hrms.Emp_MobileNo
                              where (a.HRME_Id == dto.HRME_Id && a.HRMEMNO_DeFaultFlag == "default")
                              select new EmployeeDashboardDTO
                              {
                                  HRME_MobileNo = a.HRMEMNO_MobileNo,
                              }).Distinct().ToArray();


                dto.email = (from a in _hrms.Emp_Email_Id

                             where (a.HRME_Id == dto.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                             select new EmployeeDashboardDTO
                             {
                                 HRME_EmailId = a.HRMEM_EmailId,
                             }).Distinct().ToArray();

                dto.TT_final_generation = (from a in _tt.TT_Final_GenerationDMO
                                           from b in _tt.TT_Final_Generation_DetailedDMO
                                           from c in _tt.School_M_Class
                                           from d in _tt.School_M_Section
                                           from e in _tt.TT_Master_PeriodDMO
                                           from f in _tt.TT_Master_DayDMO

                                           where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                           select new
                                           {
                                               DayName = f.TTMD_DayName,
                                               PeriodCount = e.TTMP_PeriodName.Count()


                                           }).Distinct().GroupBy(f => f.DayName).Select(g => new EmployeeDashboardDTO { DayName = g.Key, PeriodCount = g.Count() }).ToArray();

                dto.allperiods = _tt.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag == true && c.MI_Id == dto.MI_Id).ToArray();

                DateTime now = DateTime.Now;
                int A = now.Month;
                dto.coedata = (from m in _COEContext.COE_Master_EventsDMO
                               from n in _COEContext.COE_EventsDMO
                               where m.COEME_Id == n.COEME_Id && n.MI_Id == dto.MI_Id && n.ASMAY_Id == dto.ASMAY_Id && n.COEE_EStartDate.Value.Month == A
                               select new EmployeeDashboardDTO
                               {
                                   eventName = m.COEME_EventName,
                                   eventDesc = m.COEME_EventDesc,
                                   COEE_EStartDate = n.COEE_EStartDate,
                                   COEE_EEndDate = n.COEE_EEndDate,
                               }).ToArray();


                var clstchname = (from a in _PortalContext.ClassTeacherMappingDMO
                                  from e in _PortalContext.HR_Master_Employee_DMO
                                  where (a.HRME_Id == e.HRME_Id && a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == dto.HRME_Id)
                                  select new EmployeeDashboardDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      ASMCL_Id = a.ASMCL_Id,
                                      ASMS_Id = a.ASMS_Id,

                                      HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                  }).Distinct().ToList();


                if(clstchname!=null && clstchname.Count() > 0)

                {
                    dto.fillstudent = (from a in _PortalContext.AdmissionStudentDMO
                                       from b in _PortalContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == clstchname[0].ASMCL_Id && b.ASMS_Id == clstchname[0].ASMS_Id)
                                       select new EmployeeDashboardDTO
                                       {
                                           Amst_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                           amst_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                       }

                ).OrderBy(t => t.AMST_FirstName).ToArray();


                    if (dto.fillstudent.Length > 0)
                    {
                        dto.studentcount = dto.fillstudent.Length;
                    }
                }
      

             

                #region  CALENDER 
                try
                {
                    dto.calenderlist = (from m in _PortalContext.COE_Master_EventsDMO
                                        from n in _PortalContext.COE_EventsDMO
                                        from o in _PortalContext.School_Adm_Y_StudentDMO
                                        where (m.COEME_Id == n.COEME_Id && n.MI_Id == dto.MI_Id && o.ASMAY_Id == dto.ASMAY_Id && n.COEE_EStartDate != null)
                                        select new EmployeeDashboardDTO
                                        {
                                            COEME_EventName = m.COEME_EventName,
                                            COEME_EventDesc = m.COEME_EventDesc,
                                            COEE_EStartDate = n.COEE_EStartDate,
                                            COEE_EEndDate = n.COEE_EEndDate,
                                            COEE_ReminderDate = n.COEE_ReminderDate
                                        }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion

                #region MobileapppagePrivileges

                List<ApplicationUserRole> user = new List<ApplicationUserRole>();

                user = _PortalContext.ApplicationUserRole.Where(g => g.UserId == dto.UserId).ToList();

                if (user.Count() > 0)
                {

                    List<IVRM_Role_MobileApp_Privileges> Staffmobileappprivileges = new List<IVRM_Role_MobileApp_Privileges>();
                    Staffmobileappprivileges = _PortalContext.IVRM_Role_MobileApp_Privileges.Where(f => f.IVRMRT_Id == dto.roleid && f.MI_ID == dto.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {

                        dto.Staffmobileappprivileges = (from Mobilepage in _PortalContext.IVRM_MobileApp_Page
                                                        from MobileRolePrivileges in _PortalContext.IVRM_Role_MobileApp_Privileges
                                                        from UserRolePrivileges in _PortalContext.IVRM_User_MobileApp_Login_Privileges
                                                        where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == dto.roleid && MobileRolePrivileges.MI_ID == dto.MI_Id && UserRolePrivileges.IVRMUL_Id == dto.UserId)
                                                        select new LoginDTO
                                                        {
                                                            Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                            Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                            Pageurl = Mobilepage.IVRMMAP_AppPageURL
                                                        }).ToArray();

                        dto.mobileprivileges = "true";
                    }
                    else
                    {
                        dto.mobileprivileges = "false";
                    }
                }



                #endregion

                #region Mobileversion
                using (var cmdv = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdv.CommandText = "Versioncheck";
                    cmdv.CommandType = CommandType.StoredProcedure;

                    cmdv.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmdv.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar, 50)
                    {
                        Value = "Staff"
                    });
                    cmdv.Parameters.Add(new SqlParameter("@version",
               SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    });


                    if (cmdv.Connection.State != ConnectionState.Open)
                        cmdv.Connection.Open();

                    var data1 = cmdv.ExecuteNonQuery();
                    // x.FSS_FineAmount += Convert.ToDecimal(cmd.Parameters["@amt"].Value);
                    dto.version = cmdv.Parameters["@version"].Value.ToString();
                }

                #endregion
                #region MObiledeviceid
                var stureg = _PortalContext.HR_Master_Employee_DMO.Single(s => s.HRME_Id == dto.HRME_Id);
                stureg.HRME_AppDownloadedDeviceId = dto.mobiledeviceid;
                _PortalContext.Update(stureg);
                _PortalContext.SaveChanges();
                #endregion
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public EmployeeDashboardDTO saveakpkfile(EmployeeDashboardDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                IVRM_MobileApp_Download_DMO objdata = new IVRM_MobileApp_Download_DMO();


                objdata.MI_Id = data.MI_Id;
                objdata.IVRMUL_Id = data.IVRMUL_Id;
                objdata.IVRMMAD_MobileModel = "Staff";
                objdata.IVRMMAD_DownlaodDateTime = indianTime;
                objdata.CreatedDate = indianTime;
                objdata.UpdatedDate = indianTime;

                _PortalContext.Add(objdata);
                int rowAffected = _PortalContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




        public EmployeeDashboardDTO Mobileversion_control(EmployeeDashboardDTO data)        {            try            {                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "MobileAppVersioncontrol";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",             SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@Version",                     SqlDbType.VarChar)                    {                        Value = data.version                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.versiondetailslist = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }

    }
}
