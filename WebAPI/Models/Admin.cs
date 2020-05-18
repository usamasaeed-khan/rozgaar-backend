using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Admin
    {
        public string EmployeeName { get; set; }    // Name of the company, orgnaization or an individual.
        public string Description { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmployerImage { get; set; }
    }
}