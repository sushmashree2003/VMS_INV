using DataAccessMsSqlServerProvider.com.vapstech.SpaceBooking;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBooking.Services
{
    public class Booking_DetailsImpl:Interfaces.Booking_DetailsInterface
    {
        public SpaceBookingContext _VMSContext;      
        public Booking_DetailsImpl(SpaceBookingContext sp1)
        {
            _VMSContext = sp1;           
        }
        public Booking_DetailsDTO getdetails(Booking_DetailsDTO data)
        {
            try
            {
                data.bookingList = (from a in _VMSContext.HR_Master_Room_con
                                    where (a.MI_Id == data.MI_Id && a.HRMR_ActiveFlag == true && a.HRMR_OutSideBookingFlg == true)
                                    select new Booking_DetailsDTO
                                    {
                                        HRMR_RoomName = a.HRMR_RoomName,
                                        HRMR_Id = a.HRMR_Id
                                    }).Distinct().ToArray();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<Booking_DetailsDTO> booking_report(Booking_DetailsDTO data)
        {
            try
            {
                
                string formatted1 = "";
                string formatted2 = "";
                if (data.fromdate1 != null || Convert.ToString(data.fromdate1) != "")
                {
                    formatted1 = data.fromdate1.Value.ToString("yyyy-MM-dd");
                }
                if (data.fromdate2 != null || Convert.ToString(data.fromdate2) != "")
                {
                    formatted2 = data.fromdate2.Value.ToString("yyyy-MM-dd");
                }                
                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_Booking_Details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMR_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRMR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@OSSPBOOK_Status",
                      SqlDbType.VarChar)
                    {
                        Value = data.OSSPBOOK_Status
                    });
                    cmd.Parameters.Add(new SqlParameter("@OSSPBOOK_DurationType",
                      SqlDbType.VarChar)
                    {
                        Value = data.OSSPBOOK_DurationType
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate1",
                     SqlDbType.VarChar)
                    {
                        Value = formatted1
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate2",
                     SqlDbType.VarChar)
                    {
                        Value = formatted2
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
                        data.enqlist_Report = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
