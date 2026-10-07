using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class MasterHolidayImpl : Interfaces.MasterHolidayInterface
    {
        private static ConcurrentDictionary<string, MasterHolidayDTO> _login =
      new ConcurrentDictionary<string, MasterHolidayDTO>();

        public DomainModelMsSqlServerContext _db;
        public FOContext _frnt;
        public MasterHolidayImpl(DomainModelMsSqlServerContext db, FOContext FO)
        {
            _db = db;
            _frnt = FO;
        }


        public MasterHolidayDTO getdata(int id)
        {
            MasterHolidayDTO dto = new MasterHolidayDTO();
            try
            {
                List<HR_Master_LeaveYearDMO> yearlist = new List<HR_Master_LeaveYearDMO>();
                yearlist = _frnt.HR_Master_LeaveYearDMO.Where(a => a.MI_Id == id && a.HRMLY_ActiveFlag.Equals(true)).ToList();
                dto.yeardropdown = yearlist.Distinct().OrderBy(t=>t.HRMLY_LeaveYearOrder).ToArray();


                dto.holidayType = _frnt.holidayWorkingDayType.Where(d => d.MI_Id == id && d.FOHWDT_ActiveFlg == true).ToArray();

                List<FODayNameDMO> days = new List<FODayNameDMO>();
                days = _frnt.FODayNameDMO.Where(d => d.MI_Id == id).ToList();
                dto.dayslist = days.ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "master_holiday_report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",SqlDbType.BigInt)
                    {
                        Value = id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

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
                        dto.report_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
            return dto;
        }

        public MasterHolidayDTO getdetails(int id)
        {
            throw new NotImplementedException();
        }

        public MasterHolidayDTO save_details(MasterHolidayDTO mas)
        {

            var FOHWDT_Id = mas.FOHWDT_Id;
            var MI_id = mas.MI_Id;
            var HRMLY_Id = mas.HRMLY_Id;
            var dayname = mas.FOMHWD_HolidayWDName;
            if (mas.Day_flag == true)
            {
                try
                {
                    if (mas.selectedDaysDate != null)
                    {
                        for (int i = 0; i < mas.selectedDaysDate.Length; i++)
                        {
                            DateTime date = DateTime.ParseExact(mas.selectedDaysDate[i].Date, "dd/MM/yyyy",
                                  CultureInfo.InvariantCulture);
                            var checkduplicate = _frnt.holidaydate.Where(d => d.MI_Id == mas.MI_Id && d.FOMHWDD_FromDate.Value.Date >= date && d.FOMHWDD_ToDate.Value.Date <= date).ToList();
                            if (checkduplicate.Count > 0)
                            {
                                mas.message = "Some record already exist.....!!";
                            }
                            else
                            {
                                try
                                {                                   
                                    FO_Master_HolidayWorkingDay_DatesDMO dates = new FO_Master_HolidayWorkingDay_DatesDMO();
                                    dates.CreatedDate = DateTime.Now;
                                    dates.FOMHWDD_FromDate = date;
                                    dates.FOMHWDD_ToDate = date;
                                    dates.FOMHWD_ActiveFlg = true;
                                    dates.FOMHWDD_Name = mas.selectedDaysDate[i].Dayname;
                                    dates.FOHWDT_Id = FOHWDT_Id;
                                    dates.HRMLY_Id = HRMLY_Id;
                                    dates.MI_Id = MI_id;
                                    dates.UpdatedDate = DateTime.Now;
                                    _frnt.Add(dates);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        var flag = _frnt.SaveChanges();
                        if (flag > 0)
                        {
                            mas.returnval = true;
                        }
                        else
                        {
                            mas.returnval = false;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
            else if (mas.Day_flag == false)
            {
                List<MasterHolidayDTO> duplicate_records = new List<MasterHolidayDTO>();
                duplicate_records = (from a in _frnt.holidaydate
                                     //from b in _frnt.Master_holiday
                                     from c in _frnt.holidayWorkingDayType
                                     where ( a.FOMHWDD_FromDate.Value.Date == mas.FO_Start_Date.Value.Date && a.FOMHWDD_ToDate.Value.Date == mas.FOMHWDD_ToDate.Value.Date  && a.FOHWDT_Id == c.FOHWDT_Id && c.MI_Id==mas.MI_Id)
                                     select new MasterHolidayDTO
                                     {
                                         FOMHWDD_Id = a.FOMHWDD_Id,
                                        // FOMHWD_Id = b.FOMHWD_Id,
                                         FOHWDT_Id = c.FOHWDT_Id,
                                         FOMHWD_HolidayWDName = a.FOMHWDD_Name,
                                     }
                                   ).Distinct().ToList();

                var checkduplicate = duplicate_records.Where(a => a.FOHWDT_Id == mas.FOHWDT_Id && a.FOMHWD_HolidayWDName == mas.FOMHWD_HolidayWDName).ToList();

                if (checkduplicate.Count > 0)
                {
                    mas.message = mas.FOMHWD_HolidayWDName + " already exist.....!!";
                }
                else if (duplicate_records.Count() > 0)
                {                    
                    var result123 = _frnt.holidaydate.Single(x => x.MI_Id == mas.MI_Id && x.FOMHWDD_Id == duplicate_records[0].FOMHWDD_Id);
                    result123.FOMHWDD_Name = mas.FOMHWD_HolidayWDName;
                    result123.FOHWDT_Id = mas.FOHWDT_Id;
                    result123.UpdatedDate = DateTime.Now;
                    _frnt.Update(result123);                   
                    
                    int m = _frnt.SaveChanges();
                    if (m > 0)
                    {
                        mas.returnval = true;
                    }
                    else
                    {
                        mas.returnval = false;
                    }
                }
                else
                {
                    try
                    {
                        FO_Master_HolidayWorkingDay_DatesDMO dates = new FO_Master_HolidayWorkingDay_DatesDMO();
                        dates.CreatedDate = DateTime.Now;
                        dates.FOMHWDD_FromDate = mas.FO_Start_Date;
                        dates.FOMHWDD_ToDate = mas.FOMHWDD_ToDate;
                        dates.FOMHWD_ActiveFlg = true;
                        dates.FOHWDT_Id = FOHWDT_Id;
                        dates.FOMHWDD_Name = dayname;
                        dates.HRMLY_Id = mas.HRMLY_Id;
                        dates.MI_Id = mas.MI_Id;
                        dates.UpdatedDate = DateTime.Now;
                        _frnt.Add(dates);
                        var flag = _frnt.SaveChanges();
                        if (flag > 0)
                        {
                            mas.returnval = true;
                        }
                        else
                        {
                            mas.returnval = false;
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return mas;
        }

        public async Task<MasterHolidayDTO> Change(MasterHolidayDTO mas)
        {
            mas.message = "";
            //  MasterHolidayDMO mm2 = new MasterHolidayDMO();
            FO_Master_HolidayWorkingDay_DatesDMO mm4 = new FO_Master_HolidayWorkingDay_DatesDMO();

            if (mas.Day_flag == true)
            {
                DateTime moment = DateTime.Now;
                int year = moment.Year;

                using (var cmd = _frnt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Fill_Holiday";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@year",
                        SqlDbType.VarChar)
                    {
                        // Value = Convert.ToString(year)
                        Value = mas.HRMLY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

                    try
                    {
                        // var date = "";
                        // var dayname = "";
                        List<MasterHolidayDTO> daynames = new List<MasterHolidayDTO>();
                        // List<string> dates = new List<string>();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {

                                foreach (var itm in mas.daylists1)
                                {
                                    string sttr2 = itm.FOMD_DayName.ToString();
                                    string sttr = (dataReader["Dayname"].ToString()).Substring(0, 3);
                                    if (sttr.ToUpper() == sttr2)

                                    {
                                        daynames.Add(new MasterHolidayDTO
                                        {
                                            Date = dataReader["Date"].ToString(),
                                            Dayname = dataReader["Dayname"].ToString()
                                        });
                                        mas.datesanddayList = daynames.ToArray();
                                    }
                                }
                            }

                        }
                        int n = 0;
                        for (int i = 0; i < daynames.Count(); i++)
                        {
                            if (daynames.Contains(daynames[i]))
                            {
                                //MasterHolidayDMO mm3 = new MasterHolidayDMO();
                                FO_Master_HolidayWorkingDay_DatesDMO mm5 = new FO_Master_HolidayWorkingDay_DatesDMO();

                                //mm3.MI_Id = mas.MI_Id;
                                ////     mm3.FOMHWD_HolidayWDName = daynames[i];
                                //mm3.FOMHWD_ActiveFlg = true;
                                //mm3.FOHWDT_Id = mas.FOHWDT_Id;
                                //mm3.CreatedDate = System.DateTime.Today;
                                //mm3.UpdatedDate = System.DateTime.Today;

                                //_frnt.Add(mm3);

                                mm5.MI_Id = mas.MI_Id;
                                mm5.FOHWDT_Id = mas.FOHWDT_Id;
                                mm5.HRMLY_Id = mas.HRMLY_Id;
                                mm5.FOMHWD_ActiveFlg = true;
                                mm5.FOMHWDD_FromDate = mas.FOMHWDD_FromDate;
                                mm5.FOMHWDD_ToDate = mas.FOMHWDD_ToDate;
                                mm5.CreatedDate = System.DateTime.Today;
                                mm5.UpdatedDate = System.DateTime.Today;

                                _frnt.Add(mm5);

                                n = _frnt.SaveChanges();

                            }
                        }

                        if (n > 0)
                        {
                            mas.returnval = true;
                        }
                        else
                        {
                            mas.returnval = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            else if (mas.Day_flag == false)
            {
                List<MasterHolidayDTO> duplicate_records = new List<MasterHolidayDTO>();
                duplicate_records = (from a in _frnt.holidaydate
                                         //from b in _frnt.Master_holiday
                                     from c in _frnt.holidayWorkingDayType
                                     where (a.MI_Id == c.MI_Id && a.FOMHWDD_FromDate == mas.FO_Start_Date && a.FOMHWDD_ToDate == mas.FOMHWDD_ToDate && c.FOHWDT_Id == a.FOHWDT_Id && a.FOMHWDD_Name == mas.FOMHWDD_Name)
                                     select new MasterHolidayDTO
                                     {
                                         FOMHWDD_Id = a.FOMHWDD_Id,
                                         // FOMHWD_Id = b.FOMHWD_Id,
                                         FOHWDT_Id = c.FOHWDT_Id,
                                         FOMHWD_HolidayWDName = a.FOMHWDD_Name,
                                     }
                                   ).Distinct().ToList();
                var checkduplicate = duplicate_records.Where(a => a.FOHWDT_Id == mas.FOHWDT_Id && a.FOMHWD_HolidayWDName == mas.FOMHWD_HolidayWDName).ToList();

                if (checkduplicate.Count > 0)
                {
                    mas.message = mas.FOMHWD_HolidayWDName + " already exist.....!!";
                }
                else if (duplicate_records.Count() > 0)
                {
                    var result123 = _frnt.Master_holiday.Single(x => x.MI_Id == mas.MI_Id && x.FOMHWD_Id == duplicate_records[0].FOMHWD_Id);
                    result123.FOMHWD_HolidayWDName = mas.FOMHWD_HolidayWDName;
                    result123.FOHWDT_Id = mas.FOHWDT_Id;
                    result123.UpdatedDate = DateTime.Now;
                    _frnt.Update(result123);
                    int m = _frnt.SaveChanges();
                    if (m > 0)
                    {
                        mas.returnval = true;
                    }
                    else
                    {
                        mas.returnval = false;
                    }

                }
                else
                {
                    //mm2.MI_Id = mas.MI_Id;
                    //mm2.FOMHWD_HolidayWDName = mas.FOMHWD_HolidayWDName;
                    //mm2.FOMHWD_ActiveFlg = true;
                    //mm2.FOHWDT_Id = mas.FOHWDT_Id;
                    //mm2.CreatedDate = System.DateTime.Today;
                    //mm2.UpdatedDate = System.DateTime.Today;

                    //_frnt.Add(mm2);

                    mm4.MI_Id = mas.MI_Id;
                    mm4.FOMHWDD_Name = mas.FOMHWD_HolidayWDName;
                    mm4.HRMLY_Id = mas.HRMLY_Id;
                    mm4.FOMHWD_ActiveFlg = true;

                    mm4.FOMHWDD_FromDate = mas.FOMHWDD_FromDate;
                    mm4.FOMHWDD_ToDate = mas.FOMHWDD_ToDate;
                    mm4.CreatedDate = System.DateTime.Today;
                    mm4.UpdatedDate = System.DateTime.Today;

                    _frnt.Add(mm4);

                    int n = _frnt.SaveChanges();
                    if (n > 0)
                    {
                        mas.returnval = true;
                    }
                    else
                    {
                        mas.returnval = false;
                    }



                }
            }
            return mas;
        }

        public MasterHolidayDTO delete_data(int id)
        {
            MasterHolidayDTO rel = new MasterHolidayDTO();
            try
            {
                //var day_id = _frnt.Master_holiday.Where(d => d.FOMHWD_Id == id).ToList();
                var result = _frnt.holidaydate.Where(d => d.FOMHWDD_Id == id).ToList();

                if (result.Any())
                {
                    _frnt.Remove(result.ElementAt(0));

                    if (result.Any())
                    {
                        _frnt.Remove(result.ElementAt(0));
                        var flag = _frnt.SaveChanges();
                        if (flag > 0)
                        {
                            rel.returnval = true;
                        }
                        else
                        {
                            rel.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rel;
        }

        //******************Advance********************///

        public MasterHolidayDTO advloaddata(MasterHolidayDTO data)
        {
            try
            {
                data.employeelist = _frnt.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();

                data.dayslist = _frnt.FODayNameDMO.Where(d => d.MI_Id == data.MI_Id).ToArray();

                data.gridviewdetails = (from a in _frnt.FO_Master_Employee_HolidaysDMO
                                        from b in _frnt.HR_Master_Employee_DMO
                                        where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id)
                                        select new MasterHolidayDTO
                                        {
                                            FOMEH_Id = a.FOMEH_Id,
                                            HRME_Id = a.HRME_Id,
                                            HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                            FOMEH_Date = a.FOMEH_Date,
                                            FOMEH_ActiveFlg = a.FOMEH_ActiveFlg
                                        }).ToArray();

                _frnt.FO_Master_Employee_HolidaysDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterHolidayDTO saveadvmasterHolidaydata(MasterHolidayDTO data)
        {
            try
            {
                FO_Master_Employee_HolidaysDMO obj = new FO_Master_Employee_HolidaysDMO();
                var duplicate = _frnt.FO_Master_Employee_HolidaysDMO.Where(t => t.HRME_Id == data.HRME_Id && t.FOMEH_Date == data.FOMEH_Date && t.FOMEH_Day == data.FOMEH_Day).ToList();
                if (duplicate.Count > 0)
                {
                    data.message = "Duplicate";
                }
                else
                {
                    if (data.FOMEH_Id > 0)
                    {
                        var editdata = _frnt.FO_Master_Employee_HolidaysDMO.Where(t => t.FOMEH_Id == data.FOMEH_Id).FirstOrDefault();
                        editdata.HRME_Id = data.HRME_Id;
                        editdata.FOMEH_Date = data.FOMEH_Date;
                        editdata.FOMEH_Day = data.FOMEH_Day;
                        editdata.UpdatedDate = DateTime.Now;
                        editdata.FOMEH_UpdatedBy = data.LogInId;
                        _frnt.Update(editdata);
                        _frnt.SaveChanges();
                        data.message = "Update";
                    }
                    else
                    {
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.FOMEH_Date = data.FOMEH_Date;
                        obj.FOMEH_Day = data.FOMEH_Day;
                        obj.FOMEH_ActiveFlg = true;
                        obj.FOMEH_CreatedBy = data.LogInId;
                        obj.FOMEH_UpdatedBy = data.LogInId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _frnt.Update(obj);
                        _frnt.SaveChanges();
                        data.message = "Add";
                    }
                }

                data.gridviewdetails = (from a in _frnt.FO_Master_Employee_HolidaysDMO
                                        from b in _frnt.HR_Master_Employee_DMO
                                        where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id)
                                        select new MasterHolidayDTO
                                        {
                                            FOMEH_Id = a.FOMEH_Id,
                                            HRME_Id = a.HRME_Id,
                                            HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                            FOMEH_Date = a.FOMEH_Date,
                                            FOMEH_Day = a.FOMEH_Day,
                                            FOMEH_ActiveFlg = a.FOMEH_ActiveFlg
                                        }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterHolidayDTO advdelete(int id)
        {
            MasterHolidayDTO obj = new MasterHolidayDTO();
            try
            {
                obj.returnval = false;
                var getdata = _frnt.FO_Master_Employee_HolidaysDMO.Where(t => t.FOMEH_Id == id).FirstOrDefault();
                _frnt.Remove(getdata);
                _frnt.SaveChanges();
                obj.returnval = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public MasterHolidayDTO editadvmasterHoliday(MasterHolidayDTO data)
        {
            try
            {
                data.report_list = _frnt.FO_Master_Employee_HolidaysDMO.Where(t => t.FOMEH_Id == data.FOMEH_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterHolidayDTO savedataWeekoff(MasterHolidayDTO data)
        {
            try
            {

                var contactExistsP = _frnt.Database.ExecuteSqlCommand("HR_WeekoffInsert @p0,@p1,@P2",
                      data.HRME_Id, data.FOMEH_Date?.ToString("yyyy-MM-dd") ?? "0", data.LogInId);
                if (contactExistsP > 0)
                {
                    data.message = "Success";
                }
                else
                {
                    data.message = "NotSuccess";
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
