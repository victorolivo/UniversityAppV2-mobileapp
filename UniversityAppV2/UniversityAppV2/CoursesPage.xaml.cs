using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityAppV2.Models;
using UniversityAppV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesPage : ContentPage
    {
        public Term CurrentTerm { get; set; }

        //Initialize view according to the current term (selected term)
        public CoursesPage(Term t)
        {
            InitializeComponent();
            CurrentTerm = t;
            TermTitleLbl.Text = CurrentTerm.Title;
            StartDateLbl.Text = CurrentTerm.FStartDate;
            EndDateLbl.Text = CurrentTerm.FEndDate;
            coursesList.ItemsSource = TermsDB.GetAllCoursesForTerm(CurrentTerm.Id);
        }

        //Trigger: Event (User taps/selects a course)
        //Action: Takes user inside the selected course (Course details/Assessments Page)
        private async void coursesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var course = ((ListView)sender).SelectedItem as Course;
            if (course == null)
                return;

            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(new AssessmentsPage(course));
        }

        //Trigger: Event (User wants to add a new course)
        //Action: Takes user to a new page to enter and save new course details
        private void AddCourseBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddModCourse(false, CurrentTerm, null));
        }

        //Trigger: Event (User wants to modify a course)
        //Action: Takes user to a new page to enter and save course details
        private void ModifyCourse_Clicked(object sender, EventArgs e)
        {
            var course = ((MenuItem)sender).BindingContext as Course;
            Navigation.PushAsync(new AddModCourse(true, CurrentTerm, course));
        }

        //Trigger: Event (User wants to delete a course)
        //Action: Deletes course from the database; Updates course's term; Refreshes view
        private void DeleteCourse_Clicked(object sender, EventArgs e)
        {
            var course = ((MenuItem)sender).BindingContext as Course;

            TermsDB.RemoveCourse(course.Id);

            CurrentTerm.TotalCUs = TermsDB.GetTotalCusForTerm(CurrentTerm.Id);

            TermsDB.UpdateTerm(CurrentTerm);

            CourseRefreshView_Refreshing(sender, e);
        }

        //Trigger: User refreshes page (Pulls down page)
        //Action: Updates the courses list
        private async void CourseRefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            coursesList.ItemsSource = TermsDB.GetAllCoursesForTerm(CurrentTerm.Id);
            CourseRefreshView.IsRefreshing = false;
        }

        //Trigger: Automatically called when user opens page
        //Action: Updates the courses list; Keeps list up-to-date
        protected override void OnAppearing()
        {
            coursesList.ItemsSource = TermsDB.GetAllCoursesForTerm(CurrentTerm.Id);
            base.OnAppearing();
        }
    }
}