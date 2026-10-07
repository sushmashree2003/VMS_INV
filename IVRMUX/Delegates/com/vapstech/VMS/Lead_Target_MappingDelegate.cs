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
    public class Lead_Target_MappingDelegate
    {
  

        CommonVMSDelegate<Lead_Target_MappingDTO, Lead_Target_MappingDTO> COMMC = new CommonVMSDelegate<Lead_Target_MappingDTO, Lead_Target_MappingDTO>();


        public Lead_Target_MappingDTO onloadgetdetails(Lead_Target_MappingDTO  dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/OnloadGetDetails/");
        }

        public Lead_Target_MappingDTO OnloadGetDetailsnego(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/OnloadGetDetailsnego/");
        }

        public Lead_Target_MappingDTO OnChangeOnChangeleadlist(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/OnChangeOnChangeleadlist/");
        }
        public Lead_Target_MappingDTO OnChangeOnChangeproductlist(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/OnChangeOnChangeproductlist/");
        }
        public Lead_Target_MappingDTO OnChangeOnChangeBOMList(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/OnChangeOnChangeBOMList/");
        }
        public Lead_Target_MappingDTO Saveboomlistpackage(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/Saveboomlistpackage/");
        }
        public Lead_Target_MappingDTO sendmailDetails(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/sendmailDetails/");
        }
        public Lead_Target_MappingDTO savedetails(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/savedetails/");
        }
        public Lead_Target_MappingDTO get_quotation(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/get_quotation/");
        }
        public Lead_Target_MappingDTO saveNegotiationdetails(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/saveNegotiationdetails/");
        }
        
        public Lead_Target_MappingDTO LeadTargetMapping_Edit(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/leadTargetmapping_Edit/");
        }

        public Lead_Target_MappingDTO OnChangeLTM_Institution(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/OnChangeLTM_Institution/");
        }


        public Lead_Target_MappingDTO sendProDetails(Lead_Target_MappingDTO dto)
        {
            return COMMC.POSTData(dto, "Lead_Target_MappingFacade/sendProDetails/");
        }

        public Lead_Target_MappingDTO deactivate(Lead_Target_MappingDTO data)
        {
            return COMMC.POSTData(data, "Lead_Target_MappingFacade/deactivate/");
        }
        //SendMdApproval
        public Lead_Target_MappingDTO SendMdApproval(Lead_Target_MappingDTO data)
        {
            return COMMC.POSTData(data, "Lead_Target_MappingFacade/SendMdApproval/");
        }
        public Lead_Target_MappingDTO MdApproval(Lead_Target_MappingDTO data)
        {
            return COMMC.POSTData(data, "Lead_Target_MappingFacade/MdApproval/");
        }
        public Lead_Target_MappingDTO categoryDet(int id)
        {

            return COMMC.GetDataById(id, "Lead_Target_MappingFacade/getdetails/");
        }
        public Lead_Target_MappingDTO SaveScienceParkTransaction(Lead_Target_MappingDTO data)
        {
            return COMMC.POSTData(data, "Lead_Target_MappingFacade/SaveScienceParkTransaction/");
        }

        public Lead_Target_MappingDTO SaveISMSalesMultipleTransaction(Lead_Target_MappingDTO data)
        {
            return COMMC.POSTData(data, "Lead_Target_MappingFacade/SaveISMSalesMultipleTransaction/");
        }
        
    }
}
