using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.OT
{
    public class QuestionPaperTypeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<QuestionPaperTypeDTO, QuestionPaperTypeDTO> COMMM = new CommonDelegate<QuestionPaperTypeDTO, QuestionPaperTypeDTO>();

        public QuestionPaperTypeDTO getalldetails(QuestionPaperTypeDTO dto)
        {
            return COMMM.POSTVMS(dto, "QuestionPaperTypeFacade/getalldetails");
        }

        public QuestionPaperTypeDTO savedetails(QuestionPaperTypeDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "QuestionPaperTypeFacade/savedetails/");
        }
        public QuestionPaperTypeDTO loadcdata(QuestionPaperTypeDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "QuestionPaperTypeFacade/loadcdata/");
        }
        public QuestionPaperTypeDTO editData(int id)
        {
            return COMMM.GetVMS(id, "QuestionPaperTypeFacade/editData/");
        }
        public QuestionPaperTypeDTO deactivate(QuestionPaperTypeDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "QuestionPaperTypeFacade/deactivate/");
        }

       

       

    }
}
