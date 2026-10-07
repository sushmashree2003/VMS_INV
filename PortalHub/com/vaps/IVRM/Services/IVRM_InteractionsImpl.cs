using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Portals.Student;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using DomainModel.Model.com.vapstech.Portals.IVRM;

namespace PortalHub.com.vaps.Student.Services
{
    public class IVRM_InteractionsImpl : Interfaces.IVRM_InteractionsInterface
    {
        private static ConcurrentDictionary<string, IVRM_School_InteractionsDTO> _login =
           new ConcurrentDictionary<string, IVRM_School_InteractionsDTO>();
        private PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _context;

        public IVRM_InteractionsImpl(PortalContext PortalContext, DomainModelMsSqlServerContext context)
        {
            _PortalContext = PortalContext;
            _context = context;
        }
        public async Task<IVRM_School_InteractionsDTO> getloaddata(IVRM_School_InteractionsDTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                data.configflag = _PortalContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();

                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                }
                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Fee End User", StringComparison.OrdinalIgnoreCase))
                //{
                //    data.AMST_Id = 0;
                //    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //}
                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                //{
                //    data.AMST_Id = 0;
                //    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //}
                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("HOD", StringComparison.OrdinalIgnoreCase))
                //{
                //    data.AMST_Id = 0;
                //    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //}
                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase))
                //{
                //    data.AMST_Id = 0;
                //    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //}
                else
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_Inbox";
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
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@roleflg",
                    SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
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
                        data.getinboxmsg = retObject.ToArray();
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


        public async Task<IVRM_School_InteractionsDTO> getdetails(IVRM_School_InteractionsDTO data)
        {
            try
            {
                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    var clsid = (from a in _PortalContext.Adm_M_Student
                                 from b in _PortalContext.School_M_Class
                                 from c in _PortalContext.School_M_Section
                                 from d in _PortalContext.School_Adm_Y_StudentDMO
                                 from e in _PortalContext.AcademicYearDMO
                                 where (a.AMST_Id == d.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ASMAY_Id == e.ASMAY_Id && a.MI_Id == b.MI_Id && c.MI_Id == e.MI_Id && a.AMST_Id == data.AMST_Id && d.ASMAY_Id == data.ASMAY_Id)
                                 select new IVRM_School_InteractionsDTO
                                 {
                                     ASMCL_Id = b.ASMCL_Id,
                                     ASMS_Id = c.ASMS_Id,
                                     ASMC_SectionName = c.ASMC_SectionName,
                                     ASMCL_ClassName = b.ASMCL_ClassName
                                 }
                         ).Distinct().ToArray();
                    data.asmclid = clsid.FirstOrDefault().ASMCL_Id;
                    data.asmsid = clsid.FirstOrDefault().ASMS_Id;
                }
                else if (data.roleflg.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_filter";
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
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@IINTS_Flag",
                    SqlDbType.VarChar)
                    {
                        Value = data.userflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@roletype",
                   SqlDbType.VarChar)
                    {
                        Value = data.roleflg
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
                        data.getdetails = retObject.ToArray();
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
        public IVRM_School_InteractionsDTO getstudent(IVRM_School_InteractionsDTO data)
        {
            try
            {
                data.get_student = (from a in _PortalContext.Adm_M_Student
                                    from b in _PortalContext.School_Adm_Y_StudentDMO
                                    from d in _PortalContext.School_M_Class
                                    from e in _PortalContext.School_M_Section
                                    where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.AMAY_ActiveFlag == 1 && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                    select new IVRM_School_InteractionsDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        ASMCL_Id = b.ASMCL_Id,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                    }).Distinct().OrderBy(t => t.studentName).ToArray();
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_School_InteractionsDTO savedetails(IVRM_School_InteractionsDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "InteractionStudent").ToList();
                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                int level_no = 1;
                //var level = _PortalContext.IVRM_School_Master_InteractionsDMO.ToList();
                //var max_level = level.Count();
                //if (max_level <= 0)
                //{
                //    level_no = 1;
                //}
                //else
                //{
                //    level_no = max_level + 1;
                //}

                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    if (data.ISMINT_GroupOrIndFlg == "Group")
                    {
                        IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                        inter.MI_Id = data.MI_Id;
                        inter.ISMINT_InteractionId = data.trans_id;
                        inter.ASMAY_Id = data.ASMAY_Id;
                        inter.ISMINT_ComposedByFlg = data.roleflg;
                        inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                        inter.ISMINT_ComposedById = data.AMST_Id;
                        inter.ISMINT_Subject = data.ISMINT_Subject;
                        inter.ISMINT_DateTime = indianTime;
                        inter.ISMINT_Interaction = data.ISMINT_Interaction;
                        inter.ISMINT_ActiveFlag = true;
                        inter.ISMINT_CreatedBy = data.AMST_Id;
                        inter.ISMINT_UpdatedBy = data.AMST_Id;
                        inter.CreatedDate = indianTime;
                        inter.UpdatedDate = indianTime;
                        _PortalContext.Add(inter);
                        if (data.userflag == "SubjectTeacher")
                        {
                            List<long> device_ids = new List<long>();
                            foreach (var i in data.arrayST)
                            {
                                device_ids.Add(i.HRME_Id);

                                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                                intrans.ISMINT_Id = inter.ISMINT_Id;
                                intrans.ISTINT_ToId = i.HRME_Id;
                                intrans.ISTINT_ToFlg = "Staff";
                                intrans.ISTINT_ComposedById = data.AMST_Id;
                                intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                                intrans.ISTINT_DateTime = indianTime;
                                intrans.ISTINT_ComposedByFlg = data.roleflg;
                                intrans.ISTINT_InteractionOrder = level_no;
                                intrans.ISTINT_ActiveFlag = true;
                                intrans.ISTINT_CreatedBy = data.UserId;
                                intrans.ISTINT_UpdatedBy = data.UserId;
                                intrans.CreatedDate = DateTime.Now;
                                intrans.UpdatedDate = DateTime.Now;
                                _PortalContext.Add(intrans);
                            }
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id=a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToArray();
                        }
                        else
                        {
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id=a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToArray();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.ISTINT_ToId;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.AMST_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_no;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(intrans);
                        }
                    }
                    else if (data.ISMINT_GroupOrIndFlg == "Individual")
                    {
                        var level_order = 1;
                        if (data.userflag == "SubjectTeacher")
                        {

                            data.trans_id = "";
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.AMST_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            _PortalContext.Add(inter);

                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id=a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToArray();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.HRME_Id;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.AMST_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_order;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(intrans);

                        }
                        else
                        {
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.AMST_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            _PortalContext.Add(inter);

                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id=a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToArray();

                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.ISTINT_ToId;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.AMST_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_no;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(intrans);
                        }
                    }

                    var contactExists = _PortalContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    if (data.ISMINT_GroupOrIndFlg == "Group")
                    {
                        IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                        inter.MI_Id = data.MI_Id;
                        inter.ISMINT_InteractionId = data.trans_id;
                        inter.ASMAY_Id = data.ASMAY_Id;
                        inter.ISMINT_ComposedByFlg = data.roleflg;
                        inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                        inter.ISMINT_ComposedById = data.HRME_Id;
                        inter.ISMINT_Subject = data.ISMINT_Subject;
                        inter.ISMINT_DateTime = indianTime;
                        inter.ISMINT_Interaction = data.ISMINT_Interaction;
                        inter.ISMINT_ActiveFlag = true;
                        inter.ISMINT_CreatedBy = data.HRME_Id;
                        inter.ISMINT_UpdatedBy = data.HRME_Id;
                        inter.CreatedDate = indianTime;
                        inter.UpdatedDate = indianTime;
                        _PortalContext.Add(inter);
                        if (data.userflag == "Student")
                        {
                            List<long> device_ids = new List<long>();
                            foreach (var s in data.arrayStudent)
                            {
                                device_ids.Add(s.AMST_Id);
                                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                                intrans.ISMINT_Id = inter.ISMINT_Id;
                                intrans.ISTINT_ToId = s.AMST_Id;
                                intrans.ISTINT_ToFlg = "Student";
                                intrans.ISTINT_ComposedById = data.HRME_Id;
                                intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                                intrans.ISTINT_DateTime = indianTime;
                                intrans.ISTINT_ComposedByFlg = data.roleflg;
                                intrans.ISTINT_InteractionOrder = level_no;
                                intrans.ISTINT_ActiveFlag = true;
                                intrans.ISTINT_CreatedBy = data.UserId;
                                intrans.ISTINT_UpdatedBy = data.UserId;
                                intrans.CreatedDate = DateTime.Now;
                                intrans.UpdatedDate = DateTime.Now;
                                _PortalContext.Add(intrans);
                            }
                            data.deviceids = (from a in _PortalContext.Adm_M_Student
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                              }).Distinct().ToArray();

                        }
                        else if (data.userflag == "Teachers")
                        {
                            List<long> device_ids = new List<long>();
                            foreach (var t in data.arrayTeachers)
                            {
                                device_ids.Add(t.HRME_Id);
                                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                                intrans.ISMINT_Id = inter.ISMINT_Id;
                                intrans.ISTINT_ToId = t.HRME_Id;
                                intrans.ISTINT_ToFlg = "Staff";
                                intrans.ISTINT_ComposedById = data.HRME_Id;
                                intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                                intrans.ISTINT_DateTime = indianTime;
                                intrans.ISTINT_ComposedByFlg = data.roleflg;
                                intrans.ISTINT_InteractionOrder = level_no;
                                intrans.ISTINT_ActiveFlag = true;
                                intrans.ISTINT_CreatedBy = data.UserId;
                                intrans.ISTINT_UpdatedBy = data.UserId;
                                intrans.CreatedDate = DateTime.Now;
                                intrans.UpdatedDate = DateTime.Now;
                                _PortalContext.Add(intrans);
                            }
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToArray();
                        }
                        else
                        {
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToArray();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.ISTINT_ToId;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_no;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(intrans);
                        }
                    }
                    else if (data.ISMINT_GroupOrIndFlg == "Individual")
                    {
                        var level_order = 1;
                        if (data.userflag == "Student")
                        {
                            data.trans_id = "";
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }
                        
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            _PortalContext.Add(inter);

                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            data.deviceids = (from a in _PortalContext.Adm_M_Student
                                              where (a.MI_Id == data.MI_Id && a.AMST_Id == data.student_Id)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                              }).Distinct().ToArray();

                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.student_Id;
                            intrans.ISTINT_ToFlg = "Student";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_order;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(intrans);

                        }
                        else if (data.userflag == "Teachers")
                        {
                            data.trans_id = "";
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }
                            //if (orderno == 0)
                            //{
                            //    orderno = orderno + 1;
                            //    level_order = level_no;
                            //}
                            //else
                            //{
                            //    level_order = level_order + 1;
                            //}
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            _PortalContext.Add(inter);

                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToArray();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.employee_Id;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_order;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(intrans);

                        }
                        else
                        {
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            _PortalContext.Add(inter);

                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToArray();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.ISTINT_ToId;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_no;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(intrans);
                        }
                    }

                    var contactExists = _PortalContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<IVRM_School_InteractionsDTO> reply(IVRM_School_InteractionsDTO data)
        {
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_View_Reply";
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
                    cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.ISMINT_Id
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
                        data.viewmessage = retObject.ToArray();
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

        public IVRM_School_InteractionsDTO savereply(IVRM_School_InteractionsDTO data)
        {
            try
            {
                long toId = 0;
                var toflg = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                               from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                               where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_Id == data.ISMINT_Id && b.MI_Id == data.MI_Id)
                               select new IVRM_School_InteractionsDTO
                               {
                                   ISTINT_ComposedById = a.ISTINT_ComposedById,
                                   ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg
                               }).OrderBy(o => o.ISTINT_Id).ToList();

                toId = comp_id.FirstOrDefault().ISTINT_ComposedById;
                toflg = comp_id.FirstOrDefault().ISTINT_ComposedByFlg;

                int level_no = 0;
                var level_order = 0;
                var orderno = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                               from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                               where (a.ISMINT_Id == b.ISMINT_Id && b.MI_Id == data.MI_Id && a.ISMINT_Id == data.ISMINT_Id)
                               select new IVRM_School_InteractionsDTO
                               {
                                   ISTINT_InteractionOrder = a.ISTINT_InteractionOrder
                               }
                ).Distinct().ToList();

                level_no = orderno.LastOrDefault().ISTINT_InteractionOrder;

                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                if (data.ISTINT_ComposedByFlg == "Student")
                {
                    //if (toflg == "Student")
                    //{
                    //    comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                    //               from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                    //               where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_Id == data.ISMINT_Id && b.MI_Id == data.MI_Id)
                    //               select new IVRM_School_InteractionsDTO
                    //               {
                    //                   ISTINT_ToId = a.ISTINT_ToId,
                    //                   ISTINT_ToFlg = a.ISTINT_ToFlg
                    //               }).Distinct().ToList();
                    //    toId = comp_id.FirstOrDefault().ISTINT_ToId;
                    //    toflg = comp_id.FirstOrDefault().ISTINT_ToFlg;
                    //}
                    //else
                    //{
                    //    comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                    //               from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                    //               where (a.ISMINT_Id == data.ISMINT_Id && b.MI_Id == data.MI_Id)
                    //               select new IVRM_School_InteractionsDTO
                    //               {
                    //                   ISTINT_ComposedById = a.ISTINT_ComposedById,
                    //                   ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg
                    //               }).Distinct().ToList();
                    //    toId = comp_id.FirstOrDefault().ISTINT_ComposedById;
                    //    toflg = comp_id.FirstOrDefault().ISTINT_ComposedByFlg;
                    //}

                    if (level_no <= 0)
                    {
                        level_order = 1;
                    }
                    else
                    {
                        level_order = level_no + 1;
                    }
                    data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && a.HRME_Id== toId)
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                      }).Distinct().ToArray();
                    intrans.ISMINT_Id = data.ISMINT_Id;
                    intrans.ISTINT_ToId = toId;
                    intrans.ISTINT_ToFlg = toflg;
                    intrans.ISTINT_ComposedById = data.AMST_Id;
                    intrans.ISTINT_Interaction = data.ISTINT_Interaction;
                    intrans.ISTINT_DateTime = indianTime;
                    intrans.ISTINT_ComposedByFlg = data.ISTINT_ComposedByFlg;
                    intrans.ISTINT_InteractionOrder = level_order;
                    intrans.ISTINT_ActiveFlag = true;
                    intrans.ISTINT_CreatedBy = data.UserId;
                    intrans.ISTINT_UpdatedBy = data.UserId;
                    intrans.CreatedDate = indianTime;
                    intrans.UpdatedDate = indianTime;
                    _PortalContext.Add(intrans);
                }
                else
                {
                    //   if (toflg == "Staff")
                    //   {
                    //       comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                    //                  where (a.ISMINT_Id == data.ISMINT_Id)
                    //                  select new IVRM_School_InteractionsDTO
                    //                  {
                    //                      ISTINT_ToId = a.ISTINT_ToId,
                    //                      ISTINT_ToFlg = a.ISTINT_ToFlg
                    //                  }
                    //                    ).Distinct().ToList();
                    //       toId = comp_id.FirstOrDefault().ISTINT_ToId;
                    //       toflg = comp_id.FirstOrDefault().ISTINT_ToFlg;
                    //   }
                    //   else
                    //   {
                    //       comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                    //                  where (a.ISMINT_Id == data.ISMINT_Id)
                    //                  select new IVRM_School_InteractionsDTO
                    //                  {
                    //                      ISTINT_ComposedById = a.ISTINT_ComposedById,
                    //                      ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg
                    //                  }
                    //).Distinct().ToList();
                    //       toId = comp_id.FirstOrDefault().ISTINT_ComposedById;
                    //       toflg = comp_id.FirstOrDefault().ISTINT_ComposedByFlg;
                    //   }
                    if (level_no <= 0)
                    {
                        level_order = 1;
                    }
                    else
                    {
                        level_order = level_no + 1;
                    }

                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    intrans.ISMINT_Id = data.ISMINT_Id;
                    intrans.ISTINT_ToId = toId;
                    intrans.ISTINT_ToFlg = toflg;
                    intrans.ISTINT_ComposedById = data.HRME_Id;
                    intrans.ISTINT_Interaction = data.ISTINT_Interaction;
                    intrans.ISTINT_DateTime = indianTime;
                    intrans.ISTINT_ComposedByFlg = data.ISTINT_ComposedByFlg;
                    intrans.ISTINT_InteractionOrder = level_order;
                    intrans.ISTINT_ActiveFlag = true;
                    intrans.ISTINT_CreatedBy = data.UserId;
                    intrans.ISTINT_UpdatedBy = data.UserId;
                    intrans.CreatedDate = indianTime;
                    intrans.UpdatedDate = indianTime;
                    _PortalContext.Add(intrans);
                }
                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
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
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}
