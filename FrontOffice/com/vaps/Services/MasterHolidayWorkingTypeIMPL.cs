using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Services
{
    public class MasterHolidayWorkingTypeIMPL : Interfaces.MasterHolidayWorkingTypeInterface
    {
        public FOContext _FOContext;

        public MasterHolidayWorkingTypeIMPL(FOContext ttcntx)
        {
            _FOContext = ttcntx;

        }

        public MasterHolidayWorkingTypeDTO getdata(MasterHolidayWorkingTypeDTO data)
        {
            try
            {
                data.getarray = _FOContext.holidayWorkingDayType.Where(t => t.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterHolidayWorkingTypeDTO savedetails(MasterHolidayWorkingTypeDTO data)
        {
            try
            {
                if(data.FOHWDT_Id>0)
                {
                    var result = _FOContext.holidayWorkingDayType.Single(t => t.FOHWDT_Id == data.FOHWDT_Id);
                    result.FOHTWD_HolidayWDType = data.FOHTWD_HolidayWDType;
                    result.FOHTWD_HolidayWDTypeFlag = data.FOHTWD_HolidayWDTypeFlag;
                    result.FOHTWD_HolidayFlag = data.FOHTWD_HolidayFlag;
                    result.UpdatedDate = DateTime.Now;
                    result.FOHWDT_UpdatedBy = data.userid;
                    _FOContext.Update(result);
                    var Count = _FOContext.SaveChanges();
                    if (Count > 0)
                    {
                        data.returnmsg = "updated";
                    }
                    else
                    {
                        data.returnmsg = "notupdated";
                    }
                }
                else
                {
                    HolidayWorkingDayTypeDMO Obj = new HolidayWorkingDayTypeDMO();
                    Obj.FOHTWD_HolidayWDType = data.FOHTWD_HolidayWDType;
                    Obj.FOHTWD_HolidayWDTypeFlag = data.FOHTWD_HolidayWDTypeFlag;
                    Obj.FOHTWD_HolidayFlag = data.FOHTWD_HolidayFlag;
                    Obj.MI_Id = data.MI_Id;
                    Obj.FOHWDT_ActiveFlg = true;
                    Obj.CreatedDate = DateTime.Now;
                    Obj.UpdatedDate = DateTime.Now;
                    Obj.FOHWDT_CreatedBy = data.userid;
                    Obj.FOHWDT_UpdatedBy = data.userid;
                    _FOContext.Add(Obj);

                    var Count = _FOContext.SaveChanges();

                    if (Count > 0)
                    {
                        data.returnmsg = "Saved";
                    }
                    else
                    {
                        data.returnmsg = "Failed";
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
