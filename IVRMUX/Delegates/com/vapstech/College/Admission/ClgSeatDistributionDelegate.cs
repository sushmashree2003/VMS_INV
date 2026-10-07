using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgSeatDistributionDelegate
    {
        CommonDelegate<ClgSeatDistributionDTO, ClgSeatDistributionDTO> _commbranch = new CommonDelegate<ClgSeatDistributionDTO, ClgSeatDistributionDTO>();

        public ClgSeatDistributionDTO getalldetails(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/getalldetails/");
        }
        public ClgSeatDistributionDTO getCoursedata(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/getCoursedata/");
        }
        public ClgSeatDistributionDTO getBranchdata(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/getBranchdata/");

        }
        public ClgSeatDistributionDTO getSemesterdata(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/getSemesterdata/");
        }
        public ClgSeatDistributionDTO get_Category(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/get_Category/");
        }

        public ClgSeatDistributionDTO savedata(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/savedata/");
        }

        public ClgSeatDistributionDTO get_Seattotal(ClgSeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgSeatDistributionFacade/get_Seattotal/");
        }

    }
}
