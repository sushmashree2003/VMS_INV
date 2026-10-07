using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class ISM_Sales_BOMImpl : Interfaces.ISM_Sales_BOMInterface
    {

        public VMSContext _vmsconte;
        public HRMSContext _hrmscon;
        private DomainModelMsSqlServerContext _db;
        public ISM_Sales_BOMImpl(VMSContext vmsContext, HRMSContext hrms, DomainModelMsSqlServerContext db)
        {
            _vmsconte = vmsContext;
            _hrmscon = hrms;
            _db = db;
        }


        public ISM_Sales_BOMDTO getloaddata(ISM_Sales_BOMDTO dto)
        {

            try
            {
                dto.Productlist = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMPR_ActiveFlag == true).ToArray();

                dto.ServiceList = _vmsconte.ISM_Sales_Product_ServicesDMO.Where(a => a.MI_ID == dto.MI_Id).ToArray();

                dto.BOMlist = (from a in _vmsconte.ISM_Sales_BOMDMO
                               from b in _vmsconte.ISM_Sales_Master_Product_DMO_con

                               where (a.ISMSMPR_Id == b.ISMSMPR_Id && b.MI_Id == dto.MI_Id)
                               select new ISM_Sales_BOMDTO
                               {
                                   ISMSMPR_Id = b.ISMSMPR_Id,
                                   ISMSMPR_ProductName = b.ISMSMPR_ProductName,
                                   ISMSBOM_Id = a.ISMSBOM_Id,
                                   ISMSBOM_BOMName = a.ISMSBOM_BOMName,
                                   ISMSBOM_BOMDescription = a.ISMSBOM_BOMDescription

                               }).Distinct().OrderByDescending(m => m.ISMSBOM_Id).ToArray();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())

                {
                    cmd.CommandText = "Ism_Sales_Product_ServicesItems";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                        dto.Serviceitemlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                // dto.Serviceitemlist = _vmsconte.Ism_Sales_Product_Services_ItemsDMO.Where(a => a.MI_ID == dto.MI_Id && a.ISMSPSI_ActiveFlag == true).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public ISM_Sales_BOMDTO savedetails(ISM_Sales_BOMDTO dto)
        {
            try
            {

                if (dto.ISMSPS_ID != 0)
                {
                    var res = _vmsconte.ISM_Sales_Product_ServicesDMO.Where(t => t.ISMSPS_Service_Name == dto.ISMSPS_Service_Name && t.MI_ID == dto.MI_Id && t.ISMSPS_ID != dto.ISMSPS_ID).ToList();

                    if (res.Count > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        var result = _vmsconte.ISM_Sales_Product_ServicesDMO.Single(t => t.ISMSPS_ID == dto.ISMSPS_ID);
                        result.ISMSPS_Service_Name = dto.ISMSPS_Service_Name;
                        result.ISMSPS_Service_Description = dto.ISMSPS_Service_Description;
                        result.AbvStrength = dto.AbvStrength;
                        result.ISMSPS_UpdatedDate = DateTime.Now;
                        result.ISMSPS_UpdatedBy = dto.User_Id;
                        _vmsconte.Update(result);
                        var contactExists = _vmsconte.SaveChanges();
                        if (contactExists > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "Not Update";
                        }

                    }
                }
                else
                {
                    var res = _vmsconte.ISM_Sales_Product_ServicesDMO.Where(t => t.ISMSPS_Service_Name == dto.ISMSPS_Service_Name && t.MI_ID == dto.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        ISM_Sales_Product_ServicesDMO data = new ISM_Sales_Product_ServicesDMO();
                        data.MI_ID = dto.MI_Id;
                        data.ISMSPS_Service_Name = dto.ISMSPS_Service_Name;
                        data.ISMSPS_Service_Description = dto.ISMSPS_Service_Description;
                        data.AbvStrength = dto.AbvStrength;
                        data.ISMSPS_ActiveFlag = true;
                        data.ISMSPS_CreatedDate = DateTime.Now;
                        data.ISMSPS_UpdatedDate = DateTime.Now;
                        data.ISMSPS_UpdatedBy = dto.User_Id;
                        _vmsconte.Add(data);

                        var contactExists = _vmsconte.SaveChanges();
                        if (contactExists > 0)
                        {
                            dto.retrunMsg = "Saved";
                        }
                        else
                        {
                            dto.retrunMsg = "Not Saved";
                        }

                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


        public ISM_Sales_BOMDTO deactive(ISM_Sales_BOMDTO data)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Product_ServicesDMO.Single(t => t.ISMSPS_ID == data.ISMSPS_ID);

                if (result.ISMSPS_ActiveFlag == true)
                {
                    result.ISMSPS_ActiveFlag = false;
                }
                else if (result.ISMSPS_ActiveFlag == false)
                {
                    result.ISMSPS_ActiveFlag = true;
                }
                result.ISMSPS_UpdatedDate = DateTime.Now;
                _vmsconte.Update(result);
                int returnval = _vmsconte.SaveChanges();
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


        public ISM_Sales_BOMDTO saveiteamsdetails(ISM_Sales_BOMDTO dto)
        {
            try
            {

                if (dto.ISMSPS_ID != 0)
                {

                    if (dto.Iteamsavedfiles != null && dto.Iteamsavedfiles.Length > 0)
                    {

                        if (dto.Iteamsavedfiles != null && dto.Iteamsavedfiles.Length > 0)
                        {
                            if (dto.returnval == true)
                            {
                                var res = _vmsconte.Ism_Sales_Product_Services_ItemsDMO.Where(t => t.ISMSPS_ID == dto.ISMSPS_ID && t.MI_ID == dto.MI_Id).ToList();

                                if (res.Count > 0)
                                {
                                    foreach (var d in res)
                                    {
                                        _vmsconte.Remove(d);
                                    }
                                }

                                foreach (var d in dto.Iteamsavedfiles)
                                {
                                    Ism_Sales_Product_Services_ItemsDMO data = new Ism_Sales_Product_Services_ItemsDMO();
                                    data.MI_ID = dto.MI_Id;
                                    data.ISMSPS_ID = dto.ISMSPS_ID;
                                    data.ISMSPSI_ItemsName = d.ISMSPSI_ItemsName;
                                    data.ISMSPSI_ItemsDescription = d.ISMSPSI_ItemsDescription;
                                    data.ISMSPSI_Quantity = d.ISMSPSI_Quantity;
                                    data.ISMSPSI_Cost = d.ISMSPSI_Cost;
                                    data.ISMSPSI_AMC_Cost = d.ISMSPSI_AMC_Cost;
                                    data.ISMSPSI_AMC_Flag = d.ISMSPSI_AMC_Flag;
                                    data.ISMSPSI_ActiveFlag = true;
                                    data.ISMSPSI_CreatedDate = DateTime.Now;
                                    data.ISMSPSI_UpdatedDate = DateTime.Now;
                                    data.ISMSPSI_CreatedBy = dto.User_Id;
                                    data.ISMSPSI_ItemFlag = d.ISMSPSI_ItemFlag;
                                    _vmsconte.Add(data);
                                }

                            }
                            else
                            {
                                foreach (var d in dto.Iteamsavedfiles)
                                {
                                    var res = _vmsconte.Ism_Sales_Product_Services_ItemsDMO.Where(t => t.ISMSPSI_ItemsName == d.ISMSPSI_ItemsName && t.ISMSPS_ID == dto.ISMSPS_ID && t.MI_ID == dto.MI_Id).ToList();

                                    if (res.Count > 0)
                                    {
                                        dto.retrunMsg = "Duplicate";
                                    }
                                    else
                                    {
                                        Ism_Sales_Product_Services_ItemsDMO data = new Ism_Sales_Product_Services_ItemsDMO();
                                        data.MI_ID = dto.MI_Id;
                                        data.ISMSPS_ID = dto.ISMSPS_ID;
                                        data.ISMSPSI_ItemsName = d.ISMSPSI_ItemsName;
                                        data.ISMSPSI_ItemsDescription = d.ISMSPSI_ItemsDescription;
                                        data.ISMSPSI_Quantity = d.ISMSPSI_Quantity;
                                        data.ISMSPSI_Cost = d.ISMSPSI_Cost;
                                        data.ISMSPSI_AMC_Cost = d.ISMSPSI_AMC_Cost;
                                        data.ISMSPSI_AMC_Flag = d.ISMSPSI_AMC_Flag;
                                        data.ISMSPSI_ActiveFlag = true;
                                        data.ISMSPSI_CreatedDate = DateTime.Now;
                                        data.ISMSPSI_UpdatedDate = DateTime.Now;
                                        data.ISMSPSI_CreatedBy = dto.User_Id;
                                        data.ISMSPSI_ItemFlag = d.ISMSPSI_ItemFlag;
                                        _vmsconte.Add(data);
                                    }
                                }


                            }



                        }
                        var contactExists = _vmsconte.SaveChanges();
                        if (contactExists > 0)
                        {
                            dto.retrunMsg = "Saved";
                        }
                        else
                        {
                            dto.retrunMsg = "Not Saved";
                        }
                    }



                }


            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


        public ISM_Sales_BOMDTO saveBOMdetails(ISM_Sales_BOMDTO dto)
        {
            try
            {
                if (dto.ISMSBOM_Id != 0)
                {
                    
                    var update  = _vmsconte.ISM_Sales_BOMDMO.Where(t => t.ISMSBOM_BOMName != dto.ISMSBOM_BOMName && t.ISMSBOM_Id == dto.ISMSBOM_Id).ToList();

                    if (update.Count > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        var newupdate = _vmsconte.ISM_Sales_BOMDMO.Single(t => t.ISMSBOM_Id == dto.ISMSBOM_Id);
                        newupdate.ISMSMPR_Id = dto.ISMSMPR_Id;
                        newupdate.ISMSBOM_BOMName = dto.ISMSBOM_BOMName;
                        newupdate.ISMSBOM_BOMDescription = dto.ISMSBOM_BOMDescription;
                        _vmsconte.Update(newupdate);
                        var removecount = _vmsconte.ISM_Sales_BOMCONFIGDMO.Where(R => R.ISMSBOM_Id == dto.ISMSBOM_Id).ToList();

                        if(removecount .Count > 0)
                        {
                            foreach (var d in removecount)
                            {
                                _vmsconte.Remove(d);
                            }
                        }
                        foreach (var d in dto.serviceListtwo)
                        {
                           
                            ISM_Sales_BOMCONFIGDMO dataa = new ISM_Sales_BOMCONFIGDMO();
                            dataa.ISMSPS_ID = d.ISMSPS_ID;
                            dataa.ISMSBOM_Id = dto.ISMSBOM_Id;
                            dataa.ISMSBC_ActiveFlag = true;
                            dataa.ISMSBOM_CreatedDate = DateTime.Now;
                            dataa.ISMSBC_UpdatedDate = DateTime.Now;
                            dataa.ISMSBC_CreatedBy = dto.User_Id;
                            _vmsconte.Add(dataa);

                            var contactExists = _vmsconte.SaveChanges();
                            if (contactExists > 0)
                            {
                                dto.retrunMsg = "Saved";
                            }
                            else
                            {
                                dto.retrunMsg = "Not Saved";
                            }

                        }

                    }



                }
                else
                {
                    var res = _vmsconte.ISM_Sales_BOMDMO.Where(t => t.ISMSBOM_BOMName == dto.ISMSBOM_BOMName && t.ISMSMPR_Id == dto.ISMSMPR_Id).ToList();

                    if (res.Count > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        ISM_Sales_BOMDMO data = new ISM_Sales_BOMDMO();
                        data.ISMSMPR_Id = dto.ISMSMPR_Id;
                        data.ISMSBOM_BOMName = dto.ISMSBOM_BOMName;
                        data.ISMSBOM_BOMFlg = dto.ISMSBOM_BOMName;
                        data.ISMSBOM_BOMDescription = dto.ISMSBOM_BOMDescription;
                        data.ISMSBOM_ActiveFlag = true;
                        data.ISMSBOM_CreatedDate = DateTime.Now;
                        data.ISMSBOM_UpdatedDate = DateTime.Now;
                        data.ISMSBOM_CreatedBy = dto.User_Id;
                        _vmsconte.Add(data);
                        _vmsconte.SaveChanges();
                        foreach (var d in dto.serviceListtwo)
                        {
                            ISM_Sales_BOMCONFIGDMO dataa = new ISM_Sales_BOMCONFIGDMO();
                            dataa.ISMSPS_ID = d.ISMSPS_ID;
                            dataa.ISMSBOM_Id = data.ISMSBOM_Id;
                            dataa.ISMSBC_ActiveFlag = true;
                            dataa.ISMSBOM_CreatedDate = DateTime.Now;
                            dataa.ISMSBC_UpdatedDate = DateTime.Now;
                            dataa.ISMSBC_CreatedBy = dto.User_Id;
                            _vmsconte.Add(dataa);

                            var contactExists = _vmsconte.SaveChanges();
                            if (contactExists > 0)
                            {
                                dto.retrunMsg = "Saved";
                            }
                            else
                            {
                                dto.retrunMsg = "Not Saved";
                            }

                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }




        public ISM_Sales_BOMDTO viewservicedetails(ISM_Sales_BOMDTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())

                {
                    cmd.CommandText = "BOM_viewservicedetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSBOM_Id
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
                        dto.viewservicedetailslist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }



        public ISM_Sales_BOMDTO Sales_BOM_edit(ISM_Sales_BOMDTO dto)
        {
            try
            {

                dto.Sales_BOM_edits = (from a in _vmsconte.ISM_Sales_Product_ServicesDMO
                                       from b in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                       from c in _vmsconte.ISM_Sales_BOMDMO
                                       from d in _vmsconte.ISM_Sales_BOMCONFIGDMO
                                       where a.ISMSPS_ID == d.ISMSPS_ID && b.ISMSMPR_Id == c.ISMSMPR_Id && c.ISMSBOM_Id == d.ISMSBOM_Id && c.ISMSBOM_Id == dto.ISMSBOM_Id
                                       select new ISM_Sales_BOMDTO
                                       {
                                           ISMSPS_ID=a.ISMSPS_ID,
                                           ISMSPS_Service_Name=a.ISMSPS_Service_Name,
                                           ISMSBOM_Id=c.ISMSBOM_Id,
                                           ISMSBOM_BOMName=c.ISMSBOM_BOMName,
                                           ISMSBOM_BOMDescription=c.ISMSBOM_BOMDescription,
                                           ISMSMPR_Id =b.ISMSMPR_Id,
                                           ISMSMPR_ProductName=b.ISMSMPR_ProductName

                                       }).ToList().ToArray();





            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }




    }
}
