using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityAppV2.Models;
using UniversityAppV2.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsPage : ContentPage
    {
        public Course CurrentCourse { get; set; }

        //Initialize view according to the current course (selected course)
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

        //Trigger: User refreshes page (Pulls down page)
        //Action: Updates the assessments list
        private async void AssessmentsRefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            assessmentsList.ItemsSource = TermsDB.GetAllAssessmentsForCourse(CurrentCourse.Id);
            AssessmentsRefreshView.IsRefreshing = false;
        }

        //Trigger: Automatically called when user opens page
        //Action: Updates the assessments list; Keeps list up-to-date
        protected override void OnAppearing()
        {
            assessmentsList.ItemsSource = TermsDB.GetAllAssessmentsForCourse(CurrentCourse.Id);
            base.OnAppearing();
        }

        //Trigger: Event (User taps/selects an assessment)
        //Action: De-selects
        private void assessmentsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        //Trigger: Event (User wants to modify an assessment)
        //Action: Takes user to a new page to enter and save assessment details
        private void ModifyAssessment_Clicked(object sender, EventArgs e)
        {
            var assessment = ((MenuItem)sender).BindingContext as Assessment;
            Navigation.PushAsync(new AddModAssessment(true, CurrentCourse, assessment));
        }

        //Trigger: Event (User wants to delete an assessment)
        //Action: Deletes assessment from the database; Refreshes view
        private void DeleteAssessment_Clicked(object sender, EventArgs e)
        {
            var assessment = ((MenuItem)sender).BindingContext as Assessment;

            if (assessment.Type == "OA")
                CurrentCourse.OA = false;
            else
                CurrentCourse.PA = false;

            TermsDB.RemoveAssessment(assessment.Id);
            TermsDB.UpdateCourse(CurrentCourse);
            AssessmentsRefreshView_Refreshing(sender, e);
        }

        //Trigger: Event (User wants to add a new assessment)
        //Action: Takes user to a new page to enter and save new assessment details
        private void AddAssessmentBtn_Clicked(object sender, EventArgs e)
        {
            if(CurrentCourse.OA && CurrentCourse.PA)
            {
                DisplayAlert($"Maximum Reached", "There is a maximum of two assessments per course, one of each type.", "Ok");
            }
            else
            {
                Navigation.PushAsync(new AddModAssessment(false, CurrentCourse, null));
            }
        }

        //Trigger: Event (User wants to see current course notes)
        //Action: Shows the user the course notes and gives the option to share them
        private async void viewNotesBtn_Clicked(object sender, EventArgs e)
        {
            bool selection = await DisplayAlert($"{CurrentCourse.Name} Notes", CurrentCourse.Notes, "Ok", "Share Notes");
            if (!selection)
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = CurrentCourse.Notes,
                    Title = "Share Notes"
                });
            }
                
        }
    }
}