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

namespace IssueManager.com.vaps.Services
{
    public class CheckBook_BudgetHeadsIMPL : Interfaces.CheckBook_BudgetHeadsInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;

        public CheckBook_BudgetHeadsIMPL(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }


        public CheckBook_BudgetHeadsDTO getalldata(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                data.allcompany = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                data.FinancialYear = _db.IVRM_Master_FinancialYear.Distinct().ToArray();
                data.Month_List = _context.IVRM_Month_DMO.Where(R => R.Is_Active == true).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        // Getdata
        public CheckBook_BudgetHeadsDTO Getdata(CheckBook_BudgetHeadsDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHECKBOOK_Budget";
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

                data.BudgetArray = _context.VMS_Master_BudgetHeadsDMO.Where(R => R.MI_Id == data.MI_Id && R.VMBDGHD_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return data;
        }
        //TadaAllwence
        public CheckBook_BudgetHeadsDTO TadaAllwence(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                string MI_Id = "0";
                if (data.MultipleComapnyBudget != null && data.MultipleComapnyBudget.Length > 0)
                {
                    foreach (var d in data.MultipleComapnyBudget)
                    {
                        MI_Id = MI_Id + "," + d.MI_Id;
                    }
                }
                else
                {
                    MI_Id = data.MI_Id.ToString();
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chequebook_BudgetlistReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.IMFY_ID
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
                        data.CheckBookBudget = retObject.ToArray();
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


        public CheckBook_BudgetHeadsDTO savedta(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                if (data.VMBDGHD_Id > 0)
                {
                    var duplicate = _context.VMS_Master_BudgetHeadsDMO.Where(R => R.VMBDGHD_BudgetHead == data.VMBDGHD_BudgetHead && R.VMBDGHD_Id != data.VMBDGHD_Id && R.MI_Id == data.MI_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _context.VMS_Master_BudgetHeadsDMO.Where(R => R.VMBDGHD_Id == data.VMBDGHD_Id).FirstOrDefault();
                        obj.VMBDGHD_BudgetHead = data.VMBDGHD_BudgetHead;
                        obj.VMBDGHD_MonthwiseBudgetFlg = data.VMBDGHD_MonthwiseBudgetFlg;
                        obj.VMBDGHD_Remarks = data.VMBDGHD_Remarks;
                        obj.VMBDGHD_ActiveFlg = true;
                        obj.VMBDGHD_CreatedBy = data.UserId;
                        obj.VMBDGHD_CreatedDate = DateTime.Now;
                        obj.VMBDGHD_UpdatedDate = DateTime.Now;
                        obj.VMBDGHD_UpdatedBy = data.UserId;
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
                    var duplicate = _context.VMS_Master_BudgetHeadsDMO.Where(R => R.VMBDGHD_BudgetHead == data.VMBDGHD_BudgetHead && R.MI_Id == data.MI_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        VMS_Master_BudgetHeadsDMO obj = new VMS_Master_BudgetHeadsDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.VMBDGHD_BudgetHead = data.VMBDGHD_BudgetHead;
                        obj.VMBDGHD_MonthwiseBudgetFlg = data.VMBDGHD_MonthwiseBudgetFlg;
                        obj.VMBDGHD_Remarks = data.VMBDGHD_Remarks;
                        obj.VMBDGHD_ActiveFlg = true;
                        obj.VMBDGHD_CreatedBy = data.UserId;
                        obj.VMBDGHD_CreatedDate = DateTime.Now;
                        obj.VMBDGHD_UpdatedDate = DateTime.Now;
                        obj.VMBDGHD_UpdatedBy = data.UserId;
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
        //edit
        public CheckBook_BudgetHeadsDTO edit(CheckBook_BudgetHeadsDTO data)

        {
            try
            {
                if (data.VMBDGHDAMT_Id > 0)
                {
                    var obj = _context.VMS_Master_BudgetHeads_AmountDMO.Where(R => R.VMBDGHDAMT_Id == data.VMBDGHDAMT_Id).FirstOrDefault();
                    if (obj.VMBDGHDAMT_ActiveFlg == true)
                    {
                        obj.VMBDGHDAMT_ActiveFlg = false;
                    }
                    else
                    {
                        obj.VMBDGHDAMT_ActiveFlg = true;
                    }
                    obj.VMBDGHDAMT_UpdatedBy = data.UserId;
                    obj.VMBDGHDAMT_UpdatedDate = DateTime.Now;
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

        public CheckBook_BudgetHeadsDTO deactive(CheckBook_BudgetHeadsDTO data)
        {


            try
            {

                if (data.VMBDGHD_Id > 0)
                {
                    var obj = _context.VMS_Master_BudgetHeadsDMO.Where(R => R.VMBDGHD_Id == data.VMBDGHD_Id).FirstOrDefault();
                    if (obj.VMBDGHD_ActiveFlg == true)
                    {
                        obj.VMBDGHD_ActiveFlg = false;
                    }
                    else
                    {
                        obj.VMBDGHD_ActiveFlg = true;
                    }
                    _context.Update(obj);
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
        public CheckBook_BudgetHeadsDTO ApprovalData(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                if (data.VMBDGHDAMT_Id > 0)
                {
                    var duplicate = _context.VMS_Master_BudgetHeads_AmountDMO.Where(R => R.VMBDGHD_Id == data.VMBDGHD_Id && R.VMBDGHDAMT_Id != data.VMBDGHDAMT_Id && R.IMFY_ID == data.IMFY_ID && R.VMBDGHDAMT_Amount == data.VMBDGHDAMT_Amount).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _context.VMS_Master_BudgetHeads_AmountDMO.Where(R => R.VMBDGHDAMT_Id == data.VMBDGHDAMT_Id).FirstOrDefault();
                        obj.VMBDGHDAMT_Amount = data.VMBDGHDAMT_Amount;
                        obj.VMBDGHDAMT_MonthwiseAmount = data.VMBDGHDAMT_MonthwiseAmount;
                        obj.VMBDGHD_Id = data.VMBDGHD_Id;
                        obj.IMFY_ID = data.IMFY_ID;
                        obj.VMBDGHDAMT_UpdatedDate = DateTime.Now;
                        obj.VMBDGHDAMT_UpdatedBy = data.UserId;
                        _context.Update(obj);
                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    //  && R.VMBDGHDAMT_Amount == data.VMBDGHDAMT_Amount
                    var duplicate = _context.VMS_Master_BudgetHeads_AmountDMO.Where(R => R.VMBDGHD_Id == data.VMBDGHD_Id && R.IMFY_ID == data.IMFY_ID).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        VMS_Master_BudgetHeads_AmountDMO obj = new VMS_Master_BudgetHeads_AmountDMO();
                        obj.VMBDGHD_Id = data.VMBDGHD_Id;
                        obj.IMFY_ID = data.IMFY_ID;
                        obj.VMBDGHDAMT_Amount = data.VMBDGHDAMT_Amount;
                        obj.VMBDGHDAMT_MonthwiseAmount = data.VMBDGHDAMT_MonthwiseAmount;
                        obj.VMBDGHDAMT_Remarks = data.VMBDGHDAMT_Remarks;
                        obj.VMBDGHDAMT_ActiveFlg = true;
                        obj.VMBDGHDAMT_CreatedBy = data.UserId;
                        obj.VMBDGHDAMT_UpdatedBy = data.UserId;
                        obj.VMBDGHDAMT_CreatedDate = DateTime.Now;
                        obj.VMBDGHDAMT_UpdatedDate = DateTime.Now;
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

        public CheckBook_BudgetHeadsDTO BudgetReport(CheckBook_BudgetHeadsDTO data)
        {
            try
            {
                string VMBDGHD_Id = "0";
                string MI_Id = "0";
                List<long> MI_IdList = new List<long>();
                if (data.BudgetHeadlist != null && data.BudgetHeadlist.Length > 0)
                {
                    foreach (var i in data.BudgetHeadlist)
                    {
                        VMBDGHD_Id = VMBDGHD_Id + ',' + i.VMBDGHD_Id;


                    }
                }
                if (data.MultipleComapnyBudget != null && data.MultipleComapnyBudget.Length > 0)
                {
                    foreach (var d in data.MultipleComapnyBudget)
                    {
                        MI_Id = MI_Id + "," + d.MI_Id;
                        MI_IdList.Add(d.MI_Id);
                    }
                }
                else
                {
                    MI_Id = data.MI_Id.ToString();
                    MI_IdList.Add(data.MI_Id);
                }
                string IVRM_Month_Id = "0";
                if (data.temMonth_List != null && data.temMonth_List.Length > 0)
                {
                    foreach (var d in data.temMonth_List)
                    {
                        IVRM_Month_Id = IVRM_Month_Id + "," + d.IVRM_Month_Id;
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chequebook_BudgetReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.IMFY_ID
                    });
                    //@OptionFlag
                    cmd.Parameters.Add(new SqlParameter("@OptionFlag",
                  SqlDbType.VarChar)
                    {
                        Value = data.returnval
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMBDGHD_Id",
               SqlDbType.VarChar)
                    {
                        Value = VMBDGHD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MONTH_Id",
               SqlDbType.VarChar)
                    {
                        Value = IVRM_Month_Id
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
                        data.BudgetReport = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                //Monthwise
                if(data.returnval== "Monthwise")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Chequebook_BudgetReport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@IMFY_ID",
                        SqlDbType.VarChar)
                        {
                            Value = data.IMFY_ID
                        });
                        //@OptionFlag
                        cmd.Parameters.Add(new SqlParameter("@OptionFlag",
                      SqlDbType.VarChar)
                        {
                            Value = "MonthTotal"
                        });
                        cmd.Parameters.Add(new SqlParameter("@VMBDGHD_Id",
                   SqlDbType.VarChar)
                        {
                            Value = VMBDGHD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTH_Id",
                   SqlDbType.VarChar)
                        {
                            Value = IVRM_Month_Id
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
                            data.BudgetReportotal = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                }

                data.Mi_logo = _context.Institution.Where(R => MI_IdList.Contains(R.MI_Id)).Select(R => R.MI_Logo).FirstOrDefault();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return data;
        }
    }
}
