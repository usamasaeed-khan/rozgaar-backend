using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public  char Gender { get; set; }
        public string Domain { get; set; }
        public DateTime DOB { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}