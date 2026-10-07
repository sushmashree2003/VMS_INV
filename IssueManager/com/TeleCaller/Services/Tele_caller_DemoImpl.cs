using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VMS.TeleCaller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IssueManager.com.TeleCaller.Services
{
    public class Tele_caller_DemoImpl : Interfaces.Tele_caller_demoInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;
        public Tele_caller_DemoImpl(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;
            //
        }

        //public TeleCallerDTO loaddata(TeleCallerDTO data)
        //{
        //    try
        //    {
        //       // data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
        //        //data.sourcelist = _IssueManagercontext.VMS_CheckBookSourceDMO.Where(L => L.VCBSRC_ActiveFlg == true).Distinct().ToArray();
        //      //  data.sourcelist = _IssueManagercontext.VMSMaster_ChequeBook_SLNoDMO.Distinct().ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}


        public TeleCallerDTO TeleCaller_List(TeleCallerDTO data)
        {
            try
            {

               
                List<AcademicYear> list = new List<AcademicYear>();

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_DemoPresntationClientsDet";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@DemoFromdate",
                      SqlDbType.DateTime)
                    {
                        Value = data.FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@DemoToDate",
                    SqlDbType.DateTime)
                    {
                        Value = data.Todate
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
                        data.PresentationClientlist = retObject.ToArray();

                        //if (data.PresentationClientlist.Length > 0)
                        //{
                        //    data.message1 = "Success";
                        //}
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

        public TeleCallerDTO UpdateTeleCaller_List(TeleCallerDTO data)
        {
            try
            {
            
                    try
                    {

                        var Updateclientlist = _IssueManagercontext.Database.ExecuteSqlCommand("usp_i_Sales_ExecutedDemo_TelecallerRemarks @p0,@p1,@p2,@p3,@p4,@p5", data.ISMSLE_Id, data.ISMSLEDM_Id, data.ISMSMPR_Id, data.TelecallerRemarks, data.RemarksFlag, data.userid);

                    if (Updateclientlist > 0)
                    {
                        data.message = "success";
                    }
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
    }
}