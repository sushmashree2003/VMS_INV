
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Principal.Services
{
    public class PrincipalDashboardImpl : Interfaces.PrincipalDashboardInterface
    {
        private static ConcurrentDictionary<string, PrincipalDashboardDTO> _login =
         new ConcurrentDictionary<string, PrincipalDashboardDTO>();

        private readonly PortalContext _PrincipalDashboardContext;
        ILogger<PrincipalDashboardImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public PrincipalDashboardImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
        }

        public PrincipalDashboardDTO Getdetails(PrincipalDashboardDTO data)//int IVRMM_Id
        {
            try
            {
                data.Smscount = _db.IVRM_sms_sentBoxDMO.Where(t => t.MI_Id == data.MI_Id && t.Module_Name == "PRINCIPAL DASHBOARD").ToArray();
                data.Emailcount = _db.IVRM_Email_sentBoxDMO.Where(t => t.MI_Id == data.MI_Id && t.Module_Name == "PRINCIPAL DASHBOARD").ToArray();

                               
                data.notification = (from a in _PrincipalDashboardContext.Adm_Students_Certificate_Apply_DMO
                                     from b in _PrincipalDashboardContext.Adm_M_Student
                                     from c in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                     where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ASCA_Status == "Pending" && a.ASCA_ActiveFlg == true)
                                     select new TransferCertificate_DTO
                                     {
                                         ASCA_Id = a.ASCA_Id,
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName.ToUpper()) + " " + (b.AMST_MiddleName == null ? "" : b.AMST_MiddleName.ToUpper()) + " " + (b.AMST_LastName == null ? "" : b.AMST_LastName.ToUpper())).Trim(),
                                         ASCA_CertificateType = a.ASCA_CertificateType,
                                         ASCA_Reason = a.ASCA_Reason,
                                         ASCA_ApplyDate = a.ASCA_ApplyDate,
                                         ASCA_Status = a.ASCA_Status,
                                         ASCA_ActiveFlg = a.ASCA_ActiveFlg
                                     }).Distinct().OrderByDescending(m => m.ASCA_ApplyDate).ToArray();
                
                data.leavenotification = (from a in _PrincipalDashboardContext.Adm_Students_Leave_Apply_DMO
                                          from b in _PrincipalDashboardContext.Adm_M_Student
                                          from c in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                          where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ASLA_Status == "Pending" && a.ASLA_ActiveFlag == true)
                                          select new OnlineLeaveApp_DTO
                                          {
                                              ASLA_Id = a.ASLA_Id,
                                              AMST_Id = a.AMST_Id,
                                              AMST_FirstName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName.ToUpper()) + " " + (b.AMST_MiddleName == null ? "" : b.AMST_MiddleName.ToUpper()) + " " + (b.AMST_LastName == null ? "" : b.AMST_LastName.ToUpper())).Trim(),
                                              ASLA_Flag = a.ASLA_Flag,
                                              ASLA_Reason = a.ASLA_Reason,
                                              ASLA_ApplyDate = a.ASLA_ApplyDate,
                                              ASLA_FromDate = a.ASLA_FromDate,
                                              ASLA_ToDate = a.ASLA_ToDate,
                                              ASLA_ApprovedFromDate = a.ASLA_ApprovedFromDate,
                                              ASLA_ApprovedToDate = a.ASLA_ApprovedToDate,
                                              ASLA_Status = a.ASLA_Status,
                                              ASAPCS_ActiveFlg = a.ASLA_ActiveFlag
                                          }).Distinct().OrderByDescending(m => m.ASLA_ApplyDate).ToArray();

                List<PrincipalDashboardDTO> result = new List<PrincipalDashboardDTO>();
                List<PrincipalDashboardDTO> result1 = new List<PrincipalDashboardDTO>();
                List<MasterAcademic> list = new List<MasterAcademic>();

                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.yearlist = list.ToArray();

                data.CurrentAcademicYear = _db.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToArray();

                if (data.ASMAY_Id > 0)
                {
                    data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                                               from b in _db.admissioncls
                                               from c in _db.Adm_M_Student
                                               where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                               select new
                                               {
                                                   Class_Name = b.ASMCL_ClassName,
                                                   ASMCL_Order = b.ASMCL_Order,
                                                   stud_count = a.AMST_Id,
                                               }).Distinct().GroupBy(id => new { id.Class_Name, id.ASMCL_Order }).Select(g => new PrincipalDashboardDTO { Class_Name = g.Key.Class_Name, ASMCL_Order = g.Key.ASMCL_Order, stud_count = g.Count() }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_PRINCIPAL_STAFF_BIRTHDAYLIST";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
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
                            data.staffbrthlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_PRINCIPAL_STUDENT_TODAY_ABSENT";
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
                            data.stdabsentlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_No_of_Absent";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
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
                                    result.Add(new PrincipalDashboardDTO
                                    {
                                        NameOfDesig = dataReader["NameOfDesig"].ToString(),
                                        absentee = int.Parse(dataReader["absentee"].ToString())

                                    });
                                    data.fillabsent = result.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_Classwise_fee_collaction";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.CommandTimeout = 800000000;
                        cmd.CommandTimeout = 400000;
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

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();


                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result1.Add(new PrincipalDashboardDTO
                                    {
                                        paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                        ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                        recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                        feeclass = dataReader["class"].ToString()
                                    });
                                    data.fillfee = result1.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    DateTime now = DateTime.Now;
                    int A = now.Month;
                    data.coedata = (from m in _PrincipalDashboardContext.COE_Master_EventsDMO
                                    from n in _PrincipalDashboardContext.COE_EventsDMO
                                    where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == A
                                    select new PrincipalDashboardDTO
                                    {
                                        eventName = m.COEME_EventName,
                                        eventDesc = m.COEME_EventDesc,
                                        COEE_EStartDate = n.COEE_EStartDate,
                                        COEE_EEndDate = n.COEE_EEndDate,
                                    }).ToArray();
                }

                //STAFF BIRTH DAY 

                //data.staffbrthlist = (from a in _db.HR_Master_Employee_DMO
                //                  where (a.MI_Id == data.MI_Id  && a.HRME_DOB.Value.Date.Day == DateTime.Now.Day && a.HRME_DOB.Value.Date.Month == DateTime.Now.Month && a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true)
                //                  //group a by a.HRME_Id into g
                //                  select new PrincipalDashboardDTO
                //                  {
                //                      HRME_Id = a.HRME_Id,
                //                      employeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    
                //                  }
                //                ).ToArray();
               

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }

    }
}
