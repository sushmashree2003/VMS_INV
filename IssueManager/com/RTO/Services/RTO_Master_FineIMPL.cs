using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.RTO;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Services
{
    public class RTO_Master_FineIMPL : Interfaces.RTO_Master_FineInterface
    {
        public RTOContext _RTOContext;
        public DomainModelMsSqlServerContext _db;
        public RTO_Master_FineIMPL(RTOContext hh, DomainModelMsSqlServerContext db)
        {
            _RTOContext = hh;
            _db = db;
        }
        public RTO_Master_FineDTO loaddata(RTO_Master_FineDTO data)
        {
            try
            {
                data.RTOArray = _RTOContext.RTO_Master_Fine_ParameterDMO.Distinct().ToArray();
                data.vehicalArray = _RTOContext.Master_VehicleDMO.Where(R=>R.TRMV_ActiveFlag==true).Distinct().ToArray();
               
                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RTO_EMPLOYEE_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                    {
                        Value = "emplist"
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
                        data.emplist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                //EMPLIST
                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RTO_EMPLOYEE_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                    {
                        Value = "vehicallist"
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
                        data.vehicalList = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //RecieptReport
        public RTO_Master_FineDTO RecieptReport(RTO_Master_FineDTO data)
        {
            try
            {
                data.Vehicales = _RTOContext.VMS_Vehicle_KeysDMO.Where(R => R.TRMV_Id == data.TRMV_Id && R.VMSVKEYS_ActiveFlag == true).OrderByDescending(d => d.VMSVKEYS_Id).Take(1).Distinct().ToArray();
                //Vehicales
                //OrderByDescending(d => d.VBABRS_Id).Take(1).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VMS_Vehicle_KeyDTO savedata(VMS_Vehicle_KeyDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if(data.VMSVKEYS_Id > 0)
                {
                    var duplicate = _RTOContext.VMS_Vehicle_KeysDMO.Where(R => R.VMSVKEYS_RecivedDate == DateTime.Now && R.VMSVKEYS_RecivedTime == data.VMSVKEYS_RecivedTime && R.TRMV_Id == data.TRMV_Id && R.VMSVKEYS_Id != data.VMSVKEYS_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _RTOContext.VMS_Vehicle_KeysDMO.Where(R => R.VMSVKEYS_Id == data.VMSVKEYS_Id).FirstOrDefault();
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.TRMV_Id = data.TRMV_Id;
                       // obj.VMSVKEYS_DateTime = data.VMSVKEYS_DateTime;
                        obj.VMSVKEYS_ActiveFlag = true;                       
                        obj.VMSVKEYS_UpdatedDate = DateTime.Now;                       
                        obj.VMSVKEYS_UpdatedBy = data.UserId;
                        obj.VMSVKEYS_KeyReceivedHandedOverFlg = data.VMSVKEYS_KeyReceivedHandedOverFlg;
                        // obj.VMSVKEYS_Time = data.VMSVKEYS_Time;
                        obj.VMSVKEYS_RecivedTime = data.VMSVKEYS_RecivedTime;
                        obj.VMSVKEYS_RecivedDate = data.VMSVKEYS_RecivedDate;
                        obj.VMSVKEYS_Remarks = data.VMSVKEYS_Remarks;
                        _RTOContext.Update(obj);
                        int i = _RTOContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                  
                }
                else
                {
                    //VMS_Vehicle_KeysDMO
                    var duplicate = _RTOContext.VMS_Vehicle_KeysDMO.Where(R => R.VMSVKEYS_RecivedDate == DateTime.Now && R.VMSVKEYS_RecivedTime == data.VMSVKEYS_RecivedTime && R.TRMV_Id==data.TRMV_Id).ToList();
                    if(duplicate.Count >0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        VMS_Vehicle_KeysDMO obj = new VMS_Vehicle_KeysDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.TRMV_Id = data.TRMV_Id;
                        obj.VMSVKEYS_DateTime = null;
                        obj.VMSVKEYS_ActiveFlag = true;
                        obj.VMSVKEYS_CreatedDate = DateTime.Now;
                        obj.VMSVKEYS_UpdatedDate = DateTime.Now;
                        obj.VMSVKEYS_CreatedBy = data.UserId;
                        obj.VMSVKEYS_UpdatedBy = data.UserId;
                        obj.VMSVKEYS_KeyReceivedHandedOverFlg = data.VMSVKEYS_KeyReceivedHandedOverFlg;
                        obj.VMSVKEYS_RecivedTime = data.VMSVKEYS_RecivedTime;
                        obj.VMSVKEYS_RecivedDate = data.VMSVKEYS_RecivedDate;
                        obj.VMSVKEYS_Remarks = data.VMSVKEYS_Remarks;
            
                        _RTOContext.Add(obj);
                        int i = _RTOContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }

                }
              

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //onchange
        public VMS_Vehicle_KeyDTO onchange(VMS_Vehicle_KeyDTO data)
        {
            try
            {
                if (data.VMSVKEYS_Id > 0)
                {
                    var obj = _RTOContext.VMS_Vehicle_KeysDMO.Where(R => R.VMSVKEYS_Id == data.VMSVKEYS_Id).FirstOrDefault();
                    if(obj.VMSVKEYS_ActiveFlag==true)
                    {
                        obj.VMSVKEYS_ActiveFlag = false;
                    }
                    else
                    {
                        obj.VMSVKEYS_ActiveFlag = true;
                    }
                    obj.VMSVKEYS_UpdatedDate = DateTime.Now;
                    obj.VMSVKEYS_UpdatedBy = data.UserId;
                    _RTOContext.Update(obj);
                    int i = _RTOContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
                    }
                }
                else
                {
                    data.returnval = "admin";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public RTO_Master_FineDTO savedataSource(RTO_Master_FineDTO data)        {            try            {             
                if(data.RTOMFP_Id > 0)
                {
                    var duplicate = _RTOContext.RTO_Master_Fine_ParameterDMO.Where(R => R.RTOMFP_FineParameterName == data.RTOMFP_FineParameterName && R.RTOMFP_Id != data.RTOMFP_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _RTOContext.RTO_Master_Fine_ParameterDMO.Where(R => R.RTOMFP_Id == data.RTOMFP_Id).FirstOrDefault();
                        obj.RTOMFP_FineParameterName = data.RTOMFP_FineParameterName;
                        obj.RTOMFP_FineAmount = data.RTOMFP_FineAmount;
                        obj.RTOMFP_MaxViolationPermitted = data.RTOMFP_MaxViolationPermitted;
                        obj.RTOMFP_ParameterDescription = data.RTOMFP_ParameterDescription;
                        obj.RTOMFP_ActiveFlag = true;
                     
                        obj.RTOMFP_UpdatedDate = DateTime.Now;
                       
                        obj.RTOMFP_UpdatedBy = data.UserId;
                        _RTOContext.Update(obj);
                       
                        int i = _RTOContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _RTOContext.RTO_Master_Fine_ParameterDMO.Where(R => R.RTOMFP_FineParameterName == data.RTOMFP_FineParameterName).ToList();
                    if(duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        RTO_Master_Fine_ParameterDMO obj = new RTO_Master_Fine_ParameterDMO();
                        obj.RTOMFP_FineParameterName = data.RTOMFP_FineParameterName;
                        obj.RTOMFP_FineAmount = data.RTOMFP_FineAmount;
                        obj.RTOMFP_MaxViolationPermitted = data.RTOMFP_MaxViolationPermitted;
                        obj.RTOMFP_ParameterDescription = data.RTOMFP_ParameterDescription;
                        obj.RTOMFP_ActiveFlag = true;
                        obj.RTOMFP_CreatedDate = DateTime.Now;
                        obj.RTOMFP_UpdatedDate = DateTime.Now;
                        obj.RTOMFP_CreatedBy = data.UserId;
                        obj.RTOMFP_UpdatedBy = data.UserId;
                        _RTOContext.Add(obj);
                        int i = _RTOContext.SaveChanges();
                        if(i >0)
                        {
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }

                    }
                 

                }

            }            catch (Exception e)            {                Console.WriteLine(e.Message);                data.returnval = "admin";            }            return data;        }

        public RTO_Master_FineDTO clientDecative(RTO_Master_FineDTO data)        {            try            {                //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");                //DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);                if(data.RTOMFP_Id >0)
                {
                    var obj = _RTOContext.RTO_Master_Fine_ParameterDMO.Where(R => R.RTOMFP_Id == data.RTOMFP_Id).FirstOrDefault();
                    if(obj.RTOMFP_ActiveFlag==true)
                    {
                        obj.RTOMFP_ActiveFlag = false;
                    }
                    else
                    {
                        obj.RTOMFP_ActiveFlag = true;
                    }
                    obj.RTOMFP_UpdatedDate = DateTime.Now;

                    obj.RTOMFP_UpdatedBy = data.UserId;
                   
                    _RTOContext.Update(obj);
                    int i = _RTOContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
                    }
                }
                else
                {
                    data.returnval = "admin";
                }
            }            catch (Exception f)            {                Console.WriteLine(f.Message);                data.returnval = "admin";            }            return data;        }

        // VehicalUpdate
        public VMS_Vehicle_KeyDTO VehicalUpdate(VMS_Vehicle_KeyDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.VMSVKEYS_Id > 0)
                {
                    var duplicate = _RTOContext.VMS_Vehicle_KeysDMO.Where(R => R.VMSVKEYS_DateTime == DateTime.Now && R.VMSVKEYS_Time == data.VMSVKEYS_Time && R.TRMV_Id == data.TRMV_Id && R.VMSVKEYS_Id != data.VMSVKEYS_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _RTOContext.VMS_Vehicle_KeysDMO.Where(R => R.VMSVKEYS_Id == data.VMSVKEYS_Id).FirstOrDefault();
                                              
                        obj.VMSVKEYS_UpdatedDate = DateTime.Now;
                        obj.VMSVKEYS_UpdatedBy = data.UserId;
                        obj.VMSVKEYS_KeyHandedOverFlg = data.VMSVKEYS_KeyHandedOverFlg;                       
                        obj.VMSVKEYS_DateTime = data.VMSVKEYS_DateTime;
                        obj.VMSVKEYS_Time = data.VMSVKEYS_Time;
                        obj.VMSVKEYS_Remarks = data.VMSVKEYS_Remarks;
                        obj.VMSVKEYS_OdometerReading = data.VMSVKEYS_OdometerReading;
                        _RTOContext.Update(obj);
                        int i = _RTOContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }

                }
             


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
    }
}
