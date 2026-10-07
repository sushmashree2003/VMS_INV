using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.DriversChart;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.DriversChart.Services
{
    public class DriverChartGenerationImpl : Interface.DriverChartGenerationInterface
    {
        public DriversChartContext _DriversChartContext;
        public DomainModelMsSqlServerContext _db;
        public DriverChartGenerationImpl(DriversChartContext hh, DomainModelMsSqlServerContext db)
        {
            _DriversChartContext = hh;
            _db = db;
            //
        }
        public DriverChartGenerationDTO getdetails(DriverChartGenerationDTO data)
        {
            try
            {
                data.master_shift = _DriversChartContext.MasterDriverShiftsDMO.Where(a => a.VMSMDSH_ActiveFlag == true).Distinct().ToArray();
                //data.master_shift = _DriversChartContext.MasterDriverShiftsDMO.Where(a => a.VMSMDSH_ActiveFlag == true).Distinct().ToArray();

                using (var cmd = _DriversChartContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "DRIVER_NAME";                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.master_employee = retObject.ToArray();



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _DriversChartContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "DriverChartGeneration";                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.griddata = retObject.ToArray();



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


        public DriverChartGenerationDTO savedata(DriverChartGenerationDTO data)
        {
            try
            {
                var duplicate = 0;
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.VMSDRCHT_Id == 0)
                {

                    duplicate = _DriversChartContext.DriverChartGenerationDMO.Where(e => e.TRMD_Id == data.TRMD_Id && e.VMSDRCHT_ActiveFlag == true && e.VMSMDSH_Id == data.VMSMDSH_Id && e.VMSDRCHT_FromDate==data.VMSDRCHT_FromDate && e.VMSDRCHT_ToDate==data.VMSDRCHT_ToDate).Count();
                    if (duplicate == 0)
                    {
                        DriverChartGenerationDMO obj = new DriverChartGenerationDMO();
                        obj.VMSMDSH_Id = data.VMSMDSH_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.TRMD_Id = data.TRMD_Id;
                        obj.VMSDRCHT_ActiveFlag = true;
                        obj.VMSDRCHT_FromDate = data.VMSDRCHT_FromDate;
                        obj.VMSDRCHT_ToDate = data.VMSDRCHT_ToDate;
                        obj.VMSDRCHT_SaturdayOffFlg = data.VMSDRCHT_SaturdayOffFlg;
                        obj.VMSDRCHT_SundayWorkingFlg = data.VMSDRCHT_SundayWorkingFlg;
                        obj.VMSDRCHT_Remarks = data.VMSDRCHT_Remarks;
                        obj.VMSDRCHT_CreatedDate = indianTime;
                        obj.VMSDRCHT_CreatedBy = data.UserId;
                        obj.VMSDRCHT_UpdatedDate = indianTime;
                        obj.VMSDRCHT_UpdatedBy = data.UserId;

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
                    duplicate = _DriversChartContext.DriverChartGenerationDMO.Where(e => e.TRMD_Id == data.TRMD_Id && e.VMSDRCHT_ActiveFlag == true && e.VMSMDSH_Id == data.VMSMDSH_Id && e.VMSDRCHT_FromDate == data.VMSDRCHT_FromDate && e.VMSDRCHT_ToDate == data.VMSDRCHT_ToDate).Count();
                    if (duplicate > 0)
                    {
                        var update = _DriversChartContext.DriverChartGenerationDMO.Single(e => e.VMSDRCHT_Id == data.VMSDRCHT_Id);

                        update.TRMD_Id = data.TRMD_Id;

                        update.VMSMDSH_Id = data.VMSMDSH_Id;
                        update.VMSDRCHT_FromDate = data.VMSDRCHT_ToDate;
                        update.VMSDRCHT_SaturdayOffFlg = data.VMSDRCHT_SaturdayOffFlg;
                        update.VMSDRCHT_SundayWorkingFlg = data.VMSDRCHT_SundayWorkingFlg;
                        update.VMSDRCHT_UpdatedDate = indianTime;
                        update.VMSDRCHT_UpdatedBy = data.UserId;
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
    }
}
