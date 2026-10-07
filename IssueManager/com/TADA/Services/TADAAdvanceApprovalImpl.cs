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
    public class TADAAdvanceApprovalImpl : Interface.TADAAdvanceApprovalInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;
        public TADAAdvanceApprovalImpl(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }
        public TADAAdvanceApprovalDTO getalldata(TADAAdvanceApprovalDTO data)
        {
            try
            {
              
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_ADVANCE_APPROVAL";
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead 
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
        //getTourApproval
        public TADAAdvanceApprovalDTO getTourApproval(TADAAdvanceApprovalDTO data)
        {
            try
            {
                
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_TOUR_APPROVAL";                                     
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead 
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

                data.TourPlanDetails = _context.ISM_Emp_RoutePlanDMO.Where(R => R.ISM_Active == true).Distinct().ToArray();
                //addd
               

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADAAdvanceApprovalDTO savedta(TADAAdvanceApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var indlist = _context.TADA_AdavanceApplyDMO.Where(w => w.VTADAAA_Id == data.VTADAAA_Id).ToList();
                if (indlist.Count > 0)
                {
                    VMS_TADA_Advance_Apply_AuthorisationDMO obj = new VMS_TADA_Advance_Apply_AuthorisationDMO();
                    obj.VTADAAA_Id = data.VTADAAA_Id;
                    obj.IVRMUL_Id = data.UserId;
                    obj.VTADAAAA_SanctioningLevel = data.level;
                    obj.VTADAAAA_Remarks = data.VTADAAA_Remarks;
                    obj.VTADAAAA_SactionedAmount = data.VTADAAA_TotalSactionedAmount;
                    if (data.approvecnt > 0)
                    {
                        obj.VTADAAAA_StatusFlg = "Approved";
                    }
                    else
                    {
                        obj.VTADAAAA_StatusFlg = "Rejected";
                    }
                    obj.VTADAAAA_Date = indianTime;
                    obj.VTADAAAA_ActiveFlg = true;
                    obj.VTADAAAA_CreatedBy = data.UserId;
                    obj.VTADAAAA_UpdatedBy = data.UserId;
                    obj.VTADAAAA_CreatedDate = indianTime;
                    obj.VTADAAAA_UpdatedDate = indianTime;
                    _context.Add(obj);
                    if (data.headarray != null)
                    {
                        foreach (var item in data.headarray)
                        {
                            VMS_TADA_Advance_Apply_Authorisation_HeadsDMO tobj = new VMS_TADA_Advance_Apply_Authorisation_HeadsDMO();
                            tobj.VTADAAAA_Id = obj.VTADAAAA_Id;
                            tobj.VTADAAAD_Id = item.VTADAAAD_Id;
                            tobj.VTADAAAAH_SactionedAmount = item.VTADAAAAH_SactionedAmount;
                            tobj.VTADAAAAH_Remarks = item.VTADAAAD_Remarks;
                            if (item.flag == "R")
                            {
                                tobj.VTADAAAAH_StatusFlg = "Rejected";
                            }
                            else
                            {

                                tobj.VTADAAAAH_StatusFlg = "Approved";
                            }
                            tobj.VTADAAAAH_Date = indianTime;
                            tobj.VTADAAAAH_ActiveFlg = true;
                            tobj.VTADAAAAH_CreatedDate = indianTime;
                            tobj.VTADAAAAH_UpdatedDate = indianTime;
                            tobj.VTADAAAAH_UpdatedBy = data.UserId;
                            tobj.VTADAAAAH_CreatedBy = data.UserId;
                            _context.Add(tobj);
                        }
                    }
                    int ext = _context.SaveChanges();
                    if (ext > 0)
                    {
                        var checkmaxorder = (from a in _context.HR_PROCESSDMO
                                             from b in _context.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAD"
                                             select b.HRPAON_SanctionLevelNo).Max();

                        if (checkmaxorder == data.level)
                        {
                            var indentres = _context.TADA_AdavanceApplyDMO.Single(a => a.VTADAAA_Id == data.VTADAAA_Id);
                            indentres.VTADAAA_UpdatedBy = data.UserId;
                            indentres.VTADAAA_UpdatedDate = indianTime;
                            if (data.approvecnt > 0)
                            {
                                string trans_id = "";
                                indentres.VTADAAA_StatusFlg = "Approved";
                                indentres.VTADAAA_TotalSactionedAmount = data.VTADAAA_TotalSactionedAmount;
                                //oderby Client

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
                                    //VMS_Payment_VoucherDMO objlp = new VMS_Payment_VoucherDMO();
                                    //objlp.MI_Id = indentres.MI_Id;
                                    //objlp.INVMS_Id = null;
                                    //objlp.VPAYVOU_PaymentDate = DateTime.Now;
                                    //objlp.VPAYVOU_AppliedAmount = data.VTADAAA_TotalSactionedAmount;
                                    //objlp.VPAYVOU_ApprovedAmount = 0;
                                    //objlp.VPAYVOU_Remarks = indentres.VTADAAA_Remarks;
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

                                }
                            }
                            else
                            {
                                indentres.VTADAAA_StatusFlg = "Rejected";
                                indentres.VTADAAA_TotalSactionedAmount = 0;
                            }
                            _context.Update(indentres);
                            var indentresitem = _context.TADA_AdvanceApplyDetailsDMO.Where(a => a.VTADAAA_Id == data.VTADAAA_Id).ToList();
                            if (indentresitem.Count > 0)
                            {

                                foreach (var item in indentresitem)
                                {

                                    foreach (var ap in data.headarray)
                                    {
                                        if (ap.VTADAAA_Id == item.VTADAAA_Id)
                                        {
                                            if (ap.flag == "R")
                                            {

                                                item.VTADAAAD_StatusFlg = "Rejected";
                                            }
                                            else
                                            {

                                                item.VTADAAAD_StatusFlg = "Approved";
                                            }

                                            item.VTADAAAD_UpdatedBy = data.UserId;
                                            item.VTADAAAD_UpdatedDate = indianTime;

                                            _context.Update(item);
                                        }

                                    }


                                }

                            }

                            _context.SaveChanges();

                        }

                        data.returnvalue = true;

                        var primaryid = _context.VMS_TADA_Advance_Apply_AuthorisationDMO.Where(e => e.VTADAAAA_Id == obj.VTADAAAA_Id).ToList();
                        if (primaryid.Count > 0)
                        {
                            long hrid = 0;

                            var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                            if (hrmeid1.Count > 0)
                            {
                                hrid = hrmeid1.FirstOrDefault().Emp_Code;
                            }

                            emaildata(primaryid.FirstOrDefault().VTADAAAA_Id, hrid, data.MI_Id, data.UserId, data.HRME_Id, data.approvecnt);
                        }
                    }
                    else
                    {
                        data.returnvalue = false;
                    }
                }

                //payment senbd mail
                // var a = _context.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_Id == data.VTADAAA_Id && R.VTADAAA_StatusFlg == "Approved").ToList();
                //if(a.Count > 0)
                // {

                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //edit
        //TourApproval
        public TADAAdvanceApprovalDTO TourApproval(TADAAdvanceApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var indlist = _context.TADA_AdavanceApplyDMO.Where(w => w.VTADAAA_Id == data.VTADAAA_Id).ToList();
                if (indlist.Count > 0)
                {

                    if (data.listEmployeeSales != null && data.listEmployeeSales.Length > 0 && data.AdminFlag==true)
                    {

                        var Routplan = _context.ISM_Emp_RoutePlan_CategoryDMO.Where(R => R.IER_ID == Convert.ToInt64(data.IER_ID)).Distinct().ToList();
                        if (Routplan.Count > 0)
                        {
                            foreach (var d in Routplan)
                            {
                                _context.Remove(d);

                            }
                        }
                        var Routplanclient = _context.ISM_Emp_RoutePlan_ClientDetailsDMO.Where(R => R.IER_ID == Convert.ToInt64(data.IER_ID)).Distinct().ToList();
                        if (Routplanclient.Count > 0)
                        {
                            foreach (var d in Routplanclient)
                            {
                                _context.Remove(d);

                            }
                        }
                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            foreach (var d in data.listEmployeeSales)
                            {
                                var contactExistsP = _context.Database.ExecuteSqlCommand("TADA_U_Emp_RoutePlan @p0, @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.IER_ID, data.HRME_Id, data.PlanStartDate, data.PlanEndDate, d.ClientId, d.CategoryId, data.UserId, d.ISMSMPR_Id, data.PlanName, data.TourRemark);
                                data.message = "save";
                            }

                        
                        }
                        else
                        {
                            data.message = "notsave";
                            
                        }


                    }
                    VMS_TADA_Advance_Apply_AuthorisationDMO obj = new VMS_TADA_Advance_Apply_AuthorisationDMO();
                    obj.VTADAAA_Id = data.VTADAAA_Id;
                    obj.IVRMUL_Id = data.UserId;
                    obj.VTADAAAA_SanctioningLevel = data.level;
                    obj.VTADAAAA_Remarks = data.VTADAAA_Remarks;
                    obj.VTADAAAA_SactionedAmount = data.VTADAAA_TotalSactionedAmount;
                    if (data.approvecnt > 0)
                    {
                        obj.VTADAAAA_StatusFlg = "Approved";
                    }
                    else
                    {
                        obj.VTADAAAA_StatusFlg = "Rejected";
                    }
                    obj.VTADAAAA_Date = indianTime;
                    obj.VTADAAAA_ActiveFlg = true;
                    obj.VTADAAAA_CreatedBy = data.UserId;
                    obj.VTADAAAA_UpdatedBy = data.UserId;
                    obj.VTADAAAA_CreatedDate = indianTime;
                    obj.VTADAAAA_UpdatedDate = indianTime;
                    _context.Add(obj);
                    if (data.headarray != null)
                    {
                        foreach (var item in data.headarray)
                        {
                            VMS_TADA_Advance_Apply_Authorisation_HeadsDMO tobj = new VMS_TADA_Advance_Apply_Authorisation_HeadsDMO();
                            tobj.VTADAAAA_Id = obj.VTADAAAA_Id;
                            tobj.VTADAAAD_Id = item.VTADAAAD_Id;
                            tobj.VTADAAAAH_SactionedAmount = item.VTADAAAAH_SactionedAmount;
                            tobj.VTADAAAAH_Remarks = item.VTADAAAD_Remarks;
                            if (item.flag == "R")
                            {
                                tobj.VTADAAAAH_StatusFlg = "Rejected";
                            }
                            else
                            {

                                tobj.VTADAAAAH_StatusFlg = "Approved";
                            }
                            tobj.VTADAAAAH_Date = indianTime;
                            tobj.VTADAAAAH_ActiveFlg = true;
                            tobj.VTADAAAAH_CreatedDate = indianTime;
                            tobj.VTADAAAAH_UpdatedDate = indianTime;
                            tobj.VTADAAAAH_UpdatedBy = data.UserId;
                            tobj.VTADAAAAH_CreatedBy = data.UserId;
                            _context.Add(tobj);
                        }
                    }
                    int ext = _context.SaveChanges();
                    if (ext > 0)
                    {
                        var checkmaxorder = (from a in _context.HR_PROCESSDMO
                                             from b in _context.HR_Process_Auth_OrderNoDMO
                                             where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TATOUR"
                                             select b.HRPAON_SanctionLevelNo).Max();
                      

                        if (checkmaxorder == data.level)
                        {
                            var indentres = _context.TADA_AdavanceApplyDMO.Single(a => a.VTADAAA_Id == data.VTADAAA_Id);
                            indentres.VTADAAA_UpdatedBy = data.UserId;
                            indentres.VTADAAA_UpdatedDate = indianTime;
                            if (data.approvecnt > 0)
                            {
                                string trans_id = "";
                                indentres.VTADAAA_StatusFlg = "Approved";
                                indentres.VTADAAA_TotalSactionedAmount = data.VTADAAA_TotalSactionedAmount;
                                //oderby Client

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
                               

                                }
                            }
                            else
                            {
                                indentres.VTADAAA_StatusFlg = "Rejected";
                                indentres.VTADAAA_TotalSactionedAmount = 0;
                            }
                            _context.Update(indentres);
                            var indentresitem = _context.TADA_AdvanceApplyDetailsDMO.Where(a => a.VTADAAA_Id == data.VTADAAA_Id).ToList();
                            if (indentresitem.Count > 0)
                            {

                                foreach (var item in indentresitem)
                                {

                                    foreach (var ap in data.headarray)
                                    {
                                        if (ap.VTADAAA_Id == item.VTADAAA_Id)
                                        {
                                            if (ap.flag == "R")
                                            {

                                                item.VTADAAAD_StatusFlg = "Rejected";
                                            }
                                            else
                                            {

                                                item.VTADAAAD_StatusFlg = "Approved";
                                            }

                                            item.VTADAAAD_UpdatedBy = data.UserId;
                                            item.VTADAAAD_UpdatedDate = indianTime;

                                            _context.Update(item);
                                        }

                                    }


                                }

                            }

                            _context.SaveChanges();
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())                            {                                cmd.CommandText = "VMSTADA_RoutePaymentTime";                                cmd.CommandType = CommandType.StoredProcedure;                                cmd.Parameters.Add(new SqlParameter("@IER_ID",                                  SqlDbType.VarChar)                                {                                    Value = data.IER_ID                                });                                cmd.Parameters.Add(new SqlParameter("@userid",                                  SqlDbType.VarChar)                                {                                    Value = data.UserId                                });                                if (cmd.Connection.State != ConnectionState.Open)                                    cmd.Connection.Open();                                var retObject = new List<dynamic>();                                try                                {                                    using (var dataReader = cmd.ExecuteReader())                                    {                                        while (dataReader.Read())                                        {                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                            {                                                dataRow.Add(                                                    dataReader.GetName(iFiled),                                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                                );                                            }                                            retObject.Add((ExpandoObject)dataRow);                                        }                                    }




                                }                                catch (Exception ex)                                {                                    Console.WriteLine(ex.Message);                                }                            }
                        }
                        else
                        {
                            if(data.VTADAAA_TotalSactionedAmount > 0)
                            {

                            }
                            else
                            {
                                var indentres = _context.TADA_AdavanceApplyDMO.Single(a => a.VTADAAA_Id == data.VTADAAA_Id);
                                indentres.VTADAAA_UpdatedBy = data.UserId;
                                indentres.VTADAAA_UpdatedDate = indianTime;
                                indentres.VTADAAA_StatusFlg = "Rejected";
                                indentres.VTADAAA_TotalSactionedAmount = 0;
                                _context.Update(indentres);
                                _context.SaveChanges();

                            }                          

                           
                        }
                        data.returnvalue = true;

                        var primaryid = _context.VMS_TADA_Advance_Apply_AuthorisationDMO.Where(e => e.VTADAAAA_Id == obj.VTADAAAA_Id).ToList();
                        if (primaryid.Count > 0)
                        {
                            long hrid = 0;

                            var hrmeid1 = _db.Staff_User_Login.Where(c => c.Id == data.UserId).ToList();
                            if (hrmeid1.Count > 0)
                            {
                                hrid = hrmeid1.FirstOrDefault().Emp_Code;
                            }

                            emaildata(primaryid.FirstOrDefault().VTADAAAA_Id, hrid, data.MI_Id, data.UserId, data.HRME_Id, data.approvecnt);
                        }
                    }
                    else
                    {
                        data.returnvalue = false;
                    }
                }

                //payment senbd mail
                // var a = _context.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_Id == data.VTADAAA_Id && R.VTADAAA_StatusFlg == "Approved").ToList();
                //if(a.Count > 0)
                // {

                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TADAAdvanceApprovalDTO edit(TADAAdvanceApprovalDTO data)
        {
            try
            {
              
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_ADVANCE_DETAILS_FOR_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.VTADAAA_Id
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
                data.TimeArray = _context.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_ActiveFlg == true && R.VTADAAA_Id == data.VTADAAA_Id).Distinct().ToArray();
                data.multipleclientlist = (from a in _context.clientTable
                                           from b in _context.VMS_TADDA_MultipleClient_FilesDMO
                                           where a.ISMMCLT_Id == b.ISMMCLT_Id && b.VTADAAA_Id == data.VTADAAA_Id
                                           select new TADAAdvanceApprovalDTO
                                           {
                                               client_name = a.ISMMCLT_ClientName,
                                               client_remrks = b.VTADDAMCF_Remarks,
                                               ISMMCLT_Id=a.ISMMCLT_Id
                                           }).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //updateallowence
        public TADAAdvanceApprovalDTO updateallowence(TADAAdvanceApprovalDTO data)
        {
            try
            {
                if (data.listEmployeeSales != null && data.listEmployeeSales.Length > 0)
                {
                   
                    var Routplan = _context.ISM_Emp_RoutePlan_CategoryDMO.Where(R => R.IER_ID == Convert.ToInt64(data.IER_ID)).Distinct().ToList();
                    if (Routplan.Count > 0)
                    {
                        foreach (var d in Routplan)
                        {
                            _context.Remove(d);

                        }
                    }
                    var Routplanclient = _context.ISM_Emp_RoutePlan_ClientDetailsDMO.Where(R => R.IER_ID == Convert.ToInt64(data.IER_ID)).Distinct().ToList();
                    if (Routplanclient.Count > 0)
                    {
                        foreach (var d in Routplanclient)
                        {
                            _context.Remove(d);

                        }
                    }
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        foreach (var d in data.listEmployeeSales)
                        {
                            var contactExistsP = _context.Database.ExecuteSqlCommand("TADA_U_Emp_RoutePlan @p0, @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.IER_ID, data.HRME_Id, data.PlanStartDate, data.PlanEndDate, d.ClientId, d.CategoryId, data.UserId, d.ISMSMPR_Id, data.PlanName, data.TourRemark);
                            data.message = "save";
                        }

                        //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        //{
                        //    cmd.CommandText = "TADAAdavance_LeadRegionTourDetails";
                        //    cmd.CommandType = CommandType.StoredProcedure;

                        //    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                        //      SqlDbType.VarChar)
                        //    {
                        //        Value = data.IER_ID
                        //    });
                        //    if (cmd.Connection.State != ConnectionState.Open)
                        //        cmd.Connection.Open();

                        //    var retObject = new List<dynamic>();
                        //    try
                        //    {
                        //        using (var dataReader = cmd.ExecuteReader())
                        //        {
                        //            while (dataReader.Read())
                        //            {
                        //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        //                {
                        //                    dataRow.Add(
                        //                        dataReader.GetName(iFiled),
                        //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                        //                    );
                        //                }

                        //                retObject.Add((ExpandoObject)dataRow);
                        //            }
                        //        }

                        //        data.editdataOthelist = retObject.ToArray();


                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Console.WriteLine(ex.Message);
                        //    }
                        //}
                    }
                    else
                    {
                        data.message = "notsave";
                    }


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.message = "admin";
            }
            return data;
        }
        //editTour
        public TADAAdvanceApprovalDTO getView(TADAAdvanceApprovalDTO data)
        {
            try
            {
                //IER_ID
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADAAdavance_LeadRegionTourDetails";                
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.editdataOthelist = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_s_RoutePlan_EditDetails";                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.GetPlanerdata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //
                data.clientlist = _context.clientTable.Where(R => R.ISMMCLT_ActiveFlag == true).Distinct().ToArray();
                data.multipleclientlist = (from a in _context.clientTable
                                           from b in _context.VMS_TADDA_MultipleClient_FilesDMO
                                           where a.ISMMCLT_Id == b.ISMMCLT_Id && b.VTADAAA_Id == data.VTADAAA_Id
                                           select new TADAAdvanceApprovalDTO
                                           {
                                               client_name = a.ISMMCLT_ClientName,
                                               client_remrks = b.VTADDAMCF_Remarks,
                                               ISMMCLT_Id = a.ISMMCLT_Id
                                           }).Distinct().ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_Route_EmpCategory";
                    cmd.CommandType = CommandType.StoredProcedure;

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

                        data.GetCategoryArray = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                // data.Priority= _context.

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_RouteDesionMakingMD";
                    cmd.CommandType = CommandType.StoredProcedure;
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

                        data.Priority = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Categorywise_Min_Max";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRMEId",
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

                        data.Categorywise_MinMax = retObject.ToArray();


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
        public TADAAdvanceApprovalDTO editTour(TADAAdvanceApprovalDTO data)
        {
            try
            {
             
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_ADVANCE_TOUR_FOR_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.VTADAAA_Id
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
                data.TimeArray = _context.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_ActiveFlg == true && R.VTADAAA_Id == data.VTADAAA_Id).Distinct().ToArray();
                var listTada= _context.TADA_AdavanceApplyDMO.Where(R => R.VTADAAA_ActiveFlg == true && R.VTADAAA_Id == data.VTADAAA_Id).ToList();
                data.multipleclientlist = (from a in _context.clientTable
                                           from b in _context.VMS_TADDA_MultipleClient_FilesDMO
                                           where a.ISMMCLT_Id == b.ISMMCLT_Id && b.VTADAAA_Id == data.VTADAAA_Id
                                           select new TADAAdvanceApprovalDTO
                                           {
                                               client_name = a.ISMMCLT_ClientName,
                                               client_remrks = b.VTADDAMCF_Remarks
                                           }).Distinct().ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "usp_s_RoutePaymentTime";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@IER_ID",                    SqlDbType.VarChar)                    {                        Value = data.IER_ID                    });

                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.EditArrayTwo = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "Checkout_Assests_New";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",                    SqlDbType.VarChar)                    {                        Value = data.HRME_Id                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.GetArray = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }
                //ISM_Emp_RoutePlanDMO
                long IER_ID = 0;
                data.TourPlanDetails = _context.ISM_Emp_RoutePlanDMO.Where(R => R.IER_ID == IER_ID).Distinct().ToArray();
                //Client_Master
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "usp_s_RoutePlanGist";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@IER_ID",                    SqlDbType.VarChar)                    {                        Value = listTada.FirstOrDefault().IER_ID                    });

                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.Client_Master = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TourTravel_Mapiing";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
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

                //concal Lop
                //ConcalLopdata
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_ConCalLop";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.ConcalLopdata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //GpsLocationTada
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_Tracking";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
                        data.GpsLocationTada = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_get_Category_parameters";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
        public TADAAdvanceApprovalDTO TadaAllwence(TADAAdvanceApprovalDTO data)
        {
            try
            {
                
                //LeadList
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_s_Route_LeadsDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMRC_CategoryId",
                      SqlDbType.VarChar)
                    {
                        Value = data.IMRC_CategoryId
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

                        data.LeadList = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fo_DailyPunch_Dtails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
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
                        data.TimeArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if(data.TimeArray !=null   && data.TimeArray.Length >0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADA_Advance_ApplyCount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Hrme_id", SqlDbType.BigInt) { Value = data.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt) { Value = "1" });
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
                            data.GetReport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADA_Advance_ApplyCount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Hrme_id", SqlDbType.BigInt) { Value = data.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt) { Value = "2" });
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


        public TADAAdvanceApprovalDTO CalculateTada(TADAAdvanceApprovalDTO data)
        {
            try
            {
                //IER_ID
                if (data.Plan_DepartureDetails != null && data.Plan_DepartureDetails.Length > 0)
                {
                   

                    foreach (var d in data.Plan_DepartureDetails)
                    {
                        var contactExistsP = _context.Database.ExecuteSqlCommand("usp_i_RoutePlan_DepartureDetails @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", d.IER_ID, d.IVRMMS_Id, d.IRPD_DepartureDate, d.IRPD_DepartureTime, d.IRPD_ArrivalDate, d.IRPD_ArrivalTime, data.UserId, d.IRPD_Order);
                        data.message = "save";
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_LeadRegionTourDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@IER_ID",
                          SqlDbType.VarChar)
                        {
                            Value = data.IER_ID
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

                            data.editdataOthelist = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                else
                {
                    data.message = "notsave";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.message = "admin";
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
                                    where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAD" && b.IVRMUL_Id == UserID
                                    select new TADAAdvanceApprovalDTO
                                    {
                                        UserId = b.IVRMUL_Id,
                                        HRPAON_SanctionLevelNo = b.HRPAON_SanctionLevelNo
                                    }).Distinct().ToList();


                if (approvallist.Count > 0)
                {

                    var nextleveluser = (from a in _db.HR_PROCESSDMO
                                         from b in _db.HR_Process_Auth_OrderNoDMO
                                         where a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "TAAD" && b.HRPAON_SanctionLevelNo == approvallist.FirstOrDefault().HRPAON_SanctionLevelNo + 1
                                         select new TADAAdvanceApprovalDTO
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
                                    suss = sendmail(MI_Id, UserID, "STARTTADAADVAPPROVAL", tomailidapp.FirstOrDefault().HRMEM_EmailId, str, INVMPIAPP_Id, ccmail);
                                }
                                else
                                {

                                    suss = sendmail(MI_Id, UserID, "STARTTADAADVREJECT", tomailidapp.FirstOrDefault().HRMEM_EmailId, str, INVMPIAPP_Id, ccmail);
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
                                suss = sendmail(MI_Id, UserID, "ENDADAADVAPPROVAL", applymail, str, INVMPIAPP_Id, ccmail);
                            }
                            else
                            {
                                suss = sendmail(MI_Id, UserID, "ENDADAADVREJECT", applymail, str, INVMPIAPP_Id, ccmail);
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
        //getView

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
                        cmd.CommandText = "TADA_ALL_MAIL_PARAMETER";
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
        //
    }
}
