using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using AdmissionServiceHub.com.vaps.Interfaces;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;



namespace AdmissionServiceHub.com.vaps.Services
{
    public class AdmissionRegisterImpl : AdmissionRegisterInterface
    {
        string IVRM_CLM_coloumn = "";
        string ASMCL_Id = "";
        private static ConcurrentDictionary<string, castecategoryDTO> _login =
        new ConcurrentDictionary<string, castecategoryDTO>();

        private readonly AdmissionRegisterContext _AdmissionRegisterContext;
        private readonly DomainModelMsSqlServerContext _db;

        public AdmissionRegisterImpl(AdmissionRegisterContext castecategoryContext, DomainModelMsSqlServerContext db)
        {
            _AdmissionRegisterContext = castecategoryContext;
            _db = db;
        }
     

    
        //public castecategoryDTO GetcastecategoryData(castecategoryDTO castecategoryDTO)//int IVRMM_Id
        //{

        //    Array[] showdata = new Array[50];
        //    List<castecategoryDMO> Allname = new List<castecategoryDMO>();
        //    Allname = _castecategoryContext.castecategoryDMO.ToList();
        //    castecategoryDTO.castecategoryname = Allname.ToArray();
        //    return castecategoryDTO;
        //}

        //public castecategoryDTO GetSelectedRowDetails(int ID)
        //{
        //    castecategoryDTO castecategoryDTO = new castecategoryDTO();
        //    List<castecategoryDMO> lorg = new List<castecategoryDMO>();
        //    lorg = _castecategoryContext.castecategoryDMO.Where(t => t.IMCC_Id.Equals(ID)).ToList();
        //    castecategoryDTO.castecategoryname = lorg.ToArray();
        //    return castecategoryDTO;
        //}

        //public castecategoryDTO MasterDeleteModulesData(int ID)
        //{
        //    castecategoryDTO castecategoryDTO = new castecategoryDTO();
        //    List<castecategoryDMO> masters = new List<castecategoryDMO>();
        //    masters = _castecategoryContext.castecategoryDMO.Where(t => t.IMCC_Id.Equals(ID)).ToList();
        //    if (masters.Any())
        //    {
        //        _castecategoryContext.Remove(masters.ElementAt(0));
        //        _castecategoryContext.SaveChanges();
        //    }
        //    else
        //    {

        //    }

        //    return castecategoryDTO;
        //}

        public SchoolYearWiseStudentDTO GetddlDatabind(SchoolYearWiseStudentDTO mas)
       {
            
            List<AcademicYear> aya = new List<AcademicYear>();
            aya = _AdmissionRegisterContext.academicyr.Where(r=>r.Is_Active==true && r.MI_Id==mas.MI_Id).ToList();
            mas.YearList = aya.OrderByDescending(a=>a.ASMAY_Order).ToArray();

            List<School_M_Class> aya1 = new List<School_M_Class>();
            aya1 = _AdmissionRegisterContext.classs.Where(r => r.ASMCL_ActiveFlag == true && r.MI_Id == mas.MI_Id).ToList();
            mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();

            List<IVRM_COLOUMN_REPORT> aya3 = new List<IVRM_COLOUMN_REPORT>();
            aya3 = _AdmissionRegisterContext.section.ToList();
            mas.sectionList = aya3.ToArray();


            return mas;
        }

        public async Task<SchoolYearWiseStudentDTO> Getdetailsreport(SchoolYearWiseStudentDTO reg)
        {
            List<long> clsId = new List<long>();

            foreach (var item in reg.TempararyArrayListclass)
            {
                clsId.Add(item.ASMCL_Id);
            }



            //reg.alldata = (from a in _AdmissionRegisterContext.Adm_M_Student
            //               from b in _AdmissionRegisterContext.School_Adm_Y_StudentDMO

            //               where (a.AMST_Id == b.AMST_Id && b.AMAY_Id == reg.ASMAY_Id)
            //               select new SchoolYearWiseStudentDTO
            //               {

            //               }
            //      ).Distinct().ToArray();


            //return reg;


            for (int i = 0; i < reg.TempararyArrayListcoloumn.Length; i++)
            {
                string Id = reg.TempararyArrayListcoloumn[i].IVRM_CLM_PAR;

                string name = Id ;
                 IVRM_CLM_coloumn = name +","+ IVRM_CLM_coloumn;


                //_feegrouppage.SaveYearlyGroupData(Id, reg);
            }
         string coloumns  = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

            for (int i = 0; i < reg.TempararyArrayListclass.Length; i++)
            {
                long Id = reg.TempararyArrayListclass[i].ASMCL_Id;

                long name = Id;
                ASMCL_Id = Convert.ToString( name + "," + ASMCL_Id);


                //_feegrouppage.SaveYearlyGroupData(Id, reg);
            }
            string classes  = ASMCL_Id.Remove(ASMCL_Id.Length - 1);

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Admission_Register_Report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@tableparam",
                    SqlDbType.VarChar)
                {
                    Value = coloumns
                });
                cmd.Parameters.Add(new SqlParameter("@class",
                   SqlDbType.VarChar)
                {
                    Value = classes
                });
                cmd.Parameters.Add(new SqlParameter("@year",
                   SqlDbType.VarChar)
                {
                    Value = reg.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@att",
                   SqlDbType.VarChar)
                {
                    Value = reg.AMST_SOL
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    reg.SearchstudentDetails = retObject.ToArray();
                }
                catch (Exception ex)
                {

                }

            }

            return reg;
        }

      

        //SchoolYearWiseStudentDTO AdmissionRegisterInterface.Getdetailsreport(SchoolYearWiseStudentDTO reg)
        //{
        //    throw new NotImplementedException();
        //}

        //public StateDTO enqdrpcountrydata(int id)
        //{


        //    Array[] drpall = new Array[3];
        //    //CountryDTO[] coun=new CountryDTO[] 
        //    //    {
        //    //        new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="hutching"},
        //    //        new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="khosys" },
        //    //          new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="St.Philomina"},


        //    //    } ;

        //    StateDTO enq = new StateDTO();
        //    List<State> allstate = new List<State>();
        //    //     allstate = _Enquirycontext.country.Where(t => t.IVRMMC_Id.Equals(id)).FirstO;
        //    // allstate.Add(new (101, "India"));

        //    allstate = _Enquirycontext.state.ToList();
        //    allstate = _Enquirycontext.state.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
        //    enq.stateDrpDown = allstate.ToArray();




        //    return enq;

        //    //ViewBag.Country = "UK";
        //}
    }
}
