using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class AddResponseLiveChatBotImpl : AddResponseLiveChatBotInterface
    {
        
        


        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _db;
        public AddResponseLiveChatBotImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext db)
        {
            _HRMSContext = HRMSContext;
            _db = db;
        }



        public AddResponseLiveChatBotDTO savedata(AddResponseLiveChatBotDTO data)
        {
            try
            {
                foreach (var keyword in data.Keywords)
                {
                    var existingKeyWord = _HRMSContext.Chat_FAQ_Master_Questions_KeywordsDMO.Where(k => k.CTFAQMQKW_Keywords == keyword.CTFAQMQKW_Keywords).ToList();
                    if (existingKeyWord.Count > 0)
                    {
                        data.returndata = "Duplicate";
                        return data;
                    }
                }

                // Create a new response object
                AddResponseLiveChatBotDMO newResponse1 = new AddResponseLiveChatBotDMO
                {
                    CTFAQMQ_FAQ = data.CTFAQMQ_FAQ,
                    CTFAQMQ_Remarks = data.CTFAQMQ_Remarks,
                    CTFAQMQ_ActiveFlag = data.CTFAQMQ_ActiveFlag,
                    CTFAQMQ_CreatedDate = DateTime.UtcNow,
                    CTFAQMQ_UpdatedDate = DateTime.UtcNow,
                    CTFAQMQ_CreatedBy = data.userid,
                    CTFAQMQ_UpdatedBy = data.userid
                };

                // Add the main response and save it
                _HRMSContext.Add(newResponse1);
                _HRMSContext.SaveChanges();


                foreach (var keyword in data.Keywords)
                {
                    AddResponseLiveChatBotKayDMO newKeyword = new AddResponseLiveChatBotKayDMO
                    {
                        CTFAQMQ_Id = newResponse1.CTFAQMQ_Id,
                        CTFAQMQKW_Keywords = keyword.CTFAQMQKW_Keywords,
                        CTFAQMQKW_ActiveFlag = keyword.CTFAQMQKW_ActiveFlag,
                        CTFAQMQKW_CreatedDate = DateTime.UtcNow,
                        CTFAQMQKW_UpdatedDate = DateTime.UtcNow,
                        CTFAQMQKW_CreatedBy = data.userid,
                        CTFAQMQKW_UpdatedBy = data.userid
                    };

                    _HRMSContext.Add(newKeyword);
                }

                // Save answers
                foreach (var answer in data.answers)
                {
                    AddResponseLiveChatBotAnsDMO newAnswer = new AddResponseLiveChatBotAnsDMO
                    {
                        CTFAQMQ_Id = newResponse1.CTFAQMQ_Id,
                        CTFAQMQANS_Answers = answer.CTFAQMQANS_Answers,
                        CTFAQMQANS_Flg = answer.CTFAQMQANS_Flg,
                        CTFAQMQANS_ActiveFlag = true,
                        CTFAQMQANS_CreatedDate = DateTime.UtcNow,
                        CTFAQMQANS_UpdatedDate = DateTime.UtcNow,
                        CTFAQMQANS_CreatedBy = data.userid,
                        CTFAQMQANS_UpdatedBy = data.userid
                    };

                    _HRMSContext.Add(newAnswer);
                }

                var response = _HRMSContext.SaveChanges();

                if (response > 0)
                {
                    data.returndata = "Add";
                }
                else
                {
                    data.returndata = "Notsave";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returndata = "admin";
            }
            return data;
        }

        public AddResponseLiveChatBotDTO saveansdata(AddResponseLiveChatBotDTO data)
        {
            try
            {
                AddResponseLiveChatBotDMO newResponse = new AddResponseLiveChatBotDMO
                {
                    CTFAQMQ_FAQ = data.CTFAQMQ_FAQ,
                    CTFAQMQ_Remarks = data.CTFAQMQ_Remarks,
                    CTFAQMQ_ActiveFlag = data.CTFAQMQ_ActiveFlag,
                    CTFAQMQ_CreatedDate = DateTime.UtcNow,
                    CTFAQMQ_UpdatedDate = DateTime.UtcNow,
                    CTFAQMQ_CreatedBy = data.CTFAQMQ_CreatedBy,
                    CTFAQMQ_UpdatedBy = data.CTFAQMQ_UpdatedBy
                };

                _HRMSContext.Add(newResponse);
                _HRMSContext.SaveChanges();

                // Save answers
                foreach (var answer in data.answers)
                {
                    AddResponseLiveChatBotAnsDMO newAnswer = new AddResponseLiveChatBotAnsDMO
                    {
                        CTFAQMQ_Id = newResponse.CTFAQMQ_Id,
                        CTFAQMQANS_Answers = answer.CTFAQMQANS_Answers,
                        CTFAQMQANS_Flg = answer.CTFAQMQANS_Flg,
                        CTFAQMQANS_ActiveFlag = answer.CTFAQMQANS_ActiveFlag,
                        CTFAQMQANS_CreatedDate = DateTime.UtcNow,
                        CTFAQMQANS_UpdatedDate = DateTime.UtcNow,
                        CTFAQMQANS_CreatedBy = answer.CTFAQMQANS_CreatedBy,
                        CTFAQMQANS_UpdatedBy = answer.CTFAQMQANS_UpdatedBy
                    };

                    _HRMSContext.Add(newAnswer);
                }

                var response = _HRMSContext.SaveChanges();

                if (response > 0)
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




        public AddResponseLiveChatBotDTO GetAllFAQs(AddResponseLiveChatBotDTO data)
        {
            try
            {
                // Fetch all FAQs along with related keywords and answers in one go
                var allFAQs = _HRMSContext.Chat_FAQ_Master_QuestionsDMO
                    .Select(faq => new
                    {
                        faq.CTFAQMQ_Id,
                        faq.CTFAQMQ_FAQ,
                        faq.CTFAQMQ_Remarks,
                        Keywords = _HRMSContext.Chat_FAQ_Master_Questions_KeywordsDMO
                            .Where(k => k.CTFAQMQ_Id == faq.CTFAQMQ_Id)
                            .Select(k => new KeywordDTO { CTFAQMQKW_Keywords = k.CTFAQMQKW_Keywords })
                            .ToList(),
                        Answers = _HRMSContext.Chat_FAQ_Master_Questions_AnswersDMO
                            .Where(a => a.CTFAQMQ_Id == faq.CTFAQMQ_Id)
                            .Select(a => new answersDTO
                            {
                                CTFAQMQANS_Answers = a.CTFAQMQANS_Answers,
                                CTFAQMQANS_Flg = a.CTFAQMQANS_Flg
                            })
                            .ToList()
                    })
                    .ToList();

                var faqList = allFAQs.Select(faq => new AddResponseLiveChatBotDTO
                {
                    CTFAQMQ_Id = faq.CTFAQMQ_Id,
                    CTFAQMQ_FAQ = faq.CTFAQMQ_FAQ,
                    CTFAQMQ_Remarks = faq.CTFAQMQ_Remarks,
                    Keywords = faq.Keywords,
                    answers = faq.Answers
                }).ToArray();

                data.qetanswer = faqList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return data;
        }

        public AddResponseLiveChatBotDTO GetBotResponse(AddResponseLiveChatBotDTO data)
        {
            try
            {

                Chat_FAQ_Interactions_DetailsDMO dmo = new Chat_FAQ_Interactions_DetailsDMO();

                Chat_FAQ_InteractionsDMO chat_FAQ_InteractionsDMO = new Chat_FAQ_InteractionsDMO();
                chat_FAQ_InteractionsDMO.CTFAQINT_UpdatedDate = DateTime.Now;
                chat_FAQ_InteractionsDMO.CTFAQINT_Date = DateTime.Now;
                chat_FAQ_InteractionsDMO.CTFAQINT_CreatedDate = DateTime.Now;
                chat_FAQ_InteractionsDMO.MI_Id = data.MI_Id;
                chat_FAQ_InteractionsDMO.CTFAQINT_CreatedBy = data.userid;
                chat_FAQ_InteractionsDMO.CTFAQINT_UpdatedBy = data.userid;


                _HRMSContext.Chat_FAQ_InteractionsDMO.Add(chat_FAQ_InteractionsDMO);
                _HRMSContext.SaveChanges();

                string keyword = data.userMessage.ToLower();
                var exactMatch = _HRMSContext.Chat_FAQ_Master_Questions_KeywordsDMO.FirstOrDefault(k => k.CTFAQMQKW_Keywords.ToLower() == keyword);

                dmo.CTFAQINTDET_Interaction = data.userMessage;
                dmo.CTFAQINTDET_UpdatedDate = DateTime.Now;
                dmo.CTFAQINTDET_CreatedDate = DateTime.Now;
                dmo.CTFAQINTDET_CreatedBy = data.userid;
                dmo.CTFAQINTDET_UpdatedBy = data.userid;
                dmo.CTFAQINT_Id = chat_FAQ_InteractionsDMO.CTFAQINT_Id;

                if (exactMatch != null)
                {
                    var getquestion = _HRMSContext.Chat_FAQ_Master_QuestionsDMO.Where(Q => Q.CTFAQMQ_Id == exactMatch.CTFAQMQ_Id).ToList();

                    if (getquestion.Count > 0)
                    {
                        data.qetanswer = _HRMSContext.Chat_FAQ_Master_Questions_AnswersDMO.Where(A => A.CTFAQMQ_Id == getquestion[0].CTFAQMQ_Id).ToArray();
                        var answer = data.qetanswer.GetValue(0) as AddResponseLiveChatBotAnsDMO;
                        dmo.CTFAQINTDET_Response = answer.CTFAQMQANS_Answers;
                        _HRMSContext.Chat_FAQ_Interactions_DetailsDMO.Add(dmo);
                        _HRMSContext.SaveChanges();
                        return data;
                    }
                }

                #region
                //var keywords = _HRMSContext.Chat_FAQ_Master_Questions_KeywordsDMO.ToList();
                //foreach (var k in keywords)
                //{
                //    double similarity = CalculateSimilarity(keyword, k.CTFAQMQKW_Keywords.ToLower());
                //    if (similarity >= 0.8)
                //    {
                //        var getquestion = _HRMSContext.Chat_FAQ_Master_QuestionsDMO.Where(Q => Q.CTFAQMQ_Id == k.CTFAQMQ_Id).ToList();

                //        if (getquestion.Count > 0)
                //        {
                //            data.qetanswer = _HRMSContext.Chat_FAQ_Master_Questions_AnswersDMO.Where(A => A.CTFAQMQ_Id == getquestion[0].CTFAQMQ_Id).ToArray();
                //            var answer = data.qetanswer.GetValue(0) as AddResponseLiveChatBotAnsDMO;
                //            dmo.CTFAQINTDET_Response = answer.CTFAQMQANS_Answers;
                //            _HRMSContext.Chat_FAQ_Interactions_DetailsDMO.Add(dmo);
                //            _HRMSContext.SaveChanges();
                //            break;
                //        }
                //    }
                //}
                #endregion
                var keywords = _HRMSContext.Chat_FAQ_Master_Questions_KeywordsDMO.ToList();
                bool matchFound = false;

                foreach (var k in keywords)
                {
                    double similarity = CalculateSimilarity(keyword, k.CTFAQMQKW_Keywords.ToLower());
                    if (similarity >= 0.8)
                    {
                        var getquestion = _HRMSContext.Chat_FAQ_Master_QuestionsDMO.Where(Q => Q.CTFAQMQ_Id == k.CTFAQMQ_Id).ToList();

                        if (getquestion.Count > 0)
                        {
                            data.qetanswer = _HRMSContext.Chat_FAQ_Master_Questions_AnswersDMO.Where(A => A.CTFAQMQ_Id == getquestion[0].CTFAQMQ_Id).ToArray();
                            var answer = data.qetanswer.GetValue(0) as AddResponseLiveChatBotAnsDMO;
                            if (data.answers.Count > 0 && data.answers != null)
                            {
                                dmo.CTFAQINTDET_Response = answer.CTFAQMQANS_Answers;
                                matchFound = true;
                                _HRMSContext.Chat_FAQ_Interactions_DetailsDMO.Add(dmo);
                                _HRMSContext.SaveChanges();
                                break;
                            }
                        }
                    }
                }

                // If no matches were found, add the default answer
                if (matchFound == false)
                {
                    if (data.answers.Count > 0 && data.answers != null)
                    {
                        var ans = data.answers[0].CTFAQMQANS_Answers;
                        dmo.CTFAQINTDET_Response = ans;
                    }
                    else
                    {
                        dmo.CTFAQINTDET_Response = "I'm sorry, I don't have an answer for that.";
                    }

                    _HRMSContext.Chat_FAQ_Interactions_DetailsDMO.Add(dmo);
                    _HRMSContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return data;
        }

        private double CalculateSimilarity(string source, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

        private int ComputeLevenshteinDistance(string source, string target)
        {
            int[,] matrix = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; i++)
                matrix[i, 0] = i;

            for (int j = 0; j <= target.Length; j++)
                matrix[0, j] = j;

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[source.Length, target.Length];
        }
    }
}
