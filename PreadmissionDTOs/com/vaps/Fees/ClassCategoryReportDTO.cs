using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class ClassCategoryReportDTO
    {
        public long ASMAY_Id { get; set; }

        public long MI_Id { get; set; }

        public Array YearList { get; set; }
        public string FMCC_ClassCategoryName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string  ASMCL_ClassName { get; set; }
        public Array searchdatalist { get; set; }

    }
}
