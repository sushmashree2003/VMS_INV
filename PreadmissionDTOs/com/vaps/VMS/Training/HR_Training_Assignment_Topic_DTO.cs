using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Assignment_Topic_DTO
    {

        public long MI_Id { get; set; }
        public long userId { get; set; }
        public Array training { get; set; }
        public Array saveFOData { get; set; }
        public long HRMTT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string cfilepath { get; set; }
        public string HRETF_Remarks { get; set; }
        public string HRETF_Status { get; set; }
        public string HRETF_FileType { get; set; }
        public string cfilename { get; set; }
        public string HRETF_FilePath { get; set; }
        public string HRETF_FileName { get; set; }
        public string HRRemarks { get; set; }
        public long UserId { get; set; }




    }
}
