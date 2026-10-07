using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
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

namespace IssueManager.com.vaps.Services
{
    public class DriverIndentImpl : Interfaces.DriverIndentInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public DriverIndentImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public ISM_Driver_IndentDTO getdetails(ISM_Driver_IndentDTO data)
        {
            try
            {
                //======================= Driver Indent
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Insert_ISM_Driver_Indent";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_Date",
             SqlDbType.DateTime)
                    {
                        Value = data.ISMDIT_Date
                    });

                    cmd.Parameters.Add(new SqlParameter("@TRMV_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.TRMV_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_BillNo",
            SqlDbType.VarChar)
                    {
                        Value = data.ISMDIT_BillNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_Qty",
            SqlDbType.Decimal)
                    {
                        Value = data.ISMDIT_Qty
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_Amount",
            SqlDbType.Decimal)
                    {
                        Value = data.ISMDIT_Amount
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_Remarks",
            SqlDbType.VarChar)
                    {
                        Value = data.ISMDIT_Remarks
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_OpeningKM",
            SqlDbType.Decimal)
                    {
                        Value = data.ISMDIT_OpeningKM
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_ClosingKM",
            SqlDbType.Decimal)
                    {
                        Value = data.ISMDIT_ClosingKM
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_BalanceDiesel",
            SqlDbType.Decimal)
                    {
                        Value = data.ISMDIT_BalanceDiesel
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_PreviousReading",
           SqlDbType.Decimal)
                    {
                        Value = data.ISMDIT_PreviousReading
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_CurrentReading",
           SqlDbType.Decimal)
                    {
                        Value = data.ISMDIT_CurrentReading
                    });

