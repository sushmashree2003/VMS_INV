using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
   public class IVRM_NoticeBoardDTO : CommonParamDTO
    {
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        //----------------------------------------------
        public long INTB_Id { get; set; }
        public long MI_Id { get; set; }
        public string INTB_Title { get; set; }
        public string INTB_Description { get; set; }
        public string INTB_Attachment { get; set; }
        public string INTB_FilePath { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string NTB_TTSylabusFlg { get; set; }
        public bool INTB_DispalyDisableFlg { get; set; }


        public long INTBC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool INTBC_ActiveFlag { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }




        public DateTime? INTB_DisplayDate { get; set; }
        public DateTime INTB_StartDate { get; set; }
        public DateTime INTB_EndDate { get; set; }
        public bool INTB_ActiveFlag { get; set; }
        public Array notice_details { get; set; }
        public Array editdetails { get; set; }
        public Array classlist { get; set; }


        public IVRM_NoticeBoardDTO[] classlst { get; set; }
    }

    public class classlst
    {
        public long ASMCL_Id { get; set; }
    }
}

