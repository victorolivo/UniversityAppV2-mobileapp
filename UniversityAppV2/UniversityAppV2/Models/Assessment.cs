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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public int CourseId { get; set; }

        //Fomatted Dates
        public string FStartDate
        {
            get
            {
                return StartDate.ToString("D");
            }
        }
        public string FEndDate
        {
            get
            {
                return EndDate.ToString("D");
            }
        }
    }
}
