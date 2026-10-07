using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.Rating;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;

namespace IssueManager.com.Rating.Services
{
    public class Rating_MasterImpl : Interfacce.Rating_MasterInterface
    {
        public IssueManagerContext _context;

        public Rating_MasterImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }

        //Master Type
        public Rating_Master_DTO OnLoadRatingMasterType(Rating_Master_DTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    //var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    //List<long> miids = new List<long>();

                    //foreach (var c in getuserinstitution)
                    //{
                    //    miids.Add(c.MI_Id);
                    //}

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                data.GetMasterTypeLoadData = (from a in _context.Rating_Master_TypeDMO
                                              where (a.MI_Id == data.MI_Id)
                                              select new Rating_Master_DTO
                                              {
                                                  RATMT_Id = a.RATMT_Id,
                                                  MI_Id = a.MI_Id,
                                                  RATMT_RatingTypeName = a.RATMT_RatingTypeName,
                                                  RATMT_DepartmentwiseFlg = a.RATMT_DepartmentwiseFlg,
                                                  RATMT_PeriodicityFlg = a.RATMT_PeriodicityFlg,
                                                  RATMT_LInkedToRelievingFlg = a.RATMT_LInkedToRelievingFlg,
                                                  RATMT_QuestionwiseOptionFlg = a.RATMT_QuestionwiseOptionFlg,
                                                  RATMT_ActiveFlag = a.RATMT_ActiveFlag,
                                                  CreatedBy = a.RATMT_CreatedBy,
                                                  CreatedDate = a.RATMT_CreatedDate,
                                                  MappedCount = (_context.Rating_TransactionDMO.Where(b => b.RATMT_Id == a.RATMT_Id && b.MI_Id == data.MI_Id
                                                                 && b.RATTR_ActiveFlag == true).Count())
                                              }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();


                data.OverAllRatingGrid = _context.Rating_Overall_RatingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.OverAllTypeGrid = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_LInkedToRelievingFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO SaveMasterType(Rating_Master_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                if (data.RATMT_Id > 0)
                {
                    data.returnmsg = "Update";
                    data.returnval = false;
                    var CheckDuplcate = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMT_RatingTypeName == data.RATMT_RatingTypeName && a.RATMT_Id != data.RATMT_Id).ToList();
                    if (CheckDuplcate.Count == 0)
                    {
                        var Result = _context.Rating_Master_TypeDMO.Single(a => a.RATMT_Id == data.RATMT_Id);
                        Result.RATMT_RatingTypeName = data.RATMT_RatingTypeName;
                        Result.RATMT_PeriodicityFlg = data.RATMT_PeriodicityFlg;
                        Result.RATMT_DepartmentwiseFlg = data.RATMT_DepartmentwiseFlg;
                        Result.RATMT_QuestionwiseOptionFlg = data.RATMT_QuestionwiseOptionFlg;
                        Result.RATMT_UpdatedBy = data.UserId;
                        Result.RATMT_UpdatedDate = indiantime0;
                        Result.RATMT_LInkedToRelievingFlg = data.RATMT_LInkedToRelievingFlg;

                        _context.Update(Result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnmsg = "Duplicate";
                    }
                }
                else
                {
                    data.returnmsg = "Add";
                    data.returnval = false;

                    var CheckDuplcate = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMT_RatingTypeName == data.RATMT_RatingTypeName).ToList();

                    if (CheckDuplcate.Count == 0)
                    {
                        Rating_Master_TypeDMO rating_Master_TypeDMO = new Rating_Master_TypeDMO();
                        rating_Master_TypeDMO.MI_Id = data.MI_Id;
                        rating_Master_TypeDMO.RATMT_RatingTypeName = data.RATMT_RatingTypeName;
                        rating_Master_TypeDMO.RATMT_PeriodicityFlg = data.RATMT_PeriodicityFlg;
                        rating_Master_TypeDMO.RATMT_DepartmentwiseFlg = data.RATMT_DepartmentwiseFlg;
                        rating_Master_TypeDMO.RATMT_QuestionwiseOptionFlg = data.RATMT_QuestionwiseOptionFlg;
                        rating_Master_TypeDMO.RATMT_ActiveFlag = true;
                        rating_Master_TypeDMO.RATMT_CreatedBy = data.UserId;
                        rating_Master_TypeDMO.RATMT_UpdatedBy = data.UserId;
                        rating_Master_TypeDMO.RATMT_CreatedDate = indiantime0;
                        rating_Master_TypeDMO.RATMT_UpdatedDate = indiantime0;
                        rating_Master_TypeDMO.RATMT_LInkedToRelievingFlg = data.RATMT_LInkedToRelievingFlg;
                        _context.Add(rating_Master_TypeDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnmsg = "Duplicate";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO EditMasterType(Rating_Master_DTO data)
        {
            try
            {
                data.GetMaterTypeEditData = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_Id == data.RATMT_Id).ToArray();

                // data.CheckRatingTypeMapped = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_Id == data.RATMT_Id).ToArray();

                data.CheckRatingTypeMapped = (
                    from a in _context.Rating_Master_TypeDMO
                    from b in _context.Rating_TransactionDMO
                    where (b.RATMT_Id == a.RATMT_Id && b.MI_Id == data.MI_Id && a.RATMT_Id == data.RATMT_Id
                                                                 && b.RATTR_ActiveFlag == true)
                    select b).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO MastetTypeActiveDeactive(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Result = _context.Rating_Master_TypeDMO.Single(a => a.RATMT_Id == data.RATMT_Id);
                Result.RATMT_ActiveFlag = Result.RATMT_ActiveFlag == true ? false : true;
                Result.RATMT_UpdatedBy = data.UserId;
                Result.RATMT_UpdatedDate = indiantime0;
                _context.Update(Result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                data.GetMasterTypeLoadData = _context.Rating_Master_TypeDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //Master Classification
        public Rating_Master_DTO OnClickRatingClassification(Rating_Master_DTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                data.getRatingTypeData = _context.Rating_Master_TypeDMO.Where(a => a.RATMT_ActiveFlag == true && a.MI_Id == data.MI_Id).ToArray();

                data.getMasterQuestionClassificationLoadData = (from a in _context.Rating_Master_Question_ClassificationDMO
                                                                from b in _context.Rating_Master_TypeDMO
                                                                where (a.MI_Id == data.MI_Id && a.RATMT_Id == b.RATMT_Id)
                                                                select new Rating_Master_DTO
                                                                {
                                                                    RATMQECL_Id = a.RATMQECL_Id,
                                                                    MI_Id = a.MI_Id,
                                                                    RATMT_Id = a.RATMT_Id,
                                                                    RATMT_RatingTypeName = b.RATMT_RatingTypeName,
                                                                    RATMQECL_QuestionsClassification = a.RATMQECL_QuestionsClassification,
                                                                    RATMQECL_RQOrder = a.RATMQECL_RQOrder,
                                                                    RATMQECL_ActiveFlag = a.RATMQECL_ActiveFlag,
                                                                    CreatedBy = a.RATMQECL_CreatedBy,
                                                                    CreatedDate = a.RATMQECL_CreatedDate,
                                                                    MappedCount = (_context.Rating_Master_Question_SubClassificationDMO.Where(b => b.RATMQECL_Id == a.RATMQECL_Id
                                                                              && b.RATMQECLSUB_ActiveFlag == true).Count())
                                                                }).Distinct().OrderBy(a => a.CreatedDate).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_Master_DTO SaveRatingClassification(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.RATMQECL_Id > 0)
                {
                    data.returnmsg = "Update";
                    data.returnval = false;

                    var CheckDuplicate = _context.Rating_Master_Question_ClassificationDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMQECL_QuestionsClassification.Equals(data.RATMQECL_QuestionsClassification) && a.RATMQECL_Id != data.RATMQECL_Id).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        var Result = _context.Rating_Master_Question_ClassificationDMO.Single(a => a.MI_Id == data.MI_Id && a.RATMQECL_Id == data.RATMQECL_Id);
                        Result.RATMQECL_QuestionsClassification = data.RATMQECL_QuestionsClassification;
                        Result.RATMQECL_UpdatedBy = data.UserId;
                        Result.RATMQECL_UpdatedDate = indiantime0;
                        _context.Update(Result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                }
                else
                {
                    data.returnmsg = "Add";
                    data.returnval = false;

                    var ResultOrder = _context.Rating_Master_Question_ClassificationDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    var CheckDuplicate = _context.Rating_Master_Question_ClassificationDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMQECL_QuestionsClassification.Equals(data.RATMQECL_QuestionsClassification)).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        Rating_Master_Question_ClassificationDMO rating_Master_classificationDMO = new Rating_Master_Question_ClassificationDMO();
                        rating_Master_classificationDMO.MI_Id = data.MI_Id;
                        rating_Master_classificationDMO.RATMT_Id = data.RATMT_Id;
                        rating_Master_classificationDMO.RATMQECL_QuestionsClassification = data.RATMQECL_QuestionsClassification;
                        rating_Master_classificationDMO.RATMQECL_RQOrder = ResultOrder + 1;
                        rating_Master_classificationDMO.RATMQECL_ActiveFlag = true;
                        rating_Master_classificationDMO.RATMQECL_CreatedBy = data.UserId;
                        rating_Master_classificationDMO.RATMQECL_CreatedDate = indiantime0;
                        rating_Master_classificationDMO.RATMQECL_UpdatedBy = data.UserId;
                        rating_Master_classificationDMO.RATMQECL_UpdatedDate = indiantime0;
                        _context.Add(rating_Master_classificationDMO);

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
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

        public Rating_Master_DTO MasterClassificationQuestionActiveDeactive(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Result = _context.Rating_Master_Question_ClassificationDMO.Single(a => a.RATMQECL_Id == data.RATMQECL_Id && a.MI_Id == data.MI_Id);

                Result.RATMQECL_ActiveFlag = Result.RATMQECL_ActiveFlag == true ? false : true;
                Result.RATMQECL_UpdatedBy = data.UserId;
                Result.RATMQECL_UpdatedDate = indiantime0;
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

        public Rating_Master_DTO EditMasterQuestionClassification(Rating_Master_DTO data)
        {
            try
            {
                data.GetQuestionClassificationEditData = _context.Rating_Master_Question_ClassificationDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMQECL_Id == data.RATMQECL_Id).ToArray();

                // data.CheckRatingTypeMapped = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_Id == data.RATMT_Id).ToArray();

                data.CheckRatingQuestionClassificationMapped = (
                    from a in _context.Rating_Master_Question_ClassificationDMO
                    from b in _context.Rating_Master_Question_SubClassificationDMO
                    where (b.RATMQECL_Id == a.RATMQECL_Id && a.MI_Id == data.MI_Id && a.RATMT_Id == data.RATMT_Id
                                                                 && b.RATMQECLSUB_ActiveFlag == true)
                    select b).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Master Sub-Classification

        public Rating_Master_DTO OnClickRatingSubClassification(Rating_Master_DTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                data.getQuestionClassificationData = _context.Rating_Master_Question_ClassificationDMO.Where(a => a.RATMQECL_ActiveFlag == true && a.MI_Id == data.MI_Id).ToArray();

                data.getMasterQuestionsubClassificationLoadData = (from a in _context.Rating_Master_Question_SubClassificationDMO
                                                                   from b in _context.Rating_Master_Question_ClassificationDMO
                                                                   where (a.RATMQECL_Id == b.RATMQECL_Id && b.MI_Id == data.MI_Id)
                                                                   select new Rating_Master_DTO
                                                                   {

                                                                       RATMQECL_Id = a.RATMQECL_Id,
                                                                       MI_Id = b.MI_Id,
                                                                       RATMQECL_QuestionsClassification = b.RATMQECL_QuestionsClassification,
                                                                       RATMQECLSUB_Id = a.RATMQECLSUB_Id,
                                                                       RATMQECLSUB_QuestionsSubclassification = a.RATMQECLSUB_QuestionsSubclassification,
                                                                       RATMQECLSUB_RQOrder = a.RATMQECLSUB_RQOrder,
                                                                       RATMQECLSUB_ActiveFlag = a.RATMQECLSUB_ActiveFlag,
                                                                       CreatedBy = a.RATMQECLSUB_CreatedBy,
                                                                       CreatedDate = a.RATMQECLSUB_CreatedDate,
                                                                       MappedCount = (_context.Ratings_Master_QuestionsDMO.Where(b => b.RATMQECLSUB_Id == a.RATMQECLSUB_Id && b.MI_Id == data.MI_Id
                                                                                 && b.RATMQE_ActiveFlag == true).Count())
                                                                   }).Distinct().OrderBy(a => a.CreatedDate).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_Master_DTO SaveRatingsubClassification(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.RATMQECLSUB_Id > 0)
                {
                    data.returnmsg = "Update";
                    data.returnval = false;

                    var CheckDuplicate = _context.Rating_Master_Question_SubClassificationDMO.Where(a => a.RATMQECLSUB_QuestionsSubclassification.Equals(data.RATMQECLSUB_QuestionsSubclassification)
                                                                                                    && a.RATMQECLSUB_Id != data.RATMQECLSUB_Id).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        var Result = _context.Rating_Master_Question_SubClassificationDMO.Single(a => a.RATMQECL_Id == data.RATMQECL_Id && a.RATMQECLSUB_Id == data.RATMQECLSUB_Id);
                        Result.RATMQECL_Id = data.RATMQECL_Id;
                        Result.RATMQECLSUB_QuestionsSubclassification = data.RATMQECLSUB_QuestionsSubclassification;
                        Result.RATMQECLSUB_UpdatedBy = data.UserId;
                        Result.RATMQECLSUB_UpdatedDate = indiantime0;
                        _context.Update(Result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                }
                else
                {
                    data.returnmsg = "Add";
                    data.returnval = false;

                    var ResultOrder = _context.Rating_Master_Question_SubClassificationDMO.Where(a => a.RATMQECLSUB_Id == data.RATMQECLSUB_Id).Count();

                    var CheckDuplicate = _context.Rating_Master_Question_SubClassificationDMO.Where(a =>
                     a.RATMQECLSUB_QuestionsSubclassification.Equals(data.RATMQECLSUB_QuestionsSubclassification) && a.RATMQECL_Id == data.RATMQECL_Id).ToList();

                    //var CheckDuplicate = (from a in _context.Rating_Master_Question_SubClassificationDMO
                    //                      from b in _context.Rating_Master_Question_ClassificationDMO
                    //                      where (a.RATMQECL_Id == b.RATMQECL_Id && b.MI_Id == data.MI_Id && a.RATMQECLSUB_QuestionsSubclassification.Equals(data.RATMQECLSUB_QuestionsSubclassification))
                    //                      select a).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        Rating_Master_Question_SubClassificationDMO rating_Master_subclassificationDMO = new Rating_Master_Question_SubClassificationDMO();
                        rating_Master_subclassificationDMO.RATMQECL_Id = data.RATMQECL_Id;
                        rating_Master_subclassificationDMO.RATMQECLSUB_QuestionsSubclassification = data.RATMQECLSUB_QuestionsSubclassification;
                        rating_Master_subclassificationDMO.RATMQECLSUB_RQOrder = ResultOrder + 1;
                        rating_Master_subclassificationDMO.RATMQECLSUB_ActiveFlag = true;
                        rating_Master_subclassificationDMO.RATMQECLSUB_CreatedBy = data.UserId;
                        rating_Master_subclassificationDMO.RATMQECLSUB_CreatedDate = indiantime0;
                        rating_Master_subclassificationDMO.RATMQECLSUB_UpdatedBy = data.UserId;
                        rating_Master_subclassificationDMO.RATMQECLSUB_UpdatedDate = indiantime0;
                        _context.Add(rating_Master_subclassificationDMO);

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
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

        public Rating_Master_DTO MasterSubClassificationQuestionActiveDeactive(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Result = _context.Rating_Master_Question_SubClassificationDMO.Single(a => a.RATMQECLSUB_Id == data.RATMQECLSUB_Id);

                Result.RATMQECLSUB_ActiveFlag = Result.RATMQECLSUB_ActiveFlag == true ? false : true;
                Result.RATMQECLSUB_UpdatedBy = data.UserId;
                Result.RATMQECLSUB_UpdatedDate = indiantime0;
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

        public Rating_Master_DTO EditMasterQuestionSubClassification(Rating_Master_DTO data)
        {
            try
            {
                data.GetQuestionSubClassificationEditData = _context.Rating_Master_Question_SubClassificationDMO.Where(a => a.RATMQECLSUB_Id == data.RATMQECLSUB_Id).ToArray();

                // data.CheckRatingTypeMapped = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_Id == data.RATMT_Id).ToArray();

                data.CheckRatingQuestionSubClassificationMapped = (
                    from a in _context.Rating_Master_Question_SubClassificationDMO
                    from b in _context.Ratings_Master_QuestionsDMO
                    where (b.RATMQECLSUB_Id == a.RATMQECLSUB_Id && b.MI_Id == data.MI_Id && b.RATMQE_ActiveFlag == true)
                    select b).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Rating Authority

        public Rating_Master_DTO OnClickRatingAuthority(Rating_Master_DTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                data.GetDepartmentData = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).ToArray();

                data.getMasterAuthority = (from a in _context.Rating_Master_AuthorityDMO
                                           from b in _context.HR_Master_Department
                                           from c in _context.HR_Master_Designation
                                           where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.MI_Id == data.MI_Id)
                                           select new Rating_Master_DTO
                                           {
                                               RATMAUT_Id = a.RATMAUT_Id,
                                               MI_Id = a.MI_Id,
                                               RATMAUT_ActiveFlag = a.RATMAUT_ActiveFlag,
                                               RATMAUT_CreatedDate = a.RATMAUT_CreatedDate,
                                               HRMD_Id = b.HRMD_Id,
                                               HRMDES_Id = c.HRMDES_Id,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               HRMDES_DesignationName = c.HRMDES_DesignationName

                                           }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_Master_DTO OnChangeRatingDepartment(Rating_Master_DTO data)
        {
            try
            {
                data.getDesginationData = (from a in _context.HRGroupDeptDessgDMO
                                           from b in _context.HR_Master_Designation
                                           where (a.MI_Id == data.MI_Id && a.HRGTDDS_ActiveFlg == true && a.HRMDES_Id == b.HRMDES_Id
                                           && a.HRMD_Id == data.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                           select b).Distinct().ToArray();

                //data.getDesginationData = (from a in _context.HR_Master_Department
                //                           from b in _context.HR_Master_Designation
                //                           from c in _context.HR_Master_DepartmentCodeDMO
                //                           where (a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true && b.HRMDES_ActiveFlag == true
                //                           && a.HRMD_Id == data.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && a.HRMDC_ID == c.HRMDC_ID && b.HRMDC_ID == c.HRMDC_ID)
                //                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_Master_DTO SaveRatingAuthority(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.RATMAUT_Id > 0)
                {
                    data.returnmsg = "Update";
                    data.returnval = false;

                    var CheckDuplicate = _context.Rating_Master_AuthorityDMO.Where(a => a.HRMD_Id == data.HRMD_Id && a.HRMDES_Id == data.HRMDES_Id && a.MI_Id == data.MI_Id).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        var Result = _context.Rating_Master_AuthorityDMO.Single(a => a.RATMAUT_Id == data.RATMAUT_Id);
                        Result.RATMAUT_Id = data.RATMAUT_Id;
                        Result.MI_Id = data.MI_Id;
                        Result.HRMD_Id = data.HRMD_Id;
                        Result.HRMDES_Id = data.HRMDES_Id;
                        Result.RATMAUT_UpdatedDate = indiantime0;
                        Result.RATMAUT_UpdatedBy = data.UserId;
                        _context.Update(Result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                }
                else
                {
                    data.returnmsg = "Add";
                    data.returnval = false;
                    for (int j = 0; j < data.designationlistarray.Length; j++)
                    {
                        var CheckDuplicate = _context.Rating_Master_AuthorityDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id && a.HRMDES_Id == data.designationlistarray[j].HRMDES_Id).ToList();

                        if (CheckDuplicate.Count > 0)
                        {
                            //data.returnmsg = "Duplicate";
                        }
                        else
                        {

                            Rating_Master_AuthorityDMO rating_Master_AuthorityDMO = new Rating_Master_AuthorityDMO();
                            rating_Master_AuthorityDMO.RATMAUT_Id = data.RATMQECL_Id;
                            rating_Master_AuthorityDMO.MI_Id = data.MI_Id;
                            rating_Master_AuthorityDMO.HRMD_Id = data.HRMD_Id;
                            rating_Master_AuthorityDMO.HRMDES_Id = data.designationlistarray[j].HRMDES_Id;
                            rating_Master_AuthorityDMO.RATMAUT_ActiveFlag = true;
                            rating_Master_AuthorityDMO.RATMAUT_CreatedBy = data.UserId;
                            rating_Master_AuthorityDMO.RATMAUT_CreatedDate = indiantime0;
                            rating_Master_AuthorityDMO.RATMAUT_UpdatedBy = data.UserId;
                            rating_Master_AuthorityDMO.RATMAUT_UpdatedDate = indiantime0;
                            _context.Add(rating_Master_AuthorityDMO);
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

        public Rating_Master_DTO MasterAuthorityActiveDeactive(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Ratingauthority = (from a in _context.Ratings_Master_Authority_QuestionsDMO
                                       from b in _context.Rating_Master_AuthorityDMO
                                       where (a.RATMAUT_Id == b.RATMAUT_Id && b.RATMAUT_Id == data.RATMAUT_Id && b.MI_Id == data.MI_Id && a.RATMAUTTYQ_ActiveFlag == true)
                                       select a).ToList();


                if (Ratingauthority.Count > 0)
                {
                    data.returnmsg = "Mapped";
                }
                else
                {
                    var Result = _context.Rating_Master_AuthorityDMO.Single(a => a.RATMAUT_Id == data.RATMAUT_Id && a.MI_Id == data.MI_Id);

                    Result.RATMAUT_ActiveFlag = Result.RATMAUT_ActiveFlag == true ? false : true;
                    Result.RATMAUT_UpdatedBy = data.UserId;
                    Result.RATMAUT_UpdatedDate = indiantime0;
                    _context.Update(Result);

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

        //Master Question
        public Rating_Master_DTO OnClickMasterQuestions(Rating_Master_DTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    //var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    //List<long> miids = new List<long>();

                    //foreach (var c in getuserinstitution)
                    //{
                    //    miids.Add(c.MI_Id);
                    //}

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                data.getQuestionSubClassificationdata = (from a in _context.Rating_Master_Question_SubClassificationDMO
                                                         from b in _context.Rating_Master_Question_ClassificationDMO
                                                         where (a.RATMQECL_Id == b.RATMQECL_Id && a.RATMQECLSUB_ActiveFlag == true && b.RATMQECL_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                                         select new Rating_Master_DTO
                                                         {
                                                             RATMQECLSUB_Id = a.RATMQECLSUB_Id,
                                                             RATMQECLSUB_QuestionsSubclassification = a.RATMQECLSUB_QuestionsSubclassification
                                                         }).ToArray();

                data.GetMasterQuestionLoadData = (from a in _context.Ratings_Master_QuestionsDMO
                                                  where (a.MI_Id == data.MI_Id)
                                                  select new Rating_Master_DTO
                                                  {
                                                      RATMQE_Id = a.RATMQE_Id,
                                                      MI_Id = a.MI_Id,
                                                      RATMQE_RatingQuestions = a.RATMQE_RatingQuestions,
                                                      RATMQE_ManualEntryFlg = a.RATMQE_ManualEntryFlg,
                                                      RATMQE_AutoEntryFlg = a.RATMQE_AutomaticFlg,
                                                      RATMQE_RatingsQRemarks = a.RATMQE_RatingsQRemarks,
                                                      RATMQE_QuestionFlg = a.RATMQE_QuestionFlg,
                                                      RATMQE_RQOrder = a.RATMQE_RQOrder,
                                                      RATMQE_ActiveFlag = a.RATMQE_ActiveFlag,
                                                      CreatedBy = a.RATMQE_CreatedBy,
                                                      CreatedDate = a.RATMQE_CreatedDate,
                                                      MappedCount = (_context.Rating_TransactionDMO.Where(b => b.RATMQE_Id == a.RATMQE_Id && b.MI_Id == data.MI_Id
                                                                && b.RATTR_ActiveFlag == true).Count())
                                                  }).Distinct().OrderBy(a => a.CreatedDate).ToArray();


                _context.Ratings_Master_QuestionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();

                data.GetMasterQuestionOrderData = _context.Ratings_Master_QuestionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).OrderBy(a => a.RATMQE_RQOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO SaveMasterQuestion(Rating_Master_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.RATMQE_Id > 0)
                {
                    data.returnmsg = "Update";
                    data.returnval = false;

                    var CheckDuplicate = _context.Ratings_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMQE_RatingQuestions.Equals(data.RATMQE_RatingQuestions) && a.RATMQE_Id != data.RATMQE_Id).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        var Result = _context.Ratings_Master_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.RATMQE_Id == data.RATMQE_Id);
                        Result.RATMQECLSUB_Id = data.RATMQECLSUB_Id;
                        Result.RATMQE_RatingQuestions = data.RATMQE_RatingQuestions;
                        Result.RATMQE_QuestionFlg = data.RATMQE_QuestionFlg;
                        Result.RATMQE_ManualEntryFlg = data.RATMQE_ManualEntryFlg;
                        Result.RATMQE_AutomaticFlg = data.RATMQE_AutoEntryFlg;
                        Result.RATMQE_RatingsQRemarks = data.RATMQE_RatingsQRemarks;
                        Result.RATMQE_UpdatedBy = data.UserId;
                        Result.RATMQE_UpdatedDate = indiantime0;
                        _context.Update(Result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                }
                else
                {
                    data.returnmsg = "Add";
                    data.returnval = false;

                    var ResultOrder = _context.Ratings_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    var CheckDuplicate = _context.Ratings_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMQE_RatingQuestions.Equals(data.RATMQE_RatingQuestions)).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        Ratings_Master_QuestionsDMO ratings_Master_QuestionsDMO = new Ratings_Master_QuestionsDMO();
                        ratings_Master_QuestionsDMO.MI_Id = data.MI_Id;
                        ratings_Master_QuestionsDMO.RATMQE_RatingQuestions = data.RATMQE_RatingQuestions;
                        ratings_Master_QuestionsDMO.RATMQE_ManualEntryFlg = data.RATMQE_ManualEntryFlg;
                        ratings_Master_QuestionsDMO.RATMQE_RatingsQRemarks = data.RATMQE_RatingsQRemarks;
                        ratings_Master_QuestionsDMO.RATMQE_RQOrder = ResultOrder + 1;
                        ratings_Master_QuestionsDMO.RATMQE_ActiveFlag = true;
                        ratings_Master_QuestionsDMO.RATMQE_AutomaticFlg = data.RATMQE_AutoEntryFlg;
                        ratings_Master_QuestionsDMO.RATMQE_CreatedBy = data.UserId;
                        ratings_Master_QuestionsDMO.RATMQE_CreatedDate = indiantime0;
                        ratings_Master_QuestionsDMO.RATMQE_UpdatedBy = data.UserId;
                        ratings_Master_QuestionsDMO.RATMQE_UpdatedDate = indiantime0;
                        ratings_Master_QuestionsDMO.RATMQE_QuestionFlg = data.RATMQE_QuestionFlg;
                        ratings_Master_QuestionsDMO.RATMQECLSUB_Id = data.RATMQECLSUB_Id;
                        _context.Add(ratings_Master_QuestionsDMO);

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
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
        public Rating_Master_DTO EditMasterQuestions(Rating_Master_DTO data)
        {
            try
            {
                data.GetMaterQuestionEditData = _context.Ratings_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMQE_Id == data.RATMQE_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO MasteQuestionActiveDeactive(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Result = _context.Ratings_Master_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.RATMQE_Id == data.RATMQE_Id);
                Result.RATMQE_ActiveFlag = Result.RATMQE_ActiveFlag == true ? false : true;
                Result.RATMQE_UpdatedBy = data.UserId;
                Result.RATMQE_UpdatedDate = indiantime0;
                _context.Update(Result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }

                data.GetMasterQuestionLoadData = _context.Ratings_Master_QuestionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO UpdateMasterQuestionOrder(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                foreach (var c in data.Temp_Question_Order)
                {
                    var Result = _context.Ratings_Master_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.RATMQE_Id == c.RATMQE_Id);
                    Result.RATMQE_RQOrder = c.RATMQE_RQOrder;
                    Result.RATMQE_UpdatedBy = data.UserId;
                    Result.RATMQE_UpdatedDate = indiantime0;
                    _context.Update(Result);
                }

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

        //Master Option
        public Rating_Master_DTO OnClickMasterOption(Rating_Master_DTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    //var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    //List<long> miids = new List<long>();

                    //foreach (var c in getuserinstitution)
                    //{
                    //    miids.Add(c.MI_Id);
                    //}

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                data.GetMasterOptionLoadData = (from a in _context.Rating_Master_OptionsDMO
                                                where (a.MI_Id == data.MI_Id)
                                                select new Rating_Master_DTO
                                                {
                                                    RATMOP_Id = a.RATMOP_Id,
                                                    MI_Id = a.MI_Id,
                                                    RATMOP_RatingOptions = a.RATMOP_RatingOptions,
                                                    RATMOP_OptionsValue = a.RATMOP_OptionsValue,
                                                    RATMOP_RatingORemarks = a.RATMOP_RatingORemarks,
                                                    RATMOP_ROOrder = a.RATMOP_ROOrder,
                                                    RATMOP_ActiveFlag = a.RATMOP_ActiveFlag,
                                                    CreatedBy = a.RATMOP_CreatedBy,
                                                    CreatedDate = a.RATMOP_CreatedDate,
                                                    MappedCount = (
                                                                   from b in _context.Rating_Type_QuestionsDMO
                                                                   from c in _context.Rating_Type_Questions_OptionsDMO
                                                                   where
                                                                   (
                                                                    b.RATMTQ_Id == c.RATMTQ_Id
                                                                   && c.RATMOP_Id == a.RATMOP_Id
                                                                   && b.MI_Id == data.MI_Id
                                                                   && b.RATMTQ_ActiveFlag == true)
                                                                   select b).Distinct().Count()


                                                }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();

                _context.Rating_Master_OptionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();

                data.GetMasterOptionOrderData = _context.Rating_Master_OptionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).OrderBy(a => a.RATMOP_ROOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO SaveMasterOption(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.RATMOP_Id > 0)
                {
                    data.returnmsg = "Update";
                    data.returnval = false;

                    var CheckDuplicate = _context.Rating_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMOP_RatingOptions.Equals(data.RATMOP_RatingOptions) && a.RATMOP_Id != data.RATMOP_Id).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        var Result = _context.Rating_Master_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.RATMOP_Id == data.RATMOP_Id);
                        Result.RATMOP_OptionsValue = data.RATMOP_OptionsValue;
                        Result.RATMOP_RatingOptions = data.RATMOP_RatingOptions;
                        Result.RATMOP_RatingORemarks = data.RATMOP_RatingORemarks;
                        Result.RATMOP_UpdatedBy = data.UserId;
                        Result.RATMOP_UpdatedDate = indiantime0;
                        _context.Update(Result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                }
                else
                {
                    data.returnmsg = "Add";
                    data.returnval = false;

                    var ResultOrder = _context.Rating_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    var CheckDuplicate = _context.Rating_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMOP_RatingOptions.Equals(data.RATMOP_RatingOptions)).ToList();

                    if (CheckDuplicate.Count > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        Rating_Master_OptionsDMO rating_Master_OptionsDMO = new Rating_Master_OptionsDMO();
                        rating_Master_OptionsDMO.MI_Id = data.MI_Id;
                        rating_Master_OptionsDMO.RATMOP_RatingOptions = data.RATMOP_RatingOptions;
                        rating_Master_OptionsDMO.RATMOP_OptionsValue = data.RATMOP_OptionsValue;
                        rating_Master_OptionsDMO.RATMOP_RatingORemarks = data.RATMOP_RatingORemarks;
                        rating_Master_OptionsDMO.RATMOP_ROOrder = ResultOrder + 1;
                        rating_Master_OptionsDMO.RATMOP_ActiveFlag = true;
                        rating_Master_OptionsDMO.RATMOP_CreatedBy = data.UserId;
                        rating_Master_OptionsDMO.RATMOP_CreatedDate = indiantime0;
                        rating_Master_OptionsDMO.RATMOP_UpdatedBy = data.UserId;
                        rating_Master_OptionsDMO.RATMOP_UpdatedDate = indiantime0;
                        _context.Add(rating_Master_OptionsDMO);

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
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
        public Rating_Master_DTO EditMasterOption(Rating_Master_DTO data)
        {
            try
            {
                data.GetMasterOptionEditData = _context.Rating_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMOP_Id == data.RATMOP_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO MasteOptionActiveDeactive(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Result = _context.Rating_Master_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.RATMOP_Id == data.RATMOP_Id);
                Result.RATMOP_ActiveFlag = Result.RATMOP_ActiveFlag == true ? false : true;
                Result.RATMOP_UpdatedBy = data.UserId;
                Result.RATMOP_UpdatedDate = indiantime0;
                _context.Update(Result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                data.GetMasterOptionLoadData = _context.Ratings_Master_QuestionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO UpdateMasterOptionOrder(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                foreach (var c in data.Temp_Option_Order)
                {
                    var Result = _context.Rating_Master_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.RATMOP_Id == c.RATMOP_Id);
                    Result.RATMOP_ROOrder = c.RATMOP_ROOrder;
                    Result.RATMOP_UpdatedBy = data.UserId;
                    Result.RATMOP_UpdatedDate = indiantime0;
                    _context.Update(Result);
                }

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

        //Type Question Mapping
        public Rating_Master_DTO OnClickTypeQuestionMapping(Rating_Master_DTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {
                    //var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    //List<long> miids = new List<long>();

                    //foreach (var c in getuserinstitution)
                    //{
                    //    miids.Add(c.MI_Id);
                    //}

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                    data.GetMasterTypeData = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_ActiveFlag == true).Distinct().ToArray();

                    data.GetMasterQuestionData = _context.Ratings_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.RATMQE_ActiveFlag == true).Distinct().OrderBy(a => a.RATMQE_RQOrder).ToArray();

                    data.GetDepartmentData = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id
                     && a.HRMD_ActiveFlag == true).OrderBy(a => a.HRMD_DepartmentName).ToArray();


                    data.getAuthority = (from a in _context.Rating_Master_AuthorityDMO
                                         from b in _context.HR_Master_Department
                                         from c in _context.HR_Master_Designation
                                         where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.MI_Id == data.MI_Id)
                                         select new Rating_Master_DTO
                                         {
                                             RATMAUT_Id = a.RATMAUT_Id,
                                             HRMD_DepartmentName = b.HRMD_DepartmentName + " - " + c.HRMDES_DesignationName
                                         }).ToArray();

                    //data.GetTypeMasterQuestionMapping = (from a in _context.Rating_Master_TypeDMO
                    //                                     from b in _context.Ratings_Master_QuestionsDMO
                    //                                     from c in _context.Rating_Type_QuestionsDMO
                    //                                     from d in _context.Ratings_Master_Questions_AuthorityDMO
                    //                                     where (a.RATMT_Id == c.RATMT_Id && b.RATMQE_Id == c.RATMQE_Id && c.MI_Id == data.MI_Id
                    //                                     && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                    //                                     select new Rating_Master_DTO
                    //                                     {
                    //                                         RATMQE_RatingQuestions = b.RATMQE_RatingQuestions,
                    //                                         RATMT_RatingTypeName = a.RATMT_RatingTypeName,
                    //                                         RATMT_Id = a.RATMT_Id,
                    //                                         RATMQE_Id = b.RATMQE_Id,
                    //                                         RATMTQ_Id = c.RATMTQ_Id,
                    //                                         RATMQEAUT_Id = d.RATMQEAUT_Id,
                    //                                         RATMQE_ManualEntryFlg = b.RATMQE_ManualEntryFlg,
                    //                                         RATMTQ_ActiveFlag = c.RATMTQ_ActiveFlag
                    //                                     }).Distinct().ToArray();



                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RatingMasterQuestionOptionMapping";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
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

                            data.getTypeMasterQuestionMapping = retObject.ToArray();



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
        public Rating_Master_DTO OnChangeMasteTypeQuesInsitution(Rating_Master_DTO data)
        {
            try
            {
                data.GetMasterTypeData = _context.Rating_Master_TypeDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_ActiveFlag == true).Distinct().ToArray();

                data.GetMasterQuestionData = _context.Ratings_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id
                && a.RATMQE_ActiveFlag == true).Distinct().OrderBy(a => a.RATMQE_RQOrder).ToArray();

                data.GetDepartmentData = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id
                     && a.HRMD_ActiveFlag == true).OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.getTypeMasterQuestionMapping = (from a in _context.Rating_Master_TypeDMO
                                                     from b in _context.Ratings_Master_QuestionsDMO
                                                     from c in _context.Rating_Type_QuestionsDMO
                                                     where (a.RATMT_Id == c.RATMT_Id && b.RATMQE_Id == c.RATMQE_Id && c.MI_Id == data.MI_Id
                                                     && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                                     select new Rating_Master_DTO
                                                     {
                                                         RATMQE_RatingQuestions = b.RATMQE_RatingQuestions,
                                                         RATMT_RatingTypeName = a.RATMT_RatingTypeName,
                                                         RATMT_Id = a.RATMT_Id,
                                                         RATMQE_Id = b.RATMQE_Id,
                                                         RATMTQ_Id = c.RATMTQ_Id,
                                                         RATMQE_ManualEntryFlg = b.RATMQE_ManualEntryFlg,
                                                         RATMTQ_ActiveFlag = c.RATMTQ_ActiveFlag,
                                                         //RATMQEAUT_Id = (
                                                         //                       from d in _context.Ratings_Master_Questions_AuthorityDMO
                                                         //                       where
                                                         //                       (
                                                         //                        d.RATMQE_Id == b.RATMQE_Id
                                                         //                       && b.MI_Id == data.MI_Id
                                                         //                       && b.RATMQE_ActiveFlag == true)
                                                         //                       select d.RATMQEAUT_Id).FirstOrDefault()
                                                         //RATMQEAUT_Id = (_context.Ratings_Master_Questions_AuthorityDMO.Where(s => s.RATMQE_Id == b.RATMQE_Id && b.MI_Id == data.MI_Id && b.RATMQE_ActiveFlag == true).Select(r => r.RATMQEAUT_Id)).FirstOrDefault()

                                                     }).Distinct().ToArray();


                data.getAuthority = (from a in _context.Rating_Master_AuthorityDMO
                                     from b in _context.HR_Master_Department
                                     from c in _context.HR_Master_Designation
                                     where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id)
                                     select new Rating_Master_DTO
                                     {
                                         RATMAUT_Id = a.RATMAUT_Id,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName + " - " + c.HRMDES_DesignationName
                                     }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO OnChangeQuestion(Rating_Master_DTO data)
        {
            try
            {
                List<long> cateid = new List<long>();
                foreach (var e in data.GetMasterQuestionDatalist)
                {
                    cateid.Add(e.RATMQE_Id);
                }

                data.GetQuestionDeptDesigData = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                                 from b in _context.Rating_Type_QuestionsDMO
                                                 from c in _context.HR_Master_Department
                                                 from d in _context.HR_Master_Designation
                                                 where (a.RATMTQ_Id == b.RATMTQ_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id
                                                 && cateid.Contains(b.RATMQE_Id) && data.RATMT_Array.Contains(b.RATMT_Id))
                                                 select new Rating_Master_DTO
                                                 {
                                                     RATMTQD_Id = a.RATMTQD_Id,
                                                     RATMTQ_Id = a.RATMTQ_Id,
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMDES_Id = a.HRMDES_Id,
                                                     HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                     HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                 }).Distinct().ToArray();


                data.GetMasterOptionData = _context.Rating_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id
                && a.RATMOP_ActiveFlag == true).Distinct().OrderBy(a => a.RATMOP_ROOrder).ToArray();

                data.GetMasterTypeQuestionOptionMapping = (from a in _context.Rating_Type_Questions_OptionsDMO
                                                           from b in _context.Rating_Master_OptionsDMO
                                                           from c in _context.Rating_Type_QuestionsDMO
                                                           from d in _context.Rating_Master_TypeDMO
                                                           from e in _context.Rating_Type_QuestionsDMO
                                                           where (a.RATMOP_Id == b.RATMOP_Id && a.RATMTQ_Id == c.RATMTQ_Id && c.RATMT_Id == d.RATMT_Id
                                                           && c.RATMQE_Id == e.RATMQE_Id && cateid.Contains(c.RATMQE_Id) && c.RATMT_Id == data.RATMT_Id
                                                           && c.MI_Id == data.MI_Id)
                                                           select a).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO OnChangeDepartment(Rating_Master_DTO data)
        {
            try
            {


                data.getDesginationData = (from a in _context.MasterEmployee
                                           from b in _context.HR_Master_Designation
                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id
                                           && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true && a.HRMD_Id == data.HRMD_Id)
                                           select b).Distinct().ToArray();


                //var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                //var hrmd_ids = "0";
                //if (data.Department_Array.Length > 0)
                //{
                //    foreach (var ue in data.Department_Array)
                //    {
                //        hrmd_ids = hrmd_ids + "," + ue.HRMD_Id;

                //    }

                //}


                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "DesgnationDepartmentWise_multi";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@hrmd_ids", SqlDbType.VarChar)
                //    {
                //        Value = hrmd_ids
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                                               );
                //                }

                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }

                //        data.getDesginationData = retObject.ToArray();



                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO SaveTypeQuestion(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);


                if (data.questionlistarray.Length > 0 && data.questionlistarray != null)
                {
                    foreach (var ue in data.questionlistarray)
                    {

                        foreach (var d in data.Temp_Department)
                        {
                            foreach (var e in d.TempDesgin)
                            {
                                var Rating_Type_Questions = _context.Rating_Type_Questions_DepartmentDMO.Where(R => R.RATMTQ_Id == ue.RATMTQ_Id && R.HRMD_Id == d.HRMD_Id && R.HRMDES_Id == e.HRMDES_Id).ToList();
                                if (Rating_Type_Questions.Count > 0)
                                {
                                    foreach (var M in Rating_Type_Questions)
                                    {
                                        _context.Remove(M);
                                    }
                                }

                                Rating_Type_Questions_DepartmentDMO rating_Type_Questions_DepartmentDMO = new Rating_Type_Questions_DepartmentDMO();
                                rating_Type_Questions_DepartmentDMO.RATMTQ_Id = ue.RATMTQ_Id;
                                rating_Type_Questions_DepartmentDMO.HRMD_Id = d.HRMD_Id;
                                rating_Type_Questions_DepartmentDMO.HRMDES_Id = e.HRMDES_Id;
                                rating_Type_Questions_DepartmentDMO.RATMTQD_ActiveFlag = true;
                                rating_Type_Questions_DepartmentDMO.RATMTQD_CreatedBy = data.UserId;
                                rating_Type_Questions_DepartmentDMO.RATMTQD_CreatedDate = indiantime0;
                                rating_Type_Questions_DepartmentDMO.RATMTQD_UpdatedBy = data.UserId;
                                rating_Type_Questions_DepartmentDMO.RATMTQD_UpdatedDate = indiantime0;
                                _context.Add(rating_Type_Questions_DepartmentDMO);
                            }
                        }
                        // Type Question Wise Option
                        if (data.Temp_Option != null && data.Temp_Option.Length > 0)
                        {

                            var count = 0;
                            foreach (var c in data.Temp_Option)
                            {
                                var duplicate = _context.Rating_Type_Questions_OptionsDMO.Where(a => a.RATMTQ_Id == ue.RATMTQ_Id && a.RATMOP_Id == c.RATMOP_Id).ToList();
                                if (duplicate.Count > 0)
                                {

                                }
                                else
                                {
                                    count += 1;
                                    Rating_Type_Questions_OptionsDMO rating_Type_Questions_OptionsDMO = new Rating_Type_Questions_OptionsDMO();
                                    rating_Type_Questions_OptionsDMO.RATMTQ_Id = ue.RATMTQ_Id;
                                    rating_Type_Questions_OptionsDMO.RATMOP_Id = c.RATMOP_Id;
                                    rating_Type_Questions_OptionsDMO.RATMTQO_TOOOrder = count;
                                    rating_Type_Questions_OptionsDMO.RATMTQO_ActiveFlag = true;
                                    rating_Type_Questions_OptionsDMO.RATMTQO_CreatedBy = data.UserId;
                                    rating_Type_Questions_OptionsDMO.RATMTQO_CreatedDate = indiantime0;
                                    rating_Type_Questions_OptionsDMO.RATMTQO_UpdatedBy = data.UserId;
                                    rating_Type_Questions_OptionsDMO.RATMTQO_UpdatedDate = indiantime0;
                                    _context.Add(rating_Type_Questions_OptionsDMO);
                                }

                            }
                        }
                        //AKASH
                        if (data.CheckBoxDeptDes == true)
                        {
                            var Authority = _context.Ratings_Master_Authority_QuestionsDMO.Where(R => R.RATMQE_Id == ue.RATMQE_Id && R.RATMT_Id == ue.RATMT_Id && R.RATMAUT_Id == data.RATMAUT_Id).ToList();
                            if (Authority.Count > 0)
                            {

                            }
                            else
                            {
                                Ratings_Master_Authority_QuestionsDMO Ratings_Master_Authority_Question = new Ratings_Master_Authority_QuestionsDMO();
                                //Ratings_Master_Authority_Question.RATMAUTTYQ_Id = data.RATMAUTTYQ_Id;
                                Ratings_Master_Authority_Question.RATMAUT_Id = data.RATMAUT_Id;
                                Ratings_Master_Authority_Question.RATMQE_Id = ue.RATMQE_Id;
                                Ratings_Master_Authority_Question.RATMT_Id = ue.RATMT_Id;
                                Ratings_Master_Authority_Question.RATMAUTTYQ_ActiveFlag = true;
                                Ratings_Master_Authority_Question.RATMAUTTYQ_CreatedBy = data.UserId;
                                Ratings_Master_Authority_Question.RATMAUTTYQ_CreatedDate = indiantime0;
                                Ratings_Master_Authority_Question.RATMAUTTYQ_UpdatedBy = data.UserId;
                                Ratings_Master_Authority_Question.RATMAUTTYQ_UpdatedDate = indiantime0;
                                _context.Add(Ratings_Master_Authority_Question);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO ViewTypeQuestionDeptMapping(Rating_Master_DTO data)
        {
            try
            {
                data.ViewQuestionDeptDesigData = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                                  from b in _context.Rating_Type_QuestionsDMO
                                                  from c in _context.HR_Master_Department
                                                  from d in _context.HR_Master_Designation

                                                  where (a.RATMTQ_Id == b.RATMTQ_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id
                                                  && a.RATMTQ_Id == data.RATMTQ_Id)
                                                  select new Rating_Master_DTO
                                                  {
                                                      RATMTQD_Id = a.RATMTQD_Id,
                                                      RATMTQ_Id = a.RATMTQ_Id,
                                                      HRMD_Id = a.HRMD_Id,
                                                      HRMDES_Id = a.HRMDES_Id,
                                                      HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                      HRMDC_ID = c.HRMDC_ID,
                                                      HRMDES_DesignationName = d.HRMDES_DesignationName,

                                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO ViewTypeQuestionOptionMapping(Rating_Master_DTO data)
        {
            try
            {
                data.ViewMasterTypeQuestionOptionMapping = (from a in _context.Rating_Type_Questions_OptionsDMO
                                                            from b in _context.Rating_Master_OptionsDMO
                                                            from c in _context.Rating_Type_QuestionsDMO
                                                            from d in _context.Rating_Master_TypeDMO
                                                            from e in _context.Rating_Type_QuestionsDMO
                                                            where (a.RATMOP_Id == b.RATMOP_Id && a.RATMTQ_Id == c.RATMTQ_Id && c.RATMT_Id == d.RATMT_Id
                                                            && c.RATMQE_Id == e.RATMQE_Id && a.RATMTQ_Id == data.RATMTQ_Id && c.MI_Id == data.MI_Id)
                                                            select new Rating_Master_DTO
                                                            {
                                                                RATMOP_RatingOptions = b.RATMOP_RatingOptions,
                                                                RATMOP_OptionsValue = b.RATMOP_OptionsValue,
                                                                RATMOP_Id = b.RATMOP_Id,
                                                                RATMTQO_Id = a.RATMTQO_Id,
                                                                RATMTQ_Id = a.RATMTQ_Id
                                                            }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_Master_DTO ViewAuthoritydata(Rating_Master_DTO data)
        {
            try
            {
                //data.ViewAuthoritydata = (from a in _context.HR_Master_DepartmentCode_Head
                //                          from b in _context.MasterEmployee
                //                          from c in _context.HR_Master_Department
                //                          from d in _context.HR_Master_Designation
                //                          where (a.HRME_ID == b.HRME_Id && c.HRMD_Id == b.HRMD_Id && d.HRMDES_Id == b.HRMDES_Id
                //                          && a.HRMDC_ID == data.HRMDC_ID)
                //                          select new Rating_Master_DTO
                //                          {
                //                              HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + b.HRME_EmployeeLastName,
                //                              HRMD_DepartmentName = c.HRMD_DepartmentName,
                //                              HRMDES_DesignationName = d.HRMDES_DesignationName
                //                          }).Distinct().ToArray();


                data.ViewAuthoritydata = (from a in _context.HR_Master_DepartmentCode_Head
                                          from b in _context.MasterEmployee
                                          from c in _context.HR_Master_Department
                                          from d in _context.HR_Master_Designation
                                          where (a.HRME_ID == b.HRME_Id && c.HRMD_Id == b.HRMD_Id && d.HRMDES_Id == b.HRMDES_Id
                                          && a.HRMDC_ID == data.HRMDC_ID && b.HRME_ActiveFlag == true)
                                          select new Rating_Master_DTO
                                          {
                                              HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + b.HRME_EmployeeLastName,
                                              HRMD_DepartmentName = c.HRMD_DepartmentName,
                                              HRMDES_DesignationName = d.HRMDES_DesignationName
                                          }).Distinct().ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




        public Rating_Master_DTO MasteTypeQuesOptionActiveDeactive(Rating_Master_DTO data)


        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _context.Rating_Type_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.RATMTQ_Id == data.RATMTQ_Id);
                result.RATMTQ_ActiveFlag = result.RATMTQ_ActiveFlag == true ? false : true;
                result.RATMTQ_UpdatedBy = data.UserId;
                result.RATMTQ_UpdatedDate = indiantime0;
                _context.Update(result);

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
        public Rating_Master_DTO DeleteViewTypeQuestionDept(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                Array RemoveDept = _context.Rating_Type_Questions_DepartmentDMO.Where(a => a.HRMD_Id == data.HRMD_Id && a.RATMTQ_Id == data.RATMTQ_Id).ToArray();

                foreach (var d in RemoveDept)
                {
                    _context.Remove(d);
                }

                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }

                data.ViewQuestionDeptDesigData = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                                  from b in _context.Rating_Type_QuestionsDMO
                                                  from c in _context.HR_Master_Department
                                                  from d in _context.HR_Master_Designation
                                                  where (a.RATMTQ_Id == b.RATMTQ_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id
                                                  && a.RATMTQ_Id == data.RATMTQ_Id)
                                                  select new Rating_Master_DTO
                                                  {
                                                      RATMTQD_Id = a.RATMTQD_Id,
                                                      RATMTQ_Id = a.RATMTQ_Id,
                                                      HRMD_Id = a.HRMD_Id,
                                                      HRMDES_Id = a.HRMDES_Id,
                                                      HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                      HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO DeleteViewTypeQuestionDeptDes(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                Array RemoveDept = _context.Rating_Type_Questions_DepartmentDMO.Where(a => a.HRMD_Id == data.HRMD_Id && a.HRMDES_Id == data.HRMDES_Id
                && a.RATMTQ_Id == data.RATMTQ_Id).ToArray();

                foreach (var d in RemoveDept)
                {
                    _context.Remove(d);
                }

                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }

                data.ViewQuestionDeptDesigData = (from a in _context.Rating_Type_Questions_DepartmentDMO
                                                  from b in _context.Rating_Type_QuestionsDMO
                                                  from c in _context.HR_Master_Department
                                                  from d in _context.HR_Master_Designation
                                                  where (a.RATMTQ_Id == b.RATMTQ_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id
                                                  && a.RATMTQ_Id == data.RATMTQ_Id)
                                                  select new Rating_Master_DTO
                                                  {
                                                      RATMTQD_Id = a.RATMTQD_Id,
                                                      RATMTQ_Id = a.RATMTQ_Id,
                                                      HRMD_Id = a.HRMD_Id,
                                                      HRMDES_Id = a.HRMDES_Id,
                                                      HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                      HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO DeleteOptionRecord(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                Array RemoveDept = _context.Rating_Type_Questions_OptionsDMO.Where(a => a.RATMTQO_Id == data.RATMTQO_Id && a.RATMTQ_Id == data.RATMTQ_Id).ToArray();

                foreach (var d in RemoveDept)
                {
                    _context.Remove(d);
                }

                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }

                data.ViewMasterTypeQuestionOptionMapping = (from a in _context.Rating_Type_Questions_OptionsDMO
                                                            from b in _context.Rating_Master_OptionsDMO
                                                            from c in _context.Rating_Type_QuestionsDMO
                                                            from d in _context.Rating_Master_TypeDMO
                                                            from e in _context.Rating_Type_QuestionsDMO
                                                            where (a.RATMOP_Id == b.RATMOP_Id && a.RATMTQ_Id == c.RATMTQ_Id && c.RATMT_Id == d.RATMT_Id
                                                            && c.RATMQE_Id == e.RATMQE_Id && a.RATMTQ_Id == data.RATMTQ_Id && c.MI_Id == data.MI_Id)
                                                            select new Rating_Master_DTO
                                                            {
                                                                RATMOP_RatingOptions = b.RATMOP_RatingOptions,
                                                                RATMOP_OptionsValue = b.RATMOP_OptionsValue,
                                                                RATMOP_Id = b.RATMOP_Id,
                                                                RATMTQO_Id = a.RATMTQO_Id
                                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_Master_DTO MastetTypeQuestionActiveDeactive(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var RatingType = _context.Rating_Type_QuestionsDMO.Where(a => a.RATMT_Id == data.RATMT_Id && a.MI_Id == data.MI_Id).ToArray();

                for (int j = 0; j < RatingType.Length; j++)
                {

                    var Result = _context.Rating_Type_QuestionsDMO.Single(a => a.RATMTQ_Id == RatingType[j].RATMTQ_Id && a.MI_Id == RatingType[j].MI_Id);

                    //Result.RATMTQ_ActiveFlag = Result.RATMTQ_ActiveFlag == true ? false : true;
                    if (Result.RATMTQ_ActiveFlag == true)
                    {
                        Result.RATMTQ_ActiveFlag = false;
                    }
                    else
                    {
                        Result.RATMTQ_ActiveFlag = true;
                    }
                    Result.RATMTQ_UpdatedBy = data.UserId;
                    Result.RATMTQ_UpdatedDate = indiantime0;
                    _context.Update(Result);
                }
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                data.GetMasterTypeLoadData = _context.Rating_Master_TypeDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Akash
        //SaveOverAllRating

        public Rating_Master_DTO OnChangeComapnyAuthoritys(Rating_Master_DTO data)
        {
            try
            {

                data.GetDepartmentData = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id
                     && a.HRMD_ActiveFlag == true).OrderBy(a => a.HRMD_DepartmentName).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Rating_Master_DTO OnChangeDepartmentAuthority(Rating_Master_DTO data)
        {
            try
            {
                //data.GetDesginationDataAuthority = (from a in _context.MasterEmployee
                //                                    from b in _context.HR_Master_Designation
                //                                    where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id
                //                                    && a.HRMD_Id == data.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                //                                    select b).Distinct().ToArray();

                data.GetDesginationDataAuthority = (from a in _context.HRGroupDeptDessgDMO
                                                    from b in _context.HR_Master_Designation
                                                    where (a.MI_Id == data.MI_Id && a.HRGTDDS_ActiveFlg == true && a.HRMDES_Id == b.HRMDES_Id
                                                    && a.HRMD_Id == data.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                                    select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Rating_Master_DTO SaveOverAllRating(Rating_Master_DTO data)
        {
            try
            {

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                if (data.RATOVERR_Id > 0)
                {
                    data.returnmsg = "Update";
                    data.returnval = false;
                    //var CheckDuplcate = _context.Rating_Overall_RatingDMO.Where(a => a.MI_Id == data.MI_Id
                    //&& a.RATOVERR_FromValue == data.RATOVERR_FromValue &&  a.RATOVERR_Id != data.RATOVERR_Id).ToList();
                    //if (CheckDuplcate.Count == 0)
                    //{
                    var Result = _context.Rating_Overall_RatingDMO.Single(a => a.RATOVERR_Id == data.RATOVERR_Id);
                    Result.MI_Id = data.MI_Id;
                    Result.RATOVERR_FromValue = data.RATOVERR_FromValue;
                    Result.RATOVERR_ToValue = data.RATOVERR_ToValue;
                    Result.RATOVERR_Rating = data.RATOVERR_Rating;
                    //Result.RATOVERR_ActiveFlag = true;
                    //Result.RATOVERR_CreatedBy = data.UserId;
                    //Result.RATOVERR_CreatedDate = indiantime0;
                    Result.RATOVERR_UpdatedBy = data.UserId;
                    Result.RATOVERR_UpdatedDate = indiantime0;


                    _context.Update(Result);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    //}
                    //else
                    //{
                    //    data.returnmsg = "Duplicate";
                    //}
                }
                else
                {
                    data.returnmsg = "Add";


                    //var CheckDuplcate = _context.Rating_Overall_RatingDMO.Where(a => a.MI_Id == data.MI_Id
                    //&& a.RATMT_RatingTypeName == data.RATMT_RatingTypeName).ToList();

                    //if (CheckDuplcate.Count == 0)
                    //{
                    Rating_Overall_RatingDMO Rating_Overall_RatingDMO = new Rating_Overall_RatingDMO();
                    Rating_Overall_RatingDMO.MI_Id = data.MI_Id;
                    Rating_Overall_RatingDMO.RATOVERR_FromValue = data.RATOVERR_FromValue;
                    Rating_Overall_RatingDMO.RATOVERR_ToValue = data.RATOVERR_ToValue;
                    Rating_Overall_RatingDMO.RATOVERR_Rating = data.RATOVERR_Rating;
                    Rating_Overall_RatingDMO.RATOVERR_ActiveFlag = true;
                    Rating_Overall_RatingDMO.RATOVERR_CreatedBy = data.UserId;
                    Rating_Overall_RatingDMO.RATOVERR_CreatedDate = indiantime0;
                    Rating_Overall_RatingDMO.RATOVERR_UpdatedBy = data.UserId;
                    Rating_Overall_RatingDMO.RATOVERR_UpdatedDate = indiantime0;
                    _context.Add(Rating_Overall_RatingDMO);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                    //}
                    //else
                    //{
                    //    data.returnmsg = "Duplicate";
                    //}

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_Master_DTO OverAllEdit(Rating_Master_DTO data)
        {
            try
            {

                data.EditDataOverAll = _context.Rating_Overall_RatingDMO.Where(a => a.MI_Id == data.MI_Id && a.RATOVERR_Id == data.RATOVERR_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_Master_DTO OverAllRatingActiveDeactive(Rating_Master_DTO data)
        {
            try
            {
                data.returnval = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Result = _context.Rating_Overall_RatingDMO.Single(a => a.RATOVERR_Id == data.RATOVERR_Id);
                Result.RATOVERR_ActiveFlag = Result.RATOVERR_ActiveFlag == true ? false : true;
                Result.RATOVERR_UpdatedBy = data.UserId;
                Result.RATOVERR_UpdatedDate = indiantime0;
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
        //ViewTypeAuthorityData
        public Rating_Master_DTO ViewTypeAuthorityData(Rating_Master_DTO data)
        {
            try
            {
                data.viewAuthorities = (from a in _context.Ratings_Master_Questions_AuthorityDMO
                                        from b in _context.Ratings_Master_QuestionsDMO
                                        from c in _context.HR_Master_Designation
                                        from d in _context.HR_Master_Department
                                        where (a.RATMQE_Id == b.RATMQE_Id && a.HRMD_Id == d.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id
                                        && a.RATMQEAUT_Id == data.RATMQEAUT_Id)
                                        select new Rating_Master_DTO
                                        {
                                            RATMQEAUT_Id = a.RATMQEAUT_Id,
                                            RATMQE_RatingQuestions = b.RATMQE_RatingQuestions,
                                            HRMDES_DesignationName = c.HRMDES_DesignationName,
                                            HRMD_DepartmentName = d.HRMD_DepartmentName
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //Rating type and question mapping 
        public Rating_Master_DTO SaveRatingQuestionMapping(Rating_Master_DTO data)
        {
            try
            {


                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.returnmsg = "Add";
                data.returnval = false;

                foreach (var qus in data.questionlistarray)
                {
                    var CheckDuplicate = _context.Rating_Type_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.RATMT_Id == data.RATMT_Id && a.RATMQE_Id == qus.RATMQE_Id).ToList();
                    if (CheckDuplicate.Count == 0)
                    {
                        Rating_Type_QuestionsDMO obj2 = new Rating_Type_QuestionsDMO();
                        obj2.RATMQE_Id = qus.RATMQE_Id;
                        obj2.RATMT_Id = data.RATMT_Id;
                        obj2.MI_Id = data.MI_Id;
                        obj2.RATMTQ_TQOrder = 1;
                        obj2.RATMTQ_ActiveFlag = true;
                        obj2.RATMTQ_CreatedDate = DateTime.Now;
                        obj2.RATMTQ_CreatedBy = data.UserId;
                        _context.Add(obj2);
                    }
                    else
                    {
                        data.returnmsg = "Duplicate";
                    }


                }
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


        public Rating_Master_DTO OnClickRatingtypeQuestionMapping(Rating_Master_DTO data)
        {
            try
            {
                if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
                {

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
                }

                data.getRatingTypeData = _context.Rating_Master_TypeDMO.Where(a => a.RATMT_ActiveFlag == true && a.MI_Id == data.MI_Id).ToArray();

                data.GetRatingTypeQuestionMappingLoadData = (from a in _context.Rating_Type_QuestionsDMO
                                                             from b in _context.Rating_Master_TypeDMO
                                                             where (a.MI_Id == data.MI_Id && b.RATMT_Id == a.RATMT_Id && a.RATMTQ_ActiveFlag == true)
                                                             select new Rating_Master_DTO
                                                             {
                                                                 RATMT_RatingTypeName = b.RATMT_RatingTypeName,
                                                                 RATMT_Id = a.RATMT_Id,
                                                                 MI_Id = a.MI_Id,
                                                                 RATMT_ActiveFlag = b.RATMT_ActiveFlag,

                                                                 MappedCount = (
                                                                                from b in _context.Ratings_Master_QuestionsDMO
                                                                                where
                                                                                (
                                                                                 b.RATMQE_Id == a.RATMQE_Id
                                                                                && b.MI_Id == data.MI_Id
                                                                                && b.RATMQE_ActiveFlag == true)
                                                                                select b).Distinct().Count()


                                                             }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();



                //data.GetMasterOptionOrderData = _context.Rating_Master_OptionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).OrderBy(a => a.RATMOP_ROOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_Master_DTO OnChangeRatingQuestion_Rating(Rating_Master_DTO data)
        {
            try
            {


                data.getRatingTypeQuestionData = _context.Ratings_Master_QuestionsDMO.Where(a => a.RATMQE_ActiveFlag == true && a.MI_Id == data.MI_Id).ToArray();

                //data.GetRatingTypeQuestionMappingLoadData = (from a in _context.Rating_Master_TypeDMO
                //                                where (a.MI_Id == data.MI_Id)
                //                                select new Rating_Master_DTO
                //                                {
                //                                    RATMT_Id = a.RATMT_Id,
                //                                    MI_Id = a.MI_Id,
                //                                    RATMT_RatingTypeName = a.RATMT_RatingTypeName,
                //                                    RATMOP_OptionsValue = a.RATMOP_OptionsValue,
                //                                    RATMOP_RatingORemarks = a.RATMOP_RatingORemarks,
                //                                    RATMOP_ROOrder = a.RATMOP_ROOrder,
                //                                    RATMOP_ActiveFlag = a.RATMOP_ActiveFlag,
                //                                    CreatedBy = a.RATMOP_CreatedBy,
                //                                    CreatedDate = a.RATMOP_CreatedDate,
                //                                    MappedCount = (
                //                                                   from b in _context.Rating_Type_QuestionsDMO
                //                                                   from c in _context.Rating_Type_Questions_OptionsDMO
                //                                                   where
                //                                                   (
                //                                                    b.RATMTQ_Id == c.RATMTQ_Id
                //                                                   && c.RATMOP_Id == a.RATMOP_Id
                //                                                   && b.MI_Id == data.MI_Id
                //                                                   && b.RATMTQ_ActiveFlag == true)
                //                                                   select b).Distinct().Count()


                //                                }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();



                //data.GetMasterOptionOrderData = _context.Rating_Master_OptionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).OrderBy(a => a.RATMOP_ROOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Rating_Master_DTO viewquestions(Rating_Master_DTO data)
        {
            try
            {
                //&& b.MI_Id == data.MI_Id

                data.viewarray = (from a in _context.Rating_Type_QuestionsDMO
                                  from b in _context.Ratings_Master_QuestionsDMO
                                  from c in _context.Rating_Master_TypeDMO
                                  where (a.RATMQE_Id == b.RATMQE_Id && a.RATMT_Id == c.RATMT_Id && a.RATMT_Id == data.RATMT_Id)
                                  select new Rating_Master_DTO
                                  {

                                      RATMQE_RatingQuestions = b.RATMQE_RatingQuestions,
                                      RATMT_RatingTypeName = c.RATMT_RatingTypeName


                                  }).Distinct().OrderBy(a => a.RATMQE_RatingQuestions).ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }


        public Rating_Master_DTO OnChangeRatingQuestionOption_Rating(Rating_Master_DTO data)
        {
            try
            {


                // data.getRatingTypeQuestionData = _context.Ratings_Master_QuestionsDMO.Where(a => a.RATMQE_ActiveFlag == true && a.MI_Id == data.MI_Id).ToArray();




                //data.GetMasterQuestionData = (from a in _context.Rating_Type_QuestionsDMO
                //                              from b in _context.Ratings_Master_QuestionsDMO
                //                              where a.RATMQE_Id == b.RATMQE_Id && a.MI_Id == data.MI_Id && b.RATMQE_ActiveFlag == true && a.RATMT_Id == data.RATMT_Id
                //                              select b
                //                              ).ToArray();


                //data.GetMasterQuestionData = (from a in _context.Rating_Type_QuestionsDMO
                //                              from b in _context.Ratings_Master_QuestionsDMO
                //                              where a.RATMQE_Id == b.RATMQE_Id && a.MI_Id == data.MI_Id && b.RATMQE_ActiveFlag == true && data.RATMT_Array.Contains(a.RATMT_Id)
                //                              select b
                //                              ).ToArray();


                data.GetMasterQuestionData = (from a in _context.Rating_Type_QuestionsDMO
                                              from b in _context.Rating_Master_TypeDMO
                                              from c in _context.Ratings_Master_QuestionsDMO
                                              where (a.RATMT_Id == b.RATMT_Id && a.RATMQE_Id == c.RATMQE_Id && a.MI_Id == data.MI_Id && c.RATMQE_ActiveFlag == true && data.RATMT_Array.Contains(a.RATMT_Id))
                                              select new Rating_Master_DTO
                                              {
                                                  RATMQE_Id = c.RATMQE_Id,
                                                  RATMQE_RatingQuestions = c.RATMQE_RatingQuestions,
                                                  RATMQE_ManualEntryFlg = c.RATMQE_ManualEntryFlg,
                                                  RATMT_Id = b.RATMT_Id,
                                                  RATMTQ_Id = a.RATMTQ_Id

                                              }).ToArray();



                //                data.GetMasterQuestionData = _context.Ratings_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.RATMQE_ActiveFlag == true && a.tmt).Distinct().OrderBy(a => a.RATMQE_RQOrder).ToArray();

                //data.GetRatingTypeQuestionMappingLoadData = (from a in _context.Rating_Master_TypeDMO
                //                                where (a.MI_Id == data.MI_Id)
                //                                select new Rating_Master_DTO
                //                                {
                //                                    RATMT_Id = a.RATMT_Id,
                //                                    MI_Id = a.MI_Id,
                //                                    RATMT_RatingTypeName = a.RATMT_RatingTypeName,
                //                                    RATMOP_OptionsValue = a.RATMOP_OptionsValue,
                //                                    RATMOP_RatingORemarks = a.RATMOP_RatingORemarks,
                //                                    RATMOP_ROOrder = a.RATMOP_ROOrder,
                //                                    RATMOP_ActiveFlag = a.RATMOP_ActiveFlag,
                //                                    CreatedBy = a.RATMOP_CreatedBy,
                //                                    CreatedDate = a.RATMOP_CreatedDate,
                //                                    MappedCount = (
                //                                                   from b in _context.Rating_Type_QuestionsDMO
                //                                                   from c in _context.Rating_Type_Questions_OptionsDMO
                //                                                   where
                //                                                   (
                //                                                    b.RATMTQ_Id == c.RATMTQ_Id
                //                                                   && c.RATMOP_Id == a.RATMOP_Id
                //                                                   && b.MI_Id == data.MI_Id
                //                                                   && b.RATMTQ_ActiveFlag == true)
                //                                                   select b).Distinct().Count()


                //                                }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();



                //data.GetMasterOptionOrderData = _context.Rating_Master_OptionsDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).OrderBy(a => a.RATMOP_ROOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




    }
}