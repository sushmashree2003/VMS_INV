using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class GroupDeptDessgService : Interfaces.GroupDeptDessgInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public GroupDeptDessgService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;
        }

        public HRGroupDeptDessgDTO getBasicData(HRGroupDeptDessgDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public HRGroupDeptDessgDTO GetAllDropdownAndDatatableDetails(HRGroupDeptDessgDTO dto)
        {
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();

            try
            {
                //Grouptypelist
                GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                dto.groupTypedropdown = GroupTypelist.ToArray();

                //Departmentlist
                Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                dto.departmentdropdown = Departmentlist.ToArray();

                //Designationlist
                Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                dto.designationdropdown = Designationlist.ToArray();

                //Get All Data
                var getalldata = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                  from b in _HRMSContext.HR_Master_GroupType
                                  from c in _HRMSContext.HR_Master_Department
                                  from d in _HRMSContext.HR_Master_Designation
                                  where (a.MI_Id == dto.MI_Id && a.HRMGT_Id == b.HRMGT_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id)
                                  select new HRGroupDeptDessgDTO
                                  {
                                        HRGTDDS_Id = a.HRGTDDS_Id,
                                        HRMGT_Id = a.HRMGT_Id,
                                        HRMGT_EmployeeGroupType = b.HRMGT_EmployeeGroupType,
                                        HRMD_Id = a.HRMD_Id,
                                        HRMD_DepartmentName = c.HRMD_DepartmentName,
                                        HRMDES_Id = a.HRMDES_Id,
                                        HRMDES_DesignationName = d.HRMDES_DesignationName,
                                        HRGTDDS_ActiveFlg = a.HRGTDDS_ActiveFlg,
                                  }).ToArray();
                dto.gridviewdata = getalldata;
                //Leave Authorisation Data

                dto.leave_name = _HRMSContext.HR_Master_Leave.Where(t => t.MI_Id == dto.MI_Id && t.HRML_LeaveCreditFlg == true).ToArray();

                dto.get_emp = (from x in _HRMSContext.MasterEmployee
                                where (x.HRME_ActiveFlag == true && x.HRME_LeftFlag == false)
                                select new HRGroupDeptDessgDTO
                                {
                                    HRME_Id = x.HRME_Id,
                                    //HRME_EmployeeFirstName = x.HRME_EmployeeFirstName
                                    HRME_EmployeeFirstName = x.HRME_EmployeeFirstName +
                                    (x.HRME_EmployeeMiddleName == null || x.HRME_EmployeeMiddleName == "" ? "" : " " + x.HRME_EmployeeMiddleName) +
                                    (x.HRME_EmployeeLastName == null || x.HRME_EmployeeLastName == "" ? "" : " " + x.HRME_EmployeeLastName)
                                }).Distinct().ToArray();
                //END

                dto.privalue = _HRMSContext.HR_PROCESS_PRIVILEGEDTO.ToArray();

                dto.approveid = (from a in _HRMSContext.Staff_User_Login
                            from b in _HRMSContext.MasterEmployee
                            where (a.Emp_Code == b.HRME_Id && a.IVRMSTAUL_ActiveFlag == 1 && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                            select a).ToArray();
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public HRGroupDeptDessgDTO savedata(HRGroupDeptDessgDTO dto)
        {
            try
            {
                if (dto.designationids.Length > 0)
                {
                    for (int i = 0; i < dto.designationids.Length; i++)
                    {
                        var duplicate = _HRMSContext.HRGroupDeptDessgDMO.Where(t => t.HRMGT_Id == dto.HRMGT_Id && t.HRMD_Id == dto.HRMD_Id && t.HRMDES_Id == dto.designationids[i]).ToArray();
                        if (duplicate.Length == 0)
                        {
                            HRGroupDeptDessgDMO objdmo = new HRGroupDeptDessgDMO();
                            objdmo.MI_Id = dto.MI_Id;
                            objdmo.HRMGT_Id = dto.HRMGT_Id;
                            objdmo.HRMD_Id = dto.HRMD_Id;
                            objdmo.HRMDES_Id = dto.designationids[i];
                            objdmo.HRGTDDS_ActiveFlg = true;
                            objdmo.HRGTDDS_CreatedBy = dto.LogInUserId;
                            objdmo.HRGTDDS_UpdatedBy = dto.LogInUserId;
                            objdmo.HRGTDDS_CreatedDate = DateTime.Now;
                            objdmo.HRGTDDS_UpdatedDate = DateTime.Now;
                            _HRMSContext.Add(objdmo);
                            _HRMSContext.SaveChanges();
                        }
                    }
                    dto.msg = "Saved";
                }
            }
            catch (Exception ex)
            {
                dto.msg = "Failed";
                Console.WriteLine("ex");
            }
            return dto;
        }
        public HRGroupDeptDessgDTO Editdata(HRGroupDeptDessgDTO data)
        {
            try
            {
                data.returnval = false;
                var getdata = _HRMSContext.HRGroupDeptDessgDMO.Where(a => a.HRGTDDS_Id == data.HRGTDDS_Id).FirstOrDefault();
                _HRMSContext.Remove(getdata);
                _HRMSContext.SaveChanges();
                data.returnval = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HRGroupDeptDessgDTO masterDecative(HRGroupDeptDessgDTO data)
        {
            try
            {
                data.returnval = false;
                var getdata = _HRMSContext.HRGroupDeptDessgDMO.Where(a => a.HRGTDDS_Id == data.HRGTDDS_Id).FirstOrDefault();
                if (getdata.HRGTDDS_ActiveFlg == true)
                {
                    getdata.HRGTDDS_ActiveFlg = false;
                    data.msg = "Deactivated";
                }
                else if (getdata.HRGTDDS_ActiveFlg == false)
                {
                    getdata.HRGTDDS_ActiveFlg = true;
                    data.msg = "Activated";
                }
                _HRMSContext.Update(getdata);
                _HRMSContext.SaveChanges();
                data.returnval = true;
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
