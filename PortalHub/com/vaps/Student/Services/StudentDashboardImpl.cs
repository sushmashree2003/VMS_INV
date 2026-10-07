using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Employee;
using System.Globalization;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.MobileApp;

namespace PortalHub.com.vaps.Student.Services
{
    public class StudentDashboardImpl : Interfaces.StudentDashboardInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _studentDashboardContext;
        public StudentDashboardImpl(PortalContext studentDashboardContext)
        {
            _studentDashboardContext = studentDashboardContext;
        }
        public async Task<StudentDashboardDTO> Getdetails(StudentDashboardDTO data)
        {
            try
            {
                data.MI_Id = data.MI_Id;
                #region Student Details
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_StudentDashboard";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.studetailslist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Fee Details
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_FEE_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                     SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.studentfeedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Fee Details
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_MONTHLY_FEE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                     SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.academicyearFeedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Student Monthly Attendance 
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.academicyearAttendancedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region  CALENDER and Notification of EVENTS
                try
                {
                    data.calenderlist = (from m in _studentDashboardContext.COE_Master_EventsDMO
                                         from n in _studentDashboardContext.COE_EventsDMO
                                         from o in _studentDashboardContext.School_Adm_Y_StudentDMO
                                         where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && o.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate != null)
                                         select new StudentDashboardDTO
                                         {
                                             COEME_EventName = m.COEME_EventName,
                                             COEME_EventDesc = m.COEME_EventDesc,
                                             COEE_EStartDate = n.COEE_EStartDate,
                                             COEE_EEndDate = n.COEE_EEndDate,
                                             COEE_ReminderDate = n.COEE_ReminderDate
                                         }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                    int month = DateTime.Now.Month;
                    data.coereportlist = (from m in _studentDashboardContext.COE_Master_EventsDMO
                                          from n in _studentDashboardContext.COE_EventsDMO
                                          from o in _studentDashboardContext.School_Adm_Y_StudentDMO
                                          where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && o.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == month)
                                          select new StudentDashboardDTO
                                          {
                                              COEME_EventName = m.COEME_EventName,
                                              COEME_EventDesc = m.COEME_EventDesc,
                                              COEE_EStartDate = n.COEE_EStartDate,
                                              COEE_EEndDate = n.COEE_EEndDate,
                                              COEE_ReminderDate = n.COEE_ReminderDate
                                          }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion

                #region SPORTS DETAILS
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_SportsDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.sportsdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region LIBRARY DETAILS
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_LibraryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.librarydetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion


                #region  Class/Section
                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                if (clssec1.Count == 0)
                {
                    data.messag = "";
                }
                else
                {
                    #region  Notice Board and Assignment
                    try
                    {
                        var date = DateTime.Now;

                        //data.noticelist = _studentDashboardContext.IVRM_NoticeBoardDMO.Where(n => n.MI_Id == data.MI_Id && n.INTB_ActiveFlag == true).Distinct().OrderByDescending(d => d.INTB_StartDate).ToArray();

                        data.noticelist = (from a in _studentDashboardContext.IVRM_NoticeBoardDMO
                                           from b in _studentDashboardContext.IVRM_NoticeBoard_Class_DMO
                                           where (a.INTB_Id == b.INTB_Id && a.INTB_ActiveFlag == true && a.MI_Id == data.MI_Id && b.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id)
                                           select new StudentDashboardDTO
                                           {
                                               MI_Id = a.MI_Id,
                                               INTB_Id = a.INTB_Id,
                                               INTB_Title = a.INTB_Title,
                                               INTB_Description = a.INTB_Description,
                                               NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                                               INTB_Attachment = a.INTB_Attachment,
                                               INTB_FilePath = a.INTB_FilePath,
                                               INTB_DisplayDate = a.INTB_DisplayDate,
                                               INTB_StartDate = a.INTB_StartDate,
                                               INTB_EndDate = a.INTB_EndDate,
                                               ASMCL_Id = b.ASMCL_Id,
                                           }).Distinct().OrderByDescending(d => d.INTB_StartDate).ToArray();


                        //data.assignmentlist = _studentDashboardContext.IVRM_ClassWorkDMO.Where(n => n.MI_Id == data.MI_Id).ToArray();

                        data.assignmentlist = (from m in _studentDashboardContext.IVRM_ClassWorkDMO
                                               from n in _studentDashboardContext.School_M_Class
                                               from o in _studentDashboardContext.School_M_Section
                                               from p in _studentDashboardContext.AcademicYearDMO
                                               where (m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ASMAY_Id == p.ASMAY_Id && m.ASMAY_Id == data.ASMAY_Id && m.MI_Id == data.MI_Id
                                               && m.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id
                                               && m.ASMS_Id == clssec1.FirstOrDefault().ASMS_Id
                                               && m.ICW_ActiveFlag == true)
                                               select new StudentDashboardDTO
                                               {
                                                   ASMCL_ClassName = n.ASMCL_ClassName,
                                                   ICW_Content = m.ICW_Content,
                                                   ICW_Topic = m.ICW_Topic,
                                                   ICW_SubTopic = m.ICW_SubTopic,
                                                   ICW_FromDate = m.ICW_FromDate,
                                                   ICW_ToDate = m.ICW_ToDate,
                                                   ICW_Assignment = m.ICW_Assignment,
                                                   ICW_ActiveFlag = m.ICW_ActiveFlag,
                                                   ICW_Id = m.ICW_Id,
                                                   ICW_Evaluation = m.ICW_Evaluation,
                                                   ICW_Attachment = m.ICW_Attachment,
                                                   ICW_FilePath = m.ICW_FilePath,
                                                   ASMCL_Id = m.ASMCL_Id,

                                               }).Distinct().OrderByDescending(d => d.ICW_FromDate).ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion

                    #region  Syallbus ProJect TT
                    try
                    {
                        data.docs_Details = (from a in _studentDashboardContext.IVRM_DocsUploadDMO
                                             from b in _studentDashboardContext.School_M_Class
                                             from c in _studentDashboardContext.School_M_Section
                                             where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && a.IDU_ActiveFlag == true)
                                             select new StudentDashboardDTO
                                             {
                                                 ASMCL_ClassName = b.ASMCL_ClassName,
                                                 ASMC_SectionName = c.ASMC_SectionName,
                                                 IDU_Type = a.IDU_Type,
                                                 IDU_Attachment = a.IDU_Attachment,
                                                 IDU_ActiveFlag = a.IDU_ActiveFlag,
                                                 IDU_Id = a.IDU_Id
                                             }).OrderBy(a => a.IDU_Id).Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion



                    #region  Documents and gallery Images 
                    try
                    {
                        var studdetails = (from a in _studentDashboardContext.IVRM_DocsUploadDMO
                                           from b in _studentDashboardContext.School_M_Class
                                           from c in _studentDashboardContext.School_M_Section
                                           where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && a.IDU_ActiveFlag == true && a.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clssec1.FirstOrDefault().ASMS_Id)
                                           select new StudentDashboardDTO
                                           {
                                               ASMCL_ClassName = b.ASMCL_ClassName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               IDU_Type = a.IDU_Type,
                                               IDU_Attachment = a.IDU_Attachment,
                                               IDU_ActiveFlag = a.IDU_ActiveFlag,
                                               IDU_Id = a.IDU_Id
                                           }).OrderBy(a => a.IDU_Id).Distinct().ToArray();
                        if (studdetails.Length > 0)
                        {
                            data.docs_Details = studdetails;
                        }

                        //var imglist = _studentDashboardContext.IVRM_ImageUploadDMO.Where(i => i.MI_Id == data.MI_Id && i.IIU_ActiveFlag == true).OrderBy(o => o.IIU_Order).ToArray();
                        //if (imglist.Length > 0)
                        //{
                        //    data.imgs_list = imglist;
                        //}

                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine(Ex.Message);
                    }
                    #endregion

                    #region  details For HomeWork 
                    try
                    {
                        #region
                        //using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                        //{
                        //    cmd.CommandText = "Portal_HomeWork";
                        //    cmd.CommandType = CommandType.StoredProcedure;

                        //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        //SqlDbType.BigInt)
                        //    {
                        //        Value = data.MI_Id
                        //    });
                        //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        //     SqlDbType.BigInt)
                        //    {
                        //        Value = data.ASMAY_Id
                        //    });


                        //    if (cmd.Connection.State != ConnectionState.Open)
                        //        cmd.Connection.Open();

                        //    var retObject = new List<dynamic>();
                        //    try
                        //    {
                        //        using (var dataReader = await cmd.ExecuteReaderAsync())
                        //        {
                        //            while (await dataReader.ReadAsync())
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
                        //        data.homeworklist = retObject.ToArray();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Console.WriteLine(ex.Message);
                        //    }
                        //}
                        #endregion

                        data.homeworklist = (from a in _studentDashboardContext.IVRM_Homework_DMO
                                             from b in _studentDashboardContext.IVRM_Master_SubjectsDMO
                                             from c in _studentDashboardContext.School_Adm_Y_StudentDMO
                                             from d in _studentDashboardContext.AcademicYearDMO
                                             where (a.ISMS_Id == b.ISMS_Id && a.ASMCL_Id == c.ASMCL_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clssec1.FirstOrDefault().ASMS_Id
                                             && a.MI_Id == data.MI_Id && c.AMAY_ActiveFlag == 1
                                             && a.IHW_ActiveFlag == true)
                                             select new StudentDashboardDTO
                                             {
                                                 IHW_AssignmentNo = a.IHW_AssignmentNo,
                                                 IHW_Date = a.IHW_Date,
                                                 IHW_Topic = a.IHW_Topic,
                                                 IHW_Assignment = a.IHW_Assignment,
                                                 IHW_Attachment = a.IHW_Attachment,
                                                 ASMS_Id = a.ASMS_Id,
                                                 IHW_FilePath = a.IHW_FilePath,
                                                 ISMS_Id = a.ISMS_Id,
                                                 ISMS_SubjectName = b.ISMS_SubjectName,
                                                 ASMCL_Id = a.ASMCL_Id
                                             }).Distinct().OrderByDescending(d => d.IHW_Date).ToArray();

                        //  string datestring = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion

                    #region  PROMOTION STATUS
                    try
                    {
                        data.promotionstatus = _studentDashboardContext.ExamPromotionRemarksDMO.Where(p => p.MI_Id == data.MI_Id && p.AMST_Id == data.AMST_Id && p.EPRD_Promotionflag == "P").Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion


                    #region  Push Notifications
                    try
                    {
                        data.pushnotification = (from a in _studentDashboardContext.IVRM_PushNotificationDMO
                                                 from b in _studentDashboardContext.IVRM_PushNotification_Student_DMO
                                                 from c in _studentDashboardContext.Adm_M_Student
                                                 where (a.IPN_Id == b.IPN_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IPN_ActiveFlag == true && c.AMST_SOL == "S" && b.AMST_Id == data.AMST_Id)
                                                 select new StudentDashboardDTO
                                                 {
                                                     IPN_Id = a.IPN_Id,
                                                     IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                                     IPN_PushNotification = a.IPN_PushNotification,
                                                     IPN_No = a.IPN_No,
                                                     IPN_Date = a.IPN_Date,
                                                     IPNS_Id = b.IPNS_Id,
                                                     AMST_Id = b.AMST_Id,
                                                     IVRMUL_Id = a.IVRMUL_Id,
                                                     AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName) + (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) + (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName) + ":" + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                                 }).Distinct().OrderByDescending(d => d.IPN_Date).ToArray();

                        string datestring = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        DateTime startDate = DateTime.Parse(datestring);
                        DateTime expiryDate = startDate.AddDays(-30);

                        data.pushnotiflist = (from a in _studentDashboardContext.IVRM_PushNotificationDMO
                                              from b in _studentDashboardContext.IVRM_PushNotification_Student_DMO
                                              from c in _studentDashboardContext.Adm_M_Student
                                              where (a.IPN_Id == b.IPN_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IPN_ActiveFlag == true && c.AMST_SOL == "S" && b.AMST_Id == data.AMST_Id && a.IPN_Date >= expiryDate)
                                              select new StudentDashboardDTO
                                              {
                                                  IPN_Id = a.IPN_Id,
                                                  IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                                  IPN_PushNotification = a.IPN_PushNotification,
                                                  IPN_No = a.IPN_No,
                                                  IPN_Date = a.IPN_Date,
                                                  IPNS_Id = b.IPNS_Id,
                                                  AMST_Id = b.AMST_Id,
                                                  IVRMUL_Id = a.IVRMUL_Id,
                                                  AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName) + (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) + (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName) + ":" + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                              }).Distinct().OrderByDescending(d => d.IPN_Date).ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion
                }

                #endregion


                #region MobileapppagePrivileges

                List<DataAccessMsSqlServerProvider.ApplicationUserRole> user = new List<DataAccessMsSqlServerProvider.ApplicationUserRole>();

                user = _studentDashboardContext.ApplicationUserRole.Where(g => g.UserId == data.User_Id).ToList();

                if (user.Count() > 0)
                {

                    List<IVRM_Role_MobileApp_Privileges> Staffmobileappprivileges = new List<IVRM_Role_MobileApp_Privileges>();
                    Staffmobileappprivileges = _studentDashboardContext.IVRM_Role_MobileApp_Privileges.Where(f => f.IVRMRT_Id == data.roleid && f.MI_ID == data.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {

                        data.Staffmobileappprivileges = (from Mobilepage in _studentDashboardContext.IVRM_MobileApp_Page
                                                         from MobileRolePrivileges in _studentDashboardContext.IVRM_Role_MobileApp_Privileges
                                                         where (Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == data.roleid && MobileRolePrivileges.MI_ID == data.MI_Id)
                                                         select new LoginDTO
                                                         {
                                                             Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                             Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                             Pageurl = Mobilepage.IVRMMAP_AppPageURL
                                                         }).ToArray();

                        data.mobileprivileges = "true";
                    }
                    else
                    {
                        data.mobileprivileges = "false";
                    }
                }



                #endregion

                #region Mobileversion
                using (var cmdv = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdv.CommandText = "Versioncheck";
                    cmdv.CommandType = CommandType.StoredProcedure;

                    cmdv.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmdv.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar, 50)
                    {
                        Value = "Student"
                    });
                    cmdv.Parameters.Add(new SqlParameter("@version",
               SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    });


                    if (cmdv.Connection.State != ConnectionState.Open)
                        cmdv.Connection.Open();

                    var data1 = cmdv.ExecuteNonQuery();
                    // x.FSS_FineAmount += Convert.ToDecimal(cmd.Parameters["@amt"].Value);
                    data.type = cmdv.Parameters["@version"].Value.ToString();
                }

                #endregion
                #region StudentMobileId
                    var stureg = _studentDashboardContext.Adm_M_Student.Single(s => s.AMST_Id == data.AMST_Id);
                    stureg.AMST_AppDownloadedDeviceId = data.mobiledeviceid;
                _studentDashboardContext.Update(stureg);
                _studentDashboardContext.SaveChanges();
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }


        public StudentDashboardDTO saveakpkfile(StudentDashboardDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                IVRM_MobileApp_Download_DMO objdata = new IVRM_MobileApp_Download_DMO();


                objdata.MI_Id = data.MI_Id;
                objdata.IVRMUL_Id = data.IVRMUL_Id;
                objdata.IVRMMAD_MobileModel = "Student";
                objdata.IVRMMAD_DownlaodDateTime = indianTime;
                objdata.CreatedDate = indianTime;
                objdata.UpdatedDate = indianTime;

                _studentDashboardContext.Add(objdata);
                int rowAffected = _studentDashboardContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
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
