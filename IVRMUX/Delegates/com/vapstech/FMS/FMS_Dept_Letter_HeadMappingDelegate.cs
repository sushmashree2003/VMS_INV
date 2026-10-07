using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.FMS;

namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class FMS_Dept_Letter_HeadMappingDelegate
    {
        CommonDelegate<FMS_Dept_Letter_HeadMappingDTO, FMS_Dept_Letter_HeadMappingDTO> _comm = new CommonDelegate<FMS_Dept_Letter_HeadMappingDTO, FMS_Dept_Letter_HeadMappingDTO>();

        public FMS_Dept_Letter_HeadMappingDTO LoadData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/LoadData");
        }
        public FMS_Dept_Letter_HeadMappingDTO OnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/OnChangeInstiution");
        }
        public FMS_Dept_Letter_HeadMappingDTO OnSaveData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/OnSaveData");
        }
        public FMS_Dept_Letter_HeadMappingDTO EditLetterHead(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/EditLetterHead");
        }
        public FMS_Dept_Letter_HeadMappingDTO OnDelete(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/OnDelete");
        }

        // Letter Head Tracking
        public FMS_Dept_Letter_HeadMappingDTO TrackingLoadData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/TrackingLoadData");
        }        
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/TrackingOnChangeInstiution");
        }         
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeDept(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/TrackingOnChangeDept");
        }        
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnSaveData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/TrackingOnSaveData");
        }        
        public FMS_Dept_Letter_HeadMappingDTO TrackingEditLetterHead(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/TrackingEditLetterHead");
        }   
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnDelete(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/TrackingOnDelete");
        }

        // Report
        public FMS_Dept_Letter_HeadMappingDTO ReportLoadData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/ReportLoadData");
        }
        public FMS_Dept_Letter_HeadMappingDTO ReportOnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/ReportOnChangeInstiution");
        }
        public FMS_Dept_Letter_HeadMappingDTO ReportOnChangeDept(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/ReportOnChangeDept");
        }
        public FMS_Dept_Letter_HeadMappingDTO GetLetterHeadReport(FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_Dept_Letter_HeadMappingFacade/GetLetterHeadReport");
        }
    }
}
