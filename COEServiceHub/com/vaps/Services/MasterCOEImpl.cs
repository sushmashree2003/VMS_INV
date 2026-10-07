using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.COE;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoeServiceHub.com.vaps.Services
{
    public class MasterCOEImpl : Interfaces.MasterCOEInterface
    {
        private static ConcurrentDictionary<string, MasterCOEDTO> _login =
       new ConcurrentDictionary<string, MasterCOEDTO>();


        public COEContext _coecontext;
        public MasterCOEImpl(COEContext ttcategory)
        {
            _coecontext = ttcategory;
        }

        public MasterCOEDTO savedetail1(MasterCOEDTO _category)
        {
            COE_Master_EventsDMO objpge = Mapper.Map<COE_Master_EventsDMO>(_category);
            try
            {

                if (objpge.COEME_Id > 0)
                {

                    var resultCount = _coecontext.COE_Master_EventsDMO.Where(t => t.COEME_EventName == objpge.COEME_EventName && t.MI_Id == objpge.MI_Id && t.COEME_Id != objpge.COEME_Id).Count();

                    if (resultCount == 0)
                    {
                        var result = _coecontext.COE_Master_EventsDMO.Single(t => t.COEME_Id == objpge.COEME_Id && t.MI_Id == objpge.MI_Id);

                        result.COEME_EventName = objpge.COEME_EventName;
                        result.COEME_EventDesc = objpge.COEME_EventDesc;
                        result.COEME_SMSMessage = objpge.COEME_SMSMessage;
                        result.COEME_MailSubject = objpge.COEME_MailSubject;
                        result.COEME_MailHeader = objpge.COEME_MailHeader;
                        result.COEME_MailFooter = objpge.COEME_MailFooter;
                        result.COEME_Mail_Message = objpge.COEME_Mail_Message;
                        result.COEME_MailHTMLTemplate = objpge.COEME_MailHTMLTemplate;
                        result.COEME_ActiveFlag = true;


                        result.UpdatedDate = DateTime.Now;
                        _coecontext.Update(result);
                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                }
                else
                {

                    var result = _coecontext.COE_Master_EventsDMO.Where(t => t.COEME_EventName == objpge.COEME_EventName && t.MI_Id == objpge.MI_Id).Count();

                    if (result > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else if (result == 0)
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.COEME_ActiveFlag = true;
                        _coecontext.Add(objpge);
                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }

                List<COE_Master_EventsDMO> m_events = new List<COE_Master_EventsDMO>();
                m_events = _coecontext.COE_Master_EventsDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                _category.master_eventlist = m_events.OrderByDescending(t => t.COEME_Id).ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public MasterCOEDTO savedetail2(MasterCOEDTO _category)
        {
            COE_EventsDMO objpge = Mapper.Map<COE_EventsDMO>(_category);

            try
            {

                if (objpge.COEE_Id > 0)
                {

                    var resultCount = _coecontext.COE_EventsDMO.Where(t => t.COEME_Id == objpge.COEME_Id && t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.COEE_Id != objpge.COEE_Id && t.COEE_ActiveFlag == true).Count();

                    if (resultCount == 0)
                    {
                        var result = _coecontext.COE_EventsDMO.Single(t => t.COEE_Id == objpge.COEE_Id && t.MI_Id == objpge.MI_Id);

                        result.ASMAY_Id = objpge.ASMAY_Id;
                        result.COEME_Id = objpge.COEME_Id;
                        result.COEE_EStartDate = objpge.COEE_EStartDate;
                        result.COEE_EEndDate = objpge.COEE_EEndDate;
                        result.COEE_EStartTime = objpge.COEE_EStartTime;
                        result.COEE_EEndTime = objpge.COEE_EEndTime;
                        result.COEE_SMSMessage = objpge.COEE_SMSMessage;
                        result.COEE_SMSActiveFlag = objpge.COEE_SMSActiveFlag;
                        result.COEE_MailSubject = objpge.COEE_MailSubject;
                        result.COEE_MailHeader = objpge.COEE_MailHeader;

                        result.COEE_MailFooter = objpge.COEE_MailFooter;
                        result.COEE_Mail_Message = objpge.COEE_Mail_Message;
                        result.COEE_MailHTMLTemplate = objpge.COEE_MailHTMLTemplate;
                        result.COEE_MailActiveFlag = objpge.COEE_MailActiveFlag;
                        result.COEE_ReminderDate = objpge.COEE_ReminderDate;
                        result.COEE_RepeatFlag = objpge.COEE_RepeatFlag;
                        result.COEE_ReminderSchedule = objpge.COEE_ReminderSchedule;
                        result.COEE_StudentFlag = objpge.COEE_StudentFlag;
                        result.COEE_AlumniFlag = objpge.COEE_AlumniFlag;
                        result.COEE_EmployeeFlag = objpge.COEE_EmployeeFlag;
                        result.COEE_LeftEmployeeFlag = objpge.COEE_LeftEmployeeFlag;
                        result.COEE_ManagementFlag = objpge.COEE_ManagementFlag;
                        result.COEE_OtherFlag = objpge.COEE_OtherFlag;
                        result.UpdatedDate = DateTime.Now;
                        result.COEE_ActiveFlag = true;
                        result.COEE_HolidayFlag = false;
                        _coecontext.Update(result);

                        delete_event_classes(objpge.COEE_Id);

                        if (objpge.COEE_StudentFlag == true)
                        {
                            foreach (var act1 in _category.stu_class_list)
                            {
                                COE_Events_ClassesDMO objpge1 = Mapper.Map<COE_Events_ClassesDMO>(_category);

                                objpge1.ASMCL_Id = act1.ASMCL_Id;
                                objpge1.CreatedDate = DateTime.Now;
                                objpge1.UpdatedDate = DateTime.Now;
                                _coecontext.Add(objpge1);

                            }
                        }

                        delete_event_emps(objpge.COEE_Id);

                        if (objpge.COEE_EmployeeFlag == true)
                        {
                            foreach (var act2 in _category.emp_type_list)
                            {
                                COE_Events_EmployeesDMO objpge2 = Mapper.Map<COE_Events_EmployeesDMO>(_category);
                                objpge2.HRMGT_Id = act2.HRMD_Id;
                                objpge2.CreatedDate = DateTime.Now;
                                objpge2.UpdatedDate = DateTime.Now;
                                _coecontext.Add(objpge2);

                            }
                        }

                        delete_event_empsLeft(objpge.COEE_Id);//Remove old data

                        //if (objpge.COEE_LeftEmployeeFlag == true)
                        //{
                        //    foreach (var leftemp in _category.empleftlist)
                        //    {
                        //        COE_Events_EmployeesDMO objleftemp = Mapper.Map<COE_Events_EmployeesDMO>(_category);
                        //        objleftemp.HRMGT_Id = leftemp.dept_id;
                        //        objleftemp.CreatedDate = DateTime.Now;
                        //        objleftemp.UpdatedDate = DateTime.Now;
                        //        _coecontext.Add(objleftemp);

                        //    }
                        //}

                        delete_event_othr_mobs(objpge.COEE_Id);

                        if (objpge.COEE_OtherFlag == true)
                        {
                            foreach (var act5 in _category.others_list)
                            {
                                COE_Events_OthersDMO objpge5 = Mapper.Map<COE_Events_OthersDMO>(_category);
                                objpge5.COEEO_MobileNo = act5.COEEO_MobileNo;
                                objpge5.CreatedDate = DateTime.Now;
                                objpge5.UpdatedDate = DateTime.Now;
                                objpge5.COEEO_Emailid = act5.COEEO_Emailid;
                                objpge5.COEEO_Name = act5.COEEO_Name;
                                _coecontext.Add(objpge5);

                            }
                        }


                        if (_category.images_list.Length > 0)
                        {
                            foreach (var act3 in _category.images_list)
                            {
                                var obj = _coecontext.COE_Events_ImagesDMO.Where(x => x.COEE_Id == objpge.COEE_Id).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.COEEI_Images = act3;
                                    obj.CreatedDate = DateTime.Now;
                                    obj.UpdatedDate = DateTime.Now;
                                    _coecontext.Update(obj);
                                }
                                else
                                {
                                    COE_Events_ImagesDMO objpge3 = Mapper.Map<COE_Events_ImagesDMO>(_category);
                                    objpge3.COEEI_Images = act3;
                                    objpge3.CreatedDate = DateTime.Now;
                                    objpge3.UpdatedDate = DateTime.Now;
                                    _coecontext.Add(objpge3);
                                }

                            }
                        }


                        if (_category.videos_list.Length > 0)
                        {
                            foreach (var act4 in _category.videos_list)
                            {
                                COE_Events_VideosDMO objpge4 = Mapper.Map<COE_Events_VideosDMO>(_category);
                                objpge4.COEEV_Videos = act4;
                                objpge4.CreatedDate = DateTime.Now;
                                objpge4.UpdatedDate = DateTime.Now;
                                _coecontext.Add(objpge4);

                            }
                        }

                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                }
                else
                {
                    var result = _coecontext.COE_EventsDMO.Where(t => t.COEME_Id == objpge.COEME_Id && t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.COEE_ActiveFlag == true).Count();

                    if (result > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else if (result == 0)
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.COEE_ActiveFlag = true;
                        objpge.COEE_HolidayFlag = false;

                        _coecontext.Add(objpge);
                        _category.COEE_Id = objpge.COEE_Id;


                        if (objpge.COEE_StudentFlag == true)
                        {
                            foreach (var act1 in _category.stu_class_list)
                            {
                                COE_Events_ClassesDMO objpge1 = Mapper.Map<COE_Events_ClassesDMO>(_category);

                                objpge1.ASMCL_Id = act1.ASMCL_Id;
                                objpge1.CreatedDate = DateTime.Now;
                                objpge1.UpdatedDate = DateTime.Now;
                                _coecontext.Add(objpge1);

                            }
                        }
                        //  }
                        if (objpge.COEE_EmployeeFlag == true)
                        {
                            foreach (var act2 in _category.emp_type_list)
                            {
                                COE_Events_EmployeesDMO objpge2 = Mapper.Map<COE_Events_EmployeesDMO>(_category);
                                objpge2.HRMGT_Id = act2.HRMD_Id;
                                objpge2.CreatedDate = DateTime.Now;
                                objpge2.UpdatedDate = DateTime.Now;
                                _coecontext.Add(objpge2);

                            }
                        }
                        if (objpge.COEE_LeftEmployeeFlag == true)
                        {
                            foreach (var leftemp in _category.empleftlist)
                            {
                                COE_Events_EmployeesDMO objleftemp = Mapper.Map<COE_Events_EmployeesDMO>(_category);
                                objleftemp.HRMGT_Id = leftemp.dept_id;
                                objleftemp.CreatedDate = DateTime.Now;
                                objleftemp.UpdatedDate = DateTime.Now;
                                _coecontext.Add(objleftemp);
                            }
                        }
                        if (objpge.COEE_OtherFlag == true)
                        {
                            foreach (var act5 in _category.others_list)
                            {
                                COE_Events_OthersDMO objpge5 = Mapper.Map<COE_Events_OthersDMO>(_category);
                                objpge5.COEEO_MobileNo = act5.COEEO_MobileNo;
                                objpge5.CreatedDate = DateTime.Now;
                                objpge5.UpdatedDate = DateTime.Now;
                                objpge5.COEEO_Emailid = act5.COEEO_Emailid;
                                objpge5.COEEO_Name = act5.COEEO_Name;
                                _coecontext.Add(objpge5);

                            }
                        }

                        if (_category.images_list.Length > 0)
                        {
                            foreach (var act3 in _category.images_list)
                            {
                                COE_Events_ImagesDMO objpge3 = Mapper.Map<COE_Events_ImagesDMO>(_category);
                                objpge3.COEEI_Images = act3;
                                objpge3.CreatedDate = DateTime.Now;
                                objpge3.UpdatedDate = DateTime.Now;
                                _coecontext.Add(objpge3);

                            }
                        }
                        if (_category.videos_list.Length > 0)
                        {
                            foreach (var act4 in _category.videos_list)
                            {
                                COE_Events_VideosDMO objpge4 = Mapper.Map<COE_Events_VideosDMO>(_category);
                                objpge4.COEEV_Videos = act4;
                                objpge4.CreatedDate = DateTime.Now;
                                objpge4.UpdatedDate = DateTime.Now;
                                _coecontext.Add(objpge4);

                            }
                        }

                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }


                _category.mapped_eventlist = (from a in _coecontext.COE_Master_EventsDMO
                                              from b in _coecontext.COE_EventsDMO
                                              from c in _coecontext.AcademicYear
                                              where (a.MI_Id == _category.MI_Id && a.MI_Id == b.MI_Id && a.COEME_Id == b.COEME_Id && b.ASMAY_Id == c.ASMAY_Id && b.MI_Id == c.MI_Id)
                                              select new MasterCOEDTO
                                              {
                                                  ASMAY_Year = c.ASMAY_Year,
                                                  COEE_Id = b.COEE_Id,
                                                  COEME_Id = a.COEME_Id,
                                                  COEME_EventName = a.COEME_EventName,
                                                  COEE_EStartDate = b.COEE_EStartDate,
                                                  COEE_EEndDate = b.COEE_EEndDate,
                                                  COEE_ActiveFlag = b.COEE_ActiveFlag,
                                              }
                                  ).Distinct().OrderByDescending(t => t.COEME_Id).ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public MasterCOEDTO getdetails(int id)
        {
            MasterCOEDTO TTMC = new MasterCOEDTO();
            try
            {
                List<SMS_MAIL_PARAMETER_DMO> paramsall = new List<SMS_MAIL_PARAMETER_DMO>();
                paramsall = _coecontext.SMS_MAIL_PARAMETER_DMO.ToList();
                TTMC.parameterlist = paramsall.Distinct().ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _coecontext.AcademicYear.Where(y => y.MI_Id == id && y.ASMAY_ActiveFlag == 1 && y.Is_Active == true).ToList();
                TTMC.yearlist = year.Distinct().ToArray();

                List<COE_Master_EventsDMO> m_events = new List<COE_Master_EventsDMO>();
                m_events = _coecontext.COE_Master_EventsDMO.Where(e => e.MI_Id == id).ToList();
                TTMC.master_eventlist = m_events.OrderByDescending(t => t.COEME_Id).ToArray();

                List<COE_Master_EventsDMO> events = new List<COE_Master_EventsDMO>();
                m_events = _coecontext.COE_Master_EventsDMO.Where(e => e.MI_Id == id && e.COEME_ActiveFlag == true).OrderBy(e => e.COEME_EventName).ToList();

                TTMC.eventlist_act = m_events.OrderByDescending(t => t.COEME_Id).ToArray();


                List<AdmissionClass> classes = new List<AdmissionClass>();
                classes = _coecontext.AdmissionClass.Where(c => c.MI_Id == id && c.ASMCL_ActiveFlag == true).ToList();
                TTMC.classlist = classes.ToArray();

                //List<HR_Master_GroupTypeDMO> stf_types = new List<HR_Master_GroupTypeDMO>();
                //stf_types = _coecontext.HR_Master_GroupTypeDMO.Where(t => t.MI_Id == id && t.HRMGT_ActiveFlag == true).ToList();
                //TTMC.stafftypelist = stf_types.Distinct().ToArray();

                List<HR_Master_Department> stf_types = new List<HR_Master_Department>();
                stf_types = (from a in _coecontext.HR_Master_Department
                             from b in _coecontext.HR_Master_Employee_DMO
                             where (b.MI_Id == id && a.HRMD_Id==b.HRMD_Id && a.HRMD_ActiveFlag==true && b.HRME_ActiveFlag==true && b.HRME_LeftFlag==false )select a).ToList();
                TTMC.stafftypelist = stf_types.Distinct().ToArray();

                TTMC.leftemplist = (from a in _coecontext.HR_Master_Department
                                    from b in _coecontext.HR_Master_Employee_DMO
                                    where (a.MI_Id == b.MI_Id && a.HRMD_Id == b.HRMD_Id && b.MI_Id == id && b.HRME_LeftFlag == true)
                                    select new MasterCOEDTO
                                    {
                                        dept_id = a.HRMD_Id,
                                        deptname = a.HRMD_DepartmentName,
                                    }).Distinct().ToArray();

                TTMC.mapped_eventlist = (from a in _coecontext.COE_Master_EventsDMO
                                         from b in _coecontext.COE_EventsDMO
                                         from c in _coecontext.AcademicYear
                                         where (a.MI_Id == id && a.MI_Id == b.MI_Id && a.COEME_Id == b.COEME_Id && b.ASMAY_Id == c.ASMAY_Id && b.MI_Id == c.MI_Id)
                                         select new MasterCOEDTO
                                         {
                                             ASMAY_Year = c.ASMAY_Year,
                                             COEE_Id = b.COEE_Id,
                                             COEME_Id = a.COEME_Id,
                                             COEME_EventName = a.COEME_EventName,
                                             COEE_EStartDate = b.COEE_EStartDate,
                                             COEE_EEndDate = b.COEE_EEndDate,
                                             COEE_ActiveFlag = b.COEE_ActiveFlag,
                                         }).Distinct().OrderByDescending(t => t.COEE_Id).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }


        public MasterCOEDTO geteventdetails(MasterCOEDTO _category)
        {

            try
            {

                List<COE_Master_EventsDMO> s_m_events = new List<COE_Master_EventsDMO>();
                s_m_events = _coecontext.COE_Master_EventsDMO.Where(e => e.MI_Id == _category.MI_Id && e.COEME_Id == _category.COEME_Id).ToList();
                _category.selected_master_event = s_m_events.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;

        }


        public MasterCOEDTO getalldetailsviewrecords1(int id)
        {
            MasterCOEDTO page = new MasterCOEDTO();
            try
            {
                List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
                events_m = _coecontext.COE_Master_EventsDMO.Where(e => e.COEME_Id == id).ToList();
                page.edit_m_event = events_m.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterCOEDTO getalldetailsviewrecords2(int id)
        {
            MasterCOEDTO page = new MasterCOEDTO();
            try
            {
                List<COE_EventsDMO> events_map = new List<COE_EventsDMO>();
                events_map = _coecontext.COE_EventsDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_map_event = events_map.ToArray();
                var m_id = events_map[0].COEME_Id;
                List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
                events_m = _coecontext.COE_Master_EventsDMO.Where(e => e.COEME_Id == m_id).ToList();
                page.edit_m_event = events_m.ToArray();


                page.edit_stu_class_list = (from a in _coecontext.COE_Events_ClassesDMO
                                            from b in _coecontext.AdmissionClass
                                            where (a.COEE_Id == id && a.ASMCL_Id == b.ASMCL_Id)
                                            select new MasterCOEDTO
                                            {
                                                COEEC_Id = a.COEEC_Id,
                                                COEE_Id = a.COEE_Id,
                                                ASMCL_Id = a.ASMCL_Id,
                                                ASMCL_ClassName = b.ASMCL_ClassName,
                                            }
                                          ).Distinct().ToArray();



                page.edit_emp_type_list = (from a in _coecontext.COE_Events_EmployeesDMO
                                           from b in _coecontext.HR_Master_Department
                                           from c in _coecontext.HR_Master_Employee_DMO
                                           where (a.COEE_Id == id && a.HRMGT_Id == b.HRMD_Id && c.HRMD_Id == a.HRMGT_Id && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false)
                                           select new MasterCOEDTO
                                           {
                                               COEEE_Id = a.COEEE_Id,
                                               COEE_Id = a.COEE_Id,
                                               HRMD_Id = a.HRMGT_Id,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,

                                           }).Distinct().ToArray();

                page.edit_leftemp_type_list = (from a in _coecontext.COE_Events_EmployeesDMO
                                               from b in _coecontext.HR_Master_Department
                                               from c in _coecontext.HR_Master_Employee_DMO
                                               where (a.COEE_Id == id && a.HRMGT_Id == b.HRMD_Id && c.HRMD_Id == a.HRMGT_Id && c.HRME_LeftFlag == true)
                                               select new MasterCOEDTO
                                               {
                                                   COEEE_Id = a.COEEE_Id,
                                                   COEE_Id = a.COEE_Id,
                                                   HRMD_Id = a.HRMGT_Id,
                                                   HRMD_DepartmentName = b.HRMD_DepartmentName,

                                               }).Distinct().ToArray();

                List<COE_Events_OthersDMO> others_map = new List<COE_Events_OthersDMO>();
                others_map = _coecontext.COE_Events_OthersDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_oth_mobilenos_list = others_map.ToArray();

                List<COE_Events_ImagesDMO> images_map = new List<COE_Events_ImagesDMO>();
                images_map = _coecontext.COE_Events_ImagesDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_images_list = images_map.ToArray();

                List<COE_Events_VideosDMO> videos_map = new List<COE_Events_VideosDMO>();
                videos_map = _coecontext.COE_Events_VideosDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_videos_list = videos_map.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterCOEDTO getpageedit1(int id)
        {
            MasterCOEDTO page = new MasterCOEDTO();
            try
            {
                List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
                events_m = _coecontext.COE_Master_EventsDMO.Where(e => e.COEME_Id == id).ToList();
                page.edit_m_event = events_m.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterCOEDTO getpageedit2(int id)
        {
            MasterCOEDTO page = new MasterCOEDTO();
            try
            {
                List<COE_EventsDMO> events_map = new List<COE_EventsDMO>();
                events_map = _coecontext.COE_EventsDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_map_event = events_map.ToArray();
                var m_id = events_map[0].COEME_Id;
                List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
                events_m = _coecontext.COE_Master_EventsDMO.Where(e => e.COEME_Id == m_id).ToList();
                page.edit_m_event = events_m.ToArray();

                page.edit_stu_class_list = (from a in _coecontext.COE_Events_ClassesDMO
                                            from b in _coecontext.AdmissionClass
                                            where (a.COEE_Id == id && a.ASMCL_Id == b.ASMCL_Id)
                                            select new MasterCOEDTO
                                            {
                                                COEEC_Id = a.COEEC_Id,
                                                COEE_Id = a.COEE_Id,
                                                ASMCL_Id = a.ASMCL_Id,
                                                ASMCL_ClassName = b.ASMCL_ClassName,
                                            }
                                          ).Distinct().ToArray();


                page.edit_emp_type_list = (from a in _coecontext.COE_Events_EmployeesDMO
                                           from b in _coecontext.HR_Master_Department
                                           from c in _coecontext.HR_Master_Employee_DMO
                                           where (a.COEE_Id == id && a.HRMGT_Id == b.HRMD_Id && c.HRMD_Id == a.HRMGT_Id && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false)
                                           select new MasterCOEDTO
                                           {
                                               COEEE_Id = a.COEEE_Id,
                                               COEE_Id = a.COEE_Id,
                                               HRMD_Id = b.HRMD_Id,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,

                                           }).Distinct().ToArray();

                page.edit_leftemp_type_list = (from a in _coecontext.COE_Events_EmployeesDMO
                                               from b in _coecontext.HR_Master_Department
                                               from c in _coecontext.HR_Master_Employee_DMO
                                               where (a.COEE_Id == id && a.HRMGT_Id == b.HRMD_Id && c.HRMD_Id == a.HRMGT_Id && c.HRME_LeftFlag == true)
                                               select new MasterCOEDTO
                                               {
                                                   COEEE_Id = a.COEEE_Id,
                                                   COEE_Id = a.COEE_Id,
                                                   dept_id = a.HRMGT_Id,
                                                   deptname = b.HRMD_DepartmentName,

                                               }).Distinct().ToArray();

                List<COE_Events_OthersDMO> others_map = new List<COE_Events_OthersDMO>();
                others_map = _coecontext.COE_Events_OthersDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_oth_mobilenos_list = others_map.ToArray();

                List<COE_Events_ImagesDMO> images_map = new List<COE_Events_ImagesDMO>();
                images_map = _coecontext.COE_Events_ImagesDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_images_list = images_map.ToArray();

                List<COE_Events_VideosDMO> videos_map = new List<COE_Events_VideosDMO>();
                videos_map = _coecontext.COE_Events_VideosDMO.Where(e => e.COEE_Id == id).ToList();
                page.edit_videos_list = videos_map.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public MasterCOEDTO deleterec(int id)
        {
            MasterCOEDTO page = new MasterCOEDTO();
            try
            {
                //List<TT_Master_Staff_AbbreviationDMO> lorg = new List<TT_Master_Staff_AbbreviationDMO>();
                //lorg = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Where(t => t.TTMSAB_Id.Equals(id)).ToList();
                //if (lorg.Any())
                //{
                //    _ttcategorycontext.Remove(lorg.ElementAt(0));
                //    var contactExists = _ttcategorycontext.SaveChanges();
                //    if (contactExists == 1)
                //    {
                //        page.returnval = true;
                //    }
                //    else
                //    {
                //        page.returnval = false;
                //    }
                //}

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        //active deactive 
        public MasterCOEDTO deactivate1(MasterCOEDTO data)
        {
            COE_Master_EventsDMO pge = Mapper.Map<COE_Master_EventsDMO>(data);
            if (pge.COEME_Id > 0)
            {
                var result = _coecontext.COE_Master_EventsDMO.Single(t => t.COEME_Id == pge.COEME_Id);
                if (result.COEME_ActiveFlag == true)
                {
                    result.COEME_ActiveFlag = false;
                }
                else
                {
                    result.COEME_ActiveFlag = true;
                }
                // result.UpdatedDate = DateTime.Now;
                _coecontext.Update(result);
                var flag = _coecontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }
        public MasterCOEDTO deactivate2(MasterCOEDTO data)
        {
            COE_EventsDMO pge = Mapper.Map<COE_EventsDMO>(data);
            if (pge.COEME_Id > 0)
            {
                var result = _coecontext.COE_EventsDMO.Where(t => t.COEE_Id == pge.COEE_Id).Select(d => new COE_EventsDMO
                {
                    COEE_Id = d.COEE_Id,
                    COEE_ActiveFlag = d.COEE_ActiveFlag,
                    MI_Id = d.MI_Id,
                    ASMAY_Id = d.ASMAY_Id,
                    COEME_Id = d.COEME_Id
                }).ToList();
                if (result.FirstOrDefault().COEE_ActiveFlag == true)
                {
                    result.FirstOrDefault().COEE_ActiveFlag = false;
                }
                else
                {
                    result.FirstOrDefault().COEE_ActiveFlag = true;
                }
                // result.UpdatedDate = DateTime.Now;
                _coecontext.Update(result.ElementAt(0));
                var flag = _coecontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }


        public MasterCOEDTO delete_event_classes(int id)
        {
            MasterCOEDTO pagert = new MasterCOEDTO();
            //  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_ClassesDMO> lorg = new List<COE_Events_ClassesDMO>();
                lorg = _coecontext.COE_Events_ClassesDMO.Where(t => t.COEE_Id == id).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _coecontext.Remove(lorg.ElementAt(i));
                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public MasterCOEDTO delete_event_emps(int id)
        {
            List<long> dept_ids = new List<long>();
            List<long> lefts_ids = new List<long>();
            MasterCOEDTO pagert = new MasterCOEDTO();
            //  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {
                var hrmgt_ids = (from a in _coecontext.COE_Events_EmployeesDMO
                                 from b in _coecontext.HR_Master_Employee_DMO
                                 from c in _coecontext.HR_Master_Department
                                 where (b.HRMD_Id == a.HRMGT_Id && c.HRMD_Id == b.HRMD_Id  && a.COEE_Id == id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                 select a).ToList();

                var left_id = (from a in _coecontext.COE_Events_EmployeesDMO
                                 from b in _coecontext.HR_Master_Employee_DMO
                                 from c in _coecontext.HR_Master_Department
                                 where (b.HRMD_Id == a.HRMGT_Id && c.HRMD_Id == b.HRMD_Id && a.COEE_Id == id && b.HRME_LeftFlag == true)
                                 select a).ToList();
                

                if (hrmgt_ids.Count>0)
                {
                    foreach (var item in hrmgt_ids)
                    {
                        dept_ids.Add(item.HRMGT_Id);
                    }
                }
                if (left_id.Count > 0)
                {
                    foreach (var item in left_id)
                    {
                        lefts_ids.Add(item.HRMGT_Id);
                    }
                }

                //List<COE_Events_EmployeesDMO> lorg = new List<COE_Events_EmployeesDMO>();
                //lorg = _coecontext.COE_Events_EmployeesDMO.Where(t => t.COEE_Id.Equals(id) && dept_ids.Contains(t.HRMGT_Id) && !lefts_ids.Contains(t.HRMGT_Id)).ToList();
                List<COE_Events_EmployeesDMO> lorg = new List<COE_Events_EmployeesDMO>();
                lorg = _coecontext.COE_Events_EmployeesDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _coecontext.Remove(lorg.ElementAt(i));
                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public MasterCOEDTO delete_event_empsLeft(int id)
        {
            List<long> dept_ids = new List<long>();
            MasterCOEDTO pagert = new MasterCOEDTO();
            //  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {
                var hrmgt_ids = (from a in _coecontext.COE_Events_EmployeesDMO
                                 from b in _coecontext.HR_Master_Employee_DMO
                                 from c in _coecontext.HR_Master_Department
                                 where (b.HRMD_Id == a.HRMGT_Id && c.HRMD_Id == b.HRMD_Id && a.COEE_Id == id &&  b.HRME_LeftFlag == true)
                                 select a).ToList();

                if (hrmgt_ids.Count > 0)
                {
                    foreach (var item in hrmgt_ids)
                    {
                        dept_ids.Add(item.HRMGT_Id);
                    }
                }
                List<COE_Events_EmployeesDMO> lorg = new List<COE_Events_EmployeesDMO>();
                lorg = _coecontext.COE_Events_EmployeesDMO.Where(t => t.COEE_Id.Equals(id) && dept_ids.Contains(t.HRMGT_Id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _coecontext.Remove(lorg.ElementAt(i));
                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public MasterCOEDTO delete_event_othr_mobs(int id)
        {
            MasterCOEDTO pagert = new MasterCOEDTO();
            //  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_OthersDMO> lorg = new List<COE_Events_OthersDMO>();
                lorg = _coecontext.COE_Events_OthersDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _coecontext.Remove(lorg.ElementAt(i));
                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public MasterCOEDTO delete_event_images(int id)
        {
            MasterCOEDTO pagert = new MasterCOEDTO();
            //  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_ImagesDMO> lorg = new List<COE_Events_ImagesDMO>();
                lorg = _coecontext.COE_Events_ImagesDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _coecontext.Remove(lorg.ElementAt(i));
                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public MasterCOEDTO delete_event_videos(int id)
        {
            MasterCOEDTO pagert = new MasterCOEDTO();
            //  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<COE_Events_VideosDMO> lorg = new List<COE_Events_VideosDMO>();
                lorg = _coecontext.COE_Events_VideosDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _coecontext.Remove(lorg.ElementAt(i));
                        var contactExists = _coecontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }


    }
}
