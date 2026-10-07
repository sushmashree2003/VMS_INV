using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_Stream_Group_Mapping")]
    public class Fee_Master_Stream_Group_MappingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSGM_Id { get; set; }
        public long FMG_Id { get; set; }
        public long PASL_ID { get; set; }
        public long FMSGM_Active { get; set; }

        public long ASMCL_ID { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }

    }
}
