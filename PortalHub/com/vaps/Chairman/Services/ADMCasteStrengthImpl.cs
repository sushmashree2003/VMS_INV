

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
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class ADMCasteStrengthImpl : Interfaces.ADMCasteStrengthInterface
    {
        private static ConcurrentDictionary<string, ADMCasteStrengthDTO> _login =
         new ConcurrentDictionary<string, ADMCasteStrengthDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ADMCasteStrengthImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public ADMCasteStrengthDTO Getdetails(ADMCasteStrengthDTO data)
        {
         
            try
            {

                List<ADMCasteStrengthDTO> result3 = new List<ADMCasteStrengthDTO>();
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true ).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();
                

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }


        public ADMCasteStrengthDTO Getreport(ADMCasteStrengthDTO data)
        {
            try
            {
                List<ADMCasteStrengthDTO> result1 = new List<ADMCasteStrengthDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_Castewise_total_student";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmcL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmS_Id
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
                                result1.Add(new ADMCasteStrengthDTO
                                {
                                    castid = Convert.ToInt32(dataReader["castid"].ToString()),
                                    caste = (dataReader["caste"].ToString()),
                                    total = Convert.ToInt32(dataReader["total"].ToString()),
                                   


                                });
                                data.castedetails = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

        public  ADMCasteStrengthDTO Getstudentdetails(ADMCasteStrengthDTO data)
        {
            try
            {

                List<ADMCasteStrengthDTO> result1 = new List<ADMCasteStrengthDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_Castewise_student_details";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmcL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMC_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.castid
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
                                result1.Add(new ADMCasteStrengthDTO
                                {
                                    name =(dataReader["name"].ToString()),
                                    admno = (dataReader["AMST_AdmNo"].ToString()),
                                    regno =(dataReader["AMST_RegistrationNo"].ToString()),
                                    gender = (dataReader["AMST_Sex"].ToString()),
                                    mobile = (dataReader["AMST_MobileNo"].ToString()),
                                    

                                });
                                data.studentlist = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
          public ADMCasteStrengthDTO Getsection(ADMCasteStrengthDTO data)
        {
          
            try
            {

                data.fillsection = (from a in _db.School_Adm_Y_StudentDMO
                                    from b in _db.admissioncls
                                    from c in _db.Adm_M_Student
                                    from d in _db.Section
                                    where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id 
                                   // && b.ASMCL_Id == c.ASMCL_Id 
                                    && a.ASMCL_Id == data.asmcL_Id && d.ASMS_Id == a.ASMS_Id)
                                    select new ADMCasteStrengthDTO
                                    {
                                        sectionname = d.ASMC_SectionName,
                                        asmS_Id = d.ASMS_Id
                                    }).Distinct().OrderBy(t => t.asmS_Id).ToArray();



            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public ADMCasteStrengthDTO getclass(ADMCasteStrengthDTO data)//int IVRMM_Id
        {
            
            try
            {
                List<ADMCasteStrengthDTO> result3 = new List<ADMCasteStrengthDTO>();
               

                data.classarray = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)


                                   select new ADMCasteStrengthDTO
                                   {
                                       Class_Name = b.ASMCL_ClassName,
                                       asmcL_Id = b.ASMCL_Id
                                   }).Distinct().OrderBy(t => t.asmcL_Id).ToArray();


                


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
