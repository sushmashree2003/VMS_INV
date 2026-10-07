using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Rating.Services
{
    public class TypeQuestionOptionMappingImpl : Interfacce.TypeQuestionOptionMappingInterface
    {
        public IssueManagerContext _context;
        private object _IssueContext;

        public TypeQuestionOptionMappingImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }
        public Rating_ReportDTO OnLoadRatingMasterType(Rating_ReportDTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }
                data.getmastertype = _context.Rating_Master_TypeDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //public Rating_ReportDTO OnClickMasterQuestions(Rating_ReportDTO data)
        //{
        //    try
        //    {
        //        if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
        //        {
        //            var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

        //            List<long> miids = new List<long>();

        //            foreach (var c in getuserinstitution)
        //            {
        //                miids.Add(c.MI_Id);
        //            }

        //            data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();
        //        }

        //        data.GetMasterQuestionLoadData = _context.Ratings_Master_QuestionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();

        //        data.GetMasterQuestionOrderData = _context.Ratings_Master_QuestionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).OrderBy(a => a.RATMQE_RQOrder).ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}


        public async Task<Rating_ReportDTO> getReport(Rating_ReportDTO data)
        {
            try
            {

                data.get_report = (from a in _context.Rating_Type_Questions_OptionsDMO
                                   from b in _context.Rating_Type_QuestionsDMO
                                   from c in _context.Rating_Master_OptionsDMO
                                   from d in _context.Ratings_Master_QuestionsDMO
                                   where (b.MI_Id == data.MI_Id && b.RATMT_Id == data.RATMT_Id
                                   && a.RATMTQ_Id == b.RATMTQ_Id && a.RATMOP_Id == c.RATMOP_Id && b.RATMQE_Id == d.RATMQE_Id)
                                   select new Rating_ReportDTO
                                   {
                                       RATMQE_RatingQuestions = d.RATMQE_RatingQuestions,
                                       RATMOP_RatingOptions = c.RATMOP_RatingOptions,
                                       RATMT_Id = b.RATMT_Id,
                                       RATMOP_Id = c.RATMOP_Id,
                                       RATMQE_Id = d.RATMQE_Id
                                   }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<Rating_ReportDTO> getDeptReport(Rating_ReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TypeQuestionDepartmentMapping";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@RATMT_Id", SqlDbType.BigInt)
                    {
                        Value = data.RATMT_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.deptdetails = retObject.ToArray();
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
