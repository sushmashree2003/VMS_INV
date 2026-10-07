using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
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
    public class StudentYearlyAttendanceImpl : Interfaces.StudentYearlyAttendanceInterface
    {
        public StudentAttendanceReportContext _db;


        public StudentYearlyAttendanceImpl(StudentAttendanceReportContext db)
        {
            _db = db;
        }
        //public async Task<StudentAttendanceReportDTO> getmonthtotalclassheld(StudentAttendanceReportDTO data)
        //{
        //    StudentAttendanceReportDTO monthheld = new StudentAttendanceReportDTO();
        //    try { 

        //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "ATTENDANCE_MONTH_NAME_FROM_START_AND_END_DATE";
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();

        //            var retObject = new List<dynamic>();


        //            using (var dataReader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await dataReader.ReadAsync())
        //                {
        //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                    {
        //                        dataRow.Add(
        //                            dataReader.GetName(iFiled),
        //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                        );
        //                    }

        //                    retObject.Add((ExpandoObject)dataRow);
        //                }
        //            }
        //            monthheld.monthList = retObject.ToArray();
        //        }
        //    }

        //    catch (Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //        }

        //    return monthheld;
        //}
        public async Task<StudentAttendanceReportDTO> getInitailData(int mi_id)
        {
            StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(t => t.MI_Id == mi_id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToListAsync();
                ctdo.academicList = allyear.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = await _db.admissionClass.Where(t => t.MI_Id == mi_id && t.ASMCL_ActiveFlag == true).OrderBy(c => c.ASMCL_Order).ToListAsync();
                ctdo.classlist = allclass.ToArray();

                List<School_M_Section> allsection = new List<School_M_Section>();
                allsection = await _db.masterSection.Where(t => t.MI_Id == mi_id && t.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToListAsync();
                ctdo.SectionList = allsection.ToArray();
            }

            //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            //    {
            //        cmd.CommandText = "ATTENDANCE_MONTH_NAME_FROM_START_AND_END_DATE";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        if (cmd.Connection.State != ConnectionState.Open)
            //            cmd.Connection.Open();

            //        var retObject = new List<dynamic>();


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
            //        ctdo.monthList = retObject.ToArray();
            //    }
            //}

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public async Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data)
        {
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ADM_STUDENT_YEARLY_ATTENDANCE_NEW_HEADBINDING";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@asmay_id",
                 SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMCL_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@asms_id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMC_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id",
               SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.miid)
                });
                cmd.Parameters.Add(new SqlParameter("@flag",
                    SqlDbType.VarChar)
                {
                    Value = data.allorindiflag
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
                    data.monthList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "ADM_STUDENT_YEARLY_ATTENDANCE_NEW";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                 SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMAY_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@asmcl_id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMCL_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@asms_id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMC_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@mi_id",
             SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.miid)
                });
                cmd1.Parameters.Add(new SqlParameter("@flag",
                    SqlDbType.VarChar)
                {
                    Value = data.allorindiflag
                });

              
                if (cmd1.Connection.State != ConnectionState.Open)
                    cmd1.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    using (var dataReader = await cmd1.ExecuteReaderAsync())
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
                    data.studentAttendanceList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
        }
    }
}
