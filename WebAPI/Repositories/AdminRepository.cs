using System;
using CavemanTools;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebAPI.Models;
using System.Configuration;

namespace WebAPI.Repositories
{
    public class AdminRepository
    {
        public static bool AddEmployeeToDB(Admin Employer)
        {

            bool isSuccessful = false;


            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "INSERT INTO " +

                "[dbo].[Employer] (EmployeeName,Description,Email,Password,EmployerImage) " +

                "VALUES ('" + Employer.EmployeeName + "','" + Employer.Description + "','" + Employer.Email + "','"

                //+ HashPassword(Employer.Password) + "',(SELECT * FROM OPENROWSET(BULK N'"+ Employer.EmployerImage + "', SINGLE_BLOB) as T1))";
                + HashPassword(Employer.Password) + "',(SELECT * FROM OPENROWSET(BULK N'D:\\HCI\\HCi Project\\facebook.png', SINGLE_BLOB) as T1))";


            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = null;


            try
            {

                connection.Open();

                command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();

                isSuccessful = true;

            }

            catch
            {
                isSuccessful = false;
            }

            finally
            {
                command.Dispose();

                connection.Close();
            }
            //if (isSuccessful)
            //{
            //    using (System.IO.StreamWriter file =
            //    new System.IO.StreamWriter(@"D:\RozgaarRecords\EmployerRecords.txt", true))
            //    {
            //        file.WriteLine("New Employer Added at :" + DateTime.Now + ".\tName: " + Employer.EmployeeName + "\tEmail: " + Employer.Email);
            //    }
            //}

            return isSuccessful;

        }
        public static int GetID(string Email)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "SELECT Employee_ID FROM [dbo].[Employer] WHERE Email = '" + Email + "'";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                var result = command.ExecuteScalar();

                command.Dispose();

                connection.Close();

                return Convert.ToInt32(result);
            }
            catch
            {

                connection.Close();
                return 0;
            }

        }


        public static bool LoginAdmin(string Email, string Password)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "SELECT Password FROM [dbo].[Employer] WHERE Email = '" + Email + "'";

            bool isSuccessful = false;

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = null;
            try
            {
                connection.Open();

                command = new SqlCommand(query, connection);

                var result = command.ExecuteScalar();

                if (result.ToString() == HashPassword(Password)) isSuccessful = true;

                else isSuccessful = false;

            }
            catch
            {
                isSuccessful = false;
            }
            finally
            {
                command.Dispose();

                connection.Close();
            }
            if (isSuccessful)
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"D:\RozgaarRecords\LoginRecords.txt", true))
                {
                    file.WriteLine("Employer with Email: " + Email + "\tLogged in at :" + DateTime.Now + ".");
                }
            }


            return isSuccessful;
        }


        public static string HashPassword(string password)
        {

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();

                byte[] data = md5.ComputeHash(utf8.GetBytes(password));

                return Convert.ToBase64String(data);
            }

        }


        public static bool AddCategory(Category category)
        {
            bool isSuccessful = false;


            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "INSERT INTO " +

                "[dbo].[Categories] (CategoryName,CategoryImage) " +

                "VALUES ('" + category.CategoryName + "','" + category.CategoryImage + "')";



            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = null;


            try
            {

                connection.Open();

                command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();

                isSuccessful = true;

            }

            catch
            {
                isSuccessful = false;
            }

            finally
            {
                command.Dispose();

                connection.Close();
            }
            if (isSuccessful)
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"D:\RozgaarRecords\CategoryRecords.txt", true))
                {
                    file.WriteLine("Category: " + category.CategoryName + "\tAdded At :" + DateTime.Now + ".");
                }
            }


            return isSuccessful;
        }


        public static bool PostJob(PostedJobs postJob)
        {
            if(postJob.CategoriesID_FK == 0 || postJob.CategoryName != null)
                 postJob.CategoriesID_FK = GetCategoryID(postJob.CategoryName);

            bool isSuccessful = false;


            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "INSERT INTO " +

                "[dbo].[PostedJobs] (Description,Deadline,MaxCVs,NoOfPostions,EmployeeIDFK,CategoriesID_FK,Location,PostDate) " +

                "VALUES ('" + postJob.Description + "','" + postJob.Deadline + "'," + postJob.MaxCVs + ","

                + postJob.NoOfPositions + "," + postJob.EmployeeIDFK +
                
                "," + postJob.CategoriesID_FK + ",'" + postJob.Location + "','" + DateTime.Now.Date + "')";



            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = null;


            try
            {

                connection.Open();

                command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();

                isSuccessful = true;

            }

            catch(Exception e)
            {
                isSuccessful = false;
            }

            finally
            {
                command.Dispose();

                connection.Close();
            }
            if (isSuccessful)
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"D:\RozgaarRecords\PostedJobRecords.txt", true))
                {
                    file.WriteLine("EmployerID: " + postJob.EmployeeIDFK + " posted new Job. \tAt :" + DateTime.Now + ".");
                }
            }

            return isSuccessful;

        }



        public static int GetCategoryID(string CategoryName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "SELECT Category_ID FROM [dbo].[Categories] WHERE CategoryName = '" + CategoryName + "'";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                var result = command.ExecuteScalar();

                command.Dispose();

                connection.Close();

                return Convert.ToInt32(result);
            }
            catch
            {
                connection.Close();
                return 0;
            }

        }

    }
}