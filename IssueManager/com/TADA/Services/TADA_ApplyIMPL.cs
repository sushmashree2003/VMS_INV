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
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Services
{
    public class TADA_ApplyIMPL : Interface.TADA_ApplyInterface
    {

        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;
        public TADA_ApplyIMPL(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }
        public TADA_ApplyDTO getalldata(TADA_ApplyDTO data)
        {
            try
            {

                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.HRME_Id > 0)
                {
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
                            Value = "TADA"
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
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_EmployeeTADA_Advance_Apply_Details";
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
                                }
                            }

                            data.GetArray = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //if ((data.Allowense !=null && data.Allowense.Length > 0) || (data.GetArray !=null && data.GetArray.Length > 0))
                    //{

                    //}
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
                            Value = "TADA"
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
                    data.state = _context.IVRM_Master_StateDMO.Where(R=>R.IVRMMC_Id==101).Distinct().ToArray();
                    //adddd

                    var Apply_Detail = _context.TADA_ApplyDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.VTADAA_ActiveFlg == true).ToList().Select(p => p.VTADAAA_Id);

                    data.masterCountry = _context.Country.Distinct().ToArray();
                    data.Travel_data = _context.VMS_TADA_Master_TravelDMO.Where(a => a.VTADAMT_ActiveFlag == true).ToArray();

                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //getstate
        public TADA_ApplyDTO getstate(TADA_ApplyDTO dto)
        {
            try
            {
                //adddd
                var hrMdesId = (from d in _context.Staff_User_Login
                                from e in _context.MasterEmployee
                                where (d.Id == dto.UserId && e.HRME_Id == d.Emp_Code)
                                select new TADA_ApplyDTO
                                {
                                    HRMDES_Id = Convert.ToInt64(e.HRMDES_Id)
                                }

                                 ).ToList();
                if (hrMdesId.Count == 1)
                {

                    dto.City_Type = (from a in _context.TADA_CityMappingDMO
                                     from c in _context.IVRM_Master_CityDMO
                                     from d in _context.IVRM_Master_StateDMO
                                     where (a.IVRMMCT_Id == c.IVRMMCT_Id && a.VTADACM_ActiveFlag == true && a.HRMDES_Id == hrMdesId[0].HRMDES_Id && d.IVRMMS_Id == c.IVRMMS_Id && c.IVRMMS_Id == dto.IVRMMS_Id)
                                     select new TADA_Advance_ApplyDTO
                                     {
                                         IVRMMCT_Id = a.IVRMMCT_Id,
                                         IVRMMCT_Name = c.IVRMMCT_Name,


                                     }

               ).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        // Viewdetails
        public TADA_ApplyDTO Viewdetails(TADA_ApplyDTO dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TADA_ADavanceMltiple_SDetailsAmt_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                      SqlDbType.VarChar)
                    {
                        Value = dto.VTADAAA_Id
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

                        dto.Viewdetails = retObject.ToArray();


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
        // Getdata
        public TADA_ApplyDTO Getdata(TADA_ApplyDTO data)
        {
            try
            {
                data.EditArryThree = _context.VMS_TADAApply_FilesDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id && t.VTADAAF_ActiveFlag == true && t.VTADAAF_StatusFlg == "Pending").Distinct().ToArray();
                data.EditArrayTwo = _context.TADA_ApplyDetailsDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id && t.VTADAAD_ActiveFlg == true).Distinct().ToArray();
                //Getdata


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_ApplyDTO TadaAllwence(TADA_ApplyDTO data)
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
                if (data.VTADAA_FromDate != null && data.VTADAA_ToDate != null)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                    startdate = data.VTADAA_FromDate.Value.ToString("yyyy-MM-dd");
                    enddate = data.VTADAA_ToDate.Value.ToString("yyyy-MM-dd");

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
                            Value = "TADA"
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
                            data.GetArray = retObject.ToArray();
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
        public TADA_ApplyDTO savedta(TADA_ApplyDTO data)
        {
            try
            {
                string FromDate = data.VTADAA_FromDate.Value.ToString("yyyy-MM-dd");
                string ToDate = data.VTADAA_ToDate.Value.ToString("yyyy-MM-dd");
                data.returnvalue = false;
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                data.duplicatetwo = "0";
                var Getdesignation = (from P in _context.Staff_User_Login
                                      from R in _context.MasterEmployee
                                      where (P.Emp_Code == R.HRME_Id && P.Id == data.UserId)
                                      select new TADA_Advance_TourDTO
                                      {
                                          HRMDES_Id = R.HRMDES_Id

                                      }
                                ).ToList();
                if (data.VTADAAA_Id == 0 && data.VTADAA_Id == 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        data.duplicatetwo = "";
                        cmd.CommandText = "TADA_Advance_Apply_Record_Checking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });

                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
                        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        data.duplicatetwo = cmd.Parameters["@Result"].Value.ToString();
                    }
                }
                if (data.duplicatetwo == "0")
                {
                    if (data.VTADAA_Id == 0 && data.HRME_Id > 0)
                    {
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "VMS_TADA_Apply_Record_Checking";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id }); cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
                            cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
                            cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var data1 = cmd.ExecuteNonQuery();
                            data.duplicate = cmd.Parameters["@Result"].Value.ToString();
                        }

                        if (data.duplicate == "0")
                        {
                            TADA_ApplyDMO obj = new TADA_ApplyDMO();
                            obj.MI_Id = data.MI_Id;
                            if (data.VTADAAA_Id > 0)
                            {
                                obj.VTADAAA_Id = data.VTADAAA_Id;
                            }
                            obj.HRME_Id = data.HRME_Id;
                            obj.VTADAA_AppliedDate = DateTime.Now;
                            obj.VTADAA_FromDate = data.VTADAA_FromDate;
                            obj.VTADAA_ToDate = data.VTADAA_ToDate;
                            obj.VTADAA_ClientId = data.VTADAA_ClientId;
                            obj.VTADAA_ToAddress = data.VTADAA_ToAddress;
                            obj.VTADAA_Remarks = data.VTADAA_Remarks;
                            obj.VTADAA_TotalAppliedAmount = data.VTADAA_TotalAppliedAmount;
                            obj.IVRMMCT_Id = data.IVRMMCT_Id;
                            obj.VTADAA_StatusFlg = "Pending";
                            obj.VTADAA_ActiveFlg = true;
                            obj.VTADAA_ArrivalTime = data.VTADAA_ArrivalTime;
                            obj.VTADAA_DepartureTime = data.VTADAA_DepartureTime;
                            obj.VTADAAA_ClientMultiple = data.VTADAAA_ClientMultiple;
                            obj.VTADAA_CreatedBy = data.UserId;
                            obj.VTADAA_UpdatedBy = data.UserId;
                            obj.VTADAA_CreatedDate = DateTime.Now;
                            obj.VTADAA_UpdatedDate = DateTime.Now;
                            obj.VTADAA_BalanceHRMS = data.VTADAA_BalanceHRMS;
                            obj.VTADAA_Finaldocument = data.VTADAA_Finaldocument;
                            obj.VTADAA_ApprovedFlg = true;

                            _context.Add(obj);
                            if (data.AllowenceArray != null && data.AllowenceArray.Length > 0)
                            {
                                for (int c = 0; c < data.AllowenceArray.Length; c++)
                                {
                                    TADA_ApplyDetailsDMO dobj = new TADA_ApplyDetailsDMO();
                                    dobj.VTADAA_Id = obj.VTADAA_Id;
                                    dobj.VTADAAD_ExpenditureHead = data.AllowenceArray[c].Type;
                                    dobj.VTADAAD_Amount = data.AllowenceArray[c].Amount;
                                    dobj.VTADAAD_Remarks = data.AllowenceArray[c].Remarks;
                                    dobj.VTADAAD_Slots = data.AllowenceArray[c].VTADAAD_Slots;
                                    dobj.VTADAAD_Totalslots = data.AllowenceArray[c].VTADAAD_Totalslots;
                                    dobj.VTADAAD_ActiveFlg = true;
                                    dobj.VTADAAD_StatusFlg = "Pending";
                                    dobj.VTADAAD_CreatedBy = data.UserId;
                                    dobj.VTADAAD_CreatedDate = DateTime.Now;
                                    dobj.VTADAAD_UpdatedBy = data.UserId;
                                    dobj.VTADAAD_UpdatedDate = DateTime.Now;
                                    _context.Add(dobj);
                                }
                            }

                            for (int b = 0; b < data.filelist.Length; b++)
                            {
                                VMS_TADAApply_FilesDMO dobjj = new VMS_TADAApply_FilesDMO();
                                dobjj.VTADAA_Id = obj.VTADAA_Id;
                                dobjj.VTADAAF_FilePath = data.filelist[b].VTADAAF_FilePath;
                                dobjj.VTADAAF_FileName = data.filelist[b].VTADAAF_FileName;
                                dobjj.VTADAAF_Remarks = data.filelist[b].VTADAAF_Remarks;
                                dobjj.VTADAAF_Filedesc = data.filelist[b].VTADAAF_Filedesc;
                                dobjj.VTADAAF_StatusFlg = "Pending";
                                dobjj.VTADAAF_ActiveFlag = true;
                                dobjj.VTADAAF_ApprovedFlg = true;
                                _context.Add(dobjj);
                            }
                            for (int x = 0; x < data.multiple_client.Length; x++)
                            {

                                VMS_TADA_MultipleClient_FilesDMO vtmf = new VMS_TADA_MultipleClient_FilesDMO();
                                vtmf.VTADAA_Id = obj.VTADAA_Id;
                                vtmf.ISMMCLT_Id = data.multiple_client[x].ISMMCLT_Id;
                                vtmf.VTADAMCF_Remarks = data.multiple_client[x].VTADAMCF_Remarks;
                                vtmf.VTADAMCF_StatusFlg = "P";
                                vtmf.VTADAMCF_ActiveFlag = true;
                                vtmf.VTADAMCF_CreatedDate = DateTime.Now;
                                vtmf.VTADAMCF_UpdatedDate = DateTime.Now;
                                vtmf.VTADAMCF_CreatedBy = data.UserId;
                                vtmf.VTADAMCF_UpdatedBy = data.UserId;
                                _context.Add(vtmf);
                            }

                            if (data.VMSTADAAmountDTO != null && data.VMSTADAAmountDTO.Length > 0)
                            {
                                
                                for (int x = 0; x < data.VMSTADAAmountDTO.Length; x++)
                                {
                                    VMS_TADA_Apply_CityDMO VTAA = new VMS_TADA_Apply_CityDMO();
                                    VTAA.VTADAA_Id = obj.VTADAA_Id;
                                    VTAA.VTADAAC_FromState = data.VMSTADAAmountDTO[x].VTADAAAC_FromState;
                                    VTAA.VTADAAC_ToState = data.VMSTADAAmountDTO[x].VTADAAAC_ToState;
                                    VTAA.HRMDES_Id = Getdesignation.FirstOrDefault().HRMDES_Id.Value;
                                    VTAA.VTADAAC_Amount = data.VMSTADAAmountDTO[x].VTADAAAC_Amount;
                                    VTAA.VTADAMT_Id = data.VMSTADAAmountDTO[x].VTADAMT_Id;
                                    VTAA.VTADAMD_Id = data.VMSTADAAmountDTO[x].VTADAMD_Id;
                                    VTAA.VTADAAC_CreatedDate = DateTime.Now;
                                    VTAA.VTADAAC_UpdatedDate = DateTime.Now;
                                    VTAA.VTADAAC_CreatedBy = data.UserId;
                                    VTAA.VTADAAC_UpdatedBy = data.UserId;
                                    VTAA.IVRMMCT_Id = data.VMSTADAAmountDTO[x].IVRMMCT_Id;
                                    _context.Add(VTAA);
                                }
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
                                    data.CC_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                }
                                string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == obj.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                if (data.CC_Mail != null)
                                {
                                    mailcc = data.CC_Mail;
                                }

                                sendmail(data.MI_Id, emailid, "TADA_Apply", obj.VTADAA_Id, mailcc);

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
                    if (data.VTADAA_Id > 0 && data.HRME_Id > 0)
                    {
                        var resultone = _context.TADA_ApplyDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id && t.MI_Id == data.MI_Id).FirstOrDefault();
                        resultone.MI_Id = data.MI_Id;
                        resultone.HRME_Id = data.HRME_Id;
                        resultone.VTADAA_AppliedDate = DateTime.Now;
                        resultone.VTADAA_FromDate = data.VTADAA_FromDate;
                        resultone.VTADAA_ToDate = data.VTADAA_ToDate;
                        resultone.VTADAA_ClientId = data.VTADAA_ClientId;
                        resultone.VTADAA_ToAddress = data.VTADAA_ToAddress;
                        resultone.VTADAA_Remarks = data.VTADAA_Remarks;
                        resultone.VTADAA_TotalAppliedAmount = data.VTADAA_TotalAppliedAmount;
                        resultone.IVRMMCT_Id = data.IVRMMCT_Id;
                        resultone.VTADAA_UpdatedBy = data.UserId;
                        resultone.VTADAAA_ClientMultiple = data.VTADAAA_ClientMultiple;
                        resultone.VTADAA_UpdatedDate = DateTime.Now;
                        resultone.VTADAA_BalanceHRMS = data.VTADAA_BalanceHRMS;
                        resultone.VTADAA_Finaldocument = data.VTADAA_Finaldocument;
                        resultone.VTADAA_ApprovedFlg = true;
                        _context.Update(resultone);

                        List<string> typedetails = new List<string>();
                        for (int c = 0; c < data.AllowenceArray.Length; c++)
                        {
                            typedetails.Add(data.AllowenceArray[c].Type);
                        }

                        Array remove_unchecked_details = _context.TADA_ApplyDetailsDMO.Where(a => a.VTADAA_Id == data.VTADAA_Id && !typedetails.Contains(a.VTADAAD_ExpenditureHead)).ToArray();
                        foreach (var d in remove_unchecked_details)
                        {
                            _context.Remove(d);
                        }
                        if (data.AllowenceArray != null && data.AllowenceArray.Length > 0)
                        {
                            foreach (var c in data.AllowenceArray)
                            {
                                if (c.VTADAAD_Id > 0)
                                {
                                    var result_update = _context.TADA_ApplyDetailsDMO.Single(a => a.VTADAAD_Id == c.VTADAAD_Id);

                                    result_update.VTADAAD_Amount = c.Amount;
                                    result_update.VTADAAD_Remarks = c.Remarks;
                                    result_update.VTADAAD_UpdatedBy = data.UserId;
                                    result_update.VTADAAD_UpdatedDate = DateTime.Now;
                                    _context.Update(result_update);
                                }
                                else
                                {
                                    TADA_ApplyDetailsDMO dobj = new TADA_ApplyDetailsDMO();
                                    dobj.VTADAA_Id = data.VTADAA_Id;
                                    dobj.VTADAAD_ExpenditureHead = c.Type;
                                    dobj.VTADAAD_Amount = c.Amount;
                                    dobj.VTADAAD_StatusFlg = "Pending";
                                    dobj.VTADAAD_Remarks = c.Remarks;
                                    dobj.VTADAAD_ActiveFlg = true;
                                    dobj.VTADAAD_CreatedBy = data.UserId;
                                    dobj.VTADAAD_CreatedDate = DateTime.Now;
                                    dobj.VTADAAD_UpdatedBy = data.UserId;
                                    dobj.VTADAAD_UpdatedDate = DateTime.Now;
                                    _context.Add(dobj);
                                }
                            }
                        }

                        for (int x = 0; x < data.multiple_client.Length; x++)
                        {
                            var checkexist = _context.VMS_TADA_MultipleClient_FilesDMO.Where(d => data.VTADAA_Id == d.VTADAA_Id).ToList();
                            if (checkexist.Count > 0)
                            {
                                foreach (var y in checkexist)
                                {
                                    _context.Remove(y);
                                }
                            }
                            VMS_TADA_MultipleClient_FilesDMO vtmf = new VMS_TADA_MultipleClient_FilesDMO();
                            vtmf.VTADAA_Id = data.VTADAA_Id;
                            vtmf.ISMMCLT_Id = data.multiple_client[x].ISMMCLT_Id;
                            vtmf.VTADAMCF_Remarks = data.multiple_client[x].VTADAMCF_Remarks;
                            vtmf.VTADAMCF_StatusFlg = "P";
                            vtmf.VTADAMCF_ActiveFlag = true;
                            vtmf.VTADAMCF_CreatedDate = DateTime.Now;
                            vtmf.VTADAMCF_UpdatedDate = DateTime.Now;
                            vtmf.VTADAMCF_CreatedBy = data.UserId;
                            vtmf.VTADAMCF_UpdatedBy = data.UserId;
                            _context.Add(vtmf);
                        }

                        if (data.filelist != null && data.filelist.Length > 0)
                        {
                            var DetailsFiles = _context.VMS_TADAApply_FilesDMO.Where(a => a.VTADAA_Id == data.VTADAA_Id && a.VTADAAF_StatusFlg == "Pending").ToList();

                            if (DetailsFiles.Count > 0)
                            {
                                foreach (var d in DetailsFiles)
                                {
                                    _context.Remove(d);
                                }
                            }
                            for (int f = 0; f < data.filelist.Length; f++)
                            {
                                VMS_TADAApply_FilesDMO dobjj = new VMS_TADAApply_FilesDMO();
                                dobjj.VTADAA_Id = data.VTADAA_Id;
                                dobjj.VTADAAF_FilePath = data.filelist[f].VTADAAF_FilePath;
                                dobjj.VTADAAF_FileName = data.filelist[f].VTADAAF_FileName;
                                dobjj.VTADAAF_Remarks = data.filelist[f].VTADAAF_Remarks;
                                dobjj.VTADAAF_Filedesc = data.filelist[f].VTADAAF_Filedesc;
                                dobjj.VTADAAF_StatusFlg = "Pending";
                                dobjj.VTADAAF_ActiveFlag = true;
                                dobjj.VTADAAF_ApprovedFlg = true;

                                _context.Add(dobjj);
                            }
                        }
                        if (data.VMSTADAAmountDTO != null && data.VMSTADAAmountDTO.Length > 0)
                        {
                            var check = _context.VMS_TADA_Apply_CityDMO.Where(d => d.VTADAA_Id == data.VTADAA_Id).ToList();
                            if (check.Count > 0)
                            {
                                foreach (var y in check)
                                {
                                    _context.Remove(y);
                                }
                            }
                            for (int x = 0; x < data.VMSTADAAmountDTO.Length; x++)
                            {
                                VMS_TADA_Apply_CityDMO VTAA = new VMS_TADA_Apply_CityDMO();
                                VTAA.VTADAA_Id = data.VTADAA_Id;
                                VTAA.VTADAAC_FromState = data.VMSTADAAmountDTO[x].VTADAAAC_FromState;
                                VTAA.VTADAAC_ToState = data.VMSTADAAmountDTO[x].VTADAAAC_ToState;
                                VTAA.HRMDES_Id = Getdesignation.FirstOrDefault().HRMDES_Id.Value;
                                VTAA.VTADAAC_Amount = data.VMSTADAAmountDTO[x].VTADAAAC_Amount;
                                VTAA.VTADAMT_Id = data.VMSTADAAmountDTO[x].VTADAMT_Id;
                                VTAA.VTADAMD_Id = data.VMSTADAAmountDTO[x].VTADAMD_Id;
                                VTAA.VTADAAC_CreatedDate = DateTime.Now;
                                VTAA.VTADAAC_UpdatedDate = DateTime.Now;
                                VTAA.VTADAAC_CreatedBy = data.UserId;
                                VTAA.VTADAAC_UpdatedBy = data.UserId;
                                VTAA.IVRMMCT_Id = data.VMSTADAAmountDTO[x].IVRMMCT_Id;
                                _context.Add(VTAA);
                            }
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

                                data.CC_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            }
                            string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            if (data.CC_Mail != null)
                            {
                                mailcc = data.CC_Mail;
                            }
                          

                            sendmail(data.MI_Id, emailid, "TADA_Apply", data.VTADAA_Id, mailcc);

                        }
                        else
                        {
                            data.returnvalue = true;
                            data.returnval = "UpdateFailed";
                        }
                    }
                }
                else
                {
                    data.returnvalue = true;
                    data.returnval = "DuplicateAdavance";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnvalue = false;
            }

            return data;
        }
        //edit
        public TADA_ApplyDTO edit(TADA_ApplyDTO data)
        {
            try
            {
                var list = _context.VMS_TADA_Apply_AuthorisationDMO.Where(p => p.VTADAA_Id == data.VTADAA_Id).ToList();
                if (data.VTADAA_Id > 0 && list.Count == 0)
                {
                    //add
                    data.EditArray = (from a in _context.TADA_ApplyDMO
                                      from b in _context.IVRM_Master_CityDMO
                                      from c in _context.IVRM_Master_StateDMO
                                      where (a.IVRMMCT_Id == b.IVRMMCT_Id && a.MI_Id == data.MI_Id && a.VTADAA_ActiveFlg == true && a.VTADAA_StatusFlg == "Pending" && a.VTADAA_Id == data.VTADAA_Id && b.IVRMMS_Id == c.IVRMMS_Id)
                                      select new TADA_ApplyDTO
                                      {
                                          VTADAA_ClientId = a.VTADAA_ClientId,
                                          VTADAA_FromDate = a.VTADAA_FromDate,
                                          VTADAA_ToDate = a.VTADAA_ToDate,
                                          IVRMMCT_Id = a.IVRMMCT_Id,
                                          IVRMMCT_Name = b.IVRMMCT_Name,
                                          VTADAA_TotalAppliedAmount = a.VTADAA_TotalAppliedAmount,
                                          VTADAA_ToAddress = a.VTADAA_ToAddress,
                                          VTADAA_Remarks = a.VTADAA_Remarks,
                                          VTADAA_Id = a.VTADAA_Id,
                                          IVRMMS_Id = c.IVRMMS_Id,
                                          IVRMMS_Name = c.IVRMMS_Name,
                                          VTADAA_DepartureTime = a.VTADAA_DepartureTime,
                                          VTADAA_ArrivalTime = a.VTADAA_ArrivalTime,
                                          VTADAAA_ClientMultiple = a.VTADAAA_ClientMultiple,
                                          VTADAA_BalanceHRMS = a.VTADAA_BalanceHRMS,
                                          VTADAAA_Id = a.VTADAAA_Id,
                                          VTADAA_Finaldocument = a.VTADAA_Finaldocument
                                      }
                                   ).Distinct().ToArray();

                    data.EditArrayTwo = _context.TADA_ApplyDetailsDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id && t.VTADAAD_ActiveFlg == true).Distinct().ToArray();

                    data.EditArryThree = _context.VMS_TADAApply_FilesDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id && t.VTADAAF_ActiveFlag == true && t.VTADAAF_StatusFlg == "Pending").Distinct().ToArray();
                    data.tada_multipleclient = _context.VMS_TADA_MultipleClient_FilesDMO.Where(x => x.VTADAA_Id == data.VTADAA_Id).Distinct().ToArray();
                    data.Id = _context.TADA_ApplyDMO.Where(R => R.VTADAA_Id == data.VTADAA_Id && R.MI_Id == data.MI_Id && R.VTADAA_ActiveFlg == true && R.VTADAA_StatusFlg == "Pending").Select(M => M.VTADAAA_Id).FirstOrDefault();

                    if (data.Id > 0)
                    {
                        data.EditFour = _context.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_Id == data.Id && R.VTADAAA_ActiveFlg == true).Distinct().ToArray();
                    }
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
                        data.EditFive = (from a in _context.TADA_ApplyDMO
                                         from t in _context.TADA_CityMappingDMO
                                         where (a.MI_Id == data.MI_Id && t.VTADACM_ActiveFlag == true
                                         && t.IVRMMCT_Id == a.IVRMMCT_Id && a.VTADAA_Id == data.VTADAA_Id && t.HRMDES_Id == hrMdesId[0].HRMDES_Id)
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
                data.EditArryThree = _context.VMS_TADAApply_FilesDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id).Distinct().ToArray();
                data.UplodaArray = _context.VMS_TADAApply_FilesDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id && t.VTADAAF_ApprovedFlg == false).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_ApplyDTO deactive(TADA_ApplyDTO data)
        {

            try
            {
                var list = _context.VMS_TADA_Apply_AuthorisationDMO.Where(p => p.VTADAA_Id == data.VTADAA_Id && p.VTADAAA_ActiveFlg == true).ToList();
                if (data.VTADAA_Id > 0 && list.Count == 0)
                {
                    var resultone = _context.TADA_ApplyDMO.Single(t => t.VTADAA_Id == data.VTADAA_Id && t.MI_Id == data.MI_Id);
                    if (resultone.VTADAA_ActiveFlg == true)
                    {
                        resultone.VTADAA_ActiveFlg = false;
                        resultone.VTADAA_UpdatedDate = DateTime.Now;
                        resultone.VTADAA_UpdatedBy = data.UserId;
                        _context.Update(resultone);
                    }
                    else
                    {
                        resultone.VTADAA_ActiveFlg = true;
                        resultone.VTADAA_UpdatedDate = DateTime.Now;
                        resultone.VTADAA_UpdatedBy = data.UserId;
                        _context.Update(resultone);
                    }
                    var resuttwo = _context.TADA_ApplyDetailsDMO.Where(p => p.VTADAA_Id == data.VTADAA_Id).Distinct().ToArray();
                    if (resuttwo != null)
                    {
                        for (int j = 0; j < resuttwo.Length; j++)
                        {
                            if (resuttwo[j].VTADAAD_ActiveFlg == true)
                            {
                                resuttwo[j].VTADAAD_ActiveFlg = false;
                            }
                            else
                            {
                                resuttwo[j].VTADAAD_ActiveFlg = true;
                            }
                            resuttwo[j].VTADAAD_UpdatedBy = data.UserId;
                            resuttwo[j].VTADAAD_UpdatedDate = DateTime.Now;
                            _context.Update(resuttwo[j]);
                        }
                    }
                    var files = _context.VMS_TADAApply_FilesDMO.Where(R => R.VTADAA_Id == data.VTADAA_Id).Distinct().ToArray();
                    if (files != null)
                    {
                        for (int k = 0; k < files.Length; k++)
                        {
                            if (files[k].VTADAAF_ActiveFlag == true)
                            {
                                files[k].VTADAAF_ActiveFlag = false;
                            }
                            else
                            {
                                files[k].VTADAAF_ActiveFlag = true;
                            }
                            _context.Update(files[k]);
                        }
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
        //Updatefile
        public TADA_ApplyDTO Updatefile(TADA_ApplyDTO data)
        {

            try
            {
                if (data.filelist != null && data.filelist.Length > 0)
                {
                    var resultone = _context.TADA_ApplyDMO.Single(t => t.VTADAA_Id == data.VTADAA_Id);
                    resultone.VTADAA_UpdatedBy = data.UserId;
                    resultone.VTADAA_UpdatedDate = DateTime.Now;
                    resultone.VTADAA_Finaldocument = data.VTADAA_Finaldocument;
                    _context.Update(resultone);

                    var DetailsFiles = _context.VMS_TADAApply_FilesDMO.Where(a => a.VTADAA_Id == data.VTADAA_Id && a.VTADAAF_ApprovedFlg == false).ToList();

                    if (DetailsFiles.Count > 0)
                    {
                        foreach (var d in DetailsFiles)
                        {
                            _context.Remove(d);
                        }
                    }
                    for (int f = 0; f < data.filelist.Length; f++)
                    {
                        VMS_TADAApply_FilesDMO dobjj = new VMS_TADAApply_FilesDMO();
                        dobjj.VTADAA_Id = data.VTADAA_Id;
                        dobjj.VTADAAF_FilePath = data.filelist[f].VTADAAF_FilePath;
                        dobjj.VTADAAF_FileName = data.filelist[f].VTADAAF_FileName;
                        dobjj.VTADAAF_Remarks = data.filelist[f].VTADAAF_Remarks;
                        dobjj.VTADAAF_Filedesc = data.filelist[f].VTADAAF_Filedesc;
                        dobjj.VTADAAF_VerfieRemarks = data.filelist[f].VTADAAF_VerfieRemarks;
                        dobjj.VTADAAF_StatusFlg = "Pending";
                        dobjj.VTADAAF_ActiveFlag = true;
                        dobjj.VTADAAF_ApprovedFlg = false;
                        _context.Add(dobjj);
                    }
                    int M = _context.SaveChanges();
                    if (M > 0)
                    {
                        data.returnval = "save";
                        data.HRME_Id = _context.TADA_ApplyDMO.Single(R => R.VTADAA_Id == data.VTADAA_Id).HRME_Id;
                        data.MI_Id = _context.TADA_ApplyDMO.Single(R => R.VTADAA_Id == data.VTADAA_Id).MI_Id;
                        Dictionary<string, string> val = new Dictionary<string, string>();//CleintInvoiceMail
                        var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "Tadadocument" && e.MI_Id == data.MI_Id && e.ISES_MailActiveFlag == true).ToList();

                        if (template.Count == 0)
                        {
                            return data;
                        }
                        var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                        List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                        alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                        if (alldetails.Count > 0)
                        {
                            string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                            string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                            string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                            Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                            string Subject = template[0].ISES_MailSubject.ToString();
                            string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                            var message = new SendGridMessage();
                            if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                            {
                                string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                                foreach (var c in ccmail)
                                {
                                    if (c != "")
                                    {
                                        message.AddBcc(c);

                                    }
                                }

                            }

                            if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                            {
                                string[] ccmail = template[0].ISES_MailCCId.Split(',');
                                foreach (var c in ccmail)
                                {
                                    if (c != "")
                                    {
                                        message.AddCc(c);

                                    }
                                }

                            }
                            //FHEAD

                            message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                            if (data.MailSubject != null && data.MailSubject != "")
                            {
                                message.Subject = data.MailSubject;
                            }
                            else
                            {
                                message.Subject = Subject;
                            }
                            // 

                            string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            // string emailid = "sanjeev@globalqtytrainig.com";
                            message.AddTo(emailid);
                            message.HtmlContent = data.Mailmsg;
                            var client = new SendGridClient(sengridkey);
                            client.SendEmailAsync(message).Wait();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "Tadadocument" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                                var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                                var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                cmd.CommandText = "IVRM_Email_Outgoing";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                                {
                                    Value = emailid
                                });
                                cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                                {
                                    Value = data.Mailmsg
                                });
                                cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                                {
                                    Value = modulename[0]
                                });
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


                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    else
                    {
                        data.returnval = "notsave";
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

    }
}
