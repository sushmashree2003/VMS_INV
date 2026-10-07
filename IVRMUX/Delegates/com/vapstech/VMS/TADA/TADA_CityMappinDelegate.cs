using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADA_CityMappinDelegate
    {


        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TRDACityMappingDTO, TRDACityMappingDTO> COMMI = new CommonDelegate<TRDACityMappingDTO, TRDACityMappingDTO>();


        public TRDACityMappingDTO loaddata(TRDACityMappingDTO id)
        {
        
            
           return COMMI.POSTIssueManager(id, "TADA_CityMappingFacadE/loaddata/");
        }
        public TRDACityMappingDTO saverecord(TRDACityMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_CityMappingFacadE/saverecord/");
        }
        public TRDACityMappingDTO edit(TRDACityMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_CityMappingFacadE/edit/");
        }
        //deactive
        public TRDACityMappingDTO deactive(TRDACityMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_CityMappingFacadE/deactive/");
        }
        public TRDACityMappingDTO getcity(TRDACityMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_CityMappingFacadE/getcity/");
        }
        //getMasterReport
        public TRDACityMappingDTO getMasterReport(TRDACityMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_CityMappingFacadE/getMasterReport/");
        }
    }
}
