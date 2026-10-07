using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Rating
{
    public class TypeQuestionOptionMappingDelegate
    {
        CommonDelegate<Rating_ReportDTO, Rating_ReportDTO> _comm = new CommonDelegate<Rating_ReportDTO, Rating_ReportDTO>();

        public Rating_ReportDTO OnLoadRatingMasterType(Rating_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "TypeQuestionOptionMappingFacade/OnLoadRatingMasterType");
        }
        public Rating_ReportDTO getReport(Rating_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "TypeQuestionOptionMappingFacade/getReport");
        }

        public Rating_ReportDTO OnLoadRatingMasterDeptType(Rating_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "TypeQuestionOptionMappingFacade/OnLoadRatingMasterDeptType");
        }
        public Rating_ReportDTO getDeptReport(Rating_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "TypeQuestionOptionMappingFacade/getDeptReport");
        }
    }
}
