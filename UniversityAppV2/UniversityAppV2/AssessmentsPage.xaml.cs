using System;
using System.Collections.Generic;
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
    public partial class AssessmentsPage : ContentPage
    {
        public Course CurrentCourse { get; set; }

        public AssessmentsPage(Course course)
        {
            InitializeComponent();
            CurrentCourse = course;
            instructorNameLbl.Text = CurrentCourse.InstructorName;
            instructorPhoneLbl.Text = CurrentCourse.FInstructorPhoneNumber;
            instructorEmailLbl.Text = CurrentCourse.InstructorEmail;
            courseNameLbl.Text = CurrentCourse.Name;
            courseDueDateLbl.Text = "Deadline: \n" + CurrentCourse.FEndDate;
        }

        private async void AssessmentsRefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            assessmentsList.ItemsSource = TermsDB.GetAllAssessmentsForCourse(CurrentCourse.Id);
            AssessmentsRefreshView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            assessmentsList.ItemsSource = TermsDB.GetAllAssessmentsForCourse(CurrentCourse.Id);
            base.OnAppearing();
        }

        private void assessmentsList_ItemTapped(object sender, ItemTappedEventArgs e)
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
            var assessment = ((MenuItem)sender).BindingContext as Assessment;

            TermsDB.RemoveAssessment(assessment.Id);

            AssessmentsRefreshView_Refreshing(sender, e);
        }

        private void AddAssessmentBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddModAssessment(false, CurrentCourse, null));
        }

        private void viewNotesBtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.DisplayAlert($"{CurrentCourse.Name} Notes", CurrentCourse.Notes, "Ok");
        }
    }
}