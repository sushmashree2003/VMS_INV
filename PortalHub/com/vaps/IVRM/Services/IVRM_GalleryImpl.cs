using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Portals.Student;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.admission;

namespace PortalHub.com.vaps.IVRM.Services
{
    public class IVRM_GalleryImpl : Interfaces.IVRM_GalleryInterface
    {
        private static ConcurrentDictionary<string, IVRM_GalleryDTO> _login =
           new ConcurrentDictionary<string, IVRM_GalleryDTO>();
        private PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _context;
        public ClgAdmissionContext _ClgAdmissionContext;

        public IVRM_GalleryImpl(PortalContext PortalContext, DomainModelMsSqlServerContext context, ClgAdmissionContext clgAdmissionContext)
        {
            _PortalContext = PortalContext;
            _context = context;
            _ClgAdmissionContext = clgAdmissionContext;
        }



        public IVRM_GalleryDTO getloaddata(IVRM_GalleryDTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.get_galleryimg = _PortalContext.IVRM_GalleryDMO.Where(g => g.MI_Id == data.MI_Id && g.IGA_ActiveFlag == true).ToArray();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    data.classlist = (from a in _PortalContext.School_M_Class
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      from c in _PortalContext.AcademicYearDMO
                                      from d in _PortalContext.School_M_Section
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.AMST_Id == data.AMST_Id)
                                      select new IVRM_GalleryDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMS_Id = b.ASMS_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }
                else
                {
                    data.classlist = (from a in _PortalContext.School_M_Class
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      from c in _PortalContext.AcademicYearDMO
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true)
                                      select new IVRM_GalleryDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,
                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_GalleryDTO get_section(IVRM_GalleryDTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.get_galleryimg = _PortalContext.IVRM_GalleryDMO.Where(g => g.MI_Id == data.MI_Id && g.IGA_ActiveFlag == true).ToArray();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.sectionlist = (from a in _PortalContext.School_M_Class
                                        from b in _PortalContext.School_Adm_Y_StudentDMO
                                        from c in _PortalContext.AcademicYearDMO
                                        from d in _PortalContext.School_M_Section
                                        where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.ASMCL_ActiveFlag == true)
                                        select new IVRM_GalleryDTO
                                        {
                                            ASMCL_Id = b.ASMCL_Id,
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = d.ASMC_SectionName,
                                            ASMC_Order = d.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_GalleryDTO savedata(IVRM_GalleryDTO data)
        {
            try
            {
                IVRM_GalleryDMO img = new IVRM_GalleryDMO();
                img.MI_Id = data.MI_Id;
                img.IGA_GalleryName = data.IGA_GalleryName;
                img.IGA_Date = data.IGA_Date;
                img.IGA_CreatedBy = data.UserId;
                img.IGA_UpdatedBy = data.UserId;
                img.IGA_Time = data.IGA_Time;
                img.IGA_CommonGalleryFlg = data.IGA_CommonGalleryFlg;
                img.IGA_ActiveFlag = true;
                img.CreatedDate = DateTime.Now;
                img.UpdatedDate = DateTime.Now;
                _PortalContext.Add(img);

                if (data.images_list.Length > 0)
                {
                    foreach (var ig in data.images_list)
                    {
                        IVRM_Gallery_PhotosDMO imgfile = new IVRM_Gallery_PhotosDMO();
                        imgfile.IGA_Id = img.IGA_Id;
                        imgfile.IGAP_Photos = ig;
                        imgfile.IGAP_CoverPhotoFlag = false;
                        imgfile.IGAP_ActiveFlag = true;
                        imgfile.CreatedDate = DateTime.Now;
                        imgfile.UpdatedDate = DateTime.Now;
                        _PortalContext.Add(imgfile);
                    }
                }
                if (data.arraySection.Length > 0)
                {
                    foreach (var igcls in data.arraySection)
                    {
                        IVRM_Gallery_ClassDMO imgcls = new IVRM_Gallery_ClassDMO();
                        imgcls.IGA_Id = img.IGA_Id;
                        imgcls.ASMCL_Id = data.ASMCL_Id;
                        imgcls.ASMS_Id = igcls.ASMS_Id;
                        imgcls.IGACL_ActiveFlag = true;
                        imgcls.IGACL_CreatedBy = data.UserId;
                        imgcls.IGACL_UpdatedBy = data.UserId;
                        imgcls.CreatedDate = DateTime.Now;
                        imgcls.UpdatedDate = DateTime.Now;
                        _PortalContext.Add(imgcls);
                    }
                }
                else
                {
                    IVRM_Gallery_ClassDMO imgcls = new IVRM_Gallery_ClassDMO();
                    imgcls.IGA_Id = img.IGA_Id;
                    imgcls.ASMCL_Id = data.ASMCL_Id;
                    imgcls.ASMS_Id = data.ASMS_Id;
                    imgcls.IGACL_ActiveFlag = true;
                    imgcls.IGACL_CreatedBy = data.UserId;
                    imgcls.IGACL_UpdatedBy = data.UserId;
                    imgcls.CreatedDate = DateTime.Now;
                    imgcls.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(imgcls);

                }
                var contexttrans = _PortalContext.SaveChanges();
                if (contexttrans > 0)
                {
                    data.igaId = img.IGA_Id;
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
            }
            return data;
        }
        public IVRM_GalleryDTO savecover(IVRM_GalleryDTO data)
        {
            try
            {
                var coverflag = _PortalContext.IVRM_Gallery_PhotosDMO.Single(u => u.IGAP_Id == data.IGAP_Id);
                coverflag.IGAP_CoverPhotoFlag = true;
                _PortalContext.Update(coverflag);
                var contextupdate = _PortalContext.SaveChanges();
                if (contextupdate > 0)
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
                data.message = "Error";
            }
            return data;
        }


        public IVRM_GalleryDTO getcovermodel(IVRM_GalleryDTO data)
        {
            try
            {
                data.covermodel = (from a in _PortalContext.IVRM_GalleryDMO
                                   from b in _PortalContext.IVRM_Gallery_PhotosDMO
                                   where (a.IGA_Id == b.IGA_Id && a.IGA_ActiveFlag == true && a.MI_Id == data.MI_Id && a.IGA_Id == data.IGA_Id)
                                   select new IVRM_GalleryDTO
                                   {
                                       IGA_Id = a.IGA_Id,
                                       IGA_GalleryName = a.IGA_GalleryName,
                                       IGA_Date = a.IGA_Date,
                                       IGA_ActiveFlag = a.IGA_ActiveFlag,
                                       IGA_Time = a.IGA_Time,
                                       IGA_CommonGalleryFlg = a.IGA_CommonGalleryFlg,
                                       IGAP_Id = b.IGAP_Id,
                                       IGAP_Photos = b.IGAP_Photos,
                                       IGAP_CoverPhotoFlag = b.IGAP_CoverPhotoFlag,
                                       IGAP_ActiveFlag = b.IGAP_ActiveFlag
                                   }).Distinct().OrderBy(t => t.IGAP_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }





    }
}
