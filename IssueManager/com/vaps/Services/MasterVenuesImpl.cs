using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class MasterVenuesImpl : Interfaces.MasterVenuesInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public MasterVenuesImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public MasterVenues_DTO getdetails(MasterVenues_DTO dTO)
        {
            try
            {
                dTO.alldata = _IssueContext.MasterVenues_DMO.Where(t => t.MI_Id == dTO.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }

        public MasterVenues_DTO saverecord(MasterVenues_DTO data)
        {
            try
            {
                if (data.ISMMVNE_Id == 0)
                {
                    var duplicate = _IssueContext.MasterVenues_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMVNE_VenueName == data.ISMMVNE_VenueName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterVenues_DMO obj = new MasterVenues_DMO();
                        // obj.ISMMVNE_Id = data.ISMMVNE_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.ISMMVNE_VenueName = data.ISMMVNE_VenueName;
                        obj.ISMMVNE_VenueAddress = data.ISMMVNE_VenueAddress;
                        obj.ISMMVNE_CreatedBy = data.UserId;
                        obj.ISMMVNE_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMVNE_ActiveFlag = true;

                        _IssueContext.Add(obj);
                        int a = _IssueContext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var duplicate = _IssueContext.MasterVenues_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMVNE_Id != data.ISMMVNE_Id && t.ISMMVNE_VenueName == data.ISMMVNE_VenueName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var obj = _IssueContext.MasterVenues_DMO.Where(t => t.ISMMVNE_Id == data.ISMMVNE_Id).Single();

                        obj.ISMMVNE_VenueName = data.ISMMVNE_VenueName;
                        obj.ISMMVNE_VenueAddress = data.ISMMVNE_VenueAddress;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMVNE_UpdatedBy = data.UserId;

                        _IssueContext.Update(obj);
                        int a = _IssueContext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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

        public MasterVenues_DTO deactiveY(MasterVenues_DTO data)
        {
            try
            {
                var result = _IssueContext.MasterVenues_DMO.Single(t => t.ISMMVNE_Id == data.ISMMVNE_Id);
                if (result.ISMMVNE_ActiveFlag == true)
                {
                    result.ISMMVNE_ActiveFlag = false;
                }
                else
                {
                    result.ISMMVNE_ActiveFlag = true;
                }

                result.UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);
                int a = _IssueContext.SaveChanges();
                if (a > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
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

