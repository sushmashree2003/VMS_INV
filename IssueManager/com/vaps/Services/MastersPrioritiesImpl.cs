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
    public class MastersPrioritiesImpl:Interfaces.MastersPrioritiesInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public MastersPrioritiesImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public MastersPriorities_DTO getdetails(MastersPriorities_DTO dTO)
        {
            try
            {
                dTO.alldata = _IssueContext.MastersPriorities_DMO.Where(t => t.MI_Id == dTO.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }

        public MastersPriorities_DTO saverecord(MastersPriorities_DTO data)
        {
            try
            {
                if (data.HRMPR_Id == 0)
                {
                    var duplicate = _IssueContext.MastersPriorities_DMO.Where(t => (t.MI_Id == data.MI_Id && t.HRMP_Name == data.HRMP_Name && t.HRMP_Order == data.HRMP_Order) || (t.MI_Id == data.MI_Id && t.HRMP_Order == data.HRMP_Order)).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MastersPriorities_DMO obj = new MastersPriorities_DMO();
                        // obj.HRMPR_Id = data.HRMPR_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HRMP_Name = data.HRMP_Name;
                        obj.HRMP_Order = data.HRMP_Order;
                        obj.HRMP_CreatedBy = data.UserId;
                        obj.HRMP_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.HRMP_ActiveFlag = true;

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
                   // var duplicate = _IssueContext.MastersPriorities_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMPR_Id != data.HRMPR_Id && t.HRMP_Name == data.HRMP_Name || t.HRMP_Order==data.HRMP_Order).ToList();


                    var duplicate = _IssueContext.MastersPriorities_DMO.Where(t =>  (t.MI_Id == data.MI_Id && t.HRMPR_Id != data.HRMPR_Id && t.HRMP_Name == data.HRMP_Name &&  t.HRMP_Order == data.HRMP_Order) || (t.MI_Id == data.MI_Id &&  t.HRMPR_Id != data.HRMPR_Id && t.HRMP_Order == data.HRMP_Order)).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var obj = _IssueContext.MastersPriorities_DMO.Where(t => t.HRMPR_Id == data.HRMPR_Id).Single();

                        obj.HRMP_Name = data.HRMP_Name;
                        obj.HRMP_Order = data.HRMP_Order;
                        obj.UpdatedDate = DateTime.Now;
                        obj.HRMP_UpdatedBy = data.UserId;
                       
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

        public MastersPriorities_DTO deactiveY(MastersPriorities_DTO data)
        {
            try
            {
                var result = _IssueContext.MastersPriorities_DMO.Single(t => t.HRMPR_Id == data.HRMPR_Id);
                if (result.HRMP_ActiveFlag == true)
                {
                    result.HRMP_ActiveFlag = false;
                }
                else
                {
                    result.HRMP_ActiveFlag = true;
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
