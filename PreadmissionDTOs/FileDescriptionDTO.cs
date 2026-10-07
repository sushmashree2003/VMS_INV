using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PreadmissionDTOs
{
    public class FileDescriptionDTO : CommonParamDTO
    {
        public int Id { get; set; } 

        public string Description { get; set; }

        public string Name { get; set; }

        public ICollection<IFormFile> File { get; set; }

        public ICollection<IFormFile> Logo { get; set; }

        public string Email_id { get; set; }
        public string Mobileno { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string folder { get; set; }

        public string Alumni { get; set; }

        public long? admittedyer  { get; set; }

        public long? leftyear { get; set; }
        public long? classidadmitted { get; set; }
        public long? leftclassid  { get; set; }

        public string OTPSTATUS { get; set; }

        public string Otpmobl { get; set; }
        public string Otpemail { get; set; }

        public string Special { get; set; }

        public string onlinepreadminfilepath { get; set; }

        public List<Master_NumberingDTO> transnumbconfigurationsettingsss { get; set; }

        public Array PaymentDetailsList { get; set; }

        public long Mi_id { get; set; }

        public string Base64string { get; set; }

        public long? courseadmittted { get; set; }

        public long? courseleft { get; set; }
        public long? branchadmittted { get; set; }
        public long? branchleft { get; set; }

        public long? semadmittted { get; set; }
        public long? semleft { get; set; }
    }
}
