using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace HRMSServiceHub.com.vaps.Services
{
    public class FF_Settlement_Process_Service : Interfaces.FF_Settlement_ProcessInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public FF_Settlement_Process_Service(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        ////////////////////////Apply///////////////////////////////////

        public FF_Settlement_Process_DTO getalldetails(FF_Settlement_Process_DTO dto)
        {
            dto.retrunMsg = "";
            try
            {

                dto = GetAllDropdownAndDatatableDetails(dto);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public FF_Settlement_Process_DTO SaveUpdate(FF_Settlement_Process_DTO dto)
        {
            dto.HRME_Id = _HRMSContext.Staff_User_Login.Single(c => c.Id == dto.LogInUserId && c.MI_Id == dto.MI_Id).Emp_Code;

            try
            {
                if (dto.HREFFS_Id > 0)
                {
                    var duplicatAdd = _HRMSContext.HR_Emp_FullFinal_SettlementDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HREFFS_Remarks == dto.HREFFS_Remarks && t.HREFFS_Id != dto.HREFFS_Id).Count();
                    if (duplicatAdd == 0)
                    {
                        var result = _HRMSContext.HR_Emp_FullFinal_SettlementDMO.Single(t => t.HREFFS_Id == dto.HREFFS_Id);

                        result.HREFFS_AdvStatus = dto.HREFFS_AdvStatus;
                        result.HREFFS_Remarks = dto.HREFFS_Remarks;
                        result.HREFFS_ActiveFlag = true;
                        result.HREFFS_UpdatedDate = DateTime.Now;
                        result.HREFFS_CreatedDate = DateTime.Now;
                        result.HREFFS_UpdatedBy = dto.LogInUserId;
                        result.HREFFS_CreatedBy = dto.LogInUserId;
                        _HRMSContext.Update(result);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {

                    var duplicatAdd = _HRMSContext.HR_Emp_FullFinal_SettlementDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HREFFS_Remarks == dto.HREFFS_Remarks).Count();
                    if (duplicatAdd > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        HR_Emp_FullFinal_SettlementDMO dmoObj = new HR_Emp_FullFinal_SettlementDMO();
                        dmoObj.HRME_Id = dto.HRME_Id;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HREFFS_EntryDate = DateTime.Now;
                        dmoObj.HREFFS_AdvStatus = "Applied";
                        dmoObj.HREFFS_Remarks = dto.HREFFS_Remarks;                                              
                        dmoObj.HREFFS_ActiveFlag = true;
                        dmoObj.HREFFS_UpdatedDate = DateTime.Now;
                        dmoObj.HREFFS_CreatedDate = DateTime.Now;
                        dmoObj.HREFFS_UpdatedBy = dto.LogInUserId;
                        dmoObj.HREFFS_CreatedBy = dto.LogInUserId;
                        _HRMSContext.Add(dmoObj);

                        var flag = _HRMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }


                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public FF_Settlement_Process_DTO Edit(FF_Settlement_Process_DTO dto)
        {
            try
            {
                dto.empadvaList = _HRMSContext.HR_Emp_FullFinal_SettlementDMO.Where(a => a.HREFFS_Id == dto.HREFFS_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public FF_Settlement_Process_DTO deactivate(FF_Settlement_Process_DTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HREFFS_Id > 0)
                {
                    var result = _HRMSContext.HR_Emp_FullFinal_SettlementDMO.Single(t => t.HREFFS_Id == dto.HREFFS_Id);

                    if (result.HREFFS_ActiveFlag == true)
                    {
                        result.HREFFS_ActiveFlag = false;
                    }
                    else if (result.HREFFS_ActiveFlag == false)
                    {
                        result.HREFFS_ActiveFlag = true;
                    }
                    result.HREFFS_UpdatedDate = DateTime.Now;
                    result.HREFFS_UpdatedBy = dto.LogInUserId;
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HREFFS_ActiveFlag == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public FF_Settlement_Process_DTO GetAllDropdownAndDatatableDetails(FF_Settlement_Process_DTO dto)
        {
            List<HR_Emp_FullFinal_SettlementDMO> datalist = new List<HR_Emp_FullFinal_SettlementDMO>();

            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();
            List<Month> Monthlist = new List<Month>();

            try
            {
                dto.HRME_Id = _HRMSContext.Staff_User_Login.Where(t => t.Id == dto.LogInUserId).Select(t => t.Emp_Code).FirstOrDefault();

                var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning")
                                  select new HR_Employee_Salary_DetailsDTO
                                  {
                                      HRMED_Id = emp.HRMED_Id,
                                      HRMED_Name = mas.HRMED_Name,
                                      HRESD_Amount = emp.HREED_Amount,
                                      HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                                  }).ToList();

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));


                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.ToArray();

                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true && t.MI_Id == dto.MI_Id).ToList();
                dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;
                var datalistt = (from sal in _HRMSContext.HR_Emp_FullFinal_SettlementDMO
                                 from emp in _HRMSContext.MasterEmployee
                                 where sal.HRME_Id == emp.HRME_Id && sal.MI_Id == dto.MI_Id && sal.HRME_Id == dto.HRME_Id
                                 select new FF_Settlement_Process_DTO
                                 {
                                     HREFFS_Id = sal.HREFFS_Id,
                                     MI_Id = sal.MI_Id,
                                     HRME_Id = sal.HRME_Id,
                                     HRME_EmployeeFirstName = emp.HRME_EmployeeFirstName,
                                     HRME_EmployeeMiddleName = emp.HRME_EmployeeMiddleName,
                                     HRME_EmployeeLastName = emp.HRME_EmployeeLastName,
                                     HREFFS_EntryDate = sal.HREFFS_EntryDate,
                                     HREFFS_AdvYear = sal.HREFFS_AdvYear,
                                     HREFFS_AdvMonth = sal.HREFFS_AdvMonth,
                                     HREFFS_AppliedAmount = sal.HREFFS_AppliedAmount,
                                     HREFFS_ModeOfPayment = sal.HREFFS_ModeOfPayment,
                                     HREFFS_Remarks = sal.HREFFS_Remarks,
                                     HREFFS_AdvStatus = sal.HREFFS_AdvStatus,
                                     HREFFS_ActiveFlag = sal.HREFFS_ActiveFlag,
                                     HREFFS_UpdatedDate = sal.HREFFS_UpdatedDate,
                                     HREFFS_CreatedDate = sal.HREFFS_CreatedDate
                                 }).ToList();
                dto.empadvaList = datalistt.ToArray();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
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
                        dto.griddisplay = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }



        //////////////////////////////////Approval/////////////////////////////////
        public FF_Settlement_Process_DTO getData(FF_Settlement_Process_DTO dto)
        {
            dto.retrunMsg = "";
            try
            {

                dto = GetAllDetails(dto);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public FF_Settlement_Process_DTO GetAllDetails(FF_Settlement_Process_DTO dto)
        {
            try
            {                
               var HRME_Id = _HRMSContext.Staff_User_Login.Where(t => t.Id == dto.LogInUserId).Select(t => t.Emp_Code).FirstOrDefault();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_FullFinal_Settlement_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.LogInUserId
                    });


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
                        dto.empadvaList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EMP_FULLFINAL_SETTLEMENT_APPROVAL_GRID";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                 
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
                        dto.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public FF_Settlement_Process_DTO empdetail(FF_Settlement_Process_DTO dto)
        {
            try
            {

                var ApprovalHRME_Id = _HRMSContext.Staff_User_Login.Where(t => t.Id == dto.LogInUserId).Select(t => t.Emp_Code).FirstOrDefault();

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
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
                        dto.griddisplay = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EMP_FULLFINAL_SETTLEMENT_GRID";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = ApprovalHRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HREFFS_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HREFFS_Id
                    });
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
                        dto.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                var currentlevel = "";                var CurrentApprovalMail = "";                var NextLevel = "";                var NextlevelApprovalMail = "";                var HRMDES_DesignationName = "";                var hremail = "";                var ccemail = "";                string toemailid = "";                string employeeEmail = "";                string FinalFlg = "";                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_Emp_FF_SactionLevel";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)                    {                        Value = dto.LogInUserId                    });                              cmd.Parameters.Add(new SqlParameter("@HREFFS_Id", SqlDbType.BigInt)                    {                        Value = dto.HREFFS_Id                    });


                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]                                    );                                }


                                currentlevel = dataReader["currentlevel"].ToString();
                                CurrentApprovalMail = dataReader["CurrentApprovalMail"].ToString();
                                NextLevel = dataReader["NextLevel"].ToString();
                                NextlevelApprovalMail = dataReader["NextlevelApprovalMail"].ToString();
                                HRMDES_DesignationName = dataReader["HRMDES_DesignationName"].ToString();                                hremail = dataReader["HRMail"].ToString();                                employeeEmail = dataReader["EmployeeEmail"].ToString();                                FinalFlg = dataReader["HRPAON_FinalFlg"].ToString();





                            }                        }
                    }

                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }

                }

                dto.HREFFSA_SanctionLevel = Convert.ToInt64(currentlevel);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;

        }
        public FF_Settlement_Process_DTO approvedrecord(FF_Settlement_Process_DTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO conobj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);


                var currentlevel = "";                var CurrentApprovalMail = "";                var NextLevel = "";                var NextlevelApprovalMail = "";                var HRMDES_DesignationName = "";                var hremail = "";                var ccemail = "";                string toemailid = "";                string employeeEmail = "";                string FinalFlg = "";                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_Emp_FF_SactionLevel";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)                    {                        Value = dto.LogInUserId                    });

                    cmd.Parameters.Add(new SqlParameter("@HREFFS_Id", SqlDbType.BigInt)                    {                        Value = dto.HREFFS_Id                    });


                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]                                    );                                }


                                currentlevel = dataReader["currentlevel"].ToString();
                                CurrentApprovalMail = dataReader["CurrentApprovalMail"].ToString();
                                NextLevel = dataReader["NextLevel"].ToString();
                                NextlevelApprovalMail = dataReader["NextlevelApprovalMail"].ToString();
                                HRMDES_DesignationName = dataReader["HRMDES_DesignationName"].ToString();                                hremail = dataReader["HRMail"].ToString();                                employeeEmail = dataReader["EmployeeEmail"].ToString();                                FinalFlg = dataReader["HRPAON_FinalFlg"].ToString();





                            }                        }
                    }

                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }

                }


                var approvalstatus = dto.HREFFS_AdvStatus;

                if (dto.HREFFS_AdvStatus == "Approved")
                {
                    if (Convert.ToBoolean(FinalFlg) == true)
                    {
                        dto.HREFFS_AdvStatus = "Sanctioned";

                    }
                    else
                    {
                        dto.HREFFS_AdvStatus = "Partial Approved";
                        dto.HREFFS_SanctinedAmount = dto.HREFFS_SanctinedAmount;
                    }
                }

                var HREFFS_Id = _HRMSContext.HR_Emp_FullFinal_SettlementDMO.Single(t => t.HREFFS_Id == dto.HREFFS_Id);

                if (dto.HREFFS_Id > 0)
                {

                    HR_Emp_FullFinal_SettlementApprovalDMO advapp = new HR_Emp_FullFinal_SettlementApprovalDMO();

                    advapp.HREFFS_Id = dto.HREFFS_Id;
                    advapp.IVRMUL_Id = dto.LogInUserId;
                    advapp.HREFFSA_Remarks = dto.HREFFSA_Remarks;
                    advapp.HREFFSA_Date = DateTime.Now;
                    advapp.HREFFSA_SanctionLevel = Convert.ToInt64(currentlevel);
                    advapp.HREFFSA_ActiveFlag = true;
                    advapp.HREFFSA_CreatedDate = DateTime.Now;
                    advapp.HREFFSA_UpdatedDate = DateTime.Now;
                    advapp.HREFFSA_CreatedBy = dto.LogInUserId;
                    advapp.HREFFSA_UpdatedBy = dto.LogInUserId;
                    advapp.HREFFSA_Status = approvalstatus;
                    advapp.HREFFSA_Amount = dto.HREFFS_SanctinedAmount;
                    _HRMSContext.Add(advapp);
                    _HRMSContext.SaveChanges();

                    var result = _HRMSContext.HR_Emp_FullFinal_SettlementDMO.Single(t => t.HREFFS_Id == dto.HREFFS_Id);
                    result.HREFFS_UpdatedDate = DateTime.Now;
                    result.HREFFS_UpdatedBy = dto.LogInUserId;
                    result.HREFFS_AdvStatus = dto.HREFFS_AdvStatus;
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";
                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }



                dto = GetAllDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        ///////////////////////////Report////////////////////////////////////////
        public FF_Settlement_Process_DTO Onload(FF_Settlement_Process_DTO dto)
        {
            try
            {
                var employees = _HRMSContext.HR_Emp_FullFinal_SettlementDMO.Where(R => R.HREFFS_ActiveFlag == true).Distinct().ToList();
                List<long> HRME_Id = new List<long>();
                if (employees.Count > 0)
                {
                    foreach (var d in employees)
                    {
                        HRME_Id.Add(d.HRME_Id);
                    }
                }
                dto.employees = _HRMSContext.MasterEmployee.Where(R => R.HRME_ActiveFlag == true && HRME_Id.Contains(R.HRME_Id)).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public FF_Settlement_Process_DTO getapprovalreport(FF_Settlement_Process_DTO dto)
        {
            try
            {
                var from_date = dto.Fromdate.Value.Date.ToString("yyyy-MM-dd");
                var To_date = dto.Todate.Value.Date.ToString("yyyy-MM-dd");

                List<long> HRME_Id = new List<long>();

                for (var i = 0; i < dto.HRMEID.Length; i++)
                {
                    HRME_Id.Add(dto.HRMEID[i].HRME_Id);
                }
                string employeeid = string.Join(",", HRME_Id);

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EMP_FF_SETTLEMENT_APPROVAL_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value = employeeid });                
                    cmd.Parameters.Add(new SqlParameter("@From_Date", SqlDbType.VarChar) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.VarChar) { Value = To_date });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = dto.rdbbutton });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = dto.Status_Type });

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
                                    dtoRow.Add(dtoReader.GetName(iFiled), dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.approvalReport = retObject.ToArray();

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
