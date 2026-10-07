using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class FrontOfficeLogReportDelegate
    {
        CommonDelegate<FrontOfficeLogReportDTO, FrontOfficeLogReportDTO> _commnbranch = new CommonDelegate<FrontOfficeLogReportDTO, FrontOfficeLogReportDTO>();

        public FrontOfficeLogReportDTO Savedata(FrontOfficeLogReportDTO data)
        {
            return _commnbranch.POSTDataHolidayReport(data, "FrontOfficeLogReportFacade/Savedata/");
        }
    }
}

