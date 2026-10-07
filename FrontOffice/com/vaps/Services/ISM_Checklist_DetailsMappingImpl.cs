using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using FrontOffice.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;


namespace FrontOffice.com.vaps.Services
{
    public class ISM_Checklist_DetailsMappingImpl : Interfaces.ISM_Checklist_DetailsMappingInterface
    {
        public FOContext _FOContext;
        public ISM_Checklist_DetailsMappingImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }

        public ISM_Checklist_DetailsDTO getalldetails(ISM_Checklist_DetailsDTO data)
        {
            try
            {
                data.checklisttypeList = _FOContext.ISM_Checklist_TypeDMO.Where(t => t.ISM_ActiveFlag == true).ToArray();

                data.floorList = _FOContext.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id && t.HRMF_ActiveFlag == true).ToArray();

                data.checklistmappedList = (from a in _FOContext.ISM_Checklist_DetailsDMO
                                            from b in _FOContext.ISM_Checklist_TypeDMO
                                            from c in _FOContext.HR_Master_Floor
                                            where (a.ISMCLT_Id == b.ISMCLT_Id && a.HRMF_Id == c.HRMF_Id && a.ISMCL_ActiveFlag == true)
                                            select new ISM_Checklist_DetailsDTO
                                            {
                                                ISMCLD_Id = a.ISMCLD_Id,
                                                ISMCLT_Id = a.ISMCLT_Id,
                                                ISMCLT_Name = b.ISMCLT_Name,
                                                ISMCL_Name = a.ISMCL_Name,
                                                ISMCL_ActiveFlag = a.ISMCL_ActiveFlag,
                                                HRMF_Id = a.HRMF_Id,
                                                HRMF_FloorName = c.HRMF_FloorName,
                                                ISMCLD_Qty = a.ISMCLD_Qty,
                                                CreatedDate = a.CreatedDate
                                            }).Distinct().OrderByDescending(p => p.CreatedDate).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public ISM_Checklist_DetailsDTO savedata(ISM_Checklist_DetailsDTO data)
        {
            try
            {
                ISM_Checklist_DetailsDMO obj = Mapper.Map<ISM_Checklist_DetailsDMO>(data);
                if (obj.ISMCLD_Id > 0)
                {
                    var duplicate = _FOContext.ISM_Checklist_DetailsDMO.Where(t => t.ISMCLT_Id == data.ISMCLT_Id 
                    && t.ISMCL_Name == data.ISMCL_Name && t.HRMF_Id == data.HRMF_Id && t.ISMCLD_Qty == data.ISMCLD_Qty 
                    && t.ISMCLD_Id != data.ISMCLD_Id).ToArray();

                    if (duplicate.Length == 0)
                    {
                        var result = _FOContext.ISM_Checklist_DetailsDMO.Single(t => t.ISMCLD_Id == data.ISMCLD_Id);

                        result.ISMCLD_Qty = data.ISMCLD_Qty;
                        result.HRMF_Id = data.HRMF_Id;
                        result.ISMCLT_Id = data.ISMCLT_Id;
                        result.ISMCL_Name = data.ISMCL_Name;
                        result.ISMCL_UserId = data.UserId;
                        result.UpdatedDate = DateTime.Today;                       
                        _FOContext.Update(result);
                        int a = _FOContext.SaveChanges();

                        if (a > 0) { data.retrunMsg = "Add"; }
                        else { data.retrunMsg = "False"; }
                    }
                    else
                    {
                        data.retrunMsg = "Duplicate";
                    }
                }
                else
                {
                    var duplicate = _FOContext.ISM_Checklist_DetailsDMO.Where(t => t.ISMCLT_Id == data.ISMCLT_Id && t.ISMCL_Name == data.ISMCL_Name && t.HRMF_Id == data.HRMF_Id && t.ISMCLD_Qty == data.ISMCLD_Qty).ToArray();

                    if (duplicate.Length == 0)
                    {
                        obj.ISMCL_UserId = data.UserId;
                        obj.ISMCL_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Today;
                        obj.UpdatedDate = DateTime.Today;
                        _FOContext.Add(obj);
                        int a = _FOContext.SaveChanges();

                        if (a > 0) { data.retrunMsg = "Add"; }
                        else { data.retrunMsg = "False"; }
                    }
                    else
                    {
                        data.retrunMsg = "Duplicate";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public ISM_Checklist_DetailsDTO approvalloaddata(ISM_Checklist_DetailsDTO data)
        {
            try
            {
                data.checklisttypeList = _FOContext.ISM_Checklist_TypeDMO.Where(t => t.ISM_ActiveFlag == true).ToArray();

                data.floorList = _FOContext.HR_Master_Floor.Where(t => t.MI_Id == 17 && t.HRMF_ActiveFlag == true).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public async Task<ISM_Checklist_DetailsDTO> findchecklistAsync(ISM_Checklist_DetailsDTO data)
        {
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHECKLIST_APPROVALDATA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Userid", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMCLT_Id", SqlDbType.BigInt)
                    {
                        Value = data.ISMCLT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMF_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRMF_Id
                    });
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
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.checklistmappedList = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.checklistmappedList = (from a in _FOContext.ISM_Checklist_DetailsDMO
                //                           join b in _FOContext.ISM_Checklist_ApprovalDMO on a.ISMCLD_Id equals b.ISMCLD_Id into gj
                //                           from subpet in gj.Where(f => f.ISMCLT_UserId == data.UserId).DefaultIfEmpty()
                //                           where(a.ISMCLT_Id == data.ISMCLT_Id && a.HRMF_Id == data.HRMF_Id && a.ISMCL_ActiveFlag == true)
                //                           select new ISM_Checklist_DetailsDTO
                //                           {
                //                               ISMCLD_Id = a.ISMCLD_Id,
                //                               ISMCL_Name=a.ISMCL_Name,
                //                               HRMF_Id = a.HRMF_Id,
                //                               ISMCLD_Qty = a.ISMCLD_Qty,
                //                               ISMCLT_Qty = subpet.ISMCLT_Qty,
                //                               ISMCLT_Remarks = subpet.ISMCLT_Remarks
                //                           }).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            //data.checklistmappedList = (from a in _FOContext.ISM_Checklist_DetailsDMO
            //                           join b in _FOContext.ISM_Checklist_ApprovalDMO on a.ISMCLD_Id equals b.ISMCLD_Id into gj
            //                           from subpet in gj.Where(f=> f.ISMCLT_UserId == data.UserId && f.ISMCLT_Date == DateTime.Today).DefaultIfEmpty()
            //                           where(a.ISMCLT_Id == data.ISMCLT_Id && a.HRMF_Id == data.HRMF_Id && a.ISMCL_ActiveFlag == true)
            //                           select new ISM_Checklist_DetailsDTO
            //                           {
            //                               ISMCLD_Id = a.ISMCLD_Id,
            //                               ISMCL_Name=a.ISMCL_Name,
            //                               HRMF_Id = a.HRMF_Id,
            //                               ISMCLD_Qty = a.ISMCLD_Qty,
            //                               ISMCLT_Qty = subpet.ISMCLT_Qty,
            //                               ISMCLT_Remarks = subpet.ISMCLT_Remarks
            //                           }).ToArray();

            return data;
        }

        public ISM_Checklist_DetailsDTO saveapproval(ISM_Checklist_DetailsDTO data)
        {
            try
            {
                for (int i = 0; i < data.ApprovalTempDTO.Length; i++)
                {
                    ISM_Checklist_ApprovalDMO obj = new ISM_Checklist_ApprovalDMO();
                    var duplicate = _FOContext.ISM_Checklist_ApprovalDMO.Where(t => t.ISMCLD_Id == data.ApprovalTempDTO[i].ISMCLD_Id && Convert.ToDateTime(t.ISMCLT_Date) == Convert.ToDateTime(DateTime.Today) && t.ISMCLT_UserId == data.UserId).FirstOrDefault();

                    if (duplicate == null)
                    {
                        obj.ISMCLD_Id = data.ApprovalTempDTO[i].ISMCLD_Id;
                        obj.ISMCLT_Date = DateTime.Today;
                        obj.ISMCLT_UserId = data.UserId;
                        obj.ISMCLT_Remarks = data.ApprovalTempDTO[i].ISMCLT_Remarks;
                      
                        obj.ISMCLD_MorningQty = data.ApprovalTempDTO[i].ISMCLD_MorningQty;
                        obj.ISMCLD_EveningQty = data.ApprovalTempDTO[i].ISMCLD_EveningQty;
                        obj.ISMCLT_UserId = data.UserId;
                        obj.ISMCLT_CreatedBy = data.UserId;
                        obj.ISMCLT_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Today;
                        obj.UpdatedDate = DateTime.Today;
                        obj.ISMCLT_FilePath = data.ISMCLT_FilePath;
                        _FOContext.Add(obj);
                        int a = _FOContext.SaveChanges();
                    }
                    else
                    {
                        duplicate.ISMCLT_UpdatedBy = data.UserId;
                        duplicate.UpdatedDate = DateTime.Today;
                        duplicate.ISMCLT_Remarks = data.ApprovalTempDTO[i].ISMCLT_Remarks;
                        duplicate.ISMCLD_MorningQty = data.ApprovalTempDTO[i].ISMCLD_MorningQty;
                        duplicate.ISMCLD_EveningQty = data.ApprovalTempDTO[i].ISMCLD_EveningQty;

                        duplicate.ISMCLT_FilePath = data.ISMCLT_FilePath;
                        _FOContext.Update(duplicate);
                        int a = _FOContext.SaveChanges();
                    }
                }
                data.retrunMsg = "Add";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                data.retrunMsg = "False";
            }
            return data;
        }

        public ISM_Checklist_DetailsDTO checklistreport(ISM_Checklist_DetailsDTO data)
        {
            try
            {
                data.userrole = (from a in _FOContext.ApplicationUserRoleDMO
                                 from b in _FOContext.ApplRole
                                 where (a.RoleId == b.Id && a.UserId == data.UserId)
                                 select b.Name.ToUpper()).FirstOrDefault();

                data.userrolearray = (from a in _FOContext.ApplicationUserRoleDMO
                                      from b in _FOContext.ApplRole
                                      from c in _FOContext.ISM_Checklist_ApprovalDMO
                                      where (a.RoleId == b.Id && a.UserId == c.ISMCLT_UserId && (c.ISMCLT_Date.Value.Date >= Convert.ToDateTime(data.myDate_from).Date && c.ISMCLT_Date.Value.Date <= Convert.ToDateTime(data.myDate_to).Date))
                                      select b.Name.ToUpper()).Distinct().ToArray();

                data.columnnames = (from a in Enumerable.Range(0, (Convert.ToDateTime(data.myDate_to) - Convert.ToDateTime(data.myDate_from)).Days + 1)
                                    let columndate = Convert.ToDateTime(data.myDate_from).AddDays(a)
                                    select columndate).Distinct().ToArray();

                if (data.userrole != "ADMIN")
                {
                    data.filldata = (from a in _FOContext.ISM_Checklist_ApprovalDMO
                                     from b in _FOContext.ISM_Checklist_DetailsDMO
                                     where (a.ISMCLD_Id == b.ISMCLD_Id && (a.ISMCLT_Date.Value.Date >= Convert.ToDateTime(data.myDate_from).Date && a.ISMCLT_Date.Value.Date <= Convert.ToDateTime(data.myDate_to).Date) && b.ISMCLT_Id == data.ISMCLT_Id && b.HRMF_Id == data.HRMF_Id && a.ISMCLT_UserId == data.UserId)
                                     select new ISM_Checklist_DetailsDTO
                                     {
                                         ISMCL_Name = b.ISMCL_Name,
                                         ISMCLT_Date = a.ISMCLT_Date,
                                         ISMCLT_Remarks = a.ISMCLT_Remarks,
                                         ISMCLD_MorningQty=a.ISMCLD_MorningQty,
                                         ISMCLD_EveningQty=a.ISMCLD_EveningQty,
                                         //ISMCLT_Qty = a.ISMCLT_Qty,
                                         ISMCLT_FilePath = a.ISMCLT_FilePath
                                     }).OrderBy(b => b.ISMCL_Name).ThenBy(d => d.ISMCLT_Date).ToArray();
                }
                else
                {
                    data.filldata = (from a in _FOContext.ISM_Checklist_ApprovalDMO
                                     from b in _FOContext.ISM_Checklist_DetailsDMO
                                     from c in _FOContext.ApplicationUserRoleDMO
                                     from d in _FOContext.ApplRole
                                     where (a.ISMCLD_Id == b.ISMCLD_Id && (a.ISMCLT_Date.Value.Date >= Convert.ToDateTime(data.myDate_from).Date && a.ISMCLT_Date.Value.Date <= Convert.ToDateTime(data.myDate_to).Date) && b.ISMCLT_Id == data.ISMCLT_Id && b.HRMF_Id == data.HRMF_Id && a.ISMCLT_UserId == c.UserId && c.RoleId == d.Id)
                                     select new ISM_Checklist_DetailsDTO
                                     {
                                         ISMCL_Name = b.ISMCL_Name,
                                         ISMCLT_Date = a.ISMCLT_Date,
                                         ISMCLT_Remarks = a.ISMCLT_Remarks,
                                         ISMCLD_MorningQty = a.ISMCLD_MorningQty,
                                         ISMCLD_EveningQty = a.ISMCLD_EveningQty,
                                         //ISMCLT_Qty = a.ISMCLT_Qty,
                                         ISMCLT_UserId = a.ISMCLT_UserId,
                                         ISMCLT_FilePath = a.ISMCLT_FilePath,
                                         userrole = d.Name
                                     }).OrderBy(b => b.ISMCL_Name).ThenBy(d => d.ISMCLT_Date).ThenBy(e => e.ISMCLT_UserId).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                data.retrunMsg = "False";
            }
            return data;
        }

        public ISM_Checklist_DetailsDTO editRecord(ISM_Checklist_DetailsDTO data)
        {
            // ISM_Checklist_DetailsDTO data = new ISM_Checklist_DetailsDTO();
            try
            {
                data.masterdit = (from a in _FOContext.ISM_Checklist_DetailsDMO
                                  from b in _FOContext.ISM_Checklist_TypeDMO
                                  from c in _FOContext.HR_Master_Floor
                                  where (a.ISMCLT_Id == b.ISMCLT_Id && a.HRMF_Id == c.HRMF_Id && a.ISMCLD_Id == data.ISMCLD_Id)
                                  select new ISM_Checklist_DetailsDTO
                                  {

                                      ISMCLD_Id = a.ISMCLD_Id,
                                      ISMCLT_Id = a.ISMCLT_Id,
                                      ISMCLT_Name = b.ISMCLT_Name,
                                      ISMCL_Name = a.ISMCL_Name,
                                      ISMCL_ActiveFlag = a.ISMCL_ActiveFlag,
                                      HRMF_Id = a.HRMF_Id,
                                      HRMF_FloorName = c.HRMF_FloorName,
                                      ISMCLD_Qty = a.ISMCLD_Qty
                                  }).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public ISM_Checklist_DetailsDTO DeleteYss(ISM_Checklist_DetailsDTO data)
        {

            try
            {

                var result = _FOContext.ISM_Checklist_DetailsDMO.Single(t => t.ISMCLD_Id == data.ISMCLD_Id);
                if (result.ISMCL_ActiveFlag == true)
                {
                    result.ISMCL_ActiveFlag = false;
                }
                else
                {
                    result.ISMCL_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;


                _FOContext.Update(result);
                int rowAffected = _FOContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.retrunMsg = "Remove";
                }
                else
                {
                    data.retrunMsg = "NotRemove";
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
