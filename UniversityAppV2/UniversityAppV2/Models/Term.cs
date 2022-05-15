using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityAppV2.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalCUs { get; set; }

        //Fomatted Dates
        public string FStartDate 
        {
            get
            {
                return StartDate.ToString("Y");
            }
        }
        public string FEndDate 
        {
            get
            {
                return EndDate.ToString("Y");
            }
        }
    }
}
