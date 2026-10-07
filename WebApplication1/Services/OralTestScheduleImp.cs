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
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using CommonLibrary;

namespace WebApplication1.Services
{
    public class OralTestScheduleImp : Interfaces.OralTestScheduleInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
        new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly OralTestScheduleContext _OralTestScheduleContext;
        private readonly DomainModelMsSqlServerContext _context;
        public StudentApplicationContext _StudentApplicationContext;
        public FeeGroupContext _feecontext;
        public OralTestScheduleImp(StudentApplicationContext StudentApplicationContext, OralTestScheduleContext OralTestScheduleContext, DomainModelMsSqlServerContext db, FeeGroupContext feecontext)
        {
            _OralTestScheduleContext = OralTestScheduleContext;
            _context = db;
            _StudentApplicationContext = StudentApplicationContext;
            _feecontext = feecontext;
        }


        public StudentDetailsDTO GetOralTestScheduleData(StudentDetailsDTO StudentDetailsDTO)//int IVRMM_Id
        {
            try
            {
                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                StudentDetailsDTO.ASMAY_Id = Acdemic_preadmission;


                //List<StudentApplication> lorg2 = new List<StudentApplication>();
                //List<StudentApplication> lorg5 = new List<StudentApplication>();
                //List<StudentApplication> lorgfinal = new List<StudentApplication>();
                //Array[] showdata1 = new Array[1];

                //List<OralTestScheduleStudentInsertDMO> lorg1 = new List<OralTestScheduleStudentInsertDMO>();
                //lorg1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.ToList();



                //List<StudentApplication> Allname1 = new List<StudentApplication>();
                //List<long> Allname2 = new List<long>();

                //List<StudentApplication> Allname5 = new List<StudentApplication>();
                //lorg5 = _OralTestScheduleContext.StudentApplication.ToList();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                StudentDetailsDTO.classlist = allclass.ToArray();

                //List<long> Allname3 = new List<long>();
                //Allname5 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.PASRAPS_ID== 787928).ToList();

                //List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                //mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(StudentDetailsDTO.MI_Id) && d.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                //StudentDetailsDTO.mstConfig = mstConfig.ToArray();

                //if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                //{

                //    if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                //    {
                //        foreach (var a in lorg5)
                //        {
                //            StudentDetailsDTO.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == a.pasr_id).Count();

                //            if (StudentDetailsDTO.payementcheck == 1)
                //            {

                //                Allname3.Add(a.pasr_id);

                //            }
                //        }

                //        foreach (var a in lorg1)
                //        {
                //            Allname2.Add(a.PASR_Id);
                //        }


                //        Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)).ToList();

                //        StudentDetailsDTO.studentDetails = Allname1.ToArray();

                //    }
                //}
                //else
                //{
                //    foreach (var a in lorg1)
                //    {
                //        Allname2.Add(a.PASR_Id);
                //    }

                //    Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id)).ToList();

                //    StudentDetailsDTO.studentDetails = Allname1.ToArray();
                //}

                Array[] showdata = new Array[50];
                List<OralTestScheduleDMO> Allname = new List<OralTestScheduleDMO>();
                Allname = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.OralTestSchedule = Allname.ToArray();


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return StudentDetailsDTO;
        }

        public OralTestScheduleDTO GetSelectedRowDetails(int ID)
        {

            OralTestScheduleDTO OralTestScheduleDTO = new OralTestScheduleDTO();
            List<OralTestScheduleDMO> lorg = new List<OralTestScheduleDMO>();
            lorg = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
            OralTestScheduleDTO.OralTestSchedule = lorg.ToArray();

            StudentDetailsDTO StudentDetailsDTO = new StudentDetailsDTO();

            List<OralTestScheduleStudentInsertDMO> lorg1 = new List<OralTestScheduleStudentInsertDMO>();
            List<StudentApplication> lorg2 = new List<StudentApplication>();
            List<StudentApplication> lorgfinal = new List<StudentApplication>();
            lorg1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
            int j = 0;
            while (j < lorg1.Count())
            {
                lorg2.AddRange(_OralTestScheduleContext.StudentApplication.Where(t => t.pasr_id.Equals(lorg1[j].PASR_Id)).ToList());
                j++;
            }

            OralTestScheduleDTO.SelectedStudentDetails = lorg2.ToArray();
            OralTestScheduleDTO.OralTestSchedule = lorg.ToArray();

            return OralTestScheduleDTO;
        }

        public StudentDetailsDTO GetSelectedStudentData(int ID)
        {
            //int ID1 = Convert.ToInt32(ID);
            StudentDetailsDTO StudentDetailsDTO = new StudentDetailsDTO();

            List<StudentApplication> lorg = new List<StudentApplication>();


            lorg = _OralTestScheduleContext.StudentApplication.Where(t => t.pasr_id.Equals(ID)).ToList();



            StudentDetailsDTO.studentDetails = lorg.ToArray();

            return StudentDetailsDTO;
        }

        public OralTestScheduleDTO OralTestScheduleDeletesData(int ID)
        {

            OralTestScheduleDTO OralTestScheduleDTO = new OralTestScheduleDTO();


            List<OralTestScheduleStudentInsertDMO> masters = new List<OralTestScheduleStudentInsertDMO>();
            masters = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();



            if (masters.Any())
            {

                int j = 0;
                while (j < masters.Count())
                {
                    List<OralTestScheduleStudentInsertDMO> masters3 = new List<OralTestScheduleStudentInsertDMO>();
                    masters3 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(masters[j].PAOTS_Id)).ToList();
                    _OralTestScheduleContext.Remove(masters3.ElementAt(0));
                    _OralTestScheduleContext.SaveChanges();

                    j++;
                }

                List<OralTestScheduleDMO> masters1 = new List<OralTestScheduleDMO>();
                masters1 = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();


                if (masters1.Any())
                {
                    _OralTestScheduleContext.Remove(masters1.ElementAt(0));
                    _OralTestScheduleContext.SaveChanges();

                }
                else
                {

                }
            }
            else
            {
                List<OralTestScheduleDMO> masters1 = new List<OralTestScheduleDMO>();
                masters1 = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
                if (masters1.Any())
                {
                    _OralTestScheduleContext.Remove(masters1.ElementAt(0));
                    _OralTestScheduleContext.SaveChanges();

                }
                else
                {

                }
            }


            return OralTestScheduleDTO;
        }

