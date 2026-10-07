using System;

namespace PreadmissionDTOs.com.vaps.Inventory
{

    public class MasterSystemDTO
    {

        public long MS_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string System { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public Array alldata { get; set; }

        public bool returnValue { get; set; }
        public string successMessage { get; set; }
        public bool? ActiveFlag { get; set; }

    }

}
