using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using DomainModel.Model.com.vapstech.Portals.Tenants;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Implimentation
{
    public class TenantAgreementReportImpl : Interface.TenantAgreementReportInterface
    {
        public IssueManagerContext _context;


        public TenantAgreementReportImpl(IssueManagerContext isc)
        {





            _context = isc;

        }

        public async Task<TenantAgreementDTO> getdetails(TenantAgreementDTO data)
        {
            try
            {




                data.building = _context.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.getfloordetails = _context.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id).ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<TenantAgreementDTO> get_report(TenantAgreementDTO data)
        {
            try
            {
                data.getloaddata = (from a in _context.TenantAgreement
                                    from b in _context.HR_Master_Building
                                    from c in _context.HR_Master_Room
                                    where (a.HRMB_Id == b.HRMB_Id && a.HRMR_Id == c.HRMR_Id && a.MI_Id == data.MI_Id && data.buldid.Contains(a.HRMB_Id) && data.roomid.Contains(a.HRMR_Id))
                                    
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
                                        //TENANTAGR_RentDueMonth = a.TENANTAGR_RentDueMonth,
                                        TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                        HRMB_BuildingName = b.HRMB_BuildingName,
                                        HRMR_RoomName = c.HRMR_RoomName,
                                      //  count = _context.TenantAgreementFilesDMO.Where(d => a.TENANTAGR_Id == d.TENANTAGR_Id).Count()

                                    }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
            data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id && data.buldid.Contains( t.HRMB_Id)).ToArray();

            data.getfloordetails = _context.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id && data.buldid.Contains(t.HRMB_Id)).ToArray();


            return data;
        }
  
        public TenantAgreementDTO Floorchange(TenantAgreementDTO data)
        {
            data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id == data.HRMB_Id && t.HRMF_Id == data.HRMF_Id && data.buldid.Contains(t.HRMB_Id) && data.floorid.Contains(t.HRMF_Id)).ToArray();




            return data;
        }


        public TenantAgreementDTO getallofflinedetails(TenantAgreementDTO data)
        {
            try
            {
                data.years = _context.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(data.MI_Id)).ToArray();
          

                data.tenant_array = (from a in _context.TenantAgreement
                                     from b in _context.HR_Master_Building

                                     where (a.MI_Id == data.MI_Id && a.TENANTAGR_ActiveFlg == true && a.HRMB_Id==b.HRMB_Id && a.TENANTAGR_LeftFlg == false)
                                     select new TenantAgreementDTO
                                     {
                                         TENANTAGR_Id = a.TENANTAGR_Id,
                                         TENANTAGR_TenantName = a.TENANTAGR_TenantName +" / "+b.HRMB_BuildingName

                                     }).ToArray();


                data.monthnames = _context.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();



                data.tenants = (from b in _context.TenantElectricalWaterBillDMO
                                from a in _context.TenantAgreement
                                from c in _context.HR_Master_Building
                                from d in _context.HR_Master_Floor
                                from e in _context.HR_Master_Room

                                where (a.MI_Id == data.MI_Id && b.TENANTAGR_Id == a.TENANTAGR_Id && a.TENANTAGR_ActiveFlg == true && a.HRMB_Id == c.HRMB_Id &&
                                c.HRMB_Id == d.HRMB_Id && d.HRMF_Id == e.HRMF_Id && a.HRMR_Id == e.HRMR_Id)
                                select new TenantAgreementDTO
                                {
                                    TENANTAGR_Id = a.TENANTAGR_Id,
                                    TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                    TENANTEBWB_Id = b.TENANTEBWB_Id,
                                    TENANTEBWB_Amount = b.TENANTEBWB_Amount,
                                    TENANTEBWB_ActiveFlag = b.TENANTEBWB_ActiveFlag,
                                    TENANTEBWB_Month = b.TENANTEBWB_Month,
                                    TENANTEBWB_Year = b.TENANTEBWB_Year,
                                    TENANTEBWB_Flg = b.TENANTEBWB_Flg,
                                    HRMB_BuildingName = c.HRMB_BuildingName,
                                    HRMR_RoomName = e.HRMR_RoomName,

                                }).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public TenantAgreementDTO SaveElectricalbill(TenantAgreementDTO data)
        {
            try
            {
                

                if (data.TENANTEBWB_Id> 0)
                {



                    var result = _context.TenantElectricalWaterBillDMO.Single(t => t.TENANTEBWB_Id == data.TENANTEBWB_Id);
                    var res0 = _context.TenantElectricalWaterBillDMO.Where(a => a.TENANTEBWB_Id == data.TENANTEBWB_Id).ToList();
                    if (res0.Count > 0)
                    {
                        var res01 = _context.TenantElectricalWaterBillDMO.Single(a => a.TENANTEBWB_Id == data.TENANTEBWB_Id);
                        res01.TENANTAGR_Id = data.TENANTAGR_Id;
                        res01.TENANTEBWB_Amount = data.TENANTEBWB_Amount;
                        res01.TENANTEBWB_Month = data.TENANTEBWB_Month;
                        res01.TENANTEBWB_Year = data.TENANTEBWB_Year;
                        res01.TENANTEBWB_CreatedBy = data.UserId;
                        res01.TENANTEBWB_UpdatedBy = data.UserId;
                        res01.TENANTEBWB_UpdatedDate = DateTime.Now;
                        res01.TENANTEBWB_CreatedDate = DateTime.Now;
                        res01.TENANTEBWB_Flg = data.TENANTEBWB_Flg;
                        res01.TENANTEBWB_Remarks = data.TENANTEBWB_Remarks;
                        res01.TENANTEBWB_ActiveFlag = true;
                      
                   
                        _context.Update(res01);
                    }
                    else
                    {
                        TenantElectricalWaterBillDMO uptaskclt = new TenantElectricalWaterBillDMO();
                        uptaskclt.TENANTAGR_Id = data.TENANTAGR_Id;
                        uptaskclt.TENANTEBWB_Amount = data.TENANTEBWB_Amount;
                        uptaskclt.TENANTEBWB_Month = data.TENANTEBWB_Month;
                        uptaskclt.TENANTEBWB_Year = data.TENANTEBWB_Year;
                        uptaskclt.TENANTEBWB_CreatedBy = data.UserId;
                        uptaskclt.TENANTEBWB_UpdatedBy = data.UserId;
                        uptaskclt.TENANTEBWB_UpdatedDate = DateTime.Now;
                        uptaskclt.TENANTEBWB_CreatedDate = DateTime.Now;
                        uptaskclt.TENANTEBWB_Flg = data.TENANTEBWB_Flg;
                        uptaskclt.TENANTEBWB_Remarks = data.TENANTEBWB_Remarks;
                        uptaskclt.TENANTEBWB_ActiveFlag = true;


                        _context.Add(uptaskclt);
                    }



                }

               
                else
                {
                    var result1 = _context.TenantElectricalWaterBillDMO.Where(t => t.TENANTEBWB_Amount == data.TENANTEBWB_Amount && t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTEBWB_Month == data.TENANTEBWB_Month && t.TENANTEBWB_Year == data.TENANTEBWB_Year).ToList();

                    if (result1.Count > 0)
                    {
                        data.returnmsg = "duplicate";
                    }
                    else
                    {


                        TenantElectricalWaterBillDMO result = new TenantElectricalWaterBillDMO();


                        result.TENANTAGR_Id = data.TENANTAGR_Id;
                        result.TENANTEBWB_Amount = data.TENANTEBWB_Amount;
                        result.TENANTEBWB_Month = data.TENANTEBWB_Month;
                        result.TENANTEBWB_Year = data.TENANTEBWB_Year;
                        result.TENANTEBWB_CreatedBy = data.UserId;
                        result.TENANTEBWB_UpdatedBy = data.UserId;
                        result.TENANTEBWB_UpdatedDate = DateTime.Now;
                        result.TENANTEBWB_CreatedDate = DateTime.Now;
                        result.TENANTEBWB_Flg = data.TENANTEBWB_Flg;
                        result.TENANTEBWB_Remarks = data.TENANTEBWB_Remarks;
                        result.TENANTEBWB_ActiveFlag = true;



                        _context.Add(result);


                    }

                   
                    


                   
                }

               
                    var contactExists = _context.SaveChanges();


                data.tenants = (from b in _context.TenantElectricalWaterBillDMO
                                from a in _context.TenantAgreement
                                from c in _context.HR_Master_Building
                                from d in _context.HR_Master_Floor
                                from e in _context.HR_Master_Room

                                where (a.MI_Id == data.MI_Id && b.TENANTAGR_Id == a.TENANTAGR_Id && a.TENANTAGR_ActiveFlg == true && a.HRMB_Id == c.HRMB_Id &&
                                c.HRMB_Id == d.HRMB_Id && d.HRMF_Id == e.HRMF_Id && a.HRMR_Id == e.HRMR_Id)
                                     select new TenantAgreementDTO
                                     {
                                         TENANTAGR_Id = a.TENANTAGR_Id,
                                         TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                         TENANTEBWB_Id = b.TENANTEBWB_Id,
                                         TENANTEBWB_Amount = b.TENANTEBWB_Amount,
                                         TENANTEBWB_ActiveFlag = b.TENANTEBWB_ActiveFlag,
                                         TENANTEBWB_Month = b.TENANTEBWB_Month,
                                         TENANTEBWB_Year = b.TENANTEBWB_Year,
                                         TENANTEBWB_Flg=b.TENANTEBWB_Flg,
                                         HRMB_BuildingName = c.HRMB_BuildingName,
                                         HRMR_RoomName = e.HRMR_RoomName,
                                     }).ToArray();


                if (contactExists > 0)
                    {
                        data.returnmsg = "true";
                    }
                    else
                    {
                        data.returnmsg = "false";
                    }
               
           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TenantAgreementDTO Saveallbilltype(TenantAgreementDTO data)
        {
            try
            {
                for(int i = 0; i < data.tenantbilltypearr.Length; i++)
                {
                    if(data.tenantbilltypearr[i].TENANTELECTRICAL_Amount > 0)
                    {
                        TenantElectricalWaterBillDMO electrical = new TenantElectricalWaterBillDMO();


                        electrical.TENANTAGR_Id = data.tenantbilltypearr[i].TENANTAGR_Id;
                        electrical.TENANTEBWB_Amount = data.tenantbilltypearr[i].TENANTELECTRICAL_Amount;
                        electrical.TENANTEBWB_Month = data.TENANTEBWB_Month;
                        electrical.TENANTEBWB_Year = data.TENANTEBWB_Year;
                        electrical.TENANTEBWB_CreatedBy = data.UserId;
                        electrical.TENANTEBWB_UpdatedBy = data.UserId;
                        electrical.TENANTEBWB_UpdatedDate = DateTime.Now;
                        electrical.TENANTEBWB_CreatedDate = DateTime.Now;
                        electrical.TENANTEBWB_Flg = data.TENANTEBWB_Flg;
                       // electrical.TENANTEBWB_Remarks = data.tenantbilltypearr[i].TENANTEBWB_Remarks;
                        electrical.TENANTEBWB_Flg = "Electrical";
                        electrical.TENANTEBWB_ActiveFlag = true;

                        _context.Add(electrical);

                    }
                    if (data.tenantbilltypearr[i].TENANTWATER_Amount > 0)
                    {
                        TenantElectricalWaterBillDMO water = new TenantElectricalWaterBillDMO();


                        water.TENANTAGR_Id = data.tenantbilltypearr[i].TENANTAGR_Id;
                        water.TENANTEBWB_Amount = data.tenantbilltypearr[i].TENANTWATER_Amount;
                        water.TENANTEBWB_Month = data.TENANTEBWB_Month;
                        water.TENANTEBWB_Year = data.TENANTEBWB_Year;
                        water.TENANTEBWB_CreatedBy = data.UserId;
                        water.TENANTEBWB_UpdatedBy = data.UserId;
                        water.TENANTEBWB_UpdatedDate = DateTime.Now;
                        water.TENANTEBWB_CreatedDate = DateTime.Now;
                        water.TENANTEBWB_Flg = data.TENANTEBWB_Flg;
                        // water.TENANTEBWB_Remarks = data.tenantbilltypearr[i].TENANTEBWB_Remarks;
                        water.TENANTEBWB_Flg = "Water";
                        water.TENANTEBWB_ActiveFlag = true;

                        _context.Add(water);

                    }
                    if (data.tenantbilltypearr[i].TENANTOTHER_Amount > 0)
                    {
                        TenantElectricalWaterBillDMO othercharges = new TenantElectricalWaterBillDMO();


                        othercharges.TENANTAGR_Id = data.tenantbilltypearr[i].TENANTAGR_Id;
                        othercharges.TENANTEBWB_Amount = data.tenantbilltypearr[i].TENANTOTHER_Amount;
                        othercharges.TENANTEBWB_Month = data.TENANTEBWB_Month;
                        othercharges.TENANTEBWB_Year = data.TENANTEBWB_Year;
                        othercharges.TENANTEBWB_CreatedBy = data.UserId;
                        othercharges.TENANTEBWB_UpdatedBy = data.UserId;
                        othercharges.TENANTEBWB_UpdatedDate = DateTime.Now;
                        othercharges.TENANTEBWB_CreatedDate = DateTime.Now;
                        othercharges.TENANTEBWB_Flg = data.TENANTEBWB_Flg;
                        othercharges.TENANTEBWB_Remarks = data.tenantbilltypearr[i].TENANTEBWB_Remarks;
                        othercharges.TENANTEBWB_Flg = "Other";
                        othercharges.TENANTEBWB_ActiveFlag = true;

                        _context.Add(othercharges);

                    }
                    var res = _context.SaveChanges();

                    if (res > 0)
                    {
                        data.returnmsg = "true";
                    }
                    else
                    {
                        data.returnmsg = "false";
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TenantAgreementDTO deactive(TenantAgreementDTO data)
        {
            try
            {
                var result = _context.TenantElectricalWaterBillDMO.Single(t => t.TENANTEBWB_Id == data.TENANTEBWB_Id);

                if (result.TENANTEBWB_ActiveFlag == true)
                {
                    result.TENANTEBWB_ActiveFlag = false;
                }
                else if (result.TENANTEBWB_ActiveFlag == false)
                {
                    result.TENANTEBWB_ActiveFlag = true;
                }
                result.TENANTEBWB_UpdatedDate = DateTime.Now;
                _context.Update(result);

                int returnval = _context.SaveChanges();
                if (returnval > 0)
                {
                    data.returnmsg = "true";
                }
                else
                {
                    data.returnmsg = "false";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public TenantAgreementDTO editdetails(TenantAgreementDTO data)
        {
            try
            {
                data.get_printdetails = _context.TenantElectricalWaterBillDMO.Where(t => t.TENANTEBWB_Id == data.TENANTEBWB_Id).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Fine Details 

        public TenantAgreementDTO loadtenants(TenantAgreementDTO data)
        {
            try
            {
               
                data.TenantDetails = (from a in _context.TenantAgreement
                                     from b in _context.HR_Master_Building

                                     where (a.MI_Id == data.MI_Id && a.TENANTAGR_ActiveFlg == true && a.HRMB_Id == b.HRMB_Id && a.TENANTAGR_LeftFlg == false)
                                     select new TenantAgreementDTO
                                     {
                                         TENANTAGR_Id = a.TENANTAGR_Id,
                                         TENANTAGR_TenantName = a.TENANTAGR_TenantName + " / " + b.HRMB_BuildingName

                                     }).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public TenantAgreementDTO getTenantAmountDetails(TenantAgreementDTO data)
        {
            try
            {
                data.getloaddata = _context.TenantAgreement.Where(a => a.TENANTAGR_Id == data.TENANTAGR_Id && a.MI_Id == data.MI_Id).ToArray();

                data.reference = (from a in _context.TenantPaymentDetailsDMO
                                              from b in _context.TenantAgreement
                                              where (a.TENANTAGR_Id == b.TENANTAGR_Id && a.TENANTAGR_Id == data.TENANTAGR_Id && a.TENANTAGRPAY_Flag == "Rent" && (a.TENANTAGRPAY_FinePaidAmount  == 0 || a.TENANTAGRPAY_FinePaidAmount == null))
                                              select a).OrderBy(a => a.TENANTAGRPAY_PaymentYear).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public TenantAgreementDTO Savefine(TenantAgreementDTO data)
        {
            try
            {
                for (int i = 0; i < data.tenantfineentryarr.Length; i++)
                {
                    if (data.tenantfineentryarr[i].TENANTAGRPAY_Id > 0)
                    {

                        var Result = _context.TenantPaymentDetailsDMO.Single(a => a.TENANTAGRPAY_Id == data.tenantfineentryarr[i].TENANTAGRPAY_Id && a.TENANTAGR_Id == data.tenantfineentryarr[i].TENANTAGR_Id && 
                        a.TENANTAGRPAY_PaymentMonth == data.tenantfineentryarr[i].TENANTAGRPAY_PaymentMonth && a.TENANTAGRPAY_PaymentYear == data.tenantfineentryarr[i].TENANTAGRPAY_PaymentYear);

                        Result.TENANTAGRPAY_ActualFineAmount = data.tenantfineentryarr[i].TENANTAGRPAY_ActualFineAmount;
                        Result.TENANTAGRPAY_UpdatedBy = data.UserId;
                        Result.TENANTAGRPAY_UpdatedDate = DateTime.Now;

                        _context.Update(Result);

                    }
                  
                   
                    var res = _context.SaveChanges();

                    if (res > 0)
                    {
                        data.returnmsg = "true";
                    }
                    else
                    {
                        data.returnmsg = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public TenantAgreementDTO sendmail(TenantAgreementDTO data)
        {
            try
            {
                foreach (var i in data.TENANTEBWB_Ids)
                {


                    var tenantdetails = (from b in _context.TenantElectricalWaterBillDMO
                                         from a in _context.TenantAgreement
                                         from c in _context.HR_Master_Building
                                         from d in _context.HR_Master_Floor
                                         from e in _context.HR_Master_Room
                                         from f in _context.IVRM_Month_DMO

                                         where (a.MI_Id == data.MI_Id && f.IVRM_Month_Id==b.TENANTEBWB_Month && b.TENANTAGR_Id == a.TENANTAGR_Id && a.TENANTAGR_ActiveFlg == true && b.TENANTEBWB_Id == i.TENANTEBWB_Id &&
                                         c.HRMB_Id == a.HRMB_Id && d.HRMB_Id == c.HRMB_Id && e.HRMF_Id == d.HRMF_Id &&  e.HRMR_Id == a.HRMR_Id)
                                         select new TenantAgreementDTO
                                         {
                                             TENANTAGR_Id = a.TENANTAGR_Id,
                                             TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                             TENANTEBWB_Id = b.TENANTEBWB_Id,
                                             TENANTEBWB_Amount = b.TENANTEBWB_Amount,
                                             TENANTEBWB_ActiveFlag = b.TENANTEBWB_ActiveFlag,
                                             IVRM_Month_Name = f.IVRM_Month_Name,
                                             TENANTEBWB_Year = b.TENANTEBWB_Year,
                                             TENANTEBWB_Flg = b.TENANTEBWB_Flg,
                                             TENANTAGR_EmailId=a.TENANTAGR_EmailId,
                                             HRMB_BuildingName = c.HRMB_BuildingName,
                                             HRMR_RoomName = e.HRMR_RoomName,
                                             TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                             TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                             TENANTAGR_Address = a.TENANTAGR_Address,

                                         }).ToArray();


                    string emailccdetails = "";
                    string emailbccdetails = "";


                    if (tenantdetails[0].TENANTEBWB_Flg == "Other")
                    {
                        var checktemplate = _context.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "TENANTCLEANINGCHARGES"
                        && a.ISES_MailActiveFlag == true).ToList();

                        DateTime now = DateTime.Now;
                        string strDate = now.ToString("dd-MM-yyyy");

                        if (checktemplate.Count > 0)
                        {
                            emailccdetails = checktemplate.FirstOrDefault().ISES_MailCCId;
                            emailbccdetails = checktemplate.FirstOrDefault().ISES_MailBCCId;
                        }
                        else
                        {
                            emailccdetails = "";
                            emailbccdetails = "";
                        }

                        if (checktemplate.Count == 0)
                        {
                            data.returnmsg = "Email Template not Mapped to the selected Institution";
                        }
                        else
                        {

                            string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                            if (templatedetails.Contains("[TENANTNAME]"))
                            {
                                templatedetails = templatedetails.Replace("[TENANTNAME]", tenantdetails[0].TENANTAGR_TenantName.ToUpper());
                            }
                            if (templatedetails.Contains("[FLAT]"))
                            {
                                templatedetails = templatedetails.Replace("[FLAT]", tenantdetails[0].HRMR_RoomName.ToString());
                            }
                            if (templatedetails.Contains("[BUILDING]"))
                            {
                                templatedetails = templatedetails.Replace("[BUILDING]", tenantdetails[0].HRMB_BuildingName.ToString());
                            }
                            if (templatedetails.Contains("[ADDRESS]"))
                            {
                                templatedetails = templatedetails.Replace("[ADDRESS]", tenantdetails[0].TENANTAGR_Address.ToString());
                            }
                            if (templatedetails.Contains("[AMOUNT]"))
                            {

                                templatedetails = templatedetails.Replace("[AMOUNT]", tenantdetails[0].TENANTEBWB_Amount.ToString());
                            }
                            if (templatedetails.Contains("[STARTDATE]"))
                            {
                                templatedetails = templatedetails.Replace("[STARTDATE]", tenantdetails[0].TENANTAGR_AgrFromDate.ToString("dd-MM-yyyy"));
                            }
                            //if (templatedetails.Contains("[DATE1]"))
                            //{

                            //    templatedetails = templatedetails.Replace("[DATE1]", strDate );
                            //}

                            try
                            {
                                data.returnmsg = sendmailexamtt(data.MI_Id, tenantdetails[0].TENANTAGR_EmailId.ToString(), templatedetails, emailccdetails, emailbccdetails, "TENANTCLEANINGCHARGES");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }

                    else if (tenantdetails[0].TENANTEBWB_Flg == "Water")
                    {
                        var checktemplate = _context.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "TENANTWATERBILLCHARGES"
                        && a.ISES_MailActiveFlag == true).ToList();

                        DateTime now = DateTime.Now;
                        string strDate = now.ToString("dd-MM-yyyy");

                        if (checktemplate.Count > 0)
                        {
                            emailccdetails = checktemplate.FirstOrDefault().ISES_MailCCId;
                            emailbccdetails = checktemplate.FirstOrDefault().ISES_MailBCCId;
                        }
                        else
                        {
                            emailccdetails = "";
                            emailbccdetails = "";
                        }

                        if (checktemplate.Count == 0)
                        {
                            data.returnmsg = "Email Template not Mapped to the selected Institution";
                        }
                        else
                        {

                            string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                            if (templatedetails.Contains("[TENANTNAME]"))
                            {
                                templatedetails = templatedetails.Replace("[TENANTNAME]", tenantdetails[0].TENANTAGR_TenantName.ToUpper());
                            }
                            if (templatedetails.Contains("[FLAT]"))
                            {
                                templatedetails = templatedetails.Replace("[FLAT]", tenantdetails[0].HRMR_RoomName.ToString());
                            }
                            if (templatedetails.Contains("[BUILDING]"))
                            {
                                templatedetails = templatedetails.Replace("[BUILDING]", tenantdetails[0].HRMB_BuildingName.ToString());
                            }
                            if (templatedetails.Contains("[ADDRESS]"))
                            {
                                templatedetails = templatedetails.Replace("[ADDRESS]", tenantdetails[0].TENANTAGR_Address.ToString());
                            }
                            if (templatedetails.Contains("[AMOUNT]"))
                            {

                                templatedetails = templatedetails.Replace("[AMOUNT]", tenantdetails[0].TENANTEBWB_Amount.ToString());
                            }
                            if (templatedetails.Contains("[STARTDATE]"))
                            {
                                templatedetails = templatedetails.Replace("[STARTDATE]", tenantdetails[0].TENANTAGR_AgrFromDate.ToString("dd-MM-yyyy"));
                            }
                            //if (templatedetails.Contains("[DATE1]"))
                            //{

                            //    templatedetails = templatedetails.Replace("[DATE1]", strDate);
                            //}

                            try
                            {
                                data.returnmsg = sendmailexamtt(data.MI_Id, tenantdetails[0].TENANTAGR_EmailId.ToString(), templatedetails, emailccdetails, emailbccdetails, "TENANTWATERBILLCHARGES");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }

                    else
                    {
                        var checktemplate = _context.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "TENANTELECTRICALWATERBILL"
                        && a.ISES_MailActiveFlag == true).ToList();

                        if (checktemplate.Count > 0)
                        {
                            emailccdetails = checktemplate.FirstOrDefault().ISES_MailCCId;
                            emailbccdetails = checktemplate.FirstOrDefault().ISES_MailBCCId;
                        }
                        else
                        {
                            emailccdetails = "";
                            emailbccdetails = "";
                        }

                        if (checktemplate.Count == 0)
                        {
                            data.returnmsg = "Email Template not Mapped to the selected Institution";
                        }
                        else
                        {

                            string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                            if (templatedetails.Contains("[NAME]"))
                            {
                                templatedetails = templatedetails.Replace("[NAME]", tenantdetails[0].TENANTAGR_TenantName.ToUpper());
                            }
                            if (templatedetails.Contains("[AMOUNT]"))
                            {

                                templatedetails = templatedetails.Replace("[AMOUNT]", tenantdetails[0].TENANTEBWB_Amount.ToString());
                            }
                            if (templatedetails.Contains("[STATUS]"))
                            {
                                templatedetails = templatedetails.Replace("[STATUS]", tenantdetails[0].TENANTEBWB_Flg.ToUpper());
                            }
                            if (templatedetails.Contains("[MON-YR1]"))
                            {
                                var amt = tenantdetails[0].IVRM_Month_Name.ToUpper() + " - " + tenantdetails[0].TENANTEBWB_Year.ToString();
                                templatedetails = templatedetails.Replace("[MON-YR1]", amt);
                            }

                            try
                            {
                                data.returnmsg = sendmailexamtt(data.MI_Id, tenantdetails[0].TENANTAGR_EmailId.ToString(), templatedetails, emailccdetails, emailbccdetails, "TENANTELECTRICALWATERBILL");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }


                    }
                    
                }


            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                data.returnmsg = ex.Message;
            }

           

            return data;

        }
        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails, string temp)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == temp
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = url;


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
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

               
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
                        message.AddTo(mail_id);
                      //  message.AddTo("kavitajnandoor21@gmail.com");

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
                        //******************* EMAIL CC DETAILS ****************//

                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //******************* EMAIL BCC DETAILS ****************//




                        //if (mailcc != null && mailcc != "")
                        //{
                        //    message.AddCc(mailcc);
                        //}
                        //if (mailbcc != null && mailbcc != "")
                        //{
                        //    message.AddBcc(mailbcc);
                        //}

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



                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "TENANTELECTRICALWATERBILL" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
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

    }
}
