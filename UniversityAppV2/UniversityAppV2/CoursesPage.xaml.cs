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

        public CoursesPage(Term t)
        {
            InitializeComponent();
            CurrentTerm = t;
            TermTitleLbl.Text = CurrentTerm.Title;
            StartDateLbl.Text = CurrentTerm.FStartDate;
            EndDateLbl.Text = CurrentTerm.FEndDate;
            coursesList.ItemsSource = TermsDB.GetAllCoursesForTerm(CurrentTerm.Id);
        }

        private async void coursesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var course = ((ListView)sender).SelectedItem as Course;
            if (course == null)
                return;

            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(new AssessmentsPage(course));
        }

        private void AddCourseBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddModCourse(false, CurrentTerm, null));
        }


        private void ModifyCourse_Clicked(object sender, EventArgs e)
        {
            var course = ((MenuItem)sender).BindingContext as Course;
            Navigation.PushAsync(new AddModCourse(true, CurrentTerm, course));
        }

        private void DeleteCourse_Clicked(object sender, EventArgs e)
        {
            var course = ((MenuItem)sender).BindingContext as Course;

            TermsDB.RemoveCourse(course.Id);

            CurrentTerm.TotalCUs = TermsDB.GetTotalCusForTerm(CurrentTerm.Id);

            TermsDB.UpdateTerm(CurrentTerm);

            CourseRefreshView_Refreshing(sender, e);
            new MainPage();
        }

        private async void CourseRefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            coursesList.ItemsSource = TermsDB.GetAllCoursesForTerm(CurrentTerm.Id);
            CourseRefreshView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            coursesList.ItemsSource = TermsDB.GetAllCoursesForTerm(CurrentTerm.Id);
            base.OnAppearing();
        }
    }
}