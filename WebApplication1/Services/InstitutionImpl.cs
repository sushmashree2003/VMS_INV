using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using SendGrid;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;



namespace WebApplication1.Services
{
    public class InstitutionImpl : Interfaces.Institutioninterface
    {


        public DomainModelMsSqlServerContext _Context;

        public InstitutionImpl(DomainModelMsSqlServerContext OrganisationContext)
        {
            _Context = OrganisationContext;
        }
        public async Task<InstitutionDTO> OnPageloadData(InstitutionDTO stu)
        {
            try
            {

                var rolelist = _Context.MasterRoleType.Where(t => t.IVRMRT_Id == stu.roleId).ToList();

                if (rolelist[0].IVRMRT_Role == "Super Admin")
                {
                    stu = GetDataBasedonSuperAdminRoleType(stu);
                }
                else if (rolelist[0].IVRMRT_Role.Equals("Multi Admin"))
                {

                    stu = GetDataBasedonMultiAdminRoleType(stu);

                }
                else if (rolelist[0].IVRMRT_Role.Equals("Admin"))
                {

                    stu= GetDataBasedonAdminRoleType(stu);
                }

                stu.countryDrpDown = _Context.country.Select(m => new Country { IVRMMC_Id = m.IVRMMC_Id, IVRMMC_CountryName = m.IVRMMC_CountryName }).ToArray();

              //  List<MandatoryFields> mandtory = new List<MandatoryFields>();
              //  mandtory = _Context.mandatory.Where(p => p.IVRMP_Id == stu.IVRMP_Id && p.MI_Id == stu.sessionMI_Id && p.MO_Id == stu.sessionMO_Id).ToList();
              //  stu.mandatoryList = mandtory.ToArray();

              //  stu.subscriptionlist = (from sub in _Context.Master_Institution_SubscriptionValidity
              //                          from im in _Context.Institute
              //                          where sub.MI_Id == im.MI_Id
              //                          select new Master_Institution_SubscriptionValidity
              //                          {
              //                              MISV_Id = sub.MISV_Id,
              //                              MI_Id = sub.MI_Id,
              //                              MISV_FromDate = sub.MISV_FromDate,
              //                              MISV_ToDate = sub.MISV_ToDate,
              //                              MISV_SubscriptionNo = sub.MISV_SubscriptionNo,
              //                              MISV_SubscriptionType = sub.MISV_SubscriptionType,
              //                              CreatedDate = sub.CreatedDate,
              //                              UpdatedDate = sub.UpdatedDate,
              //                              MISV_ActiveFlag = sub.MISV_ActiveFlag,
              //                              Institution = sub.Institution
              //                          }).OrderBy(sub=>sub.CreatedDate).ToArray();


              //  stu.Institutionname = (from mi in _Context.Institution
              //                               from mo in _Context.Organisation
              //                               where mi.MO_Id == mo.MO_Id
              //                               select new Institution
              //                               {
              //                                   MO_Id = mi.MO_Id,
              //                                   MI_Id = mi.MI_Id,
              //                                   MI_Name = mi.MI_Name,
              //                                   MI_Address1 = mi.MI_Address1,
              //                                   MI_Pincode = mi.MI_Pincode,
              //                                   MI_ActiveFlag = mi.MI_ActiveFlag,
              //                                   Organisation = mi.Organisation
              //                               }).OrderBy(mi => mi.CreatedDate).ToArray();

              // // await (InstitutionPaginationdetails(stu.instutePagination, stu));
              ////  Master_Institution_SubscriptionValidityDTO Master_Institution_SubscriptionValidityDTO = new Master_Institution_SubscriptionValidityDTO();
              ////  await (SubscriptionPaginationdetails(stu.subscriptionPagination, Master_Institution_SubscriptionValidityDTO));



              //  //  List<Institution> allInstitution = new List<Institution>();
              //  // allInstitution = _Context.Institution.Select(x => new { Id = x.MI_Id, Value = x.MI_Name }).ToList();


              //  stu.instutedropdown = _Context.Institution.Select(m => new Institution { MI_Id = m.MI_Id, MI_Name = m.MI_Name }).ToArray();

              //  stu.TrustDropdown = _Context.Organisation.Select(m => new Organisation { MO_Id = m.MO_Id, MO_Name = m.MO_Name }).ToArray();

              //  //stu.countryDrpDown = _Context.country.Where(s=>s.IVRMMC_Default ==1).Select(m => new Country { IVRMMC_Id = m.IVRMMC_Id, IVRMMC_CountryName = m.IVRMMC_CountryName }).ToArray();

              //  stu.countryDrpDown = _Context.country.Select(m => new Country { IVRMMC_Id = m.IVRMMC_Id, IVRMMC_CountryName = m.IVRMMC_CountryName }).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }

        //Admin
        public InstitutionDTO GetDataBasedonAdminRoleType(InstitutionDTO stu)
        {

            List<MandatoryFields> mandtory = new List<MandatoryFields>();
            mandtory = _Context.mandatory.Where(p => p.IVRMP_Id == stu.IVRMP_Id && p.MI_Id == stu.sessionMI_Id && p.MO_Id == stu.sessionMO_Id).ToList();
            stu.mandatoryList = mandtory.ToArray();

            stu.subscriptionlist = (from sub in _Context.Master_Institution_SubscriptionValidity
                                    from im in _Context.Institute
                                    where sub.MI_Id == im.MI_Id && sub.MI_Id == stu.sessionMI_Id
                                    select new Master_Institution_SubscriptionValidity
                                    {
                                        MISV_Id = sub.MISV_Id,
                                        MI_Id = sub.MI_Id,
                                        MISV_FromDate = sub.MISV_FromDate,
                                        MISV_ToDate = sub.MISV_ToDate,
                                        MISV_SubscriptionNo = sub.MISV_SubscriptionNo,
                                        MISV_SubscriptionType = sub.MISV_SubscriptionType,
                                        CreatedDate = sub.CreatedDate,
                                        UpdatedDate = sub.UpdatedDate,
                                        MISV_ActiveFlag = sub.MISV_ActiveFlag,
                                        Institution = sub.Institution
                                    }).OrderBy(sub => sub.CreatedDate).ToArray();


            stu.Institutionname = (from mi in _Context.Institution
                                   from mo in _Context.Organisation
                                   where mi.MO_Id == mo.MO_Id && mi.MI_Id == stu.sessionMI_Id
                                   select new Institution
                                   {
                                       MO_Id = mi.MO_Id,
                                       MI_Id = mi.MI_Id,
                                       MI_Name = mi.MI_Name,
                                       MI_Address1 = mi.MI_Address1,
                                       MI_Pincode = mi.MI_Pincode,
                                       MI_ActiveFlag = mi.MI_ActiveFlag,
                                       Organisation = mi.Organisation
                                   }).OrderBy(mi => mi.CreatedDate).ToArray();

            stu.instutedropdown = _Context.Institution.Where(mi => mi.MI_Id == stu.sessionMI_Id && mi.MI_ActiveFlag == 1).Select(m => new Institution { MI_Id = m.MI_Id, MI_Name = m.MI_Name }).ToArray();

            stu.TrustDropdown = _Context.Organisation.Where(mi => mi.MO_Id == stu.sessionMO_Id && mi.MO_ActiveFlag == 1).Select(m => new Organisation { MO_Id = m.MO_Id, MO_Name = m.MO_Name }).ToArray();

            return stu;
        }

        //SuperAdmin
        public InstitutionDTO GetDataBasedonSuperAdminRoleType(InstitutionDTO stu)
        {

            List<MandatoryFields> mandtory = new List<MandatoryFields>();
            mandtory = _Context.mandatory.Where(p => p.IVRMP_Id == stu.IVRMP_Id).ToList();
            stu.mandatoryList = mandtory.ToArray();

            stu.subscriptionlist = (from sub in _Context.Master_Institution_SubscriptionValidity
                                    from im in _Context.Institute
                                    where sub.MI_Id == im.MI_Id
                                    select new Master_Institution_SubscriptionValidity
                                    {
                                        MISV_Id = sub.MISV_Id,
                                        MI_Id = sub.MI_Id,
                                        MISV_FromDate = sub.MISV_FromDate,
                                        MISV_ToDate = sub.MISV_ToDate,
                                        MISV_SubscriptionNo = sub.MISV_SubscriptionNo,
                                        MISV_SubscriptionType = sub.MISV_SubscriptionType,
                                        CreatedDate = sub.CreatedDate,
                                        UpdatedDate = sub.UpdatedDate,
                                        MISV_ActiveFlag = sub.MISV_ActiveFlag,
                                        Institution = sub.Institution
                                    }).OrderBy(sub => sub.CreatedDate).ToArray();


            stu.Institutionname = (from mi in _Context.Institution
                                   from mo in _Context.Organisation
                                   where mi.MO_Id == mo.MO_Id 
                                   select new Institution
                                   {
                                       MO_Id = mi.MO_Id,
                                       MI_Id = mi.MI_Id,
                                       MI_Name = mi.MI_Name,
                                       MI_Address1 = mi.MI_Address1,
                                       MI_Pincode = mi.MI_Pincode,
                                       MI_ActiveFlag = mi.MI_ActiveFlag,
                                       Organisation = mi.Organisation
                                   }).OrderBy(mi => mi.CreatedDate).ToArray();


            stu.instutedropdown = _Context.Institution.Where(t=>t.MI_ActiveFlag == 1).Select(m => new Institution { MI_Id = m.MI_Id, MI_Name = m.MI_Name }).ToArray();

            stu.TrustDropdown = _Context.Organisation.Where(t => t.MO_ActiveFlag == 1).Select(m => new Organisation { MO_Id = m.MO_Id, MO_Name = m.MO_Name }).ToArray();

            return stu;
        }

        //MultiAdmin
        public InstitutionDTO GetDataBasedonMultiAdminRoleType(InstitutionDTO stu)
        {

            var Mi_id_list = _Context.Institute.Where(d => d.MO_Id.Equals(stu.sessionMO_Id)).Select(d => d.MI_Id).ToList();


            List<MandatoryFields> mandtory = new List<MandatoryFields>();
            mandtory = _Context.mandatory.Where(p => p.IVRMP_Id == stu.IVRMP_Id &&  Mi_id_list.Contains(p.MI_Id)).ToList();
            stu.mandatoryList = mandtory.ToArray();

            stu.subscriptionlist = (from sub in _Context.Master_Institution_SubscriptionValidity
                                    from im in _Context.Institute
                                   
                                    where sub.MI_Id == im.MI_Id && Mi_id_list.Contains(sub.MI_Id)
                                    select new Master_Institution_SubscriptionValidity
                                    {
                                        MISV_Id = sub.MISV_Id,
                                        MI_Id = sub.MI_Id,
                                        MISV_FromDate = sub.MISV_FromDate,
                                        MISV_ToDate = sub.MISV_ToDate,
                                        MISV_SubscriptionNo = sub.MISV_SubscriptionNo,
                                        MISV_SubscriptionType = sub.MISV_SubscriptionType,
                                        CreatedDate = sub.CreatedDate,
                                        UpdatedDate = sub.UpdatedDate,
                                        MISV_ActiveFlag = sub.MISV_ActiveFlag,
                                        Institution = sub.Institution
                                    }).OrderBy(sub => sub.CreatedDate).ToArray();


            stu.Institutionname = (from mi in _Context.Institution
                                   from mo in _Context.Organisation
                                   where mi.MO_Id == mo.MO_Id && mi.MO_Id == stu.sessionMO_Id
                                   select new Institution
                                   {
                                       MO_Id = mi.MO_Id,
                                       MI_Id = mi.MI_Id,
                                       MI_Name = mi.MI_Name,
                                       MI_Address1 = mi.MI_Address1,
                                       MI_Pincode = mi.MI_Pincode,
                                       MI_ActiveFlag = mi.MI_ActiveFlag,
                                       Organisation = mi.Organisation
                                   }).OrderBy(mi => mi.CreatedDate).ToArray();

          
            stu.instutedropdown = _Context.Institution.Where(mi=> mi.MO_Id == stu.sessionMO_Id && mi.MI_ActiveFlag==1).Select(m => new Institution { MI_Id = m.MI_Id, MI_Name = m.MI_Name }).ToArray();

            stu.TrustDropdown = _Context.Organisation.Where(mi => mi.MO_Id == stu.sessionMO_Id && mi.MO_ActiveFlag == 1).Select(m => new Organisation { MO_Id = m.MO_Id, MO_Name = m.MO_Name }).ToArray();

            //stu.countryDrpDown = _Context.country.Where(s=>s.IVRMMC_Default ==1).Select(m => new Country { IVRMMC_Id = m.IVRMMC_Id, IVRMMC_CountryName = m.IVRMMC_CountryName }).ToArray();
            return stu;
        }


        public InstitutionDTO saveInstitute(InstitutionDTO InstitDTO)
        {
            List<Institution> allInstitutionname = new List<Institution>();
            try
            {
                Institution enq = Mapper.Map<Institution>(InstitDTO);

                // enq.MO_Id = 12;
                if (enq.MI_Id > 0)
                {
                    var result = _Context.Institution.Single(t => t.MI_Id == enq.MI_Id);
                    // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);
                    result.MO_Id = enq.MO_Id;
                    result.MI_Name = enq.MI_Name;
                    result.MI_Type = enq.MI_Type;
                    result.MI_Affiliation = enq.MI_Affiliation;
                    result.MI_Address1 = enq.MI_Address1;
                    result.MI_Address2 = enq.MI_Address2;
                    result.MI_Address3 = enq.MI_Address3;
                    result.IVRMMCT_Name = enq.IVRMMCT_Name;
                    result.IVRMMS_Id = enq.IVRMMS_Id;
                    result.IVRMMC_Id = enq.IVRMMC_Id;
                    result.MI_Pincode = enq.MI_Pincode;
                    result.MI_FaxNo = enq.MI_FaxNo;
                    result.MI_FormColor = enq.MI_FormColor;
                    result.MI_FontColor = enq.MI_FontColor;
                    result.MI_FontSize = enq.MI_FontSize;
                    result.MI_WeekStartDay = enq.MI_WeekStartDay;
                    result.MI_DateFormat = enq.MI_DateFormat;
                    result.MI_DateSeparator = enq.MI_DateSeparator;
                    result.MI_GradingSystem = enq.MI_GradingSystem;
                    result.MI_Logo = enq.MI_Logo;
                    result.MI_BackgroundImage = enq.MI_BackgroundImage;
                    result.MI_FranchiseFlag = enq.MI_FranchiseFlag;
                    //result.MI_ActiveFlag = enq.MI_ActiveFlag;
                    result.MI_Subdomain = enq.MI_Subdomain;
                    result.MI_AboutInstitute = enq.MI_AboutInstitute;
                    result.MI_ContactDetails = enq.MI_ContactDetails;
                    result.MI_IVRSOutboundNo = enq.MI_IVRSOutboundNo;
                    result.MI_IVRSVirtualNo = enq.MI_IVRSVirtualNo;
                    result.MI_GPSUserName = enq.MI_GPSUserName;
                    result.MI_GSTNO = enq.MI_GSTNO;
                    result.MI_CINNo = enq.MI_CINNo;
                    //result.MI_PAN = enq.MI_PAN;
                    //result.MI_TAN = enq.MI_TAN;
                    //added by 02/02/2017

                    result.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    InstitDTO.returnval = "update";
                    _Context.SaveChanges();
                    InstitDTO.MI_Id = result.MI_Id;

                    AddUpdateMobile(InstitDTO);
                    AddUpdatePhone(InstitDTO);
                    AddUpdateEmail(InstitDTO);

                    //SMS sms = new SMS(_Context);
                    //string Smsmessage = "Institution created successfully";
                    //sms.sendSms(InstitDTO.MI_Id, InstitDTO.mobiles[0].MIMN_MobileNo, InstitDTO.UserId, "InstitutionCreation", Smsmessage);


                }
                else
                {    //added by 02/02/2017
                    enq.CreatedDate = DateTime.Now;
                    enq.UpdatedDate = DateTime.Now;
                    enq.MI_ActiveFlag = 1;
                    _Context.Add(enq);
                    int count = _Context.SaveChanges();
                    if (count == 1)
                    {
                        StoreDefaultValuesToNewInstitution(enq.MO_Id, InstitDTO.sessionMI_Id, enq.MI_Id, enq.MI_Subdomain);

                        VirtaulSchool Updatevirtual = new VirtaulSchool();
                        Updatevirtual.IVRM_Virtual_School_Id = enq.MI_Id + enq.MO_Id;
                        Updatevirtual.IVRM_MI_Id = enq.MI_Id;
                        Updatevirtual.IVRM_MO_Id = enq.MO_Id;
                        Updatevirtual.IVRM_Sub_Domain_Name = enq.MI_Subdomain;
                        Updatevirtual.ivrm_school_code = enq.MI_Subdomain;
                        Updatevirtual.IVRM_OTP_ADMNO = "OTP";
                        Updatevirtual.CreatedDate = DateTime.Now;
                        Updatevirtual.UpdatedDate = DateTime.Now;
                        _Context.Add(Updatevirtual);
                        int countv= _Context.SaveChanges();



                        //SMS sms = new SMS(_Context);
                        //string Smsmessage = "Institution created successfully";
                        //sms.sendSms(enq.MI_Id, InstitDTO.mobiles[0].MIMN_MobileNo, InstitDTO.UserId, "InstitutionCreation", Smsmessage);

                        //var message = new SendGrid.SendGridMessage();
                        //message.From = new MailAddress("vapstech123@gmail.com", "vaps@123");
                        //message.Subject = "New Institute Creation";
                        //foreach (var email in InstitDTO.emails)
                        //{
                        //    message.AddTo(email.MIE_EmailId);
                        //}

                        //message.Html = "<p>Institution created successfully</p>";
                        //var client = new Web("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");
                        //client.DeliverAsync(message).Wait();
                    }

                    InstitDTO.returnval = "add";
                    InstitDTO.MI_Id = enq.MI_Id;

                    AddUpdateMobile(InstitDTO);
                    AddUpdatePhone(InstitDTO);
                    AddUpdateEmail(InstitDTO);
                    SendMailAndMsgToRegisteredInstitute(InstitDTO);
                }
                //store images
                //saveInstitutionLogoImagedetails(InstitDTO);
                InstitDTO.Institutionname = (from mi in _Context.Institution
                                             from mo in _Context.Organisation
                                             where mi.MO_Id == mo.MO_Id
                                             select new Institution
                                             {
                                                 MO_Id = mi.MO_Id,
                                                 MI_Id = mi.MI_Id,
                                                 MI_Name = mi.MI_Name,
                                                 MI_Address1 = mi.MI_Address1,
                                                 MI_Pincode = mi.MI_Pincode,
                                                 MI_ActiveFlag = mi.MI_ActiveFlag,
                                                 Organisation = mi.Organisation
                                             }).OrderBy(mi => mi.CreatedDate).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                InstitDTO.Institutionname = (from mi in _Context.Institution
                                             from mo in _Context.Organisation
                                             where mi.MO_Id == mo.MO_Id
                                             select new Institution
                                             {
                                                 MO_Id = mi.MO_Id,
                                                 MI_Id = mi.MI_Id,
                                                 MI_Name = mi.MI_Name,
                                                 MI_Address1 = mi.MI_Address1,
                                                 MI_Pincode = mi.MI_Pincode,
                                                 MI_ActiveFlag = mi.MI_ActiveFlag,
                                                 Organisation = mi.Organisation
                                             }).OrderBy(mi => mi.CreatedDate).ToArray();
            }

            return InstitDTO;
        }

        //

        public async void StoreDefaultValuesToNewInstitution(long MO_Id, long sessionMI_Id, long MI_Id, string MI_Subdomain)
        {
            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "preadmission_new_institute_create";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@mo_id",
                    SqlDbType.BigInt)
                {
                    Value = MO_Id
                });
                cmd.Parameters.Add(new SqlParameter("@old_mi_id",
                   SqlDbType.BigInt)
                {
                    Value = sessionMI_Id
                });

                cmd.Parameters.Add(new SqlParameter("@new_mi_id",
                   SqlDbType.BigInt)
                {
                    Value = MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@IVRM_Sub_Domain_Name",
                               SqlDbType.VarChar)
                {
                    Value = MI_Subdomain
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                       
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //

        public void AddUpdateMobile(InstitutionDTO InstitDTO)
        {
            //add/update Mobile details
            if (InstitDTO.mobiles.Count() > 0)
            {
                foreach (Institution_MobileDTO mob in InstitDTO.mobiles)
                {
                    mob.MI_Id = InstitDTO.MI_Id;
                    Institution_MobileNo mobile = Mapper.Map<Institution_MobileNo>(mob);

                    if (mobile.MIMN_Id > 0)
                    {
                        var MobileNoresult = _Context.Institution_MobileNo.Single(t => t.MIMN_Id == mob.MIMN_Id);
                        MobileNoresult.MIMN_Id = mob.MIMN_Id;
                        MobileNoresult.MIMN_MobileNo = mob.MIMN_MobileNo;
                        MobileNoresult.MI_Id = mob.MI_Id;
                        //added by 02/02/2017                    
                        MobileNoresult.UpdatedDate = DateTime.Now;
                        _Context.Update(MobileNoresult);
                        _Context.SaveChanges();
                    }
                    else
                    {
                        mobile.CreatedDate = DateTime.Now;
                        mobile.UpdatedDate = DateTime.Now;
                        _Context.Add(mobile);
                        _Context.SaveChanges();
                    }
                }
            }
        }
        public void AddUpdatePhone(InstitutionDTO InstitDTO)
        {
            //add/uppdate phone details
            if (InstitDTO.phones.Count() > 0)
            {
                foreach (Institution_Phone_NoDTO ph in InstitDTO.phones)
                {
                    ph.MI_Id = InstitDTO.MI_Id;
                    Institution_Phone_No phone = Mapper.Map<Institution_Phone_No>(ph);

                    if (phone.MIPN_Id > 0)
                    {
                        var Phone_Noresult = _Context.Institution_Phone_No.Single(t => t.MIPN_Id == ph.MIPN_Id);
                        Phone_Noresult.MIPN_Id = ph.MIPN_Id;
                        Phone_Noresult.MIPN_PhoneNo = ph.MIPN_PhoneNo;
                        Phone_Noresult.MI_Id = ph.MI_Id;
                        //added by 02/02/2017                    
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        _Context.Update(Phone_Noresult);

                    }
                    else
                    { //added by 02/02/2017
                        phone.CreatedDate = DateTime.Now;
                        phone.UpdatedDate = DateTime.Now;

                        _Context.Add(phone);
                    }
                    _Context.SaveChanges();

                }
            }

        }
        public void AddUpdateEmail(InstitutionDTO InstitDTO)
        {
            //add/update Email details
            if (InstitDTO.emails.Count() > 0)
            {
                foreach (Institution_EmailIdDTO Em in InstitDTO.emails)
                {
                    Em.MI_Id = InstitDTO.MI_Id;
                    Institution_EmailId email = Mapper.Map<Institution_EmailId>(Em);

                    if (email.MIE_Id > 0)
                    {
                        var emailresult = _Context.Institution_EmailId.Single(t => t.MIE_Id == Em.MIE_Id);
                        emailresult.MIE_Id = Em.MIE_Id;
                        emailresult.MIE_EmailId = Em.MIE_EmailId;
                        emailresult.MI_Id = Em.MI_Id;
                        //added by 02/02/2017
                     
                        email.UpdatedDate = DateTime.Now;
                        _Context.Update(emailresult);
                    }
                    else
                    {//added by 02/02/2017
                        email.CreatedDate = DateTime.Now;
                        email.UpdatedDate = DateTime.Now;
                        _Context.Add(email);
                    }
                    _Context.SaveChanges();

                }
            }
        }

        public void SendMailAndMsgToRegisteredInstitute(InstitutionDTO InstitDTO)
        {

            // SMS sms = new SMS(_Context);
            //string s = sms.sendSms(InstitDTO.MI_Id, result.PASE_MobileNo, "ENQUIRY", enq.PASE_Id);

            //Email Email = new Email(_context);

            //string m = Email.sendmail(enq.MI_Id, result.PASE_emailid, "ENQUIRY", enq.PASE_Id);
        }



        public StateDTO getStatedataByCountryID(int id)
        {
            StateDTO enq = new StateDTO();
            try
            {
                Array[] drpall = new Array[3];
                List<State> allstate = new List<State>();
                allstate = _Context.State.ToList();
                allstate = _Context.State.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
                enq.stateDrpDown = allstate.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enq;
        }

        public CityDTO getcity(int id)
        {
            CityDTO enq = new CityDTO();
            try
            {
                Array[] drpall = new Array[3];
                List<City> allcity = new List<City>();
                allcity = _Context.city.ToList();
                allcity = _Context.city.Where(t => t.IVRMMS_Id.Equals(id)).ToList();
                enq.cityDrpDown = allcity.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enq;
        }

        public InstitutionDTO deleterec(int id)
        {
            InstitutionDTO instut = new InstitutionDTO();

            try
            {
                var result = _Context.Institution.Single(t => t.MI_Id == id);
                if(result.MI_ActiveFlag == 1)
                {
                    result.MI_ActiveFlag = 0;
                    result.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    _Context.SaveChanges();
                   instut.returnval = "true";
                }
                else
                {
                    result.MI_ActiveFlag = 1;
                    result.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    _Context.SaveChanges();
                    instut.returnval = "false";
                }

                
                instut.Institutionname = (from mi in _Context.Institution
                                          from mo in _Context.Organisation
                                          where mi.MO_Id == mo.MO_Id
                                          select new Institution
                                          {
                                              MO_Id = mi.MO_Id,
                                              MI_Id = mi.MI_Id,
                                              MI_Name = mi.MI_Name,
                                              MI_Address1 = mi.MI_Address1,
                                              MI_Pincode = mi.MI_Pincode,
                                              MI_ActiveFlag = mi.MI_ActiveFlag,
                                              Organisation = mi.Organisation
                                          }).OrderBy(mi => mi.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return instut;
        }

        public InstitutionDTO getdetails(int id)
        {
            InstitutionDTO org = new InstitutionDTO();
            try
            {
                List<Institution> lorg = new List<Institution>();

                List<Institution_MobileNo> mob = new List<Institution_MobileNo>();
                List<Institution_Phone_No> phn = new List<Institution_Phone_No>();
                List<Institution_EmailId> email = new List<Institution_EmailId>();

                lorg = _Context.Institution.AsNoTracking().Where(t => t.MI_Id.Equals(id)).ToList();
                org.Institutionname = lorg.ToArray();
                org.MI_BackgroundImage = lorg.FirstOrDefault().MI_BackgroundImage;
                org.MI_Logo = lorg.FirstOrDefault().MI_Logo;
                   

                email = _Context.Institution_EmailId.AsNoTracking().Where(t => t.MI_Id.Equals(id)).ToList();
                org.EmailarrayList = email.ToArray();

                phn = _Context.Institution_Phone_No.AsNoTracking().Where(t => t.MI_Id.Equals(id)).ToList();
                org.PhonearrayList = phn.ToArray();

                mob = _Context.Institution_MobileNo.AsNoTracking().Where(t => t.MI_Id.Equals(id)).ToList();
                org.MobilearrayList = mob.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public InstitutionDTO DuplicateData(InstitutionDTO InstitDTO)
        {

          
            try
            {
                Institution enq = Mapper.Map<Institution>(InstitDTO);
               
                //duplicate institute name details

                int MI_NameExist = _Context.Institution.Where(t =>t.MO_Id==enq.MO_Id && t.MI_Name == enq.MI_Name && t.IVRMMCT_Name ==enq.IVRMMCT_Name).Count();
                if (MI_NameExist > 0)
                {
                    InstitDTO.returnval = "nameexist";

                  
                    InstitDTO.Institutionname = (from mi in _Context.Institution
                                                 from mo in _Context.Organisation
                                                 where mi.MO_Id == mo.MO_Id
                                                 select new Institution
                                                 {
                                                     MO_Id = mi.MO_Id,
                                                     MI_Id = mi.MI_Id,
                                                     MI_Name = mi.MI_Name,
                                                     MI_Address1 = mi.MI_Address1,
                                                     MI_Pincode = mi.MI_Pincode,
                                                     MI_ActiveFlag = mi.MI_ActiveFlag,
                                                     Organisation = mi.Organisation
                                                 }).OrderBy(mi => mi.CreatedDate).ToArray();

                    return InstitDTO;
                }
                //duplicate phone details
                else if (InstitDTO.phones.Count() > 0)
                {
                    foreach (Institution_Phone_NoDTO ph in InstitDTO.phones)
                    {
                        Institution_Phone_No phone = Mapper.Map<Institution_Phone_No>(ph);
                        int Phone_NoExist = _Context.Institution_Phone_No.Where(t => t.MIPN_PhoneNo == ph.MIPN_PhoneNo && t.MIPN_PhoneNo != 0).Count();
                        if (Phone_NoExist > 0)
                        {
                            InstitDTO.returnval = "phoneexist";
                          
                            InstitDTO.Institutionname = (from mi in _Context.Institution
                                                         from mo in _Context.Organisation
                                                         where mi.MO_Id == mo.MO_Id
                                                         select new Institution
                                                         {
                                                             MO_Id = mi.MO_Id,
                                                             MI_Id = mi.MI_Id,
                                                             MI_Name = mi.MI_Name,
                                                             MI_Address1 = mi.MI_Address1,
                                                             MI_Pincode = mi.MI_Pincode,
                                                             MI_ActiveFlag = mi.MI_ActiveFlag,
                                                             Organisation = mi.Organisation
                                                         }).OrderBy(mi => mi.CreatedDate).ToArray();
                            return InstitDTO;
                        }

                    }
                }

                //add/update Mobile details
                else if (InstitDTO.mobiles.Count() > 0)
                {
                    foreach (Institution_MobileDTO mob in InstitDTO.mobiles)
                    {
                        Institution_MobileNo mobile = Mapper.Map<Institution_MobileNo>(mob);
                        var MobileNoExist = _Context.Institution_MobileNo.Where(t => t.MIMN_MobileNo == mob.MIMN_MobileNo && t.MIMN_MobileNo != 0).Count();
                        if (MobileNoExist > 0)
                        {
                            InstitDTO.returnval = "mobileexist";
                          
                            InstitDTO.Institutionname = (from mi in _Context.Institution
                                                         from mo in _Context.Organisation
                                                         where mi.MO_Id == mo.MO_Id
                                                         select new Institution
                                                         {
                                                             MO_Id = mi.MO_Id,
                                                             MI_Id = mi.MI_Id,
                                                             MI_Name = mi.MI_Name,
                                                             MI_Address1 = mi.MI_Address1,
                                                             MI_Pincode = mi.MI_Pincode,
                                                             MI_ActiveFlag = mi.MI_ActiveFlag,
                                                             Organisation = mi.Organisation
                                                         }).OrderBy(mi => mi.CreatedDate).ToArray();
                            return InstitDTO;
                        }
                    }
                }

                //duplicate email details
                else if (InstitDTO.emails.Count() > 0)
                {
                    foreach (Institution_EmailIdDTO Em in InstitDTO.emails)
                    {
                        Institution_EmailId email = Mapper.Map<Institution_EmailId>(Em);

                        int MIE_EmailIdExist = _Context.Institution_EmailId.Where(t => t.MIE_EmailId == email.MIE_EmailId).Count();
                        if (MIE_EmailIdExist > 0)
                        {
                            InstitDTO.returnval = "emailexist";
                          
                            InstitDTO.Institutionname = (from mi in _Context.Institution
                                                         from mo in _Context.Organisation
                                                         where mi.MO_Id == mo.MO_Id
                                                         select new Institution
                                                         {
                                                             MO_Id = mi.MO_Id,
                                                             MI_Id = mi.MI_Id,
                                                             MI_Name = mi.MI_Name,
                                                             MI_Address1 = mi.MI_Address1,
                                                             MI_Pincode = mi.MI_Pincode,
                                                             MI_ActiveFlag = mi.MI_ActiveFlag,
                                                             Organisation = mi.Organisation
                                                         }).OrderBy(mi=>mi.CreatedDate).ToArray();
                            return InstitDTO;
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }

            return InstitDTO;
        }

        public Master_Institution_SubscriptionValidityDTO SaveSubscriptionValidity(Master_Institution_SubscriptionValidityDTO sb)
        {
            try
            {
                Master_Institution_SubscriptionValidity subscription = Mapper.Map<Master_Institution_SubscriptionValidity>(sb);

                if (subscription.MISV_Id > 0)
                {
                    var subscriptionresult = _Context.Master_Institution_SubscriptionValidity.Single(t => t.MISV_Id == subscription.MISV_Id);
                    sb.UpdatedDate = DateTime.Now;
                    Mapper.Map(sb, subscriptionresult);
                    _Context.Update(subscriptionresult);
                    _Context.SaveChanges();
                    sb.returnval = "sbupdate";
                    sb.MISV_Id = subscription.MISV_Id;
                }
                else
                {
                    int count = _Context.Master_Institution_SubscriptionValidity.Where(t => t.MI_Id == subscription.MI_Id && 
                    t.MISV_SubscriptionType == subscription.MISV_SubscriptionType
                    && t.MISV_FromDate.Equals(subscription.MISV_FromDate) && t.MISV_ToDate.Equals(subscription.MISV_ToDate)
                    ).Count();

                    if (count > 0)
                    {
                        sb.returnval = "sbdup";
                        return sb;
                    }
                    else
                    {
                        subscription.MISV_ActiveFlag = true;
                        subscription.CreatedDate = DateTime.Now;
                        subscription.UpdatedDate = DateTime.Now;
                        _Context.Add(subscription);
                        _Context.SaveChanges();
                        sb.returnval = "sbsave";
                        sb.MISV_Id = subscription.MISV_Id;
                        SendEmailSMSForSubscription(sb);
                    }
                    
                }

              




                sb.subscriptionlist = (from sub in _Context.Master_Institution_SubscriptionValidity
                                        from im in _Context.Institute
                                        where sub.MI_Id == im.MI_Id
                                        select new Master_Institution_SubscriptionValidity
                                        {
                                            MISV_Id = sub.MISV_Id,
                                            MI_Id = sub.MI_Id,
                                            MISV_FromDate = sub.MISV_FromDate,
                                            MISV_ToDate = sub.MISV_ToDate,
                                            MISV_SubscriptionNo = sub.MISV_SubscriptionNo,
                                            MISV_SubscriptionType = sub.MISV_SubscriptionType,
                                            CreatedDate = sub.CreatedDate,
                                            UpdatedDate = sub.UpdatedDate,
                                            MISV_ActiveFlag = sub.MISV_ActiveFlag,
                                            Institution = sub.Institution
                                        }).OrderBy(sub => sub.CreatedDate).ToArray();

            }
            catch(Exception ex)
            {
                var msg = ex.Message;
            }

            return sb;
        }
        public Master_Institution_SubscriptionValidityDTO SendEmailSMSForSubscription(Master_Institution_SubscriptionValidityDTO sb)
        {
            try
            {
                long MobileNo = _Context.Institution_MobileNo.FirstOrDefault(t => t.MI_Id == sb.MI_Id).MIMN_MobileNo;
                string Email_Id = _Context.Institution_EmailId.FirstOrDefault(t => t.MI_Id == sb.MI_Id).MIE_EmailId;

                SMS sms = new SMS(_Context);
                sms.sendSms(sb.MI_Id, MobileNo, "INSTITUTION", sb.MISV_Id);

                Email Email = new Email(_Context);

                string m = Email.sendmail(sb.MI_Id, Email_Id, "INSTITUTION", sb.MISV_Id);
            }
            catch (Exception ex)
            {

                throw;
            }

            return sb;
        }

        public Master_Institution_SubscriptionValidityDTO deleteSubscriptionrec(int id)
        {
            Master_Institution_SubscriptionValidityDTO instutSubdto = new Master_Institution_SubscriptionValidityDTO();

            try
            {
                var result = _Context.Master_Institution_SubscriptionValidity.Single(t => t.MISV_Id == id);
                if (result.MISV_ActiveFlag == true)
                {
                    result.MISV_ActiveFlag = false;
                    //added by 02/02/2017

                    result.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    _Context.SaveChanges();
                    instutSubdto.returnval = "true";
                }
                else
                { //added by 02/02/2017

                    result.UpdatedDate = DateTime.Now;
                    result.MISV_ActiveFlag = true;
                    _Context.Update(result);
                    _Context.SaveChanges();
                    instutSubdto.returnval = "false";
                }

                instutSubdto.subscriptionlist = (from sub in _Context.Master_Institution_SubscriptionValidity
                                        from im in _Context.Institute
                                        where sub.MI_Id == im.MI_Id
                                        select new Master_Institution_SubscriptionValidity
                                        {
                                            MISV_Id = sub.MISV_Id,
                                            MI_Id = sub.MI_Id,
                                            MISV_FromDate = sub.MISV_FromDate,
                                            MISV_ToDate = sub.MISV_ToDate,
                                            MISV_SubscriptionNo = sub.MISV_SubscriptionNo,
                                            MISV_SubscriptionType = sub.MISV_SubscriptionType,
                                            CreatedDate = sub.CreatedDate,
                                            UpdatedDate = sub.UpdatedDate,
                                            MISV_ActiveFlag = sub.MISV_ActiveFlag,
                                            Institution = sub.Institution
                                        }).OrderBy(sub => sub.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return instutSubdto;
        }


        //for Institution pagination
        public async Task<InstitutionDTO> Institutionsearchdata(SortingPagingInfoDTO spidto)
        {
            InstitutionDTO InstitutionDTO = new InstitutionDTO();
            try
            {
                await (InstitutionPaginationdetails(spidto, InstitutionDTO));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return InstitutionDTO;
        }

       

        public async Task<InstitutionDTO> InstitutionPaginationdetails(SortingPagingInfoDTO spidto, InstitutionDTO stu)
        {
            try
            {
                //var institutes = Enumerable.Empty<Institution>().AsQueryable();

                var institutes = (from mi in _Context.Institution
                                  from mo in _Context.Organisation
                                  where mi.MO_Id == mo.MO_Id
                                  select new Institution {
                                      MO_Id = mi.MO_Id,
                                      MI_Id= mi.MI_Id,
                                      MI_Name= mi.MI_Name,
                                      MI_Address1 =mi.MI_Address1,
                                      MI_Pincode =mi.MI_Pincode,
                                      MI_ActiveFlag = mi.MI_ActiveFlag,
                                      CreatedDate = mi.CreatedDate,
                                      UpdatedDate = mi.UpdatedDate,
                                      Organisation =mi.Organisation
                                  });

                if(institutes.Count()> 0)
                {
                    if (spidto.searchType =="All")
                    {
                        spidto.searchString = "All";
                    }

                    if (!string.IsNullOrEmpty(spidto.searchString))
                    {
                        spidto.CurrentPageIndex = 1;

                        int Active = 0;
                        if(spidto.searchString == "Activate")
                        {
                            Active = 1;
                        }
                        else if(spidto.searchString == "Deactivate")
                        {
                            Active = 0;
                        }

                        switch (spidto.searchType)
                        {
                            case "trust":
                                institutes = institutes.Where(d => d.Organisation.MO_Name.Contains(spidto.searchString));
                                break;
                            case "name":
                                institutes = institutes.Where(d => d.MI_Name.Contains(spidto.searchString));
                                break;
                            case "address":
                                institutes = institutes.Where(s => s.MI_Address1.Contains(spidto.searchString));
                                break;
                            case "pincode":
                                institutes = institutes.Where(s => s.MI_Pincode.Equals(Convert.ToInt32(spidto.searchString)));
                                break;
                            case "active/inactive":
                                institutes = institutes.Where(s => s.MI_ActiveFlag.Equals(Active));
                                break;
                            case "All":
                                institutes = institutes;
                                break;
                            default:
                                institutes = institutes.Where(s => s.Organisation.MO_Name.Contains(spidto.searchString) || s.MI_Name.Contains(spidto.searchString) || s.MI_Address1.Contains(spidto.searchString) || s.MI_ActiveFlag.Equals(Active));

                                //      institutes = institutes.Where(s => s.Institution.MI_Name.Contains(spidto.searchString) || s.MISV_ToDate.Contains(spidto.searchString) || s.MISV_SubscriptionType.Equals(spidto.searchString));
                                break;
                        }

                    }
                    switch (spidto.sortOrder)
                    {

                        case "trust_desc":
                            institutes = institutes.OrderByDescending(s => s.Organisation.MO_Name);
                            break;
                        case "trust":
                            institutes = institutes.OrderBy(s => s.Organisation.MO_Name);
                            break;
                        case "name":
                            institutes = institutes.OrderBy(s => s.MI_Name);
                            break;
                        case "name_desc":
                            institutes = institutes.OrderByDescending(s => s.MI_Name);
                            break;
                        case "address":
                            institutes = institutes.OrderBy(s => s.MI_Address1);
                            break;
                        case "address_desc":
                            institutes = institutes.OrderByDescending(s => s.MI_Address1);
                            break;
                        case "pincode":
                            institutes = institutes.OrderBy(s => s.MI_Pincode);
                            break;
                        case "pincode_desc":
                            institutes = institutes.OrderByDescending(s => s.MI_Pincode);
                            break;
                        default:
                            institutes = institutes.OrderBy(s => s.CreatedDate);
                            break;
                    }


                    PaginatedList<Institution> List = await PaginatedList<Institution>.CreateAsync(institutes.AsNoTracking(), spidto.CurrentPageIndex, spidto.PageSize);
                    stu.Institutionname = List.ToArray();

                    stu.instutePagination = spidto;
                    // stu.instutePagination.PageSize = pageSize;
                    stu.instutePagination.PageCount = List.TotalPages;
                    stu.instutePagination.CurrentPageIndex = List.PageIndex;
                    stu.instutePagination.TotalItems = List.TotalRecords;

                }

               
            }
            catch (Exception e)
            {
                string ex = e.Message;
            }


            return stu;
        }

        // for subscription search and pagination

        public async Task<Master_Institution_SubscriptionValidityDTO> Subscriptionsearchdata(SortingPagingInfoDTO spidto)
        {
            Master_Institution_SubscriptionValidityDTO misvdto = new Master_Institution_SubscriptionValidityDTO();
            try
            {
                await (SubscriptionPaginationdetails(spidto, misvdto));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return misvdto;
        }

        //for subscription pagination

        public async Task<Master_Institution_SubscriptionValidityDTO> SubscriptionPaginationdetails(SortingPagingInfoDTO spidto, Master_Institution_SubscriptionValidityDTO stu)
        {
            try
            {
                var institutes = (from sub in _Context.Master_Institution_SubscriptionValidity
                                  from im in _Context.Institute
                                  where sub.MI_Id == im.MI_Id
                                  select new Master_Institution_SubscriptionValidity {
                                      MI_Id = sub.MI_Id,
                                      MISV_Id=sub.MISV_Id,
                                      MISV_SubscriptionType =sub.MISV_SubscriptionType,
                                      MISV_SubscriptionNo =sub.MISV_SubscriptionNo,
                                      MISV_FromDate =sub.MISV_FromDate,
                                      MISV_ToDate = sub.MISV_ToDate,
                                      Institution=sub.Institution,
                                      CreatedDate = sub.CreatedDate,
                                      UpdatedDate = sub.UpdatedDate,
                                      MISV_ActiveFlag = sub.MISV_ActiveFlag
                                  });
                if (spidto.CurrentPageIndex == 0)
                {
                    spidto.CurrentPageIndex = 1;
                }

                bool Active = false;
                if (spidto.searchString == "Activate")
                {
                    Active = true;
                }
                else if (spidto.searchString == "Deactivate")
                {
                    Active = false;
                }

                if (institutes.Count()>0) 
                {
                    if (!string.IsNullOrEmpty(spidto.searchString))
                    {
                        spidto.CurrentPageIndex = 1;

                        switch (spidto.searchType)
                        {
                            case "fromdate":
                                institutes = institutes.Where(d => d.MISV_FromDate.Equals(spidto.searchString));
                                break;
                            case "todate":
                                institutes = institutes.Where(s => s.MISV_ToDate.Equals(spidto.searchString));
                                break;
                            case "type":
                                institutes = institutes.Where(s => s.MISV_SubscriptionType.Equals(spidto.searchString));
                                break;
                            case "name":
                                institutes = institutes.Where(s => s.Institution.MI_Name.Contains(spidto.searchString));
                                break;
                            case "subenabledisable":
                                institutes = institutes.Where(s => s.MISV_ActiveFlag.Equals(Active));
                                break;
                            default:
                                //institutes = (from sub in _Context.Master_Institution_SubscriptionValidity
                                //              from mi in _Context.Institution
                                //              where(sub.MI_Id== mi.MI_Id && mi.MI_Name.Contains(spidto.searchString))
                                //              select sub);
                                institutes = institutes.Where(s => s.Institution.MI_Name.Contains(spidto.searchString) || s.MISV_ToDate.Equals(spidto.searchString) || s.MISV_SubscriptionType.Equals(spidto.searchString)).OrderByDescending(sub => sub.CreatedDate);
                                break;
                        }
                    }
                    switch (spidto.sortOrder)
                    {
                        case "name_desc":
                            institutes = institutes.OrderByDescending(s => s.Institution.MI_Name);
                            break;
                        case "name":
                            institutes = institutes.OrderBy(s => s.Institution.MI_Name);
                            break;
                        case "fromdate":
                            institutes = institutes.OrderBy(s => s.MISV_FromDate);
                            break;
                        case "fromdate_desc":
                            institutes = institutes.OrderByDescending(s => s.MISV_FromDate);
                            break;
                        case "todate":
                            institutes = institutes.OrderBy(s => s.MISV_ToDate);
                            break;
                        case "todate_desc":
                            institutes = institutes.OrderByDescending(s => s.MISV_ToDate);
                            break;
                        case "type":
                            institutes = institutes.OrderBy(s => s.MISV_SubscriptionType);
                            break;
                        case "type_desc":
                            institutes = institutes.OrderByDescending(s => s.MISV_SubscriptionType);
                            break;
                        default:
                            institutes = institutes.OrderBy(sub => sub.CreatedDate);
                            break;
                    }


                    PaginatedList<Master_Institution_SubscriptionValidity> Listdata = await PaginatedList<Master_Institution_SubscriptionValidity>.CreateAsync(institutes.AsNoTracking(), spidto.CurrentPageIndex, spidto.PageSize);
                    stu.subscriptionlist = Listdata.ToArray();
                    stu.subscriptionPagination = spidto;
                    stu.subscriptionPagination.PageCount = Listdata.TotalPages;
                    stu.subscriptionPagination.CurrentPageIndex = Listdata.PageIndex;
                    stu.subscriptionPagination.TotalItems = Listdata.TotalRecords;
                }
               
            }
            catch (Exception e)
            {
                string ex = e.Message;
            }


            return stu;
        }


    }
}
