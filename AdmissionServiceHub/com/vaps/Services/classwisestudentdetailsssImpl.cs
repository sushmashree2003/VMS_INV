using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Identity;
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
    public class classwisestudentdetailsssImpl : Interfaces.ClasswisestudentdetailsInterface
    {
        public StudentYearLossReportContext _Classwisestudentdetailscontext;
        private readonly DomainModelMsSqlServerContext _db;

        public classwisestudentdetailsssImpl(StudentYearLossReportContext frgContext)
        {
            _Classwisestudentdetailscontext = frgContext;
        }
        private readonly UserManager<ApplicationUser> _UserManager;

        public classwisestudentdetailsssImpl(StudentYearLossReportContext Classwisestudentdetailscontext, UserManager<ApplicationUser> UserManager)
        {
            _Classwisestudentdetailscontext = Classwisestudentdetailscontext;
            _UserManager = UserManager;
        }

        public ClasswisestudentdetailsDTO getdetails(ClasswisestudentdetailsDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _Classwisestudentdetailscontext.AcademicYear.Where(y => y.MI_Id == data.mid && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _Classwisestudentdetailscontext.admissioncls.Where(c => c.MI_Id == data.mid && c.ASMCL_ActiveFlag == true).OrderBy(c => c.ASMCL_Order).ToList();
                data.fillclass = classname.ToArray();


                List<School_M_Section> secname = new List<School_M_Section>();
                secname = _Classwisestudentdetailscontext.school_M_Section.Where(s => s.MI_Id == data.mid && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                data.fillsection = secname.ToArray();




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<ClasswisestudentdetailsDTO> Getreportdetails(ClasswisestudentdetailsDTO data)
        {
            string IVRM_CLM_coloumn = "";

            string name = "";


            try
            {
                for (int i = 0; i < data.TempararyArrayheadList.Length; i++)
                {
                    name = data.TempararyArrayheadList[i].columnID;
                    if (name != null)
                    {
                        if (name == "AMST_FirstName")
                        {
                            if (data.TempararyArrayheadList.Length == 1)
                            {
                                IVRM_CLM_coloumn = "AMST_FirstName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' '  + AMST_LastName END,";
                            }

                            else
                            {
                                IVRM_CLM_coloumn = "AMST_FirstName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' '  + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' '  + AMST_LastName END,";

                            }
                        }



                        else if (name == "AMST_PerAdd3")
                        {

                            if (IVRM_CLM_coloumn != "")
                            {
                                IVRM_CLM_coloumn += "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END,";
                            }
                            else
                            {
                                IVRM_CLM_coloumn += "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END,";
                            }
                        }

                        else if (name == "AMST_ConCity")
                        {
                            if (name == "AMST_ConCity")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "AMST_ConCity = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "AMST_ConCity = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END,";
                                }
                            }
                        }

                        else if (name == "AMST_MotherName")
                        {
                            if (name == "AMST_MotherName")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "AMST_MotherName = CASE WHEN  AMST_MotherName is null or AMST_MotherName=''  then '' else AMST_MotherName end+CASE WHEN  AMST_MotherSurname is null or AMST_MotherSurname = '' or AMST_MotherSurname = '0' then ''  ELSE ' ' + AMST_MotherSurname END,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "AMST_MotherName = CASE WHEN  AMST_MotherName is null or AMST_MotherName=''  then '' else AMST_MotherName end+CASE WHEN  AMST_MotherSurname is null or AMST_MotherSurname = '' or AMST_MotherSurname = '0' then ''  ELSE ' ' + AMST_MotherSurname END,";
                                }
                            }
                        }

                        else if (name == "AMST_FatherName")
                        {
                            if (name == "AMST_FatherName")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "AMST_FatherName = CASE WHEN  AMST_FatherName is null or AMST_FatherName=''  then '' else AMST_FatherName end+CASE WHEN  AMST_FatherSurname is null or AMST_FatherSurname = '' or AMST_FatherSurname = '0' then ''  ELSE ' ' + AMST_FatherSurname END,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "AMST_FatherName = CASE WHEN  AMST_FatherName is null or AMST_FatherName=''  then '' else AMST_FatherName end+CASE WHEN  AMST_FatherSurname is null or AMST_FatherSurname = '' or AMST_FatherSurname = '0' then ''  ELSE ' ' + AMST_FatherSurname END,";
                                }
                            }
                        }

                        else if (name == "AMST_MobileNo")
                        {
                            if (name == "AMST_MobileNo")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/' +AMSTSMS_MobileNo  from dbo.Adm_Master_Student_SMSNo ff  where dbo.Adm_M_Student.AMST_Id=ff.AMST_Id for xml path('')),1,1,'')AMST_MobileNo,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/' +AMSTSMS_MobileNo  from dbo.Adm_Master_Student_SMSNo ff  where dbo.Adm_M_Student.AMST_Id=ff.AMST_Id for xml path('')),1,1,'')AMST_MobileNo,";
                                }
                            }
                        }

                        else if (name == "AMST_emailId")
                        {
                            if (name == "AMST_emailId")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +isnull(AMSTE_EmailId,'')    from dbo.Adm_Master_Student_EmailId gg  where dbo.Adm_M_Student.AMST_Id=gg.AMST_Id for xml path('')),1,1,'')AMST_emailId,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +isnull(AMSTE_EmailId,'')    from dbo.Adm_Master_Student_EmailId gg  where dbo.Adm_M_Student.AMST_Id=gg.AMST_Id for xml path('')),1,1,'')AMST_emailId,";
                                }
                            }
                        }
                        //-----Father Details----------//
                        else if (name == "AMST_FatherMobleNo")
                        {
                            if (name == "AMST_FatherMobleNo")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_FatherMobile_No as varchar(30))  from dbo.Adm_Master_FatherMobileNo hh  where dbo.Adm_M_Student.AMST_Id=hh.AMST_Id for xml path('')),1,1,'')AMST_FatherMobleNo,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_FatherMobile_No as varchar(30))  from dbo.Adm_Master_FatherMobileNo hh  where dbo.Adm_M_Student.AMST_Id=hh.AMST_Id for xml path('')),1,1,'')AMST_FatherMobleNo,";
                                }
                            }
                        }

                        else if (name == "AMST_FatheremailId")
                        {
                            if (name == "AMST_FatheremailId")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_FatheremailId  from dbo.Adm_Master_FatherEmail_Id ii   where dbo.Adm_M_Student.AMST_Id=ii.AMST_Id for xml path('')),1,1,'')AMST_FatheremailId,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_FatheremailId  from dbo.Adm_Master_FatherEmail_Id ii   where dbo.Adm_M_Student.AMST_Id=ii.AMST_Id for xml path('')),1,1,'')AMST_FatheremailId,";
                                }
                            }
                        }

                        //------Mother Details---------//

                        else if (name == "AMST_MotherMobileNo")
                        {
                            if (name == "AMST_MotherMobileNo")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_MotherMobileNo as varchar(30))  from dbo.Adm_Master_MotherMobileNo jj where dbo.Adm_M_Student.AMST_Id=jj.AMST_Id for xml path('')),1,1,'')AMST_MotherMobileNo,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_MotherMobileNo as varchar(30))  from dbo.Adm_Master_MotherMobileNo jj where dbo.Adm_M_Student.AMST_Id=jj.AMST_Id for xml path('')),1,1,'')AMST_MotherMobileNo,";
                                }
                            }
                        }

                        else if (name == "AMST_MotherEmailId")
                        {
                            if (name == "AMST_MotherEmailId")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_MotherEmailId from dbo.Adm_Master_MotherEmail_Id kk where dbo.Adm_M_Student.AMST_Id=kk.AMST_Id for xml path('')),1,1,'')AMST_MotherEmailId,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_MotherEmailId from dbo.Adm_Master_MotherEmail_Id kk where dbo.Adm_M_Student.AMST_Id=kk.AMST_Id for xml path('')),1,1,'')AMST_MotherEmailId,";
                                }
                            }
                        }
                        else if (name == "AMST_DOB")
                        {
                            if (name == "AMST_DOB")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    //IVRM_CLM_coloumn += "REPLACE(CONVERT(CHAR(11), AMST_DOB, 106),' ',' - ') AMST_DOB";
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_DOB, 103)) AMST_DOB,";
                                }
                                else
                                {
                                    // IVRM_CLM_coloumn += "REPLACE(CONVERT(CHAR(11), AMST_DOB, 106),' ',' - ') AMST_DOB,";
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_DOB, 103)) AMST_DOB,";
                                }
                            }
                        }
                        else if (name == "AMST_Date")
                        {
                            if (name == "AMST_Date")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    // IVRM_CLM_coloumn += "REPLACE(CONVERT(CHAR(11), AMST_Date, 106),' ',' - ') AMST_Date,";
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_Date, 103)) AMST_Date,";
                                }
                                else
                                {
                                    //IVRM_CLM_coloumn += "REPLACE(CONVERT(CHAR(11), AMST_Date, 106),' ',' - ') AMST_Date,";
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_Date, 103)) AMST_Date,";
                                }
                            }
                        }
                        else if(name == "JoinedYear")
                        {
                            if (name == "JoinedYear")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {                                    
                                        // IVRM_CLM_coloumn += "REPLACE(CONVERT(CHAR(11), AMST_Date, 106),' ',' - ') AMST_Date,";
                                        IVRM_CLM_coloumn += "(select a.ASMAY_Year from Adm_School_M_Academic_Year a where a.ASMAY_Id=Adm_M_Student.ASMAY_Id) as JoinedYear,";                                                                  
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(select a.ASMAY_Year from Adm_School_M_Academic_Year a where a.ASMAY_Id=Adm_M_Student.ASMAY_Id) as JoinedYear,";
                                }
                            }
                        }

                        else if (name == "JoinedClass")
                        {
                            if (name == "JoinedClass")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {                                  
                                        // IVRM_CLM_coloumn += "REPLACE(CONVERT(CHAR(11), AMST_Date, 106),' ',' - ') AMST_Date,";
                                        IVRM_CLM_coloumn += "(select n.ASMCL_ClassName from Adm_School_M_Class n where n.ASMCL_Id=Adm_M_Student.ASMCL_Id) as JoinedClass,";   
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(select n.ASMCL_ClassName from Adm_School_M_Class n where n.ASMCL_Id=Adm_M_Student.ASMCL_Id) as JoinedClass,";
                                }
                            }
                        }

                        else
                        {
                            if (IVRM_CLM_coloumn == "")
                            {
                                IVRM_CLM_coloumn = name;
                            }
                            else
                            {
                                IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                            }
                        }
                    }
                }
                string coloumns = "";
                if (IVRM_CLM_coloumn.EndsWith(","))
                {
                    coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
                }
                else
                {
                    coloumns = IVRM_CLM_coloumn;
                }


                using (var cmd = _Classwisestudentdetailscontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_classwisestudentdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@class",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@sec",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMC_Id
                    });
                    //cmd.Parameters.Add(new SqlParameter("@allindiflag",
                    //SqlDbType.VarChar)
                    //{
                    //    Value = data.allorindiflag
                    //});

                    cmd.Parameters.Add(new SqlParameter("@tablepara",
                   SqlDbType.VarChar)
                    {
                        Value = coloumns
                    });

                    cmd.Parameters.Add(new SqlParameter("@flag",
                 SqlDbType.VarChar)
                    {
                        Value = data.flag
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                SqlDbType.VarChar)
                    {
                        Value = data.mid
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

                                    var datatype = dataReader.GetFieldType(iFiled);

                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        string val = dataReader[iFiled].ToString();
                                        if (val == "")
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), "Not Available");
                                        }

                                        else
                                        {
                                            dataRow.Add(
                                                        dataReader.GetName(iFiled),
                                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dateval

                                         );
                                        }

                                    }

                                    else
                                    {
                                        string val = dataReader[iFiled].ToString();
                                        if (val == "")
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), "Not Available");
                                        }
                                        else
                                        {
                                            dataRow.Add(
                                                           dataReader.GetName(iFiled),
                                                             dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                                       );
                                        }

                                    }
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.alldatagridreport = retObject.ToArray();

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
        public ClasswisestudentdetailsDTO getsection(ClasswisestudentdetailsDTO data)
        {

            try
            {

                var sectiondata = (from a in _Classwisestudentdetailscontext.admissioncls
                                   from b in _Classwisestudentdetailscontext.school_M_Section
                                   from c in _Classwisestudentdetailscontext.AcademicYear
                                   from d in _Classwisestudentdetailscontext.Masterclasscategory
                                   from e in _Classwisestudentdetailscontext.AdmSchoolMasterClassCatSec
                                   where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                                   && d.MI_Id == data.mid && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == data.ASMCL_Id)
                                   select b
                                 ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();


                if (sectiondata.Length > 0)
                {
                    data.fillsection = sectiondata.ToArray();
                }
                else
                {
                    List<School_M_Section> secname = new List<School_M_Section>();
                    secname = _Classwisestudentdetailscontext.school_M_Section.Where(s => s.MI_Id == data.mid && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                    data.fillsection = secname.ToArray();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


    }
}
