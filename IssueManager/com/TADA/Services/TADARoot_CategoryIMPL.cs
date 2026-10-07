using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
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
    public class TADARoot_CategoryIMPL : Interface.TADARoot_CategoryInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;
        public TADARoot_CategoryIMPL(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }


        //ISM_Mst_EmpRegion_MappingDMO
        //ISM_Mst_EmpCategory_MappingDMO
        //ISM_Mst_Route_CategoryDMO
        public TADARoot_CategoryDTO getalldata(TADARoot_CategoryDTO data)
        {
            try
            {



                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

               data.GetEmployeeList = _context.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToArray();

                data.categorynamelists = (from a in _context.ISM_Mst_Route_CategoryDMO
                                          select new TADARoot_CategoryDTO
                                          {

                                              Categoryname = a.IMRC_CategoryName,
                                              categoryId = a.IMRC_CategoryId,
                                              IMRC_ActiveFlag = a.IMRC_ActiveFlag

                                          }).Distinct().ToArray();

                data.statenamelists = (from a in _context.IVRM_Master_StateDMO
                                       
                                       select new TADARoot_CategoryDTO
                                       {
                                           Statename = a.IVRMMS_Name,
                                           StateId=a.IVRMMS_Id
                                       }).Distinct().ToArray();



                //data.GeatLoadCategory

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_CategoryMaping";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = "1"
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

                        data.GeatLoadCategory = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_CategoryMaping";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = "3"
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

                        data.GetEmployeeList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_CategoryMaping";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = "4"
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

                        data.GetstateList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_route_SourceName";
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                 

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

                        data.tADA_PrimaryAccountList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_Route_PrimaryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

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

                        data.Route_PrimaryDetails = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tada_Route_PrimaryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = "1"
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

                        data.primarytypeCount = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADARoot_CategoryDTO SetOrder(TADARoot_CategoryDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tada_Route_PrimaryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = "2"
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

                        data.primarytypelist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public TADARoot_CategoryDTO SaveCategory(TADARoot_CategoryDTO data)
        {
            try
            {


                var duplicate = _context.ISM_Mst_Route_CategoryDMO.Where(t => t.IMRC_CategoryName == data.Categoryname).ToList();

                if (duplicate.Count > 0)
                {
                    data.returnval = "Duplicate";
                }
                else
                {
                    ISM_Mst_Route_CategoryDMO route = new ISM_Mst_Route_CategoryDMO();
                    route.IMRC_CategoryName = data.Categoryname;
                    route.IMRC_CategoryId = data.categoryId;
                    route.MI_Id = data.MI_Id;
                    route.IMRC_ActiveFlag = true;
                    route.IMRC_Createdby = data.UserId;
                    route.IMRC_CreatedDate = DateTime.Now;
                    route.IMRC_Updateby = data.UserId;
                    route.IMRC_UpdateDate = DateTime.Now;
                    _context.Add(route);
                }

                int returnval = _context.SaveChanges();

                if (returnval > 0)
                {

                    data.returnval = "saved";
                }
                else
                {

                }

            }
            catch (Exception ee)
            {
                data.returnval = "admin";
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public TADARoot_CategoryDTO SaveRouteEmpCategory(TADARoot_CategoryDTO data)
        {
            try
            {

                foreach (var c in data.GetList)
                {

                    var duplicate = _context.ISM_Mst_EmpCategory_MappingDMO.Where(R => R.IMRC_CategoryId == data.categoryId && R.IMECM_EmpId == data.HRME_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        
                    }
                    else
                    {
                        if (c.IMECM_id > 0)
                        {
                            ISM_Mst_EmpCategory_MappingDMO cat = new ISM_Mst_EmpCategory_MappingDMO();
                            cat.IMECM_id = c.IMECM_id;
                            cat.IMECM_MinTarget = c.IMECM_MinTarget;
                            cat.IMECM_MaxTarget = c.IMECM_MaxTarget;
                            cat.IMECM_EmpId = c.HRME_Id;
                            cat.MI_Id = data.MI_Id;
                            cat.IMRC_CategoryId = c.categoryId;
                            cat.IMRC_Percentage = c.percentage;
                            cat.IMECM_Createdby = data.UserId;
                            cat.IMECM_CreatedDate = DateTime.Now;
                            cat.IMECM_Updateby = data.UserId;
                            cat.IMECM_UpdateDate = DateTime.Now;
                            cat.IMECM_ActiveFlag = true;
                            _context.Update(cat);
                        }
                        else
                        {
                            ISM_Mst_EmpCategory_MappingDMO cat = new ISM_Mst_EmpCategory_MappingDMO();
                            cat.IMECM_EmpId = data.HRME_Id;
                            cat.IMECM_MinTarget = c.IMECM_MinTarget;
                            cat.IMECM_MaxTarget = c.IMECM_MaxTarget;
                            cat.MI_Id = data.MI_Id;
                            cat.IMRC_CategoryId = c.categoryId;
                            cat.IMRC_Percentage = c.percentage;
                            cat.IMECM_Createdby = data.UserId;
                            cat.IMECM_CreatedDate = DateTime.Now;
                            cat.IMECM_Updateby = data.UserId;
                            cat.IMECM_UpdateDate = DateTime.Now;
                            cat.IMECM_ActiveFlag = true;
                            _context.Add(cat);
                        }
                       
                    }
                  
                }
                int returnval = _context.SaveChanges();

                if (returnval > 0)
                {
                 
                    data.returnval = "saved";
                }
                else
                {
                   
                    data.returnval = "Notsaved";

                }



            }
            catch (Exception ee)
            {
                data.returnval = "admin";
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public TADARoot_CategoryDTO saveRegionMapping(TADARoot_CategoryDTO data)
        {
            try
            {
                foreach(var r in data.regionList)
                {
                    var duplicate = _context.ISM_Mst_EmpRegion_MappingDMO.Where(t => t.IVRMMS_Id == r.IVRMMS_Id && t.IMER_EmpId == data.HRME_Id ).ToList();
                    if (duplicate.Count > 0)
                    {

                    }
                    else
                    {
                        ISM_Mst_EmpRegion_MappingDMO region = new ISM_Mst_EmpRegion_MappingDMO();
                        region.IMER_Id = data.IMER_Id;
                        region.MI_Id = data.MI_Id;
                        region.IMER_EmpId = data.HRME_Id;
                        region.IVRMMS_Id = r.IVRMMS_Id;
                        region.IMER_ActiveFlag = true;
                        region.IMER_Createdby = data.UserId;
                        region.IMER_ApplicableRegion = data.IMER_ApplicableRegion;
                        region.IMER_CreatedDate = DateTime.Now;
                        region.IMER_Updateby = data.UserId;
                        region.IMER_UpdateDate = DateTime.Now;
                        _context.Add(region);
                    }
                   
                }
            
                int returnvalue = _context.SaveChanges();
                if (returnvalue > 0)
                {
                    data.returnval = "saved";
                }
                else
                {
                    data.returnval = "Notsaved";
                }
            }
            catch(Exception ee)
            {
                data.returnval = "admin";
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public TADARoot_CategoryDTO deactiveY(TADARoot_CategoryDTO user)
        {

            try
            {
                var result = _context.ISM_Mst_EmpCategory_MappingDMO.Where(t => t.IMECM_id == user.IMECM_id).FirstOrDefault();

                if (result.IMECM_ActiveFlag == true)
                {
                    result.IMECM_ActiveFlag = false;
                }
                else if (result.IMECM_ActiveFlag == false)
                {
                    result.IMECM_ActiveFlag = true;
                }
                //result.IMECM_UpdateDate = DateTime.Now;
                _context.Update(result);
                int rowAffected = _context.SaveChanges();
                if (rowAffected > 0)
                {
                    user.returnval = "true";
                }
                else
                {
                    user.returnval = "false";
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return user;
        }

        public TADARoot_CategoryDTO ActivatePrimary(TADARoot_CategoryDTO user)
        {

            try
            {
                var result = _context.ISM_Route_PrimaryAccountDMO.Where(t => t.IRPA_ID == user.IRPA_ID).FirstOrDefault();

                if (result.IRPA_Status == true)
                {
                    result.IRPA_Status = false;
                }
                else if (result.IRPA_Status == false)
                {
                    result.IRPA_Status = true;
                }
                //result.IMECM_UpdateDate = DateTime.Now;
                _context.Update(result);
                int rowAffected = _context.SaveChanges();
                if (rowAffected > 0)
                {
                    user.returnval = "true";
                }
                else
                {
                    user.returnval = "false";
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return user;
        }

        public TADARoot_CategoryDTO deactiveCat(TADARoot_CategoryDTO user)
        {

            try
            {
                var result = _context.ISM_Mst_Route_CategoryDMO.Where(t =>  t.IMRC_CategoryId == user.categoryId).FirstOrDefault();

                if (result.IMRC_ActiveFlag == true)
                {
                    result.IMRC_ActiveFlag = false;
                }
                else if (result.IMRC_ActiveFlag == false)
                {
                    result.IMRC_ActiveFlag = true;
                }
                //result.IMECM_UpdateDate = DateTime.Now;
                _context.Update(result);
                int rowAffected = _context.SaveChanges();
                if (rowAffected > 0)
                {
                    user.returnval = "true";
                }
                else
                {
                    user.returnval = "false";
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return user;
        }

        public TADARoot_CategoryDTO deactiveRegion(TADARoot_CategoryDTO user)
        {

            try
            {
                var result = _context.ISM_Mst_EmpRegion_MappingDMO.Where(t => t.IMER_Id == user.IMER_Id).FirstOrDefault();

                if (result.IMER_ActiveFlag == true)
                {
                    result.IMER_ActiveFlag = false;
                }
                else if (result.IMER_ActiveFlag == false)
                {
                    result.IMER_ActiveFlag = true;
                }
                //result.IMECM_UpdateDate = DateTime.Now;
                _context.Update(result);
                int rowAffected = _context.SaveChanges();
                if (rowAffected > 0)
                {
                    user.returnval = "true";
                }
                else
                {
                    user.returnval = "false";
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return user;
        }

        public TADARoot_CategoryDTO viewrecordspopup(TADARoot_CategoryDTO data)
        {
            try
            {
               

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_CategoryMaping";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = "2"
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

                        data.Categorylist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_CategoryMaping";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = "5"
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

                        data.StateList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch
            {

            }
            return data;
        }

        public TADARoot_CategoryDTO Saveprimary(TADARoot_CategoryDTO data)
        {
            try
            {
                List<long> ismsmso_id = new List<long>();
                foreach (var item in data.arrayPrimaryAccount)
                {
                    ismsmso_id.Add(item.ISMSMSO_ID);
                }
                try
                {
                    int totalSaved = 0;

                    foreach (var ISMSMSO_ID in ismsmso_id)
                    {
                        var SavePrimaryList = _context.Database.ExecuteSqlCommand("Usp_s_Route_PrimaryAccount @p0,@p1,@p2,@p3", ISMSMSO_ID, true, data.UserId, data.HRME_Id);
                        totalSaved += SavePrimaryList;
                    }

                    if (totalSaved > 0)
                    {
                        data.Message = "success";
                    }
                    else
                    {
                        data.Message = "Failed";
                    }

                }

                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

            }
            catch
            {

            }
            return data;
        }

        public TADARoot_CategoryDTO validateordernumber(TADARoot_CategoryDTO dto)
        {
            bool returnresult = false;
            try
            {
                if (dto.Primaryaccount.Count() > 0)
                {
                    foreach (TADARoot_CategoryDTO mob in dto.Primaryaccount)
                    {
                        if (mob.IRPA_ID > 0)
                        {
                            var result = _context.ISM_Route_PrimaryAccountDMO.Single(t => t.IRPA_ID.Equals(mob.IRPA_ID));
                            result.IRPA_Order = mob.IRPA_Order;
                            result.IRPA_UpdateDate = DateTime.Now;
                            result.IRPA_Updateby = dto.UserId;
                            _context.Update(result);
                            _context.SaveChanges();


                        }
                    }
                    
                    returnresult = true;
                    dto.returnvalue = returnresult;

                }

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
