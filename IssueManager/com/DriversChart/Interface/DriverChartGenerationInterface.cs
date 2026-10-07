using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.DriversChart.Interface
{
    public interface DriverChartGenerationInterface
    {
        DriverChartGenerationDTO getdetails(DriverChartGenerationDTO data);

        DriverChartGenerationDTO savedata(DriverChartGenerationDTO data);
    }
}
