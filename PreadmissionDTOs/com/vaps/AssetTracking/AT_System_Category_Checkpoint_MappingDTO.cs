using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class AT_System_Category_Checkpoint_MappingDTO
    {
        public long ATSCCM_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ISMMPR_Id { get; set; }
        public long MS_Id { get; set; }
        public long MC_Id { get; set; }
        public long MCP_Id { get; set; }
        public int TotalItems { get; set; }

        public ProjectDropdownDTO[] projectlist { get; set; }
        public SystemDropdownDTO[] systemlist { get; set; }
        public CategoryDropdownDTO[] categorylist { get; set; }
        public CheckpointDropdownDTO[] checkpointlist { get; set; }
        public GridMappingDTO[] alldata { get; set; }

        public List<AT_System_Category_Checkpoint_MappingDTO> selectedCheckpoints { get; set; }

        public bool returnValue { get; set; }
        public bool duplicate { get; set; }
        public bool? ActiveFlag { get; set; }
        public string message { get; set; }
    }
    public class GridMappingDTO
    {
        public long ISMMPR_Id { get; set; }
        public long ATSCCM_Id { get; set; }
        public long MS_Id { get; set; }
        public long MC_Id { get; set; }
        public string SystemName { get; set; }
        public string CategoryName { get; set; }
        public string ProjectName {  get; set; }
        public int TotalItems { get; set; }
        public bool? ActiveFlag { get; set; }
    }
    public class ProjectDropdownDTO
    {
        public long ISMMPR_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
    }

    public class SystemDropdownDTO
    {
        public long MS_Id { get; set; }
        public string System { get; set; }
    }

    public class CategoryDropdownDTO
    {
        public long MC_Id { get; set; }
        public string Category { get; set; }
    }

    public class CheckpointDropdownDTO
    {
        public long MCP_Id { get; set; }
        public string CheckPoint { get; set; }
        public bool selected { get; set; }
    }
}
