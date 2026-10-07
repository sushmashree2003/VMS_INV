using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class DriverOTImpl : Interfaces.DriverOTInterface
    {
        public FOContext _FOContext;

        public DriverOTImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }
        public DriverOTDTO getdata(DriverOTDTO data)
        {
            try
            {
                var driverlist = (from a in _FOContext.HR_Master_Employee_DMO
                                  from b in _FOContext.HR_Master_Designation_DMO
                                  from c in _FOContext.Institution
                                  where (b.HRMDES_DesignationName.ToUpper() == "DRIVER" && a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && c.MI_ActiveFlag == 1 && a.HRMDES_Id == b.HRMDES_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                  select new DriverOTDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                  });
                data.employeelist = driverlist.ToArray();  

                data.monthlist = _FOContext.Month.Where(t => t.Is_Active == true).ToArray();
                data.yearlist = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).OrderBy(t=>t.HRMLY_LeaveYearOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public DriverOTDTO employeereport(DriverOTDTO data)
        {
            try
            {
                if (data.monthid == 0)
                {
                    var reportdata = (from a in _FOContext.DriverOtDetailsDMO
                                      from b in _FOContext.HR_Master_Employee_DMO
                                      where (a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && a.VMSDT_Punchdate.Year == DateTime.Parse(data.VMSDT_Punchdate.ToString()).Year && a.VMSDT_Punchdate.Month == DateTime.Today.Month)
                                      select new DriverOTDTO
                                      {
                                          VMSDT_ID = a.VMSDT_ID,
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName),
                                          HRME_EmployeeCode = b.HRME_EmployeeCode,
                                          VMSDT_Punchdate = a.VMSDT_Punchdate,
                                          FOMSTDayofWeek = Convert.ToString(a.VMSDT_Punchdate.DayOfWeek),
                                          VMSDT_PunchIn = a.VMSDT_PunchIn,
                                          VMSDT_LateIn = a.VMSDT_LateIn,
                                          VMSDT_PunchOut = a.VMSDT_PunchOut,
                                          VMSDT_EarlyOut = a.VMSDT_EarlyOut,
                                          VMSDT_FinalLatein = a.VMSDT_FinalLatein,
                                          VMSDT_TotalOt = a.VMSDT_TotalOt,
                                          VMSDT_FinalOt = a.VMSDT_FinalOt,
                                          VMSDT_Remarks = a.VMSDT_Remarks
                                      }).OrderBy(t=>t.VMSDT_Punchdate);
                        
                    data.reportdata = reportdata.ToArray();
                }
                else
                {
                    var reportdata = (from a in _FOContext.DriverOtDetailsDMO
                                      from b in _FOContext.HR_Master_Employee_DMO
                                      where (a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && a.VMSDT_Punchdate.Year == data.VMSDT_Punchdate.Year && a.VMSDT_Punchdate.Month == data.monthid)
                                      select new DriverOTDTO
                                      {
                                          VMSDT_ID = a.VMSDT_ID,
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName),
                                          HRME_EmployeeCode = b.HRME_EmployeeCode,
                                          VMSDT_Punchdate = a.VMSDT_Punchdate,
                                          FOMSTDayofWeek = Convert.ToString(a.VMSDT_Punchdate.DayOfWeek),
                                          VMSDT_PunchIn = a.VMSDT_PunchIn,
                                          VMSDT_LateIn = a.VMSDT_LateIn,
                                          VMSDT_PunchOut = a.VMSDT_PunchOut,
                                          VMSDT_EarlyOut = a.VMSDT_EarlyOut,
                                          VMSDT_FinalLatein = a.VMSDT_FinalLatein,
                                          VMSDT_TotalOt = a.VMSDT_TotalOt,
                                          VMSDT_FinalOt = a.VMSDT_FinalOt,
                                          VMSDT_Remarks = a.VMSDT_Remarks
                                      }).OrderBy(t => t.VMSDT_Punchdate);

                    data.reportdata = reportdata.ToArray();
                }

                //MasterShiftsTimingsDMO enq = Mapper.Map<MasterShiftsTimingsDMO>(data);
                //if (enq.FOMST_Id > 0)
                //{
                //    var resultnew = _FOContext.masterShifts.ToList();
                //    var result = _FOContext.masterShiftsTimings.Single();
                //    result.FOMST_FDWHrMin = enq.FOMST_FDWHrMin;
                //    result.FOMST_HDWHrMin = enq.FOMST_HDWHrMin;
                //    result.FOMST_IHalfLoginTime = enq.FOMST_IHalfLoginTime;
                //    result.FOMST_IHalfLogoutTime = enq.FOMST_IHalfLogoutTime;
                //    result.FOMST_IIHalfLoginTime = enq.FOMST_IIHalfLoginTime;
                //    result.FOMST_IIHalfLogoutTime = enq.FOMST_IIHalfLogoutTime;
                //    result.FOMST_DelayPerShiftHrMin = enq.FOMST_DelayPerShiftHrMin;
                //    result.FOMST_EarlyPerShiftHrMin = enq.FOMST_EarlyPerShiftHrMin;
                //    result.FOMST_LunchHoursDuration = enq.FOMST_LunchHoursDuration;
                //    result.FOMST_FixTimings = enq.FOMST_FixTimings;
                //    result.UpdatedDate = DateTime.Now;
                //    _FOContext.Update(result);
                //    DriverOtDetailsDMO enq1 = Mapper.Map<DriverOtDetailsDMO>(data);
                //    var result1 = _FOContext.masterShifts.Single(t => t.FOMS_Id.Equals(result.FOMS_Id));
                //    result1.FOMS_ShiftName = enq1.FOMS_ShiftName;
                //    result1.MI_Id = enq1.MI_Id;
                //    result1.UpdatedDate = DateTime.Now;
                //    _FOContext.Update(result1);
                //    var flag1 = _FOContext.SaveChanges();
                //    //if (flag1 == 2)
                //    //    data.returnvalue = "success";
                //    //else
                //    //    data.returnvalue = "fail";
                //}
                //else
                //{
                //    var resultnew = _FOContext.masterShifts.ToList();
                //    if (resultnew.Count() > 0)
                //    {
                //        //data.returnvalue = "Duplicate";
                //    }
                //    else
                //    {
                //        DriverOtDetailsDMO enqnew = Mapper.Map<DriverOtDetailsDMO>(data);
                //        enqnew.FOMS_ActiveFlg = true;
                //        enqnew.CreatedDate = DateTime.Now;
                //        enqnew.UpdatedDate = DateTime.Now;
                //        _FOContext.Add(enqnew);

                //        MasterShiftsTimingsDMO enqnew1 = Mapper.Map<MasterShiftsTimingsDMO>(data);
                //        enqnew1.FOMS_Id = enqnew.FOMS_Id;
                //        enqnew1.CreatedDate = DateTime.Now;
                //        enqnew1.UpdatedDate = DateTime.Now;
                //        _FOContext.Add(enqnew1);
                //        var flag1 = _FOContext.SaveChanges();
                //        //if (flag1 == 2)
                //        //{
                //        //    data.returnvalue = "success";
                //        //}
                //        //else
                //        //{
                //        //    data.returnvalue = "fail";
                //        //}
                //    }
                //}
                //loadingdata(data, 0);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public DriverOTDTO savedata(DriverOTDTO data)
        {
            try
            {
                TimeSpan late_going_ot;
                TimeSpan early_coming_ot;

                var miid = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();
                var yearid = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id == miid && t.HRMLY_FromDate <= data.VMSDT_Punchdate && t.HRMLY_ToDate >= data.VMSDT_Punchdate).Select(t => t.HRMLY_Id).FirstOrDefault();

                DateTime start_time;
                DateTime end_time;

                var shifttimes = (from b in _FOContext.holidaydate
                                  from c in _FOContext.EmployeeShiftMapping
                                  from e in _FOContext.masterShiftsTimings
                                  where (b.MI_Id == miid && b.HRMLY_Id == yearid && b.FOMHWD_ActiveFlg == true && b.FOMHWDD_FromDate == data.VMSDT_Punchdate && b.FOMHWDD_ToDate == data.VMSDT_Punchdate && c.MI_Id == miid && c.HRME_Id == data.HRME_Id && c.FOHWDT_Id == b.FOHWDT_Id && e.FOMS_Id == c.FOMS_Id)
                                  select new DriverOTDTO
                                  {
                                      FOMS_Id = e.FOMS_Id,
                                      FOMST_IHalfLoginTime = e.FOMST_IHalfLoginTime,
                                      FOMST_IIHalfLogoutTime = e.FOMST_IIHalfLogoutTime
                                  }).FirstOrDefault();

                if(shifttimes == null) { data.returnvalue = "Shift"; }
                
                start_time = Convert.ToDateTime(shifttimes.FOMST_IHalfLoginTime.ToString());
                end_time = Convert.ToDateTime(shifttimes.FOMST_IIHalfLogoutTime.ToString());
                data.VMSDT_ShiftId = shifttimes.FOMS_Id;

                System.TimeSpan late_by = Convert.ToDateTime(data.VMSDT_PunchIn).Subtract(start_time);
                if (late_by.TotalMinutes > 0)
                {
                    data.VMSDT_LateIn = late_by.ToString();
                    data.VMSDT_EarlyComingOt = "00:00:00";
                }
                else
                {
                    data.VMSDT_LateIn = "00:00:00";
                    early_coming_ot = start_time.Subtract(Convert.ToDateTime(data.VMSDT_PunchIn));
                    if (early_coming_ot.TotalMinutes > 0)
                    {
                        data.VMSDT_EarlyComingOt = early_coming_ot.ToString();
                    }
                    else
                    {
                        data.VMSDT_EarlyComingOt = "00:00:00";
                    }
                }

                System.TimeSpan early_out = end_time.Subtract(Convert.ToDateTime(data.VMSDT_PunchOut));
                if (early_out.TotalMinutes > 0)
                {
                    data.VMSDT_EarlyOut = early_out.ToString();
                    data.VMSDT_LategoingOt = "00:00:00";
                }
                else
                {
                    data.VMSDT_EarlyOut = "00:00:00";
                    late_going_ot = Convert.ToDateTime(data.VMSDT_PunchOut).Subtract(end_time);
                    if (late_going_ot.TotalMinutes > 0)
                    {
                        data.VMSDT_LategoingOt = late_going_ot.ToString();
                    }
                    else
                    {
                        data.VMSDT_LategoingOt = "00:00:00";
                    }
                }

                TimeSpan total_ot1;
                if (data.withpreviousdayflag == false)
                {
                    DateTime in1 = Convert.ToDateTime(data.VMSDT_PunchIn);
                    DateTime out1 = Convert.ToDateTime(data.VMSDT_PunchOut);
                    TimeSpan ssn1 = out1.Subtract(in1);
                    DateTime tt = Convert.ToDateTime("00:00:00");
                    DateTime tt1 = Convert.ToDateTime("10:15:00");
                    TimeSpan tt5 = tt1.Subtract(tt);
                    total_ot1 = ssn1.Subtract(tt5);
                    data.VMSDT_TotalOt = total_ot1.ToString();
                }
                else
                {
                    data.VMSDT_LateIn = "00:00:00";
                    data.VMSDT_EarlyComingOt = "00:00:00";
                    DateTime in1 = Convert.ToDateTime(data.VMSDT_PunchIn);
                    DateTime out1 = Convert.ToDateTime(data.VMSDT_PunchOut);
                    total_ot1 = out1.Subtract(in1);
                    data.VMSDT_TotalOt = total_ot1.ToString();
                }

                var finallastdayot = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == data.HRME_Id && t.VMSDT_Punchdate.Year == data.VMSDT_Punchdate.Year && t.VMSDT_Punchdate.Month == data.VMSDT_Punchdate.Month && t.VMSDT_Punchdate.Date < data.VMSDT_Punchdate.Date).OrderByDescending(t => t.VMSDT_ID).ToArray();

                if (finallastdayot.Length > 0)
                {
                    if (finallastdayot[0].VMSDT_FinalOt.ToString() == "")
                    {
                        data.VMSDT_FinalOt = total_ot1.ToString();
                    }
                    else
                    {
                        data.VMSDT_FinalOt = Convert.ToString(TimeSpan.Parse(finallastdayot[0].VMSDT_FinalOt.ToString()) + TimeSpan.Parse(total_ot1.ToString()));
                    }

                    if (finallastdayot[0].VMSDT_FinalLatein.ToString() == "")
                    {
                        data.VMSDT_FinalLatein = data.VMSDT_LateIn.ToString();
                    }
                    else
                    {
                        data.VMSDT_FinalLatein = Convert.ToString(TimeSpan.Parse(finallastdayot[0].VMSDT_FinalLatein.ToString()) + TimeSpan.Parse(data.VMSDT_LateIn.ToString()));
                    }
                }
                else
                {
                    data.VMSDT_FinalOt = total_ot1.ToString();
                    data.VMSDT_FinalLatein = data.VMSDT_LateIn.ToString();
                }

                DriverOtDetailsDMO otdata = Mapper.Map<DriverOtDetailsDMO>(data);
                var duplicate = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == otdata.HRME_Id && t.VMSDT_Punchdate == otdata.VMSDT_Punchdate).ToArray();
                if (duplicate.Length == 0)
                {
                    otdata.CreatedDate = DateTime.Today;
                    otdata.UpdatedDate = DateTime.Today;
                    otdata.VMSDT_CreatedBy = data.UserId;
                    otdata.VMSDT_UpdatedBy = data.UserId;
                    _FOContext.Add(otdata);
                    _FOContext.SaveChanges();
                    data.returnvalue = "success";
                }
                else
                {
                    if (data.withpreviousdayflag == true)
                    {
                        if (finallastdayot.Length > 0)
                        {
                            otdata.VMSDT_FinalLatein = Convert.ToString(TimeSpan.Parse(finallastdayot[0].VMSDT_FinalLatein.ToString()));
                        }
                        else
                        {
                            var finaltodayot = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == data.HRME_Id && t.VMSDT_Punchdate.Year == data.VMSDT_Punchdate.Year && t.VMSDT_Punchdate.Month == data.VMSDT_Punchdate.Month && t.VMSDT_Punchdate.Date == data.VMSDT_Punchdate.Date).OrderByDescending(t => t.VMSDT_ID).ToArray();

                            if (finaltodayot[0].VMSDT_FinalOt.ToString() == "")
                            {
                                otdata.VMSDT_FinalOt = total_ot1.ToString();
                            }
                            else
                            {
                                otdata.VMSDT_FinalOt = Convert.ToString(TimeSpan.Parse(finaltodayot[0].VMSDT_FinalOt.ToString()) + TimeSpan.Parse(total_ot1.ToString()));
                            }

                            if (finaltodayot[0].VMSDT_FinalLatein.ToString() == "")
                            {
                                otdata.VMSDT_FinalLatein = data.VMSDT_LateIn.ToString();
                            }
                            else
                            {
                                otdata.VMSDT_FinalLatein = Convert.ToString(TimeSpan.Parse(finaltodayot[0].VMSDT_FinalLatein.ToString()) + TimeSpan.Parse(data.VMSDT_LateIn.ToString()));
                            }
                        }

                        otdata.CreatedDate = DateTime.Today;
                        otdata.UpdatedDate = DateTime.Today;
                        otdata.VMSDT_CreatedBy = data.UserId;
                        otdata.VMSDT_UpdatedBy = data.UserId;
                        _FOContext.Add(otdata);
                        _FOContext.SaveChanges();
                        data.returnvalue = "success";
                    }
                    else
                    {
                        if (data.VMSDT_ID > 0)
                        {
                            var editotdata = _FOContext.DriverOtDetailsDMO.Where(t => t.VMSDT_ID == data.VMSDT_ID).FirstOrDefault();
                            editotdata.HRME_Id = otdata.HRME_Id;
                            editotdata.VMSDT_Punchdate = otdata.VMSDT_Punchdate;
                            editotdata.VMSDT_PunchIn = otdata.VMSDT_PunchIn;
                            editotdata.VMSDT_PunchOut = otdata.VMSDT_PunchOut;
                            editotdata.VMSDT_LateIn = otdata.VMSDT_LateIn;
                            editotdata.VMSDT_EarlyOut = otdata.VMSDT_EarlyOut;
                            editotdata.VMSDT_EarlyComingOt = otdata.VMSDT_EarlyComingOt;
                            editotdata.VMSDT_LategoingOt = otdata.VMSDT_LategoingOt;
                            editotdata.VMSDT_TotalOt = otdata.VMSDT_TotalOt;
                            editotdata.VMSDT_FinalOt = otdata.VMSDT_FinalOt;
                            editotdata.VMSDT_Remarks = otdata.VMSDT_Remarks;
                            editotdata.VMSDT_MonthId = otdata.VMSDT_MonthId;
                            editotdata.VMSDT_FinalLatein = otdata.VMSDT_FinalLatein;
                            editotdata.VMSDT_ShiftId = otdata.VMSDT_ShiftId;
                            editotdata.withpreviousdayflag = otdata.withpreviousdayflag;
                            editotdata.UpdatedDate = DateTime.Today;
                            editotdata.VMSDT_UpdatedBy = data.UserId;
                            _FOContext.Update(editotdata);
                            _FOContext.SaveChanges();
                            data.returnvalue = "success";

                            var nextdayots = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == data.HRME_Id && t.VMSDT_Punchdate.Year == data.VMSDT_Punchdate.Year && t.VMSDT_Punchdate.Month == data.VMSDT_Punchdate.Month && t.VMSDT_Punchdate.Date > data.VMSDT_Punchdate.Date).OrderBy(t => t.VMSDT_ID).ToArray();
                            if (nextdayots.Length > 0)
                            {
                                for (int cont = 0; cont < nextdayots.Length; cont++)
                                {
                                    var lastdaydata = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == data.HRME_Id && t.VMSDT_ID == nextdayots[cont].VMSDT_ID).FirstOrDefault();
                                    DriverOtDetailsDMO lastdata = Mapper.Map<DriverOtDetailsDMO>(lastdaydata);
                                    var editshifttimes = (from e in _FOContext.masterShiftsTimings where (e.FOMS_Id == lastdata.VMSDT_ShiftId)
                                                      select new DriverOTDTO
                                                      {
                                                          FOMS_Id = e.FOMS_Id,
                                                          FOMST_IHalfLoginTime = e.FOMST_IHalfLoginTime,
                                                          FOMST_IIHalfLogoutTime = e.FOMST_IIHalfLogoutTime
                                                      }).FirstOrDefault();

                                    start_time = Convert.ToDateTime(editshifttimes.FOMST_IHalfLoginTime.ToString());
                                    end_time = Convert.ToDateTime(editshifttimes.FOMST_IIHalfLogoutTime.ToString());
                                    lastdata.VMSDT_ShiftId = editshifttimes.FOMS_Id;

                                    System.TimeSpan editlate_by = Convert.ToDateTime(lastdata.VMSDT_PunchIn).Subtract(start_time);
                                    if (editlate_by.TotalMinutes > 0)
                                    {
                                        lastdata.VMSDT_LateIn = editlate_by.ToString();
                                        lastdata.VMSDT_EarlyComingOt = "00:00:00";
                                    }
                                    else
                                    {
                                        lastdata.VMSDT_LateIn = "00:00:00";
                                        early_coming_ot = start_time.Subtract(Convert.ToDateTime(lastdata.VMSDT_PunchIn));
                                        if (early_coming_ot.TotalMinutes > 0)
                                        {
                                            lastdata.VMSDT_EarlyComingOt = early_coming_ot.ToString();
                                        }
                                        else
                                        {
                                            lastdata.VMSDT_EarlyComingOt = "00:00:00";
                                        }
                                    }

                                    System.TimeSpan editearly_out = end_time.Subtract(Convert.ToDateTime(lastdata.VMSDT_PunchOut));
                                    if (editearly_out.TotalMinutes > 0)
                                    {
                                        lastdata.VMSDT_EarlyOut = editearly_out.ToString();
                                        lastdata.VMSDT_LategoingOt = "00:00:00";
                                    }
                                    else
                                    {
                                        lastdata.VMSDT_EarlyOut = "00:00:00";
                                        late_going_ot = Convert.ToDateTime(lastdata.VMSDT_PunchOut).Subtract(end_time);
                                        if (late_going_ot.TotalMinutes > 0)
                                        {
                                            lastdata.VMSDT_LategoingOt = late_going_ot.ToString();
                                        }
                                        else
                                        {
                                            lastdata.VMSDT_LategoingOt = "00:00:00";
                                        }
                                    }

                                    TimeSpan edittotal_ot1;
                                    if (lastdata.withpreviousdayflag == false)
                                    {
                                        DateTime in1 = Convert.ToDateTime(lastdata.VMSDT_PunchIn);
                                        DateTime out1 = Convert.ToDateTime(lastdata.VMSDT_PunchOut);
                                        TimeSpan ssn1 = out1.Subtract(in1);
                                        DateTime tt = Convert.ToDateTime("00:00:00");
                                        DateTime tt1 = Convert.ToDateTime("10:15:00");
                                        TimeSpan tt5 = tt1.Subtract(tt);
                                        edittotal_ot1 = ssn1.Subtract(tt5);
                                        lastdata.VMSDT_TotalOt = edittotal_ot1.ToString();
                                    }
                                    else
                                    {
                                        lastdata.VMSDT_LateIn = "00:00:00";
                                        lastdata.VMSDT_EarlyComingOt = "00:00:00";
                                        DateTime in1 = Convert.ToDateTime(lastdata.VMSDT_PunchIn);
                                        DateTime out1 = Convert.ToDateTime(lastdata.VMSDT_PunchOut);
                                        edittotal_ot1 = out1.Subtract(in1);
                                        lastdata.VMSDT_TotalOt = edittotal_ot1.ToString();
                                    }

                                    var editfinallastdayot = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == lastdata.HRME_Id && t.VMSDT_Punchdate.Year == lastdata.VMSDT_Punchdate.Year && t.VMSDT_Punchdate.Month == lastdata.VMSDT_Punchdate.Month && t.VMSDT_Punchdate.Date < lastdata.VMSDT_Punchdate.Date).OrderByDescending(t => t.VMSDT_ID).ToArray();

                                    if (editfinallastdayot.Length > 0)
                                    {
                                        if (editfinallastdayot[0].VMSDT_FinalOt.ToString() == "")
                                        {
                                            lastdata.VMSDT_FinalOt = edittotal_ot1.ToString();
                                        }
                                        else
                                        {
                                            lastdata.VMSDT_FinalOt = Convert.ToString(TimeSpan.Parse(editfinallastdayot[0].VMSDT_FinalOt.ToString()) + TimeSpan.Parse(edittotal_ot1.ToString()));
                                        }

                                        if (editfinallastdayot[0].VMSDT_FinalLatein.ToString() == "")
                                        {
                                            lastdata.VMSDT_FinalLatein = lastdata.VMSDT_LateIn;
                                        }
                                        else
                                        {
                                            lastdata.VMSDT_FinalLatein = Convert.ToString(TimeSpan.Parse(editfinallastdayot[0].VMSDT_FinalLatein.ToString()) + TimeSpan.Parse(lastdata.VMSDT_LateIn));
                                        }
                                    }
                                    else
                                    {
                                        lastdata.VMSDT_FinalOt = edittotal_ot1.ToString();
                                        lastdata.VMSDT_FinalLatein = lastdata.VMSDT_LateIn.ToString();
                                    }

                                    lastdata.UpdatedDate = DateTime.Today;
                                    lastdata.VMSDT_UpdatedBy = data.UserId;
                                    _FOContext.Update(lastdata);
                                    _FOContext.SaveChanges();
                                    data.returnvalue = "success";
                                }
                            }
                        }
                        else {
                            //data.returnvalue = "Duplicate";
                            otdata.VMSDT_FinalLatein = Convert.ToString(TimeSpan.Parse(finallastdayot[0].VMSDT_FinalLatein.ToString()));
                            otdata.CreatedDate = DateTime.Today;
                            otdata.UpdatedDate = DateTime.Today;
                            otdata.VMSDT_CreatedBy = data.UserId;
                            otdata.VMSDT_UpdatedBy = data.UserId;
                            _FOContext.Add(otdata);
                            _FOContext.SaveChanges();
                            data.returnvalue = "success";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        public DriverOTDTO getdetails(int id)
        {
            DriverOTDTO data = new DriverOTDTO();
            var getdata = _FOContext.DriverOtDetailsDMO.Where(t => t.VMSDT_ID == id);
            data.editdata = getdata.ToArray();
            return data;
        }
        public DriverOTDTO deletedata(int id)
        {
            DriverOTDTO data = new DriverOTDTO();
            var getdata = _FOContext.DriverOtDetailsDMO.Where(t => t.VMSDT_ID == id);
            _FOContext.Remove(getdata);
            _FOContext.SaveChanges();
            data.returnvalue = "success";
            return data;
        }
        public DriverOTDTO driverInTimeEntry(DriverOTDTO data)
        {
            try
            {
                TimeSpan early_coming_ot;
                DateTime start_time;

                data.HRME_Id = _FOContext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                var miid = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();
                var yearid = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id == miid && t.HRMLY_FromDate <= data.VMSDT_Punchdate && t.HRMLY_ToDate >= data.VMSDT_Punchdate).Select(t => t.HRMLY_Id).FirstOrDefault();

                var shifttimes = (from b in _FOContext.holidaydate
                                  from c in _FOContext.EmployeeShiftMapping
                                  from e in _FOContext.masterShiftsTimings
                                  where (b.MI_Id == miid && b.HRMLY_Id == yearid && b.FOMHWD_ActiveFlg == true && b.FOMHWDD_FromDate == data.VMSDT_Punchdate && b.FOMHWDD_ToDate == data.VMSDT_Punchdate && c.MI_Id == miid && c.HRME_Id == data.HRME_Id && c.FOHWDT_Id == b.FOHWDT_Id && e.FOMS_Id == c.FOMS_Id)
                                  select new DriverOTDTO
                                  {
                                      FOMS_Id = e.FOMS_Id,
                                      FOMST_IHalfLoginTime = e.FOMST_IHalfLoginTime,
                                      FOMST_IIHalfLogoutTime = e.FOMST_IIHalfLogoutTime
                                  }).FirstOrDefault();

                start_time = Convert.ToDateTime(shifttimes.FOMST_IHalfLoginTime.ToString());
                data.VMSDT_ShiftId = shifttimes.FOMS_Id;

                System.TimeSpan late_by = Convert.ToDateTime(data.VMSDT_PunchIn).Subtract(start_time);
                if (late_by.TotalMinutes > 0)
                {
                    data.VMSDT_LateIn = late_by.ToString();
                    data.VMSDT_EarlyComingOt = "00:00:00";
                }
                else
                {
                    data.VMSDT_LateIn = "00:00:00";
                    early_coming_ot = start_time.Subtract(Convert.ToDateTime(data.VMSDT_PunchIn));
                    if (early_coming_ot.TotalMinutes > 0)
                    {
                        data.VMSDT_EarlyComingOt = early_coming_ot.ToString();
                    }
                    else
                    {
                        data.VMSDT_EarlyComingOt = "00:00:00";
                    }
                }

                DateTime in1 = Convert.ToDateTime(data.VMSDT_PunchIn);
                DateTime tt = Convert.ToDateTime("00:00:00");
                DateTime tt1 = Convert.ToDateTime("10:15:00");
                TimeSpan tt5 = tt1.Subtract(tt);

                DriverOtDetailsDMO otdata = Mapper.Map<DriverOtDetailsDMO>(data);
                var duplicate = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == otdata.HRME_Id && t.VMSDT_Punchdate == otdata.VMSDT_Punchdate).ToArray();
                if (duplicate.Length == 0)
                {
                    otdata.CreatedDate = DateTime.Today;
                    otdata.UpdatedDate = DateTime.Today;
                    otdata.VMSDT_CreatedBy = data.UserId;
                    otdata.VMSDT_UpdatedBy = data.UserId;
                    _FOContext.Add(otdata);
                    _FOContext.SaveChanges();
                    data.returnvalue = "success";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public DriverOTDTO driverOutTimeEntry(DriverOTDTO data)
        {
            try
            {
                TimeSpan late_going_ot;
                DateTime end_time;

                data.HRME_Id = _FOContext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                var miid = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();
                var yearid = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id == miid && t.HRMLY_FromDate <= data.VMSDT_Punchdate && t.HRMLY_ToDate >= data.VMSDT_Punchdate).Select(t => t.HRMLY_Id).FirstOrDefault();

                var shifttimes = (from b in _FOContext.holidaydate
                                  from c in _FOContext.EmployeeShiftMapping
                                  from e in _FOContext.masterShiftsTimings
                                  where (b.MI_Id == miid && b.HRMLY_Id == yearid && b.FOMHWD_ActiveFlg == true && b.FOMHWDD_FromDate == data.VMSDT_Punchdate && b.FOMHWDD_ToDate == data.VMSDT_Punchdate && c.MI_Id == miid && c.HRME_Id == data.HRME_Id && c.FOHWDT_Id == b.FOHWDT_Id && e.FOMS_Id == c.FOMS_Id)
                                  select new DriverOTDTO
                                  {
                                      FOMST_IHalfLoginTime = e.FOMST_IHalfLoginTime,
                                      FOMST_IIHalfLogoutTime = e.FOMST_IIHalfLogoutTime
                                  }).FirstOrDefault();

                end_time = Convert.ToDateTime(shifttimes.FOMST_IIHalfLogoutTime.ToString());

                System.TimeSpan early_out = end_time.Subtract(Convert.ToDateTime(data.VMSDT_PunchOut));
                if (early_out.TotalMinutes > 0)
                {
                    data.VMSDT_EarlyOut = early_out.ToString();
                    data.VMSDT_LategoingOt = "00:00:00";
                }
                else
                {
                    data.VMSDT_EarlyOut = "00:00:00";
                    late_going_ot = Convert.ToDateTime(data.VMSDT_PunchOut).Subtract(end_time);
                    if (late_going_ot.TotalMinutes > 0)
                    {
                        data.VMSDT_LategoingOt = late_going_ot.ToString();
                    }
                    else
                    {
                        data.VMSDT_LategoingOt = "00:00:00";
                    }
                }

                TimeSpan total_ot1;
                if (data.withpreviousdayflag == false)
                {
                    data.VMSDT_PunchIn = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == data.HRME_Id && t.VMSDT_Punchdate.Year == data.VMSDT_Punchdate.Year && t.VMSDT_Punchdate.Month == data.VMSDT_Punchdate.Month && t.VMSDT_Punchdate.Date < data.VMSDT_Punchdate.Date).Select(t => t.VMSDT_PunchIn).FirstOrDefault();
                    DateTime in1 = Convert.ToDateTime(data.VMSDT_PunchIn);
                    DateTime out1 = Convert.ToDateTime(data.VMSDT_PunchOut);
                    TimeSpan ssn1 = out1.Subtract(in1);
                    DateTime tt = Convert.ToDateTime("00:00:00");
                    DateTime tt1 = Convert.ToDateTime("10:15:00");
                    TimeSpan tt5 = tt1.Subtract(tt);
                    total_ot1 = ssn1.Subtract(tt5);
                    data.VMSDT_TotalOt = total_ot1.ToString();
                }
                else
                {
                    data.VMSDT_LateIn = "00:00:00";
                    data.VMSDT_EarlyComingOt = "00:00:00";
                    DateTime in1 = Convert.ToDateTime(data.VMSDT_PunchIn);
                    DateTime out1 = Convert.ToDateTime(data.VMSDT_PunchOut);
                    total_ot1 = out1.Subtract(in1);
                    data.VMSDT_TotalOt = total_ot1.ToString();
                }

                var finallastdayot = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == data.HRME_Id && t.VMSDT_Punchdate.Year == data.VMSDT_Punchdate.Year && t.VMSDT_Punchdate.Month == data.VMSDT_Punchdate.Month && t.VMSDT_Punchdate.Date < data.VMSDT_Punchdate.Date).OrderByDescending(t => t.VMSDT_ID).ToArray();

                if (finallastdayot.Length > 0)
                {
                    if (finallastdayot[0].VMSDT_FinalOt.ToString() == "")
                    {
                        data.VMSDT_FinalOt = total_ot1.ToString();
                    }
                    else
                    {
                        data.VMSDT_FinalOt = Convert.ToString(TimeSpan.Parse(finallastdayot[0].VMSDT_FinalOt.ToString()) + TimeSpan.Parse(total_ot1.ToString()));
                    }
                }
                else
                {
                    data.VMSDT_FinalOt = total_ot1.ToString();
                    data.VMSDT_FinalLatein = data.VMSDT_LateIn.ToString();
                }

                DriverOtDetailsDMO otdata = Mapper.Map<DriverOtDetailsDMO>(data);
                var duplicate = _FOContext.DriverOtDetailsDMO.Where(t => t.HRME_Id == otdata.HRME_Id && t.VMSDT_Punchdate == otdata.VMSDT_Punchdate).ToArray();
                if (duplicate.Length == 1)
                {
                    otdata.CreatedDate = DateTime.Today;
                    otdata.UpdatedDate = DateTime.Today;
                    otdata.VMSDT_CreatedBy = data.UserId;
                    otdata.VMSDT_UpdatedBy = data.UserId;
                    _FOContext.Add(otdata);
                    _FOContext.SaveChanges();
                    data.returnvalue = "success";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<DriverOTDTO> getApprovalStatusAsync(DriverOTDTO data)
        {
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DriverOt_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.otapprovallist = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.reportdata = (from a in _FOContext.DriverOtDetailsDMO
                                   from b in _FOContext.VMSDriverOTApprovalDMO
                                   from c in _FOContext.HR_Master_Employee_DMO
                                   where (a.VMSDT_ID == b.VMSDT_Id && a.HRME_Id == c.HRME_Id && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false)
                                   select new DriverOTDTO {
                                       VMSDT_ID = a.VMSDT_ID,
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeCode = c.HRME_EmployeeCode,
                                       VMSDT_Punchdate = a.VMSDT_Punchdate,
                                       VMSDT_PunchIn = a.VMSDT_PunchIn,
                                       VMSDT_PunchOut = a.VMSDT_PunchOut,
                                       VMSDT_TotalOt = a.VMSDT_TotalOt,
                                       VSMDTA_Status = b.VSMDTA_Status,
                                       VSMDTA_Remarks = b.VSMDTA_Remarks,
                                       HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName),
                                   }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public DriverOTDTO get_status(DriverOTDTO data)
        {
            try
            {
                string status = "Approved";
                if (data.get_OT_status != null)
                {
                    for (var i = 0; i < data.get_OT_status.Count(); i++)
                    {
                        VMSDriverOTApprovalDMO objapprove = new VMSDriverOTApprovalDMO();
                        objapprove.VMSDT_Id = data.get_OT_status[i].VMSDT_ID;
                        objapprove.VSMDTA_Status = status;
                        objapprove.VSMDTA_Remarks = data.VSMDTA_Remarks;
                        objapprove.VSMDTA_ApprovedBy = data.UserId;
                        objapprove.VSMDTA_ApprovedDate = System.DateTime.Today;
                        objapprove.CreatedDate = System.DateTime.Today;
                        objapprove.UpdatedDate = System.DateTime.Today;
                        objapprove.CreatedBy = data.UserId;
                        objapprove.UpdatedBy = data.UserId;
                        _FOContext.Add(objapprove);
                        _FOContext.SaveChanges();
                    }
                }
                data.returnvalue = "Success";
            }
            catch(Exception ex)
            {
                data.returnvalue = "false";
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public DriverOTDTO reject_status(DriverOTDTO data)
        {
            try
            {
                string status = "Rejected";
                if (data.get_OT_status != null)
                {
                    for (var i = 0; i < data.get_OT_status.Count(); i++)
                    {
                        VMSDriverOTApprovalDMO objapprove = new VMSDriverOTApprovalDMO();
                        objapprove.VMSDT_Id = data.get_OT_status[i].VMSDT_ID;
                        objapprove.VSMDTA_Status = status;
                        objapprove.VSMDTA_Remarks = data.VSMDTA_Remarks;
                        objapprove.VSMDTA_ApprovedBy = data.UserId;
                        objapprove.VSMDTA_ApprovedDate = System.DateTime.Today;
                        objapprove.CreatedDate = System.DateTime.Today;
                        objapprove.UpdatedDate = System.DateTime.Today;
                        objapprove.CreatedBy = data.UserId;
                        objapprove.UpdatedBy = data.UserId;
                        _FOContext.Add(objapprove);
                        _FOContext.SaveChanges();
                    }
                }
                data.returnvalue = "Success";
            }
            catch (Exception ex)
            {
                data.returnvalue = "false";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<DriverOTDTO> OTFinalReportAsync(DriverOTDTO data)
        {
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_DriverOt_Approved_Detilas_Checking";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.BigInt)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@Month_Id", SqlDbType.BigInt)
                    {
                        Value = data.monthid
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader11 = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader11.ReadAsync())
                            {
                                var dataRow11 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled11 = 0; iFiled11 < dataReader11.FieldCount; iFiled11++)
                                {
                                    dataRow11.Add(
                                        dataReader11.GetName(iFiled11),
                                        dataReader11.IsDBNull(iFiled11) ? 0 : dataReader11[iFiled11] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow11);
                            }
                        }
                        data.finalotreport = retObject1.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                    return data;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
