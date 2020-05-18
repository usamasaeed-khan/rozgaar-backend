using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class PostedJobs
    {
        public  int Posted_id { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int MaxCVs { get; set; }
        public int NoOfPositions { get; set; }
        public int EmployeeIDFK { get; set; }   //Employer ID
        public string CategoryName { get; set; }
        public int CategoriesID_FK { get; set; }
        public string Location { get; set; }
        public DateTime PostDate { get; set; }
        public byte[] EmployerImage { get; set; }
        public string EmployerName { get; set; }

        public PostedJobs(int id,string desc,DateTime deadline,int max,int positions,int empid,int catid,string loc,DateTime postDate,byte[] empImage,string empName)
        {
            this.Posted_id = id;
            this.Description = desc;
            this.Deadline = deadline;
            this.MaxCVs = max;
            this.NoOfPositions = positions;
            this.EmployeeIDFK = empid;
            this.CategoriesID_FK = catid;
            this.Location = loc;
            this.PostDate = postDate;
            this.EmployerImage = empImage;
            this.EmployerName = empName;
        }
    }
}