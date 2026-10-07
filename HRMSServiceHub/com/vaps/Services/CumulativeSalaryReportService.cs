using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class CumulativeSalaryReportService : Interfaces.CumulativeSalaryReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public CumulativeSalaryReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public HR_Employee_SalaryDTO GetAllDropdownAndDatatableDetails(HR_Employee_SalaryDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<Month> Monthlist = new List<Month>();

            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {
                //leave year
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                //leave year
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)
                               select pa).ToList();

                if (PROCESSList.Count() > 0)
                {
                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                    dto.companydetails = _HRMSContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();


                }
                else
                {


                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                    dto.companydetails = _HRMSContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();


                }



                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_SalaryAttandanceHoldList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                  SqlDbType.VarChar)
                    {
                        Value = "SalaryNotApproved"
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
        SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
                  SqlDbType.BigInt)
                    {
                        Value = dto.LogInUserId
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.employeeDetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                dto.getemployee = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();


                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public async Task<HR_Employee_SalaryDTO> getEmployeedetailsBySelection(HR_Employee_SalaryDTO dto)
        {
            List<CumulativeSalaryReportDTO> cumDTOList = new List<CumulativeSalaryReportDTO>();
            CumulativeSalaryReportDTO cumDTO = new CumulativeSalaryReportDTO();
            List<HR_Employee_Salary> employeSalary = new List<HR_Employee_Salary>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {

                //  Inatitution Details

                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;


                //  Employee Salary Details

                employeSalary = (from a in _HRMSContext.MasterEmployee
                                 from b in _HRMSContext.HR_Employee_Salary
                                 where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                                 && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && a.HRME_ActiveFlag == true
                                 select b).Distinct().ToList();
                if (employeSalary.Count > 0)
                {

                    ///

                    PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                   from pa in _HRMSContext.HR_PROCESSDMO
                                   from cc in _HRMSContext.Staff_User_Login
                                   where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId && pa.HRPA_TypeFlag == "Salary")
                                   select pa
                       ).ToList();

                    if (PROCESSList.Count() > 0)
                    {
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) || dto.hrmdeS_IdList.Contains(t.HRMDES_Id) || dto.hrmD_IdList.Contains(t.HRMD_Id) || dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();


                    }
                    else
                    {

                        if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }

                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }

                    }

                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    var optionsBuilder = new DbContextOptionsBuilder<HRMSContext>();
                     optionsBuilder.UseSqlServer("Data Source=dcampus.database.windows.net,1433;Initial Catalog=VMS_2022-12-23T10-33Z;Persist Security Info=False;User ID=decampus;Password=Digit@lc@mpu$@1;Connection Timeout=30;");
                   // optionsBuilder.UseSqlServer("Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=VMS;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;");

                    var hashSet = new HashSet<HR_Employee_Salary>(employeSalary);
                    for (int x = 0; x < hashSet.Count; x++)
                    {
                        var CurrentHRME_Id = hashSet.ElementAt(x);
                        Task tTemp = Task.Run(() =>
                        {

                            decimal Lopdays = 0;
                            decimal LopAmount = 0;
                            //LOP Calculation

                            var _db = new HRMSContext(optionsBuilder.Options);





                            var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans
                                          from B in _HRMSContext.HR_Master_Leave
                                          from C in _HRMSContext.HR_Emp_Leave_Trans_Details
                                          where (B.HRML_Id == A.HRELT_LeaveId &&
                                          A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == CurrentHRME_Id.HRME_Id &&
                                          A.HRELT_ActiveFlag == true && C.HRELT_Id == A.HRELT_Id && C.HRELTD_LWPFlag == true && A.HRELT_LeaveReason == "LWP Cumulative Report"
                                          && ((A.HRELT_FromDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_FromDate <= CurrentHRME_Id.HRES_ToDate)
                                          || (A.HRELT_ToDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_ToDate <= CurrentHRME_Id.HRES_ToDate))
                                          )
                                          select A).ToList();
                            if (LOPcal.Count() > 0)
                            {
                                Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);

                                LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(CurrentHRME_Id.HRES_DailyRates);
                            }
                            else
                            {
                                Lopdays = 0;
                            }

                            //Employee salary Details for particular month

                            cumDTO = (from HRES in _db.HR_Employee_Salary
                                      from HRME in _db.MasterEmployee
                                      from HRESD in _db.HR_Employee_Salary_Details
                                      from hrdes in _db.HR_Master_Designation
                                      from hrgrd in _db.HR_Master_Grade
                                      where (HRME.HRME_Id == HRES.HRME_Id
                                      && HRES.HRES_Id == CurrentHRME_Id.HRES_Id
                                      && HRESD.HRES_Id == HRES.HRES_Id
                                      && hrdes.HRMDES_Id == HRES.HRMDES_Id
                                      && hrgrd.HRMG_Id == HRME.HRMG_Id//checking condition
                                      )
                                      select new CumulativeSalaryReportDTO
                                      {
                                          HRES_Id = HRES.HRES_Id,
                                          HRME_Id = HRME.HRME_Id,
                                          HRESD_Id = HRESD.HRESD_Id,
                                          HRES_WorkingDays = HRES.HRES_WorkingDays,
                                          HRES_FromDate = HRES.HRES_FromDate,
                                          HRES_ToDate = HRES.HRES_ToDate,
                                          LOPDays = Lopdays,
                                          HRME_EmployeeFirstName = HRME.HRME_EmployeeFirstName,
                                          HRME_EmployeeMiddleName = HRME.HRME_EmployeeMiddleName,
                                          HRME_EmployeeLastName = HRME.HRME_EmployeeLastName,
                                          HRME_EmployeeCode = HRME.HRME_EmployeeCode,
                                          HRME_EmployeeOrder = HRME.HRME_EmployeeOrder,
                                          HRMDES_Designationname = hrdes.HRMDES_DesignationName,
                                          HRMG_GradeName = hrgrd.HRMG_GradeName,
                                          HRMG_ORDER = hrgrd.HRMG_Order,
                                          HRMD_Id = HRME.HRMD_Id
                                      }).FirstOrDefault();


                            var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                              from mas in _HRMSContext.HR_Master_EarningsDeductions
                                              where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == CurrentHRME_Id.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id && mas.HRMED_EDTypeFlag == "Basic Pay")
                                              select new CumulativeSalaryReportDTO
                                              {
                                                  HRESD_Amount = emp.HREED_Amount

                                              }).ToList();

                            cumDTO.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));

                            var grosspayhead = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                                from mas in _HRMSContext.HR_Master_EarningsDeductions
                                                where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == CurrentHRME_Id.HRME_Id && mas.HRMED_EarnDedFlag == "Gross" && mas.MI_Id == dto.MI_Id)
                                                select new CumulativeSalaryReportDTO
                                                {
                                                    HRESD_Amount = emp.HREED_Amount

                                                }).ToList();

                            if (grosspayhead.Count() > 0)
                            {

                                cumDTO.grosspayhead = Convert.ToDecimal(grosspayhead.Sum(t => t.HRESD_Amount));

                            }

                            else { }

                            List<HR_Employee_Salary_DetailsDTO> alldata = new List<HR_Employee_Salary_DetailsDTO>();


                            if (cumDTO != null)
                            {
                                //Employee earning Deduction Details

                                var allhead = _db.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).OrderBy(t => t.HRMED_Order).ToList();

                                if (allhead.Count() > 0)
                                {

                                    foreach (var head in allhead)
                                    {
                                        Task tTemp1 = Task.Run(() =>
                                        {
                                            HR_Employee_Salary_DetailsDTO ss = new HR_Employee_Salary_DetailsDTO();

                                            if (!head.HRMED_Name.Equals("") && head.HRMED_Name != null)
                                            {
                                                List<HR_Employee_Salary_DetailsDTO> currentdata = new List<HR_Employee_Salary_DetailsDTO>();
                                                if (dto.comm == "1")
                                                {
                                                    currentdata = (from HRES in _db.HR_Employee_Salary
                                                                   from HRESD in _db.HR_Employee_Salary_Details
                                                                   from HRMED in _db.HR_Master_EarningsDeductions

                                                                   where (HRESD.HRES_Id == CurrentHRME_Id.HRES_Id &&
                                                                            HRESD.HRES_Id == HRES.HRES_Id &&
                                                                            HRESD.HRMED_Id == HRMED.HRMED_Id &&
                                                                            HRESD.HRMED_Id == head.HRMED_Id && HRES.HRES_ApproveFlg == false
                                                                            ) //checking condition
                                                                   select new HR_Employee_Salary_DetailsDTO
                                                                   {

                                                                       HRESD_Id = HRESD.HRESD_Id,
                                                                       HRES_Id = HRES.HRES_Id,
                                                                       HRMED_Id = HRESD.HRMED_Id,
                                                                       HRMED_Name = HRMED.HRMED_Name,
                                                                       HRESD_Amount = HRESD.HRESD_Amount,
                                                                       HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                                                   }).ToList();
                                                }
                                                else
                                                {
                                                    currentdata = (from HRES in _db.HR_Employee_Salary
                                                                   from HRESD in _db.HR_Employee_Salary_Details
                                                                   from HRMED in _db.HR_Master_EarningsDeductions

                                                                   where (HRESD.HRES_Id == HRES.HRES_Id &&
                                                                            HRESD.HRMED_Id == HRMED.HRMED_Id &&
                                                                            HRESD.HRES_Id == CurrentHRME_Id.HRES_Id
                                                                             &&
                                                                            HRESD.HRMED_Id == head.HRMED_Id) //checking condition
                                                                   select new HR_Employee_Salary_DetailsDTO
                                                                   {

                                                                       HRESD_Id = HRESD.HRESD_Id,
                                                                       HRES_Id = HRES.HRES_Id,
                                                                       HRMED_Id = HRESD.HRMED_Id,
                                                                       HRMED_Name = HRMED.HRMED_Name,
                                                                       HRESD_Amount = HRESD.HRESD_Amount,
                                                                       HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                                                   }).ToList();
                                                }

                                                string monthname = dto.HRES_Month.Trim();
                                                long month = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(monthname) + 1;
                                                string monathname = month.ToString();
                                                var drblockeddetails = _db.VMSConsolidateReportTempDMO.Where(t => t.VMSCRT_EMPCode == cumDTO.HRME_EmployeeCode && t.VMSCRT_Year == dto.HRES_Year && t.VMSCRT_Month == monathname).ToList();
                                                cumDTO.VMSCRT_BDailyReport = drblockeddetails.Count() > 0 ? Convert.ToString(drblockeddetails[0].VMSCRT_BDailyReport / 2) : "0";
                                                if (currentdata.Count() > 0)
                                                {

                                                    decimal? HRESD_Amount = currentdata.Sum(t => t.HRESD_Amount);


                                                    ss.HRMED_Id = head.HRMED_Id;
                                                    ss.HRMED_Name = currentdata.FirstOrDefault().HRMED_Name;
                                                    ss.HRESD_Id = currentdata.FirstOrDefault().HRESD_Id;
                                                    ss.HRESD_Amount = Math.Round(Convert.ToDecimal(HRESD_Amount), 2);
                                                    ss.HRMED_EarnDedFlag = currentdata.FirstOrDefault().HRMED_EarnDedFlag;

                                                    alldata.Add(ss);

                                                }
                                                else
                                                {

                                                    ss.HRMED_Id = head.HRMED_Id;
                                                    ss.HRMED_Name = head.HRMED_Name;
                                                    ss.HRESD_Amount = 0;
                                                    ss.HRMED_EarnDedFlag = head.HRMED_EarnDedFlag;
                                                    ss.HRESD_Id = cumDTO.HRESD_Id;



                                                    alldata.Add(ss);

                                                }
                                            }

                                        });
                                        tTemp1.Wait();

                                    }


                                }

                                var earningresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Earning")).ToArray();
                                cumDTO.earningresult = earningresult;

                                cumDTO.grossEarning = 0;
                                foreach (var grsearn in cumDTO.earningresult)
                                {

                                    decimal? HRESD_Amount = 0;

                                    if (grsearn.HRESD_Amount != null)
                                    {
                                        HRESD_Amount = grsearn.HRESD_Amount;
                                    }
                                    else
                                    {
                                        HRESD_Amount = 0;
                                    }

                                    cumDTO.grossEarning = Math.Round(Convert.ToDecimal(cumDTO.grossEarning + HRESD_Amount), 2);


                                }
                                var deductionresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction")).ToArray();
                                cumDTO.deductionresult = deductionresult;

                                cumDTO.grossDeduction = 0;
                                foreach (var grossDeduction in cumDTO.deductionresult)
                                {

                                    decimal? HRESD_Amount = 0;

                                    if (grossDeduction.HRESD_Amount != null)
                                    {
                                        HRESD_Amount = grossDeduction.HRESD_Amount;
                                    }
                                    else
                                    {
                                        HRESD_Amount = 0;
                                    }

                                    cumDTO.grossDeduction = Math.Round(Convert.ToDecimal(cumDTO.grossDeduction + HRESD_Amount), 2);
                                }

                                if (PayrollStandard.HRC_PayMethodFlg.Equals("Method1"))
                                {
                                    // cumDTO.netSalary = Math.Round(Convert.ToDecimal((cumDTO.grossEarning - cumDTO.grossDeduction) - LopAmount), 0);
                                    cumDTO.netSalary = Math.Round(Convert.ToDecimal(cumDTO.grossEarning - cumDTO.grossDeduction), 2);
                                }
                                else
                                {
                                    cumDTO.netSalary = Math.Round(Convert.ToDecimal(cumDTO.grossEarning - cumDTO.grossDeduction), 2);
                                }


                                cumDTOList.Add(cumDTO);
                            }

                        });
                        tTemp.Wait();
                    }

                    dto.employeeSalaryslipDetails = cumDTOList.OrderBy(t => t.HRME_EmployeeOrder).ToArray();




                }


                else
                {
                    dto.employeeSalaryslipDetails = cumDTOList.ToArray();
                }

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id  && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Employee_SalaryDTO getapprvaldata(HR_Employee_SalaryDTO data)
        {
            try
            {

                data.earninganddeduction = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == data.institutionID && t.HRMED_ActiveFlag == true).ToArray();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Salary_ApprovalEmplist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                    {
                        Value = data.LogInUserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.NVarChar)
                    {
                        Value = data.HRES_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.NVarChar)
                    {
                        Value = data.HRES_Month
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.NVarChar)
                    {
                        Value = data.institutionID
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
                        data.employeeSalaryslipDetails = retObject.ToArray();
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

        public HR_Employee_SalaryDTO SalaryApproval(HR_Employee_SalaryDTO data)
        {
            try
            {
                int count = 0;
                if (data.selectedEmpdetails.Length > 0 && data.selectedEmpdetails != null)
                {
                    foreach (salaryUpdationDTO item in data.selectedEmpdetails)
                    {

                        var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("SaveSalaryApproval @p0,@p1,@p2,@p3", data.LogInUserId, item.HRES_Id, item.HRESA_Remarks, item.retrunMsg);

                        if (contactExistsP > 0)
                        {
                            data.retrunMsg = "updated";
                        }
                    }
                }

                //if (data.comm == "SendMail")
                //{
                    var sactionlevel = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(t => t.IVRMUL_Id == data.LogInUserId).Select(t => t.HRPAON_SanctionLevelNo).FirstOrDefault();
                    var finallevel = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(t => t.IVRMUL_Id == data.LogInUserId).Select(t => t.HRPAON_FinalFlg).FirstOrDefault();



                    // var salarydetails = _HRMSContext.HR_Emp_Salary_ApprovalDMO.Where(t => HRESID.Contains(t.HRES_Id) && t.IVRMUL_Id == data.LogInUserId).ToList();

                    var salaryApprovedDetails = (from a in _HRMSContext.HR_Employee_Salary
                                                 from b in _HRMSContext.MasterEmployee
                                                 from c in _HRMSContext.HR_Emp_Salary_ApprovalDMO
                                                 where (a.MI_Id==data.MI_Id && a.HRES_Id==c.HRES_Id && a.HRES_Year==data.HRES_Year && a.HRES_Month==data.HRES_Month && a.HRME_Id == b.HRME_Id && c.HRESA_ApprovalFlg == true && c.IVRMUL_Id == data.LogInUserId)
                                                 select new salaryUpdationDTO
                                                 {
                                                     HRME_EmployeeCode = b.HRME_EmployeeCode,
                                                     HRME_EmployeeFirstName = b.HRME_EmployeeFirstName,
                                                     HRESA_Remarks = c.HRESA_Remarks
                                                 }).ToList();

                    var salaryHoldDetails = (from a in _HRMSContext.HR_Employee_Salary
                                             from b in _HRMSContext.MasterEmployee
                                             from c in _HRMSContext.HR_Emp_Salary_ApprovalDMO
                                             where (a.MI_Id == data.MI_Id && a.HRES_Id == c.HRES_Id  && a.HRES_Year == data.HRES_Year && a.HRES_Month == data.HRES_Month &&  a.HRME_Id == b.HRME_Id && c.HRESA_ApprovalFlg == false && c.IVRMUL_Id == data.LogInUserId)
                                             select new salaryUpdationDTO
                                             {
                                                 HRME_EmployeeCode = b.HRME_EmployeeCode,
                                                 HRME_EmployeeFirstName = b.HRME_EmployeeFirstName,
                                                 HRESA_Remarks = c.HRESA_Remarks
                                             }).ToList();

                    string salaryhold = "";
                    int rownumber = 0;
                    if (salaryHoldDetails.Count > 0)
                    {
                    salaryhold += "<table width=" + "'600px'" + " style=" + "'text-align: center;line-height:2;border:0;cellspacing:0;cellpadding: 0;background-color:#F5F5F5'" + ">" +
                   "<tr>";
                    salaryhold += "<th colspan=4 >On Hold</th></tr>";
                    salaryhold += "<tr>";
                    salaryhold += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>"+ "SL.NO." +"</th>";
                    salaryhold += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Employee Code" + "</th>";
                    salaryhold += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Employee Name" + "</th>";
                    salaryhold += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Remark" + "</th>";
                    salaryhold += "</tr>";
                    for (int j = 0; j < salaryHoldDetails.Count; j++)
                        {
                            rownumber += 1;
                            salaryhold += "<tr>" +
                                "<td align=" + "center" + " style=" + ">" + rownumber + "</td> " +
                                "<td align=" + "left" + ">" + salaryHoldDetails[j].HRME_EmployeeCode + "</td>" +
                                "<td align=" + "left" + ">" + salaryHoldDetails[j].HRME_EmployeeFirstName + "</td>" +
                                "<td align=" + "left" + ">" + salaryHoldDetails[j].HRESA_Remarks + "</td> " +
                                "</tr>";
                        }
                    salaryhold += "</table>";
                }

                
                

               


                string salaryapproved = "";
                    int rownumber1 = 0;
                    if (salaryApprovedDetails.Count > 0)
                    {
                    salaryapproved += "<table width=" + "'600px'" + " style=" + "'text-align: center;line-height:2;border:0;cellspacing:0;cellpadding: 0;background-color:#F5F5F5'" + ">" +
                    "<tr>";
                    salaryapproved += "<th colspan=4 >Approved</th>";
                    salaryapproved += "<tr>";
                    salaryapproved += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'> " + "SL.NO." + "</th>";
                    salaryapproved += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Employee Code" + "</th>";
                    salaryapproved += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Employee Name" + "</th>";
                    salaryapproved += "<th style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Remark" + "</th>";
                    salaryapproved += "</tr>";
                    for (int j = 0; j < salaryApprovedDetails.Count; j++)
                        {
                            rownumber1 += 1;
                            salaryapproved += "<tr>" +
                                "<td align=" + "center" + " style=" + ">" + rownumber1 + "</td> " +
                                "<td align=" + "left" + ">" + salaryApprovedDetails[j].HRME_EmployeeCode + "</td>" +
                                "<td align=" + "left" + ">" + salaryApprovedDetails[j].HRME_EmployeeFirstName + "</td>" +
                                "<td align=" + "left" + ">" + salaryApprovedDetails[j].HRESA_Remarks + "</td> " +
                                "</tr>";
                        }
                    salaryapproved += "</table>";
                }


                    var emailid = "";
                    var toemployee = "";
                    var ccemailid1 = "";
                    var ccemailid2 = "";
                    var ccemailid = "";
                    if (finallevel == false)
                    {
                        emailid = (from a in _HRMSContext.HR_PROCESSDMO
                                   from b in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                   from c in _HRMSContext.Staff_User_Login
                                   from d in _HRMSContext.Emp_Email_Id
                                   where (a.HRPA_TypeFlag == "Salary" && a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == c.Id && c.Emp_Code == d.HRME_Id && d.HRMEM_DeFaultFlag == "default" && b.HRPAON_SanctionLevelNo == sactionlevel + 1)
                                   select d).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                        toemployee = (from a in _HRMSContext.HR_PROCESSDMO
                                      from b in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                      from c in _HRMSContext.Staff_User_Login
                                      from d in _HRMSContext.MasterEmployee
                                      where (a.HRPA_TypeFlag == "Salary" && a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == c.Id && c.Emp_Code == d.HRME_Id && b.HRPAON_SanctionLevelNo == sactionlevel + 1)
                                      select d).Select(t => t.HRME_EmployeeFirstName).FirstOrDefault();

                        ccemailid1 = (from a in _HRMSContext.ApplicationUserRoleDMO
                                      from b in _HRMSContext.Staff_User_Login
                                      from c in _HRMSContext.Emp_Email_Id
                                      from d in _HRMSContext.masterRoleType
                                      where (d.IVRMRT_RoleFlag == "Accountant" && a.UserId == b.Id && b.Emp_Code == c.HRME_Id && a.RoleTypeId == d.IVRMRT_Id)
                                      select c).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                        ccemailid2 = (from a in _HRMSContext.HR_Master_DepartmentCodeDMO
                                      from b in _HRMSContext.HR_Master_DepartmentCode_Head
                                      from c in _HRMSContext.Emp_Email_Id
                                      where (a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.HRME_Id && a.HRMDC_Code == "HR" && c.HRMEM_DeFaultFlag == "default")
                                      select c).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                        ccemailid = ccemailid1 + "," + ccemailid2;

                    }
                    else
                    {

                        emailid = (from a in _HRMSContext.ApplicationUserRoleDMO
                                   from b in _HRMSContext.Staff_User_Login
                                   from c in _HRMSContext.Emp_Email_Id
                                   from d in _HRMSContext.masterRoleType
                                   where (d.IVRMRT_RoleFlag == "Accountant" && a.UserId == b.Id && b.Emp_Code == c.HRME_Id && a.RoleTypeId == d.IVRMRT_Id)
                                   select c).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                        toemployee = (from a in _HRMSContext.ApplicationUserRoleDMO
                                      from b in _HRMSContext.Staff_User_Login
                                      from c in _HRMSContext.MasterEmployee
                                      from d in _HRMSContext.masterRoleType
                                      where (d.IVRMRT_RoleFlag == "Accountant" && a.UserId == b.Id && b.Emp_Code == c.HRME_Id && a.RoleTypeId == d.IVRMRT_Id)
                                      select c).Select(t => t.HRME_EmployeeFirstName).FirstOrDefault();

                        ccemailid = (from a in _HRMSContext.HR_Master_DepartmentCodeDMO
                                     from b in _HRMSContext.HR_Master_DepartmentCode_Head
                                     from c in _HRMSContext.Emp_Email_Id
                                     where (a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.HRME_Id && a.HRMDC_Code == "HR" && c.HRMEM_DeFaultFlag == "default")
                                     select c).Select(t => t.HRMEM_EmailId).FirstOrDefault();
                    }


                    if (emailid.Length > 0)
                    {
                        sendmail(data.MI_Id, emailid, ccemailid, data.HRES_Year, data.HRES_Month, toemployee, sactionlevel, salaryhold, salaryapproved, "SalaryApproval");
                    }


                //}














                if (count > 0)
                {
                    data.retrunMsg = "updated";

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Employee_SalaryDTO SalaryApprovalgriddata(HR_Employee_SalaryDTO data)
        {
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetsalaryApprovedGridData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                    {
                        Value = data.LogInUserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.NVarChar)
                    {
                        Value = data.HRES_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.NVarChar)
                    {
                        Value = data.HRES_Month
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.NVarChar)
                    {
                        Value = data.institutionID
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
                        data.salaryapprovedgridview = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetsalaryholdGridData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                    {
                        Value = data.LogInUserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.NVarChar)
                    {
                        Value = data.HRES_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.NVarChar)
                    {
                        Value = data.HRES_Month
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.NVarChar)
                    {
                        Value = data.institutionID
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
                        data.salaryholddgridview = retObject.ToArray();
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

        public HR_Employee_SalaryDTO report(HR_Employee_SalaryDTO data)
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


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Salaryapproval_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                      SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                     SqlDbType.VarChar)
                    {
                        Value = data.HRES_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                    SqlDbType.VarChar)
                    {
                        Value = data.HRES_Month
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = data.radiotype
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.GetReport = retObject.ToArray();
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



        public string sendmail(long MI_Id, string Email, string Emailcc, string Year, string month, string nextapprovar, int sactionlevel, string salaryhold, string salaryapproved, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                Mailmsg = Mailmsg.Replace("[LOGO]", institutionName[0].MI_Logo);
                Mailmsg = Mailmsg.Replace("[Level]", sactionlevel.ToString());
                Mailmsg = Mailmsg.Replace("[CompanyName]", institutionName[0].MI_Name);
                Mailmsg = Mailmsg.Replace("[YEAR]", Year);
                Mailmsg = Mailmsg.Replace("[MONTH]", month);
                Mailmsg = Mailmsg.Replace("[NAME]", nextapprovar);
                Mailmsg = Mailmsg.Replace("[SALARYHOLD]", salaryhold);
                Mailmsg = Mailmsg.Replace("[SALARYAPPROVED]", salaryapproved);


                DateTime date = DateTime.Now;
                DateTime date1 = DateTime.Parse(date.ToString());
                Mailmsg = Mailmsg.Replace("[DATE]", date1.ToString("dd/MM/yyyy"));



                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
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
                                mailbcc = "";
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
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                    if (Emailcc != null && Emailcc != "")
                    {
                        message.AddCc(Emailcc);
                    }

                    //if (mailcc != null && mailcc != "")
                    //{
                    //    message.AddCc(mailcc);
                    //}
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
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
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }



    }
}
