using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityAppV2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsPage : ContentPage
    {
        public Course CurrentCourse { get; set; }

        public AssessmentsPage(Course course)
        {
            InitializeComponent();
            CurrentCourse = course;
            instructorNameLbl.Text = CurrentCourse.InstructorName;
            instructorPhoneLbl.Text = CurrentCourse.InstructorPhoneNumber;
            instructorEmailLbl.Text = CurrentCourse.InstructorEmail;
            courseNameLbl.Text = CurrentCourse.Name;
            courseDueDateLbl.Text = "Deadline: " + CurrentCourse.FEndDate;
        }

        private async void AssessmentsRefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            //db update
            AssessmentsRefreshView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            //db update ex. coursesList.ItemsSource = TermsDB.GetAllCoursesForTerm(CurrentTerm.Id);
            base.OnAppearing();
        }

        private async void assessmentsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ModifyAssessment_Clicked(object sender, EventArgs e)
        {
            var assessment = ((MenuItem)sender).BindingContext as Assessment;
            Navigation.PushAsync(new AddModAssessment(true, CurrentCourse, assessment));
        }

        private void DeleteAssessment_Clicked(object sender, EventArgs e)
        {

        }

        private void AddAssessmentBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void viewNotesBtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.DisplayAlert($"{CurrentCourse.Name} Notes", CurrentCourse.Notes, "Ok");
        }
    }
}