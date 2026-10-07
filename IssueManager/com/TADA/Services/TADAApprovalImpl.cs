using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
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
    public class TADAApprovalImpl : Interface.TADAApprovalInterface
    {

        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;

        public TADAApprovalImpl(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }


        public TADAApprovalDTO getalldata(TADAApprovalDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_APPLY_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
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
                        data.getadvancetada = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "TADALead_All_EmployeeList";                    cmd.CommandType = CommandType.StoredProcedure;                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.GetReport = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        // Getdata //getfinaldata
        public TADAApprovalDTO getfinaldata(TADAApprovalDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_FINAL_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
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
                        data.getadvancetada = retObject.ToArray();
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
        public TADAApprovalDTO getfile(TADAApprovalDTO data)        {            try            {                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "usp_s_RoutePlanComments";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",                    SqlDbType.BigInt)                    {                        Value = data.HRME_Id                    });                    cmd.Parameters.Add(new SqlParameter("@CommentDate",                    SqlDbType.DateTime)                    {                        Value = data.VTADAA_FromDate                    });                    cmd.Parameters.Add(new SqlParameter("@ToDate",                    SqlDbType.DateTime)                    {                        Value = data.VTADAA_ToDate                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.GetReport = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return data;        }

        //getalldocument
        public TADAApprovalDTO getalldocument(TADAApprovalDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Document_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                   SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAA_Id",
                  SqlDbType.VarChar)
                    {
                        Value = 0
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
                        data.getadvancetada = retObject.ToArray();
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
        }       // Ge
        public TADAApprovalDTO savedta(TADAApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var indlist = _context.TADA_ApplyDMO.Where(w => w.VTADAA_Id == data.VTADAA_Id).ToList();

                if (indlist.Count > 0)
                {
                    VMS_TADA_Apply_AuthorisationDMO obj = new VMS_TADA_Apply_AuthorisationDMO();
                    obj.VTADAA_Id = data.VTADAA_Id;
                    obj.IVRMUL_Id = data.UserId;
                    obj.VTADAAA_SanctioningLevel = data.level;
                    obj.VTADAAA_Remarks = data.VTADAA_Remarks;
                    obj.VTADAAA_SactionedAmount = data.VTADAA_TotalSactionedAmount;
                    if (data.approvecnt > 0)
                    {
                        obj.VTADAAA_StatusFlg = "Approved";
                    }
                    else
                    {
                        obj.VTADAAA_StatusFlg = "Rejected";
                    }
                    obj.VTADAAA_Date = indianTime;
                    obj.VTADAAA_ActiveFlg = true;
                    obj.VTADAAA_CreatedBy = data.UserId;
                    obj.VTADAAA_UpdatedBy = data.UserId;
                    obj.VTADAAA_CreatedDate = indianTime;
                    obj.VTADAAA_UpdatedDate = indianTime;
                    _context.Add(obj);
                    if (data.headarray != null)
                    {
                        foreach (var item in data.headarray)
                        {
                            VMS_TADA_Apply_Authorisation_HeadsDMO tobj = new VMS_TADA_Apply_Authorisation_HeadsDMO();
                            tobj.VTADAAA_Id = obj.VTADAAA_Id;
                            tobj.VTADAAD_Id = item.VTADAAD_Id;
                            tobj.VTADAAAH_SactionedAmount = item.VTADAAAH_SactionedAmount;
                            tobj.VTADAAAH_Remarks = item.VTADAAAD_Remarks;
                            if (item.flag == "R")
                            {
                                tobj.VTADAAAH_StatusFlg = "Rejected";
                            }
                            else
                            {

                                tobj.VTADAAAH_StatusFlg = "Approved";
                            }
                            tobj.VTADAAAH_Date = indianTime;
                            tobj.VTADAAAH_ActiveFlg = true;
                            tobj.VTADAAAH_CreatedDate = indianTime;
                            tobj.VTADAAAH_UpdatedDate = indianTime;
                            tobj.VTADAAAH_UpdatedBy = data.UserId;
                            tobj.VTADAAAH_CreatedBy = data.UserId;
                            _context.Add(tobj);
                        }
                    }
                    int ext = _context.SaveChanges();
                    if (ext > 0)
                    {
                        var checkmaxorder = (from a in _context.HR_PROCESSDMO
                                             from b in _context.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAPL"
                                             select b.HRPAON_SanctionLevelNo).Max();
                        if (checkmaxorder == data.level)
                        {
                            var indentres = _context.TADA_ApplyDMO.Single(a => a.VTADAA_Id == data.VTADAA_Id);
                            indentres.VTADAA_UpdatedBy = data.UserId;
                            indentres.VTADAA_UpdatedDate = indianTime;
                            if (data.approvecnt > 0)
                            {

                                string trans_id = "";
                                indentres.VTADAA_StatusFlg = "Approved";
                                indentres.VTADAA_TotalSactionedAmount = data.VTADAA_TotalSactionedAmount;
                                var HRMBD_Idf = _context.HR_Master_BankDeatils.Where(a => a.MI_Id == indentres.MI_Id && a.HRMBD_AccType == "PV").ToList();
                                //HRMBD_Idf.FirstOrDefault().HRMBD_Id
                                if (HRMBD_Idf.Count > 0)
                                {
                                    var TransIdcount = _context.VMS_Payment_VoucherDMO.Distinct().ToList();
                                    if (TransIdcount.Count > 0)
                                    {
                                        trans_id = TransIdcount.Count.ToString();
                                    }
                                    else
                                    {
                                        trans_id = "0";
                                    }
                                    var EmplYoee = (from d in _context.MasterEmployee
                                                    where d.MI_Id == indentres.MI_Id && d.HRME_Id == indentres.HRME_Id
                                                    select new TADA_Advance_ApplyDTO
                                                    {
                                                        EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                                        HRME_Id = d.HRME_Id

                                                    }).OrderByDescending(d => d.HRME_Id).Take(1).ToArray();

                                    var VBABRS = (from a in _context.VMS_BANK_ACCOUNT_BALANCEDMO
                                                  where a.MI_Id == indentres.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == HRMBD_Idf.FirstOrDefault().HRMBD_Id
                                                  select new CheckBankBalanceDTO
                                                  {
                                                      VBABRS_BankBalalnce = a.VBABAL_ActualBankBalance,
                                                      VBABRS_Id = a.VBABAL_Id
                                                  }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
                                    // _context
                                    // VMS_Payment_VoucherDMO objlp = new VMS_Payment_VoucherDMO();
                                    //objlp.MI_Id = indentres.MI_Id;
                                    //objlp.INVMS_Id = null;
                                    //objlp.VPAYVOU_PaymentDate = DateTime.Now;
                                    //objlp.VPAYVOU_AppliedAmount = data.VTADAA_TotalSactionedAmount;
                                    //objlp.VPAYVOU_ApprovedAmount = 0;
                                    //objlp.VPAYVOU_Remarks = indentres.VTADAA_Remarks;
                                    //objlp.VPAYVOU_BankBalalnce = VBABRS.FirstOrDefault().VBABRS_BankBalalnce;
                                    //objlp.VPAYVOU_StatusFlg = "Pending";
                                    //objlp.VPAYVOU_ChequePrintedFlg = false;
                                    //objlp.VPAYVOU_ActiveFlg = true;
                                    //objlp.VPAYVOU_CreatedBy = data.UserId;
                                    //objlp.VPAYVOU_CreatedDate = indianTime;
                                    //objlp.VPAYVOU_UpdatedBy = data.UserId;
                                    //objlp.VPAYVOU_UpdatedDate = indianTime;
                                    //objlp.VPAYVOU_BankAccountNo = HRMBD_Idf.FirstOrDefault().HRMBD_BankAccountNo;
                                    //objlp.VPAYVOU_VoucherNo = trans_id;
                                    //objlp.HRMBD_Id = HRMBD_Idf.FirstOrDefault().HRMBD_Id;
                                    //objlp.VPAYVOU_SupplierFlg = false;
                                    //objlp.VPAYVOU_SelfPayFlg = false;
                                    //objlp.VPAYVOU_PaymentTo = EmplYoee.FirstOrDefault().EmplYoeeName;
                                    //objlp.VPAYVOU_PaymentMode = "Cheque";
                                    //objlp.HRMBD_InterbankTransFlg = false;
                                    //objlp.VPAYVOU_HRMBD_Id = null;
                                    //_context.Add(objlp);
                                    //var filelist = _context.VMS_TADAApply_FilesDMO.Where(R => R.VTADAA_Id == indentres.VTADAA_Id).ToList();
                                    //if (filelist != null && filelist.Count > 0)
                                    //{
                                    //    foreach (var fd in filelist)
                                    //    {
                                    //        BankAccount_BRS_FilesDMO objfile = new BankAccount_BRS_FilesDMO();
                                    //        objfile.VPAYVOUF_FileName = fd.VTADAAF_FileName;
                                    //        objfile.VPAYVOUF_FilePath = fd.VTADAAF_FilePath;
                                    //        objfile.VPAYVOUF_Filedesc = fd.VTADAAF_Filedesc;
                                    //        objfile.VPAYVOU_Id = objlp.VPAYVOU_Id;
                                    //        objfile.VPAYVOUF_StatusFlg = "approved";
                                    //        objfile.VPAYVOUF_ActiveFlg = true;
                                    //        _context.Add(objfile);
                                    //    }
                                    //}
                                }
                            }
                            else
                            {
                                indentres.VTADAA_StatusFlg = "Rejected";
                                indentres.VTADAA_TotalSactionedAmount = 0;
                            }
                            _context.Update(indentres);
                            var indentresitem = _context.TADA_ApplyDetailsDMO.Where(a => a.VTADAA_Id == data.VTADAA_Id).ToList();
                            if (indentresitem.Count > 0)
                            {

                                foreach (var item in indentresitem)
                                {

                                    foreach (var ap in data.headarray)
                                    {
                                        if (ap.VTADAAD_Id == item.VTADAAD_Id)
                                        {
                                            if (ap.flag == "R")
                                            {

                                                item.VTADAAD_StatusFlg = "Rejected";
                                            }
                                            else
                                            {

                                                item.VTADAAD_StatusFlg = "Approved";
                                            }

                                            item.VTADAAD_UpdatedBy = data.UserId;
                                            item.VTADAAD_UpdatedDate = indianTime;

                                            _context.Update(item);
                                        }

                                    }


                                }

                            }

                            _context.SaveChanges();

                        }

                        data.returnvalue = true;
                        var primaryid = _context.VMS_TADA_Apply_AuthorisationDMO.Where(e => e.VTADAA_Id == obj.VTADAA_Id).ToList();
                        if (primaryid.Count > 0)
                        {
                            long hrid = 0;

                            var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                            if (hrmeid1.Count > 0)
                            {
                                hrid = hrmeid1.FirstOrDefault().Emp_Code;
                            }

                            emaildata(primaryid.FirstOrDefault().VTADAA_Id, hrid, data.MI_Id, data.UserId, data.HRME_Id, data.approvecnt);
                        }
                    }
                    else
                    {
                        data.returnvalue = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //var duplicate = _context.TADA_CityMappingDMO.Where(p => p.MI_Id == data.MI_Id && p.HRMDES_Id == data.HRMDES_Id && p.IVRMMCT_Id == data.city_array[0].IVRMMCT_Id && p.VTADACM_Id != data.VTADACM_Id).ToList();

            return data;
        }
        //savefinaldta
        public TADAApprovalDTO savefinaldta(TADAApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var indlist = _context.TADA_ApplyDMO.Where(w => w.VTADAA_Id == data.VTADAA_Id).ToList();

                if (indlist.Count > 0)
                {
                    VMS_TADA_Apply_AuthorisationDMO obj = new VMS_TADA_Apply_AuthorisationDMO();
                    obj.VTADAA_Id = data.VTADAA_Id;
                    obj.IVRMUL_Id = data.UserId;
                    obj.VTADAAA_SanctioningLevel = data.level;
                    obj.VTADAAA_Remarks = data.VTADAA_Remarks;
                    obj.VTADAAA_SactionedAmount = data.VTADAA_TotalSactionedAmount;
                    if (data.approvecnt > 0)
                    {
                        obj.VTADAAA_StatusFlg = "Approved";
                    }
                    else
                    {
                        obj.VTADAAA_StatusFlg = "Rejected";
                    }
                    obj.VTADAAA_Date = indianTime;
                    obj.VTADAAA_ActiveFlg = true;
                    obj.VTADAAA_CreatedBy = data.UserId;
                    obj.VTADAAA_UpdatedBy = data.UserId;
                    obj.VTADAAA_CreatedDate = indianTime;
                    obj.VTADAAA_UpdatedDate = indianTime;
                    _context.Add(obj);
                    if (data.headarray != null)
                    {
                        foreach (var item in data.headarray)
                        {
                            VMS_TADA_Apply_Authorisation_HeadsDMO tobj = new VMS_TADA_Apply_Authorisation_HeadsDMO();
                            tobj.VTADAAA_Id = obj.VTADAAA_Id;
                            tobj.VTADAAD_Id = item.VTADAAD_Id;
                            tobj.VTADAAAH_SactionedAmount = item.VTADAAAH_SactionedAmount;
                            tobj.VTADAAAH_Remarks = item.VTADAAAD_Remarks;
                            if (item.flag == "R")
                            {
                                tobj.VTADAAAH_StatusFlg = "Rejected";
                            }
                            else
                            {

                                tobj.VTADAAAH_StatusFlg = "Approved";
                            }
                            tobj.VTADAAAH_Date = indianTime;
                            tobj.VTADAAAH_ActiveFlg = true;
                            tobj.VTADAAAH_CreatedDate = indianTime;
                            tobj.VTADAAAH_UpdatedDate = indianTime;
                            tobj.VTADAAAH_UpdatedBy = data.UserId;
                            tobj.VTADAAAH_CreatedBy = data.UserId;
                            _context.Add(tobj);
                        }
                    }
                    int ext = _context.SaveChanges();
                    if (ext > 0)
                    {
                        var checkmaxorder = (from a in _context.HR_PROCESSDMO
                                             from b in _context.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAFIAPL"
                                             select b.HRPAON_SanctionLevelNo).Max();
                        if (checkmaxorder == data.level)
                        {
                            var indentres = _context.TADA_ApplyDMO.Single(a => a.VTADAA_Id == data.VTADAA_Id);
                            indentres.VTADAA_UpdatedBy = data.UserId;
                            indentres.VTADAA_UpdatedDate = indianTime;
                            if (data.approvecnt > 0)
                            {

                                string trans_id = "";
                                indentres.VTADAA_StatusFlg = "Approved";
                                indentres.VTADAA_TotalSactionedAmount = data.VTADAA_TotalSactionedAmount;
                                var HRMBD_Idf = _context.HR_Master_BankDeatils.Where(a => a.MI_Id == indentres.MI_Id && a.HRMBD_AccType == "PV").ToList();
                                //HRMBD_Idf.FirstOrDefault().HRMBD_Id
                                if (HRMBD_Idf.Count > 0)
                                {
                                    var TransIdcount = _context.VMS_Payment_VoucherDMO.Distinct().ToList();
                                    if (TransIdcount.Count > 0)
                                    {
                                        trans_id = TransIdcount.Count.ToString();
                                    }
                                    else
                                    {
                                        trans_id = "0";
                                    }
                                    var EmplYoee = (from d in _context.MasterEmployee
                                                    where d.MI_Id == indentres.MI_Id && d.HRME_Id == indentres.HRME_Id
                                                    select new TADA_Advance_ApplyDTO
                                                    {
                                                        EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                                        HRME_Id = d.HRME_Id

                                                    }).OrderByDescending(d => d.HRME_Id).Take(1).ToArray();

                                    var VBABRS = (from a in _context.VMS_BANK_ACCOUNT_BALANCEDMO
                                                  where a.MI_Id == indentres.MI_Id && a.VBABAL_ActiveFlg == true && a.HRMBD_Id == HRMBD_Idf.FirstOrDefault().HRMBD_Id
                                                  select new CheckBankBalanceDTO
                                                  {
                                                      VBABRS_BankBalalnce = a.VBABAL_ActualBankBalance,
                                                      VBABRS_Id = a.VBABAL_Id
                                                  }).OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
                                    // _context
                                    // VMS_Payment_VoucherDMO objlp = new VMS_Payment_VoucherDMO();
                                    //objlp.MI_Id = indentres.MI_Id;
                                    //objlp.INVMS_Id = null;
                                    //objlp.VPAYVOU_PaymentDate = DateTime.Now;
                                    //objlp.VPAYVOU_AppliedAmount = data.VTADAA_TotalSactionedAmount;
                                    //objlp.VPAYVOU_ApprovedAmount = 0;
                                    //objlp.VPAYVOU_Remarks = indentres.VTADAA_Remarks;
                                    //objlp.VPAYVOU_BankBalalnce = VBABRS.FirstOrDefault().VBABRS_BankBalalnce;
                                    //objlp.VPAYVOU_StatusFlg = "Pending";
                                    //objlp.VPAYVOU_ChequePrintedFlg = false;
                                    //objlp.VPAYVOU_ActiveFlg = true;
                                    //objlp.VPAYVOU_CreatedBy = data.UserId;
                                    //objlp.VPAYVOU_CreatedDate = indianTime;
                                    //objlp.VPAYVOU_UpdatedBy = data.UserId;
                                    //objlp.VPAYVOU_UpdatedDate = indianTime;
                                    //objlp.VPAYVOU_BankAccountNo = HRMBD_Idf.FirstOrDefault().HRMBD_BankAccountNo;
                                    //objlp.VPAYVOU_VoucherNo = trans_id;
                                    //objlp.HRMBD_Id = HRMBD_Idf.FirstOrDefault().HRMBD_Id;
                                    //objlp.VPAYVOU_SupplierFlg = false;
                                    //objlp.VPAYVOU_SelfPayFlg = false;
                                    //objlp.VPAYVOU_PaymentTo = EmplYoee.FirstOrDefault().EmplYoeeName;
                                    //objlp.VPAYVOU_PaymentMode = "Cheque";
                                    //objlp.HRMBD_InterbankTransFlg = false;
                                    //objlp.VPAYVOU_HRMBD_Id = null;
                                    //_context.Add(objlp);
                                    //var filelist = _context.VMS_TADAApply_FilesDMO.Where(R => R.VTADAA_Id == indentres.VTADAA_Id).ToList();
                                    //if (filelist != null && filelist.Count > 0)
                                    //{
                                    //    foreach (var fd in filelist)
                                    //    {
                                    //        BankAccount_BRS_FilesDMO objfile = new BankAccount_BRS_FilesDMO();
                                    //        objfile.VPAYVOUF_FileName = fd.VTADAAF_FileName;
                                    //        objfile.VPAYVOUF_FilePath = fd.VTADAAF_FilePath;
                                    //        objfile.VPAYVOUF_Filedesc = fd.VTADAAF_Filedesc;
                                    //        objfile.VPAYVOU_Id = objlp.VPAYVOU_Id;
                                    //        objfile.VPAYVOUF_StatusFlg = "approved";
                                    //        objfile.VPAYVOUF_ActiveFlg = true;
                                    //        _context.Add(objfile);
                                    //    }
                                    //}
                                }
                            }
                            else
                            {
                                indentres.VTADAA_StatusFlg = "Rejected";
                                indentres.VTADAA_TotalSactionedAmount = 0;
                            }
                            _context.Update(indentres);
                            var indentresitem = _context.TADA_ApplyDetailsDMO.Where(a => a.VTADAA_Id == data.VTADAA_Id).ToList();
                            if (indentresitem.Count > 0)
                            {

                                foreach (var item in indentresitem)
                                {

                                    foreach (var ap in data.headarray)
                                    {
                                        if (ap.VTADAAD_Id == item.VTADAAD_Id)
                                        {
                                            if (ap.flag == "R")
                                            {

                                                item.VTADAAD_StatusFlg = "Rejected";
                                            }
                                            else
                                            {

                                                item.VTADAAD_StatusFlg = "Approved";
                                            }

                                            item.VTADAAD_UpdatedBy = data.UserId;
                                            item.VTADAAD_UpdatedDate = indianTime;

                                            _context.Update(item);
                                        }

                                    }


                                }

                            }

                            _context.SaveChanges();

                        }
                        else
                        {

                        }
                        data.returnvalue = true;
                        var primaryid = _context.VMS_TADA_Apply_AuthorisationDMO.Where(e => e.VTADAA_Id == obj.VTADAA_Id).ToList();
                        if (primaryid.Count > 0)
                        {
                            long hrid = 0;

                            var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                            if (hrmeid1.Count > 0)
                            {
                                hrid = hrmeid1.FirstOrDefault().Emp_Code;
                            }

                            emaildata(primaryid.FirstOrDefault().VTADAA_Id, hrid, data.MI_Id, data.UserId, data.HRME_Id, data.approvecnt);
                        }
                    }
                    else
                    {
                        data.returnvalue = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //var duplicate = _context.TADA_CityMappingDMO.Where(p => p.MI_Id == data.MI_Id && p.HRMDES_Id == data.HRMDES_Id && p.IVRMMCT_Id == data.city_array[0].IVRMMCT_Id && p.VTADACM_Id != data.VTADACM_Id).ToList();

            return data;
        }
        public TADAApprovalDTO edit(TADAApprovalDTO data)
        {
            try
            {
          
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_APPLY_DETAILS_FOR_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAA_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.VTADAA_Id
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
                        data.EditArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //file upload
                data.TimeArray = _context.TADA_ApplyDMO.Where(M => M.VTADAA_Id == data.VTADAA_Id && M.VTADAA_ActiveFlg == true).Distinct().ToArray();
                data.GetFile = _context.VMS_TADAApply_FilesDMO.Where(R => R.VTADAA_Id == data.VTADAA_Id && R.VTADAAF_ActiveFlag == true).Distinct().ToArray();
                //view details
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TADA_ADavanceMltiple_SDetailsAmt_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.VTADAAAAA_Id
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

                        data.Veiwdetails = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.multipleclientlist = (from a in _context.clientTable
                                           from b in _context.VMS_TADA_MultipleClient_FilesDMO
                                           where a.ISMMCLT_Id == b.ISMMCLT_Id && b.VTADAA_Id == data.VTADAA_Id
                                           select new TADAApprovalDTO
                                           {
                                               client_name = a.ISMMCLT_ClientName,
                                               client_remrks = b.VTADAMCF_Remarks,
                                               ISMMCLT_Id = a.ISMMCLT_Id
                                           }).Distinct().ToArray();
                //school  mapings 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMSTada_TourTravel_Mapiing";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAA_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.VTADAA_Id
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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //editTada
        public TADAApprovalDTO editTada(TADAApprovalDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_FINAL_DETAILS_FOR_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAA_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.VTADAA_Id
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
                        data.EditArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //file upload
                data.TimeArray = _context.TADA_ApplyDMO.Where(M => M.VTADAA_Id == data.VTADAA_Id && M.VTADAA_ActiveFlg == true).Distinct().ToArray();
                data.GetFile = _context.VMS_TADAApply_FilesDMO.Where(R => R.VTADAA_Id == data.VTADAA_Id && R.VTADAAF_ActiveFlag == true).Distinct().ToArray();
                //view details
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TADA_ADavanceMltiple_SDetailsAmt_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.VTADAAAAA_Id
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

                        data.Veiwdetails = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.multipleclientlist = (from a in _context.clientTable
                                           from b in _context.VMS_TADA_MultipleClient_FilesDMO
                                           where a.ISMMCLT_Id == b.ISMMCLT_Id && b.VTADAA_Id == data.VTADAA_Id
                                           select new TADAApprovalDTO
                                           {
                                               client_name = a.ISMMCLT_ClientName,
                                               client_remrks = b.VTADAMCF_Remarks,
                                               ISMMCLT_Id = a.ISMMCLT_Id
                                           }).Distinct().ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TourTravel_Mapiing";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.VTADAAAAA_Id
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
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lead_Verified_Records_Changed_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IVRMMCT_Id
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

                        data.City_Type = retObject.ToArray();


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
        //getfileedit
        public TADAApprovalDTO getfileedit(TADAApprovalDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Document_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = "2"
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAA_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.VTADAA_Id
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
                        data.EditArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //file upload
                data.TimeArray = _context.TADA_ApplyDMO.Where(M => M.VTADAA_Id == data.VTADAA_Id && M.VTADAA_ActiveFlg == true).Distinct().ToArray();
                data.GetFile = _context.VMS_TADAApply_FilesDMO.Where(R => R.VTADAA_Id == data.VTADAA_Id && R.VTADAAF_ApprovedFlg == false).Distinct().ToArray();
                //view details
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TADA_ADavanceMltiple_SDetailsAmt_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.VTADAAAAA_Id
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

                        data.Veiwdetails = retObject.ToArray();


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
        public TADAApprovalDTO deactive(TADAApprovalDTO data)
        {

            try
            {
                if (data.VTADAA_Id > 0)
                {


                    var resultone = _context.TADA_ApplyDMO.Single(t => t.VTADAA_Id == data.VTADAA_Id && t.MI_Id == data.MI_Id);

                    if (resultone.VTADAA_ActiveFlg == true)
                    {
                        resultone.VTADAA_ActiveFlg = false;
                        resultone.VTADAA_UpdatedDate = DateTime.Now;
                        resultone.VTADAA_UpdatedBy = data.UserId;
                        _context.Update(resultone);



                    }
                    var resuttwo = _context.TADA_ApplyDetailsDMO.Where(p => p.VTADAA_Id == data.VTADAA_Id).ToList();
                    if (resuttwo.Count > 0)
                    {
                        Array remove_details = _context.TADA_ApplyDetailsDMO.Where(a => a.VTADAA_Id == data.VTADAA_Id).ToArray();

                        foreach (var d in remove_details)
                        {
                            _context.Remove(d);
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


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return data;
        }

        //adedd
        public TADAApprovalDTO GetSavedDetails(TADAApprovalDTO data)
        {
            try
            {
                string departments = "0";
                string desgination = "0";
                string emplist = "0";

                var TadaList = _context.TADA_ApplyDMO.Where(R => R.VTADAA_Id == data.VTADAA_Id).ToList();

                var Emplisttada = _context.MasterEmployee.Where(R => R.HRME_Id == TadaList.FirstOrDefault().HRME_Id).ToList();

                var HRMDC_ID = _context.HR_Master_Department.Where(R => R.MI_Id == Emplisttada.FirstOrDefault().MI_Id && R.HRMD_Id == Emplisttada.FirstOrDefault().HRMD_Id).FirstOrDefault().HRMDC_ID;

                departments = departments + "," + HRMDC_ID;

                emplist = emplist + "," + TadaList.FirstOrDefault().HRME_Id;

                desgination = desgination + "," + Emplisttada.FirstOrDefault().HRMDES_Id;


                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(TadaList.FirstOrDefault().VTADAA_FromDate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(TadaList.FirstOrDefault().VTADAA_ToDate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");
                //ISM_User_Employees_MappingDMO

                var UserIdS = _context.ISM_User_Employees_MappingDMO.Where(R => R.HRME_Id == TadaList.FirstOrDefault().HRME_Id && R.ISMUSEMM_ActiveFlag == true && R.ISMUSEMM_Order == 1).ToList();
                var GET_EMPID = _context.Staff_User_Login.Where(a => a.Id == UserIdS.FirstOrDefault().User_Id).ToList();

                long empid = GET_EMPID.Count > 0 ? GET_EMPID.FirstOrDefault().Emp_Code : 0;


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_DailyReport_Approval_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@emplist", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getsaveddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "TADA_Emps_WorkingHours";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = TadaList.FirstOrDefault().MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar) { Value = contodate });

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
                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1]);
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.filldata = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "TADA_GET_EMPLOYEE_DEPT_WISE_STATUS_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar) { Value = desgination });
                    cmd.Parameters.Add(new SqlParameter("@emplist", SqlDbType.VarChar) { Value = emplist });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = empid });

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
                                    dataRow1.Add(dataReader1.GetName(iFiled1), dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1]);
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.empwise_statuslist = retObject1.ToArray();
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
        //Uploadfile
        public TADAApprovalDTO Uploadfile(TADAApprovalDTO data)
        {
            try
            {
                if (data.filistarray != null && data.filistarray.Length > 0 && data.VTADAA_Id > 0)
                {
                    var tadaupdate = _context.TADA_ApplyDMO.Single(R => R.VTADAA_Id == data.VTADAA_Id);
                    if (data.VTADAA_ApprovedFlg == true)
                    {

                        tadaupdate.VTADAA_ApprovedFlg = true;
                        tadaupdate.VTADAA_Finaldocument = true;
                    }
                    else
                    {
                        tadaupdate.VTADAA_Finaldocument = false;
                    }
                    _context.Update(tadaupdate);

                    foreach (var d in data.filistarray)
                    {
                        var update = _context.VMS_TADAApply_FilesDMO.Single(R => R.VTADAAF_Id == d.VTADAAF_Id);
                        update.VTADAAF_VerfieRemarks = d.VTADAAF_VerfieRemarks;
                        if (d.Flag == "R")
                        {
                            update.VTADAAF_ApprovedFlg = false;
                        }
                        else
                        {
                            update.VTADAAF_ApprovedFlg = true;
                        }
                        _context.Update(update);
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
                            //string emailid = "sanjeev@globalqtytrainig.com";
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
                // data.returnval = "admin";
            }
            return data;
        }
        public string emaildata(long INVMPIAPP_Id, long HRME_Id, long MI_Id, long UserID, long appliedhrmeid, int countt)
        {
            string suss = "";
            try
            {
                decimal total = 0;
                string str = "";
                var appliedmailid = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == appliedhrmeid && a.HRMEM_DeFaultFlag == "default").ToList();
                String applymail = "";
                if (appliedmailid.Count > 0)
                {
                    applymail = appliedmailid.FirstOrDefault().HRMEM_EmailId;
                }
                var tomailid = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == HRME_Id && a.HRMEM_DeFaultFlag == "default").ToList();
                String ccmail = "";
                if (tomailid.Count > 0)
                {
                    ccmail = tomailid.FirstOrDefault().HRMEM_EmailId;
                }

                var approvallist = (from a in _db.HR_PROCESSDMO
                                    from b in _db.HR_Process_Auth_OrderNoDMO
                                    where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAPL" && b.IVRMUL_Id == UserID
                                    select new TADAApprovalDTO
                                    {
                                        UserId = b.IVRMUL_Id,
                                        HRPAON_SanctionLevelNo = b.HRPAON_SanctionLevelNo
                                    }).Distinct().ToList();


                if (approvallist.Count > 0)
                {

                    var nextleveluser = (from a in _db.HR_PROCESSDMO
                                         from b in _db.HR_Process_Auth_OrderNoDMO
                                         where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAPL" && b.HRPAON_SanctionLevelNo == approvallist.FirstOrDefault().HRPAON_SanctionLevelNo + 1
                                         select new TADAApprovalDTO
                                         {
                                             UserId = b.IVRMUL_Id,
                                             HRPAON_SanctionLevelNo = b.HRPAON_SanctionLevelNo
                                         }).Distinct().ToList();
                    if (nextleveluser.Count > 0)
                    {
                        long apprhrmeid = 0;
                        var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == nextleveluser.FirstOrDefault().UserId).ToList();
                        if (hrmeid1.Count > 0)
                        {
                            apprhrmeid = hrmeid1.FirstOrDefault().Emp_Code;
                        }
                        var tomailidapp = _db.Multiple_Email_DMO.Where(a => a.HRME_Id == apprhrmeid && a.HRMEM_DeFaultFlag == "default").ToList();
                        if (tomailidapp.Count > 0)
                        {
                            if (tomailidapp.FirstOrDefault().HRMEM_EmailId != null && tomailidapp.FirstOrDefault().HRMEM_EmailId != "")
                            {
                                if (ccmail != "")
                                {
                                    if (applymail != "" && applymail != null)
                                    {
                                        ccmail = ccmail + "," + applymail;
                                    }

                                }
                                else
                                {
                                    if (applymail != "" && applymail != null)
                                    {
                                        ccmail = applymail;
                                    }
                                }

                                if (countt > 0)
                                {
                                    suss = sendmail(MI_Id, UserID, "APPLYSTARTTADAADVAPPROVAL", tomailidapp.FirstOrDefault().HRMEM_EmailId, str, INVMPIAPP_Id, ccmail);
                                }
                                else
                                {

                                    suss = sendmail(MI_Id, UserID, "APPLYSTARTTADAADVREJECT", tomailidapp.FirstOrDefault().HRMEM_EmailId, str, INVMPIAPP_Id, ccmail);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (applymail != "" && applymail != null)
                        {
                            if (countt > 0)
                            {
                                suss = sendmail(MI_Id, UserID, "APPLYENDADAADVAPPROVAL", applymail, str, INVMPIAPP_Id, ccmail);
                            }
                            else
                            {
                                suss = sendmail(MI_Id, UserID, "APPLYENDADAADVREJECT", applymail, str, INVMPIAPP_Id, ccmail);
                            }

                        }

                    }

                }

                //}

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return suss;
        }

        //EMAIL

        #region   email
        public string sendmail(long MI_Id, long UserID, string Template, string emailid, string items, long INVMPI_Id, string ccemailid)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "APPLYTADA_ALL_MAIL_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMPR_Id",
                           SqlDbType.VarChar)
                        {
                            Value = INVMPI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Template",
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
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

                Mailmsg = Mailmsg.Replace("[ITEMS]", items);

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;


                        message.AddTo(emailid);




                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }

                        if (ccemailid != null && ccemailid != "")
                        {
                            List<string> eevvq = new List<string>(ccemailid.Split(','));
                            eevvq.Reverse();

                            if (eevvq.Count > 0)
                            {
                                foreach (var item in eevvq)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }






                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


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

                                emailMessage.To.Add(new MailAddress(emailid));



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

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "INVALL_IVRM_Email_Outgoing";
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion


        //END



    }
}
