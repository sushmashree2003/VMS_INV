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


namespace HRMSServiceHub.com.vaps.Services
{
    public class HREmployeeLoanApprovalImpl : Interfaces.HREmployeeLoanApprovalInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HREmployeeLoanApprovalImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public HREmployeeLoanApprovalDTO getBasicData(HREmployeeLoanApprovalDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public HREmployeeLoanApprovalDTO GetAllDropdownAndDatatableDetails(HREmployeeLoanApprovalDTO dto)
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


                var hrpaid = _HRMSContext.HR_PROCESSDMO.Where(t => t.HRPA_TypeFlag == "Loan").Select(t => t.HRPA_Id).FirstOrDefault();

                dto.finalflag = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(t => t.IVRMUL_Id == dto.LogInUserId && t.HRPA_Id == hrpaid).Select(t => t.HRPAON_FinalFlg).FirstOrDefault();

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Loan_ApprovalEmplist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                    {
                        Value = dto.LogInUserId
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.NVarChar)
                    {
                        Value = dto.MI_Id
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
                        dto.employeeSalaryslipDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetsalaryApprovedGridData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                    {
                        Value = dto.LogInUserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.NVarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.NVarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.NVarChar)
                    {
                        Value = ""
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
                        dto.salaryapprovedgridview = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                dto.getemployee = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();


                //// employee grouptype
                //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                ////departmentdropdown
                //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                ////designationdropdown 
                //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

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




        public HREmployeeLoanApprovalDTO getapprvaldata(HREmployeeLoanApprovalDTO data)
        {
            try
            {
                var hrpaid = _HRMSContext.HR_PROCESSDMO.Where(t => t.HRPA_TypeFlag == "Loan" && t.MI_Id == data.institutionID).Select(t => t.HRPA_Id).FirstOrDefault();

                data.finalflag = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(t => t.IVRMUL_Id == data.LogInUserId && t.HRPA_Id == hrpaid).Select(t => t.HRPAON_FinalFlg).FirstOrDefault();

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Loan_ApprovalEmplist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                    {
                        Value = data.LogInUserId
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

        public HREmployeeLoanApprovalDTO SalaryApproval(HREmployeeLoanApprovalDTO data)
        {
            try
            {
                List<long> hrelid = new List<long>();

                var Apptype = _HRMSContext.HR_PROCESSDMO.Where(t => t.HRPA_TypeFlag == "Loan").ToList();

                var sactionlevel = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(t => t.IVRMUL_Id == data.LogInUserId && t.HRPA_Id == Apptype[0].HRPA_Id).Select(t => t.HRPAON_SanctionLevelNo).FirstOrDefault();
                var finallevel = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(t => t.IVRMUL_Id == data.LogInUserId && t.HRPA_Id == Apptype[0].HRPA_Id).Select(t => t.HRPAON_FinalFlg).FirstOrDefault();



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
                               where (a.HRPA_TypeFlag == "Loan" && a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == c.Id && c.Emp_Code == d.HRME_Id && d.HRMEM_DeFaultFlag == "default" && b.HRPAON_SanctionLevelNo == sactionlevel + 1)
                               select d).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    toemployee = (from a in _HRMSContext.HR_PROCESSDMO
                                  from b in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                  from c in _HRMSContext.Staff_User_Login
                                  from d in _HRMSContext.MasterEmployee
                                  where (a.HRPA_TypeFlag == "Loan" && a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == c.Id && c.Emp_Code == d.HRME_Id && b.HRPAON_SanctionLevelNo == sactionlevel + 1)
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
                               where (d.IVRMRT_RoleFlag == "Loan" && a.UserId == b.Id && b.Emp_Code == c.HRME_Id && a.RoleTypeId == d.IVRMRT_Id)
                               select c).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    toemployee = (from a in _HRMSContext.ApplicationUserRoleDMO
                                  from b in _HRMSContext.Staff_User_Login
                                  from c in _HRMSContext.MasterEmployee
                                  from d in _HRMSContext.masterRoleType
                                  where (d.IVRMRT_RoleFlag == "Loan" && a.UserId == b.Id && b.Emp_Code == c.HRME_Id && a.RoleTypeId == d.IVRMRT_Id)
                                  select c).Select(t => t.HRME_EmployeeFirstName).FirstOrDefault();

                    ccemailid = (from a in _HRMSContext.HR_Master_DepartmentCodeDMO
                                 from b in _HRMSContext.HR_Master_DepartmentCode_Head
                                 from c in _HRMSContext.Emp_Email_Id
                                 where (a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.HRME_Id && a.HRMDC_Code == "HR" && c.HRMEM_DeFaultFlag == "default")
                                 select c).Select(t => t.HRMEM_EmailId).FirstOrDefault();
                }



                int count = 0;
                if (data.selectedEmpdetails.Length > 0 && data.selectedEmpdetails != null)
                {
                    foreach (salaryUpdationDTO item in data.selectedEmpdetails)
                    {

                        hrelid.Add(item.HRES_Id);
                        var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("SaveLoanApproval @p0,@p1,@p2,@p3,@p4", data.LogInUserId, item.HRES_Id, item.HRESA_Remarks, item.retrunMsg, item.HREL_SanctionedAmount);

                        var hrmeid = _HRMSContext.HR_Emp_Loan.Where(t => t.HREL_Id == item.HRES_Id).ToList();

                        var ccemail = _HRMSContext.Emp_Email_Id.Where(t => t.HRME_Id == hrmeid[0].HRME_Id).Select(t => t.HRMEM_EmailId).FirstOrDefault();


                        if (contactExistsP > 0)
                        {

                            if (item.retrunMsg == "Approved")
                            {
                                if (finallevel == true)
                                {
                                    sendmailcertificate(data.MI_Id, "LOANREQUESTFINAL", item.HRME_Id, ccemail, " ");

                                }
                                else
                                {


                                    sendmailcertificate(data.MI_Id, "LOANREQUESTAPPROVED", item.HRME_Id, emailid, ccemail);


                                }

                            }
                            else
                            {
                                sendmailcertificate(data.MI_Id, "LOANREJECTED", item.HRME_Id, emailid, ccemail);

                            }


                            data.retrunMsg = "updated";
                        }
                    }
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

        public HREmployeeLoanApprovalDTO SalaryApprovalgriddata(HREmployeeLoanApprovalDTO data)
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

        public HREmployeeLoanApprovalDTO report(HREmployeeLoanApprovalDTO data)
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

        public HREmployeeLoanApprovalDTO ViewFiles(HREmployeeLoanApprovalDTO data)
        {
            data.getviewfilesdetails = (from a in _HRMSContext.HR_Emp_Loan_Documents
                                        from b in _HRMSContext.HR_Master_Documents
                                        where (a.HRMDOC_Id == b.HRMDOC_Id && a.HRELD_ActiveFlag == true
                                        && a.HREL_ID == data.HREL_ID)
                                        select new HREmployeeLoanApprovalDTO
                                        {
                                            HREL_ID = a.HREL_ID,
                                            HRMDOC_DocumentName = b.HRMDOC_DocumentName,
                                            HRMDOC_Description = b.HRMDOC_Description,
                                            HRELD_Filepath = a.HRELD_Filepath,
                                            HRELD_Filetype = a.HRELD_Filetype

                                        }).Distinct().ToArray();



            return data;
        }

        public HREmployeeLoanApprovalDTO OBCB_Email(HREmployeeLoanApprovalDTO data)
        {
            var email = "";

            List<HREmployeeLoanApprovalDTO> result = new List<HREmployeeLoanApprovalDTO>();


            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandTimeout = 500000;
                cmd.CommandText = "LOANOPENINGCLOSINGBALANCE";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = data.MI_Id
                });



                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();


                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {
                            try
                            {
                                result.Add(new HREmployeeLoanApprovalDTO
                                {
                                    EmailSMS = dataReader["HRMEM_EmailId"].ToString(),
                                    HREL_ID = Convert.ToInt64(dataReader["HREL_Id"].ToString()),
                                    //HREL_TotalPrincipalPaid=Convert.ToDecimal(dataReader["HREL_TotalPrincipalPaid"].ToString()),
                                    //HREL_TotalPending = Convert.ToDecimal(dataReader["HREL_TotalPending"].ToString())


                                });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }


                        // data.emaildetails = result.ToArray();

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            for (int i = 0; i < result.Count; i++)
            {

                var results = sendmailcertificate(data.MI_Id, "LOANBALANCE", result[i].HREL_ID, result[i].EmailSMS, email);

            }




            return data;
        }




        public string sendmailcertificate(long MI_Id, string Template, long? HRME_ID, string email, string ccemail)        {            try            {                Dictionary<string, string> val = new Dictionary<string, string>();                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template                && e.ISES_MailActiveFlag == true).ToList();                if (template.Count == 0)                {                    return "Email Template not Mapped to the selected Institution";                }                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Resultsms = Mailcontent;                string result = Mailmsg;                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "LOANAPPROVALREPLACE";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",                  SqlDbType.VarChar)                    {                        Value = HRME_ID                    });                    cmd.Parameters.Add(new SqlParameter("@Type",                  SqlDbType.VarChar)                    {                        Value = Template                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                    var datatype = dataReader.GetFieldType(iFiled);                                    if (datatype.Name == "DateTime")                                    {                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else if (datatype.Name == "Decimal")                                    {                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else                                    {                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());                                    }                                }                            }                        }                    }                    catch (Exception ex)                    {                        return ex.Message;                    }                }                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailmsg = result;                        }                    }                }                Mailmsg = result;                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {


                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailcontent = Resultsms;                        }                    }                }                Mailcontent = Resultsms;                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();                string Attechement = "";                if (alldetails.Count > 0)                {                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);                    string Subject = template[0].ISES_MailSubject.ToString();                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();                    smstpdetails = _HRMSContext.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();                    string mailcc = "";                    string mailbcc = "";                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")                    {                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');                        mailcc = ccmail[0].ToString();                        if (ccmail.Length > 1)                        {                            if (ccmail[1] != null || ccmail[1] != "")                            {                                mailbcc = ccmail[1].ToString();                            }                        }                    }                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")                    {                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();                    }                    var message = new SendGridMessage();                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);                    message.Subject = Subject;                    message.AddTo(email);


                    //mailto:message.addto("sreepriyagangadharan15@gmail.com");

                    if (Attechement.Equals("1"))                    {                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();                        if (img.Count > 0)                        {                            for (int i = 0; i < img.Count; i++)                            {                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;                                Stream stream = response.GetResponseStream();                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);                            }                        }                    }
                    //***************** EMAIL CC DETAILS **************//

                    if (ccemail != null && ccemail != "")                    {                        string[] ccmaildetails = ccemail.Split(',');                        foreach (var c in ccmaildetails)                        {                            message.AddCc(c);                        }                    }







                    if (mailcc != null && mailcc != "")                    {
                        //message.AddCc(mailcc);
                    }                    if (mailbcc != null && mailbcc != "")                    {                        message.AddBcc(mailbcc);                    }                    message.HtmlContent = Mailmsg;                    var client = new SendGridClient(sengridkey);                    try                    {                        client.SendEmailAsync(message).Wait();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())                    {                        var template1010 = _HRMSContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();                        cmd.CommandText = "IVRM_Email_Outgoing";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@EmailId",                            SqlDbType.NVarChar)                        {                            Value = email                        });                        cmd.Parameters.Add(new SqlParameter("@Message",                           SqlDbType.NVarChar)                        {                            Value = Mailcontent                        });                        cmd.Parameters.Add(new SqlParameter("@module",                        SqlDbType.VarChar)                        {                            Value = modulename[0]                        });                        cmd.Parameters.Add(new SqlParameter("@MI_Id",                       SqlDbType.BigInt)                        {                            Value = MI_Id                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        try                        {                            using (var dataReader = cmd.ExecuteReader())                            {                            }                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return "success";        }
        public HREmployeeLoanApprovalDTO BondCompTaskCreate(HREmployeeLoanApprovalDTO data)
        {
            MasterEmployeeDTO obj = new MasterEmployeeDTO();
            try
            {
                List<HREmployeeLoanApprovalDTO> result = new List<HREmployeeLoanApprovalDTO>();
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Two_Year_Completed_Employees";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new HREmployeeLoanApprovalDTO
                                {
                                    MI_Name = dataReader["EmployeeName"].ToString(),
                                    hrmE_EmployeeCode = dataReader["hrme_employeecode"].ToString(),
                                    DepartmentName = dataReader["HRMD_DepartmentName"].ToString(),
                                    DesignationName = dataReader["HRMDES_DesignationName"].ToString(),
                                    EmailSMS = dataReader["HRME_EmailId"].ToString()

                                });
                            }
                        }
                        if (result.Count > 0)
                        {
                            foreach (var dd in result)
                            {
                                SendEmails(data.MI_Id, "ReplacementHiring", dd.MI_Name, dd.hrmE_EmployeeCode, dd.DepartmentName, dd.DesignationName, dd.EmailSMS);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }

        public string SendEmails(long MI_Id, string Template, string name, string ecode, string dept, string des, string emailid)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
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



                Mailmsg = Mailmsg.Replace("[EMPLOYEENAME]", name);
                Mailmsg = Mailmsg.Replace("[EMPCODE]", ecode);
                Mailmsg = Mailmsg.Replace("[DESIGNATION]", des);
                Mailmsg = Mailmsg.Replace("[DEPARTMENT]", dept);

                //Mailmsg = Mailmsg.Replace("[DATE]", DATE);
                //Mailmsg = Mailmsg.Replace("[TASK]", TASK);
                //Mailmsg = Mailmsg.Replace("[PriorityLevel]", PriorityLevel);
                //Mailmsg = Mailmsg.Replace("[STATUS]", STATUS);
                //Mailmsg = Mailmsg.Replace("[DATE1]", DATE1);
                //Mailmsg = Mailmsg.Replace("[DATE2]", DATE2);
                //Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));

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
                                mailbcc = ccmail[1].ToString();
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
                    message.AddTo(emailid);

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailid
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
