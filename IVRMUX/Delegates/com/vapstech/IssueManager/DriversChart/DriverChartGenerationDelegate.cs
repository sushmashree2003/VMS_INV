using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.DriversChart
{
    public class DriverChartGenerationDelegate
    {

        CommonDelegate<DriverChartGenerationDTO, DriverChartGenerationDTO> comm = new CommonDelegate<DriverChartGenerationDTO, DriverChartGenerationDTO>();
        public DriverChartGenerationDTO getdetails(DriverChartGenerationDTO data)
        {
            return comm.POSTIssueManager(data, "DriverChartGenerationFacade/getdetails");
        }

        public DriverChartGenerationDTO savedata(DriverChartGenerationDTO data)
        {
            return comm.POSTIssueManager(data, "DriverChartGenerationFacade/savedata");
        }
    }
}
