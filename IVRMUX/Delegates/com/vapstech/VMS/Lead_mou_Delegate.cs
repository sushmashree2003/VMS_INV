using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using PreadmissionDTOs.com.vaps.VMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class Lead_mou_Delegate
    {
  

        CommonVMSDelegate<Lead_Target_MappingDTO, Lead_Target_MappingDTO> COMMC = new CommonVMSDelegate<Lead_Target_MappingDTO, Lead_Target_MappingDTO>();


        public Lead_Target_MappingDTO onloadgetdetails(Lead_Target_MappingDTO  dto)
        {
            return COMMC.POSTData(dto, "Lead_mou_Facade/OnloadGetDetails/");
        }
        public Lead_Target_MappingDTO lead_product(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_mou_Facade/lead_product/");
        }

        public Lead_Target_MappingDTO savedata(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_mou_Facade/savedata/");
        }

        public Lead_Target_MappingDTO MOUViewdata(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_mou_Facade/MOUViewdata/");
        }

        public Lead_Target_MappingDTO sendmail(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_mou_Facade/sendmail/");
        }

        




    }
}
