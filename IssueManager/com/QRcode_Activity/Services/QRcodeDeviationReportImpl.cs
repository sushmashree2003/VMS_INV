using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager.HRMasterArea;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity;
using IssueManager.com.QRcode_Activity.Interfaces;
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
    public class QRcodeDeviationReportImpl : Interfaces.QRcodeDeviationReportInterface
    {

        public HRMasterAreaContext _INVContext;
        ILogger<QRcodeDeviationReportImpl> _logInv;
       

        public QRcodeDeviationReportImpl(HRMasterAreaContext InvContext, ILogger<QRcodeDeviationReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public QRcodeDeviationReportDTO getloaddata(QRcodeDeviationReportDTO data)
        {


            data.get_building = _INVContext.HR_Master_Building_con.Where(a => a.MI_Id == data.MI_Id).ToArray();

            List<MasterRoleType> allroles = new List<MasterRoleType>();
            allroles = _INVContext.masterRoleType.Where(t => t.IVRMRT_Role == "House Keeping" || t.IVRMRT_Role == "Security" || t.IVRMRT_Role == "Hardware Asset Keeping").ToList();
            data.fillroletype = allroles.ToArray();


            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_empnamedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = data.IVRMRT_Id });


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
                        data.emp_deatils = retObject.ToArray();
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

        public QRcodeDeviationReportDTO getEmployeeName(QRcodeDeviationReportDTO data)
        {

           

            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_empnamedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = data.IVRMRT_Id });


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
                        data.emp_deatils = retObject.ToArray();
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

        
        public QRcodeDeviationReportDTO QRcodedeviation(QRcodeDeviationReportDTO data)
        {
            try
            {

                string hrmeids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrmeids = hrmeids + "," + ue.UserId;
                    }
                }
                string fromDate = "";
                string Todate = "";

                if (data.startdate != null && data.enddate != null)
                {
                    fromDate = data.startdate.Value.ToString("yyyy-MM-dd");
                    Todate = data.enddate.Value.ToString("yyyy-MM-dd");
                }

                //******************************Deviation Remarks & Deviation Calculation Report ************************************//

                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "QRcode_DeviationreportdetailsTEMP";
                        cmd.CommandType = CommandType.StoredProcedure;

                   
                        cmd.Parameters.Add(new SqlParameter("@userid",
                        SqlDbType.VarChar)
                        {
                            Value = hrmeids
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                     SqlDbType.VarChar)
                        {
                            Value = "multidates"
                        });
                        
                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                       SqlDbType.DateTime)
                        {
                           // Value = data.startdate
                            Value = fromDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                        SqlDbType.DateTime)
                        {
                            //Value = data.enddate
                            Value = Todate
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRMB_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.HRMB_Id
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

                            data.get_deviationreport = retObject.ToArray();
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

      
    }
}
