using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager.HRMasterArea;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.QRcode_Activity.Services
{
    public class HRMasterAreaStaffMappingImpl : Interfaces.HRMasterAreaStaffMappingInterface
    {

        public HRMasterAreaContext _INVContext;
        ILogger<HRMasterAreaStaffMappingImpl> _logInv;
        private object stafflogin;
        private object t;

        public HRMasterAreaStaffMappingImpl(HRMasterAreaContext InvContext, ILogger<HRMasterAreaStaffMappingImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public HRMasterAreaStaffMappingDTO getloaddata(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
               

                data.get_building = _INVContext.HR_Master_Building_con.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.get_Area = (from a in _INVContext.HRMasterAreaDMO
                                 from b in _INVContext.HR_Master_Building_con
                                 from c in _INVContext.HR_Master_Floor_con
                                 where a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.MI_Id == data.MI_Id
                                 select new HRMasterAreaStaffMappingDTO
                                 {
                                     HRMAR_AreaName = a.HRMAR_AreaName,
                                     HRMAR_AreaCode = a.HRMAR_AreaCode,
                                     HRMAR_QRCode = a.HRMAR_QRCode,
                                     HRMAR_TypeFlag = a.HRMAR_TypeFlag,
                                     HRMB_BuildingName = b.HRMB_BuildingName,
                                     HRMF_FloorName = c.HRMF_FloorName,


                                 }).ToArray();

                data.get_AreaActivity = (from a in _INVContext.HRMasterAreaActivityDMO
                                         from b in _INVContext.HRMasterAreaDMO
                                         from c in _INVContext.HR_Master_Building_con
                                         from d in _INVContext.HR_Master_Floor_con

                                         where c.HRMB_Id == d.HRMB_Id && d.HRMF_Id == b.HRMF_Id && a.HRMAR_Id == b.HRMAR_Id && a.HRMARACT_ActiveFlag == true
                                         select new HRMasterAreaStaffMappingDTO
                                         {
                                             HRMB_BuildingName = c.HRMB_BuildingName,
                                             HRMF_FloorName = d.HRMF_FloorName,
                                             HRMAR_AreaName = b.HRMAR_AreaName,
                                             HRMARACT_ActivityName = a.HRMARACT_ActivityName,
                                             HRMARACT_ActivityDesc = a.HRMARACT_ActivityDesc,
                                             HRMARACT_ActiveFlag = a.HRMARACT_ActiveFlag,
                                             
                                         }).ToArray();
                

                                      List<MasterRoleType> allroles = new List<MasterRoleType>();
                                      allroles = _INVContext.masterRoleType.Where(t =>t.flag == "U").ToList();
                                      data.fillroletype = allroles.ToArray();

                                      List<Institution> allinstitution = new List<Institution>();
                                      allinstitution = _INVContext.Institution.Where(r=>r.MI_ActiveFlag == 1).ToList();
                                      data.fillinstitution = allinstitution.ToArray();

  





                data.staffmappinglist = (from a in _INVContext.HRMasterAreaDMO
                                         from b in _INVContext.HR_Master_Building_con
                                         from c in _INVContext.HR_Master_Floor_con
                                         from d in _INVContext.HRMasterAreaActivityDMO
                                         from e in _INVContext.HRMasterAreaStaffMappingDMO
                                         from f in _INVContext.ApplicationUser 
                                         from g in _INVContext.HRMasterAreaActivityTimeDMO
                                             //from g in _INVContext.UserRoleWithInstituteDMO

                                         where b.HRMB_Id == a.HRMB_Id && c.HRMF_Id == a.HRMF_Id && d.HRMAR_Id == a.HRMAR_Id && e.HRMARACT_Id == d.HRMARACT_Id && f.Id==e.User_Id && g.HRMARACT_Id ==e.HRMARACT_Id
                                         select new HRMasterAreaStaffMappingDTO
                                         {
                                             HRMB_BuildingName = b.HRMB_BuildingName,
                                             HRMF_FloorName = c.HRMF_FloorName,
                                             HRMAR_AreaName = a.HRMAR_AreaName,
                                             HRMARACT_ActivityName = d.HRMARACT_ActivityName,
                                             HRMARACTSTMAP_Date = e.HRMARACTSTMAP_Date,
                                             User_Id = e.User_Id,
                                             HRMARACTSTMAP_ActiveFlag = e.HRMARACTSTMAP_ActiveFlag,
                                             HRMARACTSTMAP_Id = e.HRMARACTSTMAP_Id,
                                             UserName = f.UserName,
                                             HRMARACTT_Time = g.HRMARACTT_Time
                                             //id = e.User_Id,

                                         }).OrderByDescending(t => t.HRMARACTSTMAP_Date).Take(1000)  .ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }

        public HRMasterAreaStaffMappingDTO getfloors(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
                data.get_floor = _INVContext.HR_Master_Floor_con.Where(a => a.HRMF_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMasterAreaStaffMappingDTO getAreaName(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
                data.get_getAreaName = _INVContext.HRMasterAreaDMO.Where(a => a.HRMAR_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id && a.HRMF_Id == data.HRMF_Id).Distinct().ToArray();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMasterAreaStaffMappingDTO getActivityName(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
                data.get_getActivity = _INVContext.HRMasterAreaActivityDMO.Where(a => a.HRMARACT_ActiveFlag == true && a.HRMAR_Id == data.HRMAR_Id).Distinct().ToArray();
                

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMasterAreaStaffMappingDTO getEmployeeName(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
               

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_empnamedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = data.IVRMRT_Id });


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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.emp_deatils = retObject.ToArray();
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


        public HRMasterAreaStaffMappingDTO AutoStaffMapping(HRMasterAreaStaffMappingDTO data)
        {
            try
            {


                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRCODE_StaffActivity_Automapping";
                    cmd.CommandType = CommandType.StoredProcedure;

                  //  cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = data.IVRMRT_Id });


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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.AutoStaffMapping_deatils = retObject.ToArray();
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

        public HRMasterAreaStaffMappingDTO getUsername(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
                data.get_getusernames = (from a in _INVContext.UserRoleWithInstituteDMO
                                              from b in _INVContext.ApplicationUser
                                              from c in _INVContext.ApplicationUserRole
                                              where  a.Id==b.Id && a.Id==c.UserId && a.MI_Id == data.MI_Id && c.RoleTypeId==data.IVRMRT_Id
                                              select new HRMasterAreaStaffMappingDTO
                                              {
                                              UserName = b.UserName,
                                              id = a.Id
                                              }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMasterAreaStaffMappingDTO savedetailsStaffMapping(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
                foreach (var fl in data.ActivitynameArray)
                {


                    if (data.HRMARACTSTMAP_Id != 0)
                    {
                        var res = _INVContext.HRMasterAreaStaffMappingDMO.Where(t => t.HRMARACTSTMAP_Date == data.HRMARACTSTMAP_Date && t.HRMARACTSTMAP_Id != data.HRMARACTSTMAP_Id && t.HRMARACT_Id == fl.HRMARACT_Id).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _INVContext.HRMasterAreaStaffMappingDMO.Single(t => t.User_Id == data.id && t.HRMARACT_Id == fl.HRMARACT_Id);
                            result.HRMARACT_Id = fl.HRMARACT_Id;
                            result.User_Id = data.id;
                            result.HRMARACTSTMAP_Date = data.HRMARACTSTMAP_Date;
                            result.HRMARACTSTMAP_ActiveFlag = true;

                            result.HRMARACTSTMAP_UpdatedDate = DateTime.Now;
                            _INVContext.Update(result);

                            var contactExists = _INVContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnduplicatestatus = "Update";
                            }
                            else
                            {
                                data.returnduplicatestatus = "Not Update";
                            }
                        }
                    }
                    else
                    {
                        var res = _INVContext.HRMasterAreaStaffMappingDMO.Where(t => (t.User_Id == data.id && t.HRMARACTSTMAP_Date == data.HRMARACTSTMAP_Date && t.HRMARACT_Id == fl.HRMARACT_Id)).ToList();

                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            HRMasterAreaStaffMappingDMO area = new HRMasterAreaStaffMappingDMO();
                            area.HRMARACT_Id = fl.HRMARACT_Id;

                            area.HRMARACTSTMAP_Date = data.HRMARACTSTMAP_Date;
                            area.HRMARACTSTMAP_ActiveFlag = true;
                            area.User_Id = data.id;
                            area.HRMARACTSTMAP_CreatedDate = DateTime.Now;
                            area.HRMARACTSTMAP_UpdatedDate = DateTime.Now;
                            _INVContext.Add(area);

                            var contactExists = _INVContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnduplicatestatus = "Saved";
                            }
                            else
                            {
                                data.returnduplicatestatus = "Not Saved";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMasterAreaStaffMappingDTO deactive(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
                var result = _INVContext.HRMasterAreaStaffMappingDMO.Single(t => t.HRMARACTSTMAP_Id == data.HRMARACTSTMAP_Id);

                if (result.HRMARACTSTMAP_ActiveFlag == true)
                {
                    result.HRMARACTSTMAP_ActiveFlag = false;
                }
                else if (result.HRMARACTSTMAP_ActiveFlag == false)
                {
                    result.HRMARACTSTMAP_ActiveFlag = true;
                }
                result.HRMARACTSTMAP_UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
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

        public HRMasterAreaStaffMappingDTO edit(HRMasterAreaStaffMappingDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_ActivityDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.User_Id });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.editarrey = retObject.ToArray();
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
    }
}
