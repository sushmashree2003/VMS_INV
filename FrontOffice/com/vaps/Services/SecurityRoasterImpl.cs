using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class SecurityRoasterImpl:Interfaces.SecurityRoasterInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public SecurityRoasterImpl(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _Context = Context;
        }
        public SecurityRoasterDTO getalldetails(SecurityRoasterDTO data)
        {
            try
            {
                data.alldata = (from a in _VMSContext.HR_Master_Floor_con
                                from b in _VMSContext.SecurityDetailsDMO
                                from c in _VMSContext.SecurityRoasterDMO
                                where (a.HRMF_Id ==c.HRMF_Id &&b.ISMSD_Id==c.ISMSD_Id )
                                select new SecurityRoasterDTO
                                {
                                    ISMSR_Id = c.ISMSR_Id,
                                    ISMSD_Id = b.ISMSD_Id,
                                    ISMSD_FirstName = ((b.ISMSD_FirstName == null ? " " : b.ISMSD_FirstName) + " " + (b.ISMSD_MiddleName == null ? " " : b.ISMSD_MiddleName) + " " + (b.ISMSD_LastName == null ? " " : b.ISMSD_LastName)).Trim(),
                                    ISMSR_Date = c.ISMSR_Date,
                                    name= b.ISMSD_FirstName,
                                    HRMF_Id = c.HRMF_Id,
                                    floorname = a.HRMF_FloorName,
                                    ISMSR_From = c.ISMSR_From,
                                    ISMSR_To = c.ISMSR_To,
                                    ISMSR_Remarks = c.ISMSR_Remarks,
                                    ISMSR_Status = c.ISMSR_Status,
                                    ISMSR_ActiveFlag = c.ISMSR_ActiveFlag                                 
                                }).Distinct().ToArray();

                data.floorlist = (from a in _VMSContext.HR_Master_Floor_con where (a.MI_Id==data.MI_Id)
                                    select new SecurityRoasterDTO
                                    {
                                        HRMF_FloorName = a.HRMF_FloorName,
                                        HRMF_Id = a.HRMF_Id
                                    }).Distinct().OrderBy(t => t.HRMF_FloorName).ToArray();

                data.namelist = (from b in _VMSContext.SecurityDetailsDMO where(b.ISMSD_ActiveFlag == true)
                                 select new SecurityRoasterDTO
                                 {
                                     ISMSD_FirstName = ((b.ISMSD_FirstName == null ? " " : b.ISMSD_FirstName) + " " + (b.ISMSD_MiddleName == null ? " " : b.ISMSD_MiddleName) + " " + (b.ISMSD_LastName == null ? " " : b.ISMSD_LastName)).Trim(),
                                     ISMSD_Id = b.ISMSD_Id
                                 }).Distinct().OrderBy(t => t.ISMSD_FirstName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SecurityRoasterDTO getdata(SecurityRoasterDTO data)
        {
            try
            {
                if (data.ISMSR_Id == 0)
                {
                    if(data.flrlist.Length > 0)
                    {
                        for (int i = 0; i< data.flrlist.Length; i++)
                        {
                            var duplicate = _VMSContext.SecurityRoasterDMO.Where(t => t.ISMSD_Id == data.ISMSD_Id && t.ISMSR_Date == data.ISMSR_Date && t.HRMF_Id == data.flrlist[i] && t.ISMSR_From == data.ISMSR_From && t.ISMSR_To == data.ISMSR_To && t.ISMSR_ActiveFlag == true).Distinct().ToArray();
                            if (duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                SecurityRoasterDMO a = new SecurityRoasterDMO();
                                a.ISMSD_Id = data.ISMSD_Id;
                                a.ISMSR_Date = data.ISMSR_Date;
                                a.HRMF_Id = data.flrlist[i];
                                a.ISMSR_From = data.ISMSR_From;
                                a.ISMSR_To = data.ISMSR_To;
                                a.ISMSR_Status = data.ISMSR_Status;
                                a.ISMSR_Remarks = data.ISMSR_Remarks;
                                a.ISMSR_ActiveFlag = true;
                                a.ISMSR_UpdatedBy = data.UserId;
                                a.ISMSR_CreatedBy = data.UserId;
                                a.UpdatedDate = DateTime.Now;
                                a.CreatedDate = DateTime.Now;
                                _VMSContext.Add(a);
                            }
                            var w = _VMSContext.SaveChanges();
                            if (w > 0)
                            {
                                data.msg = "Saved";
                            }
                            else
                            {
                                data.msg = "Failed";
                            }
                        }
                    }
                }
                else if (data.ISMSR_Id > 0)
                {
                    if (data.ISMSR_Id > 0)
                    {
                        for(int i =0;i<data.flrlist.Length; i++)
                        {
                            var duplicate = _VMSContext.SecurityRoasterDMO.Where(t => t.ISMSD_Id == data.ISMSD_Id && t.ISMSR_Date == data.ISMSR_Date && t.ISMSR_Remarks == data.ISMSR_Remarks && t.ISMSR_Status == data.ISMSR_Status && t.HRMF_Id == data.flrlist[i] && t.ISMSR_From == data.ISMSR_From && t.ISMSR_To == data.ISMSR_To).Distinct().ToArray();
                            if (duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                var j = _VMSContext.SecurityRoasterDMO.Where(t => t.ISMSR_Id == data.ISMSR_Id).SingleOrDefault();

                                j.ISMSD_Id = data.ISMSD_Id;
                                j.ISMSR_Date = data.ISMSR_Date;
                                j.HRMF_Id = data.flrlist[i];
                                j.ISMSR_From = data.ISMSR_From;
                                j.ISMSR_To = data.ISMSR_To;
                                j.ISMSR_Status = data.ISMSR_Status;
                                j.ISMSR_Remarks = data.ISMSR_Remarks;
                                j.ISMSR_ActiveFlag = true;
                                j.ISMSR_UpdatedBy = data.UserId;
                                j.UpdatedDate = DateTime.Now;
                                _VMSContext.Update(j);
                                var r = _VMSContext.SaveChanges();
                                if (r > 0)
                                {
                                    data.msg = "updated";
                                }
                                else
                                {
                                    data.msg = "failed";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SecurityRoasterDTO edittab1(SecurityRoasterDTO data)
        {
            try
            {


                data.Editlist = (from c in _VMSContext.HR_Master_Floor_con
                            from d in _VMSContext.SecurityDetailsDMO
                            from e in _VMSContext.SecurityRoasterDMO
                            where (c.HRMF_Id == e.HRMF_Id && e.ISMSR_Id == data.ISMSR_Id && d.ISMSD_Id == e.ISMSD_Id)

                            select new SecurityRoasterDTO
                            {
                                ISMSR_Id = e.ISMSR_Id,
                                ISMSD_Id = e.ISMSD_Id,
                                name = d.ISMSD_FirstName,
                                ISMSR_Date = e.ISMSR_Date,
                                HRMF_Id = e.HRMF_Id,
                                floorname = c.HRMF_FloorName,
                                ISMSR_From = e.ISMSR_From,
                                ISMSR_To = e.ISMSR_To,
                                ISMSR_Status = e.ISMSR_Status,
                                ISMSR_Remarks = e.ISMSR_Remarks,
                                ISMSR_ActiveFlag = e.ISMSR_ActiveFlag
                            }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SecurityRoasterDTO deactive(SecurityRoasterDTO data)
        {
            try
            {
                var g = _VMSContext.SecurityRoasterDMO.Where(t => t.ISMSR_Id == data.ISMSR_Id).SingleOrDefault();
                if (g.ISMSR_ActiveFlag == true)
                {
                    g.ISMSR_ActiveFlag = false;
                }
                else if (g.ISMSR_ActiveFlag == false)
                {
                    g.ISMSR_ActiveFlag = true;
                }


                _VMSContext.Update(g);
                int s = _VMSContext.SaveChanges();
                if (s > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SecurityRoasterDTO Binddata(SecurityRoasterDTO data)
        {

            data.floorlist = (from a in _VMSContext.HR_Master_Floor_con
                              where (a.MI_Id==data.MI_Id && a.HRMF_ActiveFlag == true)
                              select new SecurityRoasterDTO
                              {
                                  HRMF_FloorName = a.HRMF_FloorName,
                                  HRMF_Id = a.HRMF_Id
                              }).Distinct().OrderBy(t => t.HRMF_Id).ToArray();
            return data;
        }
        public SecurityRoasterDTO report(SecurityRoasterDTO data)
        {
            try
            {
                data.report = (from b in _VMSContext.SecurityDetailsDMO
                               from c in _VMSContext.SecurityRoasterDMO
                               where (c.HRMF_Id == data.HRMF_Id && b.ISMSD_Id == c.ISMSD_Id && c.ISMSR_Date>=data.fromdate && c.ISMSR_Date <= data.todate)
                               select new SecurityRoasterDTO
                               {
                                   ISMSR_Id = c.ISMSR_Id,
                                   ISMSD_Id = b.ISMSD_Id,
                                   ISMSR_Date = c.ISMSR_Date,
                                   ISMSD_FirstName = ((b.ISMSD_FirstName == null ? " " : b.ISMSD_FirstName) + " " + (b.ISMSD_MiddleName == null ? " " : b.ISMSD_MiddleName) + " " + (b.ISMSD_LastName == null ? " " : b.ISMSD_LastName)).Trim(),
                                   HRMF_Id = c.HRMF_Id,
                                   ISMSD_Designation = b.ISMSD_Designation,
                                   ISMSR_From = c.ISMSR_From,
                                   ISMSR_To = c.ISMSR_To,
                               }).Distinct().ToArray();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
