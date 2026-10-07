using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Inventory;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PreadmissionDTOs.com.vaps.HRMS.MOMUpdateDTO;

namespace HRMSServiceHub.com.vaps.Services
{
    public class MOMUpdateImpl : Interfaces.MOMUpdateInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        ILogger<MOMUpdateImpl> _acdimpl;
        private readonly ILogger _log;
        public MOMUpdateImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }


        ////////////////////////==================transaction========///////////////////////////////////////////////////////////

        public MOMUpdateDTO oldmeetingdetail(MOMUpdateDTO data)
        {
            try
            {
                data.oldmeetdetail = _HRMSContext.MOMinutesDMO.Where(t => t.MOM_Name == data.MOM_Name && t.MOM_ParentId != 0).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public MOMUpdateDTO getonloaddata(MOMUpdateDTO data)
        {
            try
            {
                data.emplist = (from a in _HRMSContext.MasterEmployee
                                from b in _HRMSContext.HR_Master_Designation
                                from c in _HRMSContext.Emp_Email_Id
                                from d in _HRMSContext.Emp_MobileNo
                                from e in _HRMSContext.HR_Master_Department
                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMD_Id == e.HRMD_Id && b.HRMDES_Id == a.HRMDES_Id
                                && d.HRME_Id == a.HRME_Id && c.HRME_Id == a.HRME_Id && c.HRMEM_DeFaultFlag == "default" && d.HRMEMNO_DeFaultFlag == "default")
                                select new MOMUpdateDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,
                                    HRMDES_Id = b.HRMDES_Id,
                                    HRMD_Id = e.HRMD_Id,
                                    HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    HRME_EmailId = c.HRMEM_EmailId,
                                    HRME_MobileNo = d.HRMEMNO_MobileNo,
                                }).Distinct().OrderBy(T => T.HRME_Id).ToArray();

                //data.departmentdropdown = (from a in _HRMSContext.Institution
                //                           from b in _HRMSContext.HR_Master_Department
                //                           where (b.HRMD_ActiveFlag == true && a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1)
                //                           select new MOMUpdateDTO
                //                           {
                //                               HRMD_Id = b.HRMD_Id,
                //                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                //                               MI_Name = a.MI_Name
                //                           }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();
                data.venue = _HRMSContext.MasterVenues_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMVNE_ActiveFlag == true).ToArray();
                data.griddata = _HRMSContext.MOMinutesDMO.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t => t.MOM_Id).ToArray();


                ///Chenges by Aman because it's showing only one MD sir name now i have to show all employees name
                //data.approvedata = (from a in _HRMSContext.applicationuser
                //                    from b in _HRMSContext.IVRM_Staff_User_Login
                //                    from c in _HRMSContext.ApplicationUserRoleDMO
                //                    from d in _HRMSContext.applicationRole
                //                    where (b.Id == a.Id && c.UserId == a.Id && d.Id == c.RoleId && b.MI_Id == data.MI_Id && d.Name == "ADMIN")
                //                    select new MOMUpdateDTO
                //                    {
                //                        IVRMSTAUL_UserName = b.IVRMSTAUL_UserName,
                //                    }).Distinct().OrderBy(T => T.HRME_Id).ToArray();


                //data.approvedata = (from a in _HRMSContext.applicationuser
                //                    from b in _HRMSContext.IVRM_Staff_User_Login
                //                    from c in _HRMSContext.ApplicationUserRoleDMO
                //                    from d in _HRMSContext.applicationRole
                //                    where (b.Id == a.Id && c.UserId == a.Id && d.Id == c.RoleId && b.MI_Id == data.MI_Id)
                //                    select new MOMUpdateDTO
                //                    {
                //                        IVRMSTAUL_UserName = b.IVRMSTAUL_UserName,
                //                    }).Distinct().OrderBy(T => T.HRME_Id).ToArray();

                data.approvedata = (from a in _HRMSContext.applicationuser
                                    from b in _HRMSContext.IVRM_Staff_User_Login
                                    from c in _HRMSContext.ApplicationUserRoleDMO
                                    from d in _HRMSContext.applicationRole
                                    from uli in _HRMSContext.UserRoleWithInstituteDMO
                                    from emp in _HRMSContext.MasterEmployee
                                    where
                                        b.Id == a.Id &&
                                        c.UserId == a.Id &&
                                        d.Id == c.RoleId &&
                                        uli.Id == a.Id &&
                                        emp.HRME_Id == b.Emp_Code &&
                                        b.IVRMSTAUL_ActiveFlag == 1 &&
                                        uli.Activeflag == 1 &&
                                        emp.HRME_ActiveFlag == true &&
                                        emp.HRME_LeftFlag == false      
                                    select new MOMUpdateDTO
                                    {
                                        IVRMSTAUL_UserName = b.IVRMSTAUL_UserName,
                                        HRME_Id = emp.HRME_Id
                                    }).Distinct().OrderBy(T => T.HRME_Id).ToArray();



                //data.approvedata = _HRMSContext.IVRM_Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.userId).ToArray();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MOMINUTESNAME";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.meetingname = retObject.ToArray();
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
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public MOMUpdateDTO savetransaction(MOMUpdateDTO data)
        {

            try
            {
                long MOM_Ids = 0;
                if (data.MOM_Id > 0)
                {
                    var updateaction1 = _HRMSContext.MOMinutesDMO.Where(t => t.MOM_Id == data.MOM_Id).ToList();
                    if (updateaction1.Count > 0)
                    {
                        foreach (var dupoObj in updateaction1)
                        {
                            dupoObj.MOM_ParentId = data.MOM_Id;
                            dupoObj.MI_Id = data.MI_Id;
                            dupoObj.MOM_Agenda = data.MOM_Agenda;
                            dupoObj.MOM_Name = data.MOM_Name;
                            dupoObj.MOM_Venue = data.MOM_Venue;
                            dupoObj.MOM_EXTINTFlag = data.MOM_EXTINTFlag;
                            dupoObj.MOM_Date = data.MOM_Date;
                            dupoObj.MOM_StartTime = data.MOM_StartTime;
                            dupoObj.MOM_EndTime = data.MOM_EndTime;
                            dupoObj.MOM_Status = "Pending";
                            dupoObj.MOM_UpdatedDate = DateTime.Now;
                            dupoObj.MOM_ActiveFlag = true;
                            dupoObj.MOM_UpdatedBy = data.userId;
                            _HRMSContext.Update(dupoObj);
                            _HRMSContext.SaveChanges();
                            MOM_Ids = dupoObj.MOM_Id;
                        }
                        //var updateaction12 = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOM_Id == data.MOM_Id).ToList();
                        //if (updateaction12.Count > 0)
                        //{
                        //    foreach (var emailemp in updateaction12)
                        //    {
                        //        var ccemail = "";
                        //        if (emailemp.MOMPRT_Email.Length > 0)
                        //        {
                        //            sendEmail(data.MI_Id, "MOMPOSTPONE", MOM_Ids, "vandana@vapstech.com", ccemail, data.MOM_Name);
                        //        }
                        //    }
                        //}
                    }
                    //else
                    //{
                    //    MOMinutesDMO dmoObj = new MOMinutesDMO();
                    //    //dmoObj.MOM_Id = data.MOM_Id;
                    //    dmoObj.MOM_ParentId = data.MOM_Id;
                    //    dmoObj.MI_Id = data.MI_Id;
                    //    dmoObj.MOM_Agenda = data.MOM_Agenda;
                    //    dmoObj.MOM_Name = data.MOM_Name;
                    //    dmoObj.MOM_Venue = data.MOM_Venue;
                    //    dmoObj.MOM_EXTINTFlag = data.MOM_EXTINTFlag;
                    //    dmoObj.MOM_Date = data.MOM_Date;
                    //    dmoObj.MOM_StartTime = data.MOM_StartTime;
                    //    dmoObj.MOM_EndTime = data.MOM_EndTime;
                    //    dmoObj.MOM_Status = "Pending";
                    //    dmoObj.MOM_UpdatedDate = DateTime.Now;
                    //    dmoObj.MOM_CreatedDate = DateTime.Now;
                    //    dmoObj.MOM_ActiveFlag = true;
                    //    dmoObj.MOM_CreatedBy = data.userId;
                    //    dmoObj.MOM_UpdatedBy = data.userId;
                    //    _HRMSContext.Add(dmoObj);
                    //    _HRMSContext.SaveChanges();
                    //    MOM_Ids = dmoObj.MOM_Id;
                    //}

                }
                else
                {
                    var MOM_Id = _HRMSContext.MOMinutesDMO.Where(t => t.MOM_Name == data.MOM_Name && t.MOM_ActiveFlag == true && t.MOM_ParentId == 0).Select(t => t.MOM_Id).FirstOrDefault();

                    MOMinutesDMO Obj = new MOMinutesDMO();
                    Obj.MI_Id = data.MI_Id;
                    Obj.MOM_Agenda = data.MOM_Agenda;
                    Obj.MOM_Name = data.MOM_Name;
                    Obj.MOM_StartTime = data.MOM_StartTime;
                    Obj.MOM_EndTime = data.MOM_EndTime;
                    Obj.MOM_Venue = data.MOM_Venue;
                    Obj.MOM_EXTINTFlag = data.MOM_EXTINTFlag;
                    Obj.MOM_Status = "Pending";
                    Obj.MOM_Date = data.MOM_Date;
                    Obj.MOM_ParentId = MOM_Id;
                    Obj.MOM_ActiveFlag = true;
                    Obj.MOM_UpdatedDate = DateTime.Now;
                    Obj.MOM_CreatedDate = DateTime.Now;
                    Obj.MOM_CreatedBy = data.userId;
                    Obj.MOM_UpdatedBy = data.userId;
                    _HRMSContext.Add(Obj);
                    _HRMSContext.SaveChanges();
                    MOM_Ids = Obj.MOM_Id;

                }

                ///This code i commented because it's sending multiples mail to CC mail so that reason i commented and in below i wirtten code only one mail it's sending to CC mail all participent's in List 

                //if (data.Participent_Array != null)
                //{


                //    foreach (var item in data.Participent_Array)
                //    {
                //        var newparticipents = _HRMSContext.MOMParticipantsDMO.Where(t => (t.MOM_Id == MOM_Ids && t.HRME_Id == item.HRME_Id && t.MOMPRT_ParticipantsName == item.MOMPRT_ParticipantsName)).Count();
                //        if (newparticipents == 0)
                //        {
                //            MOMParticipantsDMO obb = new MOMParticipantsDMO();
                //            //obb.MOMPRT_Id = data.MOMPRT_Id;
                //            obb.MOM_Id = MOM_Ids;
                //            obb.HRME_Id = item.HRME_Id;
                //            obb.MOMPRT_ParticipantsName = item.MOMPRT_ParticipantsName;
                //            obb.MOMPRT_Email = item.MOMPRT_Email;
                //            obb.MOMPRT_MobileNo = item.MOMPRT_MobileNo;
                //            obb.MOMPRT_ActiveFlag = true;
                //            obb.MOMPRT_Designation = item.MOMPRT_Designation;
                //            obb.MOMPRT_EXTINTFlag = item.MOMPRT_EXTINTFlag;
                //            obb.MOMPRT_CreatedBy = data.userId;
                //            obb.MOMPRT_UpdatedBy = data.userId;
                //            obb.MOMPRT_CreatedDate = DateTime.Now;
                //            obb.MOMPRT_UpdatedDate = DateTime.Now;
                //            _HRMSContext.Add(obb);
                //            _HRMSContext.SaveChanges();
                //        }
                //        if (newparticipents > 0)
                //        {
                //            MOMParticipantsDMO obbjj = new MOMParticipantsDMO();
                //            obbjj.MOM_Id = MOM_Ids;
                //            obbjj.HRME_Id = item.HRME_Id;
                //            obbjj.MOMPRT_ParticipantsName = item.MOMPRT_ParticipantsName;
                //            obbjj.MOMPRT_Email = item.MOMPRT_Email;
                //            obbjj.MOMPRT_MobileNo = item.MOMPRT_MobileNo;
                //            obbjj.MOMPRT_ActiveFlag = true;
                //            obbjj.MOMPRT_Designation = item.MOMPRT_Designation;
                //            obbjj.MOMPRT_EXTINTFlag = item.MOMPRT_EXTINTFlag;
                //            obbjj.MOMPRT_CreatedBy = data.userId;
                //            obbjj.MOMPRT_UpdatedBy = data.userId;
                //            obbjj.MOMPRT_CreatedDate = DateTime.Now;
                //            obbjj.MOMPRT_UpdatedDate = DateTime.Now;
                //            _HRMSContext.Update(obbjj);
                //            _HRMSContext.SaveChanges();
                //        }

                //        // Added By Aman

                //        //var ccemail = "";
                //        //var ccemail = "aman.k@vapstech.com";
                //        //item.MOMPRT_Email = "aman.k@vapstech.com";

                //        var template = _HRMSContext.smsEmailSetting.Where(e => e.ISES_Template_Name == "MOMCreated_new_Am" && e.MI_Id == 17 && e.ISES_MailActiveFlag == true).ToList();
                //        string ccemail = template[0].ISES_MailCCId;


                //        if (item.MOMPRT_Email.Length > 0)
                //        {
                //            sendEmail(data.MI_Id, "MOMCreated_new_Am", MOM_Ids, item.MOMPRT_Email, ccemail, data.MOM_Name);
                //        }


                //    }
                //}

                //// Added by Aman for send multiples mail at a one time to all participent's with CC mail
                if (data.Participent_Array != null)
                {
                    string allEmails = "";

                    foreach (var item in data.Participent_Array)
                    {
                        var newparticipents = _HRMSContext.MOMParticipantsDMO.Where(t => (t.MOM_Id == MOM_Ids && t.HRME_Id == item.HRME_Id && t.MOMPRT_ParticipantsName == item.MOMPRT_ParticipantsName)).Count();

                        if (newparticipents == 0)
                        {
                            MOMParticipantsDMO obb = new MOMParticipantsDMO
                            {
                                MOM_Id = MOM_Ids,
                                HRME_Id = item.HRME_Id,
                                MOMPRT_ParticipantsName = item.MOMPRT_ParticipantsName,
                                MOMPRT_Email = item.MOMPRT_Email,
                                MOMPRT_MobileNo = item.MOMPRT_MobileNo,
                                MOMPRT_ActiveFlag = true,
                                MOMPRT_Designation = item.MOMPRT_Designation,
                                MOMPRT_EXTINTFlag = item.MOMPRT_EXTINTFlag,
                                MOMPRT_CreatedBy = data.userId,
                                MOMPRT_UpdatedBy = data.userId,
                                MOMPRT_CreatedDate = DateTime.Now,
                                MOMPRT_UpdatedDate = DateTime.Now
                            };

                            _HRMSContext.Add(obb);
                            _HRMSContext.SaveChanges();
                        }
                        else if (newparticipents > 0)
                        {
                            MOMParticipantsDMO obbjj = _HRMSContext.MOMParticipantsDMO
                                .FirstOrDefault(p => p.MOM_Id == MOM_Ids && p.HRME_Id == item.HRME_Id && p.MOMPRT_ParticipantsName == item.MOMPRT_ParticipantsName);

                            if (obbjj != null)
                            {
                                obbjj.MOMPRT_Email = item.MOMPRT_Email;
                                obbjj.MOMPRT_MobileNo = item.MOMPRT_MobileNo;
                                obbjj.MOMPRT_Designation = item.MOMPRT_Designation;
                                obbjj.MOMPRT_EXTINTFlag = item.MOMPRT_EXTINTFlag;
                                obbjj.MOMPRT_UpdatedBy = data.userId;
                                obbjj.MOMPRT_UpdatedDate = DateTime.Now;
                                _HRMSContext.Update(obbjj);
                                _HRMSContext.SaveChanges();
                            }
                        }

                        if (!string.IsNullOrEmpty(item.MOMPRT_Email))
                        {
                            if (string.IsNullOrEmpty(allEmails))
                            {
                                allEmails = item.MOMPRT_Email;
                            }
                            else
                            {
                                allEmails += "," + item.MOMPRT_Email;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(allEmails))
                    {
                        var template = _HRMSContext.smsEmailSetting
                            .Where(e => e.ISES_Template_Name == "MOMCreated_new_Am" && e.MI_Id == 17 && e.ISES_MailActiveFlag == true)
                            .ToList();

                        string ccemail = template[0].ISES_MailCCId;

                        sendEmail(data.MI_Id, "MOMCreated_new_Am", MOM_Ids, allEmails, ccemail, data.MOM_Name);
                    }
                }

                //if (data.ParticipentsArray.Length > 0)
                //{
                //    foreach (var item1 in data.ParticipentsArray)
                //    {
                //        var newparticipents1 = _HRMSContext.MOMParticipantsDMO.Where(t => (t.MOM_Id == MOM_Ids && t.HRME_Id == item1.HRME_Id && t.MOMPRT_ParticipantsName == item1.MOMPRT_ParticipantsName && t.MOMPRT_MobileNo == item1.MOMPRT_MobileNo)).Count();
                //        if (newparticipents1 == 0)
                //        {
                //            MOMParticipantsDMO obbj = new MOMParticipantsDMO();
                //            // obbj.MOMPRT_Id = data.MOMPRT_Id;
                //            obbj.MOM_Id = MOM_Ids;
                //            obbj.HRME_Id = item1.HRME_Id;
                //            obbj.MOMPRT_ParticipantsName = item1.MOMPRT_ParticipantsName;
                //            obbj.MOMPRT_Email = item1.MOMPRT_Email;
                //            obbj.MOMPRT_MobileNo = item1.MOMPRT_MobileNo;
                //            obbj.MOMPRT_ActiveFlag = true;
                //            obbj.MOMPRT_Designation = item1.MOMPRT_Designation;
                //            obbj.MOMPRT_EXTINTFlag = item1.MOMPRT_EXTINTFlag;
                //            obbj.MOMPRT_CreatedBy = data.userId;
                //            obbj.MOMPRT_UpdatedBy = data.userId;
                //            obbj.MOMPRT_CreatedDate = DateTime.Now;
                //            obbj.MOMPRT_UpdatedDate = DateTime.Now;
                //            _HRMSContext.Add(obbj);
                //            _HRMSContext.SaveChanges();
                //        }
                //        if (newparticipents1 > 0)
                //        {
                //            MOMParticipantsDMO objj = new MOMParticipantsDMO();
                //            objj.MOM_Id = MOM_Ids;
                //            objj.HRME_Id = item1.HRME_Id;
                //            objj.MOMPRT_ParticipantsName = item1.MOMPRT_ParticipantsName;
                //            objj.MOMPRT_Email = item1.MOMPRT_Email;
                //            objj.MOMPRT_MobileNo = item1.MOMPRT_MobileNo;
                //            objj.MOMPRT_ActiveFlag = true;
                //            objj.MOMPRT_Designation = item1.MOMPRT_Designation;
                //            objj.MOMPRT_EXTINTFlag = item1.MOMPRT_EXTINTFlag;
                //            objj.MOMPRT_CreatedBy = data.userId;
                //            objj.MOMPRT_UpdatedBy = data.userId;
                //            objj.MOMPRT_CreatedDate = DateTime.Now;
                //            objj.MOMPRT_UpdatedDate = DateTime.Now;
                //            _HRMSContext.Update(objj);
                //            _HRMSContext.SaveChanges();
                //        }
                //        //Added by Aman
                //        //var ccemail = "";
                //        //item1.MOMPRT_Email = "aman.k@vapstech.com";
                //        var template = _HRMSContext.smsEmailSetting.Where(e => e.ISES_Template_Name == "MOMCreated_new_Am" && e.MI_Id == 17 && e.ISES_MailActiveFlag == true).ToList();
                //        string ccemail = template[0].ISES_MailCCId;
                //        if (item1.MOMPRT_Email.Length > 0)
                //        {

                //            sendEmail(data.MI_Id, "MOMCreated_new_Am", MOM_Ids, item1.MOMPRT_Email, ccemail, data.MOM_Name);

                //        }

                //    }
                //}


                if (data.ParticipentsArray.Length > 0)
                {
                    string allEmails = "";

                    foreach (var item1 in data.ParticipentsArray)
                    {
                        var newparticipents1 = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOM_Id == MOM_Ids && t.HRME_Id == item1.HRME_Id && t.MOMPRT_ParticipantsName == item1.MOMPRT_ParticipantsName && t.MOMPRT_MobileNo == item1.MOMPRT_MobileNo).Count();

                        if (newparticipents1 == 0)
                        {
                            MOMParticipantsDMO obbj = new MOMParticipantsDMO
                            {
                                MOM_Id = MOM_Ids,
                                HRME_Id = item1.HRME_Id,
                                MOMPRT_ParticipantsName = item1.MOMPRT_ParticipantsName,
                                MOMPRT_Email = item1.MOMPRT_Email,
                                MOMPRT_MobileNo = item1.MOMPRT_MobileNo,
                                MOMPRT_ActiveFlag = true,
                                MOMPRT_Designation = item1.MOMPRT_Designation,
                                MOMPRT_EXTINTFlag = item1.MOMPRT_EXTINTFlag,
                                MOMPRT_CreatedBy = data.userId,
                                MOMPRT_UpdatedBy = data.userId,
                                MOMPRT_CreatedDate = DateTime.Now,
                                MOMPRT_UpdatedDate = DateTime.Now
                            };

                            _HRMSContext.Add(obbj);
                            _HRMSContext.SaveChanges();
                        }
                        else if (newparticipents1 > 0)
                        {
                            MOMParticipantsDMO objj = _HRMSContext.MOMParticipantsDMO
                                .FirstOrDefault(p => p.MOM_Id == MOM_Ids && p.HRME_Id == item1.HRME_Id && p.MOMPRT_ParticipantsName == item1.MOMPRT_ParticipantsName);

                            if (objj != null)
                            {
                                objj.MOMPRT_Email = item1.MOMPRT_Email;
                                objj.MOMPRT_MobileNo = item1.MOMPRT_MobileNo;
                                objj.MOMPRT_Designation = item1.MOMPRT_Designation;
                                objj.MOMPRT_EXTINTFlag = item1.MOMPRT_EXTINTFlag;
                                objj.MOMPRT_UpdatedBy = data.userId;
                                objj.MOMPRT_UpdatedDate = DateTime.Now;
                                _HRMSContext.Update(objj);
                                _HRMSContext.SaveChanges();
                            }
                        }

                        if (!string.IsNullOrEmpty(item1.MOMPRT_Email))
                        {
                            if (string.IsNullOrEmpty(allEmails))
                            {
                                allEmails = item1.MOMPRT_Email;
                            }
                            else
                            {
                                allEmails += "," + item1.MOMPRT_Email;
                            }
                        }
                    }

                    // If any emails have been collected, send them all together
                    if (!string.IsNullOrEmpty(allEmails))
                    {
                        var template = _HRMSContext.smsEmailSetting
                            .Where(e => e.ISES_Template_Name == "MOMCreated_new_Am" && e.MI_Id == 17 && e.ISES_MailActiveFlag == true)
                            .ToList();

                        string ccemail = template[0].ISES_MailCCId;

                        sendEmail(data.MI_Id, "MOMCreated_new_Am", MOM_Ids, allEmails, ccemail, data.MOM_Name);
                    }
                }


                if (data.FilePath_Array.Length > 0)
                {
                    foreach (var filedata in data.FilePath_Array)
                    {
                        MOMFileDMO file = new MOMFileDMO();
                        file.MOMFL_FileName = filedata.FileName;
                        file.MOMFL_FilePath = filedata.FilePath;
                        file.MOM_Id = MOM_Ids;
                        file.MOMFL_ActiveFlag = true;
                        file.MOMFL_CreatedBy = data.userId;
                        file.MOMFL_UpdatedBy = data.userId;
                        file.MOMFL_CreatedDate = DateTime.Now;
                        file.MOMFL_UpdatedDate = DateTime.Now;
                        _HRMSContext.Add(file);
                        _HRMSContext.SaveChanges();
                    }

                }
                if (data.FilePath_Array.Length >= 0)
                {
                    data.retrunMsg = "Add";
                }
                else
                {
                    data.retrunMsg = "false";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

                data.retrunMsg = "Error occured";
            }

            return data;
        }



        public MOMUpdateDTO viewflies(MOMUpdateDTO data)
        {

            try
            {
                data.getfiledetails = _HRMSContext.MOMFileDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMFL_ActiveFlag == true).ToArray();

                data.getParticipentsdetails = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOM_Id == data.MOM_Id).ToArray();
                data.emplist = (from a in _HRMSContext.MasterEmployee
                                from b in _HRMSContext.HR_Master_Designation
                                from c in _HRMSContext.Emp_Email_Id
                                from d in _HRMSContext.Emp_MobileNo
                                from e in _HRMSContext.HR_Master_Department
                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMD_Id == e.HRMD_Id && b.HRMDES_Id == a.HRMDES_Id
                                && d.HRME_Id == a.HRME_Id && c.HRME_Id == a.HRME_Id && c.HRMEM_DeFaultFlag == "default" && d.HRMEMNO_DeFaultFlag == "default")
                                select new MOMUpdateDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,
                                    HRMDES_Id = b.HRMDES_Id,
                                    HRMD_Id = e.HRMD_Id,
                                    HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    HRME_EmailId = c.HRMEM_EmailId,
                                    HRME_MobileNo = d.HRMEMNO_MobileNo,
                                }).Distinct().OrderBy(T => T.HRME_Id).ToArray();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public MOMUpdateDTO deactivate(MOMUpdateDTO data)
        {
            try
            {
                var result = _HRMSContext.MOMinutesDMO.Single(t => t.MOM_Id == data.MOM_Id && t.MI_Id == data.MI_Id);

                if (result.MOM_ActiveFlag == true)
                {
                    result.MOM_ActiveFlag = false;
                }
                else if (result.MOM_ActiveFlag == false)
                {
                    result.MOM_ActiveFlag = true;
                }
                result.MOM_UpdatedDate = DateTime.Now;
                _HRMSContext.Update(result);

                int rowAffected = _HRMSContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.retrunMsg = "true";
                }
                else
                {
                    data.retrunMsg = "false";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public MOMUpdateDTO Edittrans(MOMUpdateDTO data)
        {

            try
            {
                data.edittransdata = _HRMSContext.MOMinutesDMO.Where(t => t.MI_Id == data.MI_Id && t.MOM_Id == data.MOM_Id).ToArray();
                data.editempdata = _HRMSContext.MOMParticipantsDMO.Where(a => a.MOM_Id == data.MOM_Id).ToArray();
                data.approvedata = (from a in _HRMSContext.applicationuser
                                    from b in _HRMSContext.IVRM_Staff_User_Login
                                    from c in _HRMSContext.ApplicationUserRoleDMO
                                    from d in _HRMSContext.applicationRole
                                    where (b.Id == a.Id && c.UserId == a.Id && d.Id == c.RoleId && b.MI_Id == data.MI_Id && d.Name == "ADMIN")
                                    select new MOMUpdateDTO
                                    {
                                        IVRMSTAUL_UserName = b.IVRMSTAUL_UserName,
                                    }).Distinct().OrderBy(T => T.HRME_Id).ToArray();
            }
            catch (Exception ex)
            {

            }
            return data;
        }
        ///=======================Update=====================/////////////////////////////////////////////////////////////////////////////////////////////////
        public MOMUpdateDTO getDetails(MOMUpdateDTO data)
        {
            try
            {

                data.griddata = _HRMSContext.MOMinutesDMO.Where(t => t.MI_Id == data.MI_Id && t.MOM_ActiveFlag == true && t.MOM_Status != "Pending").OrderByDescending(t => t.MOM_Id).ToArray();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Minutes_Of_Meeting_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.aprovedlist = retObject.ToArray();
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
                data.retrunMsg = "Error occured";
            }

            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "FMS_Employee_Details";
                cmd.CommandType = CommandType.StoredProcedure;

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                var retObject = new List<dynamic>();
                try
                {
                    using (var dtoReader = cmd.ExecuteReader())
                    {
                        while (dtoReader.Read())
                        {
                            var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                            {
                                dtoRow.Add(
                                dtoReader.GetName(iFiled),
                                dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dtoRow);
                        }
                    }
                    data.empdata = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            //data.empdata = _HRMSContext.MasterEmployee.Where(e => e.HRME_ActiveFlag == true).ToArray();
            return data;
        }
        public MOMUpdateDTO UpdateDetails(MOMUpdateDTO data)
        {

            try
            {
                data.emplist = (from a in _HRMSContext.MasterEmployee
                                from b in _HRMSContext.HR_Master_Designation
                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && b.HRMDES_Id == a.HRMDES_Id)
                                select new MOMUpdateDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,
                                    HRMDES_Id = b.HRMDES_Id,
                                    HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    HRME_EmailId = a.HRME_EmailId,
                                    HRME_MobileNo = a.HRME_MobileNo,
                                }).Distinct().OrderBy(T => T.HRME_Id).ToArray();

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "participantsupdate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MOM_Id", SqlDbType.BigInt)
                    {
                        Value = data.MOM_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.updateDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //data.updateDetails = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOM_Id == data.MOM_Id).ToArray();
                data.appliedlist = _HRMSContext.MOMinutesDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOM_ActiveFlag == true).ToArray();
                data.getfiledetails = _HRMSContext.MOMFileDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMFL_ActiveFlag == true).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public MOMUpdateDTO Edit(MOMUpdateDTO data)
        {

            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetMOMUpdateParticipantsDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MOM_Id", SqlDbType.BigInt)
                    {
                        Value = data.MOM_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.editupdateDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.editupdateDetails = (from a in _HRMSContext.MOMParticipantsDMO
                //                          from b in _HRMSContext.MOMeetingMinutesDMO
                //                          where (a.MOM_Id == data.MOM_Id && a.HRME_Id == b.HRME_Id && a.MOM_Id == b.MOM_Id && a.MOMPRT_Id == b.MOMPRT_Id)
                //                          select new MOMUpdateDTO
                //                          {
                //                              MOMMNT_ActionPlans = b.MOMMNT_ActionPlans,
                //                              MOMMNT_TargetDate = b.MOMMNT_TargetDate,
                //                              MOMMNT_Names = b.MOMMNT_Names,
                //                              HRME_Id = a.HRME_Id,
                //                              MOMPRT_ParticipantsName = a.MOMPRT_ParticipantsName,
                //                              MOMPRT_MobileNo = a.MOMPRT_MobileNo,
                //                              MOMPRT_EXTINTFlag = a.MOMPRT_EXTINTFlag,
                //                              MOMPRT_Designation = a.MOMPRT_Designation,
                //                              MOMPRT_Email = a.MOMPRT_Email,
                //                              MOMPRT_ActiveFlag = a.MOMPRT_ActiveFlag,
                //                              MOMMNT_Id = b.MOMMNT_Id,
                //                              MOMPRT_Id = a.MOMPRT_Id,
                //                              MOM_Id = a.MOM_Id,
                //                          }).Distinct().ToArray();
                data.appliedlist = _HRMSContext.MOMinutesDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOM_ActiveFlag == true).ToArray();
                data.emplist = (from a in _HRMSContext.MasterEmployee
                                from b in _HRMSContext.HR_Master_Designation
                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && b.HRMDES_Id == a.HRMDES_Id)
                                select new MOMUpdateDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,
                                    HRMDES_Id = b.HRMDES_Id,
                                    HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    HRME_EmailId = a.HRME_EmailId,
                                    HRME_MobileNo = a.HRME_MobileNo,
                                }).Distinct().OrderBy(T => T.HRME_Id).ToArray();
                //data.getfiledetails = _HRMSContext.MOMFileDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMFL_ActiveFlag == true).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public MOMUpdateDTO viewuploadflies(MOMUpdateDTO data)
        {

            try
            {
                data.getfiledetails = _HRMSContext.MOMFileDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMFL_ActiveFlag == true).ToArray();

                //data.getParticipentsdetails = (from a in _HRMSContext.MOMinutesDMO
                //                               from b in _HRMSContext.MOMParticipantsDMO
                //                               from c in _HRMSContext.MOMeetingMinutesDMO
                //                               where (a.MOM_Id == b.MOM_Id && b.HRME_Id == c.HRME_Id && b.MOM_Id == c.MOM_Id  && a.MOM_ActiveFlag == true)
                //                               select new MOMUpdateDTO
                //                               {
                //                                   MOM_Minutes = a.MOM_Minutes,
                //                                   MOMPRT_ParticipantsName = b.MOMPRT_ParticipantsName,
                //                                   MOMPRT_EXTINTFlag = b.MOMPRT_EXTINTFlag,
                //                                   MOMPRT_Designation = b.MOMPRT_Designation,
                //                                   MOMPRT_Email = b.MOMPRT_Email,
                //                                   MOMPRT_MobileNo = b.MOMPRT_MobileNo,
                //                                   MOMMNT_TargetDate = c.MOMMNT_TargetDate,
                //                                   MOMMNT_ActionPlans = c.MOMMNT_ActionPlans
                //                               }).Distinct().ToArray();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MinutesOfMeetingParticipants";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MOM_Id", SqlDbType.BigInt)
                    {
                        Value = data.MOM_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.getParticipentsdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public MOMUpdateDTO saveData(MOMUpdateDTO data)
        {

            try
            {
                if (data.MOM_Flag == "" || data.MOM_Flag == null || data.MOM_Flag == "false")
                {
                    if (data.newlstget1 == null)
                    {

                        foreach (var item1 in data.ParticipentsArray)
                        {
                            var newparticipents = _HRMSContext.MOMParticipantsDMO.Where(t => (t.HRME_Id.Equals(item1.HRME_Id) && t.MOMPRT_ParticipantsName == item1.MOMPRT_ParticipantsName) && t.MOMPRT_Id == item1.MOMPRT_Id && t.MOM_Id == data.MOM_Id).Count();
                            if (newparticipents > 0)
                            {
                                if (item1.MOMPRT_ParticipantsName != "" && item1.MOMPRT_ParticipantsName != null)
                                {
                                    var updateemp = _HRMSContext.MOMParticipantsDMO.Where(R => R.MOMPRT_Id == item1.MOMPRT_Id).ToList();
                                    if (updateemp.Count > 0)
                                    {
                                        foreach (var dd in updateemp)
                                        {
                                            dd.MOMPRT_ActiveFlag = updateemp[0].MOMPRT_ActiveFlag;
                                            dd.MOMPRT_UpdatedDate = DateTime.Now;
                                            dd.MOMPRT_UpdatedBy = data.userId;
                                            _HRMSContext.Update(dd);
                                            _HRMSContext.SaveChanges();
                                        }
                                    }
                                }

                                var newactionPlans = _HRMSContext.MOMeetingMinutesDMO.Where(t => (t.HRME_Id.Equals(item1.HRME_Id)) && t.MOM_Id == data.MOM_Id).Count();
                                if (newactionPlans > 0)
                                {
                                    if (item1.HRME_Id > 0)
                                    {
                                        //    DateTime? myDate = null;

                                        //    if ((item1.MOMMNT_TargetDate)!=null)
                                        //    {                                           
                                        //            myDate = item1.MOMMNT_TargetDate;

                                        //        // Optional: else you can handle invalid date format here, if needed
                                        //        // For example: log the issue or set a different default value
                                        //    }
                                        var updateaction = _HRMSContext.MOMeetingMinutesDMO.Where(R => R.MOMMNT_Id == item1.MOMMNT_Id).ToList();
                                        if (updateaction.Count > 0)
                                        {
                                            foreach (var dmbj in updateaction)
                                            {
                                                dmbj.MOMMNT_ActionPlans = item1.MOMMNT_ActionPlans;
                                                dmbj.MOMPRT_Id = item1.MOMPRT_Id;
                                                dmbj.MOMMNT_Names = item1.MOMMNT_Names;
                                                dmbj.MOMMNT_TargetDate = item1.MOMMNT_TargetDate;
                                                dmbj.MOMMNT_UpdatedDate = DateTime.Now;
                                                dmbj.MOMMNT_UpdatedBy = data.userId;
                                                _HRMSContext.Update(dmbj);
                                                _HRMSContext.SaveChanges();
                                            }

                                        }
                                    }
                                    else if (item1.HRME_Id == 0)
                                    {
                                        var updateaction1 = _HRMSContext.MOMeetingMinutesDMO.Where(t => t.MOMMNT_Id == item1.MOMMNT_Id).ToList();
                                        if (updateaction1.Count > 0)
                                        {
                                            foreach (var dmbj1 in updateaction1)
                                            {
                                                dmbj1.MOMMNT_ActionPlans = item1.MOMMNT_ActionPlans;
                                                dmbj1.MOMMNT_Names = item1.MOMMNT_Names;
                                                dmbj1.MOMMNT_TargetDate = item1.MOMMNT_TargetDate;
                                                dmbj1.MOMPRT_Id = item1.MOMPRT_Id;
                                                dmbj1.MOMMNT_UpdatedDate = DateTime.Now;
                                                dmbj1.MOMMNT_UpdatedBy = data.userId;
                                                _HRMSContext.Update(dmbj1);
                                                _HRMSContext.SaveChanges();
                                            }

                                        }

                                    }
                                }
                                else if (newactionPlans == 0)
                                {
                                    MOMeetingMinutesDMO dmoObj = new MOMeetingMinutesDMO();
                                    dmoObj.MOM_Id = data.MOM_Id;
                                    dmoObj.MOMMNT_ActionPlans = item1.MOMMNT_ActionPlans;
                                    dmoObj.MOMMNT_Names = item1.MOMMNT_Names;
                                    dmoObj.MOMMNT_TargetDate = item1.MOMMNT_TargetDate;
                                    dmoObj.MOMPRT_Id = item1.MOMPRT_Id;
                                    dmoObj.HRME_Id = item1.HRME_Id;
                                    dmoObj.MOMMNT_ActiveFlag = true;
                                    dmoObj.MOMMNT_CreatedDate = DateTime.Now;
                                    dmoObj.MOMMNT_UpdatedDate = DateTime.Now;
                                    dmoObj.MOMMNT_CreatedBy = data.userId;
                                    dmoObj.MOMMNT_UpdatedBy = data.userId;
                                    _HRMSContext.Add(dmoObj);
                                    _HRMSContext.SaveChanges();
                                }
                            }

                        }

                        //new added employee//
                        foreach (var item in data.Participent_Array)
                        {
                            long MOMPRT_Ids = 0;
                            var newparticipents1 = _HRMSContext.MOMParticipantsDMO.Where(t => (t.HRME_Id.Equals(item.HRME_Id) && t.MOMPRT_ParticipantsName == item.MOMPRT_ParticipantsName) && t.MOM_Id == data.MOM_Id).Count();
                            if (newparticipents1 > 0)
                            {
                                var updatenew = _HRMSContext.MOMParticipantsDMO.Where(t => (t.HRME_Id.Equals(item.HRME_Id) && t.MOMPRT_ParticipantsName == item.MOMPRT_ParticipantsName) && t.MOM_Id == data.MOM_Id).ToList();
                                if (updatenew.Count > 0)
                                {
                                    foreach (var objb in updatenew)
                                    {
                                        objb.MOM_Id = data.MOM_Id;
                                        objb.HRME_Id = item.HRME_Id;
                                        objb.MOMPRT_ParticipantsName = item.MOMPRT_ParticipantsName;
                                        objb.MOMPRT_Email = item.MOMPRT_Email;
                                        objb.MOMPRT_MobileNo = item.MOMPRT_MobileNo;
                                        objb.MOMPRT_Designation = item.MOMPRT_Designation;
                                        objb.MOMPRT_EXTINTFlag = item.MOMPRT_EXTINTFlag;
                                        objb.MOMPRT_ActiveFlag = true;
                                        objb.MOMPRT_UpdatedBy = data.userId;
                                        objb.MOMPRT_CreatedBy = data.userId;
                                        objb.MOMPRT_UpdatedDate = DateTime.Now;
                                        objb.MOMPRT_CreatedDate = DateTime.Now;
                                        _HRMSContext.Update(objb);
                                        _HRMSContext.SaveChanges();
                                        MOMPRT_Ids = objb.MOMPRT_Id;
                                    }
                                }
                            }
                            else if (newparticipents1 == 0)
                            {
                                MOMParticipantsDMO obb = new MOMParticipantsDMO();
                                obb.MOM_Id = data.MOM_Id;
                                obb.HRME_Id = item.HRME_Id;
                                obb.MOMPRT_ParticipantsName = item.MOMPRT_ParticipantsName;
                                obb.MOMPRT_Email = item.MOMPRT_Email;
                                obb.MOMPRT_MobileNo = item.MOMPRT_MobileNo;
                                obb.MOMPRT_Designation = item.MOMPRT_Designation;
                                obb.MOMPRT_EXTINTFlag = item.MOMPRT_EXTINTFlag;
                                obb.MOMPRT_ActiveFlag = true;
                                obb.MOMPRT_UpdatedBy = data.userId;
                                obb.MOMPRT_CreatedBy = data.userId;
                                obb.MOMPRT_UpdatedDate = DateTime.Now;
                                obb.MOMPRT_CreatedDate = DateTime.Now;
                                _HRMSContext.Add(obb);
                                _HRMSContext.SaveChanges();
                                MOMPRT_Ids = obb.MOMPRT_Id;
                            }

                            if (data.MOM_Id > 0 || item.HRME_Id > 0)
                            {
                                MOMeetingMinutesDMO dmoObj = new MOMeetingMinutesDMO();
                                dmoObj.MOM_Id = data.MOM_Id;
                                dmoObj.MOMMNT_ActionPlans = item.MOMMNT_ActionPlans;
                                dmoObj.MOMMNT_TargetDate = item.MOMMNT_TargetDate;
                                dmoObj.MOMMNT_Names = item.MOMMNT_Names;
                                dmoObj.MOMPRT_Id = MOMPRT_Ids;
                                dmoObj.HRME_Id = item.HRME_Id;
                                dmoObj.MOMMNT_ActiveFlag = true;
                                dmoObj.MOMMNT_CreatedDate = DateTime.Now;
                                dmoObj.MOMMNT_UpdatedDate = DateTime.Now;
                                dmoObj.MOMMNT_CreatedBy = data.userId;
                                dmoObj.MOMMNT_UpdatedBy = data.userId;
                                _HRMSContext.Add(dmoObj);
                                _HRMSContext.SaveChanges();
                            }

                        }
                    }
                    else
                    {
                        //foreach (var item1 in data.ParticipentsArray)
                        //{
                        //    var newparticipents = _HRMSContext.MOMParticipantsDMO.Where(t => (t.HRME_Id.Equals(item1.HRME_Id) && t.MOMPRT_ParticipantsName == item1.MOMPRT_ParticipantsName) && t.MOMPRT_Id == item1.MOMPRT_Id && t.MOM_Id == data.MOM_Id).Count();
                        //    if (newparticipents > 0)
                        //    {
                        //        if (item1.MOMPRT_ParticipantsName != "" && item1.MOMPRT_ParticipantsName != null)
                        //        {
                        //            var updateemp = _HRMSContext.MOMParticipantsDMO.Where(R => R.MOMPRT_Id == item1.MOMPRT_Id).ToList();
                        //            if (updateemp.Count > 0)
                        //            {
                        //                foreach (var dd in updateemp)
                        //                {
                        //                    dd.MOMPRT_ActiveFlag = updateemp[0].MOMPRT_ActiveFlag;
                        //                    dd.MOMPRT_UpdatedDate = DateTime.Now;
                        //                    dd.MOMPRT_UpdatedBy = data.userId;
                        //                    _HRMSContext.Update(dd);
                        //                    _HRMSContext.SaveChanges();
                        //                }
                        //            }
                        //        }
                        //    }
                        //}


                        for (int i = 0; i < data.newlstget1.Length; i++)
                        {

                            MOMeetingMinutesDMO enq = new MOMeetingMinutesDMO();
                            MOMParticipantsDMO enq1 = new MOMParticipantsDMO();




                            var emp_code_count = _HRMSContext.MOMinutesDMO.Where(t => t.MI_Id == data.MI_Id && t.MOM_Id == data.MOM_Id).ToList();

                            if (emp_code_count.Count > 0)
                            {
                                string s = data.newlstget1[i].Empcode;
                                //string[] values = s.Split(',');
                                string[] values = s.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                //string[] values = s.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                string Hrme_id = "";
                                for (int ii = 0; ii < values.Length; ii++)
                                {
                                    Hrme_id = values[ii].ToString();
                                    var empcode = _HRMSContext.MasterEmployee.Where(t => t.HRME_EmployeeCode == Hrme_id.Trim() && t.HRME_ActiveFlag == true).Select(r => r.HRME_Id).SingleOrDefault();

                                    if (data.newlstget1[i].Type == "Internal")
                                    {

                                        //var MOMPRTId = _HRMSContext.MOMParticipantsDMO.Where(t => (t.HRME_Id.Equals(empcode) && t.MOMPRT_ParticipantsName == data.newlstget1[i].Participants) && t.MOM_Id == data.MOM_Id).Select(p => p.MOMPRT_Id).FirstOrDefault();
                                        //if (MOMPRTId > 0)
                                        //{
                                        //    if (data.newlstget1[i].Participants != "" && data.newlstget1[i].Participants != null)
                                        //    {
                                        //        var updateemp = _HRMSContext.MOMParticipantsDMO.Where(R => R.MOMPRT_Id == MOMPRTId).ToList();
                                        //        if (updateemp.Count > 0)
                                        //        {
                                        //            foreach (var indd in updateemp)
                                        //            {

                                        //                indd.MOMPRT_ActiveFlag = enq1.MOMPRT_ActiveFlag;
                                        //                indd.MOMPRT_UpdatedDate = DateTime.Now;
                                        //                indd.MOMPRT_UpdatedBy = data.userId;
                                        //                _HRMSContext.Update(indd);
                                        //                _HRMSContext.SaveChanges();
                                        //            }
                                        //        }

                                        //        var MOMMNTId = _HRMSContext.MOMeetingMinutesDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMPRT_Id == MOMPRTId && t.HRME_Id == empcode && t.MOMMNT_ActionPlans == data.newlstget1[i].ActionPlans && t.MOMMNT_Names == data.newlstget1[i].Task).Select(p => p.MOMMNT_Id).SingleOrDefault();
                                        //        if (MOMMNTId > 0)
                                        //        {
                                        //            var momupdate = _HRMSContext.MOMeetingMinutesDMO.Where(R => R.MOMMNT_Id == MOMMNTId).ToList();
                                        //            if (momupdate.Count > 0)
                                        //            {
                                        //                DateTime? myDate = null;

                                        //                if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                        //                {
                                        //                    if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                        //                    {
                                        //                        myDate = parsedDate;
                                        //                    }

                                        //                }
                                        //                foreach (var ind in momupdate)
                                        //                {
                                        //                    ind.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                        //                    ind.MOMMNT_TargetDate = myDate;
                                        //                    ind.MOMMNT_Names = data.newlstget1[i].Task;
                                        //                    ind.MOMMNT_UpdatedDate = DateTime.Now;
                                        //                    ind.MOMMNT_UpdatedBy = data.userId;
                                        //                    _HRMSContext.Update(ind);
                                        //                    _HRMSContext.SaveChanges();
                                        //                }
                                        //            }
                                        //        }
                                        //        else
                                        //        {
                                        //            DateTime? myDate = null;

                                        //            if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                        //            {
                                        //                if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                        //                {
                                        //                    myDate = parsedDate;
                                        //                }

                                        //            }

                                        //            enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                        //            enq.MOMMNT_TargetDate = myDate;
                                        //            enq.MOMMNT_Names = data.newlstget1[i].Task;
                                        //            enq.MOM_Id = data.MOM_Id;
                                        //            enq.MOMPRT_Id = MOMPRTId;
                                        //            enq.MOMMNT_ActiveFlag = true;
                                        //            enq.HRME_Id = empcode;
                                        //            enq.MOMMNT_CreatedDate = DateTime.Now;
                                        //            enq.MOMMNT_UpdatedDate = DateTime.Now;
                                        //            enq.MOMMNT_CreatedBy = data.userId;
                                        //            enq.MOMMNT_UpdatedBy = data.userId;
                                        //            _HRMSContext.Add(enq);
                                        //            _HRMSContext.SaveChanges();
                                        //        }

                                        //    }

                                        //}


                                        var MOMPRTId = _HRMSContext.MOMParticipantsDMO.Where(t => t.HRME_Id.Equals(empcode) && t.MOMPRT_ParticipantsName == data.newlstget1[i].Participants && t.MOM_Id == data.MOM_Id).Select(p => p.MOMPRT_Id).FirstOrDefault();
                                        if (MOMPRTId > 0)
                                        {
                                            var empCodes = data.newlstget1[i].Empcode.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                            var participants = data.newlstget1[i].Participants.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                            for (int j = 0; j < empCodes.Length; j++)
                                            {
                                                if (j < participants.Length && long.TryParse(empCodes[j], out long hrmeId))
                                                {
                                                    enq1 = new MOMParticipantsDMO
                                                    {
                                                        MOM_Id = data.MOM_Id,
                                                        MOMPRT_ParticipantsName = participants[j].Trim(),
                                                        MOMPRT_Designation = data.newlstget1[i].Designation,
                                                        MOMPRT_EXTINTFlag = data.newlstget1[i].Type,
                                                        //MOMPRT_ActiveFlag = data.newlstget1[i].MOMPRT_ActiveFlag,
                                                        MOMPRT_CreatedDate = DateTime.Now,
                                                        MOMPRT_UpdatedDate = DateTime.Now,
                                                        MOMPRT_CreatedBy = data.userId,
                                                        MOMPRT_UpdatedBy = data.userId,
                                                        MOMPRT_ActiveFlag = true,
                                                        MOMPRT_MobileNo = Convert.ToInt64(data.newlstget1[i].MobileNo),
                                                        MOMPRT_Email = data.newlstget1[i].Email,
                                                        HRME_Id = hrmeId
                                                    };

                                                    _HRMSContext.Add(enq1);
                                                    _HRMSContext.SaveChanges();

                                                    long momprtids = enq1.MOMPRT_Id;

                                                     enq = new MOMeetingMinutesDMO
                                                    {
                                                        MOMPRT_Id = momprtids,
                                                        MOM_Id = data.MOM_Id,
                                                        MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans,
                                                        MOMMNT_Names = data.newlstget1[i].Task,
                                                        MOMMNT_ActiveFlag = true,
                                                        MOMMNT_CreatedDate = DateTime.Now,
                                                        MOMMNT_UpdatedDate = DateTime.Now,
                                                        MOMMNT_CreatedBy = data.userId,
                                                        MOMMNT_UpdatedBy = data.userId
                                                    };

                                                    DateTime? myDate = null;
                                                    if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                                    {
                                                        if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                        {
                                                            myDate = parsedDate;
                                                        }
                                                    }

                                                    enq.MOMMNT_TargetDate = myDate;

                                                    _HRMSContext.Add(enq);
                                                    _HRMSContext.SaveChanges();

                                                    var dmoObj = new MOMeetingMinutesDMO
                                                    {
                                                        MOM_Id = data.MOM_Id,
                                                        MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans,
                                                        MOMMNT_Names = data.newlstget1[i].Task,
                                                        MOMMNT_TargetDate = myDate,
                                                        HRME_Id = hrmeId,
                                                        MOMPRT_Id = momprtids,
                                                        MOMMNT_ActiveFlag = true,
                                                        MOMMNT_CreatedDate = DateTime.Now,
                                                        MOMMNT_UpdatedDate = DateTime.Now,
                                                        MOMMNT_CreatedBy = data.userId,
                                                        MOMMNT_UpdatedBy = data.userId
                                                    };

                                                    _HRMSContext.Add(dmoObj);
                                                    _HRMSContext.SaveChanges();

                                                    data.HRME_Id = Convert.ToInt64(empcode);

                                                    try
                                                    {
                                                        var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0,@p1,@p2,@p3", data.HRME_Id, data.userId, dmoObj.MOMMNT_ActionPlans, dmoObj.MOMMNT_TargetDate);

                                                        if (outputval > 0)
                                                        {
                                                            Console.WriteLine("Task Created");
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Task Not Created");
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine($"Error occurred while calling the stored procedure: {ex.Message}");
                                                    }
                                                }
                                            }
                                        }



                                        else
                                        {
                                            long momprtids = 0;
                                            data.albumNameArray = (from a in _HRMSContext.MasterEmployee
                                                                   from b in _HRMSContext.HR_Master_Designation
                                                                   from c in _HRMSContext.Emp_Email_Id
                                                                   from d in _HRMSContext.Emp_MobileNo
                                                                   where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_EmployeeCode == data.newlstget1[i].Empcode
                                                                   && a.HRME_Id == c.HRME_Id && a.HRME_Id == d.HRME_Id && a.HRMDES_Id == b.HRMDES_Id)
                                                                   select new MOMUpdateDTO
                                                                   {
                                                                       HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                                       HRME_Id = a.HRME_Id,
                                                                       HRMDES_Id = b.HRMDES_Id,
                                                                       HRMDES_DesignationName = b.HRMDES_DesignationName,
                                                                       HRME_EmailId = c.HRMEM_EmailId,
                                                                       HRME_MobileNo = d.HRMEMNO_MobileNo
                                                                   }).ToArray();

                                            if (data.albumNameArray.Length > 0)
                                            {


                                                //enq1.MOM_Id = data.MOM_Id;
                                                //enq1.MOMPRT_ParticipantsName = data.albumNameArray[0].HRME_EmployeeFirstName;
                                                //enq1.MOMPRT_Designation = data.albumNameArray[0].HRMDES_DesignationName;
                                                //enq1.MOMPRT_EXTINTFlag = data.newlstget1[i].Type;
                                                //enq1.MOMPRT_CreatedDate = DateTime.Now;
                                                //enq1.MOMPRT_UpdatedDate = DateTime.Now;
                                                //enq1.MOMPRT_CreatedBy = data.userId;
                                                //enq1.MOMPRT_UpdatedBy = data.userId;
                                                //enq1.MOMPRT_ActiveFlag = true;
                                                //enq1.MOMPRT_MobileNo = data.albumNameArray[0].HRME_MobileNo;
                                                //enq1.MOMPRT_Email = data.albumNameArray[0].HRME_EmailId;
                                                //enq1.HRME_Id = data.albumNameArray[0].HRME_Id;
                                                //_HRMSContext.Add(enq1);
                                                //_HRMSContext.SaveChanges();
                                                //momprtids = enq1.MOMPRT_Id;

                                                //DateTime? myDate = null;

                                                //if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                                //{
                                                //    if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                //    {
                                                //        myDate = parsedDate;
                                                //    }

                                                //}
                                                //enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                                //enq.MOMMNT_TargetDate = myDate;
                                                //enq.MOMMNT_Names = data.newlstget1[i].Task;
                                                //enq.MOM_Id = data.MOM_Id;
                                                //enq.MOMPRT_Id = momprtids;
                                                //enq.MOMMNT_ActiveFlag = true;
                                                //enq.HRME_Id = data.albumNameArray[0].HRME_Id;
                                                //enq.MOMMNT_CreatedDate = DateTime.Now;
                                                //enq.MOMMNT_UpdatedDate = DateTime.Now;
                                                //enq.MOMMNT_CreatedBy = data.userId;
                                                //enq.MOMMNT_UpdatedBy = data.userId;
                                                //_HRMSContext.Add(enq);
                                                //_HRMSContext.SaveChanges();

                                                var empCodes = data.newlstget1[i].Empcode.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                var participants = data.newlstget1[i].Participants.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                                for (int j = 0; j < empCodes.Length; j++)
                                                {
                                                    if (j < participants.Length && long.TryParse(empCodes[j], out long hrmeId))
                                                    {

                                                        enq1 = new MOMParticipantsDMO
                                                        {
                                                            MOM_Id = data.MOM_Id,
                                                            MOMPRT_ParticipantsName = participants[j].Trim(),
                                                            MOMPRT_Designation = data.newlstget1[i].Designation,
                                                            MOMPRT_EXTINTFlag = data.newlstget1[i].Type,
                                                            MOMPRT_CreatedDate = DateTime.Now,
                                                            MOMPRT_UpdatedDate = DateTime.Now,
                                                            MOMPRT_CreatedBy = data.userId,
                                                            MOMPRT_UpdatedBy = data.userId,
                                                            MOMPRT_ActiveFlag = true,
                                                            MOMPRT_MobileNo = Convert.ToInt64(data.newlstget1[i].MobileNo),
                                                            MOMPRT_Email = data.newlstget1[i].Email,
                                                            HRME_Id = hrmeId
                                                        };


                                                        _HRMSContext.Add(enq1);
                                                        _HRMSContext.SaveChanges();

                                                        momprtids = enq1.MOMPRT_Id;


                                                        enq = new MOMeetingMinutesDMO
                                                        {
                                                            MOMPRT_Id = momprtids,
                                                            MOM_Id = data.MOM_Id,
                                                            MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans,
                                                            MOMMNT_Names = data.newlstget1[i].Task,
                                                            MOMMNT_ActiveFlag = true,
                                                            MOMMNT_CreatedDate = DateTime.Now,
                                                            MOMMNT_UpdatedDate = DateTime.Now,
                                                            MOMMNT_CreatedBy = data.userId,
                                                            MOMMNT_UpdatedBy = data.userId
                                                        };

                                                        // Parse TargetDate if available
                                                        DateTime? myDate = null;
                                                        if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                                        {
                                                            if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                            {
                                                                myDate = parsedDate;
                                                            }
                                                        }

                                                        enq.MOMMNT_TargetDate = myDate;

                                                        // Save the MOMeetingMinutesDMO to the database
                                                        _HRMSContext.Add(enq);
                                                        _HRMSContext.SaveChanges();

                                                        // Now call the stored procedure after saving the meeting minute
                                                        MOMeetingMinutesDMO dmoObj = new MOMeetingMinutesDMO
                                                        {
                                                            MOM_Id = data.MOM_Id,
                                                            MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans,
                                                            MOMMNT_Names = data.newlstget1[i].Task,
                                                            MOMMNT_TargetDate = myDate,
                                                            HRME_Id = hrmeId,
                                                            MOMPRT_Id = momprtids,
                                                            MOMMNT_ActiveFlag = true,
                                                            MOMMNT_CreatedDate = DateTime.Now,
                                                            MOMMNT_UpdatedDate = DateTime.Now,
                                                            MOMMNT_CreatedBy = data.userId,
                                                            MOMMNT_UpdatedBy = data.userId
                                                        };

                                                        _HRMSContext.Add(dmoObj);
                                                        _HRMSContext.SaveChanges();

                                                        //data.HRME_Id = empcode;
                                                        // Assuming empcode is a string that represents a numeric value
                                                        data.HRME_Id = Convert.ToInt64(empcode);

                                                        // Call stored procedure to create a task
                                                        try
                                                        {

                                                            var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0,@p1,@p2,@p3", data.HRME_Id, data.userId, dmoObj.MOMMNT_ActionPlans, dmoObj.MOMMNT_TargetDate);


                                                            if (outputval > 0)
                                                            {
                                                                Console.WriteLine("Task Created");
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Task Not Created");
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine($"Error occurred while calling the stored procedure: {ex.Message}");
                                                        }
                                                    }
                                                }

                                            }

                                         


                                            else if (data.albumNameArray.Length == 0)
                                            {
                                                var empCodes = data.newlstget1[i].Empcode.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                var participants = data.newlstget1[i].Participants.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                                for (int j = 0; j < empCodes.Length; j++)
                                                {
                                                    if (j < participants.Length && long.TryParse(empCodes[j], out long hrmeId))
                                                    {
                                                        
                                                            enq1 = new MOMParticipantsDMO
                                                            {
                                                                MOM_Id = data.MOM_Id,
                                                                MOMPRT_ParticipantsName = participants[j].Trim(),
                                                                MOMPRT_Designation = data.newlstget1[i].Designation,
                                                                MOMPRT_EXTINTFlag = data.newlstget1[i].Type,
                                                                MOMPRT_CreatedDate = DateTime.Now,
                                                                MOMPRT_UpdatedDate = DateTime.Now,
                                                                MOMPRT_CreatedBy = data.userId,
                                                                MOMPRT_UpdatedBy = data.userId,
                                                                MOMPRT_ActiveFlag = true,
                                                                MOMPRT_MobileNo = Convert.ToInt64(data.newlstget1[i].MobileNo),
                                                                MOMPRT_Email = data.newlstget1[i].Email,
                                                                HRME_Id = hrmeId
                                                            };


                                                            _HRMSContext.Add(enq1);
                                                            _HRMSContext.SaveChanges();

                                                            momprtids = enq1.MOMPRT_Id;


                                                            enq = new MOMeetingMinutesDMO
                                                            {
                                                                MOMPRT_Id = momprtids,
                                                                MOM_Id = data.MOM_Id,
                                                                MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans,
                                                                MOMMNT_Names = data.newlstget1[i].Task,
                                                                MOMMNT_ActiveFlag = true,
                                                                MOMMNT_CreatedDate = DateTime.Now,
                                                                MOMMNT_UpdatedDate = DateTime.Now,
                                                                MOMMNT_CreatedBy = data.userId,
                                                                MOMMNT_UpdatedBy = data.userId
                                                            };

                                                            // Parse TargetDate if available
                                                            DateTime? myDate = null;
                                                        if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                                        {
                                                            if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                            {
                                                                myDate = parsedDate;
                                                            }
                                                        }

                                                        enq.MOMMNT_TargetDate = myDate;

                                                        // Save the MOMeetingMinutesDMO to the database
                                                        _HRMSContext.Add(enq);
                                                        _HRMSContext.SaveChanges();

                                                        // Now call the stored procedure after saving the meeting minute
                                                        MOMeetingMinutesDMO dmoObj = new MOMeetingMinutesDMO
                                                        {
                                                            MOM_Id = data.MOM_Id,
                                                            MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans,
                                                            MOMMNT_Names = data.newlstget1[i].Task,
                                                            MOMMNT_TargetDate = myDate,
                                                            HRME_Id = hrmeId,
                                                            MOMPRT_Id = momprtids,
                                                            MOMMNT_ActiveFlag = true,
                                                            MOMMNT_CreatedDate = DateTime.Now,
                                                            MOMMNT_UpdatedDate = DateTime.Now,
                                                            MOMMNT_CreatedBy = data.userId,
                                                            MOMMNT_UpdatedBy = data.userId
                                                        };

                                                        _HRMSContext.Add(dmoObj);
                                                        _HRMSContext.SaveChanges();

                                                        data.HRME_Id = empcode;
                                                        // Call stored procedure to create a task
                                                        try
                                                        {
                                              
                                                            var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0,@p1,@p2,@p3", data.HRME_Id, data.userId, dmoObj.MOMMNT_ActionPlans, dmoObj.MOMMNT_TargetDate);


                                                            if (outputval > 0)
                                                            {
                                                                Console.WriteLine("Task Created");
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Task Not Created");
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine($"Error occurred while calling the stored procedure: {ex.Message}");
                                                        }
                                                    }
                                                }
                                            }






                                        }
                                    }
                                    // External departments
                                    if (data.newlstget1[i].Type == "External")
                                    {
                                        var EXMOMPRTId = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOMPRT_Email == data.newlstget1[i].Email && t.MOMPRT_MobileNo == Convert.ToInt64(data.newlstget1[i].MobileNo) && t.MOMPRT_ParticipantsName == data.newlstget1[i].Participants && t.MOM_Id == data.MOM_Id).Select(p => p.MOMPRT_Id).FirstOrDefault();
                                        if (EXMOMPRTId > 0)
                                        {
                                            if (data.newlstget1[i].Participants != "" && data.newlstget1[i].Participants != null)
                                            {
                                                var exupdateemp = _HRMSContext.MOMParticipantsDMO.Where(R => R.MOMPRT_Id == EXMOMPRTId).ToList();
                                                if (exupdateemp.Count > 0)
                                                {
                                                    foreach (var exdd in exupdateemp)
                                                    {
                                                        //if (data.newlstget1[i].Attendence == "p" || data.newlstget1[i].Attendence == "P")
                                                        //{
                                                        //    enq1.MOMPRT_ActiveFlag = true;
                                                        //}
                                                        //else if (data.newlstget1[i].Attendence == "a" || data.newlstget1[i].Attendence == "A")
                                                        //{
                                                        //    enq1.MOMPRT_ActiveFlag = false;
                                                        //}
                                                        exdd.MOMPRT_ActiveFlag = enq1.MOMPRT_ActiveFlag;
                                                        exdd.MOMPRT_UpdatedDate = DateTime.Now;
                                                        exdd.MOMPRT_UpdatedBy = data.userId;
                                                        _HRMSContext.Update(exdd);
                                                        _HRMSContext.SaveChanges();
                                                    }
                                                }

                                                var EXMOMMNTId = _HRMSContext.MOMeetingMinutesDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMMNT_ActionPlans == data.newlstget1[i].ActionPlans && t.MOMPRT_Id == EXMOMPRTId && t.HRME_Id == Convert.ToInt64(data.newlstget1[i].Empcode)).Select(p => p.MOMMNT_Id).SingleOrDefault();
                                                if (EXMOMMNTId > 0)
                                                {
                                                    var exmomupdate = _HRMSContext.MOMeetingMinutesDMO.Where(R => R.MOMMNT_Id == EXMOMMNTId).ToList();
                                                    if (exmomupdate.Count > 0)
                                                    {
                                                        DateTime? myDate = null;

                                                        if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                                        {
                                                            if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                            {
                                                                myDate = parsedDate;
                                                            }
                                                            // Optional: else you can handle invalid date format here, if needed
                                                            // For example: log the issue or set a different default value
                                                        }
                                                        foreach (var exd in exmomupdate)
                                                        {
                                                            exd.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                                            exd.MOMMNT_TargetDate = myDate;
                                                            exd.MOMMNT_Names = data.newlstget1[i].Task;
                                                            exd.MOMMNT_UpdatedDate = DateTime.Now;
                                                            exd.MOMMNT_UpdatedBy = data.userId;
                                                            _HRMSContext.Update(exd);
                                                            _HRMSContext.SaveChanges();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    DateTime? myDate = null;

                                                    if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                                    {
                                                        if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                        {
                                                            myDate = parsedDate;
                                                        }
                                                        // Optional: else you can handle invalid date format here, if needed
                                                        // For example: log the issue or set a different default value
                                                    }


                                                    var empCodes = data.newlstget1[i].Empcode.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                                    foreach (var empCode in empCodes)
                                                    {
                                                        if (long.TryParse(empCode, out long hrmeId))
                                                        {

                                                            enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                                            enq.MOMMNT_TargetDate = myDate;
                                                            enq.MOMMNT_Names = data.newlstget1[i].Task;
                                                            enq.MOM_Id = data.MOM_Id;
                                                            enq.MOMPRT_Id = EXMOMPRTId;
                                                            enq.MOMMNT_ActiveFlag = true;
                                                            //enq.HRME_Id = Convert.ToInt64(data.newlstget1[i].Empcode);
                                                            enq.HRME_Id = Convert.ToInt64(hrmeId);
                                                            enq.MOMMNT_CreatedDate = DateTime.Now;
                                                            enq.MOMMNT_UpdatedDate = DateTime.Now;
                                                            enq.MOMMNT_CreatedBy = data.userId;
                                                            enq.MOMMNT_UpdatedBy = data.userId;
                                                            _HRMSContext.Add(enq);
                                                            _HRMSContext.SaveChanges();
                                                        }
                                                    }
                                                }

                                            }

                                        }

                                        //new record expect from MOMParticipantsDMO {external dept}
                                        else
                                        {
                                            long MOMPRTIDS = 0;
                                            //if (data.newlstget1[i].Attendence == "p" || data.newlstget1[i].Attendence == "P")
                                            //{
                                            //    enq1.MOMPRT_ActiveFlag = true;
                                            //}
                                            //else if (data.newlstget1[i].Attendence == "a" || data.newlstget1[i].Attendence == "A")
                                            //{
                                            //    enq1.MOMPRT_ActiveFlag = false;
                                            //}
                                            enq1.MOM_Id = data.MOM_Id;
                                            enq1.MOMPRT_ParticipantsName = data.newlstget1[i].Participants;
                                            enq1.MOMPRT_Designation = data.newlstget1[i].Designation;
                                            enq1.MOMPRT_EXTINTFlag = data.newlstget1[i].Type;
                                            enq1.MOMPRT_ActiveFlag = true;
                                            enq1.MOMPRT_CreatedDate = DateTime.Now;
                                            enq1.MOMPRT_UpdatedDate = DateTime.Now;
                                            enq1.MOMPRT_CreatedBy = data.userId;
                                            enq1.MOMPRT_UpdatedBy = data.userId;
                                            enq1.MOMPRT_MobileNo = Convert.ToInt64(data.newlstget1[i].MobileNo);
                                            enq1.MOMPRT_Email = data.newlstget1[i].Email;
                                            enq1.HRME_Id = Convert.ToInt64(data.newlstget1[i].Empcode);
                                            _HRMSContext.Add(enq1);
                                            _HRMSContext.SaveChanges();
                                            MOMPRTIDS = enq1.MOMPRT_Id;

                                            DateTime? myDate = null;

                                            if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                            {
                                                if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                {
                                                    myDate = parsedDate;
                                                }
                                                // Optional: else you can handle invalid date format here, if needed
                                                // For example: log the issue or set a different default value
                                            }
                                            enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                            enq.MOMMNT_TargetDate = myDate;
                                            enq.MOMMNT_Names = data.newlstget1[i].Task;
                                            enq.MOMPRT_Id = MOMPRTIDS;
                                            enq.MOM_Id = data.MOM_Id;
                                            enq.MOMMNT_ActiveFlag = true;
                                            enq.HRME_Id = Convert.ToInt64(data.newlstget1[i].Empcode);
                                            enq.MOMMNT_CreatedDate = DateTime.Now;
                                            enq.MOMMNT_UpdatedDate = DateTime.Now;
                                            enq.MOMMNT_CreatedBy = data.userId;
                                            enq.MOMMNT_UpdatedBy = data.userId;
                                            _HRMSContext.Add(enq);
                                            _HRMSContext.SaveChanges();
                                        }

                                    }
                                    continue;
                                }





                            }

                        }

                    }

                    if (data.FilePath_Array.Length > 0)
                    {
                        foreach (var filedata in data.FilePath_Array)
                        {
                            MOMFileDMO file = new MOMFileDMO();
                            file.MOMFL_FileName = filedata.FileName;
                            file.MOMFL_FilePath = filedata.FilePath;
                            file.MOM_Id = data.MOM_Id;
                            file.MOMFL_ActiveFlag = true;
                            file.MOMFL_CreatedBy = data.userId;
                            file.MOMFL_UpdatedBy = data.userId;
                            file.MOMFL_CreatedDate = DateTime.Now;
                            file.MOMFL_UpdatedDate = DateTime.Now;
                            _HRMSContext.Add(file);
                            _HRMSContext.SaveChanges();
                        }

                    }

                    if (data.MOM_Minutes != "" && data.MOM_Minutes != null)
                    {
                        var updateminutes = _HRMSContext.MOMinutesDMO.Where(R => R.MOM_Id == data.MOM_Id).ToList();
                        if (updateminutes.Count > 0)
                        {
                            foreach (var d in updateminutes)
                            {
                                d.MOM_Minutes = data.MOM_Minutes;
                                d.MOM_TentativeEndTime = data.MOM_EndTime;
                                d.MOM_TentativeStartTime = data.MOM_StartTime;
                                d.MOM_Status = "Partially Updated ";
                                d.MOM_Flag = "Pending";
                                _HRMSContext.Update(d);
                            }

                            var flag1 = _HRMSContext.SaveChanges();
                            if (flag1 > 0)
                            {
                                data.retrunMsg = "Update";
                            }
                            else
                            {
                                data.retrunMsg = "false";
                            }
                        }

                    }
                }

                //finalflag check//////////////////////////////////////////////////////////////////////////
                else
                {
                    if (data.newlstget1 == null)
                    {
                        foreach (var item1 in data.ParticipentsArray)
                        {
                            DateTime? TargetDate = null;

                            if (!string.IsNullOrEmpty(item1.MOMMNT_TargetDate.ToString()))
                            {
                                if (DateTime.TryParse(item1.MOMMNT_TargetDate.ToString(), out DateTime parsedDate))
                                {
                                    TargetDate = parsedDate;
                                }
                                // Optional: else you can handle invalid date format here, if needed
                                // For example: log the issue or set a different default value
                            }
                            var newparticipents = _HRMSContext.MOMParticipantsDMO.Where(t => (t.HRME_Id.Equals(item1.HRME_Id) && t.MOMPRT_ParticipantsName == item1.MOMPRT_ParticipantsName) && t.MOMPRT_Id == item1.MOMPRT_Id && t.MOM_Id == data.MOM_Id).Count();
                            if (newparticipents > 0)
                            {
                                if (item1.MOMPRT_ParticipantsName != "" && item1.MOMPRT_ParticipantsName != null)
                                {
                                    var updateemp = _HRMSContext.MOMParticipantsDMO.Where(R => R.MOMPRT_Id == item1.MOMPRT_Id).ToList();
                                    if (updateemp.Count > 0)
                                    {
                                        foreach (var dd in updateemp)
                                        {
                                            dd.MOMPRT_ActiveFlag = updateemp[0].MOMPRT_ActiveFlag;
                                            dd.MOMPRT_UpdatedDate = DateTime.Now;
                                            dd.MOMPRT_UpdatedBy = data.userId;
                                            _HRMSContext.Update(dd);
                                            _HRMSContext.SaveChanges();
                                        }
                                    }
                                }
                                var newactionPlans = _HRMSContext.MOMeetingMinutesDMO.Where(t => (t.HRME_Id.Equals(item1.HRME_Id)) && t.MOM_Id == data.MOM_Id).Count();
                                if (newactionPlans > 0)
                                {
                                    if (item1.HRME_Id > 0)
                                    {

                                        var updateaction = _HRMSContext.MOMeetingMinutesDMO.Where(R => R.MOMMNT_Id == item1.MOMMNT_Id).ToList();
                                        if (updateaction.Count > 0)
                                        {
                                            foreach (var dmbj in updateaction)
                                            {
                                                dmbj.MOMMNT_ActionPlans = item1.MOMMNT_ActionPlans;
                                                dmbj.MOMMNT_Names = item1.MOMMNT_Names;
                                                dmbj.MOMMNT_TargetDate = TargetDate;
                                                // dmbj.MOMPRT_Id = item1.MOMPRT_Id;
                                                dmbj.MOMMNT_UpdatedDate = DateTime.Now;
                                                dmbj.MOMMNT_UpdatedBy = data.userId;
                                                _HRMSContext.Update(dmbj);
                                                _HRMSContext.SaveChanges();
                                            }

                                        }
                                    }
                                    else if (item1.HRME_Id == 0)
                                    {

                                        var updateaction = _HRMSContext.MOMeetingMinutesDMO.Where(R => R.MOMMNT_Id == item1.MOMMNT_Id).ToList();
                                        if (updateaction.Count > 0)
                                        {
                                            foreach (var dmbj in updateaction)
                                            {
                                                dmbj.MOMMNT_ActionPlans = item1.MOMMNT_ActionPlans;
                                                dmbj.MOMMNT_Names = item1.MOMMNT_Names;
                                                dmbj.MOMMNT_TargetDate = TargetDate;
                                                dmbj.MOMPRT_Id = item1.MOMPRT_Id;
                                                dmbj.MOMMNT_UpdatedDate = DateTime.Now;
                                                dmbj.MOMMNT_UpdatedBy = data.userId;
                                                _HRMSContext.Update(dmbj);
                                                _HRMSContext.SaveChanges();
                                            }

                                        }
                                    }
                                }
                                else if (newactionPlans == 0)
                                {
                                    MOMeetingMinutesDMO dmoObj = new MOMeetingMinutesDMO();
                                    dmoObj.MOM_Id = data.MOM_Id;
                                    dmoObj.MOMMNT_ActionPlans = item1.MOMMNT_ActionPlans;
                                    dmoObj.MOMMNT_Names = item1.MOMMNT_Names;
                                    dmoObj.MOMMNT_TargetDate = TargetDate;
                                    dmoObj.HRME_Id = item1.HRME_Id;
                                    dmoObj.MOMPRT_Id = item1.MOMPRT_Id;
                                    dmoObj.MOMMNT_ActiveFlag = true;
                                    dmoObj.MOMMNT_CreatedDate = DateTime.Now;
                                    dmoObj.MOMMNT_UpdatedDate = DateTime.Now;
                                    dmoObj.MOMMNT_CreatedBy = data.userId;
                                    dmoObj.MOMMNT_UpdatedBy = data.userId;
                                    _HRMSContext.Add(dmoObj);
                                    _HRMSContext.SaveChanges();

                                    try
                                    {
                                        var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0,@p1,@p2,@p3", data.HRME_Id, data.userId, dmoObj.MOMMNT_ActionPlans, dmoObj.MOMMNT_TargetDate);
                                        if (outputval > 0)
                                        {
                                            Console.WriteLine("Task Created");
                                        }
                                        else
                                        {
                                            Console.WriteLine(" Task Not Created");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }

                            }
                        }
                        foreach (var item in data.Participent_Array)
                        {
                            DateTime? TargetDate = null;


                            if (!string.IsNullOrEmpty(item.MOMMNT_TargetDate.ToString()))
                            {
                                if (DateTime.TryParse(item.MOMMNT_TargetDate.ToString(), out DateTime parsedDate))
                                {
                                    TargetDate = parsedDate;
                                }
                                // Optional: else you can handle invalid date format here, if needed
                                // For example: log the issue or set a different default value
                            }
                            long MOMPRTIds = 0;
                            var newparticipents1 = _HRMSContext.MOMParticipantsDMO.Where(t => (t.HRME_Id.Equals(item.HRME_Id) && t.MOMPRT_ParticipantsName == item.MOMPRT_ParticipantsName) && t.MOM_Id == data.MOM_Id).Count();
                            if (newparticipents1 > 0)
                            {
                                MOMParticipantsDMO oobb = new MOMParticipantsDMO();
                                // obb.MOMPRT_Id = data.MOMPRT_Id;
                                oobb.MOM_Id = data.MOM_Id;
                                oobb.HRME_Id = item.HRME_Id;
                                oobb.MOMPRT_ParticipantsName = item.MOMPRT_ParticipantsName;
                                oobb.MOMPRT_Email = item.MOMPRT_Email;
                                oobb.MOMPRT_MobileNo = item.MOMPRT_MobileNo;
                                oobb.MOMPRT_Designation = item.MOMPRT_Designation;
                                oobb.MOMPRT_EXTINTFlag = item.MOMPRT_EXTINTFlag;
                                oobb.MOMPRT_ActiveFlag = true;
                                oobb.MOMPRT_UpdatedBy = data.userId;
                                oobb.MOMPRT_UpdatedDate = DateTime.Now;
                                _HRMSContext.Update(oobb);
                                _HRMSContext.SaveChanges();
                                MOMPRTIds = oobb.MOMPRT_Id;
                            }
                            else if (newparticipents1 == 0)
                            {
                                MOMParticipantsDMO obb = new MOMParticipantsDMO();
                                //  obb.MOMPRT_Id = data.MOMPRT_Id;
                                obb.MOM_Id = data.MOM_Id;
                                obb.HRME_Id = item.HRME_Id;
                                obb.MOMPRT_ParticipantsName = item.MOMPRT_ParticipantsName;
                                obb.MOMPRT_Email = item.MOMPRT_Email;
                                obb.MOMPRT_MobileNo = item.MOMPRT_MobileNo;
                                obb.MOMPRT_Designation = item.MOMPRT_Designation;
                                obb.MOMPRT_EXTINTFlag = item.MOMPRT_EXTINTFlag;
                                obb.MOMPRT_ActiveFlag = true;
                                obb.MOMPRT_CreatedBy = data.userId;
                                obb.MOMPRT_UpdatedBy = data.userId;
                                obb.MOMPRT_CreatedDate = DateTime.Now;
                                obb.MOMPRT_UpdatedDate = DateTime.Now;
                                _HRMSContext.Add(obb);
                                _HRMSContext.SaveChanges();
                                MOMPRTIds = obb.MOMPRT_Id;
                            }
                            if ((item.MOMMNT_ActionPlans != null || item.MOMMNT_ActionPlans != "") || item.HRME_Id > 0)
                            {
                                MOMeetingMinutesDMO dmoObj = new MOMeetingMinutesDMO();
                                dmoObj.MOM_Id = data.MOM_Id;
                                dmoObj.MOMMNT_ActionPlans = item.MOMMNT_ActionPlans;
                                dmoObj.MOMMNT_TargetDate = TargetDate;
                                dmoObj.MOMMNT_Names = item.MOMMNT_Names;
                                dmoObj.HRME_Id = item.HRME_Id;
                                dmoObj.MOMPRT_Id = MOMPRTIds;
                                dmoObj.MOMMNT_ActiveFlag = true;
                                dmoObj.MOMMNT_CreatedDate = DateTime.Now;
                                dmoObj.MOMMNT_UpdatedDate = DateTime.Now;
                                dmoObj.MOMMNT_CreatedBy = data.userId;
                                dmoObj.MOMMNT_UpdatedBy = data.userId;
                                _HRMSContext.Update(dmoObj);
                                _HRMSContext.SaveChanges();
                                try
                                {
                                    var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0, @p1, @p2, @p3", dmoObj.HRME_Id, data.userId, dmoObj.MOMMNT_ActionPlans, dmoObj.MOMMNT_TargetDate);
                                    if (outputval > 0)
                                    {
                                        Console.WriteLine("Task Created");
                                    }
                                    else
                                    {
                                        Console.WriteLine(" Task Not Created");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                        }
                    }


                    else
                    {
                        for (int i = 0; i < data.newlstget1.Length; i++)
                        {

                            MOMeetingMinutesDMO enq = new MOMeetingMinutesDMO();
                            MOMParticipantsDMO enq1 = new MOMParticipantsDMO();
                            //DateTime? TargetDate = DateTime.Parse(data.newlstget1[i].TargetDate);
                            DateTime? TargetDate = null;

                            if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                            {
                                if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                {
                                    TargetDate = parsedDate;
                                }
                                // Optional: else you can handle invalid date format here, if needed
                                // For example: log the issue or set a different default value
                            }
                            var emp_code_count = _HRMSContext.MOMinutesDMO.Where(t => t.MI_Id == data.MI_Id && t.MOM_Id == data.MOM_Id).ToList();

                            if (emp_code_count.Count > 0)
                            {
                                //empcode as to check if get error
                                var empcode = _HRMSContext.MasterEmployee.Where(t => t.HRME_EmployeeCode == data.newlstget1[i].Empcode && t.HRME_ActiveFlag == true).Select(r => r.HRME_Id).SingleOrDefault();


                                if (data.newlstget1[i].Type == "Internal")
                                {
                                    //Updating the Existing Records--------------------------------------------------------------------------------------------------------------------------------------
                                    /*&& t.MOMPRT_ParticipantsName == data.newlstget1[i].Participants*/
                                    var MOMPRTId = _HRMSContext.MOMParticipantsDMO.Where(t => (t.HRME_Id.Equals(empcode)) && t.MOM_Id == data.MOM_Id && t.MOMPRT_ParticipantsName == data.newlstget1[i].Participants).Select(p => p.MOMPRT_Id).FirstOrDefault();
                                    if (MOMPRTId > 0)
                                    {
                                        if (data.newlstget1[i].Participants != "" && data.newlstget1[i].Participants != null)
                                        {

                                            var MOMMNTId = _HRMSContext.MOMeetingMinutesDMO.Where(t => t.MOM_Id == data.MOM_Id && t.HRME_Id == empcode && t.MOMMNT_Names == data.newlstget1[i].Task
                                           && t.MOMMNT_ActionPlans == data.newlstget1[i].ActionPlans && t.MOMMNT_TargetDate == TargetDate
                                            && t.MOMPRT_Id == MOMPRTId).Select(p => p.MOMMNT_Id).SingleOrDefault();
                                            if (MOMMNTId > 0)
                                            {
                                                var momupdate = _HRMSContext.MOMeetingMinutesDMO.Where(R => R.MOMMNT_Id == MOMMNTId).ToList();
                                                if (momupdate.Count > 0)
                                                {
                                                    foreach (var ind in momupdate)
                                                    {
                                                        ind.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                                        ind.MOMMNT_TargetDate = TargetDate;
                                                        ind.MOMMNT_Names = data.newlstget1[i].Task;
                                                        ind.MOMMNT_UpdatedBy = data.userId;
                                                        ind.MOMMNT_UpdatedDate = DateTime.Now;
                                                        _HRMSContext.Update(ind);
                                                        _HRMSContext.SaveChanges();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // DateTime? myDate = DateTime.Parse(data.newlstget1[i].TargetDate);
                                                enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                                enq.MOMMNT_TargetDate = TargetDate;
                                                enq.MOMMNT_Names = data.newlstget1[i].Task;
                                                enq.MOMPRT_Id = MOMPRTId;
                                                enq.MOM_Id = data.MOM_Id;
                                                enq.MOMMNT_ActiveFlag = true;
                                                enq.HRME_Id = empcode;
                                                enq.MOMMNT_CreatedDate = DateTime.Now;
                                                enq.MOMMNT_UpdatedDate = DateTime.Now;
                                                enq.MOMMNT_CreatedBy = data.userId;
                                                enq.MOMMNT_UpdatedBy = data.userId;
                                                _HRMSContext.Add(enq);
                                                _HRMSContext.SaveChanges();
                                            }
                                            try
                                            {
                                                var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0,@p1,@p2,@p3", empcode, data.userId, data.newlstget1[i].ActionPlans, TargetDate);
                                                if (outputval > 0)
                                                {
                                                    Console.WriteLine("Task Created");
                                                }
                                                else
                                                {
                                                    Console.WriteLine(" Task Not Created");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }

                                        }

                                    }

                                    //new record expect from MOMParticipantsDMO {internal dept}
                                    else
                                    {
                                        long MOMPRTIds = 0;
                                        data.albumNameArray = (from a in _HRMSContext.MasterEmployee
                                                               from b in _HRMSContext.HR_Master_Designation
                                                               from c in _HRMSContext.Emp_Email_Id
                                                               from d in _HRMSContext.Emp_MobileNo
                                                               where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_EmployeeCode == data.newlstget1[i].Empcode
                                                               && a.HRME_Id == c.HRME_Id && a.HRME_Id == d.HRME_Id && a.HRMDES_Id == b.HRMDES_Id)
                                                               select new MOMUpdateDTO
                                                               {
                                                                   HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                                   HRME_Id = a.HRME_Id,
                                                                   HRMDES_Id = b.HRMDES_Id,
                                                                   HRMDES_DesignationName = b.HRMDES_DesignationName,
                                                                   HRME_EmailId = c.HRMEM_EmailId,
                                                                   HRME_MobileNo = d.HRMEMNO_MobileNo
                                                               }).ToArray();

                                        if (data.albumNameArray.Length > 0)
                                        {
                                            //if (data.newlstget1[i].Attendence == "p" || data.newlstget1[i].Attendence == "P")
                                            //{
                                            //    enq1.MOMPRT_ActiveFlag = true;
                                            //}
                                            //else if (data.newlstget1[i].Attendence == "a" || data.newlstget1[i].Attendence == "A")
                                            //{
                                            //    enq1.MOMPRT_ActiveFlag = false;
                                            //}
                                            enq1.MOM_Id = data.MOM_Id;
                                            enq1.MOMPRT_ParticipantsName = data.albumNameArray[0].HRME_EmployeeFirstName;
                                            enq1.MOMPRT_Designation = data.albumNameArray[0].HRMDES_DesignationName;
                                            enq1.MOMPRT_EXTINTFlag = data.newlstget1[i].Type;
                                            enq1.MOMPRT_CreatedDate = DateTime.Now;
                                            enq1.MOMPRT_UpdatedDate = DateTime.Now;
                                            enq1.MOMPRT_CreatedBy = data.userId;
                                            enq1.MOMPRT_UpdatedBy = data.userId;
                                            enq1.MOMPRT_MobileNo = data.albumNameArray[0].HRME_MobileNo;
                                            enq1.MOMPRT_Email = data.albumNameArray[0].HRME_EmailId;
                                            enq1.HRME_Id = data.albumNameArray[0].HRME_Id;
                                            _HRMSContext.Add(enq1);
                                            _HRMSContext.SaveChanges();
                                            MOMPRTIds = enq1.MOMPRT_Id;

                                            DateTime? myDate = null;

                                            if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                            {
                                                if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                {
                                                    myDate = parsedDate;
                                                }
                                                // Optional: else you can handle invalid date format here, if needed
                                                // For example: log the issue or set a different default value
                                            }
                                            enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                            enq.MOMMNT_TargetDate = myDate;
                                            enq.MOMMNT_Names = data.newlstget1[i].Task;
                                            enq.MOMPRT_Id = MOMPRTIds;
                                            enq.MOM_Id = data.MOM_Id;
                                            enq.MOMMNT_ActiveFlag = true;
                                            enq.HRME_Id = data.albumNameArray[0].HRME_Id;
                                            enq.MOMMNT_CreatedDate = DateTime.Now;
                                            enq.MOMMNT_UpdatedDate = DateTime.Now;
                                            enq.MOMMNT_CreatedBy = data.userId;
                                            enq.MOMMNT_UpdatedBy = data.userId;
                                            _HRMSContext.Add(enq);
                                            _HRMSContext.SaveChanges();
                                        }

                                        else if (data.albumNameArray.Length == 0)
                                        {
                                            //if (data.newlstget1[i].Attendence == "p" || data.newlstget1[i].Attendence == "P")
                                            //{
                                            //    enq1.MOMPRT_ActiveFlag = true;
                                            //}
                                            //else if (data.newlstget1[i].Attendence == "a" || data.newlstget1[i].Attendence == "A")
                                            //{
                                            //    enq1.MOMPRT_ActiveFlag = false;
                                            //}

                                            enq1.MOM_Id = data.MOM_Id;
                                            enq1.MOMPRT_ParticipantsName = data.newlstget1[i].Participants;
                                            enq1.MOMPRT_Designation = data.newlstget1[i].Designation;
                                            enq1.MOMPRT_EXTINTFlag = data.newlstget1[i].Type;
                                            enq1.MOMPRT_CreatedDate = DateTime.Now;
                                            enq1.MOMPRT_UpdatedDate = DateTime.Now;
                                            enq1.MOMPRT_CreatedBy = data.userId;
                                            enq1.MOMPRT_UpdatedBy = data.userId;
                                            enq1.MOMPRT_MobileNo = Convert.ToInt64(data.newlstget1[i].MobileNo);
                                            enq1.MOMPRT_Email = data.newlstget1[i].Email;
                                            enq1.HRME_Id = Convert.ToInt64(data.newlstget1[i].Empcode);
                                            _HRMSContext.Add(enq1);
                                            _HRMSContext.SaveChanges();
                                            MOMPRTIds = enq1.MOMPRT_Id;

                                            DateTime? myDate = null;

                                            if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                            {
                                                if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                {
                                                    myDate = parsedDate;
                                                }
                                                // Optional: else you can handle invalid date format here, if needed
                                                // For example: log the issue or set a different default value
                                            }
                                            enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                            enq.MOMMNT_TargetDate = myDate;
                                            enq.MOMMNT_Names = data.newlstget1[i].Task;
                                            enq.MOM_Id = data.MOM_Id;
                                            enq.MOMPRT_Id = MOMPRTIds;
                                            enq.MOMMNT_ActiveFlag = true;
                                            enq.HRME_Id = Convert.ToInt64(data.newlstget1[i].Empcode);
                                            enq.MOMMNT_CreatedDate = DateTime.Now;
                                            enq.MOMMNT_UpdatedDate = DateTime.Now;
                                            enq.MOMMNT_CreatedBy = data.userId;
                                            enq.MOMMNT_UpdatedBy = data.userId;
                                            _HRMSContext.Add(enq);
                                            _HRMSContext.SaveChanges();
                                        }
                                        try
                                        {
                                            var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0, @p1, @p2, @p3", empcode, data.userId, data.newlstget1[i].ActionPlans, enq.MOMMNT_TargetDate, data.MOM_Id);
                                            if (outputval > 0)
                                            {
                                                Console.WriteLine("Task Created");
                                            }
                                            else
                                            {
                                                Console.WriteLine(" Task Not Created");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                                if (data.newlstget1[i].Type == "External")
                                {
                                    var EXMOMPRTId = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOMPRT_ParticipantsName.ToLower() == data.newlstget1[i].Participants.ToLower() && t.MOMPRT_Email == data.newlstget1[i].Email && t.MOM_Id == data.MOM_Id).Select(p => p.MOMPRT_Id).FirstOrDefault();
                                    if (EXMOMPRTId > 0)
                                    {
                                        if (data.newlstget1[i].Participants != "" && data.newlstget1[i].Participants != null)
                                        {
                                            //var exupdateemp = _HRMSContext.MOMParticipantsDMO.Where(R => R.MOMPRT_Id == EXMOMPRTId).ToList();
                                            //if (exupdateemp.Count > 0)
                                            //{
                                            //    foreach (var exdd in exupdateemp)
                                            //    {
                                            //        //if (data.newlstget1[i].Attendence == "p" || data.newlstget1[i].Attendence == "P")
                                            //        //{
                                            //        //    enq1.MOMPRT_ActiveFlag = true;
                                            //        //}
                                            //        //else if (data.newlstget1[i].Attendence == "a" || data.newlstget1[i].Attendence == "A")
                                            //        //{
                                            //        //    enq1.MOMPRT_ActiveFlag = false;
                                            //        //}
                                            //        exdd.MOMPRT_ActiveFlag = enq1.MOMPRT_ActiveFlag;
                                            //        exdd.MOMPRT_UpdatedDate = DateTime.Now;
                                            //        exdd.MOMPRT_UpdatedBy = data.userId;
                                            //        _HRMSContext.Update(exdd);
                                            //        _HRMSContext.SaveChanges();
                                            //    }
                                            //}

                                            var EXMOMMNTId = _HRMSContext.MOMeetingMinutesDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMMNT_TargetDate == TargetDate
                                            && t.MOMMNT_ActionPlans == data.newlstget1[i].ActionPlans && t.MOMMNT_Names == data.newlstget1[i].Task && t.MOMPRT_Id == EXMOMPRTId).Select(p => p.MOMMNT_Id).SingleOrDefault();
                                            if (EXMOMMNTId > 0)
                                            {
                                                var exmomupdate = _HRMSContext.MOMeetingMinutesDMO.Where(R => R.MOMMNT_Id == EXMOMMNTId).ToList();
                                                if (exmomupdate.Count > 0)
                                                {
                                                    foreach (var exd in exmomupdate)
                                                    {
                                                        exd.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                                        exd.MOMMNT_TargetDate = TargetDate;
                                                        exd.MOMMNT_Names = data.newlstget1[i].Task;
                                                        exd.MOMPRT_Id = EXMOMPRTId;
                                                        exd.MOMMNT_UpdatedBy = data.userId;
                                                        exd.MOMMNT_UpdatedDate = DateTime.Now;
                                                        _HRMSContext.Update(exd);
                                                        _HRMSContext.SaveChanges();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                DateTime? myDate = null;

                                                if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                                {
                                                    if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                                    {
                                                        myDate = parsedDate;
                                                    }
                                                    // Optional: else you can handle invalid date format here, if needed
                                                    // For example: log the issue or set a different default value
                                                }
                                                enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                                enq.MOMMNT_TargetDate = myDate;
                                                enq.MOMMNT_Names = data.newlstget1[i].Task;
                                                enq.MOM_Id = data.MOM_Id;
                                                enq.MOMPRT_Id = EXMOMPRTId;
                                                enq.MOMMNT_ActiveFlag = true;
                                                enq.HRME_Id = Convert.ToInt64(data.newlstget1[i].Empcode);
                                                enq.MOMMNT_CreatedDate = DateTime.Now;
                                                enq.MOMMNT_UpdatedDate = DateTime.Now;
                                                enq.MOMMNT_CreatedBy = data.userId;
                                                enq.MOMMNT_UpdatedBy = data.userId;
                                                _HRMSContext.Add(enq);
                                                _HRMSContext.SaveChanges();
                                                //try
                                                //{
                                                //    var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0, @p1, @p2, @p3", empcode, data.userId, data.newlstget1[i].ActionPlans, myDate);
                                                //    if (outputval > 0)
                                                //    {
                                                //        Console.WriteLine("Task Created");
                                                //    }
                                                //    else
                                                //    {
                                                //        Console.WriteLine(" Task Not Created");
                                                //    }
                                                //}
                                                //catch (Exception ex)
                                                //{
                                                //    Console.WriteLine(ex.Message);
                                                //}
                                            }

                                        }

                                    }

                                    //new record expect from MOMParticipantsDMO {external dept}
                                    else
                                    {
                                        long EXMOMPRTIds = 0;
                                        //if (data.newlstget1[i].Attendence == "p" || data.newlstget1[i].Attendence == "P")
                                        //{
                                        //    enq1.MOMPRT_ActiveFlag = true;
                                        //}
                                        //else if (data.newlstget1[i].Attendence == "a" || data.newlstget1[i].Attendence == "A")
                                        //{
                                        //    enq1.MOMPRT_ActiveFlag = false;
                                        //}
                                        enq1.MOM_Id = data.MOM_Id;
                                        enq1.MOMPRT_ParticipantsName = data.newlstget1[i].Participants;
                                        enq1.MOMPRT_Designation = data.newlstget1[i].Designation;
                                        enq1.MOMPRT_EXTINTFlag = data.newlstget1[i].Type;
                                        enq1.MOMPRT_CreatedDate = DateTime.Now;
                                        enq1.MOMPRT_UpdatedDate = DateTime.Now;
                                        enq1.MOMPRT_CreatedBy = data.userId;
                                        enq1.MOMPRT_UpdatedBy = data.userId;
                                        enq1.MOMPRT_MobileNo = Convert.ToInt64(data.newlstget1[i].MobileNo);
                                        enq1.MOMPRT_Email = data.newlstget1[i].Email;
                                        enq1.HRME_Id = Convert.ToInt64(data.newlstget1[i].Empcode);
                                        _HRMSContext.Add(enq1);
                                        _HRMSContext.SaveChanges();
                                        //EXMOMPRTIds = enq1.MOMPRT_Id;

                                        DateTime? myDate = null;

                                        if (!string.IsNullOrEmpty(data.newlstget1[i].TargetDate))
                                        {
                                            if (DateTime.TryParse(data.newlstget1[i].TargetDate, out DateTime parsedDate))
                                            {
                                                myDate = parsedDate;
                                            }
                                            // Optional: else you can handle invalid date format here, if needed
                                            // For example: log the issue or set a different default value
                                        }
                                        enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans;
                                        enq.MOMMNT_TargetDate = myDate;
                                        enq.MOMMNT_Names = data.newlstget1[i].Task;
                                        //enq.MOMPRT_Id = EXMOMPRTIds;
                                        enq.MOM_Id = data.MOM_Id;
                                        enq.MOMMNT_ActiveFlag = true;
                                        enq.HRME_Id = Convert.ToInt64(data.newlstget1[i].Empcode);
                                        enq.MOMMNT_CreatedDate = DateTime.Now;
                                        enq.MOMMNT_UpdatedDate = DateTime.Now;
                                        enq.MOMMNT_CreatedBy = data.userId;
                                        enq.MOMMNT_UpdatedBy = data.userId;
                                        _HRMSContext.Add(enq);
                                        _HRMSContext.SaveChanges();
                                        //try
                                        //{
                                        //    var outputval = _HRMSContext.Database.ExecuteSqlCommand("ISM_TaskCreation_INSERT  @p0, @p1, @p2, @p3", empcode, data.userId, data.newlstget1[i].ActionPlans, myDate);
                                        //    if (outputval > 0)
                                        //    {
                                        //        Console.WriteLine("Task Created");
                                        //    }
                                        //    else
                                        //    {
                                        //        Console.WriteLine(" Task Not Created");
                                        //    }
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    Console.WriteLine(ex.Message);
                                        //}
                                    }

                                }

                            }

                        }

                    }

                    if (data.FilePath_Array.Length > 0)
                    {
                        foreach (var filedata in data.FilePath_Array)
                        {
                            MOMFileDMO file = new MOMFileDMO();
                            file.MOMFL_FileName = filedata.FileName;
                            file.MOMFL_FilePath = filedata.FilePath;
                            file.MOM_Id = data.MOM_Id;
                            file.MOMFL_ActiveFlag = true;
                            file.MOMFL_CreatedBy = data.userId;
                            file.MOMFL_UpdatedBy = data.userId;
                            file.MOMFL_CreatedDate = DateTime.Now;
                            file.MOMFL_UpdatedDate = DateTime.Now;
                            _HRMSContext.Add(file);
                            _HRMSContext.SaveChanges();
                        }

                    }

                    if (data.MOM_Minutes != "" && data.MOM_Minutes != null)
                    {
                        var updateminutes = _HRMSContext.MOMinutesDMO.Where(R => R.MOM_Id == data.MOM_Id).ToList();
                        if (updateminutes.Count > 0)
                        {
                            foreach (var d in updateminutes)
                            {
                                d.MOM_Minutes = data.MOM_Minutes;
                                d.MOM_TentativeEndTime = data.MOM_EndTime;
                                d.MOM_TentativeStartTime = data.MOM_StartTime;
                                d.MOM_Status = "Completed";
                                d.MOM_Flag = "Updated";
                                _HRMSContext.Update(d);
                                _HRMSContext.SaveChanges();
                            }
                            if (updateminutes.Count > 0)
                            {
                                data.retrunMsg = "Update";
                            }
                            else
                            {
                                data.retrunMsg = "false";
                            }
                        }

                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }

            return data;
        }
        public MOMUpdateDTO getonloadempdata(MOMUpdateDTO data)
        {
            data.empdata = _HRMSContext.MasterEmployee.Where(e => e.HRME_ActiveFlag == true).ToArray();
            return data;

        }

        public async Task<MOMUpdateDTO> checkvalidation(MOMUpdateDTO data)
        {
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            try
            {
                for (int i = 0; i < data.newlstget1.Length; i++)
                {
                    try
                    {
                        MOMeetingMinutesDMO enq = new MOMeetingMinutesDMO();
                        MOMParticipantsDMO enq1 = new MOMParticipantsDMO();

                        //---------- TYPE---------//
                        try
                        {
                            if ((data.newlstget1[i].Type.TrimEnd().TrimStart() != null) && (data.newlstget1[i].Type.TrimEnd().TrimStart() != ""))
                            {
                                if ((Regex.IsMatch(data.newlstget1[i].Type.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$")) && ((data.newlstget1[i].Type.TrimEnd().TrimStart()).Length <= 50))
                                {
                                    enq1.MOMPRT_EXTINTFlag = data.newlstget1[i].Type.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    data.stuStatus = "Type Name is not Valid for" + " " + data.newlstget1[i].Participants;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Type Name can not be null for" + " " + data.newlstget1[i].Participants;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Type Name Header Is Changed , Header Should Be Type";
                            return data;
                        }

                        //---------- Name---------//
                        try
                        {
                            if ((data.newlstget1[i].Participants.TrimEnd().TrimStart() != null) && (data.newlstget1[i].Participants.TrimEnd().TrimStart() != ""))
                            {
                                if (data.newlstget1[i].Participants.Length != 0)
                                {
                                    enq1.MOMPRT_ParticipantsName = data.newlstget1[i].Participants.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    data.stuStatus = "Participants Name is not Valid for" + " " + data.newlstget1[i].Participants;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Participants Name can not be null for" + " " + data.newlstget1[i].Participants;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Participants Name Header Is Changed , Header Should Be Participants";
                            return data;
                        }
                        //---------- Group---------//
                        try
                        {
                            if ((data.newlstget1[i].Task.TrimEnd().TrimStart() != null) && (data.newlstget1[i].Task.TrimEnd().TrimStart() != ""))
                            {
                                if (((data.newlstget1[i].Task.TrimEnd().TrimStart()).Length <= 100))
                                {
                                    enq.MOMMNT_Names = data.newlstget1[i].Task.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    data.stuStatus = "Group/Task Name is not Valid for" + " " + data.newlstget1[i].Participants;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Group/Task Name can not be null for" + " " + data.newlstget1[i].Participants;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Group Name Header Is Changed , Header Should Be Group";
                            return data;
                        }
                        //---------- Designation---------//
                        try
                        {
                            if (data.newlstget1[i].Designation == null)
                            {

                            }
                            else if (data.newlstget1[i].Designation.Length > 0)
                            {
                                enq1.MOMPRT_Designation = data.newlstget1[i].Designation.TrimEnd().TrimStart();
                            }

                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Designation Name Header Is Changed , Header Should Be Designation";
                            return data;
                        }
                        //---------- Email---------//


                        try
                        {
                            if (data.newlstget1[i].Email == null || data.newlstget1[i].Email == "")
                            {

                            }
                            else if (data.newlstget1[i].Email != null || data.newlstget1[i].Email != "")
                            {
                                enq1.MOMPRT_Email = data.newlstget1[i].Email.TrimEnd().TrimStart();

                            }
                            //else if (data.newlstget1[i].Type == "Internal" || data.newlstget1[i].Type == "internal")
                            //{

                            //    if ((Convert.ToString(data.newlstget1[i].Email.TrimEnd().TrimStart()) != null) && (Convert.ToString(data.newlstget1[i].Email.TrimEnd().TrimStart()) != ""))
                            //    {
                            //        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                            //        Match match = regex.Match(data.newlstget1[i].Email.TrimEnd().TrimStart());
                            //        if (match.Success)
                            //        {
                            //            enq1.MOMPRT_Email = data.newlstget1[i].Email.TrimEnd().TrimStart();

                            //        }
                            //        else
                            //        {
                            //            data.stuStatus = "Email Id is not Valid for" + " " + data.newlstget1[i].Participants;
                            //            return data;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        data.stuStatus = "Email Id can not be Empty for" + " " + data.newlstget1[i].Participants;
                            //        return data;
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Email Header Is Changed , Header Should Be Email";
                            return data;
                        }

                        //---------- ActionPlans---------//
                        try
                        {
                            if ((data.newlstget1[i].ActionPlans.TrimEnd().TrimStart() != null) && (data.newlstget1[i].ActionPlans.TrimEnd().TrimStart() != ""))
                            {
                                if (data.newlstget1[i].ActionPlans.Length > 0)
                                {
                                    enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    data.stuStatus = "ActionPlans is not Valid for" + " " + data.newlstget1[i].Participants;
                                    return data;
                                }
                            }

                            // Store ActionPlans with newlines preserved
                            //if ((data.newlstget1[i].ActionPlans.TrimEnd().TrimStart() != null) &&
                            //    (data.newlstget1[i].ActionPlans.TrimEnd().TrimStart() != ""))
                            //{
                            //    if (data.newlstget1[i].ActionPlans.Length > 0)
                            //    {
                            //        string formattedActionPlans = data.newlstget1[i].ActionPlans.TrimEnd().TrimStart();

                            //         Make sure newlines are handled (replace newlines with HTML <br> if rendering on HTML)
                            //        formattedActionPlans = formattedActionPlans.Replace("\r\n", "<br>").Replace("\n", "<br>");

                            //         Store the formatted ActionPlans
                            //        enq.MOMMNT_ActionPlans = formattedActionPlans;
                            //    }
                            //    else
                            //    {
                            //        data.stuStatus = "ActionPlans is not Valid for" + " " + data.newlstget1[i].Participants;
                            //        return data;
                            //    }
                            //}


                            else
                            {
                                data.stuStatus = "ActionPlans can not be null for" + " " + data.newlstget1[i].Participants;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "ActionPlans Header Is Changed , Header Should Be ActionPlans";
                            return data;
                        }
                        //---  Mobile Number --//
                        try
                        {

                            if (data.newlstget1[i].Empcode == null || data.newlstget1[i].Empcode == "")
                            {

                            }
                            else if (data.newlstget1[i].Empcode != null || data.newlstget1[i].Empcode != "")
                            {

                                var HRMEIdss = _HRMSContext.MasterEmployee.Where(t => t.HRME_EmployeeCode == data.newlstget1[i].Empcode.TrimEnd().TrimStart()).ToList();
                                //enq1.HRME_Id = Convert.ToInt32(data.newlstget1[i].Empcode.TrimEnd().TrimStart());

                            }
                            // enq.MOMMNT_ActionPlans = data.newlstget1[i].ActionPlans.TrimEnd().TrimStart();
                            //if ((Convert.ToString(data.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(data.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != ""))
                            //{
                            //    string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                            //    if (Regex.IsMatch((Convert.ToString(data.newlstget1[i].MobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                            //    {
                            //        enq1.MOMPRT_MobileNo = Convert.ToInt64(data.newlstget1[i].MobileNo.TrimEnd().TrimStart());
                            //    }
                            //    else
                            //    {
                            //        data.stuStatus = "Mobile Number is not Valid for" + " " + data.newlstget1[i].Participants;
                            //        return data;
                            //    }
                            //}
                            //else
                            //{

                            //}

                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Empcode Header Is Changed , Header Should Be Empcode";
                            return data;
                        }

                        //--- TargetDate --//
                        //try
                        //{
                        //    if ((Convert.ToString(data.newlstget1[i].TargetDate) != null) && (Convert.ToString(data.newlstget1[i].TargetDate) != ""))
                        //    {

                        //        if (data.newlstget1[i].TargetDate.Length > 0)
                        //        {
                        //            enq.MOMMNT_TargetDate = Convert.ToDateTime(data.newlstget1[i].TargetDate);
                        //        }
                        //        else
                        //        {
                        //            data.stuStatus = "TargetDate is not Valid for" + " " + data.newlstget1[i].Participants;
                        //            return data;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        data.stuStatus = "TargetDate can not be Empty for" + " " + data.newlstget1[i].Participants;
                        //        return data;
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    data.stuStatus = "TargetDate Header Is Changed , Header Should Be TargetDate";
                        //    return data;
                        //}
                        try
                        {

                            DateTime targetDate;
                            if (DateTime.TryParseExact(data.newlstget1[i].TargetDate, new[] { "yyyy-MM-dd", "dd-MM-yyyy", "yyyy/MM/dd", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out targetDate))
                            {
                                enq.MOMMNT_TargetDate = targetDate;
                            }
                            else
                            {
                                enq.MOMMNT_TargetDate = targetDate;
                                //data.stuStatus = "TargetDate is not a valid date format for " + data.newlstget1[i].Participants + ". Please use either yyyy-MM-dd or dd-MM-yyyy.";
                                //return data;
                            }


                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "An error occurred while processing the TargetDate.";
                            return data;
                        }
                        //--- Attendence --//
                        //try
                        //{
                        //    if ((Convert.ToString(data.newlstget1[i].Attendence.TrimEnd().TrimStart()) != null) && (Convert.ToString(data.newlstget1[i].Attendence.TrimEnd().TrimStart()) != ""))
                        //    {
                        //        string MatchPhoneNumberPattern = @"([a-zA-Z])$";
                        //        if (Regex.IsMatch((Convert.ToString(data.newlstget1[i].Attendence.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                        //        {
                        //            // enq1.MOMPRT_MobileNo = Convert.ToInt64(data.newlstget1[i].Attendence.TrimEnd().TrimStart());
                        //        }
                        //        else
                        //        {
                        //            data.stuStatus = "Attendence is not Valid for" + " " + data.newlstget1[i].Participants;
                        //            return data;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        data.stuStatus = "Attendence can not be Empty for" + " " + data.newlstget1[i].Participants;
                        //        return data;
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    data.stuStatus = "Attendence  Header Is Changed , Header Should Be Attendence";
                        //    return data;
                        //}




                    }
                    catch (Exception ex)
                    {
                        // Console.Write(ex.Message);
                        failcount = failcount + 1;
                        string name = failnames;
                        finalnames += name + ",";
                        data.stuStatus = "Select Proper Excel Sheet";
                        return data;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogInformation(" Import Checking Validation" + ex.Message);
            }
            return data;
        }


        //***********************************Approval**************************////////////////////////
        public MOMUpdateDTO getdata(MOMUpdateDTO data)
        {
            try
            {

                data.getapproval = _HRMSContext.MOMinutesDMO.Where(t => t.MI_Id == data.MI_Id && t.MOM_ActiveFlag == true && t.MOM_Status == "Completed" && t.MOM_Flag == "Updated").ToArray();
                data.getapproveddata = _HRMSContext.MOMinutesDMO.Where(t => t.MI_Id == data.MI_Id && t.MOM_ActiveFlag == true && t.MOM_Status == "Completed" && t.MOM_Flag == "Approved").ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public MOMUpdateDTO approvaldata(MOMUpdateDTO data)
        {
            try
            {
                if (data.MOMPRT_Id > 0)
                {
                    var result = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOMPRT_Id == data.MOMPRT_Id).FirstOrDefault();

                    if (result.MOMPRT_ActiveFlag == true)
                    {
                        result.MOMPRT_ActiveFlag = false;
                    }
                    else if (result.MOMPRT_ActiveFlag == false)
                    {
                        result.MOMPRT_ActiveFlag = true;
                    }
                    result.MOMPRT_UpdatedDate = DateTime.Now;
                    _HRMSContext.Update(result);

                    int rowAffected = _HRMSContext.SaveChanges();
                    if (rowAffected > 0)
                    {
                        data.retrunMsg = "true";
                    }
                    else
                    {
                        data.retrunMsg = "false";
                    }
                }
                else
                {
                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "MinOfMintsApproval";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MOM_Id", SqlDbType.BigInt)
                        {
                            Value = data.MOM_Id
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dtoReader = cmd.ExecuteReader())
                            {
                                while (dtoReader.Read())
                                {
                                    var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                    {
                                        dtoRow.Add(
                                        dtoReader.GetName(iFiled),
                                        dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dtoRow);
                                }
                            }
                            data.approvaldata = retObject.ToArray();
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
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public MOMUpdateDTO getstatus(MOMUpdateDTO data)
        {
            data.retrunMsg = "";
            var email = "";
            try
            {

                List<MOMParticipantsDMO> participentslist = new List<MOMParticipantsDMO>();
                List<MOMinutesDMO> MOMName = new List<MOMinutesDMO>();
                //participentslist = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMPRT_ActiveFlag == true).ToList();
                participentslist = _HRMSContext.MOMParticipantsDMO.Where(t => t.MOM_Id == data.MOM_Id ).ToList();
                MOMName = _HRMSContext.MOMinutesDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOM_ActiveFlag == true).ToList();
                data.MOM_Flag = "Approved";
                var result12 = _HRMSContext.MOMinutesDMO.Single(t => t.MOM_Id == data.MOM_Id);
                result12.MOM_UpdatedDate = DateTime.Now;
                result12.MOM_UpdatedBy = data.userId;
                result12.MOM_Flag = data.MOM_Flag;
                _HRMSContext.Update(result12);
                var flag = _HRMSContext.SaveChanges();
                if (flag > 0)
                {
                    data.retrunMsg = "Update";
                }
                else
                {
                    data.retrunMsg = "false";
                }
                if (participentslist.Count > 0)
                {
                    for (int k = 0; k < participentslist.Count; k++)
                    {
                        if (participentslist[k].MOMPRT_Email !=null && participentslist[k].MOMPRT_Email !="")
                        {
                            if(email !=null && email !="")
                            {
                                //email = participentslist[k].MOMPRT_Email;
                                email = email + "," + participentslist[k].MOMPRT_Email;
                            }
                            else
                            {
                                email = participentslist[k].MOMPRT_Email;
                             //   email = email +","+ participentslist[k].MOMPRT_Email;
                            }
                        }

                    }

                    //added By New Aman 
                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _HRMSContext.smsEmailSetting.Where(e => e.ISES_Template_Name == "MOMCreated" && e.MI_Id == 17 && e.ISES_MailActiveFlag == true).ToList();
                    string mailcc = template[0].ISES_MailCCId;
                    string mailbcc = "";
                    if (template.Count == 0)
                    {
                        return data;
                    }
                    var institutionName = _HRMSContext.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                    var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                    var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                    string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                    string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                    string Resultsms = Mailcontent;
                    string result = Mailmsg;
                    List<Match> variables = new List<Match>();
                    foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                    {
                        variables.Add(match);
                    }
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Email_Prametars_Replace";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_IDE",
                      SqlDbType.VarChar)
                        {
                            Value = data.MOM_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                      SqlDbType.VarChar)
                        {
                            Value = "MOMCreated"
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

                                        else if (datatype.Name == "Decimal")
                                        {
                                            var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();
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
                            Console.WriteLine(ex);
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
                    Mailcontent = Resultsms;
                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                    string Attechement = "";

                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;

                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string Subject = MOMName[0].MOM_Name.ToString();///meeting name
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                        smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();

                        if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                        {

                            var message = new SendGridMessage();
                            message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                            message.Subject = Subject;
                            if (email != null && email != "")
                            {
                                string[] toMail = email.Split(',');

                                foreach (var c in toMail)
                                {
                                    message.AddTo(c);
                                }
                            }
                            
                            var client = new SendGridClient(sengridkey);
                            client.SendEmailAsync(message).Wait();

                            StringBuilder sb = new StringBuilder(data.Template);
                            StringReader sr = new StringReader(sb.ToString());
                            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HtmlWorker htmlparser = new HtmlWorker(pdfDoc);


                            if (mailcc != null && mailcc != "")
                            {
                                string[] ccmaildetails = mailcc.Split(',');

                                foreach (var c in ccmaildetails)
                                {
                                    if (c != email)
                                    {
                                        message.AddCc(c);
                                    }
                                }
                            }
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                pdfDoc.Open();

                                htmlparser.Parse(sr);
                                pdfDoc.Close();

                                byte[] bytess = memoryStream.ToArray();
                                memoryStream.Close();

                                var file = Convert.ToBase64String(bytess);
                                string emp;
                                emp = Convert.ToString(sr);
                                string c = "";
                                string v = emp.Replace("System.IO.StringReader", "MOM.Pdf");
                                message.AddAttachment(v, file);
                                message.HtmlContent = Mailmsg;
                                var client1 = new SendGridClient(sengridkey);
                                client1.SendEmailAsync(message).Wait();
                            }

                        }
                        else
                        {
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
                                    emailMessage.To.Add(new MailAddress(email));
                                    emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                    emailMessage.Subject = Subject;
                                    emailMessage.Body = Mailmsg;
                                    emailMessage.IsBodyHtml = true;
                                    if (Attechement.Equals("1"))
                                    {
                                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
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
                                        string[] ccmaildetails = mailcc.Split(',');

                                        foreach (var c in ccmaildetails)
                                        {
                                            emailMessage.CC.Add(c);
                                        }
                                    }
                                    if (mailbcc != null && mailbcc != "")
                                    {
                                        string[] bccmaildetails = mailbcc.Split(',');

                                        foreach (var c in bccmaildetails)
                                        {
                                            emailMessage.Bcc.Add(c);
                                        }
                                    }
                                    //adedd

                                    //addd
                                    clientsmtp.Send(emailMessage);
                                }
                            }
                        }
                        using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "MOMCreated").Select(d => d.IVRMIM_Id).ToList();
                            var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                            var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
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
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }


        public MOMUpdateDTO oldmeeting(MOMUpdateDTO data)
        {
            try
            {
                data.oldmeetdetail = _HRMSContext.MOMinutesDMO.Where(t => t.MOM_ParentId == data.MOM_ParentId && t.MOM_ParentId != 0).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public string sendEmail(long MI_Id, string Template, long HRME_ID, string email, string ccemail, string MOM_Name)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = "<Pre>" + Mailcontent + "</Pre>";
                string result = Mailmsg;


                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Email_Prametars_Replace";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",
                  SqlDbType.VarChar)
                    {
                        Value = HRME_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
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

                                    else if (datatype.Name == "Decimal")
                                    {
                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();
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
                //[MSG]
                // [MSG]
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
                Mailmsg = "<p>" + result + "</p>";

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
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = MOM_Name;
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    string mailcc = "";
                    string mailbcc = "";

                    //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    //{
                    //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                    //    mailcc = ccmail[0].ToString();

                    //    if (ccmail.Length > 1)
                    //    {
                    //        if (ccmail[1] != null || ccmail[1] != "")
                    //        {
                    //            mailbcc = ccmail[1].ToString();
                    //        }
                    //    }

                    //}
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }



                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(email);

                    // mailto:message.addto("kavitajnandoor21@gmail.com");
                    //message.AddTo("sreepriyagangadharan15@gmail.com");

                    if (Attechement.Equals("1"))
                    {
                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                    if (ccemail != null && ccemail != "")
                    {
                        string[] ccmaildetails = ccemail.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }


                    //if (mailcc != null && mailcc != "")
                    //{
                    //    string[] ccmaildetails = mailcc.Split(',');

                    //    foreach (var c in ccmaildetails)
                    //    {
                    //        message.AddCc(c);
                    //    }
                    //}

                    //if (ccemail != null && ccemail != "")
                    //{
                    //    message.AddBcc(ccemail);
                    //}

                    //****************** EMAIL BCC DETAILS ***************//




                    if (mailcc != null && mailcc != "")
                    {
                        //message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
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



                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
            return "Add";

        }

        public MOMUpdateDTO sendapprovalMail(MOMUpdateDTO data)
        {
            try
            {
                //data = getstatus(data);



                string mailid = "vandana@vapstech.com";
                var institutionName = _Context.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                string Mailmsg = "Text And Text";

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string Subject = "Minutes Of Meeting";
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(mailid);

                    StringBuilder sb = new StringBuilder(data.Template);
                    StringReader sr = new StringReader(sb.ToString());
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                    message.AddCc("praveenraikar124@gmail.com");
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();

                        htmlparser.Parse(sr);
                        pdfDoc.Close();

                        byte[] bytess = memoryStream.ToArray();
                        memoryStream.Close();

                        var file = Convert.ToBase64String(bytess);
                        string emp;
                        emp = Convert.ToString(sr);
                        string c = "";
                        string v = emp.Replace("System.IO.StringReader", "MOM.xls");
                        message.AddAttachment(v, file);
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Resignation_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = "Minutes of meeting"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }




                if (data.retrunMsg == "Update")
                {
                    data.retrunMsg = "Add";
                }
                else

                {
                    data.retrunMsg = "Exist";
                }

            }
            catch (Exception ex)
            {
                data.retrunMsg = "False";
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //report*****************************//////////////////////////////////////////
        public MOMUpdateDTO getreport(MOMUpdateDTO data)
        {
            try
            {

                data.fillyear = (from a in _Context.HR_Master_LeaveYearDMO
                                 where (a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                 select new MOMUpdateDTO
                                 {
                                     HRMLY_Id = a.HRMLY_Id,
                                     HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                     HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                 }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                data.fillmonth = (from a in _Context.month
                                  select new MOMUpdateDTO
                                  {
                                      IVRM_Month_Id = a.IVRM_Month_Id,
                                      monthname = a.IVRM_Month_Name
                                  }).Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();

                return data;

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }

        public MOMUpdateDTO showreport(MOMUpdateDTO data)
        {

            try
            {
                var rolelist = _HRMSContext.masterRoleType.Where(t => t.IVRMRT_Id == data.roleId).ToList();
                data.IVRMRT_Role = rolelist.FirstOrDefault().IVRMRT_Role.ToLower();
                data.getfiledetails = _HRMSContext.MOMFileDMO.Where(t => t.MOMFL_ActiveFlag == true).ToArray();
                data.userId = 0;
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MOM_meeting_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                    {
                        Value = data.Fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                    {
                        Value = data.Todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Month_id", SqlDbType.BigInt)
                    {
                        Value = data.Month_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Year_id", SqlDbType.BigInt)
                    {
                        Value = data.Year_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flg ", SqlDbType.VarChar)
                    {
                        Value = data.flg
                    });
                    cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.momdetailreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public MOMUpdateDTO viewdetails(MOMUpdateDTO data)
        {

            try
            {
                data.getfiledetails = _HRMSContext.MOMFileDMO.Where(t => t.MOM_Id == data.MOM_Id && t.MOMFL_ActiveFlag == true).ToArray();
                data.getParticipentsdetails = (from a in _HRMSContext.MOMParticipantsDMO
                                               from b in _HRMSContext.MOMeetingMinutesDMO
                                               where (a.HRME_Id == b.HRME_Id && a.MOM_Id == data.MOM_Id)
                                               select new MOMUpdateDTO
                                               {
                                                   MOMPRT_ParticipantsName = a.MOMPRT_ParticipantsName,
                                                   MOMPRT_EXTINTFlag = a.MOMPRT_EXTINTFlag,
                                                   MOMPRT_Designation = a.MOMPRT_Designation,
                                                   MOMPRT_Email = a.MOMPRT_Email,
                                                   MOMPRT_MobileNo = a.MOMPRT_MobileNo,
                                                   MOMMNT_TargetDate = b.MOMMNT_TargetDate,
                                                   MOMMNT_ActionPlans = b.MOMMNT_ActionPlans,
                                                   MOMMNT_Names = b.MOMMNT_Names
                                               }).Distinct().ToArray();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public MOMUpdateDTO SaveVendors(MOMUpdateDTO data)
        {
            try
            {
                var existingData = _HRMSContext.INV_Master_SupplierDMO.Where(dm => dm.INVMS_SupplierName == data.INVMS_SupplierName && dm.INVMS_SupplierConatctNo == data.INVMS_SupplierConatctNo && dm.INVMS_EmailId == data.INVMS_EmailId).ToList();
                if (existingData.Count > 0)
                {
                    data.retrunMsg = "Duplicate";
                }
                else
                {
                    INV_Master_SupplierDMO cmc = new INV_Master_SupplierDMO();
                    cmc.MI_Id = data.MI_Id;
                    cmc.INVMS_SupplierName = data.INVMS_SupplierName;
                    cmc.INVMS_EmailId = data.INVMS_EmailId;
                    cmc.INVMS_SupplierConatctNo = data.INVMS_SupplierConatctNo;
                    cmc.CreatedDate = DateTime.Now;
                    cmc.INVMS_CreatedBy = data.userId;
                    cmc.UpdatedDate = DateTime.Now;
                    cmc.INVMS_UpdatedBy = data.userId;
                    _HRMSContext.Add(cmc);
                    var vv = _HRMSContext.SaveChanges();
                    if (vv > 0)
                    {
                        data.retrunMsg = "Add";
                    }
                    //else
                    //{
                    //    data.retrunMsg = "Notsave";
                    //}
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "admin";
            }
            return data;
        }


        public MOMUpdateDTO onloadVendors(MOMUpdateDTO data)
        {
            try
            {
                data.onloadVendors = _HRMSContext.INV_Master_SupplierDMO.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
    }
}
