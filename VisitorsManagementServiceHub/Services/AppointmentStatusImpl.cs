using AutoMapper;
using CommonLibrary;
//using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;


namespace VisitorsManagementServiceHub.Services
{
    public class AppointmentStatusImpl : AppointmentStatusInterface
    {
        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _db;
        public AppointmentStatusImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext ctxt)
        {
            visctxt = context;
            _db = ctxt;
        }
        public AppointmentStatusDTO getDetails(AppointmentStatusDTO data)
        {
            try
            {
                //var institutionlist = (from a in visctxt.Institute
                //                       from b in visctxt.UserRoleWithInstituteDMO
                //                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                //                       select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                //data.institutionlist = institutionlist.ToArray();
                //if (data.MI_Id == 0)
                //{
                //    if (institutionlist.Count > 0)
                //    {
                //        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                //    }
                //}
                long roletypeid = 0;
                string roletype = "";

                if (data.UserId > 0)
                {
                    roletypeid = _db.appUserRole.Where(t => t.UserId == data.UserId).Distinct().FirstOrDefault().RoleTypeId;
                    if (roletypeid > 0)
                    {
                        roletype = _db.applicationRole.Where(t => t.Id == roletypeid).Distinct().FirstOrDefault().roleType;
                    }
                    if (roletype != "" && roletype != null)
                    {
                        if (roletype.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                        {
                            var institutionlist1 = (from a in visctxt.Institute
                                                    from b in visctxt.UserRoleWithInstituteDMO
                                                    where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                                    select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                            data.institutionlist = institutionlist1.ToArray();

                            if (data.MI_Id == 0)
                            {
                                if (institutionlist1.Count > 0)
                                {
                                    data.MI_Id = institutionlist1.FirstOrDefault().MI_Id;
                                }
                            }
                        }
                        else if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            var institutionlist2 = (from a in visctxt.Institute
                                                    from b in visctxt.UserRoleWithInstituteDMO
                                                    where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                                    select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                            data.institutionlist = institutionlist2.ToArray();

                            if (data.MI_Id == 0)
                            {
                                if (institutionlist2.Count > 0)
                                {
                                    data.MI_Id = institutionlist2.FirstOrDefault().MI_Id;
                                }
                            }
                        }
                        else if (roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                        {
                            var institutionlist3 = (from a in visctxt.Institute
                                                    from b in visctxt.UserRoleWithInstituteDMO
                                                    where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                                    select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                            data.institutionlist = institutionlist3.ToArray();

                            if (data.MI_Id == 0)
                            {
                                if (institutionlist3.Count > 0)
                                {
                                    data.MI_Id = institutionlist3.FirstOrDefault().MI_Id;
                                }
                            }
                        }
                        else
                        {
                            var institutionlist = (from a in visctxt.Institute
                                                   from b in visctxt.UserRoleWithInstituteDMO
                                                   where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                                   select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                            data.institutionlist = institutionlist.ToArray();
                            if (data.MI_Id == 0)
                            {
                                if (institutionlist.Count > 0)
                                {
                                    data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                                }
                            }
                        }
                    }
                    else
                    {
                        var institutionlist = (from a in visctxt.Institute
                                               from b in visctxt.UserRoleWithInstituteDMO
                                               where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                               select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                        data.institutionlist = institutionlist.ToArray();
                        if (data.MI_Id == 0)
                        {
                            if (institutionlist.Count > 0)
                            {
                                data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                            }
                        }
                    }
                }
                data.visitorlist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    where (a.MI_Id == data.MI_Id && a.VMMV_DeptFlag == false
                                    && a.VMMV_CkeckedInOutStatus != "Checked Out")
                                    select new AppointmentStatusDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName,
                                        MI_Id = a.MI_Id,
                                    }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public AppointmentStatusDTO saveData(AppointmentStatusDTO data)
        {
            try
            {
                if (data.VMMV_Id > 0)
                {
                    var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == data.VMMV_Id && d.MI_Id == data.MI_Id);
                    update.UpdatedDate = DateTime.Now;
                    update.VMMV_ExitDateTime = data.VMMV_ExitDateTime;
                    update.VMMV_CkeckedInOutStatus = data.VMMV_CkeckedInOutStatus;
                    update.VMMV_Remarks = data.VMMV_Remarks;

                    update.VMMV_UpdatedBy = data.UserId;
                    visctxt.Update(update);
                    int s = visctxt.SaveChanges();
                    if (s > 0)
                    {
                        data.returnVal = "updated";
                        var contactno = visctxt.Visitor_Management_MasterVisitor_DMO.Single(t => t.VMMV_Id == data.VMMV_Id).VMMV_VisitorContactNo;
                        var mailId = visctxt.Visitor_Management_MasterVisitor_DMO.Single(t => t.VMMV_Id == data.VMMV_Id).VMMV_VisitorEmailid;

                        var Template = "VisitorStatus";

                        if (data.type == "int_dept")
                        {
                            SMS sms = new SMS(_db);
                            sms.sendSms(data.MI_Id, contactno, Template, data.VMMV_Id);

                            Email Email = new Email(_db);
                            Email.sendmail(data.MI_Id, mailId, Template, data.VMMV_Id);
                        }
                        else if (data.type == "out_dept")
                        {
                            SMS sms = new SMS(_db);
                            sms.sendSms(data.MI_Id, contactno, Template, data.VMMV_Id);

                            Email Email = new Email(_db);
                            Email.sendmail(data.MI_Id, mailId, Template, data.VMMV_Id);
                        }

                    }
                    else
                    {
                        data.returnVal = "updateFailed";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public AppointmentStatusDTO EditDetails(AppointmentStatusDTO id)
        {
            AppointmentStatusDTO resp = new AppointmentStatusDTO();
            try
            {
                if (id.type == "int_dept")
                {
                    var editData = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                                    from e in visctxt.MasterEmployee
                                    where (a.MI_Id == b.MI_Id && a.VMMV_Id == b.VMMV_Id && e.HRME_Id == a.VMMV_ToMeet && a.MI_Id == id.MI_Id && a.VMMV_Id == id.VMMV_Id)
                                    select new AppointmentStatusDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName,
                                        empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName),
                                        VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                        VMMV_Remarks = a.VMMV_Remarks,
                                        VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                                        MI_Id = a.MI_Id,

                                    }).Distinct().ToList();
                    resp.editDetails = editData.ToArray();
                }
                else if (id.type == "out_dept")
                {
                    var editData = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                                        //from e in visctxt.MasterEmployee
                                    where (a.MI_Id == b.MI_Id && a.VMMV_Id == b.VMMV_Id && a.MI_Id == id.MI_Id && a.VMMV_Id == id.VMMV_Id)
                                    select new AppointmentStatusDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName,
                                        empname = a.VMMV_PersonToMeet,
                                        VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                        VMMV_Remarks = a.VMMV_Remarks,
                                        VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                                        MI_Id = a.MI_Id,
                                    }).Distinct().ToList();
                    resp.editDetails = editData.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }
        public AppointmentStatusDTO change_radiobutton(AppointmentStatusDTO data)
        {
            try
            {
                if (data.type == "int_dept")
                {
                    data.visitorlist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                        where (a.MI_Id == data.MI_Id && a.VMMV_DeptFlag == false && a.VMMV_CkeckedInOutStatus != "Checked Out")
                                        select new AppointmentStatusDTO
                                        {
                                            VMMV_Id = a.VMMV_Id,
                                            VMMV_VisitorName = a.VMMV_VisitorName,
                                            MI_Id = a.MI_Id,
                                        }).Distinct().ToArray();
                }
                else if (data.type == "out_dept")
                {
                    data.visitorlist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                        where (a.MI_Id == data.MI_Id && a.VMMV_DeptFlag == true && a.VMMV_CkeckedInOutStatus != "Checked Out")
                                        select new AppointmentStatusDTO
                                        {
                                            VMMV_Id = a.VMMV_Id,
                                            VMMV_VisitorName = a.VMMV_VisitorName,
                                            MI_Id = a.MI_Id,
                                        }).Distinct().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}