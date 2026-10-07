using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using QRCoder;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System.Drawing.Imaging;

namespace VisitorsManagementServiceHub.Services
{
    public class AddVisitorsImpl : AddVisitorsInterface
    {
        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _db;
        public AddVisitorsImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext dbctxt)
        {
            visctxt = context;
            _db = dbctxt;
        }
        public AddVisitorsDTO getDetails(AddVisitorsDTO data)
        {
            try
            {
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
                var gridoptionslist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                       from e in visctxt.MasterEmployee
                                       where (a.MI_Id == data.MI_Id && a.VMMV_ToMeet == e.HRME_Id
                                       && a.VMMV_DeptFlag == false)
                                       select new AddVisitorsDTO
                                       {
                                           VMMV_Id = a.VMMV_Id,
                                           VMMV_VisitorName = a.VMMV_VisitorName,
                                           VMMV_CardNo = a.VMMV_CardNo,
                                           VMMV_CkeckedInOutStatus = a.VMMV_CkeckedInOutStatus,
                                           VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                                           VMMV_IdentityCardType = a.VMMV_IdentityCardType,
                                           VMMV_FromAddress = a.VMMV_FromAddress,
                                           VMMV_FromPlace = a.VMMV_FromPlace,
                                           VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                           VMMV_MeetingLocation = a.VMMV_MeetingLocation,
                                           VMMV_MeetingPurpose = a.VMMV_MeetingPurpose,
                                           VMMV_VIPFlg = a.VMMV_VIPFlg,
                                           VMMV_Remarks = a.VMMV_Remarks,
                                           VMMV_VisitorContactNo = a.VMMV_VisitorContactNo,
                                           VMMV_VisitorEmailid = a.VMMV_VisitorEmailid,
                                           VMMV_MeetingDuration = a.VMMV_MeetingDuration,
                                           MI_Id = a.MI_Id,
                                           VMMV_DeptFlag = a.VMMV_DeptFlag,
                                           CreatedDate = a.CreatedDate,
                                           empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName),
                                           staffdeptflag = a.VMMV_DeptFlag == true ? "OutSide Dept" : "Internal Dept",
                                           count_subvisitors = visctxt.Multiple_VisitorDMO.Where(z => z.VMMV_Id == a.VMMV_Id).Count()
                                       }).Distinct().OrderByDescending(a => a.CreatedDate).ToList();

                if (gridoptionslist.Count > 0)
                {
                    data.gridoptions = gridoptionslist.OrderByDescending(a => a.VMMV_Id).ToArray();
                    data.count = gridoptionslist.Count;
                }
                else
                {
                    data.count = 0;
                }



