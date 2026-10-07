using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Services
{
    public class MasterDayIMPL : Interfaces.MasterDayInterface
    {

        public FOContext _FOContext;

        public MasterDayIMPL(FOContext ttcntx)
        {
            _FOContext = ttcntx;

        }
        public MasterDayDTO getalldetails(MasterDayDTO data)
        {
            try
            {
                data.getarray = _FOContext.FODayNameDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;


        }


        public MasterDayDTO savedetails(MasterDayDTO data)
        {
            try
            {
                if (data.FOMD_Id > 0)
                {
                    var duplicatecheck = _FOContext.FODayNameDMO.Where(x => x.FOMD_DayCode == data.FOMD_DayCode && x.FOMD_DayName == data.FOMD_DayName).ToList();
                    if (duplicatecheck.Count() > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {
                        var result = _FOContext.FODayNameDMO.Single(t => t.FOMD_Id == data.FOMD_Id);
                        result.FOMD_DayName = data.FOMD_DayName;
                        result.FOMD_DayCode = data.FOMD_DayCode;
                        result.UpdatedDate = DateTime.Now;
                      //  result.FOMD_UpdatedBy = data.userid;
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
                }

                else
                {

                    var duplicatecheck = _FOContext.FODayNameDMO.Where(x => x.FOMD_DayCode == data.FOMD_DayCode && x.FOMD_DayName == data.FOMD_DayName).ToList();
                    if (duplicatecheck.Count() > 0)
                    {
                        data.returnmsg = "Duplicate";
                    }
                    else
                    {


                        FODayNameDMO obj = new FODayNameDMO();
                        obj.FOMD_DayName = data.FOMD_DayName;
                        obj.FOMD_DayCode = data.FOMD_DayCode;
                        obj.MI_Id = data.MI_Id;
                        obj.FOMD_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        //obj.FOMD_CreatedBy = data.userid;
                       // obj.FOMD_UpdatedBy = data.userid;
                        _FOContext.Add(obj);

                        var count = _FOContext.SaveChanges();

                        if (count > 0)
                        {
                            data.returnmsg = "Saved";
                        }
                        else
                        {
                            data.returnmsg = "Failed";

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




        public MasterDayDTO deactive(MasterDayDTO data)
        {
            try
            {
                var result = _FOContext.FODayNameDMO.Single(t => t.FOMD_Id == data.FOMD_Id);

                if (result.FOMD_ActiveFlag == true)
                {
                    result.FOMD_ActiveFlag = false;
                }
                else if (result.FOMD_ActiveFlag == false)
                {
                    result.FOMD_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _FOContext.Update(result);
                int returnval = _FOContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        
   

    }
}
