using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class GenConfigImpl : Interfaces.GenConfigInterface
    {
        private static ConcurrentDictionary<string, GeneralConfigDTO> _login =
              new ConcurrentDictionary<string, GeneralConfigDTO>();

        public DomainModelMsSqlServerContext _db;

        public GenConfigImpl(DomainModelMsSqlServerContext dmoc)
        {
            _db = dmoc;
        }
        public GeneralConfigDTO Configurationget(GeneralConfigDTO data)
        {
            GeneralConfigDTO cdto = new GeneralConfigDTO();
            try
            {
                cdto.AcademicList = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(d => d.ASMAY_Order).ToArray();
                cdto.mstConfigList = (from a in _db.Institute
                                      from b in _db.GenConfig
                                      where a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id
                                      select new GeneralConfigDTO
                                      {
                                          Instuitename = a.MI_Name,
                                          MI_Id = data.MI_Id,
                                          IVRMGC_Id = b.IVRMGC_Id,
                                      }).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }
        public GeneralConfigDTO geteditdata(GeneralConfigDTO data)
        {
            GeneralConfigDTO cdto = new GeneralConfigDTO();
            try
            {
                cdto.AcademicList = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(d => d.ASMAY_Order).ToArray();
                cdto.mstConfigList = _db.GenConfig.Where(t => t.IVRMGC_Id == data.IVRMGC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }

        public GeneralConfigDTO saveMasterConfig(GeneralConfigDTO mstConfigData)
        {
            try
            {
                GeneralConfigDMO mstConf = Mapper.Map<GeneralConfigDMO>(mstConfigData);

                if (mstConfigData.IVRMGC_Id > 0)
                {
                    mstConf.IVRMGC_MobileValOTPFlag = mstConfigData.IVRMGC_MobileValOTPFlag;
                    mstConf.IVRMGC_emailValOTPFlag = mstConfigData.IVRMGC_emailValOTPFlag;
                    mstConf.IVRMGC_StudentPhotoPath = mstConfigData.IVRMGC_StudentPhotoPath;
                    mstConf.IVRMGC_StaffPhotoPath = mstConfigData.IVRMGC_StaffPhotoPath;
                    mstConf.IVRMGC_ComTrasaNoFlag = mstConfigData.IVRMGC_ComTrasaNoFlag;
                    mstConf.IVRMGC_SMSDomain = mstConfigData.IVRMGC_SMSDomain;
                    mstConf.IVRMGC_SMSURL = mstConfigData.IVRMGC_SMSURL;
                    mstConf.IVRMGC_SMSUserName = mstConfigData.IVRMGC_SMSUserName;
                    mstConf.IVRMGC_SMSPassword = mstConfigData.IVRMGC_SMSPassword;
                    mstConf.IVRMGC_SMSSenderId = mstConfigData.IVRMGC_SMSSenderId;
                    mstConf.IVRMGC_SMSWorkingKey = mstConfigData.IVRMGC_SMSWorkingKey;
                    mstConf.IVRMGC_SMSFooter = mstConfigData.IVRMGC_SMSFooter;
                    mstConf.IVRMGC_SMSActiveFlag = mstConfigData.IVRMGC_SMSActiveFlag;
                    mstConf.IVRMGC_emailUserName = mstConfigData.IVRMGC_emailUserName;
                    mstConf.IVRMGC_emailPassword = mstConfigData.IVRMGC_emailPassword;
                    mstConf.IVRMGC_HostName = mstConfigData.IVRMGC_HostName;
                    mstConf.IVRMGC_PortNo = mstConfigData.IVRMGC_PortNo;
                    mstConf.IVRMGC_MailGenralDesc = mstConfigData.IVRMGC_MailGenralDesc;
                    mstConf.IVRMGC_Webiste = mstConfigData.IVRMGC_Webiste;
                    mstConf.IVRMGC_emailid = mstConfigData.IVRMGC_emailid;
                    mstConf.IVRMGC_emailFooter = mstConfigData.IVRMGC_emailFooter;
                    mstConf.IVRMGC_CCMail = mstConfigData.IVRMGC_CCMail;
                    mstConf.IVRMGC_BCCMail = mstConfigData.IVRMGC_BCCMail;
                    mstConf.IVRMGC_ToMail = mstConfigData.IVRMGC_ToMail;
                    mstConf.IVRMGC_EmailActiveFlag = mstConfigData.IVRMGC_EmailActiveFlag;
                    mstConf.IVRMGC_Pagination = mstConfigData.IVRMGC_Pagination;
                    mstConf.IVRMGC_ReminderDays = mstConfigData.IVRMGC_ReminderDays;
                    mstConf.IVRMGC_ClassCapacity = mstConfigData.IVRMGC_ClassCapacity;
                    mstConf.IVRMGC_SectionCapacity = mstConfigData.IVRMGC_SectionCapacity;
                    mstConf.IVRMGC_SCLockingPeriod = mstConfigData.IVRMGC_SCLockingPeriod;
                    mstConf.IVRMGC_SCActive = mstConfigData.IVRMGC_SCActive;
                    mstConf.IVRMGC_FPActive = mstConfigData.IVRMGC_FPActive;
                    mstConf.IVRMGC_OnlineFPActive = mstConfigData.IVRMGC_OnlineFPActive;
                    mstConf.IVRMGC_FaceReaderActive = mstConfigData.IVRMGC_FaceReaderActive;
                    mstConf.IVRMGC_DefaultStudentSelection = mstConfigData.IVRMGC_DefaultStudentSelection;
                    mstConf.IVRMGC_PagePagination = mstConfigData.IVRMGC_PagePagination;
                    mstConf.IVRMGC_ReportPagination = mstConfigData.IVRMGC_ReportPagination;
                    mstConf.IVRMGC_AdmnoColumnDisplay = mstConfigData.IVRMGC_AdmnoColumnDisplay;
                    mstConf.IVRMGC_RegnoColumnDisplay = mstConfigData.IVRMGC_RegnoColumnDisplay;
                    mstConf.IVRMGC_RollnoColumnDisplay = mstConfigData.IVRMGC_RollnoColumnDisplay;
                    mstConf.IVRMGC_FPLockingPeriod = mstConfigData.IVRMGC_FPLockingPeriod;
                    mstConf.IVRMGC_Disclaimer = mstConfigData.IVRMGC_Disclaimer;
                    mstConf.IVRMGC_PrincipalSign = mstConfigData.IVRMGC_PrincipalSign;
                    mstConf.IVRMGC_ManagerSign = mstConfigData.IVRMGC_ManagerSign;
                    mstConf.IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag = mstConfigData.IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag;
                    mstConf.IVRMGC_TransportRequired = mstConfigData.IVRMGC_TransportRequired;
                    mstConf.IVRMGC_OTPMobileNo = mstConfigData.IVRMGC_OTPMobileNo;
                    mstConf.IVRMGC_OTPMailId = mstConfigData.IVRMGC_OTPMailId;
                    //  mstConf.IVRMGC_OnlinePaymentCompany = mstConfigData.IVRMGC_OnlinePaymentCompany;
                    mstConf.IVRMGC_SendGrid_Key = mstConfigData.IVRMGC_SendGrid_Key;
                    mstConf.IVRMGC_Classwise_Payment = mstConfigData.IVRMGC_Classwise_Payment;
                    mstConf.IVRMGC_APIOrSMTPFlg = mstConfigData.IVRMGC_APIOrSMTPFlg;
                    mstConf.IVRMGC_FMSManualReferenceFlg = mstConfigData.IVRMGC_FMSManualReferenceFlg;

                    mstConf.IVRMGC_VMReminderFlag = mstConfigData.IVRMGC_VMReminderFlag;
                    mstConf.IVRMGC_VMReminderSchedule = mstConfigData.IVRMGC_VMReminderSchedule;
                    mstConf.IVRMGC_VMRepeatFlag = mstConfigData.IVRMGC_VMRepeatFlag;

                    mstConf.UpdatedDate = DateTime.Now;
                    _db.Update(mstConf);
                    _db.SaveChanges();
                    mstConfigData.returnval = "Successfully saved the record"; // To display the message 


                    var Emilcheck = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == mstConf.MI_Id).ToList();
                    if (Emilcheck.Count() > 0)
                    {
                        EMAIL_DETAILS_DMO Emailupdate = new EMAIL_DETAILS_DMO();
                        Emailupdate = _db.EMAIL_DETAILS_DMO.Single(t => t.MI_ID == mstConf.MI_Id);

                        Emailupdate.IVRMMD_HOSTNAME = mstConfigData.IVRMGC_HostName;
                        Emailupdate.IVRMMD_Mail_ID = mstConfigData.IVRMGC_emailUserName;
                        Emailupdate.IVRMMD_Mail_PASSWORD = mstConfigData.IVRMGC_emailPassword;
                        Emailupdate.IVRMMD_PORTNO = mstConfigData.IVRMGC_PortNo.ToString();
                        if ((mstConfigData.IVRMGC_CCMail != "" && mstConfigData.IVRMGC_CCMail != null) && (mstConfigData.IVRMGC_BCCMail == "" || mstConfigData.IVRMGC_BCCMail == null))
                        {
                            Emailupdate.IVRM_mailcc = mstConfigData.IVRMGC_CCMail;
                        }
                        else if ((mstConfigData.IVRMGC_CCMail != "" && mstConfigData.IVRMGC_CCMail != null) && (mstConfigData.IVRMGC_BCCMail != "" && mstConfigData.IVRMGC_BCCMail != null))
                        {
                            Emailupdate.IVRM_mailcc = mstConfigData.IVRMGC_CCMail + "," + mstConfigData.IVRMGC_BCCMail;
                        }
                        _db.Update(Emailupdate);
                        _db.SaveChanges();
                    }
                    else
                    {
                        EMAIL_DETAILS_DMO Emailupdate = new EMAIL_DETAILS_DMO();

                        Emailupdate.IVRMMD_HOSTNAME = mstConfigData.IVRMGC_HostName;
                        Emailupdate.IVRMMD_Mail_ID = mstConfigData.IVRMGC_emailUserName;
                        Emailupdate.IVRMMD_Mail_PASSWORD = mstConfigData.IVRMGC_emailPassword;
                        Emailupdate.IVRMMD_PORTNO = mstConfigData.IVRMGC_PortNo.ToString();
                        if ((mstConfigData.IVRMGC_CCMail != "" && mstConfigData.IVRMGC_CCMail != null) && (mstConfigData.IVRMGC_BCCMail == "" || mstConfigData.IVRMGC_BCCMail == null))
                        {
                            Emailupdate.IVRM_mailcc = mstConfigData.IVRMGC_CCMail;
                        }
                        else if ((mstConfigData.IVRMGC_CCMail != "" && mstConfigData.IVRMGC_CCMail != null) && (mstConfigData.IVRMGC_BCCMail != "" && mstConfigData.IVRMGC_BCCMail != null))
                        {
                            Emailupdate.IVRM_mailcc = mstConfigData.IVRMGC_CCMail + "," + mstConfigData.IVRMGC_BCCMail;
                        }
                        Emailupdate.MI_ID = mstConf.MI_Id;
                        _db.Add(Emailupdate);
                        _db.SaveChanges();

                    }

                    var Smscheck = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID == mstConf.MI_Id).ToList();
                    if (Smscheck.Count() > 0)
                    {
                        SMS_DETAILS_DMO Smsupdate = new SMS_DETAILS_DMO();
                        Smsupdate = _db.SMS_DETAILS_DMO.Single(t => t.MI_ID == mstConf.MI_Id);
                        Smsupdate.IVRMSD_DOMIN = mstConfigData.IVRMGC_SMSDomain;
                        Smsupdate.IVRMSD_URL = mstConfigData.IVRMGC_SMSURL;
                        Smsupdate.IVRMSD_USERNAME = mstConfigData.IVRMGC_SMSUserName;
                        Smsupdate.IVRMSD_PASSWORD = mstConfigData.IVRMGC_SMSPassword;
                        Smsupdate.IVRMSD_WORKINGKEY = mstConfigData.IVRMGC_SMSWorkingKey;
                        Smsupdate.IVRMSD_SENDERID = mstConfigData.IVRMGC_SMSSenderId;
                        _db.Update(Smsupdate);
                        _db.SaveChanges();
                    }
                    else
                    {
                        SMS_DETAILS_DMO Smsupdate = new SMS_DETAILS_DMO();
                        Smsupdate.IVRMSD_DOMIN = mstConfigData.IVRMGC_SMSDomain;
                        Smsupdate.IVRMSD_URL = mstConfigData.IVRMGC_SMSURL;
                        Smsupdate.IVRMSD_USERNAME = mstConfigData.IVRMGC_SMSUserName;
                        Smsupdate.IVRMSD_PASSWORD = mstConfigData.IVRMGC_SMSPassword;
                        Smsupdate.IVRMSD_WORKINGKEY = mstConfigData.IVRMGC_SMSWorkingKey;
                        Smsupdate.IVRMSD_SENDERID = mstConfigData.IVRMGC_SMSSenderId;
                        Smsupdate.MI_ID = mstConf.MI_Id;
                        _db.Add(Smsupdate);
                        _db.SaveChanges();
                    }

                }
                else
                {
                    var mstConfigList = _db.GenConfig.Where(t => t.MI_Id == mstConf.MI_Id).ToArray();
                    if (mstConfigList.Count() > 0)
                    {
                        mstConfigData.returnval = "Already Record Is Avilable Same Instuite !";
                    }
                    else
                    {
                        mstConf.CreatedDate = DateTime.Now;
                        mstConf.UpdatedDate = DateTime.Now;
                        _db.Add(mstConf);
                        var contactExists = _db.SaveChanges();
                        if (contactExists == 1)
                        {
                            mstConfigData.returnval = "Record Saved Successfully";
                        }
                        else
                        {
                            mstConfigData.returnval = "Record Not Saved";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return mstConfigData;
        }

    }
}
