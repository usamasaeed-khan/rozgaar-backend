using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Processors
{
    public class UsersProcessor
    {
        public static bool ProcessUser(Users user)
        {
            return UsersRepository.AddUserToDB(user);
        }

        public static bool Login(string Email,string Password)
        {
            return UsersRepository.LoginUser(Email, Password);
        }
        
        public static Users GetUser(string Email)
        {
            return UsersRepository.GetUser(Email);
        }

        public static List<Category> GetCategories()
        {
            return UsersRepository.GetCategories();
        }

        public static List<PostedJobs> GetJobs()
        {
            return UsersRepository.GetPostedJobs();
        }

        public static bool ApplyForJob(AppliedJobs job)
        {
            return UsersRepository.ApplyJob(job);
        }

        public static List<PostedJobs> GetJobsByCategoryID(int CategoryID)
        {
            return UsersRepository.GetPostedJobsByCategory(CategoryID);
        }

        public static bool CheckUSer(string email)
        {
            return UsersRepository.DoesUserExist(email);
        }
    }
}