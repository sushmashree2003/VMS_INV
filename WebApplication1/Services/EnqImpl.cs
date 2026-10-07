using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using AutoMapper;
using System.Collections.Concurrent;
using CommonLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Services
{
    public class EnqImpl : Interfaces.EnquiryInterface
    {
       
        private static ConcurrentDictionary<string, EnqDTO> _login =
               new ConcurrentDictionary<string, EnqDTO>();

        public Enquirycontext _Enquirycontext;
        public DomainModelMsSqlServerContext _context;
        //ILogger<EnqImpl> _logger;
        readonly ILogger<EnqImpl> _logger;

        public EnqImpl(Enquirycontext Enquirycontext,DomainModelMsSqlServerContext context, ILogger<EnqImpl> log)
        {
            _Enquirycontext = Enquirycontext;
            _context = context;
            _logger = log;
        }
        public Enq countrydrp(Enq en)
        {
            _logger.LogInformation("Hello, world!");
            _logger.LogDebug("Get start details");
          
            try
            {

                List<Country> allCountry = new List<Country>();
                allCountry = _Enquirycontext.country.ToList();
                en.countryDrpDown = allCountry.ToArray();

                List<State> allstate = new List<State>();
                allstate = _Enquirycontext.state.ToList();
                en.stateDrpDown = allstate.ToArray();

                List<Enquiry> alldetails = new List<Enquiry>();
                alldetails = _Enquirycontext.Enquir.Where(t => t.Id.Equals(en.Id) && t.MI_Id.Equals(en.MI_Id)).OrderBy(t=>t.CreatedDate).ToList();
                en.enqdata = alldetails.ToArray();

                //Added By Sripad Joshi
                //For loading year,class,reference and source Dropdown.

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _Enquirycontext.academicyear.Where(t => t.MI_Id.Equals(en.MI_Id) && t.ASMAY_Id.Equals(en.ASMAY_Id)).ToList();
                en.yearDrpDwn = allyear.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _context.School_M_Class.Where(t => t.MI_Id==en.MI_Id && t.ASMCL_ActiveFlag==true).ToList();
                en.classDrpDwn = allclass.ToArray();

                List<MasterCategory> allcategory = new List<MasterCategory>();
                allcategory = _context.mastercategory.Where(t => t.MI_Id==en.MI_Id && t.AMC_ActiveFlag==1).ToList();
                en.categoryDrpDwn = allcategory.ToArray();

                //if (en.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                //{
                //    en.GeneratedNumber = GenerateEnquiryNumber(en.MI_Id, en.ASMAY_Id, en);
                //}
                //else if (en.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Manual")
                //{
                //    en.GeneratedNumber = "";
                //}
            }
            catch(Exception e)
            {
                _logger.LogDebug(e.Message);
                _logger.LogError(e.Message);
            }
            
            return en;
        }


        public static string GetCurrentFinancialYear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (DateTime.Today.Month > 3)
                FinYear = CurYear;
            else
                FinYear = PreYear;
            return FinYear.Trim();
        }

        //public Enq createdata(Enq en)
        //{
        //    en.PASE_Date = DateTime.UtcNow;
        //    _Enquirycontext.Add(en);
        //    _Enquirycontext.SaveChanges();
        //    return en;


        //}

        public Enq clearEnqdata(int Id)
        {
            bool returnval = true;
            Id = 9;
            Enq org = new Enq();
            List<Enquiry> enquirys = new List<Enquiry>();

           var  enqui = _Enquirycontext.Enquir.Where(t => t.PASE_Id.Equals(Id));
        //   enquirys = _Enquirycontext.Enquir.Where(t => t.PASE_Id.Equals(Id));

            if (enqui.Any())
            {
                _Enquirycontext.Remove(enquirys.ElementAt(0));

                _Enquirycontext.SaveChanges();
            }

            if (enqui.Count() > 0)
            {
                returnval = true;
            }
            else
            {
                returnval = false;
            }

            return org;
        }

        public StateDTO enqdrpcountrydata(int id)
        {


            Array[] drpall = new Array[3];
            //CountryDTO[] coun=new CountryDTO[] 
            //    {
            //        new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="hutching"},
            //        new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="khosys" },
            //          new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="St.Philomina"},


            //    } ;

            StateDTO enq = new StateDTO();
            List<State> allstate = new List<State>();
            //     allstate = _Enquirycontext.country.Where(t => t.IVRMMC_Id.Equals(id)).FirstO;
            // allstate.Add(new (101, "India"));

            allstate = _Enquirycontext.state.ToList();
            allstate = _Enquirycontext.state.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
            enq.stateDrpDown = allstate.ToArray();




            return enq;

            //ViewBag.Country = "UK";
        }

        public CityDTO getcity(int id)
        {
            Array[] drpall = new Array[3];
            CityDTO enq = new CityDTO();
            List<City> allcity = new List<City>();
            allcity = _Enquirycontext.city.ToList();
            //allcity = _OrganisationContext.city.Where(t => t.IVRMMS_Id.Equals(stateid) && t.IVRMMC_Id.Equals(stateid)).ToList();
            allcity = _Enquirycontext.city.Where(t => t.IVRMMS_Id.Equals(id)).ToList();
            enq.cityDrpDown = allcity.ToArray();
            return enq;
        }

        public async Task<Enq> saveEnqdata(Enq enqu)
        {
            enqu.returnMsg = "";
            Enquiry enq = Mapper.Map<Enquiry>(enqu);
            try {
                if (enq.PASE_Id > 0)
                {

                    var result1 = _Enquirycontext.Enquir.Where(t => (t.PASE_FirstName == enqu.PASE_FirstName && t.PASE_MiddleName == enqu.PASE_MiddleName &&
                    t.PASE_LastName==enq.PASE_LastName && t.PASE_Id!=enq.PASE_Id));
                    if (result1.Count() >= 1)
                    {
                        enqu.returnduplicatestatus = "Duplicate";

                    }
                    else
                    {

                    var result = _Enquirycontext.Enquir.Single(t => t.PASE_Id == enq.PASE_Id);
                    result.AMC_Id = enq.AMC_Id;
                    result.ASMAY_Id = enq.ASMAY_Id;
                    result.IVRMMC_Id = enq.IVRMMC_Id;
                    result.ASMCL_Id = enq.ASMCL_Id;
                    // result.MI_Id = enq.MI_Id;
                    result.PASE_Address1 = enq.PASE_Address1;
                    result.PASE_Address2 = enq.PASE_Address2;
                    result.PASE_Address3 = enq.PASE_Address3;
                    result.PASE_Pincode = enq.PASE_Pincode;
                    result.PASE_MobileNo = enq.PASE_MobileNo;
                    result.PASE_Phone = enq.PASE_Phone;
                    result.PASE_emailid = enq.PASE_emailid;
                    result.PASE_City = enq.PASE_City;
                    result.PASE_State = enq.PASE_State;
                    result.PASE_EnquiryNo = enq.PASE_EnquiryNo;
                    result.PASE_FirstName = enq.PASE_FirstName;
                    result.PASE_MiddleName = enq.PASE_MiddleName;
                    result.PASE_LastName = enq.PASE_LastName;
                    result.PASE_EnquiryDetails = enq.PASE_EnquiryDetails;

                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                        result.UpdatedDate = indianTime;

                        // result.PASE_Date = enq.PASE_Date;
                        // result.PASE_ActiveFlag = enq.PASE_ActiveFlag;
                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                    _Enquirycontext.Update(result);

                    var flag = _Enquirycontext.SaveChanges();
                    if (flag == 1)
                    {
                        enqu.returnval = true;
                        enqu.returnMsg = "u";
                        SMS sms = new SMS(_context);

                        string s = await sms.sendSms(enq.MI_Id, result.PASE_MobileNo, "ENQUIRY", enq.PASE_Id);

                        Email Email = new Email(_context);

                        string m = Email.sendmail(enq.MI_Id, result.PASE_emailid, "ENQUIRY", enq.PASE_Id);

                    }
                    else
                    {
                        enqu.returnMsg = "u";
                        enqu.returnval = false;
                    }
                }
                }
                else
                {
                    //var Emailcount = _Enquirycontext.Enquir.Where(t => t.MI_Id == enq.MI_Id && t.PASE_emailid == enq.PASE_emailid).Count();
                    //if (Emailcount > 0)
                    //{
                    //    enqu.returnMsg = "Email Id is already exist.!";
                    //    enqu.returnval = false;
                    //    return enqu;
                    //}
                    //var MobileNocount = _Enquirycontext.Enquir.Where(t => t.MI_Id == enq.MI_Id && t.PASE_MobileNo == enq.PASE_MobileNo).Count();
                    //if (MobileNocount > 0)
                    //{
                    //    enqu.returnMsg = "Mobile Number is already exist.!";
                    //    enqu.returnval = false;
                    //    return enqu;
                    //}

                    //Get Autogenerated Enquiry number


                    if (enqu.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        enqu.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                        enqu.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                        enq.PASE_EnquiryNo = a.GenerateNumber(enqu.transnumbconfigurationsettingsss);

                        // enq.PASE_EnquiryNo = GenerateEnquiryNumber(enq.MI_Id, enq.ASMAY_Id, enqu).ToString();
                    }
                    else
                    {
                        enq.PASE_EnquiryNo = enqu.PASE_EnquiryNo;

                     var count = _Enquirycontext.Enquir.Where(t =>t.MI_Id == enq.MI_Id && t.PASE_EnquiryNo == enq.PASE_EnquiryNo ).Count();
                        if(count > 0)
                        {
                            enqu.returnMsg = "Enquiry No is already exist.!";
                            enqu.returnval = false;
                            return enqu;
                        }
                    }
                  //  enq.PASE_EnquiryNo = enqu.PASE_EnquiryNo;

                    /////////////////

                    enq.PASE_ActiveFlag = 1;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    enq.PASE_Date = indianTime;
                    //added by 02/02/2017


                    enq.CreatedDate = indianTime;
                    enq.UpdatedDate = indianTime;



                    _Enquirycontext.Add(enq);

                    var flag = _Enquirycontext.SaveChanges();

                    if (flag == 1)
                    {
                        enqu.returnMsg = "a";
                        enqu.returnval = true;

                        SMS sms = new SMS(_context);

                        string s = await sms.sendSms(enq.MI_Id, enq.PASE_MobileNo, "ENQUIRY", enq.PASE_Id);

                        Email Email = new Email(_context);

                        string m = Email.sendmail(enq.MI_Id, enq.PASE_emailid, "ENQUIRY", enq.PASE_Id);

                    }
                    else
                    {
                        enqu.returnMsg = "a";
                        enqu.returnval = false;
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enqu;
        }

        public Enq EditDetails(int id)
        {
            Enq enquirydetails = new Enq();
            try
            {
             
                List<Enquiry> enq = new List<Enquiry>();
                enq = _Enquirycontext.Enquir.AsNoTracking().Where(t => t.PASE_Id.Equals(id)).ToList();
                enquirydetails.enqdata = enq.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _Enquirycontext.academicyear.Where(t => t.ASMAY_Id.Equals(enq[0].ASMAY_Id)).ToList();
                enquirydetails.yearDrpDwn = allyear.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enquirydetails;
        }

        public Enq DeleteEnqDetails(Enq enquirydetails)
        {
           
            //Enq enquirydetails = new Enq();
            List<Enquiry> enquiry = new List<Enquiry>(); // Mapper.Map<Organisation>(org);
            //_OrganisationContext.Remove(enq);
            //_OrganisationContext.SaveChanges();

            try
            {
                enquiry = _Enquirycontext.Enquir.Where(t => t.PASE_Id.Equals(enquirydetails.PASE_Id)).ToList();

                if (enquiry.Any())
                {
                    _Enquirycontext.Remove(enquiry.ElementAt(0));

                    var flag = _Enquirycontext.SaveChanges();
                    if (flag == 1)
                    {
                        enquirydetails.returnval = true;
                    }
                    else
                    {
                        enquirydetails.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enquirydetails;
        }

        //public Enq GetAllDetais(Enq en)
        //{
        //    List<Enquiry> allEnquiryDetails = new List<Enquiry>();
        //    Enquiry enq = Mapper.Map<Enquiry>(en);
           
        //    var result = _Enquirycontext.Enquir.Single(t => t.PASE_Id==enq.PASE_Id);
         
          
        //    result.PASE_Phone= enq.PASE_Phone;
        //    result.PASE_emailid = enq.PASE_emailid;
        //    result.PASE_FirstName = enq.PASE_FirstName;



        //    allEnquiryDetails = _Enquirycontext.Enquir.ToList();
        //    en.enqdata = allEnquiryDetails.ToArray();
          
        //    return en;
        //}
    }
}
