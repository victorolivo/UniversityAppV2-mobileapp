using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniversityAppV2.Models;
using UniversityAppV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddModCourse : ContentPage
    {
        //Properties for data binding with xaml
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumOfCus { get; set; }
        public string Status { get; set; }
        public string Notification { get; set; }
        public string Notes { get; set; } = "";

        //Intructor
        public string InstructorName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        public int TermId { get; set; }
        public Term CurrentTerm { get; set; }
        public bool Modify { get; set; }
        public Course CurrentCourse { get; set; }

        //Modify view according to action: Creating New or Modifying Existing
        public AddModCourse(bool modify, Term t, Course c)
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            BindingContext = this;
            CurrentTerm = t;
            CurrentCourse = c;
            Modify = modify;
            StartDateField.MinimumDate = CurrentTerm.StartDate;
            StartDateField.MaximumDate = CurrentTerm.EndDate;
            EndDateField.MinimumDate = CurrentTerm.StartDate;
            EndDateField.MaximumDate = CurrentTerm.EndDate;

            if (Modify)
                PrefillTxtFields();
            else
                PageTitleLbl.Text = "New Course";
        }

        //Trigger: internal
        //Action: Prefill Fields if in modify mode
        private void PrefillTxtFields()
        {
            PageTitleLbl.Text = "Modify Course";
            NameField.Text = CurrentCourse.Name;
            StartDateField.Date = CurrentCourse.StartDate;
            EndDateField.Date = CurrentCourse.EndDate;
            NumCusField.SelectedItem = CurrentCourse.NumCus;
            StatusField.SelectedItem = CurrentCourse.Status;
            InstrucNameField.Text = CurrentCourse.InstructorName;
            InstrucPhoneField.Text = CurrentCourse.InstructorPhoneNumber;
            InstrucEmailField.Text = CurrentCourse.InstructorEmail;
            NotesField.Text = CurrentCourse.Notes;

        }

        //Trigger: User attempts to submit
        //Action: If valid, performs appropiate action according to mode: Creates New or Saves changes made to database
        private void SubmitBtn_Clicked(object sender, EventArgs e)
        {
            if (FormValid())
            {
                if (Modify)
                {
                    CurrentCourse.Name = CourseName;
                    CurrentCourse.StartDate = StartDate;
                    CurrentCourse.EndDate = EndDate;
                    CurrentCourse.NumCus = NumOfCus;
                    CurrentCourse.Status = Status;
                    CurrentCourse.InstructorName = InstructorName;
                    CurrentCourse.InstructorPhoneNumber = Phone;
                    CurrentCourse.InstructorEmail = Email;
                    CurrentCourse.Notes = Notes;

                    TermsDB.UpdateCourse(CurrentCourse);
                }
                else
                {
                    Course newCourse = new Course
                    {
                        Name = CourseName,
                        StartDate = this.StartDate,
                        EndDate = this.EndDate,
                        NumCus = NumOfCus,
                        Status = this.Status,
                        InstructorName = this.InstructorName,
                        InstructorPhoneNumber = Phone,
                        InstructorEmail = Email,
                        Notes = this.Notes,
                        TermId = CurrentTerm.Id
                    };

                    TermsDB.AddCourse(newCourse);
                }


                CurrentTerm.TotalCUs = TermsDB.GetTotalCusForTerm(CurrentTerm.Id);

                TermsDB.UpdateTerm(CurrentTerm);

                Navigation.PopAsync();
            }
        }

        //Trigger: internal
        //Action: Input validation
        private bool FormValid()
        {
            //for email validation
            Regex email = new Regex("^\\S+@\\S+\\.\\S+$");

            //Input Validations
            if (string.IsNullOrWhiteSpace(CourseName))
            {
                App.Current.MainPage.DisplayAlert("Invalid Course Name", "Please enter a name for the course", "Ok");
                return false;
            }
            else if (StartDateField.Date > EndDateField.Date)
            {
                App.Current.MainPage.DisplayAlert("Invalid Dates", "Start Date is greater than End Date. Time travel is not yet possible", "Ok");
                return false;
            }
            else if(NumOfCus <= 0 || NumOfCus > 6)
            {
                App.Current.MainPage.DisplayAlert("Invalid CUs Selection", "Please pick a number of credits for this course from the provided list(picker)", "Ok");
                return false;
            }
            else if (Status == null)
            {
                App.Current.MainPage.DisplayAlert("Invalid Course Status", "Please select a status for this course", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(InstructorName))
            {
                App.Current.MainPage.DisplayAlert("Invalid Instructor Name", "Please enter a name for the instructor of this course", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Phone) || Phone.Length != 10)
            {
                App.Current.MainPage.DisplayAlert("Invalid Phone Number", "The phone number provided is not valid, must be a 10-digit phone number (All numbers). Avoid Country code and any special characters.", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Email))
            {
                App.Current.MainPage.DisplayAlert("Invalid Email", "Please enter the course instructor's email address", "Ok");
                return false;
            }
            else if (!email.IsMatch(Email))
            {
                App.Current.MainPage.DisplayAlert("Invalid Email", "The email provded is not valid, make sure to include the @ symbol and a domain (ex .com)", "Ok");
                return false;
            }
            else if (Notification == null)
            {
                App.Current.MainPage.DisplayAlert("Invalid Notification", "Please select a timespan for a notification (reminder) to be set or select none", "Ok");
                return false;
            }

            if (Notes == "")
                Notes = "None provided";

            CreateNotification();
            return true;
        }

        //Trigger: internal; When form is being submitted
        //Action: Creates notification
        private void CreateNotification()
        {
            DateTime notificationDate = EndDate;
            string selection = Notification;

            switch (Notification)
            {
                case "5 Seconds From Now (Test)":
                    CrossLocalNotifications.Current.Cancel(0);
                    CrossLocalNotifications.Current.Show("Course End Date Approaching", $"A course from {CurrentTerm.Title} is due {EndDate.ToString("D")}. You got this!", 0, DateTime.Now.AddSeconds(5));
                    break;
                case "On Due Date":
                    CrossLocalNotifications.Current.Cancel(1);
                    CrossLocalNotifications.Current.Show("Course End Date Approaching", $"A course from {CurrentTerm.Title} is due today. You got this!", 1, notificationDate);
                    break;
                case "1 Day Before Due Date":
                    TimeSpan spanOneDay = TimeSpan.FromDays(1);
                    notificationDate = EndDate.Subtract(spanOneDay);
                    CrossLocalNotifications.Current.Cancel(2);
                    CrossLocalNotifications.Current.Show("Course End Date Approaching", $"A course from {CurrentTerm.Title} is due tomorrow. You got this!", 2, notificationDate);
                    break;
                case "3 Days Before Due Date":
                    TimeSpan spanThreeDays = TimeSpan.FromDays(3);
                    notificationDate = EndDate.Subtract(spanThreeDays);
                    CrossLocalNotifications.Current.Cancel(3);
                    CrossLocalNotifications.Current.Show("Course End Date Approaching", $"A course from {CurrentTerm.Title} is due in three days. You got this!", 3, notificationDate);
                    break;
                case "1 Week Before Due Date":
                    TimeSpan spanOneWeek = TimeSpan.FromDays(7);
                    notificationDate = EndDate.Subtract(spanOneWeek);
                    CrossLocalNotifications.Current.Cancel(4);
                    CrossLocalNotifications.Current.Show("Course End Date Approaching", $"A course from {CurrentTerm.Title} is due in a week. You got this!", 4, notificationDate);
                    break;
                case "2 Weeks Before Due Date":
                    TimeSpan spanTwoWeeks = TimeSpan.FromDays(14);
                    notificationDate = EndDate.Subtract(spanTwoWeeks);
                    CrossLocalNotifications.Current.Cancel(5);
                    CrossLocalNotifications.Current.Show("Course End Date Approaching", $"A course from {CurrentTerm.Title} is due in two weeks. You got this!", 5, notificationDate);
                    break;
                default:
                    break;
            }

        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}