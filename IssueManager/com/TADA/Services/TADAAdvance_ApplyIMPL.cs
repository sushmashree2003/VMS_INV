using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.TADA;
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
using CommonLibrary;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Services
{
    public class TADAAdvance_ApplyIMPL : Interface.TADA_Advance_Interface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;
        public TADAAdvance_ApplyIMPL(IssueManagerContext context, DomainModelMsSqlServerContext dbcontext)
        {
            _context = context;
            _db = dbcontext;
        }
        public TADA_Advance_ApplyDTO getalldata(TADA_Advance_ApplyDTO data)
        {
            try
            {
                List<TADA_Advance_ApplyDTO> Tasklist = new List<TADA_Advance_ApplyDTO>();
                data.state = _context.IVRM_Master_StateDMO.Distinct().ToArray();
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_PlannerExtension_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Statusflag",
                     SqlDbType.VarChar)
                    {
                        Value = "TAADA"
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

                        data.Allowense = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (data.Allowense != null && data.Allowense.Length > 0)
                {
                    data.Client_Master = _context.clientTable.Where(t => t.ISMMCLT_ActiveFlag == true).Distinct().ToArray();                
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TADA_Apply_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                          SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                         SqlDbType.VarChar)
                        {
                            Value = "TAAAD"
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.masterCountry = _context.Country.Distinct().ToArray();


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADA_ADVANCE_EMP_LIST";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
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
                                    Tasklist.Add(new TADA_Advance_ApplyDTO
                                    {
                                        VTADAAA_Id = Convert.ToInt64(dataReader["vtadaaA_Id"]),


                                    });
                                }
                            }

                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    string VTADAAA_StatusFlg = "Approved";
                    List<long> VTADAAA_Ids = new List<long>();

                    List<long> VTADAAA_Exceed = new List<long>();
                    if (Tasklist != null && Tasklist.Count > 0)
                    {
                        foreach (var d in Tasklist)
                        {
                            VTADAAA_Exceed.Add(d.VTADAAA_Id);
                        }
                    }

                    var Olddata = _context.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_ActiveFlg == true && !VTADAAA_Exceed.Contains(R.VTADAAA_Id) && (R.VTADAAA_StatusFlg == "Approved" || R.VTADAAA_StatusFlg == "Pending") && R.HRME_Id == data.HRME_Id).ToList();
                    data.CountBalance = false;
                    if (Olddata.Count > 0)
                    {
                        foreach (var d in Olddata)
                        {
                            VTADAAA_Ids.Add(d.VTADAAA_Id);
                        }

                        var NewData = _context.TADA_ApplyDMO.Where(R => R.MI_Id == data.MI_Id && R.VTADAA_ActiveFlg == true && VTADAAA_Ids.Contains(R.VTADAAA_Id.Value)).ToList();

                        if (Olddata.Count == NewData.Count)
                        {
                            data.CountBalance = false;
                        }
                        else
                        {
                            data.CountBalance = true;
                        }
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //getstate
        public TADA_Advance_ApplyDTO getstate(TADA_Advance_ApplyDTO data)
        {
            try
            {
                var hrMdesId = (from d in _context.Staff_User_Login
                                from e in _context.MasterEmployee
                                where (d.Id == data.UserId && e.HRME_Id == d.Emp_Code)
                                select new TADA_Advance_ApplyDTO
                                {
                                    HRMDES_Id = Convert.ToInt64(e.HRMDES_Id)
                                }
                                 ).ToList();
                if (hrMdesId.Count == 1)
                {
                    data.City_Type = (from a in _context.TADA_CityMappingDMO
                                      from c in _context.IVRM_Master_CityDMO
                                      from d in _context.IVRM_Master_StateDMO
                                      where (a.IVRMMCT_Id == c.IVRMMCT_Id && a.VTADACM_ActiveFlag == true && a.HRMDES_Id == hrMdesId[0].HRMDES_Id && d.IVRMMS_Id == c.IVRMMS_Id && c.IVRMMS_Id == data.IVRMMS_Id)
                                      select new TADA_Advance_ApplyDTO
                                      {
                                          IVRMMCT_Id = a.IVRMMCT_Id,
                                          IVRMMCT_Name = c.IVRMMCT_Name
                                      }

                ).Distinct().ToArray();

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_Advance_ApplyDTO TadaAllwence(TADA_Advance_ApplyDTO data)
        {
            try
            {
                var hrMdesId = (from d in _context.Staff_User_Login
                                from e in _context.MasterEmployee
                                where (d.Id == data.UserId && e.HRME_Id == d.Emp_Code)
                                select new TADA_Advance_ApplyDTO
                                {
                                    HRMDES_Id = Convert.ToInt64(e.HRMDES_Id)
                                }
                                   ).ToList();
                if (hrMdesId.Count == 1)
                {
                    data.Allowense = _context.TADA_CityMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.IVRMMCT_Id == data.IVRMMCT_Id && t.VTADACM_ActiveFlag == true && t.HRMDES_Id == hrMdesId[0].HRMDES_Id).Distinct().ToArray();
                }
                string startdate = "";
                string enddate = "";
                if (data.VTADAAA_FromDate != null && data.VTADAAA_ToDate != null)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                    startdate = data.VTADAAA_FromDate.Value.ToString("yyyy-MM-dd");
                    enddate = data.VTADAAA_ToDate.Value.ToString("yyyy-MM-dd");

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADA_DailyReportApproval";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
              SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@From_Date",
                      SqlDbType.VarChar)
                        {
                            Value = startdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@To_Date",
                        SqlDbType.VarChar)
                        {
                            Value = enddate
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                        SqlDbType.VarChar)
                        {
                            Value = "TADA_Adavance"
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
                            data.Client_Master = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_Advance_ApplyDTO edit(TADA_Advance_ApplyDTO data)
        {
            try
            {
                var list = _context.VMS_TADA_Advance_Apply_AuthorisationDMO.Where(p => p.VTADAAA_Id == data.VTADAAA_Id).ToList();

                if (data.VTADAAA_Id > 0 && list.Count == 0)
                {
                    data.EditArray = (from a in _context.TADA_AdavanceApplyDMO
                                      from b in _context.IVRM_Master_CityDMO
                                      from c in _context.IVRM_Master_StateDMO
                                      where (a.IVRMMCT_Id == b.IVRMMCT_Id && a.MI_Id == data.MI_Id && a.VTADAAA_ActiveFlg == true && a.VTADAAA_StatusFlg == "Pending" && a.VTADAAA_Id == data.VTADAAA_Id && b.IVRMMS_Id == c.IVRMMS_Id)
                                      select new TADA_Advance_ApplyDTO
                                      {
                                          VTADAAA_ClientId = a.VTADAAA_ClientId,
                                          VTADAAA_FromDate = a.VTADAAA_FromDate,
                                          VTADAAA_ToDate = a.VTADAAA_ToDate,
                                          IVRMMCT_Id = a.IVRMMCT_Id,
                                          IVRMMCT_Name = b.IVRMMCT_Name,
                                          VTADAAA_TotalAppliedAmount = a.VTADAAA_TotalAppliedAmount,
                                          VTADAAA_ToAddress = a.VTADAAA_ToAddress,
                                          VTADAAA_Remarks = a.VTADAAA_Remarks,
                                          VTADAAA_Id = a.VTADAAA_Id,
                                          IVRMMS_Id = c.IVRMMS_Id,
                                          IVRMMS_Name = c.IVRMMS_Name,
                                          VTADAAA_ArrivalTime = a.VTADAAA_ArrivalTime,
                                          VTADAAA_DepartureTime = a.VTADAAA_DepartureTime,
                                          VTADAAA_ClientMultiple = a.VTADAAA_ClientMultiple,
                                      }
                                    ).Distinct().ToArray();
                    data.editmuticlientarray = _context.VMS_TADDA_MultipleClient_FilesDMO.Where(m => m.VTADAAA_Id == data.VTADAAA_Id && m.VTADDAMCF_ActiveFlag == true).ToArray();//added by adarsh
                    data.EditArrayTwo = _context.TADA_AdvanceApplyDetailsDMO.Where(t => t.VTADAAA_Id == data.VTADAAA_Id && t.VTADAAAD_ActiveFlg == true).Distinct().ToArray();

                    var hrMdesId = (from d in _context.Staff_User_Login
                                    from e in _context.MasterEmployee
                                    where (d.Id == data.UserId && e.HRME_Id == d.Emp_Code)
                                    select new TADA_Advance_ApplyDTO
                                    {
                                        HRMDES_Id = Convert.ToInt64(e.HRMDES_Id)
                                    }
                                   ).ToList();
                    if (hrMdesId.Count == 1)
                    {
                        //data.Allowense = _context.TADA_CityMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.IVRMMCT_Id == data.IVRMMCT_Id && t.VTADACM_ActiveFlag == true && t.HRMDES_Id == hrMdesId[0].HRMDES_Id).Distinct().ToArray();
                        data.Allowense = (from a in _context.TADA_AdavanceApplyDMO
                                          from t in _context.TADA_CityMappingDMO
                                          where (a.MI_Id == data.MI_Id && t.VTADACM_ActiveFlag == true
                                          && t.IVRMMCT_Id == a.IVRMMCT_Id && a.VTADAAA_Id == data.VTADAAA_Id && t.HRMDES_Id == hrMdesId[0].HRMDES_Id)
                                          select new TRDACityMappingDTO
                                          {
                                              VTADACM_FoodAmt = t.VTADACM_FoodAmt,
                                              VTADACM_TransportAmt = t.VTADACM_TransportAmt,
                                              VTADACM_AccommodationAmt = t.VTADACM_AccommodationAmt,
                                              VTADACM_Id = t.VTADACM_Id,
                                              VTADACM_ActiveFlag = t.VTADACM_ActiveFlag,
                                              VTADACM_TravelHour = t.VTADACM_TravelHour
                                          }
                                       ).Distinct().ToArray();
                    }
                }
                else
                {
                    data.returnvalue = true;
                    data.returnval = "Firststage";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_Advance_ApplyDTO savedta(TADA_Advance_ApplyDTO data)
        {
            try
            {
                data.returnvalue = false;
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                if (data.VTADAAA_Id == 0 && data.HRME_Id > 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TADA_Advance_Apply_Record_Checking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
                        string FromDate = data.VTADAAA_FromDate.Value.ToString("yyyy-MM-dd");
                        string ToDate = data.VTADAAA_ToDate.Value.ToString("yyyy-MM-dd");

                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });

                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
                        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        data.duplicate = cmd.Parameters["@Result"].Value.ToString();
                    }
                    if (data.duplicate == "0")
                    {
                        TADA_AddvanceApplyDMO obj = new TADA_AddvanceApplyDMO();
                        obj.VTADAAA_Id = data.VTADAAA_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.VTADAAA_AppliedDate = DateTime.Now;
                        obj.VTADAAA_FromDate = data.VTADAAA_FromDate;
                        obj.VTADAAA_ToDate = data.VTADAAA_ToDate;
                        obj.VTADAAA_ClientId = data.VTADAAA_ClientId;
                        obj.VTADAAA_ToAddress = data.VTADAAA_ToAddress;
                        obj.VTADAAA_Remarks = data.VTADAAA_Remarks;
                        obj.VTADAAA_TotalAppliedAmount = data.VTADAAA_TotalAppliedAmount;
                        obj.IVRMMCT_Id = data.IVRMMCT_Id;
                        obj.VTADAAA_DepartureTime = data.VTADAAA_DepartureTime;
                        obj.VTADAAA_ArrivalTime = data.VTADAAA_ArrivalTime;
                        obj.VTADAAA_ClientMultiple = data.VTADAAA_ClientMultiple;
                        obj.VTADAAA_StatusFlg = "Pending";
                        obj.VTADAAA_ActiveFlg = true;
                        obj.VTADAAA_CreatedBy = data.UserId;
                        obj.VTADAAA_UpdatedBy = data.UserId;
                        obj.VTADAAA_CreatedDate = DateTime.Now;
                        obj.VTADAAA_UpdatedDate = DateTime.Now;
                        _context.Add(obj);
                        for (int c = 0; c < data.AllowenceArray.Length; c++)
                        {
                            TADA_AddvanceApplyDetailsDMO dobj = new TADA_AddvanceApplyDetailsDMO();
                            dobj.VTADAAA_Id = obj.VTADAAA_Id;
                            dobj.VTADAAAD_ExpenditureHead = data.AllowenceArray[c].Type;
                            dobj.VTADAAAD_Amount = data.AllowenceArray[c].Amount;
                            dobj.VTADAAAD_Remarks = data.AllowenceArray[c].Remarks;
                            dobj.VTADAAAD_Slots = data.AllowenceArray[c].VTADAAAD_Slots;
                            dobj.VTADAAAD_TotalSlots = data.AllowenceArray[c].VTADAAAD_TotalSlots;
                            dobj.VTADAAAD_ActiveFlg = true;
                            dobj.VTADAAAD_StatusFlg = "Pending";
                            dobj.VTADAAAD_CreatedBy = data.UserId;
                            dobj.VTADAAAD_CreatedDate = DateTime.Now;
                            dobj.VTADAAAD_UpdatedBy = data.UserId;
                            dobj.VTADAAAD_UpdatedDate = DateTime.Now;
                            _context.Add(dobj);
                        }
                        for (int x = 0; x < data.multiple_client.Length; x++)
                        {
                            VMS_TADDA_MultipleClient_FilesDMO vtdmf = new VMS_TADDA_MultipleClient_FilesDMO();
                            vtdmf.VTADAAA_Id = obj.VTADAAA_Id;
                            vtdmf.ISMMCLT_Id = data.multiple_client[x].ISMMCLT_Id;
                            vtdmf.VTADDAMCF_Remarks = data.multiple_client[x].VTADDAMCF_Remarks;
                            vtdmf.VTADDAMCF_StatusFlg = "P";
                            vtdmf.VTADDAMCF_ActiveFlag = true;
                            vtdmf.VTADDAMCF_CreatedDate = DateTime.Now;
                            vtdmf.VTADDAMCF_UpdatedDate = DateTime.Now;
                            vtdmf.VTADDAMCF_CreatedBy = data.UserId;
                            vtdmf.VTADDAMCF_UpdatedBy = data.UserId;
                            _context.Add(vtdmf);
                        }

                        int a = _context.SaveChanges();
                        if (a > 0)
                        {
                            data.returnvalue = true;
                            data.returnval = "Insert";
                            string mailcc = "";
                            var checkmaxorder = (from P in _context.HR_PROCESSDMO
                                                 from R in _context.HR_Process_Auth_OrderNoDMO
                                                 where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "TAAD"
                                                 select new TADA_Advance_ApplyDTO
                                                 {
                                                     HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                                     IVRMUL_Id = R.IVRMUL_Id
                                                 }
                                                 ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                            if (checkmaxorder.Count > 0)
                            {
                                data.UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;
                                long Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                                data.cc_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            }
                            string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == obj.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            if (data.cc_Mail != null)
                            {
                                mailcc = data.cc_Mail;
                            }
                            // mailcc = "kanavisanjeev@gmail.com";
                            // string mailcctwo = "sanjeev@globalqtytrainig.com";                          
                            //  mailcc = data.cc_Mail + "," + mailcctwo;
                            //sujata@globalqtytrainig.com
                            // string emailid = "sanjeev@globalqtytrainig.com";
                            sendmail(data.MI_Id, emailid, "TADAAdavance_Apply", obj.VTADAAA_Id, mailcc);
                        }
                        else
                        {
                            data.returnvalue = true;
                            data.returnval = "Failed";
                        }
                    }
                    else
                    {
                        data.returnvalue = true;
                        data.returnval = "Duplicate";
                    }
                }
                if (data.VTADAAA_Id > 0 && data.HRME_Id > 0)
                {
                    var resultone = _context.TADA_AdavanceApplyDMO.Where(t => t.VTADAAA_Id == data.VTADAAA_Id && t.MI_Id == data.MI_Id).FirstOrDefault();
                    resultone.MI_Id = data.MI_Id;
                    resultone.HRME_Id = data.HRME_Id;
                    resultone.IVRMMCT_Id = data.IVRMMCT_Id;
                    resultone.VTADAAA_AppliedDate = DateTime.Now;
                    resultone.VTADAAA_FromDate = data.VTADAAA_FromDate;
                    resultone.VTADAAA_ToDate = data.VTADAAA_ToDate;
                    resultone.VTADAAA_ClientId = data.VTADAAA_ClientId;
                    resultone.VTADAAA_ToAddress = data.VTADAAA_ToAddress;
                    resultone.VTADAAA_Remarks = data.VTADAAA_Remarks;
                    resultone.VTADAAA_TotalAppliedAmount = data.VTADAAA_TotalAppliedAmount;
                    resultone.VTADAAA_UpdatedBy = data.UserId;
                    resultone.VTADAAA_UpdatedDate = DateTime.Now;
                    resultone.VTADAAA_ClientMultiple = data.VTADAAA_ClientMultiple;
                    _context.Update(resultone);




                    List<string> typedetails = new List<string>();
                    for (int c = 0; c < data.AllowenceArray.Length; c++)
                    {
                        typedetails.Add(data.AllowenceArray[c].Type);
                    }
                    Array remove_unchecked_details = _context.TADA_AdvanceApplyDetailsDMO.Where(a => a.VTADAAA_Id == data.VTADAAA_Id && !typedetails.Contains(a.VTADAAAD_ExpenditureHead)).ToArray();
                    foreach (var d in remove_unchecked_details)
                    {
                        _context.Remove(d);
                    }
                    if (data.AllowenceArray != null && data.AllowenceArray.Length > 0)
                    {
                        foreach (var c in data.AllowenceArray)
                        {
                            if (c.VTADAAAD_Id > 0)
                            {
                                var result_update = _context.TADA_AdvanceApplyDetailsDMO.Single(a => a.VTADAAAD_Id == c.VTADAAAD_Id);

                                result_update.VTADAAAD_Amount = c.Amount;
                                result_update.VTADAAAD_Remarks = c.Remarks;
                                result_update.VTADAAAD_UpdatedBy = data.UserId;
                                result_update.VTADAAAD_UpdatedDate = DateTime.Now;

                                _context.Update(result_update);
                            }
                            else
                            {
                                TADA_AddvanceApplyDetailsDMO dobj = new TADA_AddvanceApplyDetailsDMO();
                                dobj.VTADAAA_Id = data.VTADAAA_Id;
                                dobj.VTADAAAD_ExpenditureHead = c.Type;
                                dobj.VTADAAAD_Amount = c.Amount;
                                dobj.VTADAAAD_Remarks = c.Remarks;
                                dobj.VTADAAAD_Slots = c.VTADAAAD_Slots;
                                dobj.VTADAAAD_TotalSlots = c.VTADAAAD_TotalSlots;
                                dobj.VTADAAAD_ActiveFlg = true;
                                dobj.VTADAAAD_CreatedBy = data.UserId;
                                dobj.VTADAAAD_CreatedDate = DateTime.Now;
                                dobj.VTADAAAD_UpdatedBy = data.UserId;
                                dobj.VTADAAAD_UpdatedDate = DateTime.Now;
                                dobj.VTADAAAD_StatusFlg = "Pending";
                                _context.Add(dobj);
                            }
                        }

                    }
                    for (int x = 0; x < data.multiple_client.Length; x++)
                    {
                        var checkexist = _context.VMS_TADDA_MultipleClient_FilesDMO.Where(d => data.VTADAAA_Id == d.VTADAAA_Id).ToList();
                        if (checkexist.Count > 0)
                        {
                            foreach (var y in checkexist)
                            {
                                _context.Remove(y);
                            }
                        }

                        VMS_TADDA_MultipleClient_FilesDMO vtdmf = new VMS_TADDA_MultipleClient_FilesDMO();
                        vtdmf.VTADAAA_Id = data.VTADAAA_Id;
                        vtdmf.ISMMCLT_Id = data.multiple_client[x].ISMMCLT_Id;
                        vtdmf.VTADDAMCF_Remarks = data.multiple_client[x].VTADDAMCF_Remarks;
                        vtdmf.VTADDAMCF_StatusFlg = "P";
                        vtdmf.VTADDAMCF_ActiveFlag = true;
                        vtdmf.VTADDAMCF_CreatedDate = DateTime.Now;
                        vtdmf.VTADDAMCF_UpdatedDate = DateTime.Now;
                        vtdmf.VTADDAMCF_CreatedBy = data.UserId;
                        vtdmf.VTADDAMCF_UpdatedBy = data.UserId;
                        _context.Add(vtdmf);
                    }
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnvalue = true;
                        data.returnval = "Update";
                        string mailcc = "";
                        var checkmaxorder = (from P in _context.HR_PROCESSDMO
                                             from R in _context.HR_Process_Auth_OrderNoDMO
                                             where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "TAAD"
                                             select new TADA_Advance_ApplyDTO
                                             {
                                                 HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                                 IVRMUL_Id = R.IVRMUL_Id
                                             }
                                             ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                        if (checkmaxorder.Count > 0)
                        {
                            data.UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;
                            long Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                            data.cc_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        }
                        string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        if (data.cc_Mail != null)
                        {
                            mailcc = data.cc_Mail;
                        }

                        sendmail(data.MI_Id, emailid, "TADAAdavance_Apply", data.VTADAAA_Id, mailcc);
                    }
                    else
                    {
                        data.returnvalue = true;
                        data.returnval = "UpdateFailed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string sendmail(long MI_Id, string Email, string Template, long UserID, string mailcc)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        // mailcc = "";
                        string mailbcc = "";
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
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
                    }
                    else
                    {


                        // mailcc = "";
                        string mailbcc = "";
                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            // Value = Mailcontent
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
        public TADA_Advance_ApplyDTO deactive(TADA_Advance_ApplyDTO data)
        {
            try
            {
                var list = _context.VMS_TADA_Advance_Apply_AuthorisationDMO.Where(p => p.VTADAAA_Id == data.VTADAAA_Id && p.VTADAAAA_ActiveFlg == true).ToList();
                if (data.VTADAAA_Id > 0 && list.Count == 0)
                {
                    var resultone = _context.TADA_AdavanceApplyDMO.Single(t => t.VTADAAA_Id == data.VTADAAA_Id && t.MI_Id == data.MI_Id);
                    if (resultone.VTADAAA_ActiveFlg == true)
                    {
                        resultone.VTADAAA_ActiveFlg = false;
                        resultone.VTADAAA_UpdatedDate = DateTime.Now;
                        resultone.VTADAAA_UpdatedBy = data.UserId;
                        _context.Update(resultone);
                    }
                    else
                    {
                        resultone.VTADAAA_ActiveFlg = true;
                        resultone.VTADAAA_UpdatedDate = DateTime.Now;
                        resultone.VTADAAA_UpdatedBy = data.UserId;
                        _context.Update(resultone);
                    }
                    var resuttwo = _context.TADA_AdvanceApplyDetailsDMO.Where(p => p.VTADAAA_Id == data.VTADAAA_Id).Distinct().ToArray();
                    // var updatetwo = _context.TADA_AdvanceApplyDetailsDMO.Where(p => p.VTADAAA_Id == data.VTADAAA_Id).ToList();

                    if (resuttwo != null)
                    {
                        for (int j = 0; j < resuttwo.Length; j++)
                        {
                            resuttwo[j].VTADAAAD_UpdatedDate = DateTime.Now;
                            resuttwo[j].VTADAAAD_UpdatedBy = data.UserId;
                            if (resuttwo[j].VTADAAAD_ActiveFlg == true)
                            {
                                resuttwo[j].VTADAAAD_ActiveFlg = false;
                            }
                            else
                            {
                                resuttwo[j].VTADAAAD_ActiveFlg = true;
                            }
                            _context.Update(resuttwo[j]);
                        }
                        //Array remove_details = _context.TADA_AdvanceApplyDetailsDMO.Where(a => a.VTADAAA_Id == data.VTADAAA_Id).ToArray();
                        //foreach (var d in remove_details)
                        //{
                        //    _context.Remove(d);
                        //}
                    }
                    int returnval = _context.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnvalue = true;
                        data.returnval = "Delete";
                    }
                    else
                    {
                        data.returnval = "NotDelete";
                        data.returnvalue = true;
                    }

                }
                else
                {
                    data.returnvalue = true;
                    data.returnval = "Firststage";
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
