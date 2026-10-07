using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
//using MailKit.Net.Smtp;
//using MimeKit;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Net;

using PreadmissionDTOs.com.vaps.admission;



namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class studentbirthdayreportFacadeController
    {

        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;


        public studentbirthdayreportFacadeController(DomainModelMsSqlServerContext db, ApplicationDBContext ApplicationDBContext)
        {

            _db = db;
            _ApplicationDBContext = ApplicationDBContext;
        }



        //[HttpPost]
        // [Route("getdetails")]
        // //[Route("getenquirycontroller")]
        // public studentbirthdayreportDTO getdetails([FromBody] studentbirthdayreportDTO data)
        // {
        //     // id = 12;
        //     return _dd.getdetails(data);

        // }
        [Route("getYear/{id:int}")]
        public studentbirthdayreportDTO getYear(int id)
        {
            studentbirthdayreportDTO dto = new studentbirthdayreportDTO();
            dto.accyear = _db.AcademicYear.Where(d => d.MI_Id == id && d.Is_Active == true).OrderBy(d => d.ASMAY_Order).ToArray();
            dto.get_mi_list1 = (from I in _db.Institute
                                 where I.MI_ActiveFlag == 1
                                 select new studentbirthdayreportDTO
                                 {
                                     MI_ID = I.MI_Id,
                                     MI_Name = I.MI_Name
                                 }).ToArray();
            return dto;

        }


        [HttpPost]
        [Route("getdetails")]
        //[Route("getenquirycontroller")]
        public async Task<studentbirthdayreportDTO> getdetails([FromBody] studentbirthdayreportDTO data)
        {
            // id = 12;
            //return _dd.getdetails(data);

            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
            confromdate = fromdatecon.ToString("yyyy-MM-dd");

            DateTime todatecon = DateTime.Now;
            string contodate = "";
            todatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
            contodate = todatecon.ToString("yyyy-MM-dd");

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Student_Birthday_Report_New";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@month",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(data.months)
                });

                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {


                    Value = confromdate
                }

                );

                cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                {


                    Value = contodate
                }

               );
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {


                    Value = Convert.ToInt64(data.MI_ID)
                }

             );

                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.Text)
                {


                    Value = Convert.ToString(data.flag)
                }

               );


                cmd.Parameters.Add(new SqlParameter("@all1", SqlDbType.Text)
                {


                    Value = Convert.ToString(data.all1)
                }

               );




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
                                    dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentDetails = retObject.ToArray();
                    if (data.studentDetails.Length > 0)
                    {
                        data.count = data.studentDetails.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                    data.schooldetails = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_ID).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return data;


        }

        [Route("getdetailsadd")]
        //[Route("getenquirycontroller")]
        public async Task<studentbirthdayreportDTO> getdetailsadd([FromBody] studentbirthdayreportDTO data)
        {
            // id = 12;
            //return _dd.getdetails(data);

            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
            confromdate = fromdatecon.ToString("yyyy-MM-dd");

            DateTime todatecon = DateTime.Now;
            string contodate = "";
            todatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
            contodate = todatecon.ToString("yyyy-MM-dd");

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Student_Birthday_Report_New";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@month",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(data.months)
                });

                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {
                    Value = confromdate
                });

                cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                {
                    Value = contodate
                });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.MI_ID)
                });

                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.Text)
                {
                    Value = Convert.ToString(data.flag)
                });


                cmd.Parameters.Add(new SqlParameter("@all1", SqlDbType.Text)
                {
                    Value = Convert.ToString(data.all1)
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
                                    dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentDetails = retObject.ToArray();
                    if (data.studentDetails.Length > 0)
                    {
                        data.count = data.studentDetails.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                    data.schooldetails = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_ID).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return data;


        }



        [Route("ExportToExcle/")]

        public string ExportToExcle([FromBody] studentbirthdayreportDTO reg)
        {
            string str = "";



            return str;
        }


    }


}
