using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Rating.Interfacce
{
   public interface TypeQuestionOptionMappingInterface
    {
        Rating_ReportDTO OnLoadRatingMasterType(Rating_ReportDTO data);

        Task<Rating_ReportDTO> getReport(Rating_ReportDTO data);

    

        Task<Rating_ReportDTO> getDeptReport(Rating_ReportDTO data);
    }
}
