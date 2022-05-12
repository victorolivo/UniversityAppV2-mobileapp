using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniversityAppV2.Models;

namespace UniversityAppV2.Services
{
    internal class TermsDB
    {
        static SQLiteConnection db;

        static void Init()
        {
            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Terms.db");

            db = new SQLiteConnection(databasePath);
            _ = db.CreateTable<Term>();
            _ = db.CreateTable<Course>();
            _ = db.CreateTable<Assessment>();
        }


        //DB Add methods
        public static void AddTerm(Term t)
        {
            Init();
            _ = db.Insert(t);
        }

        public static void AddCourse(Course c)
        {
            Init();
            _ = db.Insert(c);
        }

        public static void AddAssessment(Assessment a)
        {
            Init();
            _ = db.Insert(a);
        }


        //DB Remove methods
        public static void RemoveTerm(int id)
        {
            Init();

            IEnumerable<Course> coursesInTerm = GetAllCoursesForTerm(id);

            if(coursesInTerm != null)
            {
                foreach (var c in coursesInTerm)
                {
                    _ = db.Delete<Course>(c.Id);
                }
            }

            _ = db.Delete<Term>(id);
        }

        public static void RemoveCourse(int id)
        {
            Init();

            IEnumerable<Assessment> assessmentsInCourse = GetAllAssessmentsForCourse(id);

            if (assessmentsInCourse != null)
            {
                foreach (var a in assessmentsInCourse)
                {
                    _ = db.Delete<Assessment>(a.Id);
                }
            }

            _ = db.Delete<Course>(id);
        }

        public static void RemoveAssessment(int id)
        {
            Init();
            _ = db.Delete<Assessment>(id);
        }


        //DB Get methods
        public static IEnumerable<Term> GetAllTerms()
        {
            Init();
            var terms = db.Table<Term>().ToList();
            return terms;
        }

        public static IEnumerable<Course> GetAllCoursesForTerm(int termId)
        {
            Init();
            var courses = (from c in db.Table<Course>()
                           where c.TermId == termId
                           select c).ToList();
            return courses;
        }

        public static IEnumerable<Assessment> GetAllAssessmentsForCourse(int courseId)
        {
            Init();
            var assessments = (from a in db.Table<Assessment>()
                           where a.CourseId == courseId
                           select a).ToList();
            return assessments;
        }

        public static int GetTotalCusForTerm(int termId)
        {
            Init();
            int totalCus = 0;
            IEnumerable<Course> courseCuCount = GetAllCoursesForTerm(termId);
            
            foreach(var c in courseCuCount)
            {
                totalCus += c.NumCus;
            }

            return totalCus;
        }


        //DB Update methods
        public static void UpdateTerm(Term t)
        {
            Init();
            _ = db.Update(t);
        }

        public static void UpdateCourse(Course c)
        {
            Init();
            _ = db.Update(c);
        }

        public static void UpdateAssessment(Assessment a)
        {
            Init();
            _ = db.Update(a);
        }

    }
}
