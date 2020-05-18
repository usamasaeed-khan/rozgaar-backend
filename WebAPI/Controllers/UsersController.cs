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
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("User/Signup")]
        public bool AddUser(Users user)
        {
            if (user == null)
            {
                return false;
            }
            return UsersProcessor.ProcessUser(user);
        }

        [HttpPost]
        [Route("User/Login")]
        public bool LoginUser(Users user)
        {
            return UsersProcessor.Login(user.Email,user.Password);
        }


        [HttpGet]
        [Route("User/GetUser")]
        public HttpResponseMessage GetUser(string Email)
        {
            Users user = UsersProcessor.GetUser(Email);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found.");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
        }


        [HttpGet]
        [Route("User/GetCategories")]
        public List<Category> GetCategories()
        {
            return UsersProcessor.GetCategories();
        }

        [HttpGet]
        [Route("User/GetJobs")]
        public List<PostedJobs> GetJobs()
        {
            return UsersProcessor.GetJobs();
        }

        [HttpPost]
        [Route("User/Apply")]
        public bool ApplyForJob(AppliedJobs job)
        {
            return UsersProcessor.ApplyForJob(job);
        }


        [HttpGet]
        [Route("User/GetJobsByCategoryID")]
        public List<PostedJobs> GetJobsByCat(int CategoryID)
        {
            return UsersProcessor.GetJobsByCategoryID(CategoryID);
        }


        [HttpGet]
        [Route("User/CheckIfUserExists")]
        public bool CheckUserStatus(string email)
        {
            return UsersProcessor.CheckUSer(email);
        }

        [HttpGet]
        [Route("User/ChangePass")]
        public bool ChangePass(string email)
        {
            return UsersRepository.UpdatePassword(email);
        }


        [HttpPost]
        [Route("User/UploadCV")]
        public bool upload([FromBody]string file)
        {
            return UsersRepository.AddCV(file);
        }

    }
}
