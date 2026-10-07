using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Portals.Student;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Portals.Employee;
using AutoMapper;

namespace PortalHub.com.vaps.Employee.Services
{
    public class NoticeBoardImpl : Interfaces.NoticeBoardInterface
    {
        public PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _db;
        //public DomainModelMsSqlServerContext _db;
        public NoticeBoardImpl(PortalContext pContext, DomainModelMsSqlServerContext db)
        {
            _PortalContext = pContext;
            _db = db;
        }

        public IVRM_NoticeBoardDTO Getdetails(IVRM_NoticeBoardDTO data)
        {
            try
            {
                var notice = _PortalContext.IVRM_NoticeBoardDMO.Where(n => n.MI_Id == data.MI_Id).OrderBy(n => n.INTB_Id).Distinct().ToArray();
                if (notice.Length > 0)
                {
                    data.notice_details = notice;
                }

                var clsslst = _PortalContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                if (clsslst.Length > 0)
                {
                    data.classlist = clsslst;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRM_NoticeBoardDTO savedetail(IVRM_NoticeBoardDTO data)
        {
            try
            {
                if (data.INTB_Id > 0)
                {
                    var ChkDuplicate = _PortalContext.IVRM_NoticeBoardDMO.Where(t => t.INTB_Title.Equals(data.INTB_Title) && t.INTB_Attachment.Equals(data.INTB_Attachment) && t.MI_Id.Equals(data.MI_Id) && t.INTB_Description.Equals(data.INTB_Description) && t.INTB_StartDate.Equals(data.INTB_StartDate) && t.INTB_EndDate.Equals(data.INTB_EndDate) && t.INTB_Id != data.INTB_Id).ToList();
                    if (ChkDuplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var resultobj = _PortalContext.IVRM_NoticeBoardDMO.Single(t => t.INTB_Id.Equals(data.INTB_Id) && t.MI_Id.Equals(data.MI_Id));

                        resultobj.MI_Id = data.MI_Id;
                        resultobj.INTB_Title = data.INTB_Title;
                        resultobj.INTB_Description = data.INTB_Description;
                        resultobj.INTB_StartDate = data.INTB_StartDate;
                        resultobj.INTB_EndDate = data.INTB_EndDate;
                        resultobj.INTB_DisplayDate = data.INTB_DisplayDate;
                        resultobj.INTB_FilePath = data.INTB_FilePath;
                        resultobj.INTB_Attachment = data.INTB_Attachment;
                        resultobj.INTB_DispalyDisableFlg = data.INTB_DispalyDisableFlg;
                        resultobj.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(resultobj);

                        var resultclass = _PortalContext.IVRM_NoticeBoard_Class_DMO.Where(t => t.INTB_Id == data.INTB_Id).ToList();
                        if (resultclass.Count>0)
                        {
                            foreach (var item in resultclass)
                            {
                                _PortalContext.Remove(item);
                            }
                        }

                        foreach (var ss in data.classlst)
                        {
                            IVRM_NoticeBoard_Class_DMO obj2 = new IVRM_NoticeBoard_Class_DMO();
                            obj2.INTB_Id = data.INTB_Id;
                            obj2.ASMCL_Id = ss.ASMCL_Id;
                            obj2.UpdatedDate = DateTime.Now;
                            obj2.CreatedDate=DateTime.Now;
                            obj2.INTBC_ActiveFlag = true;

                            _PortalContext.Add(obj2);
                        }

                        _PortalContext.Update(resultobj);
                        int returnval = _PortalContext.SaveChanges();
                        if (returnval >0)
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
                    var result = _PortalContext.IVRM_NoticeBoardDMO.Where(t => t.INTB_Title.Equals(data.INTB_Title) && t.INTB_StartDate.Equals(data.INTB_StartDate) && t.INTB_EndDate.Equals(data.INTB_EndDate) && t.INTB_Description.Equals(data.INTB_Description) && t.INTB_Attachment.Equals(data.INTB_Attachment) && t.MI_Id.Equals(data.MI_Id)).ToList();
                    if (result.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        IVRM_NoticeBoardDMO obj = new IVRM_NoticeBoardDMO();

                        obj.MI_Id = data.MI_Id;
                        obj.INTB_Title = data.INTB_Title;
                        obj.INTB_Description = data.INTB_Description;
                        obj.INTB_StartDate = data.INTB_StartDate;
                        obj.INTB_EndDate = data.INTB_EndDate;
                        obj.INTB_DisplayDate = data.INTB_DisplayDate;
                        obj.INTB_Attachment = data.INTB_Attachment;
                        obj.INTB_FilePath = data.INTB_FilePath;
                        obj.NTB_TTSylabusFlg = data.NTB_TTSylabusFlg;
                        obj.INTB_DispalyDisableFlg = data.INTB_DispalyDisableFlg;
                        obj.INTB_ActiveFlag = true;
                        obj.UpdatedDate = DateTime.Now;
                        obj.CreatedDate = DateTime.Now;

                        _PortalContext.Add(obj);

                        foreach(var ss in data.classlst)
                        {
                            IVRM_NoticeBoard_Class_DMO obj2 = new IVRM_NoticeBoard_Class_DMO();
                            obj2.INTB_Id = obj.INTB_Id;
                            obj2.ASMCL_Id = ss.ASMCL_Id;
                            obj2.INTBC_ActiveFlag = true;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.UpdatedDate = DateTime.Now;

                            _PortalContext.Add(obj2);
                        }

                        int returnval=_PortalContext.SaveChanges();

                        if (returnval > 0 )
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

        public IVRM_NoticeBoardDTO editdetails(IVRM_NoticeBoardDTO data)
        {
            try
            {
                var edit = (from a in _PortalContext.IVRM_NoticeBoardDMO
                            from b in _PortalContext.IVRM_NoticeBoard_Class_DMO
                            where (a.INTB_Id==data.INTB_Id && a.MI_Id==data.MI_Id && a.INTB_Id==b.INTB_Id) select new IVRM_NoticeBoardDTO
                            {
                                INTB_Id = a.INTB_Id,
                                INTB_Title = a.INTB_Title,
                              INTB_Description = a.INTB_Description,
                              INTB_StartDate = a.INTB_StartDate,
                              INTB_EndDate = a.INTB_EndDate,
                              INTB_DisplayDate = a.INTB_DisplayDate,
                              INTB_Attachment = a.INTB_Attachment,
                              INTB_FilePath = a.INTB_FilePath,
                              NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                              INTB_DispalyDisableFlg = a.INTB_DispalyDisableFlg,
                                // INTBC_Id = b.INTBC_Id,
                                // INTB_Id = b.INTB_Id,

                                ASMCL_Id = b.ASMCL_Id,
                            

            }).Distinct().OrderBy(t => t.INTB_Id).ToArray();
                if(edit.Length>0)
                {
                    data.editdetails = edit;
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_NoticeBoardDTO deactivate(IVRM_NoticeBoardDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_NoticeBoardDMO.Single(s => s.MI_Id == data.MI_Id && s.INTB_Id == data.INTB_Id);

                if (query.INTB_ActiveFlag == true)
                {
                    query.INTB_ActiveFlag = false;
                }
                else
                {
                    query.INTB_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _PortalContext.Update(query);
                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
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
