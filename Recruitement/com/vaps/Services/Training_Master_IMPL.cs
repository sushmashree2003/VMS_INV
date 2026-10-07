using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Training_Master_IMPL : Interfaces.Training_Master_Interface
    {
        private static ConcurrentDictionary<string, HR_Master_Floor_DTO> _HrMFD = new ConcurrentDictionary<string, HR_Master_Floor_DTO>();

        public VMSContext _vmsconte;
        public HRMSContext _hrms;

        HR_Master_Building MB = new HR_Master_Building();
        HR_Master_Floor MF = new HR_Master_Floor();
        HR_Master_Room MR = new HR_Master_Room();
        public Training_Master_IMPL(VMSContext vmsContext, HRMSContext hrms)
        {
            _vmsconte = vmsContext;
            _hrms = hrms;

        }
        //===============================Building==============================================
        public HR_Master_Building_DTO getdate_B(HR_Master_Building_DTO id)
        {

            try
            {


                id.building_list = _vmsconte.HR_Master_Building_con.Where(a => a.MI_Id == id.MI_Id).ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }

        public HR_Master_TrainingTopicDTO Topic_details(HR_Master_TrainingTopicDTO data)
        {
            try
            {
                data.training = _vmsconte.HR_Master_TrainingTopicDMO.Where(t => t.HRMTT_ActiveFlg == true).ToArray();

                data.getmasterdata = (from f in _vmsconte.HR_Master_TrainingTopicDMO
                                      from b in _vmsconte.HR_Master_External_TrainingType_Topic_MaterialsDMO
                                      where (f.HRMTT_Id == b.HRMTT_Id)
                                      select new HR_Master_TrainingTopicDTO
                                      {
                                          HRMTT_Topic = f.HRMTT_Topic,
                                          HRMETRTYTOPM_StudyMaterials = b.HRMETRTYTOPM_StudyMaterials,
                                          HRMETRTYTOPM_StudyMaterialsDesc = b.HRMETRTYTOPM_StudyMaterialsDesc
                                      }).OrderByDescending(a => a.HRMTT_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }
        public HR_Master_Building_DTO details_B(HR_Master_Building_DTO bt)
        {

            try
            {
                bt.building_list = _vmsconte.HR_Master_Building_con.Where(t => t.HRMB_Id == bt.HRMB_Id && t.MI_Id == bt.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return bt;
        }

        public HR_Master_Building_DTO SaveEdit_B(HR_Master_Building_DTO dto)
        {
            try

            {
                HR_Master_Building_DTO hm = new HR_Master_Building_DTO();

                if (dto.HRMB_Id > 0)
                {
                    var result = _vmsconte.HR_Master_Building_con.Single(t => t.HRMB_Id == dto.HRMB_Id);
                    result.HRMB_BuildingName = dto.HRMB_BuildingName;
                    result.HRMB_Desc = dto.HRMB_Desc;
                    result.HRMB_Agrement_Template = dto.HRMB_Agrement_Template;
                    result.MI_Id = dto.MI_Id;
                    result.HRMB_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {

                    MB.HRMB_BuildingName = dto.HRMB_BuildingName;
                    MB.HRMB_Desc = dto.HRMB_Desc;
                    MB.HRMB_Agrement_Template = dto.HRMB_Agrement_Template;
                    MB.MI_Id = dto.MI_Id;
                    MB.HRMB_ActiveFlag = true;
                    MB.HRMB_CreatedBy = dto.userId;
                    MB.CreatedDate = DateTime.Now;
                    _vmsconte.Add(MB);
                    _vmsconte.SaveChanges();
                    dto.returnvales = "Add";

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_Building_DTO de_activate_B(HR_Master_Building_DTO dto)
        {
            try
            {
                //var result = _vmsconte.HR_Master_Building_con.Single(t => t.HRMB_ActiveFlag == dto.HRMB_ActiveFlag);
                var result = _vmsconte.HR_Master_Building_con.Single(t => t.HRMB_Id == dto.HRMB_Id && t.MI_Id == dto.MI_Id);

                if (dto.HRMB_ActiveFlag == true)
                {
                    result.HRMB_ActiveFlag = false;
                    result.HRMB_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.returnval = false;
                }
                else
                {

                    result.HRMB_ActiveFlag = true;
                    result.HRMB_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.returnval = true;
                }


            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        //==================================Floor===============================================
        public HR_Master_Floor_DTO getdate_F(HR_Master_Floor_DTO Fl)
        {

            try
            {

                Fl.building_list = _vmsconte.HR_Master_Building_con.Where(a => a.HRMB_ActiveFlag == true && a.MI_Id == Fl.MI_Id).ToList().ToArray();

                Fl.floor_list = (from f in _vmsconte.HR_Master_Floor_con
                                 from b in _vmsconte.HR_Master_Building_con
                                 where (f.HRMB_Id == b.HRMB_Id && f.MI_Id == b.MI_Id && f.MI_Id == Fl.MI_Id)
                                 select new HR_Master_Floor_DTO
                                 {
                                     HRMF_Id = f.HRMF_Id
 ,
                                     HRMF_FloorName = f.HRMF_FloorName,
                                     HRMB_BuildingName = b.HRMB_BuildingName,
                                     HRMF_ActiveFlag = f.HRMF_ActiveFlag
                                 }).OrderByDescending(a => a.HRMF_Id).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return Fl;
        }
        public HR_Master_Floor_DTO SaveEdit_F(HR_Master_Floor_DTO dto)
        {
            HR_Master_Floor_DTO MF = new HR_Master_Floor_DTO();
            HR_Master_Floor mfcon = new HR_Master_Floor();
            try
            {
                if (dto.HRMF_Id > 0)
                {
                    var result = _vmsconte.HR_Master_Floor_con.Single(a => a.HRMF_Id == dto.HRMF_Id && a.MI_Id == dto.MI_Id);
                    result.HRMF_FloorName = dto.HRMF_FloorName;
                    result.HRMB_Id = dto.HRMB_Id;
                    result.MI_Id = dto.MI_Id;
                    result.HRMF_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";

                }
                else
                {
                    mfcon.HRMF_FloorName = dto.HRMF_FloorName;
                    mfcon.HRMB_Id = dto.HRMB_Id;
                    mfcon.MI_Id = dto.MI_Id;
                    mfcon.HRMF_ActiveFlag = true;
                    mfcon.HRMF_CreatedBy = dto.userId;
                    mfcon.CreatedDate = DateTime.Now;
                    _vmsconte.HR_Master_Floor_con.Add(mfcon);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public HR_Master_Floor_DTO details_F(HR_Master_Floor_DTO MF)
        {

            try
            {
                var result = _vmsconte.HR_Master_Floor_con.Single(a => a.HRMF_Id == MF.HRMF_Id && a.MI_Id == MF.MI_Id);

                MF.floor_list = (from f in _vmsconte.HR_Master_Floor_con
                                 from b in _vmsconte.HR_Master_Building_con
                                 where (f.HRMF_Id == MF.HRMF_Id && f.HRMB_Id == b.HRMB_Id)
                                 select new HR_Master_Floor_DTO
                                 {
                                     HRMF_Id = f.HRMF_Id
    ,
                                     HRMF_FloorName = f.HRMF_FloorName,
                                     HRMB_BuildingName = b.HRMB_BuildingName,
                                     HRMB_Id = f.HRMB_Id
                                 }).OrderByDescending(a => a.HRMF_Id).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return MF;
        }

        public HR_Master_Floor_DTO de_activate_F(HR_Master_Floor_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Master_Floor_con.Single(t => t.HRMF_Id == dto.HRMF_Id && t.MI_Id == dto.MI_Id);
                if (dto.HRMF_ActiveFlag == true)
                {
                    result.HRMF_ActiveFlag = false;
                    result.HRMF_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = false;
                }
                else
                {
                    result.HRMF_ActiveFlag = true;
                    result.HRMF_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = true;
                }

            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        //=============================================Room============================================

        public HR_Master_Room_DTO getdate_R(HR_Master_Room_DTO Rm)
        {
            try
            {


                Rm.freeamnlist = _vmsconte.HR_Master_Amenities_DMO.Where(e => e.MI_Id == Rm.MI_Id && e.HRMAM_ActiveFlag == true && e.HRMAM_PriceApplFlg == false).Distinct().OrderBy(d => d.HRMAM_AmenitiesName).ToArray();
                Rm.paidamnlist = _vmsconte.HR_Master_Amenities_DMO.Where(e => e.MI_Id == Rm.MI_Id && e.HRMAM_ActiveFlag == true && e.HRMAM_PriceApplFlg == true).Distinct().OrderBy(d => d.HRMAM_AmenitiesName).ToArray();




                Rm.building_list = _vmsconte.HR_Master_Building_con.Where(a => a.HRMB_ActiveFlag == true && a.MI_Id == Rm.MI_Id).ToList().ToArray();
                Rm.floor_list = _vmsconte.HR_Master_Floor_con.Where(a => a.HRMF_ActiveFlag == true && a.MI_Id == Rm.MI_Id).ToList().ToArray();



                Rm.employeelist = (
                 from f in _vmsconte.HR_Master_Employee_DMO
                 from h in _vmsconte.Multiple_Mobile_DMO
                 from i in _vmsconte.Multiple_Email_DMO

                 where (f.HRME_ActiveFlag == true && f.HRME_LeftFlag == false
                && i.HRME_Id == f.HRME_Id && h.HRMEMNO_DeFaultFlag == "default" && i.HRMEM_DeFaultFlag == "default")
                 select new HR_Master_Room_DTO
                 {
                     HRME_Id = f.HRME_Id,
                     HRME_EmployeeFirstName = f.HRME_EmployeeFirstName + " " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                     HRME_EmployeeCode = f.HRME_EmployeeCode,


                 }).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray(); ;





                Rm.room_list = (from f in _vmsconte.HR_Master_Floor_con
                                from b in _vmsconte.HR_Master_Building_con
                                from c in _vmsconte.HR_Master_Room_con

                                where (c.HRMB_Id == b.HRMB_Id && c.HRMF_Id == f.HRMF_Id && b.HRMB_ActiveFlag == true && f.HRMF_ActiveFlag == true && c.MI_Id == Rm.MI_Id && f.MI_Id == b.MI_Id && f.MI_Id == c.MI_Id)

                                select new HR_Master_Room_DTO
                                {
                                    HRMR_Id = c.HRMR_Id,
                                    HRMR_RoomName = c.HRMR_RoomName,
                                    HRMB_BuildingName = b.HRMB_BuildingName,
                                    HRMF_FloorName = f.HRMF_FloorName,
                                    HRMR_ActiveFlag = c.HRMR_ActiveFlag,
                                    HRMR_TypeFlag = c.HRMR_TypeFlag,
                                    HRMR_Capacity = c.HRMR_Capacity,
                                    HRMR_NoOfHrs = c.HRMR_NoOfHrs,
                                    HRMR_RentPerDay = c.HRMR_RentPerDay,
                                    HRMR_RentPerHour = c.HRMR_RentPerHour,
                                    HRMR_Desc = c.HRMR_Desc,
                                }).OrderByDescending(a => a.HRMR_Id).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return Rm;
        }
        public HR_Master_Room_DTO SaveEdit_R(HR_Master_Room_DTO dto)
        {


            try
            {
                if (dto.HRMR_Id > 0)
                {

                    var result = _vmsconte.HR_Master_Room_con.Single(w => w.HRMR_Id == dto.HRMR_Id);

                    result.HRMR_RoomName = dto.HRMR_RoomName;
                    result.HRMB_Id = dto.HRMB_Id;
                    result.HRMF_Id = dto.HRMF_Id;
                    result.MI_Id = dto.MI_Id;
                    result.HRMR_OutSideBookingFlg = dto.HRMR_OutSideBookingFlg;
                    result.HRMR_NoOfTaps = dto.HRMR_NoOfTaps;
                    result.HRMR_NoOfBHK = dto.HRMR_NoOfBHK;
                    result.HRMR_NoOfWashrooms = dto.HRMR_NoOfWashrooms;
                    result.HRMR_FinePerDay = dto.HRMR_FinePerDay;
                    result.HRMR_Ownername = dto.HRMR_Ownername;
                    result.HRMR_OwnerContactNo = dto.HRMR_OwnerContactNo;
                    result.HRMR_OwnerEmailID = dto.HRMR_OwnerEmailID;
                    result.HRMR_OwnerDOB = dto.HRMR_OwnerDOB;
                    result.HRMR_TypeFlag = dto.HRMR_TypeFlag;
                    result.HRMR_Desc = dto.HRMR_Desc;
                    if (dto.HRMR_OutSideBookingFlg == true)
                    {
                        result.HRMR_Capacity = dto.HRMR_Capacity;
                        result.HRMR_RentPerDay = dto.HRMR_RentPerDay;
                        result.HRMR_NoOfHrs = dto.HRMR_NoOfHrs;
                        result.HRMR_RentPerHour = dto.HRMR_RentPerHour;
                    }
                    else
                    {
                        result.HRMR_Capacity = 0;
                        result.HRMR_RentPerDay = 0;
                        result.HRMR_NoOfHrs = 0;
                        result.HRMR_RentPerHour = 0;
                    }
                    result.HRMR_ActiveFlag = true;
                    result.HRMR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);



                    var emnlist = _vmsconte.HR_Master_Room_AmenitiesDMO.Where(f => f.HRMR_Id == dto.HRMR_Id).ToList();
                    if (emnlist.Count > 0)
                    {
                        foreach (var item in emnlist)
                        {
                            _vmsconte.Remove(item);
                        }
                    }





                    var filllist = _vmsconte.HR_Master_Room_FilesDMO.Where(f => f.HRMR_Id == dto.HRMR_Id).ToList();
                    if (filllist.Count > 0)
                    {
                        foreach (var item in filllist)
                        {
                            _vmsconte.Remove(item);
                        }
                    }



                    var conlist = _vmsconte.HR_Master_Room_ContactsDMO.Where(f => f.HRMR_Id == dto.HRMR_Id).ToList();
                    if (conlist.Count > 0)
                    {
                        foreach (var item in conlist)
                        {
                            _vmsconte.Remove(item);
                        }
                    }
                    if (dto.HRMR_OutSideBookingFlg == true)
                    {
                        if (dto.paidemn.Length > 0)
                        {
                            foreach (var item in dto.paidemn)
                            {
                                HR_Master_Room_AmenitiesDMO obj = new HR_Master_Room_AmenitiesDMO();
                                obj.HRMR_Id = dto.HRMR_Id;
                                obj.HRMAM_Id = item.hrmaM_Id;
                                obj.HRMRAM_RentPerDay = item.perday;
                                obj.HRMRAM_NoOfHrs = item.noofhrs;
                                obj.HRMRAM_RentPerHour = item.perhours;
                                obj.HRMRAM_ActiveFlag = true;
                                obj.HRMRAM_CreatedDate = DateTime.Now;
                                obj.HRMRAM_UpdatedDate = DateTime.Now;
                                obj.HRMRAM_CreatedBy = dto.userId;
                                obj.HRMRAM_UpdatedBy = dto.userId;
                                _vmsconte.Add(obj);
                            }


                        }


                        if (dto.freeemn.Length > 0)
                        {
                            foreach (var item in dto.freeemn)
                            {
                                HR_Master_Room_AmenitiesDMO obj1 = new HR_Master_Room_AmenitiesDMO();
                                obj1.HRMR_Id = dto.HRMR_Id;
                                obj1.HRMAM_Id = item.hrmaM_Id;
                                obj1.HRMRAM_RentPerDay = 0;
                                obj1.HRMRAM_NoOfHrs = 0;
                                obj1.HRMRAM_RentPerHour = 0;
                                obj1.HRMRAM_ActiveFlag = true;
                                obj1.HRMRAM_CreatedDate = DateTime.Now;
                                obj1.HRMRAM_UpdatedDate = DateTime.Now;
                                obj1.HRMRAM_CreatedBy = dto.userId;
                                obj1.HRMRAM_UpdatedBy = dto.userId;
                                _vmsconte.Add(obj1);
                            }


                        }

                    }

                    if (dto.filelist.Length > 0)
                    {
                        foreach (var item in dto.filelist)
                        {

                            if (item.cfilepath != null && item.cfilepath != "")
                            {
                                HR_Master_Room_FilesDMO obb = new HR_Master_Room_FilesDMO();

                                obb.HRMR_Id = dto.HRMR_Id;
                                obb.HRMRFI_FileName = item.cfilename;
                                obb.HRMRFI_FilePath = item.cfilepath;
                                obb.HRMRFI_ActiveFlag = true;
                                obb.HRMRFI_CreatedDate = DateTime.Now;
                                obb.HRMRFI_UpdatedDate = DateTime.Now;
                                obb.HRMRFI_DefFlg = item.defflg;
                                obb.HRMRFI_CreatedBy = dto.userId;
                                obb.HRMRFI_UpdatedBy = dto.userId;
                                _vmsconte.Add(obb);
                            }


                        }
                    }

                    if (dto.HRMR_OutSideBookingFlg == true)
                    {

                        if (dto.emp.Length > 0)
                        {
                            foreach (var item in dto.emp)
                            {

                                HR_Master_Room_ContactsDMO obb1 = new HR_Master_Room_ContactsDMO();

                                obb1.HRMR_Id = dto.HRMR_Id;
                                obb1.HRME_Id = item.HRME_Id;
                                obb1.HRMRCO_ActiveFlag = true;
                                obb1.HRMRCO_CreatedDate = DateTime.Now;
                                obb1.HRMRCO_UpdatedDate = DateTime.Now;
                                obb1.HRMRCO_CreatedBy = dto.userId;
                                obb1.HRMRCO_UpdatedBy = dto.userId;
                                _vmsconte.Add(obb1);



                            }
                        }


                    }





                    int a = _vmsconte.SaveChanges();
                    if (a > 0)
                    {
                        dto.returnvalue = "Update";
                    }
                    else
                    {
                        dto.returnvalue = "Error";
                    }


                }
                else
                {

                    HR_Master_Room mfcon = new HR_Master_Room();
                    mfcon.HRMR_RoomName = dto.HRMR_RoomName;
                    mfcon.HRMB_Id = dto.HRMB_Id;
                    mfcon.HRMF_Id = dto.HRMF_Id;
                    mfcon.MI_Id = dto.MI_Id;
                    mfcon.HRMR_OutSideBookingFlg = dto.HRMR_OutSideBookingFlg;
                    mfcon.HRMR_Desc = dto.HRMR_Desc;

                    mfcon.HRMR_TypeFlag = dto.HRMR_TypeFlag;
                    mfcon.HRMR_NoOfTaps = dto.HRMR_NoOfTaps;
                    mfcon.HRMR_NoOfBHK = dto.HRMR_NoOfBHK;
                    mfcon.HRMR_NoOfWashrooms = dto.HRMR_NoOfWashrooms;
                    mfcon.HRMR_FinePerDay = dto.HRMR_FinePerDay;
                    mfcon.HRMR_Ownername = dto.HRMR_Ownername;
                    mfcon.HRMR_OwnerContactNo = dto.HRMR_OwnerContactNo;
                    mfcon.HRMR_OwnerEmailID = dto.HRMR_OwnerEmailID;
                    mfcon.HRMR_OwnerDOB = dto.HRMR_OwnerDOB;
                    if (dto.HRMR_OutSideBookingFlg == true)
                    {
                        mfcon.HRMR_Capacity = dto.HRMR_Capacity;
                        mfcon.HRMR_RentPerDay = dto.HRMR_RentPerDay;
                        mfcon.HRMR_NoOfHrs = dto.HRMR_NoOfHrs;
                        mfcon.HRMR_RentPerHour = dto.HRMR_RentPerHour;
                    }
                    else
                    {
                        mfcon.HRMR_Capacity = 0;
                        mfcon.HRMR_RentPerDay = 0;
                        mfcon.HRMR_NoOfHrs = 0;
                        mfcon.HRMR_RentPerHour = 0;
                    }
                    mfcon.HRMR_ActiveFlag = true;
                    mfcon.HRMR_CreatedBy = dto.userId;
                    mfcon.HRMR_UpdatedBy = dto.userId;
                    mfcon.CreatedDate = DateTime.Now;
                    mfcon.UpdatedDate = DateTime.Now;
                    _vmsconte.HR_Master_Room_con.Add(mfcon);
                    if (dto.HRMR_OutSideBookingFlg == true)
                    {
                        if (dto.paidemn.Length > 0)
                        {
                            foreach (var item in dto.paidemn)
                            {
                                HR_Master_Room_AmenitiesDMO obj = new HR_Master_Room_AmenitiesDMO();
                                obj.HRMR_Id = mfcon.HRMR_Id;
                                obj.HRMAM_Id = item.hrmaM_Id;
                                obj.HRMRAM_RentPerDay = item.perday;
                                obj.HRMRAM_NoOfHrs = item.noofhrs;
                                obj.HRMRAM_RentPerHour = item.perhours;
                                obj.HRMRAM_ActiveFlag = true;
                                obj.HRMRAM_CreatedDate = DateTime.Now;
                                obj.HRMRAM_UpdatedDate = DateTime.Now;
                                obj.HRMRAM_CreatedBy = dto.userId;
                                obj.HRMRAM_UpdatedBy = dto.userId;
                                _vmsconte.Add(obj);
                            }


                        }


                        if (dto.freeemn.Length > 0)
                        {
                            foreach (var item in dto.freeemn)
                            {
                                HR_Master_Room_AmenitiesDMO obj1 = new HR_Master_Room_AmenitiesDMO();
                                obj1.HRMR_Id = mfcon.HRMR_Id;
                                obj1.HRMAM_Id = item.hrmaM_Id;
                                obj1.HRMRAM_RentPerDay = 0;
                                obj1.HRMRAM_NoOfHrs = 0;
                                obj1.HRMRAM_RentPerHour = 0;
                                obj1.HRMRAM_ActiveFlag = true;
                                obj1.HRMRAM_CreatedDate = DateTime.Now;
                                obj1.HRMRAM_UpdatedDate = DateTime.Now;
                                obj1.HRMRAM_CreatedBy = dto.userId;
                                obj1.HRMRAM_UpdatedBy = dto.userId;
                                _vmsconte.Add(obj1);
                            }


                        }
                    }
                    if (dto.filelist.Length > 0)
                    {
                        foreach (var item in dto.filelist)
                        {

                            if (item.cfilepath != null && item.cfilepath != "")
                            {
                                HR_Master_Room_FilesDMO obb = new HR_Master_Room_FilesDMO();

                                obb.HRMR_Id = mfcon.HRMR_Id;
                                obb.HRMRFI_FileName = item.cfilename;
                                obb.HRMRFI_FilePath = item.cfilepath;
                                obb.HRMRFI_DefFlg = item.defflg;
                                obb.HRMRFI_ActiveFlag = true;
                                obb.HRMRFI_CreatedDate = DateTime.Now;
                                obb.HRMRFI_UpdatedDate = DateTime.Now;
                                obb.HRMRFI_CreatedBy = dto.userId;
                                obb.HRMRFI_UpdatedBy = dto.userId;
                                _vmsconte.Add(obb);
                            }


                        }
                    }
                    if (dto.HRMR_OutSideBookingFlg == true)
                    {
                        if (dto.emp.Length > 0)
                        {
                            foreach (var item in dto.emp)
                            {

                                HR_Master_Room_ContactsDMO obb1 = new HR_Master_Room_ContactsDMO();

                                obb1.HRMR_Id = mfcon.HRMR_Id;
                                obb1.HRME_Id = item.HRME_Id;
                                obb1.HRMRCO_ActiveFlag = true;
                                obb1.HRMRCO_CreatedDate = DateTime.Now;
                                obb1.HRMRCO_UpdatedDate = DateTime.Now;
                                obb1.HRMRCO_CreatedBy = dto.userId;
                                obb1.HRMRCO_UpdatedBy = dto.userId;
                                _vmsconte.Add(obb1);



                            }
                        }
                    }


                    int a = _vmsconte.SaveChanges();
                    if (a > 0)
                    {
                        dto.returnvalue = "Add";
                    }
                    else
                    {
                        dto.returnvalue = "Error";
                    }


                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public HR_Master_Room_DTO details_R(HR_Master_Room_DTO MF)
        {

            try
            {

                MF.room_list = _vmsconte.HR_Master_Room_con.Where(e => e.HRMR_Id == MF.HRMR_Id).Distinct().ToArray();



                MF.editfiles = (from a in _vmsconte.HR_Master_Room_FilesDMO

                                where (a.HRMR_Id == MF.HRMR_Id)
                                select new NAACCriteriaFivefileDTO

                                {
                                    cfileid = a.HRMRFI_Id,
                                    cfilename = a.HRMRFI_FileName,
                                    cfilepath = a.HRMRFI_FilePath,
                                    defflg = a.HRMRFI_DefFlg,

                                }).Distinct().ToArray();


                MF.employeelistedit = (
                    from f in _vmsconte.HR_Master_Employee_DMO
                    from g in _vmsconte.HR_Master_Room_ContactsDMO
                    where (f.HRME_ActiveFlag == true && f.HRME_LeftFlag == false
                   && f.HRME_Id == g.HRME_Id && g.HRMR_Id == MF.HRMR_Id)
                    select new HR_Master_Room_DTO
                    {
                        HRME_Id = f.HRME_Id,
                        HRME_EmployeeFirstName = f.HRME_EmployeeFirstName + " " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                        HRME_EmployeeCode = f.HRME_EmployeeCode,
                        select1 = true,


                    }).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray(); ;


                MF.editamenitiespaid = (from a in _vmsconte.HR_Master_Room_AmenitiesDMO
                                        from b in _vmsconte.HR_Master_Amenities_DMO
                                        where a.HRMR_Id == MF.HRMR_Id && a.HRMAM_Id == b.HRMAM_Id && b.HRMAM_ActiveFlag == true && b.HRMAM_PriceApplFlg == true
                                        select new paidemn
                                        {
                                            hrmraM_Id = a.HRMRAM_Id,
                                            hrmaM_Id = b.HRMAM_Id,
                                            noofhrs = a.HRMRAM_NoOfHrs,
                                            perday = a.HRMRAM_RentPerDay,
                                            perhours = a.HRMRAM_RentPerHour,
                                            amn_name = b.HRMAM_AmenitiesName,
                                        }).ToArray();

                MF.editamenitiesfree = (from a in _vmsconte.HR_Master_Room_AmenitiesDMO
                                        from b in _vmsconte.HR_Master_Amenities_DMO
                                        where a.HRMR_Id == MF.HRMR_Id && a.HRMAM_Id == b.HRMAM_Id && b.HRMAM_ActiveFlag == true && b.HRMAM_PriceApplFlg == false
                                        select new paidemn
                                        {
                                            hrmraM_Id = a.HRMRAM_Id,
                                            hrmaM_Id = b.HRMAM_Id,
                                            noofhrs = a.HRMRAM_NoOfHrs,
                                            perday = a.HRMRAM_RentPerDay,
                                            perhours = a.HRMRAM_RentPerHour,
                                            amn_name = b.HRMAM_AmenitiesName,
                                        }).ToArray();




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return MF;
        }
        public HR_Master_Room_DTO de_activate_R(HR_Master_Room_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Master_Room_con.Single(t => t.HRMR_Id == dto.HRMR_Id && t.MI_Id == dto.MI_Id);
                if (dto.HRMR_ActiveFlag == true)
                {
                    result.HRMR_ActiveFlag = false;
                    result.HRMR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = false;
                }
                else
                {
                    result.HRMR_ActiveFlag = true;
                    result.HRMR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = true;
                }

            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }


        public HR_Master_Room_DTO viewuploadflies(HR_Master_Room_DTO data)
        {
            try
            {
                data.editfiles = (from a in _vmsconte.HR_Master_Room_FilesDMO
                                  where (a.HRMR_Id == data.HRMR_Id)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.HRMR_Id,
                                      cfileid = a.HRMRFI_Id,
                                      cfilename = a.HRMRFI_FileName,
                                      cfilepath = a.HRMRFI_FilePath,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HR_Master_Room_DTO viewamnity(HR_Master_Room_DTO data)
        {
            try
            {
                data.editamenitiesfree = (from a in _vmsconte.HR_Master_Room_AmenitiesDMO
                                          from b in _vmsconte.HR_Master_Amenities_DMO
                                          where a.HRMR_Id == data.HRMR_Id && a.HRMAM_Id == b.HRMAM_Id && b.HRMAM_ActiveFlag == true
                                          select new HR_Master_Room_DTO
                                          {
                                              HRMAM_AmenitiesName = b.HRMAM_AmenitiesName,
                                              HRMAM_AmenitiesDes = b.HRMAM_AmenitiesDes,
                                              HRMRAM_Id = a.HRMRAM_Id,
                                              HRMAM_Id = b.HRMAM_Id,
                                              HRMR_NoOfHrs = a.HRMRAM_NoOfHrs,
                                              HRMR_RentPerDay = a.HRMRAM_RentPerDay,
                                              HRMR_RentPerHour = a.HRMRAM_RentPerHour,
                                              HRMAM_PriceApplFlg = Convert.ToBoolean(b.HRMAM_PriceApplFlg)
                                          }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public HR_Master_Room_DTO viewcontact(HR_Master_Room_DTO data)
        {
            try
            {




                data.employeelist = (
                   from f in _vmsconte.HR_Master_Employee_DMO
                   from g in _vmsconte.HR_Master_Room_ContactsDMO
                   from h in _vmsconte.Multiple_Mobile_DMO
                   from i in _vmsconte.Multiple_Email_DMO
                   where (f.HRME_ActiveFlag == true && f.HRME_LeftFlag == false
                  && f.HRME_Id == g.HRME_Id && g.HRMR_Id == data.HRMR_Id && h.HRME_Id == f.HRME_Id && i.HRME_Id == f.HRME_Id && h.HRMEMNO_DeFaultFlag == "default" && i.HRMEM_DeFaultFlag == "default")
                   select new HR_Master_Room_DTO
                   {
                       HRME_Id = f.HRME_Id,
                       HRMRCO_Id = g.HRMRCO_Id,
                       HRME_EmployeeFirstName = f.HRME_EmployeeFirstName + " " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                       HRME_EmployeeCode = f.HRME_EmployeeCode,
                       mobileno = h.HRMEMNO_MobileNo,
                       emailid = i.HRMEM_EmailId,


                   }).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HR_Master_Room_DTO deleteuploadfile(HR_Master_Room_DTO data)
        {
            try
            {

                var list = _vmsconte.HR_Master_Room_FilesDMO.Where(f => f.HRMRFI_Id == data.HRMRFI_Id).ToList();

                foreach (var item in list)
                {
                    _vmsconte.Remove(item);
                }

                int a = _vmsconte.SaveChanges();
                if (a > 0)
                {
                    data.returnval = true;
                }

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HR_Master_Room_DTO deleteamn(HR_Master_Room_DTO data)
        {
            try
            {

                var list = _vmsconte.HR_Master_Room_AmenitiesDMO.Where(f => f.HRMRAM_Id == data.HRMRAM_Id).ToList();

                foreach (var item in list)
                {
                    _vmsconte.Remove(item);
                }

                int a = _vmsconte.SaveChanges();
                if (a > 0)
                {
                    data.returnval = true;
                }

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HR_Master_Room_DTO deletecontact(HR_Master_Room_DTO data)
        {
            try
            {

                var list = _vmsconte.HR_Master_Room_ContactsDMO.Where(f => f.HRMRCO_Id == data.HRMRCO_Id).ToList();

                foreach (var item in list)
                {
                    _vmsconte.Remove(item);
                }

                int a = _vmsconte.SaveChanges();
                if (a > 0)
                {
                    data.returnval = true;
                }

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        // =======================================Training============================================
        public HR_Master_External_Trainer_Creation_DTO getdata_T(HR_Master_External_Trainer_Creation_DTO dto)
        {

            try
            {


                dto.qualification_list = _hrms.Master_Employee_Qulaification.Where(a => a.MI_Id == dto.MI_Id).ToList().ToArray();
                dto.gender_list = _vmsconte.IVRM_Master_Gender.Where(a => a.MI_Id == dto.MI_Id && a.IVRMMG_ActiveFlag == true).ToList().ToArray();
                //Rm.training_list = _vmsconte.HR_Trainer_Creation_con.ToList().ToArray();
                dto.training_list = (from a in _vmsconte.Master_Employee_Qulaification_con
                                     from b in _vmsconte.IVRM_Master_Gender
                                     from c in _vmsconte.HR_Master_External_Trainer_Creation_DMO_con
                                     where (a.HRMEQ_Id == c.HRMEQ_Id && b.IVRMMG_Id == c.IVRMMG_Id && c.MI_Id == dto.MI_Id)
                                     select new HR_Master_External_Trainer_Creation_DTO
                                     {
                                         HRMETR_Id = c.HRMETR_Id,
                                         HRMETR_Name = c.HRMETR_Name,
                                         HRMETR_Address = c.HRMETR_Address,
                                         HRMETR_ContactNo = c.HRMETR_ContactNo,
                                         HRMETR_Skills = c.HRMETR_Skills,
                                         HRMETR_ActiveFlag = c.HRMETR_ActiveFlag,
                                         HRME_QualificationName = a.HRME_QualificationName,
                                         HRMEQ_Id = c.HRMEQ_Id,
                                         HRMETR_DOB = c.HRMETR_DOB
                                     }).OrderByDescending(a => a.HRMETR_Id).ToList().ToArray();

                //Rm.training_list = _vmsconte.HR_Trainer_Creation_con.Where(a => a.HRTRC_Id == Rm.HRTRC_Id).Distinct().ToArray();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TrainingIssueObservedList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.BigInt)
                    {
                        Value = dto.userId
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.observedIssuesList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.train_list = _hrms.MasterEmployee.Where(a => a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_External_Trainer_Creation_DTO SaveEdit_T(HR_Master_External_Trainer_Creation_DTO dto)
        {
            HR_Master_External_Trainer_Creation_DTO MF = new HR_Master_External_Trainer_Creation_DTO();
            HR_Master_External_Trainer_Creation_DTO mfcon = new HR_Master_External_Trainer_Creation_DTO();
            HR_Master_External_Trainer_Creation_DMO t = new HR_Master_External_Trainer_Creation_DMO();
            try
            {
                if (dto.HRMETR_Id > 0)
                {
                    var result = _vmsconte.HR_Master_External_Trainer_Creation_DMO_con.Single(a => a.HRMETR_Id == dto.HRMETR_Id && a.MI_Id == dto.MI_Id);
                    result.HRMETR_Name = dto.HRMETR_Name;
                    result.HRMETR_ImagePath = dto.HRMETR_ImagePath;
                    result.HRMETR_Image_Name = dto.HRMETR_Image_Name;
                    result.IVRMMG_Id = dto.IVRMMG_Id;
                    result.HRMETR_DOB = dto.HRMETR_DOB;
                    result.HRMETR_Address = dto.HRMETR_Address;
                    result.HRMETR_City = dto.HRMETR_City;
                    result.HRMETR_Pincode = dto.HRMETR_Pincode;
                    result.HRMETR_EmailId = dto.HRMETR_EmailId;
                    result.HRMETR_ContactNo = dto.HRMETR_ContactNo;
                    result.HRMETR_DomainExp = dto.HRMETR_DomainExp;
                    result.HRMETR_TrainingExp = dto.HRMETR_TrainingExp;
                    result.HRMETR_Skills = dto.HRMETR_Skills;
                    result.HRMEQ_Id = dto.HRMEQ_Id;
                    result.HRMETR_ParttimeORFullTimeFlg = dto.HRMETR_ParttimeORFullTimeFlg;
                    result.HRMETR_Remarks = dto.HRMETR_Remarks;
                    result.HRMETR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.HR_Master_External_Trainer_Creation_DMO_con.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";

                }
                else
                {
                    t.HRMETR_Name = dto.HRMETR_Name;
                    t.HRMETR_ImagePath = dto.HRMETR_ImagePath;
                    t.HRMETR_Image_Name = dto.HRMETR_Image_Name;
                    t.IVRMMG_Id = dto.IVRMMG_Id;
                    t.HRMETR_DOB = dto.HRMETR_DOB;
                    t.HRMETR_Address = dto.HRMETR_Address;
                    t.HRMETR_City = dto.HRMETR_City;
                    t.HRMETR_Pincode = dto.HRMETR_Pincode;
                    t.HRMETR_EmailId = dto.HRMETR_EmailId;
                    t.HRMETR_ContactNo = dto.HRMETR_ContactNo;
                    t.HRMETR_DomainExp = dto.HRMETR_DomainExp;
                    t.HRMETR_TrainingExp = dto.HRMETR_TrainingExp;
                    t.HRMETR_Skills = dto.HRMETR_Skills;
                    t.HRMEQ_Id = dto.HRMEQ_Id;
                    t.HRMETR_ActiveFlag = true;
                    t.HRMETR_ParttimeORFullTimeFlg = dto.HRMETR_ParttimeORFullTimeFlg;
                    t.HRMETR_Remarks = dto.HRMETR_Remarks;
                    t.HRMETR_CreatedBy = dto.userId;
                    t.CreatedDate = DateTime.Now;
                    t.MI_Id = dto.MI_Id;
                    _vmsconte.HR_Master_External_Trainer_Creation_DMO_con.Add(t);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public HR_Master_External_Trainer_Creation_DTO details_T(HR_Master_External_Trainer_Creation_DTO MF)
        {

            try
            {


                MF.train_list = (from a in _vmsconte.Master_Employee_Qulaification_con
                                 from b in _vmsconte.IVRM_Master_Gender
                                 from c in _vmsconte.HR_Master_External_Trainer_Creation_DMO_con
                                 where (a.HRMEQ_Id == c.HRMEQ_Id && b.IVRMMG_Id == c.IVRMMG_Id && c.HRMETR_Id == MF.HRMETR_Id && c.MI_Id == MF.MI_Id)
                                 select new HR_Master_External_Trainer_Creation_DTO
                                 {
                                     HRMETR_Id = c.HRMETR_Id,
                                     HRMETR_Name = c.HRMETR_Name,
                                     HRMETR_Image_Name = c.HRMETR_Image_Name,
                                     IVRMMG_Id = c.IVRMMG_Id,
                                     HRMETR_DOB = Convert.ToDateTime(c.HRMETR_DOB),
                                     HRMETR_Address = c.HRMETR_Address,
                                     HRMETR_City = c.HRMETR_City,
                                     HRMETR_Pincode = c.HRMETR_Pincode,
                                     HRMETR_EmailId = c.HRMETR_EmailId,
                                     HRMETR_ContactNo = c.HRMETR_ContactNo,
                                     HRMETR_DomainExp = c.HRMETR_DomainExp,
                                     HRMETR_TrainingExp = c.HRMETR_TrainingExp,
                                     HRMETR_Skills = c.HRMETR_Skills,
                                     HRMEQ_Id = c.HRMEQ_Id,
                                     HRMETR_ParttimeORFullTimeFlg = c.HRMETR_ParttimeORFullTimeFlg,
                                     HRMETR_Remarks = c.HRMETR_Remarks,
                                     IVRMMG_GenderName = b.IVRMMG_GenderName,
                                     HRME_QualificationName = a.HRME_QualificationName,
                                     HRMETR_ImagePath = c.HRMETR_ImagePath

                                 }).ToList().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return MF;
        }
        public HR_Master_External_Trainer_Creation_DTO deactivate_T(HR_Master_External_Trainer_Creation_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Master_External_Trainer_Creation_DMO_con.Single(a => a.HRMETR_Id == dto.HRMETR_Id && a.MI_Id == dto.MI_Id);
                if (dto.HRMETR_ActiveFlag == true)
                {
                    result.HRMETR_ActiveFlag = false;
                    result.HRMETR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = false;
                }
                else
                {
                    result.HRMETR_ActiveFlag = true;
                    result.HRMETR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = true;
                }

            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }
        public HR_Master_External_Trainer_Creation_DTO Save_TM(HR_Master_External_Trainer_Creation_DTO dto)
        {

            try
            {
                var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("Training_Monitor_Observation_Insert @p0,@p1,@p2,@p3,@p4,@p5", dto.HRMETR_Remarks, dto.HRMETR_DOB.ToString("yyyy-MM-dd"), dto.userId, dto.HRME_Id, dto.HRMETR_Name, dto.IVRMMG_GenderName);
                if (contactExistsP > 0)
                {
                    dto.returnvalue = "Success";
                }
                else
                {
                    dto.returnvalue = "NotSuccess";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        // =======================================Master Question============================================
        public HR_Master_Feedback_Qns_DTO getdata_question(HR_Master_Feedback_Qns_DTO dto)
        {
            try
            {
                dto.question_list = _vmsconte.HR_Master_Feedback_Qns_DMO_con.Where(a => a.MI_Id == dto.MI_Id).OrderByDescending(a => a.HRMFQNS_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_Feedback_Qns_DTO SaveEdit_question(HR_Master_Feedback_Qns_DTO dto)
        {

            try
            {
                if (dto.HRMFQNS_Id > 0)
                {
                    var result = _vmsconte.HR_Master_Feedback_Qns_DMO_con.Single(a => a.HRMFQNS_Id == dto.HRMFQNS_Id && a.MI_Id == dto.MI_Id);

                    result.HRMFQNS_QuestionName = dto.HRMFQNS_QuestionName;
                    result.HRMFQNS_QuestionTypeFlg = dto.HRMFQNS_QuestionTypeFlg;
                    result.HRMFQNS_QuestionOrder = dto.HRMFQNS_QuestionOrder;
                    result.HRMFQNS_QuestionForFlg = dto.HRMFQNS_QuestionForFlg;
                    result.HRMFQNS_UpdatedBy = dto.userId;
                    result.HRMFQNS_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else
                {
                    HR_Master_Feedback_Qns_DMO t = new HR_Master_Feedback_Qns_DMO();
                    t.HRMFQNS_QuestionName = dto.HRMFQNS_QuestionName;
                    t.HRMFQNS_QuestionTypeFlg = dto.HRMFQNS_QuestionTypeFlg;
                    t.HRMFQNS_QuestionOrder = dto.HRMFQNS_QuestionOrder;
                    t.HRMFQNS_QuestionForFlg = dto.HRMFQNS_QuestionForFlg;
                    t.HRMFQNS_ActiveFlg = true;
                    t.HRMFQNS_CreatedBy = dto.userId;
                    t.HRMFQNS_CreatedDate = DateTime.Now;
                    t.MI_Id = dto.MI_Id;
                    _vmsconte.Add(t);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public HR_Master_Feedback_Qns_DTO details_question(HR_Master_Feedback_Qns_DTO dto)
        {
            try
            {
                dto.question_details_list = _vmsconte.HR_Master_Feedback_Qns_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMFQNS_Id == dto.HRMFQNS_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_Feedback_Qns_DTO deactivate_question(HR_Master_Feedback_Qns_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Master_Feedback_Qns_DMO_con.Single(a => a.HRMFQNS_Id == dto.HRMFQNS_Id && a.MI_Id == dto.MI_Id);
                if (dto.HRMFQNS_ActiveFlg == true)
                {
                    result.HRMFQNS_ActiveFlg = false;
                    result.HRMFQNS_UpdatedBy = dto.userId;
                    result.HRMFQNS_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = false;
                }
                else
                {
                    result.HRMFQNS_ActiveFlg = true;
                    result.HRMFQNS_UpdatedBy = dto.userId;
                    result.HRMFQNS_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = true;
                }

            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        // =======================================Master Feedback option============================================
        public HR_Master_Feedback_Option_DTO getdata_MFO(HR_Master_Feedback_Option_DTO dto)
        {
            try
            {
                dto.feedback_option_list = _vmsconte.HR_Master_Feedback_Option_DMO_con.Where(a => a.MI_Id == dto.MI_Id).OrderByDescending(a => a.HRMFOPT_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_Feedback_Option_DTO SaveEdit_MFO(HR_Master_Feedback_Option_DTO dto)
        {

            try
            {
                if (dto.HRMFOPT_Id > 0)
                {
                    var result = _vmsconte.HR_Master_Feedback_Option_DMO_con.Single(a => a.HRMFOPT_Id == dto.HRMFOPT_Id && a.MI_Id == dto.MI_Id);
                    result.HRMFOPT_OptionName = dto.HRMFOPT_OptionName;
                    result.HRMFOPT_OptionOrder = dto.HRMFOPT_OptionOrder;
                    result.HRMFOPT_OptionFor = dto.HRMFOPT_OptionFor;
                    result.HRMFOPT_UpdatedBy = dto.userId;
                    result.HRMFOPT_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";

                }
                else
                {
                    HR_Master_Feedback_Option_DMO t = new HR_Master_Feedback_Option_DMO();
                    t.HRMFOPT_OptionName = dto.HRMFOPT_OptionName;
                    t.HRMFOPT_OptionOrder = dto.HRMFOPT_OptionOrder;
                    t.HRMFOPT_OptionFor = dto.HRMFOPT_OptionFor;
                    t.HRMFOPT_ActiveFlg = true;
                    t.HRMFOPT_CreatedBy = dto.userId;
                    t.HRMFOPT_CreatedDate = DateTime.Now;
                    t.MI_Id = dto.MI_Id;
                    _vmsconte.Add(t);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public HR_Master_Feedback_Option_DTO details_MFO(HR_Master_Feedback_Option_DTO dto)
        {
            try
            {
                dto.feedback_option_details_list = _vmsconte.HR_Master_Feedback_Option_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMFOPT_Id == dto.HRMFOPT_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_Feedback_Option_DTO deactivate_MFO(HR_Master_Feedback_Option_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Master_Feedback_Option_DMO_con.Single(a => a.HRMFOPT_Id == dto.HRMFOPT_Id && a.MI_Id == dto.MI_Id);
                if (dto.HRMFOPT_ActiveFlg == true)
                {
                    result.HRMFOPT_ActiveFlg = false;
                    result.HRMFOPT_UpdatedBy = dto.userId;
                    result.HRMFOPT_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = false;
                }
                else
                {
                    result.HRMFOPT_ActiveFlg = true;
                    result.HRMFOPT_UpdatedBy = dto.userId;
                    result.HRMFOPT_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = true;
                }

            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        // =======================================Master Question option============================================
        public HR_Master_Question_Option_DTO getdata_MQO(HR_Master_Question_Option_DTO dto)
        {
            try
            {
                dto.question_option_list = (from a in _vmsconte.HR_Master_Question_Option_DMO_con
                                            from b in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                            from c in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                            where a.HRMFQNS_Id == b.HRMFQNS_Id && a.HRMFOPT_Id == c.HRMFOPT_Id && a.MI_Id == c.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id
                                            select new HR_Master_Question_Option_DTO
                                            {
                                                HRMFQNS_Id = b.HRMFQNS_Id,
                                                HRMFQNS_QuestionName = b.HRMFQNS_QuestionName,
                                                HRMQNOP_ActiveFlg = a.HRMQNOP_ActiveFlg
                                            }).Distinct().ToArray();

                dto.option_list = _vmsconte.HR_Master_Feedback_Option_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMFOPT_ActiveFlg == true).ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Question_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.question_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_Question_Option_DTO SaveEdit_MQO(HR_Master_Question_Option_DTO dto)
        {

            try
            {
                if (dto.hqn_id > 0)
                {
                    var result = _vmsconte.HR_Master_Question_Option_DMO_con.Where(a => a.HRMFQNS_Id == dto.HRMFQNS_Id && a.MI_Id == dto.MI_Id).ToList();
                    foreach (var item in result)
                    {
                        var result1 = _vmsconte.HR_Master_Question_Option_DMO_con.Single(a => a.HRMQNOP_Id == item.HRMQNOP_Id && a.MI_Id == dto.MI_Id);
                        _vmsconte.Remove(result1);
                    }
                    foreach (var item in dto.Feedback_Option)
                    {
                        HR_Master_Question_Option_DMO tot = new HR_Master_Question_Option_DMO();
                        tot.HRMFQNS_Id = dto.HRMFQNS_Id;
                        tot.HRMFOPT_Id = item.HRMFOPT_Id;
                        tot.HRMQNOP_ActiveFlg = true;
                        tot.MI_Id = dto.MI_Id;
                        tot.HRMQNOP_CreatedBy = dto.userId;
                        tot.HRMQNOP_CreatedDate = DateTime.Now;
                        _vmsconte.Add(tot);
                    }


                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";

                }
                else
                {

                    foreach (var item in dto.Feedback_Option)
                    {
                        HR_Master_Question_Option_DMO tot = new HR_Master_Question_Option_DMO();
                        tot.HRMFQNS_Id = dto.HRMFQNS_Id;
                        tot.HRMFOPT_Id = item.HRMFOPT_Id;
                        tot.HRMQNOP_ActiveFlg = true;
                        tot.MI_Id = dto.MI_Id;
                        tot.HRMQNOP_CreatedBy = dto.userId;
                        tot.HRMQNOP_CreatedDate = DateTime.Now;
                        _vmsconte.Add(tot);
                    }


                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public HR_Master_Question_Option_DTO details_MQO(HR_Master_Question_Option_DTO dto)
        {
            try
            {
                dto.question_details_list = (from a in _vmsconte.HR_Master_Question_Option_DMO_con
                                             from b in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                             where a.HRMFQNS_Id == b.HRMFQNS_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id && a.HRMFQNS_Id == dto.HRMFQNS_Id && a.MI_Id == dto.MI_Id
                                             select new HR_Master_Question_Option_DTO
                                             {
                                                 HRMFQNS_Id = b.HRMFQNS_Id,
                                                 HRMFQNS_QuestionName = b.HRMFQNS_QuestionName,

                                             }).Distinct().ToArray();

                dto.option_details_list = (from a in _vmsconte.HR_Master_Question_Option_DMO_con
                                           from b in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                           where a.HRMFOPT_Id == b.HRMFOPT_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id && a.HRMFQNS_Id == dto.HRMFQNS_Id && a.MI_Id == dto.MI_Id
                                           select new HR_Master_Question_Option_DTO
                                           {
                                               HRMFOPT_Id = b.HRMFOPT_Id,
                                               HRMFOPT_OptionName = b.HRMFOPT_OptionName,

                                           }).ToArray();
                dto.option_list = _vmsconte.HR_Master_Feedback_Option_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMFOPT_ActiveFlg == true).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_Question_Option_DTO deactivate_MQO(HR_Master_Question_Option_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Master_Question_Option_DMO_con.Where(a => a.HRMFQNS_Id == dto.HRMFQNS_Id && a.MI_Id == dto.MI_Id).ToList();
                foreach (var item in result)
                {
                    var result1 = _vmsconte.HR_Master_Question_Option_DMO_con.Single(a => a.HRMQNOP_Id == item.HRMQNOP_Id && a.MI_Id == dto.MI_Id);

                    if (dto.HRMQNOP_ActiveFlg == true)
                    {
                        result1.HRMQNOP_ActiveFlg = false;
                        result1.HRMQNOP_UpdatedBy = dto.userId;
                        result1.HRMQNOP_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result1);
                        dto.returnval = false;
                    }
                    else
                    {
                        result1.HRMQNOP_ActiveFlg = true;
                        result1.HRMQNOP_UpdatedBy = dto.userId;
                        result1.HRMQNOP_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result1);
                        dto.returnval = true;
                    }
                }

                _vmsconte.SaveChanges();



            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        public HR_Master_Question_Option_DTO option_view_MQO(HR_Master_Question_Option_DTO dto)
        {
            try
            {
                dto.option_view_list = (from a in _vmsconte.HR_Master_Question_Option_DMO_con
                                        from b in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                        where a.HRMFOPT_Id == b.HRMFOPT_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id && a.HRMFQNS_Id == dto.HRMFQNS_Id
                                        select new HR_Master_Question_Option_DTO
                                        {
                                            HRMFOPT_OptionName = b.HRMFOPT_OptionName,
                                            HRMFOPT_ActiveFlg = b.HRMFOPT_ActiveFlg
                                        }).ToArray();
            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }


        // =======================================Training Question mapping============================================
        public HR_Training_Question_DTO getdata_TQM(HR_Training_Question_DTO dto)
        {
            try
            {
                //dto.training_question_mapping_list = (from a in _vmsconte.HR_Training_Question_DMO_con
                //                                      from b in _vmsconte.HR_Master_TrainingTopicDMO
                //                                      from c in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                //                                      where a.HRMTT_Id == b.HRMTT_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                //                                      select new HR_Training_Question_DTO
                //                                      {
                //                                          HRMTT_Id = b.HRMTT_Id,
                //                                          HRMFQNS_QuestionName = c.HRMFQNS_QuestionName,
                //                                          HRMTT_Topic=b.HRMTT_Topic,
                //                                          HRTRQNS_ActiveFlg = a.HRTRQNS_ActiveFlg
                //                                      }).Distinct().ToArray();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Master_TrainingTopic_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.training_question_mapping_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                dto.question_list = _vmsconte.HR_Master_Feedback_Qns_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMFQNS_ActiveFlg == true).ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Training_Question_Mapping_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.training_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Training_Question_DTO SaveEdit_TQM(HR_Training_Question_DTO dto)
        {
            try
            {
                if (dto.hqn_id > 0)
                {
                    var result = _vmsconte.HR_Training_Question_DMO_con.Where(a => a.HRMTT_Id == dto.HRMTT_Id && a.MI_Id == dto.MI_Id).ToList();
                    foreach (var item in result)
                    {
                        var result1 = _vmsconte.HR_Training_Question_DMO_con.Single(a => a.HRMFQNS_Id == item.HRMFQNS_Id && a.HRMTT_Id == dto.HRMTT_Id && a.MI_Id == dto.MI_Id);
                        _vmsconte.Remove(result1);
                    }
                    foreach (var item in dto.question_Option)
                    {
                        HR_Training_Question_DMO tot = new HR_Training_Question_DMO();
                        tot.HRMTT_Id = dto.HRMTT_Id;
                        tot.HRMFQNS_Id = item.HRMFQNS_Id;
                        tot.HRTRQNS_ActiveFlg = true;
                        tot.MI_Id = dto.MI_Id;
                        tot.HRTRQNS_UpdatedBy = dto.userId;
                        tot.HRTRQNS_UpdatedDate = DateTime.Now;
                        _vmsconte.Add(tot);
                    }
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else
                {
                    foreach (var item in dto.question_Option)
                    {
                        HR_Training_Question_DMO tot = new HR_Training_Question_DMO();
                        tot.HRMTT_Id = dto.HRMTT_Id;
                        tot.HRMFQNS_Id = item.HRMFQNS_Id;
                        tot.HRTRQNS_ActiveFlg = true;
                        tot.MI_Id = dto.MI_Id;
                        tot.HRTRQNS_CreatedBy = dto.userId;
                        tot.HRTRQNS_CreatedDate = DateTime.Now;
                        _vmsconte.Add(tot);
                    }
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Training_Question_DTO details_TQM(HR_Training_Question_DTO dto)
        {
            try
            {
                dto.training_question_details_list = (from a in _vmsconte.HR_Training_Question_DMO_con
                                                      from b in _vmsconte.HR_Master_TrainingTopicDMO
                                                      where a.HRMTT_Id == b.HRMTT_Id && a.MI_Id == dto.MI_Id && a.HRMTT_Id == dto.HRMTT_Id
                                                      select new HR_Training_Question_DTO
                                                      {
                                                          HRMTT_Id = b.HRMTT_Id,
                                                          HRMTT_Topic = b.HRMTT_Topic,
                                                      }).Distinct().ToArray();

                dto.question_details_list = (from a in _vmsconte.HR_Training_Question_DMO_con
                                             from b in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                             where a.HRMFQNS_Id == b.HRMFQNS_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id && a.HRMTT_Id == dto.HRMTT_Id && a.MI_Id == dto.MI_Id
                                             select new HR_Training_Question_DTO
                                             {
                                                 HRMFQNS_Id = b.HRMFQNS_Id,
                                                 HRMFQNS_QuestionName = b.HRMFQNS_QuestionName,

                                             }).ToArray();
                dto.question_list = _vmsconte.HR_Master_Feedback_Qns_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMFQNS_ActiveFlg == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Training_Question_DTO deactivate_TQM(HR_Training_Question_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Training_Question_DMO_con.Where(a => a.HRTCR_Id == dto.HRTCR_Id && a.MI_Id == dto.MI_Id).ToList();
                foreach (var item in result)
                {
                    var result1 = _vmsconte.HR_Training_Question_DMO_con.Single(a => a.HRMFQNS_Id == item.HRMFQNS_Id && a.HRTCR_Id == dto.HRTCR_Id && a.MI_Id == dto.MI_Id);

                    if (result1.HRTRQNS_ActiveFlg == true)
                    {
                        result1.HRTRQNS_ActiveFlg = false;
                        result1.HRTRQNS_UpdatedBy = dto.userId;
                        result1.HRTRQNS_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result1);
                        dto.returnval = false;
                    }
                    else
                    {
                        result1.HRTRQNS_ActiveFlg = true;
                        result1.HRTRQNS_UpdatedBy = dto.userId;
                        result1.HRTRQNS_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result1);
                        dto.returnval = true;
                    }
                }

                _vmsconte.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        public HR_Training_Question_DTO option_view_TQM(HR_Training_Question_DTO dto)
        {
            try
            {
                dto.question_view_list = (from a in _vmsconte.HR_Training_Question_DMO_con
                                          from b in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                          where a.HRMFQNS_Id == b.HRMFQNS_Id && a.MI_Id == b.MI_Id && a.MI_Id == dto.MI_Id && a.HRMTT_Id == dto.HRMTT_Id
                                          select new HR_Master_Question_Option_DTO
                                          {
                                              HRMFQNS_QuestionName = b.HRMFQNS_QuestionName,
                                              HRMFQNS_ActiveFlg = b.HRMFQNS_ActiveFlg
                                          }).ToArray();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        public HR_Training_Question_DTO question_evaluation_TQM(HR_Training_Question_DTO dto)
        {
            try
            {
                var tr = _vmsconte.HR_Training_Create_IntTrainer_DMO_con.Where(a => a.HRTCR_Id == dto.HRTCR_Id).ToList();
                if (tr[0].HRTCR_Id > 0)
                {
                    dto.question_emp_details_list = (from a in _vmsconte.HR_Training_Create_DMO_con
                                                     from b in _vmsconte.HR_Training_Create_IntTrainer_DMO_con
                                                     from c in _vmsconte.Hr_Master_Employee_con
                                                     from d in _vmsconte.HR_Master_Designation_con
                                                     where a.HRTCR_Id == b.HRTCR_Id && a.MI_Id == dto.MI_Id && a.HRTCR_Id == dto.HRTCR_Id && b.HRME_Id == c.HRME_Id && c.HRMDES_Id == d.HRMDES_Id && c.MI_Id == d.MI_Id
                                                     select new HR_Training_Question_DTO
                                                     {
                                                         employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : c.HRME_EmployeeFirstName)
                                                          + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" ? "" : " " + c.HRME_EmployeeMiddleName)
                                                          + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" ? "" : " " + c.HRME_EmployeeLastName)).Trim(),
                                                         HRTCR_Id = a.HRTCR_Id,
                                                         HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                                         start_date = a.HRTCR_StartDate,
                                                         end_date = a.HRTCR_EndDate,
                                                         HRTCR_InternalORExternalFlg = a.HRTCR_InternalORExternalFlg,
                                                         HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                         HRME_Id = b.HRME_Id
                                                     }).ToArray();

                }
                else
                {
                    dto.question_emp_details_list = (from a in _vmsconte.HR_Training_Create_DMO_con
                                                     from b in _vmsconte.HR_Training_Create_ExtTrainer_DMO_con
                                                     from c in _vmsconte.HR_Master_External_Trainer_Creation_DMO_con
                                                     where a.HRTCR_Id == b.HRTCR_Id && a.MI_Id == dto.MI_Id && a.HRTCR_Id == dto.HRTCR_Id && b.HRME_Id == c.HRMETR_Id
                                                     select new HR_Training_Question_DTO
                                                     {
                                                         employeename = c.HRMETR_Name,
                                                         HRTCR_Id = a.HRTCR_Id,
                                                         HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                                         start_date = a.HRTCR_StartDate,
                                                         end_date = a.HRTCR_EndDate,
                                                         HRTCR_InternalORExternalFlg = a.HRTCR_InternalORExternalFlg,
                                                         HRMDES_DesignationName = "External Trainer"
                                                     }).ToArray();
                }

                dto.feedback_question = (from a in _vmsconte.HR_Training_Question_DMO_con
                                         from b in _vmsconte.HR_Master_Feedback_Qns_DMO_con
                                         where a.HRMFQNS_Id == b.HRMFQNS_Id && a.MI_Id == b.MI_Id && a.HRTCR_Id == dto.HRTCR_Id
                                         select new HR_Training_Question_DTO
                                         {
                                             HRTCR_Id = a.HRTCR_Id,
                                             HRMFQNS_Id = a.HRMFQNS_Id,
                                             HRMFQNS_QuestionName = b.HRMFQNS_QuestionName
                                         }).ToArray();

                List<long> question_id = new List<long>();
                var check = _vmsconte.HR_Training_Question_DMO_con.Where(a => a.HRTCR_Id == dto.HRTCR_Id).ToList();
                foreach (var item in check)
                {
                    question_id.Add(item.HRMFQNS_Id);
                }
                dto.feedback_option = (from a in _vmsconte.HR_Master_Question_Option_DMO_con
                                       from b in _vmsconte.HR_Master_Feedback_Option_DMO_con
                                       where a.HRMFOPT_Id == b.HRMFOPT_Id && a.MI_Id == b.MI_Id && question_id.Contains(a.HRMFQNS_Id)
                                       select new HR_Training_Question_DTO
                                       {
                                           HRMFQNS_Id = a.HRMFQNS_Id,
                                           HRMFOPT_Id = b.HRMFOPT_Id,
                                           HRMFOPT_OptionName = b.HRMFOPT_OptionName
                                       }).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        public HR_Training_Question_DTO Training_Feedback_TQM(HR_Training_Question_DTO dto)
        {
            try
            {
                var hrmEempid = _vmsconte.Staff_User_Login.Where(t => t.MI_Id == dto.MI_Id && t.Id == dto.userId).Distinct().ToList();
                foreach (var item in dto.questionoption_new)
                {
                    HR_Training_Feedback_DMO t = new HR_Training_Feedback_DMO();
                    t.MI_Id = dto.MI_Id;
                    t.HRTCR_Id = dto.HRTCR_Id;
                    t.HRME_Id = hrmEempid[0].Emp_Code;
                    t.HRTFEED_IntORExtFlg = dto.HRTCR_InternalORExternalFlg1;
                    t.HRTFEED_TrainerId = dto.HRTFEED_TrainerId;
                    t.HRMFQNS_Id = item.HRMFQNS_Id;
                    t.HRMFOPT_Id = item.HRMFOPT_Id;
                    t.HRTFEED_AboutTraining = dto.HRTFEED_AboutTraining;
                    t.HRTFEED_Improvement = dto.HRTFEED_Improvement;
                    t.HRTFEED_Response = dto.HRTFEED_Response;
                    t.HRTFEED_ActiveFlg = true;
                    t.HRTFEED_CreatedBy = dto.userId;
                    t.HRTFEED_CreatedDate = DateTime.Now;
                    _vmsconte.Add(t);

                }
                _vmsconte.SaveChanges();
                dto.returnvalue = "Add";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }
        public HR_Master_TrainingTopicDTO getdatatopic(HR_Master_TrainingTopicDTO dto)
        {
            try
            {
                dto.getmasterdata = _vmsconte.HR_Master_TrainingTopicDMO.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        public HR_Master_TrainingTopicDTO SaveEdit_Topic(HR_Master_TrainingTopicDTO dto)
        {
            try
            {
                if (dto.HRMTT_Id > 0)
                {
                    var duplicate = _vmsconte.HR_Master_TrainingTopicDMO.Where(a => a.HRMTT_Topic == dto.HRMTT_Topic).ToList();
                    if (duplicate.Count != 0)
                    {
                        dto.returnvalue = "Duplicate";
                    }
                    else
                    {
                        var result = _vmsconte.HR_Master_TrainingTopicDMO.Where(a => a.HRMTT_Id == dto.HRMTT_Id).FirstOrDefault();
                        result.HRMTT_Topic = dto.HRMTT_Topic;
                        result.HRMTT_ActiveFlg = true;
                        result.HRMTT_UpdatedBy = dto.userId;
                        result.UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result);
                        _vmsconte.SaveChanges();
                        dto.returnvalue = "Update";
                    }
                }
                else
                {
                    HR_Master_TrainingTopicDMO tot = new HR_Master_TrainingTopicDMO();
                    tot.HRMTT_Topic = dto.HRMTT_Topic;
                    tot.HRMTT_ActiveFlg = true;
                    tot.HRMTT_CreatedBy = dto.userId;
                    tot.HRMTT_UpdatedBy = dto.userId;
                    tot.CreatedDate = DateTime.Now;
                    tot.UpdatedDate = DateTime.Now;
                    _vmsconte.Add(tot);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
                dto.getmasterdata = _vmsconte.HR_Master_TrainingTopicDMO.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Master_TrainingTopicDTO deactivate_Topic(HR_Master_TrainingTopicDTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Master_TrainingTopicDMO.Where(a => a.HRMTT_Id == dto.HRMTT_Id).FirstOrDefault();
                if (result.HRMTT_ActiveFlg == true)
                {
                    result.HRMTT_ActiveFlg = false;
                    result.HRMTT_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    dto.returnvalue = "false";
                }
                else
                {
                    result.HRMTT_ActiveFlg = true;
                    result.HRMTT_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    dto.returnvalue = "true";
                }
                _vmsconte.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        public HR_Master_TrainingTopicDTO details_Topic(HR_Master_TrainingTopicDTO bt)
        {
            try
            {
                bt.edit_topic = _vmsconte.HR_Master_TrainingTopicDMO.Where(t => t.HRMTT_Id == bt.HRMTT_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return bt;
        }
        ///////=================================////////////Master_TrainingTopicMaterial////////////////////======================================================

        public HR_Master_TrainingTopicDTO SaveTopic_content(HR_Master_TrainingTopicDTO dto)
        {
            try
            {
                if (dto.HRMETRTYTOPM_Id > 0)
                {
                    var duplicate = _vmsconte.HR_Master_External_TrainingType_Topic_MaterialsDMO.Where(a => a.HRMTT_Id == dto.HRMTT_Id && a.HRMETRTYTOPM_StudyMaterialsDesc == dto.HRMETRTYTOPM_StudyMaterialsDesc).ToList();
                    if (duplicate.Count != 0)
                    {
                        dto.returnvalue = "Duplicate";
                    }
                    else
                    {
                        var result = _vmsconte.HR_Master_External_TrainingType_Topic_MaterialsDMO.Where(a => a.HRMTT_Id == dto.HRMTT_Id).FirstOrDefault();
                        result.HRMTT_Id = dto.HRMTT_Id;
                        result.HRMETRTYTOPM_StudyMaterials = dto.HRMETRTYTOPM_StudyMaterials;
                        result.HRMETRTYTOPM_StudyMaterialsDesc = dto.HRMTT_Topic;
                        result.HRMETRTYTOPM_ActiveFlag = true;
                        result.HRMETRTYTOPM_UpdatedBy = dto.userId;
                        result.HRMETRTYTOPM_UpdatedDate = DateTime.Now;
                        _vmsconte.Update(result);
                        _vmsconte.SaveChanges();
                        dto.returnvalue = "Update";
                    }
                }
                else
                {
                    if (dto.FilePath_Array.Length > 0)
                    {
                        foreach (var filedata in dto.FilePath_Array)
                        {
                            HR_Master_External_TrainingType_Topic_MaterialsDMO tot = new HR_Master_External_TrainingType_Topic_MaterialsDMO();
                            tot.HRMETRTYTOPM_StudyMaterials = filedata.FilePath;
                            tot.HRMTT_Id = dto.HRMTT_Id;
                            tot.HRMETRTYTOPM_StudyMaterialsDesc = dto.HRMETRTYTOPM_StudyMaterialsDesc;
                            tot.HRMETRTYTOPM_ActiveFlag = true;
                            tot.HRMETRTYTOPM_CreatedBy = dto.userId;
                            tot.HRMETRTYTOPM_UpdatedBy = dto.userId;
                            tot.HRMETRTYTOPM_CreatedDate = DateTime.Now;
                            tot.HRMETRTYTOPM_UpdatedDate = DateTime.Now;
                            _vmsconte.Add(tot);
                            _vmsconte.SaveChanges();
                        }

                    }

                    dto.returnvalue = "Add";
                }
                dto.getmasterdata = _vmsconte.HR_Master_External_TrainingType_Topic_MaterialsDMO.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

    }
}
