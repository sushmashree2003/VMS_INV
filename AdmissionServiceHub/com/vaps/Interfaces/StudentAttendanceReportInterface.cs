using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentAtttendanceReportInterface
    {
        StudentAttendanceReportDTO getInitailData(StudentAttendanceReportDTO id);
        Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getDataByTypeSelected(int id);
        Task<StudentAttendanceReportDTO> getdatatype(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getreportdiv(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO savetmpldatanew(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO onchangeyear(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO onclasschange(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO onsectionchange(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getreport(StudentAttendanceReportDTO data);
        
    }
}
