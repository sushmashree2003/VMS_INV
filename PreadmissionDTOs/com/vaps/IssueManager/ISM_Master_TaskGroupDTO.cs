using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class ISM_Master_TaskGroupDTO
    {
        public long ISMMTGRP_Id { get; set; }
       
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long? ISMMTGRP_CreatedByDeptId { get; set; }
        public string ISMMTGRP_TaskGroupName { get; set; }
        public string ISMMTGRP_TGRemarks { get; set; }
        public bool ISMMTGRP_TGFinishedFlg { get; set; }
        public bool ISMMTGRP_ActiveFlag { get; set; }
        public long ISMMTGRP_CreatedBy { get; set; }
        public long ISMMTGRP_UpdatedBy { get; set; }
        public DateTime? ISMMTGRP_CreatedDate { get; set; }
        public DateTime? ISMMTGRP_UpdatedDate { get; set; }

        public ISM_Master_TaskGroupDTO[] deptid { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Array alldata { get; set; }
        public Array department_list { get; set; }
        public Array editlist { get; set; }
        public Array viewuploaddept { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string msg { get; set; }



        public long ISMMTGRPD_Id { get; set; }
        //public long ISMMTGRP_Id { get; set; }
        //public long HRMD_Id { get; set; }
        public long ISMMTGRPD_CreatedBy { get; set; }
        public long ISMMTGRPD_UpdatedBy { get; set; }
        public bool? ISMMTGRPD_ActiveFlag { get; set; }
        public DateTime? ISMMTGRPD_CreatedDate { get; set; }
        public DateTime? ISMMTGRPD_UpdatedDate { get; set; }
    }
}
