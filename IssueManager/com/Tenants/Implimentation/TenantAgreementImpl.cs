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
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.Rating;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IssueManager.com.Tenants.Implimentation
{
    public class TenantAgreementImpl : Interface.TenantAgreementInterface
    {
        public IssueManagerContext _context;

        public TenantAgreementImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }

        //Master Type
        public TenantAgreementDTO OnLoadRatingMasterType(TenantAgreementDTO data)
        {
            try
            {
                data.reference = _context.MasterSource.ToArray();
                data.building = _context.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id && t.HRMB_ActiveFlag == true).ToArray();
                data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.getfloordetails = _context.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.getownerdetails = _context.Tenant_Master_Owner.ToArray();
                data.tenants = _context.TenantAgreement.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.tenantrenewal = (from a in _context.TenantAgreement
                                      from b in _context.TenantAgreementRenewalDMO
                                      where (a.TENANTAGR_Id == b.TENANTAGR_Id && a.MI_Id==data.MI_Id)
                                      select new TenantAgreementDTO
                                      {
                                          TENANTAGR_Id = a.TENANTAGR_Id,
                                          TENANTAGRREN_Id = b.TENANTAGRREN_Id,
                                          TENANTAGRREN_PreAgrFromDate = b.TENANTAGRREN_PreAgrFromDate,
                                          TENANTAGRREN_Remarks = b.TENANTAGRREN_Remarks,
                                          TENANTAGRREN_PreAgrToDate = b.TENANTAGRREN_PreAgrToDate,
                                          TENANTAGR_TenantName=a.TENANTAGR_TenantName

                                      }
                                    ).Distinct().ToArray();

                data.getloaddata = (from a in _context.TenantAgreement
                                    from b in _context.HR_Master_Building
                                    from c in _context.HR_Master_Room
                                    where (a.HRMB_Id == b.HRMB_Id && a.HRMR_Id == c.HRMR_Id && a.MI_Id == data.MI_Id)
                                    select new TenantAgreementDTO
                                    {
                                        TENANTAGR_Address = a.TENANTAGR_Address,
                                        TENANTAGR_AdvanceAmount = a.TENANTAGR_AdvanceAmount,
                                        TENANTAGR_AgreementDuration = a.TENANTAGR_AgreementDuration,
                                        TENANTAGR_Id = a.TENANTAGR_Id,
                                        TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                        TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                        TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                        TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                        TENANTAGR_MonthlyRent = a.TENANTAGR_MonthlyRent,
                                        TENANTAGR_NoOfOccupants = a.TENANTAGR_NoOfOccupants,
                                        TENANTAGR_Remarks = a.TENANTAGR_Remarks,
                                        TENANTAGR_RenewedFlag = a.TENANTAGR_RenewedFlag,
                                        TENANTAGR_RentDueDay = a.TENANTAGR_RentDueDay,
                                        TENANTAGR_PANCardNo = a.TENANTAGR_PANCardNo,
                                        TENANTAGR_RenewalPercentage = a.TENANTAGR_RenewalPercentage,
                                        PAMS_Id = a.TENANTAGR_BySource,
                                     
                                        //TENANTAGR_RentDueMonth = a.TENANTAGR_RentDueMonth,
                                        TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                        HRMB_BuildingName=b.HRMB_BuildingName,
                                        HRMR_RoomName=c.HRMR_RoomName,
                                        count = _context.TenantAgreementFilesDMO.Where(d => a.TENANTAGR_Id == d.TENANTAGR_Id).Count(),
                                        TENANTAGR_LeftFlg=a.TENANTAGR_LeftFlg,
                                        TENANTAGR_Aadhar=a.TENANTAGR_Aadhar,
                                        TENANTAGR_UserId = a.TENANTAGR_UserId,
                                       TENANTAGR_RefundedAmount = a.TENANTAGR_RefundedAmount,
                                       TENANTAGR_RefundModeOfPayment = a.TENANTAGR_RefundModeOfPayment,
                                       TENANTAGR_Imagepath=a.TENANTAGR_Imagepath

                                    }).Distinct().ToArray();


             //  return PaymentRemainder(data);

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TenantAgreementDTO SaveMasterType(TenantAgreementDTO data)
        {
            try
            {
                data.returnmsg = "";
               var userid = 0;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.TENANTAGR_Id > 0)
                {
                    var dupl = _context.TenantAgreement.Where(a => a.HRMR_Id == data.HRMR_Id && a.TENANTAGR_LeftFlg == false && a.TENANTAGR_AgrToDate >= data.TENANTAGR_AgrFromDate && a.TENANTAGR_Id!= data.TENANTAGR_Id).ToList();

                    if (dupl.Count > 0)
                    {
                        data.returnmsg = dupl[0].TENANTAGR_TenantName;
                    }
                    else
                    {
                        var resultduplicate = _context.TenantAgreement.Where(a => a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id && a.HRMR_Id == data.HRMR_Id
                        && a.TENANTAGR_Id != data.TENANTAGR_Id && a.TENANTAGR_TenantName == data.TENANTAGR_TenantName).ToList();

                        if (resultduplicate.Count > 0)
                        {
                            data.message = "Duplicate";
                        }
                        else
                        {
                            // COMMENTED FOR FILES
                            List<long> filesid = new List<long>();

                            foreach (var c in data.tempfilesDTO)
                            {
                                filesid.Add(c.TENANTAGRFL_Id);
                            }

                            Array previous_Noresultremove = _context.TenantAgreementFilesDMO.Where(t => !filesid.Contains(t.TENANTAGRFL_Id)
                            && t.TENANTAGRFL_Id == data.TENANTAGRFL_Id).ToArray();

                            foreach (TenantAgreementFiles ph1 in previous_Noresultremove)
                            {
                                _context.Remove(ph1);
                            }
                            // COMMENTED FOR FILES

                            var result = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.MI_Id == data.MI_Id);

                            result.TENANTAGR_Address = data.TENANTAGR_Address;
                            result.TENANTAGR_AdvanceAmount = data.TENANTAGR_AdvanceAmount;
                            result.TENANTAGR_AgreementDuration = data.TENANTAGR_AgreementDuration;
                            result.TENANTAGR_AgrFromDate = data.TENANTAGR_AgrFromDate;
                            result.TENANTAGR_AgrToDate = data.TENANTAGR_AgrToDate;
                            result.TENANTAGR_ContactNo = data.TENANTAGR_ContactNo;
                            result.TENANTAGR_CreatedBy = data.UserId;
                            result.TENANTAGR_CreatedDate = DateTime.Now;
                            result.TENANTAGR_UpdatedBy = data.UserId;
                            result.TENANTAGR_UpdatedDate = DateTime.Now;
                            result.TENANTAGR_EmailId = data.TENANTAGR_EmailId;
                            result.TENANTAGR_PANCardNo = data.TENANTAGR_PANCardNo;
                            result.TENANTAGR_RenewalPercentage = data.TENANTAGR_RenewalPercentage;
                            result.TENANTAGR_BySource = data.PAMS_Id;
                            result.TENANTAGR_MonthlyRent = data.TENANTAGR_MonthlyRent;
                            result.TENANTAGR_NoOfOccupants = data.TENANTAGR_NoOfOccupants;
                            result.TENANTAGR_Remarks = data.TENANTAGR_Remarks;
                            result.TENANTAGR_RenewedFlag = data.TENANTAGR_RenewedFlag;
                            result.TENANTAGR_RentDueDay = data.TENANTAGR_RentDueDay;
                           // result.TENANTAGR_RentDueMonth = data.TENANTAGR_RentDueMonth;
                            result.TENANTAGR_TenantName = data.TENANTAGR_TenantName;
                            result.TENANTAGR_ActiveFlg = true;
                            result.TENANTAGR_LeftFlg = data.TENANTAGR_LeftFlg;
                            result.TENANTAGR_LeftRemarks = data.TENANTAGR_LeftRemarks;

                           result.TENANTAGR_Imagepath = data.TENANTAGR_Imagepath;
                           result.TENANTAGR_Aadhar = data.TENANTAGR_Aadhar;
                            result.HRMR_Id = data.HRMR_Id;
                            result.HRMB_Id = data.HRMB_Id; 
                           // result.TENANTAGR_UserId = data.UserId;
                           

                            result.TENANTAGR_UpdatedDate = DateTime.Now;
                            result.TENANTAGR_Id = data.TENANTAGR_Id;
                            _context.Update(result);

                            var filecnt = _context.TenantAgreementFilesDMO.Where(t => t.TENANTAGRFL_PhotoFlg == true && t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();

                            if (filecnt.Count > 0)
                            {

                                var files = _context.TenantAgreementFilesDMO.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTAGRFL_Id==filecnt[0].TENANTAGRFL_Id);

                                files.TENANTAGRFL_FileLocation = data.TENANTAGR_Imagepath;
                                files.TENANTAGRFL_PhotoFlg = true;
                                files.TENANTAGR_Id = data.TENANTAGR_Id;
                                files.TENANTAGRFL_CreatedBy = data.UserId;
                                files.TENANTAGRFL_CreatedDate = DateTime.Now;
                                files.TENANTAGRFL_UpdatedBy = data.UserId;
                                files.TENANTAGRFL_UpdatedDate = DateTime.Now;
                                files.TENANTAGRFL_Id = files.TENANTAGRFL_Id;
                                _context.Update(files);
                            }
                            else
                            {

                                if (data.TENANTAGR_Imagepath != null )
                                {
                                    TenantAgreementFiles resultfile = new TenantAgreementFiles();
                                    resultfile.TENANTAGRFL_FileName = "Photo";
                                    resultfile.TENANTAGRFL_FileLocation = data.TENANTAGR_Imagepath;
                                   // resultfile.TENANTAGRFL_Remarks = c.TENANTAGRFL_Remarks;
                                    resultfile.TENANTAGRFL_CreatedBy = data.UserId;
                                    resultfile.TENANTAGRFL_CreatedDate = DateTime.Now;
                                    resultfile.TENANTAGRFL_UpdatedBy = data.UserId;
                                    resultfile.TENANTAGRFL_UpdatedDate = DateTime.Now;
                                    resultfile.TENANTAGR_Id = data.TENANTAGR_Id;
                                    resultfile.TENANTAGRFL_PhotoFlg = true;


                                    _context.Add(resultfile);
                                }
                            }

                          

                            //COMMENTED FOR FILES
                            if (data.tempfilesDTO.Length > 0)
                            {
                                foreach (var c in data.tempfilesDTO)
                                {
                                    if (c.TENANTAGRFL_Id > 0)
                                    {
                                        if (c.TENANTAGRFL_FileName != null && c.TENANTAGRFL_FileName != "")
                                        {
                                            var resultfiles = _context.TenantAgreementFilesDMO.Single(a => a.TENANTAGRFL_Id == c.TENANTAGRFL_Id
                                            && a.TENANTAGR_Id == data.TENANTAGR_Id);
                                            resultfiles.TENANTAGRFL_FileName = c.TENANTAGRFL_FileName;
                                            resultfiles.TENANTAGRFL_FileLocation = c.TENANTAGRFL_FileLocation;
                                            resultfiles.TENANTAGRFL_Remarks = c.TENANTAGRFL_Remarks;
                                            resultfiles.TENANTAGRFL_CreatedBy = data.UserId;
                                            resultfiles.TENANTAGRFL_CreatedDate = DateTime.Now;
                                            resultfiles.TENANTAGRFL_UpdatedBy = data.UserId;
                                            resultfiles.TENANTAGRFL_UpdatedDate = DateTime.Now;
                                            resultfiles.TENANTAGR_Id = data.TENANTAGR_Id;
                                            resultfiles.TENANTAGRFL_PhotoFlg = false;

                                            _context.Update(resultfiles);
                                        }
                                    }
                                    else
                                    {
                                        if (c.TENANTAGRFL_FileName != null && c.TENANTAGRFL_FileName != "")
                                        {
                                            TenantAgreementFiles resultfile = new TenantAgreementFiles();
                                            resultfile.TENANTAGRFL_FileName = c.TENANTAGRFL_FileName;
                                            resultfile.TENANTAGRFL_FileLocation = c.TENANTAGRFL_FileLocation;
                                            resultfile.TENANTAGRFL_Remarks = c.TENANTAGRFL_Remarks;
                                            resultfile.TENANTAGRFL_CreatedBy = data.UserId;
                                            resultfile.TENANTAGRFL_CreatedDate = DateTime.Now;
                                            resultfile.TENANTAGRFL_UpdatedBy = data.UserId;
                                            resultfile.TENANTAGRFL_UpdatedDate = DateTime.Now;
                                            resultfile.TENANTAGR_Id = data.TENANTAGR_Id;
                                            resultfile.TENANTAGRFL_PhotoFlg = false;


                                            _context.Add(resultfile);
                                        }
                                    }
                                }
                            }


                            var contactExists = _context.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;
                                data.message = "Update";
                            }
                            else
                            {
                                data.returnval = false;
                                data.message = "Update";
                            }
                        }
                    }
                }
                else
                {
                    var dupl = _context.TenantAgreement.Where(a => a.HRMR_Id == data.HRMR_Id && a.TENANTAGR_LeftFlg == false && ( a.TENANTAGR_AgrFromDate <= data.TENANTAGR_AgrFromDate || a.TENANTAGR_AgrToDate>=data.TENANTAGR_AgrFromDate)).ToList();

                    if (dupl.Count > 0)
                    {
                        data.returnmsg = dupl[0].TENANTAGR_TenantName;
                    }
                    else
                    {


                        var resultduplicate = _context.TenantAgreement.Where(a => a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id && a.HRMR_Id == data.HRMR_Id
               && a.TENANTAGR_Id != data.TENANTAGR_Id && a.TENANTAGR_TenantName == data.TENANTAGR_TenantName).ToList();

                        if (resultduplicate.Count > 0)
                        {
                            data.message = "Duplicate";
                        }
                        else
                        {

                            long count = 0;
                            using (var transaction = _context.Database.BeginTransaction())
                            {


                                TenantAgreement result = new TenantAgreement();
                                result.TENANTAGR_Address = data.TENANTAGR_Address;
                                result.TENANTAGR_AdvanceAmount = data.TENANTAGR_AdvanceAmount;
                                result.TENANTAGR_AgreementDuration = data.TENANTAGR_AgreementDuration;
                                result.TENANTAGR_AgrFromDate = data.TENANTAGR_AgrFromDate;
                                result.TENANTAGR_AgrToDate = data.TENANTAGR_AgrToDate;
                                result.TENANTAGR_ContactNo = data.TENANTAGR_ContactNo;
                                result.TENANTAGR_CreatedBy = data.UserId;
                                result.TENANTAGR_CreatedDate = DateTime.Now;
                                result.TENANTAGR_UpdatedBy = data.UserId;
                                result.TENANTAGR_UpdatedDate = DateTime.Now;
                                result.TENANTAGR_EmailId = data.TENANTAGR_EmailId;
                                result.TENANTAGR_PANCardNo = data.TENANTAGR_PANCardNo;
                                result.TENANTAGR_RenewalPercentage = data.TENANTAGR_RenewalPercentage;
                                result.TENANTAGR_MonthlyRent = data.TENANTAGR_MonthlyRent;
                                result.TENANTAGR_NoOfOccupants = data.TENANTAGR_NoOfOccupants;
                                result.TENANTAGR_Remarks = data.TENANTAGR_Remarks;
                                result.TENANTAGR_RenewedFlag = data.TENANTAGR_RenewedFlag;
                                result.TENANTAGR_RentDueDay = data.TENANTAGR_RentDueDay;
                               // result.TENANTAGR_RentDueMonth = data.TENANTAGR_RentDueMonth;
                                result.TENANTAGR_TenantName = data.TENANTAGR_TenantName;
                                result.TENANTAGR_UserId = userid;
                                result.MI_Id = data.MI_Id;
                                result.HRMB_Id = data.HRMB_Id;
                                result.HRMR_Id = data.HRMR_Id;
                                result.TENANTAGR_ActiveFlg = true;
                                result.TENANTAGR_LeftFlg = data.TENANTAGR_LeftFlg;
                                result.TENANTAGR_LeftRemarks = data.TENANTAGR_LeftRemarks;
                                result.TENANTAGR_BySource = data.PAMS_Id;
                               
                                result.TENANTAGR_Aadhar = data.TENANTAGR_Aadhar;
                                result.TENANTAGR_Imagepath = data.TENANTAGR_Imagepath;
                                result.TENANTAGR_UpdatedDate = DateTime.Now;
                                result.TENANTAGR_Id = result.TENANTAGR_Id;

                                _context.Add(result);

                                if ( data.TENANTAGR_Imagepath != null && data.TENANTAGR_Imagepath != "")
                                {
                                    TenantAgreementFiles files = new TenantAgreementFiles();
                                    files.TENANTAGRFL_FileLocation = data.TENANTAGR_Imagepath;
                                    files.TENANTAGRFL_PhotoFlg = true;
                                    files.TENANTAGR_Id = result.TENANTAGR_Id;
                                    files.TENANTAGRFL_CreatedBy = data.UserId;
                                    files.TENANTAGRFL_CreatedDate = DateTime.Now;
                                    files.TENANTAGRFL_UpdatedBy = data.UserId;
                                    files.TENANTAGRFL_UpdatedDate = DateTime.Now;
                                    _context.Add(files);
                                }


                                //COMMENTED FOR FILES
                                if (data.tempfilesDTO.Length > 0)
                                {
                                    foreach (var c in data.tempfilesDTO)
                                    {
                                        if (c.TENANTAGRFL_FileName != null && c.TENANTAGRFL_FileName != "")
                                        {
                                            TenantAgreementFiles resultfile = new TenantAgreementFiles();
                                            resultfile.TENANTAGRFL_FileName = c.TENANTAGRFL_FileName;
                                            resultfile.TENANTAGRFL_FileLocation = c.TENANTAGRFL_FileLocation;
                                            resultfile.TENANTAGRFL_Remarks = c.TENANTAGRFL_Remarks;
                                            resultfile.TENANTAGRFL_CreatedBy = data.UserId;
                                            resultfile.TENANTAGRFL_CreatedDate = DateTime.Now;
                                            resultfile.TENANTAGRFL_UpdatedBy = data.UserId;
                                            resultfile.TENANTAGRFL_UpdatedDate = DateTime.Now;
                                            resultfile.TENANTAGR_Id = result.TENANTAGR_Id;
                                            resultfile.TENANTAGRFL_PhotoFlg = true; 


                                            _context.Add(resultfile);
                                        }
                                    }
                                }
                                //COMMENTED FOR FILES
                                TenantAgreementRenewal result2 = new TenantAgreementRenewal();

                                result2.TENANTAGRREN_Remarks = data.TENANTAGRREN_Remarks;
                                result2.TENANTAGRREN_PreAgrFromDate = data.TENANTAGR_AgrFromDate;
                                result2.TENANTAGRREN_PreAgrToDate = data.TENANTAGR_AgrToDate;
                                result2.TENANTAGRREN_Rent = Convert.ToInt64(data.TENANTAGR_MonthlyRent);

                                result2.TENANTAGRREN_CreatedBy = data.UserId;
                                result2.TENANTAGRREN_CreatedDate = DateTime.Now;

                                result2.TENANTAGRREN_UpdatedBy = data.UserId;
                                result2.TENANTAGRREN_ActiveFlag = true;

                                result2.TENANTAGRREN_UpdatedDate = DateTime.Now;
                                result2.TENANTAGR_Id = result.TENANTAGR_Id;
                                

                                _context.Add(result2);
                            }
                            var i = _context.SaveChanges();



                            data.getloaddata = (from a in _context.TenantAgreement
                                                from b in _context.HR_Master_Building
                                                from c in _context.HR_Master_Room
                                                where (a.HRMB_Id == b.HRMB_Id && a.HRMR_Id == c.HRMR_Id && a.MI_Id == data.MI_Id)
                                                select new TenantAgreementDTO
                                                {
                                                    TENANTAGR_Address = a.TENANTAGR_Address,
                                                    TENANTAGR_AdvanceAmount = a.TENANTAGR_AdvanceAmount,
                                                    TENANTAGR_AgreementDuration = a.TENANTAGR_AgreementDuration,
                                                    TENANTAGR_Id = a.TENANTAGR_Id,
                                                    TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                                    TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                                    TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                                    TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                                    TENANTAGR_PANCardNo = a.TENANTAGR_PANCardNo,
                                                    TENANTAGR_RenewalPercentage = a.TENANTAGR_RenewalPercentage,
                                                    PAMS_Id = a.TENANTAGR_BySource,
                                               
                                                    TENANTAGR_MonthlyRent = a.TENANTAGR_MonthlyRent,
                                                    TENANTAGR_NoOfOccupants = a.TENANTAGR_NoOfOccupants,
                                                    TENANTAGR_Remarks = a.TENANTAGR_Remarks,
                                                    TENANTAGR_RenewedFlag = a.TENANTAGR_RenewedFlag,
                                                    TENANTAGR_RentDueDay = a.TENANTAGR_RentDueDay,
                                                    //TENANTAGR_RentDueMonth = a.TENANTAGR_RentDueMonth,
                                                    TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                                    HRMB_BuildingName = b.HRMB_BuildingName,
                                                    HRMR_RoomName = c.HRMR_RoomName,
                                                    TENANTAGR_UserId=a.TENANTAGR_UserId,
                                                    count = _context.TenantAgreementFilesDMO.Where(d => a.TENANTAGR_Id == d.TENANTAGR_Id).Count()

                                                }).Distinct().ToArray();







                            if (i > 0)
                            {
                                data.returnval = true;
                              data.message = "Add";
                              //  var password = "Password@123";

                              //  sendmailToManagement("TENANTUSERMAIL", data.TENANTAGR_TenantName, password, data.TENANTAGR_EmailId, data.TENANTAGR_ContactNo, data.MI_Id);

                            }
                            else
                            {
                                data.returnval = false;
                                data.message = "Add";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TenantAgreementDTO EditMasterType(TenantAgreementDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TenantAgreementDTO MastetTypeActiveDeactive(TenantAgreementDTO data)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public TenantAgreementDTO OnClickMasterQuestions(TenantAgreementDTO data)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TenantAgreementDTO SaveMasterQuestion(TenantAgreementDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TenantAgreementDTO EditMasterQuestions(TenantAgreementDTO data)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TenantAgreementDTO MasteQuestionActiveDeactive(TenantAgreementDTO data)
        {

            return data;
        }
        public TenantAgreementDTO UpdateMasterQuestionOrder(TenantAgreementDTO data)
        {

            return data;
        }

        //Master Option
        public TenantAgreementDTO OnClickMasterOption(TenantAgreementDTO data)
        {

            return data;
        }
        public TenantAgreementDTO SaveMasterOption(TenantAgreementDTO data)
        {
            return data;
        }
        public TenantAgreementDTO EditMasterOption(TenantAgreementDTO data)
        {

            return data;
        }
        public TenantAgreementDTO MasteOptionActiveDeactive(TenantAgreementDTO data)
        {

            return data;
        }
        public TenantAgreementDTO UpdateMasterOptionOrder(TenantAgreementDTO data)
        {

            return data;
        }

        //Type Question Mapping
        public TenantAgreementDTO OnClickTypeQuestionMapping(TenantAgreementDTO data)
        {

            return data;
        }
        public TenantAgreementDTO OnChangeMasteTypeQuesInsitution(TenantAgreementDTO data)
        {

            return data;
        }
     
        public TenantAgreementDTO PaymentRemainder(TenantAgreementDTO data)        {            List<TenantAgreementDTO> devicelist = new List<TenantAgreementDTO>();            List<TenantAgreementDTO> deviceidsnew = new List<TenantAgreementDTO>();            try            {                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "RenewalRemainder";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",                  SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@TYPE",               SqlDbType.BigInt)                    {                        Value = 2                    });                    cmd.Parameters.Add(new SqlParameter("@alertdays",                  SqlDbType.BigInt)                    {                        Value = 5                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                devicelist.Add(new TenantAgreementDTO                                {                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"]),                                    TENANTAGR_EmailId = Convert.ToString(dataReader["TENANTAGR_EmailId"]),                                    TENANTAGR_Id = Convert.ToInt64(dataReader["TENANTAGR_Id"]),

                                });                            }                        }                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }                int k = 0;                foreach (var deviceid in devicelist)                {                    string m = sendmail(deviceid.MI_Id, "PAYMENTREMAINDER", 1, deviceid.TENANTAGR_EmailId, deviceid.TENANTAGR_Id);                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return data;        }        public TenantAgreementDTO RenewalRemainder(TenantAgreementDTO data)        {            List<TenantAgreementDTO> devicelist = new List<TenantAgreementDTO>();            List<TenantAgreementDTO> deviceidsnew = new List<TenantAgreementDTO>();            try            {                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "RenewalRemainder";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",                  SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@TYPE",                 SqlDbType.BigInt)                    {                        Value = 1                    });                    cmd.Parameters.Add(new SqlParameter("@alertdays",                SqlDbType.BigInt)                    {                        Value = 5                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                devicelist.Add(new TenantAgreementDTO                                {                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"]),                                    TENANTAGR_EmailId = Convert.ToString(dataReader["TENANTAGR_EmailId"]),                                    TENANTAGR_Id = Convert.ToInt64(dataReader["TENANTAGR_Id"]),                                });                            }                        }                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }                int k = 0;                foreach (var deviceid in devicelist)                {                    string m = sendmail(deviceid.MI_Id, "TENANTRENEWAL", 2, deviceid.TENANTAGR_EmailId, deviceid.TENANTAGR_Id);                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return data;        }
        public TenantAgreementDTO OverDueRemainder(TenantAgreementDTO data)        {
   

            List<TenantAgreementDTO> devicelist = new List<TenantAgreementDTO>();            List<TenantAgreementDTO> deviceidsnew = new List<TenantAgreementDTO>();            try            {                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "OVERDUEREMAINDER";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",                  SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                                     if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                devicelist.Add(new TenantAgreementDTO                                {                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"]),                                    TENANTAGR_EmailId = Convert.ToString(dataReader["TENANTAGR_EmailId"]),                                    TENANTAGR_Id = Convert.ToInt64(dataReader["TENANTAGR_Id"]),                                });                            }                        }                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }                int k = 0;                foreach (var deviceid in devicelist)                {                    string m = sendmail(deviceid.MI_Id, "OVERDUEREMAINDER", 3, deviceid.TENANTAGR_EmailId, deviceid.TENANTAGR_Id);                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return data;        }


        public string sendmail(long MI_Id, string Template, long type, string Email, long TENANTID)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();

                var Paramaeters = _context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();


                string Resultsms = Mailcontent;
                string result = Mailmsg;

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "REMAINDERS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id",
                  SqlDbType.VarChar)
                    {
                        Value = TENANTID
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                 SqlDbType.VarChar)
                    {
                        Value = type
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

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();

                    var message = new SendGridMessage();
                    string mailcc = "";
                    string mailbcc = "";

                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    //message.AddTo("hemanth@unnathimarketing.com");
                    message.AddTo(Email)

;
                    //**************** EMAIL CC DETAILS *************//

                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }

                    //**************** EMAIL BCC DETAILS *************//

                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                    {
                        string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                        foreach (var d in ccmaildetails)
                        {
                            message.AddCc(d);
                        }
                    }

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    try
                    {
                        client.SendEmailAsync(message).Wait();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }





                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Value = Mailcontent
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";


        }
    
        public TenantAgreementDTO Editrenewal(TenantAgreementDTO data)
        {
            data.edittenantrenewal=(from a in _context.TenantAgreement
                              from b in _context.TenantAgreementRenewalDMO
                              where (a.TENANTAGR_Id == b.TENANTAGR_Id && a.TENANTAGR_Id==data.TENANTAGR_Id && b.TENANTAGRREN_Id==data.TENANTAGRREN_Id)
                              select new TenantAgreementDTO
                              {
                                  TENANTAGR_Id = a.TENANTAGR_Id,
                                  TENANTAGRREN_Id = b.TENANTAGRREN_Id,
                                  TENANTAGRREN_PreAgrFromDate = b.TENANTAGRREN_PreAgrFromDate,
                                  TENANTAGRREN_Remarks = b.TENANTAGRREN_Remarks,
                                  TENANTAGRREN_PreAgrToDate = b.TENANTAGRREN_PreAgrToDate,
                                  TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                  TENANTAGRREN_AmtOrPercentFlg = b.TENANTAGRREN_AmtOrPercentFlg,
                                  TENANTAGRREN_AggreedEnhancement = Convert.ToInt32(b.TENANTAGRREN_AggreedEnhancement),
                                  TENANTAGRREN_Rent = b.TENANTAGRREN_Rent
                              }
                                    ).Distinct().ToArray();
            return data;
        }
        public TenantAgreementDTO getrefundableadvance(TenantAgreementDTO data)
        {

            data.amenitiescharges = (from a in _context.TenantMasterNocChecklistDMO
                                     from b in _context.HR_Master_Amenities_DMO
                                     where (a.HRMAM_Id == b.HRMAM_Id && a.TENANTAGR_Id == data.TENANTAGR_Id && a.TENANTNOCCL_ActiveFlag == true)
                                     select new TenantAgreementDTO
                                     {
                                         HRMAM_AmenitiesName = b.HRMAM_AmenitiesName,
                                         TENANTNOCCL_DeductingAmount = a.TENANTNOCCL_DeductingAmount,
                                         TENANTEBWB_Remarks = a.TENANTNOCCL_Remarks
                                     }
                                     ).ToArray();

            data.advanceamount = (from num in _context.TenantAgreement
                                  where num.TENANTAGR_Id == data.TENANTAGR_Id
                                  select num.TENANTAGR_AdvanceAmount).FirstOrDefault();

            data.deductingamount = (from num in _context.TenantMasterNocChecklistDMO
                                   where num.TENANTAGR_Id == data.TENANTAGR_Id && num.TENANTNOCCL_DeductingAmount > 0 && num.TENANTNOCCL_ActiveFlag == true
                                    select num.TENANTNOCCL_DeductingAmount).Sum();

            return data;
        }

            public TenantAgreementDTO Deleterenewal(TenantAgreementDTO data)
        {
            try
            {
                var renewaldelete = _context.TenantAgreementRenewalDMO.Where(t => t.TENANTAGRREN_Id == data.TENANTAGRREN_Id).ToList();
                if (renewaldelete.Count > 0)
                {
                    foreach (var fl in renewaldelete)
                    {
                        _context.Remove(fl);


                    }
                }
                var renewalupdate = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id);
                renewalupdate.TENANTAGR_RenewedFlag = false;
                renewalupdate.TENANTAGR_UpdatedBy = data.UserId;
                renewalupdate.TENANTAGR_UpdatedDate = DateTime.Now;

                _context.Update(renewalupdate);
                var contactExists = _context.SaveChanges();

                if (contactExists > 0)
                {
                    data.message = "Deleted";

                }
                else
                {
                    data.message = "Not Deleted";
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }


            return data;
        }
        public TenantAgreementDTO tenantchange(TenantAgreementDTO data)
        {

            data.tenantchange = _context.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToArray();


            return data;
        }
        public TenantAgreementDTO saverenewal(TenantAgreementDTO data)
        {
            try
            {


                if (data.TENANTAGRREN_Id > 0)
                {
                    var resultduplicate = _context.TenantAgreementRenewalDMO.Where(a => a.TENANTAGR_Id == data.TENANTAGR_Id && a.TENANTAGRREN_PreAgrToDate == data.TENANTAGRREN_PreAgrToDate
                    && a.TENANTAGRREN_Id != data.TENANTAGRREN_Id && a.TENANTAGRREN_PreAgrFromDate == data.TENANTAGRREN_PreAgrFromDate && a.TENANTAGRREN_Remarks==data.TENANTAGRREN_Remarks ).ToList();

                    if (resultduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {


                        var result = _context.TenantAgreementRenewalDMO.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTAGRREN_Id == data.TENANTAGRREN_Id);
                        result.TENANTAGRREN_Remarks = data.TENANTAGRREN_Remarks;
                       
                        result.TENANTAGRREN_PreAgrFromDate = data.TENANTAGRREN_PreAgrFromDate;
                        result.TENANTAGRREN_PreAgrToDate = data.TENANTAGRREN_PreAgrToDate;
                        result.TENANTAGRREN_Id = data.TENANTAGRREN_Id;
                        result.TENANTAGRREN_ActiveFlag = true;
                        //result.TENANTAGRREN_CreatedBy = data.UserId;
                        //result.TENANTAGRREN_CreatedDate = DateTime.Now;

                        result.TENANTAGRREN_UpdatedBy = data.UserId;


                        result.TENANTAGRREN_UpdatedDate = DateTime.Now;
                        result.TENANTAGR_Id = data.TENANTAGR_Id;
                        result.TENANTAGRREN_AggreedEnhancement = Convert.ToInt32(data.TENANTAGRREN_AggreedEnhancement);
                        result.TENANTAGRREN_AmtOrPercentFlg = data.TENANTAGRREN_AmtOrPercentFlg;
                        result.TENANTAGRREN_Rent = data.TENANTAGRREN_Rent;



                        _context.Update(result);

                        var resultval = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id);
                        resultval.TENANTAGR_RenewedFlag = true;
                        resultval.TENANTAGR_UpdatedBy = data.UserId;
                        resultval.TENANTAGR_UpdatedDate = DateTime.Now;
                        resultval.TENANTAGR_AgrFromDate = data.TENANTAGR_AgrFromDate;
                        resultval.TENANTAGR_AgrToDate = data.TENANTAGR_AgrToDate;
                        resultval.TENANTAGR_AgreementDuration = data.TENANTAGR_AgreementDuration;
                        resultval.TENANTAGR_MonthlyRent = data.TENANTAGRREN_Rent;
                        _context.Update(resultval);


                        var contactExists = _context.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {

                    var resultduplicate = _context.TenantAgreementRenewalDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTAGRREN_PreAgrFromDate==data.TENANTAGRREN_PreAgrFromDate && t.TENANTAGRREN_PreAgrToDate==data.TENANTAGRREN_PreAgrToDate).ToList();



                    if (resultduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        TenantAgreementRenewal result = new TenantAgreementRenewal();
                        result.TENANTAGRREN_Remarks = data.TENANTAGRREN_Remarks;
                        result.TENANTAGRREN_PreAgrFromDate = data.TENANTAGRREN_PreAgrFromDate;
                        result.TENANTAGRREN_PreAgrToDate = data.TENANTAGRREN_PreAgrToDate;
                        result.TENANTAGRREN_ActiveFlag = true;
                        result.TENANTAGRREN_Id = data.TENANTAGRREN_Id;

                        result.TENANTAGRREN_CreatedBy = data.UserId;
                        result.TENANTAGRREN_CreatedDate = DateTime.Now;

                        result.TENANTAGRREN_UpdatedBy = data.UserId;


                        result.TENANTAGRREN_UpdatedDate = DateTime.Now;
                        result.TENANTAGR_Id = data.TENANTAGR_Id;
                        result.TENANTAGRREN_AggreedEnhancement = Convert.ToInt32(data.TENANTAGRREN_AggreedEnhancement);
                        result.TENANTAGRREN_AmtOrPercentFlg = data.TENANTAGRREN_AmtOrPercentFlg;
                        result.TENANTAGRREN_Rent = data.TENANTAGRREN_Rent;

                        _context.Add(result);


                        var resultval = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id);
                        resultval.TENANTAGR_RenewedFlag = true;
                        resultval.TENANTAGR_AgrFromDate = data.TENANTAGR_AgrFromDate;
                        resultval.TENANTAGR_AgrToDate = data.TENANTAGR_AgrToDate;
                        resultval.TENANTAGR_UpdatedBy = data.UserId;
                        resultval.TENANTAGR_UpdatedDate = DateTime.Now;
                        resultval.TENANTAGR_AgreementDuration = data.TENANTAGR_AgreementDuration;
                        resultval.TENANTAGR_MonthlyRent = data.TENANTAGRREN_Rent;

                        _context.Update(resultval);


                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Add";
                        }
                    }
                    data.tenantrenewal = (from a in _context.TenantAgreement
                                          from b in _context.TenantAgreementRenewalDMO
                                          where (a.TENANTAGR_Id == b.TENANTAGR_Id)
                                          select new TenantAgreementDTO
                                          {
                                              TENANTAGR_Id = a.TENANTAGR_Id,
                                              TENANTAGRREN_Id = b.TENANTAGRREN_Id,
                                              TENANTAGRREN_PreAgrFromDate = b.TENANTAGRREN_PreAgrFromDate,
                                              TENANTAGRREN_Remarks = b.TENANTAGRREN_Remarks,
                                              TENANTAGRREN_PreAgrToDate = b.TENANTAGRREN_PreAgrToDate,
                                              TENANTAGR_TenantName = a.TENANTAGR_TenantName

                                          }
                                  ).Distinct().ToArray();


                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public TenantAgreementDTO saverefundadvance(TenantAgreementDTO data)
        {
            try
            {
                        var resultval = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id);
                resultval.TENANTAGR_RefundedAmount = data.TENANTAGR_RefundedAmount;
                resultval.TENANTAGR_RefundModeOfPayment = data.TENANTAGR_RefundModeOfPayment;

                _context.Update(resultval);


                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Add";
                        }

            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public TenantAgreementDTO delete(TenantAgreementDTO data)
        {
            try
            {
                var renewal = _context.TenantAgreementRenewalDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();

                if (renewal.Count == 0)
                {
                    var files = _context.TenantAgreementFilesDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();

                    if (files.Count > 0)
                    {
                        foreach (var fl in files)
                        {
                            _context.Remove(fl);


                        }
                        var tenant = _context.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();
                        if (tenant.Count > 0)
                        {
                            foreach (var f2 in tenant)
                            {
                                _context.Remove(f2);
                               

                            }

                        }


                    }
                    else
                    {
                        var tenant = _context.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();
                        if (tenant.Count > 0)
                        {
                            foreach (var fl in tenant)
                            {
                                _context.Remove(fl);
                              
                            }

                        }

                    }

                }
                else
                {
                    data.message = "Not Deleted";
                }
               var con= _context.SaveChanges();
                if (con > 0)
                {
                    data.message = "Deleted";
                }
                else
                {
                    data.message = "Not Deleted";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "Request Failed";
            }

            return data;
        }
        public TenantAgreementDTO Edit(TenantAgreementDTO data)
        {
            try
            {

                data.geteditdetails = (from a in _context.TenantAgreement
                                       from b in _context.HR_Master_Floor
                                       from c in _context.HR_Master_Room
                                       //from d in _context.TenantAgreementFilesDMO
                                       where (a.TENANTAGR_Id == data.TENANTAGR_Id && a.HRMB_Id == b.HRMB_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.HRMF_Id == c.HRMF_Id && c.HRMR_Id == a.HRMR_Id)
                                       select new TenantAgreementDTO
                                       {
                                           TENANTAGR_Address = a.TENANTAGR_Address,
                                           TENANTAGR_AdvanceAmount = a.TENANTAGR_AdvanceAmount,
                                           TENANTAGR_AgreementDuration = a.TENANTAGR_AgreementDuration,
                                           TENANTAGR_Id = a.TENANTAGR_Id,
                                           TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                           TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                           TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                           TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                           TENANTAGR_MonthlyRent = a.TENANTAGR_MonthlyRent,
                                           TENANTAGR_NoOfOccupants = a.TENANTAGR_NoOfOccupants,
                                           TENANTAGR_Remarks = a.TENANTAGR_Remarks,
                                           TENANTAGR_PANCardNo = a.TENANTAGR_PANCardNo,
                                           TENANTAGR_RenewalPercentage = a.TENANTAGR_RenewalPercentage,
                                           TENANTAGR_RenewedFlag = a.TENANTAGR_RenewedFlag,
                                           TENANTAGR_RentDueDay = a.TENANTAGR_RentDueDay,
                                           // TENANTAGR_RentDueMonth = a.TENANTAGR_RentDueMonth,
                                           TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                           HRMB_Id = a.HRMB_Id,
                                           HRMR_Id = a.HRMR_Id,
                                           HRMF_Id = b.HRMF_Id,
                                           PAMS_Id = a.TENANTAGR_BySource,
                                       
                                           TENANTAGR_LeftFlg =a.TENANTAGR_LeftFlg,
                                           TENANTAGR_LeftRemarks=a.TENANTAGR_LeftRemarks,
                                           TENANTAGR_Aadhar=a.TENANTAGR_Aadhar,
                                          // TENANTAGR_Imagepath=d.TENANTAGRFL_FileLocation

                                       }).Distinct().ToArray();

                string image_path = _context.TenantAgreementFilesDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTAGRFL_PhotoFlg == true).Select(s => s.TENANTAGRFL_FileLocation).FirstOrDefault();
                data.TENANTAGRFL_FileName = image_path;

                data.geteditfilesdetails = (from a in _context.TenantAgreementFilesDMO
                                            where (a.TENANTAGR_Id == data.TENANTAGR_Id && (a.TENANTAGRFL_PhotoFlg==false || a.TENANTAGRFL_PhotoFlg == null))
                                            select new TenantAgreementDTO
                                            {
                                                TENANTAGRFL_Id = a.TENANTAGRFL_Id,
                                                TENANTAGRFL_Remarks = a.TENANTAGRFL_Remarks,
                                                TENANTAGRFL_FileName = a.TENANTAGRFL_FileName,
                                                TENANTAGRFL_FileLocation = a.TENANTAGRFL_FileLocation,
                                                TENANTAGR_Id = a.TENANTAGR_Id,
                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public TenantAgreementDTO ViewFiles(TenantAgreementDTO data)
        {
            data.getviewfilesdetails = (from a in _context.TenantAgreementFilesDMO
                                        where (a.TENANTAGR_Id == data.TENANTAGR_Id)
                                        select new TenantAgreementDTO
                                        {
                                            TENANTAGR_Id = a.TENANTAGR_Id,
                                            TENANTAGRFL_Id = a.TENANTAGRFL_Id,
                                            TENANTAGRFL_Remarks = a.TENANTAGRFL_Remarks,
                                            TENANTAGRFL_FileName = a.TENANTAGRFL_FileName,
                                            TENANTAGRFL_FileLocation = a.TENANTAGRFL_FileLocation
                                        }).Distinct().ToArray();



            return data;
        }

        public TenantAgreementDTO TenantDetails(TenantAgreementDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantFullDetail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Id", SqlDbType.VarChar) { Value = data.TENANTAGR_Id });

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
                        data.TenantDetails = retObject.ToArray();
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


        


     
    public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
             data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id==data.HRMB_Id).ToArray();

            data.getfloordetails = _context.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id == data.HRMB_Id).ToArray();


            return data;
        }
        public TenantAgreementDTO roomchange(TenantAgreementDTO data)
        {
            data.getamenitiesdetails = (from a in _context.HR_Master_Amenities_DMO
                                        from b in _context.HR_Master_Room_AmenitiesDMO
                                        where (a.HRMAM_Id == b.HRMAM_Id && b.HRMR_Id==data.HRMR_Id && b.HRMRAM_ActiveFlag==true)
                                        select new TenantAgreementDTO
                                        {
                                            HRMAM_Id = a.HRMAM_Id,
                                            HRMAM_AmenitiesName = a.HRMAM_AmenitiesName,
                                            HRMRAM_RentPerDay = b.HRMRAM_RentPerDay,
                                            HRMRAM_NoOfHrs = b.HRMRAM_NoOfHrs,
                                            HRMRAM_RentPerHour=b.HRMRAM_RentPerHour,
                                            HRMRAM_Id=b.HRMRAM_Id

                                        }).Distinct().ToArray();

            return data;
        }


        public TenantAgreementDTO Floorchange(TenantAgreementDTO data)
        {
            data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id == data.HRMB_Id && t.HRMF_Id==data.HRMF_Id).ToArray();




            return data;
        }
        public TenantAgreementDTO CreateUser(TenantAgreementDTO data)
        {
            var tenantdet = _context.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).ToList();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var dublicatecheck = 0;
                     dublicatecheck = _context.ApplicationUser.Where(t =>  t.Id == tenantdet[0].TENANTAGR_UserId).Count();
                    if (dublicatecheck > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                        ApplUser user = new ApplUser();
                        user.AccessFailedCount = 0;
                        user.ConcurrencyStamp = "";
                        user.Email = tenantdet[0].TENANTAGR_EmailId;
                        user.EmailConfirmed = true;
                        user.LockoutEnabled = true;
                        user.NormalizedEmail = tenantdet[0].TENANTAGR_EmailId;
                        user.NormalizedUserName = tenantdet[0].TENANTAGR_TenantName.Trim();
                        user.PasswordHash = "AQAAAAEAACcQAAAAEPhnWI7McHZqXzg5vE1PUDo4WVzezsjaMX8ZBj1agl6az3F7JjxQtHp2VSIx7oYRYg==";
                        user.PhoneNumber = tenantdet[0].TENANTAGR_ContactNo;
                        user.PhoneNumberConfirmed = false;
                        user.SecurityStamp = "";
                        user.TwoFactorEnabled = false;
                        user.UserName = tenantdet[0].TENANTAGR_TenantName.Trim();
                        user.Entry_Date = DateTime.Now;
                        user.Machine_Ip_Address = null;
                        user.UserImagePath = null;
                        user.Entry_Date = DateTime.Now;
                        user.CreatedDate = DateTime.Now;
                        user.UpdatedDate = DateTime.Now;
                        user.RoleTypeFlag = "Tenant User";

                        user.Name = tenantdet[0].TENANTAGR_TenantName.Trim();
                        _context.Add(user);
                        var n = _context.SaveChanges();

                        if (n > 0)
                        {
                            var applicationrole = _context.ApplicationRole_con.Where(t => t.roleType == "Tenant User").ToList();
                            var roletype = _context.IVRM_Role_Type.Where(t => t.IVRMRT_RoleFlag == "Tenant User" && t.IVRMR_Id == applicationrole[0].Id).ToList();
                            DataAccessMsSqlServerProvider.ApplicationUserRole appuserrole = new DataAccessMsSqlServerProvider.ApplicationUserRole();
                            var userid = user.Id;
                            appuserrole.UserId = user.Id;
                            appuserrole.RoleId = applicationrole[0].Id;
                            appuserrole.RoleTypeId = roletype[0].IVRMRT_Id;
                            _context.Add(appuserrole);
                            _context.SaveChanges();


                            UserRoleWithInstituteDMO userrole = new UserRoleWithInstituteDMO();
                            userrole.MI_Id = tenantdet[0].MI_Id;
                            userrole.Id = user.Id;
                            userrole.Activeflag = 1;
                            userrole.CreatedDate = DateTime.Now;
                            userrole.UpdatedDate = DateTime.Now;

                            _context.Add(userrole);

                            var result = _context.TenantAgreement.Single(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.MI_Id == tenantdet[0].MI_Id);
                            result.TENANTAGR_UserId = user.Id;
                            _context.Update(result);


                            var count = _context.SaveChanges();
                            if (count > 0 )
                            {
                                  var password = "Password@123";
                                data.message = "Add";
                                var path = "https://vapsmanagementsoftware.azurewebsites.net/#/login/";
                                sendmailToManagement("CandidateUserMail", tenantdet[0].TENANTAGR_TenantName.Trim(), password, tenantdet[0].TENANTAGR_EmailId, tenantdet[0].TENANTAGR_ContactNo, path, tenantdet[0].MI_Id);
                            }
                            else
                            {
                                data.message = "False";
                            }
                        }
                        else
                        {
                            data.message = "Error";
                        }
                        transaction.Commit();
                    }
                    if(data.message == "Duplicate")
                    {
                        var username = _context.ApplicationUser.Where(t => t.Id == tenantdet[0].TENANTAGR_UserId).Select(t => t.UserName).FirstOrDefault();
                        var password = "Password@123  OR Password@1234";
                        var MI_Id = 17;
                        var path = "https://vapsmanagementsoftware.azurewebsites.net/#/login/";
                        sendmailToManagement("TENANTUSERNAMEANDPASSWORD", username, password, tenantdet[0].TENANTAGR_EmailId, tenantdet[0].TENANTAGR_ContactNo, path, MI_Id);
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                    data.message = "Error ";
                }

            }


                return data;
        }



        public string sendmailToManagement(string Template, string Username, string password, string email, string mobilenumber, string path, long mi_id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == 17).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                Mailmsg = Mailmsg.Replace("[USR]", Username);
                Mailmsg = Mailmsg.Replace("[PASSWORD]", password);
                Mailmsg = Mailmsg.Replace("[ADDRESS]", path);
                Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(17)).ToList();

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
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(17)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
                    {
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
                        message.AddTo(email)
;


                        if (Attechement.Equals("1"))
                        {
                            var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }




                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)
;
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                      

                    }


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.smsEmailSetting.Where(e => e.MI_Id == 17 && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = 17
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";


        }

    }
}