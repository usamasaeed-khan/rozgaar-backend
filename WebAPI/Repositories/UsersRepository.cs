using CavemanTools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Http;
//using System.Web.Mail;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class UsersRepository
    {
        public static bool AddUserToDB(Users user)
        {

            bool isSuccessful = false;


            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "INSERT INTO " +

                "[dbo].[User] (Name,Gender,Domain,DOB,Location,Email,Password) " +

                "VALUES ('" + user.Name + "','" + user.Gender + "','" + user.Domain + "','" 

                + user.DOB.Date + "','" + user.Location + "','" + user.Email + "','"

                + HashPassword(user.Password) + "')";



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
                new System.IO.StreamWriter(@"D:\RozgaarRecords\UserRecords.txt", true))
                {
                    file.WriteLine("New User Added at :"+DateTime.Now+".\tName: "+user.Name+"\tEmail: "+user.Email);
                }
            }

            return isSuccessful;

        }
        //public static Users GetUser(string Email)
        //{
        //    Users user = new Users();

        //    var connectionString = "Data Source=.;Initial Catalog=RozgaarDB;Integrated Security=True";

        //    var query = "SELECT UserID FROM [dbo].[USER] WHERE Email = '" + Email + "'";

        //    SqlConnection connection = new SqlConnection(connectionString);


        //    SqlCommand command = new SqlCommand(query, connection);

        //    try
        //    {
        //        connection.Open();


        //        var result = command.ExecuteScalar();

        //        command.Dispose();

        //        connection.Close();

        //        return ;
        //    }
        //    catch
        //    {
        //        command.Dispose();

        //        connection.Close();

        //        return 0;
        //    } 

        //}









        public static Users GetUser(string Email)
        {
            Users user = new Users();

            SqlDataReader reader ;

            var query = "SELECT  UserID,Name,Gender,Domain,DOB,Location,Email,Password FROM [dbo].[User] where Email = '" + Email +"'";

            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);

            using (connection)
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            user.UserID = reader.GetInt32(0);
                            user.Name = reader.GetString(1);
                            user.Gender = reader.GetString(2).ToCharArray()[0];
                            user.Domain = reader.GetString(3);
                            user.DOB = reader.GetDateTime(4);
                            user.Location = reader.GetString(5);
                            user.Email = reader.GetString(6);
                            user.Password = reader.GetString(7);
                        }
                    }
                    reader.Close();
                    if (reader == null) return null;
                    
                }
                catch
                {
                    return null;
                }
                finally
                {

                    connection.Close();

                    command.Dispose();

                }

            }
            return user;

        }





        public static bool LoginUser(string Email, string Password)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "SELECT Password FROM [dbo].[USER] WHERE Email = '" + Email + "'";

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
                    file.WriteLine("User with Email: " + Email+"\tLogged in at :" + DateTime.Now + ".");
                }
            }

            return isSuccessful;
        }


        public static string HashPassword(string password)
        {

            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();

                byte[] data = md5.ComputeHash(utf8.GetBytes(password));

                return Convert.ToBase64String(data);
            }

        }

        public static List<Category> GetCategories()
        {

            List<Category> categories = new List<Category>();

            SqlDataReader reader = null;

            var query = "SELECT  Category_ID, CategoryName, CategoryImage FROM Categories";

            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);

            using (connection)
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category(reader.GetInt32(0),reader.GetString(1),(byte[])reader.GetValue(2)));
                        }
                    }
                }
                catch(Exception e)
                {
                    categories.Add(new Category(0, "Error Occurred.", e.Message.ToByteArray()));
                }
                finally
                {

                    reader.Close();

                    connection.Close();

                    command.Dispose();

                }

            }
            return categories;
        }


        public static List<PostedJobs> GetPostedJobs()
        {

            List<PostedJobs> jobs = new List<PostedJobs>();

            SqlDataReader reader = null;

            var query = "SELECT  Posted_id, Description, Deadline, MAXCVs,NoOfPostions,EmployeeIDFK,CategoriesID_FK,Location,PostDate FROM PostedJobs";

            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);

            using (connection)
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            byte[] empImage = GetEmployerImage(reader.GetInt32(5));
                            string empName = GetEmployerName(reader.GetInt32(5));
                            jobs.Add(new PostedJobs(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2).Date,reader.GetInt32(3),reader.GetInt32(4),reader.GetInt32(5),reader.GetInt32(6),reader.GetString(7),reader.GetDateTime(8).Date,empImage,empName));
                        }
                    }
                }
                catch ( Exception e)
                {
                    return null;
                }
                finally
                {

                    reader.Close();

                    connection.Close();

                    command.Dispose();

                }

            }
            return jobs;
        }


        public static List<PostedJobs> GetPostedJobsByCategory(int CategoryID)
        {

            List<PostedJobs> jobs = new List<PostedJobs>();

            SqlDataReader reader = null;

            var query = "SELECT  Posted_id, Description, Deadline, MAXCVs,NoOfPostions,EmployeeIDFK,CategoriesID_FK," +

                "Location,PostDate FROM PostedJobs WHERE [CategoriesID_FK] = " + CategoryID + ";";

            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);

            using (connection)
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            byte[] empImage = GetEmployerImage(reader.GetInt32(5));
                            string empName = GetEmployerName(reader.GetInt32(5));
                            jobs.Add(new PostedJobs(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2).Date, reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetString(7), reader.GetDateTime(8).Date, empImage, empName));
                        }
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {

                    reader.Close();

                    connection.Close();

                    command.Dispose();

                }

            }
            return jobs;
        }




        public static byte[] GetEmployerImage(int EmployerID)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "SELECT EmployerImage FROM [dbo].[Employer] WHERE Employee_ID = '" + EmployerID + "'";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                var result = command.ExecuteScalar();

                command.Dispose();

                connection.Close();

                return (byte[])result;
            }
            catch
            {
                connection.Close();
                return null;
            }

        }
        public static string GetEmployerName(int EmployerID)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "SELECT EmployeeName FROM [dbo].[Employer] WHERE Employee_ID = '" + EmployerID + "'";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                var result = command.ExecuteScalar();

                command.Dispose();

                connection.Close();

                return result.ToString();
            }
            catch
            {
                connection.Close();
                return null;
            }

        }

        public static bool ApplyJob(AppliedJobs job)
        {
            bool isSuccessful = false;


            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "INSERT INTO " +

                "[dbo].[AppliedJobs] (UserID_FK,PostedJobID_FK,ApplyDate) " +

                "VALUES (" + job.UserID_FK + "," + job.PostedJobID_FK + ",'" + DateTime.Now.Date + "')";



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
                    new System.IO.StreamWriter(@"D:\RozgaarRecords\AppliedJobRecords.txt", true))
                    {
                        file.WriteLine("UserID: "+job.UserID_FK + " applied to JobID: " + job.PostedJobID_FK+"\tAt :" + DateTime.Now + ".");
                    }
                }

            return isSuccessful;
        }


        public static bool DoesUserExist(string email)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "SELECT Email FROM [dbo].[User] WHERE Email = '" + email + "'";



            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                var result = command.ExecuteScalar();

                command.Dispose();

                connection.Close();

                return result.ToString().Equals(email);
            }
            catch
            {
                connection.Close();

                return false;
            }

        }


        public static bool UpdatePassword(string email)
        {
            string newPassword = RandomString(10);


            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "UPDATE [dbo].[User] SET [Password] = '" + HashPassword(newPassword)  + "' WHERE [Email] = '" + email + "'";


            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                var result = command.ExecuteNonQuery();

                command.Dispose();

                connection.Close();

                SendEmail(email, newPassword);


                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"D:\RozgaarRecords\PasswordUpdateRecords.txt", true))
                {
                    file.WriteLine("Email: " + email + " Updated Password At: " + DateTime.Now + ".");
                }




                return true;
            }
            catch(Exception e)
            {
                connection.Close();

                return false;
            }



        }



        public static bool AddCV(string file)
        {
            bool isSuccessful = false;


            var connectionString = ConfigurationManager.ConnectionStrings["RozgaarConnectionString"].ConnectionString;

            var query = "INSERT INTO " +

                "[dbo].[CV] ([file]) " +

                "VALUES ((SELECT * FROM OPENROWSET(BULK N'D:\\Files\\codewars problemset_beta.pdf', SINGLE_BLOB) as T1))";



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
                using (System.IO.StreamWriter file1 =
                        new System.IO.StreamWriter(@"D:\RozgaarRecords\CVRecords.txt", true))
                {
                    file1.WriteLine("New CV uploaded at: " + DateTime.Now + ".");
                }
            }

            return isSuccessful;
        }







        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private static void SendEmail(string email,string newPassword)
        {

            try
            {
                MailMessage mail = new MailMessage();

                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("k163884@nu.edu.pk");

                mail.To.Add(email);

                mail.Subject = "ROZGAAR ACCOUNT PASSWORD RESET";

                mail.Body = "This is to inform you that your Rozgaar Account Password for email: "+email+"" +

                    " has been changed.\n\nThe updated password is: "+newPassword+".\n\n\nIncase, you have not" +

                    " updated you can reply to this mail.\n\nThanks.";

                smtpServer.Port = 587;

                smtpServer.Credentials = new System.Net.NetworkCredential("k163884@nu.edu.pk", "32672slate");

                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);

            }
            catch(Exception e)
            {

            }

        }


    }




   


}