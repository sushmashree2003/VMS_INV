using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Services
{
    public class TADA_GPsLOcataionIMPL   : Interface.TADA_GPsLOcataionInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;
        public TADA_GPsLOcataionIMPL(IssueManagerContext context, DomainModelMsSqlServerContext dbcontext)
        {
            _context = context;
            _db = dbcontext;
        }
        public TADA_GPsLOcataionDTO GetLocation(TADA_GPsLOcataionDTO data)
        {
            try
            {
                string HRME_Id = "0";string fromDate = "";string Todate = "";
                if (data.TADA_GPsLOcataion !=null  && data.TADA_GPsLOcataion.Length >0)
                {
                    foreach(var  d in data.TADA_GPsLOcataion)
                    {
                        HRME_Id = HRME_Id + "," + d.HRME_Id;
                    }
                }
                if (data.Fromdate != null && data.Todate != null)
                {
                    fromDate = data.Fromdate.Value.ToString("yyyy-MM-dd");
                    Todate = data.Todate.Value.ToString("yyyy-MM-dd");
                   
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMSTADA_Data_LocationList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromDate", SqlDbType.VarChar)
                    {
                        Value = fromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = Todate
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
                        data.getReport = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_GPsLOcataionDTO saveLocation(TADA_GPsLOcataionDTO data)
        {
            try
            {
                var contactExistsP = _context.Database.ExecuteSqlCommand("VMSTADA_Data_Insert @p0,@p1,@p2,@p3,@p4,@p5",
                         data.User_Id,data.VMSGPSTADA_Address,data.VMSGPSTADA_LATLONGValue,data.VMSGPSTADA_Remarks,data.ISMSLE_Id,data.IER_Id);
                if (contactExistsP > 0)
                {
                    data.message = "save";
                }
                else
                {
                    data.message = "notsave";
                }
            }
            catch (Exception ee)
            {
                data.message = "admin";
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
