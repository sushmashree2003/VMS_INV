using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.IssueManager.Rating;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;

namespace IssueManager.com.Rating.Services
{
    public class Rating_TransactionImpl : Interfacce.Rating_TransactionInterface
    {
        public IssueManagerContext _context;
        public Rating_TransactionImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }

        public Rating_TransactionDTO OnLoadRatingTransaction(Rating_TransactionDTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    List<long> miids = new List<long>();

                    foreach (var c in getuserinstitution)
                    {
                        miids.Add(c.MI_Id);
                    }

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();
                }

                // data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id==data.MI_Id).ToArray();

                var rolet = _context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = rolet.FirstOrDefault().IVRMRT_Role;




                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Rating_Transaction_Load_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.VarChar) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetSavedDataOnLoad = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Added By Akash

                data.getRatingType = (from a in _context.Rating_Type_QuestionsDMO
                                      from b in _context.Rating_Master_TypeDMO
                                      where (a.RATMT_Id == b.RATMT_Id && b.RATMT_LInkedToRelievingFlg == true && a.MI_Id == data.MI_Id
                                              && b.MI_Id == data.MI_Id)
                                      select new Rating_TransactionDTO
                                      {
                                          RATMT_Id = b.RATMT_Id,
                                          typeNameList = b.RATMT_RatingTypeName,
                                      }).Distinct().ToArray();




                data.GetRatingMasterType = (from b in _context.Rating_Type_QuestionsDMO
                                            from d in _context.Rating_Master_TypeDMO
                                            where (b.RATMT_Id == d.RATMT_Id
                                            && b.RATMTQ_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                            select d).Distinct().ToArray();




                data.overAllTypeGrid = (from a in _context.Rating_Type_Questions_RatingDMO
                                        from b in _context.Rating_Type_QuestionsDMO
                                        from c in _context.Rating_Master_TypeDMO
                                        from d in _context.Ratings_Master_QuestionsDMO
                                        where (a.RATMT_Id == b.RATMT_Id && a.RATMT_Id == c.RATMT_Id
                                        && a.RATMQE_Id == d.RATMQE_Id && c.RATMT_LInkedToRelievingFlg == true &&
                                        b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                        select new Rating_TransactionDTO
                                        {
                                            RATMTQR_Id = a.RATMTQR_Id,
                                            RATMT_Id = a.RATMT_Id,
                                            RATMQE_Id = a.RATMQE_Id,
                                            RATMTQ_Id = a.RATMTQ_Id,
                                            typeNameList = c.RATMT_RatingTypeName,
                                            questionNameList = d.RATMQE_RatingQuestions,
                                            fromvaluelist = a.RATMTQR_FromValue,
                                            tovaluelist = a.RATMTQR_ToValue,
                                            ratinglist = a.RATMTQR_Rating,
                                            RATMTQR_ActiveFlag = a.RATMTQR_ActiveFlag
                                        }).Distinct().ToArray();



                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_RatingNotdone_User_EmployeeListtnew";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.BigInt) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetEmployeeUnratedDetails = retObject.ToArray();
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

        public Rating_TransactionDTO checkrating(Rating_TransactionDTO data)
        {
            try
            {
                var rolet = _context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = rolet.FirstOrDefault().IVRMRT_Role;

                var year = data.RATTR_FeedbackDate.Value.Year;
                var month = data.RATTR_FeedbackDate.Value.Month;

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Rating_User_EmployeeList_Datewise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.flag });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = year });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = month });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetEmployeeDetails = retObject.ToArray();
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

        public Rating_TransactionDTO OnChangeEmployee(Rating_TransactionDTO data)
        {
            try
            {
                var EmployeeDetails = _context.MasterEmployee.Where(a => a.HRME_Id == data.HRME_Id).ToList();

                var HRMD_Id = EmployeeDetails.FirstOrDefault().HRMD_Id;
                var HRMDES_Id = EmployeeDetails.FirstOrDefault().HRMDES_Id;
                var MI_Id = EmployeeDetails.FirstOrDefault().MI_Id;

                var empdetails = (from a in _context.MasterEmployee
                                  from b in _context.Staff_User_Login
                                  where (a.HRME_Id == b.Emp_Code && a.HRME_Id == data.HRME_Id)
                                  select new Rating_TransactionDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      UserId = b.Id,
                                      MI_Id = a.MI_Id,
                                      HRME_DOJ = a.HRME_DOJ
                                  }).ToList();


                if (empdetails[0].HRME_DOJ <= data.RATTR_FeedbackDate)
                {
                    //var result = _context.Rating_TransactionDMO.Where(a => a.HRME_Id == data.HRME_Id && a.RATTR_FeedbackDate.Value.Month == data.RATTR_FeedbackDate.Value.Month
                    //&& a.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year).ToList();

                    var result = (from a in _context.Rating_TransactionDMO
                                  from b in _context.Ratings_Master_QuestionsDMO
                                  where (a.RATMQE_Id == b.RATMQE_Id && a.HRME_Id == data.HRME_Id && a.RATTR_FeedbackDate.Value.Month == data.RATTR_FeedbackDate.Value.Month &&
                                  a.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year && (b.RATMQE_AutomaticFlg == false || b.RATMQE_AutomaticFlg == null))
                                  select a).ToList();

                    if (result.Count == 0)
                    {
                        data.GetRatingMasterType = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                                    from b in _context.Rating_Type_QuestionsDMO
                                                    from c in _context.Ratings_Master_QuestionsDMO
                                                    from d in _context.Rating_Master_TypeDMO
                                                    where (a.RATMTQ_Id == b.RATMTQ_Id && b.RATMQE_Id == c.RATMQE_Id && b.RATMT_Id == d.RATMT_Id && a.RATMTQD_ActiveFlag == true
                                                    && b.RATMTQ_ActiveFlag == true && a.HRMD_Id == HRMD_Id && a.HRMDES_Id == HRMDES_Id && b.MI_Id == MI_Id)
                                                    select d).Distinct().ToArray();
                    }
                    else
                    {
                        data.returnmsg = "duplicate";
                    }
                }
                else
                {
                    data.returnmsg = "DOJ";
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.employeeprofile = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PendingIssues";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = empdetails[0].UserId });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMPR_Id", SqlDbType.BigInt) { Value = 0 });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.prioritylist = retObject.ToArray();
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
        public Rating_TransactionDTO GetSearchDetails(Rating_TransactionDTO data)
        {
            try
            {

                var EmployeeDetails = _context.MasterEmployee.Where(a => a.HRME_Id == data.HRME_Id).ToList();

                var HRMD_Id = EmployeeDetails.FirstOrDefault().HRMD_Id;
                var HRMDES_Id = EmployeeDetails.FirstOrDefault().HRMDES_Id;
                data.MI_Id = EmployeeDetails.FirstOrDefault().MI_Id;

                var empdetails = (from a in _context.MasterEmployee
                                  from b in _context.Staff_User_Login
                                  where (a.HRME_Id == b.Emp_Code && a.HRME_Id == data.HRME_Id)
                                  select new Rating_TransactionDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      UserId = b.Id,
                                      MI_Id = a.MI_Id
                                  }).ToList();

                List<long> cateid = new List<long>();
                foreach (var e in data.ratingtypelist)
                {
                    cateid.Add(e.RATMT_Id);
                }


                if (data.flag == "Regular")
                {

                    var result = _context.Rating_TransactionDMO.Where(a => a.HRME_Id == data.HRME_Id && a.RATTR_FeedbackDate.Value.Month == data.RATTR_FeedbackDate.Value.Month
                    && a.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year).ToList();

                    var questioncount = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                         from b in _context.Rating_Type_QuestionsDMO
                                         from c in _context.Ratings_Master_QuestionsDMO
                                         where (a.RATMTQ_Id == b.RATMTQ_Id && b.RATMQE_Id == c.RATMQE_Id && c.MI_Id == data.MI_Id && a.HRMD_Id == HRMD_Id && a.HRMDES_Id == HRMDES_Id
                                         && c.RATMQE_ActiveFlag == true && b.RATMTQ_ActiveFlag == true)
                                         select a).ToList();

                    if (questioncount.Count >= result.Count)
                    {

                        try
                        {
                            // var confirmstatus = _context.Database.ExecuteSqlCommand("HR_EmployeeRating_Automaticprocess @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.HRME_Id, data.RATTR_FeedbackDate.Value.Day, data.RATTR_FeedbackDate.Value.Month, data.RATTR_FeedbackDate.Value.Year, empdetails[0].UserId);

                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "HR_EmployeeRating_Automaticprocess";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 50000;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt) { Value = data.HRME_Id });
                                cmd.Parameters.Add(new SqlParameter("@Day", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Day });
                                cmd.Parameters.Add(new SqlParameter("@MONTHID", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Month });
                                cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Year });
                                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt) { Value = empdetails[0].UserId });
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
                                                dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                            }
                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    data.getTypeQuestionTransaction = retObject.ToArray();


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
                    }


                    //data.getRatingTypeTransaction = (from a in _context.Rating_TransactionDMO
                    //                                 from b in _context.Rating_Master_TypeDMO
                    //                                 where (a.RATMT_Id == b.RATMT_Id && a.HRME_Id == data.HRME_Id && a.RATTR_FeedbackDate.Value.Month == data.RATTR_FeedbackDate.Value.Month &&
                    //                                 a.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year)
                    //                                 select b).OrderBy(b => b.RATMT_RatingTypeName).Distinct().ToArray();


                    data.getRatingTypeTransaction = (from a in _context.Rating_Type_QuestionsDMO
                                                     from b in _context.Rating_Master_TypeDMO
                                                     from c in _context.Ratings_Master_QuestionsDMO
                                                     where (a.RATMT_Id == b.RATMT_Id && a.RATMQE_Id == c.RATMQE_Id && a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && c.MI_Id == empdetails[0].MI_Id &&
                                                     c.RATMQE_AutomaticFlg == true)
                                                     select b).OrderBy(b => b.RATMT_RatingTypeName).Distinct().ToArray();

                    data.getRatingClassificationTransaction = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                                               from b in _context.Rating_Type_QuestionsDMO
                                                               from c in _context.Rating_Master_TypeDMO
                                                               from d in _context.Rating_Master_Question_ClassificationDMO
                                                               from e in _context.Rating_Master_Question_SubClassificationDMO
                                                               from f in _context.Ratings_Master_QuestionsDMO
                                                               where (a.RATMTQ_Id == b.RATMTQ_Id && b.RATMT_Id == d.RATMT_Id && c.RATMT_Id == d.RATMT_Id &&
                                                               d.RATMQECL_Id == e.RATMQECL_Id && e.RATMQECLSUB_Id == f.RATMQECLSUB_Id && f.MI_Id == empdetails[0].MI_Id && b.RATMQE_Id == f.RATMQE_Id &&
                                                               f.RATMQE_AutomaticFlg == true && a.HRMD_Id == HRMD_Id && a.HRMDES_Id == HRMDES_Id)
                                                               select d).OrderBy(d => d.RATMQECL_RQOrder).Distinct().ToArray();


                    data.getRatingSubClassificationTransaction = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                                                  from b in _context.Rating_Type_QuestionsDMO
                                                                  from c in _context.Rating_Master_TypeDMO
                                                                  from d in _context.Rating_Master_Question_ClassificationDMO
                                                                  from e in _context.Rating_Master_Question_SubClassificationDMO
                                                                  from f in _context.Ratings_Master_QuestionsDMO
                                                                  where (a.RATMTQ_Id == b.RATMTQ_Id && b.RATMT_Id == d.RATMT_Id && c.RATMT_Id == d.RATMT_Id &&
                                                                  d.RATMQECL_Id == e.RATMQECL_Id && e.RATMQECLSUB_Id == f.RATMQECLSUB_Id && f.MI_Id == empdetails[0].MI_Id && b.RATMQE_Id == f.RATMQE_Id &&
                                                                  f.RATMQE_AutomaticFlg == true && a.HRMD_Id == HRMD_Id && a.HRMDES_Id == HRMDES_Id)
                                                                  select e).OrderBy(e => e.RATMQECLSUB_RQOrder).Distinct().ToArray();



                    //data.getTypeQuestionTransaction = (from a in _context.Rating_TransactionDMO
                    //                                   from b in _context.Ratings_Master_QuestionsDMO
                    //                                   from c in _context.Rating_Master_OptionsDMO
                    //                                   where (a.RATMQE_Id == b.RATMQE_Id && c.RATMOP_Id == a.RATMOP_Id && a.HRME_Id == data.HRME_Id && cateid.Contains(a.RATMT_Id) &&
                    //                                   a.RATTR_FeedbackDate.Value.Month == data.RATTR_FeedbackDate.Value.Month && a.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year
                    //                                   && b.RATMQE_AutomaticFlg == true)
                    //                                   select new Rating_TransactionDTO
                    //                                   {
                    //                                       RATTR_Id = a.RATTR_Id,
                    //                                       RATMQE_Id = b.RATMQE_Id,
                    //                                       MI_Id = b.MI_Id,
                    //                                       RATMQE_RatingQuestions = b.RATMQE_RatingQuestions,
                    //                                       RATMQE_ManualEntryFlg = b.RATMQE_ManualEntryFlg,
                    //                                       RATMQE_RQOrder = b.RATMQE_RQOrder,
                    //                                       RATMQE_AutomaticFlg = b.RATMQE_AutomaticFlg,
                    //                                       RATMT_Id = a.RATMT_Id,
                    //                                       RATMOP_Id = c.RATMOP_Id,
                    //                                       RATMOP_RatingOptions = c.RATMOP_RatingOptions,
                    //                                       RATMOP_OptionsValue = c.RATMOP_OptionsValue,
                    //                                       RATMQE_RatingsQRemarks = b.RATMQE_RatingsQRemarks

                    //                                   }).ToArray();

                    data.getTypeQuestionOptionTransaction = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                                             from b in _context.Rating_Type_QuestionsDMO
                                                             from c in _context.Rating_Master_TypeDMO
                                                             from d in _context.Ratings_Master_QuestionsDMO
                                                             from e in _context.Rating_Type_Questions_OptionsDMO
                                                             from f in _context.Rating_Master_OptionsDMO
                                                             where (b.RATMT_Id == c.RATMT_Id && b.RATMQE_Id == d.RATMQE_Id && e.RATMTQ_Id == b.RATMTQ_Id && f.RATMOP_Id == e.RATMOP_Id && f.MI_Id == empdetails[0].MI_Id
                                                             && b.RATMTQ_ActiveFlag == true && d.RATMQE_AutomaticFlg == true && a.HRMD_Id == HRMD_Id && a.HRMDES_Id == HRMDES_Id
                                                             )
                                                             select new Rating_TransactionDTO
                                                             {
                                                                 RATMOP_OptionsValue = f.RATMOP_OptionsValue,
                                                                 RATMOP_RatingOptions = f.RATMOP_RatingOptions,
                                                                 RATMOP_ROOrder = f.RATMOP_ROOrder,
                                                                 RATMOP_Id = f.RATMOP_Id,
                                                                 RATMQE_Id = d.RATMQE_Id,
                                                                 RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg
                                                             }).Distinct().ToArray(); //OrderBy(a => a.RATMOP_ROOrder).


                    data.getRatingTypedata = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                              from b in _context.Rating_Type_QuestionsDMO
                                              from c in _context.Ratings_Master_QuestionsDMO
                                              from e in _context.Rating_Master_Question_SubClassificationDMO
                                              from f in _context.Rating_Master_Question_ClassificationDMO
                                              from d in _context.Rating_Master_TypeDMO
                                              where (a.RATMTQ_Id == b.RATMTQ_Id && b.RATMQE_Id == c.RATMQE_Id && b.RATMT_Id == d.RATMT_Id && a.RATMTQD_ActiveFlag == true && c.RATMQE_RatingsQRemarks != "OVERALLRATING"
                                              && c.RATMQECLSUB_Id == e.RATMQECLSUB_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMT_Id == d.RATMT_Id
                                              && b.RATMTQ_ActiveFlag == true && a.HRMD_Id == HRMD_Id && a.HRMDES_Id == HRMDES_Id && b.MI_Id == data.MI_Id && cateid.Contains(d.RATMT_Id))
                                              select d).OrderBy(d => d.RATMT_Id).Distinct().ToArray(); //OrderBy(d => d.RATMT_RatingTypeName).  && d.RATMT_QuestionwiseOptionFlg == false


                    data.GetTypeClassificationQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                                           from b in _context.Rating_Type_Questions_DepartmentDMO
                                                           from c in _context.Rating_Master_TypeDMO
                                                           from e in _context.Rating_Master_Question_ClassificationDMO
                                                           from f in _context.Rating_Master_Question_SubClassificationDMO
                                                           from d in _context.Ratings_Master_QuestionsDMO
                                                           where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id &&
                                                           c.RATMT_Id == e.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id
                                                           && a.RATMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id
                                                           && cateid.Contains(a.RATMT_Id) && d.RATMQE_AutomaticFlg == false && c.RATMT_QuestionwiseOptionFlg == false)
                                                           select e).OrderBy(e => e.RATMQECL_RQOrder).Distinct().ToArray();

                    data.GetTypeSubClassificationQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                                              from b in _context.Rating_Type_Questions_DepartmentDMO
                                                              from c in _context.Rating_Master_TypeDMO
                                                              from e in _context.Rating_Master_Question_ClassificationDMO
                                                              from f in _context.Rating_Master_Question_SubClassificationDMO
                                                              from d in _context.Ratings_Master_QuestionsDMO
                                                              where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id &&
                                                              c.RATMT_Id == e.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id
                                                              && a.RATMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id
                                                              && cateid.Contains(a.RATMT_Id) && d.RATMQE_AutomaticFlg == false && c.RATMT_QuestionwiseOptionFlg == false)
                                                              select f).OrderBy(f => f.RATMQECLSUB_RQOrder).Distinct().ToArray();


                    data.GetTypeQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                             from b in _context.Rating_Type_Questions_DepartmentDMO
                                             from c in _context.Rating_Master_TypeDMO
                                             from e in _context.Rating_Master_Question_ClassificationDMO
                                             from f in _context.Rating_Master_Question_SubClassificationDMO
                                             from d in _context.Ratings_Master_QuestionsDMO
                                             where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id &&
                                               c.RATMT_Id == e.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id
                                             && a.RATMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id
                                             && cateid.Contains(a.RATMT_Id) && d.RATMQE_AutomaticFlg == false && c.RATMT_QuestionwiseOptionFlg == false)
                                             select new Rating_TransactionDTO
                                             {
                                                 RATMQE_Id = d.RATMQE_Id,
                                                 MI_Id = d.MI_Id,
                                                 RATMQE_RatingQuestions = d.RATMQE_RatingQuestions,
                                                 RATMQE_ManualEntryFlg = d.RATMQE_ManualEntryFlg,
                                                 RATMQE_RQOrder = d.RATMQE_RQOrder,
                                                 RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                                 RATMT_Id = c.RATMT_Id,
                                                 RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                                 RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                             }).Distinct().ToArray();

                    data.GetTypeQuestionOptions = (from a in _context.Rating_Type_QuestionsDMO
                                                   from b in _context.Rating_Type_Questions_DepartmentDMO
                                                   from c in _context.Rating_Master_TypeDMO
                                                   from d in _context.Ratings_Master_QuestionsDMO
                                                   from e in _context.Rating_Type_Questions_OptionsDMO
                                                   from f in _context.Rating_Master_OptionsDMO
                                                   where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                                   && a.RATMTQ_Id == e.RATMTQ_Id && e.RATMOP_Id == f.RATMOP_Id && a.RATMTQ_ActiveFlag == true
                                                   && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id && d.RATMQE_AutomaticFlg == false && c.RATMT_QuestionwiseOptionFlg == false
                                                   && cateid.Contains(a.RATMT_Id))
                                                   select new Rating_TransactionDTO
                                                   {
                                                       RATMOP_OptionsValue = f.RATMOP_OptionsValue,
                                                       RATMOP_RatingOptions = f.RATMOP_RatingOptions,
                                                       RATMOP_ROOrder = f.RATMOP_ROOrder,
                                                       RATMOP_Id = f.RATMOP_Id,
                                                       RATMQE_Id = a.RATMQE_Id,
                                                       RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                                       RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                                   }).Distinct().OrderByDescending(a => a.RATMOP_OptionsValue).ToArray();

                }
                else
                {

                    //data.getRatingTypedata = (from a in _context.Rating_Type_Questions_DepartmentDMO
                    //                          from b in _context.Rating_Type_QuestionsDMO
                    //                          from c in _context.Ratings_Master_QuestionsDMO
                    //                          from d in _context.Rating_Master_TypeDMO
                    //                          where (a.RATMTQ_Id == b.RATMTQ_Id && b.RATMQE_Id == c.RATMQE_Id && b.RATMT_Id == d.RATMT_Id && a.RATMTQD_ActiveFlag == true
                    //                          && b.RATMTQ_ActiveFlag == true && a.HRMD_Id == HRMD_Id && a.HRMDES_Id == HRMDES_Id && b.MI_Id == data.MI_Id && cateid.Contains(d.RATMT_Id))
                    //                          select d).OrderBy(d => d.RATMT_Id).Distinct().ToArray(); //OrderBy(d => d.RATMT_RatingTypeName).


                    data.getRatingTypedata = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                              from b in _context.Rating_Type_QuestionsDMO
                                              from c in _context.Ratings_Master_QuestionsDMO
                                              from e in _context.Rating_Master_Question_SubClassificationDMO
                                              from f in _context.Rating_Master_Question_ClassificationDMO
                                              from d in _context.Rating_Master_TypeDMO
                                              where (a.RATMTQ_Id == b.RATMTQ_Id && b.RATMQE_Id == c.RATMQE_Id && b.RATMT_Id == d.RATMT_Id && a.RATMTQD_ActiveFlag == true && c.RATMQE_RatingsQRemarks != "OVERALLRATING"
                                              && c.RATMQECLSUB_Id == e.RATMQECLSUB_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMT_Id == d.RATMT_Id
                                              && b.RATMTQ_ActiveFlag == true && a.HRMD_Id == HRMD_Id && a.HRMDES_Id == HRMDES_Id && b.MI_Id == data.MI_Id && cateid.Contains(d.RATMT_Id))
                                              select d).OrderBy(d => d.RATMT_Id).Distinct().ToArray();



                    data.GetTypeClassificationQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                                           from b in _context.Rating_Type_Questions_DepartmentDMO
                                                           from c in _context.Rating_Master_TypeDMO
                                                           from e in _context.Rating_Master_Question_ClassificationDMO
                                                           from f in _context.Rating_Master_Question_SubClassificationDMO
                                                           from d in _context.Ratings_Master_QuestionsDMO
                                                           where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id &&
                                                           c.RATMT_Id == e.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id
                                                           && a.RATMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id
                                                           && cateid.Contains(a.RATMT_Id)  && c.RATMT_QuestionwiseOptionFlg == false)
                                                           select e).OrderBy(e => e.RATMQECL_RQOrder).Distinct().ToArray();



                    data.GetTypeSubClassificationQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                                              from b in _context.Rating_Type_Questions_DepartmentDMO
                                                              from c in _context.Rating_Master_TypeDMO
                                                              from e in _context.Rating_Master_Question_ClassificationDMO
                                                              from f in _context.Rating_Master_Question_SubClassificationDMO
                                                              from d in _context.Ratings_Master_QuestionsDMO
                                                              where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id &&
                                                              c.RATMT_Id == e.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id
                                                              && a.RATMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id
                                                              && cateid.Contains(a.RATMT_Id)  && c.RATMT_QuestionwiseOptionFlg == false)
                                                              select f).OrderBy(f => f.RATMQECLSUB_RQOrder).Distinct().ToArray();


                    data.GetTypeQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                             from b in _context.Rating_Type_Questions_DepartmentDMO
                                             from c in _context.Rating_Master_TypeDMO
                                             from e in _context.Rating_Master_Question_ClassificationDMO
                                             from f in _context.Rating_Master_Question_SubClassificationDMO
                                             from d in _context.Ratings_Master_QuestionsDMO
                                             where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id &&
                                               c.RATMT_Id == e.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id
                                             && a.RATMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id
                                             && cateid.Contains(a.RATMT_Id)  && c.RATMT_QuestionwiseOptionFlg == false)
                                             select new Rating_TransactionDTO
                                             {
                                                 RATMQE_Id = d.RATMQE_Id,
                                                 MI_Id = d.MI_Id,
                                                 RATMQE_RatingQuestions = d.RATMQE_RatingQuestions,
                                                 RATMQE_ManualEntryFlg = d.RATMQE_ManualEntryFlg,
                                                 RATMQE_RQOrder = d.RATMQE_RQOrder,
                                                 RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                                 RATMT_Id = c.RATMT_Id,
                                                 RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                                 RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                             }).Distinct().ToArray();


                    data.GetTypeQuestionOptions = (from a in _context.Rating_Type_QuestionsDMO
                                                   from b in _context.Rating_Type_Questions_DepartmentDMO
                                                   from c in _context.Rating_Master_TypeDMO
                                                   from d in _context.Ratings_Master_QuestionsDMO
                                                   from e in _context.Rating_Type_Questions_OptionsDMO
                                                   from f in _context.Rating_Master_OptionsDMO
                                                   where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                                   && a.RATMTQ_Id == e.RATMTQ_Id && e.RATMOP_Id == f.RATMOP_Id && a.RATMTQ_ActiveFlag == true
                                                   && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id  && c.RATMT_QuestionwiseOptionFlg == false
                                                   && cateid.Contains(a.RATMT_Id))
                                                   select new Rating_TransactionDTO
                                                   {
                                                       RATMOP_OptionsValue = f.RATMOP_OptionsValue,
                                                       RATMOP_RatingOptions = f.RATMOP_RatingOptions,
                                                       RATMOP_ROOrder = f.RATMOP_ROOrder,
                                                       RATMOP_Id = f.RATMOP_Id,
                                                       RATMQE_Id = a.RATMQE_Id,
                                                       RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                                       RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                                   }).Distinct().OrderByDescending(a => a.RATMOP_OptionsValue).ToArray();


                    //data.GetTypeQuestions = (from a in _context.Rating_Type_QuestionsDMO
                    //                         from b in _context.Rating_Type_Questions_DepartmentDMO
                    //                         from c in _context.Rating_Master_TypeDMO
                    //                         from d in _context.Ratings_Master_QuestionsDMO
                    //                         where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                    //                         && a.RATMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id
                    //                         && cateid.Contains(a.RATMT_Id) && d.RATMQE_AutomaticFlg == false)
                    //                         select new Rating_TransactionDTO
                    //                         {
                    //                             RATMQE_Id = d.RATMQE_Id,
                    //                             MI_Id = d.MI_Id,
                    //                             RATMQE_RatingQuestions = d.RATMQE_RatingQuestions,
                    //                             RATMQE_ManualEntryFlg = d.RATMQE_ManualEntryFlg,
                    //                             RATMQE_RQOrder = d.RATMQE_RQOrder,
                    //                             RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                    //                             RATMT_Id = c.RATMT_Id

                    //                         }).Distinct().ToArray();

                    //data.GetTypeQuestionOptions = (from a in _context.Rating_Type_QuestionsDMO
                    //                               from b in _context.Rating_Type_Questions_DepartmentDMO
                    //                               from c in _context.Rating_Master_TypeDMO
                    //                               from d in _context.Ratings_Master_QuestionsDMO
                    //                               from e in _context.Rating_Type_Questions_OptionsDMO
                    //                               from f in _context.Rating_Master_OptionsDMO
                    //                               where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                    //                               && a.RATMTQ_Id == e.RATMTQ_Id && e.RATMOP_Id == f.RATMOP_Id && a.RATMTQ_ActiveFlag == true
                    //                               && a.MI_Id == data.MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id && d.RATMQE_AutomaticFlg == false
                    //                               && cateid.Contains(a.RATMT_Id))
                    //                               select new Rating_TransactionDTO
                    //                               {
                    //                                   RATMOP_OptionsValue = f.RATMOP_OptionsValue,
                    //                                   RATMOP_RatingOptions = f.RATMOP_RatingOptions,
                    //                                   RATMOP_ROOrder = f.RATMOP_ROOrder,
                    //                                   RATMOP_Id = f.RATMOP_Id,
                    //                                   RATMQE_Id = a.RATMQE_Id,
                    //                                   RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg

                    //                               }).Distinct().OrderByDescending(a => a.RATMOP_OptionsValue).ToArray();
                }

                var GetTypesDetails = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_Id == data.RATMT_Id).ToList();

                var GetPeriodicity = GetTypesDetails.FirstOrDefault().RATMT_PeriodicityFlg;

                if (GetPeriodicity == "Monthly")
                {
                    data.GetSavedDetails = _context.Rating_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id
                    && cateid.Contains(a.RATMT_Id) && a.RATTR_FeedbackDate.Value.Month == data.RATTR_FeedbackDate.Value.Month
                    && a.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year).ToArray();
                }
                else if (GetPeriodicity == "Yearly")
                {
                    data.GetSavedDetails = _context.Rating_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id
                    && cateid.Contains(a.RATMT_Id) && a.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Rating TaskRole Category Fetch EmpoloyeeTask Category Wise

        public Rating_TransactionDTO viewemployeetaskcategory(Rating_TransactionDTO data)
        {
            try
            {
                var taskcategoryflag = _context.ISM_Master_TaskCategory_DMO.Single(a => a.ISMMTCAT_Id == data.ISMMTCAT_Id && a.MI_Id == data.MI_Id);

                data.getRatingQuestion = (from a in _context.Ratings_Master_QuestionsDMO
                                          from b in _context.Rating_Master_Question_SubClassificationDMO
                                          where (a.RATMQECLSUB_Id == b.RATMQECLSUB_Id && a.MI_Id == data.MI_Id && a.RATMQE_QuestionFlg == taskcategoryflag.ISMMTCAT_TaskCategoryName)
                                          select new Rating_TransactionDTO
                                          {
                                              RATMQE_Id = a.RATMQE_Id,
                                              RATMQE_RatingQuestions = a.RATMQE_RatingQuestions,
                                              RATMQECLSUB_Id = b.RATMQECLSUB_Id,
                                              RATMQECLSUB_QuestionsSubclassification = b.RATMQECLSUB_QuestionsSubclassification
                                          }).ToArray();

                data.taskcategoryquestionoptionlist = (from a in _context.Ratings_Master_QuestionsDMO
                                                       from b in _context.Rating_Type_QuestionsDMO
                                                       from c in _context.Rating_Type_Questions_OptionsDMO
                                                       from d in _context.Rating_Master_OptionsDMO
                                                       where (a.RATMQE_Id == b.RATMQE_Id && b.RATMTQ_Id == c.RATMTQ_Id && a.MI_Id == data.MI_Id && c.RATMOP_Id == d.RATMOP_Id
                                                       && d.MI_Id == data.MI_Id && a.RATMQE_QuestionFlg == taskcategoryflag.ISMMTCAT_TaskCategoryName)
                                                       select new Rating_TransactionDTO
                                                       {
                                                           RATMOP_OptionsValue = d.RATMOP_OptionsValue,
                                                           RATMOP_Id = d.RATMOP_Id,
                                                           RATMOP_RatingOptions = d.RATMOP_RatingOptions,
                                                           RATMQE_Id = a.RATMQE_Id,
                                                           RATMQE_RatingQuestions = a.RATMQE_RatingQuestions

                                                       }).Distinct().OrderByDescending(a => a.RATMOP_OptionsValue).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Rating_ProxyRoleAndEnhancement_FetchEmployeeTasks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMMTCAT_Id", SqlDbType.BigInt) { Value = data.ISMMTCAT_Id });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.ratingRoleCategoryEmployeerTasks = retObject.ToArray();
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

        public Rating_TransactionDTO SaveRatingTaskCategory(Rating_TransactionDTO data)
        {
            try
            {

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.Save_Rating_TaskCategory != null && data.Save_Rating_TaskCategory.Length > 0)
                {
                    foreach (var savedata in data.Save_Rating_TaskCategory)
                    {
                        Rating_TaskCategory_TransactionDMO Rating_TaskCategory = new Rating_TaskCategory_TransactionDMO();
                        Rating_TaskCategory.ISMTCR_Id = savedata.ISMTCR_Id;
                        Rating_TaskCategory.RATMQE_Id = data.RATMQE_Id;
                        Rating_TaskCategory.HRME_Id = savedata.HRME_Id;
                        Rating_TaskCategory.RATTCT_Value = savedata.RATMOP_OptionsValue;
                        Rating_TaskCategory.RATTCT_Flag = savedata.ISMMTCAT_TaskCategoryName;
                        Rating_TaskCategory.RATTCT_Feedbackdate = data.RATTR_FeedbackDate;
                        Rating_TaskCategory.RATTCT_Feedback_UserId = data.UserId;
                        Rating_TaskCategory.RATTCT_ActiveFlag = true;
                        Rating_TaskCategory.RATTCT_CreatedBy = data.UserId;
                        Rating_TaskCategory.RATTCT_CreatedDate = indiantime0;
                        Rating_TaskCategory.RATTCT_UpdatedBy = data.UserId;
                        Rating_TaskCategory.RATTCT_UpdatedDate = indiantime0;
                        _context.Add(Rating_TaskCategory);
                    }
                    var i = _context.SaveChanges();
                    if (i > 0)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_TransactionDTO viewautomaticrating(Rating_TransactionDTO data)
        {
            try
            {
                var miid = _context.MasterEmployee.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();
                var user_id = _context.Staff_User_Login.Where(s => s.Emp_Code == data.HRME_Id).Select(s => s.Id).FirstOrDefault();
                var monthid = data.RATTR_FeedbackDate.Value.Month;
                var yaerid = data.RATTR_FeedbackDate.Value.Year;

                if (data.RATMQE_RatingsQRemarks == "LIEO")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Employee_lateinearlyout_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@month",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@year",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid",
             SqlDbType.VarChar)
                        {
                            Value = miid
                        });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
             SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_lieo = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
                else if (data.RATMQE_RatingsQRemarks == "UNLEAVE")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Unauthorizedleave_report_Rating";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
           SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@month",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@year",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
             SqlDbType.Bit)
                        {
                            Value = 1
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_ul = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "DressCode")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Rating_Dresscode_Caluclation";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                        {
                            Value = miid
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "Appearance")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Rating_Appearance_Caluclation";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                        {
                            Value = miid
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "Behaviour")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Rating_Behaviour_Caluclation";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                        {
                            Value = miid
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "Notice")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_NOTICEBOARD_COUNT_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
                SqlDbType.VarChar)
                        {
                            Value = yaerid
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                        {
                            Value = miid
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
                else if (data.RATMQE_RatingsQRemarks == "Holiday")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Employee_HolidayworkingDaycounts";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                        {
                            Value = miid
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
                SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "NonEarned")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmployeeEarnedandNonearned";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
                SqlDbType.VarChar)
                        {
                            Value = yaerid
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "Deviate")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_MonthlyDeviation_Rating";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
           SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@month",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@year",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
                else if (data.RATMQE_RatingsQRemarks == "Deviation")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_MonthlyDeviationFor_IndividualandMakerChecker_Rating";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@userid",
           SqlDbType.VarChar)
                        {
                            Value = user_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
                else if (data.RATMQE_RatingsQRemarks == "Reopen")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_REOPEN_TASK_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
           SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "RejectedCount")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Rejected_Taskcount_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
           SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
              SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
                else if (data.RATMQE_RatingsQRemarks == "DRExtension")                {                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())                    {                        cmd.CommandText = "ISM_DRExtension_Employeewise_countlist";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@HRME_id",                        SqlDbType.VarChar)                        {                            Value = data.HRME_Id                        });                        cmd.Parameters.Add(new SqlParameter("@MONTHID",                SqlDbType.VarChar)                        {                            Value = monthid                        });                        cmd.Parameters.Add(new SqlParameter("@YEAR",                SqlDbType.VarChar)                        {                            Value = yaerid                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        var retObject = new List<dynamic>();                        try                        {                            using (var dataReader = cmd.ExecuteReader())                            {                                while (dataReader.Read())                                {                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                    {                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);                                    }                                    retObject.Add((ExpandoObject)dataRow);                                }                            }                            data.autoratingdetail_deviation = retObject.ToArray();                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }                else if (data.RATMQE_RatingsQRemarks == "Planner")                {                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())                    {                        cmd.CommandText = "ISM_PlannerExtension_EmployeeWiseCount";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@HRME_id",                        SqlDbType.VarChar)                        {                            Value = data.HRME_Id                        });                        cmd.Parameters.Add(new SqlParameter("@MONTHID",                SqlDbType.VarChar)                        {                            Value = monthid                        });                        cmd.Parameters.Add(new SqlParameter("@YEAR",                SqlDbType.VarChar)                        {                            Value = yaerid                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        var retObject = new List<dynamic>();                        try                        {                            using (var dataReader = cmd.ExecuteReader())                            {                                while (dataReader.Read())                                {                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                    {                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);                                    }                                    retObject.Add((ExpandoObject)dataRow);                                }                            }                            data.autoratingdetail_deviation = retObject.ToArray();                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }


                else if (data.RATMQE_RatingsQRemarks == "DRExtension")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_DRExtension_Employeewise_countlist";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_id",
                        SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
                SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "Planner")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_PlannerExtension_EmployeeWiseCount";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_id",
                        SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID",
                SqlDbType.VarChar)
                        {
                            Value = monthid
                        });
                        cmd.Parameters.Add(new SqlParameter("@YEAR",
                SqlDbType.VarChar)
                        {
                            Value = yaerid
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "OVERALLRATING")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Employee_CorporateHiring_OverallRating";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = miid });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt) { Value = data.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@Day", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Day });
                        cmd.Parameters.Add(new SqlParameter("@MONTHID", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Month });
                        cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Year });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "Enhancement")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RATING_TASKCATEGORY_ENHANCEMENT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MONTHID", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Month });
                        cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Year });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt) { Value = data.HRME_Id });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if (data.RATMQE_RatingsQRemarks == "Proxy Role")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RATING_TASKCATEGORY_PROXYROLE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MONTHID", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Month });
                        cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.VarChar) { Value = data.RATTR_FeedbackDate.Value.Year });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt) { Value = data.HRME_Id });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.autoratingdetail_deviation = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_TransactionDTO SaveRating(Rating_TransactionDTO data)
        {
            try
            {
                var autoquestion_count = 0;
                var manualquestion_count = 0;
                var autorating_count = 0;
                var manualrating_count = 0;
                var Totalquestion_count = 0;
                decimal sumofrating = 0;



                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                var EmployeeDetails = _context.MasterEmployee.Where(a => a.HRME_Id == data.HRME_Id).ToList();
                var HRMD_Id = EmployeeDetails.FirstOrDefault().HRMD_Id;
                var HRMDES_Id = EmployeeDetails.FirstOrDefault().HRMDES_Id;
                data.MI_Id = EmployeeDetails.FirstOrDefault().MI_Id;

                if (data.Save_AutoRating_Details != null && data.Save_AutoRating_Details.Length > 0)
                {

                    foreach (var saveauto in data.Save_AutoRating_Details)
                    {
                        foreach (var queclasdata in saveauto.questionclassificationdata)
                        {
                            foreach (var quesubclasdata in queclasdata.questionsubclassificationdata)
                            {
                                foreach (var sar in quesubclasdata.questionlistdata)
                                {
                                    //autoquestion_count = autoquestion_count + 1;

                                    //autorating_count = autorating_count + sar.RATMOP_OptionsValue;
                                    var autoquestioncount = _context.Rating_TransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.RATMT_Id == sar.RATMT_Id &&
                                    t.RATMQE_Id == sar.RATMQE_Id && t.RATTR_FeedbackDate.Value.Month == data.RATTR_FeedbackDate.Value.Month && t.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year).ToList();

                                    if (autoquestioncount.Count == 0)
                                    {

                                        Rating_TransactionDMO savetransaction = new Rating_TransactionDMO();
                                        savetransaction.MI_Id = data.MI_Id;
                                        savetransaction.HRME_Id = data.HRME_Id;
                                        savetransaction.RATTR_FeedbackDate = data.RATTR_FeedbackDate;
                                        savetransaction.RATMT_Id = sar.RATMT_Id;
                                        savetransaction.RATMQE_Id = sar.RATMQE_Id;
                                        savetransaction.RATMOP_Id = sar.RATMOP_Id;
                                        savetransaction.RATTR_Feedback_UserId = data.UserId;
                                        savetransaction.RATTR_ActiveFlag = true;
                                        savetransaction.RATTR_CreatedBy = data.UserId;
                                        savetransaction.RATTR_CreatedDate = DateTime.Now;
                                        savetransaction.RATTR_UpdatedBy = data.UserId;
                                        savetransaction.RATTR_UpdatedDate = DateTime.Now;
                                        _context.Add(savetransaction);
                                    }
                                }
                            }

                        }
                    }
                    var j = _context.SaveChanges();
                    if (j > 0)
                    {
                        data.returnval = true;
                    }

                }

                if (data.Save_Rating_Details != null && data.Save_Rating_Details.Length > 0)
                {

                    foreach (var e in data.Save_Rating_Details)
                    {
                        foreach (var classlist in e.classificationlist)
                        {
                            foreach (var subclasslist in classlist.subclassificationlist)
                            {

                                foreach (var c in subclasslist.questionlist)
                                {
                                    if (c.RATTR_Id > 0)
                                    {
                                        var Result = _context.Rating_TransactionDMO.Single(a => a.RATTR_Id == c.RATTR_Id);
                                        if (c.RATMQE_ManualEntryFlg)
                                        {
                                            Result.RATTR_FeedBack = c.name;
                                        }
                                        else
                                        {
                                            Result.RATMOP_Id = Convert.ToInt64(c.name);
                                        }
                                        Result.RATTR_UpdatedBy = data.UserId;
                                        Result.RATTR_UpdatedDate = DateTime.Now;
                                        _context.Update(Result);
                                    }
                                    else
                                    {
                                        //manualquestion_count = manualquestion_count + 1;

                                        //var optionvalue = _context.Rating_Master_OptionsDMO.Where(t => t.RATMOP_Id == Convert.ToInt64(c.name) && t.RATMOP_ActiveFlag == true).Select(t => t.RATMOP_OptionsValue).FirstOrDefault();

                                        //manualrating_count = manualrating_count + Convert.ToInt32(optionvalue);


                                        Rating_TransactionDMO rating_TransactionDMO = new Rating_TransactionDMO();
                                        rating_TransactionDMO.MI_Id = data.MI_Id;
                                        rating_TransactionDMO.HRME_Id = data.HRME_Id;
                                        rating_TransactionDMO.RATTR_FeedbackDate = data.RATTR_FeedbackDate;
                                        rating_TransactionDMO.RATMT_Id = e.RATMT_Id;
                                        rating_TransactionDMO.RATMQE_Id = c.RATMQE_Id;
                                        if (c.RATMQE_ManualEntryFlg)
                                        {
                                            rating_TransactionDMO.RATTR_FeedBack = c.name;
                                        }
                                        else
                                        {
                                            rating_TransactionDMO.RATMOP_Id = Convert.ToInt64(c.name);
                                        }

                                        rating_TransactionDMO.RATTR_Feedback_UserId = data.UserId;
                                        rating_TransactionDMO.RATTR_ActiveFlag = true;
                                        rating_TransactionDMO.RATTR_CreatedBy = data.UserId;
                                        rating_TransactionDMO.RATTR_CreatedDate = DateTime.Now;
                                        rating_TransactionDMO.RATTR_UpdatedBy = data.UserId;
                                        rating_TransactionDMO.RATTR_UpdatedDate = DateTime.Now;
                                        _context.Add(rating_TransactionDMO);
                                    }
                                }
                            }
                        }
                    }


                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                }

                //var overallrating = (from a in _context.Rating_Type_QuestionsDMO
                //                     from b in _context.Ratings_Master_QuestionsDMO
                //                     where (a.RATMQE_Id == b.RATMQE_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.RATMQE_RatingsQRemarks == "OVERALLRATING")
                //                     select new Rating_TransactionDTO
                //                     {
                //                         RATMT_Id = a.RATMT_Id,
                //                         RATMQE_Id = a.RATMQE_Id,
                //                         RATMQE_ManualEntryFlg = b.RATMQE_ManualEntryFlg,
                //                     }).ToList();

                //Totalquestion_count = autoquestion_count + manualquestion_count;
                //sumofrating = autorating_count + manualrating_count;

                //decimal averagerating = (sumofrating / Totalquestion_count);

                //var roundavgrating = Math.Round(averagerating);

                //var Rating_OptionId = _context.Rating_Master_OptionsDMO.Where(t => t.RATMOP_OptionsValue == roundavgrating && t.MI_Id == data.MI_Id).Select(t => t.RATMOP_Id).FirstOrDefault();

                //if (overallrating.Count > 0)
                //{
                //    Rating_TransactionDMO overallrating_TransactionDMO = new Rating_TransactionDMO();
                //    overallrating_TransactionDMO.MI_Id = data.MI_Id;
                //    overallrating_TransactionDMO.HRME_Id = data.HRME_Id;
                //    overallrating_TransactionDMO.RATTR_FeedbackDate = data.RATTR_FeedbackDate;
                //    overallrating_TransactionDMO.RATMT_Id = overallrating[0].RATMT_Id;
                //    overallrating_TransactionDMO.RATMQE_Id = overallrating[0].RATMQE_Id;
                //    overallrating_TransactionDMO.RATMOP_Id = Rating_OptionId;
                //    overallrating_TransactionDMO.RATTR_Feedback_UserId = data.UserId;
                //    overallrating_TransactionDMO.RATTR_ActiveFlag = true;
                //    overallrating_TransactionDMO.RATTR_CreatedBy = data.UserId;
                //    overallrating_TransactionDMO.RATTR_CreatedDate = DateTime.Now;
                //    overallrating_TransactionDMO.RATTR_UpdatedBy = data.UserId;
                //    overallrating_TransactionDMO.RATTR_UpdatedDate = DateTime.Now;
                //    _context.Add(overallrating_TransactionDMO);
                //}
                //var k = _context.SaveChanges();
                //if (k > 0)
                //{
                //    data.returnval = true;
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_TransactionDTO viewratinglist(Rating_TransactionDTO data)
        {
            try
            {
                // var feedbackdate = data.RATTR_FeedbackDate.Value.Date.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VIEWEMPLOYEERATING";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@RATMT_Id", SqlDbType.BigInt)
                    {
                        Value = data.RATMT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@RATTR_FeedbackDate", SqlDbType.Date)
                    {
                        Value = data.RATTR_FeedbackDate
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
                        data.viewEmployeeRating = retObject.ToArray();
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


        //consolidated_report
        public async Task<Rating_TransactionDTO> get_Consolidated_details(Rating_TransactionDTO data)
        {
            try
            {
                var rolet = _context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }

                var hrmEempid = _context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                long? dept_id = 0;
                long? design_id = 0;
                var hrmeDepartment_id = _context.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

                    data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                }

                var hrmeDepartment = _context.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == dept_id && t.HRMD_ActiveFlag == true).Distinct().ToList();
                if (hrmeDepartment.Count > 0)
                {
                    data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                }

                data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                List<AcademicYear> year = new List<AcademicYear>();
                year = _context.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == data.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = year.ToArray();


                data.Month_array = _context.IVRM_Month_DMO.Where(R => R.Is_Active == true).Distinct().ToArray();

                //data.ratingtype = _context.Rating_Master_TypeDMO.Where(R => R.MI_Id == data.MI_Id &&
                //R.RATMT_ActiveFlag == true).Distinct().ToArray();

                data.ratingtype = (from a in _context.Rating_Master_TypeDMO
                                   from b in _context.Institution
                                   where (a.MI_Id == b.MI_Id && a.RATMT_ActiveFlag == true && b.MI_ActiveFlag == 1)
                                   select new Rating_TransactionDTO
                                   {
                                       RATMT_Id = a.RATMT_Id,
                                       RATMT_RatingTypeName = a.RATMT_RatingTypeName,
                                       MI_Id = a.MI_Id,
                                       MI_Name = b.MI_Subdomain,

                                   }).OrderBy(a => a.RATMT_Id).ToArray();

                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_admin";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Department List ********************************// 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentList_Rating";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
SqlDbType.BigInt)
                    {
                        Value = dept_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
SqlDbType.BigInt)
                    {
                        Value = empid
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
                        data.DepartmentList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Designation List ********************************// 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DesignationList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                        data.Designation = retObject.ToArray();
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
        public async Task<Rating_TransactionDTO> getconsolidatedreport(Rating_TransactionDTO data)
        {

            try
            {

                string months_select = "";
                string months_pvt = "";
                string month_s = "";

                string hrme = "0";
                if (data.arrayuserEmp.Length > 0 && data.arrayuserEmp != null)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrme = hrme + "," + ue.HRME_Id;
                    }
                }
                string hrmd = "0";
                if (data.departmentlist.Length > 0 && data.departmentlist != null)
                {
                    foreach (var ue in data.departmentlist)
                    {
                        hrmd = hrmd + "," + ue.HRMDC_ID;
                    }
                }
                string hrmdes = "0";
                if (data.arrayuserdes.Length > 0 && data.arrayuserdes != null)
                {
                    foreach (var ue in data.arrayuserdes)
                    {
                        hrmdes = hrmdes + "," + ue.HRMDES_Id;
                    }
                }
                string month = "0";
                if (data.monthList.Length > 0 && data.monthList != null)
                {
                    foreach (var ue in data.monthList)
                    {
                        month = month + "," + ue.IVRM_Month_Id;

                        months_select += "isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([" + ue.IVRM_Month_Name + "],0))))/NULLIF(COUNT([" + ue.IVRM_Month_Name + "]),0),0) as int),0)+";
                        months_pvt += "[" + ue.IVRM_Month_Name + "],";
                        month_s += "CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([" + ue.IVRM_Month_Name + "],null))))/NULLIF(COUNT([" + ue.IVRM_Month_Name + "]),''),0) as int)";
                        month_s += ue.IVRM_Month_Name + ",";
                        //  month_s += "isnull([" + ue.IVRM_Month_Name + "],0)" + ue.IVRM_Month_Name+",";
                    }
                }
                if (months_select.EndsWith('+'))
                {
                    months_select = months_select.Trim('+');
                }
                if (months_pvt.EndsWith(','))
                {
                    months_pvt = months_pvt.Trim(',');
                }
                if (month_s.EndsWith(','))
                {
                    month_s = month_s.Trim(',');
                }

                string RATMT_Id = "0";
                if (data.ratingtypelist.Length > 0 && data.ratingtypelist != null)
                {
                    foreach (var ue in data.ratingtypelist)
                    {
                        RATMT_Id = RATMT_Id + "," + ue.RATMT_Id;
                    }
                }


                if (data.flag == "all")
                {
                    months_select = "isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([January],0))))/NULLIF(COUNT([January]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([February],0))))/NULLIF(COUNT([February]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([March],0))))/NULLIF(COUNT([March]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([April],0))))/NULLIF(COUNT([April]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([May],0))))/NULLIF(COUNT([May]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([June],0))))/NULLIF(COUNT([June]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([July],0))))/NULLIF(COUNT([July]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([August],0))))/NULLIF(COUNT([August]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([September],0))))/NULLIF(COUNT([September]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([October],0))))/NULLIF(COUNT([October]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([November],0))))/NULLIF(COUNT([November]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([December],0))))/NULLIF(COUNT([December]),0),0) as int),0)";
                    months_pvt = "[January],[February],[March],[April],[May],[June],[July],[August],[September],[October],[November],[December]";
                    month = "0,1,2,3,4,5,6,7,8,9,10,11,12";
                    month_s = "CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([January],null))))/NULLIF(COUNT([January]),''),0) as int)January,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([February],null))))/NULLIF(COUNT([February]),''),0) as int)February,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([March],null))))/NULLIF(COUNT([March]),''),0) as int)March,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([April],null))))/NULLIF(COUNT([April]),''),0) as int)April,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([May],null))))/NULLIF(COUNT([May]),''),0) as int)May,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([June],null))))/NULLIF(COUNT([June]),''),0) as int)June,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([July],null))))/NULLIF(COUNT([July]),''),0) as int)July,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([August],null))))/NULLIF(COUNT([August]),''),0) as int)August,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([September],null))))/NULLIF(COUNT([September]),''),0) as int)September,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([October],null))))/NULLIF(COUNT([October]),''),0) as int)October,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([November],null))))/NULLIF(COUNT([November]),''),0) as int)November,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([December],null))))/NULLIF(COUNT([November]),''),0) as int)December";
                }




                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "EmployeeWiseConsolidatedRating_Report";
                    cmd.CommandText = "EmployeeWiseConsolidatedRating_Report_test";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.VarChar)
                    {
                        Value = hrme
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmd
                        // Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
          SqlDbType.VarChar)
                    {
                        //  Value = data.@HRMDES_Id
                        Value = hrmdes
                        //Value = 32
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
      SqlDbType.VarChar)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthSelect",
    SqlDbType.VarChar)
                    {
                        Value = months_select
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthPvt",
  SqlDbType.VarChar)
                    {
                        Value = months_pvt
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
SqlDbType.VarChar)
                    {
                        Value = month
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthS",
SqlDbType.VarChar)
                    {
                        Value = month_s
                    });
                    cmd.Parameters.Add(new SqlParameter("@RATMT_Id",
SqlDbType.VarChar)
                    {
                        Value = RATMT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
SqlDbType.VarChar)
                    {
                        Value = data.flag
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
                        data.consolidateData = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                //            if (data.consolidateData.Length > 0)
                //            {
                //                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //                {
                //                    cmd.CommandText = "EmployeeWiseConsolidatedRating_Report_uniqueids";
                //                    cmd.CommandType = CommandType.StoredProcedure;
                //                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //                    SqlDbType.VarChar)
                //                    {
                //                        Value = data.MI_Id
                //                    });
                //                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                //               SqlDbType.VarChar)
                //                    {
                //                        Value = hrme
                //                    });
                //                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                //             SqlDbType.VarChar)
                //                    {
                //                        Value = hrmd

                //                    });
                //                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                //          SqlDbType.VarChar)
                //                    {

                //                        Value = hrmdes

                //                    });
                //                    cmd.Parameters.Add(new SqlParameter("@year",
                //      SqlDbType.VarChar)
                //                    {
                //                        Value = data.year
                //                    });
                //                    cmd.Parameters.Add(new SqlParameter("@monthSelect",
                //    SqlDbType.VarChar)
                //                    {
                //                        Value = months_select
                //                    });
                //                    cmd.Parameters.Add(new SqlParameter("@monthPvt",
                //  SqlDbType.VarChar)
                //                    {
                //                        Value = months_pvt
                //                    });
                //                    cmd.Parameters.Add(new SqlParameter("@month",
                //SqlDbType.VarChar)
                //                    {
                //                        Value = month
                //                    });
                //                    cmd.Parameters.Add(new SqlParameter("@monthS",
                //SqlDbType.VarChar)
                //                    {
                //                        Value = month_s
                //                    });
                //                    if (cmd.Connection.State != ConnectionState.Open)
                //                        cmd.Connection.Open();

                //                    var retObject = new List<dynamic>();
                //                    try
                //                    {
                //                        using (var dataReader = await cmd.ExecuteReaderAsync())
                //                        {
                //                            while (await dataReader.ReadAsync())
                //                            {
                //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                                {
                //                                    dataRow.Add(
                //                                        dataReader.GetName(iFiled),
                //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                                    );
                //                                }
                //                                retObject.Add((ExpandoObject)dataRow);
                //                            }
                //                        }
                //                        data.consolidateData_ids = retObject.ToArray();
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        Console.WriteLine(ex.Message);
                //                    }

                //                }
                //            }
                if (data.consolidateData.Length > 0)
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
                Console.WriteLine(ex.Message);
            }


            return data;
        }


        public async Task<Rating_TransactionDTO> getemployeereport(Rating_TransactionDTO data)
        {

            try            {                var hrme_id = _context.Staff_User_Login.Where(s => s.Id == data.UserId).Select(d => d.Emp_Code).FirstOrDefault();                var empdetail = _context.MasterEmployee.Where(t => t.HRME_Id == hrme_id).ToArray();                var hrmdc_id = _context.HR_Master_Department.Where(d => d.HRMD_Id == empdetail[0].HRMD_Id).Select(d => d.HRMDC_ID).Single();                string months_select = "";                string months_pvt = "";                string month_s = "";


                string month = "0";
                if (data.monthList.Length > 0 && data.monthList != null)
                {
                    foreach (var ue in data.monthList)
                    {
                        month = month + "," + ue.IVRM_Month_Id;


                        months_select += "isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([" + ue.IVRM_Month_Name + "],0))))/NULLIF(COUNT([" + ue.IVRM_Month_Name + "]),0),0) as int),0)+";
                        months_pvt += "[" + ue.IVRM_Month_Name + "],";
                        month_s += "CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([" + ue.IVRM_Month_Name + "],null))))/NULLIF(COUNT([" + ue.IVRM_Month_Name + "]),''),0) as int)";
                        month_s += ue.IVRM_Month_Name + ",";
                        //  month_s += "isnull([" + ue.IVRM_Month_Name + "],0)" + ue.IVRM_Month_Name+",";
                    }
                }
                if (months_select.EndsWith('+'))
                {
                    months_select = months_select.Trim('+');
                }
                if (months_pvt.EndsWith(','))
                {
                    months_pvt = months_pvt.Trim(',');
                }
                if (month_s.EndsWith(','))
                {
                    month_s = month_s.Trim(',');
                }


                if (data.flag == "all")
                {
                    months_select = "isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([January],0))))/NULLIF(COUNT([January]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([February],0))))/NULLIF(COUNT([February]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([March],0))))/NULLIF(COUNT([March]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([April],0))))/NULLIF(COUNT([April]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([May],0))))/NULLIF(COUNT([May]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([June],0))))/NULLIF(COUNT([June]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([July],0))))/NULLIF(COUNT([July]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([August],0))))/NULLIF(COUNT([August]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([September],0))))/NULLIF(COUNT([September]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([October],0))))/NULLIF(COUNT([October]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([November],0))))/NULLIF(COUNT([November]),0),0) as int),0)+isnull(CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([December],0))))/NULLIF(COUNT([December]),0),0) as int),0)";
                    months_pvt = "[January],[February],[March],[April],[May],[June],[July],[August],[September],[October],[November],[December]";
                    month = "0,1,2,3,4,5,6,7,8,9,10,11,12";
                    month_s = "CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([January],null))))/NULLIF(COUNT([January]),''),0) as int)January,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([February],null))))/NULLIF(COUNT([February]),''),0) as int)February,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([March],null))))/NULLIF(COUNT([March]),''),0) as int)March,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([April],null))))/NULLIF(COUNT([April]),''),0) as int)April,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([May],null))))/NULLIF(COUNT([May]),''),0) as int)May,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([June],null))))/NULLIF(COUNT([June]),''),0) as int)June,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([July],null))))/NULLIF(COUNT([July]),''),0) as int)July,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([August],null))))/NULLIF(COUNT([August]),''),0) as int)August,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([September],null))))/NULLIF(COUNT([September]),''),0) as int)September,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([October],null))))/NULLIF(COUNT([October]),''),0) as int)October,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([November],null))))/NULLIF(COUNT([November]),''),0) as int)November,CAST(ROUND(CONVERT(decimal(4,2),(sum(isnull([December],null))))/NULLIF(COUNT([November]),''),0) as int)December";
                }


                var Rating_Type = _context.Rating_Master_TypeDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();
                string RATMT_Ids = "0";
                if (Rating_Type.Length > 0 && Rating_Type != null)
                {
                    foreach (var ue in Rating_Type)
                    {
                        RATMT_Ids = RATMT_Ids + "," + ue.RATMT_Id;
                    }
                }                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeMonthWiseRating_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.VarChar)
                    {
                        Value = hrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmdc_id

                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
          SqlDbType.VarChar)
                    {

                        Value = empdetail[0].HRMDES_Id

                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
      SqlDbType.VarChar)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthSelect",
    SqlDbType.VarChar)
                    {
                        Value = months_select
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthPvt",
  SqlDbType.VarChar)
                    {
                        Value = months_pvt
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
SqlDbType.VarChar)
                    {
                        Value = month
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthS",
SqlDbType.VarChar)
                    {
                        Value = month_s
                    });
                    cmd.Parameters.Add(new SqlParameter("@RATMT_Id",
SqlDbType.VarChar)
                    {
                        Value = RATMT_Ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
SqlDbType.VarChar)
                    {
                        Value = data.flag
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
                        data.consolidateData = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                if (data.consolidateData.Length > 0)                {                    data.returnval = true;                }                else                {                    data.returnval = false;                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }


            return data;
        }



        public Rating_TransactionDTO SaveOverAllRating(Rating_TransactionDTO data)
        {
            try
            {

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                if (data.RATMTQR_Id > 0)
                {
                    data.returnmsg = "Update";
                    data.returnval = false;

                    var Result = _context.Rating_Type_Questions_RatingDMO.Single(a => a.RATMTQR_Id == data.RATMTQR_Id);
                    Result.RATMTQ_Id = data.RATMTQ_Id;
                    Result.RATMT_Id = data.RATMT_Id;
                    Result.RATMQE_Id = data.RATMQE_Id;
                    Result.RATMTQR_FromValue = data.RATMTQR_FromValue;
                    Result.RATMTQR_ToValue = data.RATMTQR_ToValue;
                    Result.RATMTQR_Rating = data.RATMTQR_Rating;
                    Result.RATMTQR_UpdatedBy = data.RATMTQR_UpdatedBy;
                    Result.RATMTQR_UpdatedDate = indiantime0;
                    _context.Update(Result);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }

                }
                else
                {
                    data.returnmsg = "Add";


                    Rating_Type_Questions_RatingDMO Rating_Type_Questions_RatingDMO = new Rating_Type_Questions_RatingDMO();
                    Rating_Type_Questions_RatingDMO.RATMTQ_Id = data.RATMTQ_Id;
                    Rating_Type_Questions_RatingDMO.RATMT_Id = data.RATMT_Id;
                    Rating_Type_Questions_RatingDMO.RATMQE_Id = data.RATMQE_Id;
                    Rating_Type_Questions_RatingDMO.RATMTQR_FromValue = data.RATMTQR_FromValue;
                    Rating_Type_Questions_RatingDMO.RATMTQR_ToValue = data.RATMTQR_ToValue;
                    Rating_Type_Questions_RatingDMO.RATMTQR_Rating = data.RATMTQR_Rating;
                    Rating_Type_Questions_RatingDMO.RATMTQR_ActiveFlag = true;
                    Rating_Type_Questions_RatingDMO.RATMTQR_CreatedBy = data.UserId;
                    Rating_Type_Questions_RatingDMO.RATMTQR_CreatedDate = indiantime0;
                    Rating_Type_Questions_RatingDMO.RATMTQR_UpdatedBy = data.UserId;
                    Rating_Type_Questions_RatingDMO.RATMTQR_UpdatedDate = indiantime0;
                    _context.Add(Rating_Type_Questions_RatingDMO);
                    var i = _context.SaveChanges();
                    if (i > 0)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_TransactionDTO OverAllEdit(Rating_TransactionDTO data)
        {
            try
            {

                data.EditDataOverAll = _context.Rating_Type_Questions_RatingDMO.Where(a => a.RATMTQR_Id == data.RATMTQR_Id).ToArray();

                data.getRatingType = (from a in _context.Rating_Type_QuestionsDMO
                                      from b in _context.Rating_Master_TypeDMO
                                      where (a.RATMT_Id == b.RATMT_Id && b.RATMT_LInkedToRelievingFlg == true && a.MI_Id == data.MI_Id
                                              && b.MI_Id == data.MI_Id)
                                      select new Rating_TransactionDTO
                                      {
                                          RATMT_Id = b.RATMT_Id,
                                          typeNameList = b.RATMT_RatingTypeName,
                                      }).Distinct().ToArray();

                data.getRatingQuestion = (from a in _context.Rating_Type_QuestionsDMO
                                          from b in _context.Ratings_Master_QuestionsDMO
                                          where (a.RATMQE_Id == b.RATMQE_Id && a.MI_Id == data.MI_Id
                                              && b.MI_Id == data.MI_Id
                                              && a.RATMT_Id == data.RATMT_Id)
                                          select new Rating_TransactionDTO
                                          {
                                              RATMQE_Id = b.RATMQE_Id,
                                              questionNameList = b.RATMQE_RatingQuestions,
                                          }).Distinct().ToArray();

                data.getRatingTypeQuestion = (from a in _context.Rating_Type_QuestionsDMO
                                              from b in _context.Rating_Master_TypeDMO
                                              from c in _context.Ratings_Master_QuestionsDMO
                                              where (a.RATMT_Id == b.RATMT_Id && a.RATMQE_Id == c.RATMQE_Id
                                              && b.RATMT_LInkedToRelievingFlg == true && a.MI_Id == data.MI_Id
                                              && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                              && a.RATMT_Id == data.RATMT_Id && a.RATMQE_Id == data.RATMQE_Id)
                                              select new Rating_TransactionDTO
                                              {
                                                  RATMTQ_Id = a.RATMTQ_Id,
                                                  typeNameList = b.RATMT_RatingTypeName,
                                                  questionNameList = c.RATMQE_RatingQuestions,
                                                  RATMT_Id = b.RATMT_Id,
                                                  RATMQE_Id = c.RATMQE_Id
                                              }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_TransactionDTO OverAllRatingActiveDeactive(Rating_TransactionDTO data)
        {
            try
            {
                data.returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Result = _context.Rating_Type_Questions_RatingDMO.Single(a => a.RATMTQR_Id == data.RATMTQR_Id);
                Result.RATMTQR_ActiveFlag = Result.RATMTQR_ActiveFlag == true ? false : true;
                Result.RATMTQR_UpdatedBy = data.UserId;
                Result.RATMTQR_UpdatedDate = indiantime0;
                _context.Update(Result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //getQuestions
        public Rating_TransactionDTO getQuestions(Rating_TransactionDTO data)
        {
            try
            {

                data.getRatingQuestion = (from a in _context.Rating_Type_QuestionsDMO
                                          from b in _context.Ratings_Master_QuestionsDMO
                                          where (a.RATMQE_Id == b.RATMQE_Id && a.MI_Id == data.MI_Id
                                              && b.MI_Id == data.MI_Id
                                              && a.RATMT_Id == data.RATMT_Id)
                                          select new Rating_TransactionDTO
                                          {
                                              RATMQE_Id = b.RATMQE_Id,
                                              questionNameList = b.RATMQE_RatingQuestions,
                                          }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //getTypeQuestions
        public Rating_TransactionDTO getTypeQuestions(Rating_TransactionDTO data)
        {
            try
            {

                data.getRatingTypeQuestion = (from a in _context.Rating_Type_QuestionsDMO
                                              from b in _context.Rating_Master_TypeDMO
                                              from c in _context.Ratings_Master_QuestionsDMO
                                              where (a.RATMT_Id == b.RATMT_Id && a.RATMQE_Id == c.RATMQE_Id
                                              && b.RATMT_LInkedToRelievingFlg == true && a.MI_Id == data.MI_Id
                                              && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                              && a.RATMT_Id == data.RATMT_Id && a.RATMQE_Id == data.RATMQE_Id)
                                              select new Rating_TransactionDTO
                                              {
                                                  RATMTQ_Id = a.RATMTQ_Id,
                                                  typeNameList = b.RATMT_RatingTypeName,
                                                  questionNameList = c.RATMQE_RatingQuestions,
                                                  RATMT_Id = b.RATMT_Id,
                                                  RATMQE_Id = c.RATMQE_Id
                                              }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_TransactionDTO getdesignation(Rating_TransactionDTO data)
        {
            try
            {

                string departments = "10000";
                if (data.departmentlist.Length > 0)
                {
                    foreach (var ue in data.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Designation_Data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
                    {
                        Value = departments
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Designation = retObject.ToArray();
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
        public Rating_TransactionDTO OnLoadRatingDressCode(Rating_TransactionDTO data)
        {
            try
            {

                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }


                var hrme_ids = _context.Staff_User_Login.Where(a => a.Id == data.UserId && a.IVRMSTAUL_ActiveFlag == 1).ToList();

                var empdepartment = _context.MasterEmployee.Where(b => b.HRME_Id == hrme_ids[0].Emp_Code && b.HRME_ActiveFlag == true).ToList();


                var questionauthority = (from a in _context.Ratings_Master_QuestionsDMO
                                         from b in _context.Ratings_Master_Authority_QuestionsDMO
                                         from c in _context.Rating_Master_AuthorityDMO
                                         where (a.RATMQE_Id == b.RATMQE_Id && b.RATMAUT_Id == c.RATMAUT_Id && a.MI_Id == data.MI_Id && c.HRMD_Id == empdepartment[0].HRMD_Id && c.HRMDES_Id == empdepartment[0].HRMDES_Id && a.RATMQE_ActiveFlag == true && a.RATMQE_QuestionFlg == "DressCode")
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = a.RATMQE_Id,
                                             RATMQE_RatingQuestions = a.RATMQE_RatingQuestions,
                                             HRMD_Id = c.HRMD_Id,
                                             HRMDES_Id = c.HRMDES_Id
                                         }).ToList();


                data.GetTypeQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                         from b in _context.Rating_Type_Questions_DepartmentDMO
                                         from c in _context.Rating_Master_TypeDMO
                                         from e in _context.Rating_Master_Question_ClassificationDMO
                                         from f in _context.Rating_Master_Question_SubClassificationDMO
                                         from d in _context.Ratings_Master_QuestionsDMO
                                         where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                         && a.RATMTQ_ActiveFlag == true && e.RATMT_Id == c.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id && a.MI_Id == data.MI_Id && d.RATMQE_Id == questionauthority[0].RATMQE_Id
                                         && d.RATMQE_AutomaticFlg == true && c.RATMT_QuestionwiseOptionFlg == false && d.RATMQE_QuestionFlg == "DressCode")
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = d.RATMQE_Id,
                                             MI_Id = d.MI_Id,
                                             RATMQECL_Id = e.RATMQECL_Id,
                                             RATMQECL_QuestionsClassification = e.RATMQECL_QuestionsClassification,
                                             RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                             RATMQECLSUB_QuestionsSubclassification = f.RATMQECLSUB_QuestionsSubclassification,
                                             RATMQE_RatingQuestions = d.RATMQE_RatingQuestions,
                                             RATMQE_ManualEntryFlg = d.RATMQE_ManualEntryFlg,
                                             RATMQE_RQOrder = d.RATMQE_RQOrder,
                                             RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                             RATMT_Id = c.RATMT_Id,
                                             RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                         }).Distinct().ToArray();

                data.getDresscodetransaction = (from a in _context.Rating_TransactionDMO
                                                from b in _context.Rating_Type_QuestionsDMO
                                                from c in _context.Ratings_Master_QuestionsDMO
                                                from d in _context.Rating_Type_Questions_OptionsDMO
                                                from e in _context.Rating_Master_OptionsDMO
                                                from f in _context.Rating_Master_Question_SubClassificationDMO
                                                from g in _context.MasterEmployee
                                                from h in _context.Institution
                                                where (a.RATMQE_Id == b.RATMQE_Id && a.RATMT_Id == b.RATMT_Id && b.RATMQE_Id == c.RATMQE_Id && b.RATMTQ_Id == d.RATMTQ_Id && e.RATMOP_Id == d.RATMOP_Id
                                                && a.RATMOP_Id == e.RATMOP_Id && a.HRME_Id == g.HRME_Id && c.RATMQECLSUB_Id == f.RATMQECLSUB_Id && g.MI_Id == h.MI_Id && a.RATTR_Feedback_UserId == data.UserId && c.RATMQE_QuestionFlg == "DressCode")
                                                select new Rating_TransactionDTO
                                                {
                                                    RATTR_Id = a.RATTR_Id,
                                                    RATTR_FeedbackDate = a.RATTR_FeedbackDate,
                                                    RATMQE_Id = a.RATMQE_Id,
                                                    RATMQE_RatingQuestions = c.RATMQE_RatingQuestions,
                                                    RATMOP_Id = e.RATMOP_Id,
                                                    RATMOP_RatingOptions = e.RATMOP_RatingOptions,
                                                    RATMOP_OptionsValue = e.RATMOP_OptionsValue,
                                                    RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                                    RATMQECLSUB_QuestionsSubclassification = f.RATMQECLSUB_QuestionsSubclassification,
                                                    HRME_Id = g.HRME_Id,
                                                    HRME_EmployeeFirstName = g.HRME_EmployeeFirstName + " " + (g.HRME_EmployeeMiddleName == null ? "" : g.HRME_EmployeeMiddleName) + " " + (g.HRME_EmployeeLastName == null ? "" : g.HRME_EmployeeLastName),
                                                    MI_Id = h.MI_Id,
                                                    MI_Name = h.MI_Name

                                                }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_TransactionDTO checkratingDresscode(Rating_TransactionDTO data)
        {
            try
            {
                var hrme_ids = _context.Staff_User_Login.Where(a => a.Id == data.UserId && a.IVRMSTAUL_ActiveFlag == 1).ToList();

                var empdepartment = _context.MasterEmployee.Where(b => b.HRME_Id == hrme_ids[0].Emp_Code && b.HRME_ActiveFlag == true).ToList();

                var questionauthority = (from a in _context.Ratings_Master_QuestionsDMO
                                         from b in _context.Ratings_Master_Authority_QuestionsDMO
                                         from c in _context.Rating_Master_AuthorityDMO
                                         where (a.RATMQE_Id == b.RATMQE_Id && b.RATMAUT_Id == c.RATMAUT_Id && a.MI_Id == data.MI_Id && c.HRMD_Id == empdepartment[0].HRMD_Id && c.HRMDES_Id == empdepartment[0].HRMDES_Id && a.RATMQE_ActiveFlag == true && a.RATMQE_QuestionFlg == "DressCode")
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = a.RATMQE_Id,
                                             RATMQE_RatingQuestions = a.RATMQE_RatingQuestions,
                                             HRMD_Id = c.HRMD_Id,
                                             HRMDES_Id = c.HRMDES_Id
                                         }).ToList();

                var year = data.RATTR_FeedbackDate.Value.Year;
                var month = data.RATTR_FeedbackDate.Value.Month;
                var ratingdate = data.RATTR_FeedbackDate.Value.ToString("yyyy-MM-dd");

                if (questionauthority.Count > 0)
                {

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RATING_DRESSCODE_FETCH_EMPLOYEE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt) { Value = empdepartment[0].HRMD_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.BigInt) { Value = empdepartment[0].HRMDES_Id });
                        cmd.Parameters.Add(new SqlParameter("@RATMQE_Id", SqlDbType.BigInt) { Value = questionauthority[0].RATMQE_Id });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt) { Value = year });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt) { Value = month });
                        cmd.Parameters.Add(new SqlParameter("@RATTR_FeedbackDate", SqlDbType.VarChar) { Value = ratingdate });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.GetEmployeeDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_TransactionDTO SaveRatingDresscode(Rating_TransactionDTO data)
        {
            try
            {
                var autoquestion_count = 0;
                var manualquestion_count = 0;
                var autorating_count = 0;
                var manualrating_count = 0;
                var Totalquestion_count = 0;
                decimal sumofrating = 0;

                long ratingoption_id = 0;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                if (data.employeedata.Length > 0)
                {
                    for (int k = 0; k < data.employeedata.Length; k++)
                    {
                        var ratingdresscodecount = _context.Rating_TransactionDMO.Where(r => r.RATMT_Id == data.RATMT_Id && r.RATMQE_Id == data.RATMQE_Id && r.HRME_Id == data.employeedata[k].HRME_Id &&
                        r.RATTR_FeedbackDate.Value.Month == data.RATTR_FeedbackDate.Value.Month && r.RATTR_FeedbackDate.Value.Year == data.RATTR_FeedbackDate.Value.Year).ToList();

                        if (ratingdresscodecount.Count == 0)
                        {
                            ratingoption_id = 4;
                        }
                        else if (ratingdresscodecount.Count == 1)
                        {
                            ratingoption_id = 3;
                        }
                        else if (ratingdresscodecount.Count == 2)
                        {
                            ratingoption_id = 0;
                        }
                        else if (ratingdresscodecount.Count == 3)
                        {
                            ratingoption_id = -1;
                        }
                        else if (ratingdresscodecount.Count >= 4)
                        {
                            ratingoption_id = -2;
                        }

                        var ratmopids = (from a in _context.Rating_Type_QuestionsDMO
                                         from b in _context.Rating_Type_Questions_OptionsDMO
                                         from c in _context.Rating_Master_OptionsDMO
                                         where (a.RATMTQ_Id == b.RATMTQ_Id && b.RATMOP_Id == c.RATMOP_Id && a.RATMT_Id == data.RATMT_Id && a.RATMQE_Id == data.RATMQE_Id && c.MI_Id == data.MI_Id &&
                                         c.RATMOP_ActiveFlag == true && c.RATMOP_OptionsValue == ratingoption_id)
                                         select c).ToList();

                        Rating_TransactionDMO rating_TransactionDMO = new Rating_TransactionDMO();
                        rating_TransactionDMO.MI_Id = data.MI_Id;
                        rating_TransactionDMO.HRME_Id = data.employeedata[k].HRME_Id;
                        rating_TransactionDMO.RATTR_FeedbackDate = data.RATTR_FeedbackDate;
                        rating_TransactionDMO.RATMT_Id = data.RATMT_Id;
                        rating_TransactionDMO.RATMQE_Id = data.RATMQE_Id;
                        rating_TransactionDMO.RATMOP_Id = ratmopids[0].RATMOP_Id;
                        rating_TransactionDMO.RATTR_Feedback_UserId = data.UserId;
                        rating_TransactionDMO.RATTR_ActiveFlag = true;
                        rating_TransactionDMO.RATTR_CreatedBy = data.UserId;
                        rating_TransactionDMO.RATTR_CreatedDate = DateTime.Now;
                        rating_TransactionDMO.RATTR_UpdatedBy = data.UserId;
                        rating_TransactionDMO.RATTR_UpdatedDate = DateTime.Now;
                        _context.Add(rating_TransactionDMO);

                    }



                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        // Rating Appearance
        public Rating_TransactionDTO OnClickRatingAppearance(Rating_TransactionDTO data)
        {
            try
            {
                //var questionauthority = (from a in _context.Ratings_Master_QuestionsDMO
                //                         from b in _context.Ratings_Master_Authority_QuestionsDMO
                //                         from c in _context.Rating_Master_AuthorityDMO
                //                         where (a.RATMQE_Id == b.RATMQE_Id && b.RATMAUT_Id == c.RATMAUT_Id && a.MI_Id == data.MI_Id && a.RATMQE_ActiveFlag == true && a.RATMQE_QuestionFlg == "DressCode")
                //                         select new Rating_TransactionDTO
                //                         {
                //                             RATMQE_Id = a.RATMQE_Id,
                //                             RATMQE_RatingQuestions = a.RATMQE_RatingQuestions,
                //                             HRMD_Id = c.HRMD_Id,
                //                             HRMDES_Id = c.HRMDES_Id
                //                         }).ToList();


                var departmentval = _context.Staff_User_Login.Where(a => a.Id == data.UserId && a.IVRMSTAUL_ActiveFlag == 1).ToList();

                //var empdepartment = _context.MasterEmployee.Where(b => b.HRME_Id == departmentval[0].Emp_Code && b.HRME_ActiveFlag == true).ToList();

                //if (questionauthority[0].HRMD_Id == empdepartment[0].HRMD_Id && questionauthority[0].HRMDES_Id == empdepartment[0].HRMDES_Id)
                //{

                //    if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                //    {
                //        data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                //    }



                //}

                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                data.GetEmployeeDetails = _context.MasterEmployee.Where(a => a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id).ToArray();

                data.getAppearancetransaction = (from a in _context.Rating_TransactionDMO
                                                 from b in _context.Rating_Type_QuestionsDMO
                                                 from c in _context.Ratings_Master_QuestionsDMO
                                                 from d in _context.Rating_Type_Questions_OptionsDMO
                                                 from e in _context.Rating_Master_OptionsDMO
                                                 from f in _context.Rating_Master_Question_SubClassificationDMO
                                                 from g in _context.MasterEmployee
                                                 from h in _context.Institution
                                                 where (a.RATMQE_Id == b.RATMQE_Id && a.RATMT_Id == b.RATMT_Id && b.RATMQE_Id == c.RATMQE_Id && b.RATMTQ_Id == d.RATMTQ_Id && e.RATMOP_Id == d.RATMOP_Id
                                                 && a.RATMOP_Id == e.RATMOP_Id && a.HRME_Id == g.HRME_Id && c.RATMQECLSUB_Id == f.RATMQECLSUB_Id && g.MI_Id == h.MI_Id && a.RATTR_Feedback_UserId == data.UserId && c.RATMQE_QuestionFlg == "Appearance")
                                                 select new Rating_TransactionDTO
                                                 {
                                                     RATTR_Id = a.RATTR_Id,
                                                     RATTR_FeedbackDate = a.RATTR_FeedbackDate,
                                                     RATMQE_Id = a.RATMQE_Id,
                                                     RATMQE_RatingQuestions = c.RATMQE_RatingQuestions,
                                                     RATMOP_Id = e.RATMOP_Id,
                                                     RATMOP_RatingOptions = e.RATMOP_RatingOptions,
                                                     RATMOP_OptionsValue = e.RATMOP_OptionsValue,
                                                     RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                                     RATMQECLSUB_QuestionsSubclassification = f.RATMQECLSUB_QuestionsSubclassification,
                                                     HRME_Id = g.HRME_Id,
                                                     HRME_EmployeeFirstName = g.HRME_EmployeeFirstName + " " + (g.HRME_EmployeeMiddleName == null ? "" : g.HRME_EmployeeMiddleName) + " " + (g.HRME_EmployeeLastName == null ? "" : g.HRME_EmployeeLastName),
                                                     MI_Id = h.MI_Id,
                                                     MI_Name = h.MI_Name

                                                 }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_TransactionDTO checkratingAppearance(Rating_TransactionDTO data)
        {
            try
            {
                var hrme_ids = _context.Staff_User_Login.Where(a => a.Id == data.UserId && a.IVRMSTAUL_ActiveFlag == 1).ToList();

                var empdepartment = _context.MasterEmployee.Where(b => b.HRME_Id == hrme_ids[0].Emp_Code && b.HRME_ActiveFlag == true).ToList();


                data.GetTypeQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                         from c in _context.Rating_Master_TypeDMO
                                         from e in _context.Rating_Master_Question_ClassificationDMO
                                         from f in _context.Rating_Master_Question_SubClassificationDMO
                                         from d in _context.Ratings_Master_QuestionsDMO
                                         where ( a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                         && a.RATMTQ_ActiveFlag == true && e.RATMT_Id == c.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id && a.MI_Id == data.MI_Id 
                                         && d.RATMQE_AutomaticFlg == true && c.RATMT_QuestionwiseOptionFlg == false && d.RATMQE_QuestionFlg == "Appearance")
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = d.RATMQE_Id,
                                             MI_Id = d.MI_Id,
                                             RATMQECL_Id = e.RATMQECL_Id,
                                             RATMQECL_QuestionsClassification = e.RATMQECL_QuestionsClassification,
                                             RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                             RATMQECLSUB_QuestionsSubclassification = f.RATMQECLSUB_QuestionsSubclassification,
                                             RATMQE_RatingQuestions = d.RATMQE_RatingQuestions,
                                             RATMQE_ManualEntryFlg = d.RATMQE_ManualEntryFlg,
                                             RATMQE_RQOrder = d.RATMQE_RQOrder,
                                             RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                             RATMT_Id = c.RATMT_Id,
                                             RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                         }).Distinct().ToArray();

                data.GetTypeQuestionOptions = (from a in _context.Rating_Type_QuestionsDMO
                                               from c in _context.Rating_Master_TypeDMO
                                               from g in _context.Rating_Master_Question_ClassificationDMO
                                               from h in _context.Rating_Master_Question_SubClassificationDMO
                                               from d in _context.Ratings_Master_QuestionsDMO
                                               from e in _context.Rating_Type_Questions_OptionsDMO
                                               from f in _context.Rating_Master_OptionsDMO
                                               where ( a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                               && a.RATMTQ_Id == e.RATMTQ_Id && c.RATMT_Id == g.RATMT_Id && g.RATMQECL_Id == h.RATMQECL_Id && h.RATMQECLSUB_Id == d.RATMQECLSUB_Id && e.RATMOP_Id == f.RATMOP_Id && a.RATMTQ_ActiveFlag == true
                                               && a.MI_Id == data.MI_Id &&  d.RATMQE_AutomaticFlg == true && c.RATMT_QuestionwiseOptionFlg == false && d.RATMQE_QuestionFlg == "Appearance"
                                               )
                                               select new Rating_TransactionDTO
                                               {
                                                   RATMOP_OptionsValue = f.RATMOP_OptionsValue,
                                                   RATMOP_RatingOptions = f.RATMOP_RatingOptions,
                                                   RATMOP_ROOrder = f.RATMOP_ROOrder,
                                                   RATMOP_Id = f.RATMOP_Id,
                                                   RATMQE_Id = a.RATMQE_Id,
                                                   RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                                   RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                               }).Distinct().OrderByDescending(a => a.RATMOP_OptionsValue).ToArray();


                var questionauthority = (from a in _context.Ratings_Master_QuestionsDMO
                                         from b in _context.Ratings_Master_Authority_QuestionsDMO
                                         from c in _context.Rating_Master_AuthorityDMO
                                         where (a.RATMQE_Id == b.RATMQE_Id && b.RATMAUT_Id == c.RATMAUT_Id && a.MI_Id == data.MI_Id && c.HRMD_Id == empdepartment[0].HRMD_Id && c.HRMDES_Id == empdepartment[0].HRMDES_Id && a.RATMQE_ActiveFlag == true && a.RATMQE_QuestionFlg == "Appearance")
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = a.RATMQE_Id,
                                             RATMQE_RatingQuestions = a.RATMQE_RatingQuestions,
                                             HRMD_Id = c.HRMD_Id,
                                             HRMDES_Id = c.HRMDES_Id
                                         }).ToList();

                var year = data.RATTR_FeedbackDate.Value.Year;
                var month = data.RATTR_FeedbackDate.Value.Month;
                var ratingdate = data.RATTR_FeedbackDate.Value.ToString("yyyy-MM-dd");

                if (questionauthority.Count > 0)
                {

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RATING_APPEARANCE_FETCH_EMPLOYEE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt) { Value = empdepartment[0].HRMD_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.BigInt) { Value = empdepartment[0].HRMDES_Id });
                        cmd.Parameters.Add(new SqlParameter("@RATMQE_Id", SqlDbType.BigInt) { Value = questionauthority[0].RATMQE_Id });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt) { Value = year });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt) { Value = month });
                        cmd.Parameters.Add(new SqlParameter("@RATTR_FeedbackDate", SqlDbType.VarChar) { Value = ratingdate });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.GetEmployeeDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        // Rating Behaviour
        public Rating_TransactionDTO OnClickRatingBehaviour(Rating_TransactionDTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                var hrme_ids = _context.Staff_User_Login.Where(a => a.Id == data.UserId && a.IVRMSTAUL_ActiveFlag == 1).ToList();

                var empdepartment = _context.MasterEmployee.Where(b => b.HRME_Id == hrme_ids[0].Emp_Code && b.HRME_ActiveFlag == true).ToList();

                data.GetTypeQuestions = (from a in _context.Ratings_Master_QuestionsDMO
                                         from b in _context.Ratings_Master_Authority_QuestionsDMO
                                         from c in _context.Rating_Master_AuthorityDMO
                                         where (a.RATMQE_Id == b.RATMQE_Id && b.RATMAUT_Id == c.RATMAUT_Id && a.MI_Id == data.MI_Id && c.HRMD_Id == empdepartment[0].HRMD_Id && c.HRMDES_Id == empdepartment[0].HRMDES_Id && a.RATMQE_ActiveFlag == true && a.RATMQE_QuestionFlg == "Behaviour")
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = a.RATMQE_Id,
                                             RATMQE_RatingQuestions = a.RATMQE_RatingQuestions,
                                             HRMD_Id = c.HRMD_Id,
                                             HRMDES_Id = c.HRMDES_Id
                                         }).ToArray();

                data.getbehaviourtransaction = (from a in _context.Rating_TransactionDMO
                                                from b in _context.Rating_Type_QuestionsDMO
                                                from c in _context.Ratings_Master_QuestionsDMO
                                                from d in _context.Rating_Type_Questions_OptionsDMO
                                                from e in _context.Rating_Master_OptionsDMO
                                                from f in _context.Rating_Master_Question_SubClassificationDMO
                                                from g in _context.MasterEmployee
                                                from h in _context.Institution
                                                where (a.RATMQE_Id == b.RATMQE_Id && a.RATMT_Id == b.RATMT_Id && b.RATMQE_Id == c.RATMQE_Id && b.RATMTQ_Id == d.RATMTQ_Id && e.RATMOP_Id == d.RATMOP_Id
                                                && a.RATMOP_Id == e.RATMOP_Id && a.HRME_Id == g.HRME_Id && c.RATMQECLSUB_Id == f.RATMQECLSUB_Id && g.MI_Id == h.MI_Id && a.RATTR_Feedback_UserId == data.UserId && c.RATMQE_QuestionFlg == "Behaviour")
                                                select new Rating_TransactionDTO
                                                {
                                                    RATTR_Id = a.RATTR_Id,
                                                    RATTR_FeedbackDate = a.RATTR_FeedbackDate,
                                                    RATMQE_Id = a.RATMQE_Id,
                                                    RATMQE_RatingQuestions = c.RATMQE_RatingQuestions,
                                                    RATMOP_Id = e.RATMOP_Id,
                                                    RATMOP_RatingOptions = e.RATMOP_RatingOptions,
                                                    RATMOP_OptionsValue = e.RATMOP_OptionsValue,
                                                    RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                                    RATMQECLSUB_QuestionsSubclassification = f.RATMQECLSUB_QuestionsSubclassification,
                                                    HRME_Id = g.HRME_Id,
                                                    HRME_EmployeeFirstName = g.HRME_EmployeeFirstName + " " + (g.HRME_EmployeeMiddleName == null ? "" : g.HRME_EmployeeMiddleName) + " " + (g.HRME_EmployeeLastName == null ? "" : g.HRME_EmployeeLastName),
                                                    MI_Id = h.MI_Id,
                                                    MI_Name = h.MI_Name

                                                }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_TransactionDTO checkratingbehaviour(Rating_TransactionDTO data)
        {
            try
            {
                var hrme_ids = _context.Staff_User_Login.Where(a => a.Id == data.UserId && a.IVRMSTAUL_ActiveFlag == 1).ToList();

                var empdepartment = _context.MasterEmployee.Where(b => b.HRME_Id == hrme_ids[0].Emp_Code && b.HRME_ActiveFlag == true).ToList();

                data.GetTypeQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                         from c in _context.Rating_Master_TypeDMO
                                         from e in _context.Rating_Master_Question_ClassificationDMO
                                         from f in _context.Rating_Master_Question_SubClassificationDMO
                                         from d in _context.Ratings_Master_QuestionsDMO
                                         where (a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                         && a.RATMTQ_ActiveFlag == true && e.RATMT_Id == c.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id && a.MI_Id == data.MI_Id
                                         && d.RATMQE_AutomaticFlg == true && c.RATMT_QuestionwiseOptionFlg == false && d.RATMQE_QuestionFlg == "Behaviour")
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = d.RATMQE_Id,
                                             MI_Id = d.MI_Id,
                                             RATMQECL_Id = e.RATMQECL_Id,
                                             RATMQECL_QuestionsClassification = e.RATMQECL_QuestionsClassification,
                                             RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                             RATMQECLSUB_QuestionsSubclassification = f.RATMQECLSUB_QuestionsSubclassification,
                                             RATMQE_RatingQuestions = d.RATMQE_RatingQuestions,
                                             RATMQE_ManualEntryFlg = d.RATMQE_ManualEntryFlg,
                                             RATMQE_RQOrder = d.RATMQE_RQOrder,
                                             RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                             RATMT_Id = c.RATMT_Id,
                                             RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                         }).Distinct().ToArray();

                data.GetTypeQuestionOptions = (from a in _context.Rating_Type_QuestionsDMO
                                               from c in _context.Rating_Master_TypeDMO
                                               from g in _context.Rating_Master_Question_ClassificationDMO
                                               from h in _context.Rating_Master_Question_SubClassificationDMO
                                               from d in _context.Ratings_Master_QuestionsDMO
                                               from e in _context.Rating_Type_Questions_OptionsDMO
                                               from f in _context.Rating_Master_OptionsDMO
                                               where (a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                               && a.RATMTQ_Id == e.RATMTQ_Id && c.RATMT_Id == g.RATMT_Id && g.RATMQECL_Id == h.RATMQECL_Id && h.RATMQECLSUB_Id == d.RATMQECLSUB_Id && e.RATMOP_Id == f.RATMOP_Id && a.RATMTQ_ActiveFlag == true
                                               && a.MI_Id == data.MI_Id && d.RATMQE_AutomaticFlg == true && c.RATMT_QuestionwiseOptionFlg == false && d.RATMQE_QuestionFlg == "Behaviour"
                                               )
                                               select new Rating_TransactionDTO
                                               {
                                                   RATMOP_OptionsValue = f.RATMOP_OptionsValue,
                                                   RATMOP_RatingOptions = f.RATMOP_RatingOptions,
                                                   RATMOP_ROOrder = f.RATMOP_ROOrder,
                                                   RATMOP_Id = f.RATMOP_Id,
                                                   RATMQE_Id = a.RATMQE_Id,
                                                   RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                                   RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                               }).Distinct().OrderByDescending(a => a.RATMOP_OptionsValue).ToArray();


                var questionauthority = (from a in _context.Ratings_Master_QuestionsDMO
                                         from b in _context.Ratings_Master_Authority_QuestionsDMO
                                         from c in _context.Rating_Master_AuthorityDMO
                                         where (a.RATMQE_Id == b.RATMQE_Id && b.RATMAUT_Id == c.RATMAUT_Id && a.MI_Id == data.MI_Id && c.HRMD_Id == empdepartment[0].HRMD_Id && c.HRMDES_Id == empdepartment[0].HRMDES_Id && a.RATMQE_ActiveFlag == true && a.RATMQE_QuestionFlg == "Behaviour")
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = a.RATMQE_Id,
                                             RATMQE_RatingQuestions = a.RATMQE_RatingQuestions,
                                             HRMD_Id = c.HRMD_Id,
                                             HRMDES_Id = c.HRMDES_Id
                                         }).ToList();

                var year = data.RATTR_FeedbackDate.Value.Year;
                var month = data.RATTR_FeedbackDate.Value.Month;


                if (questionauthority.Count > 0)
                {

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RATING_BEHAVIOUR_FETCH_EMPLOYEE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt) { Value = empdepartment[0].HRMD_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.BigInt) { Value = empdepartment[0].HRMDES_Id });
                        cmd.Parameters.Add(new SqlParameter("@RATMQE_Id", SqlDbType.BigInt) { Value = questionauthority[0].RATMQE_Id });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt) { Value = year });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt) { Value = month });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.GetEmployeeDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_TransactionDTO OnChangeBehaviourEmployee(Rating_TransactionDTO data)
        {
            try
            {
                var EmployeeDetails = _context.MasterEmployee.Where(a => a.HRME_Id == data.HRME_Id).ToList();

                var HRMD_Id = EmployeeDetails.FirstOrDefault().HRMD_Id;
                var HRMDES_Id = EmployeeDetails.FirstOrDefault().HRMDES_Id;
                var MI_Id = EmployeeDetails.FirstOrDefault().MI_Id;

                var empdetails = (from a in _context.MasterEmployee
                                  from b in _context.Staff_User_Login
                                  where (a.HRME_Id == b.Emp_Code && a.HRME_Id == data.HRME_Id)
                                  select new Rating_TransactionDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      UserId = b.Id,
                                      MI_Id = a.MI_Id,
                                      HRME_DOJ = a.HRME_DOJ
                                  }).ToList();


                data.GetTypeQuestions = (from a in _context.Rating_Type_QuestionsDMO
                                         from b in _context.Rating_Type_Questions_DepartmentDMO
                                         from c in _context.Rating_Master_TypeDMO
                                         from e in _context.Rating_Master_Question_ClassificationDMO
                                         from f in _context.Rating_Master_Question_SubClassificationDMO
                                         from d in _context.Ratings_Master_QuestionsDMO
                                         where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                         && a.RATMTQ_ActiveFlag == true && e.RATMT_Id == c.RATMT_Id && e.RATMQECL_Id == f.RATMQECL_Id && f.RATMQECLSUB_Id == d.RATMQECLSUB_Id && a.MI_Id == MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id
                                         && d.RATMQE_AutomaticFlg == true && c.RATMT_QuestionwiseOptionFlg == false && d.RATMQE_QuestionFlg == data.flag)
                                         select new Rating_TransactionDTO
                                         {
                                             RATMQE_Id = d.RATMQE_Id,
                                             MI_Id = d.MI_Id,
                                             RATMQECL_Id = e.RATMQECL_Id,
                                             RATMQECL_QuestionsClassification = e.RATMQECL_QuestionsClassification,
                                             RATMQECLSUB_Id = f.RATMQECLSUB_Id,
                                             RATMQECLSUB_QuestionsSubclassification = f.RATMQECLSUB_QuestionsSubclassification,
                                             RATMQE_RatingQuestions = d.RATMQE_RatingQuestions,
                                             RATMQE_ManualEntryFlg = d.RATMQE_ManualEntryFlg,
                                             RATMQE_RQOrder = d.RATMQE_RQOrder,
                                             RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                             RATMT_Id = c.RATMT_Id,
                                             RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                         }).Distinct().ToArray();

                data.GetTypeQuestionOptions = (from a in _context.Rating_Type_QuestionsDMO
                                               from b in _context.Rating_Type_Questions_DepartmentDMO
                                               from c in _context.Rating_Master_TypeDMO
                                               from g in _context.Rating_Master_Question_ClassificationDMO
                                               from h in _context.Rating_Master_Question_SubClassificationDMO
                                               from d in _context.Ratings_Master_QuestionsDMO
                                               from e in _context.Rating_Type_Questions_OptionsDMO
                                               from f in _context.Rating_Master_OptionsDMO
                                               where (a.RATMTQ_Id == b.RATMTQ_Id && a.RATMT_Id == c.RATMT_Id && a.RATMQE_Id == d.RATMQE_Id
                                               && a.RATMTQ_Id == e.RATMTQ_Id && c.RATMT_Id == g.RATMT_Id && g.RATMQECL_Id == h.RATMQECL_Id && h.RATMQECLSUB_Id == d.RATMQECLSUB_Id && e.RATMOP_Id == f.RATMOP_Id && a.RATMTQ_ActiveFlag == true
                                               && a.MI_Id == MI_Id && b.HRMD_Id == HRMD_Id && b.HRMDES_Id == HRMDES_Id && d.RATMQE_AutomaticFlg == true && c.RATMT_QuestionwiseOptionFlg == false && d.RATMQE_QuestionFlg == data.flag
                                               )
                                               select new Rating_TransactionDTO
                                               {
                                                   RATMOP_OptionsValue = f.RATMOP_OptionsValue,
                                                   RATMOP_RatingOptions = f.RATMOP_RatingOptions,
                                                   RATMOP_ROOrder = f.RATMOP_ROOrder,
                                                   RATMOP_Id = f.RATMOP_Id,
                                                   RATMQE_Id = a.RATMQE_Id,
                                                   RATMQE_AutomaticFlg = d.RATMQE_AutomaticFlg,
                                                   RATMT_QuestionwiseOptionFlg = c.RATMT_QuestionwiseOptionFlg

                                               }).Distinct().OrderByDescending(a => a.RATMOP_OptionsValue).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.employeeprofile = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PendingIssues";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = empdetails[0].UserId });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMPR_Id", SqlDbType.BigInt) { Value = 0 });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.prioritylist = retObject.ToArray();
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
        public Rating_TransactionDTO SaveBehaviourRating(Rating_TransactionDTO data)
        {
            try
            {
                var autoquestion_count = 0;
                var manualquestion_count = 0;
                var autorating_count = 0;
                var manualrating_count = 0;
                var Totalquestion_count = 0;
                decimal sumofrating = 0;



                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                //var EmployeeDetails = _context.MasterEmployee.Where(a => a.HRME_Id == data.HRME_Id).ToList();
                //var HRMD_Id = EmployeeDetails.FirstOrDefault().HRMD_Id;
                //var HRMDES_Id = EmployeeDetails.FirstOrDefault().HRMDES_Id;
                //data.MI_Id = EmployeeDetails.FirstOrDefault().MI_Id;

                if (data.Save_Behaviour_Rating_Detail != null && data.Save_Behaviour_Rating_Detail.Length > 0)
                {

                    foreach (var e in data.Save_Behaviour_Rating_Detail)
                    {

                        var rating_transid = _context.Rating_TransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == e.HRME_Id && t.RATMT_Id == e.RATMT_Id &&
                        t.RATMQE_Id == e.RATMQE_Id && t.RATTR_FeedbackDate == data.RATTR_FeedbackDate).Select(t => t.RATTR_Id).FirstOrDefault();

                        if (rating_transid > 0)
                        {
                            var Result = _context.Rating_TransactionDMO.Single(a => a.RATTR_Id == rating_transid);

                            Result.RATMOP_Id = Convert.ToInt64(e.name);

                            Result.RATTR_UpdatedBy = data.UserId;
                            Result.RATTR_UpdatedDate = DateTime.Now;
                            _context.Update(Result);
                        }
                        else
                        {
                            Rating_TransactionDMO rating_TransactionDMO = new Rating_TransactionDMO();
                            rating_TransactionDMO.MI_Id = data.MI_Id;
                            rating_TransactionDMO.HRME_Id = e.HRME_Id;
                            rating_TransactionDMO.RATTR_FeedbackDate = data.RATTR_FeedbackDate;
                            rating_TransactionDMO.RATMT_Id = e.RATMT_Id;
                            rating_TransactionDMO.RATMQE_Id = e.RATMQE_Id;
                            if (e.RATMQE_ManualEntryFlg)
                            {
                                rating_TransactionDMO.RATTR_FeedBack = e.name;
                            }
                            else
                            {
                                rating_TransactionDMO.RATMOP_Id = Convert.ToInt64(e.name);
                            }

                            rating_TransactionDMO.RATTR_Feedback_UserId = data.UserId;
                            rating_TransactionDMO.RATTR_ActiveFlag = true;
                            rating_TransactionDMO.RATTR_CreatedBy = data.UserId;
                            rating_TransactionDMO.RATTR_CreatedDate = DateTime.Now;
                            rating_TransactionDMO.RATTR_UpdatedBy = data.UserId;
                            rating_TransactionDMO.RATTR_UpdatedDate = DateTime.Now;
                            _context.Add(rating_TransactionDMO);
                        }

                    }


                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<Rating_TransactionDTO> mobemployeerating(Rating_TransactionDTO data)
        {

            try
            {

                string hrme = "0";
                if (data.arrayuserEmp != null && data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrme = hrme + "," + ue.HRME_Id;
                    }
                }
                
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EmployeeWise_Rating";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@YEAR",
                    SqlDbType.BigInt)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_IDs",
               SqlDbType.VarChar)
                    {
                        Value = hrme
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
                        data.autoCaluclatedRating = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }               
                if (data.autoCaluclatedRating.Length > 0)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<Rating_TransactionDTO> subclassificationwisereport(Rating_TransactionDTO data)
        {

            try            {                var hrme_id = _context.Staff_User_Login.Where(s => s.Id == data.UserId).Select(d => d.Emp_Code).FirstOrDefault();                var empdetail = _context.MasterEmployee.Where(t => t.HRME_Id == hrme_id).ToArray();                var hrmdc_id = _context.HR_Master_Department.Where(d => d.HRMD_Id == empdetail[0].HRMD_Id).Select(d => d.HRMDC_ID).Single();
                var Rating_Type = _context.Rating_Master_TypeDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();
                string RATMT_Ids = "0";
                if (Rating_Type.Length > 0 && Rating_Type != null)
                {
                    foreach (var ue in Rating_Type)
                    {
                        RATMT_Ids = RATMT_Ids + "," + ue.RATMT_Id;
                    }
                }                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SubclassificationwiseRating_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.VarChar)
                    {
                        Value = hrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmdc_id

                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
          SqlDbType.VarChar)
                    {

                        Value = empdetail[0].HRMDES_Id

                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
      SqlDbType.VarChar)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
     SqlDbType.VarChar)
                    {
                        Value = data.monthid
                    });

                    cmd.Parameters.Add(new SqlParameter("@RATMT_Id",
SqlDbType.VarChar)
                    {
                        Value = RATMT_Ids
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
                        data.consolidateData = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                if (data.consolidateData.Length > 0)                {                    data.returnval = true;                }                else                {                    data.returnval = false;                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }


            return data;
        }
    }
}