using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Services
{
    public class TADA_CityMappingIMPL : Interface.TADACityMappingInterface
    {
        public IssueManagerContext _context;
        public TADA_CityMappingIMPL(IssueManagerContext context)
        {
            _context = context;
        }
        public TRDACityMappingDTO loaddata(TRDACityMappingDTO data)
        {
            try

            {
                data.Designation_types = _context.HR_Master_Designation.Where(t => t.HRMDES_ActiveFlag == true &&t.MI_Id==data.MI_Id).Distinct().ToArray();
                data.state = _context.IVRM_Master_StateDMO.Distinct().ToArray();          

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TRDACityMappingDTO getcity(TRDACityMappingDTO data)
        {
            try
            {
                data.City_Type = _context.IVRM_Master_CityDMO.Where(p => p.IVRMMS_Id == data.IVRMMS_Id).Distinct().ToArray();
                //&& a.MI_Id == data.MI_Id
                if (data.message== "BDM")
                {
                   
                    data.Get_CityMapping = (from a in _context.VMS_TADA_CityMappingBDM
                                            from b in _context.HR_Master_Designation
                                            from c in _context.IVRM_Master_CityDMO
                                            where (a.HRMDES_Id == b.HRMDES_Id && a.IVRMMCT_Id == c.IVRMMCT_Id && c.IVRMMS_Id == data.IVRMMS_Id && a.HRMDES_Id == data.HRMDES_Id)
                                            //&& a.VTADACM_ActiveFlag == true
                                            select new TRDACityMappingDTO
                                            {
                                                HRMDES_DesignationName = b.HRMDES_DesignationName,
                                                IVRMMCT_Name = c.IVRMMCT_Name,
                                                VTADACM_FoodAmt = a.VTADACM_FoodAmt,
                                                VTADACM_TransportAmt = a.VTADACM_TransportAmt,
                                                VTADACM_AccommodationAmt = a.VTADACM_AccommodationAmt,
                                                VTADACM_TravelHour = a.VTADACM_TravelHour,
                                                VTADACM_Id = a.VTADACM_Id,
                                                HRMDES_Id = a.HRMDES_Id,
                                                VTADACM_ActiveFlag = a.VTADACM_ActiveFlag
                                            }

                                         ).Distinct().ToArray();
                }
                else
                {
                    
                    data.Get_CityMapping = (from a in _context.TADA_CityMappingDMO
                                            from b in _context.HR_Master_Designation
                                            from c in _context.IVRM_Master_CityDMO
                                            where (a.HRMDES_Id == b.HRMDES_Id && a.IVRMMCT_Id == c.IVRMMCT_Id && c.IVRMMS_Id == data.IVRMMS_Id && a.HRMDES_Id == data.HRMDES_Id)
                                            //&& a.VTADACM_ActiveFlag == true
                                            select new TRDACityMappingDTO
                                            {
                                                HRMDES_DesignationName = b.HRMDES_DesignationName,
                                                IVRMMCT_Name = c.IVRMMCT_Name,
                                                VTADACM_FoodAmt = a.VTADACM_FoodAmt,
                                                VTADACM_TransportAmt = a.VTADACM_TransportAmt,
                                                VTADACM_AccommodationAmt = a.VTADACM_AccommodationAmt,
                                                VTADACM_TravelHour = a.VTADACM_TravelHour,
                                                VTADACM_Id = a.VTADACM_Id,
                                                HRMDES_Id = a.HRMDES_Id,
                                                VTADACM_ActiveFlag = a.VTADACM_ActiveFlag
                                            }

                                         ).Distinct().ToArray();
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TRDACityMappingDTO getMasterReport(TRDACityMappingDTO data)
        {
            try
            {
                data.MI_Logo = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Select(R => R.MI_Logo).FirstOrDefault();
                List<long> sec = new List<long>();
               
                if (data.List != null)
                {
                    foreach (var i in data.List)
                    {
                        sec.Add(i.HRMDES_IdS);
                        
                    }
                }
                data.Get_MasterReport = (from a in _context.TADA_CityMappingDMO
                                         from b in _context.HR_Master_Designation
                                         from c in _context.IVRM_Master_CityDMO
                                         where (a.HRMDES_Id == b.HRMDES_Id && a.IVRMMCT_Id == c.IVRMMCT_Id  && c.IVRMMS_Id == data.IVRMMS_Id   && a.MI_Id==data.MI_Id && sec.Contains(a.HRMDES_Id))                                        
                                         select new TRDACityMappingDTO
                                         {
                                             HRMDES_DesignationName = b.HRMDES_DesignationName,
                                             IVRMMCT_Name = c.IVRMMCT_Name,
                                             VTADACM_FoodAmt = a.VTADACM_FoodAmt,
                                             VTADACM_TransportAmt = a.VTADACM_TransportAmt,
                                             VTADACM_AccommodationAmt = a.VTADACM_AccommodationAmt,
                                             VTADACM_TravelHour = a.VTADACM_TravelHour,
                                             VTADACM_ActiveFlag = a.VTADACM_ActiveFlag,
                                             VTADACM_Id=a.VTADACM_Id,
                                             HRMDES_Id=b.HRMDES_Id
                                         }

                                     ).Distinct().ToArray();               
                data.company_name = _context.Institution.Single(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).MI_Name;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TRDACityMappingDTO saverecord(TRDACityMappingDTO data)
        {
            try
            {
                if(data.message=="BDM")
                {
                    if (data.VTADACM_Id == 0)
                    {
                        data.message = "";
                        data.returnval = "Add";
                        for (int c = 0; c < data.city_array.Length; c++)
                        {
                            var duplicate = _context.VMS_TADA_CityMappingBDM.Where(p => p.MI_Id == data.MI_Id
                            && p.HRMDES_Id == data.HRMDES_Id
                            && p.IVRMMCT_Id == data.city_array[c].IVRMMCT_Id).ToList();
                            if (duplicate.Count == 0)
                            {
                                VMS_TADA_CityMappingBDM obj = new VMS_TADA_CityMappingBDM();
                                obj.MI_Id = data.MI_Id;
                                obj.HRMDES_Id = data.HRMDES_Id;
                                obj.IVRMMCT_Id = data.city_array[c].IVRMMCT_Id;
                                obj.VTADACM_FoodAmt = data.VTADACM_FoodAmt;
                                obj.VTADACM_TransportAmt = data.VTADACM_TransportAmt;
                                obj.VTADACM_AccommodationAmt = data.VTADACM_AccommodationAmt;
                                obj.VTADACM_TravelHour = data.VTADACM_TravelHour;
                                obj.VTADACM_ActiveFlag = true;
                                obj.VTADACM_CreatedDate = DateTime.Now;
                                obj.VTADACM_UpdatedDate = DateTime.Now;
                                obj.VTADACM_CreatedBy = data.UserId;
                                obj.VTADACM_UpdatedBy = data.UserId;
                                _context.Add(obj);
                            }
                            else
                            {
                                if (data.message != "")
                                {
                                    data.message = data.city_array[c].IVRMMCT_Name;
                                }
                                else
                                {
                                    data.message = data.message + ", " + data.city_array[c].IVRMMCT_Name;
                                }
                            }
                        }
                        int a = _context.SaveChanges();
                        if (a > 0)
                        {
                            data.returnvalue = true;
                        }
                        else
                        {
                            data.returnvalue = false;
                        }
                    }
                    else
                    {
                        if (data.VTADACM_Id > 0)
                        {
                            data.returnval = "Update";
                            var duplicate = _context.VMS_TADA_CityMappingBDM.Where(p => p.MI_Id == data.MI_Id && p.HRMDES_Id == data.HRMDES_Id && p.IVRMMCT_Id == data.city_array[0].IVRMMCT_Id && p.VTADACM_Id != data.VTADACM_Id).ToList();
                            if (duplicate.Count == 0)
                            {
                                var obj = _context.VMS_TADA_CityMappingBDM.Where(t => t.VTADACM_Id == data.VTADACM_Id).Single();
                                obj.MI_Id = data.MI_Id;
                                obj.VTADACM_FoodAmt = data.VTADACM_FoodAmt;
                                obj.VTADACM_TransportAmt = data.VTADACM_TransportAmt;
                                obj.VTADACM_AccommodationAmt = data.VTADACM_AccommodationAmt;
                                obj.VTADACM_TravelHour = data.VTADACM_TravelHour;
                                obj.VTADACM_UpdatedDate = DateTime.Now;
                                obj.VTADACM_UpdatedBy = data.UserId;
                                _context.Update(obj);
                                int a = _context.SaveChanges();
                                if (a > 0)
                                {
                                    data.returnvalue = true;
                                }
                                else
                                {
                                    data.returnvalue = false;
                                }
                            }
                            else
                            {
                                data.returnval = "Duplicate";
                            }

                        }

                    }

                }
                else
                {
                    if (data.VTADACM_Id == 0)
                    {
                        data.message = "";
                        data.returnval = "Add";
                        for (int c = 0; c < data.city_array.Length; c++)
                        {
                            var duplicate = _context.TADA_CityMappingDMO.Where(p => p.MI_Id == data.MI_Id
                            && p.HRMDES_Id == data.HRMDES_Id
                            && p.IVRMMCT_Id == data.city_array[c].IVRMMCT_Id).ToList();
                            if (duplicate.Count == 0)
                            {
                                TADA_CityMappingDMO obj = new TADA_CityMappingDMO();
                                obj.MI_Id = data.MI_Id;
                                obj.HRMDES_Id = data.HRMDES_Id;
                                obj.IVRMMCT_Id = data.city_array[c].IVRMMCT_Id;
                                obj.VTADACM_FoodAmt = data.VTADACM_FoodAmt;
                                obj.VTADACM_TransportAmt = data.VTADACM_TransportAmt;
                                obj.VTADACM_AccommodationAmt = data.VTADACM_AccommodationAmt;
                                obj.VTADACM_TravelHour = data.VTADACM_TravelHour;
                                obj.VTADACM_ActiveFlag = true;
                                obj.VTADACM_CreatedDate = DateTime.Now;
                                obj.VTADACM_UpdatedDate = DateTime.Now;
                                obj.VTADACM_CreatedBy = data.UserId;
                                obj.VTADACM_UpdatedBy = data.UserId;
                                _context.Add(obj);
                            }
                            else
                            {
                                if (data.message != "")
                                {
                                    data.message = data.city_array[c].IVRMMCT_Name;
                                }
                                else
                                {
                                    data.message = data.message + ", " + data.city_array[c].IVRMMCT_Name;
                                }
                            }
                        }
                        int a = _context.SaveChanges();
                        if (a > 0)
                        {
                            data.returnvalue = true;
                        }
                        else
                        {
                            data.returnvalue = false;
                        }
                    }
                    else
                    {
                        if (data.VTADACM_Id > 0)
                        {
                            data.returnval = "Update";
                            var duplicate = _context.TADA_CityMappingDMO.Where(p => p.MI_Id == data.MI_Id && p.HRMDES_Id == data.HRMDES_Id && p.IVRMMCT_Id == data.city_array[0].IVRMMCT_Id && p.VTADACM_Id != data.VTADACM_Id).ToList();
                            if (duplicate.Count == 0)
                            {
                                var obj = _context.TADA_CityMappingDMO.Where(t => t.VTADACM_Id == data.VTADACM_Id).Single();
                                obj.MI_Id = data.MI_Id;
                                obj.VTADACM_FoodAmt = data.VTADACM_FoodAmt;
                                obj.VTADACM_TransportAmt = data.VTADACM_TransportAmt;
                                obj.VTADACM_AccommodationAmt = data.VTADACM_AccommodationAmt;
                                obj.VTADACM_TravelHour = data.VTADACM_TravelHour;
                                obj.VTADACM_UpdatedDate = DateTime.Now;
                                obj.VTADACM_UpdatedBy = data.UserId;
                                _context.Update(obj);
                                int a = _context.SaveChanges();
                                if (a > 0)
                                {
                                    data.returnvalue = true;
                                }
                                else
                                {
                                    data.returnvalue = false;
                                }
                            }
                            else
                            {
                                data.returnval = "Duplicate";
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

        public TRDACityMappingDTO edit(TRDACityMappingDTO data)
        {
            try
            {
                if(data.message== "BDM")
                {
                    data.editfiles = (from a in _context.VMS_TADA_CityMappingBDM
                                      from b in _context.HR_Master_Designation
                                      from c in _context.IVRM_Master_CityDMO
                                      where (a.HRMDES_Id == b.HRMDES_Id && a.IVRMMCT_Id == c.IVRMMCT_Id && a.VTADACM_Id == data.VTADACM_Id)
                                      select new TRDACityMappingDTO
                                      {
                                          HRMDES_DesignationName = b.HRMDES_DesignationName,
                                          IVRMMCT_Name = c.IVRMMCT_Name,
                                          VTADACM_FoodAmt = a.VTADACM_FoodAmt,
                                          VTADACM_TransportAmt = a.VTADACM_TransportAmt,
                                          VTADACM_TravelHour = a.VTADACM_TravelHour,
                                          VTADACM_AccommodationAmt = a.VTADACM_AccommodationAmt,
                                          VTADACM_Id = a.VTADACM_Id,
                                          HRMDES_Id = a.HRMDES_Id,
                                          IVRMMC_Id = a.IVRMMCT_Id,
                                          VTADACM_ActiveFlag = data.VTADACM_ActiveFlag
                                      }

                                  ).Distinct().ToArray();
                }
                else
                {
                    data.editfiles = (from a in _context.TADA_CityMappingDMO
                                      from b in _context.HR_Master_Designation
                                      from c in _context.IVRM_Master_CityDMO
                                      where (a.HRMDES_Id == b.HRMDES_Id && a.IVRMMCT_Id == c.IVRMMCT_Id && a.VTADACM_Id == data.VTADACM_Id)
                                      select new TRDACityMappingDTO
                                      {
                                          HRMDES_DesignationName = b.HRMDES_DesignationName,
                                          IVRMMCT_Name = c.IVRMMCT_Name,
                                          VTADACM_FoodAmt = a.VTADACM_FoodAmt,
                                          VTADACM_TransportAmt = a.VTADACM_TransportAmt,
                                          VTADACM_TravelHour = a.VTADACM_TravelHour,
                                          VTADACM_AccommodationAmt = a.VTADACM_AccommodationAmt,
                                          VTADACM_Id = a.VTADACM_Id,
                                          HRMDES_Id = a.HRMDES_Id,

                                          IVRMMC_Id = a.IVRMMCT_Id,

                                          VTADACM_ActiveFlag = data.VTADACM_ActiveFlag

                                      }

                                  ).Distinct().ToArray();
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }               //deactive
        public TRDACityMappingDTO deactive(TRDACityMappingDTO data)
        {
            try
            {
                if (data.message == "BDM")
                {
                    var result = _context.TADA_CityMappingDMO.Single(t => t.VTADACM_Id == data.VTADACM_Id);
                    if (result.VTADACM_ActiveFlag == true)
                    {
                        result.VTADACM_ActiveFlag = false;
                    }
                    else if (result.VTADACM_ActiveFlag == false)
                    {
                        result.VTADACM_ActiveFlag = true;
                    }
                    result.VTADACM_UpdatedDate = DateTime.Now;
                    result.VTADACM_UpdatedBy = data.UserId;
                    _context.Update(result);

                    int returnval = _context.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnvalue = true;
                    }
                    else
                    {
                        data.returnvalue = false;
                    }
                }
                else
                {
                    var result = _context.TADA_CityMappingDMO.Single(t => t.VTADACM_Id == data.VTADACM_Id);
                    if (result.VTADACM_ActiveFlag == true)
                    {
                        result.VTADACM_ActiveFlag = false;
                    }
                    else if (result.VTADACM_ActiveFlag == false)
                    {
                        result.VTADACM_ActiveFlag = true;
                    }
                    result.VTADACM_UpdatedDate = DateTime.Now;
                    result.VTADACM_UpdatedBy = data.UserId;
                    _context.Update(result);

                    int returnval = _context.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnvalue = true;
                    }
                    else
                    {
                        data.returnvalue = false;
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
