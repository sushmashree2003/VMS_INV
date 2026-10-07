using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IssueManager.com.TADA.Services
{
    public class TADACityDistanceMappingImpl : Interface.TADACityDistanceMappingInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;
        public TADACityDistanceMappingImpl(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }
        public TADACityDistanceMappingDTO getalldata(TADACityDistanceMappingDTO data)
        {
            try
            {
                data.companylist = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tada_State_Names";
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
                        data.StateList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                data.Travel_data = _context.VMS_TADA_Master_TravelDMO.Where(a => a.VTADAMT_ActiveFlag == true).ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_State_Details";
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
                        data.Travelstatedetails = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                data.Travel_Tier = _context.VMS_TADA_Master_TierDMO.Where(a => a.VRMMDT_ActiveFlag == true).ToArray();
                data.Statedatalist = _context.IVRM_Master_StateDMO.Where(a => a.IVRMMC_Id == 101).ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "City_wise_TierNames";
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
                        data.TADA_Traveltier = retObject.ToArray();

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
        public TADACityDistanceMappingDTO OnchangeCity(TADACityDistanceMappingDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tada_City_Names";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id",
                         SqlDbType.VarChar)
                    {
                        Value = data.IVRMMS_Id
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
                        data.CityList = retObject.ToArray();

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
        public TADACityDistanceMappingDTO savedata(TADACityDistanceMappingDTO data)
        {
            try
            {


                //if (data.TADACityDistanceDTO != null && data.TADACityDistanceDTO.Length > 0)
                //{
                //    foreach (var c in data.TADACityDistanceDTO)
                //    {

                //        var duplicate = _context.VMSTADAMasterCityDMO.Where(p => p.VTADAMC_City1 == c.VTADAMC_City1 && p.VTADAMC_City2 == c.VTADAMC_City2 && p.VTADAMC_State1 == c.VTADAMC_State1 && p.VTADAMC_State2 == c.VTADAMC_State2).ToList();
                //        if (duplicate.Count == 0)
                //        {
                //            {
                //                VMSTADAMasterCityDMO VMSTADAMasterCityDMO = new VMSTADAMasterCityDMO();
                //                VMSTADAMasterCityDMO.VTADAMC_Id = c.VTADAMC_Id;
                //                VMSTADAMasterCityDMO.VTADAMC_City1 = c.VTADAMC_City1;
                //                VMSTADAMasterCityDMO.VTADAMC_City2 = c.VTADAMC_City2;
                //                VMSTADAMasterCityDMO.VTADAMC_State1 = c.VTADAMC_State1;
                //                VMSTADAMasterCityDMO.VTADAMC_State2 = c.VTADAMC_State2;
                //                VMSTADAMasterCityDMO.VTADAMC_DistanceKM = c.VTADAMC_DistanceKM;
                //                VMSTADAMasterCityDMO.VTADAMC_CreatedBy = data.Userid;
                //                VMSTADAMasterCityDMO.VTADAMC_UpdatedBy = data.Userid;
                //                VMSTADAMasterCityDMO.VTADAMC_CreatedDate = DateTime.Now;
                //                VMSTADAMasterCityDMO.VTADAMC_UpdatedDate = DateTime.Now;
                //                _context.Add(VMSTADAMasterCityDMO);
                //                var i = _context.SaveChanges();
                //                if (i > 0)
                //                {
                //                    data.returnval = true;
                //                    data.Message = "Saved";
                //                }
                //                else
                //                {
                //                    data.returnval = false;
                //                    data.Message = "Failed";
                //                }
                //            }
                //        }
                //        else
                //        {
                //            data.Message = "Duplicate";

                //        }
                //    }


                //}

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public TADACityDistanceMappingDTO EditData(TADACityDistanceMappingDTO data)
        {
            try
            {
                var editDetails = _context.VMSTADAMasterCityDMO.Where(a => a.VTADAMC_Id == data.VTADAMC_Id).ToList();

                data.EditDetails = editDetails.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TADACityDistanceMappingDTO TravelDetails(TADACityDistanceMappingDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAMT_Id",
                         SqlDbType.VarChar)
                    {
                        Value = data.VTADAMT_Id
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
                        data.Travel_Details = retObject.ToArray();

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
        public TADACityDistanceMappingDTO GetDesignation(TADACityDistanceMappingDTO data)
        {
            try
            {

                data.Designation = _context.HR_Master_Designation.Where(t => t.HRMDES_ActiveFlag == true && t.MI_Id == data.MI_Id).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADACityDistanceMappingDTO Savedetails(TADACityDistanceMappingDTO data)
        {
            try
            {

                if (data.VTADAMC_Id == 0)
                {
                    for (int i = 0; i < data.Traveldatalist.Length; i++)
                    {
                        var temptravel = data.Traveldatalist[i].VTADAMD_Id;
                        for (int j = 0; j < data.Designationlist.Length; ++j)
                        {

                            var tempdata = data.Designationlist[j].HRMDES_Id;
                            {
                                //var duplicate = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id != 0 && t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.ISMMCLTPR_MOURefNo==data.ISMMCLTPR_MOURefNo && t.ISMMCLTPR_ProposalRefNo==data.ISMMCLTPR_ProposalRefNo && t.ISMMMD_Id==tempdata && t.ISMMCLTPR_NodalOfficerName==data.ISMMCLTPR_NodalOfficerName).ToArray();
                                var duplicate = _context.VMSTADAMasterCityDMO.Where(t => t.VTADAMC_Amount== data.Traveldatalist[i].amount && t.HRMDES_Id == tempdata && t.VTADAMT_Id == data.VTADAMT_Id && t.VTADAMD_Id == temptravel && t.VTADAMC_FromState == data.VTADAMC_FromState && t.VTADAMC_ToState == data.VTADAMC_ToState).ToArray();

                                if (duplicate.Count() > 0)
                                {
                                    data.duplicate = true;
                                }
                                else
                                {
                                    VMSTADAMasterCityDMO rrr = new VMSTADAMasterCityDMO();
                                    rrr.MI_Id = data.MI_Id;
                                    rrr.VRMMDT_Id = data.VRMMDT_Id;

                                    rrr.VTADAMC_FromState = data.VTADAMC_FromState;
                                    rrr.VTADAMC_ToState = data.VTADAMC_ToState;
                                    rrr.HRMDES_Id = tempdata;
                                    //  rrr.VTADAMC_Amount = data.amount;
                                    rrr.VTADAMC_Amount = data.Traveldatalist[i].amount;

                                    rrr.VTADAMT_Id = data.VTADAMT_Id;
                                    rrr.VTADAMD_Id = temptravel;
                                    rrr.VTADAMC_CreatedBy = data.UserId;
                                    rrr.VTADAMC_UpdatedBy = data.UserId;
                                    rrr.VTADAMC_CreatedDate = DateTime.Now;
                                    rrr.VTADAMC_UpdatedDate = DateTime.Now;
                                    _context.Add(rrr);
                                    int y = _context.SaveChanges();
                                    if (y > 0)
                                    {
                                        data.Message = "saved";
                                    }
                                    else
                                    {
                                        data.Message = "Failed";
                                    }
                                }
                            }
                        }


                    }
                }
                else if (data.VTADAMC_Id > 0)
                {
                    for (int j = 0; j < data.Traveldatalist.Length; ++j)
                    {
                        var temptravel = data.Traveldatalist[j].VTADAMD_Id;
                        for (int i = 0; i < data.Designationlist.Length; i++)
                        {
                            var tempdata = data.Designationlist[i].HRMDES_Id;
                            {
                                var duplicate = _context.VMSTADAMasterCityDMO.Where(t => t.VTADAMC_Amount == data.Traveldatalist[i].amount && t.VTADAMC_Id == data.VTADAMC_Id && t.HRMDES_Id == tempdata && t.VTADAMT_Id == data.VTADAMT_Id && t.VTADAMD_Id == temptravel && t.VTADAMC_FromState == data.VTADAMC_FromState && t.VTADAMC_ToState == data.VTADAMC_ToState).ToArray();
                                if (duplicate.Count() > 0)
                                {

                                    data.duplicate = true;
                                }
                                else
                                {
                                    var yy = _context.VMSTADAMasterCityDMO.Where(t => t.VTADAMC_Id == data.VTADAMC_Id).SingleOrDefault();
                                    yy.VTADAMC_FromState = data.VTADAMC_FromState;
                                    yy.VTADAMC_ToState = data.VTADAMC_ToState;
                                    yy.VRMMDT_Id = data.VRMMDT_Id;
                                    yy.HRMDES_Id = tempdata;
                                    yy.VTADAMT_Id = data.VTADAMT_Id;
                                    yy.VTADAMC_Amount = data.Traveldatalist[i].amount;
                                    yy.MI_Id = data.MI_Id;
                                    yy.VTADAMD_Id = temptravel;
                                    yy.VTADAMC_CreatedBy = data.UserId;
                                    yy.VTADAMC_UpdatedBy = data.UserId;
                                    yy.VTADAMC_CreatedDate = DateTime.Now;
                                    yy.VTADAMC_UpdatedDate = DateTime.Now;
                                    _context.Update(yy);
                                    int r = _context.SaveChanges();
                                    if (r > 0)
                                    {
                                        data.Message = "updated";
                                    }
                                    else
                                    {
                                        data.Message = "failed";
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADACityDistanceMappingDTO EditDetails(TADACityDistanceMappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_EDIT_Travel_State_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAMC_Id",
                         SqlDbType.VarChar)
                    {
                        Value = data.VTADAMC_Id
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
                        data.Tadaeditdetails = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tada_State_Names";
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
                        data.StateList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                data.Design_List = _context.HR_Master_Designation.Where(t => t.HRMDES_ActiveFlag == true && t.MI_Id == data.MI_Id).Distinct().ToArray();

                data.Travel_data = _context.VMS_TADA_Master_TravelDMO.Where(a => a.VTADAMT_ActiveFlag == true).ToArray();
               // data.Designation = _context.HR_Master_Designation.Where(t => t.HRMDES_ActiveFlag == true && t.MI_Id == data.MI_Id).Distinct().ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_State_Details";
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
                        data.Travelstatedetails = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAMT_Id",
                         SqlDbType.VarChar)
                    {
                        Value = data.VTADAMT_Id
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
                        data.Travel_Details = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_TierDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAMC_Id",
                         SqlDbType.VarChar)
                    {
                        Value = data.VTADAMC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
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
                        data.editdesign = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_TierDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAMC_Id",
                         SqlDbType.VarChar)
                    {
                        Value = data.VTADAMC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
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
                        data.Designation = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                // data.editdesign = _context.VMS_TADA_Master_Travel_DetailsDMO.Where(t => t.VTADAMD_ActiveFlag == true).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TADACityDistanceMappingDTO Getcities(TADACityDistanceMappingDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tada_City_Names";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IVRMMS_Id",
                         SqlDbType.VarChar)
                    {
                        Value = data.IVRMMS_Id
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
                        data.CityList = retObject.ToArray();

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
        public TADACityDistanceMappingDTO SaveTierdata(TADACityDistanceMappingDTO data)
        {
            try
            {
                if (data.TravelCity.Length > 0)
                {
                    for (int i = 0; i < data.TravelCity.Length; i++)
                    {
                        {
                            var obj = _context.IVRM_Master_CityDMO.Where(t => t.IVRMMCT_Id == data.TravelCity[i].IVRMMCT_Id).SingleOrDefault();
                            obj.VRMMDT_Id = data.VRMMDT_Id;
                            obj.IVRMMCT_Default = 1;
                            _context.Update(obj);
                            int y = _context.SaveChanges();
                            if (y > 0)
                            {
                                data.Message = "updated";
                            }
                            else
                            {
                                data.Message = "Failed";
                            }
                        }
                    }
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
