using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class AppliedJobs
    {
        public int UserID_FK { get; set; }
        public int PostedJobID_FK { get; set; }
    }
}