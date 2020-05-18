using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Processors
{
    public class AdminProcessor
    {
        public static bool SignUp(Admin admin)
        {
            return AdminRepository.AddEmployeeToDB(admin);
        }

        public static bool Login(string Email, string Password)
        {
            return AdminRepository.LoginAdmin(Email, Password);
        }

        public static int GetID(string Email)
        {
            return AdminRepository.GetID(Email);
        }

        public static bool AddCategory(Category category)
        {
            return AdminRepository.AddCategory(category);
        }


        public static bool PostJob(PostedJobs postjob)
        {
            return AdminRepository.PostJob(postjob);
        }

    }
}