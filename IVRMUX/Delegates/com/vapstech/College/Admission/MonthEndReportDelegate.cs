using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class MonthEndReportDelegate
    {
        CommonDelegate<MonthEndReportDTO, MonthEndReportDTO> _commbranch = new CommonDelegate<MonthEndReportDTO, MonthEndReportDTO>();
        public MonthEndReportDTO getdata(MonthEndReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "MonthEndReportFacade/getdata/");
        }
        public MonthEndReportDTO getreport(MonthEndReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "MonthEndReportFacade/getreport/");
        }
    }
}
