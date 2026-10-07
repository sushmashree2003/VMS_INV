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
    public class VenueMappingImpl:Interfaces.VenueMappingInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public VenueMappingImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public VenueMapping_DTO getdetails(VenueMapping_DTO dTO)
        {
            try
            {
                dTO.venuelist = _IssueContext.MasterVenues_DMO.Where(t => t.MI_Id == dTO.MI_Id && t.ISMMVNE_ActiveFlag == true).Distinct().ToArray();

                dTO.alldata = (from a in _IssueContext.ISM_Master_MeetingRoom_DMO
                               from b in _IssueContext.MasterVenues_DMO
                               where (a.MI_Id == b.MI_Id && a.ISMMVNE_Id == b.ISMMVNE_Id && b.ISMMVNE_ActiveFlag == true && a.MI_Id==dTO.MI_Id)
                               select new VenueMapping_DTO {
                                   ISMMMER_MeetingRoomName=a.ISMMMER_MeetingRoomName,
                                   ISMMMER_FloorName=a.ISMMMER_FloorName,
                                   ISMMVNE_Id=a.ISMMVNE_Id,
                                   ISMMMER_Id=a.ISMMMER_Id,
                                   ISMMMER_ActiveFlag=a.ISMMMER_ActiveFlag,
                                   ISMMMER_Facilities=a.ISMMMER_Facilities,
                                   ISMMVNE_VenueName = b.ISMMVNE_VenueName,
                                   ISMMVNE_VenueAddress=b.ISMMVNE_VenueAddress

                               }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }

        public VenueMapping_DTO saverecord(VenueMapping_DTO data)
        {
            try
            {
                if (data.ISMMMER_Id == 0)
                {
                    var duplicate = _IssueContext.ISM_Master_MeetingRoom_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMVNE_Id==data.ISMMVNE_Id && t.ISMMMER_MeetingRoomName == data.ISMMMER_MeetingRoomName && t.ISMMMER_FloorName==data.ISMMMER_FloorName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        ISM_Master_MeetingRoom_DMO obj = new ISM_Master_MeetingRoom_DMO();

                        //obj.ISMMMER_Id = data.ISMMMER_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.ISMMVNE_Id = data.ISMMVNE_Id;
                        obj.ISMMMER_MeetingRoomName = data.ISMMMER_MeetingRoomName;
                        obj.ISMMMER_FloorName = data.ISMMMER_FloorName;
                        obj.ISMMMER_Facilities = data.ISMMMER_Facilities;
                        obj.ISMMMER_ActiveFlag = true;
                        obj.ISMMMER_CreatedBy = data.UserId;
                        obj.ISMMMER_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
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
                    var duplicate = _IssueContext.ISM_Master_MeetingRoom_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMMER_Id != data.ISMMMER_Id && t.ISMMVNE_Id == data.ISMMVNE_Id && t.ISMMMER_MeetingRoomName == data.ISMMMER_MeetingRoomName && t.ISMMMER_FloorName == data.ISMMMER_FloorName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var obj = _IssueContext.ISM_Master_MeetingRoom_DMO.Where(t => t.ISMMMER_Id == data.ISMMMER_Id).Single();

                        obj.ISMMVNE_Id = data.ISMMVNE_Id;
                        obj.ISMMMER_MeetingRoomName = data.ISMMMER_MeetingRoomName;
                        obj.ISMMMER_FloorName = data.ISMMMER_FloorName;
                        obj.ISMMMER_Facilities = data.ISMMMER_Facilities;
                        obj.ISMMMER_UpdatedBy = data.UserId;
                        obj.UpdatedDate = DateTime.Now;

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

        public VenueMapping_DTO deactiveY(VenueMapping_DTO data)
        {
            try
            {
                var result = _IssueContext.ISM_Master_MeetingRoom_DMO.Single(t => t.ISMMMER_Id == data.ISMMMER_Id);
                if (result.ISMMMER_ActiveFlag == true)
                {
                    result.ISMMMER_ActiveFlag = false;
                }
                else
                {
                    result.ISMMMER_ActiveFlag = true;
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
