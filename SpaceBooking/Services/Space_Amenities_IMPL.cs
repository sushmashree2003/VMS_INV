using DataAccessMsSqlServerProvider.com.vapstech.SpaceBooking;
using DomainModel.Model.com.vapstech.SpaceBooking;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBooking.Services
{
    public class Space_Amenities_IMPL:Interfaces.Space_Amenities_Interface
    {
        public SpaceBookingContext _VMSContext;
        public Space_Amenities_IMPL(SpaceBookingContext VMSContext)
        {
            _VMSContext = VMSContext;
        }
        

        public SpaceBookingDTO get_master_amenities(SpaceBookingDTO dto)
        {
            try
            {
                dto.master_amenities_list = _VMSContext.HR_Master_Amenities_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();

            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
       public SpaceBookingDTO SaveEdit_MasterAmenities(SpaceBookingDTO dto)
        {
            try
            {
                if (dto.HRMAM_Id>0)
                    {
                    var dm= _VMSContext.HR_Master_Amenities_DMO_con.Single(a => a.MI_Id == dto.MI_Id && a.HRMAM_Id==dto.HRMAM_Id);
                    dm.HRMAM_AmenitiesName = dto.HRMAM_AmenitiesName;
                    dm.HRMAM_AmenitiesDes = dto.HRMAM_AmenitiesDes;
                    dm.HRMAM_Price = dto.HRMAM_Price;
                    dm.HRMAM_PriceApplFlg = dto.HRMAM_PriceApplFlg;
                    dm.HRMAM_UpdatedBy = dto.userId;
                    dm.HRMAM_UpdatedDate = DateTime.Now;
                    _VMSContext.Update(dm);
                    _VMSContext.SaveChanges();
                     dto.returnvalue = "Update";

                }
                else
                {
                    HR_Master_Amenities_DMO dm = new HR_Master_Amenities_DMO();
                    dm.HRMAM_AmenitiesName = dto.HRMAM_AmenitiesName;
                    dm.MI_Id = dto.MI_Id;
                    dm.HRMAM_AmenitiesDes = dto.HRMAM_AmenitiesDes;
                    dm.HRMAM_CreatedBy = dto.userId;
                    dm.HRMAM_CreatedDate = DateTime.Now;
                    dm.HRMAM_Price = dto.HRMAM_Price;
                    dm.HRMAM_PriceApplFlg = dto.HRMAM_PriceApplFlg;
                    dm.HRMAM_ActiveFlag = true;
                    _VMSContext.Add(dm);
                    _VMSContext.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public SpaceBookingDTO Edit_MasterAmenities(SpaceBookingDTO dto)
        {
            try
            {
              dto.master_amenities_details = _VMSContext.HR_Master_Amenities_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMAM_Id == dto.HRMAM_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SpaceBookingDTO deactivate_MasterAmenities(SpaceBookingDTO dto)
        {
            try
            {
                if(dto.HRMAM_ActiveFlag==true)
                {
                    var dm = _VMSContext.HR_Master_Amenities_DMO_con.Single(a => a.MI_Id == dto.MI_Id && a.HRMAM_Id == dto.HRMAM_Id);
                    dm.HRMAM_ActiveFlag = false;
                    dm.HRMAM_UpdatedBy = dto.userId;
                    dm.HRMAM_UpdatedDate = DateTime.Now;
                    _VMSContext.Update(dm);
                    _VMSContext.SaveChanges();
                    dto.returnvalue = "false";
                }
                else
                {
                    var dm = _VMSContext.HR_Master_Amenities_DMO_con.Single(a => a.MI_Id == dto.MI_Id && a.HRMAM_Id == dto.HRMAM_Id);
                    dm.HRMAM_ActiveFlag = true;
                    dm.HRMAM_UpdatedBy = dto.userId;
                    dm.HRMAM_UpdatedDate = DateTime.Now;
                    _VMSContext.Update(dm);
                    _VMSContext.SaveChanges();
                    dto.returnvalue = "true";
                }
               

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
