using Plugin.LocalNotifications;
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
    public partial class AddModAssessment : ContentPage
    {
        //Properties for data binding with xaml
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime DueDate { get; set; }
        public string Notification { get; set; }
        public Assessment CurrentAssessment { get; set; }
        public Course CurrentCourse { get; set; }
        public bool Modify { get; set; }

        //Modify view according to action: Creating New or Modifying Existing
        public AddModAssessment(bool modify, Course c, Assessment a)
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            BindingContext = this;
            CurrentAssessment = a;
            CurrentCourse = c;
            Modify = modify;
            DueDateField.MinimumDate = CurrentCourse.StartDate;
            DueDateField.MaximumDate = CurrentCourse.EndDate;
            DueDate = CurrentCourse.EndDate;

            //Check assesment type
            

            if (modify)
                prefillFields();
            else
                PageTitleLbl.Text = "Add Assessment";
        }

        //Trigger: internal
        //Action: Prefill Fields if in modify mode
        private void prefillFields()
        {
            PageTitleLbl.Text = "Modify Assessment";
            NameField.Text = CurrentAssessment.Name;

            if (CurrentAssessment.Type == "OA")
                CurrentCourse.OA = false;
            else
                CurrentCourse.PA = false;

            TypeField.SelectedItem = CurrentAssessment.Type;
            DueDateField.Date = CurrentAssessment.DueDate;
            NotificationField.SelectedItem = "None";
            Notification = "None";
        }

        protected override bool OnBackButtonPressed()
        {
            if (Modify)
            {
                if (CurrentAssessment.Type == "OA")
                    CurrentCourse.OA = true;
                else
                    CurrentCourse.PA = true;
            }
            
            return false;
            
        }

        //Trigger: User attempts to submit
        //Action: If valid, performs appropiate action according to mode: Creates New or Saves changes made to database
        private void SubmitBtn_Clicked(object sender, EventArgs e)
        {
            if (FormValid())
            {
                if (Modify)
                {
                    CurrentAssessment.Name = Name;
                    CurrentAssessment.Type = Type;
                    CurrentAssessment.DueDate = DueDate;

                    if (CurrentAssessment.Type == "OA")
                        CurrentCourse.OA = true;
                    else
                        CurrentCourse.PA = true;

                    TermsDB.UpdateAssessment(CurrentAssessment);
                    TermsDB.UpdateCourse(CurrentCourse);
                }
                else
                {
                    Assessment a = new Assessment
                    {
                        Name = this.Name,
                        DueDate = this.DueDate,
                        Type = this.Type,
                        CourseId = CurrentCourse.Id
                    };

                    if (a.Type == "OA")
                        CurrentCourse.OA = true;
                    else
                        CurrentCourse.PA = true;

                    TermsDB.AddAssessment(a);
                    TermsDB.UpdateCourse(CurrentCourse);
                }

                Navigation.PopAsync();
            }
        }

        //Trigger: internal
        //Action: Input validation
        private bool FormValid()
        {
            //Input Validations
            if (string.IsNullOrWhiteSpace(Name))
            {
                App.Current.MainPage.DisplayAlert("Invalid Assessment Name", "Please enter a name for the assessment", "Ok");
                return false;
            }
            else if (Type == null)
            {
                App.Current.MainPage.DisplayAlert("Invalid Assessment Type", "Assessment type was not selected or there is already an assessment of this type for this course", "Ok");
                return false;
            }
            else if (Notification == null)
            {
                App.Current.MainPage.DisplayAlert("Invalid Notification", "Please select a timespan for a notification (reminder) to be set or select none", "Ok");
                return false;
            }

            CreateNotification();
            return true;
        }

        //Trigger: internal; When form is being submitted
        //Action: Creates notification
        private void CreateNotification()
        {
            DateTime notificationDate = DueDate;
            string selection = Notification;

            switch (Notification)
            {
                case "5 Seconds From Now (Test)":
                    CrossLocalNotifications.Current.Cancel(0);
                    CrossLocalNotifications.Current.Show("Assessment Due Soon", $"An assessment from {CurrentCourse.Name} is due {DueDate.ToString("D")}. You got this!", 0, DateTime.Now.AddSeconds(5));
                    break;
                case "On Due Date":
                    CrossLocalNotifications.Current.Cancel(1);
                    CrossLocalNotifications.Current.Show("Assessment Due Soon", $"An assessment from {CurrentCourse.Name} is due today. You got this!", 1, notificationDate);
                    break;
                case "1 Day Before Due Date":
                    TimeSpan spanOneDay = TimeSpan.FromDays(1);
                    notificationDate = DueDate.Subtract(spanOneDay);
                    CrossLocalNotifications.Current.Cancel(2);
                    CrossLocalNotifications.Current.Show("Assessment Due Soon", $"An assessment from {CurrentCourse.Name} is due tomorrow. You got this!", 2, notificationDate);
                    break;
                case "3 Days Before Due Date":
                    TimeSpan spanThreeDays = TimeSpan.FromDays(3);
                    notificationDate = DueDate.Subtract(spanThreeDays);
                    CrossLocalNotifications.Current.Cancel(3);
                    CrossLocalNotifications.Current.Show("Assessment Due Soon", $"An assessment from {CurrentCourse.Name} is due in three days. You got this!", 3, notificationDate);
                    break;
                case "1 Week Before Due Date":
                    TimeSpan spanOneWeek = TimeSpan.FromDays(7);
                    notificationDate = DueDate.Subtract(spanOneWeek);
                    CrossLocalNotifications.Current.Cancel(4);
                    CrossLocalNotifications.Current.Show("Assessment Due Soon", $"An assessment from {CurrentCourse.Name} is due in a week. You got this!", 4, notificationDate);
                    break;
                case "2 Weeks Before Due Date":
                    TimeSpan spanTwoWeeks = TimeSpan.FromDays(14);
                    notificationDate = DueDate.Subtract(spanTwoWeeks);
                    CrossLocalNotifications.Current.Cancel(5);
                    CrossLocalNotifications.Current.Show("Assessment Due Soon", $"An assessment from {CurrentCourse.Name} is due in two weeks. You got this!", 5, notificationDate);
                    break;
                default:
                    break;
            }
            
        }

        private void TypeField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeField == null)
                return;
            else if(TypeField.SelectedItem.ToString() == "OA" && CurrentCourse.OA)
            {
                DisplayAlert("Invalid Assessment Type", "An assessment of this type already exist for this course", "Ok");
                Type = null;
                TypeField.BackgroundColor = Color.Salmon;
            }
            else if(TypeField.SelectedItem.ToString() == "PA" && CurrentCourse.PA)
            {
                DisplayAlert("Invalid Assessment Type", "An assessment of this type already exist for this course", "Ok");
                Type = null;
                TypeField.BackgroundColor = Color.Salmon;
            }
            else
            {
                TypeField.BackgroundColor = Color.Default;
            }
            
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
            Navigation.PopAsync();
        }
    }
}