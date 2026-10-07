using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager.HRMasterArea;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Implimentation
{
    public class NocCreationImpl : Interface.NocCreationInterface
    {

        public IssueManagerContext _context;
        ILogger<NocCreationImpl> _logInv;
        private object stafflogin;
        private object t;

        public NocCreationImpl(IssueManagerContext context, ILogger<NocCreationImpl> log)
        {
            _context = context;
            _logInv = log;
        }

        public NocCreationDTO getloaddata(NocCreationDTO data)
        {
            try
            {
                data.tname = _context.TenantAgreement.Distinct().ToArray();
                data.get_building = _context.HR_Master_Building.Where(a => a.MI_Id == data.MI_Id).ToArray();
               // data.get_floor = _context.HR_Master_Floor.Where(a => a.MI_Id == data.MI_Id).ToArray();
                data.get_Area = _context.HR_Master_Room.Where(a => a.MI_Id == data.MI_Id).ToArray();

                //data.staffmappinglistTemp = _context.NocCreationDMO.Distinct().ToArray();

                data.staffmappinglist = (from a in _context.TenantAgreement
                                         from b in _context.HR_Master_Room
                                         from c in _context.NocCreationDMO
                                         where (a.MI_Id == b.MI_Id && a.HRMB_Id == b.HRMB_Id && a.TENANTAGR_Id == c.TENANTAGR_Id && a.HRMR_Id==b.HRMR_Id)
                                         select new NocCreationDTO
                                         {
                                             name = a.TENANTAGR_TenantName,
                                             area = b.HRMR_RoomName,
                                             remarks = c.TENANTNOC_Remarks,
                                             date = c.TENANTNOC_NOCDate,
                                             TENANTNOC_ActiveFlag = c.TENANTNOC_ActiveFlag,
                                             TENANTNOC_Id = c.TENANTNOC_Id,
                                             HRMB_Id = a.HRMB_Id,
                                             TENANTAGR_Id = a.TENANTAGR_Id,
                                             HRMR_Id = a.HRMR_Id,
                                             



                                         }).Distinct().ToArray();


                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "BUILDING_DETAILSPROD";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //SqlDbType.VarChar)
                //    {
                //        Value = data.MI_Id
                //    });
                 
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader =  cmd.ExecuteReader())
                //        {
                //            while ( dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.get_report = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }
        public NocCreationDTO getfloors(NocCreationDTO data)
        {
            try
            {
                //  data.get_floor = _context.HR_Master_Floor.Where(a => a.HRMF_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id).Distinct().ToArray();
               // data.get_getAreaName = _context.HR_Master_Room.Where(a => a.HRMR_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id).Distinct().ToArray();

                data.get_getAreaName = (from a in _context.HR_Master_Room
                                        from b in _context.TenantAgreement
                                        where (a.HRMB_Id == b.HRMB_Id  && a.HRMR_ActiveFlag == true && a.HRMR_Id == data.HRMR_Id && b.HRMB_Id == data.HRMB_Id && b.TENANTAGR_Id == data.TENANTAGR_Id)
                                        select a).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NocCreationDTO getAreaName(NocCreationDTO data)
        {
            try
            {
                // data.get_getAreaName = _context.HR_Master_Room.Where(a =>a.HRMR_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id && a.HRMF_Id == data.HRMF_Id).Distinct().ToArray();

                data.tenantlist = _context.TenantAgreement.Where(a => a.HRMR_Id == data.HRMR_Id && a.HRMB_Id == data.HRMB_Id && a.TENANTAGR_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
   
        public NocCreationDTO gettenantname(NocCreationDTO data)
        {
            try
            {
                data.tenantlist = _context.NocCreationDMO.Where(a => a.TENANTNOC_ActiveFlag == true && a.TENANTNOC_Id == data.TENANTNOC_Id).Distinct().ToArray();

                data.tname = _context.TenantAgreement.Where(a => a.HRMB_Id == data.HRMB_Id && a.TENANTAGR_ActiveFlg == true).Distinct().ToArray();

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    
        public NocCreationDTO deactive(NocCreationDTO data)
        {
            try            {                var result = _context.NocCreationDMO.Single(t => t.TENANTNOC_Id == data.TENANTNOC_Id);                if (result.TENANTNOC_ActiveFlag == true)                {                    result.TENANTNOC_ActiveFlag = false;                }                else if (result.TENANTNOC_ActiveFlag == false)                {                    result.TENANTNOC_ActiveFlag = true;                }                result.TENANTNOC_UpdatedDate = DateTime.Now;                _context.Update(result);                int returnval = _context.SaveChanges();                if (returnval > 0)                {                    data.returnval = true;                }                else                {                    data.returnval = false;                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return data;        }

        //public NocCreationDTO edit(NocCreationDTO data)
        //{
        //    try
        //    {
        //        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "QRcode_ActivityDetails";
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.User_Id });
        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();

        //            var retObject = new List<dynamic>();
        //            try
        //            {
        //                using (var dataReader = cmd.ExecuteReader())
        //                {
        //                    while (dataReader.Read())
        //                    {
        //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                        {
        //                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
        //                        }
        //                        retObject.Add((ExpandoObject)dataRow);
        //                    }
        //                }
        //                data.editarrey = retObject.ToArray();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return data;
        //}

        public NocCreationDTO savedetailsStaffMapping(NocCreationDTO data)
        {
            try
            {
                //var template = _context.NocCreationDMO.ToList();
                //var Mailcontent = template.FirstOrDefault().HRME_Id;
                //var IDDD = template.FirstOrDefault().TENANTAGR_Id;


                var HRME_ID = _context.Staff_User_Login.Where(t => t.Id == data.User_Id).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.TENANTNOC_Id != 0)
                    {
                        var dublicatecheck = _context.NocCreationDMO.Where(t => t.TENANTAGR_Id ==data.TENANTAGR_Id && t.TENANTNOC_Remarks==data.TENANTNOC_Remarks).ToList();
                        if (dublicatecheck.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _context.NocCreationDMO.Single(t =>  t.TENANTAGR_Id == data.TENANTAGR_Id);
                            //result.TENANTAGR_Id = data.TENANTAGR_Id;
                            

                            result.TENANTNOC_NOCDate = data.TENANTNOC_NOCDate;
                            result.TENANTNOC_Remarks = data.TENANTNOC_Remarks;
                            result.TENANTAGR_Id = data.TENANTAGR_Id;
                            result.HRME_Id = HRME_ID;
                            result.TENANTNOC_CreatedDate = DateTime.Now;
                            result.TENANTNOC_UpdatedDate = DateTime.Now;
                            result.TENANTNOC_CreatedBy = data.User_Id;
                            result.TENANTNOC_UpdatedBy = data.User_Id;
                            _context.Update(result);

                            var contactExists = _context.SaveChanges();
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
                        var res = _context.NocCreationDMO.Where(t => ( t.TENANTNOC_NOCDate == data.TENANTNOC_NOCDate && t.TENANTAGR_Id == data.TENANTAGR_Id)).ToList();

                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            NocCreationDMO area = new NocCreationDMO();
                           // area.TENANTAGR_Id = data.TENANTAGR_Id;
                           // area.HRME_Id = 1;
                            
                            area.TENANTNOC_NOCDate = data.TENANTNOC_NOCDate;
                            area.TENANTNOC_Remarks = data.TENANTNOC_Remarks;
                            area.TENANTAGR_Id = data.TENANTAGR_Id;
                            area.HRME_Id = HRME_ID;
                            area.TENANTNOC_ActiveFlag = true;
                            area.TENANTNOC_CreatedDate = DateTime.Now;
                            area.TENANTNOC_UpdatedDate = DateTime.Now;
                            area.TENANTNOC_CreatedBy = data.User_Id;
                            area.TENANTNOC_UpdatedBy = data.User_Id;
                            _context.Add(area);

                            var contactExists = _context.SaveChanges();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
