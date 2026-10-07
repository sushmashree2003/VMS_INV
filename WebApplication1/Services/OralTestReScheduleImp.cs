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
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace WebApplication1.Services
{
    public class OralTestReScheduleImp : Interfaces.OralTestReScheduleInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
        new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly OralTestScheduleContext _OralTestScheduleContext;
        private readonly DomainModelMsSqlServerContext _context;
        public ScheduleReportContext _ScheduleReportContext;
        public StudentApplicationContext _StudentApplicationContext;
        public FeeGroupContext _feecontext;
        public OralTestReScheduleImp(StudentApplicationContext StudentApplicationContext, OralTestScheduleContext OralTestScheduleContext, DomainModelMsSqlServerContext db, FeeGroupContext feecontext, ScheduleReportContext DomainModelContext)
        {
            _OralTestScheduleContext = OralTestScheduleContext;
            _context = db;
            _StudentApplicationContext = StudentApplicationContext;
            _feecontext = feecontext;
            _ScheduleReportContext = DomainModelContext;
        }


        public StudentDetailsDTO GetOralTestScheduleData(StudentDetailsDTO StudentDetailsDTO)//int IVRMM_Id
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



                List<StudentApplication> Allname1 = new List<StudentApplication>();
                List<long> Allname2 = new List<long>();

                List<StudentApplication> Allname5 = new List<StudentApplication>();
                lorg5 = _OralTestScheduleContext.StudentApplication.ToList();



                List<long> Allname3 = new List<long>();
                Allname5 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.PASRAPS_ID== 787928).ToList();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(StudentDetailsDTO.MI_Id) && d.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.mstConfig = mstConfig.ToArray();

                if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                {

                    if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                    {
                        foreach (var a in lorg5)
                        {
                            StudentDetailsDTO.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == a.pasr_id).Count();

                            if (StudentDetailsDTO.payementcheck == 1)
                            {

                                Allname3.Add(a.pasr_id);

                            }
                        }

                        foreach (var a in lorg1)
                        {
                            Allname2.Add(a.PASR_Id);
                        }


                        Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)).ToList();

                        StudentDetailsDTO.studentDetails = Allname1.ToArray();

                    }
                }
                else
                {
                    foreach (var a in lorg1)
                    {
                        Allname2.Add(a.PASR_Id);
                    }

                    Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id)).ToList();

                    StudentDetailsDTO.studentDetails = Allname1.ToArray();
                }

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
                    //List<OralTestScheduleStudentInsertDMO> masters3 = new List<OralTestScheduleStudentInsertDMO>();
                    //masters3 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(masters[j].PAOTS_Id)).ToList();
                    var MM1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Single(t => t.PASR_Id == masters[j].PASR_Id);
                    MM1.PAOTSS_StatusFlag = 0;
                    _OralTestScheduleContext.Update(MM1);
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
                masters.FirstOrDefault().PAOTSS_StatusFlag = 0;
                _OralTestScheduleContext.Update(masters);
                _OralTestScheduleContext.SaveChanges();
            }
            else
            {

            }
            return OralTestScheduleDTO;
        }
        public async Task<OralTestScheduleDTO> OralTestScheduleData(OralTestScheduleDTO mas)
        {

            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            mas.PAOTS_Id = Convert.ToInt64(mas.PAOTS_ScheduleName);

            OralTestScheduleDMO MM = Mapper.Map<OralTestScheduleDMO>(mas);
            long paotss = 0;
            if (mas.PAOTS_Id > 0)
            {
                    int j = 0;
                    if (mas.SelectedStudentData != null)
                    {
                        while (j < mas.SelectedStudentData.Count())
                        {

                        var MM1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Single(t => t.PASR_Id == mas.SelectedStudentData[j].PASR_Id && t.PAOTSS_StatusFlag == 1);


                        var studentlist  = _OralTestScheduleContext.StudentApplication.Where(t => t.pasr_id == mas.SelectedStudentData[j].PASR_Id).ToList();

                        //added by 02/02/2017
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                        MM1.UpdatedDate = indianTime;
                        MM1.PAOTSS_StatusFlag =2;

                        var n = _OralTestScheduleContext.Update(MM1);
                        _OralTestScheduleContext.SaveChanges();

                        //added on 20/10/17

                        OralTestScheduleStudentInsertDMO MM2  = new OralTestScheduleStudentInsertDMO();
                        MM2.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                        MM2.PAOTS_Id = mas.PAOTS_Id;
                        //added by 02/02/2017
                        MM2.CreatedDate = DateTime.Now;
                        MM2.UpdatedDate = DateTime.Now;
                        MM2.PAOTSS_StatusFlag = 1;
                        MM2.PAOTSS_Date = mas.PAOTS_ScheduleDate;
                        if (mas.PAOTS_TPFlag == false)
                            {
                                string iDate = mas.PAOTS_ScheduleTime;
                                DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                var addmin = mas.PAOTS_TimePeriod * j;
                                var start_tm = oDate.AddMinutes(addmin);
                                string finalstattim = start_tm.ToString("H:mm");
                                var end_tm = start_tm.AddMinutes(mas.PAOTS_TimePeriod);
                                string finalendtim = end_tm.ToString("H:mm");
                            MM2.PAOTSS_Time = finalstattim;
                            MM2.PAOTSS_Time_To = finalendtim;
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
                            MM2.PAOTSS_Time = finalstattim;
                            MM2.PAOTSS_Time_To = finalendtim;
                            }

                            var n1 = _OralTestScheduleContext.Add(MM2);
                            _OralTestScheduleContext.SaveChanges();

                            if (n1 != null)
                            {
                                Email Email = new Email(_context);
                                string m = Email.sendmail(mas.MI_Id, studentlist.FirstOrDefault().PASR_emailId, "ORAL_TEST_SCHEDULE", MM2.PASR_Id);
                                paotss = MM2.PAOTSS_Id;

                                SMS sms = new SMS(_context);
                                string sss = await sms.sendSms(mas.MI_Id, studentlist.FirstOrDefault().PASR_MobileNo, "ORAL_TEST_SCHEDULE",MM2.PASR_Id);
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
                            string m = Email.sendmail(mas.MI_Id, mas.SelectedStudentDataForEdit[k].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                            SMS sms = new SMS(_context);
                            string sss = await sms.sendSms(mas.MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);


                            k++;
                        }
                    }
                
               
                mas.PAOTS_Id = 0;

            }
       

            return mas;
        }

        public async Task<ScheduleReportDTO> Getreportdetails(ScheduleReportDTO data)
        {
            try
            {
                string name = "";
                string IVRM_CLM_coloumn = "";

                using (var cmd = _ScheduleReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_schedulelist_students";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@schids",
                     SqlDbType.VarChar)
                    {
                        Value = data.schids
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {

                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Avialable" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.allreports = retObject.ToArray();


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


    }
}
