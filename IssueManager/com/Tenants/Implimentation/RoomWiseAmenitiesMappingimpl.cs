using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Training;
using IssueManager.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{


    public class RoomWiseAmenitiesMappingimpl : Interfaces.RoomWiseAmenitiesMappingInterface
    {
        public IssueManagerContext _context;

      

        public RoomWiseAmenitiesMappingimpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }



        public RoomWiseAmenitiesMappingDTO getloaddata(RoomWiseAmenitiesMappingDTO data)
        {
            try

            {
                //   -- from b in _context.HR_Master_Amenities_DMO
                data.rooms = (from a in _context.HR_Master_Building
                              from b in _context.HR_Master_Room
                              where (a.HRMB_Id == b.HRMB_Id && a.MI_Id == data.MI_Id)
                              select new RoomWiseAmenitiesMappingDTO
                              {
                                  HRMR_Id = b.HRMR_Id,
                                  HRMR_RoomName = a.HRMB_BuildingName + "  -  " + b.HRMR_RoomName

                              }).Distinct().ToArray();
                data.get_org = _context.HR_Master_Amenities_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();

                //data.get_levl= (from a in _context.HR_Master_Room

                //                from c in _context.HR_Master_Amenities_DMO
                //                from d in _context.HR_Master_Room_AmenitiesDMO
                //                where (d.HRMR_Id==a.HRMR_Id && d.HRMRAM_Id==c.HRMAM_Id && a.MI_Id==data.MI_Id && c.MI_Id==data.MI_Id && a.MI_Id==c.MI_Id)
                //                select new  RoomWiseAmenitiesMappingDTO
                //                {
                //                    HRMR_RoomName = a.HRMR_RoomName,
                //                    HRMAM_AmenitiesName = c.HRMAM_AmenitiesName,
                //                    HRMRAM_RentPerDay=d.HRMRAM_RentPerDay,
                //                    HRMRAM_NoOfHrs=d.HRMRAM_NoOfHrs,
                //                    HRMRAM_RentPerHour=d.HRMRAM_RentPerHour,
                //                    HRMRAM_Id=d.HRMRAM_Id,
                //                    HRMAM_ActiveFlag=d.HRMRAM_ActiveFlag,

                //                }
                //                  ).Distinct().ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {
                    
                    cmd.CommandText = "Master_Room_AmenitiesMapping";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",                                SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });
                    
                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.get_levl = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //dto.returnval = "admin";
            }
            return data;

        }




        public RoomWiseAmenitiesMappingDTO savedetails(RoomWiseAmenitiesMappingDTO data)
        {
            try
            {
                string amenities_arr = "0";
                if (data.HRMRAM_Id == 0)
                //if(data.HRMRAM_Id!=0)
                {

                    for (int i = 0; i < data.HRMRAM_Array.Length; i++)
                    {
                        HR_Master_Room_AmenitiesDMO obj = new HR_Master_Room_AmenitiesDMO();


                        obj.HRMRAM_Id = data.HRMRAM_Id;
                        obj.HRMR_Id = data.HRMR_Id;
                        obj.HRMAM_Id = data.HRMRAM_Array[i];
                        obj.HRMRAM_RentPerDay = data.HRMRAM_RentPerDay;
                        obj.HRMRAM_NoOfHrs = data.HRMRAM_NoOfHrs;
                        obj.HRMRAM_RentPerHour = data.HRMRAM_RentPerHour;
                        obj.HRMRAM_ActiveFlag = true;
                        obj.HRMRAM_CreatedDate = DateTime.Now;
                        obj.HRMRAM_UpdatedDate = DateTime.Now;
                        obj.HRMRAM_CreatedBy = data.UserId;
                        obj.HRMRAM_UpdatedBy = data.UserId;
                        obj.HRMRAM_filepath = data.HRMRAM_filepath;
                        _context.Add(obj);
                    }
                    int rowAffected = _context.SaveChanges();
                    if (rowAffected > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }

                else if (data.HRMRAM_Id > 0)
                {

                    for (int i = 0; i < data.HRMRAM_Array.Length; i++)
                    {

                        HR_Master_Room_AmenitiesDMO obj = new HR_Master_Room_AmenitiesDMO();

                        obj.HRMRAM_Id = data.HRMRAM_Id;
                        obj.HRMR_Id = data.HRMR_Id;
                        obj.HRMAM_Id = data.HRMRAM_Array[i];
                        obj.HRMRAM_RentPerDay = data.HRMRAM_RentPerDay;
                        obj.HRMRAM_NoOfHrs = data.HRMRAM_NoOfHrs;
                        obj.HRMRAM_RentPerHour = data.HRMRAM_RentPerHour;
                        obj.HRMRAM_ActiveFlag = true;
                        obj.HRMRAM_CreatedDate = DateTime.Now;
                        obj.HRMRAM_UpdatedDate = DateTime.Now;
                        obj.HRMRAM_CreatedBy = data.UserId;
                        obj.HRMRAM_UpdatedBy = data.UserId;
                        obj.HRMRAM_filepath = data.HRMRAM_filepath;

                        _context.Update(obj);
                    }

                    int rowAffected = _context.SaveChanges();
                    if (rowAffected > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

       
        public RoomWiseAmenitiesMappingDTO Edit(RoomWiseAmenitiesMappingDTO data)
        {
            try
            {
                data.getloaddetails = (from a in _context.HR_Master_Amenities_DMO
                                       from b in _context.HR_Master_Room_AmenitiesDMO
                                       from c in _context.HR_Master_Room
                                       from d in _context.HR_Master_Building
                                       
                                       where (a.HRMAM_Id == b.HRMAM_Id && a.MI_Id == data.MI_Id && b.HRMRAM_Id == data.HRMRAM_Id && b.HRMR_Id == c.HRMR_Id && c.HRMB_Id == d.HRMB_Id)
                                       select new RoomWiseAmenitiesMappingDTO
                                       {
                                           HRMAM_AmenitiesName = a.HRMAM_AmenitiesName,
                                           HRMR_Id = b.HRMR_Id,
                                           HRMR_RoomName = d.HRMB_BuildingName + "  -  " + c.HRMR_RoomName,
                                           HRMRAM_RentPerDay = b.HRMRAM_RentPerDay,
                                           HRMRAM_NoOfHrs = b.HRMRAM_NoOfHrs,
                                           HRMRAM_RentPerHour = b.HRMRAM_RentPerHour,
                                           HRMRAM_Id = b.HRMRAM_Id,
                                           HRMAM_Id = b.HRMAM_Id,
                                           HRMRAM_filepath = b.HRMRAM_filepath
                                       }
                                       ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public RoomWiseAmenitiesMappingDTO deactive(RoomWiseAmenitiesMappingDTO data)
        {
            try
            {
                var result = _context.HR_Master_Room_AmenitiesDMO.Single(t => t.HRMRAM_Id == data.HRMRAM_Id);

                if (result.HRMRAM_ActiveFlag == true)
                {
                    result.HRMRAM_ActiveFlag = false;
                }
                else if (result.HRMRAM_ActiveFlag == false)
                {
                    result.HRMRAM_ActiveFlag = true;
                }
                result.HRMRAM_UpdatedDate = DateTime.Now;
                _context.Update(result);
                int returnval = _context.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
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
    

