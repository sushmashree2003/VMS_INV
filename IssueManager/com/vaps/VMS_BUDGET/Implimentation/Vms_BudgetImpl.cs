using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.VMS_BUDGET.Implimentation
{
    public class Vms_BudgetImpl : Interface.Vms_BudgetInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;

        public Vms_BudgetImpl(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }
        public CheckBook_BudgetHeadsDTO getalldata1(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                data.allcompany = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                data.FinancialYear = _db.IVRM_Master_FinancialYear.Distinct().ToArray();
                data.Month_List = _context.IVRM_Month_DMO.Where(R => R.Is_Active == true).Distinct().ToArray();
                // data.Amount_List = _context.VMS_Master_BudgetHeads_newDMO.Where(M => M.VMBDGHDN_ActiveFlg == true).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //public CheckBook_BudgetHeadsDTO deactive1(CheckBook_BudgetHeadsDTO data)
        //{


        //    try
        //    {

        //        if (data.VMBDGHD_Id > 0)
        //        {
        //            var obj = _context.VMS_Master_BudgetHeads_newDMO.Where(R => R.VMBDGHDN_Id == data.VMBDGHD_Id).FirstOrDefault();
        //            if (obj.VMBDGHDN_ActiveFlg == true)
        //            {
        //                obj.VMBDGHDN_ActiveFlg = false;
        //            }
        //            else
        //            {
        //                obj.VMBDGHDN_ActiveFlg = true;
        //            }
        //            _context.Update(obj);
        //            int i = _context.SaveChanges();
        //            if (i > 0)
        //            {
        //                data.returnval = "active";
        //            }
        //            else
        //            {
        //                data.returnval = "Notactive";
        //            }
        //        }
        //        else
        //        {
        //            data.returnval = "admin";
        //        }
        //    }

        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //        data.returnval = "admin";
        //    }

        //    return data;
        //}


        public CheckBook_BudgetHeadsDTO deactive1(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                if (data.VMBDGHD_Id > 0)
                {
                    var obj = _context.VMS_Master_BudgetHeads_newDMO.Where(R => R.VMBDGHDN_Id == data.VMBDGHD_Id).FirstOrDefault();
                    if (obj != null)
                    {
                        if (obj.VMBDGHDN_ActiveFlg == true)
                        {
                            obj.VMBDGHDN_ActiveFlg = false;

                        }
                        else
                        {
                            obj.VMBDGHDN_ActiveFlg = true;

                        }

                        _context.Update(obj);
                        int i = _context.SaveChanges();

                        if (i > 0)
                        {

                            if (obj.VMBDGHDN_ActiveFlg == true)
                            {

                                data.returnval = "activated";
                            }
                            else
                            {
                                data.returnval = "deactivated";
                            }

                        }

                    }
                    else
                    {
                        data.returnval = "NotFound";
                    }
                }

                else if (data.VMBDGHDAMTN_Id > 0)
                {
                    var obj = _context.VMS_Master_BudgetHeads_Amount_newDMO.Where(R => R.VMBDGHDAMTN_Id == data.VMBDGHDAMTN_Id).FirstOrDefault();
                    if (obj != null)
                    {
                        if (obj.VMBDGHDAMTN_ActiveFlg == true)
                        {
                            obj.VMBDGHDAMTN_ActiveFlg = false;

                        }
                        else
                        {
                            obj.VMBDGHDAMTN_ActiveFlg = true;

                        }

                        _context.Update(obj);
                        int i = _context.SaveChanges();

                        if (i > 0)
                        {
                            if (obj.VMBDGHDAMTN_ActiveFlg == true)
                            {

                                data.returnval = "activated";
                            }
                            else
                            {
                                data.returnval = "deactivated";
                            }

                        }
                    }
                    else
                    {
                        data.returnval = "NotFound";
                    }
                }

                else
                {
                    data.returnval = "admin";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";  // Error case
            }

            return data;
        }


        public CheckBook_BudgetHeadsDTO Savedata(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                if (data.VMBDGHD_Id > 0)
                {
                    var duplicate = _context.VMS_Master_BudgetHeads_newDMO.Where(R => R.VMBDGHDN_BudgetHead == data.VMBDGHD_BudgetHead && R.VMBDGHDN_Id != data.VMBDGHD_Id && R.MI_Id == data.MI_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _context.VMS_Master_BudgetHeads_newDMO.FirstOrDefault(R => R.VMBDGHDN_Id == data.VMBDGHD_Id);
                        obj.VMBDGHDN_BudgetHead = data.VMBDGHD_BudgetHead;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.VMBDGHDN_MonthwiseBudgetFlg = data.VMBDGHD_MonthwiseBudgetFlg;
                        obj.VMBDGHDN_Remarks = data.VMBDGHD_Remarks;
                        obj.VMBDGHDN_ActiveFlg = true;
                        obj.VMBDGHDN_CreatedBy = data.UserId;
                        obj.VMBDGHDN_CreatedDate = DateTime.Now;
                        obj.VMBDGHDN_UpdatedDate = DateTime.Now;
                        obj.VMBDGHDN_UpdatedBy = data.UserId;
                        obj.ISMMCLT_Id = data.ISMMCLT_Id;
                        _context.Update(obj);
                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "Update";
                        }
                        else
                        {
                            data.returnval = "NotUpdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _context.VMS_Master_BudgetHeads_newDMO
                                .Where(R => R.VMBDGHDN_BudgetHead == data.VMBDGHD_BudgetHead
                                            && R.MI_Id == data.MI_Id)
                                .ToList();

                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        VMS_Master_BudgetHeads_newDMO obj = new VMS_Master_BudgetHeads_newDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.VMBDGHDN_BudgetHead = data.VMBDGHD_BudgetHead;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.VMBDGHDN_MonthwiseBudgetFlg = data.VMBDGHD_MonthwiseBudgetFlg;
                        obj.VMBDGHDN_Remarks = data.VMBDGHD_Remarks;
                        obj.VMBDGHDN_ActiveFlg = true;
                        obj.VMBDGHDN_CreatedBy = data.UserId;
                        obj.VMBDGHDN_CreatedDate = DateTime.Now;
                        obj.VMBDGHDN_UpdatedDate = DateTime.Now;
                        obj.VMBDGHDN_UpdatedBy = data.UserId;
                        obj.VMBDGHDN_SatuswiseFlg = data.VMBDGHDN_SatuswiseFlg;
                        obj.ISMMCLT_Id = data.ISMMCLT_Id;
                        _context.Add(obj);
                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "Save";
                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                        //  data.returnval = "admin";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }

            return data;
        }
        public CheckBook_BudgetHeadsDTO Getdata1(CheckBook_BudgetHeadsDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHECKBOOK_BudgetNew";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.Flag
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
                        data.alldata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.BudgetArray = _context.VMS_Master_BudgetHeads_newDMO.Where(R => R.MI_Id == data.MI_Id && R.HRMD_Id == data.HRMD_Id && R.VMBDGHDN_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return data;
        }
        public CheckBook_BudgetHeadsDTO editBudget(CheckBook_BudgetHeadsDTO data)

        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Budget_Edit";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMBDGHDAMTN_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.VMBDGHDAMTN_Id
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
                        data.Month_List = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public CheckBook_BudgetHeadsDTO Getdept(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHECKBOOK_BudgetNew";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.Flag
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
                        data.alldata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BUDGET_Department";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
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
                        data.deptList = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.BudgetArray = _context.VMS_Master_BudgetHeads_newDMO.Where(a => a.MI_Id == data.MI_Id && a.VMBDGHDN_ActiveFlg == true).Distinct().ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Clientwise_Budget";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
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
                        data.clientList = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return data;
        }

        public CheckBook_BudgetHeadsDTO ApprovalData1(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                if (data.VMBDGHDAMT_Id > 0)
                {
                    var duplicate = _context.VMS_Master_BudgetHeads_Amount_newDMO.Where(R => R.VMBDGHDN_Id == data.VMBDGHD_Id && R.VMBDGHDAMTN_Id != data.VMBDGHDAMT_Id && R.IMFY_ID == data.IMFY_ID && R.VMBDGHDAMTN_Amount == data.VMBDGHDAMT_Amount).ToList();
                    if (duplicate.Any())
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _context.VMS_Master_BudgetHeads_Amount_newDMO.FirstOrDefault(R => R.VMBDGHDAMTN_Id == data.VMBDGHDAMT_Id);
                        obj.VMBDGHDAMTN_Amount = data.VMBDGHDAMT_Amount;
                        obj.VMBDGHDAMTN_MonthwiseAmount = data.VMBDGHDAMT_MonthwiseAmount;
                        obj.VMBDGHDAMTN_Remarks = data.VMBDGHDAMT_Remarks;
                        obj.VMBDGHDN_Id = data.VMBDGHD_Id;
                        obj.IMFY_ID = data.IMFY_ID;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.VMBDGHDAMTN_Name = data.VMBDGHDAMTN_Name;
                        obj.VMBDGHDAMTN_UpdatedDate = DateTime.Now;
                        obj.VMBDGHDAMTN_UpdatedBy = data.UserId;
                        obj.ISMMCLT_Id = data.ISMMCLT_Id;
                        _context.Update(obj);
                        if (data.Amount_List != null && data.Amount_List.Length > 0)
                        {
                            var duplicateMonth = _context.VMS_Master_BudgetHeads_Amount_MonthDMO.Where(R => R.VMBDGHDAMTN_Id == data.VMBDGHDAMT_Id).ToList();
                            if (duplicateMonth.Count > 0)
                            {
                                foreach (var dd in duplicateMonth)
                                {
                                    _context.Remove(dd);
                                }
                            }

                            foreach (var ss in data.Amount_List)
                            {
                                VMS_Master_BudgetHeads_Amount_MonthDMO childObj = new VMS_Master_BudgetHeads_Amount_MonthDMO();
                                childObj.VMBDGHDAMTN_Id = obj.VMBDGHDAMTN_Id;
                                childObj.VMBDGHDAMTNM_MonthId = ss.VMBDGHDAMTNM_MonthId;
                                childObj.VMBDGHDAMTNM_Amount = ss.VMBDGHDAMTNM_Amount;
                                childObj.VMBDGHDAMTNM_ActiveFlag = true;
                                childObj.VMBDGHDAMTNM_CreatedDate = DateTime.Now;
                                childObj.VMBDGHDAMTNM_UpdatedDate = DateTime.Now;
                                childObj.VMBDGHDAMTNM_CreatedBy = data.UserId;
                                _context.Add(childObj);
                            }
                        }
                        int i = _context.SaveChanges();
                        data.returnval = i > 0 ? "update" : "Notupdate";

                    }
                }
                else
                {
                    var duplicate = _context.VMS_Master_BudgetHeads_Amount_newDMO.Where(R => R.VMBDGHDN_Id == data.VMBDGHD_Id
                    && R.IMFY_ID == data.IMFY_ID && R.VMBDGHDAMTN_Name == data.VMBDGHDAMTN_Name).ToList();
                    if (duplicate.Any())
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        VMS_Master_BudgetHeads_Amount_newDMO obj = new VMS_Master_BudgetHeads_Amount_newDMO();
                        obj.VMBDGHDN_Id = data.VMBDGHD_Id;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.IMFY_ID = data.IMFY_ID;
                        obj.VMBDGHDAMTN_Amount = data.VMBDGHDAMT_Amount;
                        obj.VMBDGHDAMTN_MonthwiseAmount = data.VMBDGHDAMT_MonthwiseAmount;
                        obj.VMBDGHDAMTN_Remarks = data.VMBDGHDAMT_Remarks;
                        obj.VMBDGHDAMTN_Name = data.VMBDGHDAMTN_Name;
                        obj.VMBDGHDAMTN_ActiveFlg = true;
                        obj.VMBDGHDAMTN_CreatedBy = data.UserId;
                        obj.VMBDGHDAMTN_UpdatedBy = data.UserId;
                        obj.VMBDGHDAMTN_CreatedDate = DateTime.Now;
                        obj.VMBDGHDAMTN_UpdatedDate = DateTime.Now;
                        obj.ISMMCLT_Id = data.ISMMCLT_Id;
                        _context.Add(obj);

                        if (data.Amount_List != null && data.Amount_List.Length > 0)
                        {
                            foreach (var ss in data.Amount_List)
                            {
                                VMS_Master_BudgetHeads_Amount_MonthDMO childObj = new VMS_Master_BudgetHeads_Amount_MonthDMO();
                                childObj.VMBDGHDAMTN_Id = obj.VMBDGHDAMTN_Id;
                                childObj.VMBDGHDAMTNM_MonthId = ss.VMBDGHDAMTNM_MonthId;
                                childObj.VMBDGHDAMTNM_Amount = ss.VMBDGHDAMTNM_Amount;
                                childObj.VMBDGHDAMTNM_ActiveFlag = true;
                                childObj.VMBDGHDAMTNM_CreatedDate = DateTime.Now;
                                childObj.VMBDGHDAMTNM_UpdatedDate = DateTime.Now;
                                childObj.VMBDGHDAMTNM_CreatedBy = data.UserId;
                                _context.Add(childObj);
                            }
                        }

                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "Save";
                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error instead of console write
                Console.WriteLine($"Error: {ex.Message}");
                data.returnval = "admin";
            }

            return data;
        }


        public CheckBook_BudgetHeadsDTO edit1(CheckBook_BudgetHeadsDTO data)

        {
            try
            {
                if (data.VMBDGHDAMT_Id > 0)
                {
                    var obj = _context.VMS_Master_BudgetHeads_Amount_newDMO.Where(R => R.VMBDGHDAMTN_Id == data.VMBDGHDAMT_Id).FirstOrDefault();
                    if (obj.VMBDGHDAMTN_ActiveFlg == true)
                    {
                        obj.VMBDGHDAMTN_ActiveFlg = false;
                    }
                    else
                    {
                        obj.VMBDGHDAMTN_ActiveFlg = true;
                    }
                    obj.VMBDGHDAMTN_UpdatedBy = data.UserId;
                    obj.VMBDGHDAMTN_UpdatedDate = DateTime.Now;
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "Notactive";
                    }
                }
                else
                {
                    data.returnval = "admin";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public CheckBook_BudgetHeadsDTO BudgetHeadsReport1(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                data.allcompany = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                data.FinancialYear = _db.IVRM_Master_FinancialYear.Distinct().ToArray();
                data.deptList = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().ToArray();

                data.Month_List = _context.IVRM_Month_DMO.Where(R => R.Is_Active == true).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        //GetReport

        public CheckBook_BudgetHeadsDTO GetReport(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                var dept_ids = "0";
                if (data.departmentSelected != null && data.departmentSelected.Length > 0)
                {
                    foreach (var ii in data.departmentSelected)
                    {
                        dept_ids = dept_ids + "," + ii.HRMD_Id.ToString();
                    }

                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetHeadsWise_Amount_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = dept_ids });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt) { Value = data.IMFY_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag", SqlDbType.VarChar) { Value = data.Flag });
                    //cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.BigInt) { Value = data.MonthId });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetHeadsWise_Amount_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = dept_ids });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt) { Value = data.IMFY_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag", SqlDbType.VarChar) { Value = data.Flag });
                    //cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.BigInt) { Value = data.MonthId });
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.DepartmentArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //OverallArray
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetHeadsWise_Amount_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = dept_ids });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt) { Value = data.IMFY_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag", SqlDbType.VarChar) { Value = data.Flag });
                    //cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.BigInt) { Value = data.MonthId });
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.OverallArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetHeadsWise_Amount_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = dept_ids });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt) { Value = data.IMFY_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "4" });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag", SqlDbType.VarChar) { Value = data.Flag });
                    //cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.BigInt) { Value = data.MonthId });
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.OverallTotal = retObject.ToArray();
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
        //GetdeptClient
        public CheckBook_BudgetHeadsDTO GetdeptClient(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetDepatClient_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.Flag });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.alldata = retObject.ToArray();
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



        public CheckBook_BudgetHeadsDTO BudgetApprovalUpdate(CheckBook_BudgetHeadsDTO data)
        {

            try
            {
                foreach( var b in data.saveremrksdata)
                {
                    var contactExistsP = _context.Database.ExecuteSqlCommand("Budget_Amount_Update @p0,@p1,@p2,@p3,@p4", data.MI_Id, b.HRMD_Id, b.VMBDGHDAMTN_Id, b.VMBDGHDN_Id, b.TotalAmount);

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Budget_Approval";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@VMBDGHDN_Id_N", SqlDbType.BigInt) { Value = b.VMBDGHDN_Id });
                        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = data.Flag });
                        cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt) { Value = data.UserId });
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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.OverallTotal = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (contactExistsP > 0)
                    {
                        data.returnval = "Save";
                    }
                    else
                    {
                        data.returnval = "NotSave";
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }






        public CheckBook_BudgetHeadsDTO GetApprovalReport(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                var dept_ids = "0";
                if (data.departmentSelected != null && data.departmentSelected.Length > 0)
                {
                    foreach (var ii in data.departmentSelected)
                    {
                        dept_ids = dept_ids + "," + ii.HRMD_Id.ToString();
                    }

                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetHeadsWise_Amount_Report_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = dept_ids });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt) { Value = data.IMFY_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag", SqlDbType.VarChar) { Value = data.Flag });
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt) { Value = data.UserId });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetHeadsWise_Amount_Report_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = dept_ids });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt) { Value = data.IMFY_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag", SqlDbType.VarChar) { Value = data.Flag });
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt) { Value = data.UserId });
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.DepartmentArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //OverallArray
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetHeadsWise_Amount_Report_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = dept_ids });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt) { Value = data.IMFY_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag", SqlDbType.VarChar) { Value = data.Flag });
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt) { Value = data.UserId });
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.OverallArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_BudgetHeadsWise_Amount_Report_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = dept_ids });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt) { Value = data.IMFY_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "4" });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag", SqlDbType.VarChar) { Value = data.Flag });

                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt) { Value = data.UserId });
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.OverallTotal = retObject.ToArray();
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
    }

}