                data.emplist = (from a in _db.HR_Master_Employee_DMO
                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                select new AddVisitorsDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,
                                }).Distinct().ToArray();

                data.emplistautjorizedby = (from a in _db.HR_Master_Employee_DMO
                                            where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                            select new AddVisitorsDTO
                                            {
                                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                HRME_Id = a.HRME_Id,
                                            }).Distinct().ToArray();

               
                string valueqr =  data.VMAP_VisitorName ;

                Bitmap bitmap = new Bitmap(100, 100);


                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(60);

                byte[] byteArray = BitmapToByteArray(QrBitmap);
                string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public static byte[] BitmapToByteArray(Bitmap QrBitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                QrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public AddVisitorsDTO EditDetails(AddVisitorsDTO id)
        {
            AddVisitorsDTO resp = new AddVisitorsDTO();
            try
            {
                var editData = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == id.MI_Id && a.VMMV_Id == id.VMMV_Id).ToArray();

                resp.editDetails = editData.ToArray();

                var getmulti_visitors = visctxt.Multiple_VisitorDMO.Where(a => a.VMMV_Id == id.VMMV_Id).ToArray();

                resp.getmulti_visitors = getmulti_visitors;

                if (editData.FirstOrDefault().VMMV_DeptFlag == false)
                {
                    long empid = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == id.MI_Id && a.VMMV_Id == id.VMMV_Id).Distinct().SingleOrDefault().VMMV_ToMeet;

                    long empidhrmeid = visctxt.Visitor_Management_Visitor_ToMeetDMO.Where(a => a.MI_Id == id.MI_Id && a.VMMV_Id == id.VMMV_Id).Distinct().SingleOrDefault().HRME_Id;

                    resp.emplist = (from a in _db.HR_Master_Employee_DMO
                                    where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                    select new AddVisitorsDTO
                                    {
                                        HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                        HRME_Id = a.HRME_Id,
                                    }).Distinct().ToArray();

                    resp.emplistautjorizedby = (from a in _db.HR_Master_Employee_DMO
                                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                                select new AddVisitorsDTO
                                                {
                                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' '
                                                    + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                    HRME_Id = a.HRME_Id,
                                                }).Distinct().ToArray();

                    resp.editemplist = (from a in _db.HR_Master_Employee_DMO
                                        where (a.HRME_Id == empid && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                        select new InwardDTO
                                        {
                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' '
                                            + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                            HRME_Id = a.HRME_Id,
                                        }).Distinct().ToArray();

                    resp.editemplistauthby = (from a in _db.HR_Master_Employee_DMO
                                              where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == empidhrmeid)
                                              select new AddVisitorsDTO
                                              {
                                                  HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' '
                                                  + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                  HRME_Id = a.HRME_Id,
                                              }).Distinct().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }
        public AddVisitorsDTO deactivate(AddVisitorsDTO obj)
        {
            try
            {
                var query = visctxt.Visitor_Management_MasterVisitor_DMO.Where(d => d.VMMV_Id == obj.VMMV_Id).ToList();
                if (query.Count > 0)
                {
                    var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == obj.VMMV_Id);
                    update.UpdatedDate = DateTime.Now;
                    if (update.VMMV_ActiveFlag == true)
                    {
                        update.VMMV_ActiveFlag = false;
                    }
                    else
                    {
                        update.VMMV_ActiveFlag = true;
                    }
                    visctxt.Update(update);
                    int s = visctxt.SaveChanges();
                    if (s > 0)
                    {
                        obj.value = true;
                        if (obj.VMMV_ActiveFlag == false)
                        {
                            obj.msg = "Record Activated Successfully";
                        }
                        else if (obj.VMMV_ActiveFlag == true)
                        {
                            obj.msg = "Record Deactivated Successfully";
                        }
                    }
                    else
                    {
                        obj.value = false;
                        obj.msg = "activationFailed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public AddVisitorsDTO saveData(AddVisitorsDTO data)
        {
            try
            {
                if (data.VMMV_Id == 0)
                {
                    //var checkduplicate = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == data.MI_Id && a.VMMV_VisitorName == data.VMMV_VisitorName
                    //&& a.VMMV_VisitorContactNo == data.VMMV_VisitorContactNo && a.VMMV_VisitorEmailid == data.VMMV_VisitorEmailid
                    //&& a.VMMV_EntryDateTime == data.VMMV_EntryDateTime && a.VMMV_MeetingDateTime == data.VMMV_MeetingDateTime).ToList();
                    //if (checkduplicate.Count > 0)
                    //{
                    //    data.returnVal = "duplicate";
                    //}
                    //else
                    //{
                    Visitor_Management_MasterVisitor_DMO mapp = new Visitor_Management_MasterVisitor_DMO();

                    mapp.MI_Id = data.MI_Id;
                    mapp.VMMV_Id = data.VMMV_Id;
                    mapp.VMMV_VisitorName = data.VMMV_VisitorName;
                    mapp.VMMV_VisitorContactNo = data.VMMV_VisitorContactNo;
                    mapp.VMMV_VisitorEmailid = data.VMMV_VisitorEmailid;
                    mapp.VMMV_IdentityCardType = data.VMMV_IdentityCardType;
                    mapp.VMMV_CardNo = data.VMMV_CardNo;
                    mapp.VMMV_CardImage = data.VMMV_CardImage;
                    mapp.VMMV_FromPlace = data.VMMV_FromPlace;
                    mapp.VMMV_VisitTypeFlg = data.VMMV_VisitTypeFlg;
                    mapp.VMMV_FromAddress = data.VMMV_FromAddress;
                    mapp.VMMV_MeetingPurpose = data.VMMV_MeetingPurpose;
                    mapp.VMMV_EntryDateTime = data.VMMV_EntryDateTime;
                    mapp.VMMV_MeetingDateTime = data.VMMV_MeetingDateTime;
                    mapp.VMMV_MeetingDuration = data.VMMV_MeetingDuration;
                    mapp.VMMV_MeetingLocation = data.VMMV_MeetingLocation;
                    mapp.VMMV_VIPFlg = data.VMMV_VIPFlg;
                    mapp.VMMV_PersonsAccompanying = data.VMMV_PersonsAccompanying;
                    mapp.VMMV_AuthorisationBy = data.VMMV_AuthorisationBy;
                    mapp.VMMV_VisitorPhoto = data.VMMV_VisitorPhoto;
                    mapp.VMMV_VisitorFingerPrint = data.VMMV_VisitorFingerPrint;
                    mapp.VMMV_ExitDateTime = data.VMMV_ExitDateTime;
                    mapp.VMMV_Remarks = data.VMMV_Remarks;
                    mapp.VMMV_DeptFlag = data.dept_flag;
                    mapp.VMMV_PersonToMeet = data.VMMV_PersonToMeet;
                    mapp.VMMV_Remarks = data.VMMV_Remarks;
                    mapp.VMMV_ActiveFlag = true;
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;
                    mapp.VMMV_CreatedBy = data.UserId;
                    mapp.VMMV_UpdatedBy = data.UserId;
                    mapp.VMMV_ToMeet = data.VMVTMT_ToMeet_HRME_Id;
                    mapp.TENANTAGR_Id = data.TENANTAGR_Id;
                    mapp.VMMV_CkeckedInOutStatus = "Checked In";

                    visctxt.Add(mapp);

                    if (data.multivisitor != null && data.multivisitor.Length > 0)
                    {
                        foreach (var item in data.multivisitor)
                        {
                            if (item.VMMVVI_VisitorName != null && item.VMMVVI_VisitorName != "")
                            {
                                Multiple_VisitorDMO mvd = new Multiple_VisitorDMO();

                                mvd.VMMV_Id = mapp.VMMV_Id;
                                mvd.VMMVVI_VisitorName = item.VMMVVI_VisitorName;
                                mvd.VMMVVI_VisitorAddress = item.VMMVVI_VisitorAddress;
                                mvd.VMMVVI_VisitorEmailId = item.VMMVVI_VisitorEmailId;
                                mvd.VMMVVI_VisitorContactNo = item.VMMVVI_VisitorContactNo;
                                mvd.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                mvd.VMMVVI_CreatedDate = DateTime.Now;
                                mvd.VMMVVI_CreatedBy = data.UserId;
                                mvd.VMMVVI_UpdatedBy = data.UserId;
                                mvd.VMMVVI_Updateddate = DateTime.Now;
                                mvd.VMMVVI_VisitorCardNo = item.VMMVVI_VisitorCardNo;
                                mvd.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                visctxt.Add(mvd);
                            }
                        }
                    }

                    if (data.VMAP_Id > 0)
                    {
                        Visitor_Management_Visitor_Appointment_DMO obj2 = new Visitor_Management_Visitor_Appointment_DMO();
                        obj2.VMAP_Id = data.VMAP_Id;
                        obj2.MI_Id = data.MI_Id;
                        obj2.VMMV_Id = mapp.VMMV_Id;
                        visctxt.Add(obj2);

                        var checkresultcount = visctxt.Visitor_Management_Appointment_DMO.Where(a => a.VMAP_Id == data.VMAP_Id).ToList();

                        if (checkresultcount.Count > 0)
                        {
                            var checkresult = visctxt.Visitor_Management_Appointment_DMO.Single(a => a.VMAP_Id == data.VMAP_Id);
                            checkresult.VMAP_ChekInOutStatus = "Check In";
                            checkresult.UpdatedDate = DateTime.Now;
                            checkresult.VMAP_UpdatedBy = data.UserId;
                            visctxt.Update(checkresult);
                        }
                    }

                    if (data.dept_flag == false)
                    {
                        string[] split = data.VMMV_EntryDateTime.Split(":");

                        data.fhrors = Convert.ToInt32(split[0]);
                        data.fminutes = Convert.ToInt32(split[1]);

                        Visitor_Management_Visitor_ToMeetDMO management_Visitor_ToMeetDMO = new Visitor_Management_Visitor_ToMeetDMO();
                        management_Visitor_ToMeetDMO.MI_Id = data.MI_Id;
                        management_Visitor_ToMeetDMO.VMMV_Id = mapp.VMMV_Id;
                        management_Visitor_ToMeetDMO.VMVTMT_ToMeet_HRME_Id = data.VMVTMT_ToMeet_HRME_Id;
                        management_Visitor_ToMeetDMO.VMVTMT_DateTime = data.VMMV_MeetingDateTime.AddHours(data.fhrors).AddMinutes(data.fminutes);
                        management_Visitor_ToMeetDMO.HRME_Id = data.HRME_Id;
                        management_Visitor_ToMeetDMO.VMVTMT_MetFlg = false;
                        management_Visitor_ToMeetDMO.VMVTMT_Location = data.VMMV_MeetingLocation;
                        management_Visitor_ToMeetDMO.VMVTMT_Flg = true;
                        management_Visitor_ToMeetDMO.VMVTMT_CreatedBy = data.UserId;
                        management_Visitor_ToMeetDMO.VMVTMT_UpdatedBy = data.UserId;
                        management_Visitor_ToMeetDMO.VMVTMT_CreatedDate = DateTime.Now;
                        management_Visitor_ToMeetDMO.VMVTMT_UpdatedDate = DateTime.Now;
                        visctxt.Add(management_Visitor_ToMeetDMO);
                    }


                    //Visitor_Management_Visitor_Appointment_DMO obj2 = new Visitor_Management_Visitor_Appointment_DMO();
                    //obj2.VMVAP_Id = data.VMVAP_Id;
                    //obj2.MI_Id = data.MI_Id;
                    //obj2.VMMV_Id = mapp.VMMV_Id;
                    //visctxt.Add(obj2);

                    int s = visctxt.SaveChanges();
                    if (s > 0)
                    {
                        data.returnVal = "saved";
                        var vis_list1 = visctxt.Visitor_Management_MasterVisitor_DMO.OrderByDescending(d => d.VMMV_Id).First();
                        var qrid = visctxt.Visitor_Management_MasterVisitor_DMO.Single(t => t.VMMV_Id == mapp.VMMV_Id);

                        string valueqr = "VM" + "#" + data.MI_Id + "#" + mapp.VMMV_Id;
                        Bitmap bitmap = new Bitmap(100, 100);
                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                        QRCode QrCode = new QRCode(QrCodeInfo);
                        Bitmap QrBitmap = QrCode.GetGraphic(10);
                        byte[] byteArray = BitmapToByteArray(QrBitmap);
                        qrid.VMMV_QRCode = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));
                        data.ASCISS_QRCode = qrid.VMMV_QRCode;
                        visctxt.Update(qrid);
                        int i = visctxt.SaveChanges();


                        //var qrid = visctxt.Visitor_Management_MasterVisitor_DMO.Single(t => t.VMMV_Id == mapp.VMMV_Id);

                        //string valueqr = "VM" + "#" + data.MI_Id + "#" + mapp.VMMV_Id;
                        //Bitmap bitmap = new Bitmap(100, 100);
                        //QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        //QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                        //QRCode QrCode = new QRCode(QrCodeInfo);
                        //Bitmap QrBitmap = QrCode.GetGraphic(10);
                        //byte[] byteArray = BitmapToByteArray(QrBitmap);
                        //qrid.VMMV_QRCode = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));
                        //data.ASCISS_QRCode = qrid.VMMV_QRCode;
                        //visctxt.Update(qrid);
                        //int i = visctxt.SaveChanges();





                        if (data.dept_flag == false)
                        {
                            var name = (from a in _db.HR_Master_Employee_DMO
                                        from b in _db.Multiple_Mobile_DMO
                                        from c in _db.Multiple_Email_DMO
                                        where (b.HRME_Id == a.HRME_Id && c.HRME_Id == a.HRME_Id && b.HRMEMNO_DeFaultFlag == "default"
                                        && c.HRMEM_DeFaultFlag == "default" && a.HRME_Id.Equals(data.VMVTMT_ToMeet_HRME_Id) && a.HRME_ActiveFlag == true
                                        && a.HRME_LeftFlag == false)
                                        select new AddVisitorsDTO
                                        {
                                            empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                            HRMEMNO_MobileNo = b.HRMEMNO_MobileNo,
                                            HRMEM_EmailId = c.HRMEM_EmailId,
                                        }).Distinct().SingleOrDefault();

                            //string empname = "AMAN";
                            //string empmobileno = "6200526259";
                            //string empemailid = "amankumarssm2365@gmail.com";

                            string empname = name.empname.ToString();
                            string empmobileno = name.HRMEMNO_MobileNo.ToString();
                            string empemailid = name.HRMEM_EmailId.ToString();



                            if (data.VMMV_VisitorContactNo > 0)
                            {
                                SMS sentsms = new SMS(_db);

                                sentsms.sendVisitorSms(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empname, data.VMMV_VisitorContactNo, "Visitor");

                                //sendSms(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empname, data.VMMV_VisitorContactNo, "Visitor");
                            }

                            if (data.VMMV_VisitorEmailid != null || data.VMMV_VisitorEmailid != "")
                            {
                                //    sendmail(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empname, data.VMMV_VisitorEmailid, "Visitor");

                                Email email = new Email(_db);

                                //  email.sendmailVisitorss( data.MI_Id, data.VMMV_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empname, data.VMMV_VisitorEmailid, "Visitor", data.VMMV_MeetingPurpose,"");


                               email.sendmailVisitorss(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empname, data.VMMV_VisitorEmailid, "Visitor", data.VMMV_MeetingPurpose, data.ASCISS_QRCode);




                            }

                            if (empmobileno != "" || empmobileno != null)
                            {
                                SMS sentsmsstaff = new SMS(_db);

                                //EmpSendSMSAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empmobileno, empname, "Visitor");

                                sentsmsstaff.EmpSendSMSAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empmobileno, empname, "Visitor");

                            }

                            if (empemailid != "" || empemailid != null)
                            {
                                Email email1 = new Email(_db);
                                email1.EmpSendEmail(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empemailid, empname, "Visitor", data.VMMV_MeetingPurpose,data.ASCISS_QRCode);

                                //EmpSendEmailAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empemailid, empname, "Visitor");
                            }


                            data.vis_list = (from b in visctxt.Visitor_Management_MasterVisitor_DMO
                                             from a in visctxt.MasterEmployee
                                             where (b.VMMV_Id == vis_list1.VMMV_Id && b.MI_Id == data.MI_Id && a.HRME_Id == vis_list1.VMMV_ToMeet
                                             && b.VMMV_DeptFlag == false)
                                             select new AddVisitorsDTO
                                             {
                                                 empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                 VMMV_VisitorContactNo = b.VMMV_VisitorContactNo,
                                                 VMMV_VisitorEmailid = b.VMMV_VisitorEmailid,
                                                 VMMV_VisitorName = b.VMMV_VisitorName,
                                                 VMMV_EntryDateTime = b.VMMV_EntryDateTime,
                                                 VMMV_MeetingDateTime = b.VMMV_MeetingDateTime,
                                                 VMMV_MeetingPurpose = b.VMMV_MeetingPurpose,
                                                 VMMV_FromAddress = b.VMMV_FromAddress,
                                                 VMMV_Remarks = b.VMMV_Remarks,
                                                 VMMV_FromPlace = b.VMMV_FromPlace,
                                                 VMMV_MeetingLocation = b.VMMV_MeetingLocation,
                                                 VMMV_VIPFlg = b.VMMV_VIPFlg,
                                                 VMMV_Id = b.VMMV_Id,
                                             }).ToList().ToArray();

                        }

                        else if (data.dept_flag == true)
                        {
                            data.vis_list = (from b in visctxt.Visitor_Management_MasterVisitor_DMO
                                             where (b.VMMV_Id == vis_list1.VMMV_Id && b.MI_Id == data.MI_Id && b.VMMV_DeptFlag == true)
                                             select new AddVisitorsDTO
                                             {
                                                 empname = b.VMMV_PersonToMeet,
                                                 VMMV_VisitorContactNo = b.VMMV_VisitorContactNo,
                                                 VMMV_VisitorEmailid = b.VMMV_VisitorEmailid,
                                                 VMMV_VisitorName = b.VMMV_VisitorName,
                                                 VMMV_EntryDateTime = b.VMMV_EntryDateTime,
                                                 VMMV_MeetingDateTime = b.VMMV_MeetingDateTime,
                                                 VMMV_MeetingPurpose = b.VMMV_MeetingPurpose,
                                                 VMMV_FromAddress = b.VMMV_FromAddress,
                                                 VMMV_Remarks = b.VMMV_Remarks,
                                                 VMMV_FromPlace = b.VMMV_FromPlace,
                                                 VMMV_VIPFlg = b.VMMV_VIPFlg,
                                                 VMMV_MeetingLocation = b.VMMV_MeetingLocation,
                                                 VMMV_Id = b.VMMV_Id,
                                             }).ToList().ToArray();

                            if (data.VMMV_VisitorContactNo > 0)
                            {
                                SMS sentsms = new SMS(_db);

                                //sendSms(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), data.meetpersion, data.VMMV_VisitorContactNo, "Visitor");
                                //sentsms.sendVisitorSms(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), data.meetpersion, data.VMMV_VisitorContactNo, "Visitor");
                            }

                            if (data.VMMV_VisitorEmailid != null || data.VMMV_VisitorEmailid != "")
                            {
                                Email email = new Email(_db);

                                //email.sendmailVisitorss(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), data.meetpersion, data.VMMV_VisitorEmailid, "Visitor");

                                //sendmail(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), data.meetpersion, data.VMMV_VisitorEmailid, "Visitor");
                            }
                        }

                        //qr code //mapp.VMMV_Id
                      

                    }
                    else
                    {
                        data.returnVal = "savingFailed";
                    }
                }

                else if (data.VMMV_Id > 0)
                {
                    //var checkduplicate = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.VMMV_Id != data.VMMV_Id && a.MI_Id == data.MI_Id
                    //&& a.VMMV_VisitorName == data.VMMV_VisitorName && a.VMMV_VisitorContactNo == data.VMMV_VisitorContactNo
                    //&& a.VMMV_VisitorEmailid == data.VMMV_VisitorEmailid && a.VMMV_EntryDateTime == data.VMMV_EntryDateTime
                    //&& a.VMMV_MeetingDateTime == data.VMMV_MeetingDateTime && a.VMMV_ToMeet == data.VMMV_ToMeet).ToList();
                    //if (checkduplicate.Count > 0)
                    //{
                    //    data.returnVal = "duplicate";
                    //}
                    //else
                    //{
                    var query = visctxt.Visitor_Management_MasterVisitor_DMO.Where(d => d.VMMV_Id == data.VMMV_Id).ToList();
                    if (query.Count > 0)
                    {
                        var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == data.VMMV_Id);

                        update.VMMV_VisitorName = data.VMMV_VisitorName;
                        update.VMMV_VisitorContactNo = data.VMMV_VisitorContactNo;
                        update.VMMV_VisitorEmailid = data.VMMV_VisitorEmailid;
                        update.VMMV_IdentityCardType = data.VMMV_IdentityCardType;
                        update.VMMV_CardNo = data.VMMV_CardNo;
                        update.VMMV_CardImage = data.VMMV_CardImage;
                        update.VMMV_FromPlace = data.VMMV_FromPlace;
                        update.VMMV_VisitTypeFlg = data.VMMV_VisitTypeFlg;
                        update.VMMV_FromAddress = data.VMMV_FromAddress;
                        update.VMMV_ToMeet = data.VMMV_ToMeet;
                        update.VMMV_MeetingPurpose = data.VMMV_MeetingPurpose;
                        update.VMMV_EntryDateTime = data.VMMV_EntryDateTime;
                        update.VMMV_MeetingDateTime = data.VMMV_MeetingDateTime;
                        update.VMMV_MeetingDuration = data.VMMV_MeetingDuration;
                        update.VMMV_MeetingLocation = data.VMMV_MeetingLocation;
                        update.VMMV_PersonsAccompanying = data.VMMV_PersonsAccompanying;
                        update.VMMV_AuthorisationBy = data.VMMV_AuthorisationBy;
                        update.VMMV_VisitorPhoto = data.VMMV_VisitorPhoto;
                        update.VMMV_VisitorFingerPrint = data.VMMV_VisitorFingerPrint;
                        update.VMMV_ExitDateTime = data.VMMV_ExitDateTime;
                        update.VMMV_Remarks = data.VMMV_Remarks;
                        update.VMMV_DeptFlag = data.dept_flag;
                        update.VMMV_VIPFlg = data.VMMV_VIPFlg;
                        update.VMMV_PersonToMeet = data.VMMV_PersonToMeet;
                        update.VMMV_Remarks = data.VMMV_Remarks;
                        update.UpdatedDate = DateTime.Now;
                        update.VMMV_UpdatedBy = data.UserId;
                        update.VMMV_ToMeet = data.VMVTMT_ToMeet_HRME_Id;
                        visctxt.Update(update);

                        if (data.multivisitor.Length > 0)
                        {
                            List<long> ids = new List<long>();

                            foreach (var c in data.multivisitor)
                            {
                                ids.Add(c.VMMVVI_Id);
                            }

                            Array getvalues = visctxt.Multiple_VisitorDMO.Where(a => a.VMMV_Id == data.VMMV_Id && !ids.Contains(a.VMMVVI_Id)).ToArray();

                            foreach (var d in getvalues)
                            {
                                visctxt.Remove(d);
                            }


                            foreach (var item in data.multivisitor)
                            {
                                if (item.VMMVVI_VisitorName != null && item.VMMVVI_VisitorName != "")
                                {
                                    if (item.VMMVVI_Id > 0)
                                    {
                                        var result = visctxt.Multiple_VisitorDMO.Single(a => a.VMMV_Id == data.VMMV_Id && a.VMMVVI_Id == item.VMMVVI_Id);
                                        result.VMMVVI_VisitorName = item.VMMVVI_VisitorName;
                                        result.VMMVVI_VisitorAddress = item.VMMVVI_VisitorAddress;
                                        result.VMMVVI_VisitorEmailId = item.VMMVVI_VisitorEmailId;
                                        result.VMMVVI_VisitorContactNo = item.VMMVVI_VisitorContactNo;
                                        result.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                        result.VMMVVI_UpdatedBy = data.UserId;
                                        result.VMMVVI_Updateddate = DateTime.Now;
                                        result.VMMVVI_VisitorCardNo = item.VMMVVI_VisitorCardNo;
                                        result.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                        visctxt.Update(result);
                                    }
                                    else
                                    {
                                        Multiple_VisitorDMO mvd = new Multiple_VisitorDMO();

                                        mvd.VMMV_Id = data.VMMV_Id;
                                        mvd.VMMVVI_VisitorName = item.VMMVVI_VisitorName;
                                        mvd.VMMVVI_VisitorAddress = item.VMMVVI_VisitorAddress;
                                        mvd.VMMVVI_VisitorEmailId = item.VMMVVI_VisitorEmailId;
                                        mvd.VMMVVI_VisitorContactNo = item.VMMVVI_VisitorContactNo;
                                        mvd.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                        mvd.VMMVVI_CreatedDate = DateTime.Now;
                                        mvd.VMMVVI_CreatedBy = data.UserId;
                                        mvd.VMMVVI_UpdatedBy = data.UserId;
                                        mvd.VMMVVI_Updateddate = DateTime.Now;
                                        mvd.VMMVVI_VisitorCardNo = item.VMMVVI_VisitorCardNo;
                                        mvd.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                        visctxt.Add(mvd);
                                    }
                                }
                            }
                        }

                        if (data.dept_flag == false)
                        {
                            string[] split = data.VMMV_EntryDateTime.Split(":");

                            data.fhrors = Convert.ToInt32(split[0]);
                            data.fminutes = Convert.ToInt32(split[1]);

                            var checkresult = visctxt.Visitor_Management_Visitor_ToMeetDMO.Where(a => a.MI_Id == data.MI_Id && a.VMMV_Id == data.VMMV_Id).ToList();

                            if (checkresult.Count > 0)
                            {
                                var checkresultupdate = visctxt.Visitor_Management_Visitor_ToMeetDMO.Single(a => a.MI_Id == data.MI_Id
                                && a.VMMV_Id == data.VMMV_Id && a.VMVTMT_Id == checkresult.FirstOrDefault().VMVTMT_Id);
                                checkresultupdate.VMVTMT_ToMeet_HRME_Id = data.VMVTMT_ToMeet_HRME_Id;
                                checkresultupdate.VMVTMT_Location = data.VMMV_MeetingLocation;
                                checkresultupdate.HRME_Id = data.HRME_Id;
                                checkresultupdate.VMVTMT_DateTime = data.VMMV_MeetingDateTime.AddHours(data.fhrors).AddMinutes(data.fminutes);
                                checkresultupdate.VMVTMT_UpdatedBy = data.UserId;
                                checkresultupdate.VMVTMT_UpdatedDate = DateTime.Now;
                                visctxt.Update(checkresultupdate);
                            }
                            else
                            {
                                Visitor_Management_Visitor_ToMeetDMO management_Visitor_ToMeetDMO = new Visitor_Management_Visitor_ToMeetDMO();
                                management_Visitor_ToMeetDMO.MI_Id = data.MI_Id;
                                management_Visitor_ToMeetDMO.VMMV_Id = data.VMMV_Id;
                                management_Visitor_ToMeetDMO.VMVTMT_ToMeet_HRME_Id = data.VMVTMT_ToMeet_HRME_Id;
                                management_Visitor_ToMeetDMO.VMVTMT_Location = data.VMMV_MeetingLocation;
                                management_Visitor_ToMeetDMO.HRME_Id = data.HRME_Id;
                                management_Visitor_ToMeetDMO.VMVTMT_DateTime = data.VMMV_MeetingDateTime.AddHours(data.fhrors).AddMinutes(data.fminutes);
                                management_Visitor_ToMeetDMO.VMVTMT_MetFlg = false;
                                management_Visitor_ToMeetDMO.VMVTMT_Flg = true;
                                management_Visitor_ToMeetDMO.VMVTMT_CreatedBy = data.UserId;
                                management_Visitor_ToMeetDMO.VMVTMT_UpdatedBy = data.UserId;
                                management_Visitor_ToMeetDMO.VMVTMT_CreatedDate = DateTime.Now;
                                management_Visitor_ToMeetDMO.VMVTMT_UpdatedDate = DateTime.Now;
                                visctxt.Add(management_Visitor_ToMeetDMO);

                            }
                        }

                        int s = visctxt.SaveChanges();
                        if (s > 0)
                        {
                            data.returnVal = "updated";
                        }
                        else
                        {
                            data.returnVal = "updateFailed";
                        }
                    }
                    //}


                    var vis_list1 = visctxt.Visitor_Management_MasterVisitor_DMO.OrderByDescending(d => d.VMMV_Id == data.VMMV_Id).First();

                    if (data.dept_flag == false)
                    {
                        data.vis_list = (from b in visctxt.Visitor_Management_MasterVisitor_DMO
                                         from a in visctxt.MasterEmployee
                                         where (b.VMMV_Id == data.VMMV_Id && b.MI_Id == data.MI_Id && a.HRME_Id == vis_list1.VMMV_ToMeet
                                         && b.VMMV_DeptFlag == false)
                                         select new AddVisitorsDTO
                                         {
                                             empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' '
                                             + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' +
                                             a.HRME_EmployeeLastName),
                                             VMMV_VisitorContactNo = b.VMMV_VisitorContactNo,
                                             VMMV_VisitorEmailid = b.VMMV_VisitorEmailid,
                                             VMMV_VisitorName = b.VMMV_VisitorName,
                                             VMMV_EntryDateTime = b.VMMV_EntryDateTime,
                                             VMMV_MeetingDateTime = b.VMMV_MeetingDateTime,
                                             VMMV_MeetingPurpose = b.VMMV_MeetingPurpose,
                                             VMMV_FromAddress = b.VMMV_FromAddress,
                                             VMMV_Remarks = b.VMMV_Remarks,
                                             VMMV_FromPlace = b.VMMV_FromPlace,
                                             VMMV_MeetingLocation = b.VMMV_MeetingLocation,
                                             VMMV_Id = b.VMMV_Id,
                                         }).ToList().ToArray();

                    }
                    else if (data.dept_flag == true)
                    {
                        data.vis_list = (from b in visctxt.Visitor_Management_MasterVisitor_DMO

                                         where (b.VMMV_Id == data.VMMV_Id && b.MI_Id == data.MI_Id && b.VMMV_DeptFlag == true)
                                         select new AddVisitorsDTO
                                         {
                                             empname = b.VMMV_PersonToMeet,
                                             VMMV_VisitorContactNo = b.VMMV_VisitorContactNo,
                                             VMMV_VisitorEmailid = b.VMMV_VisitorEmailid,
                                             VMMV_VisitorName = b.VMMV_VisitorName,
                                             VMMV_EntryDateTime = b.VMMV_EntryDateTime,
                                             VMMV_MeetingDateTime = b.VMMV_MeetingDateTime,
                                             VMMV_MeetingPurpose = b.VMMV_MeetingPurpose,
                                             VMMV_FromAddress = b.VMMV_FromAddress,
                                             VMMV_Remarks = b.VMMV_Remarks,
                                             VMMV_FromPlace = b.VMMV_FromPlace,
                                             VMMV_MeetingLocation = b.VMMV_MeetingLocation,
                                             VMMV_Id = b.VMMV_Id,
                                         }).ToList().ToArray();
                    }
                }
                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public AddVisitorsDTO UpdateStatus(AddVisitorsDTO data)
        {
            try
            {
                var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == data.VMMV_Id && d.MI_Id == data.MI_Id);
                update.UpdatedDate = DateTime.Now;
                update.VMMV_ExitDateTime = data.VMMV_ExitDateTime;
                update.VMMV_ExitDate = data.VMMV_ExitDate;
                update.VMMV_CkeckedInOutStatus = "Checked Out";
                update.VMMV_Remarks = data.VMMV_Remarks;

                update.VMMV_UpdatedBy = data.UserId;
                visctxt.Update(update);

                var checkresultcount = visctxt.Visitor_Management_Visitor_Appointment_DMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToList();

                if (checkresultcount.Count > 0)
                {
                    var checkresultcountd = visctxt.Visitor_Management_Appointment_DMO.Where(a => a.VMAP_Id == checkresultcount.FirstOrDefault().VMAP_Id).ToList();

                    if (checkresultcountd.Count > 0)
                    {
                        DateTime fromdatecon1 = DateTime.Now;
                        string confromdate1 = "";

                        fromdatecon1 = Convert.ToDateTime(data.VMMV_ExitDate.Value.Date.ToString("yyyy-MM-dd"));
                        confromdate1 = fromdatecon1.ToString("yyyy-MM-dd") + " " + data.VMMV_ExitDateTime;

                        var checkresult = visctxt.Visitor_Management_Appointment_DMO.Single(a => a.VMAP_Id == checkresultcount.FirstOrDefault().VMAP_Id);
                        checkresult.VMAP_ChekInOutStatus = "Check Out";
                        checkresult.VMAP_ExitDateTime = confromdate1;
                        checkresult.UpdatedDate = DateTime.Now;
                        checkresult.VMAP_UpdatedBy = data.UserId;
                        visctxt.Update(checkresult);
                    }
                }


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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO GetVisitorDetails(AddVisitorsDTO data)
        {
            try
            {
                var getdetails = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToList();
                if (getdetails.FirstOrDefault().TENANTAGR_Id != null && getdetails.FirstOrDefault().TENANTAGR_Id != 0)
                {
                    var editData = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    from e in visctxt.TenantAgreement
                                    where (e.TENANTAGR_Id == a.TENANTAGR_Id && a.MI_Id == data.MI_Id && a.VMMV_Id == data.VMMV_Id)
                                    select new AddVisitorsDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName,
                                        empname = e.TENANTAGR_TenantName,
                                        VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                        VMMV_Remarks = a.VMMV_Remarks,
                                        VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                                        MI_Id = a.MI_Id,
                                        VMMV_ExitDate = a.VMMV_ExitDate,
                                        VMMV_ExitDateTime = a.VMMV_ExitDateTime,

                                    }).Distinct().ToList();
                    data.viewdetails = editData.ToArray();
                }
                else if (getdetails.FirstOrDefault().VMMV_DeptFlag == false)
                {
                    var editData = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    from e in visctxt.MasterEmployee
                                    where (e.HRME_Id == a.VMMV_ToMeet && a.MI_Id == data.MI_Id && a.VMMV_Id == data.VMMV_Id)
                                    select new AddVisitorsDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName,
                                        empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName),
                                        VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                        VMMV_Remarks = a.VMMV_Remarks,
                                        VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                                        MI_Id = a.MI_Id,
                                        VMMV_ExitDate = a.VMMV_ExitDate,
                                        VMMV_ExitDateTime = a.VMMV_ExitDateTime,

                                    }).Distinct().ToList();
                    data.viewdetails = editData.ToArray();
                }
                else if (getdetails.FirstOrDefault().VMMV_DeptFlag == true)
                {
                    var editData = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    where (a.MI_Id == data.MI_Id && a.VMMV_Id == data.VMMV_Id)
                                    select new AddVisitorsDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName,
                                        empname = a.VMMV_PersonToMeet,
                                        VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                        VMMV_Remarks = a.VMMV_Remarks,
                                        VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                                        MI_Id = a.MI_Id,
                                        VMMV_ExitDate = a.VMMV_ExitDate,
                                        VMMV_ExitDateTime = a.VMMV_ExitDateTime,
                                    }).Distinct().ToList();
                    data.viewdetails = editData.ToArray();
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<string> sendSms(long MI_Id, string VMMV_VisitorName, DateTime? VMMV_MeetingDateTime, string VMMV_EntryDateTime, string empname, long VMMV_VisitorContactNo, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                DateTime? date = VMMV_MeetingDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMMV_VisitorName);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMMV_EntryDateTime);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        sms = result;
                    }
                }
                sms = result;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    string PHNO = VMMV_VisitorContactNo.ToString();

                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
            return "success";
        }
        public async void sendmail(long MI_Id, string VMMV_VisitorName, DateTime? VMMV_MeetingDateTime, string VMMV_EntryDateTime, string empname, string VMMV_VisitorEmailid, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                DateTime? date = VMMV_MeetingDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMMV_VisitorName);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMMV_EntryDateTime);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        Mailmsg = result_value;
                    }
                }
                Mailmsg = result_value;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        mailcc = alldetails[0].IVRM_mailcc.ToString();
                    }
                    try
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(VMMV_VisitorEmailid);
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = VMMV_VisitorEmailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        //cmd.Parameters.Add(new SqlParameter("@type",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = type
                        //});

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public async Task<string> EmpSendSMSAsync(long MI_Id, string VMMV_VisitorName, DateTime? VMMV_MeetingDateTime, string VMMV_EntryDateTime, string empmobileno, string empname, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                DateTime? date = VMMV_MeetingDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMMV_EntryDateTime);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMMV_VisitorName);
                        sms = result;
                    }
                }
                sms = result;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    string PHNO = empmobileno;

                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";
        }
        public async Task<string> EmpSendEmailAsync(long MI_Id, string VMMV_VisitorName, DateTime? VMMV_MeetingDateTime, string VMMV_EntryDateTime, string empemailid, string empname, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                DateTime? date = VMMV_MeetingDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMMV_EntryDateTime);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMMV_VisitorName);
                        Mailmsg = result_value;
                    }
                }
                Mailmsg = result_value;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        mailcc = alldetails[0].IVRM_mailcc.ToString();
                    }

                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.
                    try
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(empemailid);
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = empemailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";
        }
        public async Task<AddVisitorsDTO> changeofradio(AddVisitorsDTO data)
        {
            try
            {
                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Visitor_Type_Change_RadioButton";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                SqlDbType.VarChar)
                    {
                        Value = data.type
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.gridoptions = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (data.type == "2")
                {
                    data.VisiteSerchList = (from a in _db.TenantAgreement
                                            from b in _db.HR_Master_Building
                                            from c in _db.HR_Master_Room
                                            where (a.HRMB_Id == b.HRMB_Id && a.HRMR_Id == c.HRMR_Id && a.MI_Id == data.MI_Id)
                                            select new TenantAgreementDTO
                                            {
                                                HRMB_Id = a.HRMB_Id,
                                                HRMR_Id = a.HRMR_Id,
                                                TENANTAGR_Id = a.TENANTAGR_Id,
                                                TENANTAGR_Address = a.TENANTAGR_Address,
                                                TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                                TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                                TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                                HRMB_BuildingName = b.HRMB_BuildingName,
                                                HRMR_RoomName = c.HRMR_RoomName,
                                                TENANTAGR_LeftFlg = a.TENANTAGR_LeftFlg,
                                                TENANTAGR_Aadhar = a.TENANTAGR_Aadhar,
                                                TENANTAGR_RefundedAmount = a.TENANTAGR_RefundedAmount,
                                                TENANTAGR_RefundModeOfPayment = a.TENANTAGR_RefundModeOfPayment,
                                                TENANTAGR_Imagepath = a.TENANTAGR_Imagepath
                                            }).Distinct().ToArray();

                    data.buildinglist = _db.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id && t.HRMB_ActiveFlag == true).ToArray();
                    data.roomlist = _db.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id && t.HRMR_ActiveFlag == true).ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO getAssignDetails(AddVisitorsDTO data)
        {
            try
            {
                long roletypeid = 0;
                string roletype = "";
                var gethrmeid = _db.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

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
                                    from b in visctxt.Visitor_Management_Visitor_ToMeetDMO
                                    from c in visctxt.Institute
                                    where (a.VMMV_Id == b.VMMV_Id && c.MI_Id == a.MI_Id && a.VMMV_DeptFlag == false && a.VMMV_CkeckedInOutStatus != "Checked Out"
                                    && b.VMVTMT_ToMeet_HRME_Id == gethrmeid.FirstOrDefault().Emp_Code && b.VMVTMT_MetFlg == false)
                                    select new AddVisitorsDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName + " : " + c.MI_Name,
                                        MI_Id = a.MI_Id,
                                        VMMV_VisitorContactNo = a.VMMV_VisitorContactNo,
                                        VMMV_VisitorEmailid = a.VMMV_VisitorEmailid

                                    }).Distinct().ToArray();

                data.emplist = (from a in _db.HR_Master_Employee_DMO
                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                && a.HRME_Id != gethrmeid.FirstOrDefault().Emp_Code)
                                select new AddVisitorsDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,
                                }).Distinct().ToArray();

                data.emplistautjorizedby = (from a in _db.HR_Master_Employee_DMO
                                            where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                            select new AddVisitorsDTO
                                            {
                                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                HRME_Id = a.HRME_Id,
                                            }).Distinct().ToArray();



                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Visitor_Get_Visitor_Details_Emp";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = gethrmeid.FirstOrDefault().Emp_Code });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.assigned_visitorlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO getVisitorAssignDetails(AddVisitorsDTO data)
        {
            try
            {
                long roletypeid = 0;
                string roletype = "";
                var gethrmeid = _db.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Visitor_Get_Visitor_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@VMMV_Id", SqlDbType.VarChar)
                    {
                        Value = data.VMMV_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = gethrmeid.FirstOrDefault().Emp_Code
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.gridoptions = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO saveAssignedData(AddVisitorsDTO data)
        {
            try
            {
                long roletypeid = 0;
                string roletype = "";
                var gethrmeid = _db.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                var checkduplicate = visctxt.Visitor_Management_Visitor_ToMeetDMO.Where(a => a.VMMV_Id == data.VMMV_Id
                && a.VMVTMT_ToMeet_HRME_Id == data.VMVTMT_ToMeet_HRME_Id && a.HRME_Id == gethrmeid.FirstOrDefault().Emp_Code).ToList();

                if (checkduplicate.Count > 0)
                {
                    var checkresult = visctxt.Visitor_Management_Visitor_ToMeetDMO.Single(a => a.VMMV_Id == data.VMMV_Id
                    && a.VMVTMT_ToMeet_HRME_Id == data.VMVTMT_ToMeet_HRME_Id && a.HRME_Id == gethrmeid.FirstOrDefault().Emp_Code);
                    checkresult.VMVTMT_UpdatedBy = data.UserId;
                    checkresult.VMVTMT_UpdatedDate = DateTime.Now;
                    visctxt.Update(checkresult);
                }
                else
                {
                    Visitor_Management_Visitor_ToMeetDMO visitor_Management_Visitor_ToMeetDMO = new Visitor_Management_Visitor_ToMeetDMO();
                    visitor_Management_Visitor_ToMeetDMO.VMMV_Id = data.VMMV_Id;
                    visitor_Management_Visitor_ToMeetDMO.MI_Id = data.MI_Id;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_ToMeet_HRME_Id = data.VMVTMT_ToMeet_HRME_Id;
                    visitor_Management_Visitor_ToMeetDMO.HRME_Id = gethrmeid.FirstOrDefault().Emp_Code;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_DateTime = data.VMVTMT_DateTime.AddHours(data.fhrors).AddMinutes(data.fminutes);
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_Flg = true;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_UpdatedBy = data.UserId;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_UpdatedDate = DateTime.Now;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_CreatedBy = data.UserId;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_CreatedDate = DateTime.Now;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_Location = data.VMVTMT_Location;
                    visctxt.Add(visitor_Management_Visitor_ToMeetDMO);
                }


                var getresult = visctxt.Visitor_Management_Visitor_ToMeetDMO.Single(a => a.VMVTMT_Id == data.VMVTMT_Id);

                getresult.VMVTMT_MetFlg = data.VMVTMT_MetFlg;
                getresult.VMVTMT_Remarks = data.VMVTMT_Remarks;

                getresult.VMVTMT_DateTime = data.VMVTMT_DateTime;
                visctxt.Update(getresult);

                var i = visctxt.SaveChanges();
                if (i > 0)
                {
                    data.returnVal = "saved";

                    var vis_list1 = visctxt.Visitor_Management_MasterVisitor_DMO.OrderByDescending(d => d.VMMV_Id == data.VMMV_Id).First();

                    data.VMMV_VisitorName = vis_list1.VMMV_VisitorName;
                    data.VMMV_MeetingDateTime = vis_list1.VMMV_MeetingDateTime;
                    data.VMMV_EntryDateTime = vis_list1.VMMV_EntryDateTime;

                    if (data.dept_flag == false)
                    {
                        var name = (from a in _db.HR_Master_Employee_DMO
                                    from b in _db.Multiple_Mobile_DMO
                                    from c in _db.Multiple_Email_DMO
                                    where (b.HRME_Id == a.HRME_Id && c.HRME_Id == a.HRME_Id && a.MI_Id == data.MI_Id && b.HRMEMNO_DeFaultFlag == "default"
                                    && c.HRMEM_DeFaultFlag == "default" && a.HRME_Id.Equals(data.VMVTMT_ToMeet_HRME_Id) && a.HRME_ActiveFlag == true
                                    && a.HRME_LeftFlag == false)
                                    select new AddVisitorsDTO
                                    {
                                        empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                        HRMEMNO_MobileNo = b.HRMEMNO_MobileNo,
                                        HRMEM_EmailId = c.HRMEM_EmailId,
                                    }).Distinct().SingleOrDefault();

                        string empname = name.empname.ToString();
                        string empmobileno = name.HRMEMNO_MobileNo.ToString();
                        string empemailid = name.HRMEM_EmailId.ToString();

                        if (empmobileno != "" || empmobileno != null)
                        {
                            SMS sentsmsstaff = new SMS(_db);
                            var e = sentsmsstaff.EmpSendSMSAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empmobileno, empname, "Visitor").Result;
                        }

                        if (empemailid != "" || empemailid != null)
                        {
                            Email email1 = new Email(_db);
                            email1.EmpSendEmail(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empemailid, empname, "AssignVisitoreToEmp", data.VMMV_MeetingPurpose,data.ASCISS_QRCode);
                        }
                    }
                }
                else
                {
                    data.returnVal = "Error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO SearchAppVisitors(AddVisitorsDTO data)
        {
            try
            {
                var getvmap_id = visctxt.Visitor_Management_Visitor_Appointment_DMO.Where(a => a.MI_Id == data.MI_Id).Select(a => a.VMAP_Id).ToList();

                //var getappvistiordetails = visctxt.Visitor_Management_Appointment_DMO.Where(a => a.MI_Id == data.MI_Id
                //&& a.VMAP_ActiveFlag == true && !getvmap_id.Contains(a.VMAP_Id) && a.VMAP_Status== "Approved").ToArray();


                var getappvistiordetails = (from a in visctxt.Visitor_Management_Appointment_DMO
                                            from b in visctxt.MasterEmployee
                                            where (a.VMAP_HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.VMAP_ActiveFlag == true && !getvmap_id.Contains(a.VMAP_Id) && a.VMAP_Status == "Approved")
                                            select new AddVisitorsDTO
                                            {
                                                VMAP_ToMeet = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                                VMAP_VisitorName = a.VMAP_VisitorName,
                                                VMAP_VisitorEmailid = a.VMAP_VisitorEmailid,
                                                // VMAP_ToMeet = a.VMAP_ToMeet,
                                                VMAP_MeetingDateTime = a.VMAP_MeetingDateTime,
                                                VMAP_VisitorContactNo = a.VMAP_VisitorContactNo,
                                                VMAP_FromPlace = a.VMAP_FromPlace,
                                                VMAP_MeetingPurpose = a.VMAP_MeetingPurpose,
                                                VMAP_Remarks = a.VMAP_Remarks,
                                                VMAP_Status = a.VMAP_Status,
                                                VMAP_Id = a.VMAP_Id
                                            }).ToArray();



                data.getappvistiordetails = getappvistiordetails.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO searchfilter(AddVisitorsDTO data)
        {
            try
            {


                if (data.searchtype == "VN")
                {
                    data.visitorList = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                        where (a.MI_Id == data.MI_Id && a.VMMV_ActiveFlag == true && a.VMMV_VisitorName.Contains(data.searchfilter))
                                        select new AddVisitorsDTO
                                        {
                                            VMMV_Id = a.VMMV_Id,
                                            VMMV_VisitorName = a.VMMV_VisitorName,

                                        }).ToArray();

                    // data.visitorList = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == data.MI_Id && a.VMMV_ActiveFlag == true && a.VMMV_VisitorName.Contains(data.searchfilter)).ToArray();

                }
                else if (data.searchtype == "MN")
                {

                    data.visitorList = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                        where (a.MI_Id == data.MI_Id && a.VMMV_ActiveFlag == true && a.VMMV_VisitorContactNo.ToString().Contains(data.searchfilter))
                                        select new AddVisitorsDTO
                                        {
                                            VMMV_Id = a.VMMV_Id,
                                            VMMV_VisitorName = a.VMMV_VisitorContactNo.ToString(),

                                        }).ToArray();

                    // data.visitorList = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == data.MI_Id && a.VMMV_ActiveFlag == true && a.VMMV_VisitorContactNo== data.VMMV_VisitorContactNo).ToArray();

                }
                else if (data.searchtype == "EI")
                {

                    data.visitorList = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                        where (a.MI_Id == data.MI_Id && a.VMMV_ActiveFlag == true && a.VMMV_VisitorEmailid.Contains(data.searchfilter))
                                        select new AddVisitorsDTO
                                        {
                                            VMMV_Id = a.VMMV_Id,
                                            VMMV_VisitorName = a.VMMV_VisitorEmailid

                                        }).ToArray();
                    // data.visitorList = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == data.MI_Id && a.VMMV_ActiveFlag == true && a.VMMV_VisitorEmailid.Contains(data.searchfilter)).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public AddVisitorsDTO Getmultivisitordetails(AddVisitorsDTO data)
        {
            try
            {
                var getdetails = visctxt.Multiple_VisitorDMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToList();


                //var getdatas = (from a in visctxt.Multiple_VisitorDMO
                //                where (a.VMMV_Id == data.VMMV_Id)
                //                select new multivisitor
                //                {
                //                    VMMVVI_Id = a.VMMVVI_Id,
                //                    VMMVVI_VisitorName = a.VMMVVI_VisitorName,
                //                    VMMVVI_VisitorEmailId = a.VMMVVI_VisitorEmailId,
                //                    VMMVVI_VisitorContactNo = a.VMMVVI_VisitorContactNo,
                //                    VMMVVI_VisitorAddress = a.VMMVVI_VisitorAddress

                //                }).Distinct().ToList();

                //data.getmultivisitordetails = getdatas.ToArray();


                data.extravisitor = visctxt.Multiple_VisitorDMO.Where(e => e.VMMV_Id == data.VMMV_Id).ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO visitornameschange(AddVisitorsDTO id)
        {

            try
            {


                var feachdata = (from a in visctxt.Visitor_Management_MasterVisitor_DMO

                                 where (a.MI_Id == id.MI_Id && a.VMMV_Id == id.VMMV_Id)
                                 select a).ToList();
                id.visitornameschange = feachdata.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }
        public AddVisitorsDTO GetVisitorAssginDetails(AddVisitorsDTO id)
        {
            try
            {
                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Visitor_Get_Visitor_Assigned_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = id.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@VMMV_Id", SqlDbType.VarChar)
                    {
                        Value = id.VMMV_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        id.visitorassigndetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }
        public AddVisitorsDTO addtocart(AddVisitorsDTO id)
        {
            try
            {
                id.getAppointmentdetails = visctxt.Visitor_Management_Appointment_DMO.Where(a => a.VMAP_Id == id.VMAP_Id).ToArray();

                id.getAppointment_visitordetails = visctxt.Visitor_Management_Appointment_VisitorsDMO.Where(a => a.VMAP_Id == id.VMAP_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }

        //Tenent Visitor Managment
        public AddVisitorsDTO getTenentVisitorDetails(AddVisitorsDTO data)
        {
            try
            {
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



                var gridoptionslist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                       from e in visctxt.MasterEmployee
                                       where (a.MI_Id == data.MI_Id && a.VMMV_ToMeet == e.HRME_Id
                                       && a.VMMV_DeptFlag == false)
                                       select new AddVisitorsDTO
                                       {
                                           VMMV_Id = a.VMMV_Id,
                                           VMMV_VisitorName = a.VMMV_VisitorName,
                                           VMMV_CardNo = a.VMMV_CardNo,
                                           VMMV_CkeckedInOutStatus = a.VMMV_CkeckedInOutStatus,
                                           VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                                           VMMV_IdentityCardType = a.VMMV_IdentityCardType,
                                           VMMV_FromAddress = a.VMMV_FromAddress,
                                           VMMV_FromPlace = a.VMMV_FromPlace,
                                           VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                           VMMV_MeetingLocation = a.VMMV_MeetingLocation,
                                           VMMV_MeetingPurpose = a.VMMV_MeetingPurpose,
                                           VMMV_Remarks = a.VMMV_Remarks,
                                           VMMV_VisitorContactNo = a.VMMV_VisitorContactNo,
                                           VMMV_VisitorEmailid = a.VMMV_VisitorEmailid,
                                           VMMV_MeetingDuration = a.VMMV_MeetingDuration,
                                           MI_Id = a.MI_Id,
                                           VMMV_DeptFlag = a.VMMV_DeptFlag,
                                           CreatedDate = a.CreatedDate,
                                           empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName),
                                           staffdeptflag = a.VMMV_DeptFlag == true ? "OutSide Dept" : "Internal Dept",
                                           count_subvisitors = visctxt.Multiple_VisitorDMO.Where(z => z.VMMV_Id == a.VMMV_Id).Count()
                                       }).Distinct().OrderByDescending(a => a.CreatedDate).ToList();

                if (gridoptionslist.Count > 0)
                {
                    data.gridoptions = gridoptionslist.OrderByDescending(a => a.VMMV_Id).ToArray();
                    data.count = gridoptionslist.Count;
                }
                else
                {
                    data.count = 0;
                }

                data.VisiteSerchList = visctxt.TenantAgreement.Where(t => t.TENANTAGR_ActiveFlg == true && t.TENANTAGR_LeftFlg == false).ToArray();


                data.VisiteSerchList = (from a in visctxt.TenantAgreement
                                        from b in _db.HR_Master_Building
                                        from c in _db.HR_Master_Room
                                        where (a.HRMB_Id == b.HRMB_Id && a.HRMR_Id == c.HRMR_Id && a.MI_Id == data.MI_Id)
                                        select new TenantAgreementDTO
                                        {
                                            TENANTAGR_Address = a.TENANTAGR_Address,
                                            TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                            TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                            TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                            HRMB_BuildingName = b.HRMB_BuildingName,
                                            HRMR_RoomName = c.HRMR_RoomName,
                                            TENANTAGR_LeftFlg = a.TENANTAGR_LeftFlg,
                                            TENANTAGR_Aadhar = a.TENANTAGR_Aadhar,
                                            TENANTAGR_RefundedAmount = a.TENANTAGR_RefundedAmount,
                                            TENANTAGR_RefundModeOfPayment = a.TENANTAGR_RefundModeOfPayment,
                                            TENANTAGR_Imagepath = a.TENANTAGR_Imagepath
                                        }).Distinct().ToArray();

                data.emplistautjorizedby = (from a in _db.HR_Master_Employee_DMO
                                            where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                            select new AddVisitorsDTO
                                            {
                                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                HRME_Id = a.HRME_Id,
                                            }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


       

        public async Task<AddVisitorsDTO> QRCODECertificates(AddVisitorsDTO data)
        {

            try
            {

                var qrid = visctxt.Visitor_Management_MasterVisitor_DMO.Where(t => t.VMMV_Id == data.VMMV_Id).Count();

                string valueqr = "BC" + "#" + data.MI_Id + "#" + data.VMMV_Id + "#"  + qrid;
                Bitmap bitmap = new Bitmap(100, 100);
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(10);
                byte[] byteArray = BitmapToByteArray(QrBitmap);
                data.ASCISS_QRCode = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));


                Visitor_Management_MasterVisitor_DMO Qrcode1 = new Visitor_Management_MasterVisitor_DMO();
                Qrcode1.MI_Id = data.MI_Id;
                Qrcode1.VMMV_QRCode = data.ASCISS_QRCode;


                visctxt.Add(Qrcode1);


                //}
                visctxt.Add(Qrcode1);
                var res = visctxt.SaveChanges();

                if (res > 0)
                {
                   // data.VMMV_Id = Qrcode1.VMMV_Id;
                }

                return data;
            
            


        }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }



      


        public AddVisitorsDTO FetchDetails(AddVisitorsDTO data)
        {
            try
            {

                //   data.vis_list = visctxt.Visitor_Management_MasterVisitor_DMO.Where(V => V.MI_Id == data.MI_Id && V.VMMV_Id == data.VMMV_Id && V.VMMV_ActiveFlag == true).ToArray();
                var results = visctxt.Visitor_Management_MasterVisitor_DMO
               .Where(v => v.MI_Id == data.MI_Id && v.VMMV_Id == data.VMMV_Id && v.VMMV_ActiveFlag==true)
               .ToArray();

                data.vis_list = results;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }




    }
}