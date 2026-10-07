using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public  class WhatsappDTO
    {
        public string messaging_product { get; set; }
        public string mobileNo { get; set; }
        public string MI_WhatsApptype { get; set; }
        public string type { get; set; }
        public string parameters { get; set; }
        public string text { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string language { get; set; }
        public templete template { get; set; }
        public string to { get; set; }
        public string file { get; set; }
        public ICollection<IFormFile> File { get; set; }
    }
    public class parameter
    {
        public string type { get; set; }
        public string text { get; set; }
       
    }
    public class component
    {
        public string type { get; set; }
        public List<parameter> parameters { get; set; }
    }
    public class document
    {
        public string messaging_product { get; set; }
        public string file { get; set; }
    }
    public class language
    {
        public string code { get; set; }
    }
    public class templete
    {
        public string name { get; set; }
        public language language { get; set; }
        public List<component> components { get; set; }
    }
}
