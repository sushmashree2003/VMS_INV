using DataAccessMsSqlServerProvider;
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
    public class SBReportImpl:Interfaces.SBReportInterface
    {
        public SpaceBookingContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public SBReportImpl(SpaceBookingContext sp1, DomainModelMsSqlServerContext sp2)
        {
            _VMSContext = sp1;
            _Context = sp2;
        }

        public SBReport_DTO getdetails(SBReport_DTO data)
        {
            try
            {
                var au = _Context.ApplicationUser.Single(a => a.Id == data.UserId);
                var ur = _Context.appUserRole.Single(a => a.UserId == au.Id);
                var user = _Context.applicationRole.Single(a => a.Id == ur.RoleId);
                data.userflag = user.Name;
                //data.roomname_list = _VMSContext.HR_Master_Room.Where(a => a.HRMR_ActiveFlag == true && a.MI_Id==data.MI_Id && a.HRMR_OutSideBookingFlg==true).Distinct().ToArray();
                data.milist = _VMSContext.Institution.Where(a => a.MI_ActiveFlag ==1).ToArray();
                data.miid = data.MI_Id;
                data.Master_Institution = _VMSContext.Institution.Where(p => p.MI_ActiveFlag == 1).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public SBReport_DTO registration_report(SBReport_DTO data)
        {       
            try
            {
                data.userReg_Report = (from a in _VMSContext.Office_Space_User_Registration
                                       where (a.MI_ID == data.MI_Id_new && a.CreatedDate.Value.Date >= data.fromdate1
                                       && a.CreatedDate.Value.Date <= data.fromdate2)
                                       select a).Distinct().ToArray();
                data.company_name = _VMSContext.Institution.Single(a => a.MI_ActiveFlag == 1 && a.MI_Id==data.MI_Id_new).MI_Name;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public async Task<SBReport_DTO> enquary_report(SBReport_DTO data)
        {
            try
            {
                string formatted1 = "";
                string formatted2 = "";

                if (data.fromdate1!=null || Convert.ToString(data.fromdate1) !="")
                {
                     formatted1 = data.fromdate1.Value.ToString("yyyy-MM-dd");
                }
                if (data.fromdate2 != null || Convert.ToString(data.fromdate2) != "")
                {
                     formatted2 = data.fromdate2.Value.ToString("yyyy-MM-dd");
                }

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "OS_EnquaryList_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id_new
                    });
                    cmd.Parameters.Add(new SqlParameter("@enqstatus",
                     SqlDbType.VarChar)
                    {
                        Value = data.enqstatus
                    });
                    cmd.Parameters.Add(new SqlParameter("@enqtype",
                     SqlDbType.VarChar)
                    {
                        Value = data.enqtype
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
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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

                        data.company_name = _VMSContext.Institution.Single(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id_new).MI_Name;
                    }
                   
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public async Task<SBReport_DTO> booking_report(SBReport_DTO data)
        {
            try
            {
                string room_ids = "0";
                List<long> room1_ids = new List<long>();
                foreach (var item in data.selectedroomlist)
                {
                    room1_ids.Add(item.HRMR_Id);
                }
                for (int s = 0; s < room1_ids.Count(); s++)
                {
                    room_ids = room_ids + ',' + room1_ids[s].ToString();
                }
                string mI_IDSss = "0";
                List<long> MI_IDS = new List<long>();
                foreach (var c in data.MI_IDList)
                {
                    MI_IDS.Add(c.MI_IDS);
                }
                for (int k = 0; k < MI_IDS.Count(); k++)
                {
                    mI_IDSss = mI_IDSss + ',' + MI_IDS[k].ToString();
                }
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
                    cmd.CommandText = "OS_SPACEBOOKING_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = mI_IDSss
                    });
                    cmd.Parameters.Add(new SqlParameter("@bookingstatus",
                     SqlDbType.VarChar)
                    {
                        Value = data.bookingstatus
                    });
                    cmd.Parameters.Add(new SqlParameter("@bookingtype",
                     SqlDbType.VarChar)
                    {
                        Value = data.bookingtype
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
                    cmd.Parameters.Add(new SqlParameter("@HRMR_Id",
                     SqlDbType.VarChar)
                    {
                        Value = room_ids
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.bookinglist_Report = retObject.ToArray();
                        }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //data.company_name = _VMSContext.Institution.Single(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id_new).MI_Name;
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
            return data;
        }

        public async Task<SBReport_DTO> getroom(SBReport_DTO dto)
        {
            try
            {
                List<long> MI_Id = new List<long>();
                foreach (var c in dto.MI_IDList)
                {
                    MI_Id.Add(c.MI_IDS);
                }
                dto.roomname_list = _VMSContext.HR_Master_Room.Where(a => a.HRMR_ActiveFlag == true && a.HRMR_OutSideBookingFlg == true 
                 && MI_Id.Contains(a.MI_Id)).Distinct().ToArray();
            }


            catch (Exception ex)
            {
                throw ex;
            }

            return dto;
        }


    }
}
