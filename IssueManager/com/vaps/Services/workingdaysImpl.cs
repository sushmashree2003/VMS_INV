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
    public class workingdaysImpl : Interfaces.workingdaysInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public workingdaysImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public workingdays_DTO getdetails(workingdays_DTO dTO)
        {
            try
            {
                dTO.daylist = _IssueContext.masterday.Where(t => t.MI_Id == dTO.MI_Id && t.FOMD_ActiveFlag == true).Distinct().ToArray();

                dTO.alldata = (from a in _IssueContext.workingDays_DMO
                               from b in _IssueContext.masterday
                               where (a.MI_Id == b.MI_Id && a.FOMD_Id == b.FOMD_Id && b.FOMD_ActiveFlag == true && a.MI_Id == dTO.MI_Id)
                               select new workingdays_DTO
                               {
                                   ISMMWD_Id = a.ISMMWD_Id,
                                   FOMD_Id = a.FOMD_Id,
                                   ISMMWD_Hours = a.ISMMWD_Hours,
                                   ISMPRTY_ActiveFlag = a.ISMPRTY_ActiveFlag,
                                   FOMD_DayName = b.FOMD_DayName
                               }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }

        public workingdays_DTO saverecord(workingdays_DTO data)
        {
            try
            {
                if (data.ISMMWD_Id == 0)
                {
                    foreach (var hour in data.daylisdata)
                    {
                        workingDays_DMO obj = new workingDays_DMO();

                        //obj.ISMMWD_Id = data.ISMMWD_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.FOMD_Id = hour.FOMD_Id;
                        obj.ISMMWD_Hours = data.ISMMWD_Hours;
                        obj.ISMPRTY_ActiveFlag = true;
                        obj.ISMMWD_CreatedBy = data.UserId;
                        obj.ISMMWD_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _IssueContext.Add(obj);
                    }

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
                else
                {

                    var obj = _IssueContext.workingDays_DMO.Where(t => t.ISMMWD_Id == data.ISMMWD_Id).Single();
                    foreach (var hour in data.daylisdata)
                    {

                        obj.FOMD_Id = hour.FOMD_Id;
                        obj.ISMMWD_Hours = data.ISMMWD_Hours;
                        obj.ISMMWD_UpdatedBy = data.UserId;
                        obj.UpdatedDate = DateTime.Now;

                        _IssueContext.Update(obj);
                    }
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public workingdays_DTO deactiveY(workingdays_DTO data)
        {
            try
            {
                var result = _IssueContext.workingDays_DMO.Single(t => t.ISMMWD_Id == data.ISMMWD_Id);
                if (result.ISMPRTY_ActiveFlag == true)
                {
                    result.ISMPRTY_ActiveFlag = false;
                }
                else
                {
                    result.ISMPRTY_ActiveFlag = true;
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

        public workingdays_DTO editrecord(workingdays_DTO data)
        {
            try
            {
                var result = _IssueContext.workingDays_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMWD_Id == data.ISMMWD_Id).ToList();

                data.editlist = result.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
