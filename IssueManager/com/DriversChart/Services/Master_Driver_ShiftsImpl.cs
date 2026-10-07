using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.DriversChart;
using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.DriversChart.Services
{
    public class Master_Driver_ShiftsImpl :Interface.Master_Driver_ShiftsInterface
    {
        public DriversChartContext _DriversChartContext;
        public DomainModelMsSqlServerContext _db;
        public Master_Driver_ShiftsImpl(DriversChartContext hh, DomainModelMsSqlServerContext db)
        {
            _DriversChartContext = hh;
            _db = db;
            //
        }
        public DriverShiftsDTO getdetails(DriverShiftsDTO data)
        {
            try
            {
                data.drivers_array = _DriversChartContext.MasterDriverShiftsDMO.Where(a => a.VMSMDSH_ActiveFlag == true).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public DriverShiftsDTO savedata(DriverShiftsDTO data)
        {
            try
            {
                var duplicate = 0;
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.VMSMDSH_Id == 0)
                {

                    duplicate = _DriversChartContext.MasterDriverShiftsDMO.Where(e => e.VMSMDSH_ShiftName == data.VMSMDSH_ShiftName && e.VMSMDSH_ActiveFlag==true).Count(); 
                   if(duplicate==0)
                    {
                        MasterDriverShiftsDMO obj = new MasterDriverShiftsDMO();
                        obj.VMSMDSH_Id = data.VMSMDSH_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.VMSMDSH_ShiftName = data.VMSMDSH_ShiftName;
                        obj.VMSMDSH_ActiveFlag = true;
                        obj.VMSMDSH_ShiftStartTime = data.VMSMDSH_ShiftStartTime;
                        obj.VMSMDSH_ShiftEndTime = data.VMSMDSH_ShiftEndTime;
                        obj.VMSMDSH_OnlyOnSpecificDayFlg = data.VMSMDSH_OnlyOnSpecificDayFlg;
                        obj.VMSMDSH_Remarks = data.VMSMDSH_Remarks;
                        obj.VMSMDSH_CreatedDate = indianTime;
                        obj.VMSMDSH_CreatedBy = data.UserId;
                        obj.VMSMDSH_UpdatedBy = data.UserId;
                        obj.VMSMDSH_UpdatedDate = indianTime;

                        _DriversChartContext.Add(obj);

                        int cxt = _DriversChartContext.SaveChanges();
                        if (cxt > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                    else
                    {
                        data.duplicate = true;
                    }
                   
                    //
                }
                else
                {
                    duplicate = _DriversChartContext.MasterDriverShiftsDMO.Where(e => e.VMSMDSH_ShiftName == data.VMSMDSH_ShiftName && e.VMSMDSH_ActiveFlag == true).Count();
                    if (duplicate > 0)
                    {
                        var update = _DriversChartContext.MasterDriverShiftsDMO.Single(e => e.VMSMDSH_Id == data.VMSMDSH_Id);

                        update.VMSMDSH_ShiftName = data.VMSMDSH_ShiftName;

                        update.VMSMDSH_ShiftStartTime = data.VMSMDSH_ShiftStartTime;
                        update.VMSMDSH_ShiftEndTime = data.VMSMDSH_ShiftEndTime;
                        update.VMSMDSH_OnlyOnSpecificDayFlg = data.VMSMDSH_OnlyOnSpecificDayFlg;
                        update.VMSMDSH_Remarks = data.VMSMDSH_Remarks;
                        update.VMSMDSH_UpdatedDate = indianTime;
                        update.VMSMDSH_UpdatedBy = data.UserId;
                        _DriversChartContext.Update(update);
                        int cxt = _DriversChartContext.SaveChanges();
                        if (cxt > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.duplicate = true;
                    }
                }


            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public DriverShiftsDTO Editdata(DriverShiftsDTO data)
        {
            try
            {
                data.edit_array = _DriversChartContext.MasterDriverShiftsDMO.Where(s => s.VMSMDSH_Id == data.VMSMDSH_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


    }
}
