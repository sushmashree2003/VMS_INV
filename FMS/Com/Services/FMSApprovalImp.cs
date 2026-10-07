using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Services
{
    public class FMSApprovalImp : Interface.FMSApprovalInterface
    {
        public FMSContext _context;
        public FMSApprovalImp(FMSContext _cont)
        {
            _context = _cont;
        }
        public FMSApprovalDTO loaddata(FMSApprovalDTO data)
        {
            try
            {

                //data.filename = _context.FMS_Master_PhysicalFileNameDMO.Where(R => R.FMSMPFN_ActiveFlg == true && R.MI_Id == data.MI_Id).Distinct().ToArray();
                //data.registration = _context.FMSApprovalRequestPageDMO.Select(R => R.FMSFLREQ_Id).Count();
                //data.get_tax = (from a in _context.FMSApprovalRequestPageDMO
                //                from b in _context.FMS_Master_PhysicalFileNameDMO
                //                where (a.FMSMPFN_Id == b.FMSMPFN_Id && b.MI_Id == data.MI_Id)
                //                select new FMSApprovalRequestPageDTO
                //                {
                //                    filenames = b.FMSMPFN_PhysicalFileName,
                //                    requireddays = a.FMSFLREQ_RequiredDays,
                //                    FMSFLREQ_Id = a.FMSFLREQ_Id,
                //                    FMSFLREQ_ActiveFlg = a.FMSFLREQ_ActiveFlg

                //                }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FMSApprovalDTO getdatas(FMSApprovalDTO data)
        {
            try
            {
                

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Approval_Emplist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDate",
                  SqlDbType.VarChar)
                    {
                        Value = data.fromdate.Date.ToString("yyyy-MM-dd")
                });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
                  SqlDbType.VarChar)
                    {
                        Value = data.todate.Date.ToString("yyyy-MM-dd")
            });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }
                        data.viewlist = retObject.ToArray();

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


        public FMSApprovalDTO savedetails(FMSApprovalDTO data)
        {
            try
            {
                if (data.grid_arrays != null && data.grid_arrays.Length > 0)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                    foreach (var i in data.grid_arrays)

                    {
                        data.approved_flag = false;
                        if (i.approved_flag == 1)
                        {
                            data.approved_flag = true;
                        }
                        else
                        {
                            data.approved_flag = false;
                        }

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "FMS_Approval_PROC";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@FMSFLREQ_Id",
                            SqlDbType.BigInt)
                            {
                                Value = i.FMSFLREQ_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ApprovedFlag",
                            SqlDbType.Bit
                            )
                            {
                                Value = data.approved_flag
                            });
                            cmd.Parameters.Add(new SqlParameter("@ApprovedDays",
                            SqlDbType.BigInt
                            )
                            {
                                Value = i.Approveddays
                            });
                            cmd.Parameters.Add(new SqlParameter("@Remark",
                            SqlDbType.VarChar
                            )
                            {
                                Value = i.remarks
                            });
                            cmd.Parameters.Add(new SqlParameter("@User_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.UserId
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
                                data.viewlist = retObject.ToArray();

                            }


                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
