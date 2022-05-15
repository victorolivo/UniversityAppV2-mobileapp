using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace UniversityAppV2.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumCus { get; set; }
        public string Status { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhoneNumber { get; set; }
        public string InstructorEmail { get; set; }
        public string Notes { get; set; } = "";
        public int TermId { get; set; }

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

        public string FInstructorPhoneNumber
        {
            get
            {
                long temp = Convert.ToInt64(InstructorPhoneNumber);
                return $"{temp:(###) ###-####}";
            }
        }
    }
}
