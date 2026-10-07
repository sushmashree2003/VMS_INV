using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class SecurityDetailsImpl : Interfaces.SecurityDetailsInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public SecurityDetailsImpl(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _Context = Context;
        }

        public SecurityDetailsDTO getalldetails(SecurityDetailsDTO data)
        {
            try
            {
                data.alldata = (from a in _VMSContext.IVRM_Master_Country
                                from b in _VMSContext.SecurityDetailsDMO
                                from c in _VMSContext.IVRM_Master_State
                                where (a.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMS_Id == b.IVRMMS_Id)

                                select new SecurityDetailsDTO
                                {
                                    ISMSD_Id = b.ISMSD_Id,
                                    ISMSD_FirstName = ((b.ISMSD_FirstName == null ? " " : b.ISMSD_FirstName) + " " + (b.ISMSD_MiddleName == null ? " " : b.ISMSD_MiddleName) + " " + (b.ISMSD_LastName == null ? " " : b.ISMSD_LastName)).Trim(),
                                    ISMSD_MobileNo = b.ISMSD_MobileNo,
                                    ISMSD_EmailId = b.ISMSD_EmailId,
                                    ISMSD_PerStreet = b.ISMSD_PerStreet,
                                    ISMSD_PerArea = b.ISMSD_PerArea,
                                    ISMSD_PerCity = b.ISMSD_PerCity,
                                    ISMSD_PerPincode = b.ISMSD_PerPincode,
                                    IVRMMC_Id = a.IVRMMC_Id,
                                    countryname = a.IVRMMC_CountryName,
                                    IVRMMS_Id = c.IVRMMS_Id,
                                    statename = c.IVRMMS_Name,
                                    ISMSD_Photo = b.ISMSD_Photo,
                                    ISMSD_Aadhar = b.ISMSD_Aadhar,
                                    ISMSD_Designation = b.ISMSD_Designation,
                                    ISMSD_PANNo = b.ISMSD_PANNo,
                                    ISMSD_ActiveFlag = b.ISMSD_ActiveFlag,

                                }).Distinct().ToArray();

                data.countrylist = (from a in _VMSContext.IVRM_Master_Country
                                    select new SecurityDetailsDTO
                                    {
                                        IVRMMC_CountryName = a.IVRMMC_CountryName,
                                        IVRMMC_Id = a.IVRMMC_Id,
                                        IVRMMC_CountryCode = a.IVRMMC_CountryCode
                                    }).Distinct().OrderBy(t => t.IVRMMC_CountryName).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SecurityDetailsDTO getdata(SecurityDetailsDTO data)
        {
            try
            {
                if (data.ISMSD_Id == 0)
                {
                    var duplicate = _VMSContext.SecurityDetailsDMO.Where(t => t.ISMSD_Id == data.ISMSD_Id).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        SecurityDetailsDMO a = new SecurityDetailsDMO();

                        a.ISMSD_FirstName = data.ISMSD_FirstName;
                        a.ISMSD_MiddleName = data.ISMSD_MiddleName;
                        a.ISMSD_LastName = data.ISMSD_LastName;
                        a.ISMSD_MobileNo = data.ISMSD_MobileNo;
                        a.ISMSD_EmailId = data.ISMSD_EmailId;
                        a.ISMSD_PerStreet = data.ISMSD_PerStreet;
                        a.ISMSD_PerArea = data.ISMSD_PerArea;
                        a.ISMSD_PerCity = data.ISMSD_PerCity;
                        a.ISMSD_PerPincode = data.ISMSD_PerPincode;
                        a.IVRMMC_Id = data.IVRMMC_Id;
                        //a.countryname = data.IVRMMC_CountryName;
                        a.IVRMMS_Id = data.IVRMMS_Id;
                        //a.statename = data.IVRMMS_Name;
                        a.ISMSD_Photo = data.ISMSD_Photo;
                        a.ISMSD_Aadhar = data.ISMSD_Aadhar;
                        a.ISMSD_Designation = data.ISMSD_Designation;
                        a.ISMSD_PANNo = data.ISMSD_PANNo;
                        a.ISMSD_ActiveFlag = true;
                        a.CreatedDate = DateTime.Now;
                        a.UpdatedDate = DateTime.Now;
                        a.ISMSD_CreatedBy = data.UserId;
                        a.ISMSD_UpdatedBy = data.UserId;
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
                else if (data.ISMSD_Id > 0)
                {
                    var duplicate = _VMSContext.SecurityDetailsDMO.Where(t => t.ISMSD_Id != data.ISMSD_Id && t.ISMSD_FirstName == data.ISMSD_FirstName && t.ISMSD_LastName == data.ISMSD_LastName && t.ISMSD_Designation == data.ISMSD_Designation).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var j = _VMSContext.SecurityDetailsDMO.Where(t => t.ISMSD_Id == data.ISMSD_Id).SingleOrDefault();

                        j.ISMSD_FirstName = data.ISMSD_FirstName;
                        j.ISMSD_MiddleName = data.ISMSD_MiddleName;
                        j.ISMSD_LastName = data.ISMSD_LastName;
                        j.ISMSD_MobileNo = data.ISMSD_MobileNo;
                        j.ISMSD_EmailId = data.ISMSD_EmailId;
                        j.ISMSD_PerStreet = data.ISMSD_PerStreet;
                        j.ISMSD_PerArea = data.ISMSD_PerArea;
                        j.ISMSD_PerCity = data.ISMSD_PerCity;
                        j.ISMSD_PerPincode = data.ISMSD_PerPincode;
                        j.IVRMMC_Id = data.IVRMMC_Id;
                        //a.countryname = data.IVRMMC_CountryName;
                        j.IVRMMS_Id = data.IVRMMS_Id;
                        //a.statename = data.IVRMMS_Name;
                        j.ISMSD_Photo = data.ISMSD_Photo;
                        j.ISMSD_Aadhar = data.ISMSD_Aadhar;
                        j.ISMSD_Designation = data.ISMSD_Designation;
                        j.ISMSD_PANNo = data.ISMSD_PANNo;
                        //j.ISMSD_ActiveFlag = data.ISMSD_ActiveFlag;
                        j.CreatedDate = DateTime.Now;
                        j.UpdatedDate = DateTime.Now;
                        j.ISMSD_CreatedBy = data.UserId;
                        j.ISMSD_UpdatedBy = data.UserId;
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
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SecurityDetailsDTO edittab1(SecurityDetailsDTO data)
        {
            try
            {


                var edit = (from c in _VMSContext.IVRM_Master_Country
                                 from d in _VMSContext.SecurityDetailsDMO
                                 from e in _VMSContext.IVRM_Master_State
                                 where (c.IVRMMC_Id == d.IVRMMC_Id && d.ISMSD_Id == data.ISMSD_Id && d.IVRMMS_Id == e.IVRMMS_Id)

                                 select new SecurityDetailsDTO
                                 {
                                     ISMSD_Id = d.ISMSD_Id,
                                     ISMSD_FirstName = d.ISMSD_FirstName,
                                     ISMSD_MiddleName = d.ISMSD_MiddleName,
                                     ISMSD_LastName = d.ISMSD_LastName,                                    
                                     ISMSD_MobileNo = d.ISMSD_MobileNo,
                                     ISMSD_EmailId = d.ISMSD_EmailId,
                                     ISMSD_PerStreet = d.ISMSD_PerStreet,
                                     ISMSD_PerArea = d.ISMSD_PerArea,
                                     ISMSD_PerCity = d.ISMSD_PerCity,
                                     ISMSD_PerPincode = d.ISMSD_PerPincode,
                                     IVRMMC_Id = c.IVRMMC_Id,
                                     countryname = c.IVRMMC_CountryName,
                                     IVRMMS_Id = e.IVRMMS_Id,
                                     statename = e.IVRMMS_Name,
                                     ISMSD_Photo = d.ISMSD_Photo,
                                     ISMSD_Aadhar = d.ISMSD_Aadhar,
                                     ISMSD_Designation = d.ISMSD_Designation,
                                     ISMSD_PANNo = d.ISMSD_PANNo,
                                     ISMSD_ActiveFlag = d.ISMSD_ActiveFlag,


                                 }).Distinct().ToList();
                if (edit.Count > 0)
                {
                    data.Editlist = edit.ToArray();
                    data.IVRMMC_Id = edit[0].IVRMMC_Id;

                    data.statelistdata = (from b in _VMSContext.IVRM_Master_State
                                          where (b.IVRMMC_Id == data.IVRMMC_Id)
                                          select new SecurityDetailsDTO
                                          {
                                              IVRMMS_Name = b.IVRMMS_Name,
                                              IVRMMS_Id = b.IVRMMS_Id,
                                              IVRMMS_Code = b.IVRMMS_Code,
                                          }).Distinct().OrderBy(t => t.IVRMMS_Name).ToArray();
                }
                

                     

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SecurityDetailsDTO deactive(SecurityDetailsDTO data)
        {
            try
            {
                var g = _VMSContext.SecurityDetailsDMO.Where(t => t.ISMSD_Id == data.ISMSD_Id).SingleOrDefault();
                if (g.ISMSD_ActiveFlag == true)
                {
                    g.ISMSD_ActiveFlag = false;
                }
                else if (g.ISMSD_ActiveFlag == false)
                {
                    g.ISMSD_ActiveFlag = true;
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
        public SecurityDetailsDTO Get_StateData(SecurityDetailsDTO data)
        {
            try
            {
                data.statelistdata = (from b in _VMSContext.IVRM_Master_State
                                      where (b.IVRMMC_Id == data.IVRMMC_Id)
                                      select new SecurityDetailsDTO
                                      {
                                          IVRMMS_Name = b.IVRMMS_Name,
                                          IVRMMS_Id = b.IVRMMS_Id,
                                          IVRMMS_Code = b.IVRMMS_Code,
                                      }).Distinct().OrderBy(t => t.IVRMMS_Name).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
