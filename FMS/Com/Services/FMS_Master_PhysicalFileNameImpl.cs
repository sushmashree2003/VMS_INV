using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using DomainModel.Model.com.vapstech.FMS;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Services
{
    public class FMS_Master_PhysicalFileNameImpl : Interface.FMS_Master_PhysicalFileNameInterface
    {
        public FMSContext _FMSContext;

        public FMS_Master_PhysicalFileNameImpl(FMSContext FMSContext)
        {
            _FMSContext = FMSContext;
        }

        // Master Physical File
        public FMS_Master_PhysicalFileNameDTO loadPhysicalFiledata(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.getinstitutiondetails = _FMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.gridviewDetails = _FMSContext.FMS_Master_PhysicalFileNameDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getdepartmentlist = _FMSContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.HRMD_DepartmentName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO savedata(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                if (data.FMSMPFN_Id > 0)
                {
                    var duplicate = _FMSContext.FMS_Master_PhysicalFileNameDMO.Where(t => t.FMSMPFN_PhysicalFileName == data.FMSMPFN_PhysicalFileName
                     && t.MI_Id == data.MI_Id && t.FMSMPFN_Id != data.FMSMPFN_Id).ToArray();

                    if (duplicate.Length > 0)
                    {
                        data.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        FMS_Master_PhysicalFileNameDMO fopunch = new FMS_Master_PhysicalFileNameDMO();
                        fopunch = _FMSContext.FMS_Master_PhysicalFileNameDMO.Where(t => t.FMSMPFN_Id == data.FMSMPFN_Id).FirstOrDefault();
                        fopunch.FMSMPFN_PhysicalFileName = data.FMSMPFN_PhysicalFileName;
                        fopunch.FMSMPFN_FileContent = data.FMSMPFN_FileContent;
                        fopunch.FMSMPFN_CreatedYear = data.FMSMPFN_CreatedYear;
                        fopunch.FMSMPFN_FileNo = data.FMSMPFN_FileNo;
                        fopunch.FMSMPFN_FileType = data.FMSMPFN_FileType;
                        fopunch.FMSMPFN_UpdatedDate = DateTime.Now;
                        fopunch.FMSMPFN_UpdatedBy = data.UserId;
                        _FMSContext.Update(fopunch);


                        if (data.deptlist_temp != null && data.deptlist_temp.Length > 0)
                        {
                            List<long> temparr = new List<long>();
                            List<long> temparr1 = new List<long>();

                            //GETTING ALL FILES
                            foreach (deptlist_temp ph in data.deptlist_temp)
                            {
                                temparr.Add(ph.FMSDPFN_Id);
                            }

                            //REMOVING FILES
                            Array files_Noresultremove = _FMSContext.FMS_Department_PhysicalFileNameDMO.Where(t => !temparr.Contains(t.FMSDPFN_Id)
                            && t.FMSMPFN_Id == data.FMSMPFN_Id).ToArray();

                            foreach (FMS_Department_PhysicalFileNameDMO ph1 in files_Noresultremove)
                            {
                                _FMSContext.Remove(ph1);
                            }

                            foreach (var c in data.deptlist_temp)
                            {
                                if (c.FMSDPFN_Id > 0)
                                {
                                    var result = _FMSContext.FMS_Department_PhysicalFileNameDMO.Single(a => a.FMSDPFN_Id == c.FMSDPFN_Id);
                                    result.FMSDPFN_UpdatedBy = data.UserId;
                                    result.FMSDPFN_UpdatedDate = DateTime.Now;
                                    _FMSContext.Update(result);
                                }
                                else
                                {
                                    FMS_Department_PhysicalFileNameDMO fMS_Department_PhysicalFileNameDMO = new FMS_Department_PhysicalFileNameDMO();
                                    fMS_Department_PhysicalFileNameDMO.HRMD_Id = c.HRMD_Id;
                                    fMS_Department_PhysicalFileNameDMO.FMSDPFN_ActiveFlg = true;
                                    fMS_Department_PhysicalFileNameDMO.FMSMPFN_Id = data.FMSMPFN_Id;
                                    fMS_Department_PhysicalFileNameDMO.FMSDPFN_UpdatedBy = data.UserId;
                                    fMS_Department_PhysicalFileNameDMO.FMSDPFN_UpdatedDate = DateTime.Now;
                                    fMS_Department_PhysicalFileNameDMO.FMSDPFN_CreatedBy = data.UserId;
                                    fMS_Department_PhysicalFileNameDMO.FMSDPFN_CreatedDate = DateTime.Now;
                                    _FMSContext.Add(fMS_Department_PhysicalFileNameDMO);
                                }
                            }
                        }

                        var contactExists = _FMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "Update";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }
                }
                else
                {
                    var duplicate = _FMSContext.FMS_Master_PhysicalFileNameDMO.Where(t => t.FMSMPFN_PhysicalFileName == data.FMSMPFN_PhysicalFileName
                     && t.MI_Id == data.MI_Id).ToArray();

                    if (duplicate.Length > 0)
                    {
                        data.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        FMS_Master_PhysicalFileNameDMO objpunch = new FMS_Master_PhysicalFileNameDMO();
                        objpunch.MI_Id = data.MI_Id;
                        objpunch.FMSMPFN_PhysicalFileName = data.FMSMPFN_PhysicalFileName;
                        objpunch.FMSMPFN_FileContent = data.FMSMPFN_FileContent;
                        objpunch.FMSMPFN_CreatedYear = data.FMSMPFN_CreatedYear;
                        objpunch.FMSMPFN_FileNo = data.FMSMPFN_FileNo;
                        objpunch.FMSMPFN_FileType = data.FMSMPFN_FileType;
                        objpunch.FMSMPFN_ActiveFlg = true;
                        objpunch.FMSMPFN_CreatedDate = DateTime.Now;
                        objpunch.FMSMPFN_UpdatedDate = DateTime.Now;
                        objpunch.FMSMPFN_CreatedBy = data.UserId;
                        objpunch.FMSMPFN_UpdatedBy = data.UserId;
                        _FMSContext.Add(objpunch);

                        if (data.deptlist_temp != null && data.deptlist_temp.Length > 0)
                        {
                            foreach (var c in data.deptlist_temp)
                            {
                                FMS_Department_PhysicalFileNameDMO fMS_Department_PhysicalFileNameDMO = new FMS_Department_PhysicalFileNameDMO
                                {
                                    FMSMPFN_Id = objpunch.FMSMPFN_Id,
                                    HRMD_Id = c.HRMD_Id,
                                    FMSDPFN_ActiveFlg = true,
                                    FMSDPFN_CreatedDate = DateTime.Now,
                                    FMSDPFN_UpdatedDate = DateTime.Now,
                                    FMSDPFN_CreatedBy = data.UserId,
                                    FMSDPFN_UpdatedBy = data.UserId
                                };
                                _FMSContext.Add(fMS_Department_PhysicalFileNameDMO);
                            }
                        }

                        var contactExists = _FMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }
                }

                data.gridviewDetails = _FMSContext.FMS_Master_PhysicalFileNameDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO editRecord(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.editDetails = _FMSContext.FMS_Master_PhysicalFileNameDMO.Where(t => t.FMSMPFN_Id == data.FMSMPFN_Id).ToArray();

                data.physicaldetails = _FMSContext.FMS_Department_PhysicalFileNameDMO.Where(t => t.FMSMPFN_Id == data.FMSMPFN_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                FMS_Master_PhysicalFileNameDMO obj = new FMS_Master_PhysicalFileNameDMO();
                obj = _FMSContext.FMS_Master_PhysicalFileNameDMO.Where(t => t.FMSMPFN_Id == data.FMSMPFN_Id).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.FMSMPFN_ActiveFlg == true) { obj.FMSMPFN_ActiveFlg = false; data.retrunMsg = "Deactivated"; }
                    else
                    if (obj.FMSMPFN_ActiveFlg == false) { obj.FMSMPFN_ActiveFlg = true; data.retrunMsg = "Activated"; }
                    _FMSContext.Update(obj);
                    _FMSContext.SaveChanges();
                }
                data.gridviewDetails = _FMSContext.FMS_Master_PhysicalFileNameDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Master File Category
        public FMS_Master_PhysicalFileNameDTO loadFileCategorydata(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.getinstitutiondetails = _FMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.filecategoryDetails = _FMSContext.FMS_Master_FileCategoryDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getdepartmentlist = _FMSContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.HRMD_DepartmentName).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO saveFileCategory(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                if (data.FMSMFC_Id > 0)
                {
                    var duplicate = _FMSContext.FMS_Master_FileCategoryDMO.Where(t => t.FMSMFC_FileCategoryName == data.FMSMFC_FileCategoryName
                    && t.FMSMFC_Id != data.FMSMFC_Id && t.MI_Id == data.MI_Id).ToList();

                    var duplicate2 = _FMSContext.FMS_Master_FileCategoryDMO.Where(t => t.FMSMFC_FileCategoryCode == data.FMSMFC_FileCategoryCode
                    && t.FMSMFC_Id != data.FMSMFC_Id && t.MI_Id == data.MI_Id).ToList();

                    if (duplicate.Count == 0 && duplicate2.Count == 0)
                    {
                        FMS_Master_FileCategoryDMO fopunch = new FMS_Master_FileCategoryDMO();
                        fopunch = _FMSContext.FMS_Master_FileCategoryDMO.Where(t => t.FMSMFC_Id == data.FMSMFC_Id).FirstOrDefault();
                        fopunch.FMSMFC_FileCategoryName = data.FMSMFC_FileCategoryName;
                        fopunch.FMSMFC_FileCategoryCode = data.FMSMFC_FileCategoryCode;
                        fopunch.FMSMFC_UpdatedDate = DateTime.Now;
                        fopunch.FMSMFC_UpdatedBy = data.UserId;
                        _FMSContext.Update(fopunch);

                        if (data.deptlisttemp != null && data.deptlisttemp.Length > 0)
                        {
                            List<long> temparr = new List<long>();
                            List<long> temparr1 = new List<long>();

                            //GETTING ALL FILES
                            foreach (deptlisttemp ph in data.deptlisttemp)
                            {
                                temparr.Add(ph.FMSDFC_Id);
                            }

                            //REMOVING FILES
                            Array files_Noresultremove = _FMSContext.FMS_Department_FileCategoryDMO.Where(t => !temparr.Contains(t.FMSDFC_Id)
                            && t.FMSMFC_Id == data.FMSMFC_Id).ToArray();

                            foreach (FMS_Department_FileCategoryDMO ph1 in files_Noresultremove)
                            {
                                _FMSContext.Remove(ph1);
                            }


                            foreach (var c in data.deptlisttemp)
                            {
                                if (c.FMSDFC_Id > 0)
                                {
                                    var result = _FMSContext.FMS_Department_FileCategoryDMO.Single(a => a.FMSDFC_Id == c.FMSDFC_Id);
                                    result.FMSDFC_UpdatedBy = data.UserId;
                                    result.FMSDFC_UpdatedDate = DateTime.Now;
                                    _FMSContext.Update(result);
                                }
                                else
                                {
                                    FMS_Department_FileCategoryDMO fMS_Department_FileCategoryDMO = new FMS_Department_FileCategoryDMO();
                                    fMS_Department_FileCategoryDMO.HRMD_Id = c.HRMD_Id;
                                    fMS_Department_FileCategoryDMO.FMSDFC_ActiveFlg = true;
                                    fMS_Department_FileCategoryDMO.FMSMFC_Id = data.FMSMFC_Id;
                                    fMS_Department_FileCategoryDMO.FMSDFC_UpdatedBy = data.UserId;
                                    fMS_Department_FileCategoryDMO.FMSDFC_UpdatedDate = DateTime.Now;
                                    fMS_Department_FileCategoryDMO.FMSDFC_CreatedBy = data.UserId;
                                    fMS_Department_FileCategoryDMO.FMSDFC_CreatedDate = DateTime.Now;
                                    _FMSContext.Add(fMS_Department_FileCategoryDMO);
                                }
                            }
                        }

                        var contactExists = _FMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "Update";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        data.retrunMsg = "Duplicate";
                    }
                }
                else
                {
                    var duplicate = _FMSContext.FMS_Master_FileCategoryDMO.Where(t => t.FMSMFC_FileCategoryName == data.FMSMFC_FileCategoryName
                     && t.MI_Id == data.MI_Id).ToList();

                    var duplicate2 = _FMSContext.FMS_Master_FileCategoryDMO.Where(t => t.FMSMFC_FileCategoryCode == data.FMSMFC_FileCategoryCode
                    && t.MI_Id == data.MI_Id).ToList();

                    if (duplicate.Count == 0 && duplicate2.Count == 0)
                    {

                        FMS_Master_FileCategoryDMO objpunch = new FMS_Master_FileCategoryDMO();
                        objpunch.MI_Id = data.MI_Id;
                        objpunch.FMSMFC_FileCategoryName = data.FMSMFC_FileCategoryName;
                        objpunch.FMSMFC_FileCategoryCode = data.FMSMFC_FileCategoryCode;
                        objpunch.FMSMFC_ActiveFlg = true;
                        objpunch.FMSMFC_CreatedDate = DateTime.Now;
                        objpunch.FMSMFC_UpdatedDate = DateTime.Now;
                        objpunch.FMSMFC_CreatedBy = data.UserId;
                        objpunch.FMSMFC_UpdatedBy = data.UserId;
                        _FMSContext.Add(objpunch);

                        if (data.deptlisttemp != null && data.deptlisttemp.Length > 0)
                        {
                            foreach (var c in data.deptlisttemp)
                            {
                                FMS_Department_FileCategoryDMO fMS_Department_FileCategoryDMO = new FMS_Department_FileCategoryDMO();
                                fMS_Department_FileCategoryDMO.HRMD_Id = c.HRMD_Id;
                                fMS_Department_FileCategoryDMO.FMSDFC_ActiveFlg = true;
                                fMS_Department_FileCategoryDMO.FMSMFC_Id = objpunch.FMSMFC_Id;
                                fMS_Department_FileCategoryDMO.FMSDFC_UpdatedBy = data.UserId;
                                fMS_Department_FileCategoryDMO.FMSDFC_UpdatedDate = DateTime.Now;
                                fMS_Department_FileCategoryDMO.FMSDFC_CreatedBy = data.UserId;
                                fMS_Department_FileCategoryDMO.FMSDFC_CreatedDate = DateTime.Now;
                                _FMSContext.Add(fMS_Department_FileCategoryDMO);
                            }
                        }

                        var contactExists = _FMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        data.retrunMsg = "Duplicate";
                    }
                }

                data.filecategoryDetails = _FMSContext.FMS_Master_FileCategoryDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO editRecord2(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.editDetails = _FMSContext.FMS_Master_FileCategoryDMO.Where(t => t.FMSMFC_Id == data.FMSMFC_Id).ToArray();

                data.geteditdeptDetails = _FMSContext.FMS_Department_FileCategoryDMO.Where(t => t.FMSMFC_Id == data.FMSMFC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord2(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                FMS_Master_FileCategoryDMO obj = new FMS_Master_FileCategoryDMO();
                obj = _FMSContext.FMS_Master_FileCategoryDMO.Where(t => t.FMSMFC_Id == data.FMSMFC_Id).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.FMSMFC_ActiveFlg == true) { obj.FMSMFC_ActiveFlg = false; data.retrunMsg = "Deactivated"; }
                    else
                    if (obj.FMSMFC_ActiveFlg == false) { obj.FMSMFC_ActiveFlg = true; data.retrunMsg = "Activated"; }
                    _FMSContext.Update(obj);
                    _FMSContext.SaveChanges();
                }
                data.filecategoryDetails = _FMSContext.FMS_Master_FileCategoryDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ViewDeptList(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.getsaveddeptDetails = (from a in _FMSContext.FMS_Department_FileCategoryDMO
                                            from b in _FMSContext.HR_Master_Department
                                            where (a.HRMD_Id == b.HRMD_Id && a.FMSMFC_Id == data.FMSMFC_Id)
                                            select new FMS_Master_PhysicalFileNameDTO
                                            {
                                                FMSDFC_Id = a.FMSDFC_Id,
                                                FMSMFC_Id = a.FMSMFC_Id,
                                                departmentname = b.HRMD_DepartmentName,
                                                HRMD_Id = a.HRMD_Id,
                                                FMSDFC_ActiveFlg = a.FMSDFC_ActiveFlg
                                            }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveDept(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                var result = _FMSContext.FMS_Department_FileCategoryDMO.Single(a => a.FMSDFC_Id == data.FMSDFC_Id);

                result.FMSDFC_ActiveFlg = result.FMSDFC_ActiveFlg == true ? false : true;
                result.FMSDFC_UpdatedBy = data.UserId;
                result.FMSDFC_UpdatedDate = DateTime.Now;
                _FMSContext.Update(result);
                var i = _FMSContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                data.getsaveddeptDetails = (from a in _FMSContext.FMS_Department_FileCategoryDMO
                                            from b in _FMSContext.HR_Master_Department
                                            where (a.HRMD_Id == b.HRMD_Id && a.FMSMFC_Id == data.FMSMFC_Id)
                                            select new FMS_Master_PhysicalFileNameDTO
                                            {
                                                FMSDFC_Id = a.FMSDFC_Id,
                                                FMSMFC_Id = a.FMSMFC_Id,
                                                departmentname = b.HRMD_DepartmentName,
                                                HRMD_Id = a.HRMD_Id,
                                                FMSDFC_ActiveFlg = a.FMSDFC_ActiveFlg
                                            }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Master Dispatch Mode
        public FMS_Master_PhysicalFileNameDTO loadDispatchModedata(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.getinstitutiondetails = _FMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.dispatchmodeDetails = _FMSContext.FMS_Master_DispatchModeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO saveDispatchMode(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {

                if (data.FMSMDMD_Id > 0)
                {
                    var duplicate = _FMSContext.FMS_Master_DispatchModeDMO.Where(t => t.FMSMTMD_DispatchMode == data.FMSMTMD_DispatchMode
                    && t.MI_Id == data.MI_Id && t.FMSMDMD_Id == data.FMSMDMD_Id).ToArray();
                    if (duplicate.Length > 0)
                    {
                        data.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        FMS_Master_DispatchModeDMO fopunch = new FMS_Master_DispatchModeDMO();
                        fopunch = _FMSContext.FMS_Master_DispatchModeDMO.Where(t => t.FMSMDMD_Id == data.FMSMDMD_Id).FirstOrDefault();
                        fopunch.FMSMTMD_DispatchMode = data.FMSMTMD_DispatchMode;
                        fopunch.FMSMTMD_DispatchModeDetails = data.FMSMTMD_DispatchModeDetails;
                        fopunch.FMSMDMD_UpdatedDate = DateTime.Now;
                        fopunch.FMSMDMD_UpdatedBy = data.UserId;
                        _FMSContext.Update(fopunch);
                        var contactExists = _FMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "Update";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }
                }
                else
                {
                    var duplicate = _FMSContext.FMS_Master_DispatchModeDMO.Where(t => t.FMSMTMD_DispatchMode == data.FMSMTMD_DispatchMode
                    && t.MI_Id == data.MI_Id).ToArray();
                    if (duplicate.Length > 0)
                    {
                        data.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        FMS_Master_DispatchModeDMO objpunch = new FMS_Master_DispatchModeDMO();
                        objpunch.MI_Id = data.MI_Id;
                        objpunch.FMSMTMD_DispatchMode = data.FMSMTMD_DispatchMode;
                        objpunch.FMSMTMD_DispatchModeDetails = data.FMSMTMD_DispatchModeDetails;
                        objpunch.FMSMDMD_ActiveFlg = true;
                        objpunch.FMSMDMD_CreatedDate = DateTime.Now;
                        objpunch.FMSMDMD_UpdatedDate = DateTime.Now;
                        objpunch.FMSMDMD_CreatedBy = data.UserId;
                        objpunch.FMSMDMD_UpdatedBy = data.UserId;
                        _FMSContext.Add(objpunch);
                        var contactExists = _FMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }
                }

                data.dispatchmodeDetails = _FMSContext.FMS_Master_DispatchModeDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO editRecord3(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.editDetails = _FMSContext.FMS_Master_DispatchModeDMO.Where(t => t.FMSMDMD_Id == data.FMSMDMD_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord3(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                FMS_Master_DispatchModeDMO obj = new FMS_Master_DispatchModeDMO();
                obj = _FMSContext.FMS_Master_DispatchModeDMO.Where(t => t.FMSMDMD_Id == data.FMSMDMD_Id).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.FMSMDMD_ActiveFlg == true) { obj.FMSMDMD_ActiveFlg = false; data.retrunMsg = "Deactivated"; }
                    else
                    if (obj.FMSMDMD_ActiveFlg == false) { obj.FMSMDMD_ActiveFlg = true; data.retrunMsg = "Activated"; }
                    _FMSContext.Update(obj);
                    _FMSContext.SaveChanges();
                }
                data.dispatchmodeDetails = _FMSContext.FMS_Master_DispatchModeDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Master File Cabinet
        public FMS_Master_PhysicalFileNameDTO loadFileCabinetdata(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.getinstitutiondetails = _FMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.FileCabinetDetails = (from a in _FMSContext.FMS_Master_FileCabinetDMO
                                           from b in _FMSContext.HR_Master_Building_con
                                           from c in _FMSContext.HR_Master_Floor_con
                                           from d in _FMSContext.HR_Master_Room_con
                                           where (a.MI_Id == data.MI_Id && a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMR_Id == d.HRMR_Id
                                           && a.MI_Id == data.MI_Id)
                                           select new FMS_Master_PhysicalFileNameDTO
                                           {
                                               FMSMFCBT_Id = a.FMSMFCBT_Id,
                                               FMSMFCBT_FileCabinetName = a.FMSMFCBT_FileCabinetName,
                                               HRMB_Id = a.HRMB_Id,
                                               HRMB_BuildingName = b.HRMB_BuildingName,
                                               HRMF_Id = a.HRMF_Id,
                                               HRMF_FloorName = c.HRMF_FloorName,
                                               HRMR_Id = a.HRMR_Id,
                                               HRMR_RoomName = d.HRMR_RoomName,
                                               FMSMFCBT_ActiveFlg = a.FMSMFCBT_ActiveFlg,
                                               FMSMFCBT_NoOfColumns = a.FMSMFCBT_NoOfColumns,
                                               FMSMFCBT_NoOfRows = a.FMSMFCBT_NoOfRows
                                           }).ToArray();

                data.buildingList = _FMSContext.HR_Master_Building_con.Where(t => t.HRMB_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();
                data.roomList = _FMSContext.HR_Master_Room_con.Where(t => t.HRMR_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();
                data.floorList = _FMSContext.HR_Master_Floor_con.Where(t => t.HRMF_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO saveFileCabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                var duplicate = _FMSContext.FMS_Master_FileCabinetDMO.Where(t => t.FMSMFCBT_FileCabinetName == data.FMSMFCBT_FileCabinetName
                && t.HRMB_Id == data.HRMB_Id && t.HRMF_Id == data.HRMF_Id && t.HRMR_Id == data.HRMR_Id

                && t.FMSMFCBT_NoOfColumns == data.FMSMFCBT_NoOfColumns && t.FMSMFCBT_NoOfRows == data.FMSMFCBT_NoOfRows
                && t.FMSMFCBT_ActiveFlg == true).ToArray();

                if (data.FMSMFCBT_Id > 0)
                {
                    FMS_Master_FileCabinetDMO fopunch = new FMS_Master_FileCabinetDMO();
                    fopunch = _FMSContext.FMS_Master_FileCabinetDMO.Where(t => t.FMSMFCBT_Id == data.FMSMFCBT_Id).FirstOrDefault();
                    fopunch.FMSMFCBT_FileCabinetName = data.FMSMFCBT_FileCabinetName;
                    fopunch.HRMB_Id = data.HRMB_Id;
                    fopunch.HRMF_Id = data.HRMF_Id;
                    fopunch.HRMR_Id = data.HRMR_Id;
                    fopunch.FMSMFCBT_NoOfColumns = data.FMSMFCBT_NoOfColumns;
                    fopunch.FMSMFCBT_NoOfRows = data.FMSMFCBT_NoOfRows;
                    fopunch.FMSMFCBT_UpdatedDate = DateTime.Now;
                    fopunch.FMSMFCBT_UpdatedBy = data.UserId;
                    _FMSContext.Update(fopunch);

                    foreach (var cr in data.FMS_Master_FileCabinet_Rows)
                    {
                        if (cr.FMSMFCR_Id > 0)
                        {
                            var resultrows = _FMSContext.FMS_Master_FileCabinet_RowDMO.Single(a => a.FMSMFCR_Id == cr.FMSMFCR_Id);
                            resultrows.FMSMFCBT_Id = data.FMSMFCBT_Id;
                            resultrows.FMSMFCR_CabinetRowName = cr.FMSMFCR_CabinetRowName;
                            resultrows.FMSMFCR_UpdatedDate = DateTime.Now;
                            resultrows.FMSMFCR_UpdatedBy = data.UserId;
                            _FMSContext.Update(resultrows);
                        }
                        else
                        {
                            FMS_Master_FileCabinet_RowDMO objrows = new FMS_Master_FileCabinet_RowDMO();
                            objrows.FMSMFCBT_Id = data.FMSMFCBT_Id;
                            objrows.FMSMFCR_CabinetRowName = cr.FMSMFCR_CabinetRowName;
                            objrows.FMSMFCR_ActiveFlg = true;
                            objrows.FMSMFCR_CreatedDate = DateTime.Now;
                            objrows.FMSMFCR_UpdatedDate = DateTime.Now;
                            objrows.FMSMFCR_CreatedBy = data.UserId;
                            objrows.FMSMFCR_UpdatedBy = data.UserId;
                            _FMSContext.Add(objrows);
                        }
                    }

                    foreach (var cc in data.FMS_Master_FileCabinet_Columns)
                    {
                        if (cc.FMSMFCC_Id > 0)
                        {
                            var resultcolumns = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Single(a => a.FMSMFCC_Id == cc.FMSMFCC_Id);
                            resultcolumns.FMSMFCBT_Id = data.FMSMFCBT_Id;
                            resultcolumns.FMSMFCC_CabinetColumnName = cc.FMSMFCC_CabinetColumnName;
                            resultcolumns.FMSMFCC_UpdatedDate = DateTime.Now;
                            resultcolumns.FMSMFCC_UpdatedBy = data.UserId;
                            _FMSContext.Update(resultcolumns);
                        }
                        else
                        {
                            FMS_Master_FileCabinet_ColumnDMO objrows = new FMS_Master_FileCabinet_ColumnDMO();
                            objrows.FMSMFCBT_Id = data.FMSMFCBT_Id;
                            objrows.FMSMFCC_CabinetColumnName = cc.FMSMFCC_CabinetColumnName;
                            objrows.FMSMFCC_ActiveFlg = true;
                            objrows.FMSMFCC_CreatedDate = DateTime.Now;
                            objrows.FMSMFCC_UpdatedDate = DateTime.Now;
                            objrows.FMSMFCC_CreatedBy = data.UserId;
                            objrows.FMSMFCC_UpdatedBy = data.UserId;
                            _FMSContext.Add(objrows);
                        }
                    }

                    var contactExists = _FMSContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.retrunMsg = "Update";
                    }
                    else
                    {
                        data.retrunMsg = "false";
                    }
                }
                else
                {
                    FMS_Master_FileCabinetDMO objpunch = new FMS_Master_FileCabinetDMO();
                    objpunch.MI_Id = data.MI_Id;
                    objpunch.FMSMFCBT_FileCabinetName = data.FMSMFCBT_FileCabinetName;
                    objpunch.HRMB_Id = data.HRMB_Id;
                    objpunch.HRMF_Id = data.HRMF_Id;
                    objpunch.HRMR_Id = data.HRMR_Id;
                    objpunch.FMSMFCBT_NoOfColumns = data.FMSMFCBT_NoOfColumns;
                    objpunch.FMSMFCBT_NoOfRows = data.FMSMFCBT_NoOfRows;
                    objpunch.FMSMFCBT_ActiveFlg = true;
                    objpunch.FMSMFCBT_CreatedDate = DateTime.Now;
                    objpunch.FMSMFCBT_UpdatedDate = DateTime.Now;
                    objpunch.FMSMFCBT_CreatedBy = data.UserId;
                    objpunch.FMSMFCBT_UpdatedBy = data.UserId;
                    _FMSContext.Add(objpunch);

                    if (data.FMSMFCBT_NoOfRows > 0)
                    {
                        for (int ii = 0; ii < data.FMS_Master_FileCabinet_Rows.Length; ii++)
                        {
                            FMS_Master_FileCabinet_RowDMO objrows = new FMS_Master_FileCabinet_RowDMO();
                            objrows.FMSMFCBT_Id = objpunch.FMSMFCBT_Id;
                            objrows.FMSMFCR_CabinetRowName = data.FMS_Master_FileCabinet_Rows[ii].FMSMFCR_CabinetRowName;
                            objrows.FMSMFCR_ActiveFlg = true;
                            objrows.FMSMFCR_CreatedDate = DateTime.Now;
                            objrows.FMSMFCR_UpdatedDate = DateTime.Now;
                            objrows.FMSMFCR_CreatedBy = data.UserId;
                            objrows.FMSMFCR_UpdatedBy = data.UserId;
                            _FMSContext.Add(objrows);
                        }
                    }

                    if (data.FMSMFCBT_NoOfColumns > 0)
                    {
                        for (int ii = 0; ii < data.FMS_Master_FileCabinet_Columns.Length; ii++)
                        {
                            FMS_Master_FileCabinet_ColumnDMO objrows = new FMS_Master_FileCabinet_ColumnDMO();
                            objrows.FMSMFCBT_Id = objpunch.FMSMFCBT_Id;
                            objrows.FMSMFCC_CabinetColumnName = data.FMS_Master_FileCabinet_Columns[ii].FMSMFCC_CabinetColumnName;
                            objrows.FMSMFCC_ActiveFlg = true;
                            objrows.FMSMFCC_CreatedDate = DateTime.Now;
                            objrows.FMSMFCC_UpdatedDate = DateTime.Now;
                            objrows.FMSMFCC_CreatedBy = data.UserId;
                            objrows.FMSMFCC_UpdatedBy = data.UserId;
                            _FMSContext.Add(objrows);
                        }
                    }

                    var i = _FMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.retrunMsg = "Add";
                    }
                    else
                    {
                        data.retrunMsg = "false";
                    }
                }

                data.FileCabinetDetails = (from a in _FMSContext.FMS_Master_FileCabinetDMO
                                           from b in _FMSContext.HR_Master_Building_con
                                           from c in _FMSContext.HR_Master_Floor_con
                                           from d in _FMSContext.HR_Master_Room_con
                                           where (a.MI_Id == data.MI_Id && a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMR_Id == d.HRMR_Id)
                                           select new FMS_Master_PhysicalFileNameDTO
                                           {
                                               FMSMFCBT_Id = a.FMSMFCBT_Id,
                                               FMSMFCBT_FileCabinetName = a.FMSMFCBT_FileCabinetName,
                                               HRMB_Id = a.HRMB_Id,
                                               HRMB_BuildingName = b.HRMB_BuildingName,
                                               HRMF_Id = a.HRMF_Id,
                                               HRMF_FloorName = c.HRMF_FloorName,
                                               HRMR_Id = a.HRMR_Id,
                                               HRMR_RoomName = d.HRMR_RoomName,
                                               FMSMFCBT_ActiveFlg = a.FMSMFCBT_ActiveFlg,
                                               FMSMFCBT_NoOfColumns = a.FMSMFCBT_NoOfColumns,
                                               FMSMFCBT_NoOfRows = a.FMSMFCBT_NoOfRows
                                           }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO editRecord4(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.editDetails = _FMSContext.FMS_Master_FileCabinetDMO.Where(t => t.FMSMFCBT_Id == data.FMSMFCBT_Id).ToArray();
                data.rowdetails = _FMSContext.FMS_Master_FileCabinet_RowDMO.Where(t => t.FMSMFCBT_Id == data.FMSMFCBT_Id).ToArray();
                data.columndetails = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Where(t => t.FMSMFCBT_Id == data.FMSMFCBT_Id).ToArray();
                //data.editDetails = (from a in _FMSContext.FMS_Master_FileCabinetDMO
                //                    from b in _FMSContext.HR_Master_Building_con
                //                    from c in _FMSContext.HR_Master_Floor_con
                //                    from d in _FMSContext.HR_Master_Room_con
                //                    select new FMS_Master_PhysicalFileNameDTO
                //                    {
                //                        FMSMFCBT_Id = a.FMSMFCBT_Id,
                //                        FMSMFCBT_FileCabinetName = a.FMSMFCBT_FileCabinetName,
                //                        HRMB_Id=a.HRMB_Id,
                //                        HRMB_BuildingName = b.HRMB_BuildingName,
                //                        HRMF_Id=a.HRMF_Id,
                //                        HRMF_FloorName = c.HRMF_FloorName,
                //                        HRMR_Id=a.HRMR_Id,
                //                        HRMR_RoomName = d.HRMR_RoomName,
                //                        FMSMFCBT_ActiveFlg=a.FMSMFCBT_ActiveFlg,
                //                        FMSMFCBT_NoOfColumns=a.FMSMFCBT_NoOfColumns,
                //                        FMSMFCBT_NoOfRows=a.FMSMFCBT_NoOfRows
                //                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord4(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                FMS_Master_FileCabinetDMO obj = new FMS_Master_FileCabinetDMO();
                obj = _FMSContext.FMS_Master_FileCabinetDMO.Where(t => t.FMSMFCBT_Id == data.FMSMFCBT_Id).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.FMSMFCBT_ActiveFlg == true) { obj.FMSMFCBT_ActiveFlg = false; data.retrunMsg = "Deactivated"; }
                    else
                    if (obj.FMSMFCBT_ActiveFlg == false) { obj.FMSMFCBT_ActiveFlg = true; data.retrunMsg = "Activated"; }
                    _FMSContext.Update(obj);
                    _FMSContext.SaveChanges();
                }

                data.FileCabinetDetails = (from a in _FMSContext.FMS_Master_FileCabinetDMO
                                           from b in _FMSContext.HR_Master_Building_con
                                           from c in _FMSContext.HR_Master_Floor_con
                                           from d in _FMSContext.HR_Master_Room_con
                                           where (a.MI_Id == data.MI_Id && a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMR_Id == d.HRMR_Id)
                                           select new FMS_Master_PhysicalFileNameDTO
                                           {
                                               FMSMFCBT_Id = a.FMSMFCBT_Id,
                                               FMSMFCBT_FileCabinetName = a.FMSMFCBT_FileCabinetName,
                                               HRMB_Id = a.HRMB_Id,
                                               HRMB_BuildingName = b.HRMB_BuildingName,
                                               HRMF_Id = a.HRMF_Id,
                                               HRMF_FloorName = c.HRMF_FloorName,
                                               HRMR_Id = a.HRMR_Id,
                                               HRMR_RoomName = d.HRMR_RoomName,
                                               FMSMFCBT_ActiveFlg = a.FMSMFCBT_ActiveFlg,
                                               FMSMFCBT_NoOfColumns = a.FMSMFCBT_NoOfColumns,
                                               FMSMFCBT_NoOfRows = a.FMSMFCBT_NoOfRows
                                           }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Configuration
        public FMS_Master_PhysicalFileNameDTO loadreferenceconfg(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.getinstitutiondetails = _FMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.gridviewDetails = _FMSContext.FMS_ReferenceNo_ConfigurationDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO savereferenceconfg(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                var duplicateorder = _FMSContext.FMS_ReferenceNo_ConfigurationDMO.Where(t => t.FMSRNCON_CriteriaOrder == data.FMSRNCON_CriteriaOrder && t.FMSRNCON_ActiveFlg == true).ToArray();

                var duplicate = _FMSContext.FMS_ReferenceNo_ConfigurationDMO.Where(t => t.FMSRNCON_Criterias == data.FMSRNCON_Criterias && t.FMSRNCON_ActiveFlg == true).ToArray();

                if (duplicateorder.Length > 0)
                {
                    data.retrunMsg = "Duplicate Order";
                }
                else if (duplicate.Length > 0)
                {
                    data.retrunMsg = "Duplicate Criteria";
                }
                else
                {
                    if (data.FMSRNCON_Id > 0)
                    {

                        FMS_ReferenceNo_ConfigurationDMO fopunch = new FMS_ReferenceNo_ConfigurationDMO();
                        fopunch = _FMSContext.FMS_ReferenceNo_ConfigurationDMO.Where(t => t.FMSRNCON_Id == data.FMSRNCON_Id).FirstOrDefault();
                        fopunch.FMSRNCON_Criterias = data.FMSRNCON_Criterias;
                        fopunch.FMSRNCON_CriteriaOrder = data.FMSRNCON_CriteriaOrder;
                        fopunch.FMSRNCON_UpdatedDate = DateTime.Now;
                        fopunch.FMSRNCON_UpdatedBy = data.UserId;
                        _FMSContext.Update(fopunch);
                        var contactExists = _FMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "Update";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        FMS_ReferenceNo_ConfigurationDMO objpunch = new FMS_ReferenceNo_ConfigurationDMO();
                        objpunch.MI_Id = data.MI_Id;
                        objpunch.FMSRNCON_Criterias = data.FMSRNCON_Criterias;
                        objpunch.FMSRNCON_CriteriaOrder = data.FMSRNCON_CriteriaOrder;
                        objpunch.FMSRNCON_ActiveFlg = true;
                        objpunch.FMSRNCON_CreatedDate = DateTime.Now;
                        objpunch.FMSRNCON_UpdatedDate = DateTime.Now;
                        objpunch.FMSRNCON_CreatedBy = data.UserId;
                        objpunch.FMSRNCON_UpdatedBy = data.UserId;
                        _FMSContext.Add(objpunch);
                        var contactExists = _FMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.retrunMsg = "Add";
                        }
                        else
                        {
                            data.retrunMsg = "false";
                        }
                    }

                    data.gridviewDetails = _FMSContext.FMS_ReferenceNo_ConfigurationDMO.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO editreferenceconfg(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.editDetails = _FMSContext.FMS_ReferenceNo_ConfigurationDMO.Where(t => t.FMSRNCON_Id == data.FMSRNCON_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactivereferenceconfg(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                FMS_ReferenceNo_ConfigurationDMO obj = new FMS_ReferenceNo_ConfigurationDMO();
                obj = _FMSContext.FMS_ReferenceNo_ConfigurationDMO.Where(t => t.FMSRNCON_Id == data.FMSRNCON_Id).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.FMSRNCON_ActiveFlg == true) { obj.FMSRNCON_ActiveFlg = false; data.retrunMsg = "Deactivated"; }
                    else
                    if (obj.FMSRNCON_ActiveFlg == false) { obj.FMSRNCON_ActiveFlg = true; data.retrunMsg = "Activated"; }
                    _FMSContext.Update(obj);
                    _FMSContext.SaveChanges();
                }
                data.gridviewDetails = _FMSContext.FMS_ReferenceNo_ConfigurationDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* File Name & Cabinet Mapping */
        public FMS_Master_PhysicalFileNameDTO LoadFilenameAndCabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.getinstitutiondetails = _FMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.filenamedetails = (from e in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                        where (e.MI_Id == data.MI_Id && e.FMSMPFN_ActiveFlg == true)
                                        select new FMS_Master_PhysicalFileNameDTO
                                        {
                                            FMSMPFN_Id = e.FMSMPFN_Id,
                                            FMSMPFN_PhysicalFileName = ((e.FMSMPFN_PhysicalFileName == null
                                            || e.FMSMPFN_PhysicalFileName == "" ? "" : e.FMSMPFN_PhysicalFileName) +
                                            (e.FMSMPFN_CreatedYear == null || e.FMSMPFN_CreatedYear == "" ? "" : "/" + e.FMSMPFN_CreatedYear) +
                                            (e.FMSMPFN_FileNo == null || e.FMSMPFN_FileNo == "" ? "" : "/" + e.FMSMPFN_FileNo)).Trim(),
                                        }).Distinct().OrderBy(a => a.FMSMPFN_PhysicalFileName).ToArray();

                //data.cabinetnamedetails = _FMSContext.FMS_Master_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMFCBT_ActiveFlg == true).
                //    OrderBy(a => a.FMSMFCBT_FileCabinetName).ToArray();

                data.cabinetnamedetails = (from a in _FMSContext.FMS_Master_FileCabinetDMO
                                           from b in _FMSContext.HR_Master_Building_con
                                           from c in _FMSContext.HR_Master_Floor_con
                                           from d in _FMSContext.HR_Master_Room_con
                                           where (a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMR_Id == d.HRMR_Id && a.MI_Id == data.MI_Id
                                           && a.FMSMFCBT_ActiveFlg == true)
                                           select new FMS_Master_PhysicalFileNameDTO
                                           {
                                               FMSMFCBT_Id = a.FMSMFCBT_Id,
                                               FMSMFCBT_FileCabinetName = ((b.HRMB_BuildingName == null || b.HRMB_BuildingName == "" ? "" : b.HRMB_BuildingName) +
                                             (c.HRMF_FloorName == null || c.HRMF_FloorName == "" ? "" : "/" + c.HRMF_FloorName) +
                                             (d.HRMR_RoomName == null || d.HRMR_RoomName == "" ? "" : "/" + d.HRMR_RoomName) +
                                             (a.FMSMFCBT_FileCabinetName == null || a.FMSMFCBT_FileCabinetName == "" ? "" : "/" + a.FMSMFCBT_FileCabinetName)
                                             ).Trim(),

                                           }).Distinct().OrderBy(a => a.FMSMFCBT_FileCabinetName).ToArray();



                data.gridFileNameCabinetDetails = (from e in _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO
                                                   from a in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                                   from b in _FMSContext.FMS_Master_FileCabinetDMO
                                                   from c in _FMSContext.FMS_Master_FileCabinet_RowDMO
                                                   from d in _FMSContext.FMS_Master_FileCabinet_ColumnDMO
                                                   from h in _FMSContext.HR_Master_Building_con
                                                   from f in _FMSContext.HR_Master_Floor_con
                                                   from g in _FMSContext.HR_Master_Room_con
                                                   where (e.FMSMPFN_Id == a.FMSMPFN_Id && e.FMSMFCBT_Id == b.FMSMFCBT_Id && e.FMSMFCR_Id == c.FMSMFCR_Id
                                                   && b.HRMB_Id == h.HRMB_Id && b.HRMF_Id == f.HRMF_Id && b.HRMR_Id == g.HRMR_Id
                                                   && e.FMSMFCC_Id == d.FMSMFCC_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id)
                                                   select new FMS_Master_PhysicalFileNameDTO
                                                   {
                                                       FMSMPFNFC_Id = e.FMSMPFNFC_Id,
                                                       FMSMPFN_PhysicalFileName = ((a.FMSMPFN_PhysicalFileName == null || a.FMSMPFN_PhysicalFileName == "" ? "" : a.FMSMPFN_PhysicalFileName) +
                                            (a.FMSMPFN_CreatedYear == null || a.FMSMPFN_CreatedYear == "" ? "" : "/" + a.FMSMPFN_CreatedYear) +
                                            (a.FMSMPFN_FileNo == null || a.FMSMPFN_FileNo == "" ? "" : "/" + a.FMSMPFN_FileNo)).Trim(),
                                                       FMSMFCBT_FileCabinetName = ((h.HRMB_BuildingName == null || h.HRMB_BuildingName == "" ? "" : h.HRMB_BuildingName) +
                                            (f.HRMF_FloorName == null || f.HRMF_FloorName == "" ? "" : "/" + f.HRMF_FloorName) +
                                            (g.HRMR_RoomName == null || g.HRMR_RoomName == "" ? "" : "/" + g.HRMR_RoomName) +
                                            (b.FMSMFCBT_FileCabinetName == null || b.FMSMFCBT_FileCabinetName == "" ? "" : "/" + b.FMSMFCBT_FileCabinetName)).Trim(),

                                                       FMSMFCR_CabinetRowName = c.FMSMFCR_CabinetRowName,
                                                       FMSMFCC_CabinetColumnName = d.FMSMFCC_CabinetColumnName,
                                                       FMSMFCBT_Id = e.FMSMFCBT_Id,
                                                       FMSMPFN_Id = e.FMSMPFN_Id,
                                                       FMSMFCC_Id = e.FMSMFCC_Id,
                                                       FMSMFCR_Id = e.FMSMFCR_Id,
                                                       FMSMPFN_ActiveFlg = e.FMSMPFN_ActiveFlg,
                                                       FMSMPFN_CreatedDate = e.FMSMPFN_CreatedDate
                                                   }).Distinct().OrderByDescending(a => a.FMSMPFN_CreatedDate).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO OnChangephysicalname(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                var Checkfilecabinet = _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMPFN_Id == data.FMSMPFN_Id).ToList();

                if (Checkfilecabinet.Count > 0)
                {
                    data.FMSMFCBT_Id = Checkfilecabinet.FirstOrDefault().FMSMFCBT_Id;

                    data.Checkfilecabinet = Checkfilecabinet.ToArray();

                    data.filecabinetrow = _FMSContext.FMS_Master_FileCabinet_RowDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
                    data.filecabinetcolumn = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO OnChangeFileCabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                data.filecabinetrow = _FMSContext.FMS_Master_FileCabinet_RowDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
                data.filecabinetcolumn = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO SaveFileNameAndCabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.FMSMPFNFC_Id > 0)
                {
                    var checkduplicate = _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.FMSMPFN_Id == data.FMSMPFN_Id && a.FMSMPFNFC_Id != data.FMSMPFNFC_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        data.retrunMsg = "Update";

                        var result = _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO.Single(a => a.MI_Id == data.MI_Id && a.FMSMPFNFC_Id == data.FMSMPFNFC_Id);
                        result.FMSMPFN_Id = data.FMSMPFN_Id;
                        result.FMSMFCBT_Id = data.FMSMFCBT_Id;
                        result.FMSMFCC_Id = data.FMSMFCC_Id;
                        result.FMSMFCR_Id = data.FMSMFCR_Id;
                        result.FMSMPFN_UpdatedDate = indiantime0;
                        result.FMSMPFN_UpdatedBy = data.UserId;
                        _FMSContext.Update(result);
                        var i = _FMSContext.SaveChanges();
                        if (i > 0)
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
                    var checkduplicate = _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMPFN_Id == data.FMSMPFN_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        data.retrunMsg = "Add";
                        FMS_Master_PhysicalFileName_FileCabinetDMO fMS_Master_PhysicalFileName_FileCabinetDMO = new FMS_Master_PhysicalFileName_FileCabinetDMO();
                        fMS_Master_PhysicalFileName_FileCabinetDMO.MI_Id = data.MI_Id;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMPFN_Id = data.FMSMPFN_Id;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMFCBT_Id = data.FMSMFCBT_Id;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMFCC_Id = data.FMSMFCC_Id;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMFCR_Id = data.FMSMFCR_Id;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMPFN_ActiveFlg = true;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMPFN_CreatedDate = indiantime0;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMPFN_UpdatedDate = indiantime0;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMPFN_CreatedBy = data.UserId;
                        fMS_Master_PhysicalFileName_FileCabinetDMO.FMSMPFN_UpdatedBy = data.UserId;

                        _FMSContext.Add(fMS_Master_PhysicalFileName_FileCabinetDMO);
                        var i = _FMSContext.SaveChanges();
                        if (i > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO Editfilenamecabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                var Checkfilecabinetd = _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id
                && a.FMSMPFNFC_Id == data.FMSMPFNFC_Id).ToList();

                var Checkfilecabinet = (from a in _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO
                                        from e in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                        where (e.FMSMPFN_Id == a.FMSMPFN_Id && a.MI_Id == data.MI_Id && a.FMSMPFNFC_Id == data.FMSMPFNFC_Id)
                                        select new FMS_Master_PhysicalFileNameDTO
                                        {
                                            FMSMPFN_Id = e.FMSMPFN_Id,
                                            FMSMPFN_PhysicalFileName = ((e.FMSMPFN_PhysicalFileName == null
                                            || e.FMSMPFN_PhysicalFileName == "" ? "" : e.FMSMPFN_PhysicalFileName) +
                                            (e.FMSMPFN_CreatedYear == null || e.FMSMPFN_CreatedYear == "" ? "" : "/" + e.FMSMPFN_CreatedYear) +
                                            (e.FMSMPFN_FileNo == null || e.FMSMPFN_FileNo == "" ? "" : "/" + e.FMSMPFN_FileNo)).Trim(),
                                            FMSMFCC_Id = a.FMSMFCC_Id,
                                            FMSMFCR_Id = a.FMSMFCR_Id,
                                            FMSMPFNFC_Id = a.FMSMPFNFC_Id,
                                            FMSMFCBT_Id = a.FMSMFCBT_Id,
                                        }).Distinct().ToList();

                data.editphysicalfileandcabinet = Checkfilecabinet.ToArray();

                data.FMSMFCBT_Id = Checkfilecabinet.FirstOrDefault().FMSMFCBT_Id;

                data.filecabinetrow = _FMSContext.FMS_Master_FileCabinet_RowDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
                data.filecabinetcolumn = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ActivateRecord5(FMS_Master_PhysicalFileNameDTO data)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

            try
            {
                var result = _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO.Single(a => a.MI_Id == data.MI_Id && a.FMSMPFNFC_Id == data.FMSMPFNFC_Id);
                result.FMSMPFN_ActiveFlg = result.FMSMPFN_ActiveFlg == false ? true : false;
                result.FMSMPFN_UpdatedDate = indiantime0;
                result.FMSMPFN_UpdatedBy = data.UserId;
                _FMSContext.Update(result);
                var i = _FMSContext.SaveChanges();
                if (i > 0)
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
        public FMS_Master_PhysicalFileNameDTO OnChangeInstitution(FMS_Master_PhysicalFileNameDTO data)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

            try
            {
                if (data.Typetab == "Tab1")
                {
                    data.gridviewDetails = _FMSContext.FMS_Master_PhysicalFileNameDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                    data.getdepartmentlist = _FMSContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.HRMD_DepartmentName).ToArray();
                }
                else if (data.Typetab == "Tab2")
                {
                    data.filecategoryDetails = _FMSContext.FMS_Master_FileCategoryDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                    data.getdepartmentlist = _FMSContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.HRMD_DepartmentName).ToArray();
                }
                else if (data.Typetab == "Tab3")
                {
                    data.dispatchmodeDetails = _FMSContext.FMS_Master_DispatchModeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                }
                else if (data.Typetab == "Tab4")
                {
                    data.FileCabinetDetails = (from a in _FMSContext.FMS_Master_FileCabinetDMO
                                               from b in _FMSContext.HR_Master_Building_con
                                               from c in _FMSContext.HR_Master_Floor_con
                                               from d in _FMSContext.HR_Master_Room_con
                                               where (a.MI_Id == data.MI_Id && a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMR_Id == d.HRMR_Id
                                               && a.MI_Id == data.MI_Id)
                                               select new FMS_Master_PhysicalFileNameDTO
                                               {
                                                   FMSMFCBT_Id = a.FMSMFCBT_Id,
                                                   FMSMFCBT_FileCabinetName = a.FMSMFCBT_FileCabinetName,
                                                   HRMB_Id = a.HRMB_Id,
                                                   HRMB_BuildingName = b.HRMB_BuildingName,
                                                   HRMF_Id = a.HRMF_Id,
                                                   HRMF_FloorName = c.HRMF_FloorName,
                                                   HRMR_Id = a.HRMR_Id,
                                                   HRMR_RoomName = d.HRMR_RoomName,
                                                   FMSMFCBT_ActiveFlg = a.FMSMFCBT_ActiveFlg,
                                                   FMSMFCBT_NoOfColumns = a.FMSMFCBT_NoOfColumns,
                                                   FMSMFCBT_NoOfRows = a.FMSMFCBT_NoOfRows
                                               }).ToArray();

                    data.buildingList = _FMSContext.HR_Master_Building_con.Where(t => t.HRMB_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();
                    data.roomList = _FMSContext.HR_Master_Room_con.Where(t => t.HRMR_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();
                    data.floorList = _FMSContext.HR_Master_Floor_con.Where(t => t.HRMF_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();
                }
                else if (data.Typetab == "Tab5")
                {
                    data.filenamedetails = (from e in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                            where (e.MI_Id == data.MI_Id && e.FMSMPFN_ActiveFlg == true)
                                            select new FMS_Master_PhysicalFileNameDTO
                                            {
                                                FMSMPFN_Id = e.FMSMPFN_Id,
                                                FMSMPFN_PhysicalFileName = ((e.FMSMPFN_PhysicalFileName == null
                                                || e.FMSMPFN_PhysicalFileName == "" ? "" : e.FMSMPFN_PhysicalFileName) +
                                                (e.FMSMPFN_CreatedYear == null || e.FMSMPFN_CreatedYear == "" ? "" : "/" + e.FMSMPFN_CreatedYear) +
                                                (e.FMSMPFN_FileNo == null || e.FMSMPFN_FileNo == "" ? "" : "/" + e.FMSMPFN_FileNo)).Trim(),
                                            }).Distinct().OrderBy(a => a.FMSMPFN_PhysicalFileName).ToArray();

                    data.cabinetnamedetails = (from a in _FMSContext.FMS_Master_FileCabinetDMO
                                               from b in _FMSContext.HR_Master_Building_con
                                               from c in _FMSContext.HR_Master_Floor_con
                                               from d in _FMSContext.HR_Master_Room_con
                                               where (a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMR_Id == d.HRMR_Id && a.MI_Id == data.MI_Id
                                               && a.FMSMFCBT_ActiveFlg == true)
                                               select new FMS_Master_PhysicalFileNameDTO
                                               {
                                                   FMSMFCBT_Id = a.FMSMFCBT_Id,
                                                   FMSMFCBT_FileCabinetName = ((b.HRMB_BuildingName == null || b.HRMB_BuildingName == "" ? "" : b.HRMB_BuildingName) +
                                                 (c.HRMF_FloorName == null || c.HRMF_FloorName == "" ? "" : "/" + c.HRMF_FloorName) +
                                                 (d.HRMR_RoomName == null || d.HRMR_RoomName == "" ? "" : "/" + d.HRMR_RoomName) +
                                                 (a.FMSMFCBT_FileCabinetName == null || a.FMSMFCBT_FileCabinetName == "" ? "" : "/" + a.FMSMFCBT_FileCabinetName)
                                                 ).Trim(),

                                               }).Distinct().OrderBy(a => a.FMSMFCBT_FileCabinetName).ToArray();

                    data.gridFileNameCabinetDetails = (from e in _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO
                                                       from a in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                                       from b in _FMSContext.FMS_Master_FileCabinetDMO
                                                       from c in _FMSContext.FMS_Master_FileCabinet_RowDMO
                                                       from d in _FMSContext.FMS_Master_FileCabinet_ColumnDMO
                                                       from h in _FMSContext.HR_Master_Building_con
                                                       from f in _FMSContext.HR_Master_Floor_con
                                                       from g in _FMSContext.HR_Master_Room_con
                                                       where (e.FMSMPFN_Id == a.FMSMPFN_Id && e.FMSMFCBT_Id == b.FMSMFCBT_Id && e.FMSMFCR_Id == c.FMSMFCR_Id
                                                       && b.HRMB_Id == h.HRMB_Id && b.HRMF_Id == f.HRMF_Id && b.HRMR_Id == g.HRMR_Id
                                                       && e.FMSMFCC_Id == d.FMSMFCC_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id)
                                                       select new FMS_Master_PhysicalFileNameDTO
                                                       {
                                                           FMSMPFNFC_Id = e.FMSMPFNFC_Id,
                                                           FMSMPFN_PhysicalFileName = ((a.FMSMPFN_PhysicalFileName == null || a.FMSMPFN_PhysicalFileName == "" ? "" : a.FMSMPFN_PhysicalFileName) +
                                                (a.FMSMPFN_CreatedYear == null || a.FMSMPFN_CreatedYear == "" ? "" : "/" + a.FMSMPFN_CreatedYear) +
                                                (a.FMSMPFN_FileNo == null || a.FMSMPFN_FileNo == "" ? "" : "/" + a.FMSMPFN_FileNo)).Trim(),
                                                           FMSMFCBT_FileCabinetName = ((h.HRMB_BuildingName == null || h.HRMB_BuildingName == "" ? "" : h.HRMB_BuildingName) +
                                                (f.HRMF_FloorName == null || f.HRMF_FloorName == "" ? "" : "/" + f.HRMF_FloorName) +
                                                (g.HRMR_RoomName == null || g.HRMR_RoomName == "" ? "" : "/" + g.HRMR_RoomName) +
                                                (b.FMSMFCBT_FileCabinetName == null || b.FMSMFCBT_FileCabinetName == "" ? "" : "/" + b.FMSMFCBT_FileCabinetName)).Trim(),

                                                           FMSMFCR_CabinetRowName = c.FMSMFCR_CabinetRowName,
                                                           FMSMFCC_CabinetColumnName = d.FMSMFCC_CabinetColumnName,
                                                           FMSMFCBT_Id = e.FMSMFCBT_Id,
                                                           FMSMPFN_Id = e.FMSMPFN_Id,
                                                           FMSMFCC_Id = e.FMSMFCC_Id,
                                                           FMSMFCR_Id = e.FMSMFCR_Id,
                                                           FMSMPFN_ActiveFlg = e.FMSMPFN_ActiveFlg,
                                                           FMSMPFN_CreatedDate = e.FMSMPFN_CreatedDate
                                                       }).Distinct().OrderByDescending(a => a.FMSMPFN_CreatedDate).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ViewPhysicalFileDeptList(FMS_Master_PhysicalFileNameDTO data)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

            try
            {
                data.getsaveddeptDetails = (from a in _FMSContext.FMS_Department_PhysicalFileNameDMO
                                            from b in _FMSContext.HR_Master_Department
                                            where (a.HRMD_Id == b.HRMD_Id && a.FMSMPFN_Id == data.FMSMPFN_Id)
                                            select new FMS_Master_PhysicalFileNameDTO
                                            {
                                                FMSDPFN_Id = a.FMSDPFN_Id,
                                                FMSMPFN_Id = a.FMSMPFN_Id,
                                                departmentname = b.HRMD_DepartmentName,
                                                HRMD_Id = a.HRMD_Id,
                                                FMSDPFN_ActiveFlg = a.FMSDPFN_ActiveFlg
                                            }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveDeptPhyscialfile(FMS_Master_PhysicalFileNameDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _FMSContext.FMS_Department_PhysicalFileNameDMO.Single(a => a.FMSDPFN_Id == data.FMSDPFN_Id);

                result.FMSDPFN_ActiveFlg = result.FMSDPFN_ActiveFlg == true ? false : true;
                result.FMSDPFN_UpdatedBy = data.UserId;
                result.FMSDPFN_UpdatedDate = indiantime0;
                _FMSContext.Update(result);
                var i = _FMSContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                data.getsaveddeptDetails = (from a in _FMSContext.FMS_Department_PhysicalFileNameDMO
                                            from b in _FMSContext.HR_Master_Department
                                            where (a.HRMD_Id == b.HRMD_Id && a.FMSMPFN_Id == data.FMSMPFN_Id)
                                            select new FMS_Master_PhysicalFileNameDTO
                                            {
                                                FMSDPFN_Id = a.FMSDPFN_Id,
                                                FMSMPFN_Id = a.FMSMPFN_Id,
                                                departmentname = b.HRMD_DepartmentName,
                                                HRMD_Id = a.HRMD_Id,
                                                FMSDPFN_ActiveFlg = a.FMSDPFN_ActiveFlg
                                            }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
    }
}