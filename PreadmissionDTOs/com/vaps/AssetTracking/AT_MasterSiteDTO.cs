using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class AT_MasterSiteDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }

        public string MI_logo { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public string staffcheck { get; set; }
        public long INVMSI_Id { get; set; }
        public long MI_Id { get; set; }
        public long MI_Id_new { get; set; }
        public long UserId { get; set; }
        public string INVMSI_SiteBuildingName { get; set; }
        public string INVMSI_SiteRemarks { get; set; }
        public bool INVMSI_ActiveFlg { get; set; }
        public Array get_sites { get; set; }
        public Array get_sitereport { get; set; }
        public Array get_mi_list { get; set; }
        public sitearrayDTO[] sitearray { get; set; }
        public string MI_Logo { get; set; }
    }

    public class sitearrayDTO
    {
        public long INVMSI_Id { get; set; }
        public string INVMSI_SiteBuildingName { get; set; }
    }
}
