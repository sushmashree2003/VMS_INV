using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Interface
{
    public interface FMS_Dept_Letter_HeadMappingInterface
    {
        FMS_Dept_Letter_HeadMappingDTO LoadData(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO OnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO OnSaveData(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO EditLetterHead(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO OnDelete(FMS_Dept_Letter_HeadMappingDTO data);

        // Letter Head Tracking
        FMS_Dept_Letter_HeadMappingDTO TrackingLoadData(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeDept(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO TrackingOnSaveData(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO TrackingEditLetterHead(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO TrackingOnDelete(FMS_Dept_Letter_HeadMappingDTO data);

        //Report
        FMS_Dept_Letter_HeadMappingDTO ReportLoadData(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO ReportOnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO ReportOnChangeDept(FMS_Dept_Letter_HeadMappingDTO data);
        FMS_Dept_Letter_HeadMappingDTO GetLetterHeadReport(FMS_Dept_Letter_HeadMappingDTO data);
    }
}
