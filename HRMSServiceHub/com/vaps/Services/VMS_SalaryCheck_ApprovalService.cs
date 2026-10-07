using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class VMS_SalaryCheck_ApprovalService : Interfaces.VMS_SalaryCheck_ApprovalInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public VMS_SalaryCheck_ApprovalService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public HR_Employee_Salary_PaymentDetailsDTO getBasicData(HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public HR_Employee_Salary_PaymentDetailsDTO GetAllDropdownAndDatatableDetails(HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            try
            {
                //leave year
                dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).ToArray();
                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();

                //GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                //dto.groupTypedropdown = GroupTypelist.ToArray();

                ////Departmentlist
                //Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                //dto.departmentdropdown = Departmentlist.ToArray();

                ////Designationlist
                //Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                //dto.designationdropdown = Designationlist.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public async Task<HR_Employee_Salary_PaymentDetailsDTO> getEmployeedetailsBySelection(HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            List<HR_Employee_Salary> employeeSalaryDetails = new List<HR_Employee_Salary>();
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            try
            {
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                dto.empid = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_ActiveFlag == true).Select(t => t.HRME_Id).ToList();

                dto.empIds = String.Join(",", dto.empid);

                List<MasterEmployeeDTO> main_list = new List<MasterEmployeeDTO>();
                for (int i = 0; i < dto.empid.Count(); i++)
                {
                    MasterEmployeeDTO objemp = new MasterEmployeeDTO();

                    objemp.employeedetailList = (from a in _HRMSContext.MasterEmployee
                            from b in _HRMSContext.HR_Master_Department
                            from c in _HRMSContext.HR_Master_Designation
                            where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_Id == dto.empid[i])
                            select new MasterEmployeeDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                HRMD_Id = a.HRMD_Id,
                                HRMD_DepartmentName = b.HRMD_DepartmentName,
                                HRMDES_Id = a.HRMDES_Id,
                                HRMDES_DesignationName = c.HRMDES_DesignationName
                            }).ToArray();

                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmployeeSalarySlipGeneration";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.empid[i])
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.MI_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_LeaveYear",SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.HRMLY_LeaveYear)
                        });

                        cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Name",SqlDbType.VarChar)
                        {
                            Value = Convert.ToString(dto.IVRM_Month_Name)
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
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
                            objemp.employeeEarningsDeductionsDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if(objemp.employeeEarningsDeductionsDetails.Length > 0)
                    {
                        main_list.Add(objemp);
                    }                    
                }
                dto.main_list = main_list.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public HR_Employee_Salary_PaymentDetailsDTO saveapproval(HR_Employee_Salary_PaymentDetailsDTO data)
        {
            try
            {
                for (int i = 0; i < data.ApprovalTempDTO.Length; i++)
                {
                    long HRESId = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Year == data.HRMLY_LeaveYear && t.HRES_Month == data.IVRM_Month_Name && t.HRME_Id == data.ApprovalTempDTO[i].HRME_Id).Select(t => t.HRES_Id).FirstOrDefault();

                    HR_Employee_Salary_PaymentDetailsDMO obj = new HR_Employee_Salary_PaymentDetailsDMO();
                    var duplicate = _HRMSContext.HR_Employee_Salary_PaymentDetailsDMO.Where(t => t.HRES_Id == HRESId && t.HRME_Id == data.ApprovalTempDTO[i].HRME_Id && t.HRESPDET_Year == data.HRMLY_LeaveYear && t.HRESPDET_Month == data.IVRM_Month_Name).FirstOrDefault();

                    if (duplicate == null)
                    {
                        obj.HRES_Id = HRESId;
                        obj.HRME_Id = data.ApprovalTempDTO[i].HRME_Id;
                        obj.HRESPDET_Year = data.HRMLY_LeaveYear;
                        obj.HRESPDET_Month = data.IVRM_Month_Name;
                        obj.HRESPDET_Amount = data.ApprovalTempDTO[i].revisedSalary;
                        obj.HRESPDET_PaymentDate = DateTime.Today;
                        obj.HRESPDET_ModeOfPayment = "Bank";
                        obj.HRESPDET_Remarks = data.ApprovalTempDTO[i].HRESPDET_Remarks;
                        obj.HRESPDET_ActiveFlg = true;
                        obj.HRESPDET_CreatedDate = DateTime.Today;
                        obj.HRESPDET_CreatedBy = data.UserId;
                        obj.HRESPDET_UpdatedDate = DateTime.Today;
                        obj.HRESPDET_UpdatedBy = data.UserId;
                        obj.MI_Id = data.MI_Id;
                        obj.HRESPDET_Percentage = data.ApprovalTempDTO[i].HRESPDET_Percentage;
                        _HRMSContext.Add(obj);
                        int a = _HRMSContext.SaveChanges();
                    }
                    else
                    {
                        duplicate.HRESPDET_UpdatedBy = data.UserId;
                        duplicate.HRESPDET_UpdatedDate = DateTime.Today;
                        duplicate.HRESPDET_PaymentDate = DateTime.Today;
                        duplicate.HRESPDET_Amount = data.ApprovalTempDTO[i].revisedSalary;
                        duplicate.HRESPDET_Remarks = data.ApprovalTempDTO[i].HRESPDET_Remarks;
                        duplicate.HRESPDET_Percentage = data.ApprovalTempDTO[i].HRESPDET_Percentage;
                        _HRMSContext.Update(duplicate);
                        int a = _HRMSContext.SaveChanges();
                    }
                }
                data.retrunMsg = "Add";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                data.retrunMsg = "False";
            }
            return data;
        }

    }
}
