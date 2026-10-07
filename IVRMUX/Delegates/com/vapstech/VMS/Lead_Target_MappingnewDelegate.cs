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
    public class Lead_Target_MappingnewDelegate
    {

        CommonVMSDelegate<Lead_Target_MappingnewDTO, Lead_Target_MappingnewDTO> COMMC = new CommonVMSDelegate<Lead_Target_MappingnewDTO, Lead_Target_MappingnewDTO>();


        public Lead_Target_MappingnewDTO onloadgetdetails(Lead_Target_MappingnewDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacadenew/OnloadGetDetails/");
        }

        public Lead_Target_MappingnewDTO get_desig(Lead_Target_MappingnewDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacadenew/get_desig/");
        }
        public Lead_Target_MappingnewDTO get_employee(Lead_Target_MappingnewDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacadenew/get_employee/");
        }
        public Lead_Target_MappingnewDTO getdetails(Lead_Target_MappingnewDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacadenew/getdetails/");
        }
        public Lead_Target_MappingnewDTO getcatdetails(Lead_Target_MappingnewDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacadenew/getcatdetails/");
        }
        public Lead_Target_MappingnewDTO SaveTarget(Lead_Target_MappingnewDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacadenew/SaveTarget/");
        }
        public Lead_Target_MappingnewDTO Edit(Lead_Target_MappingnewDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacadenew/Edit/");
        }


    }
}
