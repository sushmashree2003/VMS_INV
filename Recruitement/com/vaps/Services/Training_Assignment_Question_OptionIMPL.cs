using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.Training;
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
    public class Training_Assignment_Question_OptionIMPL : Interfaces.Training_Assignment_Question_OptionInterface
    {
        public VMSContext _vmsconte;



        public Training_Assignment_Question_OptionIMPL(VMSContext vmsContext)
        {
            _vmsconte = vmsContext;
        }
        public HR_Training_Assignment_Question_Option_DTO getdata_TAQO(HR_Training_Assignment_Question_Option_DTO dto)
        {
            try
            {
                dto.question_option_list = (from a in _vmsconte.HR_Training_Assignment_Question_Option_DMO
                                            from b in _vmsconte.HR_Training_Assignment_Questions_DMO
                                            from c in _vmsconte.HR_Training_Assignment_Option_DMO
                                            where a.HRTAQNS_Id == b.HRTAQNS_Id && a.HRTAOPT_Id == c.HRTAOPT_Id && a.MI_Id == c.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id
                                            select new HR_Training_Assignment_Question_Option_DTO
                                            {
                                                HRTAQNS_Id = b.HRTAQNS_Id,
                                                HRTAQNS_QuestionName = b.HRTAQNS_QuestionName,
                                                HRTAQOPT_ActiveFlg = a.HRTAQOPT_ActiveFlg
                                            }).Distinct().ToArray();

                dto.option_list = _vmsconte.HR_Training_Assignment_Option_DMO.Where(a => a.MI_Id == dto.MI_Id && a.HRTAOPT_ActiveFlg == true).ToArray();
                dto.question_list = _vmsconte.HR_Training_Assignment_Questions_DMO.Where(a => a.MI_Id == dto.MI_Id && a.HRTAQNS_ActiveFlg == true).ToArray();

                //using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "HR_Question_list_proc";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //    {
                //        Value = dto.MI_Id
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
                //                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        dto.question_list = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Training_Assignment_Question_Option_DTO deactivate_TAQO(HR_Training_Assignment_Question_Option_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Training_Assignment_Question_Option_DMO.Where(a => a.HRTAQNS_Id == dto.HRTAQNS_Id && a.MI_Id == dto.MI_Id).ToList();
                foreach (var item in result)
                {
                    var result1 = _vmsconte.HR_Training_Assignment_Question_Option_DMO.Single(a => a.HRTAQOPT_Id == item.HRTAQOPT_Id && a.MI_Id == dto.MI_Id);

                    if (dto.HRTAQOPT_ActiveFlg == true)
                    {
                        result1.HRTAQOPT_ActiveFlg = false;
                        result1.HRTAQOPT_UpdatedBy = dto.userId;
                        result1.HRTAQOPT_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result1);
                        dto.returnval = false;
                    }
                    else
                    {
                        result1.HRTAQOPT_ActiveFlg = true;
                        result1.HRTAQOPT_UpdatedBy = dto.userId;
                        result1.HRTAQOPT_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result1);
                        dto.returnval = true;
                    }
                }

                _vmsconte.SaveChanges();



            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        public HR_Training_Assignment_Question_Option_DTO details_TAQO(HR_Training_Assignment_Question_Option_DTO dto)
        {
            try
            {
                dto.question_details_list = (from a in _vmsconte.HR_Training_Assignment_Question_Option_DMO
                                             from b in _vmsconte.HR_Training_Assignment_Questions_DMO
                                             where a.HRTAQNS_Id == b.HRTAQNS_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id && a.HRTAQNS_Id == dto.HRTAQNS_Id && a.MI_Id == dto.MI_Id
                                             select new HR_Training_Assignment_Question_Option_DTO
                                             {
                                                 HRTAQNS_Id = b.HRTAQNS_Id,
                                                 HRTAQNS_QuestionName = b.HRTAQNS_QuestionName,

                                             }).Distinct().ToArray();

                dto.option_details_list = (from a in _vmsconte.HR_Training_Assignment_Question_Option_DMO
                                           from b in _vmsconte.HR_Training_Assignment_Option_DMO
                                           where a.HRTAOPT_Id == b.HRTAOPT_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id && a.HRTAQNS_Id == dto.HRTAQNS_Id && a.MI_Id == dto.MI_Id
                                           select new HR_Training_Assignment_Question_Option_DTO
                                           {
                                               HRTAOPT_Id = b.HRTAOPT_Id,
                                               HRTAOPT_OptionName = b.HRTAOPT_OptionName,

                                           }).ToArray();
                dto.option_list = _vmsconte.HR_Training_Assignment_Option_DMO.Where(a => a.MI_Id == dto.MI_Id && a.HRTAOPT_ActiveFlg == true).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Training_Assignment_Question_Option_DTO option_view_TAQO(HR_Training_Assignment_Question_Option_DTO dto)
        {
            try
            {
                dto.option_view_list = (from a in _vmsconte.HR_Training_Assignment_Question_Option_DMO
                                        from b in _vmsconte.HR_Training_Assignment_Option_DMO
                                        where a.HRTAOPT_Id == b.HRTAOPT_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id && a.HRTAQNS_Id == dto.HRTAQNS_Id
                                        select new HR_Training_Assignment_Question_Option_DTO
                                        {
                                            HRTAOPT_OptionName = b.HRTAOPT_OptionName,
                                            HRTAOPT_ActiveFlg = b.HRTAOPT_ActiveFlg
                                        }).ToArray();
            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        public HR_Training_Assignment_Question_Option_DTO SaveEdit_TAQO(HR_Training_Assignment_Question_Option_DTO dto)
        {

            try
            {
                if (dto.hqn_id > 0)
                {
                    var result = _vmsconte.HR_Training_Assignment_Question_Option_DMO.Where(a => a.HRTAQNS_Id == dto.HRTAQNS_Id && a.MI_Id == dto.MI_Id).ToList();
                    foreach (var item in result)
                    {
                        var result1 = _vmsconte.HR_Training_Assignment_Question_Option_DMO.Single(a => a.HRTAQOPT_Id == item.HRTAQOPT_Id && a.MI_Id == dto.MI_Id);
                        _vmsconte.Remove(result1);
                    }
                    foreach (var item in dto.Feedback_Option)
                    {
                        HR_Training_Assignment_Question_Option_DMO tot = new HR_Training_Assignment_Question_Option_DMO();
                        tot.HRTAQNS_Id = dto.HRTAQNS_Id;
                        tot.HRTAOPT_Id = item.HRTAOPT_Id;
                        tot.HRTAQOPT_ActiveFlg = true;
                        tot.HRTAQOPT_CreatedBy = dto.userId;
                        tot.HRTAQOPT_CreatedDate = DateTime.Now;
                        _vmsconte.Add(tot);
                    }


                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";

                }
                else
                {

                    foreach (var item in dto.Feedback_Option)
                    {
                        HR_Training_Assignment_Question_Option_DMO tot = new HR_Training_Assignment_Question_Option_DMO();
                        tot.HRTAQNS_Id = dto.HRTAQNS_Id;
                        tot.HRTAOPT_Id = item.HRTAOPT_Id;
                        tot.HRTAQOPT_ActiveFlg = true;
                        tot.MI_Id = dto.MI_Id;
                        tot.HRTAQOPT_CreatedBy = dto.userId;
                        tot.HRTAQOPT_CreatedDate = DateTime.Now;
                        _vmsconte.Add(tot);
                    }


                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
    }
}
