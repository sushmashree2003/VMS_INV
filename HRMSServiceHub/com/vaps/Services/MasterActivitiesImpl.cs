using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.Training;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HRMSServiceHub.com.vaps.Services
{
    public class MasterActivitiesImpl : Interfaces.MasterActivitiesInterface
    {
        public HRMSContext _context;
        public MasterActivitiesImpl(HRMSContext _con)
        {
            _context = _con;
        }

        public MasterActivitiesDTO onloaddata(MasterActivitiesDTO data)
        {
            try
            {


                data.getloaddetails = _context.HR_PROCESS_PRIVILEGEDTO.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterActivitiesDTO saverecord(MasterActivitiesDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);


                List<long> employeeids = new List<long>();
                if (data.HR_PR_ID > 0)
                {

                    var checkduplicate = _context.HR_PROCESS_PRIVILEGEDTO.Where(a => a.HR_PR_ID != data.HR_PR_ID).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                        var checkresult = _context.HR_PROCESS_PRIVILEGEDTO.Single(a => a.HR_PR_ID == data.HR_PR_ID);
                        checkresult.HR_PR_ID = data.HR_PR_ID;
                        checkresult.HR_PR_NAME = data.HR_PR_NAME;
                        checkresult.HR_PR_DESC = data.HR_PR_DESC;
                        checkresult.HR_UpdatedBy = data.Userid;
                        checkresult.HR_UpdatedDate = indiantime0;
                        _context.Update(checkresult);



                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {


                    var checkduplicate = _context.HR_PROCESS_PRIVILEGEDTO.Where(a =>  a.HR_PR_ID == data.HR_PR_ID).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                        HR_PROCESS_PRIVILEGE obj = new HR_PROCESS_PRIVILEGE();
                        obj.HR_PR_ID = data.HR_PR_ID;
                        obj.HR_PR_NAME = data.HR_PR_NAME;
                        obj.HR_PR_DESC = data.HR_PR_DESC;
                        obj.HR_UpdatedBy = data.Userid;
                        obj.HR_UpdatedDate = indiantime0;
                        obj.HR_CreatedBy = data.Userid;      
                                obj.HR_CreatedDate = indiantime0;
                                _context.Add(obj);
                         
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public MasterActivitiesDTO deactiveY(MasterActivitiesDTO data)
        {
            //try
            //{
            //    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            //    var checkactivestatus = _context.External_TrainingDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id == data.HREXTTRN_Id);

            //    if (checkactivestatus.HREXTTRN_ActiveFlag == true)
            //    {
            //        var resultdeactive = _context.External_TrainingDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id == data.HREXTTRN_Id);

            //        if (resultdeactive.HREXTTRN_ActiveFlag == true)
            //        {
            //            resultdeactive.HREXTTRN_ActiveFlag = false;
            //        }
            //        else
            //        {
            //            resultdeactive.HREXTTRN_ActiveFlag = true;
            //        }

            //        resultdeactive.HREXTTRN_UpdatedDate = indiantime0;
            //        resultdeactive.HREXTTRN_UpdatedBy = data.Userid;
            //        _context.Update(resultdeactive);

            //        var i = _context.SaveChanges();
            //        if (i > 0)
            //        {
            //            data.returnval = true;
            //        }
            //        else
            //        {
            //            data.returnval = false;
            //        }

            //    }
            //    else
            //    {
            //        var resultdeactive = _context.External_TrainingDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id == data.HREXTTRN_Id);

            //        if (resultdeactive.HREXTTRN_ActiveFlag == true)
            //        {
            //            resultdeactive.HREXTTRN_ActiveFlag = false;
            //        }
            //        else
            //        {
            //            resultdeactive.HREXTTRN_ActiveFlag = true;
            //        }

            //        resultdeactive.HREXTTRN_UpdatedDate = indiantime0;
            //        resultdeactive.HREXTTRN_UpdatedBy = data.Userid;
            //        _context.Update(resultdeactive);

            //        var i = _context.SaveChanges();
            //        if (i > 0)
            //        {
            //            data.returnval = true;
            //        }
            //        else
            //        {
            //            data.returnval = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }




    }
}
