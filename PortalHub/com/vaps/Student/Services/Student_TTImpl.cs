using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using AutoMapper;
using DomainModel.Model.com.vapstech.TT;

namespace PortalHub.com.vaps.Student.Services
{
    public class Student_TTImpl : Interfaces.Student_TTInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _Poralcontext;
        public Student_TTImpl(PortalContext Portalcontext)
        {
            _Poralcontext = Portalcontext;
        }

        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
        {
            try
            {
                //  data.getyear = _Poralcontext.AcademicYearDMO.Where(y => y.ASMAY_Id == data.ASMAY_Id && y.MI_Id == data.MI_Id).Distinct().ToArray();

                data.getyear = (from d in _Poralcontext.AcademicYearDMO
                                from a in _Poralcontext.School_M_Class
                                from b in _Poralcontext.School_M_Section
                                from c in _Poralcontext.School_Adm_Y_StudentDMO
                                where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                select new ExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id,
                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                    ASMS_Id = c.ASMS_Id,
                                    ASMC_SectionName = b.ASMC_SectionName,
                                    ASMAY_Id = c.ASMAY_Id,
                                    ASMAY_Year = d.ASMAY_Year
                                }
                           ).OrderBy(y => y.ASMAY_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public async Task<StudentDashboardDTO> getStudentTT(StudentDashboardDTO data)
        {
            try
            {
                var clssec = (from a in _Poralcontext.Adm_M_Student
                              from b in _Poralcontext.School_Adm_Y_StudentDMO
                              from c in _Poralcontext.School_M_Class
                              from s in _Poralcontext.School_M_Section
                              where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                              select new StudentDashboardDTO
                              {
                                  ASMCL_Id = c.ASMCL_Id,
                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                  ASMS_Id = s.ASMS_Id,
                                  ASMC_SectionName = s.ASMC_SectionName

                              }).Distinct().ToArray();

                using (var cmd = _Poralcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_StudentTT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Asmay_id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                    SqlDbType.BigInt)
                    {
                        Value = clssec.FirstOrDefault().ASMCL_Id,
                    });

                    cmd.Parameters.Add(new SqlParameter("@asms_id",
                    SqlDbType.VarChar)
                    {
                        Value = clssec.FirstOrDefault().ASMS_Id,
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
                        data.getStudentTT = retObject.ToArray();
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
