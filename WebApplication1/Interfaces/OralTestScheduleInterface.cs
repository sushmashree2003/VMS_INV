using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface OralTestScheduleInterface
    {
        Task<OralTestScheduleDTO> OralTestScheduleData(OralTestScheduleDTO mas);

        OralTestScheduleDTO OralTestScheduleDeletesData(int ID);

        OralTestScheduleDTO OralTestScheduleDeletesStudentData(OralTestScheduleDTO OralTestScheduleDTO);

        StudentDetailsDTO classwisestudent(StudentDetailsDTO classwisestudent);
        OralTestScheduleDTO GetSelectedRowDetails(int ID);

        StudentDetailsDTO GetSelectedStudentData(int ID);

        StudentDetailsDTO GetOralTestScheduleData(StudentDetailsDTO StudentDetailsDTO);
      
    }
}
