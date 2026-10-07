using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class LocationFeeGroupMappingImpl : Interfaces.LocationFeeGroupMappingInterface
    {
        public TransportContext _context;
        ILogger<LocationFeeGroupMappingImpl> _areaimpl;
        //      public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public LocationFeeGroupMappingImpl(ILogger<LocationFeeGroupMappingImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public TR_Location_FeeGroup_MappingDTO getdata(int id)
        {
            TR_Location_FeeGroup_MappingDTO data = new TR_Location_FeeGroup_MappingDTO();
            try
            {
                data.MI_Id = id;



                var date = DateTime.Now;
                data.yearlist = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id ).Distinct().ToArray();
                data.locationlist = _context.MasterLocationDMO.Where(a => a.MI_Id == id && a.TRML_ActiveFlg == true).Distinct().Distinct().ToArray();

                var year = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id && y.ASMAY_From_Date <= date && y.ASMAY_To_Date >=date).Distinct().ToList();
                if (year.Count >0)
                {
                    data.ASMAY_Id = year[0].ASMAY_Id;
                }
               



                data.grouplist = _context.FeeGroupDMO.Where(w => w.MI_Id == data.MI_Id && w.FMG_ActiceFlag == true).Distinct().ToArray();

                data.griddata = (from a in _context.TR_Location_FeeGroup_MappingDMO
                                 from b in _context.AcademicYear
                                 from c in _context.FeeGroupDMO
                                 from d in _context.MasterLocationDMO
                                 where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && a.TRML_Id == d.TRML_Id && d.TRML_ActiveFlg == true)
                                 select new TR_Location_FeeGroup_MappingDTO
                                 {
                                     TRLFM_Id = a.TRLFM_Id,
                                     FMG_Id=c.FMG_Id,
                                     FMG_GroupName=c.FMG_GroupName,
                                     TRML_Id=d.TRML_Id,
                                     TRML_LocationName=d.TRML_LocationName,
                                     ASMAY_Id=b.ASMAY_Id,
                                     ASMAY_Year=b.ASMAY_Year,
                                     TRLFM_ActiveFlag =a.TRLFM_ActiveFlag
                                 }).ToArray();

                //  data.getmasterarea = _context.MasterAreaDMO.Where(a => a.MI_Id == id).OrderByDescending(a=>a.TRMA_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area getdata" + ex.Message);
            }
            return data;
        }
        public TR_Location_FeeGroup_MappingDTO savedata(TR_Location_FeeGroup_MappingDTO data)
        {
            try
            {
                if (data.TRLFM_Id > 0)
                {
                    var check_duplicate_areaname_update = _context.TR_Location_FeeGroup_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.FMG_Id == data.FMG_Id && a.TRML_Id == data.TRML_Id && a.TRLFM_Id != data.TRLFM_Id).ToList();
                   

                    if (check_duplicate_areaname_update.Count == 0)
                    {
                        var result = _context.TR_Location_FeeGroup_MappingDMO.Single(a => a.MI_Id == data.MI_Id && a.TRLFM_Id == data.TRLFM_Id);
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.FMG_Id = data.FMG_Id;
                        result.TRML_Id = data.TRML_Id;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.retrval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var check_duplicate_areaname = _context.TR_Location_FeeGroup_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMG_Id == data.FMG_Id && a.TRML_Id == data.TRML_Id ).ToList();
                   

                    if (check_duplicate_areaname.Count == 0)
                    {
                        TR_Location_FeeGroup_MappingDMO areadmo = new TR_Location_FeeGroup_MappingDMO();
                        areadmo.MI_Id = data.MI_Id;
                        areadmo.ASMAY_Id = data.ASMAY_Id;
                        areadmo.TRML_Id = data.TRML_Id;
                        areadmo.FMG_Id = data.FMG_Id;
                        areadmo.TRLFM_ActiveFlag = true;
                        areadmo.CreatedDate = DateTime.Now;
                        areadmo.UpdatedDate = DateTime.Now;

                        _context.Add(areadmo);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrval = true;
                        }


                    }
                    else
                    {
                        data.message = "Duplicate";
                    }


                }
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area savedata" + ex.Message);
            }
            return data;
        }

        public TR_Location_FeeGroup_MappingDTO geteditdata(TR_Location_FeeGroup_MappingDTO data)
        {
            try
            {
                data.editdatadetails = _context.TR_Location_FeeGroup_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.TRLFM_Id == data.TRLFM_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area geteditdata " + ex.Message);
            }
            return data;
        }

        public TR_Location_FeeGroup_MappingDTO activedeactive(TR_Location_FeeGroup_MappingDTO data)
        {
            try
            {
                //var check_Area_Used = (from a in _context.MasterRouteDMO
                //                       from b in _context.MasterAreaDMO
                //                       where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRMA_ActiveFlg == true && a.TRMA_Id == data.TRMA_Id && a.TRMR_ActiveFlg == true)
                //                       select new TR_Location_FeeGroup_MappingDTO
                //                       {
                //                           TRMA_Id = a.TRMA_Id
                //                       }).ToList();


            //    if (check_Area_Used.Count > 0)
             ///   {
                  //  data.message = "You Can Not Deactivate This Record Its Already Mapped";
              //  }
              //  else
              //  {
                    var result = _context.TR_Location_FeeGroup_MappingDMO.Single(a => a.MI_Id == data.MI_Id && a.TRLFM_Id == data.TRLFM_Id);
                    if (result.TRLFM_ActiveFlag == false)
                    {
                        result.TRLFM_ActiveFlag = true;
                    }
                    else
                    {
                        result.TRLFM_ActiveFlag = false;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.retrval = true;
                    }
                //}
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _areaimpl.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            }
            return data;
        }
    }
}
