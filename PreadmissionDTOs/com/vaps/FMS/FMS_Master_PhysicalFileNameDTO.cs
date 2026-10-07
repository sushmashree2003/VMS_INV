using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMS_Master_PhysicalFileNameDTO
    {
        public long FMSMPFN_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMSMPFN_PhysicalFileName { get; set; }
        public string FMSMPFN_FileContent { get; set; }
        public string Typetab { get; set; }
        public bool FMSMPFN_ActiveFlg { get; set; }
        public DateTime? FMSMPFN_CreatedDate { get; set; }
        public DateTime? FMSMPFN_UpdatedDate { get; set; }
        public long? FMSMPFN_CreatedBy { get; set; }
        public long? FMSMPFN_UpdatedBy { get; set; }
        public string FMSMPFN_CreatedYear { get; set; }
        public string FMSMPFN_FileNo { get; set; }
        public string FMSMPFN_FileType { get; set; }
        public Array gridviewDetails { get; set; }
        public Array editDetails { get; set; }
        public Array geteditdeptDetails { get; set; }
        public Array getsaveddeptDetails { get; set; }
        public Array dispatchmodeDetails { get; set; }
        public Array filecategoryDetails { get; set; }
        public Array getdepartmentlist { get; set; }
        public Array getinstitutiondetails { get; set; }
        public Array rowdetails { get; set; }
        public Array columndetails { get; set; }
        public Array physicaldetails { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }
        public bool returnval { get; set; }
        public long FMSMFC_Id { get; set; }       
        public string FMSMFC_FileCategoryCode { get; set; }
        public string FMSMFC_FileCategoryName { get; set; }
        public long FMSMDMD_Id { get; set; }
        public string FMSMTMD_DispatchMode { get; set; }
        public string FMSMTMD_DispatchModeDetails { get; set; }
        public long FMSMFCBT_Id { get; set; }
        public string FMSMFCBT_FileCabinetName { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long FMSMFCBT_NoOfColumns { get; set; }
        public long FMSMFCBT_NoOfRows { get; set; }
        public FMS_Master_FileCabinet_RowDTO[] FMS_Master_FileCabinet_Rows { get; set; }
        public FMS_Master_FileCabinet_ColumnDTO[] FMS_Master_FileCabinet_Columns { get; set; }
        public deptlisttemp[] deptlisttemp { get; set; }
        public deptlist_temp[] deptlist_temp { get; set; }
        public Array FileCabinetDetails { get; set; }
        public string HRMB_BuildingName { get; set; }
        public string HRMF_FloorName { get; set; }
        public string HRMR_RoomName { get; set; }
        public bool FMSMFCBT_ActiveFlg { get; set; }
        public long FMSRNCON_Id { get; set; }
        public int FMSRNCON_CriteriaOrder { get; set; }
        public string FMSRNCON_Criterias { get; set; }
        public Array buildingList { get; set; }
        public Array roomList { get; set; }
        public Array floorList { get; set; }
        public long FMSDFC_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string departmentname { get; set; }
        public bool FMSDFC_ActiveFlg { get; set; }
        public Array filenamedetails { get; set; }
        public Array cabinetnamedetails { get; set; }
        public Array gridFileNameCabinetDetails { get; set; }
        public Array filecabinetrow { get; set; }
        public Array filecabinetcolumn { get; set; }
        public Array editphysicalfileandcabinet { get; set; }
        public Array Checkfilecabinet { get; set; }
        public long FMSMPFNFC_Id { get; set; }
        public long FMSMFCC_Id { get; set; }
        public long FMSMFCR_Id { get; set; }
        public string FMSMFCR_CabinetRowName { get; set; }
        public string FMSMFCC_CabinetColumnName { get; set; }
        public long FMSDPFN_Id { get; set; }
        public bool FMSDPFN_ActiveFlg { get; set; }
    }

    public class deptlisttemp
    {
        public long HRMD_Id { get; set; }
        public long FMSDFC_Id { get; set; }
    }

    public class deptlist_temp
    {
        public long HRMD_Id { get; set; }
        public long FMSDPFN_Id { get; set; }
    }
}
