using System;
using UniversityAppV2.Models;
using UniversityAppV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Color color = (Color)Application.Current.Resources["defaultBackbuttonColor"];
            NavigationPage.SetIconColor(this, color);

            MainPage = new NavigationPage(new MainPage());
            //MainPage.BackgroundColor = Color.FromHex("#92b2fd");
            MainPage.BackgroundColor = Color.FromHex("#314f8b");

        }

        protected override void OnStart()
        {
            //Uncomment the following code to load Sample Data

            #region Sample Data (Run Once, the comment this section out)
            //Term sampleTerm = new Term
            //{
            //    Title = "Spring Term",
            //    StartDate = DateTime.Now,
            //    EndDate = DateTime.Now.AddMonths(6),
            //    TotalCUs = 0
            //};
            //TermsDB.AddTerm(sampleTerm);

            //Course sampleCourse = new Course
            //{
            //    Name = "C971",
            //    StartDate = DateTime.Now,
            //    EndDate = DateTime.Now.AddDays(30),
            //    NumCus = 3,
            //    Status = "In-Progress",
            //    InstructorName = "Victor Olivo",
            //    InstructorPhoneNumber = "7873596062",
            //    InstructorEmail = "volivo2@wgu.edu",
            //    Notes = "Mobile application development using Xamarin.Forms.",
            //    TermId = sampleTerm.Id
            //};
            //TermsDB.AddCourse(sampleCourse);
            //sampleTerm.TotalCUs = TermsDB.GetTotalCusForTerm(sampleTerm.Id);
            //TermsDB.UpdateTerm(sampleTerm);

            //Assessment sampleAssessment1 = new Assessment
            //{
            //    Name = "Xamarin Basics",
            //    DueDate = sampleCourse.EndDate,
            //    Type = "OA",
            //    CourseId = sampleCourse.Id
            //};
            //TermsDB.AddAssessment(sampleAssessment1);

            //Assessment sampleAssessment2 = new Assessment
            //{
            //    Name = "Mobile Application",
            //    DueDate = sampleCourse.EndDate,
            //    Type = "PA",
            //    CourseId = sampleCourse.Id
            //};
            //TermsDB.AddAssessment(sampleAssessment2);
            #endregion
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
