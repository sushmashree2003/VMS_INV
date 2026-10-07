using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Training_Feedback : Interfaces.Training_Feedback_Interface
    {
        public VMSContext _vmsconte;
        public HRMSContext _hrms;

        public Training_Feedback(VMSContext vmsContext, HRMSContext hrms)
        {
            _vmsconte = vmsContext;
            _hrms = hrms;
        }
        //===============================Building==============================================
        public HR_Training_Feedback_DTO loaddata(HR_Training_Feedback_DTO data)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.userId).Select(t => t.Emp_Code).FirstOrDefault();
                data.getmasterdata = (from f in _vmsconte.HR_Master_TrainingTopicDMO
                                      from b in _vmsconte.HR_Master_External_TrainingType_Topic_MaterialsDMO
                                      where (f.HRMTT_Id == b.HRMTT_Id)
                                      select new HR_Master_TrainingTopicDTO
                                      {
                                          HRMTT_Topic = f.HRMTT_Topic,
                                          HRMETRTYTOPM_StudyMaterials = b.HRMETRTYTOPM_StudyMaterials,
                                          HRMETRTYTOPM_StudyMaterialsDesc = b.HRMETRTYTOPM_StudyMaterialsDesc
                                      }).OrderByDescending(a => a.HRMTT_Id).ToArray();

                data.trainerprogdetails = (from a in _vmsconte.HR_Training_Create_DMO_con
                                           from b in _vmsconte.HR_Training_Create_Participants_DMO_con
                                           from c in _vmsconte.HR_Training_Create_IntTrainer_DMO_con
                                           where (a.HRTCR_Id == b.HRTCR_Id && b.HRTCR_Id == c.HRTCR_Id && b.HRME_Id != c.HRME_Id && b.HRME_Id == userhrme_id)
                                           select new HR_Training_Feedback_DTO
                                           {
                                               HRTCR_Id = a.HRTCR_Id,
                                               HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                           }).Distinct().ToArray();

                data.traineeprogdetails = (from a in _vmsconte.HR_Training_Create_DMO_con
                                           from b in _vmsconte.HR_Training_Create_IntTrainer_DMO_con
                                           where (a.HRTCR_Id == b.HRTCR_Id && b.HRME_Id == userhrme_id)
                                           select new HR_Training_Feedback_DTO
                                           {
                                               HRTCR_Id = a.HRTCR_Id,
                                               HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                           }).Distinct().ToArray();

                if (data.traineeprogdetails.Length > 0)
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_TopicFeedbackList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = userhrme_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Trainee"
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
                            data.traninglisttrainee = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                if (data.trainerprogdetails.Length > 0)
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_TopicFeedbackList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = userhrme_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Trainer"
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
                            data.traninglisttrainer = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }









            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public HR_Training_Feedback_DTO getQuestions(HR_Training_Feedback_DTO data)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.userId).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.Type == "Trainee")
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_TraineeOrTrainerFeedbackList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = userhrme_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMTT_Id", SqlDbType.VarChar)
                        {
                            Value = data.HRMTT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.VarChar)
                        {
                            Value = data.HRTCR_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = data.Type
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
                            data.traineelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                if (data.Type == "Trainer")
                {
                    // data.trainerlist = data.traineelist;
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_TraineeOrTrainerFeedbackList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = userhrme_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMTT_Id", SqlDbType.VarChar)
                        {
                            Value = data.HRMTT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.VarChar)
                        {
                            Value = data.HRTCR_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = data.Type
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
                            data.trainerlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }



                data.mappedquestionlist = (from a in _vmsconte.HR_Training_Question_DMO_con
                                           from b in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                           from c in _vmsconte.HR_Master_TrainingTopicDMO
                                           where (a.MI_Id == b.MI_Id && a.HRMTT_Id == c.HRMTT_Id && a.HRMTT_Id == data.HRMTT_Id && a.HRMFQNS_Id == b.HRMFQNS_Id && a.HRTRQNS_ActiveFlg == true
                                           && b.HRMFQNS_QuestionForFlg == data.Type && b.HRMFQNS_ActiveFlg == true)
                                           select new HR_Training_Feedback_DTO
                                           {
                                               HRMFQNS_Id = b.HRMFQNS_Id,
                                               HRMFQNS_QuestionName = b.HRMFQNS_QuestionName,
                                               HRMFQNS_QuestionTypeFlg = b.HRMFQNS_QuestionTypeFlg,
                                           }).Distinct().ToArray();

                data.mappedoptionlist = (from a in _vmsconte.HR_Training_Question_DMO_con
                                         from b in _vmsconte.HR_Master_Question_Option_DMO_con
                                         from c in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                         where (a.MI_Id == b.MI_Id && a.HRMTT_Id == data.HRMTT_Id && a.HRMFQNS_Id == b.HRMFQNS_Id && a.HRTRQNS_ActiveFlg == true && c.HRMFOPT_OptionFor == data.Type && b.HRMQNOP_ActiveFlg == true && b.HRMFOPT_Id == c.HRMFOPT_Id && c.HRMFOPT_ActiveFlg == true)
                                         select new HR_Training_Feedback_DTO
                                         {
                                             HRMFOPT_Id = b.HRMFOPT_Id,
                                             HRMFOPT_OptionName = c.HRMFOPT_OptionName,
                                             HRMFQNS_Id = b.HRMFQNS_Id
                                         }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public HR_Training_Feedback_DTO getFeedbackTrainer(HR_Training_Feedback_DTO data)
        {
            try
            {
                if (data.Type == "Trainer")
                {
                    data.trainingFeedbacklist = _vmsconte.HR_Training_Feedback_DMO_con.Where(t => t.HRTCR_Id == data.HRTCR_Id && t.HRTFEED_TrainerId == data.HRTFEED_TrainerId && t.HRTFEED_CreatedBy == data.userId).Distinct().ToArray();
                }
                else if (data.Type == "Trainee")
                {
                    data.trainingFeedbacklist = _vmsconte.HR_Training_Feedback_DMO_con.Where(t => t.HRTCR_Id == data.HRTCR_Id && t.HRME_Id == data.HRME_Id && t.HRTFEED_CreatedBy == data.userId).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Training_Feedback_DTO savetrainerfeedback(HR_Training_Feedback_DTO dto)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();

                for (int counter = 0; counter < dto.question_Answer.Length; counter++)
                {
                    if (dto.question_Answer[counter].HRTFEED_Id > 0)
                    {
                        var result = _vmsconte.HR_Training_Feedback_DMO_con.Single(t => t.HRTFEED_Id == dto.question_Answer[counter].HRTFEED_Id);
                        result.MI_Id = dto.MI_Id;
                        result.HRTCR_Id = dto.HRTCR_Id;
                        result.HRMTT_Id = dto.HRMTT_Id;
                        result.HRME_Id = userhrme_id;
                        result.HRTFEED_IntORExtFlg = "Internal";
                        result.HRTFEED_TrainerId = dto.HRTFEED_TrainerId;
                        result.HRMFQNS_Id = dto.question_Answer[counter].HRMFQNS_Id;
                        result.HRMFOPT_Id = dto.question_Answer[counter].answer;
                        result.HRTFEED_ActiveFlg = true;
                        result.HRTFEED_CreatedBy = dto.userId;
                        result.HRTFEED_UpdatedBy = dto.userId;
                        result.HRTFEED_CreatedDate = DateTime.Now;
                        result.HRTFEED_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Update";
                    }
                    else
                    {
                        HR_Training_Feedback_DMO hm = new HR_Training_Feedback_DMO();
                        hm.MI_Id = dto.MI_Id;
                        hm.HRTCR_Id = dto.HRTCR_Id;
                        hm.HRMTT_Id = dto.HRMTT_Id;
                        hm.HRME_Id = userhrme_id;
                        hm.HRTFEED_IntORExtFlg = "Internal";
                        hm.HRTFEED_TrainerId = dto.HRTFEED_TrainerId;
                        hm.HRMFQNS_Id = dto.question_Answer[counter].HRMFQNS_Id;
                        hm.HRMFOPT_Id = dto.question_Answer[counter].answer;
                        hm.HRTFEED_ActiveFlg = true;
                        hm.HRTFEED_AboutTraining = dto.question_Answer[counter].name;
                        hm.HRTFEED_CreatedBy = dto.userId;
                        hm.HRTFEED_UpdatedBy = dto.userId;
                        hm.HRTFEED_CreatedDate = DateTime.Now;
                        hm.HRTFEED_UpdatedDate = DateTime.Now;
                        _vmsconte.Add(hm);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Add";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Training_Feedback_DTO savetraineefeedback(HR_Training_Feedback_DTO dto)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();

                for (int counter = 0; counter < dto.question_Answer.Length; counter++)
                {
                    if (dto.question_Answer[counter].HRTFEED_Id > 0)
                    {
                        var result = _vmsconte.HR_Training_Feedback_DMO_con.Single(t => t.HRTFEED_Id == dto.question_Answer[counter].HRTFEED_Id);
                        result.MI_Id = dto.MI_Id;
                        result.HRTCR_Id = dto.HRTCR_Id;
                        result.HRME_Id = dto.HRME_Id;
                        result.HRMTT_Id = dto.HRMTT_Id;
                        result.HRTFEED_IntORExtFlg = "Internal";
                        result.HRTFEED_TrainerId = userhrme_id;
                        result.HRMFQNS_Id = dto.question_Answer[counter].HRMFQNS_Id;
                        result.HRMFOPT_Id = dto.question_Answer[counter].answer;
                        result.HRTFEED_AboutTraining = dto.question_Answer[counter].name;
                        result.HRTFEED_ActiveFlg = true;
                        result.HRTFEED_CreatedBy = dto.userId;
                        result.HRTFEED_UpdatedBy = dto.userId;
                        result.HRTFEED_CreatedDate = DateTime.Now;
                        result.HRTFEED_UpdatedDate = DateTime.Now;
                        result.HRTFEED_Verified = dto.HRTFEED_Verified;
                        _vmsconte.Update(result);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Update";
                    }
                    else
                    {
                        HR_Training_Feedback_DMO hm = new HR_Training_Feedback_DMO();
                        hm.MI_Id = dto.MI_Id;
                        hm.HRTCR_Id = dto.HRTCR_Id;
                        hm.HRME_Id = dto.HRME_Id;
                        hm.HRMTT_Id = dto.HRMTT_Id;
                        hm.HRTFEED_IntORExtFlg = "Internal";
                        hm.HRTFEED_TrainerId = userhrme_id;
                        hm.HRMFQNS_Id = dto.question_Answer[counter].HRMFQNS_Id;
                        hm.HRMFOPT_Id = dto.question_Answer[counter].answer;
                        hm.HRTFEED_ActiveFlg = true;
                        hm.HRTFEED_AboutTraining = dto.question_Answer[counter].name;
                        hm.HRTFEED_CreatedBy = dto.userId;
                        hm.HRTFEED_UpdatedBy = dto.userId;
                        hm.HRTFEED_CreatedDate = DateTime.Now;
                        hm.HRTFEED_UpdatedDate = DateTime.Now;
                        hm.HRTFEED_Verified = dto.HRTFEED_Verified;
                        _vmsconte.Add(hm);
                        _vmsconte.SaveChanges();
                        dto.returnvales = "Add";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public HR_Training_Feedback_DTO trainerfeedbackreport(HR_Training_Feedback_DTO dto)
        {
            try
            {
                dto.totalparticipants = (from a in _vmsconte.HR_Training_Feedback_DMO_con
                                         from b in _vmsconte.HR_Master_Employee_DMO
                                         where (a.HRTCR_Id == dto.HRTCR_Id && a.HRTFEED_TrainerId == dto.HRTFEED_TrainerId && a.HRME_Id == b.HRME_Id)
                                         select new HR_Training_Feedback_DTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName)
                                         }).Distinct().ToArray();

                dto.getfeedbackquestions = (from a in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                            from b in _vmsconte.HR_Training_Question_DMO_con
                                            where (a.HRMFQNS_Id == b.HRMFQNS_Id && b.HRTCR_Id == dto.HRTCR_Id && b.HRTRQNS_ActiveFlg == true && a.HRMFQNS_QuestionForFlg == "Trainee")
                                            select new HR_Training_Feedback_DTO
                                            {
                                                HRMFQNS_Id = b.HRMFQNS_Id,
                                                HRMFQNS_QuestionName = a.HRMFQNS_QuestionName
                                            }).Distinct().ToArray();

                dto.getfeedbackreport = (from a in _vmsconte.HR_Training_Create_DMO_con
                                         from b in _vmsconte.HR_Training_Create_Participants_DMO_con
                                         from c in _vmsconte.HR_Master_Employee_DMO
                                         from d in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                         from e in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                         from f in _vmsconte.HR_Training_Feedback_DMO_con
                                         where (a.HRTCR_Id == dto.HRTCR_Id && a.HRTCR_Id == b.HRTCR_Id && b.HRME_Id == c.HRME_Id && f.HRTFEED_TrainerId == dto.HRTFEED_TrainerId && f.HRTCR_Id == dto.HRTCR_Id && f.HRME_Id == c.HRME_Id && d.HRMFQNS_Id == f.HRMFQNS_Id && e.HRMFOPT_Id == f.HRMFOPT_Id && d.HRMFQNS_QuestionForFlg == "Trainee")
                                         select new HR_Training_Feedback_DTO
                                         {
                                             HRME_Id = f.HRME_Id,
                                             HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null ? "" : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? "" : c.HRME_EmployeeLastName),
                                             HRTFEED_IntORExtFlg = f.HRTFEED_IntORExtFlg,
                                             HRMFQNS_Id = f.HRMFQNS_Id,
                                             HRMFQNS_QuestionName = d.HRMFQNS_QuestionName,
                                             HRMFOPT_Id = f.HRMFOPT_Id,
                                             HRMFOPT_OptionName = e.HRMFOPT_OptionName
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HR_Training_Feedback_DTO traineefeedbackreport(HR_Training_Feedback_DTO dto)
        {
            try
            {
                dto.totalparticipants = (from a in _vmsconte.HR_Training_Feedback_DMO_con
                                         from b in _vmsconte.HR_Master_Employee_DMO
                                         where (a.HRTCR_Id == dto.HRTCR_Id && a.HRME_Id == dto.HRME_Id && a.HRTFEED_TrainerId == b.HRME_Id)
                                         select new HR_Training_Feedback_DTO
                                         {
                                             HRTFEED_TrainerId = a.HRTFEED_TrainerId,
                                             HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName)
                                         }).Distinct().ToArray();

                dto.getfeedbackquestions = (from a in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                            from b in _vmsconte.HR_Training_Question_DMO_con
                                            where (a.HRMFQNS_Id == b.HRMFQNS_Id && b.HRTCR_Id == dto.HRTCR_Id && b.HRTRQNS_ActiveFlg == true && a.HRMFQNS_QuestionForFlg == "Trainer")
                                            select new HR_Training_Feedback_DTO
                                            {
                                                HRMFQNS_Id = b.HRMFQNS_Id,
                                                HRMFQNS_QuestionName = a.HRMFQNS_QuestionName
                                            }).Distinct().ToArray();

                dto.getfeedbackreport = (from a in _vmsconte.HR_Training_Create_DMO_con
                                         from b in _vmsconte.HR_Training_Create_Participants_DMO_con
                                         from c in _vmsconte.HR_Master_Employee_DMO
                                         from d in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                         from e in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                         from f in _vmsconte.HR_Training_Feedback_DMO_con
                                         where (a.HRTCR_Id == dto.HRTCR_Id && a.HRTCR_Id == b.HRTCR_Id && b.HRME_Id == c.HRME_Id && f.HRME_Id == dto.HRME_Id && f.HRTCR_Id == dto.HRTCR_Id && f.HRTFEED_TrainerId == c.HRME_Id && d.HRMFQNS_Id == f.HRMFQNS_Id && e.HRMFOPT_Id == f.HRMFOPT_Id && d.HRMFQNS_QuestionForFlg == "Trainer")
                                         select new HR_Training_Feedback_DTO
                                         {
                                             HRTFEED_TrainerId = f.HRTFEED_TrainerId,
                                             HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null ? "" : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? "" : c.HRME_EmployeeLastName),
                                             HRTFEED_IntORExtFlg = f.HRTFEED_IntORExtFlg,
                                             HRMFQNS_Id = f.HRMFQNS_Id,
                                             HRMFQNS_QuestionName = d.HRMFQNS_QuestionName,
                                             HRMFOPT_Id = f.HRMFOPT_Id,
                                             HRMFOPT_OptionName = e.HRMFOPT_OptionName
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }



        /////////////////////////////report////////////////////////////////////////////////
        public HR_Training_Feedback_DTO reportloaddata(HR_Training_Feedback_DTO data)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.userId).Select(t => t.Emp_Code).FirstOrDefault();

                data.trainerprogdetails = (from a in _vmsconte.HR_Training_Create_DMO_con
                                           from b in _vmsconte.HR_Training_Create_Participants_DMO_con                                
                                           where (a.HRTCR_Id == b.HRTCR_Id && b.HRME_Id == userhrme_id)
                                           select new HR_Training_Feedback_DTO
                                           {
                                               HRTCR_Id = a.HRTCR_Id,
                                               HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                           }).Distinct().ToArray();

                data.traineeprogdetails = (from a in _vmsconte.HR_Training_Create_DMO_con
                                           from b in _vmsconte.HR_Training_Create_IntTrainer_DMO_con
                                           where (a.HRTCR_Id == b.HRTCR_Id && b.HRME_Id == userhrme_id)
                                           select new HR_Training_Feedback_DTO
                                           {
                                               HRTCR_Id = a.HRTCR_Id,
                                               HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                           }).Distinct().ToArray();

                if (data.traineeprogdetails.Length > 0)
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_TopicFeedbackList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = userhrme_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Trainee"
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
                            data.traninglisttrainee = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }

                if (data.trainerprogdetails.Length > 0)
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_TopicFeedbackList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = userhrme_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Trainer"
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
                            data.traninglisttrainer = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public HR_Training_Feedback_DTO reportpagetrainerfeedbackreport(HR_Training_Feedback_DTO dto)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();
                if (dto.Type == "Trainer")
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_FeedbackReport";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                        {
                            Value = userhrme_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Trainer"
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.BigInt)
                        {
                            Value = dto.HRTCR_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMTT_Id", SqlDbType.BigInt)
                        {
                            Value = dto.HRMTT_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRTFEED_TrainerId", SqlDbType.BigInt)
                        {
                            Value = dto.HRTFEED_TrainerId
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
                            dto.getfeedbackquestions = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (dto.Type == "Trainee")
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Training_FeedbackReport";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Trainee"
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.BigInt)
                        {
                            Value = dto.HRTCR_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMTT_Id", SqlDbType.BigInt)
                        {
                            Value = dto.HRMTT_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRTFEED_TrainerId", SqlDbType.BigInt)
                        {
                            Value = userhrme_id
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
                            dto.getfeedbackquestions = retObject.ToArray();
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
            return dto;
        }

        public HR_Training_Feedback_DTO reportpagetraineefeedbackreport(HR_Training_Feedback_DTO dto)
        {
            try
            {
                dto.totalparticipants = (from a in _vmsconte.HR_Training_Feedback_DMO_con
                                         from b in _vmsconte.HR_Master_Employee_DMO
                                         where (a.HRTCR_Id == dto.HRTCR_Id && a.HRME_Id == dto.HRME_Id && a.HRTFEED_TrainerId == b.HRME_Id)
                                         select new HR_Training_Feedback_DTO
                                         {
                                             HRTFEED_TrainerId = a.HRTFEED_TrainerId,
                                             HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName)
                                         }).Distinct().ToArray();

                dto.getfeedbackquestions = (from a in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                            from b in _vmsconte.HR_Training_Question_DMO_con
                                            where (a.HRMFQNS_Id == b.HRMFQNS_Id && b.HRTCR_Id == dto.HRTCR_Id && b.HRTRQNS_ActiveFlg == true && a.HRMFQNS_QuestionForFlg == "Trainer")
                                            select new HR_Training_Feedback_DTO
                                            {
                                                HRMFQNS_Id = b.HRMFQNS_Id,
                                                HRMFQNS_QuestionName = a.HRMFQNS_QuestionName
                                            }).Distinct().ToArray();

                dto.getfeedbackreport = (from a in _vmsconte.HR_Training_Create_DMO_con
                                         from b in _vmsconte.HR_Training_Create_Participants_DMO_con
                                         from c in _vmsconte.HR_Master_Employee_DMO
                                         from d in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                         from e in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                         from f in _vmsconte.HR_Training_Feedback_DMO_con
                                         where (a.HRTCR_Id == dto.HRTCR_Id && a.HRTCR_Id == b.HRTCR_Id && b.HRME_Id == c.HRME_Id && f.HRME_Id == dto.HRME_Id && f.HRTCR_Id == dto.HRTCR_Id && f.HRTFEED_TrainerId == c.HRME_Id && d.HRMFQNS_Id == f.HRMFQNS_Id && e.HRMFOPT_Id == f.HRMFOPT_Id && d.HRMFQNS_QuestionForFlg == "Trainer")
                                         select new HR_Training_Feedback_DTO
                                         {
                                             HRTFEED_TrainerId = f.HRTFEED_TrainerId,
                                             HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null ? "" : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? "" : c.HRME_EmployeeLastName),
                                             HRTFEED_IntORExtFlg = f.HRTFEED_IntORExtFlg,
                                             HRMFQNS_Id = f.HRMFQNS_Id,
                                             HRMFQNS_QuestionName = d.HRMFQNS_QuestionName,
                                             HRMFOPT_Id = f.HRMFOPT_Id,
                                             HRMFOPT_OptionName = e.HRMFOPT_OptionName
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HR_Training_Feedback_DTO trainigheldreport(HR_Training_Feedback_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_TRAINING_AND_FEEDBACK";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar)
                    {
                        Value = dto.Monthid
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                    {
                        Value = dto.HRMLY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = dto.feedbacktype
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
                        dto.trainingfeedbackdetails = retObject.ToArray();
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
            return dto;
        }


    }
}
