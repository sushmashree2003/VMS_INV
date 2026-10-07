using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Lead_Target_MappingInterface
    {
        Lead_Target_MappingDTO getBasicData(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO OnloadGetDetailsnego(Lead_Target_MappingDTO dto);
        
        Lead_Target_MappingDTO OnChangeOnChangeleadlist(Lead_Target_MappingDTO dto);

        Lead_Target_MappingDTO OnChangeOnChangeproductlist(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO OnChangeOnChangeBOMList(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO Saveboomlistpackage(Lead_Target_MappingDTO dto);
        Task<Lead_Target_MappingDTO> sendmailDetails(Lead_Target_MappingDTO data);
      
        Lead_Target_MappingDTO Edit_LeadTargetMapping(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO savedetails(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO get_quotation(Lead_Target_MappingDTO dto);

        Lead_Target_MappingDTO saveNegotiationdetails(Lead_Target_MappingDTO dto);

        Lead_Target_MappingDTO OnChangeLTM_Institution(Lead_Target_MappingDTO dto);
        Task<Lead_Target_MappingDTO> sendProDetails(Lead_Target_MappingDTO data);

        //SendMdApproval
        Task<Lead_Target_MappingDTO> SendMdApproval(Lead_Target_MappingDTO data);
        Task<Lead_Target_MappingDTO> MdApproval(Lead_Target_MappingDTO data);
        Lead_Target_MappingDTO deactivate(Lead_Target_MappingDTO data);
        Lead_Target_MappingDTO getdetails(int id);


        Task<Lead_Target_MappingDTO> SaveScienceParkTransaction(Lead_Target_MappingDTO data);
        Task<Lead_Target_MappingDTO> SaveISMSalesMultipleTransaction (Lead_Target_MappingDTO data);
    }
}
