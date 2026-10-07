using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager.HRMasterArea;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.QRcode_Activity.Services
{
    public class StaffAreaActivityRecordImpl : Interfaces.StaffAreaActivityRecordInterface
    {

        public HRMasterAreaContext _INVContext;
        ILogger<StaffAreaActivityRecordImpl> _logInv;
        private object stafflogin;
        private object t;

        public StaffAreaActivityRecordImpl(HRMasterAreaContext InvContext, ILogger<StaffAreaActivityRecordImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public StaffAreaActivityRecordDTO getloaddata(StaffAreaActivityRecordDTO data)
        {
            try
            {

                data.StaffAreaActivityRecord = (from a in _INVContext.StaffAreaActivityRecordDMO
                                                from b in _INVContext.HRMasterAreaActivityDMO
                                                from c in _INVContext.ApplicationUser
                                                from d in _INVContext.HRMasterAreaDMO 

                                                where a.HRMARACT_Id == b.HRMARACT_Id && a.User_Id == c.Id && a.HRMAR_Id ==d.HRMAR_Id && a.HRSTARACTRCD_ActiveFlag == true
                                                select new StaffAreaActivityRecordDTO
                                                {
                                                    HRMARACT_ActivityName = b.HRMARACT_ActivityName,
                                                    UserName = c.UserName,
                                                    HRSTARACTRCD_ActiveFlag = a.HRSTARACTRCD_ActiveFlag,
                                                    HRSTARACTRCD_Date=a.HRSTARACTRCD_Date,
                                                    HRSTARACTRCD_Time=a.HRSTARACTRCD_Time,
                                                    HRMA_QRCODEPath=d.HRMA_QRCODEPath
                                                }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }
        

        public StaffAreaActivityRecordDTO savedetails(StaffAreaActivityRecordDTO data)
        {
            try
            {
                

                var arealist = _INVContext.HRMasterAreaDMO.Where(t => t.HRMA_QRCODEPath == data.HRMA_QRCODEPath && t.MI_Id == data.MI_Id).FirstOrDefault();
                data.HRMAR_Id = arealist.HRMAR_Id;
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_ActivityDetailsActivity";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.User_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMAR_Id", SqlDbType.VarChar) { Value = data.HRMAR_Id }); 

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
                        data.QRcodeActivityDetails = retObject.ToArray();
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


        public StaffAreaActivityRecordDTO Activitysavedetails(StaffAreaActivityRecordDTO data)
        {
            try
            {
                string imagepath = data.HRMA_QRCODEPath;
                //var arealist = _INVContext.HRMasterAreaDMO.Where(t => t.HRMAR_Id == data.HRMAR_Id && t.MI_Id == data.MI_Id).FirstOrDefault();
                if(data.tasklistarray !=null)
                {
                    foreach (var obj in data.tasklistarray)
                    {
                        var contactExistsP = _INVContext.Database.ExecuteSqlCommand("HR_StaffAreaActivityRecord_insert @p0,@p1,@p2,@p3,@p4,@p5,@p6", data.MI_Id, obj.HRMAR_Id, data.User_Id, obj.HRMARACT_Id,obj.HRSTARACTRCD_Status, imagepath, obj.HRMARACTT_Id);

                        if (contactExistsP > 0)
                        {
                            data.returnval = "Add";
                        }
                        else
                        {
                            data.returnval = "notUpdated";
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


    }
}
