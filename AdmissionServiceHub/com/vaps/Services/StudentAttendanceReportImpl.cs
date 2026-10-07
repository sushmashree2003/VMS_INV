using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentAttendanceReportImpl : Interfaces.StudentAtttendanceReportInterface
    {
        public StudentAttendanceReportContext _db;
        ILogger<StudentAttendanceReportImpl> _acdimpl;

        public StudentAttendanceReportImpl(StudentAttendanceReportContext db, ILogger<StudentAttendanceReportImpl> acdimpl)
        {
            _db = db;
            _acdimpl = acdimpl;
        }
        public StudentAttendanceReportDTO getInitailData(StudentAttendanceReportDTO ctdo)
        {
            //StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.academicYear.Where(t => t.MI_Id == ctdo.miid && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                ctdo.academicList = allyear.ToArray();

                List<MasterAcademic> defaultyear = new List<MasterAcademic>();
                defaultyear = _db.academicYear.Where(t => t.MI_Id == ctdo.miid && t.Is_Active == true && t.ASMAY_Id == ctdo.ASMAY_Id).OrderBy(a => a.ASMAY_Order).ToList();
                ctdo.academicListdefault = defaultyear.ToArray();



                List<MasterMonthDMO> allmonth = new List<MasterMonthDMO>();
                allmonth = _db.masterMonth.ToList();
                ctdo.monthList = allmonth.ToArray();

                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == ctdo.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                 ).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == ctdo.miid && a.IVRMSTAUL_UserName.Equals(ctdo.username.Trim()))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                if (check_rolename.FirstOrDefault().rolename.Equals("ADMIN") || check_rolename.FirstOrDefault().rolename.Equals("admin"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("HOD") || check_rolename.FirstOrDefault().rolename.Equals("Hod"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Super Admin"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }

                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }


                else if (check_rolename.FirstOrDefault().rolename.Equals("COORDINATOR") || check_rolename.FirstOrDefault().rolename.Equals("Coordinator"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }

                else
                {
                    if (empcode_check.Count > 0)
                    {
                        ctdo.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }
                                  ).Distinct().ToArray();


                        ctdo.SectionList = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new StudentAttendanceReportDTO
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                            }
                                            ).Distinct().ToArray();



                    }
                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public StudentAttendanceReportDTO getDataByTypeSelected(int id)
        {
            StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            ctdo.studentList = (from a in _db.admissionStduent
                                from b in _db.admissionyearstudent
                                where a.AMST_Id == b.AMST_Id && a.AMST_SOL == "S"
                                select new StudentAttendanceReportDTO
                                {
                                    AMST_Id = a.AMST_Id,
                                    AMST_FirstName = a.AMST_FirstName,
                                    AMST_MiddleName = a.AMST_MiddleName,
                                    AMST_LastName = a.AMST_LastName,
                                    AMST_AdmNo = a.AMST_AdmNo

                                }).ToArray();
            return ctdo;
        }
        public async Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data)
        {
            _acdimpl.LogInformation("entered fnction block");
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                _acdimpl.LogInformation("entered sp block");
                _acdimpl.LogInformation("fromdate:'" + data.fromdate + "'");
                _acdimpl.LogInformation("todate:'" + data.todate + "'");
                _acdimpl.LogInformation("type:'" + data.type + "'");
                _acdimpl.LogInformation("radiotype:'" + data.radiotype + "'");

                cmd.CommandText = "Attendance_monthwise";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMCL_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMC_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {
                    Value = Convert.ToDateTime(data.fromdate).ToString("dd/MM/yyyy")
                });
                cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                {
                    Value = Convert.ToDateTime(data.todate).ToString("dd/MM/yyyy")
                });
                cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int)
                {
                    Value = Convert.ToInt64(data.type)
                });
                cmd.Parameters.Add(new SqlParameter("@radiotype", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.radiotype)
                });
                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.AMST_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.AMM_ID)
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id",
             SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.miid)
                });
                cmd.Parameters.Add(new SqlParameter("@datewise",
           SqlDbType.VarChar)
                {
                    Value = data.datewise
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    _acdimpl.LogInformation("enter  try block retobject");
                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        _acdimpl.LogInformation("before  while condition");
                        while (await dataReader.ReadAsync())
                        {
                            _acdimpl.LogInformation("enter after  while condition");
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            _acdimpl.LogInformation("enter  datarow condition");
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                _acdimpl.LogInformation("enter  for condition");
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                );
                            }
                            _acdimpl.LogInformation("out of for condition");

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    _acdimpl.LogInformation("out of  while condition");
                    data.studentAttendanceList = retObject.ToArray();
                    _acdimpl.LogInformation("data of studentAttendanceList : '" + data.studentAttendanceList + "'");
                }
                catch (Exception ex)
                {
                    //  Console.WriteLine(ex.Message);
                    _acdimpl.LogInformation("error : '" + ex.Message + "'");
                }
            }
            if (data.datewise == 1)
            {
                _acdimpl.LogInformation("entered fnction block");
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    _acdimpl.LogInformation("entered sp block");
                    _acdimpl.LogInformation("fromdate:'" + data.fromdate + "'");
                    _acdimpl.LogInformation("todate:'" + data.todate + "'");
                    _acdimpl.LogInformation("type:'" + data.type + "'");
                    _acdimpl.LogInformation("radiotype:'" + data.radiotype + "'");

                    cmd.CommandText = "Adm_Student_Attendance_Month_Datewise_Namebinding";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@month",
                     SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        _acdimpl.LogInformation("enter  try block retobject");
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            _acdimpl.LogInformation("before  while condition");
                            while (await dataReader.ReadAsync())
                            {
                                _acdimpl.LogInformation("enter after  while condition");
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                _acdimpl.LogInformation("enter  datarow condition");
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    _acdimpl.LogInformation("enter  for condition");
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                _acdimpl.LogInformation("out of for condition");

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        _acdimpl.LogInformation("out of  while condition");
                        data.monthList = retObject.ToArray();
                        _acdimpl.LogInformation("data of studentAttendanceList : '" + data.studentAttendanceList + "'");
                    }
                    catch (Exception ex)
                    {
                        //  Console.WriteLine(ex.Message);
                        _acdimpl.LogInformation("error : '" + ex.Message + "'");
                    }
                }
                List<StudentAttendanceReportDTO> result = new List<StudentAttendanceReportDTO>();
                _acdimpl.LogInformation("entered fnction block1");
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    _acdimpl.LogInformation("entered sp block");
                    _acdimpl.LogInformation("fromdate:'" + data.fromdate + "'");
                    _acdimpl.LogInformation("todate:'" + data.todate + "'");
                    _acdimpl.LogInformation("type:'" + data.type + "'");
                    _acdimpl.LogInformation("radiotype:'" + data.radiotype + "'");

                    cmd.CommandText = "Attendance_Monthwise_TotalClassHeld";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMC_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
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
                                result.Add(new StudentAttendanceReportDTO
                                {
                                    countclass = Convert.ToDecimal(dataReader["classheld"]),
                                });
                            }
                        }

                        if (result.Count > 0)
                        {
                            data.countclass = result.FirstOrDefault().countclass;
                        }

                    }
                    catch (Exception ex)
                    {
                        //  Console.WriteLine(ex.Message);
                        _acdimpl.LogInformation("error : '" + ex.Message + "'");
                    }
                }

                _acdimpl.LogInformation("entered fnction block2");
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    _acdimpl.LogInformation("entered sp block");
                    _acdimpl.LogInformation("fromdate:'" + data.fromdate + "'");
                    _acdimpl.LogInformation("todate:'" + data.todate + "'");
                    _acdimpl.LogInformation("type:'" + data.type + "'");
                    _acdimpl.LogInformation("radiotype:'" + data.radiotype + "'");

                    cmd.CommandText = "Adm_Get_Attendance_percentage_Student_Attendancereport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMC_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MONTHId", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.totalpercentageatt = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        //  Console.WriteLine(ex.Message);
                        _acdimpl.LogInformation("error : '" + ex.Message + "'");
                    }
                }
            }

            data.institutiondetails = _db.Institution.Where(a => a.MI_Id == data.miid && a.MI_ActiveFlag == 1).ToArray();


            return data;
        }
        public async Task<StudentAttendanceReportDTO> getdatatype(StudentAttendanceReportDTO data)
        {
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_namebinding";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMCL_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@secid", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMC_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                {
                    Value = data.type1
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.miid)
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
                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentAttendanceList = retObject.ToArray();
                    // data.studentAttendanceList = retObject.Distinct().GroupBy(x => x.name).Select(x => x.First()).OrderBy(x=>x.name).ToArray();

                    // studDto.studentListById = stud.Distinct().GroupBy(x => x.RegistrationNo).Select(x => x.First()).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
        }
        public StudentAttendanceReportDTO getreportdiv(StudentAttendanceReportDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Attendance_Deviation_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@secid", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMC_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(data.fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(data.todate).ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@flag",
             SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.flag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@hrmeid",
          SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.hrmE_Id)
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentAttendanceList = retObject.ToArray();
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
        public StudentAttendanceReportDTO savetmpldatanew(StudentAttendanceReportDTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Admission_Attendance_Total_present_absent_classwise";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
               SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthid",
               SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMC_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.type
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.newarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Admission_Get_MonthDates_Attendance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
               SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthid",
               SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.newarray_date = retObject.ToArray();
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

            if (data.type == 1)
            {
                data.classsecdetails = (from a in _db.admissionClass
                                        from b in _db.masterSection
                                        from c in _db.AdmSchoolMasterClassCatSec
                                        from d in _db.Masterclasscategory
                                        from e in _db.academicYear
                                        where (a.ASMCL_Id == d.ASMCL_Id && c.ASMCC_Id == d.ASMCC_Id && d.ASMAY_Id == e.ASMAY_Id && c.ASMS_Id == b.ASMS_Id
                                        && a.MI_Id == data.miid && d.MI_Id == data.miid && d.ASMAY_Id == data.ASMAY_Id && d.Is_Active == true && d.Is_Active == true)
                                        select new StudentAttendanceReportDTO
                                        {
                                            ASMCL_className = a.ASMCL_ClassName,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMCL_Id = a.ASMCL_Id,
                                            ASMS_Id = b.ASMS_Id,
                                            classorder = a.ASMCL_Order,
                                            secorder = b.ASMC_Order
                                        }).OrderBy(f => f.classorder).ThenBy(g => g.secorder).ToArray();

                data.classdetails = _db.admissionClass.Where(a => a.MI_Id == data.miid && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();
            }
            else
            {
                data.classsecdetails = (from a in _db.admissionClass
                                        from b in _db.masterSection
                                        from c in _db.AdmSchoolMasterClassCatSec
                                        from d in _db.Masterclasscategory
                                        from e in _db.academicYear
                                        where (a.ASMCL_Id == d.ASMCL_Id && c.ASMCC_Id == d.ASMCC_Id && d.ASMAY_Id == e.ASMAY_Id && c.ASMS_Id == b.ASMS_Id && a.MI_Id == data.miid && d.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMC_Id
                                        && d.MI_Id == data.miid && d.ASMAY_Id == data.ASMAY_Id && d.Is_Active == true && d.Is_Active == true)
                                        select new StudentAttendanceReportDTO
                                        {
                                            ASMCL_className = a.ASMCL_ClassName,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMCL_Id = a.ASMCL_Id,
                                            ASMS_Id = b.ASMS_Id,
                                            classorder = a.ASMCL_Order,
                                            secorder = b.ASMC_Order
                                        }).OrderBy(f => f.classorder).ThenBy(g => g.secorder).ToArray();




                data.classdetails = _db.admissionClass.Where(a => a.MI_Id == data.miid && a.ASMCL_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_Id).OrderBy(a => a.ASMCL_Order).ToArray();


            }

            return data;
        }
        public StudentAttendanceReportDTO onchangeyear(StudentAttendanceReportDTO data)
        {
            try
            {

                data.classlist = (from a in _db.admissionClass
                                  from b in _db.Masterclasscategory
                                  from c in _db.academicYear
                                  where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true && c.Is_Active == true
                                  && b.ASMAY_Id == data.ASMAY_Id)
                                  select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();


                data.stafflist = (from a in _db.Adm_SchAttLoginUser
                                  from b in _db.Adm_SchAttLoginUserClass
                                  from c in _db.admissionClass
                                  from d in _db.masterSection
                                  from e in _db.masteremployee
                                  from f in _db.academicYear
                                  where (a.ASALU_Id == b.ASALU_Id && a.HRME_Id == e.HRME_Id && a.ASMAY_Id == f.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                  && a.ASMAY_Id == data.ASMAY_Id && c.ASMCL_ActiveFlag == true
                                  && d.ASMC_ActiveFlag == 1 && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && a.MI_Id == data.miid)
                                  select new StudentAttendanceReportDTO
                                  {
                                      employeename = ((e.HRME_EmployeeFirstName == null ? "" : e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null ? "" : " " + e.HRME_EmployeeLastName) + (e.HRME_EmployeeCode == null ? "" : " : " + e.HRME_EmployeeCode)).Trim(),
                                      hrmE_Id = e.HRME_Id
                                  }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO onclasschange(StudentAttendanceReportDTO data)
        {
            try
            {
                data.SectionList = (from a in _db.admissionClass
                                    from b in _db.Masterclasscategory
                                    from c in _db.academicYear
                                    from d in _db.AdmSchoolMasterClassCatSec
                                    from e in _db.masterSection
                                    where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && d.ASMS_Id == e.ASMS_Id && b.ASMCC_Id == d.ASMCC_Id
                                    && a.ASMCL_ActiveFlag == true && b.Is_Active == true && c.Is_Active == true && e.ASMC_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id
                                    && b.ASMCL_Id == data.ASMCL_Id)
                                    select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO onsectionchange(StudentAttendanceReportDTO data)
        {
            try
            {
                if (data.ASMS_Id == 0)
                {
                    data.stafflist = (from a in _db.Adm_SchAttLoginUser
                                      from b in _db.Adm_SchAttLoginUserClass
                                      from c in _db.admissionClass
                                      from d in _db.masterSection
                                      from e in _db.masteremployee
                                      from f in _db.academicYear
                                      where (a.ASALU_Id == b.ASALU_Id && a.HRME_Id == e.HRME_Id && a.ASMAY_Id == f.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                      && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && c.ASMCL_ActiveFlag == true
                                      && d.ASMC_ActiveFlag == 1 && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && a.MI_Id == data.miid)
                                      select new StudentAttendanceReportDTO
                                      {
                                          employeename = ((e.HRME_EmployeeFirstName == null ? "" : e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null ? "" : " " + e.HRME_EmployeeLastName) + (e.HRME_EmployeeCode == null ? "" : " : " + e.HRME_EmployeeCode)).Trim(),
                                          hrmE_Id = e.HRME_Id
                                      }).Distinct().ToArray();
                }
                else
                {
                    data.stafflist = (from a in _db.Adm_SchAttLoginUser
                                      from b in _db.Adm_SchAttLoginUserClass
                                      from c in _db.admissionClass
                                      from d in _db.masterSection
                                      from e in _db.masteremployee
                                      from f in _db.academicYear
                                      where (a.ASALU_Id == b.ASALU_Id && a.HRME_Id == e.HRME_Id && a.ASMAY_Id == f.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                      && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && c.ASMCL_ActiveFlag == true
                                      && d.ASMC_ActiveFlag == 1 && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && a.MI_Id == data.miid)
                                      select new StudentAttendanceReportDTO
                                      {
                                          employeename = ((e.HRME_EmployeeFirstName == null ? "" : e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null ? "" : " " + e.HRME_EmployeeLastName) + (e.HRME_EmployeeCode == null ? "" : " : " + e.HRME_EmployeeCode)).Trim(),
                                          hrmE_Id = e.HRME_Id
                                      }).Distinct().ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Active Deactive Report
        public StudentAttendanceReportDTO getreport(StudentAttendanceReportDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Active_Deactive_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
               SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });              

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMC_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.type
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.newarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
    }
}
