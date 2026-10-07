using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com
{
    public class Master_DRBlocked_ScenarioWiseDto
    {

        public long MDS_id { get; set; }
        public long? HRME_ID { get; set; }

     
        public string Scenario { get; set; }

        public long? ActiveFlag { get; set; }

        public DateTime? MDS_CreatedDate { get; set; }

        public DateTime? MDS_UpdatedDate { get; set; }

        public long? MDS_CreatedBy { get; set; }

        public long? MDS_UpdatedBy { get; set; }

        public long? User_Id { get; set; }
    }
}
