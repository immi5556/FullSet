using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sj.Mg.Idsrv1.Models
{
    public class ExternalRegistrationModel
    {
        [Required]
        public string First { get; set; }
        [Required]
        public string Last { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Address { get; set; }
        public string Password { get; set; }

        public string Ans1 { get; set; }
        
        public string Ans2 { get; set; }
        
        public string Ans3 { get; set; }
        
        public string Ans4 { get; set; }
        
        public string Ans5 { get; set; }
        
        public string Ans6 { get; set; }
        
        public string Ans7 { get; set; }
    }
}