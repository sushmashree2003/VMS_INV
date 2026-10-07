using System;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class AT_Master_CheckPointDTO
    {
        public long MCP_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string CheckPoint { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public Array alldata { get; set; }

        public bool returnValue { get; set; }
        public bool duplicate { get; set; }
        public string successMessage { get; set; }
        public bool? ActiveFlag { get; set; }
    }
}
