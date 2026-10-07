using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class InternalJobOpeningPostIMPL : Interfaces.InternalJobOpeningPostInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;

        public InternalJobOpeningPostIMPL(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public InternalJobOpeningPostDTO getalldetails(InternalJobOpeningPostDTO data)
        {
            try
            {
                data.getarray = _VMSContext.HR_MRFRequisitionDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_MRFRequisitionPROCD";
                    cmd.CommandType = CommandType.StoredProcedure;
                   // cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar) { Value = data.MI_Id });
                  //  cmd.Parameters.Add(new SqlParameter("@HRML_LeaveCode", SqlDbType.VarChar) { Value = leavecodes });
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreport = retObject.ToArray();
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

        //public InternalJobOpeningPostDTO report(InternalJobOpeningPostDTO data)        //{        //    try        //    {        //        string mi_ids = "0";        //        List<long> mid = new List<long>();        //        if (data.selected_Inst.Length > 0)        //        {        //            foreach (var item in data.selected_Inst)        //            {        //                mid.Add(item.MI_Id);        //            }        //            for (int i = 0; i < mid.Count; i++)        //            {        //                mi_ids = mi_ids + "," + mid[i].ToString();        //            }        //        }


        //        using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())        //        {        //            cmd.CommandText = "HR_Salaryapproval_Report";        //            cmd.CommandType = CommandType.StoredProcedure;        //            cmd.Parameters.Add(new SqlParameter("@mi_id",        //              SqlDbType.VarChar)        //            {        //                Value = mi_ids        //            });        //            cmd.Parameters.Add(new SqlParameter("@year",        //             SqlDbType.VarChar)        //            {        //                Value = data.HRES_Year        //            });        //            cmd.Parameters.Add(new SqlParameter("@month",        //            SqlDbType.VarChar)        //            {        //                Value = data.HRES_Month        //            });        //            cmd.Parameters.Add(new SqlParameter("@flag",        //           SqlDbType.VarChar)        //            {        //                Value = data.radiotype        //            });        //            if (cmd.Connection.State != ConnectionState.Open)        //                cmd.Connection.Open();        //            var retObject = new List<dynamic>();        //            try        //            {        //                using (var dataReader = cmd.ExecuteReader())        //                {        //                    while (dataReader.Read())        //                    {        //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;        //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)        //                        {        //                            dataRow.Add(        //                                dataReader.GetName(iFiled),        //                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}        //                            );        //                        }        //                        retObject.Add((ExpandoObject)dataRow);        //                    }        //                }
        //                data.GetReport = retObject.ToArray();        //            }        //            catch (Exception ex)        //            {        //                Console.Write(ex.Message);        //            }        //        }

        //    }        //    catch (Exception ex)        //    {        //        Console.WriteLine(ex.Message);        //    }        //    return data;        //}
    }
    
}
