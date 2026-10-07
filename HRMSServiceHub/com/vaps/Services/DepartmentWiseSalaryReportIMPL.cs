using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
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
using HRMSServiceHub.com.vaps.Interfaces;

namespace HRMSServicesHub.com.vaps.Services
{
    public class DepartmentWiseSalaryReportIMPL : Interfaces.DepartmentWiseSalaryReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public DepartmentWiseSalaryReportIMPL(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public DepartmentWiseSalaryReportDTO getBasicData(DepartmentWiseSalaryReportDTO dto)
        {
            dto.get_mi_list1 = (from I in _HRMSContext.Institution
                                where I.MI_ActiveFlag == 1
                                select new DepartmentWiseSalaryReportDTO
                                {
                                    MI_Id = I.MI_Id,
                                    MI_Name = I.MI_Name
                                }).ToArray();


            dto.monthlist = _Context.month.Where(t => t.Is_Active == true).ToArray();
            dto.yearlist = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id==dto.MI_Id && t.HRMLY_ActiveFlag == true).ToArray();


            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public DepartmentWiseSalaryReportDTO GetAllDropdownAndDatatableDetails(DepartmentWiseSalaryReportDTO dto)
        {
            List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();

            List<HR_Master_EarningsDeductions> earningdeductiondatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistArrear = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductions> arreardatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductions> grossdatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistGross = new List<HR_Master_EarningsDeductionsDTO>();

            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();




            try
            {
                //  dto.LogInUserId = 362;

                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)


                               select pa
                          ).ToList();

                if (PROCESSList.Count() > 0)
                {

                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedetailList = EmployeeList.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();

                }
                else
                {

                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedetailList = EmployeeList.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();


                }

           

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        public DepartmentWiseSalaryReportDTO getdaily_data(DepartmentWiseSalaryReportDTO dto)
        {

           

            return dto;
        }


        public DepartmentWiseSalaryReportDTO getsalaryalldetails(DepartmentWiseSalaryReportDTO dto)
        {
            

            return dto;
        }


        public DepartmentWiseSalaryReportDTO get_depts(DepartmentWiseSalaryReportDTO data)
        {
            try
            {
                data.departmentdropdown = (from b in _HRMSContext.HR_Master_Department
                                           where ( data.MI_Idlist.Contains(b.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public DepartmentWiseSalaryReportDTO Getreport(DepartmentWiseSalaryReportDTO data)
        {
            try
            {
                string department = "0";
                for(var k=0;k<data.hrmD_IdList.Length; k++)
                {
                    department = department + "," + data.hrmD_IdList[k];
                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DeptwiseMonthlysalary";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.VarChar)
                    {
                        Value = data.IVRM_Month_Name
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.NVarChar)
                    {
                        Value = department
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
                        data.GetReportvalue = retObject1.ToArray();
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
