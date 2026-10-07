using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Student_Login_History")]
    public class PreadmisionLoginHistory:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long MI_Id { get; set; }
        public long IVRMSTUUL_Id { get; set; }
        public DateTime PASLH_LoginDateTime { get; set; }
        public DateTime PASLH_LogoutDateTime { get; set; }
        public string PASLH_MAACAdd { get; set; }
        public string PASLH_IPAdd { get; set; }
        public string PASLH_Attempt { get; set; }


    }
}