        public OralTestScheduleDTO OralTestScheduleDeletesStudentData(OralTestScheduleDTO OralTestScheduleDTO)
        {
            List<OralTestScheduleStudentInsertDMO> masters = new List<OralTestScheduleStudentInsertDMO>();
            masters = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PASR_Id.Equals(OralTestScheduleDTO.PASR_Id) && t.PAOTS_Id.Equals(OralTestScheduleDTO.PAOTS_Id)).ToList();
            if (masters.Any())
            {
                _OralTestScheduleContext.Remove(masters.ElementAt(0));
                _OralTestScheduleContext.SaveChanges();
            }
            else
            {

            }
            return OralTestScheduleDTO;
        }

        public StudentDetailsDTO classwisestudent(StudentDetailsDTO StudentDetailsDTO)
        {
            try
            {
                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                StudentDetailsDTO.ASMAY_Id = Acdemic_preadmission;


                List<StudentApplication> lorg2 = new List<StudentApplication>();
                List<StudentApplication> lorg5 = new List<StudentApplication>();
                List<StudentApplication> lorgfinal = new List<StudentApplication>();
                Array[] showdata1 = new Array[1];

                List<OralTestScheduleStudentInsertDMO> lorg1 = new List<OralTestScheduleStudentInsertDMO>();
                lorg1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.ToList();
                List<long> Allname2 = new List<long>();
                Allname2 = lorg1.Select(t => t.PASR_Id).ToList();

                List<StudentApplication> Allname1 = new List<StudentApplication>();

                List<long> studentlistof = new List<long>();

                List<StudentApplication> Allname5 = new List<StudentApplication>();
                if (StudentDetailsDTO.ASMCL_ID != 0)
                {
                    lorg5 = _OralTestScheduleContext.StudentApplication.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_Id == StudentDetailsDTO.ASMCL_ID && t.ASMAY_Id == StudentDetailsDTO.ASMAY_Id).ToList();
                }
                else
                {
                    lorg5 = _OralTestScheduleContext.StudentApplication.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMAY_Id == StudentDetailsDTO.ASMAY_Id).ToList();
                }


                studentlistof = lorg5.Select(t => t.pasr_id).ToList();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                StudentDetailsDTO.classlist = allclass.ToArray();


                List<long> Allname3 = new List<long>();
                Allname5 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.PASRAPS_ID == 787928).ToList();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(StudentDetailsDTO.MI_Id) && d.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.mstConfig = mstConfig.ToArray();

                if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                {

                    if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                    {
                        //foreach (var a in lorg5)
                        //{
                        Allname3 = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" &&
                        studentlistof.Contains(t.PASA_Id)).Select(t => t.PASA_Id).ToList();

                        //if (StudentDetailsDTO.payementcheck == 1)
                        //{

                        //    Allname3.Add(a.pasr_id);

                        //}
                        //}

                        //foreach (var a in lorg1)
                        //{
                        //    Allname2.Add(a.PASR_Id);
                        //}

                        if (StudentDetailsDTO.ASMCL_ID != 0)

                        {
                            Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)
                            && t.ASMCL_Id == StudentDetailsDTO.ASMCL_ID).ToList();

                            StudentDetailsDTO.studentDetails = Allname1.ToArray();
                        }
                        else
                        {
                            Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)).ToList();

                            StudentDetailsDTO.studentDetails = Allname1.ToArray();
                        }


                    }
                }
                else
                {
                    //foreach (var a in lorg1)
                    //{
                    //    Allname2.Add(a.PASR_Id);
                    //}
                    if (StudentDetailsDTO.ASMCL_ID != 0)
                    {
                        Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && t.ASMCL_Id == StudentDetailsDTO.ASMCL_ID).ToList();
                        StudentDetailsDTO.studentDetails = Allname1.ToArray();
                    }
                    else
                    {
                        Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id)).ToList();

                        StudentDetailsDTO.studentDetails = Allname1.ToArray();
                    }


                }

                Array[] showdata = new Array[50];
                List<OralTestScheduleDMO> Allname = new List<OralTestScheduleDMO>();
                Allname = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.OralTestSchedule = Allname.ToArray();

                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                StudentDetailsDTO.admissioncatdrpall = allclass.ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return StudentDetailsDTO;
        }
        public async Task<OralTestScheduleDTO> OralTestScheduleData(OralTestScheduleDTO mas)
        {

            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            long trnsno = 0;
            long senderid = mas.IVRMSTAUL_Id;


            // SMS NEW TABLES CODE START

            //SMS smstransno = new SMS(_context);
            //    //  public async Task<string> sendSmsnew(long MI_Id, long mobileNo, string Template, long UserID, string sms)
            //    trnsno = smstransno.getsmsno(mas.MI_Id);
            // string studentempflag = "STUDENT";

            // SMS NEW TABLES CODE END 

            OralTestScheduleDMO MM = Mapper.Map<OralTestScheduleDMO>(mas);
            long paotss = 0;
            if (mas.PAOTS_Id > 0)
            {


                var schedule = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id != mas.PAOTS_Id && t.PAOTS_ScheduleName == mas.PAOTS_ScheduleName && t.PAOTS_ScheduleDate == mas.PAOTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                if (schedule == 0)
                {
                    var result = _OralTestScheduleContext.OralTestScheduleDMO.Single(t => t.PAOTS_Id == mas.PAOTS_Id);
                    result.PAOTS_ScheduleName = mas.PAOTS_ScheduleName;
                    result.PAOTS_ScheduleDate = mas.PAOTS_ScheduleDate;
                    result.PAOTS_ScheduleTime = mas.PAOTS_ScheduleTime;



                    result.PAOTS_ScheduleTimeTo = mas.PAOTS_ScheduleTimeTo;
                    result.PAOTS_AM_PM = mas.PAOTS_AM_PM;
                    result.PAOTS_Remarks = mas.PAOTS_Remarks;
                    result.PAOTS_EntryDate = result.PAOTS_EntryDate;  //added by 02/02/2017
                    result.PAOTS_Skills = mas.PAOTS_Skills;
                    result.PAOTS_Superviser = mas.PAOTS_Superviser;

                    result.UpdatedDate = DateTime.Now;
                    _OralTestScheduleContext.Update(result);
                    _OralTestScheduleContext.SaveChanges();
                    mas.PAOTS_Id = MM.PAOTS_Id;
                    var othd = mas.PAOTS_ScheduleTime;

                    int j = 0;
                    if (mas.SelectedStudentData != null)
                    {
                        while (j < mas.SelectedStudentData.Count())
                        {
                            OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                            MM1.PAOTS_Id = mas.PAOTS_Id;
                            //added by 02/02/2017
                            MM1.CreatedDate = DateTime.Now;
                            MM1.UpdatedDate = DateTime.Now;

                            //added on 20/10/17
                            MM1.PAOTSS_Date = mas.PAOTS_ScheduleDate;
                            if (mas.PAOTS_TPFlag == false)
                            {
                                string iDate = mas.PAOTS_ScheduleTime;
                                DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                var addmin = mas.PAOTS_TimePeriod * j;
                                var start_tm = oDate.AddMinutes(addmin);
                                string finalstattim = start_tm.ToString("H:mm");
                                var end_tm = start_tm.AddMinutes(mas.PAOTS_TimePeriod);
                                string finalendtim = end_tm.ToString("H:mm");
                                MM1.PAOTSS_Time = finalstattim;
                                MM1.PAOTSS_Time_To = finalendtim;
                            }
                            else
                            {
                                string iDate = mas.PAOTS_ScheduleTime;
                                DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                var addhr = mas.PAOTS_TimePeriod * j;
                                var start_tm = oDate.AddHours(addhr);
                                string finalstattim = start_tm.ToString("H:mm");
                                var end_tm = start_tm.AddHours(mas.PAOTS_TimePeriod);
                                string finalendtim = end_tm.ToString("H:mm");
                                //var paotss_tm = oDate.AddHours(addhr);
                                //string finaltim = paotss_tm.ToString("H:mm");
                                MM1.PAOTSS_Time = finalstattim;
                                MM1.PAOTSS_Time_To = finalendtim;
                            }

                            var n = _OralTestScheduleContext.Add(MM1);
                            _OralTestScheduleContext.SaveChanges();

                            if (n != null)
                            {
                                Email Email = new Email(_context);
                                string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                paotss = MM1.PAOTSS_Id;

                                SMS sms = new SMS(_context);
                                string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                                SMS smsS = new SMS(_context);
                                string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);

                                // SMS NEW TABLES CODE START

                                //await sms.sendSmsnewTemplet(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);

                                // SMS NEW TABLES CODE END
                            }
                            j++;
                        }
                    }
                    int k = 0;
                    if (mas.SelectedStudentDataForEdit != null)
                    {
                        while (k < mas.SelectedStudentDataForEdit.Count())
                        {
                            OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentDataForEdit[k].PASR_Id;
                            MM1.PAOTS_Id = mas.PAOTS_Id;

                            Email Email = new Email(_context);
                            string m = Email.sendmail(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                            SMS sms = new SMS(_context);

                            string sss = await sms.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                            SMS smsS = new SMS(_context);
                            string sssS = await smsS.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);

                            // SMS NEW TABLES CODE START

                            //await sms.sendSmsnewTemplet(mas.SelectedStudentDataForEdit[j].MI_Id, mas.SelectedStudentDataForEdit[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);

                            // SMS NEW TABLES CODE END


                            k++;
                        }
                    }
                }
                else
                {
                    mas.returnvalue = "false";
                }


                mas.PAOTS_Id = 0;

            }
            else
            {
                try
                {


                    MM.IVRMSTAUL_Id = Convert.ToInt32(mas.IVRMSTAUL_Id);

                    MM.PAOTS_EntryDate = System.DateTime.Today.Date;

                    var schedule = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_ScheduleName == mas.PAOTS_ScheduleName && t.PAOTS_ScheduleDate == mas.PAOTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                    if (schedule == 0)
                    {
                        //added by 02/02/2017
                        MM.CreatedDate = DateTime.Now;
                        MM.UpdatedDate = DateTime.Now;
                        _OralTestScheduleContext.Add(MM);
                        _OralTestScheduleContext.SaveChanges();

                        mas.PAOTS_Id = MM.PAOTS_Id;

                        int j = 0;
                        if (mas.SelectedStudentData != null)
                        {
                            while (j < mas.SelectedStudentData.Count())
                            {
                                OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                                MM1.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                                MM1.PAOTS_Id = mas.PAOTS_Id;
                                MM1.PAOTSS_StatusFlag = 1;
                                //added by 02/02/2017
                                MM1.CreatedDate = DateTime.Now;
                                MM1.UpdatedDate = DateTime.Now;

                                //added on 20/10/17
                                MM1.PAOTSS_Date = mas.PAOTS_ScheduleDate;
                                if (mas.PAOTS_TPFlag == false)
                                {
                                    if(mas.autoschedule==true)
                                    {
                                        string iDate = mas.PAOTS_ScheduleTime;
                                        string todate = mas.PAOTS_ScheduleTimeTo;
                                        DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                        DateTime ToendDate = DateTime.ParseExact(todate, "HH:mm", null);
                                        string finalstattim = oDate.ToString("H:mm");
                                        string finalendtim = ToendDate.ToString("H:mm");
                                        MM1.PAOTSS_Time = finalstattim;
                                        MM1.PAOTSS_Time_To = finalendtim;
                                    }
                                    else
                                    {
                                        string iDate = mas.PAOTS_ScheduleTime;
                                        DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                        var addmin = mas.PAOTS_TimePeriod * j;
                                        var start_tm = oDate.AddMinutes(addmin);
                                        string finalstattim = start_tm.ToString("H:mm");
                                        var end_tm = start_tm.AddMinutes(mas.PAOTS_TimePeriod);
                                        string finalendtim = end_tm.ToString("H:mm");
                                        MM1.PAOTSS_Time = finalstattim;
                                        MM1.PAOTSS_Time_To = finalendtim;
                                    }
                                   
                                }
                                else
                                {
                                    string iDate = mas.PAOTS_ScheduleTime;
                                    DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                    var addhr = mas.PAOTS_TimePeriod * j;
                                    var start_tm = oDate.AddHours(addhr);
                                    string finalstattim = start_tm.ToString("H:mm");
                                    var end_tm = start_tm.AddHours(mas.PAOTS_TimePeriod);
                                    string finalendtim = end_tm.ToString("H:mm");
                                    //var paotss_tm = oDate.AddHours(addhr);
                                    //string finaltim = paotss_tm.ToString("H:mm");
                                    MM1.PAOTSS_Time = finalstattim;
                                    MM1.PAOTSS_Time_To = finalendtim;
                                }


                                _OralTestScheduleContext.Add(MM1);
                                int n = _OralTestScheduleContext.SaveChanges();
                                if (n > 0)
                                {

                                    Email Email = new Email(_context);
                                    string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                    SMS sms = new SMS(_context);
                                    string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                                    SMS smsS = new SMS(_context);
                                    string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);


                                    // SMS NEW TABLES CODE START

                                    //await sms.sendSmsnewTemplet(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);

                                    // SMS NEW TABLES CODE END
                                }
                                j++;
                            }
                        }



                    }
                    else
                    {
                        mas.returnvalue = "false";
                    }
                    mas.PAOTS_Id = 0;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }


            }

            return mas;
        }
    }
}
