using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using WebAPI.Processors;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    public class AdminController : ApiController
    {
        [HttpPost]
        [Route("Employer/Signup")]
        public bool SignUpAdmin(Admin admin)
        {
            return AdminProcessor.SignUp(admin);
        }


        [HttpPost]
        [Route("Employer/Login")]
        public bool LoginAdmin(Admin admin)
        {
            return AdminProcessor.Login(admin.Email, admin.Password);
        }

        [HttpGet]
        [Route("Employer/GetID")]
        public int GetID(String Email)
        {
            return AdminProcessor.GetID(Email);
        }

        [HttpPost]
        [Route("Employer/AddCategory")]
        public bool AddCategory(Category category)
        {
            return AdminProcessor.AddCategory(category);
        }



        [HttpPost]
        [Route("Employer/PostJob")]
        public bool PostJob(PostedJobs postjob)
        {
            return AdminProcessor.PostJob(postjob);
        }

    }
}
