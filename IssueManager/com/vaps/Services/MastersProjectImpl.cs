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
    public class MastersProjectImpl:Interfaces.MastersProjectInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public MastersProjectImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }
        public MastersProject_DTO getdetails(MastersProject_DTO dTO)
        {
            try
            {
                dTO.alldata = _IssueContext.MastersProject_DMO.Where(t => t.MI_Id == dTO.MI_Id).Distinct().ToArray();
                dTO.getinstitution = _IssueContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }
        public MastersProject_DTO OnChangeInstitution(MastersProject_DTO dTO)
        {
            try
            {
                dTO.alldata = _IssueContext.MastersProject_DMO.Where(t => t.MI_Id == dTO.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }
        public MastersProject_DTO saverecord(MastersProject_DTO data)
        {
            try
            {
                if (data.ISMMPR_Id == 0)
                {
                    var duplicate = _IssueContext.MastersProject_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMPR_ProjectName == data.ISMMPR_ProjectName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MastersProject_DMO obj = new MastersProject_DMO();
                        // obj.ISMMPR_Id = data.ISMMPR_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.ISMMPR_ProjectName = data.ISMMPR_ProjectName;
                        obj.ISMMPR_Desc = data.ISMMPR_Desc;
                        obj.ISMMPR_InternalProjectFlg = data.ISMMPR_InternalProjectFlg;
                      
                        obj.ISMMPR_CreatedBy = data.UserId;
                        obj.ISMMPR_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMPR_ActiveFlg = true;

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
                    var duplicate = _IssueContext.MastersProject_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMPR_Id != data.ISMMPR_Id && t.ISMMPR_ProjectName == data.ISMMPR_ProjectName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var obj = _IssueContext.MastersProject_DMO.Where(t => t.ISMMPR_Id == data.ISMMPR_Id).Single();

                        obj.ISMMPR_ProjectName = data.ISMMPR_ProjectName;
                        obj.ISMMPR_Desc = data.ISMMPR_Desc;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMPR_UpdatedBy = data.UserId;
                        obj.ISMMPR_InternalProjectFlg = data.ISMMPR_InternalProjectFlg;
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
        public MastersProject_DTO deactiveY(MastersProject_DTO data)
        {
            try
            {
                var result = _IssueContext.MastersProject_DMO.Single(t => t.ISMMPR_Id == data.ISMMPR_Id);
                if (result.ISMMPR_ActiveFlg == true)
                {
                    result.ISMMPR_ActiveFlg = false;
                }
                else
                {
                    result.ISMMPR_ActiveFlg = true;
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
