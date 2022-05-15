using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityAppV2.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; }
        public int CourseId { get; set; }

        //Fomatted Dates
       
        public string FDueDate
        {
            get
            {
                return DueDate.ToString("D");
            }
        }
    }
}