                    cmd.Parameters.Add(new SqlParameter("@CreatedDate",
                               SqlDbType.DateTime)
                    {
                        Value = data.CreatedDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@UpdatedDate",
           SqlDbType.DateTime)
                    {
                        Value = data.UpdatedDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMDIT_PreparedByUserId",
           SqlDbType.BigInt)
                    {
                        Value = data.ISMDIT_PreparedByUserId
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var data1 = cmd.ExecuteNonQuery();
                    if(data1>0)
                    {
                        data.responsemessage = "Sucess";
                    }
                    else
                    {
                        data.responsemessage = "fail";
                    }
                    //var retObject = new List<dynamic>();
                    //try
                    //{
                    //    using (var dataReader1 = cmd.ExecuteReader())
                    //    {
                    //        while (dataReader1.Read())
                    //        {
                    //            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                    //            for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                    //            {
                    //                dataRow1.Add(
                    //                    dataReader1.GetName(iFiled1),
                    //                    dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                    //                );
                    //            }

                    //            retObject.Add((ExpandoObject)dataRow1);
                    //        }
                    //    }
                    //    data.DriverIndentList = retObject.ToArray();
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_Driver_IndentDTO loaddata(ISM_Driver_IndentDTO data)
        {
            try
            {
                data.MI_logo = _IssueContext.Institution.Where(R => R.MI_Id == data.MI_Id).Select(P => P.MI_Logo).FirstOrDefault();
                data.alldata = (from a in _IssueContext.Master_VehicleDMO
                                from b in _IssueContext.ISM_Driver_IndentDMO
                                from c in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                where (b.TRMV_Id == a.TRMV_Id && a.TRMV_ActiveFlag == true && b.ISMDIT_Id == c.ISMDIT_Id && c.ISMDIT_PreparedByUserId == data.User_Id)
                                select new ISM_Driver_IndentDTO
                                {
                                    ISMDIT_Id = b.ISMDIT_Id,
                                    ISMDIT_Date = b.ISMDIT_Date,
                                    TRMV_Id = b.TRMV_Id,
                                    vehicleno = a.TRMV_VehicleNo,
                                    ISMDIT_BillNo = b.ISMDIT_BillNo,
                                    ISMDIT_Qty = b.ISMDIT_Qty,
                                    ISMDIT_Amount = b.ISMDIT_Amount,
                                    ISMDIT_Remarks = b.ISMDIT_Remarks,
                                    ISMDIT_OpeningKM = b.ISMDIT_OpeningKM,
                                    ISMDIT_ClosingKM = b.ISMDIT_ClosingKM,
                                    ISMDIT_BalanceDiesel = b.ISMDIT_BalanceDiesel,
                                    ISMDIT_Status = c.ISMDIT_Status
                                }).Distinct().OrderBy(t=>t.ISMDIT_Date).ToArray();

                data.vehicle_list = (from a in _IssueContext.Institution
                                     from b in _IssueContext.Master_VehicleDMO
                                     where (b.MI_Id == a.MI_Id && a.MI_ActiveFlag == 1 && b.TRMV_ActiveFlag == true)
                                     select new ISM_Driver_IndentDTO
                                     {
                                         TRMV_Id = b.TRMV_Id,
                                         TRMV_VehicleNo = b.TRMV_VehicleNo
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_Driver_IndentDTO save(ISM_Driver_IndentDTO data)
        {
            try
            {
                data.HRME_Id = _IssueContext.Staff_User_Login.Where(c => c.Id == data.User_Id && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;

                if (data.HRME_Id > 0)
                {
                    data.HRMDES_Id = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id).FirstOrDefault().HRMDES_Id;

                    data.DesignationName = _IssueContext.HR_Master_Designation.Where(t => t.HRMDES_Id == data.HRMDES_Id && t.MI_Id == data.MI_Id).FirstOrDefault().HRMDES_DesignationName;
                }

                if (data.ISMDIT_Id == 0)
                {
                    var duplicate = _IssueContext.ISM_Driver_IndentDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMDIT_Date == data.ISMDIT_Date && t.TRMV_Id == data.TRMV_Id && t.ISMDIT_BillNo == data.ISMDIT_BillNo).Distinct().ToList();

                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        ISM_Driver_IndentDMO a = new ISM_Driver_IndentDMO();
                        a.MI_Id = data.MI_Id;
                        a.TRMV_Id = data.TRMV_Id;
                        a.ISMDIT_Date = data.ISMDIT_Date;
                        a.ISMDIT_BillNo = data.ISMDIT_BillNo;
                        a.ISMDIT_Qty = data.ISMDIT_Qty;
                        a.ISMDIT_Amount = data.ISMDIT_Amount;
                        a.ISMDIT_Remarks = data.ISMDIT_Remarks;
                        a.ISMDIT_OpeningKM = data.ISMDIT_OpeningKM;
                        a.ISMDIT_ClosingKM = data.ISMDIT_ClosingKM;
                        a.ISMDIT_BalanceDiesel = data.ISMDIT_BalanceDiesel;
                        a.ISMDIT_PreviousReading = data.ISMDIT_PreviousReading;
                        a.ISMDIT_CurrentReading = data.ISMDIT_CurrentReading;
                        a.CreatedDate = DateTime.Now;
                        a.UpdatedDate = DateTime.Now;
                        _IssueContext.Add(a);
                        _IssueContext.SaveChanges();

                        ISM_Driver_Indent_ApprovalDMO obj2 = new ISM_Driver_Indent_ApprovalDMO();
                        obj2.ISMDIT_Id = a.ISMDIT_Id;
                        obj2.ISMDIT_PreparedByUserId = data.User_Id;
                        obj2.ISMDIT_ApprovalUserId = 0;
                        obj2.ISMDIT_ReceivedByUserId = 0;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.UpdatedDate = DateTime.Now;
                        obj2.ISMDIT_Remark = data.ISMDIT_Remarks;
                        obj2.ISMDIT_PreparedByFlag = data.DesignationName;
                        obj2.ISMDIT_Status = "Applied";
                        _IssueContext.Add(obj2);

                        var w = _IssueContext.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "Saved";
                            SendEmailIndentApply(data, "Driver Indent Request", "Driver Indent Request", data.MI_Id);
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.ISMDIT_Id > 0)
                {
                    var duplicate = _IssueContext.ISM_Driver_IndentDMO.Where(t => t.ISMDIT_Id != data.ISMDIT_Id && t.MI_Id == data.MI_Id && t.ISMDIT_Date == data.ISMDIT_Date && t.TRMV_Id == data.TRMV_Id && t.ISMDIT_BillNo == data.ISMDIT_BillNo).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var j = _IssueContext.ISM_Driver_IndentDMO.Where(t => t.MI_Id == data.MI_Id & t.ISMDIT_Id == data.ISMDIT_Id).SingleOrDefault();
                        j.ISMDIT_Id = data.ISMDIT_Id;
                        j.MI_Id = data.MI_Id;
                        j.TRMV_Id = data.TRMV_Id;
                        j.ISMDIT_Date = data.ISMDIT_Date;
                        j.ISMDIT_BillNo = data.ISMDIT_BillNo;
                        j.ISMDIT_Qty = data.ISMDIT_Qty;
                        j.ISMDIT_Amount = data.ISMDIT_Amount;
                        j.ISMDIT_Remarks = data.ISMDIT_Remarks;
                        j.ISMDIT_OpeningKM = data.ISMDIT_OpeningKM;
                        j.ISMDIT_ClosingKM = data.ISMDIT_ClosingKM;
                        j.ISMDIT_BalanceDiesel = data.ISMDIT_BalanceDiesel;
                        j.ISMDIT_PreviousReading = data.ISMDIT_PreviousReading;
                        j.ISMDIT_CurrentReading = data.ISMDIT_CurrentReading;
                        j.CreatedDate = data.CreatedDate;
                        j.UpdatedDate = data.UpdatedDate;
                        _IssueContext.Update(j);
                        var r = _IssueContext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_Driver_IndentDTO edittab1(ISM_Driver_IndentDTO data)
        {
            try
            {
                data.MI_logo = _IssueContext.Institution.Where(R => R.MI_Id == data.MI_Id).Select(P => P.MI_Logo).FirstOrDefault();
                data.editlist = (from a in _IssueContext.Master_VehicleDMO
                                 from b in _IssueContext.ISM_Driver_IndentDMO
                                 from c in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                 from d in _IssueContext.Staff_User_Login
                                 from e in _IssueContext.MasterEmployee
                                 from f in _IssueContext.Institution
                                 where (a.TRMV_Id == b.TRMV_Id && b.ISMDIT_Id == data.ISMDIT_Id && b.ISMDIT_Id == c.ISMDIT_Id && c.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id && c.ISMDIT_PreparedByFlag.ToUpper() == "DRIVER" && e.MI_Id == f.MI_Id)
                                 select new ISM_Driver_IndentDTO
                                 {
                                     ISMDIT_Id = b.ISMDIT_Id,
                                     TRMV_Id = b.TRMV_Id,
                                     vehicleno = a.TRMV_VehicleNo,
                                     ISMDIT_Date = b.ISMDIT_Date,
                                     ISMDIT_BillNo = b.ISMDIT_BillNo,
                                     ISMDIT_Qty = b.ISMDIT_Qty,
                                     ISMDIT_Amount = b.ISMDIT_Amount,
                                     ISMDIT_Remarks = b.ISMDIT_Remarks,
                                     ISMDIT_OpeningKM = b.ISMDIT_OpeningKM,
                                     ISMDIT_ClosingKM = b.ISMDIT_ClosingKM,
                                     ISMDIT_BalanceDiesel = b.ISMDIT_BalanceDiesel,
                                     HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null ? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? "" : e.HRME_EmployeeLastName),
                                     MI_Logo = f.MI_Logo
                                 }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ISM_Driver_IndentDTO> getApprovalStatusAsync(ISM_Driver_IndentDTO data)
        {
            try
            {
             
               string userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).Select(t => t.IVRMRT_Role).FirstOrDefault();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DriverIndentApproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = userrole.ToUpper()
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.IndentApprovalList = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if(userrole.ToUpper() == "ADMIN")
                {
                    data.IndentApproved = (from a in _IssueContext.ISM_Driver_IndentDMO
                                           from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                           from c in _IssueContext.Master_VehicleDMO
                                           where (a.ISMDIT_Id == b.ISMDIT_Id && b.ISMDIT_PreparedByFlag == "ADMIN" && b.ISMDIT_PreparedByFlag!= "" && a.TRMV_Id == c.TRMV_Id)
                                           select new ISM_Driver_IndentDTO
                                           {
                                               ISMDIT_Id = a.ISMDIT_Id,
                                               ISMDIT_Date = a.ISMDIT_Date,
                                               TRMV_Id = a.TRMV_Id,
                                               TRMV_VehicleName = c.TRMV_VehicleName,
                                               ISMDIT_BillNo = a.ISMDIT_BillNo,
                                               ISMDIT_Qty = a.ISMDIT_Qty,
                                               ISMDIT_Amount = a.ISMDIT_Amount,
                                               ISMDIT_Remarks = a.ISMDIT_Remarks,
                                               ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                                               ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                                               ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                                               ISMDIT_PreviousReading = a.ISMDIT_PreviousReading,
                                               ISMDIT_CurrentReading = a.ISMDIT_CurrentReading,
                                               ISMDIT_Status = b.ISMDIT_Status
                                           }).OrderBy(t=>t.ISMDIT_Date).ToArray();
                }
                else if(userrole.ToUpper() == "ACCOUNTANT")
                {
                    data.IndentApproved = (from a in _IssueContext.ISM_Driver_IndentDMO
                                           from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                           from c in _IssueContext.Master_VehicleDMO
                                           where (a.ISMDIT_Id == b.ISMDIT_Id && b.ISMDIT_PreparedByFlag == "ACCOUNTANT" && a.TRMV_Id == c.TRMV_Id)
                                           select new ISM_Driver_IndentDTO
                                           {
                                               ISMDIT_Id = a.ISMDIT_Id,
                                               ISMDIT_Date = a.ISMDIT_Date,
                                               TRMV_Id = a.TRMV_Id,
                                               TRMV_VehicleName = c.TRMV_VehicleName,
                                               ISMDIT_BillNo = a.ISMDIT_BillNo,
                                               ISMDIT_Qty = a.ISMDIT_Qty,
                                               ISMDIT_Amount = a.ISMDIT_Amount,
                                               ISMDIT_Remarks = a.ISMDIT_Remarks,
                                               ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                                               ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                                               ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                                               ISMDIT_PreviousReading = a.ISMDIT_PreviousReading,
                                               ISMDIT_CurrentReading = a.ISMDIT_CurrentReading,
                                               ISMDIT_Status = b.ISMDIT_Status
                                           }).OrderBy(t => t.ISMDIT_Date).ToArray();
                }



                //if (userrole.ToUpper() == "ADMIN")
                //{
                //    data.IndentApprovalList = (from a in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                //                               from b in _IssueContext.ISM_Driver_IndentDMO
                //                               from c in _IssueContext.Master_VehicleDMO
                //                               from d in _IssueContext.Staff_User_Login
                //                               from e in _IssueContext.MasterEmployee
                //                               where (a.ISMDIT_PreparedByFlag.ToUpper() == "ADMIN" && a.ISMDIT_Id != b.ISMDIT_Id && b.TRMV_Id == c.TRMV_Id && a.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                //                               select new ISM_Driver_IndentDTO
                //                               {
                //                                   ISMDIT_Id = b.ISMDIT_Id,
                //                                   ISMDIT_Date = b.ISMDIT_Date,
                //                                   TRMV_Id = b.TRMV_Id,
                //                                   TRMV_VehicleName = c.TRMV_VehicleName,
                //                                   ISMDIT_BillNo = b.ISMDIT_BillNo,
                //                                   HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null ? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? "" : e.HRME_EmployeeLastName),
                //                                   ISMDIT_Qty = b.ISMDIT_Qty,
                //                                   ISMDIT_Amount = b.ISMDIT_Amount,
                //                                   ISMDIT_Remarks = b.ISMDIT_Remarks,
                //                                   ISMDIT_OpeningKM = b.ISMDIT_OpeningKM,
                //                                   ISMDIT_ClosingKM = b.ISMDIT_ClosingKM,
                //                               }).ToArray();
                //}
                //else if (userrole.ToUpper() == "ACCOUNTANT")
                //{
                //    data.IndentApprovalList = (from a in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                //                               from b in _IssueContext.ISM_Driver_IndentDMO
                //                               from c in _IssueContext.Master_VehicleDMO
                //                               from d in _IssueContext.Staff_User_Login
                //                               from e in _IssueContext.MasterEmployee
                //                               where (a.ISMDIT_Id == b.ISMDIT_Id && b.MI_Id == data.MI_Id && a.ISMDIT_PreparedByFlag == "MD" && b.TRMV_Id == c.TRMV_Id && a.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                //                               select new ISM_Driver_IndentDTO
                //                               {
                //                                   ISMDIT_Id = b.ISMDIT_Id,
                //                                   ISMDIT_Date = b.ISMDIT_Date,
                //                                   TRMV_Id = b.TRMV_Id,
                //                                   TRMV_VehicleName = c.TRMV_VehicleName,
                //                                   ISMDIT_BillNo = b.ISMDIT_BillNo,
                //                                   HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null ? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? "" : e.HRME_EmployeeLastName),
                //                                   ISMDIT_Qty = b.ISMDIT_Qty,
                //                                   ISMDIT_Amount = b.ISMDIT_Amount,
                //                                   ISMDIT_Remarks = b.ISMDIT_Remarks,
                //                                   ISMDIT_OpeningKM = b.ISMDIT_OpeningKM,
                //                                   ISMDIT_ClosingKM = b.ISMDIT_ClosingKM,
                //                               }).ToArray();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Driver_IndentDTO checkedVehicleno(ISM_Driver_IndentDTO data)
        {
            try
            {
                var vehicledetails = (from a in _IssueContext.ISM_Driver_IndentDMO
                                      from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                      where (a.ISMDIT_Id == b.ISMDIT_Id && a.MI_Id == data.MI_Id && a.TRMV_Id == data.TRMV_Id
                                      && b.ISMDIT_Status == "Approved")
                                      select a).Distinct().OrderByDescending(a => a.ISMDIT_Date).Take(1).ToArray();
                data.vehicledetails = vehicledetails;
                }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
            
        public ISM_Driver_IndentDTO get_status(ISM_Driver_IndentDTO data)
        {
            try
            {
                string roletype = "";
                string roleone = "";
                string roletwo = "";

                roleone = (from a in _IssueContext.Staff_User_Login
                                   from b in _IssueContext.ApplicationUserRole_con
                                   from c in _IssueContext.IVRM_Role_Type
                                   where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && a.Id == data.User_Id)
                                   select c.IVRMRT_Role).FirstOrDefault();

              
                roletwo = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).Select(t => t.IVRMRT_Role).FirstOrDefault();



                if (roleone != null)
                {
                    roletype= roleone;
                }
                else
                {
                    
                    roletype = roletwo;
                }

                for (int i = 0; i < data.get_indent_status.Length; i++)
                {
                    ISM_Driver_Indent_ApprovalDMO obj1 = new ISM_Driver_Indent_ApprovalDMO();
                    obj1.ISMDIT_Id = data.get_indent_status[i].ISMDIT_Id;
                    obj1.ISMDIT_PreparedByUserId = data.get_indent_status[i].ISMDIT_PreparedByUserId; //data.User_Id;
                    obj1.ISMDIT_ApprovalUserId = data.User_Id;
                    obj1.ISMDIT_ReceivedByUserId = 0;
                    obj1.CreatedDate = DateTime.Today;
                    obj1.UpdatedDate = DateTime.Today;
                    obj1.ISMDIT_Remark = data.ISMDIT_Remark;
                    obj1.ISMDIT_PreparedByFlag = roletype;
                    obj1.ISMDIT_Status = "Approved";
                    _IssueContext.Add(obj1);
                    var op = _IssueContext.SaveChanges();

                    if (op > 0)
                    {
                        ISM_Driver_Indent_ApprovalDMO obj = new ISM_Driver_Indent_ApprovalDMO();
                        obj = _IssueContext.ISM_Driver_Indent_ApprovalDMO.Where(t => t.ISMDIT_Id == data.get_indent_status[i].ISMDIT_Id && t.ISMDIT_PreparedByFlag.ToUpper() == "DRIVER").FirstOrDefault();
                        obj.ISMDIT_Status = "Approved";
                        _IssueContext.Update(obj);
                        _IssueContext.SaveChanges();
                    }

                    if(roletype == "ADMIN")
                    {
                        IndentApproveMail(data,i, "Driver Indent Request", "Driver Indent Request", data.MI_Id,"Approving");
                    }
                }

                data.msg = "Saved";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.msg = "Failed";
            }
            return data;
        }

        public ISM_Driver_IndentDTO reject_status(ISM_Driver_IndentDTO data)
        {
            try
            {
                string roletype = "";
                string roleone = "";
                string roletwo = "";

                roleone = (from a in _IssueContext.Staff_User_Login
                                   from b in _IssueContext.ApplicationUserRole_con
                                   from c in _IssueContext.IVRM_Role_Type
                                   where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && a.Id == data.User_Id)
                                   select c.IVRMRT_Role).FirstOrDefault();

                roletwo = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).Select(t => t.IVRMRT_Role).FirstOrDefault();

                if (roleone != null)
                {
                    roletype = roleone;
                }
                else
                {
                    roletype = roletwo;
                }

                for (int i = 0; i < data.get_indent_status.Length; i++)
                {
                    ISM_Driver_Indent_ApprovalDMO obj1 = new ISM_Driver_Indent_ApprovalDMO();
                    obj1.ISMDIT_Id = data.get_indent_status[i].ISMDIT_Id;
                    obj1.ISMDIT_PreparedByUserId = data.get_indent_status[i].ISMDIT_PreparedByUserId;// data.User_Id;
                    obj1.ISMDIT_ApprovalUserId = data.User_Id;
                    obj1.ISMDIT_ReceivedByUserId = 0;
                    obj1.CreatedDate = DateTime.Today;
                    obj1.UpdatedDate = DateTime.Today;
                    obj1.ISMDIT_Remark = data.ISMDIT_Remark;
                    obj1.ISMDIT_PreparedByFlag = roletype;
                    obj1.ISMDIT_Status = "Rejected";
                    _IssueContext.Add(obj1);
                    var op = _IssueContext.SaveChanges();

                    if (op > 0)
                    {
                        ISM_Driver_Indent_ApprovalDMO obj = new ISM_Driver_Indent_ApprovalDMO();
                        obj = _IssueContext.ISM_Driver_Indent_ApprovalDMO.Where(t => t.ISMDIT_Id == data.get_indent_status[i].ISMDIT_Id && t.ISMDIT_PreparedByFlag.ToUpper() == "DRIVER").FirstOrDefault();
                        obj.ISMDIT_Status = "Rejected";
                        _IssueContext.Update(obj);
                        _IssueContext.SaveChanges();
                    }

                    if (roleone == "ADMIN")
                    {
                        IndentApproveMail(data, i, "Driver Indent Request", "Driver Indent Request", data.MI_Id, "Rejecting");
                    }
                }

                data.msg = "Saved";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.msg = "Failed";
            }
            return data;
        }

        public ISM_Driver_IndentDTO getrpt(ISM_Driver_IndentDTO data)
        {
            try
            {
                data.MI_logo = _IssueContext.Institution.Where(R => R.MI_Id == data.MI_Id_new).Select(P => P.MI_Logo).FirstOrDefault();
                if (data.rdotype == "Approved")
                {
                    data.indentreport = (from a in _IssueContext.ISM_Driver_IndentDMO
                                         from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                         from c in _IssueContext.Master_VehicleDMO
                                         from d in _IssueContext.Staff_User_Login
                                         from e in _IssueContext.MasterEmployee
                                         where (a.ISMDIT_Id == b.ISMDIT_Id && a.ISMDIT_Date.Value.Date >= data.fromdate && a.ISMDIT_Date.Value.Date <= data.todate && b.ISMDIT_PreparedByFlag.ToUpper() == "ADMIN" && b.ISMDIT_Status == "Approved" && a.TRMV_Id == c.TRMV_Id && b.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                                         
                                         select new ISM_Driver_IndentDTO
                                         {
                                          //   ISMDIT_DiffrenceKm=(a.ISMDIT_ClosingKM - a.ISMDIT_OpeningKM),
                                             UpdatedDate=b.UpdatedDate,
                                             ISMDIT_Date = a.ISMDIT_Date,
                                             TRMV_Id = a.TRMV_Id,
                                             TRMV_VehicleNo = c.TRMV_VehicleNo,
                                             TRMV_VehicleName = c.TRMV_VehicleName,
                                             ISMDIT_BillNo = a.ISMDIT_BillNo,
                                             ISMDIT_Qty = a.ISMDIT_Qty,
                                             ISMDIT_Amount = a.ISMDIT_Amount,
                                             ISMDIT_Remarks = a.ISMDIT_Remarks,
                                             ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                                             ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                                             ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                                             HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null? "": e.HRME_EmployeeLastName),
                                             ISMDIT_Status = b.ISMDIT_Status
                                         }).OrderBy(t => t.ISMDIT_Date).ToArray();
                }
                else if (data.rdotype == "Rejected")
                {
                    data.indentreport = (from a in _IssueContext.ISM_Driver_IndentDMO
                                         from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                         from c in _IssueContext.Master_VehicleDMO
                                         from d in _IssueContext.Staff_User_Login
                                         from e in _IssueContext.MasterEmployee
                                         where (a.ISMDIT_Id == b.ISMDIT_Id && a.ISMDIT_Date.Value.Date >= data.fromdate && a.ISMDIT_Date.Value.Date <= data.todate && b.ISMDIT_PreparedByFlag.ToUpper() == "ADMIN" && b.ISMDIT_Status == "Rejected" && a.TRMV_Id == c.TRMV_Id && b.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                                         select new ISM_Driver_IndentDTO
                                         {
                                             UpdatedDate = b.UpdatedDate,
                                             ISMDIT_Date = a.ISMDIT_Date,
                                             TRMV_Id = a.TRMV_Id,
                                             TRMV_VehicleNo = c.TRMV_VehicleNo,
                                             TRMV_VehicleName = c.TRMV_VehicleName,
                                             ISMDIT_BillNo = a.ISMDIT_BillNo,
                                             ISMDIT_Qty = a.ISMDIT_Qty,
                                             ISMDIT_Amount = a.ISMDIT_Amount,
                                             ISMDIT_Remarks = a.ISMDIT_Remarks,
                                             ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                                             ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                                             ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                                             HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null ? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? "" : e.HRME_EmployeeLastName),
                                             ISMDIT_Status = b.ISMDIT_Status
                                         }).OrderBy(t => t.ISMDIT_Date).ToArray();
                }
                else if (data.rdotype == "Applied")
                {
                    data.indentreport = (from a in _IssueContext.ISM_Driver_IndentDMO
                                         from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                         from c in _IssueContext.Master_VehicleDMO
                                         from d in _IssueContext.Staff_User_Login
                                         from e in _IssueContext.MasterEmployee
                                         where (a.ISMDIT_Id == b.ISMDIT_Id && a.ISMDIT_Date.Value.Date >= data.fromdate && a.ISMDIT_Date.Value.Date <= data.todate && b.ISMDIT_PreparedByFlag.ToUpper() == "DRIVER" && b.ISMDIT_Status == "Applied" && a.TRMV_Id == c.TRMV_Id && b.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                                         select new ISM_Driver_IndentDTO
                                         {
                                             ISMDIT_Date = a.ISMDIT_Date,
                                             TRMV_Id = a.TRMV_Id,
                                             TRMV_VehicleNo = c.TRMV_VehicleNo,
                                             TRMV_VehicleName = c.TRMV_VehicleName,
                                             ISMDIT_BillNo = a.ISMDIT_BillNo,
                                             ISMDIT_Qty = a.ISMDIT_Qty,
                                             ISMDIT_Amount = a.ISMDIT_Amount,
                                             ISMDIT_Remarks = a.ISMDIT_Remarks,
                                             ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                                             ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                                             ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                                             HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null ? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? "" : e.HRME_EmployeeLastName),
                                             ISMDIT_Status = b.ISMDIT_Status
                                         }).OrderBy(t=>t.ISMDIT_Date).ToArray();
                }
                else if (data.rdotype == "All")
                {
                    double i = 0;
                    data.indentreport = (from a in _IssueContext.ISM_Driver_IndentDMO
                                         from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                                         from c in _IssueContext.Master_VehicleDMO
                                         from d in _IssueContext.Staff_User_Login
                                         from e in _IssueContext.MasterEmployee
                                         where (a.ISMDIT_Id == b.ISMDIT_Id && a.ISMDIT_Date.Value.Date >= data.fromdate && a.ISMDIT_Date.Value.Date <= data.todate && b.ISMDIT_PreparedByFlag.ToUpper() == "ADMIN" && a.TRMV_Id == c.TRMV_Id && b.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                                         select new ISM_Driver_IndentDTO
                                         {
                                             ISMDIT_DiffrenceKm = (a.ISMDIT_ClosingKM - a.ISMDIT_OpeningKM),
                                             UpdatedDate = b.UpdatedDate,
                                             ISMDIT_Date = a.ISMDIT_Date,
                                             TRMV_Id = a.TRMV_Id,
                                             TRMV_VehicleNo = c.TRMV_VehicleNo,
                                             TRMV_VehicleName = c.TRMV_VehicleName,
                                             ISMDIT_BillNo = a.ISMDIT_BillNo,
                                             ISMDIT_Qty = a.ISMDIT_Qty,
                                             ISMDIT_Amount = a.ISMDIT_Amount,
                                                
                                             ISMDIT_Remarks = a.ISMDIT_Remarks,
                                             ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                                             ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                                             ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                                             HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null ? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? "" : e.HRME_EmployeeLastName),
                                             ISMDIT_Status = b.ISMDIT_Status
                                         }).OrderBy(t => t.ISMDIT_Date).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public void SendEmailIndentApply(ISM_Driver_IndentDTO obj, string subject, string body, long id)
        {
            try
            {
                var drivername = (from a in _IssueContext.Staff_User_Login
                                       from c in _IssueContext.MasterEmployee
                                       where (a.Id == obj.User_Id && a.Emp_Code == c.HRME_Id)
                                       select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();
                
                string drivermailid = (from a in _IssueContext.Multiple_Email_DMO
                                       where (a.HRME_Id == obj.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                                       select a.HRMEM_EmailId).FirstOrDefault();

                var vehicleno = _IssueContext.Master_VehicleDMO.Where(t => t.TRMV_Id == obj.TRMV_Id).FirstOrDefault();

                string mailid = "siddesh@vapstech.com";//"goutamkumar@vapstech.com"; 
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("IndentApply", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }
                    if (drivermailid != null) { message.AddBcc(drivermailid); }
                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", drivername.HRME_EmployeeFirstName + " " + (drivername.HRME_EmployeeMiddleName == null ? "" : drivername.HRME_EmployeeMiddleName) + " " + (drivername.HRME_EmployeeLastName == null ? "" : drivername.HRME_EmployeeLastName));
                        message.HtmlContent = message.HtmlContent.Replace("[VEHICLENO]", vehicleno.TRMV_VehicleNo);
                        message.HtmlContent = message.HtmlContent.Replace("[FUELQUANTITY]", obj.ISMDIT_Qty.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[AMOUNT]", obj.ISMDIT_Amount.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[OPENINGKM]", obj.ISMDIT_OpeningKM.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[CLOSINGKM]", obj.ISMDIT_ClosingKM.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[BALANCEDIESEL]", obj.ISMDIT_BalanceDiesel.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[REMARK]", obj.ISMDIT_Remarks);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("IndentApply", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void IndentApproveMail(ISM_Driver_IndentDTO obj,long index, string subject, string body, long id,string status)
        {
            try
            {
                string drivermailid = (from a in _IssueContext.Multiple_Email_DMO
                                       from c in _IssueContext.Staff_User_Login
                                       where (a.HRME_Id == c.Emp_Code && a.HRMEM_DeFaultFlag == "default" && c.Id == obj.ISMDIT_PreparedByUserId)
                                       select a.HRMEM_EmailId).FirstOrDefault();

                string mailid = "accounts@vapstech.com"; //"goutamkumar@vapstech.com";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("IndentApprove", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }
                    if (drivermailid != null) { message.AddBcc(drivermailid); }
                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);                        
                        message.HtmlContent = message.HtmlContent.Replace("[STATUS]", status);
                        message.HtmlContent = message.HtmlContent.Replace("[REMARK]", obj.ISMDIT_Remark);
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", obj.get_indent_status[index].HRME_EmployeeFirstName);
                        message.HtmlContent = message.HtmlContent.Replace("[VEHICLENO]", obj.get_indent_status[index].TRMV_VehicleNo);
                        message.HtmlContent = message.HtmlContent.Replace("[FUELQUANTITY]", obj.get_indent_status[index].ISMDIT_Qty.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[AMOUNT]", obj.get_indent_status[index].ISMDIT_Amount.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[OPENINGKM]", obj.get_indent_status[index].ISMDIT_OpeningKM.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[CLOSINGKM]", obj.get_indent_status[index].ISMDIT_ClosingKM.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[BALANCEDIESEL]", obj.get_indent_status[index].ISMDIT_BalanceDiesel.ToString());
                        message.HtmlContent = message.HtmlContent.Replace("[ROLE]", "ADMIN");
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("IndentApprove", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
