using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
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
    public class GeneralRequestProcessImpl : Interfaces.GeneralRequestProcessInterface
    {
        public FOContext _FOContext;
        public DomainModelMsSqlServerContext _db;
        public GeneralRequestProcessImpl(FOContext FOContext, DomainModelMsSqlServerContext db)
        {
            _FOContext = FOContext;
            _db = db;
        }

        /////punch request ////
        public FO_Emp_PunchDTO getpunch(FO_Emp_PunchDTO data)
        {
            var qyery1 = (from q in _db.Staff_User_Login
                          from r in _db.HR_Master_Employee_DMO
                          where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                          select new FO_Emp_PunchDTO
                          {
                              HRME_Id = r.HRME_Id,
                          }).Distinct().ToArray();
            data.empname = (from a in _db.HR_Master_Employee_DMO
                            from b in _FOContext.HR_Master_Designation_DMO
                            where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == qyery1.FirstOrDefault().HRME_Id)
                            select new FO_Emp_PunchDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                HRME_DOJ = a.HRME_DOJ,
                                HRME_MobileNo = _FOContext.Multiple_Mobile_DMO.Where(m => m.HRME_Id == a.HRME_Id && m.HRMEMNO_DeFaultFlag == "default").Select(a => a.HRMEMNO_MobileNo).FirstOrDefault(),

                                HRME_EmailId = _FOContext.Multiple_Email_DMO.Where(m => m.HRME_Id == a.HRME_Id && m.HRMEM_DeFaultFlag == "default").Select(a => a.HRMEM_EmailId).FirstOrDefault(),

                                HRMDES_DesignationName = b.HRMDES_DesignationName,
                                HRME_EmployeeCode = a.HRME_EmployeeCode
                            }).Distinct().ToArray();
            using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "FO_Emp_Punch_Alter_RequestGrid";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                {
                    Value = data.UserId
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
                    data.punchdetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return data;
        }
        public FO_Emp_PunchDTO emprequest(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO TTMC = new FO_Emp_PunchDTO();
            try
            {
                var que1 = _FOContext.FO_Emp_Punch.Where(a => a.MI_Id == data.MI_Id && a.FOEP_PunchDate.Value.Date == data.empdate.Date && a.HRME_Id == data.HRME_Id).Select(t => t.FOEP_Id).FirstOrDefault();

                if (que1 > 0)
                {
                    var que2 = _FOContext.FO_Emp_Punch_Details.Where(b => b.MI_Id == data.MI_Id && b.FOEP_Id == que1).ToList();
                    TTMC.employeelist = que2.ToArray();
                    TTMC.returnval = true;
                }
                else
                {
                    TTMC.returnval = false;
                }
            }
            catch (Exception ex)
            {
                TTMC.returnval = false;
            }
            return TTMC;
        }
        public FO_Emp_PunchDTO grsavedetail(FO_Emp_PunchDTO data)
        {
            data.retrunMsg = "";
            try
            {
                var panchdate = _FOContext.Database.ExecuteSqlCommand("FO_Emp_Punch_Alter_Request_Insert @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.HRME_Id, data.FOEPAR_PunchDate, data.FOEPAR_Remarks, data.FOEPARD_Time, data.FOEPARD_InOutFlg);
                if (panchdate > 0)
                {
                    data.retrunMsg = "updated";
                }
                else
                {
                    data.retrunMsg = "notUpdated";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }

            return data;
        }
        
        ///Approval
        public FO_Emp_PunchDTO getdata(FO_Emp_PunchDTO data)
        {

            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_AllEmp_Punch_Missing_RequestGrid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.punchrequest = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_AllEmp_Punch_Missing_ApprovedGrid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.punchapproveddetails = retObject.ToArray();
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
            return data;
        }
        public FO_Emp_PunchDTO approvaldata(FO_Emp_PunchDTO data)
        {
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Emp_Punch_Alter_RequestGridApproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FOEPAR_Id", SqlDbType.BigInt)
                    {
                        Value = data.FOEPAR_Id
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
                        data.approvaldata = retObject.ToArray();
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
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public FO_Emp_PunchDTO getstatus(FO_Emp_PunchDTO data)
        {
            data.retrunMsg = "";
            var emp_Id = (from a in _FOContext.Staff_User_Login
                          from b in _FOContext.HR_Master_Employee_DMO
                          where (a.Id == data.UserId && a.Emp_Code == b.HRME_Id)
                          select new FO_Emp_PunchDTO
                          {
                              HRME_Id = b.HRME_Id,
                          }).Distinct().ToArray();
            data.HRME_Id = emp_Id[0].HRME_Id;
            try
            {
                if(data.FOEPARA_ApprovalFlg==true)
                {
                    FO_Emp_Punch_Alter_Request_ApprovalDMO apppunch = new FO_Emp_Punch_Alter_Request_ApprovalDMO();
                    apppunch.FOEPAR_Id = data.FOEPAR_Id;
                    apppunch.HRME_Id = data.HRME_Id;
                    apppunch.FOEPARA_ApprovedTime = data.FOEPARA_ApprovedTime;
                    apppunch.FOEPARA_Remarks = data.FOEPARA_Remarks;
                    apppunch.FOEPARA_ActiveFlg = true;
                    apppunch.FOEPARA_ApprovalFlg = true;
                    apppunch.FOEPARA_CreatedDate = DateTime.Now;
                    apppunch.FOEPARA_UpdatedDate = DateTime.Now;
                    apppunch.FOEPARA_CreatedBy = data.UserId;
                    apppunch.FOEPARA_UpdatedBy = data.UserId;
                    _FOContext.Add(apppunch);
                    _FOContext.SaveChanges();

                    if(data.FOEPAR_Id>0)
                    {
                        var update = _FOContext.FO_Emp_Punch_Alter_Request_DetailsDMO.Where(R => R.FOEPAR_Id == data.FOEPAR_Id).ToList();
                        if(update.Count>0)
                        {
                            foreach (var pun in update)
                            {
                                pun.FOEPARD_ApprvedFlg = true;
                                pun.FOEPARD_Remarks = data.FOEPARA_Remarks;
                                pun.FOEPARD_ApprovedTime = data.FOEPARA_ApprovedTime;
                                pun.FOEPARD_UpdatedBy = data.UserId;
                                pun.FOEPAR_Id = data.FOEPAR_Id;
                                _FOContext.Update(pun);
                            }
                            var flag1 = _FOContext.SaveChanges();
                            if (flag1 > 0)
                            {
                                data.retrunMsg = "Update";
                            }
                            else
                            {
                                data.retrunMsg = "false";
                            }
                        }
                       
                    }                   
                }
           
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }
    }
}
