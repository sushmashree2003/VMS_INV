using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    public class HR_Training_Create_DetailsDMO
    {
        public long HRTCRD_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HRTCRD_Date { get; set; }
        public string HRTCRD_StartTime { get; set; }
        public string HRTCRD_EndTime { get; set; }
        public string HRTCRD_Content { get; set; }
        public string HRTCRD_Status { get; set; }
        public long HRTCRD_Rating { get; set; }
        public string HRTCRD_TrainerRemarks { get; set; }
        public bool HRTCRD_ActiveFlg { get; set; }
        public DateTime? HRTCRD_CreatedDate { get; set; }
        public DateTime? HRTCRD_UpdatedDate { get; set; }
        public long HRTCRD_CreatedBy { get; set; }
        public long HRTCRD_UpdatedBy { get; set; }

    }
}
